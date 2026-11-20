# Atari Visible X/Y Gain — Read-Only Investigation

**Date:** 2026-11-16
**Status:** Investigation complete; original physical X/Y gain unresolved
**Scope:** Atari coordinate/display evidence and the current Level 7 direct-Atari prototype; no implementation

## Direct answer

The available project-local evidence does **not** establish whether an equal
Tempest AVG vector-coordinate increment produces equal visible horizontal and
vertical lengths on an original cabinet. It also does not establish a fixed
unequal ratio, a nominal ratio subject to adjustment, or independent X/Y
adjustment ranges. Therefore no numeric Atari X:Y visible-gain ratio can be
reported.

The Atari software path does apply the same digital coordinate calculation to
both source components and uses a common vector-scale command; the source
component references differ, and serialization places the source-Y result in
vector X and source-X result in vector Y. That is evidence about the digital
vector list, not the analog X/Y transfer or the calibrated monitor image.
Project-local documentation of the X/Y DAC/integrator circuits, deflection
amplifiers, their independent gains, and the monitor's size/calibration
settings was not found.

The immediate reason the current C# prototype looks wider than the authorized
comparison image is established in code: its final map uses `displayAspect:
1.0`, making its screen-pixel gain equal on the two axes. The image-only
diagnostic measured a 1.00 C# tube-bounds ratio versus 0.763 in the reference
image. That visual measurement is **not** an Atari hardware ratio and cannot
fill the missing circuit evidence.

**Disposition:** a global non-uniform X/Y transform could explain the
proportional shape difference without changing any record-06 points or
connectivity. There is **not enough Atari hardware evidence to authorize a
hardware-backed aspect correction or choose its value**. The narrow remaining
gap is the original Tempest AVG X/Y analog transfer and its calibration to the
installed monitor.

> Terminology: the Atari disassembly identifies Tempest's vector generator as
> an **AVG** (Analog Vector Generator), rather than a DVG. The project prototype
> uses `AtariDvg*` type names and this task uses “DVG”; below, “vector
> coordinates” refers to the source-side signed coordinates without claiming
> that the C# prototype emulates Atari's AVG hardware.

## Scope and evidence

### Local sources inspected

- [Tempest Rom Source code.md](../../Reference/arcade/Tempest%20Rom%20Source%20code.md):
  Atari-side coordinate routine and vector-list serialization, including the
  previously traced `$c098` coordinate path and `$c765-$c79d` vector helper.
- [MBUDOC.DOC](../../Reference/arcade/MBUDOC.DOC) and
  [MBUCOD.V05.txt](../../Reference/arcade/MBUCOD.V05.txt): local Math Box
  interface and microcode documentation. These concern arithmetic, not the
  AVG analog-output or monitor-deflection circuit.
