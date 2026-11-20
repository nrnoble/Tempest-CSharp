# Whole-Game Presentation Shared Vertical Placement Correction

**Date:** 2026-11-13
**Status:** EXPERIMENTAL PRESENTATION CANDIDATE — automated validation passed; manual acceptance pending
**Completion token:** `TEMPEST_PRESENTATION_SHARED_VERTICAL_PLACEMENT_CORRECTION_20261113`

## Starting state and scope

- Branch: `master`
- `HEAD` and `origin/master`: `385f603a94391756b2ca6cccd524dc17b281e782`
- Index: clean
- Exactly four tracked candidate files modified:
  `Program.cs`, `DisplayConfiguration.cs`, `VectorRenderer.cs`, and
  `DisplayConfigurationTests.cs`
- Non-ignored untracked files: none
- Starting diff summary: 4 files changed, 373 insertions, 35 deletions

The authoritative basis was the 2026-11-12 whole-game Level 9 investigation,
its label-correction report, the 2026-11-13 reconstruction report, and the
2026-11-13 cross-level clipping diagnostic. The corrected image provenance is
LEFT = current C#, RIGHT/MAME = target. No Gemini or discarded experiment
material was used. No visual assets were searched for, opened, or analyzed.

The substantive prompt was preserved at
[`2026-11-13-whole-game-presentation-shared-vertical-placement-correction-prompt.md`](../Prompts/2026-11-13-whole-game-presentation-shared-vertical-placement-correction-prompt.md).

## Exact files read and changed

Project source/test files read:

- `src/Tempest.Game/Rendering/DisplayConfiguration.cs`
- `src/Tempest.Game/Rendering/Projection.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Levels/LevelDefinition.cs`
- `src/Tempest.Game/Levels/LevelLoader.cs`
- `src/Tempest.Game/Levels/Level.cs`
- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `tests/Tempest.Game.Tests/DisplayConfigurationTests.cs`

Authoritative reports read:

- `AI-Logs/Reports/2026-11-12-whole-game-presentation-level9-readonly-investigation-report.md`
- `AI-Logs/Reports/2026-11-12-whole-game-presentation-level9-label-correction-report.md`
- `AI-Logs/Reports/2026-11-13-whole-game-presentation-experiment-reconstruction-report.md`
- `AI-Logs/Reports/2026-11-13-whole-game-presentation-cross-level-clipping-diagnostic-report.md`

Tracked files changed in this correction:

- `src/Tempest.Game/Rendering/DisplayConfiguration.cs`
- `tests/Tempest.Game.Tests/DisplayConfigurationTests.cs`

`Program.cs` and `VectorRenderer.cs` were not changed during this correction.
The existing shared playfield camera architecture is retained.

## Shared placement derivation

The old shared portrait vertical translation was **−78 internal units**. The
clipping diagnostic identified Level 6's near-rim minimum as the all-level
limiting bound. With unchanged `CenterY=360`, `YOffset=−40`, model minimum
Y `−218`, near depth 50, `K=70`, and `ZOffset=5`, its projected minimum is:

`360 + (−218 − 40) × 70 / (50 + 5) = 31.63636...`

After the unchanged shared playfield scale 1.05, that is
`33.21818...` internal units. The exact no-overflow translation boundary is
therefore approximately `−33.21818`. The single candidate translation is now
**−33.1**, leaving approximately `0.11818` portrait-internal units, or
**0.14182 host pixels**, of top-edge clearance. This small positive allowance
avoids placing the float boundary exactly on zero; it is not an arcade-derived
constant or a visual-calibration claim.

No per-level, level-group, topology-based, or geometry-based presentation
values were added. The 1.05 playfield scale, 1.2 output scale, horizontal
placement, 600 × 800 logical portrait area, centered 720 × 960 viewport,
1280 × 960 host window, projection space, `K=70`, and `ZOffset=5` are
unchanged. Gameplay points, topology, lane order, mechanics, and HUD layout
are unchanged.

## Levels 1–9 final tube bounds

Bounds are approximate host pixels from existing production points projected
at both rims (`z=50` and `z=450`) and passed through the candidate shared
camera. The viewport is `x=[280,1000]`, `y=[0,960]`. The horizontal bounds
are unchanged from the prior candidate.

