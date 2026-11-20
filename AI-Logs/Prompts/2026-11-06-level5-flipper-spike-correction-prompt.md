FAIL-CLOSED COMPLETENESS CHECK

Read this entire user-provided task before using any tools, reading files,
running commands, modifying files, staging, committing, pushing, or taking
repository action.

Before doing anything else, locate the unique completion token near the end of
this user-provided task.

Verify exactly one complete standalone token beginning with "TEMPEST_" appears
on a line by itself in the user-provided task.

VS Code/agent-generated context after the user's token does not invalidate it.

If exactly one complete TEMPEST_ token cannot be found, STOP before any project
or repository action.


TEMPEST LEVEL 5 ROM-BACKED FLIPPER / SPIKE CORRECTION

Implement one narrowly scoped candidate correction to production Level 5
Flipper behavior based on the completed ROM analysis and Neal's direct arcade
observation.

The target is ONLY the Level 5 ROM program-$53 Spike-sensitive behavior.

Do not generalize this behavior to all Spike levels.

Do not implement Level 8 or Level 9.

Do not alter accepted Level 7 behavior.

Stop after automated validation for Neal's manual Level 5 acceptance.


ACTIVE REPOSITORY

Work only in:

C:\Users\nrnob\source\repos\Tempest

Do not use the former OneDrive repository.


EXPECTED BASELINE

Expected branch:

master

Expected HEAD:

aa4eb762b9e207b2014daec4533fbd5a513cffcd
Implement accepted Game Over presentation

master should track origin/master.

Before doing anything run:

git status --short --branch
git rev-parse HEAD
git log -1 --oneline
git diff --stat
git diff --cached --stat
git ls-files --others --exclude-standard

Require a clean tracked worktree and index.

The ignored AI analysis records are expected and do not make the Git worktree
dirty.

If anything else is unexpected, STOP rather than resetting, cleaning or
discarding work.


READ FIRST

Read:

.github/copilot-instructions.md

TEMPEST-DEVELOPMENT-NOTES.md

TEMPEST-BEHAVIOR-SPEC.md

AI-Logs/Reports/2026-11-06-level-specific-flipper-spike-rom-analysis-report.md

Also inspect the relevant:

- Level 5 implementation/configuration;
- Flipper movement code;
- Spike representation and damage code;
- Tanker/cargo creation code;
- Level 5 tests;
- Level 6 tests;
- Level 7 Flipper movement tests;
- ordinary death/re-entry tests.

Use the checked-in arcade ROM source under:

Reference\arcade\

as the sole ROM authority.

Do not search the Internet.


EVIDENCE CLASSIFICATION

Keep these distinctions explicit:

ROM-BACKED:
behavior directly demonstrated by Atari arcade ROM code/data.

ARCADE OBSERVED:
behavior Neal has directly observed on the original arcade or MAME.

CURRENT C#:
existing implementation behavior only.

PROJECT TRANSLATION / APPROXIMATION:
C# mapping required because the architectures differ.

REFERENCE VERIFIED:
Incitti behavior only.

Do not turn project translations into ROM claims.


BOUNDARIES

This task is NOT a general Flipper rewrite.

Implement only what Level 5 needs to reproduce the ROM-backed $53
Spike-sensitive movement rule.

Freeze:

- Levels 1–4;
- Level 6;
- Level 7;
- Level 7 accepted 2-3-2-2-2 ordinary route;
- Level 7 deep Tanker-cargo behavior;
- rim pursuit;
- 2.5 rim-speed calibration;
- --rim-speed;
- Game Over;
- Super Zapper;
- firing;
- shots;
- spinner;
- scoring;
- enemy supply;
- geometry;
- progression;
- approaches/exits;
- audio.

No unrelated cleanup or redesign.


ROM-BACKED LEVEL 5 BEHAVIOR

The completed ROM analysis found that displayed Level 5 selects Flipper movement
program:

$53

Level 5 starts with nonzero Spike height.

Program $53:

1. Moves the Flipper upward in its current lane.

2. Tests the LIVE Spike height of the Flipper's CURRENT lane.

3. If the Spike predicate is false, it does not begin another lane change.

4. The Flipper nevertheless continues moving upward in the same lane.

5. When the Spike predicate becomes true, the normal flip/lane change may begin.

