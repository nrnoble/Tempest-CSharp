# Level 3 concurrency: ROM investigation

Date: 2026-11-09

Disposition: **READ-ONLY INVESTIGATION COMPLETE. CHANGE RECOMMENDED, NOT IMPLEMENTED.**

## 1. Scope, starting state, and evidence classification

The entire substantive prompt was read before repository action; exactly one
standalone completion token was present. The prompt is preserved in
[the matching prompt record](../Prompts/2026-11-09-level3-concurrency-rom-investigation-prompt.md).

**PROJECT-CURRENT:** Starting branch `master`, tracking `origin/master`.
HEAD and local `origin/master` were both
`7acac60035383bfb6e0cb170c0adfa0dd88b6c4b`.
The index and tracked worktree were clean. The only nonignored untracked material
was the existing 21 chip files under `Reference/arcade/roms.rev2/`.
No network lookup, game execution, build, test execution, generated assembly,
commit, push, or tracked edit was needed.

Classification used below:

- **ROM-BACKED:** Instructions/data in the local listing; important table and
  routine bytes were also checked against local Rev. 2 chip files. This is
  static ROM evidence, not a new running-cabinet observation.
- **PROJECT-CURRENT:** Current C# source, test contracts, or documented project
  acceptance. Reading a test is not a claim that it was executed.
- **ARCADE-OBSERVED:** Previously documented direct observations, only for the
  particular visible behavior observed.
- **REFERENCE-DERIVED:** Incitti evidence, not Atari authority.
- **INFERENCE:** Consequences or proposed project translations derived from
  the evidence, not newly measured arcade behavior.
- **UNKNOWN:** Not established by this investigation.

This report is a historical investigation record, not a promotion of new
accepted behavior into the behavior specification.

## 2. Executive findings

1. **ROM-BACKED:** Medium/default Level 3 starts with 15 generic pending
   entries, selected into Flippers or Tankers at entry time. There is no fixed
   preassigned Flipper/Tanker wave mixture. Tankers carry two Flippers.
   Pulsars, Spikers, and Fuseballs are not Level 3 entry types or cargo.
2. **ROM-BACKED:** Level 3 combines a seven-slot physical enemy array
   (`max_enm = 6`, inclusive indices 0..6), species availability, minimum-based
   type selection, pending-entry progression, and a Super Zapper entry gate.
   Ordinary entry allows at most **one active Tanker**, not six.
3. **ROM-BACKED:** The Flipper table maximum is four, but **four is not an
   unconditional Level 3 active-Flipper ceiling**. Tanker cargo availability
   is subtracted using wrapping byte decrements. A carrier can be admitted
   when only one Flipper availability remains; the resulting underflow can
   re-enable Flipper selection above four, bounded by the physical slots.
4. **PROJECT-CURRENT:** C# uses six simultaneous Tankers and an Incitti-shaped
   `< 9` gate on beginning Egg approach, not a hard nine-enemy hatch ceiling.
   It has no Level 3 Flipper species gate or seven-slot hatch gate and does
   not reserve latent cargo in its approach population count.
5. **INFERENCE:** Change is warranted, but copying the Level 1/2 four-Flipper
   hatch guard would be incorrect. Use a Level-3-specific mixed admission
   policy. Preserve the accepted ledger and cargo semantics; explicitly
   resolve any strict physical-slot versus complete cargo-pair conflict
   before claiming literal ROM fidelity.

## 3. Evidence map and independently checked ROM bytes

Primary source: [local ROM-derived listing](../../Reference/arcade/Tempest%20Rom%20Source%20code.md).
The following listing line anchors identify the inspected areas:

