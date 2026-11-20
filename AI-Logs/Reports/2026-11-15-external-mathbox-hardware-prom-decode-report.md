# External Math Box Hardware and PROM Coordinate Decode

**Date:** 2026-11-15
**Status:** READ-ONLY INVESTIGATION COMPLETE
**Primary classification:** **MATHBOX TRANSFER DECODED AND INDEPENDENTLY VERIFIED**

## Scope and result

This investigation closes the command-selection and microcode-control gaps
for the Tempest CPU coordinate routine at `$C098`. The verified project-local
Rev 2 PROM bytes were assembled and executed with a bounded, fail-closed
temporary decoder. The selected operation is Math Box command `$14` (`SZXD`,
Z/X′ divide), dispatched to micro-PC `$BD`. For the coordinate input domain,
the local PROM execution establishes:

```text
M(magnitude, depth) = floor((magnitude * 128) / depth), when depth > 0
M(magnitude, 0)     = 0xFFFF
```

The positive-depth result is an unsigned 16-bit value with seven fractional
bits relative to `magnitude/depth`. The CPU separately restores the coordinate
sign. The zero-depth result is the observed microcode result, not a division
guard or saturation rule; the production `$C098` path supplies positive depth.

No C# projection or other project implementation was made. The actual Atari
schematic sheet was not obtained; the primary command and control evidence is
the Atari-authored Math Box documentation and microcode source. The MiSTer
RTL and MAME were used only as secondary structural and behavioral
cross-checks, respectively.

## Starting Git state

Before external research:

- Branch: `master`.
- `HEAD` and `origin/master`: `385f603a94391756b2ca6cccd524dc17b281e782`.
- Index: clean.
- Tracked worktree: the expected 16 modified candidate files, 2,021
  insertions and 127 deletions.
- Non-ignored untracked files: exactly the expected
  `src/Tempest.Game/Levels/RomPresentationMetadata.cs` and
  `tests/Tempest.Game.Tests/RomPresentationMetadataTests.cs`.
- No unrelated work was observed.

The substantive prompt was copied byte-for-byte to
`AI-Logs/Prompts/2026-11-15-external-mathbox-hardware-prom-decode-prompt.md`.
The source attachment and saved prompt SHA-256 both equal
`BC1249317CF0658D4967A03B9C35BA3A14356C444B21AE3429857DEF2A925319`.

The prior reports read for context and starting-state authority were
`AI-Logs/Reports/2026-11-15-mathbox-dvg-coordinate-translation-readonly-report.md`
and
`AI-Logs/Reports/2026-11-15-mathbox-prom-coordinate-operation-decode-report.md`.
Their local PROM fingerprint table was reused.

## Sources and applicability

### Primary Atari evidence

