# Mathbox / DVG Coordinate Translation — Read-Only Investigation

**Date:** 2026-11-15
**Status:** READ-ONLY INVESTIGATION COMPLETE
**Primary classification:** **MATHBOX MICROCODE DECODING REQUIRED**

## Scope and starting Git state

This investigation addresses whether current project-local evidence supports a
numerically faithful translation of Rev 2 `lev_scale`, `lev_y3d`, and signed
`lev_y2d` through the Atari mathbox/vector-list path into a modern projection.
No implementation or presentation/gameplay changes were made.

Starting state:

- Branch: `master`; upstream: `origin/master`.
- `HEAD` and `origin/master`: `385f603a94391756b2ca6cccd524dc17b281e782`.
- Index: clean.
- Tracked worktree: the 16 modified presentation/debug candidate files
  documented by the metadata-foundation report; no unrelated tracked changes.
- Non-ignored untracked files: the authorized
  `src/Tempest.Game/Levels/RomPresentationMetadata.cs` and
  `tests/Tempest.Game.Tests/RomPresentationMetadataTests.cs`.
- The existing candidate diff was 16 files, 2,021 insertions, and 127
  deletions. The observed paths and counts matched the expected state.

The task prompt was copied verbatim to
`AI-Logs/Prompts/2026-11-15-mathbox-dvg-coordinate-translation-readonly-prompt.md`;
the saved copy's SHA-256 matched the read-only attachment.

## Evidence inspected and limits

Read:

- `AI-Logs/Reports/2026-11-14-rev2-presentation-table-extraction-report.md`
  for Rev 2 table values and displayed-level mapping.
- `AI-Logs/Reports/2026-11-14-rev2-presentation-mathematical-translation-report.md`
  for the prior CPU-side pipeline analysis and unresolved mathbox/DVG questions.
- `AI-Logs/Reports/2026-11-14-rom-presentation-metadata-foundation-report.md`
  for the current inert C# metadata model and expected Git state.
- `Reference/arcade/Tempest Rom Source code.md`, bounded to the coordinate
  routine at `$C098-$C16D`, its caller at `$BD09`, `vapp_to(X)` at
  `$C765-$C79D`, the unrelated `divide` routine at `$DCE6-$DD0C`, and the
  nearby vector-center emission helper.
- `src/Tempest.Game/Levels/RomPresentationMetadata.cs` and
  `src/Tempest.Game/Rendering/Projection.cs`, solely for metadata completeness
  and current-equation comparison.
- Targeted searches of the project source/tests and local reports for mathbox,
  PROM, vector-coordinate, and numeric-output evidence.

No raw ROM or PROM bytes were read; no ROM directory contents were enumerated.
No gameplay tables, screenshots, images, videos, MAME/C# frames, recordings,
Gemini material, Incitti implementation, or web sources were inspected.
`TEMP/myNotes.md` was not read.

The annotated disassembly provides the CPU-facing register transactions and
vector-list packing, but no mathbox PROM sequence or decoded microcode for this
coordinate operation. The separate `$DCE6` routine is annotated as appearing
to divide, with its author explicitly uncertain about mathbox details; it is
not shown as the coordinate routine's mathbox implementation and is not used
here to infer the transfer.

## Authoritative metadata and sufficiency

The existing catalog preserves every raw per-level input identified for this
translation: displayed level, remapped tube-table index, raw `lev_scale`,
raw `lev_y3d`, both raw `lev_y2d` bytes, and raw `lev_open`. The combined
little-endian unsigned and signed 16-bit Y2D interpretations are derived from
the preserved bytes. **The metadata is sufficient as-is for the identified
raw fields; no required source field is missing.**

There is a naming/representation caveat, not a missing-data defect:
`ScreenYOffset*` can imply a host screen-Y quantity, while the disassembly
applies `lev_y2d` to the mathbox component derived from source Y and later
serializes that component as vector X. Do not reinterpret or rename the
existing inert metadata in this investigation.

