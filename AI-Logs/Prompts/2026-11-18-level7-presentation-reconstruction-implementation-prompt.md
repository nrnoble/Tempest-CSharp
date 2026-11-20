READ THIS ENTIRE USER-PROVIDED TASK BEFORE TAKING ANY ACTION.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, reading files, running commands, or taking repository
action:

1. Locate the unique completion token near the end of this task.
2. Verify that exactly one complete token beginning with "TEMPEST_" appears on
   a line by itself.
3. Automatically appended VS Code/agent context after the token does not
   invalidate it.
4. If exactly one complete token cannot be found, STOP before repository action
   and explain why.

TASK

Implement the bounded clean reconstruction of Level 7 ordinary
--atari-presentation described in the controlling read-only plan.

Replace the current special two-geometry Level 7 presentation with one shared
geometry and one shared projector:

- preserve the existing production Level 7 gameplay geometry;
- use that same geometry for the visible tube;
- project the tube and all gameplay entities through the existing shared
  AtariPresentationAdapter path;
- use the adapter Jacobian as the shared local-scale authority;
- calculate one objective final Level 7 placement from the resulting complete
  tube envelope;
- remove the active special record-06 tube/entity presentation architecture
  after replacement validation;
- preserve raw record-06 data and metadata as ROM-backed evidence;
- preserve all Level 7 gameplay and simulation.

Do not commit or push.

Stop after automated validation and provide Neal with manual test commands.

Manual acceptance remains PENDING.

CONTROLLING PLAN

Use this report as the controlling handoff:

AI-Logs/Reports/2026-11-18-level7-presentation-reconstruction-plan-report.md

Follow its recommended Option A architecture:

LevelConfiguration.Level7Production
and
LevelLoader.LoadLevel7Clover()

remain the single authoritative Level 7 geometry for both gameplay and
presentation.

Do not replace the production Level 7 points with record-06-derived points.

Do not retain a second visual tube or a cross-geometry entity mapper.

CURRENT BASELINE

Begin from the committed baseline after ExitZoom focal-compensation closeout:

aee59b39c1d68be830d0014a7b9497d1bb40f3c7

Verify:

- branch;
- HEAD;
- upstream;
- ahead/behind;
- index;
- tracked worktree;
- relevant untracked files.

Expected branch/upstream:

- master;
- origin/master;
- 0 ahead / 0 behind.

Preserve unrelated local work, including if present:

- .github/copilot-instructions.md;
- Reference/arcade/MBUCOD.V05.txt;
- Reference/arcade/MBUDOC.DOC.

Do not reset, restore, clean, stash, rebase, or switch branches.

PROBLEM BEING REPLACED

The current active Level 7 presentation uses two different geometries:

GAMEPLAY

- production Level 7 clover from LevelLoader.LoadLevel7Clover();
- authoritative for lane state, movement, Flipper pivots, collision and all
  simulation.

VISIBLE TUBE

- Atari record 06;
- projected through AtariTubeVectorListBuilder,
  AtariCoordinateProjector and AtariLevel7Presentation.

VISIBLE ENTITIES

- gameplay lane/depth state remapped onto record-06 near/far endpoints through
  AtariLevel7EntityPresentation;
- linear near/far interpolation;
- separate lane-width local-scale formula;
- special mapped-Flipper transition presentation.

That bridge aligned entities with the record-06 tube but introduced the severe
manual depth-motion presentation regression.

Remove the bridge rather than repairing it again.

REPLACEMENT ARCHITECTURE

For Level 7 ordinary Atari-presentation Playing:

1. Use CurrentLevel.Definition.Points, which must remain the exact production
   Level 7 clover.

2. Draw the visible Level 7 tube from those production points.

3. Project every near and far tube endpoint through the same ordinary
   ProjectWorldPoint / AtariPresentationAdapter.Project path used by gameplay
   entities.

4. Project every gameplay entity from its existing authoritative lane/depth
   state and those same production points.

5. Use the same Level 7 adapter mapping, metadata, depth convention and final
   camera for tube and entities.

6. Use the active adapter Jacobian/local scale for entity artwork.

7. Apply one final Level-7-specific presentation translation equally to tube
   points and entity anchors.

8. Apply the common portrait camera exactly once.

There must be:

- one geometry;
- one point order;
- one lane authority;
- one depth convention;
- one projector;
- one local-scale authority;
- one placement translation;
- one final camera.

No active ordinary Level 7 path may use record-06 endpoint interpolation after
this reconstruction.

PRODUCTION LEVEL 7 GEOMETRY IS FROZEN

Preserve exactly:

- all 16 production Level 7 points;
- point order;
- closed topology;
- 16 playable lanes;
- CenterY = 310;
- YOffset = -10;
- Level Position;
- Level Depth;
- initial player lane;
- gameplay configuration identity.

Do not modify gameplay geometry to improve presentation.

Do not rotate or cyclically reindex points.

Do not apply the historical record-06 comparison offset.

Do not add per-point visual corrections.

Do not add screenshot-derived coordinates.

LEVEL 7 GAMEPLAY PROTECTION

Do not change:

- lane topology;
- enemy supply;
- Tanker behavior;
- ordinary Flipper behavior;
- Tanker-cargo Flipper behavior;
- directed Flipper transition selection;
- long/reflex routes;
- Spikers;
- Spikes;
- Eggs;
- enemy shots;
- player shots;
- collision;
- wave completion;
- Super Zapper behavior;
- scoring;
- lives;
- timing;
- RNG;
- recording/playback;
- fixed-step simulation.

Rendering must remain state-read-only.

RAW RECORD-06 DATA MUST REMAIN

Preserve verbatim:

AtariTubeRecordCatalog.Level7Record06

including its raw X/Y bytes, metadata, point order and closed topology.

Preserve the Level 7 entry in RomPresentationMetadataCatalog.

Record 06 remains ROM/Atari-backed evidence.

It no longer drives the ordinary visible Level 7 tube after reconstruction.

Do not claim that production Level 7 coordinates are an exact record-06
conversion.

Do not delete record-06 raw-data/provenance tests.

Re-home or rewrite those tests if their current test class is removed.

SHARED LEVEL 7 ADAPTER

Reuse the existing Level 7 model-frame mapping in AtariPresentationAdapter as
the initial shared presentation path.

Do not invent another adapter.

Preserve the current Level 7 adapter mapping inputs unless implementation
evidence shows the current source differs materially from the controlling plan.

Do not retune:

- model-frame scale;
- orientation/reflection;
- raw ROM metadata;
- shared perspective;
- Projection.K;
- Projection.ZOffset;
- camera zoom;
- viewport.

The implementation report must identify the exact Level 7 adapter values
actually used.

TUBE RENDERING

Replace the special record-06 Level 7 tube dispatch with the normal shared tube
rendering path.

At near and far depth:

- take each production Level 7 point;
- project it through the same world/adaptor path used by entities;
- connect corresponding near/far endpoints;
- draw cyclic near/far rims using the production closed topology;
- use production lane indices for highlighting and pulsing.

The visible tube must not be constructed from:

- AtariLevel7Presentation mapped arrays;
- AtariTubeVectorListBuilder output;
- AtariCoordinateProjector output;
- screenshot-fitted gains;
- far-set offsets;
- record-06 lane identity assumptions.

ENTITY ROUTING

Route these Level 7 classes through the shared production-geometry projector:

- Blaster;
- Tankers;
- ordinary Flippers;
- Tanker-released Flippers;
- Spikers;
- Eggs;
- starting and generated Spikes;
- player shots;
- enemy shots;
- destruction/effect anchors;
- lane highlights and pulse presentation.

Each entity must retain its current gameplay state and artwork.

Do not create Level-7-only visual offsets.

BLASTER

Preserve:

- existing player edge/lane;
- EdgePosition;
- spinner response;
- outward orientation correction;
- accepted ordinary Playing size;
- firing;
- collision;
- artwork topology;
- current yellow color.

Derive the visible rim edge from the same projected production tube endpoints.

Continue using the accepted geometry-derived outward side.

Do not change Blaster ExitZoom/Approach scale behavior.

TANKERS, SPIKERS, EGGS AND SHOTS

Use the same production lane midpoint/depth semantics already used by the
normal renderer.

Project anchors through the shared Level 7 adapter.

Use adapter local scale where existing artwork requires projected scale.

Do not clamp or reinterpret gameplay heights.

Preserve initial Egg below-far semantics through the existing generic
projection behavior.

Do not retain the special Level 7 record-lane interpolation.

SPIKES

Project Spike base and tip from their existing gameplay depth values on the
same production lane.

Do not use mapped record-06 rail endpoints.

Preserve growth, collision and destruction state.

FLIPPER TRANSITIONS

Remove the active Level 7 mapped-Flipper presentation path.

Do not use:

AtariLevel7EntityPresentation.MapFlipperTransition

Instead:

- use the existing DirectedLaneTransition produced from production gameplay
  geometry;
