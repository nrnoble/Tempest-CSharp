FAIL-CLOSED COMPLETENESS CHECK

Read this entire user-provided task before using tools, reading files, running
commands, modifying files, staging, committing, pushing, or taking any
repository action.

Before doing anything else, locate the unique completion token near the end.

Require exactly one complete standalone token beginning with "TEMPEST_" on a
line by itself.

VS Code/agent-generated reminders, attachment metadata, tagged-file context, or
other system-generated context after the user's token do not invalidate it.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before
any repository action and explain why.


TEMPEST VISIBLE RECORDED-PLAYBACK CONTROLS — PHASE 2

Implement Phase 2 of the accepted recorded-playback debugger roadmap.

PHASE 2 PURPOSE

1. Route visible recorded-file playback through the accepted shared
   GameplayPlaybackSession.

2. Preserve existing visible playback fidelity and live-input handoff.

3. Add visual playback-observation controls:

   SPACE:
   toggle pause/resume while recorded playback is active.

   RIGHT ARROW:
   while recorded playback is paused, advance exactly one recorded simulation
   tick.

4. Stop for Neal's manual visual acceptance.

This playback is observation of gameplay that already happened.

Debugger controls must not become gameplay input, alter the recording, or be
written into any recording.

Do not implement Left Arrow/backward stepping yet.

Do not implement rewind.

Do not implement fast forward or speed controls.

Do not implement selected-tick seeking.

Do not add snapshots.

Do not add the headless runner or CLI yet.

Do not begin Level 8 or Level 9 work.


ACTIVE REPOSITORY

Work only in:

C:\Users\nrnob\source\repos\Tempest


EXPECTED CLEAN BASELINE

Expected branch:

master

Expected HEAD and origin/master:

1fd28c1b9a7eaad0a0a1ed35c781f57a8b3d90af
Add shared deterministic playback session

Require:

- clean tracked worktree;
- clean index;
- master tracking origin/master;
- local master matching origin/master;
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

Read:

AI-Logs/Reports/2026-11-06-recorded-playback-debugger-headless-analysis-report.md

AI-Logs/Reports/2026-11-06-shared-playback-session-phase1-report.md

AI-Logs/Reports/2026-11-07-shared-playback-session-phase1-closeout-report.md

Inspect current source and tests, including:

src/Tempest.Game/Program.cs

src/Tempest.Game/Debug/GameplayPlaybackSession.cs

src/Tempest.Game/Debug/GameplayStateSummary.cs

src/Tempest.Game/Debug/GameplayPlayback.cs

src/Tempest.Game/Debug/GameplayRecordingFile.cs

src/Tempest.Game/Debug/GameplayStartupConfiguration.cs

src/Tempest.Game/Systems/PlayerInputReplay.cs

src/Tempest.Game/Systems/InputSystem.cs

src/Tempest.Game/TempestGame.cs

tests/Tempest.Game.Tests/GameplayPlaybackSessionTests.cs

tests/Tempest.Game.Tests/GameplayPlaybackTests.cs

tests/Tempest.Game.Tests/GameplayRecordingTests.cs

tests/Tempest.Game.Tests/GameplayInputTests.cs

tests/Tempest.Game.Tests/InputSystemTests.cs

and relevant direct-level, progression, recording, death/re-entry, Game Over,
transition, and rendering tests.

Inspect current source as authority rather than relying only on reports.


ACCEPTED PHASE 1 FOUNDATION

Preserve:

- GameplayPlaybackSession;
- boundary-zero startup contract;
- one frame consumed per successful StepForward;
- one production RunOneStep call per consumed frame;
- RunToBoundary semantics;
- completion semantics;
- centralized recorded-playback input gates;
- immutable GameplayStateSummary;
- stable Flipper summary ordering;
- Raylib-independent session;
- version 1 recording compatibility;
- version 2 recording compatibility;
- strict recording validation;
- existing recording format;
- existing production simulation and update ordering.

Do not redesign Phase 1 unless current source reveals a concrete defect.


USER'S VISUAL PLAYBACK GOAL

Recorded playback is a tool for analyzing gameplay that already occurred.

It should allow Neal to pause an exact deterministic recording and inspect
difficult gameplay one simulation tick at a time.

This is particularly intended for examining behavior such as:

- Flipper lane-change patterns;
- movement-direction reversals;
- Tanker cargo;
- Spike interactions;
- collision timing;
- later high-speed gameplay that is difficult to study while actively playing.

The controls do not interact with or change the recorded gameplay.

They only control whether and when the next recorded simulation frame is
executed.


VISIBLE PLAYBACK MIGRATION

Migrate visible FILE playback selected by:

