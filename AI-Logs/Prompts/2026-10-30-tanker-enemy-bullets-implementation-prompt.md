Implement Tanker-fired EnemyBullets in the controlled Tanker development scenario.

This is a focused IMPLEMENTATION task.

Do not broaden the Tanker milestone.
Do not implement another enemy or general level progression.
Do not commit or push.

CURRENT VERIFIED CHECKPOINT

Git checkpoint:

7e25aea Add Tanker gameplay and improve code maintainability

Expected starting state:

- working tree clean;
- dotnet build passes;
- dotnet test reports 146 passed, 0 failed;
- Tanker/cargo behavior is implemented and manually validated;
- EnemyBullet behavior is implemented and manually validated for Flippers;
- source comments follow the project's long-term maintainability standard.

READ FIRST

Read:

1. .github/copilot-instructions.md
2. TEMPEST-DEVELOPMENT-NOTES.md
3. README.md
4. AI-Logs/Reports/2026-10-30-next-milestone-analysis-report.md
5. current Gameplay/Tanker.cs
6. current Gameplay/EnemyBullet.cs
7. current Gameplay/Flipper.cs
8. current TempestGame.cs
9. relevant current SimulationTests.cs sections
10. Reference/incitti/tempest.bmx Tanker and Bullets paths
11. Reference/incitti/Readme.txt as needed

Everything under Reference/incitti is reference-only.

Never modify anything under Reference/incitti.

GOAL

Enable Tanker-fired EnemyBullets only when both development options are present:

--tanker-test
--enemy-bullets-test

Example:

dotnet run --project src/Tempest.Game -- --tanker-test --enemy-bullets-test --lives 10

Required mode behavior:

- --tanker-test alone:
  - Tankers remain silent;
  - Flippers remain silent;
  - existing cargo-focused Tanker scenario remains unchanged.

- --enemy-bullets-test without --tanker-test:
  - existing Flipper bullet behavior remains unchanged.

- --tanker-test plus --enemy-bullets-test:
  - ordinary Flippers may fire using their existing behavior;
  - released cargo Flippers may fire using their existing behavior;
  - Tankers may fire using the source-backed Tanker rules below.

Do not add a new command-line flag.

SOURCE-BACKED TANKER FIRING RULES

Implement these Incitti-derived Tanker rules:

1. A newly created armed Tanker receives 1 or 2 potential successful shots.

2. Its initial bullet timer is:

   16 + random offset

3. Mirror the random-offset convention established by the current project after
   checking Incitti's Rand(64) behavior and the existing deterministic Random seam.

4. On each eligible Tanker update:

   - first advance the Tanker's climbing height;
   - only evaluate firing when the post-climb height is strictly below 380;
   - decrement the bullet timer;
   - when the timer becomes less than zero, reset it to 16;
   - make the firing decision;
   - create a bullet only when the inclusive random decision from 0 through 100 is
     strictly greater than 75;
   - consume one ammunition unit only when a bullet is actually created.

5. A failed firing decision:

   - does not consume ammunition;
   - leaves the timer reset to 16.

6. A successful bullet begins:

   - on the Tanker's current lane;
   - at the Tanker's post-climb height.

7. Do not fire when:

   - ammunition is zero;
   - the Tanker is unarmed because --enemy-bullets-test is absent;
   - the Tanker's post-climb height is 380 or greater;
   - cargo release is pending or complete;
   - the Tanker has already been destroyed.

8. Tankers do not fire during natural cargo release processing.

Use the existing EnemyBullet class unchanged unless a genuine source-backed defect
is discovered.

Do not create a Tanker-specific projectile class.

TANKER ENTITY DESIGN

Add only the Tanker state required for firing, such as:

- bullet ammunition;
- bullet timer.

Expose narrowly useful read-only properties for deterministic tests if consistent
with the current Flipper implementation.

Provide a small firing/update seam that allows TempestGame to receive an optional
EnemyBullet from a Tanker update.

Prefer an interface similar in intent to the existing Flipper update/firing path,
but do not force identical code structure if Tanker's lifecycle makes a smaller
design clearer.

The Tanker must not know about:

- TempestGame collections;
- player score;
- Raylib;
- global command-line options;
- Tanker accounting;
- collision-list insertion.

TempestGame remains responsible for deciding whether a Tanker is armed and for
registering any created bullet.

RANDOMNESS

Use the existing injected Random or deterministic decision approach.

Do not instantiate an independent Random inside Tanker.

