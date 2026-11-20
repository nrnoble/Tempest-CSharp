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

Implement a tightly bounded Level-7-only render-registration and centering
candidate for the current --atari-presentation Level 7 path.

The implementation must correct two confirmed presentation defects:

1. Gameplay objects are currently rendered from the production C# Level 7
   clover while the visible tube is rendered from Atari record 06. Register
   Level 7 gameplay presentation onto the visible mapped record-06 tube without
   changing gameplay or simulation geometry.

2. The visible record-06 tube retains a prototype coordinate origin under the
   production playfield camera and is not centered in the available gameplay
   composition area below the HUD. Apply the investigation's justified shared
   placement translation without changing the accepted tube shape.

This is a presentation-only implementation candidate.

Final manual acceptance remains Neal's decision.

CONTROLLING INVESTIGATION

Use this report as the controlling handoff:

AI-Logs/Reports/2026-11-16-level7-entity-tube-registration-and-centering-investigation-report.md

That investigation established:

- The visible Level 7 tube uses Atari record 06 through
  AtariCoordinateProjector, AtariTubeVectorListBuilder, and
  AtariLevel7Presentation.
- Level 7 gameplay objects still use the production C# clover from
  LevelDefinition.Points through AtariPresentationAdapter.Project.
- The visible tube and gameplay entities share the final playfield camera, but
  differ substantially before the camera.
- Gameplay therefore appears to occur on a separate invisible playable tube.
- The mismatch varies by lane and depth, so it cannot be corrected with one
  entity-only offset.
- A render-only lane/depth adapter can map existing game state onto the mapped
  record-06 near/far geometry without changing simulation or collisions.
- The complete visible tube requires an approximately (+97.47,+78.19) shared
  logical translation after the accepted shape transform to center its bounds
  in the gameplay composition area below the persistent HUD.
- Record 06 and the accepted Level 7 tube-shape mapping must remain frozen.

MANUAL EVIDENCE

Neal manually tested:

dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation --level 7

Neal reports:

- Gameplay behaves correctly.
- The visible record-06 tube shape looks substantially right.
- Tankers, Flippers, Eggs, starting Spikes, the Blaster, shots, and effects
  appear registered to a different invisible playable tube.
- The visible record-06 tube behaves like an overlay rather than the surface
  on which gameplay is visibly occurring.
- The visible tube is not centered beneath the HUD like the supplied correct
  presentation reference.

The controlling investigation confirmed the split rendering paths in code.

Preserve gameplay behavior and correct only presentation registration.

IMAGE SCOPE

No image analysis is required for this implementation task.

Do not search for, enumerate, open, inspect, or analyze screenshots, images,
videos, frames, contact sheets, or unknown visual assets.

The relevant symptoms and calculations are already recorded in the controlling
report.

FROZEN LEVEL 7 TUBE

Do not change or reopen:

- record-06 source bytes;
- record-06 metadata;
- record selection;
- point count;
- point order;
- closed-rim topology;
- cyclic rim adjacency;
- same-index near/far rail pairing;
- near position p=$10;
- far position p=$F0;
- AtariCoordinateProjector mathematics;
- the accepted anisotropic Level 7 presentation gain;
- the accepted relative far-set translation;
- or the decision to use record 06 as the Level 7 presentation basis.

Do not repeat record, connectivity, Math Box, factor-128, shape-transform, or
aspect/gain investigations.

Do not create replacement tube geometry.

Do not add per-point tube corrections.

PRESENTATION-ONLY ARCHITECTURE

Add or complete a small Level-7-only render adapter that presents gameplay
state on the existing mapped record-06 tube.

The adapter must:

- read existing simulation/gameplay state;
- read the existing Level 7 lane index, edge position, depth/height,
  transition state, and animation state as applicable;
- reuse the existing AtariLevel7Presentation mapped near and far endpoint
  geometry;
