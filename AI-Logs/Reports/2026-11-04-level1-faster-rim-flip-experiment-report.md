# Level 1 Faster Rim-Flip Experiment

## Baseline and rejected ordering experiment

- Branch: `master`; `HEAD` remains `3bebf42cbe9514846ab32bf4c8a12bcfdb048da7` (`Implement accepted production Level 7`).
- The starting tracked changes matched the documented player-shot/Flipper ordering experiment. `TempestGame.cs` and the contact-ordering changes in `Level2CanFlipTests.cs` and `SimulationTests.cs` were restored to accepted `HEAD` behavior before the speed change.
- Only the requested rim-speed behavior was then added to `Flipper.cs`; its unrelated ordering changes were not retained.
- The production-regression class was removed from the pre-existing untracked `Level1StationaryRimCombatComparisonTests.cs`. Its deterministic analysis/model coverage was preserved and returned to accepted shot-before-Flipper collision order, then extended to compare the original and experimental rim speeds without asserting a particular death tick.

**ARCADE / MANUAL OBSERVATION:** Neal A/B tested the accepted baseline and the player-shot/Flipper ordering experiment. It did not noticeably improve rim-pressure/camping behavior, and the experimental version made shot timing/feel seem subtly wrong. The production ordering experiment is rejected and is not present in the final changes.

## Evidence and implementation

**ARCADE OBSERVED:** On Neal's original machine, Flippers move substantially faster after reaching the rim than while climbing, especially on Level 1. This is consistent with the previously traced ROM's separate top-rim processing and Level 1 `flip_top_accel = 2`. The absolute ROM-to-C# angle-time mapping remains unresolved.

Level 1 now has a configuration-specific rim-turn override of **16 degrees/update**, versus the existing **8 degrees/update** below the rim. This is a **PROJECT EXPERIMENT**, not a literal ROM timing. The existing 24-tick rim pause, targeting, direction, climb rate, and collision behavior are unchanged. Levels 2-5 remain at 8 degrees/update; Levels 6-7 remain at 9 degrees/update.

The test measured the existing Level 1 directed pivot at approximately 156.22 degrees: 20 updates at 8 degrees/update versus 10 updates at the experimental 16 degrees/update. A focused test also verifies the unchanged 24-tick pause and that below-rim cargo movement still uses the original 8-degree turn and 2-unit climb rates.

## Deterministic pressure-model comparison

The preserved analysis-only scheduler uses real C# Flipper/Shot movement with accepted baseline collision ordering. It is a fixed-position, deterministic fixture, not a full wave or arcade-RNG simulation. Results are observations, not required gameplay outcomes:

| Scenario | Baseline: 8 deg/update | Experiment: 16 deg/update |
|---|---|---|
| 1 pursuer, held fire | 600 ticks; cleared without death (91 shots accepted) | 600 ticks; cleared without death (91 accepted) |
| 2 pursuers, held fire | 600 ticks; cleared without death (92 accepted) | death at tick 137; 1 remained (21 accepted) |
| 4 pursuers, held fire | death at tick 265; 2 remained (42 accepted) | death at tick 137; 3 remained (21 accepted) |
| 4 pursuers, single-tap | death at tick 265; 2 remained (7 accepted) | death at tick 69; all 4 remained (2 accepted) |

This model suggests faster pursuit can materially change some crowded stationary fixtures, while a single pursuer remains clearable. It does not replace Neal's manual judgment or establish whole-wave balance.

## Validation

- Focused gameplay/regression selection: **633 passed, 0 failed, 0 skipped**. It covered rim pursuit, Level 1 movement, accepted shot/contact behavior, firing, Levels 2-7, Level 7 movement/progression, death, Super Zapper, and the preserved pressure analysis.
- `dotnet test TempestCSharp.slnx`: **758 passed, 0 failed, 0 skipped**.
- `dotnet build TempestCSharp.slnx`: succeeded, **0 warnings, 0 errors**.
- `git diff --check`: passed.

## Files and final state

Production changes:

- `src/Tempest.Game/Levels/LevelConfiguration.cs` — Level 1-only rim-turn override; other level configurations fall back to their existing turn speed.
- `src/Tempest.Game/Gameplay/Flipper.cs` — use the configured rim rate only while at rim height.

Test changes:

- `tests/Tempest.Game.Tests/RimPursuitTests.cs`
- `tests/Tempest.Game.Tests/DirectedRotationInvestigationTests.cs`
- `tests/Tempest.Game.Tests/PlayerDeathPresentationTests.cs`
- `tests/Tempest.Game.Tests/SimulationTests.cs`
- `tests/Tempest.Game.Tests/Level1StationaryRimCombatComparisonTests.cs` — preserved, untracked analysis harness.

The prompt and this report are saved under ignored `AI-Logs/`. No README, behavior specification, development notes, or reference files were changed. No Internet disassembly research was performed. The game was not launched. No files were staged; no commit or push was made.

Manual launch command for Neal:

```powershell
dotnet run --project src\Tempest.Game -- --lives 10
```

For Level 1, judge whether the faster rim pursuit is immediately visible and closer to the arcade's slow-climb/fast-rim contrast; whether single-tapping or held Fire still feels too safe; whether skilled temporary defense and careful shot timing remain possible; whether multiple Flippers create meaningful pressure; and whether anything feels unnaturally fast or unfair. Stop for Neal's manual test.
