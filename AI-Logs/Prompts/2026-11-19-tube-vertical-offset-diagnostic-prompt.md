# Task: Add an independent tube vertical-position diagnostic

Continue from the current uncommitted Level 1 / Level 7 presentation diagnostic candidate.

Manual testing has established that the new controls are extremely useful.

For Level 1:

`--tube-y-gain 1.32 --level1-tube-scale .8`

looks reasonably close. The shape/proportion and overall scale may eventually need fine tuning, but do not spend time fine tuning them in this task.

A new manual requirement has become clear: as tube Y gain and overall scale change, Neal needs to adjust the tube's vertical screen position independently.

Do **not** automatically reposition the tube based on scale or Y gain.

Instead, add an explicit development-only diagnostic:

`--tube-vertical-offset`

Its purpose is simple: move the complete presented tube up or down without changing its shape, size, perspective, or gameplay geometry.

## Required behavior

`--tube-vertical-offset` must:

- default to exact identity/no displacement when omitted;
- accept finite positive, negative, and zero values;
- reject malformed and non-finite values;
- have no arbitrary aesthetic numeric minimum or maximum;
- allow deliberately extreme finite values for diagnostic testing;
- operate independently from `--tube-y-gain`;
- operate independently from `--level1-tube-scale`;
- work for both Level 1 and Level 7 during the current diagnostic work;
- translate the complete tube presentation vertically as one registered presentation;
- keep the Blaster, enemies, shots, Spikes, Flippers, tube geometry, and other tube-attached presentation registered appropriately;
- remain presentation-only;
- not mutate model/gameplay geometry, simulation state, lane/depth state, movement, timing or collision behavior.

Positive versus negative direction may follow the project's existing screen/presentation coordinate convention. Clearly document the direction in the manual handoff.

## Critical separation of controls

Keep these three concepts independent:

1. `--tube-y-gain` changes the tube's vertical proportion/shape.
2. `--level1-tube-scale` uniformly changes Level 1 overall presented size while retaining the Y-gain-created proportion.
3. `--tube-vertical-offset` translates the resulting presented tube vertically without changing its shape or scale.

Do not automatically derive one value from another. Do not create automatic recentering behavior. The purpose is to let Neal manually determine the visually appropriate combination.

## Scope and compatibility

Do not recalibrate Level 1/7 Y gain, Level 1 scale, Level 7 inner/core relationship, core displacement, notch-to-core relationship, rail convergence, depth projection, portrait perspective, viewport dimensions, HUD layout, gameplay geometry, or mechanics. Do not select final production constants. The approximate Level 1 values are diagnostic observations only.

Do not inspect screenshots, videos, frames, comparison images, or other visual assets. Neal will perform visual calibration manually.

Preserve the established live screenshot workflow: F9 toggles pause, F10 captures Tempest-owned screenshots under `UserData\Screenshots`, collision-safe/incrementing names remain in use, and F12 is Raylib's separate native screenshot facility. Do not redesign capture or organize screenshots into subfolders.

Preserve existing validated presentation behavior, including Approach and ExitZoom. Implement only minimum coherent phase compatibility. Recording/playback must remain simulation-equivalent regardless of presentation values, and visual diagnostic values must not enter the gameplay recording format.

## Automated validation

Add focused coverage for identity defaults and zero, positive/negative/very large finite offsets, malformed/non-finite rejection, Level 1 combinations with Y gain and uniform scale, Level 7 combinations with Y gain, translation without shape/scale changes, entity registration, no model/simulation mutation, playback equivalence, unrelated-level behavior, Approach/ExitZoom, and existing screenshot behavior.

Run bounded focused tests, relevant targeted regressions, the complete solution test suite, Release build, and `git diff --check`. Follow project guidance for blocked/stalled validation; never weaken Windows security or retry the same blocked artifact in a loop.

Do not launch the game.

## Manual handoff

After automated validation, provide concise commands for Neal:

- Level 1 using approximately `--tube-y-gain 1.32 --level1-tube-scale .8`, combined with several widely spaced positive and negative `--tube-vertical-offset` probe values.
- Level 7 using freely adjustable `--tube-y-gain` values combined with several widely spaced positive and negative offset probes.

Probe values are recommendations only, not parser limits. Extreme finite values must remain testable without another code change.

Save this substantial prompt and an implementation report in the established ignored `AI-Logs` locations. Preserve all unrelated worktree changes and user/reference assets. Do not stage, commit, push, reset, restore, stash, clean, or rebase. Stop after automated validation and provide the manual-test commands.

TEMPEST_TUBE_VERTICAL_OFFSET_DIAGNOSTIC
