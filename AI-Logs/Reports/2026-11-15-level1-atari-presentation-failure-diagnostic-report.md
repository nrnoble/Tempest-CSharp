# Level 1 Atari-Presentation Failure Diagnostic

Date: 2026-11-15

## Executive conclusion

**Primary classification: MULTIPLE INDEPENDENT ERRORS.**

The temporary Level 1 frame conversion preserves the C# point shape: the same
scale is used for X and Y, and it cancels out of the near-rim projection. The
first decisive *shape/depth* divergence is the adapter's far-depth reciprocal
mapping. At the far rim, its projected size is 0.1515 of the near rim rather
than the legacy 0.1209, and its center is 7.8 host pixels above the near-rim
center rather than 112.8 pixels below it. This produces the candidate's
compressed, radial-looking tube rails.

The excessive upward placement is independent: Level 1's signed ROM Y2D
value, -192, contributes -68.18 logical pixels (-85.91 host pixels) to the
candidate near rim. The unchanged portrait camera then applies the frozen
-33.1 translation (-39.72 host pixels), taking the candidate's calculated
near-rim top to -70.65 host pixels and clipping it at the viewport. The
portrait camera is not the source of the shape divergence, and -33.1 is not
the source of the candidate-specific vertical displacement.

No model geometry, lane order, topology, or gameplay geometry change is
indicated by this diagnostic. The candidate remains unaccepted. No fix or
constant change was made.

## Authorized visual evidence

Only these three authorized images were opened and analyzed:

1. `C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\Level-1\Level-1 atari-presentation.png` — actual candidate failure.
2. `C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\Level-1\Level-1 Paint-modified legacy.png` — correctly shaped legacy tube with rough, Paint-adjusted placement; not an arcade or pixel-perfect target.
3. `C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\Level-1\MAME LEVEL-1.png` — arcade visual objective.

No other screenshots, images, videos, recordings, visual directories, or
visual assets were inspected or processed.

## Visual measurements

Measurements below are approximate image-space bounds of the tube, not
source-code values. The first two images are 1280x960; MAME is 480x640.

| Image | Near rim | Far rim | Center / relation | Margins, clipping, HUD |
|---|---|---|---|---|
| Atari-presentation candidate | About x=319..961; top clipped at y=0, lower edge about y=571. The source-derived full rim is about 642x642. | About x=592..689, y=194..291. | Source-derived near center (640,250); far center (640,242), only about 8 px above it. | Near rim clips at the top; left/right margins are about 319 px and bottom margin about 389 px. The tube intrudes into the top HUD region. |
| Paint-modified legacy | About x=319..961, y=169..810; about 642x641, nearly circular in this image. | About x=602..679, y=565..642; about 77x77. | Near center about (640,490); far center about (640,604), about 114 px below the near rim. | About 169 px top and 150 px bottom margin; about 319 px left/right. The rim sits below the HUD. This placement is intentionally rough and Paint-modified. |
| MAME Level 1 | About x=89..390, y=145..539; about 301x394, vertically elongated. | About x=218..262, y=385..441; about 44x56. | Near center about (240,342); far center about (240,413), about 71 px below the near rim. | About 145 px top, 101 px bottom, and 89-90 px side margins. The near rim sits below the HUD. |

The MAME near-rim width/height ratio is approximately 0.76; both C# near rims
are approximately 1.00 before candidate clipping. MAME's far/near width and
height ratios are about 0.15 and 0.14. Legacy is about 0.12; the candidate's
source-derived ratio is about 0.15. Thus the candidate's far-rim *size ratio*
is closer, but its far-rim vertical relation, top containment, and overall
centering are substantially less like MAME. The legacy placement reference is
closer overall in those examined respects; neither C# image matches the MAME
near-rim oval. No pixel equivalence is claimed.

The Paint-modified image places its near rim about 154 px below the current
legacy pipeline's calculated near-rim center. That is consistent with the
user-described manual placement adjustment; it is not evidence that the
current legacy camera produces that placement.

## Immutable-input verification

Level 1 is the same closed, 16-point definition in both paths:

- `LevelLoader.LoadLevel1()` supplies the ordered points beginning
  `(200,0), (184,76), (141,141), (76,184)` and continuing around the
  16-point circle; `CenterY=400`, `YOffset=-80`
  (`src/Tempest.Game/Levels/LevelLoader.cs:43-50`).
- `LevelConfiguration.Level1` uses that definition with
  `Depth=Level.Depth=400` and `Position=Level.Position=50`
  (`src/Tempest.Game/Levels/LevelConfiguration.cs:130-144`,
  `src/Tempest.Game/Levels/Level.cs:18-19`).
- `LevelDefinition.Points` is read by `DrawTube`; near and far positions are
  projected from the same point at `Position` and `Position+Depth`
  (`src/Tempest.Game/Rendering/VectorRenderer.cs:963-981`).
- The candidate receives those same point coordinates and definition/config
  values. It transforms only presentation coordinates; no point, order,
  topology, level position, or depth is changed
  (`VectorRenderer.cs:218-270`).
- Level 1's C# definition is continuous/closed; ROM metadata has `OpenRaw=0`
  (`LevelLoader.cs:43`, `Levels/RomPresentationMetadata.cs:54`). The adapter
  topology guard accepts this pairing.

The user identifies the captures as ordinary Level 1 gameplay. A still image
does not encode the simulation's Approach flag, so the reconstructions below
use the specified ordinary non-Approach condition: both Approach offsets are
zero. No input geometry differs between the two render paths.

## Legacy pipeline reconstruction

For ordinary Level 1 rendering, `Program.cs` sets `Projection.K=70` and
`Projection.ZOffset=5` (`Program.cs:233-234`). The legacy projection is
`xscreen=400+x*70/(z+5)` and
`yscreen=400+(y-80)*70/(z+5)` (`Rendering/Projection.cs:40-87`).
The near and far `z` values are 50 and 450, so the camera denominators are
55 and 455.

| Rim | Logical bounds (x min..max, y min..max) | Width x height | Center | Host/viewport bounds |
|---|---|---:|---|---|
| Near | 145.45..654.55, 43.64..552.73 | 509.09 x 509.09 | (400, 298.18) | x=319.27..960.73, y=15.26..656.72 |
| Far | 369.23..430.77, 356.92..418.46 | 61.54 x 61.54 | (400, 387.69) | x=601.23..678.77, y=410.00..487.54 |
| Whole tube | 145.45..654.55, 43.64..552.73 | 509.09 x 509.09 | (400, 298.18) | x=319.27..960.73, y=15.26..656.72 |

Far/near width and height ratios are both 0.120879. The far-rim center is
89.51 logical pixels (112.78 host pixels) below the near-rim center.
Representative logical rail endpoints for point 0 are near `(654.55,298.18)`
and far `(430.77,387.69)`; point 4 is near `(400,552.73)` and far
`(400,418.46)`.

The current portrait camera has zoom 1.26 and transforms coordinates as
`hostX=136+1.26*x`, `hostY=1.26*y-39.72`. The scissor viewport is
x=[280,1000), y=[0,960) (`Rendering/DisplayConfiguration.cs:20-52`,
`Rendering/VectorRenderer.cs:296-302`). Legacy bounds are inside the viewport;
the near rim has about 15.26 host pixels of top clearance and is not clipped.

## Atari-candidate pipeline reconstruction

Level 1's model-frame mapping is `u=25/14`, Y orientation `+1`
(`AtariPresentationAdapter.cs:133-138`). Metadata is scale raw 24, camera
height raw 80, signed Y2D raw -192, and closed topology
(`Levels/RomPresentationMetadata.cs:44-55`).

For `zNear=50`, `zFar=450`, `K=70`, `ZOffset=5`, `verticalOffset=-80`:

- Temporary frame coordinates are
  `xRaw=x/u` and `yRaw=(y-80)/u + 128 - 80 = (y-80)/u + 48`.
  The model's X and Y are converted using the same `u`; this conversion
  alone preserves their relative shape.
