# Paused playback screenshot and UserData correction

## Starting state

- Branch was `master`, tracking `origin/master`; both HEADs were
  `50be0b88c9f8f2f1ac9229db54dc0cfa7a292f50`.
- The worktree already contained the uncommitted paused-screenshot candidate:
  changes in `README.md`, `TEMPEST-BEHAVIOR-SPEC.md`,
  `TEMPEST-DEVELOPMENT-NOTES.md`, `src/Tempest.Game/Program.cs` and
  `src/Tempest.Game/Rendering/VectorRenderer.cs`, plus untracked
  `PlaybackScreenshotCapture.cs`, `PlaybackScreenshotCaptureTests.cs`, and
  `screenshot000.png` through `screenshot003.png`.
- There were no staged changes. The four generic screenshots were preserved;
  none were moved, deleted, or staged.
- Neal's manual report says the screenshot images, PAUSED overlay and exact
  paused boundary look perfect. The remaining failure was screenshot ownership
  and output path; no rendering change was made.

## Raylib ownership investigation

- The installed package is Raylib-cs 8.1.0. Its package README identifies its
  Windows native libraries as the official Raylib 6.0 builds; the deployed
  `raylib.dll` contains the `screenshot%03i.png` and
  `Screenshot taken successfully` strings.
- Raylib 6.0 `src/rcore.c` was checked at upstream lines 930-935. With
  `SUPPORT_SCREEN_CAPTURE`, the native frame loop detects `IsKeyPressed(KEY_F12)`,
  calls `TakeScreenshot(TextFormat("screenshot%03i.png", screenshotCounter))`,
  then increments the counter. This path writes in the process working
  directory. This is the native source of generic screenshot names; the event
  history for each existing image is not known, so no individual old file is
  attributed to a specific keypress.
- The installed binding exposes `TakeScreenshot(string)`,
  `SetConfigFlags` and `SetExitKey`, but no screenshot-key setter or runtime
  screenshot-disable API. `SetConfigFlags` does not change the F12 screenshot
  behavior. Disabling native F12 would require a native rebuild/replacement or
  platform-level key interception, both explicitly excluded.
- F11 remains the project-owned key and does not invoke native F12. One
  accepted F11 press requests one Tempest-managed capture. Native F12 remains a
  separate available Raylib shortcut; avoid F12 during manual acceptance.

## Implementation and policy

- Added `ApplicationPaths`, rooted at `.NET`'s `AppContext.BaseDirectory`, with
  `UserDataDirectory`, `GameplayRecordingsDirectory` and
  `ScreenshotsDirectory`.
- Under the current `dotnet run --project src\Tempest.Game` invocation, the
  application root is
  `src\Tempest.Game\bin\Debug\net9.0\`; user data therefore stays within the
  already-ignored build output. A published standalone app uses the directory
  containing its application binaries/executable. No `.gitignore` change was
  needed.
- Project-owned captures now go to
  `<application root>\UserData\Screenshots\`. The console reports the full
  destination. Names retain displayed level, exact boundary, PNG format and
  collision-safe numeric suffixes; existing files are not overwritten.
  `UserData` and `Screenshots` are created only when an accepted capture is
  requested.
- Bare filenames for `--record-gameplay`, `--playback-file` and
  `--headless-playback` resolve under
  `<application root>\UserData\GameplayRecordings\`. Explicit relative paths
  such as `Temp\GameplayRecordings\levels1-7.json` preserve their existing
  process-working-directory meaning; absolute paths remain unchanged.
  Recording output creation still uses the existing no-overwrite behavior.
- `Temp\GameplayRecordings\` remains the ignored disposable development
  location. Existing recordings are not migrated, rewritten or deleted.
- Capture remains paused visible file playback only. It stays outside
  `PlayerInput` and retains the established render order: game and PAUSED
  overlay are drawn, the capture callback runs, then `EndDrawing` presents the
  frame. No simulation, cursor, RNG, playback/scrubbing, live-input handoff or
  headless behavior was otherwise changed. Recording v1/v2 schemas and strict
  validation are unchanged.

## Files changed

- Production: `src/Tempest.Game/ApplicationPaths.cs`,
  `src/Tempest.Game/Debug/HeadlessGameplayPlaybackCli.cs`,
  `src/Tempest.Game/Debug/PlaybackScreenshotCapture.cs`,
  `src/Tempest.Game/Program.cs`.
- Tests: `tests/Tempest.Game.Tests/ApplicationPathsTests.cs`,
  `tests/Tempest.Game.Tests/HeadlessGameplayPlaybackTests.cs`,
  `tests/Tempest.Game.Tests/PlaybackScreenshotCaptureTests.cs`.
- Documentation: `README.md`, `TEMPEST-BEHAVIOR-SPEC.md`,
  `TEMPEST-DEVELOPMENT-NOTES.md`.
- The pre-existing candidate change in `src/Tempest.Game/Rendering/VectorRenderer.cs`
  was retained to preserve the manually approved capture timing.
- Prompt preserved at
  `AI-Logs/Prompts/2026-11-07-paused-playback-screenshot-userdata-correction-prompt.md`.
  AI-Logs is ignored by the repository's existing ignore policy.

## Validation

- Focused path, screenshot and bare headless-filename tests:
  16 passed, 0 failed, 0 skipped.
- Playback, input and rendering regression filter (including Phase 1/2/3
  playback coverage): 143 passed, 0 failed, 0 skipped.
- `dotnet test TempestCSharp.slnx`: 962 passed, 0 failed, 0 skipped.
- `dotnet build TempestCSharp.slnx`: succeeded, 0 warnings, 0 errors.
- `git diff --check`: passed with no whitespace errors. Git emitted only its
  existing LF-to-CRLF working-copy notices for `README.md` and
  `VectorRenderer.cs`.
- The interactive game was not launched. Manual capture acceptance remains
  required.

## Neal's manual retest

Run:

```powershell
dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\levels1-7.json
```

1. Pause with Space and scrub to an interesting boundary.
2. Press F11 once. Verify exactly one Tempest-owned
   `UserData\Screenshots\playback-level-<level>-boundary-<boundary>.png` is
   created, its image remains perfect with the PAUSED boundary overlay, and
   the boundary does not change.
3. Verify the full path is under the application's `UserData\Screenshots\`
   and no new generic `screenshotNNN.png` appears in the working directory.
4. Press F11 again at the same boundary. Verify the numeric suffix is unique
   and the first file was not overwritten.
5. Continue scrubbing normally; verify F11 while playback is running does
   nothing.
6. Confirm the existing explicit `Temp\GameplayRecordings\...` recording and
   playback commands still work.

## Final Git state

- HEAD and `origin/master` remain
  `50be0b88c9f8f2f1ac9229db54dc0cfa7a292f50`.
- The candidate remains unstaged; the pre-existing generic screenshots remain
  untracked and untouched. No commit or push was made.
- The AI prompt and report are under the already-ignored `AI-Logs/` directory.
- No gameplay change, recording-format change, snapshot implementation,
  enemy-explosion implementation, Level 8/9 work or unrelated staging occurred.
