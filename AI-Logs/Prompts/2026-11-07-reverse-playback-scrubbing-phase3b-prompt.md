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


TEMPEST REVERSE RECORDED-PLAYBACK SCRUBBING — PHASE 3B

Implement the tightly bounded Phase 3B reverse-scrubbing milestone.

PHASE 3B PURPOSE

Add deterministic backward navigation to the already accepted visible
recorded-playback scrubbing system.

Accepted intended controls:

LEFT ARROW TAP
- move backward exactly 1 recorded simulation tick.

LEFT ARROW HOLD
- repeatedly perform the same -1-tick operation using native keyboard-repeat
  behavior.

SHIFT + LEFT ARROW
- move backward exactly 5 recorded simulation ticks.

SHIFT + LEFT ARROW HOLD
- repeatedly perform -5-tick groups using native keyboard-repeat behavior.

Backward navigation must be implemented by deterministic reconstruction from the
recorded startup.

Do NOT attempt to run Tempest simulation logic backward.

Do NOT reverse gameplay mutations.

Do NOT add snapshots unless this specific task later proves replay-from-start
unusable. The current authorization is replay-from-start only.

Stop after automated validation for Neal's manual testing against:

Temp\GameplayRecordings\levels1-7.json

Do not stage.
Do not commit.
Do not push.


ACTIVE REPOSITORY

Work only in:

C:\Users\nrnob\source\repos\Tempest


EXPECTED CLEAN BASELINE

Expected branch:

master

Expected HEAD and origin/master:

a028cdc4156ea5d0fbccd660e4fc427226daa390
Add headless recorded playback runner

Require:

- clean tracked worktree;
- clean index;
- master tracking origin/master;
- local HEAD matching origin/master;
- no unexplained nonignored untracked files.

Before changing anything run:

git status --short --branch
git rev-parse HEAD
git log -1 --oneline
git rev-parse origin/master
git diff --stat
git diff --cached --stat
git ls-files --others --exclude-standard
git remote -v

If repository state differs unexpectedly, STOP rather than resetting, cleaning,
restoring, deleting, discarding, or overwriting anything.


READ FIRST

Read:

.github/copilot-instructions.md

README.md

TEMPEST-DEVELOPMENT-NOTES.md

TEMPEST-BEHAVIOR-SPEC.md

Read relevant reports:

AI-Logs/Reports/2026-11-06-recorded-playback-debugger-headless-analysis-report.md

AI-Logs/Reports/2026-11-07-shared-playback-session-phase1-closeout-report.md

AI-Logs/Reports/2026-11-07-visible-playback-controls-phase2-acceptance-closeout-report.md

AI-Logs/Reports/2026-11-07-headless-playback-runner-phase3a-acceptance-closeout-report.md

Inspect current source/tests including:

GameplayPlaybackSession.cs

GameplayStateSummary.cs

HeadlessGameplayPlaybackRunner.cs

VisiblePlaybackController.cs

PlaybackOverlayStatus.cs

GameplayRecordingFile.cs

GameplayStartupConfiguration.cs

PlayerInputReplay.cs

InputSystem.cs

Program.cs

TempestGame.cs

GameplayPlaybackSessionTests.cs

VisiblePlaybackControllerTests.cs

HeadlessGameplayPlaybackTests.cs

GameplayRecordingTests.cs

and relevant deterministic playback/input tests.

Current source is authoritative.


PROTECTED ACCEPTED PLAYBACK FOUNDATION

Preserve Phase 1:

- shared GameplayPlaybackSession;
- explicit simulation-boundary semantics;
- one RunOneStep per consumed recorded frame;
- GameplayStateSummary;
- stable diagnostic ordering;
- Raylib-independent core.

Preserve Phase 2:

SPACE
- pause/resume.

RIGHT TAP
- +1 tick.

RIGHT HOLD
- repeated +1 using native keyboard repeat.

SHIFT+RIGHT
- +5 sequential ordinary ticks.

SHIFT+RIGHT HOLD
- repeated +5 groups.

PAUSED boundary/duration overlay.

Normal live-input handoff.

Preserve Phase 3A:

- headless runner;
- headless CLI;
- exact-boundary execution;
- JSON state output;
- SilentAudioEvents;
- no Raylib requirement;
- read-only recording behavior;
- explicit failure exit codes.

Do not regress any of these.


USER'S SCRUBBING MODEL

The accepted mental model is film/video-editing scrubbing.

Neal should be able to:

