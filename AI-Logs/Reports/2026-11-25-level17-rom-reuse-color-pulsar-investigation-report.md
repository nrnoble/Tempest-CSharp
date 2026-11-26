# Level 17 ROM reuse, color, Pulsar, and architecture investigation

**Status: PARTIAL — the implementation boundary and many Level 17 ROM values are
established, but exact Flipper-movement and palette-index bytes, plus the
supplied screenshots, are unavailable from the accessible evidence. No
production code, tests, durable documentation, Git state, or gameplay assets
were modified.**

## 1. Starting state

- Branch: `master`
- HEAD: `fde09aa31af760958f656aa553555462d51792a0`
- Upstream: `origin/master`
- Pre-existing tracked modification:
  `TEMPEST-BEHAVIOR-SPEC.md`
- Pre-existing untracked reference file:
  `Reference/arcade/MBUCOD.V05.txt`
- The tracked modification and untracked file were preserved unchanged.
- The interactive game was not launched.
- The local `TEMP/GameplayRecordings/lvl1-16.json` recording was not read or
  replayed.

## 2. Evidence and attached visual context

The accessible attachment contained the investigation prompt text. The second
tagged attachment was empty, and no two supplied screenshots were available in
the agent attachment context. Therefore no screenshot can be used as evidence
for this report. Visual evidence remains pending Neal's later review if and
when the screenshots are attached through an accessible path.

Local evidence used:

- project disassembly:
  [Tempest Rom Source code.md](../../Reference/arcade/Tempest%20Rom%20Source%20code.md)
- current durable behavior and development records
- current production source and tests

## 3. Level 17 tube reuse

**ROM-BACKED:** `curlevel` is incremented and stores the current zero-based
campaign level: the ROM draws/labels the displayed level as `curlevel + 1`
and its level-over transition increments `curlevel`
([reference lines](../../Reference/arcade/Tempest%20Rom%20Source%20code.md)).

**ROM-BACKED:** `get_tube_no` subtracts 16 repeatedly and indexes
`lev_remap[y]`, while retaining the tube-remap result for the low 16-level
cycle. Thus source Level 17 is in the second cycle and resolves to the same
`lev_remap` row as source Level 1.

**ROM-BACKED:** the first-cycle Rev 2 remap is:

`[0, 1, 2, 3, 4, 5, 6, 7, 13, 9, 8, 12, 14, 15, 10, 11]`

for displayed Levels 1–16. Level 17 therefore resolves to shape row 0, the
canonical Level 1 shape.

**PROJECT TRANSLATION / implementation gap:** the C# remap catalog currently
contains only the 1–16 array and throws for displayed levels above 16:

- [RomPresentationMetadata.cs](../../src/Tempest.Game/Levels/RomPresentationMetadata.cs)
- [AtariPresentationCalibration.cs](../../src/Tempest.Game/Rendering/AtariPresentationCalibration.cs)

A future Level 17 implementation should inherit Level 1 geometry rather than
copy its points. The Level 17 `LevelConfiguration` should add its own
displayed-level, ROM gameplay parameters, supply, enemies, and presentation
metadata while referencing shape row 0/canonical Level 1 geometry.

## 4. Shape-specific versus level-specific data

**Shape-specific**

- 16-point X/Y/angle arrays
- ROM tube remap identity
- open/closed tube flag
- tube-scale, camera-height, and 2-D placement metadata
- accepted shape-based projection/calibration inherited from the same tube

**Displayed-level/gameplay-specific**

- enemy supply
- Flipper/Tanker/Spiker/Pulsar/Fuseball minima and maxima
- Tanker cargo
- Flipper movement program
- Spike height and lane initialization
- enemy shot holdoff and slots
- enemy speeds
- difficulty
- palette/16-level color block
- HUD level identity
- progression and direct startup

The current C# record separates `BoardIndex`, `PaletteIndex`,
`DisplayedLevel`, and gameplay configuration, but `BoardIndex` and
`PaletteIndex` are still descriptive placeholders rather than a complete
16-level ROM color data model:

- [LevelConfiguration.cs](../../src/Tempest.Game/Levels/LevelConfiguration.cs)

## 5. Level 17 color architecture

