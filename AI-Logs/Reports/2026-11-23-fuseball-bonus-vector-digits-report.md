# Fuseball Bonus Vector Digits Report

Date: 2026-11-23

## Scope

The task replaced conventional-font rendering of the Fuseball score popup only when Atari presentation is active. Legacy rendering remains unchanged. Scoring, popup lifetime, Fuseball state, projection, and tube calibration were not changed.

## Completion token

`TEMPEST_FUSEBALL_BONUS_VECTOR_DIGITS`

## Implementation

- Added an independent `AtariGlyphScale` derived from the existing popup scale in `FuseballScorePopup`.
- Extracted the popup-facing centered digit renderer on `AtariHudVector`, reusing the same private ROM-backed digit stroke table already used by the accepted Atari HUD.
- Kept the accepted HUD `AddGlyph` geometry and HUD rendering methods intact.
- Preserved digit 1 as the accepted single vertical segment.
- Added explicit decimal-only validation and deterministic finite-geometry checks for popup rendering.
- Added a presentation-mode branch in `VectorRenderer`: Atari presentation submits vector lines; legacy presentation retains `Raylib.DrawTextEx`.
- Projected the popup with the existing `ProjectLane` gameplay path, preserving its current edge, height/depth, and popup lifetime association.

## Changed files

- `src/Tempest.Game/Gameplay/FuseballScorePopup.cs`
- `src/Tempest.Game/Rendering/AtariHudVector.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `tests/Tempest.Game.Tests/AtariHudVectorTests.cs`
- `AI-Logs/Prompts/2026-11-23-fuseball-bonus-vector-digits-prompt.md`

## Validation

- PASS: focused Atari HUD/Fuseball filter, 101 tests passed.
- PASS: complete solution suite, 1,825 tests passed.
- PASS: Release solution build, 0 warnings and 0 errors.
- PASS: `git diff --check`.
- No interactive game launch was performed.

## Manual acceptance

Neal should run:

```powershell
dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation --level 11
```

Optional isolated Fuseball scenario:

```powershell
dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation --fuseball-test
```

Judge whether the bonus amount uses HUD-matched vector digits, remains readable and registered to the destroyed Fuseball position, appears/disappears normally, and does not change gameplay scoring.
