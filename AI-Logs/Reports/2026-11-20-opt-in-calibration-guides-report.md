# Opt-in calibration guides implementation

## Scope and result

Implemented `--calibration-guides` as a development-only Atari-presentation
option, off by default. It requires `--atari-presentation`, rejects duplicate
or valued forms, and is compatible with gameplay recording and file playback.
The existing guide drawing remains unchanged: yellow lines span the portrait
viewport at host Y=180 and Y=1210 after tube transforms and before the
screenshot callback. Both normal and Game Over animation paths use the same
gated submission. Pause and screenshot controls and naming were not changed.

The Level 1-10 first-pass calibration table and all three existing independent
diagnostic overrides were preserved. Neal reports the encoded defaults passed
manual testing for all ten levels; these are accepted project decisions, not
ROM-backed geometry. This milestone does not claim new manual acceptance of
the default-off or opt-in guide behavior.

## Files changed for this task

- `src/Tempest.Game/Debug/DebugController.cs`: parse and validate the option;
  allow it in recording and playback argument processing.
- `src/Tempest.Game/Program.cs`: pass the parsed setting to the renderer.
- `src/Tempest.Game/Rendering/VectorRenderer.cs`: gate both guide paths.
- `tests/Tempest.Game.Tests/PortraitCalibrationGuideTests.cs`: cover defaults,
  parsing, file modes, pause/screenshot control compatibility, exact guides,
  and independent and combined tube overrides.
- `README.md` and `TEMPEST-BEHAVIOR-SPEC.md`: document the development-only
  opt-in and accepted first-pass calibration status.
- Paired prompt and this report under ignored `AI-Logs`.

The files above had pre-existing worktree changes or were untracked, except
the new prompt and report. Those changes were preserved. No visual assets
were inspected. No Git staging, commit, push, reset, restore, stash, clean,
or rebase was performed.

## Validation

- **PASS:** focused guide, calibration, override, screenshot, and input tests:
  201 tests (`dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj
  --no-restore --filter <focused selectors> --blame-hang-timeout 2min`).
- **PASS:** presentation, progression, playback, Approach, and ExitZoom
  regressions: 435 tests with the matching combined filter and
  `--blame-hang-timeout 2min`.
- **PASS:** complete solution suite: 1,728 tests, none skipped
  (`dotnet test TempestCSharp.slnx --no-build --no-restore
  --blame-hang-timeout 2min`).
- **PASS:** Release solution build, zero warnings and errors
  (`dotnet build TempestCSharp.slnx --configuration Release --no-restore`).
- **PASS:** `git diff --check` (Git emitted its existing README line-ending
  warning).

Headless tests cover submitted guide geometry and pause/capture control
compatibility, not actual Raylib framebuffer pixels. The game was not launched.
Manual verification of the on-screen and F10 screenshot appearance in both
default and opt-in modes remains for Neal.

## Manual handoff

```powershell
dotnet run --project src\Tempest.Game -- --atari-presentation --level 1
dotnet run --project src\Tempest.Game -- --atari-presentation --level 1 --calibration-guides
dotnet run --project src\Tempest.Game -- --atari-presentation --level 1 --calibration-guides --tube-y-gain 1.4 --tube-scale 0.8 --tube-vertical-offset 35
```

In each mode use F9 to pause and F10 to capture; verify guides are absent in
the first mode and present in the other modes. F12 remains native Raylib.

TEMPEST_OPT_IN_CALIBRATION_GUIDES