- [AtariCoordinateProjector.cs](../../src/Tempest.Game/Rendering/AtariCoordinateProjector.cs#L40),
  [AtariTubeVectorListBuilder.cs](../../src/Tempest.Game/Rendering/AtariTubeVectorListBuilder.cs#L30),
  [AtariDvgViewportMap.cs](../../src/Tempest.Game/Rendering/AtariDvgViewportMap.cs#L5),
  [VectorRenderer.cs](../../src/Tempest.Game/Rendering/VectorRenderer.cs#L403),
  and [DisplayConfiguration.cs](../../src/Tempest.Game/Rendering/DisplayConfiguration.cs#L10):
  current direct-Atari path and final raster mapping.
- Existing local investigations:
  [DVG / AVG Vector Orientation and Units](./2026-11-15-dvg-vector-orientation-units-readonly-report.md),
  [Math Box-to-vector coordinate translation](./2026-11-15-mathbox-dvg-coordinate-translation-readonly-report.md),
  [direct Atari tube-renderer architecture](./2026-11-16-direct-atari-tube-renderer-architecture-investigation-report.md),
  and [Level 7 screenshot shape diagnostic](./2026-11-16-level7-mame-vs-csharp-shape-diagnostic-investigation-report.md).
- The two authorized visual references:
  [Level-7 MAME.png](<../../UserData/Screenshots/Level-7/Prototype/Level-7%20MAME.png>)
  and
  [Level-7 prototype-cs.png](<../../UserData/Screenshots/Level-7/Prototype/Level-7%20prototype-cs.png>).
  They were used only as visual references. No MAME source, emulator
  implementation, or display configuration was investigated.

The local Atari texts inspected do not include the original Tempest AVG
schematic or monitor deflection/service documentation needed to establish
analog gain, adjustment range, or calibration target. The Math Box documents
do not describe those display electronics. No raw ROM copies were opened or
examined.

The digital Math Box/ROM arithmetic and the record-06 connectivity were not
re-derived here. In particular, this work did not repeat the prior RN-high or
factor-128 investigations, and it did not reopen the settled record-06
structure.

## Coordinate-space chain

| Space | Evidence-backed description | X/Y gain status |
|---|---|---|
| **A. Raw tube record** | Per-point X/Y are unsigned record bytes, not pixels. | No visible gain yet. |
| **B. Projected Atari source coordinates** | The ROM path forms signed magnitudes around separate X and Y references, computes both through the common per-record/runtime scale path, applies the source-side base/offset, then prepares vector components. The established C# prototype computes these values in `AtariCoordinateProjector`. | Same digital scale calculation for both components; Y has a separate source offset. This is not analog gain. |
| **C. Atari vector-coordinate words** | Atari's vector helper serializes source-Y-derived data as vector X and source-X-derived data as vector Y. The local ROM-derived report records signed 13-bit VCTR values and the common `vscale b=1 l=0` / `vcentre` commands. | Axis order and common vector scale are software/list facts. They do not measure physical X/Y sensitivity. |
| **D. Command/vector deltas** | Atari emits AVG commands; the C# builder instead retains endpoint points and line segments. It does not serialize or send those segments through an AVG/DVG device. | No additional Atari command-stream gain is implemented by this C# renderer. |
| **E. DAC/analog deflection** | No local schematic or component-level source traces either axis from DAC/integrator through deflection amplifier. | **Unknown**: equal, unequal, adjustable, and nominal values are not established. |
| **F. Physical monitor coordinates** | No local source establishes the cabinet monitor's axis sensitivity, physical mounting transform, size controls, or final calibration. | **Unknown**. Portrait orientation alone does not establish non-square gain. |
| **G. C# logical portrait coordinates** | The provisional map centers into 600x800 coordinates, uses global scale `0.4`, `displayAspect=1.0`, no map-level axis swap or rotation, X sign `+1`, and Y sign `-1`. | Equal logical X/Y gain by explicit provisional choice. |
| **H. C# raster pixels** | The direct prototype uses the portrait HUD camera (uniform zoom `1.2`, X offset `280`) and the 720x960 portrait scissor viewport. | Uniform camera scale preserves the map's 1:1 aspect. |

### Atari software-side scaling

The ROM-derived coordinate analysis records a shared divisor for the two
source components, formed from runtime position plus the record scale, and a
common quotient operation. The existing arithmetic report gives the
normal-path form as `floor((magnitude * 256) / divisor)` for each component;
this investigation relies on that established result and does not re-open its
Math Box call-state qualification. The inspected vector list uses the common
`vscale b=1 l=0` command. No independent per-axis digital multiplier was
identified in this path.

There are still meaningful coordinate distinctions: X and Y use different
source references; the Y-derived source result has its record offset; and
Atari serializes the Y-derived result into vector X. These change origins,
signs, or axis identity; they are not evidence of different physical
deflection gains.

The signed 13-bit vector-word range (`-4096..4095`) is established by the
source-side packing evidence summarized in the local DVG/AVG investigation.
It is a digital representation range, not a monitor width/height ratio.

### Hardware gain and calibration

| Question | Finding |
|---|---|
| Different nominal digital X/Y gain? | None identified in the traced software path; it uses common coordinate arithmetic and a common vector-scale command. |
| Separate X/Y DAC scaling or analog gain? | **Unknown**; the local evidence does not include these circuits. |
| Different X/Y deflection-amplifier gain? | **Unknown**. |
| Independent X-size/Y-size controls or calibration components? | **Unknown**; no local schematic or service description establishes their presence, function, or range. |
| Monitor orientation? | The comparison is portrait, but available local primary material does not establish cabinet mounting or the relation between physical portrait orientation and axis sensitivity. Orientation can change mapping/order without proving gain. |
| Equal logical vector increments produce equal visible lengths? | **Unknown** for original hardware. |
| Atari nominal X:Y visible-gain ratio? | **Not established; no numeric ratio can be calculated.** |
| Cabinet-adjusted/factory-calibrated value? | **Not established**; no adjustment range or calibration target was found. |

Thus the strongest supported Atari statement is limited: the ROM/AVG command
path does not show a separate software X-versus-Y multiplier, but local
evidence ends before analog deflection and the calibrated monitor image.

## Current C# mapping and the equal-gain assumption

`AtariCoordinateProjector` assigns the source-Y result to `AtariDvgPoint.X`
and source-X to `.Y`. `AtariTubeVectorListBuilder` uses those points as the
near/far endpoints and rim vertices. The direct-Atari renderer maps each
segment endpoint with `AtariDvgViewportMap.DevelopmentOnlyProvisional` and
passes the resulting line to `Raylib.DrawLineV`.

The map's current values and equations are:

```text
logicalX = 300 + 0.4 * DVG_X
logicalY = 400 - 0.4 * DVG_Y
screenX  = 280 + 1.2 * logicalX = 640 + 0.48 * DVG_X
screenY  =       1.2 * logicalY = 480 - 0.48 * DVG_Y
```

In the parameterized map, `displayAspect` multiplies X only. If it were `a`,
the absolute output gains would be `0.48*a` pixels per vector-coordinate
unit horizontally and `0.48` vertically; therefore the current code's ratio
is **C# X:Y = 1:1**. The equality comes exactly from
`displayAspect: 1.0` in the provisional map, followed by a uniform camera
scale. It is not required by Atari evidence, nor an accepted project
translation. The code labels the map `DevelopmentOnlyProvisional`, and its
comment explicitly says physical gain/aspect are unknown: this is a
**DEVELOPMENT-ONLY provisional choice**, unsupported as a claim about Atari
hardware.

The map's signs, origin, rotation, and the projector's axis permutation are
also C# implementation choices at the boundary. They must not be read as
proof of the physical monitor's orientation or X/Y sensitivity.

## Screenshot comparison and shape question

The prior same-day image-only investigation measured blue-tube bounds of
approximately 652 by 652 pixels in the C# image and 284 by 372 pixels in the
authorized comparison image: **visual diagnostic ratios** 1.00 and 0.763.
Those are raster measurements only. The comparison image is not used to infer
Atari circuitry or to set an implementation constant.

Mathematically, a positive global anisotropic transform
`(x,y) -> (kx*x, ky*y)` with `kx != ky` changes the visible X/Y proportions
while preserving the same point indices, endpoint pairing, adjacency, and
closed-rim topology. Because it is invertible, it also preserves the
underlying connectivity and does not introduce point reordering. An X
compression relative to Y is therefore consistent with the screenshot
difference and requires no change to record-06 geometry or segment
construction.

This establishes a plausible *kind* of presentation explanation, not that
Atari applied that transform or that the screenshot ratio is the Atari ratio.
No constant was fitted or proposed from the image.

## Decision and remaining evidence

1. **Equal or unequal original visible gain?** Unresolved by local hardware
   evidence. Software-side common scaling does not settle analog gain.
2. **Where could a difference originate?** If present, it could arise after
   the software vector list in the X/Y analog output, deflection-amplifier,
   or monitor/calibration path. The exact point is not established.
3. **Fixed, nominal-adjustable, or fully adjustable?** Unresolved.
4. **Numeric nominal ratio?** None established.
5. **Does C# model an Atari relationship?** It currently selects equal gain
   (`displayAspect=1.0`) provisionally; no hardware relationship is encoded.
6. **Could that explain the Level 7 shape discrepancy?** Yes, mathematically;
   the screenshot comparison is consistent with horizontal compression.
7. **Is there enough evidence for an implementation candidate?** **No.**

The single narrowest remaining evidence gap is primary documentation of the
original Tempest AVG's X/Y output circuitry and its calibration to the
installed monitor: specifically the two DAC/integrator and deflection paths,
any independent gain/size adjustments and ranges, and the service target (if
one exists). Until that is available, a future change should remain
unimplemented; if resolved, the technically smallest correction belongs at
the final vector-to-display mapping, not in record geometry or connectivity.

## Repository safety and completion

Initial repository state recorded before investigation:

- Branch: `master`; `HEAD`: `385f603a94391756b2ca6cccd524dc17b281e782`;
  upstream: `origin/master`.
- Index: clean.
- Worktree already contained 17 tracked modifications and 13 non-ignored
  untracked paths, including the untracked direct-Atari presentation files,
  Atari reference documents, and `screenshot000.png`. All were pre-existing
  and preserved.

No source, tests, configuration, durable documentation, geometry, projection,
viewport, presentation code, gameplay, protected material, or Git state was
changed. No build, test, or interactive launch was warranted for this
read-only investigation. The required ignored AI-Logs files are the only
created files:

- Prompt:
  `AI-Logs/Prompts/2026-11-16-atari-dvg-visible-xy-aspect-gain-readonly-investigation-prompt.md`
  (copied from the supplied task and verified against its SHA-256).
- Report:
  `AI-Logs/Reports/2026-11-16-atari-dvg-visible-xy-aspect-gain-readonly-investigation-report.md`.

**Completion token:** `TEMPEST_DVG_VISIBLE_XY_ASPECT_GAIN_READONLY_INVESTIGATION`
