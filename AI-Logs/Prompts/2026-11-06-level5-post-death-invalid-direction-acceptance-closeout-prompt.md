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

This check detects an incomplete or truncated user task.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before
any project or repository action and explain why.


TEMPEST LEVEL 5 POST-DEATH INVALID-DIRECTION FIX ACCEPTANCE CLOSEOUT

Close out the manually accepted Level 5 post-death invalid-direction crash fix.

The root cause has been proven, the narrow correction has been implemented,
automated validation has passed, and Neal has now completed extensive manual
acceptance.

This task is limited to:

1. recording manual acceptance;
2. reconciling durable documentation;
3. rerunning final automated validation;
4. reviewing the complete accumulated diagnostic/fix diff;
5. staging only this accepted milestone;
6. committing it;
7. pushing the accepted checkpoint to the existing private GitHub remote.

Do not begin Level 8 or Level 9 work.

Do not broaden this into another Flipper redesign.

Do not modify recording/playback behavior.


ACTIVE REPOSITORY

Work only in:

C:\Users\nrnob\source\repos\Tempest

Do not use or modify the frozen former OneDrive repository.


EXPECTED STARTING BASELINE

Expected branch:

master

Expected committed HEAD and origin/master:

11ddd630a3c6e01c18795658c9625280ca24ca8e
Extend recorded gameplay startup metadata

Expected worktree:

- uncommitted Level 5 invalid-direction correction;
- its focused regression tests;
- the preceding normal-exit diagnostic;
- corresponding durable documentation;
- clean index;
- no unrelated tracked or nonignored untracked files.

The accumulated milestone is expected to involve these tracked files:

README.md

TEMPEST-BEHAVIOR-SPEC.md

TEMPEST-DEVELOPMENT-NOTES.md

src/Tempest.Game/Gameplay/Flipper.cs

src/Tempest.Game/Program.cs

src/Tempest.Game/Systems/InputSystem.cs

src/Tempest.Game/TempestGame.cs

tests/Tempest.Game.Tests/InputSystemTests.cs

tests/Tempest.Game.Tests/Level5ProductionTests.cs

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

If repository state differs unexpectedly, STOP rather than resetting, cleaning,
restoring, deleting, discarding, or overwriting anything.


READ FIRST

Read:

.github/copilot-instructions.md

README.md

TEMPEST-DEVELOPMENT-NOTES.md

TEMPEST-BEHAVIOR-SPEC.md

AI-Logs/Reports/2026-11-06-level5-post-death-exit-root-cause-report.md

AI-Logs/Reports/2026-11-06-level5-post-death-invalid-direction-fix-report.md

Also inspect:

- the complete current diff;
- Flipper.cs;
- TempestGame.cs;
- Program.cs;
- InputSystem.cs;
- Level5ProductionTests.cs;
- InputSystemTests.cs;
- relevant Tanker/cargo, death/re-entry, Level 5 $53 and directed-transition
  code.

Do not assume the diff contents solely from reports. Verify current source.


MANUAL ACCEPTANCE

Record:

LEVEL 5 POST-DEATH INVALID-DIRECTION FIX:
MANUALLY ACCEPTED.

Neal extensively tested production Level 5 after the correction.

Neal verified:

1. Level 5 no longer exits unexpectedly after active gameplay, Blaster death
   and normal re-entry.

2. The former crash sequence was exercised repeatedly without recurrence.

3. Level 5 was tested extensively.

4. Level 5 death/re-entry remained stable.

5. Normal Level 5 gameplay remained correct.

6. Normal Level 5-to-Level 6 progression remained correct.

7. No obvious regression in Level 5 Flipper, Spike, Tanker or cargo behavior was
   observed.

8. All other currently implemented production levels were also manually tested.

9. No unexpected exit or obvious neighboring-level regression was observed.

10. Several gameplay recordings and separate playbacks were performed.

11. Record/playback continued to reproduce gameplay correctly.