--playback-file <file>

to use GameplayPlaybackSession for recorded-frame execution.

The visible loop should no longer independently duplicate the session's:

- recording cursor;
- simulation boundary;
- recorded-frame retrieval;
- playback input gating;
- recorded RunOneStep execution;
- completion calculation.

GameplayPlaybackSession must be the authoritative recorded execution path.

Program.cs should continue to own:

- Raylib window lifecycle;
- rendering;
- audio composition;
- real-time outer loop;
- input polling for playback controls;
- cursor/window behavior;
- completion message;
- transition to live input after recorded playback completes;
- normal application quit behavior.

Do not create another visible playback engine.


BUILT-IN PLAYBACK PROFILES

Inspect whether the existing built-in deterministic playback modes can safely
use the same session:

--gameplay-playback

--gameplay-playback-level4

If Phase 1's session already supports their replay/startup model cleanly, route
them through the same session to avoid duplicate visible playback execution.

If their sparse/profile semantics require a materially different contract, do
not force them into an unsafe abstraction. Document the distinction and keep
this milestone focused on --playback-file.

Do not change their accepted gameplay behavior or manual acceptance.


SPACE PAUSE/RESUME

While recorded playback is active:

- Space toggles paused/running state.
- Use press-edge behavior, not held-level toggling.
- One Space press changes state once.
- Holding Space must not repeatedly toggle.
- Pausing must not consume a recorded frame.
- Pausing must not call RunOneStep.
- Pausing must not advance the session boundary or cursor.
- Resuming continues from the exact same boundary.
- Space must not become PlayerInput.
- Space must not be recorded.
- Space must not activate Super Zapper during recorded playback control.

This is an explicit mode-dependent control interpretation.

Outside active recorded playback-debugger mode, preserve existing Space behavior,
including normal Super Zapper input.

Do not globally remove or remap Space.


PAUSED WINDOW BEHAVIOR

While paused:

- continue the Raylib outer loop;
- keep the window responsive;
- continue polling debugger and quit controls;
- redraw the same current game state;
- freeze TempestGame simulation;
- freeze playback cursor;
- freeze simulation boundary;
- freeze all RunOneStep-driven presentation state;
- emit no new simulation audio events;
- do not collect ignored gameplay input for later simulation;
- do not hand paused keyboard/controller input to the next recorded tick.

Existing audio voices may finish naturally.

Do not redesign or forcibly stop the audio system merely because playback is
paused.


RIGHT ARROW FORWARD STEP

While recorded playback is active AND paused:

One press-edge of Right Arrow must:

1. consume exactly the next recorded frame;
2. apply the accepted recorded-playback input gates;
3. preserve that frame's restart bit;
4. call TempestGame.RunOneStep exactly once through GameplayPlaybackSession;
5. advance the recording cursor exactly once;
6. advance the simulation boundary exactly once;
7. redraw the resulting state.

Holding Right Arrow must not unintentionally execute many ticks unless the
existing press-edge API itself produces deliberate OS repeats and the
implementation explicitly prevents those repeats.

Prefer one physical press producing one simulation tick.

Right Arrow while playback is running should not change playback speed and
should not add another simulation step.

Right Arrow must not become recorded PlayerInput while playback control is
active.


LEFT ARROW

Do not implement backward stepping in Phase 2.

While recorded playback is active:

- Left Arrow must not seek backward;
- Left Arrow must not affect recorded PlayerInput;
- Left Arrow must not move the Blaster during deterministic recorded playback.

If a temporary no-op is needed to reserve the control, keep it silent and
document that backward stepping is Phase 3B.

Do not print repeated console noise for Left Arrow.


OTHER GAMEPLAY INPUT DURING PLAYBACK

Recorded gameplay must continue to come only from the recording until its final
boundary.

Live gameplay controls during active recorded playback must not alter the
recorded simulation.

This includes:

- spinner/mouse movement;
- keyboard Left/Right gameplay movement;
- Fire;
- Super Zapper;
- controller axis;
- restart.

Playback-control keys belong to the outer playback controller, not PlayerInput.

Preserve normal window-close and Enter immediate-exit behavior.


PAUSE STATE OWNERSHIP

Introduce one small maintainable visible-playback controller/state abstraction
if useful.

Possible names:

VisiblePlaybackController

PlaybackControlState

or another project-consistent name.

It may own:

- paused/running state;
- pause toggle;
- one-shot forward-step request;
- completion/control-mode state if necessary.

Do not put Raylib polling inside GameplayPlaybackSession.

GameplayPlaybackSession must remain reusable and Raylib-independent.

