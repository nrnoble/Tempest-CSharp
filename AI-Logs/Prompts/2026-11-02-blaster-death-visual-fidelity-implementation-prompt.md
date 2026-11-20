IMPORTANT: Read this entire prompt before taking any action. This prompt is complete only if you can find the exact completion token at the very bottom of the prompt, appearing exactly once.

Before using any tool, reading or modifying any repository file, or running any command:
1. Read the entire prompt.
2. Locate the completion token at the bottom.
3. Verify that it appears exactly once.
4. If you cannot find it exactly once, STOP immediately and report that the prompt is incomplete. Do not perform any repository action.

Repository:
C:\Users\nrnob\OneDrive\Repro\Tempest

Current accepted clean checkpoint:
7ece019 Implement accepted larger game window

TEMP/ is intentionally untracked.
Do not modify, stage, remove, clean, or otherwise disturb TEMP/.

MILESTONE

Implement the Blaster death-animation visual-fidelity refinement.

AUTHORITATIVE ANALYSIS

Before modifying code, read the complete analysis report:

AI-Logs/Reports/2026-11-02-blaster-death-visual-fidelity-analysis-report.md

Treat that report as the immediate engineering analysis for this milestone.

Also inspect the current implementation and tests rather than relying solely on the report.

BACKGROUND

The existing player-death architecture and gameplay semantics are already accepted.

Player-death enemy retraction has already been implemented and manually validated.

The C# project already contains a Blaster death animation. Do NOT replace the player-death system or create an unrelated second death system.

The current visual is substantially simpler than the arcade effect: the analysis found that C# currently renders eight yellow radial lines whose radius expands through the death interval.

This milestone refines that existing visual presentation.

ARCADE VISUAL AUTHORITY

Neal supplied the arcade death sequence as sequential frames:

frame_0041.png through frame_0065.png

The frame sequence establishes the following observable presentation:

- the effect occurs at the Blaster death location;
- it is an angular/vector starburst-like presentation;
- multiple related outlines/layers are visible;
- prominent colors include white, red/orange, and yellow;
- the layers are not simply one unchanged radial shape;
- the effect develops from a relatively compact presentation;
- it becomes substantially larger;
- it then contracts;
- a smaller remnant remains;
- that remnant diminishes until the effect disappears;
- the tube remains visible behind the effect.

Conceptually:

compact/developing effect
    ->
large layered multicolor vector/starburst
    ->
contraction
    ->
small diminishing remnant
    ->
disappearance

IMPORTANT EVIDENCE LIMIT

Do NOT invent unsupported arcade precision.

The supplied evidence does NOT yet establish exact:
- vector coordinates;
- number of vertices;
- number of simultaneous outlines;
- per-frame color schedule;
- mathematical scale curve;
- exact phase boundaries at individual ticks.

Reproduce the clearly observable character of the effect without pretending that unmeasured details are arcade-verified.

AUTHORITY ORDER

1. Supplied Atari arcade frame evidence and established arcade observations.
2. TEMPEST-BEHAVIOR-SPEC.md.
3. TEMPEST-DEVELOPMENT-NOTES.md.
4. Mark Incitti's implementation as an engineering reference.
5. Existing tested C# implementation.
6. New assumptions only where unavoidable.

If Incitti differs visually from the arcade frame evidence, the Atari evidence wins.

IMPLEMENTATION CONSTRAINTS

Preserve the existing player-death architecture where practical.

The analysis established that:
- TempestGame owns the existing 60-tick life/death boundary;
- PlayerDeathPresentation owns ordinary-death presentation timing and enemy retraction;
- VectorRenderer owns the existing Blaster death drawing;
- existing enemy retraction is already independent presentation behavior.

Do NOT change the 60-tick life boundary solely to tune this visual.

Prefer deriving deterministic Blaster-death visual state from the existing death/presentation clock.

The resulting presentation should have a deterministic envelope that provides:

1. initial development;
2. substantial outward expansion;
3. a large layered peak;
4. contraction;
5. a small diminishing remnant;
6. clean disappearance.

The implementation does NOT need six explicit gameplay states. Use the simplest maintainable representation.

VISUAL DESIGN

Replace/refine the current simple eight-yellow-ray presentation so the result more closely resembles the supplied arcade frames.

Prefer:
- angular vector outlines rather than filled graphics;
- several related outline layers;
- white, red/orange, and yellow layering;
- visibly related but non-identical layer sizes and/or orientations;
- deterministic scale evolution;
- stable centering at the Blaster death location;
- a strong large-starburst portion;
- visible contraction after the peak;
- an increasingly small terminal remnant.

