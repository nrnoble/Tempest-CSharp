# Paused playback screenshot chord correction

## Result

Implemented Ctrl+Shift+S as the Tempest-owned screenshot command during paused
visible file playback. Either left/right Control and left/right Shift are
accepted. The command requires Raylib's S press edge, is sampled only in the
paused file-playback path, and remains separate from `PlayerInput` and the
recording format.

The prior manual evidence was retained accurately: Neal observed one logical
F12, no F11, no project request/destination, and a native `screenshot000.png`.
F12 is sampled only by the optional diagnostic; neither F11 nor F12 is a
Tempest capture binding. The bounded diagnostic now records Control/Shift
observations, S/F12 press edges, project request/destination/file status, and
new or overwritten generic native screenshots.

The existing capture order, paused presentation, boundary-aware filename,
collision-safe suffixing, application-root path policy, and lazy
`UserData\Screenshots` creation remain in place. A screenshot does not create
`UserData\GameplayRecordings`. No native library, OS hook, or raw input
mechanism was changed.

## Files changed

- Input and capture: `src/Tempest.Game/Systems/InputSystem.cs`,
  `src/Tempest.Game/Debug/PlaybackScreenshotCapture.cs`,
  `src/Tempest.Game/Debug/PlaybackScreenshotKeyDiagnostic.cs`,
  `src/Tempest.Game/Program.cs`.
- Tests: `tests/Tempest.Game.Tests/PlaybackScreenshotCaptureTests.cs`.
- Documentation: `README.md`, `TEMPEST-BEHAVIOR-SPEC.md`,
  `TEMPEST-DEVELOPMENT-NOTES.md`.
- The existing screenshot/UserData candidate changes remain in the worktree.

## Validation

- Focused screenshot, input, CLI, and application-path tests: 61 passed,
  0 failed.
- Full `dotnet test TempestCSharp.slnx`: 971 passed, 0 failed, 0 skipped.
- `dotnet build TempestCSharp.slnx --no-restore`: succeeded with 0 warnings
  and 0 errors.
- `git diff --check`: passed; Git emitted only existing LF-to-CRLF notices.
- No game launch, staging, commit, or push was performed. Existing
  `screenshot000.png` through `screenshot006.png` remain untouched and
  untracked.

## Pending manual acceptance

Neal must run:

```powershell
dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\levels1-7.json --screenshot-key-diagnostic
```

Pause visible playback, scrub to an identifiable boundary, then press
Ctrl+Shift+S. Confirm the diagnostic observes Control, Shift, and one S press
edge, reports one project request and an existing boundary-aware PNG under the
application's `UserData\Screenshots`, and reports no new/overwritten generic
`screenshotNNN.png`. Confirm the image and PAUSED overlay remain correct, the
boundary/cursor do not advance, another fresh press uses a unique suffix, and
running playback does not capture. Manual acceptance remains pending; automated
tests cannot establish absence of Raylib's native output.
