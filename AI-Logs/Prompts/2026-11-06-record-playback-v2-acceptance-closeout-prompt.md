FAIL-CLOSED COMPLETENESS CHECK

Read this entire user-provided task before using any tools, reading files,
running commands, modifying files, staging, committing, pushing, or taking any
repository action.

Before doing anything else, locate the unique completion token near the end of
this user-provided task.

Verify exactly one complete standalone token beginning with "TEMPEST_" appears
on a line by itself in the user-provided task.

VS Code/agent-generated reminders, attachment information, tagged-file context,
or other system-generated information after the user's completion token do not
invalidate it.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before
any project or repository action.


TEMPEST RECORD / PLAYBACK VERSION 2 ACCEPTANCE CLOSEOUT

Close out the manually accepted self-describing gameplay recording version-2
milestone.

Neal has completed manual validation and ACCEPTS the implementation.

The new functionality works perfectly in Neal's manual testing.

This task is limited to:

1. recording manual acceptance;
2. recording one serious but separate Level 5 defect for future investigation;
3. explicitly keeping that defect outside this milestone;
4. reconciling durable documentation;
5. rerunning final automated validation;
6. reviewing the complete recording/playback-v2 diff;
7. staging only this milestone;
8. committing it;
9. pushing the accepted checkpoint to the existing private GitHub remote.

Do not investigate or fix the separate Level 5 exit defect in this task.


ACTIVE REPOSITORY

Work only in:

C:\Users\nrnob\source\repos\Tempest

Do not use or modify the frozen former OneDrive repository.


EXPECTED STARTING BASELINE

Expected branch:

master

Expected HEAD:

8222acee9baf6e893499a7d6ddb859bd1b7e0903
Add direct level startup and controller quit

Expected:

- master tracks origin/master;
- local master and origin/master begin at the same accepted checkpoint;
- worktree contains only the uncommitted recording/playback-v2 milestone;
- index is clean;
- no unexplained nonignored untracked files.

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
restoring, deleting or discarding anything.


READ FIRST

Read:

.github/copilot-instructions.md

TEMPEST-DEVELOPMENT-NOTES.md

TEMPEST-BEHAVIOR-SPEC.md

AI-Logs/Reports/2026-11-06-record-playback-startup-options-analysis-report.md

AI-Logs/Reports/2026-11-06-record-playback-startup-options-implementation-report.md

Also inspect the complete current production/test/documentation diff.

Do not redesign the accepted implementation.


MANUAL ACCEPTANCE

Record:

RECORD / PLAYBACK VERSION 2:
MANUALLY ACCEPTED.

Neal manually tested a direct production Level 5 recording and subsequent
separate playback.

Neal verified that playback:

1. starts at production Level 5;

2. begins with the recorded 10 lives;

3. reproduces Blaster movement and firing;

4. reproduces Super Zapper use;

5. reproduces deaths that occurred during recording;

6. reproduces Level 5 enemy and Spike behavior;

7. progresses naturally from Level 5 to Level 6 when the recording does so;

8. returns to live control normally after the recorded input finishes.

Neal found no problem with the recording/playback-v2 changes and reports that
the implementation works perfectly.

Record this as the manual acceptance for the version-2 startup-metadata
extension.

Do not claim manual verification of malformed-file cases or every automated
edge case.


PRESERVE THE ACCEPTED REPLAY FOUNDATION

The original recorded-input implementation was previously judged 100% perfect
by Neal.

The version-2 extension is now also manually accepted.

Freeze:

- one effective input frame per simulation tick;
- seeded deterministic replay;
- existing PlayerInput field meanings;
- restart-input recording;
- fixed simulation-step behavior;
- normal production simulation;
- playback cursor semantics;
- live-input handoff after playback;
- production level progression;
- RNG implementation.

Do not retune or refactor these without a separately justified milestone.


ACCEPTED VERSION-2 STARTUP METADATA

Record the accepted self-describing startup identity:

- starting displayed production level;
- effective starting lives;
- effective rim-speed multiplier;
- deterministic initial random seed.

Version-2 playback restores those simulation-affecting values from the
recording itself.

Do not require playback CLI options to reconstruct them.


VERSION-1 COMPATIBILITY

Preserve existing version-1 recording compatibility.

Version-1 recordings retain their established effective startup:

- Level 1;
- 3 lives;
- rim speed 1.0;
- recorded seed/input behavior.

Do not migrate or rewrite existing version-1 files.


PLAYBACK OVERRIDE POLICY

Preserve the accepted deterministic-safety rule:

During playback, simulation-affecting command-line overrides are rejected.

This includes at least:

--level

--lives

