# Open-Tube ExitZoom Pivot Investigation

Date: 2026-11-18

## Result

**READ-ONLY INVESTIGATION COMPLETE.** No source, tests, project configuration,
durable documentation, presentation, gameplay, recordings, or Git state were
changed. The only writes are this report and the byte-identical copy of the
user task in `AI-Logs/Prompts/`.

1. **No—the terminal-endpoint midpoint is not the correct pivot for the
   current Level 8/9/10 candidate.** Manual results say the defect remains; the
   midpoint holds the omitted closing-chord center while the actual mapped
   rail-convergence point continues to move. More generally, ROM evidence
   does not assign terminal midpoint a focal role for open tubes.
2. **The current adapter does not distort the tube shape as K/ZOffset change.**
   Its open-tube mapping is a uniform similarity at every fixed depth. The
   terminal-midpoint compensation therefore expands coherently, but around a
   point that the supplied manual observation says is visually wrong.
3. **Levels 8/9/10 do not share one numeric 2D pivot after their accepted
   placements.** They do share the same projection equations, which produce a
   common rail-convergence point and a common homothety-origin rule.
   Geometry-derived centers likewise follow one rule but have level-specific
   coordinates.
4. **Best presentation-space focal candidate to test:** the common
   intersection of the mapped near-to-far tube rails. All 16 rails are
   concurrent under this adapter. This is an equation-derived
   **PROJECT TRANSLATION**, not an Atari-defined or visually accepted answer.
   The arc-length-weighted centroid remains the best geometry-derived center
   of the visible open polyline, but it is not the rail focus.
5. **One shared translation is mathematically sufficient** to hold any chosen
   far-polyline pivot while preserving coherent expansion of all mapped far
   and near geometry. No anisotropic or shape correction is indicated.
6. **Keep the manually accepted Level 5 candidate unchanged.** Its closed,
   symmetric far rim has a meaningful mapped center and its compensation has
   passed Neal's manual check.

The evidence supports a narrow implementation prompt to latch and compensate
the mapped rail-convergence point, while preserving Level 5 and all accepted
Playing placement. It does **not** establish that this is the final visually
correct arcade-fidelity pivot; Neal's manual acceptance would still be
required.

## Evidence classification and scope

- **MANUAL PROJECT OBSERVATIONS supplied in the task:** Level 5 now looks
  correct; Levels 8/9/10 remain wrong in the same general way after terminal
  midpoint compensation. These are authoritative project observations, not
  claims of direct original-arcade comparison.
- **ROM-BACKED data / source structure:** local Rev. 2 disassembly documents
  the open flag, tube geometry arrays and presentation metadata fields. The
  project catalog and loaders supply the recovered level data.
- **PROJECT TRANSLATION:** C# model normalization, reciprocal projector,
  accepted per-level placement and ExitZoom compensation.
- **INFERENCE:** the arc-length-weighted centroid is the most stable
  geometry-derived center of an open visible polyline. Neither ROM source nor
  the supplied manual evidence selects it as the final perceptual pivot.

Primary local evidence:

- `src/Tempest.Game/Levels/LevelLoader.cs` — ordered open geometry for Levels
  8/9/10 and model normalization.
- `src/Tempest.Game/Levels/RomPresentationMetadata.cs` —
  per-level table rows and signed Y2D interpretation.
- `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs` — model-frame
  mapping, reciprocal-depth projection and accepted placements.
- `src/Tempest.Game/Rendering/ExitZoomFocalCompensation.cs` and
  `src/Tempest.Game/Rendering/VectorRenderer.cs` — current candidate pivot and
  shared logical-space translation path.
- `src/Tempest.Game/Levels/LevelDefinition.cs`,
  `src/Tempest.Game/Levels/Level.cs`, and
  `src/Tempest.Game/Rendering/VectorRenderer.cs` — open topology and 15-lane
  near/far line construction.
- `Reference/arcade/Tempest Rom Source code.md` — Rev. 2 table definitions,
  setup, open flag, vector construction, and endpoint/midpoint handling.
