FAIL-CLOSED COMPLETENESS CHECK

Read this entire user-provided task before using any tools, reading files,
running commands, modifying files, staging, committing, pushing, or taking any
repository action.

Before doing anything else, locate the unique completion token near the end of
this user-provided task.

Verify exactly one complete standalone token beginning with "TEMPEST_" appears
on a line by itself in the user-provided task.

VS Code/agent-generated reminders, attachment information, tagged-file context,
or other system-generated information may appear after the user's completion
token and do not invalidate it.

This check is intended to detect an incomplete or truncated user task.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before
any project or repository action.


TEMPEST LEVEL 5 FLIPPER / SPIKE ACCEPTANCE CLOSEOUT

Close out the current Level 5 ROM-backed Flipper/Spike correction.

Neal has completed manual gameplay testing and ACCEPTS the Level 5 correction.

This task is limited to:

1. recording manual acceptance;
2. reconciling durable documentation;
3. final automated validation;
4. reviewing the accumulated Level 5 correction diff;
5. committing the accepted correction;
6. pushing the clean accepted checkpoint to the existing private GitHub remote.

Do not begin the direct-level-launch milestone.

Do not remove the V quit alias in this task.

Those are separate follow-up infrastructure/control changes.


ACTIVE REPOSITORY

Work only in:

C:\Users\nrnob\source\repos\Tempest

Do not use or modify the frozen former OneDrive copy.


EXPECTED STARTING BASELINE

Expected branch:

master

Expected pre-correction HEAD:

aa4eb762b9e207b2014daec4533fbd5a513cffcd
Implement accepted Game Over presentation

The worktree should contain only the uncommitted Level 5 Flipper/Spike correction
and its related tests/documentation.

Before doing anything run:

git status --short --branch
git rev-parse HEAD
git log -1 --oneline
git diff --stat
git diff --name-status
git diff
git diff --cached --stat
git ls-files --others --exclude-standard
git diff --check
git remote -v

Require:

- branch master;
- expected starting HEAD;
- master tracks origin/master;
- no staged changes;
- no unexplained unrelated work.

If unexpected changes exist, STOP rather than resetting, restoring, cleaning,
deleting or discarding anything.


READ FIRST

Read:

.github/copilot-instructions.md

TEMPEST-DEVELOPMENT-NOTES.md

TEMPEST-BEHAVIOR-SPEC.md

AI-Logs/Reports/2026-11-06-level-specific-flipper-spike-rom-analysis-report.md

AI-Logs/Reports/2026-11-06-level5-flipper-spike-correction-report.md

Inspect the complete current diff and the affected production/tests.

Do not broaden the implementation.


MANUAL ACCEPTANCE

Record:

LEVEL 5 FLIPPER / SPIKE CORRECTION:
MANUALLY ACCEPTED.

Neal manually played production Level 5 after the ROM-backed $53 correction.

The corrected behavior passes Neal's manual Level 5 play testing.

The accepted behavior is:

- ordinary Level 5 Flippers continue climbing while below the relevant live
  Spike tip;
- the Spike condition suppresses the next below-rim lane change rather than
  freezing vertical movement;
- after the live Spike threshold permits it, lane-changing behavior resumes;
- the behavior visually reproduces the Level 5 Spike-lane-following behavior
  Neal expected from the arcade;
- no blocking Level 5 gameplay regression was observed.

Do not claim Neal manually isolated and verified every automated edge case unless
he explicitly did so.

In particular, automated evidence remains appropriate for exact shortened-Spike,
destroyed-Spike, cargo-depth and reconstruction edge cases unless separately
manually observed.


EVIDENCE CLASSIFICATION

Preserve:

ROM-BACKED:

- displayed Level 5 selects Flipper movement program $53;
- $53 uses the current lane's live Spike-height predicate;
- below the relevant live Spike threshold, the script continues along movement
  while suppressing the next flip;
- equality satisfies the predicate;
- a destroyed/absent Spike does not suppress the flip;
- shortened live Spike height changes the threshold;
- $53 is conditional control flow, not a permanent lock state;
- natural/near-rim Tanker cargo uses the ROM's straight-climb path;
- deep player-shot cargo uses the level-selected movement program.

ARCADE OBSERVED / MANUALLY ACCEPTED:

- Level 5 visibly exhibits the expected Spike-lane-following behavior;
- the corrected C# Level 5 now feels/looks acceptable in manual gameplay.

