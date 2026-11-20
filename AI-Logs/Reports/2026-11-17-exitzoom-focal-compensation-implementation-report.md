# ExitZoom Far-Opening Focal Compensation Candidate

Date: 2026-11-17

## Status

**IMPLEMENTATION CANDIDATE COMPLETE — automated validation PASS; manual
acceptance PENDING.** This is a DEVELOPMENT-ONLY, presentation-only correction
for `--atari-presentation`. No gameplay or durable behavior documentation was
changed. No interactive game was launched and no visual assets were inspected.

## Implementation

Added `ExitZoomFocalCompensation`, owned by `VectorRenderer`. On the first
mapped world-point projection while the state is `ExitZoom`, it latches the
opening through the same uncorrected point projector used to draw the tube.
On later K/ZOffset or portrait-perspective samples, it recalculates that mapped
opening and returns `latchedOpening - currentOpening`.

The compensation is added in logical presentation space by the shared
`ProjectWorldPoint` path, after the active adapter and accepted placement but
before the shared portrait host camera. The unchanged camera subsequently
maps this translation to host space. This moves the complete projected
playfield together, including tube rails/rims, the Blaster anchor, Spikes,
and tube-space objects using the shared projector. Local artwork sizes are not
modified. HUD, the fixed `AVOID SPIKES` warning, and transition starfield
positions do not use this world-point path and are not compensated.

Opening definition is derived from `CurrentLevel.Definition.Points`:

- Closed definitions (`Continuous`) use the arithmetic center of every mapped
  far-rim vertex.
- Open definitions use the midpoint of the two mapped terminal vertices
  (`Points[0]` and `Points[^1]`), not the all-vertex centroid.

The helper is inactive outside `ExitZoom`, resets when another state is
rendered, and resets when `VectorRenderer.SetGame` replaces the game. A new
level/configuration also starts a fresh latch. The regular loop performs one
simulation step before each render, so the first ExitZoom render follows the
state transition before travel updates its projection values. No latch or
compensation is stored in gameplay/recording state.

The translation is global and has no per-level focal constants. Option-off
projection remains on the existing legacy path. Level 5's special live
near-anchor mapping and the accepted Level 8/9/10 placement constants were not
changed. Level 7-specific Playing geometry, entity registration, centering,
and depth-motion code were not changed.

## Trajectory evidence

Samples are logical presentation Y: onset `(K=70, ZOffset=5)`, first Z step
`(70, 3)`, Z-phase end `(70, -41)`, first K-growth step `(72, -41)`, mid-K
`(240, -41)`, and late `(440, -41)`. “Before” values are the controlling
investigation's evaluation of the old mapped-opening equations. “After” is
the exact invariant exercised against the rendered far geometry by the new
tests; the host camera does not change.

| Level | Before: mapped opening Y at six samples | After: opening Y at all six samples |
|---|---|---|
| 5 | 525.473, 524.900, 453.848, 451.060, 216.928, -61.800 | 525.473 |
| 8 | 347.061, 340.826, -497.399, -526.247, -2949.479, -5834.279 | 347.061 |
| 9 | 300.413, 293.599, -622.427, -653.952, -3302.101, -6454.658 | 300.413 |

The corresponding old onset-relative late drift (-587.273, -6181.340, and
-6755.071 logical pixels for Levels 5, 8, and 9) is removed by the shared
translation. The tests verify finite output through both phases.

Levels 1–4 are included as working controls. Their mapped closed far-rim
centers remain at onset through the same compensated samples, and tube
coordinates at ExitZoom onset exactly match Playing coordinates at unchanged
projection values. Level 6 is included in the shared-path regression set.
Level 10 is included in the opening trajectory test and is driven through its
AvoidSpikes/ExitDelay gate; its diagnostic placement remains unchanged. Its
old calculated opening trajectory was 483.540, 488.579, 1166.077, 1189.394,
3147.980, 5479.630 logical Y and is held at 483.540 by the candidate.

## Registration and preservation

- Tests compare every tube line under ExitZoom compensation against an
  unlatched renderer at the same projection sample and require one identical
  translation.
- Level 8 tests require the Blaster artwork and Spike lines/tips to receive
  that same translation. The Blaster scale is equal in both renderers, and
  all Blaster segments retain their local geometry; no second translation is
  applied.
- Closed and open opening tests derive the center from rendered mapped far
  vertices. The open test distinguishes the terminal midpoint from the
  all-vertex centroid and verifies the mapped result differs from source
  geometry.
