# Level 17 frozen rim-Flipper correction report

Date: 2026-11-25

Status: **PASS - evidence-backed rim-handoff correction completed; manual acceptance remains pending**

## 1. Executive summary

Level 17 Flippers that reached the rim could remain in `Climbing` state because
the straight-`$07` update branch continued to own the actor after rim arrival.
The branch now hands rim entry to the shared Flipper rim-pursuit state machine
and relinquishes straight-route control.

The diagnostic recording reproduces the defect before correction and advances
cargo Flipper lanes after correction. A permanent theory test covers ordinary
and Tanker-cargo Level 17 Flippers.

## 2. Starting Git/worktree state

- Branch: `master`
- HEAD: `fde09aa31af760958f656aa553555462d51792a0`
- Upstream: `origin/master`
- Ahead/behind: `0/0`
- Staged state: no staged changes
- Pre-existing tracked [TEMPEST-BEHAVIOR-SPEC.md](../../TEMPEST-BEHAVIOR-SPEC.md)
  modification remained preserved.
- Existing Level 17 candidate source/test changes and AI-Logs remained
  preserved.
- Protected local recordings and ROM/reference material were not modified.

## 3. Manual defect evidence

Recorded as **MANUAL LEVEL 17 DEFECT EVIDENCE**:

- Level 17 launched and remained playable.
- Flippers reached the rim.
- Some or all remained frozen in one rim lane.
- The Blaster could move underneath and destroy frozen rim Flippers.
- Pulsars continued moving/flipping.
- Tankers behaved as expected.
- The reported defect was specific to rim-arrival Flippers.

## 4. Recording metadata/final telemetry

`TEMP/GameplayRecordings/lvl-17-test.json`:

- Format: `tempest-gameplay-input`
- Version: 2
- Random seed: `1177968332`
- Starting displayed level: 17
- Starting lives: 10
- Duration: 2609 ticks
- Direct Level 17 start: confirmed
- Final completion reason: `RecordingCompleted`
- Remaining frames: 0
- Final level/state: Level 17, corrected run reached `ExitZoom`
- Lives: 6
- Score: 5956

The initial candidate run completed as `RecordingCompleted` with 0 remaining
frames and Level 17 `ExitZoom`, but that completion was not accepted as proof
of rim behavior.

## 5. First Flipper rim-arrival boundary

The earliest captured affected actor was Tanker-cargo Flipper `cargoUnitId 7`.

- At tick 800: lane 9, height 394.75, `Climbing`, no rim-pursuit latch.
- At tick 850: lane 9 before correction, height 400, `Climbing`,
  `RimPursuitDirection=1`, no transition or lane change.
- After correction, tick 850: lane 8, state `Pausing`,
  `RimPursuitDirection=1`.

The pre-rim-to-rim interval was at approximately tick 800-850.

## 6. Frozen interval

Before correction, cargo 7 remained lane 9 and `Climbing` at height 400 across
the 850-2000 interval. It was no longer frozen after the correction:

- Tick 850: cargo 7 entered `Pausing`.
- Tick 1500: visible cargo 36/37 were `Flipping` with active lane transitions.

## 7. Ordinary-versus-cargo classification

- Recording-visible affected category: Tanker-cargo Flippers.
- Ordinary original Flippers were not captured at a useful rim boundary in this
  recording.
- The bounded automated fixture covers ordinary and cargo Level 17 Flippers.
- Root defect is shared by both straight-`$07` paths because both set
  `useStraight07Program`.

## 8. Flipper state before rim arrival

Before rim arrival, the captured cargo 7 was in `Climbing` at lane 9 with
height 394.75 and no active rim-pursuit latch.

## 9. Flipper state at rim arrival

Before correction, cargo 7 reached height 400 and was assigned
`RimPursuitDirection=1`, but remained `Climbing` with a zero/empty pivot and no
active transition.

After correction, rim arrival sets the pursuit latch, exits the straight
route, and enters shared rim pause/handoff state.

## 10. Frozen Flipper state after arrival

Before correction, `Climbing`, lane unchanged, height 400,
`RimPursuitDirection` latched, `ActiveTransition=null`,
`PivotProgress=0`.

After correction, the actor enters `Pausing`, then `Flipping`, and completes or
continues lane transitions.

## 11. Working prior-level comparison

The shared rim handoff in [Flipper.cs](../../src/Tempest.Game/Gameplay/Flipper.cs)
uses `BeginPause` followed by `BeginPivot` to establish lane pursuit. Level 17
did not relinquish `useStraight07Program` at rim depth, so it never reached that
shared state path.

## 12. Working Pulsar comparison

The recording showed Pulsars active through the Level 17 rim interval:

- Tick 500: 3 Pulsars
- Tick 1000: 5 Pulsars
- Tick 1500: 4-5 Pulsars

This disproves a whole-level frozen-rim or general tube-update problem.

## 13. Simulation-versus-rendering classification

**Confirmed simulation frozen before correction.** Headless state showed lane,
transition, and movement-state invariants unchanged. The recording also proves
post-correction lane/transition advances in simulation.

## 14. Route/factory trace