| Displayed level | Tube index | `lev_scale` | `lev_y3d` | Signed `lev_y2d` | `lev_open` |
|---:|---:|---:|---:|---:|---|
| 1 | 0 | `0x18` (24) | `0x50` (80) | -192 | closed |
| 4 | 3 | `0x0F` (15) | `0x68` (104) | -128 | closed |
| 7 | 6 | `0x18` (24) | `0x68` (104) | -144 | closed |
| 8 | 7 | `0x18` (24) | `0xB0` (176) | +96 | open |
| 9 | 13 | `0x0C` (12) | `0xB0` (176) | +320 | open |

These are ROM-backed values from the authoritative extraction report; they
were not re-extracted or reinterpreted here.

## CPU-side coordinate routine

**SOURCE-BACKED:** `$C098` is the CPU routine called by the tube-point drawing
caller. For a point with source components `(pointX, pointY, pointDepth)`, it
forms:

```text
D  = pointDepth + lev_scale
dx = pointX - 0x80
dy = pointY - lev_y3d
```

The routine writes the low/high bytes of `D` to mathbox registers
`mb_w_15`/`mb_w_16`, and writes each unsigned coordinate magnitude to both
`mb_w_0e` and `mb_w_14`. It keeps each coordinate sign in CPU memory, waits
for the mathbox-busy status, and reads the two-byte result from `mb_rd_l` /
`mb_rd_h`. The source-Y request is read to `$63/$64`; the source-X request is
read to `$61/$62`. Thus the requests use the same CPU-visible register path
and depth input.

The routine clamps a negative formed depth to one. For the production
`lev_scale` values and a nonnegative byte `pointDepth`, `D` is positive
(minimum 12 for these levels); a zero denominator is not reached through this
input formation. The mathbox's behavior at zero or near-zero input is not
documented by the inspected source.

CPU-side signed restoration and placement are visible:

```text
mathX = sign(dx) * M(abs(dx), D)
mathY = sign(dy) * M(abs(dy), D)
sourceXResult = callerBaseX + mathX
sourceYResult = signed_16(lev_y2d) + mathY
```

`M` is an unknown mathbox transfer, not an asserted division formula. The
`lev_y2d` addition/subtraction is performed after the first mathbox response;
the CPU clamps signed overflow to `0x7FFF` or `0x8000`. This is CPU-side
post-projection saturation and does not establish mathbox saturation.

### Mathbox operation and arithmetic details

The CPU call site and operand/result register groups are identified, but an
exact mathbox command/entry point or PROM sequence is **not** established by
the local annotated text. In particular, `$C098` does not expose a decoded
microprogram or the internal transfer; a CPU routine address is not a
mathbox-microcode address.

| Property | Finding |
|---|---|
| CPU-side operation | `$C098` issues two magnitude/depth requests through `mb_w_0e`, `mb_w_14`, `mb_w_15`, and `mb_w_16`. |
| CPU-visible operand widths | Each magnitude is written as an 8-bit byte; depth is supplied as two bytes. Internal mathbox operand widths are unknown. |
| CPU-visible result width | Two bytes are read per result and treated by CPU sign-restoration code as a magnitude. Internal result width/format is unknown. |
| Transfer / numerator-denominator order | Unknown. The symbolic `M(magnitude,D)` boundary is retained. |
| Signedness inside the mathbox | Unknown; signs are handled separately by the CPU coordinate routine. |
| Fixed-point radix / coefficient | Unknown. |
| Rounding or truncation inside the mathbox | Unknown. |
| Internal saturation and output range | Unknown. CPU-side Y2D composition saturation is known; it is not mathbox saturation. |
| Zero / near-zero transfer behavior | Unknown. Valid production inputs yield `D >= lev_scale > 0`. |
| Same transfer for X and Y | Both requests use the same routine, operand registers, and depth input; exact microcode equivalence is not independently decoded. |
| Embedded aspect correction | Unknown. |

