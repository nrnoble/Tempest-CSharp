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


TEMPEST HEADLESS PLAYBACK RUNNER PHASE 3A ACCEPTANCE CLOSEOUT

Close out, validate, commit, and push the manually accepted Phase 3A headless
recorded-playback runner.

Phase 3A has passed automated validation and Neal has now tested it with several
real gameplay recordings.

This task is limited to:

1. recording Phase 3A manual acceptance;
2. reconciling durable documentation;
3. running final automated validation;
4. reviewing the complete Phase 3A diff;
5. staging only Phase 3A;
6. committing it;
7. pushing the accepted checkpoint to the existing private GitHub remote.

Do not begin Phase 3B.

Do not implement backward playback.

Do not implement snapshots.

Do not modify Phase 2 visual controls.

Do not begin Level 8/9 work.


ACTIVE REPOSITORY

Work only in:

C:\Users\nrnob\source\repos\Tempest


EXPECTED STARTING BASELINE

Expected branch:

master

Expected committed HEAD and origin/master:

358fca9229153e4f0fefe51fbedd711696095955
Add visible recorded playback controls

Expected candidate worktree from the Phase 3A report:

MODIFIED:

README.md
TEMPEST-BEHAVIOR-SPEC.md
TEMPEST-DEVELOPMENT-NOTES.md
src/Tempest.Game/Program.cs

ADDED:

src/Tempest.Game/Debug/HeadlessGameplayPlaybackCli.cs
src/Tempest.Game/Debug/HeadlessGameplayPlaybackRunner.cs
tests/Tempest.Game.Tests/HeadlessGameplayPlaybackTests.cs

Expected index:

clean

AI-Logs are intentionally ignored.

Before changing anything run:

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

Read:

AI-Logs/Reports/2026-11-06-recorded-playback-debugger-headless-analysis-report.md

AI-Logs/Reports/2026-11-07-shared-playback-session-phase1-closeout-report.md

AI-Logs/Reports/2026-11-07-visible-playback-controls-phase2-acceptance-closeout-report.md

AI-Logs/Reports/2026-11-07-headless-playback-runner-phase3a-report.md

If one of the historical filenames differs slightly, locate the corresponding
report rather than assuming it is missing.

Inspect the complete current diff and actual current versions of:

HeadlessGameplayPlaybackRunner.cs
HeadlessGameplayPlaybackCli.cs
GameplayPlaybackSession.cs
GameplayStateSummary.cs
Program.cs
HeadlessGameplayPlaybackTests.cs

Also review the recording loader and relevant recording/playback tests.

Current source is authoritative.


MANUAL ACCEPTANCE

Record:

HEADLESS RECORDED-PLAYBACK RUNNER PHASE 3A:
MANUALLY ACCEPTED.

Neal tested Phase 3A with real gameplay recordings stored under:

Temp\GameplayRecordings\

These recordings are disposable/ignored development recordings and must not be
committed.


REAL RECORDING TEST 1

Neal ran:

dotnet run --project src\Tempest.Game -- --headless-playback Temp\GameplayRecordings\testlevel5-3.json --boundary 4820 --json

The requested boundary was reached exactly:

requestedBoundary = 4820
achievedBoundary = 4820
recordingDurationTicks = 7260
remainingFrames = 2440
completionReason = RequestedBoundaryReached

The returned structured state was coherent with the recording, including:

starting displayed level = 5
starting lives = 12
starting rim-speed multiplier = 1
current displayed level = 7
LevelState = Playing
score = 11894
remaining lives = 10
player lane = 3
Super Zapper charges = 2

The command completed in less than a perceptible amount of ordinary waiting for
Neal.

Do not turn that observation into a formal benchmark claim.


REAL RECORDING TEST 2

Neal ran the same recording to completion:

dotnet run --project src\Tempest.Game -- --headless-playback Temp\GameplayRecordings\testlevel5-3.json

Result:

achieved boundary = 7260 / 7260
remaining frames = 0
completion = RecordingCompleted

Final reported state:

Level 7 Complete
score 21805
lives 8

The runner completed correctly without interactive gameplay.


INVALID BOUNDARY TEST

Neal ran:

dotnet run --project src\Tempest.Game -- --headless-playback Temp\GameplayRecordings\testlevel5-3.json --boundary 999999

It correctly rejected the invalid target:

OUT-OF-RANGE BOUNDARY (5):
Requested boundary 999999 is outside 0 through 7260.

This behavior is manually accepted.


