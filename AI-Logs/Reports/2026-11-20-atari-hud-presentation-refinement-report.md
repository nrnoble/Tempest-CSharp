# Atari HUD Presentation Refinement Report

Date: 2026-11-20

## Scope

This report records the bounded implementation of the requested Atari vector HUD
scale/layout refinement and portrait development-diagnostic relocation.

Completion token: `TEMPEST_ATARI_HUD_PRESENTATION_REFINEMENT`

## Changed Files

- `src/Tempest.Game/Rendering/AtariHudVector.cs`
  - Refined independent 600x800 portrait layout constants.
  - Enlarged reserve-life Blaster presentation scale.
  - Reduced and independently scaled current/high-score, initials, and level fields.
  - Fixed visible score glyph right justification at the explicit score-field right edge.
  - Preserved ROM-informed glyph dictionaries and Blaster base geometry.
- `src/Tempest.Game/Rendering/DisplayConfiguration.cs`
  - Added centralized host-coordinate constants for portrait development diagnostics.
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
  - Kept portrait HUD vector submissions in the portrait HUD camera.
  - Removed portrait-path FLIPPERS/ACTIVE and level-test development text from the portrait camera.
  - Added host-coordinate diagnostic rendering in the unused left application area.
  - Preserved the legacy HUD branch unchanged.
- `tests/Tempest.Game.Tests/AtariHudVectorTests.cs`
  - Added independent-scale, stable right-edge, and host-diagnostic placement coverage.

## Evidence and Decisions

- Prototype composition was inspected only from the two exact authorized
  `Protoype-2` image paths identified in the task.
- Layout values are explicit PROJECT TRANSLATION values in the 600x800 portrait
  HUD coordinate system.
- No ROM glyph or reserve-Blaster base geometry was redrawn.
- Existing Raylib RGB translations remain unchanged; no broad color calibration
  was performed.
- Tube presentation, playfield cameras, viewport size/centering, calibration,
  Approach, ExitZoom, entity registration, and gameplay geometry were not changed.

## Validation

- PASS: focused Atari HUD/layout/diagnostic tests: 18/18.
- PASS: relevant presentation/playback/screenshot/Approach/ExitZoom/portrait
  regressions: 650/650.
- PASS: complete solution test suite: 1746/1746.
- PASS: Release solution build: 0 warnings, 0 errors.
- PASS: `git diff --check`.
- Git state remains uncommitted and unstaged.

## Manual Acceptance Handoff

Use the existing verified CLI options:

1. Level 2 Atari presentation:

   `dotnet run --project src\Tempest.Game -- --atari-presentation --level 2`

2. Enlarged reserve-Blaster scale and spacing:

   `dotnet run --project src\Tempest.Game -- --atari-presentation --level 2 --lives 10`

3. Level 10 multi-digit level placement:

   `dotnet run --project src\Tempest.Game -- --atari-presentation --level 10`

4. Visible live score/level playback:

   `dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\levels1-7.json`

5. Diagnostic block visibility:

   The portrait development diagnostics are enabled by the existing wave
   diagnostic state; `--level2-test --lives 10` is an established Level 2
   diagnostic test option:

   `dotnet run --project src\Tempest.Game -- --atari-presentation --level2-test --lives 10`

Manual acceptance should verify reserve-Blaster size, score and field placement,
AAA and level placement, colors, left-area diagnostic relocation, and absence
of any tube or viewport movement.

## Unresolved

Manual visual acceptance remains with Neal. No gameplay, recording, screenshot,
legacy presentation, or tube behavior changes were made.