--rim-speed

and incompatible special scenario/test startup modes.

The recording is authoritative for its simulation startup identity.

Do not silently ignore or override recorded simulation metadata.


GAME-OVER DENSITY

Preserve the established classification:

--game-over-density

is renderer/observation-only and is not part of deterministic gameplay startup
identity.

Do not move it into the recording startup configuration.


SEPARATE LEVEL 5 EXIT DEFECT

During manual testing Neal identified a severe pre-existing/unrelated defect.

This is NOT part of the recording/playback-v2 implementation and must not be
investigated or fixed during this closeout.

Record the following evidence accurately.

OBSERVED:

1. The defect has so far been isolated to Level 5.

2. If the Blaster is NOT destroyed while playing Level 5, Level 5 plays
   normally and can progress normally into Level 6.

3. If the Blaster IS destroyed while on Level 5, the ordinary death/re-entry
   sequence appears to complete normally.

4. Within a few seconds after Level 5 re-entry, the application unexpectedly
   exits.

5. The exit can occur while Neal provides NO keyboard input and NO controller
   input after re-entry.

6. It also occurs when normal keyboard or controller input is used.

7. It occurs in ordinary campaign play without recording/playback or diagnostic
   startup options.

8. Example reproduction:
   - launch ordinary game;
   - play Levels 1 through 4 normally;
   - reach Level 5;
   - play briefly;
   - allow the Blaster to be destroyed on Level 5;
   - allow re-entry to complete;
   - provide no controls if desired;
   - the game exits unexpectedly within a few seconds.

9. Death on Levels 1 through 4 does NOT trigger this behavior merely because
   the game later reaches Level 5.

10. The relevant trigger isolated so far is:
    a Blaster death occurring ON Level 5 followed by normal re-entry.

11. The console ends with the normal Raylib-style message:

    INFO: Window closed successfully

12. No crash exception or other console error has been observed in these
    reproductions.

13. Other tested levels have not reproduced the same behavior so far.

IMPORTANT EVIDENCE LIMITS:

- Do not claim the recording/playback feature causes it.
- Current evidence argues against requiring recording/playback because ordinary
  campaign play reproduces it.
- Do not claim controller input causes it.
- Do not claim keyboard input causes it.
- Do not claim Enter causes it.
- Do not claim the death/re-entry implementation itself is definitely the root
  cause.
- Do not claim the Raylib shutdown message identifies the initiating event.
- Do not claim the process crashed.
- Do not speculate about a fix.

Classify this as:

OBSERVED / SEVERE / REPRODUCIBLE / LEVEL-5-SPECIFIC SO FAR / ROOT CAUSE UNKNOWN.

It should be the next high-priority investigation after this accepted milestone
is committed and pushed.

Do NOT investigate it in this closeout.

Do NOT add diagnostic logging.

Do NOT modify input handling.

Do NOT modify death/re-entry behavior.

Do NOT attempt a speculative fix.


WHY THE BUG MUST REMAIN SEPARATE

The recording/playback-v2 functionality passed its manual acceptance criteria.

The Level 5 exit defect also reproduces without record/playback functionality.

Therefore do not contaminate the accepted recording/playback diff with an
unrelated defect investigation.

First establish a clean recording/playback-v2 checkpoint.

Then investigate Level 5 exit behavior from that known-good committed baseline.


DOCUMENTATION

Update durable documentation so that:

RECORD / PLAYBACK VERSION 2:
MANUALLY ACCEPTED.

Record:

- self-describing startup metadata;
- version-1 compatibility;
- production Level 5 recording/playback acceptance;
- lives restoration;
- level restoration;
- rim-speed restoration;
- seeded deterministic replay;
- normal progression;
- live-input handoff;
- playback override rejection;
- accepted simulation/observation option distinction.

Also record the Level 5 post-death unexpected-exit defect as a separate deferred
high-priority investigation.

Do not mix the defect into recording/playback evidence or imply causation.


DEFERRED PLAYBACK ENHANCEMENTS

Continue to defer:

- playback speed;
- pause;
- single-step/frame-step;
- automatic looping;
- playback start frame;
- playback end frame;
- true seeking;
- snapshots;
- observation overlays;
- deterministic state signatures;
- dedicated headless playback runner;
- playlist/browser UI;
- attract-mode integration.

These remain future tools for reducing manual-testing cost as later levels
become more difficult.

Do not implement them now.


FINAL AUTOMATED VALIDATION

Run focused recording/playback/version-format/parser tests.

Run deterministic replay tests.

Run neighboring regression covering:

- existing recording/playback;
- direct-level startup;
- Level 5 $53 behavior;
- Level 7 movement/cargo;
- production progression;
- campaign soak;
- deaths/re-entry;
- restart;
- Super Zapper;
- rim pursuit;
- input handling;
- live-input handoff.

Then run:

dotnet test TempestCSharp.slnx

dotnet build TempestCSharp.slnx

git diff --check

Tests must actually execute.

Require:

- 0 failed;
- 0 unexpected skipped;
- build 0 errors;
- report warning count accurately;
- git diff --check passes.

Record actual counts rather than assuming previous counts.


NO INTERACTIVE LAUNCH

Do not launch the game.

Manual acceptance is already supplied by Neal.

Do not attempt to reproduce the Level 5 exit bug.


AI LOGS

Preserve the complete task as:

AI-Logs/Prompts/2026-11-06-record-playback-v2-acceptance-closeout-prompt.md

Create:

AI-Logs/Reports/2026-11-06-record-playback-v2-acceptance-closeout-report.md

The report must contain:

1. Starting Git state.
2. Manual acceptance.
3. Accepted version-2 metadata.
4. Version-1 compatibility.
5. Accepted record CLI behavior.
6. Accepted playback behavior.
7. Playback override policy.
8. Density classification.
9. RNG/startup ordering.
10. Restart semantics.
11. Input/frame preservation.
12. Production files changed.
13. Test files changed.
14. Documentation files changed.
15. Focused validation counts.
16. Determinism counts.
17. Neighboring regression counts.
18. Full-suite counts.
19. Build warnings/errors.
20. git diff --check result.
21. Staged file list.
22. Commit hash/message.
23. Push result.
24. Final local Git state.
25. origin/master verification.
26. Level 5 unexpected-exit defect summary.
27. Explicit statement that the defect was not investigated or fixed.
28. Deferred playback enhancements.


FINAL DIFF REVIEW

Before staging run:

git status --short --branch
git rev-parse HEAD
git diff --stat
git diff --name-status
git diff
git diff --check
git ls-files --others --exclude-standard

Verify the complete diff contains only:

- recording/playback version-2 infrastructure;
- corresponding tests;
- appropriate documentation.

Verify there is no:

- Level 5 exit bug fix;
- speculative input fix;
- Level 8/9 implementation;
- advanced playback feature;
- gameplay retuning;
- generated recording file;
- reference modification;
- Videos/TEMP content;
- bin/obj content;
- Git configuration change.


STAGING

After validation and diff review pass, stage ONLY the accepted recording/playback
version-2 milestone.

Then run:

git status --short
git diff --cached --stat
git diff --cached --name-status
git diff --cached
git diff --cached --check

Require:

- bounded staged patch;
- no unexpected deletion;
- no generated recordings;
- no unrelated gameplay/bug-fix changes;
- no unstaged tracked changes requiring resolution.


COMMIT

If the staged review is correct, commit with:

git commit -m "Extend recorded gameplay startup metadata"

Do not amend prior commits.


POST-COMMIT CHECK

Run:

git status --short --branch
git rev-parse HEAD
git log -1 --oneline
git diff --stat
git diff --cached --stat
git ls-files --others --exclude-standard

Require a clean tracked worktree/index before pushing.


PUSH

Push normally:

git push

Do not force-push.

Do not alter remote configuration.

Do not create a branch.

After push run:

git status --short --branch
git rev-parse HEAD
git rev-parse origin/master

Require local master and origin/master to match.


FINAL RESPONSE

Return:

1. Manual acceptance status.
2. Accepted version-2 startup metadata.
3. Version-1 compatibility behavior.
4. Accepted recording syntax.
5. Accepted playback syntax.
6. Playback simulation-override policy.
7. Density policy.
8. RNG/startup ordering.
9. Input/frame/restart preservation.
10. Level 5 manual round-trip result.
11. Production files committed.
12. Test files committed.
13. Documentation files committed.
14. Focused test counts.
15. Determinism counts.
16. Neighboring regression counts.
17. Full-suite counts.
18. Build warnings/errors.
19. git diff --check result.
20. staged diff --check result.
21. Commit message.
22. New commit hash.
23. Push result.
24. Final Git status.
25. origin/master verification.
26. Concise Level 5 unexpected-exit defect statement.
27. Confirmation the unrelated defect was not investigated/fixed.
28. Deferred playback enhancement list.
29. Closeout report filename.
30. Confirmation no Level 8/9, advanced playback features, unrelated gameplay
    changes, force-push, or remote reconfiguration occurred.

Stop after successful push.

TEMPEST_RECORD_PLAYBACK_V2_ACCEPTANCE_CLOSEOUT_COMPLETE