| Area | Listing lines | Addresses / symbols |
|---|---:|---|
| Counters and vectors | 137-138, 161-203, 220-221, 302-330 | `enemies_in=$0108`, `enemies_top=$0109`, `max_enm=$011c`; min/max/available/type arrays; pending tables; seven-element enemy vectors |
| Pending initialization and clear | 2471-2516 | `$9234-$928e`, `set_enm_and_spikes`, `$9246` |
| Parameter loading and difficulty | 2549-2673 | `$92c5-$93df`, effective displayed level is `curlevel+1`; loader `$92d6`; difficulty `$9328-$937f`; initial cargo entries `$93d7-$93dc` |
| Level tables and parameter pointers | 2732-2903 | `$9475-$965d`, detailed values below |
| Loader table operations | 2916-2995 | `$9677-$96ff`: constant, per-level, and block-of-16 lookups |
| Active update order | 3016-3025 | `state_04`, creation `$9714`, movement `$9717`, shooting `$971d`, collisions `$9720` |
| Pending entry progression | 3217-3288 | `create_enemies=$98a2`, `$9923-$994c` |
| Physical slot allocation | 3293-3332 | `$994d-$99a4` |
| Species availability and selection | 3335-3525 | `$99a5-$9afc` |
| Movement occupancy and rim transfer | 3553-3580, 3709-3741, 3779-3826 | `$9b1e`, `$9c57`, `0c_at_top=$9d06` |
| Removal and cargo split | 4180-4239 | `$a06f-$a0f6` |
| Firing and shot counters | 4542-4590 | `enm_shoot=$a2a6`, separate `enm_shotcnt` and shot vectors |
| Shot / zap removal callers | 4591-4610, 4651-4674 | `$a309-$a32c`, `$a38e-$a3b6` |
| Death / all-rim end handling | 4836-4930 | `$a504-$a5b2` |
| Super Zapper spawn exclusion / removal | 5233-5289 | `check_zap=$a839`, `$a888-$a8ac` |

**ROM-BACKED:** Direct, read-only byte reads confirmed:

- `136002-113.d1` mapped at `$9000`: Flipper min/max
  `$94cd/$94d6`, Tanker min/max `$94eb/$9503`, Pulsar tables
  `$9520/$9529`, Spiker tables `$9489/$94a5`, Fuseball tables
  `$9541/$954e`, cargo tables `$9475/$9481`, global `$9598`, supply `$95b3`.
- `136002-114.e1` mapped at `$9800`: creation gate `$98a2-$98d8`,
  slot lookup `$994d-$9956`, availability arithmetic `$99a5-$9a25`,
  and Tanker cargo-selection condition `$9abb-$9af0`.
- `136002-115.f1` mapped at `$a000`: removal/split `$a06f-$a0f6`.
- `136002-116.h1` mapped at `$a800`: `$a8a4-$a8ae` is
  `B9 8A 02 29 FC 99 8A 02 4C 98 A3`, clearing cargo flags before zap removal.

In particular, `$99d6/$99d9` really are two `DEC $013c,X`
instructions, not saturating subtraction. `$9ae1` tests only for zero,
not whether two cargo places are available. These details are not speculative
interpretations of the listing comments.

## 4. Level 3 composition and parameters

**ROM-BACKED:** The loader obtains these values for effective displayed Level 3:

| Parameter | Value | Table / meaning |
|---|---:|---|
| `wave_enemies` | 15 | `$95b3`, row `0a 0c 0f ...`; generic original pending entries, not total eventual Flippers |
| Flipper minimum / maximum | 1 / 4 | `$94cd/$94d6`; selection thresholds, subject to cargo arithmetic |
| Tanker minimum / maximum | 1 / 1 | `$94eb/$9503`, third per-level value is `01` |
| Pulsar minimum / maximum | 0 / 0 | `$9520/$9529`, first eligible table range begins at displayed 17 |
| Spiker minimum / maximum | 0 / 0 | `$9489/$94a5`, third per-level value `00` |
| Fuseball minimum / maximum | 0 / 0 | `$9541/$954e`, no matching Level 3 range |
| `tanker_load[0..3]` | 1, 1, 1, 1 | First two set at `$93d7`; other two from `$9475/$9481`; `1` means two Flippers |
| `max_enm` | 6 | `$9598`, all displayed levels; highest slot index |
| `enm_shotmax` | 1 | `$9407`, third per-level value; two shot slots, not enemy capacity |
| `wave_spikeht` | 0 | `$959c`, no initial Level 3 spikes |

**ROM-BACKED:** Easy and Hard do not change these species or physical-slot
limits. Hard adjusts the generic supply using `$936e-$9377`; for 15 the three
LSRs leave A=1 and carry=1, so ADC produces 17. Medium/Easy remain 15.
Hard's firing-bit change does not enable Pulsar selection on Level 3.

**ROM-BACKED:** All generic pending entries receive `pending_seg` and
`pending_vid` representations at initialization. The ROM has no equivalent
fixed four-Egg queue plus 50-tick batches of six. Pending entries are not
active enemy slots and do not increment the active species/global counters.
A Tanker encodes cargo in its flags, not in two additional physical enemy
slots or stable ledger identities.

