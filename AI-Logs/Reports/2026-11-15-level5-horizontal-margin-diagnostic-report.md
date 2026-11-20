# Level 5 Horizontal-Margin Diagnostic

Date: 2026-11-16

## Executive classification

**UNRESOLVED**

The current source maps the Level 5 near-rim X extent to within about 2.8 host
pixels of the authorized MAME frame's estimated blue-line width. That is within
the frame's raster-boundary uncertainty and does not substantiate a
visibly excessive-margin translation error. The manually reported symptom
cannot be reconciled from source math alone; the C# screenshot was expressly
not authorized for inspection.

The only supported X-only adjustment is a small raster-center alignment
estimate (multiply current X displacement by `0.993877`, yielding a final net
factor of `0.665897`). It increases each host margin by only about 1.38 pixels
and therefore is not a supported remedy for the reported visible symptom.
Do not implement it until the final-output discrepancy is verified.

## Authorized image and evidence boundary

The only inspected visual asset was:

`C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\Level-5\Tempest-MAME-frame_5880.png`

It is a 480 x 640 RGB image. No directory enumeration or other image, video,
recording, screenshot, or visual asset inspection occurred. In the actual
outer blue tube-line envelope, the near-rim horizontal edge centers are
approximately x=90 and x=389, with about ±1 pixel uncertainty per edge.
Antialiasing/glow can extend the visible envelope by roughly one additional
pixel. Thus the centerline width estimate is 299 ± 2 pixels, center x=239.5,
and symmetric edge-to-image margins are about 90.5 ± 1 pixel each.

Normalized to the image width:

| MAME measure | Estimate |
|---|---:|
| Near-rim width | 299 / 480 = 0.622917 (±0.004167) |
| Left margin | 90.5 / 480 = 0.188542 (±0.002083) |
| Right margin | 90.5 / 480 = 0.188542 (±0.002083) |
| Horizontal center | x=239.5 in pixel-index coordinates; 0.500000 using pixel-center normalization |

The outer envelope is centered to raster precision. This is a practical MAME
comparison, not a physical-arcade specification.

## Current C# reconstruction

Current source confirms `AtariPresentationAdapter.Level5HorizontalPresentationScale`
is `0.670f`. `ProjectLevel5` projects model X with `Projection.Project`,
subtracts the projected near anchor, and multiplies that displacement by
`0.670`. At production camera settings (`K=70`, `ZOffset=5`), the X
displacement from model X is therefore:

`logicalX = 400 + modelX * 70 / (depth + 5) * 0.670`

Level 5's source model X extrema are ±210; its near, middle, and far depths
are 50, 250, and 450. The portrait playfield camera then maps logical X to
host X as:

`hostX = 640 + (logicalX - 400) * 1.26`

The portrait viewport is 720 pixels wide, centered at host X=640 in the
1280-pixel window (left edge 280). The renderer scissor clips to this viewport.
There is no second X zoom: the host camera's `1.26` is the single post-projection
gain. `WindowScale=1.6` establishes the 1280 x 960 host size; it is not an
additional playfield gain. HUD uses a separate camera and does not transform
the tube.

| Depth | Logical X bounds (current) | Host X bounds (current) | Host width | Center, logical / host |
|---|---:|---:|---:|---:|
| Near, 50 | 220.927..579.073 | 414.368..865.632 | 451.263 | 400 / 640 |
| Middle, 250 | 361.376..438.624 | 591.334..688.666 | 97.331 | 400 / 640 |
| Far, 450 | 378.354..421.646 | 612.726..667.274 | 54.548 | 400 / 640 |

Current near-rim margins within the 720-pixel viewport are 134.368 pixels on
each side, or 0.186623 of viewport width per side. Current near-rim width is
0.626755 of viewport width.

The local horizontal derivative (host pixels per model-X unit; logical value
in parentheses) is:

| Depth | Derivative |
|---|---:|
| Near | 1.074436 (0.852727 logical) |
| Middle | 0.231741 (0.183922 logical) |
| Far | 0.129877 (0.103077 logical) |

The legacy near-rim logical width before the Level 5 X factor is
`420 * 70 / 55 = 534.545`. Its host width before that factor would be
673.527 pixels, or 0.935454 of the 720-pixel viewport. Applying `0.670` to
that pre-camera displacement and then the one `1.26` camera zoom yields the
measured current candidate above.

## Coordinate reconciliation and classification

The spaces reconcile directly because both the 480 x 640 reference and the
720 x 960 portrait viewport have the same 3:4 aspect ratio:

`MAME normalized X -> same portrait normalized X -> logicalX = 400 + (u - 0.5) * 720/1.26 -> hostX = 280 + 720*u`

The normalized MAME width implies a 720-viewport width of 448.500 host pixels
and a logical width of 355.952. The current source produces 451.263 host
pixels, only 2.763 pixels wider than the centerline estimate. The current
candidate's margins are consequently about 1.38 pixels smaller per side than
the MAME estimate, not visibly larger. This difference is within the ±3 host
pixel width uncertainty obtained by mapping the ±2-pixel MAME edge-to-edge
uncertainty into the viewport.

