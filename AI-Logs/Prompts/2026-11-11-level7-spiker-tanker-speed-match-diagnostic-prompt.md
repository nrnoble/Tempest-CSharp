FAIL-CLOSED TASK COMPLETENESS CHECK

Read this entire user-provided task before using tools, reading repository files, running commands, or taking repository action.

Before doing anything else, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context may appear after the user's token and do not invalidate it.

This check detects an incomplete or truncated user task, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project or repository action and explain why.


TASK: DEVELOPMENT-ONLY LEVEL 7 SPIKER/TANKER SPEED-MATCH EXPERIMENT

This is a tightly bounded DEVELOPMENT-ONLY diagnostic experiment.

The experiment tests a relationship observed by Neal during manual comparison with MAME and the arcade game:

1. Level 7 Spiker movement appears to use the same speed as Tanker movement.
2. Spiker shots appear to use the same speed as Tanker shots.

Do not treat this as a proven numerical ROM result.

Do not convert this experiment into permanent production behavior.

Do not change normal no-option Level 7 behavior.

Do not begin Level 8 work.


STARTING CHECKPOINT

Read:

AI-Logs/Reports/2026-11-11-level7-expanded-spiker-speed-and-slow-recording-diagnostic-report.md

Treat it as the immediately preceding implementation checkpoint.

Before editing, run:

git status --short --branch
git diff --stat

The expected candidate includes:

- accumulated uncommitted Level 7 concurrency, admission, reconstruction, movement, and repeated-cycle Spiker-firing work;
- development-only Level 7 Spiker-speed calibration support;
- recording/playback metadata support for deterministic diagnostic startup state;
- existing presentation-only --speed support;
- the durable TEMP/myNotes.md exclusion in .github/copilot-instructions.md.

Preserve all of it.

If the source/test inventory materially differs from the preceding report, STOP and report the discrepancy before editing.

Do not reset, restore, clean, checkout over changes, stage, commit, push, or discard anything.


MANUAL RESULTS AND OBSERVATIONS

Preserve the following evidence classifications.

MANUAL C# CALIBRATION RESULT:

- Normal/current Level 7 Spiker movement looked too fast to Neal.
- Fixed Level 7 Spiker speeds 3.000, 2.8125, and 2.625 also looked too fast.
- Spikers continued firing correctly during repeated ascent cycles in the latest manual test.

MANUAL C# OBSERVATION:

At fixed Spiker speed 3.000, Spikers traveled at the same speed as their shots. When a Spiker reversed direction, the continuing shot created a visually interesting effect resembling the Spiker releasing the shot.

Neal has explicitly rejected that effect as the fidelity target because it does not reflect the observed arcade/MAME behavior.

MAME/ARCADE MANUAL OBSERVATION:

- Spiker movement appears to match Tanker movement.
- Spiker-shot movement appears to match Tanker-shot movement.
- A Spiker shot travels faster than its firing Spiker and visibly pulls away.

These are qualitative relationship observations.

Do not infer an exact numerical arcade speed or ratio unless the local ROM evidence directly establishes one.


PRIMARY GOAL

Create the smallest practical DEVELOPMENT-ONLY diagnostic mode that tests this exact relationship:

- Level 7 Spiker movement uses the same effective speed/rate as the existing C# Tanker movement used in the comparable Level 7 gameplay context.
- Spiker shots use the same effective speed/rate as existing C# Tanker shots.

Do not guess or hard-code values before inspecting the actual implementation.

First determine:

1. How Tanker movement speed is currently selected.
2. Whether Tanker movement uses a fixed value, a range, a per-enemy random selection, a level-derived value, or another mechanism.
3. Which Tanker behavior is comparable to an ascending Spiker moving toward the rim.
4. Whether Tanker and Spiker shots already share the same EnemyBullet implementation and movement value.
5. Whether there is any actual C# distinction between a Spiker shot and a Tanker shot.
6. Whether Tanker movement changes by level or enemy lifecycle state.
7. Whether matching Tanker behavior requires using a value, an existing calculation, or an existing speed-selection policy.

Report these findings before describing the experimental implementation.

