# Levels 1-7 enemy supply and ROM reconciliation

Date: 2026-11-08

## Executive findings

- **BASELINE SUPPLY MATCHES:** The C# configured original supply is 10, 12, 15,
  17, 20, 22, and 20 for Levels 1-7. The local ROM `wave_enemies` table gives
  the same values for its base/default table entries.
- **CONDITIONAL TOTAL MISMATCH:** The ROM's hard-difficulty branch adds
  approximately one eighth of the table count using its 6502 shift/carry
  sequence. The resulting Level 1-7 counts are 11, 14, 17, 19, 23, 25, and
  23. Current C# configurations have no difficulty adjustment and remain at
  the base values. This is a proven count difference only when comparing
  against the ROM's hard setting; the cabinet difficulty used for Neal's
  observation is unknown.
- **STRONGER LIKELY VISUAL DIFFERENCE:** C# Levels 1-6 do not implement the
  ROM's universal six-active-enemy creation gate, and their active type limits
  differ. C# Levels 3-7 also use a 50-tick Egg/release model (six-at-a-time for
  ledger-backed levels), unlike the ROM's delayed generic pending entries.
  These differences can change how many enemies are concurrently visible
  without changing the configured base supply.
- **PROVEN TYPE-ENTRY DIFFERENCES:** The ROM's minimum-type table and picker
  gate make ordinary bottom Flippers selectable on Levels 1-4, but not
  Levels 5-7. Current C# permits direct Flipper originals on Levels 5 and 6;
  Level 7's dedicated picker correctly excludes them. C# Tanker and Spiker
  concurrent limits also differ on Levels 3-6.
- **TANKER CARGO IS NOT A SIMPLE COUNT ERROR:** A ROM Tanker consumes one
  generic pending unit; its two Flipper children are later materialized as
  active enemies without another `enemies_pending` decrement. C# represents
  each carrier as one original identity and its two children as separately
  owned cargo identities. Therefore visible enemies can exceed the original
  wave-unit total in both models.
- **LEVEL 3 AND LEVEL 7 ACCEPTED WORK:** Level 3's 15-original C# ledger,
  two-child Tanker ownership, and exact identity reconstruction remain the
  accepted C# design; they are not a claim that the ROM has an identity
  ledger. Level 7's later ROM-specific no-direct-Flipper, generic re-entry,
  and same-unit final-Spiker conversion are present in current C# code and
  tests. An earlier Level 7 investigation's “current C# comparison” predates
  those corrections and is historical, not current behavior.
- No production or test changes were made. No game, build, or test suite was
  run. No arcade count or recording was newly observed, so the exact level
  Neal noticed and the cabinet difficulty remain unknown.

## Starting Git state and scope

- HEAD: `d2d8b1ef69b4a13be06ab59b41a1c9d38a8dd6e2`, the requested baseline.
- Branch: `master...origin/master`.
- Initial index and tracked worktree were clean. `screenshot000.png` through
  `screenshot013.png` were already untracked and were left untouched.
- This was a read-only investigation of production Levels 1-7. The requested
  prompt copy and this report are the only files created, both under ignored
  `AI-Logs/`.

## Prior evidence reviewed

Durable project evidence:

- `TEMPEST-DEVELOPMENT-NOTES.md`, including the Level 3 ledger/ownership
  diagrams, Level 4-6 source-derived wave notes, and Level 7 supply/re-entry
  acceptance notes.
- `TEMPEST-BEHAVIOR-SPEC.md`, especially the Level 3 and Level 7 evidence and
  manual-acceptance classifications.

Prior AI reports:

- `2026-11-01-c3-level3-mixed-wave-implementation-report.md`
- `2026-11-01-c3b-level3-mixed-wave-implementation-report.md`
- `2026-11-04-level7-reentry-rom-investigation-report.md`
- `2026-11-04-level7-arcade-corrections-implementation-report.md`
- `2026-11-01-level4-d1-production-candidate-implementation-report.md`
- `2026-11-03-level5-implementation-report.md`
- `2026-11-04-level6-implementation-report.md`
- `2026-10-30-tanker-analysis.md` and
  `2026-10-30-tanker-cargo-direction-correction-report.md`

