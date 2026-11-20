FAIL-CLOSED COMPLETENESS CHECK

Read this entire user-provided task before using any tools, reading files, running commands, editing files, staging files, or taking any repository action.

Before doing anything else, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context may appear after the user's completion token and do not invalidate it.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project or repository action and explain why.


TEMPEST DEVELOPMENT-ONLY RIM-SPEED MULTIPLIER

Implement a temporary DEVELOPMENT-ONLY command-line control that lets Neal interactively compare different Flipper rim-turn speeds without repeatedly modifying source code.

The purpose is diagnostic:

- determine whether Flipper rim-turn speed materially explains the remaining arcade-fidelity discrepancy;
- determine what range produces a clearly perceptible difference;
- determine whether faster rim turning makes stationary lane defense less sustainable;
- obtain Neal's perceptual estimate of an arcade-like rim speed before choosing any permanent production value.

This task must NOT declare any experimental multiplier arcade-correct.

Do not commit.
Do not push.
Do not update durable project authority documents.
Do not launch the game.

Stop after automated validation and provide commands for Neal's manual testing.


ACCEPTED BASELINE

The accepted production baseline remains:

3bebf42cbe9514846ab32bf4c8a12bcfdb048da7
Implement accepted production Level 7

The worktree may contain a later experimental Level 1 faster-rim-flip change and/or test/harness changes.

Before editing anything, inspect:

git status --short --branch
git rev-parse HEAD
git log -1 --oneline
git diff --stat
git diff --name-status
git diff
git diff --cached --stat
git ls-files --others --exclude-standard
git diff --check

Verify HEAD remains the accepted baseline.

Do not reset, clean, discard, restore, delete, stage or commit anything blindly.

Classify every current worktree change before modifying it.


REJECTED SHOT/CONTACT EXPERIMENT MUST STAY REJECTED

Read:

AI-Logs/Reports/2026-11-04-player-shot-flipper-contact-ordering-implementation-report.md

Neal manually A/B tested that experiment and rejected it:

- it did not perceptibly improve the excessive stationary-defense problem;
- the experimental build made shot timing/feel seem subtly wrong.

The production shot/contact-ordering experiment must NOT be reintroduced.

The accepted baseline Blaster shot/contact behavior should remain in effect.

The rejected experiment previously touched:

- src/Tempest.Game/TempestGame.cs
- src/Tempest.Game/Gameplay/Flipper.cs
- tests/Tempest.Game.Tests/SimulationTests.cs
- tests/Tempest.Game.Tests/Level2CanFlipTests.cs
- tests/Tempest.Game.Tests/Level1StationaryRimCombatComparisonTests.cs

Do not assume every current difference in these files is rejected work. Inspect the present diff and the report carefully.

Preserve useful test-only analysis/harness material when it is clearly separable from the rejected production behavior.


CURRENT FASTER-RIM EXPERIMENT

Neal subsequently requested a small Level 1 faster-rim experiment.

The intended experiment was:

- Level 1 rim angular turn speed: 8 degrees/update -> 16 degrees/update.
- Level 1 24-tick rim pause unchanged.
- Below-rim movement unchanged.
- Levels 2-7 unchanged.
- All Blaster firing/shot behavior unchanged.

Neal manually tested that experiment and did NOT perceive a meaningful speed difference.

Do not interpret that as proof rim speed is irrelevant.

Instead, replace the fixed experimental speed adjustment with the runtime multiplier requested below so Neal can test deliberately large contrasts.

If the exact current worktree does not match that description, inspect and report what is actually present rather than forcing the expected state.


NEW DEVELOPMENT-ONLY COMMAND-LINE OPTION

Add:

--rim-speed <multiplier>

Examples:

dotnet run --project src\Tempest.Game -- --rim-speed 0.5
dotnet run --project src\Tempest.Game -- --rim-speed 1
dotnet run --project src\Tempest.Game -- --rim-speed 2
dotnet run --project src\Tempest.Game -- --rim-speed 4

Semantics:

--rim-speed 1

must mean the accepted production/default rim angular-turn speed for each level.

Examples:

--rim-speed 0.5
half the normal rim angular-turn rate.

--rim-speed 2
twice the normal rim angular-turn rate.

--rim-speed 4
four times the normal rim angular-turn rate.


CRITICAL SCOPE: WHAT THE MULTIPLIER AFFECTS

The multiplier affects ONLY Flipper angular movement while performing rim pursuit/turning.

It must NOT affect:

- below-rim Flipper angular movement;
- below-rim lane-change programs;
- Level 7's accepted 2-3-2-2-2 below-rim route;
- Flipper climb speed;
- Tanker climb speed;
- Tanker cargo release;
- Pulsar movement;
- Spiker movement;
- Fuseball movement;
- enemy firing;
- Blaster movement;
- Blaster firing;
- player-shot movement;
- player-shot capacity;
- shot cooldown;
- Super Zapper;
- global simulation speed;
- approach/exit animation;
- any non-rim gameplay timer.


RIM PAUSE MUST REMAIN INDEPENDENT

For this experiment, --rim-speed must NOT alter the existing rim pause.

Example:

If Level 1 normally pauses 24 ticks at the rim, then:

--rim-speed 0.5
--rim-speed 1
--rim-speed 2
--rim-speed 4

must all retain that same 24-tick Level 1 pause.

This separation is intentional.

Neal currently wants to determine whether the visible speed of the actual rim flip/pivot is relevant.

Do not combine rim angular speed and rim pause into one multiplier.


APPLY ACROSS IMPLEMENTED LEVELS 1-7

Unlike the previous fixed Level-1-only 16-degree experiment, the runtime --rim-speed multiplier should apply to the existing configured RIM angular turn rate for Flippers on all implemented Levels 1-7.

It must be multiplicative.

For example, conceptually:

effectiveRimTurnRate =
    configuredLevelRimTurnRate * diagnosticRimSpeedMultiplier

Do NOT replace each level's configured base rate with one universal rate.

Therefore Levels 1-7 retain their relative configured differences.

Only the diagnostic multiplier is shared.


DEFAULT BEHAVIOR MUST BE IDENTICAL

When --rim-speed is omitted:

dotnet run --project src\Tempest.Game -- --lives 10

must use multiplier 1.0 and behave exactly like the accepted/default rim-speed configuration.

Likewise:

dotnet run --project src\Tempest.Game -- --rim-speed 1 --lives 10

must be behaviorally equivalent with respect to rim speed.

The diagnostic option must not silently make the previous 16-degree Level 1 experiment the new default.

The normal/default Level 1 rim rate must return to its accepted pre-experiment value when multiplier = 1.


ARGUMENT VALIDATION

Implement sensible fail-fast validation.

Accept positive finite decimal values.

Reject:

- zero;
- negative values;
- NaN;
- infinity;
- missing numeric value;
- malformed numeric values.

Use invariant-culture parsing so a command-line decimal such as:

--rim-speed 2.5

has consistent meaning independent of Windows locale.

On invalid input:

- report a concise useful error;
- do not silently fall back to 1;
- do not run gameplay with an unintended value.

Do not invent an arbitrary narrow maximum unless technically necessary.

If extremely high multipliers expose an existing geometrical/step-size limitation, preserve safe behavior and document that limit rather than hiding it.


DEVELOPMENT-ONLY STATUS

This is a temporary diagnostic/development option.

It is not an arcade menu.

It is not DIP-switch emulation.

It is not a user-facing difficulty setting.

It is not a permanent game-design decision.

Do not display it in the ordinary gameplay HUD.

Do not add menu UI.

Do not persist the value between runs.

Do not tie it to score, lives, difficulty, or level selection.


IMPLEMENTATION DESIGN

Prefer the smallest clean implementation using the project's existing command-line and configuration/composition paths.

The multiplier should be parsed once at startup and flow explicitly into the gameplay configuration that requires it.

Avoid:

- global mutable static diagnostic state if a normal configuration path can carry the value;
- checking command-line arguments from inside Flipper.Update;
- duplicating Flipper movement code;
- adding level-number conditionals throughout gameplay;
- modifying every level constant individually;
- generalized runtime tuning infrastructure beyond what this diagnostic needs.

