# Level 7 record-06 presentation implementation candidate

Date: 2026-11-16

## Outcome

Promoted the manually promising reference-shape presentation mapping into the
opt-in Level 7 Atari presentation path. With `--atari-presentation --level 7`
while Level 7 is in `Playing`, the tube now uses Atari record 06, its existing
near/far Atari projection, and the shared calibrated presentation mapping.
Level 7 gameplay geometry and simulation state remain unchanged. The candidate
is presentation-only and **final manual visual acceptance is PENDING**.

The candidate applies only during ordinary `Playing`. Existing tube dispatch
for Approach, exit, and other states remains unchanged.

## Implementation and evidence

- Added `AtariLevel7Presentation` as the shared mapping authority. The prior
  `AtariTubeReferenceShapeDiagnostic` now delegates to it, so the diagnostic
  and production candidate cannot drift.
- Kept one mapped `Near[i]`/`Far[i]` endpoint pair for each straight rail and
  reused those same mapped points for the corresponding cyclic rim edges.
  Existing rail color and pulsing-rim behavior is retained.
- Mapping: horizontal gain `0.4215`, vertical gain `0.575`, shared aspect
  `0.7330435`, global scale `0.345`, near anchor `(300, 400)`, far translation
  `(+33.75, +46.25)` to anchor `(333.75, 446.25)`.
- **ROM-BACKED:** record 06 bytes and its metadata retain their existing
  classification. Record 06, its point order/topology, near runtime position
  `$10`, far runtime position `$F0`, and projector mathematics were not changed.
- **PROJECT TRANSLATION / visually calibrated candidate:** the anisotropic
  gains and far-set translation are presentation choices, not ROM geometry or
  demonstrated Atari hardware behavior. Neal's diagnostic result, “The
  reference-shape diagnostic looks pretty good,” supports trying this candidate
  but is not final acceptance.
- The factor-128 RN=$000F diagnostic remains opt-in DEVELOPMENT-ONLY and is
  not used by the production Level 7 candidate.
- Durable behavior documentation was not changed; it contains no stale Level 7
  presentation-candidate status entry requiring correction.

## Files changed for this milestone

- `src/Tempest.Game/Rendering/AtariLevel7Presentation.cs` (added)
- `src/Tempest.Game/Rendering/AtariTubeReferenceShapeDiagnostic.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Debug/DebugController.cs`
- `tests/Tempest.Game.Tests/AtariTubeRecordPrototypeTests.cs`
- `tests/Tempest.Game.Tests/VectorRendererAtariPresentationTests.cs`

These files were already part of the pre-existing dirty worktree except for
the new `AtariLevel7Presentation.cs`; their pre-existing work was preserved.
No screenshots, video, or other image assets were inspected.

## Diagnostic and manual controls

- **Implementation candidate:** `--atari-presentation --level 7`.
- `--atari-tube-reference-shape-diagnostic` remains available with
  `--atari-tube-prototype --level 7` as a DEVELOPMENT-ONLY control using the
  same Level 7 mapping.
- `--atari-tube-prototype --level 7` remains available as the old provisional
  viewport control.
- `--atari-tube-rn000f-diagnostic` remains a separate opt-in DEVELOPMENT-ONLY
  experiment and does not affect the implementation candidate.

## Validation

- Focused and targeted Level 7, Levels 1-9 presentation selection, direct-Atari
  prototype, Atari-presentation, CLI diagnostics, factor-128 isolation,
  recording/playback, display configuration, and rendering-selection
  regressions: **PASS**, 530 passed, 0 failed.
  Command:
  `dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~AtariTubeRecordPrototypeTests|FullyQualifiedName~VectorRendererAtariPresentationTests|FullyQualifiedName~AtariPresentationAdapterTests|FullyQualifiedName~DebugControllerTests|FullyQualifiedName~GameplayRecordingTests|FullyQualifiedName~GameplayPlayback|FullyQualifiedName~HeadlessGameplayPlayback|FullyQualifiedName~VisiblePlaybackController|FullyQualifiedName~DisplayConfigurationTests|FullyQualifiedName~Level7|FullyQualifiedName~DirectLevelStartupTests|FullyQualifiedName~TransitionPresentationTests|FullyQualifiedName~PlaybackScreenshotCaptureTests|FullyQualifiedName~RomPresentationMetadataTests" --blame-hang-timeout 3m --verbosity minimal`