- `AI-Logs/Reports/2026-11-17-exitzoom-focal-compensation-implementation-report.md`
  — controlling implementation handoff and bounded K/ZOffset samples.

No raw ROM, screenshots, images, videos, frames, contact sheets, or
`TEMP/myNotes.md` were inspected. No internet or Incitti evidence was used.

## What “far opening” means

An open definition is an ordered set of **16 cross-section vertices**
`P[0]..P[15]`. Its visible far end is the polyline formed by projecting all
16 points at `Position + Depth` and drawing segments `P[0]-P[1]` through
`P[14]-P[15]`. There is no visible `P[15]-P[0]` closing edge. The same 16
source vertices are also projected at the near depth and joined to their
far-depth counterparts by the tube rails. The C# definition exposes 15
playable lanes; `NextEdge(14)` is vertex 15, not vertex 0.

The local disassembly defines `lev_open` as `$00` closed / `$ff` open. Its
setup loads all 16 tube points and generates adjacent midpoints; the vector
builder branches on `lev_open` when choosing its initial point. The open flag
and geometry do not designate a focal point. The midpoint between terminal
points can be derived as the midpoint of the absent closing chord (and
wrapped midpoint bookkeeping exists), but that chord is not a visible far
rim or playable lane. It has no established ExitZoom-center significance.

For candidate terminology:

- “Terminal midpoint” means `(mapped P[0] + mapped P[15]) / 2`.
- “All far vertices” means all 16 mapped points. Their arithmetic centroid is
  not the terminal midpoint.
- “Far envelope center” means the midpoint of the min/max X and Y bounds.
- “Length-weighted center” means the sum of each visible segment midpoint
  weighted by that segment's mapped length, divided by total visible length.
- The open line has no enclosed area, so it has no unique polygon-area
  centroid. The bounding-box center, vertex centroid and length-weighted
  centroid are distinct legitimate constructions, not interchangeable
  authorities.

## Geometry, metadata and placements

All three definitions have 16 ordered points / 15 visible far segments.
Their C# source coordinates use
`x = (rawX - 128) * 25/14`,
`y = -(rawY - 128) * 25/14`; the shared model-frame mapping is `(25/14, -1)`.
Each uses `CenterY=300`, `YOffset=0`, `Position=50`, and `Depth=400`.

The ordered ROM-coordinate point sets below are the inputs used for the
calculations (hexadecimal, in source order):

| Level | Raw X vertices | Raw Y vertices |
|---|---|---|
| 8 | `10 1E 2C 3A 48 56 64 70 90 9E AC BA C8 D6 E4 F0` | `F0 D0 B0 90 70 50 30 10 10 30 50 70 90 B0 D0 F0` |
| 9 | `10 10 30 30 50 50 70 70 90 90 B0 B0 D0 D0 F0 F0` | `90 70 70 50 50 30 30 10 10 30 30 50 50 70 70 90` |
| 10 | `10 10 10 10 16 29 46 69 97 BA D7 EA F0 F0 F0 F0` | `F0 CB A6 80 5C 39 20 12 12 20 39 5C 80 A6 CB F0` |

| Displayed level | Status | ROM row | Scale | Camera Y3D | signed Y2D | Open | Accepted final Y |
|---|---|---:|---:|---:|---:|---:|---:|
| 8 | production | 7 | `0x18` | `0xB0` | +96 | yes | +212.28139 |
| 9 | production | 13 | `0x0C` | `0xB0` | +320 | yes | +180.96825 |
| 10 | diagnostic only | 9 | `0x18` | `0x50` | -224 | yes | +50 |

Level 10 remains diagnostic Level-7-like gameplay on genuine Level 10
geometry; its accepted +50 placement and gameplay status are untouched.

## Candidate-point tables at normal Playing / ExitZoom onset

Both states use the same baseline `K=70`, `ZOffset=5`; consequently these
coordinates are both the normal Playing values and ExitZoom-onset values.
“Model” is the candidate coordinate in the normalized source/model frame.
Logical coordinates are after the active Atari adapter and accepted placement,
before the shared portrait camera. Host coordinates apply the current
experimental portrait camera (uniform zoom 1.26; target Y 412.47619).

