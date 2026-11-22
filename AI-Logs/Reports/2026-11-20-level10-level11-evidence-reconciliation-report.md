# Level 10 / Level 11 evidence reconciliation

## Outcome

**STILL BLOCKED for Level 11 implementation.** The genuine Level 10 recovery
used a complete local Rev. 2 program-ROM image to supply geometry bytes omitted
from the tracked disassembly. That ROM directory is expressly out of scope for
this investigation. The tracked disassembly does, however, provide substantial
Level 11 gameplay-parameter evidence that the earlier block report did not
record. No production implementation was attempted.

## 1. Level 10 recovery chain

**HISTORICAL PROJECT EVIDENCE / ROM-BACKED (per the recovery record):**
[the Level 10 recovery report](2026-11-17-level10-geometry-recovery.md)
records this chain:

1. The tracked [ROM disassembly](../../Reference/arcade/Tempest%20Rom%20Source%20code.md)
   supplies the table layout and semantics: `lev_x`, `lev_y`, and
   `lev_angle` are 16-by-16 point tables, `lev_remap` selects encountered
   levels, and the draw/setup code preserves point order and consults
   `lev_open`. It leaves the relevant table bytes as `.chunk` declarations.
2. The disassembly's level initialization/advance/display paths
   (`$9187-$9197`, `$C98E-$C996`, `$C9E3-$C9EA`) and geometry setup
   (`$C237-$C239`, `$C2E8-$C30C`) establish that displayed Level 10 uses
   internal index 9 and selects `lev_remap[9]`. They establish indexing, not
   the value of that entry or the point bytes.
3. The recovery then examined ten named 2-KiB program images at one bounded
   signature location. The disassembly signature at `$B967` uniquely matched
   `136002-118.k1` at chip offset `$0167`, anchoring the `$B800` page.
   The report says it extracted the row and companion bytes at disassembly
   addresses minus `$B800`; e.g. row 9 X/Y/angle bytes at chip offsets
   `$020C-$021B`, `$030C-$031B`, `$040C-$041B`, with `lev_remap[9]` at
   `$0485`.
4. It established point order from the setup loop, decoded the raw bytes, and
   cross-checked the same extraction against known Level 8 row 7 values.
   The recovery report explicitly says **no derived file was written**.

Thus Level 10 geometry was **not** read directly from expanded textual
disassembly, calculated from declarations, or sourced from a pre-existing
Level 10 C# definition. Text established indexing, addresses, order, and
interpretation; the binary program ROM supplied the missing values.

**HISTORICAL PROJECT EVIDENCE:** The earlier
[diagnostic Level 10 report](2026-11-17-diagnostic-level10-genuine-geometry-report.md)
stopped after inspecting four named 256-byte chips and candidate offsets.
That check did not establish that no complete program ROM was present. The
later recovery found and uniquely anchored the 2-KiB `136002-118.k1` image.

The later [genuine-geometry implementation report](2026-11-17-level10-genuine-geometry-implementation.md)
preserves the full ordered X/Y/angle arrays, row-9 mapping, open flag, and
presentation metadata. It is enough to confirm the factual Level 10 geometry
record, but not enough by itself to reproduce the extraction provenance: the
recovery report is needed for the ROM filename, signature anchor, address
translation, and extraction method.

**PROJECT TRANSLATION:** The implementation converts centered raw coordinates
using scale `25/14` and reflected Y, retains 16 ordered points, and represents
the open shape as 15 adjacent playable segments. That C# mapping is distinct
from the raw ROM evidence and was later manually accepted for the Level 10
diagnostic; its Level-7-like gameplay is explicitly not production Level 10.

## 2. Source inventory

- **TRACKED TEXTUAL REFERENCE:** The disassembly supplies geometry table
  declarations and address/index semantics, but not the missing geometry
  values. It does contain annotated/raw parameter-table bytes for gameplay
  values, including Level 11 entries discussed below.
- **HISTORICAL PROJECT EVIDENCE:** The Level 10 recovery used
  `Reference/arcade/roms.rev2/136002-118.k1`. This investigation did not
  access or enumerate that directory. The source is unavailable **under this
  task's authorization**, not asserted to be absent from the machine.
- **CURRENT C# REPRESENTATION:** `LevelLoader.Level10RomEvidence` and
  `RomPresentationMetadataCatalog` retain Level 10 row-9 data and metadata.
  The catalog covers production Levels 1-9 plus diagnostic Level 10; there is
  no Level 11 geometry or metadata entry.
- **HISTORICAL PROJECT EVIDENCE:** No separate generated/extracted textual
  geometry artifact was made during Level 10 recovery. The later report and
  C# are records/implementations of Level 10 only, not sources for Level 11.

## 3. Difference from the Level 11 block

The [Level 11 block report](2026-11-20-level11-fast-production-evidence-block-report.md)
correctly notes that the tracked disassembly does not expand Level 11 geometry
bytes and that no ROM directory was inspected. The Level 10 extraction method
cannot be repeated within this task's authorization: its required binary
source is expressly prohibited here. The data gap is the same kind (omitted
geometry/remap table values), though Level 11 requires its own mapping and
selected row; Level 10's row 9 cannot establish either.

