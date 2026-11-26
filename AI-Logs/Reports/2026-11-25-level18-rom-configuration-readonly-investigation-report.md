# Displayed Level 18 ROM/Configuration Read-Only Investigation

Date: 2026-11-26

**Disposition: NOT READY - one shared Pulsar movement-translation blocker.**

No production behavior was changed. The Rev. 2 listing establishes Level 18's
eligibility, wave data, `$0B` Flipper program, cargo, speeds, firing policy,
shape row, and palette cycle. The remaining blocker is not missing ROM data:
the existing C# Pulsar has one fixed host climb rate and no configurable
ROM-along threshold, while Level 18 changes the ROM threshold from `$28` to
`$14` and uses two distinct ROM speeds. No accepted Level 18 host translation
exists for those differences. Do not solve this with a Level-18 Pulsar class
or silently reuse Level 17's one-speed approximation.

## 1. Executive summary

The Level 18 mapping is:

```
displayed 18
  -> ROM curlevel $11 (zero-based)
  -> effective ROM lev $12 (one-based)
  -> 16-level cycle 2, cycle-local position 2
  -> zero-based remap index 1
  -> ROM shape row 1 / canonical Shape 2
  -> palette cycle 1
```

Level 18 reuses the already-defined, closed, 16-lane canonical Level 2 square;
it does not copy coordinates. The ROM palette block is the existing
Levels 17-32 block `00 07 0B 08 44 03 0C 0C`. Current C# color resolvers already
select the cycle-1 role colors for displayed Level 18.

Default/medium ROM policy is 21 originals, Flippers 0-3, Tankers 1-1,
Spikers 0-0, Pulsars 2-5, Fuseballs 0-0, no starting or dynamic Spikes, two
Flipper Tanker cargo, ordinary Flipper program `$0B`, Flipper/Tanker raw speed
`$AC` (cracked `$FD.60`, magnitude 2.625), shot holdoff `$1D` (29 ROM ticks),
and an inclusive shot last-slot index of `$02` (three slots). Medium Pulsar
firing is off. Hard adds supply to 24, changes Flipper/Tanker raw speed to
`$A2`, enables Pulsar firing, and permits four enemy-shot slots; easy retains
21 supply, changes speed to `$B6`, and permits two slots.

The ROM selects two Pulsar speeds around its `$0157` along-position threshold:
fixed `$FE.A0` (magnitude 1.375) on one side and the Level 18 Flipper speed
`$FD.60` (magnitude 2.625) on the other. The ROM threshold is `$14`, versus
Level 17's `$28`. The shared C# `Pulsar.Update` currently moves at one host
unit per update in both directions and does not model this threshold. Because
the Level 17 host value is an accepted translation for Level 17 only, not a
Level 18 translation, this investigation stops at **NOT READY** pending a
bounded shared-Pulsar translation decision.

## 2. Starting Git/worktree state

Captured before the investigation:

- Branch: `master`
- HEAD: `0ce7e538374c4e155b75cf536b4f3a67c8da3de0`
- Upstream: `origin/master`
- Ahead/behind: `0/0`
- Staged changes: none
- Tracked worktree changes: none
- Untracked relevant file: `Reference/arcade/MBUCOD.V05.txt`
- The untracked reference file was preserved and not opened or searched.
- No other untracked files were reported.

This agrees with the current Level 17 final-closeout authority: its report-only
finalization followed the accepted Level 17 production commit. This task did
not modify Git state, stage, commit, or push.

## 3. Evidence authorities and limits

Primary local evidence:

- `Reference/arcade/Tempest Rom Source code.md`: Rev. 2 disassembly listing,
  including the parameter-table decoder, enemy admission, firing, P-code,
  shape/palette tables, and speed cracking.
- `src/Tempest.Game/Levels/LevelConfiguration.cs`,
  `Level17WaveSelection.cs`, `Level16WaveSelection.cs`, `LevelLoader.cs`,
  `RomPresentationMetadata.cs`, `AtariTubeRecordCatalog.cs`, and
  `LevelProgression.cs`: current shared configuration seams.
- `src/Tempest.Game/Gameplay/Flipper.cs`, `Tanker.cs`, `Pulsar.cs`,
  `EnemyBullet.cs`, `TempestGame.cs`, and the generic identity/ledger code:
  current shared lifecycles and translation limits.
- `TEMPEST-BEHAVIOR-SPEC.md`, `TEMPEST-DEVELOPMENT-NOTES.md`, and
  `.github/copilot-instructions.md`: accepted Level 17 behavior, palette,
  geometry, semantic-policy, and preservation rules.
- The final Level 17 closeout, ROM-map final evidence, ROM-to-C# integration
  readiness report, second-cycle palette/presentation reports, and the
  Levels 11/15/17 shared-lifecycle audit under `AI-Logs/Reports/`.

The Level 17 closeout records manual acceptance of Level 17, not Level 18.
Incitti was not used as gameplay authority. No screenshots, videos, gameplay
recordings, private scratchpad, raw ROM image, or Internet ROM material was
used. The untracked `MBUCOD.V05.txt` was not needed because the checked-in
disassembly and accepted Level 17 ROM-map evidence suffice.

Classifications below use **ROM-BACKED**, **ARCADE/MAME-OBSERVED**,
**EXISTING ACCEPTED PROJECT BEHAVIOR**, **PROJECT TRANSLATION / PROJECT
DECISION**, **REFERENCE-DERIVED**, and **UNRESOLVED**. No C# proposal is
promoted to ROM fact.

## 4. Displayed/internal/cycle-local mapping

The ROM stores `curlevel` zero-based. The parameter initializer adds one
before applying level-range tables. Therefore:

