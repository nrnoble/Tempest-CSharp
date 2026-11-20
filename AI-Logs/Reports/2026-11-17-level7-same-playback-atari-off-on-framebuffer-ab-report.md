# Level 7 same-playback Atari OFF/ON framebuffer A/B preparation

Date: 2026-11-17
Status: **IMPLEMENTATION COMPLETE; MANUAL A/B CAPTURE PENDING**

## Result

The existing Level 7 framebuffer readback was safely reused. Recorded playback
now selects short image windows by deterministic playback boundary, and the
same framebuffer diagnostic supports Atari OFF and Atari ON. The controlled
run captures production pixels through the existing Raylib screen-buffer
readback, and the synchronized runtime trace retains the submitted entity
coordinates for only those selected windows.

No simulation, recording, playback, presentation, geometry, projection, or
timing behavior was changed. The legacy Atari-on manual depth-band capture
remains available. No interactive game was launched; no production PNG,
screenshot, video, or other user visual asset was opened or analyzed.

## Direct answers

1. **Can the existing diagnostic be reused?** Yes. Its production-path
   `LoadImageFromScreen` readback and lossless PNG export remain unchanged.
   The former depth-band/Tanker selection remains for manual runs; recorded
   file playback selects equivalent frames by playback boundary instead.

2. **Minimal changes:** added the boundary-window selector and per-image mode,
   playback-path, boundary, simulation-tick, level/state, render-sequence, and
   capture-timing metadata; allowed Atari OFF only for interactive recorded
   playback with both diagnostics; retained exact renderer-submitted and
   camera-transformed vertices for the selected playback frames. OFF records
   keep Atari-only mapped depth/anchor/scale fields null and use the existing
   non-Atari renderer. With framebuffer capture disabled, the previous
   visible-runtime trace schema and selection behavior remain unchanged.

3. **Selected boundaries/windows:** centers 100, 150, 200, 250, and 300; the
   three consecutive boundaries around each are:
   - 99, 100, 101
   - 149, 150, 151
   - 199, 200, 201
   - 249, 250, 251
   - 299, 300, 301

4. **Hard capture bounds:** at most 15 PNGs per run; each retained PNG is
   limited to 8 MiB; total retained framebuffer PNG data is limited to
   120 MiB. The PNG quota is checked after export, so one in-progress oversized
   export can temporarily exist before it is deleted. The correlation JSONL
   is separate from the PNG quota and records only these selected playback
   frames, plus run/transition/end records.

5. **Synchronization:** `GameplayPlaybackSession.CurrentBoundary` is the
   primary selection key. A boundary is captured at most once; render-frame
   sequence and simulation tick are retained as correlation values. If a
   selected boundary is skipped or the run ends before reaching it, that
   boundary is listed as missed and no unrelated frame is substituted. Any
   missed boundary makes the completion status incomplete.

6. **Renderer geometry/state:** for the tracked entities already observed by
   the runtime diagnostic (Tanker, enemy bullet, Spiker, Egg, player shot,
   Flipper, and Blaster), the selected-window trace retains entity ID/type,
   state, lane, gameplay height, projected depth, submitted primitive count,
   exact submitted coordinates, camera-transformed coordinates, and geometry
   bounds. Atari ON additionally retains normalized Atari depth, mapped
   anchor, and local presentation scale. These Atari-only fields are null in
   OFF mode rather than synthesized. The capture record links to the frame's
   entity records by render-frame sequence. Untracked renderer elements remain
   outside the existing per-entity trace.

7. **Actual framebuffer readback:** the production `VectorRenderer.Draw`
   callback calls `Raylib.LoadImageFromScreen()` after production scene and
   overlays have rendered and before `Raylib.EndDrawing()` swaps the frame.
   `Raylib.ExportImage()` writes that readback as PNG, then the CPU image is
   unloaded. No reconstructed scene, second render, or synthetic pixels are
   used.

8. **Observer-effect timing:** each image record includes Stopwatch readback,
   PNG-write, and total capture durations. The linked frame record includes
   render start/present timestamps, elapsed time, and normal render-call
   duration. Both modes use the same capture policy; no timing equivalence is
   claimed.

