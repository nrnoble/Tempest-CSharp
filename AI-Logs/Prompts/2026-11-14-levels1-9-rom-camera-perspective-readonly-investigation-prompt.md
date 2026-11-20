IMPORTANT COMPLETENESS CHECK

Read this entire user-provided task before taking any action.

Before using tools, reading project files, running commands, modifying files, or performing repository actions, locate the unique completion token near the end of this task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code or agent-generated reminders, tagged-file information, attachment metadata, active/current editor files, or other automatically appended context may appear after the user's completion token and do not invalidate it.

This check exists to detect an incomplete or truncated user task, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before taking any project or repository action and explain why.


AUTOMATIC / ACTIVE-EDITOR CONTEXT IS NOT AUTHORIZATION

Files automatically surfaced by VS Code because they are open, active, recently viewed, suggested, or otherwise included as editor/chat context are NOT authorized merely because VS Code shows them to the agent.

Do not read or use automatically surfaced files unless this task expressly authorizes them or they are directly necessary within the bounded investigation below.

In particular:

DO NOT read TEMP/myNotes.md.

TEMP/myNotes.md is Neal's personal scratchpad and is not project evidence unless Neal explicitly authorizes it in the current user task.


TASK

Perform a tightly bounded READ-ONLY ROM/source-backed investigation of the fundamental projection/presentation difference between:

- current C# Levels 1-7;
- current C# Level 8;
- current C# Level 9;
- Atari's original per-level tube projection/presentation system.

Do NOT implement anything.

New manual evidence changes the question substantially:

- after several hours of manual calibration, Neal judges --portrait-perspective approximately +125 to be about right for Level 9;
- Level 8 may plausibly use similar perspective strength but has a separate unresolved presentation problem;
- Levels 1-7 are fundamentally wrong in perspective before meaningful --portrait-perspective calibration begins;
- changing the perspective coefficient on Levels 1-7 merely changes or exaggerates an already-wrong presentation;
- all tested levels continue to play correctly;
- altered perspective has not produced gameplay problems;
- extreme values distort presentation as expected;
- Neal has not observed clipping during this manual experimentation.

Therefore:

Do NOT attempt to find individual coefficients for Levels 1-7.

Do NOT attempt to tune the current shared coefficient until Levels 1-7 look acceptable.

Determine instead whether C# is missing, mis-translating, or conflating original Atari per-level projection/presentation inputs.


CURRENT AUTHORITATIVE REPOSITORY STATE

Repository baseline remains:

branch:
master

HEAD:
385f603a94391756b2ca6cccd524dc17b281e782

commit:
Close out ROM-backed enemy-shot rendering

origin/master should remain at the same commit.

The current portrait/perspective candidate remains uncommitted and unstaged.

Before analysis, determine the exact current worktree state and compare it against:

AI-Logs/Reports/2026-11-14-whole-game-presentation-human-visible-perspective-calibration-report.md

The index is expected to be clean.

Do not fail merely because an old prose checkpoint described a different modified-file count.

Use actual Git state plus the latest report.

If unrelated or unexpected user work exists, STOP and report it.

Do not restore, reset, clean, discard, overwrite, stage, commit, or push anything.


AUTHORITATIVE CURRENT REPORTS

Read:

AI-Logs/Reports/2026-11-14-whole-game-presentation-perspective-envelope-readonly-analysis-report.md

AI-Logs/Reports/2026-11-14-whole-game-presentation-endpoint-normalized-perspective-implementation-report.md

AI-Logs/Reports/2026-11-14-whole-game-presentation-human-visible-perspective-calibration-report.md

Use these for:

- current presentation architecture;
- endpoint-normalized perspective formula;
- transition-safe composition;
- accepted shared placement;
- diagnostic-option semantics;
- earlier bounded ROM-source findings.

Do not treat previously discarded Gemini prompts/reports as evidence or authority.


MANUAL EVIDENCE TO PRESERVE

Evidence classification:

MANUAL / ARCADE-COMPARISON CALIBRATION:

- Level 9 at approximately +125 appears about right to Neal.
- This is a C# PROJECT CALIBRATION value, not an Atari constant.
- Do not choose a final value or bake +125 into production during this investigation.

MANUAL C# OBSERVATION:

- Level 8 may plausibly respond to a similar coefficient but has another unresolved presentation problem.

MANUAL C# OBSERVATION:

- Levels 1-7 have fundamentally wrong apparent perspective before coefficient tuning.
- Perspective coefficient tuning does not correct the root problem.

MANUAL GAMEPLAY OBSERVATION:

- all levels tested continue to play correctly;
- changing presentation perspective has not affected gameplay;
- extreme values distort presentation;
- no clipping has been observed by Neal during this manual calibration.

