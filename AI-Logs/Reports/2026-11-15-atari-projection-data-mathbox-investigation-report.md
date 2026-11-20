# Tempest Atari Projection Data and Math Box Investigation

## Executive conclusion

The numeric bytes hidden by the disassembly's `.chunk` placeholders were recovered from the local raw Atari ROM image `136002-118.k1`. The relevant records contain 16 distinct X rows and 16 distinct Y rows, plus per-record scale, Y-offset, and open/closed values. The ROM values are independently aligned with the disassembly by the bytes immediately before and after the requested table block.

The numeric arcade transformation is **not fully reconstructable from the evidence examined**. The disassembly traces the CPU operands, active Math Box control byte, two result reads, and signed CPU offset/saturation steps. It does not define the arithmetic selected by Math Box control value `$0f`, the operation's fixed-point scale, or its internal signedness. The `$c098` routine itself does not write the control register, so the arithmetic interpretation cannot be derived from its register sequence alone. No conventional perspective formula is assumed.

## ROM data recovery

The source is the project-local ROM-derived disassembly, `Reference/arcade/Tempest Rom Source code.md`, with the corresponding bytes recovered from `Reference/arcade/roms.rev2/136002-118.k1`.

The disassembly declares the point arrays and 16-entry parameter tables at `$b97c-$bcfb` (source lines 7461-7480). The raw ROM file is mapped at `$b800`; its file offsets `$017b-$04fc` correspond to CPU addresses `$b97b-$bcfc`. The byte at `$b97b` is `$60`, matching the disassembly's `RTS`; the byte at `$bcfc` is `$3e`, matching the disassembly's following `.byte $3e`. These boundary checks validate the extraction alignment.

Raw values below are hexadecimal bytes. `lev_x` and `lev_y` are shown one 16-byte row per record, in record-index order `00-0f`; each row contains the 16 points in the order stored in ROM. Table bytes are treated as unsigned point coordinates because the setup/render code compares them as unsigned bytes and computes a magnitude plus a separate sign. No visual meaning is inferred from their values.

### `lev_x`

- Address range: `$b97c-$ba7b`
- Raw ROM range: `136002-118.k1`, offsets `$017c-$027b`
- Use: selected record's 16 bytes are copied into `tube_x` by `$c235-$c2c3`; `$c098` consumes each resulting point.

```text
00: f0 e7 cf aa 80 56 31 19 10 19 31 56 80 aa cf e7
01: f0 f0 f0 b8 80 48 10 10 10 10 10 48 80 b8 f0 f0
02: f0 f0 b8 b8 80 48 48 10 10 10 48 48 80 b8 b8 f0
03: ec d5 b1 90 70 4f 2b 14 14 2b 4f 70 90 b1 d5 ec
04: f0 c0 a0 94 6c 60 40 10 10 40 60 6c 94 a0 c0 f0
05: d9 c2 ac 97 80 69 52 3c 27 10 35 5a 80 a6 ca f0
06: ea e0 9c 80 64 20 16 50 16 20 64 80 9c e0 ea b0
07: 10 1e 2c 3a 48 56 64 70 90 9e ac ba c8 d6 e4 f0
08: 10 1e 2d 3c 4b 5a 69 78 87 96 a5 b4 c3 d2 e1 f0
09: 10 10 10 10 16 29 46 69 97 ba d7 ea f0 f0 f0 f0
0a: 10 24 30 36 3e 49 5a 75 94 a4 ac ba da e2 ea f0
0b: 80 70 48 20 10 20 48 70 80 90 b8 e0 f0 e0 b8 90
0c: da a4 87 80 79 5c 26 10 10 20 48 80 b8 e0 f0 f0
0d: 10 10 30 30 50 50 70 70 90 90 b0 b0 d0 d0 f0 f0
0e: b0 80 50 47 18 30 18 47 50 80 b0 b9 e8 d4 e8 b9
0f: 10 1e 21 28 3c 55 66 73 8d 9a ab c4 d8 df e2 f0
```

### `lev_y`

- Address range: `$ba7c-$bb7b`
- Raw ROM range: `136002-118.k1`, offsets `$027c-$037b`
- Use: selected record's 16 bytes are copied into `tube_y` by `$c235-$c2c3`; `$c098` consumes each resulting point.

