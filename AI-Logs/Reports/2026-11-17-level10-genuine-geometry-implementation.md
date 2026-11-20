# Diagnostic Level 10 genuine-geometry implementation — PASS

## Outcome

Implemented Experiment 2: Level-7-like diagnostic gameplay now runs on displayed
Level 10's recovered ROM-backed shape-row-9 geometry. Level 10 is still a
development-only direct start, not an authentic arcade Level 10 gameplay
configuration. Experiment 1 and the Level 7 Atari-presentation path remain
unchanged. No gameplay launch or manual motion conclusion was made.

**Manual motion comparison: PENDING Neal.**

## Geometry and topology

`LevelLoader.Level10RomEvidence` records the established Rev. 2 table values:

- `lev_remap[9] = 0x09`, selecting shape row 9.
- Ordered X: `10 10 10 10 16 29 46 69 97 BA D7 EA F0 F0 F0 F0`.
- Ordered Y: `F0 CB A6 80 5C 39 20 12 12 20 39 5C 80 A6 CB F0`.
- Ordered angles: `0C 0C 0C 0D 0E 0F 0F 00 01 01 02 03 04 04 04 00`.
- Raw presentation metadata: scale `0x18`, Y3D `0x50`, Y2D low/high
  `0x20/0xFF` (`0xFF20`, signed `-224`), and open flag `0xFF`.

The model definition preserves all 16 source points in order and applies the
existing open-shape project translation (center at raw coordinate 128, scale
`25/14`, reflect Y). It sets `Continuous=false`, yielding 15 playable adjacent
segments. The renderer test verifies that neither near nor far rim joins
point 15 back to point 0.

Level 10 now owns a separate `LevelDefinition`; it does not share or compare
equal to Level 7's definition. The 15-lane open topology requires one gameplay
adaptation: its spawn sequence is `0..14`, excluding the nonexistent lane 15.
The existing Level topology handles non-wrapping movement and creates 15
starting Spikes from that lane count. No other geometry-forced gameplay
adaptation was made.

## Gameplay and presentation

The diagnostic configuration continues to reuse Level 7's enemy supply and
composition, egg/timing configuration, Flipper configuration and movement
program, firing policy/cadence, exit policy, initial lane and Spike-height
characteristics. Its 15-lane spawn sequence is the only intentional
geometry-dependent gameplay change. It remains outside campaign progression
and retains direct startup/restart and recording startup identity.

Atari presentation no longer routes Level 10 through the Level 7 record-06
entity/tube geometry. The record-06 candidate is selected only for displayed
Level 7. Level 10 instead projects its own model definition through the
existing generic Atari adapter, validates its open topology against the
row-9 metadata, and uses the existing raw-shape mapping used for open Levels
8/9 (`25/14`, Y orientation `-1`). Tests compare actual rendered Level 10 tube
coordinates to that projection and confirm the Level 7 candidate remains
selected for Level 7.

**Provisional presentation choice:** Level 10 uses the established Level 8/9
raw-shape normalization and centered open-definition representation. The
existing reciprocal projection consumes the recovered row-9 metadata, but the
resulting placement and visual fidelity have not been calibrated or manually
accepted. The mapping is only intended to make the genuine geometry and
depth-dependent motion observable for this experiment. No fscale interpretation
or final visual adjustment was introduced.

## Files changed for this experiment

- `src/Tempest.Game/Levels/LevelLoader.cs` — Level 10 ROM evidence and
  independent open model geometry.
- `src/Tempest.Game/Levels/LevelConfiguration.cs` — Level-7-like diagnostic
  configuration with the 15-lane spawn adaptation.
- `src/Tempest.Game/Levels/RomPresentationMetadata.cs` — genuine row-9
  presentation metadata; metadata documentation now identifies adapter use as
  a project translation.
- `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs` — Level 10 uses
  the open raw-shape model mapping rather than the Level 7 mapping.
- `src/Tempest.Game/Rendering/VectorRenderer.cs` — record-06 Level 7
  presentation dispatch is exclusive to Level 7.
- `src/Tempest.Game/Debug/DebugController.cs` and
  `src/Tempest.Game/Program.cs` — diagnostic scope and presentation comments.
- `tests/Tempest.Game.Tests/DiagnosticLevel10Tests.cs` — geometry, ordering,
  open topology, gameplay reuse, rendering, startup, and dual-mode playback.
- `tests/Tempest.Game.Tests/RomPresentationMetadataTests.cs` — row-9 metadata
  expectations and diagnostic topology.
- `AI-Logs/Prompts/2026-11-17-level10-genuine-geometry-implementation-prompt.md`
  and this report.

No durable behavior specification was changed. No screenshots, videos, ROM
files, framebuffer tooling, or protected user material were inspected or
modified. The interactive game was not launched.

## Validation

- **PASS — focused tests:**
  `dotnet test tests/Tempest.Game.Tests/Tempest.Game.Tests.csproj --filter "FullyQualifiedName~DiagnosticLevel10Tests|FullyQualifiedName~RomPresentationMetadataTests" --blame-hang-timeout 5min --verbosity minimal`
  Result: 9 passed, 0 failed, 0 skipped.
- **PASS — full regression suite:**
  `dotnet test TempestCSharp.slnx --blame-hang-timeout 5min --verbosity minimal`
  Result: 1,594 passed, 0 failed, 0 skipped.
- **PASS — Release build:**
  `dotnet build TempestCSharp.slnx --configuration Release --no-restore --verbosity minimal`
  Result: 0 warnings, 0 errors.
