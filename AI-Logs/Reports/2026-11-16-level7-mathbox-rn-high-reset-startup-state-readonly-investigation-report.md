# Level 7 Math Box RN.high Reset and Startup-State Investigation

**Status: COMPLETE — RN.high unresolved at the first normal record-06 DIVZ.**

**Required classification: UNDEFINED OR UNRESOLVED AT FIRST LEVEL 7 DIVZ**

## Executive finding

The local Atari documents and ROM/microcode sources establish how normal
software writes `RN.low`, and establish that `RN` is a 16-bit microcode
register. They do not establish the value or reset behavior of `RN.high`.
Normal Level 7 initialization writes `$0f` to the low byte only. Neither the
normal DIVZ operation nor the other identified ordinary coordinate operations
modify RN.high. Thus, at the first normal record-06 DIVZ, RN.high is whatever
the Math Box power-on/reset state or any unobserved external state left there;
the available evidence cannot say whether that value is zero.

The exact ROM operand for the NL write is `$608C` (`STA $608C`); `$0C` is the
documented interface offset. Prior notes' `$600C` notation is not the literal
operand in the inspected ROM instructions.

This is not a finding that hardware leaves RN.high undefined. No local
primary source describes the register file's power-on or hardware-reset
contents, or demonstrates whether the external reset circuitry clears it.
No project-local trusted Math Box emulator/device model or bounded register
capture was available.

## Evidence sources and scope

Primary local sources inspected:

- `Reference/arcade/MBUDOC.DOC`: interface-offset table and architecture/
  register-file descriptions (pages 2-4).
- `Reference/arcade/MBUCOD.V05.txt`: register definitions, register-store
  mapping, power-on diagnostic, RDISP/CLRNH, DIVZ/DIVID, CLIP, and mapping
  PROM source.
- `Reference/arcade/Tempest Rom Source code.md`: CPU reset at `$D940`,
  normal level loop and `$B1B6`/`$C1C3` initialization, `$C098` coordinate
  routine, `$BD3E` temporary count operation, and Math Box accesses in test
  code.

Prior local reports used as navigation and to confirm the bounded prior
findings, not as primary proof:

- `AI-Logs/Reports/2026-11-15-atari-mathbox-c098-reconciliation-report.md`
- `AI-Logs/Reports/2026-11-15-external-mathbox-hardware-prom-decode-report.md`
- `AI-Logs/Reports/2026-11-16-level7-mathbox-normal-callstate-readonly-investigation-report.md`

The Math Box PROM bytes, ROM binaries, screenshots, recordings, and videos
were not opened. No external search, interactive game launch, test, build,
or temporary calculation was needed. A bounded source-tree filename check
found no project-local Math Box emulator/device model. A related test contains
controlled quotient examples and a prototype-assumption projection case; it
does not model Math Box power-on/reset or retained RN state. The metadata
source comment likewise describes the translation as inert. Neither is
evidence of Atari RN.high.

## Reset and startup trace

1. **Power-on state:** Neither the Atari Math Box interface document nor the
   reviewed microcode source specifies the initial value of the register
   file or RN.high on power-up.
2. **Math Box hardware reset:** No local source inspected specifies a Math
   Box register-file reset signal or its effect on RN. The microcode source
   has a section labeled `POWER-ON DIAGNOSTIC`, but its label does not prove
   that the diagnostic runs automatically on a board reset. Its body writes
   Q and registers 0/1; it does not initialize RN.high.
3. **CPU reset:** The Tempest ROM reset routine at `$D940` writes the
   watchdog and `$5800` (`vg_reset`), clears CPU/vector RAM, and continues
   through game initialization. This establishes a vector-generator reset
   write, not a Math Box register reset. The code contains no Math Box
   reset write in that reset sequence. Absence of such a CPU write does not
   establish what an external electrical reset does to the Math Box.
4. **Normal game entry:** The main loop calls `$B1B6`; that routine calls
   `$C1C3` before the normal presentation work. `$C1C3` clears the four
   bytes of Z and writes `$0f` to NL/RN.low. It does not write RN.high.