**ROM-BACKED:** `setcolours` selects an 8-nibble block based on
`curlevel & $70`. Source 16 is the first level of the 17–32 block, so Level 17
installs the ROM's 17–32 color block, not the 1–16 block.

**ROM-BACKED:** the disassembly defines six 16-level color blocks:

- levels 1–16
- levels 17–32
- levels 33–48
- levels 49–64
- levels 65–80
- levels 81–99

The data bytes are represented locally as `.chunk 8`; the disassembly excerpt
does not provide the raw 8-byte block values. It is therefore
**UNRESOLVED** whether any entity-specific RGB color can be named beyond
cycle membership and existing C# symbol translations.

**Current C# architecture:**

- All current production configurations use `PaletteIndex: 0`.
- The tube/rails and fixed entity vectors use project RGB constants.
- Literal ROM palette bytes are absent from the host catalog.
- `ResolvePlayerLaneColor` supports only palette 0.
- Super Zapper tube rail indices are fixed 0–6 project translations.
- Fixed colors exist for Spikers, Tankers, and Pulsars.

Relevant source:

- [VectorRenderer.cs](../../src/Tempest.Game/Rendering/VectorRenderer.cs)
- [SuperZapperSystem.cs](../../src/Tempest.Game/Systems/SuperZapperSystem.cs)

**ARCADE/MAME-OBSERVED evidence in the repository**, accepted in durable
documentation, establishes a substantial 17-era palette cycle but not exact
ROM RGB indices or host values. The exact Level 17 screenshot mapping remains
unverified here because the images were not accessible.

The host should separate:

- reusable shape geometry
- displayed-level/cycle palette identity
- fixed entity-vector role translations
- presentation-calibration reuse

No 17–32 C# palette catalog exists.

## 6. Level 17 ROM wave values

The following values are derived from the ROM parameter tables for source level
`$11` (hexadecimal), the internal 17th source level:

| Item | ROM value | Evidence/class |
|---|---:|---|
| Ordinary wave supply | 20 | ROM-BACKED |
| Physical enemy slots | 7 | ROM-BACKED maximum index 6; host C# slot envelope is a translation |
| Minimum/maximum Flippers | 0 / 3 | ROM-BACKED |
| Minimum/maximum Tankers | 1 / 1 | ROM-BACKED |
| Minimum/maximum Spikers | 0 / 2 | ROM-BACKED |
| Minimum/maximum Pulsars | 2 / 2 | ROM-BACKED |
| Minimum/maximum Fuseballs | 0 / 0 | ROM-BACKED |
| Tanker cargo | two units, cargo type 1 | ROM-BACKED `tanker_load+2/+3=1`; project mapping identifies type 1 as Flippers |
| Raw starting Spike height | `$A0` | ROM-BACKED |
| Enemy shot count/slot | source level has last slot 1, yielding 2 slots | ROM-BACKED source `$9607_0f=02`; C# tick/index translation remains project behavior |
| Pulsar firing | disabled | ROM-BACKED: `$9607_03` enables `$40` only for levels `$3c+` |

Difficulty-specific wave counts beyond the shared base supply are
**UNRESOLVED** from the accessible listing. Level 17 has no established C#
difficulty policy.

## 7. Pulsar production requirements

### Existing implementation

- `Pulsar` already exists as a direct-hatched development actor.
- It has climbing, descending, lane pause, rim pause, flipping and terminal
  states.
- It has a shared pulse clock, pulse presentation, rim-pursuit behavior,
  player-collision handling, scoring, zapping, and directed lane transitions.
- It is isolated behind `PulsarTest`; natural levels do not admit it.
- Its pause constants use source `lv = 16` arithmetic.
- Its 51-update decision, 181-update pulse cycle, movement state and source
  timing are REFERENCE-DERIVED or PROJECT TRANSLATION, not complete ROM
  production evidence.

Source: [Pulsar.cs](../../src/Tempest.Game/Gameplay/Pulsar.cs),
[TempestGame.cs](../../src/Tempest.Game/TempestGame.cs),
[VectorRenderer.cs](../../src/Tempest.Game/Rendering/VectorRenderer.cs)

### Level 17 entry

**ROM-BACKED:** minima/maxima require two Pulsars.

**ROM-BACKED:** Pulsar firing is disabled on Level 17.

