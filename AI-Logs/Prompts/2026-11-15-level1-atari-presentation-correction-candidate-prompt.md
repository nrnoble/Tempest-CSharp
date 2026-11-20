FAIL-CLOSED COMPLETENESS CHECK

Before using tools, opening files, running commands, or taking repository action, read this entire user-provided task and locate the completion token near the end.

Verify that exactly one complete token beginning with TEMPEST_ appears on a line by itself in the user-provided task. VS Code/agent-generated reminders, tagged-file information, attachment metadata, and other system-generated context may appear after the token and do not invalidate it.

This check detects an incomplete or truncated user task, not appended agent context.

If exactly one complete TEMPEST_ token cannot be found, STOP before any project or repository action and explain why.

# Tempest: Level 1-Only Atari-Presentation Correction Candidate

This task authorizes one tightly bounded Level 1-only experimental correction to the existing opt-in --atari-presentation candidate.

The purpose is not to make Level 1 pixel-identical to MAME.

The current visual objective is:

Make the C# Level 1 presentation substantially more similar in overall tube shape, looking-down-the-tube character, and placement to the arcade/MAME reference, without changing accepted gameplay/model geometry.

MAME is a strong visual reference, not a pixel-perfect specification. Neal notes that even MAME differs somewhat from the actual arcade game.

Do not distort or replace gameplay geometry merely to reproduce the visible MAME outline.

## Authorized AI-Logs artifacts

Create exactly two new ignored AI-Logs artifacts:

1. AI-Logs/Prompts/2026-11-15-level1-atari-presentation-correction-candidate-prompt.md
2. AI-Logs/Reports/2026-11-15-level1-atari-presentation-correction-candidate-report.md

Do not create any other AI-Logs artifact.

Do not stage or commit these files.

## Controlling diagnostic

Read completely:

AI-Logs/Reports/2026-11-15-level1-atari-presentation-failure-diagnostic-report.md

Treat it as the controlling diagnosis.

The established diagnostic conclusions are:

1. Legacy and --atari-presentation use the same immutable Level 1 points, point order, closed topology, Position=50, and Depth=400.
2. The temporary Level 1 model-to-Atari frame conversion itself preserves the point shape at the near rim.
3. At the near rim, candidate and legacy derivatives are equal.
4. The first decisive shape/depth divergence occurs at the candidate far-depth reciprocal mapping.
5. Legacy far/near scale is approximately 0.120879; the candidate produces approximately 0.151515.
6. More importantly, legacy moves the far-rim center substantially downward relative to the near rim, while the candidate collapses that separation and places the far-rim center slightly above the near-rim center.
7. The candidate-specific upward placement is independently caused by applying Level 1 signed ROM Y2D=-192 through the current translation.
8. Frozen portrait placement -33.1 is a later unchanged translation. It worsens clipping of an already-too-high candidate but is not the source of either candidate-specific error.
9. A pure translation cannot repair the wrong near/far relationship.
10. No model/gameplay geometry change is indicated.

Verify source details as necessary, but do not reopen the completed diagnostic.

## Exact visual authorization

You may inspect only these three exact images if visual reference is needed:

C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\Level-1\Level-1 atari-presentation.png

C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\Level-1\Level-1 Paint-modified legacy.png

C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\Level-1\MAME LEVEL-1.png

Do not search for, enumerate, open, inspect, compare, or analyze any other screenshot, image, video, extracted frame, contact sheet, recording, or visual asset anywhere in the repository.

The Paint image uses the correctly shaped legacy C# tube and roughly repositions it. Ignore incidental Paint artifacts.

The MAME image is a visual reference, not a numeric geometry mandate.

Do not derive arbitrary implementation constants by fitting image pixels.

## Implementation objective

Create a Level 1-only experimental correction within the existing --atari-presentation candidate that independently addresses:

### A. Depth/shape failure

Correct the candidate presentation so the far rim recedes and is positioned relative to the near rim in a way consistent with the established good legacy Level 1 looking-down-the-tube relationship.

Preserve the good near-rim behavior.

Do not achieve this by:

- changing Level 1 model points;
- stretching the Level 1 model into an oval;
- reordering points;
- altering lane identities;
- altering topology;
- arbitrary image-space warping;
- fitting to MAME pixel measurements.

### B. Candidate-specific vertical displacement

Prevent the current direct use/translation of Atari signed Y2D from pushing Level 1 upward into clipping.

Treat this independently from the depth correction.

Do not compensate by retuning the frozen portrait placement -33.1.

Do not change Level 1 CenterY or YOffset merely to cancel the candidate.

## Critical design constraint

