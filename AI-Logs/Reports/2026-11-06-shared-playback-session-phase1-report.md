# Shared Deterministic Playback Session — Phase 1 Report

Date: 2026-11-07

## 1. Starting Git state

- Repository: `C:\Users\nrnob\source\repos\Tempest`.
- Branch: `master`, tracking `origin/master`.
- Starting `HEAD` and `origin/master`:
  `7032a7219cde69a0ae25b00e5f3bc9ac1ee3ed04`
  (`Use vector text for Avoid Spikes`).
- Initial tracked worktree and index were clean; there were no unexplained
  nonignored untracked files. The expected baseline matched exactly.
- The complete supplied task was copied to
  `AI-Logs/Prompts/2026-11-06-shared-playback-session-phase1-prompt.md`;
  source and destination SHA-256 values matched.

## 2. Existing playback orchestration

`Program.cs` loaded a file or seeded profile, owned a local simulation tick,
selected replay/live input, applied state-specific input and restart gates,
called `TempestGame.RunOneStep` once, advanced the tick and rendered. At file
completion, the existing visible loop resumed live controls. `PlayerInputReplay`
provides a tick-indexed frame source and treats omitted in-memory frames as
neutral input.

## 3. Shared-session design

Added `GameplayPlaybackSession`, constructed from a validated
`GameplayStartupConfiguration` and `PlayerInputReplay`. It owns the normal
production `TempestGame`, replay source, boundary/cursor, duration and
completion state. Startup seed/production-level consistency is checked before
game construction; the existing seeded production initializer is reused.

## 4. Boundary semantics

Boundary 0 is the restored startup state before any recorded input. Recorded
frame 0 is consumed to produce boundary 1; consuming frame N produces N+1.
After D frames, boundary D is the state after all recorded frames. The
contract appears in the API documentation, project documentation and tests.

## 5. Input-gating ownership

The session centralizes recorded playback's game-over/completion restart gate
and death/exit movement gates in `ApplyPlaybackInputGates`. `Program.cs` uses
the same helper for visible file/profile playback. Ordinary live-input handling
remains in `Program.cs` and was not otherwise changed.

## 6. `StepForward`

One successful step fetches the next indexed frame, applies the shared gates,
passes its accepted `PlayerInput` and restart bit to `TempestGame.RunOneStep`
exactly once, then advances the boundary/cursor exactly once. It returns
`false` without simulation when already complete. It does not render, poll
controls, pace in real time, or call Raylib.

## 7. `RunToBoundary`

`RunToBoundary(N)` accepts only `0 <= N <= DurationTicks`, advances through
`StepForward`, and returns a summary at N. A current-boundary request is a
no-op. Negative/out-of-range targets throw `ArgumentOutOfRangeException`;
earlier targets throw `InvalidOperationException` because backward
reconstruction is not implemented.

## 8. Completion semantics

At `DurationTicks`, the session reports completion, zero remaining frames and
the cursor at the duration. `StepForward` cannot consume a nonexistent frame.
The session does not introduce live input or an automatic handoff; that remains
the visible application's responsibility.

## 9. State-summary schema

Added immutable `GameplayStateSummary`. It copies boundary, duration, next
frame/cursor, remaining frames and completion; recorded starting level/lives/
rim speed/seed; current displayed level and `LevelState`; score, lives, player
lane/fraction/depth, Super Zapper charges; and counts of shots, enemy bullets,
Eggs, Flippers, Tankers, Spikers, Spikes, Fuseballs and Pulsars. The summary
exposes no mutable game entities.

## 10. Stable ordering

Flipper summaries sort by cargo/original identity, lane, height, movement
state, direction, pivot endpoints and turn angle. No entity snapshot relies on
the incidental mutable Flipper list order.

## 11. Flipper diagnostic fields

