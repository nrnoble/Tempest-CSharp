# Mathbox PROM Coordinate Operation Decode

**Date:** 2026-11-15
**Result:** **MATHBOX COMMAND-SELECTION GAP**
**Scope:** Bounded, read-only investigation of the Rev 2 mathbox files for the
CPU coordinate routine at `$C098`. No projection or gameplay changes.

## Summary

The seven authorized files are present in the local Rev 2 reference set. Their
sizes and SHA-256 values match the earlier project-local inventory. However,
the local evidence identifies them only as PROM/lookup devices; it does not
establish which device is the command ROM, how CPU mathbox writes address it,
or how its output selects a microprogram start. The annotated ROM listing also
does not document PROM data wiring or the mathbox control-word semantics.

The investigation therefore stopped before interpreting PROM contents,
assembling microinstructions, or executing a decoder. The exact coordinate
transfer and numeric vectors remain unknown.

## Starting Git state

- Branch: `master`; upstream: `origin/master`.
- `HEAD` and `origin/master`: `385f603a94391756b2ca6cccd524dc17b281e782`.
- Index: clean.
- Tracked worktree: the expected 16 modified candidate files; diff summary:
  2,021 insertions and 127 deletions.
- Non-ignored untracked files: the two expected metadata/test candidate files.
- No unexpected work was observed.

The substantive task prompt was copied byte-for-byte to
`AI-Logs/Prompts/2026-11-15-mathbox-prom-coordinate-operation-decode-prompt.md`.
The source attachment and saved prompt SHA-256 values match:
`AEE875AF7429D59E5E51FA00FDC7E9B5E42668C29F1C65AD6E2CE182153B80DC`.

## Authorized PROM inventory and provenance

Only the seven specified files under `Reference/arcade/roms.rev2/` were
opened, for file size and SHA-256 calculation. Their byte contents were not
dumped or decoded.

| File | Bytes | SHA-256 |
|---|---:|---|
| `136002-126.a1` | 32 | `14EFB14AC87A8DB219FB998E4CE14FA62488F1C9CFD882B41B9EB09EFFFEE373` |
| `136002-127.e1` | 256 | `2150566BF27FB038C9A19081BC6FBDAE49B3F0E932EB5328261E51A1B2CF2767` |
| `136002-128.f1` | 256 | `26CDD1D690C0033366FF3ABA5284898DD4BC3192F004EF6004EFCC17C8E0C5DF` |
| `136002-129.h1` | 256 | `67A8CF9A25938FA75FFB05E70FD4252CEA808565BF3EB778D6EA13859C82DBC8` |
| `136002-130.j1` | 256 | `243E3AA7CD48920599DD01BF810A9CE38148E036E85A836A101E27D126563622` |
| `136002-131.k1` | 256 | `68B5EC6328BC5585930F5B7A617A4B2C3FFC961634D1DAE2841B8E8E681DD80D` |
| `136002-132.l1` | 256 | `42C7C60353DDC1A12344D6DB4617D2D4FD59E805FA2967736BB78E084A8F1109` |

The project-local report
`AI-Logs/Reports/2026-11-08-level8-rev2-rom-geometry-continuation-report.md`
lists these exact sizes and hashes and identifies `126.a1` and `127.e1` through
`132.l1` as small PROM/lookup devices in the local Rev 2 set. The current
hashes match that inventory. That report found no expected checksum manifest
and states that the annotated listing does not document these devices'
hardware lookup pin/address decodes.

Thus, local provenance supports that these exact files belong to the supplied
Rev 2 reference set. It does **not** establish the requested functional
assignment (one command ROM plus six 4-bit microcode PROMs), their bit
contributions, or their address wiring. Sizes and filenames alone are not
treated as proof of those roles.

## CPU coordinate routine and command selection

The annotated listing at `$C098` establishes the CPU-visible request sequence:

1. Write the formed depth low/high bytes to `mb_w_15` / `mb_w_16`
   (`$6015` / `$6016`).
2. Write the absolute source-Y offset byte to both `mb_w_0e` and `mb_w_14`,
   wait for the busy indication to clear, and read `mb_rd_l` / `mb_rd_h`.
3. Write the absolute source-X offset byte to the same magnitude registers,
   wait, and read the result.

The CPU restores each sign separately. This establishes the coordinate
routine's operand preparation and visible register path, not its internal
mathbox command.

The local annotated listing does not state which register write or write
combination selects an entry in `136002-126.a1`, the command ROM's address
inputs, its output-to-micro-PC mapping, or the coordinate command's starting
micro-PC. The `$C098` CPU address is not evidence of a mathbox micro-PC.
Accordingly:

- Command-ROM role as `136002-126.a1`: **not verified locally**.
- Command/address inputs and command selection from the `$C098` writes:
  **unresolved**.
- Selected command and coordinate microprogram start address: **not
  determined**.

## PROM assembly and hardware model

The local sources do not establish:

- Whether each file stores one byte per address, packed nibbles, or another
  organization at the logical device outputs.
- Which PROM output bits map to which microinstruction bits, any inversion or
  permutation, or the assembled microinstruction width.
- Micro-PC width, address sequencing, branch encoding, or how the command
  output loads the start address.
- Am2901 source selection, ALU function/carry, destination and Q controls,
  shift behavior, multiply-step behavior, status/overflow interpretation, or
  the sequencer's completion/stall and output-bus rules.

