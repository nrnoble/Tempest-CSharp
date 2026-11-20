# Atari versus C# Projection Comparison

## Executive conclusion

**Outcome D — MIXED OR UNRESOLVED.**

Atari and C# share a broad reciprocal-depth scaling shape, but exact projection
equivalence is not established. Atari's normal `$c098` path uses
`floor(256 * axis_difference / ($57 + lev_scale))`, then restores the axis
sign, applies a signed Y offset, and saturates to signed 16-bit. C# uses a
floating-point `K / (z + ZOffset)` multiplier on its own centered geometry.
Its `--portrait-perspective` option adds a later depth-only Y displacement; it
does not supply Atari's per-tube divisor, Y reference, or signed Y offset.

The current renderer does not consume Atari presentation metadata. The
worktree does contain an inert per-level metadata catalog and ROM-derived
Level 8/9 point conversions, but a full point/reference/unit mapping for the
selected level geometries is not established. Consequently, the current active
projection does not reproduce the Atari per-tube transform, while the
available evidence also does not prove that the two models are fundamentally
incompatible after a coordinate mapping.

## Evidence and assumptions

### Sources used

**Atari evidence**

- `Reference/arcade/Tempest Rom Source code.md`: table declarations at
  `$b97c-$bcfb`; level remapping at `$c2e8-$c30c`; per-level setup at
  `$c235-$c2e7`; normal tube drawing at `$c30d-$c3b9`; point transform loop at
  `$c473-$c4e0`; and CPU/Math Box coordinate routine at `$c098-$c16d`.
  Relevant source lines are 7461-7480, 8016-8135, 8202-8310, 8315-8400,
  and 8480-8524.
- `AI-Logs/Reports/2026-11-15-atari-mathbox-c098-reconciliation-report.md`:
  controlling operation and retained-state caveats.
- `AI-Logs/Reports/2026-11-15-atari-projection-data-mathbox-investigation-report.md`:
  recovered point rows, parameter tables, and prior bounded ROM checks.
- `AI-Logs/Reports/2026-11-15-option2-arcade-projection-investigation-report.md`:
  investigation history and traversal context.

The controlling reconciliation records its earlier checks of `MBUDOC.DOC`,
`MBUCOD.V05.txt`, mapping/microcode PROMs, and narrowly selected ROM ranges.
This comparison directly rechecked the local disassembly data flow, but did not
reopen or enumerate raw ROM/PROM files. It does not repeat the completed PROM
investigation.

**C# evidence**

- `src/Tempest.Game/Rendering/Projection.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `src/Tempest.Game/Rendering/DisplayConfiguration.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Levels/Level.cs:18-19`
- `src/Tempest.Game/Levels/LevelConfiguration.cs:136-141,308-375`
- `src/Tempest.Game/Levels/LevelDefinition.cs:4-9`
- `src/Tempest.Game/Levels/LevelLoader.cs:43-50,97-146`
- `src/Tempest.Game/Levels/RomPresentationMetadata.cs:49-68`

### Atari state assumptions

For the numeric sample, the selected record supplies the listed raw X/Y point,
`lev_scale`, `lev_y3d`, signed `lev_y2d/lev_y2db`, and `lev_open`. The normal
scale setup is assumed active: `$5b:$5f = $ff:(-lev_scale mod 256)`. `$5e` is
assumed to be `$80`, `$66:$67` (the X offset) is zero, and the active
`$68:$69` Y offset is assumed equal to that record's signed Y2D pair. That Y
pair is loaded directly on the corresponding setup branch; another setup path
can derive it from retained state, so this is an explicit conditional
assumption, not a claim about every call.

`$57=$10` and `$57=$f0` are the two values supplied by the normal drawing path
for its endpoint passes. `$57=$80` is an analytic midpoint sample, not a claim
that the game calls `$c098` at that value. The quotient calculation assumes
`RN.low=$0f` and `RN.high=0`; the reconciliation establishes the low-byte
write but does not establish the high-byte/reset state. Therefore the numeric
quotients below are conditional on that retained-state assumption.

Open/closed state is listed but does not enter `$c098` arithmetic. The
disassembly shows it changing traversal outside the coordinate routine.

### Normalization limits

Both depth variables increase from the near to the far endpoint in the sampled
paths. For curve comparison only, `$57` values `$10,$80,$f0` are paired by
normalized interval position `t=0,0.5,1` with C# depths
`z=50,250,450`. The C# production levels use `Position=50` and `Depth=400`.
This aligns interval endpoints and direction, not physical units. It is not a
pixel-coordinate calibration or a claim that Atari `$57` equals C# `z`.

## Current C# model

### Projection and state

`Projection.Project` (`Projection.cs:17-19,40-112`) uses global mutable `K`
(default `65`) and `ZOffset` (default `5`). For inputs
`x,y,z,centerY,yOffset`:

```text
liveDepth = z + ZOffset
depthScale = liveDepth / K
baseX = 400 + x / depthScale
baseY = centerY + (y + yOffset) / depthScale
      = centerY + (y + yOffset) * K / (z + ZOffset)
