# Whole-Game Presentation Perspective / Envelope Read-Only Analysis

**Date:** 2026-11-14
**Status:** COMPLETED — READ-ONLY INVESTIGATION
**Primary classification:** READY FOR ONE SHARED PERSPECTIVE EXPERIMENT
**Completion token:** `TEMPEST_PRESENTATION_PERSPECTIVE_ENVELOPE_READONLY_20261114`

## 1. Scope and starting Git state

The supplied prompt was preserved verbatim at
`AI-Logs/Prompts/2026-11-14-whole-game-presentation-perspective-envelope-readonly-analysis-prompt.md`.
This was a bounded source, prior-report, local-disassembly, and three-image
analysis. No code, tests, tracked documentation, gameplay geometry, or
presentation values were changed. No game, build, or test suite was run.

Starting state:

- Branch: `master`
- `HEAD` and `origin/master`:
  `385f603a94391756b2ca6cccd524dc17b281e782`
- Index: clean
- Exactly four tracked candidate files modified:
  - `src/Tempest.Game/Program.cs`
  - `src/Tempest.Game/Rendering/DisplayConfiguration.cs`
  - `src/Tempest.Game/Rendering/VectorRenderer.cs`
  - `tests/Tempest.Game.Tests/DisplayConfigurationTests.cs`
- Starting diff summary: 4 files changed, 429 insertions, 36 deletions
- Non-ignored untracked files: none

The candidate files were read only and remain unmodified by this investigation.

## 2. Exact materials inspected

### Project source

