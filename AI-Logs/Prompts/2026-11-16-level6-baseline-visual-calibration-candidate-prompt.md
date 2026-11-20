FAIL-CLOSED COMPLETENESS CHECK

Before using tools, opening files, running commands, or taking repository action, read this entire user-provided task and locate the completion token near the end.

Verify exactly one complete token beginning with TEMPEST_ appears on a line by itself. Agent-generated context after the token does not invalidate it.

If exactly one complete token cannot be found, STOP before any project or repository action.

# Tempest: Level 6 Baseline, Visual Calibration, and Bounded Presentation Candidate

This task authorizes one combined Level 6 investigation and implementation milestone for the opt-in --atari-presentation path.

The task must:

1. establish the current Level 6 numerical presentation baseline;
2. compare the current C# output with the authorized MAME reference;
3. derive the smallest defensible render-only correction;
4. implement that correction only if the evidence is sufficient;
5. validate it;
6. stop for Neal's manual acceptance.

Do not guess.

Do not modify gameplay/model geometry.

Do not commit or push.

Do not launch the interactive game.

## Authorized artifacts

Create exactly:

- AI-Logs/Prompts/2026-11-16-level6-baseline-visual-calibration-candidate-prompt.md
- AI-Logs/Reports/2026-11-16-level6-baseline-visual-calibration-candidate-report.md

Do not create other AI-Logs artifacts.

Do not stage or commit them.

## Exact visual authorization

Analyze only these two exact files:

### Current C# Level 6 problem image

C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\Level-6\Level-6 Tube too high.png

This file shows the current C# Level 6 presentation problem.

Its filename is a user description, not a complete engineering diagnosis.

### MAME Level 6 reference

C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\Level-6\Tempest-MAME-frame_8100.png

This file is the practical visual target.

MAME is a strong reference, not a pixel-perfect physical-arcade specification.

These two files are the complete visual authorization.

Do not enumerate the directory.

Do not search for, open, inspect, compare, or analyze any other screenshot, image, video, recording, extracted frame, contact sheet, or visual asset.

Do not modify or generate visual files.

## Accepted Levels 1-5

Treat Levels 1-5 presentation work as manually accepted and frozen.

Do not modify or retune their current presentation.

Level 5 has been rechecked against both MAME and the actual arcade game and is accepted as rendered. The earlier concern about its horizontal margins was withdrawn.

Do not generalize a Level 1-5 numeric scale, placement, or aspect factor to Level 6 merely because the presentations appear similar.

The separate candidate-specific Blaster-exit rendering defect remains outside this task.

## Existing Level 6 gameplay status

Level 6 gameplay has prior manual acceptance.

Preserve:

- current production points;
- point order;
- lane identity;
- closed topology;
- player/enemy movement;
- directed transitions;
- Tanker behavior;
- Spikers;
- Flippers;
- Spikes;
- collisions;
- timing;
- scoring;
- lives;
- deterministic recording/playback.

This task concerns presentation only.

## Controlling source evidence

Read as needed:

- AI-Logs/Reports/2026-11-15-levels2-6-atari-csharp-coordinate-mapping-report.md
- current Level 6 source/configuration
- current AtariPresentationAdapter
- current renderer integration
- accepted Levels 1-5 presentation reports only where needed to protect existing behavior

The earlier mapping report established:

- displayed Level 6 maps to Atari record 05;
- current C# Level 6 uses a reference-derived 16-point closed triangle;
- Level 6 has only partial structural correspondence with Atari record 05;
- the previous coordinate fit requires features and residuals that do not authorize direct use as a renderer mapping;
- comparison alignment, axis operations, and fitted scales are analysis metadata only;
- production points, point order, and lanes must not be changed.

Do not reopen ROM or Math Box analysis.

Do not turn a C#/Atari coordinate-correspondence scale into a presentation gain.

Level 4 already demonstrated that this can create a unit-domain error.

## Objective

Produce one Level 6 render-only presentation candidate that moves the current C# output toward the authorized MAME reference while preserving:

- Level 6 model geometry;
- gameplay;
- depth progression unless evidence proves it defective;
- tube/world-object alignment;
- HUD separation;
- accepted Levels 1-5;
- playback determinism.

The correction may address only presentation characteristics demonstrated by the two authorized images and source calculations.

Do not assume from the current C# filename that placement is the only problem.

Measure first.

## Part A: current Level 6 baseline

Inventory the current Level 6 production definition:

- exact point list;
- point count and order;
- topology and playable lanes;
- CenterY;
- YOffset;
- Position;
- Depth;
- current K and ZOffset;
- current candidate/fallback behavior;
- geometry provenance.

Using all Level 6 points, reconstruct the current C# presentation at near, middle, and far depths.

