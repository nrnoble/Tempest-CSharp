# Level 6 Baseline Visual Calibration Candidate

Date: 2026-11-16
Outcome: **PARTIAL**

## Outcome and stop reason

The current C# Level 6 image does not agree with the baseline predicted by the
current startup source. The image's near-rim extent is reproduced by the same
source calculation using K=65, while the current startup path sets K=70. The
difference is material, so the task's source-to-final-pixels gate is not met.
No Level 6 presentation candidate was implemented or approved.

The K=65 explanation is an inference, not an established cause. The authorized
image may predate the current source, or may have been produced under a
different runtime condition. Establishing which would require a new capture
or launching the game, both outside this task's boundary.

## Starting repository state

- Branch: `master`, tracking `origin/master`.
- HEAD: `385f603a94391756b2ca6cccd524dc17b281e782` (`Close out ROM-backed enemy-shot rendering`).
- The index was clean. No files were staged.
- The starting worktree already contained these modified tracked files:
  - `src/Tempest.Game/Debug/DebugController.cs`
  - `src/Tempest.Game/Debug/PlaybackScreenshotCapture.cs`
  - `src/Tempest.Game/Levels/LevelLoader.cs`
  - `src/Tempest.Game/Program.cs`
  - `src/Tempest.Game/Rendering/DisplayConfiguration.cs`
  - `src/Tempest.Game/Rendering/Projection.cs`
  - `src/Tempest.Game/Rendering/VectorRenderer.cs`
  - `src/Tempest.Game/Systems/InputSystem.cs`
  - `tests/Tempest.Game.Tests/DebugControllerTests.cs`
  - `tests/Tempest.Game.Tests/DisplayConfigurationTests.cs`
  - `tests/Tempest.Game.Tests/GameOverDensityDiagnosticTests.cs`
  - `tests/Tempest.Game.Tests/GameplayRecordingTests.cs`
  - `tests/Tempest.Game.Tests/InputSystemTests.cs`
  - `tests/Tempest.Game.Tests/Level9ProductionTests.cs`
  - `tests/Tempest.Game.Tests/PlaybackScreenshotCaptureTests.cs`
  - `tests/Tempest.Game.Tests/TransitionPresentationTests.cs`
- The starting worktree also contained these untracked files:
  - `Reference/arcade/MBUCOD.V05.txt`
  - `Reference/arcade/MBUDOC.DOC`
  - `screenshot000.png`
  - `src/Tempest.Game/Levels/RomPresentationMetadata.cs`
  - `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`
  - `tests/Tempest.Game.Tests/AtariPresentationAdapterTests.cs`
  - `tests/Tempest.Game.Tests/RomPresentationMetadataTests.cs`
  - `tests/Tempest.Game.Tests/VectorRendererAtariPresentationTests.cs`

All pre-existing changes and untracked files were preserved. Only the two
authorized Level 6 images were inspected; no other visual asset was opened,
searched, or analyzed. No visual files were created or modified.

## Level 6 source baseline

Displayed Level 6 uses a closed 16-point triangle with 16 playable lanes.
The points, in production order, are:

```text
(-116,145), (-171,145), (-137,72), (-102,0),
(-68,-72), (-34,-145), (0,-218), (34,-145),
(68,-72), (102,0), (137,72), (171,145),
(116,145), (54,145), (0,145), (-54,145)
```

The definition has `CenterY=360`, `YOffset=-40`; production has
`Position=50`, `Depth=400`. Its provenance is **REFERENCE-DERIVED** geometry,
not an Atari record-05 coordinate conversion. The Nov. 15 coordinate-mapping
report classifies Level 6 as partial structural correspondence and explicitly
prohibits using its comparison fit as a renderer gain.

Current startup sets `Projection.K=70` and `Projection.ZOffset=5`.
`Projection.PortraitPerspective` defaults to zero in the experimental portrait
path. The current portrait host viewport is 720x960 at x=280 in the 1280x960
window. The playfield camera zoom is 1.26 and maps projected logical
coordinates to host coordinates as:

```text
hostX = 640 + 1.26 * (logicalX - 400)
hostY = 1.26 * logicalY - 39.72
```

With `--atari-presentation`, Level 6 currently falls back to the unchanged
legacy projection. It is not in `HasModelFrameMapping`, and the renderer emits
the bounded development-only legacy-fallback diagnostic. This fallback does
not make the current source output match the supplied image.

### Current source-predicted bounds

Bounds use all 16 points at near, middle, and far depth. Logical projection
uses the current startup K=70/ZOffset=5; host bounds include the current
portrait playfield camera.

