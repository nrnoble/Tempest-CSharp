READ THIS ENTIRE USER-PROVIDED TASK BEFORE TAKING ANY ACTION.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, reading files, running commands, inspecting images, or
taking repository action:

1. Locate the unique completion token near the end of this task.
2. Verify that exactly one complete token beginning with "TEMPEST_" appears on
   a line by itself.
3. Automatically appended VS Code/agent context after the token does not
   invalidate it.
4. If exactly one complete token cannot be found, STOP before project or
   repository action and explain why.

TASK

Perform a bounded READ-ONLY, IMAGE-GROUNDED matched comparison of the current
C# Atari-presentation tubes against the arcade/MAME reference tubes.

The four images attached to THIS task are the only authorized quantitative
visual evidence.

Ignore every comparison image supplied in any earlier chat or task.

Do not use earlier mistaken attachments.

AUTHORIZED IMAGES

Use exactly these four attachments:

1. Level-1 SideBySide.png
2. Level-2 sidebyside.png
3. level-5 Sidebyside.png
4. Level-7 SidebySide.png

For every attachment:

LEFT HALF = current C# presentation
RIGHT HALF = arcade/MAME visual reference

The filename/capture source identifies the reference only.

Do not investigate MAME implementation, source code, video settings, scaling,
aspect settings, or emulation.

Treat the right half solely as visual reference evidence for the intended
arcade presentation.

Do not inspect, search for, enumerate, open, or analyze any other screenshot,
image, video, frame, contact sheet, or visual asset.

Do not use the previously supplied Level 2 long-term prototype in this
quantitative analysis.

PRIMARY QUESTION

After removing ordinary differences in:

- screenshot translation;
- uniform overall size;
- orientation/reflection where required;

how closely does each C# tube match its corresponding reference tube?

In particular:

Why does Level 7 appear substantially broader horizontally than its reference
while Levels 1, 2, and 5 appear considerably closer?

Determine whether the Level 7 difference is primarily:

A. horizontal-versus-vertical proportion;
B. near/far perspective;
C. different underlying point geometry;
D. capture/tube state;
E. placement/composition;
F. some combination.

Do not implement a correction.

Do not select a final tuning value.

CURRENT BASELINE

Expected committed baseline:

c8bc40416eff4ffaac17862f7c894e5a34d133f9

Verify:

- branch master;
- upstream origin/master;
- ahead/behind;
- index;
- tracked worktree;
- relevant untracked state.

The accepted display contract is frozen:

Host = 1600 x 1280
Portrait viewport = 960 x 1280
Viewport origin = (320,0)
Viewport aspect = 3:4
Uniform host mapping

Do not reopen or modify it.

CONTROLLING PRIOR REPORTS

Use as context:

AI-Logs/Reports/2026-11-19-cross-level-presentation-proportion-investigation-report.md

and:

AI-Logs/Reports/2026-11-19-image-grounded-presentation-proportion-investigation-report.md

The first established source/transform mathematics.

The second attempted the visual analysis but performed none because no image
attachments were available.

This task supplies the missing authorized image evidence.

PRIOR MATHEMATICAL FINDINGS

Preserve unless the current source has materially changed:

- Level 1 production geometry W:H = 1.000.
- Level 2 production geometry W:H = 1.000.
- Level 5 production geometry W:H = 1.000.
- Level 7 production geometry W:H = 1.000.
- Level 7 centered RMS X:Y = 1.000.
- Atari record 06 bounding W:H = 1.000.
- Record 06 centered RMS X:Y is approximately 0.994683.
- Level 1 and Level 7 active adapter X/Y gains are isotropic.
- Level 2 uses an accepted vertical presentation factor of 1.314.
- Level 5 uses .670 X / .880 Y presentation gains.
- Levels 2 and 5 therefore have effective projected W:H near 0.761.
- Shared host camera/viewport gain is uniform.
- Final placement translation cannot itself change X:Y proportion.

Do not repeat these calculations unless needed to reconcile image findings.

VISUAL EXTRACTION

