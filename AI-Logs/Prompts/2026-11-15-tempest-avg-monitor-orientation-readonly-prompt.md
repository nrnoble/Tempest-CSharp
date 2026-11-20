IMPORTANT COMPLETENESS CHECK

Read this entire user-provided task before taking any action.

Before using tools, reading files, running commands, modifying files, or performing repository actions, locate the unique completion token near the end of this task.

Verify that exactly one complete standalone token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

Automatically appended VS Code or agent context after the user's completion token does not invalidate it.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project or repository action.


AUTOMATIC / ACTIVE-EDITOR CONTEXT IS NOT AUTHORIZATION

Automatically surfaced active/recent/editor files are not authorized merely because VS Code supplied them.

Use only files expressly authorized below or directly necessary to this bounded investigation.

DO NOT read TEMP/myNotes.md.


TASK

Perform a tightly bounded READ-ONLY primary-hardware investigation of the remaining Tempest AVG-to-physical-portrait orientation/sign gap.

Do NOT implement anything.

The preceding investigation classified the remaining blocker as:

VECTOR ORIENTATION GAP REMAINS

It already established:

- Tempest uses a Tempest-specific AVG rather than a DVG for this coordinate path;
- vscale b=1 l=0 is emitted;
- vcentre establishes the logical vector center;
- VCTR operands are center-relative displacements;
- coordinates are signed 13-bit two's-complement values in [-4096,+4095];
- source-Y-derived Math Box result is serialized as VCTR X;
- source-X-derived Math Box result is serialized as VCTR Y;
- Math Box arithmetic is independently verified;
- MAME provides a useful behavioral cross-check but is not Atari hardware authority.

The unresolved question is now specifically:

What exact axis/sign/rotation relationship maps these centered Atari VCTR coordinates through the original Tempest AVG output and monitor wiring into the physical portrait image seen by the player?

Resolve that question from original Atari hardware documentation where possible.

Do NOT fit coordinates from screenshots.
Do NOT modify C#.


CURRENT REPOSITORY STATE

Baseline:

branch:
master

HEAD:
385f603a94391756b2ca6cccd524dc17b281e782

origin/master should match HEAD.

Expected current candidate:

- clean index;
- 16 intentional modified tracked files;
- two intentional untracked metadata/test files:
  - src/Tempest.Game/Levels/RomPresentationMetadata.cs
  - tests/Tempest.Game.Tests/RomPresentationMetadataTests.cs
- no unrelated user work.

Before analysis perform bounded checks of:

- branch;
- HEAD;
- origin/master;
- staged changes;
- tracked worktree;
- non-ignored untracked files;
- diff summary.

Read:

AI-Logs/Reports/2026-11-15-external-mathbox-hardware-prom-decode-report.md

AI-Logs/Reports/2026-11-15-dvg-vector-orientation-units-readonly-report.md

Use actual Git state plus these reports as authority.

If unrelated work exists, STOP.

Do not restore, reset, clean, discard, modify, stage, commit, or push anything.


AUTHORITATIVE EXISTING RESULTS

Do not redo:

- ROM presentation-table extraction;
- Math Box PROM decoding;
- command $14 analysis;
- microcode execution;
- endpoint-normalized C# diagnostic work.

Treat these findings as established unless original Atari primary documentation proves a factual conflict:

Math Box command $14 transfer for production positive depths:

M(magnitude, depth) =
    floor((magnitude * 128) / depth)

CPU/source coordinate stages:

depthInput = pointDepth + lev_scale

relativeY = pointY - lev_y3d
relativeX = pointX - 0x80

sourceYResult =
    signed lev_y2d
    + sign(relativeY) * M(abs(relativeY), depthInput)

sourceXResult =
    callerBaseX
    + sign(relativeX) * M(abs(relativeX), depthInput)

Atari VCTR packing:

Vx = signed13(sourceYResult)
Vy = signed13(sourceXResult)

vcentre immediately precedes the relevant VCTR.

These Vx/Vy values are centered vector displacements.


PRIMARY SOURCE GOAL

Locate and inspect the best available original Atari documentation sufficient to establish:

1. Tempest AVG vector X output signal path.
2. Tempest AVG vector Y output signal path.
3. DAC polarity for each axis.
4. analog integrator polarity where applicable.
5. board connector/pin mapping carrying X/Y deflection to the monitor.
6. monitor X/Y deflection input wiring.
7. polarity through the deflection amplifiers.
8. physical orientation/mounting of the monitor in the cabinet.
9. whether the physical portrait image requires:
   - axis swap;
   - X inversion;
   - Y inversion;
   - 90/270-degree rotation;
   - another fixed transform.

The desired output is one evidence-backed 2x2 sign/permutation matrix, plus center translation semantics if required.


SOURCE PRIORITY

Use this hierarchy:

1. Original Atari Tempest PCB schematics.
2. Original Atari Math Box/AVG schematics.
3. Original Atari monitor/deflection schematics applicable to Tempest.
4. Original Atari Tempest operation/service documentation and cabinet drawings.
5. Original Atari engineering notes or wiring diagrams.
6. Primary monitor-manufacturer schematics/manuals if Atari documentation references them.
7. MAME only as an independent behavioral cross-check.
8. Hardware-level schematic-derived FPGA implementation as a separate engineering cross-check.
9. Secondary technical explanations only where primary evidence is unavailable.

Do not let MAME's ROT270 metadata substitute for Atari wiring evidence.


EXTERNAL RESEARCH AUTHORIZATION

External web/PDF research is explicitly authorized only for:

- original Tempest schematics;
- Tempest AVG/Analog Vector Generator hardware;
- Tempest monitor and deflection board;
- cabinet monitor mounting/orientation;
- wiring harness/connector pinouts;
- original Atari technical documentation;
- primary monitor technical documentation;
- MAME/hardware-model cross-checks.

Do not research gameplay strategies, screenshots, videos, fan presentation tuning, or unrelated Tempest implementations.


PDF / SCHEMATIC HANDLING

When a PDF or schematic is located:

- identify title;
- publisher/manufacturer;
- publication/manual number where present;
- revision/date where present;
- exact page/sheet number;
- relevant net labels;
- relevant connector/pin labels;
- signal direction;
- polarity/inversion evidence.

Do not rely solely on OCR text when schematic symbols or net connectivity are decisive.

Inspect the actual relevant schematic page where possible.

Do not reproduce whole copyrighted pages in the report.

Summarize the technical evidence and cite page/sheet references.


AVG OUTPUT PATH

Trace from digital AVG coordinate/current-position outputs through:

- X/Y DAC inputs;
- DAC outputs;
- sample/hold or analog stages if present;
- integrators;
- analog switches;
- scaling stages;
- PCB output connector;
- monitor deflection inputs.

Determine which digital logical direction increases each analog output.

Record every inversion.

Do not assume an op-amp stage inverts merely from naming; trace the actual topology where readable.


MONITOR DEFLECTION PATH

Trace:

- horizontal deflection input;
- vertical deflection input;
- any polarity inversion;
- yoke connection signs;
- board-to-yoke wiring;
- relevant cabinet orientation.

The goal is not analog simulation.

The goal is only to establish:

digital +X / -X
digital +Y / -Y

to:

player-visible right / left
player-visible down / up.


CABINET ORIENTATION

Determine from original cabinet/monitor documentation:

- whether the monitor is physically rotated;
- direction of rotation where establishable;
- how the X/Y deflection axes appear to a standing player;
- whether software/vector hardware already accounts for portrait orientation before the monitor.

Do not use emulator ROT metadata as primary proof.


CENTER / ORIGIN

Determine whether:

- vcentre corresponds to physical beam center;
- any analog DC offset moves the visible center;
- the monitor or AVG introduces a nonzero per-axis center offset after vcentre.

If the hardware's logical center is the physical display center modulo analog calibration, state that.

Do not introduce modern host coordinates.


SCALE AND VISIBLE RANGE

Investigate only enough analog/vector scaling to answer:

- whether the same nominal logical VCTR unit has the same gain on both relevant axes;
- whether any fixed X/Y gain/aspect correction exists;
- whether the portrait image uses equal logical units with physical monitor geometry handling the shape;
- whether hardware gain differs by axis;
- whether a known clean logical visible range exists.

Do not derive millimeters.

Do not map to pixels.

If gain is adjustable/calibration-dependent, report that instead of inventing one exact physical scale.


CANONICAL PORTRAIT TRANSFORM

