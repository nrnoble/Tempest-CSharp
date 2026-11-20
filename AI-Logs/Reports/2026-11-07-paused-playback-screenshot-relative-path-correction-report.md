# Paused playback screenshot relative-path correction

## Starting Git state

- Branch `master`; `HEAD` and `origin/master` were both
  `50be0b88c9f8f2f1ac9229db54dc0cfa7a292f50`
  (`Add reverse recorded playback scrubbing`).
- Index was clean. The worktree contained the accumulated uncommitted
  screenshot/UserData candidate and local generic screenshot artifacts
  `screenshot000.png` through `screenshot013.png`; these images were not
  changed, renamed, moved, or staged.
- The complete supplied prompt is preserved at
  `AI-Logs/Prompts/2026-11-07-paused-playback-screenshot-relative-path-correction-prompt.md`.
  Its SHA-256 matches the attached source. `AI-Logs/` is ignored.

## Evidence and correction

Neal's latest diagnostic established that Ctrl+Shift+S was recognized:
Control and Shift were observed, there was one S press edge, F12 was false,
the Tempest capture request was created, and no generic native
`screenshotNNN.png` appeared. The project file did not exist. Raylib reported
failure because Tempest passed the absolute Windows destination; Raylib 6
prefixed the process working directory and mangled the path.

The only project screenshot command remains Ctrl+Shift+S during paused visible
file playback. The correction passes Raylib a forward-slash relative path such
as `UserData/Screenshots/playback-level-2-boundary-000988.png`. .NET retains
the full intended path for directory creation, collision-safe reservation,
output verification and success reporting. The path helper rejects
destinations outside `UserData\Screenshots`. Failure messages include both
the Raylib-relative path and expected full path.

The current development root is `Directory.GetCurrentDirectory()`, explicitly
distinct from `AppContext.BaseDirectory` (the application binary root). From
the repository root, screenshots and `UserData` are beside `Temp`, not under
`src\Tempest.Game\bin\Debug\net9.0`. No published-installation root detection
was added; packaging can later provide an application data root for a
standalone build.

Screenshot and recordings directories remain lazy. A screenshot creates only
`UserData\Screenshots`, not `UserData\GameplayRecordings`. Bare recording
filenames resolve under development-root `UserData\GameplayRecordings`;
explicit relative and absolute recording paths retain their existing
semantics. Existing `Temp\GameplayRecordings` recordings were not moved or
rewritten.

The root-level `/UserData/` ignore rule excludes generated user files without
ignoring source or tracked content. Filenames retain the accepted
`playback-level-<level>-boundary-<boundary>.png` form and collision-safe
numeric suffixes. The manually accepted capture order remains gameplay draw,
PAUSED overlay draw, capture callback, then drawing completion. Capture stays
outside `PlayerInput` and does not advance simulation, boundary, cursor or RNG.

## Files and scope

The cumulative candidate has 16 source/test/documentation/configuration files:

- Production: `.gitignore`, `src/Tempest.Game/ApplicationPaths.cs`,
  `src/Tempest.Game/Debug/DebugController.cs`,
  `src/Tempest.Game/Debug/HeadlessGameplayPlaybackCli.cs`,
  `src/Tempest.Game/Debug/PlaybackScreenshotCapture.cs`,
  `src/Tempest.Game/Debug/PlaybackScreenshotKeyDiagnostic.cs`,
  `src/Tempest.Game/Program.cs`,
  `src/Tempest.Game/Rendering/VectorRenderer.cs`,
  `src/Tempest.Game/Systems/InputSystem.cs`.
- Tests: `tests/Tempest.Game.Tests/ApplicationPathsTests.cs`,
  `tests/Tempest.Game.Tests/HeadlessGameplayPlaybackTests.cs`,
  `tests/Tempest.Game.Tests/InputDiagnosticTests.cs`,
  `tests/Tempest.Game.Tests/PlaybackScreenshotCaptureTests.cs`.
- Documentation: `README.md`, `TEMPEST-BEHAVIOR-SPEC.md`,
  `TEMPEST-DEVELOPMENT-NOTES.md`.

No production code outside the existing screenshot/input/path/render-capture
seams was changed. The candidate remains bounded and internally consistent.
The tracked diff is 367 insertions and 46 deletions; the five new source/test
files add 743 lines, for an effective cumulative diff of 1,110 insertions and
46 deletions. The ignored AI prompt/report and untracked user screenshots are
not part of that diff.

No gameplay simulation, recording schema, playback-session semantics, audio,
enemy behavior, snapshot, enemy-explosion implementation, or Level 8/9 work
was added. Raylib was not rebuilt or replaced; no Windows hook or raw input
was introduced.

## Validation

- Focused screenshot/path/input/playback regressions, including Phase 1/2/3
  playback coverage: **158 passed, 0 failed, 0 skipped**.
- Full `dotnet test TempestCSharp.slnx`: **974 passed, 0 failed, 0 skipped**.
- `dotnet build TempestCSharp.slnx`: succeeded, **0 warnings, 0 errors**.
- `git diff --check`: passed; Git printed only LF-to-CRLF working-copy
  advisories.
- `git check-ignore` confirms `AI-Logs/` and repository-root `UserData/` are
  ignored.
- The game was not launched. No files were staged, committed, or pushed.

## Manual retest and rollback gate

From `C:\Users\nrnob\source\repos\Tempest`, Neal should run:

```powershell
dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\levels1-7.json --screenshot-key-diagnostic
```

Pause with Space, scrub to an identifiable boundary, and press Ctrl+Shift+S
once. Confirm the diagnostic reports Control/Shift observed, one S press edge,
F12 false, one project request, a relative Raylib path, the full repository
root `UserData\Screenshots` destination, file existence and positive length.
Verify that this directory was created on demand, exactly one boundary-aware
PNG was written, the image and PAUSED overlay remain correct, no generic
repository-root `screenshotNNN.png` appeared, and the paused boundary did not
move. Press the chord again and verify a unique suffix without changing the
first file; resume playback and confirm no capture while running and that
forward/reverse scrubbing still works.

Manual acceptance remains pending. If this isolated path correction fails to
produce a valid project-owned PNG at repository-root `UserData\Screenshots`,
stop and recommend abandoning the entire uncommitted screenshot milestone.
Do not automatically roll back.

## Final Git state

The expected committed baseline remains `50be0b88c9f8f2f1ac9229db54dc0cfa7a292f50`;
the screenshot candidate remains unstaged and uncommitted. Existing generic
screenshots through `screenshot013.png` remain untouched. No generated
`UserData` PNG is staged.