- calculate visible anchors and local bases from the mapped record geometry;
- return presentation values only;
- never mutate gameplay state.

Do not create another general projection architecture.

Prefer one shared Level 7 presentation-registration authority that can provide:

- mapped record near endpoints;
- mapped record far endpoints;
- lane edge endpoints at a normalized depth;
- lane center/anchor at a normalized depth;
- tangent and normal/local basis at a normalized depth;
- visible local scale when derivable from mapped geometry;
- edge-position interpolation for the Blaster and rim entities.

Avoid duplicating the accepted record mapping in individual entity renderers.

CENTERING IMPLEMENTATION

Apply one shared logical translation after the accepted Level 7 shape transform
and before the final playfield camera mapping:

- logical X translation: approximately +97.47
- logical Y translation: approximately +78.19

Use the exact values justified by the controlling report's calculation from the
current camera and gameplay-area bounds. Preserve reasonable precision rather
than introducing arbitrary rounded tuning.

The translation must move together:

- the entire visible record-06 tube;
- all mapped near and far endpoints;
- all registered gameplay entities;
- the Blaster;
- shots;
- effects;
- lane highlights;
- and related Level 7 presentation markers.

Do not change:

- anisotropic gain;
- relative far-set offset;
- perspective/depth mapping;
- tube dimensions;
- or tube topology.

Treat the centering values as a PROJECT TRANSLATION candidate pending Neal's
manual acceptance.

Do not independently recenter entities and tube.

LANE IDENTITY

Use identity lane correspondence as the initial candidate:

gameplay lane i -> record-06 presentation lane i

This is already the implicit assumption used by the existing tube
highlight/pulsing behavior.

Make the assumption explicit and testable.

Do not invent a lane-remapping table without evidence.

Do not change gameplay lane indices or model topology.

If implementation evidence proves identity mapping internally inconsistent,
STOP and report the exact conflict rather than selecting a speculative
permutation.

DEPTH MAPPING

Use the existing gameplay projected depth represented by the production model:

z = Position + Depth - rendered height/depth contribution

Normalize ordinary in-tube depth so that:

- t=0 corresponds to the near/rim endpoint geometry;
- t=1 corresponds to the far/web endpoint geometry.

Use the exact normalizing values established by the controlling report and
current Level 7 state rather than introducing unrelated constants.

For a lane bounded by record points i and NextEdge(i):

1. Interpolate mapped Near[i] to mapped Far[i] using t.
2. Interpolate mapped Near[next] to mapped Far[next] using t.
3. Use those two interpolated edge endpoints as the visible lane geometry at
   that depth.
4. Derive lane center, tangent, normal, and appropriate local scale from those
   visible endpoints as needed.

Do not clamp all normal gameplay depths unless current rendering semantics
require it.

Preserve simulation depth exactly as render input.

EGG BELOW-FAR POLICY

Initial Eggs can have t greater than 1 because their current model state places
them below/beyond the far web.

Implement one explicitly bounded DEVELOPMENT-ONLY presentation policy for this
candidate:

- preserve the existing depth relationship through linear extrapolation beyond
  t=1;
- do not mutate Egg height, readiness, hatching state, timing, or gameplay
  position;
- do not clamp Eggs to the far rim in this first candidate;
- keep the policy isolated and clearly testable.

This choice preserves the current below-far semantic while moving its visible
anchor onto the record-06 presentation relationship.

Manual acceptance of the resulting Egg presentation remains pending.

Do not generalize this policy to unrelated levels.

ENTITY-SPECIFIC REQUIREMENTS

Route the normal Level 7 Playing presentation of the following through the new
adapter where applicable.

1. BLASTER

- Map the Blaster's current gameplay edge/lane position to the visible mapped
  near-rim edge.
- Derive the visible edge tangent and perpendicular/local basis from the mapped
  record-06 near geometry.
- Preserve Blaster artwork, input, lane state, firing, collision, animation,
  and gameplay size semantics.
