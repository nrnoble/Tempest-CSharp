# Level 7 Record-06 Reference-Shape Presentation Diagnostic

Date: 2026-11-16
Outcome: **IMPLEMENTED; AUTOMATED VALIDATION PASS; MANUAL ACCEPTANCE PENDING**

## Scope and outcome

Added an opt-in **DEVELOPMENT-ONLY / VISUAL-DIAGNOSTIC** presentation mapping
for the existing direct-Atari Level 7 record-06 prototype. The ordinary
`--atari-tube-prototype` path remains the control. This is not an Atari hardware
reconstruction, ROM-backed correction, geometry change, or acceptance of the
visual result.

The new selector is:

```text
--atari-tube-reference-shape-diagnostic
```

It requires an explicit `--level 7` and `--atari-tube-prototype` interactive
startup. It is rejected with unsupported prototype contexts, including
recording/playback, other gameplay scenarios, input diagnostics, incompatible
presentation options, and the factor-128 RN=$000F diagnostic. It is opt-in,
does not enter recording metadata, and does not change the default.

## Presentation mapping and derivation

The controlling investigation report is
[2026-11-16-level7-record06-reference-shape-transform-investigation-report.md](./2026-11-16-level7-record06-reference-shape-transform-investigation-report.md).
Its shared-gain fit in reference-raster coordinates is approximately
`(0.4215, 0.575)`, with a far-set-specific relative translation of
`(+27,+37)` pixels in the 480x640 fit raster.

The prototype portrait viewport is 720x960, or 1.5 times the reference raster
on both axes. The HUD camera then scales logical viewport coordinates by 1.2.
Therefore:

- The common horizontal aspect is `0.4215 / 0.575 = 0.7330435`.
- The existing DVG viewport scale `0.4` is multiplied by the fitted vertical
  gain and portrait raster ratio: `0.4 * 0.575 * 1.5 = 0.345`.
- The far-only logical translation is
  `(27 * 1.5 / 1.2, 37 * 1.5 / 1.2) = (+33.75,+46.25)`.

The near endpoint set retains the existing provisional map's logical anchor
`(300,400)`. The far map uses the same gain and an additional logical
translation, giving anchor `(333.75,446.25)`. The offset is applied before the
existing HUD camera, after projection and vector-list construction. No
reference-raster pixel coordinates are copied directly into logical space.

Representative record-06 endpoint mappings in logical viewport coordinates:

| Endpoint | Projected DVG point | Diagnostic logical point |
|---|---:|---:|
| Near index 0 | `(214,678)` | `(354.1206,166.09)` |
| Far index 0 | `(-90,102)` | `(310.989,411.06)` |

These are visual-fit calibration values only. They are **not ROM-BACKED,
Atari-documented, hardware constants, or accepted project translations**.
Original physical X/Y gain, analog-output and monitor calibration, historical
depth-conditioned translation, and historical use of these fitted values
remain unknown.

## Preservation

The mapping consumes the existing projected near/far endpoint arrays only.
Each mapped endpoint is reused for its rim edges and same-index rail;
rails remain straight `Near[i]`-to-`Far[i]` segments, and each closed rim
retains its cyclic ordering and closure.

Record-06 coordinates and metadata, runtime positions, projector behavior,
builder topology, point identities/order, gameplay, accepted Levels 1-6,
Levels 8-9, `--atari-presentation`, and the factor-128 diagnostic were not
changed. With the new option absent, the direct-Atari control continues to use
the original provisional viewport mapping. The new mapping is gated by the
existing direct Level 7 prototype/state selection and does not read or modify
gameplay state.

## Files changed for this task

