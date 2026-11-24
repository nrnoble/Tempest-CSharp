# Level 13 legacy-versus-Atari presentation investigation

**Status: PARTIAL — structural cause identified; exact correction not established.**

**Scope classification: SHARED PRESENTATION LIMITATION EXPOSED BY LEVEL 13**

**Implementation readiness: STILL UNRESOLVED**

This is a read-only investigation of normal Playing presentation. It does not
reopen accepted Levels 1-12, propose that legacy is the target, or establish
Neal's manual acceptance. The completion token is
`TEMPEST_LEVEL13_PRESENTATION_DIVERGENCE_READONLY`.

## Executive finding

Both projections receive the same genuine closed Level 13 model, and at the
near plane the neutral Atari projection preserves the same ordered affine
silhouette and scale as the legacy projection, apart from an overall vertical
translation. The model frame does not mirror or rotate it. The material
difference is in the shared Atari projection's depth translation: its converted
camera-height term displaces the Level 13 far model origin **185.124 logical
pixels below the near origin**, while legacy projects both origins to the same
Y. It also gives a far-to-near scale ratio of **0.151515**, versus legacy's
**0.120879**. The first mathematical departure is the different reciprocal
depth function; the first operation creating the much more conspicuous
depth-dependent vertical displacement is the raw camera-origin term, evaluated
through that reciprocal. A signed placement term translates both ends equally.

The reference and candidate images support the distinction: MAME shows the
deep convergence below the near-rim center but substantially less dominated by
downward displacement than the candidate. Neal's observation that legacy is
closer is consistent with the legacy path retaining the useful rim shape
without this large drift. It does **not** establish that the legacy
zero-drift or its depth ratio reproduces MAME. Exact target depth behavior and
an accepted project translation remain unestablished.

The shared projection mechanism is used on accepted levels as well. This is a
limitation exposed here, not evidence that an accepted level should change or
that the raw Atari metadata is wrong. A future remedy must be scoped to
Level 13; no global change is recommended.

## Evidence and visual comparison

- **ARCADE-OBSERVED, bounded to the supplied MAME image:** the visible outline
  has top and bottom V-like indentations between paired points and stepped,
  pointed side protrusions. The deep region is below the near-rim center; the
  upper and lower rails retain room around it. The tube sits below the HUD.
- **CURRENT C# CANDIDATE, bounded to the supplied neutral Atari image:** the
  ordered top indentation and side features have the same recognizable
  orientation; no conspicuous reflection or rotation is apparent. The tube is
  broader in the portrait playfield and its far end is pulled substantially
  toward the lower near rim. Long upper rails converge on that low focal
  region, and lower rails appear compressed and crowd/cross near the lower
  rim. Both images contain a near-rim outline; this is not evidence that
  changing its vertex order would correct the perspective.
- The authorized MAME PNG is 480x640; the C# PNG is a 1600x1280 host with a
  centered 960x1280 portrait viewport. Different image and viewport sizes,
  HUD arrangements, and screenshot timing prevent treating raw image
  coordinates as a direct transform. No screenshot-fitted values were used.
- **USER-REPORTED COMPARISON:** legacy looks closer, but not correct. No
  legacy screenshot was supplied, examined, or numerically measured.

Image assessment is qualitative. The numerical results below are calculations
from C# source and metadata, not measurements of either image.

## Row 14 and model-frame analysis

`src/Tempest.Game/Levels/LevelLoader.cs:149` records 16 ordered raw pairs.
`LevelLoader.cs:385` constructs the closed model using
`s = 25/14`, `(x,y) = (s*(rawX-128), -s*(rawY-128))`, centerY 300 and
YOffset 0. `RomPresentationMetadata.cs:60` maps displayed Level 13 to row 14;
the record catalog and `Level13ProductionTests.cs:39` repeat the same ordered
bytes. These are mutually consistent with the recorded ROM-backed extraction
in the existing implementation report; they are not an independent fresh
verification of ROM binaries. The center/scale/reflection are **PROJECT
TRANSLATIONS**, not a claim that the ROM renders by this exact C# equation.
No evidence found here establishes a row extraction or geometry error.

