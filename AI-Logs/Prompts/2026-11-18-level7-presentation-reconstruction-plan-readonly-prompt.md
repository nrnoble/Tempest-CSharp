READ THIS ENTIRE USER-PROVIDED TASK BEFORE TAKING ANY ACTION.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, reading project files, running commands, or taking any
repository action:

1. Locate the unique completion token near the end of this user-provided task.
2. Verify that exactly one complete token beginning with "TEMPEST_" appears on
   a line by itself.
3. Automatically appended VS Code/agent context after the token does not
   invalidate it.
4. This check detects an incomplete or truncated user task, not appended agent
   context.
5. If exactly one complete token cannot be found, STOP before repository action
   and explain why.

TASK

Perform a bounded READ-ONLY Level 7 Atari-presentation reconstruction and
removal-plan investigation.

Do not implement, delete, restore, revert, or modify code in this task.

The objective is to design a clean replacement for the current special Level 7
presentation architecture using the successful architectural principles from
Levels 8, 9, and diagnostic Level 10.

The deliverable is a precise implementation plan containing:

- what Level-7-specific experimental presentation code should be removed;
- what Atari/ROM-derived Level 7 data should remain;
- which shared presentation path Level 7 should use instead;
- how tube and entities will remain registered without a special entity mapper;
- how placement should be calculated;
- how to prevent recurrence of the depth-motion regression;
- exact tests and validation required for a later implementation milestone.

Do not perform the reconstruction yet.

USER DECISION

Neal has decided to stop incrementally repairing the current Level 7
presentation candidate.

The intended next milestone is a clean Level 7 presentation reconstruction.

This means:

- preserve the working Level 7 gameplay and simulation;
- preserve valid Atari/ROM-derived Level 7 geometry and metadata;
- remove unnecessary Level-7-only presentation machinery;
- use the simpler successful Levels 8–10 architecture where justified;
- avoid another screenshot-fitted or separately interpolated geometry system.

Do not interpret "wipe/reconstruct" as permission to delete code during this
read-only investigation.

CURRENT ACCEPTED BASELINE

Begin from the current committed baseline after ExitZoom focal-compensation
closeout.

Expected baseline:

aee59b39c1d68be830d0014a7b9497d1bb40f3c7

on master tracking origin/master at 0 ahead / 0 behind.

Verify actual state rather than assuming it.

The accepted ExitZoom focal-compensation architecture is frozen:

- closed tubes use mapped far-rim center;
- open tubes use mapped rail convergence;
- gameplay and exit simulation remain unchanged.

Preserve all accepted work in the reconstruction plan.

PROBLEM STATEMENT

The unresolved Level 7 presentation defect is severe and specific:

- with Atari presentation disabled, Level 7 playback looks normal;
- with Atari presentation enabled, Level 7 visibly exhibits the reported
  depth-motion problem;
- enemies traveling from deep to near appear progressively slower;
- player shots traveling from near to deep appear to accelerate;
- Blaster and Flipper rim movement appear normal;
- deterministic gameplay and progression remain functional;
- other successfully implemented presentation levels do not exhibit the same
  characteristic defect.

Level 7 gameplay itself is not currently considered defective.

Treat the problem as presentation-specific unless direct evidence proves
otherwise.

CONTROLLING HISTORICAL EVIDENCE

Inspect the relevant reports and current source, including at minimum:

- Level 7 record-06 reconstruction reports;
- Level 7 reference-shape investigation;
- Level 7 presentation diagnostic and implementation reports;
- entity-to-tube registration and centering investigation;
- render-registration implementation report;
- mapped-Flipper crash correction;
- depth-to-visible-motion investigation;
- regression-delta investigation;
- visible-runtime instrumentation;
- good/bad runtime A/B analysis;
- framebuffer diagnostic reports where relevant;
- successful Level 8 implementation/closeout;
- successful Level 9 implementation/closeout;
- successful Level 10 geometry/presentation records;
- accepted Blaster and ExitZoom closeouts.

Use reports to reconstruct the historical architecture and regression boundary.

Do not treat every historical experiment as accepted design.

REGRESSION BOUNDARY

The previous regression-delta report identifies the Level 7 entity-registration
milestone as the narrowest supported boundary:

BEFORE REGISTRATION

- the visible Level 7 tube used record-06 presentation;
- gameplay entities still used the production Level 7 geometry through
  AtariPresentationAdapter.Project;
- gameplay motion appeared normal;
- tube and entities were visibly misregistered.

AFTER REGISTRATION

- gameplay entities were moved onto record-06 near/far lane interpolation via
  AtariLevel7EntityPresentation;
- tube and entities became visibly registered;
- the severe manual depth-motion regression appeared;
- local entity scale also changed from adapter Jacobian scale to mapped
  lane-width ratio.

