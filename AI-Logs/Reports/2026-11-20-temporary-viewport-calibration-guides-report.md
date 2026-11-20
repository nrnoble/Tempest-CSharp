# Temporary viewport calibration guides

## Scope and implementation

Added exactly two development-only yellow horizontal guides to the Atari presentation path. They are drawn after playfield/HUD projection and scissoring, at host Y=180 and Y=1210, from the first through last pixel of the centered portrait viewport. The same final-coordinate helper runs in normal and game-over frames, before screenshot capture and viewport border drawing. Existing tube diagnostics, screenshot naming/destination, viewport, presentation constants, gameplay, and recording behavior were not changed by this addition.

Changed for this task:

- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `tests/Tempest.Game.Tests/PortraitCalibrationGuideTests.cs`
- `AI-Logs/Prompts/2026-11-20-temporary-viewport-calibration-guides-prompt.md`

The pre-existing presentation-calibration candidate and unrelated worktree files were preserved. No visual assets were inspected. No files were staged or committed.

## Validation

- **PASS** — Focused guide, F9/F10, screenshot-capture, tube-diagnostic, and presentation regressions: 250 passed, 0 failed.
- **PASS** — Complete solution test suite: 1703 passed, 0 failed.
- **PASS** — Release solution build: 0 warnings, 0 errors.
- **PASS** — `git diff --check` (with the existing README LF/CRLF working-copy warning only).

## Remaining manual acceptance

Neal's visual calibration acceptance remains pending. Launch Level 2 and evaluate the fixed guides manually:

`dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation --level 2 --tube-y-gain <gain> --tube-scale <scale> --tube-vertical-offset <offset>`

Replace placeholders with values Neal chooses. Do not treat the guide coordinates as tube-placement rules.
