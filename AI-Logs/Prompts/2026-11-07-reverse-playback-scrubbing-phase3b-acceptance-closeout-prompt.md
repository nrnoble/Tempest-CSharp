FAIL-CLOSED COMPLETENESS CHECK

Read this entire user-provided task before using tools, reading files, running
commands, modifying files, staging, committing, pushing, or taking repository
action.

Before anything else, locate the unique completion token near the end.

Require exactly one complete standalone token beginning with "TEMPEST_" on a
line by itself.

VS Code/agent-generated context after the user's token does not invalidate it.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before
any repository action.


TEMPEST REVERSE PLAYBACK SCRUBBING PHASE 3B ACCEPTANCE CLOSEOUT

Close out, validate, commit, and push the manually accepted Phase 3B reverse
recorded-playback scrubbing milestone.

Neal has completed extensive manual testing against:

Temp\GameplayRecordings\levels1-7.json

Neal reports:

"This is perfect. It does everything I need it to do."

Neal is especially satisfied that holding Shift while scrubbing provides the
fast forward/reverse navigation he wanted.

This task is limited to:

1. record Phase 3B manual acceptance;
2. reconcile durable documentation;
3. final automated validation;
4. review the complete Phase 3B diff;
5. stage only Phase 3B;
6. commit it;
7. push the accepted checkpoint.

Do not implement screenshots yet.

Do not implement snapshots.

Do not begin Level 8/9 work.


ACTIVE REPOSITORY

Work only in:

C:\Users\nrnob\source\repos\Tempest


EXPECTED BASELINE

Expected branch:

master

Expected committed HEAD and origin/master:

a028cdc4156ea5d0fbccd660e4fc427226daa390
Add headless recorded playback runner

Expected:

- Phase 3B candidate unstaged;
- clean index;
- master tracks origin/master;
- no unrelated tracked/nonignored work.

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

If state differs unexpectedly, STOP rather than resetting, cleaning, restoring,
deleting, discarding, or overwriting anything.


READ FIRST

Read:

.github/copilot-instructions.md
README.md
TEMPEST-DEVELOPMENT-NOTES.md
TEMPEST-BEHAVIOR-SPEC.md

Read:

AI-Logs/Reports/2026-11-07-reverse-playback-scrubbing-phase3b-report.md

AI-Logs/Reports/2026-11-07-headless-playback-runner-phase3a-acceptance-closeout-report.md

AI-Logs/Reports/2026-11-07-visible-playback-controls-phase2-acceptance-closeout-report.md

Inspect current source/tests and the COMPLETE current diff.

Do not rely solely on reports.


MANUAL ACCEPTANCE

Record:

REVERSE RECORDED-PLAYBACK SCRUBBING PHASE 3B:
MANUALLY ACCEPTED.

Neal tested the implementation using the real 12,761-tick Levels 1–7 recording.

Neal reports the reverse-scrubbing behavior is perfect and does everything he
needs.


ACCEPTED CONTROL MODEL

Preserve:


SPACE

Pause/resume.


RIGHT TAP

+1 recorded simulation tick.


RIGHT HOLD

Repeated +1 operations using accepted native keyboard-repeat behavior.


SHIFT + RIGHT

+5 sequential ordinary simulation ticks.


SHIFT + RIGHT HOLD

Repeated +5 groups.


LEFT TAP

-1 recorded simulation tick through deterministic reconstruction.


LEFT HOLD

Repeated -1 reconstruction using native keyboard-repeat behavior.


SHIFT + LEFT

-5 ticks through deterministic reconstruction.


SHIFT + LEFT HOLD

Repeated -5 groups.


SCRUBBING EXPERIENCE

Record that the complete forward/reverse control model now provides the
editing-style scrubbing workflow Neal wanted.

Neal has a background studying TV/film and extensive editing-room experience.

The useful interaction model is:

- hold Right or Left to scrub;
- release near an interesting event;
- tap Left/Right for exact one-tick inspection;
- hold Shift with an arrow for faster five-tick-group scrubbing;
- Space remains available for normal pause/resume.

Do not retune native repeat timing.

Do not replace the accepted scrubbing interaction with conventional playback
speed controls.


REVERSE IMPLEMENTATION

Preserve deterministic replay-from-start reconstruction.

Backward navigation does NOT reverse game operations.

For an earlier target boundary:

recorded startup
->
fresh GameplayPlaybackSession
->
ordinary deterministic recorded frames from boundary 0
->
exact target boundary
->
replace active visible playback session
->
render target state.

Do not change this accepted design during closeout.


NO SNAPSHOTS REQUIRED

Record the important Phase 3B result:

Snapshots are NOT currently needed.

Phase 3A showed that Neal's real 12,761-tick Levels 1–7 recording executes
headlessly with no perceptible delay in normal use.

Phase 3B manual testing now establishes that replay-from-start reverse
reconstruction is sufficiently responsive for the desired visible scrubbing
workflow.

