IMPORTANT COMPLETENESS CHECK

Read this entire user-provided task before taking any action.

Before using tools, opening files, browsing the web, opening PDFs, running commands, modifying files, or performing repository actions, locate the unique completion token near the end of this task.

Verify that exactly one complete standalone token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

Automatically appended VS Code/agent context after the user's completion token does not invalidate it.

This completeness check exists to detect a truncated user task, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project or repository action.


AUTOMATIC / ACTIVE-EDITOR CONTEXT IS NOT AUTHORIZATION

Automatically surfaced active, recent, suggested, or editor-context files are NOT authorized merely because VS Code supplied them.

Use only files expressly authorized below or directly necessary to this narrowly bounded investigation.

DO NOT read TEMP/myNotes.md.


TASK

Perform one extremely narrow READ-ONLY investigation of the remaining Tempest AVG analog-output polarity gap.

Do NOT repeat the preceding broad AVG/monitor/cabinet investigation.

Do NOT attempt to solve every remaining physical-monitor question.

Do NOT implement anything.

The immediately preceding investigation:

AI-Logs/Reports/2026-11-15-tempest-avg-monitor-orientation-readonly-report.md

finished with:

AVG OUTPUT POLARITY GAP

Read that report first and treat its established findings as authoritative for this task.

The previous investigation already established:

- Tempest AVG/VCTR coordinate semantics;
- centered signed VCTR displacements;
- signed 13-bit representation;
- vcentre behavior;
- source-to-VCTR axis packing;
- Wells-Gardner X = horizontal;
- Wells-Gardner Y = vertical;
- logical center semantics;
- current unresolved physical polarity/orientation gap.

Do NOT redo any of that work.


ONLY PRIMARY QUESTION

Determine, from original Atari schematic evidence if possible:

For increasing positive Tempest AVG/VCTR coordinate output on each logical axis, what polarity appears at the named analog AVG X/Y output leaving the Tempest PCB?

Specifically establish if possible:

1. relationship between increasing logical AVG/VCTR X and named analog X output polarity;
2. relationship between increasing logical AVG/VCTR Y and named analog Y output polarity;
3. exact board connector/pin carrying analog X where documented;
4. exact board connector/pin carrying analog Y where documented;
5. any proven axis swap before the named analog outputs;
6. every proven signal inversion between the relevant digital coordinate/count and analog board output.

This task ends at the Tempest PCB analog output.

Do NOT require final monitor/yoke/player-visible orientation to call this task useful.

Do NOT continue into cabinet mounting unless the same exact schematic directly settles that relationship with no additional research.


FIRST HARD GATE: SHEET 3 SIDE B

The first external-research action must be an attempt to acquire the missing original Atari Tempest drawing-package sheet identified by the preceding report:

Sheet 3 Side B
DP-190-03B

for the matching Tempest Analog Vector-Generator PCB / drawing package.

Possible filename forms may vary, but the target identity must be Sheet 3 Side B / 03B from the relevant Tempest drawing package.

Do NOT start by reopening:

- TM-190;
- the Wells-Gardner manual;
- Sheet 01A;
- Sheet 01B;
- Sheet 02B;
- Sheet 03A;
- cabinet drawings;
- monitor-removal instructions.

Acquire or locate Sheet 03B first.


IF SHEET 03B CANNOT BE OBTAINED QUICKLY

If Sheet 03B cannot be located through a small number of direct searches/links:

STOP RESEARCH.

Do NOT compensate by opening many related manuals or trying to reconstruct the missing sheet indirectly.

Classification:

ATARI SHEET 3B ACQUISITION GAP

Write the report and stop.


EXTERNAL-DOCUMENT LIMIT

After reading the preceding local report, open at most THREE external technical documents/pages during this task.

The allowed three-document budget is:

1. Sheet 03B itself.
2. ONE immediately connected original Atari Tempest schematic sheet only if Sheet 03B sends an essential X/Y net across a sheet boundary.
3. ONE Atari/Wells-Gardner connector or monitor schematic only if needed to identify the output connector/pin after the AVG board output is already established.

Do not open a fourth external document.

Repeatedly opening the same PDF/page still counts against the spirit of this restriction.