- displayed Level 18 = effective `lev $12` (decimal 18);
- internal `curlevel = $11` (decimal 17);
- `lwb = (((lev - 1) & $0F) + 1) = 2`;
- cycle number, one-based = `((18 - 1) / 16) + 1 = 2`;
- palette index, zero-based = `floor((18 - 1) / 16) = 1`;
- modulo-16 remap index, zero-based = `(18 - 1) & $0F = 1`;
- `lev_remap[1] = $01`, hence ROM shape row 1 / canonical Shape 2.

This is the same arithmetic used by the ROM table decoder and 16-level
remapping; it is not an extrapolation from Level 17.

**Current code boundary:** `RomTubeMappingCatalog.ResolveShapeRow`,
`RomPresentationMetadataCatalog.ForDisplayedLevel`, and
`AtariPresentationCalibration.ForDisplayedLevel` currently stop at displayed
Level 17. `CyclePaletteResolver` and `RomPaletteCatalog` already support
palette cycle 1 for displayed Levels 17-32.

## 5. Canonical Shape 2 reuse

**ROM-BACKED:** source Level 18 maps to row 1, the second zero-based row.
Displayed Level 2 is the accepted first-cycle level selecting that same row.

**EXISTING ACCEPTED PROJECT BEHAVIOR:** `LevelConfiguration.Level2Square`
and `LevelLoader.LoadLevel2Square()` are the canonical C# Shape 2 definition.
It is a closed square with 16 ordered vertices and 16 playable lanes; its
points are distinct and do not contain the repeated/shared-vertex
self-intersection seen in some later rows.

Future configuration should use `BoardIndex: 1` and the existing Level 2
definition identity. Do not duplicate its points or copy its gameplay policy.

## 6. Shape 2 production presentation reuse

**ROM-BACKED:** displayed Level 18 uses remap row 1. Its per-row presentation
metadata therefore reuses the Level 2 row-1 values: scale `$1C`, camera/Y3D
`$50`, signed Y2D `$FF20`, and closed topology (`lev_open=$00`).

**EXISTING ACCEPTED PROJECT BEHAVIOR:** the canonical Level 2 production
projection uses the Level 2 model-frame mapping `(1.436688312, +1)` and its
accepted calibration `(TubeYGain=1, TubeScale=1.1, VerticalOffset=35,
TubeDepthYGain=1)`. It must not inherit Level 17's Shape 1 / Level 1
projection or calibration.

**Current architecture gap, bounded:** the adapter currently tests
`displayedLevel == 2` for the Shape 2 projection branch; model-frame lookup,
metadata lookup, and calibration lookup reject displayed 18. The smallest
correct extension is shape-keyed canonical resolution so Level 18 routes
through the existing Shape 2/Level 2 projection and calibration. This is a
reuse/mapping change, not visual tuning or a new Level 18 transform. A new
Level 18 calibration value must not be invented.

Required future presentation chain:

```
Displayed Level 18
      -> canonical Shape 2 / row 1
      -> existing Level 2 geometry and model frame
      -> existing Level 2 production projection/calibration
```

## 7. Cycle-1 palette reuse

**ROM-BACKED:** Level 18 resolves palette index 1 and raw block
`00 07 0B 08 44 03 0C 0C`, identical to the already recovered
Levels 17-32 block.

**EXISTING ACCEPTED PROJECT BEHAVIOR:** cycle-1 semantic colors are already
resolved by displayed-level cycle, not shape:

| Role | Cycle-1 host translation |
|---|---|
| Tube, web, restored base rails | Red |
| Score/high score/initials HUD | Cyan |
| Reserve-life HUD | Green |
| Displayed-level HUD | Red |
| Player/Blaster lane | Yellow |
| Flipper | Magenta |
| Tanker | Blue |
| Egg | Yellow |
| Pulsar | Cyan; pulse transient returns to its active base role |

Transient Super Zapper/pulse behavior restores through the active cycle's
base-role resolver. Level 18 introduces no new eligible enemy color role:
Pulsars are already eligible and manually represented in Level 17's cycle-1
translation. No Level 18 color override is warranted. Manual Level 18
presentation acceptance remains required for the reused Shape 2 composition.

## 8. Eligibility before parameters

Eligibility is established from the min/max and feature-enable logic first.
Raw speed/height/color/movement values below do not independently enable a
species or object.

| Feature/species | Level 18 eligible? | ROM evidence and condition | Classification |
|---|---|---|---|
| Ordinary Flipper | Yes | `min_flippers=0`, `max_flippers=3`; nonzero maximum permits selection. Species firing bit `$40`. | ROM-BACKED |
| Tanker | Yes | `min_tankers=1`, `max_tankers=1`; Tanker selection routine has available cargo type. | ROM-BACKED |
| Spiker | No | `min_spikers=0`, `max_spikers=0` for source Levels 17-19. | ROM-BACKED |
| Pulsar | Yes | `min_pulsars=2`, `max_pulsars=5`. | ROM-BACKED |
| Fuseball/Fuzzball | No | `min_fuzzballs=0`, `max_fuzzballs=0` for source Levels 17-21. | ROM-BACKED |
| Starting Spikes | No | `wave_spikeht` cycle-local position 2 is `$00`; startup copies zero to all 16 spike slots. | ROM-BACKED |
| Dynamically grown Spikes | No | Only eligible Spiker P-code `$00` invokes the grow/reverse/convert Spike action; Spikers are ineligible and no starting Spike exists. | ROM-BACKED |
| Tanker cargo | Yes, conditional on Tanker | All four `tanker_load` entries resolve to type `$01` at source Level 18; the type is Flipper cargo, two units. | ROM-BACKED |
| Flipper firing | Yes | Species `$028A` firing flag `$40`; subject to common shot gates. | ROM-BACKED |
| Tanker firing | Yes | Tanker type `$41` includes `$40`; subject to common shot gates. | ROM-BACKED |
| Spiker firing | No active shooter | Spiker type has `$40`, but the species is ineligible. The bit is dormant, not an activation rule. | ROM-BACKED |
| Pulsar firing | Medium/easy: no; hard: yes | Pulsar type flag is `pulsar_fire`; base Level 18 range does not set it. Hard difficulty ORs `$40`. | ROM-BACKED |
| Fuseball firing | No | Fuseball maximum is zero; species flag is not an enable path. | ROM-BACKED |