- hold Right to scrub forward;
- release near an interesting event;
- tap Right for precise +1 inspection;
- tap or hold Left to scrub backward;
- use Shift for 5-tick navigation in either direction.

The controls inspect gameplay that already happened.

They do not alter the recording.

They do not become gameplay input.


BACKWARD NAVIGATION PRINCIPLE

Tempest simulation remains forward-only.

Do NOT attempt to mathematically undo:

- collisions;
- enemy destruction;
- scoring;
- lives;
- random draws;
- enemy creation;
- wave-ledger changes;
- Tanker cargo;
- Spike damage;
- Super Zapper;
- progression;
- death/re-entry;
- Game Over;
- transition state.

Instead:

for target boundary N:

1. reconstruct a fresh playback session from the recording's original startup;
2. replay deterministically from boundary 0;
3. execute ordinary recorded frames sequentially;
4. stop exactly at boundary N;
5. replace the currently displayed playback session/state with the reconstructed
   session;
6. render boundary N.

This is deterministic reconstruction, not reverse simulation.


RECONSTRUCTION API

Introduce the smallest reusable abstraction needed for this.

Prefer reusing the Phase 3A headless runner/session construction path rather
than duplicating startup restoration.

Conceptually something like:

ReconstructToBoundary(recording, N)

may return a fresh GameplayPlaybackSession positioned exactly at N.

Use project-consistent naming.

The recreated session must preserve:

- startup level;
- startup lives;
- rim speed;
- seed;
- input frames;
- restart bits;
- RNG initialization order;
- production progression;
- production simulation;
- recording cursor;
- exact boundary.

Do not reconstruct from current mutable state.


LEFT TAP

While recorded playback is active AND paused:

one Left Arrow navigation event must move:

current boundary N
->
max(0, N-1)

If N > 0:

- reconstruct to N-1;
- replace the current visible playback session;
- render N-1.

If N == 0:

- remain at boundary 0;
- do nothing else;
- do not throw;
- do not wrap to the end.


LEFT HOLD

Use the same native keyboard-repeat model already accepted for Right Arrow.

Prefer the existing Raylib repeat API already used by Phase 2.

Each repeat event performs the SAME -1 operation.

Do not use IsKeyDown to reconstruct on every rendered frame.

Do not create a custom repeat frequency unless the current installed binding
unexpectedly requires one.

The desired behavior mirrors accepted Right scrubbing:

tap Left:
-1.

hold Left:
repeated -1 at native repeat cadence.

release:
stop immediately.


SHIFT + LEFT

While paused:

Shift+Left requests a 5-tick backward navigation group.

Target:

max(0, CurrentBoundary - 5)

Do one reconstruction directly to that target boundary.

Do NOT reconstruct five separate times merely to move five ticks if one
deterministic reconstruction to N-5 produces the same state.

The simulation executed during reconstruction must still process every ordinary
recorded frame from startup through the target boundary.

At boundaries 1–4:

Shift+Left should go to boundary 0.

Never go negative.


SHIFT + LEFT HOLD

Each native repeated Left event while Shift remains held requests another
5-tick backward group.

Example:

100
-> 95
-> 90
-> 85

Each group reconstructs deterministically to the requested target.

Release stops repeat navigation immediately.


RIGHT NAVIGATION AFTER REWIND

This is especially important.

After rewinding to an earlier boundary:

- Right tap must resume +1 from that reconstructed earlier session;
- Right hold must repeat +1 normally;
- Shift+Right must advance +5 normally;
- Space resume must continue normal playback from that earlier boundary.

Example:

boundary 4820

Left
-> reconstruct 4819

Right
-> ordinary StepForward
-> boundary 4820

State at the reconstructed/replayed 4820 must match a fresh deterministic
execution to original boundary 4820 on selected diagnostic state.

This round-trip is a critical invariant.


DETERMINISTIC ROUND-TRIP TEST

Add automated coverage for:

Fresh session -> boundary N -> capture summary A.

Then:

visible/control session -> boundary N
-> rewind to N-1
-> forward one tick
-> capture summary B.

Selected meaningful A/B summary state must match.

Also test:

N
-> Shift+Left to N-5
-> Shift+Right +5
-> state equivalent to original boundary N.

Do not claim total internal equivalence beyond the state contract tested.


RECONSTRUCTION AND AUDIO

Reconstruction is an implementation mechanism, not visible playback.

Do NOT emit the audio events of thousands of replayed reconstruction ticks.

Use SilentAudioEvents during reconstruction.

Once reconstructed session becomes the visible active session:

ordinary subsequent visible forward steps/resume should use normal visible
playback audio behavior.

Analyze carefully how the current session owns audio.

If a freshly reconstructed session cannot safely switch from silent audio to the
existing visible audio provider, design the smallest correct session-construction
seam.

Do not redesign the audio system.

Do not replay historical audio while rewinding.


RECONSTRUCTION AND RENDERING

Do not render intermediate reconstruction ticks.

Only render the resulting target boundary.

Do not initialize another Raylib window.

Do not close/reopen the existing window.

The existing visible game's window remains active while reconstruction occurs.


RESPONSIVENESS

Phase 3A manual testing showed that a real 12,761-tick Levels 1–7 recording
completed headlessly with no perceptible delay in ordinary use.

Use replay-from-start first.

Do not add snapshots preemptively.

Do not add performance benchmarking code.

Manual Phase 3B testing will determine whether reverse scrubbing is responsive
enough.


PAUSED MODE

Backward navigation is available only while recorded playback is paused.

While playback is running:

- Left does not rewind;
- Shift+Left does not rewind;
- navigation commands must not alter simulation cadence.

This mirrors Right/Shift+Right navigation semantics.


BOUNDARY 0

Boundary 0 must be a useful visible state.

At boundary 0:

- startup has been restored;
- frame 0 has not been consumed;
- Left does nothing;
- Shift+Left does nothing;
- Right can advance to boundary 1;
- Space may resume playback from startup.

Overlay must display:

PAUSED  0 / D


FINAL RECORDED BOUNDARY

Backward navigation must work from DurationTicks.

A user who steps to or pauses on the final recorded boundary must be able to
move backward into the recording before live-input handoff occurs, provided the
playback debugger is still controlling that completed session.

Do not force an irreversible handoff merely because the recording reached its
final boundary while paused.

Preserve the accepted Phase 2 rule that the final boundary remains inspectable.


LIVE-INPUT HANDOFF

Once the user explicitly resumes/continues into live gameplay after recording
completion:

deterministic backward navigation of the completed recording is no longer
available in that live gameplay mode.

After live handoff:

- Left/Right return to normal gameplay movement;
- Space returns to Super Zapper;
- Shift has ordinary behavior;
- playback controller no longer intercepts scrub commands.

Do not attempt to rewind live gameplay that was never recorded.


PLAYERINPUT SEPARATION

Left scrub controls must never:

- enter PlayerInput;
- move the recorded Blaster during reconstruction;
- get serialized;
- modify the recording file;
- alter simulation identity.

They are playback debugger commands only.


OVERLAY

Preserve:

PAUSED  boundary / duration

The displayed boundary must update correctly after:

- Left -1;
- held Left repeats;
- Shift+Left -5;
- repeated Shift+Left groups;
- Right forward after rewind;
- Shift+Right after rewind.

Do not redesign the overlay.


NO NEW REWIND UI

Do not add:

- transport bar;
- timeline;
- rewind icon;
- mouse seeking;
- scrub slider;
- bookmarks;
- history panel.

Keyboard scrubbing only in Phase 3B.


OPTIONAL CONSOLE DIAGNOSTIC

Do not spam the console during scrubbing.

No per-reconstruction line is needed.

If a reconstruction failure occurs, preserve fail-visible exception behavior.


RECONSTRUCTION FAILURE

A deterministic reconstruction failure is a real simulation/debugging failure.

Do not silently retain the old state and pretend navigation succeeded.

Fail visibly with useful exception information.

Do not convert it into a normal playback exit.


STATE SUMMARY

GameplayStateSummary must continue to describe the newly reconstructed visible
session correctly.

No new summary fields are required unless a narrowly necessary diagnostic field
is missing.

Do not broaden the summary gratuitously.


HEADLESS PHASE 3A PRESERVATION

The accepted CLI must remain unchanged:

--headless-playback

--boundary

--json

Do not modify its semantics for Phase 3B unless reuse requires an internal helper
extraction with identical external behavior.


RECORDING FILES

Do not modify recordings.

Do not add reverse-navigation metadata.

Do not add snapshots to recordings.

Do not create sidecar index files.

Replay-from-start uses existing v1/v2 recording information only.


VERSION 1 / VERSION 2

Backward reconstruction must work for both supported versions.

V1:

Level 1
3 lives
rim speed 1.0
recorded seed/input

V2:

explicit recorded startup metadata

Do not alter format compatibility.


TESTS — BASIC LEFT

Cover:

1. Paused boundary N + Left -> N-1.

2. Boundary 1 + Left -> 0.

3. Boundary 0 + Left remains 0.

4. Left while running does nothing.

5. Left does not enter PlayerInput.

6. Left does not modify recording.

7. Reconstructed startup metadata remains correct.

8. Reconstructed cursor equals target boundary.

9. Summary boundary equals target boundary.


TESTS — LEFT REPEAT

At the control abstraction level, prove:

- initial Left repeat event requests one backward step;
- subsequent native repeat event requests another;
- release/no repeat generates no extra navigation;
- running playback ignores reverse navigation events.

Avoid native timing-dependent tests.


TESTS — SHIFT+LEFT

Cover:

1. N >= 5 -> N-5.

2. N = 4 -> 0.

3. N = 1 -> 0.

4. N = 0 -> 0.

5. repeated Shift+Left groups continue in steps of five.

6. no negative boundaries.

7. Left modifier state never becomes PlayerInput.


TESTS — ROUND TRIP

Cover:

N
-> Left
-> Right
-> selected state equals original N.

Also:

N
-> Shift+Left
-> Shift+Right
-> selected state equals original N.

Use deterministic meaningful GameplayStateSummary fields.

Include a test with active Flipper state if practical so direction/pivot fields
are covered.

Include a test across nontrivial entity state if cleanly achievable.


TESTS — IMPORTANT GAME STATES

Backward reconstruction should be exercised across representative boundaries
involving where practical:

- Playing;
- PlayerDying;
- re-entry;
- transition/Approach;
- Level Complete or final recording boundary;
- progression between levels.

Do not require synthetic coverage of every LevelState in one milestone.


TESTS — AUDIO

Prove reconstruction itself uses silent audio.

Prove ordinary visible forward playback after reconstruction continues through
the existing visible audio path.

Use audio test seams rather than a physical audio device.


TESTS — PHASE 2 REGRESSION

Protect:

- Space;
- Right +1;
- Right repeat;
- Shift+Right +5;
- overlay;
- completion behavior;
- live-input handoff.


TESTS — PHASE 3A REGRESSION

Protect:

- headless CLI;
- exact boundaries;
- JSON output;
- exit codes;
- v1/v2;
- no Raylib/audio device requirement.


NEIGHBORING GAMEPLAY REGRESSION

Run meaningful tests covering:

- recording/playback;
- direct startup;
- Level 5;
- Level 7;
- death/re-entry;
- progression;
- campaign soak;
- Tanker/cargo;
- Flippers;
- Super Zapper;
- input.

No gameplay behavior should change.


DOCUMENTATION

Update durable documentation with Phase 3B implementation candidate status.

Record:

- Left tap = -1;
- Left native repeat = repeated -1;
- Shift+Left = -5;
- Shift+Left native repeat = repeated -5;
- backward navigation only while paused recorded playback is active;
- deterministic replay-from-start reconstruction;
- no reverse-mutation logic;
- reconstruction uses silent audio;
- intermediate reconstruction ticks are not rendered;
- overlay tracks reconstructed boundary;
- Right/Shift+Right work after rewind;
- live handoff remains one-way into unrecorded live gameplay;
- no snapshots;
- no recording-format changes;
- manual acceptance pending.

Record that real Phase 3A execution of the 12,761-tick Levels 1–7 recording
showed no perceptible delay to Neal, motivating replay-from-start before
snapshots.

Do not claim reverse scrubbing performance is accepted until Neal tests it.


PERFORMANCE POLICY

Do not add benchmark instrumentation.

Manual usability is the acceptance criterion.

Specifically determine whether:

- Left tap feels immediate;
- Left hold feels like useful reverse scrubbing;
- Shift+Left remains responsive even late in levels1-7.json.

If yes, snapshots remain unnecessary.

If no, document the latency rather than immediately implementing snapshots.


AI LOGS

Preserve this complete task as:

AI-Logs/Prompts/2026-11-07-reverse-playback-scrubbing-phase3b-prompt.md

Create:

AI-Logs/Reports/2026-11-07-reverse-playback-scrubbing-phase3b-report.md

Report must include:

1. Starting Git state.
2. Reconstruction architecture.
3. Shared Phase 1/3A reuse.
4. Session replacement behavior.
5. Boundary semantics.
6. Left tap.
7. Left repeat.
8. Shift+Left.
9. Shift+Left repeat.
10. Boundary-zero behavior.
11. Final-boundary behavior.
12. Right-after-rewind round trip.
13. Shift round trip.
14. PlayerInput separation.
15. Silent reconstruction audio.
16. Visible audio after reconstruction.
17. Rendering behavior.
18. Overlay behavior.
19. Live-input handoff.
20. v1/v2 behavior.
21. Headless CLI preservation.
22. Snapshot decision.
23. Production files changed.
24. Tests changed.
25. Documentation changed.
26. Focused test counts.
27. Neighboring regression counts.
28. Full-suite counts.
29. Build warnings/errors.
30. git diff --check.
31. Manual launch command.
32. Manual acceptance checklist.
33. Final Git state.
34. Confirmation no snapshots, recording-format changes, benchmark code,
    enemy-explosion implementation, Level 8/9, stage, commit, or push occurred.


AUTOMATED VALIDATION

Run focused Phase 3B tests.

Run neighboring Phase 1/2/3A and gameplay regressions.

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


NO INTERACTIVE GAME LAUNCH

Do not launch the game.

Neal performs the manual scrubbing test.

Headless commands may be used if needed for deterministic validation.

Do not stage.
Do not commit.
Do not push.


MANUAL TEST RECORDING

Use:

Temp\GameplayRecordings\levels1-7.json

Provide:

dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\levels1-7.json


MANUAL ACCEPTANCE CHECKLIST

Ask Neal to test at early, middle and late portions of the recording.

Verify:

1. Space pauses as before.

2. Right tap remains +1.

3. Right hold remains accepted forward scrubbing.

4. Shift+Right remains +5.

5. Left tap moves exactly -1.

6. Repeated Left taps reliably walk backward one tick at a time.

7. Left hold repeatedly scrubs backward.

8. Releasing Left stops backward scrubbing immediately.

9. Shift+Left moves exactly -5.

10. Shift+Left near boundary 0 stops at 0.

11. Holding Shift+Left repeatedly moves backward in five-tick groups.

12. PAUSED boundary display tracks all reverse operations.

13. At boundary 0, further Left input does nothing.

14. Right works correctly immediately after rewinding.

15. Rewind -1 followed by forward +1 visibly returns to the same state.

16. -5 followed by +5 visibly returns to the same state.

17. Space resumes playback from the reconstructed earlier boundary.

18. Audio does not replay historical events while rewinding.

19. Normal playback audio resumes correctly.

20. Reverse scrubbing remains responsive late in levels1-7.json.

21. Final recorded boundary can be scrubbed backward.

22. Normal live controls return after playback completion/handoff.

23. No playback fidelity regression, crash, skipped frame, duplicated frame, or
    unexpected input occurs.


SNAPSHOT ACCEPTANCE DECISION

After manual testing:

If reverse scrubbing is comfortably responsive across levels1-7.json:

record that replay-from-start is sufficient and snapshots remain deferred.

If noticeable latency harms usability:

record exactly where/how without implementing snapshots yet.

Stop for review.


FINAL GIT CHECK

Run:

git status --short --branch
git rev-parse HEAD
git rev-parse origin/master
git diff --stat
git diff --name-status
git diff
git diff --check
git diff --cached --stat
git ls-files --others --exclude-standard

Expected HEAD remains:

a028cdc4156ea5d0fbccd660e4fc427226daa390

Require:

- Phase 3B candidate unstaged;
- no commit;
- no push;
- no recording modification.


FINAL RESPONSE

Return:

1. Reconstruction API/design.
2. Left-tap behavior.
3. Left-repeat behavior.
4. Shift+Left behavior.
5. Shift+Left-repeat behavior.
6. Boundary-zero behavior.
7. Final-boundary behavior.
8. Right-after-rewind behavior.
9. Round-trip determinism results.
10. PlayerInput separation.
11. Audio behavior.
12. Rendering behavior.
13. Overlay behavior.
14. Live-input handoff.
15. v1/v2 preservation.
16. Phase 3A CLI preservation.
17. Snapshot status.
18. Production files changed.
19. Tests changed.
20. Documentation changed.
21. Focused counts.
22. Neighboring counts.
23. Full-suite counts.
24. Build warnings/errors.
25. git diff --check.
26. Manual launch command.
27. Manual acceptance checklist.
28. Report filename.
29. Final Git status.
30. Confirmation no snapshots, recording-format changes, benchmark code,
    enemy-explosion implementation, Level 8/9, stage, commit, or push occurred.

Stop for Neal's manual acceptance.

TEMPEST_REVERSE_PLAYBACK_SCRUBBING_PHASE3B_COMPLETE