NEW REAL RECORDING TESTS

Neal also created new real recordings.


LEVEL 1 RECORDING

Command:

dotnet run --project src\Tempest.Game -- --headless-playback Temp\GameplayRecordings\level1.json

Result:

achieved boundary = 797 / 797
remaining frames = 0
completion = RecordingCompleted

Final state:

Level 2 Approach
score 1725
lives 20


FULL LEVELS 1–7 RECORDING

Command:

dotnet run --project src\Tempest.Game -- --headless-playback Temp\GameplayRecordings\levels1-7.json

Result:

achieved boundary = 12761 / 12761
remaining frames = 0
completion = RecordingCompleted

Final state:

Level 7 Complete
score 39545
lives 15

This long recording contains 12,761 recorded simulation ticks.

Neal reports that processing the entire recording was effectively
imperceptible in normal use — less than a perceptible second from his
perspective.

No formal timing instrumentation was used.

No formal performance benchmark is required.

Record the correct conclusion narrowly:

The real 12,761-tick Levels 1–7 recording completes sufficiently quickly for
the intended development/debugging use, with no perceptible delay observed by
Neal in ordinary manual testing.

Do NOT claim a measured sub-second benchmark.


PERFORMANCE DECISION

Neal considers the practical performance question answered.

Do not add benchmarking code.

Do not add timers merely to produce a numeric performance figure.

The observed performance is sufficient to justify trying deterministic
replay-from-start reconstruction in Phase 3B before considering snapshots.


ACCEPTED PHASE 3A ARCHITECTURE

Preserve:

- HeadlessGameplayPlaybackRunner;
- thin development-only headless CLI;
- existing GameplayRecordingFile loader;
- shared GameplayPlaybackSession;
- production TempestGame.RunOneStep;
- existing boundary semantics;
- GameplayStateSummary;
- SilentAudioEvents;
- no Raylib window;
- no rendering;
- no normal audio device;
- no real-time pacing;
- no live input;
- no live-input handoff;
- read-only recording behavior.


BOUNDARY CONTRACT

Preserve:

Boundary 0:
restored startup before recorded frame 0.

After frame 0:
Boundary 1.

After D recorded frames:
Boundary D.

RunToBoundary(N):

- reaches exactly N;
- does not consume frame N when stopping at N;
- rejects negative values;
- rejects values greater than recording duration;
- never silently clamps;
- never skips simulation frames.


RUN TO COMPLETION

Preserve:

- sequential execution through recording duration;
- remaining frames = 0 at completion;
- RecordingCompleted result;
- no live controls afterward;
- no Raylib startup.


CLI

Preserve the accepted development syntax:

dotnet run --project src\Tempest.Game -- --headless-playback <recording-file>

and:

dotnet run --project src\Tempest.Game -- --headless-playback <recording-file> --boundary N --json

Preserve actual current parser grammar after verifying source.

Do not rename accepted options during closeout.


EXIT CODES

Preserve the Phase 3A explicit failure classifications reported by the
implementation:

0 = success

2 = invalid command/incompatible option

3 = recording load/validation failure

4 = simulation failure

5 = out-of-range boundary

Do not convert failures into success exits.


EXCEPTION BEHAVIOR

Preserve full fail-visible behavior.

Simulation failures must remain diagnosable.

Do not add broad exception suppression.

If the CLI catches an exception to format diagnostic output, it must retain
meaningful exception information and return nonzero.


JSON OUTPUT

Preserve the explicit diagnostic JSON schema.

It should continue to report:

- requested boundary;
- achieved boundary;
- recording duration;
- remaining frames;
- completion reason;
- bounded GameplayStateSummary;
- stable Flipper diagnostics.

Do not serialize arbitrary internal object graphs.


VS CODE COPILOT PURPOSE

Record that the accepted Phase 3A runner now gives VS Code Copilot a practical
headless analysis interface.

Intended future AI workflow:

same recording Neal can scrub visually
->
headless run to exact boundary N
->
structured GameplayStateSummary
->
inspect nearby boundaries
->
reason about gameplay behavior
->
create focused regression tests when appropriate.

This is explicit project-owned state inspection, not arbitrary process-memory
inspection.


VISIBLE/HEADLESS COMPLEMENT

Record the accepted two-view workflow:

NEAL:

visible deterministic recorded playback
->
pause/scrub
->
identify interesting gameplay event.

VS CODE COPILOT:

same recording
->
headless exact-boundary execution
->
structured internal gameplay state.