- `BelowRimFlipperProgram.Straight07` selects straight climbing for ordinary
  Level 17 Flippers.
- `TankerCargoMovementProgram.Level17Straight07` selects the same straight
  below-rim flag for cargo.
- Both routes set `useStraight07Program=true`.
- The branch clamped height at rim, latched `RimPursuitDirection`, and returned.
- It never cleared the route or called the shared rim handoff.

## 15. Rim-handoff trace

At height/depth arrival, the corrected branch now:

1. clamps height to `Level.Depth`;
2. establishes the rim-pursuit latch;
3. clears `useStraight07Program`;
4. calls `HandOffToRim`, entering shared pause/rim-handoff state.

The next update proceeds through the normal `Pausing` and `BeginPivot` logic.

## 16. Exact root cause

Level 17 Flippers froze at the rim because the straight-`$07` branch remained
active after `Height >= Level.Depth`, and because it returned without clearing
its route flag or establishing shared rim handoff. This prevented the normal
rim pause/pivot update.

## 17. Production correction

The branch in [Flipper.cs](../../src/Tempest.Game/Gameplay/Flipper.cs) now
requires below-rim height for route entry, clears straight-route control at rim
arrival, and calls `HandOffToRim`.

No generic prior-level route or Level 17 presentation code was changed.

## 18. Why correction preserves `$07` below-rim behavior

The straight route remains active only while `Height < Level.Depth`. It retains
straight climb and no below-rim lane flip/reversal, but terminates at rim entry.

## 19. Why correction preserves Levels 1-16

The correction is guarded by below-rim height and straight-route control. The
restored generic `TankerCargoMovementProgram.Straight07` route remains distinct
from `Level17Straight07`; no prior-level cargo or route semantics were changed.

## 20. Files changed

- [Flipper.cs](../../src/Tempest.Game/Gameplay/Flipper.cs): rim-handoff correction
- [Level17ProductionTests.cs](../../tests/Tempest.Game.Tests/Level17ProductionTests.cs):
  ordinary/cargo rim handoff theory
- [2026-11-25-level17-frozen-rim-flipper-correction-prompt.md](../Prompts/2026-11-25-level17-frozen-rim-flipper-correction-prompt.md):
  substantive task record

## 21. Permanent focused regression test

`Level17ProductionTests.Straight07FlippersHandOffToRimAndResumePursuit`
covers ordinary and Tanker-cargo Flippers.

The test reaches rim depth and asserts:

- height clamps to rim depth;
- rim-pursuit direction exists;
- the actor starts flipping or changes lane/establishes an active transition
  within 100 updates.

## 22. Level 17 recording final result

**PASS**:

- Parses as version 2;
- direct start confirmed;
- 2609/2609 ticks consumed;
- `RecordingCompleted`;
- remaining frames 0;
- post-correction cargo movement confirmed.

## 23. Levels 1-16 recording result

**PASS at the established campaign boundary**:

- Seed `1990198549`
- Boundary `35350`
- `RecordingCompleted`
- Score `134039`
- Lives `9`
- Active candidate continues at intended Level 17 `Playing`

## 24. Level 15 seed-1982 result

**PASS**: 2 theory cases passed, 0 failed.

## 25. Focused validation

**PASS**: 68 focused rim, route, cargo, Ledger, Level 17, Levels 11/15/16 tests.

The permanent Level 17 theory passed for both ordinary and cargo cases.

## 26. Full-suite result

**PASS**: 2060 passed, 0 failed, 0 skipped.

## 27. Release-build result

**PASS**: zero warnings and zero errors.

## 28. `git diff --check`

**PASS**. Git reported only the existing LF-to-CRLF work-copy warning for
`LevelLoader.cs`.

## 29. Remaining presentation/color blockers

Unchanged and pending:

- legacy-looking Level 17 tube presentation;
- missing second-cycle visible color scheme;
- exact host palette role mapping;
- Level 17 visual calibration/placement.

No presentation or palette code was changed.

## 30. Manual-validation commands

Level 17 recording:

```powershell
dotnet run --project src\Tempest.Game -- --playback-file TEMP\GameplayRecordings\lvl-17-test.json
```

Neal should verify ordinary/cargo rim motion, Pulsars, Tankers, and Blaster
collision/shooting behavior.

Levels 1-16 recording:

```powershell
dotnet run --project src\Tempest.Game -- --playback-file TEMP\GameplayRecordings\lvl1-16.json
```

Neal should confirm the campaign remains synchronized through accepted Level 16.

## 31. Temporary diagnostic cleanup

No temporary source worktree or recording rewrite was used. The protected
Level 17 diagnostic recording was not modified.

## 32. Final Git/worktree state

- Branch: `master`
- HEAD unchanged: `fde09aa31af760958f656aa553555462d51792a0`
- Upstream: `origin/master`
- Ahead/behind: `0/0`
- Existing tracked/untracked candidate state preserved.

## 33. Stage/commit/push

Nothing was staged, committed, or pushed.

## 34. Level 18+

Untouched.

## 35. Recommended next step

Neal should perform the listed manual recording validation. No interactive game
was launched. Level 17 presentation/color remains a separate milestone.
