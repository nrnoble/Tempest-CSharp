# Tempest: Level 2 Presentation-Aspect Candidate

## Outcome

**READY FOR LEVEL 2 ASPECT CHECK**

The opt-in `--atari-presentation` Level 2 path applies one calibrated vertical
aspect factor after depth projection and before the accepted `+114` placement.
Automated validation passed. After the manual command was provided, Neal
answered yes to the first manual question, accepting the Level 2 shape and
preserved placement/depth for this candidate.

## Starting state

- Branch: `master`
- HEAD: `385f603a94391756b2ca6cccd524dc17b281e782`
- Upstream: `origin/master`; status showed no ahead/behind divergence.
- Index: empty.
- Tracked modifications already present at start:
  `src/Tempest.Game/Debug/DebugController.cs`,
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
- Pre-existing untracked paths included
  `Reference/arcade/MBUCOD.V05.txt`, `Reference/arcade/MBUDOC.DOC`,
  `src/Tempest.Game/Levels/RomPresentationMetadata.cs`,
  `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`,
  `tests/Tempest.Game.Tests/AtariPresentationAdapterTests.cs`,
  `RomPresentationMetadataTests.cs`, and
  `VectorRendererAtariPresentationTests.cs`. A protected local visual asset
  was also listed by Git and was not opened, analyzed, or named here.
- All pre-existing work was preserved. The adapter and candidate tests remain
  untracked; the renderer was already tracked and modified before this task.

## Authorized visual evidence

Only these two files were opened and measured:

1. `UserData/Screenshots/Level-2/Level-2 atari presentation Square.png`
2. `UserData/Screenshots/Level-2/Level-2 MAME Rectangle.png`

No other visual material was searched, enumerated, opened, compared, or
processed. Neither authorized image was modified.

## Image measurements

The C# image is `1280 x 960`; the MAME image is `1098 x 1488`. Their overall
dimensions and capture compositions differ, so absolute pixel sizes and
margins are not treated as a common scale. Rim width/height ratios are
dimensionless. Bounds below are pixel-coordinate extents; faint glow,
anti-aliasing, and intersecting rails make the MAME far-rim edge estimates
approximate.

| Image/rim | Approx. bounds (x, y) | Width x height | W/H | Center |
|---|---|---:|---:|---:|
| C# near | (381..898, 221..737) | 517 x 516 | 1.00194 | (639.5, 479) |
| C# far | (608..671, 561..623) | 63 x 62 | 1.01613 | (639.5, 592) |
| MAME near | (190..950, 341..1337) | 760 x 996 | 0.76305 | (570, 839) |
| MAME far | (506..632, 937..1100) | 126 x 163 | 0.77301 | (569, 1018.5) |

Measured center separations are approximately 113 C# image pixels and 179.5
MAME image pixels. The C# outer tube spans y=221..737, leaving about 221 px
above and 222 px below in its 960-pixel image. The MAME outer tube spans about
y=341..1337, leaving about 341 px above and 150 px below in its 1488-pixel
image. Those capture-dependent margins and center separations were recorded
but not used to derive placement or perspective changes.

### Source-derived C# bounds

Level 2 model bounds are `322 x 322`. With the accepted `K=70`, `ZOffset=5`,
and depths 50 / 250 / 450 (near / middle / far), the existing logical
dimensions are approximately `409.82`, `88.39`, and `49.54` on each axis.
The portrait host's uniform zoom of `1.26` makes current near/far dimensions
approximately `516.37 x 516.37` and `62.42 x 62.42`. These source-derived
square bounds closely agree with the C# screenshot's rim ratios.

With the existing `CenterY=400`, `YOffset=-80`, and `+114`, source-derived
near/far host centers are approximately `(640, 479.63)` and `(640, 592.41)`.
The existing host-space center separation is about 112.78 px.

## Aspect-factor derivation

For each rim, compare the square-candidate W/H ratio to the MAME W/H ratio:

```text
near factor = (517 / 516) / (760 / 996) = 1.313066
far factor  = (63 / 62) / (126 / 163) = 1.314516
mean        = 1.313791
rounded first factor = 1.314
```

Near and far independently imply factors within about 0.12% of each other,
supporting one stable factor rather than per-rim fitting. The classification
is **PROJECT VISUAL CALIBRATION FROM AUTHORIZED MAME REFERENCE**; it is not
ROM-derived, mathematically exact arcade behavior, or a claim that MAME is
pixel-perfect.

## Implementation seam

`AtariPresentationAdapter.Project` applies the factor only in the Level 2
candidate branch. It scales the projected model-Y displacement from that
depth's projected tube center, then applies the existing `+114` translation.
The Y offset/Approach vertical offset remains part of the unscaled center.
Screen X, the `depth + ZOffset` denominator, `K`, `Position`, `Depth`,
horizontal local scale, and placement are unchanged.

The returned local Jacobian is anisotropic for Level 2:

```text
dX/dmodelX = existing depth scale
dY/dmodelY = existing depth scale * 1.314
```

The public factor input rejects non-finite and non-positive values
explicitly. `VectorRenderer` uses both axes for tube-oriented local geometry
and offsets; the candidate world-anchor path remains shared through
`ProjectWorldPoint` / `ProjectLane`. The shared portrait placement `-33.1`
and camera/viewport were not modified.

