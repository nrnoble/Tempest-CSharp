# Atari Math Box `$c098` Reconciliation

**Outcome: PARTIAL / EVIDENCE LIMIT.** The operation selected by Tempest's `$c098` writes is established and cross-verified against the local Tempest PROMs and Atari's microcode source. The projection formula can be calculated when the runtime state is known. A complete unconditional arcade-coordinate oracle is not established because the available evidence does not establish the high byte of the quotient-count register's reset state or the physical decoder for the result-read aliases.

## Executive findings

- The earlier persistent “mode `$0f`” interpretation is wrong. Tempest writes the data byte `$0f` to Math Box offset `$0c`, documented as `NL` / the number of quotient bits. It does not write `$0f` to a mode register. A write to offset `$14` selects and starts `DIVZ`, the `Z/X'` divide routine.
- The actual `$c098` CPU writes are `$608e`, `$6094`, `$6095`, and `$6096`. Their low five address bits select offsets `$0e`, `$14`, `$15`, and `$16`; the addresses `$600e`, `$6014`, `$6015`, and `$6016` used in earlier discussion are offset shorthand, not the machine instructions' actual CPU addresses.
- The local 32-byte mapping PROM and the relevant Tempest microcode path match the corresponding mapping and symbolic routines in `MBUCOD.V05.txt`. The six 256-byte PROM candidates form the documented six-by-256x4 microcode set. Only the needed mapping entries and micro-PC locations were interpreted; this is not a full-image comparison.
- With the normal level setup pair `$5b:$5f = $ff:(-$lev_scale mod 256)`, `$c098` divides `axis_difference * 256` by `$57 + lev_scale`. The Math Box returns the signed 16-bit quotient; Tempest then restores the axis sign, applies CPU-side offsets, and saturates on signed overflow.
- Exact results for a particular call require its current `$57`, `$5b:$5f`, `$68:$69`, `$66:$67`, and Math Box retained state. `$c098` does not reinitialize all of that state.

## Evidence inventory and authority

| Source | Use and authority |
|---|---|
| `Reference/arcade/MBUDOC.DOC` | Original Atari Math Box interface, mapping-ROM behavior, microinstruction fields, and register/function descriptions. ATARI-DOCUMENTED. |
| `Reference/arcade/MBUCOD.V05.txt` | Original Atari symbolic microcode and mapping-ROM source. ATARI-DOCUMENTED. |
| `Reference/arcade/roms.rev2/136002-126.a1` | 32-byte local mapping PROM; relevant entries checked against the Atari mapping source. ROM-BACKED. |
| `Reference/arcade/roms.rev2/136002-127.e1` through `136002-132.l1` | Six 256-byte local microcode PROMs; only the required words were reconstructed. ROM-BACKED. |
| `Reference/arcade/roms.rev2/136002-119.lm1` | The bounded `$c098` CPU-code range was compared with the disassembly. ROM-BACKED. |
| `Reference/arcade/roms.rev2/136002-118.k1` | Selected table bytes used in the example were checked against the table-recovery handoff. ROM-BACKED. |
| `Reference/arcade/Tempest Rom Source code.md` | CPU bus instructions, setup, data flow, and caller annotations. Instruction bytes are TEMPEST-DISASSEMBLY-BACKED; descriptive names/comments remain non-authoritative. |
| `AI-Logs/Reports/2026-11-15-atari-projection-data-mathbox-investigation-report.md` and `AI-Logs/Reports/2026-11-15-atari-mathbox-mode0f-investigation-report.md` | Navigation only. Material claims used here were checked against the original documents, disassembly bytes, or local ROM/PROM bytes. |

No C# implementation or third-party implementation was used as evidence. No Internet or external service was used.

## Tempest Math Box PROM identification

`MBUDOC.DOC` describes a 256-word, 24-bit microcode ROM made from six 256x4 PROMs, and describes the mapping PROM as 32x8. `MBUCOD.V05.txt` independently describes three 256-byte output pages for the three bytes of each microinstruction and includes a 32-entry mapping-PROM source.

The local candidate files have the corresponding organization:

- `136002-126.a1` is 32 bytes. Its relevant mapping entries agree with the mapping source.
- `136002-127.e1` through `136002-132.l1` are six 256-byte files. Their reconstructed words at the invoked paths agree field-for-field with the Atari source.
- The three microinstruction bytes are physically represented by pairs of 4-bit PROMs: chips 132/131 provide the first byte, 130/129 the second, and 128/127 the third. The pairing and nibble order were established by matching the register-store path and the divide path to the documented fields and MBUCOD instructions.
- `136002-125.d7` is also 256 bytes, but was not opened or attributed to the Math Box. The six-file organization and matching relevant path identify 127-132 as the microcode set without inspecting 125.

**Relationship result:** the mapping entries and microinstructions relevant to `$c098` are an exact match to MBUCOD V05; no revision difference was found on the compared path. The whole six-PROM image was not decoded, so no whole-image equivalence claim is made.

## CPU address-to-function mapping

The CPU instruction bytes in ROM `$c098-$c16d` match the disassembly bytes in `136002-119.lm1` at file offset `CPU address - $c000`. In this write window, the Math Box mapping ROM uses CPU `A0-A4`; consequently the low five address bits select the documented offset. Higher address bits select the board's address window and can produce aliases with the same low-five-bit offset.

| CPU access in `$c098` | R/W | Decoded write offset / read function | Data and software purpose | Mapping entry and microcode path |
|---|---|---|---|---|
| `$6095` | W | `$15`, `XPL` | Low byte of the 16-bit divisor `X'`, calculated from `$57`, `$5f`, and `$5b`. | Mapping entry `$15` starts the low-byte `RXP` store at micro-PC `$2e`; it branches to the `RXP` display-and-stall path. |
| `$6096` | W | `$16`, `XPH` | High byte of `X'`; normally zero or one for the level-scale setup. | Mapping entry `$16` starts the high-byte `RXP` store at micro-PC `$2f`; it branches to the same display-and-stall path. |
| `$608e` | W | `$0e`, `ZLH` | The unsigned axis-difference byte is written into the high byte of `ZL`. | Mapping entry `$0e` starts the high-byte `RZL` store at micro-PC `$35`; it branches to the `RZL` display-and-stall path. It loads data; it does not start the divide. |
| `$6094` | W | `$14`, `SZXD` | The bus byte is not used by the divide path; the write selects the operation. | Mapping entry `$14` starts `DIVZ` (`Z/X'`) at micro-PC `$bd`. This is the arithmetic-start write for each axis. |
| `$6040` | R | `STAT` | `$c098` polls bit 7 until the Math Box is no longer busy. | Documented status read at offset 0; not a mapping-ROM microcode start. |
| `$6060` | R | Result-low read alias | The returned low byte is saved into the CPU-side result pair. | Tempest reads the low result here. MBUDOC names a low-output read port (`YLOW`), but its A0-A4 write-mapping description does not establish this physical read alias. |
| `$6070` | R | Result-high read alias | The returned high byte is saved into the CPU-side result pair. | Tempest reads the high result here. MBUDOC names a high-output read port (`YHIGH`), but its A0-A4 write-mapping description does not establish this physical read alias. |

Thus `$600e`, `$6014`, `$6015`, and `$6016` are useful shorthand for write offsets, but are not the CPU addresses encoded by `$c098`. The corresponding actual instructions access `$608e`, `$6094`, `$6095`, and `$6096`. The mapping PROM directly resolves their low-five-bit offsets. The read aliases `$6060/$6070` are distinct CPU addresses; available local Atari material does not show enough read-address wiring to translate them literally into MBUDOC's read offsets 2/3. Their use as the two result bytes is established by Tempest's instruction flow, but the physical read decoder remains UNKNOWN.

## Mapping-PROM and microcode reconstruction

The mapping PROM was read as a bounded 32-byte device. The entries needed for `$c098`, the register displays, and the startup writes resolve to the `RXP`, `RZL`, `RZH`, `RN`, and `DIVZ` routines named in MBUCOD. The write-to-micro-PC relation agrees with the source mapping expressions.

For the microcode, each selected PROM nibble was combined in the three-byte order described above, then decoded using MBUDOC's fields: A/B register selectors, source selector, STALL, ALU function, destination, SIGN, JMP, MULT, and CARIN. Only micro-PCs `$17`, `$1a`, `$2e-$2f`, `$34-$37`, and `$bd-$d8` were inspected. No PROM byte dump or hash is reproduced in this report.

Representative independent checks:

- The words selected by the `XPL/XPH` and `ZLH` mapping entries decode as the MBUCOD `LDL/LDH` register-store instructions for `RXP` and `RZL`, with the expected display/stall return path.
- The word at `DIVZ` decodes as the MBUCOD `ADD ZA,RZL,T0 RAMF`; subsequent words match the source's signed setup, unsigned divide loop, quotient sign correction, and terminal STALL paths. The loop's branch targets also land on the corresponding MBUCOD labels.
- The `$14` mapping entry lands directly at `DIVZ`, not at a data-selected mode routine.

This is a manual, bounded field reconstruction, not a reproduction of Atari's assembler. It covers the invoked path and the required register-load/return instructions only.

## Relevant microcode trace

### Register loads

- `XPL` and `XPH` write the low and high bytes of `RXP` (`X'`). Each write loads the register byte and reaches its display/stall path.
- `ZLH` writes the high byte of `RZL`. Each write loads the byte and reaches the `RZL` display/stall path.
- A write to `SZXD` has no input-data role in the divide instructions; its decoded mapping starts `DIVZ`.

### `DIVZ` / `Z/X'`

The source and local PROM trace agree on this sequence:

1. Copy the two 16-bit halves of `Z` into the divide work state (`T0` and `Q`).
2. Save the quotient sign from `RXP` and the high dividend word, then form magnitudes for signed division.
3. Use `T0` as divisor and the `T1:Q` pair as the 32-bit dividend. The unsigned core performs trial subtraction, restores/shifts the remainder as required, and shifts quotient bits into `Q`.
4. Use the saved sign to leave `Q` unchanged or negate it.
5. Stall with the 16-bit quotient on the output bus.

The final result is the signed-division quotient; the routine does not round it. For the nonnegative axis magnitudes and positive divisor prepared by the normal `$c098` path, the quotient is the integer quotient with the remainder discarded.

## `$c098` setup and CPU-side reconciliation

### Retained Math Box state

The initialization routine at `$c1c3` writes zero to the four bytes of `Z` through offsets `$0d/$0e/$0f/$10` and writes `$0f` to offset `$0c`. The Atari interface table identifies offset `$0c` as `NL`, the quotient-bit count. The mapping entry for `$0c` is the low-byte `RN` store; this is not a mode write. `$c098` itself does not write `RN`.

The divider's work registers `T0`, `T1`, `Q`, `T2`, and `T3` are populated or updated by the microcode path. The retained inputs used by the operation are:

- `RZH:RZL`: 32-bit dividend. The initialization zeros all four bytes. `$c098` replaces `RZL`'s high byte for each axis, leaving the ordinary gameplay setup's other bytes zero.
- `RXP` (`X'`): 16-bit divisor, loaded afresh by `$c098` before its two axis divides.
- `RN`: the quotient-bit count. The initialization writes low byte `$0f`; the nearby `$bd3e` helper temporarily writes `$18` and restores `$0f` before the following coordinate calls.

**Uncertainty:** the traced CPU writes do not initialize the high byte of `RN`; the mapping entry writes its low byte only. MBUDOC does not specify the Math Box register-file reset state. The local instructions therefore establish `RN.low = $0f`, but the evidence reviewed does not prove `RN.high = 0` after reset. With a zero high byte, the decrement/JPL loop runs 16 rounds (from 15 down through 0); without that state, the exact round count is not guaranteed by the inspected sources.

### Runtime and table inputs

The disassembly comment at `draw_linegfx` describes `$57` as “position along tube.” The ROM instructions establish that `$57` is zero-extended into the divisor calculation; the semantic name is a disassembly annotation, not an Atari hardware definition.

Level setup at `$c235` loads `lev_scale` and stores its two's-complement negative in `$5f`, with `$5b = $ff`. For this setup, the 6502 calculation at `$c098` computes:

`X' = zero_extend($57) - signed($5b:$5f)`.

So when `$5b:$5f` still contains the sign-extended negative selected scale, `X' = $57 + lev_scale`. If the computed high byte is negative, `$c098` substitutes divisor 1. This describes the current pair rather than asserting that every caller necessarily has the level-scale pair active; the disassembly has other code that can change these globals.

For each axis, `$c098` computes an unsigned magnitude and stores its sign separately:

- Y magnitude: `abs($58 - lev_y3d)`; the comparison sign is retained for CPU post-processing.
- X magnitude: `abs($56 - $5e)`; `$c16e` initializes `$5e` to `$80`, and the comparison sign is retained separately.

