# Level-Specific Flipper / Spike ROM Analysis

Date: 2026-11-06
Scope: Read-only analysis of Atari Tempest Flipper movement and Spike interaction for displayed Levels 1-9.

## Executive conclusion

The ROM selects a Flipper movement-program entry from a level-indexed table; movement is not governed by one universal or random below-rim rule. The 16-level table contains six distinct entry offsets, of which five are used on Levels 1-9. Displayed Levels 5 and 9 both select `$53`, the only program among those nine that tests the current lane's live Spike height. Levels 4, 6, 7, and 8 have nonzero starting Spike heights but select programs that do not test Spikes. Thus Spike presence alone does not determine the lane-change behavior.

Program `$53` has a Spike-dependent lane-stay effect, but no explicit lock/capture flag or separate locked state. It keeps moving up the current lane; if the current lane's Spike predicate is false, it loops without starting a lane change. Once the predicate is true, it starts the normal flip. This is a control-flow condition in one program, not a random capture decision.

The ROM's Level 7 `$24` script remains in conflict with the manually accepted arcade-observed and C# `2-3-2-2-2` route. The ROM listing traces `$24` as two completed flips, reverse, three completed flips, reverse, and repeat. Neither finding should be silently substituted for the other.

No production code, tests, durable behavior specifications, or references were changed. No game, build, or tests were run.

## Evidence and limits

