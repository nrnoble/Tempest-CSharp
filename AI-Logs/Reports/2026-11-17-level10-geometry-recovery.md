# Level 10 geometry recovery — RECOVERED

## Outcome

Yes. The genuine displayed Level 10 tube mapping and ordered geometry are
uniquely recoverable from local Rev. 2 evidence. The `.chunk` declarations in
the disassembly describe the tables and their sizes but omit their values;
the values are present in the complete local program ROM
`Reference/arcade/roms.rev2/136002-118.k1`.

No gameplay, rendering, tests, configuration, reference material, or durable
project documentation was changed. No game launch or gameplay acceptance was
performed or claimed.

## Evidence chain and decoding

1. **Table layout — direct disassembly evidence.**
   `Reference/arcade/Tempest Rom Source code.md` at `$B97B-$B980` states that
   the two-dimensional arrays are indexed by level first (`x[L][P]`), points
   are the coordinates, angles are sector values, and `lev_remap` gives the
   order in which levels are encountered. At `$B97C-$BCFC`, the declarations
   give the exact table sequence and sizes: `lev_x` at `$B97C`, `lev_y` at
   `$BA7C`, `lev_angle` at `$BB7C`, `lev_remap` at `$BC7C`, then scale,
   camera height, signed 2-D Y placement, open topology, and flipper-scale
   tables through `$BCFB`.

2. **Level index — direct disassembly plus established displayed-level
   correspondence.**
   The new-level path loads `p1_level` from `startlevtbl` at `$9187-$9197`;
   the table begins with `$00` at `$91FE`. Level advancement increments
   `p1_level` at `$C98E-$C996`, and the display value is formed by incrementing
   it at `$C9E3-$C9EA`. Geometry setup passes `p1_level` to `get_tube_no` at
   `$C237-$C239`. That routine reduces it to the low-nibble table index and
   reads `lev_remap` at `$C2E8-$C30C`. This matches the established project
   mapping of displayed Level 8 to internal index 7 and displayed Level 9 to
   internal index 8. Therefore displayed Level 10 uses `lev_remap[9]`.

3. **ROM-to-address anchor — bounded binary verification.**
   The ten named 2-KiB program ROMs (`136002-113.d1`, `114.e1`, `115.f1`,
   `116.h1`, `118.k1`, `119.lm1`, `120.mn1`, `121.p1`, `123.np3`, and
   `124.r3`) were checked only at offset `$0167` for the disassembly bytes at
   `$B967`: `AD 15 04 F0 09 AD 6F CE AE 6E CE B8 50 06 AD 87 CE AE 86 CE 60`.
   Exactly one matched: `136002-118.k1`. This anchors its `$B800` ROM page;
   the needed table bytes are at their disassembly addresses minus `$B800`.
   The ROM directory also contains smaller images, including 256-byte files,
   but these are not the only local ROM material and are not needed to recover
   these tables. The required bytes are in the full 2-KiB program ROM.

4. **Point ordering — direct disassembly evidence and mechanical decode.**
   Geometry setup at `$C2A0-$C2C3` reads each table row backward from its final
   byte while writing to the matching descending point slot. Thus the raw
   order is preserved: point `P=0` through `P=15`. The ROM drawing path at
   `$C505-$C546` branches on `lev_open` and uses the point sequence to draw
   the tube.

## Recovered Level 10 data

Displayed Level 10 has internal index `9`; `lev_remap[9] = $09`, selecting
shape row `9`. The following are the raw ordered ROM values, with columns
indexed by point `P=0..15`:

| P | X | Y | Angle |
|---:|---:|---:|---:|
| 0 | 10 | F0 | 0C |
| 1 | 10 | CB | 0C |
| 2 | 10 | A6 | 0C |
| 3 | 10 | 80 | 0D |
| 4 | 16 | 5C | 0E |
| 5 | 29 | 39 | 0F |
| 6 | 46 | 20 | 0F |
| 7 | 69 | 12 | 00 |
| 8 | 97 | 12 | 01 |
| 9 | BA | 20 | 01 |
| 10 | D7 | 39 | 02 |
| 11 | EA | 5C | 03 |
| 12 | F0 | 80 | 04 |
| 13 | F0 | A6 | 04 |
| 14 | F0 | CB | 04 |
| 15 | F0 | F0 | 00 |

