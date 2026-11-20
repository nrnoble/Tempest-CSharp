FAIL-CLOSED COMPLETENESS CHECK

Read this entire user-provided task before using tools, reading files, running
commands, modifying files, staging, committing, pushing, or taking repository
action.

Before doing anything else, locate the unique completion token near the end.

Require exactly one complete standalone token beginning with "TEMPEST_" on a
line by itself.

VS Code/agent-generated context after the user's completion token does not
invalidate it.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before
any repository action.


TEMPEST SHARED DETERMINISTIC PLAYBACK SESSION PHASE 1 CLOSEOUT

Close out, validate, commit, and push the shared deterministic playback-session
Phase 1 milestone.

This milestone establishes the common playback execution foundation that future
visible playback controls and headless VS Code Copilot tooling will share.

Phase 1 is accepted based on automated validation.

No manual gameplay or visible-playback acceptance is required because Phase 1
does not add visual playback controls and does not yet migrate visible playback
execution onto the shared session.

This task is limited to:

1. final review of the complete Phase 1 candidate;
2. documentation reconciliation;
3. final automated validation;
4. staging only the accepted Phase 1 milestone;
5. committing it;
6. pushing it to the existing private GitHub remote.

Do not begin Phase 2 implementation.

Do not add visual playback controls.

Do not add a headless CLI.

Do not add backward stepping, rewind, snapshots, fast forward, or Level 8/9.


ACTIVE REPOSITORY

Work only in:

C:\Users\nrnob\source\repos\Tempest


EXPECTED STARTING BASELINE

Expected branch:

master

Expected committed HEAD and origin/master:

7032a7219cde69a0ae25b00e5f3bc9ac1ee3ed04
Use vector text for Avoid Spikes

Expected index:

clean

Expected worktree:

only the uncommitted shared-playback-session Phase 1 implementation, tests, and
documentation.

The Phase 1 report identifies these expected tracked changes:

ADDED:

src/Tempest.Game/Debug/GameplayPlaybackSession.cs

src/Tempest.Game/Debug/GameplayStateSummary.cs

tests/Tempest.Game.Tests/GameplayPlaybackSessionTests.cs

MODIFIED:

src/Tempest.Game/Program.cs

tests/Tempest.Game.Tests/GameplayRecordingTests.cs

README.md

TEMPEST-DEVELOPMENT-NOTES.md

TEMPEST-BEHAVIOR-SPEC.md

AI-Logs are intentionally ignored.

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

AI-Logs/Reports/2026-11-06-recorded-playback-debugger-headless-analysis-report.md

AI-Logs/Reports/2026-11-06-shared-playback-session-phase1-report.md

Inspect the complete current diff and current versions of:

src/Tempest.Game/Debug/GameplayPlaybackSession.cs

src/Tempest.Game/Debug/GameplayStateSummary.cs

src/Tempest.Game/Program.cs

tests/Tempest.Game.Tests/GameplayPlaybackSessionTests.cs

tests/Tempest.Game.Tests/GameplayRecordingTests.cs

Also inspect the existing recording loader, replay cursor, startup configuration,
TempestGame.RunOneStep, and relevant playback/recording tests.

Do not rely only on reports. Verify actual source and tests.


PHASE 1 ACCEPTANCE

Record:

SHARED DETERMINISTIC PLAYBACK SESSION PHASE 1:
AUTOMATED ACCEPTANCE PASSED.

No manual visual acceptance was required because:

- visible playback was not migrated onto the session;
- no visible controls were introduced;
- no rendering behavior changed;
- existing visible playback and live-input handoff remain in Program.cs;
- this milestone introduces a Raylib-independent playback execution and
  diagnostic seam.

The preceding candidate validation reported:

- focused shared-session tests: 13 passed;
- neighboring regression: 198 passed;
- full suite: 889 passed;
- build: 0 warnings, 0 errors;
- git diff --check: passed.

Rerun all required validation and report the final actual counts.


ACCEPTED PHASE 1 CONTRACT

Preserve the accepted implementation candidate.


SHARED SESSION

GameplayPlaybackSession:

- is constructed from validated playback startup and recorded frames;
- owns the production TempestGame instance;
- owns the recorded frame cursor;
- owns the current simulation boundary;
- exposes duration, remaining frames, and completion;
- calls the normal production TempestGame.RunOneStep;
- has no Raylib dependency;
- supports silent headless execution through existing audio composition;
- does not render;
- does not poll debugger controls;
- does not perform real-time pacing.


BOUNDARY MODEL

Preserve the explicit boundary contract:

Boundary 0:

restored recorded startup before any recorded frame is consumed.

Consuming frame 0:

calls RunOneStep exactly once and produces boundary 1.

Consuming frame N:

produces boundary N+1.

For a recording with D frames:

boundary D is the state after all D recorded frames have executed.

Keep this contract explicit in code comments, tests, and documentation.


STEP FORWARD

Preserve:

- exactly one recorded frame consumed;
- existing playback input gates applied;
- restart bit preserved;
- RunOneStep called exactly once;
- cursor advanced exactly once;
- boundary advanced exactly once;
- false/no simulation when already complete;
- no rendering;
- no Raylib;
- no real-time sleep;
- no debugger-input consumption.


RUN TO BOUNDARY

Preserve:

- valid range 0 through DurationTicks;
- repeated use of StepForward;
- exact stop at requested boundary;
- current-boundary request is a no-op;
- invalid negative/out-of-range targets throw;
- earlier-than-current targets are explicitly unsupported in Phase 1;
- no silent clamping;
- no skipped frames;
- no changed simulation delta.


COMPLETION

Preserve:

- cursor equals duration at completion;
- remaining frames equals zero;
- no frame can be consumed beyond duration;
- the reusable session does not introduce live input;
- existing visible live-input handoff remains the application's responsibility.


INPUT GATING

Preserve the shared recorded-playback input/restart gates.

Visible replay and shared-session execution must not diverge in their application
of accepted death, exit, Game Over, and Level Complete input gates.

Do not change ordinary live gameplay input unnecessarily.


STATE SUMMARY

Preserve immutable GameplayStateSummary behavior.

The summary should:

- represent an exact simulation boundary;
- expose playback boundary/cursor/duration/remaining/completion;
- expose recorded startup metadata;
- copy selected gameplay state;
- expose no mutable game entity;
- use stable deterministic entity ordering;
- not advance simulation when created.

Preserve the Phase 1 selected fields unless final review establishes a defect.


FLIPPER SUMMARY

Preserve the immutable Flipper diagnostic fields:

- original/cargo identities;
- cargo classification;
- lane;
- height;
- movement state;
- direction;
- rim-pursuit direction;
- pivot source;
- pivot destination;
- current/total pivot angle;
- normalized pivot progress.

The Level 5 Direction == 0 defect demonstrates why direction belongs in this
summary.

Do not broaden the summary into an indiscriminate dump of private state.


STABLE ORDERING

Preserve deterministic summary ordering based on explicit identity and gameplay
fields.

Do not depend on incidental mutable-list ordering.


VERSION COMPATIBILITY

Preserve:

- version 1 loading/default startup semantics;
- version 2 starting level/lives/rim-speed metadata;
- seed restoration;
- input frames;
- restart bits;
- strict recording validation;
- accepted recording format.

Do not modify the recording schema.


VISIBLE PLAYBACK PRESERVATION

Phase 1 must not alter accepted visible playback presentation.

The existing application still owns:

- rendering;
- real-time loop;
- completion message;
- cursor/window behavior;
- recording capture;
- live-input handoff after playback.

Only the accepted recorded-input gating helper may be shared.

Do not migrate visible playback further during closeout.


FUTURE USER-FACING PLAYBACK CONTROLS

Record the approved roadmap, but do not implement it now.

The intended visible recorded-file controls are conceptually:

SPACE:

toggle pause/resume.

RIGHT ARROW while paused:

advance exactly one recorded simulation tick.

LEFT ARROW while paused:

move backward exactly one recorded simulation tick by deterministic
reconstruction from the recording start.

Later:

- fast forward;
- rewind;
- run/seek to a selected boundary.

These are playback-observation controls only.

They are not PlayerInput.

They must not interact with, alter, or become part of recorded gameplay.


FUTURE VS CODE COPILOT USE