6. The Flipper moves during the turn.

7. After a completed flip, the program returns to the move-and-test behavior.

8. The newly occupied lane is tested only after the completed flip and subsequent
   along movement.

There is:

- no explicit permanent "locked" state;
- no random choice deciding whether to lock;
- no universal Spike-lock rule;
- no replacement movement program selected dynamically.

The apparent lock is produced by repeatedly suppressing lane changes while the
current Spike-height condition is not satisfied.


SPIKE PREDICATE

Preserve the ROM semantics identified in the report.

Opcode $1C reads the current lane's live spike_ht.

The ROM analysis establishes:

- zero Spike height is treated as $FF;
- otherwise the stored live Spike height is compared against enemy_along;
- the predicate is true when spike_ht >= enemy_along, including equality.

Therefore the Level 5 behavior must distinguish at least:

BELOW THE LIVE SPIKE TIP:
remain in the current lane while continuing ordinary upward movement.

AT THE TIP:
equality satisfies the predicate; lane changing may resume.

ABOVE THE TIP:
lane changing may resume.

DESTROYED / ABSENT SPIKE:
do not suppress lane changes.

PARTIALLY DAMAGED / SHORTENED SPIKE:
use its actual current height. A shortened Spike can therefore release a
Flipper earlier than an intact Spike.

Do not snapshot Spike height when the Flipper enters the lane.

The behavior depends on current live Spike state.


IMPORTANT TERMINOLOGY

Do not implement a permanent IsLockedToSpike state unless the C# architecture
absolutely requires a translated state and it is clearly documented as a
PROJECT TRANSLATION.

Prefer representing the actual rule:

"Can the current Level 5 Flipper start its next below-rim lane change at its
current depth given the current lane's live Spike height?"

The Flipper is not physically attached to the Spike in ROM evidence.


LEVEL-SPECIFIC SEAM

Current C# uses generic CanFlip behavior for Level 5.

Do not hard-code:

if level == 5

through arbitrary gameplay code if a small level-configuration seam can express
the rule cleanly.

Introduce the narrowest maintainable configuration/data seam needed to say that
Level 5 uses Spike-gated below-rim flipping.

The design must anticipate that future Level 9 is ROM-backed to use the same
$53 family, but:

DO NOT IMPLEMENT LEVEL 9.

Do not configure nonexistent future levels.

Do not create a large generalized ROM interpreter unless clearly necessary.

Prefer a small explicit level movement-policy seam.


LEVEL 6 MUST NOT INHERIT THIS

The ROM analysis established:

Level 6 -> $0B

and $0B does NOT test Spike height.

Therefore Level 6 Flippers must continue being able to perform their established
below-rim lane changes without the new Level 5 Spike gate.

Add regression protection for this distinction.


LEVEL 7 MUST NOT INHERIT THIS

Level 7 -> ROM $24

$24 does NOT test Spike height.

Current production Level 7 also has a manually accepted C# route around:

2-3-2-2-2

Preserve that route exactly.

Do not make Spike height suppress Level 7's programmed lane changes.

Add regression protection if needed.


LEVEL 4 MUST NOT INHERIT THIS

Level 4 also has starting Spikes but uses ROM $24, not $53.

The existence of Spikes alone must never activate the Level 5 behavior.

Preserve current Level 4 behavior.


ORIGINAL FLIPPERS

Ordinary production Level 5 Flippers should receive the new Level 5
Spike-sensitive policy.

Test starting in and entering a Spike lane.

Verify:

- upward movement continues;
- the next lane change is suppressed while below the live Spike tip;
- reaching/passing the threshold permits lane-changing behavior again.


TANKER CARGO — IMPORTANT ROM DISTINCTION

Do not automatically give every Level 5 Tanker-released Flipper identical $53
behavior.

The ROM analysis established a release-depth distinction.

NATURAL / NEAR-RIM TANKER CARGO

The ROM selects:

$07

for Tanker cargo released near the rim.

$07 is straight-climb behavior.

Do not force $53 Spike interaction onto this cargo merely because the displayed
level is Level 5.


DEEP PLAYER-SHOT TANKER CARGO

When the player destroys the Tanker at sufficient depth, the ROM selects the
level movement program for its children.

On Level 5 that is:

$53

