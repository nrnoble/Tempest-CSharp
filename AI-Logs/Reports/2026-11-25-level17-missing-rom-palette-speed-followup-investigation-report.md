# Level 17 missing ROM palette and speed follow-up investigation

**Status: PARTIAL — NOT READY.** The ROM table interpreter establishes a Level
17 Flipper movement-program byte, and several source-table calculations are
available. However, the local Rev. 2 ROM file-to-address mapping was not
validated by the bounded reads. I therefore cannot report the eight raw
palette bytes as verified. The follow-up also found concrete contradictions
with the first report concerning maximum Pulsars, firing under hard difficulty,
and Level 17 shot holdoff. Per the task stop conditions, this report makes no
Level 17 implementation recommendation.

## 1. Executive summary

The local source listing identifies Level 17 as source level `$11`. Its
`flipper_move` record uses opcode `$06`, which indexes the program value by
the level's position within a 16-level block. The loader's explicit
`lwb = (((lev-1)&15)+1)` rule therefore selects the first data byte for Level
17, `$07`; this is ROM-backed table-index evidence, not an inference from
Level 1 geometry or Level 16 adjacency.

The source listing also establishes:

- The Level 17 Pulsar minimum is 2, while its maximum table entry decodes to
  5, not 2 as stated in the first report.
- The Level 17 medium-table Pulsar firing flag is off, but the hard-difficulty
  initializer unconditionally ORs `$40` into that flag. Thus the first
  report's unqualified “Pulsar firing disabled” statement is incomplete.
- Level 17 shot holdoff is `$20` (32 ROM ticks), not `$14`; `$14` applies to
  source levels 21–64.
- The Level 17 Tanker load vector entries are type 1. The source path reviewed
  here does not establish that the vector means exactly two cargo units.

I attempted only the bounded reads needed to verify omitted table bytes. The
candidate chip mapping failed a known-instruction consistency check. The
returned bytes are recorded below as **rejected candidate reads**, not as
Level 17 ROM data. The correct local ROM mapping must be established before
repeating the palette extraction or claiming raw bytes.

## 2. Starting Git and repository state

Read-only baseline, before this report and its paired prompt were created:

- Branch: `master`, tracking `origin/master`
- HEAD: `fde09aa31af760958f656aa553555462d51792a0`
- Staged changes: none
- Pre-existing tracked modification: `TEMPEST-BEHAVIOR-SPEC.md`
- Pre-existing untracked files:
  - `AI-Logs/Prompts/2026-11-25-level17-rom-reuse-color-pulsar-investigation-prompt.md`
  - `AI-Logs/Reports/2026-11-25-level17-rom-reuse-color-pulsar-investigation-report.md`
  - `Reference/arcade/MBUCOD.V05.txt`

Those pre-existing changes were preserved. The new prompt and this report are
the only files created for this follow-up.

## 3. First-report findings accepted, and conflicts discovered

The tube-cycle/remap findings, the distinction between shape data and
displayed-level gameplay/palette data, the accepted Levels 1–16 regression
boundary, and the lack of normal Level 17 production integration remain
accepted as recorded in the first report.

Three first-report claims require correction or qualification based on the
local Atari source listing:

1. **Pulsar maximum:** the source-level 17 maximum decodes to 5, not 2.
2. **Pulsar firing:** disabled by the Level 17 medium table, but enabled by
   the hard-difficulty adjustment.
3. **Shot holdoff:** source level 17 decodes to `$20`; the `$14` constant
   begins at source level 21.

The first report's “two Flippers” Tanker cargo count is not confirmed by the
bounded cross-check. The source establishes type-1 entries in a four-entry
load vector, but this pass did not establish the vector's complete cargo-count
semantics. No discrepancy is asserted about the type-1 interpretation itself.

## 4. Evidence and classifications

Primary local source consulted:

- `Reference/arcade/Tempest Rom Source code.md`

The source listing contains the level-table loader, table decoder, color
installation routine, parameter tables, and enemy initialization code. The
following classifications are used:

- **ROM-BACKED (source-listing):** directly established by the local
  ROM-derived disassembly and its explicit table-decoder comments/code.
- **PROJECT TRANSLATION:** an interpretation needed to map a ROM value into
  current host behavior.
- **UNRESOLVED:** source or mapping evidence reviewed here is insufficient.

The local `Reference/arcade/MBUCOD.V05.txt` file was identified as a math-box
microcode listing; its opening 35 lines did not provide the game CPU ROM
address map and it was not used as evidence for game parameters.

## 5. Exact Level 17 Flipper movement-program byte

**ROM-BACKED (source-listing): `$07`.**

Evidence and calculation:

- The Level 17 source number used by the parameter loader is `$11` (17).
- The loader at `$92d6` sets the effective level to `curlevel + 1`.
- The table decoder documents opcode `$06` as selecting
  `b[lwb-ltmin]`, where
  `lwb = (((lev-1)&15)+1)`.
