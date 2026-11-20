FAIL-CLOSED COMPLETENESS CHECK

Before using any tools, opening any files, running any commands, or taking any repository action, first read this entire user-provided task and locate the completion token near the end.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task. VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context may appear after the token and do not invalidate it.

This check exists to detect an incomplete or truncated user task, not appended agent context.

If exactly one complete TEMPEST_ token cannot be found, STOP before any project or repository action and explain why.

# Tempest: Read-Only Level 7 Geometry Provenance and Disposition Review

This is a tightly bounded READ-ONLY historical/provenance investigation.

Do not modify production code, test code, project documentation, configuration, Atari reference material, ROM/PROM files, Git state, staging, commits, branches, or any existing project content.

## Authorized deliverables

Create exactly two ignored AI-Logs artifacts:

1. AI-Logs/Prompts/2026-11-15-level7-geometry-provenance-disposition-prompt.md
2. AI-Logs/Reports/2026-11-15-level7-geometry-provenance-disposition-report.md

Do not create any other file.

Do not stage or commit either artifact.

## Objective

Determine the intended project status and provenance of the current production Level 7 clover geometry.

Specifically answer:

1. Was the current Level 7 point geometry intended to be an exact coordinate conversion of Atari tube record 06?
2. Was it intended as an approximation or translation of the Atari clover?
3. Was it deliberately established as independent project geometry based on earlier implementation/reference decisions?
4. What aspects of the current Level 7 geometry received manual gameplay/presentation acceptance?
5. Does any durable project evidence authorize changing the Level 7 geometry now?
6. For future presentation work, must the current Level 7 gameplay/model geometry be treated as frozen, or is there evidence that its point geometry remains provisional?

This milestone does not decide how to project Level 7.

It establishes only the provenance and project disposition of the existing geometry.

## Controlling handoff

Read:

AI-Logs/Reports/2026-11-15-atari-csharp-coordinate-frame-mapping-report.md

Its relevant numerical conclusion is that:

- Level 7 maps to Atari record 06;
- both are closed/continuous topology;
- the current C# point set is project-authored static geometry;
- the best allowed frame fit requires a +5 cyclic point correspondence and a different scale;
- residuals exceed simple rounding;
- therefore the report classified Level 7 as PARTIAL STRUCTURAL CORRESPONDENCE, not an exact or consistent approximate coordinate conversion.

Treat that as the numerical handoff. Do not repeat the full fitting exercise unless necessary to verify a specific claim.

## Evidence authority for this milestone

This investigation is about project history and disposition, not new Atari reverse engineering.

Use, in priority order:

1. TEMPEST-BEHAVIOR-SPEC.md
2. TEMPEST-DEVELOPMENT-NOTES.md
3. .github/copilot-instructions.md only where it establishes durable project authority or acceptance policy
4. current Level 7 production source needed to identify the geometry actually in use
5. existing Level 7-specific AI-Logs reports and preserved prompts only as historical evidence
6. the coordinate-frame-mapping report as the current numerical comparison

AI-Logs historical reports must not silently override later durable documentation or Neal's recorded manual acceptance.

Do not use TEMP/myNotes.md.

Do not use screenshots, frames, video, or visual analysis.

Do not search the Internet.

Do not reopen ROM/Math Box analysis.

Do not inspect unrelated levels except when a durable record directly compares them with Level 7 in a way necessary to interpret the decision.

## Part A: Trace Level 7 geometry history

Establish the history of the current Level 7 clover point set.

Locate the earliest evidence available in the repository/history records for:

- introduction of the Level 7 clover geometry;
- its intended source or engineering reference;
- whether coordinates were copied, converted, approximated, manually constructed, or otherwise derived;
- any later changes to the clover geometry;
- any explicit decision to freeze or preserve it.

Where evidence permits, identify exact filenames, report locations, commits, source locations, or durable-note sections.

Do not infer provenance merely from numerical similarity.

If the historical record never states how the literal point coordinates were obtained, say so.

## Part B: Separate geometry acceptance from gameplay acceptance

Review durable Level 7 acceptance records and distinguish between acceptance of:

- tube shape/topology;
- lane ordering;
- directed rotation behavior;
- Flipper movement;
- Tanker behavior;
- Spiker behavior;
- enemy speeds;
- firing;
- death/re-entry;
- natural level transition;
- overall arcade likeness;
- screen-space presentation or perspective.

Do not convert general statements such as "gameplay looks correct" into specific coordinate acceptance unless the record explicitly supports that conclusion.

Determine exactly what manual acceptance says about the Level 7 clover geometry itself.

## Part C: Determine gameplay/model dependencies

Inspect only the current code necessary to determine whether Level 7 point ordering and geometry participate in gameplay/model behavior.

Identify, without modifying anything:

- whether lane identity depends on point index;
- whether below-rim movement/directed rotation relies on lane ordering;
- whether Tanker cargo lane selection relies on the same topology;
- whether collision/rim behavior uses these points or their lane relationships;
- whether replacing/reordering Level 7 points could therefore affect already accepted gameplay.

This is dependency analysis only.

Do not calculate or propose replacement points.

Do not redesign lane indexing.

## Part D: Reconcile history with the new Atari comparison

