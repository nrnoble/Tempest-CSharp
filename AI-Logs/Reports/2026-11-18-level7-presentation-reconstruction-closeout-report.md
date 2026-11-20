# Level 7 Atari Presentation Reconstruction — Closeout Report

## Status

**PASS — manually accepted, validated, committed, and pushed.**

## Manual acceptance

Neal's authoritative result:

> The severe Level 7 Atari-presentation bug is resolved. Level 7 now plays as expected. Enemy movement from deep to near appears normal. Player-shot movement from near to deep appears normal. Rim movement appears normal. Tube and gameplay entities remain registered. The reconstructed tube structure is acceptable. Level 7 geometry does not need further correction. No further Level-7-specific presentation work is wanted for this milestone.

Neal also compared C# and MAME/arcade-reference captures side by side. They show the same recognizable Level 7 structure, without accepted evidence of incorrect point order, connectivity, lane structure, or rail attachment. The C# capture appears horizontally broader relative to its height. This is a separate whole-game viewport/screenshot-proportion issue, not a Level 7 geometry defect. The captures are not pixel-identical.

## Accepted Level 7 presentation

- `LevelConfiguration.Level7Production` and its `LevelLoader.LoadLevel7Clover()` definition remain the single production geometry authority: 16 unchanged ordered points and closed 16-lane topology.
- The production clover remains **REFERENCE-DERIVED** and is not claimed to be a literal conversion of Atari record 06.
- Tube and gameplay entities use the same production geometry and `AtariPresentationAdapter`; its Jacobian remains the local-scale authority. The consistent final translation is applied once, followed by the common portrait camera.
- `Level7VerticalPlacement = 228.99702f` is the accepted **PROJECT TRANSLATION**. Horizontal correction is zero.
- Normal Flippers continue to use gameplay-selected `DirectedLaneTransition`, production geometry, `CalculateFlipperPivotEndpoints`, and shared adapter projection. Ordinary and cargo Flippers follow the same route.
- The failed active record-06 two-geometry architecture and record-lane entity projection are removed. Raw record-06 coordinates, metadata, ordering, closed topology, catalog data, Level 7 ROM presentation metadata, and raw-data/provenance coverage remain preserved as ROM/Atari-backed evidence. Record 06 does not drive the ordinary visible Level 7 tube.
- No gameplay geometry/rules, movement, timing, RNG, recording/playback simulation, or deterministic behavior changed. Rendering remains state-read-only.
- Accepted Levels 1–6, Level 8 `+212.28139`, Level 9 `+180.96825`, diagnostic Level 10 `+50`, Blaster orientation/color/travel scaling, ExitZoom focal compensation, Approach, transitions, and option-off/legacy behavior remain frozen.

## Removed prototype and diagnostic components

Removed prototype/record-06 presentation code:

- `AtariLevel7Presentation`
- `AtariLevel7EntityPresentation`
- `AtariTubeVectorListBuilder`
- `AtariCoordinateProjector`
- `AtariDvgViewportMap`
- `AtariTubeReferenceShapeDiagnostic`
- Their obsolete presentation/prototype tests and CLI/runtime routes

Removed the now-retired `--level7-visible-runtime-diagnostic` and `--level7-framebuffer-diagnostic` options, their `DebugController` parsers and `Program` wiring, all dedicated `VectorRenderer` instrumentation hooks, `Level7VisibleRuntimeDiagnostic`, `Level7FramebufferDiagnostic`, and their dedicated tests. The options now report that they were removed and exit with code 2 before game startup. Generic recording/playback, screenshot capture, pause, stepping, and shared diagnostics remain.

## Durable documentation

Updated:

- `TEMPEST-DEVELOPMENT-NOTES.md`
- `TEMPEST-BEHAVIOR-SPEC.md`

Both record manual acceptance, the shared production architecture, evidence classifications, preserved record-06 data, accepted placement, frozen gameplay/shared systems, and the separate deferred whole-game viewport/screenshot-proportion milestone. Neither treats the broader comparison appearance as a Level 7 geometry defect.

## Validation

1. **PASS — focused Level 7 shared-presentation tests:** 9 passed, 0 failed.

   `dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~Level7AtariPresentationTests" --blame-hang-timeout 4m --verbosity minimal`

2. **PASS — targeted Level 7 gameplay/presentation regressions:** 160 passed, 0 failed.

   `dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~Level7AtariPresentationTests|FullyQualifiedName~Level7ProductionTests|FullyQualifiedName~Level7AdmissionPolicyTests|FullyQualifiedName~Level7FlipperMovementTests|FullyQualifiedName~Level7SpikerFiringTests|FullyQualifiedName~GameplayRecordingTests|FullyQualifiedName~GameplayPlayback" --blame-hang-timeout 5m --verbosity minimal`