**PROJECT-CURRENT:** [LevelConfiguration](../../src/Tempest.Game/Levels/LevelConfiguration.cs)
lines 170-199 defines `Level3Mixed` from the plus-board configuration:
15 originals, four initial Eggs / 11 reserve, 50-tick reserve interval, and
Flippers/Tankers only. Its `MaximumActiveEggs=4` is not enforced by the
production-ledger release path, which adds up to six reserve identities every
50 active ticks. The two-approaching-Egg policy is independently enforced.
The [configuration/release tests](../../tests/Tempest.Game.Tests/Level3MixedWaveTests.cs)
lines 13-80 explicitly encode this distinction.

## 5. Actual ROM admission mechanism

### Global and pending-entry gates

**ROM-BACKED:** `state_04` calls `create_enemies` before enemy movement and
shot collision processing. `$98a7-$98b5` computes
`enemies_in + enemies_top`; both BCC and BEQ allow pending progression when
that sum is at most six. Greater than six blocks it. A nonzero
`zap_running` also blocks it.

The pending-vector walk at `$98c0-$991a` scans 64 entries. Eligible nonzero
`pending_vid` bytes decrement. Reaching zero calls `$9923`, which chooses a
type and allocates an active slot. Success clears that pending entry and
decrements `enemies_pending`. Failure sets the pass-local blocking flag and
increments the exhausted entry back to one, preserving it for later. The
pass also constrains pending progression at a lane-mask boundary and moves
pending lane positions on alternating `timectr` parity.

**ROM-BACKED:** Thus entry is constrained by pending representation/progression,
lane population, type availability, physical slots, and zap state, not just
one timer or a random probability of spawning. POKEY randomness initializes
pending lanes, chooses a type-list starting point, and chooses a cargo-list
starting point. Minimum/type constraints can determine the result without a
meaningful random choice on Level 3. There is no evidence here of the C#
`Rand(0,100)>95` approach rule in Atari.

### Species availability and byte wrap

**ROM-BACKED:** `$99a5-$99be` initializes availability for each species:

```text
available[type] = max(maximum[type] - active_count[type], 0)
```

`$99c0-$99dd` scans occupied enemy slots. For each nonzero cargo-kind flag,
two byte decrements are applied to that cargo species' availability.
For Level 3 this means subtracting two from Flipper availability per Tanker,
with **8-bit wrap**, not clamping negative values to zero.

`$99df-$99f9` computes physical free capacity as
`max_enm + 1 - sum(active_species_counts)` and upper-clamps every availability
to that capacity. `$994d` separately scans slots six down through zero and
will not install into an occupied slot.

For actual active Flippers F and active Tankers T, while counts are consistent:

```text
free_slots = 7 - F - T
flipper_available =
    min(free_slots, (max(4 - F, 0) - 2*T) modulo 256)
tanker_available = min(free_slots, max(1 - T, 0))
```

**ROM-BACKED:** Ordinary Tanker selection also requires nonzero availability
of its cargo species (`$9ad5-$9ae1`). It does NOT require two available units.
With T=0, F=3, a Tanker is admissible because Flipper availability is one.
With F=4 and T=0, neither an ordinary Flipper nor a Tanker can enter.

**ROM-BACKED:** Selection prefers available types whose minimum is
unsatisfied, scanning from higher type index down. Tankers precede Flippers
on Level 3 when both minima are unmet. With no unmet minimum it performs the
bounded random-start type scan. A single available type still needs a nonzero
minimum. This is not a fixed randomized total wave recipe.

Bounded arithmetic evaluation of the verified instructions gave:

| F | T | Physical free | Raw wrapped Flipper availability | After physical clamp |
|---:|---:|---:|---:|---:|
| 0 | 1 | 6 | 2 | 2 |
| 1 | 1 | 5 | 1 | 1 |
| 2 | 1 | 4 | 0 | 0 |
| 3 | 1 | 3 | 255 | 3 |
| 4 | 1 | 2 | 254 | 2 |
| 5 | 1 | 1 | 254 | 1 |
| 6 | 1 | 0 | 254 | 0 |

**ROM-BACKED:** These are literal instruction/arithmetic results for the
specified states. The table maximum of four is therefore an admission input,
not a universal hard Flipper population cap in the mixed wave.

