IMPORTANT COMPLETENESS CHECK

Read this entire user-provided task before taking any action.

Before using tools, reading project files, running commands, modifying files, or performing repository actions, locate the unique completion token near the end of this task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code or agent-generated reminders, tagged-file information, attachment metadata, active/current editor files, or other automatically appended context may appear after the user's completion token and do not invalidate it.

This check exists to detect an incomplete or truncated user task, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before taking any project or repository action and explain why.


AUTOMATIC / ACTIVE-EDITOR CONTEXT IS NOT AUTHORIZATION

Files automatically surfaced by VS Code because they are open, active, recently viewed, suggested, or otherwise included as editor/chat context are NOT authorized merely because VS Code shows them to the agent.

Do not read or use automatically surfaced files unless:

1. this task explicitly authorizes them; or
2. they are directly necessary source/report files within the bounded investigation defined below.

In particular:

DO NOT read TEMP/myNotes.md.

TEMP/myNotes.md is Neal's personal scratchpad and is never project evidence unless Neal explicitly authorizes it in the current user task.

Do not treat automatically appended editor/file context as overriding these scope restrictions.


TASK

Perform a tightly bounded READ-ONLY investigation of:

1. the arcade/MAME perspective looking down the Tempest tube; and
2. the shared gameplay presentation envelope, including the stable upper region reserved for score/status/lives text.

Do NOT implement anything.

The current experimental portrait candidate has reached an important manual checkpoint:

- Neal considers the current shared vertical placement a very good step in the right direction;
- Neal considers the current vertical placement to be where it should be;
- freeze the current shared vertical placement;
- tube size still appears to need improvement;
- however, the immediately preceding read-only size investigation found that useful uniform enlargement conflicts with the accepted placement/all-level envelope;
- therefore DO NOT implement or tune tube size in this task;
- perspective looking down the tube is now the primary investigation target.

Determine what actually causes the visual perspective difference between the current C# presentation and the supplied arcade/MAME gameplay references, and determine what one shared whole-game presentation/projection model should be investigated next.

No code changes.
No test changes.
No presentation tuning.
No commit or push.


CURRENT AUTHORITATIVE REPOSITORY STATE

Repository baseline:

branch:
master

HEAD:
385f603a94391756b2ca6cccd524dc17b281e782

commit:
Close out ROM-backed enemy-shot rendering

origin/master should remain at the same commit.

The current worktree intentionally contains exactly four tracked modified candidate files:

- src/Tempest.Game/Program.cs
- src/Tempest.Game/Rendering/DisplayConfiguration.cs
- src/Tempest.Game/Rendering/VectorRenderer.cs
- tests/Tempest.Game.Tests/DisplayConfigurationTests.cs

The index is expected to be clean.

No relevant non-ignored untracked files are expected.

Do not restore, reset, clean, discard, overwrite, stage, commit, or push these changes.


STARTING SAFETY CHECK

Before analysis, perform bounded read-only checks of:

- branch;
- HEAD;
- origin/master;
- staged changes;
- tracked worktree changes;
- non-ignored untracked files;
- diff summary.

Expected:

- master;
- HEAD/origin at 385f603a94391756b2ca6cccd524dc17b281e782;
- clean index;
- exactly the four intentional tracked candidate files modified;
- no relevant non-ignored untracked files.

If the state differs materially, STOP and report the discrepancy.

Do not modify the repository to force it into the expected state.


AUTHORITATIVE PROJECT REPORTS

Read only the directly relevant presentation reports:

AI-Logs/Reports/2026-11-12-whole-game-presentation-level9-readonly-investigation-report.md

AI-Logs/Reports/2026-11-12-whole-game-presentation-level9-label-correction-report.md

AI-Logs/Reports/2026-11-13-whole-game-presentation-cross-level-clipping-diagnostic-report.md

AI-Logs/Reports/2026-11-13-whole-game-presentation-shared-vertical-placement-correction-report.md

AI-Logs/Reports/2026-11-13-whole-game-presentation-tube-size-readonly-analysis-report.md

For earlier image-dependent conclusions, the label-correction report supersedes the original investigation report.

The shared-vertical-placement correction report governs the current placement.

The tube-size analysis governs the conclusion that a simple enlarged uniform scale is not currently established as a placement-neutral next step.


CURRENT VALUES TO FREEZE

Do not modify or propose implementation changes to these values during this investigation:

- host window: 1280 x 960;
- portrait logical presentation: 600 x 800;
- centered portrait viewport: 720 x 960;
- output scale: 1.2;
- playfield scale: 1.05;
- shared vertical translation: -33.1;
- horizontal center/placement;
- shared playfield camera architecture;
- separate HUD/screen-space camera;
- --legacy-presentation behavior.

Current projection values:

- K = 70;
- ZOffset = 5.

