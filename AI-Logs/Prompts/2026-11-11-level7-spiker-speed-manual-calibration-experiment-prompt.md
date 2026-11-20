FAIL-CLOSED TASK COMPLETENESS CHECK

Read this entire user-provided task before using any tools, reading project files, running commands, or taking any repository action.

Before doing anything else, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context may appear after the user's token and do not invalidate it.

This check exists to detect an incomplete or truncated user task, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project or repository action and explain that the task may be incomplete.


TASK: LEVEL 7 DEVELOPMENT-ONLY SPIKER-SPEED MANUAL CALIBRATION EXPERIMENT

This is a tightly bounded DEVELOPMENT-ONLY Level 7 calibration experiment.

Do not make a permanent production-speed correction in this task.

Preserve the entire current uncommitted Level 7 candidate exactly except for the minimum development-only diagnostic infrastructure needed for this experiment.

Do not reset, discard, clean, rewrite, commit, push, or broadly refactor the current candidate.


BACKGROUND

The immediately preceding Level 7 Spiker/projectile relative-speed diagnostic established the existing C# behavior.

Read and use:

AI-Logs/Reports/2026-11-11-level7-spiker-projectile-relative-speed-calibration-diagnostic-report.md

The deterministic diagnostic established that:

- enemy projectile movement is currently 3.000 depth units per active step;
- current Level 7 production Spiker speeds nominally range approximately 2.625 through 4.625;
- at Spiker speed 2.625 the projectile pulls ahead;
- at Spiker speed 3.000 the separation remains constant;
- at Spiker speed 4.500 the Spiker moves substantially faster than its projectile and overtakes it.

Neal has also inspected an existing frame-by-frame Level 7 MAME recording.

MAME observation supports this qualitative relationship:

MAME-OBSERVED:
A projectile fired by a Level 7 Spiker visibly pulls away from its firing Spiker while both move toward the rim. Projectile-to-Spiker separation grows.

Separate earlier MAME observation also showed a Spiker projectile fired from behind overtaking a Tanker.

Do NOT infer an exact MAME numerical speed or projectile/Spiker ratio from those observations. Perspective makes raw screen-pixel distances unsuitable as a proven linear tube-depth scale, and no C# active-step-to-MAME-frame mapping has been established.

Neal additionally has a subjective perception from manual C# play that Level 7 Spikers themselves may be climbing too quickly compared with arcade/MAME.

IMPORTANT:

That last statement is NOT established arcade behavior.

Treat it strictly as a TEST HYPOTHESIS:

PROJECT TEST HYPOTHESIS:
Current C# Level 7 Spiker climb speed may be too fast.

The purpose of this task is to make that hypothesis easy to test manually without changing projectile speed or permanently tuning production behavior.


PRIMARY GOAL

Add the smallest practical DEVELOPMENT-ONLY mechanism that lets Neal manually play Level 7 with controlled alternative Spiker speeds or speed ranges.

The experiment must isolate Spiker movement speed.

Enemy projectile speed must remain completely unchanged at its current production behavior.

Neal will manually compare several bounded Spiker-speed alternatives against:

1. MAME/arcade visual Spiker climb speed;
2. projectile-versus-Spiker relative movement;
3. overall Level 7 gameplay pressure/difficulty;
4. repeated Spiker ascent/retreat cycles.

This task provides the experimental controls.

It does NOT decide the final production value.


FIRST: VERIFY THE CURRENT IMPLEMENTATION

Before implementing the diagnostic:

1. Inspect Git/worktree status.
2. Preserve all existing uncommitted Level 7 candidate work.
3. Read the preceding relative-speed diagnostic report.
4. Inspect the actual current production code paths responsible for:
   - Level 7 Spiker speed selection;
   - Spiker movement;
   - enemy projectile movement.
5. Verify rather than assume that the preceding diagnostic numbers still describe the current candidate.

If the actual current implementation materially contradicts the preceding report, STOP and report the discrepancy before adding the experiment.

Do not silently reinterpret or repair unrelated code.


DEVELOPMENT-ONLY CONTROL

Prefer a development-only command-line diagnostic override consistent with existing Tempest diagnostic conventions.

For example, something conceptually equivalent to:

--level7-spiker-speed <value>

or, only if necessary because production legitimately uses a randomized range:

--level7-spiker-speed-range <min,max>

The exact interface should follow existing project architecture and command-line conventions after inspection.

Do not create a large subsystem.

