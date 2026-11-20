FAIL-CLOSED COMPLETENESS CHECK

Before using any tools, opening any files, running commands, or taking any repository action, read this entire user-provided task and locate the completion token near the end.

Verify that exactly one complete token beginning with TEMPEST_ appears on a line by itself in the user-provided task. VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context may appear after the token and do not invalidate it.

This check detects an incomplete or truncated user task, not appended agent context.

If exactly one complete TEMPEST_ token cannot be found, STOP before any project or repository action and explain why.

# Tempest: Bounded Atari Presentation Adapter Implementation Candidate

This task authorizes one tightly bounded implementation candidate.

Implement the first render-only Atari presentation adapter described by the controlling plan. Preserve the existing legacy projection, gameplay/model geometry, lane identities, topology, deterministic simulation, accepted portrait candidate, and all pre-existing worktree changes.

Do not commit or push.

Do not launch the interactive game.

Stop after automated validation and provide Neal with manual launch and acceptance instructions.

## Authorized AI-Logs artifacts

Create exactly two new ignored AI-Logs artifacts:

1. Preserved prompt:

AI-Logs/Prompts/2026-11-15-atari-presentation-adapter-implementation-prompt.md

1. Implementation report:

AI-Logs/Reports/2026-11-15-atari-presentation-adapter-implementation-report.md

Do not create other AI-Logs files.

The preserved prompt must retain the substantive task, scope, constraints, validation requirements, manual-acceptance boundary, and completion token while omitting automatically appended agent context.

AI-Logs are ignored working artifacts. Do not stage or commit them.

## Controlling plan

Read completely:

AI-Logs/Reports/2026-11-15-atari-presentation-adapter-implementation-plan-report.md

Treat that report as the controlling engineering plan.

Consult these handoffs only if needed to verify a decisive formula, input, or boundary:

- AI-Logs/Reports/2026-11-15-presentation-layer-atari-transform-feasibility-report.md
- AI-Logs/Reports/2026-11-15-atari-mathbox-c098-reconciliation-report.md
- AI-Logs/Reports/2026-11-15-atari-csharp-projection-comparison-report.md
- AI-Logs/Reports/2026-11-15-atari-csharp-coordinate-frame-mapping-report.md
- AI-Logs/Reports/2026-11-15-level7-geometry-provenance-disposition-report.md

Do not reopen Atari archaeology, Math Box reverse engineering, geometry provenance, or architectural feasibility.

Do not search the Internet.

Do not use third-party implementations.

## Objective

Implement one opt-in, development-only, portrait-only Atari-style presentation candidate that:

- operates only in the rendering/presentation layer;
- is selected at or immediately around VectorRenderer.ProjectWorldPoint;
- supports Levels 1, 7, 8, and 9;
- uses explicit legacy fallback for Levels 2-6;
- preserves the current legacy projection as the unchanged default;
- preserves every stored production point and point order;
- preserves every lane identity and topology decision;
- preserves accepted gameplay and simulation behavior;
- preserves playback determinism;
- preserves shared portrait placement -33.1;
- keeps Game Over vectors on their existing legacy projection path;
- keeps transition stars, HUD, text, and overlays outside the adapter;
- coordinates world-anchor transformation with depth-responsive local glyph scaling;
- stops for Neal’s manual visual and gameplay acceptance before commit or activation.

This is a candidate, not final activation.

## Starting Git and worktree protection

Before substantive work, record:

- branch;
- HEAD;
- upstream;
- tracked modifications;
- untracked paths;
- staged/index state.

The expected baseline is:

- branch master;
- HEAD 385f603a94391756b2ca6cccd524dc17b281e782;
- upstream origin/master;
- an empty index;
- pre-existing tracked modifications from the active portrait-presentation, pause, screenshot, and related candidate;
- pre-existing untracked Atari documents, RomPresentationMetadata.cs, and RomPresentationMetadataTests.cs.

Verify rather than assume.

Protect every pre-existing change.

Do not:

- clean;
- reset;
- restore;
- stash;
- stage unrelated work;
- rewrite existing changes;
- discard untracked files;
- commit;
- push.

If the observed branch, HEAD, or index differs materially from the expected state, stop before implementation and report the discrepancy.

## Non-negotiable gameplay/model boundary

The adapter must not modify or feed back into:

- LevelDefinition.Points;
- point ordering;
- lane numbering;
- PlayableLaneCount;
- Continuous;
- open/closed topology;
- gameplay coordinates;
- entity state;
- entity depth or height;
- enemy routing;
- directed pivots;
- collisions;
- wave definitions;
- timing;
- scoring;
- lives;
- player input;
- recording files;
- playback input;
- deterministic simulation state.

The adapter may derive temporary render-time coordinates and local presentation scales from immutable model inputs.

Level 7 must retain its current Incitti-derived points and order. Never apply the comparison-only +5 cyclic alignment to production geometry, lanes, entities, or rendering.

## First-candidate level boundary

Enable candidate presentation only for:

- Level 1;
- Level 7;
- Level 8;
- Level 9.

For Levels 2-6:

- use the current legacy projection unchanged;
- emit a clear development diagnostic once per level or other equivalently bounded non-spamming notice;
- do not guess a model-frame mapping;
- do not copy another level’s mapping;
- do not fail gameplay merely because the candidate lacks a mapping.

For a supposedly supported candidate level:

- missing, inconsistent, malformed, or non-finite metadata or mapping is an explicit error;
- do not silently fall back;
- do not substitute neighboring-level data.

## Implementation architecture

Follow the controlling plan’s selected architecture.

### New adapter

Add:

src/Tempest.Game/Rendering/AtariPresentationAdapter.cs

The adapter must be stateless with respect to simulation and must not hold references that allow gameplay mutation.

It must own:

- the continuous Atari-style transform;
- validation;
- the supported per-level model-frame mapping table;
- returned logical-screen coordinates;
- returned finite local logical scale or diagonal Jacobian.

Keep model-frame mappings separate from raw ROM metadata. Do not rewrite or relabel ROM metadata to imply that model-frame mappings came directly from the ROM.

### Selection seam

Select the candidate from VectorRenderer.ProjectWorldPoint or the smallest equivalent render-only seam justified by current source.

Do not:

- replace arithmetic inside Projection.Project;
- alter Projection.Project;
- pre-transform or rewrite stored model points;
- place candidate state into mutable global projection configuration;
- broaden this into a projection-strategy framework.

With the candidate disabled, use the exact existing legacy path and values.

### Required inputs

The future adapter should receive only the render/presentation inputs needed by the controlling plan:

- immutable model X and Y;
- existing render depth;
- LevelDefinition.CenterY;
- LevelDefinition.YOffset;
- LevelConfiguration.Position;
- LevelConfiguration.Depth;
- ApproachDepthOffset;
- ApproachVerticalOffset;
- current Projection.K;
- current Projection.ZOffset;
- current displayed level’s RomPresentationMetadata;
- supported model-frame mapping values u and e.

Do not pass gameplay services or mutable entities into the adapter.

### Required outputs

Return:

- finite logical-screen coordinates;
- finite local logical-units-per-model-unit scale, or an equivalent diagonal Jacobian needed by registered glyph rendering.

Do not return a mutated model coordinate.

## Mathematical contract

Implement the mathematical contract from the controlling plan without aesthetic retuning.

Let:

- u be C# model units per Atari raw unit;
- e be exactly +1 or -1;
- v = Definition.YOffset + ApproachVerticalOffset;
- s = metadata.ScaleRaw;
- r = metadata.CameraHeightRaw;
- Y2D = metadata.ScreenYOffsetSigned;
- zNear = Position + ApproachDepthOffset;
- zFar = Position + Depth + ApproachDepthOffset;
- z = inputDepth + ApproachDepthOffset.

Temporary presentation frame:

Xraw = 128 + x / u

Yraw = 128 + e * (y + v) / u

dX = x / u

dY = Yraw - r

Normalized depth:

t = (z - zNear) / (zFar - zNear)

p = 16 + 224 * t

Do not clamp t. Preserve continuous extrapolation outside the interval.

Continuous reciprocal operation:

Q(d,p,s) = 256 * d / (p + s)

Logical-unit conversion:

lambda = u * K * (16 + s) / (256 * (zNear + ZOffset))

Near-origin Y value:

QyOrigin = Q(e * v / u + 128 - r, 16, s)

Host/logical baseline:

B = CenterY + v * K / (zNear + ZOffset) - e * lambda * QyOrigin

Output:

screenX = 400 + lambda * Q(dX,p,s)

screenY = B + e * lambda * (Y2D + Q(dY,p,s))

Local Jacobian:

Jxx = Jyy = lambda * 256 / (u * (p + s))

Jxy = Jyx = 0

Use the returned local scale for world-anchored glyph dimensions that currently depend on legacy depth scaling.

Do not reproduce Atari integer floor/quantization, signed-16 saturation, bit-exact retained Math Box state, or literal DVG-to-host calibration in this candidate.

Do not add tuning constants.

## Supported model-frame mappings

Use exactly these first-candidate mappings:

- Level 1: u = 25 / 14, e = +1
- Level 7: u = 1.678958593, e = +1
- Level 8: u = 25 / 14, e = -1
- Level 9: u = 25 / 14, e = -1

Do not encode Level 7’s comparison-only cyclic alignment.

Use existing ROM presentation metadata for scale, camera-height/Y reference, signed Y2D, record identity, and topology cross-checks.

Do not allow ROM OpenRaw to override current C# topology. It may be validated against topology, but stored C# topology remains authoritative during this candidate.

## Numeric validation

Reject:

- non-finite coordinates;
- non-finite offsets;
- non-finite output;
- non-finite local scale;
- non-positive or non-finite u;
- e values other than +1 or -1;
- non-positive or non-finite K;
- non-finite ZOffset;
- zero or non-finite depth interval;
- zero or non-finite zNear + ZOffset;
- non-finite t;
- non-finite p;
- zero or non-finite p + s.

Do not silently replace invalid values with zero.

A valid negative extrapolated denominator may remain algebraic extrapolation, as planned. Document and test that behavior rather than calling it Atari hardware behavior.

Follow current exception/error conventions where practical.

## Candidate option

Add an explicit development-only command-line option:

--atari-presentation

Requirements:

- absent by default;
- portrait-only;
- does not change release/default presentation;
- rejected with --legacy-presentation;
- rejected whenever --portrait-perspective is explicitly supplied, even if its value is zero;
- permitted with interactive --record-gameplay;
- permitted with interactive --playback-file;
- not serialized into gameplay recordings;
- selected again explicitly when replaying with candidate presentation;
- rejected in headless playback analysis;
- rejected with --input-diagnostic;
- does not alter pause, stepping, input, screenshot, or recording semantics.

Do not remove or modify the existing --portrait-perspective diagnostic when the Atari candidate is absent.

Do not change its permissive finite-number parser policy except where option exclusivity requires validation.

## Renderer integration

Transform all registered world anchors consistently.

### Tube and lane geometry

Use the candidate for:

- near and far tube vertices;
- rails;
- rims;
- lane boundaries;
- Spike start and tip;
- lane-edge interpolation;
- lane-centered entity anchors;
- player edge endpoints;
- entity depth anchors;
- gameplay-computed pivot endpoints.

Preserve index loops, lane associations, interpolations, and topology.

Because the transform is affine in X/Y at fixed depth, existing same-depth model-space interpolation may remain before projection.

### World-anchored glyph scale

Inventory and update only the depth-responsive local scale used around transformed world anchors.

Candidate paths include:

- Flippers;
- Tankers;
- Fuseballs;
- Spikers;
- player shots;
- enemy bullets;
- destruction glyphs;
- Blaster/claw presentation.

Preserve:

- existing vector programs;
- colors;
- animation;
- rotation;
- segment proportions;
- base dimensions;
- accepted player-shot design;
- intentional fixed one- or two-pixel floors;
- Fuseball’s existing separate additive behavior where applicable.