The evidence does **not** prove `K * magnitude / D`, a reciprocal table,
integer division, multiplication order, or any particular fixed-point
equation. No known numeric source-input/mathbox-output pairs or mathbox
decoder/tests were found in the inspected project source, tests, and reports.

## DVG/vector-list translation and orientation

**SOURCE-BACKED:** `vapp_to(X)` documents the four-byte block as
`AA BB CC DD`, then emits:

```text
vector X = DDCC = result from $63/$64 (source-Y mathbox result + lev_y2d)
vector Y = BBAA = result from $61/$62 (source-X mathbox result + caller base)
```

Each serialized coordinate is interpreted as signed 13-bit, with the high
three bits dropped. This implies representable signed values `-4096..4095`
after serialization; the source does not establish a clip or saturation rule
at that boundary. The CPU-side `$C098` additions do independently saturate
signed 16-bit overflow as described above.

`vapp_to(X)` emits a vector-scale command, `vcentre`, and then `vldraw`.
This establishes that the center opcode is placed in the list before the
draw, but the inspected local text does not numerically define the vector
origin, coordinate directions/signs, physical extents, or display clipping.
The byte/word swap is a **SOURCE-BACKED serialization order**. Whether that
order is only a software packing convention or corresponds to a physical
monitor rotation is **UNKNOWN** from these materials.

The local evidence therefore distinguishes:

- **Mathbox/source components:** source-Y relative component and source-X
  relative component are formed independently.
- **DVG serialization:** source-Y-derived component becomes vector X;
  source-X-derived component becomes vector Y.
- **Vector origin and direction:** only a `vcentre` opcode emission is
  established; numerical origin and positive-axis directions remain unknown.
- **Physical portrait orientation:** not established by the inspected local
  source or reports. No monitor-rotation convention is assumed.
- **Host coordinates:** no exact conversion to C# logical pixels or the
  current screen-down axis is established.

No numeric vector-list output paired with known model/depth inputs was found.
The minimum future calibration evidence is a numeric DVG/vector coordinate
reference tied to known physical center and independent positive-axis
directions/scales, sufficient to resolve origin, axis swap/sign, and units
before applying arbitrary viewport scaling.

## Raw-field numerical roles

### `lev_scale`

**SOURCE-BACKED:** The actual depth input is `D = pointDepth + lev_scale`.
For identical source points, the table values produce `d+24` (Levels 1, 7,
and 8), `d+15` (Level 4), and `d+12` (Level 9). A smaller byte therefore
reduces the depth-like input; it is not shown multiplying model coordinates
as a modern scale factor.

If `M` is reciprocal/ratio-like, the smaller value would increase relative
coordinate response at a fixed depth, with a nonuniform depth effect. That
direction remains a **conditional mathematical interpretation**, not an
established numeric result, because `M` is unknown.

### `lev_y3d`

**SOURCE-BACKED:** The input is `dy = pointY - lev_y3d`. At the same point,
the representative references are 80, 104, 104, 176, and 176 source-byte
units for Levels 1, 4, 7, 8, and 9. Increasing the field moves the relative
input downward by that byte difference; it changes the Y-derived mathbox
component before projection. It is not in the depth input and is not
established as a direct C# `YOffset`. After serialization, the affected
component is vector X.

### `lev_y2d`

**SOURCE-BACKED:** This signed 16-bit value is applied after the
Y-derived mathbox result and before vector serialization. It shifts that
component by the raw signed amount (subject to CPU signed-16 saturation),
independent of point depth. The affected component is serialized as vector
X, not proven C# screen Y. The requested examples correspond to shifts
`-224`, `-192`, `-144`, `-128`, `+96`, and `+320` in the source/mathbox
result word; their pixel magnitudes and screen directions are unknown.

## Representative symbolic examples

No matching ROM-unit model points and mathbox outputs are available, so exact
numeric vector or C# examples cannot be responsibly produced. The following
worked forms use the same symbolic point `(x,y,d)` and keep the unknown
transfer explicit. Let `sx = sign(x-128)`, `sy = sign(y-y3d)`, and let
`sat16` denote the source's signed-16 overflow handling.

