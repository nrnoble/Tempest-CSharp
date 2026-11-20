# Tempest: Level 2 Atari-Presentation Candidate

## Outcome

**READY FOR LEVEL 2 MANUAL CHECK**

Level 2 is enabled in the opt-in `--atari-presentation` candidate within the
authorized adapter/renderer test surface. Automated validation passed. This is
an experimental visual candidate; Neal's manual visual acceptance is pending.

## Starting state

- Branch: `master`
- HEAD: `385f603a94391756b2ca6cccd524dc17b281e782`
- Upstream: `origin/master`; ahead/behind `0/0`
- Index: empty
- Tracked modifications already present: `src/Tempest.Game/Debug/DebugController.cs`,
  `src/Tempest.Game/Debug/PlaybackScreenshotCapture.cs`,
  `src/Tempest.Game/Levels/LevelLoader.cs`, `src/Tempest.Game/Program.cs`,
  `src/Tempest.Game/Rendering/DisplayConfiguration.cs`,
  `src/Tempest.Game/Rendering/Projection.cs`,
  `src/Tempest.Game/Rendering/VectorRenderer.cs`,
  `src/Tempest.Game/Systems/InputSystem.cs`,
  `tests/Tempest.Game.Tests/DebugControllerTests.cs`,
  `DisplayConfigurationTests.cs`, `GameOverDensityDiagnosticTests.cs`,
  `GameplayRecordingTests.cs`, `InputSystemTests.cs`,
  `Level9ProductionTests.cs`, `PlaybackScreenshotCaptureTests.cs`, and
  `TransitionPresentationTests.cs`.
- Pre-existing untracked paths included
  `Reference/arcade/MBUCOD.V05.txt`, `Reference/arcade/MBUDOC.DOC`,
  `src/Tempest.Game/Levels/RomPresentationMetadata.cs`,
  `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`,
  `tests/Tempest.Game.Tests/AtariPresentationAdapterTests.cs`,
  `RomPresentationMetadataTests.cs`, and
  `VectorRendererAtariPresentationTests.cs`. A local visual asset was also
  present in Git status and is intentionally not named or inspected here.
- All pre-existing changes and local reference material were preserved. The
  adapter and two candidate test files were already untracked; changes in them
  remain untracked and unstaged.

## Level 2 mapping

The controlling coordinate-mapping handoff establishes displayed Level 2 →
Atari record `01`, a consistent approximate centered uniform fit with
`u=1.436688312`, X/Y orientation `+1/+1`, and closed 16-point topology. The
`j=(i+2) mod 16` index relation remains comparison-only. No production points,
lanes, order, or traversal were rotated.

The current metadata agrees with the handoff: table index `01`, `ScaleRaw=0x1c`,
`CameraHeightRaw=0x50`, signed Y2D `-224`, and closed topology. Metadata and
C# topology are validated; no metadata source was changed.

## Candidate behavior

Before editing, the existing generic candidate arithmetic was evaluated at
`K=70`, `ZOffset=5`, and the Level 2 near/middle/far depths. Its center Y was
approximately `227.784 / 237.871 / 239.527`, versus legacy
`298.182 / 378.039 / 387.692`; its middle/far local scales were about
`0.359 / 0.209`, versus legacy `0.275 / 0.154`. This would carry the diagnosed
global upward displacement and alter C# near/far progression.

The Level 2 translation therefore preserves the existing C# projection
relationship, `depth + ZOffset`, and existing `CenterY`/`YOffset` placement.
It produces finite continuous near/middle/far projection with the far rim
meaningfully recessed, and maintains square X symmetry and the adapter's local
Jacobian. The mapping is registered and validated as approximate; it does not
reindex or warp the stored C# points.

ROM metadata participates in catalog consistency and topology checks. The
Level 2 visual arithmetic deliberately does not apply the ROM reciprocal-depth
term, `lev_y3d` camera-height offset, or signed Y2D `-224`; direct generic
application produced the measured displacement/progression mismatch. Those
values remain preserved and validated as Atari evidence. No placement constant
or camera tuning was added.

All candidate world anchors continue through the shared renderer projection
seam; candidate local sizing uses the adapter's local scale. Approach offsets
compose through the Level 2 adapter path.

## Level 1 preservation

No Level 1 production arithmetic or constants changed. The `u=25/14`, `e=+1`
mapping, legacy C# depth progression, and `+122` placement remain as before.
Focused adapter and renderer tests passed their existing Level 1 projection,
placement, tube, player, shot, and gameplay-state assertions.

## Other-level preservation

Levels 3-6 remain explicit legacy fallbacks with the same one-time diagnostic.
Levels 7-9 retain their existing candidate mappings and numerical references.
Game Over remains on its legacy projection exemption. No `VectorRenderer.cs`
or other pre-existing modified production file was edited in this milestone.

## Geometry/gameplay protection

Tests verified unchanged Level 2 point coordinates/order, closed topology,
playable lane count, configuration depth/position, and repeated projection
output. They also compared Level 2 tube, player, shot, and egg anchors across
legacy and candidate rendering and checked that rendering did not mutate
geometry or gameplay state. No gameplay, Approach simulation, playback,
recording, screenshot, input, or Game Over implementation was changed.

## Tests and validation

The project uses .NET SDK `9.0.304`, VSTest `17.14.1`, xUnit, and `net9.0`.
Every test command used `--blame-hang-timeout 5min`; no validation command
timed out or was blocked.

1. Adapter and candidate renderer tests:
   `dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~AtariPresentationAdapterTests|FullyQualifiedName~VectorRendererAtariPresentationTests" --blame-hang-timeout 5min`
   **PASS** — 45 passed, 0 failed, 0 skipped.
2. Level 2, Approach, playback, recording, and screenshot-capture regressions:
   `dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~Level2|FullyQualifiedName~LevelApproachTests|FullyQualifiedName~TransitionPresentationTests|FullyQualifiedName~GameplayPlayback|FullyQualifiedName~GameplayRecordingTests|FullyQualifiedName~HeadlessGameplayPlaybackTests|FullyQualifiedName~PlaybackScreenshotCaptureTests|FullyQualifiedName~VisiblePlaybackControllerTests" --blame-hang-timeout 5min`
   **PASS** — 270 passed, 0 failed, 0 skipped.
3. Full suite:
   `dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --blame-hang-timeout 5min`
   **PASS** — 1476 passed, 0 failed, 0 skipped.
4. Release build:
   `dotnet build .\TempestCSharp.slnx --configuration Release --no-restore -warnaserror`
   **PASS** — 0 warnings, 0 errors.
5. `git diff --check`: **PASS**. The targeted whitespace check for modified
   candidate source/tests and the prompt artifact also passed. Git emitted only
   the pre-existing LF-to-CRLF notices for `LevelLoader.cs` and `InputSystem.cs`.
6. Final Git verification: branch, HEAD, upstream, tracked modifications, and
   untracked-path inventory were unchanged from the starting state; index
   remained empty. The two authorized `AI-Logs/` files are ignored.

## Limitations

The Level 2 frame mapping is approximate and its visual acceptance remains
pending. No screenshot or other visual asset was inspected, and the game was
not launched. This candidate intentionally retains the current C# projection
and placement instead of asserting literal Atari internal depth/Y2D operations
are correct in the C# coordinate system.

## Manual command

dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 2 --atari-presentation

## First manual question

Does Level 2 now have a tube shape, looking-down-the-tube presentation, and
overall placement that look reasonably similar to the arcade/MAME expectation,
without an obvious new problem?

## Git state

Nothing was staged, committed, or pushed. The existing worktree was preserved.
