# Level 6 Tanker Flipper `$0b` bounded comparison

Date: 2026-11-10

## Result

**C — current C# cannot represent the ROM's deep-shot versus near-rim
movement distinction.** The paths have substantially different observable
cadences: deep `$0b` cargo climbs in straight steps separated by stationary
flips, while `$07` cargo climbs straight without below-rim flips. Level 6 C#
uses the same climbing-pivot/pause cycle for either release. This is a
ROM-backed structural distinction, not a finding that Neal's accepted C#
appearance looks wrong. No new arcade comparison or game launch was performed.

## Movement comparison

### ROM-BACKED: deep-shot `$0b`

The `$0b` program initializes an eight-move counter, executes eight
per-speed along-tube advances, then starts a lane flip. The flip-completion
loop has no along-movement action, so the Flipper stays at the same tube depth
for the turn. On completion, program flow returns to the start of the loop;
there is no separate scripted dwell before the next eight-advance interval.
The eight advances are ROM speed-table movement operations, not a known
number of C# world-height units or pixels.

The two cargo children start with opposite direction bits. `$0b` contains no
direction-reversal instruction, so each retains its lane-travel direction
through its repeated cycles. Open-end correction is separate from `$0b`;
screen-left/right or clockwise/counterclockwise equivalence remains **UNKNOWN**.

The loop repeats while below the top. On reaching the ROM top condition, the
top-of-tube handler takes over instead of continuing the below-rim program.
Sources: [ROM listing](../../Reference/arcade/Tempest%20Rom%20Source%20code.md),
lines 3542-3578, 3784-3860, and 4254-4270.

### ROM-BACKED: near-rim/natural `$07`

`$07` repeatedly performs one per-speed along-tube movement and branches back
to itself. It has no lane-flip operation, so cargo continues straight below
the rim until the top-of-tube condition hands it to rim handling. Natural
Tanker splitting occurs in the `< $20` near-rim branch and therefore selects
this default path, rather than `$0b`.

Sources: [ROM listing](../../Reference/arcade/Tempest%20Rom%20Source%20code.md),
lines 3542-3578, 3710-3732, 4182-4191, and 4250-4253.

### PROJECT-CURRENT: Level 6 C#

Both Tanker children begin in their release pivot. During that turn they gain
height at 2 units per update. On completion, the below-rim cargo path pauses
for 23 ticks and gains 1 height unit per pause tick. It then starts another
pivot in the same lane-travel direction; this climbing-pivot/23-tick-pause
cycle repeats until rim-height handling selects player pursuit. Thus C# has
neither `$0b`'s eight straight advances followed by a fixed-depth flip, nor
`$07`'s uninterrupted straight climb.

Level 6 uses `BelowRimFlipperProgram.Existing`; no Level 6 cargo movement
policy branches by release cause or Tanker depth. The depth/release distinction
is therefore absent in C#.

Sources: [LevelConfiguration.cs](../../src/Tempest.Game/Levels/LevelConfiguration.cs),
lines 257-283; [Flipper.cs](../../src/Tempest.Game/Gameplay/Flipper.cs),
lines 95-180, 528-570; [TempestGame.cs](../../src/Tempest.Game/TempestGame.cs),
lines 1499-1553; [Tanker.cs](../../src/Tempest.Game/Gameplay/Tanker.cs),
lines 130-157, 180-210.

## Practical setup and comparison

- **Deep-shot case:** let a Tanker travel well down the tube, then destroy it
  with a player shot before it reaches the rim. Arcade cargo at the ROM
  `enemy_along >= $20` branch should use `$0b`. Do not try to measure the
  byte threshold visually; use a clearly deep Tanker and recognize the
  player-shot release.
- **Natural case:** leave a Tanker alive until it naturally splits near the
  rim. Arcade cargo in the `< $20` branch should use `$07`. The release cause
  and near-rim position distinguish it from the deep-shot case.
- In both cases, the two children start in opposite lane-travel directions.
  Compare their directions relative to each other, not screen handedness.

Short checklist:

1. After deep-shot release, compare the straight-climb interval before each
   flip; watch whether depth holds still throughout the flip and whether the
   next interval starts without an added wait.
2. Check that each child's lane-travel direction persists below the rim and
   that the pair travel oppositely.
3. After natural split, compare straight climbing below the rim against C#'s
   release pivot and repeated climbing pivots/pauses; then note the rim
   handoff.

## Recommendation and preservation

Neal's report that current Level 6 movement looks correct remains
**ARCADE-OBSERVED** at that stated level and is not downgraded. The cadence
difference is **ROM-BACKED**; the current behavior is **PROJECT-CURRENT**,
not ROM-backed or an established `$0b`/`$07` **PROJECT TRANSLATION**.
Perceptual significance against the arcade is not newly established.

Recommend a separate, bounded Level 6 movement milestone only if Neal
authorizes it: give deep player-shot cargo the `$0b` cadence and near-rim or
natural cargo the `$07` straight path, with deterministic coverage and manual
comparison. Do not retune the current candidate based on this report alone.

No production code, tests, durable documentation, index, or HEAD was changed.
Only this requested report and the preserved prompt were created; both are
ignored `AI-Logs/` records. The Level 6 concurrency candidate remains
untouched.

## Validation

- HEAD remains `94ad02f52f8fe7f1eec79d60ffd7d5114b8ee933`.
- Index tree remains `ad98fd2f574412e602b54b4c3ff1619ec2756310`.
- Candidate tracked-diff fingerprint remains
  `752a31388d8f186636614ae7d872794871576c4a`.
- Both pre-existing untracked candidate files retain their recorded
  SHA-256 values from the starting state.
- `git diff --check` passes.
- No build, tests, game launch, staging, commit, or push was performed.
