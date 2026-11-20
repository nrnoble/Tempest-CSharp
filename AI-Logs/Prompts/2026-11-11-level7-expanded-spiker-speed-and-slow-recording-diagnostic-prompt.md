FAIL-CLOSED TASK COMPLETENESS CHECK

Read this entire user-provided task before using tools, reading repository files, running commands, or taking repository action.

Before doing anything else, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context may appear after the user's token and do not invalidate it.

This check detects an incomplete or truncated user task, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project or repository action and explain why.


TASK: IMPLEMENT EXPANDED LEVEL 7 SPIKER CALIBRATION AND RECORDING-COMPATIBLE GAME-SPEED CONTROL

This is a tightly bounded DEVELOPMENT-ONLY calibration and recording milestone.

Preserve the entire current uncommitted Level 7 candidate.

Do not select a permanent production Spiker speed.

Do not change enemy-projectile speed.

Do not perform the deferred Stage 2 Spiker/projectile value-swap experiment.


STARTING CHECKPOINT

Read:

AI-Logs/Reports/2026-11-11-level7-spiker-speed-manual-calibration-experiment-report.md

Treat it as the immediately preceding implementation checkpoint.

Before editing, run:

git status --short --branch
git diff --stat

The expected tracked/untracked candidate inventory is the inventory recorded in that report, plus the intentionally modified:

.github/copilot-instructions.md

The instructions-file change contains the durable TEMP/myNotes.md exclusion rule added in the immediately preceding maintenance task.

Preserve that rule.

If the actual source/test file inventory materially differs from the prior calibration report, STOP and report the discrepancy before making changes.

Do not reset, restore, clean, checkout, stage, commit, push, or discard anything.


MANUAL CALIBRATION RESULT

Neal manually tested:

- normal/current randomized Level 7 Spiker behavior;
- fixed Spiker speed 3.000;
- fixed Spiker speed 2.8125;
- fixed Spiker speed 2.625.

Neal judged all four cases too fast compared with arcade/MAME experience.

Classify this as:

MANUAL C# CALIBRATION RESULT

Do not convert it into an exact arcade-derived, MAME-derived, or ROM-derived numerical speed.


PART 1: EXPAND THE DEVELOPMENT-ONLY LEVEL 7 SPIKER-SPEED OVERRIDE

The existing development-only option is:

--level7-spiker-speed <value>

It currently accepts fixed values from 2.625 through 3.000.

Extend its accepted diagnostic range to:

1.000 through 3.000 inclusive.

Retain appropriate validation:

- invariant numeric parsing;
- finite values only;
- reject malformed input;
- reject repeated or conflicting values;
- reject values below 1.000;
- reject values above 3.000.

Useful manual calibration values include:

1.0
1.5
2.0
2.5
2.625
3.0

Do not create separate hard-coded modes for these values. They are ordinary numeric inputs.

The option remains Level-7-only.

With no --level7-spiker-speed option, current normal Level 7 production behavior must remain unchanged.

Do not change:

- the normal randomized production Spiker-speed formula;
- enemy-projectile speed;
- Spiker firing cadence;
- Spiker ammunition;
- repeated-cycle firing behavior;
- Spike growth rules, except unavoidable timing consequences of a chosen diagnostic movement speed;
- concurrency;
- admission;
- reconstruction;
- enemy supply;
- Tanker movement;
- Flipper movement;
- Tanker-cargo movement;
- geometry;
- Blaster behavior;
- Levels 1-6;
- Level 8.


PART 2: ADD DEVELOPMENT-ONLY --speed CONTROL

Add the smallest safe development-only command-line option conceptually equivalent to:

--speed <multiplier>

Required values include:

--speed 1.0
--speed 0.5

Inspect the actual fixed-step loop, rendering/presentation pacing, recording system, and playback system before implementing this option.

The intended semantic model, if supported by the existing architecture, is:

--speed changes WALL-CLOCK / PRESENTATION PACING.

It must not multiply or rewrite gameplay mechanics.

For example:

--speed 1.0

means normal real-time presentation.

--speed 0.5

means the same fixed-step gameplay simulation is presented at approximately half normal real-time speed.

Do not implement --speed by multiplying:

- Spiker movement;
- projectile movement;
- Blaster movement;
- enemy movement;
- firing timers;
- lifecycle timers;
- collision behavior;
- simulation-state values.

The fixed simulation step should retain its existing semantic meaning.

If the architecture cannot safely support wall-clock-only speed scaling without altering deterministic gameplay semantics, STOP and explain the conflict instead of improvising a different model.

Choose a small sensible accepted multiplier range after inspecting existing conventions.

At minimum:

0.5
1.0

must be supported if the implementation is safe.

Reject:

- zero;
- negative values;
- NaN;
- infinity;
- malformed input;
- repeated or conflicting values.

