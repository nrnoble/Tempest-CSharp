FAIL-CLOSED COMPLETENESS CHECK

Before using tools, opening files, running commands, or taking repository action, read this entire user-provided task and locate the completion token near the end.

Verify that exactly one complete token beginning with TEMPEST_ appears on a line by itself in the user-provided task. VS Code/agent-generated reminders, tagged-file information, attachment metadata, and other system-generated context may appear after the token and do not invalidate it.

If exactly one complete TEMPEST_ token cannot be found, STOP before any project or repository action and explain why.

# Tempest: Read-Only Levels 2-6 Atari-to-C# Coordinate Mapping

This is a tightly bounded READ-ONLY investigation.

Do not modify production code, tests, documentation, configuration, reference material, visual assets, Git state, staging, commits, branches, or any existing project content.

Do not implement presentation support for Levels 2-6 during this task.

## Authorized artifacts

Create exactly two ignored AI-Logs artifacts:

1. AI-Logs/Prompts/2026-11-15-levels2-6-atari-csharp-coordinate-mapping-prompt.md
2. AI-Logs/Reports/2026-11-15-levels2-6-atari-csharp-coordinate-mapping-report.md

Do not create any other file.

Do not stage or commit either artifact.

## Accepted Level 1 checkpoint

Preserve this newly established manual result:

- Level 1 tube shape looks excellent.
- Level 1 tube placement looks excellent.
- Level 1 no longer clips.
- The stable upper visual area is appropriate for eventual persistent game information.
- Gameplay remains stable.
- Recorded gameplay playback works without error.

Do not reopen or modify the accepted Level 1 tube shape, depth progression, or placement.

A separate --atari-presentation Blaster-exit drawing defect exists, but it does not affect gameplay and is outside this task.

## Objective

Establish the C#-to-Atari model-frame mappings needed to support Levels 2, 3, 4, 5, and 6 in the render-only --atari-presentation candidate.

For each level, determine whether the existing immutable C# production geometry can be related to the corresponding Atari tube record through a simple presentation-frame conversion while preserving:

- stored points;
- point order;
- lane identities;
- open/closed topology;
- gameplay/model geometry;
- accepted mechanics;
- playback determinism.

The task must distinguish exact mappings, approximate mappings, structural correspondences, and genuine geometry mismatches.

Do not design or implement the final projection correction in this milestone.

## Controlling handoffs

Read only as needed:

- AI-Logs/Reports/2026-11-15-atari-projection-data-mathbox-investigation-report.md
- AI-Logs/Reports/2026-11-15-atari-csharp-coordinate-frame-mapping-report.md
- AI-Logs/Reports/2026-11-15-level7-geometry-provenance-disposition-report.md
- AI-Logs/Reports/2026-11-15-atari-presentation-adapter-implementation-plan-report.md
- AI-Logs/Reports/2026-11-15-level1-atari-presentation-correction-candidate-report.md
- AI-Logs/Reports/2026-11-15-level1-placement-only-correction-report.md

Use the earlier mapping methodology as a handoff, but verify Levels 2-6 against current source and recovered Atari data.

Do not reopen Math Box analysis.

Do not search the Internet.

Do not use third-party implementations.

## Visual-access prohibition

No image or frame analysis is authorized.

Do not search for, enumerate, open, inspect, compare, or analyze any screenshot, image, video, recording, extracted frame, contact sheet, or visual asset.

This task is numerical and source-based only.

## Required level-to-record verification

Using lev_remap, verify the Atari tube record for each displayed level:

- Level 2
- Level 3
- Level 4
- Level 5
- Level 6

Do not assume displayed level number equals record index.

Record the verified mapping and the corresponding recovered:

- lev_x;
- lev_y;
- lev_scale;
- lev_y3d;
- signed lev_y2d/lev_y2db;
- lev_open.

Use the recovered tables already documented in the existing reports unless a narrow source verification is necessary.

## Part A: inventory current C# geometries

For each of Levels 2-6, identify:

- exact LevelDefinition.Points;
- point count;
- point order;
- Continuous;
- playable lane count;
- CenterY;
- YOffset;
- any construction or conversion formula;
- whether the geometry is project-authored, ROM-derived, reference-derived, or undocumented;
- source location;
- gameplay systems that depend materially on its point order or coordinates.

Do not infer provenance from visual or numerical similarity alone.

## Part B: establish corresponding Atari rows

For each mapped Atari record, list all stored point pairs and relevant presentation parameters.

Keep raw Atari coordinate data separate from projected coordinates.

Identify:

- X reference;
- Y reference;
- point order;
- topology;
- record-specific parameters.

Do not apply projection while testing the underlying coordinate-frame relationship.

## Part C: bounded mapping search

For each level, test whether one deterministic frame mapping can relate the Atari point row to the current C# points.

Test only:

1. X/Y translation or centering.
2. X reflection.
3. Y reflection.
4. Uniform scale.
5. Independent X/Y scale only if uniform scale fails.
6. Cyclic point-index rotation.
7. Point-index reversal.
8. Duplicate endpoint or omitted closing-edge convention.
9. A documented source conversion if current code directly supplies one.

Do not introduce:

- nonlinear warping;
- per-point adjustments;
- arbitrary reshaping;
- perspective projection;
- lane remapping;
- geometry replacement.

Use all corresponding points.

