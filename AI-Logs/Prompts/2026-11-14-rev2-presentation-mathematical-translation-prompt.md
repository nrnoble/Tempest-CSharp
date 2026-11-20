IMPORTANT COMPLETENESS CHECK

Read this entire user-provided task before taking any action.

Before using tools, reading project files, running commands, modifying files, or performing repository actions, locate the unique completion token near the end of this task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

Automatically appended VS Code/agent context after the user's token does not invalidate the token.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project or repository action.


AUTOMATIC / ACTIVE-EDITOR CONTEXT IS NOT AUTHORIZATION

Automatically surfaced active/recent/editor files are not authorized merely because VS Code supplied them as context.

Do not use such files unless this task explicitly authorizes them or they are directly necessary to the bounded investigation.

DO NOT read TEMP/myNotes.md.


TASK

Perform a tightly bounded READ-ONLY mathematical translation investigation of Atari Tempest Rev 2 per-tube presentation/projection data.

Do NOT implement anything.

The preceding milestones have now established:

1. Atari has ROM-backed per-tube:
   - lev_scale
   - lev_y3d
   - signed lev_y2d / lev_y2db
   - lev_open
   - lev_remap

2. The actual Rev 2 values and displayed-level mapping have now been securely extracted from the project-local Rev 2 ROM.

3. Current C# does not represent lev_scale, lev_y3d, and lev_y2d as a coherent ROM-backed per-level presentation model.

4. Manual testing establishes:
   - Level 9 at C# --portrait-perspective approximately +125 looks about right;
   - Level 8 may plausibly use similar perspective strength but has another unresolved presentation issue;
   - Levels 1-7 begin with fundamentally wrong-looking perspective;
   - coefficient tuning does not fix that underlying Levels 1-7 problem;
   - all tested gameplay remains correct.

The purpose of this investigation is to establish the mathematical role and translation architecture of Atari's scale, camera-height, and 2-D placement data before any C# implementation is authorized.


CURRENT REPOSITORY STATE

Baseline:

branch:
master

HEAD:
385f603a94391756b2ca6cccd524dc17b281e782

commit:
Close out ROM-backed enemy-shot rendering

origin/master should match HEAD.

The portrait/perspective candidate remains intentionally modified and unstaged.

Before analysis run bounded read-only checks of:

- branch;
- HEAD;
- origin/master;
- index;
- tracked worktree;
- non-ignored untracked files;
- diff summary.

Compare with:

AI-Logs/Reports/2026-11-14-whole-game-presentation-human-visible-perspective-calibration-report.md

AI-Logs/Reports/2026-11-14-levels1-9-rom-camera-perspective-readonly-investigation-report.md

AI-Logs/Reports/2026-11-14-rev2-presentation-table-extraction-report.md

Expected:

- clean index;
- current portrait/perspective candidate only;
- no unrelated user work.

If unexpected user work exists, STOP.

Do not modify repository state.


AUTHORITATIVE ROM DATA

Use the extracted Rev 2 values from:

AI-Logs/Reports/2026-11-14-rev2-presentation-table-extraction-report.md

For displayed Levels 1-9 the report establishes:

Level 1:
tube 0, scale=0x18, y3d=0x50, y2d=-192, closed

Level 2:
tube 1, scale=0x1C, y3d=0x50, y2d=-224, closed

Level 3:
tube 2, scale=0x18, y3d=0x50, y2d=-192, closed

Level 4:
tube 3, scale=0x0F, y3d=0x68, y2d=-128, closed

Level 5:
tube 4, scale=0x18, y3d=0x50, y2d=-192, closed

Level 6:
tube 5, scale=0x18, y3d=0x50, y2d=-192, closed

Level 7:
tube 6, scale=0x18, y3d=0x68, y2d=-144, closed

Level 8:
tube 7, scale=0x18, y3d=0xB0, y2d=+96, open

Level 9:
tube 13, scale=0x0C, y3d=0xB0, y2d=+320, open

These are ROM-BACKED raw/source-interpreted presentation values.

Do not substitute current C# values for them.

Do not reinterpret +125 as a ROM field.


