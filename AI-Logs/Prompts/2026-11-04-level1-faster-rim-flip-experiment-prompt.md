FAIL-CLOSED COMPLETENESS CHECK

Read this entire user-provided task before using any tools, reading files, running commands, editing files, staging files, or taking any repository action.

Before doing anything else, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context may appear after the user's completion token and do not invalidate it.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project or repository action.


TEMPEST LEVEL 1 FASTER RIM-FLIP EXPERIMENT

Perform one deliberately small gameplay experiment to determine whether faster Flipper movement around the rim explains Neal's remaining arcade-fidelity discrepancy.

This is an EXPERIMENTAL tuning milestone.

Do not commit.
Do not push.
Do not update durable project authority documents.
Do not launch the game.

Neal will manually compare the result.


STARTING STATE

The accepted production baseline remains:

3bebf42cbe9514846ab32bf4c8a12bcfdb048da7
Implement accepted production Level 7

However, the worktree may currently contain the unaccepted player-shot/Flipper contact-ordering experiment.

That experiment has now FAILED Neal's manual acceptance:

- no perceptible improvement to the excessive stationary-defense problem;
- shot timing/feel seems subtly wrong;
- Neal does not want to keep that production behavior.

Before implementing rim-speed tuning:

1. Inspect:
   git status --short --branch
   git rev-parse HEAD
   git diff --stat
   git diff --name-status
   git diff
   git ls-files --others --exclude-standard

2. Identify the complete uncommitted player-shot/Flipper contact-ordering experiment from:
   AI-Logs/Reports/2026-11-04-player-shot-flipper-contact-ordering-implementation-report.md

3. Revert ONLY that rejected production experiment and its associated experimental regression modifications back to accepted HEAD behavior.

The rejected experiment included production changes in:
- src/Tempest.Game/TempestGame.cs
- src/Tempest.Game/Gameplay/Flipper.cs

and associated changes/additions in:
- tests/Tempest.Game.Tests/SimulationTests.cs
- tests/Tempest.Game.Tests/Level2CanFlipTests.cs
- tests/Tempest.Game.Tests/Level1StationaryRimCombatComparisonTests.cs

The Level1StationaryRimCombatComparisonTests.cs file originated as an analysis harness before the production experiment. Preserve useful test-only rim-pressure investigation material if it can be cleanly separated from the rejected production-regression expectations.

Do NOT blindly delete useful investigation coverage simply because the final ordering experiment failed.

Do not disturb ignored AI-Logs.

If the worktree no longer corresponds cleanly to the documented ordering experiment, STOP rather than discarding uncertain work.


MANUAL RESULT TO PRESERVE

Record in the new ignored experiment report:

ARCADE / MANUAL OBSERVATION:

Neal A/B tested the accepted baseline and the player-shot/Flipper ordering experiment.

The ordering experiment did not produce a noticeable improvement in rim-pressure/camping behavior.

The experimental version also made shot timing/feel seem subtly wrong.

Therefore the production ordering experiment is rejected.

Do not update README.md, TEMPEST-BEHAVIOR-SPEC.md or TEMPEST-DEVELOPMENT-NOTES.md yet.


MASTER ROM SOURCE

The sole disassembly authority is:

Reference/arcade/Tempest Rom Source code.md

Do not search the Internet or use another Tempest disassembly.

Relevant previous ROM finding:

- Level 1 Flippers climb using their below-rim program.
- Upon reaching the rim they enter a separate top pursuit program.
- The ROM uses a top-rim acceleration parameter (`flip_top_accel`).
- For Level 1 Medium behavior, the traced value is 2.
- Easy changes the early-level value from 2 to 1.
- The exact ROM-to-C# absolute lane-turn duration remains unresolved because required ROM angle information is not decoded sufficiently for a literal time conversion.

Therefore this task does NOT claim that any selected C# numeric speed is exact ROM timing.


NEAL'S NEW ARCADE OBSERVATION

Treat this as ARCADE OBSERVED:

On Neal's original arcade machine, Flippers move substantially faster once they reach the rim.

This is particularly obvious on Level 1:

