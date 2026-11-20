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

Perform a bounded READ-ONLY, IMAGE-GROUNDED matched-comparison investigation of
the current C# Atari-presentation tubes versus the supplied arcade/reference
comparison images.

The primary unresolved question is:

Why does Level 7 look noticeably broader horizontally relative to its supplied
reference when Levels 1, 2, and 5 appear proportionally much closer?

This task must distinguish actual tube-shape/proportion differences from:

- uniform size differences;
- translation/placement differences;
- capture/crop differences;
- near/far perspective differences;
- gameplay-state differences;
- HUD/composition differences.

Do not implement a correction.

Do not change viewport, geometry, projection, perspective, placement, or
gameplay.

CURRENT BASELINE

Begin from:

c8bc40416eff4ffaac17862f7c894e5a34d133f9

on master tracking origin/master.

Verify actual:

- branch;
- HEAD;
- upstream;
- ahead/behind;
- index;
- tracked worktree;
- relevant untracked state.

The accepted display contract is frozen:

Host:
1600 x 1280

Portrait viewport:
960 x 1280

Viewport origin:
(320,0)

Aspect:
3:4

The viewport is a uniform mapping and has already been mathematically excluded
as a Level-7-only X/Y distortion source.

Do not reopen it.

CONTROLLING PRIOR ANALYSIS

Use:

AI-Logs/Reports/2026-11-19-cross-level-presentation-proportion-investigation-report.md

That report established from source/math:

- Level 1 production geometry W:H = 1.000;
- Level 2 production geometry W:H = 1.000;
- Level 5 production geometry W:H = 1.000;
- Level 7 production geometry W:H = 1.000;
- Level 7 centered RMS X:Y = 1.000;
- record 06 bounding W:H = 1.000;
- record 06 centered RMS X:Y is approximately 0.994683;
- Level 7 adapter local X/Y gain is isotropic;
- Level 1 adapter output is also isotropic;
- Level 2 uses a Y presentation factor of 1.314;
- Level 5 uses .670 X / .880 Y presentation gains;
- Levels 2 and 5 therefore produce projected W:H near 0.761;
- shared host camera and viewport use equal X/Y gain;
- ordinary placement translations do not alter proportion.

The report could not resolve the visual Level-1-versus-Level-7 contradiction
because the comparison images were not available to that task.

This investigation supplies the missing image evidence.

AUTHORIZED IMAGES ONLY

You MAY inspect only the specifically identified comparison images supplied for
this task.

Neal will provide or attach the matched comparison images for:

- Level 1 C# and arcade/reference;
- Level 2 C# and arcade/reference;
- Level 5 C# and arcade/reference;
- Level 7 C# and arcade/reference.

If supplied as combined side-by-side files, inspect those exact files.

If individual source images are also explicitly attached to this task, they may
be inspected.

Do not search for, enumerate, open, inspect, or analyze any other repository
screenshots, images, video frames, contact sheets, or unknown visual assets.

Do not infer authorization from being in the same directory.

If one of the required Level 1/2/5/7 comparisons is not actually available,
analyze the available authorized set and state the limitation.

LEVEL 2 PROTOTYPE IMAGE

If Neal also supplies the earlier Level 2 long-term prototype image, treat it
as CONTEXT ONLY.

Do not use the prototype for quantitative matching.

Do not derive:

- tube proportions;
- scale;
- HUD coordinates;
- tube placement;
- viewport dimensions;
- implementation constants

from that prototype.

The prototype only communicates long-term composition intent:

- large dominant portrait tube;
- stable HUD/status region above;
- gameplay tube occupying most of the region beneath HUD;
- direct visual comparability with arcade/reference captures.

PRIMARY VISUAL ANALYSIS RULE

Do NOT compare raw pixel width/height of the two screenshots without first
normalizing away irrelevant capture differences.

For every C# / reference pair:

1. Identify corresponding visible tube landmarks.
2. Normalize translation.
3. Normalize one shared UNIFORM scale.
4. Where necessary, resolve a discrete orientation/reflection correspondence.
5. Do NOT permit anisotropic scaling in the initial similarity fit.
6. Measure the residual shape disagreement after that normalization.

This determines whether the tubes actually differ in SHAPE rather than merely
size or placement.

LANDMARK SELECTION

Use tube geometry, not gameplay objects, as the primary landmarks.

Prefer:

- near-rim vertices;
- far-rim/deep-opening vertices;
- stable rail intersections/endpoints;
- open-tube terminal points where applicable.

Do not use:

- Blaster;
- Flippers;
- Tankers;
- shots;
- score text;
- lives;
- temporary effects

as geometry landmarks.

If gameplay overlaps a tube vertex, use another unobstructed geometrically
equivalent landmark where possible.

RENDERED LINE EXTRACTION

The tube is represented by colored vector lines on black.

Use bounded image analysis to distinguish tube lines from:

- yellow Blaster geometry;
- green/red enemies;
- HUD text;
- status graphics;
- viewport border;
- effects.