- Lifecycle tests render across Level 4 and Level 10 AvoidSpikes waits and
  ExitDelay, test capture on actual ExitZoom entry, verify ReverseZoom clears
  the old anchor, and require a retry to establish a new one. The Level 4 and
  Level 10 onset probes change K only at the state boundary to make premature
  capture observable; this is DEVELOPMENT-ONLY test instrumentation, not a
  gameplay rule.
- Successful Level 1 handoff verifies the next Approach tube matches a fresh
  renderer, with no ExitZoom compensation retained.
- Rendering-state assertions verify that rendering does not change state,
  player depth/lane/edge position, K, or ZOffset. No gameplay, recording
  schema, playback input, RNG, exit timing, AvoidSpikes, collision, or
  progression code was modified.
- Accepted Level 8 `+212.28139`, Level 9 `+180.96825`, and Level 10 `+50`
  placements remain covered by their existing presentation tests. Existing
  Blaster-scale, Level 7, option-off/legacy, recording/playback, camera, and
  Approach tests also passed.

## Files changed

- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `src/Tempest.Game/Rendering/ExitZoomFocalCompensation.cs`
- `tests/Tempest.Game.Tests/ExitZoomFocalCompensationTests.cs`
- `AI-Logs/Prompts/2026-11-17-exitzoom-focal-compensation-implementation-prompt.md`
- `AI-Logs/Reports/2026-11-17-exitzoom-focal-compensation-implementation-report.md`

The existing `.github/copilot-instructions.md` modification and untracked
`Reference/arcade/MBUCOD.V05.txt` / `Reference/arcade/MBUDOC.DOC` were preserved.
No screenshots, images, videos, frames, contact sheets, ROM files, or
`TEMP/myNotes.md` were inspected.

## Validation

- **PASS — focused:** `ExitZoomFocalCompensationTests`, 16 passed, 0 failed.
- **PASS — targeted regressions:** 843 passed, 0 failed. Included ExitZoom,
  AvoidSpikes/retry, Blaster scale, Atari adapter/renderer, Levels 1–10
  presentation coverage (excluding Level 7 ExitZoom reconstruction), Level
  Approach/transition, recording/playback, simulation, display/camera, and
  Level 7 Playing regressions.
- **PASS — full solution:** `dotnet test TempestCSharp.slnx --no-restore
  --blame-hang-timeout 4m --verbosity minimal`; 1,650 passed, 0 failed,
  0 skipped. The VSTest hang-timeout protection completed without a hang.
- **PASS — Release build:** `dotnet build TempestCSharp.slnx
  --configuration Release --no-restore --verbosity minimal`; 0 warnings,
  0 errors.
- **PASS — `git diff --check`.**
- **PASS — prompt copy:** the saved prompt is text-identical to the complete
  662-line user task attachment.

## Git state

Starting and ending branch/commit/tracking state:

- Branch `master`, HEAD `753728e79ed86624c9b1728dfd1c8db8f49d1e2b`
- Upstream `origin/master`, ahead/behind `0/0`
- Index unchanged and clean; nothing staged, committed, pushed, or checked out
- Pre-existing tracked modification: `.github/copilot-instructions.md`
- Pre-existing untracked references: `Reference/arcade/MBUCOD.V05.txt` and
  `Reference/arcade/MBUDOC.DOC`
- Candidate source and test files are new and unstaged; `AI-Logs/` files are
  ignored and unstaged

## Manual handoff

Run each command from the repository root. Do not use gameplay recordings for
these checks.

```powershell
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation --level 1
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation --level 4
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation --level 5
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation --level 8
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation --level 9
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation --level 10
```

Level 10 is optional diagnostic comparison only; its manual acceptance is not
required for this candidate.

Neal should judge whether ExitZoom stays focused on the far/deep opening as
perspective increases, the tube expands around it, the Blaster remains
attached to its lane and compact, Levels 1–4 remain correct, Levels 5/8/9
regressions are fixed, Approach handoff is coherent, and gameplay/timing plus
accepted ordinary Playing placement remain unchanged.

**Manual acceptance: PENDING.** No Level 7 reconstruction was done; its
specific Playing path is untouched, and its ExitZoom presentation has not
received manual acceptance. No commit or push was made.

## AI-Logs

- Prompt: `AI-Logs/Prompts/2026-11-17-exitzoom-focal-compensation-implementation-prompt.md`
- Report: `AI-Logs/Reports/2026-11-17-exitzoom-focal-compensation-implementation-report.md`