**ROM-BACKED:** Level 17 Pulse flip/timing parameters exist in the
`9607` table, but the accessible disassembly excerpt does not unambiguously
record the Level 17 raw byte for every pulse field. The existing pulse cycle
must not be promoted to a Level 17 ROM constant.

**Missing production integration**

- no Level 17 `LevelConfiguration`
- no normal Pulsar admission or identity in the wave ledger
- no Pulsar entry in `Level3OriginalType` or `Level3OriginalState`
- no generic cargo-Pulsar type
- no Level 17 movement-speed or pulse-rate configuration
- no Level 17 palette extension
- no direct-start/resolution entry

The smallest future implementation surface would extend configuration and
wave selection/ledger policy; it should reuse the existing actor and renderer
before redesigning them.

### Pulsar rendering

Existing rendering provides a six-segment zigzag glyph, pulse-white state,
active lane-boundary flashing, and Atari-local projection. This is suitable
as a reuse candidate. It is:

- arcade-observed in the isolated development scenario
- partly reference-derived in timing and motion
- project-translated in projection/color

No screenshot could be used to validate or retune it.

## 8. Tanker cargo

**ROM-BACKED:** both Level 17 Tanker cargo slots are type 1.

**PROJECT TRANSLATION:** existing cargo architecture represents cargo type 1
as Flippers. In the accessible source, type 1 is the only level-appropriate
value established here.

Cargo count is two. Level 17 has no ROM evidence establishing Pulsar cargo,
Tanker cargo random selection, or cargo variation. The cargo Flippers should
use the same Tanker ownership, latent/pending activation, re-entry, and
completion obligations as the existing generic ledger, not a parallel
accounting system.

The Level 17 cargo result is narrower than later-level behavior: no Pulsar
cargo is established for source Level 17.

## 9. Enemy movement and species

### Flippers

- Ordinary Flippers are present but no minimum requires them.
- Maximum is 3.
- Level 17 exact ROM movement-program byte is **UNRESOLVED** in the accessible
  local listing. The disassembly comment gives only the first 16-level
  sequence and does not establish the second-cycle Level 17 entry.
- Do not copy Level 16 `$24` merely because it precedes Level 17.
- Do not copy Level 1 behavior merely because geometry recycles.

### Tankers

- Minimum/maximum 1/1.
- Cargo: two Flippers.
- Movement: Tanker speed is derived from the Flipper speed in ROM startup.
- Level 17 raw Flipper/Tanker low-byte speed is `$AF` in the `9607_67`
  0x11–0x19 range.
- Separate ROM Tanker movement-program evidence was not established.

### Spikers

- Minimum 0, maximum 2.
- Raw Spiker speed is `$00` for source Levels 17–20.
- The no-Spiker minimum is still compatible with up to two emergent Spikers.
- The existing terminal-Spiker-to-Tanker conversion should be considered
  reusable only after the normal Level 17 wave policy is defined.

### Fuseballs

- Minimum 0, maximum 0 on Level 17.
- They must not enter the normal Level 17 composition.
- Their move probability/flag values are not needed for the Level 17 supply.

## 10. Spike, firing, speed, and admission

**Spikes**

- Raw `wave_spikeht` is `$A0` for source Level 17.
- C# height remains its existing accepted translation:
  `(0xF0 - raw) * Level.Depth / 224`.
- This result is not the raw ROM coordinate and should not be copied from
  Level 1 or Level 16.
- The reused Level 1 topology does not establish the placement lanes; existing
  production behavior initializes accepted Spikes across every playable lane
  before lane-specific evidence says otherwise.

**Enemy firing**

- Source Level 17 is outside Pulsar firing's `$3c+` range.
- Tanker, Flipper, and Spiker firing remain configuration-dependent and
  require separate Level 17 ROM fields before implementation.
- Level 17 `enm_shotspd` is shared `$C0`.
- Source Level 17 lies in the shot-holdoff `$14`/15–40 range.
- C# should not normalize the raw holdoff directly to ticks without an
  established project conversion.

**Admission and concurrency**

- ROM `max_enm = 06` establishes 7 physical record slots.
- ROM minima/maxima establish a mixed source of Flippers, Tanker cargo,
  Spikers, and Pulsars.
