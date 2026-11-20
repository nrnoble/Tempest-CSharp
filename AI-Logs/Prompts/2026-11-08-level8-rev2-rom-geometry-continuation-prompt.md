FAIL-CLOSED PROMPT CHECK

Read this entire user-provided task before doing anything.

Before using tools, reading files, running commands, modifying files, or taking
any repository action, locate the unique completion token near the end of this
user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a
line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment
metadata, or other system-generated context may appear after the user's token
and do not invalidate it.

This check is intended to detect an incomplete or truncated USER task, not
appended agent context.

If exactly one complete TEMPEST_ token cannot be found, stop before any project
or repository action and explain that the supplied user task appears incomplete.


TASK

Continue the read-only Level 8 ROM preimplementation investigation by using the
actual local Atari Tempest Rev 2 ROM binary files to resolve the specific data
missing from the annotated ROM listing.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected accepted baseline:

bc109b4597f83477f5c7998bd3a9e258c72c6edf
Close out accepted interactive speed diagnostic

Confirm the actual Git state before beginning.

This task is investigation-only.

Do not implement Level 8.

Do not modify production code, tests, durable documentation, ROM binaries,
reference files, recordings, screenshots, or Git state.

Do not stage, commit, push, clean, rename, move, or delete anything.


PRIOR LEVEL 8 INVESTIGATION

Read this report before doing new analysis:

AI-Logs/Reports/2026-11-08-level8-rom-preimplementation-investigation-report.md

Treat it as the established first-pass investigation.

Do not repeat analysis that the prior report already resolved unless the raw
Rev 2 ROM binaries directly contradict or materially qualify a finding.

The prior investigation established substantial Level 8 behavior but stopped
because the annotated source represents several geometry/color arrays only as
unexpanded .chunk declarations.

The purpose of this continuation is to recover those missing bytes from the
actual local Rev 2 ROM data and finish the blocked parts of the Level 8
specification.


LOCAL REV 2 ROM BINARIES

The actual Atari Tempest Rev 2 ROM dump files are already available locally at:

Reference/arcade/roms.rev2/

The directory contains these files:

136002-113.d1
136002-114.e1
136002-115.f1
136002-116.h1
136002-118.k1
136002-119.lm1
136002-120.mn1
136002-121.p1
136002-123.np3
136002-124.r3
136002-125.d7
136002-126.a1
136002-127.e1
136002-128.f1
136002-129.h1
136002-130.j1
136002-131.k1
136002-132.l1
136002-217.j1
136002-222.r1
136002-316.h1

These are reference inputs.

Do not:

- edit them;
- rename them;
- move them;
- rewrite them;
- patch them;
- stage them;
- commit them.

Read them only.

If they are currently tracked or ignored, do not change that status during this
task.


PRIMARY EVIDENCE ORDER

Use evidence in this order:

1. Actual local Rev 2 ROM binary bytes.
2. Project-local annotated ROM listing:
   Reference/arcade/Tempest Rom Source code.md
3. Previous Level 8 investigation report.
4. Existing C# architecture and prior accepted project reports as engineering
   context only.

Do not search the internet for another disassembly.

Do not use:

- screenshots;
- MAME visual output;
- gameplay videos;
- Mark Incitti's implementation;
- another Tempest implementation;
- online level charts;
- prior visual knowledge of Level 8

to determine Level 8 tube geometry.

Preserve the blind reconstruction experiment.


FIRST REQUIREMENT: VERIFY ROM MAPPING

Before extracting geometry bytes, establish how the supplied Rev 2 binary files
map into the Tempest CPU and vector-ROM address spaces.

Determine:

- which files contain program ROM;
- which files contain vector graphics/data ROM;
- load addresses;
- sizes;
- ordering;
- any interleaving or address transformation;
- whether revision-specific replacement ROMs substitute for another file;
- how addresses appearing in the annotated listing map to physical ROM files
  and file offsets.

Cross-check this mapping against the annotated listing.

Do not assume that a CPU address can simply be converted to a file offset by
subtracting a guessed base address.

For every subsequently extracted table, record:

- CPU/vector address;
- ROM filename;
- file offset;
- byte length;
- raw byte values.

If ROM mapping cannot be established confidently, stop before interpreting the
geometry and report the blockage.