X spans raw 24..232 around 128, and Y spans 20..230, whose bounding midpoint
is 125 rather than 128. The translated Y bounding-box midpoint is therefore
about +5.357 model units, and small side asymmetries also exist. Thus 128 is a
coordinate origin, not a claim that the polygon is perfectly symmetric or
that its bounds are centered there. This small bounding-center distinction
does not explain the 185.124 logical depth-origin shift.

`AtariPresentationAdapter.cs:494` selects `(25/14, -1)` for Levels 8-13 and
direct 15. It was reused as the established raw normalization, rather than
fitted specifically to Level 13. For Level 13 it exactly recovers
`xRawDelta = rawX-128` and, before the camera term,
`yOrientation*y/s = rawY-128`. The later multiplication by the same Y
orientation returns screen shape orientation to the original C# model; the
frame's Y reflection therefore does not flip the near rim. Closed
construction joins vertex 15 back to vertex 0 and uses this same point list;
it does not require the polygon to be symmetric. Accepted Level 12 uses the
same frame and loader translation, but its acceptance is not evidence for a
Level 13-specific fitted frame.

## Metadata and projection chain

The captured Level 13 metadata at `RomPresentationMetadata.cs:89` is shape
14, scale `0x18` = 24, camera height `0x20` = 32, signed little-endian Y
`0xFEA0` = -352, closed `0x00`, Flipper scale `0x0400`. This matches the
existing candidate report and Level 12's values. Topology is checked against
the closed definition. The shared adapter uses the scale, camera height and
signed Y as inputs to a **PROJECT TRANSLATION**, not literal emulation.
Flipper scale is captured in metadata but is not consumed by this tube
projection; no inference is made here about its other presentation semantics.
No binary ROMs were opened.

Normal Playing has position 50, depth 400, hence near z=50 and far z=450;
the normal camera is K=70 and ZOffset=5. `Projection.PortraitPerspective`
defaults to zero. A nondefault diagnostic perspective would change legacy,
but is not assumed here. The following coordinates are logical playfield
coordinates before viewport transform. They describe the same input points:

1. **Legacy:** `Projection.Project` returns
   `(400 + x*70/(z+5), 300 + y*70/(z+5))`. Its near local scale is 70/55
   = 1.272727 and far local scale is 70/455 = 0.153846. Both origin
   projections are `(400,300)`. No shape-dependent vertical placement exists.
2. **Atari frame and depth:** `u=25/14`, orientation `-1`, normalized
   `t=(z-50)/400`, `q=16+224*t`, denominator `q+24`, and
   `L=u*70*40/(256*55)=0.355113636`. Thus near denominator is 40 and far
   denominator is 264. A point's local scale is
   `L*256/(u*(q+24))`: 1.272727 near, 0.192837 far.
3. **Camera-origin mapping:** raw Y displacement is `-y/u + 96`, because
   `128-cameraHeight = 96`. The near-origin baseline subtracts its near
   projection so that the near camera-height contribution cancels. The
   depth-varying contribution thereafter is
   `L*256*96*(1/40 - 1/(q+24))`, increasing from zero at near to 185.124 at
   far. This is the first source operation producing the large vertical
   focal drift. It is not a screenshot measurement.
4. **Signed placement and calibration:** with orientation -1, signed Y
   -352 adds `L*352 = 125` logical pixels at both ends. Neutral Y gain and
   tube scale are 1, offset 0. There is no Level 13-specific final placement
   branch. Thus Atari origins are `(400,425)` near and `(400,610.124)` far.
   At non-neutral settings, Y gain multiplies model Y, tube scale scales
   displacements around the corresponding depth's zero-coordinate anchor,
   and vertical offset translates the finished result.
5. **Viewport:** portrait playfield maps logical `(X,Y)` to
   `(800 + 1.68*(X-400), 1.68*Y - 52.96)` on the 1600x1280 host. The
   960x1280 viewport begins at host X=320; HUD uses a separate 1.6 scale.
   Consequently Atari origin anchors map to approximately `(800,661.04)`
   and `(800,972.05)`. With default portrait viewport, legacy origin anchors
   both map to `(800,451.04)`. The explicitly selected legacy 4:3 camera
   instead maps `(X,Y)` to `(2X, 2Y+40)`, so its origins both map to
   `(800,640)`. The independent legacy projection and viewport selections
   matter: absence of `--atari-presentation` uses legacy projection even
   with the default portrait viewport.

