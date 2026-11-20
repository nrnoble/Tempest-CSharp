# Level 1/7 tube Y-gain diagnostic implementation

**Status: AUTOMATED VALIDATION PASS — MANUAL ACCEPTANCE PENDING.** This is a
DEVELOPMENT-ONLY outer-cross-section experiment. No gain above `1.00` is
accepted production behavior, no value is selected, and the Level 7
inner/core/perspective relationship remains unresolved and unmodified.

## Implementation

- **CLI:** `--tube-y-gain <value>` defaults to exactly `1.00`; accepts finite
  invariant-culture values from `1.00` through `1.32`, inclusive. `1`, `1.00`,
  `1.21`, `1.23`, `1.31`, and `1.32` are valid. Missing, malformed,
  duplicate, non-finite, and out-of-range values are rejected; the value must
  be a separate argument (`--tube-y-gain=...` is not accepted).
- Requires `--atari-presentation` and explicit `--level 1` or `--level 7`.
  Visible `--playback-file` is also allowed only when the loaded recording
  starts on Level 1 or 7; startup-level validation occurs before window
  creation. The active adapter applies the gain only while Level 1 or 7 is
  displayed. Headless playback remains incompatible with Atari presentation.
- **Exact model transform:** only at the adapter seam, before depth projection:
  `x' = x`, `y' = 0 + gain * (y - 0)`, `z' = z`, for displayed Levels 1 and 7.
  The source-space anchor is `(0,0)`. `CenterY`, `YOffset`, Approach offsets,
  depth, reciprocal projection, placement, and viewport are not scaled.
- **Local basis:** `LocalScaleX` and scalar `LocalScale` remain unchanged;
  `LocalScaleY` is multiplied by the gain for Levels 1/7. Their local
  presentation offsets use `(1, gain)`. This preserves screen-oriented scalar
  sizing while propagating the Y Jacobian to tube-oriented geometry.
- **Registration and state behavior:** tube points and entity anchors continue
  through the shared `VectorRenderer.ProjectWorldPoint` / active adapter path.
  The Blaster retains its projected rim anchor, outward-facing calculation,
  ordinary Playing size, and Approach/ExitZoom scalar normalization; positive
  gains do not invert its normal. Shots, Spikes, enemies, Flippers, and
  transitions retain their simulation depth/lane/timing and use that shared
  projection. No gameplay geometry or entity state is mutated.
- **Approach:** model cross-section Y uses the same gain; `ApproachDepthOffset`
  and `ApproachVerticalOffset` remain separate, unscaled projection inputs.
  Automated adapter coverage uses nonzero Approach offsets.
- **ExitZoom:** the frozen closed-tube far-rim-center rule is unchanged. Its
  existing focal callback uses the active adapter, so the latched pivot is
  computed from the gain-transformed mapped rim. The focal compensation test
  now exercises gain `1.31` on Levels 1 and 7 and confirms the opening remains
  fixed.
- **Recording/playback:** the gain is not stored in `GameplayStartupConfiguration`
  or recording schema. Identical replay sessions at gains `1.00` and `1.31`
  retain equal state summaries and playback boundaries.
- **Controls/protection:** Levels 2 and 5 rendering tests compare equal output
  at gains `1.00` and `1.31`; the same regression covers Levels 3, 4, 6, 8, 9,
  and diagnostic 10. Level 2's `1.314` factor, Level 5's `.670/.880` gains,
  all placements, and the frozen viewport are unchanged.

## Files changed

Project source and tests:

- `src/Tempest.Game/Debug/DebugController.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `tests/Tempest.Game.Tests/TubeYGainTests.cs`
- `tests/Tempest.Game.Tests/ExitZoomFocalCompensationTests.cs`

Required ignored task records:

- Prompt: `AI-Logs/Prompts/2026-11-19-level1-level7-tube-y-gain-diagnostic-prompt.md`
- Report: `AI-Logs/Reports/2026-11-19-level1-level7-tube-y-gain-diagnostic-report.md`

No behavior spec or development notes were changed because the experiment is
not manually accepted behavior.

## Validation

The test platform is VSTest with xUnit v2 on .NET SDK `9.0.304`. Build and test
outputs used the isolated path
`%TEMP%\TempestTubeYGain-20261119` because the repository's normal
`Tempest.Game.exe` output was locked by an already-running `Tempest.Game`
process. That process was not launched or terminated by this task. The isolated
build-output directory was removed after validation.

- **Focused — PASS, 103 tests:** `dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --artifacts-path "$env:TEMP\TempestTubeYGain-20261119" --filter "FullyQualifiedName~TubeYGainTests|FullyQualifiedName~AtariPresentationAdapterTests|FullyQualifiedName~ExitZoomFocalCompensationTests" --blame-hang-timeout 2m`
- **Targeted regressions — PASS, 712 tests:** covered the new gain tests, Atari adapter/renderer, Levels 1/2/5/7/8/9/10, Blaster, transitions/Flippers, shots, Approach, ExitZoom, playback/recording, and display configuration.

  ```powershell
  dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --artifacts-path "$env:TEMP\TempestTubeYGain-20261119" --filter "FullyQualifiedName~TubeYGainTests|FullyQualifiedName~AtariPresentationAdapterTests|FullyQualifiedName~VectorRendererAtariPresentationTests|FullyQualifiedName~BlasterPresentationTests|FullyQualifiedName~BlasterScaleRegressionTests|FullyQualifiedName~BlasterLaneHighlightTests|FullyQualifiedName~Level1|FullyQualifiedName~Level2|FullyQualifiedName~Level5|FullyQualifiedName~Level7|FullyQualifiedName~Level8AtariPresentationTests|FullyQualifiedName~Level9AtariPresentationTests|FullyQualifiedName~DiagnosticLevel10Tests|FullyQualifiedName~ExitZoomFocalCompensationTests|FullyQualifiedName~LevelApproachTests|FullyQualifiedName~TransitionPresentationTests|FullyQualifiedName~DirectedRotationInvestigationTests|FullyQualifiedName~ArcadeFlipperFiringTests|FullyQualifiedName~PlayerShotRenderingTests|FullyQualifiedName~GameplayPlayback|FullyQualifiedName~GameplayRecordingTests|FullyQualifiedName~HeadlessGameplayPlaybackTests|FullyQualifiedName~VisiblePlaybackControllerTests|FullyQualifiedName~DisplayConfigurationTests" --blame-hang-timeout 5m
  ```
- **Full solution — PASS, 1,622 tests:** `dotnet test TempestCSharp.slnx --artifacts-path "$env:TEMP\TempestTubeYGain-20261119" --blame-hang-timeout 5m`
- **Release solution build — PASS:** `dotnet build TempestCSharp.slnx --configuration Release --artifacts-path "$env:TEMP\TempestTubeYGain-20261119"`; 0 warnings, 0 errors.
- **CLI smoke checks — PASS:** missing `--atari-presentation` and unsupported Level 2 invocations both exited with code 2 before window startup.
- **Whitespace — PASS:** `git diff --check` and a no-index whitespace check of the new test file reported no errors.
- **Manual game acceptance — PENDING.** The game was not launched and no image/video analysis was performed.

The first test attempt against the default output was **BLOCKED** by the
pre-existing process holding `src\Tempest.Game\bin\Debug\net9.0\Tempest.Game.exe`
open. The isolated-output reruns above completed successfully; no retry
targeted or modified the locked executable.

## Repository state

| Field | Start | Finish |
|---|---|---|
| Branch | `master` | `master` |
| HEAD | `c8bc40416eff4ffaac17862f7c894e5a34d133f9` | unchanged |
| Upstream | `origin/master` | `origin/master` |
| Ahead / behind | `0 / 0` | `0 / 0` |
| Index | empty | empty |
| Pre-existing tracked worktree | `.github/copilot-instructions.md` modified | unchanged; preserved |
| Pre-existing untracked reference files | `Reference/arcade/MBUCOD.V05.txt`, `Reference/arcade/MBUDOC.DOC` | unchanged; preserved |
| Task source/test changes | none | listed above |

Nothing was staged, committed, pushed, reset, restored, stashed, cleaned, or
rebased. User/reference assets were not opened or modified.

## Manual A/B handoff

Run each command as a separate session. Compare ordinary Playing, outer
proportion, horizontal center, vertical placement, tube/entity registration,
Blaster rim attachment and outward direction, enemy lanes, shots/Spikes,
Flipper transitions, exit, Approach, and gameplay/timing. Decide separately
which gain, if any, improves each level, and whether Level 7's inner/core
relationship remains wrong. These commands do not imply that any non-identity
gain is accepted.

**Level 1**

```text
dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --level 1 --atari-presentation
dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --level 1 --atari-presentation --tube-y-gain 1.00
dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --level 1 --atari-presentation --tube-y-gain 1.21
dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --level 1 --atari-presentation --tube-y-gain 1.23
dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --level 1 --atari-presentation --tube-y-gain 1.31
```

**Level 7**

```text
dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --level 7 --atari-presentation
dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --level 7 --atari-presentation --tube-y-gain 1.00
dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --level 7 --atari-presentation --tube-y-gain 1.21
dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --level 7 --atari-presentation --tube-y-gain 1.23
dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --level 7 --atari-presentation --tube-y-gain 1.31
```

**Unaffected controls (without `--tube-y-gain`)**

```text
dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --level 2 --atari-presentation
dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --level 5 --atari-presentation
```

**Known limitation:** the experiment tests only outer cross-section
proportion. It does not change or resolve Level 7 far/core-center displacement,
notch-to-core relationship, rail convergence, depth projection, or portrait
perspective. Manual acceptance remains **PENDING**.