PROJECT TRANSLATION / APPROXIMATION:

- mapping ROM along/Spike coordinates into the existing C# height system;
- any C# movement-policy/configuration seam introduced for $53;
- existing C# deep-cargo threshold if not literally derived from ROM units.

Do not describe a C# coordinate approximation as a literal ROM conversion.


FREEZE THE ACCEPTED CORRECTION

Do not retune the accepted Level 5 behavior.

Do not change:

- Spike threshold semantics;
- mark/visual rendering unrelated to gameplay;
- Flipper movement speed;
- lane-change timing beyond what the accepted correction already changed;
- Tanker behavior except what is already required by the correction;
- death/re-entry beyond the accepted correction;
- enemy supply;
- firing;
- scoring;
- Super Zapper;
- spinner;
- Game Over;
- geometry;
- progression;
- audio.


LEVEL 4 / 6 / 7 PRESERVATION

The accepted correction must remain Level-5-specific.

Preserve:

Level 4:
starting Spikes do not imply the $53 gate.

Level 6:
uses its existing behavior and does not inherit Level 5 Spike gating.

Level 7:
preserve the manually accepted ordinary 2-3-2-2-2 route and accepted Tanker
cargo behavior exactly.

Do not use this closeout to bring those levels closer to literal ROM p-code.


FUTURE LEVEL DEVELOPMENT WORKFLOW

Update durable development guidance to record the agreed workflow for future
levels.

Before implementing each future production level, perform a bounded analysis of
that level's complete gameplay identity, including where applicable:

- tube geometry/topology;
- palette;
- original enemy supply;
- enemy eligibility;
- starting Spikes;
- level-specific Flipper movement program;
- Flipper/Spike interaction;
- original-versus-Tanker-cargo behavior;
- Tanker behavior;
- Spiker behavior;
- Pulsar behavior;
- Fuseball behavior;
- enemy firing parameters;
- rim behavior;
- Super Zapper state;
- exit/Avoid-Spikes behavior;
- progression;
- other explicit level-specific enemy mechanics.

Then use:

analyze one level
-> implement that level
-> automated validation
-> Neal manual arcade acceptance
-> correct discrepancies
-> update durable documentation
-> commit
-> push
-> move to the next level

The purpose is to avoid implementing future levels from generic assumptions
and then returning later to retrofit their level-specific enemy behavior.

Do not implement Level 8 or Level 9 during this closeout.


DEFERRED FOLLOW-UP INFRASTRUCTURE

Record, without implementing, the next planned separate milestone:

GENERIC DIRECT-LEVEL LAUNCH

Desired future behavior:

dotnet run --project src\Tempest.Game -- --level N --lives 10

The command should:

- start directly at the requested implemented production level;
- initialize that level through the real production configuration/state path;
- avoid creating duplicate test-only versions of levels;
- preserve appropriate fresh-game semantics;
- after clearing the selected level, continue normally to the next implemented
  production level;
- allow Neal to quit manually whenever the needed testing is complete.

This is intended to make manual testing scalable as Levels 8, 9 and later levels
are implemented.

Do not implement it now.


V QUIT-ALIAS FOLLOW-UP

Also record a separate accepted requirement for the next infrastructure/control
milestone:

The V key must NOT quit the game.

One physical controller button emits ordinary keyboard V.

Therefore the existing development convenience behavior that treats V as an
additive quit alias is no longer desired.

The next infrastructure/control milestone should remove V as a quit alias while
preserving normal Escape/window-close behavior.

Do not change the V binding in this Level 5 closeout.


TEST VALIDATION

First run focused validation covering:

- Level 5 Flipper/Spike behavior;
- intact Spike threshold;
- equality threshold;
- shortened Spike;
- destroyed/absent Spike;
- ordinary Level 5 Flippers;
- natural/near-rim Tanker cargo;
- deep player-shot Tanker cargo;
- Level 5 death/re-entry;
- Level 4 noninheritance;
- Level 6 noninheritance;
- Level 7 route preservation.

Record actual total/passed/failed/skipped counts.


NEIGHBORING REGRESSION

Run a meaningful neighboring regression covering:

- Levels 1-7;
- progression;
- campaign soak;
- Tankers;
- Spike lifecycle/damage;
- ordinary death/re-entry;
- Level 7 movement;
- Level 7 cargo;
- Super Zapper;
- rim pursuit;
- restart.

