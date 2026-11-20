# Visible Recorded-Playback Controls — Phase 2 Report

Date: 2026-11-07

## 1. Starting Git state

- Repository: `C:\Users\nrnob\source\repos\Tempest`.
- Branch: `master`, tracking `origin/master`.
- Starting `HEAD` and `origin/master` were both
  `1fd28c1b9a7eaad0a0a1ed35c781f57a8b3d90af`
  (`Add shared deterministic playback session`).
- The tracked worktree and index were clean; there were no unexplained
  nonignored untracked files.
- The complete supplied task, including its completion token, was copied to
  `AI-Logs/Prompts/2026-11-07-visible-playback-controls-phase2-prompt.md`.
  Its SHA-256 matched the supplied attachment. AI-Logs is ignored by Git.

## 2. Existing visible playback path

Before this change, `Program.cs` held the visible replay tick, retrieved
`PlayerInputReplay.GetFrame`, applied the shared gates, called
`TempestGame.RunOneStep`, advanced its tick, reported completion and handed
input back to live play. It shared the session's input-gate helper but
duplicated cursor and recorded-frame execution.

## 3. Migration to `GameplayPlaybackSession`

Visible `--playback-file` now constructs one `GameplayPlaybackSession` after
the recording and audio adapter are ready. `Program.cs` renders the exact
`TempestGame` owned by that session. The session is authoritative for frame
retrieval, playback gates, `RunOneStep`, cursor/boundary advancement and
completion; the visible loop no longer has a second file-playback engine.
The built-in Level 1 profile also uses this session. The built-in Level 4
profile retains its terminal diagnostic startup and previous playback path;
that startup contract is not represented by the production-only session.

## 4. Visible controller ownership

`VisiblePlaybackController` owns pause state, pause/step command handling and
one-time completion-report state. It has no Raylib dependency. `Program.cs`
continues to own the window/audio lifecycle, quit handling, input polling,
rendering, real-time loop, completion message and live-input handoff.

## 5. Debugger-control/input separation

`InputSystem.ReadPlaybackControls` samples only Space and Right Arrow with
Raylib press-edge queries and returns a separate `PlaybackControlInput`.
Neither key is added to `PlayerInput`; debugger commands are never sent to the
recording/session frame source or serialized. Left Arrow, spinner/mouse,
controller axis, Fire and restart are not sampled as live gameplay input while
session-backed playback is active.

## 6. Space pause/resume

Space toggles running/paused on a press edge. A toggle iteration consumes no
simulation frame. Pausing does not call `RunOneStep`, change session
boundary/cursor, activate Super Zapper or capture ignored gameplay input.
When paused at the final boundary, Space releases debugger ownership; live
input is sampled on the next outer iteration, not on the resume-command
iteration.

## 7. Right Arrow forward step

While paused, one Right Arrow press calls `GameplayPlaybackSession.StepForward`
once, including the recorded frame's normal input gates and restart bit.
Running playback advances its ordinary one session frame per outer iteration;
Right Arrow adds no step. At completion, stepping is inert. Raylib's
`IsKeyPressed` edge query is used rather than held-key polling.

## 8. Left Arrow reservation

Left Arrow is not polled by the playback-control seam, has no playback action,
does not move the Blaster or affect a recorded frame, and is reserved for
Phase 3B backward reconstruction.

## 9. Paused rendering behavior

The outer loop continues polling quit/debugger controls and redraws the same
Tempest state. No simulation, cursor, boundary or `RunOneStep`-driven
presentation state advances while paused. The window remains responsive.

## 10. Paused audio behavior

No new simulation audio events occur while paused because the session is not
stepped. Existing audio voices may finish naturally; the audio system was not
stopped or redesigned.

## 11. Forward-step audio behavior

A paused step executes the normal production `RunOneStep` once, so events from
that recorded tick can play once. Redrawing the boundary does not replay those
events.

## 12. Overlay/status presentation