- Correct both anchor and orientation.
- Do not address the separate tube-exit Blaster defect in this milestone.

2. ORDINARY AND TANKER-RELEASED FLIPPERS

- Continue using existing gameplay lane, height/depth, pivot, transition, and
  animation state.
- Map their anchors to the corresponding visible record lane/depth.
- Derive tangent and normal/orientation from the mapped visible lane geometry,
  including active transitions where currently supported.
- Preserve Flipper vector art, animation, firing, movement, and cargo identity.
- Do not alter below-rim movement mechanics.

3. TANKERS

- Map the existing lane/depth anchor to the visible record lane.
- Preserve existing Tanker artwork and animation.
- Use a visible-geometry-derived local scale only where the existing renderer
  currently depends on projection scale.
- Do not change cargo, spawning, movement, or firing.

4. EGGS

- Map lane/depth anchors through the adapter.
- Use the explicit t>1 extrapolation policy above.
- Preserve screen-oriented marker artwork unless the existing renderer
  requires a geometric basis.
- Do not alter Egg lifecycle or hatching.

5. SPIKERS

- Map the anchor to the visible lane/depth.
- Preserve gameplay rotation and existing spiral/vector artwork.
- Use mapped visible local scale where current rendering requires projection
  scale.
- Do not alter Spiker motion, firing, or Spike interaction.

6. STARTING AND GENERATED SPIKES

- Map Spike base and tip separately using their existing gameplay depths.
- Draw the Spike along the visible mapped lane's depth direction.
- Preserve Spike height, growth, collision, destruction, and gameplay state.

7. PLAYER SHOTS

- Map the current gameplay lane/depth anchor to the visible record lane.
- Preserve shot timing, slots, collision, movement, artwork, color, and
  animation.
- Use mapped local scale only as presentation data.

8. ENEMY SHOTS

- Map the current gameplay lane/depth anchor to the visible record lane.
- Preserve firing logic, velocity, collision, timing, artwork, color, and
  animation.

9. DESTRUCTION AND RELATED EFFECTS

- Map effect anchors through the visible record geometry when their source
  object/lane is Level 7 gameplay state.
- Preserve existing deterministic effect timing, colors, shapes, and random
  inputs.
- Do not convert screen-oriented art into lane-oriented art unless the
  existing effect renderer already requires a lane basis.

10. LANE HIGHLIGHTS AND PRESENTATION MARKERS

- Keep gameplay lane indices and record presentation indices registered through
  the same explicit identity mapping.
- Apply the same shared centering transform.
- Preserve pulsing/highlight state and color behavior.

OPTIONAL NON-LEVEL-7 ENTITIES

Pulsars and Fuseballs are not part of normal Level 7 composition.

Do not broaden this milestone merely to redesign their presentation.

If existing synthetic Level 7 scenarios render them through shared helpers,
either:

- route their anchors through the same adapter with no gameplay changes when
  this falls out naturally from the shared implementation; or
- preserve existing scenario behavior and report the limitation.

Do not allow optional scenario entities to destabilize the normal Level 7
candidate.

PLAYING-STATE SCOPE

Apply the new registration only to ordinary Level 7 Playing under
--atari-presentation.

Do not modify:

- Approach;
- tube exit;
- transitions;
- Game Over;
- legacy rendering;
- option-off behavior;
- other levels.

The existing Level 7 record-06 tube candidate is already limited to ordinary
Playing. Keep the entity adapter aligned with that supported state.

GAMEPLAY AND DETERMINISM PROTECTION

Do not change:

- LevelDefinition.Points;
- production Level 7 geometry;
- lanes;
- topology;
- collisions;
- movement;
- spawning;
- enemy supply;
- Tanker cargo;
- Flipper routing;
- Spiker behavior;
- Spike behavior;
- Egg lifecycle;
- timing;
- score;
- lives;
- Super Zapper behavior;
- RNG;
- recording format;
- playback input;
- recorded state;
- deterministic simulation.