The implementation must allow tests to control:

- ammunition selection;
- initial timer offset;
- firing decision.

Do not change existing Flipper random behavior.

TEMPESTGAME INTEGRATION

When both Tanker and enemy-bullet development options are enabled:

- create the Tanker with source-backed ammunition and timer values;
- collect any Tanker-created EnemyBullet;
- add it to the existing enemyBullets collection;
- add it to the existing development collision ordering at the correct creation
  point.

Preserve insertion-order projectile collision behavior.

The bullet is a separately active projectile after creation.

Destroying or naturally releasing the Tanker must not automatically remove an
already-fired bullet.

Existing bullet rules remain authoritative:

- travels outward toward the player;
- remains in its firing lane;
- can kill the player;
- can be shot for 25 points;
- does not participate in enemy accounting;
- does not prevent level completion;
- clears through the established player-death and exit paths.

Do not modify Tanker original-slot or cargo-unit accounting.

LIFE RESET

If an unresolved Tanker returns as a fresh Tanker-designated Egg after player death:

- the old Tanker instance and its unused ammunition/timer state are discarded;
- any active enemy bullets are cleared through the established death-reset behavior;
- the newly hatched Tanker receives fresh source-backed ammunition and initial timer
  values when both development options are enabled.

Do not preserve Tanker firing state across life loss.

If the Tanker original slot has already resolved, it must not return merely because
a Tanker bullet remains or was cleared.

UPDATE ORDER

Preserve the current global simulation ordering.

Tanker firing must occur as part of the existing Tanker update phase.

Do not move:

- player firing;
- player-shot collision;
- enemy-bullet movement;
- Egg processing;
- Flipper processing;
- Spiker processing;
- enemy-to-player collision;
- completion checks.

A Tanker-created bullet should not receive an unintended extra movement update on
its creation tick if the current global enemy-bullet update phase has already run.

Document this ordering if it is not already evident.

MAINTAINABILITY COMMENTS

Add concise comments explaining only non-obvious intent, including where useful:

- why the Tanker uses a delayed random initial firing timer;
- why the height check uses the post-climb value and a strict below-380 boundary;
- why failed firing decisions do not consume ammunition;
- why Tanker bullets are activated by the existing combined development flags;
- why firing state is recreated rather than preserved after life loss.

Prefer comments explaining why.

Do not comment trivial assignments or duplicate the analysis report in source code.

NO ACCOUNTING OR RENDERING CHANGES

Enemy bullets remain projectiles, not enemies.

Do not change:

- Level.EnemyTotal;
- normal enemy accounting;
- Tanker ledger accounting;
- cargo accounting;
- Tanker score;
- cargo score;
- Tanker rendering;
- EnemyBullet rendering;
- HUD enemy counts;
- level-completion criteria;
- exit behavior.

TESTS

Add focused deterministic tests covering at least:

MODE ISOLATION

1. --tanker-test alone creates a silent Tanker.
2. --enemy-bullets-test alone preserves existing Flipper bullet behavior.
3. Combining both flags arms the Tanker and Flippers.
4. No new flag is required.

INITIAL STATE

5. Armed Tanker ammunition is within the source-backed 1-to-2 range.
6. Controlled deterministic values can produce exactly 1 and exactly 2.
7. Initial timer is 16 plus the controlled source-backed random offset.
8. Unarmed Tanker has zero ammunition and does not consume or change firing state.

TIMER AND FIRING

9. Eligible updates decrement the Tanker timer.
10. Timer expiration resets to exactly 16.
11. A decision of 75 does not fire.
12. A decision of 76 fires.
13. A failed decision does not consume ammunition.
14. A successful decision consumes exactly one ammunition unit.
15. No further bullet is created after ammunition reaches zero.

HEIGHT AND ORIGIN

16. Tanker height advances before firing evaluation.
17. The bullet begins on the Tanker's lane.
18. The bullet begins at the Tanker's post-climb height.
19. Post-climb height below 380 remains eligible.
20. Post-climb height exactly 380 is not eligible.
21. Post-climb height above 380 is not eligible.

LIFECYCLE

22. A Tanker pending or completing cargo release cannot fire.
23. A Tanker destroyed by the player cannot fire afterward.
24. A fired bullet survives ordinary Tanker shot destruction.
25. A fired bullet survives natural Tanker cargo release unless removed by its own
    normal lifecycle.
