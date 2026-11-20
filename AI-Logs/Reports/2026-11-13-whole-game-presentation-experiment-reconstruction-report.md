# Whole-Game Presentation Experiment Reconstruction

**Date:** 2026-11-13
**Status:** PARTIAL — fail-closed cross-level clipping
**Completion token:** `TEMPEST_PRESENTATION_RECONSTRUCTION_EXPERIMENT_20261113`

## Starting baseline

- Branch: `master`
- `HEAD` and `origin/master`: `385f603a94391756b2ca6cccd524dc17b281e782`
- Upstream: `origin/master`
- Index, tracked worktree, and non-ignored untracked files: clean
- Prompt preserved at `../Prompts/2026-11-13-whole-game-presentation-experiment-reconstruction-prompt.md`

The candidate was reconstructed from the accepted source and the two authoritative
2026-11-12 investigation reports. The corrected report's provenance was followed:
LEFT is current C#; RIGHT/MAME is the target. No discarded experiments were
consulted, and no visual assets were searched, opened, or analyzed.

## Implementation performed

- Added an explicit `PresentationMode`; normal startup selects the experimental
  portrait presentation and `--legacy-presentation` selects the established
  4:3 presentation.
- Preserved the 1280 × 960 host window, legacy 800 × 600 projection space,
  `K = 70`, and `ZOffset = 5`.
- Added the experimental 600 × 800 logical / 720 × 960 centered viewport
  (280-pixel side margins), with 1.2 output scale, 1.05 uniform playfield
  scale, and initial vertical translation −78.
- Applied the candidate through one camera to the complete playfield rendering
  pass; HUD and screen-space messages use a separate portrait camera/layout.
- Expanded display regression coverage to production Levels 1–9, model-point
  preservation, Level 9 prominence, shared orientation, and viewport bounds.

No gameplay or model coordinates, level topology, projection constants,
simulation timing, or mechanics were intentionally changed. Level 9's tube
prominence assertion targets at least 88% of portrait viewport width.

## Validation and fail-closed result

Command:

`dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --filter "FullyQualifiedName~DisplayConfigurationTests" --blame-hang-timeout 3m --verbosity minimal`

- **Build:** PASS — both game and test projects compiled.
- **Focused tests:** FAIL — 21 passed, 9 failed, 30 total.
- **Cross-level blocker:** the shared transform put tube points above the
  portrait viewport on Levels 1, 3, 5, 6, 7, and 8. The first failing bounds
  are negative screen-space Y values (approximately −6 to −54 pixels).
  Orientation assertions did not report inversions; the failure is clipping
  caused by the shared presentation placement/viewport assumption.
- The newly broadened legacy-projection assertions for Levels 8 and 9 also
  failed with an expected value of 48 versus 46. The exact cause was not
  pursued after the cross-level fail-closed result.
- The HUD viewport corner assertion differed only by floating-point rounding
  (`960` versus `960.00006`).
- **Full suite:** not run.
- **`git diff --check`:** PASS.

Per the task's cross-level rule, no offset, per-level correction, or other
follow-up implementation was attempted after the clipping was detected. The
candidate is not ready for acceptance; Neal's review is required before any
further calibration.

## Changed files and Git state

Tracked worktree changes:

- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Rendering/DisplayConfiguration.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `tests/Tempest.Game.Tests/DisplayConfigurationTests.cs`

The paired prompt and this report are ignored AI-Logs working material. At
report creation, the index remained clean, no non-ignored untracked files were
present, and branch/`HEAD`/`origin/master` remained `master` /
`385f603a94391756b2ca6cccd524dc17b281e782`. Nothing was staged, committed, or
pushed.

## Manual acceptance and remaining questions

The game was not launched. No manual visual acceptance was performed. No
visual assets were analyzed. Perspective/shoulder calibration remains
unresolved; `K` and `ZOffset` were deliberately left unchanged. The shared
portrait placement must first be reviewed because earlier-level clipping is
already established mechanically.

For Neal's diagnosis, the requested comparison commands are:

```powershell
dotnet run --project src\Tempest.Game -- --level 9
dotnet run --project src\Tempest.Game -- --level 9 --legacy-presentation
```

Recommended regression checks after resolving the blocker are Level 9 first,
then Levels 1–8 for inversion, mirroring, clipping, displacement, and coherent
alignment of tube, Blaster, enemies, projectiles, effects, and transition
presentation. Do not treat this partial candidate or automated results as
manual acceptance.
