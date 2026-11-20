# DVG / AVG Vector Orientation and Units — Read-Only Investigation

**Date:** 2026-11-15
**Status:** READ-ONLY INVESTIGATION COMPLETE
**Primary classification:** **VECTOR ORIENTATION GAP REMAINS**

## Summary

The coordinate routine feeds a Tempest-specific **AVG**, not a DVG. The
Atari disassembly documents the exact 6502-side packing and labels the
resulting VCTR coordinates as signed 13-bit values. The routine appends a
fixed `vscale b=1 l=0`, a `vcentre`, and then a `vldraw`; therefore each
coordinate result is a centered VCTR displacement, not a standalone absolute
beam address.

The ROM-derived vector-list annotations provide useful signed numeric
examples. MAME independently models a Tempest AVG, swaps its internal
center-relative output axes, and marks Tempest `ROT270`; its vector renderer
then normalizes points against an emulator-defined visible area. These facts
are consistent with portrait output, but they do not prove the physical
monitor's direction/polarity or supply an Atari-native analog-to-screen
transform. The original vector-generator schematic and monitor deflection
documentation were not available in the consulted Atari text. Consequently,
no exact, hardware-backed final portrait transform is asserted.

No C# projection was implemented or changed.

## Starting Git state

The bounded pre-research check found:

- Branch: `master`.
- `HEAD` and `origin/master`: `385f603a94391756b2ca6cccd524dc17b281e782`.
- Index: clean.
- Tracked worktree: exactly the expected 16 modified files.
- Non-ignored untracked files: exactly the expected
  `src/Tempest.Game/Levels/RomPresentationMetadata.cs` and
  `tests/Tempest.Game.Tests/RomPresentationMetadataTests.cs`.
- Diff summary: 16 files, 2,021 insertions, 127 deletions.
- No unrelated work was observed.

The starting state matched the task's expected candidate state.

The substantive task was copied byte-for-byte to
`AI-Logs/Prompts/2026-11-15-dvg-vector-orientation-units-readonly-prompt.md`.
The source attachment and saved prompt SHA-256 both equal
`9382A58CF2EFAA5733A1C6AA207E624D0E3B5D1D47BE62C9A7026F2CD41DBDAF`.

## Evidence inspected and limits

### Project-local evidence

- `AI-Logs/Reports/2026-11-15-external-mathbox-hardware-prom-decode-report.md`
  for the already-verified transfer, source pipeline, and Level 1/4/7/8/9
  synthetic examples. The PROMs were not re-decoded.
- `AI-Logs/Reports/2026-11-15-mathbox-dvg-coordinate-translation-readonly-report.md`
  for the preceding source-side coordinate interpretation.
- `Reference/arcade/Tempest Rom Source code.md`, bounded to:
  - the `$C098`-based coordinate pipeline and its caller;
  - the `vapp_to(X)` comment and byte emission at `$C765-$C79D`;
  - the vector-list examples at `$3260-$33A6`.
- `src/Tempest.Game/Rendering/Projection.cs` and
  `src/Tempest.Game/Rendering/DisplayConfiguration.cs` for comparing the
  current presentation; neither is Atari authority.
- Targeted searches for the `--portrait-perspective` integration points.

The Atari source explicitly calls Tempest a color X-Y game and identifies a
Tempest Analog Vector-Generator PCB. The consulted text of Atari service
manual TM-190 does not state the monitor's physical rotation, the polarity of
the X/Y deflection inputs, or an exact vector-unit-to-screen mapping. Its
description of accompanying schematics/monitor material does not supply
those missing details in the OCR text consulted.

### External cross-checks