The routine waits for completion and reads the first quotient into `$63:$64` for Y. It then replaces `ZLH` with the X magnitude, starts the second divide with the same `X'`, and reads the quotient into `$61:$62`.

### CPU-side signs, offsets, and saturation

After the Y divide, the CPU adds the quotient to `$68:$69` when the Y difference is nonnegative, or subtracts it when negative. After the X divide, it similarly adds/subtracts the quotient from `$66:$67`. Signed overflow saturates positive results to `$7fff` and negative results to `$8000`. `$c235` initializes `$66:$67` to zero; `$68:$69` are the current signed Y-offset pair, loaded directly from `lev_y2d/lev_y2db` on the corresponding setup branch or otherwise dependent on existing state.

Open/closed tube traversal uses separate state and control flow. It is not part of this coordinate arithmetic.

## Numeric transformation

Let `p` be the current `$57`, `B:L` the live `$5b:$5f` pair, and `d` an unsigned axis difference. The CPU prepares `x' = (zero_extend(p) - signed(B:L)) mod 2^16`, replacing it with 1 if the high-byte sign check is negative. With the normal table setup, this is `p + lev_scale`.

The Math Box dividend is formed by writing `d` into `ZLH` while the other bytes of `Z` remain zero in the normal initialized path:

`Z = d * 2^8`.

`DIVZ` computes the signed quotient `Z / X'`, returning the 16-bit quotient on its output bus. For the normal nonnegative operands, this is:

`q = floor((d * 256) / x')`.

No rounding step is present; the divide loop's remainder is not added to the quotient. The factor of 256 is explicit in the placement of the byte in `ZLH`, not a guessed camera convention. The CPU then applies the separate axis sign and offset rules above, with signed-16 saturation.

This establishes a depth-sensitive scaling transform. It does not justify substituting a generic camera/perspective equation or assigning an unverified physical meaning to every internal register name.

## Minimal numeric verification

This is a conditional worked input, not a claim about a specific observed gameplay frame. Record 00, point 00, and its parameter bytes were checked directly in `136002-118.k1` at the offsets given in the table-recovery report: `lev_x=$f0`, `lev_y=$80`, `lev_scale=$18`, `lev_y3d=$50`, and the Y-offset pair `$ff40` (-192). Use `$57=$50`, the normal scale pair `$5b:$5f=$ff:$e8`, `$5e=$80`, `$66:$67=0`, and assume `$68:$69=$ff40` is the active offset pair.

- Divisor: `$50 + $18 = $68` (104).
- X: `abs($f0 - $80) = $70` (112); dividend `$7000`; quotient `floor($7000/$68) = $0113` (275), remainder 72.
- Y: `abs($80 - $50) = $30` (48); dividend `$3000`; quotient `floor($3000/$68) = $0076` (118), remainder 16.
- With the stated offset pair, the CPU results are X `$0113` and Y `$ff40 + $0076 = $ffb6`; neither saturates.

The quotient calculation assumes the documented 8-bit `N` setting is active with a zero high byte. The runtime `$57` and active-offset assumptions are explicit example inputs, not evidence that this tuple occurred during play.

## Required conclusions

1. **Was “mode `$0f`” wrong, incomplete, or correct?** Wrong as a persistent mode. `$0f` is the value written to the low `N` input at offset `$0c`; offset `$14` selects `DIVZ`.
2. **What does the relevant `$c098` access do?** `$608e` loads the axis magnitude into `ZLH`; `$6094` starts `Z/X'`.
3. **Which CPU write starts the arithmetic for each axis?** The write to `$6094` (offset `$14`) after each `$608e` load.
4. **Which retained registers participate?** `RZH:RZL` is the dividend; `RXP` is the divisor; `RN` supplies the divide count. `T0`, `T1`, `Q`, `T2`, and `T3` are divide work registers.
5. **What operation produces the 16-bit result?** Signed 32-bit-by-16-bit division, with a 16-bit quotient on the output bus; normal `$c098` operands are nonnegative, so the value is the truncated integer quotient of `(axis_difference << 8) / X'`.
6. **Does it implement depth-sensitive scaling/projection?** Yes: the divisor depends on the live position/scale pair, and the numerator is the axis magnitude multiplied by 256. “Projection” here describes the recovered arithmetic, not a conventional camera formula.
7. **Are arcade coordinate results now numerically reproducible?** The arithmetic relationship is reproducible for a call when its runtime and retained state are supplied. A complete unconditional result is not established until the `RN` high-byte/reset state and exact read-port decode are resolved; offsets and runtime position are also call-state inputs.
8. **Is the original transform sufficiently understood for a later read-only C# comparison?** Yes, for an equation/data-flow comparison with explicit runtime state and the limitations above. No C# comparison was performed in this investigation.

