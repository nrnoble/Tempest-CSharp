FAIL-CLOSED PROMPT CHECK

Read this entire user-provided task before doing anything.

Before using tools, reading files, running commands, modifying files, or taking
any repository action, locate the unique completion token near the end of this
user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a
line by itself in the user-provided task.

Agent-generated context after the user's token is allowed.

If exactly one complete TEMPEST_ token cannot be found, stop before taking any
project or repository action and explain why.


TASK

Correct one manually discovered defect in the existing unstaged Level 1
four-Flipper-cap diagnostic:

Enemy shots are invisible during the diagnostic.

Repository:

C:\Users\nrnob\source\repos\Tempest

Preserve the complete existing unstaged diagnostic candidate.

Do not undo or reimplement the diagnostic.

Do not stage, commit, or push.


MANUAL RESULT FROM NEAL

Neal manually compared the production Level 1 baseline with:

--level1-four-flipper-cap-test

The capped diagnostic is better and appears promising.

However, enemy shots are invisible during the diagnostic.

This makes the current A/B comparison incomplete because invisible enemy
projectiles can still affect gameplay and perceived difficulty.

Do not treat the diagnostic as manually accepted yet.


REQUIRED REPORT

Read:

AI-Logs/Reports/2026-11-09-level1-four-flipper-cap-diagnostic-report.md

That report establishes that the diagnostic intentionally preserves:

- normal production Level 1 Flipper firing;
- the normal two-enemy-shot capacity;
- firing holdoff/probability behavior;
- bullet simulation and collision behavior.

Therefore invisible enemy shots are a defect, not an intended experimental
difference.


FIRST INVESTIGATION

Before editing, determine whether enemy bullets are:

1. created and simulated but omitted only from rendering;
2. created but removed incorrectly;
3. colliding while invisible;
4. disabled in the diagnostic despite the report's intended policy;
5. hidden by a scenario-specific renderer or presentation condition;
6. affected by a direct-start flag or title/presentation classification.

Compare the diagnostic with ordinary production Level 1 using the same
deterministic conditions.

Do not assume the cause before tracing it.


FIX BOUNDARY

Correct the smallest underlying cause.

The four-Flipper diagnostic must use the same enemy-shot presentation as
accepted production Level 1.

Preserve:

- two-shot global capacity;
- Level 1 firing holdoff;
- probability thresholds;
- bullet spawn position;
- bullet movement;
- bullet lifetime;
- collision behavior;
- damage behavior;
- color;
- size;
- sound;
- update ordering.

Do not enable the separate --enemy-bullets-test behavior.

Do not require --enemy-bullets-test to make production Level 1 shots visible.

Do not alter normal production Level 1 or Levels 2-8.


EXPERIMENT ISOLATION

After correction, the only gameplay difference between production Level 1 and
the diagnostic must remain:

No more than four active Flippers may coexist in the diagnostic.

Enemy-shot visibility must not become another experimental variable.


REQUIRED TESTS

Add focused tests proving:

1. Diagnostic Level 1 Flippers can create enemy bullets under the same
   conditions as production Level 1.

2. Diagnostic and production Level 1 retain the same two-shot capacity.

3. The renderer receives and draws active enemy bullets in the diagnostic.

4. A diagnostic enemy bullet uses the accepted production position, color,
   size, and geometry path.

5. An active diagnostic bullet advances normally.

6. A visible bullet and its collision representation refer to the same active
   object/state.

7. The diagnostic does not need --enemy-bullets-test for visibility.

8. The separate --enemy-bullets-test option remains unchanged.

9. The four-active-Flipper cap and waiting-Egg behavior remain unchanged.

10. Production Level 1 and Levels 2-8 retain existing bullet behavior.

Prefer testing the renderer's bullet-mark/output seam rather than relying only
on internal bullet existence.


VALIDATION

Run:

1. Narrow diagnostic enemy-firing/rendering tests.
2. Existing Level 1 projectile tests.
3. Existing four-Flipper-cap tests.
4. Focused rendering and collision regressions.
5. Expanded Levels 1-8 regression.
6. Full solution tests:

dotnet test TempestCSharp.slnx

7. Full build:

dotnet build TempestCSharp.slnx

8. Whitespace validation:

git diff --check

Do not launch the interactive game.

Neal will perform the corrected manual A/B comparison.


DOCUMENTATION

Update durable documentation only if it currently describes the diagnostic in
a way that requires correction.

Do not add unnecessary documentation for a simple implementation defect.

Preserve MANUAL ACCEPTANCE PENDING.


AI-LOG POLICY

Save this prompt under AI-Logs/Prompts/.

Create a NEW focused correction report under AI-Logs/Reports/.

Do not rewrite the existing diagnostic implementation report.

Do not stage or commit AI-Logs.

Do not place the report under TEMP/ or Temp/.


PROTECTED FILES

Do not modify:

- Reference/arcade/roms.rev2/
- UserData/
- screenshots;
- recordings;
- Videos/;
- TEMP/;
- unrelated files.

Do not run git clean.


GIT / STOP POINT

Do not stage.

Do not commit.

Do not push.

Leave the corrected diagnostic unstaged for Neal's continued A/B test.

Do not promote the cap into production.

Do not alter release timing, rim speed, collisions, firing rules, or supply.

Do not begin Level 9.


MANUAL RETEST

Provide both commands:

Production baseline:

dotnet run --project src\Tempest.Game -- --level 1 --lives 10 --spinner-sensitivity 1

Corrected capped diagnostic:

dotnet run --project src\Tempest.Game -- --level1-four-flipper-cap-test --lives 10 --spinner-sensitivity 1

Ask Neal to verify first:

1. Enemy bullets are visible in the capped diagnostic.
2. Bullet appearance matches production Level 1.
3. Bullets move and collide normally.
4. The diagnostic never exceeds four active Flippers.
5. The capped version still feels better when bullet visibility is restored.

STOP after automated validation and manual handoff.

TEMPEST_LEVEL1_CAP_ENEMY_BULLET_VISIBILITY_FIX_20261109
