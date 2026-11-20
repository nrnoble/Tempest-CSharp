# Level 7 Math Box Normal Call-State Investigation

**Outcome: PARTIAL / NORMAL RN HIGH BYTE UNRESOLVED.** The normal Level 7 call
path sets the Math Box quotient-count low byte to `$0f` and invokes the
`DIVZ`/`Z/X'` operation for each axis and point. Atari's local Math Box PROM
trace gives `floor(128 * magnitude / depth)` when the count register is
`$000f`. The prototype instead uses `floor(256 * magnitude / depth)`. However,
the normal CPU path does not establish the high byte of `RN`, so the `$000f`
PROM result cannot yet be promoted to the unconditional result of every
normal Level 7 call.

## Executive findings

- `$0f` is not a Math Box operation mode. Atari's interface documentation
  identifies offset `$0c` as `NL`; the game writes `$0f` there as the low byte
  of the quotient-bit count `RN`. The per-axis operation is selected by the
  `$6094` write (offset `$14`, `SZXD`), which dispatches to `DIVZ` / `Z/X'`.
- The Level 7 path prepares `X' = p + lev_scale` and writes each unsigned
  axis magnitude to `ZLH`. The local PROM trace establishes the quotient
  `floor(128 * magnitude / X')` for `RN=$000f`, not the prototype's factor
  of 256.
- Startup writes `RN.low=$0f`; neither that write nor `$c098` writes
  `RN.high`. The code copies `RN` to a work counter and decrements the work
  counter, leaving `RN` itself intact. Its high byte/reset state is therefore
  not established by the reviewed normal software path. No nonzero value is
  inferred.
- The conditional factor-of-two error is a strong candidate for the observed
  structural mismatch, but it is not confirmed as the normal hardware result
  until `RN.high` is resolved.
- Level 7's ROM target Y2D is `$ff70` (-144). The prototype applies it to the
  source-Y result after projection, matching the CPU's axis/sign/base order.
  At the call site, however, the active `$68:$69` pair is copied directly from
  Y2D only when `$02==$1e`; the other branch retains transition state. Thus
  `-144` is exact for the direct-load branch and settled target, not proven for
  every construction call irrespective of transition state.

## Evidence and scope

- The project-local `Tempest Rom Source code.md` supplies the CPU instruction
  path and call sites. Its opening caveat is retained: instruction-flow
  conclusions are disassembly-backed; annotations are not promoted to Atari
  fact.
- Atari's local `MBUDOC.DOC` documents `NL`, `ZLH`, `SZXD`, the mapping-PROM
  interface, and the N-cycle divide. Atari's `MBUCOD.V05.txt` defines `RN`,
  `DIVZ`, the signed/unsigned divide path, and the work-counter loop.
- The existing local PROM-decode report supplies the bounded decode and
  execution of the identified Rev 2 Math Box PROM path. Its local execution
  established `floor(128*magnitude/depth)` for the `$000f` count, and its
  sampled high-level command-`$14` comparison agreed. The PROM bytes were not
  reopened for this investigation.
- The earlier `$c098` reconciliation report's `floor(256*magnitude/depth)`
  equation conflicts with the later PROM-backed N-cycle execution. It follows
  the written dividend as a full-precision divide but does not account for
  the actual quotient-cycle count. For this numeric question, the local
  PROM-backed trace is the stronger evidence.
- The existing table-extraction and Level 7 prototype reports were used for
  record-06 values and the current prototype outputs; geometry extraction,
  topology, screenshots, and viewport fitting were not revisited.
- No ROM/PROM bytes, screenshots, recordings, or videos were read here. No
  Internet search, game launch, build, or test run was performed. The numeric
  comparison below was a bounded calculation; no temporary project files
  were created.

## Normal Level 7 call path

1. The level setup at `$c235` obtains the displayed level, calls
   `get_tube_no`, and selects the `lev_remap` row. The existing ROM-backed
   table recovery establishes displayed Level 7 -> record index `$06`.
2. `$c235` loads that record's `lev_scale=$18` into the scale setup and stores
   its two's-complement negative in `$5f`; `$5b` is set to `$ff`. It loads
   `lev_y3d=$68` into `$60`, the camera-height operand. The 16 `lev_x` and
   `lev_y` bytes are copied into `tube_x` and `tube_y`.
