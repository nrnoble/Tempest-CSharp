# Headless Recorded-Playback Runner — Phase 3A Report

Date: 2026-11-07

## 1. Starting Git state

- Branch: `master`, tracking `origin/master`.
- Starting `HEAD` and `origin/master` were both
  `358fca9229153e4f0fefe51fbedd711696095955`
  (`Add visible recorded playback controls`).
- The tracked worktree and index were clean; there were no unexplained
  nonignored untracked files.
- The supplied task was archived at
  `AI-Logs/Prompts/2026-11-07-headless-playback-runner-phase3a-prompt.md`.
  Its SHA-256 matches the supplied attachment.

## 2. Runner architecture

Added `HeadlessGameplayPlaybackRunner`, an in-process API that accepts a
`GameplayRecording` or recording path. It owns a private
`GameplayPlaybackSession`; it does not expose mutable game entities or create
a second replay loop.

## 3. Shared-session reuse

Every consumed frame is executed through `GameplayPlaybackSession.StepForward`,
which applies the already-accepted playback gates and invokes production
`TempestGame.RunOneStep`. The same session remains the visible playback source.

## 4. Recording loading path

The path constructor calls `GameplayRecordingFile.LoadRecording`. This keeps
strict v1/v2 parsing, validation, startup metadata and frame handling
authoritative. The CLI uses the same loader and never calls recording-save
methods.

## 5. Boundary semantics

Boundary 0 is restored startup before frame 0; consuming one frame advances
one boundary. `RunToBoundary(N)` executes sequentially until exactly N and
does not consume frame N. Negative, over-duration and backward targets are
rejected rather than clamped. An optional observer receives immutable
summaries at the starting boundary and after each sequential step; callers
can filter the callback to selected boundaries.

## 6. Completion semantics

`RunToCompletion` advances to `DurationTicks` and returns a result containing
requested/achieved boundary, duration, remaining frames, completion reason,
completion status and `GameplayStateSummary`. It does not stop early based on
the current game state.

## 7. No-live-input policy

The runner and CLI do not poll keyboard, spinner/mouse, controller or other
live input. All gameplay input comes from validated recording frames.

## 8. No-live-handoff policy

At recording completion the runner returns a result. It does not resume
interactive gameplay or consume an additional live-input tick. Visible
playback's existing handoff remains in its existing path.

## 9. State-summary output

The result reuses the immutable, bounded `GameplayStateSummary` and exposes no
live mutable entities. Existing selected values include startup/current
level, `LevelState`, score/lives, player lane/depth, Super Zapper charges,
entity counts and ordered Flipper identity/movement/pivot information.

## 10. JSON schema

The CLI serializes an explicit diagnostic DTO using camel-cased property names.
It reports requested/achieved boundary, recording duration, remaining frames,
completion reason, copied gameplay fields and stably ordered Flipper
diagnostics. It does not serialize internal object graphs. JSON goes to
stdout; human-readable output is the default.

## 11. CLI syntax

```powershell
dotnet run --project src\Tempest.Game -- --headless-playback <recording-file>
dotnet run --project src\Tempest.Game -- --headless-playback <recording-file> --boundary 4820 --json
```

The command is development-only and dispatches before the normal application
setup.

## 12. CLI parser/conflict policy

`--headless-playback <file>` must be first. Only one optional integer
`--boundary N` and one `--json` are accepted after the file. Missing values,
malformed/duplicate options, extra arguments, unknown options, simulation
overrides (`--level`, `--lives`, `--rim-speed`) and special scenarios are
rejected explicitly; the parser does not silently ignore them.

## 13. Exit-code policy

- `0`: success.
- `2`: invalid command or incompatible option.
- `3`: recording read, load or validation failure (including unsupported
  versions).
- `4`: simulation failure.
- `5`: integer boundary outside `0..DurationTicks`.

No assertion/checkpoint failure code is needed because no CLI assertion
facility was added.

## 14. Exception policy

The reusable API does not catch simulation or observer exceptions. The CLI
prints simulation exceptions using `Exception.ToString()` (type, message,
stack and inner-exception details) and returns nonzero. Boundary range checks
are performed separately from simulation execution so a gameplay
`ArgumentOutOfRangeException` is not misreported as an invalid target.

## 15. Silent-audio policy

The runner passes `SilentAudioEvents.Instance` to the production session.
Simulation events are discarded without initializing normal Raylib audio
hardware; no audio playback is required.

## 16. Raylib-independence evidence

The runner and diagnostic CLI have no Raylib calls. The executable dispatches
the headless path before `Raylib.InitWindow`, input setup, `SetTargetFPS`,
`Raylib.InitAudioDevice` and renderer construction. In-process API/CLI tests
run without a window or audio device. A real `dotnet run` smoke test produced
JSON and exited successfully without launching a game window.

## 17. Version 1 behavior

The unchanged strict loader restores v1 as Level 1, three lives and rim speed
1.0, with its recorded seed/frames. Runner tests load v1 and inspect startup
and boundary 1.

## 18. Version 2 behavior

The unchanged strict loader restores v2's explicit production level, lives
and rim-speed multiplier. Runner tests exercise Level 5/10 lives/2.5 speed;
existing strict-format tests remain in the regression selection.

## 19. Visible/headless equivalence

