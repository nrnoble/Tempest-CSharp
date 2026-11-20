# Tempest: Level 2 Placement-Only Correction Candidate

## Outcome

**READY FOR LEVEL 2 PLACEMENT CHECK**

Level 2 receives a single `+114` logical-Y project translation in the opt-in
candidate. The existing shape, C# depth progression, near/far relationship,
model coordinates, and gameplay are unchanged. Neal's placement-only manual
acceptance remains pending; depth/perspective is not being presented for
judgment yet.

## Starting state

- Branch: `master`; HEAD: `385f603a94391756b2ca6cccd524dc17b281e782`;
  upstream: `origin/master`; ahead/behind: `0/0`.
- Index was empty.
- Tracked modifications already present: `src/Tempest.Game/Debug/DebugController.cs`,
  `src/Tempest.Game/Debug/PlaybackScreenshotCapture.cs`,
  `src/Tempest.Game/Levels/LevelLoader.cs`, `src/Tempest.Game/Program.cs`,
  `src/Tempest.Game/Rendering/DisplayConfiguration.cs`,
  `src/Tempest.Game/Rendering/Projection.cs`,
  `src/Tempest.Game/Rendering/VectorRenderer.cs`,
  `src/Tempest.Game/Systems/InputSystem.cs`,
  `tests/Tempest.Game.Tests/DebugControllerTests.cs`,
  `DisplayConfigurationTests.cs`, `GameOverDensityDiagnosticTests.cs`,
  `GameplayRecordingTests.cs`, `InputSystemTests.cs`, `Level9ProductionTests.cs`,
  `PlaybackScreenshotCaptureTests.cs`, and `TransitionPresentationTests.cs`.
- Pre-existing untracked paths included `Reference/arcade/MBUCOD.V05.txt`,
  `Reference/arcade/MBUDOC.DOC`,
  `src/Tempest.Game/Levels/RomPresentationMetadata.cs`,
  `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`,
  `tests/Tempest.Game.Tests/AtariPresentationAdapterTests.cs`,
  `tests/Tempest.Game.Tests/RomPresentationMetadataTests.cs`, and
  `tests/Tempest.Game.Tests/VectorRendererAtariPresentationTests.cs`. One
  visual asset path is intentionally not named here. All these paths were
  preserved; the visual asset was not opened or investigated. The adapter and
  candidate tests remain untracked and unstaged.

## Pre-placement Level 2 bounds

Calculated from the current Level 2 square's `y = -161..161`, `CenterY=400`,
`YOffset=-80`, `Position=50`, `Depth=400`, and the candidate's production
`K=70`, `ZOffset=5`. These are source-derived candidate calculations, not
visual-asset measurements.

The portrait playfield camera maps logical Y to host Y as
`hostY = 1.26 * logicalY - 39.72`; its viewport is 960 host pixels tall.

| Bounds | Logical Y | Portrait-host Y |
|---|---:|---:|
| Near rim | 93.27..503.09 | 77.80..594.17 |
| Far rim | 362.92..412.46 | 417.56..479.98 |
| Whole tube | 93.27..503.09 | 77.80..594.17 |

The current whole-tube logical center is `298.18`; its portrait-host center is
`335.99`. Current host margins are about `77.80` above and `365.83` below.

## Placement derivation

Centering the unchanged complete host envelope in the 960-pixel portrait
viewport would require about `(480 - 335.99) / 1.26 = 114.29` logical pixels.
The first candidate rounds this once to the uniform `+114` logical-Y value.
It moves the host bounds to approximately `221.44..737.81`, leaving `221.44`
pixels above and `222.19` pixels below. The complete tube remains inside the
portrait gameplay viewport, with substantially more upper room and useful
lower margin.

This is a **PROJECT TRANSLATION for presentation composition**, not
Atari/ROM-derived behavior. It is not copied from Level 1 and is not based on
ROM Y2D.

## Placement seam

`AtariPresentationAdapter.Project` now first calculates Level 2's unchanged
screen X, projected Y, and local scale. It then adds `Level2VerticalPlacement`
to that projected Y only. X translation is exactly zero; every Level 2 world
point routed through the existing common renderer projection receives the same
Y delta. `Projection.Project`, the portrait camera, viewport, and
`PortraitPlayfieldVerticalTranslation = -33.1f` were not changed.

## Shape/depth preservation

