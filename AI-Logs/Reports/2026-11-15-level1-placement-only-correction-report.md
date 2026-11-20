# Tempest: Level 1 Placement-Only Correction Candidate

Date: 2026-11-15

## Outcome

**READY FOR LEVEL 1 PLACEMENT CHECK**

The placement-only candidate is implemented and required automated validation
passed. This is an experimental project translation, not an arcade-fidelity
acceptance. Neal's manual check remains required.

## Starting state

- Branch: `master`
- HEAD: `385f603a94391756b2ca6cccd524dc17b281e782`
- Upstream: `origin/master`
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
- Untracked paths already present: `Reference/arcade/MBUCOD.V05.txt`,
  `Reference/arcade/MBUDOC.DOC`, `screenshot000.png`,
  `src/Tempest.Game/Levels/RomPresentationMetadata.cs`,
  `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`,
  `tests/Tempest.Game.Tests/AtariPresentationAdapterTests.cs`,
  `tests/Tempest.Game.Tests/RomPresentationMetadataTests.cs`, and
  `tests/Tempest.Game.Tests/VectorRendererAtariPresentationTests.cs`.

The specified current-candidate image,
`UserData/Screenshots/Level-1/live-level-1-tick-000091-001.png`, was absent at
the exact path. Only the other authorized image, the Paint composition
illustration, was viewed. No other visual asset was inspected.

## Placement seam

The Level 1 branch in `AtariPresentationAdapter.Project` first calculates the
corrected depth-dependent X/Y projection and local scale, then adds a single
constant to the returned Y coordinate. This is downstream of shape and depth
calculation; it cannot change X, depth progression, rim scale, center
separation, or local Jacobian.

Candidate world points enter the adapter through the existing
`atariPresentationEnabled`/model-mapping branch in `VectorRenderer`'s common
world-point projection. Tube vertices, player/Blaster anchors, and world
entities therefore receive the same translation. The existing portrait
camera and shared `PortraitPlayfieldVerticalTranslation = -33.1f` were not
modified. Legacy and unsupported-level paths do not call the Level 1 branch.

## Initial placement derivation

**Source-derived constraint:** the controlling Level 1 correction report
establishes that corrected Level 1 projection retains the prior C# depth
progression and legacy near/far shape. The diagnostic reconstruction gives
untranslated logical near-rim bounds of approximately
`x=145.45..654.55`, `y=43.64..552.73`. With the unchanged portrait transform
`hostY = 1.26 * logicalY - 39.72`, the corresponding host bounds are
approximately `y=15.26..656.72` in a 960-pixel viewport.

**Manual visual-composition target:** the allowed 1280x960 Paint illustration
places the rough near-rim outline at about `y=169..810`. This is a composition
guide, not a pixel target; its green lines and incidental Paint marks are not
implementation requirements.

**Project translation:** moving the rough current top from `15.26` to `169`
requires about `153.74` host pixels, or `153.74 / 1.26 = 122.016` logical
pixels. Rounded once to a simple uniform translation, `+122` logical Y moves
the candidate about `+153.72` host pixels. The resulting estimated bounds are
`y=168.98..810.44`, leaving about 149.56 pixels below the tube. This is one
placement value, not source/ROM-derived behavior. The missing current-candidate
image limits direct visual comparison; the existing accepted shape report,
source calculations, and available rough composition illustration provide a
defensible first candidate.

## Implementation

Changed only the existing candidate adapter and its focused tests:

- `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs` — adds a Level
  1-only `+122` logical-pixel Y translation after projection.
- `tests/Tempest.Game.Tests/AtariPresentationAdapterTests.cs` — asserts
  unchanged pre-placement projection, X, uniform Y delta, rim bounds, center
  separation, scale, and Approach composition.
- `tests/Tempest.Game.Tests/VectorRendererAtariPresentationTests.cs` — asserts
  translated tube vertices, player/Blaster endpoints, and shot anchors while
  protecting geometry and gameplay state.

The frozen portrait placement, camera, projection, level data, gameplay,
recording, screenshot infrastructure, and HUD remain untouched. No game was
launched.

## Shape preservation

The adapter test compares every Level 1 model point at near, middle, and far
depth against the pre-placement formula. Candidate X remains unchanged;
candidate Y is the old value plus 122; local scale is unchanged. For both near
and far rims, the test compares width and height before/after placement and
compares near/far vertical center separation. Existing finite-difference
Jacobian and Level 1 continuity tests also pass.

The renderer test confirms each candidate tube endpoint is the legacy endpoint
plus the same `(0, 122)` logical translation. Player/Blaster endpoints and a
shot anchor share that translation, while their relative geometry and gameplay
state remain unchanged. Other enemy, shot, Spike, and tube anchors use the same
world-point projection seam; local glyph scale continues to use the unchanged
adapter Jacobian.

## Regression protection

- Legacy Level 1 without `--atari-presentation`: unchanged; candidate output is
  compared with the legacy tube plus only the candidate translation.
- Levels 2-6: existing bounded legacy fallback remains covered.
- Levels 7-9: existing reference-numerics tests passed unchanged.
- Game Over: existing legacy-path equivalence test passed.
- Approach and repeated inputs: composition/determinism tests passed.
- Playback, recording, and screenshot regressions passed; no gameplay or
  recording-format code changed.
- The frozen shared portrait translation remains `-33.1`; its existing tests
  are included in the full suite.

## Validation

All test commands used VSTest `--blame-hang-timeout 5m`; they completed without
timeout or hang.

1. `dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --filter "FullyQualifiedName~AtariPresentationAdapterTests|FullyQualifiedName~VectorRendererAtariPresentationTests" --blame-hang-timeout 5m --verbosity minimal`
   — **PASS**, 34 passed, 0 failed, 0 skipped. Includes candidate regression
   numerics for Levels 7-9 and Level 2-6 fallback/Game Over checks.
2. `dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --filter "FullyQualifiedName~Level1|FullyQualifiedName~Approach|FullyQualifiedName~Playback|FullyQualifiedName~Recording|FullyQualifiedName~Screenshot" --blame-hang-timeout 5m --verbosity minimal`
   — **PASS**, 262 passed, 0 failed, 0 skipped.
3. `dotnet test .\TempestCSharp.slnx --blame-hang-timeout 5m --verbosity minimal`
   — **PASS**, 1465 passed, 0 failed, 0 skipped.
4. `dotnet build .\TempestCSharp.slnx --configuration Release --no-restore --verbosity minimal`
   — **PASS**, 0 warnings, 0 errors.
5. `git diff --check` — **PASS**. Git emitted existing LF-to-CRLF warnings for
   untouched `LevelLoader.cs` and `InputSystem.cs`; there were no whitespace
   errors. The three untracked candidate source/test files and prompt were
   separately checked for trailing whitespace with none found.
6. Final Git verification — branch, HEAD, upstream, tracked modifications,
   untracked path inventory, and empty index matched the starting state. The
   authorized `AI-Logs/` files are ignored. Nothing was staged, committed, or
   pushed.

## Manual command

dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 1 --atari-presentation

## First manual question

Does the Level 1 tube now sit at a reasonable vertical location with
appropriate visual room above it, while preserving the tube shape that was
already judged good?

## Git state

All pre-existing work was preserved. The index is empty, and nothing was
staged, committed, or pushed.
