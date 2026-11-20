IMPORTANT COMPLETENESS CHECK

Read this entire user-provided task before taking any action.

Before using tools, reading project files, running commands, modifying files, or performing repository actions, locate the unique completion token near the end of this task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

Automatically appended VS Code/agent context after the user's token does not invalidate the token.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project or repository action.


AUTOMATIC / ACTIVE-EDITOR CONTEXT IS NOT AUTHORIZATION

Automatically surfaced active/recent/editor files are not authorized merely because VS Code supplied them.

Do not use such files unless this task expressly authorizes them or they are directly necessary to this bounded implementation.

DO NOT read TEMP/myNotes.md.


TASK

Implement the bounded live gameplay pause plus screenshot feature established by:

AI-Logs/Reports/2026-11-14-live-pause-screenshot-feasibility-report.md

Implement exactly this required user behavior:

1. Press F9 during normal live gameplay to pause.
2. Simulation freezes completely at the exact current game state.
3. Rendering continues normally so the frozen game remains visible.
4. While paused, press F10 to request a screenshot.
5. Save the complete currently rendered 1280 x 960 host window as PNG.
6. Allow repeated F10 presses while remaining paused, producing distinct screenshots.
7. Press F9 again to resume from exactly the paused simulation state, with no extra simulation steps, accumulated-time catch-up, or replayed buffered input.

This is a development/observation feature.

Do not add a pause menu.
Do not add a pause overlay.
Do not add final HUD work.
Do not modify gameplay mechanics.
Do not modify presentation calibration.
Do not stage, commit, or push.


CURRENT AUTHORITATIVE STATE

Baseline:

branch:
master

HEAD:
385f603a94391756b2ca6cccd524dc17b281e782

commit:
Close out ROM-backed enemy-shot rendering

origin/master should match HEAD.

The current portrait/perspective candidate remains intentionally modified and unstaged.

Before implementation run bounded checks of:

- branch;
- HEAD;
- origin/master;
- index;
- tracked worktree;
- non-ignored untracked files;
- diff summary.

Read:

AI-Logs/Reports/2026-11-14-live-pause-screenshot-feasibility-report.md

Use that report as authority for the existing loop, control, screenshot, path, recording, playback, and audio architecture.

Also use actual current Git state.

Expected:

- clean index;
- current portrait/perspective candidate only;
- no unrelated user work.

If unrelated unexpected work exists, STOP.

Do not restore/reset/clean/discard existing candidate work.


PRESERVE CURRENT PRESENTATION

Do not change:

- 1280 x 960 host;
- 720 x 960 portrait viewport;
- accepted blue viewport outline;
- portrait logical dimensions;
- accepted shared vertical placement -33.1;
- playfield scale 1.05;
- endpoint-normalized portrait perspective;
- --portrait-perspective diagnostic behavior;
- current perspective default;
- HUD/layout;
- legacy presentation;
- viewport/scissor behavior.

A screenshot must capture the presentation exactly as currently rendered.


LIVE PAUSE OWNERSHIP

Implement live pause as process/control state in the live Program loop.

Do NOT add pause state to:

- TempestGame;
- production gameplay state;
- PlayerInput;
- GameplayRecordingFile;
- recording schema;
- VisiblePlaybackController;
- GameplayPlaybackSession.

Keep live pause distinct from visible-playback pause.


F9 LIVE PAUSE CONTROL

Use F9 as a discrete press-edge live pause toggle.

F9 applies to normal live gameplay.

When F9 changes live gameplay from running to paused:

- do not call TempestGame.RunOneStep on paused loops;
- do not capture a gameplay-recording tick;
- do not increment simulationTick;
- do not forward normal gameplay input into simulation;
- continue rendering each application loop;
- continue processing application quit;
- continue processing F9;
- continue processing F10 screenshot requests.

When F9 resumes:

- resume with exactly the next normal simulation step;
- there is no catch-up;
- there is no simulated representation of time spent paused;
- no buffered gameplay inputs from paused loops are replayed.

Do not introduce a fixed-step accumulator.

Do not introduce wall-clock rebasing logic unless current source has materially changed from the feasibility report.


