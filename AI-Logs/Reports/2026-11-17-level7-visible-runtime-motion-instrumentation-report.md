# Level 7 visible-runtime motion instrumentation

Date: 2026-11-17
Status: **IMPLEMENTATION COMPLETE; MANUAL DIAGNOSTIC RUNS PENDING**

## Outcome

Added an opt-in, development-only JSONL trace for the current Level 7
`--atari-presentation` runtime. It records the actual per-entity geometry
submitted by `VectorRenderer` during Level 7 Playing, alongside simulation
ticks, render-call timing, mode, state, and mapped presentation data. It does
not alter simulation, playback data, or gameplay tuning.

The trace supports a side-by-side comparison of a Level 7 recorded playback
and current manual Level 7 play. It does not explain or correct the observed
motion regression.

## Instrumentation architecture

- Added `Level7VisibleRuntimeDiagnostic` in
  `src/Tempest.Game/Debug/Level7VisibleRuntimeDiagnostic.cs`.
- Added `--level7-visible-runtime-diagnostic <new-file-name.jsonl>`.
  The switch requires interactive `--atari-presentation` rendering and a
  Level 7 production start or Level 7 recorded playback. It rejects output
  paths (only a filename is accepted), existing destination files, gameplay
  speed/calibration overrides, and incompatible diagnostic/test modes.
- Output is created under the ignored
  `TEMP\Diagnostics\Level7VisibleRuntime\` directory. Each run is capped at
  60,000 Level 7 Playing render frames; a limit record and console message
  make truncation explicit.
- Runtime entity IDs are assigned by object identity inside the diagnostic;
  no IDs or other state are written to gameplay objects.

## Exact stages and recorded values

`VectorRenderer` records the coordinates it actually passes to the drawing
operations for the Blaster, Flippers, Tankers, Spikers, Eggs, player shots,
and visible enemy bullets. The trace retains each entity's runtime ID, type,
state, lane, gameplay height, normalized mapped depth, mapped anchor,
presentation scale, glyph scale, optional Spiker rotation, and bounds of the
submitted glyph geometry.

Logical geometry bounds come from the line endpoints, point positions, or
rectangle coordinates submitted during that render call. Camera-transformed
anchor positions and geometry bounds are diagnostic calculations using the
active Raylib `Camera2D`; they are reported in Raylib window-pixel coordinates.
They are not framebuffer readback, rasterized extrema, physical-monitor
pixels, or scanout measurements.

Each Level 7 Playing frame records:

- render-frame sequence, current simulation tick, recorded-playback boundary
  and their deltas from the previous Level 7 render frame;
- per-frame input mode (`manualGameplay`, `manualGameplayRecording`,
  `recordedPlayback`, or `scriptedPlayback`) and pause state;
- monotonic `Stopwatch` time before rendering and immediately after
  `EndDrawing` returns, elapsed time since the preceding Level 7 render call,
  and render-call duration;
- each entity's submitted geometry, anchor displacement, frame/tick deltas,
  elapsed observation time, displacement rate per elapsed second and per
  simulation tick, plus lifecycle/state/lane events.

`fixedLaneContinuous` is true only for adjacent render observations in
Playing, with no pause, a positive simulation-tick delta, and unchanged
entity state and lane. Lane/state transitions, gaps, spawn/removal, pause,
and Level 7 state transitions remain explicit so they cannot be mistaken for
continuous depth motion.

Only Level 7 Playing frames are recorded continuously. Other Level 7 states
produce sparse transition markers; leaving Level 7 produces one scope-end
marker. The `EndDrawing` return timestamp is the latest presentation timing
available here. Actual framebuffer contents, monitor presentation, and
scanout remain unobserved.

## Commands for Neal

The existing control recording identified by the preceding read-only report
is present at `TEMP\GameplayRecordings\level7-candidate-6.json`. Its contents
were not inspected or modified for this task.

**A. Known-good recorded playback**

```powershell
dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --playback-file TEMP\GameplayRecordings\level7-candidate-6.json --atari-presentation --level7-visible-runtime-diagnostic level7-control-20261117.jsonl
```

Output: `TEMP\Diagnostics\Level7VisibleRuntime\level7-control-20261117.jsonl`

**B. Normal manual Level 7 gameplay**

```powershell
dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --level 7 --atari-presentation --level7-visible-runtime-diagnostic level7-manual-20261117.jsonl
```

Output: `TEMP\Diagnostics\Level7VisibleRuntime\level7-manual-20261117.jsonl`

The destination uses `CreateNew`; choose a different filename for another
run rather than overwriting a diagnostic. Neal only needs to watch the control
playback and confirm it still looks correct, then play normal Level 7 and
confirm the regression is visible. Preserve both resulting logs; no number
analysis is requested during gameplay.

## Validation

- Release game-project build:
  `dotnet build src\Tempest.Game\Tempest.Game.csproj --no-restore --configuration Release --verbosity minimal`
  — **PASS**, 0 warnings, 0 errors.
- Targeted diagnostic, CLI, Level 7 presentation, player-shot, recording and
  playback tests — **PASS**, 212 passed, 0 failed. Command:
  `dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~Level7VisibleRuntimeDiagnosticTests|FullyQualifiedName~DebugControllerTests|FullyQualifiedName~AtariLevel7EntityPresentationTests|FullyQualifiedName~VectorRendererAtariPresentationTests|FullyQualifiedName~PlayerShotRenderingTests|FullyQualifiedName~GameplayRecordingTests|FullyQualifiedName~GameplayPlayback" --blame-hang-timeout 3m --verbosity minimal`