Do not over-engineer the range or interface.


PART 3: RECORDING SUPPORT IS REQUIRED

Neal needs this workflow:

dotnet run --project src\Tempest.Game -- --level 7 --lives 10 --speed 0.5 --record-gameplay Temp\GameplayRecordings\lvl7-SpikerTest.json --level7-spiker-speed 2.0

This combined use is an explicit required deliverable, not an optional enhancement.

Inspect the existing accepted gameplay recording/playback architecture before changing it.

Determine and document:

1. Whether --speed changes only wall-clock presentation pacing.

2. Whether --speed should remain outside recording metadata because recorded ticks and inputs are independent of real-time pacing.

3. Whether --level7-spiker-speed changes deterministic startup/simulation state.

4. Whether the selected diagnostic Spiker speed must therefore be stored in recording startup metadata.

5. How playback restores the selected Level 7 diagnostic Spiker speed automatically.

6. Whether playback can use a different --speed presentation multiplier without changing the recorded simulation.

Preferred result, if supported by the architecture:

- Recording at --speed 0.5 records the same tick-based simulation semantics as recording at normal presentation speed.
- --speed controls wall-clock presentation only.
- Playback may run with its own presentation-speed selection.
- The selected --level7-spiker-speed is preserved in recording metadata because it affects deterministic gameplay startup/state.
- Playback automatically restores that recorded Spiker diagnostic speed.
- Existing recordings remain compatible.

Do not simply remove the current recording incompatibility guard.

Make the smallest maintainable change required for faithful deterministic recording and playback.

Preserve existing accepted recording/playback behavior when the new diagnostics are absent.

If supporting the diagnostic state safely requires a substantial recording-format redesign, STOP and report the obstacle before making a broad change.


BACKWARD COMPATIBILITY

Protect existing recorded-gameplay files.

If recording metadata is extended:

- preserve compatibility with existing recordings where practical;
- use a safe default when the new metadata field is absent;
- do not reinterpret existing recordings as having a Level 7 diagnostic speed;
- add focused compatibility coverage.

Do not broadly redesign or version the recording system unless the existing architecture clearly requires it.


REQUIRED MANUAL COMMANDS

At completion, provide exact PowerShell-ready commands matching the actual implementation.

Provide at least:

NORMAL-SPEED FIXED SPIKER CONTROL

dotnet run --project src\Tempest.Game -- --level 7 --lives 10 --level7-spiker-speed 2.0

HALF-SPEED RECORDING AT SPIKER 1.0

dotnet run --project src\Tempest.Game -- --level 7 --lives 10 --speed 0.5 --record-gameplay Temp\GameplayRecordings\lvl7-SpikerTest-1.0.json --level7-spiker-speed 1.0

HALF-SPEED RECORDING AT SPIKER 1.5

dotnet run --project src\Tempest.Game -- --level 7 --lives 10 --speed 0.5 --record-gameplay Temp\GameplayRecordings\lvl7-SpikerTest-1.5.json --level7-spiker-speed 1.5

HALF-SPEED RECORDING AT SPIKER 2.0

dotnet run --project src\Tempest.Game -- --level 7 --lives 10 --speed 0.5 --record-gameplay Temp\GameplayRecordings\lvl7-SpikerTest-2.0.json --level7-spiker-speed 2.0

HALF-SPEED RECORDING AT SPIKER 2.5

dotnet run --project src\Tempest.Game -- --level 7 --lives 10 --speed 0.5 --record-gameplay Temp\GameplayRecordings\lvl7-SpikerTest-2.5.json --level7-spiker-speed 2.5

Also provide exact playback commands for those resulting recordings.

If the actual accepted syntax differs, provide the correct implemented commands rather than copying these examples blindly.


AUTOMATED VALIDATION

Add or update focused tests proving, as applicable:

1. --level7-spiker-speed accepts:
   - 1.000;
   - 1.500;
   - 2.000;
   - 2.500;
   - 2.625;
   - 3.000.

2. It rejects:
   - values below 1.000;
   - values above 3.000;
   - malformed values;
   - NaN;
   - infinity;
   - repeated/conflicting options.

3. Normal no-option Level 7 production speed selection remains unchanged.

4. Enemy-projectile speed remains unchanged.

5. --speed accepts supported values including 0.5 and 1.0.

6. Invalid --speed values are rejected.

7. --speed affects only the intended wall-clock/pacing abstraction and does not modify fixed-step gameplay movement or mechanics.

8. Ordinary gameplay recording/playback remains unchanged when diagnostics are absent.

9. A recording made with a fixed diagnostic Level 7 Spiker speed preserves and restores that deterministic startup state during playback, if metadata storage is required.

10. Existing recording files without the new optional metadata remain compatible.

11. Recording presentation speed does not silently alter recorded simulation semantics.