The previous adapter attempted to translate Atari presentation arithmetic too literally into a C# geometry/presentation pipeline that already has established screen-shape and placement behavior.

For this Level 1 experiment, prefer preserving known-good C# presentation characteristics where Atari-derived terms are not directly transferable.

Do not assume every recovered Atari variable must appear literally in the final C# presentation formula.

An Atari-derived term should survive into the candidate only when its role is compatible with the current C# representation and moves presentation toward the stated objective.

This is a presentation translation, not Math Box emulation.

## Required correction strategy

Before editing, inspect the current Level 1 legacy and candidate calculations narrowly enough to identify the smallest presentation-only correction.

Use the diagnostic’s established facts to construct a correction with these properties:

1. Near-rim Level 1 projection remains equal or effectively equal to the current good legacy near-rim scale.
2. The far-rim scale and center relationship no longer use the diagnosed candidate mapping that collapses/reverses near/far separation.
3. The correction preserves a coherent continuous depth progression between near and far.
4. The correction does not produce discontinuities for entities moving through depth.
5. X and Y anchors remain registered to the same tube presentation.
6. The direct candidate-specific Level 1 Y2D displacement no longer pushes the tube upward.
7. Existing later portrait placement remains unchanged.
8. No new arbitrary aesthetic coefficient is introduced.

The simplest evidence-backed outcome is preferred over preserving adapter terms merely because they came from Atari.

If the evidence shows that retaining the legacy Level 1 depth progression while using only compatible Atari-derived presentation information is the smallest safe candidate, that is permitted.

If a correction cannot be expressed without introducing arbitrary tuning or changing model geometry, STOP with PARTIAL rather than improvising.

## Level boundary

This correction is Level 1 only.

Do not alter candidate behavior for:

- Level 7;
- Level 8;
- Level 9;
- Levels 2-6 legacy fallback.

Do not use another level’s behavior as a substitute.

Do not inspect or manually analyze other level screenshots or videos.

If shared adapter code must change, gate the experimental correction explicitly to displayed Level 1 and prove with tests that the other paths remain numerically unchanged.

## Preserve current model/gameplay state

Do not modify:

- LevelDefinition.Points;
- point order;
- lanes;
- topology;
- PlayableLaneCount;
- Position;
- Depth;
- Level 1 enemy/gameplay configuration;
- collisions;
- movement;
- timing;
- scoring;
- playback state;
- recording format;
- input;
- deterministic simulation.

Gameplay has remained stable and playback works correctly. Protect that state.

## Preserve legacy behavior

With --atari-presentation absent, all existing projection/rendering behavior must remain unchanged.

Do not modify the legacy formulas to make the experimental candidate easier to implement.

Do not change Projection.Project unless absolutely unavoidable. If modifying the legacy projection appears necessary, STOP and report why.

The current --portrait-perspective diagnostic remains separate and incompatible with --atari-presentation.

## Frozen placement

Do not modify:

- shared portrait placement -33.1;
- portrait camera zoom;
- viewport dimensions;
- portrait camera configuration.

The diagnostic established that -33.1 is not the source of the candidate-specific failure.

## Tube and object consistency

The correction must apply coherently throughout Level 1 depth.

Any Level 1 world object currently routed through the Atari candidate must continue to use a presentation transform compatible with the corrected tube.

Do not create one formula for the tube and an unrelated formula for enemies or shots.

Where the implementation uses a returned local scale/Jacobian for depth-responsive world objects, update the Level 1 experimental correction coherently with the corrected depth behavior.

Do not redesign object artwork, shots, enemies, or effects.

## Game Over, HUD, Approach, and transitions

Do not broaden this candidate.

- Game Over remains on its current legacy exemption.
- HUD/text remain unchanged.
- Transition stars remain unchanged.
- Existing Approach simulation remains unchanged.

If the Level 1 correction naturally composes through the existing render-only Approach inputs, preserve that behavior and test it.

Do not redesign Approach.

## Implementation surface

Prefer modifying only the smallest existing candidate surface, normally:

- src/Tempest.Game/Rendering/AtariPresentationAdapter.cs
- narrow candidate integration in src/Tempest.Game/Rendering/VectorRenderer.cs only if required;
- focused adapter/renderer candidate tests.

Do not modify debug/playback/screenshot/input infrastructure unless compilation or a directly affected test proves that a narrow change is required.

Do not modify LevelLoader, LevelDefinition, LevelConfiguration, DisplayConfiguration, Projection, ROM metadata, or gameplay systems for this experiment.

If a required correction expands materially outside the presentation adapter and focused integration/tests, STOP with PARTIAL and explain before broadening scope.

## Required tests

Add or adjust focused tests that prove:

1. Level 1 legacy path is unchanged.
2. Level 1 stored points, order, topology, Position, and Depth remain unchanged.
3. Level 1 candidate near-rim scale remains at the established good near behavior.
4. Level 1 candidate far-rim scale no longer follows the diagnosed bad far-depth mapping.
5. Far-rim center is below the near-rim center for the ordinary Level 1 case.
6. Depth progression between near and far is continuous and finite.
7. Equal/opposite X displacements remain symmetric.
8. Corrected Y handling no longer applies the diagnosed direct Level 1 upward Y2D displacement in the same harmful way.
9. Frozen -33.1 remains later in the portrait camera and unchanged.
10. Level 1 world anchors and returned local scale/Jacobian remain mutually consistent.
11. Repeated identical inputs remain deterministic.
12. Invalid/non-finite inputs still fail explicitly.
13. Candidate outputs for Levels 7, 8, and 9 remain numerically unchanged from the pre-correction candidate.
14. Levels 2-6 still use their unchanged legacy fallback.
15. Game Over remains unchanged on the legacy path.
16. No gameplay/model state is mutated.

Do not create screenshot pixel tests as the primary oracle.

## Validation

All potentially long commands must be bounded with established hang/timeout protection.

Run:

1. Level 1 adapter-focused tests.
2. Candidate regression tests for Levels 7-9.
3. Renderer candidate tests.
4. Relevant existing Level 1, projection/presentation, Approach, playback, and screenshot regressions.
5. Full test suite.
6. Release build with zero warnings and zero errors.
7. git diff --check.
8. Final Git/worktree/index verification.

Do not report blocked, canceled, hung, timed-out, or incomplete commands as passed.

If the existing running game locks normal output files, do not stop it automatically. Use the previously successful isolated-output strategy or another materially different safe route.

Follow the established Windows Application Control and stall self-stop rules.

## Manual validation boundary

Do not launch the interactive game.

After successful automated validation, STOP and provide only the minimal Level 1 manual command needed for the first visual check:

dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 1 --atari-presentation

Do not provide a broad acceptance checklist at this stage.

Do not ask Neal to create a new recording yet.

Do not ask Neal to test Levels 7-9 yet.

The first manual question is simply:

Does the corrected Level 1 candidate move materially closer to the desired overall tube shape, looking-down-the-tube character, and reasonable screen placement without introducing an obvious new problem?

No tuning should occur during that first observation.

## Git/worktree protection

Before substantive work record:

- branch;
- HEAD;
- upstream;
- tracked modifications;
- untracked paths;
- index/staging state.

Preserve all pre-existing work.

Do not:

- clean;
- reset;
- restore;
- stash;
- stage;
- commit;
- push.

Do not update durable documentation.

The candidate remains experimental until Neal manually evaluates it.

## Required outcome

At completion select exactly one:

### READY FOR LEVEL 1 MANUAL CHECK

Level 1 correction completed and all required automated validation passed.

### PARTIAL

The correction is incomplete, validation failed, or broader work would be required.

### BLOCKED

Repository state, environment, security, or another hard boundary prevents safe completion.

Do not use READY FOR LEVEL 1 MANUAL CHECK unless all required automated validation completes successfully.

## Implementation report

Save:

AI-Logs/Reports/2026-11-15-level1-atari-presentation-correction-candidate-report.md

Include:

### Outcome

State READY FOR LEVEL 1 MANUAL CHECK, PARTIAL, or BLOCKED.

### Starting state

Record branch, HEAD, upstream, tracked/untracked state, and index.

### Correction implemented

Describe separately:

- depth/shape correction;
- vertical-placement correction.

Explain why each follows from the controlling diagnostic.

### What remained frozen

Explicitly confirm model points/order/topology, Level 1 configuration, legacy projection, -33.1, portrait camera, gameplay, playback, and recording state.

### Files changed

List every changed/created source or test file and why.

### Tests

Map focused tests to required invariants.

### Validation

Report commands, bounds/timeouts, pass/fail counts, full-suite result, Release warning/error count, git diff --check, and final Git status.

### Regression protection

State whether Levels 7-9 candidate outputs remained unchanged and Levels 2-6 retained legacy fallback.

### Deviations

Report any deviation from this prompt.

### Remaining limitations

Do not overstate arcade fidelity.

### Manual command

Provide only:

dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 1 --atari-presentation

### First manual question

Ask only whether the corrected Level 1 presentation is materially closer to the desired overall tube shape, looking-down-the-tube character, and screen placement without an obvious new problem.

### Git state

Confirm nothing was staged, committed, or pushed.

TEMPEST_LEVEL1_ATARI_PRESENTATION_CORRECTION_CANDIDATE_20261115