Record that the same shared session will support future headless tooling that can:

- load a recording;
- run to boundary N;
- return structured state;
- compare nearby boundaries;
- evaluate invariants;
- help identify gameplay patterns and defects;
- potentially support reviewed recording-backed regression fixtures.

Do not add the headless CLI during this closeout.


ROADMAP AFTER CLOSEOUT

Record this staged recommendation:

PHASE 2:

- migrate visible file playback onto GameplayPlaybackSession;
- preserve existing playback fidelity and live-input handoff;
- add Space pause/resume;
- add Right Arrow one-tick forward while paused;
- perform Neal's visual/manual acceptance.

PHASE 3A:

- reusable headless runner API;
- thin CLI for VS Code Copilot;
- run to boundary;
- structured state output;
- assertions/checkpoints.

PHASE 3B:

- Left Arrow one-tick backward;
- deterministic replay-from-start reconstruction;
- visible rewind/seek to prior boundaries;
- benchmark performance.

LATER:

- visible speed/fast forward;
- continuous rewind;
- bookmarks/loops;
- state comparisons;
- snapshots only if replay-from-start is measurably inadequate.

Do not implement any roadmap item during closeout.


NO SNAPSHOTS

Do not add:

- arbitrary state capture;
- Random-state serialization;
- state restoration;
- reverse mutation logic;
- state hashes/signatures.

Those remain deferred.


NO GAMEPLAY CHANGES

Do not modify:

- Levels 1–7 behavior;
- Level geometry;
- enemy behavior;
- Flipper movement;
- Spikes;
- Tankers/cargo;
- Super Zapper;
- score;
- death/re-entry;
- progression;
- direct-level startup;
- recording format;
- recorded input semantics;
- rendering;
- audio behavior;
- input mappings;
- GAME OVER;
- AVOID SPIKES.


DOCUMENTATION

Reconcile durable documentation to record:

SHARED DETERMINISTIC PLAYBACK SESSION PHASE 1:
AUTOMATED ACCEPTANCE PASSED.

Record:

- shared Raylib-independent session;
- explicit boundary numbering;
- StepForward contract;
- RunToBoundary contract;
- completion contract;
- centralized recorded-playback input gating;
- immutable GameplayStateSummary;
- stable Flipper summary ordering;
- version 1/2 preservation;
- existing visible playback preserved;
- future Space/Right Arrow/Left Arrow controls;
- future headless VS Code Copilot runner;
- no backward navigation, snapshots, visible controls, or CLI yet.

Replace stale Phase 1 candidate/pending wording where it refers specifically to
this now-accepted foundation.

Do not claim the future controls exist.

Do not rewrite unrelated historical reports.


AI LOGS

Preserve this complete task as:

AI-Logs/Prompts/2026-11-07-shared-playback-session-phase1-closeout-prompt.md

Create:

AI-Logs/Reports/2026-11-07-shared-playback-session-phase1-closeout-report.md

The report must include:

1. Starting Git state.
2. Acceptance classification.
3. Why manual acceptance was not required.
4. Shared-session design.
5. Boundary contract.
6. StepForward contract.
7. RunToBoundary contract.
8. Completion semantics.
9. Input-gating ownership.
10. State-summary schema.
11. Flipper diagnostic fields.
12. Stable-ordering contract.
13. Raylib independence.
14. Audio behavior.
15. Version 1/2 compatibility.
16. Existing visible playback preservation.
17. Future visible controls.
18. Future headless Copilot use.
19. Roadmap phases.
20. Production files changed.
21. Test files changed.
22. Documentation files changed.
23. Focused test counts.
24. Neighboring regression counts.
25. Full-suite counts.
26. Build warnings/errors.
27. git diff --check result.
28. Complete staged file list.
29. staged diff check result.
30. Commit message/hash.
31. Push result.
32. Final Git state.
33. origin/master verification.
34. Confirmation no Phase 2/3 implementation, backward step, snapshot, visible
    controls, headless CLI, Level 8/9, or unrelated gameplay change occurred.


FINAL AUTOMATED VALIDATION

Run focused coverage for:

- GameplayPlaybackSession;
- GameplayStateSummary;
- boundary zero;
- StepForward;
- RunToBoundary;
- completion;
- invalid target rejection;
- state-summary immutability;
- stable Flipper ordering;
- version 1 startup;
- version 2 startup;
- restart frames;
- input gating.

Run meaningful neighboring regression covering:

- GameplayRecordingTests;
- GameplayPlaybackTests;
- direct-level startup;
- Level 5;
- Level 7;
- death/re-entry;
- progression;
- campaign soak;
- Super Zapper;
- rim pursuit;
- Tanker cargo;
- input handling;
- visible playback behavior and live-input handoff tests.

Then run:

dotnet test TempestCSharp.slnx

dotnet build TempestCSharp.slnx

git diff --check

Tests must actually execute.

Require:

- 0 failed;
- 0 unexpected skipped;
- build 0 errors;
- warning count reported accurately;
- git diff --check passes.


NO INTERACTIVE GAME LAUNCH

Do not launch the game.

No further manual testing is needed for Phase 1.


FINAL DIFF REVIEW

Before staging run:

git status --short --branch
git rev-parse HEAD
git rev-parse origin/master
git diff --stat
git diff --name-status
git diff
git diff --check
git diff --cached --stat
git ls-files --others --exclude-standard

Verify the complete diff contains only:

- shared deterministic playback session;
- immutable state summary;
- narrow shared recorded-input gating;
- Phase 1 tests;
- Phase 1 documentation.

Verify there is no:

- recording-format change;
- gameplay change;
- visible playback control;
- backward step;
- rewind;
- fast forward;
- snapshot;
- headless CLI;
- Level 8/9 work;
- unrelated input/rendering/audio change;
- generated recording;
- Temp/TEMP content;
- Videos content;
- bin/obj output;
- reference modification;
- Git configuration change.


STAGING

After validation and review pass, stage ONLY the Phase 1 milestone.

Expected tracked paths, subject to reconciliation with the actual diff:

README.md

TEMPEST-DEVELOPMENT-NOTES.md

TEMPEST-BEHAVIOR-SPEC.md

src/Tempest.Game/Debug/GameplayPlaybackSession.cs

src/Tempest.Game/Debug/GameplayStateSummary.cs

src/Tempest.Game/Program.cs

tests/Tempest.Game.Tests/GameplayPlaybackSessionTests.cs

tests/Tempest.Game.Tests/GameplayRecordingTests.cs

Do not stage AI-Logs.

Do not stage generated recordings or anything under Temp/TEMP.

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
- no remaining unstaged tracked change requiring resolution;
- staged diff check passes.


COMMIT

If staged review is correct, commit with:

git commit -m "Add shared deterministic playback session"

Do not amend prior commits.

Use the project's established co-author policy if required by current
instructions.


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

Push normally:

git push

Do not force-push.

Do not modify remote configuration.

Do not create a branch.

After pushing run:

git status --short --branch
git rev-parse HEAD
git rev-parse origin/master

Require local master and origin/master to identify the same accepted commit.


FINAL RESPONSE

Return:

1. Acceptance status.
2. Reason manual acceptance was not required.
3. Shared-session API.
4. Boundary semantics.
5. StepForward behavior.
6. RunToBoundary behavior.
7. Completion behavior.
8. Input-gating ownership.
9. State-summary scope.
10. Flipper diagnostic fields.
11. Stable-order policy.
12. Raylib independence.
13. Audio behavior.
14. Version 1/2 compatibility.
15. Existing visible playback preservation.
16. Future visible controls.
17. Future headless Copilot use.
18. Production files committed.
19. Test files committed.
20. Documentation files committed.
21. Focused test counts.
22. Neighboring regression counts.
23. Full-suite counts.
24. Build warnings/errors.
25. git diff --check result.
26. staged diff check result.
27. Commit message.
28. New commit hash.
29. Push result.
30. Final Git status.
31. origin/master verification.
32. Closeout report filename.
33. Confirmation no Phase 2/3 implementation, backward step, snapshot, visible
    controls, headless CLI, Level 8/9 work, force-push, remote reconfiguration,
    or unrelated gameplay change occurred.

Stop after successful push.

TEMPEST_SHARED_PLAYBACK_SESSION_PHASE1_CLOSEOUT_COMPLETE