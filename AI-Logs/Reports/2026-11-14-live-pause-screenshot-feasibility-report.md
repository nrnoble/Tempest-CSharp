# Live pause and screenshot feasibility

## Scope and starting state

Read-only architecture investigation; no production, test, project, presentation, control, or recording files were modified.

- Branch: `master`
- HEAD: `385f603a94391756b2ca6cccd524dc17b281e782`
- `origin/master`: same commit; upstream is `origin/master`
- Index: clean
- Tracked worktree: the ten modified files were the in-progress presentation candidate (Program, DebugController, DisplayConfiguration, Projection, VectorRenderer, and their related tests); no unrelated tracked changes were present.
- Non-ignored untracked files: none
- Starting diff summary: 10 candidate files, 1,776 insertions and 88 deletions.

The candidate was left unchanged. The prompt and this report are under ignored `AI-Logs/`. Nothing was staged, committed, or pushed.

## Files and evidence examined

- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Systems/InputSystem.cs`
- `src/Tempest.Game/Systems/PlayerInputRecorder.cs`
- `src/Tempest.Game/TempestGame.cs` (`RunOneStep`)
- `src/Tempest.Game/Debug/VisiblePlaybackController.cs`
- `src/Tempest.Game/Debug/GameplayPlaybackSession.cs`
- `src/Tempest.Game/Debug/PlaybackScreenshotCapture.cs`
- `src/Tempest.Game/Debug/PlaybackScreenshotKeyDiagnostic.cs`
- `src/Tempest.Game/ApplicationPaths.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `src/Tempest.Game/Rendering/DisplayConfiguration.cs`
- `src/Tempest.Game/Systems/AudioSystem.cs`
- `src/Tempest.Game/Tempest.Game.csproj`
- `.gitignore`
- Installed `Raylib-cs` 8.1.0 XML API documentation and relevant local references/usages.

No images, screenshots, videos, gameplay recordings, `TEMP/myNotes.md`, Incitti material, ROMs, or web sources were inspected.

## Current loop and timing

`Program.cs` initializes the Raylib window, calls `Raylib.SetTargetFPS`, and loops. Each ordinary live loop checks quit, collects eligible player input, calls `TempestGame.RunOneStep` once, captures that same tick into a recording if active, increments `simulationTick`, and calls `VectorRenderer.Draw`.

The application does not read frame delta, maintain a fixed-step accumulator, or schedule multiple simulation steps to catch up. Raylib's target-FPS pacing determines loop frequency; the game advances one simulation step per loop. Thus there is no application accumulator to freeze, clear, reset, or rebase. Frame-time spikes do not result in multi-step catch-up in the current loop.

`RunOneStep` advances gameplay and tick-driven presentation state, including effects and animation counters, and may emit audio events. The renderer draws from the current game state; its game-over animation path draws existing vector records rather than advancing them.

## Recommended live-pause architecture

Keep live pause as process/control state in the main loop, not in `TempestGame`, gameplay recording state, or playback session state. Poll quit as today, then handle the live pause toggle before live input and `RunOneStep`.

- Running: process quit and pause controls, collect gameplay input, run one step, record that tick, then draw.
- Paused: continue processing quit, pause, and screenshot controls; do not call `RunOneStep`, capture a recording tick, or increment `simulationTick`; draw the unchanged game state every loop.
- Resume: toggle out of pause and return to the ordinary one-step-per-loop path. There is no elapsed-pause delta to apply and no accumulator catch-up.

This is the existing loop with a step gate (option C from the prompt). A nested blocking pause loop, timing redesign, and accumulator reset are unnecessary. Rendering can remain continuous at the configured target frame rate, and the window-close/Enter quit path remains responsive.

### Input while paused

Current gameplay input is polled as levels: arrows, Ctrl/mouse fire, and gamepad axis use current down/axis values; zap uses a press edge and mouse movement uses `GetMouseDelta`. Paused loops must not forward any gameplay input into simulation. Held level-based controls will be observed on the first resumed step if still held; that matches the current input model and avoids adding a separate release-to-rearm state. Consume/discard mouse delta during paused loops (or at the pause/resume boundary) so spinner movement cannot be replayed as gameplay input.

Pause and screenshot controls should be polled separately from gameplay input. Avoid buffering gameplay controls for later execution.

### Recording, determinism, and RNG

Recording currently captures exactly once alongside each `RunOneStep`, with contiguous simulation-tick numbering enforced by `PlayerInputRecorder`. During pause, neither step nor capture occurs, so the tick number and recording frame count simply stop. Pause and screenshot commands remain outside `PlayerInput` and recording data; the existing schema/version need not change.

Because no simulation step runs while paused, no game RNG is consumed by pause or capture. Rendering and file capture do not call `RunOneStep`. For equal initial state and equal per-tick gameplay inputs, duration or count of pauses/screenshots therefore does not change the simulation result. This is achievable with the current architecture.

## Playback behavior and boundaries

Visible playback already has separate `VisiblePlaybackController.IsPaused` state. Space toggles playback pause; Right advances one recorded frame (five with Shift); Left reconstructs one boundary backward (five with Shift) while paused. `Program` processes these controls outside live gameplay input and renders each loop. Preserve that controller and its distinct pause state rather than sharing a Boolean with live pause.

The live Pause binding should control normal live gameplay only. Existing visible playback controls remain authoritative; headless playback has no window to capture. The existing screenshot feature is currently narrower: `PlaybackScreenshotCapture` is requested only for paused `--playback-file` playback using Ctrl+Shift+S. It does not currently cover normal gameplay, running playback, or the built-in visible playback profile.