- **ROM-BACKED** findings below come from the checked-in [Tempest ROM source transcription](../../Reference/arcade/Tempest%20Rom%20Source%20code.md), which is the designated arcade authority for this analysis.
- **REFERENCE VERIFIED** findings come from [Incitti's source](../../Reference/incitti/tempest.bmx) and [Original Arcade levelset data](../../Reference/incitti/levelsets/levelset1.txt). They are secondary engineering/board references, not proof of ROM behavior.
- **CURRENT C#** describes this repository's implementation and tests only.
- The task's Level 5/9 lock and Level 6-8 crossing observations are treated as hypotheses, as requested. No independent arcade recording or new manual test was available for them. The already-recorded Level 7 manual observation remains scoped to what Neal accepted.
- The ROM file is a disassembly/transcription, not an independently inspected ROM binary. P-code instructions are explicitly listed as bytes; several parameter/geometry tables are `.chunk` declarations whose values are represented in comments. This analysis confirms the mapping against that checked-in listing, not against a separate binary dump.
- ROM `lev_x`, `lev_y`, `lev_angle`, `lev_remap`, and `lev_open` tables are omitted `.chunk` data. Therefore the exact ROM geometry name/remap, physical screen handedness, and per-board open-edge flags cannot be established from this source. Incitti's level names below are labeled secondary. The ROM `rev_if_edge` handler does reverse direction at an open endpoint when `open_level` is set, but the omitted table prevents assigning that behavior to a specific ROM level here.

## Level selection, tube identity, and initial Spike state

The level-parameter routine derives an effective table level from zero-based `curlevel` (`curlevel + 1` for the displayed level), then evaluates level tables in a 16-level block. `flipper_move` is a table selected this way, not by enemy origin, player position, board shape, difficulty, or a per-Flipper random roll. The first nine table entries are:

| Displayed level | Secondary Incitti board name | ROM movement entry | ROM starting `wave_spikeht` |
|---:|---|---:|---:|
| 1 | Circle | `$07` | `$00` |
| 2 | Square | `$0b` | `$00` |
| 3 | Plus | `$19` | `$00` |
| 4 | Binoculars | `$24` | `$e0` |
| 5 | Cross | `$53` | `$d8` |
| 6 | Triangle | `$0b` | `$d4` |
| 7 | Clover | `$24` | `$d0` |
| 8 | Vee | `$19` | `$c8` |
| 9 | Steps | `$53` | `$c0` |

The board names come from Incitti's ordered levelset only; they are not independently confirmed by the omitted ROM remap/geometry tables. ROM Spike values are level-table values copied to all 16 `spike_ht` entries at wave initialization. Levels 1-3 start with no Spikes; Levels 4-9 start with nonzero Spike heights. `flipper_move` repeats by level block modulo 16. The full 16-entry table has six distinct offsets (`$07`, `$0b`, `$19`, `$24`, `$53`, `$87`); only the first five appear on Levels 1-9. `$87` is outside the requested level range.

Sources: [effective level and table evaluation, `$92c5-$9326`](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L2547), [Spike and Flipper parameter tables](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L2833), [table-index rules](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L2934), [Spike initialization, `$9234-$9245`](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L2471), [secondary board names](../../Reference/incitti/levelsets/levelset1.txt#L1).

## P-code execution and the five Level 1-9 programs

The enemy loop loads the Flipper's saved movement PC, reads instructions from the p-code byte region, dispatches through the opcode jump table, and saves the PC when an instruction halts that tick. Relevant executor behavior is:

| Opcode | Effect relevant to these routes |
|---:|---|
| `$00` | Halt this enemy's p-code for the current tick. |
| `$02` | Load an immediate value into the enemy's `$0298` route counter. |
| `$04` | If scratch `$010c` is zero, skip the following branch instruction and operand. |
| `$06` | Branch to the immediate p-code offset. |
| `$08` | Decrement `$0298`; branch while nonzero, otherwise continue past its operand. |
| `$0c` | Move along the tube at the current Flipper speed; normal top-of-tube handling is separate. |
| `$12` | Start a flip; it also applies `rev_if_edge` when `open_level` is set. |
| `$14` | Continue/complete the flip and set scratch according to whether the flip remains active. |
| `$16` | Toggle direction bit `$40`. |
| `$1a` | Branch to its target when scratch `$010c` is zero. |
| `$1c` | Set scratch `$010c` from the current segment's Spike-height comparison. |

The dispatcher and handlers are at [`$9ba2-$9eaa`](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3565); the movement programs are at [`$a0fe-$a157`](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L4248).

| Entry | Trace and route meaning |
|---:|---|
| `$07` | Repeatedly move along the current lane. No below-rim lane changes. |
| `$0b` | Move eight times, start a flip, finish it without moving along during the turn, then repeat. No Spike test. |
| `$19` | Repeated flips with one along-move between flip continuation ticks; the script describes a flip each cycle with one movement tick between flips. No Spike test. |
| `$24` | Lead-in move; complete two flips in one direction; reverse; complete three; reverse; repeat. Along-movement occurs on continuation ticks, not the flip-start tick. No Spike test. |
| `$53` | Move, test the current lane's Spike, and loop in place in the program if not yet above its tip. If above, start a flip; move during the turn and continue until the flip is complete, then return to the move-and-test sequence. No explicit counter or random decision. |

### Controlled instruction traces

The offsets below are the table-relative p-code offsets in the ROM comments; the byte region starts at `$a0f7`. Each `00` yields the enemy's tick, so the next p-code action resumes on a later update.

- **`$07`**: `07: 0c` move; `08: 00` halt; `09: 06 07` branch to the move. Repeat without a lateral flip.
- **`$0b`**: `0b: 02 08` load eight; `0d: 0c` move; `0e: 00` halt; `0f: 08 0c` decrement and repeat the move eight times; `11: 12` start a flip; `12: 00` halt; `13: 14` continue it. While scratch is nonzero, `14: 04` does not skip and `15: 06 11` repeats the continuation. At completion scratch is zero, so `$04` skips that branch and the `17: 06 0a` branch returns to the counter setup. No `$0c` is executed during the flip.
- **`$19`**: `19: 0c` move; `1a: 00` halt; `1b: 12` start a flip; `1c: 00` halt; then `1d: 14` continue and `1e: 0c` move on each continuation tick. If still flipping, `1f: 04` falls through to `20: 06 1b`; when complete it skips that branch and `22: 06 18` returns to the entry move. There is no Spike predicate.
- **`$24`**: `24: 0c` lead-in move; `26: 02 02` load two; `28: 12` start flip. Each continuation runs `2a: 14`, `2b: 0c`; when that flip completes, `30: 08 27` counts it. After the second, `32: 16` reverses, `33: 02 03` loads three, and `35: 12` starts the next block. `3d: 08 34` counts the three; `3f: 16` reverses and `40: 06 23` returns to the lead-in. The two/three counters count completed flips, not along-movement ticks.
- **`$53`**: `53: 00` halt; `54: 0c` move; `55: 1c` test; `56: 1a 53` returns to the halt if the Spike condition is false, otherwise `58: 12` starts a flip. After the halt, `5a: 0c` moves and `5b: 14` continues the flip. If still active, scratch is nonzero and `5c: 1a 52` falls through to `5e: 00`, then `5f: 06 5a` repeats the continuation. On completion scratch becomes zero, so it branches to the halt at `53`; the next update moves and retests before another flip. Thus the program checks the new lane only after the completed flip and the next along-move.

`$24` does not encode `2-3-2-2-2`; its explicit repeating counter pattern is `2, reverse, 3, reverse`. The p-code route counters are per enemy. For non-open boards the initial direction bit is clear and the script's `$16` instructions control reversal where present. The source also has deterministic endpoint reversal for open boards; because `lev_open` values are omitted, this report does not assign physical endpoint behavior to each level or claim screen-left/right handedness.

## What the `$53` Spike test actually does

Opcode `$1c` indexes `spike_ht` by the Flipper's current `enemy_seg`. A zero entry is replaced by `$ff`; otherwise the stored Spike height is compared with `enemy_along`. The condition is true when `spike_ht >= enemy_along`, including equality. The source labels this as “enemy is above the end of its segment's spike.”

For `$53`, the observable state transitions are:

1. Move along the current lane.
2. Read that lane's current Spike height.
3. If the condition is false, branch back to the program's halt and keep climbing the same lane on later ticks. No flip starts.
4. If true, start a flip and continue the flip while moving along.
5. Once the flip completes, return to the move-and-test loop; the new current segment is tested after the next along-move.

Consequences and edge cases:

- Starting on a Spike lane and entering a Spike lane use the same test; it reads only the current segment. A neighboring lane is not checked during the turn.
- Below the tip, the false condition suppresses lane changes while movement continues; it does not pause the Flipper or use a separate locked mode.
- At the tip, equality satisfies the predicate. Above the tip, the flip is allowed.
- A missing/destroyed Spike (`spike_ht == 0`) becomes `$ff`, so the flip condition is allowed. A partially damaged Spike's live `spike_ht` is used, so its shortened tip changes when the condition becomes true.
- The predicate is reevaluated from current Spike state after each completed flip; it is not latched at spawn and does not change the selected p-code program.
- The Flipper reads `spike_ht`, not a visual Spike animation/state flag. The player-shot path updates that stored height when damaging a Spike.

This is the ROM basis for describing a Spike “lane lock” on Levels 5 and 9, with an important qualification: the lock is a repeated conditional lane-stay effect until the current tip has been passed, not an explicit capture state or a guarantee of staying to the rim. Empty or shortened Spike state can permit earlier flips. The movement program itself contains no random branch. The current lane's live Spike state and start lane can make the visible route differ between Flippers.

For a concrete Level 5 comparison, with the initial `spike_ht = $d8`, a current `enemy_along = $e0` makes the predicate false (`$d8 < $e0`), while equality at `$d8` makes it true. If a partial hit changes the stored height to `$e0`, the same `$e0` along value now satisfies the predicate. If the Spike is destroyed and stored as zero, the `$ff` substitute satisfies it for normal along values. These are comparator outcomes, not claims about a measured number of arcade ticks.

Sources: [Spike predicate, `$9c22-$9c3a`](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3678), [`$53` program, `$a14a-$a157`](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L4329), [Spike damage path, `$a19e` onward](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L4420).

## Ordinary Flippers versus Tanker cargo

Ordinary Flippers use the selected level-table entry. Tanker cargo is a separate creation route and can select a different entry based on release depth:

- The selector at `$9b07` uses the saved along coordinate. For a release below `$20` (near the rim), it selects the default `$07` “just move up” program. At or beyond `$20`, it selects the level's `flipper_move` entry.
- Natural Tanker release occurs at the rim boundary, so its Flipper cargo uses `$07`, even on Levels 5 and 9.
- Player-shot Tanker destruction calls the same child setup while preserving the Tanker's current along coordinate. Deep destruction (at or beyond `$20`) gives the children the level-selected program: `$53` on Levels 5/9, `$0b` on Level 6, `$24` on Level 7, and `$19` on Level 8.
- The two children are allocated on opposite adjacent sides with opposite direction bits. Their lane and direction differ, but a deep child uses the same level program and current-lane Spike test as an ordinary Flipper.
- Therefore “Level 5/9 cargo always locks” is too broad: near-rim/natural cargo uses straight `$07`; deep player-shot cargo uses `$53`. The same release-depth distinction applies to other level scripts.

Sources: [depth-based selector, `$9b07-$9b1d`](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3542), [Tanker child allocation, `$a06f-$a0f6`](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L4179), [player-shot destruction call sites](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L4609).

## Randomness and route determinism

Program selection is deterministic for the effective displayed level. None of the five programs used on Levels 1-9 contains a random opcode; `$53` is conditional on live `spike_ht`, not a random roll. Ordinary Flippers start with the ROM's default direction bit clear. `$24` changes direction at its explicit count boundaries; open-endpoint correction is deterministic when enabled. Tanker children start on distinct sides and have opposite direction bits.

Random enemy/species choice, pending spawn lane, spawn timing, and Spike damage can affect which Flippers appear and their starting/state-dependent visible route. Those inputs do not randomly select a different ordinary Flipper movement program. Difficulty changes other level parameters, including movement speed, but the inspected program selector is level-indexed. Death/re-entry creates fresh pending supply rather than resuming an old Flipper PC; a newly selected Flipper starts with its current level's program. It does not resume a prior phase.

## ROM, Incitti, and current C# compared

### Incitti (secondary reference)

Incitti uses a generic `canflip`/Spike state machine rather than a ROM-style per-level movement-program table. Its climbing Flipper checks its current `OnEdge.Spike`; below that Spike's height it continues climbing in a sliding state, and reaching/passing the tip leads to a pause and lane move. After a move it checks the new lane's Spike and can resume sliding there. It recalculates the player-relative direction while in the sliding state. This can create a visually similar Spike-lane stay on more than one board, but it does not establish ROM behavior and does not explain why ROM Levels 5/9 select `$53` while Levels 6-8 do not.

Source: [Incitti Flipper update, lines 2732-2855](../../Reference/incitti/tempest.bmx#L2732).

### Current C#

- `Flipper.Update` contains no Spike-state read; the repository search found no `Spike` reference in [Flipper.cs](../../src/Tempest.Game/Gameplay/Flipper.cs).
- C# Levels 5 and 6 have Spikes and generic `CanFlip=true` configuration, but those Spike heights do not affect below-rim Flipper routing. Level 7 opts into the separate accepted observed-route implementation. Its `2-3-2-2-2` pattern and deep player-shot Tanker-cargo route are C# behavior, not ROM-program `$24`.
- Production C# Levels 8 and 9 are not implemented. Current Level 7 movement tests establish the C# route and cargo behavior only; they do not prove ROM or arcade behavior.
- The C# Level 5 generic lane-changing behavior is a potential mismatch with the ROM's `$53` Spike-sensitive route. This analysis does not authorize changing it.

Sources: [Level 5-7 configurations](../../src/Tempest.Game/Levels/LevelConfiguration.cs#L212), [C# movement and program gate](../../src/Tempest.Game/Gameplay/Flipper.cs#L74), [C# Level 7 route tests](../../tests/Tempest.Game.Tests/Level7FlipperMovementTests.cs#L13), [Level 7 source/arcade conflict in the behavior spec](../../TEMPEST-BEHAVIOR-SPEC.md#L160).

## Level-by-level findings

| Level | ROM-backed route/Spike result | Comparison or unresolved point |
|---:|---|---|
| 1 | `$07`; no starting Spikes; straight climb. | Broadly agrees with accepted Level 1 no-below-rim-flip behavior. Timing is not assessed here. |
| 2 | `$0b`; no starting Spikes; eight along-moves per flip, no movement during the flip. | The C# generic `canflip` policy is not a literal transcription of this p-code loop. |
| 3 | `$19`; no starting Spikes; repeated flips with movement between flip-continuation ticks. | This is distinct from both `$0b` and `$24`. |
| 4 | `$24`; starts with `$e0` Spikes, but script does not read them. | Demonstrates that a Spike board does not imply Spike locking. |
| 5 | `$53`; starts with `$d8` Spikes; lane-stay depends on current Spike height. | User-supplied lock hypothesis is consistent with the ROM program, subject to the live-state/threshold qualifications and cargo release-depth exception. |
| 6 | `$0b`; starts with `$d4` Spikes; no Spike test. | Script does not gate lane changes on Spike height. |
| 7 | `$24`; starts with `$d0` Spikes; no Spike test. | ROM's `2/reverse/3/reverse` p-code conflicts with the accepted visible arcade/C# `2-3-2-2-2` route. |
| 8 | `$19`; starts with `$c8` Spikes; no Spike test. | Board name “Vee” is Incitti secondary evidence; ROM geometry/open-edge data are omitted; C# production level is absent. |
| 9 | `$53`; starts with `$c0` Spikes; same Spike-sensitive program as Level 5. | Board name “Steps” is Incitti secondary evidence; ROM geometry/open-edge data are omitted; C# production level is absent. |

## Bounded recommendation

1. Preserve all accepted Level 1-7 behavior. In particular, do not replace Level 7's manually accepted `2-3-2-2-2` route with the ROM `$24` trace without resolving the discrepancy with Neal.
2. Do not change Level 5 based solely on this analysis. Record `$53` as a strong ROM-backed reason to investigate the apparent Spike-lane stay, but seek a controlled arcade comparison before revising accepted C# behavior.
3. For any separately authorized future level work, use a narrow level-selected movement-program seam, distinct from generic `CanFlip`. At minimum, tests should exercise the Level 5/9 `$53` predicate at below-tip, equality, above-tip, absent/destroyed, and shortened-Spike states; verify lane checks occur only after the current segment changes; verify movement continues during the stay; and distinguish natural near-rim cargo (`$07`) from deep player-shot cargo (level program).
4. Establish ROM-to-screen lane handedness and the omitted geometry/open-edge table values before labeling route direction as clockwise/counterclockwise or assigning endpoint reversal to a particular level. Keep Incitti-derived board names and mechanics explicitly secondary.

## Validation and files

- Rechecked the task's standalone completion token before repository work; the copied prompt record is a byte-for-byte copy of the user attachment (SHA-256 `308BC9BD574A30C5F4327CB8B07FE04A64E1070730B5BA754AD59A4A2FFA94A9`).
- Verified the required initial branch/HEAD/clean-tree checkpoint before source investigation.
- Performed source, project-document, and test inspection only. No build or test run was needed for this analysis-only task; no interactive game was launched.
- The only authorized writes are [the preserved prompt](../Prompts/2026-11-06-level-specific-flipper-spike-rom-analysis-prompt.md) and this report.
- Neal's manual confirmation of the Level 5/9 behavior remains open; no implementation or game launch is requested or performed.