Record actual counts.


FULL VALIDATION

Then run:

dotnet test TempestCSharp.slnx

dotnet build TempestCSharp.slnx

git diff --check

Tests must actually execute.

Require:

- 0 failed;
- 0 unexpected skipped;
- build 0 errors;
- report build warning count accurately;
- git diff --check passes.

Do not treat a build-only result as test success.


DOCUMENTATION

Update durable documentation to change the Level 5 correction from:

MANUAL ACCEPTANCE PENDING

to:

MANUALLY ACCEPTED.

Record the future one-level-at-a-time analysis policy.

Record the future direct-level-launch requirement.

Record the future removal of the V quit alias.

Do not mark direct-level launch or V removal as already implemented.

Do not rewrite historical AI reports merely because the Level 5 candidate is now
accepted.


AI LOGS

Save paired records:

AI-Logs/Prompts/2026-11-06-level5-flipper-spike-acceptance-closeout-prompt.md

AI-Logs/Reports/2026-11-06-level5-flipper-spike-acceptance-closeout-report.md

The closeout report must contain:

1. Starting Git state.
2. Manual acceptance.
3. Final accepted Level 5 behavior.
4. ROM-backed evidence.
5. Arcade/manual evidence.
6. Project translations.
7. Production files changed.
8. Test files changed.
9. Documentation files changed.
10. Focused validation result.
11. Neighboring regression result.
12. Full-suite result.
13. Build warnings/errors.
14. git diff --check result.
15. Complete staged file list.
16. Commit message/hash.
17. Push result.
18. Final Git status.
19. Confirmation origin/master matches local master.
20. Deferred direct-level-launch requirement.
21. Deferred V quit-alias removal.
22. Confirmation Level 8/9 were not implemented.


FINAL DIFF REVIEW

Before staging run:

git status --short --branch
git rev-parse HEAD
git diff --stat
git diff --name-status
git diff
git diff --check
git ls-files --others --exclude-standard

Inspect the complete accumulated Level 5 diff.

Verify no:

- Level 8;
- Level 9;
- unrelated gameplay;
- Game Over changes;
- reference modifications;
- Videos;
- TEMP;
- generated bin/obj files;
- Git configuration changes;
- direct-level-launch implementation;
- V-key changes.


STAGING

After validation and diff review pass, stage ONLY the accepted Level 5
correction and required durable documentation.

Then run:

git status --short

git diff --cached --stat

git diff --cached --name-status

git diff --cached

git diff --cached --check

Require a clean staged diff containing only this accepted milestone.


COMMIT

If staged review is correct, commit locally with:

git commit -m "Correct Level 5 Flipper Spike behavior"

Do not amend the previous Game Over commit.


POST-COMMIT CHECK

Run:

git status --short --branch
git rev-parse HEAD
git log -1 --oneline
git diff --stat
git diff --cached --stat
git ls-files --others --exclude-standard

Require clean tracked worktree/index before pushing.


PUSH

The repository already has the private GitHub remote configured.

After the local commit and post-commit verification succeed:

git push

Do not force-push.

Do not change remote configuration.

Do not create branches.

After pushing verify:

git status --short --branch
git rev-parse HEAD
git rev-parse origin/master

Require local master and origin/master to identify the same accepted commit.


FINAL RESPONSE

Return:

1. Manual acceptance status.
2. Final accepted Level 5 $53 behavior.
3. ROM-backed evidence summary.
4. Project-translation summary.
5. Level 4/6/7 preservation.
6. Production files committed.
7. Test files committed.
8. Documentation files committed.
9. Focused test counts.
10. Neighboring regression counts.
11. Full-suite counts.
12. Build warnings/errors.
13. git diff --check result.
14. staged diff --check result.
15. Commit message.
16. New commit hash.
17. Push result.
18. Final local Git status.
19. origin/master verification.
20. Deferred generic --level N requirement.
21. Deferred V quit-alias removal requirement.
22. Closeout report filename.
23. Confirmation no Level 8/9, direct-level-launch implementation, V-key change,
    unrelated gameplay work, force-push, or remote reconfiguration occurred.

Stop after the accepted Level 5 commit is pushed.

TEMPEST_LEVEL5_FLIPPER_SPIKE_ACCEPTANCE_CLOSEOUT_COMPLETE