## 9. Starting Spike eligibility

The ROM `wave_spikeht` payload is the established 16-position cycle:

`00 00 00 E0 D8 D4 D0 C8 C0 B8 B0 A8 A0 A0 A0 A8`

The table's cycle-local position is one-based. Level 18 gives
`lwb=2`, selects the second `$00`, and `set_enm_and_spikes` writes that zero
to all 16 `spike_ht` entries. Therefore Level 18 has no starting Spike
objects. The current generic C# helper returns `null` for displayed 18 if
called, but production configuration is not yet defined.

This value does not establish Spiker eligibility.

## 10. Spiker eligibility and dormant Spiker data

`min_spikers` covers Level 18 with `$00`; `max_spikers` covers source
Levels 17-19 with `$00`. Spikers cannot be selected. The ROM raw Spiker speed
table also gives `$00` for source Levels 17-20, and Spiker species have a
firing bit, but both are dormant parameters for Level 18.

Do not instantiate, speed, fire, retreat, convert, or reconstruct a Level 18
Spiker on the basis of those raw parameters. Since no Spiker is eligible,
Spiker min/max are 0/0 and all Spiker movement, retreat, terminal conversion,
and projectile policy are inapplicable. The accepted invariant that a
Spiker's projectile outruns that Spiker remains unchanged; it is vacuous for
Level 18.

## 11. Dynamic Spike policy

The only located growth action is Spiker P-code `$00` (`$0E` action: grow
Spike, reverse, convert). With zero starting Spike height and no eligible
Spiker, Level 18 permits no Spike object, including a dynamically generated
one. This is a separate conclusion from the starting-Spike table entry.

## 12. Wave supply

The `wave_enemies` table uses opcode `$08` for source Levels 17-26:

`base $14 + (lev - $11) * $01`

For source Level 18 (`lev=$12`), default/medium supply is `$15` = **21
originals**. This is the wave ledger total, not the simultaneous enemy count
and not extra Tanker cargo identities.

Difficulty:

- Easy: no supply adjustment; 21.
- Medium/default: 21.
- Hard: ROM computes `wave + (wave >> 3) + carry`. For `$15`, the three
  right shifts leave `$02` with carry 1; `$15 + $02 + 1 = $18` = **24**.

This is ROM selection/supply evidence, not visual enemy counting.

## 13. Species ranges, physical envelope, and selection arithmetic

| Species | Minimum | Maximum | Medium/default interpretation |
|---|---:|---:|---|
| Flipper | 0 | 3 | Optional; eligible because maximum is nonzero |
| Tanker | 1 | 1 | One is the ROM minimum and maximum |
| Spiker | 0 | 0 | Ineligible |
| Pulsar | 2 | 5 | Two is the ROM minimum; up to five |
| Fuseball | 0 | 0 | Ineligible |

`max_enm=$06` indexes the seven physical enemy records `0..6`; the ROM's
availability calculation uses `max_enm+1` (=7) and reserves carrier space.
The existing Level 17 C# selector models a seven-record physical envelope.
Tanker-held Flipper cargo reduces available Flipper-type capacity by two.
Cargo is not another original wave-supply count.

The ROM first computes per-type availability from max minus current counts,
subtracts each Tanker's latent cargo from its selected cargo species, and
limits each type to the remaining physical envelope. Unmet nonzero minima are
tried before ordinary fallback selection. With several minima outstanding,
the routine scans eligible types from a random start; once minima are met, it
uses the random-start/type availability loop. The special Spiker-versus-Tanker
spike-height choice is unreachable because Spikers are ineligible.

Minima are target counts in ROM admission; they are not a promise that every
type is simultaneously active in seven slots. The project ledger may reserve
the minimum original identities as it does for Level 17, while active
concurrency remains constrained by physical slots and carrier cargo. Do not
replace ROM selection with visual counting or a fixed Flipper/Tanker recipe.

## 14. Difficulty effects

| Parameter | Easy | Medium/default | Hard |
|---|---:|---:|---:|
| Original supply | 21 | 21 | 24 |
| Flipper/Tanker raw speed byte | `$B6` | `$AC` | `$A2` |
| Cracked signed 8.8 speed | `$FD.B0` | `$FD.60` | `$FD.10` |
| Flipper/Tanker magnitude | 2.3125 | 2.625 | 2.9375 |
| `enm_shotmax` last index | 1 | 2 | 3 |
| Inclusive enemy-shot slots | 2 | 3 | 4 |
| Pulsar firing bit | clear | clear | `$40` set |
| Species min/max | unchanged | table values above | unchanged |
| Shot holdoff | `$1D` | `$1D` | `$1D` |
| Pulsar threshold / pulse beat | `$14` / `$04` | `$14` / `$04` | `$14` / `$04` |

Easy/hard speed bytes follow the 6502 initializer arithmetic; calculated
fixed-point magnitudes are ROM parameter conversions, not C# per-frame speed
claims. This project has no general difficulty selection path for Level 18.
The bounded first implementation must name its selected difficulty (the
current Level 17 precedent is medium/default); do not silently enable hard
Pulsar fire.

## 15. Tanker cargo

Tanker eligibility is independent of the cargo byte. Level 18 is eligible
for one Tanker. The four-entry cargo vector resolves to `$01` in all entries:
the base first two are `$01`, and both source-level tables for
`tanker_load+2/+3` select `$01` at Level 18. `$01` means **two Flippers**;
it is a cargo species code, not a count.

Cargo type/data is separate from the shared Tanker release lifecycle,
two-unit cargo identity allocation, Egg/re-entry handling, and rim handoff.
Released Flippers use the Level 18 `$0B` movement program. The ROM Tanker
itself uses the shared straight-up P-code `$07` and the same cracked movement
speed as Flippers.

