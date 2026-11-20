# Level 5 concurrency and ordinary admission: ROM investigation

Date: 2026-11-09

Disposition: **READ-ONLY INVESTIGATION COMPLETE. A LEVEL-5-SPECIFIC
ADMISSION-AND-LIFECYCLE DIAGNOSTIC IS RECOMMENDED; NO IMPLEMENTATION
AUTHORIZED OR PERFORMED.**

## 1. Scope, baseline, and evidence

The complete substantive task, including its single standalone completion
token, is preserved in the
[matching prompt record](../Prompts/2026-11-09-level5-concurrency-rom-investigation-prompt.md).

The worktree began at the requested accepted baseline,
`62c66e0ab19436ea162935627021154be21e0a41`, on branch `master`. The index and
tracked worktree were clean. Investigation was read-only; only the ignored
prompt and this ignored report were written. Production, tests, accepted
gameplay documentation, and reference material were not changed. No game,
build, or test suite was run.

Evidence classifications:

- **ROM-BACKED:** supported by instructions/data in the local Rev. 2
  [ROM-derived listing](../../Reference/arcade/Tempest%20Rom%20Source%20code.md).
  This report does not claim a new running-cabinet observation or independent
  binary verification of each Level 5 table byte.
- **REFERENCE-DERIVED:** Incitti material; not Atari authority.
- **PROJECT-CURRENT:** current C# source or documented project acceptance.
- **ARCADE-OBSERVED:** only prior direct arcade observations recorded by Neal.
- **PROJECT TRANSLATION / PROJECT DECISION:** intentional engineering behavior,
  not a claim of literal ROM behavior.
- **INFERENCE:** consequences of the ROM data and routines, not a measured
  gameplay frequency or fixed species recipe.
- **UNKNOWN:** not established by this investigation.

The investigation reuses the generic allocator, shared physical-slot,
pending-entry, cargo, removal, and zap findings in the
[Level 3 report](2026-11-09-level3-concurrency-rom-investigation-report.md)
and [Level 4 report](2026-11-09-level4-concurrency-rom-investigation-report.md).
Those reports' C# snapshots predate this baseline; current C# conclusions
below were checked against this checkout. Level 5's table values and Spiker
path were separately traced rather than inferred from either prior level.

## 2. Executive findings

1. **ROM-BACKED:** Displayed Level 5 has 20 generic pending enemy identities
   on Easy/Medium and 23 on Hard. The ROM does not preassign these identities
   a final species mixture.
2. **ROM-BACKED:** Ordinary bottom entries can be Tankers and Spikers, but not
   Flippers. Their Level 5 min/max values are Flipper **0/5**, Tanker **1/1**,
   Spiker **2/3**, Pulsar **0/0**, Fuseball **0/0**. Minima are selection
   priorities, not a predetermined wave recipe.
3. **ROM-BACKED:** The physical enemy array has seven places (indices 0-6).
   Species maxima do not replace that shared capacity. A Tanker occupies one
   place; its two latent Flippers occupy none until release.
4. **ROM-BACKED:** The Spiker grow/retreat action is shared, not a Level 4-only
   routine. When `enemies_pending` reaches zero, a retreating Spiker changes
   in its existing place into a Flipper-carrying Tanker. This bypasses ordinary
   Tanker admission and can take live Tankers above their ordinary maximum.
5. **INFERENCE:** In ordinary selection, the zero Flipper minimum excludes
   Flippers even when their maximum-derived availability is positive. Once
   Tanker and Spiker availability are exhausted, the selector can leave up to
   three of the seven places unused. Cargo release and the Spiker conversion
   can subsequently change live composition without another ordinary choice.
6. **PROJECT-CURRENT:** Level 5 correctly disables ordinary Flipper selection
   and uses the shared identity/cargo ledger, but still uses the Incitti-shaped
   mixed selector, a six-Tanker guard, a four-Spiker guard, and the `< 9`
   approach gate. It has no seven-place hatch limit.
