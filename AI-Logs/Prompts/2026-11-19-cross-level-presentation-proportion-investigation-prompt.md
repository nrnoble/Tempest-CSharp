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

Perform a bounded READ-ONLY cross-level presentation-proportion investigation.

The current question is:

Why does reconstructed Level 7 appear noticeably broader horizontally relative
to its height than the supplied arcade/reference Level 7 comparison, while
Levels 1, 2, and 5 appear proportionally much closer to their supplied
references under the SAME accepted 1600x1280 host window and 960x1280 portrait
viewport?

Do not implement a correction.

Do not change the viewport.

Do not change Level 7 geometry.

Do not fit geometry from screenshots.

Determine where the cross-level proportional difference originates in current
geometry, normalization, model-frame mapping, projection, or presentation
mathematics.

CURRENT ACCEPTED VIEWPORT

The current manually accepted display contract is:

Host window:
1600 x 1280

Portrait viewport:
960 x 1280

Viewport origin:
(320,0)

Viewport aspect:
3:4

Left/right margins:
320 / 320

Top/bottom margins:
0 / 0

This is a uniform 4/3 physical enlargement of the former 720x960 portrait
viewport.

Treat this viewport as FROZEN for this investigation.

Do not propose changing the global viewport merely to make Level 7 narrower.

MANUAL CROSS-LEVEL EVIDENCE

Treat the following as MANUAL OBSERVATIONS supplied by Neal:

LEVEL 1

A C# versus arcade/reference side-by-side comparison shows the tube's
width-to-height proportions looking reasonably close.

LEVEL 2

A C# versus arcade/reference side-by-side comparison shows the rectangular
tube's width-to-height proportion looking reasonably close.

The rectangular geometry makes gross X/Y distortion particularly easy to
notice, and no Level-7-like horizontal broadening is apparent.

LEVEL 5

A C# versus arcade/reference comparison shows the characteristic narrow and
wide sections looking proportionally close.

Again, the conspicuous Level 7 horizontal broadening is not apparent.

LEVEL 7

Level 7 now plays correctly after its clean presentation reconstruction.

The tube and entity presentation are coherent.

There is no accepted evidence of wrong point order, topology, lane structure,
or rail connectivity.

However, side-by-side comparison shows the C# Level 7 tube noticeably broader
horizontally relative to its height than the arcade/reference Level 7 tube.

The difference is large enough to complicate direct visual comparison.

OTHER LEVELS

Neal's recent review found Levels 1-6, 8, and 9 generally acceptable under the
new enlarged viewport, with Level 7 the notable proportional outlier.

Treat that as manual evidence, not mathematical proof.

PRIMARY QUESTION

At what stage does Level 7 acquire a different X:Y proportional relationship
from the other otherwise successful tubes?

Determine separately:

1. the intrinsic X:Y proportion of Level 7's production geometry;
2. the intrinsic X:Y proportion of preserved Atari record 06;
3. Level 7 model-frame X/Y scale;
4. Level 7 projection X/Y gain;
5. Level 7 final placement;
6. shared portrait camera X/Y gain;
7. final viewport X/Y gain.

Compare those stages with Levels 1, 2, 5, 8, and 9.

The goal is to identify the FIRST stage where Level 7 becomes proportionally
different.

AUTHORIZED IMAGE SCOPE

You MAY inspect only the specifically supplied comparison images relevant to
this investigation if they are available in the current task/session:

- Level 1 side-by-side comparison supplied by Neal;
- Level 2 side-by-side comparison supplied by Neal;
- Level 5 side-by-side comparison supplied by Neal;
- Level 7 side-by-side comparison supplied by Neal;
- the supplied Level 2 long-term prototype composition image.

Do not search for, enumerate, open, inspect, or analyze other screenshots,
videos, frames, contact sheets, or visual assets.

The Level 2 prototype image is CONTEXT ONLY.

Do not measure the Level 2 prototype for implementation values.

Do not treat it as current acceptance criteria.

IMAGE ROLE

The comparison images establish the visual symptom:

- Levels 1/2/5 are useful proportional controls;
- Level 7 is the apparent outlier.

Use images only to characterize that symptom and, if useful, make bounded
normalized width/height measurements.

Do not derive replacement geometry coordinates from screenshots.

Do not derive production X/Y scale constants from screenshot pixels.

Do not fit Level 7 to the reference during this task.

GEOMETRY-SPACE ANALYSIS

For each comparison level:

- Level 1
- Level 2
- Level 5
- Level 7
- Level 8
- Level 9

calculate the production geometry's intrinsic:

- min X;
- max X;
- width;
- min Y;
- max Y;
- height;
- width/height ratio;
- geometric center;
- RMS X extent from center;
- RMS Y extent from center;
- RMS X/Y ratio.

Do this BEFORE projection/presentation.

For asymmetric/nonrectangular levels, do not rely only on bounding boxes.

Use RMS or another explicitly defined whole-point measure as a second
independent proportion metric.

Record the geometry source/provenance for each level.