For each image:

1. Identify the vertical divider separating C# and reference.
2. Treat each half independently.
3. Identify the visible blue tube geometry.
4. Exclude HUD text and level digits.
5. Exclude yellow Blaster geometry.
6. Exclude enemies, shots, effects and other colored gameplay vectors.
7. Exclude viewport borders if visible.
8. Identify stable tube landmarks.

Do not assume every blue pixel belongs to tube geometry if HUD/status content
is also blue.

Document the extraction method.

LANDMARKS

Use geometrically meaningful corresponding landmarks.

Prefer:

- outer/near-rim vertices;
- inner/far-rim or deep-opening vertices;
- rail/rim intersections;
- distinctive corners;
- open endpoints where applicable.

Do not use temporary gameplay objects as landmarks.

For each pair, record the matched landmark list and correspondence.

SIMILARITY NORMALIZATION FIRST

For each level, first fit ONLY:

- translation;
- one uniform scale;
- discrete rotation/reflection if needed.

Do NOT permit separate X and Y gains in the first fit.

The purpose is to remove:

- placement;
- absolute size;
- capture scale;
- orientation.

Then evaluate the remaining shape residual.

Report:

- best uniform scale;
- translation;
- orientation/reflection;
- normalized RMS residual;
- normalized maximum residual;
- systematic residual direction.

Normalize residuals by reference tube height or another explicitly stated
reference size so levels can be compared.

RAW BOUNDS ARE NOT ENOUGH

Do not conclude that a tube is too wide merely from each screenshot's raw
bounding box.

The captures have different absolute size/composition.

Use matched-landmark similarity normalization first.

Raw bounds may be reported only as supporting context.

LEVEL 1 CONTROL

Analyze Level 1 as the critical isotropic control.

Compare:

- outer rim;
- far opening;
- radial rail directions;
- top/bottom extents;
- side extents;
- near/far relationship.

Determine whether the manual assessment that Level 1 is "reasonably close" is
supported after uniform normalization.

Measure any systematic X/Y residual.

LEVEL 2 CONTROL

Analyze:

- four outer corners;
- four far/deep rectangle corners;
- representative rails.

Determine:

- C# outer rectangle W:H;
- reference outer rectangle W:H;
- C# far rectangle W:H;
- reference far rectangle W:H;
- similarity-fit residual.

The current Level 2 implementation uses Y factor 1.314.

After image measurement, determine whether the visual evidence independently
supports approximately that presentation relationship.

Do not promote pixel measurements to ROM evidence.

LEVEL 5 CONTROL

Analyze the cross geometry using:

- upper stem corners;
- horizontal wing corners;
- lower stem corners;
- central/far structure;
- corresponding breakpoint vertices;
- representative rails.

The current Level 5 presentation uses .670 X / .880 Y gains.

Determine whether the reference comparison independently supports that
approximately 0.761 effective W:H relationship.

Use Level 5 as a strong nonrectangular proportion control.

LEVEL 7 PRIMARY ANALYSIS

Analyze Level 7 in greatest detail.

Use corresponding:

- four primary outer lobes;
- four inward notches/channels;
- top V;
- bottom V;
- left/right side extents;
- central/far structure;
- representative near-to-far rails;
- all clearly recoverable near-rim vertices.

After similarity alignment determine:

1. Does C# remain systematically broader?
2. Is the broadening symmetric left/right?
3. Is the difference similar in upper and lower lobes?
4. Are notches proportionally broader?
5. Are rail slopes systematically shallower?
6. Does the far/deep structure show the same X:Y difference?
7. Is the discrepancy consistent with one global Y enlargement?
8. Or are individual vertices arranged differently?

Quantify the result.

NEAR/FAR PERSPECTIVE ANALYSIS

For Levels 1, 2, 5, and 7 separately measure:

- near envelope width;
- near envelope height;
- far/deep envelope width;
- far/deep envelope height;
- far/near width ratio;
- far/near height ratio;
- far-center displacement relative to near center;
- representative rail convergence angles.