Therefore:

- retain deterministic replay-from-start;
- keep snapshots deferred;
- do not add snapshot complexity without a demonstrated future need.

Do not claim formal benchmark numbers.

No benchmark instrumentation was used or needed.


ROUND-TRIP BEHAVIOR

Preserve:

N
-> Left
-> N-1
-> Right
-> N

and:

N
-> Shift+Left
-> N-5
-> Shift+Right
-> N

Automated tests already protect selected GameplayStateSummary equivalence.

Manual behavior is now accepted.


BOUNDARY ZERO

Preserve:

- Left cannot move earlier than boundary 0;
- Shift+Left clamps at boundary 0;
- no negative boundary;
- no wraparound;
- Right can immediately move forward again.


FINAL RECORDING BOUNDARY

Preserve the ability to scrub backward from the final recorded boundary while
the paused playback debugger still owns the recording.

Do not force irreversible live handoff merely because the paused recording
reached its final boundary.


LIVE INPUT HANDOFF

Preserve the one-way transition into unrecorded live gameplay.

After live handoff:

- playback scrubbing stops owning arrow keys;
- Left/Right return to ordinary live movement;
- Space returns to Super Zapper;
- normal controls resume.

Do not attempt to rewind unrecorded live gameplay.


AUDIO

Preserve:

- reconstruction uses SilentAudioEvents;
- historical audio is not replayed while scrubbing backward;
- normal visible audio resumes after reconstruction;
- forward visible steps retain normal audio semantics.

No audio redesign.


RENDERING

Preserve:

- intermediate reconstruction ticks are not rendered;
- existing Raylib window remains open;
- only the reconstructed target boundary is displayed;
- PAUSED boundary/duration overlay reflects the reconstructed state.

No window restart or second visible simulation.


PLAYERINPUT SEPARATION

Scrubbing controls remain debugger controls only.

They must not:

- enter PlayerInput;
- modify the recorded Blaster;
- alter recording contents;
- be serialized;
- become simulation identity.


PHASE 3A PRESERVATION

Preserve the accepted headless CLI unchanged:

--headless-playback
--boundary
--json

Do not modify its public semantics during closeout.


RECORDING FORMAT

No recording changes.

Preserve:

- v1;
- v2;
- metadata;
- frame format;
- strict validation;
- read-only playback;
- original recording bytes.


DOCUMENTATION

Reconcile durable documentation so Phase 3B is:

MANUALLY ACCEPTED.

Replace stale candidate/manual-pending wording specifically for Phase 3B.

Record:

- complete accepted scrub controls;
- editing/scrubbing workflow;
- replay-from-start reconstruction;
- manual success on levels1-7.json;
- late-recording responsiveness;
- snapshot decision;
- audio behavior;
- overlay behavior;
- round-trip behavior;
- live-input handoff;
- no recording-format change.

Do not rewrite unrelated historical reports.


NEXT MILESTONE

Record that the next small milestone is:

PAUSED-PLAYBACK SCREENSHOT CAPTURE.

Intended scope only, not implementation:

- screenshot available only while recorded playback is paused;
- capture only the rendered Tempest game window/content;
- save PNG under an ignored/disposable location such as Temp\Screenshots\;
- include useful playback boundary information in filename;
- screenshot must not advance simulation or recording cursor;
- investigate Print Screen first;
- if Windows/Print Screen behavior makes that unsuitable, choose a simple
  development-only alternative key such as F12;
- do not add Windows-specific global keyboard hooks merely to suppress normal OS
  Print Screen behavior.

Do NOT implement screenshot functionality during this closeout.


DEFERRED ENEMY-DESTRUCTION VISUAL

Preserve the separate deferred observation:

A Blaster-hit enemy currently disappears immediately in the C# version.

Neal observes that the arcade briefly shows a small explosion animation.

Do not implement it now.


AI LOGS

Preserve this complete task as:

AI-Logs/Prompts/2026-11-07-reverse-playback-scrubbing-phase3b-acceptance-closeout-prompt.md

Create:

AI-Logs/Reports/2026-11-07-reverse-playback-scrubbing-phase3b-acceptance-closeout-report.md

Report must include:

1. Starting Git state.
2. Manual acceptance.
3. Scrubbing workflow.
4. Space behavior.
5. Right tap/hold.
6. Shift+Right behavior.
7. Left tap/hold.
8. Shift+Left behavior.
9. Reconstruction architecture.
10. Boundary-zero behavior.
11. Final-boundary behavior.
12. Round-trip behavior.
13. PlayerInput separation.
14. Audio behavior.
15. Rendering behavior.
16. Overlay behavior.
17. Live-input handoff.
18. v1/v2 preservation.
19. Phase 3A preservation.
20. 12,761-tick real-recording acceptance.
21. Reverse-scrubbing responsiveness.
22. Snapshot decision.
23. Next paused-screenshot milestone.
24. Deferred enemy-explosion note.
25. Production files changed.
26. Test files changed.
27. Documentation changed.
28. Focused validation counts.
29. Neighboring regression counts.
30. Full-suite counts.
31. Build warnings/errors.
32. git diff --check.
33. Staged file list.
34. staged diff check.
35. Commit message/hash.
36. Push result.
37. Final Git status.
38. origin/master verification.