Relevant current code/tests inspected:

- `Levels/LevelConfiguration.cs`, `LevelProgression.cs`, `LevelLoader.cs`,
  `Level.cs`, `Level3WaveLedger.cs`, `Level3WaveSelection.cs`, and
  `Level7WaveSelection.cs`
- `TempestGame.cs`, `Gameplay/Egg.cs`, `Gameplay/Spiker.cs`, and the wave HUD
  in `Rendering/VectorRenderer.cs`
- `LevelConfigurationTests.cs`, `Level3MixedWaveTests.cs`,
  `Level3WaveLedgerTests.cs`, `Level7ProductionTests.cs`,
  `Level7FlipperMovementTests.cs`, and `LevelProgressionTests.cs`

Only the project-local annotated listing
`Reference/arcade/Tempest Rom Source code.md` was used to establish ROM
counts and rules. No internet disassembly or Incitti implementation was used
to establish ROM supply. Incitti-derived reports were reviewed only as prior
project-history context; they do not override the local ROM listing.

## Current C# supply architecture

### Configuration and identity meaning

`LevelProgression` maps the natural production chain through the static
configurations in `LevelConfiguration`: Level 1, Level 2 square, Level 3
mixed, Level 4, Level 5, Level 6, then Level 7. Geometry comes from
`LevelLoader`; `Level` owns mutable Eggs, reserve, Spikes, and accounting;
`TempestGame` performs hatching, enemy selection, Tanker release, death, and
completion.

For Levels 1-2, `EnemySupply` is a fixed one-Egg/one-Flipper obligation
count. For Levels 3-7, `Level3WaveLedger.OriginalCount` begins at
`EnemySupply`. It creates no Tanker cargo IDs until an original is committed
as a Tanker. That commitment allocates two `(owner original ID, cargo
ordinal)` identities exactly once. Those two cargo identities are separate
obligations, not two more original wave slots. Consequently `TotalIdentityCount`
and the number of visible enemies can grow beyond `EnemySupply`.

The ROM has no equivalent per-original identity ledger. Its scalar pending
counter and active slot counters support comparison of wave units and
materialized enemies, but equating those counters with C# identity IDs is a
project interpretation, not a ROM fact.

### C# initial supply and type policy

All seven configurations start with four Eggs. Their configured reserve is
`EnemySupply - 4`. Levels 1-2 contain only Flippers. Level 3 selects
Flippers/Tankers; Levels 4-6 select Flippers/Tankers/Spikers. Level 7 uses the
same broad composition enum but a dedicated Level 7 picker that selects only
Tankers and Spikers for ordinary bottom entry.

There are no production Pulsars or Fuseballs in these configurations.
Tankers carry two Flippers. In ledger-backed waves, a Tanker is an original
identity and the cargo Flippers are separately owned identities; natural
release, shot destruction, Super Zapper expiry, and reconstruction follow
different ledger transitions.

### Release, active caps, and pacing

- Levels 1-2 release one reserve Egg when the 50-tick release timer expires
  and the current Egg collection has room below four. Egg approach selection
  is on a 50-tick timer with at most two Eggs approaching.
- Levels 3-7 release up to six reserve/cargo Eggs together per 50-tick
  release interval. In this ledger-backed path, `MaximumActiveEggs = 4` is
  not the release cap. The approach gate allows at most two approaching Eggs
  and stops approaching when the active count reaches nine.
- Levels 1-6 have no hard six-active-enemy limit in the production hatch
  path. Level 7's picker gates ordinary original selection at six active
  enemy units. The six cap is not a cap on queued Eggs, and released Tanker
  cargo can add active children outside the ordinary original-selection
  step.
- Species are selected at Egg hatch, not encoded as a fixed per-wave
  distribution at initial release. Level 3 and Levels 4-6 use their shared
  C# random selectors; Level 7 uses its dedicated quota/cap selector.

These are C# scheduling policies. They should not be read as ROM timing or a
fixed number of simultaneous enemies.

### Spikes, death, and completion