Report:

- logical X/Y bounds;
- portrait-host X/Y bounds;
- width and height;
- bounding-box center;
- projected model-origin/presentation anchor;
- top/bottom/left/right viewport margins;
- HUD clearance;
- near/far size ratio;
- near/far center progression;
- local X/Y derivatives.

Confirm whether --atari-presentation currently uses legacy fallback for Level 6.

## Part B: measure both authorized images

For each image, identify the actual portrait gameplay viewport before comparing tube dimensions.

Do not compare against the full host window when the tube renders inside a narrower viewport.

Measure the blue outer near-rim tube only:

- left/right/top/bottom edges;
- width and height;
- horizontal and vertical center;
- normalized width and height;
- normalized margins;
- HUD bounds relevant to clearance;
- minimum visible tube-to-HUD separation.

Where measurable, record the far-rim bounds and center, but do not let temporary gameplay objects define them.

Exclude:

- player/Blaster;
- enemies;
- shots;
- transition text;
- playback status text;
- temporary objects.

State raster/antialiasing uncertainty.

## Part C: reconcile coordinate spaces

Trace each image into a common normalized portrait-viewport coordinate system.

For current C# final pixels, compare measured bounds with source-predicted bounds.

If they materially disagree, stop implementation and diagnose the discrepancy.

Do not implement a correction from source math that fails to describe the current final pixels.

For MAME, convert normalized target bounds into:

- desired C# portrait-host bounds;
- desired C# logical bounds;
- desired presentation anchor;
- desired normalized margins.

Distinguish clearly:

- target final normalized extent;
- multiplier relative to legacy Level 6;
- multiplier relative to any existing candidate stage;
- final net presentation gain.

Do not multiply equivalent descriptions twice.

## Part D: presentation center and HUD composition

Apply the established visual composition rule:

- persistent HUD/information occupies an upper visual region;
- the tube occupies the gameplay presentation area beneath it;
- tube geometry may have different extents around its presentation anchor;
- a tall tube may require downward accommodation to preserve HUD clearance;
- this is not a request to implement literal separate viewports.

Determine from the MAME reference whether Level 6:

- uses the common gameplay presentation center;
- uses a bounded downward center exception;
- or cannot be distinguished reliably.

Do not center the Level 6 bounding box merely as a shortcut.

Use the projected model origin or another source-justified presentation anchor.

## Part E: extent and aspect calibration

Compare current C# and MAME normalized Level 6 extents.

Determine whether the smallest supported correction is:

1. translation only;
2. uniform scale plus translation;
3. independent X/Y presentation gains plus translation;
4. another bounded render-only affine presentation operation;
5. insufficient evidence.

Do not assume Level 6 should copy Level 5's factors.

Do not assume the triangle should fill the same width or height as Level 5.

If X and Y target ratios differ materially, preserve that distinction rather than forcing a uniform scale.

If the differences are within measurement uncertainty, do not add unnecessary corrections.

## Part F: depth baseline

Evaluate the current C# depth progression against:

- source calculations;
- current C# image;
- measurable MAME near/far relationship.

Preserve the established C# depth progression unless the evidence demonstrates a concrete mismatch.

Do not introduce literal Atari reciprocal-depth arithmetic, lev_scale, lev_y3d, or signed Y2D merely because those values exist.

Do not tune depth from a crowded or uncertain far-rim measurement.

If depth cannot be judged reliably, leave it unchanged for the first candidate and defer only that manual judgment.

## Part G: candidate specification

If evidence is sufficient, derive one minimum Level 6 candidate specification.

It may include only:

- presentation anchor/translation;
- uniform or independent X/Y extent gains;
- unchanged C# depth progression unless a change is conclusively justified.

For every value, classify provenance as:

- current C# source/baseline;
- final-pixel C# measurement;
- normalized MAME visual calibration;
- project translation.

Do not call MAME-derived values ROM-backed or exact arcade constants.

If evidence is insufficient, select PARTIAL and do not implement.

## Part H: implementation

If and only if Part G yields a defensible candidate, implement it in the render-only --atari-presentation path for displayed Level 6.

Requirements:

- no model-point changes;
- no point reordering;
- no lane remapping;
- no topology changes;
- no gameplay changes;
- no comparison mapping used as screen gain;
- no modification to accepted Levels 1-5;
- no changes to Levels 7-9;
- legacy Level 6 remains unchanged without the option.

Scale projected displacements about the established Level 6 presentation anchor.

Apply any final translation after extent/depth projection so it does not accidentally alter shape or perspective.

## Tube and world-object consistency

Apply the same Level 6 presentation mapping to all relevant world anchors:

- tube vertices;
- rims;
- rails;
- player/Blaster;
- enemies;
- shots;
- Spikes;
- lane-centered objects;
- below-rim objects;
- pivot endpoints.

