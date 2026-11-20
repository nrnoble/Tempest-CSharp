# Level 4 Unit-Domain Correction Candidate

## Outcome

**READY FOR LEVEL 4 MANUAL RECHECK**

Level 4's opt-in candidate now projects C# model coordinates in their original unit domain. Point-by-point legacy equivalence passed at near, middle, and far depth, and all required automated validation completed successfully. This is a corrected baseline, not manual presentation acceptance.

## Starting state

- Branch: `master`
- HEAD: `385f603a94391756b2ca6cccd524dc17b281e782`
- Upstream: `origin/master`
- Index: empty.
- Pre-existing tracked modifications: `src/Tempest.Game/Debug/DebugController.cs`, `src/Tempest.Game/Debug/PlaybackScreenshotCapture.cs`, `src/Tempest.Game/Levels/LevelLoader.cs`, `src/Tempest.Game/Program.cs`, `src/Tempest.Game/Rendering/DisplayConfiguration.cs`, `src/Tempest.Game/Rendering/Projection.cs`, `src/Tempest.Game/Rendering/VectorRenderer.cs`, `src/Tempest.Game/Systems/InputSystem.cs`, `tests/Tempest.Game.Tests/DebugControllerTests.cs`, `tests/Tempest.Game.Tests/DisplayConfigurationTests.cs`, `tests/Tempest.Game.Tests/GameOverDensityDiagnosticTests.cs`, `tests/Tempest.Game.Tests/GameplayRecordingTests.cs`, `tests/Tempest.Game.Tests/InputSystemTests.cs`, `tests/Tempest.Game.Tests/Level9ProductionTests.cs`, `tests/Tempest.Game.Tests/PlaybackScreenshotCaptureTests.cs`, and `tests/Tempest.Game.Tests/TransitionPresentationTests.cs`.
- Pre-existing untracked paths: `Reference/arcade/MBUCOD.V05.txt`, `Reference/arcade/MBUDOC.DOC`, `screenshot000.png`, `src/Tempest.Game/Levels/RomPresentationMetadata.cs`, `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`, `tests/Tempest.Game.Tests/AtariPresentationAdapterTests.cs`, `tests/Tempest.Game.Tests/RomPresentationMetadataTests.cs`, and `tests/Tempest.Game.Tests/VectorRendererAtariPresentationTests.cs`.

The existing work was preserved. The image path appeared only in the required Git status inventory; no visual asset was opened, searched, or analyzed.

## Unit-domain correction

The previous candidate converted already-C# model displacements to Atari comparison units and then passed those values to a projection expecting C# model units. Its `1/aX` and `1/aY` presentation gains were therefore unintended.

The Level 4 branch now projects the original `x` and `y` values directly with the established C# depth scale. `AtariModelFrameMapping` remains available with the fitted correspondence values (`aX=2.081778520`, `aY=2.231150659`, references `128` and `121.501111`, positive orientations), but its coefficients are explicitly documented as correspondence metadata, not presentation gains.

For the required ordinary state, the branch computes `K / (depth + ZOffset)`, then `400 + x * scale` and `CenterY + (y + YOffset) * scale`; both local scales equal that depth scale. No aspect multiplier, placement translation, or depth change was introduced.

## Legacy-equivalence evidence

The adapter test checks all 16 production points at depths `50`, `250`, and `450` against both `Projection.Project` and the specified formula. Each X/Y difference must be at most `0.0001`; every comparison passed. It also checks X/Y bounds, width, height, projected bounding-box centers, local X/Y derivatives, origin projection at each depth, near/far size ratios, and center progression.

| Depth | X bounds | Y bounds | Width × height | Bounding-box center | X/Y local scale |
|---:|---:|---:|---:|---:|---:|
| 50 (near) | 141.182..658.818 | 166.091..474.545 | 517.636 × 308.455 | (400.000, 320.318) | 1.181818 |
| 250 (middle) | 344.176..455.824 | 333.863..400.392 | 111.647 × 66.529 | (400.000, 367.127) | 0.254902 |
| 450 (far) | 368.714..431.286 | 354.143..391.429 | 62.571 × 37.286 | (400.000, 372.786) | 0.142857 |

Near/far width and height ratios are both `8.272727`. Bounding-box center Y progresses `52.468` logical units near-to-far; the projected origin Y progresses `67.532`. Candidate and legacy values agreed within the stated tolerance.

## Protected presentation state

`CenterY=380`, `YOffset=-65`, `Position=50`, `Depth=400`, C# `K/(depth+ZOffset)` progression, shared portrait placement `-33.1`, portrait zoom, and viewport dimensions remain unchanged. No Level 4 placement, perspective, geometry, aspect treatment, or visual tuning was added.