Therefore deep player-shot Level 5 cargo should participate in the
Spike-sensitive movement rule.

Preserve existing cargo identity/direction ownership behavior unless a change is
required specifically for this rule.


TANKER SIBLING DIRECTION

The ROM analysis found that Tanker children are seeded on opposite adjacent
sides with opposite direction bits.

Do not alter current Tanker sibling directional behavior in this task unless
necessary for Level 5 $53 integration.

Do not revisit Level 7 cargo routing.


DEATH / RE-ENTRY

Do not invent restoration of the old movement-program instruction pointer.

The ROM analysis found that re-entry creates fresh pending supply and newly
selected Flippers begin using the current level's movement program rather than
resuming an old movement-program phase.

Map this into the existing C# reconstruction architecture narrowly.

At minimum verify Level 5 Spike-sensitive behavior remains correct after
ordinary death/re-entry.

Do not redesign the identity ledger.


TESTS

Add focused tests proving the Level 5 behavior.

At minimum cover:

1. Level 5 ordinary Flipper below an intact current-lane Spike tip continues
   climbing but does not begin the next lane change.

2. Continued lane stay is not a movement pause.

3. Equality at the live Spike-tip threshold permits the lane-changing behavior.

4. Above the live Spike tip permits the lane-changing behavior.

5. Destroyed/absent Spike does not suppress flipping.

6. Partially damaged/shortened Spike changes the release threshold.

7. The live current Spike height is reevaluated rather than latched.

8. Entering a Spike lane applies the current-lane rule after the completed lane
   change at the correct translated point.

9. Level 5 ordinary behavior is deterministic and contains no random
   lock/unlock decision.

10. Level 6 Spike lanes do NOT activate the Level 5 gate.

11. Level 7 Spike lanes do NOT activate the Level 5 gate and accepted
    2-3-2-2-2 behavior is unchanged.

12. Level 4 presence of Spikes does NOT activate the Level 5 gate.

13. Level 5 natural/near-rim Tanker cargo preserves the intended straight
    near-rim behavior rather than incorrectly receiving $53.

14. Deep player-shot Level 5 Tanker cargo receives the Level 5 Spike-sensitive
    policy.

15. Ordinary death/re-entry does not lose the Level 5 policy.

16. Existing Level 1-7 progression remains unchanged.

Do not write tests for Level 9 production because Level 9 is not implemented.


CURRENT C# TRANSLATION CARE

ROM enemy_along and C# Height/Depth do not necessarily share literal coordinate
units or orientation.

Before implementing the threshold comparison:

- inspect existing Spike height semantics;
- inspect C# Flipper height/depth orientation;
- establish the correct equivalent comparison;
- document any coordinate mapping as PROJECT TRANSLATION.

Do NOT copy ROM numeric values such as $D8 directly into the C# 400-unit tube
coordinate system unless an established conversion exists.

Use the existing Level 5 Spike representation as the C# source of live Spike
height.


NO BROAD ROM INTERPRETER

Do not implement a generic ROM p-code virtual machine merely for this
correction.

A future level-program system may eventually warrant a data-driven abstraction,
but this task should make the smallest maintainable change needed for Level 5
while leaving a sensible seam for future per-level movement behaviors.

Do not prematurely redesign Levels 1–7 around ROM opcode execution.


AUTOMATED VALIDATION

Run focused Level 5 Flipper/Spike tests.

Run Level 5 Tanker cargo and death/re-entry tests.

Run regression protection for:

- Level 4;
- Level 6;
- Level 7;
- Level 7 movement;
- Level 7 cargo;
- Spike lifecycle/damage;
- progression;
- campaign soak.

Then run:

dotnet test TempestCSharp.slnx

dotnet build TempestCSharp.slnx

git diff --check

Record actual counts.

Tests must actually execute.

Require:

0 failed

0 unexpected skipped

Build must have 0 errors.

Report warning count accurately.


DOCUMENTATION

Do not mark the behavior accepted yet.

Update durable documentation only enough to record the implementation candidate
and evidence classification.

Record:

ROM-BACKED:
Level 5 selects $53 and uses the live current-lane Spike predicate.

ARCADE OBSERVED:
Neal has observed apparent Level 5 Spike-lane following/locking behavior.

PROJECT TRANSLATION:
the exact mapping from ROM enemy_along/spike_ht to C# coordinates/state.

