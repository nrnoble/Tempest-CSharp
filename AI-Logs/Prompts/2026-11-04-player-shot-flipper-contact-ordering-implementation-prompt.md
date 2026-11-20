FAIL-CLOSED COMPLETENESS CHECK

Read this entire user-provided task before using any tools, reading files, running commands, editing files, staging files, or taking any repository action.

Before doing anything else, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context may appear after the user's completion token and do not invalidate it.

This check is intended to detect an incomplete or truncated user task, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project or repository action and explain that the task may be incomplete or truncated.


TEMPEST ROM-SHAPED PLAYER-SHOT / FLIPPER CONTACT ORDERING IMPLEMENTATION

Implement one tightly bounded production experiment correcting the demonstrated player-shot-versus-Flipper contact-order discrepancy.

This task changes only the ordering distinction between:

1. the immediate collision opportunity belonging to a newly created player shot; and

2. collision processing for player shots that were already active before the current gameplay update.

Do not broaden the task into general difficulty tuning, shot retuning, Flipper movement tuning, rim-speed tuning, or anti-camping behavior.


REQUIRED BASELINE AND EXPECTED EXISTING WORKTREE

The accepted production baseline is:

3bebf42cbe9514846ab32bf4c8a12bcfdb048da7
Implement accepted production Level 7

A prior analysis-only milestone intentionally left one new untracked test/harness file:

tests/Tempest.Game.Tests/Level1StationaryRimCombatComparisonTests.cs

This file is expected and must be preserved, inspected, integrated into validation, and retained as part of this experimental milestone.

The ignored analysis records should also exist:

AI-Logs/Prompts/2026-11-04-level1-stationary-rim-combat-comparison-prompt.md
AI-Logs/Reports/2026-11-04-level1-stationary-rim-combat-comparison-report.md

A previously seen untracked:

comment for AI.md

may or may not still exist. If present, preserve it untouched. Do not stage or commit it.

Before editing anything, run:

git status --short --branch
git rev-parse HEAD
git log -1 --oneline
git diff --stat
git diff --cached --stat
git ls-files --others --exclude-standard
git diff --check

Verify:

- branch is master;
- HEAD is exactly the accepted Level 7 commit;
- no tracked or staged changes exist;
- the untracked comparison test is the expected prior-milestone artifact;
- any other untracked file is either the known comment file or must be investigated before proceeding.

Do not reset, restore, clean, delete, overwrite, or discard the comparison test.

If unexpected tracked/staged changes or unexplained untracked files exist, STOP and report the discrepancy.


AUTHORITATIVE INVESTIGATION REPORTS

Read completely:

- .github/copilot-instructions.md
- AI-Logs/Reports/2026-11-04-blaster-fire-rim-difficulty-rom-investigation-report.md
- AI-Logs/Reports/2026-11-04-level1-stationary-rim-combat-comparison-report.md
- tests/Tempest.Game.Tests/Level1StationaryRimCombatComparisonTests.cs
- current player-shot, Flipper, gameplay-ordering, collision, and relevant test source

Read the applicable sections of:

- TEMPEST-BEHAVIOR-SPEC.md
- TEMPEST-DEVELOPMENT-NOTES.md
- README.md

Do not modify those three durable documents during this experimental implementation.

They should be updated only after Neal manually accepts the resulting gameplay.


MASTER ROM SOURCE BOUNDARY

The sole authoritative Tempest disassembly is:

Reference/arcade/Tempest Rom Source code.md

Do not search the Internet for another Tempest disassembly.

Do not use:

- another Tempest disassembly;
- GitHub Tempest source;
- Atari 2600 Tempest source;
- another arcade ROM revision;
- MAME source;
- another C port;
- a console/computer port;
- an online gameplay implementation;
- an Internet service manual.

The completed reports already contain the required source trace. Recheck the project-local master source only where necessary to implement or validate the ordering distinction.

If the project-local source does not establish a behavior, classify it as UNKNOWN rather than importing an external answer.


NEAL'S ARCADE OBSERVATION

Treat the following as ARCADE OBSERVED:

On enclosed arcade tubes, camping indefinitely in one rim lane is not a sustainable strategy.

Holding Fire does not allow the Blaster to destroy practically every Flipper pursuing it.

Careful single-tapping can temporarily destroy one or several pursuing enemies, but multiple rim enemies continue creating pressure.

A skilled player must:

- time shots carefully;
- track several rim enemies;
- move when stationary defense becomes unsafe;
- manage moments when no useful shot is available.

Neal can sometimes destroy one or two pursuers while stationary, but additional pursuing Flippers can overtake and destroy the Blaster.

A critical part of the arcade pressure is that a desired defensive shot may not be available at the necessary moment.

The goal is NOT to kill a motionless player automatically.

The goal is NOT to impose an anti-camping timer or arbitrary penalty.

The correct threat must emerge from the underlying player-shot and enemy-contact mechanics.

OPEN-TUBE QUALIFICATION:

On open tubes such as the V-shaped tube, camping at the far-left or far-right endpoint is legitimately more defensible because rim enemies can approach from only one direction.

That endpoint strategy may remain temporarily useful.

Even there, several remaining enemies should still be capable of overwhelming the player.

Do not use this closed-Level-1 experiment to establish or implement final open-tube balance.

No open-tube gameplay implementation is authorized here.


DEMONSTRATED DEFECT

The deterministic comparison proved this specific difference:

CURRENT C#:

- An already-existing player shot is moved and checked against a Flipper before the Flipper receives its current update.
- On the last pivot update into the Blaster's occupied lane, that shot can destroy the Flipper.
- The Flipper is removed before its lethal completed-pivot/player-contact result is processed.
- The existing projectile therefore saves the Blaster.

MASTER ROM ORDER:

- A newly created shot receives an immediate birth-position collision opportunity.
- Enemy movement and enemy/player contact then occur.
- The later hit check for already-moving player shots occurs afterward.
- If the Flipper completes lethal contact before that later hit check, the later projectile hit cannot retroactively save the Blaster.

This is a demonstrated material contributor to the C# game's excessive stationary safety.

Implement this ordering distinction as narrowly as possible.


REQUIRED PRODUCTION CONTRACT

Implement the following production behavior:

1. Preserve the ordinary held-level Fire input behavior.

2. When the current gameplay update successfully creates a NEW player shot:
   - preserve an immediate collision opportunity at the shot's birth position;
   - this immediate collision may destroy a matching Flipper before that Flipper's movement/contact update;
   - this must be a distinct birth-position collision event;
   - it must not be confused with the later collision processing of already-moving shots.

3. For player shots that existed BEFORE the current gameplay update:
   - preserve their normal movement for the current gameplay update;
   - do not resolve their Flipper hits before Flipper movement/contact;
   - allow Flippers to update;
   - allow completed-pivot/direct-arrival/player-contact logic to resolve;
   - only after the Flipper movement/contact phase, resolve appropriate surviving moving-shot-versus-Flipper collisions.

4. If a Flipper has already caused lethal player contact in that update:
   - a later already-moving-shot collision must not retroactively cancel or prevent that player death;
   - preserve normal subsequent cleanup/death handling;
   - prevent duplicate scoring or inconsistent enemy-accounting outcomes.

5. Preserve existing behavior for a NEW shot whose immediate birth-position collision legitimately destroys the threatening Flipper before enemy movement.

6. Apply the production ordering consistently wherever the shared ordinary player-shot/Flipper combat path is used across accepted Levels 1-7, unless inspection proves that a development-only or lifecycle state requires a separate existing policy.

Do not fake the result through a Level-1-only special case if the underlying shared ordering is global.


VERY IMPORTANT STATE DISTINCTION

The implementation must reliably distinguish:

- a shot created during the current gameplay update; and
- a shot that was already active when the update began.

Do not assume list position or incidental collection enumeration alone is a durable contract unless it is explicitly protected and documented.

Prefer a small, maintainable design such as:

- explicit birth-collision processing during successful shot creation;
- movement of the active shot collection;
- deferred ordinary Flipper hit processing after Flipper movement/contact;
- minimal snapshot/state where needed to avoid treating a just-created shot as an ordinary existing moving shot twice in the same update.

Avoid introducing a broad new entity scheduler or generalized event system for this one correction.


PLAYER-DEATH AND SAME-TICK OUTCOME RULES

Inspect the exact current player-death lifecycle before editing.

The intended result is:

- NEW shot birth collision can prevent the threatening Flipper from surviving to move/contact.
- EXISTING moving shot cannot prevent a Flipper contact that has already become lethal in the current update.
- A later moving-shot collision may still destroy another surviving Flipper in the same update only if that remains compatible with the existing death/accounting lifecycle.
- No enemy may be scored or resolved twice.
- No player death may be canceled after it is committed.
- No stale projectile or Flipper may survive incorrectly.
- Death/re-entry conservation, score, lives, wave accounting, cargo accounting, and completion blockers must remain valid.

Do not invent same-tick post-death behavior. Preserve existing lifecycle behavior unless the demonstrated ordering correction strictly requires a small change.


DO NOT CHANGE THESE MECHANICS

Do not change:

- held-level Fire behavior;
- fire-button input mapping;
- three-tick C# firing cooldown;
- current ten-shot capacity;
- player-shot travel speed;
- player-shot creation depth or lane;
- normal player-shot lifetime;
- ordinary-hit shot retirement/recycling;
- global 60 Hz simulation;
- Flipper climb speeds;
- Flipper turn speeds;
- rim pause values;
- rim direction-selection policy;
- below-rim lane-change programs;
- Level 7's accepted 2-3-2-2-2 route;
- broad source/destination pivot-lane shootability in this milestone;
- hit tolerances;
- enemy-shot behavior;
- difficulty settings;
- enemy supply;
- level composition;
- geometry;
- Super Zapper behavior;
- spinner controls;
- audio balance;
- rendering;
- progression;
- death duration;
- exit behavior.

Do not implement DIP switches or a difficulty menu.

Do not add an anti-camping mechanic.

Do not make the player die automatically merely for remaining stationary.

Do not guess arcade rim timing.


SUPER ZAPPER SCOPE

Preserve the accepted Super Zapper behavior.

Neal's arcade strategy observation is:

- the first Super Zapper use in a tube is the genuine emergency escape and clears the tube's current enemies;
- the second use kills only the closest enemy;
- Neal rarely uses the second use;
- the first use is normally saved for the latter half of a wave or for dangerous accumulated enemy pressure.

This explains why ordinary stationary firing should not itself become a permanent emergency-defense substitute.

However, do not change Super Zapper behavior, selection, timing, charges, scoring, or presentation in this milestone.


TEST-FIRST REQUIREMENTS

Before or alongside the production edit, adapt the deterministic comparison tests into regression tests for the intended production contract.

The earlier analysis test file may currently contain ROM-model and comparison-only tests.

Preserve useful investigation evidence, but clearly distinguish:

- analysis/model tests;
- production regression tests.

At minimum, add or refine focused tests proving:

1. EXISTING-SHOT FINAL-PIVOT RACE
   - an already-existing shot is positioned to hit on the same update that a Flipper completes a lethal pivot into the Blaster lane;
   - the Flipper/player contact wins;
   - the player dies;
   - the later shot does not retroactively save the player.

2. NEW-SHOT BIRTH COLLISION
   - the player fires a new shot whose birth-position collision matches the threatening Flipper;
   - the Flipper is destroyed before its movement/contact;
   - the player survives;
   - score/accounting resolve exactly once.

3. NONLETHAL EXISTING-SHOT HIT
   - an already-existing shot still moves and destroys a Flipper after that Flipper's update when no player contact occurs;
   - this verifies the change does not disable ordinary player-shot combat.

4. NONMATCHING BIRTH SHOT
   - a newly created shot that does not satisfy the immediate birth collision does not incorrectly destroy the Flipper;
   - it proceeds through the normal projectile lifecycle without duplicate collision processing.