The block report's geometry stop therefore remains justified. Its gameplay
search was incomplete, however: the tracked disassembly's `flipper_move`,
`wave_enemies`, `wave_spikeht`, and per-type min/max parameter records contain
Level 11 values in their annotated table bytes. A prior
[enemy-supply ROM analysis](2026-11-08-level1-7-enemy-supply-rom-investigation-report.md)
documents how the same parameter-table decoder yields generic supply and
selection limits. The [Flipper/Spike analysis](2026-11-06-level-specific-flipper-spike-rom-analysis-report.md)
and [Tanker movement analysis](2026-11-10-level7-concurrency-and-movement-rom-investigation-report.md)
provide the relevant program and cargo-depth semantics.

This distinction matters: the geometry method used a binary image because the
geometry table values were absent from text; it did not recover authentic
Level 10 gameplay configuration. The Level 10 implementation deliberately
reused Level 7-like diagnostic gameplay.

## 4. Level 11 facts recovered

**ROM/DISASSEMBLY BACKED / TRACKED TEXTUAL REFERENCE**, from the tracked
disassembly's effective-level logic (`$92C5-$9326`), geometry lookup
(`$C237-$C30C`), and annotated parameter tables (including
`flipper_move` at `$9607_5B`, `wave_enemies` at `$9607_4F`, and
`wave_spikeht` at `$9607_53`):

- **Indexing, not shape mapping:** displayed Level 11 is effective parameter
  level 11 and uses `p1_level = 10` for geometry lookup. The geometry path
  therefore consults `lev_remap[10]`; its value and selected shape row remain
  unknown.
- **Flipper program:** the eleventh one-based value in the 16-entry
  `flipper_move` data is `$24`; ordinary Flipper initialization reads this
  level-selected value at `$9A9D-$9AA7`. The program at `$A0FE-$A157` is the
  ROM p-code's repeating two-flips, reverse, three-flips, reverse sequence.
  This is ROM listing evidence, not a Level 11 manual arcade observation.
- **Tanker-released Flippers:** the ROM's general release-depth selector uses
  the current level's `flipper_move` at or beyond raw along-position `$20`
  (therefore `$24` at Level 11); below `$20` it uses straight program `$07`.
  Natural near-rim release is below that boundary. This is a ROM-backed
  distinction, not a claim that every cargo release uses the ordinary route.
- **Base supply:** `wave_enemies` for Level 11 is `$1B` (27 generic pending
  enemies), not a fixed species-by-species list. The ROM hard-difficulty
  branch produces 30 for this value; the ordinary table value is 27.
- **Concurrent selection limits:** Flippers 0/5, Tankers 1/2, Spikers 2/3,
  and Fuseballs (ROM label `fuzzballs`) 1/1 minimum/maximum. Pulsars have
  minimum 0 at Level 11 and are not enabled by this table before Level 17.
  These limits govern dynamic selection; they are not exact whole-wave
  species counts. The zero Flipper minimum excludes ordinary bottom
  selection even though the maximum is nonzero.
- **Capacity:** `max_enm = 6` is the highest slot index, i.e. seven physical
  enemy slots.
- **Starting Spike parameter:** `wave_spikeht` supplies `$B0` to each of the
  16 ROM spike-height entries. This does not establish tube topology or the
  number of playable lanes.
- **Direct Fuseball threshold:** The table's Level 11 minimum of one is
  direct disassembly support for Fuseball eligibility at Level 11. The
  separate earlier milestone analysis of this threshold was Incitti-derived;
  the ROM table is the stronger local evidence.

No values above were copied from Level 10 or inferred from another board.

## 5. Remaining gaps

- **UNRESOLVED:** `lev_remap[10]`, the Level 11 shape row, its 16 ordered X/Y
  vertices and angles, and `lev_open[10]` / selected-row topology. The
  declarations alone do not determine these arbitrary table bytes.
- **UNRESOLVED:** Level 11 scale, camera height, signed placement, Flipper
  scale, and any other row-specific presentation metadata.
- **UNRESOLVED:** playable lane count and a Level 11 C# geometry/projection
  translation, because these depend on the missing mapping and topology.
- **UNRESOLVED:** a fixed species-by-species wave roster. The recovered
  minimum/maximum tables specify dynamic selection constraints, not a
  deterministic composition list.

Recovering geometry would require authorized access to the relevant complete
Rev. 2 program-ROM evidence (or a verified textual extraction of the Level 11
remap and selected row). That evidence was not accessed or generated here.

## 6. Recommendation

**STILL BLOCKED.** The supported Level 11 gameplay-table facts improve the
handoff but do not establish genuine geometry, remap, topology, or presentation
metadata. Since geometry is not supported, the task does not meet the stated
PARTIALLY READY criterion, and Level 11 must not be implemented from these
partial facts.

## Validation and worktree

Read-only source/report investigation only. **No tests or build run** (no code
changed); no game launch or manual gameplay acceptance performed. The task's
pre-existing tracked and untracked worktree changes were left untouched; no
Git index or history operation was performed.
