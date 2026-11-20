# Tempest Atari Math Box Mode `$0f` Investigation

## Executive conclusion

**Mode `$0f` was not resolved.** The project-local Atari evidence establishes the CPU-visible register sequence used by `$c098`, but does not establish the Math Box operation or its numeric semantics. This investigation reached the useful limit of the available local evidence; the `$c098` result is not yet numerically reconstructable.

## Evidence located

- The handoff, [2026-11-15-atari-projection-data-mathbox-investigation-report.md](./2026-11-15-atari-projection-data-mathbox-investigation-report.md), was used as a guide. Its recovered geometry tables were not repeated.
- The only project-local Atari reference source located outside the raw ROM directory is [Tempest Rom Source code.md](../../Reference/arcade/Tempest%20Rom%20Source%20code.md). It is a ROM-derived disassembly, not original Atari documentation. Its opening note explicitly warns that material may be wrong or missing. Instruction bytes and traced CPU behavior are treated as disassembly-backed; annotations are not promoted to Atari fact.
- A bounded inventory of `Reference\arcade\roms.rev2` found eight 2,048-byte files (`136002-113.d1`–`136002-121.p1`, excluding 117; specifically 113, 114, 115, 116, 118, 119, 120, 121), eight 256-byte files (`136002-125.d7`, `127.e1`–`132.l1`), one 32-byte file (`136002-126.a1`), and three additional 2,048-byte files (`136002-217.j1`, `222.r1`, `316.h1`). This was filename/size metadata only: **no ROM/PROM bytes were opened or decoded**.
- No local Atari schematic, PROM-to-function mapping, or separate Math Box hardware description was found in the reference inventory. The small ROM files cannot be attributed to the Math Box from their filenames and sizes alone. Decoding an unidentified file would not establish a Math Box control path, so no PROM was selected for byte-level inspection.
- The Incitti reference implementation was not used as evidence.

## Mode `$0f` trace

The disassembly shows the following CPU-visible behavior:

1. Initialization writes `$0f` to `$600c` (`mb_w_0c`) at `$c1f7-$c1f9`.
2. The `$c098` routine does not write `$600c`. It prepares its operands while that previously selected control value is active.
3. A separate path writes `$18` to `$600c` and later restores `$0f` (`$bd51-$bd6f`). A separate self-test helper writes `$10` and reads the Math Box result (`$dce6-$dd0c`). Its disassembly annotation says it appears to divide, while explicitly noting uncertainty about how the Math Box works. Neither path defines mode `$0f`.
4. No local Atari source found in this investigation decodes `$0f` into an arithmetic operation, microprogram, or state sequence.

The `$dce6` annotation is an **annotation/interpretation**, not an Atari-documented operation definition. It cannot safely be generalized from mode `$10` to `$0f`.

## Register semantics

The following are **DISASSEMBLY-BACKED CPU bus and software semantics only**:

- `$600e` and `$6014` are each written with the same one-byte absolute axis difference. `$c098` first writes the Y difference, polls `$6040` while bit 7 is set, and reads `$6060/$6070`; it then writes the X difference, polls again, and reads the same result addresses.
- `$6015/$6016` receive a two-byte CPU-prepared value before both axis passes. The low byte is computed by subtracting `$5f` from `$57`; the high byte is computed by subtracting `$5b` from zero with the borrow. If the high-byte result is negative, the routine substitutes `$0001`.
- `$6060/$6070` are read as low and high result bytes. The disassembly establishes a 16-bit CPU readout, but not the Math Box's internal result width or how it maps to those bytes.
- `$6040` is polled as a busy/ready condition. The disassembly does not establish how the underlying arithmetic is initiated or sequenced internally.

The software produces one Y magnitude from the absolute difference between `$58` and `y3d`, and one X magnitude from the absolute difference between `$56` and `$5e`; it records the side of each comparison separately. Setup loads `$5f` as the negation of the selected scale byte and sets `$5b` to `$ff` (`$c235-$c258`). These facts establish the submitted byte values and their CPU-side preparation, not the Math Box's interpretation of them.

## `$c098` reconciliation

The first Math Box read is placed in `$63/$64`; the second is placed in `$61/$62`. The routine applies Y sign and offset handling on the CPU, then X sign handling, with signed-overflow saturation to `$7fff` or `$8000`. Setup initializes the X offset pair to zero. This confirms that the Math Box output is consumed as a two-byte value before CPU-side coordinate handling; it does not establish that the Math Box result is a signed magnitude, a fixed-point value, a quotient, or any particular projection formula.

## Numeric transformation and verification

No evidence-grounded equation from the submitted operands to the `$6060/$6070` result can be stated. In particular, the available local evidence does not establish operand signedness, intermediate width, fixed-point scaling, truncation or rounding, or result saturation inside the Math Box. There are therefore no valid numeric examples to calculate.

## Evidence classifications

- **ATARI-DOCUMENTED:** None located for Math Box mode `$0f`.
- **ROM/PROM-BACKED:** None newly established. Raw ROM/PROM bytes were not inspected in this investigation.
- **DISASSEMBLY-BACKED:** Mode `$0f` is written during initialization; `$c098` leaves the control register unchanged, writes the operands described above, polls the status register, reads the two result bytes, and performs CPU-side sign/offset/saturation handling. The separate helper writes mode `$10`.
- **STRONG INFERENCE:** The Math Box performs or sequences some hardware operation behind the CPU-visible control, operand, status, and result registers. The actual operation cannot be identified from this evidence.
- **HYPOTHESIS:** No hypothesis is used as a conclusion. In particular, division, multiplication, and conventional perspective math are not established for mode `$0f`.
- **UNKNOWN:** The operation selected by `$0f`; register-field roles beyond the observed CPU writes; internal arithmetic and state; signedness; precision; scaling; and rounding/truncation.

## Local-evidence stopping assessment

The useful limit of the available project-local evidence has been reached. The ROM-derived disassembly gives the CPU-side trace but no Math Box implementation. Although small raw ROM/PROM files are present, there is no local mapping tying a particular file to the Math Box or to control `$0f`; guessing a file and decoding it would exceed the evidence boundary. The operation remains unresolved.

## Next recommendation

The smallest useful next evidence would be original Atari hardware documentation that maps the Math Box control and operand registers to the implementing logic/PROMs, including the decode or microsequence for `$0f`. If that mapping identifies a local PROM, its relevant entries could then be examined in a separate, bounded investigation. No external search was conducted.

## Scope and completion

- Report: `AI-Logs/Reports/2026-11-15-atari-mathbox-mode0f-investigation-report.md`.
- Files created: this report only.
- Atari/reference material read: the handoff report and the relevant declarations, `$c098` routine, setup, mode writes, and divide-helper portions of `Reference/arcade/Tempest Rom Source code.md`. Targeted searches also found no Math Box operation definition in the behavior spec or development notes.
- Raw ROM/PROM inspection: **none**. The filenames and file sizes in `Reference/arcade/roms.rev2` were inventoried only; no byte ranges were read.
- Commands used: bounded `rg` searches and PowerShell file-list/size metadata queries. No scripts, builds, tests, game launch, Internet search, or C# implementation analysis were used.
- No tracked files were modified. The pre-existing worktree changes were left untouched. Nothing was staged, committed, or pushed.
- Most important newly established fact: the `$c098` trace proves the CPU-side operand writes, 16-bit result reads, and subsequent coordinate handling, but not the Math Box operation behind mode `$0f`.
- Most important remaining unknown: the exact arithmetic and numeric convention selected by Math Box mode `$0f`.