- a Level 1 Flipper climbs comparatively slowly from the bottom;
- once it reaches the rim, its lane-to-lane flipping/pursuit is visibly much faster.

As the levels increase, the contrast between below-rim climbing and rim movement becomes less visually obvious.

Neal believes excessively slow C# rim pursuit may be a major reason that stationary lane defense is too easy.

This observation is consistent with the ROM having distinct top-rim movement processing and `flip_top_accel`, but the numeric C# mapping remains a PROJECT EXPERIMENT.


EXPERIMENTAL TARGET

Change ONLY Level 1 Flipper angular turning speed while the Flipper is pursuing the Blaster on the rim.

Current measured C# Level 1 behavior from the deterministic investigation:

- Rim pause: 24 simulation ticks.
- Rim angular turn step: 8 degrees/update.
- Actual directed adjacent-lane Level 1 rim sweep in the tested geometry: approximately 156.22 degrees.
- This required approximately 20 turning updates after the pause.

For this first experiment:

- KEEP the existing 24-tick rim pause unchanged.
- KEEP all below-rim movement unchanged.
- KEEP rim targeting/direction logic unchanged.
- KEEP collision behavior at accepted baseline behavior.
- DOUBLE ONLY the Level 1 rim angular turn rate from 8 degrees/update to 16 degrees/update.

This 16-degree value is a PROJECT EXPERIMENT.

It is NOT claimed to be a literal ROM value.

The purpose is to isolate Neal's visual observation that the rim flip itself is substantially faster than the C# implementation.


DO NOT APPLY THIS GLOBALLY YET

For this first experiment, Level 1 only.

Do not alter Levels 2-7 yet.

The arcade observation suggests that the relationship may vary by level, and the ROM uses level-derived parameters.

If Level 1 testing strongly supports faster rim movement, later work can investigate appropriate per-level rim acceleration for Levels 2-7.

Do not assume every level should use exactly 2x C# angular speed.


DO NOT CHANGE

Do not change:

- Blaster firing cadence;
- held-Fire behavior;
- shot cooldown;
- number of player shots;
- shot speed;
- shot lifetime;
- shot collision tolerance;
- shot/pivot lane rules;
- accepted baseline shot/contact ordering;
- Flipper below-rim climb speed;
- Level 1 below-rim straight climb;
- rim pause duration;
- rim targeting;
- pursuit direction;
- other-rim-enemy logic;
- Level 2-7 rim speed;
- Level 7 2-3-2-2-2 below-rim route;
- enemy firing;
- difficulty;
- wave supply;
- Super Zapper;
- spinner behavior;
- audio;
- geometry;
- progression;
- rendering;
- death/re-entry;
- exit behavior.

Do not add anti-camping behavior.

The only intentional production gameplay difference should be:

LEVEL 1 RIM FLIP ANGULAR RATE:
8 degrees/update -> 16 degrees/update.


IMPLEMENTATION QUALITY

Implement the Level-1-specific experiment through the existing configuration/tuning architecture if practical.

Do not hard-code:
if (level == 1) ...

inside Flipper movement if the existing LevelConfiguration/FlipperConfiguration system can express the experiment cleanly.

Keep Levels 2-7 configuration exactly unchanged.

Use a concise comment if necessary:

- arcade observation: rim Flippers visibly accelerate relative to below-rim Level 1 motion;
- ROM support: separate top-rim processing with Level 1 `flip_top_accel = 2`;
- C# 16-degree rate: PROJECT EXPERIMENT, not literal ROM timing.

Do not over-comment.


TESTS

Add/update the minimum focused tests necessary to prove:

1. Level 1 below-rim turn/climb behavior is unchanged.

2. Level 1 rim turn rate is now 16 degrees/update.

3. Existing Level 1 rim pause remains exactly 24 ticks.

4. Level 2-5 remain at their existing rim timing values.

5. Levels 6-7 remain at their existing rim timing values.

6. Level 7's accepted below-rim 2-3-2-2-2 route remains unchanged.

7. Player firing cadence/capacity/speed remain unchanged.

8. Accepted baseline player-shot/Flipper contact behavior was restored after rejecting the ordering experiment.

