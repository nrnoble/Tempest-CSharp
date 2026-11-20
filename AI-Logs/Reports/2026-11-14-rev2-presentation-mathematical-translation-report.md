# Rev 2 Presentation Mathematical Translation Investigation

**Date:** 2026-11-14
**Status:** READ-ONLY INVESTIGATION COMPLETE
**Primary diagnostic classification:** READY FOR ROM-BACKED METADATA FOUNDATION ONLY
**Completion token:** `TEMPEST_REV2_PRESENTATION_MATHEMATICAL_TRANSLATION_20261114`

## Scope and Git state

This investigation derives the presentation stages visible in the authorized
Rev 2 disassembly and compares them with the current C# projection path. It
does not implement a projection, select C# constants, or change gameplay or
geometry.

Starting state:

- Branch: `master`; upstream: `origin/master`.
- `HEAD` and `origin/master`: `385f603a94391756b2ca6cccd524dc17b281e782`.
- Index: clean.
- The ten expected tracked presentation-candidate files were modified and
  unstaged; no non-ignored untracked files existed.
- The tracked diff was 1,763 insertions and 88 deletions. Its captured
  `git hash-object` value was
  `f3bf422e9f38b61ebca93265f822faca29a24fd9`.
- The candidate paths and state matched the three required prior reports;
  no unrelated user work was found.

The substantive task was preserved verbatim at
`AI-Logs/Prompts/2026-11-14-rev2-presentation-mathematical-translation-prompt.md`.

No source or test files were changed. No build, test suite, or game was run.
No source diff was staged, committed, or pushed.

## Materials inspected and boundaries

Project-local source and prior evidence:

- `Reference/arcade/Tempest Rom Source code.md`, bounded to the level setup
  and remap at `$c235-$c303`, the coordinate conversion at `$c098-$c16d`,
  its tube drawing caller around `$bd09`, the transition update at
  `$904d-$90b3`, and vector-list conversion `vapp_to(X)` at `$c765-$c79d`.
- `AI-Logs/Reports/2026-11-14-rev2-presentation-table-extraction-report.md`
  for the already extracted Rev 2 raw table values and level mapping.
- `AI-Logs/Reports/2026-11-14-levels1-9-rom-camera-perspective-readonly-investigation-report.md`
  for prior source/C# comparisons and the recorded manual observations.
- `AI-Logs/Reports/2026-11-14-whole-game-presentation-human-visible-perspective-calibration-report.md`
  for the current portrait diagnostic's project-calibration role.
- Current `Projection.cs`, `VectorRenderer.cs`, and
  `DisplayConfiguration.cs` in `src/Tempest.Game/Rendering/`.

No screenshots, images, videos, MAME/C# frames, recordings, Gemini material,
Incitti source, web sources, raw ROM files, or ROM-directory contents were
inspected. `TEMP/myNotes.md` was not read.

## Authoritative ROM values used

The values below are **ROM-BACKED raw/source-interpreted data** copied from
the extraction report; they were not re-extracted in this investigation.

| Displayed level | Tube index | `lev_scale` | `lev_y3d` | Signed `lev_y2d` | `lev_open` |
|---:|---:|---:|---:|---:|---|
| 1 | 0 | `0x18` | `0x50` | -192 (`0xFF40`) | closed |
| 2 | 1 | `0x1C` | `0x50` | -224 (`0xFF20`) | closed |
| 3 | 2 | `0x18` | `0x50` | -192 (`0xFF40`) | closed |
| 4 | 3 | `0x0F` | `0x68` | -128 (`0xFF80`) | closed |
| 5 | 4 | `0x18` | `0x50` | -192 (`0xFF40`) | closed |
| 6 | 5 | `0x18` | `0x50` | -192 (`0xFF40`) | closed |
| 7 | 6 | `0x18` | `0x68` | -144 (`0xFF70`) | closed |
| 8 | 7 | `0x18` | `0xB0` | +96 (`0x0060`) | open |
| 9 | 13 | `0x0C` | `0xB0` | +320 (`0x0140`) | open |

`lev_y2d` is assembled from low byte `lev_y2d` and high byte `lev_y2db` as
little-endian signed 16-bit two's complement. The report establishes the
displayed-level to table-index mapping. No C# value or the portrait `+125`
diagnostic was substituted for a ROM field.