## Gameplay/model protection

No gameplay or model source was changed. Adapter tests preserve the 16-point snapshot, point order, count, lanes, closed topology, and configuration. Renderer tests confirm repeated output and unchanged game state; Level 4 tube and world-anchor checks cover player lanes, eggs, Tankers, Flippers, enemy bullets, Fuseballs, Spikes, and player-shot artwork using the corrected projection/depth scale.

## Other-level preservation

No Level 1-3 output or mapping was changed. Levels 5-6 remain fallback; Levels 7-9 and Game Over remain on their existing paths. Approach, transitions, screenshots, input, recording, and playback code were not changed. Focused tests for these areas and cross-level candidate/fallback behavior passed.

## Tests and validation

All test commands used the established VSTest `--blame-hang-timeout 2m` protection.

- Level 4 adapter formula, mapping, and point-by-point gate (**9 passed**):
  `dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~Level4CandidateMatchesLegacyPointByPointAtNearMiddleAndFarDepths|FullyQualifiedName~Level4UsesPositiveIndependentAxisFrameAndPreservesItsPlacementBaseline|FullyQualifiedName~NearMidAndFarProjectionsMatchTheContinuousContract" --blame-hang-timeout 2m`
- Level 4 tube/world-anchor focused tests (**14 passed**):
  `dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~Level4CandidateMatchesLegacyTubeProjectionWithoutChangingItsPlacementBaseline|FullyQualifiedName~SameDepthLaneInterpolationsStayRegisteredAndCandidateShotsUseTheAdapterScale|FullyQualifiedName~CandidateTubeKeepsPointOrderTopologyAndUsesOnlyEstablishedMappings" --blame-hang-timeout 2m`
- Full adapter suite (**38 passed**):
  `dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~AtariPresentationAdapterTests" --blame-hang-timeout 2m`
- Full renderer/world-anchor suite (**23 passed**):
  `dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~VectorRendererAtariPresentationTests" --blame-hang-timeout 2m`
- Combined adapter/renderer cross-level regressions, including accepted Levels 1-3 and Levels 5-9 candidate/fallback coverage (**61 passed**):
  `dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~AtariPresentationAdapterTests|FullyQualifiedName~VectorRendererAtariPresentationTests" --blame-hang-timeout 2m`
- Level 4 gameplay, Approach, transitions, playback, recording, screenshots, and input regressions (**253 passed**):
  `dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~Level4CandidateTests|FullyQualifiedName~LevelApproachTests|FullyQualifiedName~TransitionPresentationTests|FullyQualifiedName~GameplayPlaybackTests|FullyQualifiedName~GameplayRecordingTests|FullyQualifiedName~PlaybackScreenshotCaptureTests|FullyQualifiedName~HeadlessGameplayPlaybackTests|FullyQualifiedName~GameplayPlaybackSessionTests|FullyQualifiedName~VisiblePlaybackControllerTests|FullyQualifiedName~InputSystemTests" --blame-hang-timeout 2m`
- Full solution (**1,492 passed, 0 failed, 0 skipped**):
  `dotnet test .\TempestCSharp.slnx --no-restore --blame-hang-timeout 2m`
- Release build: **PASS**, 0 warnings, 0 errors:
  `dotnet build .\TempestCSharp.slnx --configuration Release --no-restore`
- `git diff --check`: **PASS**, exit code 0. Git emitted only existing LF-to-CRLF notices for `LevelLoader.cs` and `InputSystem.cs`.

The first renderer comparison used exact `Vector2` equality and caught an approximately `0.00002` rounding difference between algebraically equivalent projection arithmetic. The check was changed to a strict `0.0001` positional tolerance; focused renderer/world-anchor tests and all subsequent suites passed. No material projection disagreement remains.

## Deviations

None. Only `AtariPresentationAdapter.cs`, `AtariPresentationAdapterTests.cs`, and `VectorRendererAtariPresentationTests.cs` were edited for the correction. The two authorized ignored AI-Logs artifacts were created; no other AI-Logs file was created.

## Limitations

Numerical legacy equivalence establishes a corrected Level 4 baseline only. It does not establish arcade-fidelity acceptance or acceptance of any Level 4 visual characteristic; Neal's fresh manual evaluation remains required.

## Manual command

dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 4 --atari-presentation

## First manual question

Is the corrected Level 4 candidate now usable enough for Neal to evaluate its presentation normally?

## Git state

Final verification retained branch `master`, HEAD `385f603a94391756b2ca6cccd524dc17b281e782`, and upstream `origin/master`. The index remained empty; no files were staged, committed, or pushed. All pre-existing tracked modifications and untracked paths were preserved. Both AI-Logs files are ignored by `.gitignore`.
