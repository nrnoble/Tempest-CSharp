IMPORTANT COMPLETENESS CHECK

Read this entire user-provided task before taking any action.

Before using tools, reading project files, running commands, modifying files, or performing repository actions, locate the unique completion token near the end of this task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code or agent-generated reminders, tagged-file information, attachment metadata, active/current editor files, or other automatically appended context may appear after the user's completion token and do not invalidate it.

This check exists to detect an incomplete or truncated user task, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before taking any project or repository action and explain why.


AUTOMATIC / ACTIVE-EDITOR CONTEXT IS NOT AUTHORIZATION

Files automatically surfaced by VS Code because they are open, active, recently viewed, suggested, or otherwise included as editor/chat context are NOT authorized merely because VS Code shows them to the agent.

Do not read or use automatically surfaced files unless this task expressly authorizes them or they are directly necessary within the bounded implementation below.

In particular:

DO NOT read TEMP/myNotes.md.

TEMP/myNotes.md is Neal's personal scratchpad and is not project evidence unless Neal explicitly authorizes it in the current user task.


TASK

Implement the endpoint-normalized baseline-referenced formulation recommended by the completed read-only perspective/transition investigation.

Replace only the current live-amplified portrait-perspective correction.

The new formulation must:

- reproduce the current stable-play portrait result exactly at K=70 and ZOffset=5;
- preserve the accepted near-rim placement;
- preserve the established stable far-rim correction;
- prevent the added portrait-perspective term from being multiplied into extreme values by animated ExitZoom/ReverseZoom K and ZOffset;
- remain projectively coherent with the live camera;
- keep intermediate tube points on the straight rails rendered between near and far endpoints;
- keep tube-relative gameplay anchors registered with equivalent tube-space anchors;
- remain continuous, stateless, deterministic, and reversible;
- preserve the existing ExitZoom/ReverseZoom base-camera animation;
- preserve Approach behavior using its shifted near/far depth context;
- keep the current --portrait-perspective range at [-5,+5];
- keep the default at 0;
- make no range expansion in this milestone;
- remain unstaged, uncommitted, and unpushed.

This is an implementation and validation milestone for transition-safe perspective composition.

It is NOT a human-visible range-expansion milestone.


CURRENT AUTHORITATIVE REPOSITORY STATE

Repository baseline:

branch:
master

HEAD:
385f603a94391756b2ca6cccd524dc17b281e782

commit:
Close out ROM-backed enemy-shot rendering

origin/master should remain at the same commit.

The current worktree intentionally contains exactly nine modified tracked candidate files:

- src/Tempest.Game/Debug/DebugController.cs
- src/Tempest.Game/Program.cs
- src/Tempest.Game/Rendering/DisplayConfiguration.cs
- src/Tempest.Game/Rendering/Projection.cs
- src/Tempest.Game/Rendering/VectorRenderer.cs
- tests/Tempest.Game.Tests/DebugControllerTests.cs
- tests/Tempest.Game.Tests/DisplayConfigurationTests.cs
- tests/Tempest.Game.Tests/GameplayRecordingTests.cs
- tests/Tempest.Game.Tests/TransitionPresentationTests.cs

The index is expected to be clean.

No relevant non-ignored untracked files are expected.

Do not restore, reset, clean, discard, overwrite, stage, commit, or push the current candidate.


STARTING SAFETY CHECK

Before implementation, run bounded checks of:

- branch;
- HEAD;
- origin/master;
- staged changes;
- tracked worktree changes;
- non-ignored untracked files;
- diff summary.

Expected:

- branch master;
- HEAD and origin/master at 385f603a94391756b2ca6cccd524dc17b281e782;
- clean index;
- exactly the nine intentional tracked candidate files modified;
- no relevant non-ignored untracked files.

If the repository differs materially, STOP before implementation and report the discrepancy.

Do not alter anything to force the expected state.


AUTHORITATIVE REPORTS

Read:

AI-Logs/Reports/2026-11-14-whole-game-presentation-shared-perspective-experiment-report.md

AI-Logs/Reports/2026-11-14-whole-game-presentation-perspective-range-expansion-report.md