```

The implementation validates finite inputs and nonzero denominators; it does
not floor the quotient or saturate the result to signed 16-bit. X is centered
at `Width/2` (400). `K` and `ZOffset` are shared projection state, not ROM
per-tube fields.

`VectorRenderer.ProjectWorldPoint` (`VectorRenderer.cs:181-193`) supplies:

```text
z          = requested depth + ApproachDepthOffset
centerY    = CurrentLevel.Definition.CenterY
yOffset   = CurrentLevel.Definition.YOffset + ApproachVerticalOffset
nearDepth = Configuration.Position + ApproachDepthOffset
farDepth  = Configuration.Position + Configuration.Depth + ApproachDepthOffset
```

For ordinary non-approach gameplay the approach offsets are zero. Tube rails
are projected at `Configuration.Position` and
`Configuration.Position + Configuration.Depth`; entity depths instead derive
from their height/progression state. The level configurations use the shared
`Level.Position=50` and `Level.Depth=400` (`Level.cs:18-19`;
`LevelConfiguration.cs:136-141,308-375`).

The C# point input is `CurrentLevel.Definition.Points`. For example, Levels 8
and 9 center raw X at `$80`, reflect raw Y for screen-down coordinates, and
scale the points by `25/14`. Level 9's points come from ROM row 13 and Level 8's from row 7
(`LevelLoader.cs:109-146`). Other level definitions include project-authored
point sets.
`CenterY` and `YOffset` are C# level-definition fields; they are not populated
from the ROM metadata catalog in the projection call.

### Portrait correction and placement

When `PortraitPerspective != 0`, `Projection.Project` requires both endpoints
(`Projection.cs:58-109`).
Let `q(z)=1/(z+ZOffset)`, `qn=q(nearDepth)`, and `qf=q(farDepth)`. The code
computes:

```text
fraction = (qn - q(z)) / (qn - qf)
baselineInterval = 1/(farDepth + 5) - 1/(nearDepth + 5)
deltaY = PortraitPerspective * 70 * baselineInterval * fraction
portraitY = baseY + deltaY
```

Thus the correction is zero at the near endpoint and reaches its normalized
endpoint displacement at the far endpoint. It changes neither base X nor the
base depth denominator. It is independent of point X/Y, `lev_y3d`, and the
signed ROM Y2D pair. `Program.cs` sets the value only for interactive portrait
rendering; the default is zero. `--portrait-perspective` supplies a
command-line diagnostic/presentation input, not a gameplay parameter.

`Program.cs:211-213` sets the portrait value only in experimental portrait
mode. After projection, the experimental portrait camera
(`DisplayConfiguration.cs:25-58`) applies zoom
`1.2 * 1.05 = 1.26` and shared vertical placement. Its target calculation
implements a host-space translation of `1.2 * -33.1 = -39.72` pixels. This is
after the projection and is common to the rendered playfield.

### Geometry and topology inputs

`LevelDefinition.Continuous` (`LevelDefinition.cs:4-9`) controls C# topology:
`PlayableLaneCount` is `VertexCount` for a continuous tube and
`VertexCount - 1` for an open one. `DrawTube` iterates those playable lanes.
This is a geometry/topology choice, separate from the projection arithmetic.

`RomPresentationMetadataCatalog` (`RomPresentationMetadata.cs:49-68`) contains
raw per-level remap, scale, Y3D, Y2D,
and open fields for displayed Levels 1-9, but the renderer and
`Projection.Project` do not consume it. The Level 9 evidence holder also
contains row-13 X/Y points (`LevelLoader.cs:7-40`); Levels 8/9 use their
ROM-derived geometry in `LevelLoader`. This makes some source data present in
the worktree, but not active Atari-equivalent projection state.

## Recovered Atari model

For a selected record, let `p=$57`, `s=lev_scale`, `r` be the relevant axis
reference, `a` the raw axis point, and `o` the signed post-division offset.
Under normal table setup and the stated Math Box retained-state assumption:

```text
divisor = p + s
d       = abs(a - r)
q       = floor((d * 256) / divisor)
result  = sign(a - r) * q + o
```

The normal X reference is `$80` and X offset is zero. The Y reference is
`lev_y3d`; the Y offset is the active signed `lev_y2d/lev_y2db` pair. The CPU
applies sign and offset after the Math Box quotient and saturates signed
overflow to `$7fff` or `$8000`.

The disassembly (`Tempest Rom Source code.md:8016-8135,8202-8310,8480-8524`)
confirms that level setup selects parameters through
`lev_remap`, loads scale and Y3D, sets open state, and prepares Y2D state. The
Math Box result depends on the live `$57` and retained state. `lev_open` affects
traversal elsewhere, not this equation.

## Correspondence table

| Atari term or operation | Closest C# term | Classification | Reason |
|---|---|---|---|
| `$57 + lev_scale` divisor | `z + ZOffset`, divided by `K` | **PARTIAL CORRESPONDENCE** | Both produce a depth-dependent denominator, but C# has a shared additive offset and scale; it has no active per-tube `lev_scale` input. Units and endpoint values are not equated. |
| `axis_difference * 256` | signed C# point coordinate times `K` | **PARTIAL CORRESPONDENCE** | Both scale an axis displacement linearly before division. C# has floating-point `K` (default 65); no general ROM-unit mapping or integer quotient is applied. |
| X reference `$80` | centered C# X points (e.g. Level 9 subtracts 128) | **PARTIAL CORRESPONDENCE** | Level 8/9 geometry centers raw X at `$80`, but the projection receives an already-centered coordinate and has no X-reference input. |
| `lev_y3d` Y reference | C# point Y, `CenterY`, and `YOffset` | **PARTIAL CORRESPONDENCE** | These terms affect vertical placement, but the projection does not receive a ROM Y reference. C# Y geometry is centered at `$80` for Levels 8/9, not at each record's `lev_y3d`. |
| Signed `lev_y2d/lev_y2db` | C# `CenterY` / `YOffset` | **PARTIAL CORRESPONDENCE** | Atari applies a signed offset after division. C# `CenterY` is post-projection, while `YOffset` is added to the point before division; neither is loaded from the metadata catalog. |
| Per-tube `lev_scale` | C# `ZOffset` or level `Position` | **PARTIAL CORRESPONDENCE** | These can affect a denominator, but `ZOffset` is global and `Position` is shared as 50 in production; no per-tube scale is currently applied. |
| Atari post-division Y offset | `--portrait-perspective` | **PARTIAL CORRESPONDENCE** | Both can add to a projected Y, but Atari's offset is signed and record-specific after axis division. The portrait correction is a shared depth-only endpoint interpolation independent of point side/reference. |
| Atari `lev_open` traversal | C# `Continuous` / `PlayableLaneCount` | **PARTIAL CORRESPONDENCE** | Both determine whether the closing edge is traversed, but C# topology comes from `LevelDefinition`, not the Atari flag or metadata projection path. |
| Math Box floor, sign restoration, saturation | C# floating-point projection | **NO CORRESPONDENCE** | The current projection has no integer floor, signed-16 saturation, or equivalent Math Box retained-state behavior. |

No exact direct correspondence is established for the per-tube arithmetic.

## Selected tube records and level mapping

`lev_remap` is indexed by the zero-based parameter level, not by displayed
level number. The disassembly's `get_tube_no` reads that table. The selected
sample is:

| Displayed level | `lev_remap` index | Tube record | Scale | Y3D | Signed Y2D | Open | Selection reason |
|---:|---:|---:|---:|---:|---:|---|---|
| 1 | 0 | `00` | `18` | `50` | -192 | no | Enclosed baseline from Levels 1-7. |
| 7 | 6 | `06` | `18` | `68` | -144 | no | Enclosed record with a distinct Y reference/offset tuple. |
| 8 | 7 | `07` | `18` | `b0` | +96 | yes | Open record; distinct Y reference/offset from Level 9. |
| 9 | 8 | `0d` | `0c` | `b0` | +320 | yes | Arcade Level 9; distinct scale and ROM-derived C# point geometry. |

Representative point samples (hex X,Y) are indices 00 and 08:

| Level / record | Point 00 | Point 08 |
|---|---|---|
| 1 / `00` | `f0,80` | `10,80` |
| 7 / `06` | `ea,a0` | `16,60` |
| 8 / `07` | `10,f0` | `90,10` |
| 9 / `0d` | `10,90` | `90,10` |

These are ROM values from the data-recovery handoff. The current comparison did
not reread the raw ROM image.

## Numeric comparison

### Atari point outputs

For each sampled point and `$57` in `$10,$80,$f0`, the script used the
conditional quotient formula above, normal signed point direction, zero X
offset, and each record's signed Y2D offset. Values in each cell are ordered
`$57=$10 / $80 / $f0`. No result in this bounded sample saturates.

| Level / record / point | Divisors | Atari X result | Atari Y result |
|---|---|---|---|
| 1 / `00` / 00 | 40 / 152 / 264 | +716 / +188 / +108 | +115 / -112 / -146 |
| 1 / `00` / 08 | 40 / 152 / 264 | -716 / -188 / -108 | +115 / -112 / -146 |
| 7 / `06` / 00 | 40 / 152 / 264 | +678 / +178 / +102 | +214 / -50 / -90 |
| 7 / `06` / 08 | 40 / 152 / 264 | -678 / -178 / -102 | -195 / -157 / -151 |
| 8 / `07` / 00 | 40 / 152 / 264 | -716 / -188 / -108 | +505 / +203 / +158 |
| 8 / `07` / 08 | 40 / 152 / 264 | +102 / +26 / +15 | -928 / -173 / -59 |
| 9 / `0d` / 00 | 28 / 140 / 252 | -1024 / -204 / -113 | +28 / +262 / +288 |
| 9 / `0d` / 08 | 28 / 140 / 252 | +146 / +29 / +16 | -1142 / +28 / +158 |

The different X/Y outputs within a record come from the selected point
magnitudes, Y reference, and signed offset; `lev_open` does not enter these
calculations.

### Normalized depth curves

Compare relative axis gain after normalizing each model to its near endpoint.
The Atari gain is `(16 + scale) / (p + scale)`. The C# gain is
`(50 + 5) / (z + 5)` at the corresponding normalized interval positions.
This cancels unknown absolute coordinate units and is not a screen-pixel
equivalence.

| Model / record | Near (`t=0`) | Mid (`t=0.5`) | Far (`t=1`) |
|---|---:|---:|---:|
| Atari scale `18` (Levels 1, 7, 8) | 1.000000 | 0.263158 | 0.151515 |
| Atari scale `0c` (Level 9) | 1.000000 | 0.200000 | 0.111111 |
| C# base (`z=50,250,450`) | 1.000000 | 0.215686 | 0.120879 |

The C# curve is closer to Level 9's scale-`0c` curve in this normalized sample
than to the scale-`18` curves. At mid and far, C# gain is respectively about
7.8% and 8.8% above the Level 9 relative gain; against scale `18`, it is about
18.0% and 20.2% below. This is a relative-shape result under the stated
endpoint pairing, not proof of visual agreement.

### C# Level 9 point and portrait correction

Level 9 point 00 is raw `(0x10,0x90)`. Its C# geometry conversion yields
`x=-200`, `y=-28.5714`, with `CenterY=300`, `YOffset=0`. With `K=65`,
`ZOffset=5`, and `z=50,250,450`, the base projection is:

| `z` | Base X | Base Y | `deltaY` at `--portrait-perspective 125` | Corrected Y |
|---:|---:|---:|---:|---:|
| 50 | 163.636 | 266.234 | 0.000 | 266.234 |
| 250 | 349.020 | 292.727 | -124.777 | 167.950 |
| 450 | 371.429 | 295.918 | -139.860 | 156.058 |

These are C# logical projection coordinates before the portrait camera. The
correction changes only Y; it does not change the normalized X or Y base gain,
and the camera's shared placement is applied later. They are not directly
comparable to Atari's signed 16-bit outputs as screen coordinates.

### Calculation method

One inline, deterministic Python calculation used four records, two points per
record, and exactly three Atari runtime values (`$10,$80,$f0`). It computed
integer quotients with `//`, restored signs, applied the stated Y offsets, and
evaluated the corresponding C# depth gains and correction endpoints. It was
not saved as a file. No chart or image was generated.

