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

Implement a bounded Level 8 Atari-presentation candidate using the successful
Level 10 milestone as the preferred development pattern:

- establish and use the correct existing Atari tube data;
- preserve gameplay/model behavior;
- preserve correct projection/perspective;
- place the tube correctly in the portrait gameplay composition;
- stop for Neal's manual acceptance.

Do not use the experimental Level 7 reconstruction/registration work as the
default architectural model for Level 8.

LEVEL 10 MODEL FOR THIS MILESTONE

Use the accepted Level 10 Atari-presentation milestone as the workflow and
architectural reference where applicable.

Relevant closeout:

AI-Logs/Reports/2026-11-17-level10-atari-presentation-closeout-report.md

Level 10 was manually accepted after preserving its geometry,
projection/perspective, entity registration, and gameplay while correcting
final placement.

Do not copy Level 10 constants into Level 8.

Copy the METHOD:

1. establish correct Level 8 geometry/provenance;
2. preserve existing working gameplay and registration;
3. make the smallest presentation-only correction required;
4. center/place the tube appropriately;
5. validate;
6. stop for manual acceptance.

CURRENT BASELINE

Begin from the current committed/pushed baseline after Level 10 closeout.

Do not assume the starting commit or worktree state from an older report.
Inspect:

- branch;
- HEAD;
- origin/upstream;
- index;
- tracked worktree;
- relevant untracked files.

Preserve unrelated local work.

LEVEL 8 GEOMETRY / ATARI DATA

First determine exactly how current Level 8 tube geometry is sourced under
--atari-presentation.

Specifically establish:

1. Which Atari tube record corresponds to displayed Level 8.
2. Whether that record and its metadata are already present in
   AtariTubeRecordCatalog and/or RomPresentationMetadata.
3. Whether current --atari-presentation Level 8 already consumes that recovered
   Atari data directly or instead uses production C# LevelDefinition.Points.
4. Whether the existing current Level 8 tube is already geometrically correct
   but merely mis-scaled/misplaced.
5. Whether gameplay entities are already correctly registered to the Level 8
   tube.
6. Whether Level 8 motion/timing remains normal with --atari-presentation.

Use project-local Atari ROM/disassembly evidence and existing recovered data.

Do not search the Internet for another disassembly.

Do not reopen raw ROM files unless bounded verification is genuinely necessary
under the project instructions.

If the required Level 8 Atari record data is already recovered and validated,
reuse it rather than extracting it again.

Do not invent tube geometry.

DO NOT IMPORT LEVEL 7 ARCHITECTURE BY DEFAULT

Level 7 is a separate unresolved/reconstruction milestone.

Do not copy or generalize from Level 7:

- AtariLevel7EntityPresentation;
- Level-7-specific lane/depth registration;
- Level-7-specific anisotropic calibration;
- Level-7 far-set translation;
- Level-7 reference-shape fitting;
- Level-7 centering values;
- Level-7 motion investigations;
- Level-7 factor-128 diagnostics;
- Level-7 Flipper transition handling.

Do not modify Level 7 in this task.

If current shared code necessarily passes through something developed during
Level 7 work, preserve existing behavior and do not broaden its use unless the
Level 8 evidence independently justifies it.

GAMEPLAY PRESERVATION

Level 8 gameplay is protected.

Do not change:

- production Level 8 gameplay geometry;
- lane topology;
- entity movement;
- Flipper behavior;
- Tankers;
- Spikers;
- Spikes;
- shots;
- collisions;
- spawning;
- completion;
- Super Zapper behavior;
- score;
- lives;
- timing;
- RNG;
- recording/playback simulation;
- level progression.

The presentation milestone must not change simulation behavior.

ENTITY REGISTRATION

Prefer preserving the existing Level 8 entity rendering/projection path if it
already keeps gameplay visually registered to the tube.

Do not replace working Level 8 entity presentation merely to make the
architecture resemble Level 7.

Verify registration with geometric/math tests using representative:

- Blaster rim position;
- enemy at near, middle, and far depth;
- player shot;
- starting/developing Spike;
- Egg/deep-end representation where applicable.

