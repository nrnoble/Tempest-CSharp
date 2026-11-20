# Reverse Recorded-Playback Scrubbing — Phase 3B Report

Date: 2026-11-07

## Starting Git state

- Branch: `master`, tracking `origin/master`.
- Starting `HEAD` and `origin/master` were both
  `a028cdc4156ea5d0fbccd660e4fc427226daa390`
  (`Add headless recorded playback runner`).
- Configured remote: `origin` at
  `https://github.com/nrnoble/Tempest-CSharp.git` for fetch and push.
- The tracked worktree and index were clean; there were no unexplained
  nonignored untracked files.
- The complete supplied task was copied verbatim to
  `AI-Logs/Prompts/2026-11-07-reverse-playback-scrubbing-phase3b-prompt.md`.
  SHA-256: `3634FDD6FF8E73265C0A1636A85883B0608236FF96A13E6AFB9027C2280A8028`.

## Reconstruction and playback behavior

- `GameplayPlaybackSession.ReconstructToBoundary` creates a fresh session
  from the existing startup configuration and replay source, runs ordinary
  recorded ticks from boundary 0, and returns only after reaching the exact
  target. It validates targets and propagates reconstruction failures.
- Reconstruction reuses the Phase 1 shared session, its production
  `RunOneStep`, input/restart gates, seed initialization order, and cursor.
  The Phase 3A headless runner and CLI were not modified.
- `VisiblePlaybackController` swaps its session only after successful
  reconstruction. `Program` rebinds the existing renderer and uses the
  signed boundary delta to keep its playback tick aligned. The existing
  Raylib window stays open.
- Boundary N means N recorded frames consumed. Left clamps to
  `max(0, N-1)`; Shift+Left clamps to `max(0, N-5)`. Boundary 0 does not
  reconstruct or wrap. At the final boundary, backward navigation works
  while the paused controller remains active.
- Left press and each native Left repeat event request one reconstruction.
  Shift+Left and each native repeat event request one five-tick group.
  No held-key polling or custom repeat timing was added. While running,
  Left does not rewind or change normal one-tick playback cadence.
- Right and Shift+Right still use ordinary sequential session steps after a
  rewind. Automated -1/+1 and -5/+5 round trips matched the tested
  `GameplayStateSummary`, including active Flipper pivot diagnostics and
  restored startup metadata. This asserts only the summary contract, not
  total internal simulation identity.
- Left/Shift+Left remain debugger controls, separate from `PlayerInput`.
  Tests compare reconstructed state with fresh replay and verify loaded
  version-1/version-2 recording bytes are unchanged.
- Reconstruction runs with `SilentAudioEvents`; after success, the new game
  is connected to the existing visible audio provider. Tests verify
  reconstruction emits no historical audio and a later ordinary visible
  step emits normal audio.
- Intermediate ticks are neither rendered nor sent through Raylib. The
  current renderer is rebound once, then draws the reconstructed boundary.
  The paused `PAUSED  boundary / duration` overlay reads the replacement
  session boundary.
- Space and final-boundary live-input handoff remain unchanged. After the
  one-way handoff to unrecorded live play, playback scrubbing no longer
  intercepts Left/Right or Space.
- Version 1 defaults (Level 1, three lives, rim speed 1.0) and version 2
  startup metadata remain covered through the existing strict loader.
  Recording format and the Phase 3A `--headless-playback`, `--boundary`,
  `--json`, and exit-code behavior are unchanged.
- No snapshots were added. Neal previously observed no perceptible delay
  during ordinary headless execution of the 12,761-tick Levels 1–7
  recording; that motivated replay-from-start before considering snapshots.
  This does not establish reverse-scrubbing responsiveness.

## Files changed

Production:

- `src/Tempest.Game/Debug/GameplayPlaybackSession.cs`
- `src/Tempest.Game/Debug/VisiblePlaybackController.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `src/Tempest.Game/Systems/InputSystem.cs`
- `src/Tempest.Game/TempestGame.cs`

Tests:

- `tests/Tempest.Game.Tests/GameplayPlaybackSessionTests.cs`
- `tests/Tempest.Game.Tests/HeadlessGameplayPlaybackTests.cs`
- `tests/Tempest.Game.Tests/InputSystemTests.cs`
- `tests/Tempest.Game.Tests/VisiblePlaybackControllerTests.cs`

Durable documentation:

- `README.md`
- `TEMPEST-BEHAVIOR-SPEC.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`

AI records:

- Prompt: `AI-Logs/Prompts/2026-11-07-reverse-playback-scrubbing-phase3b-prompt.md`
- Report: `AI-Logs/Reports/2026-11-07-reverse-playback-scrubbing-phase3b-report.md`

## Validation

- Focused new reverse-scrubbing tests: **15 passed, 0 failed, 0 skipped**.
- Focused playback/session/recording/input selection:
  **121 passed, 0 failed, 0 skipped**.
- Neighboring regressions (recording/playback, direct startup, Level 5/7,
  death/re-entry, progression, campaign soak, Flippers, Super Zapper, audio,
  and input): **632 passed, 0 failed, 0 skipped**.
- `dotnet test TempestCSharp.slnx`: **946 passed, 0 failed, 0 skipped**.
- `dotnet build TempestCSharp.slnx`: **0 warnings, 0 errors**.
- `git diff --check`: passed; Git emitted only existing LF-to-CRLF
  advisories.
- No interactive game or headless run of Neal's manual recording was
  launched by Copilot.

## Manual acceptance pending

Neal should test early, middle, and late portions of
`Temp\GameplayRecordings\levels1-7.json` with:

```powershell
dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\levels1-7.json
```

Check Space pause/resume; Right tap/hold and Shift+Right; Left tap, repeated
tap, hold/release, and Shift+Left groups; zero-boundary clamping; final-boundary
rewind; overlay updates; -1/+1 and -5/+5 visual round trips; resume from a
reconstructed boundary; no historical audio on rewind and normal audio
afterward; late-recording responsiveness; and the existing one-way live-input
handoff. Also verify there are no crashes, skipped/duplicated frames,
unexpected gameplay input, or playback-fidelity regression.

The normal direct-game launch command remains:

```powershell
dotnet run --project src\Tempest.Game -- --lives 10
```

Manual reverse-scrubbing acceptance and the snapshot responsiveness decision
remain pending. If latency is harmful, record where and how; do not add
snapshots before review.

## Final Git state and scope

- `HEAD` and `origin/master` remain
  `a028cdc4156ea5d0fbccd660e4fc427226daa390`; branch remains `master`.
- The Phase 3B candidate is unstaged. No commit or push occurred.
- Only the expected tracked source, test, and documentation files are
  modified; the prompt/report are ignored AI-Logs records. No
  nonignored untracked files or recording changes were made.
- No snapshots, recording-format changes, benchmark code, enemy-explosion
  implementation, Level 8/9 work, staging, commit, or push occurred.
- Manual acceptance is pending from Neal; this report does not claim
  reverse-scrubbing performance or arcade-behavior acceptance.
