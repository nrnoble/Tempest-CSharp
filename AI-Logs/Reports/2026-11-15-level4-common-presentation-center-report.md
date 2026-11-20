# Level 4 common presentation-center candidate

## Outcome

**READY FOR LEVEL 4 CENTER CHECK**. This is a project presentation translation,
not an arcade-fidelity claim. Neal's manual acceptance remains outstanding.
The game was not launched.

## Starting repository state

- Branch `master`, HEAD `385f603a94391756b2ca6cccd524dc17b281e782`,
  upstream `origin/master`, index empty.
- Tracked worktree modifications: `src/Tempest.Game/Debug/DebugController.cs`,
  `src/Tempest.Game/Debug/PlaybackScreenshotCapture.cs`,
  `src/Tempest.Game/Levels/LevelLoader.cs`, `src/Tempest.Game/Program.cs`,
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
- Pre-existing untracked paths: `Reference/arcade/MBUCOD.V05.txt`,
  `Reference/arcade/MBUDOC.DOC`, one visual asset listed by Git status,
  `src/Tempest.Game/Levels/RomPresentationMetadata.cs`,
  `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`,
  `tests/Tempest.Game.Tests/AtariPresentationAdapterTests.cs`,
  `tests/Tempest.Game.Tests/RomPresentationMetadataTests.cs`, and
  `tests/Tempest.Game.Tests/VectorRendererAtariPresentationTests.cs`.

The specified prompt and report paths did not exist before this task. Existing
work was preserved. No visual assets, recordings, or raw ROMs were inspected.

## Common center and placement

Production `Program.cs` sets `K=70`, `ZOffset=5`. Levels 1-3 have the same
model origin, `CenterY=400`, `YOffset=-80`, and near depth 50; their accepted
candidate placements are respectively `+122`, `+114`, and `+114.29437`.
At the near rim their projected model origins, transformed through the shared
portrait camera, are host `(640,489.709)`, `(640,479.629)`, and `(640,480.000)`.
Level 1 is approximately 9.709 host pixels below the center, not mathematically
identical. This is a visual common-center interpretation, not a claim that the
three accepted outputs coincide exactly.

The defensible common **PROJECT TRANSLATION** target is the shared portrait
viewport's geometric center `(640,480)` host, equivalent to approximately
`(400,412.476190)` logical. Level 2's placement was explicitly derived from
the 960-pixel viewport center, and Level 3's was derived from the same
composition; Level 1's accepted visual target places its origin within about
10 pixels. The projected near-rim model origin is the corresponding reference
point across these symmetric models, rather than an average of their bounding
boxes. Its use for Level 4 deliberately does not center Level 4's asymmetric
bounding box. Other levels' existing placements are unchanged. The accepted
origin positions are checked directly by the new deterministic test.

Level 4's near-rim origin before placement is logical `(400,297.272727)`,
host `(640,334.843636)`; after placement it is approximately logical
`(400,412.476187)`, host `(640,480.000)`. The applied constant translation is
logical `(0,+115.20346)`, or approximately host `(0,+145.15636)` at the
unchanged portrait zoom. This is fixed after projection, including during
Approach and zoom, not recalculated per frame.

## HUD and geometry

At normal production projection, Level 4's near-rim bounds before translation
are logical X `121.272727..678.727273`, Y `149.636364..481.818182`;
after translation X is unchanged and Y is `264.839824..597.021642`.
The post-placement near-rim host Y range is `293.978..712.527`; the far-rim
logical Y range is `467.357..507.511`. Normal portrait score/lives and up
to three production wave lines end by host Y `165.6`, so the tube has
approximately 128.38 host pixels of vertical clearance. The test checks near
and far vertices; linear projection in reciprocal depth keeps the intervening
rails between those endpoints. This is source-derived normal gameplay HUD
clearance, not pixel or manual visual verification.