3. The normal endpoint builder at `$c30d` first calls `$c473` with
   `$57=$f0` (far), and, when the near endpoint is required, later calls it
   with `$57=$10` (near). `$c473` iterates the 16 points, loads each point
   into `$56/$58`, and calls `$c098`.
4. `$c098` forms `X'` from zero-extended `$57` minus the signed `$5b:$5f`
   pair. With the normal `$ff:(-$18 mod 256)` level-scale pair, this is
   `p+$18`: divisor `$28` (40) at `$p=$10`, and `$108` (264) at `$p=$f0`.
   A negative computed depth is clamped by the ROM to one; that branch is
   irrelevant to these two positive Level 7 positions.
5. `$c098` computes `abs($58-$60)` for source Y and `abs($56-$5e)` for source
   X, where `$5e=$80`; it writes the Y magnitude to `$608e`, then writes the
   same byte to `$6094` to start the first divide. After the ready poll it
   reads the result to `$63:$64`, restores the Y sign, applies `$68:$69`, and
   saturates signed overflow. It then writes the X magnitude to `$608e` and
   `$6094`, reads the result to `$61:$62`, restores the X sign, and applies
   `$66:$67` (zeroed by setup).

## Math Box command and retained state

| State | Normal-path evidence | Conclusion |
|---|---|---|
| Operation | `$6094` is a write to offset `$14`; Atari documentation names it `SZXD`, and the local mapping/PROM path dispatches to `$BD` / `DIVZ`. | The coordinate operation is `Z/X'`, not a data-selected `$0f` mode. |
| `RN.low` | Math Box initialization at `$c1c3-$c1fc` writes `$0f` to `$600c`, offset `$0c` (`NL`). The separate `$bd3e` helper may temporarily write `$18`, then restores `$0f`. | `$0f` is established for the normal count low byte. |
| `RN.high` | Initialization writes the `Z` bytes at offsets `$0d-$10` and writes only the low `RN` byte at `$0c`. `$c098` does not write `$0c` or a high `RN` byte. | Unknown at the normal call; it is not established as zero or nonzero. |
| Divide work state | `DIVZ` loads the dividend and divisor work registers; the signed setup prepares `T1/Q/T2`; `DIVID` copies `RN` into `T3` and decrements `T3` during its N-cycle loop. | The loop updates its work counter, not `RN`; the divide's quotient/remainder work is prepared anew. |
| Dividend bytes | `$c1c3` clears offsets `$0d-$10`; each `$c098` call writes its current magnitude to offset `$0e` (`ZLH`). | For the normal initialized path, the first and later axis requests have the same zero remaining dividend bytes and a newly written magnitude. |
| Divisor | `$c098` writes both bytes of `$57+$18` to offsets `$15/$16` before the Y and X operations. | X and Y in a point call use the same divisor. |

The normal coordinate path does not access the register-display mappings that
reach `CLRNH` in `MBUCOD`; that separate micro-routine is not evidence that
`RN.high` is cleared by the `$600c` low-byte write. The local Atari material
reviewed here does not define the Math Box register-file power-on value, and
the normal CPU writes do not settle it. Possible retention is not evidence of
a retained nonzero value.

### Point, axis, and endpoint state

- The first point's other `Z` bytes are cleared by `$c1c3`; the subsequent
  points re-use those zero bytes and replace `ZLH` for each axis. The divide
  microcode repopulates its work state for each request.
- Y is projected first, then X. Both requests use the same `RN` state and
  divisor; the first result's CPU-side sign/base work does not write `RN`.
- The far pass is executed before the near pass in `$c30d`. The only relevant
  input difference between those passes is `$57` (and therefore the divisor).
  There is no normal-path write to `RN.high` between points, axes, or the two
  passes. The exact quotient behavior remains conditional on the unresolved
  high byte.

## Y3D and Y2D/base disposition

