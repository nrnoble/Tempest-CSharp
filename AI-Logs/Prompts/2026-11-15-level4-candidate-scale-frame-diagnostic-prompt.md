FAIL-CLOSED COMPLETENESS CHECK

Before using tools, opening files, running commands, or taking repository action, read this entire user-provided task and locate the completion token near the end.

Verify that exactly one complete token beginning with TEMPEST_ appears on a line by itself in the user-provided task. VS Code/agent-generated reminders, tagged-file information, attachment metadata, and other system-generated context may appear after the token and do not invalidate it.

If exactly one complete TEMPEST_ token cannot be found, STOP before any project or repository action and explain why.

# Tempest: Read-Only Level 4 Candidate Scale/Frame Diagnostic

This is a tightly bounded READ-ONLY diagnostic of the current Level 4 --atari-presentation candidate.

Do not implement a correction.

Do not modify production code, tests, geometry, configuration, documentation, reference material, Git state, staging, commits, branches, or existing project content.

## Manual result

The first manual Level 4 --atari-presentation test did not pass.

Neal reports:

- The current Level 4 candidate is not playable as rendered.
- No Level 4 presentation characteristic should be marked accepted from this candidate.
- Other aspects may potentially be reasonable, but the current failure prevents reliable acceptance judgments.

Do not reinterpret or expand those observations into a prescribed geometric diagnosis.

In particular, do not assume from this task that the eventual correction should:

- increase or decrease width;
- increase or decrease height;
- apply a uniform scale;
- apply an anisotropic scale;
- change placement;
- change perspective;
- change the affine frame;
- change model geometry.

Determine the cause from source and calculations.

## Accepted Levels 1-3

Preserve accepted Levels 1-3 exactly.

Do not reopen or modify their presentation.

The shared candidate-specific Blaster-exit rendering defect remains outside this task.

## Authorized artifacts

Create exactly two ignored AI-Logs artifacts:

1. AI-Logs/Prompts/2026-11-15-level4-candidate-scale-frame-diagnostic-prompt.md
2. AI-Logs/Reports/2026-11-15-level4-candidate-scale-frame-diagnostic-report.md

Do not create any other file.

Do not stage or commit either artifact.

## Visual prohibition

No image, screenshot, video, recording, extracted frame, contact sheet, or other visual analysis is authorized.

Do not search for or enumerate visual assets.

Do not request visual-reference files.

Diagnose the current candidate from source, established handoffs, current/legacy numerical behavior, and bounded calculations.

If numerical/source analysis cannot determine the failure sufficiently for a safe next candidate, say so. Do not guess.

## Controlling handoffs

Read:

AI-Logs/Reports/2026-11-15-level4-geometry-provenance-presentation-mapping-report.md

and:

AI-Logs/Reports/2026-11-15-level4-atari-presentation-candidate-report.md

Use current source as authority for the implementation actually running.

Consult the Levels 2-6 mapping report only when necessary to verify a specific Level 4 comparison value.

Do not reopen broad provenance, ROM, Math Box, or cross-level investigations.

Do not search the Internet.

## Objective

Determine why the implemented Level 4 candidate fails manual usability despite passing automated validation.

Specifically compare:

1. unchanged model-space Level 4 geometry;
2. legacy Level 4 presentation;
3. the pre-implementation affine-frame proposal;
4. the implemented Level 4 candidate;
5. the resulting logical and portrait-host coordinate extents.

Identify the first stage at which the candidate diverges materially from the legacy presentation and determine whether that divergence is an intended consequence of the affine-frame mapping or an implementation/translation error.

Do not make visual-fidelity judgments.

## Part A: verify implementation against the approved plan

Trace the implemented Level 4 path.

Verify precisely:

- how aX=2.081778520 is used;
- how aY=2.231150659 is used;
- how X reference 128 is used;
- how Y reference 121.501111 is used;
- where model coordinates are converted;
- where depth scaling is applied;
- how LocalScaleX and LocalScaleY are calculated;
- how Level 4 CenterY=380 and YOffset=-65 are incorporated;
- which frame/reference terms mathematically cancel and which do not;
- how portrait camera conversion occurs afterward.

Determine whether the implemented equations actually represent the mapping intended by the controlling investigation.

If not, classify the discrepancy explicitly as an implementation defect.

## Part B: reconstruct legacy Level 4 numerically

Using all Level 4 points and current production values, calculate legacy presentation at:

- near depth;
- middle depth;
- far depth.

Report:

- logical X bounds;
- logical Y bounds;
- logical width and height;
- logical center;
- portrait-host X/Y bounds;
- portrait-host width and height;
- portrait-host center;
- top/bottom/left/right margins;
- near/far size relationship;
- near/far center progression;
- local X/Y derivatives.

This is the numerical baseline.

Do not treat legacy as automatically arcade-correct.

## Part C: reconstruct current candidate numerically

Perform the same calculation for the implemented Level 4 candidate.

For near/middle/far report the same measurements.

Then calculate candidate-to-legacy ratios for:

- width;
- height;
- local X derivative;
- local Y derivative;
- near/far size ratio;
- center position.

Determine whether the candidate's overall screen extent difference is primarily generated by:

- dividing by aX/aY;
- reference conversion;
- depth scaling;
- center/baseline calculation;
- local-to-host conversion;
- another identified term.

Do not label the result aesthetically.

## Part D: investigate potential double conversion

Explicitly test whether the candidate applies a C#↔Atari frame conversion to model coordinates that are already in final C# model units, thereby applying a comparison mapping as an additional presentation shrink/expansion when the fit was intended only to describe coordinate correspondence.

