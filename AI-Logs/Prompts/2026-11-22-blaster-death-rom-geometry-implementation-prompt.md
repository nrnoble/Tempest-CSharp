# 2026-11-22 Blaster-Death ROM Geometry Implementation Prompt

## Scope

Perform the final bounded implementation of the ROM-derived Blaster-death
fragment geometry. Replace only the existing three-outline Blaster-death
renderer geometry, preserve the stepped stage schedule and 60-tick death
lifecycle, and do not change gameplay or presentation infrastructure.

## Requirements

- Use one immutable 24-segment ROM-derived local geometry table in original
  draw order, with exact endpoints and Atari color codes 9, 10, and 11.
- Omit the initial and final pen-up moves.
- Center raw coordinates using `(10,4)`, use positive Y up, invert Y for
  screen coordinates, and use centralized `13/17` coordinate scale.
- Preserve the stage order `1,2,3,4,5,6,4,2,1`; durations
  `6,6,6,6,6,12,9,6,3`; and centralized stage scale.
- Translate code 9 to the accepted C# yellow, code 11 to accepted orange-red,
  and code 10 to accepted white. Classify RGB mapping as PROJECT TRANSLATION.
- Remove the three 16-edge legacy starburst geometry exclusively used by this
  death path.
- Preserve death/re-entry/lives/Game Over/collision/playback and legacy/Atari
  presentation behavior.
- Add exact geometry, coordinate, color, and stage tests.
- Run bounded targeted and complete validation, release build, and
  `git diff --check`.
- Do not launch the interactive game, stage/commit/push, reset/restore/stash,
  inspect protected visual/reference assets beyond authorized handoff reports,
  or update durable behavior documentation before manual acceptance.

## Completion Token

TEMPEST_BLASTER_DEATH_ROM_GEOMETRY_IMPLEMENTATION
