# Enable Existing Four-Phase Fuseball Animation in Production Gameplay

Date: 2026-11-22

## Scope

Implement a tightly bounded rendering-only correction for the existing four-phase Fuseball visual model.

The completed read-only handoff is `AI-Logs/Reports/2026-11-22-fuseball-readonly-investigation-report.md`.
Do not repeat the visual or ROM investigation.

## Objective

Advance the existing shared Fuseball visual counter during normal active production gameplay.

Preserve:
- the existing modulo-16 counter;
- the existing divide-by-4 phase mapping;
- the four existing geometry records;
- the five Fuseball color groups;
- the existing 25 vector-line submissions;
- Fuseball gameplay state and behavior;
- FuseballTest/Hurry-Up behavior;
- F9 pause semantics and deterministic simulation-step timing;
- restart and deterministic playback behavior;
- legacy and Atari presentation phase selection;
- Level 11 and all unrelated behavior.

Do not introduce a new animation counter, wall-clock timer, speed option, timing diagnostic,
new geometry, or gameplay-recording data field.

The cadence is a PROJECT TRANSLATION, not established exact arcade cadence.

## Activation

Remove the FuseballTest-only advancement gate and advance the existing shared visual clock
exactly once per qualifying active simulation step. Prefer a shared global clock that continues
through inactive periods within active play. Do not advance while active gameplay entities are
genuinely frozen, including the established pause/death behavior.

## Testing

Add focused regression coverage for production and test-mode animation, phase wrap and order,
four-tick cadence, 25-line rendering, five colors, unchanged geometry, Fuseball gameplay invariants,
pause/death freezing, restart/direct startup, deterministic playback, presentation consistency,
and Level 11 behavior.

Do not use screenshots or golden-image tests.

## Validation

Run new Fuseball production-animation tests, existing rendering/FuseballTest/Hurry-Up tests,
terminal movement, identity, Level 11, rendering/presentation, playback/determinism tests,
the complete solution test suite, release build, and `git diff --check`.

Do not launch the interactive game.

## Documentation and Git

Save a paired implementation report under `AI-Logs/Reports/`.
Do not update durable behavior documentation before manual acceptance.
Do not stage, commit, push, reset, restore, clean, stash, or rebase.

TEMPEST_FUSEBALL_PRODUCTION_VISUAL_ANIMATION
