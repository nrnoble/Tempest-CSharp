# Level 7 presented-rail-distance motion analysis

Date: 2026-11-17
Status: **COMPLETE — READ-ONLY PRESENTATION-GEOMETRY INVESTIGATION**
Classification: **A for the measured Atari-ON C# presentation mapping** — Tanker
anchor progress is constant as a fraction of its same-lane presented path;
motion relative to the growing glyph scale decreases. This is a statement
about the current project presentation candidate, not an arcade-verified
motion law.

## Result

For a fixed Atari-ON Level 7 lane, the entity anchor follows a straight segment
between that lane's mapped deep and rim center points. The Level 7 mapping
linearly interpolates those points in normalized depth, and the final
portrait-playfield camera is affine and uniformly scaled. Tanker 15 therefore
advances **0.004375 of its total presented lane path per simulation
tick/playback boundary at every sampled depth**. Its absolute submitted
anchor motion remains about **1.1745 host pixels per boundary** on lane 1.

The fraction of the *remaining* path traversed per boundary is not constant:
it rises from 0.00516 at depth 0.8481 to 0.02280 at depth 0.1919 because the
remaining path is shrinking. This is not a reduction in absolute or
total-path-normalized progression. At the same time, local lane scale and
glyph width grow toward the rim, so anchor motion per glyph width falls from
0.05618 to 0.02047 across the sampled windows.

Atari OFF differs: its same-lane path is also straight, but its legacy
perspective projection parameterizes that line nonlinearly in gameplay depth.
Its fractional progression per boundary increases across the equivalent
samples, from 0.00070 to 0.00631.

No correction is justified by the measured Atari-ON rail progression. The
reported subjective easing of interception is consistent with the growing
target and decreasing motion relative to target size, but these geometry
measurements do **not** establish additional real simulation time or
opportunity to shoot. That temporal consequence remains unresolved.

## Scope, evidence, and preservation

Read the current project instructions, the named framebuffer and deterministic
A/B reports, the named good/bad runtime report, current Level 7 rendering and
projection source, and the existing diagnostic logs. The full runtime
diagnostic was processed incrementally; it was not dumped into output.
Mathematical results below were checked against the diagnostic anchors and
local scales.

The prior deterministic A/B report establishes identical simulation state at
all 3,511 boundaries of `Level-7-slowmo.json`. The four synchronized OFF/ON
framebuffer windows provide Tanker 15 samples at boundaries 100, 150, 200,
and 250. The full `level7-control-20261117.json` runtime trace is the recorded
playback control with Atari-ON mapped Level 7 geometry; “control” here means
recorded-playback control, not Atari OFF. The distinct manual-runtime trace
was not used as a controlled presentation comparison.

**Evidence classification:** the record-06 bytes in the local catalog are
ROM-backed data. The viewport gains, offsets, DVG quotient assumptions,
screen centering, lane correspondence, and mapped Level 7 shape are marked in
source as project translations or provisional assumptions. This report
establishes how the *current candidate implementation* maps that data; it
does not establish that the candidate geometry is arcade-faithful.

No screenshots, videos, frames, or other visual assets were opened. No game
was launched and no capture was made. No production, test, configuration,
diagnostic, recording, or durable-documentation file was changed. No Git
state was changed.

## 1. Authoritative Atari-ON same-lane path

The source path is:

1. [AtariTubeRecordCatalog.cs](../../src/Tempest.Game/Levels/AtariTubeRecordCatalog.cs)
   supplies record 06, including the 16 raw X/Y points and Level 7 metadata.
2. [AtariTubeVectorListBuilder.cs](../../src/Tempest.Game/Rendering/AtariTubeVectorListBuilder.cs)
   projects the record at the prototype's near runtime position 16 and far
   position 240, then pairs corresponding points as rails.
3. [AtariLevel7Presentation.cs](../../src/Tempest.Game/Rendering/AtariLevel7Presentation.cs)
   maps each endpoint with its near/far viewport map and applies one common
   centering translation. For a lane `i`, the entity-center endpoints are
   the midpoints of points `i` and `(i+1) mod 16` in `NearPoints` and
   `FarPoints`.
4. [AtariLevel7EntityPresentation.cs](../../src/Tempest.Game/Rendering/AtariLevel7EntityPresentation.cs)
   linearly interpolates both lane vertices at normalized depth, then returns
   the lane segment's center.
5. [VectorRenderer.cs](../../src/Tempest.Game/Rendering/VectorRenderer.cs)
   uses that center for Tankers and other lane-centered entities. The
   diagnostic `mappedAnchor` is the renderer anchor; there is no later
   entity-specific anchor warp.