Document the extraction/matching method.

Do not silently treat every blue pixel as tube geometry if HUD or other blue
content is present.

Do not derive production geometry coordinates from the extracted pixels.

The measurements are VISUAL DIAGNOSTIC only.

LEVEL 1 CONTROL

For Level 1 compare:

- outer/near rim;
- inner/far rim;
- representative rails at top, side, and lower portions;
- rotational symmetry;
- normalized radial distances;
- normalized near/far ratio.

After translation + uniform scale + orientation alignment, calculate:

- RMS landmark residual;
- maximum landmark residual;
- horizontal residual pattern;
- vertical residual pattern.

Determine whether the manual "reasonably close" assessment is supported.

LEVEL 2 CONTROL

Level 2 is especially useful because its square/rectangular geometry makes
proportion differences obvious.

Compare:

- outer rectangle width/height;
- far rectangle width/height;
- corresponding corner positions;
- rail slopes;
- near/far relative positioning.

After uniform normalization, determine whether the remaining difference is
consistent with the known C# Y factor 1.314.

Do not assume the existing factor is correct solely because the images appear
close.

Report the visual diagnostic ratio independently.

LEVEL 5 CONTROL

Level 5 has a distinctive cross-like silhouette.

Compare:

- narrow upper extent;
- broad middle wings;
- narrow lower extent;
- central/deep region;
- corresponding turn vertices;
- representative rail directions.

Determine whether the visual match is consistent with the existing .670 X /
.880 Y presentation relationship.

Again, report image evidence independently.

LEVEL 7 PRIMARY CASE

Perform the most detailed analysis on Level 7.

Compare corresponding:

- four principal lobes;
- four inward notches/channels;
- top and bottom V structures;
- left and right extents;
- central/deep region;
- representative rails;
- near-rim vertices;
- far/deep vertices.

After translation + UNIFORM scale normalization, determine:

1. whether C# remains systematically broader;
2. how much broader, if measurable;
3. whether the residual is consistent across all four lobes;
4. whether only near-rim geometry differs;
5. whether only far/deep geometry differs;
6. whether rail angles imply a perspective-depth difference;
7. whether one global anisotropic X/Y correction would substantially remove
   the residual;
8. whether a general affine fit materially outperforms one X/Y gain;
9. whether the remaining difference instead reflects genuinely different point
   geometry.

Do not implement any fit.

MATCHED PERSPECTIVE CHECK

A major goal is to distinguish X:Y proportion from near/far perspective.

For each level calculate or estimate from matched landmarks:

- normalized near-rim envelope;
- normalized far-rim/deep-opening envelope;
- far/near width ratio;
- far/near height ratio;
- center displacement of far relative to near;
- representative rail convergence angles.

If C# and reference have different far/near scale ratios, classify that as a
PERSPECTIVE difference rather than an X:Y aspect difference.

If near and far cross-sections independently need the same Y/X correction,
classify that as evidence for a presentation-proportion difference.

CAPTURE-STATE CHECK

Determine whether each C# / reference pair appears to represent comparable
tube phases/states.

Check for evidence of:

- ordinary Playing;
- exit zoom;
- Approach;
- different K/ZOffset state;
- different depth/perspective state.

Do not assume two screenshots are matched merely because they show the same
displayed level.

If a reference appears to be from a materially different tube/perspective
state, say so and reduce confidence in proportion fitting.

Do not manufacture a correspondence across incompatible states.

SIZE / PLACEMENT / PROPORTION SEPARATION

For every pair report independently:

A. uniform size difference;
B. translation/placement difference;
C. X:Y proportion difference;
D. perspective/near-far difference;
E. residual geometry difference.

Do not combine these into one vague "looks different" score.

NORMALIZED COMPARISON TABLE

Produce one table for Levels 1, 2, 5, and 7 including:

- number of matched landmarks;
- C# tube raw visual bounds;
- reference tube raw visual bounds;
- best uniform similarity scale;
- translation;
- orientation/reflection;
- similarity-fit RMS residual normalized by reference tube height;
- maximum normalized residual;
- best anisotropic X/Y ratio, diagnostic only;
- anisotropic-fit RMS residual;
- far/near width ratios;
- far/near height ratios;
- perspective classification;
- final visual diagnosis.

Do not present fitted diagnostic ratios as implementation constants.

LEVEL 7 ONE-GAIN TEST

Only after the similarity analysis, test diagnostically whether one global
Level-7-only presentation-space Y gain about a fixed anchor would explain the
observed difference.

Determine:

- best-fit diagnostic Y gain relative to current C#;
- appropriate fixed anchor for comparison;
- residual after that one-gain fit;
- whether near and far regions improve simultaneously;
- whether rail angles improve consistently;
- whether the fit leaves systematic per-point residuals.

This is analysis only.

Do not change code.

Do not promote the fitted gain to PROJECT TRANSLATION yet.

LEVEL 7 GENERAL-AFFINE CONTROL

Also calculate a best general affine fit only as a diagnostic upper bound.