Paused session-backed playback draws a small uppercase
`PAUSED  boundary / duration` status at logical center `(400, 570)`, scale 2,
through the shared `VectorFont`. Digits and slash were added to that font for
the boundary display. The optional renderer status is absent during running
playback and live play, including the completed/unpaused handoff.

## 13. Final-boundary behavior

If a paused forward step consumes the final frame, that recorded state remains
paused and is rendered. The cursor/boundary stays at duration; no extra frame,
live input or same-iteration live simulation is consumed. Right Arrow does
nothing there. Space unpauses, then the following outer iteration enters the
live-input path.

## 14. Completion reporting

Completion reporting is one-shot. Running playback retains the existing
`Gameplay playback completed after N simulation ticks; live controls resumed.`
message. If playback completes while paused, the message instead says it is
paused at the final boundary, avoiding a false claim that live controls have
already resumed.

## 15. Live-input handoff

After an unpaused session reaches duration, the final recorded state is drawn
for that iteration; live input runs on the next normal outer iteration. A
paused final boundary stays under controller ownership until resumed. The
recording cursor remains at duration.

## 16. Ordinary gameplay control restoration

After handoff, the visible controller no longer intercepts controls.
`InputSystem.ReadPlayer` restores ordinary mappings, including Space for Super
Zapper and Left/Right for movement. Enter, Escape and window-close behavior
remain in the outer loop.

## 17. Recording format preservation

No recording format, startup metadata, frame fields, strict validation, save
behavior, recording capture or file behavior was changed. Playback controls
are session/UI state only; v1 and v2 remain unchanged.

## 18. Phase 1/headless preservation

`GameplayPlaybackSession` and `GameplayStateSummary` remain Raylib-independent.
No Raylib polling, rendering, window title, pacing or visible-control state was
added to the session. No headless runner/CLI, backward reconstruction,
snapshots or RNG-state capture was added.

## 19. Production files changed

- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Debug/VisiblePlaybackController.cs` (new)
- `src/Tempest.Game/Systems/InputSystem.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `src/Tempest.Game/Rendering/VectorFont.cs`
- `src/Tempest.Game/Rendering/PlaybackOverlayStatus.cs` (new)

## 20. Test files changed

- `tests/Tempest.Game.Tests/VisiblePlaybackControllerTests.cs` (new)
- `tests/Tempest.Game.Tests/GameplayRecordingTests.cs`
- `tests/Tempest.Game.Tests/InputSystemTests.cs`
- `tests/Tempest.Game.Tests/VectorFontTests.cs`

The tests cover session-backed file execution and selected production-state
equivalence, boundaries/cursor, pause/resume, held-key press-edge behavior,
audio, final-boundary restart/handoff, normal controls after handoff, and
deterministic vector status geometry. Existing recording tests retain v1/v2
coverage.

## 21. Documentation changed