9. **Automated validation:**
   - **PASS** — Release game build, 0 warnings and 0 errors:
     `dotnet build src\Tempest.Game\Tempest.Game.csproj --configuration Release --no-restore --verbosity minimal`
   - **PASS** — 284 focused tests covering framebuffer/runtime diagnostics,
     both Atari-mode configurations, CLI validation, renderer geometry,
     recording/playback, Level 7 production, and relevant presentation tests.
   - **PASS** — tests assert identical selected boundary and render-frame
     sequences in the OFF/ON configurations, one image per boundary, 15-image
     and 120-MiB PNG limits, missed-boundary reporting, metadata correlation,
     Atari-only metadata omission in OFF, and disabled-diagnostic trace shape.
   - **PASS** — existing-output rejection and oversized-image cleanup tests
     continue to pass.
   - **PASS** — `git diff --check`.
   - **PASS** — `git check-ignore -v` confirmed both diagnostic output roots
     and `AI-Logs/` are ignored.
   - **NOT RUN BY DESIGN** — interactive command runs and native framebuffer
     readback; Neal will perform the visible A/B. Tests used injected image
     delegates and temporary test files only.

10. **Commands for Neal:**

    **COMMAND A — Atari OFF**

    ```powershell
    dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --playback-file TEMP\GameplayRecordings\Level-7-slowmo.json --level7-visible-runtime-diagnostic level7-same-playback-atari-off-20261117.jsonl --level7-framebuffer-diagnostic level7-same-playback-atari-off-20261117.jsonl
    ```

    **COMMAND B — Atari ON**

    ```powershell
    dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --playback-file TEMP\GameplayRecordings\Level-7-slowmo.json --atari-presentation --level7-visible-runtime-diagnostic level7-same-playback-atari-on-20261117.jsonl --level7-framebuffer-diagnostic level7-same-playback-atari-on-20261117.jsonl
    ```

11. **Manual confirmation:** run OFF first, then ON. The order is recommended
    for a control-first review but is not required for boundary synchronization.
    During OFF playback, confirm the normal non-Atari presentation. During ON
    playback, confirm whether the known motion/presentation problem is visible.
    Do not open or compare the PNGs during this step. If the visible difference
    is obvious, report that observation afterward.

12. **Preserve for the later read-only analysis:**
    - `TEMP\Diagnostics\Level7VisibleRuntime\level7-same-playback-atari-off-20261117.jsonl`
    - `TEMP\Diagnostics\Level7Framebuffer\level7-same-playback-atari-off-20261117\`
    - `TEMP\Diagnostics\Level7VisibleRuntime\level7-same-playback-atari-on-20261117.jsonl`
    - `TEMP\Diagnostics\Level7Framebuffer\level7-same-playback-atari-on-20261117\`

    PNG names include the playback boundary and render-frame sequence, for
    example `boundary-00000099-frame-XXXXXXXX.png`. If an output destination
    already exists, the program refuses to overwrite it; choose a new matching
    run name for both diagnostic options.

## Capture completion and behavior

After boundary 301 is captured, the console reports
`Level 7 framebuffer diagnostic capture complete`, image/byte totals, and the
output directory. No further framebuffer readback occurs; recorded playback
and gameplay continue normally. If a boundary was missed or capture fails,
the console reports an incomplete/stopped result and missed boundaries or the
explicit error.

## Files touched

- `src/Tempest.Game/Debug/DebugController.cs`
- `src/Tempest.Game/Debug/Level7FramebufferDiagnostic.cs`
- `src/Tempest.Game/Debug/Level7VisibleRuntimeDiagnostic.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `tests/Tempest.Game.Tests/Level7FramebufferDiagnosticTests.cs`
- `tests/Tempest.Game.Tests/Level7VisibleRuntimeDiagnosticTests.cs`
- `AI-Logs/Prompts/2026-11-17-level7-same-playback-atari-off-on-framebuffer-ab-prompt.md`

No durable behavior documentation was changed. Existing recordings,
diagnostics, user visual assets, and protected reference material were not
modified. No files were staged or committed.

## Git preservation

Starting and final branch: `master`; HEAD:
`385f603a94391756b2ca6cccd524dc17b281e782`; upstream: `origin/master`.
The index remained empty. The substantial pre-existing tracked modifications
and untracked files remained present; no Git history or worktree preservation
operation was performed.

Manual A/B capture and visual acceptance remain **PENDING**.

TEMPEST_LEVEL7_SAME_PLAYBACK_ATARI_OFF_ON_FRAMEBUFFER_AB