Do not silently elevate any manual observation to ROM-BACKED status.

Do not discard manual evidence because it is not directly encoded in the ROM.


FROZEN ACCEPTED PRESENTATION FOUNDATION

Do not propose changing during this investigation:

- 1280 x 960 host window;
- 600 x 800 portrait logical presentation;
- 720 x 960 centered portrait viewport;
- output scale 1.2;
- playfield scale 1.05;
- manually accepted shared vertical placement -33.1;
- endpoint-normalized perspective implementation;
- K=70 stable value;
- ZOffset=5 stable value;
- gameplay/model geometry;
- current level topology;
- gameplay mechanics;
- transition timing;
- playback determinism;
- HUD/presentation separation.

These may be analyzed only where necessary to explain the fundamental per-level projection discrepancy.

Do not recommend changing -33.1 merely to make an enclosed tube look different.


NO VISUAL-ASSET ANALYSIS

Do not search for, enumerate, open, inspect, generate, or analyze:

- screenshots;
- images;
- videos;
- MAME frames;
- C# frames;
- gameplay recordings;
- comparison images;
- contact sheets;
- active-editor images.

The manual observations above are the visual evidence for this source investigation.

Do not inspect Gemini material.

Do not inspect Incitti source.

Do not search the web.


AUTHORIZED ROM / ARCADE SOURCE

Use the project-local ROM-derived source/disassembly:

Reference/arcade/Tempest Rom Source code.md

Targeted bounded reads/searches are authorized.

Do NOT inspect raw ROM binary files or ROM directories.

Do NOT derive unknown table values by opening protected/raw ROM data.

If the local textual source does not contain a required value, state that explicitly rather than guessing it.


PRIMARY QUESTION

Why do current C# Levels 1-7 begin from a fundamentally wrong-looking perspective while Level 9 can be calibrated naturally through the shared portrait-perspective control?

Determine whether current C# is:

1. missing original Atari per-level presentation/projection inputs;
2. using incorrect values for those inputs;
3. translating Atari values incorrectly;
4. encoding equivalent concepts through CenterY/YOffset but with incorrect semantics;
5. transforming current ROM-derived Levels 8-9 differently from Levels 1-7;
6. conflating camera-height, 2-D placement, scale, and presentation calibration;
7. or exhibiting another source-demonstrable mismatch.

Do not assume open/closed topology is the cause merely because the symptoms appear correlated.

Do not invent:

if (openTube) ...
else ...

unless explicit original source evidence establishes such presentation logic.


ATARI PER-LEVEL TABLE INVESTIGATION

Investigate the original source's per-level table concepts for all 16 tube definitions where the textual source permits.

In particular trace:

- lev_scale;
- lev_y3d;
- lev_y2d;
- lev_y2db;
- lev_open;
- any closely related level-table fields that are demonstrably involved in projection/presentation.

For each field determine from source:

- storage format;
- signed/unsigned interpretation where explicitly establishable;
- how it is indexed;
- which runtime variable receives it;
- what routine consumes it;
- whether it controls scale, camera height/elevation, 2-D placement, topology, or another function;
- whether it participates during ordinary tube drawing;
- whether it participates in transitions;
- whether it alters gameplay/model geometry or only rendering/presentation.

Use address/routine names where useful.

Separate:

ROM-BACKED
REFERENCE-DERIVED / SOURCE INTERPRETATION
PROJECT TRANSLATION
MANUAL OBSERVATION
HYPOTHESIS


EXTRACT TABLE VALUES ONLY WHERE TEXTUALLY AVAILABLE

If actual per-level values for Levels 1-16 are present in the authorized textual source, report them in a compact matrix.

For current production Levels 1-9, report every textually established value.

Do not fabricate missing values.

If some entries are placeholders, omitted chunks, unresolved labels, or otherwise unavailable in the textual disassembly, state:

NOT AVAILABLE FROM AUTHORIZED TEXT SOURCE

Do not substitute current C# values and call them ROM values.


CURRENT C# LEVELS 1-9 MATRIX

For production Levels 1 through 9, report current C# values/concepts that influence presentation/projection, including where applicable:

- LevelDefinition point-generation source;
- open/closed status;
- vertex count / playable-lane count;
- CenterY;
- YOffset;
- any scale applied while importing/constructing tube coordinates;
- any reflection/negation performed by LevelLoader;
- Position;
- Depth;
- current K;
- current ZOffset;
- portrait presentation mapping;
- any level-specific presentation/projection handling.

Clearly identify which C# values are:

- literal imported/ROM-derived data;
- project translations;
- manually calibrated presentation values;
- legacy implementation choices;
- current shared defaults.


