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


TEMPEST SHARED DETERMINISTIC PLAYBACK SESSION — PHASE 1

Implement the first bounded milestone from the accepted recorded-playback
debugger/headless-runner analysis.

PHASE 1 PURPOSE:

Create the shared deterministic playback execution foundation that future:

- visible pause/resume;
- visible forward stepping;
- backward stepping;
- fast forward;
- rewind;
- headless recording execution;
- VS Code Copilot state inspection

will all use.

This milestone must NOT yet add visible playback controls.

Do NOT add Space pause/resume yet.

Do NOT add Right Arrow stepping yet.

Do NOT add Left Arrow backward stepping yet.

Do NOT add fast forward or rewind yet.

Do NOT add snapshots.

Do NOT add a headless command-line mode yet.

Do NOT begin Level 8/9 work.


ACTIVE REPOSITORY

Work only in:

C:\Users\nrnob\source\repos\Tempest


EXPECTED CLEAN BASELINE

Expected branch:

master

Expected HEAD and origin/master:

7032a7219cde69a0ae25b00e5f3bc9ac1ee3ed04
Use vector text for Avoid Spikes

Require:

- clean tracked worktree;
- clean index;
- master tracking origin/master;
- local master matching origin/master;
- no unexplained nonignored untracked files.

Before modifying anything run:

git status --short --branch
git rev-parse HEAD
git log -1 --oneline
git rev-parse origin/master
git diff --stat
git diff --cached --stat
git ls-files --others --exclude-standard
git remote -v

If anything differs unexpectedly, STOP rather than resetting, cleaning,
restoring, deleting, or discarding anything.


READ FIRST

Read:

.github/copilot-instructions.md
README.md
TEMPEST-DEVELOPMENT-NOTES.md
TEMPEST-BEHAVIOR-SPEC.md

Read:

AI-Logs/Reports/2026-11-06-recorded-playback-debugger-headless-analysis-report.md

Also read relevant accepted reports for:

- deterministic gameplay playback;
- recorded-input round trip;
- recording/playback v2;
- recording/playback v2 acceptance;
- generic direct-level startup.

Inspect current source/tests including:

Program.cs
DebugController.cs
GameplayPlayback.cs
GameplayRecordingFile.cs
GameplayStartupConfiguration.cs
PlayerInputReplay.cs
PlayerInputRecorder.cs
InputSystem.cs
TempestGame.cs
GameplayPlaybackTests.cs
GameplayRecordingTests.cs

and any simulation/state tests needed to design the shared session correctly.

Current source is authoritative.


USER'S LONG-TERM PLAYBACK GOAL

Preserve this future requirement in the design.

Recorded-file playback is observation of gameplay that already happened.

It should eventually allow Neal to visually inspect difficult gameplay using
controls conceptually equivalent to:

SPACE:
toggle pause / resume

RIGHT ARROW while paused:
advance exactly one simulation tick

LEFT ARROW while paused:
move backward exactly one simulation tick using deterministic reconstruction

Later:
fast forward and rewind

These controls are future playback-debugger commands.

They are NOT gameplay input and must never enter PlayerInput or be written into
a gameplay recording.

Do not implement those controls during this Phase 1 milestone.


VS CODE COPILOT GOAL

The same recording should eventually be usable by VS Code Copilot without the
game UI.

The AI should eventually be able to:

- load a recording;
- run deterministically to tick/boundary N;
- inspect structured state;
- compare nearby ticks;
- test invariants;
- identify meaningful gameplay behavior;
- potentially turn difficult recordings into regression fixtures.

Do not implement the headless CLI yet.

Phase 1 should establish the shared reusable execution and inspection foundation
that makes this possible.


ACCEPTED PLAYBACK CONTRACT

Protect the accepted recording/playback behavior.

Preserve:

- recording format v1 compatibility;
- recording format v2 compatibility;
- one effective input frame per simulation tick;
- PlayerInput meanings;
- restart-bit meanings;
- recorded startup seed;
- recorded starting production level;
- recorded starting lives;
- recorded rim-speed multiplier;
- normal production initialization;
- normal production simulation;
- existing update ordering;
- existing RNG behavior;
- normal progression;
- existing recording validation;
- existing visible playback fidelity;
- existing live-input handoff after visible playback.

Do not modify the recording file format.


SHARED GAMEPLAY PLAYBACK SESSION