PAUSED GAMEPLAY INPUT

While live-paused:

- do not forward movement to gameplay;
- do not forward fire;
- do not forward Super Zapper;
- do not forward other gameplay actions;
- do not advance input-driven simulation state.

The feasibility report established that level-based held controls may naturally be observed on the first resumed simulation step if the user is still physically holding them. Preserve that simple current-input behavior.

Do not add release-to-rearm state.

Mouse/spinner delta must not accumulate during pause and then apply on resume.

Poll/consume/discard mouse delta as required by the actual current InputSystem architecture.

Do not buffer it.


F10 SCREENSHOT CONTROL

Use F10 as a discrete press-edge screenshot request.

At minimum, F10 must work while normal live gameplay is paused.

Implement it through the existing completed-frame screenshot path identified by the feasibility report.

Do NOT call Raylib.TakeScreenshot directly at F10 key-poll time if that would capture before the current frame is completely composed.

Instead:

- queue/request screenshot during process-level input handling;
- execute capture at the existing VectorRenderer completed-back-buffer callback before EndDrawing;
- capture the frame rendered on that loop.

The screenshot must not advance simulation.


SCREENSHOT CONTENTS

Capture the complete 1280 x 960 host frame as displayed.

The PNG should include:

- portrait viewport;
- host margins;
- gameplay vectors;
- HUD and normal visible overlays;
- accepted thin blue viewport border;
- current perspective;
- current frozen game state.

Do not crop to 720 x 960.

Do not manually reconstruct image pixels.

Use the locally confirmed Raylib-cs:

Raylib.TakeScreenshot(string fileName)

through the existing screenshot helper/callback architecture.


MULTIPLE SCREENSHOTS

Repeated F10 presses during one paused simulation tick must produce distinct files.

Do not overwrite the first screenshot.

Reuse/generalize the current atomic filename reservation pattern already present in PlaybackScreenshotCapture.

Preserve:

- FileMode.CreateNew-style collision avoidance;
- sequence fallback;
- cleanup of failed/zero-byte reservation where existing helper supports it;
- non-crashing failure reporting.

Do not create retry loops.


OUTPUT DIRECTORY

Use the already established:

UserData\Screenshots

through ApplicationPaths.

Do not create a new screenshot root.

Do not modify .gitignore.

Do not write into:

- Reference;
- Videos;
- TEMP;
- source directories;
- AI-Logs.


LIVE SCREENSHOT FILENAMES

Use a clear automatic live-game filename.

Preferred form:

live-level-{level}-tick-{tick:D6}-{sequence:D3}.png

or the smallest equivalent form supported cleanly by the existing capture helper.

Requirements:

- identify live capture;
- include current level where readily available;
- include current simulation tick;
- include collision-safe sequence;
- repeated screenshots at the same paused tick remain distinct.

Do not add unnecessary metadata.


SCREENSHOT REQUEST LIFETIME

A screenshot request should be consumed exactly once by the completed-frame capture callback.

Do not capture every subsequent frame because one F10 press left a Boolean set.

Multiple F10 presses must generate multiple explicit requests/captures.

If the current loop architecture can only observe one key-edge per render loop, one screenshot per F10 press is sufficient.


SCREENSHOT OUTSIDE LIVE PAUSE

The required feature is F10 while live-paused.

If the existing generalized screenshot request path naturally allows F10 capture during normal running live gameplay without additional architecture, it is acceptable for F10 to capture the current completed frame while running as well.

However:

- do not broaden the milestone merely to support running-game screenshots;
- do not change simulation timing to enable them;
- do not interfere with the required paused-capture behavior.

Document the actual implemented scope.


PLAYBACK PRESERVATION

Existing visible playback pause/step/reverse controls remain authoritative and unchanged.

Do not repurpose F9 as the playback pause key if that conflicts with the existing Space-based controls.

Do not merge live pause and playback pause state.

Preserve existing paused --playback-file screenshot behavior and existing Ctrl+Shift+S semantics unless the smallest generalized helper safely shares only the file-generation/capture mechanics.

If F10 support can naturally work in visible playback through the same completed-frame request path with no playback state changes, that is acceptable but not required.