- `src/Tempest.Game/Rendering/Projection.cs`
- `src/Tempest.Game/Rendering/DisplayConfiguration.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Levels/LevelLoader.cs`
- `src/Tempest.Game/Levels/LevelDefinition.cs`
- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/Levels/Level.cs`

Only the projection, camera, initialization, Level 9 data, tube drawing, and
depth-related ranges needed for this analysis were read.

### Authoritative reports

- `AI-Logs/Reports/2026-11-12-whole-game-presentation-level9-readonly-investigation-report.md`
- `AI-Logs/Reports/2026-11-12-whole-game-presentation-level9-label-correction-report.md`
- `AI-Logs/Reports/2026-11-13-whole-game-presentation-cross-level-clipping-diagnostic-report.md`
- `AI-Logs/Reports/2026-11-13-whole-game-presentation-shared-vertical-placement-correction-report.md`
- `AI-Logs/Reports/2026-11-13-whole-game-presentation-tube-size-readonly-analysis-report.md`

The label-correction report governs earlier image attribution, the
shared-vertical-placement report governs current placement, and the tube-size
report governs the conclusion that uniform enlargement is not the next step.

### Exactly authorized visual files inspected

- `Videos/Mame Frames/level7-frame_0224.png`
- `Videos/Mame Frames/level7-frame_1265.png`
- `Videos/Mame Frames/level7-frame_2070.png`

No other visual file, screenshot, contact sheet, video, recording, or frame was
searched for, enumerated, opened, extracted, or analyzed. No filename was
treated as proof that all three frames show production Level 7. The image
dimensions below describe captures and do not establish arcade hardware
resolution.

### Local Atari source

Only bounded portions of `Reference/arcade/Tempest Rom Source code.md` were
read: the RAM labels, level-array declarations and comments, level setup
routine, and the `y3d` use. No raw ROM file or directory was inspected.
Incitti, Gemini material, external sources, and the web were not consulted.

## 3. Frozen presentation values and supplied arcade observations

The following values were treated as frozen and were not tuned:

| Value | Current value |
|---|---:|
| Host window | 1280 × 960 |
| Legacy logical projection space | 800 × 600 |
| Portrait logical presentation | 600 × 800 |
| Centered portrait viewport | 720 × 960; left = 280 |
| Output scale | 1.2 |
| Shared playfield scale | 1.05 |
| Shared vertical translation | −33.1 |
| Combined playfield zoom | 1.26 |
| Horizontal center / placement | projected x = 400; centered viewport |
| Projection `K` / `ZOffset` | 70 / 5 |
| HUD | separate portrait/screen-space camera |
| A/B reference | existing `--legacy-presentation` behavior |

`Program.cs` sets `Projection.K = 70` and `Projection.ZOffset = 5` during
normal initialization. `Projection.K` has a different class initializer, but
the production startup assignment is authoritative for the current run.

**ARCADE-OBSERVED (supplied by Neal):** gameplay retains an upper
score/status/lives region above the tube across levels; tube placement and
margins are consistent across all 16 layouts; the tubes do not visibly jump
between fundamentally different presentation scales; and the desired arcade
view has a pronounced looking-down-the-tube perspective unlike the current
C# presentation. Preserve the upper text region, but do not implement or
coordinate final HUD/leaderboard content here. Neal considers the current
shared vertical placement correct and froze it. Tube-size improvement remains
desired but is explicitly deferred.

## 4. Authorized frame measurements

The captures are 1920 × 1080 pixels. Bright-blue playfield bounds were
measured from pixels with `y >= 245`, `B >= 70`, `B > 1.25R`, and `B > 1.18G`;
this excludes the upper blue level digit. The colored HUD bounds are
approximate pixel-mask bounds for the green score/status, yellow life icons,
and any blue status digit. They are repeatable image measurements, not
hardware-space measurements. Tube bounds include the visible bright-blue
geometry and can include a small amount of other blue gameplay detail.

| Frame | Image | Blue playfield bounds `[x0..x1, y0..y1]` | Approx. size | Center x | Side margins L/R | Colored HUD bounds | HUD-to-tube gap | Approx. far opening/pit |
|---|---:|---|---:|---:|---:|---|---:|---|
| `level7-frame_0224.png` | 1920 × 1080 | `[718..1239, 289..867]` | 522 × 579 | 978.5 | 718 / 680 px | `[625..1060, 172..244]` | 45 px | around `(975,675)` |
| `level7-frame_1265.png` | 1920 × 1080 | `[698..1255, 266..875]` | 558 × 610 | 976.5 | 698 / 664 px | `[627..1062, 170..244]` | 22 px | around `(976,530)` |
| `level7-frame_2070.png` | 1920 × 1080 | `[604..1361, 260..869]` | 758 × 610 | 982.5 | 604 / 558 px | `[672..1060, 170..223]` | 37 px | around `(975,270)` |

Sizes use inclusive pixel counts; margins count pixels outside the measured
bounds. “HUD-to-tube gap” is the difference between the tube's topmost blue
pixel and the lowest colored HUD pixel. The opening/pit locations are visual
estimates, not threshold-derived point correspondences. The measured
horizontal tube centers are all within about 23 pixels (1.2% of image width)
of the 1920-pixel image center. The blue tube envelope begins at about 24–27%
of image height and ends around 80–81%; the visible colored text band occupies
roughly 16–23%.

**Interpretation:** all three images show a broad near/outer tube extent
receding along visible rails toward a much smaller central/far region. The
principal tube geometry is centered consistently in the capture even though
its left/right bounds vary substantially with the visible tube shape. The
estimated vertical location of the far opening differs substantially between
frames; this is not evidence that the files correspond to a particular level
or matched camera phase, nor does it justify three calibrations.

## 5. Shared presentation envelope and HUD reservation

The three captures and Neal's cross-level observation support a conceptual
whole-game composition of:

1. a stable upper score/status/lives region;
2. a distinct black separation gap;
3. the tube/playfield region;
4. a visible lower margin.

In these captures, colored text is concentrated around 16–23% of image height,
the principal tube bounds start around 24–27%, and the tube ends around
80–81%, leaving about 19–20% below. The measured text-to-tube separation is
about 2–4% of image height. These are capture-relative proportions, not final
portrait coordinates or arcade hardware dimensions.

**Recommendation:** retain a normalized upper gameplay-text band conceptually
above the tube, followed by a deliberate gap. Keep that band independent of
the playfield camera, and make the all-level tube envelope respect it. Do not
derive final HUD glyph coordinates or make a particular screenshot's pixel
height a project constant. The separate HUD camera already gives the
architecture a suitable place to keep future text stable while the tube
projection is investigated.

## 6. Current C# projection and presentation path

For model point `(x,y)` at depth `z`, the source-level projective mapping is:

```text
denominator = z + ZOffset
xProjected = 400 + x * K / denominator
yProjected = CenterY + (y + YOffset) * K / denominator
```

Production Level 9 draws tube vertices at near depth `Level.Position = 50`
and far depth `Level.Position + Level.Depth = 450`, where `Depth = 400`.
`DrawTube` projects both rims and connects matching vertices. Tube and
playfield entities are drawn under the shared playfield `BeginMode2D` camera;
portrait HUD/screen-space content is drawn separately. The presentation
camera's host-space mapping for the frozen portrait values is:

```text
xHost = 640 + 1.26 * (xProjected - 400)
yHost = 1.26 * yProjected - 39.72
```

The camera is a post-projection, positive uniform affine transform. It does
not alter depth or near/far perspective ratios. Projection is used by tube
geometry and playfield entities through `Projection.Project` and the
approach-geometry helper; the common camera keeps those rendered elements
registered. Level point data remains gameplay geometry and is not a
presentation-only canvas.

### What the current inputs control

- **`K`:** multiplies projected offsets from `(400, CenterY)` at every depth.
  It changes overall projected size and can change bounds/placement around
  that center, but cancels from the near/far size ratio. `K` also participates
  in existing exit/reverse-zoom presentation.
- **`ZOffset`:** changes the denominator at every depth. Its relative effect
  is larger at the near rim than the far rim, so it changes the near/far
  scale ratio and apparent depth, not just overall size. It also participates
  in existing transitions.
- **`CenterY`:** an additive projected vertical center. A change shifts both
  rims equally in projected pixels and does not change size or perspective.
  Level 9 uses `CenterY = 300`.
- **`YOffset`:** is added to model Y before division by depth. It changes
  vertical placement by different amounts at near and far depths and can
  alter apparent vertical convergence. It is a per-level projection input;
  Level 9 uses `YOffset = 0`. Changing it per level would not be a shared
  camera solution.
- **Depth:** sets the denominator at the near/far rims and throughout entity
  travel. Changing the near/far relationship alters perspective and affects
  gameplay-depth registration; it is not a post-projection size control.
- **Portrait playfield scale:** scales all projected playfield content
  uniformly around its current camera anchor. It changes overall
  prominence/clearance but leaves the projection and near/far ratio intact.
- **Horizontal placement:** projected center `x=400` and the shared camera
  place the playfield in the centered portrait viewport. A uniform transform
  does not add a different horizontal vanishing point.
- **Convergence / camera elevation:** increasing depth shrinks projected
  model offsets toward `(400, CenterY)`. The current equation has no separate
  depth-dependent camera-height or view-elevation term that moves that
  convergence center vertically with depth. A level's asymmetric model
  points can make its near/far bounding-box centers differ without supplying
  a shared camera-elevation rule.

## 7. Level 9 current near/far calculations

The Level 9 source points span approximately `x = −200..200` and
`y = −28.5714..200`; its definition has `CenterY = 300`, `YOffset = 0`.
Applying `K=70`, `ZOffset=5`, and the current portrait camera gives:

| Reference | Projected bounds before portrait camera | Projected size | Current host bounds | Host size |
|---|---|---:|---|---:|
| Near rim, `z=50` | `x=[145.45,654.55]`, `y=[263.64,554.55]` | 509.09 × 290.91 | `x=[319.27,960.73]`, `y=[292.46,659.01]` | 641.45 × 366.55 |
| Far rim, `z=450` | `x=[369.23,430.77]`, `y=[295.60,330.77]` | 61.54 × 35.16 | `x=[601.23,678.77]`, `y=[332.74,377.05]` | 77.54 × 44.31 |

- Near/far width ratio: `455/55 = 8.2727`.
- Near/far height ratio: also `8.2727`, since the same model-Y range is
  scaled by the same two depth factors.
- Projected near/far bounding-box vertical-center difference:
  approximately 95.90 projected units, or 120.84 host pixels.
- Both rim bounding boxes remain horizontally centered at host `x=640`.
- The asymmetry of Level 9's model-Y bounds contributes to its different
  near/far bounding-box centers. The actual projection's common vanishing
  center `(400,300)` maps to host `(640,338.28)`.

The large mathematical near/far size ratio confirms that the current
projection is perspective, not orthographic. It does not by itself establish
an arcade match: this ratio is a source-derived C# relationship and must not
be treated as a measured arcade ratio.

## 8. Bounded sensitivity analysis

These are analytical probes around the current Level 9 values, not proposed
settings. Widths below are host-space widths; no value was applied to source.

### `K` (varying alone)

| `K` | Near-rim width | Far-rim width | Near/far ratio |
|---:|---:|---:|---:|
| 63 (−10%) | 577.31 px | 69.78 px | 8.2727 |
| 70 | 641.45 px | 77.54 px | 8.2727 |
| 77 (+10%) | 705.60 px | 85.29 px | 8.2727 |

`K` is a global magnitude control, not a way to increase depth ratio. A
`K`-only change would also change extents and could create clipping. It is
frozen, and its transition use makes it a poor first isolated perspective
experiment.

### `ZOffset` (varying alone)

| `ZOffset` | Near-rim width | Far-rim width | Near/far ratio |
|---:|---:|---:|---:|
| 4 | 653.33 px | 77.71 px | 8.4074 |
| 5 | 641.45 px | 77.54 px | 8.2727 |
| 6 | 630.00 px | 77.37 px | 8.1429 |

Increasing `ZOffset` weakens the relative near/far scale difference; reducing
it strengthens that relationship. The absolute far width changes little
compared with the near width. This is a true perspective control but is
explicitly frozen and is animated by existing transition behavior.

### `CenterY` and `YOffset`

For Level 9, changing `CenterY` by ±10 projected units shifts near and far
rim points by ±10 units alike (±12.6 host pixels) and does not change the
near/far ratio. Changing `YOffset` by +10 instead shifts the near rim by
about +16.04 host pixels and the far rim by about +1.94 host pixels, changing
their relative vertical placement by about 14.10 pixels; the signs reverse
for −10. Level 9's current `YOffset` is zero. Because `CenterY` and
`YOffset` are existing per-level inputs, changing either as a Level 9
correction would not establish a shared all-level camera rule.

### Near/far depth relationship

With the near plane held at `z=50`, an analytical ±10 change in the 400-unit
depth span gives:

| Depth span | Far `z` | Near/far ratio |
|---:|---:|---:|
| 390 | 440 | 8.0909 |
| 400 | 450 | 8.2727 |
| 410 | 460 | 8.4545 |

Increasing the far depth strengthens the relative near/far difference. This
is only a sensitivity calculation: changing gameplay depth is not proposed
and would affect movement/registration as well as rendering.

### Post-projection playfield scale

The current `PortraitPlayfieldScale = 1.05` and output scale `1.2` combine to
1.26. A 1% relative change to playfield scale would change Level 9's near
width by about 6.41 host pixels and far width by about 0.78 pixels, while
leaving the 8.2727 ratio unchanged. It would also move bounds relative to
the camera's fixed placement anchor. Uniform enlargement is therefore a size
change, not a perspective correction, and remains deferred under the prior
all-level/headroom analysis and Neal's placement freeze.

## 9. Arcade perspective and local ROM-source evidence

### Visual comparison and supported difference

**ARCADE-OBSERVED (authorized captures):** the three geometries show long
rails and a broad outer tube envelope receding toward a small far/pit region.
That gives a strong visual impression of viewing down the tube. Their apparent
far-opening locations vary, so these captures do not support identifying a
single screenshot as Level 9 or fitting separate per-image values.

The authoritative 2026-11-12 label-correction report also records an earlier
comparison in which the C# upper outer shoulders were above corresponding
far-rim shoulders while the MAME shoulders were below them. That report
explicitly left exact correspondence and phase unresolved. It is useful
qualitative evidence against translation or uniform scaling alone, but is
not re-measured here and does not determine a numeric projection.

**PROJECT TRANSLATION:** C# already uses a depth denominator and has an
8.27:1 Level 9 near/far scale ratio. The supported difference is therefore
not simply “C# lacks perspective” or “the entire tube needs uniform
enlargement.” The current projection fixes its vertical perspective center
at `CenterY` for every depth; it has no explicit camera-height input in that
equation. The likely model-level gap is a ROM-informed view-elevation /
camera-height contribution coupled to depth, or an equivalent shared
projective transform, rather than a post-projection scale. This is a
testable explanation, not a proven one-to-one mapping from a ROM field to
the current discrepancy.

**HYPOTHESIS / boundary:** the captures do not provide matched point
correspondences, a known C#-to-arcade scale, or a uniquely identifiable
projection parameter. The exact camera-elevation sign, magnitude, mapping to
ROM data, and transition behavior remain uncalibrated. A stronger perspective
can be investigated, but no numeric candidate is justified here.

### ROM-BACKED concepts in the local disassembly

The local source explicitly documents 16-entry level arrays for:

- `lev_scale`: reciprocal level scale;
- `lev_y3d`: 3-D Y offset, explicitly commented as “camera height”;
- `lev_y2d` / `lev_y2db`: signed 2-D Y offset;
- `lev_open`: open/closed state.

The level setup routine loads the per-level scale, `y3d`, open flag, and
signed 2-D Y components. A bounded routine uses the loaded `y3d` in a
camera-height-related calculation. This is **ROM-BACKED evidence that
distinct per-level scale, 3-D camera-height, 2-D placement, and topology
inputs exist**. It supports a shared projection semantics that consumes
level data; it does not support arbitrary per-level screen fitting.

The examined disassembly does not establish a modern C# `K/(z+ZOffset)`
equation, a projection matrix, or a direct numeric conversion from those
fields to current C# constants. The C# `Level9RomEvidence` records Level 9
bytes (`Scale=0x0C`, `Y3d=0xB0`, `Y2d=0x0140`, `Open=0xFF`), but those are not
equal by name to `K`, `ZOffset`, `CenterY`, or `YOffset`. No missing ROM
values were invented, and no claim is made that `K=70` or `ZOffset=5` is
ROM-defined.

## 10. Size, prominence, clipping, and shared-rule conclusions

- **Can looking-down perspective improve without enlarging the playfield?**
  Yes in principle. A depth-coupled camera-elevation term can change the
  near/far vertical relationship while keeping uniform playfield scale 1.05.
- **Can prominence change with scale frozen?** Yes. A change in projected
  depth shape, far-opening position, or rail convergence can alter perceived
  prominence even when the uniform scale and outer near width are unchanged.
- **Can perspective create clipping risk?** Yes, depending on the projection
  term and its anchor. A term anchored to leave the near rim fixed can protect
  that rim directly, but still moves far-rim/rail geometry; an unanchored
  term can move the near rim too. All projected bounds must be recalculated
  for Levels 1–9, and the reserved upper text region must be respected.
- **Should size remain deferred?** Yes. Freeze the accepted shared vertical
  placement and 1.05 scale through one perspective experiment and manual
  evaluation. Revisit size only as a separate authorized milestone.
- **Can one shared rule serve the layouts?** The three frames' centered,
  differently shaped tubes and Neal's cross-level observation are consistent
  with one coherent projection/presentation rule. Use one equation and one
  playfield presentation path; source-backed per-level metadata may be inputs
  when its mapping is established. Do not add screenshot- or level-specific
  calibration constants.
- **Future Levels 10–16:** once their geometries exist, evaluate their near,
  far, rail, horizontal-margin, lower-margin, and top-text clearance bounds
  with the same projection. Check tube/entity registration and preserve model
  geometry. Do not infer their bounds from the existing 1–9 levels.

The shared playfield camera remains the correct registration boundary. A
future depth/elevation adjustment should be applied consistently at the
shared projection entry points, including tube and game entities, while
keeping the HUD on its separate camera. Preserve current gameplay geometry,
determinism, and legacy presentation. No command-line parser change or new
option name is proposed here.

## 11. Diagnostic classification and smallest next experiment

**READY FOR ONE SHARED PERSPECTIVE EXPERIMENT.**

The bounded evidence supports one reversible experiment: evaluate a single
shared, depth-coupled camera-elevation/vertical-convergence term in the common
projection path, leaving `K`, `ZOffset`, the portrait viewport, scale 1.05,
horizontal placement, vertical translation −33.1, HUD camera, and model
geometry unchanged. Anchor the experiment explicitly to the accepted
near-rim placement if feasible; do not choose a numeric coefficient from
these screenshots. Keep `--legacy-presentation` as the unambiguous A/B
reference. This is an experimental model to assess, not accepted behavior or
an implementation instruction for this task.

The three authorized frames are sufficient to motivate this qualitative
shared experiment; additional arcade screenshots are not required before
one reversible candidate. Additional matched references would be useful for
later numeric calibration and exact point correspondence. Neal's manual
acceptance remains necessary after any future implementation.

## 12. Validation, final state, and remaining acceptance

No build, test suite, or interactive game was launched; this was read-only
analysis. Pixel measurement was limited to the three authorized PNG paths.

Final state:

- Branch remains `master`.
- `HEAD` and `origin/master` remain
  `385f603a94391756b2ca6cccd524dc17b281e782`.
- Index remains clean.
- The same four tracked candidate files remain modified; no candidate content
  was changed by this investigation.
- No other non-ignored untracked files were introduced.
- Only the authorized ignored prompt and report were created.
- Nothing was staged, committed, or pushed.
- Remaining work is a separately authorized shared perspective experiment
  and Neal's manual visual acceptance; no size change or final HUD work is
  included.