Compare general-affine residual against the one-Y-gain residual.

If general affine provides negligible improvement, a simple presentation
proportion correction may be justified.

If general affine provides substantial improvement, the problem is not
adequately described as one Y-scale discrepancy.

Do not implement a general affine correction.

SOURCE / IMAGE RECONCILIATION

After visual measurement, reconcile the result with the prior source/math
analysis.

Specifically answer:

- Level 7 source geometry is 1:1 in bounds and RMS. Does the reference image
  appear non-1:1 after perspective normalization?
- Level 1 source and adapter are also 1:1. Why does Level 1 visually compare
  differently?
- Are Level 2 and Level 5 visual results consistent with their explicit
  vertical presentation gains?
- Does the reference imagery imply level-specific presentation proportion
  behavior?
- Are we comparing fundamentally different reference geometry rather than
  different presentation scaling?

Do not discard either mathematical or visual evidence.

Explain contradictions.

VIEWPORT IS FROZEN

Do not recommend changing the accepted global viewport unless all or most
matched levels exhibit the same systematic X:Y error.

Current accepted values remain:

- window 1600x1280;
- viewport 960x1280;
- viewport origin (320,0);
- aspect 3:4;
- uniform host gain.

The clean viewport closeout is:

c8bc40416eff4ffaac17862f7c894e5a34d133f9

Treat it as the baseline.

LEVEL 7 GAMEPLAY AND GEOMETRY ARE FROZEN

Do not recommend changing production Level 7 gameplay geometry unless the
visual evidence proves that no presentation-space transform can reproduce the
reference and the point geometry itself differs materially.

Even then, report the finding only.

Do not authorize a gameplay geometry change.

Level 7 currently plays correctly.

POTENTIAL FUTURE CORRECTION

If and only if the visual evidence strongly establishes a simple
Level-7-specific presentation proportion difference, define the smallest
future DEVELOPMENT-ONLY experiment.

Prefer, if supported:

- one Level-7-only Y presentation gain;
- applied after shared adapter projection;
- around an explicitly defined fixed anchor;
- applied consistently to tube and gameplay entity presentation;
- local scale/basis corrected consistently;
- gameplay geometry untouched;
- all accepted global viewport values untouched.

Do not provide an implementation prompt in this investigation.

Do not select a final production value unless Neal later manually accepts an
experiment.

If evidence does not justify this direction, state the narrower next evidence
needed.

NO IMPLEMENTATION

Do not modify:

- source;
- tests;
- configuration;
- documentation;
- viewport;
- geometry;
- presentation;
- gameplay;
- recordings;
- Git state.

Do not add diagnostics.

Do not stage, commit, push, reset, restore, clean, stash, rebase, or switch
branches.

Do not launch the game.

Temporary bounded analysis outside tracked project content is permitted.

Python/PowerShell may be used for image/geometry arithmetic, but do not write
analysis artifacts into tracked project paths.

AI-LOGS

Follow `.github/copilot-instructions.md`.

Save:

- this substantive prompt under `AI-Logs/Prompts/`;
- the paired image-grounded comparison report under `AI-Logs/Reports/`.

These ignored files are the only permitted writes.

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

on master tracking origin/master at 0/0.

Verify rather than assume.

Preserve unrelated local work.

EXPECTED REPORT

Near the beginning answer directly:

1. Does Level 7 remain measurably broader than its reference after translation
   and uniform-size differences are removed?
2. If yes, by approximately how much?
3. Is the difference primarily X:Y proportion, perspective, geometry, capture
   state, or a combination?
4. Why does Level 1 compare better despite also being mathematically isotropic?
5. Are Levels 2 and 5 visual matches consistent with their existing vertical
   presentation gains?
6. Does one Level-7-only Y gain substantially explain the mismatch?
7. Does general affine fitting materially improve beyond one Y gain?
8. Is the global viewport correctly frozen?
9. Is Level 7 gameplay geometry correctly frozen?
10. Is there enough evidence for a bounded Level-7-only presentation
    experiment?

The report must include:

- exact authorized images inspected;
- capture-state compatibility assessment;
- image tube-line extraction method;
- landmark-matching method;
- Level 1 normalized comparison;
- Level 2 normalized comparison;
- Level 5 normalized comparison;
- Level 7 normalized comparison;
- normalized comparison table;
- near/far perspective table;
- Level 7 one-Y-gain diagnostic fit;
- Level 7 general-affine diagnostic fit;
- source/math reconciliation;
- visual evidence classification;
- implementation evidence classification;
- whether viewport remains frozen;
- whether gameplay geometry remains frozen;
- smallest future experiment, if justified;
- confidence and limitations;
- confirmation no project/Git state changed except required AI-Logs;
- exact prompt/report paths.

Do not ask Neal questions during execution.

Do not pause for manual input.

Complete autonomously and stop after the image-grounded diagnostic report.

TEMPEST_MATCHED_IMAGE_PRESENTATION_PROPORTION_READONLY