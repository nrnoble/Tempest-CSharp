# Level 15 production promotion report

## Status

**PASS — implementation and automated validation.** The already accepted
Level 15 configuration is now the next production level after Level 14 and
the current terminal level. Level 16 is not implemented. No interactive game
was launched; optional campaign-promotion observation remains available to
Neal and is not a new gameplay-fidelity acceptance requirement.

## Starting audit

- Branch `master`, HEAD `bff70ae3faa50c8bb85f9ccde0c6d5ef4f048e6c`,
  upstream `origin/master`, ahead/behind `0/0`; index and tracked worktree
  initially clean. This matches the final Level 15 closeout.
- The final-closeout prompt and report and five earlier Level 15 prompt/report
  pairs were already untracked post-push historical records. They were not
  modified. Untracked arcade reference and `temo/` material were preserved
  without opening their contents.
- No stage, commit, push, reset, restore, stash, clean, or rebase was performed.

## Implementation and evidence

- `LevelProgression` now links Level 14 to the same accepted Level 15
  configuration, renamed `Level15Production`; Level 15 has no next
  production level. Generic direct-start lookup remains available, and both
  startup selectors accept Level 15. The existing terminal completion remains
  on Level 15; an explicit restart follows the existing fresh-Level-1 rule.
- No Level 15 configuration value, wave policy, gameplay mechanic, geometry,
  projection, calibration, HUD, recording format, background opacity, or
  ExitZoom implementation was changed. The accepted Level 11 parameter
  baseline remains a separately documented project baseline, not a new claim
  about Level 15 ROM gameplay parameters.
- The existing row-10 geometry, raw `$A0` Spike and approximately `142.857`
  project-coordinate height, terminal-Spiker conversion, and presentation
  defaults `1.15 / 1 / 75 / 1` remain unchanged. The prior closeout records
  Neal's manual acceptance; this promotion does not establish additional
  arcade-fidelity acceptance.
- The actual Level 14-to-15 production path is exercised through wave
  completion, exit and Approach. Tests check score/lives carry, zapper
  recharge and flash reset, outgoing starfield identity, fresh wave and
  ledger, active HUD, and active Level 15 calibration. Continuing starts at
  Level 11 exercise the chain through terminal Level 15 for both selectors.
- A new deterministic production soak runs two seeded games for 1,200 ticks
  each, checking production initialization, admission, observed Tanker
  movement, each Tanker's two distinct cargo identities, lanes, original
  ledger conservation, `ValidateState`, player and enemy shots, reachable
  Flipper/Tanker/Spiker/Fuseball identities, and absence of Pulsars.
- During test development an initial soak assertion incorrectly assumed all
  Level 15 Flippers were Tanker cargo. **FAIL:** seed 1981 produced an
  ordinary Flipper at tick 123. Inspection of the existing Level 11 baseline
  picker confirmed ordinary Flippers are reachable; the test was corrected
  to verify their ledger identity without changing production behavior. The
  final soak passes.
- Version-2 Level 15 recording and playback, legacy-style version-2 input,
  presentation overrides and Level 15 background-opacity invariance are
  covered. No recordings were rewritten.

## Changed files

- `src/Tempest.Game/Levels/LevelProgression.cs` and
  `LevelConfiguration.cs`: chain and production configuration name.
- `src/Tempest.Game/Debug/DebugController.cs` and
  `GameplayStartupConfiguration.cs`: current eligibility and diagnostics.
- `src/Tempest.Game/Program.cs`, `TempestGame.cs`, and
  `Rendering/AtariPresentationCalibration.cs`: updated scope descriptions.
- Tests: `BackgroundOpacityOptionsTests.cs`, `DebugControllerTests.cs`,
  `DirectLevelStartupTests.cs`, `GameplayRecordingTests.cs`,
  `Level12ProductionTests.cs`, `Level13ProductionTests.cs`,
  `Level15TerminalSpikerTests.cs`, `Level15TubeCandidateTests.cs`,
  `Level8ProductionTests.cs`, `LevelConfigurationTests.cs`,
  `StartLevelTests.cs`, `TubeDepthYGainTests.cs`, and new
  `Level15ProductionSoakTests.cs` under `tests/Tempest.Game.Tests/`.
- `README.md`, `TEMPEST-BEHAVIOR-SPEC.md`, and
  `TEMPEST-DEVELOPMENT-NOTES.md`: current progression and historical
  checkpoint clarification, preserving evidence distinctions.
- This report and its paired prompt are new, untracked files in the tracked
  `AI-Logs` directories, intentionally not staged.

## Validation

VSTest/xUnit v2, .NET SDK 9.0.304, Release, `--no-restore`. Test commands use
the VSTest two-minute per-test hang limit; the bounded soak uses 1,200 ticks.

- **PASS:** focused promotion/enemy/HUD/presentation/recording/playback matrix,
  1,204 passed, 0 failed, 0 skipped:
  `dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --configuration Release --no-restore --filter 'FullyQualifiedName~Level15|FullyQualifiedName~Level14|FullyQualifiedName~Level13|FullyQualifiedName~Level12|FullyQualifiedName~Level11|FullyQualifiedName~Level10|FullyQualifiedName~Level9|FullyQualifiedName~LevelProgression|FullyQualifiedName~StartLevel|FullyQualifiedName~DirectLevelStartup|FullyQualifiedName~DebugController|FullyQualifiedName~Flipper|FullyQualifiedName~Tanker|FullyQualifiedName~Spiker|FullyQualifiedName~Fuseball|FullyQualifiedName~Atari|FullyQualifiedName~Hud|FullyQualifiedName~BackgroundOpacity|FullyQualifiedName~Recording|FullyQualifiedName~Playback|FullyQualifiedName~Tube' --blame-hang-timeout 2min --verbosity minimal`
- **PASS:** final complete solution suite, 1,973 passed, 0 failed, 0 skipped:
  `dotnet test TempestCSharp.slnx --configuration Release --no-restore --blame-hang-timeout 2min --verbosity minimal`.
  This includes the added Level 15 1,200-tick production soak, both startup
  selector/parser tests, earlier-level regressions, and the final additions
  for legacy-style Level 15 playback and background opacity.
- **PASS:** `dotnet build TempestCSharp.slnx --configuration Release --no-restore --verbosity minimal`:
  0 warnings, 0 errors.
- **PASS:** `git diff --check`. Git reported its existing LF-to-CRLF working
  copy warning for `README.md`; no whitespace error.
- No required validation was canceled, blocked, or timed out. Parser and
  production initialization tests exercise the requested startup options
  without launching the graphical application.

## Optional manual handoff

Neal may verify promotion without repeating the accepted Level 15 checklist:

```powershell
dotnet run --project src\Tempest.Game -- --start-level 14 --lives 10
dotnet run --project src\Tempest.Game -- --start-level 15 --lives 10
```

Observe Level 14 entering Level 15, Level 15's presentation/configuration
activating, and Level 15 otherwise matching the accepted direct-start version.
The known generic ExitZoom imperfection remains outside this milestone.