Level 4-7 create 16 starting Spikes, one per lane, at C# height 60. Levels
1-3 create none. Spiker-created Spikes are persistent hazards, not extra
enemy identities; neither projectiles nor Spikes enter enemy-supply totals.

For Levels 1-2, death discards active runtime enemies and rebuilds only
unresolved supply as Eggs; destroyed and rim-cleared units remain resolved.
For Levels 3-6, the ledger retains unresolved original/cargo IDs, selected
species, and terminal outcomes; unresolved materialized cargo returns as
cargo Eggs. Level 7 opts into a separate ROM-shaped policy: it clears
unresolved original species, discards latent cargo attached to a carrier,
and converts unresolved released cargo into generic re-entry records while
retaining ledger accounting.

Ledger-backed completion requires all unresolved original and cargo IDs
resolved, no reserve or re-entry reserve, no Eggs, and no active/pending
enemy representations. Ordinary Levels 1-2 use destroyed plus rim-cleared
counts and require no remaining Egg/reserve/active enemies.

The C# HUD distinguishes the ledger-backed `ORIGINAL`, `CARGO`, and `ACTIVE`
counts. `ORIGINAL` is not the number of visible sprites; `CARGO` is separate,
and `ACTIVE` includes Eggs and listed enemy objects. The ROM has no matching
identity-labelled HUD ledger.

## ROM supply, type tables, and control flow

### Count source and effective level

The ROM stores `curlevel` zero-based. `$92c5-$92d4` derives the effective
table level as `curlevel + 1`; the shared parameter-table evaluator at
`$92d6-$9326` applies level-ranged values from the pointer vector beginning
at `$9607`. The `wave_enemies` row is table `$9607_4f`, stored in RAM at
`$015b`. For displayed Levels 1-7 its base values are the first seven bytes:

`$0a, $0c, $0f, $11, $14, $16, $14` = `10, 12, 15, 17, 20, 22, 20`.

At new-level setup (`$9009-$900c`, `$9181-$91a4`), `$9234-$9237`
copies `wave_enemies` into `enemies_pending` (`$03ab`). `$9246-$926e`
initializes pending segment/visibility-delay entries for that generic count.
The ROM does not create four initial Egg objects or define an initial-four /
reserve partition. The pending entries are generic delayed entries, not
species-labelled original identities.

At hard difficulty, `$934d-$9377` adds the ROM's shift/carry-derived one-eighth
adjustment to `wave_enemies` before setup copies it. The resulting counts are:

| Level | ROM table base | ROM hard branch | C# fixed configuration | Difference on hard |
|---:|---:|---:|---:|---:|
| 1 | 10 | 11 | 10 | +1 |
| 2 | 12 | 14 | 12 | +2 |
| 3 | 15 | 17 | 15 | +2 |
| 4 | 17 | 19 | 17 | +2 |
| 5 | 20 | 23 | 20 | +3 |
| 6 | 22 | 25 | 22 | +3 |
| 7 | 20 | 23 | 20 | +3 |

The C# repository has no production difficulty selector/field corresponding
to this ROM branch. These are exact conditional differences, not evidence
that a normal/easy arcade run has those higher totals.

### Pending release, active capacity, and completion

The ROM's `max_enm` table (`$9607_4b`, RAM `$011c`) is six throughout the
level range. `create_enemies` (`$98a2-$999c`) compares `enemies_in +
enemies_top` with that cap and advances pending visibility/delay entries.
Only after a pending entry successfully materializes does `$993c` decrement
`enemies_pending`. There is no C#-style 50-tick reserve interval or four-Egg
queue in this path.

The ROM has no original-ID completion ledger. `$a593-$a5af` waits for
`enemies_pending` and enemy-death work to clear and checks active enemy slots
near the rim before starting level completion. At death, `$a504-$a57b` adds
`enemies_in + enemies_top + enemies_pending`, caps the result at `$3f`, and
stores it back as generic pending supply.

### Type eligibility and concurrent limits