Point-by-point comparisons at depths 50, 250 and 450 preserve the corrected
legacy-equivalent Level 4 X and Y up to the single Y translation, including
width, height, near/far size ratio, near/far center separation, local X/Y
scales, origin progression, point order, lanes and closed topology. The
near/far scale ratio is `8.272727`. Tests compare the legacy path independently
and confirm unchanged gameplay/model snapshots. The prior correction report's
`K=65` baseline was retained in the shape test; the center calculations above
use actual startup `K=70`.

The existing common `ProjectWorldPoint` path applies the translation to
tube vertices and tube-attached world positions without changing local scales.
Renderer tests compare tube lines, player edges, player shots, eggs, Tankers,
Flippers, enemy bullets, Fuseballs, and Spike lines/tips against legacy plus
the same delta; existing registration and player-shot artwork tests cover
local scaling. Other world-positioned objects use the same common seam.
Game Over retains its separate unchanged legacy path.

## Changes and validation

- Modified the pre-existing untracked `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`
  and its pre-existing untracked adapter/renderer test files only.
- Created the authorized ignored prompt and this report. No durable
  documentation was updated.
- Initial combined test run: **FAIL**, 62 passed and 1 failed because an
  existing Level 4 test helper still expected the pre-placement projection.
  The helper was updated to independently expect legacy plus the new delta.
- Candidate adapter and renderer/world-anchor, accepted Levels 1-3 and
  Levels 5-9 mapping regressions: **PASS**, 63 tests:
  `dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~AtariPresentationAdapterTests|FullyQualifiedName~VectorRendererAtariPresentationTests" --blame-hang-timeout 2m --verbosity minimal`.
- Level 1-9, Level 4, Approach, transitions, playback, recording, screenshot
  and Game Over regressions: **PASS**, 721 tests:
  `dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-build --no-restore --filter "FullyQualifiedName~Level1|FullyQualifiedName~Level2|FullyQualifiedName~Level3|FullyQualifiedName~Level4|FullyQualifiedName~Level5|FullyQualifiedName~Level6|FullyQualifiedName~Level7|FullyQualifiedName~Level8|FullyQualifiedName~Level9|FullyQualifiedName~LevelApproachTests|FullyQualifiedName~TransitionPresentationTests|FullyQualifiedName~GameplayPlayback|FullyQualifiedName~GameplayRecordingTests|FullyQualifiedName~HeadlessGameplayPlaybackTests|FullyQualifiedName~PlaybackScreenshotCaptureTests|FullyQualifiedName~VisiblePlaybackControllerTests|FullyQualifiedName~GameOver" --blame-hang-timeout 2m --verbosity minimal`.
- Full suite: **PASS**, 1,494 passed, 0 failed, 0 skipped:
  `dotnet test .\TempestCSharp.slnx --no-restore --blame-hang-timeout 2m --verbosity minimal`.
- Release: **PASS**, zero warnings and errors:
  `dotnet build .\TempestCSharp.slnx --configuration Release --no-restore -warnaserror --verbosity minimal`.
- `git diff --check`: **PASS**; existing LF-to-CRLF notices for
  `LevelLoader.cs` and `InputSystem.cs`. This Git command does not cover the
  pre-existing untracked candidate files.

## Limitations and manual boundary

No screenshots, videos, or recordings were opened or analyzed. Automated
mechanical checks do not establish arcade fidelity or visual acceptance. The
center is an approximate shared visual composition: the Level 1 offset is
explicitly preserved rather than hidden. HUD clearance covers the normal tube
and normal production HUD, not every transient overlay or sprite extremity.
There were no scope deviations. Neal should first judge placement only, not
fine Level 4 perspective or size:

`dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 4 --atari-presentation`

Does Level 4 now occupy the same appropriate gameplay presentation center as
accepted Levels 1-3 while retaining its own Level 4 shape and extent?

## Ending Git state

Branch and HEAD remain unchanged, upstream `origin/master`, ahead/behind
`0/0`, and index empty. The tracked and untracked status inventory is the same
as the starting inventory; the three pre-existing untracked candidate source
and test files contain this milestone's changes. The two authorized AI-Logs
files are ignored. Nothing was staged, committed, pushed, cleaned, reset,
restored, or stashed.