Do not modify normal production Level 7 behavior when the diagnostic option is absent.

The normal no-option path must remain behaviorally identical to the current candidate.

The override should affect LEVEL 7 SPIKER MOVEMENT ONLY.

It must not alter:

- projectile speed;
- projectile update cadence;
- Spiker firing cadence;
- Spiker ammunition;
- repeated-cycle firing behavior;
- Spike growth mechanics except for unavoidable timing consequences of the Spiker moving at a different speed;
- Tanker movement;
- Flipper movement;
- Tanker cargo movement;
- concurrency;
- admission;
- reconstruction;
- enemy supply;
- Blaster behavior;
- Levels 1-6;
- Level 8.


CALIBRATION VALUES

Do not guess a new permanent production value.

Provide a small, useful set of manually testable fixed Level 7 Spiker-speed candidates.

Use the existing diagnostic evidence to choose sensible candidates.

At minimum, make these cases easy for Neal to test:

A. CURRENT / CONTROL
Normal Level 7 behavior with no diagnostic override.

B. 3.000
A deterministic Spiker speed equal to the current projectile movement value.

This is an important comparison point, not a proposed final value.

C. 2.625
The low end already measured by the prior deterministic C# diagnostic.

D. At least one sensible intermediate value between 2.625 and 3.000 if useful for perceptual calibration.

Do not create a huge matrix of values.

If inspection shows a better compact set of 3-4 diagnostic values for distinguishing the hypothesis, explain why and use it.

Values above 3.000 are not useful as the primary slower-Spiker experiment unless needed as a control, because the prior diagnostic already establishes that a Spiker moving above 3.000 outruns the current +3.000 projectile.

Do not modify projectile speed to compensate.


IMPORTANT DISTINCTION: FIXED SPEED VS PRODUCTION RANGE

Current Level 7 production appears to select varying Spiker speeds.

For this diagnostic, deterministic FIXED Spiker-speed overrides are preferable if technically reasonable because Neal needs to know exactly what movement rate he is observing.

If a fixed override is used, clearly explain that the diagnostic deliberately suppresses the production Spiker speed variation for the selected test run.

Normal Level 7 without the diagnostic option must continue using the existing production behavior.


AUTOMATED DIAGNOSTIC / TEST REQUIREMENTS

Reuse the existing:

Level7SpikerProjectileRelativeSpeedDiagnosticTests.cs

where practical.

Do not destroy or rewrite the evidence it currently provides.

Add only enough automated validation to establish that:

1. the diagnostic override selects the requested Spiker speed on Level 7;
2. projectile speed remains unchanged;
3. with no override, current production Level 7 Spiker-speed behavior is unchanged;
4. Levels 1-6 are not affected by the Level 7 diagnostic option;
5. the override does not alter Spiker firing rules or other unrelated behavior.

For the selected calibration values, report the expected deterministic projectile-to-Spiker separation trend:

- projectile pulls ahead;
- separation remains constant;
- or Spiker gains.

This mathematical result is C# MEASURED / C# DERIVED only.

Do not label it arcade behavior.


MANUAL TEST COMMANDS

At the end, give Neal exact launch commands for each calibration case.

Do not launch the interactive game yourself.

The commands should allow Neal to start Level 7 directly using the existing project-supported direct-level mechanism plus the new development-only speed override.

Provide clearly labeled commands for:

1. CURRENT CONTROL — ordinary Level 7 with no override.
2. FIXED 3.000.
3. FIXED 2.625.
4. Any additional intermediate candidate chosen.

Keep commands copy/paste ready for Neal's Windows PowerShell/terminal environment.


MANUAL ACCEPTANCE QUESTIONS FOR NEAL

Ask Neal to evaluate each diagnostic setting against the same narrow questions.

A. SPIKER CLIMB SPEED

Ignoring the projectile initially:

Does the Spiker itself appear to climb the tube:

- clearly too fast;
- somewhat too fast;
- approximately like MAME/arcade;
- somewhat too slow;
- clearly too slow?

This is the primary experiment.

B. PROJECTILE RELATIONSHIP

Does a projectile visibly pull away from the firing Spiker?

Classify:

- strongly;
- moderately;
- slightly;
- approximately equal;
- Spiker catches projectile.

C. LEVEL 7 GAMEPLAY PRESSURE

Without intentionally dying or creating artificial stress conditions:

Does ordinary Level 7 feel closer to MAME/arcade difficulty?