The renderer may read existing state but must not write simulation state.

Visible recorded playback under --atari-presentation should use the corrected
presentation while reproducing the same underlying simulation.

PROTECT ACCEPTED WORK

Do not alter accepted Levels 1-6.

Do not alter Levels 8-9 candidates.

Do not generalize Level 7 calibration or adapter values to other levels.

Do not alter the existing --atari-presentation paths for other levels.

Do not change the factor-128 diagnostic.

Do not reopen or retune the retained Level 7 tube shape.

Do not investigate or modify the separate Blaster-exit defect.

No image analysis is authorized for this implementation.

CODE QUALITY

Keep the adapter narrowly scoped, explicit, and maintainable.

Add concise comments explaining:

- gameplay geometry remains authoritative for simulation;
- record-06 geometry is used only for visible Level 7 presentation;
- lane/depth registration is render-only;
- identity lane correspondence is an explicit initial PROJECT TRANSLATION;
- Egg t>1 extrapolation is a DEVELOPMENT-ONLY candidate pending manual review;
- centering translation is a PROJECT TRANSLATION candidate.

Do not narrate obvious code.

Avoid scattering Level 7 constants across entity render helpers.

TESTING REQUIREMENTS

Add or update focused tests proving at minimum:

A. FROZEN TUBE AND CENTERING

1. Record-06 bytes and metadata remain unchanged.
2. Projector outputs remain unchanged.
3. Near p=$10 and far p=$F0 remain unchanged.
4. Accepted anisotropic gain remains unchanged.
5. Accepted relative far-set translation remains unchanged.
6. Shared centering translation is applied equally to near and far mapped
   endpoints.
7. The translation moves tube and entities through the same presentation
   frame.
8. Tube shape and relative near/far geometry are invariant under centering.

B. ADAPTER GEOMETRY

9. Identity gameplay-lane-to-record-lane mapping is explicit.
10. t=0 produces the mapped near lane.
11. t=1 produces the mapped far lane.
12. t=.25, .5, and .85 produce exact interpolated lane geometry.
13. t>1 uses the candidate linear extrapolation policy.
14. Lane center is computed from the visible mapped lane endpoints.
15. Tangent and normal/local basis are derived from visible lane geometry.
16. Same mapped endpoint values are reused consistently.

C. REPRESENTATIVE REGISTRATION PROBES

17. The controlling report's representative lane-0 rim, upper, mid-depth,
    deep-end, starting-Spike-tip, and initial-Egg probes now resolve to the
    record-06 presentation positions rather than the old model-projection
    positions.
18. No single legacy entity offset remains in the registration path.
19. Host-camera mapping of the centered candidate matches the expected shared
    presentation frame.

D. ENTITY CLASSES

20. Blaster anchor and basis use visible near-rim geometry.
21. Ordinary and Tanker-cargo Flipper anchors/bases use visible lane geometry.
22. Tanker anchor uses visible lane/depth mapping.
23. Egg anchor uses mapped extrapolated depth without state mutation.
24. Spiker anchor uses visible lane/depth mapping.
25. Spike base and tip use separately mapped depths.
26. Player-shot anchor uses visible lane/depth mapping.
27. Enemy-shot anchor uses visible lane/depth mapping.
28. Destruction/effect anchors use visible mapping where applicable.
29. Rim/lane highlighting uses the same lane correspondence.

E. PROTECTION

30. Gameplay and model state are unchanged before and after render mapping.
31. Recording/playback data formats are unchanged.
32. Deterministic gameplay tests remain unchanged and passing.
33. Legacy and option-off behavior remain unchanged.
34. Levels 1-6 rendering selection remains unchanged.
35. Levels 8-9 rendering selection remains unchanged.
36. Existing supported Level 7 tube rendering remains unchanged apart from the
    shared centering translation.
