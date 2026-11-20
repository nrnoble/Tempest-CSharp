# Stage B1 Implementation: Isolated Level 2 Square Geometry

Implementation report: 2026-10-30
Starting checkpoint: `f41a53c Add Level 1 configuration and initialization groundwork`
Scope: B1 isolated Level 2 configuration and square Flipper hinge geometry only.

## 1. Purpose and authority

Implemented the approved B1 scope from the [Stage B analysis](./2026-10-30-level2-square-canflip-analysis-report.md). The exact supplied implementation prompt is saved as [2026-10-30-level2-square-b1-implementation-prompt.md](../Prompts/2026-10-30-level2-square-b1-implementation-prompt.md).

The Stage B analysis and cited source data were used as design/reference evidence, not as proof of arcade behavior. `TEMPEST-BEHAVIOR-SPEC.md` was not modified.

## 2. Isolated Level 2 configuration

Added `LevelConfiguration.Level2SquareTest` with internal identity 1, displayed level 2, board 1, palette 0, closed 16-vertex/16-lane geometry, depth 400, position 50, center Y 400, Y offset -80, player lane 7, and supply 12 (four initial Eggs plus eight reserve units). It uses the approved ordered square points and the existing Level runtime and deterministic Egg scheduling model.

The spawn-lane and Egg-direction sequences are valid for the 16-lane runtime. These deterministic scheduling choices are project policy, not Incitti- or Atari-established behavior. Normal composition remains Flippers only.

Canflip is explicitly disabled for B1. The scenario tests the square and existing climb-to-rim behavior, not full Level 2 gameplay.

## 3. Launch and flag behavior

Added `--level2-test` as a development-only isolated launch selection. With no flags, Level 1 remains the default. The flag conflicts with `--spiker-test`, `--tanker-test`, `--fuseball-test`, and `--pulsar-test`; `--lives` and the separate opt-in `--enemy-bullets-test` remain composable.

Level Complete replay with R and Game Over restart both reinstall the selected square configuration through the existing fresh-wave path. Life reconstruction retains the current Level 2 runtime. No natural progression path was added.

The window title and HUD identify the Level 2 development scenario, including its non-production status and terminal replay behavior.

## 4. Square hinge geometry

The shared `Level.CalculateEdgeTurnAngle` remains unchanged. A narrowly selected square endpoint-hinge path finds the lanes' shared vertex and computes the sweep between the old and destination free-endpoint rays. This yields 180-degree straight-side sweeps and 90-degree corner sweeps, including the 15/0 wrap; 180-degree signs are deterministic from the transition direction.

The source points are integer-rounded: adjacent straight-side endpoint radii can differ by one unit (80 versus 81). Consequently, a rigid rotation at a fixed radius cannot both preserve the exact source endpoint and land on the exact destination point. The implementation preserves the shared-vertex hinge and angular sweep while linearly correcting the radius over the arc; progress 1 returns the exact ordered destination lane endpoints. This is radial correction, not midpoint interpolation. The tests verify ordered endpoints in both travel directions and finite intermediate coordinates.

## 5. Level 1 compatibility

Ordinary Flippers use the legacy lane-turn helper unless the installed configuration explicitly selects square endpoint hinges. The existing Level 1 renderer transform remains on its legacy path. The shared turn helper and Tanker cargo turn calculation were not changed. The full existing Level 1/gameplay regression suite passed.

## 6. Renderer changes

The existing renderer selects the square hinge projection only for the square configuration. Added endpoint and shot projection access for deterministic headless checks, plus square HUD labeling. The existing renderer still draws the tube, player, Eggs, shots, Flippers, and Super Zapper flash; no generic renderer or open-tube path was introduced.

## 7. Accounting and lifecycle

The configuration-backed Level runtime accounts for supply 12. Tests cover a kill resolving the last unit and a non-scoring all-rim clear, as well as Level Complete replay, Game Over restart, and death reconstruction. The existing fresh-wave/event setup and reconstruction paths were reused; no parallel runtime hierarchy or duplicated reset path was added.

## 8. Super Zapper and all-rim behavior

No Super Zapper targeting or lifecycle behavior was changed. Existing gameplay tests cover charge, target, flash, death, and replay behavior; the Level 2 tests verify fresh replay recharge and death persistence. The Level 2 supply tests verify that unresolved incoming units block completion, a rim survivor blocks exit until it reaches the rim, and the rim clear does not award kill score.

## 9. Files changed

- [README.md](../../README.md)
- [TEMPEST-DEVELOPMENT-NOTES.md](../../TEMPEST-DEVELOPMENT-NOTES.md)
- [DebugController.cs](../../src/Tempest.Game/Debug/DebugController.cs)
- [Flipper.cs](../../src/Tempest.Game/Gameplay/Flipper.cs)
- [Level.cs](../../src/Tempest.Game/Levels/Level.cs)
- [LevelConfiguration.cs](../../src/Tempest.Game/Levels/LevelConfiguration.cs)
- [LevelLoader.cs](../../src/Tempest.Game/Levels/LevelLoader.cs)
- [Program.cs](../../src/Tempest.Game/Program.cs)
- [VectorRenderer.cs](../../src/Tempest.Game/Rendering/VectorRenderer.cs)
- [TempestGame.cs](../../src/Tempest.Game/TempestGame.cs)
- [Level2SquareTests.cs](../../tests/Tempest.Game.Tests/Level2SquareTests.cs)