Program/InputSystem may sample Raylib controls and translate them into the
visible playback-controller commands.


INPUTSYSTEM SEAM

Prefer a narrow playback-debugger input seam.

For example, InputSystem may expose clear press-edge queries such as:

- pause/resume requested;
- forward-step requested.

Exact naming should follow project conventions.

Do not combine debugger commands into PlayerInput.

Do not serialize debugger commands.

Do not alter the recording file format.


COMPLETION WHILE RUNNING

Preserve existing accepted behavior:

- recording executes through DurationTicks;
- completion is reported once;
- live controls resume on the next normal simulation iteration;
- session cursor remains at duration;
- simulation can continue under live input after handoff.

Do not change the established console completion message unless necessary to
report pause/control state clearly and narrowly.


COMPLETION WHILE PAUSED OR STEPPING

Define and test final-boundary behavior.

If Right Arrow advances the paused session to the final boundary:

- render the final recorded state;
- report recording completion once;
- remain observably at the final recorded state for that rendered iteration;
- do not accidentally consume live gameplay input in the same step;
- do not consume a nonexistent recorded frame.

Choose one clear handoff behavior for the next user command:

Preferred:

- at the final recorded boundary, the playback session is complete;
- the next Space/resume action or next normal unpaused iteration transitions to
  the existing live-input path;
- Right Arrow at the final boundary does nothing.

If current architecture supports a simpler behavior while still preserving a
visible final boundary and no same-tick live-input contamination, document and
test it.

Do not silently lose the final recorded frame.


VISIBLE STATUS INDICATION

Add only the minimum visual indication necessary for Neal to know playback
control state.

At minimum, while file playback is paused, display a small development-only
overlay indicating:

PAUSED

and ideally:

current boundary / duration

Example concept only:

PAUSED  4820 / 7260

Use the established uppercase shared VectorFont because this is text drawn
inside the Tempest window.

Keep it unobtrusive and clearly development-only.

Do not cover important gameplay geometry.

Do not redesign the HUD.

Do not add a full playback UI, timeline, transport bar, mouse interface, or
debug dashboard.

If status can be communicated adequately through the window title or console
without a visual overlay, evaluate that, but Neal must be able to tell visually
that playback is paused while examining the game.

Prefer a small in-window vector overlay.


UPPERCASE PRESENTATION

Any in-window playback-control text must use uppercase lettering and the shared
VectorFont, consistent with the accepted Tempest presentation policy.

Do not add default/system-font text inside the gameplay presentation.


RENDERING

Rendering while paused should redraw the current Tempest state without
advancing simulation.

Do not create a screenshot cache unless current architecture requires it.

Do not modify gameplay geometry, colors, projection, or accepted rendering.

The same `TempestGame` state should render repeatedly while paused.


AUDIO

Preserve normal playback audio while running.

While paused:

- no new simulation audio events occur because RunOneStep is not called;
- already playing sound may finish naturally.

While stepping forward:

- normal audio events generated by that one simulation tick may play once.

Do not replay prior audio when redrawing the same paused boundary.

Do not suppress all audio permanently after resume.

Do not retune audio or volume.


LIVE-INPUT HANDOFF

The accepted recording/playback system resumes live controls after recorded
playback completes.

Preserve this.

After handoff:

- Space again has its ordinary gameplay meaning;
- Right/Left Arrow again have their ordinary live-development movement meaning;
- playback pause/step controls are no longer active;
- the visible controller must not continue intercepting normal gameplay input.

Explicitly test this mode transition.


RECORDING MODE

Do not alter:

--record-gameplay

Recording behavior, save behavior, input capture, startup metadata, and file
format must remain unchanged.

Playback controls are not stored in recordings.


RECORDING FORMAT

Do not change:

- format identifier;
- v1 schema;
- v2 schema;
- startup metadata;
- frame structure;
- strict validation;
- JSON behavior.

Playback control state is session/UI state, not recording identity.


HEADLESS SESSION PRESERVATION

GameplayPlaybackSession must remain usable without Raylib.

Do not add:

- Raylib key polling;
- rendering;
- window title logic;
- real-time pacing;
- visible-control state

to GameplayPlaybackSession.

The future headless runner must be able to use the same session unchanged.


NO BACKWARD RECONSTRUCTION

Do not implement:

- StepBackward;
- replay-from-start reconstruction;
- rewind;
- backward seek;
- snapshots;
- Random-state capture.

Those remain Phase 3B/later work.


NO FAST FORWARD

Do not add:

- playback speed multipliers;
- multiple ticks per displayed frame;
- held-arrow fast stepping;
- skipped rendering intervals;
- audio suppression modes.

Those remain later work.