ROM INTEGRITY / PROVENANCE

Read-only checksums may be calculated if useful.

If the local project already contains expected CRC/SHA data, compare against
that information.

Do not download replacement ROMs.

Do not alter a ROM merely because its checksum differs from an external
expectation.

The task is to analyze the exact local Rev 2 ROM material Neal already has.


MISSING TABLES TO RECOVER

The previous Level 8 investigation identified these annotated-listing
addresses whose raw values were missing:

lev_x      at $B97C
lev_y      at $BA7C
lev_angle  at $BB7C
lev_remap  at $BC7C
lev_open   at $BCCC

Recover the corresponding raw data from the actual Rev 2 ROM files.

Also locate and recover any omitted tables required to establish:

- Level 8 tube scale;
- 2D/3D offsets;
- tube center;
- projection parameters;
- tube color/palette selection;
- level color block;
- other geometry attributes necessary to faithfully reconstruct the tube.

Do not extract unrelated ROM regions merely because binary access is now
available.


LEVEL 8 TUBE SELECTOR

Re-establish from the prior report:

- displayed Level 8 = internal zero-based level index 7;
- effective parameter-table level = $08.

Using the recovered lev_remap bytes:

1. Recover all 16 lev_remap values for context.
2. Identify lev_remap[7].
3. Determine the exact tube/shape index selected by Level 8.
4. Record the raw byte and derivation.

This must be based on the binary data, not a known Level 8 shape.


BLIND LEVEL 8 GEOMETRY RECONSTRUCTION

Using the selected shape index and recovered ROM geometry tables, reconstruct
Level 8 numerically.

Determine:

- all raw X values;
- all raw Y values;
- all raw angle values;
- point order;
- segment order;
- lane count;
- lane connectivity;
- open/closed status from lev_open;
- endpoint behavior if open;
- wrap behavior if closed;
- convex transitions;
- reflex/concave transitions;
- straight transitions;
- directed transition angles;
- any repeated/shared vertices;
- topology relevant to Blaster movement;
- topology relevant to Flipper/Tanker-cargo movement;
- topology relevant to collision;
- geometry-specific scale/offset parameters.

Show the derivation.

Do not merely assign a familiar descriptive shape name.


IMPORTANT: DO NOT VISUALLY CHECK IT YET

After deriving the Level 8 tube, do NOT open:

- MAME;
- screenshots of Level 8;
- arcade video;
- an external topology chart;
- Incitti's Level 8;
- another implementation.

Do not use any visual source to confirm whether the result "looks right."

The eventual Level 8 implementation and Neal's first launch will perform that
visual validation.

The report may describe the geometry mathematically, but should avoid relying
on prior visual knowledge.


PROPOSED C# GEOMETRY

Once the raw Level 8 geometry is known, inspect the existing C# LevelDefinition
and LevelLoader representation.

Produce a complete proposed normalized Level 8 geometry specification.

Include:

- normalized vertex coordinates;
- ordering;
- Continuous/open/closed value;
- playable lane count;
- edges;
- neighboring lanes;
- pivot angles;
- scaling;
- centering.

For every normalization step, explain the transformation from raw ROM
coordinates to proposed C# coordinates.

Keep these evidence categories separate:

RAW-ROM-DATA
ROM-LISTING-BACKED
PROJECT TRANSLATION CANDIDATE

Do not describe normalized C# coordinates as literal ROM coordinates.


OPEN-TUBE AUDIT

If the recovered lev_open value shows Level 8 is open:

Audit existing C# helpers including:

- LevelDefinition.PlayableLaneCount;
- NextEdge;
- PreviousEdge;
- player rim traversal;
- enemy traversal;
- collision;
- directed Flipper pivots;
- rendering.

Determine whether the current general architecture already correctly supports
the exact Level 8 topology.

Do not modify anything.

If the tube is closed, explicitly record that this open-topology concern does
not apply to Level 8.


TUBE COLOR AND PRESENTATION

Recover enough raw data to determine Level 8's ROM tube/palette selection if
possible.

Trace:

- resolved curtube;
- color table indexing;
- color-block bytes;
- any level-specific recoloring;
- any tube-specific presentation parameters.

Do not invent RGB colors from ROM indices.