Current C# already has `Tanker.CarriedFlipperCount=2` and a shared cargo
identity/release path. `Tanker.GetCargoMovementProgram` has no Level 18 route
selection; it must resolve a semantic below-rim program, not add a separate
Tanker or cargo lifecycle.

## 16. Ordinary Flipper movement byte

The source `flipper_move` range begins at `$9607_5B` with opcode `$06`,
cycle-wide range 1-99, and 16 payload bytes:

`07 0B 19 24 53 0B 24 19 53 87 24 19 53 07 87 24`

Because `lwb=2`, Level 18 selects the second payload byte: **`$0B`**.
Level 17 selects `$07`; the two levels do not share the same below-rim
program.

## 17. Flipper P-code trace and rim handoff

ROM `$0B` enters at P-code offset `$0B` (`$A102`):

1. `$02,$08`: set per-enemy loop counter to 8.
2. `$0C`: move by that enemy type's current speed; the movement action returns
   to the P-code loop.
3. `$08,$0C`: decrement the counter and repeat the move while nonzero.
4. `$12`: start a lane flip; stop this P-code update.
5. `$14`: continue/end the flip; `$04` waits while the between-segments flag
   is set.
6. `$06,$0A`: branch to offset `$0B`; repeat the eight-move/one-flip cycle.

The route has no spike predicate and contains no explicit `$16` reverse
instruction. Direction is initialized by the shared enemy record and the
flip operation; the Level 17 C# `+1` direction is a project-side convention,
not independent proof of ROM handedness. The route has no separate
below-rim terminal action: the common move/end-of-tube path takes control at
rim arrival.

## 18. Existing C# movement-strategy match

`Flipper` already implements the same eight-along-advances,
hold-depth-during-turn `$0B` strategy for Level 6 Tanker cargo as
`Level6Deep0B` / `Level6DeepAlong` / `Level6DeepTurning`. That movement
mechanism can be reused for Level 18 ordinary Flippers and cargo; it is not
currently exposed as a selectable ordinary `BelowRimFlipperProgram`, and
Tanker selection is gated to Level 6. Wire the existing shared mechanism to
semantic route data and the shared rim handoff. No new Level 18 actor or new
`$0B` route algorithm is indicated.

## 19. Flipper/Tanker speed and ratio

The Level 18 table uses opcode `$08`: raw Flipper speed is
`$AF + 1 * $FD = $AC` (signed -84). `crack_speed` shifts the signed value
left three places, yielding `$FD.60`; its positive along-depth magnitude is
`$02.60 / $100 = 2.625`. Startup copies the cracked Flipper pair into the
Tanker speed pair.

| | Level 17 context | Level 18 |
|---|---:|---:|
| Raw medium byte | `$AF` | `$AC` |
| Cracked 8.8 word | `$FD.78` | `$FD.60` |
| Positive magnitude | 2.53125 | 2.625 |

**Current C# translation:** accepted Level 17 uses `2.53125f` per simulation
update for both Flipper and Tanker. There is no exact ROM-8.8-to-host-frame
converter. A ratio-preserving Level 18 translation is
`2.53125 * 84/81 = 2.625`; this preserves the signed raw-speed magnitude
ratio without manually tuning. This is a PROJECT TRANSLATION, not equality
between a ROM update and a C# frame. Level 18 should use one shared
Flipper/Tanker climb value, not separate actor speeds.

Easy/hard raw bytes and their cracked results are listed in section 14.
No random speed term is supported by this table.

## 20. Spiker speed and behavior

Spikers are ineligible, so the raw Spiker speed table's `$00` for Level 18
remains dormant. Do not instantiate a Spiker, apply the byte as speed, or
derive any retreat, terminal conversion, or firing behavior from it. Existing
shared Spiker behavior and the faster-projectile invariant are unaffected.

## 21. Pulsar policy and shared-actor blocker

**ROM-BACKED Level 18 data:**

- eligible, minimum 2 / maximum 5;
- fixed `spd_pulsar=$FE.A0`, signed magnitude 1.375;
- `pulsar_fliprate=$14`;
- the other movement region uses current source Level 18 Flipper speed
  `$FD.60`, magnitude 2.625;
- `pulse_beat=$04`;
- medium/easy firing disabled, hard firing bit `$40`;
- shared Pulsar P-code and common player collision/hazard policy.

At `$9CB5-$9D04`, P-code compares the Pulsar's along coordinate with
`$0157` (`pulsar_fliprate`) and uses the Pulsar speed or Flipper speed on the
respective side; the same threshold participates in reversal and pulse-lane
checks. Thus `$14` is behaviorally relevant, not decorative parameter data.
Level 17's threshold is `$28`; its fixed Pulsar speed and `$04` pulse beat
are the same.

**Current C# comparison:** one shared `Pulsar` actor exists and is already
used by Level 17. Its climb/descend increments are fixed at 1 host unit per
update; flip decision, lane/rim pause, turn rate, and global pulse clock are
fixed project/reference-derived host behavior. It has no configurable
ROM-along threshold and does not consume `$FE.A0`, the Level 18 Flipper speed,
or `$14`. `Level17WaveSelection` records raw values, but those constants do
not provide an exact conversion in `Pulsar.Update`.

**Blocker:** no accepted Level 18 PROJECT TRANSLATION maps the two ROM speed
regions and changed threshold to the fixed-step shared actor. The needed
change, if authorized, is generic Pulsar configuration/behavior, not
`Level18Pulsar`. Establish and test that translation before claiming the
complete Level 18 parameter specification is implementation-ready. Manual
Pulsar gameplay acceptance would still be required after that decision.

The existing shared actor, original identity, collision, pulse clock, zap,
and completion machinery remain reusable; no Level-18-specific identity or
lifecycle is needed.

## 22. Fuseball policy