## Structural diagnosis

1. **Does C# reproduce Atari's recovered division structure?**
   Only broadly. Both have a linear coordinate displacement divided by a
   depth-related term. C# uses floating point `coordinate * K / (z + 5)`;
   Atari's recovered path uses a conditional integer quotient
   `floor(256 * magnitude / ($57 + lev_scale))`, followed by CPU sign,
   offset, and saturation.

2. **Where do numerator, denominator, reference, offsets, units, or endpoints
   differ?**
   C# uses its point coordinate directly and global `K`; Atari forms
   differences from `$80` or `lev_y3d` and multiplies the magnitude by 256.
   C# uses global `ZOffset=5` and live C# depth; Atari adds selected
   `lev_scale` to `$57`. Atari's Y2D offset follows division; C# `YOffset` is
   pre-division and `CenterY` is post-projection. C# uses floating-point
   output without Atari's floor/saturation. C# portrait endpoint normalization
   is separate from the base projection.

3. **Does C# presently use Atari-equivalent per-tube scale, Y reference, and
   Y offset?**
   No, not in the active projection path. Raw metadata for Levels 1-9 exists
   but is inert. The renderer passes the current C# geometry's center/offset,
   global projection state, and common depth interval; no `lev_scale`,
   `lev_y3d`, or active `lev_y2d/lev_y2db` value reaches `Projection.Project`.