- `flipper_move` points to table record `$9607_5b`, whose header is
  `06 01 63`. It contains 16 data values; the listing comment begins
  `07 0b 19 24 53 0b 24 19 53 87 24 19 53 07 87 24`.
- For `lev=$11`, `lwb=1`; with `ltmin=1`, the lookup index is zero. The
  selected value is the first data byte, `$07`.

This use of the modulo-16 table operation explicitly establishes the
second-cycle entry. It does not depend on assuming that Level 17 behaves like
Level 1 or Level 16.

The requested full below-rim P-code sequence, lane-flip count, reversal and
move/flip ordering, Spike-sensitive branches, initial-direction semantics,
Tanker cargo-Flipper program semantics, and exact correspondence to the
existing C# movement-program implementation were **not completed**. The
investigation stopped after the ROM address-map validation failed; these are
not implementation assumptions.

## 6. ROM address-map validation and palette extraction

### Mapping attempt

The disassembly addresses `$9607` and `$c198` were provisionally mapped using
the candidate files `136002-113.d1` and `136002-120.mn1`, respectively, with
file offset computed as address minus a presumed `$9000` or `$c000` base.
Each candidate file was 2,048 bytes. These presumed bases were not established
by a local manifest or a matching known-byte check.

Bounded reads performed:

| Candidate file | Offset | Length | Returned bytes | Result |
|---|---:|---:|---|---|
| `136002-113.d1` | `0x607` | 19 | `EF 95 6D 01 E3 95 B3 00 FA 93 19 01 07 94 1A 01 CD 94 29` | Rejected: does not match the listing's expected data at `$9607` |
| `136002-120.mn1` | `0x198` | 6 | `A9 18 85 00 BD 02` | Rejected: does not match the known `$c198` instruction bytes `A5 9F 29 70 C9 5F` |
| `136002-120.mn1` | `0x205` | 8 | `AC 26 01 F0 03 CE 26 01` | Not accepted as palette data because the address map failed validation |

These are not claimed as the contents of the Level 17 table or color block.
No further ROM ranges were read. No ROM file was modified, hashed, or
downloaded.

### Palette selection logic established from the listing

The routine `setcolours` at `$c196`:

1. reads `curlevel`;
2. masks with `$70`, caps at `$5f`, shifts right once, then ORs `$07`;
3. begins with `Y=7`, reads the indexed byte, and decrements both X and Y
   through eight iterations;
4. writes each byte's low nibble to `$0019,Y` and `col_ram,Y`;
5. writes its high nibble to `$0021,Y` and `$0808,Y`.

For `curlevel=$10` (displayed Level 17), the index starts at `$0f` and descends
to `$08`, addressing the eight bytes at `$c205..$c20c` in reverse iteration
order. Consequently, table byte 0 at `$c205` supplies array index 0, and table
byte 7 at `$c20c` supplies array index 7. The exact raw block remains
**UNRESOLVED** because the ROM mapping was not validated.

The listing documents six cycle blocks beginning at `$c1fd`, `$c205`,
`$c20d`, `$c215`, `$c21d`, and `$c225`. It does not label the eight runtime
indices with semantic entity roles in the inspected excerpt. No ROM-backed
hue/intensity interpretation, entity-role map, host RGB mapping, or
cycle-specific C# palette equivalence is asserted.

The startup path shown in the listing calls `setcolours` after writing the
selected level to `curlevel`. The precise 16-to-17 transition timing and any
approach-state palette behavior were not traced in this stopped follow-up.

## 7. Screenshot accessibility and reconciliation

No screenshots were present in the current task's accessible attachment
context. The task text attachment was available; the other tagged attachment,
`file_Untitled-3.txt`, was empty. No repository screenshots or videos were
searched or opened.

There are therefore no objective screenshot color observations and no
screenshot/ROM reconciliation. Screenshot unavailability does not explain the
palette-byte blocker; the blocker is the unverified ROM address mapping.

## 8. Pulsar parameters and source-level discrepancies

The local listing provides these source-level facts:

| Field | Level 17 source value | Evidence/qualification |
|---|---:|---|
| `min_pulsars` | `$02` | `9607_1b`: opcode `$02`, source levels `$11..$20`, value `$02` |
| `max_pulsars` | `$05` | `9607_1f`: opcode `$04`, source levels `$11..$20`; first indexed value is `$05` |
| `pulsar_fliprate` | `$28` (40) | `9607_57`: opcode `$04`, source `$11` selects first value `$28` |
| `pulse_beat` | `$04` | `9607_3f`: constant `$04` for source levels `$01..$30` |
| Pulsar speed fields | `$FE` / `$A0` initialized at `$93cd..$93d4` | Fixed initialization shown; movement units and exact Level 17 timing effects not traced |
| Medium-table firing flag | `$00` | `9607_03` enables `$40` only for source `$3c..$63` |
| Hard-difficulty firing flag | `$40` bit set | `$937a..$937f` OR `$40` into `pulsar_fire` on the hard branch |