- use the normal CalculateFlipperPivotEndpoints path;
- project both endpoints through the shared Level 7 adapter;
- preserve the gameplay-selected directed route;
- preserve long/reflex paths, including 270-degree routes;
- preserve transition progress and timing;
- use the same path for ordinary and Tanker-cargo Flippers.

The prior mapped-Flipper crash correction remains historical evidence but
should no longer be active once the special mapper is removed.

PLACEMENT

Do not preserve the current record-06 screenshot-calibrated Level 7 placement.

After the shared geometry/projector is active:

1. Project the complete Level 7 tube envelope at near and far depths.
2. Calculate logical and host-space bounds.
3. Preserve the existing horizontal gameplay center.
4. Determine the HUD-cleared portrait gameplay region using the same accepted
   composition rule used for Levels 8 and 9.
5. Calculate the exact one-time logical translation required to center the
   complete tube vertically in that gameplay region.
6. Apply the same translation to tube geometry and all projected entity/world
   anchors.

Do not copy:

- Level 8 +212.28139;
- Level 9 +180.96825;
- Level 10 +50;
- current Level 7 record-06 centering;
- current screenshot-derived constants.

Do not alter geometry, projection, perspective or scale to correct placement.

The report must include the full calculation and final candidate translation.

HORIZONTAL CENTER SAFETY

The future candidate must retain the correct horizontal center.

Do not allow a vertical placement correction to move the tube left or right.

If the objective complete-envelope calculation proposes a nonzero X
translation, verify why.

Prefer zero X correction when the projected envelope is already centered.

Stop and report if horizontal placement cannot be derived unambiguously.

REMOVE ACTIVE SPECIAL LEVEL 7 PATH

After replacement tests pass, remove the active special path and its obsolete
code.

Expected removals include, subject to actual consumer verification:

- AtariLevel7Presentation.cs;
- AtariLevel7EntityPresentation.cs;
- special Level 7 tube renderer dispatch;
- GetLevel7LaneGeometry or equivalent special helpers;
- special Level 7 local-scale helpers;
- mapped record-06 Flipper transition presentation;
- screenshot-derived Level 7 presentation constants;
- candidate record-06 centering constants;
- candidate-only Level 7 renderer predicates.

Do not leave dead active-path code hidden behind an unused selector.

Do not remove a file until all current consumers are identified and migrated.

PROTOTYPE HELPER CLEANUP

After the ordinary Level 7 replacement path and tests pass, inspect remaining
consumers of:

- AtariTubeVectorListBuilder;
- AtariCoordinateProjector;
- AtariDvgViewportMap;
- AtariTubeReferenceShapeDiagnostic.

If a helper exists solely for the obsolete direct record-06 prototype:

- remove it;
- remove its obsolete runtime wiring;
- remove or rewrite prototype-specific tests;
- preserve raw record-06 data/provenance assertions elsewhere.

Do not remove a helper if another accepted level or feature still uses it.

Document every retained consumer.

CLI AND DIAGNOSTIC POLICY

The old prototype controls may include:

- --atari-tube-prototype;
- --atari-tube-rn000f-diagnostic;
- reference-shape diagnostic selection.

Remove those controls if their only runtime implementation is the obsolete
Level 7 prototype.

Update:

- DebugController parsing;
- Program wiring;
- help/usage text;
- parser tests;
- compatibility/context tests.

Do not leave silently accepted no-op switches.

KEEP these diagnostics through manual acceptance:

- --level7-visible-runtime-diagnostic;
- --level7-framebuffer-diagnostic.

Do not remove their implementation or tests in this milestone.

They remain available for same-recording comparison if manual testing finds a
problem.

No diagnostic should affect ordinary presentation unless explicitly selected.

TEST MIGRATION

Add a focused test class:

Level7AtariPresentationTests

or use an equivalently precise existing name.

Test at minimum:

A. DATA AND GEOMETRY

1. Level 7 production definition remains the exact 16-point ordered clover.
2. Topology remains closed with 16 playable lanes.
3. CenterY, YOffset, Position and Depth remain unchanged.
4. Raw record-06 X/Y bytes remain unchanged.
5. Record-06 metadata and closed topology remain unchanged.
6. Ordinary visible Level 7 tube is built from production points, not record
   endpoints.
7. No screenshot-derived point constants or per-point corrections exist.

B. SHARED PROJECTION

8. Every near tube endpoint equals shared adapter projection of the
   corresponding production point.