**INFERENCE:** A plausible normal path reaches the wrapping case: a Tanker
plus one ordinary Flipper becomes three Flippers after release; another
Tanker may then enter, yielding F=3,T=1 and permitting further Flippers.
The seven-slot array prevents an eighth active object regardless. A six-
Flipper/one-Tanker state has eight effective Flipper units when latent cargo
is included, and nine unresolved carrier/Flipper/cargo units before including
any pending entries. These are conditional static state consequences, not a
claim that a cabinet run reached them or that they occur frequently.

### Removal, rim, transitions, cargo, firing

| Category / event | ROM treatment | Classification |
|---|---|---|
| Ordinary active Flipper | One physical slot; increments `enemies_in` and `n_flippers` at installation | ROM-BACKED |
| Active Tanker | One physical slot and `n_tankers`; cargo flags additionally subtract two from Flipper availability | ROM-BACKED |
| Latent cargo | No physical slot or `n_flippers` count before split; affects availability, not global active sum | ROM-BACKED |
| Released cargo | Each successful `$994d` allocation becomes an ordinary counted Flipper; not a new pending entry | ROM-BACKED |
| Rim Flipper | `$9d2c/$9d63` transfer `enemies_in` to `enemies_top`, preserving species count and occupied slot | ROM-BACKED |
| Flipping / paused enemy | Still occupies its nonzero slot; transition flags do not exempt it from availability counting | ROM-BACKED |
| Pending entry | Separate pending tables; no active capacity until allocation | ROM-BACKED |
| Ordinary Flipper death/removal | `$a06f-$a098` clears slot and decrements global/species counts, opening capacity | ROM-BACKED |
| Tanker shot/natural split | Same removal clears carrier; two child allocations attempted; normally net +1 active object, not simply freeing capacity | ROM-BACKED |
| Full physical array at Tanker split | Carrier removal guarantees at least its one old slot; second child allocation may fail if no other slot is free. Split does not rerun ordinary species admission | ROM-BACKED |
| Super Zapper sequence | Blocks pending entry while `zap_running` is nonzero. `$a8a4-$a8ac` clears cargo bits before removal, so that ROM zap path does not split a Tanker into live cargo | ROM-BACKED |
| Firing / ammunition / bullets | Separate flags, delay, shot vectors and counters at `$a2a6`; shooting does not decrement enemy occupancy or reserve new enemy capacity | ROM-BACKED |
| Pulsars | Ineligible on Level 3, including Tanker cargo; their generic species/global machinery is not a Level 3 constraint | ROM-BACKED |
| Death / level exit | `$a56a-$a57b` returns remaining active count to generic pending supply; `$a598-$a5b2` recognizes no pending and all surviving objects at the rim for exit | ROM-BACKED |

**INFERENCE:** Full-slot release of a single child is a real compatibility
edge to decide about, not permission to discard a validated C# cargo identity.
No new arcade observation establishes how often that edge is visible.

## 6. Current C# implementation and accepted boundaries

### Accepted Levels 1 and 2 are isolated

**PROJECT-CURRENT:** [TempestGame](../../src/Tempest.Game/TempestGame.cs)
lines 32, 136-139, and 797-813 enforce four active Flippers only for
natural-production canonical `Level1` / `Level2Square` configurations.
The check uses the authoritative collection, including rim and pending-zap
Flippers, at the hatch boundary. Ready Eggs wait without identity loss or
repeated hatch randomness. Production-ledger waves branch to a different
hatch method before that guard.

Read the bounded contract tests in
[Level1ProductionFlipperCapTests](../../tests/Tempest.Game.Tests/Level1ProductionFlipperCapTests.cs)
lines 11-110 and
[Level2FourFlipperCapTests](../../tests/Tempest.Game.Tests/Level2FourFlipperCapTests.cs)
lines 11-90, plus the accepted
[Level 2 specification section](../../TEMPEST-BEHAVIOR-SPEC.md)
lines 2280-2344. No Level 1/2 retuning or reopening is warranted.

### Mixed Level 3 admission and counting

**PROJECT-CURRENT:** [Level](../../src/Tempest.Game/Levels/Level.cs)
lines 194-209, 297-315, and 345-392:

- Releases up to six reserve identities every 50 active ticks, irrespective
  of the nominal four-Egg configuration property.
- Starts one roaming Egg approaching when counted active units are `< 9`
  and approaching/ready Eggs are `< 2`; requires either a `0..100` roll above
  95 or `active units + current Eggs < 4`.