| Level | ROM metadata and formed inputs | Post-mathbox result and serialized coordinates |
|---:|---|---|
| 1 | `D=d+24`; `dy=y-80`; `dx=x-128` | `Vx16=sat16(-192 + sy*M(|dy|,D))`; `Vy16=baseX + sx*M(|dx|,D)` |
| 4 | `D=d+15`; `dy=y-104`; `dx=x-128` | `Vx16=sat16(-128 + sy*M(|dy|,D))`; `Vy16=baseX + sx*M(|dx|,D)` |
| 7 | `D=d+24`; `dy=y-104`; `dx=x-128` | `Vx16=sat16(-144 + sy*M(|dy|,D))`; `Vy16=baseX + sx*M(|dx|,D)` |
| 8 | `D=d+24`; `dy=y-176`; `dx=x-128` | `Vx16=sat16(+96 + sy*M(|dy|,D))`; `Vy16=baseX + sx*M(|dx|,D)` |
| 9 | `D=d+12`; `dy=y-176`; `dx=x-128` | `Vx16=sat16(+320 + sy*M(|dy|,D))`; `Vy16=baseX + sx*M(|dx|,D)` |

For each row, the emitted coordinate words are `Vx = truncate13(Vx16)` and
`Vy = truncate13(Vy16)`, where the local source comment says the high three
bits are dropped. `baseX` is the caller-provided `$66/$67` base; this
investigation does not substitute a C# point or assume that base is zero.
No host/C# coordinate is computable without `M`, source-to-vector units, and
orientation.

## Level 1 versus Level 9

**ROM-BACKED:** Level 1 uses `(scale=24, y3d=80, y2d=-192)`, and Level 9
uses `(scale=12, y3d=176, y2d=+320)`.

**MATHEMATICAL DERIVATION:** For the same point, Level 9 has a 12-unit
smaller depth adjustment, a 96-unit higher source-Y reference, and a
512-unit more positive post-projection offset. The depth and camera reference
affect projected coordinate responses; only the offset is a point-independent
translation of the source-Y-derived component. The likely apparent-size or
convergence difference depends on the still-unknown transfer. No numerical
pixel-size claim follows.

**MANUAL OBSERVATION ONLY:** Approximately `--portrait-perspective 125`
appears about right for Level 9; Level 8 may accept similar strength but has
a separate unresolved presentation problem; Levels 1–7 have fundamentally
wrong base perspective that the diagnostic coefficient does not fix.
Gameplay remains correct, extreme diagnostic values distort presentation
without changing gameplay, and Neal reported no clipping during diagnostic
exploration. These observations were not used to fit `M`, are not ROM values,
and do not establish a production default.

## Compatibility, gameplay, architecture, and oracle

### Current C# equation

The current `Projection.Project` has the broad form
`center + (coordinate + offset) * K / (z + ZOffset)`, with separate X/Y
placement and an endpoint-normalized portrait correction. The Atari pipeline
is symbolically a signed magnitude transfer `M(magnitude, pointDepth +
lev_scale)`, followed by CPU sign restoration and signed placement. Without
the transfer, units, and vector orientation, compatibility is **D — still
indeterminate** among the requested equation classifications. `K` and
`ZOffset` are not established conversions of ROM fields. `CenterY` and
`YOffset` are only stage analogues, not proven Atari values.

Do not wire metadata into `Projection.Project`, `VectorRenderer`, rendering,
transitions, gameplay, or the portrait diagnostic; do not change any frozen
constants or behavior in this milestone.

### Gameplay geometry protection

No evidence requires changing lanes, topology, `LevelDefinition.Points`,
collision geometry, or enemy/player paths. The identified fields enter the
presentation-coordinate path. Preserve gameplay geometry; do not modify it
without separate evidence of an import error.

### Smallest future architecture (conceptual only)

After mathbox arithmetic and vector units/orientation are established, the
smallest supportable direction is one shared rendering-side path:

```text
raw per-level ROM metadata
-> verified Atari-compatible mathbox/projection
-> canonical signed vector coordinates
-> explicit portrait/host viewport mapping
```

The eventual consumer would be the shared projection/rendering path
(`Projection` and its `VectorRenderer` caller), not level topology or gameplay
simulation. Keep the existing path available as the legacy/default mode and
gate any future candidate through an explicit diagnostic selector. Do not
layer endpoint-normalized perspective onto the Atari path until a comparison
shows it is independently needed. Whether `CenterY`/`YOffset` remain as
legacy values and whether `K`/`ZOffset` remain solely for zoom transitions
must be decided in a separately authorized implementation design; no such
change is required by the evidence now available.

### Objective validation oracle

No objective numeric oracle currently exists in the inspected project
materials: no known model/depth-to-mathbox pairs, decoded mathbox tests, or
numeric vector-list outputs were found. Source invariants (input formation,
sign restoration, Y2D placement, and vector packing) can test structure but
cannot validate the transfer. Manual visual observations alone are
insufficient. A future implementation first needs decoded mathbox
input/output pairs, followed by numeric vector-list references and a proven
vector-to-host transform.

## Decision matrix

| Option | Evidence support | Risk / maintainability | Fidelity potential | Validation available |
|---|---|---|---|---|
| A. Current reciprocal projection with converted per-level constants | Stage similarities exist; no transfer or unit conversion proves the constants. | High fidelity risk if assumed; simple to maintain only after proof. | Unknown. | No numeric oracle. |
| B. New shared Atari-compatible projection driven by raw metadata | Source proves shared input order and per-level fields, not the mathbox function. | Best eventual shared design; implementation now would encode an unknown. | High potential after decoding. | Structural invariants only. |
| C. Canonical Atari vector space then portrait mapping | DVG packing order and signed-13 serialization are known; transfer, vector units, origin, and orientation are not. | Clean boundary, but canonical values cannot yet be generated. | High potential after both gaps close. | No numeric vector oracle. |
| D. Keep inert metadata and current manually calibrated presentation | Fully supported as the safe present state. | Low risk; preserves accepted behavior. | Does not complete Atari presentation translation. | Existing gameplay/presentation behavior remains; no Atari numeric oracle. |
| E. Per-level hand tuning | No evidence supports it as an Atari translation. | High maintenance and provenance risk. | Low / unverified. | Manual comparison only, not an objective oracle. |
| F. Open/closed-specific cameras | `lev_open` does not select a camera path in the inspected routine. | Unsupported branching. | Low / speculative. | No validating evidence. |

## Classification and next milestone

**MATHBOX MICROCODE DECODING REQUIRED.**

The CPU-side coordinate preparation, sign handling, post-projection Y2D
placement, and DVG word ordering are sufficiently described to bound the next
investigation. The numerical transfer, fixed-point behavior, and corresponding
numeric test vectors are not. Do not begin an active Atari-compatible
projection from the current evidence.

Smallest next milestone: a separately authorized, narrowly bounded
mathbox/PROM investigation for only the `$C098` coordinate operation, using
an available local decoded PROM or a specifically authorized source. It must
establish the operand interpretation, transfer, quantization/overflow, and
known input/output pairs before any projection implementation. Vector-to-host
calibration/orientation remains a subsequent gap.

## Validation and final Git state

This was a read-only source/evidence investigation; no build, test suite, or
game was run. No validation beyond source/report inspection and exact prompt
copy verification was applicable.

Final state:

- Branch remains `master`; `HEAD` and `origin/master` remain
  `385f603a94391756b2ca6cccd524dc17b281e782`.
- Index remains clean.
- The same 16 tracked candidate files remain modified; the two authorized
  metadata/test files remain the only non-ignored untracked files.
- The candidate/metadata source and test files were not changed by this task.
- Only the requested ignored prompt and report were created.
- Nothing was staged, committed, or pushed.
- No prohibited visual, Gemini, Incitti, web, or raw-ROM material was
  inspected.