9. Every far tube endpoint equals the same adapter at far depth.
10. Tube rails connect matching production indices.
11. Cyclic rims preserve production lane order.
12. All entity anchors use the same production points and adapter.
13. One Level 7 placement translation applies to tube and entities.
14. Common portrait camera is applied once.
15. Adapter Jacobian is the local-scale authority.
16. No active path calls AtariLevel7EntityPresentation.
17. No active ordinary tube path calls AtariLevel7Presentation.
18. No second record-06 visual tube remains.

C. ENTITY REGISTRATION

19. Blaster lies on the visible rim and retains outward orientation.
20. Tanker anchors match shared-projected lane centers.
21. Ordinary Flipper endpoints match shared projection.
22. Tanker-cargo Flipper uses the same path.
23. Spiker anchors match shared projection.
24. Egg anchors match shared projection at existing depths.
25. Spike base and tip match shared projection.
26. Player shot anchors match shared projection.
27. Enemy shot anchors match shared projection.
28. Effects remain registered.
29. Lane highlight uses the same projected tube endpoints.

D. FLIPPER TRANSITIONS

30. All 16 source lanes and both legal directions are covered.
31. Transition endpoints match production directed geometry.
32. Interior progress remains continuous.
33. Long/reflex routes remain long.
34. Representative 270-degree routes are preserved.
35. Ordinary and cargo Flippers share the path.
36. Rendering does not mutate transition/gameplay state.
37. The prior special mapped-transition exception path is absent.

E. PLACEMENT

38. Complete near/far envelope is calculated from shared projected geometry.
39. Horizontal center is retained.
40. Vertical center matches the HUD-cleared gameplay center.
41. One shared translation is used.
42. Tube and entity anchors receive the same translation.
43. Placement does not alter shape, scale, perspective or near/far relation.

F. GAMEPLAY AND DETERMINISM

44. Gameplay/model state is unchanged before/after rendering.
45. Presentation off/on produces identical deterministic simulation for the
    same Level 7 seed/input.
46. Existing recording format and playback state remain unchanged.
47. Existing production/admission/Flipper/Spiker tests remain unchanged and
    passing.
48. Collision and completion state remain unchanged.
49. RNG-derived outcomes remain unchanged.

G. CROSS-LEVEL PROTECTION

50. Levels 1-6 accepted presentation remains unchanged.
51. Level 8 +212.28139 remains unchanged.
52. Level 9 +180.96825 remains unchanged.
53. Level 10 +50 remains unchanged.
54. Accepted Blaster orientation remains unchanged.
55. Accepted Blaster ExitZoom/Approach scale remains unchanged.
56. Accepted ExitZoom focal compensation remains unchanged.
57. Option-off/legacy behavior remains unchanged.
58. Approach, exit, retry and transition behavior remain unchanged.

Do not use screenshots as automated test oracles.

Do not impose constant screen-pixel speed as a test requirement.

DEPTH-MOTION REGRESSION PROTECTION

Use the genuine existing Level 7 deterministic recording when available:

TEMP\GameplayRecordings\Level-7-slowmo.json

Do not inspect TEMP/myNotes.md.

Do not modify the recording.

Use the same recording/state sequence for presentation-off and
presentation-on comparisons.

Automated evidence must prove:

- playback boundaries and simulation states remain identical;
- all active Level 7 entity anchors use shared adapter geometry;
- no special linear record-lane interpolation remains;
- no special lane-width local-scale formula remains;
- rim and depth-moving entities use one coherent projector.

Automated tests cannot establish perceived visual acceptance.

Neal's manual playback comparison remains required.

EXITZOOM AND APPROACH

Do not modify the accepted ExitZoom focal-compensation implementation.

Level 7 remains a closed tube and should enter the generic closed far-rim-center
rule using the reconstructed production definition.

Add regression coverage proving:

- the generic mapped far-rim center can be calculated;
- tube and Blaster receive the same compensation;
- no Level-7-only focal path is introduced;
- Approach does not retain stale ExitZoom compensation;
- accepted Blaster travel scale remains unchanged.

Do not manually accept Level 7 ExitZoom through automated tests.

CODE QUALITY

Prefer removing obsolete complexity rather than wrapping it in more flags.

Do not leave comments describing the failed two-geometry architecture as
current design.

Add concise comments only where needed to preserve:

- one shared Level 7 geometry/projector contract;
- record 06 as retained evidence, not ordinary runtime geometry;
- placement as a project translation;
- rendering as state-read-only.

Do not narrate obvious code.

NO IMAGE ANALYSIS

No image analysis is required or authorized.

Do not inspect screenshots, images, video, frames, contact sheets, or visual
assets.

Do not repeat screenshot fitting.

VALIDATION

Run bounded focused tests:

dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~Level7AtariPresentationTests" --blame-hang-timeout 4m --verbosity minimal