Do not assume that Tanker speed is one fixed number.

Do not assume that Tanker-shot and Spiker-shot speeds differ.

If Tanker and Spiker shots already use the same production movement path and speed, preserve that implementation unchanged and explicitly report that the shot-speed half of Neal's proposed relationship is already true in C#.


DEVELOPMENT-ONLY DIAGNOSTIC INTERFACE

Prefer one clear diagnostic option representing the relationship being tested, conceptually similar to:

--level7-spiker-match-tanker-speed

The exact name may follow existing project conventions after inspection.

The option must:

- affect only Level 7 Spiker movement;
- derive or select Spiker movement from the actual existing Tanker movement behavior;
- leave Tanker movement itself unchanged;
- leave shot speed unchanged if Tanker and Spiker shots already use the same implementation;
- preserve repeated-cycle Spiker firing;
- preserve recording/playback determinism;
- remain inactive when absent.

Do not silently reinterpret the existing numeric:

--level7-spiker-speed <value>

Keep the numeric development-only override available unless a direct technical conflict makes that unsafe.

The relationship-based Tanker-match option and the numeric Spiker override must be mutually exclusive.

Reject commands specifying both.

If the Tanker-match experiment requires deterministic startup metadata for faithful recording/playback, represent it explicitly in recording metadata using the smallest maintainable backward-compatible design.

Do not encode the result as an unexplained numeric Spiker override if doing so would lose the semantic meaning that the recording used the Tanker-match experiment.


TANKER SPEED SEMANTICS

The experiment must use an actual Tanker movement rate or policy already present in C#.

Do not invent a new "Tanker-like" speed.

If Tanker speed is randomized or otherwise varies, choose the smallest experiment that faithfully tests Neal's proposed relationship.

Possible valid outcomes after inspection include:

A. FIXED TANKER SPEED

If Tankers use one fixed relevant movement value, use that exact value for Level 7 Spikers in the diagnostic mode.

B. PER-ENEMY TANKER SPEED SELECTION

If Tankers select movement speed per enemy, reuse the equivalent Tanker selection calculation or policy for each diagnostic Spiker without consuming randomness incorrectly.

C. LEVEL-DERIVED TANKER SPEED

If Tanker movement is derived from level/progression data, use the actual Level 7 Tanker derivation.

D. MULTIPLE TANKER MOVEMENT PHASES

If Tankers have different movement rates in different lifecycle phases, identify which phase corresponds to an ascending Spiker moving toward the rim.

Do not choose among these possibilities without inspecting the code and tests.

Do not broadly share or merge Spiker and Tanker classes merely to implement this diagnostic.


SHOT SPEED SEMANTICS

Determine whether Spiker shots and Tanker shots already use:

- the same EnemyBullet type;
- the same Advance method;
- the same per-step displacement;
- the same update cadence.

If they already match, do not change projectile code or projectile speed.

Add focused evidence demonstrating that the existing relationship is the same.

If they do not match, do not immediately modify production projectile behavior.

Instead:

1. document the exact difference;
2. implement the smallest DEVELOPMENT-ONLY path needed to make Spiker shots use the Tanker-shot behavior during this diagnostic;
3. leave normal behavior unchanged;
4. preserve all other enemy projectile behavior.

Do not globally change enemy-projectile speed unless inspection proves there is one shared value and changing it would be necessary for the experiment. If a global change would affect Tanker shots or other accepted behavior outside the intended experiment, STOP and report the conflict instead of broadening scope.


REPEATED-CYCLE FIRING PROTECTION

The latest manual testing confirmed that Spikers continue firing during repeated ascent cycles.

This accepted candidate behavior must remain intact.

The diagnostic must not alter:

- firing eligibility;
- ammunition reset/replenishment;
- firing holdoff;
- firing cadence;
- ascent/retreat lifecycle transitions;
- persistent Spike growth;
- repeated-cycle firing restoration.

Add or retain focused regression evidence that a diagnostic Tanker-speed-matched Spiker continues firing during repeated ascent cycles.


RECORDING AND HALF-SPEED OBSERVATION

Recording and half-speed observation are required parts of this experiment.