- `README.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `TEMPEST-BEHAVIOR-SPEC.md`

The documentation describes the Phase 2 candidate and explicitly leaves Neal's
manual visual acceptance pending. The prompt and this report are local
AI-Logs records and are ignored by Git.

## 22. Focused test counts

Playback/session, recording, gameplay/input, rendering/font and transition
selection: **110 passed, 0 failed, 0 skipped**.

## 23. Neighboring regression counts

The broader selected regression covered shared session and state summary,
v1/v2 recording/playback, input and Super Zapper, direct startup, progression,
campaign soak, death/re-entry, Levels 3-7, and related rendering/transition
tests: **349 passed, 0 failed, 0 skipped**. This selection overlaps the
focused selection.

## 24. Full-suite counts

`dotnet test TempestCSharp.slnx`: **901 passed, 0 failed, 0 skipped**.

## 25. Build warnings/errors

`dotnet build TempestCSharp.slnx`: **0 warnings, 0 errors**.

## 26. Whitespace validation

`git diff --check`: passed. Git emitted only existing-style LF-to-CRLF
advisories; no whitespace error was reported.

## 27. Manual launch commands

No game was launched. Neal can use an existing recording with:

```powershell
dotnet run --project src\Tempest.Game -- --playback-file <recording-file>
```

For example, if that file exists:

```powershell
dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\testlevel5-3.json
```

## 28. Manual acceptance checklist

Neal should verify:

1. Playback fidelity and correct startup match the accepted recording.
2. Space pauses immediately; the window remains responsive and the image stays
   unchanged over time.
3. The unobtrusive `PAUSED` overlay and boundary/duration are legible.
4. One Right Arrow press advances one simulation tick; repeated taps remain
   one-tick steps, and holding does not run freely.
5. Left Arrow does not move the Blaster or change playback.
6. Space resumes at the exact next recorded frame.
7. Audio finishes naturally while paused, steps emit only their own events,
   and resumed playback does not suppress audio.
8. Playback completes normally; the final paused boundary remains visible,
   then live controls resume.
9. Space returns to Super Zapper and Left/Right to live movement after handoff.
10. Enter exits immediately; no crash, skipped/duplicated event or fidelity
    regression is observed.

Manual acceptance is pending; automated tests do not establish visual/audio
acceptance.

## 29. Remaining Phase 3A/3B roadmap

- **Phase 3A:** reusable headless runner and thin CLI for v1/v2 recordings,
  exact boundary execution, structured state and checkpoints.
- **Phase 3B:** Left Arrow/backward reconstruction by replay-from-start,
  followed by performance measurement before considering snapshots.
- Rewind, fast-forward/speed control, seeking and snapshots remain outside this
  implementation.

## 30. Final Git state

Final Git checks confirmed `master`; `HEAD` and `origin/master` remain at
`1fd28c1b9a7eaad0a0a1ed35c781f57a8b3d90af`. Phase 2 source, tests and
documentation are unstaged. The prompt and report are ignored local AI-Logs
records; no staged changes, commits, pushes or nonignored generated files were
created.

## 31. Scope confirmation

No backward stepping/reconstruction, rewind, fast-forward, speed control,
snapshots, headless CLI, recording-format change, Level 8/9 work or unrelated
gameplay change occurred. No staging, commit, push or interactive game launch
occurred. Stop for Neal's manual visual acceptance.

## Phase 2 Navigation Refinement — 2026-11-07

### Neal's initial manual result

Neal reported: "This is close to perfect. I am very impressed!" His initial
manual testing found normal recorded playback correct, Space pause/resume
working, and one simulation tick per Right Arrow press ideal. He found
tick-by-tick inspection genuinely useful. This report records his result for
the initial Phase 2 candidate; the repeat/group refinement below still needs
his manual acceptance.

### Right Arrow repeat mechanism

The installed Raylib-cs 8.1.0 package exposes
`Raylib.IsKeyPressedRepeat(KeyboardKey)`. The repository's development-only
input diagnostic already reads this API. `InputSystem.ReadPlaybackControls`
combines `IsKeyPressed(Right)` and `IsKeyPressedRepeat(Right)` into one
forward-navigation event; it does not use held-key polling for stepping.
Left and Right Shift are sampled with `IsKeyDown` as a modifier state.

### Tap, hold and Shift+Right behavior

- Right tap while paused: one ordinary `GameplayPlaybackSession.StepForward`.
- Right hold while paused: native repeat events each request another ordinary
  one-tick step; no repeat event means no additional advancement.
- Shift+Right while paused: up to five sequential `StepForward` calls.
- Shift+Right hold: each native Right repeat event with Shift held requests
  another group of up to five sequential steps.
- Right and Shift+Right while running do not add navigation steps; normal
  playback still advances at most one recorded frame per outer iteration.

`VisiblePlaybackController.ProcessControls` returns the number of frames it
actually advanced so `Program.cs` keeps its simulation tick in sync. Groups
stop when `StepForward` reports the recording boundary; they do not throw or
consume a nonexistent frame. Each processed frame uses the normal production
session execution, recording input gates, deterministic order and ordinary
audio events.

### Left Arrow reservation

Left Arrow and Shift+Left are not part of `PlaybackControlInput` and are not
polled as playback navigation. They remain inert during active recorded
playback. Future Phase 3B intent is Left tap = -1, Left hold = repeated -1,
Shift+Left = -5, and Shift+Left hold = repeated -5. No backward reconstruction
or other backward behavior was implemented.

### Input, overlay, audio and live handoff

Playback navigation remains separate from `PlayerInput` and recording files.
The existing live-input path is unchanged, so normal Left/Right movement,
Space/Super Zapper and ordinary Shift behavior return once playback hands
control back. The paused overlay remains `PAUSED  boundary / duration`; tests
confirm it reports the boundary after a +1 and a +5 operation. Paused redraws
do not emit audio; each actual simulation tick retains its normal audio
behavior, including each tick in a five-step group. No separate audio policy
was added.

### Production files changed by refinement

- `src/Tempest.Game/Systems/InputSystem.cs` uses the native repeat event and
  Shift state for the isolated playback input seam.
- `src/Tempest.Game/Debug/VisiblePlaybackController.cs` performs one- or
  five-tick sequential operations and reports actual advanced tick count.
- `src/Tempest.Game/Program.cs` adds the returned tick count to the visible
  loop's playback counter.

### Tests changed by refinement

- `tests/Tempest.Game.Tests/VisiblePlaybackControllerTests.cs` covers repeat
  events, no stepping without a repeat event, running-mode behavior, sequential
  five-tick groups, repeated groups, short final groups, no frame past duration,
  Shift without a forward event, and overlay boundary changes.
- `tests/Tempest.Game.Tests/InputSystemTests.cs` covers initial/repeat event
  unification and separate Shift state.
- `tests/Tempest.Game.Tests/GameplayRecordingTests.cs` keeps session-backed
  file playback/capture compatibility coverage current with the controller's
  advanced-tick return value.

### Documentation and prompt records

`README.md`, `TEMPEST-DEVELOPMENT-NOTES.md`, and `TEMPEST-BEHAVIOR-SPEC.md`
document the accepted intended playback navigation model, the future
unimplemented Left/Shift+Left controls, and pending manual acceptance.

The complete continuation prompt is archived as
`AI-Logs/Prompts/2026-11-07-playback-navigation-refinement-prompt.md`. Its
SHA-256 matches the supplied attachment:
`DBB0E1667369F27C1FE587F8F0066D7170A9BF5FBAC41CD8D0EF115A58D43512`.
Both the prompt and report are under the ignored `AI-Logs/` directory.

### Validation results

- Focused playback/navigation/input/recording/rendering tests:
  **89 passed, 0 failed, 0 skipped**.
- Neighboring playback/gameplay/presentation regression selection:
  **353 passed, 0 failed, 0 skipped**.
- `dotnet test TempestCSharp.slnx`:
  **905 passed, 0 failed, 0 skipped**.
- `dotnet build TempestCSharp.slnx`: succeeded, **0 warnings, 0 errors**.
- `git diff --check`: passed; Git may report line-ending advisories, not
  whitespace errors.

### Manual acceptance after refinement

No interactive game was launched. Neal must verify Space pause/resume; Right
tap and text-editor-like held-key repeat; repeat stopping on release; Shift+Right
single and repeated five-tick groups; accurate paused boundary; Left and
Shift+Left inertness; playback fidelity, completion and live-input handoff;
restored ordinary controls; and absence of skipped/duplicated frames, crashes
or input regressions.

Manual command:

```powershell
dotnet run --project src\Tempest.Game -- --playback-file <recording-file>
```

### Final Git state for refinement

`HEAD` and `origin/master` remain
`1fd28c1b9a7eaad0a0a1ed35c781f57a8b3d90af`. Source, tests and documentation
remain unstaged; the exact prompt copy and this report are ignored. No
staging, commit, push or game launch occurred. Phase 2 refinement is pending
Neal's manual acceptance. No backward behavior, rewind, snapshots, headless
CLI, recording change, Level 8/9 work or unrelated gameplay change was made.