- Continues already-approaching Eggs regardless of later active population.
- Loops through all ready Eggs and calls the factory, with no global hatch cap.

**PROJECT-CURRENT:** `TempestGame` lines 836-967 selects/constructs mixed
enemies:

- `CountProductionActiveEnemyUnits` counts Flippers, Tankers, Spikers and
  shot-released cargo pending activation. All Flippers count, including rim,
  ordinary/cargo, transition and pending-zap Flippers.
- Eggs/reserve and latent cargo do not count toward the approach gate.
- `Level3WaveSelection` only admits Flippers or Tankers and uses the six-
  Tanker eligibility range. The factory also rejects a Tanker when the
  active Tanker count is already six.
- Ordinary Flippers have no species limit. Ready cargo Eggs and previously
  selected original Flippers also lack a hatch capacity check.
- Pulsars/Fuseballs are not part of this normal wave or this counting helper;
  no Level 3 Pulsar-cap change is needed.
- `< 9` is an approach admission condition, not a hard active ceiling:
  in-flight Eggs still hatch and carrier release independently increases
  active object count.

**PROJECT-CURRENT:** The
[Level3MixedWaveTests](../../tests/Tempest.Game.Tests/Level3MixedWaveTests.cs)
lines 84-124 lock down the 9/8 active approach boundary and roll 95/96;
lines 158-204 expressly lock down six active Tankers and demonstrate six plus
an ordinary Flipper. These are current C# contracts, not evidence for Atari.
No new gameplay maximum was measured in this investigation.

**REFERENCE-DERIVED:** Incitti
[`tempest.bmx`](../../Reference/incitti/tempest.bmx) lines 1982-1995 and
2093-2099 contain the six-Tanker choice and `< 9`, `< 2`, probabilistic
approach conditions. This establishes their engineering origin, not arcade
authenticity. C# is source-shaped rather than a line-for-line timing port.

### Identity, cargo, reconstruction, zap, completion

**PROJECT-CURRENT:** [Level3WaveLedger](../../src/Tempest.Game/Levels/Level3WaveLedger.cs)
lines 11-33, 171-194, 229-251, and 253-348:

- Starts 15 original stable identities; each first Tanker commitment adds
  two owned cargo identities. Cargo is initially latent, not a runtime Flipper.
- Distinguishes latent, pending-shot-activation, active, reentry reserve, Egg
  and terminal cargo states. Unresolved ledger counts are completion
  obligations, not a concurrency capacity.
- Death preserves selected original species, latent ownership and terminal
  sets, returning unresolved materialized cargo as separate fresh Flipper
  Eggs ahead of original reserve. Neither allocation nor capacity policy
  should reset or merge those identities.

**PROJECT-CURRENT:** `TempestGame` lines 1280-1409, 1448-1500, 1594-1616
and 1787-1845:

- Removing a Tanker materializes its complete pair. Shot children enter the
  pending-activation list; natural children enter the Flipper list after its
  update phase; zap children retain accepted delayed/stillborn handling.
  There is no cargo-release concurrency gate. Before and after shot
  activation the counted total includes both children.
- Pending-zap enemies remain counted until actual removal. Shooting,
  ammunition depletion or reaching the rim does not free capacity.
- Killing a Flipper frees one counted unit; carrier removal plus two children
  normally increases counted units by one.
- Death clears runtime collections and reconstructs the unresolved ledger.
  Completion requires every obligation resolved; all-rim completion excludes
  reserve, Eggs, pending activation and latent cargo and verifies exact
  represented identity sets. A concurrency wait must not become a kill,
  escape, rim-clear or completion shortcut.

**PROJECT-CURRENT:** Existing C3b acceptance in
[development notes](../../TEMPEST-DEVELOPMENT-NOTES.md) lines 1671-1712 and
[behavior specification](../../TEMPEST-BEHAVIOR-SPEC.md) lines 1391-1400
accepts the visible C# mixed wave and lifecycle, expressly not unknown
Atari concurrency or exact probabilities. Some early documentation describes
silent direct diagnostics; current production firing has later evolved and
is not proposed for change.

**ARCADE-OBSERVED:** Previously documented ordinary shot destruction releases
two live children in distinct non-carrier lanes
([behavior specification](../../TEMPEST-BEHAVIOR-SPEC.md), lines 1089-1120).
That supports retaining normal complete-pair behavior, but is not a
measurement of the full-slot edge, a Level 3 cap, or hidden ledger semantics.