The project-local disassembly itself cautions at the separate `$DCE6` routine
that the author is not clear enough on the mathbox to establish its details.
No project-local Am2901 control-word table or mathbox schematic was found in
the inspected evidence. The expected four-slice/16-bit model is therefore not
asserted as a locally verified execution model.

## Decode, transfer, and oracle status

No command entry or starting micro-PC could be tied to the `$C098` writes.
Under the task's fail-closed conditions, no PROM bytes were assigned control
meaning, no microinstruction trace or executable decoder was produced, and no
arithmetic conclusion was drawn.

| Property | Result |
|---|---|
| `M(magnitude, depthInput)` | Unknown |
| Internal operand widths and signedness | Unknown |
| Numerator/denominator roles or operation | Unknown |
| Fixed-point radix or normalization constant | Unknown |
| Rounding or truncation | Unknown |
| Output range, saturation, or overflow | Unknown |
| Zero/near-zero mathbox behavior | Unknown |
| Dependence on prior mathbox state | Unknown |
| Whether X and Y requests execute identical microcode | Not established; CPU uses the same visible register path |
| Numeric input/output vectors | None generated |

For production input formation, the prior report establishes
`depthInput = pointDepth + lev_scale`; with a nonnegative byte point depth, its
minimum is 12 for Level 9, 15 for Level 4, and 24 for Levels 1, 7, and 8.
This does not establish mathbox behavior at any of those inputs or at zero.

### Independent check

The local listing has a self-test screen routine at `$DBF7-$DC19`. It calls the
separate `$DCE6` routine and checks for `A == 1`, `Y == 0`, and a nonnegative
`X` for the state supplied from `$2E/$2F`. The listing labels `$DCE6` as
appearing to divide but explicitly expresses uncertainty about the mathbox.
This is a candidate ROM-side expected-value check, not an independently
decoded coordinate-transfer vector. It could not be reproduced because
command selection and control decoding remain unresolved. No independent
numeric cross-verification was completed.

## Coordinate examples and DVG boundary

No specific source point/depth pair and no decoded mathbox output were
established, so numeric coordinate examples are not available. The following
preserves only the known CPU-side symbolic inputs and placement for one shared
symbolic point `(x, y, d)`; it is not an output oracle. Let
`sx = sign(x - 128)`, `sy = sign(y - lev_y3d)`, and `sat16` denote the known
CPU signed-16 overflow handling after the Y2D addition.

| Level | `lev_scale` | `lev_y3d` | signed `lev_y2d` | Known symbolic input/result boundary |
|---:|---:|---:|---:|---|
| 1 | 24 | 80 | -192 | `D=d+24`; Y request `M(|y-80|,D)`; X request `M(|x-128|,D)`; source-Y-derived word is `sat16(-192 + sy*M(|y-80|,D))`. |
| 4 | 15 | 104 | -128 | `D=d+15`; Y request `M(|y-104|,D)`; X request `M(|x-128|,D)`; source-Y-derived word is `sat16(-128 + sy*M(|y-104|,D))`. |
| 7 | 24 | 104 | -144 | `D=d+24`; Y request `M(|y-104|,D)`; X request `M(|x-128|,D)`; source-Y-derived word is `sat16(-144 + sy*M(|y-104|,D))`. |
| 8 | 24 | 176 | +96 | `D=d+24`; Y request `M(|y-176|,D)`; X request `M(|x-128|,D)`; source-Y-derived word is `sat16(96 + sy*M(|y-176|,D))`. |
| 9 | 12 | 176 | +320 | `D=d+12`; Y request `M(|y-176|,D)`; X request `M(|x-128|,D)`; source-Y-derived word is `sat16(320 + sy*M(|y-176|,D))`. |

The prior investigation establishes the serialization boundary: the
source-Y-derived component is emitted as DVG vector X, the source-X-derived
component as DVG vector Y, with signed 13-bit packing. No DVG-to-host
orientation/unit conversion or C# pixel mapping is asserted here.

## Classification and smallest next milestone

**MATHBOX COMMAND-SELECTION GAP**

The narrowest blocking evidence is a local or specifically authorized Atari
Tempest Rev 2 mathbox logic schematic/control reference that ties the CPU
mathbox register-write strobes to the command-ROM address, output command, and
starting micro-PC. Once that mapping is known, the same source must establish
the six PROM output-bit order and micro-PC wiring. A locally available
Am2901/Am2901A data sheet or matching mathbox control-word table is also
required to decode ALU, carry, Q, writeback, shift, status, and sequencer
controls. No web search or external material was used.

The next milestone should remain read-only: obtain the specifically scoped
mathbox schematic/control reference, verify its revision and wiring against
the seven local PROM fingerprints, and only then decode the `$C098` command.
Keep DVG orientation/vector-unit calibration as a separate subsequent
milestone.

## Final state and protections

- The branch, `HEAD`, upstream, candidate worktree, and index were not changed.
- No production source, tests, metadata, or ROM files were modified.
- Nothing was staged, committed, or pushed.
- Only the requested ignored prompt and this ignored report were created.
- No screenshots, images, videos, gameplay recordings, MAME frames, Incitti
  source, web sources, or `TEMP/myNotes.md` were inspected.
- No build, test, game, decoder, or PROM execution was run.