Level 18 Fuseball min/max are 0/0 (source Levels 17-21). The species is
ineligible even if dormant movement/speed parameters exist elsewhere. No
pending-supply Fuseball terminal policy applies. The shared semantic
pending-supply predicate is not required for a Level 18 Fuseball because
there cannot be one.

## 23. Firing eligibility and gates

The shared ROM `enm_shoot` routine checks player state, active enemy record,
along position at least `$30`, species firing flag `$40`, shot-delay
expiration, non-death state, and a random threshold indexed by current
enemy-shot count. It then searches free slots starting at the inclusive
`enm_shotmax` last-slot index. The routine applies to eligible Flippers and
Tankers at Level 18. Spiker's `$40` flag is dormant because no Spiker is
eligible. Pulsar's flag is clear at medium/easy and set only on hard.

The ROM threshold bytes are `00 E0 F0 FA FF`: with the default three slots,
chance-to-pass is 100% at zero active shots, 12.5% at one, and 6.25% at two.
Hard's fourth slot uses `FA` (6/256). These are probability gates before
free-slot allocation, not guaranteed shot cadence.

Current C# uses shared Flipper/Tanker firing and `EnemyBullet`; its
`TempestGame` capacity/holdoff path can be configured from Level 18 data.
There is no current production Pulsar shooter, but no medium/default Pulsar
shot is eligible. Hard difficulty and hard Pulsar fire are outside the
current default-only production path and must not be silently enabled.

## 24. Shot holdoff, capacity, and speed

- Holdoff table: `$50 + 17 * (-3) = $1D`, 29 ROM ticks. No difficulty
  adjustment changes it.
- `enm_shotmax` for Levels 10-64 is last index `$02`; medium/default
  capacity is indexes 0-2 = three shots. Easy decrements to index `$01`
  (two shots); hard increments to `$03` and caps there (four shots).
- Enemy-shot table opcode `$0A` computes base `$C0 + spd_flipper_lsb` before
  speed cracking. Level 18 medium raw shot value is
  `($C0 + $AC) & $FF = $6C`; cracked word `$03.60`, magnitude 3.375.
  Corresponding easy/hard results are `$03.B0` (3.6875) and `$03.10`
  (3.0625), from the adjusted Flipper byte.
- Level 17's medium ROM result is `$03.78` (3.46875). The ROM shot ratio
  L18/L17 is `36/37`.

**Existing accepted C# translation:** `EnemyBullet.Advance()` moves 3 host
units per simulation update, shared by enemy species. The Level 17 report
classifies 3 as an accepted PROJECT TRANSLATION, not literal ROM speed. There
is no exact 8.8-to-host converter. Reusing the shared 3-unit projectile is
the bounded current architecture; a level-specific ratio-scaled shot speed
would be a new translation decision, not an already existing C# conversion.
No Spiker projectile comparison is active on Level 18.

## 25. Egg/admission behavior

ROM creates pending enemy identities from the wave supply and admits them
through the shared type/max/capacity selection routine. It does not declare
four C# Egg actor instances as a ROM count.

**EXISTING ACCEPTED PROJECT BEHAVIOR:** reuse the production Egg/reserve
pipeline: 4 initial active Eggs, max 4 active Eggs, max 2 approaching Eggs,
50-tick reserve-release/hatch intervals, deterministic 16-lane sequence, and
the existing alternating Egg direction sequence. These are C# project
configuration values, not ROM-backed Level 18 timing. The minimum Tanker and
Pulsar identities and the remaining Flipper/Pulsar/Tanker selections go
through the generic actor factory and ledger. No Level 18 Egg class is
required.

Admission capacity must account for the seven physical enemy slots and a
Tanker's two latent cargo Flippers. A blocked hatch remains an Egg, consistent
with current shared behavior.

## 26. Ledger and identity compatibility

Every eligible original species is already represented by the shared generic
identity model: Flipper, Tanker, Pulsar. Tanker cargo uses the separate
two-unit cargo identity namespace and Flipper runtime representation.
Pulsars already have original identity and active state support from Level 17.
No Level 18 type, ledger, counter, or parallel identity store is required.

## 27. Death/reconstruction compatibility

For Flipper, Tanker, Pulsar, and Tanker cargo:

- original/cargo identity representation: already supported;
- reserve/Egg reconstruction: shared production path;
- active actor reconstruction: generic ledger-backed path;
- terminal/rim/cleared representation: generic lifecycle;
- Level-specific reconstruction: none required.

The new movement route must be selected by shared configuration on fresh
cargo re-entry as well as natural release, preserving identity rather than
creating new cargo.

## 28. Completion compatibility

All eligible originals, cargo identities, reserves, Eggs, hatch-pending
actors, active Flippers/Tankers/Pulsars, and terminal actors use the existing
generic completion obligations. No Pulsar- or Level-18-specific completion
counter/branch is required. Completion remains blocked while any generic
identity or pending/active representation remains unresolved.

## 29. Semantic lifecycle-policy mapping

| Shared policy | Level 18 participation | Semantic property/seam |
|---|---|---|
| Ordinary species selection | Yes | Configured eligibility/min/max and Flipper-selection policy |
| Tanker cargo selection/release | Yes | Two Flipper cargo identities plus selected `$0B` movement strategy |
| Shared rim lifecycle | Yes | Program route hands off at rim; common Flipper rim policy |
| Generic death reconstruction | Yes | Existing `UsesGenericRomDeathReconstruction` semantic property |
| Generic identity/completion ledger | Yes | Existing original/cargo identity state |
| Spiker pending-supply conversion | No | No eligible Spiker |
| Fuseball pending-supply terminal movement | No | No eligible Fuseball |
| Starting/dynamic Spike policy | No Spike actors | Raw zero and no eligible Spike grower |
| Flipper/Tanker firing | Yes, medium/default | Shared shot holdoff, capacity, random and depth/state gates |
| Pulsar firing | Hard only | ROM flag; current medium/default remains disabled |