- `normalizedDepth=(z-50)/400`: near 0, far 1.
- Adapter reciprocal values are `16+224*normalizedDepth`: near 16, far 240.
- Divisors after adding `scaleRaw=24` are 40 near and 264 far.
- Logical scale `lambda=u*K*(16+24)/(256*(50+5))=0.355113636`.
- Near-origin Y displacement is `-80/u+48=3.2`; its near projection is
  `256*3.2/40=20.48`.
- Host baseline `B=400+(-80)*70/55-lambda*20.48=290.909091`.
- Per point:
  `xscreen=400+lambda*256*xRaw/(reciprocalDepth+24)`;
  `yscreen=B+lambda*(-192+256*yRaw/(reciprocalDepth+24))`.

The table reconstructs every immutable input point at both depths. Coordinates
are logical screen `(x,y)`; all values are rounded to 0.01.

| i | Input (x,y) | Temporary (xRaw,yRaw) | Legacy near | Legacy far | Candidate near | Candidate far |
|---:|---:|---:|---:|---:|---:|---:|
| 0 | (200,0) | (112.00,3.20) | (654.55,298.18) | (430.77,387.69) | (654.55,230.00) | (438.57,223.83) |
| 1 | (184,76) | (103.04,45.76) | (634.18,394.91) | (428.31,399.38) | (634.18,326.73) | (435.48,238.48) |
| 2 | (141,141) | (78.96,82.16) | (579.45,477.64) | (421.69,409.38) | (579.45,409.45) | (427.19,251.02) |
| 3 | (76,184) | (42.56,106.24) | (496.73,532.36) | (411.69,416.00) | (496.73,464.18) | (414.66,259.31) |
| 4 | (0,200) | (0.00,115.20) | (400.00,552.73) | (400.00,418.46) | (400.00,484.55) | (400.00,262.40) |
| 5 | (-76,184) | (-42.56,106.24) | (303.27,532.36) | (388.31,416.00) | (303.27,464.18) | (385.34,259.31) |
| 6 | (-141,141) | (-78.96,82.16) | (220.55,477.64) | (378.31,409.38) | (220.55,409.45) | (372.81,251.02) |
| 7 | (-184,76) | (-103.04,45.76) | (165.82,394.91) | (371.69,399.38) | (165.82,326.73) | (364.52,238.48) |
| 8 | (-200,0) | (-112.00,3.20) | (145.45,298.18) | (369.23,387.69) | (145.45,230.00) | (361.43,223.83) |
| 9 | (-184,-76) | (-103.04,-39.36) | (165.82,201.45) | (371.69,376.00) | (165.82,133.27) | (364.52,209.17) |
| 10 | (-141,-141) | (-78.96,-75.76) | (220.55,118.73) | (378.31,366.00) | (220.55,50.55) | (372.81,196.64) |
| 11 | (-76,-184) | (-42.56,-99.84) | (303.27,64.00) | (388.31,359.38) | (303.27,-4.18) | (385.34,188.35) |
| 12 | (0,-200) | (0.00,-108.80) | (400.00,43.64) | (400.00,356.92) | (400.00,-24.55) | (400.00,185.26) |
| 13 | (76,-184) | (42.56,-99.84) | (496.73,64.00) | (411.69,359.38) | (496.73,-4.18) | (414.66,188.35) |
| 14 | (141,-141) | (78.96,-75.76) | (579.45,118.73) | (421.69,366.00) | (579.45,50.55) | (427.19,196.64) |
| 15 | (184,-76) | (103.04,-39.36) | (634.18,201.45) | (428.31,376.00) | (634.18,133.27) | (435.48,209.17) |

| Rim | Logical bounds (x min..max, y min..max) | Width x height | Center | Host/viewport bounds |
|---|---|---:|---|---|
| Near | 145.45..654.55, -24.55..484.55 | 509.09 x 509.09 | (400,230.00) | x=319.27..960.73, y=-70.65..570.81 |
| Far | 361.43..438.57, 185.26..262.40 | 77.13 x 77.13 | (400,223.83) | x=591.40..688.60, y=193.71..290.90 |
| Whole tube | 145.45..654.55, -24.55..484.55 | 509.09 x 509.09 | (400,230.00) | x=319.27..960.73, y=-70.65..570.81 |