7. **PROJECT-CURRENT / mismatch:** Level 5 Spikers return to reserve and can
   later be selected again. The C# terminal Spiker-to-Tanker conversion is
   enabled only for the Level 4 diagnostic or Levels 7/8's generic ROM policy;
   it is not enabled for Level 5. Thus a complete concurrency correction
   cannot be admission-only: it must include the Level 5 Spiker lifecycle
   consequence, while preserving the accepted identity, cargo, Spike, and
   gameplay systems.

## 3. Level 5 ROM parameters

The loader uses the displayed level (`curlevel + 1`) to read the per-level
tables at `$9607`. The fifth displayed-level entries produce:

| Parameter | Displayed Level 5 | ROM source / interpretation |
|---|---:|---|
| Generic pending supply | 20 Easy/Medium; 23 Hard | `wave_enemies`, `$9607_4f` / `$95b3`; Hard adds `floor(20/8) + bit2(20) = 3` |
| Ordinary Flipper minimum / maximum | 0 / 5 | `$9607_13` / `$9607_17`; zero minimum prevents ordinary Flipper selection |
| Ordinary Tanker minimum / maximum | 1 / 1 | `$9607_23` / `$9607_27` |
| Ordinary Spiker minimum / maximum | 2 / 3 | `$9607_2b` / `$9607_2f` |
| Pulsar minimum / maximum | 0 / 0 | Pulsar ranges begin at displayed Level 17 |
| Fuseball minimum / maximum | 0 / 0 | Fuseball ranges begin at displayed Level 11 |
| Tanker cargo | 2 Flippers | `tanker_load` value 1 denotes the two-Flipper load; the first two load bytes are initialized to 1 and the Level 5 table values are also 1 |
| Physical enemy capacity | 7 | `max_enm=6` at `$9607_4b`; it is the highest index, not a count |
| Enemy-shot capacity | 4 shots at Medium | `enm_shotmax=3` at `$9607_0f`, also a highest index; Easy/Hard adjust this separately |
| Initial Spike parameter | `$d8` | `wave_spikeht` at `$9607_53`; ROM coordinate, not directly C# world height |
| Flipper movement program | `$53` | fifth `flipper_move` value at `$9607_5b`; movement, not an admission cap |

The species indices used by the picker are Flipper 0, Pulsar 1, Tanker 2,
Spiker 3, and Fuseball 4. The table-derived supply counts generic pending
originals, not the later cargo units of a Tanker. The ROM's enemy-shot array
is separate from its seven enemy places.

## 4. ROM ordinary admission

### Shared allocator and selector

**ROM-BACKED:** The common `create_enemies` routine at `$98a2` gates ordinary
entry on available physical capacity and `zap_running`; pending entries are
then progressed independently. At `$9923-$994c`, a successful ordinary
creation decrements `enemies_pending`. `$994d-$99a4` searches the seven
physical slots and increments `enemies_in` and the selected species counter.
Rim transfer moves the enemy between `enemies_in` and `enemies_top`; it does
not free the enemy place.

At `$99a5-$99ee`, the ROM computes each type's max-minus-current availability,
subtracts two Flipper-availability units per live Tanker with cargo, then
clamps each type's availability to shared physical headroom. The cargo
subtraction is byte arithmetic and can wrap. The picker then counts nonzero
availability and applies minima before its fallback selection:

- With multiple types available, unsatisfied minima are checked in descending
  type-index order. Thus an unmet Spiker minimum is considered before an
  unmet Tanker minimum; Flipper minimum is zero.
- If only one type has availability, the selector still requires that type's
  minimum to be nonzero.
- Once minima are met, if both Tanker and Spiker remain available, the ROM
  selects between them using the shortest-Spike coordinate threshold `$cc`
  (`<$cc` chooses Tanker; otherwise Spiker).
- The fallback randomized scan only attempts types with nonzero minimum.
  Consequently, Level 5's Flipper availability can be positive without ever
  making Flipper an ordinary selection.