The key diagnostic:

If C# and reference have similar cross-section X:Y ratios but substantially
different far/near ratios, classify the issue as primarily PERSPECTIVE.

If the same X:Y correction is needed independently at near and far
cross-sections, classify it as primarily PRESENTATION PROPORTION.

If neither explains the point residuals, classify it as GEOMETRY DIFFERENCE.

CAPTURE-STATE COMPATIBILITY

Before fitting each pair, determine whether the two halves appear to be in
compatible presentation states.

Look for evidence of:

- ordinary Playing;
- exit zoom;
- Approach;
- altered perspective;
- tube-transition state.

If a pair is not sufficiently state-compatible, label its quantitative result
lower confidence.

Do not force a precise geometry fit across known incompatible states.

PLAYER POSITION DOES NOT INVALIDATE TUBE COMPARISON

The Blaster and enemies may be in different lanes/states across C# and
reference.

Ignore those gameplay-state differences unless they obscure required tube
landmarks.

We are comparing tube presentation, not gameplay state.

LEVEL 7 ONE-Y-GAIN DIAGNOSTIC FIT

Only after the uniform similarity analysis:

Fit one Level-7-only presentation-space Y gain while preserving X.

Use an explicitly defined fixed anchor.

Test reasonable anchor candidates based on existing architecture, preferably:

- projected tube-envelope center; or
- existing Level 7 logical presentation center.

Do not fit arbitrary independent translation for near and far after the shared
gain except the ordinary overall alignment already used for comparison.

Report:

- diagnostic best-fit Y gain;
- anchor used;
- RMS residual;
- max residual;
- near-region residual;
- far-region residual;
- rail-angle residual.

Compare against the uniform-similarity result.

Do not implement the gain.

GENERAL AFFINE UPPER BOUND

Fit a general 2D affine transform as a DIAGNOSTIC UPPER BOUND.

Report its residual.

Compare:

Uniform similarity residual
versus
One-Y-gain residual
versus
General-affine residual.

Interpretation:

- If one Y gain captures nearly all affine improvement, the discrepancy is
  plausibly one presentation-proportion issue.
- If general affine is much better, a one-gain explanation is incomplete.
- If neither fixes large local residuals, underlying shape/landmark geometry
  differs.

Do not propose an affine runtime correction.

LEVEL 1 VERSUS LEVEL 7 CONTRADICTION

Explicitly resolve or characterize this contradiction:

Source/math says both Level 1 and Level 7 are isotropic through the active
presentation path.

Manual visual assessment says Level 1 compares reasonably well while Level 7
looks broad.

Use the matched image residuals to determine whether:

- Level 1 actually has a hidden proportion mismatch too;
- Level 7 reference geometry itself has a different normalized shape;
- matched perspective differs;
- Level 7 needs a presentation-only gain despite isotropic source geometry;
- or another explanation is supported.

Do not leave this question implicit.

LEVEL 2 / LEVEL 5 VALIDATION OF METHOD

Use Levels 2 and 5 to validate the image-analysis method.

Their code already contains known vertical presentation influences.

If image fitting cannot detect a proportion relationship compatible with those
known transforms, explain why before trusting a Level 7 fitted value.

This is an important sanity check.

CROSS-LEVEL RESULTS TABLE

Produce one concise table:

Level
Matched landmarks
Capture compatibility
C# raw W:H
Reference raw W:H
Similarity scale
Similarity RMS
Similarity max
Diagnostic Y gain
Y-gain RMS
Affine RMS
Near/far perspective classification
Final diagnosis
Confidence

Levels:
1, 2, 5, 7.

SOURCE / IMAGE RECONCILIATION

Reconcile the measured visual findings with implementation math.

Do not overwrite prior source evidence with image fitting.

Classify separately:

- IMPLEMENTATION-ESTABLISHED;
- VISUAL DIAGNOSTIC;
- MANUAL OBSERVATION;
- PROJECT TRANSLATION;
- unresolved.