Confirm this boundary from current evidence.

Do not assume the special entity mapper can or should be repaired.

Measure its architectural cost and deletion consequences.

SUCCESSFUL LEVELS 8–10 MODEL

Study the successful architecture used by Levels 8, 9, and diagnostic 10.

Identify the shared principles that made those milestones simple and stable:

- one authoritative level geometry used by gameplay and presentation;
- tube and gameplay entities share one presentation projector;
- entity registration follows naturally from shared geometry/projection;
- no second visual tube is overlaid on a separate gameplay tube;
- no Level-specific entity lane/depth interpolation system;
- no screenshot-derived point corrections;
- placement is applied as one final presentation translation;
- simulation remains independent from rendering;
- ExitZoom uses the accepted shared focal-compensation architecture.

Do not copy Level 8/9 values into Level 7.

Copy the architectural method only.

LEVEL 7 DATA AUTHORITY

Determine exactly what Level 7 data should survive reconstruction.

At minimum classify:

1. production Level 7 gameplay geometry;
2. Atari record-06 raw data;
3. RomPresentationMetadata for displayed Level 7;
4. level topology and lane count;
5. gameplay lane identity/order;
6. current record-06 point order;
7. current near/far projector inputs;
8. current calibrated presentation constants;
9. current centering values;
10. current lane-correspondence assumptions.

For each item label it as:

- ROM/Atari-backed;
- current production gameplay authority;
- accepted project translation;
- development-only experiment;
- screenshot-derived calibration;
- unresolved assumption;
- removable historical diagnostic.

Do not assume record 06 must be discarded merely because the current
presentation architecture failed.

Also do not assume record 06 must remain the active presentation geometry if it
cannot share the authoritative gameplay geometry cleanly.

CENTRAL ARCHITECTURE QUESTION

Determine which clean architecture is technically justified.

Evaluate at least these alternatives:

OPTION A

Use the current production Level 7 gameplay geometry as the shared geometry,
then present tube and entities through the same AtariPresentationAdapter path,
as Levels 8–10 do.

OPTION B

Replace the production Level 7 gameplay definition's coordinates with the
validated record-06-derived geometry so gameplay and presentation share one
geometry, only if this can preserve accepted gameplay mechanics and collision
behavior exactly.

OPTION C

Keep separate gameplay and record-06 geometry but create a simpler generic
coordinate mapping without Level-7-only entity dispatch.

This option should be treated skeptically because it risks recreating the same
two-tube architecture.

OPTION D

Another architecture directly established by current source and evidence.

For every option state:

- gameplay impact;
- collision impact;
- lane identity impact;
- entity registration method;
- depth mapping;
- local scale method;
- Flipper transition handling;
- open/closed topology implications;
- ExitZoom compatibility;
- implementation complexity;
- removal opportunities;
- regression risk;
- ability to preserve accepted Level 7 mechanics.

Recommend one option.

Do not recommend the current architecture merely because it already exists.

GAMEPLAY GEOMETRY PROTECTION

Level 7 gameplay has extensive accepted mechanics.

The future reconstruction must not alter:

- Level 7 lane topology;
- Tanker behavior;
- ordinary and cargo Flipper behavior;
- Spikers;
- Spikes;
- enemy shots;
- player shots;
- collision;
- wave supply;
- completion;
- Super Zapper behavior;
- scoring;
- lives;
- timing;
- RNG;
- recording/playback simulation.

Investigate whether switching active geometry would affect any of these.

If record-06 coordinates differ materially from production gameplay points,
quantify the consequences before recommending replacement.

Do not assume "same 16 lanes" means identical gameplay geometry.

CURRENT SPECIAL COMPONENT INVENTORY

Map every current consumer, test, CLI option, report-supported diagnostic, and
code dependency for at least:

- AtariLevel7Presentation;
- AtariLevel7EntityPresentation;
- AtariTubeRecordCatalog.Level7Record06;
- AtariTubeVectorListBuilder;
- AtariCoordinateProjector;
- AtariDvgViewportMap;
- AtariTubeReferenceShapeDiagnostic;
- Level7VisibleRuntimeDiagnostic;
- Level7FramebufferDiagnostic;
- Level7-specific renderer dispatch;
- Level7-specific entity projection helpers;
- Level7-specific Flipper transition handling;
- Level7-specific centering/placement constants;
- factor-128 or other historical diagnostic controls;
- Level7-specific tests.

For each component classify:

- retain unchanged;
- retain but generalize/share;
- remove during reconstruction;
- remove after replacement validation;
- preserve as ignored historical evidence only;
- unresolved pending implementation evidence.

Include exact file and symbol names.

Do not delete diagnostics merely because they are no longer needed without
checking whether current project instructions or tests require them.

