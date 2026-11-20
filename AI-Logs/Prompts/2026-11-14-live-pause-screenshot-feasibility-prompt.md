IMPORTANT COMPLETENESS CHECK

Read this entire user-provided task before taking any action.

Before using tools, reading project files, running commands, modifying files, or performing repository actions, locate the unique completion token near the end of this task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

Automatically appended VS Code/agent context after the user's token does not invalidate the token.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project or repository action.


AUTOMATIC / ACTIVE-EDITOR CONTEXT IS NOT AUTHORIZATION

Automatically surfaced active/recent/editor files are not authorized merely because VS Code supplied them.

Do not use such files unless this task explicitly authorizes them or they are directly necessary to the bounded investigation.

DO NOT read TEMP/myNotes.md.


TASK

Perform a tightly bounded READ-ONLY feasibility and architecture investigation for adding live gameplay pause plus screenshot capture to Tempest.

Do NOT implement anything.

The requested behavior is exactly:

1. Press one dedicated Pause key during live gameplay.
2. Simulation freezes completely at the current game state/frame.
3. Rendering continues normally so the frozen game remains visible.
4. While paused, press one dedicated Screenshot key.
5. Save the currently rendered complete host window as a PNG.
6. Allow additional screenshots while remaining paused.
7. Press Pause again and continue from exactly the same simulation state with no accumulated-time catch-up burst.

Determine the smallest maintainable implementation architecture that provides this behavior without destabilizing fixed-step simulation, deterministic recording/playback, reverse playback, input handling, audio, or current presentation work.


CURRENT REPOSITORY STATE

Baseline:

branch:
master

HEAD:
385f603a94391756b2ca6cccd524dc17b281e782

commit:
Close out ROM-backed enemy-shot rendering

origin/master should match HEAD.

The current portrait/perspective candidate remains intentionally modified and unstaged.

Before analysis inspect:

- branch;
- HEAD;
- origin/master;
- index;
- tracked worktree;
- non-ignored untracked files;
- diff summary.

Use the latest current candidate state rather than relying on an old modified-file count.

Expected:

- clean index;
- current candidate work only;
- no unrelated user work.

If unrelated unexpected work exists, STOP.

Do not restore/reset/clean/discard anything.


CURRENT ACCEPTED PRESENTATION CONTEXT

Preserve all current presentation work, including:

- 1280 x 960 host window;
- 720 x 960 portrait viewport;
- accepted thin blue portrait viewport border;
- manually accepted shared vertical placement -33.1;
- endpoint-normalized portrait-perspective implementation;
- permissive --portrait-perspective diagnostic;
- current HUD and viewport behavior;
- legacy presentation.

This investigation must not modify or recalibrate presentation.


PRIMARY LIVE-PAUSE QUESTION

Trace the actual top-level game loop and determine the smallest way to freeze simulation while continuing rendering.

Identify exactly:

- where wall-clock/frame delta is measured;
- where fixed-step accumulator state is stored;
- where simulation steps execute;
- where input is collected;
- where rendering occurs;
- where player input is translated into simulation input;
- whether audio events are dispatched during simulation or rendering;
- whether playback/debug controls are processed outside simulation;
- how application quit remains responsive.

Determine whether live pause should conceptually:

A. suppress addition of wall-clock delta to simulation accumulator;

B. clear/reset accumulator at pause/resume boundaries;

C. suppress RunOneStep while allowing render/control polling;

D. use another existing fixed-step-safe mechanism.

Do not guess.

The primary requirement is:

After remaining paused for an arbitrary human-visible duration, resuming must NOT execute accumulated simulation steps to catch up with elapsed real time.

The simulation must continue from exactly the paused state.


DO NOT USE A BLOCKING PAUSE LOOP UNLESS SOURCE REQUIRES IT

Investigate whether pause can remain inside the normal application/render loop.

Preferred conceptual behavior, if source supports it:

unpaused:
poll controls
advance fixed-step timing/simulation
render

paused:
poll only process-level pause/screenshot/quit controls
do not advance simulation time
render the exact frozen simulation state

Do not implement a nested blocking loop merely because it is a common pause example.

A blocking loop would need strong source-level justification.


EXISTING PLAYBACK PAUSE / STEP ARCHITECTURE

Inspect existing deterministic/visible gameplay playback controls.

Determine:

- whether playback already supports pause;
- where paused playback state is stored;
- how frame stepping works;
- how reverse stepping works;
- whether rendering while playback-paused already demonstrates a safe model for live pause;
- which control infrastructure can be reused;
- which state must remain separate so live pause does not accidentally become playback mode.

Do not redesign playback.

Do not merge conceptually different states merely to reuse a Boolean.


LIVE INPUT SEMANTICS WHILE PAUSED

Determine what ordinary gameplay input should do while paused.

Desired default:

- gameplay movement/fire/zap input must not advance or mutate simulation while paused;
- Pause control remains responsive;
- Screenshot control remains responsive;
- normal application quit remains responsive.

Determine whether holding gameplay controls during pause could become active immediately upon resume under the current input model.

Recommend the smallest predictable behavior.

Do not add a pause menu or UI.


RECORDING SEMANTICS

Investigate interaction with live gameplay recording.

The pause feature is intended primarily as a development/observation control, not gameplay behavior.

Determine whether:

- entering/exiting live pause should be excluded from recorded gameplay input;
- screenshot commands should be excluded from recording;
- simulation tick numbering should simply stop while paused;
- resuming should continue recording from the next simulation tick without representing wall-clock pause duration.

Prefer keeping recording schema/version unchanged if architecture permits.

Do not change recording format in this investigation.


PLAYBACK SEMANTICS

Investigate whether the same screenshot function can naturally work while:

- visible deterministic playback is running;
- visible playback is paused;
- --playback-file is running;
- reverse stepping is active.

Determine whether live Pause should:

- control normal gameplay only;
- reuse an already established playback pause control where appropriate;
- or be exposed as one process-level presentation pause that works consistently in both live and visible playback.

Do not assume the answer.

The user's required seven-step behavior applies at minimum to normal live gameplay.

Screenshot support during playback is desirable only if it falls naturally out of the architecture without broadening scope.


SCREENSHOT CAPTURE

Identify the Raylib-cs screenshot API actually available to this project and its exact current signature from installed/current project references or directly relevant local API usage.

Do not search the web unless local project/package evidence is insufficient.

Determine the correct point in the actual Tempest draw lifecycle for capture of the completed visible host frame.

The desired PNG should include what Neal actually sees in the game window, including as applicable:

- complete 1280 x 960 host window;
- host margins;
- portrait viewport;
- tube/gameplay presentation;
- HUD;
- accepted blue viewport border;
- normal overlays that are part of the frame.

Do not capture only the 720 x 960 viewport.

Do not build image pixels manually if Raylib's screenshot facility already captures the host window.

Determine whether screenshot capture should be requested by input and executed after the relevant rendering has completed, rather than immediately at key-poll time.

Do not implement yet.


SCREENSHOT WHILE PAUSED

Explicitly determine whether taking one or more screenshots can occur without:

- advancing simulation;
- changing RNG;
- changing gameplay input state;
- changing playback cursor;
- changing score/lives/state;
- leaving pause;
- accumulating elapsed simulation time.

Multiple screenshot presses while paused must produce multiple output files rather than overwriting the same filename.


FILENAME AND STORAGE POLICY

Inspect existing project conventions for user-generated runtime files.

Determine the safest location for screenshots.

Prefer:

- a local ignored directory already suited to user-generated/runtime artifacts;

or, if none exists:

- one narrowly selected ignored screenshots directory.

Do not save screenshots into tracked source or reference directories.

Do not edit .gitignore unless genuinely necessary.

Recommend an automatic unique filename scheme that:

- cannot silently overwrite an existing screenshot under normal use;
- requires no user typing during gameplay;
- remains understandable when viewed later.

Potential filename components may include only source-available information such as:

- date/time;
- level number;
- sequence counter.

Do not over-engineer metadata.

Determine whether timestamp resolution alone is sufficient for multiple rapid screenshots.

No implementation.


KEY-BINDING INVESTIGATION

Inventory current keyboard controls and development/debug controls sufficiently to choose non-conflicting candidate keys.

Do not change bindings.

Report:

- keys currently used by live gameplay;
- quit;
- recording/playback;
- playback pause/step/reverse controls;
- debug controls directly relevant to avoiding collisions.

Recommend:

- one Pause key;
- one Screenshot key.

Favor mnemonic and easily reachable controls that do not collide with existing behavior.

Do not assume standard Escape/P because those may already have meanings.


AUDIO WHILE PAUSED

Trace the existing audio architecture.

Determine whether the correct minimal semantics are:

- no new gameplay audio events because simulation is frozen;
- already-playing one-shot sounds are allowed to finish naturally;

or whether the current audio implementation supports/needs explicit pause/resume of active audio.

Do not add audio-engine complexity merely for conceptual purity.

Recommend the least intrusive behavior consistent with a frozen simulation.


FIXED-STEP / ACCUMULATOR PROOF

Provide a precise timing explanation for:

normal running
-> pause requested
-> several seconds/minutes paused
-> one or more screenshots
-> resume
-> next simulation tick

Demonstrate why there will be no catch-up burst.

Identify any accumulator state that must be:

- frozen;
- discarded;
- reset;
- rebased.

Also verify whether frame-time spikes immediately before or after pause need handling.

Do not modify source.


DETERMINISM

Determine whether pause/screenshot can remain completely outside simulation state.