3. **PASS — targeted shared/cross-level presentation regressions:** 179 passed, 0 failed.

   `dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~AtariPresentationAdapterTests|FullyQualifiedName~VectorRendererAtariPresentationTests|FullyQualifiedName~Level8AtariPresentationTests|FullyQualifiedName~Level9AtariPresentationTests|FullyQualifiedName~DiagnosticLevel10Tests|FullyQualifiedName~BlasterPresentationTests|FullyQualifiedName~BlasterScaleRegressionTests|FullyQualifiedName~ExitZoomFocalCompensationTests|FullyQualifiedName~TransitionPresentationTests|FullyQualifiedName~DirectedRotationInvestigationTests" --blame-hang-timeout 5m --verbosity minimal`

4. **PASS — full solution tests:** 1,585 passed, 0 failed.

   `dotnet test TempestCSharp.slnx --no-restore --configuration Release --blame-hang-timeout 5m --verbosity minimal`

5. **PASS — Release build:** 0 warnings, 0 errors.

   `dotnet build TempestCSharp.slnx --no-restore --configuration Release --verbosity minimal`

6. **PASS — `git diff --check` and `git diff --cached --check`.**
7. **PASS — retired diagnostic switches:** both were rejected before game startup with exit code 2.
8. **PASS — preservation checks:** production geometry/data files were unchanged; no source/test consumers of removed mappers or diagnostic classes remain. Retained tests assert the production points, record-06 raw X/Y bytes and metadata, raw-data immutability, shared presentation route, placement, directed Flipper pivots, state-read-only rendering, and playback equivalence.

## Staging review

The staged diff was reviewed as a Level 7 reconstruction/diagnostic-cleanup milestone. Every deletion was limited to the failed Level 7 prototype or dedicated retired diagnostics/tests; the source/test search found no remaining runtime consumers. `git diff --cached --check` passed.

Exact staged/committed file list:

- `TEMPEST-BEHAVIOR-SPEC.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `src/Tempest.Game/Debug/DebugController.cs`
- `src/Tempest.Game/Debug/Level7FramebufferDiagnostic.cs` (deleted)
- `src/Tempest.Game/Debug/Level7VisibleRuntimeDiagnostic.cs` (deleted)
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Rendering/AtariCoordinateProjector.cs` (deleted)
- `src/Tempest.Game/Rendering/AtariDvgViewportMap.cs` (deleted)
- `src/Tempest.Game/Rendering/AtariLevel7EntityPresentation.cs` (deleted)
- `src/Tempest.Game/Rendering/AtariLevel7Presentation.cs` (deleted)
- `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`
- `src/Tempest.Game/Rendering/AtariTubeReferenceShapeDiagnostic.cs` (deleted)
- `src/Tempest.Game/Rendering/AtariTubeVectorListBuilder.cs` (deleted)
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `tests/Tempest.Game.Tests/AtariLevel7EntityPresentationTests.cs` (deleted)
- `tests/Tempest.Game.Tests/AtariPresentationAdapterTests.cs`
- `tests/Tempest.Game.Tests/AtariTubeRecordPrototypeTests.cs` (deleted; raw-data coverage retained in `Level7AtariPresentationTests.cs`)
- `tests/Tempest.Game.Tests/DebugControllerTests.cs`
- `tests/Tempest.Game.Tests/DiagnosticLevel10Tests.cs`
- `tests/Tempest.Game.Tests/ExitZoomFocalCompensationTests.cs`
- `tests/Tempest.Game.Tests/Level7AtariPresentationTests.cs`
- `tests/Tempest.Game.Tests/Level7FramebufferDiagnosticTests.cs` (deleted)
- `tests/Tempest.Game.Tests/Level7VisibleRuntimeDiagnosticTests.cs` (deleted)
- `tests/Tempest.Game.Tests/Level8AtariPresentationTests.cs`
- `tests/Tempest.Game.Tests/Level9AtariPresentationTests.cs`
- `tests/Tempest.Game.Tests/VectorRendererAtariPresentationTests.cs`

Excluded and preserved unrelated local work: `.github/copilot-instructions.md`, `Reference/arcade/MBUCOD.V05.txt`, and `Reference/arcade/MBUDOC.DOC`. `AI-Logs/` was not staged or committed.

## Commit and push

- Commit: `7ba80f39df9daeaa86a7cafc81690f2f5c37bb33`
- Message: `Close out Level 7 Atari presentation reconstruction`
- Push: **PASS** — `master` pushed to existing upstream `origin/master`.

Final verification:

- Branch: `master`
- HEAD: `7ba80f39df9daeaa86a7cafc81690f2f5c37bb33`
- Upstream: `origin/master`
- Ahead/behind: `0/0`
- Index: clean
- Accepted reconstruction changes remaining unstaged: none
- Unrelated local work remains preserved and unstaged/untracked as before.

## AI-Logs

- Prompt: `AI-Logs/Prompts/2026-11-18-level7-presentation-reconstruction-closeout-prompt.md`
- Report: `AI-Logs/Reports/2026-11-18-level7-presentation-reconstruction-closeout-report.md`