12. One supplied Level 5 playback completed after 7,260 simulation ticks,
    reported that live controls resumed, and later exited through the explicit
    normal diagnostic:

    Exit reason: WindowShouldClose

13. The playback run showed no exception and Raylib cleanup completed normally.

Neal accepts the correction.

Do not claim that Neal manually isolated every automated cargo identity or
direction edge case.


PROVEN ROOT CAUSE

Record the proven implementation defect accurately.

The crash was caused by reconstructed Level 5 Tanker cargo:

1. A production Tanker released two cargo Flippers.

2. A cargo Flipper remained unresolved when the Blaster died.

3. Death/re-entry cleared active runtime entities while preserving unresolved
   cargo identity in the production ledger.

4. The unresolved cargo identity returned through a fresh Egg.

5. The fresh-Egg cargo reconstruction path used CreateFreshTankerCargo.

6. That path previously left the new cargo Flipper at the default:

   Direction == 0

7. Level 5's $53 Spike-sensitive policy could later permit a below-rim pivot.

8. Cargo normally already had a valid direction from Tanker release, so that
   path did not assign a replacement direction before BeginPivot.

9. The reconstructed Flipper therefore passed zero into:

   Level.CalculateDirectedLaneTransition(..., direction)

10. The geometry guard correctly threw:

    ArgumentOutOfRangeException("direction")

11. The exception unwound through Program's cleanup, after which Raylib printed:

    INFO: Window closed successfully

12. PowerShell reported the abnormal CLR result:

    -532462766

This is a proven C# state-initialization defect.

It is not new arcade evidence.


RELATIONSHIP TO LEVEL 5 $53

Preserve the evidence distinction:

- The $53 Spike-sensitive policy exposed the missing direction invariant.
- The $53 policy was not itself proven incorrect.
- The accepted Spike gate, threshold, climbing behavior and timing were not
  changed.
- The correction must not be described as disabling or weakening $53.

Level 5 happened to expose the latent lifecycle defect because a reconstructed
cargo Flipper could reach a gated pivot without a valid direction.


ACCEPTED CORRECTION

Record and preserve the narrow correction:

- Fresh reconstructed Level 5 Tanker cargo receives a valid
  reference-shortest direction toward the player.
- Existing Level 7 programmed-direction restoration remains unchanged.
- BeginPivot validates its required direction domain before constructing the
  directed lane transition.
- CalculateDirectedLaneTransition retains strict validation.
- Invalid direction values are not clamped, ignored, or silently defaulted.
- The correction occurs at the state-initialization/invariant boundary rather
  than weakening geometry.

Do not redesign this accepted correction during closeout.


CONTROL AND FAILURE REGRESSION

Preserve the automated production-realistic regression.

The pre-fix test reproduced:

ArgumentOutOfRangeException("direction")

through the real lifecycle:

production Level 5
-> Tanker
-> shot release
-> cargo Flippers
-> cargo/player collision
-> Blaster death
-> death/re-entry reconstruction
-> unresolved cargo Egg
-> reconstructed cargo Flipper
-> Spike-gated pivot
-> invalid direction failure

After the correction, the lifecycle completes without the exception.

Also preserve the no-input Level 5 death/re-entry control.


NORMAL EXIT DIAGNOSTIC

The accumulated worktree includes a small normal-exit diagnostic that reports:

Exit reason: WindowShouldClose

or:

Exit reason: Enter

The diagnostic proved useful by distinguishing normal exit from exception
unwinding.

Retain it if final review confirms it remains:

- low-noise;
- accurate;
- development-appropriate;
- nonintrusive;
- tested.

It must not:

- catch or suppress gameplay exceptions;
- convert exceptions into successful exits;
- change normal exit selection;
- change Enter behavior;
- change Raylib window-close behavior.

The accepted playback output demonstrated its intended use:

Gameplay playback completed after 7260 simulation ticks; live controls resumed.
Exit reason: WindowShouldClose

Do not add a broad logging framework.