No evidence supports a full-window/viewport mix-up, double/omitted portrait
zoom, wrong X stage, or wrong baseline extent. The ratio of the MAME-derived
post-camera target width to the C# pre-factor baseline host width is
`448.500 / 673.527 = 0.6659`, consistent with the reported rounded `0.670`
gain. Thus the factor's coordinate stage and magnitude are consistent with
current source arithmetic. Current source applies the factor to world X
displacement and to the tube-oriented local X scale.

Therefore the asserted excessive-margin discrepancy is not explained by a
verified coordinate translation error. The first calibration is consistent
with the permitted MAME measurement within image uncertainty, while the
manual report says the C# margins are too large. Since inspecting the C#
output is prohibited here, the conflicting observation remains unresolved.

## MAME-derived target and X-only specification

With the MAME edge-center estimate:

- Target normalized margins: 0.188542 each (±0.002083).
- Target normalized near-rim width: 0.622917 (±0.004167).
- Target portrait-host width: 448.500 pixels (±3 pixels).
- Target C# logical width: 355.952 (±2.381 logical units).
- Center stays at logical X=400, host X=640, and viewport-relative X=360.
- Target host bounds: 415.750..864.250 in the window, or 135.750..584.250
  relative to the portrait viewport.
- Target logical near-rim bounds: 222.024..577.976.

The single X-only factor that maps current near-rim displacement to the
centerline estimate is:

| Factor meaning | Value |
|---|---:|
| Multiplier on current `0.670` candidate | 0.993877 |
| Final net gain relative to legacy Level 5 displacement | 0.665897 |

These are alternate descriptions of one gain; do not multiply them again.
Applying the factor once gives:

| Depth | Logical width | Host width | Host bounds |
|---|---:|---:|---:|
| Near | 355.952 | 448.500 | 415.750..864.250 |
| Middle | 76.774 | 96.735 | 591.632..688.368 |
| Far | 43.027 | 54.214 | 612.893..667.107 |

All centers remain X=400 logical / X=640 host. A reasonable numerical
acceptance band for a future implementation is near-rim width 448.5 ±3 host
pixels (logical width 355.95 ±2.38), with each normalized viewport margin
0.18854 ±0.00208 and left/right symmetry within one host pixel. The current
candidate already falls within the width uncertainty band. The sub-3-pixel
alignment adjustment increases, rather than decreases, each margin by about
1.38 host pixels, so it cannot be claimed to fix the manual complaint.

## Depth and protected-state assessment

One corrected factor preserves the horizontal center because it scales only
the X displacement around the near anchor. It multiplies near, middle, and
far displacements equally, so the existing near/far width ratio
(`55/455`) is unchanged. The projection denominator `depth + ZOffset` remains
untouched.

`ProjectLevel5` is the shared world-point mapping used for tube vertices,
rails, rims, and world anchors. The renderer also applies the Level 5 X factor
to tube-oriented local X scales and offsets. Therefore a future authorized
single factor change can preserve rail/anchor coherence without changing
model geometry. Y factor `0.880`, vertical placement, HUD clearance, depth,
Level 5 topology and gameplay, recording/playback, Levels 1-4, Level 6
fallback, and Levels 7-9 remain outside this diagnostic and were not changed.

## Decision-relevant unknowns

| Unknown | Could change X correction? | Could require Y/placement/depth? | Could require model/gameplay? | Could materially change comparison? |
|---|---|---|---|---|
| Why Neal's C# candidate appears to have larger margins despite source math matching MAME | Yes, if final rendered bounds differ from source prediction | No evidence currently supports it | No | Yes; final C# pixels could identify a renderer/runtime mismatch |
| Exact MAME raster edge center within ±1 pixel | Only changes the tiny alignment estimate, within the stated tolerance | No | No | No; it cannot explain a visibly large margin difference |
| Physical arcade equivalence of the MAME frame | No; this task explicitly sets MAME as the target | No | No | No for this milestone |

Do not investigate ROM behavior, game geometry, or other levels to resolve
these non-contributing questions.

## Smallest next milestone

**Run one authorized Level 5 final-pixel bounds reconciliation:** with Neal's
authorization to inspect a C# Level 5 screenshot, measure only its outer
near-rim X edges against the already measured MAME image, in normalized
portrait-viewport coordinates. Do not change X until that measurement shows a
width outside 448.5 ±3 host pixels; if it is inside that band, retain the
current X factor and return to Neal for visual acceptance.

## Validation and Git state

No build, test suite, or game launch was required or performed. Analysis was
bounded to source/math and the one authorized MAME image.

Starting state: branch `master`, HEAD
`385f603a94391756b2ca6cccd524dc17b281e782`, tracking `origin/master` at 0/0,
empty index. There were 16 modified tracked files and 8 pre-existing untracked
paths. All pre-existing work was preserved.

Ending state: same branch, HEAD, upstream/tracking, tracked worktree changes,
and pre-existing untracked paths. Only the two authorized, ignored
`AI-Logs/` files were created. No tracked/indexed content changed; nothing was
staged, committed, pushed, cleaned, reset, restored, or stashed.
