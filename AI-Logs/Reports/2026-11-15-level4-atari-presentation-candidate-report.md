# Level 4 Atari-Presentation Candidate

## Outcome

**READY FOR LEVEL 4 MANUAL CHECK**

The opt-in Level 4 affine frame is implemented within the authorized adapter
and renderer surfaces. Required validation completed successfully. Manual
presentation acceptance remains Neal's responsibility.

## Starting state

- Branch: `master`
- HEAD: `385f603a94391756b2ca6cccd524dc17b281e782`
- Upstream: `origin/master`
- Index: empty
- Tracked worktree modifications already present: `src/Tempest.Game/Debug/DebugController.cs`,
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
  `Reference/arcade/MBUDOC.DOC`, `screenshot000.png`,
  `src/Tempest.Game/Levels/RomPresentationMetadata.cs`,
  `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`,
  `tests/Tempest.Game.Tests/AtariPresentationAdapterTests.cs`,
  `tests/Tempest.Game.Tests/RomPresentationMetadataTests.cs`, and
  `tests/Tempest.Game.Tests/VectorRendererAtariPresentationTests.cs`.
- The existing candidate integration work was preserved. The untracked image
  surfaced in the required status inventory only; no visual asset was opened,
  searched, or analyzed.

## Adapter extension

Added `AtariModelFrameMapping` as an explicit, immutable affine model-frame
descriptor with independent X/Y model units per Atari unit, X/Y references,
and X/Y orientation. Construction rejects non-positive/non-finite scales,
non-finite references, and orientations other than `+1` or `-1`.

The Level 4 mapping is available through the existing candidate renderer path.
`VectorRenderer` uses the adapter only when Atari presentation is enabled;
without the option Level 4 continues through the unchanged legacy projection.
The mapping does not use or alter ROM metadata to store C# conversion values.
Screen placement and the existing Level 1-3 placement/aspect rules remain
separate from Level 4's model-frame descriptor.

The projection result exposes explicit `LocalScaleX` and `LocalScaleY`; the
renderer returns and uses both axes for its existing tube-oriented local
geometry pathways.

## Level 4 mapping

- X scale: `aX=2.081778520`
- Y scale: `aY=2.231150659`
- X reference: `128`
- Y reference: `121.501111`
- Orientation: positive X, positive Y
- Production point order: forward; no rotation, reversal, reflection, or lane
  reindexing
- Maximum residuals: approximately `8.048826` C# units in X and `5.595232` in Y
- RMS residual: approximately `6.395377` C# units

This is an explicit **REFERENCE-DERIVED APPROXIMATION**, not an exact Atari
conversion. The references are used only as model-frame origins and cancel
when model coordinates are expressed as displacements from those origins.

## Depth treatment

Level 4 retains the established C# progression `K / (depth + ZOffset)` and
returns the resulting depth scale divided independently by the X and Y frame
scales. The near gameplay rim remains larger than the far rim, and center
progression remains continuous and finite.

No Atari reciprocal-depth arithmetic or literal `lev_scale=0x0F`,
`lev_y3d=0x68`, or signed `Y2D=-128` placement term was added.

## Placement and shorter profile

Level 4 continues to use its existing `CenterY=380` and `YOffset=-65` baseline.
No Level 1 `+122`, Level 2 `+114`, Level 3 placement, Level 2 `1.314` aspect,
screen translation, envelope centering, or equal-height correction is applied.
The per-axis frame changes model-coordinate dimensions only; the projected
center baseline remains unchanged. No numeric height target was supplied, so
the qualitative shorter-profile requirement remains for Neal's manual check.

## Geometry/gameplay protection

No Level 4 definition or gameplay code changed. Tests verify the unchanged
point snapshot, point order, closed topology, lane count, player state, level
state, configuration depth/position, shots, and enemy counts across candidate
tube rendering. Projection and anchor calls do not mutate the simulation.

## Tube and object presentation

Near/far tube vertices and lane-based anchors share the adapter path. Tests
verify registered positions for the player, eggs, shots, Flippers, Tankers,
enemy bullets, Fuseballs, and Spikes. Existing axis-aware local-scale pathways
are used for tube-oriented vector geometry. Player-shot artwork remains
screen-oriented and uses the X scale; no new stretch was applied to static
shots, HUD/text, overlays, Game Over, or unrelated effects.

## Accepted Levels 1-3 preservation

The existing Level 1-3 projection branches and constants were left intact.
Focused adapter/renderer regressions verify their prior outputs, including
Level 2's accepted `1.314` aspect and `+114` placement.

## Other-level preservation

Levels 5-6 remain on legacy fallback. Levels 7-9 candidate behavior and
Game Over's legacy projection exemption remain unchanged. Approach offsets
compose through the existing render-only seam. Recording/playback,
transitions, screenshot capture, input, and simulation were not modified.

## Tests and validation

All commands completed; VSTest used the established two-minute hang-timeout
protection.

- Adapter-focused:
  `dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~AtariPresentationAdapterTests" --blame-hang-timeout 2m`
  — **37 passed, 0 failed, 0 skipped**.
- Renderer/world-anchor:
  `dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~VectorRendererAtariPresentationTests" --blame-hang-timeout 2m`
  — **23 passed, 0 failed, 0 skipped**.
- Combined adapter, renderer, accepted Levels 1-3, and Levels 5-9 fallback/
  candidate regressions:
  `dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~AtariPresentationAdapterTests|FullyQualifiedName~VectorRendererAtariPresentationTests" --blame-hang-timeout 2m`
  — **60 passed, 0 failed, 0 skipped**.
- Level 4, Approach, transition, playback, recording, and screenshot regressions:
  `dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~Level4CandidateTests|FullyQualifiedName~LevelApproachTests|FullyQualifiedName~TransitionPresentationTests|FullyQualifiedName~GameplayPlaybackTests|FullyQualifiedName~GameplayRecordingTests|FullyQualifiedName~PlaybackScreenshotCaptureTests|FullyQualifiedName~HeadlessGameplayPlaybackTests|FullyQualifiedName~GameplayPlaybackSessionTests|FullyQualifiedName~VisiblePlaybackControllerTests" --blame-hang-timeout 2m`
  — **217 passed, 0 failed, 0 skipped**.
- Full suite:
  `dotnet test .\TempestCSharp.slnx --no-restore --blame-hang-timeout 2m`
  — **1,491 passed, 0 failed, 0 skipped**.
- Release build:
  `dotnet build .\TempestCSharp.slnx --configuration Release --no-restore`
  — **PASS**, 0 warnings, 0 errors.
- `git diff --check` — **PASS**, exit code 0. Git printed only the existing
  LF-to-CRLF notices for `LevelLoader.cs` and `InputSystem.cs`.

No interactive game launch or visual analysis was performed.

## Deviations

None.

## Limitations

Level 4 remains an approximate reference-derived presentation candidate with
measurable fit residuals. Its placement, shorter profile, and overall shape
remain pending Neal's manual acceptance.

## Manual command

dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 4 --atari-presentation

## First manual questions

1. Does Level 4 remain properly placed?
2. Does it retain its characteristically shorter profile rather than being
   forced to resemble Levels 1-3 in height?
3. Is there any obvious new shape failure?

## Git state

Final branch, HEAD, upstream, tracked worktree path set, and untracked path set
match the recorded starting inventory. The index remains empty. No files were
staged, committed, or pushed. The only new AI-Logs files are this report and
the authorized prompt; both remain ignored and unstaged.
