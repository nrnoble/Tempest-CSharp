# Level 7 framebuffer readback instrumentation

Date: 2026-11-17
Status: **IMPLEMENTATION COMPLETE; MANUAL CAPTURE AND ACCEPTANCE PENDING**

## Result

Added an opt-in, development-only readback of a bounded set of actual Level 7
production framebuffer images. The diagnostic observes the Raylib screen
buffer after `VectorRenderer` has rendered the production scene and overlays,
and before `Raylib.EndDrawing` swaps the frame. It does not reconstruct or
redraw any entity.

The capture option requires both `--atari-presentation` and the existing
`--level7-visible-runtime-diagnostic` trace. With the new option disabled,
the existing render callback and trace format remain unchanged. No gameplay,
projection, camera, entity, speed, timing, recording, or playback behavior was
corrected or tuned.

## Readback feasibility and lifecycle

The project references Raylib-cs 8.1.0. Its installed API documentation
describes `Raylib.LoadImageFromScreen()` as loading an image from the screen
buffer; `Raylib.ExportImage(image, path)` exports that image. Both APIs compile
against the project dependency.

`Program` uses the existing `VectorRenderer.Draw` `beforeEndDrawing` callback.
That callback runs after scene drawing, `EndMode2D`, and the portrait overlays,
immediately before `Raylib.EndDrawing`. When the new option is enabled, it
calls `LoadImageFromScreen`, exports that exact image as PNG, and unloads the
CPU image. This is production-path raster output, not a second render or a
projection of submitted coordinates. It observes the screen buffer at this
point only; it does not observe post-swap composition, monitor refresh, or
scanout.

## Selection, bounds, and output

- The diagnostic waits for Level 7 Playing frames that are not paused or
  state-transition frames.
- A candidate must be the same runtime Tanker identity, remain `Climbing` in
  the same lane and input mode over adjacent render frames, advance by a
  positive simulation tick, and have increasing gameplay height. Spawns,
  gaps, pauses, transitions, and other discontinuities do not qualify.
- It selects depth bands in order: deep (`0.8–1.0`), middle (`>0.2–<0.8`),
  and near-rim (`0.0–0.2`). Each burst follows one Tanker for up to eight
  consecutive eligible frames while it remains in that band.
- Hard limits: **3 burst attempts**, **8 frames per burst**, **24 framebuffer
  images total**, **8 MiB per PNG**, and **128 MiB total retained PNG data**.
  An interrupted burst consumes its band attempt and is not retried. A PNG
  that exceeds a byte limit is removed and capture stops. The byte limit is
  checked after export; thus a single in-progress export can temporarily add
  one image before it is rejected.