The dynamic picker does not yield a fixed final species mix. Tanker/Spiker
choices depend on the evolving counts, shared space, minimum state, random
scan, and shortest-Spike condition.

### Bounded representative states

These are direct small calculations from the table and picker, not a
simulation:

| Live ordinary F/T/S | Physical places free | Relevant availability / result |
|---|---:|---|
| 0/0/0 | 7 | F=5, T=1, S=3; unmet Spiker minimum is selected first |
| 0/0/2 | 5 | Spiker minimum met; unmet Tanker minimum selects the Tanker |
| 0/1/2 | 4 | T max reached; one Spiker place remains, so Spiker can be selected |
| 0/1/3 | 3 | F availability remains positive (and is cargo-adjusted), but T/S are at their maxima and F min is zero; no ordinary species is selected |

Absent cargo releases or transformations, ordinary bottom admissions can
therefore have at most one Tanker and three Spikers concurrently. This is not
a four-live-object wave cap: a Tanker can release two live Flippers while
pending supply remains, and those children bypass ordinary selection.
The physical array remains the hard seven-object bound. Cargo may raise live
Flippers above five, and the Spiker conversion may raise live Tankers above
one. Species maxima limit ordinary admission availability; they do not
enforce all later live populations.

At the 0/1/3 state, one live Tanker reserves two Flipper-availability units.
The vector may still have Flipper availability after physical clamping, but
the zero Flipper minimum prevents the selector from using it. This is a
species-selection dead end with physical space unused, not proof that all
availability bytes are zero.

### Cargo, pending supply, and live-count effects

Latent Tanker cargo is not an enemy object or physical slot. Ordinary
availability calculation subtracts its two Flipper units. When a Tanker
reaches its release path, the carrier is removed and cargo allocation tries
the two Flippers separately; each allocated child occupies a physical place.
Two children replacing one carrier can increase live occupancy by one. If
only one physical place is available after carrier removal, only one child
can be allocated. Cargo does not decrement generic pending supply or run
ordinary species selection.

The Level 5 Flipper maximum of five is therefore not a late-wave active cap.
Cargo bypasses it. A Spiker-derived Tanker can also exceed the ordinary
Tanker maximum as described below. The seven-slot allocation is the physical
limit; enemy shots and persistent Spike hazards are not enemy places.

## 5. Level 5 lifecycle and occupancy

### Spiker grow/retreat/hop/conversion

**ROM-BACKED:** The enemy movement interpreter dispatches Spiker action `$0e`
to `eact_0e` (`$9fc4`; jump-table entry `$9bb0`). This is shared Spiker
machinery, not guarded by a Level 4 number. Level 5's Spiker minimum/maximum
and starting-Spike table value make this path relevant. The routine grows a
Spike, reverses/retreats, and hops the same live enemy to a shortest-Spike
lane. It does not release the physical enemy slot during growth, retreat, or
hop.

- If `enemies_pending != 0`, the Spiker keeps the same slot and continues its
  grow/retreat/hop lifecycle. It is not a new pending identity and does not
  ordinary-reenter.
- If `enemies_pending == 0`, `$a00e-$a01f` changes that same enemy's type and
  cargo flags to a Flipper-carrying Tanker. No new physical allocation,
  ordinary Tanker admission, or pending decrement occurs.
- The conversion code does not update `n_spikers` or `n_tankers`; generic
  species counters may cease to reflect the visible converted species. The
  allocator's occupied slots still control actual physical availability.

**INFERENCE:** Level 5 can have converted Tankers beyond the ordinary Tanker
maximum. Up to the admitted Spikers already present can become carriers after
the last pending entry; those transitions retain, rather than free, their
slots. Their later cargo release can further increase live Flipper count,
subject to physical allocation. Do not copy Level 4's policy unchanged:
Level 5 additionally admits ordinary Tankers and has different species
minima/maxima, even though it shares the generic terminal Spiker action.