Keep the implementation maintainable enough that the diagnostic option can later be removed cleanly.


PROJECT COMMENTS

Add only concise maintainability-focused comments where necessary.

The important distinction is:

ARCADE OBSERVED:
Neal perceives substantially faster Flipper lane-to-lane pursuit after reaching the rim, especially on Level 1.

ROM-BACKED:
Atari uses separate top-rim processing and a level-derived top acceleration mechanism.

PROJECT DIAGNOSTIC:
--rim-speed is a C# experimental multiplier intended to identify whether angular rim speed materially affects the observed gameplay discrepancy.

Do not describe any multiplier value as arcade-correct before Neal establishes that manually.


TEST REQUIREMENTS

Add focused tests proving:

1. No --rim-speed argument produces multiplier 1.0.

2. --rim-speed 1 produces the accepted/default rim angular speed.

3. --rim-speed 0.5 produces exactly half the configured rim angular step.

4. --rim-speed 2 produces exactly twice the configured rim angular step.

5. --rim-speed 4 produces exactly four times the configured rim angular step.

6. A non-integer value such as 2.5 parses and applies correctly.

7. Zero is rejected.

8. Negative values are rejected.

9. Missing values are rejected.

10. Malformed values are rejected.

11. NaN/infinity are rejected.

12. Level 1 rim pause remains unchanged under different multipliers.

13. Below-rim Level 1 movement remains unchanged under different multipliers.

14. Level 2-6 below-rim behavior remains unchanged.

15. Level 7 2-3-2-2-2 below-rim movement remains unchanged.

16. Each implemented level's own configured base rim-turn rate is multiplied rather than replaced by a universal constant.

17. Player Fire cadence remains unchanged.

18. Player-shot capacity/speed remain unchanged.

19. Accepted baseline shot/contact ordering is in effect; the rejected ordering experiment is absent.

20. Super Zapper behavior remains unchanged.

21. Natural Level 1-7 progression remains intact.

22. The normal no-argument production path is behaviorally identical to multiplier 1 for all affected configuration values.


OPTIONAL OBSERVABILITY

If useful and very cheap, the development startup output may print a concise diagnostic line ONLY when --rim-speed is explicitly supplied, for example:

Rim speed multiplier: 4x

Do not clutter normal startup when the option is omitted.

This is optional. Do not add UI complexity merely to display the value.


AUTOMATED VALIDATION

Run focused tests covering:

- argument parsing;
- configuration propagation;
- Flipper below-rim movement;
- Flipper rim movement;
- rim pursuit;
- Levels 1-7 movement;
- Level 7 2-3-2-2-2 behavior;
- firing;
- player shots;
- Super Zapper;
- progression.

Then run an appropriate accepted Levels 1-7 regression set.

Then run:

dotnet test TempestCSharp.slnx
dotnet build TempestCSharp.slnx
git diff --check

Record actual nonzero test counts.

Build must complete with 0 errors.

Report warning count accurately.


DOCUMENTATION BOUNDARY

Do NOT update:

- README.md
- TEMPEST-BEHAVIOR-SPEC.md
- TEMPEST-DEVELOPMENT-NOTES.md

This remains an experimental diagnostic.

Save the substantive prompt and paired report under ignored AI-Logs according to .github/copilot-instructions.md.

Suggested:

AI-Logs/Prompts/2026-11-04-rim-speed-command-line-diagnostic-prompt.md

AI-Logs/Reports/2026-11-04-rim-speed-command-line-diagnostic-report.md


MANUAL TESTING

Do not launch the interactive game.

After automated validation, give Neal these commands:

BASELINE:

dotnet run --project src\Tempest.Game -- --lives 10

EXPLICIT BASELINE:

dotnet run --project src\Tempest.Game -- --rim-speed 1 --lives 10

HALF SPEED:

dotnet run --project src\Tempest.Game -- --rim-speed 0.5 --lives 10

DOUBLE SPEED:

dotnet run --project src\Tempest.Game -- --rim-speed 2 --lives 10

FOUR TIMES SPEED:

dotnet run --project src\Tempest.Game -- --rim-speed 4 --lives 10

Also tell Neal that arbitrary positive finite values are allowed, for example:

dotnet run --project src\Tempest.Game -- --rim-speed 3 --lives 10

dotnet run --project src\Tempest.Game -- --rim-speed 2.5 --lives 10


RECOMMENDED A/B PROCEDURE

Do NOT require Neal to test every value exhaustively.

The most informative first comparison is deliberately extreme:

1. Try 0.5.
2. Try 4.

If those do not produce an obvious visual/gameplay difference in rim Flipper pursuit, STOP and report that before trying to fine-tune 1.5 versus 2.

If 0.5 versus 4 clearly differs:

3. Compare 1 versus 2.
4. Try intermediate/higher values as Neal finds useful.
5. Identify a perceptual region rather than demanding false numeric precision.

Neal should judge:

- visible lane-to-lane rim flip speed;
- pressure from one pursuer;
- pressure from multiple pursuers;
- stationary single-tap sustainability;
- stationary held-fire sustainability;
- whether spinner movement becomes more necessary;
- whether rim pursuit feels closer to his arcade machine;
- whether any tested multiplier becomes obviously unfair or visually unnatural.

Do not interpret tiredness, hunger, caffeine, or a single unusually good/bad run as precise calibration evidence. Neal may repeat comparisons when he feels confident in his perceptual judgment.


OPEN-TUBE QUALIFICATION

The command-line multiplier may technically affect rim Flippers on any implemented level, but no final open-tube balance conclusion should be drawn from this experiment.

Neal's arcade observation is:

- endpoint defense on open tubes is legitimately stronger because enemies can approach from only one direction;
- it is still not indefinitely safe against substantial remaining pressure.

Levels 8+ are not implemented, so this is context for future work only.


WHAT SUCCESS MEANS

This diagnostic succeeds even if the answer is:

"Rim angular speed is not the important variable."

That would be useful evidence.

Possible outcomes include:

A. Large multiplier changes clearly affect both visible pursuit and camping safety.
   Rim angular speed is a meaningful contributor.

B. Large multiplier changes are visually obvious but camping remains too safe.
   Rim angular speed is real but not the principal cause.

C. Large multiplier changes are barely perceptible.
   Investigate rim pause/cadence or another mechanic before further speed tuning.

D. Faster pursuit creates the correct pressure but a specific value needs refinement.
   Use Neal's A/B observations for a later bounded per-level implementation.

Do not make a permanent production decision in this task.


GIT / WORKTREE

Do not stage.
Do not commit.
Do not push.

Before completion run:

git status --short --branch
git rev-parse HEAD
git diff --stat
git diff --name-status
git diff
git diff --check
git ls-files --others --exclude-standard

Confirm:

- HEAD remains 3bebf42cbe9514846ab32bf4c8a12bcfdb048da7;
- rejected shot/contact-order experiment has not returned;
- fixed Level 1 16-degree experiment has been replaced by the multiplier design rather than becoming the new default;
- no authority or reference document changed;
- no unintended gameplay constants changed;
- no generated/debug artifact was added;
- no commit/push occurred.


FINAL RESPONSE

Return:

1. Exact --rim-speed syntax.
2. Default behavior.
3. What the multiplier affects.
4. What it explicitly does not affect.
5. Confirmation that multiplier 1 restores accepted/default rim angular rates.
6. Confirmation that the rejected shot/contact experiment is absent.
7. Confirmation that the fixed Level 1 16-degree experiment is no longer the default.
8. Production files changed.
9. Test files changed.
10. Focused test results/counts.
11. Shared regression results/counts.
12. Full-suite result/counts.
13. Build warnings/errors.
14. git diff --check result.
15. Final Git status.
16. Ready-to-copy commands for 0.5, 1, 2, 4 and arbitrary values.
17. Report filename.
18. Confirmation no game launch, authority/reference edit, commit, push or Internet disassembly research occurred.

Stop for Neal's manual A/B testing.

TEMPEST_RIM_SPEED_COMMAND_LINE_DIAGNOSTIC_COMPLETE