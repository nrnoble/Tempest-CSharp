IMPORTANT COMPLETENESS CHECK

Read this entire user-provided task before taking any action.

Before using tools, reading project files, running commands, modifying files, or performing repository actions, locate the unique completion token near the end of this task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code or agent-generated reminders, tagged-file information, attachment metadata, or other automatically appended context may appear after the user's completion token and do not invalidate it.

This check exists to detect an incomplete or truncated user task, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before taking any project or repository action and explain why.


TASK

Perform a tightly bounded READ-ONLY diagnostic continuation of the current PARTIAL whole-game portrait-presentation reconstruction candidate.

The candidate must remain exactly as it is throughout this task.

Do not implement a correction.

The purpose of this diagnostic is to determine why the same shared portrait presentation transform fits Level 9 but places portions of Levels 1, 3, 5, 6, 7, and 8 above the experimental viewport.

Determine whether the evidence supports one safe whole-game correction model that preserves gameplay geometry and avoids per-level presentation patches.


CURRENT AUTHORITATIVE STATE

The accepted repository baseline remains:

branch:
master

HEAD:
385f603a94391756b2ca6cccd524dc17b281e782

commit:
Close out ROM-backed enemy-shot rendering

origin/master should remain at the same commit.

The worktree now intentionally contains four tracked modified files from the PARTIAL reconstruction candidate:

- src/Tempest.Game/Program.cs
- src/Tempest.Game/Rendering/DisplayConfiguration.cs
- src/Tempest.Game/Rendering/VectorRenderer.cs
- tests/Tempest.Game.Tests/DisplayConfigurationTests.cs

These four modified files are intentional experimental work.

Do not discard, restore, reset, rewrite, clean, stage, commit, or push them.

The index is expected to remain clean.


AUTHORITATIVE REPORTS

Read:

AI-Logs/Reports/2026-11-12-whole-game-presentation-level9-readonly-investigation-report.md

AI-Logs/Reports/2026-11-12-whole-game-presentation-level9-label-correction-report.md

AI-Logs/Reports/2026-11-13-whole-game-presentation-experiment-reconstruction-report.md

For image-dependent findings, the label-correction report supersedes the original 2026-11-12 investigation report.

The reconstruction report is authoritative for the current candidate and its automated failure.

The current candidate reportedly uses:

- 1280 x 960 host window;
- preserved legacy 800 x 600 projection/game-coordinate space;
- experimental 600 x 800 logical portrait presentation;
- centered 720 x 960 viewport;
- approximately 280-pixel side margins;
- 1.2 output scale;
- 1.05 shared playfield scale;
- initial vertical presentation translation of -78;
- unchanged K=70;
- unchanged ZOffset=5;
- one shared playfield camera;
- separate HUD/screen-space camera and layout;
- --legacy-presentation for the established presentation.

Treat these as experimental project translations, not arcade or ROM specifications.


STARTING SAFETY CHECK

Before analysis, run bounded read-only checks of:

- current branch;
- HEAD;
- origin/master;
- staged changes;
- tracked worktree changes;
- non-ignored untracked files;
- diff summary.

Expected:

- branch master;
- HEAD and origin/master at 385f603a94391756b2ca6cccd524dc17b281e782;
- index clean;
- exactly the four intentional tracked candidate files modified;
- no relevant non-ignored untracked files.

If the repository differs materially from that state, STOP before analysis and report the discrepancy.

Do not modify anything to force the expected state.


STRICT READ-ONLY SCOPE

Do not modify any source, test, project, configuration, documentation, prompt, or report file during the diagnostic analysis.

The only files this task may create are:

- the preserved diagnostic prompt under AI-Logs/Prompts;
- one new ignored diagnostic report under AI-Logs/Reports.

Do not change the four candidate files.

Do not:

- implement a correction;
- tune constants;
- change -78;
- change 1.05;
- change viewport dimensions;
- change K;
- change ZOffset;
- alter HUD placement;
- alter tests;
- update snapshots or expected values;
- change level definitions;
- change model points;
- add per-level offsets;
- add level-specific presentation branches;
- stage;
- commit;
- push;
- launch the interactive game.


NO VISUAL-ASSET ANALYSIS

Do not search for, enumerate, open, inspect, extract, generate, or analyze any image, screenshot, video, gameplay recording, MAME frame, C# frame, contact sheet, or other visual asset.

