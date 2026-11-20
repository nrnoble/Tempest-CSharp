READ THIS ENTIRE USER-PROVIDED TASK BEFORE TAKING ANY ACTION.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, reading project files, running commands, or taking any
repository action:

1. Locate the unique completion token near the end of this user-provided task.
2. Verify that exactly one complete token beginning with "TEMPEST_" appears on
   a line by itself in the user-provided task.
3. VS Code/agent-generated reminders, tagged-file information, attachment
   metadata, or other system-generated context may appear after the token and
   do not invalidate it.
4. This check is intended to detect an incomplete or truncated user task, not
   appended agent context.
5. If exactly one complete TEMPEST_ token cannot be found, STOP before project
   or repository action and explain why.

TASK

Perform a bounded READ-ONLY investigation of incorrect apparent motion along
tube depth in the current Level 7 --atari-presentation candidate.

The investigation must determine why entities and shots that move at valid
simulation speeds appear to accelerate or decelerate as they travel along the
visible record-06 tube.

Do not implement a correction.

Do not change gameplay speed.

Do not use screenshots as evidence for motion.

MANUAL OBSERVATIONS

Neal manually reports:

1. The Blaster moves normally around the rim.
2. Flippers move normally around the rim.
3. Enemies emerging from the deep/bottom end initially appear to move at normal
   speed, then appear to move progressively slower as they travel toward the
   rim.
4. Blaster shots fired from the rim toward the deep/bottom end initially appear
   slow, then appear to accelerate as they approach the deep end.
5. The enemy and shot symptoms appear to be the same depth-motion problem seen
   in opposite travel directions.
6. Gameplay timing and mechanics otherwise appear to work.
7. The defect existed before the mapped-Flipper transition crash correction.
8. The Level 7 visible tube is now centered, and gameplay presentation is
   registered to that tube.

Treat these as MANUAL OBSERVATIONS.

A strong initial hypothesis is that constant or correct simulation-depth
movement is being transferred through an incorrect depth-to-visible-position
function. This is a hypothesis to test, not an established cause.

RIM-MOTION CONTROL OBSERVATION

The normal-looking Blaster and Flipper rim movement is important control
evidence.

Explicitly determine why motion along the rim remains visually normal while
motion along tube depth does not.

Use that distinction to separate:

- lane/edge-coordinate mapping;
- rim transition mapping;
- depth normalization;
- near/far interpolation;
- perspective transfer;
- entity-specific height/depth conversion;
- and final camera mapping.

Do not investigate rim movement as defective unless the implementation trace
directly contradicts the manual observation.

CRASH-FIX SEPARATION

The prior crash report is:

AI-Logs/Reports/2026-11-16-level7-mapped-flipper-crash-fix-report.md

That work corrected mapped Flipper pivot-transition geometry and did not change:

- record-06 data;
- lane/depth registration;
- centering;
- non-Flipper entities;
- simulation logic;
- gameplay timing;
- recording/playback;
- or VectorRenderer.

Confirm the current diff supports that separation, but do not reopen the crash
fix unless direct evidence proves a dependency.

The present problem predates that correction.

CONTROLLING PRESENTATION CONTEXT

Use the current implementation and the relevant reports, especially:

- the entity-to-record-06 registration and centering investigation;
- the render-registration and centering implementation report;
- the Level 7 presentation implementation report;
- the mapped-Flipper crash-fix report;
- the record-06 presentation and transform reports where needed.

The current intended architecture is:

- production Level 7 gameplay geometry remains authoritative for simulation;
- record-06 geometry is the visible Level 7 presentation surface;
- a render-only adapter maps gameplay lane/depth state onto mapped record-06
  near/far geometry;
- the final playfield camera is shared.

Do not reopen whether record 06 is correct.

PRIMARY QUESTION

What exact mathematical transfer maps gameplay depth or entity height to
visible position along a mapped record-06 lane, and why does that transfer
produce the observed apparent acceleration/deceleration?

Trace the complete depth path for at least:

1. a normal enemy moving from deep to near;
2. a Tanker moving from deep to near;
3. an ordinary Flipper moving from deep to near while not lane-transitioning;
4. a Tanker-released Flipper moving from deep to near;
5. a Spiker moving along depth where applicable;
6. a Blaster/player shot moving from near to deep;
7. an enemy shot moving in its normal direction;
8. a starting Spike base/tip relationship;
9. an Egg at or beyond the far end.

For each class, trace:

- authoritative simulation field;
- units and direction;
- per-tick change;
- any entity-specific multiplier;
- conversion to projected depth;
- normalization to t or equivalent;
- near/far endpoint interpolation;
- presentation scale;
- final camera transform;
- and screen displacement per tick.

Do not assume all entity classes use the same height-to-depth conversion.

MATHEMATICAL QUESTIONS

