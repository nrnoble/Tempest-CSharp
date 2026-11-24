Read this entire user-provided task before taking any action.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, files, commands, or taking repository action, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with `TEMPEST_` appears on a line by itself in the user-provided task.

If exactly one complete token cannot be found, stop before any project or repository action and explain why.

# Task: Add a general Atari-presentation depth-origin diagnostic for manual tube calibration

Continue from the current uncommitted Level 13 production candidate.

Use the completed Level 13 presentation investigation as the factual handoff:

`AI-Logs/Reports/2026-11-23-level13-presentation-divergence-report.md`

Do not repeat that investigation.

The investigation established that Level 13 exposes a degree of freedom that the current calibration controls cannot express:

- `--tube-y-gain` changes model-Y displacement;
- `--tube-scale` scales point displacement;
- `--tube-vertical-offset` applies a common final translation;
- none of those independently changes the shared Atari projection's depth-dependent model-origin drift.

Add a development-only command-line diagnostic that allows Neal to manually vary that depth-origin contribution.

Do not implement a Level-13-only command-line option.

The option must be generally available for supported Atari-presentation levels, with a neutral default that leaves all existing accepted presentation behavior unchanged.

## Objective

Provide one independent calibration control for the depth-dependent vertical origin displacement identified by the Level 13 investigation.

The control must allow experimentation with:

- no change from the existing Atari projection;
- reduced depth-origin drift;
- zeroed depth-origin drift;
- reversed or exaggerated drift when deliberately testing;
- arbitrary finite probe values.

Do not constrain the diagnostic to an aesthetic range.

Neal may deliberately test extreme values.

## Option naming

Inspect existing command-line naming conventions and choose one clear name.

Preferred semantic form:

`--tube-depth-y-gain <value>`

or an equally clear established-style name if the existing parser conventions strongly favor another term.

The report and CLI help must state exactly what the option controls.

Do not call it generic perspective if it only controls the depth-dependent vertical origin contribution.

Do not reuse `--tube-y-gain`.

## Neutral semantics

Define the diagnostic so that its neutral/default value reproduces the current Atari presentation exactly.

Preferred semantics:

- `1.0` = existing current Atari depth-origin behavior;
- `0.0` = remove the depth-dependent origin contribution while leaving the common placement and local geometry intact;
- values between 0 and 1 reduce the contribution;
- values greater than 1 exaggerate it;
- negative values reverse the contribution.

If the implementation architecture makes an additive formulation materially clearer, document the equivalent semantics precisely, but preserve an obvious neutral value.

Do not silently change existing presentation defaults.

## What the diagnostic scales

The Level 13 investigation identified the relevant depth-varying origin term associated with the converted camera-height contribution.

Apply the new diagnostic specifically to that depth-dependent contribution.

Do not indiscriminately scale:

- model Y geometry;
- near-rim shape;
- entire screen Y;
- signed common placement;
- final vertical offset;
- uniform tube scale;
- depth-size ratio;
- HUD;
- entity-local geometry.

At the near plane, where the depth-origin contribution is zero by construction, changing the new diagnostic should not move or reshape the near-rim silhouette except for unavoidable floating-point noise.

At increasing depth, it should alter the vertical origin displacement according to the specified gain.

## Preserve independent existing controls

The following controls must remain independent:

- `--tube-y-gain`
- `--tube-scale`
- `--tube-vertical-offset`
- the new depth-origin diagnostic

They must compose deterministically.

Changing one must not rewrite the stored value of another.

Partial command-line overrides must preserve all omitted defaults.

Do not remove or repurpose existing diagnostics.

## General availability

The new command-line option must be accepted for any level currently supported by Atari presentation.

Do not restrict parser acceptance to Level 13.

Do not create separate per-level CLI option names.

This is a general development diagnostic intended to remain useful when calibrating future Levels 14-16.

However:

- neutral/default behavior on all currently accepted Levels 1-12 must remain exactly unchanged;
- do not encode non-neutral defaults for any accepted level in this task;
- Level 13 also remains neutral by default until Neal manually finds and accepts a value.

The diagnostic being globally available does not mean every level needs a non-neutral value.

## Presentation-mode requirement

Require Atari presentation in the same manner as the other tube calibration controls.

If the option is supplied without the presentation mode required by the existing diagnostic architecture, reject it clearly before window creation.

Follow current project conventions rather than inventing a different validation model.

## Numeric validation

Accept finite signed invariant-culture values.

Allow:

- zero;
- positive values;
- negative values;
- large finite probe values.

Reject:

- missing value;
- malformed value;
- NaN;
- positive infinity;
- negative infinity;
- duplicate instances if existing diagnostic options reject duplicates.

Do not impose arbitrary aesthetic lower/upper limits.

Extreme values may make the tube visually distorted or move deep geometry outside the viewport. That is expected diagnostic behavior rather than automatically a project defect.

## Preserve depth-size ratio

This task changes only the depth-dependent vertical origin contribution.

Do not add a second diagnostic for the far/near local-size ratio in this task.

The Level 13 investigation identified that ratio as another independent difference, but its correct target is not established.

Neal first wants to determine how much correcting/reducing the vertical depth-origin drift improves Level 13.

Do not broaden the implementation.

## Tube/entity registration

Apply the same adjusted depth-origin mapping through the existing shared Atari projector used by:

- tube points;
- Blaster anchor;
- Flippers;
- Tankers;
- Spikers;
- Fuseballs;
- projectiles;
- gameplay-associated bonus popups;
- other attached tube entities.

Do not alter tube lines independently from entities.

The new diagnostic must preserve tube/entity registration at every depth.

HUD remains on its separate fixed presentation path and must not move.

## Normal Playing scope

The immediate Level 13 problem is normal Playing presentation.

Inspect current Atari projection reuse carefully.

Prefer applying the diagnostic through the shared normal Atari world-point mapping where that preserves consistency.

Do not silently change Approach or ExitZoom diagnostic behavior unless they already consume the exact same calibrated world mapping by design.

Report clearly which presentation states the diagnostic affects.

Do not redesign Approach or ExitZoom.

Do not reopen the deferred Level 12 ExitZoom problem.

## Level 13 calibration usage

After implementation, Neal will manually test Level 13 using the new option.

Provide a small set of widely separated probe values intended to reveal direction and useful range quickly.

Suggested initial probes should include at least:

- `1.0` current behavior;
- `0.75`;
- `0.5`;
- `0.25`;
- `0.0`.

Optionally suggest one negative or greater-than-one diagnostic probe if useful for confirming direction.

These are probe recommendations only, not parser limits.

Do not automatically choose a Level 13 default.

Do not screenshot-fit a value.

## Preserve source evidence

Do not modify genuine Level 13 geometry.

Preserve:

- shape row 14;
- ordered points;
- closed topology;
- lane count;
- gameplay orientation;
- spinner behavior;
- ROM metadata;
- model-frame mapping.

Do not change raw camera-height metadata.

The diagnostic scales the C# PROJECT TRANSLATION contribution derived from that metadata; it does not rewrite the evidence.

## Preserve accepted levels

At neutral/default value, verify outputs for Levels 1-12 remain unchanged.

Preserve direct Level 15 at neutral/default value.

Do not recalibrate accepted levels.

Do not claim those levels need this option.

## Automated tests: parser

Verify:

1. option omitted -> neutral current behavior;
2. `1.0` -> current behavior;
3. `0.0` accepted;
4. positive finite values accepted;
5. negative finite values accepted;
6. large finite probes accepted;
7. malformed rejected;
8. non-finite rejected;
9. missing value rejected;
10. duplicate option rejected where appropriate;
11. option without required Atari presentation rejected if that matches existing diagnostic conventions.

## Automated tests: projection semantics

Using representative genuine production geometry, verify:

1. near-plane zero-coordinate anchor is unchanged across different depth-origin values;
2. near-rim local geometry remains unchanged;
3. far/deep zero-coordinate Y varies according to the new gain;
4. gain 1 reproduces the prior current output;
5. gain 0 removes only the depth-dependent camera-origin contribution;
6. signed common placement remains unchanged;
7. vertical offset remains independent;
8. Y gain remains independent;
9. tube scale remains independent;
10. far/near local-size ratio remains unchanged by this diagnostic;
11. X coordinates remain unchanged;
12. output remains deterministic and finite for reasonable/extreme finite probes.

## Automated tests: registration

Verify the same gain affects tube and attached entity anchors consistently.

At representative depths verify:

- tube point;
- player/Blaster point;
- Flipper point;
- Fuseball point or equivalent attached entity;
- projectile point where practical.

Do not require every entity class if the projector is proven shared centrally, but provide enough integration coverage to catch divergent rendering paths.

Verify HUD coordinates remain unchanged.

## Regression protection

Run coverage proving neutral behavior remains unchanged for:

- Levels 1-12 Atari presentation;
- Level 13 neutral candidate;
- direct Level 15;
- topology validation;
- spinner/lane state;
- Fuseball popup registration;
- Fuseball animation;
- open-rim Flipper correction;
- Approach;
- generic ExitZoom;
- playback determinism.

Do not change gameplay state.

## Validation

Run bounded:

1. parser/CLI tests;
2. new depth-origin projection tests;
3. calibration-composition tests;
4. entity-registration tests;
5. Level 13 presentation tests;
6. Levels 1-12 Atari presentation regressions;
7. Level 13 headless production soak;
8. direct Level 15 preservation;
9. HUD tests;
10. Fuseball bonus/vector tests;
11. playback/determinism tests;
12. Approach/ExitZoom regressions;
13. complete solution test suite;
14. Release solution build;
15. `git diff --check`.

Use explicit hang/time bounds.

Do not launch the interactive game.

## Manual handoff

Provide Level 13 commands using:

- neutral `1.0`;
- `0.75`;
- `0.5`;
- `0.25`;
- `0.0`.

Combine with existing calibration controls only when useful.

Use Level 13's current neutral baseline for other controls initially.

Explain:

- smaller values reduce the depth-dependent downward drift;
- 0 removes that contribution;
- 1 preserves current behavior;
- this does not alter the far/near perspective scale ratio.

Neal will manually compare the resulting Level 13 composition against MAME.

Do not claim acceptance.

## Records and Git

Save this prompt and paired implementation report under tracked AI-Logs.

Do not stage, commit, or push.

Preserve the active Level 13 production candidate and unrelated tracked/untracked material.

Do not reset, restore, stash, clean, or rebase.

Stop after the diagnostic implementation, automated validation, and manual calibration handoff.

TEMPEST_TUBE_DEPTH_Y_GAIN_DIAGNOSTIC