Replace only the old depth-responsive scale factor with the adapter’s local scale/Jacobian.

Do not apply one scalar where current source demonstrates meaningful anisotropic usage. The planned candidate Jacobian is diagonal and isotropic, but preserve interfaces carefully so future separation does not require gameplay changes.

### Screen-space effects

Keep existing screen-space sizing and effect geometry for items intentionally screen-space, including as applicable:

- marker pixels;
- Spike-tip pixel floors;
- popup text sizing;
- player-death burst sizing;
- transition stars;
- HUD and overlays.

Transform only their world anchor when the current path uses a world anchor.

## Approach and transitions

Compose current render-only Approach offsets into the adapter:

- add ApproachDepthOffset to the point and both active tube interval endpoints;
- include ApproachVerticalOffset in v;
- preserve existing deterministic Approach ticks;
- preserve tube/player registration;
- preserve visible spinner-controlled Blaster behavior;
- preserve existing enemy visibility rules;
- keep the starfield on its current independent path;
- preserve transition timing.

Do not modify LevelApproach or TransitionStarfield unless a narrow compile-time integration need is unavoidable. If source inspection contradicts the plan and would require altering their behavior, stop and report rather than broadening scope.

## Game Over

Keep accepted Game Over vectors on the exact legacy projection path.

Their raw vector records are not ordinary LevelDefinition.Points model units.

Do not:

- feed them into the Atari adapter;
- change their conversion;
- change their scale;
- redesign the effect;
- alter Game Over text or timing.

Add a focused test or explicit routing proof that the candidate does not change Game Over projection.

## HUD and text

Keep HUD, score, lives, warnings, overlays, playback text, and other screen-space text outside the adapter.

Where text uses a world anchor, transform the anchor only and preserve existing font size unless the controlling plan specifies otherwise.

Do not begin HUD-layout work.

## Playback, pause, stepping, and screenshots

The candidate is rendering-only.

Preserve:

- recording format;
- playback data;
- deterministic simulation;
- Space pause;
- Right forward stepping;
- Left reverse reconstruction while paused;
- existing accelerated reverse behavior;
- live F10 screenshot request;
- paused-playback screenshot capture;
- screenshot path/naming/format;
- input behavior.

Do not modify PlaybackScreenshotCapture.cs, InputSystem.cs, or recording formats unless a narrow option-composition test reveals an existing parser/wiring requirement. If functional modification would be needed, stop and report before expanding scope.

## Expected implementation surface

The controlling plan proposes:

### New production file

- src/Tempest.Game/Rendering/AtariPresentationAdapter.cs

### Existing production files that may be modified

- src/Tempest.Game/Rendering/VectorRenderer.cs
- src/Tempest.Game/Debug/DebugController.cs
- src/Tempest.Game/Program.cs

### New focused tests

- tests/Tempest.Game.Tests/AtariPresentationAdapterTests.cs
- tests/Tempest.Game.Tests/VectorRendererAtariPresentationTests.cs

### Existing tests that may be modified narrowly

- tests/Tempest.Game.Tests/DebugControllerTests.cs
- tests/Tempest.Game.Tests/DisplayConfigurationTests.cs

Use existing additional rendering, playback, screenshot, transition, shot, bullet, destruction-effect, and level production tests as regression coverage. Modify them only when a direct candidate assertion is necessary.

### Files normally excluded from functional modification

- src/Tempest.Game/Rendering/Projection.cs
- src/Tempest.Game/Rendering/DisplayConfiguration.cs
- src/Tempest.Game/Levels/RomPresentationMetadata.cs
- src/Tempest.Game/Levels/LevelLoader.cs
- src/Tempest.Game/Levels/LevelConfiguration.cs
- src/Tempest.Game/Levels/LevelDefinition.cs
- src/Tempest.Game/Levels/LevelApproach.cs
- src/Tempest.Game/Rendering/TransitionStarfield.cs
- src/Tempest.Game/Debug/PlaybackScreenshotCapture.cs
- src/Tempest.Game/Systems/InputSystem.cs
- gameplay, collision, simulation, and recording-format files
- durable project documentation

