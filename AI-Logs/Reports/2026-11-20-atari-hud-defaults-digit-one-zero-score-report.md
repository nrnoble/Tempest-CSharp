# Atari HUD Defaults, Digit 1, and Zero-Score Report

Date: 2026-11-20

## Scope

This report records the bounded implementation of Neal's accepted Atari HUD
defaults and two HUD defects:

- Encode score right edge 250, score baseline Y 75, score scale 1.5,
  level baseline Y 75, and reserve-life baseline Y 100.
- Correct digit 1 to one visible vertical stroke.
- Display one rightmost 0 for a zero score while preserving right-justified
  positive-score formatting.

Completion token: `TEMPEST_ACCEPT_HUD_DEFAULTS_FIX_ONE_AND_ZERO`

## Changed Files

- `src/Tempest.Game/Rendering/AtariHudVector.cs`
  - Encoded the five accepted default values.
  - Changed the digit-1 glyph to one 8,0-to-8,24 vertical segment.
  - Changed zero score formatting to five blanks followed by one 0.
- `tests/Tempest.Game.Tests/AtariHudVectorTests.cs`
  - Added exact accepted-default assertions.
  - Added positive-score right-justification coverage.
  - Added digit-1 single-vertical-segment coverage.

## Evidence and Decisions

- The accepted layout values are PROJECT TRANSLATION values manually accepted
  by Neal and apply as exact coded defaults.
- The tracked Atari disassembly places digit 1 at ROM address/subroutine:
  `316c 4004 vsdraw x=+8 y=+0 z=off`, `316e 4cc0 vsdraw x=+0 y=+24 z=12`,
  `3170 5408 vsdraw x=+16 y=-24 z=off`.
- The vertical draw at `316e` is ROM/DISASSEMBLY BACKED.
- Separating the pen-up moves/character advance from the visible segment and
  correcting the C# glyph are PROJECT TRANSLATION behavior.
- Neal's requirement for exactly one vertical visible line supplies the manual
  visual authority.
- New-game score state was already initialized through `ScoreSystem.Score = 0`
  by the existing `ScoreSystem` default and `Reset()` behavior. Only score
  formatting was changed; no game-state initialization or reset boundary was
  altered.
- Existing diagnostic options remain absolute partial overrides. All
  omitted dimensions continue to resolve to the constants above.

## Validation

- PASS: focused Atari HUD and debug-controller tests: 105/105.
- PASS: `git diff --check`.
- No interactive game process was launched.
- The implementation candidates build as part of the focused test run.

## Manual Acceptance Handoff

Neal should use the exact accepted default configuration:

```powershell
dotnet run --project src\Tempest.Game -- --atari-presentation --level 2
```

Verify:

- score field at the accepted right edge and baseline
- score scale 1.5
- level baseline Y 75
- reserve-life baseline Y 100
- digit 1 is one vertical line
- new-game score displays as rightmost 0
- positive scores grow leftward without extra leading zeroes

No tube/presentation, viewport, gameplay, scoring, or diagnostic relocation
changes were made.
