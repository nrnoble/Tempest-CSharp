IMPORTANT COMPLETENESS CHECK

Read this entire user-provided task before taking any action.

Before using tools, reading files, running commands, modifying files, or performing repository actions, locate the unique completion token near the end of this task.

Verify that exactly one complete standalone token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

Automatically appended VS Code or agent context after the user's completion token does not invalidate it.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project or repository action.


AUTOMATIC / ACTIVE-EDITOR CONTEXT IS NOT AUTHORIZATION

Automatically surfaced active/recent/editor files are not authorized merely because VS Code supplied them.

Use only files explicitly authorized below or directly necessary to this bounded investigation.

DO NOT read TEMP/myNotes.md.


TASK

Perform a tightly bounded READ-ONLY investigation of Atari Tempest Rev 2 DVG/vector-coordinate orientation, origin, units, and their faithful translation into a canonical modern portrait coordinate space.

Do NOT implement anything.

The Math Box transfer is now decoded and independently verified.

The immediately preceding investigation established for the coordinate path:

M(magnitude, depth) = floor((magnitude * 128) / depth), for depth > 0

and established the CPU-side pipeline through signed lev_y2d and into signed 13-bit DVG/vector words.

The remaining blocker is no longer the Math Box arithmetic.

The remaining blocker is:

- DVG coordinate-axis meaning;
- axis ordering;
- positive directions;
- coordinate origin;
- vector-unit scale/range;
- physical portrait-display orientation;
- any DVG/vector transformation between generated coordinate words and beam location;
- translation into a canonical modern coordinate system appropriate for later C# implementation.

Do not map directly into current C# host pixels until the canonical Atari vector-space mapping is established.

Do not implement C# projection.


CURRENT REPOSITORY STATE

Baseline:

branch:
master

HEAD:
385f603a94391756b2ca6cccd524dc17b281e782

origin/master should match HEAD.

Expected current candidate state:

- clean index;
- 16 intentional modified tracked files;
- two intentional non-ignored untracked files:
  - src/Tempest.Game/Levels/RomPresentationMetadata.cs
  - tests/Tempest.Game.Tests/RomPresentationMetadataTests.cs
- no unrelated user work.

Before research perform bounded read-only checks of:

- branch;
- HEAD;
- origin/master;
- staged changes;
- tracked worktree;
- non-ignored untracked files;
- diff summary.

Read:

AI-Logs/Reports/2026-11-15-external-mathbox-hardware-prom-decode-report.md

Use actual Git state plus that report as authority.

If unrelated user work exists, STOP.

Do not restore, reset, clean, discard, modify, stage, commit, or push anything.


AUTHORITATIVE MATHBOX RESULT

Treat the previous Math Box result as established.

Do not re-decode the PROMs.

The established coordinate transfer is:

depthInput = pointDepth + lev_scale

relativeY = pointY - lev_y3d
relativeX = pointX - 0x80

projectedYMagnitude =
    floor(abs(relativeY) * 128 / depthInput)

projectedXMagnitude =
    floor(abs(relativeX) * 128 / depthInput)

sourceYResult =
    signed lev_y2d
    + sign(relativeY) * projectedYMagnitude

sourceXResult =
    callerBaseX
    + sign(relativeX) * projectedXMagnitude

The coordinate routine restores signs outside the unsigned Math Box divide.

The prior investigation also established:

- the source-Y-derived result is serialized as DVG vector X;
- the source-X-derived result is serialized as DVG vector Y;
- the vector words use signed 13-bit packing after the documented high-bit treatment.

Do not alter these findings unless higher-priority original Atari evidence proves a factual error.

If such a conflict appears, stop and report it rather than silently changing the established model.


PRIMARY QUESTION

Determine the exact relationship among:

1. CPU/mathbox source coordinate components;
2. emitted DVG/AVG vector coordinate words;
3. DVG internal/current beam coordinate system;
4. physical Tempest portrait monitor orientation;
5. a canonical modern portrait coordinate system.

The goal is to produce a transformation that can eventually be expressed conceptually as:

Atari model / ROM presentation data
-> verified Math Box projection
-> signed Atari vector coordinates
-> canonical portrait vector space
-> later C# portrait-presentation fitting

This milestone should stop at canonical vector space.

Do not tune the current 600x800 C# presentation.


SOURCE AUTHORITY

Use this hierarchy:

1. Original Atari Tempest/AVG/vector-generator schematics, manuals, engineering notes, and source comments.
2. Original Atari service documentation.
3. Primary documentation for vector-generator components where needed.
4. Hardware-level implementations explicitly derived from Atari schematics, used as cross-checks.
5. MAME DVG/AVG implementation as independent behavioral cross-check.
6. Byte-exact/call-by-call verified ports as engineering cross-checks.
7. Secondary technical explanations.

Do not use gameplay screenshots as a substitute for hardware/documentation evidence.


EXTERNAL RESEARCH AUTHORIZATION

External research is authorized narrowly for:

- Atari Analog Vector Generator / DVG / AVG coordinate semantics;
- Tempest vector-generator hardware orientation;
- Tempest physical monitor rotation/orientation;
- signed vector coordinate representation;
- vector origin/current-position behavior;
- scale/coordinate interpretation;
- MAME's implementation only for independent verification;
- original schematics/service documentation.

Do NOT research:

- gameplay strategies;
- graphics fidelity opinions;
- fan recreations except hardware-level technical cross-checks;
- presentation tuning;
- Gemini output.

Do not download alternative game ROMs.


LOCAL EVIDENCE

Use project-local evidence where relevant:

- Reference/arcade/Tempest Rom Source code.md
- verified Rev 2 metadata reports
- current Math Box decode report
- existing vector-ROM/DVG analysis reports if present
- current C# source only for later stage comparison, not as Atari authority.

Raw ROM access is not expected in this milestone.

If a specific bounded ROM/vector-ROM lookup becomes essential, identify the exact need before reading it and report it.


DVG / AVG IDENTIFICATION

First establish terminology precisely.

Determine whether Tempest's vector hardware in the relevant presentation path is:

- DVG;
- AVG;
- a Tempest-specific AVG variant;
- or terminology varies between source/comment/hardware layers.

Do not casually conflate DVG and AVG if the coordinate formats differ.

State which coordinate instruction format the CPU-generated words from the coordinate routine actually feed.


VECTOR INSTRUCTION FORMAT

Derive the exact relevant vector instruction format for absolute-position or movement instructions used by the coordinate path.

Determine:

- instruction word count;
- opcode bits;
- coordinate bits;
- sign representation;
- coordinate width;
- scaling bits;
- intensity/color relevance if any;
- whether values are absolute positions or deltas;
- current-position semantics;
- global/local scale state.

Focus only on the commands relevant to positioning/drawing the projected tube.

Do not audit the complete vector instruction set unless required.


13-BIT SIGNED COORDINATES

The previous source analysis reports signed 13-bit coordinate values.

Establish exactly:

- valid numeric range;
- sign-extension rule;
- which physical instruction bits contain the coordinate;
- whether upper bits are ignored, opcode bits, or sign-related;
- wrap, clamp, or truncation behavior;
- how negative coordinates interact with the vector generator.

Cross-check against original Atari documentation where possible.


AXIS ORDER

Resolve why:

- source-Y-derived Math Box result is serialized in the vector-X word;
- source-X-derived Math Box result is serialized in the vector-Y word.

Determine whether this represents:

A. deliberate axis swap for portrait cabinet orientation;

B. historical naming convention in the 6502 source;

C. vector-generator serialization ordering only;

D. another hardware relationship.

Do not infer the answer solely from the apparent swap.

Use Atari/MAME/hardware evidence.


PHYSICAL MONITOR ORIENTATION

Establish Tempest's physical vector-monitor orientation and how the game's logical vector axes map to visible:

- screen left/right;
- screen up/down.

Determine:

- cabinet monitor rotation;
- whether video hardware itself rotates coordinates;
- whether software produces already-rotated coordinates;
- whether modern emulators rotate final output after vector generation.

Provide a clear transformation diagram.

Distinguish:

hardware vector X/Y
from
visible portrait X/Y.


VECTOR ORIGIN

Determine the vector generator's coordinate origin/current-position semantics.

Questions:

- Is (0,0) a corner, center, or another logical point?
- Does the generator maintain a signed current vector position?
- Does the game establish an origin through a center/STAT/CNTR-type command?
- Does Tempest use a vector-generator center instruction?
- What absolute/current coordinates exist after that operation?
- How does the coordinate routine's callerBaseX participate?

Identify the exact center/origin used for tube projection where source permits.


COORDINATE RANGE AND CLIPPING

Determine source-backed vector coordinate range and hardware clipping/visibility behavior.

Questions:

- What signed coordinate range can the generator represent?
- What region is physically visible?
- Are coordinates beyond visible monitor range legal?
- Is clipping done digitally, analogly, or effectively by monitor deflection limits?
- Is there a scale register affecting vector displacement versus coordinate value?