## Atari coordinate pipeline

### Source-proven setup and input formation

**SOURCE-BACKED:** `$c235` selects the player's level, calls `get_tube_no`,
and uses `curtube` to load the per-tube presentation arrays. The fields are
loaded independently. The source does not branch on `lev_open` to select a
camera formula.

**SOURCE-BACKED:** `$c240-$c251` complements `lev_scale` and adds one, storing
the byte result `-L` in `$5f` and `$5d`. `$5b` is set to `0xFF`, providing
the high byte paired with `$5f` for the 16-bit subtraction in the coordinate
routine. Setup also derives `$a0 = 0x10 - (-L) = 0x10 + L`; the inspected
coordinate path does not use `$a0` as its projection denominator.

At `$c098-$c0a4`, the routine forms a 16-bit value from the point's
along-tube position `$57`:

```text
denominator_input = (0x00:pointDepth) - (0xFF:-L)
                  = pointDepth + L
```

It writes the low and high bytes to mathbox inputs `mb_w_15` and `mb_w_16`
(addresses `$6095/$6096`). The sign check clamps a negative result to one;
for the supplied positive scale values and ordinary byte positions, the
formed depth-plus-scale value is nonnegative. The source-proven role is
therefore a scale-derived adjustment to the depth-like mathbox input, not a
direct multiplication of model X/Y and not a numerator constant.

The same depth input is used for both projected components. The routine
compares point Y `$58` with `y3d` and forms the unsigned magnitude
`abs(pointY - y3d)` plus a separate sign byte (`$33`). That magnitude is
written to mathbox inputs `mb_w_0e` and `mb_w_14` (`$608e/$6094`); after the
mathbox completes, its 16-bit result is read into `$63/$64`.

The routine then compares point X `$56` with `$5e` (initialized to `0x80`)
and forms `abs(pointX - 0x80)` plus sign byte `$34`. It reuses the same
depth input, writes this second magnitude to `mb_w_0e` and `mb_w_14`, and
reads the result into `$61/$62`.

### Mathbox boundary and output ordering

**SOURCE-BACKED:** The CPU-side sequence establishes two signed-magnitude
coordinate requests against a shared depth input, and that each response is
16 bits. It does not expose the mathbox's internal transfer function,
coefficient, fixed-point radix, rounding, or exact multiply/divide
implementation. Consequently this investigation cannot responsibly replace
the hardware operation with a fully numeric floating-point equation.

**MATHEMATICAL DERIVATION:** A useful symbolic description of the source flow
is:

```text
D = pointDepth + lev_scale
dy = pointY - lev_y3d
dx = pointX - 0x80

mathY = sign(dy) * M(abs(dy), D)
mathX = sign(dx) * M(abs(dx), D)
resultY = signed_16(lev_y2d) + mathY
resultX = baseX + mathX
```

`M(magnitude, D)` names the unavailable mathbox operation; it is not asserted
to be an exact `K * magnitude / D` equation. The reciprocal/division-like
interpretation is supported by the source's `lev_scale` comment, the
depth-like input pairing, and the common projection use, but the mathbox
equation and units are **UNKNOWN** from the authorized text.

The signed 16-bit `lev_y2d` is applied after the first mathbox response:
when `dy` is nonnegative, the routine adds `$68/$69` to `$63/$64`; when
negative, it subtracts the projected magnitude from `$68/$69`. Overflow is
saturated to signed 16-bit maximum (`0x7FFF`) or minimum (`0x8000`).
Thus, **SOURCE-BACKED**, its role is a post-projection constant placement,
not another camera-height input.

`vapp_to(X)` receives the four-byte block beginning at `$61`. Its comment
defines the vector-list values as `X = DDCC` and `Y = BBAA`, each interpreted
as a signed 13-bit value with the high three bits dropped. Here `CC:DD`
comes from `$63/$64` (the component based on point Y and adjusted by
`lev_y2d`), while `AA:BB` comes from `$61/$62` (the component based on point
X). Therefore the source/mathbox Y component is serialized as vector-generator
X, and the source/mathbox X component as vector-generator Y. This axis
ordering must be accounted for before equating Atari offsets with C# screen
axes. The local text does not establish the final physical-monitor-to-host
pixel mapping or the mathbox fixed-point scale.

