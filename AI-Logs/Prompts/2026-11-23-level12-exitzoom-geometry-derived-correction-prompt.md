Read this entire user-provided task before taking any action.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, files, commands, or taking repository action, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with TEMPEST_ appears on a line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context after the user's token do not invalidate it.

If exactly one complete token cannot be found, stop before any project or repository action and explain why.

# Task: Implement the bounded Level 12 ExitZoom perspective correction

Continue from the current uncommitted production Level 12 candidate.

Use this completed read-only investigation as the factual handoff:

AI-Logs/Reports/2026-11-23-level12-exitzoom-calibration-coupling-investigation-report.md

Do not repeat the investigation.

Implement the smallest deterministic Level-12-only ExitZoom presentation correction supported by that report.

The correction must:

1. make the Level 12 exit view progress downward toward the deep end of the tube;
2. keep the complete Blaster visible throughout ExitZoom;
3. preserve tube/Blaster registration;
4. preserve gameplay and exit timing;
5. preserve accepted Levels 1-11;
6. avoid arbitrary per-frame screen-space offsets.

This is a presentation correction only.

## Manual evidence

Neal reports:

- Level 12’s current exit view appears straight forward.
- The arcade exit should visually follow downward toward the deep end of the tube.
- The Blaster should remain visible throughout the exit.
- The current implementation sends the Blaster outside the portrait viewport.
- Returning Level 12 calibration to identity values does not correct the fundamental exit-perspective problem.

The current good first-pass Level 12 Atari calibration remains:

- tube Y gain 1.3;
- tube scale 0.8;
- tube vertical offset 0.

Do not encode or alter those defaults in this task unless they are already represented as Level 12 defaults by the current candidate.

Do not use calibration changes as the ExitZoom correction.

## Established calibration coupling

Preserve the investigation’s established transform order.

### Tube Y gain

--tube-y-gain participates upstream in the mapped model Y displacement used by the ExitZoom opening/focal calculation.

### Tube scale

--tube-scale participates upstream in projection and therefore affects the mapped opening and focal calculation.

### Tube vertical offset

--tube-vertical-offset is applied after ExitZoom focal compensation as a common final Y translation.

Do not incorrectly treat all three controls as post-ExitZoom decoration.

Do not decouple Y gain or tube scale from ExitZoom merely to simplify this correction.

Do not alter their CLI semantics.

## Established root cause

The shared closed-tube ExitZoom path currently:

- maps the closed far-rim vertices;
- computes their arithmetic centroid;
- latches that centroid’s screen position at ExitZoom onset;
- applies translation so that centroid remains stationary;
- increases ExitZoom scale through K/ZOffset changes around a fixed screen-forward axis.

This stationary far-rim anchor is a PROJECT TRANSLATION, not a ROM-backed camera rule.

For Level 12:

- the player’s starting lane is substantially off the projection center;
- scalar expansion around the fixed axis magnifies that lateral displacement;
- the Blaster remains mathematically attached to the correct lane;
- but the complete Blaster glyph moves outside the portrait viewport;
- calibration values change the amount of displacement but do not correct the fixed-focal design.

Do not treat this as a separate Blaster registration bug.

Do not solve it by hiding, separately pinning, or independently translating only the Blaster.

## Scope classification

Treat the source architecture as a shared ExitZoom approximation that is visibly exposed by Level 12.

Implement a Level-12-only correction candidate.

Do not change accepted Level 5, Level 8, or Level 9 ExitZoom behavior.

Do not reopen Levels 1-11.

Do not make a global ExitZoom redesign in this task.

Structure the correction cleanly enough that the Level-12-specific policy remains explicit and testable rather than scattered through generic projection code.

## Required Level 12 exit behavior

During Level 12 ExitZoom:

- the visual path should progress downward toward the mapped deep end of row 12;
- the progression should be deterministic and continuous;
- the complete Blaster glyph should remain within the portrait playfield viewport;
- the Blaster should remain visually attached to its actual lane and rim position;
- tube geometry and attached entities should remain mutually registered;
- the visual path should be derived from genuine mapped Level 12 geometry and the current Blaster base;
- normal Level 12 Playing presentation must remain unchanged.

The correction should operate only while Level 12 is in ExitZoom.

Do not apply it during:

- ordinary Playing;
- Approach;
- normal tube rendering;
- death/re-entry;
- Game Over;
- another level’s ExitZoom.

## Geometry-derived focal/camera path

Develop the Level 12 ExitZoom path from existing production data:

- genuine row-12 closed geometry;
- mapped far-rim/deep-end geometry;
- Level 12 model frame;
- current player lane and edge position;
- current projected Blaster base;
- current ExitZoom progress/K/ZOffset;
- existing portrait viewport bounds.

Do not use screenshot measurements.