Introduce a small reusable deterministic playback execution abstraction.

Use an appropriate maintainable name, preferably:

GameplayPlaybackSession

or a clearly equivalent project-consistent name.

The session should encapsulate the deterministic FILE-REPLAY execution that is
currently partly orchestrated inside Program.cs.

The shared session must be usable without Raylib.

It should own or expose as appropriate:

- validated recording startup configuration;
- production TempestGame instance;
- recorded frame source/cursor;
- current simulation boundary;
- recording duration;
- remaining recorded frames;
- recording-complete state.

It should execute the existing production simulation.

Do not create a second game simulation.


BOUNDARY SEMANTICS

Adopt the analysis report's explicit simulation-boundary contract.

BOUNDARY 0:

The restored recorded startup state before consuming any recorded input frame.

FRAME 0:

The first recorded gameplay input frame.

Executing frame 0:

- applies that recorded frame and its restart bit;
- calls TempestGame.RunOneStep exactly once;
- advances the recording cursor exactly once;
- produces BOUNDARY 1.

Generally:

after consuming recorded frame N:

the resulting game state is boundary N+1.

At recording duration D:

boundary D is the state after all D recorded frames have been executed.

Keep this definition explicit in code/tests/comments so future forward/backward
navigation cannot develop off-by-one ambiguity.


RECORDED INPUT GATING

Program.cs currently applies state-specific production input/restart gates before
RunOneStep.

Centralize the FILE-PLAYBACK equivalent of those accepted gates in the shared
playback execution seam so visible and future headless playback cannot diverge.

Preserve exact accepted semantics.

Do not change gameplay input behavior.

Do not change ordinary live gameplay input handling unnecessarily.


FORWARD STEP API

Provide a reusable forward execution operation conceptually equivalent to:

StepForward()

Exact naming may follow current conventions.

One successful forward step must:

1. consume exactly the next recorded frame;
2. preserve its restart bit;
3. apply existing accepted playback input gates;
4. call production TempestGame.RunOneStep exactly once;
5. advance the cursor exactly once;
6. advance the boundary exactly once;
7. expose the resulting boundary state.

It must not:

- skip frames;
- execute more than one simulation tick;
- render;
- initialize Raylib;
- sleep for real-time pacing;
- consume keyboard/debugger controls.


COMPLETION

At boundary DurationTicks:

- the recording is complete;
- StepForward must not consume nonexistent frames;
- the session must expose completion clearly;
- no automatic live input exists inside the reusable session.

The existing visible application may retain its current live-input handoff around
the session later.

Design the shared deterministic recording session independently from live input.


RUN TO TICK / BOUNDARY

Provide a reusable forward-only operation conceptually equivalent to:

RunToBoundary(N)

or:

RunToTick(N)

Choose terminology carefully and document whether the value represents a state
boundary.

Preferred:

RunToBoundary(N)

because the analysis defines boundary 0 before frame 0.

It should:

- accept N from 0 through DurationTicks;
- advance using the same StepForward operation;
- stop exactly at boundary N;
- reject invalid negative/out-of-range values;
- never clamp silently;
- never skip simulation ticks;
- never use a larger delta.

If target N equals current boundary, do nothing.

Phase 1 does not need backward RunToBoundary behavior.

If N is earlier than the current boundary, reject it clearly for now.

Backward reconstruction comes later.


IMMUTABLE GAMEPLAY STATE SUMMARY

Introduce a bounded read-only diagnostic summary.

Suggested name:

GameplayStateSummary

The summary represents state at an exact simulation boundary.

It must:

- copy values from current game state;
- not expose mutable game entities;
- use stable deterministic collection ordering;
- identify the simulation boundary;
- identify recording cursor/remaining frames;
- be usable by automated tests;
- be suitable for later structured/headless output.

Do not expose every private field indiscriminately.


PHASE-1 STATE SUMMARY SCOPE

Keep the first summary useful but bounded.

At minimum include where current architecture supports it cleanly:

PLAYBACK:

- current boundary;
- recording duration;
- next recording frame/cursor;
- remaining recorded frames;
- recording-complete status.

STARTUP:

- recorded displayed starting level;
- recorded starting lives;
- recorded rim-speed multiplier;
- recorded seed.

CURRENT GAME:

- displayed/current level;
- LevelState;
- score;
- remaining lives;
- player lane/fractional position if cleanly exposed;
- player/game depth or equivalent positional state if appropriate;
- Super Zapper charges;
- active player-shot count;
- active enemy-bullet count;
- active Egg count;
- active Flipper count;
- active Tanker count;
- active Spiker count;
- active Spike count;
- Fuseball count;
- Pulsar count.

For FLIPPERS, include a bounded typed subrecord if cleanly possible:

- stable identity where available;
- lane;
- height;
- movement state;
- direction;
- pivot source;
- pivot destination;
- pivot progress/angle;
- cargo/original identity information where already accessible.

The recently diagnosed Level 5 invalid-direction defect demonstrates why
Flipper direction is especially valuable diagnostically.

Do not make mutable fields public merely to satisfy the summary.

If some desired state is not cleanly exposed, document it as a later summary
extension rather than introducing broad production visibility changes.


SUMMARY ORDERING

Collections must use stable ordering suitable for comparing boundaries.

Prefer:

- stable identity;
- ledger identity;
- deterministic lane/entity key;

as appropriate to actual objects.

Do not rely on incidental mutable collection order if it can vary.


NO STATE HASH YET

Do not implement deterministic signatures/hashes in this milestone.

State summaries should remain human/test inspectable.

Hashes may come later.


NO SNAPSHOT SUPPORT

Do not capture or restore arbitrary simulation state.

Do not serialize System.Random state.

Do not add snapshots.

Backward navigation later begins with replay-from-start.


NO BACKWARD NAVIGATION

Do not implement:

- StepBackward;
- rewind;
- backward RunToBoundary;
- seeking to prior state.

Only ensure the Phase 1 API does not make those unnecessarily difficult later.


NO VISIBLE DEBUGGER CONTROLS YET

Do not implement:

- Space pause/resume;
- Left Arrow;
- Right Arrow;
- speed controls;
- fast-forward controls;
- overlays.

Those come after the shared session proves correct.


NO HEADLESS CLI YET

Do not add a new command-line option in Phase 1.

The session must be Raylib-independent so the next milestone can create a thin
headless runner around it.

Automated tests may use the session directly.


AUDIO

The reusable deterministic session should support headless operation using the
existing SilentAudioEvents behavior.

Do not initialize Raylib audio in the session.

Do not redesign audio.

Visible playback can continue using its existing production audio composition
until the visible loop is migrated to the shared session in a later milestone.


PROGRAM.CS

Avoid a broad Program.cs rewrite in Phase 1.

The goal is first to establish and test the reusable playback session.

If extremely small shared helpers must be extracted from Program.cs to preserve
identical gating behavior, do so narrowly.

Do not migrate all visible playback to the session yet unless current source
proves that migration is necessary to validate the session.

The later visible-integration milestone will deliberately route normal visible
file playback through this shared session.


TESTS — BOUNDARY SEMANTICS

Add focused automated coverage proving:

1. Restored startup is boundary 0.

2. No recorded frame has been consumed at boundary 0.

3. One StepForward consumes frame 0 exactly once.

4. One StepForward calls production simulation exactly once.

5. The result is boundary 1.

6. N steps produce boundary N.

7. Duration D produces boundary D.

8. No frame is consumed beyond D.

9. Completion is reported at D.

10. Invalid forward targets are rejected.

11. RunToBoundary(current) is a no-op.

12. Backward targets are explicitly unsupported/rejected in Phase 1.


TESTS — RECORDING SEMANTICS

Protect:

- v1 startup semantics;
- v2 startup semantics;
- seed restoration;
- starting level;
- starting lives;
- rim speed;
- restart frames;
- full PlayerInput fields;
- production progression;
- recording completion.


TESTS — DETERMINISM

For the same recording and build:

- two fresh sessions must produce equivalent selected state summaries at the
  same boundaries;
- StepForward N times must agree with RunToBoundary(N);
- existing accepted replay behavior/state comparisons remain valid.

Use meaningful state fields rather than claiming complete deterministic identity
from the Phase-1 summary.


TESTS — STATE SUMMARY

Prove:

- summary boundary matches session boundary;
- recording cursor and remaining count are correct;
- summary is immutable/read-only;
- stable entity ordering is deterministic;
- selected gameplay fields reflect actual TempestGame state;
- Flipper direction/state information is correct in a controlled test;
- summary creation does not advance simulation;
- repeated summary reads at one boundary return equivalent content.