If entity/tube registration is already coherent, freeze it.

If an actual Level 8 registration defect is established, STOP and report it
rather than introducing a broad Level-7-style adapter as part of this milestone.

PROJECTION / PERSPECTIVE

Preserve the current Level 8 projection/perspective if existing evidence and
registration indicate it is working.

Do not retune perspective simply because a presentation diagnostic exists.

Do not change:

- shared projection formula;
- frozen whole-game perspective values;
- Atari record projection arithmetic;
- general camera architecture;

unless a Level-8-specific demonstrated defect makes the current values
mathematically incompatible with the correct Level 8 tube.

If a deeper projection problem is found, STOP and report instead of mixing it
with positioning work.

CENTERING AND COMPOSITION

The target composition is:

- persistent HUD/status region remains above the tube;
- the Level 8 tube is horizontally centered in the portrait gameplay area;
- the Level 8 tube is vertically centered in the available gameplay
  composition beneath the HUD;
- preserve reasonable lower margin;
- do not center blindly in the entire 1280x960 host window if that would
  intrude into the HUD region.

Determine current Level 8 visible bounds under --atari-presentation.

Calculate:

- tube bounding box;
- current geometric/bounding-box center;
- intended horizontal gameplay center;
- HUD-reserved boundary;
- available vertical gameplay area;
- intended vertical center;
- exact shared translation needed.

Placement must be one shared final presentation translation.

Do not move individual points.

Do not change tube shape to fix placement.

Do not change near/far relationship to fix placement.

Where Level 8's shape naturally has asymmetric extents, center the complete
visible tube envelope in the gameplay composition unless stronger existing
project evidence requires another placement interpretation.

HORIZONTAL CENTER

Use an objective computed center from the rendered tube bounds/geometry.

Do not fit horizontal placement from a screenshot if the correct center follows
from the portrait viewport/gameplay-area geometry.

The expected goal is the established whole-game composition: visually centered
tube geometry beneath the HUD.

VERTICAL CENTER

Similarly calculate vertical placement from:

- persistent HUD reservation;
- portrait gameplay bounds;
- current Level 8 tube bounds.

Do not import Level 10's +50 placement or any other level's Y constant.

Level 8 gets its own computed translation because tube bounds differ by shape.

IMPLEMENTATION POLICY

If investigation shows that Level 8 geometry, projection and entity
registration are already correct and only placement is wrong:

IMPLEMENT the smallest Level-8-specific final presentation translation required
to center it correctly.

If one uniform translation is sufficient, do only that.

Do not introduce scale/aspect/perspective controls when translation is enough.

If current Level 8 --atari-presentation does not yet use the correct Atari tube
record but the correct record is already available and can be integrated
without changing gameplay:

- use the existing recovered Atari data;
- preserve gameplay geometry;
- preserve entity registration through the simplest justified existing
  presentation architecture;
- keep implementation Level-8-specific;
- do not perform screenshot-driven fitting.

If doing so reveals a substantial registration or projection problem, STOP
before broadening the implementation and report the blocker.

NO IMAGE ANALYSIS BY DEFAULT

No image analysis is required for this implementation task.

Do not search for, enumerate, open, inspect, or analyze screenshots, images,
videos, frames, contact sheets, or unknown visual assets.

If the Level 8 geometry/placement cannot be resolved from ROM/project data and
the current rendering mathematics, STOP and state what bounded visual evidence
would be needed rather than inspecting arbitrary assets.

MANUAL ACCEPTANCE WILL HANDLE VISUAL FIDELITY.

LEVEL 10 PROTECTION

Do not alter the accepted Level 10 milestone.

Preserve its:

- accepted geometry;
- entity registration;
- projection/perspective;
- gameplay;
- +50 final Y placement;
- documentation/acceptance status.

Tests must demonstrate that Level 10 remains on its frozen accepted path.

OTHER LEVEL PROTECTION

Do not alter:

- accepted Levels 1-6;
- unresolved Level 7;
- Level 9 candidate;
- other levels.

This is one bounded Level 8 milestone.

