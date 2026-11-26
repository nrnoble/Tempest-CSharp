# Level 17 Rev. 2 CPU ROM address-map validation and final evidence recovery

**Status: PARTIAL — mapping proven and bounded ROM recovery ready, but the
task's remaining P-code/Pulsar implementation trace is not enough to produce
an implementation recommendation.**

No production code, tests, durable documentation, ROM files, or Git state
were modified. The interactive game was not launched.

## 1. Starting state and required reports

- Branch: `master`, tracking `origin/master`
- Pre-existing tracked modification: `TEMPEST-BEHAVIOR-SPEC.md`
- Pre-existing untracked Level 17 prompts/reports and
  `Reference/arcade/MBUCOD.V05.txt` were preserved.
- Read completely for this correction:
  - [2026-11-25-level17-rom-reuse-color-pulsar-investigation-report.md](2026-11-25-level17-rom-reuse-color-pulsar-investigation-report.md)
  - [2026-11-25-level17-missing-rom-palette-speed-followup-investigation-report.md](2026-11-25-level17-missing-rom-palette-speed-followup-investigation-report.md)

The prior follow-up incorrectly used `136002-120.mn1` for `$C198` and
`136002-113.d1` for `$9607` with guessed bases. The `$9607` chip was not
wrong; its read location was misinterpreted in the previous candidate
attempt. The `$C198` chip was wrong.

## 2. Verified CPU ROM layout

The existing repository report independently established and this pass
revalidated a byte-linear Rev. 2 CPU layout:

| CPU range | Rev. 2 file | Length |
|---|---|---:|
| `$9000-$97FF` | `136002-113.d1` | `$800` |
| `$9800-$9FFF` | `136002-114.e1` | `$800` |
| `$A000-$A7FF` | `136002-115.f1` | `$800` |
| `$A800-$AFFF` | `136002-316.h1` | `$800` |
| `$B000-$B7FF` | `136002-217.j1` | `$800` |
| `$B800-$BFFF` | `136002-118.k1` | `$800` |
| `$C000-$C7FF` | `136002-119.lm1` | `$800` |
| `$C800-$CFFF` | `136002-120.mn1` | `$800` |
| `$D000-$D7FF` | `136002-121.p1` | `$800` |
| `$D800-$DFFF` | `136002-222.r1` | `$800` |

For these linear CPU images:

`file offset = CPU address - range base`.

No header, byte interleaving, or byte swapping was observed in the bounded
reads. The ROM source listing uses CPU addresses directly. The PROM/lookup
images are not 6502 linear program images and were not used for this mapping.

## 3. Multiple independent known-byte validations

| Anchor | Listing bytes | Mapped file | Offset | Actual bytes | Result |
|---|---|---|---:|---|---|
| setcolours `$C196-$C19B` | `A5 9F 29 70 C9 5F` | `136002-119.lm1` | `$0196` | `A5 9F 29 70 C9 5F` | PASS |
| `$9607` pointer table | `EF 95 6D 01 E3 95 B3 00 FA 93 19 01 07 94 1A 01` | `136002-113.d1` | `$0607` | `EF 95 6D 01 E3 95 B3 00 FA 93 19 01 07 94 1A 01` | PASS |
| `$B944` instruction | `A6 74 A4` | `136002-118.k1` | `$0144` | `A6 74 A4` | PASS |
| `$B967` instruction | `AD 15 04` | `136002-118.k1` | `$0167` | `AD 15 04` | PASS |
| `$C300` remap instruction | `B9 7C BC 8D 12 01` | `136002-119.lm1` | `$0300` | `B9 7C BC 8D 12 01` | PASS |
| Startup at `$9009` | `20 C5 92 20 34 92` | `136002-113.d1` | `$0009` | `20 C5 92 20 34 92` | PASS |

The independent chips, bases, and sequences all agree. The mapping is therefore
**ROM-BACKED / Rev. 2 CPU mapping validated**. The candidate reads from
`136002-120.mn1` that assumed `$C000` remain rejected candidate reads, not
ROM evidence.

## 4. Required bounded ROM recovery

### 4.1 Level 17 Flipper movement program

The raw table at `$95F3-$9605` is:

`00 06 01 63 07 0B 19 24 53 0B 24 19 53 87 24 19 53 07 87`