Run targeted Level 7 gameplay/presentation regression tests:

dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~Level7AtariPresentationTests|FullyQualifiedName~Level7ProductionTests|FullyQualifiedName~Level7AdmissionPolicyTests|FullyQualifiedName~Level7FlipperMovementTests|FullyQualifiedName~Level7SpikerFiringTests|FullyQualifiedName~GameplayRecordingTests|FullyQualifiedName~GameplayPlayback" --blame-hang-timeout 5m --verbosity minimal

Run targeted cross-level/presentation validation:

dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~AtariPresentationAdapterTests|FullyQualifiedName~VectorRendererAtariPresentationTests|FullyQualifiedName~Level8AtariPresentationTests|FullyQualifiedName~Level9AtariPresentationTests|FullyQualifiedName~DiagnosticLevel10Tests|FullyQualifiedName~BlasterPresentationTests|FullyQualifiedName~BlasterScaleRegressionTests|FullyQualifiedName~ExitZoomFocalCompensationTests|FullyQualifiedName~TransitionPresentationTests|FullyQualifiedName~DirectedRotationInvestigationTests" --blame-hang-timeout 5m --verbosity minimal

Then run:

- full solution tests with explicit VSTest hang-timeout protection;
- Release solution build;
- git diff --check.

Do not weaken tests.

Canceled, blocked, stalled, timed-out or incomplete commands are not PASS.

Do not launch the interactive game.

STOP CONDITIONS

STOP and report rather than broadening the task if reconstruction requires:

- changing production Level 7 point coordinates;
- changing gameplay lane order/topology;
- changing movement/collision/timing/RNG;
- changing recording format;
- creating another entity-only depth mapper;
- retuning accepted Blaster or ExitZoom systems;
- using screenshots to fit placement or geometry;
- changing Levels 8/9/10;
- inventing a record-06-to-production lane map;
- preserving two active tube geometries.

If placement cannot be computed objectively from the shared projected envelope,
stop and report the exact ambiguity.

REPOSITORY SAFETY

Do not stage, commit, or push.

Do not reset, restore, clean, stash, rebase, or switch branches.

Preserve unrelated local work.

Record starting and ending:

- branch;
- HEAD;
- upstream;
- ahead/behind;
- index;
- tracked worktree;
- relevant untracked state.

AI-LOGS

Follow .github/copilot-instructions.md.

Save:

- this substantive implementation prompt under AI-Logs/Prompts/;
- the paired Level 7 reconstruction implementation report under
  AI-Logs/Reports/.

Do not stage or commit AI-Logs.

MANUAL HANDOFF

After automated validation, STOP.

Do not launch the game.

Provide these exact commands:

Direct Level 7 test:

dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation --level 7

Genuine recording without Atari presentation:

dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --playback-file TEMP\GameplayRecordings\Level-7-slowmo.json

Same recording with Atari presentation:

dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --playback-file TEMP\GameplayRecordings\Level-7-slowmo.json --atari-presentation

Ask Neal to judge:

- gameplay timing/progression remains identical;
- tube shape is acceptable;
- horizontal placement remains centered;
- vertical placement beneath the HUD is correct;
- Blaster stays on the visible rim;
- Tankers, Flippers, Spikers, Eggs and Spikes stay on visible lanes;
- enemies move naturally from deep to near;
- player shots move naturally from near to deep;
- rim motion remains normal;
- Flipper transitions preserve directed long routes;
- exit and Approach remain coherent;
- no regression to accepted whole-game presentation behavior.

Manual acceptance remains PENDING.

EXPECTED REPORT

The paired report must include:

- exact replacement architecture;
- exact files/symbols removed;
- exact files/symbols retained;
- record-06 data-retention evidence;
- production geometry preservation evidence;
- active Level 7 adapter values;
- final placement calculation and candidate translation;
- horizontal-center proof;
- entity-registration evidence;
- Flipper-transition replacement;
- prototype/CLI cleanup performed;
- diagnostics retained;
- deterministic playback/state equivalence;
- focused test results;
- targeted Level 7 results;
- targeted cross-level results;
- full-suite result;
- Release build result;
- git diff --check result;
- starting and ending Git state;
- exact manual commands;
- known limitations;
- manual acceptance status PENDING;
- exact AI-Logs prompt/report paths.

Do not ask Neal questions during execution.

Do not pause for manual input.

Complete autonomously and stop with the clean Level 7 shared-presentation
candidate ready for Neal's manual acceptance.

TEMPEST_LEVEL7_SHARED_PRESENTATION_RECONSTRUCTION