LEVELS 1-7 VERSUS LEVELS 8-9 LOADER PATH

Pay particular attention to whether Levels 1-7 and Levels 8-9 reach C# through different geometry/data construction paths.

Trace:

- where Levels 1-7 point coordinates come from;
- where Levels 8-9 point coordinates come from;
- scaling;
- centering;
- sign conventions;
- Y reflection/negation;
- source coordinate origins;
- CenterY/YOffset assignment;
- topology construction.

Determine whether C# has effectively embedded some arcade presentation/camera transformation into model-space geometry for one group but not another.

This is a source question, not a request to change geometry.


CENTER Y / YOFFSET SEMANTICS

Current C# projection is conceptually:

x = centerX + x * K / (z + ZOffset)

y = CenterY + (y + YOffset) * K / (z + ZOffset)

plus the separate endpoint-normalized portrait-perspective term.

Determine whether C# CenterY and YOffset correspond cleanly to Atari's:

- lev_y3d camera height;
- lev_y2d / lev_y2db 2-D placement;

or whether that apparent correspondence is misleading.

Specifically determine:

- whether camera-height should mathematically enter before reciprocal-depth division;
- whether 2-D placement should enter after projection;
- whether current YOffset enters at the correct stage;
- whether CenterY enters at the correct stage;
- whether C# currently uses one value to approximate two Atari concepts;
- whether sign convention differs.

Do not modify formulas.


SCALE SEMANTICS

Determine whether Atari lev_scale is:

- world-geometry scale;
- projection scale;
- reciprocal scale;
- mathbox input;
- some combination.

Compare with current C#:

- imported point-coordinate scaling;
- K;
- playfield scale 1.05;
- portrait output scale 1.2.

Do NOT assume these are equivalent merely because each influences apparent size.

Identify which stage of the pipeline each belongs to.


OPEN/CLOSED TOPOLOGY

Investigate lev_open separately.

Determine whether source shows lev_open affecting:

- only lane/rim wrapping;
- tube drawing topology;
- player movement;
- camera/perspective parameters;
- selection of different per-level projection data;
- something else.

This is crucial because manual observation suggests non-enclosed tubes may behave differently visually, but that is not evidence that lev_open directly selects a different camera.

Explicitly state whether any open-versus-enclosed camera rule is ROM-backed.


ROM PIPELINE VERSUS C# PIPELINE

Produce a side-by-side conceptual mapping:

ATARI:
level table
-> level setup
-> scale/camera-height/2-D offset runtime values
-> relevant mathbox/projection input
-> vector coordinates

C#:
LevelLoader / LevelDefinition
-> points + CenterY/YOffset
-> Projection.Project
-> endpoint-normalized portrait correction
-> playfield camera
-> host viewport

Identify where C# has:

- a direct equivalent;
- an approximation;
- no equivalent;
- uncertain mapping.


LEVEL 9 CALIBRATION ROLE

Use Level 9 +125 only as a diagnostic reference.

Ask:

If the C# base camera/presentation for Level 9 is structurally close enough that +125 produces a convincing result, what is different about the base pipeline for Levels 1-7?

Do not tune Levels 1-7 to +125.

Do not make +125 the default.

Do not rewrite Level 9.

Do not infer that Atari has a value "125".


LEVEL 8 SEPARATE PROBLEM

Inspect Level 8's loader/projection path sufficiently to determine whether a source-level difference from Level 9 could plausibly explain Neal's statement that Level 8 may accept similar perspective strength but has another presentation problem.

Do not fix Level 8.

Identify the problem only if source evidence supports a specific diagnosis.

Otherwise classify:

LEVEL 8 ADDITIONAL PROBLEM NOT YET ISOLATED.


GAMEPLAY / PRESENTATION SEPARATION

Manual testing indicates that all tested levels continue to play correctly while presentation perspective changes substantially.

Investigate enough source architecture to state whether:

- movement/collision/lane identity operate on level/model coordinates;
- the perspective transformation is render-only;
- changing presentation leaves gameplay geometry intact.

Do not use this as permission to modify model geometry.

The preferred eventual solution should preserve the existing gameplay/model coordinates unless compelling ROM evidence proves the model itself has been imported incorrectly.


TRANSITION AND PLAYBACK SCOPE

Do not re-investigate the already accepted endpoint-normalized transition solution except where needed to understand per-level camera data.

Do not redesign:

- ExitZoom;
- ReverseZoom;
- Approach;
- playback.

State whether any discovered per-level Atari camera/presentation data would conceptually need to feed those presentation paths later.

No implementation.


BOUNDED NUMERICAL ANALYSIS

Small external calculations are allowed if useful.

Use only bounded level ranges and source-derived/current-C# values.