The `flipper_move` record selects `$07` for source Level 17 through the
listing's opcode-6 modulo-16 rule:

- source/effective level `$11`;
- `lwb = (((lev-1)&15)+1) = 1`;
- table index `lwb-ltmin = 0`;
- first payload byte = `$07`.

This confirms the prior report's `$07` **ROM-BACKED**.

### 4.2 Levels 17–32 palette block

The exact block read from `136002-119.lm1` is:

| CPU | Offset | Byte |
|---|---:|---|
| `$C205` | `$0205` | `$00` |
| `$C206` | `$0206` | `$07` |
| `$C207` | `$0207` | `$0B` |
| `$C208` | `$0208` | `$08` |
| `$C209` | `$0209` | `$44` |
| `$C20A` | `$020A` | `$03` |
| `$C20B` | `$020B` | `$0C` |
| `$C20C` | `$020C` | `$0C` |

Exact eight-byte value:

`00 07 0B 08 44 03 0C 0C`

The neighboring read `$C1FD-$C20C` produced 16 bytes:

`00 04 08 0C C3 07 0B 0B 00 07 0B 08 44 03 0C 0C`

This is a valid boundary sanity check: the first eight bytes belong to
Levels 1–16 and the recovered second eight bytes to Levels 17–32.

`setcolours` at `$C196` writes the selected block from `$C205-$C20C` for
source Level 17. Semantic entity-role meaning is still absent from the
listing and is therefore not claimed.

## 5. `$07` Flipper P-code evidence

The P-code entry point is `$A0FE`:

- `$A0FE`: `0C` move per speed;
- `$A0FF`: `00` done;
- `$A100`: `06` branch;
- `$A101`: `06` branch target `$07`.

Thus `$07` means: repeatedly execute the movement action at ROM P-code
entry `$A0FE`, move at the effective Flipper/Tanker speed, and repeat until
the P-machine is halted or the normal enemy movement/completion path takes
control. It does not execute a flip or reversal sequence. This is
**ROM-BACKED** P-code behavior.

The ROM comment explicitly describes `$07` as “just move up” and says it is
used for Tankers, for Flippers on some levels, and for pieces when a Tanker
splits. The same P-code entry is therefore shared by ordinary and
Tanker-cargo Flippers where the ROM assigns `$07`.

The C# currently has a Tanker cargo `Straight07` translation and a
`Repeating24` translation, plus separate program selectors for multiple
production levels. This is a compatible host implementation boundary for
straight movement, but it is not automatically proof that C# should select
`Straight07` for Level 17. Final selection remains implementation work.

## 6. Pulsar corrected policy

| Difficulty | Medium/default | Easy | Hard |
|---|---|---|---|
| Minimum | `$02` | no ROM evidence for a separate adjustment | no ROM evidence for a separate adjustment |
| Maximum | `$05` | no ROM evidence for a separate adjustment | no ROM evidence for a separate adjustment |
| `pulsar_fliprate` | `$28` for source `$11-$12`, transitions later | unchanged by source 17–32 table unless difficulty separately adjusts it | unchanged by source 17–32 table unless separately traced |
| `pulse_beat` | `$04` | no separate source value for Level 17 found | no separate source value for Level 17 found |
| Firing flag | clear | clear unless an easy branch explicitly ORs it (not shown) | `$40` ORed by the hard branch |
| Speed init | `$FE/$A0` fixed initialization | fixed initialization | fixed initialization |

The prior report's maximum `$02` claim is superseded: Level 17's
**ROM-BACKED** `max_pulsars` is `$05`. Medium/default and easy leave the
Pulsar firing flag clear for Level 17; the hard branch ORs `$40`.

The source pulse clock uses `pulsing += pulse_beat`; it reverses
`pulse_beat` after the pulse variable reaches `$0F` on ascent or `$C1`
on descent. `eact_26` sets the hazard condition for now or four ticks ahead.
The P-code entry repeatedly executes pulsar motion at fixed initialized speed.

Existing C# 51-update, 181-update, 18-tick lane pause, 14-tick rim pause,
and fixed 12-degree pulse flip constants remain
**REFERENCE-DERIVED / PROJECT TRANSLATION**, not directly ROM-backed solely
from this Level 17 trace.

## 7. Tanker cargo semantics

The ROM comment defines `$028A` low two bits as:

- `$01`: split into two Flippers;
- `$02`: split into two Pulsars;
- `$03`: split into two Fuzzballs.

The Level 17 Tanker vector and initialization contain value `$01`:

- `tanker_load[0]` and `[1]` initialized to `$01`;
- `9607_43` and `9607_47` select `$01` for source Level 17;
- the available-enemy routine explicitly decrements the held enemy type
  availability twice for every active Tanker;
- an active Tanker selected by its low cargo bits releases/splits into that
  cargo type.

For Level 17, the ROM therefore establishes **two cargo Flippers**
**ROM-BACKED**. The vector entry is a cargo type, not a count: `$01` means
the split is into two Flippers.

## 8. Enemy-shot slot semantics

At Level 17, `9607_0F` table decoder returns raw `enm_shotmax = $02`.
`$A2D6` starts the allocation search at `enm_shotmax`, then scans lower
indices for an empty `enm_shotpos` slot.

Therefore:

- medium/default: raw `$02`, inclusive capacity of three enemy-shot slots;
- easy: decrements to `$01`, inclusive capacity of two slots;
- hard: increments to `$03`, capped at `$03`, inclusive capacity of four slots;
- the field is a last/maximum slot index, not a count.

Firing slots are only allocated after:

- enemy is below the rim;
- enemy species' `$40` firing bit is set;
- per-enemy shot delay is ready;
- not in death state;
- random threshold passes;
- an empty scanned shot slot exists.

The existing C# accepted enemy-shot representation should not be rewritten
before prior-level behavior is separately reconciled. Any current
last-index-versus-count translation remains project behavior pending a
dedicated implementation audit.

## 9. Firing-eligible species

`$028A` species firing:

| Species | Level 17 status |
|---|---|
| Flipper | Yes, `$028A` bit `$40` is set; subject to shot delay, along coordinate, random threshold and slots |
| Tanker | Yes, tanker type has `$41`/`$40` firing bit under the same gates |
| Spiker | Yes, `$028A` bit `$40` is set; subject to the same gates |
| Pulsar | Medium/easy off; hard ORs `$40` |
| Fuseball | Excluded by zero Level 17 maximum/minimum |

Level 17 shot holdoff is `$20` (32 ROM ticks), confirmed by
`$50 + 16*(-3)`. It is copied to each firing enemy's `shot_delay` on shot
allocation. Enemy-shot speed is base `$C0`, with the listing's opcode `$0A`
adding RAM `$0160`; the exact effective modifier was not resolved by the
bounded listing pass, so the effective 16-bit speed is **UNRESOLVED**.

## 10. Speed evidence

- Level 17 Flipper/Tanker low-byte raw value: `$AF`; crack-speed conversion
  produces a fixed-point 8.8 word distinct from direct C# units.
- Spiker raw low-byte table value is `$00` for source Levels 17–20, but the
  zero is only a table input. Direct movement interpretation is unresolved
  without a fuller `eact_0c`/initializer trace.
- Pulsar fixed initialization is `$FE.A0`; direct C# conversion remains
  unresolved.
- Enemy-shot base `$C0` remains unresolved because opcode `$0A` depends on
  RAM `$0160`.
- No global speed multiplier is established by this evidence.

## 11. Implementation readiness

**NOT READY**

Ready facts:

- CPU ROM address mapping validated;
- exact Levels 17–32 palette block recovered;
- exact Level 17 Flipper P-code record `$07` confirmed;
- `$07` is straight move-only P-code;
- exact Tanker cargo result is two Flippers;
- corrected Pulsar maximum `$05`;
- hard-firing boundary `$40`;
- shot holdoff `$20`;
- raw Flipper/Tanker `$AF`;
- shot-slot field is a last index.

Remaining blockers before implementation:

- exact C# correspondence for cracked 8.8 words;
- complete P-code/Pulsar route and pause trace with no relabeling of C#
  constants;
- exact easy/hard movement or pulse differences beyond known firing/supply
  adjustments;
- effective enemy-shot speed modifier at `$0160`;
- Level 17 C# difficulty policy and ledger integration;
- visual screenshot/ROM reconciliation, unavailable in this task.

No implementation recommendation is made.

## 12. Stop condition

The first mandatory gate passed. Bounded ROM bytes and the source traces
listed above were recovered. The investigation stopped before source changes,
tests, implementation, durable documentation updates, staging, or an
interactive launch.