Do not redesign playback.


RECORDING BEHAVIOR

When live gameplay recording is active and F9 pauses:

- stop simulation stepping;
- stop recording capture;
- stop simulationTick advancement.

Pause duration is not represented in the recording.

F9 and F10 are not gameplay inputs and must not be written to PlayerInput.

Resume recording on exactly the next real simulation tick.

Do not change:

- GameplayRecordingFile schema;
- version;
- metadata;
- deterministic input format.

A recorded session should reproduce the gameplay simulation independently of how long Neal paused live execution or how many screenshots were captured.


DETERMINISM / RNG

Pause and screenshot must not consume gameplay RNG.

For equal initial state and equal per-tick gameplay inputs:

- no pause;
- long pause;
- multiple screenshots while paused;

must result in equivalent simulation state after the same number of simulation ticks.

Do not add pause duration to simulation state.


AUDIO

Do not redesign AudioSystem.

While paused:

- no RunOneStep means no new gameplay-generated audio events;
- already-playing one-shot sounds may finish naturally.

Do not attempt to suspend/resume Raylib sound voices.

Do not add audio state solely for pause.


QUIT RESPONSIVENESS

Application quit behavior must remain responsive while paused.

Preserve the current window-close and Enter quit behavior as applicable.

Pause must not trap the user in a nested loop.


NO PAUSE OVERLAY

Do not draw:

- PAUSED text;
- translucent overlays;
- menu;
- screenshot indicator;
- timestamp;
- filename;
- border changes.

Neal wants the frozen game presentation clean for screenshots.

Any screenshot success/failure feedback should use the existing terminal/logging pattern and must not be rendered into the screenshot.


ERROR HANDLING

Raylib.TakeScreenshot returns void.

Retain/generalize the existing helper's explicit failure handling:

- filesystem/path failures must not crash the game when reasonably catchable;
- report failure;
- remove zero-byte reservation when possible;
- no retry loop;
- no false success if output is missing or empty.

Do not fabricate a Raylib success return.


KEY CONFLICT PROTECTION

Use:

F9 = live Pause
F10 = Screenshot

Do not change meanings of:

- P;
- Space;
- arrows;
- Enter;
- fire controls;
- existing playback controls;
- F12 diagnostic;
- Ctrl+Shift+S paused-file-playback screenshot.

Do not assign additional alternate keys in this milestone.


IMPLEMENTATION SCOPE

Expected production scope from the feasibility report:

- src/Tempest.Game/Program.cs
- src/Tempest.Game/Systems/InputSystem.cs
- src/Tempest.Game/Debug/PlaybackScreenshotCapture.cs
  or one narrowly generalized screenshot helper replacing/generalizing only its capture mechanics

Modify additional source files only if directly necessary for clean dependency passing/testing.

Do not modify TempestGame merely to store pause state.

Do not alter presentation/projection source.

Do not alter recording schema.

Do not modify .gitignore.


TEST REQUIREMENTS

Add focused automated tests where practical for:

1. F9 toggles live pause.
2. One or many paused application loops do not call/produce simulation advancement.
3. simulationTick does not advance while paused.
4. live recording capture does not advance while paused.
5. resume performs exactly the next ordinary tick.
6. pause duration has no effect on resulting simulation state.
7. paused gameplay input is not forwarded.
8. paused mouse/spinner delta is discarded rather than replayed.
9. quit remains processable while paused where architecture is testable.
10. F10 queues one screenshot request.
11. screenshot capture is deferred to completed-frame callback.
12. screenshot request is consumed once.
13. repeated paused captures create distinct filenames at the same simulation tick.
14. filenames use UserData\Screenshots and safe atomic reservation.
15. screenshot failure does not crash and does not claim false success.
16. screenshot request does not advance simulation or RNG.
17. recording schema/version remains unchanged.
18. existing playback pause/step/reverse behavior remains unchanged.
19. existing paused playback screenshot behavior remains unchanged.
20. existing portrait/legacy presentation tests remain unchanged.

Prefer small focused helpers if needed for unit testing.

Do not introduce a large new framework merely to test Program loop structure.

