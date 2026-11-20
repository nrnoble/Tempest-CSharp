# 1600x1280 Window / 960x1280 Viewport Implementation Report

## Status

Implementation candidate is ready for Neal's manual acceptance. Manual
acceptance is **PENDING**. No interactive game was launched, and no screenshot,
image, or video was inspected.

## Starting repository state

- Branch: `master`
- HEAD: `7ba80f39df9daeaa86a7cafc81690f2f5c37bb33`
- Upstream: `origin/master`
- Ahead/behind: `0 / 0`
- Index: clean
- Pre-existing worktree state preserved:
  - Modified `.github/copilot-instructions.md`
  - Untracked `Reference/arcade/MBUCOD.V05.txt`
  - Untracked `Reference/arcade/MBUDOC.DOC`

The starting commit matched the requested baseline. None of the pre-existing
files were edited, staged, or removed.

## Implementation

The authoritative configuration is `src/Tempest.Game/Rendering/DisplayConfiguration.cs`.

- Host window: `1280x960` -> `1600x1280`
- Portrait logical canvas: unchanged at `600x800`
- Portrait viewport: `720x960` -> `960x1280`
- Viewport origin: `(280,0)` -> `(320,0)`
- Margins: left/right `320`, top/bottom `0`
- Aspect ratio: remains exactly `3:4`
- Portrait output scale: `1.2` -> `1.6`
- Playfield zoom: `1.26` -> `1.68`
- Old-to-new viewport scale: exactly `4/3` on both axes
- Portrait camera host center: `(640,480)` -> `(800,640)`
- Portrait logical camera target and shared placement remain unchanged.

Logical coordinates, projection `K`, `ZOffset`, perspective, playfield scale,
per-level adapter inputs, tube geometry, and Level 7/8/9/10 logical placements
were not changed. Regression tests explicitly retain placements
`+228.99702`, `+212.28139`, `+180.96825`, and `+50`.

The legacy logical canvas remains `800x600` and uniformly scales by `2` to
`1600x1200`, centered vertically with a `40`-pixel offset in the `1600x1280`
window. It is not stretched to the host's different aspect ratio.

`--atari-presentation` remains an adapter opt-in; it does not select the
viewport or window size. Without that flag, the normal default remains portrait
rendering at the new size, without the Atari adapter. The separate
`--legacy-presentation` option selects the legacy 4:3 canvas described above.
All modes use the new host window dimensions.

The gameplay and HUD scissor rectangles continue to use the shared viewport
bounds, now `(320,0,960,1280)`. The blue border remains one host pixel and now
uses the centralized border-thickness setting with those same bounds.

Screenshot capture remains unchanged: the existing `Raylib.TakeScreenshot`
path captures the full window back buffer; there is no viewport-only capture
path to resize. Accordingly, a normal full-window capture follows the new
`1600x1280` window dimensions. No screenshot file was produced for this
validation.

Input remains unchanged and is acquired from mouse delta, keyboard, and
controller state; no absolute viewport-pixel mapping was introduced.
Gameplay, timing, recording schema, playback, progression, and transitions were
not modified.

## Files changed for this task

- `src/Tempest.Game/Rendering/DisplayConfiguration.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `tests/Tempest.Game.Tests/AtariPresentationAdapterTests.cs`
- `tests/Tempest.Game.Tests/DisplayConfigurationTests.cs`
- `tests/Tempest.Game.Tests/Level8AtariPresentationTests.cs`
- `tests/Tempest.Game.Tests/Level9AtariPresentationTests.cs`
- `tests/Tempest.Game.Tests/PreviousPortraitHostTransform.cs` (new test helper)

The presentation tests that compared old host-space pixel coordinates now
express the expected center-based `4/3` enlargement. Logical placement and
geometry assertions remain unchanged.

## Validation

All successful build/test commands used isolated output folders because a
pre-existing `Tempest.Game` process held the repository's Debug output files.
That process was not stopped or modified.

- **PASS** Focused display tests: 71 passed, 0 failed.
- **PASS** Targeted presentation/gameplay regressions: 879 passed, 0 failed.
  The selected classes covered Levels 1-10, Atari presentation and adapter
  behavior, Blaster scaling, ExitZoom/Approach/transitions, input, recording
  and playback, screenshot capture, legacy/option behavior, and display
  configuration.
- **PASS** Full solution tests with VSTest hang-timeout: 1,586 passed,
  0 failed, 0 skipped.
- **PASS** Release solution build: 0 warnings, 0 errors.
- **PASS** `git diff --check`.

The first normal Debug test invocation was **BLOCKED** by the already-running
`Tempest.Game.exe` (PID 65856), which locked `src/Tempest.Game/bin/Debug/net9.0`.
A no-apphost retry was also blocked because that process held
`Tempest.Game.dll`. Subsequent isolated-output test runs completed
successfully; the process was left untouched.

## Ending Git state

- Branch, HEAD, upstream, and ahead/behind remain unchanged from the starting
  state.
- No files were staged, committed, or pushed.
- The pre-existing `.github/copilot-instructions.md` modification and the two
  untracked arcade reference files remain untouched.
- Task source/test files are modified as listed above; the test helper is
  untracked.
- `AI-Logs/` is ignored and was not staged.

## Remaining manual acceptance

Run the requested normal command:

```powershell
dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation
```

Close any existing Tempest.Game window first; its Debug output was locked by a
pre-existing process during validation.

For bounded direct-level checks, use the same command with `--level 7`,
`--level 8`, `--level 9`, or diagnostic `--level 10`.

Judge only this sizing milestone: the window is larger, the portrait viewport
is centered with equal side margins and fills the host height, accepted tubes
and HUD are uniformly larger without reshaping or composition changes,
entities remain registered, gameplay and transitions remain unchanged, no
content clips or leaks into the side margins, and the one-pixel viewport border
is correct. Arcade/MAME screenshot-proportion matching remains a separate
future milestone and is not part of this acceptance.

## AI-Logs records

- Prompt: `AI-Logs/Prompts/2026-11-18-window-viewport-enlargement-prompt.md`
- Report: `AI-Logs/Reports/2026-11-18-window-viewport-enlargement-report.md`
