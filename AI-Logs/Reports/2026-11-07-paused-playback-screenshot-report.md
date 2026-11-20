# Paused Playback Screenshot Capture — Implementation Report

Date: 2026-11-07

## Status

**PAUSED-PLAYBACK SCREENSHOT CAPTURE: IMPLEMENTED / MANUAL ACCEPTANCE PENDING.**
Neal must verify an actual saved PNG and its overlay before this candidate is
accepted. No interactive game was launched.

## Starting Git state

- Branch: `master`, tracking `origin/master`.
- Starting `HEAD` and `origin/master` were both
  `50be0b88c9f8f2f1ac9229db54dc0cfa7a292f50`
  (`Add reverse recorded playback scrubbing`).
- `origin` fetch and push URLs were both
  `https://github.com/nrnoble/Tempest-CSharp.git`.
- Tracked worktree and index were clean; there were no unexplained
  nonignored untracked files.
- The complete supplied prompt was copied to
  `AI-Logs/Prompts/2026-11-07-paused-playback-screenshot-prompt.md`.
  Source and archived SHA-256:
  `0D2A6E129A5AD1171A6D3D89900D7E118746073CB80E13EBAD073349708615AE`.

## API, keys, and Raylib behavior

- The project uses Raylib-cs 8.1.0. Its installed XML API documents
  `Raylib.TakeScreenshot(string)`, which saves the current screen as PNG; the
  project build also verifies the binding and selected key compile.
- Print Screen is represented by the binding, but is not selected: Windows
  may process it as an OS screenshot shortcut as well as deliver it to the
  focused application. Reliably making it application-owned would require
  out-of-scope global/low-level input interception.
- The bundled native Raylib build contains its default `screenshot%03i.png`
  output path. Raylib's F12 capture behavior would create an uncontrolled,
  potentially duplicate screenshot, so the project-owned paused-playback
  command uses **F11** instead.
- F12 remains a separate native Raylib shortcut; this feature does not bind
  or use it. Press F11 for boundary-named paused-playback captures.

## Capture behavior

- Capture is enabled only for active, paused `--playback-file` playback.
  Running playback, built-in playback scenarios, recording, live gameplay,
  and headless playback do not request project screenshots.
- F11 is polled only in that paused file-playback state and is not added to
  `InputSystem`, `PlayerInput`, playback frames, or recording data.
- The renderer invokes the Raylib capture callback after the game and PAUSED
  boundary/duration overlay are drawn, after `EndMode2D`, and before
  `EndDrawing` presents the completed buffer.
- The callback reads the current displayed level and session boundary. It
  does not call `RunOneStep`, consume a frame, reconstruct a session, resume
  playback, change RNG/game state, or move the player.
- Output is created on demand under ignored `Temp\Screenshots\`.
- Filenames use
  `playback-level-{level}-boundary-{boundary:D6}.png`; repeated captures use
  suffixes such as `-002`. A `FileMode.CreateNew` reservation prevents
  overwriting existing names, including when another process races for one.
- Success prints the repository-relative path. File/API output errors in the
  supported I/O exception set are reported to stderr, incomplete reservations
  are removed when possible, and playback remains paused.
- Recording v1/v2 formats and contents, `GameplayPlaybackSession`,
  headless playback, and accepted Phase 1/2/3 scrubbing semantics are
  unchanged. No gameplay or audio behavior changed.

## Files changed

Production:

- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Debug/PlaybackScreenshotCapture.cs` (new)
- `src/Tempest.Game/Rendering/VectorRenderer.cs`

Tests:

- `tests/Tempest.Game.Tests/PlaybackScreenshotCaptureTests.cs` (new)

Documentation:

- `README.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `TEMPEST-BEHAVIOR-SPEC.md`

The prompt and this report are ignored `AI-Logs/` records.

## Automated validation

- Focused screenshot policy, naming, unique-suffix, output, error, and
  boundary/cursor tests: **8 passed, 0 failed, 0 skipped**.
- Combined neighboring input/rendering and Phase 2/3 playback regressions
  (`VisiblePlaybackControllerTests`, `GameplayPlaybackSessionTests`,
  `HeadlessGameplayPlaybackTests`, recording, input, vector-font, and player
  shot rendering): **133 passed, 0 failed, 0 skipped**. This selection
  includes the focused tests.
- `dotnet test TempestCSharp.slnx`: **954 passed, 0 failed, 0 skipped**.
- `dotnet build TempestCSharp.slnx`: **0 warnings, 0 errors**.
- `git diff --check`: passed. Git emitted only existing LF-to-CRLF working
  copy advisories; no whitespace errors.
- `Temp\Screenshots\` matches the existing `.gitignore` `Temp/` rule. No
  screenshot directory or PNG was generated during automated validation.

## Manual acceptance required

Run from the repository root:

```powershell
dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\levels1-7.json
```

Neal should:

1. Press Space to pause, then use Left/Right and Shift+Left/Right to choose a
   boundary.
2. Press F11 and verify a PNG appears under `Temp\Screenshots\`.
3. Open it and verify it shows the Tempest window content at the selected
   boundary, including the PAUSED boundary/duration overlay.
4. Capture the same boundary again and verify the first PNG remains unchanged
   and the second uses a unique suffix.
5. Verify the boundary/cursor stays fixed during capture and normal scrubbing
   continues afterward.
6. Verify F11 during running playback does nothing, and live gameplay after
   handoff is unaffected.

Do not use F12 for this check; that is the bundled native Raylib screenshot
shortcut, not the project-owned boundary-aware command.

## Final Git state and scope

- `HEAD` and `origin/master` remain
  `50be0b88c9f8f2f1ac9229db54dc0cfa7a292f50`.
- The candidate is unstaged and uncommitted; no push occurred.
- Tracked changes are limited to the five implementation/documentation files
  listed above; the new capture service and its tests are nonignored
  untracked files. The prompt/report remain ignored.
- No enemy-explosion implementation, gameplay change, recording change,
  snapshot, Level 8/9 work, staging, commit, or push occurred.