### `lev_y3d` and camera-height interpretation

**ROM-BACKED:** `lev_y3d` is documented as a 3-D Y offset / “camera height.”
**SOURCE-BACKED:** The routine computes point Y minus `y3d` by comparing the
two bytes, then passes the magnitude and sign to the mathbox before adding
the 2-D offset. In symbolic form the projected numerator is based on
`pointY - y3d`, not `y3d - pointY`.

**MATHEMATICAL DERIVATION:** This is the stage of a vertical camera/reference
translation before perspective conversion. It changes the projected
Y-derived component through its relative-coordinate magnitude; the inspected
routine does not feed `y3d` into the depth input or the separate X-relative
coordinate request. It therefore does not directly alter both source axes.
Because `vapp_to(X)` swaps the component words into vector-generator axes,
the Y-derived component reaches vector X in that output format. This is an
axis serialization fact, not evidence that `y3d` also changes the X mathbox
calculation.

If C# point coordinates had Atari's same origin, unit, and axis direction,
the algebraic input corresponding to `pointY - y3d` would be
`worldY + YOffset` with `YOffset = -y3d`. Those conditions are not
established: C# Y orientation, geometry normalization, and the DVG-axis
mapping require a conversion. No such conversion or C# value is selected
here.

### `lev_y2d`, signed representation, and transition

**SOURCE-BACKED:** The low/high bytes are loaded into `$68/$69` as a signed
16-bit quantity. At normal coordinate conversion the quantity is added to
or subtracted from the mathbox Y-derived result according to the pre-projection
`pointY - y3d` sign. This is a pure constant translation in that projected
mathbox component, independent of the current point depth; the DVG serialization
then places that component in vector X.

On initial setup (`$02 == 0x1E`) the target signed offset is copied directly.
Otherwise `$c275-$c28b` computes target minus current as a 16-bit difference
and shifts it right four places to establish a transition increment. The
`$904d` state uses a fractional accumulator and updates `$68/$69` over the
transition rather than replacing the value at once. This confirms animated
2-D placement; the inspected excerpt does not establish an exact pixel-time
curve or C# duration.

**PROJECT TRANSLATION:** C# `CenterY` is at the correct broad *post-projection
addition stage* to host a converted constant offset. It is not a direct
equivalent without converting signed 16-bit Atari vector units, the DVG axis
ordering, origin/center, and the C# screen-down convention. The required
base-origin and axis conversion remains unresolved.

### `lev_scale` interpretation

**ROM-BACKED:** The source labels `lev_scale` reciprocal level scale.
**SOURCE-BACKED:** Setup negates the byte, and the common coordinate routine
subtracts that negative value from point depth. The actual input is
`pointDepth + lev_scale`, shared by the X- and Y-derived mathbox requests.
Thus, operationally in this path, the raw field adjusts the depth-like
denominator input; it is not shown multiplying X/Y numerators or scaling
the level's model points.

**MATHEMATICAL DERIVATION:** If the mathbox response is ratio-like, a smaller
`lev_scale` at fixed point/depth increases the relative-coordinate response
because it reduces `D = pointDepth + lev_scale`. It also changes the
near/far response non-uniformly, with the largest relative effect where
point depth is small. The precise transfer curve remains unavailable.

Current C# `K` is a global numerator/focal-like multiplier, so the raw Atari
byte is not conceptually interchangeable with `K`. C# `ZOffset` is
stage-analogous because it is added to depth before division, but it is
global, has different unverified units, and is not currently populated from
per-level `lev_scale`. No direct mapping to `K`, `ZOffset`, `1.05`, or the
geometry import scale `25/14` is justified.

## Representative level pipelines

For each row, let `d`, `x`, and `y` be the same hypothetical source point
inputs. `M` remains the unknown mathbox response described above. These
examples isolate table-input effects; they do not claim exact screen
coordinates or validate the point geometry.

