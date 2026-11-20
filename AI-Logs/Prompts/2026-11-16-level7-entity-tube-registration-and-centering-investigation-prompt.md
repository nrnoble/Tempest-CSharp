READ THIS ENTIRE USER-PROVIDED TASK BEFORE TAKING ANY ACTION.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, reading project files, running commands, inspecting images,
or taking any repository action:

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

Perform a bounded READ-ONLY investigation of the Level 7 Atari-presentation
integration failure revealed by Neal's manual testing.

The investigation has two related but distinct questions:

PRIMARY QUESTION

Why do Level 7 gameplay entities, the Blaster, shots, Eggs, Tankers, Spikes,
and effects appear to be rendered on a different invisible playable tube
instead of registering to the visible record-06 presentation tube?

SECONDARY QUESTION

Why is the visible record-06 tube not centered in the intended gameplay area
like the supplied correct-presentation reference?

Do not implement a correction.

Do not retune the established Level 7 tube shape.

MANUAL TEST RESULT

Neal manually ran:

dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation --level 7

The result failed full Level 7 presentation integration.

Neal reports with high confidence:

- gameplay itself works correctly;
- the game appears to be played on an invisible geometric tube;
- the visible blue record-06 tube behaves like a separate visual overlay;
- Flippers in an earlier screenshot appeared to move on that invisible
  playable tube;
- the latest screenshot more clearly shows a Tanker climbing that invisible
  tube;
- the starting Spikes remain associated with the invisible tube;
- Eggs appear to float at the deep end of the invisible tube;
- the Blaster and other gameplay vectors likewise do not register to the
  visible record-06 tube;
- the visible record-06 tube shape now looks substantially right;
- the visible tube is not properly centered in the intended gameplay area like
  the supplied correct-presentation reference.

Treat the "invisible playable tube" description as a MANUAL DIAGNOSTIC
HYPOTHESIS supported by visible testing, not as an implementation fact until
the rendering paths are traced.

The implementation report for the current candidate is:

AI-Logs/Reports/2026-11-16-level7-record06-presentation-implementation-report.md

That report states that the record-06 tube uses AtariLevel7Presentation while
gameplay geometry and simulation remain unchanged.

The investigation must determine whether the visible tube and gameplay
entities are in fact rendered through different geometric or coordinate
pipelines.

AUTHORIZED IMAGES

You MAY inspect only these explicitly authorized images if present:

1. The latest Level 7 manual failure screenshot showing a Tanker, Eggs, starting
   Spikes, Blaster, and the visible record-06 tube:

   live-level-7-tick-000280-001.png

2. The earlier Level 7 manual failure screenshot showing Flippers and other
   misregistered gameplay objects:

   live-level-7-tick-000373-001.png

3. The earlier clearer no-Flipper integration screenshot:

   live-level-7-tick-000015-001.png

4. The supplied correct-presentation reference used previously:

   Level7_frame_0090-MAME.png

The filename of the comparison image identifies its capture source only. Do not
investigate MAME, its implementation, settings, scaling, or rendering.

If exact file locations are not immediately known, locate only these exact
filenames within the authorized Level-7 screenshot area.

Do not enumerate, open, inspect, or analyze other screenshots, images, videos,
frames, contact sheets, or unknown visual assets.

Use the manual screenshots to identify the visible symptoms. Establish the
technical cause from the C# rendering and coordinate paths.

PRESERVE THE LEVEL 7 TUBE CANDIDATE

The record-06 tube shape candidate is retained.

Do not reopen or question:

- record-06 source bytes;
- record-06 metadata;
- record-06 point order;
- cyclic rim adjacency;
- same-index near/far rail pairing;
- near position p=$10;
- far position p=$F0;
- projector mathematics;
- the calibrated anisotropic presentation gain;
- the calibrated far-set translation;
- or the decision to keep record 06 as the Level 7 tube basis.

Do not repeat the previous record-06, connectivity, RN.high, factor-128,
reference-shape, or aspect/gain investigations.

The tube shape itself is not the target of this investigation.

PRIMARY INVESTIGATION: MODEL-TO-PRESENTATION REGISTRATION

Trace separately:

A. how the visible record-06 tube reaches the screen;

and

B. how each class of gameplay object reaches the screen.

For the visible tube, trace at minimum:

- record-06 source data;
- AtariCoordinateProjector;
- AtariTubeVectorListBuilder;
- AtariLevel7Presentation;
- near/far endpoint mapping;
- VectorRenderer dispatch;
- camera/viewport mapping;
- final screen coordinates.

For gameplay objects, trace as applicable:

- production Level 7 model geometry;
- lane identity;
- lane endpoints;
- lane interpolation;
- entity depth/height;
- entity model position;
- entity projection;
- entity rotation/orientation;
- final presentation adapter;
- camera/viewport mapping;
- final screen coordinates.

Investigate each relevant gameplay presentation class separately where its path
differs:

1. Blaster.
2. Tankers.
3. Ordinary Flippers.
4. Tanker-released Flippers.
5. Eggs.
6. Spikers.
7. Starting Spikes.
8. Player shots.
9. Enemy shots.
10. Destruction/effect vectors.
11. Any rim or deep-end markers used by gameplay presentation.

Do not infer that all entity types share one rendering path without tracing
the relevant dispatch and helper calls.

KEY QUESTIONS

Answer these questions from implementation evidence:

1. Does the record-06 tube use AtariLevel7Presentation while gameplay entities
   continue to use Projection.Project or another production model-geometry
   projection?
2. Are the visible tube and gameplay entities based on different Level 7 point
   sets?
3. Does the record-06 tube use Atari record points while entities derive their
   visible positions from LevelDefinition.Points or another production tube?
4. Do entities and the tube use different near/far endpoint sets?
5. Do entities and the tube use different aspect, scale, translation, camera,
   or vertical placement?
6. Are entities projected onto the existing production Level 7 clover and then
   drawn over the separately mapped record-06 tube?
7. Does lane index i in gameplay correspond to record-06 presentation lane i?
8. Is lane direction or point order reversed between model and presentation?
9. Is a lane-to-record-lane correspondence table needed, or are indices already
   equivalent?
10. Can game-model lane/depth positions be rendered onto record-06 presentation
    geometry without changing simulation or collision geometry?
11. Which coordinate representation supplies the necessary interpolation for
    below-rim entities?
12. How should rim entities be mapped?
13. How should deep-end Eggs and starting Spikes be mapped?
14. How should the Blaster and shots be registered?
15. Are object orientation and vector rotation currently calculated from the
    invisible production tube and therefore also visually wrong?
16. Are any objects being rendered outside the gameplay region because their
    positions use the old presentation transform?

OBJECTIVE REGISTRATION PROBES

Use bounded calculations where useful.

Select representative Level 7 gameplay states or model coordinates without
launching the interactive game.

For representative lane/depth positions, calculate both:

- the current gameplay-entity screen position;
- the corresponding record-06 presentation screen position.

Include enough probes to reveal the mismatch clearly, such as:

- one rim position;
- one upper-tube position;
- one mid-depth position;
- one deep-end position;
- one starting-Spike position;
- and one Egg/deep-end position.

For each probe, identify:

- gameplay lane/index;
- gameplay depth/height;
- source model points used;
- current screen coordinate;
- intended corresponding record-06 lane/depth endpoint interpolation;
- record-06 presentation coordinate;
- and the discrepancy.

Do not create screenshot-derived entity coordinates.

Do not fit individual entities to image pixels.

The correct investigation target is a general model-to-presentation mapping.

MODEL/PRESENTATION SEPARATION

Treat the working gameplay geometry as protected.

Neal reports that gameplay plays correctly.

Do not propose changing:

- Level 7 gameplay points;
- lane topology;
- collision geometry;
- model depth;
- movement;
- spawning;
- Tanker cargo handling;
- Flipper movement;
- Spiker behavior;
- Spike behavior;
- Eggs;
- timing;
- score;
- lives;
- RNG;
- recording/playback state.

The expected correction direction, if the hypothesis is confirmed, is a
render-only Level 7 registration adapter that maps gameplay lane/depth state
onto the visible record-06 presentation geometry.

Do not assume that is the answer in advance. Establish it from code.

If such an adapter is justified, determine whether it can reuse:

- AtariLevel7Presentation mapped near/far points;
- lane index and depth interpolation;
- existing entity vector shapes;
- existing colors/animation;
- and existing gameplay state without mutation.

ENTITY ORIENTATION AND SHAPE

Position registration alone may be insufficient.

Determine whether the existing entity renderer derives orientation, local
basis vectors, rotation angle, width, or apparent scale from the production
tube's lane geometry.

If so, identify what must instead be derived from the corresponding visible
record-06 presentation lane.

For each entity category, distinguish:

- logical gameplay position;
- rendered anchor position;
- rendered orientation;
- rendered local scale;
- rendered vector shape.

Do not redesign entity artwork.

SECONDARY INVESTIGATION: TUBE CENTERING

Investigate the visible record-06 tube's placement independently from entity
registration.

Neal reports that the tube shape now looks substantially right but is not
properly centered in the intended gameplay area like the correct-presentation
reference.

Do not alter the tube shape to fix placement.

Determine:

1. The current visible tube bounds and geometric/visual center.
2. The current gameplay area's intended center under the accepted Levels 1-4
   composition rule:
   - persistent HUD region remains above;
   - tube appears centered in the remaining gameplay composition area;
   - unusually tall shapes may require vertical accommodation without
     redefining geometry.
3. The current AtariLevel7Presentation near and far anchors.
4. Any camera offset or portrait viewport transformation affecting placement.
5. The translation required to center the complete Level 7 tube presentation
   appropriately beneath the HUD.
6. Whether one shared translation can center the tube without changing shape,
   near/far relationship, or entity registration.
7. Whether the current calibration inherited a screenshot-fitting coordinate
   origin that does not match the project's accepted gameplay-area center.

Use the authorized correct-presentation reference only as bounded compositional
evidence.