Do not open many browser windows or tabs.

Do not browse entire manuals.


PDF / SCHEMATIC RULE

For any PDF:

- inspect only the exact relevant schematic page;
- do not read sequential pages;
- do not browse the table of contents hoping to find another clue;
- do not inspect parts lists;
- do not inspect repair procedures;
- do not inspect monitor-removal instructions;
- do not inspect cabinet photographs;
- do not inspect exploded mechanical assembly views;
- do not inspect unrelated schematic sections.

If the required signal path cannot be read reliably from the exact relevant schematic:

STOP.

Do not spend extended time trying multiple OCR/render/screenshot strategies.


NO MONITOR-ROTATION INVESTIGATION

Do NOT investigate:

- how the monitor was mechanically rotated;
- whether the monitor was mounted clockwise or counterclockwise;
- cabinet portrait orientation from photographs;
- mechanical monitor-removal drawings;
- monitor assembly photographs;
- cabinet construction.

Those questions are explicitly OUT OF SCOPE here.

This task concerns the polarity of the AVG board's named analog X/Y outputs.

A later task can decide whether monitor/cabinet evidence is still necessary.


NO VISUAL GAMEPLAY EVIDENCE

Do not inspect:

- gameplay screenshots;
- MAME screenshots;
- C# screenshots;
- gameplay video;
- recordings;
- YouTube;
- Gemini output;
- Incitti material.

Do not infer electrical polarity from what a Tempest gameplay screen looks like.


DO NOT REPEAT ESTABLISHED HARDWARE ARCHAEOLOGY

Do not re-investigate:

- ROM presentation metadata;
- lev_scale;
- lev_y3d;
- lev_y2d;
- Math Box PROMs;
- command $14;
- Math Box microcode;
- the verified divide equation;
- VCTR signed format;
- AVG versus DVG terminology;
- vcentre;
- current C# projection;
- --portrait-perspective;
- MAME ROT270.

The prior reports are authoritative for those findings.


CURRENT REPOSITORY STATE

This is a READ-ONLY task.

Before external research, perform a bounded Git-state check:

- branch;
- HEAD;
- origin/master;
- staged changes;
- tracked worktree;
- non-ignored untracked files;
- diff summary.

Expected baseline:

branch:
master

HEAD/origin:
385f603a94391756b2ca6cccd524dc17b281e782

Expected candidate state from the preceding report:

- clean index;
- 16 intentional modified tracked files;
- two intentional non-ignored untracked files:
  - src/Tempest.Game/Levels/RomPresentationMetadata.cs
  - tests/Tempest.Game.Tests/RomPresentationMetadataTests.cs
- no unrelated user work.

Use actual state plus:

AI-Logs/Reports/2026-11-15-tempest-avg-monitor-orientation-readonly-report.md

as authority.

If unrelated or unexpected user work exists, STOP.

Do not restore, reset, clean, discard, stage, commit, or push anything.


TRACE REQUIREMENT

If Sheet 03B is obtained and sufficiently legible, trace ONLY the relevant AVG X/Y output circuitry.

For EACH axis identify where possible:

A. digital coordinate/count source entering analog conversion;
B. DAC or equivalent conversion stage;
C. any directly involved latch/switch/current source;
D. integrator/amplifier stage;
E. explicit inverting/non-inverting behavior proven by component topology;
F. named analog output net;
G. board connector/pin where shown;
H. total number of proven signal inversions.

Do not infer inversion merely because an op-amp, transistor or integrator stage exists.

Follow the actual schematic connections.


POLARITY RESULT FORMAT

For each axis, conclude only one of:

AVG +X -> analog X becomes more positive
AVG +X -> analog X becomes more negative
AVG X POLARITY NOT ESTABLISHED

AVG +Y -> analog Y becomes more positive
AVG +Y -> analog Y becomes more negative
AVG Y POLARITY NOT ESTABLISHED

Do not convert these yet to:

- screen right;
- screen left;
- screen up;
- screen down.

That physical/player-visible mapping is outside this task.


CONNECTOR RESULT

If connector evidence is available on the permitted schematic(s), report:

- connector identifier;
- pin number;
- net/signal name;
- X/Y correspondence.

If it is not available:

CONNECTOR/PIN NOT ESTABLISHED

Do not search additional documents solely to fill a connector number after polarity has otherwise been established.


TIME / PROGRESS BOUND

This task must not repeat the previous long PDF investigation.

After the initial local report/Git checks:

If approximately 10 minutes of document/tool work produces no NEW electrical fact about:

- AVG X polarity;
- AVG Y polarity;
- X/Y output connector/net;

STOP external research.

Write the report based on what has actually been established.

Do not continue for an extended period because another PDF might possibly contain the answer.

If one exact relevant schematic is being productively traced and new electrical facts are still being established near the bound, completing that single trace is acceptable.

Do not start another research branch.


NO TOOL-WORKAROUND LOOPS

If PDF or schematic tooling becomes the blocker:

- do not repeatedly try alternate OCR systems;
- do not repeatedly reopen the same PDF;
- do not convert dozens of pages;
- do not enumerate images;
- do not create contact sheets;
- do not begin manual page-by-page browsing;
- do not open 15 browser windows.

Stop and report:

SCHEMATIC LEGIBILITY GAP

or:

ATARI SHEET 3B ACQUISITION GAP

as appropriate.


SOURCE PRIORITY

Use:

1. original Atari Sheet 03B;
2. directly connected Atari schematic sheet if required;
3. exact monitor/connector schematic only if already necessary under the three-document limit.

Do not use MAME to establish polarity.

Do not use FPGA/MiSTer to establish polarity in this task.

Do not use secondary explanations unless they merely help locate the original sheet.


NO IMPLEMENTATION

Do not modify:

- C# source;
- tests;
- ROM metadata;
- durable documentation;
- project configuration;
- ROM/PROM files;
- reference files;
- current presentation candidate.

Only create:

- ignored prompt copy;
- ignored report.


CLASSIFICATION

Finish with exactly one primary classification:

AVG OUTPUT POLARITY ESTABLISHED

Use only if both X and Y positive/negative relationships are proven from original Atari schematic evidence.

AVG X POLARITY ONLY ESTABLISHED

Use only if X is proven but Y is not.

AVG Y POLARITY ONLY ESTABLISHED

Use only if Y is proven but X is not.

AVG OUTPUT POLARITY GAP REMAINS

Use if the relevant original documentation is obtained but cannot establish either polarity.

ATARI SHEET 3B ACQUISITION GAP

Use if Sheet 03B cannot be obtained.

SCHEMATIC LEGIBILITY GAP

Use if Sheet 03B is obtained but cannot be traced reliably enough.

Do not broaden the classification into final monitor/cabinet orientation.


REPORT

Preserve this substantive prompt under AI-Logs/Prompts.

Create:

AI-Logs/Reports/2026-11-15-tempest-avg-output-polarity-narrow-report.md

Report only:

1. starting Git state;
2. previous report used;
3. external technical documents/pages opened, maximum three;
4. whether Sheet 03B was obtained;
5. exact Sheet 03B title/identifier/source;
6. any immediately connected sheet used;
7. X signal trace;
8. Y signal trace;
9. X polarity result;
10. Y polarity result;
11. connector/pin evidence;
12. facts established;
13. exact unresolved gap;
14. primary classification;
15. whether ONE further narrowly defined technical investigation is warranted;
16. final Git state;
17. confirmation no project/source/test/metadata/reference changes;
18. confirmation nothing staged, committed, or pushed;
19. confirmation no gameplay visuals, Gemini, Incitti, MAME orientation inference, or cabinet-photo inference was used.


FINAL RESPONSE

Report:

1. Whether the narrow investigation completed.
2. Primary classification.
3. Whether Sheet 03B was obtained.
4. Exact external documents opened.
5. X polarity result.
6. Y polarity result.
7. Connector/pin result.
8. Exact remaining gap.
9. Whether another technical investigation is warranted.
10. Report path.
11. Confirmation no project changes.
12. Confirmation nothing staged, committed, or pushed.

Do not implement anything.

TEMPEST_AVG_OUTPUT_POLARITY_NARROW_20261115