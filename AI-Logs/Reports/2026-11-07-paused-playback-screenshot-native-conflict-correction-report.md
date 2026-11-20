# Paused playback screenshot native-conflict correction

## Starting worktree

- Branch `master`; HEAD and `origin/master` were both
  `50be0b88c9f8f2f1ac9229db54dc0cfa7a292f50`.
- Worktree already held the uncommitted screenshot/UserData implementation and
  its documentation/tests. No changes were staged.
- The new manual report says generic `screenshot002.png` through
  `screenshot005.png` appeared. Existing user screenshots were not moved,
  renamed, deleted, staged or otherwise changed by this correction.
- The previous report's F11-separates-project-capture assumption is explicitly
  superseded here. The old report remains historical.

## Manual evidence and cause

- Neal reports that the accepted screenshot interaction during the F11
  candidate again resulted in native-style generic files and Raylib's
  screenshot success messages. This proves native capture still occurred
  during that manual test.
- Neal did not provide an input-event trace or confirm whether the matching
  boundary-aware file appeared as well. Therefore the exact physical-to-
  logical mapping (F11, F12, both, or another function key) and whether both
  capture paths ran remain unproven. No hardware/Fn-lock cause is asserted.
- Raylib 6.0 `rcore.c` lines 927-935 show `EndDrawing` polls input and then
  checks `IsKeyPressed(KEY_F12)`, saving `screenshot%03i.png` in the process
  working directory. Raylib-cs 8.1.0 binds the official Raylib 6.0 native
  libraries; the installed native DLL contains the screenshot filename
  string. This is the proven native mechanism, but not proof of which key
  Neal's keyboard surfaced.
- The installed binding offers `TakeScreenshot(string)` but no supported
  screenshot-key setter or runtime disable option. F12 remains native and
  cannot be disabled without rebuilding/replacing Raylib or platform-level
  interception; neither is done.
- Search of the project input mappings finds no F9 gameplay/playback binding.
  Raylib 6.0's `rcore.c` has no F9 screenshot/GIF action; the native automatic
  screenshot action found is F12. F10 was avoided because it may activate
  Windows window-menu behavior. F9 is selected provisionally to avoid the
  known app/native conflicts. The opt-in diagnostic below lets Neal establish
  the actual function-key events without assuming F9/F11 behavior.
- Project-owned output from the second manual attempt is unknown. The
  one-shot diagnostic reports the Tempest request, exact reserved destination,
  and whether the file exists, independently of native output.

## Code and retained policies

- Project screenshot key changed from F11 to F9. Production code continues to
  query `Raylib.IsKeyPressed`, not held-key or repeat input, and capture
  remains paused visible file playback only.
- Added opt-in `--screenshot-key-diagnostic` for visible `--playback-file`.
  While paused, it samples F1-F12 press edges for an 18-frame window after the
  first function-key event, then reports once:
  - each observed function-key press count, which can reveal multiple logical
    key events during one physical attempt;
  - whether the project capture request was created, its exact destination,
    and whether that file exists;
  - generic `screenshotNNN.png` files newly created or overwritten since the
    diagnostic began.
- The diagnostic baseline includes path, size and last-write time to detect
  Raylib overwriting existing generic files (Raylib's counter starts from
  zero). It does not stream unrelated keyboard events. With no diagnostic
  option, ordinary operation remains quiet.
- No keyboard remapping, OS interception, Raylib replacement or screenshot
  timing change was introduced. The already accepted render order and
  boundary-aware `UserData\Screenshots` path are retained.
- `AppContext.BaseDirectory`, `UserData\GameplayRecordings`, bare-name
  resolution, explicit relative/absolute paths, no-overwrite suffixing and
  disposable `Temp\GameplayRecordings` behavior are unchanged.
- The game, playback/scrubbing, cursor, simulation, `PlayerInput` contract and
  recording formats are unchanged.

## Files changed in this correction

- Production: `src/Tempest.Game/Debug/DebugController.cs`,
  `src/Tempest.Game/Debug/PlaybackScreenshotCapture.cs`,
  `src/Tempest.Game/Debug/PlaybackScreenshotKeyDiagnostic.cs`,
  `src/Tempest.Game/Program.cs`.
- Tests: `tests/Tempest.Game.Tests/InputDiagnosticTests.cs`,
  `tests/Tempest.Game.Tests/PlaybackScreenshotCaptureTests.cs`.
- Documentation: `README.md`, `TEMPEST-BEHAVIOR-SPEC.md`,
  `TEMPEST-DEVELOPMENT-NOTES.md`.
- The existing changes from the previous correction remain in the worktree.
- Full user prompt is paired at
  `AI-Logs/Prompts/2026-11-07-paused-playback-screenshot-native-conflict-correction-prompt.md`.
  The existing repository ignore policy ignores `AI-Logs/`.

## Validation

- Focused screenshot/path/CLI tests: 20 passed, 0 failed, 0 skipped.
- Playback/input/rendering regression filter: 147 passed, 0 failed, 0 skipped.
- Full `dotnet test TempestCSharp.slnx`: 966 passed, 0 failed, 0 skipped.
- `dotnet build TempestCSharp.slnx`: succeeded, 0 warnings, 0 errors.
- `git diff --check`: passed; only the existing LF-to-CRLF notices were
  reported for files already known to Git.
- One earlier focused attempt transiently encountered Windows application
  control blocking the test-output copy of `Tempest.Game.dll`. A later full
  suite and a fresh focused run both succeeded. No security-control bypass was
  attempted.
- No interactive game launch was performed.

## Neal's manual retest

Run:

```powershell
dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\levels1-7.json --screenshot-key-diagnostic
```

1. Note the highest existing generic `screenshotNNN.png` number before launch.
2. Pause with Space and scrub to an identifiable boundary.
3. Press F9 exactly once and wait at least one third of a second for the
   one-shot diagnostic.
4. Verify the diagnostic reports observed F1-F12 logical press counts,
   `Project capture request created: True`, the exact destination and an
   existing project PNG. Record whether it also observed F11/F12 or another
   function key.
5. Verify exactly one boundary-aware PNG appears under the application's
   `UserData\Screenshots\`, with the already accepted image and PAUSED overlay.
6. Verify no generic `screenshotNNN.png` was newly created or overwritten in
   the working directory. Compare against both the pre-launch inventory and
   diagnostic output.
7. Press F9 again at the same boundary. Verify one unique-suffix UserData file
   is added and no generic file changes.
8. Verify boundary/cursor remain unchanged, capture does nothing while
   playback runs, and playback scrubbing remains correct.

If F9 does not register, use the diagnostic's F1-F12 counts to identify the
actual logical key; do not infer a key mapping from the generic filename alone.

## Final Git state

- HEAD and `origin/master` remain
  `50be0b88c9f8f2f1ac9229db54dc0cfa7a292f50`.
- Changes remain unstaged and uncommitted. No push occurred.
- Existing generic screenshot artifacts were not modified.
- No gameplay change, recording-format change, snapshots,
  enemy-explosion implementation, Level 8/9 work or unrelated staging occurred.
- Neal's manual retest is still required; acceptance remains pending.