If the evidence is sufficient, define a canonical modern portrait coordinate space entirely in Atari logical units.

Preferred form:

input:
    centered VCTR displacement (Vx, Vy)

output:
    centered portrait logical displacement (Px, Py)

with:

[Px]   [a b] [Vx]
[Py] = [c d] [Vy]

where a,b,c,d should be limited to exact:
-1
0
+1

unless primary hardware evidence proves an additional fixed axis scale is required.

Examples of possible matrices include swaps/rotations/reflections, but DO NOT choose one without evidence.

Define:

- +Px = player-visible right
- +Py = player-visible down

if that convention is adopted.

State the convention explicitly.

No 600x800 fitting belongs in this matrix.


MAME CROSS-CHECK

After deriving the Atari hardware transform, compare against:

- MAME AVG_TEMPEST output handling;
- MAME ROT270;
- vector-buffer orientation.

Determine whether the composed MAME transform is equivalent in sign/permutation to the primary-hardware result.

If MAME differs:

- investigate why;
- do not automatically prefer MAME;
- report any driver/render-layer convention causing the difference.


SELF-TEST VECTOR ORACLE

Use the already identified Atari vector-list examples as an independent logical cross-check.

Examples include:

- VCTR X +60, Y 0;
- VCTR X 0, Y +256;
- VCTR X +16, Y -256.

The Atari source labels repeated X=0, Y=+256 strokes as vertical lines in a self-test display.

Use the final hardware-derived transform to determine what player-visible direction those vectors should have.

This is particularly useful for validating swap/sign conclusions.

Do not inspect a screenshot of the self-test.


HARDWARE-LEVEL FPGA CROSS-CHECK

A schematic-derived Tempest FPGA implementation may be used only after deriving the Atari result.

Inspect:

- final AVG coordinate outputs;
- any explicit axis swap/inversion;
- video orientation handling.

Treat agreement as engineering cross-verification, not primary authority.


REPRESENTATIVE MATHBOX OUTPUTS

Carry the established representative VCTR results through the hardware-derived canonical matrix:

Level 1:
Vx=-96, Vy=+64

Level 4:
Vx=-73, Vy=+74

Level 7:
Vx=-96, Vy=+64

Level 8:
Vx=0, Vy=+64

Level 9:
Vx=+202, Vy=+78

Report resulting canonical centered portrait displacements.

Do not add C# fit/scale.


VECTOR-UNIT CONCLUSION

State whether canonical units can simply remain one-to-one with VCTR integer units.

If original analog gains are axis-dependent but adjustable, determine whether canonical software space should remain native VCTR units and leave physical/aspect calibration to the later presentation-fit stage.

Do not embed analog calibration pots into logical coordinates unless primary Atari behavior requires it.


CURRENT C# COMPARISON

After canonical matrix is derived, determine conceptually how it should relate to C#:

ROM metadata
-> exact Math Box transfer
-> signed VCTR coordinates
-> canonical centered portrait Atari coordinates
-> modern presentation fit
-> current 600x800 portrait logical area
-> 720x960 viewport
-> 1280x960 host

Do not implement.

Determine whether the current C# 600x800 portrait space is suitable as the final fit target without claiming it is an Atari-native coordinate space.


ROLE OF CURRENT PRESENTATION CALIBRATION

Keep frozen:

- -33.1 placement;
- 1.05 scale;
- --portrait-perspective mechanism;
- manually observed Level 9 approximately +125.

These are project translations/calibration evidence.

Do not fold them into the canonical Atari matrix.

State whether they should be reevaluated only after ROM-backed Atari projection becomes active.


GAMEPLAY GEOMETRY

Do not change or recommend immediate changes to:

- lane geometry;
- LevelDefinition.Points;
- collisions;
- enemy/player paths.

The canonical transform is render/presentation evidence.

If a later active implementation needs raw Atari model coordinates distinct from current gameplay points, identify that as a future translation seam rather than changing gameplay.


OBJECTIVE VALIDATION ORACLE

If the canonical transform is derived, define automated oracle cases using:

- known VCTR instruction words;
- self-test vector list;
- center behavior;
- representative Math Box outputs;
- hardware-derived sign matrix;
- MAME independent sign/permutation cross-check.