## 7. ROM versus C# comparison

| Subject | ROM-BACKED | PROJECT-CURRENT | Assessment |
|---|---|---|---|
| Medium original supply | 15 generic pending entries | 15 original IDs | Agree in count; representation differs |
| Eligible enemies | Flippers, Tankers with two-Flipper cargo | Same | Agree; no Pulsars |
| Species mixture | Availability/minimum-driven at entry, no fixed total of each | Incitti-shaped randomized hatch choice | Confirmed selection mismatch |
| Active Tankers | At most one through ordinary entry | Up to six | Confirmed mismatch |
| Flipper admission | Table maximum 4, wrapping latent-cargo adjustment, physical clamp | No Level 3 species availability check | Confirmed mismatch; strict four cap would still be wrong |
| Physical active capacity | Seven slots, inclusive `max_enm=6` | `< 9` at approach only; no physical hatch array | Confirmed mismatch |
| Latent cargo | Reserves species availability, not active physical slots | Ledger obligations only, excluded from approach count | Confirmed admission mismatch |
| Released cargo | Counted Flippers; bypasses ordinary species picker; physical allocation can fail | Always complete pair, including deferred shot activation | Broad counting agrees; saturation edge differs |
| Rim / transitions | Still counted until removal/exit | Still in counted Flipper collection | Agree in capacity concept |
| Pending entries | Initialized pending vectors, progression gated | Four initial Eggs, batches, probabilistic approach | Confirmed scheduling mismatch; not proposed for wholesale rewrite |
| Super Zapper | Entry pause; carrier cargo flags cleared before removal | Existing delayed removal and stillborn cargo pair; no equivalent global entry pause | Existing lifecycle/admission divergence; preserve pending explicit authorization |
| Enemy fire | Separate projectile accounting; no capacity release | Separate bullets/ammo; enemy object stays counted | Agree in capacity concept |
| Death | Generic active survivors added back to pending supply | Stable species/cargo identity reconstruction | Established project translation, not a cap implementation prerequisite |

**UNKNOWN:** No new arcade/MAME run establishes actual Level 3 maximum
occupancy reached, distribution of Tanker/Flipper totals, frequency of byte-
wrap cases, full-slot cargo loss frequency, exact pending timing in 60-Hz C#
ticks, or Neal's cabinet revision/difficulty. The data and branching are
well established for the inspected local Rev. 2 files; other revisions were
not checked. Do not describe the conditional maximum as a measured arcade
population or a four-Flipper plus one-Tanker invariant.

## 8. Recommended smallest safe next milestone

All recommendations in this section are **INFERENCE**, not implemented policy.

### Recommendation

Authorize a narrowly scoped **Level 3 mixed-admission diagnostic**, not a
blanket four-Flipper cap and not an automatic production promotion.

Reuse the Level 1/2 engineering principle of checking capacity at the ready-
Egg hatch boundary and leaving blocked identities untouched. Do not merely
extend `HasProductionFlipperLimit`: mixed waves take another branch and need
species/cargo-aware admission. Do not blindly reuse Level 8's strict six-
object/saturating-reservation selector either; it is a project translation,
not the literal Level 3 ROM rules.

### Bounded implementation outline for a later authorized task

1. Gate the diagnostic to the canonical production Level 3 configuration
   across campaign/direct/recorded startup. Do not change Levels 1/2 or
   incidentally apply policy to other ledger waves.
2. Compute a Level 3 occupancy snapshot from authoritative live collections:
   ordinary plus cargo Flippers, Tankers, pending shot cargo, and latent cargo
   on active carriers. Count rim/transitions/pending zap. Exclude reserve,
   Eggs, terminal ledger entries, bullets and destruction presentation.
3. Implement a small pure Level-3 admission decision using one-Tanker
   availability, literal Flipper byte-wrap arithmetic, seven physical
   admission places, and minimum preference. Avoid random rejection loops.
   Cache no stale occupancy across multiple successful same-tick hatches.
4. Apply the decision before consuming/removing an Egg, committing its type,
   allocating cargo, marking a cargo Egg active, or drawing construction
   randomness. A denied Egg remains ready and unresolved. For already-
   committed original/cargo reentry, gate its retained species without
   rerolling identity/type. Treat that reentry policy explicitly as a
   project translation because the ROM reconstructs generic pending supply.