The level-ranged tables are reached through pointers at `$9617-$9655`.
`min_flippers`, `max_flippers`, `min_tankers`, `max_tankers`,
`min_spikers`, `max_spikers`, `min_pulsars`, `max_pulsars`,
`min_fuzzballs`, and `max_fuzzballs` are current-type minimum/maximum
selection limits, not predetermined per-wave counts. In `$99a5-$9a86`, every
ordinary type-selection route requires a nonzero type minimum. Thus the
zero Flipper minimum beginning at Level 5 prevents ordinary Flipper bottom
selection, even though the Flipper maximum remains nonzero for released
cargo.

| Level | Flipper min/max; ordinary bottom entry | Tanker min/max | Spiker min/max | Pulsar / Fuzzball |
|---:|---|---:|---:|---|
| 1 | 1 / 4; yes | 0 / 0 | 0 / 0 | 0 / 0 |
| 2 | 1 / 4; yes | 0 / 0 | 0 / 0 | 0 / 0 |
| 3 | 1 / 4; yes | 1 / 1 | 0 / 0 | 0 / 0 |
| 4 | 1 / 4; yes | 0 / 0 | 1 / 2 | 0 / 0 |
| 5 | 0 / 5; no | 1 / 1 | 2 / 3 | 0 / 0 |
| 6 | 0 / 5; no | 1 / 2 | 2 / 4 | 0 / 0 |
| 7 | 0 / 5; no | 1 / 2 | 2 / 4 | 0 / 0 |

These are concurrent selection limits/minima. The tables do not specify a
fixed total count by species over a full level. The selector also considers
active availability, remaining global capacity, outstanding minima, and
Spiker/Tanker spike-height choice.

### Tankers and cargo

The ROM `tanker_load` type tables (`$9607_43`, `$9607_47`) resolve to Flipper
type 1 over Levels 1-7; normal Tanker eligibility is governed by the limits
above. Selecting an ordinary Tanker consumes one successful generic pending
entry and therefore decrements `enemies_pending` once. The Tanker is one
active enemy slot while carrying its two latent Flippers. The picker reserves
two slots of the cargo type per active Tanker in its per-type availability
calculation (`$99c0-$99dc`); this is not another pending-supply decrement.

At the `$20` rim boundary, `$a06f-$a0f3` retires the Tanker slot and separately
attempts to allocate two Flipper child slots. Those children become active
enemy units but do not consume pending entries. If a Spiker reaches the
`$f2` branch with no generic pending entries, `$9fc6-$a027` can rewrite that
same slot into a Flipper-carrying Tanker at `$f0`; this does not add a new
pending/original unit. Level 4's normal Tanker table is 0/0, but that special
Spiker conversion is a separate path and should not be mistaken for normal
Tanker selection.

### Starting Spikes

`wave_spikeht` (`$9607_53`, RAM `$015a`) is copied by `$923a-$9243` to all
16 lane spike-height entries. Levels 1-3 are zero; Levels 4-7 use raw ROM
bytes `$e0`, `$d8`, `$d4`, and `$d0`. C# uses 16 height-60 Spikes for each
of Levels 4-7. The raw bytes are established by the listing, but this pass
did not establish a validated ROM-along-coordinate to C#-height conversion.
The equal C# heights versus different raw ROM bytes are therefore an
unresolved geometry/fidelity question, not a proven enemy-supply discrepancy.

## Level-by-level comparison

“Direct Flipper” and type caps below describe eligibility/current policy, not
a promised fixed species count. Tanker caps are concurrent, not per-wave.

### C# configuration/runtime matrix

