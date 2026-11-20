Implement the approved rimit / hurry-up milestone for Tempest.

Repository:
C:\Users\nrnob\OneDrive\Repro\Tempest

Starting clean checkpoint:
37535eb Implement Pulsar behavior and add behavior specification

FIRST read:

AI-Logs/Reports/2026-10-30-next-milestone-after-pulsar-report.md
TEMPEST-BEHAVIOR-SPEC.md

Also inspect the current C# implementation, tests, and literal Incitti reference
before editing.

If the analysis report conflicts with current code or literal source, follow the
stronger evidence and document the discrepancy.

SCOPE

Implement only the Incitti-derived rimit hurry-up behavior for the existing
direct-hatched Fuseball and Pulsar development scenarios.

Do not implement:
- Super Zapper
- source onrim-based level exit
- Level progression
- Level 2 or canflip
- new level geometry or composition
- Tanker-carried Pulsars or Fuseballs
- enemy-bullet promotion
- new rendering
- new scoring
- unrelated fixes or refactors

Do not commit.

BEHAVIOR

Add one latched hurry-up state owned by the game/simulation.

The source-derived trigger must require:
- reserve Eggs are exhausted;
- active Eggs are exhausted;
- at least one active enemy remains;
- at least one active enemy is below Level.Depth.

Determine the correct evaluation point from the CURRENT update order.

The implementation and tests must establish the exact trigger tick relative to:
- Egg approach/hatching;
- removal of the last active Egg;
- Fuseball/Pulsar updates;
- completion checks.

Do not accept a test that merely proves hurry-up eventually activates.

LIFECYCLE

Hurry-up must:
- begin false on new game/level initialization;
- latch true once its trigger becomes true;
- remain true for the rest of that same-level attempt;
- survive the existing player-death freeze;
- survive ordinary same-level enemy reconstruction/re-entry;
- clear on full game restart or replay initialization.

Do not replenish, clear, or recalculate it merely because the player loses a life.

FUSEBALL

Pass hurry-up state into the existing Fuseball update behavior without creating
a second global or random source.

During hurry-up:
- at a lateral-boundary phase decision, force the climbing state even when the
  ordinary controlled random result would choose descending;
- during climbing, apply the existing source climb term twice;
- with the current development speed of 3.3, the resulting climb increment is
  6.6 per climbing update;
- clamp correctly at the rim;
- do not change descending speed, lateral cadence, collision behavior, scoring,
  animation, or accounting.

PULSAR

Pass hurry-up state into the existing Pulsar update behavior.

During hurry-up:
- at the top endpoint, force the lane-flip branch even when the ordinary
  controlled random result would choose descending;
- preserve the existing pause, lane transfer and flip behavior;
- do not double or otherwise change Pulsar vertical movement speed;
- do not change the shared pulse oscillator;
- do not change pulsing-lane collision or rendering.

COMPLETION AND ACCOUNTING

Do not copy Incitti's onrim-based exit rule.

The current C# project decision remains:
- enemies must still resolve through the existing kill-all completion policy;
- hurry-up does not award points;
- hurry-up does not increment destroyed count;
- hurry-up does not remove or resolve enemies;
- hurry-up does not alter reserve, active-Egg, or enemy-accounting totals;
- hurry-up alone cannot start tube exit.

MAINTAINABILITY

Use focused comments for non-obvious rules:
- why the state latches;
- why it persists through ordinary life reconstruction;
- why Fuseball climb applies the existing increment twice;
- why Pulsar is redirected at the top without receiving a speed increase;
- why source onrim-based exit behavior is deliberately not copied.

Avoid obvious or excessive comments.

Do not introduce a generic hurry-up subsystem or new command-line flag.
Prefer a simple read-only game state passed into the existing enemy update paths.

TESTS

Add deterministic tests covering:

Trigger:
- false while reserve Eggs remain;
- false while active Eggs remain;
- false when no active enemies remain;
- false when all active enemies are at the rim;
- true only when both Egg sources are exhausted and at least one active enemy is
  below Level.Depth;
- exact activation tick relative to the current update order;
- once true, remains latched even if the immediate trigger later becomes false.

Accounting:
- no score change;
- no destroyed-count change;
- no reserve or active-Egg distortion;
- no premature completion or exit;
- ordinary completion behavior remains unchanged.

Fuseball:
- ordinary controlled boundary result can select descending before hurry-up;
- the same result is forced to climbing during hurry-up;
- climb increment is exactly twice the existing source climb term;
- current 3.3 development value produces 6.6;
- rim clamp remains correct;
- descending, lateral cadence, collisions, scoring and animation regressions remain
  unchanged.

Pulsar:
- ordinary top-end controlled result can select descending before hurry-up;
- the same result takes the lane-flip branch during hurry-up;
- one-unit vertical movement is unchanged;
- pause/lane-transfer behavior remains correct;
- shared pulse timing and rendering remain unchanged.

Lifecycle:
- hurry-up freezes/persists through PlayerDying;
- remains latched after same-level Egg reconstruction;
- reconstructed Fuseballs/Pulsars experience the latched state at the defined
  first eligible update;
- full restart/replay clears the latch.

Regression:
- normal Level 1 unchanged;
- Spiker and Tanker scenarios unchanged;
- existing Fuseball and Pulsar tests remain green;
- existing completion and death/reconstruction tests remain green.

VALIDATION

Run:
- focused hurry-up/Fuseball/Pulsar tests;
- full automated test suite;
- build;
- git diff --check.

Record exact pass/fail/skip counts and build warnings/errors.

Do not claim manual validation.

DOCUMENTATION

Update README.md and TEMPEST-DEVELOPMENT-NOTES.md to describe the new
source-derived hurry-up behavior in the development scenarios.

Do not update TEMPEST-BEHAVIOR-SPEC.md yet.

The behavior specification is updated only after:
- implementation review;
- automated validation;
- Neal's manual validation.

Save this exact prompt to:

AI-Logs/Prompts/2026-10-30-rimit-hurry-up-implementation-prompt.md

Save the implementation report to:

AI-Logs/Reports/2026-10-30-rimit-hurry-up-implementation-report.md

The report must include:
1. exact trigger and evaluation order;
2. latch/reset/death lifecycle;
3. Fuseball behavior;
4. Pulsar behavior;
5. completion/accounting preservation;
6. files changed;
7. tests added;
8. focused/full/build/diff-check results;
9. manual validation command/checklist;
10. explicit out-of-scope behavior;
11. remaining arcade uncertainties.

STOP after implementation, validation, documentation, and report generation.

Do not commit.
Do not start Super Zapper or another milestone.
