# Atari Vector Gameplay HUD Implementation Report

Date: 2026-11-20
Status: Implementation complete; manual acceptance pending
Completion token: `TEMPEST_ATARI_VECTOR_HUD_IMPLEMENTATION`

## Scope

Implemented the bounded first Atari-presentation gameplay HUD using deterministic vector
line submissions. The implementation adds reserve-life Blasters, live score, fixed high score
and initials, and live level geometry in the existing 600x800 portrait HUD coordinate system.

## Changed Files

- [AtariHudVector.cs](/C:/Users/nrnob/source/repos/Tempest/src/Tempest.Game/Rendering/AtariHudVector.cs)
  - Centralizes portrait HUD layout constants and RGB translations.
  - Holds the ROM/DISASSEMBLY BACKED digit 0-9 and uppercase-A DVG coordinate translation.
  - Implements fixed six-position score formatting, fixed 38930 and AAA text, two-position
    level formatting, reserve-life counting, and a 25-symbol display cap.
- [VectorRenderer.cs](/C:/Users/nrnob/source/repos/Tempest/src/Tempest.Game/Rendering/VectorRenderer.cs)
  - Portrait/experimental presentation draws the gameplay HUD as vector lines.
  - Legacy presentation retains the existing Raylib gameplay HUD text behavior.
- [AtariHudVectorTests.cs](/C:/Users/nrnob/source/repos/Tempest/tests/Tempest.Game.Tests/AtariHudVectorTests.cs)
  - Covers formatting, live state, deterministic finite geometry, reserve semantics, display
    capping, viewport bounds, colors, and state preservation.
- [2026-11-20-atari-vector-hud-implementation-prompt.md](/C:/Users/nrnob/source/repos/Tempest/AI-Logs/Prompts/2026-11-20-atari-vector-hud-implementation-prompt.md)
  - Paired substantive prompt record.

## Evidence and Design

- Digit and uppercase-A stroke coordinates are translated from the local
  `Reference/arcade/Tempest Rom Source code.md` DVG subroutines:
  ROM/DISASSEMBLY BACKED.
- Nominal reserve-life geometry follows the DVG Blaster subroutine identified by the report:
  ROM/DISASSEMBLY BACKED.
- Glyph scale, screen placement, reserve-life spacing, and Raylib RGB choices are PROJECT
  TRANSLATION choices for the bounded first implementation.
- The 25 reserve-symbol display cap is PROJECT DECISION. It is reached before the current
  600-wide portrait logical boundary for this implementation; no 25th symbol was drawn outside
  the viewport.
- No junction-brightness special case was added. It remains an unresolved monitor/artifact
  interpretation and is not an acceptance blocker.

## State and Overflow Policies

- Current score reads `game.Score.Score` live and uses the existing C# integer without mutation.
- For out-of-range or negative values, the score display uses the six least significant decimal
  positions: `magnitude % 1_000_000`, with leading positions blank.
- Level uses `CurrentLevel.Configuration.DisplayedLevel`; values use the last two decimal positions
  for a diagnostic or future overflow: `magnitude % 100`, displayed in a fixed two-character field.
- Reserve count is `max(totalLives - 1, 0)`, capped at 25 for drawing only. Underlying
  `Player.Lives` is never changed.

## Validation

- PASS: focused HUD/vector/presentation tests: 47 passed.
- PASS: complete solution test suite: 1743 passed.
- PASS: Release solution build: 0 warnings, 0 errors.
- PASS: `git diff --check`.
- The interactive game was not launched.

## Manual Handoff

Run established interactive direct-level and playback options:

```powershell
dotnet run --project src/Tempest.Game/Tempest.Game.csproj -- --atari-presentation --level 1
dotnet run --project src/Tempest.Game/Tempest.Game.csproj -- --atari-presentation --level 1 --lives 26
dotnet run --project src/Tempest.Game/Tempest.Game.csproj -- --atari-presentation --level 10
dotnet run --project src/Tempest.Game/Tempest.Game.csproj -- --atari-presentation --gameplay-playback
```

Manual acceptance must cover:

- glyph appearance and scale;
- live score right justification and leading blanks;
- fixed high-score and AAA spacing;
- live level placement;
- reserve-life geometry, spacing, and the 25-symbol cap;
- green, blue, and yellow target colors;
- overall upper-region composition;
- no tube movement or regression of the accepted presentation.

F9 and F10 behavior remains outside this implementation and the established controls remain
available. No production, calibration, tube, playback, scoring, lives, or progression behavior
was changed.