```text
00: 80 aa cf e7 f0 e7 cf aa 80 56 31 19 10 19 31 56
01: 80 b8 f0 f0 f0 f0 f0 b8 80 48 10 10 10 10 10 48
02: 80 b8 b8 f0 f0 f0 b8 b8 80 48 48 10 10 10 48 48
03: 94 b0 b8 a7 a7 b8 b0 94 6c 50 48 59 59 48 50 6c
04: 96 a3 c5 f0 f0 c5 a3 96 6a 5d 3b 10 10 3b 5d 6a
05: 3d 6a 97 c4 f0 c4 97 6a 3d 10 10 10 10 10 10 10
06: a0 e0 ea b0 ea e0 a0 80 60 20 16 50 16 20 60 80
07: f0 d0 b0 90 70 50 30 10 10 30 50 70 90 b0 d0 f0
08: 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40 40
09: f0 cb a6 80 5c 39 20 12 12 20 39 5c 80 a6 cb f0
0a: c0 a6 8a 6a 4a 2f 14 24 20 39 59 75 72 90 b0 d0
0b: 80 57 48 57 80 a9 ba a9 80 57 48 57 80 a9 ba a9
0c: e4 e8 b7 80 b7 e8 e4 b2 7a 47 20 10 20 47 7a b2
0d: 90 70 70 50 50 30 30 10 10 30 30 50 50 70 70 90
0e: e6 d0 e6 b9 ae 80 52 47 14 30 14 47 52 80 ae b9
0f: 7e 6a 51 3a 2c 2c 38 4e 4e 38 2c 2c 3a 51 6a 7e
```

### Per-record parameter tables

| Table | Address range | Raw ROM offsets | Raw values, record `00-0f` | Use established by disassembly |
|---|---|---|---|---|
| `lev_remap` | `$bc7c-$bc8b` | `$047c-$048b` | `00 01 02 03 04 05 06 07 0d 09 08 0c 0e 0f 0a 0b` | `$c2e8-$c30c` maps the current level to a tube record. This table is indexed by level, not by tube record. |
| `lev_scale` | `$bc8c-$bc9b` | `$048c-$049b` | `18 1c 18 0f 18 18 18 18 0a 18 10 0f 18 0c 14 0a` | `$c235-$c248` reads the selected value and negates it into `$5f`; `$c098` uses `$5f`. |
| `lev_y3d` | `$bc9c-$bcab` | `$049c-$04ab` | `50 50 50 68 50 50 68 b0 a0 50 90 80 20 b0 60 a0` | `$c257-$c25a` loads the selected byte into `y3d`/`$60`; `$c098` compares `$58` against it. |
| `lev_y2d` | `$bcac-$bcbb` | `$04ac-$04bb` | `40 20 40 80 40 40 70 60 00 20 40 00 a0 40 40 00` | `$c268-$c28b` establishes the current Y offset state. |
| `lev_y2db` | `$bcbc-$bccb` | `$04bc-$04cb` | `ff ff ff ff ff ff ff 00 01 ff 00 00 fe 01 ff 01` | Loaded as the high byte paired with `lev_y2d`; `$c098` uses the pair in CPU-side offset arithmetic. |
| `lev_open` | `$bccc-$bcdb` | `$04cc-$04db` | `00 00 00 00 00 00 00 ff ff ff ff 00 00 ff 00 ff` | `$c25c-$c25f` loads the state; gameplay traversal branches on it. |

`lev_remap` is indexed by level order. Its raw level-to-record map is:

```text
level:  00 01 02 03 04 05 06 07 08 09 0a 0b 0c 0d 0e 0f
record: 00 01 02 03 04 05 06 07 0d 09 08 0c 0e 0f 0a 0b
```

`lev_angle` occupies `$bb7c-$bc7b` between the point tables and `lev_remap`. Its boundary was used to align the parameter addresses; its contents were not decoded or compared because this milestone only required it to establish the following table boundary. The disassembly's annotation describes it as a 4-bit sector angle, but that annotation is not used to infer projection math.

## Cross-tube comparison

The first column is the ROM tube-record index used by the point/parameter arrays. `y2d` is the low byte and `y2db` the high byte; the signed decimal column is their little-endian 16-bit two's-complement value, consistent with the CPU's paired-byte add/subtract and signed-overflow saturation in `$c098`.

| Record | Scale | Y3D | Y2D low | Y2DB high | Y2D signed word | Open |
|---:|---:|---:|---:|---:|---:|---:|
| `00` | `18` | `50` | `40` | `ff` | `-192` | `00` |
| `01` | `1c` | `50` | `20` | `ff` | `-224` | `00` |
| `02` | `18` | `50` | `40` | `ff` | `-192` | `00` |
| `03` | `0f` | `68` | `80` | `ff` | `-128` | `00` |
| `04` | `18` | `50` | `40` | `ff` | `-192` | `00` |
| `05` | `18` | `50` | `40` | `ff` | `-192` | `00` |
| `06` | `18` | `68` | `70` | `ff` | `-144` | `00` |
| `07` | `18` | `b0` | `60` | `00` | `96` | `ff` |
| `08` | `0a` | `a0` | `00` | `01` | `256` | `ff` |
| `09` | `18` | `50` | `20` | `ff` | `-224` | `ff` |
| `0a` | `10` | `90` | `40` | `00` | `64` | `ff` |
| `0b` | `0f` | `80` | `00` | `00` | `0` | `00` |
| `0c` | `18` | `20` | `a0` | `fe` | `-352` | `00` |
| `0d` | `0c` | `b0` | `40` | `01` | `320` | `ff` |
| `0e` | `14` | `60` | `40` | `ff` | `-192` | `00` |
| `0f` | `0a` | `a0` | `00` | `01` | `256` | `ff` |