| Level | Source-backed symbolic inputs | Effect for the same point |
|---:|---|---|
| 1 | `D=d+24`; `dy=y-80`; post-offset `-192` | Baseline example: mathbox Y response is based on `y-80`, then shifted by -192; both projected components share `d+24`. |
| 4 | `D=d+15`; `dy=y-104`; post-offset `-128` | Relative to Level 1, depth adjustment is 9 smaller, the camera-height reference is 24 higher, and the post-offset is 64 less negative. |
| 7 | `D=d+24`; `dy=y-104`; post-offset `-144` | Same scale as Level 1, but camera-height reference is 24 higher and post-offset is 48 less negative. |
| 8 | `D=d+24`; `dy=y-176`; post-offset `+96` | Same scale as Levels 1/7, much higher camera-height reference, and the post-projection offset crosses from negative to positive. |
| 9 | `D=d+12`; `dy=y-176`; post-offset `+320` | Same camera-height reference as Level 8, half its scale adjustment, and a post-offset 224 greater than Level 8. |

In every case the X-derived component uses `dx=x-128` and the same
`D=d+lev_scale`; `lev_y2d` does not enter that X-relative request. Mathbox
numerics and DVG-to-pixel conversion are unavailable, so exact coordinates
stop at the symbolic pipeline.

## Level 1 versus Level 9

**ROM-BACKED:** Level 1 has `(scale=24, y3d=80, y2d=-192)`, while Level 9
has `(scale=12, y3d=176, y2d=+320)`.

**MATHEMATICAL DERIVATION:**

- **Depth/convergence:** at a fixed point depth, Level 9's smaller scale byte
  gives `d+12` rather than `d+24`. Under a ratio-like mathbox, this raises
  projected relative-coordinate magnitude and changes near/far convergence;
  it does not merely enlarge the numerator uniformly.
- **Camera height:** the Y reference rises by 96 source units. This changes
  `pointY-y3d` before projection, so its vertical effect varies with depth.
- **Placement:** the signed post-projection offset changes by +512 source
  units. It is depth-independent within the mathbox result axis, subject to
  the later vector-axis conversion.
- **Apparent tube size:** scale and camera-height affect projected
  coordinates of geometry; the 2-D offset alone translates the resulting
  projected component and does not change its internal dimensions.

**MANUAL OBSERVATION:** Current C# Level 9 can be made visually convincing
with approximately `+125` on the shared endpoint-normalized
`--portrait-perspective`, while Levels 1–7 cannot be corrected by coefficient
tuning and gameplay remains correct. The ROM differences above provide
plausible independent presentation mechanisms, but the unavailable mathbox
transfer and C# unit/axis conversions prevent attributing the observed
symptoms to any one field. `+125` is not a ROM field or a future target value.

## Open/closed control

**SOURCE-BACKED:** `lev_open` is loaded alongside the camera fields, but the
coordinate conversion at `$c098` does not read it. The source uses open status
for topology/edge and movement behavior; it does not select a different
camera equation.

**OPEN/CLOSED CORRELATES WITH SOME CURRENT MANUAL VISUAL FAMILIES BUT DOES NOT SELECT THE ATARI CAMERA FORMULA.**

No topology-based camera branch is supported.

## Current C# stage comparison

Current `Projection.Project` computes:

```text
depthScale = (z + ZOffset) / K
screenX = 400 + x / depthScale
screenY = CenterY + (y + YOffset) / depthScale
```

The endpoint-normalized portrait diagnostic adds a depth-normalized Y
correction after that base projection. `VectorRenderer.ProjectWorldPoint`
feeds current level `CenterY`/`YOffset` and the shared near/far depth interval
to this common method. The playfield camera subsequently applies shared
portrait zoom/translation; the 720-by-960 viewport is centered/scissored in
the host window, while the HUD uses a separate camera.