### Level 8

| Candidate | Model point | Logical point | Host point |
|---|---:|---:|---:|
| Terminal midpoint | `(0.000, -200.000)` | `(400.000, 347.061)` | `(640.000, 397.577)` |
| All-vertex centroid | `(1.339, 0.000)` | `(400.258, 385.628)` | `(640.325, 446.172)` |
| Far-envelope center | `(0.000, 0.000)` | `(400.000, 385.628)` | `(640.000, 446.172)` |
| Length-weighted polyline center | `(1.223, 12.320)` | `(400.236, 388.004)` | `(640.297, 449.165)` |
| Mapped rail-convergence point | not a tube-model point | `(400.000, 369.100)` | `(640.000, 425.345)` |
| Adapter homothety origin | not a tube-model point | `(400.000, 512.281)` | `(640.000, 605.755)` |

### Level 9

| Candidate | Model point | Logical point | Host point |
|---|---:|---:|---:|
| Terminal midpoint | `(0.000, -28.571)` | `(400.000, 300.413)` | `(640.000, 338.800)` |
| All-vertex centroid | `(0.000, 85.714)` | `(400.000, 316.574)` | `(640.000, 359.164)` |
| Far-envelope center | `(0.000, 85.714)` | `(400.000, 316.574)` | `(640.000, 359.164)` |
| Length-weighted polyline center | `(0.000, 93.333)` | `(400.000, 317.652)` | `(640.000, 360.521)` |
| Mapped rail-convergence point | not a tube-model point | `(400.000, 292.332)` | `(640.000, 328.618)` |
| Adapter homothety origin | not a tube-model point | `(400.000, 480.968)` | `(640.000, 566.300)` |

### Level 10

| Candidate | Model point | Logical point | Host point |
|---|---:|---:|---:|
| Terminal midpoint | `(0.000, -200.000)` | `(400.000, 483.540)` | `(640.000, 569.540)` |
| All-vertex centroid | `(0.000, 19.643)` | `(400.000, 525.895)` | `(640.000, 622.908)` |
| Far-envelope center | `(0.000, -1.786)` | `(400.000, 521.763)` | `(640.000, 617.701)` |
| Length-weighted polyline center | `(0.000, 38.071)` | `(400.000, 529.449)` | `(640.000, 627.386)` |
| Mapped rail-convergence point | not a tube-model point | `(400.000, 538.636)` | `(640.000, 638.962)` |
| Adapter homothety origin | not a tube-model point | `(400.000, 350.000)` | `(640.000, 401.280)` |

“Center of the mapped far endpoint set” is not a separate fifth point if
“endpoint set” means the two terminal vertices: it is exactly the terminal
midpoint. If it means all 16 vertices at far depth, it is the all-vertex
centroid. The explicit definitions above avoid conflating them.

At onset, the far-vertex Y envelopes are Level 8 `[347.061, 424.196]`,
Level 9 `[300.413, 332.736]`, and Level 10 `[483.540, 559.986]`. The
rail-convergence point lies within the Level 8 and Level 10 bounding
envelopes; for Level 9 it is 8.081 logical pixels above the far envelope.
It is still relevant to the visible tube because it is the common intersection
of its projected rails, not a point required to lie on the open polyline.

## ExitZoom trajectories and compensation

The following are **uncompensated logical Y** values for each candidate,
ordered by sample:

`onset (70,5)`, `first-Z (70,3)`, `end-Z (70,-41)`,
`first-K (72,-41)`, `mid-K (240,-41)`, `late-K (440,-41)`.