| Question | Finding |
|---|---|
| Extracted Level 7 target | The recovered metadata is `lev_y3d=$68`, Y2D bytes `$70/$ff`, signed `$ff70=-144`, and `lev_scale=$18`. These are ROM-backed table values in the existing extraction record. |
| Call-site active base | `$c235` loads Y2D low/high directly into `$68/$69` when `$02==$1e`. Otherwise it computes a transition increment in `$0121`; the value used by `$c098` is the current `$68:$69`, not unconditionally the table target. |
| Semantic use | `$c098` applies `$68:$69` only to the source-Y quotient, after restoring its sign. X instead uses the zero `$66:$67` base. |
| Prototype use | `AtariCoordinateProjector` adds `ScreenYOffset` to its signed source-Y result, then maps source-Y to the first DVG coordinate. That matches the ROM's post-divide Y-base placement and axis mapping for a call where `$68:$69=$ff70`. It does not model a transition-dependent active base. |

Therefore the table value and its signed interpretation are correct. Its
point-by-point semantic use in the prototype is correct for the direct-load /
settled-base state, but `$ff70` is not proven to be the active value at every
possible transition call.

## Conditional numeric reconstruction and prototype comparison

The local PROM report's exact result for the normal nonnegative operands when
`RN=$000f` is:

```text
depth = p + 24
q(magnitude, depth) = floor((128 * magnitude) / depth)
sourceX = sign(rawX - 0x80) * qX
sourceY = -144 + sign(rawY - 0x68) * qY
DVG = (sourceY, sourceX)
```

This is conditional: `RN.low=$0f` is established, but `RN.high=0` is not.
The prototype uses the same depth, signs, base placement, and DVG axis order,
but computes `floor(256*magnitude/depth)`.

Representative point 00 is `(rawX,rawY)=($ea,$a0)`, so `dx=106` and
`dy=56`. At the near depth 40, the conditional PROM result is
`qx=floor(106*128/40)=339`, `qy=floor(56*128/40)=179`; sign restoration and
the Y base give source `(339,35)` and DVG `(35,339)`. The current prototype
returns `qx=678`, `qy=358`, source `(678,214)`, DVG `(214,678)`. At far depth
264, the conditional result is `(qx,qy)=(51,27)`, source `(51,-117)`, DVG
`(-117,51)`; the prototype gives `(102,54)`, source `(102,-90)`, DVG
`(-90,102)`.

All 16 point outputs follow. Each cell shows quotient `(qx,qy)` and the
resulting DVG coordinate; the record-order raw bytes and magnitudes make the
sign and base application auditable. “PROM conditional” assumes `RN.high=0`;
“current” is the existing C# projector.

| Pt | Raw `(X,Y)` | `dx,dy` | Near PROM conditional `p=16` | Near current C# | Far PROM conditional `p=240` | Far current C# |
|---:|---|---|---|---|---|---|
| 00 | `EA,A0` | `106,56` | `339,179 -> (35,339)` | `678,358 -> (214,678)` | `51,27 -> (-117,51)` | `102,54 -> (-90,102)` |
| 01 | `E0,E0` | `96,120` | `307,384 -> (240,307)` | `614,768 -> (624,614)` | `46,58 -> (-86,46)` | `93,116 -> (-28,93)` |
| 02 | `9C,EA` | `28,130` | `89,416 -> (272,89)` | `179,832 -> (688,179)` | `13,63 -> (-81,13)` | `27,126 -> (-18,27)` |
| 03 | `80,B0` | `0,72` | `0,230 -> (86,0)` | `0,460 -> (316,0)` | `0,34 -> (-110,0)` | `0,69 -> (-75,0)` |
| 04 | `64,EA` | `28,130` | `89,416 -> (272,-89)` | `179,832 -> (688,-179)` | `13,63 -> (-81,-13)` | `27,126 -> (-18,-27)` |
| 05 | `20,E0` | `96,120` | `307,384 -> (240,-307)` | `614,768 -> (624,-614)` | `46,58 -> (-86,-46)` | `93,116 -> (-28,-93)` |
| 06 | `16,A0` | `106,56` | `339,179 -> (35,-339)` | `678,358 -> (214,-678)` | `51,27 -> (-117,-51)` | `102,54 -> (-90,-102)` |
| 07 | `50,80` | `48,24` | `153,76 -> (-220,-153)` | `307,153 -> (-297,-307)` | `23,11 -> (-155,-23)` | `46,23 -> (-167,-46)` |
| 08 | `16,60` | `106,8` | `339,25 -> (-169,-339)` | `678,51 -> (-195,-678)` | `51,3 -> (-147,-51)` | `102,7 -> (-151,-102)` |
| 09 | `20,20` | `96,72` | `307,230 -> (-374,-307)` | `614,460 -> (-604,-614)` | `46,34 -> (-178,-46)` | `93,69 -> (-213,-93)` |
| 10 | `64,16` | `28,82` | `89,262 -> (-406,-89)` | `179,524 -> (-668,-179)` | `13,39 -> (-183,-13)` | `27,79 -> (-223,-27)` |
| 11 | `80,50` | `0,24` | `0,76 -> (-220,0)` | `0,153 -> (-297,0)` | `0,11 -> (-155,0)` | `0,23 -> (-167,0)` |
| 12 | `9C,16` | `28,82` | `89,262 -> (-406,89)` | `179,524 -> (-668,179)` | `13,39 -> (-183,13)` | `27,79 -> (-223,27)` |
| 13 | `E0,20` | `96,72` | `307,230 -> (-374,307)` | `614,460 -> (-604,614)` | `46,34 -> (-178,46)` | `93,69 -> (-213,93)` |
| 14 | `EA,60` | `106,8` | `339,25 -> (-169,339)` | `678,51 -> (-195,678)` | `51,3 -> (-147,51)` | `102,7 -> (-151,102)` |
| 15 | `B0,80` | `48,24` | `153,76 -> (-220,153)` | `307,153 -> (-297,307)` | `23,11 -> (-155,23)` | `46,23 -> (-167,46)` |