| Level | C# original supply | Initial Eggs | C# reserve | C# direct Flipper eligibility | C# Tankers | C# Tanker cargo representation | C# Spikers | C# Pulsars | C# Fuseballs | C# starting Spikes |
|---:|---:|---:|---:|---|---|---|---|---|---|---|
| 1 | 10 | 4 | 6 | Flippers only | None | None | None | None | None | None |
| 2 | 12 | 4 | 8 | Flippers only | None | None | None | None | None | None |
| 3 | 15 | 4 | 11 | Yes; random original mix | Up to 6 active | Two owned cargo IDs per committed Tanker | None | None | None | None |
| 4 | 17 | 4 | 13 | Yes; random original mix | Up to 6 active | Two owned cargo IDs per committed Tanker | Yes; up to 4 active | None | None | 16 at height 60 |
| 5 | 20 | 4 | 16 | Yes; current selector permits it | Up to 6 active | Two owned cargo IDs per committed Tanker | Yes; up to 4 active | None | None | 16 at height 60 |
| 6 | 22 | 4 | 18 | Yes; current selector permits it | Up to 6 active | Two owned cargo IDs per committed Tanker | Yes; up to 4 active | None | None | 16 at height 60 |
| 7 | 20 | 4 | 16 | No; dedicated picker excludes ordinary bottom Flippers | Up to 2 active | Two owned cargo IDs per committed Tanker | Yes; up to 4 active | None | None | 16 at height 60 |

### ROM-derived comparison and proven differences

| Level | ROM wave count (base / hard) | ROM direct Flipper | ROM Tanker / cargo | ROM Spiker | ROM Pulsar | ROM Fuzzball | ROM starting Spike bytes | Exact C# difference / confidence |
|---:|---:|---|---|---|---|---|---|---|
| 1 | 10 / 11 | Yes; max 4 | Normal picker 0; no eligible Tanker | 0 | 0 | 0 | 00 | Base count matches. C# has no four-Flipper active type cap and no global six cap. **ROM-LISTING-BACKED / C# CODE** |
| 2 | 12 / 14 | Yes; max 4 | Normal picker 0 | 0 | 0 | 0 | 00 | Base count matches. Same active-cap difference as Level 1. **ROM-LISTING-BACKED / C# CODE** |
| 3 | 15 / 17 | Yes; max 4 | Max 1; Flipper cargo | 0 | 0 | 0 | 00 | Base count matches. C# allows up to 6 concurrent Tankers and has no four-Flipper/six-total cap. **ROM-LISTING-BACKED / C# CODE** |
| 4 | 17 / 19 | Yes; max 4 | Normal picker 0; separate no-pending Spiker conversion can yield a Tanker | 1-2 | 0 | 0 | e0 | Base count matches. C# selects Tankers directly and permits up to 4 Spikers; ROM normal Tanker max is 0 and Spiker max is 2. **ROM-LISTING-BACKED / C# CODE** |
| 5 | 20 / 23 | No ordinary bottom Flipper; max 5 for active cargo accounting | Max 1; Flipper cargo | 2-3 | 0 | 0 | d8 | Base count matches. C# permits direct bottom Flippers, up to 6 Tankers, and up to 4 Spikers; ROM permits no direct Flipper, max 1 Tanker, max 3 Spikers. **ROM-LISTING-BACKED / C# CODE** |
| 6 | 22 / 25 | No ordinary bottom Flipper; max 5 for active cargo accounting | Max 2; Flipper cargo | 2-4 | 0 | 0 | d4 | Base count matches. C# permits direct bottom Flippers and up to 6 Tankers; ROM permits neither direct Flippers nor more than 2 Tankers. Spiker max 4 matches. **ROM-LISTING-BACKED / C# CODE** |
| 7 | 20 / 23 | No ordinary bottom Flipper; max 5 for active cargo accounting | Max 2; Flipper cargo | 2-4 | 0 | 0 | d0 | Base count and the dedicated no-direct-Flipper, Tanker, Spiker, and six-selection-cap rules align in broad shape. C# uses a random post-minimum choice rather than the ROM's active-availability/spike-height/random-start selector. **ROM-LISTING-BACKED / CURRENT C# / AUTOMATED TESTS** |

For all rows, a Tanker that appears as a Spiker conversion is not an extra
pending unit. For Levels 3 and 5-7, normal Tanker caps are per-tube active
limits, and a Tanker can later be replaced by two materialized cargo enemies.
The ROM maximum Flipper number is not a direct-entry count for Levels 5-7:
the zero minimum gates bottom selection, while Flipper cargo is still
materialized and counted as active species.

## Reconciliation of prior Level 3 and Level 7 findings

### Level 3