The exact prompt and this report are under ignored `AI-Logs/`. No other documentation authority was changed.

## 10. Tests added

`Level2SquareTests` adds deterministic coverage for configuration and exact point order; default and existing development scenario selection; flag conflicts and composition; replay, restart, and life reconstruction; configured-supply kill and rim-clear accounting; the legacy square helper's twelve 0-degree and four 90-degree results; all adjacent square hinge pairs and signs; renderer endpoint capture; finite projections; and flash geometry/color behavior.

## 11. Automated validation

Final results:

| Validation | Result |
|---|---|
| Focused B1 tests (`FullyQualifiedName~Level2SquareTests`) | 13 passed, 0 failed, 0 skipped |
| Grouped B1 and gameplay regression classes (`Level2SquareTests`, `LevelConfigurationTests`, `SimulationTests`, `SuperZapperTests`) | 352 passed, 0 failed, 0 skipped |
| Full solution tests (`dotnet test TempestCSharp.slnx --no-restore`) | 352 passed, 0 failed, 0 skipped |
| Solution build (`dotnet build TempestCSharp.slnx --no-restore`) | Succeeded, 0 warnings, 0 errors |
| `git diff --check` | Passed |
| `git status --short` | Ten modified tracked files and one untracked test file; the ignored prompt/report do not appear |

The class-filtered run covers the existing Flipper, Tanker, Pulsar, all-rim/Hurry-Up, Spiker exit, Super Zapper, and accounting regressions alongside B1 tests.

## 12. Intermediate failures and corrections

- The first focused test build lacked the `Tempest.Game.Systems` import for `PlayerInput`; adding the correct namespace fixed compilation.
- An initial angle assertion expected the wrong signed direction for one corner and misclassified a straight-side reversal. The expectation was corrected to the ray geometry and deterministic direction rule.
- Review found that the first destination-endpoint assertion checked the endpoint set without enforcing endpoint order. The production endpoint ordering was corrected and the test strengthened to assert exact ordered source and destination endpoints. The final focused and full runs above passed after these corrections.

## 13. Manual validation results

Neal completed the two requested manual runs after the renderer correction; the
sequence and exact results are recorded in the addendum below. The initial
plain square run looked OK, the first bullet-enabled run exposed the renderer
crash, and both commands passed on retest after the correction. The crash did
not recur.

These observations are limited to the specified B1 development scenarios.
They do not promote square geometry or transition timing to **ARCADE VERIFIED**.
**B1 deliberately does not implement canflip; canflip was not validated.**

## 14. Explicit B1 limitations

No canflip state or below-rim lane changes, natural Level 1-to-2 progression, production Level 2, other enemy families, open-tube behavior, dynamic palette progression, bonus/audio work, Tanker firing changes, or Blaster movement/input tuning was implemented. `--level2-test` alone does not enable enemy bullets.

## 15. B2 relationship

B2 remains separate: it may add the evidence-backed Level 2 canflip configuration and ordinary below-rim lane-changing state. B1 manual results and tests do not validate or assert canflip behavior.

## 16. Remaining geometry and arcade uncertainties

The 80/81 endpoint-radius correction is a consequence of the exact integer board points. Neal's manual observation that the square geometry and gameplay appeared OK does not establish arcade authenticity. No claim is made that the resulting animation, timing, spawn schedule, Flipper behavior, or any other B1 behavior is arcade-authentic. No arcade values or canflip rules were inferred beyond the approved reference analysis.

## Stop condition

B1 implementation, documentation, automated validation, and report generation are complete. No commit was made. No canflip or natural progression work was started.

## 17. Manual crash follow-up and correction

### Neal's manual result

Neal's initial plain square run was:

```powershell
dotnet run --project src/Tempest.Game -- --level2-test --lives 10
```

The square geometry and gameplay appeared OK.

Neal's first bullet-enabled run was:

```powershell
dotnet run --project src/Tempest.Game -- --level2-test --enemy-bullets-test --lives 10
```

This run crashed during drawing. Neal may have used Super Zapper once or twice
before the failure, but whether either use is related is unknown. This is a
failed manual B1 run, not a successful validation. No evidence currently
establishes that enemy bullets or Super Zapper caused the failure.

Reported exception and stack:

```text
System.ArgumentException:
Square Flipper pivots must connect adjacent lanes.
(Parameter 'destinationEdge')

Level.CalculateSquareFlipperHingeRotation
Level.CalculateSquareFlipperPivotEndpoints
VectorRenderer.ProjectFlipperEndpoints
VectorRenderer.DrawFlipper
VectorRenderer.DrawFlippers
VectorRenderer.Draw
Program.Main
```