`VectorRenderer.cs:1343` projects the same near/far model vertex for each
rail and both closed rims. `VectorRenderer.cs:310`, `:1234`, `:1428`, and
`:1630` show the shared point projector used for tube and representative
entity/lane anchors. Per-depth local scale axes are also consumed by
tube-oriented artwork. Future correction must preserve both anchor and local
scale registration; altering tube lines alone would not suffice.

## Deterministic A/B comparison

Values below are rounded to 0.001 logical pixels. Each near/far pair is a
rail's endpoints at z=50 and z=450, before viewport mapping, with neutral
calibration. Indices are the unchanged source row order. Relations to each
path's origin follow directly by subtracting `(400,300)` on legacy, and
`(400,425)` near / `(400,610.124)` far on Atari.

| Feature / row index | Model `(x,y)` | Legacy near -> far | Atari near -> far |
|---|---|---|---|
| Top right peak 0 | `(85.714,-182.143)` | `(509.091,68.182)` -> `(413.187,271.978)` | `(509.091,193.182)` -> `(416.529,575.000)` |
| Top notch 1 | `(0,-142.857)` | `(400,118.182)` -> `(400,278.022)` | `(400,243.182)` -> `(400,582.576)` |
| Top left peak 2 | `(-85.714,-182.143)` | `(290.909,68.182)` -> `(386.813,271.978)` | `(290.909,193.182)` -> `(383.471,575.000)` |
| Left upper extreme 4 | `(-185.714,-82.143)` | `(163.636,195.455)` -> `(371.429,287.363)` | `(163.636,320.455)` -> `(364.187,594.284)` |
| Left notch 5 | `(-142.857,0)` | `(218.182,300)` -> `(378.022,300)` | `(218.182,425)` -> `(372.452,610.124)` |
| Left lower extreme 6 | `(-185.714,82.143)` | `(163.636,404.545)` -> `(371.429,312.637)` | `(163.636,529.545)` -> `(364.187,625.964)` |
| Bottom left peak 8 | `(-85.714,192.857)` | `(290.909,545.455)` -> `(386.813,329.670)` | `(290.909,670.455)` -> `(383.471,647.314)` |
| Bottom notch 9 | `(0,142.857)` | `(400,481.818)` -> `(400,321.978)` | `(400,606.818)` -> `(400,637.672)` |
| Bottom right peak 10 | `(85.714,192.857)` | `(509.091,545.455)` -> `(413.187,329.670)` | `(509.091,670.455)` -> `(416.529,647.314)` |
| Right lower extreme 12 | `(185.714,82.143)` | `(636.364,404.545)` -> `(428.571,312.637)` | `(636.364,529.545)` -> `(435.813,625.964)` |
| Right notch 13 | `(150,0)` | `(590.909,300)` -> `(423.077,300)` | `(590.909,425)` -> `(428.926,610.124)` |
| Right upper extreme 14 | `(185.714,-82.143)` | `(636.364,195.455)` -> `(428.571,287.363)` | `(636.364,320.455)` -> `(435.813,594.284)` |
| Model origin / derived reference | `(0,0)` | `(400,300)` -> `(400,300)` | `(400,425)` -> `(400,610.124)` |

Near-rim logical width is 472.727 in either neutral path. Atari's far local
scale / near local scale is `40/264 = 5/33`; legacy's is `55/455 = 11/91`.
For example, the Atari lower-left peak's far Y 647.314 is close to its near
Y 670.455 even though the same legacy endpoints are 329.670 and 545.455.
That mechanically explains the lower-rail compression observed in the
candidate without changing the model.

## Calibration capability