5. Leave release/approach timing, firing, cargo geometry, scoring, death,
   ledger conservation, zap timers and completion unchanged initially.
   Changes to type probabilities are intentional consequences of replacing
   the six-Tanker picker, not an excuse to rewrite the whole scheduler.

### Required fidelity decision before strict-ceiling claims

Literal ROM full-slot behavior can allocate only one child when a Tanker was
one of seven active objects. C# deliberately materializes and retains both
cargo identities. Those cannot both remain unchanged AND guarantee a strict
seven-object total at every cargo release.

Do not silently drop, resolve, recreate, defer or merge a child to make a
counter pass. Two legitimate later scopes need Neal's explicit choice:

- **Admission-only diagnostic (recommended first):** reproduce ordinary-entry
  availability but preserve accepted complete-pair materialization. Explicitly
  report that cargo may cause a release-driven population above the ROM physical
  array (e.g. six Flippers plus a Tanker becomes eight Flippers). Block further
  ready-Egg hatches while above capacity. This is a deliberate project
  approximation, NOT exact global concurrency fidelity.
- **Strict physical concurrency:** separately authorize a lifecycle-compatible
  overflow strategy or ROM-faithful missing-child semantics after targeted
  evidence/manual comparison. Reserving extra physical headroom at admission
  can preserve pairs but changes the ROM's permissive nonzero-availability
  rule; it too must be labeled a project approximation. Do not bundle this
  silently into a purportedly simple cap.

Similarly, ROM-wide zap entry exclusion can be investigated as an admission
choice without replacing the accepted stillborn-cargo lifecycle. No need to
reopen full Super Zapper or death/reconstruction fidelity to correct the
obvious six-versus-one Tanker mismatch.

### Required bounded validation for that future task

- Pure admission cases: F=0/1/2 with T=1; F=3,T=0 carrier eligibility; wrapping
  F=3/4/5,T=1; total six/seven; no Pulsars; minimum priority.
- Same-tick ready-Egg burst, capacity reopening, no repeated blocked RNG,
  stable waiting identity, rim/transition/pending-zap counts.
- Ordinary versus cargo reentry and selected-Tanker reentry without identity
  replacement; complete-pair natural/shot/zap lifecycle, pending activation,
  death and exact completion sets.
- Explicit full-slot pair-release scenario and documented chosen deviation.
- Keep multi-carrier lifecycle stress fixtures as deliberately artificial
  fixtures if useful; do not retain six active production Tankers merely
  because an old production expectation asserts six.
- Smallest affected tests first, finite tick/seed limits, useful failure state
  and VSTest hang protection. Then deterministic campaign/recorded-path
  regressions and Neal's manual diagnostic acceptance. No broad refactor.

## 9. Investigation validation and final state

**PROJECT-CURRENT:** Inspection was bounded by file/range/result limits.
Direct ROM reads used fixed chip files/ranges; the arithmetic diagnostic
evaluated exactly seven F values. No unbounded gameplay loop was executed.

One initial PowerShell binary-dump command returned **FAIL** due to a nested-
array/range formatting mistake for its single split range, after the earlier
table/admission ranges had printed successfully. It was not a security block,
not a failed gameplay test, and not treated as a successful complete check.
A corrected direct fixed-offset read completed for `$a06f-$a0f6`; the separate
fixed-offset zap and remaining-table checks also completed successfully.

**PROJECT-CURRENT:** Only the ignored prompt and this ignored report were
created. No production, test, project, durable gameplay documentation,
instruction, ROM/reference, TEMP, UserData or recording file was edited.
No game, tests or build were run; no binary validation is claimed.
No staging, commit or push occurred. Levels 1 and 2 remain accepted and
unchanged. No implementation milestone was started.

Closing checks passed: HEAD remains
`7acac60035383bfb6e0cb170c0adfa0dd88b6c4b`; `git diff --exit-code` and
`git diff --cached --exit-code` confirm a clean tracked worktree and index;
`git diff --check` passes. `git check-ignore -v` confirms both new AI records
are ignored. The archived prompt has exactly the required standalone token.
Final status is `master...origin/master` with only the same 21 pre-existing
untracked ROM files under `Reference/arcade/roms.rev2/`.
There is no manual gameplay acceptance required for this report.
Neal's next action is to review the findings and authorize the desired
diagnostic/fidelity scope before implementation.