AI-Logs/Reports/2026-11-14-whole-game-presentation-perspective-transition-interaction-readonly-report.md

The perspective/transition interaction report is authoritative for this implementation.

It classified the next step as:

READY FOR BASELINE-REFERENCED PERSPECTIVE CORRECTION

Specifically, it recommended the endpoint-normalized formulation.

Do not confuse that recommendation with the simpler direct stable-baseline q0(z) candidate. The direct pointwise candidate failed projective-line coherence during transient ZOffset values and must not be implemented.


NO VISUAL-ASSET ANALYSIS

Do not search for, enumerate, open, inspect, generate, or analyze:

- screenshots;
- images;
- videos;
- MAME frames;
- C# frames;
- gameplay recordings;
- playback files;
- contact sheets;
- comparison images;
- active-editor images.

Do not inspect Gemini material, Incitti source, web sources, or raw ROM files.

Use only current source, the three reports above, and bounded source/math analysis.


EXISTING FORMULA TO REPLACE

The current added correction is based on live K and live ZOffset:

qLive(z) = 1 / (z + ZOffset)
qLiveN   = 1 / (near + ZOffset)

deltaYCurrent = a * K * (qLive(z) - qLiveN)

This is coherent during any single live-camera frame, but it treats the portrait coefficient like a model-space displacement under the transient camera.

During ExitZoom and ReverseZoom, animated K and ZOffset amplify the correction severely.

Replace only this correction behavior.

Do not change the existing base projection:

xProjected = 400 + x * K / (z + ZOffset)

yBase = CenterY + (y + YOffset) * K / (z + ZOffset)


IMPLEMENT THE ENDPOINT-NORMALIZED FORMULATION

Use the active near and far world depths for the current projection context.

Define:

qLive(z) = 1 / (z + ZOffset)
qLiveN   = 1 / (near + ZOffset)
qLiveF   = 1 / (far + ZOffset)

u(z) = (qLiveN - qLive(z)) / (qLiveN - qLiveF)

Define the stable baseline reference using:

BaselineK = 70
BaselineZOffset = 5

q0N = 1 / (near + BaselineZOffset)
q0F = 1 / (far + BaselineZOffset)

deltaY = a * BaselineK * (q0F - q0N) * u(z)

The final projected Y is:

yProjected = yBase + deltaY

Requirements:

- At z=near, u=0 and deltaY=0.
- At z=far, u=1 and deltaY equals the established stable far correction.
- At stable K=70 and ZOffset=5, the new formula must reproduce the previous formula at every depth within narrow floating-point tolerance.
- X projection must remain unchanged.
- The correction magnitude must not scale with animated K.
- The correction endpoints must not amplify as live ZOffset approaches -41.
- The interpolation must remain affine in qLive so intermediate points stay on the rendered endpoint-to-endpoint rail.
- No clamping of u is authorized merely to hide out-of-range behavior.
- Handle valid outside-tube depths according to the same projective equation and validate them explicitly.
- Reject or fail safely for singular/non-finite projection contexts rather than silently generating invalid coordinates.

Use the report's mathematics as authority, but inspect current source signatures and architecture before choosing the smallest maintainable implementation.


ACTIVE NEAR/FAR PROJECTION CONTEXT

The current central projection path carries a near anchor but may not carry the active far endpoint.

Add only the bounded context plumbing required to provide consistent active near and far depths to the shared projection calculation.

For ordinary production play:

- near = active level/configuration Position;
- far = near + active level/configuration Depth.

For Approach:

- both near and far must use the same established Approach depth offset;
- near = Position + Approach.DepthOffset;
- far = Position + Depth + Approach.DepthOffset.

For ExitZoom/ReverseZoom:

- preserve the established world-depth range and live K/ZOffset animation;
- use the active level's near/far tube depths;
- do not alter exit timing, player depth advancement, camera restoration, or state transitions.

For Game Over and other stable restored contexts:

- use the appropriate active level/configuration near/far references;
- do not introduce a separate projection model.

Do not hard-code production near=50 and far=450 in the public/shared implementation when active configuration values are available.

