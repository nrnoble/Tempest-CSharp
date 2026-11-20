# Level 6 Fresh-Capture Reconciliation and Presentation Candidate

Date: 2026-11-16
Outcome: **READY FOR LEVEL 6 MANUAL CHECK**

## Outcome

The fresh Level 6 C# near-rim pixels reconcile with the current startup
projection at K=70, ZOffset=5, with the current portrait camera and no Approach
offset. The earlier C# image is a materially smaller K=65-like presentation;
it was not a valid proxy for the fresh current-worktree output. A bounded,
opt-in, Level 6-only render transform was derived from the reconciled C# source
and normalized MAME near-rim measurements, implemented, and validated.

The candidate is a **PROJECT TRANSLATION**, not a ROM-backed constant or a
claim of exact physical-arcade output. Far-rim geometry remains obscured in the
authorized frame, so C# depth progression is unchanged. Neal's manual
evaluation remains required.

## Visual authorization and measurements

Only these three exact images were inspected:

1. `C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\Level-6\Level-6 Legacy cs.png`
2. `C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\Level-6\Level-6 Tube too high.png`
3. `C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\Level-6\Tempest-MAME-frame_8100.png`

No other image, screenshot, video, recording, extracted frame, contact sheet,
or visual asset was searched, opened, compared, or analyzed. No visual file
was created or modified.

The two C# images are 1280x960. The source-defined portrait viewport is
`x=280..1000`, `y=0..960` (720x960). The MAME image is 480x640 and is itself
the portrait viewport.

| Image | Outer blue pixel bounds (inclusive) | Pixel extent | Viewport-normalized extent | Center (normalized) | Margins (L/R/T/B) |
|---|---|---:|---:|---:|---:|
| Fresh C# | x=366..913, y=0..582 | 548x583 | .7611 x .6073 | .4993, .3031 | .1194 / .1194 / 0 / .3927 |
| Earlier C# | x=385..894, y=30..570 | 510x541 | .7083 x .5635 | .4993, .3125 | .1458 / .1458 / .0313 / .4052 |
| MAME reference | approximately x=90..389, y=146..538 | approximately 300x393 | .6250 x .6141 | .5000, .5344 (pixel centers) | .1875 / .1875 / .2281 / .1578 |

For the MAME extent calculation, raster pixel bounds are treated as the
half-open edges `[90,390) x [146,539)`, with about one pixel of antialiasing
uncertainty. Those half-open edges center at normalized y=.5352; the
pixel-center measurement is .5344, a difference under one pixel. The earlier
and fresh C# frames differ by 38 horizontal pixels
(7.45%) and 42 vertical pixels (7.76%); the fresh frame also reaches the top
edge, while the earlier frame begins at y=30.

The MAME green HUD pixels occupy approximately x=78..280, y=89..121. The
near-rim top at y=146 leaves about 25 pixels (0.039 viewport heights) below
that HUD. An isolated yellow MAME glyph at y=129..140 has an uncertain role and
was excluded from the HUD measurement. In the fresh C# frame, upper HUD-like
pixels occupy x=292..987, y=14..71; lower text extends to about y=139 and was
kept separate from the HUD measurement.

## Source/runtime reconciliation

The Level 6 production source is the closed 16-point triangle with 16 playable
lanes, `CenterY=360`, `YOffset=-40`, `Position=50`, and `Depth=400`. Its
geometry provenance remains **REFERENCE-DERIVED**, as established in the
handoff; no Atari-coordinate correspondence fit was used.

The complete current path is:

- `Projection.K` is declared with a default of 65, but normal startup in
  `Program` explicitly sets K=70 and ZOffset=5 before game construction.
- The only K command-line-style presentation option is not a K override:
  portrait perspective is independently parsed, while
  `--atari-presentation` cannot be combined with it and starts at the
  perspective default of zero.
- K and ZOffset animate during ExitZoom. Successful exit initialization calls
  `RestoreProjection`, resetting them to 70 and 5 before the next level's
  Approach. Reverse zoom clamps K to at least 70 and also restores 70/5.
- A direct production `--level 6` start is Playing, not Approach. During an
  inter-level Approach, the renderer applies `DepthOffset` and its matching
  vertical offset to the point and near/far interval; the captured near-rim
  bounds match the zero-offset source projection.
- Before this change, the opt-in renderer explicitly fell back to
  `ProjectWorldPointLegacy` for Level 6. That path uses the Level 6 definition
  and production configuration above, followed by the portrait playfield
  camera.
- Portrait output scale is 1.2, playfield scale is 1.05, and combined camera
  zoom is 1.26. The host mapping is
  `hostX = 640 + 1.26 * (logicalX - 400)` and
  `hostY = 1.26 * logicalY - 39.72`; the viewport begins at x=280.

At K=70/ZOffset=5, the continuous source near-rim bounds in host pixels are
x=365.778..914.222, y=0.142..582.262 (548.444x582.120). The fresh screenshot's
pixel bounds x=366..913, y=0..582 agree within rasterization tolerance. The
same source/camera arithmetic at K=65 predicts x=385.365..894.635,
y=29.695..570.235, matching the earlier screenshot's bounds within a pixel.

Therefore the apparent K discrepancy came from comparing the earlier
K=65-like screenshot with current K=70 startup output. The current Program
override, not the static K initializer, controls ordinary rendering; there is
no current startup/debug path that leaves production at K=65. The exact
historical runtime state that produced the earlier image is not established by
its pixels, and no timestamp-based cause is claimed.

## MAME comparison and candidate derivation

The practical MAME target normalizes to width .6250, height .6141, horizontal
center .5000, and vertical bounds .2281..8422 (top/bottom margins .2281/.1578).
Mapped into the C# portrait host viewport, the target near-rim edges are about
x=415..865, y=219..808.5. Current reconciled C# bounds are much too wide and
too high, while their vertical extent is already close to the MAME extent.
This supports independent X/Y extent factors; it does not support a depth
change.