The `max_pulsars` decoder is explicit: opcode `$04` selects
`b[lev-ltmin]`; at source `$11`, the first value in the 17–32 record is
`$05`. This directly contradicts the first report's stated maximum of 2.

The firing gate is also explicit. The table leaves Level 17's flag clear for
the medium baseline, but the hard-difficulty branch sets the same `$40` bit
before the flag is used by Pulsar initialization at `$9aac`. The first
report's unqualified disabled-firing boundary is therefore incomplete.

The exact pulse waveform intervals, climb/descend decision timing, pauses,
reversal timing, lane-change and collision-state cadence, and difficulty
effects beyond the observed firing flag were not traced. The first report's
reference-derived update and pause constants are not promoted to ROM-backed
values. Existing C# Pulsar parameter equivalence remains unverified in this
follow-up.

## 9. Level 17 speed and firing table calculations

The source table decoder and `crack_speed` routine establish the following
values. The output bytes below are the 8.8 speed words written by the loader;
they are not host-game displacement values.

| Mechanic | Level 1 raw | Level 16 raw | Level 17 raw | ROM conversion/result |
|---|---:|---:|---:|---|
| Flipper speed | `$D4` | `$A0` | `$AF` | `crack_speed` shifts the signed 8.8 input left three places: `FE.A0`, `FD.00`, `FD.78` |
| Tanker speed | shared with Flipper | shared with Flipper | shared with Flipper | Startup copies the cracked Flipper speed into Tanker speed |
| Spiker speed | `$00` | `$00` | `$00` | Input table is `$00` through source 20; complete movement interpretation not traced |
| Enemy-shot speed | Base table `$C0` | Base table `$C0` | Base table `$C0` | Opcode `$0a` computes `b[0] + RAM[$0160]`; the effective RAM modifier/output was not resolved |
| Pulsar speed | fixed init | fixed init | fixed init | `$FE.A0` is initialized directly, not selected from a level-indexed speed table |
| Shot holdoff | `$50` | `$23` | `$20` | `$50 + (lev-1)*$FD` for levels 1–20; Level 17 is 32 ticks |
| Enemy-shot maximum field | table varies | `$02` | `$02` | Level 17 falls in constant `$02` range; count-vs-last-index semantics not independently settled here |

For the listed 8.8 words, signed magnitudes are Level 1 Flipper `1.375`,
Level 16 `3.0`, and Level 17 `2.53125`. Thus Level 17's ROM cracked magnitude
is about 1.8409 times Level 1, but 0.84375 times Level 16. The cycle boundary
is not a universal increase over Level 16. These ratios describe only this
ROM speed word; they do not prove an overall host or arcade update-rate ratio.

The holdoff decoder gives Level 17 `$20`:
`$50 + 16 * (-3) = $20`. The next record begins at source `$15` (decimal 21)
and assigns `$14`. This corrects the first report's Level 17 holdoff range.

### Difficulty arithmetic visible in the initializer

The difficulty bits are documented as medium `0`, easy `1`, hard `2`.

- **Easy:** decrements the shot-maximum field and computes the Flipper speed
  adjustment using the listing's complemented value, three logical shifts,
  and `ADC`. With Level 17 input `$AF`, the result is `$B9` under the observed
  carry path. No Level 17 wave-supply adjustment is made on this branch.
- **Hard:** increments the shot-maximum field, caps it at `$03`, adjusts
  Flipper speed with three shifts, OR `$E0`, and `ADC`. For Level 17 `$AF`,
  the observed carry path gives `$A5`. It adjusts supply as
  `wave + (wave >> 3) + carry`; for base supply `$14` (20), the ROM shifts
  yield `$02` with carry 1, so the result is `$17` (23).
- **Hard Pulsar firing:** the same hard branch ORs `$40` into
  `pulsar_fire`; this applies at Level 17.
- No shot-holdoff adjustment is present in the inspected difficulty block.

The raw shot-maximum field's final physical slot count and the firing gates
for Tankers, Flippers, and Spikers were not traced to completion. The first
report's “two slots” interpretation is therefore not independently reaffirmed
here. No C# difficulty or shot translation was inspected.

## 10. Tanker cargo boundary cross-check

The loader initializes `tanker_load[0]` and `[1]` to type `$01`. The Level 17
table records for `tanker_load+2` and `tanker_load+3` also select `$01`
(`9607_43` and `9607_47`, source 17–32). The tanker selection routine
randomly indexes the four-entry vector and uses the selected type value.
This confirms the vector's Level 17 type entries; it does not, by itself,
establish that exactly two cargo units are released. Difficulty-independent
cargo selection/count semantics were not completed.