Tests compare fresh runner results against fresh shared-session execution at
matching boundaries, including selected summary fields and Flipper
diagnostics. No pixel comparison was attempted. Phase 2 visible controls and
live-input handoff were not modified.

## 20. VS Code Copilot workflow

The CLI supports boundary inspection as JSON. For N-1/N/N+1 comparisons, run
three fresh commands against the same recording, compare their
`stateSummary` values, then add a focused typed regression test for a confirmed
behavioral change. In-process callers may collect boundary summaries through
the read-only observer. No first-divergence command or assertion language was
added.

## 21. Checkpoint observer

Implemented as an optional `Action<int, GameplayStateSummary>` observer.
It receives boundary 0 and each reached boundary, exposes no game object and
does not catch observer exceptions. It remains a small callback seam rather
than an event framework.

## 22. File-safety behavior

The runner opens only the supplied recording through the existing loader.
It has no output-file option, sidecar behavior or recording rewrite. A CLI
test compares file bytes before and after a run and confirms they are
unchanged. No gameplay recording fixture was committed.

## 23. Production files changed

- `src/Tempest.Game/Debug/HeadlessGameplayPlaybackRunner.cs` (new).
- `src/Tempest.Game/Debug/HeadlessGameplayPlaybackCli.cs` (new).
- `src/Tempest.Game/Program.cs` (early headless dispatch).

## 24. Test files changed

- `tests/Tempest.Game.Tests/HeadlessGameplayPlaybackTests.cs` (new).

Tests cover v1/v2 startup, exact boundaries, sequential observation,
completion, out-of-range/backward rejection, deterministic session
equivalence, observer exception propagation, human/JSON CLI output, CLI
validation/conflicts, exit codes, Flipper diagnostics and read-only files.

## 25. Documentation changed

- `README.md`.
- `TEMPEST-DEVELOPMENT-NOTES.md`.
- `TEMPEST-BEHAVIOR-SPEC.md`.

The documents identify Phase 3A as an automated implementation candidate and
leave manual CLI-usability acceptance pending.

## 26. Focused test counts

`dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --filter FullyQualifiedName~HeadlessGameplayPlaybackTests`:
**26 passed, 0 failed, 0 skipped.**

## 27. Neighboring regression counts

The selected playback/gameplay regression covered the new runner, shared
session, recording formats, visible playback controls, input, direct startup,
death, progression/campaign soak, Level 5, Level 7, ledgers/cargo,
Super Zapper and audio events:
**257 passed, 0 failed, 0 skipped.** This selection includes the focused
runner tests.

## 28. Full-suite counts

`dotnet test TempestCSharp.slnx`: **931 passed, 0 failed, 0 skipped.**

## 29. Build warnings/errors

`dotnet build TempestCSharp.slnx`: **0 warnings, 0 errors.**

## 30. `git diff --check`

Passed. Git emitted the existing README LF-to-CRLF advisory only; there were
no whitespace errors.

## 31. Manual CLI commands/checklist

Use a recording Neal has reviewed or created; no specific recording is
assumed to exist:

```powershell
dotnet run --project src\Tempest.Game -- --headless-playback <recording-file>
dotnet run --project src\Tempest.Game -- --headless-playback <recording-file> --boundary 4820 --json
```

Manual CLI usability checklist:

1. Confirm no game window or gameplay audio starts.
2. Confirm completion returns promptly and reports the final boundary.
3. Confirm a requested boundary is exact.
4. Confirm JSON is readable and the level/lives/state make sense for the
   selected recording.
5. Confirm an invalid boundary produces a clear error and nonzero exit.

The real executable was smoke-tested with a temporary synthetic v2 recording;
no user recording was assumed or modified. No interactive game was launched.
Neal's manual CLI-usability check remains pending.

## 32. Phase 3B roadmap

Backward stepping remains deferred. The intended initial approach is
reconstructing from recorded startup and replaying sequentially to the earlier
boundary: Left tap/hold = -1 tick and Shift+Left tap/hold = -5 ticks. Measure
replay-from-start costs before considering snapshots. No rewind, snapshots,
reverse mutation, RNG-state serialization, seek UI or speed-control work was
added.

## 33. Final Git state

`git status --short --branch` reports `## master...origin/master`, with only
the four modified tracked files and three new untracked source/test files
listed below. `HEAD` and `origin/master` remain at
`358fca9229153e4f0fefe51fbedd711696095955`.

Modified:

- `README.md`
- `TEMPEST-BEHAVIOR-SPEC.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `src/Tempest.Game/Program.cs`

Untracked:

- `src/Tempest.Game/Debug/HeadlessGameplayPlaybackCli.cs`
- `src/Tempest.Game/Debug/HeadlessGameplayPlaybackRunner.cs`
- `tests/Tempest.Game.Tests/HeadlessGameplayPlaybackTests.cs`

No staged changes or nonignored untracked files remain; the prompt/report
records are ignored under `AI-Logs/`. `git diff --check` passes with only the
existing README line-ending advisory. No commit or push was made.

## 34. Scope confirmation

No backward step, rewind, snapshots, recording-format change, visible-control
change, enemy-explosion implementation, Level 8/9 work, staging, commit or
push occurred. The candidate is ready for Neal's review.