| Depth | Logical X bounds | Logical Y bounds | Host X bounds | Host Y bounds | Host width x height |
|---|---:|---:|---:|---:|---:|
| Near, 50 | 182.364..617.636 | 31.636..493.636 | 365.778..914.222 | 0.142..582.262 | 548.444 x 582.120 |
| Middle, 250 | 353.059..446.941 | 289.176..388.824 | 580.854..699.146 | 324.642..450.198 | 118.292 x 125.555 |
| Far, 450 | 373.692..426.308 | 320.308..376.154 | 606.852..673.148 | 363.868..434.234 | 66.295 x 70.366 |

The projected model-origin host anchors are x=640 and y=349.735, 400.045,
406.126 at near, middle, and far depth, respectively. The near/far size ratio
is 8.2727 on both axes. The local host-space X and Y derivatives are equal
under the legacy projection: 1.603636, 0.345882, and 0.193846 host pixels per
model unit at near, middle, and far depth. Current source near-rim margins
are approximately 85.8px left/right, 0.1px top, and 377.7px bottom.

## Authorized image measurements and reconciliation

### Current C# image

`UserData/Screenshots/Level-6/Level-6 Tube too high.png` is 1280x960. Its
actual portrait viewport is x=280..1000, y=0..960. The rendered blue tube
pixels (Raylib blue, RGB 0,121,241) occupy x=385..894, y=30..570. These are
approximately 510x541 pixels, centered at x=639.5, y=300. The corresponding
viewport-normalized extent is about 0.708x0.564; left/right margins are about
0.146 each, top margin 0.031, and bottom margin 0.405. Pixel rasterization
uncertainty is about one pixel.

The visible upper yellow HUD pixels occupy x=292..987, y=14..71. The tube
apex and HUD glyphs are laterally separated in this frame; bounding boxes alone
overstate their overlap. The lower recharge/playback text is not part of the
tube measurement.

With current K=70, source predicts a near-rim bound of x=365.778..914.222,
y=0.142..582.262. This is about 38px wider and 42px taller than the measured
pixel extent, with a top-edge discrepancy of about 30px. By contrast, the
same projection and camera calculation using K=65 predicts x=385.365..894.635,
y=29.695..570.235, agreeing with the image to within rasterization. Since
current `Program` sets K=70, the supplied C# image cannot be reconciled with
the current source baseline.

The far rim is partially obscured by temporary gameplay objects and cannot
be measured reliably enough to tune depth.

### MAME reference

`UserData/Screenshots/Level-6/Tempest-MAME-frame_8100.png` is 480x640; the
full image is the portrait gameplay viewport. The blue outer near-rim boundary
is approximately x=90..389, y=146..538, with about one pixel of
rasterization/antialiasing uncertainty. This gives normalized extent about
0.625x0.613, normalized left/right margins about 0.188, top margin about
0.228, and bottom margin about 0.159.

The green HUD/information pixels occupy x=78..280, y=89..121. An isolated
yellow glyph at x=42..62, y=129..140 has an uncertain role and was excluded
from both the tube and HUD-clearance measurements. The visible gap from the
green HUD to the tube's top is about 25px (0.039 of viewport height). The far
rim is crowded by gameplay objects and is not reliable for depth calibration.
MAME is treated as the authorized practical visual reference, not an exact
physical-arcade specification.

The MAME near-rim extent, mapped to the C# portrait viewport, corresponds to
approximate host bounds x=415..865, y=219..807. Its bounding-box center is
slightly below viewport center (normalized y about 0.534). The single frame
does not distinguish a common presentation center from a bounded downward
exception once the triangle's asymmetric extents and its projected model
origin are considered. No bounding-box-centering shortcut was selected.

The C# and MAME near/far structures are not sufficiently unobscured to
establish a depth defect. No reciprocal-depth, level-scale, camera-height, or
Y2D terms were introduced; the established C# depth progression remains
unchanged in this investigation.

## Candidate decision, implementation, and validation

The MAME image gives a clear normalized near-rim target, but the current C#
image does not describe the current K=70 source output. Therefore extent
ratios, translation, and per-axis local scales cannot be safely finalized
against the required current-pixel baseline. An image-only affine transform
would be speculative. The candidate specification is **not established**.

No source or test files were changed. No Level 6 model points, lanes,
topology, gameplay, or presentation behavior were modified. Accepted Levels
1-5, Levels 7-9, Game Over, Approach, recording/playback, and HUD behavior
were not changed. The required candidate, renderer/world-anchor, regression,
full-suite, Release-build, and `git diff --check` validations were not run:
there was no implementation to validate, and the source-to-image gate failed.
This is not a validation pass.

No interactive game launch or manual acceptance was performed. No manual
launch command is issued for this partial outcome.

## Ending state and remaining boundary

The worktree and index retain the starting repository state. This report and
the paired prompt are ignored `AI-Logs/` records only; nothing was staged,
committed, or pushed. The cause of the K=65-like image versus K=70 startup
source remains unresolved. The task explicitly forbids requesting a new
capture or launching the game, so implementation must remain stopped at
**PARTIAL**.
