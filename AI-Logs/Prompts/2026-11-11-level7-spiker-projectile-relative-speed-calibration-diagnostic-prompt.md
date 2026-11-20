FAIL-CLOSED TASK COMPLETENESS CHECK

Read this entire user-provided task before using any tools, reading project files, running commands, or taking any repository action.

Before doing anything else, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context may appear after the user's token and do not invalidate it.

This check exists to detect an incomplete or truncated user task, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project or repository action and explain that the task may be incomplete.


TASK: LEVEL 7 SPIKER / PROJECTILE RELATIVE-SPEED CALIBRATION DIAGNOSTIC

This is a tightly bounded diagnostic-only Tempest Level 7 milestone.

Do NOT make a production gameplay-speed correction in this task.

The current uncommitted Level 7 candidate must be preserved. Do not reset, discard, rewrite, or broadly clean up existing work.

Before making any changes, inspect the repository's actual Git/worktree state and the current Level 7 implementation enough to understand the existing candidate. Preserve unrelated changes exactly as found.

BACKGROUND / OBSERVATION

Direct MAME observation has established an important Level 7 behavior:

A projectile fired by a Spiker can visibly travel faster up the tube than the Spiker that fired it. In a reproduced MAME case, a Spiker projectile fired from behind a Tanker was able to overtake the Tanker.

The current C# implementation does not appear to preserve the same relative Spiker/projectile relationship under all Level 7 Spiker speeds.

Earlier static ROM interpretation produced a conflicting result. Do not resolve that conflict merely by choosing the static ROM calculation or by assuming the MAME observation was inaccurate.

For this milestone, direct MAME observation is empirical evidence that must be measured against deterministic C# behavior.

GOAL

Create the smallest practical DEVELOPMENT-ONLY deterministic diagnostic needed to measure Level 7 Spiker/projectile relative movement over fixed active simulation steps.

The diagnostic should make it possible to compare:

1. distance advanced by the Spiker;
2. distance advanced by a projectile fired by that Spiker;
3. projectile-to-Spiker separation;
4. change in that separation over known fixed active-step intervals;
5. enough initial-state information to make the measurement understandable and reproducible.

This diagnostic is intended to answer a narrow question:

How much faster or slower is a Level 7 projectile relative to its firing Spiker in the C# simulation, and what corresponding relative-rate behavior can Neal observe in MAME?

SCOPE

Allowed:

- Inspect the existing Level 7 Spiker and enemy-projectile movement implementation.
- Inspect project-local Atari/Rev. 2 ROM-derived reference material when useful for understanding existing behavior.
- Add a narrowly bounded DEVELOPMENT-ONLY diagnostic, test, or deterministic measurement harness.
- Reuse existing development/test infrastructure where practical.
- Add narrowly scoped automated tests for the diagnostic if justified.
- Produce a simple measurement protocol Neal can reproduce in MAME.
- Record exact C# diagnostic numbers produced by the deterministic scenario.
- Identify the existing production constants/code paths responsible for the measured relationship, without changing those production values.

Not allowed:

- NO production Spiker speed correction.
- NO enemy-projectile speed correction.
- NO Tanker movement changes.
- NO Flipper movement changes.
- NO Tanker-cargo movement changes.
- NO firing-cadence changes.
- NO concurrency/admission/reconstruction changes.
- NO enemy-supply changes.
- NO geometry changes.
- NO broad refactor.
- NO change to accepted Levels 1-6.
- NO Level 8 work.
- NO speculative gameplay tuning.
- Do not turn this into another broad Level 7 investigation.

In particular, do NOT choose a desired Spiker speed and implement it.

We want evidence first.

DIAGNOSTIC DESIGN

Prefer the smallest deterministic implementation that isolates relative along-tube movement.

The diagnostic should establish a known Level 7 Spiker state and a corresponding projectile in the same lane/path relationship.

Measure both using the simulation's actual movement semantics rather than inventing a separate approximation of the production movement formulas.

Where practical, sample at:

- initial/reference state;
- one active simulation step;
- several subsequent fixed active-step intervals;
- a sufficiently long interval to make relative separation unmistakable without introducing unrelated lifecycle behavior.

Choose sensible fixed intervals based on the actual implementation after inspection. Keep the run tightly bounded.

For every reported sample, provide something equivalent to:

active step
Spiker along-tube position/depth
projectile along-tube position/depth
projectile-to-Spiker separation
change in separation from the reference state

If different legitimate Level 7 Spiker movement rates already exist, determine whether more than one representative deterministic case is necessary to expose the issue. Keep this minimal. Do not broaden coverage unnecessarily.

Clearly distinguish:

- raw position/depth values;
- distance traveled during the interval;
- relative separation;
- relative movement ratio, if calculating a ratio is mathematically meaningful for the measured interval.