Equivalent ordered byte arrays:

- `lev_x[9]`: `10 10 10 10 16 29 46 69 97 BA D7 EA F0 F0 F0 F0`
- `lev_y[9]`: `F0 CB A6 80 5C 39 20 12 12 20 39 5C 80 A6 CB F0`
- `lev_angle[9]`: `0C 0C 0C 0D 0E 0F 0F 00 01 01 02 03 04 04 04 00`

The row bytes are at chip offsets `$020C-$021B` (X), `$030C-$031B` (Y), and
`$040C-$041B` (angle). Companion row-9 bytes are at `$0485` (`lev_remap`),
`$0495` (scale), `$04A5` (y3d), `$04B5` (y2d low), `$04C5` (y2db high),
`$04D5` (open), `$04E5` (fscale), and `$04F5` (fscale2).

`lev_open[9]=$FF`, directly encoded as open by the disassembly's table
comment. With 16 ordered points, the open topology has 15 adjacent playable
segments and no point-15-to-point-0 closing segment.

Directly adjacent presentation metadata, if needed by a later implementation:

- `lev_scale[9]=$18` (source comment: reciprocal level scale).
- `lev_y3d[9]=$50` (source comment: 3-D Y offset / camera height).
- `lev_y2d[9]=$20`, `lev_y2db[9]=$FF`; low/high bytes combine to signed
  `$FF20` = `-224`.
- `lev_fscale[9]=$28`, `lev_fscale2[9]=$04`; source combines these as
  `(fscale2 << 7) | fscale = $0228`.

The raw values above are ROM facts. Their placement into the project's C#
projection or pixel units is not established by this investigation.

## Cross-check and validation

**PASS — Level 8 method cross-check.** Applying the same chip/address/table
decode to row 7 reproduces the established Level 8 X, Y, and angle arrays,
`lev_remap[7]=$07`, scale `$18`, camera height `$B0`, signed placement
`$0060`, and open flag `$FF`. These agree with
`TEMPEST-BEHAVIOR-SPEC.md`'s manually accepted Level 8 record and
`RomPresentationMetadata.cs`'s secondary mapping. This verifies the table
bases, row stride, point order, companion-table offsets, and open-byte
interpretation without reopening Level 8 presentation.

**PASS — bounded extraction.** The ROM verifier required each selected
program image to be exactly `$800` bytes, matched the disassembly signature
at the single expected offset, and extracted the geometry and companion
tables from the verified chip. The Level 8 assertions passed; the Level 10
row and metadata were decoded from the same verified layout. No derived file
was written.

**PASS — final Git/worktree integrity.** Starting state was branch `master`,
HEAD `385f603a94391756b2ca6cccd524dc17b281e782`, tracking `origin/master`,
with extensive pre-existing unstaged Level 7 investigation changes and
untracked diagnostic files. Those were preserved. `git diff --check` and a
comparison of the final status to the starting status found no new tracked or
untracked project/reference changes; only these ignored AI-Logs records were
written.

## Sources and evidence classifications

- `Reference/arcade/Tempest Rom Source code.md`, `$B97B-$BCFB`,
  `$9187-$9207`, `$C237-$C30C`, and `$C505-$C546`: **ROM-BACKED** table
  layout, indexing and interpretation.
- `Reference/arcade/roms.rev2/136002-118.k1`, offsets `$017C-$04FB`:
  **ROM-BACKED** raw geometry and companion table values.
- `TEMPEST-BEHAVIOR-SPEC.md`, Level 8 section: accepted Level 8 comparison
  data, documented there as **REV-2-BINARY-CONFIRMED / RAW-ROM-DATA**, used
  only for cross-check.
- `src/Tempest.Game/Levels/RomPresentationMetadata.cs`, production rows at
  lines 56-67: secondary project corroboration for Level 8/9 indexing and
  metadata; not used as authority for Level 10.
- `Reference/arcade/MBUCOD.V05.txt` and `Reference/arcade/MBUDOC.DOC` were
  inspected and identified as Mathbox microcode/documentation, not sources
  for the tube tables.

## Remaining uncertainty

No ambiguity remains in the raw Level 10 remap, row, ordered points, angles,
open flag, or listed companion bytes. Translating these raw presentation
values into C# rendering behavior remains separate implementation work.