The current integration contains several explicit target-wave flags in
`TempestGame` and `Tanker`. Level 18 must not add repetitive flag lists for
generic reconstruction or terminal policies. Reuse semantic configuration
properties where they already describe the lifecycle; the Level 18 wave
selector/configuration supplies its distinct data. The only necessary
generic-architecture extension found by this review is making the existing
`$0B` route and Pulsar parameters configuration-selectable on shared actors.
No target-level actor/lifecycle workaround is warranted.

## 30. Natural Level 17 -> 18 progression

Current `LevelProgression.GetNextProductionLevel` ends at Level 17.
Future bounded wiring should add one Level 17 -> Level 18 link and create a
single canonical `Level18Production` configuration. The existing fresh-wave
path carries score/lives and applies normal level-entry/Super Zapper state;
no separate transition implementation is indicated. The active displayed
level becomes 18, while board row 1/palette cycle 1 resolve independently.

## 31. Direct Level 18 startup

Current `GetProductionLevel`, `GetDirectStartLevel`, and CLI validation derive
availability from the production progression chain. Adding Level 18 to that
chain should make `--start-level 18` and supported diagnostic `--level 18`
resolve to the same `Level18Production` instance. Both must not create
separate policies. HUD uses displayed level 18. Existing score/lives startup,
player lane, approach/starfield, and Super Zapper initialization remain shared.

## 32. Terminal boundary

For the bounded Level 18 milestone, Level 18 is the last production
configuration and remains terminal after completion. `GetNextProductionLevel`
returns null after Level 18; Level 19 and later remain unsupported. Do not
extend ROM cycle support or campaign progression beyond the requested bound.

## 33. Presentation implications

- Shape: canonical Shape 2 / Level 2.
- Metadata: Level 2 row 1 values; closed 16-lane square.
- Model frame/projection/calibration: existing Level 2 production route and
  accepted calibration `(1, 1.1, 35, 1)`.
- Palette: cycle 1, independent of shape.
- Entity projection/scaling: shared production projector, no separate
  Level 18 entity scale.
- Cycle-1 roles: red tube/web, cyan score HUD, green reserve lives, red level
  HUD, yellow Blaster lane, magenta Flipper, blue Tanker, yellow Egg, cyan
  Pulsar/white pulse transient.
- No new cycle-1 semantic role becomes eligible.

No visual tuning or image inspection was performed. Manual visual acceptance
of Level 18 should confirm the reused Shape 2 production route and cycle-1
roles after implementation.

## 34. Consolidated Level 18 implementation-readiness matrix

| Field | Value | Evidence / classification | Existing C# seam | New code? | Manual acceptance? |
|---|---|---|---|---|---|
| Displayed/internal level | 18 / `curlevel=$11`, effective `lev=$12` | ROM-BACKED arithmetic | `LevelConfiguration.DisplayedLevel` | Yes, config/progression | No |
| Progression index | 17 (zero-based) | Existing progression convention | `LevelProgression` | Yes, link | No |
| Cycle-local position | 2 (one-based), remap index 1 | ROM-BACKED | `RomTubeMappingCatalog` | Yes, allow cycle 2 | No |
| Shape / board | Row 1 / canonical Shape 2; board index 1 | ROM-BACKED + accepted Level 2 definition | `Level2Square`, `LoadLevel2Square()` | Reuse + catalog extension | Yes, presentation |
| Topology/lanes | Closed, 16 vertices/lanes | ROM-BACKED + existing definition | `LevelDefinition` | No geometry | Yes, presentation |
| Palette | Index 1; `00 07 0B 08 44 03 0C 0C` | ROM-BACKED | `RomPaletteCatalog`, cycle resolvers | No palette data | Yes, presentation |
| Base supply | 21 originals, medium/default | ROM-BACKED | Level wave selector/config | Yes, value | Gameplay check |
| Easy/hard supply | 21 / 24 | ROM-BACKED difficulty code | No Level 18 difficulty model | Only if difficulty enabled | If enabled |
| Initial Eggs/reserve | 4 initial; existing max 4, approaching 2; 50-tick pacing | Existing accepted project behavior | `LevelConfiguration` Egg fields | Config reuse | Gameplay |
| Active/physical envelope | 7 records, indexes 0-6 | ROM-BACKED + existing C# selector convention | `Level17WaveSelection.MaximumPhysicalEnemySlots` | Reuse/config | Gameplay |
| Flipper range | 0-3 | ROM-BACKED | Existing ROM wave-selection seam | Configuration data | Gameplay |
| Tanker range | 1-1 | ROM-BACKED | Existing Tanker selector/ledger | Configuration data | Gameplay |
| Spiker range/eligibility | 0-0, absent | ROM-BACKED | Generic selector | No actor admission | Gameplay absence check |
| Pulsar range | 2-5 | ROM-BACKED | Existing Pulsar identity/selector | Configuration data | Gameplay |
| Fuseball range/eligibility | 0-0, absent | ROM-BACKED | Generic selector | No actor admission | Gameplay absence check |
| Starting Spikes | No; `wave_spikeht=$00` | ROM-BACKED | `StartingSpikeHeightForDisplayedLevel` | Config resolution | Gameplay absence check |
| Dynamic Spikes | None; no Spiker | ROM-BACKED | Shared Spiker growth only | No | Gameplay absence check |
| Tanker cargo | Two Flippers; all cargo type entries `$01` | ROM-BACKED | Shared two-unit cargo/ledger | Route selection wiring | Gameplay |
| Flipper program | `$0B`; eight advances, flip, repeat | ROM-BACKED | Level 6 deep-cargo route exists | Expose shared route to ordinary Flippers | Gameplay |
| Cargo route | `$0B`, same route program on release/re-entry | ROM-BACKED | `Level6Deep0B` only currently selected for Level 6 | Semantic selection wiring | Gameplay |
| Flipper/Tanker speed | `$AC`, `$FD.60`, magnitude 2.625 default | ROM-BACKED + ratio-preserving PROJECT TRANSLATION | Level 17 speed translation | Config value | Gameplay |
| Pulsar speeds/threshold | `$FE.A0` (1.375), Flipper `$FD.60` (2.625), threshold `$14` | ROM-BACKED; host translation UNRESOLVED | Shared `Pulsar` fixed at 1/update | Yes, generic shared parameterization/translation | Required |
| Pulsar pulse beat | `$04`, same as Level 17 | ROM-BACKED; host clock is project translation | Shared global pulse clock | No new raw value; mapping remains approximate | Gameplay |
| Spiker speed | Raw `$00`, dormant | ROM-BACKED; ineligible | Shared Spiker | No Level 18 use | No |
| Shot holdoff | `$1D` = 29 ROM ticks | ROM-BACKED; direct host-tick use would be PROJECT APPROXIMATION | `FlipperShotHoldoffTicks` | Yes, config value | Gameplay |
| Shot last index/capacity | `$02` / 3 medium; easy 2, hard 4 | ROM-BACKED | `MaximumActiveEnemyShots` | Yes, config value | Gameplay |
| Shot speed | `$6C` -> `$03.60` = 3.375 ROM units | ROM-BACKED; shared host 3/update is accepted PROJECT TRANSLATION | `EnemyBullet.Advance()` | No for current 3-unit host policy | Gameplay |
| Firing species | Flipper/Tanker; Pulsar hard only; no Spiker/Fuseball | ROM-BACKED | Shared Flipper/Tanker loops; no Pulsar shooter | Hard-only support out of default scope | Gameplay |
| Firing random/depth/state gates | `$30` along threshold; delay, alive flag, `$00/E0/F0/FA/FF` thresholds | ROM-BACKED | Shared C# pressure/capacity path | Preserve/reuse; test | Gameplay |
| Difficulty-related changes | Easy `$B6`; hard `$A2`, supply24, Pulsar fire; holdoff/min/max unchanged | ROM-BACKED | No general Level 18 difficulty selector | Only for a named difficulty mode | If enabled |
| Reconstruction | Flipper/Tanker/Pulsar plus two cargo Flippers | Existing accepted generic lifecycle | Shared original/cargo identity ledger | No new identity class | Regression/manual |
| Completion | Generic identities, Eggs, cargo, active/terminal actors | Existing accepted generic lifecycle | Generic completion/ledger | No counter | Regression |
| Semantic policies | Generic reconstruction, selector, cargo, firing, rim handoff | Existing semantic properties/shared calls | `LevelConfiguration`, `TempestGame`, `Tanker` | Wire policy data; avoid level-flag lists | Regression |
| Production presentation | Shape 2/Level 2 model + calibration | Existing accepted project behavior | Adapter/catalog/calibration | Shape-keyed extension needed | Yes |
| Natural progression | Level 17 -> 18 | Project progression decision | `LevelProgression` | Yes | Yes |
| Direct startup | `--start-level 18`, supported `--level 18`, same config | Existing CLI resolver | `DebugController` / progression | Yes | Yes |
| Terminal boundary | Level 18 terminal; Level 19 unsupported | Project decision requested for milestone | `GetNextProductionLevel` | Yes | Yes |