- Diagnostic serialization and exact player-shot draw-argument capture were
  exercised by tests using a temporary JSONL output; the tests removed their
  temporary directory.
- The runtime switch parser, filename validation, playback-option composition,
  state-transition records, geometry bounds, identity continuity, and
  fixed-lane sample validity were covered by tests.
- `git diff --check` — **PASS**. Git emitted only the pre-existing LF-to-CRLF
  notices for `LevelLoader.cs` and `InputSystem.cs`.
- `git check-ignore -v TEMP\Diagnostics\Level7VisibleRuntime\...` confirmed
  the output location is ignored.
- The interactive game was not launched. Consequently, no actual-runtime
  diagnostic file or visual comparison was produced in this implementation
  task; Neal's two visible runs remain **PENDING**.

Diagnostics are opt-in. When the option is omitted, no diagnostic file or
frame metadata is created, and gameplay/simulation/playback code is unchanged.
Existing Level 7 rendering, shot, recording, and playback regressions passed.

## Scope, limitations, and acceptance

- No behavior correction, calibration, timing change, geometry change, or
  gameplay change was made. No suspected cause was established.
- No recording was modified; no screenshot, video, visual asset,
  `TEMP\myNotes.md`, or protected ROM copy was inspected.
- The direct renderer trace can establish submitted geometry and correlate it
  with state and time, but cannot observe work performed after
  `EndDrawing` returns. If the recorded coordinates and timing remain constant
  while Neal confirms visible depth-dependent motion, that contradiction and
  the unobserved downstream presentation stage must be retained for the next
  investigation.
- **Manual diagnostic status: PENDING.** After both runs, Neal should provide
  the preserved JSONL files for the separate analysis milestone.

## Git preservation

Initial state: branch `master`, HEAD
`385f603a94391756b2ca6cccd524dc17b281e782`, tracking `origin/master`; empty
index; 17 modified tracked paths; 17 nonignored untracked paths. The working
tree already contained substantial presentation changes. The touched tracked
files (`Program.cs`, `DebugController.cs`, and `VectorRenderer.cs`) were
already modified before this task; those edits were preserved.

Task changes are limited to those three existing dirty files, the new
`Level7VisibleRuntimeDiagnostic.cs` implementation, its new test file, and
these ignored prompt/report records. No file was staged, committed, pushed,
reset, restored, stashed, rebased, cleaned, or checked out. Final branch,
HEAD, and upstream remain unchanged; the index is still empty. The same 17
tracked paths remain modified, and the 19 nonignored untracked paths comprise
the 17 pre-existing paths plus the new diagnostic implementation and test.
The prompt and report are ignored under `AI-Logs/`. No pre-existing untracked
file was modified.

Prompt: `AI-Logs/Prompts/2026-11-17-level7-visible-runtime-motion-instrumentation-prompt.md`
Report: `AI-Logs/Reports/2026-11-17-level7-visible-runtime-motion-instrumentation-report.md`

Completion token: `TEMPEST_LEVEL7_VISIBLE_RUNTIME_MOTION_INSTRUMENTATION`