Focused adapter tests compare every Level 2 model point at near, middle, and
far depths against the pre-placement C# projection: X and local scale remain
equal, while Y is exactly the original value plus 114. Near- and far-rim
widths/heights, near/far center separation, and the local Jacobian remain
unchanged. Existing projection tests continue to verify the continuous C#
depth progression. The Level 2 points, order, closed topology, lane count, and
gameplay/model state are unchanged.

## World-object consistency

Renderer tests verify the common delta for tube lines, player endpoints, shot
and egg anchors, flipper endpoints, enemy bullets, and Fuseballs. Spike and
other lane-based world anchors continue through the existing shared
`ProjectLane`/`ProjectWorldPoint` seam. No glyph/object sizes were changed.
Approach offsets compose through the same candidate projection deterministically.

## Other-level preservation

- Level 1's accepted candidate mapping, `+122` placement, and rendered output
  remain unchanged.
- Levels 3-6 retain the legacy fallback and one-time diagnostic.
- Levels 7-9 retain their existing candidate numerical references.
- Game Over remains on the legacy projection exemption.
- Existing `-33.1` portrait-camera assertions pass; no camera or viewport
  constants changed.
- No gameplay, geometry, timing, input, recording/playback, pause, stepping,
  or screenshot-capture code changed.

One pre-existing console diagnostic in `Program.cs` still describes Levels
2-6 as legacy fallback even though the current adapter has a Level 2 mapping.
It was left untouched to keep this placement milestone within the authorized
adapter/test surface.

## Tests and validation

All completed test runs used VSTest hang protection (`--blame-hang-timeout
5min`). After an initial ordinary-output test attempt could not copy the
Debug apphost because a running `Tempest.Game` process (PID 114808) held it,
the process was left untouched and all subsequent test/build commands used
isolated session output directories. Test commands appended
`-p:OutDir=C:\Users\nrnob\.copilot\session-state\817bdcbf-dddb-4635-a3ca-f70b4f9dc8d1\files\level2-placement-bin\`;
the Release build used the same option with
`...\level2-placement-release\`.

1. Adapter and candidate renderer tests:
   `dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~AtariPresentationAdapterTests|FullyQualifiedName~VectorRendererAtariPresentationTests" --blame-hang-timeout 5min --verbosity minimal`
   — **PASS**, 46 passed, 0 failed, 0 skipped.
2. Level 1-9, Approach, playback, recording, and screenshot-capture regressions:
   `dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~Level1|FullyQualifiedName~Level2|FullyQualifiedName~Level3|FullyQualifiedName~Level4|FullyQualifiedName~Level5|FullyQualifiedName~Level6|FullyQualifiedName~Level7|FullyQualifiedName~Level8|FullyQualifiedName~Level9|FullyQualifiedName~LevelApproachTests|FullyQualifiedName~TransitionPresentationTests|FullyQualifiedName~GameplayPlayback|FullyQualifiedName~GameplayRecordingTests|FullyQualifiedName~HeadlessGameplayPlaybackTests|FullyQualifiedName~PlaybackScreenshotCaptureTests|FullyQualifiedName~VisiblePlaybackControllerTests" --blame-hang-timeout 5min --verbosity minimal`
   — **PASS**, 680 passed, 0 failed, 0 skipped.
3. Full suite:
   `dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --blame-hang-timeout 5min --verbosity minimal`
   — **PASS**, 1477 passed, 0 failed, 0 skipped.
4. Release build:
   `dotnet build .\TempestCSharp.slnx --configuration Release --no-restore -warnaserror --verbosity minimal`
   — **PASS**, 0 warnings, 0 errors.
5. `git diff --check` — **PASS**. Git emitted only the pre-existing LF-to-CRLF notices for untouched `LevelLoader.cs` and `InputSystem.cs`. A separate targeted trailing-whitespace check of the candidate files and prompt — **PASS**.
6. Final Git verification — branch, HEAD, upstream, tracked modifications,
   untracked-path inventory, and empty index matched the starting state. The
   prompt and report are ignored `AI-Logs/` artifacts; nothing was staged,
   committed, or pushed.

No screenshots, videos, recordings, or other visual assets were inspected, and
the game was not launched.

## Manual command

dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 2 --atari-presentation

## First manual question

Does Level 2 now sit at a reasonable vertical location with sufficient visual
room above the tube and useful lower margin, while its existing tube shape
remains unchanged?

## Next manual step

Only after Neal accepts vertical placement should Level 2 depth/perspective be
judged as the next manual step.

## Git state

Nothing was staged, committed, or pushed.
