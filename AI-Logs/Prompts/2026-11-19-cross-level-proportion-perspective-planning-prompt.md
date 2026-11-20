READ THIS ENTIRE USER-PROVIDED TASK BEFORE TAKING ANY ACTION.

FAIL-CLOSED CHECK

There are only two reasons to stop before completing the investigation:

1. The completion-token check fails.
2. One or more of the four explicitly authorized absolute image paths cannot
   be opened.

Do not stop merely because:

- a landmark is partly obscured;
- a metric has lower confidence;
- automatic image extraction is imperfect;
- one calculation method fails;
- Python is unavailable;
- the screenshots show different gameplay objects;
- an individual requested value cannot be measured precisely;
- unrelated local work exists.

Continue with available authorized evidence, state limitations, and complete
the report.

COMPLETION-TOKEN CHECK

Before using tools, reading files, opening images, or running commands:

1. Locate the unique completion token near the end of this user-provided task.
2. Verify exactly one complete token beginning with "TEMPEST_" appears on a
   line by itself.
3. Automatically appended VS Code/agent context after the token does not
   invalidate it.
4. If exactly one complete token cannot be found, STOP.

TASK

Perform a bounded READ-ONLY presentation-model investigation to determine:

1. whether Levels 1 and 7 are missing the same class of vertical presentation
   correction already present in Levels 2 and 5;

2. whether one common presentation rule can explain the outer tube proportions
   of Levels 1, 2, 5, and 7;

3. whether outer cross-section proportion and near/far perspective must be
   handled as separate presentation concerns;

4. what smallest DEVELOPMENT-ONLY experiment would be technically justified
   next.

Do not implement anything.

Do not modify presentation constants.

Do not select a final production value.

Do not change gameplay geometry or the global viewport.

AUTHORIZED IMAGE PATHS

Open and analyze only these four exact files:

C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\comparisons\Level-1 SideBySide.png

C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\comparisons\Level-2 sidebyside.png

C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\comparisons\level-5 Sidebyside.png

C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\comparisons\Level-7 SidebySide.png

Each file contains:

LEFT = current C# presentation
RIGHT = arcade/MAME visual reference

Do not inspect any other image, screenshot, video, frame, contact sheet, or
visual asset.

Do not enumerate UserData or neighboring files.

IMAGE AVAILABILITY

Open all four exact paths directly.

If all four open, continue.

If an exact path cannot be opened, verify that path once and stop only if the
file is genuinely unavailable.

Do not substitute another image.

CONTROLLING REPORTS

Use these reports:

AI-Logs/Reports/2026-11-19-cross-level-presentation-proportion-investigation-report.md

AI-Logs/Reports/2026-11-19-absolute-path-matched-image-proportion-investigation-report.md

The first contains source/transform mathematics.

The second contains the successful matched-image measurements.

Do not rerun the entire prior investigation.

Use the established results as input and perform only the additional analysis
needed to determine the correct presentation-model direction.

ESTABLISHED IMAGE FINDINGS

Treat these as VISUAL DIAGNOSTIC findings from the controlling report:

Reference outer W:H ratios:

- Level 1: 0.762
- Level 2: 0.764
- Level 5: 0.762
- Level 7: 0.765

Current C# outer W:H ratios:

- Level 1: 1.000
- Level 2: 0.762
- Level 5: 0.760
- Level 7: 0.999

Prior diagnostic Y fits:

- Level 1: 1.230
- Level 7: 1.210

Level 7 outer-envelope residual:

- similarity RMS: 5.43% of reference height;
- one-Y-gain RMS: 2.89%;
- general-affine RMS: 2.75%.

Level 1 outer-envelope residual:

- similarity RMS: 4.66%;
- one-Y-gain RMS: 1.41%;
- affine RMS: approximately 1.41%.

Levels 2 and 5 already compare closely in outer proportion.

The Level 7 inner/core relationship remains lower confidence and is not
explained conclusively by the outer-envelope gain.

ESTABLISHED SOURCE FINDINGS

Treat these as IMPLEMENTATION-ESTABLISHED:

- Levels 1, 2, 5, and 7 source point clouds each have W:H = 1.000.
- Levels 1 and 7 currently use isotropic presentation gains.
- Level 2 uses a vertical presentation factor of 1.314.
- Level 5 uses .670 X and .880 Y gains.
- Level 2 and Level 5 effective width-to-height presentation ratios are near
  0.761.
