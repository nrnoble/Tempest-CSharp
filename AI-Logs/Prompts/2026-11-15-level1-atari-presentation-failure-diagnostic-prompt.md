FAIL-CLOSED COMPLETENESS CHECK

Before using tools, opening files, running commands, or taking any repository action, read this entire user-provided task and locate the completion token near the end.

Verify that exactly one complete token beginning with TEMPEST_ appears on a line by itself in the user-provided task. VS Code/agent-generated reminders, tagged-file information, attachment metadata, and other system-generated context may appear after the token and do not invalidate it.

This check detects an incomplete or truncated user task, not appended agent context.

If exactly one complete TEMPEST_ token cannot be found, STOP before any project or repository action and explain why.

# Tempest: Read-Only Level 1 Atari-Presentation Failure Diagnostic

This is a tightly bounded READ-ONLY diagnostic.

Do not modify production code, tests, documentation, configuration, reference material, screenshots, Git state, staging, commits, branches, or any existing project content.

The current --atari-presentation implementation remains an unaccepted candidate.

Do not implement a fix.

Do not tune constants.

Do not launch the game.

## Authorized deliverables

Create exactly two ignored AI-Logs artifacts:

1. Preserved prompt:

AI-Logs/Prompts/2026-11-15-level1-atari-presentation-failure-diagnostic-prompt.md

1. Diagnostic report:

AI-Logs/Reports/2026-11-15-level1-atari-presentation-failure-diagnostic-report.md

Do not create any other file.

Do not stage or commit either artifact.

## Exact visual authorization

Analyze only these three exact files:

1. Actual C# Level 1 --atari-presentation failure:

C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\Level-1\Level-1 atari-presentation.png

1. Paint-modified placement reference made from the correctly shaped legacy C# tube:

C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\Level-1\Level-1 Paint-modified legacy.png

1. MAME Level 1 arcade objective:

C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\Level-1\MAME LEVEL-1.png

These three files are the complete visual authorization for this task.

Do not search for, enumerate, open, inspect, compare, analyze, batch-process, or derive frames from any other screenshot, image, video, recording, contact sheet, or visual asset anywhere in the repository.

Do not inspect image directories beyond opening the three exact paths listed above.

Do not modify or generate visual files.

## Meaning of the three images

Keep these roles separate.

### Actual Atari-presentation image

This is the failure to diagnose.

It shows Level 1 with --atari-presentation. The tube is wrong in at least two independent ways:

- the tube drawing/presentation itself is incorrect, appearing much more circular or broadly radial than expected;
- the tube is positioned too high and is clipped at the top.

Do not reduce the failure to vertical placement alone.

### Paint-modified legacy image

The tube in this image came from a C# screenshot rendered without --atari-presentation.

Neal considers that legacy C# tube drawing correctly shaped.

The image was edited quickly in Microsoft Paint to show an approximate desired placement of that already-correct legacy tube.

Treat this image only as:

- evidence that the legacy tube shape should be preserved;
- a rough placement diagnostic.

Ignore Paint artifacts, incomplete lines, small marks, missing details, rough edges, and other incidental imperfections.

Do not treat this image as:

- an arcade screenshot;
- a pixel-perfect target;
- a coding instruction;
- a request to change a particular center, offset, constant, or camera;
- evidence that the tube geometry should be altered.

### MAME image

This is the arcade visual objective for Level 1.

A perfect pixel match is not required.

The visible near rim appears vertically elongated or oval-like in screen space, but do not infer that the underlying Atari or C# model geometry is literally oval. That appearance may result from projection, axis treatment, screen mapping, or another presentation operation.

## User-observed project state

Treat these as manual observations:

- The legacy C# Level 1 tube is drawn correctly in basic shape.
- The --atari-presentation Level 1 tube is not drawn correctly.
- The candidate is also too high and clipped at the top.
- The candidate’s circular/radial shape problem is more dramatic on some other levels, but those levels are outside this task.
- Gameplay remains stable across the implemented levels.
- Recorded playback files continue to work without problems.

This task investigates Level 1 presentation only.

Do not inspect or analyze Levels 7, 8, or 9.

## Objective

Determine where the --atari-presentation path first changes the correct-looking legacy Level 1 tube into the incorrect circular/radial candidate presentation.

Separately determine where the excessive upward placement and clipping enter the output pipeline.

The diagnostic must distinguish:

1. underlying immutable Level 1 model points;
2. legacy logical projection;
3. Atari-adapter logical projection;
4. tube near/far coordinate extent;
5. X-versus-Y scale and aspect behavior;
6. logical-to-portrait-camera conversion;
7. frozen shared placement -33.1;
8. viewport clipping.

The main question is not “which constant should be tuned?”

The main question is:

Which exact stage and mathematical term causes the candidate to alter the already-correct legacy tube shape, and which exact stage independently causes the high placement and clipping?

## Controlling implementation handoff

Read:

AI-Logs/Reports/2026-11-15-atari-presentation-adapter-implementation-report.md

Use the implementation report as a navigation handoff.

Inspect current source directly for decisive calculations.