The Pulsar boundary is contradicted as described above: hard difficulty sets
the firing-enable bit at Level 17. Because the first report's boundary is
conditional only when “disabled” means medium/default difficulty, it must be
corrected before Level 17 implementation.

## 11. C# comparison and architecture implications

No implementation recommendation is made because the task's stop condition
was reached. The following comparisons remain open:

- Existing Flipper movement-program translation and full P-code behavior.
- C# Flipper/Tanker, Spiker, Pulsar, enemy-shot, holdoff, and transition-rate
  speed representations.
- Whether a cycle palette belongs in presentation metadata or a shared
  palette catalog, and the exact mapping from eight ROM nibbles to visible
  roles.
- Whether the current Pulsar actor can be parameterized with the ROM
  pulse/movement fields without changing its state design.
- C# shot-slot semantics and species-specific firing eligibility.
- The complete Tanker cargo-count and Level 17 Pulsar admission/ledger
  boundaries.

The first report's established architecture observations remain contextual
only; they are not a Level 17 implementation recommendation.

## 12. Implementation-readiness gate

## NOT READY

Blockers:

1. The correct Rev. 2 ROM file-to-address mapping has not been established.
   The attempted chip candidates failed the known-byte check. The complete
   Levels 17–32 palette block and its required raw-value verification remain
   blocked.
2. The first report's Pulsar maximum and hard-difficulty firing boundary need
   correction and a bounded authoritative recheck before implementation.
3. Full Flipper P-code sequence and its existing C# reuse assessment remain
   incomplete.
4. Exact Pulsar timing/state transitions and the current C# correspondence
   remain incomplete.
5. Tanker cargo-count semantics, physical enemy-shot slot count, and
   species-specific firing gates remain unresolved.
6. ROM-to-host translations for the remaining speed/timing mechanisms and
   palette roles have not been established.

Smallest next evidence task: establish the Rev. 2 CPU ROM chip-to-address map
from an authoritative local manifest or another verified local mapping; then
repeat only the `$9607_5b` and `$c205..$c20c` bounded reads and validate them
against known disassembly bytes. Reconcile the Pulsar maximum, hard firing,
and holdoff corrections in the first report before authorizing an
implementation candidate.

## 13. Required report checklist and execution record

- **Evidence sources:** first Level 17 report; local ROM-derived
  `Tempest Rom Source code.md`; `MBUCOD.V05.txt` opening context only.
- **Flipper byte:** established as `$07` by the opcode-6 16-level index rule.
- **Flipper full P-code trace/C# reuse:** blocked; not guessed.
- **Palette block address/layout/selector:** `$c205..$c20c`; raw values not
  verified; selector arithmetic and nibble assignments recorded above.
- **Palette roles/host RGB:** unresolved; no arbitrary RGB mapping asserted.
- **Screenshots:** unavailable; no substitute assets inspected.
- **Pulsar values:** table values and hard-fire discrepancy recorded above;
  full timing/state trace blocked.
- **Speed comparisons and cracking:** Flipper/Tanker outputs and Level 17
  holdoff calculations recorded above; other movement and host conversions
  remain unresolved.
- **Difficulty:** Level 17 hard supply 23 and hard/easy raw speed/fire
  adjustments recorded above; C# difficulty support not investigated.
- **Tanker cargo:** four table entries resolve to type 1; exact cargo count
  unresolved.
- **Implementation scope/manual acceptance:** not recommended pending blockers.
- **Files/resources inspected:** first report, local disassembly excerpts,
  `MBUCOD.V05.txt` first 35 lines, and the two task attachments.
- **ROM file ranges:** candidate `136002-113.d1` offset `0x607`, length 19;
  candidate `136002-120.mn1` offsets `0x198` length 6 and `0x205` length 8.
  These reads failed mapping validation and were not used as ROM data.
- **Commands/calculations:** baseline `git status --short --branch`,
  `git rev-parse HEAD`, `git branch -vv`, diff summaries and untracked-file
  listing; focused source searches/views; file-name/size inspection for ROM
  candidates; bounded `FileStream` reads; table arithmetic shown above.
- **Production code changed:** no.
- **Tests changed:** no.
- **Durable behavior/development documentation changed:** no.
- **Staged, committed, or pushed:** no.
- **Level 18+ implementation or investigation:** none.
- **Interactive game launched:** no.
- **Tests/builds:** not run; this was a bounded read-only investigation and it
  stopped at an evidence-map blocker.
- **Recommended next step:** establish and independently validate the local
  Rev. 2 CPU ROM address map, then resolve the listed source discrepancies
  before any Level 17 implementation authorization.