- Level 7 record 06 is approximately isotropic.
- The shared camera and 960x1280 portrait viewport use uniform X/Y gain.
- Final placement translations do not change shape proportion.

Do not reinterpret these as Atari hardware facts.

PRIMARY MODEL QUESTION

Determine whether the current evidence supports a common presentation rule of
this general form:

For a tube whose source/model cross-section is roughly isotropic, apply a
vertical presentation gain that maps the displayed near-rim envelope toward the
reference outer ratio near 0.763.

Do not assume this rule is correct.

Test it against Levels 1, 2, 5, and 7.

Distinguish:

A. a common reference-envelope rule;

B. individually fitted per-level presentation gains;

C. shape-family-specific rules;

D. no sufficiently justified common rule.

COMMON TARGET-RATIO ANALYSIS

Using the four reference images, calculate:

- mean reference outer W:H;
- median reference outer W:H;
- range and spread;
- implied Y gain for each current C# tube to reach one common target ratio;
- residual difference between that common-rule gain and each level's prior
  individually fitted diagnostic gain.

Determine whether the reference ratio clustering is tight enough to support a
shared PROJECT TRANSLATION concept.

Do not use the common target as a final production constant.

LEVEL 1 ANALYSIS

Determine whether Level 1 supports a clean proportion-only correction.

Specifically:

- test the common-target Y gain;
- compare it with the prior Level 1 diagnostic gain 1.230;
- measure outer-contour residual;
- measure near/far center and relative-size residual;
- determine whether the same gain improves near and far structure;
- determine whether general affine materially improves beyond one Y gain.

If Level 1 is well explained by one Y gain, state that clearly.

LEVEL 7 ANALYSIS

Separate Level 7 into two questions.

QUESTION A: OUTER CROSS-SECTION

Determine whether the outer silhouette is well explained by:

- the common-target Y gain;
- the prior individual diagnostic gain 1.210;
- another narrowly fitted Y gain.

Compare residuals and confidence.

QUESTION B: DEPTH / CORE / RAIL RELATIONSHIP

Re-examine the authorized Level 7 image only as needed to assess:

- near-envelope center;
- far/core center;
- far/core size;
- upper-notch-to-core relation;
- representative rail convergence;
- whether the apparent sign reversal in far-center displacement is reliable;
- whether gameplay vectors could have contaminated the previous core estimate.

Do not infer a precise perspective formula from uncertain landmarks.

Report whether the interior evidence is:

- reliable enough to define a separate perspective problem;
- suggestive but insufficient;
- or likely contaminated by landmark extraction.

OUTER PROPORTION VERSUS PERSPECTIVE

Define two independent presentation dimensions:

1. CROSS-SECTION PROPORTION:
   X:Y shape of the near and far cross-sections.

2. DEPTH PERSPECTIVE:
   size, position, and displacement relationship between near and far
   cross-sections.

Determine whether the current implementation conflates these concerns.

Identify which current code/formulas control:

- Level-specific X/Y presentation gain;
- near/far projection scale;
- far-center displacement;
- final per-level translation;
- local entity scale/basis.

Do not change the code.

LEVEL 2 AND LEVEL 5 CONTROLS

Use Levels 2 and 5 to determine whether their existing vertical presentation
corrections affect only cross-section proportion or also influence depth
perspective.

For each trace the current transform order:

source geometry
-> level-specific X/Y gain
-> depth projection
-> placement
-> common host camera

Determine whether the existing Level 2/5 patterns provide a safe architectural
precedent for Levels 1/7.

Do not assume Level 2/5 implementation is ideal merely because the outer
contours match.

TRANSFORM-ORDER QUESTION

Determine where a future vertical gain would most safely occur.

Compare these possible seams:

A. scale model Y before depth projection;

B. scale projected logical Y about the projected tube-envelope center;

C. scale final logical playfield Y before the host camera;

D. another existing seam established by Levels 2/5.

For each evaluate:

- tube/entity registration;
- near/far behavior;
- local entity basis/scale;
- Blaster orientation;
- shots and Spikes;
- ExitZoom focal compensation;
- Approach;
- final placement;
- gameplay-state isolation.

Recommend one seam only if evidence is sufficient.

Do not implement it.

ANCHOR ANALYSIS

A presentation-space Y gain requires a fixed anchor.

Evaluate:

- projected near-envelope center;
- complete near/far envelope center;
- existing per-level presentation center;
- LevelDefinition CenterY/YOffset reference;
- another current architectural anchor.

Determine which anchor:

- preserves horizontal center;
- avoids arbitrary vertical drift;
- keeps tube and entities registered;
- behaves coherently through ordinary Playing;
- can be recalculated for ExitZoom/Approach if later required.

Do not select an anchor from screenshot pixels alone.

LEVEL APPLICABILITY

Do not assume a correction should apply only to Level 7.

Determine whether evidence supports a future experiment for:

- Level 1 only;
- Level 7 only;
- Levels 1 and 7 together;
- all currently isotropic closed tubes;
- another explicitly justified group.

Do not extend beyond Levels 1, 2, 5, and 7 without source evidence.

Do not infer behavior for all 99 levels.

GAMEPLAY AND SHARED SYSTEM PROTECTION

Any future presentation experiment must preserve:

- gameplay geometry;
- collision;
- lane topology;
- movement;
- RNG;
- timing;
- recording/playback;
- tube/entity registration;
- Blaster outward orientation;
- Blaster ExitZoom/Approach scale;
- ExitZoom focal compensation;
- per-level placement;
- 1600x1280 host;
- 960x1280 viewport.

Identify every shared system that a candidate gain would touch indirectly.

PERSPECTIVE EVIDENCE LIMIT

Do not create a perspective correction from the current Level 7 core estimate
unless the landmarks are sufficiently reliable.

If better evidence is required, specify the narrowest useful capture:

- ordinary Playing;
- unobscured tube center;
- known level;
- matching presentation state;
- no transition;
- enough visible rails to identify near/far correspondence.

Do not request a video unless static captures are insufficient.

POTENTIAL DEVELOPMENT EXPERIMENT

If the evidence supports a vertical-proportion experiment, define one bounded
candidate plan.

The plan may include:

- a development-only diagnostic gain;
- explicit affected levels;
- explicit transform seam;
- explicit anchor;
- tube/entity/world-coordinate consistency;
- local basis/scale treatment;
- parser behavior if a runtime option is justified;
- focused automated tests;
- manual A/B commands.

Do not implement.

Do not select a final accepted value.

If Level 7 perspective remains unresolved, define a first experiment that tests
outer proportion only without claiming to solve the entire tube presentation.

NO IMPLEMENTATION

Do not modify:

- source;
- tests;
- configuration;
- durable documentation;
- images;
- presentation;
- geometry;
- gameplay;
- recordings;
- viewport;
- Git state.

Do not launch the game.

Temporary bounded calculations may be used outside tracked project content.

Do not inspect any visual files other than the four authorized images.

AI-LOGS

Follow `.github/copilot-instructions.md`.

Save:

- this substantive prompt under AI-Logs/Prompts/;
- the paired proportion-versus-perspective planning report under
  AI-Logs/Reports/.

These ignored files are the only permitted project writes.

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

Preserve unrelated local work.

EXPECTED REPORT

Answer near the beginning:

1. Do Levels 1 and 7 appear to be missing the same class of vertical
   presentation correction?
2. Is the reference outer-ratio clustering strong enough to justify one common
   diagnostic target?
3. Should the next experiment affect Level 1, Level 7, or both?
4. What diagnostic gain or gain range should the experiment expose?
5. At what transform seam should it be applied?
6. What fixed anchor should be used?
7. Can the outer proportion correction be tested independently of perspective?
8. Is Level 1 adequately explained by one Y gain?
9. Is Level 7 outer shape adequately explained by one Y gain?
10. What Level 7 interior/perspective issue remains unresolved?
11. What additional evidence, if any, is needed before touching perspective?
12. Is there enough evidence for a bounded development-only implementation
    prompt?

The report must include:

- exact four image paths used;
- confirmation no other images were inspected;
- reference target-ratio statistics;
- shared-rule versus per-level-fit comparison;
- Level 1 results;
- Level 2 control results;
- Level 5 control results;
- Level 7 outer results;
- Level 7 core/rail confidence review;
- proportion-versus-perspective separation;
- transform-order analysis;
- anchor analysis;
- level-applicability decision;
- shared-system risk analysis;
- smallest justified development experiment;
- any remaining evidence need;
- evidence classifications;
- repository start/finish state;
- confirmation no project/Git changes except AI-Logs;
- exact prompt/report paths.

Do not ask Neal questions during execution.

Do not pause for manual input.

Complete autonomously and stop after delivering the read-only planning report.

TEMPEST_CROSS_LEVEL_PROPORTION_VERSUS_PERSPECTIVE_PLAN_READONLY