K and ZOffset are NOT authorized to change.

They may be analyzed as mathematical inputs to the perspective model.

Do not assume in advance that adjusting either is the correct solution.


MANUAL ARCADE OBSERVATIONS TO PRESERVE

Treat the following as Neal's direct/manual arcade observations and presentation requirements:

- the arcade gameplay presentation has a stable upper region containing score/status/lives information above the tube;
- this upper information region exists across gameplay levels;
- tube placement is consistent across all 16 arcade tube layouts;
- margins are consistent across all 16 arcade tube layouts;
- arcade tubes do not visibly jump between fundamentally smaller/larger presentation scales from level to level;
- the desired view has a pronounced looking-down-the-tube perspective that differs from the current C# presentation;
- exact score/status text layout is not the immediate priority;
- preserving the long-term upper text/HUD region IS important now because tube placement must leave space for it;
- leaderboard/attract-screen implementation is deferred until after the game itself is complete.

Do not attempt to implement the leaderboard, attract screen, high-score entry, or final HUD in this task.


EXPLICITLY AUTHORIZED VISUAL FILES

For this investigation ONLY, visual analysis is explicitly authorized for exactly these three files:

Videos/Mame Frames/level7-frame_0224.png

Videos/Mame Frames/level7-frame_1265.png

Videos/Mame Frames/level7-frame_2070.png

These are the three authorized arcade/MAME gameplay reference frames.

Their filenames must NOT be treated as proof that all three depict production Level 7.

Analyze what is actually visible in the frames.

You may open and inspect these three files.


STRICT VISUAL-ASSET DENY RULE

Authorization of those three files does NOT authorize any broader visual search.

DO NOT:

- enumerate Videos/;
- enumerate Videos/Mame Frames/ beyond accessing the three exact paths above;
- search for other PNG/JPG/JPEG files;
- search for screenshots;
- search for MAME frames;
- search for C# frames;
- inspect comparison images;
- inspect video files;
- extract video frames;
- inspect gameplay recordings;
- inspect contact sheets;
- inspect unknown repository images;
- inspect attract/leaderboard screenshots;
- inspect any image merely because VS Code surfaced it as active/current context.

If one of the three exact authorized files cannot be opened, report that limitation.

Do not substitute another image.


GEMINI MATERIAL IS PROHIBITED

Do not search for, locate, read, restore, or use:

- Gemini prompts;
- Gemini reports;
- Gemini chat exports;
- discarded Gemini-generated source;
- constants/formulas derived from that experiment.

The Gemini experiment is not project authority.


VISUAL ANALYSIS: PRESENTATION ENVELOPE

For each of the three authorized gameplay frames, measure or estimate consistently from actual image pixels:

- full image width/height;
- visible gameplay-content horizontal center;
- approximate upper gameplay-text/HUD band;
- highest visible tube point;
- lowest visible tube point;
- leftmost visible tube point;
- rightmost visible tube point;
- tube bounding width;
- tube bounding height;
- horizontal margins;
- gap between gameplay text region and principal tube geometry;
- approximate far/pit/convergence region location;
- approximate near/outer-rim extent where visually identifiable.

Clearly distinguish:

- direct pixel measurements;
- approximate measurements;
- interpretation.

Do not claim arcade hardware resolution from screenshot pixel dimensions.

These frames are captures/reference images, not proof of vector-monitor raster dimensions.


SHARED PRESENTATION-ENVELOPE QUESTION

Use the three frames plus Neal's direct observation to characterize the common gameplay composition.

Determine whether the evidence supports a conceptual shared envelope consisting of:

1. upper score/status/lives region;
2. separation/gap;
3. tube/playfield region;
4. lower margin.

Do not implement the HUD.

Do not derive final text coordinates.

The purpose is to determine what region should remain reserved so later HUD implementation does not force the tube presentation to move again.


PERSPECTIVE QUESTION

Investigate why the arcade frames produce a stronger visual impression of looking down into the tube than the current C# projection/presentation.

Analyze the current source path from:

- model tube point;
- level geometry;
- level CenterY/YOffset or equivalent existing per-level projection inputs;
- world depth;
- Projection.Project;
- K;
- ZOffset;
- near/far depth;
- current shared portrait playfield camera;
- final viewport mapping.

Determine which parts affect:

- overall uniform size;
- vertical placement;
- horizontal placement;
- near-rim apparent scale;
- far-rim apparent scale;
- near/far scale ratio;
- convergence;
- apparent depth;
- apparent camera elevation / looking-down relationship.

Do not conflate post-projection uniform scaling with perspective.

The previous size investigation established that current uniform playfield scaling preserves the existing near/far perspective relationship.


LEVEL 9 AS PRIMARY C# CALIBRATION FIXTURE