A shared screenshot request/capture path can naturally cover normal live gameplay and any visible playback state without altering playback stepping. On a running/reverse-stepping playback loop, capture after that loop's control/step processing, so the captured image corresponds to the frame actually rendered. For paused playback, it must not call `ProcessControls` in a way that steps or reconstructs the session.

## Screenshot API, frame, and output

The installed `Raylib-cs` 8.1.0 package exposes `Raylib.TakeScreenshot(string fileName)`, returning `void`; the project already passes this method as an `Action<string>`. Its local XML documentation says it takes a screenshot of the current screen and saves PNG.

`VectorRenderer.Draw` renders the whole window, including portrait scissor viewport, HUD/overlays, and the viewport border. Its `beforeEndDrawing` callback runs after the draw calls and before `Raylib.EndDrawing`; a source comment identifies this as the completed back buffer read point. Queue screenshot requests during input polling and invoke capture through this callback, not at key-poll time. This captures the full 1280 x 960 host frame, not only the 720 x 960 viewport, and includes the host margins and visible presentation elements.

The current `ApplicationPaths` already selects `UserData\\Screenshots`, and root `.gitignore` ignores `/UserData/`. Use that existing directory; no `.gitignore` or path-policy change is needed.

Use an understandable live filename such as `live-level-{level}-tick-{tick:D6}-{sequence:D3}.png`. Reserve each destination atomically with `FileMode.CreateNew`, incrementing the sequence on collision. A timestamp alone is not sufficient for multiple rapid captures at the same tick. The existing playback capture helper already uses an atomic reservation and sequence suffix; generalize/reuse that behavior rather than duplicate it. Multiple screenshots during one pause then remain distinct without changing simulation.

Raylib's screenshot method provides no success result. The existing helper handles expected filesystem/path exceptions, logs a failure, and checks that the reserved output exists and is non-empty afterward. Retain that explicit, non-crashing path rather than inventing retries. A zero-byte reservation must be cleaned up on failure when possible.

## Keys and audio

Current game keys: Left/Right move; Left Ctrl or left mouse fires; Space or right mouse triggers Super Zapper; R and P are restart/replay press aliases in applicable end states; Enter quits. Visible playback uses Space, Right/Left, and Shift modifiers. Paused file playback uses Ctrl+Shift+S for screenshots. F12 is polled by the screenshot-key diagnostic. Do not assign P, Space, arrows, Enter, the fire controls, or F12 to the new live controls.

Recommended non-conflicting candidates are **F9 for Pause** and **F10 for Screenshot**. Neither is used by the inspected gameplay/playback input path; both are discrete press-edge controls. This avoids changing P's restart alias or the existing Ctrl+Shift+S playback chord.

Gameplay audio events are dispatched from simulation code through `IAudioEvents`; `AudioSystem` plays one-shot sounds with `Raylib.PlaySound` and has no pause/resume operation for active sounds. Skipping simulation prevents new gameplay audio events. Let sounds already playing finish naturally; do not add audio-engine pause complexity.

No pause overlay is necessary for the requested behavior. Keep the existing frame unmodified before capture; do not draw a status message into the screenshot. Optional post-capture terminal feedback already fits the capture helper's existing pattern.

## Smallest implementation and validation scope

Likely production scope:

1. `Program.cs`: a live-only pause state and step/record/tick gate; preserve quit polling and drawing in both states; queue screenshot input and pass the request to the existing post-render callback.
2. `Systems/InputSystem.cs`: dedicated F9/F10 press-edge polling and paused mouse-delta discard if needed.
3. `Debug/PlaybackScreenshotCapture.cs` (or a narrowly generalized screenshot helper): accept a generic request, retain safe `UserData\\Screenshots` path handling, atomic unique names, and explicit failure reporting. Preserve existing file-playback screenshot behavior.

No `TempestGame`, recording schema, playback-controller redesign, presentation, project-file, or `.gitignore` change is indicated.

Focused automated coverage should verify: pause toggles without advancing a live simulation/recording tick; resume advances exactly the next ordinary tick, independent of paused duration; paused gameplay input and mouse delta are discarded; quit remains available; capture requests are deferred until after frame drawing; repeated paused captures use distinct files; capture failure is reported without a crash; and existing playback pause/step/reverse and screenshot tests remain valid. A small pure pause-control helper is only warranted if needed to test state transitions cleanly; do not add an accumulator or refactor simulation.

No tests or builds were run because this was a read-only feasibility investigation, not an implementation.

## Result

**READY FOR BOUNDED PAUSE/SCREENSHOT IMPLEMENTATION**

The requested behavior fits the existing step/render loop, can remain outside deterministic simulation and recording state, and uses an available host-window screenshot API. No timing, recording-schema, or playback redesign is required.

Smallest milestone: add live-only pause gating and a shared post-render screenshot request/capture path, keeping playback's pause/navigation state separate and preserving its existing screenshot behavior.

## Final state

Final Git check: branch `master`; HEAD and `origin/master` remain `385f603a94391756b2ca6cccd524dc17b281e782`; upstream remains `origin/master`; index is clean; tracked diff remains the same ten candidate files and 1,776 insertions/88 deletions; no non-ignored untracked files. Both log files are ignored by `/AI-Logs/`.

The candidate remains unchanged; no source or test edits were made. Nothing was staged, committed, or pushed.