Candidate far/near width and height ratios are both 0.151515. The far-rim
center is 6.17 logical pixels (7.78 host pixels) above the near-rim center.
Representative point-0 rail endpoints are near `(654.55,230.00)` and far
`(438.57,223.83)`; point 4 is near `(400,484.55)` and far `(400,262.40)`.
The candidate near rim's theoretical top is -70.65 host pixels, so the
viewport clips it to y=0; the calculated lower edge is y=570.81. The far rim
is not clipped.

## First shape divergence

At the near rim, candidate and legacy X/Y scale derivatives both equal
`70/55=1.272727` logical pixels per model unit. Candidate near width, height,
and X coordinates therefore match legacy; candidate Y is only translated.
The common frame scale `u` cancels between model-to-raw conversion and
projection. There is no shared-u, Y-orientation, or temporary-frame
distortion at this first near-rim comparison.

The first decisive shape/depth divergence is at the far-rim reciprocal
mapping (`AtariPresentationAdapter.cs:71-87, 109-112`). Legacy far derivative
is `70/455=0.153846`; candidate far derivative is
`lambda*256/(u*264)=0.192837`, about 25.3% larger. Consequently the candidate
far rim is 0.1515 of the near rim rather than legacy's 0.1209. More
decisively, the far-rim center moves down 112.78 host pixels in legacy but up
7.78 pixels in the candidate. The selected candidate reciprocal denominator
is therefore not equivalent to the legacy camera denominator across the
tube depth interval. The near/far spacing collapses and reverses, yielding
the radial-looking rails.

The portrait camera applies one isotropic 1.26 scale, so it cannot change the
near-rim aspect or correct this depth relationship. It only scales and
translates the projection, then clips it. The Level 1 C# points are
themselves a circle and legacy projection uses equal X/Y scaling; the code
does not show an existing anisotropic screen-shape treatment that the
temporary frame conversion removes. The MAME oval appearance is not encoded
in these C# Level 1 points.

## Placement diagnosis

At the near-rim center, legacy gives y=298.18. Candidate gives y=230.00.
The candidate's baseline and near-origin projection cancel the converted
Y-origin terms at near depth, leaving
`legacy near center + lambda*ScreenYOffsetSigned`.
For Level 1, `lambda*(-192)=-68.18` logical pixels, or -85.91 host pixels.
This identifies signed Y2D, not existing `CenterY`, `YOffset`, Y orientation,
or `CameraHeightRaw`, as the candidate-specific near-rim upward shift.

The frozen portrait translation `-33.1` maps to another -39.72 host pixels.
It is a later, unchanged global transform: it does not produce the logical
candidate/legacy difference or alter tube shape. It worsens the clipping,
but the candidate near rim would still extend about 30.93 px above the
viewport even with that translation removed. Legacy's current calculated
near rim remains 15.26 px inside the top edge with -33.1. Thus -33.1 exposes
and aggravates an earlier candidate displacement; it is not the source of
that displacement and should not be retuned based on this failure.

A pure post-projection translation can move the candidate near rim without
changing its aspect, far/near size ratio, or near/far center relationship.
It can address placement/clipping, but cannot repair the wrong depth
relationship. The Paint reference is consistent with preserving the legacy
tube outline and applying a largely global downward placement adjustment; it
does not justify reshaping the model points.

## MAME-direction comparison

The legacy Paint reference is closer overall to MAME in containment below the
HUD, top/bottom margins, vertical centering, and the far rim's downward
placement relative to the near rim. Its normalized near-to-far center
separation is about 0.18 near-rim heights, close to MAME's about 0.18; the
candidate's is about -0.01 and in the opposite direction. Candidate's
far/near diameter ratio happens to be closer to MAME, but does not offset its
clipped near rim and collapsed/reversed far-rim placement. Both C# paths have
approximately circular near rims, unlike MAME's vertically elongated
screen-space near rim. These are directional comparisons, not pixel-match
claims or implementation constants.

