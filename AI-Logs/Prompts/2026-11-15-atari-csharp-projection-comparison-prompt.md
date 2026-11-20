FAIL-CLOSED COMPLETENESS CHECK

Before using any tools, opening any files, running any commands, or taking any repository action, first read this entire user-provided task and locate the completion token near the end.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task. VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context may appear after the token and do not invalidate it.

This check exists to detect an incomplete or truncated user task, not appended agent context.

If exactly one complete TEMPEST_ token cannot be found, STOP before any project or repository action and explain why.

# Tempest: Read-Only State-Parameterized Atari versus C# Projection Comparison

This is a tightly bounded READ-ONLY comparison.

Do not modify production code, test code, project documentation, configuration, Atari reference files, ROM/PROM files, Git state, staging, commits, branches, or any other existing project content.

## Authorized deliverables

Exactly two new ignored AI-Logs artifacts are authorized:

1. Preserved task prompt:

AI-Logs/Prompts/2026-11-15-atari-csharp-projection-comparison-prompt.md

1. Investigation report:

AI-Logs/Reports/2026-11-15-atari-csharp-projection-comparison-report.md

Do not create any other file.

Both files are ignored working artifacts. Do not stage or commit them.

The prompt file must preserve the substantive user task, scope, constraints, validation requirements, and completion token while omitting automatically appended agent context.

## Objective

Perform a state-parameterized, read-only comparison between:

1. the original Atari tube-coordinate transformation established by the recovered ROM, PROM, disassembly, Math Box documentation, and microcode evidence; and
2. the current uncommitted C# tube projection and portrait-presentation implementation.

Determine precisely:

- where the two models are mathematically equivalent;
- where they differ;
- which differences are global;
- which differences arise from Atari’s tube-specific geometry or parameters;
- what the current C# --portrait-perspective correction actually compensates for;
- why one global correction can improve the presentation of one tube while failing to correct other tubes;
- whether the current C# model has enough inputs to reproduce Atari’s per-tube transformation without changing gameplay/model geometry.

This milestone is comparison only.

Do not implement, tune, or recommend specific replacement constants.

## Required evidence

Read these investigation handoffs:

- AI-Logs/Reports/2026-11-15-option2-arcade-projection-investigation-report.md
- AI-Logs/Reports/2026-11-15-atari-projection-data-mathbox-investigation-report.md
- AI-Logs/Reports/2026-11-15-atari-mathbox-c098-reconciliation-report.md

The final reconciliation report is the controlling handoff for the resolved Math Box operation. Use earlier reports for recovered table data and investigation history.

Verify important Atari claims against the original local evidence as needed:

- Reference/arcade/MBUDOC.DOC
- Reference/arcade/MBUCOD.V05.txt
- Reference/arcade/Tempest Rom Source code.md
- the narrowly relevant files in Reference/arcade/roms.rev2/

Inspect the current C# worktree only to identify and compare its projection and portrait-presentation arithmetic.

The current C# implementation is evidence for C# behavior only. It is not evidence for Atari behavior.

Do not search the Internet.

Do not use third-party implementations.

## Established Atari operation to verify and use

The previous investigation concluded that, under normal level setup and with explicit runtime state, Atari computes a divisor from the live position and selected tube scale:

divisor = runtime_position + lev_scale

For an axis magnitude:

quotient = floor((axis_difference * 256) / divisor)

The CPU then restores the axis direction, applies the relevant signed offset, and saturates on signed 16-bit overflow.

The preceding report also established that:

- lev_scale contributes to the divisor;
- lev_y3d establishes the Y reference used to form the Y magnitude;
- the X reference is normally $80;
- lev_y2d/lev_y2db supplies the signed Y offset;
- lev_open affects traversal outside this coordinate calculation;
- tube records have distinct X/Y point sets and multiple distinct parameter tuples.

Treat these statements as handoff conclusions, not unquestionable premises. Verify the specific values and data flow needed for this comparison against the cited local evidence.

Do not reopen the completed full PROM investigation. Inspect original Atari data only where necessary to verify a value used in the comparison.

## State-parameterized comparison requirement

Do not compare the Atari and C# formulas as context-free expressions.

Identify the state required to evaluate each model.

### Atari state

At minimum, account for:

- selected tube record;
- lev_x and lev_y point values;
- lev_scale;
- lev_y3d;
- signed lev_y2d/lev_y2db;
- runtime $57;
- X reference corresponding to $5e;
- active X and Y offsets;
- open/closed traversal state where relevant;
- any unresolved retained-state assumption that materially affects the result.

### C# state

Identify the current variables and constants used by:

- the base projection;
- live depth or equivalent progression state;
- projection denominator;
- X projection;
- Y projection;
- scale;
- portrait placement;
- --portrait-perspective;
- endpoint normalization;
- any tube geometry consumed by the renderer;
- any per-tube presentation inputs currently available.

