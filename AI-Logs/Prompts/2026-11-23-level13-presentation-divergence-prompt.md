Read this entire user-provided task before taking any action.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, files, commands, images, or taking repository action, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with `TEMPEST_` appears on a line by itself in the user-provided task.

VS Code/agent-generated reminders, attachment metadata, current-editor context, or other system-generated information appearing after the user's token do not invalidate it.

If exactly one complete token cannot be found, stop before any project or repository action and explain why.

# Task: Read-only Level 13 legacy-vs-Atari presentation divergence investigation

Perform a tightly bounded READ-ONLY investigation.

Do not implement a correction.

Do not modify production source, tests, configuration, calibration defaults, durable documentation, gameplay, or Git state.

Level 13 gameplay implementation has passed automated validation but manual presentation acceptance remains pending.

## Manual observation

Neal reports:

- genuine Level 13 gameplay/tube geometry has been implemented;
- the Level 13 Atari-presentation tube is difficult to calibrate with the existing:
  - `--tube-y-gain`
  - `--tube-scale`
  - `--tube-vertical-offset`
- no practical calibration Neal has tried makes Atari presentation look sufficiently like MAME;
- the legacy Level 13 presentation looks **closer** to MAME than Atari presentation;
- legacy presentation still does not look correct;
- therefore do not treat legacy presentation itself as the target.

The concern is that Level 13 may expose a structural presentation/projection problem that calibration magnitudes alone cannot correct.

## Exact authorized visual references

You are explicitly authorized to inspect only these two image files:

### MAME reference

`UserData\Screenshots\Level-13\MAME\Level-13 MAME.002.png`

### Current C# Atari presentation

`UserData\Screenshots\Level-13\MAME\live-level-13-tick-000020-cs.png`

The C# screenshot uses the default/neutral Level 13 Atari-presentation calibration.

These exact paths are intentional.

Do not search for substitutes.

Do not enumerate other screenshot directories.

Do not inspect other UserData images, videos, recordings, Temp files, or visual assets.

The MAME image is authorized visual reference evidence for Level 13 composition.

The C# image is authorized evidence of the current Atari-presentation result.

## Do not request or require a legacy screenshot

A legacy screenshot has not been supplied.

Do not search for one.

Use source/math to determine what legacy projection does differently.

Neal's manual statement that legacy looks closer to MAME is valid comparative evidence, but do not invent pixel measurements for an unavailable legacy image.

## Primary question

Answer:

**Why does genuine Level 13 shape row 14 render substantially closer to the MAME reference under the legacy projection than under the current Atari-presentation path, and can the first structural divergence be corrected without changing gameplay geometry or accepted global presentation behavior?**

Do not begin by tuning constants.

## Preserve genuine Level 13 gameplay geometry

Treat the current Level 13 production geometry as the model under investigation.

Do not modify or reinterpret:

- displayed Level 13;
- shape-row mapping to row 14;
- ordered vertices;
- topology;
- lane count;
- gameplay geometry;
- orientation;
- spinner semantics.

First determine whether its mathematical silhouette is internally consistent with the ROM-derived row.

If there is evidence that row-14 extraction/translation itself is wrong, report that explicitly.

Do not silently repair it.

## Visual comparison: MAME vs current C# Atari

Inspect the two authorized screenshots carefully.

Compare only observable Level 13 composition characteristics such as:

- overall silhouette;
- near-rim outline;
- deep/far-end location;
- apparent depth;
- radial convergence;
- horizontal extent;
- vertical extent;
- center relationship between near rim and far/deep end;
- relative placement beneath the HUD;
- major protrusions/indentations;
- apparent orientation;
- whether corresponding structural features appear mirrored, rotated, compressed, stretched, or displaced.

Do not screenshot-fit coordinates.

Do not derive production constants from pixel measurements.

The images should identify **what kind of distortion exists**, while source/math should identify **why**.

## Source comparison: legacy projection

Trace the complete normal-Playing legacy projection of Level 13.

Determine:

- Level 13 source/model points supplied to legacy projection;
- geometry centering;
- depth mapping;
- perspective mapping;
- screen center;
- scale;
- vertical placement;
- near-rim transformation;
- far/deep-end transformation;
- entity registration.

Record enough representative numeric values to characterize the resulting transform.

Do not alter legacy projection.

## Source comparison: Atari presentation

Trace the complete normal-Playing Atari-presentation projection using neutral Level 13 calibration:

- Y gain `1`;
- tube scale `1`;
- vertical offset `0`.

Trace:

1. raw Level 13 production model points;
2. Atari model-frame selection;
3. comparison/model-frame mapping;
4. ROM presentation metadata;
5. portrait perspective mapping;
6. calibration application;
7. near-rim mapping;
8. deep/far-end mapping;
9. final screen translation;
10. viewport transform.

Determine the exact first stage where its geometry meaningfully diverges from the legacy result.

Do not merely compare final screen coordinates.

## A/B transform comparison

Using identical genuine Level 13 model points, compute representative outputs through:

### Path A

Legacy presentation.

### Path B

Atari presentation at neutral calibration.

Compare representative geometry including:

- topmost near-rim feature;
- bottommost near-rim feature;
- leftmost feature;
- rightmost feature;
- one or more distinctive Level 13 inward/notched features;
- center/far-end point or derived far-end reference;
- representative rail endpoints.

For each path report:

- transformed position;
- relation to presentation center;
- apparent scale/depth relationship.