## 35. Configuration-only items

Once the shared Pulsar translation is decided, these are data/configuration:
displayed level and progression index; board/palette identity; 21 supply;
species minima/maxima; cargo type/count; `$0B` movement selection; medium
Flipper/Tanker translation; `$1D` holdoff; three-shot default capacity;
Pulsar `$14` threshold, `$FE.A0` speed input, and `$04` pulse beat; and the
explicit medium/default difficulty selection. The Level 2 shape and cycle-1
colors are reuse, not new values.

## 36. Existing shared-strategy items

- `$0B` ordinary Flipper movement already exists as the Level 6 deep cargo
  route; expose/select that same shared route for ordinary Level 18 Flippers.
- `$0B` Tanker cargo movement reuses the same eight-advance/turn implementation.
- Existing shared rim handoff, Tanker cargo release, Flipper/Tanker firing,
  Eggs, identity ledger, Pulsar actor type, reconstruction, completion,
  projection, and cycle-1 palette resolvers remain the required foundations.

## 37. New reusable-strategy requirements

No new below-rim Flipper algorithm is required by the ROM evidence. The
existing Level 6 `$0B` route matches the P-code's eight movement advances,
held depth during pivot, and repeated cycle. The required work is selectable
semantic configuration for ordinary Flippers and Tanker cargo, then common
rim handoff. Do not call it `Level18Flipper`.

## 38. Architecture-extension requirements

The shared Pulsar configuration/update path cannot currently represent the
Level 18 threshold and two ROM movement speeds. A bounded generic Pulsar
parameterization/host-translation decision is required; no target-level
subclass, parallel lifecycle, or Pulsar-specific completion counter is
indicated. The shape metadata/projection/calibration resolution must also
become canonical-shape keyed so the Level 2 route can be reused for displayed
18.

## 39. Project translations and decisions

- Convert Flipper/Tanker signed ROM 8.8 speed magnitude to a shared fixed-step
  host value using the preserved Level 17 ratio; Level 18 medium result 2.625.
- Reuse the accepted shared 3-unit C# enemy projectile as a project
  translation; do not claim exact 8.8 equivalence.
- Reuse current project Egg pacing and identity lifecycle; these are not ROM
  counts/timings.
- Preserve the shared rim lifecycle after the `$0B` below-rim route.
- Keep Level 18 terminal and Level 19 unsupported for this bounded milestone.
- Do not transfer Level 17's Pulsar host speed/threshold to Level 18 as an
  accepted translation. Resolve this first.

## 40. Manual-acceptance requirements

After implementation and its automated gates, Neal must manually accept:

- Level 18 Flipper `$0B` cadence, direction, climb rate, Tanker cargo and
  shared rim handoff;
- Pulsar movement and pulse timing after the shared host translation is
  explicitly chosen;