### Root cause and exact invalid state

At a completed lethal rim pivot, the old `Flipper.CompletePivot` assigned
`EdgeIndex = DestinationEdge` and `SourceEdge = EdgeIndex`, reset both turn
angles, set `CompletedIntoPlayer`, then returned while leaving
`State == Flipping` and the old `DestinationEdge` in place. For a real pivot
from lane 1 to lane 0, the renderer therefore received:

| Property | Value |
|---|---:|
| `State` | `Flipping` |
| `EdgeIndex` | 0 |
| `SourceEdge` | 0 |
| `DestinationEdge` | 0 |
| `TurnAngle` / `TotalTurnAngle` | 0 / 0 |

This is a same-lane pair, not an adjacent pivot. The 15/0 wrap is valid and
handled by the closed-lane helpers; the validator was not rejecting a valid
wrap transition. `CompletePivot` was the last writer of `EdgeIndex` and
`SourceEdge`; it did not rewrite `DestinationEdge` on this early-return path.

The non-pivot Flipper remained in `game.Flippers` during the player-death
freeze, and the renderer iterates the live list, so it was still visible.
Flippers removed by zap expiry, shot, death reconstruction, replay, or
all-rim resolution are removed/cleared from the live list before drawing; no
stale renderer snapshot is involved. No Flipper state is persisted/restored
across these paths. Super Zapper only sets a pending timer and can later remove
the target; it does not change Flipper geometry/state fields. Enemy-bullet mode
only enables ammunition, firing decisions, and bullet updates. Neither is
causal in the reproduced failure; either may change timing or remove a
Flipper before it reaches the failing state. Plain `--level2-test` contains the
same completed-pivot path and can fail when a Flipper completes a pivot into
the occupied lane.

The same completed-collision state could exist on Level 1, but its legacy
renderer does not invoke square adjacency validation: with zero turn angle it
projects the source lane unchanged. Leaving `Flipping` early also meant the
Level 1 legacy path happened to draw the same lane geometry.

### Correction

`Flipper.BeginPause` now establishes the non-pivot state contract:
`SourceEdge == DestinationEdge == EdgeIndex`, zero total turn angle, zero
progress, and `State == Pausing`. `CompletePivot` retains the lethal collision
signal and always transitions through `BeginPause`, instead of returning in
`Flipping`. Thus the square hinge renderer is selected only for an active
adjacent pivot. The square adjacency validation remains in place. No lane is
clamped or substituted, and canflip, Level 1 turn geometry, Tanker cargo
geometry, Zapper behavior, and bullet cadence were not changed.

The deterministic lane-1-to-lane-0 regression was first run before the fix:
it failed at renderer projection with the same `ArgumentException`. After the
fix it verifies the completed-pivot collision signal, canonical paused state,
lane projection, and that no hinge request occurs. Additional tests cover
climbing/paused projection, both Zapper uses with an actual bullet-enabled
square Flipper, bullet rendering, every square pivot pair including 15/0, and
the equivalent Level 1 completed-pivot projection. Existing Level 2 tests
continue to cover death/re-entry after Zapper use, all-rim removal, and replay.

### Updated automated results

The earlier successful B1 results above are preserved as the original
checkpoint. After this crash correction:

| Validation | Result |
|---|---|
| Focused `Level2SquareTests` | 17 passed, 0 failed, 0 skipped |
| Focused `SuperZapperTests` | 31 passed, 0 failed, 0 skipped |
| Focused Flipper/enemy-bullet selector | 55 passed, 0 failed, 0 skipped |
| Grouped Level 2/configuration/simulation/Super Zapper regression | 356 passed, 0 failed, 0 skipped |
| Full solution tests | 356 passed, 0 failed, 0 skipped |
| Solution build | Succeeded, 0 warnings, 0 errors |
| `git diff --check` | Passed |
| `git status --short` | Ten modified tracked files and one untracked test file; the ignored prompt/report do not appear |

During expanded test development, one assertion incorrectly expected an
enemy bullet to remain at its spawn height after the next update; it was
replaced with finite projection and render-line checks. This was a test
expectation correction, not a gameplay change.

### Final manual retest results

After the correction, Neal reran both commands:

```powershell
dotnet run --project src/Tempest.Game -- --level2-test --lives 10
dotnet run --project src/Tempest.Game -- --level2-test --enemy-bullets-test --lives 10
```

Both runs passed, and the completed-lethal-pivot renderer crash did not recur.
In particular, the plain square run had appeared OK in the initial manual
check; the first bullet-enabled run exposed the crash documented above; and
the post-correction retest of both plain and bullet-enabled scenarios passed.
This records the observed B1 manual results only: it does not validate
canflip, which B1 deliberately does not implement, or establish arcade
authenticity for square geometry or transition timing.