26. A Tanker rebuilt after life loss receives fresh ammunition/timer values.
27. Active bullets clear through the established life-reset behavior.
28. Tanker/cargo accounting is unchanged by bullet creation or destruction.
29. Shooting a Tanker bullet awards 25 points without resolving an enemy unit.
30. Residual Tanker bullets do not prevent completion and clear at exit.

REGRESSIONS

31. All existing 146 tests remain green.
32. Tanker natural release remains unchanged.
33. Tanker shot release remains unchanged.
34. Two cargo Flippers still separate outward and remain distinct.
35. Fresh-Egg life reset remains unchanged.
36. Direct Flipper rim-arrival collision remains unchanged.
37. Existing Flipper bullet behavior remains unchanged.
38. Spiker/spike scenario remains unchanged.
39. Normal Level 1 remains unchanged.
40. Configurable lives and exit behavior remain unchanged.

Avoid duplicating broad existing tests unnecessarily.

Prefer focused unit tests for Tanker firing mechanics plus a smaller number of
TempestGame integration tests.

MANUAL VALIDATION CHECKLIST

The final report must tell the user to run:

dotnet run --project src/Tempest.Game -- --tanker-test --enemy-bullets-test --lives 10

Manual checks should include:

- Tankers emit occasional small white rotating bullets;
- bullets originate in the Tanker's lane and at its apparent depth;
- cadence appears delayed and occasional rather than immediate/constant;
- no more than the source-backed ammunition is emitted;
- bullets travel toward the player;
- player collision deducts exactly one life through the normal death lifecycle;
- bullets can be shot for 25 points;
- shooting the Tanker does not erase an already-fired bullet;
- natural cargo release does not erase an already-fired bullet;
- Tanker score and cargo behavior remain correct;
- standard --tanker-test remains silent without --enemy-bullets-test;
- behavior requires comparison with the original arcade version.

EXPLICITLY OUT OF SCOPE

Do not implement:

- Spiker retreat bullets;
- normal-Level-1 automatic Flipper bullet activation;
- Fuseballs;
- Pulsars;
- Super Zapper;
- Tanker stillborn cargo;
- score popups;
- rimit hurry-up;
- mid-tube Flipper progression behavior;
- level progression;
- new level composition;
- audio or bonuses;
- renderer changes;
- the suspected converging-rim-Flipper issue.

WORKFLOW

Before editing:

1. Run git status --short and confirm the tree is clean.
2. Run dotnet build.
3. Run dotnet test.
4. Confirm the baseline is exactly:
   - 146 total
   - 146 passed
   - 0 failed

If the baseline is not clean and green, stop and report the discrepancy.

During implementation:

- keep the task narrowly scoped;
- use the current architecture;
- preserve maintainability comments;
- keep the project buildable;
- use focused deterministic tests;
- do not launch or automate the Raylib window;
- do not modify Reference/incitti;
- do not commit or push.

After implementation:

1. Run focused Tanker bullet tests.
2. Run dotnet build.
3. Run the complete dotnet test suite.
4. Run git diff --check.
5. Run git diff --stat.
6. Inspect the complete diff.
7. Run git status --short.
8. Confirm Reference/incitti remains unchanged.
9. Report every modified tracked file.
10. Do not commit or push.

AI ARCHIVE

AI-Logs/ already exists.

Create if necessary:

AI-Logs/Prompts/
AI-Logs/Reports/

Save this complete task specification to:

AI-Logs/Prompts/2026-10-30-tanker-enemy-bullets-implementation-prompt.md

Preserve the prompt verbatim if available.

If exact verbatim preservation is unavailable, begin with:

"Faithful task specification reconstructed by the VS Code Agent. The original
prompt was not archived verbatim during execution."

Do not save a truncated prompt without clearly identifying the truncation.

Save the final report to:

AI-Logs/Reports/2026-10-30-tanker-enemy-bullets-implementation-report.md

FINAL REPORT

Include:

1. Summary
2. Incitti source behavior implemented
3. Tanker ammunition and timer design
4. Flag/mode behavior
5. Update and collision ordering
6. Life-reset behavior
7. Files changed
8. Tests added
9. Baseline and final build/test totals
10. git diff --check result
11. Manual validation checklist
12. Explicitly deferred features
13. Remaining arcade-comparison uncertainties
14. Todo reconciliation

Do not include raw tool traces, transient progress output or internal reasoning.

TODO HANDLING

A todo list may be used.

Before finishing:

- reconcile every todo;
- identify anything deferred;
- leave no unexplained incomplete item.

Do not commit or push.