Divisors are 40 for every near row and 264 for every far row. The table does
not include viewport mapping or apply any visual fitting.

## Diagnostic disposition

- **Current `RN.high=0` assumption: PARTIALLY SUPPORTED.** The `$0f` low byte
  is established. The high byte is not established by the normal CPU writes
  or by the reviewed Atari interface document.
- **Confirmed implementation behavior:** the C# projector multiplies each
  magnitude by 256 before division. The local PROM trace with `RN=$000f`
  yields a factor of 128. This is a confirmed conditional mismatch, not yet
  proof of the quotient used by normal Level 7 hardware.
- **Observed discrepancy cause:** not conclusively established. If the normal
  high byte is zero, the prototype doubles each projected magnitude before
  sign/base application, producing the demonstrated enlarged, point-dependent
  geometry. The unresolved high byte prevents elevating that cause from a
  strong conditional explanation to confirmed normal Atari behavior.
- **Narrowest remaining math uncertainty:** the value of `RN.high` when the
  first normal record-06 `DIVZ` call runs, including whether hardware reset or
  a prior documented startup operation establishes it. Resolve with primary
  Atari reset/register-state evidence or a bounded verified capture of that
  actual startup state; do not infer it from the C# default.
- **Correction prompt readiness:** not enough evidence for an Atari-exact
  correction implementation prompt. The next bounded task should resolve
  `RN.high`; it should not change geometry or viewport constants.
- **Smallest future correction direction:** if `RN.high=0` is established,
  replace the prototype's factor-256 transfer with the PROM-backed
  15-cycle factor-128 result and preserve the ROM's post-divide Y2D/sign
  handling. If not, model the verified normal count state rather than
  hard-coding that conditional equation.

## Preservation and completion

- Starting state: branch `master`, HEAD
  `385f603a94391756b2ca6cccd524dc17b281e782`, upstream `origin/master`, empty
  index. The worktree already contained 17 modified tracked paths and 13
  untracked paths, including the prototype and local Math Box documents. They
  were preserved.
- Created the exact user-provided prompt at
  `AI-Logs/Prompts/2026-11-16-level7-mathbox-normal-callstate-readonly-investigation-prompt.md`
  and this paired report. The prompt copy was verified line-for-line and is
  ignored by Git.
- No project source, tests, configuration, durable documentation, protected
  reference material, or Git state was changed. No files were staged or
  committed.
- **Validation:** no build or tests were applicable to this read-only numeric
  investigation. Final comparison confirmed the branch, HEAD, upstream,
  empty index, 17 pre-existing tracked modifications, and 13 pre-existing
  untracked paths are unchanged; both task records are ignored by Git.
