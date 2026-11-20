# Tube vertical-offset diagnostic implementation report

## Scope and implementation

Added the development-only `--tube-vertical-offset` control for Atari presentation on Levels 1 and 7. It defaults to zero, accepts any finite signed `float` (including `float.MaxValue`/`float.MinValue`), and rejects malformed or non-finite inputs. Like the current Y-gain and Level 1 scale controls, it requires `--atari-presentation` and a Level 1/7 start; playback validates against its saved starting level.

The renderer adds the offset after shared world-point projection and ExitZoom focal compensation. This translates tube points and attached presentation together, including during Approach/ExitZoom, without changing projection scale, Y gain, Level 1 scale, geometry, or simulation. No automatic recentering or recording-format changes were introduced. README documents the independent controls and screen-coordinate direction: positive moves down; negative moves up.

## Changed project files

- `src/Tempest.Game/Debug/DebugController.cs` — option model, finite signed parser, scope/playback validation, recording-option recognition.
- `src/Tempest.Game/Program.cs` — CLI parsing, playback scope validation, renderer wiring.
- `src/Tempest.Game/Rendering/VectorRenderer.cs` — presentation-only translation after ExitZoom compensation.
- `tests/Tempest.Game.Tests/TubeVerticalOffsetTests.cs` — parser, identity, translation/registration, phase, scope, model-state and playback coverage.
- `README.md` — diagnostic-control separation and coordinate direction.

## Validation

- **PASS** — focused vertical-offset tests: 27 passed.
- **PASS** — Y-gain, ExitZoom focal-compensation, and playback screenshot regressions: 106 passed.
- **PASS** — complete solution test suite: 1,674 passed, 0 failed.
- **PASS** — Release solution build: 0 warnings, 0 errors.
- **PASS** — `git diff --check`. Git emitted only its existing README LF-to-CRLF working-copy warning.
- The game was not launched. No screenshots, videos, frames, or other visual assets were inspected.

## Manual acceptance pending

Run from the repository root. Values below are recommended probes, not parser bounds. Positive values move the complete Level 1/7 presentation down; negative values move it up. Any finite single-precision offset remains accepted.

Level 1, retaining the approximate diagnostic gain and scale:

```powershell
dotnet run --project src\Tempest.Game -- --atari-presentation --level 1 --tube-y-gain 1.32 --level1-tube-scale .8 --tube-vertical-offset -320
dotnet run --project src\Tempest.Game -- --atari-presentation --level 1 --tube-y-gain 1.32 --level1-tube-scale .8 --tube-vertical-offset 0
dotnet run --project src\Tempest.Game -- --atari-presentation --level 1 --tube-y-gain 1.32 --level1-tube-scale .8 --tube-vertical-offset 320
```

Level 7, freely adjust `--tube-y-gain` independently:

```powershell
dotnet run --project src\Tempest.Game -- --atari-presentation --level 7 --tube-y-gain 1.2 --tube-vertical-offset -480
dotnet run --project src\Tempest.Game -- --atari-presentation --level 7 --tube-y-gain 1.4 --tube-vertical-offset 0
dotnet run --project src\Tempest.Game -- --atari-presentation --level 7 --tube-y-gain 1.6 --tube-vertical-offset 480
```

Neal's visual calibration and manual acceptance remain outstanding. No production constants have been selected.
