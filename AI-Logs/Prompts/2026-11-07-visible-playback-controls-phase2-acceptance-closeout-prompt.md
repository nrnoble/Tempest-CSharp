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


TEMPEST VISIBLE PLAYBACK CONTROLS PHASE 2 ACCEPTANCE CLOSEOUT

Close out, validate, commit, and push the manually accepted visible
recorded-playback controls Phase 2 milestone, including its forward-navigation
refinement.

Neal has completed manual visual acceptance.

This task is limited to:

1. recording Phase 2 manual acceptance;
2. reconciling durable documentation;
3. final automated validation;
4. reviewing the complete accumulated Phase 2 diff;
5. staging only Phase 2;
6. committing it;
7. pushing the accepted checkpoint to the existing private GitHub remote.

Do not begin Phase 3A or Phase 3B.

Do not implement backward navigation.

Do not implement a headless CLI.

Do not implement snapshots.

Do not begin Level 8 or Level 9 work.


ACTIVE REPOSITORY

Work only in:

C:\Users\nrnob\source\repos\Tempest


EXPECTED STARTING BASELINE

Expected branch:

master

Expected committed HEAD and origin/master:

1fd28c1b9a7eaad0a0a1ed35c781f57a8b3d90af
Add shared deterministic playback session

Expected:

- Phase 2 implementation candidate is unstaged;
- index clean;
- master tracks origin/master;
- no unrelated tracked or nonignored untracked work.

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

AI-Logs/Reports/2026-11-07-visible-playback-controls-phase2-report.md

Also read the Phase 1 closeout report and inspect the actual current source,
tests, documentation, and complete accumulated diff.

Verify rather than assuming report contents.


MANUAL ACCEPTANCE

Record:

VISIBLE RECORDED-PLAYBACK CONTROLS PHASE 2:
MANUALLY ACCEPTED.

Neal manually tested the initial Phase 2 implementation and its navigation
refinement.

Neal reports the result is extremely successful and describes the overall
interaction as playback "scrubbing", analogous to the frame-by-frame and
shuttle/scrub inspection familiar from film/video editing.

The playback controls are intended for ANALYZING gameplay that already occurred,
not interacting with or changing the recorded gameplay.


MANUALLY ACCEPTED CONTROLS

Neal verified all of the following:


SPACE

- toggles pause/resume;
- playback pauses at the current deterministic simulation boundary;
- playback resumes correctly from that boundary.


RIGHT ARROW TAP

- advances exactly one simulation tick while paused;
- one tick per tap is ideal;
- repeated taps support precise frame/tick-by-tick analysis.


RIGHT ARROW HOLD

- repeats the SAME one-tick-forward operation;
- native keyboard-repeat behavior is comfortable and useful;
- releasing Right immediately stops repeated stepping;
- the observed repeat cadence happens to produce something close to real-time
  playback on Neal's system.

IMPORTANT:

Do NOT tune the repeat rate to target real-time playback.

Near-real-time behavior is a useful side effect, not a simulation-speed
requirement.

The invariant is:

each repeat event = exactly one ordinary StepForward operation.


SHIFT + RIGHT ARROW

- advances exactly five ordinary simulation ticks;
- the paused boundary counter advances by exactly five when sufficient frames
  remain;
- the operation is five sequential normal StepForward calls, not skipped input
  or a changed simulation delta.


SHIFT + RIGHT ARROW HOLD

- repeats five-tick groups using native keyboard-repeat events;
- each group still executes every underlying recorded simulation tick.


END OF RECORDING

Neal verified:

- +5 stepping near the end stops cleanly at the final boundary;
- it does not run beyond recording duration;
- no nonexistent recorded frame is consumed.


LEFT ARROW

- remains inert during recorded playback;
- it does not move the recorded Blaster;
- it does not consume frames;
- it remains reserved for future Phase 3B backward reconstruction.


SHIFT + LEFT ARROW

- remains inert;
- it is reserved for future -5-tick navigation.


NORMAL PLAYBACK

Neal verified:

- normal playback fidelity remains correct;
- Space resumes correctly;
- playback completes correctly;
- ordinary live controls return after playback handoff.


PAUSED OVERLAY

The accepted overlay remains:

PAUSED  boundary / duration

Neal accepts the overlay and boundary behavior.


SCRUBBING EXPERIENCE

Record this usability finding:

The combination of:

- +1 tick tap;
- repeated +1 while holding Right;
- immediate stop on release;
- +5 Shift navigation;

provides an effective playback-scrubbing workflow.

This makes it practical to:

- move quickly through uninteresting portions;
- release near an interesting gameplay event;
- inspect exact behavior one simulation tick at a time.

This behavior should be preserved.

Do not convert it into a conventional video-style playback-rate control merely
because held Right happens to approximate normal playback speed.