- Full solution tests:
  `dotnet test TempestCSharp.slnx --no-restore --blame-hang-timeout 5m --verbosity minimal`
  — **PASS**, 1,563 passed, 0 failed.
- Release solution build:
  `dotnet build TempestCSharp.slnx --no-restore --configuration Release --verbosity minimal`
  — **PASS**, 0 warnings, 0 errors.
- `git diff --check` — **PASS**. Git emitted existing LF-to-CRLF notices for
  `LevelLoader.cs` and `InputSystem.cs`.
- An initial targeted run exposed one Level 7 presentation test still
  expecting the prior model-geometry tube. Its expectation was updated to
  assert the record-06 mapped endpoints; the targeted regressions and full
  suite then passed.

No interactive game was launched.

## Git state

Starting state:

- Branch: `master`
- HEAD: `385f603a94391756b2ca6cccd524dc17b281e782`
- Upstream: `origin/master`
- Index: empty
- Tracked worktree: 17 pre-existing modified files, 2,894 insertions and 177
  deletions in the starting diff.
- Modified tracked paths (same set at start and end):
  `.github/copilot-instructions.md`,
  `src/Tempest.Game/Debug/DebugController.cs`,
  `src/Tempest.Game/Debug/PlaybackScreenshotCapture.cs`,
  `src/Tempest.Game/Levels/LevelLoader.cs`,
  `src/Tempest.Game/Program.cs`,
  `src/Tempest.Game/Rendering/DisplayConfiguration.cs`,
  `src/Tempest.Game/Rendering/Projection.cs`,
  `src/Tempest.Game/Rendering/VectorRenderer.cs`,
  `src/Tempest.Game/Systems/InputSystem.cs`,
  `tests/Tempest.Game.Tests/DebugControllerTests.cs`,
  `tests/Tempest.Game.Tests/DisplayConfigurationTests.cs`,
  `tests/Tempest.Game.Tests/GameOverDensityDiagnosticTests.cs`,
  `tests/Tempest.Game.Tests/GameplayRecordingTests.cs`,
  `tests/Tempest.Game.Tests/InputSystemTests.cs`,
  `tests/Tempest.Game.Tests/Level9ProductionTests.cs`,
  `tests/Tempest.Game.Tests/PlaybackScreenshotCaptureTests.cs`, and
  `tests/Tempest.Game.Tests/TransitionPresentationTests.cs`.
- Pre-existing untracked paths:
  `Reference/arcade/MBUCOD.V05.txt`, `Reference/arcade/MBUDOC.DOC`,
  `screenshot000.png`,
  `src/Tempest.Game/Levels/AtariTubeRecordCatalog.cs`,
  `src/Tempest.Game/Levels/RomPresentationMetadata.cs`,
  `src/Tempest.Game/Rendering/AtariCoordinateProjector.cs`,
  `src/Tempest.Game/Rendering/AtariDvgViewportMap.cs`,
  `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`,
  `src/Tempest.Game/Rendering/AtariTubeReferenceShapeDiagnostic.cs`,
  `src/Tempest.Game/Rendering/AtariTubeVectorListBuilder.cs`,
  `tests/Tempest.Game.Tests/AtariPresentationAdapterTests.cs`,
  `tests/Tempest.Game.Tests/AtariTubeRecordPrototypeTests.cs`,
  `tests/Tempest.Game.Tests/RomPresentationMetadataTests.cs`, and
  `tests/Tempest.Game.Tests/VectorRendererAtariPresentationTests.cs`.

Ending state:

- Branch, HEAD, and upstream unchanged.
- Index remains empty; no files were staged, committed, or pushed.
- The same 17 tracked paths remain modified; the tracked diff is 2,943
  insertions and 177 deletions.
- The same pre-existing untracked paths remain, with the added
  `src/Tempest.Game/Rendering/AtariLevel7Presentation.cs`.
- Required prompt and report are under ignored `AI-Logs/`; neither was staged.

## Protected levels and manual acceptance

Levels 1-6 and 8-9 were not retuned. Existing gameplay Level 7 tests, recording
and playback behavior, and option-off/legacy behavior remain covered by the
passing regressions.

Neal's exact manual test command:

```powershell
dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation --level 7
```

Run this command to assess the implementation candidate. **Final Level 7
visual acceptance: PENDING Neal's manual test.**
