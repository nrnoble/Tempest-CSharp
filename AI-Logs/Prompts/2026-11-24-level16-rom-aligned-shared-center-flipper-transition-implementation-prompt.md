# Tempest Level 16 ROM-Aligned Shared-Center Flipper Transition Implementation

Implement a bounded correction for the Level 16 shared-center Flipper transition
crash, using the completed local ROM-backed investigation. The shared-center
investigation report is authoritative; read it and the three required prior
Level 16 reports completely before modifying code.

## Scope and constraints

- Correct both divergences: the C# path currently chooses the wrong half-turn
  for lane 0 -> 15, and requires every intermediate endpoint ray to intersect
  the tube polygon.
- Use Level 16 topology plus its ROM angle-row availability to scope the new
  transform. Preserve existing accepted transition behavior on Levels 1-15.
- Derive the ROM phase path and direction from lane-angle metadata and ROM
  direction semantics. Preserve `$0B -> $03` increasing modulo-16 for lane
  0 -> 15; cover its reverse and both directions at lanes 7 <-> 8.
- Keep Level 16 geometry/order, `$24` route, Tanker cargo, gameplay timing and
  all other gameplay state unchanged. Do not add a Level-16-only Atari rule,
  redesign enemy glyphs, or require polygon-boundary intersections for
  qualifying transitions.
- Do not suppress exceptions, fabricate intersections, use piecewise lobes,
  collapse/re-expand, alter Level 15, calibrate presentation, implement Level
  17/recycling, launch the game, stage, commit, or push.
- Classify the C# glyph mapping and progress mapping as PROJECT TRANSLATIONS.
  Do not claim eight visible Atari frames or manual visual acceptance.

## Validation and manual acceptance

Add a headless production-path regression combining Tanker cargo, the real
Flipper factory/configuration, `$24` movement, transition state, and
`VectorRenderer` projection. Add phase/direction tests, both shared junctions
in both directions, bounded all-directed-transition coverage, Level 16
Tanker/cargo tests, geometry/rendering tests, prior-level transition
regressions, the full solution suite, Release build, and `git diff --check`.
Use bounded test execution. Stop for Neal's manual retest; do not launch the
interactive game. The exact retest command is:

`dotnet run --project src\Tempest.Game -- --start-level 16 --lives 12`

Keep required AI-Logs unstaged. Do not modify previous AI reports. Update the
durable behavior documentation only as needed to record the bounded rule and
evidence classification; do not mark Level 16 manually accepted.

TEMPEST_LEVEL16_ROM_ALIGNED_SHARED_CENTER_FLIPPER_TRANSITION_IMPLEMENTED