- **PASS — whitespace:** `git diff --check` returned exit code 0. Git emitted
  only line-ending normalization warnings for the already-modified
  `LevelLoader.cs` and `InputSystem.cs`; it reported no whitespace errors.

The first full-suite attempt found an old untracked metadata test that asserted
Level 10 still aliased Level 7 metadata. That expectation was updated to the
recovered row-9 values; the focused and final full-suite runs then passed.

## Starting and final Git/worktree state

Starting and final repository state:

- Branch: `master`
- HEAD: `385f603a94391756b2ca6cccd524dc17b281e782`
- Upstream: `origin/master`
- Index: no staged changes
- The tracked modified-path list and untracked-path list were unchanged by this
  task. The pre-existing Level 7 investigation changes, local reference files,
  and screenshot remained untouched. The prompt/report are under ignored
  `AI-Logs/` and were not staged.
- No commit or push was made.

Pre-existing modified tracked paths at both start and finish:

`.github/copilot-instructions.md`;
`src/Tempest.Game/Debug/DebugController.cs`;
`src/Tempest.Game/Debug/GameplayPlaybackSession.cs`;
`src/Tempest.Game/Debug/GameplayStartupConfiguration.cs`;
`src/Tempest.Game/Debug/PlaybackScreenshotCapture.cs`;
`src/Tempest.Game/Levels/Level7WaveSelection.cs`;
`src/Tempest.Game/Levels/LevelConfiguration.cs`;
`src/Tempest.Game/Levels/LevelLoader.cs`;
`src/Tempest.Game/Levels/LevelProgression.cs`;
`src/Tempest.Game/Program.cs`;
`src/Tempest.Game/Rendering/DisplayConfiguration.cs`;
`src/Tempest.Game/Rendering/Projection.cs`;
`src/Tempest.Game/Rendering/VectorRenderer.cs`;
`src/Tempest.Game/Systems/InputSystem.cs`;
`src/Tempest.Game/TempestGame.cs`;
`tests/Tempest.Game.Tests/DebugControllerTests.cs`;
`tests/Tempest.Game.Tests/DirectLevelStartupTests.cs`;
`tests/Tempest.Game.Tests/DisplayConfigurationTests.cs`;
`tests/Tempest.Game.Tests/GameOverDensityDiagnosticTests.cs`;
`tests/Tempest.Game.Tests/GameplayRecordingTests.cs`;
`tests/Tempest.Game.Tests/InputDiagnosticTests.cs`;
`tests/Tempest.Game.Tests/InputSystemTests.cs`;
`tests/Tempest.Game.Tests/Level9ProductionTests.cs`;
`tests/Tempest.Game.Tests/PlaybackScreenshotCaptureTests.cs`;
`tests/Tempest.Game.Tests/TransitionPresentationTests.cs`.

Pre-existing untracked paths at both start and finish:

`Reference/arcade/MBUCOD.V05.txt`;
`Reference/arcade/MBUDOC.DOC`;
`screenshot000.png`;
`src/Tempest.Game/Debug/Level7FramebufferDiagnostic.cs`;
`src/Tempest.Game/Debug/Level7VisibleRuntimeDiagnostic.cs`;
`src/Tempest.Game/Levels/AtariTubeRecordCatalog.cs`;
`src/Tempest.Game/Levels/RomPresentationMetadata.cs`;
`src/Tempest.Game/Rendering/AtariCoordinateProjector.cs`;
`src/Tempest.Game/Rendering/AtariDvgViewportMap.cs`;
`src/Tempest.Game/Rendering/AtariLevel7EntityPresentation.cs`;
`src/Tempest.Game/Rendering/AtariLevel7Presentation.cs`;
`src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`;
`src/Tempest.Game/Rendering/AtariTubeReferenceShapeDiagnostic.cs`;
`src/Tempest.Game/Rendering/AtariTubeVectorListBuilder.cs`;
`tests/Tempest.Game.Tests/AtariLevel7EntityPresentationTests.cs`;
`tests/Tempest.Game.Tests/AtariPresentationAdapterTests.cs`;
`tests/Tempest.Game.Tests/AtariTubeRecordPrototypeTests.cs`;
`tests/Tempest.Game.Tests/DiagnosticLevel10Tests.cs`;
`tests/Tempest.Game.Tests/Level7FramebufferDiagnosticTests.cs`;
`tests/Tempest.Game.Tests/Level7VisibleRuntimeDiagnosticTests.cs`;
`tests/Tempest.Game.Tests/RomPresentationMetadataTests.cs`;
`tests/Tempest.Game.Tests/VectorRendererAtariPresentationTests.cs`.

## Neal's manual recording/playback commands

Run these from the repository root in PowerShell. Create the recording first,
then replay that same file in both modes:

```powershell
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --record-gameplay .\UserData\GameplayRecordings\level10-genuine-geometry.json --level 10
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --playback-file .\UserData\GameplayRecordings\level10-genuine-geometry.json
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --playback-file .\UserData\GameplayRecordings\level10-genuine-geometry.json --atari-presentation
```

The sole manual judgment requested is whether the characteristic apparent
depth-dependent enemy-motion defect reproduces when Level-7-like gameplay runs
on genuine Level 10 geometry with `--atari-presentation`. The observation is
**PENDING Neal**. No arcade fidelity, final placement, margins, or authentic
Level 10 behavior assessment is requested.