Use exact code locations and formulas.

Do not infer values that the current C# code does not contain.

## Part A: Establish the C# equations

Trace the current uncommitted C# projection and portrait correction from source input to screen coordinate.

Express the equations or ordered arithmetic for:

1. base projected X;
2. base projected Y;
3. portrait-specific Y correction;
4. scale;
5. shared placement;
6. endpoint normalization;
7. any near/far rim or depth-endpoint handling;
8. tube geometry input.

Identify which terms are:

- global constants;
- runtime state;
- tube-specific data;
- command-line diagnostic inputs;
- renderer/presentation-only corrections.

Do not modify or normalize the code for easier comparison. Describe what it actually does.

## Part B: Put Atari and C# into comparable terms

Create a symbol mapping between the recovered Atari inputs and the closest C# concepts.

For each proposed correspondence, classify it as:

**DIRECT CORRESPONDENCE**

The operations and roles agree sufficiently to treat the terms as counterparts.

**PARTIAL CORRESPONDENCE**

The terms serve related roles but differ in arithmetic, units, range, sign, placement, or application point.

**NO CORRESPONDENCE**

One model contains a term or operation for which the other has no equivalent.

**UNKNOWN**

Available evidence does not establish the relationship.

Do not force Atari and C# into the same terminology when their operations differ.

In particular, determine whether the following correspond:

- Atari $57 + lev_scale and the C# depth denominator;
- Atari axis_difference * 256 and the C# projected coordinate numerator;
- Atari lev_y3d and the C# Y origin/reference;
- Atari signed lev_y2d/lev_y2db and C# vertical placement;
- Atari per-tube lev_scale and any C# global or tube-specific projection parameter;
- Atari post-division offset and the C# portrait-perspective correction;
- Atari open/closed traversal and C# tube topology handling.

## Part C: Numeric state-parameterized comparison

Perform a small bounded numeric comparison.

Use no more than four Atari tube records chosen to represent materially distinct recovered parameter/state cases. The set should include, when possible:

- the tube record used by arcade Level 9;
- at least one enclosed tube from Levels 1-7;
- at least one open tube;
- one additional record only if needed to test a distinct parameter tuple.

First use lev_remap to establish the correct level-to-record mapping. Do not assume the level number equals the record index.

For each selected record:

1. list the Atari record index and recovered per-tube parameters;
2. list a small number of representative point values;
3. use an explicit bounded set of runtime-depth inputs sufficient to compare the shape of the transform;
4. compute Atari outputs under all stated assumptions;
5. evaluate the current C# base projection over corresponding normalized positions where a defensible mapping exists;
6. evaluate the C# portrait correction separately rather than folding it invisibly into the base result;
7. compare denominator behavior, relative scaling, symmetry, vertical reference, and endpoint behavior.

Do not claim pixel-perfect screen-coordinate equivalence unless coordinate systems, units, scaling, and placement have actually been reconciled.

If the two coordinate systems cannot be directly aligned, compare normalized curves, ratios, or relative behavior and state the limitation explicitly.

Any quantitative analysis must use a small deterministic script with explicit input and iteration bounds. Do not create a permanent script file. Use a temporary or inline analysis method and record the formulas and inputs in the report.

Do not generate charts or image files unless a chart is essential to detect a relationship that cannot be stated numerically. Prefer compact numeric results. If no chart is necessary, do not create one.

## Part D: Diagnose the structural mismatch

Based only on the established equations, inputs, and bounded numeric comparison, answer:

1. Does the C# base projection reproduce Atari’s recovered division structure?
2. If broadly similar, where do its numerator, denominator, reference point, offsets, units, or endpoint treatment differ?
3. Does the C# model presently use Atari-equivalent per-tube scale, Y-reference, and Y-offset inputs?
4. Is --portrait-perspective changing the base depth projection, or adding a later presentation correction?
5. Can that correction reproduce Atari’s tube-specific divisor, Y reference, and signed offset behavior?
6. Why can a value near 125 improve the currently observed Level 9 presentation while failing to correct Levels 1-7?

For question 6, distinguish clearly between:

- conclusions demonstrated mathematically by the compared models;
- plausible explanations supported by the structure;
- visual claims that cannot be made because this milestone does not inspect screenshots or frames.

Do not treat manual observations as mathematical proof. Use the observations only as the symptom the comparison is intended to explain.

## Part E: Determine the smallest faithfully bounded next step

Conclude whether the evidence supports one of these outcomes:

### OUTCOME A: MODEL ALREADY EQUIVALENT

The C# base model already matches the Atari transform sufficiently, and the remaining discrepancy lies elsewhere.

### OUTCOME B: PARAMETERIZATION GAP

The C# base model is structurally compatible but lacks or misapplies Atari-equivalent per-tube inputs.

### OUTCOME C: STRUCTURAL MODEL GAP

