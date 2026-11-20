FAIL-CLOSED COMPLETENESS CHECK

Read this entire user-provided task before using any tools, reading files,
running commands, modifying files, staging, committing, pushing, or taking any
repository action.

Before doing anything else, locate the unique completion token near the end of
this user-provided task.

Verify exactly one complete standalone token beginning with "TEMPEST_" appears
on a line by itself in the user-provided task.

VS Code/agent-generated reminders, attachment information, tagged-file context,
or other system-generated information after the user's token do not invalidate it.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before
any repository action.


TEMPEST DIRECT-LEVEL / ENTER-QUIT ACCEPTANCE CLOSEOUT

Close out the manually accepted generic direct-level startup and controller
exit-control milestone.

The implementation candidate has received Neal's manual acceptance.

This task is limited to:

1. recording that manual acceptance;
2. recording one non-blocking unexplained observation without investigating it;
3. reconciling durable documentation;
4. rerunning final validation;
5. reviewing the complete diff;
6. staging only this milestone;
7. committing it;
8. pushing the accepted checkpoint to the existing private GitHub remote.

Do not begin Level 8 or Level 9 work.


ACTIVE REPOSITORY

Work only in:

C:\Users\nrnob\source\repos\Tempest

Do not use the frozen former OneDrive repository.


EXPECTED STARTING BASELINE

Expected branch:

master

Expected HEAD:

8178429121febe5d543fcd16e546be0b75025c69
Correct Level 5 Flipper Spike behavior

Expected:

- master tracks origin/master;
- local master and origin/master begin at the accepted Level 5 checkpoint;
- worktree contains only the uncommitted direct-level/Enter-quit milestone;
- index is clean;
- no unexplained untracked nonignored files.

Before modifying anything run:

git status --short --branch
git rev-parse HEAD
git log -1 --oneline
git rev-parse origin/master
git diff --stat
git diff --name-status
git diff
git diff --cached --stat
git ls-files --others --exclude-standard
git diff --check
git remote -v

If repository state differs unexpectedly, STOP rather than cleaning, resetting,
restoring or discarding work.


READ FIRST

Read:

.github/copilot-instructions.md

TEMPEST-DEVELOPMENT-NOTES.md

TEMPEST-BEHAVIOR-SPEC.md

AI-Logs/Reports/2026-11-06-generic-direct-level-and-enter-quit-report.md

Also inspect the complete production/test/documentation diff for the current
candidate.

No implementation redesign is requested.


MANUAL ACCEPTANCE

Record:

GENERIC DIRECT-LEVEL STARTUP:
MANUALLY ACCEPTED.

CONTROLLER EXIT CONTROL:
MANUALLY ACCEPTED.

Neal verified manually that:

1. The generic direct-level option can start any currently implemented
   production Level 1 through Level 7.

2. Direct startup eliminates the need to play earlier accepted levels merely to
   reach the level being manually tested.

3. V no longer exits the game.

4. V did not present itself as a replacement gameplay action during the manual
   validation.

5. Enter/Return now immediately exits the game as intended.

6. The controller button mapped to Enter/Return therefore gives Neal a practical
   controller-only way to exit the game.

The purpose is to let Neal perform a manual gameplay session using the arcade
controller and exit the game without reaching for the computer keyboard.

Do not claim manual verification of anything Neal did not explicitly report.


ACCEPTED DIRECT-LEVEL BEHAVIOR

The accepted syntax is:

--level N

with currently supported production Levels:

1 through 7.

Examples:

dotnet run --project src\Tempest.Game -- --level 1 --lives 10

dotnet run --project src\Tempest.Game -- --level 5 --lives 10

dotnet run --project src\Tempest.Game -- --level 7 --lives 10

Record that direct startup:

- selects the requested STARTING production level;
- reuses production level configuration/initialization rather than a duplicate
  test level;
- uses fresh-game state;
- honors --lives;
- does not make the requested level terminal;
- permits normal production progression after that level;
- preserves Level 7's existing terminal campaign boundary because Level 8 is not
  yet implemented;
- remains DEVELOPMENT infrastructure rather than arcade gameplay behavior.

Do not change these semantics during closeout.


ACCEPTED EXIT CONTROLS

Record:

- V is no longer a production quit alias.
- Enter/Return is the controller-friendly immediate quit alias.
- Enter/Return exits immediately without confirmation.
- Escape/window-close behavior remains the ordinary existing exit mechanism.
- Other controller mappings remain unchanged.

Do not reassign V.

Do not change Enter into a confirmation/hold/double-press system.


NON-BLOCKING UNEXPLAINED OBSERVATION

During manual testing, Neal observed on multiple occasions that the game
unexpectedly exited and the console ended with:

INFO: Window closed successfully

Neal does NOT want this investigated during this milestone.

Record it accurately as:

OBSERVED / UNRESOLVED / NON-BLOCKING / DEFERRED.

Important:

- Do not call it a crash.
- Do not claim it was caused by Enter.
- Do not claim it was caused by the controller.
- Do not claim Alt, Ctrl or Shift caused it.
- Do not claim the clean Raylib shutdown message proves the initiating event.

Possible controller interaction is only a hypothesis.

Neal notes that other controller buttons emit ordinary keyboard Alt, Control
and Shift events, and wonders whether some combination could produce behavior
equivalent to Enter or otherwise trigger shutdown.

That is not established evidence.

Do not investigate it now.

Do not add logging now.

Do not change key mappings to address it now.

Do not launch input diagnostics now merely because of this observation.

If unexplained clean exits continue or become disruptive, handle them later as a
separate bounded input/exit investigation using the existing diagnostic
capabilities.


FREEZE THE ACCEPTED IMPLEMENTATION

Do not redesign:

- --level parsing;
- production level initialization;
- progression;
- Enter quit;
- V handling;
- existing diagnostics.

Only change something if final automated validation exposes an actual correctness
defect.


NO GAMEPLAY CHANGES

Do not alter:

- Level 1–7 geometry;
- enemy supply;
- Level 5 $53 Flipper/Spike behavior;
- Level 7 2-3-2-2-2 behavior;
- Tanker cargo;
- Spike behavior;
- Flipper speed;
- rim speed;
- Super Zapper;
- firing;
- spinner behavior;
- score;
- lives beyond existing direct-start semantics;
- death/re-entry;
- approaches/exits;
- Game Over;
- audio;
- palettes.

Do not implement Level 8 or Level 9.


DOCUMENTATION

Reconcile durable documentation so that the candidate is no longer marked as
manual-acceptance pending.

Record:

- --level N is manually accepted development infrastructure;
- current supported range is 1–7;
- it chooses the starting production level only;
- ordinary production progression continues afterward;
- V is no longer a quit alias;
- Enter/Return is the accepted immediate controller-friendly exit alias;
- Escape/window close remain;
- the purpose is scalable level-by-level manual testing;
- the unexplained clean-exit observation is deferred and non-blocking.

Preserve the previously documented future-level workflow:

analyze one level
-> implement one level
-> automated validation
-> direct-launch/manual arcade testing
-> correct discrepancies
-> manual acceptance
-> documentation
-> commit/push
-> next level

Do not implement future levels during this closeout.


FINAL AUTOMATED VALIDATION

Rerun focused coverage for:

- --level parsing;
- direct production level startup;
- fresh-game semantics;
- requested lives;
- invalid command-line handling;
- natural onward progression;
- Enter immediate quit;
- V no-action behavior;
- input diagnostic preservation.

Run neighboring regression covering:

- Levels 1–7;
- Level 5 $53 behavior;
- Level 7 movement/cargo;
- campaign progression/soak;
- death/re-entry;
- Super Zapper;
- rim pursuit;
- replay/playback if relevant to startup composition;
- production input controls.

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

Record actual counts rather than assuming the earlier candidate counts remain
unchanged.


AI LOGS

Save paired ignored records:

AI-Logs/Prompts/2026-11-06-generic-direct-level-enter-quit-acceptance-closeout-prompt.md

AI-Logs/Reports/2026-11-06-generic-direct-level-enter-quit-acceptance-closeout-report.md

The report must include:

1. Starting Git state.
2. Manual acceptance.
3. Accepted --level N semantics.
4. Supported level range.
5. Production initialization seam.
6. Normal progression behavior.
7. Accepted V behavior.
8. Accepted Enter/Return behavior.
9. Escape/window-close preservation.
10. Non-blocking unexplained clean-exit observation.
11. Clear statement that the unexplained exits were not investigated.
12. Production files changed.
13. Test files changed.
14. Documentation files changed.
15. Focused validation counts.
16. Neighboring regression counts.
17. Full-suite counts.
18. Build warnings/errors.
19. diff-check result.
20. Complete staged file list.
21. Commit hash/message.
22. Push result.
23. Final local Git state.
24. origin/master verification.
25. Confirmation no Level 8/9 or unrelated gameplay work occurred.


FINAL DIFF REVIEW

Before staging run:

git status --short --branch
git rev-parse HEAD
git diff --stat
git diff --name-status
git diff
git diff --check
git ls-files --others --exclude-standard

Verify the complete diff contains only the accepted infrastructure/control
milestone and documentation.

Specifically verify there is no:

- Level 8 work;
- Level 9 work;
- Level 5 gameplay retuning;
- Level 7 gameplay retuning;
- unrelated controller remapping;
- investigation/fix for the unexplained exits;
- reference modification;
- Videos/TEMP content;
- generated bin/obj content;
- Git configuration change.


STAGING

After validation and final review pass, stage ONLY the accepted
direct-level/Enter-quit milestone.

Then run:

git status --short
git diff --cached --stat
git diff --cached --name-status
git diff --cached
git diff --cached --check

Require a clean, bounded staged patch and no unintended unstaged tracked changes.


COMMIT

If staged review is correct, commit with:

git commit -m "Add direct level startup and controller quit"

Do not amend prior commits.


POST-COMMIT CHECK

Run:

git status --short --branch
git rev-parse HEAD
git log -1 --oneline
git diff --stat
git diff --cached --stat
git ls-files --others --exclude-standard

Require a clean tracked worktree/index before push.


PUSH

Push normally:

git push

Do not force-push.

Do not modify the remote.

Do not create a branch.

After push run:

git status --short --branch
git rev-parse HEAD
git rev-parse origin/master

Require local master and origin/master to match.


FINAL RESPONSE

Return:

1. Manual acceptance status.
2. Accepted --level N syntax and range.
3. Production initialization behavior.
4. Normal onward progression behavior.
5. Fresh-game semantics.
6. V-key behavior.
7. Enter/Return behavior.
8. Escape/window-close behavior.
9. Unexplained clean-exit observation and explicit deferred status.
10. Confirmation no investigation or speculative fix was made for those exits.
11. Production files committed.
12. Tests committed.
13. Documentation committed.
14. Focused test counts.
15. Neighboring regression counts.
16. Full-suite counts.
17. Build warnings/errors.
18. git diff --check result.
19. staged git diff --check result.
20. Commit message.
21. New commit hash.
22. Push result.
23. Final Git status.
24. origin/master verification.
25. Closeout report filename.
26. Confirmation no Level 8/9 or unrelated gameplay/control work occurred.

Stop after successful push.

TEMPEST_DIRECT_LEVEL_ENTER_QUIT_ACCEPTANCE_CLOSEOUT_COMPLETE