Provide compatible per-axis local presentation scales where required for tube-oriented local geometry.

Do not automatically distort intentionally screen-oriented artwork.

Do not redesign objects, effects, colors, or animation.

If this requires a broad renderer redesign, stop with PARTIAL.

## HUD, Approach, playback, and Game Over

Preserve:

- HUD/text rendering;
- Approach simulation and timing;
- transition stars;
- Game Over legacy path;
- input;
- pause/stepping;
- screenshots;
- recording format;
- deterministic playback.

The candidate must remain presentation-only.

## Preferred implementation surface

Prefer changes only to:

- src/Tempest.Game/Rendering/AtariPresentationAdapter.cs
- src/Tempest.Game/Rendering/VectorRenderer.cs only if narrowly required
- focused adapter/renderer tests.

Do not modify:

- LevelLoader;
- LevelConfiguration;
- LevelDefinition;
- Projection;
- DisplayConfiguration;
- RomPresentationMetadata;
- gameplay systems;
- recording/playback;
- input;
- HUD;
- Game Over;
- durable documentation.

If broader changes are required, stop with PARTIAL.

## Required tests

If implementation proceeds, prove:

1. candidate is Level 6-only and opt-in;
2. legacy Level 6 is unchanged without the option;
3. stored points/order/lanes/topology remain unchanged;
4. gameplay/model state is not mutated;
5. source-predicted candidate bounds agree with the implemented transform;
6. candidate approaches the normalized MAME bounds within a justified tolerance;
7. horizontal/vertical center and margins match the derived specification;
8. HUD clearance is positive;
9. depth progression remains unchanged unless explicitly justified;
10. near/far relationship remains finite and coherent;
11. tube and world anchors share the mapping;
12. local per-axis scales match finite-difference checks;
13. screen-oriented artwork remains unaffected where intended;
14. accepted Levels 1-5 outputs remain unchanged;
15. Levels 7-9 remain unchanged;
16. Game Over remains legacy;
17. Approach remains deterministic;
18. recording/playback remains presentation-independent;
19. repeated inputs remain deterministic;
20. invalid/non-finite candidate parameters fail explicitly.

Do not use screenshot pixels as the sole automated oracle. The images justify calibration; mathematical tests protect the implementation.

## Validation

Use bounded commands and established hang-timeout protection.

If a running process locks normal outputs, use the safe isolated-output strategy rather than terminating it automatically.

Run:

1. Level 6 candidate-focused tests;
2. renderer/world-anchor tests;
3. accepted Levels 1-5 regressions;
4. Levels 7-9 candidate regressions;
5. relevant Level 6 gameplay, Approach, transition, recording/playback, screenshot, CLI, and display regressions;
6. full test suite;
7. Release build with zero warnings/errors;
8. git diff --check;
9. final Git/worktree/index verification.

Do not report blocked, canceled, timed-out, hung, stalled, or incomplete validation as passed.

## Manual boundary

Do not launch the game.

If implementation and validation succeed, provide only:

dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 6 --atari-presentation

Ask only:

Is the Level 6 candidate appropriately placed beneath the HUD and visually usable enough for normal evaluation of its size, shape, and perspective?

Do not ask Neal to test Level 7.

Do not request another recording or screenshot.

## Git protection

Record starting repository state.

Preserve all current work.

Do not clean, reset, restore, stash, stage, commit, or push.

Do not update durable documentation.

## Required outcome

Choose exactly one:

### READY FOR LEVEL 6 MANUAL CHECK

A defensible Level 6 candidate was derived, implemented, and fully validated.

### PARTIAL

The visual/source evidence did not support a safe candidate, final C# pixels disagreed with source calculations, required renderer changes exceeded scope, or validation remained incomplete.

### BLOCKED

Environment, security, or repository state prevented safe completion.

## Report

Save:

AI-Logs/Reports/2026-11-16-level6-baseline-visual-calibration-candidate-report.md

Include:

- outcome;
- starting repository state;
- exact two images inspected and confirmation no others were inspected;
- Level 6 baseline geometry/configuration;
- current C# final-pixel/source reconciliation;
- normalized MAME measurements;
- center/HUD analysis;
- extent/aspect/depth decisions;
- exact candidate specification and provenance;
- implementation surface;
- bounds and HUD-clearance result;
- gameplay/model protection;
- accepted Levels 1-5 preservation;
- Levels 7-9 preservation;
- tests and validation;
- deviations;
- limitations;
- manual command;
- ending Git state and confirmation nothing was staged, committed, or pushed.

TEMPEST_LEVEL6_BASELINE_VISUAL_CALIBRATION_CANDIDATE_20261116