| Atari concept | Current C# comparison | Classification |
|---|---|---|
| `lev_scale` | No per-level field. `ZOffset` has the depth-addition stage; `K` is a different numerator-like factor. | **MISSING** as ROM-backed metadata; `ZOffset` is **STAGE-ANALOGOUS BUT UNSOURCED**. |
| `lev_y3d` | `YOffset` is added to Y before division, but its sign, units, origin, and axis mapping are not converted from Atari. | **STAGE-ANALOGOUS BUT UNSOURCED**. |
| `lev_y2d` | `CenterY` is added after base division, but no signed table conversion exists; the DVG axis mapping and units differ. | **STAGE-ANALOGOUS BUT UNSOURCED**. |
| `lev_open` | C# `Continuous` represents open/closed tube topology; neither is a camera selector. | **DIRECT EQUIVALENT** for topology only; **NOT COMPARABLE** as projection input. |
| Model X/Y | Atari point/midpoint bytes enter separate X/Y mathbox requests; C# `Vector2` geometry may be literal or normalized/import-scaled. | **NOT COMPARABLE** numerically without geometry/origin/axis conversion. |
| Depth | Atari point-along-tube byte is combined with `lev_scale`; C# `z` is shared renderer depth plus transition offset. | **STAGE-ANALOGOUS BUT UNSOURCED**; unit mapping is unknown. |
| Final output placement | Atari emits signed 13-bit DVG coordinates after mathbox and `vcentre`; C# emits logical pixels, then Raylib camera/viewport transforms them. | **APPROXIMATION** of the final presentation stage, not a direct coordinate mapping. |

The C# camera and viewport are a subsequent host presentation transform; they
cannot substitute for the missing per-level Atari projection inputs.

## Can `CenterY` and `YOffset` represent both Atari vertical concepts?

**MATHEMATICAL DERIVATION:** Algebraically, the C# form can hold two
independent vertical effects:

```text
postOffset + (pointY + preOffset) * gain / (depth + depthOffset)
```

The pre-division term is structurally compatible with a converted
`pointY-y3d`; the post-division term is structurally compatible with a
converted `y2d`. Under matching coordinates and signs, `YOffset` would carry
`-y3d`, and `CenterY` would carry the converted signed 2-D placement.

This establishes representational capacity, not a safe numeric translation.
The C# screen-Y output is not yet demonstrated to use the same basis as the
Atari mathbox Y component, and `vapp_to(X)` serializes that component as DVG X.
Atari's 16-bit offset units, the DVG origin and axis ordering, the C# Y
reflection, and the projection scale all need an explicit conversion.
Moreover, the C# equation currently has only one shared `ZOffset` and `K`;
it does not express or validate the full Atari mathbox operation.

Therefore the existing field stages are useful analogues, but simply copying
or fitting values into current `CenterY`/`YOffset` is not established as
arcade-faithful.

## Shared projection and per-level metadata architecture

**SOURCE-BACKED:** Setup loads the fields by the selected tube index and the
same coordinate routine consumes them for all levels. No per-level projection
subroutine or open/closed camera branch appears in the inspected path.

**PROJECT TRANSLATION:** A data-only per-level presentation record feeding
one shared, stage-accurate projection routine is the best-supported future
architecture. It should keep the source fields distinct (depth/reciprocal
scale input, pre-projection camera-height reference, signed post-projection
placement, and topology), and make all unit/sign/axis conversions explicit.
The record can preserve raw ROM values without claiming their C# pixel units.

A future equation should follow the discovered order and signed-component
handling, but must not be encoded as an assumed `K*y/(z+ZOffset)` until the
mathbox transfer and output scale are verified. Per-level functions,
hand-tuned coefficients, and topology-based projection branches are not
supported by the shared source path.

## Role of `--portrait-perspective`

**MANUAL OBSERVATION:** Approximately `+125` is useful evidence about the
current incomplete C# projection and Level 9 only.

**PROJECT TRANSLATION:** Classification **D — cannot be determined until
implementation/manual comparison**. Keep the current diagnostic unchanged
and keep its default at zero. After a validated ROM-backed shared projection
exists, compare at zero first; only manual acceptance can establish whether
a small independent project calibration is still useful. Do not treat +125
as a ROM constant or bake it into per-level metadata.

## Gameplay/model geometry protection

**PROJECT TRANSLATION:** The identified Atari scale, camera-height, and
2-D-placement translations belong to presentation/projection. They do not
justify changing `LevelDefinition.Points`, lane coordinates, enemy positions,
or gameplay geometry. Manual evidence says tested gameplay remains correct.
Preserve model/gameplay geometry unless separate direct evidence establishes
that geometry itself is mistranslated.

## Decision matrix