The oracle must be sufficient to test a future canonical-vector conversion independently of screenshots.


FAIL-CLOSED CONDITIONS

Stop without a canonical matrix if:

- relevant original schematic cannot be obtained;
- AVG X/Y output paths cannot be traced;
- monitor input axes cannot be identified;
- polarity cannot be established;
- cabinet mounting direction remains ambiguous;
- Atari primary evidence conflicts without resolution;
- a matrix would require guessing from MAME only.

Classify the narrowest remaining gap.


CLASSIFICATION

Finish with exactly one:

CANONICAL ATARI PORTRAIT TRANSFORM ESTABLISHED

Use if original hardware documentation establishes the center-relative axis/sign matrix and it passes independent cross-checks.

CANONICAL TRANSFORM ESTABLISHED, PHYSICAL SCALE ADJUSTABLE

Use if sign/orientation is established but analog physical gain is adjustable or non-exact; canonical VCTR logical units remain usable.

AVG OUTPUT POLARITY GAP

Use if vector word semantics are known but AVG analog-output polarity is unresolved.

MONITOR DEFLECTION POLARITY GAP

Use if AVG outputs are known but monitor/yoke polarity is unresolved.

CABINET ORIENTATION GAP

Use if electronics are known but physical mounting/visible orientation remains unresolved.

ATARI SCHEMATIC ACQUISITION GAP

Use if necessary original pages cannot be obtained.

EXTERNAL REFERENCE CONFLICT

Use if primary sources disagree.

Do not choose a ready classification to continue development.


NO IMPLEMENTATION

Do not modify:

- C# source;
- tests;
- metadata;
- documentation;
- candidate;
- ROM/PROM/reference files.

Only create:

- ignored prompt;
- ignored report;
- small ignored numerical mapping artifact if useful.


BOUNDED TOOLING

Small scripts are permitted for:

- signed vector decoding;
- matrix composition;
- MAME transform comparison;
- oracle generation.

No dependency installation.

No interactive game.

No image/video gameplay analysis.


REPORT

Preserve this substantive prompt under AI-Logs/Prompts.

Create:

AI-Logs/Reports/2026-11-15-tempest-avg-monitor-orientation-readonly-report.md

Include:

- starting Git state;
- primary Atari documents consulted with URLs/page/sheet identifiers;
- relevant schematic signal path;
- AVG output polarity;
- connector/wiring evidence;
- monitor deflection polarity;
- cabinet orientation evidence;
- center/origin result;
- axis-gain result;
- player-visible axis result;
- canonical matrix;
- canonical-unit decision;
- self-test vector oracle result;
- MAME cross-check;
- FPGA/hardware-model cross-check if used;
- Level 1/4/7/8/9 transformed examples;
- current C# relationship;
- presentation-calibration separation;
- gameplay-geometry conclusion;
- future automated oracle;
- primary classification;
- smallest next milestone;
- remaining uncertainty;
- final Git state;
- confirmation no source/test/metadata changes;
- confirmation nothing staged/committed/pushed;
- confirmation no gameplay visual/Gemini/Incitti material was inspected.


FINAL RESPONSE

Report:

1. Whether investigation completed.
2. Primary classification.
3. Starting/final Git state.
4. Atari documents obtained.
5. Relevant schematic pages/sheets.
6. AVG X/Y output paths.
7. AVG output polarity.
8. Board-to-monitor wiring.
9. Monitor/yoke polarity.
10. Cabinet physical orientation.
11. vcentre/physical-center conclusion.
12. Axis gain/scale conclusion.
13. Player-visible axis mapping.
14. Canonical portrait matrix.
15. Canonical unit definition.
16. Self-test oracle result.
17. MAME cross-check.
18. Hardware-model cross-check.
19. Level 1 transformed example.
20. Level 4 transformed example.
21. Level 7 transformed example.
22. Level 8 transformed example.
23. Level 9 transformed example.
24. Current C# relationship.
25. Future role of current project calibration.
26. Gameplay-geometry conclusion.
27. Objective validation oracle.
28. Smallest next milestone.
29. Remaining gaps.
30. Report path.
31. Confirmation no project changes.
32. Confirmation nothing staged, committed, or pushed.

Do not implement C# projection.

TEMPEST_AVG_MONITOR_ORIENTATION_READONLY_20261115