5. MULTIPLE FLIPPERS
   - one threatening Flipper commits lethal contact while another entities/shots are present;
   - no duplicate scoring, double removal, skipped accounting, or invalid collection mutation occurs.

6. SHOT IDENTITY / CURRENT-TICK DISTINCTION
   - prove that only the newly created shot receives the immediate birth opportunity;
   - pre-existing shots use the deferred moving-shot path.

7. HELD FIRE
   - held Fire preserves the current accepted cadence;
   - the correction changes ordering, not shot frequency.

8. CAPACITY
   - maximum active player shots remains ten.

9. BELOW-RIM REGRESSION
   - ordinary below-rim shot destruction still behaves normally.

10. LEVEL 7 REGRESSION
    - accepted Level 7 movement and player-shot destruction remain intact.

11. DEATH/RE-ENTRY REGRESSION
    - the changed same-tick ordering does not corrupt unresolved enemy reconstruction or wave accounting.

12. COMPLETION REGRESSION
    - deleting or deferring a hit does not cause an incorrect same-tick level completion.


DETERMINISTIC PRESSURE HARNESS

After implementing the narrow production ordering correction, rerun the deterministic stationary-pressure fixtures from:

tests/Tempest.Game.Tests/Level1StationaryRimCombatComparisonTests.cs

Record before-versus-after results for:

- one pursuer with held Fire;
- two pursuers with held Fire;
- four pursuers with held Fire;
- four pursuers under the existing controlled single-tap pattern;
- 10-slot versus 8-slot ablation if the test remains useful.

Do not require the numeric outcome to change arbitrarily.

Do not change the fixture merely to manufacture a death.

Report honestly whether the new production ordering changes:

- death tick;
- Flippers killed;
- Flippers remaining;
- final-pivot rescues;
- accepted shots;
- capacity-unavailable attempts;
- contact events.

If the deterministic pressure results remain unchanged despite the focused race test changing correctly, report that clearly.

Do not add further production tuning in response.


SCOPE ACROSS LEVELS 1-7

The ordering correction is expected to be a shared combat rule, but Levels 1-7 contain different enemy compositions and movement programs.

Validate that:

- Level 1 straight-climbing Flippers and rim pursuit still work;
- Levels 2-6 accepted behavior remains intact;
- Level 7 no-bottom-individual-Flipper behavior remains intact;
- Tanker release/cargo behavior remains intact;
- Tanker and Spiker accounting remains intact;
- Level 7 2-3-2-2-2 movement remains intact;
- player shots still destroy valid targets across implemented levels;
- progression and terminal Level 7 behavior remain intact.

Do not retune any level.


DOCUMENTATION

Do not modify during this experimental implementation:

- README.md
- TEMPEST-BEHAVIOR-SPEC.md
- TEMPEST-DEVELOPMENT-NOTES.md

The production correction must first pass automated validation and Neal's manual acceptance.

Historical analysis reports must remain unchanged.

Save this prompt and a paired implementation report under ignored AI-Logs according to .github/copilot-instructions.md.


IMPLEMENTATION REPORT

Save paired files similar to:

AI-Logs/Prompts/2026-11-04-player-shot-flipper-contact-ordering-implementation-prompt.md

AI-Logs/Reports/2026-11-04-player-shot-flipper-contact-ordering-implementation-report.md

The report should include:

1. Starting baseline and worktree.
2. Existing untracked comparison-test disposition.
3. Master-ROM source boundary.
4. Neal's arcade observation and open-tube qualification.
5. Demonstrated ordering defect.
6. Exact production ordering before the change.
7. Exact production ordering after the change.
8. How newly created shots are distinguished from existing shots.
9. Birth-position collision behavior.
10. Existing moving-shot collision behavior.
11. Same-tick player-death behavior.
12. Scoring/accounting/death/re-entry consequences.
13. Production files changed.
14. Test files added/changed.
15. Focused regression results.
16. Deterministic pressure before/after results.
17. Shared Levels 1-7 regression results.
18. Full-suite result.
19. Build result.
20. Diff/whitespace review.
21. Remaining unknowns.
22. Manual acceptance command and checklist.
23. Final Git state.
24. Confirmation that no commit or push occurred.