This is a hypothesis to test, not a conclusion.

Ask:

- Was aX/aY intended as a relationship for interpreting C# points relative to Atari points?
- Did implementation divide existing C# model displacements by those factors and then project them in a domain where the old C# projection already expected C# model units?
- If so, is there a compensating conversion back into C# logical-screen units?
- If not, what net screen-space gain/loss does the affine frame introduce?

Trace units explicitly.

Do not resolve this by intuition.

## Part E: compare mapping intent with presentation use

Separate:

### Coordinate correspondence

A mathematical mapping used to compare C# geometry with Atari source geometry.

### Presentation transformation

A mapping that should actually modify final C# screen coordinates.

Determine whether the investigation established that the correspondence mapping itself should become a literal presentation scale, or only that it proved a compatible temporary coordinate frame exists.

If the previous implementation prompt promoted a comparison scale into a presentation scale without sufficient justification, state that explicitly.

Do not defend an implementation solely because it followed the prior prompt.

## Part F: preserve what is not implicated

Determine whether any evidence requires changing:

- Level 4 stored points;
- point order;
- lanes;
- topology;
- existing C# depth progression;
- CenterY;
- YOffset;
- portrait placement -33.1;
- portrait zoom;
- gameplay;
- playback.

If not implicated by the diagnosed failure, explicitly protect those items from the next candidate.

Do not infer a placement correction merely from an overall size discrepancy.

## Required classification

Select exactly one primary classification:

### AFFINE FRAME APPLIED IN WRONG UNIT DOMAIN

The mapping is useful for correspondence but was applied directly as screen/model scaling without the necessary unit restoration.

### IMPLEMENTATION DEFECT

The code does not correctly implement the intended affine mapping.

### APPROVED MAPPING PRODUCES THE FAILURE

The code correctly implements the approved mapping, but that mapping itself is unsuitable as a C# presentation operation.

### DEPTH/PROJECTION INTERACTION

The affine frame is coherent, but interaction with the current depth/projection pipeline causes the failure.

### MULTIPLE CONTRIBUTING ERRORS

More than one independent stage materially causes the failure.

### UNRESOLVED

Available evidence cannot identify the decisive stage.

## Candidate correction derivation

If the diagnosis establishes a uniquely justified correction form, derive it mathematically but do not implement it.

The derivation must answer:

- which scale/reference terms remain necessary;
- which terms should be correspondence-only rather than presentation gains;
- what units enter and leave each stage;
- how legacy C# logical-screen scale is retained or intentionally altered;
- how independent-axis information can affect shape without unintentionally changing overall extent;
- whether a normalization is required;
- whether placement remains unchanged.

Do not select aesthetic constants.

Do not infer target dimensions from unavailable visual references.

If the correction magnitude cannot be derived without visual evidence, identify exactly what bounded visual measurement would be needed next.

## Decision-relevant uncertainty

For every unresolved item answer:

1. Could it change the primary classification?
2. Could it change the correction architecture?
3. Could it require model/gameplay geometry changes?
4. Could it require placement or perspective changes?
5. Could it materially change the next implementation candidate?

Do not recommend further investigation for non-blocking uncertainty.

## Hard scope boundary

Do NOT:

- edit production code;
- edit tests;
- add tests;
- modify Level 4 geometry;
- modify lane ordering;
- change placement;
- tune perspective;
- change Levels 1-3;
- work on Levels 5-9;
- fix the Blaster-exit defect;
- inspect visual assets;
- launch the game;
- search the Internet;
- stage;
- commit;
- push.

No build or full test suite is required.

Calculations must be bounded and temporary/inline.

## Git/worktree protection

Before substantive work record:

- branch;
- HEAD;
- upstream;
- tracked modifications;
- untracked paths;
- index/staging state.

Preserve current work exactly.

Do not clean, reset, restore, stash, stage, commit, or push.

## Stop condition

Stop when:

1. Implementation is traced.
2. Legacy Level 4 bounds are reconstructed.
3. Candidate Level 4 bounds are reconstructed.
4. Candidate/legacy ratios are calculated.
5. Units through the affine frame are traced.
6. Correspondence mapping is distinguished from presentation scaling.
7. One primary classification is selected.
8. A correction form is derived if evidence permits.
9. One smallest next milestone is recommended.

## Report

Save:

AI-Logs/Reports/2026-11-15-level4-candidate-scale-frame-diagnostic-report.md

Include:

### Executive conclusion

State the classification and first decisive divergence.

### Implementation trace

Give exact source equations and units.

### Legacy reconstruction

Record near/mid/far bounds, centers, margins, derivatives, and ratios.

### Candidate reconstruction

Record the same data plus candidate-to-legacy ratios.

### Unit-domain analysis

Trace aX/aY, references, depth scale, and logical-screen conversion.

### Correspondence versus presentation

State whether the mapping was validly promoted into a render transform.

### Protected state

List what the diagnosis does not justify changing.

### Correction derivation

State the mathematically justified correction form, if established.

### Decision-relevant unknowns

Apply the five-question test.

### Smallest next milestone

Recommend exactly one bounded next action.

### Scope and completion

Report prompt/report paths, files created, source/handoffs inspected, calculations, starting/ending Git state, and confirmation that no tracked file or index state changed and nothing was staged, committed, or pushed.

TEMPEST_LEVEL4_CANDIDATE_SCALE_FRAME_DIAGNOSTIC_READONLY_20261115