TESTING

Add or update narrowly relevant Level 8 presentation tests proving at minimum:

1. Displayed Level 8 selects the intended Atari tube data/record.
2. Level 8 geometry provenance is explicit.
3. Tube point order/topology remain unchanged.
4. Projection/perspective values remain unchanged unless explicitly and
   independently justified.
5. Entity registration remains coherent.
6. Blaster remains on the visible rim.
7. Representative near/middle/far enemy anchors remain registered.
8. Representative player-shot registration remains coherent.
9. Spike/Egg/deep-end presentation remains coherent where applicable.
10. Level 8 gameplay/model state is unchanged by rendering.
11. Final Level 8 tube bounds are centered horizontally in the intended
    gameplay area.
12. Final Level 8 tube bounds are centered vertically in the HUD-cleared
    gameplay area.
13. Placement uses one shared final translation.
14. Placement does not alter tube geometry, scale or perspective.
15. --atari-presentation off remains unchanged.
16. Levels 1-7 remain unchanged.
17. Level 9 remains unchanged.
18. Accepted Level 10 remains unchanged, including its +50 placement.
19. Approach/exit/transition selection remains unchanged.
20. Recording/playback state and deterministic simulation remain unchanged.

Do not use screenshot pixels as automated test expectations.

VALIDATION

Run bounded focused Level 8 presentation tests.

Run relevant targeted regressions covering:

- Level 8;
- Levels 1-10 presentation selection;
- entity registration;
- Level 10 frozen placement regression;
- recording/playback;
- Approach/transition/exit;
- display/camera;
- option-off behavior.

Then run:

- full solution tests with explicit hang-timeout protection;
- Release solution build;
- git diff --check.

Do not weaken valid tests.

Canceled, stalled, timed-out, blocked or incomplete commands are not passes.

Do not launch the interactive game.

REPOSITORY SAFETY

Before project work record:

- branch;
- HEAD;
- upstream/tracking;
- index;
- tracked worktree changes;
- relevant untracked state.

Preserve unrelated local work.

Do not reset, restore, clean, stash, rebase, switch branches, stage, commit or
push.

This task stops before closeout.

AI-LOGS

Follow .github/copilot-instructions.md.

Save:

- this substantive prompt under AI-Logs/Prompts/;
- the paired Level 8 implementation report under AI-Logs/Reports/.

A chat response does not substitute for the report.

Do not stage or commit AI-Logs.

MANUAL HANDOFF

After automated validation, STOP.

Do not launch the interactive game.

Provide Neal with the exact command for manual Level 8 testing.

Prefer:

dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation --level 8

unless the actual current CLI requires a materially different validated command.

State what Neal should judge:

- correct Level 8 tube shape;
- horizontal centering;
- vertical centering beneath the HUD;
- Blaster/rim registration;
- enemy and shot registration;
- normal along-tube motion;
- normal rim motion;
- gameplay unchanged;
- no regression to other accepted presentation behavior.

Manual acceptance remains PENDING.

EXPECTED REPORT

The report must include:

- starting repository state;
- exact Level 8 Atari tube record/data source;
- whether the data was already present;
- current and final Level 8 presentation paths;
- any code changed;
- any tests changed;
- explicit confirmation that gameplay geometry was not changed;
- explicit confirmation that Level 7 architecture was not generalized into
  Level 8 without independent justification;
- tube bounds before placement;
- calculated intended gameplay center;
- exact final shared translation;
- confirmation that scale/perspective were preserved;
- entity-registration evidence;
- focused validation;
- targeted regression validation;
- full-suite result;
- Release build;
- git diff --check;
- Level 10 regression protection including accepted +50 placement;
- beginning/end Git state;
- exact manual Level 8 command;
- known limitations;
- manual acceptance status PENDING;
- exact AI-Logs prompt/report paths.

Do not ask Neal questions during execution.

Do not pause for manual input.

Complete autonomously and stop with the candidate ready for Neal's manual
Level 8 test.

TEMPEST_LEVEL8_ATARI_PRESENTATION_IMPLEMENTATION