Both use the same accepted deterministic playback session and production
simulation.


PHASE 2 PRESERVATION

Do not modify accepted visible playback controls:

Space = pause/resume

Right tap = +1 tick

Right hold = repeated +1 using native repeat

Shift+Right = +5 ticks

Shift+Right hold = repeated +5 groups

Left/Shift+Left = inert pending Phase 3B

PAUSED boundary/duration overlay

normal live-input handoff


RECORDING FORMAT PRESERVATION

Do not alter:

- recording format identifier;
- v1 schema;
- v2 schema;
- startup metadata;
- frame fields;
- strict validation;
- save behavior.

Headless execution remains read-only.


GENERATED RECORDINGS

The real recordings used for manual Phase 3A acceptance remain under:

Temp\GameplayRecordings\

Do not stage them.

Do not commit them.

Do not copy them elsewhere.

They remain development recordings.

A later separately authorized milestone may deliberately curate a recording as
a regression fixture if useful.


PHASE 3B ROADMAP

After this clean Phase 3A checkpoint is pushed, the next planned milestone is
backward visual scrubbing.

Accepted intended controls:

Left tap:
-1 tick.

Left hold:
repeated -1 operations.

Shift+Left:
-5 ticks.

Shift+Left hold:
repeated -5 groups.

Initial mechanism:

restore recorded startup
->
headlessly replay deterministically from boundary 0
->
stop at earlier requested boundary
->
replace visible playback state with reconstructed state
->
render that earlier boundary.

Do not reverse collisions, RNG draws, scores, ledger changes, enemy lifecycle,
or other simulation mutations directly.


SNAPSHOT DECISION

Snapshots remain deferred.

The observed headless performance of the 12,761-tick real recording strongly
supports trying replay-from-start first.

Do not implement snapshots unless later practical Phase 3B testing shows
reconstruction latency is actually unacceptable.


DEFERRED

Do not implement during closeout:

- backward stepping;
- rewind;
- snapshots;
- Random-state serialization;
- faster playback controls;
- seek UI;
- timeline;
- bookmarks;
- loops;
- first-divergence CLI;
- deterministic hashes;
- permanent recording fixture;
- enemy destruction explosion;
- Level 8/9.


DEFERRED ENEMY EXPLOSION

Preserve the previously recorded deferred observation:

In the current C# game, an enemy destroyed by a Blaster shot disappears
immediately.

Neal observes that the original arcade briefly replaces the destroyed enemy
with a small explosion animation.

This remains a future visual-fidelity milestone.

Do not implement it here.


DOCUMENTATION

Reconcile durable documentation so Phase 3A becomes:

HEADLESS RECORDED-PLAYBACK RUNNER PHASE 3A:
MANUALLY ACCEPTED.

Replace relevant candidate/manual-pending wording.

Record:

- automated acceptance;
- real-file manual acceptance;
- exact-boundary success;
- completion success;
- invalid-boundary rejection;
- level1.json result;
- levels1-7.json result;
- 12,761-tick practical performance observation;
- explicit decision not to benchmark further;
- shared session;
- no Raylib;
- SilentAudioEvents;
- JSON output;
- exit-code policy;
- read-only file behavior;
- VS Code Copilot workflow;
- Phase 2 preservation;
- Phase 3B next;
- snapshots deferred pending actual need.

Do not rewrite unrelated historical reports.


AI LOGS

Preserve this complete task as:

AI-Logs/Prompts/2026-11-07-headless-playback-runner-phase3a-acceptance-closeout-prompt.md

Create:

AI-Logs/Reports/2026-11-07-headless-playback-runner-phase3a-acceptance-closeout-report.md

The report must include:

1. Starting Git state.
2. Manual acceptance.
3. Automated acceptance.
4. Runner architecture.
5. Shared-session reuse.
6. Boundary semantics.
7. Completion semantics.
8. JSON output.
9. CLI syntax.
10. Exit-code policy.
11. Exception policy.
12. Silent-audio policy.
13. Raylib independence.
14. v1/v2 compatibility.
15. testlevel5-3 boundary-4820 result.
16. testlevel5-3 completion result.
17. invalid-boundary result.
18. level1.json result.
19. levels1-7.json result.
20. 12,761-tick practical performance observation.
21. Explicit decision not to benchmark further.
22. VS Code Copilot workflow.
23. Phase 2 preservation.
24. Recording-file safety.
25. Phase 3B roadmap.
26. Snapshot decision.
27. Deferred enemy-explosion note.
28. Production files changed.
29. Test files changed.
30. Documentation changed.
31. Focused test counts.
32. Neighboring regression counts.
33. Full-suite counts.
34. Build warnings/errors.
35. git diff --check.
36. Complete staged file list.
37. staged diff check.
38. Commit message/hash.
39. Push result.
40. Final Git status.
41. origin/master verification.
42. Confirmation no Phase 3B implementation, backward navigation, snapshots,
    recording-format change, visible-control change, enemy-explosion work,
    Level 8/9 work, force-push, or remote reconfiguration occurred.


