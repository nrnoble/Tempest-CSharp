# Atari HUD Calibration Diagnostics Report

Date: 2026-11-20

## Scope

Added development-only CLI controls for independent Atari HUD field calibration.
The controls override only presentation constants and remain active across
level and playback presentation changes. No HUD values are written to gameplay
recordings.

Completion token: `TEMPEST_ATARI_HUD_CALIBRATION_DIAGNOSTICS`

## Changed Files

- `src/Tempest.Game/Debug/DebugController.cs`
  - Parses the 13 HUD options with separate values.
  - Rejects duplicates, malformed/non-finite values, invalid scales, separate
    `--option=value` syntax, and options without Atari presentation.
- `src/Tempest.Game/Rendering/AtariHudVector.cs`
  - Added `AtariHudCalibrationOptions`.
  - Added absolute per-field defaults and independent resolved overrides.
  - Preserved live formatting, glyph dictionaries, stable score right edge,
    reserve count/cap, and state behavior.
  - Kept the `38930`/`AAA` origin spacing proportional to the combined high
    score/initials group scale while preserving the 110-unit default offset.
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
  - Added constructor-side validation and passes HUD calibration to the Atari
    portrait HUD renderer.
- `src/Tempest.Game/Program.cs`
  - Parses and wires HUD controls and reports that they are development-only.
- `tests/Tempest.Game.Tests/AtariHudVectorTests.cs`
  - Added default, independence, extreme-geometry, and direct-construction
    coverage.
  - Added high-score/initials spacing coverage across group scale changes.
- `tests/Tempest.Game.Tests/DebugControllerTests.cs`
  - Added positive/negative/zero, scale, malformed, duplicate, context,
    and partial-override coverage.
- `AI-Logs/Prompts/2026-11-20-atari-hud-calibration-diagnostics-prompt.md`
  - Paired substantive task prompt.

## Current Coded Defaults

| Group | X | Y | Scale | Advance |
|---|---:|---:|---:|---:|
| Current score | right edge `220` | baseline `72` | `1.10` | - |
| High score/AAA | high start `270` | baseline `48` | `0.75` | - |
| Current level | start `270` | baseline `108` | `0.75` | - |
| Reserve lives | start `30` | baseline `132` | `0.60` | `22` |

Coordinates use the existing 600x800 portrait logical HUD system. Positive X
moves right and positive Y moves down.

## Validation

- PASS: focused HUD/CLI/presentation subset: 100/100.
- PASS: complete solution test suite: 1768/1768.
- PASS: Release solution build: 0 warnings, 0 errors.
- PASS: `git diff --check`.
- No interactive game process was launched.

## Manual Calibration Commands

Run from the repository root. Each probe bracket is deliberately wide.

### Current score

```powershell
dotnet run --project src\Tempest.Game -- --atari-presentation --level 2 --hud-score-x 180 --hud-score-y 72 --hud-score-scale 1.10
dotnet run --project src\Tempest.Game -- --atari-presentation --level 2 --hud-score-x 220 --hud-score-y 48 --hud-score-scale 0.75
dotnet run --project src\Tempest.Game -- --atari-presentation --level 2 --hud-score-x 260 --hud-score-y 96 --hud-score-scale 1.40
```

### High score/initials group

```powershell
dotnet run --project src\Tempest.Game -- --atari-presentation --level 2 --hud-high-x 220 --hud-high-y 48 --hud-high-scale 0.75
dotnet run --project src\Tempest.Game -- --atari-presentation --level 2 --hud-high-x 270 --hud-high-y 24 --hud-high-scale 0.50
dotnet run --project src\Tempest.Game -- --atari-presentation --level 2 --hud-high-x 320 --hud-high-y 72 --hud-high-scale 1.00
```

### Current level

```powershell
dotnet run --project src\Tempest.Game -- --atari-presentation --level 2 --hud-level-x 220 --hud-level-y 108 --hud-level-scale 0.75
dotnet run --project src\Tempest.Game -- --atari-presentation --level 2 --hud-level-x 270 --hud-level-y 84 --hud-level-scale 0.50
dotnet run --project src\Tempest.Game -- --atari-presentation --level 2 --hud-level-x 320 --hud-level-y 132 --hud-level-scale 1.00
```

### Reserve-life position/scale/advance

```powershell
dotnet run --project src\Tempest.Game -- --atari-presentation --level 2 --hud-lives-x 20 --hud-lives-y 132 --hud-lives-scale 0.60 --hud-lives-advance 22
dotnet run --project src\Tempest.Game -- --atari-presentation --level 2 --hud-lives-x 40 --hud-lives-y 108 --hud-lives-scale 0.90 --hud-lives-advance 14
dotnet run --project src\Tempest.Game -- --atari-presentation --level 2 --hud-lives-x 60 --hud-lives-y 156 --hud-lives-scale 1.20 --hud-lives-advance 34
```

### Combined example

```powershell
dotnet run --project src\Tempest.Game -- --atari-presentation --level 2 --hud-score-x 240 --hud-score-y 72 --hud-score-scale 1.10 --hud-high-x 280 --hud-high-y 48 --hud-high-scale 0.75 --hud-level-x 280 --hud-level-y 108 --hud-level-scale 0.75 --hud-lives-x 40 --hud-lives-y 132 --hud-lives-scale 0.75 --hud-lives-advance 26
```

### Reserve-life evaluation

```powershell
dotnet run --project src\Tempest.Game -- --atari-presentation --level 2 --lives 10
```

### Two-digit level evaluation

```powershell
dotnet run --project src\Tempest.Game -- --atari-presentation --level 10
```

### Visible playback

An established visible gameplay recording is present at
`TEMP/GameplayRecordings/Levels1-7.json`:

```powershell
dotnet run --project src\Tempest.Game -- --atari-presentation --playback-file TEMP\GameplayRecordings\Levels1-7.json --hud-score-x 220 --hud-score-y 72 --hud-score-scale 1.10 --hud-high-x 270 --hud-high-y 48 --hud-high-scale 0.75 --hud-level-x 270 --hud-level-y 108 --hud-level-scale 0.75 --hud-lives-x 30 --hud-lives-y 132 --hud-lives-scale 0.60 --hud-lives-advance 22
```

## Manual Acceptance Remaining

Neal should choose preferred values for score, high/AAA, level, and reserve-life
position/scale/spacing. No new production defaults are encoded by this task.