| Level | Candidate | Y trajectory |
|---|---|---|
| 8 | Terminal midpoint | `347.061, 340.826, -497.399, -526.247, -2949.479, -5834.279` |
| 8 | All-vertex centroid | `385.628, 380.849, -261.709, -283.823, -2141.398, -4352.798` |
| 8 | Envelope center | `385.628, 380.849, -261.709, -283.823, -2141.398, -4352.798` |
| 8 | Length-weighted center | `388.004, 383.314, -247.190, -268.890, -2091.622, -4261.541` |
| 8 | Rail convergence | `369.100, 363.696, -362.719, -387.719, -2487.719, -4987.719` |
| 8 | Adapter homothety origin | `512.281, 512.281, 512.281, 512.281, 512.281, 512.281` |
| 9 | Terminal midpoint | `300.413, 293.599, -622.427, -653.952, -3302.101, -6454.658` |
| 9 | All-vertex centroid | `316.574, 310.371, -523.661, -552.365, -2963.476, -5833.847` |
| 9 | Envelope center | `316.574, 310.371, -523.661, -552.365, -2963.476, -5833.847` |
| 9 | Length-weighted center | `317.652, 311.489, -517.077, -545.593, -2940.901, -5792.459` |
| 9 | Rail convergence | `292.332, 285.214, -671.810, -704.746, -3471.413, -6765.063` |
| 9 | Adapter homothety origin | `480.968, 480.968, 480.968, 480.968, 480.968, 480.968` |
| 10 | Terminal midpoint | `483.540, 488.579, 1166.077, 1189.394, 3147.980, 5479.630` |
| 10 | All-vertex centroid | `525.895, 532.533, 1424.916, 1455.628, 4035.426, 7106.614` |
| 10 | Envelope center | `521.763, 528.245, 1399.663, 1429.654, 3948.846, 6947.884` |
| 10 | Length-weighted center | `529.449, 536.221, 1446.633, 1477.965, 4109.884, 7243.121` |
| 10 | Rail convergence | `538.636, 545.755, 1502.778, 1535.714, 4302.381, 7596.032` |
| 10 | Adapter homothety origin | `350.000, 350.000, 350.000, 350.000, 350.000, 350.000` |

All candidate X values remain 400 except Level 8's slight vertex-centroid and
length-centroid offset. For Level 8, their X trajectories are respectively
`400.258, 400.268, 401.578, 401.623, 405.411, 409.921` and
`400.236, 400.245, 401.442, 401.483, 404.943, 409.063`; the terminal and
envelope centers remain at 400. Levels 9/10 have X=400 for all listed
candidates at all samples. The adapter homothety origin is stationary at all
samples; the rail-convergence point moves according to the same expansion
ratio as the tube.

For any mapped candidate `P*`, the current generic compensation applies
`T(t) = P*(onset) - P*(t)`. It would hold that candidate exactly and make the
entire tube expand about its onset position. The common expansion ratios at
the six samples are `1, 1.037736, 6.111111, 6.285714, 20.952381, 38.412698`.
The latching formula is sound; selecting the terminal midpoint holds the
missing-chord center but allows the actual rail-convergence point to continue
moving. Latching the rail-convergence point instead holds the perspective
focus while preserving the same coherent expansion.

## Transform decomposition and near/far consistency

For Levels 8/9/10, the adapter's far mapping simplifies to:

```text
P_i(K, ZOffset) = A_level + q * V_i(far, geometry, ROM metadata)
q = K / (Position + ZOffset)
A_level = (400, CenterY + acceptedVerticalPlacement)
```

For any fixed depth, `V_i` is fixed during ExitZoom: normalized depth and
ROM metadata are unchanged, `CenterY=300`, `YOffset=0`, and the accepted
placement is applied after projection. Every far point therefore obeys:

```text
P_i(t) = A_level + r(t) * (P_i(onset) - A_level)
r(t) = q(t) / q(onset)
```

All corresponding near points obey the same `r(t)` with their own fixed
`V_i(near, ...)`. Thus:

- Translation only cannot describe a sample once `r != 1`; pairwise far and
  near distances change by `r`.
- Translation plus uniform scale is exact around `A_level`. No anisotropic
  scale or higher-order/nonuniform transform is needed.
- The current candidate's added translation moves that natural similarity
  center from `A_level` to the chosen latched far pivot. All 15 visible far
  segments, all 16 rail pairs, and the near geometry remain coherent.
- The shared host camera is affine and uniformly scales by 1.26, so it
  preserves the same similarity. No host-space warp explains the defect.