REGRESSION PROTECTION

Run neighboring tests covering:

- GameplayRecordingTests;
- GameplayPlaybackTests;
- direct-level startup;
- v1/v2 recording compatibility;
- Level 5;
- Level 7;
- death/re-entry;
- progression;
- campaign soak;
- Super Zapper;
- Flipper movement;
- Tanker cargo;
- input gating;
- recording live-input handoff behavior.

Do not change accepted gameplay.


DOCUMENTATION

Update durable documentation narrowly to record the Phase-1 implementation
candidate.

Document:

- shared deterministic playback session;
- boundary numbering;
- forward-step semantics;
- forward run-to-boundary;
- read-only state summary;
- Raylib-independent design;
- future visible controls:
  SPACE pause/resume;
  RIGHT ARROW step forward;
  LEFT ARROW step backward by deterministic reconstruction;
- future fast-forward/rewind;
- future headless CLI for VS Code Copilot;
- no backward navigation/snapshots/UI controls implemented yet.

Manual acceptance of visible behavior is NOT required for this Phase 1 because
visible playback is not being migrated yet.

Do not claim the future controls exist.


AI LOGS

Preserve complete task as:

AI-Logs/Prompts/2026-11-06-shared-playback-session-phase1-prompt.md

Create:

AI-Logs/Reports/2026-11-06-shared-playback-session-phase1-report.md

Report must include:

1. Starting Git state.
2. Existing playback orchestration.
3. Shared-session design.
4. Boundary semantics.
5. Input-gating ownership.
6. StepForward behavior.
7. RunToBoundary behavior.
8. Completion semantics.
9. State-summary schema.
10. Stable ordering.
11. Flipper diagnostic fields.
12. Raylib independence.
13. Audio behavior.
14. v1/v2 preservation.
15. Visible behavior preservation.
16. Production files changed.
17. Test files changed.
18. Documentation changed.
19. Focused test counts.
20. Neighboring regression counts.
21. Full-suite counts.
22. Build warnings/errors.
23. git diff --check result.
24. Future Phase 2/3 roadmap.
25. Final Git state.
26. Confirmation no backward stepping, snapshots, visible debugger controls,
    headless CLI, staging, commit, push, Level 8/9 or unrelated gameplay change
    occurred.


AUTOMATED VALIDATION

Run focused shared-session/state-summary tests.

Run neighboring playback/gameplay regressions.

Then run:

dotnet test TempestCSharp.slnx

dotnet build TempestCSharp.slnx

git diff --check

Require:

- tests actually execute;
- 0 failed;
- 0 unexpected skipped;
- build 0 errors;
- warning count reported accurately;
- git diff --check passes.


NO INTERACTIVE LAUNCH

Do not launch the game.

This phase introduces no visual controls requiring Neal's manual acceptance.

Do not stage.
Do not commit.
Do not push.


FINAL DIFF REVIEW

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

HEAD must remain:

7032a7219cde69a0ae25b00e5f3bc9ac1ee3ed04

Verify no:

- gameplay change;
- recording-format change;
- visible-control implementation;
- backward stepping;
- rewind;
- snapshot;
- headless CLI;
- Level 8/9 work;
- unrelated rendering/audio/input change;
- staging;
- commit;
- push.


FINAL RESPONSE

Return:

1. Shared-session type/API.
2. Boundary semantics.
3. StepForward behavior.
4. RunToBoundary behavior.
5. Completion semantics.
6. Input-gating ownership.
7. State-summary schema.
8. Flipper diagnostic fields.
9. Stable-order policy.
10. Raylib independence.
11. Audio handling.
12. v1/v2 compatibility.
13. Existing visible playback preservation.
14. Production files changed.
15. Tests changed.
16. Documentation changed.
17. Focused test counts.
18. Neighboring regression counts.
19. Full-suite counts.
20. Build warnings/errors.
21. git diff --check result.
22. Recommended next implementation milestone.
23. Report filename.
24. Final Git status.
25. Confirmation no backward step, snapshot, visible debugger controls, headless
    CLI, stage, commit, push, Level 8/9 or unrelated gameplay work occurred.

Stop for Neal's review before proceeding.

TEMPEST_SHARED_PLAYBACK_SESSION_PHASE1_COMPLETE