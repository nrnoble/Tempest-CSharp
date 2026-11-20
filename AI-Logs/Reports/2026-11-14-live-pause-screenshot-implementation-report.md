# Live pause and screenshot implementation

## Status

**Implementation complete; automated validation passed; Neal's manual acceptance is pending.**

This is a development/observation feature. No pause menu, pause overlay, HUD work,
gameplay change, presentation calibration, or durable behavior-specification
update was made.

## Starting Git state

- Branch: `master`
- HEAD: `385f603a94391756b2ca6cccd524dc17b281e782`
- `origin/master`: same commit; upstream: `origin/master`
- Index: clean
- Tracked worktree: the ten modified presentation-candidate files described by
  the feasibility report; no unrelated tracked changes
- Non-ignored untracked files: none
- Starting diff: 10 files, 1,776 insertions, 88 deletions

The candidate was preserved and integrated. No pre-existing edit was reverted.

## Sources inspected

- `AI-Logs/Reports/2026-11-14-live-pause-screenshot-feasibility-report.md`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Systems/InputSystem.cs`
- `src/Tempest.Game/Systems/PlayerInputRecorder.cs`
- `src/Tempest.Game/Debug/VisiblePlaybackController.cs`
- `src/Tempest.Game/Debug/PlaybackScreenshotCapture.cs`
- `src/Tempest.Game/ApplicationPaths.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- Related input and screenshot-capture tests

The task prompt was preserved at
`AI-Logs/Prompts/2026-11-14-live-pause-screenshot-implementation-prompt.md`.
No images, screenshots, videos, gameplay recordings, `TEMP/myNotes.md`, ROMs,
Incitti material, or web sources were inspected.

## Implementation

### Pause state and simulation gating

Live pause is a local process/control Boolean in `Program`; it is not stored in
`TempestGame`, gameplay state, `PlayerInput`, recording data, or playback state.
`InputSystem` polls F9 with a press edge and provides a small pure toggle used by
the live loop.

The existing outer loop still handles window-close and Enter quit signals and
renders every iteration. When paused, it consumes and discards mouse delta, and
does not poll/forward gameplay controls, call `TempestGame.RunOneStep`, capture
a recording tick, or increment `simulationTick`. F9 and F10 are not included
in `PlayerInput`.

On resume, the ordinary live path runs exactly one next simulation step in that
loop. There is no nested pause loop, elapsed-time accumulator, catch-up, or
pause-duration representation. Current level-based controls are read normally
on the resumed step if still held.

The loop gating is verified by source structure rather than an interactive
window test. The pure F9/F10 input mapping and pause toggle have focused unit
coverage.

### Mouse, recording, RNG, and audio

While paused, `InputSystem.DiscardMouseDelta` consumes Raylib's per-loop mouse
delta so it cannot be replayed as spinner movement after resuming. No release-
to-rearm state was added.

Recording remains paired with actual simulation steps. Paused duration is
omitted; tick numbering and recording capture resume on the next real tick.
Recording schema, version, metadata, and input format are unchanged.

No simulation step means pause consumes no gameplay RNG and emits no new
gameplay-generated audio events. Screenshot capture has no game-state/RNG
dependency. Existing one-shot sounds may finish naturally; audio voices are not
suspended or resumed.

### Screenshot request and capture

F10 is polled as a discrete press edge in the live path. The request exists
only for that application-loop iteration and is consumed once by the
`VectorRenderer.Draw` completed-frame callback. The callback runs after the
frame's drawing work and before `Raylib.EndDrawing`; it invokes
`Raylib.TakeScreenshot(string)` through the existing helper. It does not
capture at key-poll time or render any status/overlay.

The existing `ApplicationPaths.ScreenshotsDirectory` remains
`UserData\Screenshots`. Capture reads the complete rendered host back buffer;
it does not crop or reconstruct pixels. Thus the existing 1280 x 960 host
presentation, portrait viewport and margins, current gameplay/HUD/visible
overlays, perspective, and blue viewport border are captured as rendered.
No renderer or presentation-calibration source was changed for this feature.

Live filenames use:

`live-level-{level}-tick-{tick:D6}-{sequence:D3}.png`

For example, repeated captures at Level 9 tick 4820 reserve
`live-level-9-tick-004820-001.png` and
`live-level-9-tick-004820-002.png`. The shared helper uses atomic
`FileMode.CreateNew` reservations and sequence fallback. It reports expected
filesystem/path failures, verifies non-empty output, removes incomplete
reservations when possible, and does not retry the capture call or claim
success for missing/empty output.

The same F10 path can also capture a normal running live frame. It is not
enabled during active visible playback. Existing paused file-playback
Ctrl+Shift+S capture remains on its original request/key path and retains its
playback filename convention and boundary semantics.

### Changed files

Implementation and focused tests:

- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Systems/InputSystem.cs`
- `src/Tempest.Game/Debug/PlaybackScreenshotCapture.cs`
- `tests/Tempest.Game.Tests/InputSystemTests.cs`
- `tests/Tempest.Game.Tests/PlaybackScreenshotCaptureTests.cs`

The existing ten-file portrait/perspective candidate remains modified and
untouched by this implementation.

## Validation

- Focused VSTest subset (input, screenshot capture, recording, playback
  session/controller): **PASS**, 144 passed, 0 failed
- Full VSTest suite: **PASS**, 1,422 passed, 0 failed
- Release solution build: **PASS**, 0 warnings, 0 errors
- `git diff --check`: **PASS** (Git emitted a working-copy LF-to-CRLF warning
  for `InputSystem.cs`; no whitespace errors)

Automated tests cover F9/F10 edge mapping and pause toggling, repeated live
captures at one tick with distinct filenames, expected output directory and
relative paths, and failure/empty-output reporting and reservation cleanup.
Existing playback pause/navigation and screenshot tests passed unchanged.
Frame-buffer contents and the native callback timing cannot be proven by
headless tests; they were checked against the existing renderer callback and
screenshot helper architecture. No interactive game was launched.

## Manual acceptance handoff

Run this command:

```powershell
dotnet run --project .\src\Tempest.Game -- --level 9 --portrait-perspective 125
```

Manual checklist:

1. Begin ordinary Level 9 gameplay.
2. Press F9 and confirm gameplay freezes immediately while its frame remains
   rendered.
3. Wait several seconds.
4. Press F10 once, then press F10 again without resuming.
5. Confirm two distinct PNGs exist under `UserData\Screenshots`.
6. Confirm both show the complete 1280 x 960 host frame, including margins,
   HUD, gameplay, current perspective, and blue portrait border.
7. Confirm neither image contains a pause or screenshot overlay.
8. Press F9 and confirm play resumes from the paused state without catch-up.
9. Confirm normal gameplay controls work after resume.
10. If recording is available, confirm playback reflects gameplay progression
    without representing the wall-clock pause.

Manual gameplay/presentation acceptance remains Neal's responsibility and is
not claimed here.

## Final Git state

- Branch: `master`
- HEAD and `origin/master`: `385f603a94391756b2ca6cccd524dc17b281e782`
- Upstream: `origin/master`
- Index: clean; nothing staged
- Tracked worktree: 14 modified files (the original ten-file candidate plus
  the implementation and test edits above)
- Non-ignored untracked files: none
- Nothing was committed or pushed

`AI-Logs/` remains ignored historical/working material.
