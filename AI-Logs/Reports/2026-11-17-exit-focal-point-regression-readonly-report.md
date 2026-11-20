# ExitZoom Focal-Point Regression — Read-Only Investigation

Date: 2026-11-17

## Status and direct answers

**COMPLETE — the presentation-math cause is established.** No source, tests,
configuration, durable project documentation, presentation values, gameplay,
recordings, or Git state were changed. The required prompt and this report are
the only writes, under ignored `AI-Logs/`.

1. **Levels 1–4:** `ExitZoom` projects around logical `(400, CenterY)`, then
   adds each level's accepted vertical placement. The effective fixed logical
   focal point is `(400, CenterY + placement)`. The camera maps it to host
   `(640, 1.26 * (CenterY + placement) - 39.72)`.
2. **Levels 8–9:** the ROM adapter also has a fixed focal point at logical
   `(400, CenterY + placement)`, after its final translation. That point is
   not the far-opening center: at zoom onset the openings are already 165.220
   and 180.556 logical pixels above it, respectively. Their offsets are then
   magnified using the **near-depth denominator**, causing very large drift.
3. **Level 5:** the focal anchor is not fixed while K/ZOffset changes. Its
   exact logical Y is
   `CenterY + placement + K * YOffset / (Position + ZOffset)`.
   Because `YOffset=-90` and the denominator falls from 55 to 9, the anchor
   itself moves from 436.857 to -148.597 before K growth starts.
4. **Why the controls look coherent:** Levels 1–4 use the point's own
   projected depth denominator. Their far-rim centroid displacement is
   divided by `farDepth + ZOffset` (409 during K growth), rather than by the
   near-rim denominator (9). Their computed drift is nonzero, but far smaller:
   about 0.9–1.4 logical pixels at the end of the Z phase, versus 71.6 for
   Level 5 and 844.5/922.8 for Levels 8/9.
5. **Root cause:** the failing mappings tie the presentation's relevant
   vertical displacement or anchor to a live near-rim scale. This is not a
   generic “all post-projection translations are wrong” defect. The accepted
   placements are constant additions that move the far opening and the focal
   point together; they do not cause the measured drift.
6. **Regression boundary:** the pre-adapter parent
   `385f603a94391756b2ca6cccd524dc17b281e782` has no Atari adapter file.
   `7fb0819cdccddbf0f3e4fbc40409023050546d9d` introduced the tracked
   whole-game adapter and renderer integration, including the Level 5
   near-anchor transform and the Level 8/9 near-depth-normalized projector.
   The later Level 8/9 closeout commits added accepted documentation/tests and
   only minor adapter comments; they did not correct or alter these equations.
   The earlier broad presentation closeouts say there were no observed
   regressions, but do not establish that these ExitZoom focal trajectories
   were tested.
7. **Simulation:** exit delay, AvoidSpikes gates, player travel, K/ZOffset
   cadence, collisions, and handoff remain shared production behavior. The
   cadence implementation predates the presentation adapter and the targeted
   deterministic exit/campaign tests pass.
8. **Correction evidence:** sufficient to prototype one bounded,
   presentation-only ExitZoom pivot correction. A shared path can latch each
   level's mapped far-rim/opening center at ExitZoom onset and compensate the
   post-adapter presentation so that point remains fixed. No correction was
   implemented or tested here; do not retune placements or projection values.

The user-described “wrong focal point” is treated as a **MANUAL DIAGNOSTIC
HYPOTHESIS**. The formulas and trajectories below establish the mathematical
behavior; they do not claim arcade equivalence.

## Scope, evidence, and preservation

- **ARCADE-OBSERVED / MANUAL REGRESSION OBSERVATIONS:** the level grouping and
  exit symptom are Neal's observations in the task. This investigation did not
  launch the game or independently observe gameplay.
- **DEVELOPMENT-ONLY:** all state samples are bounded evaluations of the
  production presentation equations at the production ExitZoom cadence.
