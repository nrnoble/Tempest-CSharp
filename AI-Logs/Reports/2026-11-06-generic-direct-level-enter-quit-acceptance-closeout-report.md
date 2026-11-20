# Generic Direct-Level and Enter-Quit Acceptance Closeout

Date: 2026-11-06

## Starting Git state

- Branch: `master`, tracking `origin/master`.
- Starting `HEAD` and `origin/master`: `8178429121febe5d543fcd16e546be0b75025c69`
  (`Correct Level 5 Flipper Spike behavior`).
- The index was clean. The only worktree changes were the expected direct-level/
  Enter-quit candidate: 10 modified tracked files and the two new test files.
  No unexplained untracked nonignored files were present.

## Manual acceptance and recorded behavior

Neal manually accepted:

- **GENERIC DIRECT-LEVEL STARTUP: MANUALLY ACCEPTED.**
- **CONTROLLER EXIT CONTROL: MANUALLY ACCEPTED.**

The accepted development-only syntax is `--level N`, for production Levels 1–7.
It selects the requested starting production configuration through the
production configuration/initialization path, starts fresh, honors `--lives`,
and does not make the selected level terminal. Normal production progression
continues after the selected level; Level 7 retains its existing terminal
campaign boundary because Level 8 is not implemented. This is scalable
level-by-level manual testing infrastructure, not arcade gameplay behavior.

V no longer has a production action. Enter/Return is the immediate
controller-friendly quit alias, without confirmation. Escape/window-close
behavior and all other controller mappings remain unchanged. The purpose is to
let Neal play and exit through the arcade controller without reaching for the
computer keyboard.

**Observed / unresolved / non-blocking / deferred:** During manual testing,
Neal observed multiple unexpected exits ending with
`INFO: Window closed successfully`. The initiating event is unknown; that
message does not establish a cause. A possible controller interaction, or a
combination involving ordinary Alt, Control, and Shift events, remains only a
hypothesis. The observation was not investigated. No logging, mapping changes,
or input diagnostics were added or run for it.

## Changed files

**Production**

- `src/Tempest.Game/Debug/DebugController.cs`
- `src/Tempest.Game/Levels/LevelProgression.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Systems/InputSystem.cs`
- `src/Tempest.Game/TempestGame.cs`

**Tests**

- `tests/Tempest.Game.Tests/DebugControllerTests.cs`
- `tests/Tempest.Game.Tests/DirectLevelStartupTests.cs`
- `tests/Tempest.Game.Tests/InputSystemTests.cs`
- `tests/Tempest.Game.Tests/LevelProgressionTests.cs`

**Documentation**

- `README.md`
- `TEMPEST-BEHAVIOR-SPEC.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`

The complete task prompt is archived at
`AI-Logs/Prompts/2026-11-06-generic-direct-level-enter-quit-acceptance-closeout-prompt.md`.
It was verified identical to the supplied task and has exactly one standalone
completion token. The prompt and this report are ignored AI-log records and are
not part of the commit.

## Validation

- Focused parser, direct-start, progression, input, and input-diagnostic tests:
  **70 passed, 0 failed, 0 skipped**.
- Neighboring Level 1–7, campaign/soak, death/re-entry, Level 5, Level 7,
  Super Zapper, rim pursuit, playback/recording, and production-input
  regressions: **654 passed, 0 failed, 0 skipped**.
- `dotnet test TempestCSharp.slnx`: **839 passed, 0 failed, 0 skipped**.
- `dotnet build TempestCSharp.slnx`: **0 warnings, 0 errors**.
- Invalid command-line smoke checks (`--level 8` and missing level value):
  each printed the supported range and exited with code 2 before game startup.
- `git diff --check` and staged `git diff --cached --check`: passed.
- No interactive game was launched.

## Staging, commit, and push

The complete staged file list was:

1. `README.md`
2. `TEMPEST-BEHAVIOR-SPEC.md`
3. `TEMPEST-DEVELOPMENT-NOTES.md`
4. `src/Tempest.Game/Debug/DebugController.cs`
5. `src/Tempest.Game/Levels/LevelProgression.cs`
6. `src/Tempest.Game/Program.cs`
7. `src/Tempest.Game/Systems/InputSystem.cs`
8. `src/Tempest.Game/TempestGame.cs`
9. `tests/Tempest.Game.Tests/DebugControllerTests.cs`
10. `tests/Tempest.Game.Tests/DirectLevelStartupTests.cs`
11. `tests/Tempest.Game.Tests/InputSystemTests.cs`
12. `tests/Tempest.Game.Tests/LevelProgressionTests.cs`

Commit: `8222acee9baf6e893499a7d6ddb859bd1b7e0903`

Message: `Add direct level startup and controller quit`

Push: successful, normal push to the existing `origin` remote.

Final local state: clean `master`, tracking `origin/master`.
`HEAD` and `origin/master` both resolve to
`8222acee9baf6e893499a7d6ddb859bd1b7e0903`.

No Level 8/9 work, unrelated gameplay/control changes, or investigation/fix of
the unexplained exits occurred.