## Tube-attached objects

Tube vertices, rails, player/Blaster anchors, enemy and player-shot anchors,
Spikes, lane-centered objects, and below-rim anchors continue through the
common candidate projection. World-oriented local dimensions now use the
vertical Jacobian for Fuseballs, Tankers, Spikers, Flippers, and the player
claw; Pulsar local wave offsets receive the same vertical presentation factor.

The static player-shot picture and enemy-shot vector picture retain their
existing scalar/screen-oriented local artwork; their anchors still follow the
Level 2 tube transform. HUD/text, point-sized Eggs, and screen-oriented
effects are not stretched. Colors, animation, timing, and stored base
geometry are unchanged.

## Frozen accepted state

The accepted Level 2 `+114` placement, depth progression, near/far center
relationship, `Position`, `Depth`, model points, point order, lanes, and
closed topology remain unchanged. The shared `-33.1`, projection constants,
camera, viewport, gameplay, Approach simulation/timing, recording format,
input, and playback state are unchanged. The correction is presentation-only
and does not mutate model or gameplay state.

## Other-level preservation

- Level 1 remains numerically unchanged.
- Levels 3-6 remain on their existing legacy fallback.
- Levels 7-9 retain their existing candidate output.
- Game Over remains on its legacy projection exemption.
- Legacy Level 2 without `--atari-presentation` remains unchanged.

Focused adapter/renderer tests and the cross-level regressions cover these
paths, as well as deterministic repeated output, Approach composition, tube
anchors, geometry/topology, and screen-oriented shot artwork.

## Tests and validation

Test commands used VSTest hang-timeout protection (`--blame-hang-timeout
5min`) and isolated session output directories.

1. Adapter and candidate renderer tests:
   `dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~AtariPresentationAdapterTests|FullyQualifiedName~VectorRendererAtariPresentationTests" --blame-hang-timeout 5min --verbosity minimal -p:OutDir=C:\Users\nrnob\.copilot\session-state\a5372623-c67e-4ba7-8156-a15ff8cfc54d\files\level2-aspect-finaltests\`
   **PASS** — 47 passed, 0 failed, 0 skipped.
2. Level 1-9, Approach, playback, recording, and screenshot-capture
   regressions:
   `dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-build --no-restore --filter "FullyQualifiedName~Level1|FullyQualifiedName~Level2|FullyQualifiedName~Level3|FullyQualifiedName~Level4|FullyQualifiedName~Level5|FullyQualifiedName~Level6|FullyQualifiedName~Level7|FullyQualifiedName~Level8|FullyQualifiedName~Level9|FullyQualifiedName~LevelApproachTests|FullyQualifiedName~TransitionPresentationTests|FullyQualifiedName~GameplayPlayback|FullyQualifiedName~GameplayRecordingTests|FullyQualifiedName~HeadlessGameplayPlaybackTests|FullyQualifiedName~PlaybackScreenshotCaptureTests|FullyQualifiedName~VisiblePlaybackControllerTests" --blame-hang-timeout 5min --verbosity minimal -p:OutDir=C:\Users\nrnob\.copilot\session-state\a5372623-c67e-4ba7-8156-a15ff8cfc54d\files\level2-aspect-finaltests\`
   **PASS** — 681 passed, 0 failed, 0 skipped.
3. Full suite:
   `dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --configuration Release --no-build --no-restore --blame-hang-timeout 5min --verbosity minimal -p:OutDir=C:\Users\nrnob\.copilot\session-state\a5372623-c67e-4ba7-8156-a15ff8cfc54d\files\level2-aspect-finalserial\`
   **PASS** — 1,478 passed, 0 failed, 0 skipped.
4. Release build:
   `dotnet build .\TempestCSharp.slnx --configuration Release --no-restore -warnaserror --verbosity minimal -p:OutDir=C:\Users\nrnob\.copilot\session-state\a5372623-c67e-4ba7-8156-a15ff8cfc54d\files\level2-aspect-finalserial\`
   **PASS** — 0 warnings, 0 errors.
5. `git diff --check`: **PASS**. Git emitted only the pre-existing LF-to-CRLF
   notices for untouched `LevelLoader.cs` and `InputSystem.cs`. A targeted
   trailing-whitespace check for candidate source, tests, and prompt:
   **PASS**. An earlier concurrent test build reported a transient MSB3101
   shared-obj-cache warning; the final sequential Release build and complete
   suite passed cleanly.
6. Final Git verification: branch, HEAD, upstream, pre-existing tracked and
   untracked state, and empty index matched the starting state, except for the
   authorized changes in the candidate adapter/tests and narrow renderer
   integration. The prompt and this report are ignored; nothing was staged,
   committed, or pushed.

## Limitations

This is a first visual calibration from two captures with different image
dimensions and framing. It does not establish exact arcade geometry or
pixel-perfect equivalence. Neal accepted this candidate's manual presentation
check; that acceptance does not make the calibration ROM-derived or
pixel-perfect. Automated tests establish implementation invariants, not
visual equivalence.

## Manual command

dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 2 --atari-presentation

## First manual question

Does Level 2 now have a vertically rectangular screen-space shape reasonably
similar to the MAME reference while preserving the already accepted placement
and depth/perspective?

## Git state

Nothing was staged, committed, or pushed. All pre-existing work and protected
local material were preserved.