REMOVAL PLAN

Produce an ordered removal plan that avoids leaving Level 7 temporarily broken
or tests misleadingly green.

Prefer this sequence where appropriate:

1. add replacement shared Level 7 presentation path;
2. add equivalence/protection tests;
3. route Level 7 tube and entities through replacement;
4. validate gameplay/model state;
5. remove obsolete Level-7-only dispatch;
6. remove obsolete helpers/constants;
7. remove or rewrite obsolete tests;
8. remove obsolete CLI diagnostics only when no longer needed;
9. run final validation;
10. stop for manual acceptance.

Do not prescribe deletion-first if that would remove the current comparison
path before replacement evidence exists.

DEPTH-MOTION SAFETY

The future architecture must prevent recurrence of the current defect.

Specify one explicit contract:

For identical Level 7 gameplay states, tube geometry and all gameplay entities
must be projected through one coherent presentation coordinate system.

No entity class may use:

- a separate linearly interpolated visual tube while others use reciprocal
  projection;
- a different depth convention;
- a separate camera;
- a duplicated placement transform;
- a Level-specific local-scale formula without evidence.

Determine what the successful Levels 8–10 path uses for:

- anchor projection;
- local Jacobian/scale;
- near/far relationship;
- shot movement;
- entity orientation.

Recommend that same shared contract for Level 7 where applicable.

ENTITY REGISTRATION

Define how the reconstructed architecture will keep the following attached to
the visible tube without a special two-geometry mapper:

- Blaster;
- Tankers;
- ordinary Flippers;
- Tanker cargo Flippers;
- Spikers;
- Eggs;
- Spikes;
- player shots;
- enemy shots;
- effects;
- lane highlights.

For each identify the authoritative gameplay state and the shared presentation
operation.

Do not include per-entity visual offsets except existing artwork-local geometry.

FLIPPER TRANSITIONS

The special mapped-Flipper transition implementation previously required a
crash correction because record-06 mapped hinge geometry differed from gameplay
hinge geometry.

Determine whether the reconstructed shared-geometry path can return to the
normal directed transition renderer used by successful levels.

If yes, identify which special Level 7 transition code/tests become removable.

If no, explain the exact remaining Level-7-specific need.

Preserve long directed transition semantics, including 270-degree routes.

PLACEMENT

Do not choose a new Level 7 final placement value during this read-only task.

Define how the future implementation should calculate placement after the clean
shared projection is active:

- calculate full near/far tube envelope;
- preserve horizontal center;
- center vertically in the HUD-cleared gameplay composition;
- use one shared final translation;
- apply the same translation to tube and entity anchors;
- do not alter geometry to fix placement.

The current experimental Level 7 placement values should not automatically
survive reconstruction.

Classify them for removal or recalculation.

PERSPECTIVE AND SCALE

Preserve the accepted whole-game perspective architecture.

Do not repeat:

- record-06 screenshot fitting;
- DVG aspect investigation;
- factor-128 investigation;
- analog hardware investigation;
- screenshot-derived far-set translation experiments.

Determine whether the reconstructed Level 7 path can use the same shared
projection/perspective model as Levels 8–10.

If Level 7 metadata requires a legitimate per-level adapter input, identify it
from existing data rather than visual fitting.

EXITZOOM AND APPROACH

The recently accepted ExitZoom focal rules are frozen:

- closed tubes use mapped far-rim center;
- open tubes use mapped rail convergence.

Determine how reconstructed Level 7 will participate in this shared path.

Do not modify ExitZoom in this task.

Preserve the accepted Blaster ExitZoom/Approach scale correction.

The implementation plan must include regression coverage proving the
reconstruction does not disturb those accepted systems.

DIAGNOSTIC CLEANUP

Determine which Level 7 diagnostics remain useful after reconstruction.

Potentially obsolete diagnostics include:

- special record-06 prototype selector;
- factor-128 control;
- reference-shape selector;
- visible-runtime motion logging;
- framebuffer diagnostic;
- special screenshot capture controls tied only to the failed architecture.

Do not assume all should be removed.

Classify each as:

- still useful general tool;
- useful until reconstruction acceptance, then remove;
- obsolete immediately after replacement;
- historical AI-Logs evidence only.

State whether removing any diagnostic changes public CLI/help text or tests.

TEST MIGRATION PLAN

Produce a concrete test plan for the implementation milestone.

At minimum include:

A. DATA AND GEOMETRY

- authoritative Level 7 geometry source;
- point order;
- topology;
- lane count;
- metadata;
- no screenshot-derived constants.

B. SHARED PRESENTATION

- tube and entities use the same projector;
- one depth convention;
- one final camera;
- one placement transform;
- one local-scale authority.

C. GAMEPLAY PROTECTION