## Required per-level classification

Assign exactly one classification:

### EXACT FRAME MAPPING

A consistent frame transformation reproduces all C# points, aside from ordinary floating-point representation.

### CONSISTENT APPROXIMATE FRAME MAPPING

One frame transformation explains the geometry with bounded rounding or documented approximation. Quantify the residual.

### PARTIAL STRUCTURAL CORRESPONDENCE

Topology and general structure correspond, but no single simple mapping reproduces the point set.

### NO CONSISTENT FRAME MAPPING

The immutable C# geometry cannot be related through the permitted operations.

### UNKNOWN

Evidence is insufficient.

Do not call an approximate result exact.

## Required measurements

For each level report:

- displayed level;
- Atari record;
- selected index rotation;
- forward or reversed order;
- X sign;
- Y sign;
- X/Y reference;
- uniform scale, if applicable;
- independent scales, if tested;
- fitted translation;
- maximum absolute X residual;
- maximum absolute Y residual;
- RMS point residual;
- topology correspondence;
- provenance evidence;
- classification.

Use a bounded deterministic inline or temporary calculation.

Do not create a script file, chart, image, or visual artifact.

## Cross-level analysis

Determine whether Levels 2-6 share:

- the same centering convention;
- the same Y orientation;
- the same unit scale;
- the same index direction;
- a consistent starting-index convention;
- consistent topology representation;
- a common provenance pattern.

Compare these findings with the already established mappings:

- Level 1: approximately u=25/14, e=+1, same order.
- Level 7: approximate structural fit with distinct scale; preserve existing geometry/order.
- Levels 8-9: exact u=25/14, e=-1, same order.

Do not alter the accepted mappings.

## Presentation-support decision

For each Level 2-6, select one disposition:

### READY FOR PRESENTATION MAPPING

A defensible model-frame mapping exists and can later be supplied to the render-only adapter without changing model geometry or lane identity.

### READY WITH EXPLICIT APPROXIMATION

A bounded approximate mapping exists; future candidate support is feasible but must be labeled and manually validated.

### REQUIRES SEPARATE GEOMETRY/PROVENANCE REVIEW

The relationship is too uncertain to authorize adapter mapping without further bounded investigation.

### NOT SUITABLE FOR CURRENT ADAPTER

The current immutable geometry cannot be represented coherently by the existing adapter model without a different architecture or model change.

Do not implement any selected mapping.

## Decision-relevant uncertainty

For every unresolved question, answer:

1. Could resolving it change that level's mapping classification?
2. Could it change whether adapter support is safe?
3. Could it require gameplay/model geometry changes?
4. Could it materially change the next implementation milestone?

Do not recommend further analysis for an unknown that cannot change the decision.

## Hard scope boundary

Do NOT:

- modify source;
- modify tests;
- add tests;
- modify geometry;
- modify point order;
- rotate production lanes;
- modify topology;
- modify projection;
- modify Level 1;
- modify the Level 1 placement;
- modify -33.1;
- fix the Blaster-exit defect;
- implement Levels 2-6 adapter support;
- launch the game;
- inspect visual assets;
- run gameplay;
- stage;
- commit;
- push.

No build or test suite is required for this read-only investigation.

## Git/worktree protection

Before substantive work record:

- branch;
- HEAD;
- upstream;
- tracked modifications;
- untracked paths;
- index/staging state.

Preserve every pre-existing change exactly.

Do not clean, reset, restore, stash, stage, commit, or push.

## Stop condition

Stop when:

1. All five level-to-record mappings are verified.
2. All five C# point sets are inventoried.
3. All five Atari rows are compared.
4. Every level has one mapping classification.
5. Every level has one presentation-support disposition.
6. Cross-level conventions are evaluated.
7. Decision-relevant unknowns are identified.
8. Exactly one smallest next milestone is recommended.

## Report

Save:

AI-Logs/Reports/2026-11-15-levels2-6-atari-csharp-coordinate-mapping-report.md

Include:

### Executive conclusion

Summarize the five classifications and support dispositions.

### Evidence and source locations

List only actual sources used.

### Level-to-record verification

Show the verified mapping.

### C# geometry inventory

Record the current geometry and provenance for each level.

### Atari coordinate inventory

Record the relevant rows, references, parameters, and topology.

### Per-level mapping results

For Levels 2-6 individually include formulas, index mapping, residuals, topology, provenance, classification, and disposition.

### Cross-level conventions

State which mappings share a common frame and which do not.

### Relationship to accepted Level 1

Confirm that the accepted Level 1 result is not reopened or altered.

### Decision-relevant unknowns

Apply the four-question test.

### Smallest next milestone

Recommend exactly one bounded next step.

Prefer implementing one newly mapped level at a time unless the evidence demonstrates that multiple levels share an exact common mapping and can be added safely as one tightly bounded group.

### Scope and completion

Report:

- prompt path;
- report path;
- files created;
- source and handoff files inspected;
- calculations performed;
- starting and ending Git state;
- confirmation that no tracked file changed;
- confirmation that the index remained unchanged;
- confirmation that nothing was staged, committed, or pushed;
- mapping classification and disposition for each Level 2-6;
- most important decision-relevant unknown.

TEMPEST_LEVELS2_6_ATARI_CSHARP_COORDINATE_MAPPING_READONLY_20261115