Do not perform broad searches for image or video extensions.

This diagnostic must use source, current diffs, reports, and bounded numerical calculations only.


PRIMARY DIAGNOSTIC QUESTION

Answer:

Why does the same shared portrait presentation transform fit Level 9 while putting portions of Levels 1, 3, 5, 6, 7, and 8 above the portrait viewport?

Determine whether the cause is primarily:

1. an unsuitable global vertical translation;
2. an unsuitable shared transform anchor;
3. varying pre-existing model/projection bounds among tube geometries;
4. current level-specific presentation/projection behavior that predates the candidate;
5. a mismatch between how the tube and gameplay entities are projected or transformed;
6. an incorrect test assumption;
7. or another clearly demonstrated cause.

Do not guess.

Trace the evidence through the actual source and bounded calculations.


BOUNDS ANALYSIS FOR LEVELS 1-9

For every production Level 1 through Level 9, calculate and report in a compact matrix:

- model-space X/Y bounds of LevelDefinition.Points;
- relevant level YOffset or equivalent existing placement input;
- projected tube-point bounds under the unchanged legacy projection;
- legacy screen-space bounds;
- experimental internal/playfield bounds before output scaling;
- final experimental viewport/screen-space bounds;
- minimum and maximum X;
- minimum and maximum Y;
- amount, if any, extending beyond each viewport edge;
- whether orientation remains preserved;
- whether winding/order remains preserved where applicable.

Use existing production level definitions and the current candidate transformation.

Do not alter production state.

If a small temporary external script or bounded shell calculation is needed, place it outside tracked project files and remove it before completion.

Prefer existing code/test-accessible calculations where possible.

Do not add diagnostic code to production or test files.


TUBE AND GAMEPLAY-ENTITY ALIGNMENT

Investigate the architectural concern that tube drawing and positioning of tube-relative gameplay entities may follow separate projection/rendering paths.

Trace, without modifying code:

- how tube vertices become screen coordinates;
- how the Blaster becomes screen coordinates;
- how enemies become screen coordinates;
- how Spikers and Spikes become screen coordinates;
- how player and enemy shots become screen coordinates;
- how gameplay-space effects become screen coordinates;
- where the shared experimental camera is applied to each.

Determine whether every tube-relative element reaches the same final presentation transform.

State explicitly whether the current candidate can produce this failure:

- tube lines move to the portrait presentation;
- gameplay entities continue following an older screen mapping;
- entities therefore appear to cross visible lane boundaries even though simulation state remains correct.

If that failure is structurally prevented by the current shared playfield camera, explain the evidence.

If any rendering path bypasses or duplicates the common transform, identify the exact path and risk.

Do not correct it in this task.


GLOBAL TRANSLATION VERSUS GEOMETRIC ANCHOR

Analyze whether the fixed -78 vertical translation is being applied relative to an appropriate whole-game reference.

Determine what the current transform is anchored to, such as:

- legacy canvas center;
- projected origin;
- near-rim center;
- tube centroid;
- model origin;
- bounding-box center;
- another source-defined reference.

Then determine whether differences in tube geometry and existing placement explain why a Level 9-derived fixed translation clips other tubes.

Evaluate candidate shared concepts only at the analytical level, including where supported:

- adjusting one whole-game vertical translation;
- anchoring the presentation to a stable global geometric reference;
- calculating one shared fit from an envelope covering all production tube bounds;
- reserving a shared safe region for all tubes;
- another single whole-game rule derived from the architecture.

Do not implement or numerically tune a proposed correction.

Do not recommend per-level offsets merely because tube bounds differ.


GAMEPLAY GEOMETRY REMAINS FROZEN

Do not recommend modifying:

- LevelDefinition.Points;
- lane topology;
- tube connectivity;
- collisions;
- enemy or player paths;
- projectile mechanics;
- wave behavior;
- timing;
- movement;
- level mechanics.

The desired remedy must remain in the shared presentation layer unless evidence proves that the candidate accidentally altered gameplay geometry.


SECONDARY TEST FAILURES

Diagnose, read-only, the reconstruction report's two secondary findings.

1. Legacy Level 8 and Level 9 assertions:

The expanded legacy-coordinate assertions reported:

expected 48
actual 46

Determine:

- the exact assertions involved;
- where expected 48 came from;
- where actual 46 comes from;
- whether this is a faulty newly added expectation;
- whether it exposes an existing level/projection behavior;
- whether it is related to the portrait clipping;
- whether it is unrelated test noise.

Do not update the expected or actual value.

2. HUD floating-point difference:

The HUD corner assertion reported approximately:

expected 960
actual 960.00006

Determine whether that is ordinary floating-point rounding or evidence of a genuine bounds problem.

Do not change tolerances or production calculations.

Keep this secondary to the primary clipping diagnosis.


PROJECTION DISCIPLINE

Do not experimentally vary K or ZOffset.

Do not use the diagnostic as an opportunity to solve the unresolved shoulder/far-rim perspective difference.

Determine only whether unchanged K/ZOffset contribute directly to the clipping calculation.

Perspective calibration remains a later milestone.


REFERENCE AUTHORITY

Do not search the web.

Do not consult external Tempest implementations.

Do not use Mark Incitti's implementation to derive a correction.

Do not use any Gemini-generated material or discarded outage-period work.

Use only:

- the current accepted project and current candidate diff;
- the three authoritative reports named above;
- directly relevant durable project documentation;
- bounded numerical analysis from production Levels 1-9.


BOUNDED COMMANDS

Any command, test, calculation, or source query must be bounded by a clear timeout, file scope, level range, or iteration limit.

Do not run the full test suite.

Do not launch the game.

Do not start an open-ended repository search.

If a command stalls, hangs, times out, or encounters a security block, stop according to the project's established self-stop policy.

Do not report blocked or incomplete commands as passed.


DIAGNOSTIC DECISION

At the end, classify the evidence as one of:

READY FOR ONE SHARED CORRECTION

Use only if the source and bounds analysis clearly identifies one bounded whole-game presentation correction suitable for the next implementation prompt.

INSUFFICIENT EVIDENCE

Use if more evidence is required.

ARCHITECTURAL MISMATCH

Use if tube and gameplay-entity rendering paths do not receive the same presentation mapping and that must be resolved before placement calibration.

TEST-ASSUMPTION PROBLEM

Use if the apparent cross-level clipping is materially caused by invalid test assumptions rather than the actual presentation mapping.

If multiple issues exist, identify the primary classification and list secondary issues separately.


REPORT

Preserve this substantive diagnostic prompt under AI-Logs/Prompts according to project instructions.

Create a new ignored report under AI-Logs/Reports with a descriptive 2026-11-13 filename, such as:

2026-11-13-whole-game-presentation-cross-level-clipping-diagnostic-report.md

The report must include:

- starting Git state;
- authoritative current candidate assumptions;
- source paths inspected;
- Level 1-9 bounds matrix;
- transform sequence;
- actual transform anchor;
- explanation of differing vertical extents;
- tube/entity alignment findings;
- clipping root cause;
- legacy 48-versus-46 diagnosis;
- HUD 960-versus-960.00006 diagnosis;
- whether K/ZOffset are directly implicated;
- recommended single whole-game correction model, if supported;
- approaches explicitly rejected;
- remaining uncertainties;
- final diagnostic classification;
- final Git state;
- confirmation that candidate source/test files were unchanged;
- confirmation that no visual assets were analyzed.


FINAL RESPONSE

Report:

1. Whether the read-only diagnosis completed.
2. Starting and final Git state.
3. Confirmation that the four candidate files were unchanged.
4. Level-by-level bounds summary for Levels 1-9.
5. Exact levels and edges that exceed the viewport.
6. Root cause of the cross-level clipping.
7. Current shared-transform anchor.
8. Whether tube and gameplay entities share the same final transform.
9. Whether the tube/entity divergence experienced in discarded experimentation is possible in this candidate.
10. Diagnosis of legacy expected 48 versus actual 46.
11. Diagnosis of HUD 960 versus 960.00006.
12. Whether K or ZOffset are directly implicated.
13. Final classification.
14. Recommended smallest next implementation step, if evidence is sufficient.
15. Diagnostic report path.
16. Confirmation that nothing was staged, committed, or pushed.
17. Confirmation that no visual assets were analyzed.

Do not implement the recommendation.

TEMPEST_PRESENTATION_CROSS_LEVEL_CLIPPING_DIAGNOSTIC_20261113