Do not infer modern viewport clipping from analog monitor behavior.


VECTOR UNITS

Determine what one Atari vector coordinate unit means relative to:

- DVG/AVG integer coordinate space;
- vector generator current position;
- DAC/integrator input;
- physical display, where documentation permits.

The goal is not millimeters.

The goal is to determine whether there is an exact or conventional canonical logical coordinate range suitable for software reproduction.

If physical analog transformation prevents one exact pixel equivalent, say so explicitly.


CANONICAL PORTRAIT SPACE

If hardware orientation and coordinate ranges are established, derive one canonical modern portrait coordinate mapping.

Requirements:

- deterministic;
- preserves Atari geometry;
- preserves sign/orientation;
- does not introduce C# project-specific scaling yet;
- can represent the complete relevant Atari vector range;
- has an explicitly documented origin and dimensions/range;
- preferably uses Atari logical units directly or a simple exact rotation/translation thereof.

Possible conceptual examples only:

canonicalX = transformed Atari visible-horizontal coordinate
canonicalY = transformed Atari visible-vertical coordinate

Do not assume 600x800, 720x960, or 800x600.

Derive from Atari coordinate semantics.


CANONICAL TRANSLATION EQUATION

Provide an explicit equation/mapping from the two Math Box/CPU result words into canonical portrait vector coordinates.

Include:

- axis swap if required;
- sign inversion if required;
- origin translation if required;
- unit scale if justified;
- source of any translation constant.

Every constant must be provenance-labeled.

Do not introduce magic fitting constants.


REPRESENTATIVE NUMERIC EXAMPLES

Use the established synthetic examples from the Math Box report for:

- Level 1;
- Level 4;
- Level 7;
- Level 8;
- Level 9.

For each, carry the resulting DVG/vector coordinates through the newly derived orientation/origin mapping into canonical portrait coordinates.

These remain synthetic evidence examples, not visual acceptance.

Do not use screenshots.


KNOWN VECTOR-LIST ORACLE

Seek a numeric oracle independent from the newly derived transformation.

Potential oracles:

- original Atari vector-list examples;
- vector-generator diagnostic/self-test expected positions;
- MAME vector coordinate outputs;
- service-manual test patterns;
- byte-exact port/vector list output;
- known CNTR/current-position behavior.

Prefer original diagnostic data if available.

Generate at least a small set of input vector words and expected logical coordinates.

Cross-check the canonical mapping.

Separate independent evidence from same-model self-consistency.


MAME CROSS-CHECK

MAME may be used only as an independent behavioral cross-check.

Determine how MAME maps Tempest's vector coordinates through:

- AVG interpretation;
- rotation metadata or renderer orientation;
- final normalized/vector coordinates.

Do not adopt arbitrary MAME host-resolution scaling as Atari hardware truth.

Use MAME to validate:

- axis ordering;
- signs;
- rotation;
- coordinate origin;
- vector scale ratios where appropriate.


CURRENT C# COMPARISON

Only after deriving canonical Atari portrait coordinates, compare with current C#.

Determine whether current C#:

- already has a coordinate space that can represent canonical Atari space directly;
- should introduce a separate canonical Atari projection space;
- can map canonical output into the current 600x800 portrait presentation through one final presentation transform.

Do not change code.

Do not assume legacy 800x600 is the canonical Atari vector space.


PRESENTATION FIT VERSUS ARCADE PROJECTION

Explicitly separate:

ATARI PROJECTION:
ROM metadata + Math Box + vector coordinate generation

from

C# PRESENTATION FIT:
mapping canonical Atari portrait geometry into a modern 600x800 / 720x960 / 1280x960 presentation.

Atari projection should not be altered merely to satisfy the modern viewport.

The later C# presentation transform may scale/place the canonical result as a unit.


ROLE OF CURRENT PORTRAIT-PERSPECTIVE DIAGNOSTIC

Determine conceptually whether the current endpoint-normalized:

--portrait-perspective

would sit:

- before canonical vector conversion;
- after canonical Atari projection as an optional project calibration;
- or become unnecessary for ROM-backed projection.

Do not change it.

Treat Level 9 approximately +125 as manual project evidence about the current incomplete projection, not Atari math.


GAMEPLAY GEOMETRY

Determine whether full Atari presentation reconstruction can stay render-only.

Do not recommend changing:

- model lanes;
- LevelDefinition.Points;
- collision geometry;
- enemy/player paths;

unless direct source evidence establishes that current gameplay geometry cannot supply the necessary Atari projection coordinates.