The preceding milestone established:

- --speed controls presentation pacing rather than gameplay-state values;
- --speed 0.5 targets half-speed presentation;
- diagnostic Spiker startup state can be preserved in recording metadata;
- playback can restore deterministic diagnostic state while selecting its own presentation speed.

Retain those semantics.

Neal must be able to run a command conceptually equivalent to:

dotnet run --project src\Tempest.Game -- --level 7 --lives 10 --speed 0.5 --record-gameplay Temp\GameplayRecordings\lvl7-SpikerTankerMatch.json --level7-spiker-match-tanker-speed

Playback must restore the recorded Tanker-match diagnostic automatically.

Neal must be able to play the recording at half presentation speed with a command conceptually equivalent to:

dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\lvl7-SpikerTankerMatch.json --speed 0.5

If the implemented option name differs, provide the exact actual commands.

Do not require Neal to repeat the Tanker-match option during playback if the recording metadata restores it.

Reject a conflicting direct diagnostic override alongside --playback-file where necessary to preserve recorded simulation identity.

Preserve compatibility with existing recordings that do not contain this new diagnostic state.


MANUAL ACCEPTANCE TARGETS

The diagnostic is ready for Neal when the exact commands allow evaluation of:

1. SPIKER VERSUS TANKER MOVEMENT

Does the Spiker visibly travel at the same speed as a comparable Tanker?

2. SPIKER SHOT VERSUS TANKER SHOT

Do the two shots visibly use the same movement speed/rate?

3. SHOT VERSUS FIRING SPIKER

Does the Spiker shot visibly pull away from its firing Spiker rather than remaining attached to it or being overtaken?

4. REVERSAL PRESENTATION

When the Spiker reverses, does the already-fired shot continue naturally without producing the rejected equal-speed "release" effect?

5. REPEATED FIRING

Does the Spiker continue firing during later ascent cycles?

6. LEVEL 7 GAMEPLAY PRESSURE

Does ordinary Level 7 move closer to Neal's arcade/MAME experience without altering unrelated concurrency, cargo, Flipper movement, Tanker behavior, or projectile behavior?


OUT OF SCOPE

Do not:

- make Tanker-match behavior the production default;
- remove the existing numeric Spiker diagnostic;
- change normal Level 7 production Spiker selection;
- change Tanker production movement;
- change accepted Flipper movement;
- change Tanker-cargo movement;
- change concurrency;
- change admission;
- change reconstruction;
- change enemy supply;
- change projectile firing cadence;
- change Blaster behavior;
- alter Levels 1-6;
- start Level 8;
- broadly refactor gameplay classes;
- commit or push.

Do not perform the previously discussed blind literal value swap merely because this experiment compares Spiker and Tanker relationships.

This experiment supersedes the arbitrary swap as the immediate next test.

Any production decision requires Neal's separate manual acceptance.


AUTOMATED VALIDATION

Add tightly scoped automated evidence proving, as applicable:

1. The new Tanker-match diagnostic option parses correctly.
2. Repeated/conflicting occurrences are rejected.
3. Combining Tanker-match mode with --level7-spiker-speed is rejected.
4. The diagnostic is confined to Level 7.
5. With no diagnostic option, normal Level 7 Spiker speed selection is unchanged.
6. Diagnostic Spikers use the actual relevant Tanker movement value or policy.
7. Tanker movement itself remains unchanged.
8. Spiker shots and Tanker shots use the same movement behavior during the experiment.
9. Existing projectile behavior is unchanged if both shot types already shared it.
10. Repeated-cycle Spiker firing remains intact.
11. Recording preserves/restores the Tanker-match diagnostic state.
12. Playback restores the experiment without requiring the option again.
13. Existing recordings without the new diagnostic metadata remain compatible.
14. Half-speed recording and playback remain presentation-only.
15. Existing numeric 1.0-3.0 Spiker calibration remains available and unchanged.

Run bounded relevant sets covering:

- command-line/debug-controller parsing;
- Level 7 Spiker movement;
- Tanker movement;
- enemy projectile movement;
- repeated-cycle Spiker firing;
- Level 7 production and admission;
- gameplay recording/playback;
- relevant simulation tests;
- Level 7 regression subset;
- recording/playback regression subset;
- solution build;
- git diff --check.

