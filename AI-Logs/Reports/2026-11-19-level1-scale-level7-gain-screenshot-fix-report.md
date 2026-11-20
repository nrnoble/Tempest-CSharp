# Implementation report: Level 1 scale, Level 7 gain, paused screenshots

## Result

Added `--level1-tube-scale` as an opt-in, positive-finite, Level 1-only uniform
presentation scale. Its default is 1.0; it scales the Level 1 cross-section
about its projected center and scales local attached presentation through the
shared renderer path. It composes with `--tube-y-gain`, preserving the Y-gain
oval proportion. Neither diagnostic is stored in gameplay recordings or
changes simulation/model geometry.

Removed the former 1.32 upper boundary from `--tube-y-gain`; finite values
above it are accepted, while malformed, non-finite, and values below 1 remain
rejected. No production calibration value was selected.

## Screenshot investigation

The established code and project history distinguish three controls:

- F9 toggles pause during live play.
- F10 requests Tempest's live screenshot, including while paused.
- Ctrl+Shift+S requests screenshots during paused visible file playback.

Raylib's native F12 shortcut is not a Tempest capture command; it writes
`screenshotNNN.png` in the process working directory. Tempest's existing
capture helper instead reserves a collision-safe filename beneath the
development root's `UserData\Screenshots\` and passes Raylib a relative path.
The source-level cause of the wrong-destination screenshots is using native
F12 rather than the established project-owned capture control. No capture
system redesign or production screenshot-path change was needed. Added a
focused F9/F10 paused workflow regression and documented the distinction.

## Changed in this task

- `src/Tempest.Game/Debug/DebugController.cs` — parse/scope the new scale and
  remove the Y-gain aesthetic ceiling.
- `src/Tempest.Game/Program.cs` — wire diagnostics and validate playback level.
- `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs` and
  `src/Tempest.Game/Rendering/VectorRenderer.cs` — apply uniform scaling only
  to Level 1 projected geometry and attached presentation.
- `tests/Tempest.Game.Tests/TubeYGainTests.cs` — parser, projection, scope,
  registration, and simulation-equivalence coverage.
- `tests/Tempest.Game.Tests/PlaybackScreenshotCaptureTests.cs` — F9-pause/F10
  destination and repeated-filename coverage.
- `README.md` — distinguish live project screenshots from native F12.

The repository already had unrelated tracked and untracked changes at task
start, including candidate presentation edits and local/reference material.
Those were preserved; no files were staged or committed. Existing Approach and
ExitZoom behavior was not changed.

## Validation

- Focused Debug tests (`TubeYGainTests`, `PlaybackScreenshotCaptureTests`,
  `InputSystemTests`): **PASS**, 120 passed.
- Full solution tests: **PASS**, 1,647 passed, 0 failed, 0 skipped.
- Release solution build: **PASS**, 0 warnings, 0 errors.
- `git diff --check`: **PASS**. Git noted that the working README's LF line
  endings may be converted to CRLF on a future write; no whitespace errors.
- No interactive game launch or visual-asset inspection was performed.

## Manual acceptance still required

Run from the repository root. Recommended Level 1 probes (recommendations,
not parser limits):

```powershell
dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --level 1 --atari-presentation --tube-y-gain 1.32 --level1-tube-scale 0.5
dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --level 1 --atari-presentation --tube-y-gain 1.32 --level1-tube-scale 0.75
dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --level 1 --atari-presentation --tube-y-gain 1.32 --level1-tube-scale 1.0
```

For Level 7, compare widely spaced gains; finite values above these probes are
also accepted by the parser:

```powershell
dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --level 7 --atari-presentation --tube-y-gain 1.5
dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --level 7 --atari-presentation --tube-y-gain 2.5
dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --level 7 --atari-presentation --tube-y-gain 4
dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --level 7 --atari-presentation --tube-y-gain 8
```

For live screenshot verification, launch from the repository root, press F9 to
pause Level 1, then press F10 repeatedly. Confirm files appear under
`UserData\Screenshots\` with distinct incrementing suffixes at the unchanged
simulation tick. F12 is native Raylib behavior and is not the Tempest capture
command. For paused visible file playback, use the existing Ctrl+Shift+S
workflow. Manual visual/presentation acceptance remains pending Neal's review.