- **PROJECT TRANSLATION:** the adapter, model-frame mappings, portrait camera,
  and accepted placements are C# presentation choices, not claims about Atari
  hardware projection.
- **ROM-BACKED INPUTS:** the level metadata and raw geometry noted below retain
  their existing provenance; the adapted equations are not thereby
  ROM-backed.

Only these four authorized Level 5 frames were inspected:

- `UserData/Screenshots/Level-5/Broken Exit/playback-level-5-boundary-007108.png`
- `UserData/Screenshots/Level-5/Broken Exit/playback-level-5-boundary-007125.png`
- `UserData/Screenshots/Level-5/Broken Exit/playback-level-5-boundary-007140.png`
- `UserData/Screenshots/Level-5/Broken Exit/playback-level-5-boundary-007203.png`

Objective characterization: 007108 shows the complete tube with the Blaster
near the converging center. In 007125 and 007140 the rails flare much farther
out from the compact Blaster as the view advances. By 007203 the tube is
strongly enlarged/cropped, with its rail envelope extending beyond the frame.
This is consistent with increasing separation between the exit opening and
the zoom pivot. No pixel coordinates were measured, and no constants were
derived from the images.

No other visual assets, videos, recordings, contact sheets, or protected ROM
files were inspected. `TEMP/myNotes.md` was not accessed.

## Starting Git state

Verified before investigation:

- Branch `master`, HEAD
  `753728e79ed86624c9b1728dfd1c8db8f49d1e2b`
- Upstream `origin/master`, ahead/behind `0/0`
- Index clean
- Existing tracked modification preserved: `.github/copilot-instructions.md`
- Existing untracked references preserved:
  `Reference/arcade/MBUCOD.V05.txt` and `Reference/arcade/MBUDOC.DOC`

No Git operation that changes repository state was performed.

## Shared equations and exact focal point

All levels use `Position=50`, `Depth=400`; the near and far tube sections are
therefore at depths 50 and 450. The production ExitZoom samples use the
following live camera values:

| Sample | ExitZoom updates | K | ZOffset | Player depth |
|---|---:|---:|---:|---:|
| Last Playing / ExitDelay | 0 | 70 | 5 | 400 |
| First ExitZoom update | 1 | 70 | 3 | 397 |
| End of ZOffset phase | 23 | 70 | -41 | 331 |
| First K-growth update | 24 | 72 | -41 | 327.8 |
| Mid K growth | 73 | 240 | -41 | 171 |
| Late, before handoff | 123 | 440 | -41 | 11 |

This cadence follows `UpdateExitZoom`: ZOffset is decremented by 2 and player
depth by 3 until ZOffset becomes -41; then K grows by 2 (and by an additional
2 after K exceeds 100) while depth falls by 3.2. The handoff occurs on the
next update after the late sample, when player depth falls below 10. Level 4
and 5's AvoidSpikes gate resets camera/player state while waiting; after it
transitions through the unchanged 21-tick ExitDelay, their travel uses these
same samples.

Let `C=CenterY`, `t=accepted placement`, `N=Position+ZOffset`, and
`F=Position+Depth+ZOffset`. For Levels 1–4 the adapter's effective vertical
equations at a point `(y,z)` reduce to:

```text
screenY = C + t + K * (y + YOffset) / (z + ZOffset)
```

Level 2 additionally multiplies the point's Y displacement by its accepted
1.314 aspect factor; it does not affect the centered tube/opening points
calculated here. Changing K or ZOffset leaves the focal location `C+t` fixed.
The Level 1–3 far-rim centroids have `y=0`; Level 4's centroid is `y=14.5`.
Their displacement from the focal point still uses each point's far-depth
denominator `F`, not the live near denominator `N`.

Level 5 projects, obtains the near-depth anchor, then maps the point's
displacement around that anchor with vertical gain `g=.88`:

```text
nearAnchorY = C + K * YOffset / N
screenY = nearAnchorY + t + g * (projectedY(z) - nearAnchorY)
```

