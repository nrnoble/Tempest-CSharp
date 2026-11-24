# Background-only opacity correction

## Status

**Implementation and automated validation PASS.** The bounded desktop smoke
check passed for window initialization and process responsiveness. Visual
compositor behavior and Neal's manual acceptance remain **PENDING**.

The starting worktree contained this in-progress window-opacity candidate and
unrelated existing local changes. No unrelated material was altered, and no
files were staged or committed. Level 14 implementation and documentation
were not changed.

## Binding and implementation

The game targets .NET 9 and references Raylib-cs 8.1.0. The installed
`lib/net8.0/Raylib-cs.xml` documents `ConfigFlags.TransparentWindow` as “Set to
allow transparent framebuffer” and exposes `Raylib.SetConfigFlags`. A Release
compile and tests also verified the enum member is present in the installed
binding. The native runtime reported Raylib 6.0 with the GLFW/Win32 backend.

**Choice A — replace whole-window opacity.** Neal's manual result showed the
intermediate `--window-opacity` fades every Tempest visual, which defeats the
overlay requirement. The option, `SetWindowOpacity` application path, and its
test were removed rather than retaining an unrelated whole-window control.

`--background-opacity` accepts finite invariant-culture values from 0 through
1; the omitted and explicit-1 cases leave the existing window flags unchanged.
Only values below 1 request `ConfigFlags.TransparentWindow`, before
`Raylib.InitWindow`. The renderer's established clear stage retains black RGB
and uses the configured alpha; all subsequent vector submissions keep their
existing colors, alpha, projection, and order. The standalone input diagnostic
uses the same startup option and clear behavior. The option is rejected for
headless playback and is not recording data.

The current screenshot path remains `Raylib.TakeScreenshot`, called by the
existing capture callback before `EndDrawing`; Raylib's binding documentation
describes this as a current-screen capture and `LoadImageFromScreen` as a
screen-buffer read. A disposable probe using the installed binding/runtime
cleared a transparent framebuffer, saved it through `TakeScreenshot`, and
read the PNG back: the tested clear pixel had alpha 255. Thus on this tested
Windows runtime, screenshots remain opaque Tempest-only images even when the
clear alpha is 0; they do not capture or composite the desktop behind the
window. The game screenshot architecture was not changed.

## Changed files

- `src/Tempest.Game/Debug/BackgroundOpacityOptions.cs` — startup setting,
  transparent-framebuffer lifecycle seam, and black clear color.
- `src/Tempest.Game/Debug/DebugController.cs` and
  `src/Tempest.Game/Debug/HeadlessGameplayPlaybackCli.cs` — parser, option
  compatibility, and headless rejection.
- `src/Tempest.Game/Program.cs` and
  `src/Tempest.Game/Debug/InputDiagnostic.cs` — set flags before window
  creation and pass the background clear color.
- `src/Tempest.Game/Rendering/VectorRenderer.cs` — alpha-controlled
  background clear only.
- `tests/Tempest.Game.Tests/BackgroundOpacityOptionsTests.cs` — parser,
  lifecycle, color, vector geometry/color, and model-state coverage.
- `README.md` — option and observed screenshot behavior.
- `AI-Logs/Prompts/2026-11-24-background-opacity-correction-prompt.md` and
  this report.

## Validation

- **PASS** — Focused `BackgroundOpacityOptionsTests`: 21 passed, 0 failed.
- **PASS** — Complete Release solution suite:
  `dotnet test TempestCSharp.slnx --configuration Release --no-restore
  --blame-hang-timeout 3m --verbosity minimal`; 1,959 passed, 0 failed,
  0 skipped.
- **PASS** — Release solution build: 0 warnings, 0 errors.
- **PASS** — `git diff --check` (Git emitted only the existing README
  LF-to-CRLF working-copy warning).
- **PASS** — Bounded graphical smoke with
  `--level 14 --atari-presentation --background-opacity 0.5`: native window
  handle was nonzero, process reported responsive, CPU time increased over
  the observation interval, and no exception was reported. The process was
  explicitly stopped. This establishes process health, not visual
  transparency or vector opacity.
- **PASS** — Screenshot-alpha probe on the installed Raylib 6.0 desktop
  runtime: a fully transparent clear was saved with alpha 255 in the PNG.
  The probe was temporary and is not part of the repository.

The first focused run exposed an ambiguous `Color` overload; after making the
test's byte overload explicit, the next run found that Raylib-cs quantizes
0.5 to alpha 127. Tests now assert the actual binding behavior. Both issues
were corrected before the final passing suite.

## Manual acceptance pending

Use the following from the repository root:

```powershell
dotnet run --project src\Tempest.Game -- --level 14 --atari-presentation --background-opacity 1
dotnet run --project src\Tempest.Game -- --level 14 --atari-presentation --background-opacity 0.5 --calibration-guides --tube-y-gain 1 --tube-scale 1.1 --tube-vertical-offset 200 --tube-depth-y-gain 1
dotnet run --project src\Tempest.Game -- --level 14 --atari-presentation --background-opacity 0 --calibration-guides --tube-y-gain 1 --tube-scale 1.1 --tube-vertical-offset 200 --tube-depth-y-gain 1
```

Neal must verify that the MAME/desktop reference is visible through the
background; tube, HUD, Blaster, enemies, shots, effects, and calibration
guides stay solid; geometry and input remain unchanged; the overlay is useful;
and F10 screenshots are acceptable as opaque Tempest-only output. Do not
claim arcade or visual acceptance before this check. Ordinary windowed mode
is the target; fullscreen/borderless behavior remains unverified.

No staging, commit, or push was performed. No Level 15 work was started.