5. **Normal Level 7 construction:** The established normal path selects
   record 06, then `$C098` invokes DIVZ for each coordinate axis. Each
   DIVZ starts from the retained RN value. The ordinary path does not
   establish the initial high byte before its first record-06 coordinate
   call.

The power-on diagnostic title, ROM reset code, and `$C1C3` software
initialization therefore establish different things; none proves a
zero-valued RN.high at the first Level 7 call.

## RN width, mapping, and microcode semantics

- MBUCOD defines a 16-word register file used by four 2901 slices and
  assigns `RN = RY + 1` (register 6). RN is a full 16-bit register; its
  upper byte exists as retained register-file state. Atari's microcode
  explicitly names and operates on the high byte in `CLRNH`.
- The host interface identifies offset `$0C` as `NL`, the number of
  quotient bits. In MBUCOD's mapping table, offset `$0C` maps to
  `2*RN+REGS`; the register-store sequence makes this `LDL RN`, a low-byte
  write. The next offset, `$0D`, maps to the low-byte write of RZL, not
  to `LDH RN`. The documented host mapping therefore provides no direct
  byte write to RN.high.
- The initialization instruction is `STA $608C` with `$0f`, the ROM
  operand for the interface's NL offset `$0C`. Its microcode mapping writes
  RN.low and leaves RN.high untouched; it is not a full-word RN
  initialization.
- DIVZ enters DIVID, whose microcode copies RN to work register T3 and
  decrements T3 during the quotient loop. It does not decrement or clear
  RN. The copy and loop use the 16-bit register datapath, so RN.high would
  participate in the work count and branch/termination behavior if
  nonzero. The `$000f` count produces the previously traced 16-cycle
  operation; a nonzero high byte is not equivalent to that count.
- Two other generic microcode paths can affect RN: the RDISP sequence
  enters `CLRNH`, which explicitly clears RN.high; the CLIP routine
  decrements RN itself. They are real microcode capabilities, not proof
  that either runs in ordinary Tempest gameplay.

## Pre-coordinate operations and retained state

The ROM's ordinary Math Box writes and the relevant microcode establish:

- `$C1C3` writes RN.low `$0f`; it does not write RN.high.
- `$C098` writes the divisor and Z magnitude and starts DIVZ. DIVZ copies
  RN into T3, then changes T3, not RN.
- `$BD3E` temporarily writes `$18` to NL/RN.low, performs a DIVZ, and
  restores `$0f`. These writes do not establish or alter RN.high.
- Ordinary repeated coordinate DIVZ calls therefore preserve RN.high
  across axes, points, and the far/near passes, provided no separate
  register-display or clipping entry is invoked.
- The generic RDISP/CLRNH and CLIP routines are mapped in the Math Box
  microcode, but the inspected Tempest ROM has no write to their
  corresponding mapped entry points (`DXP`/`DYPL`/`DYPH` or `YHSC`).
  They are not observed in the ordinary startup/coordinate path.
- Separate test code around `$DCE6` writes `$10` to NL and starts a
  DIVZ-like operation. This changes RN.low only and is not evidence of
  normal campaign startup state. The local ROM source does not establish
  that this test path runs in the ordinary campaign.

No identified ordinary startup or coordinate micro-operation clears,
normalizes, copies back, or overwrites RN.high. This does not exclude
unmodeled board reset behavior or a path not represented by the reviewed
normal ROM flow.

## First Level 7 call and startup-path comparison

At the first normal record-06 DIVZ:

- RN.low is established as `$0f`.
- RN.high is **unresolved**. The state could be reset-defined or otherwise
  retained; the local evidence does not distinguish those possibilities.
- Subsequent ordinary axes, points, and near/far passes preserve that
  high byte because DIVZ uses T3 as its work counter and no normal-path
  RN.high-changing operation was found.

In the normal Atari campaign path, the repeated coordinate DIVZ calls do
not by themselves change RN.high. The sources inspected do not establish
an Atari direct-Level-7 selection/service path, so they cannot show whether
such a path would run the same initialization or have the same reset
history. The current C# direct-level diagnostic is not an Atari startup
path and does not model RN as retained Math Box state. Equivalence between
natural campaign progression and direct Level 7 construction is therefore
not established.