If implementation requires a functional change outside the authorized surface, stop with PARTIAL and explain the exact need. Do not silently broaden scope.

## Required automated invariants and tests

Add focused evidence for all applicable invariants:

1. Candidate disabled preserves existing legacy results.
2. Existing portrait diagnostic remains numerically unchanged when candidate is absent.
3. Candidate selection is explicit and development-only.
4. Duplicate, malformed, and incompatible options are rejected.
5. Stored level point arrays remain unchanged after repeated candidate rendering.
6. Point count, order, lane indices, topology, and Level 7 ordering remain unchanged.
7. Adapter does not mutate level, model, entity, or simulation state.
8. Exact ROM metadata retrieval remains correct for Levels 1-9.
9. Supported mappings exist only for Levels 1, 7, 8, and 9.
10. Levels 2-6 take explicit legacy fallback and never borrow another mapping.
11. Missing or invalid metadata for a supported level fails explicitly.
12. Near/mid/far calculations match the plan for Levels 1, 7, 8, and 9.
13. Equal/opposite X displacements remain symmetric where expected.
14. Y-reference and signed Y2D behavior work for positive and negative offsets.
15. Tube endpoints and same-depth interpolated anchors remain registered.
16. Local Jacobian agrees with bounded finite differences.
17. Registered glyph paths use candidate scale without removing intentional floors.
18. Approach offsets compose without changing Approach simulation state.
19. Candidate works with interactive record and playback options without entering recording data.
20. Candidate and explicit --portrait-perspective are rejected together.
21. Candidate and --legacy-presentation are rejected together.
22. Candidate is rejected for headless playback analysis and input diagnostics.
23. Frozen -33.1 placement remains in the later portrait camera stage.
24. Invalid/non-finite arithmetic inputs fail explicitly.
25. Repeated identical inputs are deterministic.
26. Game Over remains on the legacy projection path.
27. No topology or open/closed behavior changes.
28. Legacy behavior for Levels 2-6 is preserved.

Do not use framebuffer screenshot pixels as the primary automated oracle.

## Maintainability requirements

Use focused names and comments that explain:

- the adapter is presentation-only;
- the underlying model geometry is immutable;
- the formula is a continuous translation of recovered Atari arithmetic;
- endpoint normalization is a project translation, not proof of literal Atari depth units;
- Y2D is post-projection and is not the same as C# YOffset;
- Level 7 mapping is approximate and must not rotate lane indices;
- legacy fallback for Levels 2-6 is deliberate and temporary;
- local glyph scale must remain consistent with transformed anchors;
- Game Over’s coordinate frame is intentionally exempt.

Avoid comments that merely restate individual code lines.

Do not perform unrelated cleanup or refactoring.

## Bounded validation

All commands must be bounded.

Potentially long test commands must use explicit VSTest hang-timeout protection or another established project timeout mechanism.

Run validation in this order:

1. New adapter-focused tests.
2. New renderer-integration tests.
3. Focused option/parser tests.
4. Relevant existing rendering/presentation tests, including Approach, transition, shots, bullets, destruction effects, Level 1, Level 7, Level 8, Level 9, playback, and screenshots.
5. Full test suite.
6. Release build of TempestCSharp.slnx, requiring zero warnings and zero errors.
7. git diff --check.
8. Final branch, HEAD, upstream, index, tracked, and untracked status verification.

Do not report a timed-out, canceled, blocked, or partial command as passed.

If a test fails:

- isolate it with progressively smaller bounded runs;
- determine whether it is a candidate defect or pre-existing failure;
- fix only candidate defects within scope;
- do not alter accepted legacy behavior merely to satisfy the candidate.

## Windows security and stalls

If Windows Application Control, Smart App Control, or similar security blocks a generated DLL, EXE, test host, or command:

- do not weaken security;
- do not bypass the block;
- do not enter a same-artifact retry loop;
- do not substitute stale binaries;
- do not claim validation passed.

Retry only through a materially different safe route.

If required validation cannot complete, stop with a clear BLOCKED or PARTIAL report preserving the worktree.

All commands, loops, analyses, and tests must have explicit bounds. If meaningful progress stops beyond the expected duration, stop the operation and isolate the problem rather than waiting indefinitely.

## Manual-acceptance boundary

Do not launch the interactive game.

After automated validation, stop and provide Neal with candidate-enabled and legacy launch commands.

At minimum provide commands for:

- Level 1 candidate and legacy comparison;
- Level 7 candidate;
- Level 8 candidate;
- Level 9 candidate;
- Level 7 recording with candidate;
- playback of that recording with candidate;
- playback of the same recording using legacy presentation.

Use a fresh temporary recording path and do not overwrite an existing recording.

Manual validation must ask Neal to evaluate separately:

- unchanged tube geometry and lane order;
- perspective/depth behavior;
- vertical placement;
- open/closed topology;
- tube/entity registration;
- local glyph and shot scale;
- rim objects;
- below-rim objects;
- Spikes;
- Approach;
- natural Level 7-to-8 transition;
- pause and frame stepping;
- screenshot capture;
- deterministic recorded playback;
- gameplay/mechanics preservation.

Shared portrait placement -33.1 remains frozen during this candidate.

Do not suggest tuning candidate constants during the first manual run.

## Commit and publication boundary

Do not stage implementation files unless an existing project rule absolutely requires staging for a validation command. No current task requires staging.

Do not commit.

Do not push.

Do not update durable documentation before manual acceptance.

Do not make the candidate the default.

Do not delete the legacy path.

A later closeout prompt will handle documentation, final validation, staging, commit, and push only after Neal explicitly accepts the candidate.

## Required outcome classification

At completion select exactly one:

### READY FOR MANUAL ACCEPTANCE

Implementation completed and every required automated validation passed.

### PARTIAL

Implementation is present, but one or more required items remain incomplete or failed.

### BLOCKED

A security, environment, repository-state, or evidence boundary prevents safe completion.

Do not use READY FOR MANUAL ACCEPTANCE unless focused tests, relevant regressions, full suite, Release build, git diff --check, and final Git-state checks all pass.

## Implementation report

Save:

AI-Logs/Reports/2026-11-15-atari-presentation-adapter-implementation-report.md

Include:

### Outcome

State READY FOR MANUAL ACCEPTANCE, PARTIAL, or BLOCKED.

### Starting state

Record branch, HEAD, upstream, tracked changes, untracked paths, and index.

### Work performed

List each production and test file created or modified and why.

### Architecture implemented

Describe candidate selection, adapter boundary, metadata handling, fallback, mathematical contract, local scale, and Game Over exemption.

### Geometry and gameplay protection

State how points, indices, lanes, topology, entities, simulation, playback, and recording remained unchanged.

### Tests added or changed

Map tests to the required invariants.

### Validation

Report every command, timeout/hang protection, result, test count where available, build warning/error count, and git diff --check.

Canceled, blocked, timed-out, hung, or incomplete commands must be labeled accurately.

### Deviations from plan

List any deviation and why.

If no deviations occurred, say so.

### Remaining limitations

Include Levels 2-6 legacy fallback, Level 7 approximate frame mapping, continuous rather than bit-exact Atari arithmetic, Game Over legacy exemption, and pending manual visual acceptance.

### Manual-validation commands

Give exact PowerShell commands for candidate/legacy comparison, recording, and playback.

### Manual-acceptance checklist

List the required visual and gameplay checks.

### Git and repository state

Report ending branch, HEAD, upstream, tracked changes, untracked paths, index, and confirmation that nothing was staged, committed, or pushed.

### Most important implementation result

State the most important completed result.

### Most important remaining uncertainty

State what only Neal’s manual testing can determine.

TEMPEST_ATARI_PRESENTATION_ADAPTER_IMPLEMENTATION_20261115