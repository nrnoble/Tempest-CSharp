# Atari HUD Defaults, Digit 1, and Zero-Score Prompt

Date: 2026-11-20

## Scope

Encode the accepted Atari HUD defaults:

- score right edge 250
- score baseline Y 75
- score scale 1.5
- level baseline Y 75
- reserve-life baseline Y 100

Preserve all existing diagnostic overrides and unchanged HUD defaults.

Correct the HUD digit 1 to one visible vertical stroke while preserving the
ROM-informed glyph advance. Correct score formatting so a newly started or live
score of zero displays one rightmost 0 instead of a blank field.

Do not change tube/presentation, viewport, gameplay, scoring awards, state
transitions, or development diagnostics.

## Evidence and Constraints

- The accepted values are Neal's manual visual authority.
- Digit 1 is evaluated against the tracked DVG/disassembly evidence and
  Neal's required single-vertical-line appearance.
- Digit 1 correction uses ROM/DISASSEMBLY BACKED visible-segment evidence and
  PROJECT TRANSLATION to distinguish pen-up moves/advance from drawn strokes.
- Zero formatting is a renderer formatting correction only; the live
  `ScoreSystem.Score` state remains zero and is not reset outside existing
  game-state boundaries.
- Validation must remain automated and bounded; do not launch the interactive
  game for manual acceptance.

Completion token: `TEMPEST_ACCEPT_HUD_DEFAULTS_FIX_ONE_AND_ZERO`
