# Open-Tube ExitZoom Convergence Pivot Implementation

Date: 2026-11-18

## Status

**IMPLEMENTATION CANDIDATE COMPLETE — automated validation PASS; manual
acceptance PENDING.** This is a DEVELOPMENT-ONLY, presentation-only refinement
of the existing ExitZoom focal-compensation candidate. No game was launched.
No gameplay or durable behavior documentation was changed.

## Implementation

The open-tube pivot now uses the common intersection of all mapped
near-to-far rails. The helper maps every definition point at both
`configuration.Position` and `configuration.Position + configuration.Depth`
through the existing callback, which uses the active adapter and accepted
placement in logical presentation space before the shared host camera.

For each non-degenerate rail, the implementation normalizes its perpendicular
and accumulates the least-squares normal equations over the complete rail set.
It solves the resulting 2x2 system, requires at least two independent
directions, verifies that the maximum perpendicular residual is below the
numerical tolerance, and rejects non-finite or inconsistent inputs explicitly.
Only rails whose endpoints coincide within the derived tolerance are excluded.
The tolerance is `max(1e-5, coordinateScale * 32 * float epsilon)`, based on
mapped coordinate magnitude and float precision rather than visual preference.

Closed tubes retain the existing mapped far-rim arithmetic center unchanged.
For Level 5, the mapped point set, summation order, and center calculation are
unchanged. Open-tube terminal midpoint is not used; it is the midpoint of the
omitted closing chord, not a visible rail or focal point.

The existing presentation-only latch and shared translation remain unchanged:
`latchedPivotAtExitStart - currentMappedPivot`. Rendering still applies the
same translation after adapter and accepted placement, before the portrait
host camera, and through the shared world-point path. No per-level pivot
coordinates or constants were added. Gameplay, timing, recording, and
playback state are untouched.

The open-rail intersection is an equation-derived **PROJECT TRANSLATION**,
not literal Atari ROM behavior or manual arcade acceptance.

## Geometry and behavior tests

Focused tests use the actual mapped tube rail lines and verify:

- All 16 rails in Levels 8, 9, and diagnostic 10 are valid and participate.
- The least-squares fit is independently residual-checked against the
  float-precision-derived tolerance.
- Onset convergence is approximately `(400, 369.100)`, `(400, 292.332)`,
  and `(400, 538.636)` for Levels 8, 9, and 10 respectively. These values
  exist only as test expectations.
- Each open-tube result differs from the terminal midpoint.
- The convergence remains fixed over ZOffset and K-growth samples, while all
  tube rail/rim endpoints expand uniformly around that pivot and remain finite.
- Closed Level 5 and Levels 1-4/6 continue using the mapped far-rim center.
- The existing lifecycle and shared tube/Blaster/Spike registration checks
  still pass; Blaster size remains unchanged.

No screenshots, images, videos, frames, or other visual assets were inspected.

## Files

- `src/Tempest.Game/Rendering/ExitZoomFocalCompensation.cs`
- `tests/Tempest.Game.Tests/ExitZoomFocalCompensationTests.cs`
- `AI-Logs/Prompts/2026-11-18-open-tube-exit-convergence-pivot-implementation-prompt.md`
  (byte-identical to the provided task attachment)
- `AI-Logs/Reports/2026-11-18-open-tube-exit-convergence-pivot-implementation-report.md`

`VectorRenderer.cs` was not changed by this task; its pre-existing candidate
changes were preserved. The unrelated `.github/copilot-instructions.md`
modification and untracked local reference files were preserved.

## Validation

- **PASS — focused:** `dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter FullyQualifiedName~ExitZoomFocalCompensationTests --verbosity minimal`; 19 passed, 0 failed.
- **PASS — targeted regressions:** 670 passed, 0 failed. Filtered for ExitZoom, AvoidSpikes, Spikes, Blaster, Atari presentation, VectorRenderer, Approach, recording/playback, display, and camera tests.
- **PASS — full solution:** `dotnet test .\TempestCSharp.slnx --no-restore --blame-hang-timeout 4m --verbosity minimal`; 1,653 passed, 0 failed, 0 skipped. VSTest hang-timeout protection completed.
- **PASS — Release build:** `dotnet build .\TempestCSharp.slnx --configuration Release --no-restore --verbosity minimal`; 0 warnings, 0 errors.
- **PASS — `git diff --check`.**
- **PASS — prompt copy:** SHA-256 comparison confirmed byte identity with the supplied attachment.

## Git state

Starting and ending branch/commit/tracking state:

- Branch `master`; HEAD `753728e79ed86624c9b1728dfd1c8db8f49d1e2b`
- Upstream `origin/master`; ahead/behind `0/0`
- Index clean; nothing staged, committed, pushed, checked out, stashed, reset,
  restored, or cleaned
- Pre-existing tracked worktree changes remain in `.github/copilot-instructions.md`
  and `src/Tempest.Game/Rendering/VectorRenderer.cs`; both were preserved
- Pre-existing untracked local references remain
  `Reference/arcade/MBUCOD.V05.txt` and `Reference/arcade/MBUDOC.DOC`
- Existing untracked candidate files
  `src/Tempest.Game/Rendering/ExitZoomFocalCompensation.cs` and
  `tests/Tempest.Game.Tests/ExitZoomFocalCompensationTests.cs` remain untracked
  and contain this milestone's changes
- AI-Logs records are ignored and were not staged

## Manual handoff

Manual acceptance remains **PENDING**. Neal should run and judge:

```powershell
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation --level 5
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation --level 8
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation --level 9
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation --level 10
```

Level 10 is optional and diagnostic-only. Confirm Level 5 remains fixed;
Levels 8/9 (and optional 10) expand around the visible rail-convergence point;
open-tube perspective no longer shifts to another apparent focal point; tube
and Blaster remain registered; the Blaster remains compact; ordinary Playing
placement is unchanged; Approach handoff is coherent; and gameplay and timing
remain normal.

No known automated blocker remains. The pivot remains a candidate until Neal's
manual acceptance.