- Images are lossless PNG files under
  `TEMP\Diagnostics\Level7Framebuffer\<run-name>\`. The new option accepts a
  `.jsonl` run name; its stem names the capture folder. The folder must not
  already exist. The existing runtime JSONL also uses `CreateNew`. Use unique
  names for subsequent runs; neither output is overwritten.
- Per-image capture metadata is added directly to that image's corresponding
  frame record in the existing visible-runtime JSONL. It contains the output
  folder and PNG name, burst/band/sample number, render-frame sequence,
  simulation tick, render-start timestamp, and image dimensions/format/size.
  The linked Tanker metadata includes runtime ID, lane, state, height,
  normalized depth, mapped anchor, camera-transformed anchor, and logical and
  host submitted-geometry bounds. The same frame record retains full render
  timing, including duration through `EndDrawing`; capture metadata separately
  records readback, PNG-write, and capture durations.
- Player shots and the Blaster remain naturally visible in any selected full
  frame; their existing synchronized submitted-geometry records are retained.
  No shot-triggered burst or special Blaster raster tracker was added.

When all three bursts complete, the console prints a
`Level 7 framebuffer diagnostic capture complete` message with counts and
output path, followed by confirmation that no further framebuffer readback
will occur. Neal may continue playing; the existing visible-runtime JSONL
continues according to its existing behavior. If a burst is interrupted or a
disk/write limit stops capture, the console reports an incomplete/stopped
result instead.

## Observer effect and limitations

Screen-buffer readback can synchronize GPU/CPU work, and PNG export is
synchronous in the pre-swap callback. Either can stall a rendered frame. The
diagnostic records readback and write durations plus the existing full
render-call duration so the manual run can expose material stalls. No claim is
made that this instrumentation is timing-transparent; if capture stalls are
substantial, the resulting motion timing must not be attributed to the
original regression without accounting for that observer effect.

Automated tests use injected image/read/write delegates to validate selection,
limits, serialization, and correlation without launching Raylib. The game
build verifies the actual Raylib API calls compile, but no interactive native
readback was performed here. The produced production PNGs have not been
generated or visually inspected. Actual rasterized-motion analysis remains a
separate task after Neal confirms the regression was visibly occurring during
the capture.

## Changed files

- `src/Tempest.Game/Debug/Level7FramebufferDiagnostic.cs` — bounded image
  readback, burst selection, PNG writing, and capture timing.
- `src/Tempest.Game/Debug/Level7VisibleRuntimeDiagnostic.cs` — synchronized
  Tanker candidate selection and optional per-frame capture metadata.
- `src/Tempest.Game/Debug/DebugController.cs` — opt-in option and dependency
  validation.
- `src/Tempest.Game/Program.cs` — pre-swap callback integration and output
  setup.
- `tests/Tempest.Game.Tests/Level7FramebufferDiagnosticTests.cs` — bounded
  burst, metadata-correlation, and per-image disk-limit tests.
- `tests/Tempest.Game.Tests/Level7VisibleRuntimeDiagnosticTests.cs` — option
  validation and disabled-mode trace checks.
- `AI-Logs/Prompts/2026-11-17-level7-framebuffer-readback-instrumentation-prompt.md`
  — saved substantive task prompt.

No durable behavior documentation was changed. No recordings, existing
diagnostic captures, user images/videos, ROM copies, or `TEMP\myNotes.md` were
modified or inspected. No visual analysis was performed.

## Validation

- **PASS** — Release game build:
  `dotnet build src\Tempest.Game\Tempest.Game.csproj --no-restore --configuration Release --verbosity minimal`
  (0 warnings, 0 errors).
- **PASS** — 157 targeted Level 7 framebuffer/runtime, CLI, presentation, and
  gameplay-recording tests:
  `dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --configuration Release --no-restore --filter "FullyQualifiedName~Level7FramebufferDiagnosticTests|FullyQualifiedName~Level7VisibleRuntimeDiagnosticTests|FullyQualifiedName~DebugControllerTests|FullyQualifiedName~VectorRendererAtariPresentationTests|FullyQualifiedName~AtariLevel7EntityPresentationTests|FullyQualifiedName~GameplayRecordingTests" --blame-hang-timeout 3m --verbosity minimal`
- **PASS** — New option parsing/context, diagnostics-disabled trace behavior,
  maximum 3-by-8 capture count, no readback after completion, per-image byte
  limit handling, metadata/entity correlation, and timing serialization are
  covered by tests. Automated image delegates generated only temporary test
  files; no production framebuffer output was created.
- **PASS** — `git diff --check`. Git printed only the pre-existing LF-to-CRLF
  notices for `LevelLoader.cs` and `InputSystem.cs`.
- **PASS** — `git check-ignore -v` confirmed
  `TEMP\Diagnostics\Level7Framebuffer\...` is ignored.
- **BLOCKED, alternate route PASS** — An initial Debug-config test build could
  not replace `src\Tempest.Game\bin\Debug\net9.0\Tempest.Game.exe` because it
  was locked by an already-running `Tempest.Game` process (PID 104920; MSB3027
  / MSB3021). That process was not started or stopped by this task. The
  bounded Release test run above completed successfully instead.
- The interactive game was not launched. Manual capture and Neal's visual
  confirmation remain **PENDING**.

## Manual handoff

Run this exact command for the known-bad manual condition:

```powershell
dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --level 7 --atari-presentation --level7-visible-runtime-diagnostic level7-manual-framebuffer-20261117.jsonl --level7-framebuffer-diagnostic level7-manual-framebuffer-20261117.jsonl
```

The synchronized trace will be written to
`TEMP\Diagnostics\Level7VisibleRuntime\level7-manual-framebuffer-20261117.jsonl`.
PNG samples will be written to
`TEMP\Diagnostics\Level7Framebuffer\level7-manual-framebuffer-20261117\`.
Use new names if either destination already exists. The `.jsonl` argument to
`--level7-framebuffer-diagnostic` names the folder; image metadata is embedded
in the existing visible-runtime trace rather than written to a second
unsynchronized log.

Neal should play Level 7, confirm whether the known depth-motion regression
was visibly present during the run, and preserve both the runtime JSONL and
the entire framebuffer folder. Report whether the capture-complete or
incomplete message appeared and whether any capture stall was noticeable.
Neal does not need to inspect the images.

## Git preservation and scope

Initial state: branch `master`, HEAD
`385f603a94391756b2ca6cccd524dc17b281e782`, tracking `origin/master`;
empty index; 17 modified tracked paths and 17 nonignored untracked paths.
Final state: same branch, HEAD, and upstream; empty index; the same 17
pre-existing modified tracked paths and 19 nonignored untracked paths. The two
additional untracked paths are this milestone's framebuffer diagnostic and
test; the pre-existing untracked Level 7 diagnostic files were preserved.
The prompt and report are ignored under `AI-Logs/`.

No file was staged, committed, pushed, reset, restored, stashed, rebased,
cleaned, or checked out. No behavior correction or visual/gameplay change was
made. No suspected cause was established.

Completion token: `TEMPEST_LEVEL7_FRAMEBUFFER_READBACK_INSTRUMENTATION`