For this report, normalized depth is `d=0` at the rim and `d=1` at the deep
endpoint. Define `R_i` as the mapped midpoint of near/rim lane edge `i` and
`F_i` as the mapped midpoint of its far/deep edge. The actual Tanker center
path is

```
C_i(d) = (1-d) R_i + d F_i
L_i    = |F_i - R_i|
```

The entity lies on this path: at every depth its anchor is the midpoint of
the two same-depth lane vertices. This is the lane-center path between the
two rail boundaries, not a chord substituted for a curved path. Both boundary
rails and this center path are straight under the current interpolation.

For Tanker 15's lane 1, the mapped center endpoints are approximately:

| Endpoint | Logical presentation coordinates | Host coordinates after camera |
|---|---:|---:|
| Rim / near, `d=0` | (563.373, 341.398) | (845.850, 390.441) |
| Deep / far, `d=1` | (425.404, 503.740) | (672.009, 594.993) |

Lane 1's total presented center-path length is **213.05 logical units** or
**268.444 host pixels**. Every lane has a different length because the
record-06 point geometry and the horizontal aspect mapping differ by
direction. Host-pixel lengths (same-lane center path, rim to deep) are:

| Lane | Atari ON (px) | Atari OFF (px) | Lane | Atari ON (px) | Atari OFF (px) |
|---:|---:|---:|---:|---:|---:|
| 0 | 316.370 | 271.889 | 8 | 212.046 | 286.414 |
| 1 | 268.444 | 189.668 | 9 | 196.526 | 194.224 |
| 2 | 160.716 | 194.224 | 10 | 136.492 | 189.668 |
| 3 | 134.643 | 286.414 | 11 | 162.268 | 271.889 |
| 4 | 194.846 | 291.060 | 12 | 269.666 | 266.910 |
| 5 | 210.977 | 205.365 | 13 | 317.084 | 177.544 |
| 6 | 126.481 | 205.365 | 14 | 241.920 | 177.544 |
| 7 | 126.962 | 291.060 | 15 | 241.668 | 266.910 |

Distances use the mode's own path and final camera, not cross-mode screen
shape equality.

## 2–3. Path shape and final transforms

**Atari ON:** the DVG endpoint maps use a shared linear scale/aspect, rotation,
sign/axis mapping, and translation; the far map adds a fixed translation.
Level-wide bounds centering adds another fixed translation. These are affine
maps. The subsequent `ExperimentalPortraitPlayfieldCamera` has zero rotation
and uniform zoom 1.26; the host mapping used by the diagnostic is also affine.
No later nonuniform camera or perspective transform acts on the mapped
Level-7 lane. These transforms preserve the straight center path and its
linear parameter `d`. The horizontal aspect map changes lengths by lane, but
not the fraction of a given lane traversed for a given `delta d`.

**Atari OFF:** a fixed lane's world midpoint is projected by
`Projection.Project`. With the synchronized run's `K=70`, `ZOffset=5`, and
default portrait perspective 0, both projected coordinates are affine in
`q=K/(z+ZOffset)`. Its fixed-lane path is straight, but gameplay depth maps
nonlinearly to position along it. The final portrait camera is still affine
and uniform.

The nearest/farthest Atari-ON endpoints are project-mapped record-06 points,
not the Level 7 C# gameplay polygon. Atari-ON screen centering and the final
camera affect coordinates and scale but do not bend or nonuniformly
reparameterize distance along the lane.

## 4–5. Depth and local scale

In normal Level 7 play the configuration has `Position=50`, `Depth=400`, and
an entity's projected depth is `Position + Depth - height`. Therefore:

```
d = (projectedDepth - Position) / Depth
  = 1 - height / 400
```

For Tanker 15, height advances by 1.75 per simulation tick, so `d` decreases
by exactly `1.75/400 = 0.004375` per tick.

Atari-ON local scale is the current mapped lane-edge length divided by that
lane's near/rim edge length:

```
S_i(d) = |a_i + d (b_i - a_i)| / |a_i|
```

where `a_i` and `b_i` are the lane-edge vectors at rim and deep endpoints.
This is the norm of an affine vector, generally a slightly nonlinear
function. On Tanker 15 lane 1 it is nearly affine: `S(0)=1`,
`S(1)=0.15177694`; the maximum deviation from the line joining those values
over `[0,1]` is below `4e-7`, and `dS/dd` is approximately `-0.84822`.
Thus local scale grows toward the rim, while the anchor's depth parameter
remains affine. The scale formula and the anchor formula use the same `d`,
but they are different geometric quantities.

## 6. Tanker 15 path measurements

For Atari ON, with `L=268.44376` host pixels and depth `d`:

- distance already traveled from deep is `(1-d)L`;
- distance remaining to rim is `dL`;
- movement per boundary is approximately `0.004375L = 1.17444` pixels;
- movement / total lane path is `0.004375`;
- movement / remaining-to-rim distance is `0.004375/d`;
- remaining-distance change per boundary is approximately `-1.17444` pixels.

Each motion entry averages the two adjacent submitted-anchor steps around the
listed center boundary. Scale-relative movement uses both the previous
report's submitted glyph width and source `localPresentationScale`; these are
not rail-distance denominators. No near-zero remaining-distance division is
used: the smallest Tanker 15 remaining-distance denominator in this table is
51.508 host pixels.

| Boundary | Height | `d` | Absolute motion (px/boundary) | Total path (px) | Traveled from deep (px) | Remaining to rim (px) | Motion / total path | Motion / remaining | `delta` remaining (px/boundary) | Local scale | Glyph width (px) | Motion / glyph width |
|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|
| 100 | 60.75 | 0.848125 | 1.17447 | 268.444 | 40.770 | 227.674 | 0.004375 | 0.005159 | -1.17447 | 0.28060 | 20.905 | 0.05618 |
| 150 | 148.25 | 0.629375 | 1.17446 | 268.444 | 99.492 | 168.952 | 0.004375 | 0.006951 | -1.17446 | 0.46615 | 33.062 | 0.03552 |
| 200 | 235.75 | 0.410625 | 1.17445 | 268.444 | 158.214 | 110.230 | 0.004375 | 0.010655 | -1.17445 | 0.65170 | 45.219 | 0.02597 |
| 250 | 323.25 | 0.191875 | 1.17442 | 268.444 | 216.936 | 51.508 | 0.004375 | 0.022801 | -1.17442 | 0.83725 | 57.376 | 0.02047 |

The full ON runtime trace independently contains 16 full-depth fixed-lane
Tanker climbs across 11 lanes (out of 24 Tanker trajectories in 14 lanes).
Every qualifying climb has the same `0.004375` normalized-depth change per
tick in deep (`d>=0.8`), middle (`0.2<d<0.8`), and near (`d<=0.2`) bands.
Across the 4,099 continuous fixed-lane Tanker intervals in the trace, the
median total-path fraction per tick is 0.004375 in each depth third; the
median absolute motion is about 0.928 host px/tick in each third. Differences
in absolute pixels across lanes follow their different path lengths, not
depth-dependent fractional progression.

The source-derived mapped anchor and local scale were checked against the
full diagnostic: maximum host-anchor residual was 0.000163 px, logical-anchor
residual was 0.000138 units, and local-scale residual was below 0.00000072.
This verifies the equations against the trace, not against arcade hardware.

## 7. Additional Tankers and lanes

The 16 qualifying full-depth trajectories span lanes 0, 1, 2, 3, 4, 6, 8,
10, 12, 13, and 14. For each, the diagnostic depth changes by 0.004375 per
tick in all three bands; since each trajectory's mapped center path is a
straight affine interpolation, this is also exactly 0.004375 of its own
lane's total path per tick. The full trace's non-qualifying partial tracks
show the same relationship over their observed fixed-lane intervals. The
result is representative across lanes and entities, not specific to Tanker
15. Lane-specific absolute pixel distances are accounted for using each
lane's own `L_i`.

## 8. Atari OFF comparison

For OFF, let `d=1-height/400` as above, and let
`q(d)=70/(55+400d)`. From deep (`d=1`) toward rim (`d=0`), the fraction of
the OFF path already traveled is:

```
F(d) = (q(d) - q(1)) / (q(0) - q(1))
```

For Tanker 15 lane 1, `L=189.66787` host pixels. The source-derived OFF
projected anchors match the four synchronized OFF samples within 0.00004 host
pixels.

| Boundary | `d` | Absolute motion (px/boundary) | Total path (px) | Traveled from deep (px) | Remaining to rim (px) | Motion / total path | Motion / remaining | `delta` remaining (px/boundary) | Legacy local scale | Glyph width (px) |
|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|
| 100 | 0.848125 | 0.13362 | 189.668 | 4.019 | 185.649 | 0.000705 | 0.000720 | -0.13362 | 0.17755 | 14.153 |
| 150 | 0.629375 | 0.22066 | 189.668 | 12.604 | 177.064 | 0.001163 | 0.001246 | -0.22066 | 0.22820 | 17.472 |
| 200 | 0.410625 | 0.43199 | 189.668 | 28.042 | 161.626 | 0.002278 | 0.002673 | -0.43199 | 0.31927 | 23.439 |
| 250 | 0.191875 | 1.19650 | 189.668 | 63.986 | 125.682 | 0.006308 | 0.009520 | -1.19650 | 0.53131 | 37.331 |