FINAL AUTOMATED VALIDATION

Run focused Phase 3B tests.

Run neighboring Phase 1/2/3A playback tests.

Run meaningful gameplay regressions including:

- recording/playback;
- input;
- direct startup;
- Level 5;
- Level 7;
- Flippers;
- Tanker/cargo;
- death/re-entry;
- progression;
- campaign soak;
- Super Zapper;
- audio.

Then run:

dotnet test TempestCSharp.slnx
dotnet build TempestCSharp.slnx
git diff --check

Require:

- tests actually execute;
- 0 failed;
- 0 unexpected skipped;
- build 0 errors;
- warning count accurately reported;
- diff check passes.


NO MORE MANUAL TESTING

Do not launch the game.

Neal's Phase 3B manual acceptance is complete.


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

Verify only Phase 3B code/tests/documentation are present.

Verify no:

- snapshots;
- recording-format change;
- screenshot implementation;
- enemy-explosion implementation;
- benchmark code;
- Level 8/9;
- generated recordings;
- Temp/TEMP staged content;
- Videos;
- bin/obj;
- Git configuration change;
- unrelated gameplay work.


STAGING

Stage ONLY Phase 3B.

Reconcile exact tracked paths against the actual diff.

Expected changes from the implementation report include:

src/Tempest.Game/Debug/GameplayPlaybackSession.cs

src/Tempest.Game/Debug/VisiblePlaybackController.cs

src/Tempest.Game/Program.cs

src/Tempest.Game/Rendering/VectorRenderer.cs

src/Tempest.Game/Systems/InputSystem.cs

src/Tempest.Game/TempestGame.cs

tests/Tempest.Game.Tests/GameplayPlaybackSessionTests.cs

tests/Tempest.Game.Tests/HeadlessGameplayPlaybackTests.cs

tests/Tempest.Game.Tests/InputSystemTests.cs

tests/Tempest.Game.Tests/VisiblePlaybackControllerTests.cs

README.md

TEMPEST-BEHAVIOR-SPEC.md

TEMPEST-DEVELOPMENT-NOTES.md

Do not stage AI-Logs.

Do not stage Temp/TEMP or recordings.

Then run:

git status --short
git diff --cached --stat
git diff --cached --name-status
git diff --cached
git diff --cached --check
git diff --stat

Require:

- bounded staged patch;
- no unrelated files;
- no unintended deletion;
- no unresolved unstaged tracked changes;
- staged diff check passes.


COMMIT

If correct, commit with:

git commit -m "Add reverse recorded playback scrubbing"

Do not amend prior commits.

Use the established repository co-author policy if required.


POST-COMMIT

Run:

git status --short --branch
git rev-parse HEAD
git log -1 --oneline
git diff --stat
git diff --cached --stat
git ls-files --others --exclude-standard

Require clean tracked worktree/index.


PUSH

Push normally:

git push

Do not force-push.
Do not change remote configuration.
Do not create a branch.

Then verify:

git status --short --branch
git rev-parse HEAD
git rev-parse origin/master

Require local HEAD == origin/master.


FINAL RESPONSE

Return:

1. Manual acceptance.
2. Complete scrub-control model.
3. Left tap behavior.
4. Left hold behavior.
5. Shift+Left behavior.
6. Shift+Left hold behavior.
7. Reconstruction architecture.
8. Boundary-zero behavior.
9. Final-boundary behavior.
10. Round-trip results.
11. Audio.
12. Rendering.
13. Overlay.
14. PlayerInput separation.
15. Live-input handoff.
16. v1/v2 preservation.
17. Phase 3A preservation.
18. Real levels1-7.json acceptance.
19. Reverse-scrubbing responsiveness.
20. Snapshot decision.
21. Next screenshot milestone.
22. Deferred enemy explosion.
23. Production files committed.
24. Tests committed.
25. Documentation committed.
26. Focused counts.
27. Neighboring counts.
28. Full-suite counts.
29. Build warnings/errors.
30. git diff --check.
31. staged diff check.
32. Commit message.
33. New commit hash.
34. Push result.
35. Final Git status.
36. origin/master verification.
37. Closeout report filename.
38. Confirmation no snapshot, recording-format change, screenshot implementation,
    enemy-explosion implementation, benchmark code, Level 8/9, force-push,
    remote change, or unrelated gameplay work occurred.

Stop after successful push.

TEMPEST_REVERSE_PLAYBACK_PHASE3B_ACCEPTANCE_CLOSEOUT_COMPLETE