Do not hide the underlying raw measurements behind only a percentage or ratio.

IMPORTANT: Use the project's actual coordinate/sign conventions and explain them. Do not assume that numerically increasing depth necessarily means movement toward the rim.

MAME COMPARISON PROCEDURE

At the end, give Neal a SIMPLE observational procedure for making an equivalent comparison in MAME.

This needs to be practical rather than laboratory-perfect.

Prefer a procedure that can use MAME pause/frame advance and an easily recognizable Level 7 situation.

The procedure should tell Neal:

1. what Level 7 situation to look for;
2. how to pick a Spiker shot that is suitable for observation;
3. what visual reference points to record at the starting frame;
4. how many frames or equivalent fixed observation intervals to advance;
5. what to record at the ending frame(s);
6. how to compare projectile displacement with Spiker displacement;
7. how to report the result back without needing pixel-perfect image analysis.

If exact correspondence between a MAME video frame and one C# active simulation step cannot be established from existing evidence, SAY SO.

Do not manufacture a frame-to-step mapping.

In that case, structure the MAME procedure around relative displacement over a fixed number of MAME frames and make clear that we are comparing the relative rate/relationship rather than pretending the absolute time bases have already been proven identical.

A particularly useful observable is whether the gap between projectile and firing Spiker:

- grows toward the rim;
- remains approximately constant; or
- shrinks.

Where practical, also make it possible to estimate a rough relative speed ratio from observable travel distances.

STATIC ROM CONFLICT

Investigate the existing project-local ROM/disassembly evidence only enough to accurately document why the previous static interpretation conflicts with observed MAME behavior.

Do NOT allow the static calculation to override an experimentally measured MAME result.

Classify conclusions carefully:

ROM-BACKED:
Only conclusions directly supported by the disassembly/data.

MAME-OBSERVED:
Behavior directly observed in MAME.

C# MEASURED:
Numbers produced by the deterministic diagnostic.

PROJECT INTERPRETATION:
Any explanation connecting those observations where the ROM mechanism has not actually been demonstrated.

Do not silently convert PROJECT INTERPRETATION into ROM-BACKED behavior.

VALIDATION

Keep automated validation proportional to this diagnostic milestone.

Run:

- the new focused diagnostic/test;
- directly relevant existing Level 7 tests needed to ensure the diagnostic did not alter behavior;
- build;
- git diff --check.

Use bounded commands and tests.

Do not launch the interactive game for Neal.

If Windows Application Control, Smart App Control, or similar security blocks an artifact or command, follow the project's established safe-stop policy. Do not weaken security, bypass it, repeatedly retry the same blocked artifact, substitute stale binaries, or claim validation passed.

If any operation stalls or exceeds a reasonable bounded duration, stop it and isolate the problem with smaller bounded runs.

Canceled, stalled, blocked, or timed-out validation is NOT a pass.

GIT / PRESERVATION

This is NOT a closeout task.

Do NOT commit.
Do NOT push.
Do NOT stage unrelated files.
Do NOT clean/reset the worktree.
Do NOT discard the existing Level 7 candidate.

Preserve all existing unrelated changes.

Before finishing, show the final Git status and make clear exactly what this diagnostic added or changed.

DOCUMENTATION / REPORT

This is substantial Tempest work.

Save the substantive prompt under AI-Logs/Prompts using the project's established date-prefixed naming convention.

Save the investigation/implementation report under AI-Logs/Reports, NOT TEMP/.

AI-Logs remain ignored historical/working artifacts and must not be staged or committed.

A suitable report name is:

AI-Logs/Reports/2026-11-11-level7-spiker-projectile-relative-speed-calibration-diagnostic-report.md

The report should include:

- starting repository/worktree state;
- files inspected;
- diagnostic design;
- exact deterministic C# measurements;
- relevant production movement code/constants discovered;
- ROM evidence and its limits;
- explanation of the conflict with MAME observation without speculating beyond the evidence;
- automated validation performed and exact results;
- simple MAME measurement procedure for Neal;
- files changed;
- final Git status;
- explicit statement that NO production gameplay speed correction was made;
- explicit statement that manual/MAME comparison is pending Neal.

STOP CONDITION

After the deterministic diagnostic, report, validation, and MAME measurement instructions are complete, STOP.

Do not proceed from measurement into tuning.

Even if the diagnostic makes an obvious correction appear warranted, report the evidence and wait for Neal's MAME measurement/manual assessment.

The next milestone, if any, will separately decide whether a small Level-7 Spiker-only speed correction is justified.

TEMPEST_LEVEL7_SPIKER_PROJECTILE_CALIBRATION_DIAGNOSTIC_ONLY_20261111