Avoid:
- particle systems;
- textures/sprites;
- random nondeterministic explosions;
- screen-clearing flashes;
- replacing the tube view;
- unnecessary general-purpose animation frameworks.

Keep the effect consistent with the project's vector-rendering style.

GAMEPLAY PRESERVATION

This is primarily a presentation milestone.

Do NOT intentionally alter:

- enemy retraction;
- enemy reconstruction;
- enemy identity/accounting;
- Spike persistence;
- Spike reverse/retry behavior;
- score;
- lives;
- Game Over;
- Super Zapper;
- firing;
- spinner movement;
- collisions;
- rim pursuit;
- level progression;
- between-level approach;
- display/window dimensions or scaling.

The animation must not generate gameplay entities or participate in collision/scoring.

SPIKE-DEATH ROUTE

The analysis indicates that Spike reverse-zoom uses the life timer but differs from the ordinary enemy-retraction presentation route.

Preserve existing Spike death/retry semantics.

Ensure the Blaster animation is correct for each existing death route where the current architecture expects it.

Do not force ordinary enemy retraction behavior into Spike death simply to share animation code.

TESTS

Add or refine focused deterministic tests for the death-animation presentation.

Test behavior/state rather than subjective pixel-perfect appearance.

At minimum protect:

1. The effect begins at the expected point in player death.
2. Its origin remains tied to the death location.
3. The visual envelope initially develops/expands.
4. A later point is substantially larger than an early point.
5. The envelope subsequently contracts.
6. A small terminal/remnant stage exists.
7. The presentation terminates cleanly.
8. Required color layers are represented during the appropriate active part of the animation if color is modeled in deterministic presentation state.
9. Ordinary death still preserves the existing 60-tick lifecycle.
10. Life deduction/reconstruction timing is unchanged.
11. Game Over timing is unchanged.
12. Enemy retraction remains intact and independent.
13. Spike death/retry behavior remains intact.
14. Existing player-death tests continue to pass.

Avoid fragile tests that merely duplicate arbitrary renderer coordinates.

DOCUMENTATION

Update only documentation materially affected by the accepted implementation.

Preserve the evidence classifications used by TEMPEST-BEHAVIOR-SPEC.md.

Do not label newly chosen geometry, timing curves, or implementation values as ARCADE VERIFIED merely because they approximate the supplied frames.

The arcade-observed broad visual progression can be documented as arcade evidence.

Implementation-specific interpolation, phase boundaries, geometry, and constants should be identified appropriately as project implementation choices unless directly supported by evidence.

AI LOGS

Following the project's established workflow:

- save this implementation prompt under AI-Logs/Prompts using the established naming convention;
- save an implementation report under AI-Logs/Reports;
- include validation results and any deliberate implementation assumptions in the report.

VALIDATION

Run focused tests first.

Then run the appropriate broader affected tests.

Then run the complete solution test suite.

Then build the solution.

Run git diff --check.

Do not commit.

Do not stage files.

Do not modify TEMP/.

MANUAL VALIDATION REQUIRED

Automated tests do NOT constitute visual acceptance.

After automated validation passes, stop and provide Neal with the command needed to run the normal game for manual testing.

Provide a concise manual checklist specifically comparing the resulting animation to the arcade frame sequence.

The manual checklist must cover:

1. effect remains centered at the Blaster death location;
2. tube remains visible behind the effect;
3. compact/developing beginning;
4. layered white/red-orange/yellow vector appearance;
5. substantial expansion;
6. large starburst-like peak;
7. visible contraction;
8. small diminishing remnant;
9. clean disappearance rather than an abrupt arbitrary cutoff;
10. enemy retraction remains normal and concurrent where applicable;
11. Spikes remain unaffected;
12. reconstruction/life behavior remains normal;
13. no new rendering artifacts, flashes, crashes, or gameplay regressions.

Do not commit until Neal manually accepts the visual result.

FINAL REPORT

Report:
- files changed;
- implementation approach;
- deterministic animation envelope/phases;
- how the existing death clock was reused;
- treatment of ordinary versus Spike death;
- tests added/changed;
- focused-test result;
- full-suite result;
- build result;
- git diff --check result;
- worktree status;
- exact manual-run command;
- manual acceptance checklist;
- implementation-report path.

COMPLETION TOKEN:
TEMPEST_BLASTER_DEATH_FIDELITY_IMPLEMENTATION_COMPLETE