Baseline K=70 and BaselineZOffset=5 may be explicit named presentation/project constants because those define the accepted stable reference used by the candidate.


SHARED PROJECTION PATH

Apply the endpoint-normalized correction through the smallest centralized shared projection path.

Ensure equivalent tube-space world anchors receive the same:

- live K;
- live ZOffset;
- active near depth;
- active far depth;
- perspective coefficient;
- Approach offset context where applicable.

Confirm coherent treatment for:

- tube vertices and rails;
- Blaster anchors;
- enemies;
- Spikers;
- Spikes;
- player shots;
- enemy shots;
- gameplay-space effects;
- score popups;
- applicable transition geometry;
- Game Over vectors where they use the shared playfield projection.

Do not add:

- tube-only transforms;
- entity-only offsets;
- per-entity formulas;
- per-level coefficients;
- open/closed branches;
- screenshot-specific logic.

If directly equivalent tube/entity anchors cannot receive the same near/far context without broader redesign, STOP and report an architectural blocker.


FROZEN VALUES AND BEHAVIOR

Do not change:

- --portrait-perspective range: [-5,+5];
- default perspective coefficient: 0;
- parser semantics;
- command-line option name;
- legacy conflict behavior;
- host window: 1280 x 960;
- portrait viewport: 720 x 960;
- logical portrait presentation: 600 x 800;
- playfield scale: 1.05;
- shared vertical translation: -33.1;
- horizontal placement;
- K=70 stable value;
- ZOffset=5 stable value;
- live ExitZoom/ReverseZoom K/ZOffset animation;
- level geometry;
- CenterY;
- YOffset;
- gameplay/model coordinates;
- collisions;
- movement;
- firing;
- timing;
- randomness;
- recording schema;
- playback input interpretation;
- HUD layout;
- final text presentation;
- legacy presentation.

Do not expand the coefficient range even if the new formula appears capable of supporting it.

A later milestone will address the human-visible range only after this composition passes automated and manual transition validation.


PROJECTIVE-LINE COHERENCE

Prove and test that the new formulation remains affine in qLive for each rendered frame.

At minimum, test near, far, and representative intermediate depths for:

- stable K=70, ZOffset=5;
- early ExitZoom;
- ExitZoom boundary at K=70, ZOffset=-41;
- representative high-K ExitZoom;
- representative early ReverseZoom;
- representative middle ReverseZoom;
- representative later ReverseZoom;
- full-offset Approach;
- late Approach;
- restored stable play.

Cover representative production geometries:

- closed Level 1;
- closed Level 6;
- open Level 9.

The final projected intermediate point must remain collinear with the projected near/far endpoints within a narrow floating-point tolerance.

Do not rely only on proof or only on tests. Record both.


STABLE EQUIVALENCE

At stable K=70 and ZOffset=5, verify that the new endpoint-normalized formula reproduces the previous perspective candidate at representative depths and coefficients.

Test:

- coefficient 0;
- coefficient +5;
- coefficient -5;
- near;
- far;
- multiple intermediate depths;
- representative Levels 1, 6, and 9;
- representative world anchors.

The existing neutral portrait result and the existing ±5 stable calibration must remain visually and numerically unchanged within narrow tolerance.


TRANSITION AMPLIFICATION PROTECTION

Verify mathematically and through focused tests that the added correction is bounded by its stable near/far endpoint displacement during in-tube ExitZoom/ReverseZoom contexts.

At coefficient +5:

- the far correction should retain the stable +5 magnitude rather than rising toward hundreds of pixels;
- near correction remains zero;
- intermediate corrections remain on the live projective rail.

Test representative states including:

- K=70, ZOffset=-41;
- K=440, ZOffset=-41;
- representative ReverseZoom K/ZOffset pairs from the report.

Do not assert that the base exit camera never clips.

Distinguish:

- clipping/movement created by the pre-existing base transition camera;
- any additional displacement created by the perspective term.

The new correction must not introduce live-K/live-Z amplification.


CONTINUITY

The new formula must be a pure function of:

- world point/depth;
- active near/far depth;
- live K/ZOffset;
- stable baseline constants;
- perspective coefficient.

No hidden mutable state.
No added fade state.
No transition history.
No new snapshots.
No recording fields.

Test continuity or bounded frame-to-frame behavior across:

- stable play to first ExitZoom update;
- ExitZoom phase boundary;
- high-K exit progression;
- fatal collision into ReverseZoom;
- ReverseZoom restoration;
- successful exit into Approach/restored projection;
- Approach completion into standard play.

Do not add gating at state boundaries.


TUBE/ENTITY REGISTRATION

Add focused tests confirming directly equivalent tube-space and gameplay-space anchors receive identical final mapping at:

- stable play;
- ExitZoom boundary;
- representative high-K exit;
- ReverseZoom;
- Approach.

Include representative anchors for:

- player/Blaster;
- shot;
- Spike or lane-centered point;
- enemy-centered point where practical.

Do not impose false constraints on local glyph geometry, Flipper pivot animation, deliberate cross-lane movement, or explosion shapes.

Test common world-anchor registration only.


OUTSIDE-TUBE AND NONSTANDARD DEPTHS

Validate finite, coherent behavior for directly relevant depths outside the near/far interval, including:

- enemy retraction depth;
- behind-pit/other existing extended rendering depth;
- Game Over projected depths;
- Approach-offset tube depths;
- player exit depths.

Because u is not clamped, document and test expected projective extrapolation.

Do not add clamping unless the current architecture already requires it for a proven safety condition.

If an existing relevant depth produces singular or non-finite behavior under a real production camera state, STOP and report the blocker.


PLAYBACK AND REVERSE STEPPING

Confirm deterministic reconstruction:

- the same simulation/render state plus the same runtime coefficient results in the same projected output;
- forward and reverse navigation reconstruct equivalent projection state;
- no recording schema or metadata changes are required;
- perspective remains a process-level presentation option;
- the same recording may be viewed under different perspective values without changing simulation results.

Use bounded tests. Do not require interactive playback.


LEGACY PRESERVATION

Legacy presentation must remain unchanged.

The endpoint-normalized correction must be inactive in legacy mode.

Preserve current rejection of:

--legacy-presentation --portrait-perspective <value>

Do not update legacy snapshots or expectations merely to absorb candidate changes.


FAIL-CLOSED CONDITIONS

STOP rather than broaden the implementation if:

- stable portrait output changes unexpectedly;
- straight rails are not preserved;
- equivalent entity anchors leave their rails;
- near-rim placement moves;
- X projection changes;
- a level-specific correction appears necessary;
- an open/closed branch appears necessary;
- gameplay/model geometry would need modification;
- ExitZoom/ReverseZoom simulation would need modification;
- gating/fading would be required;
- scale or -33.1 placement would need changing;
- range expansion would be required;
- final HUD work would be required;
- recording schema would need changing;
- visual assets or prohibited references would be required.

Report PARTIAL or BLOCKED and preserve a diagnosable worktree.


IMPLEMENTATION SCOPE

Expected production changes should be limited primarily to:

- src/Tempest.Game/Rendering/Projection.cs
- src/Tempest.Game/Rendering/VectorRenderer.cs

Modify other existing candidate files only if directly required to propagate the active near/far projection context or update directly affected tests.

Expected directly affected tests may include:

- tests/Tempest.Game.Tests/DisplayConfigurationTests.cs
- tests/Tempest.Game.Tests/TransitionPresentationTests.cs
- tests/Tempest.Game.Tests/GameplayRecordingTests.cs
- tests/Tempest.Game.Tests/DebugControllerTests.cs

Do not modify unrelated gameplay tests or durable documentation.

Do not create a second projection system.


BOUNDED VALIDATION

First run a focused Release build/test cycle covering:

- projection/presentation;
- stable equivalence;
- straight-rail incidence;
- tube/entity registration;
- Approach;
- ExitZoom;
- ReverseZoom;
- transition presentation;
- recording/playback reconstruction;
- parser/legacy behavior if directly affected.

Use an explicit VSTest hang timeout of no more than 3 minutes.