- Atari, *Tempest Operation, Maintenance and Service Manual*, TM-190 (1981),
  [Internet Archive item](https://archive.org/details/atari-tempest-tm-190-service-manual)
  and OCR text. Used only for the documented color X-Y monitor and
  Tempest-specific analog vector-generator PCB nomenclature.
- Pinned MAME DVG/AVG implementation:
  [`avgdvg.cpp`](https://github.com/mamedev/mame/blob/77999d3dd53e7aa1d80d0657feda8ca2279cbd3b/src/devices/video/avgdvg.cpp)
  and
  [`avgdvg.h`](https://github.com/mamedev/mame/blob/77999d3dd53e7aa1d80d0657feda8ca2279cbd3b/src/devices/video/avgdvg.h).
- Pinned MAME Tempest driver:
  [`tempest.cpp`](https://github.com/mamedev/mame/blob/77999d3dd53e7aa1d80d0657feda8ca2279cbd3b/src/mame/atari/tempest.cpp).
- Pinned MAME vector rendering:
  [`vector.cpp`](https://github.com/mamedev/mame/blob/77999d3dd53e7aa1d80d0657feda8ca2279cbd3b/src/emu/vector.cpp)
  and
  [`render.cpp`](https://github.com/mamedev/mame/blob/77999d3dd53e7aa1d80d0657feda8ca2279cbd3b/src/emu/render.cpp).

MAME is treated as an independent behavioral cross-check, not Atari
authority. Its normalized viewport and host output are not used as Atari
physical units. No direct original AVG schematic page was obtained.

No raw ROM or PROM bytes were opened, hashed, enumerated, or changed. No
screenshots, gameplay recordings, other visual files, Gemini output, or
Incitti material were inspected. `TEMP/myNotes.md` was not read.

## Hardware terminology

**ROM-BACKED / SOURCE-BACKED:** The Atari vector-list routine emits the
instruction forms `vscale`, `vcentre`, and `vldraw`. Its `vldraw` is a
two-coordinate vector instruction. The local Atari disassembly calls the
hardware an analog vector generator.

**REFERENCE-DERIVED:** MAME registers Tempest as `AVG_TEMPEST`, a
Tempest-specific subclass of its AVG implementation; MAME explicitly
distinguishes AVG behavior from DVG behavior. The Tempest driver instantiates
`AVG_TEMPEST`, not `DVG`. The CPU-produced coordinates in this path therefore
feed the Tempest AVG's VCTR interpretation, not the DVG instruction format.

Conclusion: the accurate terminology for this path is **Tempest-specific
AVG**. “DVG” is not interchangeable with AVG here. This conclusion is
consistent with Atari's board naming and the Atari command list, while the
detailed instruction interpretation below is cross-checked against MAME.

## Relevant instruction format and coordinate representation

The coordinate routine's `vapp_to(X)` helper appends:

1. `vscale b=1 l=0` (one 16-bit instruction);
2. `vcentre` (one 16-bit instruction);
3. `vldraw` (two 16-bit words: one signed X coordinate and one signed Y
   coordinate).

Thus the movement/draw instruction consumes four bytes/two words. The source
comment describes a four-byte input block `AA BB CC DD`, with the resulting
coordinates `X=DDCC`, `Y=BBAA`, each interpreted as a signed 13-bit number;
the helper masks away the upper three result bits while emitting the words.

MAME's AVG byte handlers cross-check the format: the first VCTR coordinate
is assembled in `m_dvy`, the second in `m_dvx`; bit 12 is the coordinate sign
bit, while the high instruction bits carry operation/intensity state. For
VCTR, operation state selects vector movement; those high bits are not extra
coordinate magnitude bits. MAME's Tempest-specific intensity/color handling
does not change coordinate decoding.

The signed encoding is equivalent to 13-bit two's complement:

```text
signed13(word) = (word & 0x1fff) >= 0x1000
    ? (word & 0x1fff) - 0x2000
    : (word & 0x1fff)
```

Therefore the representable VCTR coordinate range is **-4096 through +4095**.
The sign bit is coordinate bit 12; bits 15–13 belong to instruction and
intensity/opcode interpretation and are not part of the coordinate. The Atari
helper's explicit high-bit mask and MAME's field assembly agree. The
coordinate routine is not a DVG signed-magnitude format.

`vldraw` coordinates are **movement/displacement operands**, not absolute
positions. The AVG maintains a current beam position. The preceding
`vcentre` resets that position to the vector-generator center, making this
particular routine's displacement originate at the center. A `z=off` vector
in the disassembly changes position without requesting a visible stroke;
intensity affects visibility, not signed coordinate interpretation.

`vscale b=1 l=0` is part of this path's state. MAME separates binary and
linear scale state and uses normalization/timing in its AVG simulation. The
coordinate word itself remains the stable integer logical operand; the
analog deflection and elapsed vector timing are not a host-pixel unit.

## Origin and current-position semantics

The Atari helper emits `vcentre` immediately before each projected VCTR. MAME
implements the AVG center operation by setting both current positions to its
configured centers. Thus the source-supported logical origin for these
projection results is **the vector center**:

```text
Atari centered logical origin: (0, 0)
current beam after vcentre:    (xcenter, ycenter)
VCTR operand:                  displacement from that center
```

No numeric Atari analog center voltage or exact physical beam-center
coordinate follows from these facts. In the coordinate routine,
`callerBaseX` is added to the source-X-derived projection result; that result
is serialized as vector Y. It is an input to the displacement, not an
independent hardware origin or a reason to translate by a host-screen
constant.

## Axis ordering and mapping stages

The established CPU-side outputs remain unchanged:

```text
sourceYResult = signed lev_y2d + sign(relativeY) * M(abs(relativeY), depth)
sourceXResult = callerBaseX      + sign(relativeX) * M(abs(relativeX), depth)

Vx_word = signed13(sourceYResult)  // serialized as vector X
Vy_word = signed13(sourceXResult)  // serialized as vector Y
```

This is the Atari routine's serialization order. MAME's AVG parser reads the
VCTR Y field and X field in its hardware instruction order, then its
Tempest-specific output handler transposes the centered AVG position:

```text
AVG internal movement:  x += displacementX
                        y -= displacementY

Tempest AVG output:     x_out = y - ycenter + xcenter
                        y_out = x - xcenter + ycenter
```

Ignoring the scale/time factor, this makes the MAME Tempest device's
pre-render centered output move in directions proportional to
`(-Vy_word, +Vx_word)`. MAME then applies Tempest's `ROT270` system
orientation and its vector-buffer/render transforms. Its `vector_device`
normalizes emitted points against the driver's emulator-defined visible
area. These are distinct stages; the latter normalization and rotation are
not Atari analog constants.

The local Atari code establishes the source-to-word axis swap. The MAME
Tempest callback establishes an additional swap in its emulated hardware
output. MAME's driver-level `ROT270` produces portrait-oriented emulator
output. This supports an intentional portrait-related axis treatment, but
the consulted Atari primary text does not state the physical X/Y coil
polarity or the precise composition of the board and cabinet rotations.
Accordingly, the exact final visible left/right and up/down signs remain
unproved as Atari behavior; the source-to-word serialization must not be
mistaken for that final mapping.

### Physical display orientation

Atari TM-190 describes Tempest as a color X-Y game and identifies its analog
vector-generator PCB. It does not establish a numeric rotation, screen
quadrant, or positive-deflection direction in the consulted text.

MAME's Tempest game registrations use `ROT270`. Its `avg_tempest_device`
swaps output axes around center, and `render.cpp` has Tempest-specific
vector-buffer orientation handling. This is a credible behavioral
cross-check for portrait output, not proof that the physical monitor's
deflection coils or cabinet wiring use the same sign convention. No
screen-coordinate transform is promoted from MAME to Atari authority.

### Coordinate range, analog visibility, and vector units

- **Word range:** each emitted coordinate is a signed 13-bit operand,
  `[-4096, 4095]`.
- **Current position:** the beam position is accumulated from movements and
  reset to center by `vcentre`; it is not itself restricted to that one
  operand range.
- **Scale:** the coordinate helper explicitly sets `b=1, l=0` before the
  center and VCTR command. MAME's AVG uses scale and vector timing in
  interpreting displacement. The original analog integrator/DAC transfer
  and its exact physical amplitude per word are not established by the
  retrieved Atari service text.
- **Clipping:** the Atari source examined does not digitally clamp these
  VCTR endpoints to a modern viewport. MAME's vector renderer clips or
  normalizes for its own visible area; that is emulator presentation, not
  proof of hardware clipping. The exact physical deflection/monitor-visible
  bounds and analog limiting behavior remain **UNKNOWN** without the
  deflection schematic and monitor documentation.
- **Unit:** one integer is one encoded VCTR operand unit in the fixed
  `vscale` command context. It is useful as a deterministic, centered
  logical vector unit. It is not one pixel, a fixed number of millimeters,
  or a proven constant DAC-voltage step. The analog display's physical
  mapping cannot be represented by an evidence-backed exact pixel
  equivalent from the inspected sources.

## Numeric vector-list oracle

The Atari ROM disassembly provides a small independent encoded-list oracle;
its comments annotate the words and expected logical signed coordinates.
Examples include:

| Atari list data | Disassembly interpretation | Use |
|---|---|---|
| `0000003c` | `vldraw x=+60 y=+0 z=off` | Positive X and zero Y |
| `01004000` | `vldraw x=+0 y=+256 z=4` | Positive Y movement, lit |
| `1f000010` | `vldraw x=+16 y=-256 z=off` | Positive X and negative Y |

The surrounding Atari source labels the repeated `x=0, y=+256` strokes as
“Set of 6 vertical lines (used in a selftest display).” That is an
independent axis/orientation check at the ROM-list level. It does not supply
the vector-to-host-pixel scale or prove physical coil polarity.

MAME's AVG field assembly and Tempest output code are an independent
implementation cross-check for the same instruction family. The MAME driver
selects `AVG_TEMPEST`, declares its own visible area `(0..580, 0..570)`, and
registers Tempest with `ROT270`. MAME's vector renderer then normalizes to
its visible-area dimensions. The ROM word annotations and MAME's decode
agree on the signed logical coordinates; the emulator's 580-by-570 area is
not adopted as hardware truth.

The available oracle is sufficient to test instruction decoding and
center-relative movement at the logical word stage. It does not independently
validate a final Atari-to-canonical-portrait sign/rotation matrix because
that physical transform remains unresolved.

## Representative Math Box examples

The following inputs and Math Box outputs are copied from the preceding
verified report. They are synthetic, use `(pointX, pointY, pointDepth) =
(160,128,40)` and `callerBaseX=0`, and are not vector-ROM points or visual
acceptance. `Vx` and `Vy` below are the signed 13-bit operands in the order
established by Atari `vapp_to`. The last column is the MAME Tempest
device's **pre-render direction pair** `(-Vy,+Vx)` with scale/time magnitude
omitted; it is not a final physical or host coordinate.

| Level | VCTR `X=Vx` | VCTR `Y=Vy` | Signed 13-bit words | MAME pre-render direction `(-Vy,+Vx)` |
|---:|---:|---:|---|---|
| 1 | -96 | +64 | `$1FA0`, `$0040` | (-64, -96) |
| 4 | -73 | +74 | `$1FB7`, `$004A` | (-74, -73) |
| 7 | -96 | +64 | `$1FA0`, `$0040` | (-64, -96) |
| 8 | 0 | +64 | `$0000`, `$0040` | (-64, 0) |
| 9 | +202 | +78 | `$00CA`, `$004E` | (-78, +202) |

The exact canonical portrait coordinates cannot be reported from these
examples without asserting an unverified physical orientation/sign map. The
table deliberately stops at signed vector operands and the explicitly
identified MAME pre-render stage.

## Canonical portrait space and exact-transformation status

The evidence supports this **native vector-word space**, but not yet a
fully verified physical portrait space:

```text
coordinates: signed integer VCTR words (Vx, Vy)
origin:      (0, 0), established by vcentre immediately before the VCTR
range:       each word [-4096, +4095]
units:       encoded vector-word units under the emitted vscale b=1, l=0
```

The exact verified mapping stops at:

```text
Atari source components
  -> verified Math Box transfer and CPU sign/offset restoration
  -> Vx = signed13(sourceYResult)
     Vy = signed13(sourceXResult)
  -> center-relative VCTR displacement (Vx, Vy)
```

The MAME cross-check adds a model-specific, scaled, centered stage:

```text
MAME pre-render relative position is proportional to (-Vy, +Vx)
  -> Tempest ROT270 / vector-buffer transforms
  -> MAME visible-area normalization
```

This is not enough to assert an Atari-native final mapping such as
`canonicalX = +/- Vx`, `canonicalY = +/- Vy`, or any equivalent fixed
rotation/translation. No translation constant is justified beyond the
center reset, and no C# viewport dimensions or fitting constants belong in
this stage. The canonical portrait transform remains blocked on primary
evidence for physical axis polarity/rotation.

## Current C# comparison and presentation separation

The current C# projection remains a separate host-presentation path:

- `Projection.Project` returns coordinates in the legacy logical
  `800x600` space and applies the current perspective/placement equation.
- The optional `--portrait-perspective` value is an endpoint-normalized
  project calibration; it is not Atari Math Box state.
- `DisplayConfiguration` has a `600x800` portrait HUD space and a
  `720x960` portrait viewport in the existing host window. Those are modern
  project presentation dimensions, not Atari vector units.

A future implementation needs a distinct canonical Atari vector layer, then
one explicit final transform into the current C# portrait presentation.
Atari projection must not be changed merely to fit those dimensions.

Conceptually, `--portrait-perspective` belongs only as an optional
project-specific calibration after an established Atari projection and
canonical mapping; it must not be folded into ROM metadata or the Math Box
equation. It may become unnecessary if the verified Atari path reproduces
the accepted presentation. The present orientation gap prevents deciding
that by implementation or visual comparison.

## Gameplay geometry

No evidence requires changing lanes, `LevelDefinition.Points`, collision
geometry, or enemy/player paths. The verified coordinate work is in the
presentation/vector-list path. Preserve gameplay geometry; keep any future
Atari projection render-only unless direct evidence establishes otherwise.

## Minimum future automated oracle

Before active implementation, the smallest useful deterministic test oracle
should assert:

1. Each selected raw Rev 2 metadata tuple and synthetic model point forms the
   expected depth and relative coordinates.
2. The verified Math Box result is exactly
   `floor(abs(magnitude)*128/depth)` for positive depth.
3. CPU-side signs, signed `lev_y2d`, `callerBaseX`, and 13-bit truncation
   produce the expected `Vx` and `Vy` words for positive, negative, zero,
   and representable boundary cases.
4. `vscale`, `vcentre`, and VCTR emission preserve the documented state and
   byte/word order; the center reset makes the logical origin exactly zero.
5. Known Atari vector-list inputs decode to the annotated examples above,
   including `+256`, `-256`, and a zero component.
6. The eventual physical/canonical orientation matrix is tested against an
   independent Atari diagnostic/list oracle, not only against its own code.

Items 1–5 can be exact integer invariants. The physical transfer to analog
monitor coordinates, monitor-visible clipping, and final modern viewport fit
are not established by the existing numeric list oracle. A final C# fit
remains a separate presentation decision and requires Neal's manual
acceptance.

## Unresolved evidence and smallest next milestone

The narrow remaining gap is **primary evidence for the physical
Tempest-AVG-to-portrait axis signs and rotation**, including the exact
deflection-output polarity and the monitor's physical mounting orientation.
MAME's `AVG_TEMPEST` logic, `ROT270`, and the Atari self-test list are strong
cross-checks, but do not replace the missing Atari schematic/monitor
deflection documentation for a hardware-backed canonical mapping.

The smallest next milestone is a read-only acquisition/inspection of the
original Tempest Analog Vector-Generator schematic and matching monitor
deflection documentation, limited to:

- VCTR X/Y DAC/integrator paths and sign polarity;
- the center command's analog reference;
- board-to-monitor deflection axis wiring and cabinet rotation;
- any scale/clamp/visible-deflection limits needed to define logical range.

Then cross-check that derived transform against the already identified
Atari self-test vector-list words. Do not start C# projection work until the
result resolves the orientation/sign gap.

## Final state and protections

- Branch remains `master`; `HEAD` and `origin/master` remain
  `385f603a94391756b2ca6cccd524dc17b281e782`.
- Index remains clean.
- The same 16 tracked candidate files remain modified.
- The same two candidate metadata/test files remain the only non-ignored
  untracked files.
- The only created repository-local files are this ignored report and the
  ignored exact prompt copy.
- No production source, tests, metadata, durable behavior/development
  documentation, ROM/PROM/reference files, or candidate work were modified.
- Nothing was staged, committed, or pushed.
- No visual files, Gemini material, or Incitti material were inspected.
- No build, test suite, or game was run; this was a bounded read-only source
  and reference investigation.