| Level | Portrait host X bounds | Portrait host Y bounds | Overflow L/R/T/B |
|---|---:|---:|---:|
| 1 | `[319.27,960.73]` | `[15.26,656.72]` | `0/0/0/0` |
| 2 | `[381.81,898.19]` | `[77.80,594.17]` | `0/0/0/0` |
| 3 | `[324.08,955.92]` | `[20.07,651.91]` | `0/0/0/0` |
| 4 | `[288.80,991.20]` | `[148.82,567.37]` | `0/0/0/0` |
| 5 | `[303.24,976.76]` | `[2.09,675.62]` | `0/0/0/0` |
| 6 | `[365.78,914.22]` | `[0.14,582.26]` | `0/0/0/0` |
| 7 | `[352.95,927.05]` | `[47.79,621.89]` | `0/0/0/0` |
| 8 | `[319.27,960.73]` | `[17.55,659.01]` | `0/0/0/0` |
| 9 | `[319.27,960.73]` | `[292.46,659.01]` | `0/0/0/0` |

Level 9 retains a projected width of approximately `641.46` of the 720-pixel
viewport, or **89.09%**. No production tube bounds exceed any viewport edge.
Automated tests confirm orientation/order preservation and unchanged source
model points.

## Test corrections and alignment regression

- The legacy tube line-count assertion now uses
  `VertexCount + 2 * PlayableLaneCount`: 48 lines for closed 16-lane tubes,
  46 for open 16-vertex / 15-lane Levels 8–9. `DrawTube` was not changed.
- The HUD bottom-right mapping assertion now uses a **0.001-pixel** distance
  tolerance around `(1000,960)`, narrowly accommodating the observed
  `960.00006` floating-point result.
- Added an intermediate-depth registration test for production Levels 1, 6,
  and 9 at depth 250. It projects a representative tube-rail midpoint and
  lane/gameplay anchor through the same experimental playfield camera and
  asserts agreement within 0.001 pixels.

Source inspection confirmed that the current `VectorRenderer.Draw` pass
applies one `BeginMode2D(GetPlayfieldCamera(...))` around tube, Blaster,
enemies, shots, Spikes, effects, score popups, transition stars, and applicable
Game Over vectors. HUD/screen messages retain their separate camera. No
tube-relative playfield path was found bypassing the shared playfield camera.

## Validation

- Focused command:
  `dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --filter "FullyQualifiedName~DisplayConfigurationTests|FullyQualifiedName~TransitionPresentationTests" --blame-hang-timeout 3m --verbosity minimal`
  - **PASS:** 58 passed, 0 failed, 0 skipped.
- Full suite:
  `dotnet test TempestCSharp.slnx --blame-hang-timeout 3m --verbosity minimal`
  - **PASS:** 1,362 passed, 0 failed, 0 skipped.
- Build:
  `dotnet build TempestCSharp.slnx --no-restore --verbosity minimal`
  - **PASS:** 0 warnings, 0 errors.
- `git diff --check` and staged-diff check: **PASS**.

No command timed out or was blocked. The interactive game was not launched.

## Manual acceptance and remaining questions

This remains an experimental candidate, not an accepted visual calibration.
Manual Level 9 comparison and regression on Levels 1–8 remain required.
Perspective/shoulder relationships were not calibrated; HUD design, scaling,
projection constants, and gameplay geometry were not adjusted.

Experimental Level 9:

```powershell
dotnet run --project .\src\Tempest.Game -- --level 9
```

Legacy Level 9:

```powershell
dotnet run --project .\src\Tempest.Game -- --level 9 --legacy-presentation
```

To check Levels 1–8 individually, substitute the desired number for `9`:

```powershell
dotnet run --project .\src\Tempest.Game -- --level <1-8>
```

Recommended manual checks: portrait composition and tube prominence;
vertical placement and all-edge clipping; no inversion or mirroring;
registration of tube, Blaster, enemies, Spikers, shots, Spikes, and effects;
HUD legibility/containment; and unchanged legacy presentation.

## Final Git state

Branch remains `master`; `HEAD` and `origin/master` remain
`385f603a94391756b2ca6cccd524dc17b281e782`. The index is clean. The same four
candidate files remain modified, with this correction limited to
`DisplayConfiguration.cs` and `DisplayConfigurationTests.cs`. No non-ignored
untracked files were introduced. The prompt and this report are ignored
AI-Logs files. Nothing was staged, committed, or pushed. No visual assets or
Gemini materials were analyzed.