4. **Does `--portrait-perspective` change base depth projection?**
   No. It adds `deltaY` after base Y has been projected. It does not change X,
   `K`, the base denominator, or point geometry.

5. **Can that correction reproduce Atari's tube-specific divisor, Y reference,
   and signed offset?**
   No. It has one command-line scalar and an endpoint-normalized depth
   fraction. It has no axis-side, selected-record, Y3D, or signed Y2D input.
   Even if it approximates one rendered tube's vertical residual, it cannot
   encode those distinct Atari terms.

6. **Why might a value near 125 help Level 9 but fail Levels 1-7?**
   **NUMERICALLY DEMONSTRATED:** under the stated endpoint mapping, the C#
   normalized depth curve is closer to the selected Level 9 record's
   scale-`0c` curve than to the sampled scale-`18` curves. The C# correction is
   the same function of normalized depth for all production levels with the
   shared `50/400` interval; it has no parameter to change between those
   tuples. It does not correct differences in X, denominator gain, Y3D
   reference, or signed Y2D offset.

   **STRONG INFERENCE:** a global Y displacement can partially compensate a
   Level 9 presentation residual, while distinct shapes, scale/Y tuples, and
   open topology leave different residuals on other levels. The normalized
   curve is one plausible structural reason for the reported Level 9 symptom,
   not a visual diagnosis.

   **UNKNOWN:** whether `125` actually improves any frame or gameplay
   presentation. This milestone intentionally did not inspect screenshots,
   frames, videos, or launch the game.