A correct design should ideally mean:

same initial game state + same gameplay inputs
produces same simulation result
regardless of how long the user pauses for observation or how many screenshots are taken.

State whether that is achievable under the current architecture.

Determine whether RNG state remains untouched.

Do not add pause state to GameplayRecordingFile unless evidence proves it necessary.


SCREENSHOT PRESENTATION OVERLAY

Do not propose a large pause menu.

Determine whether any pause indicator is necessary.

Neal did not request one.

Default recommendation should be no gameplay-obscuring pause overlay unless current architecture makes it necessary to know pause state.

Similarly, do not draw "Screenshot saved" into the frame before capture.

If user feedback after capture is useful, recommend only a non-intrusive mechanism that cannot contaminate the screenshot, and keep it optional/deferred unless trivially supported.


ERROR HANDLING

Determine minimal behavior if screenshot writing fails.

The game should not crash merely because a development screenshot cannot be saved if the current API permits graceful handling/detection.

If Raylib's screenshot API exposes no success/failure result, state that limitation rather than inventing one.

Do not implement speculative filesystem retry loops.


NO VISUAL-ASSET ANALYSIS

Do not open or analyze existing:

- screenshots;
- images;
- videos;
- MAME frames;
- gameplay recordings.

Do not inspect:

- Gemini material;
- Incitti source;
- raw ROM files;
- web sources unless required solely to verify an unavailable local Raylib API fact.

This is a source-architecture investigation.


SOURCE SCOPE

Read only files directly necessary to establish:

- program/main render loop;
- fixed-step timing;
- input/debug controller;
- gameplay recording;
- visible playback;
- playback pause/step/reverse controls;
- renderer draw ordering;
- audio event architecture;
- runtime-file/output conventions.

Do not perform a broad repository audit.


NO IMPLEMENTATION

Do not modify:

- production source;
- tests;
- documentation;
- project files;
- .gitignore;
- presentation;
- controls;
- recordings.

Only create:

- ignored prompt log;
- ignored report.


DESIGN CLASSIFICATION

Finish with exactly one:

READY FOR BOUNDED PAUSE/SCREENSHOT IMPLEMENTATION

Use if the seven required behaviors fit naturally into the current fixed-step/render architecture without recording-schema changes or broad redesign.

PAUSE ARCHITECTURE NEEDS REFACTOR

Use if correct pause/resume requires meaningful restructuring of fixed-step timing or input.

SCREENSHOT API GAP

Use if the required host-window capture cannot be implemented with the project's available libraries/API.

RECORDING/PLAYBACK CONFLICT

Use if the requested pause semantics cannot remain deterministic without recording/playback redesign.

INSUFFICIENT EVIDENCE

Use if a responsible implementation cannot yet be specified.


REPORT

Preserve this prompt under AI-Logs/Prompts.

Create:

AI-Logs/Reports/2026-11-14-live-pause-screenshot-feasibility-report.md

Include:

- starting Git state;
- files inspected;
- current game-loop timing architecture;
- current input/control architecture;
- existing playback pause architecture;
- recommended live-pause state ownership;
- accumulator handling;
- gameplay-input behavior while paused;
- recording semantics;
- playback semantics;
- screenshot API/signature;
- screenshot capture point in render lifecycle;
- screenshot contents;
- storage directory recommendation;
- filename recommendation;
- key recommendations and conflict analysis;
- audio semantics;
- deterministic-state proof;
- multiple-screenshot behavior;
- error-handling limitations;
- implementation file/test scope;
- decision classification;
- smallest recommended implementation milestone;
- final Git state;
- confirmation current candidate unchanged;
- confirmation nothing staged/committed/pushed;
- confirmation no prohibited visual/reference material inspected.


FINAL RESPONSE

Report:

1. Whether investigation completed.
2. Primary classification.
3. Starting/final Git state.
4. Candidate files unchanged.
5. Game-loop/fixed-step architecture.
6. Recommended pause implementation point.
7. Accumulator strategy.
8. Resume/catch-up conclusion.
9. Gameplay-input behavior while paused.
10. Recording behavior.
11. Playback behavior.
12. Screenshot API.
13. Correct screenshot-capture point.
14. Exact intended screenshot contents.
15. Storage-directory recommendation.
16. Filename policy.
17. Pause-key recommendation.
18. Screenshot-key recommendation.
19. Audio behavior.
20. Determinism/RNG conclusion.
21. Multiple-screenshot conclusion.
22. Error-handling conclusion.
23. Required implementation files.
24. Required test scope.
25. Smallest recommended implementation milestone.
26. Report path.
27. Confirmation nothing staged, committed, or pushed.
28. Confirmation no visual/prohibited sources were inspected.

Do not implement anything.

TEMPEST_LIVE_PAUSE_SCREENSHOT_FEASIBILITY_20261114