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
4. This check detects an incomplete/truncated user task, not appended agent
   context.
5. If exactly one complete TEMPEST_ token cannot be found, STOP before project
   or repository action and explain why.

TASK

Implement a bounded Level 9 Atari-presentation candidate, following the same
successful development method used for manually accepted Level 8.

The desired workflow is:

1. verify and reuse the existing recovered Atari Level 9 tube geometry/data;
2. preserve all working gameplay and entity registration;
3. preserve existing projection, perspective, scale, and camera architecture;
4. calculate the complete Level 9 tube envelope;
5. horizontally and vertically place the complete tube in the intended
   HUD-cleared portrait gameplay composition;
6. make only the smallest Level-9-specific final presentation correction
   required;
7. validate;
8. stop for Neal's manual acceptance.

Do not introduce Level-7-specific architecture.

Do not perform screenshot-driven calibration.

LEVEL 8 METHOD IS THE PRIMARY MODEL

Use the accepted Level 8 milestone as the preferred architectural/workflow
pattern.

Relevant implementation/closeout records include:

AI-Logs/Reports/2026-11-17-level8-atari-presentation-implementation-report.md
AI-Logs/Reports/2026-11-17-level8-atari-presentation-closeout-report.md

Level 8 succeeded by:

- reusing already recovered geometry;
- preserving an already coherent entity/tube presentation path;
- calculating the full projected near/far tube envelope;
- determining the HUD-cleared gameplay center;
- applying one Level-specific final translation after projection;
- changing no gameplay, geometry, projection, scale or perspective;
- validating and stopping for manual acceptance.

Repeat that METHOD for Level 9.

Do not copy Level 8's +212.28139 placement value.

LEVEL 9 EXISTING EVIDENCE

Use current project source and established local reports as evidence rather
than assuming Level 9 data must be recovered again.

Existing project evidence indicates:

- displayed Level 9 selects Atari shape row 13 through lev_remap;
- shape row 13 is an open 16-vertex tube with 15 playable lanes;
- Level 9 geometry has already been recovered and translated into the current
  C# Level 9 production definition;
- current project work already contains ROM-derived Level 9 geometry;
- Level 9 presentation metadata has already been recovered;
- existing Level 9 gameplay has previously received manual mechanics
  acceptance.

Verify the CURRENT implementation and current committed baseline before relying
on older reports.

Do not reopen raw ROM files if existing recovered data and tests are sufficient.

Do not search the Internet for another disassembly.

GEOMETRY / PROVENANCE VERIFICATION

Before changing presentation, establish:

1. Which current Level 9 geometry is used by production gameplay.
2. Its Atari row/provenance.
3. The exact ordered point count.
4. Open/closed topology.
5. Playable lane count.
6. Current Level 9 ROM presentation metadata.
7. Current model-frame coordinate translation.
8. Whether --atari-presentation already uses that same geometry.
9. Whether tube and gameplay entities already share the same Level 9 projector.
10. Whether gameplay entities are already visibly/register mathematically to
    the same tube geometry.

If those relationships are already coherent, freeze them.

Do not replace/rebuild Level 9 geometry merely because raw Atari bytes exist
elsewhere.

ENTITY REGISTRATION

Preserve the existing Level 9 entity/tube registration if it is coherent.

Verify representative mappings for:

- Blaster at the rim;
- enemy near the rim;
- enemy at mid-depth;
- enemy near the far/deep end;
- player shot;
- enemy shot;
- starting/developing Spike where applicable;
- deep-end Egg where applicable.

Use mathematical/render-path assertions, not screenshots.

If tube and entities already consume the same geometry/projection path, do not
introduce a new entity adapter.

If a substantial registration defect is discovered, STOP and report it rather
than broadening this Level 9 placement milestone.

PROJECTION / PERSPECTIVE / SCALE

Preserve the current Level 9 projection, perspective, and scale unless a
directly demonstrated Level-9-specific defect makes that impossible.

Do not retune:

- Projection.K;
- Projection.ZOffset;
- shared portrait perspective;
- raw-shape normalization;
- camera zoom;
- viewport dimensions;
- Level 9 geometry;
- entity scaling;

merely to improve placement.

Placement should occur after the existing accepted/current projection.

LEVEL 9 PLACEMENT

Calculate Level 9's complete visible tube envelope under the actual current
Level 9 production values.

Include all projected near and far tube vertices.

Use the real current:

- Level Position;
- Level Depth;
- Level 9 geometry;
- Level 9 metadata;
- model-frame mapping;
- projection/perspective values;
- portrait camera;
- viewport.

Do not assume Level 8's depth interval or placement.

Determine:

- logical tube bounds;
- host-space tube bounds;
- horizontal center;
- vertical center;
- persistent HUD-cleared gameplay boundary;
- target horizontal gameplay center;
- target vertical gameplay center;
- required shared final translation.

HORIZONTAL PLACEMENT

Target horizontal centering in the portrait gameplay area.

If Level 9 is already horizontally centered, do not add an X correction.

If an X translation is objectively required, calculate it from the complete
tube envelope rather than visual fitting.

VERTICAL PLACEMENT

Vertically center the complete Level 9 tube envelope in the available gameplay
composition below the persistent HUD.

Use the same composition rule accepted for Level 8:

- stable HUD/status region above;
- tube in the gameplay area beneath it;
- reasonable upper and lower clearance;
- do not center blindly in the full 960-pixel height if that intrudes into
  HUD composition.

Calculate Level 9's value independently.

Do not import:

- Level 8's +212.28139;
- Level 10's +50;
- Level 7 centering constants;
- older provisional per-level translations.

IMPLEMENTATION

If geometry, projection and entity registration are already correct and one
shared final translation is sufficient:

IMPLEMENT only that Level-9-specific final translation.

Prefer the same architectural seam used successfully by Level 8:
a Level-specific post-projection presentation translation in the existing
AtariPresentationAdapter, provided current architecture confirms that is still
the correct seam.

Apply the translation consistently to:

- tube vertices;
- gameplay entity anchors using the same adapter;
- Blaster/rim presentation;
- shots and effects using that existing world-anchor path.

Do not add per-entity offsets.

Do not move individual tube points.

Do not alter near/far relationships.

Do not change Level 9 geometry.

Do not add a second projection system.

If one uniform translation is not sufficient, STOP and report what additional
problem exists rather than escalating automatically.

LEVEL 7 EXCLUSION

Do not change Level 7.

Do not import or generalize:

- AtariLevel7Presentation;
- AtariLevel7EntityPresentation;
- record-06 prototype mapping;
- Level 7 anisotropic calibration;
- Level 7 far-set translation;
- Level 7 lane/depth adapter;
- Level 7 slowdown work;
- Level 7 frame diagnostics.

Level 7 remains a separate future reconstruction milestone.

LEVEL 8 PROTECTION

Level 8 is manually accepted, closed, committed and frozen.

Preserve exactly:

- recovered shape-7 geometry;
- current projection/entity registration;
- accepted +212.28139 final logical Y placement;
- documentation/acceptance status.

Do not modify Level 8.

Regression tests must protect its frozen placement.

LEVEL 10 PROTECTION

Preserve accepted Level 10 and its frozen +50 logical-pixel placement.

Do not modify its geometry, projection, presentation, gameplay diagnostic, or
accepted documentation.

OTHER LEVEL PROTECTION

Do not alter accepted Levels 1-6 or other presentation behavior.

This is one bounded Level 9 milestone.

GAMEPLAY PROTECTION

Do not change Level 9:

- gameplay geometry;
- lane topology;
- open-end behavior;
- movement;
- Flipper behavior;
- Tanker behavior;
- Spiker behavior;
- Spike behavior;
- shots;
- collisions;
- spawning;
- enemy supply;
- completion;
- Super Zapper behavior;
- score;
- lives;
- timing;
- RNG;
- recording/playback simulation;
- level progression.

This task is presentation-only.

MOTION REGRESSION CHECK

Because Level 7 experienced a severe presentation-specific depth-motion defect,
explicitly verify that Level 9 retains its existing coherent entity projection
path and does not introduce a Level-7-style alternate depth mapper.

Do not change motion behavior.

Add/retain tests showing identical deterministic Level 9 simulation with and
without the Level 9 presentation selection.

This is regression protection, not a new Level 9 timing investigation.

IMAGE POLICY

No image analysis is required or authorized for this implementation task.

Do not search for, enumerate, open, inspect, or analyze screenshots, images,
videos, frames, contact sheets, or unknown visual assets.

Manual visual acceptance occurs after automated validation.

STARTING REPOSITORY STATE

Begin from the current post-Level-8-closeout repository.

Inspect and record:

- branch;
- HEAD;
- upstream/tracking;
- ahead/behind;
- index;
- tracked worktree modifications;
- relevant untracked files.

The Level 8 closeout report says its closeout ended at commit:

60b1e95f080ff9a3f0c78d7b000fd12e22cf0d4b

with origin/master synchronized.

Verify current state rather than assuming it.

Preserve unrelated local work, including the known local instruction/reference
files if still present.

Do not reset, restore, clean, stash, rebase, switch branches, stage, commit, or
push.

This implementation milestone stops before closeout.

AI-LOGS

Follow .github/copilot-instructions.md.

Save:

- this substantive prompt under AI-Logs/Prompts/;
- the paired Level 9 implementation report under AI-Logs/Reports/.

A chat response is not a substitute for the report.

Do not stage or commit AI-Logs.

TESTING REQUIREMENTS

Add or update narrowly relevant Level 9 presentation tests proving at minimum:

1. Displayed Level 9 uses the expected recovered Atari geometry.
2. Level 9 geometry provenance remains explicit.
3. Exact point order is unchanged.
4. Open topology and playable lane count are unchanged.
5. Level 9 presentation metadata remains unchanged.
6. Existing model-frame mapping remains unchanged.
7. Projection/perspective remains unchanged.
8. Tube and entities remain on the same presentation path.
9. Blaster remains registered to the visible rim.
10. Representative near/middle/far enemy anchors remain registered.
11. Player-shot registration remains coherent.
12. Enemy-shot registration remains coherent.
13. Spike/deep-end presentation remains coherent where applicable.
14. Gameplay/model state is unchanged by rendering.
15. Deterministic simulation remains equivalent with and without Level 9
    Atari presentation.
16. Complete tube bounds are correctly calculated.
17. Final horizontal placement is centered.
18. Final vertical placement is centered in the HUD-cleared gameplay area.
19. Placement is one shared final translation.
20. Placement does not alter geometry, scale, perspective or near/far
    relationship.
21. Option-off behavior remains unchanged.
22. Levels 1-7 remain unchanged.
23. Frozen Level 8 +212.28139 remains unchanged.
24. Frozen Level 10 +50 remains unchanged.
25. Approach/transition/exit selection remains unchanged.
26. Recording/playback state and formats remain unchanged.

Do not use screenshot pixels as test oracles.

VALIDATION

Run bounded focused Level 9 production/presentation tests.

Run relevant targeted regressions covering:

- Level 9;
- Levels 1-10 presentation selection;
- Level 8 frozen placement;
- Level 10 frozen placement;
- entity registration;
- recording/playback;
- Approach/transition/exit;
- display/camera;
- option-off behavior.

Then run:

- full solution tests with explicit VSTest hang-timeout protection;
- Release solution build;
- git diff --check.

Do not weaken valid tests.

Canceled, stalled, timed-out, blocked or incomplete commands are not PASS.

Do not launch the interactive game.

MANUAL HANDOFF

After automated validation, STOP.

Provide Neal with the exact Level 9 manual command, expected to be:

dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation --level 9

unless current validated CLI behavior requires otherwise.

Ask Neal to judge manually:

- correct Level 9 tube shape;
- correct open topology;
- horizontal centering;
- vertical centering beneath the HUD;
- Blaster/rim registration;
- enemy registration;
- player/enemy shot registration;
- normal along-tube motion;
- normal rim motion;
- gameplay unchanged;
- no regression to Levels 8 or 10.

Do not ask questions while the prompt is running.

Do not pause for manual input.

Manual acceptance remains PENDING.

EXPECTED REPORT

The paired report must include:

- starting Git state;
- exact Level 9 geometry source/provenance;
- relevant Atari shape row;
- Level 9 point count/topology/lane count;
- current ROM presentation metadata;
- current presentation path;
- whether geometry and metadata were already present;
- entity-registration evidence;
- motion-regression protection evidence;
- tube bounds before placement;
- target gameplay center;
- exact final X/Y translation;
- files changed;
- tests changed;
- explicit confirmation gameplay was not changed;
- explicit confirmation Level 7 architecture was not imported;
- explicit confirmation Level 8 and Level 10 frozen placements remain
  unchanged;
- focused validation;
- targeted validation;
- full-suite validation;
- Release build;
- git diff --check;
- ending Git state;
- exact manual Level 9 command;
- known limitations;
- manual acceptance status PENDING;
- exact AI-Logs prompt/report paths.

Do not stage, commit, or push.

Complete autonomously and stop with the bounded Level 9 candidate ready for
Neal's manual test.

TEMPEST_LEVEL9_ATARI_PRESENTATION_IMPLEMENTATION