For any finite positive tube scale `S`, Y gain `G`, and final offset `O`,
let `A(z)` be the unscaled model-origin anchor and `p(z)` the neutral point
displacement from it. The controls yield
`A(z) + S*(pX(z), G*pY(z)) + (0,O)`.

- `G` changes vertical model displacement, not the zero-coordinate anchor.
- `S` changes both point-displacement axes, not either depth's anchor.
- `O` translates both anchors equally.

Therefore the absolute far-minus-near origin shift stays 185.124 logical
pixels and the far/near local-size ratio stays 5/33 for all three controls.
At a fixed desired near-rim width, `S` is fixed and neither `G` nor `O`
independently changes that displacement relative to the width. Increasing
`S` can reduce the *normalized* drift but necessarily enlarges the rim; it
does not remove the absolute drift. `G` can alter the rim aspect and apparent
drift relative to its height, so qualitative similarity alone cannot prove
every possible calibration inadequate. The exact legacy zero-drift
relationship and depth ratio cannot be reproduced by these controls.

This establishes an expressiveness limitation, not a measured proof of the
one correct MAME target. Neither use of legacy values nor a camera-height
zeroing is justified as the final correction: the supplied MAME image
visibly has nonzero relative displacement.

## Readiness and bounded future direction

The source establishes a Level-13-specific presentation correction is
technically possible without changing the model or accepted global outputs:
a candidate could alter only the Level 13 normal-Playing depth-dependent
origin mapping, inside the shared tube/entity projection path, while retaining
the existing controls. The different depth-size ratio is a second independent
degree of freedom, so it must not be silently assumed correct. Preserve
model order, topology, lane state, local Jacobians, deterministic evaluation
and HUD. Other displayed levels, including direct 15, and non-Playing paths
must remain byte-for-byte behaviorally unchanged.

However, the evidence does not establish a specific correct depth-origin
mapping, a target depth ratio, or whether the latter needs correction.
Zeroing the identified drift would reproduce the legacy anchor relation, not
the observed MAME relation. Because the task prohibits screenshot-fitting
constants and legacy is explicitly not the target, choosing a concrete
mapping now would be speculative. Consequently the requested concrete
implementation is **still unresolved**, not correction-ready. Do not copy
another level or present a guessed coefficient as ROM-backed.

If a later candidate is authorized, suggested regression coverage should
check unchanged genuine points and 16-lane closure; normal-Playing rim and
far-origin relationship at representative depths; tube/entity anchor and
local-scale registration; unchanged lane/spinner state; repeatable mappings;
composition of all three calibration controls; unchanged outputs for Levels
1-12 and direct Level 15; unchanged Approach, ExitZoom and HUD. These are
suggestions only; no tests were added or run in this investigation.

## Validation and repository protection

- **PASS:** Read-only source trace and bounded calculation of the exact
  projection formulas and representative endpoints. The calculation ran
  from an inline Python script and created no temporary analysis file.
- **PASS:** Inspected exactly the two authorized images; no other visual
  assets or ROM binaries were inspected.
- **PARTIAL:** Visual conclusion is qualitative; no independent ROM extraction,
  MAME-coordinate fitting, or manual gameplay acceptance was performed.
- An optional PNG-dimension check using Python Pillow initially failed because
  Pillow is unavailable; a Python standard-library PNG header check succeeded.
  No dependency was installed. This does not affect the calculations.
- Automated test/build execution was not needed for this read-only analysis;
  the preceding candidate report's test results are historical, not rerun
  here. The interactive game was not launched.
- Starting branch `master` at `534cbf21150dc501300c548f98972a4c64198602`
  tracked `origin/master` at the same commit, with an empty index and the
  existing Level 13 candidate and unrelated untracked files. No project,
  test, configuration, durable documentation, image, reference file, or Git
  state was intentionally modified. No stage, commit, push, stash, reset,
  restore, clean, or rebase was performed.
- The required paired prompt and this report were created under `AI-Logs`.
  The directory contains tracked historical records, but these new files are
  untracked; the explicit prohibition on staging takes precedence over making
  the new records tracked. Existing records were not rewritten.
- Manual Level 13 presentation acceptance remains pending; this investigation
  does not establish arcade-fidelity acceptance.