Identify the FIRST operation that accounts for the major visual difference.

## Calibration-capability test

Determine mathematically whether existing Level 13 Atari calibration controls are capable of correcting the identified divergence.

Specifically evaluate the degrees of freedom provided by:

- Y gain;
- uniform tube scale;
- vertical offset.

Answer:

- Can these controls reproduce the needed structural relationship?
- Or does the mismatch require a transform that these controls cannot express?

Examples of non-calibration-correctable differences might include:

- orientation;
- rotation;
- shear;
- focal/perspective mapping;
- comparison-frame basis;
- centering around the wrong reference;
- anisotropic transformation in the wrong coordinate domain;
- far-end/near-rim relationship.

Do not introduce a new control in this investigation.

## Model-frame investigation

Pay particular attention to Level 13's Atari model-frame mapping.

Determine:

- which model frame/basis Level 13 uses;
- how that frame was selected;
- whether it is derived correctly for shape row 14;
- whether closed-tube construction interacts correctly with the frame;
- whether another accepted level uses the same frame logic;
- whether the frame assumes symmetry/centering that Level 13 geometry does not satisfy.

Do not copy another level's frame to see if it looks better.

Establish the mathematical mismatch first.

## ROM presentation metadata

Inspect the Level 13 ROM-backed presentation metadata already captured by the candidate.

Determine whether:

- scale;
- camera height;
- signed Y placement;
- Flipper scale;
- open/closed flag

are being interpreted through the established translation consistently.

Do not re-read ROM binaries unless current implementation/report evidence is genuinely inconsistent.

This should primarily be a C# transform-chain investigation.

## Why legacy looks closer

Give a precise explanation.

Possible forms include:

- legacy accidentally preserves Level 13's useful raw affine silhouette;
- Atari mapping uses an unsuitable model-frame basis;
- Atari depth perspective exaggerates the wrong axis;
- Atari centering uses a reference inappropriate for Level 13;
- another specific transform.

Do not conclude that legacy is globally more correct.

Explain only why it happens to be closer for Level 13.

## Accepted-level protection

Do not reopen Levels 1-12.

The whole-game Atari presentation and per-level calibrations were manually accepted on those levels.

Any future correction recommendation must be Level-13-specific unless direct mathematical evidence proves a shared defect requiring separate future consideration.

Do not change global projection merely to make Level 13 work.

## No ExitZoom work

This investigation concerns normal Level 13 Playing presentation only.

Do not investigate:

- ExitZoom;
- Approach;
- Level 12 deferred exit discrepancy.

Do not let transition behavior broaden this task.

## No gameplay work

Do not change or investigate:

- enemy mix;
- Flipper movement;
- Tanker cargo;
- Spikers;
- Fuseballs;
- Pulsars;
- Spike height;
- firing;
- spinner controls;
- score;
- lives;
- Super Zapper;
- wave completion.

Level 13 gameplay acceptance is separate.

## Deterministic numeric analysis

Use bounded calculations/tests/scripts if useful to compare transforms.

Do not permanently modify source.

Temporary analysis must be removed before completion.

Produce enough numeric evidence that the conclusion does not depend only on visual judgment.

## Scope classification

Classify the presentation problem as exactly one:

- LEVEL-13 MODEL-FRAME TRANSLATION DEFECT
- LEVEL-13 PRESENTATION-METADATA TRANSLATION DEFECT
- LEVEL-13 GEOMETRY TRANSLATION DEFECT
- EXISTING CALIBRATION CONTROLS ARE SUFFICIENT
- SHARED PRESENTATION LIMITATION EXPOSED BY LEVEL 13
- INCONCLUSIVE

Support the classification with source/math and the authorized images.

## Implementation readiness

Choose exactly one:

- READY FOR BOUNDED LEVEL-13 CORRECTION
- CALIBRATION ONLY — NO CODE CHANGE NEEDED
- NEEDS ONE SPECIFIC MISSING OBSERVABLE
- STILL UNRESOLVED

If READY:

Specify the smallest Level-13-specific correction.

It must:

- preserve gameplay/model geometry;
- preserve accepted Levels 1-12;
- preserve current calibration controls;
- remain deterministic;
- keep tube/entities registered.

Do not implement it.

If CALIBRATION ONLY:

Provide a mathematically justified calibration strategy rather than screenshot-fitted guesses.

If a specific missing observable is genuinely necessary, identify exactly one rather than starting another broad investigation.

## Suggested regression coverage

If correction-ready, specify tests proving:

- genuine Level 13 gameplay geometry unchanged;
- normal Playing silhouette correction;
- tube/entity registration;
- spinner/lane state unchanged;
- model-frame mapping deterministic;
- calibration controls still compose correctly;
- Levels 1-12 outputs unchanged;
- direct Level 15 unchanged;
- Approach and ExitZoom unchanged;
- HUD unchanged.

Do not implement tests during this read-only task.

## Records and repository protection

Save this prompt and paired investigation report under tracked AI-Logs.

Do not modify:

- source;
- tests;
- configuration;
- calibration defaults;
- durable documentation;
- reference files;
- authorized screenshots.

Do not stage, commit, or push.

Do not reset, restore, stash, clean, or rebase.

Preserve the current Level 13 candidate and unrelated tracked/untracked material.

Stop after the read-only Level 13 presentation investigation report.

TEMPEST_LEVEL13_PRESENTATION_DIVERGENCE_READONLY