For the centered Level 5 far rim this becomes:

```text
screenY = C + t + K * YOffset * ((1-g)/N + g/F)
effective pivot Y = C + t + K * YOffset / N
```

The pivot therefore moves as N and K change. It is not simply `C+t`.

For Levels 8–10 the adapter's equation can be factored exactly as:

```text
screenY = C + t + K/N * B(y, reciprocalDepth, metadata)
```

where `B` is the code's near-origin baseline and ROM-metadata/reciprocal
projection expression, independent of K and ZOffset. The fixed focal point is
therefore `C+t`, but the far-opening displacement `K/N * B` is large and
changes rapidly as N falls to 9. The far endpoint's `B` is not zero.

### Playfield camera and host mapping

`DisplayConfiguration.ExperimentalPortraitPlayfieldCamera` is shared by all
levels:

- Target `(400, 412.476190...)`
- Offset `(640, 480)`
- Zoom `1.26`
- Host mapping: `xHost = 1.26*xLogical + 136`; `yHost = 1.26*yLogical - 39.72`

Thus the camera is neither per-level nor animated during ExitZoom. Its affine
mapping preserves logical drift multiplied by 1.26; it is not the cause.

### Fixed versus moving projection centers

| Level | Focal Y in logical space | Host Y | Opening - focal at onset | Behavior as K/ZOffset change |
|---|---:|---:|---:|---|
| 1 | 522.000 | 618.000 | -12.308 | Fixed |
| 2 | 514.000 | 607.920 | -12.308 | Fixed |
| 3 | 514.294 | 608.291 | -12.308 | Fixed |
| 4 | 495.203 | 584.236 | -7.769 | Fixed |
| 5 | `551.4026 - 90*K/(50+ZOffset)` | Varies | +88.615 at onset | Moves with live near anchor |
| 8 | 512.281 | 605.755 | -165.220 | Fixed; far opening does not coincide |
| 9 | 480.968 | 566.300 | -180.556 | Fixed; far opening does not coincide |
| 10 | 350.000 | 401.280 | +133.540 | Fixed; diagnostic far opening does not coincide |