- deterministic Level 7 simulation equivalent before/after rendering;
- Level 7 mechanics unchanged;
- recording/playback state unchanged;
- collision/model state unchanged.

D. MOTION REGRESSION

- use the same genuine Level 7 recording with presentation off/on;
- compare identical gameplay states;
- ensure the replacement path does not route through
  AtariLevel7EntityPresentation;
- verify no Level-7-only depth interpolation;
- preserve normal rim movement;
- preserve player/enemy shot direction and timing.

Automated calculations cannot replace Neal's visual acceptance.

E. ENTITY COVERAGE

- Blaster;
- Tanker;
- ordinary/cargo Flipper;
- Spiker;
- Egg;
- Spike;
- player shot;
- enemy shot;
- effects.

F. TRANSITIONS

- directed Flipper pivots;
- ExitZoom;
- Approach;
- ReverseZoom or exit retry where applicable.

G. CROSS-LEVEL PROTECTION

- accepted Levels 1–6;
- Levels 8 and 9;
- diagnostic Level 10;
- accepted Blaster orientation;
- accepted Blaster travel scale;
- accepted ExitZoom focal behavior.

VALIDATION PLAN

Recommend exact bounded validation groups for the future implementation:

- focused Level 7 reconstruction tests;
- targeted Level 7 gameplay/presentation tests;
- recording/playback;
- transition/exit/Approach;
- Levels 1–10 regression selection;
- full solution tests with hang timeout;
- Release build;
- git diff --check.

Do not run broad implementation validation in this read-only planning task
unless a narrow existing test is required to verify a factual dependency.

No interactive game launch.

MANUAL ACCEPTANCE PLAN

Define a bounded manual acceptance sequence for the future implementation.

Prefer:

1. direct Level 7 ordinary gameplay;
2. genuine Level 7 deterministic playback;
3. compare presentation off/on for gameplay timing and motion;
4. verify tube shape and placement;
5. verify Blaster and entities remain on visible lanes;
6. verify enemy deep-to-rim motion;
7. verify player-shot rim-to-deep motion;
8. verify rim movement;
9. verify Flipper transitions;
10. verify exit and Approach.

State that Neal's manual testing remains final authority.

NO IMPLEMENTATION

Do not modify:

- source;
- tests;
- configuration;
- documentation;
- CLI;
- presentation;
- gameplay;
- recordings;
- Git state.

Do not delete files.

Do not add replacement code.

Do not stage, commit, push, reset, restore, clean, stash, rebase, or switch
branches.

Do not launch the game.

No image/video/frame analysis is authorized.

Do not inspect TEMP/myNotes.md.

Temporary bounded external calculations are permitted only if needed to compare
existing geometry or dependencies.

AI-LOGS

Follow .github/copilot-instructions.md.

Save:

- this substantive prompt under AI-Logs/Prompts/;
- the paired Level 7 reconstruction-plan report under AI-Logs/Reports/.

These ignored records are the only permitted writes.

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

aee59b39c1d68be830d0014a7b9497d1bb40f3c7

Verify actual state.

Preserve unrelated local work, including if present:

- .github/copilot-instructions.md;
- local arcade reference files.

EXPECTED REPORT

Near the beginning answer directly:

1. What current Level-7-specific presentation components should be removed?
2. What Level 7 data and behavior must remain?
3. Which successful Levels 8–10 architecture should replace them?
4. Should Level 7 use production gameplay geometry or record-06-derived
   geometry as the one shared authority?
5. Can the special AtariLevel7EntityPresentation path be removed completely?
6. Can special Flipper transition presentation be removed?
7. Which diagnostics and tests become obsolete?
8. What exact implementation order minimizes risk?
9. What are the primary reconstruction risks?
10. Is the evidence sufficient for one bounded implementation prompt?

The report must include:

- current repository state;
- historical regression boundary;
- current Level 7 architecture diagram or structured description;
- successful Levels 8–10 comparison;
- geometry/data authority table;
- alternative architecture evaluation;
- recommended replacement architecture;
- exact retain/remove/generalize inventory;
- consumer/dependency map;
- ordered replacement/removal plan;
- gameplay-protection plan;
- entity-registration plan;
- depth-motion safety contract;
- Flipper transition plan;
- placement calculation plan;
- ExitZoom/Approach compatibility plan;
- diagnostic cleanup plan;
- test migration plan;
- validation plan;
- manual acceptance plan;
- risks and stop conditions;
- confirmation no project/Git state changed except required AI-Logs;
- exact prompt/report paths.

Do not ask Neal questions during execution.

Do not pause for manual input.

Complete autonomously and stop after delivering the read-only reconstruction
plan.

TEMPEST_LEVEL7_PRESENTATION_RECONSTRUCTION_PLAN_READONLY