MANUAL ACCEPTANCE:
PENDING.


AI LOGS

Save paired records:

AI-Logs/Prompts/2026-11-06-level5-flipper-spike-correction-prompt.md

AI-Logs/Reports/2026-11-06-level5-flipper-spike-correction-report.md

The report must include:

1. Starting Git state.
2. Exact ROM evidence used.
3. Current C# mismatch.
4. Implementation seam chosen.
5. Coordinate translation used.
6. Ordinary Flipper behavior.
7. Intact Spike behavior.
8. Shortened Spike behavior.
9. Destroyed Spike behavior.
10. Natural Tanker-cargo behavior.
11. Deep player-shot cargo behavior.
12. Death/re-entry behavior.
13. Level 4/6/7 preservation.
14. Production files changed.
15. Test files changed.
16. Focused validation.
17. Regression validation.
18. Full-suite/build/diff validation.
19. Manual acceptance checklist.
20. Final Git state.


NO INTERACTIVE GAME LAUNCH

Do not launch the interactive game.

Stop after automated validation.

Do not stage.

Do not commit.

Do not push.


MANUAL ACCEPTANCE

After automated validation, provide the ordinary launch command:

dotnet run --project src\Tempest.Game -- --lives 10

Neal will manually judge Level 5 against the arcade/MAME reference.

Ask Neal specifically to observe:

1. Ordinary Flipper entering an intact Spike lane.

2. Whether it continues climbing rather than freezing.

3. Whether it stays in that lane while below the live Spike tip.

4. Whether it becomes free to change lanes around the tip threshold.

5. Whether a shortened Spike releases it sooner.

6. Whether destroying the Spike removes the lane-stay constraint.

7. Whether normal non-Spike lane-changing still feels correct.

8. Whether Tanker cargo still behaves normally.

9. Whether Levels 4, 6, and 7 show any regression.

10. Whether overall Level 5 gameplay still feels like the arcade.


FUTURE-LEVEL DEVELOPMENT POLICY

Do not implement this policy as code in this task, but record it as a recommended
development workflow:

Before implementing each future production level, perform a bounded
preimplementation ROM/arcade analysis covering that level's:

- geometry/topology;
- palette;
- enemy supply;
- starting Spikes;
- enemy eligibility;
- level-specific Flipper movement program;
- Flipper/Spike interaction;
- ordinary-versus-Tanker-cargo movement;
- Tanker behavior;
- Spiker behavior;
- Pulsar/Fuseball behavior where applicable;
- firing parameters;
- rim behavior;
- Super Zapper state;
- exit/Avoid-Spikes behavior;
- progression;
- other explicit level-specific enemy mechanics.

Then:

analyze one level
-> implement one level
-> automated validation
-> Neal manual arcade acceptance
-> correct discrepancies
-> document
-> commit/push
-> move to the next level

The goal is to avoid implementing levels from generic assumptions and then
having to revisit each one later.


GIT

Do not stage.
Do not commit.
Do not push.

Finish with:

git status --short --branch
git rev-parse HEAD
git diff --stat
git diff --name-status
git diff
git diff --check
git ls-files --others --exclude-standard

HEAD must remain:

aa4eb762b9e207b2014daec4533fbd5a513cffcd


FINAL RESPONSE

Return:

1. Exact Level 5 behavior implemented.
2. ROM-backed $53 semantics.
3. C# coordinate/state translation.
4. Whether an explicit lock state was avoided.
5. Ordinary Flipper behavior.
6. Shortened/destroyed Spike behavior.
7. Natural Tanker cargo behavior.
8. Deep player-shot Tanker cargo behavior.
9. Re-entry behavior.
10. Level 4/6/7 regression protection.
11. Production files changed.
12. Tests changed.
13. Documentation changed.
14. Focused test counts.
15. Neighboring regression counts.
16. Full-suite counts.
17. Build warnings/errors.
18. git diff --check result.
19. Manual launch command.
20. Manual Level 5 checklist.
21. Report filename.
22. Final Git status.
23. Confirmation no Level 8/9, unrelated gameplay, staging, commit or push
    occurred.

Stop for Neal's manual Level 5 acceptance.

TEMPEST_LEVEL5_FLIPPER_SPIKE_CORRECTION_COMPLETE