## Classification

**MULTIPLE INDEPENDENT ERRORS**

The candidate reciprocal-depth mapping produces the shape/depth error;
the separate signed-Y2D placement term moves the tube upward, and the
unchanged portrait translation compounds top clipping. These terms are
separable: a post-projection translation cannot fix the depth relationship.
They occur in the same adapter function for the first two effects, but are
independent mathematical terms and require separate diagnosis. The adapter
matches its current implementation contract; no implementation defect is
identified.

## Decision-relevant unknowns

| Unknown | Could change classification? | Could change code layer? | Could require model/gameplay geometry? | Could change next step? |
|---|---|---|---|---|
| The still images do not contain a captured Approach-state flag. The user identifies ordinary gameplay; calculations use zero Approach offsets as requested. | No; the independent depth and signed-Y2D terms remain. | No; the discrepancy remains in presentation projection/placement. | No. | No; ordinary-state source reconstruction is complete. |
| Exact MAME and Paint-edited rim pixels are approximate visual bounds, with different image sizes and some Paint artifacts. | No; source calculations identify the divergence. | No. | No. | No; greater pixel precision is not needed to identify the first source divergence. |
| Neal's manual fidelity acceptance of a corrected candidate has not occurred. | No for this source diagnosis; it can determine later acceptance. | No for the identified terms. | No evidence here supports changing gameplay geometry. | Only a later authorized candidate/manual-acceptance task; it does not block this report. |

No remaining unknown requires another diagnostic before the next decision.

## Smallest next step

If authorized, make one Level 1-only adapter correction candidate that tests
the reciprocal-depth mapping and signed-Y2D placement independently, while
keeping model points/order/topology, legacy projection, and portrait
translation `-33.1` frozen. This diagnostic does not authorize that work.

## Scope and completion

- Prompt: `AI-Logs/Prompts/2026-11-15-level1-atari-presentation-failure-diagnostic-prompt.md`
- Report: `AI-Logs/Reports/2026-11-15-level1-atari-presentation-failure-diagnostic-report.md`
- Files created: exactly those two ignored AI-Logs artifacts.
- Source files inspected: `AI-Logs/Reports/2026-11-15-atari-presentation-adapter-implementation-report.md`;
  `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`,
  `VectorRenderer.cs`, `Projection.cs`, `DisplayConfiguration.cs`,
  `src/Tempest.Game/Levels/LevelLoader.cs`, `LevelConfiguration.cs`,
  `LevelDefinition.cs`, `Level.cs`, `RomPresentationMetadata.cs`, and
  `src/Tempest.Game/Program.cs`.
- Exact images inspected: the three paths listed under Authorized visual
  evidence only.
- Calculations: all 16 Level 1 points at near/far depth through both
  projections; logical and portrait bounds; reciprocal denominators,
  per-depth scale, near/far ratios, vertical centers, camera placement, and
  viewport clipping.
- Validation: no build or test run was required. No game was launched.
- Starting and ending Git state: branch `master`; HEAD
  `385f603a94391756b2ca6cccd524dc17b281e782`; upstream `origin/master`;
  index empty. The same 16 tracked paths remained modified and the same
  8 untracked paths remained present. The handoff report's ending untracked
  inventory did not enumerate the already-present
  `screenshot000.png` and `src/Tempest.Game/Levels/RomPresentationMetadata.cs`;
  both were preserved. `screenshot000.png` was not opened; the metadata
  source was read only as an authorized listed source.
- No tracked file was changed by this diagnostic; the index remained
  unchanged. Nothing was staged, committed, or pushed.
- Primary classification: **MULTIPLE INDEPENDENT ERRORS**.
- First decisive shape divergence: candidate far-depth reciprocal mapping
  (`reciprocalDepth=240`, divisor 264), which yields a far-rim scale and
  center relationship unlike legacy.
- Most important remaining unknown: subsequent manual acceptance remains
  Neal's decision; it does not block this diagnosis.
