# Generic Direct-Level Startup and Enter Quit Report

Date: 2026-11-06

## Starting Git state

- Branch: `master`, tracking `origin/master`; worktree and index clean.
- Starting `HEAD` and `origin/master`:
  `8178429121febe5d543fcd16e546be0b75025c69`
  (`Correct Level 5 Flipper Spike behavior`).
- No unexplained untracked files were present.

## Existing mechanisms and implementation

- Existing direct selectors (`--level2-test`, `--level3-test`,
  `--level4-test`, `--level6-test`, `--level7-test`, and the Level 3 geometry
  diagnostic) remain isolated terminal diagnostics. Seeded playback also has
  Level 1 and Level 4 startup modes. Existing custom-configuration test
  construction deliberately did not enable production progression.
- Added `--level N` parsing and validation against the currently connected
  production progression chain. Valid levels are 1–7; duplicate, missing,
  nonnumeric, and unimplemented values fail before Raylib startup with exit
  code 2. Invalid startup was exercised for 0, -1, 8, 999, nonnumeric, and
  missing values; each printed the supported range and exited 2.
- Generic startup resolves the selected `LevelConfiguration` through
  `LevelProgression`, then uses `TempestGame.InitializeFreshWave`, the same
  production initializer used by normal play. No duplicate geometry, wave
  configuration, or test-only level was added.
- Fresh start uses the selected production level, requested/default lives,
  zero score, fresh Super Zapper charges/flash state, fresh wave and Spike
  state, empty active enemies/shots/bullets, no death or transition state,
  and no replay. Normal production behavior and level-specific eligibility
  remain configuration-driven. Progression stays enabled after direct start.
- A direct start is a starting point only. Successful exits continue through
  ordinary production progression. Reaching terminal Level 7 keeps the
  existing campaign boundary and fresh Level 1 restart behavior.
- `--level N` is exclusive with legacy scenario selectors and deterministic
  playback; `--lives` works before or after it. Legacy diagnostics and their
  restart behavior were retained.

## Exit controls and diagnostic behavior

- Before this change, `InputSystem` treated V as an immediate quit alias next
  to Raylib's window-close condition. Neal's physical controller has a button
  that emits ordinary V.
- Production quit now combines Enter's press edge with the unchanged
  `WindowShouldClose` condition. V has no production mapping or compensating
  gameplay action. Enter is checked at the main-loop boundary before a
  simulation step and follows existing cursor/audio/window cleanup.
- Escape's Raylib behavior and window-close behavior were not changed.
- The standalone input diagnostic remains opt-in and continues to observe
  raw V and Enter key events independently of production mappings. Its source
  and event loop were not changed.
- Prior documentation describing V as a quit alias was updated. Neal's
  manual acceptance of the new Enter/V production behavior is pending.

## Files changed

**Production**

- `src/Tempest.Game/Debug/DebugController.cs`
- `src/Tempest.Game/Levels/LevelProgression.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Systems/InputSystem.cs`
- `src/Tempest.Game/TempestGame.cs`

**Tests**

- Added `tests/Tempest.Game.Tests/DebugControllerTests.cs`.
- Added `tests/Tempest.Game.Tests/DirectLevelStartupTests.cs`.
- Updated `tests/Tempest.Game.Tests/InputSystemTests.cs`.
- Updated `tests/Tempest.Game.Tests/LevelProgressionTests.cs` to verify direct
  Level 4 -> 5, Level 5 -> 6, and Level 6 -> 7 progression as well as natural
  campaign progression.

**Documentation**

- Updated `README.md`, `TEMPEST-BEHAVIOR-SPEC.md`, and
  `TEMPEST-DEVELOPMENT-NOTES.md`.
- The exact supplied prompt is archived at
  `AI-Logs/Prompts/2026-11-06-generic-direct-level-and-enter-quit-prompt.md`.
  Its SHA-256 matches the read-only supplied attachment. Both AI log paths are
  ignored by Git.

## Validation

- Focused parser/direct-start/progression/input tests: **70 passed, 0 failed,
  0 skipped**.
- Neighboring gameplay and progression regressions (Levels 1–7, Level 5,
  Level 7 movement/cargo, campaign soak, death/re-entry, Super Zapper, rim
  pursuit, playback/recording, and production controls): **616 passed, 0
  failed, 0 skipped**.
- `dotnet test TempestCSharp.slnx`: **839 passed, 0 failed, 0 skipped**.
- `dotnet build TempestCSharp.slnx`: **0 warnings, 0 errors**.
- `git diff --check`: passed.
- No interactive game was launched.

## Manual commands and acceptance still required

```powershell
dotnet run --project src\Tempest.Game -- --lives 10
dotnet run --project src\Tempest.Game -- --level 1 --lives 10
dotnet run --project src\Tempest.Game -- --level 4 --lives 10
dotnet run --project src\Tempest.Game -- --level 5 --lives 10
dotnet run --project src\Tempest.Game -- --level 6 --lives 10
dotnet run --project src\Tempest.Game -- --level 7 --lives 10
```

Neal should verify normal Level 1 startup; production-equivalent direct Levels
5 and 7; Level 5 -> 6 progression; requested lives; V remaining inert; Enter
immediate quit from the controller; Escape/window close; spinner, Fire, Super
Zapper and other controller controls; and no visible gameplay regression.
Direct Level 7 should retain its accepted production geometry and behavior.

## Final state and scope

- `HEAD` remains
  `8178429121febe5d543fcd16e546be0b75025c69`; no files were staged.
- The tracked work consists only of the five production files, four test
  files, and three documentation files listed above. The two ignored AI log
  records do not appear in Git status.
- No Level 8/9 work, Level 1–7 gameplay retuning, staging, commit, or push
  occurred. Stop for Neal's manual infrastructure/control acceptance.