If some loop behavior is best proven by source structure plus smaller isolated tests, document that boundary honestly.


BOUNDED VALIDATION

Run focused tests covering:

- live pause state/control;
- InputSystem changes;
- screenshot helper/capture;
- gameplay recording;
- visible playback;
- existing playback screenshot behavior;
- directly affected Program/debug behavior.

Use VSTest hang timeout no greater than 3 minutes.

If focused validation passes:

1. run the complete test suite with 3-minute VSTest hang timeout;
2. run Release build;
3. run git diff --check.

Do not repeatedly rerun unchanged failing commands.

Do not use stale binaries after a failed build.

Follow established security/stall self-stop policy.


MANUAL HANDOFF

Do not launch the game.

Provide Neal one standard live command, preferably using his current presentation calibration example:

dotnet run --project .\src\Tempest.Game -- --level 9 --portrait-perspective 125

Manual acceptance:

1. Begin ordinary Level 9 gameplay.
2. Press F9.
3. Confirm simulation/gameplay freezes immediately while the frozen frame remains rendered.
4. Wait several seconds.
5. Press F10 once.
6. Press F10 again without resuming.
7. Confirm two distinct PNGs exist under UserData\Screenshots.
8. Confirm screenshots show the complete 1280 x 960 host frame, including margins, HUD, gameplay, current perspective, and blue portrait border.
9. Confirm no PAUSED/screenshot overlay contaminated either image.
10. Press F9 again.
11. Confirm play continues normally from the paused state without fast-forward/catch-up.
12. Confirm normal gameplay controls behave normally after resume.
13. If recording is available in the test session, verify the resulting playback contains the same gameplay progression without representing the wall-clock pause interval.

Do not claim manual acceptance before Neal performs it.


AI LOGGING

Preserve this prompt under AI-Logs/Prompts.

Create:

AI-Logs/Reports/2026-11-14-live-pause-screenshot-implementation-report.md

Include:

- starting Git state;
- source/reports inspected;
- files changed;
- live-pause state ownership;
- F9 implementation;
- step/record/tick gating;
- gameplay-input behavior;
- mouse/spinner-delta behavior;
- screenshot request flow;
- completed-frame callback behavior;
- output directory;
- filename scheme;
- multiple-capture behavior;
- failure handling;
- playback preservation;
- recording preservation;
- determinism/RNG result;
- audio behavior;
- focused tests;
- full-suite result;
- Release build;
- git diff --check;
- manual command;
- exact manual checklist;
- final Git state;
- confirmation nothing staged/committed/pushed;
- confirmation no prohibited visual/reference material was inspected.


NO DURABLE DOCUMENTATION UPDATE YET

Do not update:

TEMPEST-DEVELOPMENT-NOTES.md

or:

TEMPEST-BEHAVIOR-SPEC.md

Manual acceptance is still pending.

If accepted, durable development documentation can be updated during closeout.


NO COMMIT OR PUSH

Do not stage.
Do not commit.
Do not push.


FINAL RESPONSE

Report:

1. Whether implementation completed.
2. Starting/final Git state.
3. Files changed.
4. F9 pause behavior.
5. F10 screenshot behavior.
6. Pause-state ownership.
7. Simulation-step gating.
8. simulationTick behavior.
9. Recording behavior.
10. Input behavior while paused.
11. Mouse/spinner-delta behavior.
12. Resume/catch-up behavior.
13. Screenshot capture point.
14. Screenshot contents.
15. Output directory.
16. Filename format.
17. Multiple-screenshot behavior.
18. Failure handling.
19. Playback preservation.
20. Audio behavior.
21. Determinism/RNG evidence.
22. Focused tests.
23. Full suite.
24. Release build.
25. git diff --check.
26. Exact manual command.
27. Manual checklist.
28. Report path.
29. Final Git state.
30. Confirmation nothing staged, committed, or pushed.
31. Confirmation no prohibited visual/reference material was inspected.

Stop after automated validation and hand the feature to Neal for manual acceptance.

TEMPEST_LIVE_PAUSE_SCREENSHOT_IMPLEMENTATION_20261114