If image data suggests a Level 7 Y gain, state clearly that the value is a
visual diagnostic candidate, not ROM-backed or arcade-hardware-derived.

VIEWPORT AND GAMEPLAY ARE FROZEN

Do not reopen:

- 1600x1280 host window;
- 960x1280 viewport;
- viewport origin (320,0);
- global viewport aspect;
- Level 7 gameplay coordinates;
- lane topology;
- collision geometry;
- Level 7 shared presentation architecture;
- accepted per-level Y placement;
- Levels 1-6/8/9;
- diagnostic Level 10.

No source changes.

POTENTIAL NEXT STEP

If visual evidence strongly supports a simple Level-7-only presentation-space
Y gain:

Describe, but DO NOT IMPLEMENT, one bounded development-only experiment.

The future experiment should:

- preserve production gameplay geometry;
- preserve the shared projector;
- apply one Level-7-only presentation Y gain;
- apply it consistently to tube and gameplay entity presentation;
- update local presentation basis/scale consistently;
- preserve horizontal center;
- preserve accepted vertical composition where possible;
- require Neal's manual A/B acceptance.

Do not select the value as final production behavior.

If evidence does NOT justify that experiment, identify the single narrowest
remaining uncertainty.

NO IMPLEMENTATION

Do not modify:

- source;
- tests;
- configuration;
- documentation;
- presentation;
- geometry;
- viewport;
- gameplay;
- recordings;
- Git state.

Do not launch the game.

Temporary bounded image-analysis calculations are permitted outside tracked
project content.

Do not save generated masks, crops, landmark files, or analysis output into
tracked project paths.

AI-LOGS

Follow `.github/copilot-instructions.md`.

Save:

- this substantive prompt under AI-Logs/Prompts/;
- the paired corrected-image matched-comparison report under AI-Logs/Reports/.

These ignored logs are the only permitted file writes.

Do not stage or commit AI-Logs.

REPOSITORY SAFETY

Record start and finish:

- branch;
- HEAD;
- upstream;
- ahead/behind;
- index;
- tracked worktree;
- relevant untracked state.

Expected baseline:

c8bc40416eff4ffaac17862f7c894e5a34d133f9

Verify actual state.

Preserve unrelated local changes and references.

EXPECTED REPORT

Near the beginning answer directly:

1. Does Level 7 remain measurably broader after translation and uniform size
   are normalized?
2. Approximately how large is the residual proportion difference?
3. Is it primarily X:Y proportion, perspective, different geometry, capture
   state, or a combination?
4. Does Level 1 truly compare better after the same objective normalization?
5. Do Levels 2 and 5 validate the analysis by recovering their known
   proportion tendencies?
6. What Level-7-only Y gain best fits visually, if one is justified?
7. How much does that improve versus uniform similarity?
8. Does general affine fitting materially outperform the one-gain model?
9. Is the global viewport correctly frozen?
10. Is Level 7 gameplay geometry correctly frozen?
11. Is there enough evidence for a bounded Level-7-only presentation
    experiment?

The report must include:

- exact four authorized images inspected;
- explicit confirmation earlier mistaken images were ignored;
- extraction method;
- landmark lists and correspondence;
- capture-state compatibility;
- Level 1 quantitative comparison;
- Level 2 quantitative comparison;
- Level 5 quantitative comparison;
- Level 7 quantitative comparison;
- cross-level result table;
- near/far perspective table;
- Level 7 similarity fit;
- Level 7 one-Y-gain fit;
- Level 7 affine upper-bound fit;
- Level 1-versus-Level 7 reconciliation;
- Level 2/5 method validation;
- source/image reconciliation;
- evidence classifications;
- confidence/limitations;
- smallest justified next experiment, if any;
- confirmation no project/Git state changed except AI-Logs;
- exact prompt/report paths.

Do not ask Neal questions during execution.

Do not pause for manual input.

Complete autonomously and stop after the corrected image-grounded diagnostic
report.

TEMPEST_CORRECTED_MATCHED_IMAGE_PROPORTION_READONLY