If existing C# mappings can translate the relevant ROM color indices, report
those as PROJECT TRANSLATION CANDIDATE unless already accepted as equivalent.

Do not use screenshots to choose colors in this task.


STARTING SPIKE $C8 TRANSLATION

The previous report established:

wave_spikeht = $C8

for Level 8 and that the value is copied into all 16 ROM spike-height slots.

Use the actual ROM binary data plus relevant ROM routines to investigate the
coordinate meaning of $C8.

Determine whether the existing C# translation can be derived rather than
guessed.

Specifically trace:

- along-position representation;
- Spike height storage;
- Spike drawing/placement;
- Spiker growth;
- player-shot erosion;
- level exit/Spike collision;
- any coordinate conversion or comparison involving spike_ht.

Compare previous raw values for Levels 4-7 with their accepted C# height
translation if useful.

Do not assume:

$c8 == C# height 200

and do not automatically reuse C# height 60.

If an exact conversion still cannot be established, say so and recommend the
smallest provisional/manual-validation strategy for Level 8 implementation.


TANKER CARGO $20 BOUNDARY

Perform the targeted instruction-by-instruction trace requested by the previous
investigation.

Resolve the cargo movement selection around ROM along-position $20.

Establish precisely:

- comparison operands;
- branch condition;
- unsigned/signed interpretation;
- equality behavior;
- natural Tanker release along-position;
- shot-destroyed Tanker behavior;
- which path selects $07;
- which path selects Level 8 $19;
- whether equality at $20 belongs to $07 or $19.

Report exact addresses and control flow.

Reconcile this with prior accepted Level 7 documentation without modifying any
historical report.

If a historical report used looser wording, identify it as historical wording
rather than rewriting it.


LEVEL 8 $19 FLIPPER PROGRAM

The previous report established Level 8 uses Flipper program $19.

Use raw ROM bytes to confirm its instruction stream.

Document:

- move-along instruction;
- flip-start instruction;
- flip-continuation behavior;
- loop/restart behavior;
- direction behavior;
- whether any reversal opcode appears;
- whether any Spike test appears;
- timing/update implications.

Determine the expected below-rim sequence for a Level 8 Tanker cargo Flipper
using $19.

Do not translate this into a Level 7 2/3 pattern.

Do not implement.


LEVEL 8 SUPPLY AND TYPE FINDINGS

The previous report established, for Medium/default:

wave_enemies = 24
max_enm = 6
Flipper min/max = 0/5
Tanker min/max = 1/2
Spiker min/max = 2/4
Pulsar min/max = 0/0
Fuzzball min/max = 0/0
Tanker cargo = two Flippers

Preserve these unless the actual Rev 2 binary bytes contradict them.

Where practical, use the binary data to confirm the relevant table entries.

Do not repeat the full enemy-supply investigation.

Record either:

CONFIRMED BY REV 2 BINARY

or:

PRIOR LISTING FINDING PRESERVED

for each major parameter.


DEATH / RE-ENTRY

The previous report recommends Level 7-style generic reconstruction for
Level 8.

Do not repeat the entire trace.

Use raw binary evidence only where it helps resolve uncertainty.

Confirm whether any Level 8-specific binary/table value alters that conclusion.

Otherwise preserve the prior recommendation.


LEVEL 7 -> LEVEL 8 PROGRESSION

Do not re-investigate accepted transition mechanics unless the newly recovered
tube-selection/scale/color data introduces a Level 8-specific requirement.

Record the final Level 8 geometry/presentation parameters needed by the
existing transition architecture.


LEVEL 8 -> LEVEL 9

Do not investigate Level 9 beyond what is needed to preserve the existing
terminal Level 8 recommendation.

No Level 9 implementation is authorized.


ROM FILE SAFETY

Before and after analysis:

- verify ROM file names/sizes;
- if useful, calculate hashes;
- do not change timestamps intentionally;
- do not write into the ROM directory;
- do not create patched ROMs;
- do not create modified copies beside the originals.

If extracting tables requires a script or command, write output only to an
ignored working location or directly into the new AI report.

Do not put binary extracts into tracked project locations.

Do not stage or commit ROM-derived temporary files.


NO IMPLEMENTATION

Do not:

- create Level 8;
- add geometry;
- change LevelConfiguration;
- change LevelLoader;
- change progression;
- modify enemy selection;
- modify Flipper programs;
- change Spikes;
- change tests;
- update durable project documentation;
- launch the game;
- launch MAME;
- create recordings;
- stage;
- commit;
- push.

This task is still read-only investigation.


AI-LOG POLICY

These instructions are explicit and non-conflicting:

1. Preserve this complete substantive prompt under:

   AI-Logs/Prompts/

2. Create a NEW continuation report under:

   AI-Logs/Reports/

3. Use a descriptive date-prefixed paired filename.

4. Existing investigation/implementation/closeout reports are historical
   records and must remain unchanged unless explicitly requested otherwise.

5. This task does NOT authorize modifying an existing report.

6. AI-Logs/ is intentionally Git-ignored.

7. Creating the new prompt/report files under AI-Logs/ is permitted.

8. Do NOT stage or commit anything under AI-Logs/.

9. Do NOT put the continuation report under TEMP/, Temp/, or another working
   directory.


CONTINUATION REPORT

Create a report such as:

AI-Logs/Reports/2026-11-08-level8-rev2-rom-geometry-continuation-report.md

The report must contain:

1. Starting Git state.
2. Rev 2 ROM directory/file inventory.
3. ROM mapping into CPU/vector address spaces.
4. Optional hash/integrity information used.
5. Exact ROM file and offset for every recovered table.
6. Recovered lev_x bytes.
7. Recovered lev_y bytes.
8. Recovered lev_angle bytes.
9. Recovered lev_remap bytes.
10. Recovered lev_open bytes.
11. lev_remap[7] result.
12. Selected Level 8 tube index.
13. Exact Level 8 raw geometry.
14. Lane order.
15. Lane count.
16. Connectivity.
17. Open/closed status.
18. Directed pivot/angle analysis.
19. Recovered scale/center/offset information.
20. Recovered color/palette information.
21. Proposed normalized C# geometry.
22. Full ROM-to-C# geometry derivation.
23. Starting-Spike $C8 translation investigation.
24. Exact Tanker-cargo $20 boundary trace.
25. Confirmed Level 8 $19 Flipper program.
26. Rev 2 binary confirmation of Level 8 supply/type parameters.
27. Death/re-entry conclusion.
28. Existing Level 8 findings preserved from the first report.
29. Any first-report findings corrected by raw binary evidence.
30. Exact remaining unknowns.
31. Whether the original geometry blocker is now resolved.
32. Whether Level 8 is now ready for implementation.
33. Revised bounded Level 8 implementation recommendation.
34. Revised automated-test plan.
35. Revised manual-acceptance checklist.
36. Final Git state.
37. Confirmation that ROM binaries are unchanged.


IMPLEMENTATION READINESS GATE

Only declare:

LEVEL 8 READY FOR IMPLEMENTATION

if the Rev 2 binary investigation establishes enough evidence for:

- tube selector;
- exact geometry;
- open/closed topology;
- lane/connectivity representation;
- suitable C# geometry normalization;
- Level 8 enemy supply/type policy;
- $19 Flipper behavior;
- Tanker/cargo path;
- starting Spike strategy or a clearly bounded provisional translation;
- death/re-entry strategy;
- natural Level 7 -> 8 progression;
- terminal Level 8 boundary.

If geometry remains unresolved, do not recommend implementation.


STOP POINT

Stop after the completed Rev 2 binary continuation investigation.

Do not implement Level 8.

At completion provide Neal a concise summary answering:

- Which ROM file contains the missing geometry tables?
- What is lev_remap[7]?
- Which tube index does Level 8 use?
- What exact geometry did the raw bytes produce?
- How many playable lanes are there?
- Is Level 8 open or closed?
- What color/presentation data was recovered?
- Was $C8 successfully translated to C# Spike height?
- What exactly happens at the Tanker cargo $20 boundary?
- What exact behavior does Flipper program $19 produce?
- Did the binary data confirm the previously derived Level 8 enemy parameters?
- Is the blind tube reconstruction complete?
- Is Level 8 now ready for implementation?

TEMPEST_LEVEL8_REV2_ROM_GEOMETRY_CONTINUATION_20261108