Relevant source should normally be limited to:

- src/Tempest.Game/Rendering/AtariPresentationAdapter.cs
- src/Tempest.Game/Rendering/VectorRenderer.cs
- src/Tempest.Game/Rendering/Projection.cs
- src/Tempest.Game/Rendering/DisplayConfiguration.cs
- src/Tempest.Game/Levels/LevelLoader.cs
- src/Tempest.Game/Levels/LevelConfiguration.cs
- src/Tempest.Game/Levels/LevelDefinition.cs
- src/Tempest.Game/Levels/RomPresentationMetadata.cs
- src/Tempest.Game/Program.cs

Consult earlier reports only if a specific established value or formula must be verified.

Do not reopen the broad Atari, Math Box, Level 7, coordinate-provenance, feasibility, or implementation-planning investigations.

Do not search the Internet.

Do not use third-party implementations.

## Starting repository protection

Before substantive inspection, record:

- branch;
- HEAD;
- upstream;
- concise tracked modifications;
- untracked paths;
- index/staging state.

Preserve all current work exactly.

Do not:

- clean;
- reset;
- restore;
- stash;
- stage;
- commit;
- push.

If branch, HEAD, or index differs materially from the implementation report’s ending state, report the discrepancy before continuing.

## Part A: Visual observations

Inspect only the three authorized images.

For each image, record only observations relevant to:

- near-rim bounds;
- far-rim bounds;
- tube width;
- tube height;
- width-to-height relationship;
- near/far size relationship;
- tube center or bounding-box center;
- top, bottom, left, and right margins;
- clipping;
- relationship to the HUD area.

Where image dimensions permit, obtain approximate bounding boxes or ratios. Clearly label estimates as visual/image measurements rather than source-code values.

Do not spend time documenting incidental HUD text, Paint artifacts, enemy positions, or decorative details unless necessary to establish tube placement.

Do not identify a code cause from appearance alone.

## Part B: Confirm immutable Level 1 inputs

Verify that legacy and candidate rendering use the same:

- Level 1 LevelDefinition.Points;
- point order;
- topology;
- near and far depths;
- level position and depth;
- center and offset inputs before candidate-specific arithmetic;
- Approach state for the captured ordinary gameplay condition, if determinable.

Confirm whether the candidate modifies only presentation calculations or whether any candidate-specific path changes the inputs before projection.

If immutable inputs differ unexpectedly, report that immediately.

Do not modify them.

## Part C: Reconstruct the legacy Level 1 output numerically

Using current source and a small bounded inline calculation, compute the legacy logical-screen coordinates for all Level 1 near-rim and far-rim vertices under the ordinary non-Approach state represented by the screenshot.

Calculate:

- logical near-rim bounding box;
- logical far-rim bounding box;
- logical tube bounding box;
- width and height;
- width-to-height ratio;
- near/far size ratio;
- bounding-box center;
- representative rail endpoints.

Then trace those logical coordinates through the current portrait camera and frozen shared placement -33.1.

Report the resulting host/viewport bounds and whether clipping should occur.

Use actual source constants and state. Do not estimate values from memory.

## Part D: Reconstruct the Atari-candidate Level 1 output numerically

Perform the same bounded calculation using the active --atari-presentation path.

Calculate:

- temporary Atari-like frame coordinates;
- normalized depth values;
- divisor values;
- reciprocal results;
- logical near-rim bounds;
- logical far-rim bounds;
- logical tube bounds;
- width and height;
- width-to-height ratio;
- near/far size ratio;
- logical center;
- output after portrait-camera mapping and frozen -33.1;
- viewport clipping.

Use all 16 Level 1 points at near and far depth.

Do not calculate unrelated entity or glyph paths unless needed to explain the tube boundary itself.

## Part E: Find the first shape divergence

Compare legacy and candidate values stage by stage.

Explicitly test whether the circular/radial appearance first arises from:

- temporary model-to-Atari frame conversion;
- use of one shared u value for both axes;
- Y orientation handling;
- subtraction of lev_y3d;
- use of lev_scale in the reciprocal denominator;
- signed Y2D application;
- lambda;
- host baseline B;
- use of an isotropic local transformation;
- portrait-camera scaling;
- another identified term.

Determine which terms affect:

- tube width;
- tube height;
- near-rim aspect;
- far-rim aspect;
- near/far relationship;
- global vertical placement.

Do not use vague conclusions such as “the perspective is wrong.” Identify the first numerical stage where the output loses the legacy tube’s correct screen-space proportions.

Check specifically whether the candidate is applying recovered Atari coordinate behavior to a C# geometry representation that already contains the needed screen-shape/aspect treatment.

That is a hypothesis to test, not a conclusion to assume.

## Part F: Separate placement from shape

Determine whether a pure post-projection translation of the candidate could correct placement while leaving the wrong shape unchanged.

Determine whether the Paint-modified legacy reference is consistent with:

- unchanged legacy shape;
- a largely global vertical repositioning;
- no major reshaping.

Identify where the candidate’s excessive upward placement originates:

- Atari Y reference;
- signed Y2D;
- derived host baseline;
- existing C# CenterY or YOffset;
- frozen -33.1;
- portrait-camera conversion;
- interaction among these terms.

Do not change or recommend changing -33.1 merely because the final tube is high.

Establish whether -33.1 is:

- the source of the new error;
- exposing an earlier candidate error;
- or simply a later unchanged translation applied to already-wrong logical coordinates.

## Part G: Compare with MAME objective

Use the MAME image only to evaluate the direction of the mismatch.

Determine whether the legacy C# tube, as represented in the Paint placement image, is visually closer to MAME than the Atari-presentation candidate in:

- width-to-height character;
- near-rim containment;
- far-rim placement relative to near rim;
- overall screen centering;
- surrounding margins.

Do not claim pixel equivalence.

Do not derive implementation constants from image measurements.

Do not alter the conclusion merely because the option is named --atari-presentation.

## Part H: Diagnostic conclusion

Select exactly one primary classification:

### CANDIDATE FRAME-CONVERSION ERROR

The candidate’s temporary coordinate-frame conversion removes or replaces a screen-shape/aspect treatment already present in the legacy C# geometry.

### CANDIDATE PROJECTION-MAPPING ERROR

The candidate frame is coherent, but its depth, per-level Atari parameter, baseline, unit conversion, or output mapping produces the wrong shape.

### PORTRAIT-COMPOSITION ERROR

Candidate logical coordinates are reasonable, but the later portrait-camera conversion produces the wrong shape.

### MULTIPLE INDEPENDENT ERRORS

One stage causes the wrong shape and another causes the high placement/clipping.

### IMPLEMENTATION DEFECT

Current code does not match the approved mathematical contract or applies the candidate inconsistently.

### UNRESOLVED

Available evidence cannot identify the first divergence.

Choose only the classification supported by source, calculations, and the three authorized images.

A classification may include secondary contributing factors, but identify the first decisive divergence.

## Decision relevance

For each remaining unknown, answer:

1. Could resolving the unknown change the primary classification?
2. Could it change which code layer would eventually need correction?
3. Could it require changing model/gameplay geometry?
4. Could it materially change the next diagnostic or implementation step?

Do not recommend another investigation for a non-blocking unknown.

## Hard scope boundary

Do NOT:

- edit production code;
- edit tests;
- add tests;
- tune constants;
- change -33.1;
- change CenterY;
- change YOffset;
- change model points;
- change point ordering;
- change lanes;
- change topology;
- modify the adapter;
- modify the portrait camera;
- create prototype code;
- create screenshots;
- edit images;
- launch the game;
- inspect Levels 7-9;
- inspect any visual file other than the three authorized images;
- inspect videos;
- search the Internet;
- stage files;
- commit;
- push.

No build or test suite is required for this read-only diagnostic.

Any calculation must be inline or temporary, deterministic, and explicitly bounded to the Level 1 tube points and projection stages.

## Stop condition

Stop when:

1. The three images have been compared within scope.
2. Legacy Level 1 bounds are reconstructed.
3. Candidate Level 1 bounds are reconstructed.
4. The first shape-divergence stage is identified or declared unresolved.
5. The placement error is separated from the shape error.
6. The role of frozen -33.1 is established.
7. One primary classification is selected.
8. One smallest next step is recommended.

Do not implement the correction.

## Diagnostic report

Save:

AI-Logs/Reports/2026-11-15-level1-atari-presentation-failure-diagnostic-report.md

Include:

### Executive conclusion

State the primary classification and first decisive divergence.

### Authorized visual evidence

List the three exact image paths and their distinct roles.

Confirm that no other visual material was inspected.

### Visual measurements

Record the relevant tube bounds, ratios, margins, and clipping observations.

### Immutable-input verification

State whether legacy and candidate use the same model geometry, point order, topology, and depth interval.

### Legacy pipeline reconstruction

Give formulas, source locations, intermediate values, logical bounds, portrait-camera bounds, and clipping result.

### Candidate pipeline reconstruction

Give formulas, source locations, intermediate values, logical bounds, portrait-camera bounds, and clipping result.

### First shape divergence

Identify the first stage that changes the correct-looking legacy tube proportions.

### Placement diagnosis

Identify where the high placement and clipping originate and state the exact role of -33.1.

### MAME-direction comparison

State which rendering path is closer to the arcade objective in the specifically examined respects.

### Classification

Select and justify one required classification.

### Decision-relevant unknowns

Apply the four-question test.

### Smallest next step

Recommend exactly one bounded next action.

Do not provide a broad redesign or implementation prompt in this report.

### Scope and completion

Report:

- prompt path;
- report path;
- files created;
- source files inspected;
- exact images inspected;
- calculations performed;
- starting and ending Git state;
- confirmation that no tracked file changed;
- confirmation that the index remained unchanged;
- confirmation that nothing was staged, committed, or pushed;
- primary classification;
- first decisive divergence;
- most important remaining unknown, if any.

TEMPEST_LEVEL1_ATARI_PRESENTATION_FAILURE_DIAGNOSTIC_READONLY_20261115