AUTOMATED VALIDATION

Run focused tests covering:

- player firing;
- player shots;
- Flipper collision;
- rim pursuit;
- deterministic stationary rim combat;
- score/accounting;
- death/re-entry;
- Tanker/cargo;
- Level 7 movement/production;
- progression;
- completion.

Then run an appropriate shared seam regression selection for accepted Levels 1-7.

Then run:

dotnet test TempestCSharp.slnx
dotnet build TempestCSharp.slnx
git diff --check

Do not treat zero tests discovered as success.

Record actual passed/failed/skipped counts.

Build must have 0 errors. Report warning count accurately.


DIFF REVIEW

Before concluding, inspect:

git status --short --branch
git diff --stat
git diff --name-status
git diff
git diff --check
git ls-files --others --exclude-standard

Verify:

- only the narrow production ordering implementation, its tests, and intended ignored AI records changed;
- no authority document changed;
- no reference file changed;
- no level tuning changed;
- no shot cadence/speed/capacity changed;
- no rim timing changed;
- no Super Zapper change occurred;
- no debug or generated artifact was added;
- comment for AI.md, if present, remains untouched.

Do not stage.

Do not commit.

Do not push.


MANUAL ACCEPTANCE

Do not launch the interactive game.

After successful automated validation, provide Neal this command:

dotnet run --project src\Tempest.Game -- --lives 10

Manual acceptance should focus first on Level 1's enclosed circle.

Ask Neal to evaluate:

1. Does the game launch and play normally?

2. While remaining stationary and holding Fire:
   - can one pursuing Flipper still sometimes be destroyed?
   - can several pursuing Flippers create meaningfully greater danger?
   - is indefinite stationary defense less reliable than before?

3. While remaining stationary and carefully single-tapping:
   - can well-timed new shots still destroy a threatening Flipper?
   - does careful timing work temporarily rather than guaranteeing indefinite safety?
   - can multiple pursuing Flippers still overtake the Blaster?

4. Does the player encounter situations where:
   - a needed shot is not available at the necessary moment; or
   - an existing in-flight shot no longer incorrectly saves the Blaster on the final lethal pivot?

5. Does ordinary moving/spinner play remain normal and fair?

6. Are enemies still shootable in expected lanes and visual states?

7. Do Levels 2-7 still play normally if Neal chooses to continue through them?

8. Is the game now closer to the arcade without feeling artificially punitive?

Do not require every stationary attempt to end in death.

Temporary lane defense must remain possible.

Well-timed new shots must remain effective.

The desired result is sustained rim pressure, not an anti-camping rule.

No open-tube acceptance is required because Levels 8+ are not implemented.


STOP CONDITION

After implementation and automated validation:

- do not commit;
- do not update durable authority documents;
- do not proceed to another gameplay change;
- stop for Neal's manual comparison.

If manual testing later shows the correction is insufficient, do not automatically change capacity, lifetime, pivot shootability, or rim timing.

Those require a separate evidence-driven decision.


FINAL RESPONSE

Return:

1. Exact production ordering change.
2. How the new-shot birth exception is preserved.
3. How existing moving shots are deferred.
4. Same-tick player-contact outcome.
5. Production files changed.
6. Test files changed.
7. Focused test counts.
8. Shared regression counts.
9. Full-suite counts.
10. Build warnings/errors.
11. git diff --check result.
12. Deterministic pressure before/after results.
13. Confirmation that cadence, speed, capacity, rim timing, pivot-lane rules, below-rim movement, difficulty and Super Zapper were unchanged.
14. Manual launch command.
15. Concise manual acceptance checklist.
16. Report filename.
17. Final Git status.
18. Confirmation that no game launch, authority-document edit, reference edit, commit, push or Internet disassembly search occurred.

Stop for Neal's review.

TEMPEST_PLAYER_SHOT_FLIPPER_CONTACT_ORDERING_IMPLEMENTATION_COMPLETE