The C# projection differs materially from the recovered Atari arithmetic and cannot be made equivalent merely by supplying different constants.

### OUTCOME D: MIXED OR UNRESOLVED

Some parts correspond, but remaining uncertainty prevents one of the other conclusions.

Choose only the outcome justified by the evidence.

Recommend the smallest next milestone. Do not implement it.

Do not provide arbitrary constants, aesthetic tuning ranges, or a broad renderer redesign.

## Evidence discipline

Classify major claims as:

**ATARI-BACKED**

Established by original Atari documentation, ROM/PROM data, microcode, or disassembly.

**C#-SOURCE-BACKED**

Established by the current C# worktree.

**NUMERICALLY DEMONSTRATED**

Established by the bounded state-parameterized calculations in this milestone.

**STRONG INFERENCE**

Supported by the compared structures but not directly demonstrated.

**HYPOTHESIS**

Plausible but unproven.

**UNKNOWN**

Not established by available evidence.

Do not silently promote a correspondence or interpretation to fact.

## Hard scope boundary

Do NOT:

- modify C# code;
- modify tests;
- add tests;
- modify project documentation;
- modify Atari reference files;
- modify ROM/PROM files;
- alter the current portrait formula;
- alter shared vertical placement;
- tune --portrait-perspective;
- add per-tube constants;
- change tube geometry;
- change rendering;
- change gameplay;
- generate an experimental implementation;
- launch the game;
- inspect screenshots;
- inspect frames;
- inspect videos;
- inspect unrelated repository images;
- search the Internet;
- use third-party implementations;
- perform broad ROM archaeology;
- stage files;
- commit;
- push.

No build or test suite is required.

## Git and worktree protection

Before substantive investigation, record:

- current branch;
- current commit;
- concise tracked and untracked status;
- index/staging status.

The worktree contains the active uncommitted portrait-presentation candidate and newly added Atari reference files. Preserve all existing work exactly.

Do not clean, reset, restore, stash, stage, commit, or push anything.

At completion, confirm that the investigation changed no tracked file and did not alter the existing index.

## Bounded execution and security

Any calculation, script, search, or command that could run for an extended period must have an explicit bound.

Prefer targeted source inspection and small deterministic calculations.

If an operation stalls or makes no meaningful progress within its expected duration, stop and narrow it.

If Windows Application Control, Smart App Control, or similar security blocks an artifact or command:

- do not weaken security;
- do not bypass it;
- do not repeat the same blocked route;
- do not use stale output;
- do not claim success.

Use a materially different safe route if available. Otherwise stop with a clear BLOCKED or PARTIAL result.

## Stop conditions

Stop when all of these have been completed:

1. Current C# equations and inputs are established.
2. Atari and C# terms are mapped without forced equivalence.
3. The bounded state-parameterized comparison is complete.
4. The structural mismatch is classified.
5. The smallest justified next milestone is identified.

Do not expand into implementation.

An UNKNOWN, PARTIAL, or mixed result is acceptable.

## Report requirements

Save the report to:

AI-Logs/Reports/2026-11-15-atari-csharp-projection-comparison-report.md

The report must include:

### Executive conclusion

State the selected outcome: A, B, C, or D.

### Evidence and assumptions

List the Atari and C# sources used, unresolved Atari state assumptions, and any normalization necessary for comparison.

### Current C# model

Give exact source locations, input categories, and equations.

### Recovered Atari model

Restate only the verified operation and tube-specific inputs needed for this comparison.

### Correspondence table

Classify each mapped term as direct, partial, none, or unknown.

### Selected tube records and level mapping

Identify the bounded sample and justify it from recovered parameters.

### Numeric comparison

Record exact formulas, inputs, outputs, ratios, and limitations.

### Structural diagnosis

Answer all six required diagnostic questions.

### Outcome classification

Explain why the evidence supports A, B, C, or D and excludes the other outcomes.

### Implications for the accepted portrait candidate

State what the findings mean for:

- the validated formula;
- shared placement -33.1;
- Level 9 calibration near 125;
- Levels 1-7;
- Level 8;
- gameplay/model geometry.

Do not alter or invalidate accepted behavior without direct evidence.

### Smallest next milestone

Recommend one bounded next step only.

### Evidence classifications

Separate Atari-backed, C#-source-backed, numerically demonstrated, inferred, hypothetical, and unknown claims.

### Scope and completion

Report:

- prompt path;
- report path;
- files created;
- source files inspected;
- Atari data or ROM ranges rechecked, if any;
- calculations or temporary scripts used;
- starting and ending Git state;
- confirmation that no tracked files were modified;
- confirmation that the existing index was not altered;
- confirmation that nothing was staged, committed, or pushed;
- most important demonstrated equivalence;
- most important demonstrated mismatch;
- most important remaining unknown.

TEMPEST_ATARI_CSHARP_PROJECTION_COMPARISON_READONLY_20261115