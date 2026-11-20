# Blaster Exit/Approach Scale Correction

Date: 2026-11-17

## Status

**IMPLEMENTED — automated validation PASS; manual acceptance PENDING.**
The shared renderer now sizes travel-state Blaster artwork using its active
presentation adapter's local scale relative to a stable gameplay-rim
projection. No gameplay or camera simulation was changed. No interactive game
was launched.

## Correction

For Atari-presentation routes during exit travel, Approach, and ReverseZoom:

```text
BlasterHeight =
    20 * CurrentAdapterLocalScale(
        PlayerProjectedDepth,
        active Approach offsets,
        active K and ZOffset)
       / StableGameplayRimAdapterLocalScale(
        Level.Position,
        no Approach offsets,
        K = 70,
        ZOffset = 5)
```

The stable camera values come from the existing
`Projection.PortraitPerspectiveBaselineK` and
`Projection.PortraitPerspectiveBaselineZOffset` constants. Both scales are
obtained from the current level's presentation adapter, including metadata
backed routes and the Level 5/6 axis-specific adapters. Explicit camera
parameters let the same adapters calculate the baseline without mutating the
live exit camera.

The Atari renderer keeps exactly 20 units when the player is on the rim outside
Approach, including ordinary `Playing` and `ExitDelay`. Approach evaluates its
live offset even though the player is registered at the distant rim; it
therefore starts small and reaches exactly 20 when the offset reaches zero.
Option-off and legacy calculations remain unchanged.

Merely swapping the previous `nearScale / playerScale` operands would still
compare against a near-rim scale calculated from live exit `K`/`ZOffset`.
This implementation instead evaluates the baseline under the stable production
camera through the same adapter route.

## Numerical checks

Level 1 deterministic ExitZoom samples use the existing production cadence:

| Sample | Camera / depth state | Old reversed height | Corrected height |
|---|---|---:|---:|
| Exit tick 23 | K=70, ZOffset=-41, projected depth=119 | 173.33 | 14.10 |
| Exit tick 24 | K=72, ZOffset=-41 | — | 13.93 |
| Exit tick 60 | K=188, ZOffset=-41 | — | 15.04 |
| Exit tick 100 | K=348, ZOffset=-41 | — | 16.86 |
| Exit tick 123 | K=440, ZOffset=-41, projected depth=439 | 884.44 | 17.37 |

Across all 124 Level 1 exit samples, the height remains finite and changes by
less than one logical unit per tick. The corrected Blaster-height/projected
edge-length ratio remains between 0.18 and 0.23; the oversized old t=23 value
is reproduced by the regression test.

For the same t=23 state and metadata-backed local Jacobians, the adapter
equations yield approximately 62.17 units for Levels 8/10 and 51.35 for Level
9, compared with the former reversed-ratio values 39.32 and 47.60. The
projection scales differ by route; the shared tests also bound the resulting
Blaster-to-edge ratio rather than imposing a per-level size.

For Level 1 Approach, the corrected height is approximately 1.08 at the
initial 960-unit depth offset, approximately 2.04 at the 90-tick midpoint,
and exactly 20 at completion. The previous Atari ratio began at approximately
369.09. A bounded Level 4 ReverseZoom sample stays between 18 and 20 units as
ZOffset returns from 3 to 6, then returns to exactly 20 after the player
returns to the rim.

## Scope and preservation

Changed project files:

- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`
- `src/Tempest.Game/Rendering/Projection.cs`
- `tests/Tempest.Game.Tests/BlasterScaleRegressionTests.cs`

The new tests cover ordinary Playing and ExitDelay size, both Level 1 zoom
phases and their boundaries, the successful exit-to-Approach handoff,
Approach growth/completion, bounded ReverseZoom, render-state read-only
behavior, six-line artwork topology, and adapter-backed exit scale across
Levels 1-6 and 8-10. Level 10 reaches ExitZoom through its AvoidSpikes gate.
Level 7 receives a bounded shared-scale regression check; no Level-7-specific
geometry, registration, centering, or motion code changed.

The accepted ordinary Playing outward orientation, anchor/lane geometry,
highlighting, movement, firing, collision, and fixed 20-unit artwork remain
unchanged. No placement values, tube projection, global camera settings,
Approach timing/offsets, starfield, reverse-zoom simulation, recording format,
playback selection, or simulation state was changed. The removed Blaster
brightness diagnostic remains absent. No durable behavior document was
updated because manual acceptance is still pending.

No screenshots, images, videos, frames, or recording collections were opened
or analyzed. `TEMP/myNotes.md` was not accessed.

## Validation

- Focused Blaster scale/presentation checks:
  **PASS — 13 passed, 0 failed, 0 skipped.**
- Targeted Blaster/player rendering, Level 1/4 exits, Levels 8/9/10
  presentation, Atari adapters, Approach/transition, ReverseZoom,
  display/camera, and recording/playback regressions:
  **PASS — 378 passed, 0 failed, 0 skipped.**
- Full solution tests using VSTest with `--blame-hang-timeout 5min`:
  **PASS — 1,634 passed, 0 failed, 0 skipped.**
- Release solution build:
  **PASS — 0 warnings, 0 errors.**
- `git diff --check`: **PASS.**

No canceled, stalled, blocked, or timed-out validation was counted as a pass.
The renderer tests compare projected geometry and numeric bounds, not
screenshots.

## Git state and preservation

Starting state matched the expected baseline:

- Branch `master`, HEAD
  `34eedf0cf996cfe660494e34bdff2a1499696068`.
- Upstream `origin/master`, 0 ahead / 0 behind.
- Empty index.
- Pre-existing tracked modification: `.github/copilot-instructions.md`.
- Pre-existing untracked files:
  `Reference/arcade/MBUCOD.V05.txt`,
  `Reference/arcade/MBUDOC.DOC`, and `screenshot000.png`.

Ending state: branch, HEAD, upstream, and ahead/behind remain unchanged; the
index remains empty. The `.github/copilot-instructions.md` modification and
both untracked arcade-reference files remain untouched. The three renderer
files and new test file are modified/untracked as expected and are not staged.
No commit or push was performed.

The initially untracked `screenshot000.png` was present at the starting-state
check but absent at the later ending-state check. It was not opened, restored,
or intentionally modified during this task; the cause of its disappearance is
unknown. This is recorded as an unrelated workspace discrepancy.

## Manual handoff

Use the following commands from the repository root; the game was not launched:

1. Ordinary Level 1 with Atari presentation:

   ```powershell
   dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation
   ```

2. Built-in deterministic Level 1 profile with Atari presentation:

   ```powershell
   dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --gameplay-playback --atari-presentation
   ```

   This is the known seeded Level 1 profile. It is not established as an
   exit/Approach recording; no user recording filename was inferred or
   searched for.

3. Representative open-tube direct starts:

   ```powershell
   dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 8 --atari-presentation
   dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 9 --atari-presentation
   ```

4. Diagnostic Level 10, when desired:

   ```powershell
   dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 10 --atari-presentation
   ```

Manual acceptance remains **PENDING**. Neal should confirm that ordinary
Playing remains accepted; the Blaster stays compact and follows the exiting
lane through ExitDelay and both zoom phases; successful exit hands off
smoothly; Approach starts small, grows coherently, and finishes at the
accepted size; and representative closed/open tubes preserve gameplay,
timing, and accepted Levels 8/9/10 placement.

## AI-Logs

- Prompt: [2026-11-17-blaster-exit-approach-scale-correction-prompt.md](../Prompts/2026-11-17-blaster-exit-approach-scale-correction-prompt.md)
- Report: [2026-11-17-blaster-exit-approach-scale-correction-report.md](./2026-11-17-blaster-exit-approach-scale-correction-report.md)