- The adapter's `PortraitPerspective` value is not part of this adapter
  equation; the analyzed production candidate uses the current default.

The independently evaluated maximum residual of all 16 vertices from the
uniform-similarity equation was below `6e-12` logical units in double
precision. For comparison, a best-fit translation-only model at late K has
far-point maximum residuals of 2047.432 (Level 8), 1218.710 (Level 9), and
2143.140 (Level 10); near-point maximum residuals are 13513.053, 10968.393,
and 14144.724. Those nonzero residuals reflect real uniform expansion, not
shape distortion.

Two distinct presentation points must not be conflated:

- `A_level = (400, CenterY + acceptedVerticalPlacement)` is the natural
  homothety origin of the mapped points. It is fixed without compensation.
- `C_level` is the common intersection of the 16 mapped near-to-far rail
  lines. A least-squares intersection fit over all 16 rails gives the values
  below; every rail's perpendicular residual is at most `1.93e-13` logical
  units at onset. This is the actual perspective convergence point in the
  mapped presentation, and it moves as `A_level + r*(C_onset-A_level)`.

| Level | Rail intersection at onset (logical) | Max rail residual | Rail intersection at onset (host) |
|---|---:|---:|---:|
| 8 | `(400.000, 369.100)` | `1.44e-13` | `(640.000, 425.345)` |
| 9 | `(400.000, 292.332)` | `6.18e-14` | `(640.000, 328.618)` |
| 10 | `(400.000, 538.636)` | `1.93e-13` | `(640.000, 638.962)` |

Although `C_level` may lie outside the open far-polyline envelope (Level 9
does), its exact all-rail intersection makes it directly relevant to the
perceived deep perspective. It is not a tube-model vertex or a ROM-encoded
focal point; it is a **PROJECT TRANSLATION** consequence of the adapter and
mapped geometry.

The current terminal-midpoint compensation does not keep `C_level` fixed.
After that translation, the convergence point becomes
`P_terminal(onset) + r*(C_onset-P_terminal(onset))`. At late K its logical Y
is approximately `1193.622` (Level 8), `-9.993` (Level 9), and `2599.942`
(Level 10). Under the proposed convergence-pivot translation
`T_C(t)=C_onset-C(t)`, it remains exactly at its onset value, and every
mapped point becomes `C_onset + r*(P_i(onset)-C_onset)`. This is the precise
uniform expansion about a stationary perspective focus that the current
candidate was intended to provide.

## Common numeric point versus common rule

No complete candidate point is numerically common to Levels 8/9/10 after
accepted placement. The terminal points are `(400,347.061)`, `(400,300.413)`,
and `(400,483.540)`; rail-convergence points are `(400,369.100)`,
`(400,292.332)`, and `(400,538.636)`; length-weighted centers are
`(400.236,388.004)`, `(400,317.652)`, and `(400,529.449)`. The adapter
homothety origins also differ. Shared X=400 is not a shared 2D focal point.

The shared structural facts are stronger than coincidence: identical
centered/reflected model normalization, same position/depth and adapter route,
same accepted host camera, and the same affine-in-`q` projection form. Metadata
and shape change each candidate's coordinate, but not the common rules:
“length-weighted center of the visible far polyline,” “common intersection of
the mapped rails,” or “adapter homothety origin.” The first two yield
level-specific points by common geometry/presentation rules; the third yields
level-specific numeric origins by the common projection equation. Do not
hand-enter per-level focal coordinates.

## Level 5 closed control

Level 5's far end is a closed 16-vertex rim. Its symmetric cross geometry has
model-space mean `(0,0)`; mapping all far-rim vertices and taking their
arithmetic mean yields the center of the visible closed rim, independent of
the first/last vertex choice. The controlling report gives its onset mapped
center as logical Y `525.473`, and the user now reports that its ExitZoom
focal behavior looks correct. This makes the mapped closed-rim center a
meaningful **empirically accepted project pivot** for Level 5. It does not
establish that a missing chord's midpoint is the equivalent open-tube center.

Level 5's mapping, pivot, ordinary Playing behavior, and accepted result are
out of scope for change. Levels 1-4 were not broadly reinvestigated.