Do not reopen horizontal/vertical gain or far-set relative translation while
solving centering.

A centering change must be a shared translation applied to the complete
record-06 presentation after the accepted shape transform.

Do not choose final placement values in this read-only task unless the
calculation is exact and unambiguous. Report the justified candidate
translation for a future implementation prompt.

CURRENT IMPLEMENTATION AND REPORTS

Inspect the minimum relevant current code and reports, including as needed:

- AtariLevel7Presentation.cs
- AtariTubeReferenceShapeDiagnostic.cs
- AtariCoordinateProjector.cs
- AtariTubeVectorListBuilder.cs
- AtariDvgViewportMap.cs
- AtariPresentationAdapter.cs
- Projection.cs
- VectorRenderer.cs
- LevelLoader.cs
- Level 7 model/definition data
- entity render helpers
- Blaster render paths
- shot/effect render paths
- Spike/Egg rendering paths
- display/camera configuration
- relevant focused tests
- the Level 7 presentation implementation report
- the reference-shape transform report
- the record-06 segment reconstruction report

Do not broadly re-audit unrelated renderer code.

READ-ONLY REQUIREMENT

Do not modify:

- source code;
- tests;
- project files;
- configuration;
- durable documentation;
- geometry;
- projection;
- viewport mapping;
- entity rendering;
- gameplay;
- recording/playback;
- accepted presentation values;
- or Git state.

Do not implement a candidate correction.

Do not launch the interactive game.

Do not stage, commit, push, reset, restore, stash, rebase, clean, or switch
branches.

Temporary bounded calculations outside tracked project content are permitted.

Do not create scripts or analysis artifacts in tracked project paths.

AI-LOGS

Follow the current .github/copilot-instructions.md.

The normal ignored AI-Logs files are required and are the permitted
file-writing exception.

Save:

- this substantive prompt under AI-Logs/Prompts/;
- the completed investigation report under AI-Logs/Reports/.

Use descriptive date-prefixed paired filenames.

A chat response is not a substitute for the report file.

Do not stage or commit AI-Logs.

EVIDENCE CLASSIFICATION

Clearly distinguish:

- MANUAL OBSERVATION;
- MANUAL DIAGNOSTIC HYPOTHESIS;
- implementation-established fact;
- ROM/Atari-established behavior;
- PROJECT TRANSLATION;
- DEVELOPMENT-ONLY behavior;
- inference;
- unresolved behavior.

Do not promote Neal's invisible-tube hypothesis to fact unless the code trace
confirms it.

Do not dismiss the hypothesis merely because gameplay itself is correct.

REPOSITORY SAFETY

At the beginning, record:

- branch;
- HEAD;
- upstream/tracking state;
- index state;
- tracked worktree changes;
- relevant untracked state.

Preserve all pre-existing candidate work and protected assets.

At completion, verify that no project or Git state changed except the required
ignored paired AI-Logs records.

EXPECTED REPORT

Save the report under AI-Logs/Reports/ and summarize the findings in chat.

Near the beginning, directly answer:

1. Is Level 7 gameplay currently being rendered on a different geometric tube
   from the visible record-06 tube?
2. What exact rendering paths create that result?
3. Can gameplay remain unchanged while its presentation is registered onto the
   visible record-06 tube?
4. Why is the visible record-06 tube not properly centered?

The report must include:

1. Authorized images inspected.
2. Manual observations and hypotheses.
3. Current visible record-06 tube render path.
4. Current Blaster render path.
5. Current Tanker render path.
6. Current Flipper render path.
7. Current Egg render path.
8. Current Spiker and Spike render paths.
9. Current player-shot and enemy-shot render paths.
10. Current effect render paths.
11. The gameplay/model Level 7 geometry source.
12. The visible record-06 geometry source.
13. Whether those sources differ.
14. Current coordinate-space chain for each relevant class.
15. Representative numerical lane/depth registration probes.
16. Whether lane indices correspond directly.
17. Whether point order/direction differs.
18. Whether a general render-only lane/depth adapter is technically justified.
19. Required handling for position, orientation, scale, and local entity basis.
20. Whether the adapter can preserve all simulation/gameplay state.
21. Current tube bounds and center.
22. Intended gameplay-area compositional center.
23. Exact cause of the centering discrepancy.
24. The smallest technically justified future centering correction.
25. The smallest technically justified future entity-registration correction.
26. Any entity class requiring separate treatment.
27. Risks to recording/playback or deterministic simulation, if any.
28. Whether enough evidence exists for a bounded implementation prompt.
29. Confirmation that record 06 and the accepted tube-shape mapping remain
    frozen.
30. Confirmation that no source, tests, presentation, gameplay, protected
    material, or Git state changed.
31. Exact paths of the saved prompt and report files.

Do not ask Neal questions during execution.

Do not pause for manual input or acceptance.

Complete the bounded read-only investigation autonomously and stop after
reporting the evidence.

TEMPEST_LEVEL7_ENTITY_TUBE_REGISTRATION_AND_CENTERING_INVESTIGATION