WHY THIS MATTERS

Record the development purpose:

Tick-by-tick playback has already exposed gameplay/presentation details that are
difficult to see while simultaneously playing Tempest.

For example, Neal has already noticed a separate deferred fidelity issue:

Enemies destroyed by a Blaster shot currently disappear immediately in the C#
game, whereas Neal observes that the original arcade replaces a destroyed enemy
with a small, brief explosion animation.

This is NOT part of Phase 2.

Record it only as a deferred future visual-fidelity milestone if it is not
already durably recorded.

Do not implement the enemy explosion now.


PHASE 2 IMPLEMENTATION TO FREEZE

Preserve:

- file playback routed through GameplayPlaybackSession;
- GameplayPlaybackSession authoritative for recorded-frame execution;
- VisiblePlaybackController;
- separate PlaybackControlInput;
- playback controls outside PlayerInput;
- Space pause/resume;
- Right Arrow tap +1;
- native Right Arrow repeat +1;
- Shift+Right +5;
- native Shift+Right repeat +5 groups;
- Left/Shift+Left inertness;
- pause overlay;
- exact boundary reporting;
- final-boundary behavior;
- live-input handoff;
- restoration of normal gameplay controls after handoff.


NATIVE REPEAT POLICY

The installed Raylib-cs binding exposes IsKeyPressedRepeat.

Preserve its use for Right Arrow scrubbing.

Do not replace it with:

- IsKeyDown every rendered frame;
- a custom fixed timer;
- a hardcoded repeat frequency;
- a simulation-speed multiplier.

Neal explicitly accepts the current native-repeat feel as perfect for this use.


PLAYER INPUT SEPARATION

Preserve the strict distinction:

PLAYBACK ANALYSIS CONTROL:

Space
Right
Shift+Right
future Left/Shift+Left

RECORDED GAMEPLAY INPUT:

spinner/movement
Fire
Super Zapper
controller axis
restart
etc.

Playback analysis controls must not:

- enter PlayerInput;
- get serialized;
- change recording identity;
- alter deterministic recorded gameplay.


AUDIO

Preserve accepted Phase 2 behavior:

- paused redraw emits no new simulation audio;
- existing voices may finish naturally;
- +1 ticks emit their ordinary events;
- +5 executes the ordinary audio behavior of its constituent simulation ticks;
- resume restores ordinary playback audio.

Do not introduce another audio policy.


RECORDING FORMAT

Do not change:

- format identifier;
- v1;
- v2;
- startup metadata;
- frame structure;
- recording capture;
- strict validation;
- save semantics.

Playback controls remain UI/session state only.


PHASE 1 PRESERVATION

GameplayPlaybackSession and GameplayStateSummary must remain Raylib-independent.

Do not move:

- key polling;
- rendering;
- window handling;
- real-time pacing;

into the shared playback session.


FUTURE ROADMAP

Record the accepted sequence:


PHASE 3A

Headless playback runner for VS Code Copilot:

- reusable API;
- thin development CLI;
- load v1/v2 recordings;
- run to selected boundary;
- structured GameplayStateSummary output;
- assertions/checkpoints;
- nonzero failure behavior.


PHASE 3B

Backward visual playback:

LEFT TAP:
-1 tick.

LEFT HOLD:
repeated -1 tick reconstruction.

SHIFT + LEFT:
-5 ticks.

SHIFT + LEFT HOLD:
repeated -5 groups.

Use deterministic reconstruction from recorded startup rather than reversing
game mutations.


AFTER PHASE 3B

Evaluate:

- continuous rewind/scrubbing;
- explicit faster forward navigation if still useful;
- run/seek to target boundary;
- bookmarks;
- loops;
- comparison tools.


SNAPSHOTS

Remain deferred.

Try replay-from-start reconstruction and measure performance before considering
snapshots.


NO GAMEPLAY CHANGES

Do not modify:

- Levels 1-7;
- enemies;
- Flippers;
- Tankers/cargo;
- Spikes;
- firing;
- Super Zapper gameplay;
- death/re-entry;
- score;
- progression;
- Game Over;
- transitions;
- recording semantics;
- direct-level startup;
- ordinary controls outside playback mode;
- audio balance;
- enemy destruction presentation.


DOCUMENTATION

Reconcile durable documentation to change specifically relevant Phase 2 wording
from candidate/manual-pending to:

MANUALLY ACCEPTED.

Record:

- Neal's successful initial Phase 2 acceptance;
- successful navigation-refinement acceptance;
- scrubbing use case;
- one tick per tap is ideal;
- native repeated +1 behavior is accepted;
- current native repeat rate must not be retuned merely to target real time;
- Shift+Right +5 accepted;
- live-input restoration accepted;
- Left/Shift+Left future semantics;
- future Phase 3A/3B sequence;
- no recording-format change.