If focused validation passes, run:

1. full test suite with explicit 3-minute VSTest hang timeout;
2. Release build;
3. git diff --check.

Do not repeatedly rerun unchanged failures.

Do not use stale binaries after failed builds.

If Windows security, a locked executable, timeout, hang, or stalled command blocks validation, follow the established self-stop policy.

Do not report incomplete validation as passed.


MANUAL ACCEPTANCE HANDOFF

Do not launch the interactive game.

Because the range remains [-5,+5], manual testing in this milestone is not intended to judge the final strength of stable-play perspective.

The manual purpose is to verify composition with transitions.

Provide commands for:

1. Level 9 neutral portrait:
   --portrait-perspective 0

2. Level 9 maximum current positive:
   --portrait-perspective 5

3. Level 9 maximum current negative:
   --portrait-perspective -5

4. Level 9 legacy reference.

5. A representative Spike-exit level where ExitZoom/ReverseZoom can be observed.

6. One deterministic playback or playback-file example using coefficient +5.

Manual checks must focus on:

- no projection explosion during ExitZoom;
- no extreme upward launch of far geometry;
- no new clipping attributable to the coefficient;
- no boundary jump caused by gating;
- rails remain straight;
- Blaster/shots/Spikes remain registered;
- ReverseZoom remains continuous;
- successful exit and between-level Approach remain coherent;
- neutral stable presentation remains unchanged;
- legacy remains unchanged.

Do not ask Neal to judge 0 versus 5 as a meaningful final arcade perspective comparison.

The human-visible range-expansion milestone remains deferred until this transition-safe formula is accepted.


DOCUMENTATION AND AI LOGGING

Preserve the substantive prompt under AI-Logs/Prompts.

Create:

AI-Logs/Reports/2026-11-14-whole-game-presentation-endpoint-normalized-perspective-implementation-report.md

The report must include:

- starting Git state;
- reports/source inspected;
- files changed;
- old formula;
- new endpoint-normalized formula;
- near/far-context plumbing;
- stable equivalence proof/results;
- projective-line coherence proof/results;
- transition amplification results;
- transition continuity results;
- tube/entity registration results;
- Approach handling;
- outside-tube depth handling;
- playback/reverse-step determinism;
- parser/range/default preservation;
- legacy preservation;
- focused test results;
- full-suite result;
- Release build result;
- git diff --check result;
- exact manual commands;
- manual acceptance boundary;
- unresolved human-visible calibration question;
- final Git state;
- confirmation nothing was staged, committed, or pushed;
- confirmation no visual assets, Gemini material, Incitti source, web sources, or raw ROM files were inspected.

Do not rewrite historical reports.

Do not update TEMPEST-DEVELOPMENT-NOTES.md or TEMPEST-BEHAVIOR-SPEC.md yet.

Durable documentation waits for manual acceptance.


NO COMMIT OR PUSH

Do not stage.
Do not commit.
Do not push.

Leave the candidate inspectable and reversible.


FINAL RESPONSE

Report:

1. Whether endpoint-normalized implementation completed.
2. Starting/final Git state.
3. Files changed.
4. Old and new formulas.
5. Active near/far context propagation.
6. Stable-play exact-equivalence result.
7. Near-rim preservation.
8. X-coordinate preservation.
9. Projective-line coherence result.
10. Transition amplification result.
11. Transition continuity result.
12. Tube/entity registration result.
13. Approach result.
14. Outside-tube depth result.
15. Playback/reverse-step determinism result.
16. Confirmation range remains [-5,+5].
17. Confirmation default remains 0.
18. Legacy-preservation result.
19. Focused tests.
20. Full suite.
21. Release build.
22. git diff --check.
23. Any blocked or partial work.
24. Exact manual commands.
25. Manual transition checklist.
26. Report path.
27. Final Git state.
28. Confirmation nothing was staged, committed, or pushed.
29. Confirmation no visual assets or prohibited references were inspected.

Stop after automated validation and hand the transition-safe candidate to Neal.

TEMPEST_PRESENTATION_ENDPOINT_NORMALIZED_PERSPECTIVE_20261114