OFF produces **increasing absolute and total-path-normalized progression
toward the rim**, approximately nine times the early sampled fraction by
boundary 250. The OFF progression is not constant-fractional and is not
assumed to be an arcade ideal. As in ON, movement divided by remaining
distance is a separate quantity; here it rises both because the point moves
faster along the path and because remaining distance shrinks.

## 9. Player shots

The full Atari-ON recorded-playback trace contains **83** qualifying,
fixed-lane shot trajectories from near the rim (`height>=350`) to the deep
end (`height<=20`), across **8** lanes; each has 64–66 observations.
Across these trajectories there are 5,386 continuous intervals. In the
rim-to-deep orientation, each shot advances `d` and the fraction of its
same-lane total path by 0.015 per simulation tick. The median scale falls
from 0.8472 in the near third to 0.5734 in the middle and 0.2991 in the deep
third. Thus shot motion relative to its local scale increases toward the deep
end, matching the reported inverse scale-relative tendency. Shot movement /
remaining-to-deep distance is `0.015/(1-d)`; it is not the 0.015
total-path fraction.

Shot evidence is sufficient to establish this current presentation mapping
trend. It does not establish arcade shot tuning or visual acceptance.

## 10–12. Interpretation, mechanism, and limits

**Best-supported classification: A.** For the current Atari-ON Level 7
candidate, entities advance constantly along their own presented lane when
measured as a fraction of the total center path. The earlier decline relative
to glyph width/local scale is also real in submitted geometry. The path
analysis does not support a depth-dependent abnormality in Atari-ON
total-path progression, so it does not support B or C.

The exact implementation mechanism is affine anchor interpolation:

```
d = 1 - height/400
C_i(d) = (1-d) R_i + d F_i
delta(path fraction per Tanker tick) = 1.75/400 = 0.004375
```

The local lane scale is the length of an interpolated lane-edge vector divided
by its near-edge length. It increases toward the rim, while the anchor
displacement per tick remains constant in absolute pixels for a fixed lane.
Therefore anchor motion relative to displayed glyph width decreases. The
screen-space effect is already in submitted geometry; the previous framebuffer
report established that rasterization reproduced it.

This explains a **perceptual** reason interception may feel easier near the
rim: the target grows while its anchor moves less per boundary relative to
that target's displayed width. The measured height advance remains 1.75 per
tick and the remaining same-lane path decreases linearly. These facts do not
show more elapsed or simulation time for moving and shooting. Neal's
observation of progressively more *real* opportunity remains unresolved by
this geometric analysis and is not dismissed.

Limits:

- The ON geometry is a provisional/project-translated candidate; its
  correspondence to arcade path geometry is not established by this task.
- Atari OFF's full continuous same-recording trace was not available in the
  paired OFF/ON framebuffer logs; its four sampled windows and exact
  implementation-derived curve suffice to compare the requested points.
- This analysis establishes submitted anchor paths, not collision geometry,
  actual shot-hit probability, monitor presentation, or perceived timing.

## 13–14. Correction boundary and smallest next step

There is **not enough evidence for a correction candidate** to the
Atari-ON lane interpolation or entity speed: same-lane total-path progress is
constant and cross-lane normalization removes rail-length differences.
Changing mapped anchors, local scale, or simulation speed to address the
previous glyph-width ratio would conflate scale-relative appearance with
rail-distance progression and is not supported here.

The smallest justified next step, only if resolving the remaining manual
observation is desired, is a read-only analysis of remaining simulation ticks
to the rim alongside actual Blaster/shot/collision opportunities in the same
recording. That would test whether there is a literal gameplay-time advantage
or only the measured visual-scale effect. No additional capture or replay is
needed for the conclusion in this report.

## Validation and final state

- **PASS** — streamed both existing synchronized OFF/ON logs and the full
  recorded-playback runtime trace under explicit record and time bounds.
- **PASS** — record-06 formulas reproduced Atari-ON diagnostic anchors and
  scales within the residuals stated above; the OFF projection reproduced
  Tanker 15 sampled anchors within 0.00004 host pixels.
- **PASS** — original diagnostic files (including existing visual captures)
  were SHA-256 hashed before and after the analysis/report write; all
  diagnostic hashes matched.
- **PASS** — production/test/configuration source, diagnostics, recordings,
  visual captures, and Git state remain unchanged. No game launch, new
  capture, stage, commit, push, reset, restore, stash, rebase, or clean was
  performed.
- **PASS** — the only files created are the ignored task prompt and this
  required report under `AI-Logs/`.

**Manual acceptance:** not performed or claimed. The report is read-only and
ends without proposing an implementation change.