LEVEL 7 PRODUCTION VERSUS RECORD 06

This is a central part of the task.

Compare:

A. LevelLoader.LoadLevel7Clover() production coordinates

against:

B. AtariTubeRecordCatalog.Level7Record06 raw coordinates and a mathematically
appropriate centered representation.

Do not activate record 06.

Do not change either data set.

Determine:

- intrinsic X range;
- intrinsic Y range;
- X:Y ratio;
- centered RMS X:Y ratio;
- point-order relationship;
- whether one set can be mapped to the other by:
  - uniform scale + rotation/reflection + translation;
  - anisotropic global X/Y scale + rotation/reflection + translation;
  - general affine map;
  - or not by a simple global map.

If an anisotropic map is sufficient, calculate its diagnostic X/Y ratio.

Label it:

MATHEMATICAL DATASET COMPARISON

not:

ATARI PRESENTATION CONSTANT.

Do not implement it.

LEVEL 7 GEOMETRY PROVENANCE

The current Level 7 production clover is reference-derived production
authority.

Record 06 is preserved ROM/Atari-backed evidence.

Do not conflate those classifications.

Determine whether the production clover's broad X:Y shape exists directly in
its source coordinates BEFORE AtariPresentationAdapter is involved.

If so, state that clearly.

That would mean the current visual difference is intrinsic to the production
geometry rather than introduced by the viewport.

If not, continue downstream.

LEVEL 1 / 2 / 5 CONTROL ANALYSIS

Determine how these successful-looking controls obtained their current
production geometry.

For each identify:

- geometry source;
- any normalization applied during LevelLoader construction;
- X scale;
- Y scale;
- reflection;
- rotation;
- CenterY;
- YOffset;
- continuous/open topology;
- active RomPresentationMetadata entry;
- active AtariPresentationAdapter model-frame transform.

Pay particular attention to whether their geometry constructors or adapters
use different X/Y unit conversion.

LEVEL 8 / 9 CONTROL ANALYSIS

Levels 8 and 9 provide another useful comparison because their current
production geometry is already recovered from Atari data and their ordinary
Playing presentation is manually accepted.

For each establish:

- raw geometry source;
- production normalization;
- X/Y model-frame conversion;
- active metadata;
- final Level-specific Y placement.

Do not change their accepted values.

MODEL-FRAME ANALYSIS

Trace the active AtariPresentationAdapter route for each comparison level.

For each identify the mathematical transform from production model coordinate
delta:

(dx,dy)

to pre-camera logical presentation delta at a fixed representative depth.

Calculate the local Jacobian or equivalent:

dScreenX/dModelX
dScreenX/dModelY
dScreenY/dModelX
dScreenY/dModelY

Use a representative ordinary Playing depth, including near and far endpoints
where depth changes scale.

Determine:

- whether the transform is isotropic in model units;
- whether it swaps axes;
- whether it reflects an axis;
- whether X and Y use different gains;
- whether Level 7 differs from successful controls.

Do not infer proportion from named constants only.

Calculate the effective transformation.

PROJECTION-SPACE ANALYSIS

For each comparison level, project its geometry at:

- near/rim depth;
- far/deep depth.

Before final Level-specific translation and before host camera, calculate:

- projected width;
- projected height;
- width/height ratio;
- RMS X/Y ratio.

Then repeat after Level-specific final translation.

Confirm whether translation changes proportion.

It should not, but establish this mathematically.

HOST-CAMERA ANALYSIS

Trace the accepted common portrait camera into the accepted 960x1280 viewport.

Establish its X and Y host gains.

Verify whether the host camera is uniform.

Verify that the accepted viewport enlargement applies the same 4/3 factor on X
and Y.

Show mathematically whether the global viewport could create a Level-7-only
proportional difference.

If the same camera/viewport applies to every level, explicitly eliminate it as
the source if supported by the equations.

CROSS-LEVEL NORMALIZATION TABLE

Produce one table with one row per level:

Level
Geometry source
Raw/model width
Raw/model height
Raw/model W:H
RMS X:Y
Model-frame X gain
Model-frame Y gain
Effective gain ratio
Projected near W:H
Projected far W:H
Final viewport X gain
Final viewport Y gain
Visual manual classification

Use at least Levels:

1, 2, 5, 7, 8, 9.

This table should make the first diverging stage obvious.

DO NOT ASSUME LEVEL 7 IS WRONG

The investigation must distinguish among:

A. Level 7 production geometry is intrinsically broader than Atari record 06.

B. Level 7 production geometry is proportionally reasonable, but its
   model-frame transform is unlike successful levels.

C. Projection/perspective creates the discrepancy.

D. Final camera/viewport creates the discrepancy.

E. The supplied visual comparison reflects another difference such as
   perspective state, tube size, or capture composition rather than X:Y
   geometry.

F. More than one factor contributes.

Do not select A merely because it is currently plausible.

Prove the earliest difference numerically.

VIEWPORT MUST REMAIN FROZEN

Do not recommend changing:

- 1600x1280 host size;
- 960x1280 portrait viewport;
- viewport origin (320,0);
- 3:4 viewport aspect;
- one-host-pixel border.

The viewport may only be recommended for reconsideration if the mathematics
demonstrate a global proportion defect across controls.

The current manual Level 1/2/5 comparisons argue against that, so any contrary
conclusion requires explicit evidence.

LEVEL 7 GAMEPLAY MUST REMAIN FROZEN

Do not propose changing production Level 7 gameplay points during this
read-only investigation.

A future presentation correction, if justified, must first consider whether
Level 7 can receive a presentation-only model-frame proportion translation
without changing gameplay/collision geometry.

Do not implement such a translation here.

DEFERRED LEVEL 2 PROTOTYPE CONTEXT

The supplied Level 2 prototype represents long-term presentation direction.

Record only these qualitative project goals:

- large dominant portrait tube;
- stable HUD/status region above the tube;
- tube occupying most of the gameplay region beneath HUD;
- presentation suitable for direct arcade/reference comparison.

Do not use the prototype for:

- current sizing constants;
- tube coordinates;
- HUD coordinates;
- placement;
- aspect calibration;
- automated tests.

It is design context only.

ROM / REFERENCE EVIDENCE

Use project-local evidence only.

Relevant sources may include:

- current LevelLoader geometry definitions;
- RomPresentationMetadataCatalog;
- AtariTubeRecordCatalog preserved Level 7 record-06 data;
- current AtariPresentationAdapter;
- projection/camera code;
- existing Levels 1/2/5/7/8/9 evidence reports.

Do not search the web.

Do not inspect alternative Tempest implementations.

Do not use MAME source as authority.

The supplied MAME/arcade-reference images are visual references only.

NO IMPLEMENTATION

Do not modify:

- source;
- tests;
- configuration;
- documentation;
- geometry;
- presentation;
- viewport;
- gameplay;
- recordings;
- Git state.

Do not add diagnostics.

Do not launch the game.

Do not stage, commit, push, reset, restore, clean, stash, rebase, or switch
branches.

Temporary bounded calculations outside tracked project content are permitted.

Python or PowerShell may be used only as arithmetic/geometry tools.

Do not create tracked analysis artifacts.

AI-LOGS

Follow .github/copilot-instructions.md.

Save:

- this substantive prompt under AI-Logs/Prompts/;
- the paired cross-level proportion investigation report under
  AI-Logs/Reports/.

The required ignored AI-Logs are the only permitted writes.

Do not stage or commit AI-Logs.

REPOSITORY SAFETY

Record at start and finish:

- branch;
- HEAD;
- upstream;
- ahead/behind;
- index;
- tracked worktree;
- relevant untracked state.

Expected committed baseline:

7ba80f39df9daeaa86a7cafc81690f2f5c37bb33

Verify current state.

Note that the accepted enlarged-window/viewport candidate may currently be
uncommitted if it has not yet received a Git closeout.

Preserve it exactly.

Preserve unrelated local instruction/reference files.

Do not attribute pre-existing work to this investigation.

EXPECTED REPORT

Near the beginning answer directly:

1. Is Level 7 actually proportionally broader before the viewport is applied?
2. At what earliest coordinate stage does Level 7 diverge from the successful
   controls?
3. Does Level 7 production geometry itself explain the difference?
4. How does production Level 7 compare mathematically with Atari record 06?
5. Does AtariPresentationAdapter introduce any Level-7-specific X:Y gain?
6. Does the global portrait camera or 960x1280 viewport contribute to the
   Level-7-only difference?
7. Why do Levels 1, 2, and 5 look proportionally closer?
8. Do Levels 8 and 9 support the same conclusion?
9. Is a Level-7-only presentation correction technically justified?
10. If yes, what is the smallest correction family to test later without
    changing gameplay geometry?

The report must include:

- exact evidence sources inspected;
- authorized images inspected, if any;
- image measurements clearly labeled VISUAL DIAGNOSTIC if used;
- Level 1 geometry/projection analysis;
- Level 2 geometry/projection analysis;
- Level 5 geometry/projection analysis;
- Level 7 geometry/projection analysis;
- Level 8 geometry/projection analysis;
- Level 9 geometry/projection analysis;
- production Level 7 versus record-06 mathematical comparison;
- transformation-fit classification;
- cross-level geometry-proportion table;
- cross-level adapter/Jacobian table;
- near/far projected-proportion table;
- host-camera/viewport gain proof;
- first-divergence determination;
- evidence classification;
- whether viewport remains correctly frozen;
- whether Level 7 gameplay geometry remains correctly frozen;
- smallest justified future experiment, if any;
- Level 2 long-term prototype goals recorded only qualitatively;
- confirmation no project/Git state changed except AI-Logs;
- exact prompt/report paths.

Do not ask Neal questions during execution.

Do not pause for manual input.

Complete autonomously and stop after reporting the evidence.

TEMPEST_CROSS_LEVEL_PRESENTATION_PROPORTION_READONLY