RAW-ROM ACCESS POLICY

The prior extraction already closed the byte-value gap.

Do NOT reopen raw ROM files merely to repeat extraction.

Use the extraction report as authoritative for the six table values.

Targeted access to the project-local textual disassembly is authorized for mathematical semantics and code-flow interpretation.

If a specific raw byte not present in the extraction report becomes absolutely necessary, STOP and identify the missing evidence rather than expanding raw-ROM scope.


AUTHORIZED TEXTUAL ATARI SOURCE

Use:

Reference/arcade/Tempest Rom Source code.md

Perform targeted bounded reads/searches around only the routines required to derive:

- lev_scale setup;
- lev_y3d setup/use;
- lev_y2d application;
- mathbox coordinate input;
- mathbox output conversion;
- tube drawing coordinate pipeline;
- level-change interpolation of 2-D Y placement;
- any directly necessary fixed-point/sign arithmetic.

Do not perform a broad disassembly audit.


NO VISUAL ANALYSIS

Do not inspect:

- screenshots;
- images;
- videos;
- MAME frames;
- C# frames;
- recordings;
- Gemini material;
- Incitti source;
- web sources.

The manual observations stated in this prompt are the only visual evidence required.


PRIMARY MATHEMATICAL QUESTION

Derive, as far as the authorized source allows, the original Atari transformation from:

- tube/model X/Y;
- depth/Z or equivalent;
- lev_scale;
- lev_y3d;
- signed lev_y2d;

to final vector-screen X/Y.

The desired result is NOT necessarily one neat modern floating-point equation if the original mathbox pipeline does not support that simplification.

Instead establish, step by step:

- which values are subtracted/added;
- which values are negated;
- fixed-point widths;
- sign handling;
- reciprocal/division-like operations;
- multiplications;
- order of operations;
- scale application stage;
- camera-height application stage;
- post-projection 2-D offset stage.

Separate exact source-proven flow from modern mathematical interpretation.


LEV_Y3D DERIVATION

The source identifies lev_y3d as 3-D Y offset / camera height.

Trace exactly how it participates in coordinate conversion.

Determine:

- whether pointY - y3d or y3d - pointY is formed;
- sign handling;
- mathbox input registers;
- depth/scale interaction;
- whether it affects X as well as Y indirectly;
- whether its effect is equivalent to a camera translation before perspective division;
- whether current C# YOffset has the correct mathematical stage;
- what sign/unit conversion would be required before any C# field could represent it.

Use Levels 1, 7, 8, and 9 as representative ROM values:

0x50
0x68
0xB0

Do not implement a conversion.


LEV_Y2D DERIVATION

Trace the signed combined lev_y2d/lev_y2db value.

Determine:

- exact signed representation;
- order relative to mathbox projection;
- whether it is added or subtracted;
- vector coordinate units/fixed-point units where source establishes them;
- how level-change interpolation modifies it;
- whether it behaves as a pure 2-D screen-space Y translation;
- whether current C# CenterY is mathematically the right stage for such a value;
- what additional base-origin conversion would be required in C#.

Use representative values:

Level 1: -192
Level 7: -144
Level 8: +96
Level 9: +320

Do not choose C# CenterY values.


LEV_SCALE DERIVATION

The source describes lev_scale as reciprocal level scale.

Trace:

- how the byte is negated/transformed during level setup;
- which mathbox/register values it feeds;
- whether it acts on X/Y equally;
- interaction with depth;
- whether it modifies a reciprocal-depth denominator, numerator, or separate relative-coordinate stage;
- whether current C# K is even conceptually comparable;
- whether it is closer to camera focal scale, world scale, depth scale, or another operation.

Use representative ROM values:

0x18
0x1C
0x0F
0x0C

Do not map these directly to C# K or 1.05.


DERIVE REPRESENTATIVE LEVEL PIPELINES

Using source-backed operations and the extracted values, construct symbolic or numeric worked examples for:

- Level 1
- Level 4
- Level 7
- Level 8
- Level 9

The examples should show how different ROM scale/y3d/y2d inputs alter presentation even if tube geometry were otherwise held constant.