Do not hard-code a sequence of Y values.

Do not apply a manually authored per-frame translation table.

Do not merely replace the stationary centroid with another stationary point.

The path must visibly progress down-tube over the existing ExitZoom interval.

Use a normalized deterministic ExitZoom progress value derived from established state rather than wall-clock time.

Clearly document:

- what source geometry defines the deep-end direction;
- how the moving target progresses;
- how the Blaster visibility constraint is calculated;
- how tube and Blaster receive one coherent camera/presentation transform;
- where calibration enters the calculation.

## Blaster visibility constraint

The complete projected Blaster glyph must remain within the portrait playfield viewport throughout Level 12 ExitZoom.

Do not test only the Blaster base point.

Use the full rendered Blaster bounds or an equivalent conservative bound based on the actual submitted glyph geometry.

Preserve a small centralized safety inset only if required to prevent edge clipping.

Any safety inset must be:

- minimal;
- documented;
- deterministic;
- applied through the shared Level 12 ExitZoom camera/transform;
- not a separate Blaster-only adjustment.

Do not change Blaster size merely to keep it visible.

Do not clamp the Blaster’s model lane or gameplay movement.

Do not disable spinner movement during exit.

If the player moves along the rim during ExitZoom, the corrected presentation should remain coherent and keep the full Blaster visible within the extent supported by the Level 12 exit policy.

## Tube and Blaster coherence

Apply one coherent Level 12 ExitZoom presentation transform to:

- tube geometry;
- Blaster base;
- Blaster local glyph;
- attached tube entities where existing ExitZoom rendering includes them.

Do not create divergent focal paths for the tube and Blaster.

The correction may use the current Blaster base as an input to selecting the camera/focal path, but the output must remain a common presentation transform.

Preserve model-space registration.

## Preserve simulation and timing

Do not change:

- exit-state duration;
- ExitZoom K cadence;
- ExitZoom ZOffset cadence;
- player projected-depth progression;
- level completion timing;
- next-level transition timing;
- spinner simulation;
- lane state;
- collision;
- score;
- lives;
- Super Zapper;
- deterministic playback state;
- recording schema.

The existing simulation drives ExitZoom exactly as before.

Only Level 12’s visual projection/camera policy changes.

## Preserve Level 12 production evidence

Do not change:

- shape row 12;
- closed topology;
- 16 playable lanes;
- geometry ordering;
- ROM presentation metadata;
- corrected Continuous = true;
- enemy supply;
- wave policy;
- Flipper program;
- Tanker/Spiker/Fuseball behavior;
- Spike initialization;
- firing limits;
- HUD level 12.

Do not reintroduce Pulsars into Level 12.

Raw Pulsar table values must not be treated as proof of enemy eligibility before Level 17.

Do not modify enemy selection in this task.

## Preserve accepted global systems

Do not change:

- Levels 1-11;
- direct Level 15;
- Atari vector HUD;
- Fuseball production animation;
- Fuseball terminal movement;
- terminal Spiker-to-Tanker conversion;
- Blaster-death accepted baseline;
- screenshot behavior;
- pause behavior;
- calibration guides;
- playback/reverse playback;
- Approach.

Do not inspect screenshots, videos, UserData, Temp, or ROM files.

The source/report evidence is sufficient.

## Implementation architecture

Prefer a narrowly scoped policy/helper such as a Level 12 ExitZoom camera/focal strategy rather than adding repeated if level == 12 branches across projection methods.

The helper should consume explicit inputs and return the common presentation correction required for the current ExitZoom sample.

Keep generic ExitZoom behavior unchanged for all other levels.

Do not weaken or remove existing fail-closed model-frame/topology validation.

Keep comments maintainability-focused and classify the Level 12 path as a manually evaluated PROJECT TRANSLATION derived from genuine geometry.

## Automated tests: calibration coupling

Add focused tests proving:

1. Level 12 Y gain still participates upstream in mapped ExitZoom geometry.
2. Level 12 tube scale still participates upstream in mapped ExitZoom geometry.
3. Level 12 vertical offset remains a post-compensation common translation.
4. Identity calibration and 1.3 / .8 / 0 use the same Level 12 exit policy.
5. Calibration changes final geometry but does not switch or bypass the correction.
6. Model geometry and model-frame mapping remain unchanged.
7. HUD rendering remains independent of playfield calibration and ExitZoom.

Do not assert that calibration has no effect. The investigation proved otherwise.

## Automated tests: moving deep-end trajectory

Using genuine production Level 12 points, test several representative ExitZoom samples corresponding to:

- onset;
- end of the ZOffset phase;
- middle of the K phase;
- late ExitZoom.

Verify:

1. The corrected target/path is deterministic.
2. The corrected view progresses in the required down-tube direction.
3. The target is not stationary for the entire exit.
4. The path is derived from Level 12 geometry rather than arbitrary per-sample constants.
5. No discontinuity occurs at the ZOffset-to-K transition.
6. Tube points remain finite.
7. The closed far-rim structure remains coherent.
8. Repeated evaluation of the same state returns identical results.

## Automated tests: Blaster visibility

At the same representative samples, and across all valid Level 12 rim lanes where practical, verify:

1. The full Blaster glyph bounds remain inside the portrait playfield viewport.
2. The Blaster base remains attached to the projected active lane edge.
3. Tube and Blaster receive the same common Level 12 ExitZoom transform.
4. No independent Blaster-only screen translation is applied.
5. Calibration 1 / 1 / 0 keeps the Blaster visible.
6. Calibration 1.3 / .8 / 0 keeps the Blaster visible.
7. Tube vertical offset moves tube and Blaster together.
8. Spinner movement during ExitZoom preserves valid lane/edge state.
9. Blaster size/geometry remains unchanged.
10. No clipping occurs at the viewport safety inset.

Use exact production Blaster geometry or a conservative bound based on it.

## Automated tests: accepted-level preservation

Lock representative existing outputs for levels using the generic path.

At minimum verify:

1. Level 5 remains on the established generic closed-tube ExitZoom path.
2. Levels 8 and 9 remain on their accepted open-tube convergence path.
3. Genuine Level 11 remains unchanged.
4. Direct Level 15 remains unchanged.
5. No Level 1-11 calibration value changes.
6. Generic ExitZoomFocalCompensation behavior remains unchanged outside Level 12.
7. Unsupported future levels remain fail-closed where applicable.

Do not make new manual-acceptance claims for levels whose ExitZoom was not previously accepted.

## Automated tests: simulation preservation

Verify:

1. ExitZoom duration is unchanged.
2. K/ZOffset sequence is unchanged.
3. Player projected-depth sequence is unchanged.
4. Level 12 simulation state is equivalent with and without presentation.
5. Presentation correction does not mutate gameplay/model state.
6. Playback remains deterministic.
7. Reverse playback reconstructs the same Level 12 ExitZoom sample.
8. Normal Level 12 Playing projection is unchanged.
9. Approach projection is unchanged.
10. HUD projection is unchanged.

## Headless validation

Preserve and run the established Level 12 bounded headless production soak.

The visual correction itself may require projection-level tests rather than a headless renderer, but the gameplay soak must remain passing.

Do not replace manual visual acceptance with automated bounds tests.

## Validation

Run bounded:

1. New Level 12 ExitZoom camera/focal tests.
2. New full-Blaster visibility tests.
3. Calibration-coupling tests.
4. Existing ExitZoom focal-compensation tests.
5. Level 12 production/topology tests.
6. Level 12 headless production soak.
7. Spinner/input tests.
8. Atari presentation/model-frame/HUD tests.
9. Playback/reverse-playback determinism tests.
10. Levels 1-11 presentation regressions.
11. Direct Level 15 preservation tests.
12. Fuseball-animation regressions.
13. Blaster-death baseline regressions.
14. Complete solution test suite.
15. Release solution build.
16. git diff --check.

Use established explicit hang timeouts.

Do not weaken Windows security.

Do not enter same-artifact retry loops.

Do not launch the interactive game.

## Manual handoff

After automated validation provide:

dotnet run --project src\Tempest.Game -- --level 12 --atari-presentation

and:

dotnet run --project src\Tempest.Game -- --level 12 --atari-presentation --tube-y-gain 1.3 --tube-scale .8

and:

dotnet run --project src\Tempest.Game -- --level 12 --atari-presentation --tube-y-gain 1.3 --tube-scale .8 --calibration-guides

Neal must judge:

- the exit view now progresses downward toward the deep end;
- the complete Blaster remains visible throughout exit;
- the Blaster remains attached to the rim and responds normally;
- the tube remains coherent;
- no abrupt camera jump occurs;
- normal Playing presentation remains unchanged;
- 1.3 / .8 remains the preferred first-pass Level 12 calibration;
- accepted Levels 1-11 remain unaffected.

Do not encode a final Level 12 ExitZoom acceptance before Neal’s manual test.

## Records and Git

Save this substantial prompt and implementation report under the tracked AI-Logs convention.

Do not update durable behavior documentation before manual acceptance.

Do not stage, commit, or push.

Preserve unrelated tracked/untracked work, historical Blaster-death experiment records, and local/protected material.

Do not reset, restore, stash, clean, or rebase.

Stop after the bounded Level 12 ExitZoom correction, automated validation, and manual handoff.

TEMPEST_LEVEL12_EXITZOOM_GEOMETRY_DERIVED_CORRECTION