- **PREVIOUSLY ACCEPTED PROJECT BEHAVIOR:** The production-shaped C# Level 3
  has 15 original IDs; a Tanker owns two distinct cargo IDs; releases,
  reconstruction, and completion conserve those identities. Existing
  `Level3MixedWaveTests` and `Level3WaveLedgerTests` protect the model.
- **ROM-LISTING-BACKED:** The ROM base pending count is 15. It allows active
  Flippers up to four and Tankers up to one, with no ordinary Spikers,
  Pulsars, or Fuzzballs at Level 3. Cargo Flippers are not separate pending
  units while aboard.
- **RECONCILIATION:** The C# 15-original value remains correct at base
  difficulty, but the C# six-Tanker concurrent cap, absent Flipper cap, and
  no global six-active cap differ from the ROM. The C# random source-shaped
  composition is not an Atari fixed distribution.
- **DEATH / COMPLETION:** C# deliberately preserves selected species and
  exact unresolved original/cargo IDs on Levels 3-6. ROM death reconstruction
  preserves only generic unresolved quantity, not species or cargo linkage.
  These are not contradictory claims: one is accepted C# project policy, the
  other is ROM behavior. Existing Level 3 graphical acceptance was for visible
  C# behavior, not hidden Atari identity semantics.

### Level 7

- **PREVIOUSLY ACCEPTED / ROM-BACKED:** The local table establishes base 20,
  Flipper min/max 0/5, Tanker 1/2, Spiker 2/4, and the six-active cap. The
  ROM picker excludes ordinary bottom Flippers. Tankers carry Flippers and
  release them as separate active children. Death aggregates active-below,
  at-rim, and pending units into generic pending quantity. A final-pending
  Spiker may transform in place to a Flipper-carrying Tanker.
- **CURRENT C# / AUTOMATED TEST EVIDENCE:** Level 7 now uses its dedicated
  selector, clears unresolved species on death, converts released cargo
  records to generic re-entry supply, and tests the one-unit Spiker and
  final-Spiker-to-Tanker paths. Its height-zero conversion seam remains an
  explicit project approximation of the ROM along-coordinate branch. Neal's
  accepted `2-3-2-2-2` visible movement route is separate from enemy supply.
- **HISTORICAL REPORT CONFLICT:** The “Current C# comparison” in
  `2026-11-04-level7-reentry-rom-investigation-report.md` describes the
  pre-correction C# selector (which could choose Flippers and retained
  species). The later Level 7 arcade-corrections report, current source,
  durable behavior spec, and current tests supersede that comparison.
- **DOCUMENTATION QUALIFICATION:** Durable Level 7 notes call 20 the
  ROM-backed wave. This is the table/base count; it should not be interpreted
  as invariant across ROM difficulty, because the local hard branch yields
  23. The prior re-entry report already states that distinction.
- **ARCADE OBSERVATION:** Neal's no-ordinary-bottom-Flipper and Tanker/cargo
  observations remain explicitly visible-behavior evidence. They do not
  establish hidden per-wave counts or the arcade's internal identity model.

## Total count versus observed wave presentation

The most defensible explanation is a combination, not a blanket claim that
all configured totals are wrong:

1. **Base total:** C# and ROM table counts match at Levels 1-7.
2. **Difficulty:** A hard ROM run has the higher counts in the matrix; C# has
   no corresponding production setting.
3. **Concurrent enemies:** The ROM limits ordinary creation to six active
   slots and applies level-specific active species maxima. C# Levels 1-6 do
   not have the same total/type caps. This can make the C# tube look more
   crowded, even with an equal base supply.
4. **Pacing:** C# 50-tick Egg releases, up-to-six ledger releases, and two
   approaching Eggs are not the ROM's pending-entry delay pool. Arrival
   cadence and visible clustering can differ independently of total supply.
5. **Species mix:** Direct Flippers on C# Levels 5-6 and the C# Tanker/Spiker
   caps on Levels 3-6 can change what appears and how many carriers are
   visible. The exact realized mix is randomized; no single fixed C# count
   can be asserted for those types.
