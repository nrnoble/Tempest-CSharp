# Production Level 7 preimplementation analysis

Date: 2026-11-04. Analysis only; implementation requires Neal's review.

## 1. Baseline, scope, and evidence

- Required and verified HEAD: `79e33886772424a31973332f33f3781b55e4c2f8`.
- Initial status: `## master`; tracked worktree and index clean, both diff
  statistics empty. Levels 1-6 remain the accepted production foundation.
- Reviewed project instructions, current behavior/development authority,
  relevant acceptance and movement history, README, the final Level 6 report,
  Incitti source/data, checked-in annotated ROM source, and relevant C# seams
  and tests. Historical pending/terminal statements do not override the
  accepted Level 6 closeout.
- The complete substantive attachment is preserved in
  [the prompt record](../Prompts/2026-11-04-level7-preimplementation-analysis-prompt.md).
  UTF-8 comparison confirmed identical content after newline normalization.
- Only the ignored prompt and this ignored report were written. Production,
  tests, project authority documents, references, audio, and replay were not
  modified. No interactive game, build, or test run was launched.
- REFERENCE VERIFIED below means explicit Incitti behavior. ROM-BACKED means
  a conclusion from the checked-in annotated disassembly and instruction/data
  trace, not a fresh observation of a running arcade machine. Neither label
  automatically means ARCADE VERIFIED.

## 2. Exact Level 7 identity and geometry