9. Death/contact behavior retains accepted baseline semantics.

10. Ordinary progression and Level 7 terminal behavior remain intact.

If the preserved stationary-pressure harness can measure the faster Level 1 rim turn without becoming brittle, add an experimental comparison result.

Do not manufacture an assertion that stationary play must die after a specific tick.


AUTOMATED VALIDATION

Run focused tests covering:

- Level 1 Flipper movement;
- rim pursuit;
- shot/Flipper collision;
- player firing;
- Level 2 movement;
- Levels 3-7 regression seams;
- Level 7 movement;
- progression;
- death;
- Super Zapper as an unchanged neighboring system.

Then run:

dotnet test TempestCSharp.slnx
dotnet build TempestCSharp.slnx
git diff --check

Record actual counts.

Do not treat zero discovered tests as success.


DOCUMENTATION

Do not modify:

- README.md
- TEMPEST-BEHAVIOR-SPEC.md
- TEMPEST-DEVELOPMENT-NOTES.md

This is an experiment pending Neal's manual judgment.

Save the substantive prompt and paired report under ignored AI-Logs according to project instructions.

Suggested names:

AI-Logs/Prompts/2026-11-04-level1-faster-rim-flip-experiment-prompt.md

AI-Logs/Reports/2026-11-04-level1-faster-rim-flip-experiment-report.md


MANUAL ACCEPTANCE

Do not launch the game.

After validation, give Neal:

dotnet run --project src\Tempest.Game -- --lives 10

Neal should focus on Level 1.

Do not ask him to quantify frames initially.

Ask him to judge:

1. Is the increased rim-turn speed immediately visible?

2. Does the relationship now look more like the arcade:
   relatively slow Level 1 climb, followed by distinctly faster rim pursuit?

3. Does stationary single-tapping become noticeably less sustainable?

4. Does holding Fire remain too safe, or does the faster Flipper now create meaningful pressure?

5. Can a skilled player still temporarily defend one lane?

6. Does careful shot timing still matter and work?

7. Do multiple rim Flippers now force movement more often?

8. Does normal spinner gameplay still feel fair rather than artificially difficult?

9. Does anything else about Level 1 feel broken or unnaturally fast?

The experiment does NOT need to perfectly solve the camping problem to be informative.

The key questions are:

- is faster rim movement clearly closer to the arcade?
- is 2x too slow, about right, or too fast?
- does it materially affect stationary defense?


A/B ITERATION

If Neal reports:

TOO SLOW:
do not immediately choose another value without review.

ABOUT RIGHT:
preserve the experiment and plan a per-level ROM/arcade-informed rim-speed milestone.

TOO FAST:
preserve the evidence and choose a smaller experimental adjustment in a separate step.

If it looks arcade-correct but camping remains too effective, then rim speed is real but not the only remaining contributor.

Do not automatically combine multiple tuning changes.


FINAL GIT STATE

Do not stage.
Do not commit.
Do not push.

At completion run:

git status --short --branch
git rev-parse HEAD
git diff --stat
git diff --name-status
git diff --check
git ls-files --others --exclude-standard

Report:

- rejected ordering-experiment changes removed;
- exact Level 1 rim-speed experiment files;
- preserved test-only analysis artifacts;
- HEAD still at 3bebf42;
- no authority/reference changes;
- no commit/push.


FINAL RESPONSE

Return:

1. Confirmation that the rejected shot/contact experiment was removed.
2. Exact resulting Level 1 rim-speed change.
3. Confirmation the 24-tick pause stayed unchanged.
4. Confirmation Levels 2-7 rim settings stayed unchanged.
5. Focused test counts.
6. Full-suite count.
7. Build warnings/errors.
8. git diff --check.
9. Files changed/untracked.
10. Manual launch command.
11. Short Level 1 acceptance checklist.
12. Report filename.
13. Final Git status.
14. Confirmation no authority/reference changes, game launch, commit, push, or Internet disassembly research occurred.

Stop for Neal's manual test.

TEMPEST_LEVEL1_FASTER_RIM_FLIP_EXPERIMENT_COMPLETE