Do not require exact screen coordinates if required model/depth/mathbox inputs are unavailable.

Where exact calculation stops, identify the missing variable explicitly.


COMPARE LEVEL 9 VERSUS EARLY LEVELS

This is particularly important.

Compare ROM-backed differences:

Level 1:
scale 0x18
y3d 0x50
y2d -192

Level 9:
scale 0x0C
y3d 0xB0
y2d +320

Determine which mathematical differences would be expected to affect:

- camera elevation;
- near/far convergence;
- apparent tube size;
- vertical placement.

Relate this to the MANUAL observation that current C# Level 9 can be made visually convincing with the shared +125 correction while Level 1 cannot.

Do not conclude causation unless the source/math supports it.


OPEN/CLOSED CONTROL

Keep lev_open separate.

Confirm from source whether the mathematical coordinate transformation consumes lev_open.

If not, state explicitly:

OPEN/CLOSED CORRELATES WITH SOME CURRENT MANUAL VISUAL FAMILIES BUT DOES NOT SELECT THE ATARI CAMERA FORMULA.

Do not create topology-based camera logic.


CURRENT C# MATHEMATICAL COMPARISON

Trace current C#:

Projection.Project
endpoint-normalized portrait correction
playfield camera
viewport mapping

Compare mathematical stages against Atari.

For each Atari concept classify the current C# equivalent as:

DIRECT EQUIVALENT
STAGE-ANALOGOUS BUT UNSOURCED
APPROXIMATION
MISSING
NOT COMPARABLE
UNCERTAIN

At minimum classify:

- lev_scale
- lev_y3d
- lev_y2d
- lev_open
- model X/Y
- depth
- final output placement.


TEST THE CURRENT C# CENTER/YOFFSET INTERPRETATION

Without modifying code, determine whether the current C# pattern:

y = CenterY + (worldY + YOffset) * K / (z + ZOffset)

can represent Atari's two independent vertical concepts at all.

Specifically:

- Can YOffset represent a converted y3d term?
- Can CenterY represent a converted y2d term?
- Are additional scale/base-origin conversions sufficient?
- Or does Atari's mathbox order require a structurally different expression?

This is an architectural question.

Do not fit numeric values.


TEST WHETHER PER-LEVEL ROM METADATA CAN FEED ONE SHARED C# PROJECTION

Determine whether a future architecture such as conceptually:

LevelPresentationParameters
- Scale
- CameraHeight
- ScreenYOffset

feeding one shared projection routine is mathematically appropriate.

This is only a conceptual model.

Do not create classes or code.

Determine whether per-level metadata plus a shared equation is preferable to:

- per-level projection functions;
- open/closed branches;
- hand-tuned perspective coefficients.

Give evidence.


ROLE OF --PORTRAIT-PERSPECTIVE

Determine what should happen conceptually to the current endpoint-normalized --portrait-perspective diagnostic if C# later receives ROM-backed per-level camera metadata.

Possible outcomes:

A. still useful as a small project calibration layered over ROM-backed projection;

B. becomes unnecessary except as diagnostics;

C. should be zero for ROM-backed presentation;

D. cannot be determined until implementation/manual comparison.

Do not remove or change it.

Treat Level 9 approximately +125 as manual evidence about the current incomplete C# camera model, not necessarily the value needed after proper ROM translation.


GAMEPLAY GEOMETRY PROTECTION

Determine explicitly which eventual Atari translations belong solely to rendering/projection rather than model/gameplay coordinates.

Do not recommend changing LevelDefinition.Points merely because presentation differs.

Manual evidence says gameplay remains correct.

Preserve geometry unless direct ROM evidence proves the geometry itself is mistranslated.


EVIDENCE CLASSIFICATION

Every major conclusion must be labeled:

ROM-BACKED
SOURCE-BACKED
MATHEMATICAL DERIVATION
PROJECT TRANSLATION
MANUAL OBSERVATION
HYPOTHESIS

Do not describe source interpretation as exact ROM behavior unless directly established.


DECISION MATRIX

Evaluate at least:

A. Reuse current CenterY/YOffset with ROM-derived conversions.

B. Add explicit per-level Scale/CameraHeight/ScreenYOffset metadata feeding current-style projection.

C. Add per-level metadata plus a revised shared projection equation.

D. Maintain current projection and solve with --portrait-perspective alone.

E. Per-level hand-tuned coefficients.

F. Open/closed-specific cameras.

For each report:

- evidence support;
- architectural cleanliness;
- fidelity potential;
- gameplay-risk level;
- unresolved evidence.

Do not implement.


DIAGNOSTIC CLASSIFICATION

Choose one:

READY FOR ROM-BACKED PRESENTATION IMPLEMENTATION

Use only if the mathematical translation is sufficiently established to implement a bounded Levels 1-9 presentation candidate.

READY FOR ROM-BACKED METADATA FOUNDATION ONLY

Use if the raw metadata can safely be introduced but exact projection conversion remains unresolved.

MATHBOX TRANSLATION GAP

Use if the remaining fixed-point/mathbox behavior prevents a responsible C# equation.

CAMERA-HEIGHT TRANSLATION READY, SCALE/PLACEMENT GAP

Use if only part of the model is implementable.

MULTI-STAGE TRANSLATION REQUIRED

Use if several independently understood stages should be implemented/validated separately.

INSUFFICIENT EVIDENCE

Use if the source does not support responsible next work.


NO IMPLEMENTATION

Do not modify:

- C# source;
- tests;
- documentation;
- presentation constants;
- command-line options;
- level geometry;
- candidate worktree.

Only create:

- ignored prompt;
- ignored report.


BOUNDED TOOLING

Small read-only mathematical scripts are allowed.

Do not build or run the game.

Do not run the test suite.

Do not install dependencies.

Temporary scripts must be outside tracked project source and removed afterward.


REPORT

Preserve the prompt under AI-Logs/Prompts.

Create:

AI-Logs/Reports/2026-11-14-rev2-presentation-mathematical-translation-report.md

Include:

- starting Git state;
- source/report files inspected;
- raw ROM data used from extraction report;
- Atari coordinate pipeline;
- lev_scale derivation;
- lev_y3d derivation;
- lev_y2d derivation;
- worked Level 1/4/7/8/9 examples;
- Level 1 versus Level 9 comparison;
- lev_open conclusion;
- current C# mathematical pipeline;
- Atari-to-C# stage map;
- CenterY/YOffset representational analysis;
- conceptual per-level metadata architecture;
- role of current portrait-perspective diagnostic;
- gameplay/model-geometry protection;
- decision matrix;
- evidence classifications;
- primary diagnostic classification;
- smallest recommended implementation milestone;
- unresolved evidence;
- final Git state;
- confirmation candidate files unchanged;
- confirmation nothing staged/committed/pushed;
- confirmation no visual/Gemini/Incitti/web/raw-ROM material was inspected.


FINAL RESPONSE

Report:

1. Whether investigation completed.
2. Primary classification.
3. Starting/final Git state.
4. Source inspected.
5. Atari projection pipeline derived.
6. lev_scale mathematical role.
7. lev_y3d mathematical role.
8. lev_y2d mathematical role.
9. Level 1 worked interpretation.
10. Level 4 worked interpretation.
11. Level 7 worked interpretation.
12. Level 8 worked interpretation.
13. Level 9 worked interpretation.
14. Level 1 versus Level 9 mathematical differences.
15. lev_open camera conclusion.
16. Current C# stage comparison.
17. CenterY conclusion.
18. YOffset conclusion.
19. Whether one shared C# projection plus per-level metadata is supported.
20. Recommended metadata/equation architecture.
21. Future role of --portrait-perspective.
22. Gameplay-geometry conclusion.
23. Decision-matrix summary.
24. Smallest recommended next implementation milestone.
25. Remaining evidence gaps.
26. Report path.
27. Confirmation no source/test changes.
28. Confirmation nothing staged, committed, or pushed.

Do not implement anything.

TEMPEST_REV2_PRESENTATION_MATHEMATICAL_TRANSLATION_20261114