1. **Atari, `MBUDOC.DOC`, “Math-Box Documentation,” 30 Mar 1981**
   (original control documentation, hosted in the historical Tempest source
   archive):
   [MBUDOC.DOC](https://github.com/historicalsource/tempest/blob/6c783bee488ed736fc3fdc3a81fdc412c3bec386/MBUDOC.DOC),
   especially “External Appearance” pp. 2 and “Architecture” pp. 3–7.
   It documents the 32 CPU offsets, address A0–A4 loading the mapping PROM,
   256-by-24 microcode made from six 256-by-4 PROMs, every microinstruction
   field, Am2901 source/destination conventions, sign/shift branch signal,
   and sequencer halt/restart behavior.

2. **Atari, `MBUCOD.V05`, “Micro Code for 2901 Math-Box,”**
   original Tempest microcode source:
   [MBUCOD.V05](https://github.com/historicalsource/tempest/blob/6c783bee488ed736fc3fdc3a81fdc412c3bec386/MBUCOD.V05).
   Its `Z/X′` section labels the operation `DIVZ`, initializes the divide
   operands, and falls through to the signed divide. Its `DIVID` comments
   describe the iterative divide and quotient output. This is original
   microcode documentation, not a modern behavioral guess.

3. **Atari, `TEMPST.DOC`, original PROM inventory and board listing:**
   [TEMPST.DOC](https://github.com/historicalsource/tempest/blob/6c783bee488ed736fc3fdc3a81fdc412c3bec386/TEMPST.DOC).
   The inventory identifies `136002-126` at A1 as a 32x8 mapping PROM and
   `136002-127..132` as six 256x4 microcode PROMs in E1/F1/H1/J1/K1/L1.
   This inventory is an organization cross-check; it is not asserted to be
   the exact Rev 2 PROM readout.

4. **Atari Tempest service manual TM-190 (1981):**
   [Internet Archive item](https://archive.org/details/atari-tempest-tm-190-service-manual),
   OCR service-manual memory map entry “Math Box Start,” `$6080-$609F`.
   The annotated local Atari program listing gives the concrete `$C098`
   writes and the matching `$6080`-based offsets. No schematic page was
   available for direct inspection.

### AMD and secondary cross-checks

5. **AMD, *The Am2900 Family Data Book* (1978), Am2901A section, pp. 2-3
   onward:**
   [Bitsavers scan](https://bitsavers.org/components/amd/bitslice/1978_The_Am2900_Family_Data_Book.pdf).
   This AMD primary data book documents the register-file ports, source and
   function selects, carry/overflow, write destinations, Q register, and
   shift controls. It identifies the Am2901A as a pin-compatible
   replacement for the Am2901. Atari `MBUDOC` supplies the board-specific
   shift and sequencer wiring.

6. **Eric Smith, `brouhaha/mathdis`:**
   [README](https://github.com/brouhaha/mathdis/blob/17aab25e56fe2cf9eb03bc52dbbd5652c4b6b79f/README.md)
   and [disassembler](https://github.com/brouhaha/mathdis/blob/17aab25e56fe2cf9eb03bc52dbbd5652c4b6b79f/mathdis.c).
   The README independently describes a 32-command Math Box, dispatch PROM,
   8-bit start address, and halt bit. The disassembler explicitly cautions
   that a schematic and Am2901 data sheet are needed to understand its output;
   it was not treated as the hardware authority.

7. **Videodr0me, Tempest MiSTer hardware-level RTL**, pinned repository
   revision:
   [`rtl/tempest_mathbox.sv`](https://github.com/Videodr0me/Arcade-Tempest_MiSTer/blob/31c52ce5d81d5ceee5a902475df438312fe30aff/rtl/tempest_mathbox.sv)
   and [source provenance](https://github.com/Videodr0me/Arcade-Tempest_MiSTer/blob/31c52ce5d81d5ce5a902475df438312fe30aff/SOURCES.md).
   The RTL states it models the Tempest schematic Sheet 3B and loads original
   command/microcode PROMs. Its nibble assembly, field slices, source
   selection, branch condition and STALL handling agree with the decoded
   local words. The schematic itself is kept outside that repository, so
   this is a secondary hardware-level cross-check, not a direct schematic
   citation.

8. **Tempest full disassembly and C port**, pinned
   [ROM generator](https://github.com/tcottrill/Tempest-Full-Disassembly-and-C-Port/blob/da0fa4d6324f8cac22da2f488b1617a839a79fb0/c_src/tools/gen_roms.py),
   [Math Box interpreter](https://github.com/tcottrill/Tempest-Full-Disassembly-and-C-Port/blob/da0fa4d6324f8cac22da2f488b1617a839a79fb0/c_src/mathbox.c),
   and [Math Box interface](https://github.com/tcottrill/Tempest-Full-Disassembly-and-C-Port/blob/da0fa4d6324f8cac22da2f488b1617a839a79fb0/c_src/mathbox.h).
   Its generator documents the exact six-nibble order and asserts two
   source-derived control words. Its README describes the separate ROM
   call-by-call verification. It is a useful independent engineering
   comparison, not original Atari authority; public PROM bytes were not used.

9. **MAME**, pinned
   [`src/mame/atari/tempest.cpp`](https://github.com/mamedev/mame/blob/77999d3dd53e7aa1d80d0657feda8ca2279cbd3b/src/mame/atari/tempest.cpp)
   and
   [`src/mame/atari/mathbox.cpp`](https://github.com/mamedev/mame/blob/77999d3dd53e7aa1d80d0657feda8ca2279cbd3b/src/mame/atari/mathbox.cpp).
   The Tempest map independently identifies writes `$6080-$609F`; `go_w`
   implements command `$14` with a high-level restoring divide. It is used
   only for numeric behavioral cross-checking, not as the microcode
   definition.

No screenshots, gameplay recordings, Gemini material, Incitti material, or
host-rendered frames were inspected. No external PROM image was downloaded or
used.

## Local Rev 2 PROMs and provenance

All decoding used only these seven existing project-local files. Sizes and
SHA-256 values match the prior local inventory report:

| File | Bytes | SHA-256 |
|---|---:|---|
| `Reference/arcade/roms.rev2/136002-126.a1` | 32 | `14EFB14AC87A8DB219FB998E4CE14FA62488F1C9CFD882B41B9EB09EFFFEE373` |
| `Reference/arcade/roms.rev2/136002-127.e1` | 256 | `2150566BF27FB038C9A19081BC6FBDAE49B3F0E932EB5328261E51A1B2CF2767` |
| `Reference/arcade/roms.rev2/136002-128.f1` | 256 | `26CDD1D690C0033366FF3ABA5284898DD4BC3192F004EF6004EFCC17C8E0C5DF` |
| `Reference/arcade/roms.rev2/136002-129.h1` | 256 | `67A8CF9A25938FA75FFB05E70FD4252CEA808565BF3EB778D6EA13859C82DBC8` |
| `Reference/arcade/roms.rev2/136002-130.j1` | 256 | `243E3AA7CD48920599DD01BF810A9CE38148E036E85A836A101E27D126563622` |
| `Reference/arcade/roms.rev2/136002-131.k1` | 256 | `68B5EC6328BC5585930F5B7A617A4B2C3FFC961634D1DAE2841B8E8E681DD80D` |
| `Reference/arcade/roms.rev2/136002-132.l1` | 256 | `42C7C60353DDC1A12344D6DB4617D2D4FD59E805FA2967736BB78E084A8F1109` |

The local bytes were re-read for execution. Their PROM sizes, Atari's
documented device organization, and source-derived word checks agree. No
claim is made that the public secondary implementations' PROM files are
byte-identical to these local Rev 2 bytes.

## Dispatch path from `$C098`

Atari `MBUDOC` documents the Math Box as 32 offsets, with the CPU's A0-A4
addressing the mapping PROM on each write. The service-manual map places this
write range at `$6080-$609F`, so the five low address bits are the command
index; the write data is simultaneously presented to the datapath.

The local listing at `$C098-$C0E3` writes:

| CPU write | Offset / index | Atari name | Role in coordinate request |
|---|---:|---|---|
| `$6095` | `$15` | XPL | low byte of `X′` (the depth input) |
| `$6096` | `$16` | XPH | high byte of `X′` |
| `$608E` | `$0E` | ZLH | high byte of the 32-bit `Z` dividend; receives the magnitude |
| `$6094` | `$14` | SZXD | starts the Z/X′ divide |

The sequence occurs twice: once for the source-Y-relative magnitude and once
for the source-X-relative magnitude, using the same depth. Thus `$C098` as a
6502 routine address is not itself a Math Box command. Its final write to
`$6094` presents command index `$14` to the mapping PROM. The preceding
`$608E`, `$6095`, and `$6096` writes prepare the command's operands.

The local mapping PROM entry is:

```text
mapping[0x14] = 0xBD
```

Atari `MBUDOC` names offset `$14` `SZXD` (“Z/X′ divide”); Atari's
`MBUCOD.V05` has the corresponding `DIVZ` entry, which falls through to the
signed divide routine. Therefore, the local coordinate command starts at
micro-PC `$BD`. The duplicate magnitude byte written to `$6094` is the
command-start write's data bus value; the `$BD` trace's operand setup uses the
already prepared registers, while the preceding `$608E` write supplies the
magnitude byte.

The local mapping entries for the other writes are `map[0x0E]=$35`,
`map[0x15]=$2E`, and `map[0x16]=$2F`; those are the register-load micro-routines,
not the coordinate divide entry.

## PROM assembly and 24-bit control word

The mapping PROM `136002-126.a1` is 32 bytes / 32x8. It returns an 8-bit
starting micro-PC. The microcode consists of 256 words, each 24 bits, from
the six 256-byte local PROM files' low nibbles:

| Device | Nibble contributes | Word bits |
|---|---:|---:|
| `136002-127.e1` | 0 | 3–0 |
| `136002-128.f1` | 1 | 7–4 |
| `136002-129.h1` | 2 | 11–8 |
| `136002-130.j1` | 3 | 15–12 |
| `136002-131.k1` | 4 | 19–16 |
| `136002-132.l1` | 5 | 23–20 |

Assembly is `word[pc] = Σ ((PROM[k][pc] & 0x0F) << (4*k))`. This is the
low-nibble-first ordering documented by Atari's PROM inventory and
`MBUCOD.V05` output format. No data-bit inversion or extra permutation is
needed for the local bytes. Physical active-low pin polarity was not checked
against an actual schematic; the claim is specifically that the documented
logical PROM data nibbles assemble directly.

Structural checks on the assembled local PROMs passed:

- `uPC $20 = $1073B4`, matching Atari's `RSTO` / `LDL 0 LDAB 16 JMP` source
  word.
- For `n=0..15` except the documented special slot `n=6`,
  `uPC $10+n = $0n3B10`, matching Atari's `RDISP` source words.
- `map[$14] = $BD`.

The bit fields are:

| Bits | Name | Meaning |
|---|---|---|
| 23–20 | A | 4-bit Am2901 A-register address; also jump target high nibble |
| 19–16 | B | 4-bit B-register/writeback address; also jump target low nibble |
| 15 | I2HI | source-select I2 for upper two Am2901 slices |
| 14 | I2LO | source-select I2 for lower two slices |
| 13–12 | I1,I0 | remaining shared source-select bits |
| 11 | STALL | stop clock after this microinstruction |
| 10–8 | I5–I3 | Am2901 ALU function |
| 7 | LDAB | load 8-bit jump-target latch from A:B |
| 6–4 | I8–I6 | destination/writeback and shift mode |
| 3 | SIGN | select MSB* = OVR xor F15; otherwise MSB*=0 |
| 2 | JMP | branch to target latch when MSB*=0 |
| 1 | MULT | conditionally invert I1 using prior Q0 |
| 0 | CARIN | ALU carry-in |

The A and B addresses select the 16-word by 16-bit register file made from
four Am2901 slices. B selects the RAM writeback register. The independent
high/low I2 control bits permit byte-specific selection of the host D bus.

## Am2901 and Atari control semantics

The AMD Am2900 data book supplies the Am2901A source/function/destination
tables; Atari `MBUDOC` specifies how the Math Box wires those controls:

| I2:I1:I0 | R source | S source |
|---:|---|---|
| 000 | A | Q |
| 001 | A | B |
| 010 | 0 | Q |
| 011 | 0 | B |
| 100 | 0 | A |
| 101 | D | A |
| 110 | D | Q |
| 111 | D | 0 |

Function codes I5:I3 are `R+S`, `~R+S`, `R+~S`, `R|S`, `R&S`, `~R&S`,
`R^S`, and `~(R^S)`. CARIN participates in arithmetic functions; the Atari
assembly rules use carry-in 1 for subtraction where needed. Carry ripples
through the four slices. The Am2901 overflow is the XOR of carries into and
out of the sign bit. The logical-operation OVR signal is not a valid general
sign/branch condition; Atari documents arithmetic use for SIGN/ASR.

| Destination I8:I6 | Effect |
|---:|---|
| 000 QREG | write ALU result to Q |
| 001 NOP | no RAM/Q write |
| 010 RAMA | write result to RAM[B], drive output Y from RAM[A] |
| 011 RAMF | write result to RAM[B], drive output Y from ALU F |
| 100 RAMQD | write RAM[B], then right shift concatenated RAM[B]:Q |
| 101 RAMD | write RAM[B], then right shift RAM[B] |
| 110 RAMQU | write RAM[B], then left shift concatenated RAM[B]:Q |
| 111 RAMU | write RAM[B], then left shift RAM[B] (low input is floating/garbage) |

For right shifts, the ALU result bit 0 enters Q bit 15 in RAMQD; the RAM
MSB receives MSB*. Q0 is latched for the following instruction. For left
shifts, Q0 is forced low; RAMQU transfers Q's MSB into RAM[B]'s low bit.
The host D bus is connected as the 8-bit input to both byte halves of the
16-bit datapath. The coordinate trace does not reach RAMU or use MULT.

MULT implements Atari's CADD trick: when MULT=1 and the prior Q0 is zero,
invert I1 so an add of A+B becomes A+0 (selectively suppressing the addend
during shift/multiply steps).

LDAB stores A:B as an 8-bit jump target. A jump is taken when JMP=1 and
MSB*=0; otherwise the 8-bit micro-PC increments. SIGN=0 makes MSB*=0 and
therefore permits an unconditional branch. With SIGN=1, MSB* is
OVR xor F15 and the branch tests that condition. STALL=1 halts after the
current instruction; a later 6502 write restarts the sequencer and loads a
new dispatch-PROM start address.

## Bounded coordinate trace

The temporary decoder read only the seven local PROM files. Each command
had a 512-instruction cap and failed on unknown SIGN/OVR or RAMU semantics.
The coordinate command halted in every run; no unknown control condition was
encountered. A preserved ignored TSV contains five complete traces (397
microinstruction rows), with raw word, decoded fields, selected R/S values,
F/OVR/MSB*, Q/Q0 transition, jump, and next PC:

`AI-Logs/Reports/2026-11-15-external-mathbox-coordinate-uPC-trace.tsv`

Representative reachable control words:

| uPC | Raw word | Relevant control |
|---:|---:|---|
| BD | `ACC030` | A=`A`, B=`C`, source A:Q, RAMF setup |
| C1 | `0D203C` | SIGN/JMP conditional setup; can branch to C8 |
| CE | `D2C09C` | SIGN/JMP, target latch load; routes divide branches |
| CD | `CD1131` | subtraction trial, RAMF |
| D0 | `0F313C` | conditional subtract/branch loop |
| D2 | `0D3060` | left-shift update |
| D8 | `002810` | STALL=1, NOP destination; command completes |

Across tested branch cases, the reachable PCs are
`BD, BE, BF, C0, C1, C8, C9, CA, CC, CD, CE, CF, D0, D1, D2, D3, D4, D5,
D6, D8`. All reachable control fields are decoded by the Atari/AMD
documentation. The loop completes at `$D8` with STALL set.

## Transfer derivation and arithmetic limits

For the normal `$C098` range, `magnitude` is an unsigned byte and `depth` is
the positive 16-bit X′ input formed from `pointDepth + lev_scale`. Exact
integer behavior established from the local PROM execution is:

```text
function M(magnitude: uint8, depth: uint16) -> uint16:
    if depth == 0:
        return 0xFFFF
    return floor((uint32(magnitude) * 128) / depth)
```

The hardware divide produces an unsigned quotient. Mathematically, for
positive depth it is the ratio `magnitude/depth` represented with a factor
of 2^7. Integer division truncates toward zero (floor for these
nonnegative operands); there is no rounding. With `magnitude<=255` and
`1<=depth<=283`, the maximum output is 32,640 (`$7F80`), so the output fits
without wrap or saturation. For depth zero the microcode returns `$FFFF`;
this is not a protective divide-by-zero result and is outside the production
coordinate path. `$C098` clamps a negative formed depth to one, and the
positive `lev_scale` ensures ordinary nonnegative point depths produce
positive depth.

The transfer returns two bytes, read low then high from `$6060/$6070`. Signs
are not carried through the divide: the CPU restores each source component's
sign afterward, then applies signed `lev_y2d` to the source-Y-derived result
with signed-16 saturation. Both X and Y magnitudes use the same prepared
register path and command `$14`; their transfer semantics are identical.

Prior-state handling: Atari code at `$C1C3` writes zero to the Z operand
bytes and the relevant Math Box operand registers, then writes `N=15`.
The local execution modeled that setup, as well as `$C098`'s X′ and
magnitude writes. Sequential X/Y requests matched fresh-state requests.
Eight arbitrary pre-setup RAM/Q/jump states also converged after those
documented clearing writes. The coordinate transfer therefore has no
observed dependency on prior operation state after the documented
initialization.

## Local numeric oracle and independent cross-check

An exhaustive bounded run executed all 256 magnitudes at every positive
depth from 1 through 283: **72,448 local PROM executions**, all halted within
512 instructions and all matched `floor(128*magnitude/depth)`. It also
verified 25 representative vectors against MAME's separate high-level
command-`$14` restoring-divide implementation. Those were direct numerical
agreements; MAME is an independent behavioral cross-check, not an
independent microcode decoder.

Selected local vectors (all outputs unsigned; `lo`/`hi` are bytes; every
row halted):

| Magnitude | Depth | Lo | Hi | Unsigned | Instructions |
|---:|---:|---:|---:|---:|---:|
| 0 | 12 | `00` | `00` | 0 | 77 |
| 1 | 13 | `09` | `00` | 9 | 78 |
| 64 | 12 | `AA` | `02` | 682 | 82 |
| 64 | 13 | `76` | `02` | 630 | 83 |
| 255 | 13 | `CE` | `09` | 2,510 | 84 |
| 64 | 15 | `22` | `02` | 546 | 80 |
| 64 | 16 | `00` | `02` | 512 | 78 |
| 64 | 20 | `99` | `01` | 409 | 81 |
| 64 | 23 | `64` | `01` | 356 | 81 |
| 64 | 24 | `55` | `01` | 341 | 81 |
| 64 | 25 | `47` | `01` | 327 | 81 |
| 64 | 28 | `24` | `01` | 292 | 80 |
| 64 | 29 | `1A` | `01` | 282 | 81 |
| 64 | 32 | `00` | `01` | 256 | 78 |
| 64 | 36 | `E3` | `00` | 227 | 81 |
| 64 | 140 | `3A` | `00` | 58 | 81 |
| 64 | 143 | `39` | `00` | 57 | 80 |
| 64 | 152 | `35` | `00` | 53 | 80 |
| 64 | 156 | `34` | `00` | 52 | 80 |
| 64 | 267 | `1E` | `00` | 30 | 81 |
| 64 | 270 | `1E` | `00` | 30 | 81 |
| 64 | 279 | `1D` | `00` | 29 | 80 |
| 64 | 283 | `1C` | `00` | 28 | 80 |
| 255 | 12 | `A0` | `0A` | 2,720 | 81 |
| 255 | 28 | `8D` | `04` | 1,165 | 81 |
| 255 | 283 | `73` | `00` | 115 | 81 |
| 0 | 0 | `FF` | `FF` | 65,535 | 92 |
| 1 | 0 | `FF` | `FF` | 65,535 | 92 |
| 255 | 0 | `FF` | `FF` | 65,535 | 92 |
| 1 | 1 | `80` | `00` | 128 | 78 |
| 255 | 1 | `80` | `7F` | 32,640 | 85 |

The independent cross-check used the Tempest memory-map dispatch and
MAME's high-level `go_w` command `$14` implementation, not MAME as the
microcode authority. The MiSTer hardware RTL independently agrees on the
PROM nibble order and decoded control-field boundaries. The Atari C-port
generator's source-word assertions also passed against the local PROM
assembly; its call-by-call port claims are secondary evidence, not a second
physical-board measurement.

## Representative level coordinate-stage examples

These are deliberately synthetic examples, not points recovered from vector
ROM data or arcade observation. They use the same byte point for each level:
`pointX=160`, `pointY=128`, `pointDepth=40`; `callerBaseX=0` is chosen only
to make the source-X-derived component numeric. Signed 13-bit words are
shown before any DVG-to-host conversion.

| Level | `D=40+lev_scale` | `dy=pointY-lev_y3d` | `M(|dy|,D)` | `lev_y2d + sign(dy)*M` → DVG X | `dx=+32` | `M(32,D)` → DVG Y |
|---:|---:|---:|---:|---:|---:|---:|
| 1 | 64 | +48 | 96 | -192 + 96 = **-96** (`$1FA0`, signed 13-bit) | +32 | **+64** (`$0040`) |
| 4 | 55 | +24 | 55 | -128 + 55 = **-73** (`$1FB7`) | +32 | **+74** (`$004A`) |
| 7 | 64 | +24 | 48 | -144 + 48 = **-96** (`$1FA0`) | +32 | **+64** (`$0040`) |
| 8 | 64 | -48 | 96 | +96 - 96 = **0** (`$0000`) | +32 | **+64** (`$0040`) |
| 9 | 52 | -48 | 118 | +320 - 118 = **+202** (`$00CA`) | +32 | **+78** (`$004E`) |

The source-Y-derived word is serialized as DVG vector X; the source-X-derived
word is serialized as vector Y, with signed 13-bit packing (high three bits
dropped). These results do not establish physical positive direction,
portrait monitor rotation, vector origin, scale, or host-pixel conversion.

## DVG/orientation findings

The prior local-source result remains intact: Atari's vector-list helper
serializes the source-Y-derived result as vector X and the source-X-derived
result as vector Y. Signed 13-bit packing is documented by the local
disassembly. The accessed Atari Math Box control references do not establish
physical monitor orientation, vector origin, positive axis direction, or
source-unit-to-host-pixel scale. No C# pixels are inferred.

## Evidence classification and remaining milestone

- **ROM-BACKED:** Atari's `MBUCOD.V05` and `MBUDOC.DOC`, together with the
  verified local PROM bytes, establish command index `$14`, mapping value
  `$BD`, the DIVZ/signed-divide structure, and the decoded local control
  words. Exhaustive local PROM execution establishes the exact
  `floor(128*magnitude/depth)` output over the production input domain.
- **REFERENCE-DERIVED:** The MAME numeric comparison and MiSTer RTL
  structural agreement are secondary implementation cross-checks, not Atari
  authority. The Atari C-port is an additional engineering comparison.
- **DEVELOPMENT-ONLY:** The temporary PROM decoder and preserved TSV trace
  are investigation tools; they are not claims about normal hardware
  instrumentation or behavior beyond the decoded execution.
- **UNKNOWN:** The actual Sheet 3B schematic was not obtained, so no direct
  schematic-backed conclusion is claimed. Physical axis orientation,
  monitor rotation, vector origin, and vector-to-host units remain
  unresolved.

The smallest next milestone is a separate, bounded DVG orientation/vector
unit investigation using a numeric vector-list reference tied to known
physical center and positive-axis directions. It is not required to close
the Math Box transfer, but is required before mapping these results into
host pixels.

## Final state and protections

- Branch remains `master`; `HEAD` and `origin/master` remain
  `385f603a94391756b2ca6cccd524dc17b281e782`.
- Index remains clean.
- The same 16 tracked candidate files remain modified; the same two
  authorized metadata/test candidate files remain the only non-ignored
  untracked files.
- The only created project-local files are this ignored report, the ignored
  exact prompt copy, and the ignored bounded TSV trace.
- No production source, tests, metadata, documentation outside `AI-Logs`,
  ROM/PROM bytes, or candidate work were modified.
- Nothing was staged, committed, or pushed.
- No visual, Gemini, or Incitti material was inspected.
- No build, test suite, or game was run; this was a bounded source/PROM
  decoding investigation.