### What occupies/frees a physical place

| Entity or state | ROM treatment |
|---|---|
| Live Flipper, Tanker, Spiker, including rim / transition / retreat states | One occupied shared enemy place |
| Spiker hop or conversion to Tanker | Same occupied place |
| Latent Tanker cargo | No place until a child is allocated |
| Released cargo Flipper | One place per successfully allocated child |
| Pending original enemy | Not active until it is allocated into a physical slot |
| Pending Super Zapper target | Remains an enemy until the ROM removes it |
| Enemy projectile / persistent Spike | Separate from enemy-place occupancy |
| Shot/destruction or completed zap removal | Frees the removed enemy's place |
| Tanker shot/natural cargo release | Carrier is removed; released children are separately allocated |
| Tanker removed by Super Zapper | Cargo flags are cleared before removal; no live cargo split |
| Death / reconstruction | Active enemies are reset into generic pending supply; this is not an in-wave capacity release rule |

**ROM-BACKED:** Rim enemies remain counted (`enemies_top` rather than
`enemies_in`). A visual transition, rim transfer, pending zap timer, or
retreat is not by itself a freed place.

## 6. Super Zapper interaction

**ROM-BACKED:** The shared creation routine sets its entry gate when
`zap_running` is nonzero. Level 5 uses this same routine, so ordinary pending
entries are paused during the zap sequence. The seven-place capacity and zap
pause are independent conditions.

**PROJECT-CURRENT:** Level 5 ordinary Egg approach/hatching has no
`SuperZapper`-active admission check. Pending-zap C# enemies remain in their
enemy lists until their timer expires, but the normal Level 5 Egg path still
updates during a zap. This is a generic ROM-versus-C# admission mismatch
already identified in prior investigations, not a Level 5 redesign request.
No change to the accepted Zapper target, timing, scoring, or Tanker cargo
resolution is recommended as part of this investigation.

## 7. Current production C# Level 5

Inspected the canonical
[Level 5 configuration](../../src/Tempest.Game/Levels/LevelConfiguration.cs),
[Level update and Egg admission](../../src/Tempest.Game/Levels/Level.cs),
[gameplay selection and lifecycle](../../src/Tempest.Game/TempestGame.cs),
[legacy mixed selector](../../src/Tempest.Game/Levels/Level3WaveSelection.cs),
[identity/cargo ledger](../../src/Tempest.Game/Levels/Level3WaveLedger.cs),
[Spiker](../../src/Tempest.Game/Gameplay/Spiker.cs), and
[Tanker](../../src/Tempest.Game/Gameplay/Tanker.cs).

### Configuration and admission

- **PROJECT-CURRENT:** `Level5Production` has 20 original ledger identities,
  four initial Eggs, a 50-tick reserve interval, `MaximumActiveEggs=4`,
  `MaximumApproachingEggs=2`, 50-tick `HatchSelectionIntervalTicks`,
  Flippers/Tankers/Spikers, and ordinary Flipper selection disabled.
  It has no Pulsars or Fuseballs.
- The production-ledger Egg release path releases up to six reserve identities
  every 50 active ticks; its batch path does not apply the four-Egg cap.
  `UpdateLevel3MixedEggs` instead starts approaches while active enemy units
  are `< 9` and fewer than two Eggs are approaching, ordinarily using a
  per-update random roll (`> 95`) and bypassing that roll near the end. The
  configured 50-tick hatch interval is not the production-ledger approach
  gate. These are current C# scheduling rules, not Atari timing.
- First hatch selection for Level 5 routes to
  `SelectLevel4OriginalType` because it has the shared three-species
  composition. The helper is an Incitti-shaped random candidate picker:
  Flipper candidates are disabled by the Level 5 config, Tanker selection
  uses the general `activeTankers < 6` candidate bound, and Spikers are
  suppressed near the end or when the current Spiker count is above three.
  This is not the ROM's minimum-priority, maximum-availability, shortest-Spike
  selector.