Do not launch interactive Tempest.

Do not perform manual gameplay acceptance.

Follow the project's safe-stop rules for security blocks, hangs, stalls, timeouts, or non-progress.

A blocked, canceled, hung, stalled, or timed-out command is not a pass.


EVIDENCE CLASSIFICATION

Keep these distinctions explicit:

MAME/ARCADE MANUAL OBSERVATION:
Neal observes matching Spiker/Tanker movement and matching Spiker-shot/Tanker-shot movement, with shots faster than the firing enemies.

MANUAL C# CALIBRATION RESULT:
Normal/current, 3.000, 2.8125, and 2.625 Spiker movement looked too fast.

MANUAL C# OBSERVATION:
At fixed Spiker speed 3.000, Spiker and shot moved together until reversal created the rejected visual "release" effect.

C# MEASURED:
Exact values, policies, code paths, and deterministic test results found in the current implementation.

PROJECT TEST HYPOTHESIS:
Matching C# Spiker movement to existing C# Tanker movement may reproduce the observed arcade/MAME relationship.

PROJECT DECISION:
None until Neal manually evaluates the diagnostic.

Do not claim that the C# Tanker value is automatically ROM-correct or arcade-correct.


GIT AND WORKTREE SAFETY

This is not a closeout task.

Preserve the accumulated candidate exactly.

Do not:

- stage;
- commit;
- push;
- reset;
- restore;
- clean;
- checkout over changes;
- discard candidate work.

At completion show:

git status --short --branch

Clearly distinguish pre-existing changes from files changed specifically for this diagnostic.


REQUIRED AI LOGS

Save the complete substantive prompt under:

AI-Logs/Prompts/

using a descriptive date-prefixed filename.

Create the implementation report at exactly:

AI-Logs/Reports/2026-11-11-level7-spiker-tanker-speed-match-diagnostic-report.md

The task is not complete until both files exist and the report has been read back or otherwise explicitly verified.

AI-Logs remain ignored historical/working artifacts and must not be staged or committed.


REQUIRED REPORT CONTENT

The report must include:

- starting Git status;
- checkpoint/worktree comparison;
- files inspected;
- exact current Tanker movement implementation;
- exact relevant Level 7 Tanker movement value, range, or policy;
- exact current Spiker movement implementation;
- exact Tanker-shot implementation;
- exact Spiker-shot implementation;
- whether Tanker and Spiker shots already matched before this experiment;
- diagnostic interface and semantics;
- recording metadata decision;
- backward-compatibility findings;
- confirmation repeated-cycle Spiker firing is preserved;
- tests and exact results;
- build result;
- git diff --check result;
- exact manual recording command;
- exact playback command;
- files changed specifically by this task;
- final Git status;
- explicit confirmation normal Level 7 production behavior remains unchanged;
- explicit confirmation Tanker production movement remains unchanged;
- explicit confirmation no production projectile behavior was changed;
- explicit confirmation no permanent Spiker speed was selected;
- explicit confirmation no commit or push occurred;
- explicit confirmation interactive Tempest was not launched;
- manual acceptance status: PENDING NEAL.


COMPLETION REQUIREMENTS

Before claiming completion:

1. Verify the prompt log exists.
2. Verify the required report exists.
3. Read back or verify the report.
4. Show the final Git status.
5. State whether the result is complete, partial, or blocked.

If the report cannot be created, do not claim completion.

If implementation or validation is blocked, create a PARTIAL or BLOCKED report recording the exact blocker and remaining work.


STOP CONDITION

After implementing the development-only Tanker-match diagnostic, completing bounded validation, saving the prompt/report records, and providing the exact recording/playback commands, STOP.

Do not launch Tempest.

Do not make the diagnostic production behavior.

Do not change unrelated gameplay.

Wait for Neal's manual half-speed recorded comparison.

TEMPEST_LEVEL7_SPIKER_TANKER_SPEED_MATCH_DIAGNOSTIC_20261111