Level 5's focal Y through the sample sequence is `436.857, 432.535,
-148.597, -168.597, -1848.597, -3848.597` logical pixels (host:
`510.720, 505.274, -226.953, -252.153, -2368.953, -4888.953`).

The geometry/model origin is `(0,0)` in the C# model frame. For Levels 1–5
the listed far-rim centers are the actual vertex centroids (Level 4's is
`y=14.5`, not zero). The open Level 8–10 source points are centered/scaled
from raw `(128,128)` with reflected Y; their all-vertex centroids are,
respectively, `y=0`, `85.714286`, and `19.642857`. These centroids differ
from the midpoint of the two deep-opening endpoints for each open tube.

For Levels 1/3/4, source `x=0, y=-YOffset` maps to the focal point before
placement; for Level 2 its accepted Y-aspect makes the corresponding source
coordinate `y=-YOffset/1.314`. The model origin `(0,0)` is not generally the
projection focal point. Level 5's near anchor is explicitly the projected
model point `(0,0)` at near depth, and that anchor moves with K/ZOffset.

The ROM adapter's internal Y reference is
`yOriginDelta = orientation * YOffset / unitsPerRawUnit + 128 - CameraHeightRaw`.
For Levels 8/9 it is -48 raw units; for diagnostic Level 10 it is +48. The
adapter combines that reference, signed ROM Y2D, reciprocal depth, and the
mapped model point before adding the accepted placement. It does not expose a
single source-space point that is the far-opening pivot.

## Working/failing presentation matrix

Common values: X projection center is logical 400 with no per-level X
translation; level `Position=50`, `Depth=400`; the playfield camera above is
shared. `CenterY+t` is the effective focal point except for Level 5's moving
anchor. Metadata is the selected ROM row, raw reciprocal scale, raw camera
height, signed Y2D, and open flag. Metadata does not mean its Atari projection
equation is used by every level.

| Level / role | Geometry / topology; CenterY, YOffset | Model mapping and axis treatment | Metadata / active adapter route | Post-projection and accepted Y placement | Exit gate / deep point |
|---|---|---|---|---|---|
| 1 — working closed control | C# 16-point circle, closed; 400, -80 | `(u=25/14, Y=+1)` available; no axis swap/reflection in geometry | Row 0: scale 24, camera 80, Y2D -192, closed. Atari route selects Level 1's C# depth equation, not the ROM reciprocal equation. | +122 | Standard; far-rim centroid `(0,0)` |
| 2 — working control | C# square, closed; 400, -80 | `(u=1.436688312, Y=+1)`; no swap/reflection | Row 1: 28, 80, -224, closed. Level 2 C# depth equation plus 1.314 Y aspect. | +114 | Standard; far-rim centroid `(0,0)` |
| 3 — working control | C# plus, closed; 400, -80 | `(u=1.756696429, Y=+1)`; no swap/reflection | Row 2: 24, 80, -192, closed. Level 3 C# depth equation. | +114.29437 | Standard; far-rim centroid `(0,0)` |
| 4 — working nontrivial control | C# binoculars, closed; 380, -65 | Stored independent-axis correspondence `(2.081778520, 2.231150659)`, refs `(128,121.501111)`, orientations `(+,+)`; actual renderer branch uses C# x/y directly, with no axis swap/reflection | Row 3: 15, 104, -128, closed. Level 4 C# depth equation. | +115.20346 | AvoidSpikes (counter threshold 70); far-rim centroid `(0,14.5)` |
| 5 — failing | C# cross, closed; 415, -90 | No raw-model mapper. Special `.67 X/.88 Y` displacement scaling around the live projected near anchor; no axis swap/reflection. | Row 4: 24, 80, -192, closed, but the Level 5 renderer branch does not consume ROM metadata. It calls `ProjectLevel5`. | +136.4026 after near-anchor projection; accepted placement is frozen. | AvoidSpikes (counter threshold 150); far-rim centroid `(0,0)` |
| 8 — failing accepted open tube | Rev. 2 shape 7, 16 vertices, open; 300, 0 | `u=25/14`, Y orientation -1; LevelLoader centers/scales raw coordinates and reflects Y, no X/Y axis swap. Deep opening endpoints average `(0,-200)`. | Row 7: 24, 176, +96, open. Shared ROM-metadata adapter. | +212.28139 after projection; accepted placement is frozen. | Standard; open endpoint midpoint is the deep point |
| 9 — failing accepted open tube | Rev. 2 shape 13, 16 vertices, open; 300, 0 | `u=25/14`, Y orientation -1; centered/scaled and reflected Y, no axis swap. All-vertex centroid `(0,85.714286)`; deep opening endpoints average `(0,-28.571429)`. | Row 13: 12, 176, +320, open. Shared ROM-metadata adapter. | +180.96825 after projection; accepted placement is frozen. | Standard; open endpoint midpoint is the deep point |
| 10 — diagnostic comparison | Rev. 2 shape 9, 16 vertices, open; 300, 0 | `u=25/14`, Y orientation -1; centered/scaled and reflected Y, no axis swap. All-vertex centroid `(0,19.642857)`; deep opening endpoints average `(0,-200)`. | Row 9: 24, 80, -224, open. Same ROM-metadata adapter as 8/9. | +50 after projection; accepted diagnostic placement is frozen. | Inherits Level 7 AvoidSpikes gate (counter threshold 30); calculated only, not launched or manually retested |

Level 7 is excluded as a behavioral control. Its separate presentation
architecture/issues are not needed to explain these paths.

## Deterministic far-opening and envelope trajectories

`N` is the near-rim vertex centroid. `F` is the far-rim vertex centroid. For
Levels 1–5, which are closed, `O=F` is the deep-point diagnostic. For open
Levels 8–10, `O` is the midpoint of the two deep/open endpoints; it is not
the centroid of all 16 vertices. `E` is the logical envelope-center
diagnostic `(N+F)/2`. All coordinates are logical pixels; `O host` is after
the actual portrait camera. `dO` is drift from the same opening's
ExitZoom-onset coordinate. The expected fixed point for the stability test is
`O at onset`, not an assumption that the opening initially equals the
projector's focal point.

| Level | Sample | N | F | O | E | O host | dO logical / host |
|---|---|---:|---:|---:|---:|---:|---:|
| 1 | onset | 420.182 | 509.692 | 509.692 | 464.937 | 602.492 | 0 / 0 |
| 1 | Z first | 416.340 | 509.638 | 509.638 | 462.989 | 602.424 | -0.054 / -0.068 |
| 1 | Z end | -100.222 | 508.308 | 508.308 | 204.043 | 600.748 | -1.384 / -1.744 |
| 1 | K first | -118.000 | 507.917 | 507.917 | 194.958 | 600.255 | -1.775 / -2.237 |
| 1 | K mid | -1611.333 | 475.056 | 475.056 | -568.139 | 558.851 | -34.636 / -43.641 |
| 1 | K late | -3389.111 | 435.936 | 435.936 | -1476.587 | 509.560 | -73.756 / -92.932 |
| 2 | onset | 412.182 | 501.692 | 501.692 | 456.937 | 592.412 | 0 / 0 |
| 2 | Z first | 408.340 | 501.638 | 501.638 | 454.989 | 592.344 | -0.054 / -0.068 |
| 2 | Z end | -108.222 | 500.308 | 500.308 | 196.043 | 590.668 | -1.384 / -1.744 |
| 2 | K first | -126.000 | 499.917 | 499.917 | 186.958 | 590.175 | -1.775 / -2.237 |
| 2 | K mid | -1619.333 | 467.056 | 467.056 | -576.139 | 548.771 | -34.636 / -43.641 |
| 2 | K late | -3397.111 | 427.936 | 427.936 | -1484.587 | 499.480 | -73.756 / -92.932 |
| 3 | onset | 412.476 | 501.987 | 501.987 | 457.231 | 592.783 | 0 / 0 |
| 3 | Z first | 408.634 | 501.932 | 501.932 | 455.283 | 592.715 | -0.054 / -0.068 |
| 3 | Z end | -107.928 | 500.602 | 500.602 | 196.337 | 591.039 | -1.384 / -1.744 |
| 3 | K first | -125.706 | 500.211 | 500.211 | 187.253 | 590.546 | -1.775 / -2.237 |
| 3 | K mid | -1619.039 | 467.351 | 467.351 | -575.844 | 549.142 | -34.636 / -43.641 |
| 3 | K late | -3396.817 | 428.231 | 428.231 | -1484.293 | 499.851 | -73.756 / -92.932 |
| 4 | onset | 430.934 | 487.434 | 487.434 | 459.184 | 574.446 | 0 / 0 |
| 4 | Z first | 428.512 | 487.401 | 487.401 | 457.957 | 574.405 | -0.033 / -0.042 |
| 4 | Z end | 102.433 | 486.556 | 486.556 | 294.495 | 573.341 | -0.878 / -1.106 |
| 4 | K first | 91.203 | 486.309 | 486.309 | 288.756 | 573.030 | -1.125 / -1.418 |
| 4 | K mid | -851.461 | 465.568 | 465.568 | -192.946 | 546.896 | -21.866 / -27.551 |
| 4 | K late | -1973.690 | 440.877 | 440.877 | -766.407 | 515.785 | -46.557 / -58.662 |
| 5 | onset | 436.857 | 525.473 | 525.473 | 481.165 | 622.375 | 0 / 0 |
| 5 | Z first | 432.535 | 524.900 | 524.900 | 478.717 | 621.654 | -0.572 / -0.721 |
| 5 | Z end | -148.597 | 453.848 | 453.848 | 152.625 | 532.128 | -71.625 / -90.247 |
| 5 | K first | -168.597 | 451.060 | 451.060 | 141.231 | 528.616 | -74.412 / -93.759 |
| 5 | K mid | -1848.597 | 216.928 | 216.928 | -815.835 | 233.610 | -308.544 / -388.766 |
| 5 | K late | -3848.597 | -61.800 | -61.800 | -1955.199 | -117.588 | -587.273 / -739.964 |
| 8 | onset | 478.190 | 385.628 | 347.061 | 431.909 | 397.577 | 0 / 0 |
| 8 | Z first | 476.904 | 380.849 | 340.826 | 428.877 | 389.721 | -6.235 / -7.856 |
| 8 | Z end | 303.948 | -261.709 | -497.399 | 21.120 | -666.442 | -844.460 / -1064.019 |
| 8 | K first | 297.996 | -283.823 | -526.247 | 7.087 | -702.791 | -873.308 / -1100.368 |
| 8 | K mid | -202.004 | -2141.398 | -2949.479 | -1171.701 | -3756.064 | -3296.540 / -4153.640 |
| 8 | K late | -797.242 | -4352.798 | -5834.279 | -2575.020 | -7390.912 | -6181.340 / -7788.489 |
| 9 | onset | 510.508 | 316.574 | 300.413 | 413.541 | 338.800 | 0 / 0 |
| 9 | Z first | 511.631 | 310.371 | 293.599 | 411.001 | 330.215 | -6.813 / -8.585 |
| 9 | Z end | 661.524 | -523.661 | -622.427 | 68.931 | -823.978 | -922.840 / -1162.778 |
| 9 | K first | 666.682 | -552.365 | -653.952 | 57.159 | -863.700 | -954.365 / -1202.500 |
| 9 | K mid | 1100.016 | -2963.476 | -3302.101 | -931.730 | -4200.367 | -3602.513 / -4539.167 |
| 9 | K late | 1615.889 | -5833.847 | -6454.658 | -2108.979 | -8172.589 | -6755.071 / -8511.389 |
| 10 | onset | 454.545 | 525.895 | 483.540 | 490.220 | 569.540 | 0 / 0 |
| 10 | Z first | 458.485 | 532.533 | 488.579 | 495.512 | 575.890 | +5.039 / +6.349 |
| 10 | Z end | 988.889 | 1424.916 | 1166.077 | 1206.902 | 1429.538 | +682.537 / +859.997 |
| 10 | K first | 1007.143 | 1455.628 | 1189.394 | 1231.385 | 1458.916 | +705.854 / +889.376 |
| 10 | K mid | 2540.476 | 4035.426 | 3147.980 | 3287.951 | 3926.735 | +2664.440 / +3357.194 |
| 10 | K late | 4365.873 | 7106.614 | 5479.630 | 5736.243 | 6864.613 | +4996.090 / +6295.073 |

The far point does not remain perfectly fixed even for Levels 1–4 during K
growth: its late drift is approximately 47–74 logical pixels. Their
distinguishing result is the much slower, far-depth-based response and a
closed, centered far rim—not exact zero motion. The sign of Level 10's drift
differs because its geometry/metadata produce a positive far-opening offset;
it has no compensating focal transform.

For the focal-stability test, the next table is `O - effective pivot` in
logical pixels in the same six-sample order as above. Multiply by 1.26 for
host-space displacement from the pivot. Level 5 uses its moving anchor;
the other levels use the fixed point in the focal table.

| Level | Onset | Z first | Z end | K first | K mid | K late |
|---|---:|---:|---:|---:|---:|---:|
| 1 | -12.308 | -12.362 | -13.692 | -14.083 | -46.944 | -86.064 |
| 2 | -12.308 | -12.362 | -13.692 | -14.083 | -46.944 | -86.064 |
| 3 | -12.308 | -12.362 | -13.692 | -14.083 | -46.944 | -86.064 |
| 4 | -7.769 | -7.802 | -8.647 | -8.894 | -29.635 | -54.326 |
| 5 | +88.615 | +92.365 | +602.445 | +619.658 | +2065.526 | +3786.797 |
| 8 | -165.220 | -171.455 | -1009.680 | -1038.528 | -3461.760 | -6346.561 |
| 9 | -180.556 | -187.369 | -1103.395 | -1134.921 | -3783.069 | -6935.626 |
| 10 | +133.540 | +138.579 | +816.077 | +839.394 | +2797.980 | +5129.630 |

## Level-specific transform order and commutation

The renderer's point pipeline is:

```text
LevelDefinition.Points
-> active route in VectorRenderer.ProjectWorldPoint
-> model-frame/level adapter (or Level 5 special adapter)
-> final accepted logical presentation translation
-> fixed portrait playfield camera
-> host viewport
```

During ExitZoom, the adapter is reevaluated with live K and ZOffset. ExitZoom
does not move the tube geometry or apply a second camera object. The player
depth changes for Blaster rendering; the tube's near/far projection depths
remain the level's position and position-plus-depth.

For Levels 1–4, the adapter directly uses the level's C# geometry and a
per-point `K/(z+ZOffset)` scale. For Level 5, it first applies the C# projector,
then rescales displacement around a newly projected near anchor. Levels 8–10
use a reciprocal-depth model projection whose complete logical scale is
normalized by the live near denominator `Position+ZOffset`. The accepted
placements for Levels 1–5 and 8–10 are then added in logical screen space.

An additive translation after projection **does commute** with changing the
projector's logical center by that same constant:

```text
P(y,z) + t == P_with_centerY_plus_t(y,z)
```

It adds the same displacement to the focal point and the opening; it cancels
from their relative drift. It does **not** commute with moving source geometry
or changing `YOffset`, because those terms are multiplied by depth-dependent
scale. Level 5's `.88` transform is not just a translation: its near anchor
depends on the live projection, so its effective pivot moves with K/ZOffset.
Levels 8/9's final placements are genuine post-projection translations and
are correct for ordinary placement; they do not cause the exit divergence.

The user's core hypothesis is therefore **partly supported, but not in its
generic form**: the failing paths have depth-dependent adapter transforms
before their final placement, and their ExitZoom focal relationships do not
hold the deep opening fixed. However, the post-projection placements
themselves are not at an inappropriate stage; an additive placement simply
moves the complete projected result, including its focal point.

## Level 5 and AvoidSpikes

Level 5 differs from working Level 4 in more than geometry: it changes
`CenterY/YOffset` to `(415,-90)`, uses a cross rather than binocular geometry,
and introduces the special `.67/.88` near-anchor adapter. Those first two
values affect the normal projected point; the decisive exit distinction is
the live near-anchor term. The Level 5 branch does not use a model-frame
reflection or axis swap. Its ROM metadata is not the source of the
near-anchor computation.

AvoidSpikes is **not** the cause. Level 4 also passes through AvoidSpikes and
then uses the same shared ExitZoom K/ZOffset cadence as the standard-gate
levels. Level 5's gate resets player depth and restores projection while
waiting. Once travel begins, the focal point moves because of the Level 5
presentation equation even if the spike lane is clear and no collision or
ReverseZoom occurs. Spike collision can cancel a particular exit attempt, but
does not explain the continuous camera drift on an active successful exit.

## Level 8/9 placements and Level 10 comparison

The accepted Level 8 `+212.28139` and Level 9 `+180.96825` values are
post-projection logical translations. They shift the fixed focal point and
opening equally, so neither changes `opening - focal` nor the calculated
onset-relative drift. They must remain frozen. Their equations still magnify
the opening's offset using `K/(Position+ZOffset)`, not the far-depth
denominator; the deep endpoint consequently leaves its onset location quickly.

Diagnostic Level 10 takes the same general ROM-adapter route as Levels 8/9,
with the frozen `+50` placement. Its computed opening moves in the opposite
direction because its metadata/geometry make its onset opening lie below the
adapter focal point rather than above it. This is evidence against a special
Level 8/9 placement issue, not a reason to alter Level 10. Level 10 was not
launched or manually retested.

## Regression history and simulation status

Read-only history establishes:

- Parent `385f603a94391756b2ca6cccd524dc17b281e782` has no tracked
  `AtariPresentationAdapter.cs`; it is the last pre-adapter architecture.
- Commit `7fb0819cdccddbf0f3e4fbc40409023050546d9d` introduced the adapter,
  its Level 5 near-anchor route, and the ROM routes for Levels 8/9/10 as part
  of the whole-game Atari-presentation milestone.
- The Level 8 closeout `60b1e95f080ff9a3f0c78d7b000fd12e22cf0d4b` and Level 9
  closeout `bcd79da63021821c4e31a2a646152a70ccc443e5` added tests/docs and
  only small adapter comment changes. The projector equations were already
  present at `7fb0819`.
- The prior Level 10 and Level 8/9 reports record broad manual acceptance,
  but do not supply these ExitZoom focal-point measurements. Current manual
  regression observations are not contradicted by that missing specific test.

`git blame` places the ZOffset/K cadence in October 2026, before the
presentation adapter. `UpdateExitZoom` still decrements ZOffset by 2 while
advancing player depth by 3; after the offset crosses -40, it grows K with the
existing 2/4 cadence while advancing depth by 3.2. `AvoidSpikes`, spike
collision, ExitDelay, player/lane input, scoring/lives, RNG, playback and
Approach handoff were not modified by this investigation. No evidence
indicates level-specific simulation evolution is the cause.

## Smallest future correction direction and boundaries

Evidence is sufficient for one bounded **presentation-only** experiment:
capture the active route's mapped far-rim/deep-opening center at ExitZoom
onset, then use a shared ExitZoom-only presentation pivot/compensation so
that mapped point remains fixed while the rest of the tube expands. It should
operate after the active level adapter and before the shared host camera, and
leave ordinary Playing coordinates untouched. The implementation would need
to calculate the anchor through each active adapter (closed far-rim center
for Levels 1–5; open-endpoint midpoint for 8/9; diagnostic only for 10).
This provides a common invariant without per-level placement retuning.

This is a technically justified direction, not an accepted correction. The
calculated trajectory does not establish perceived fidelity, the exact
user-intended anchor point, or whether the same pivot must apply to
ReverseZoom/Approach. Future work must preserve accepted Playing placements,
projection constants, Blaster orientation and scale; Neal's manual acceptance
would still be required.

The Blaster Exit/Approach scale closeout
`2026-11-17-blaster-exit-approach-scale-closeout-report.md` remains frozen.
This tube/opening focal defect is distinct; no Blaster scale or geometry was
retuned.

## Validation and final state

- **PASS** — bounded projection calculations followed the current C# equations
  for all listed levels and samples; no image-derived constants were used.
- **PASS** — deterministic exit/campaign tests:
  `dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~CampaignSoakTests|FullyQualifiedName~ExitLaneControlTests" --blame-hang-timeout 4m --verbosity minimal`
  completed with 28 passed, 0 failed, 0 skipped.
- The interactive game was not launched. No manual retest was performed or
  required for this read-only investigation.
- **PASS** — project/gameplay and Git state remain unchanged. The index is
  clean; the pre-existing copilot-instructions modification and two untracked
  reference files remain preserved. AI-Logs remain ignored and unstaged.

AI-Logs:

- Prompt: `AI-Logs/Prompts/2026-11-17-exit-focal-point-regression-investigation-prompt.md`
- Report: `AI-Logs/Reports/2026-11-17-exit-focal-point-regression-readonly-report.md`