## ROM metadata and limits

The local Rev. 2 listing describes `lev_scale` as reciprocal scale, `lev_y3d`
as camera-height/Y offset, signed `lev_y2d`/`lev_y2db` as 2D Y offset, and
`lev_open` as the closed/open flag. Setup loads those values and the ordered
tube vertices. None is a center or focal-point field. The `y3d`, signed
`y2d`, and open fields are not evidence of an encoded pivot. The adapter uses
them to form its projection; that does not make the C# mapping literal Atari
mathbox/DVG behavior.

**No Atari/ROM evidence found here defines the ExitZoom pivot for an open
tube.** The exact projection-origin and weighted-polyline candidates are
mathematical/project-translation candidates only. The mapped rail intersection
is also equation-derived, not ROM-designated.

## Future test direction

Smallest justified next implementation candidate:

1. Leave the closed-tube branch and manually accepted Level 5 behavior
   unchanged.
2. For open tubes only, map every source point at both near and far depth and
   fit the common intersection of all 16 near-to-far rail lines at onset and
   under live K/ZOffset. Use that point as the latched/current pivot.
3. Apply the existing shared logical translation
   `latchedConvergence - currentConvergence` after the active adapter and
   accepted per-level placement but before the shared host camera. Continue
   applying it uniformly to tube and tube-space gameplay presentation.
4. Add deterministic tests that distinguish terminal midpoint from rail
   convergence, assert the measured rail-fit residual, verify onset
   coordinates, and verify exact uniform scale of all near/far geometry and
   unchanged ordinary Playing. The weighted-centroid calculation can remain
   a diagnostic comparison, not the selected focal rule.
5. Retain the adapter homothety origin `A_level` as the no-compensation
   mathematical baseline. Do not add per-level constants or retune accepted
   placements.
6. Require Neal's manual ExitZoom acceptance on production Levels 8/9 before
   treating the pivot as accepted. Level 10 remains diagnostic-only; no new
   Level 10 acceptance is required.

This direction is a testable **PROJECT TRANSLATION** based on the current
mapped rail convergence, not a claim that the arcade uses this exact pivot.
If the manual result rejects it, evidence still rules out shape distortion as
the cause under this adapter and points to a different desired screen-space
reference, not per-level hand offsets.

## Preservation and Git state

Initial repository state recorded before investigation:

- Branch `master`; HEAD
  `753728e79ed86624c9b1728dfd1c8db8f49d1e2b`; upstream `origin/master`;
  ahead/behind `0/0`, matching the expected baseline.
- Index had no staged changes.
- Pre-existing tracked worktree modifications: `.github/copilot-instructions.md`
  and `src/Tempest.Game/Rendering/VectorRenderer.cs`.
- Pre-existing untracked files:
  `Reference/arcade/MBUCOD.V05.txt`,
  `Reference/arcade/MBUDOC.DOC`,
  `src/Tempest.Game/Rendering/ExitZoomFocalCompensation.cs`, and
  `tests/Tempest.Game.Tests/ExitZoomFocalCompensationTests.cs`.
- These candidate and local reference files were preserved and not attributed
  to this investigation. No staging, commit, push, branch switch, stash, reset,
  restore, or cleanup occurred.
- Accepted Blaster outward orientation, ExitZoom/Approach scale,
  color/brightness, and gameplay were not changed or reopened.
- Accepted Playing placements remain frozen: Level 8 `+212.28139`, Level 9
  `+180.96825`, Level 10 `+50`; all other frozen presentation and gameplay
  surfaces remain untouched.
- No game was launched. No automated test was run because no project code
  changed; the numerical analysis used bounded local calculations only.

Ending repository state was checked after writing the ignored AI-Logs records;
the tracked/untracked project state remained otherwise identical.

## AI-Logs

- Prompt: `AI-Logs/Prompts/2026-11-18-open-tube-exit-pivot-investigation-prompt.md`
  (byte-identical to the complete user attachment)
- Report: `AI-Logs/Reports/2026-11-18-open-tube-exit-pivot-investigation-report.md`