Using the existing record-06 comparison, answer:

1. Does the newly demonstrated numerical mismatch contradict any prior explicit project claim that Level 7 was an exact ROM-coordinate conversion?
2. Or does it merely reveal that a previously accepted project clover was never intended to be an exact Atari coordinate conversion?
3. Does the +5 cyclic correspondence represent only a comparison/index alignment, or is there historical evidence that C# lane numbering was intentionally rotated relative to Atari?
4. Are the residual point differences evidence of an acknowledged approximation, or is their provenance simply undocumented?

Do not reinterpret an absence of documentation as proof of intent.

## Required disposition

Select exactly one:

### FROZEN ACCEPTED PROJECT GEOMETRY

The current Level 7 geometry is a deliberately accepted project geometry and must remain unchanged during presentation work unless a future separately authorized gameplay/geometry milestone explicitly reopens it.

### ACCEPTED GAMEPLAY, GEOMETRY PROVENANCE UNCERTAIN

Level 7 gameplay is accepted, but durable evidence does not establish whether the specific point geometry is intentionally independent, approximate, or provisional. Presentation work must preserve it for now because changing it is not authorized.

### PROVISIONAL GEOMETRY

Durable evidence explicitly establishes that the current Level 7 geometry remained provisional or expected to be replaced/refined.

### CONFLICTING EVIDENCE

Durable/historical records materially conflict and no safe disposition can be selected.

Select only the disposition directly supported by evidence.

Do not choose FROZEN ACCEPTED PROJECT GEOMETRY merely because gameplay was accepted.

## Required conclusions

Answer directly:

1. What is the documented provenance of the current Level 7 points?
2. Was exact Atari record-06 coordinate equivalence ever claimed?
3. What Level 7 geometry/topology features were manually accepted?
4. Which gameplay systems depend on the current geometry/order?
5. Does the new numerical mismatch invalidate any prior acceptance?
6. Is Level 7 geometry authorized to change during the current presentation milestone?
7. What disposition should future presentation work assume?

## Evidence classifications

Classify major findings as:

**DURABLE-PROJECT-DECISION**

Explicitly recorded in durable notes/spec/instructions.

**HISTORICAL-REPORT-BACKED**

Recorded in preserved AI-Logs reports/prompts.

**C#-SOURCE-BACKED**

Established by current production source.

**MANUALLY-ACCEPTED**

Explicitly recorded as Neal's manual acceptance.

**NEW-COMPARISON-RESULT**

Established by the completed coordinate-frame investigation.

**STRONG INFERENCE**

Supported by multiple records but not explicit.

**UNKNOWN**

Not established.

Do not silently convert historical interpretation into project fact.

## Hard scope boundary

Do NOT:

- modify Level 7 points;
- modify any C#;
- modify tests;
- modify durable project documentation;
- modify project instructions;
- modify Atari reference files;
- inspect images, screenshots, frames, or videos;
- launch the game;
- run gameplay;
- redo Atari projection investigation;
- redo Math Box investigation;
- calculate replacement Level 7 geometry;
- design a projection adapter;
- tune portrait perspective;
- change shared placement;
- stage files;
- commit;
- push.

No build or test suite is required.

## Git/worktree protection

Before substantive investigation record:

- branch;
- HEAD;
- tracked modifications;
- untracked paths;
- index/staging state.

Preserve the active portrait-presentation worktree exactly.

Do not clean, reset, restore, stash, stage, commit, or push anything.

At completion verify that no existing tracked file or index state changed.

## Bounded execution

All repository/history searches must be tightly targeted to Level 7 geometry, clover, acceptance, record 06, and directly related terminology.

Do not recursively inspect unrelated historical logs.

If a relevant historical report is identified, inspect that report rather than broadening into adjacent reports.

Stop once the disposition is supported or the evidence is demonstrably insufficient/conflicting.

## Report

Save:

AI-Logs/Reports/2026-11-15-level7-geometry-provenance-disposition-report.md

Include:

### Executive conclusion

State the selected disposition immediately.

### Current Level 7 geometry

Identify the production source and topology.

### Provenance history

Present only evidence-supported history.

### Manual acceptance record

Separate geometry/topology acceptance from gameplay/mechanics acceptance.

### Gameplay/model dependencies

Identify what relies on current point/lane ordering.

### Reconciliation with Atari record 06

Explain what the new mapping result does and does not change.

### Required conclusions

Answer all seven questions directly.

### Evidence classifications

Clearly separate durable decisions, historical evidence, current source, manual acceptance, new comparison results, inference, and unknowns.

### Smallest next milestone

Recommend exactly one next step consistent with the selected disposition.

Do not provide implementation instructions.

### Scope and completion

Report:

- prompt path;
- report path;
- files created;
- durable project files inspected;
- Level 7 historical reports inspected;
- source files inspected;
- starting and ending Git state;
- confirmation that no tracked file changed;
- confirmation that the index remained unchanged;
- confirmation that nothing was staged, committed, or pushed;
- selected Level 7 disposition;
- most important remaining unknown, if any.

TEMPEST_LEVEL7_GEOMETRY_PROVENANCE_DISPOSITION_READONLY_20261115