Do not attempt to tune concurrency or other mechanics based on this test.

D. REPEATED SPIKER CYCLES

When a Spiker ascends, retreats, and ascends again:

Does movement continue to look plausible and does the already-corrected repeated-cycle firing behavior remain intact?


EVIDENCE CLASSIFICATION

Keep conclusions explicitly separated:

ROM-BACKED:
Only behavior directly demonstrated from the local Atari Rev. 2 disassembly/data.

MAME-OBSERVED:
Only behavior directly observed by Neal in MAME.

ARCADE-OBSERVED:
Only behavior directly observed by Neal on the arcade game.

C# MEASURED:
Deterministic results from C# diagnostics/tests.

PROJECT TEST HYPOTHESIS:
The current perception that Level 7 C# Spikers may be too fast.

PROJECT DECISION:
Any eventual calibration chosen after manual testing.

Do not promote the Spiker-too-fast hypothesis to MAME-OBSERVED, ARCADE-OBSERVED, or ROM-BACKED.


DO NOT PERFORM THE FALLBACK VALUE-SWAP EXPERIMENT IN THIS TASK

There is a possible SECOND-STAGE experiment if this slower-Spiker calibration produces no convincing result:

development-only testing of whether existing Spiker/projectile-related values may have been effectively reversed or incorrectly translated.

DO NOT implement that experiment now.

DO NOT swap values now.

DO NOT change projectile speed now.

Simply mention in the final report that this fallback experiment remains available if Neal rejects all Stage 1 Spiker-speed candidates.

Stage 2 requires separate explicit authorization.


VALIDATION

Keep validation proportional and bounded.

Run:

- focused tests for the new diagnostic;
- the existing Level 7 Spiker/projectile relative-speed diagnostic;
- directly relevant Level 7 Spiker/firing/movement tests;
- appropriate Level 7 regression tests;
- build;
- git diff --check.

Do not launch the interactive game.

All potentially long commands must be bounded.

If Windows Application Control, Smart App Control, or another security mechanism blocks an artifact or command:

- do not weaken security;
- do not bypass security;
- do not repeatedly retry the same blocked artifact;
- do not substitute stale binaries;
- do not claim success.

Stop safely and report the blocked artifact, command, error, completed work, and remaining validation.

Likewise, stop bounded tests/processes that stall or make no meaningful progress.

Timed-out, canceled, stalled, or blocked commands are not passes.


GIT / WORKTREE SAFETY

This is NOT a closeout task.

DO NOT:

- commit;
- push;
- stage unrelated files;
- reset;
- clean;
- checkout over existing changes;
- discard the current Level 7 candidate.

Preserve all unrelated work exactly as found.

At completion show:

git status --short --branch

Clearly distinguish pre-existing candidate changes from files added/modified specifically for this calibration experiment.


PROMPT / REPORT RECORDS

This is substantial Tempest work.

Save this substantive task under:

AI-Logs/Prompts/

using an appropriate date-prefixed descriptive filename.

Save the implementation/diagnostic report under:

AI-Logs/Reports/2026-11-11-level7-spiker-speed-manual-calibration-experiment-report.md

AI-Logs are ignored historical/working artifacts.

Do not stage or commit AI-Logs.

The report must include:

- starting Git/worktree state;
- files inspected;
- verified current Spiker-speed implementation;
- verified current projectile-speed implementation;
- development-only override design;
- exact calibration values made available;
- C# deterministic separation result for each relevant value;
- focused validation and exact results;
- exact manual launch commands;
- manual acceptance checklist for Neal;
- files changed;
- final Git status;
- explicit confirmation that normal no-option Level 7 behavior was not intentionally changed;
- explicit confirmation that projectile production speed was not changed;
- explicit confirmation that no permanent Spiker production-speed choice was made;
- explicit confirmation that the fallback value-swap experiment was NOT performed;
- manual acceptance status: PENDING NEAL.


STOP CONDITION

After implementing and validating the DEVELOPMENT-ONLY calibration mechanism and providing Neal the exact manual test commands, STOP.

Do not choose the "best" speed yourself.

Do not convert a diagnostic value into production behavior.

Do not begin the value-swap experiment.

Do not proceed to Level 7 closeout.

Wait for Neal's manual comparison against MAME/arcade.

TEMPEST_LEVEL7_SPIKER_SPEED_STAGE1_MANUAL_CALIBRATION_20261111