Also run bounded relevant sets covering:

- command-line/debug-controller parsing;
- Level 7 Spiker projectile relative-speed diagnostics;
- Level 7 Spiker firing;
- Level 7 production/movement/regression;
- gameplay recording/playback;
- solution build;
- git diff --check.

Do not launch interactive Tempest.

Do not perform manual gameplay acceptance.

Follow the project's established safe-stop policy for blocked artifacts, Windows security interference, hangs, stalls, timeouts, or commands that stop making meaningful progress.

A blocked, canceled, stalled, hung, or timed-out command is not a pass.


EVIDENCE CLASSIFICATION

Maintain the following classifications:

MAME-OBSERVED:
A Level 7 Spiker projectile visibly pulls away from its firing Spiker.

MANUAL C# CALIBRATION RESULT:
Neal judged normal/current, 3.000, 2.8125, and 2.625 Spiker movement too fast.

PROJECT TEST HYPOTHESIS:
A Level 7 Spiker speed below 2.625 may better match arcade/MAME.

C# MEASURED:
Results produced by deterministic tests and diagnostics.

PROJECT DECISION:
None. No production Spiker speed has been selected.

Do not label any newly available diagnostic value as ROM-backed, arcade-correct, MAME-correct, or accepted production behavior.


STAGE 2 REMAINS OUT OF SCOPE

Do not perform the proposed literal Spiker/projectile value swap.

Do not change projectile speed.

The possible future Stage 2 experiment remains:

Identify the actual translated Spiker-movement and enemy-projectile speed values and, if technically well-defined, exchange them in a development-only experiment.

That experiment requires separate authorization after Neal evaluates the expanded 1.0-3.0 range.


GIT AND WORKTREE SAFETY

This is not a closeout.

Do not:

- stage;
- commit;
- push;
- reset;
- restore;
- clean;
- checkout over existing changes;
- discard candidate work.

Preserve all unrelated changes exactly as found.

At completion, show:

git status --short --branch

Clearly distinguish:

- pre-existing Level 7 candidate changes;
- the prior .github/copilot-instructions.md maintenance change;
- files changed specifically by this implementation task.


REQUIRED AI LOGS

This is substantial Tempest work.

Save the complete substantive prompt under:

AI-Logs/Prompts/

using a descriptive date-prefixed filename.

Create the implementation report at exactly:

AI-Logs/Reports/2026-11-11-level7-expanded-spiker-speed-and-slow-recording-diagnostic-report.md

The task is not complete until the report file has been created and then read back or otherwise explicitly verified to exist.

AI-Logs remain ignored working/history artifacts.

Do not stage or commit them.


REQUIRED REPORT CONTENT

The report must include:

- starting Git status;
- whether starting source/test inventory matched the prior Stage 1 report;
- files inspected;
- files changed specifically by this task;
- expanded 1.0-3.0 Level 7 Spiker diagnostic implementation;
- accepted numeric range and parser behavior;
- confirmation that normal no-option production behavior is unchanged;
- --speed implementation;
- exact --speed semantics;
- fixed-step versus wall-clock/presentation distinction;
- recording metadata decision and reasoning;
- behavior of --level7-spiker-speed during recording;
- behavior of --level7-spiker-speed during playback;
- behavior of --speed during recording and playback;
- compatibility findings for existing recordings;
- focused test commands and exact results;
- Level 7 regression command and exact result;
- recording/playback regression command and exact result;
- build command and result;
- git diff --check result;
- exact manual recording commands;
- exact playback commands;
- final Git status;
- explicit confirmation enemy-projectile speed was not changed;
- explicit confirmation normal Level 7 production Spiker selection was not changed;
- explicit confirmation no permanent Spiker speed was selected;
- explicit confirmation Stage 2 value swapping was not performed;
- explicit confirmation interactive Tempest was not launched;
- manual acceptance status: PENDING NEAL.


COMPLETION REQUIREMENTS

Before claiming completion:

1. Verify the required prompt log exists.
2. Verify the required report exists.
3. Verify the report contains the required implementation and validation information.
4. Show final Git status.
5. State whether implementation is complete, partial, or blocked.

If the report cannot be created, do not claim task completion.

If implementation or validation is blocked, create a PARTIAL or BLOCKED report describing:

- work completed;
- work not completed;
- exact blocker;
- exact command;
- exact error;
- remaining validation;
- Neal's required next step.


STOP CONDITION

After implementation, bounded automated validation, prompt/report records, exact manual commands, and final Git status are complete, STOP.

Do not launch Tempest.

Do not choose a production Spiker speed.

Do not change projectile speed.

Do not perform Stage 2.

Wait for Neal's manual testing.

TEMPEST_LEVEL7_IMPLEMENT_EXPANDED_SPIKER_AND_RECORDING_SPEED_20261111