## Evidence classifications

- **ATARI-DOCUMENTED:** 32-offset interface; mapping-ROM load/start behavior on writes; microcode organization and fields; `NL`, `ZLH`, `XPL`, `XPH`, and `SZXD` functions; symbolic `DIVZ` and divide-loop behavior in MBUCOD V05.
- **TEMPEST-ROM/PROM-BACKED:** Local mapping-PROM entries and the selected microcode path match the source; sampled tube and parameter bytes in record 00; the `$c098-$c16d` instructions match local CPU ROM bytes.
- **TEMPEST-DISASSEMBLY-BACKED:** Actual CPU addresses, data preparation, status polling, result reads, sign restoration, offsets, saturation, setup, and temporary `$18`/restore-`$0f` count behavior.
- **CROSS-VERIFIED:** The `$14` write maps to `DIVZ`; the `$0e/$15/$16` writes load the expected `ZL`/`X'` bytes; the local path's fields and branches agree with the corresponding MBUCOD source.
- **INFERENCE:** `$57` represents position along the tube in gameplay; this semantic label comes from disassembly context, not MBUDOC.
- **HYPOTHETICAL:** The numeric example's selected runtime position and active offset state.
- **UNKNOWN:** `RN` high-byte/reset state; physical decode connecting `$6060/$6070` to the documented result-read functions; the exact current offsets and runtime value for an arbitrary gameplay call.

## Implications for the next milestone

The transform is sufficiently established for a later **read-only, state-parameterized comparison** with the current C# model. Such a comparison should distinguish the recovered arithmetic from unestablished call-time values, and must not treat the unresolved `RN` reset state or read-port wiring as proven.

## Scope and completion

- **Report:** `AI-Logs/Reports/2026-11-15-atari-mathbox-c098-reconciliation-report.md`.
- **Files created:** This report only. The alternate clarification selected the single c098 report; no prompt file was created.
- **Files inspected:** The two Atari documents; selected `$c098`, initialization, level-setup, `$bd3e`, and Math Box access sections of `Tempest Rom Source code.md`; both prior investigation handoffs as navigation; local files listed below. No C# files, images, recordings, or video were inspected.
- **PROM ranges inspected:** Mapping PROM `136002-126.a1`, offsets `$00-$1f`. Each of `136002-127.e1` through `136002-132.l1`, micro-PC offsets `$17`, `$1a`, `$2e-$2f`, `$34-$37`, and `$bd-$d8`. No PROM hashes or PROM byte/nibble dump is included.
- **ROM ranges inspected:** `136002-119.lm1`, offsets `$0098-$016d` (202 parsed instruction bytes checked); `136002-118.k1`, offsets `$017c-$018b`, `$027c-$028b`, `$048c`, `$049c`, `$04ac`, and `$04bc`.
- **Commands/scripts:** Non-interactive Git status/branch/index checks; bounded PowerShell/Python reads and comparisons of the named files and ranges. No builds, tests, game launch, package installation, Internet search, staging, commit, or push.
- **Starting Git state:** Branch `master`, HEAD `385f603a94391756b2ca6cccd524dc17b281e782`, tracking `origin/master`; staged diff empty. There were 16 pre-existing tracked modifications and four pre-existing untracked paths: the two added Atari documents and two portrait-candidate C# files. The selected report target did not exist before this investigation.
- **Ending Git state:** No tracked file changed and the index was not altered. Existing work was left untouched. Nothing was staged, committed, or pushed. The only new file is this report under ignored `AI-Logs/`.
- **Most important newly established fact:** `$6094`/offset `$14`, not a data byte `$0f`, starts the `Z/X'` divide used by `$c098`.
- **Most important remaining unknown:** The high-byte/reset state of the internal `RN` counter, which prevents an unconditional claim about the exact quotient-loop count for every call.