## Outcome classification

The evidence supports **D — MIXED OR UNRESOLVED**:

- Outcome A is excluded: the active formulas and per-tube inputs do not match.
- Outcome B is plausible but not established as the final classification. The
  reciprocal-depth forms are structurally related, and the existing metadata
  could support a parameterized adapter, but the coordinate-unit/reference
  mapping and exact point correspondence are not established across all
  selected levels; C# also lacks Atari's integer quotient/saturation behavior.
- Outcome C is not established. The denominator forms can be aligned
  algebraically under a suitable state/coordinate mapping, so the evidence
  does not prove that the models are irreducibly incompatible.

The active C# model is not currently sufficient to reproduce the Atari
per-tube transform as wired. Whether it can do so without changing gameplay
geometry remains unresolved; the inert metadata and selected ROM-derived
Level 8/9 shapes are not proof of a complete mapping for all levels.

## Implications for the accepted portrait candidate

- The existing C# portrait formula remains a shared presentation-only Y
  correction. This comparison does not invalidate or change it.
- The shared `-33.1` placement remains a later, level-shared camera
  translation. It has no mathematical role equivalent to the Atari Y2D
  post-division offset.
- A calibration near `125` is not an Atari parameter. The numeric result
  explains why a global correction may be nearer to one sample's relative
  curve, but it does not authorize retuning or establish visual acceptance.