Do not optimize parameters.

Do not fit coefficients to screenshots.

Do not attempt to automatically solve for perspective values.

The goal is architectural/source understanding.


DO NOT IMPLEMENT

Do not modify:

- source code;
- tests;
- project configuration;
- tracked documentation;
- candidate constants;
- perspective coefficient;
- parser behavior;
- level definitions;
- geometry;
- projection;
- HUD.

The only allowed files created are:

- ignored prompt log;
- ignored read-only report.


DECISION MATRIX

At minimum compare these possible explanations:

A. Current C# CenterY/YOffset are fundamentally adequate; only coefficient calibration is missing.

B. C# lacks/misplaces Atari per-level camera-height semantics.

C. C# lacks/misplaces Atari 2-D Y-placement semantics.

D. C# has incorrect per-level scale translation.

E. Levels 1-7 versus 8-9 geometry import paths produce inconsistent presentation semantics.

F. Open/closed topology directly changes camera rules.

G. Multiple factors above contribute.

For each provide:

- supporting evidence;
- contrary evidence;
- evidence classification;
- confidence: HIGH / MEDIUM / LOW.

Do not force a single-factor explanation if evidence supports a combination.


DIAGNOSTIC CLASSIFICATION

Finish with one primary classification:

READY FOR ROM-BACKED PER-LEVEL PRESENTATION TRANSLATION

Use only if the Atari concepts and their proper C# equivalents are sufficiently established for one bounded implementation.

READY FOR ONE CAMERA-SEMANTICS CORRECTION

Use if one shared mathematical-placement error explains Levels 1-7 without requiring per-level guessed constants.

ROM DATA EXTRACTION GAP

Use if required per-level Atari values are missing from the authorized textual source and implementation would require additional ROM evidence.

GEOMETRY-IMPORT SEMANTICS MISMATCH

Use if the principal problem lies in inconsistent LevelLoader transformations rather than camera metadata.

MULTI-FACTOR PRESENTATION MISMATCH

Use if more than one established problem must be separated into subsequent bounded milestones.

INSUFFICIENT EVIDENCE

Use if no responsible next implementation can yet be specified.

Do not choose a classification simply to unblock coding.


REPORT

Preserve this prompt under AI-Logs/Prompts.

Create:

AI-Logs/Reports/2026-11-14-levels1-9-rom-camera-perspective-readonly-investigation-report.md

The report must include:

- starting Git state;
- exact source/reports inspected;
- confirmation candidate files unchanged;
- current manual findings;
- Atari per-level field definitions;
- actual Atari table values where authorized text exposes them;
- explicit unavailable values;
- Atari runtime/projection data flow;
- C# Levels 1-9 matrix;
- Levels 1-7 loader path;
- Levels 8-9 loader path;
- CenterY/YOffset semantic analysis;
- scale semantic analysis;
- lev_open analysis;
- Atari-versus-C# pipeline map;
- Level 9 +125 diagnostic interpretation;
- Level 8 finding;
- gameplay/presentation separation;
- decision matrix;
- root-cause findings;
- unresolved evidence gaps;
- primary diagnostic classification;
- smallest recommended next milestone;
- whether additional arcade/MAME frames or Gemini video observation would materially help;
- final Git state;
- confirmation nothing staged/committed/pushed;
- confirmation no images/videos/Gemini/Incitti/web/raw-ROM material was inspected.


FINAL RESPONSE

Report:

1. Whether investigation completed.
2. Starting/final Git state.
3. Confirmation candidate unchanged.
4. Source files inspected.
5. ROM-backed per-level presentation concepts discovered.
6. Actual per-level ROM values available from authorized text.
7. Values unavailable from authorized text.
8. C# Levels 1-9 presentation/projection matrix.
9. Key Levels 1-7 versus 8-9 differences.
10. CenterY semantic conclusion.
11. YOffset semantic conclusion.
12. lev_y3d mapping conclusion.
13. lev_y2d mapping conclusion.
14. lev_scale mapping conclusion.
15. lev_open conclusion.
16. Whether C# is missing an Atari camera-height concept.
17. Whether the geometry import paths contribute.
18. Interpretation of Level 9 approximately +125.
19. Level 8 finding.
20. Gameplay/presentation separation conclusion.
21. Decision-matrix summary.
22. Primary diagnostic classification.
23. Smallest recommended next milestone.
24. Whether more visual/video evidence is needed.
25. Report path.
26. Confirmation nothing staged, committed, or pushed.
27. Confirmation no visual assets or prohibited references were inspected.

Do not implement anything.

TEMPEST_LEVELS1_9_ROM_CAMERA_PERSPECTIVE_READONLY_20261114