REFERENCE VERIFIED: the seventh board in the Original Arcade set is
`Level 7 - clover`, not the separate custom `levelset7.txt`.
Source: [levelset1.txt:14](../../Reference/incitti/levelsets/levelset1.txt#L14).

| Property | Level 7 |
|---|---|
| Zero-based progression / displayed level | 6 / 7 |
| Board / palette group | 6 / 0 |
| Topology | Closed, continuous flag 1 |
| Vertices / playable lanes | 16 / 16; includes closing lane 15 -> 0 |
| Center Y / Y offset | 310 / -10 |
| Depth / ready position | 400 / 50 |
| Projection | Existing K=70, ZOffset=5 at ready/play |
| Player start | Reference lane 7; retain C# fractional lane position 0.5 |

Coordinates in exact source order:

```text
(-160,160), (-179,44), (-89,0), (-179,-44),
(-160,-160), (-44,-179), (0,-89), (44,-179),
(160,-160), (179,-44), (89,0), (179,44),
(160,160), (44,179), (0,89), (-44,179)
```

SetUpLevel reads continuity/center/offset and 16 coordinate pairs at
[tempest.bmx:4177](../../Reference/incitti/tempest.bmx#L4177).
Depth/approach-to-50 are at
[tempest.bmx:1280](../../Reference/incitti/tempest.bmx#L1280);
successful progression sets lane 7 at
[tempest.bmx:1431](../../Reference/incitti/tempest.bmx#L1431).
Fractional position 0.5 and reuse of the accepted approach are C# policy,
not independently measured arcade placement.

Read-only arithmetic on the actual row confirmed 35 fields, 16 vertices,
and positive signed double area 204128. Applying the existing C# interior
angle formula yields a repeating hinge sequence:
108.604109, 106.751441, 307.893009, 106.751441 degrees.
At 9 degrees/update those pivot increments take 13, 12, 35, 12 updates,
respectively, excluding pauses/state-boundary ticks. These are geometric
calculations, not a new renderer test or measured arcade timing. The four
reflex hinges are vertices 2, 6, 10, 14; preserve full long sweeps.

## 3. Enemy supply: a material reference disagreement

Do not silently extrapolate the preceding levels.

- REFERENCE VERIFIED Incitti:
  `10 + lv/2 + current_board*2`, integer division at lv=6/board=6,
  gives **25 original units**, four initially active Eggs, **21 reserve**.
  See [tempest.bmx:4216](../../Reference/incitti/tempest.bmx#L4216).
- ROM-BACKED medium/default:
  `wave_enemies` seventh byte is `$14`, giving **20 original units**.
  The table is at [ROM:2841](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L2841).
  The effective displayed level is curlevel+1
  ([ROM:2547](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L2547)).
  This is the first supply disagreement in the implemented 1-7 sequence:
  preceding six values agree at 10/12/15/17/20/22.
- Recommended PROJECT DECISION for a new production Level 7: use the
  ROM-backed **20 originals**, retaining accepted C# four-initial-Egg policy,
  hence **16 reserve**. This 4+16 integration is not a claim that the ROM has
  Incitti's Egg representation or schedule.
- Neal must review that recommendation before implementation. Using 25 is a
  possible deliberate Incitti-fidelity choice, but must be labeled as such,
  not described as an unambiguous Atari supply.

## 4. Enemy eligibility and production selection

REFERENCE VERIFIED at lv=6/board=6:

- Flippers: enabled.
- Tankers: enabled; two Flipper children. Pulsar and Fuseball cargo disabled.
- Spikers (Incitti "Spinners"): enabled.
- Fuseballs: disabled (first board-based eligibility at board >9).
- Pulsars: disabled (lv>15, displayed Level 17).
- Starting Spikes: one height-60 Spike per lane, 16 total.

See [tempest.bmx:4224](../../Reference/incitti/tempest.bmx#L4224) and
[tempest.bmx:2026](../../Reference/incitti/tempest.bmx#L2026).
ROM tables also permit Flippers/Tankers/Spikers and not Pulsars/Fuseballs on
7; their simultaneous min/max controls are not the C# picker:
Flippers min/max 0/5, Tankers 1/2, Spikers 2/4, max enemy index 6.
See [ROM:2737](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L2737)
and [ROM:2831](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L2831).
These limits are not fixed per-wave species counts.

Reuse `FlippersTankersAndSpikers`, the existing identity ledger, and accepted
picker unchanged unless separately authorized. Its raw candidate 3 or 4
selects Tanker; Incitti rejects 3 while Fuseballs are disabled. The discrepancy
is already accepted and documented, not a reason to retune old levels.
Spikers are suppressed when active enemy units plus active Eggs is <4,
or active Spikers >3; unreleased reserve is excluded. No fixed composition
ratio or exact random distribution is established as arcade-verified.
See [Level3WaveSelection.cs:42](../../src/Tempest.Game/Levels/Level3WaveSelection.cs#L42).

## 5. Spike exit and timing

REFERENCE VERIFIED: displayed Level 7 is the last in Incitti's early
Avoid Spikes group (zero-based current_level >2 and <7).
Its wait threshold is `150-(6-3)*40 = 30`, so `++counter > 30`
means **31 avoidance updates**, then the separate **21-update pretravel**
delay, then the existing zoom.
See [tempest.bmx:1344](../../Reference/incitti/tempest.bmx#L1344),
[tempest.bmx:1389](../../Reference/incitti/tempest.bmx#L1389).

Set `AvoidSpikesWaitThreshold=30`; do not copy Level 6's 70.
Keep Levels 4/5 thresholds 150 and Level 6 threshold 70 unchanged.
Reuse accepted movement, no-new-fire during warning/pretravel, firing during
travel, retained in-flight shots, persistent Spike damage, swept collision,
reverse/retry, and final-life Game Over. These C# policies include arcade
observations that override literal Incitti firing eligibility.

Incitti's height-60 starting Spikes are reusable reference data. ROM uses
different along-position units (`wave_spikeht` Level 7 `$d0`); do not claim
height 60 is an exact ROM coordinate conversion.

## 6. Flipper timing: distinguish the two movement models

REFERENCE VERIFIED Incitti lv=6:

- `canflip=true`; ordinary climb +2, climb on a Spike +3.
- Flip step `8+6/5 = 9` degrees/update.
- Below-rim lane pause `21-6/5 = 20`; rim pause `24-6/5 = 23`.
- Below-rim pauses add +1 height; ordinary below-rim pivots add +2.
- Starting below a Spike tip keeps the entity sliding until it clears the
  tip, then an eight-update pause; entering another uncleared Spike lane can
  return it to sliding. This is general Incitti logic, not a Level-7-only
  behavior.

Sources: [tempest.bmx:4255](../../Reference/incitti/tempest.bmx#L4255),
[tempest.bmx:2747](../../Reference/incitti/tempest.bmx#L2747),
[tempest.bmx:2785](../../Reference/incitti/tempest.bmx#L2785).

Those are **Incitti timing constants**, not an exact specification of the
ROM's Level 7 movement program. In particular, applying a 20-tick pause
after every zigzag flip would not reproduce the ROM program below.
The ROM uses fixed-point along-speed, geometry-angle steps, and p-code;
the C# 400-unit scale/degree increments are not a cycle-exact conversion.
Retain the accepted C# rim behavior unless separately authorized.

## 7. Production firing, Tankers, Spikers, and Super Zapper

ROM-BACKED medium/default Level 7 Flipper firing:

- Holdoff **62** (`80-3*(7-1)`).
- `enm_shotmax=$02` means indices 0..2: **three active slots**.
- Existing pressure thresholds `00/E0/F0/FA/FF`, inclusive random acceptance,
  ready initialization, movement-independent delay, pivot emission gate,
  and successful-shot-only reload remain reusable.

Sources: [ROM:2699](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L2699),
[ROM:4547](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L4547).
Both C# displayed-level switches currently throw beyond 6; add explicit 7
cases, not a fallback. Existing C# cutoff Height<=343 remains an approximation.
The three-slot restriction is used by Flipper emissions with all existing
enemy bullets contributing pressure; unchanged Tanker/Spiker code does not
become a newly unified hard-cap ROM firing system.

REFERENCE VERIFIED Tanker lv=6 climb is **1.75** (integer `6/20=0`).
Existing start height 10, release above 385, stop firing at 380, 1-2 rounds,
initial timer `16+Rand(64)`, decrement-before-negative/reset-16 and >75 shot
roll need no Level-7-specific change.
See [tempest.bmx:2573](../../Reference/incitti/tempest.bmx#L2573),
[tempest.bmx:2622](../../Reference/incitti/tempest.bmx#L2622).

REFERENCE VERIFIED Spiker source expression:
`6/48.0 + Rand(2.5,4.5)`. Exact BlitzMax fractional Rand coercion and
arcade-equivalent distribution are not established. Existing accepted C#
uses `6/48f + 2.5 + NextDouble()*2`, i.e. **[2.625,4.625)**, and naturally
uses the new progression index. Keep it labeled C# reuse, not an exactly
verified source distribution. Retreat speed multiplier .90, growth>100
decision, 45-degree visual rotation step, optional retreat bullet, and
return-to-reserve behavior require no new level-specific rule.
See [tempest.bmx:2003](../../Reference/incitti/tempest.bmx#L2003),
[tempest.bmx:3634](../../Reference/incitti/tempest.bmx#L3634),
[TempestGame.cs:747](../../src/Tempest.Game/TempestGame.cs#L747).

Fresh Level 7 should have two Zapper charges, zero flash, no stale targets,
fresh enemies/ledger/Spikes, no death or Hurry-Up state. Preserve carried
score/current lives. Ordinary death retains remaining Zapper charges,
resolved identities, score, and damaged Spikes while reconstructing unresolved
units. Fresh-level recharge is also explicit in Incitti completion.
See [TempestGame.cs:361](../../src/Tempest.Game/TempestGame.cs#L361).

## 8. Incitti below-rim Flipper trace

This source does NOT contain a per-board route program table.

1. `SetUpEnemies` disables canflip only at lv=0, enables it thereafter, and
   computes generic speed/pause values from level number.
2. Normal sliding checks the lane's Spike. Below a tip it climbs at +3;
   otherwise its first +2 climb enters the initial zero pause when canflip.
3. Sliding calls `FindShortDir(enemyLane, playerLane)`. On a closed 16-lane
   tube it chooses the shorter index route; a same-lane tie chooses +/-1
   randomly. An opposite-lane distance of eight follows the arithmetic sign.
4. Below-rim pause expiry retains that direction and chooses an adjacent lane.
   Direction is not continuously retargeted during a pivot.
5. Direction can be resampled if the entity returns to sliding on a Spike.
   At the rim Incitti recalculates toward the player at each pause; C#'s
   accepted separate persistent rim latch deliberately differs.
6. Topology selects wrap vs endpoint reversal. Geometry determines hinge
   angle and hence pivot duration; spike state can interrupt the rhythm.
   It does not choose a special clover left/right sequence.

Sources: [tempest.bmx:2750](../../Reference/incitti/tempest.bmx#L2750),
[tempest.bmx:2806](../../Reference/incitti/tempest.bmx#L2806),
[tempest.bmx:3987](../../Reference/incitti/tempest.bmx#L3987),
[tempest.bmx:1452](../../Reference/incitti/tempest.bmx#L1452).

A recognizable geometry-dependent rhythm can therefore emerge from the
Incitti generic mechanism, but it does not establish the characteristic
arcade Level 7 route. Generic canflip alone is insufficient evidence.

## 9. ROM establishes a specific Level 7 movement program

The checked-in ROM source resolves the central investigation:

- `flipper_move` table opcode $06 indexes position in a 16-level block.
  Levels 1-7 select **07, 0b, 19, 24, 53, 0b, 24**.
  [ROM:2856](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L2856)
  and [ROM:2929](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L2929).
- Ordinary Flipper creation loads that selected entry into the movement PC.
  This is executable selection, not merely a descriptive comment:
  [ROM:3482](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3482),
  [ROM:3315](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3315).
- Program **$24**, used by Level 7 (also Level 4), moves, performs **two
  complete adjacent flips in one direction, reverses, performs three in the
  other direction, reverses, and repeats**. The counters, reverse opcode $16,
  and branches explicitly encode this sequence:
  [ROM:4283](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L4283).
- The program advances along the tube on its movement/continue-flip ticks,
  not on flip-start ticks; it also has explicit halts/state boundaries.
  It does not use Incitti's universal 20-update between-flip pause.
- No attract-to-player opcode $24 occurs in this $24-entry program.
  (Entry address $24 and opcode $24 are distinct concepts.)
  Neither direction randomization nor repeated player-following is in its
  below-rim route loop. Direction persists within a two- or three-flip block,
  not forever.
- ROM ordinary Flipper initialization starts its type/direction byte clear.
  In ROM segment indexing, that clear direction completes toward the previous
  segment; $16 toggles it. The raw direction and segment ordering must be
  mapped to the imported C# coordinate order before calling it clockwise or
  selecting a C# sign. Screen-handedness is not established by this analysis.
- A symbolic route from S, first direction D, is:
  `S, S+D, S+2D, S+D, S, S-D, S, S+D, ...` modulo 16.
  A full two/three cycle drifts one lane opposite its initial direction.
  Geometry changes each pivot's duration, not its prescribed flip count.
- `rev_if_edge` is relevant to open endpoints, not closed clover:
  [ROM:3980](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3980).
- At the rim, the ROM switches to a separate top program. Do not continue
  the below-rim block counter as a substitute for accepted rim pursuit.
  [ROM:3786](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3786).
- Tanker offspring have their own creation path and may use a different
  movement entry depending on release position. Do not assume every cargo
  Flipper must immediately run the ordinary-original $24 loop:
  [ROM:3542](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3542).
  Retain accepted cargo release/routing for the bounded milestone.

Conclusion: Incitti has a general mechanism, but the ROM contains actual
level-selected route programs. Level 7 should differ visibly from the
one-direction generic C# route. Its ROM pattern is not unique to clover:
Level 4 selects the same program; geometry gives it a different presentation.
This is ROM-BACKED, not yet Neal-observed Level 7 ARCADE VERIFIED evidence.

## 10. Previously accepted Levels 1-6: classification, not reopening

| Level | Current C# / comparison | Classification and action |
|---|---|---|
| 1 | Straight below-rim climb; canflip false; ROM $07 also straight | Known correct per Neal's established observation; preserve |
| 2 | Shared canflip, player-relative initial choice, persistent below-rim direction, geometry-dependent pivot | Broad movement manually accepted; reference-consistent with Incitti in the non-Spike case, not all internals arcade-verified. ROM $0b uses eight move ticks then flip without along-motion during flip: future cadence investigation only |
| 3 | Same route mechanism with plus geometry | Accepted inward/reflex geometry; Incitti-consistent non-Spike routing. ROM $19 flips continuously with different cadence: potentially needs future fidelity investigation |
| 4 | Same persistent route with binoculars geometry | Accepted gameplay; ROM $24 two/three reversals are absent: concrete ROM-source mismatch, potentially future fidelity work, not a newly demonstrated gameplay acceptance defect |
| 5 | Same route with cross geometry | Accepted gameplay; ROM $53 explicitly rides Spikes and Incitti also has Spike-aware sliding absent from C# Flipper.Update: future targeted fidelity investigation |
| 6 | Same route, triangle, 9/20/23 tuning | Multiple-playthrough acceptance remains PASS; ROM $0b cadence differs from generic C# motion: future fidelity investigation, no retuning authorized |

Shared logic alone is not a defect. Static source mismatches above are
demonstrated mismatches to those sources, not proof Neal's accepted gameplay
is broken or authority to revise it. No concrete newly reproduced gameplay
defect requiring immediate correction was established. Keep every accepted
Level 1-6 configuration on the existing movement path.

Current mechanism:
[Flipper.cs:57](../../src/Tempest.Game/Gameplay/Flipper.cs#L57),
[Flipper.cs:181](../../src/Tempest.Game/Gameplay/Flipper.cs#L181),
[LevelConfiguration.cs:115](../../src/Tempest.Game/Levels/LevelConfiguration.cs#L115).
It shares ordinary below-rim direction policy across 2-6, but not identical
geometry/timing. Cargo is a separate accepted case, not ordinary canflip.

## 11. C# seams and exact limitations

| Surface | Required eventual work / reusable behavior |
|---|---|
| LevelLoader | Add source-ordered clover loader; currently ends at triangle. No data-file parser or new levelset architecture needed |
| LevelConfiguration | Add immutable 6/7/6/0 object, closed clover, approved supply, mixed composition, 16 height-60 Spikes, threshold 30, and opt-in movement policy |
| FlipperConfiguration / Flipper | Existing boolean and tuning fields cannot encode two/three block counts or programmed reversals. Add the smallest Level-7-opt-in route/cadence state, not a universal VM or broad rewrite. Old default and cargo policy unchanged |
| Firing switches | DisplayedLevel holdoff/capacity switches stop at 6 and throw; add 7 -> 62 / 3 |
| LevelProgression | Resolver returns null after Level6Production. Add only its successor Level7Production; keep 7 terminal |
| Successful exit | Existing complete-after-safe-travel commit point already reuses initializer and carries score/lives. Failed Spike travel must remain on Level 6 |
| Direct diagnostic | Add --level7-test parsing, scenario conflicts, constructor guard/launch selection, disabled natural progression, firing parity, window title and title-update exclusion, replay/Game Over restart behavior |
| Renderer | Geometry and HUD are configuration-driven; palette 0 supported. Existing endpoint hinges, reflex sweeps and inward clipping reusable. If a Level 7 diagnostic HUD label is desired, wire it without changing Level 6 presentation |
| Wave/ledger | Composition-based, not level-number gated. Original count parameterized; two-Flipper Tanker cargo and Spiker reserve behavior reusable |
| Reconstruction | Retains Level/ledger/damaged Spikes and rebuilds fresh enemies. Reset new route state with each fresh ordinary Flipper; preserve separate cargo policy |
| Campaign soak | Campaign list/endpoint end at 6. Add 7 and extend fresh-wave resolver whitelist through outgoing Level 6 (currently only 1-5), while retaining fail-closed unsupported-state checks |
| Approach | Reuse frozen fresh wave, player rim input, recharge message, 181 updates and projection restoration; exited Level 6 uses mixed-color stars already |
| Terminal tests | Update natural Level 6 terminal expectation to Level 7 handoff; add Level 7 terminal/new campaign. Direct Level 6 remains isolated and terminal |

Principal seams:
[LevelProgression.cs:5](../../src/Tempest.Game/Levels/LevelProgression.cs#L5),
[LevelConfiguration.cs:57](../../src/Tempest.Game/Levels/LevelConfiguration.cs#L57),
[TempestGame.cs:1737](../../src/Tempest.Game/TempestGame.cs#L1737),
[DebugController.cs:103](../../src/Tempest.Game/Debug/DebugController.cs#L103),
[Program.cs:40](../../src/Tempest.Game/Program.cs#L40),
[CampaignSoakTests.cs:13](../../tests/Tempest.Game.Tests/CampaignSoakTests.cs#L13).

Closed 16-lane support is adequate; topology modulo assumptions and
FindReferenceShortestDirection's eight-lane half-distance do not block
clover. Renderer palette support currently only 0 also does not block it.
Tanker 1.75 is still applicable at lv=6; neither Tanker nor ledger requires
generalization for hypothetical later speeds/enemy types.

## 12. Reference continuation after Level 7

Incitti awards its level-completion bonus, clears the level, increments
current_level to 7, selects board 7/palette 0, installs the open Vee
(16 vertices/15 lanes), restores two Zappers and lane 7, then approaches.
Source: [tempest.bmx:1403](../../Reference/incitti/tempest.bmx#L1403).

That is reference behavior, not permission to implement it. Proposed C#
terminal Level 7, no completion bonus, and R -> fresh Level 1 natural
campaign are PROJECT DECISIONS following the accepted bounded milestones.
Direct Level 7 would replay its own scenario; Game Over restart likewise.

## 13. Unknowns and prerequisite decisions

Known source data does not establish observed arcade fidelity. Still needed:

1. Neal's review of **ROM 20 vs Incitti 25** supply; recommendation is 20.
2. An explicit Level-7-only movement integration contract: map initial ROM
   direction/segment orientation into C# lanes, specify completed-flip
   counting and initial/repeat phases, and reconcile ROM program cadence
   with C# degree/height units. Do not quietly bolt two/three reversals onto
   the old 20-tick pauses and call it exact ROM motion.
3. Whether Neal observes the ROM two/three route in trustworthy Level 7
   footage, including its initial handedness and behavior above Spike tips.
   Source establishes the program; this analysis did not measure a video.
4. Exact arcade speed, turn duration, pause durations, Egg randomness,
   species distribution, firing pressure, Spiker speed distribution, internal
   accounting and cargo intermediate routes remain unmeasured.
5. C# geometry can represent the clover, but the full renderer/pivot path has
   not been executed on a newly implemented Level 7 in this analysis.

Before coding, resolve items 1-2 in the approved implementation specification.
The route counts are not unknown and need not be invented; direction mapping
and cadence must nevertheless be nailed down rather than guessed. If those
cannot be specified safely from the source trace, a focused motion/footage
analysis is the prerequisite, not a generic Level 6 clone.

## 14. One recommended bounded implementation milestone

**Production Level 7 clover, with opt-in ROM-backed two/three ordinary-Flipper
motion, natural Level 6 -> 7, direct production-equivalent diagnostic,
terminal Level 7, tests, and subsequent manual acceptance.**

Subject to the prerequisite decisions above:

- Use exact Incitti clover coordinates, identity 6/7/6/0, approved supply
  (recommended ROM-backed 20), accepted four initial Eggs and pacing,
  existing Flipper/Tanker/Spiker composition/accounting.
- Add only the narrow Level-7 movement capability required by program $24.
  Reuse inward pivot geometry, accepted rim pursuit and cargo routing.
  Preserve old defaults and all accepted 1-6 behavior; do not make a whole
  enemy p-code interpreter.
- Keep evidence labels explicit: Incitti timing values 2/9/20/23 describe
  Incitti; approved ROM-like cadence may replace ordinary Level 7 lane pauses.
  Any height/degree mapping compromise is a PROJECT DECISION requiring manual
  comparison, not a literal ROM timing claim.
- Add Level 7 holdoff 62/capacity 3 and threshold 30; retain existing Tanker,
  Spiker, Spike collision/firing, audio events and Zapper implementation.
- Reuse safe progression commit/fresh initializer and accepted approach.
- Add --level7-test with production movement/firing parity and isolated
  terminal replay; no replay infrastructure work.
- Require focused and full automated gates, then Neal's manual acceptance.
  Do not claim Level 7 accepted or commit it as accepted before that test.

This is not ready as a configuration-only copy of Level 6. The narrow motion
capability is justified by actual ROM evidence, not speculative improvement.

## 15. Explicit exclusions

No Level 8+, open-tube architecture, production Pulsars or Fuseballs, broad
Flipper/Pulsar redesign, speculative routes, retroactive 1-6 tuning,
replay/attract changes, high scores, end-game animation, completion/extra-life
bonuses, deferred enemy-destruction volume balancing, other audio changes,
spinner retuning, broad rendering-fidelity work, reference modifications,
generated recordings, or unrelated refactoring/cleanup.

## 16. Eventual automated validation

Use focused tests following the Level 6 precedent, with source-cited expected
values rather than inheriting the predecessor:

- Exact 16 source-ordered vertices, closed lane 15/0, center/offset,
  identity, palette, 400/50 depth/position, player 7/0.5, approved 20 or 25
  supply and corresponding 16 or 21 reserve, four initial Eggs, and ledger.
- All 16 hinges in both directions: finite endpoints at start/mid/end,
  correct destination, inside-web arcs, no lost full 307.893-degree reflex
  sweeps, wrap, variable 13/12/35/12 pivot increments at step 9.
- Ordinary-original two/three route across several cycles and wrap; count
  only completed pivots, reverse only at block boundaries; moving player and
  RNG changes must not retarget this program mid-block. Pin initial direction
  mapping and every agreed cadence/height/state boundary.
- Rim arrival during a long pivot; below-rim-to-rim handoff uses the accepted
  persistent pursuit policy rather than the zigzag counter. Cargo release
  and fresh cargo Eggs retain approved separate behavior.
- Life reconstruction creates fresh route counters/no stale direction;
  persistent Spikes, resolved originals/cargo, score, charges survive;
  new level/replay fully resets wave, flash, targets, death/Hurry-Up state.
- Eligibility excludes Pulsars/Fuseballs and their cargo; accepted picker
  thresholds, Tanker ammunition/release, Spiker selection/retreat/re-hatch
  unchanged. Do not assert fabricated fixed random species ratios.
- Firing 62/3, initial-ready, probability boundaries, capacity pressure,
  delay through pivots, post-pivot ready emission, cutoff, cargo firing and
  Tanker/Spiker regression; unchanged 1-6 constants.
- Exactly 31 AvoidSpikes updates, then 21 pretravel; retained shots,
  warning/firing/movement boundary, shortening/scoring, lethal long Spike,
  reverse/retry and final life; 4/5=151 and 6=71 remain unchanged.
- Natural Level 6 safe exit installs Level 7 only after completed travel;
  failed exit stays on 6. Score/lives carry, two Zappers/zero flash,
  new ledger, no stale bullets/targets; mixed stars keyed to exited 6,
  approach movement/firing freeze and seamless Playing arrival.
- Direct-access configuration/movement/firing parity, --lives/conflicts,
  completion replay and Game Over reset; direct Level 6 remains isolated.
- Natural Level 7 completes without installing 8; R/Game Over start fresh
  Level 1 with expected score/lives; no bonus.
- Campaign soak through Level 7 arrival; extend outgoing Level 6 whitelist,
  preserve unsupported-state failures and explicit orchestration-only claims.

Suggested eventual focused runner (new class names are proposals):

```powershell
dotnet test TempestCSharp.slnx --filter "FullyQualifiedName~Level7ProductionTests|FullyQualifiedName~Level7FlipperMovementTests|FullyQualifiedName~ArcadeFlipperFiringTests|FullyQualifiedName~LevelProgressionTests|FullyQualifiedName~LevelConfigurationTests|FullyQualifiedName~ExitLaneControlTests|FullyQualifiedName~CampaignSoakTests|FullyQualifiedName~Level2CanFlipTests|FullyQualifiedName~Level3GeometryTests"
dotnet test TempestCSharp.slnx
dotnet build TempestCSharp.slnx
git diff --check
```

Keep existing cargo, Zapper, and lifecycle tests in the full gate; run their
focused classes too if the implementation touches those mechanisms.
The prior 719-test/zero-warning result belongs to Level 6 closeout, not a new
test run or proof of proposed Level 7 behavior.

## 17. Concise manual acceptance checklist

After implementation and automated validation, Neal should:

1. Compare the clover silhouette, four inward notches, complete closed rim,
   player movement/placement, HUD and palette with arcade footage.
2. Follow ordinary Flippers from hatch: record two flips, reverse, three,
   reverse, repeat, with spawn lane, initial direction and approximate
   height. Move the Blaster behind them: does the below-rim route persist?
   Distinguish ordinary originals from cargo and rim enemies.
3. Compare outer-lobe vs reflex-notch pivots, long inward arcs, durations,
   Spike-tip crossings and rim arrival; ensure the result looks like the
   characteristic arcade clover rather than a generic one-way route.
4. Verify Flipper/Tanker/Spiker mix, no Pulsars/Fuseballs, reasonable ordinary
   and cargo firing, Tanker child survival/release, and sensible Spiker
   building/retreat/re-entry. Do not infer hidden constants from good feel.
5. Reach 7 naturally from 6: mixed stars, correct upcoming clover, movable
   rim Blaster, recharge, carried score/lives, fresh charges and smooth arrival.
6. Lose a life and verify no stale wave/route state, correct accounting,
   remaining charges, persistent damaged Spikes and normal responsiveness.
7. Clear 7: shorter warning wait, controlled shooting only during travel,
   short-Spike clearance and lethal remaining long Spike, retry/final life.
8. Confirm terminal 7/new-campaign restart and direct diagnostic replay.

Existing recorded-input playback may aid observation of a compatible normal
campaign later. It currently starts at Level 1; do not promise direct Level 7
file recording or extend frozen replay infrastructure for this milestone.
Future launch commands, not executed here:

```powershell
dotnet run --project src\Tempest.Game -- --level7-test --lives 10
dotnet run --project src\Tempest.Game -- --lives 10
```

The first flag is a proposal and does not exist at this checkpoint.

## 18. Final state

Final repository checks: HEAD unchanged at
`79e33886772424a31973332f33f3781b55e4c2f8`, status `## master`,
unstaged/cached diff statistics empty. The prompt/report are separately
accounted ignored AI records; no production/test/project-document/reference
modification is included. No audio change, generated gameplay recording,
commit, push, game launch, or Level 7 implementation occurred.

Stop after analysis. Await Neal's review before any implementation.