Each immutable Flipper subrecord includes original slot/cargo IDs, cargo
classification, lane, height, movement state, direction, rim-pursuit direction,
pivot source/destination lanes, current/total turn angle and normalized pivot
progress. The Level 5 production test compares these values with controlled
live entities, including their direction values.

## 12. Raylib independence

The session and summary call only the existing `TempestGame`/replay APIs.
Neither initializes nor references Raylib. The focused tests construct and run
sessions without a window.

## 13. Audio behavior

The session accepts an optional existing `IAudioEvents` adapter and passes it
to `TempestGame`. Omitting it retains `TempestGame`'s existing
`SilentAudioEvents` default. No Raylib audio device is initialized or audio
system redesigned.

## 14. Version 1/2 preservation

No recording schema or loader change was made. Tests load both v1 and v2
fixtures through the existing strict loader, then run them through the session,
checking version-specific startup defaults/metadata, seed, full input fields,
restart-bit frame data and production game equivalence.

## 15. Visible behavior preservation

Only the recorded-input gate was shared with the visible loop. Rendering,
real-time loop/cursor, recording capture, completion reporting and the existing
live-input handoff remain in `Program.cs`. No visual playback controls were
added or manually accepted; no manual visual acceptance was required for this
headless seam.

## 16. Production files changed

- Added `src/Tempest.Game/Debug/GameplayPlaybackSession.cs`.
- Added `src/Tempest.Game/Debug/GameplayStateSummary.cs`.
- Updated `src/Tempest.Game/Program.cs` to use shared playback input gates.

## 17. Test files changed

- Added `tests/Tempest.Game.Tests/GameplayPlaybackSessionTests.cs`.
- Updated `tests/Tempest.Game.Tests/GameplayRecordingTests.cs` for session
  coverage of both recording versions.

## 18. Documentation changed

- `README.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `TEMPEST-BEHAVIOR-SPEC.md`

The docs identify this as an implementation candidate, state the boundary and
summary contracts, record future controls/headless-Copilot use, and explicitly
state that those future capabilities are not implemented.

## 19. Focused shared-session tests

`GameplayPlaybackSessionTests` plus
`SharedPlaybackSessionPreservesVersionedStartupAndFrameSemantics`:
**13 passed, 0 failed, 0 skipped**.

## 20. Neighboring regression tests

Playback/recording, direct startup, Levels 5/7, production progression,
campaign soak, death/re-entry, Super Zapper, rim pursuit and input-gating
selection (excluding the focused session tests): **198 passed, 0 failed,
0 skipped**.

## 21. Full solution tests

`dotnet test TempestCSharp.slnx`: **889 passed, 0 failed, 0 skipped**.

## 22. Build warnings/errors

`dotnet build TempestCSharp.slnx`: **0 warnings, 0 errors**.

## 23. Whitespace validation

`git diff --check`: passed. Git emitted only its existing-style README line
ending advisory (`LF` may be replaced by `CRLF`); no whitespace error occurred.

## 24. Future Phase 2/3 roadmap

- **Phase 2:** migrate visible recorded-file playback onto the shared session,
  preserve its rendering/live-input handoff, then add the explicitly requested
  pause/resume and forward-step debugger controls.
- **Phase 3:** add backward reconstruction by deterministic replay from the
  recording start, then a thin headless runner/CLI for VS Code Copilot to run
  to boundaries and inspect summaries. Fast-forward/rewind can follow without
  changing simulation tick size.

## 25. Final Git state

- `HEAD` and `origin/master` remain
  `7032a7219cde69a0ae25b00e5f3bc9ac1ee3ed04`.
- Branch remains `master`; the index is clean.
- Expected implementation and documentation edits remain unstaged in the
  worktree. The prompt/report records are local AI logs.

## 26. Scope confirmation

No backward stepping/reconstruction, rewind, snapshots, visible debugger
controls, headless CLI, Level 8/9 work or unrelated gameplay change occurred.
No recording-format change, interactive launch, staging, commit or push
occurred. Stop for Neal's review before proceeding.