Manual evidence remains that gameplay works.


OBJECTIVE VALIDATION ORACLE

Define the minimum automated oracle required before active implementation.

Now that Math Box arithmetic is exact, a useful future oracle should cover:

model/presentation input
-> Math Box result
-> vector coordinate word
-> canonical portrait coordinate

Identify which parts can be verified exactly and which remain presentation-fit decisions.


CLASSIFICATION

Finish with exactly one:

READY FOR CANONICAL ATARI VECTOR-SPACE FOUNDATION

Use if vector instruction semantics, orientation, origin, units and numeric oracle are sufficient to implement a render-only canonical vector-space layer without changing current visible output.

READY FOR BOUNDED ROM-BACKED PROJECTION CANDIDATE

Use only if the canonical space plus preserved metadata and exact Math Box transfer are sufficient to implement an active reversible presentation candidate.

VECTOR ORIENTATION GAP REMAINS

Use if signs/rotation/origin cannot be established.

VECTOR UNIT SCALE GAP REMAINS

Use if orientation is known but meaningful canonical units cannot be established.

NUMERIC ORACLE GAP REMAINS

Use if mapping appears plausible but lacks objective independent validation.

EXTERNAL REFERENCE CONFLICT

Use if credible Atari/MAME/hardware sources disagree.

Do not choose a ready classification merely to continue development.


FAIL-CLOSED CONDITIONS

Stop before implementation recommendations if:

- vector instruction format remains ambiguous;
- axis orientation is contradictory;
- center/origin semantics are unresolved;
- coordinate width/sign handling is unresolved;
- MAME/reference behavior conflicts materially with Atari documentation;
- canonical transformation requires arbitrary fitting constants.

Report the narrowest remaining gap.


NO IMPLEMENTATION

Do not modify:

- production source;
- tests;
- metadata;
- documentation;
- candidate work;
- ROM/PROM/reference files.

Only create:

- ignored prompt;
- ignored report;
- bounded ignored numerical artifacts if useful.


BOUNDED TOOLING

Small read-only scripts are allowed for:

- signed-coordinate decoding;
- instruction parsing;
- rotation/origin mapping;
- numeric cross-checks.

No dependency installation.

No interactive game.

No full test suite.

Do not analyze visual files.


REPORT

Preserve this substantive prompt under AI-Logs/Prompts.

Create:

AI-Logs/Reports/2026-11-15-dvg-vector-orientation-units-readonly-report.md

Include:

- starting Git state;
- Atari/local/external evidence used;
- vector hardware terminology result;
- relevant instruction format;
- signed-coordinate representation;
- axis-order result;
- physical monitor orientation;
- vector-generator rotation behavior;
- origin/current-position semantics;
- coordinate range;
- clipping/visibility findings;
- vector-unit interpretation;
- canonical portrait-space definition;
- exact canonical transformation;
- representative Level 1/4/7/8/9 examples;
- independent numeric oracle;
- MAME cross-check;
- current C# comparison;
- projection-versus-presentation separation;
- portrait-perspective diagnostic role;
- gameplay-geometry result;
- objective future validation oracle;
- primary classification;
- smallest next milestone;
- remaining gaps;
- final Git state;
- confirmation no source/test/metadata changes;
- confirmation nothing staged/committed/pushed;
- confirmation no visual/Gemini/Incitti material was inspected.


FINAL RESPONSE

Report:

1. Whether investigation completed.
2. Primary classification.
3. Starting/final Git state.
4. Evidence inspected.
5. DVG/AVG terminology result.
6. Relevant vector instruction format.
7. Signed coordinate result.
8. Axis-order result.
9. Physical portrait orientation.
10. Origin/current-position result.
11. Coordinate range.
12. Clipping/visibility result.
13. Vector-unit result.
14. Canonical portrait-space definition.
15. Exact canonical transformation.
16. Level 1 example.
17. Level 4 example.
18. Level 7 example.
19. Level 8 example.
20. Level 9 example.
21. Independent numeric oracle result.
22. MAME cross-check.
23. Current C# compatibility result.
24. Projection-versus-presentation conclusion.
25. Future role of --portrait-perspective.
26. Gameplay-geometry conclusion.
27. Objective validation-oracle result.
28. Smallest next milestone.
29. Remaining gaps.
30. Report path.
31. Confirmation no project changes.
32. Confirmation nothing staged, committed, or pushed.

Do not implement anything.

TEMPEST_DVG_VECTOR_ORIENTATION_UNITS_READONLY_20261115