- Levels 1-7 have distinct point sets and multiple distinct Atari parameter
  tuples; one global correction cannot represent those per-record inputs.
- Level 8 is open and has different Y3D/Y2D metadata from Level 9, despite the
  sampled scale being `18` for Level 8 and `0c` for Level 9. The portrait
  correction does not encode either record's topology or Y tuple.
- No gameplay/model geometry was changed or invalidated. The renderer still
  projects the existing `LevelDefinition.Points`; no Atari geometry or
  metadata was wired into gameplay.

## Smallest next milestone

Perform one bounded, read-only coordinate-frame mapping for Levels 1, 7, 8,
and 9: determine whether each current `LevelDefinition.Points`, `CenterY`,
and `YOffset` can be related to its Atari row and references in a consistent
unit/sign convention without changing those gameplay/model geometries. Resolve
that mapping before proposing any projection adapter or implementation.

## Evidence classifications

- **ATARI-BACKED:** `lev_remap`, selected raw point/parameter values, the
  `$c098` operand/data flow, conditional divide equation, CPU-side sign/Y2D
  application, saturation, and separate open/closed traversal, as recorded
  and cross-checked in the controlling Atari handoffs and local disassembly.
- **C#-SOURCE-BACKED:** the floating-point base projection, portrait
  correction, shared camera placement, active geometry inputs, current
  topology handling, and inert metadata catalog.
- **NUMERICALLY DEMONSTRATED:** the stated conditional Atari point outputs,
  normalized curves, and Level 9 C# base/correction outputs from the bounded
  inline calculation.
- **STRONG INFERENCE:** normalized Level 9 curve closeness is one possible
  explanation for a global portrait correction fitting that level better;
  this does not prove what caused a visual result.
- **HYPOTHESIS:** no hypothesis is used as an established conclusion.
- **UNKNOWN:** exact coordinate-unit/sign mapping for all sampled geometries;
  actual retained `RN.high` state and some call-time Y2D state; pixel-level
  equivalence; and the observed visual effect of `--portrait-perspective 125`.

## Scope and completion

- Prompt: `AI-Logs/Prompts/2026-11-15-atari-csharp-projection-comparison-prompt.md`.
- Report: `AI-Logs/Reports/2026-11-15-atari-csharp-projection-comparison-report.md`.
- Files created: only the authorized prompt and this report, both ignored under
  `AI-Logs/`.
- Source files inspected: the three required Atari handoff reports; local
  Tempest disassembly sections named above; C# projection, renderer, display,
  program, level/configuration/definition/loader, and ROM metadata source files.
- Atari data/ROM/PROM ranges rechecked in this milestone: no raw ROM or PROM
  range. The selected table bytes and earlier bounded checks are taken from
  the required data-recovery and reconciliation reports.
- Calculations: one bounded inline Python script; no temporary or permanent
  script file. No tests/builds were required or run. The game was not launched.
- Starting Git state: branch `master`, HEAD
  `385f603a94391756b2ca6cccd524dc17b281e782`, tracking `origin/master`,
  16 pre-existing tracked modifications, four pre-existing untracked paths,
  and an empty index.
- - Ending Git state: branch `master`, HEAD
  `385f603a94391756b2ca6cccd524dc17b281e782`, tracking `origin/master`;
  the same 16 tracked modifications and four untracked paths as at start;
  index remains empty. The two new AI-Logs artifacts are ignored.
- No tracked files were modified. The existing index was not altered. Nothing
  was staged, committed, or pushed.
- Most important demonstrated equivalence: both base transforms have a
  reciprocal-depth coordinate-gain structure; selected Level 9 and C# relative
  depth curves are close under the stated endpoint normalization.
- Most important demonstrated mismatch: Atari's active per-record scale,
  Y3D/Y2D, integer quotient/sign/saturation path is not present in the active
  C# projection; portrait perspective is only a shared post-projection Y
  correction.
- Most important remaining unknown: whether all current production level
  geometry can be mapped to the corresponding Atari point/reference
  coordinates without changing gameplay/model geometry.