Comparisons directly supported by these bytes:

- All 16 X rows are distinct and all 16 Y rows are distinct; no two records have the same X/Y pair.
- `lev_scale` has 7 distinct byte values; `lev_y3d` has 8; `lev_y2d` has 7; and `lev_y2db` has 4.
- Repeated full numeric tuples `(scale, y3d, y2d, y2db)` are `00,02,04,05` (`18,50,40,ff`) and `08,0f` (`0a,a0,00,01`). Each repeated tuple has one `lev_open` value. No complete numeric tuple is shared by an open and closed record.
- No single parameter value partitions open from closed: for example, `scale=18`, `y3d=50`, `y2d=40`, and `y2db=ff` each occur among both open and closed records. The full-tuple consistency is a property of these records, not evidence that the numeric fields were intended to encode openness.
- `lev_open=$ff` for records `07,08,09,0a,0d,0f` (six open records); it is `$00` for `00-06,0b,0c,0e` (ten closed records).

## `$c098` Math Box trace

The trace below is disassembly-backed; register names and addresses follow the local source's aliases (`mb_w_XX`, `mb_rd_l`, `mb_rd_h`, and `eactl_mbst`). The routine spans `$c098-$c16d`.

1. **Depth/scale word prepared.** The CPU subtracts `$5f` from `$57` into the low byte written to `$6015` (`mb_w_15`), then subtracts `$5b` with the propagated borrow from zero and writes the high byte to `$6016` (`mb_w_16`). If the high-byte result is negative, it replaces the pair with `$0001`. Level setup sets `$5f` to the two's-complement negation of the selected `lev_scale` and `$5b` to `$ff`.
2. **Y magnitude and sign prepared.** The CPU compares `$58` with `y3d`/`$60`, computes the unsigned absolute difference, writes the same byte to `$600e` and `$6014`, and records which side of `y3d` the input was on in `$33`.
3. **First result read.** It polls `$6040` (`eactl_mbst`) while bit 7 is set, then reads `$6060` and `$6070` (`mb_rd_l`/`mb_rd_h`) into `$63/$64`.
4. **Y offset applied by the CPU.** For the nonnegative side, `$68/$69` is added to `$63/$64`; for the negative side, the result is subtracted from `$68/$69`. Signed overflow saturates to `$7fff` or `$8000`. The offset pair originates from the selected `lev_y2d`/`lev_y2db`, with setup/transition state prepared at `$c268-$c28b`.
5. **X magnitude and sign prepared.** The CPU compares `$56` with `$5e`, computes the unsigned absolute difference, writes that byte to `$600e` and `$6014`, and records the side in `$34`. The `$6015/$6016` pair is not rewritten between the Y and X passes.
6. **Second result read and X offset.** After the same busy poll, `$6060/$6070` are read into `$61/$62`. The CPU applies `$66/$67` according to `$34`, with signed saturation. Setup initializes `$66/$67` to zero.

The routine does **not** write `$600c` (`mb_w_0c`), the visible control/mode register. The initialization at `$c1f7-$c1f9` writes `$0f`; a path at `$bd51-$bd6f` temporarily writes `$18` and restores `$0f`, while the separate divide helper at `$dce6-$dd0c` writes `$10`. Those writes show that the control value matters, but the disassembly does not define the arithmetic selected by `$0f`. The `$dce6` comment calls its helper an apparent divide while explicitly noting uncertainty about Math Box details; it is not a verified definition of `$0f`.

The Math Box return is consumed as a two-byte magnitude before CPU-side sign and offset handling. The code establishes a 16-bit result readout and a signed 16-bit final coordinate representation; it does not establish internal Math Box precision, fixed-point scaling, rounding, saturation, or operand signedness. The direct computation of the `$c098` result therefore cannot be rewritten as a verified multiplication, division, or perspective equation from the examined evidence.

## Reconstructed Atari transformation

The established sequence is:

1. Select the tube record through `lev_remap`; copy its 16 X/Y points.
2. Load that record's `lev_scale`, `lev_y3d`, `lev_y2d`/`lev_y2db`, and `lev_open`.
3. For each point, form the CPU operand pair from `$57`, `$5f`, and `$5b`; form Y and X absolute differences against `y3d` and `$5e`.
4. Submit each axis difference with the prepared pair under the active Math Box mode, read a 16-bit result, apply the selected Y offset or zero X offset on the CPU, and saturate the signed coordinate.
5. The separately loaded `lev_open` value changes gameplay vector traversal. It is not read by `$c098`.

The missing link is the arithmetic/units mapping for Math Box mode `$0f` (and the register-field interpretation needed to turn its inputs into a numeric result). Without that, there are no evidence-grounded worked projection examples or complete numeric output equation. Filling this gap with a conventional camera formula would exceed the evidence.

## Evidence classifications

- **ROM-BACKED:** The listed X/Y point bytes and the values in `lev_remap`, `lev_scale`, `lev_y3d`, `lev_y2d`, `lev_y2db`, and `lev_open` were read from `136002-118.k1` at the bounded offsets stated above. Boundary bytes match the disassembly.
- **DISASSEMBLY-BACKED:** The table boundaries and layout; record selection and consumers; the `$c098` operand writes, busy polls, result reads, CPU-side offset/saturation; `$0f` initialization; and the open/closed traversal branch.
- **STRONG INFERENCE:** The common `$c098` path applies to distinct per-record geometry and parameters, rather than each point being a final precomputed screen coordinate. The exact geometric meaning of each parameter remains unproven.
- **HYPOTHESIS:** None is used as a conclusion. In particular, interpreting mode `$0f` as multiplication or as a conventional perspective operation remains unverified.
- **UNKNOWN:** The arithmetic selected by mode `$0f`, Math Box operand signedness and internal precision, fixed-point unit/scale, and its rounding behavior.

## Implications

- **Common versus per-tube inputs:** The CPU routine and active mode are shared; point geometry, scale, `y3d`, Y-offset pair, and open state are selected per record. `$57` and `$5e` are common runtime inputs in the traced path.
- **Open versus closed processing:** `lev_open` is a distinct per-record flag and affects vector traversal outside `$c098`; the table contains six `$ff` open records and ten `$00` closed records.
- **Universal numeric parameter:** The ROM does not support representing the mechanism by one universal per-tube numeric value: the requested parameter tables have multiple distinct values, and the 16 point records are all distinct. A shared Math Box code path exists, but its exact numeric operation is unresolved.

## Remaining unknowns

- What arithmetic and fixed-point convention Math Box control value `$0f` selects.
- Whether the two-byte prepared operand and the Math Box's two-byte readout are signed, unsigned, or internally wider before truncation.
- Exact scaling/rounding of each Math Box result and therefore the numeric coordinate for any specific point.
- Whether the available local ROM package contains a Math Box operation decode in a PROM whose role is not identified by the inspected disassembly.

## Next recommendation

The smallest justified next step is to locate an authoritative project-local Atari Math Box hardware/operation decode for control value `$0f` and the `$600e/$6014/$6015/$6016` register sequence. No further geometry-table recovery is needed. If no such local decode is available, this evidence investigation has reached its limit; the numeric Atari transformation should remain unresolved rather than be inferred from the current C# model or an assumed perspective formula.

## Scope and completion

- Report: `AI-Logs/Reports/2026-11-15-atari-projection-data-mathbox-investigation-report.md`.
- Files created: this report only.
- Original Atari files whose bytes were read: `136002-118.k1`, offsets `$017b-$04fc` (table block plus one boundary byte on either side); `136002-119.lm1`, offsets `$017b` and `$04fc` only, used to reject an incorrect ROM-to-address alignment.
- Project-local source read: `Reference/arcade/Tempest Rom Source code.md`, limited to the table declarations, consumers, `$c098` routine, Math Box setup/control accesses, and bounded related call sites.
- Exact ROM address region decoded: `$b97c-$bcfb`; boundary bytes at `$b97b` and `$bcfc` were checked. No other ROM address regions were read, and no PROM contents were inspected.
- No current C# implementation, test code, screenshots, frames, or videos were used as Atari evidence. No build/tests were required or run; the game was not launched.
- No tracked files were modified by this investigation. The starting worktree already contained unrelated tracked modifications and untracked files; all were left untouched.
- Nothing was staged, committed, or pushed. `AI-Logs/` is ignored.
- Most important new ROM-backed finding: all 16 distinct X/Y record rows and the per-record parameter bytes are now available for direct numeric comparison.
- Most important remaining unknown: the active Math Box mode `$0f`'s arithmetic and fixed-point semantics, preventing a complete numeric projection equation.