37. Approach, exit, transition, and Game Over selection remain unchanged.
38. Factor-128 diagnostic remains isolated and unchanged.

Do not use screenshots as automated test oracles.

Use exact mathematical expectations derived from the accepted mapping and
controlling report.

VALIDATION

Run bounded focused tests for:

- Level 7 presentation registration;
- tube mapping and centering;
- entity anchors and local bases;
- representative registration probes;
- render-only state preservation.

Run relevant targeted regressions covering:

- Level 7 gameplay;
- Level 7 presentation;
- direct-Atari prototype;
- --atari-presentation selection;
- entity rendering helpers;
- Blaster;
- Tankers;
- Flippers;
- Eggs;
- Spikers;
- Spikes;
- player shots;
- enemy shots;
- effects;
- recording/playback;
- transition/Approach/exit selection;
- Levels 1-9 presentation selection;
- option-off behavior.

Then run:

- full solution tests with explicit VSTest hang-timeout protection;
- Release solution build;
- git diff --check.

Do not weaken valid tests.

Canceled, timed-out, stalled, blocked, or incomplete commands are not passes.

Follow the standing Windows-security and bounded-command policies.

Do not launch the interactive game.

REPOSITORY SAFETY

Before project changes, record:

- branch;
- HEAD;
- upstream/tracking status;
- index state;
- tracked worktree changes;
- relevant untracked state.

Preserve all pre-existing candidate work and protected material.

The controlling read-only report noted a status-snapshot discrepancy involving
the already-existing AtariLevel7Presentation.cs. Treat the file as pre-existing
candidate work. Do not attribute its creation to this task, delete it, or
overwrite unrelated content.

Do not:

- reset;
- restore;
- clean;
- stash;
- rebase;
- switch branches;
- stage;
- commit;
- or push.

Do not absorb unrelated changes.

AI-LOGS

Follow .github/copilot-instructions.md.

Save:

- this substantive prompt under AI-Logs/Prompts/;
- the paired implementation report under AI-Logs/Reports/.

Use descriptive date-prefixed paired filenames.

The report is required even if findings are also returned in chat.

Do not stage or commit AI-Logs.

MANUAL HANDOFF

After automated validation, STOP.

Do not launch the interactive game.

Provide the exact manual test command:

dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation --level 7

State clearly that Neal should evaluate:

- whether the visible tube is centered below the HUD;
- whether the Blaster sits on the visible rim;
- whether Tankers climb visible tube lanes;
- whether ordinary and Tanker-released Flippers follow visible lanes;
- whether Eggs appear at the visible deep end;
- whether starting Spikes lie on visible lanes;
- whether player and enemy shots follow visible lane/depth paths;
- whether entity orientation and scale look coherent;
- whether gameplay still behaves exactly as before;
- and whether the retained record-06 tube shape remains unchanged.

Do not ask Neal questions during execution.

Do not pause for manual input.

Manual acceptance remains PENDING when the task finishes.

EXPECTED REPORT

The paired implementation report must include:

- implementation outcome;
- files changed;
- adapter architecture;
- exact lane/depth normalization;
- explicit lane-identity assumption;
- Egg t>1 presentation policy;
- centering translation applied;
- entity classes routed through the adapter;
- any entity class intentionally left unchanged;
- representative before/after numerical probes;
- confirmation that simulation and collision geometry remain unchanged;
- confirmation that recording/playback formats and state remain unchanged;
- focused test results;
- targeted regression results;
- full-suite result;
- Release build result;
- git diff --check result;
- starting and ending Git/worktree state;
- exact manual command;
- known limitations;
- manual acceptance status PENDING;
- exact saved prompt/report paths.

Do not stage, commit, or push.

The task ends when the validated Level 7 render-registration and centering
candidate is ready for Neal's manual gameplay/presentation test.

TEMPEST_LEVEL7_RENDER_REGISTRATION_AND_CENTERING_IMPLEMENTATION