| Option | Evidence support | Architectural cleanliness | Fidelity potential | Gameplay risk | Unresolved evidence |
|---|---|---|---|---|---|
| A. Reuse current `CenterY`/`YOffset` with ROM conversions | The pre/post stages are analogous, but no unit/sign/axis conversion is established. | Moderate as a temporary interface; unsafe as a claimed final mapping. | Medium to high only after mathbox and DVG conversions are verified. | Low if rendering-only. | Mathbox transfer, source-to-C# axes, origin, units, and scale. |
| B. Per-level metadata feeding the current equation unchanged | Raw metadata is established; current equation has a matching broad order but not a proven full transfer. | Good data model, incomplete equation. | Medium; risks flattening distinct mathbox semantics. | Low if rendering-only. | Denominator interpretation, output gain, units, axis mapping. |
| C. Per-level metadata plus a revised shared projection | Common source routine consumes per-level inputs; stages and signed placement are visible. | **Best-supported option.** | High potential without per-level special cases. | Low if strictly rendering-side and deterministic. | Exact mathbox operation, quantization, DVG-to-pixel conversion. |
| D. Keep current projection and solve only with `--portrait-perspective` | Conflicts with distinct ROM-backed per-level inputs and manual Levels 1–7 result. | Poor as a complete solution. | Low. | Low gameplay risk, high fidelity risk. | Does not address missing scale/camera/placement data. |
| E. Per-level hand-tuned coefficients | No source support for coefficients as replacements for the fields. | Poor; duplicates behavior and obscures provenance. | Low and fragile. | Low gameplay risk, high maintenance/fidelity risk. | All source-to-C# conversions remain open. |
| F. Open/closed-specific cameras | `lev_open` is topology data; camera fields load independently. | Unsupported branching. | Low. | Low direct gameplay risk; presentation behavior would be speculative. | No source evidence for such a camera rule. |

## Primary classification and recommended next milestone

**READY FOR ROM-BACKED METADATA FOUNDATION ONLY.**

The extracted bytes and displayed-level mapping can safely be represented as
data without changing presentation. Source proves distinct input and output
stages and their broad ordering, but it does not expose the mathbox equation,
fixed-point radix/rounding, or DVG-to-host coordinate conversion. Therefore
an active ROM-backed projection implementation is not yet responsible.

Smallest next implementation milestone: introduce and test data-only
per-level presentation metadata for the extracted Levels 1–9 rows, preserving
the raw scale byte, camera-height byte, signed 16-bit 2-D offset, table index,
and open/closed topology. Keep rendering behavior and gameplay geometry
unchanged in that milestone. A later bounded translation milestone should
verify the mathbox transfer/output units and DVG axis/origin mapping before
wiring metadata into a revised shared projection routine.

## Unresolved evidence

- The mathbox's exact function, numerator/gain, fixed-point radix, rounding,
  and any internal saturation are not present in the inspected textual code.
- The scale comment says reciprocal level scale, while CPU input formation
  makes `pointDepth + lev_scale`; the exact mathbox interpretation of this
  denominator-like input is not proven.
- Exact conversion from source 8-bit points and signed 16-bit y2d to DVG
  coordinates, then physical vector-screen pixels, is unknown beyond the
  signed 13-bit output packing and component ordering.
- The conversion from these vector units/origins/axes to C# logical pixels,
  the corresponding C# sign for camera height, and per-level gain are not
  established.
- The diagnostic's residual role after any correct translation requires
  later implementation and Neal's manual visual acceptance.
- No gameplay or geometry defect is established; no geometry change is
  authorized by this investigation.

## Validation and final state

Read-only mathematical/source investigation; build and tests were not
applicable and were not run.

Final state:

- Branch remains `master`; upstream remains `origin/master`.
- `HEAD` and `origin/master` remain
  `385f603a94391756b2ca6cccd524dc17b281e782`.
- Index remains clean.
- The same ten pre-existing candidate source/test files remain modified and
  unstaged; the tracked diff hash remains
  `f3bf422e9f38b61ebca93265f822faca29a24fd9`.
- No non-ignored untracked files were introduced. The only task outputs are
  the requested ignored prompt and report.
- No source/test changes were made. Nothing was staged, committed, or pushed.
- No visual, Gemini, Incitti, web, or raw-ROM material was inspected.