- The accepted `Level3MixedAdmissionPolicy` and `Level4AdmissionPolicy` are
  not selected for `Level5Production`: their runtime branches are restricted
  to the Level 3 mixed and Level 4 diagnostic configurations. Level 5 remains
  isolated from those accepted policies, although it shares the older
  three-species helper named `SelectLevel4OriginalType`.
- The hatch path's Spiker guard is `spikers.Count > 3`, so it permits a fourth
  active Spiker before rejecting the next one. Its Tanker guard permits up
  to six outside the dedicated Level 3/4 diagnostics. There is no Level 5
  Flipper cap on released cargo and no seven-active-enemy hatch ceiling.
  `< 9` controls approach, not the number of enemies that can hatch or be
  released.
- C# Tankers carry two latent Flipper identities in the ledger. Latent cargo
  is not in the active enemy count and is not included in Level 5 ordinary
  selection availability. Released children join the live Flipper population;
  there is no seven-place release gate.

### Runtime population, lifecycle, identity, and completion

- `CountProductionActiveEnemyUnits` counts Flippers, Tankers, Spikers, and
  shot-released cargo awaiting activation. Eggs are handled separately.
  Rim/transition states remain represented by their enemy objects. A pending
  zap target remains in the corresponding list until actual timer expiry.
  Enemy bullets and persistent Spikes are separate.
- **PROJECT-CURRENT mismatch:** On Level 5 Spiker retreat, the production
  ledger returns its original identity to reserve and clears its selected
  species. `UsesGenericRomDeathReconstruction` is true only for the Level 7
  and Level 8 ROM policies, and the special terminal conversion check is
  Level 4-diagnostic-only. Therefore Level 5 does not convert an end-of-wave
  Spiker into a Tanker; the returned identity may later hatch and be selected
  again. The existing reusable `ConvertActiveSpikerToTanker` ledger operation
  is not enabled for this configuration.
- Death/reconstruction clears runtime enemies and rebuilds unresolved ledger
  identities; resolved outcomes remain resolved. This identity/accounting
  translation is accepted project behavior, not a claim of the ROM's exact
  species reconstruction.
- Completion requires unresolved original and cargo identities, reserves,
  Eggs, and active enemies to be resolved. All-rim completion has distinct
  identity/eligibility obligations. A later policy must not turn a wait for
  physical headroom into a kill, resolution, reserve loss, or completion.
- Level 5 has four C# active enemy-shot places by its level configuration,
  matching the Medium ROM value when counted as a capacity rather than the
  table's highest-index encoding.

## 8. ROM versus C# and preservation requirements

| Subject | ROM-backed Level 5 | Current production C# | Assessment |
|---|---|---|---|
| Medium/Easy supply | 20 generic pending identities | 20 stable original ledger identities | Count agrees; pending/Egg representation and cadence differ |
| Hard supply | 23 | No Level 5 difficulty-specific supply variant | Confirmed supply-mode difference |
| Ordinary species | Tanker and Spiker; Flipper min 0 blocks ordinary selection | Flipper excluded; Tanker/Spiker random selector | Eligibility agrees in broad terms; selection differs |
| Species limits | F 0/5, T 1/1, S 2/3 | No matching Level 5 maxima/minima; generic T cap 6, S guard permits 4 | Confirmed mismatch |
| Minima / priority | Spiker minimum 2, then Tanker minimum 1; dynamic choices | No ROM minimum priority | Confirmed mismatch |
| Physical capacity | Seven shared places | No seven-place hatch or release gate; `< 9` is an approach condition | Confirmed mismatch |
| Latent cargo | Reduces Flipper availability by 2 per carrier | Cargo tracked for identity/completion, omitted from Level 5 selection headroom | Confirmed admission mismatch |
| Spiker ending | Same slot converts to two-Flipper Tanker at no-pending condition | Returns to reserve; may hatch again | Confirmed lifecycle mismatch |
| Released cargo | Bypasses selection; each child needs a physical place | Complete C# pair is released without ROM physical cap | Counting path differs at saturation |
| Super Zapper | Pauses ordinary entry | Egg approach/hatch continues during zap | Generic mismatch, separate from this milestone |
| Shots / spikes | Separate arrays/hazards; not enemy places | Separate C# bullets and persistent Spike objects | Agreement in capacity classification |
| Identity / death / completion | Generic ROM reconstruction is not species-stable | Stable ledger and completion obligations | Accepted project translation; preserve |