- shot pressure/holdoff as experienced in play;
- Shape 2 production projection with the cycle-1 palette and semantic colors;
- natural Level 17 -> 18 entry, direct Level 18, score/life/Super Zapper
  continuity, Level 18 terminal behavior, and no Level 19.

Manual acceptance is not itself the blocker; the missing shared Pulsar host
translation is.

## 41. Recommended regression/test plan

Add focused permanent coverage for:

1. Displayed/internal/cycle-local arithmetic and shape-row 1/board 1 reuse.
2. Level 18 ROM raw palette block, palette index 1, role colors, transient
   restore, and separation from shape identity.
3. Eligibility-before-parameters for all five species, starting Spike zero,
   Spiker zero, and no dynamic Spike path.
4. Medium supply 21; hard supply 24; min/max and seven physical slots.
5. Tanker cargo type `$01`, exactly two Flipper cargo identities, capacity
   accounting, natural/shot/zap release, and death re-entry.
6. `$0B` P-code cadence and no below-rim spike gate; route through common rim
   handoff for ordinary and cargo Flippers.
7. Speed raw bytes and exact cracked 8.8 calculations; medium/hard/easy
   differences; shot holdoff and inclusive last-slot capacity.
8. Firing species, random thresholds, depth/state gates, Pulsar default-off
   and hard flag representation.
9. Shared Pulsar parameterization/translation tests, including Level 17
   regression preservation.
10. Generic identity conservation, death/reconstruction and completion for
    Flipper/Tanker/Pulsar/cargo with no special counter.
11. Natural Level 17 -> 18; direct `--start-level 18`; supported `--level 18`;
    HUD/displayed identity; score/lives/Super Zapper; Level 18 terminal and
    Level 19 rejected.
12. Shape 2 projection/calibration equality with its Level 2 canonical path;
    no copied geometry or entity scaling.
13. Levels 1-17 production regressions, especially Level 2 projection, Level
    17 accepted Pulsar behavior, Level 15 policy, Level 16, and existing
    gameplay-recording oracles where authorized/applicable.

Do not run broad regression suites during this read-only investigation.

## 42. READY/NOT READY result

## NOT READY

## 43. Exact blockers

Concrete blocker:

1. **Level 18 shared Pulsar host translation is unresolved.** ROM Level 18
   uses a threshold `$14`, fixed Pulsar speed `$FE.A0`, and Flipper speed
   `$FD.60`; `Pulsar.Update` currently uses one fixed host movement increment
   and has no threshold parameter. The accepted Level 17 one-unit host
   behavior does not establish an accepted translation for Level 18's
   different ROM parameters. Resolve this for the shared Pulsar actor before
   calling the configuration implementation-ready.

Other work is bounded and does not independently block: shape-keyed reuse of
Level 2 presentation; Level 18 wave/configuration wiring; the selectable
shared `$0B` route; startup/progression insertion; and manual acceptance.

## 44. Files/resources inspected

- `TEMPEST-BEHAVIOR-SPEC.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `.github/copilot-instructions.md`
- `AI-Logs/Reports/2026-11-25-level17-final-closeout-report.md`
- `AI-Logs/Reports/2026-11-25-level17-rom-map-final-evidence-report.md`
- `AI-Logs/Reports/2026-11-25-level17-rom-to-csharp-integration-readiness-investigation-report.md`
- `AI-Logs/Reports/2026-11-25-level17-production-presentation-second-cycle-palette-investigation-report.md`
- `AI-Logs/Reports/2026-11-25-level17-shape1-projection-second-cycle-palette-implementation-report.md`
- `AI-Logs/Reports/2026-11-25-level17-rail-tanker-egg-palette-refinement-report.md`
- `AI-Logs/Reports/2026-11-25-level11-15-17-behavior-special-case-architecture-audit-report.md`
- `Reference/arcade/Tempest Rom Source code.md`
- Current C# configuration, progression, presentation, Flipper, Tanker,
  Pulsar, EnemyBullet, and TempestGame source noted in section 3.
- `tests/Tempest.Game.Tests/Level17ProductionTests.cs` for current Level 17
  reuse and regression seams.
- The attached user task snapshot, copied unchanged to the paired prompt path.

The untracked `Reference/arcade/MBUCOD.V05.txt` and all protected TEMP/UserData
assets were not inspected.

## 45. Commands/calculations run

- Read-only Git baseline: branch/HEAD/upstream/ahead-behind, index diff,
  tracked diff, and untracked path listing.
- Bounded `rg`, file, and source-range reads for the authorities and symbols
  listed above.
- PowerShell arithmetic verified Level 18 easy/medium/hard Flipper and shot
  8.8 values, `$1D` holdoff, hard supply 24, and shot-slot capacities.
- No tests, builds, recording playback, game launch, screenshots, videos,
  ROM-image reads, or Internet searches were run.

## 46. Production code changed: no

No production code was changed.

## 47. Tests changed: no

No tests were changed.

## 48. Durable docs changed: no

No durable behavior/development documentation was changed.

## 49. Stage/commit/push: none

No files were staged, committed, or pushed.

## 50. Level 19+ implementation: none

No Level 19+ implementation was made. Protected or untracked reference
material was not modified.

The prompt was copied byte-for-text from the read-only attachment and verified
against its source content. The report is this required paired artifact.

## 51. Recommended next step

Run a bounded follow-up on the ROM `$9CB5-$9D04` Pulsar threshold
path versus the shared C# `Pulsar.Update` translation, then make an explicit
project decision for translating the Level 18 `$14` threshold and two speed
regions without reopening accepted Level 17 behavior. After that decision,
the Level 18 work can proceed as configuration plus shared `$0B` route wiring,
shape-keyed Level 2 presentation reuse, progression/startup, and focused
regressions. Stop for Neal's manual Level 18 acceptance after automated
validation.

**Production code, tests, and durable behavior docs remain unchanged.**