- `src/Tempest.Game/Debug/DebugController.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Rendering/AtariDvgViewportMap.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `src/Tempest.Game/Rendering/AtariTubeReferenceShapeDiagnostic.cs` (new)
- `tests/Tempest.Game.Tests/AtariTubeRecordPrototypeTests.cs`
- `AI-Logs/Prompts/2026-11-16-level7-record06-reference-shape-presentation-diagnostic-prompt.md`
- This report:
  `AI-Logs/Reports/2026-11-16-level7-record06-reference-shape-presentation-diagnostic-report.md`

Both AI-Logs files are ignored and unstaged.

The source and test files above were already part of a dirty worktree at task
start; the implementation was integrated without reverting or absorbing the
other pre-existing changes.

## Validation

- Focused prototype suite:
  `dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --filter FullyQualifiedName~AtariTubeRecordPrototypeTests --blame-hang-timeout 2m --no-restore`
  — **PASS**, 46 passed.
- Targeted presentation, CLI, Level 7, recording/playback, and rendering
  regressions:
  `dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --filter "FullyQualifiedName~AtariTubeRecordPrototypeTests|FullyQualifiedName~DebugControllerTests|FullyQualifiedName~DisplayConfigurationTests|FullyQualifiedName~AtariPresentationAdapterTests|FullyQualifiedName~VectorRendererAtariPresentationTests|FullyQualifiedName~GameplayRecordingTests|FullyQualifiedName~GameplayPlaybackTests|FullyQualifiedName~GameplayPlaybackSessionTests|FullyQualifiedName~PlaybackScreenshotCaptureTests|FullyQualifiedName~Level7" --blame-hang-timeout 5m --no-restore`
  — **PASS**, 450 passed.
- Full solution test suite:
  `dotnet test .\TempestCSharp.slnx --blame-hang-timeout 5m`
  — **PASS**, 1,551 passed, 0 failed, 0 skipped.
- Release solution build:
  `dotnet build .\TempestCSharp.slnx --configuration Release --no-restore`
  — **PASS**, 0 warnings, 0 errors.
- `git diff --check` — **PASS**, exit code 0. Git emitted only existing
  LF-to-CRLF working-copy advisories for `LevelLoader.cs` and `InputSystem.cs`.

An initial focused run exposed an overly strict exact-float assertion on a
10-unit diagnostic delta (`2.5289917` versus rounded `2.529`); the test now
uses a small floating-point tolerance for that derived delta while retaining
exact representative mapped-point assertions. The focused rerun and all later
validation passed.

The interactive game was not launched. Automated checks establish mapping and
topology invariants only; visual acceptance remains **PENDING**.

## Git and worktree state

Starting state:

- Branch: `master`
- HEAD: `385f603a94391756b2ca6cccd524dc17b281e782`
- Upstream: `origin/master`, 0 ahead / 0 behind
- Index: clean
- Pre-existing tracked worktree modifications: 17 paths
- Pre-existing nonignored untracked paths: 13

Ending state:

- Branch and HEAD unchanged; upstream remains `origin/master`.
- Upstream tracking remains 0 ahead / 0 behind.
- Index remains clean; no files staged.
- The pre-existing 17 tracked modifications and 13 untracked paths remain.
- The new diagnostic source is the one additional nonignored untracked path.
- No unrelated pre-existing files or protected material were modified.

No staging, commit, or push was performed.

## Manual A/B handoff

Control:

```powershell
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 7 --atari-tube-prototype
```

Reference-shape diagnostic:

```powershell
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 7 --atari-tube-prototype --atari-tube-reference-shape-diagnostic
```

Separate factor-128 experiment, for identification only:

```powershell
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 7 --atari-tube-prototype --atari-tube-rn000f-diagnostic
```

The factor-128 command is a separate experiment and must not be combined with
the reference-shape A/B comparison. The A/B difference is only the final
presentation mapping of the already-projected endpoint sets: the diagnostic
uses one shared anisotropic gain and a far-set-only translation; the control
uses the original shared provisional map.

Compare against the supplied Level 7 reference shape for:

- outer lobe and notch proportions;
- near-rim shape;
- far-rim relative position;
- rail direction and convergence; and
- overall normalized correspondence.

The candidate is a single bounded fit, not a tuning interface. The fitted
parameters are based on screenshot raster comparison and cannot establish
historical Atari display behavior. Neal's manual visual acceptance is
**PENDING**.