Answer from code and bounded calculations:

1. Is visible lane position computed with a linear Lerp between mapped near and
   far endpoints?
2. If so, is the interpolation parameter linear in simulation depth?
3. Is the parameter direction correct?
4. Is t=0 consistently the near/rim end?
5. Is t=1 consistently the far/deep end?
6. Are entity heights converted to depth consistently?
7. Are some classes using height while others use projected depth?
8. Are any classes multiplying height by 20 or another factor?
9. Is the far-set-specific presentation translation built into endpoint
   interpolation, and what does that imply for per-tick displacement?
10. Is the anisotropic presentation gain relevant to speed variation along a
    fixed lane?
11. Does the final camera use a uniform affine transform that cannot itself
    cause acceleration?
12. Does any reciprocal-depth or perspective function still apply to some
    classes but not others?
13. Was a previously nonlinear production projection replaced by linear
    endpoint interpolation?
14. If so, was simulation speed calibrated in model/depth space, projected
    screen space, or another coordinate space?
15. Does a correct visible Atari-style presentation require nonlinear depth
    interpolation even though the rail is a straight segment?
16. Is the observed effect explainable solely by lane-dependent rail length or
    orientation?
17. Are normalized-depth increments constant per simulation tick for each
    moving object?
18. Do integer truncation, clamping, extrapolation, or state transitions alter
    those increments?
19. At what precise stage does constant simulation motion become non-uniform
    visible motion?

DO NOT ASSUME LINEAR INTERPOLATION IS WRONG

A straight visible rail can still use either linear or nonlinear progression
along that rail.

Determine what the prior production renderer did.

Trace the previous Level 7 entity presentation path through
AtariPresentationAdapter.Project and/or Projection.Project and derive its
depth-to-screen transfer function.

Compare it with the new record-06 adapter.

Determine whether the old renderer's reciprocal or perspective behavior was
part of visually correct motion, even though its tube geometry was the wrong
presentation surface.

Do not restore the old geometry or old screen positions.

The question is whether the old depth transfer behavior must be translated
onto the new record-06 rails.

OBJECTIVE MOTION TELEMETRY

Use deterministic, noninteractive, bounded calculations or existing test seams.

Do not launch the interactive game.

For representative lanes, calculate visible displacement per simulation tick
at multiple depths.

At minimum sample:

- t near 0.0;
- t near 0.1;
- t near 0.25;
- t near 0.5;
- t near 0.75;
- t near 0.9;
- t near 1.0.

Where production states permit t<0 or t>1, report those separately.

For each representative object class, produce a table containing:

- simulation depth/height before tick;
- simulation depth/height after tick;
- normalized depth before/after;
- current visible logical coordinate before/after;
- current host/screen coordinate before/after;
- current visible displacement magnitude;
- direction of travel;
- applicable lane;
- and any local scale used.

Use at least:

- one long visible rail;
- one short visible rail;
- one rail with a substantially different angle;
- and enough lanes to determine whether the effect is global or lane-specific.

ENEMY/SHOT RECIPROCAL CHECK

Directly compare one enemy and one Blaster shot traversing the same lane in
opposite directions using equal or appropriately normalized simulation-depth
steps.

Determine whether the same mapping predicts:

- enemy deceleration toward the rim;
- and shot acceleration toward the far end.

If it does, identify the shared mathematical cause.

If it does not, identify the class-specific divergence.

Do not rely on verbal intuition; show the calculated progression.

CONSTANT-SPEED CONTROL

Construct a bounded synthetic control that advances a presentation probe by a
constant simulation-depth increment along one mapped lane.

Measure its visible displacement sequence.

Then compare:

A. current Level 7 adapter;
B. prior production Level 7 presentation transfer;
C. pure linear interpolation along the mapped record rail;
D. any candidate transfer already present in the codebase.

This is analysis only. Do not implement a new transfer.

Determine which mathematical family matches the manual observations and which
would preserve appropriate apparent motion.

ARC-LENGTH AND PERSPECTIVE DISTINCTION

Explicitly distinguish:

- constant simulation-depth speed;
- constant normalized-depth speed;
- constant parameter speed;
- constant visible rail arc-length speed;
- perspective-correct apparent motion;
- and historically accurate Atari motion, if established.

Do not assume that constant screen-pixel speed is necessarily arcade-correct.

The goal is to identify why the current motion is visibly implausible relative
to Neal's direct gameplay observation, not to impose a generic constant-pixel
rule.

If Atari/ROM evidence establishes depth-speed behavior, use it.

If not, classify any proposed visual transfer as a PROJECT TRANSLATION
candidate rather than Atari fact.

GAMEPLAY PROTECTION

Treat current gameplay behavior as protected.

Do not propose changing:

- enemy simulation speeds;
- shot simulation speeds;
- Level 7 timing;
- movement updates;
- entity height increments;
- shot velocity;
- firing cadence;
- gameplay depth;
- collision;
- spawning;
- RNG;
- score;
- lives;
- recording/playback state.

A future correction, if justified, should be render-only unless evidence proves
the simulation itself is wrong.

Do not change code in this task.

RIM-MOTION PATH

Trace why rim motion is unaffected:

- Blaster edge-coordinate movement;
- Flipper rim lane movement;
- mapped near-rim interpolation;
- mapped Flipper transition geometry;
- final camera.

Determine whether rim motion bypasses the problematic depth mapping entirely.

State whether this control observation supports the depth-transfer diagnosis.

ENTITY-SPECIFIC BOUNDARIES

Identify any class that cannot use one shared render-depth transfer because of
different semantics.

Pay particular attention to:

- Eggs with t>1;
- Spikes using separate base and tip depths;
- Flipper lane transitions;
- shots whose height units differ from enemies;
- Tanker cargo;
- effects anchored from destroyed entities;
- rim objects at t=0.

Do not force all classes into one function unless the code and units justify it.

RECORDING AND PLAYBACK

Determine whether visible deterministic playback can be used later for manual
motion acceptance without changing the gameplay recording format.

A render-only transfer should leave recorded input and deterministic simulation
unchanged.

Verify that conclusion from architecture and tests rather than assuming it.

IMAGES AND VIDEO

Screenshots cannot resolve this motion problem and are not authorized for this
task.

Do not search for, enumerate, open, inspect, or analyze screenshots, images,
videos, frames, contact sheets, or visual assets.

Do not ask Neal to provide video during this investigation.

Use deterministic calculations and code tracing.

READ-ONLY REQUIREMENT

Do not modify:

- source;
- tests;
- project files;
- configuration;
- durable documentation;
- geometry;
- projection;
- presentation;
- gameplay;
- recording/playback;
- or Git state.

Do not add diagnostics or telemetry to production code.

Temporary bounded calculations outside tracked project content are permitted.

Do not stage, commit, push, reset, restore, stash, rebase, clean, or switch
branches.

AI-LOGS

Follow .github/copilot-instructions.md.

The normal ignored AI-Logs files are required and are the permitted
file-writing exception.

Save:

- this substantive prompt under AI-Logs/Prompts/;
- the completed investigation report under AI-Logs/Reports/.

Use descriptive date-prefixed paired filenames.

A chat response is not a substitute for the report file.

Do not stage or commit AI-Logs.

REPOSITORY SAFETY

At the beginning, record:

- branch;
- HEAD;
- upstream/tracking state;
- index;
- tracked worktree;
- relevant untracked state.

Preserve all pre-existing work.

At completion, verify that no project or Git state changed except the required
ignored AI-Logs records.

EXPECTED REPORT

Save the report under AI-Logs/Reports/ and summarize it in chat.

Near the beginning, directly answer:

1. Why do enemies visually decelerate as they move from the deep end toward the
   rim?
2. Why do Blaster shots visually accelerate as they move from the rim toward
   the deep end?
3. Why do Blaster and Flipper rim movements remain normal?
4. Is simulation speed correct while presentation speed is distorted?
5. What exact depth-to-visible-motion transfer causes the result?

The report must include:

1. Manual observations and evidence classification.
2. Current depth-to-visible-position formula.
3. Per-class simulation fields and units.
4. Per-class depth normalization.
5. Current near/far interpolation formula.
6. Prior production presentation formula.
7. Final camera formula and whether it can cause acceleration.
8. Deterministic telemetry tables at multiple depths.
9. Long-, short-, and differently angled-lane comparisons.
10. Enemy-versus-Blaster-shot reciprocal comparison.
11. Constant-depth-step control.
12. Linear-interpolation control.
13. Prior-presentation-transfer control.
14. Exact stage where visible speed becomes distorted.
15. Explanation for normal rim motion.
16. Whether one shared render-depth transfer is justified.
17. Entity classes requiring special treatment.
18. Egg t>1 implications.
19. Spike base/tip implications.
20. Flipper-transition implications.
21. Recording/playback impact.
22. Whether enough evidence exists for a bounded implementation prompt.
23. If yes, the smallest technically justified render-only correction
    direction without implementing it.
24. If no, the single narrowest remaining uncertainty.
25. Confirmation that gameplay speeds and simulation remain unchanged.
26. Confirmation that no source, tests, presentation, gameplay, protected
    material, or Git state changed.
27. Exact paths of the saved prompt and report.

Do not ask Neal questions during execution.

Do not pause for manual input.

Complete the bounded read-only investigation autonomously and stop after
reporting the evidence.

TEMPEST_LEVEL7_DEPTH_TO_VISIBLE_MOTION_READONLY_INVESTIGATION