Level 9 remains the strongest current C# calibration fixture because its presentation has already been manually compared and its placement accepted.

Using source calculations only, characterize current Level 9:

- near-rim projected bounds;
- far-rim projected bounds;
- near/far width ratio;
- near/far height ratio;
- vertical separation between appropriate near/far geometric references;
- convergence/pit location;
- final host-space positions under the current portrait mapping.

Compare those relationships, cautiously, with measurable relationships in the three authorized arcade frames.

The comparison need not claim that any one authorized frame is Level 9.

The goal is to compare presentation/perspective characteristics, not to falsely identify levels from filenames.


REPRESENTATIVE CROSS-GEOMETRY CHECK

The three authorized frames show different tube geometries.

Use that variation to ask whether the apparently consistent arcade presentation can plausibly be represented by one common projection/presentation rule.

Do not derive a separate calibration per screenshot.

Do not propose separate calibration values per level.

If the evidence suggests one parameter set cannot plausibly serve different geometries, explain exactly why.

Neal's arcade observation remains that the presentation placement/margins are consistently managed across all 16 tube layouts.


ATARI / LOCAL SOURCE EVIDENCE

You may inspect the existing project-local ROM-derived disassembly:

Reference/arcade/Tempest Rom Source code.md

Use targeted searches and bounded reads only.

Do NOT inspect raw ROM binaries/directories.

Determine whether the local disassembly explicitly exposes relevant concepts such as:

- per-level scale;
- 3D vertical/camera offset;
- 2D vertical placement;
- open/closed tube information;
- projection/matrix inputs;
- other presentation parameters directly relevant to the looking-down perspective.

Evidence classification is mandatory:

- ROM-BACKED when directly demonstrated by the local disassembly;
- ARCADE-OBSERVED when supplied by Neal or directly measured from the authorized gameplay frames;
- PROJECT TRANSLATION for current C# implementation choices;
- HYPOTHESIS when evidence is insufficient.

Do not invent missing ROM values.

Do not infer that current C# constants equal arcade ROM constants merely because names or concepts appear similar.


INCITTI RESTRICTION

Do NOT inspect Mark Incitti's implementation during this task.

The goal is to understand the current C# architecture, arcade visual evidence, and project-local ROM evidence without importing another implementation's projection assumptions.

Incitti must not supply perspective constants or architectural decisions here.


PERSPECTIVE PARAMETER SENSITIVITY

Perform bounded analytical sensitivity calculations for the current C# Level 9 projection.

Without modifying source or running the interactive game, determine qualitatively and quantitatively what happens to relevant Level 9 near/far relationships when varying, one at a time:

- K around the current value;
- ZOffset around the current value;
- any existing per-level CenterY/YOffset input relevant to perspective;
- near/far depth relationship, only analytically;
- current post-projection playfield scale.

Use a few bounded analytical probes, not a large parameter sweep.

The purpose is to classify what each variable actually controls.

Do not recommend a numeric implementation candidate merely because one analytical probe appears visually plausible.

Do not change current constants.


SEPARATE SIZE FROM PERSPECTIVE

Explicitly answer:

- Can the arcade-like looking-down perspective be improved without simply enlarging the whole playfield?
- Would a perspective correction likely change Level 9's perceived prominence even if PortraitPlayfieldScale remains 1.05?
- Does a perspective correction create new near-rim clipping risk even while overall uniform scale stays frozen?
- Should tube-size calibration remain deferred until after one perspective candidate is manually evaluated?

Do not implement either change.


ALL-LEVEL SAFETY MODEL

Even though only Levels 1-9 presently exist, the target architecture must support all 16 eventual arcade tube layouts with one coherent presentation system.

Do not analyze nonexistent C# Levels 10-16 as if their geometry already exists.

Instead identify the invariants the future shared presentation should maintain, including:

- reserved top HUD region;
- consistent presentation placement;
- consistent margin policy;
- shared projection semantics;
- no arbitrary per-level screen fitting;
- no changes to gameplay/model geometry merely for presentation;
- coherent tube/entity registration.

Explain what future Levels 10-16 should be tested against once their geometries exist.


COMMAND-LINE / MANUAL-CALIBRATION CONSTRAINT

No command-line changes are authorized in this read-only task.

However, preserve the prior requirement for any future experimental presentation option:

A presentation-only calibration option should remain orthogonal to normal gameplay where practical and should compose with existing supported modes such as:

- --level N;
- --lives N where semantically allowed;
- --speed N;
- --rim-speed N where semantically allowed;
- --record-gameplay <file>;
- --playback-file <file>;
- existing deterministic playback modes.

Do not invent a new option name.

Do not alter parser behavior.

Legacy presentation must remain an unambiguous A/B reference.


READ-ONLY SOURCE SCOPE

Read only source files directly necessary to trace projection and presentation, likely including:

- src/Tempest.Game/Rendering/Projection.cs
- src/Tempest.Game/Rendering/DisplayConfiguration.cs
- src/Tempest.Game/Rendering/VectorRenderer.cs
- src/Tempest.Game/Levels/LevelLoader.cs
- src/Tempest.Game/Levels/LevelConfiguration.cs
- directly required level/model definitions
- src/Tempest.Game/Program.cs only where presentation selection/initialization is relevant
- the five authoritative presentation reports
- the bounded local disassembly sections relevant to projection/presentation.

Do not perform an open-ended repository audit.


NO IMPLEMENTATION

Do not:

- modify production source;
- modify tests;
- modify tracked documentation;
- change presentation constants;
- change K;
- change ZOffset;
- change scale;
- change -33.1;
- add HUD text;
- add leaderboard code;
- add command-line options;
- change model points;
- change level geometry;
- change gameplay;
- change mechanics;
- stage;
- commit;
- push.

The only files this task may create are:

- the ignored saved prompt under AI-Logs/Prompts;
- one ignored analytical report under AI-Logs/Reports.


NO BUILD / TEST / GAME LAUNCH

Do not launch the interactive game.

Do not run the full test suite.

Do not build unless a source-level compile fact absolutely cannot be established another way.

Prefer source inspection, existing report evidence, image measurement, and bounded standalone numerical calculations.

Any calculation loop must have a small explicit bound.


DIAGNOSTIC CLASSIFICATION

Finish with one primary classification:

READY FOR ONE SHARED PERSPECTIVE EXPERIMENT

Use only if the evidence identifies a bounded shared presentation/projection change suitable for one reversible implementation experiment without per-level special casing.

PERSPECTIVE REQUIRES MORE ARCADE EVIDENCE

Use if the three authorized frames cannot constrain the next experiment safely.

PERSPECTIVE REQUIRES MORE ROM EVIDENCE

Use if the architecture is understood but missing ROM-backed inputs prevent a responsible next experiment.

PRESENTATION ENVELOPE MUST BE RESOLVED FIRST

Use if the top HUD/tube spatial relationship must be decided more precisely before perspective can proceed.

ARCHITECTURAL MISMATCH

Use if current C# projection/presentation architecture cannot represent the required shared perspective cleanly without broader redesign.

If secondary issues exist, report them separately.


REPORT

Preserve this substantive prompt under AI-Logs/Prompts according to project instructions.

Create:

AI-Logs/Reports/2026-11-14-whole-game-presentation-perspective-envelope-readonly-analysis-report.md

The report must include:

- starting Git state;
- exact files inspected;
- exact authorized visual files inspected;
- confirmation no other visuals were inspected;
- current frozen presentation values;
- manual observations supplied by Neal;
- per-frame measurement table;
- shared presentation-envelope findings;
- recommended reserved top gameplay-text region concept;
- current C# projection equation/path;
- Level 9 current near/far relationships;
- arcade-reference perspective observations;
- local ROM-source findings with evidence classification;
- K sensitivity;
- ZOffset sensitivity;
- CenterY/YOffset sensitivity where applicable;
- depth sensitivity;
- uniform-scale sensitivity;
- size-versus-perspective conclusions;
- clipping implications of perspective change;
- whole-game/shared-rule feasibility;
- implications for eventual Levels 10-16;
- unresolved uncertainties;
- primary diagnostic classification;
- smallest recommended next experiment, if supported;
- final Git state;
- confirmation candidate files were unchanged;
- confirmation nothing was staged/committed/pushed.


FINAL RESPONSE

Report:

1. Whether the read-only investigation completed.
2. Starting/final branch, HEAD, origin, index, and worktree state.
3. Confirmation current candidate files were unchanged.
4. The three visual files actually analyzed.
5. Confirmation no other visual files were inspected.
6. Shared gameplay presentation-envelope findings.
7. Recommended treatment of the upper HUD/text region.
8. Current C# perspective model.
9. Specific visual/perspective differences supported by the arcade frames.
10. Level 9 current near/far perspective relationships.
11. Relevant ROM-backed presentation/projection findings.
12. What K controls.
13. What ZOffset controls.
14. What CenterY/YOffset controls.
15. What uniform playfield scale controls.
16. Whether size and perspective can remain separate.
17. Whether perspective correction can improve perceived prominence without uniform enlargement.
18. Cross-level/shared-rule feasibility.
19. Any new clipping risk created by changing perspective.
20. Primary diagnostic classification.
21. Smallest recommended next implementation experiment, if supported.
22. Whether more arcade screenshots are needed before that experiment.
23. Report path.
24. Confirmation nothing was staged, committed, or pushed.

Do not implement the recommendation.

TEMPEST_PRESENTATION_PERSPECTIVE_ENVELOPE_READONLY_20261114