NO HEADLESS CLI

Do not add a command-line runner or JSON state export in Phase 2.

That is Phase 3A.


NO GAMEPLAY CHANGES

Do not change:

- Levels 1–7;
- level geometry;
- enemies;
- Flipper behavior;
- direction/pivots;
- Tankers/cargo;
- Spikes;
- firing;
- Super Zapper gameplay;
- score;
- death/re-entry;
- progression;
- Game Over;
- Avoid Spikes;
- transition presentation;
- rim speed;
- direct-level startup;
- recording semantics;
- ordinary live controls outside playback mode.


TESTS — MIGRATED VISIBLE EXECUTION

Add focused tests proving:

1. Visible file playback uses GameplayPlaybackSession for recorded execution.

2. The session boundary and visible progression remain synchronized.

3. Existing normal visible playback produces the same selected production game
   state as before migration.

4. Completion is reported once.

5. Live-input handoff still occurs after completion.

6. No recorded frame is skipped or duplicated.


TESTS — PAUSE/RESUME

Prove:

1. Playback starts running by default.

2. Space press toggles to paused.

3. Held Space does not repeatedly toggle.

4. Paused redraw iterations do not call RunOneStep.

5. Paused redraw iterations do not consume frames.

6. Boundary, cursor and gameplay state remain unchanged while paused.

7. A later Space press resumes.

8. Resume continues with the exact next recorded frame.

9. Pause command never enters PlayerInput.

10. Space does not activate Super Zapper during playback-control mode.

11. Space returns to ordinary gameplay meaning after live-input handoff.


TESTS — FORWARD STEP

Prove:

1. Right Arrow while running does not add an extra step.

2. Right Arrow while paused consumes exactly one recorded frame.

3. Exactly one RunOneStep occurs.

4. Boundary advances by one.

5. Cursor advances by one.

6. Restart bit from that frame is preserved.

7. Recorded PlayerInput fields are preserved.

8. One press yields one step.

9. A paused step renders the resulting boundary.

10. Right Arrow at completion does nothing.

11. Right Arrow is not recorded as gameplay input.

12. Right Arrow returns to normal live-control meaning after playback handoff.


TESTS — LEFT ARROW RESERVATION

Prove that Left Arrow during active recorded playback does not:

- move the recorded Blaster;
- alter PlayerInput;
- alter boundary;
- consume a frame;
- attempt unsupported backward navigation.

Do not add backward behavior.


TESTS — FINAL BOUNDARY

Prove:

- final recorded frame is executed exactly once;
- final boundary can be rendered;
- completion is reported once;
- no nonexistent frame is consumed;
- no same-step live input contaminates the final recorded frame;
- the subsequent handoff is predictable and tested.


TESTS — PAUSED OVERLAY

If an in-window overlay is added, test:

- uppercase PAUSED text;
- shared VectorFont usage;
- deterministic placement;
- boundary/duration text if included;
- visibility only while playback is paused;
- hidden during running playback and live gameplay;
- no effect on simulation.

Avoid brittle native pixel tests.

Use the existing vector-command/geometry testing style.


REGRESSION TESTING

Protect:

- GameplayPlaybackSession Phase 1;
- GameplayStateSummary;
- v1/v2 recording compatibility;
- GameplayRecordingTests;
- existing GameplayPlaybackTests;
- direct-level startup;
- recording capture;
- completion/live handoff;
- input mappings;
- Space Super Zapper during normal live gameplay;
- Left/Right live movement;
- Enter quit;
- playback fidelity;
- death/re-entry;
- progression;
- campaign soak;
- Levels 5 and 7;
- Game Over and transitions where relevant.


DOCUMENTATION

Update durable documentation narrowly to record the Phase 2 implementation
candidate.

Document:

- visible file playback now uses GameplayPlaybackSession;
- Space pauses/resumes recorded playback;
- Right Arrow advances one simulation tick while paused;
- Left Arrow is reserved for later backward reconstruction and has no playback
  action yet;
- debugger controls are outside PlayerInput and recordings;
- pause freezes simulation/cursor/tick-based presentation;
- rendering remains responsive;
- live-input handoff remains;
- ordinary gameplay mappings return after handoff;
- no recording-format change;
- no backward step, rewind, fast forward, headless CLI, or snapshots yet;
- manual visual acceptance is pending.

Do not claim visual acceptance before Neal performs it.


AI LOGS

Preserve the complete task as:

AI-Logs/Prompts/2026-11-07-visible-playback-controls-phase2-prompt.md

Create:

AI-Logs/Reports/2026-11-07-visible-playback-controls-phase2-report.md

The report must include:

1. Starting Git state.
2. Existing visible playback path.
3. Migration to GameplayPlaybackSession.
4. Visible controller ownership.
5. Debugger-control/input separation.
6. Space pause/resume semantics.
7. Right Arrow forward-step semantics.
8. Left Arrow reserved/no-op semantics.
9. Paused rendering behavior.
10. Paused audio behavior.
11. Forward-step audio behavior.
12. Overlay/status presentation.
13. Final-boundary behavior.
14. Completion reporting.
15. Live-input handoff.
16. Ordinary gameplay control restoration.
17. Recording-format preservation.
18. Phase 1/headless preservation.
19. Production files changed.
20. Test files changed.
21. Documentation changed.
22. Focused test counts.
23. Neighboring regression counts.
24. Full-suite counts.
25. Build warnings/errors.
26. git diff --check result.
27. Manual launch commands.
28. Manual acceptance checklist.
29. Remaining Phase 3A/3B roadmap.
30. Final Git state.
31. Confirmation no backward stepping, rewind, fast forward, speed control,
    snapshots, headless CLI, recording change, stage, commit, push, Level 8/9,
    or unrelated gameplay work occurred.


AUTOMATED VALIDATION

Run focused tests for:

- visible playback migration;
- pause/resume controller;
- forward stepping;
- left-arrow reservation;
- final-boundary behavior;
- paused overlay;
- live-input handoff;
- ordinary input restoration.

Run neighboring regression covering:

- shared playback session;
- state summary;
- recording/playback v1/v2;
- input;
- Super Zapper;
- direct startup;
- death/re-entry;
- progression;
- campaign soak;
- Levels 5 and 7;
- relevant rendering/presentation tests.

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

Neal will perform manual visual acceptance after automated validation.

Do not stage.
Do not commit.
Do not push.


MANUAL ACCEPTANCE COMMAND

Use an existing recorded gameplay file, for example:

dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\testlevel5-3.json

Do not assume that exact file exists in every environment.

Provide the general form:

dotnet run --project src\Tempest.Game -- --playback-file <recording-file>


MANUAL ACCEPTANCE CHECKLIST

Ask Neal to verify:

1. Recorded playback begins and looks exactly like normal accepted playback.

2. Space pauses immediately.

3. The window remains responsive while paused.

4. The current gameplay image remains visible.

5. Game simulation visibly stops while paused.

6. Repeated time spent paused does not change gameplay state.

7. The PAUSED indicator is clear and unobtrusive.

8. One Right Arrow press advances exactly one small simulation step.

9. Repeated Right Arrow taps allow frame-by-frame study.

10. Holding or tapping Right Arrow does not unexpectedly run freely.

11. Left Arrow does not move the Blaster or alter playback.

12. Space resumes from the exact paused point.

13. Audio behavior is sensible when pausing, stepping, and resuming.

14. Playback completes normally.

15. Live controls resume normally after playback completion.

16. Space returns to Super Zapper behavior after live handoff.

17. Left/Right Arrow return to normal live movement after handoff.

18. Enter still exits immediately.

19. No playback fidelity regression, crash, skipped event, or duplicated event
    is observed.


FINAL GIT CHECK

Finish with:

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

1fd28c1b9a7eaad0a0a1ed35c781f57a8b3d90af

Expected:

- Phase 2 implementation candidate unstaged;
- no commit;
- no push;
- no backward stepping;
- no fast forward/rewind;
- no headless CLI;
- no Level 8/9 work.


FINAL RESPONSE

Return:

1. Visible playback migration result.
2. GameplayPlaybackSession ownership.
3. Pause/resume behavior.
4. Forward-step behavior.
5. Left Arrow behavior.
6. Debugger-control/PlayerInput separation.
7. Paused rendering behavior.
8. Audio behavior.
9. Overlay/status behavior.
10. Final-boundary behavior.
11. Completion reporting.
12. Live-input handoff.
13. Ordinary control restoration.
14. Recording-format preservation.
15. Phase 1/headless preservation.
16. Production files changed.
17. Tests changed.
18. Documentation changed.
19. Focused test counts.
20. Neighboring regression counts.
21. Full-suite counts.
22. Build warnings/errors.
23. git diff --check result.
24. Manual launch command.
25. Manual acceptance checklist.
26. Report filename.
27. Final Git status.
28. Confirmation no backward step, rewind, fast forward, speed control,
    snapshots, headless CLI, recording change, stage, commit, push, Level 8/9,
    or unrelated gameplay work occurred.

Stop for Neal's manual acceptance.

TEMPEST_VISIBLE_PLAYBACK_CONTROLS_PHASE2_COMPLETE