6. **Cargo:** Comparing `EnemySupply`/`ORIGINAL` only with every visible
   sprite omits cargo. The C# ledger's separate `CARGO` count is the right
   companion value. ROM pending supply likewise does not include latent
   Tanker cargo, although released children subsequently occupy active slots.
7. **Re-entry:** Levels 3-6 preserve C# species/identity where ROM rebuilds a
   generic pool. Level 7 has a dedicated C# approximation of generic ROM
   reconstruction. This can change post-death type sequences.
8. **Starting Spikes:** The C# and ROM counts agree for Levels 1-7 (none on
   1-3; one per lane on 4-7), but their numeric height representations have
   not been proven equivalent.

No evidence gathered here identifies which of these mechanisms caused
Neal's particular observation. No user recording or arcade footage was
re-counted.

## Findings to preserve

- Do not change the matching base supply totals without evidence that the
  comparison was made on ROM hard difficulty or a different table progression.
- Preserve the accepted Level 3 identity/cargo model; don't rewrite it to
  claim ROM identity semantics.
- Preserve Level 7's dedicated picker, generic re-entry behavior, same-unit
  conversion, and separate arcade-observed movement route.
- Do not count Spikes, bullets, or latent cargo as additional ROM pending
  wave units.
- Do not assume one visible enemy entering equals one new original identity;
  Tanker cargo and Spiker conversion break that equivalence.

## Ranked future work (recommendation only; no implementation)

1. **First, classify the comparison:** establish the arcade difficulty setting
   and whether Neal is comparing the HUD's remaining-original value, active
   sprites, bottom entrants, or Tanker plus released cargo. This has no code
   risk and decides whether the base count or hard-mode difference applies.
2. **Smallest source-proven picker milestone:** if Neal wants a correction,
   make a Level-5-only policy/test change that prevents ordinary bottom
   Flippers but continues to permit Tanker cargo Flippers. Keep the 20 base
   original count, Egg cadence, cargo ownership, death policy, and completion
   unchanged. Level 5 is the first level whose ROM Flipper minimum becomes
   zero; review the visible change manually before applying the same rule to
   Level 6. This is a behavior change and should not be made without explicit
   implementation authorization and arcade acceptance.
3. **Then reconcile active caps/type limits one level at a time:** start with
   Level 3's six-versus-one Tanker cap, then Level 4-6 limits and the ROM
   six-active gate. Keep release pacing separate unless evidence shows it
   cannot be separated.
4. **Handle hard-mode totals separately:** add a C# difficulty concept only
   if the project intends to compare multiple ROM difficulty settings; test
   the exact shift/carry result, not a rounded floating-point `count / 8`.
5. **Investigate Spike-height mapping separately** from supply and wave
   scheduling.

Suggested automated acceptance for a future Level 5 picker correction:

- deterministic tests show no ordinary bottom Flipper is selected while the
  two Tanker cargo Flippers can still materialize;
- original supply remains 20 and the Tanker parent still consumes one
  original identity while its pair remains in the cargo namespace;
- repeated death/re-entry preserves unresolved-set conservation and
  completion still waits for both original and cargo obligations;
- Levels 1-4 and Level 7 selection behavior stays unchanged;
- no active-cap or release-cadence changes are bundled into that correction.

Suggested manual arcade checks: at Level 5, count bottom Flipper entrants
separately from Tankers and rim-released cargo; record total unresolved
entrants and simultaneous active enemies; repeat after one death; compare
normal and hard cabinet settings if available. Only after automated tests
pass should Neal launch the C# campaign using
`dotnet run --project src\Tempest.Game -- --lives 10` and compare the same
events. VS Code Copilot did not launch the game.

## Validation and final Git state

- No tests were run: this was read-only analysis, and tests were inspected as
  evidence rather than used as a ritual validation step.
- The attached prompt copy was SHA-256 compared with the read-only attachment;
  the hashes matched.
- Final verification: HEAD remained
  `d2d8b1ef69b4a13be06ab59b41a1c9d38a8dd6e2`; no staged or tracked changes
  were created. The original untracked `screenshot000.png` through
  `screenshot013.png` remained untouched. The two requested AI log records
  remain ignored by Git.