Do not rewrite unrelated historical reports.


AI LOGS

Preserve complete task as:

AI-Logs/Prompts/2026-11-07-visible-playback-controls-phase2-acceptance-closeout-prompt.md

Create:

AI-Logs/Reports/2026-11-07-visible-playback-controls-phase2-acceptance-closeout-report.md

Report must include:

1. Starting Git state.
2. Manual acceptance.
3. User's scrubbing goal.
4. Visible playback migration.
5. Space semantics.
6. Right-tap semantics.
7. Right-hold repeat semantics.
8. Decision not to retune repeat rate.
9. Shift+Right semantics.
10. Shift+Right repeat semantics.
11. End-boundary behavior.
12. Left/Shift+Left reservation.
13. PAUSED overlay.
14. Audio.
15. PlayerInput separation.
16. Live-input handoff.
17. Recording-format preservation.
18. Phase 1 preservation.
19. Deferred enemy-destruction explosion observation if durably recorded.
20. Phase 3A/3B roadmap.
21. Production files changed.
22. Test files changed.
23. Documentation changed.
24. Focused test counts.
25. Neighboring regression counts.
26. Full-suite counts.
27. Build warnings/errors.
28. git diff --check.
29. staged file list.
30. staged diff check.
31. Commit message/hash.
32. Push result.
33. Final Git state.
34. origin/master verification.


FINAL AUTOMATED VALIDATION

Run focused Phase 2 coverage for:

- session-backed visible playback;
- Space pause/resume;
- Right +1;
- native repeat +1;
- Shift+Right +5;
- repeated +5;
- final boundary;
- inert Left/Shift+Left;
- PAUSED overlay;
- live-input handoff;
- normal control restoration.

Run neighboring regression covering:

- Phase 1 session;
- state summary;
- v1/v2 recording;
- recording capture;
- playback;
- input/Super Zapper;
- direct-level startup;
- death/re-entry;
- progression;
- campaign soak;
- Levels 5 and 7;
- relevant presentation/rendering behavior.

Then run:

dotnet test TempestCSharp.slnx

dotnet build TempestCSharp.slnx

git diff --check

Tests must actually execute.

Require:

- 0 failed;
- 0 unexpected skipped;
- build 0 errors;
- warning count accurately reported;
- diff check passes.


NO INTERACTIVE GAME LAUNCH

Do not launch the game.

Neal's manual acceptance has already passed.


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

Verify the accumulated diff contains only the accepted Phase 2 implementation,
navigation refinement, tests, and documentation.

Verify no:

- backward navigation;
- rewind;
- snapshots;
- headless CLI;
- recording-format change;
- Level 8/9;
- unrelated gameplay;
- enemy explosion implementation;
- generated recording;
- Temp/TEMP;
- Videos;
- bin/obj;
- Git configuration change.


STAGING

After successful review, stage ONLY Phase 2.

Reconcile exact paths against actual diff.

Do not stage AI-Logs.

Do not stage recordings or Temp/TEMP content.

Run:

git status --short
git diff --cached --stat
git diff --cached --name-status
git diff --cached
git diff --cached --check
git diff --stat

Require:

- bounded staged patch;
- no unrelated file;
- no unintended deletion;
- no unresolved unstaged tracked change;
- staged diff check passes.


COMMIT

If correct, commit using:

git commit -m "Add visible recorded playback controls"

Do not amend previous commits.

Use established repository co-author policy if required.


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
2. Scrubbing workflow.
3. Space behavior.
4. Right-tap behavior.
5. Right-hold behavior.
6. Repeat-rate decision.
7. Shift+Right behavior.
8. Shift+Right-hold behavior.
9. Final-boundary behavior.
10. Left/Shift+Left future behavior.
11. Overlay result.
12. Audio result.
13. PlayerInput separation.
14. Live-input restoration.
15. Recording-format preservation.
16. Phase 1 preservation.
17. Phase 3A/3B roadmap.
18. Deferred enemy-explosion note status.
19. Production files committed.
20. Tests committed.
21. Documentation committed.
22. Focused counts.
23. Neighboring counts.
24. Full-suite counts.
25. Build warnings/errors.
26. git diff --check.
27. staged diff check.
28. Commit message.
29. New commit hash.
30. Push result.
31. Final Git status.
32. origin/master verification.
33. Closeout report filename.
34. Confirmation no backward navigation, rewind, snapshots, headless CLI,
    recording change, enemy-explosion implementation, Level 8/9, force-push,
    remote change, or unrelated gameplay work occurred.

Stop after successful push.

TEMPEST_VISIBLE_PLAYBACK_PHASE2_ACCEPTANCE_CLOSEOUT_COMPLETE