EXCEPTION VISIBILITY

Preserve the current fail-visible behavior.

An unhandled gameplay exception must continue to display:

- exception type;
- message;
- parameter name where applicable;
- stack trace;
- source files/line numbers when available;
- nonzero process result.

Do not add a catch that converts fatal gameplay errors into exit code 0.

Raylib's successful cleanup message must not be treated as the initiating exit
reason.


RECORDING OBSERVATION

The earlier failed run did not produce a recording because the unhandled
exception escaped before the normal recording-save path.

That was consistent with existing recording semantics and was not a
recording/playback defect.

Do not alter recording save behavior in this closeout.


FREEZE ACCEPTED GAMEPLAY

Do not alter:

- Level 5 $53 behavior;
- Spike thresholds;
- Spike damage;
- Flipper movement timing;
- Tanker release;
- cargo accounting;
- Level 7 programmed routes;
- Level 7 2-3-2-2-2 behavior;
- rim speed;
- firing;
- Super Zapper;
- score;
- enemy supply;
- death/re-entry outside the proven correction;
- progression;
- recording/playback;
- direct-level startup;
- Game Over;
- rendering;
- audio.


DOCUMENTATION

Reconcile durable documentation to record:

LEVEL 5 POST-DEATH INVALID-DIRECTION FIX:
MANUALLY ACCEPTED.

Record:

- exact reconstructed-cargo root cause;
- exact invalid value Direction == 0;
- exact exception pathway;
- narrow direction initialization correction;
- BeginPivot invariant;
- unchanged $53 behavior;
- extensive Level 5 manual acceptance;
- manual testing of all other production levels;
- successful recording/playback retesting;
- normal 7,260-tick playback completion and live-control handoff;
- retained normal-exit diagnostics;
- ordinary exception visibility;
- no remaining known recurrence of this crash in manual testing.

Replace stale wording such as:

- manual acceptance pending;
- root cause unknown;
- fix candidate only;

where it refers specifically to this now-proven and accepted defect.

Do not rewrite unrelated historical reports.

Historical investigation reports may retain what was known at the time.


AI LOGS

Preserve this complete task as:

AI-Logs/Prompts/2026-11-06-level5-post-death-invalid-direction-acceptance-closeout-prompt.md

Create:

AI-Logs/Reports/2026-11-06-level5-post-death-invalid-direction-acceptance-closeout-report.md

The report must include:

1. Starting Git state.
2. Manual acceptance.
3. Original failure signature.
4. Original stack trace summary.
5. Abnormal process result.
6. Proven reconstructed-cargo lifecycle.
7. Exact Direction == 0 defect.
8. Relationship to $53.
9. Exact correction.
10. BeginPivot invariant.
11. Geometry guard preservation.
12. Exit-diagnostic disposition.
13. Exception-visibility preservation.
14. Level 5 manual acceptance details.
15. Other-level manual regression details.
16. Record/playback retest details.
17. 7,260-tick playback result.
18. Production files changed.
19. Test files changed.
20. Documentation files changed.
21. Focused validation counts.
22. Neighboring regression counts.
23. Full-suite counts.
24. Build warnings/errors.
25. git diff --check result.
26. Complete staged file list.
27. Commit message/hash.
28. Push result.
29. Final Git state.
30. origin/master verification.
31. Confirmation no Level 8/9 or unrelated work occurred.


FINAL AUTOMATED VALIDATION

First run focused coverage for:

- production-realistic reconstructed Level 5 cargo lifecycle;
- Direction initialization;
- BeginPivot direction invariant;
- Level 5 $53 behavior;
- Level 5 Spike gating;
- Level 5 death/re-entry;
- no-input death/re-entry control;
- Tanker cargo identity/reconstruction;
- Level 4 non-regression;
- Level 6 non-regression;
- Level 7 route/cargo preservation;
- normal Enter/window-close diagnostic behavior.

Record actual counts.


NEIGHBORING REGRESSION

Run a meaningful neighboring regression covering:

- Levels 1–7;
- Level 3 ledger/cargo;
- Level 4 production behavior;
- Level 5 production behavior;
- Level 6 production behavior;
- Level 7 movement/cargo;
- Tankers;
- cargo identity;
- Spike lifecycle;
- Flipper movement;
- directed transitions;
- death/re-entry;
- firing;
- Super Zapper;
- rim pursuit;
- progression;
- campaign soak;
- direct-level startup;
- recording/playback v2;
- input controls.

Record actual counts.


FULL VALIDATION

Run:

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

Do not treat a zero-test or build-only result as test success.


FINAL DIFF REVIEW

Before staging run:

git status --short --branch
git rev-parse HEAD
git diff --stat
git diff --name-status
git diff
git diff --check
git ls-files --others --exclude-standard

Verify the complete accumulated diff contains only:

- the Level 5 reconstructed-cargo direction fix;
- its invariant;
- the normal-exit diagnostic;
- focused tests;
- appropriate documentation.

Verify there is no:

- unrelated gameplay tuning;
- Level 8 work;
- Level 9 work;
- recording redesign;
- advanced playback feature;
- special Level 5 workaround at the geometry guard;
- swallowed exception;
- generated recording;
- TEMP content;
- Videos content;
- bin/obj output;
- reference modification;
- Git configuration change.


STAGING

After final validation and review pass, stage ONLY the accepted milestone.

Expected tracked paths should be reconciled against actual diff, but are expected
to include:

README.md

TEMPEST-BEHAVIOR-SPEC.md

TEMPEST-DEVELOPMENT-NOTES.md

src/Tempest.Game/Gameplay/Flipper.cs

src/Tempest.Game/Program.cs

src/Tempest.Game/Systems/InputSystem.cs

src/Tempest.Game/TempestGame.cs

tests/Tempest.Game.Tests/InputSystemTests.cs

tests/Tempest.Game.Tests/Level5ProductionTests.cs

Do not stage AI-Logs because that directory is intentionally ignored.

Do not stage any generated recording.

Then run:

git status --short
git diff --cached --stat
git diff --cached --name-status
git diff --cached
git diff --cached --check
git diff --stat

Require:

- bounded staged patch;
- no unintended deletion;
- no unrelated file;
- no unstaged tracked changes requiring resolution;
- staged diff check passes.


COMMIT

If staged review is correct, commit with:

git commit -m "Fix reconstructed Level 5 cargo direction"

Do not amend prior commits.

Use the repository's established co-author policy if required by current project
instructions.


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

Do not change remote configuration.

Do not create a branch.

After pushing run:

git status --short --branch
git rev-parse HEAD
git rev-parse origin/master

Require local master and origin/master to identify the same accepted commit.


FINAL RESPONSE

Return:

1. Manual acceptance status.
2. Exact original exception.
3. Exact original invalid Direction value.
4. Exact reconstructed-cargo lifecycle.
5. Proven root cause.
6. Relationship to $53.
7. Exact correction.
8. BeginPivot invariant.
9. Geometry guard preservation.
10. Normal-exit diagnostic disposition.
11. Exception-visibility preservation.
12. Level 5 manual testing result.
13. Other-level manual testing result.
14. Record/playback retest result.
15. 7,260-tick playback result.
16. Production files committed.
17. Test files committed.
18. Documentation files committed.
19. Focused test counts.
20. Neighboring regression counts.
21. Full-suite counts.
22. Build warnings/errors.
23. git diff --check result.
24. staged diff --check result.
25. Commit message.
26. New commit hash.
27. Push result.
28. Final Git status.
29. origin/master verification.
30. Closeout report filename.
31. Confirmation no Level 8/9, unrelated gameplay, recording redesign, advanced
    playback work, force-push, or remote reconfiguration occurred.

Stop after successful push.

TEMPEST_LEVEL5_INVALID_DIRECTION_ACCEPTANCE_CLOSEOUT_COMPLETE