FINAL AUTOMATED VALIDATION

Run focused Phase 3A tests for:

- runner API;
- v1;
- v2;
- boundary 0;
- exact boundary execution;
- completion;
- invalid range;
- JSON output;
- stable Flipper diagnostics;
- CLI parsing/conflicts;
- exit codes;
- simulation-failure behavior;
- read-only recording;
- no-live-input/no-live-handoff.

Run neighboring regression covering:

- GameplayPlaybackSession;
- GameplayStateSummary;
- visible Phase 2 controls;
- recording/playback;
- input;
- direct-level startup;
- death/re-entry;
- progression;
- campaign soak;
- Level 5;
- Level 7;
- Tanker/cargo;
- Super Zapper;
- audio events.

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
- diff check passes.


NO ADDITIONAL MANUAL TESTING

Do not launch the game.

Do not create additional recordings.

Do not run performance benchmarks.

Neal's Phase 3A manual acceptance is complete.


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

Verify the accumulated diff contains only Phase 3A runner/CLI, tests, and
appropriate documentation.

Verify no:

- Phase 3B;
- backward step;
- rewind;
- snapshot;
- recording-format modification;
- Phase 2 visual-control change;
- gameplay change;
- enemy explosion;
- Level 8/9;
- generated recording;
- Temp/TEMP staged content;
- Videos;
- bin/obj;
- reference changes;
- Git configuration changes.


STAGING

After validation and review, stage ONLY Phase 3A.

Expected candidate paths from the implementation report:

README.md

TEMPEST-BEHAVIOR-SPEC.md

TEMPEST-DEVELOPMENT-NOTES.md

src/Tempest.Game/Program.cs

src/Tempest.Game/Debug/HeadlessGameplayPlaybackCli.cs

src/Tempest.Game/Debug/HeadlessGameplayPlaybackRunner.cs

tests/Tempest.Game.Tests/HeadlessGameplayPlaybackTests.cs

Reconcile exact paths against the actual diff.

Do not stage AI-Logs.

Do not stage anything under Temp/TEMP.

Then run:

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
- no unstaged tracked change requiring resolution;
- staged diff check passes.


COMMIT

If correct, commit with:

git commit -m "Add headless recorded playback runner"

Do not amend previous commits.

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

Do not modify the remote.

Do not create a branch.

After push verify:

git status --short --branch
git rev-parse HEAD
git rev-parse origin/master

Require local HEAD == origin/master.


FINAL RESPONSE

Return:

1. Manual acceptance.
2. Runner API.
3. CLI syntax.
4. Boundary semantics.
5. Completion behavior.
6. JSON output.
7. Exit-code policy.
8. Exception behavior.
9. Silent-audio behavior.
10. Raylib independence.
11. v1/v2 compatibility.
12. Real boundary-4820 result.
13. Real 7,260-tick completion result.
14. Invalid-boundary result.
15. level1.json result.
16. levels1-7.json 12,761-tick result.
17. Practical performance result.
18. Confirmation no benchmark was needed.
19. VS Code Copilot workflow.
20. Phase 2 preservation.
21. Recording-file safety.
22. Phase 3B roadmap.
23. Snapshot decision.
24. Deferred enemy-explosion status.
25. Production files committed.
26. Tests committed.
27. Documentation committed.
28. Focused counts.
29. Neighboring counts.
30. Full-suite counts.
31. Build warnings/errors.
32. git diff --check.
33. staged diff check.
34. Commit message.
35. New commit hash.
36. Push result.
37. Final Git status.
38. origin/master verification.
39. Closeout report filename.
40. Confirmation no backward navigation, rewind, snapshot, recording-format
    change, visible-control change, enemy-explosion implementation, Level 8/9,
    force-push, remote change, or unrelated gameplay work occurred.

Stop after successful push.

TEMPEST_HEADLESS_PLAYBACK_PHASE3A_ACCEPTANCE_CLOSEOUT_COMPLETE