The established Level 6 near-rim model-origin anchor projects to host
(640,349.735). The transform uses this near-origin anchor, not the tube
bounding-box center. Factors and placement were calculated in the actual
current C# projection domain:

| Candidate value | Provenance |
|---|---|
| X extent factor `0.820504` | Current source continuous width 548.444 host px to MAME-normalized target width `0.625 * 720 = 450` px. No X translation: source and target horizontal centers are both the viewport center. |
| Y extent factor `1.012678` | Current source continuous height 582.120 host px to MAME-normalized target height `(393/640) * 960 = 589.5` px. |
| Vertical placement `+177.21448` logical units | After scaling around the source near-origin anchor, the translation maps the near rim to the MAME-derived C# host bounds y=219..808.5. At camera zoom 1.26 this is about +223.29 host px. |
| Depth progression | Unchanged C# projection using the live K/ZOffset and existing point depths. The authorized far rim is obscured, so no additional depth terms are justified. |

The factors are derived from fresh C# final-pixel reconciliation, normalized
MAME measurements, and the project's existing portrait camera. They are not
ROM data and do not reuse any C#/Atari coordinate-correspondence scale.

## Implementation and preservation

Changed only the focused adapter/renderer/test surfaces plus the directly
related startup diagnostic text:

- `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`: added a Level 6
  projection using the legacy C# projection, near-origin anchor, independent
  local X/Y scales, and placement after extent treatment.
- `src/Tempest.Game/Rendering/VectorRenderer.cs`: routes displayed Level 6
  world points and local offsets through the candidate only when the opt-in
  is enabled; keeps the Level 6 Game Over tube on its legacy path. Screen-
  oriented shot artwork retains the scalar scale.
- `src/Tempest.Game/Program.cs`: corrected the development-only opt-in message
  to reflect the production Levels 1-9 presentation coverage.
- `tests/Tempest.Game.Tests/AtariPresentationAdapterTests.cs` and
  `tests/Tempest.Game.Tests/VectorRendererAtariPresentationTests.cs`: added
  normalized bounds/margin/anchor/Jacobian/Approach/error checks; tested opt-in,
  legacy Level 6 output, deterministic rendering, world anchors, model-state
  preservation, screen-oriented shot artwork, no fallback diagnostic, and
  unchanged Level 6 Game Over projection.

No Level 6 model points, point ordering, lane count, topology, gameplay
systems, timing, simulation, recordings, or playback code changed. The mapping
is presentation-only and only used for displayed Level 6 with
`--atari-presentation`; legacy Level 6 remains unchanged without that option.
Levels 1-5 and 7-9 mappings were not changed. HUD rendering and camera
configuration were not changed. Approach offsets are included in each frame's
projection, but simulation state is never mutated. The existing Game Over
projection path remains intact.

The candidate near rim matches the target normalized extent and margins
within the adapter test tolerance of .002. Its top is at about y=219 in the
C# viewport, below the normalized MAME HUD end at y=183, leaving about 36 px
of scaled reference clearance. It is also below the fresh C# upper HUD-like
region (bottom y=71) and lower text (bottom about y=139), leaving positive
clearance in both cases.

## Validation

- Targeted `AtariPresentationAdapterTests` and
  `VectorRendererAtariPresentationTests`: **PASS**, 74 passed, 0 failed.
- Full .NET test suite with a five-minute hang guard: **PASS**, 1505 passed,
  0 failed, 0 skipped.
- Release solution build: **PASS**, 0 warnings, 0 errors.
- `git diff --check`: **PASS**. Git emitted only pre-existing line-ending
  conversion warnings for `LevelLoader.cs` and `InputSystem.cs`.
- The first default-output test attempt could not replace the game apphost
  because the already-running `Tempest.Game` process (PID 40436) held it. A
  second attempt with `UseAppHost=false` found the same process locking the
  existing game DLL. Neither process was terminated. Tests and Release build
  were then completed successfully using isolated `bin-isolated` output paths;
  those generated directories were removed afterward.
- No interactive game was launched. No manual visual acceptance was
  performed.

## Git state

Starting state: branch `master`, tracking `origin/master`, HEAD
`385f603a94391756b2ca6cccd524dc17b281e782`, index clean. The starting worktree
already had 16 modified tracked files and 8 untracked files:

- Modified tracked: `src/Tempest.Game/Debug/DebugController.cs`,
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
  `tests/Tempest.Game.Tests/PlaybackScreenshotCaptureTests.cs`,
  `tests/Tempest.Game.Tests/TransitionPresentationTests.cs`.
- Untracked: `Reference/arcade/MBUCOD.V05.txt`,
  `Reference/arcade/MBUDOC.DOC`, `screenshot000.png`,
  `src/Tempest.Game/Levels/RomPresentationMetadata.cs`,
  `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`,
  `tests/Tempest.Game.Tests/AtariPresentationAdapterTests.cs`,
  `tests/Tempest.Game.Tests/RomPresentationMetadataTests.cs`,
  `tests/Tempest.Game.Tests/VectorRendererAtariPresentationTests.cs`.

These were preserved and integrated; no unrelated work was reverted.

Ending state: same branch, HEAD, upstream, and clean index. Only the focused
adapter/renderer/test edits and the opt-in diagnostic wording were made on
top of the pre-existing work. The two requested `AI-Logs` records are ignored
and unstaged. Nothing was staged, committed, or pushed.

## Manual boundary

Neal's manual evaluation remains the final acceptance for placement, size,
shape, perspective, and usability. Run only:

`dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 6 --atari-presentation`