Accepted Level 5 systems that a later concurrency task must preserve are
documented in
[the behavior specification](../../TEMPEST-BEHAVIOR-SPEC.md) and
[development notes](../../TEMPEST-DEVELOPMENT-NOTES.md): cross geometry and
Level 4-to-5/5-to-next-level progression; initial Spikes; Flipper movement;
Tanker cargo routing, firing, and Zapper behavior; Spiker/Spike behavior;
shared shots and enemy bullets; rim/all-rim rules; death/reentry and stable
ledger identities; scoring; fresh Super Zapper state; and deterministic
recording/playback. Neal manually accepted visible Level 5 play and normal
completion, including Tankers, cargo, Spikers, starting Spikes, and Zapper.
That acceptance does not establish hidden ROM concurrency or endorse the
current admission limits as Atari behavior.

## 9. Applicability, uncertainties, and recommended next milestone

- **Can Level 5 reuse Level 3/4 engineering infrastructure?** Yes: shared
  identity/cargo bookkeeping, an admission snapshot, validation, and physical
  headroom calculations can be reused. The ROM rules must be Level-5-specific.
- **Which policy parameters must be Level-5-specific?** Supply by difficulty;
  F/T/S minima and maxima; zero-minimum Flipper exclusion; Tanker cargo
  reservation; dynamic Tanker/Spiker priority/selection; seven physical
  places; and the terminal Spiker conversion condition.
- **Would either previous level's policy work unchanged?** No. Level 3's
  Flipper/Tanker rules and one-Tanker limit omit Spikers; Level 4's policy
  has different Flipper/Spiker limits and no ordinary Tanker admission.
- **Is new lifecycle handling required?** Yes. End-of-pending Spiker-to-Tanker
  conversion is live Level 5 behavior and changes Tanker/cargo populations
  after ordinary admission.
- **Admission-only or coupled correction?** A complete Level 5 concurrency
  correction needs admission and the terminal Spiker lifecycle together.
  Admission-only limits would miscount late-wave carriers and their cargo.

**Recommended smallest next milestone:** Implement an isolated, Level-5-
specific admission-and-lifecycle diagnostic using the existing ledger and
conversion infrastructure. Do not silently switch accepted production
behavior or reuse Level 3/4 constants. Protect existing Level 5 acceptance,
recording determinism, identity conservation, zap handling, and completion;
include bounded deterministic cases for minima priority, unavailable Flipper
selection with open physical places, tanker cargo headroom/release, seven-slot
saturation, and no-pending Spiker conversion. After automated validation,
Neal should compare arcade behavior for initial ordinary composition,
concurrency/saturation, tanker/Spiker counts, retreat/hop, late Spiker
conversion, cargo release under low headroom, and zap-time entry pause.

**Unknown / limitation:** The listing describes generic program flow and
Level 5 table values, but there was no new cabinet/MAME run to measure
selection frequencies, observed population maxima, timing, difficulty
settings, or full-slot cargo outcomes. The dynamic selector does not imply a
fixed final species mixture. The C# manual acceptance is evidence for visible
recreation behavior only.

At closeout the repository remains at
`62c66e0ab19436ea162935627021154be21e0a41`; tracked files, tests, and accepted
documentation are unchanged. Only the ignored prompt and report records were
created. No Level 5 correction, commit, push, game launch, or Level 6
concurrency work was performed.