## Conditional numeric and geometry consequence

If RN.high is zero, RN is `$000f` at the first normal Level 7 call and the
prior local PROM trace's `floor(128 * magnitude / depth)` result applies
to that call. The preceding factor-128 table is then supported for normal
Level 7, subject to the stated call-state inputs.

If RN.high is nonzero, the 16-bit RN-to-T3 copy changes the quotient-loop
count/termination; the `$000f` factor-128 trace cannot be applied as-is.
The current C# factor-256 result is not established as the hardware result
for a nonzero high byte either. No PROM execution was performed for a
hypothetical RN value because the actual value remains unknown.

Accordingly, the Level 7 geometry discrepancy still has no established
normal Atari cause. The factor-of-two explanation remains conditional, and
there is not enough evidence for a bounded correction implementation
prompt.

## Evidence classifications and confidence boundaries

| Conclusion | Evidence classification | Confidence boundary |
|---|---|---|
| Tempest reset code writes `vg_reset`, while normal initialization stores `$0f` to the NL low-byte mapping. | ROM-BACKED | High for the instructions and mapped CPU writes in the local disassembly; does not establish external board reset wiring. |
| NL offset `$0C` selects `LDL RN`; RN is a full 16-bit microcode register and DIVID copies it to T3. | ATARI-DOCUMENTED | High for the Atari interface/microcode description and register operations; not a measurement of a particular board's power-on value. |
| The generic RDISP/CLRNH and CLIP microcode can change RN.high/RN, respectively. | ATARI-DOCUMENTED | High for those microcode paths; the ordinary Tempest ROM path inspected does not invoke their mapped entry points. |
| RN.high at the first natural Level 7 DIVZ is unknown; ordinary coordinate DIVZ calls preserve it. | UNKNOWN for the value; ROM-BACKED for preservation | The available local evidence cannot distinguish a reset-to-zero value from other initial or externally retained state. Preservation applies to the traced ordinary path, not unmodeled service or hardware behavior. |
| The factor-128 result and geometry explanation apply if RN.high is zero. | ROM-BACKED, conditional | The prior bounded PROM trace establishes the result for RN=`$000f`; the unresolved runtime RN.high prevents unconditional attribution to Level 7. |
| The test's controlled quotient cases establish Atari reset state. | DEVELOPMENT-ONLY; not supported as Atari evidence | The test covers controlled arithmetic/prototype values and has no retained hardware register model. |

## Narrowest additional evidence needed

Obtain one bounded, hardware-validated register trace for an ordinary
natural campaign startup that exposes RN[15:8] (register-file word 6):
record it after Math Box power-on/reset, after the CPU's NL write
(`$608C = $0f`), and immediately before the first normal record-06
`$6094`/offset `$14` DIVZ write. Those observations distinguish reset
state from CPU initialization and retained pre-Level-7 state, and directly
resolve RN.high at the questioned call. No existing project-local trusted
capture path was found; this task did not add one.

## Preservation, validation, and completion

- **Repository baseline:** branch `master`; HEAD
  `385f603a94391756b2ca6cccd524dc17b281e782`; tracking `origin/master`;
  index empty. The starting worktree contained 17 modified tracked paths
  and 13 non-ignored untracked paths. All were pre-existing and preserved.
- **Changes:** no source, tests, project configuration, durable
  documentation, protected reference material, or Git state was changed.
  No geometry, projection, viewport, presentation, or gameplay code was
  changed. The only created files are the required ignored prompt/report
  under `AI-Logs/`; no files were staged or committed.
- **Validation:** read-only source/microcode cross-check completed. No
  build or tests were applicable. Final Git-state verification is recorded
  after this report was written.
- **Required prompt:** `AI-Logs/Prompts/2026-11-16-level7-mathbox-rn-high-reset-startup-state-readonly-investigation-prompt.md`
- **Report:** `AI-Logs/Reports/2026-11-16-level7-mathbox-rn-high-reset-startup-state-readonly-investigation-report.md`