- Pulsars and cargo identities are absent from the existing ledger.
- The existing 7-slot project envelope can be reused conceptually, but Pulsar
  entries and cargo/Pulsar transitions require ledger coverage.

## 11. Death, reconstruction, and completion

The existing generic ledger tracks pending originals, Eggs, Tankers, cargo,
Spiker conversion, destroyed/rim-cleared states and re-entry. It does not
represent Pulsars as originals or cargo.

For Level 17, a future implementation must conservatively include:

- pending 20-unit supply
- original identities
- active and reserve Pulsars
- active Tanker and cargo Flipper identities
- Egg/re-entry reservations
- Spiker/Tanker conversion if it becomes admitted
- destroyed and rim-cleared actors
- all unresolved identities before completion

No new completion counter should be invented until the generic ledger extension
is designed. The established all-rim and remaining-enemy guards can be reused
only after Pulsars and cargo participate correctly.

## 12. Progression and direct startup

### Natural 16→17 progression

The future Level 17 configuration must:

- follow the Level 16 completion transition
- preserve score/lives
- recharge/state-reset Super Zapper according to existing level initialization
- select shape row 0
- install the 17–32 palette block
- initialize 20-unit supply and Level 17 parameters
- retain existing approach/starfield behavior unless separate evidence changes
  it
- resolve Atari presentation calibration from the canonical Level 1 shape for
  geometry, while retaining level-17-specific palette and gameplay data

### Direct startup

`--start-level 17` and `--level 17` are currently blocked.

The blocker is intentional and expected:

- `LevelProgression.GetDirectStartLevel` stops after Level 16.
- `GameplayStartupConfiguration` rejects any level not returned by the
  progression resolver.
- `AtariPresentationCalibration` throws for levels above 16.
- `RomPresentationMetadataCatalog` throws for levels above 16.

Natural and direct startup should use the same `LevelConfiguration`; future
direct-start implementation is not expected to create a separate wave path.

## 13. Presentation calibration

Level 17 should inherit shape-based tube geometry, model-frame mapping, tube
Y gain, scale, vertical offset and depth-Y policy from the canonical Level 1
shape only if the shape and presentation mapping are formalized that way.

It must not automatically inherit Level 1:

- gameplay enemy supply
- Flipper behavior
- Spike values
- shot cadence
- Palette/cycle
- displayed-level calibration if manual Level 17 acceptance changes placement

At this stage no Level 17 calibration can be recommended or encoded.

## 14. Levels 1–16 regression boundary

No replay was rerun. The established local full-campaign recording
`TEMP/GameplayRecordings/lvl1-16.json` remains a deterministic simulation
boundary through Level 16.

Future Level 17 implementation should add:

- focused ROM-table/configuration tests
- direct-start and natural-progression tests
- shape-reuse identity tests
- Level 17 ledger/admission tests
- Pulsar/Tanker cargo tests
- Levels 1–16 existing tests and the existing 16-level recording unchanged

The 16-level campaign and its presentation calibration should remain frozen
unless a concrete shared defect is demonstrated.

## 15. Architecture readiness

The architecture is partly ready:

- geometry is object-based and reusable
- production ledger infrastructure is available for several later levels
- Pulsar actor/rendering code is substantial
- direct-start and progression share a resolver
- calibration and ROM presentation metadata are centralized

It is not ready for Level 17 production without bounded extensions:

- remapping/catalog support above 16
- cycle-level palette data
- Level 17 configuration and wave selection
- Pulsar original/cargo ledger support
- Level 17 exact movement, firing, timing and startup integration
- direct-start resolver extension

## 16. Stop condition and unresolved findings

This investigation stops as **PARTIAL** for implementation preparation.

The following require later evidence or authorization:

1. exact Level 17 Flipper movement-program byte
2. exact Level 17 palette-block 8-byte values/RGB meaning
3. exact raw Pulsar flip/pulse timing values
4. difficulty-specific supply/difficulty adjustments
5. exact source conversion of holdoff/speed values to C# ticks
6. accessible supplied screenshot visual confirmation
7. exact Tanker/Pulsar cargo or firing behavior if source evidence changes

No ambiguity was converted into a Level 17 implementation requirement.
The prompt and this report are paired AI-Logs records for future review.
