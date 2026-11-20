IMPORTANT FAIL-CLOSED INSTRUCTION

Read this entire user-provided task before doing anything.

Before using any tools, reading project files, running commands, or changing the repository, locate the unique completion token included near the end of this user-provided task.

The completion token begins with "TEMPEST_" and appears on a line by itself.

Verify that exactly one complete token beginning with "TEMPEST_" is present in the user-provided task.

VS Code or the agent environment may append reminders, tagged-file information, attachment metadata, or other system-generated context after the user-provided task. Such automatically appended material does not invalidate the completion token.

If you cannot find exactly one complete token beginning with "TEMPEST_" in the user-provided task, STOP immediately without using tools or taking repository action and explain why.

PROJECT

Repository:
C:\Users\nrnob\OneDrive\Repro\Tempest

Required starting checkpoint:
dbf00313b609f81ff26860fd0fcdf0bd0f46c3b0

PURPOSE

Perform a read-only preimplementation analysis for the next production milestone:

Level 6 and natural Level 5→6 progression.

Do not implement Level 6 in this task.

Do not change production code, tests, project documentation, reference material, or other repository files except for the prompt/report logging required by the project's existing Copilot instructions.

Do not launch the interactive game.

STEP 1 - VERIFY THE BASELINE

Run:

Set-Location 'C:\Users\nrnob\OneDrive\Repro\Tempest'
git rev-parse HEAD
git status --short --branch
git diff --stat
git diff --cached --stat

HEAD must be exactly:

dbf00313b609f81ff26860fd0fcdf0bd0f46c3b0

The tracked worktree and index must be clean before analysis begins.

If not, STOP and report the discrepancy.

STEP 2 - READ CURRENT PROJECT AUTHORITY

Read the relevant current sections of:

.github/copilot-instructions.md
TEMPEST-DEVELOPMENT-NOTES.md
TEMPEST-BEHAVIOR-SPEC.md
README.md

Treat Levels 1–5 as the accepted production foundation.

Do not reopen accepted Levels 1–5 behavior unless Level 6 requires a narrowly identified reusable extension.

STEP 3 - ESTABLISH LEVEL 6 REFERENCE DATA

Inspect the repository's Incitti reference source and determine the exact reference behavior/data applicable to displayed Level 6.

Do not extrapolate Level 5 constants.

Determine, with source locations/evidence where possible:

- Level 6 tube/geometry identity.
- Closed versus open tube.
- Tube/lane count and geometry data.
- Palette/color information relevant at Level 6.
- Enemy eligibility.
- Wave composition/counts.
- Egg/reserve/release parameters.
- Flipper behavior and canflip policy.
- Tanker availability and cargo behavior.
- Spiker availability and Spike configuration.
- Fuseball eligibility.
- Pulsar eligibility.
- Starting Spikes, if any.
- Enemy firing parameters/policies.
- Any level-dependent speed/timing values.
- Super Zapper starting/recharge behavior.
- Exit/Avoid Spikes behavior.
- Any Level 6-specific scoring/bonus behavior that is required for correct Level 6 gameplay.
- What happens after successful Level 6 completion in the reference.

Explicitly distinguish:

1. Reference facts.
2. Existing accepted C# behavior that can be reused.
3. Missing C# capability required for Level 6.
4. Unknowns that must not be guessed.

STEP 4 - AUDIT CURRENT C# LEVEL-PROGRESSION SEAMS

Inspect only the code/tests necessary to determine the smallest production change.

At minimum consider the current equivalents of:

- LevelConfiguration
- LevelProgression
- LevelSet
- TempestGame level initialization/progression
- between-level approach
- level-specific firing configuration
- Spike/Spiker setup
- wave composition/selection
- geometry/rendering assumptions
- tests covering Level 4, Level 5, progression, campaign soak, approach, exit, firing, and reconstruction

Determine:

- Exactly where production progression currently terminates after Level 5.
- Which existing abstractions already support Level 6 without modification.
- Which abstractions have explicit Level 1–5 limits or throw/fail beyond Level 5.
- Whether Level 6 geometry requires any genuinely new geometry capability.
- Whether existing enemy systems already represent everything Level 6 needs.
- Whether extending the campaign soak through Level 6 would be appropriate after implementation.
- Whether a direct Level 6 development selector would materially help manual validation, following the established pattern for earlier levels.

Do not redesign working systems merely because another abstraction could be cleaner.

STEP 5 - DEFINE THE SMALLEST LEVEL 6 MILESTONE

Recommend a tightly bounded implementation milestone.

The preferred shape, if supported by the evidence, is:

- Add production Level 6 configuration.
- Add natural Level 5→6 progression.
- Reuse existing production simulation, rendering, enemies, approach, audio, controls, Super Zapper, reconstruction, and exit systems.
- Make Level 6 terminal after successful completion for this first bounded milestone unless existing authoritative roadmap/reference evidence strongly supports including Level 7 in the same change.

However, do not force this shape if the Level 6 source evidence demonstrates that an additional prerequisite capability is necessary.

Explicitly identify what should stay out of scope, including unless actually required:

- Level 7+
- broad later-level architecture
- attract mode/replay changes
- bonuses unrelated to Level 6 correctness
- high scores
- end-game animation
- spinner retuning
- accepted audio/presentation retuning
- entity-rendering fidelity work
- unrelated refactoring

STEP 6 - PROPOSE VALIDATION

Without changing tests yet, propose the smallest automated coverage needed for implementation.

Include likely checks for:

- exact Level 6 configuration
- natural Level 5→6 progression
- score/lives carry
- fresh Level 6 wave
- fresh Super Zapper state
- Level 6 enemy eligibility/composition
- Spike state if applicable
- death/re-entry/accounting
- exit behavior
- terminal Level 6 behavior
- prior Levels 1–5 regression protection
- campaign soak extension if justified

Also propose a concise manual acceptance checklist for Neal.

The interactive game must not be launched by VS Code Copilot.

STEP 7 - FINAL REPOSITORY CHECK

Before reporting, run:

git status --short --branch
git diff --stat
git diff --cached --stat

Account separately for any AI prompt/report files created solely because the project's global Copilot instructions require them.

There must be no production, test, gameplay-documentation, project, or reference-source modifications from this analysis.

FINAL REPORT

Report:

1. Verified starting HEAD.
2. Level 6 reference geometry/tube identity.
3. Level 6 enemy eligibility and wave composition.
4. Level 6 Spiker/Spike policy.
5. Level 6 firing/timing parameters.
6. Super Zapper/start-state requirements.
7. Natural Level 5→6 transition requirements.
8. Reference behavior after Level 6 completion.
9. Existing C# systems that can be reused unchanged.
10. Exact C# limitations currently blocking Level 6.
11. Any unknowns or conflicts that must be resolved before implementation.
12. Recommended smallest Level 6 implementation scope.
13. Explicit exclusions.
14. Recommended focused automated validation.
15. Recommended manual acceptance checklist.
16. Final Git status and confirmation that no Level 6 implementation was performed.

Do not implement Level 6.

Stop after the analysis and wait for Neal to review it.

TEMPEST_LEVEL6_PREIMPLEMENTATION_ANALYSIS_DBF0031
