# Level 4 concurrency and ordinary admission: ROM investigation

Date: 2026-11-09

Disposition: **INVESTIGATION COMPLETE. LEVEL-4-SPECIFIC DIAGNOSTIC RECOMMENDED;
NO IMPLEMENTATION AUTHORIZED OR PERFORMED.**

## 1. Scope, baseline, and evidence

The entire substantive task was read before repository action. Exactly one
standalone completion token was established. The complete task, excluding
automatically appended context, is preserved in the
[matching prompt record](../Prompts/2026-11-09-level4-concurrency-rom-investigation-prompt.md).

Starting branch: `master`, tracking `origin/master`. HEAD and the locally
recorded upstream both equal the expected accepted baseline:
`af560b100bebad672ec9231d4f88332848378594`.
Initial short status, tracked diff, and staged diff were empty. No unexpected
nonignored untracked material was reported.

Only the two requested ignored AI-Logs records were written. Production,
tests, accepted gameplay documentation, and reference material were not
modified. Levels 1-3 were not reopened or retuned.

Evidence classifications follow the
[repository instructions](../../.github/copilot-instructions.md):

- **ROM-BACKED:** supported by instructions/data in the public local
  [ROM-derived disassembly](../../Reference/arcade/Tempest%20Rom%20Source%20code.md).
  Local raw ROM copies were ignored: not opened, hashed, or enumerated.
  This task does not claim new independent binary verification or a
  running-cabinet measurement.
- **REFERENCE-DERIVED:** Incitti source, not Atari authority.
- **PROJECT DECISION / PROJECT TRANSLATION:** accepted intentional behavior
  and engineering mappings recorded in durable project documentation.
- **ARCADE-OBSERVED:** only the particular previously documented observations,
  not a new arcade/gameplay session in this task.
- **INFERENCE:** consequences, reachability examples, and recommendations.
- **APPROXIMATION:** known nonliteral representation or behavior.
- **UNKNOWN:** not established by the available evidence.

Plain statements labeled "current C#" below are source inspection, not claims
that source behavior is arcade-validated. Historical reports are not accepted
behavior authorities.

## 2. Executive findings

1. **ROM-BACKED:** Medium/Easy displayed Level 4 has **17 generic pending
   entries**. Ordinary selectable species are **Flippers and Spikers**, NOT
   Tankers. Min/max: Flipper **1/4**, Spiker **1/2**, Tanker **0/0**,
   Pulsar **0/0**, Fuseball **0/0**. Hard increases supply to **19** without
   changing those species limits.
2. **ROM-BACKED:** Seven physical enemy slots are shared. Before pending
   supply is exhausted, ordinary entry can fill at most four Flippers and
   two Spikers: six objects, even though a seventh physical place exists.
   Species are chosen dynamically; this is not a fixed final wave recipe.
3. **ROM-BACKED:** Tankers are nevertheless possible. A retreating Spiker
   remains a live slot, hops to a shortest-spike lane, and, once
   `enemies_pending == 0`, becomes a two-Flipper Tanker in that same slot.
   This bypasses ordinary Tanker admission entirely.
4. **INFERENCE from the ROM:** Up to two such carriers can coexist in a fresh
   normal wave/life. Their cargo can take Flippers above four; the seven-slot
   allocator still bounds actual live objects. Therefore neither "no Tankers
   ever" nor "Level 4 is always capped at four Flippers/six enemies" is correct.
5. Current C# instead allows ordinary Flipper/Tanker/Spiker selection,
   six Tankers, four Spikers, unlimited Flippers at hatch, and an
   Incitti-shaped `< 9` approach gate rather than a physical hatch ceiling.
   Its accepted Level 3 admission policy is correctly isolated from Level 4.
6. **INFERENCE:** Reuse the accepted ready-Egg/ledger/occupancy engineering,
   not the unchanged Level 3 policy. Level 4 needs a Spiker-aware selector
   AND an explicit end-of-pending lifecycle decision. A cap-only change can
   reintroduce endless final-Spiker recycling.

## 3. Reused machinery versus reverified evidence

Starting point:
[Level 3 investigation](2026-11-09-level3-concurrency-rom-investigation-report.md).
Its then-current C# description predates accepted Level 3 admission and was
not treated as the current implementation.

Reused generic conclusions: separate pending/active representations, inclusive
slot allocation, species availability, wrapping cargo subtraction, physical
clamp, minimum preference, removal/split accounting, rim occupancy, and zap
entry exclusion. No new raw-chip analysis was needed.

Reverified applicability by bounded listing reads:

| Area | Listing lines | Symbols/addresses examined |
|---|---:|---|
| Pending initialization and count clearing | 2471-2549 | `$9234-$928e`; effective level setup `$92c5` |
| Common parameter loader/difficulty/cargo defaults | 2549-2673 | `$92d6-$93df` |
| Level 4 table entries and destination pointers | 2719-2905 | `$944d-$965d`, detailed below |
| Table operation semantics | 2916-2996 | `$9677-$96ee`: constant, per-level, block-of-16, speed addition |
| Shared active-play call path | 3016-3025 | `$970b`: zap, create, move, shots, collisions |
| Pending progression and physical allocation | 3217-3332 | `$98a2-$99a4` |
| Availability and type selection | 3335-3525 | `$99a5-$9afc` |
| Shared movement interpreter and rim transfer | 3543-3835 | `$9b1e-$9d66`, Spiker p-code dispatch included |
| Newly important Spiker lifecycle | 4104-4180 | `$9fc4-$a06e` |
| Removal, cargo allocation, Spiker movement program | 4180-4262 | `$a06f-$a101` |
| Death and all-rim exit | 4836-4930 | `$a504-$a5b2` |
| Zap exclusion/removal | 5233-5289 | `$a839-$a8ac` |

The common `state_04` path and parameterized picker contain no separate
displayed-Level-4 admission routine. What changes is the loaded data and
Spiker movement program. In particular, the Spiker-to-Tanker conversion was
not a Level 3 mechanism and required a new trace.

## 4. ROM-backed Level 4 parameters and composition

**ROM-BACKED:** Effective table level is `curlevel + 1`; displayed Level 4
uses the fourth entry, not internal index 4.

| Parameter | Level 4 value | Table/address and interpretation |
|---|---:|---|
| `wave_enemies` | 17 (`$11`) | `$95b3`: `0a 0c 0f 11 ...`; generic original pending entries |
| Flipper min/max | 1 / 4 | `$94cd/$94d6`, inclusive displayed 1-4 range |
| Tanker min/max | 0 / 0 | `$94eb/$9503`: fourth values both `00` |
| Spiker min/max | 1 / 2 | `$9489/$94a5`: fourth values `01` / `02` |
| Pulsar min/max | 0 / 0 | `$9520/$9529`, no matching range before displayed 17 |
| Fuseball min/max | 0 / 0 | `$9541/$954e`, no matching Level 4 range |
| `tanker_load[0..3]` | 1, 1, 1, 1 | `$93d7-$93dc`, `$9475`, `$9481`; kind 1 = two Flippers |
| `max_enm` | 6 | `$9598`; highest index, physical capacity **seven**, slots 0..6 |
| `wave_spikeht` | `$e0` (224) | `$959c`, fourth block-of-16 entry; initial spike coordinate, not C# height |
| `flipper_move` | `$24` | `$95f4`, fourth block-of-16 entry; movement program, no extra admission species |
| `shot_holdoff` | 71 | `$93fa`: `80 + (4-1)*(-3)` |
| `enm_shotmax` | 2 | `$9407`, fourth value; highest shot index, three enemy-shot places, NOT enemy places |

Difficulty code `$9328-$937f` does not alter min/max species values, cargo
kind, or physical enemy capacity. Easy leaves pending supply at 17; Hard
uses the carry from the third LSR in its ADC:

```text
HardSupply = 17 + floor(17 / 8) + bit2(17) = 17 + 2 + 0 = 19
```

Easy/Hard also change firing capacity and Flipper speed/rim acceleration as
shown in that routine; such speed changes affect occupancy duration, not the
species admission maxima. Default enemy-shot places are three, Easy two,
Hard four. No new difficulty mode or timing translation is proposed.

**ROM-BACKED:** Pending entries are initialized in the generic pending
vectors; they are not preassigned stable species. Initial spike hazards are
separate from enemy supply. A live Spiker may make repeated growth/retreat/hop
cycles without consuming another pending original. Later Tanker cargo is
additional live offspring, not part of the 17 initial pending entries.
Consequently "17 enemies" is not a fixed total of eventual live Flippers,
nor a predetermined Flipper/Spiker/Tanker mixture.

## 5. Actual admission and live transformations

### 5.1 Ordinary entry while pending supply remains

**ROM-BACKED:** `$98a7-$98be` permits pending countdown only while
`enemies_in + enemies_top <= 6` and `zap_running == 0`. The pending scan,
lane-mask constraint, failed-entry retry, and independent allocator remain
as established for Level 3. Reaching zero pending visual/countdown state
attempts selection/allocation; only success decrements `enemies_pending`.
Failure preserves the pending entry for retry.

With consistent pre-conversion species counts, let F = active Flippers and
S = active Spikers. Ordinary Level 4 has no carriers and thus no latent
cargo subtraction:

```text
free = 7 - F - S
availableFlipper = min(free, max(4 - F, 0))
availableSpiker  = min(free, max(2 - S, 0))
availableTanker = availablePulsar = availableFuseball = 0
```

The maximums are admission inputs, not assumed universal populations.
Here, absent cargo/transformations, they really do bound ordinary active
Flippers to four and Spikers to two. At F=4,S=2 a seventh slot is physically
free but no species is available. The outer global gate being open is
therefore insufficient to admit another enemy.

**ROM-BACKED:** Types are indexed Flipper=0, Pulsar=1, Tanker=2, Spiker=3,
Fuseball=4. Descending unmet-minimum preference chooses a Spiker before a
Flipper when both are absent. After one Spiker, an absent Flipper gets
preference. When both minima are satisfied and both types available, the
bounded random-start descending scan determines the type. The special
shortest-spike Spiker-versus-Tanker admission branch at `$9a40-$9a60` cannot
run on ordinary Level 4 because Tanker availability is zero.

The one-available-type branch still requires its nonzero minimum; both
Level 4 ordinary species have minimum one. There is no source-shaped
"active enemies plus Eggs below four excludes Spikers" ROM entry condition.

Six explicitly evaluated states (PowerShell arithmetic only; no simulation):

| F | S | Free physical | Flipper availability | Spiker availability | Selection consequence |
|---:|---:|---:|---:|---:|---|
| 0 | 0 | 7 | 4 | 2 | Spiker minimum first |
| 0 | 1 | 6 | 4 | 1 | Flipper minimum first |
| 1 | 1 | 5 | 3 | 1 | Bounded random-start scan between eligible types |
| 4 | 1 | 2 | 0 | 1 | Spiker only |
| 1 | 2 | 4 | 3 | 0 | Flipper only |
| 4 | 2 | 1 | 0 | 0 | No entry, despite one physical free place |

### 5.2 Spiker retreat is not a return to pending supply

**ROM-BACKED:** Spiker program `$a0f7` repeatedly moves and executes
`eact_0e=$9fc4`. It grows a spike, reverses at along `$20`, retreats to `$f2`,
and invokes `spiker_hop=$a028`. The hop examines 16 lane candidates and
chooses a shortest-spike lane (largest along-coordinate; absent spike treated
as `$ff`), clears retreat direction, and resets along to `$f0`.

- If `enemies_pending != 0`, the SAME live Spiker continues. Its occupied
  physical slot is not freed, it does not increment pending supply, and it
  does not pass through ordinary species admission again.
- If `enemies_pending == 0`, `$a00e-$a01f` installs cargo flag 1 and changes
  the object's type bits to Tanker. `$a022-$a024` and p-code conditional
  `$a0f9` send it to the straight-climb Tanker route. No new pending entry
  or physical allocation is involved in this conversion.

Thus **maximum ordinary admitted Tankers is zero**, while live converted
Tankers are legitimate. **INFERENCE:** Since at most two Spikers were admitted
and no pending entries remain to replenish them, at most two converted
Tankers coexist in an otherwise normal fresh Level 4 wave/life. This is a
static consequence, not a measured frequency or a blanket rule for C#'s
identity-preserving post-death translation.

**ROM-BACKED, important bookkeeping detail:** The conversion instructions
change type/cargo flags, not `n_spikers` or `n_tankers`. Installation increments
the initial species counter at `$9995`; removal decrements the CURRENT type
at `$a098`. The bounded listing search found no compensating conversion
counter writes. Consequently, after conversion these byte counters need not
equal the actual species visible in slots, and a converted Tanker's removal
can decrement the Tanker counter from zero to `$ff`.

Do not silently describe this as a conventional live-species cap or faithfully
copy the stale counters into a C# ledger. In the normal path it happens only
after pending supply is exhausted, so there is no later ordinary entry for
those stale/wrapped species counters to select. Generic clearing resets them
for reconstruction. Actual occupied slots still control allocation.

### 5.3 Cargo arithmetic and late-wave population

**ROM-BACKED:** Generic latent-cargo handling still executes two wrapping
byte decrements per carrier, after saturating max-minus-count and before the
physical clamp. A hypothetical F=3,T=1 snapshot with consistent counts gives
`(4-3-2) mod 256 = 255`, then physical clamping.

**Crucial Level 4 distinction:** A normal carrier arises only AFTER
`enemies_pending` becomes zero. Therefore that wrap cannot reopen ordinary
pending entry in the normal Level 4 path. The Level 3 explanation of wrapping
enabling additional ordinary Flippers must NOT be imported as Level 4's
normal mechanism. Released cargo, rather than ordinary wrapped admission,
can take Level 4 Flippers above four.

The removal/split routine `$a06f-$a0f6` frees the carrier slot and tries two
child allocations through `$994d`. Cargo does not rerun ordinary min/max
selection and does not decrement pending supply. Latent children have no
physical places before release; each successfully allocated child occupies
one afterward. If only one slot is free after carrier removal, the second
child cannot be allocated.

**INFERENCE, bounded conditional example, not a cabinet observation:**

```text
F=4,S=2, pending=0: six occupied slots.
Both Spikers complete retreat and convert -> F=4,T=2: still six slots.
First Tanker splits -> F=6,T=1: seven slots.
Second Tanker splits -> F=7: its old slot permits one child; second fails.
```

This explains both the seven-object bound and why four is not an absolute
late-wave Flipper ceiling. It does not assert that every wave reaches these
states or that all 17 originals become this final composition.

### 5.4 Occupancy and release rules

| Entity/event | ROM-BACKED treatment |
|---|---|
| Live ordinary Flipper/Spiker | One shared physical slot; species/global counts at installation |
| Rim Flipper | `enemies_in` transfers to `enemies_top`; slot/species occupancy retained |
| Flipping, paused, growing or retreating enemy | Remains an occupied slot; no transition exemption |
| Spiker hop/conversion | Same occupied slot, no ordinary re-admission |
| Latent Tanker cargo | No physical slot; affects generic species availability |
| Released cargo | Physical Flipper slots, bypass ordinary species selection |
| Pending entries | Separate vectors; not active slots until allocation |
| Pending zap target | No C#-style per-enemy timer equivalence; occupied until ROM removal |
| Zap sequence | Nonzero `zap_running` pauses pending entry; targets remain occupied until removed |
| Zap Tanker removal | `$a8a4-$a8ac` clears cargo bits before removal; no live cargo split |
| Shot/destruction | Actual removal clears slot and decrements active total |
| Tanker natural/shot split | Frees carrier then allocates children: not simply one place permanently freed |
| Enemy bullets and persistent spikes | Separate arrays/hazards; do not occupy enemy slots |
| Life loss | Active population returns to generic pending count; fresh species selection after reset |
| No pending and all surviving enemies at rim | Existing ROM exit path; not admission freeing capacity early |

## 6. Current production C# Level 4

Principal inspected sources:
[configuration](../../src/Tempest.Game/Levels/LevelConfiguration.cs),
[Level](../../src/Tempest.Game/Levels/Level.cs),
[game](../../src/Tempest.Game/TempestGame.cs),
[selector](../../src/Tempest.Game/Levels/Level3WaveSelection.cs),
[ledger](../../src/Tempest.Game/Levels/Level3WaveLedger.cs),
[Spiker](../../src/Tempest.Game/Gameplay/Spiker.cs),
and [Egg](../../src/Tempest.Game/Gameplay/Egg.cs).

### Configuration and scheduling

- Configuration lines 197-223: `Level4Candidate`, 17 originals,
  Flippers/Tankers/Spikers, four initial Eggs and 13 reserve, two approaching
  Eggs, 50-tick reserve interval, height-60 persistent Spikes, AvoidSpikes exit.
  Natural progression/direct diagnostic use the same configuration.
- `Level.cs` lines 194-210: the production-ledger path releases up to six
  reserve identities every 50 active ticks. `MaximumActiveEggs=4` does NOT
  cap this path's Egg list.
- Lines 345-364: `activeEnemyUnits < 9` and fewer than two nonroaming Eggs
  permit beginning approach. Near end (`activeEnemyUnits + eggs.Count < 4`)
  bypasses the random test; otherwise `randomInclusive(0,100) > 95`.
  This is an approach gate, NOT a nine-enemy hatch ceiling.
- Eggs advance on every eighth update, approach by +2 height, then wait ready.
  Ready Eggs count among the two nonroaming Eggs. The configured
  `HatchSelectionIntervalTicks=50` is not used by this mixed approach path.
- `HatchReadyMixedEnemies` lines 367-393 examines ready Eggs sequentially and
  immediately adds each successful hatch. Null leaves its identity ready.
  It does not enforce seven physical places or a shared hard population cap.

### Selection and limits

`TempestGame.cs` lines 838-949 uses the production ledger and Level 4 selector.
Unselected originals can become any of the three configured species.
Spikers are suppressed when active population plus Eggs is below four, or
active Spikers exceed three. A construction guard rejects a Spiker when
`spikers.Count > 3`, permitting **four active Spikers**.

The Tanker constructor guard permits **six active Tankers**. There is no
Level 4 Flipper admission ceiling or latent-cargo reservation. Cargo reentry
bypasses original type selection and has no Level 4 physical-slot guard.
Construction/cargo rolls can be consumed on unsuccessful type attempts;
Level 4 does not have the Level 3 deny-before-randomness policy.

**REFERENCE-DERIVED comparison:** Incitti lines 1982-2032 supplies the
six-Tanker selection boundary and late-Spiker suppression; lines 2094-2096
supplies the `<9`/two-approaching/random approach shape; lines 3659-3670
removes retreating Spinners and increments reserve Eggs. C# retains this
general architecture, not Atari admission.

**APPROXIMATION / current-source nuance:** This is not an exact Incitti picker.
C# accepts candidate **3 or 4** as Tanker
(`Level3WaveSelection.cs`, line 74), whereas Incitti candidate 3 is Fuseball
and candidate 4 is Tanker. With six Tankers, C# upper=3 still allows selection
of Tanker candidate 3, but the construction guard rejects it. Therefore the
six-Tanker guard, not the picker alone, establishes the limit. No probability
equivalence to Incitti is claimed. This was documented, not corrected.

### What counts and what frees capacity

`TempestGame.cs` lines 987-998 counts:

```text
flippers.Count + tankers.Count + spikers.Count
    + shotReleasedCargoPendingActivation.Count
```

This includes ordinary/released-cargo Flippers, rim/turning enemies, and
pending-zap live objects until removed. Pending shot cargo is counted even
before entering the Flipper collection. Latent carrier cargo, reserve,
roaming/approaching Eggs, terminal ledger identities, bullets, spike hazards,
and destruction visuals do not count in this active population. Eggs are
separately added for near-end selection/approach conditions. Fuseballs and
Pulsars are ineligible on this wave, not additional entry choices.

Actual shot kills, delayed zap expiry, carrier removal and Spiker reserve
return reduce their live collections; carrier release adds two children,
normally increasing total population by one. Moving to the rim or beginning
a transition does not reduce a collection count. Death clears runtime objects
and reconstructs unresolved identities as Eggs. All-rim resolution is nonkill
accounting followed by exit, not a way to spawn extra active enemies.

No zap-state gate surrounds Egg release, approach or hatch in the active-play
path (`TempestGame.cs`, lines 377-400). Pending-zap objects still count, but
entry is not globally paused for the ROM's zap interval. Display flash duration
is not an equivalent admission flag.

### Cargo, Spikers, and reconstruction

- `ReleaseTankerCargo`, lines 1364-1430, preserves two cargo identities.
  Natural release marks carrier escaped without Tanker kill score.
  Shot release scores 200 and queues live children for activation after the
  current enemy update. Zap release scores the carrier and gives stillborn
  children their own accepted delays. No literal seven-slot child loss.
- `UpdateSpikers`, lines 1152-1208, keeps growing/retreating Spikers live
  until `ReturnedToReserve`; then removes the object and returns the same
  unresolved original to reserve with type `Unselected`.
- The existing same-ID conversion helper
  `Level3WaveLedger.ConvertActiveSpikerToTanker` (lines 352-365) is used only
  when `UsesGenericRomDeathReconstruction` and no generic pending units remain.
  That property is true for Level 7/8 policies, NOT Level 4. Therefore Level 4
  does not perform the ROM terminal-Spiker conversion.
- Ordinary Level 4 death preserves selected original species and latent cargo
  commitments; unresolved materialized cargo reenters individually. Terminal
  outcomes stay terminal. It is not the ROM's generic species reconstruction.

### Level 3 isolation

**PROJECT DECISION / PROJECT TRANSLATION:** The accepted
[Level 3 specification](../../TEMPEST-BEHAVIOR-SPEC.md), lines 2346-2406,
documents its accepted waiting policy, wrapping cargo arithmetic, one-Tanker
ordinary admission, and complete-pair saturation translation.

Current `HasLevel3MixedAdmissionDiagnostic`
(`TempestGame.cs`, lines 140-141) uses reference identity against canonical
`Level3Mixed`. Level 4 is a different configuration and does not enter
`TrySelectLevel3DiagnosticType`. The four-Flipper production guard likewise
is restricted to Levels 1/2 and bypassed by production-ledger hatching.
These are source-confirmed boundaries, not requests to change Levels 1-3.

## 7. ROM versus C# comparison

| Subject | ROM-BACKED Level 4 | Current C# Level 4 | Assessment |
|---|---|---|---|
| Initial supply | 17 Medium/Easy, 19 Hard generic entries | 17 original IDs | Default count agrees; no Level 4 difficulty translation established |
| Ordinary species | Flipper, Spiker | Flipper, Tanker, Spiker | Confirmed mismatch |
| Ordinary Tankers | Zero (min/max 0/0) | Up to six | Confirmed mismatch |
| Tankers later | Spiker conversion after pending=0 | Ordinary selection, not terminal Spiker conversion | Confirmed lifecycle mismatch |
| Ordinary Flippers | Availability from max 4, minimum 1 | No species gate | Confirmed mismatch |
| Spikers | Max 2, min 1; priority above Flipper minimum | Up to 4, near-end suppression | Confirmed mismatch |
| Shared capacity | Seven physical places; ordinary F/S can use at most six | `<9` only at approach; no physical hatch ceiling | Confirmed mismatch |
| Cargo | Two Flippers per carrier; bypass ordinary species picker | Complete two-Flipper pair, bypass original picker | Broad agreement; carrier origin/overflow/zap differ |
| Latent reservation | Generic wrapping subtraction, but no normal pending entry after conversion | No Level 4 reservation | Mechanisms differ; not the active ordinary-Level-4 mismatch driver |
| Spiker retreat with pending | Same live slot, shortest-spike hop | Frees live slot, same ID returns via reserve/Egg/picker | Confirmed difference; accepted C# translation |
| Final Spiker | Converts, preventing indefinite same-Spiker hopping | Late-wave selector suppresses Spiker reselection | Different termination mechanisms |
| Rim/transitions/pending zap | Capacity retained until actual removal | Counted live collections retained | Agreement in counting concept |
| Zap entry | Paused by `zap_running` | No equivalent global pause | Confirmed admission difference; preserve accepted zap lifecycle |
| Bullet/spike hazards | Separate from enemy places/supply | Separate from enemy identities/count | Agreement |
| Death | Generic surviving supply, fresh species | Stable types/owned cargo retained | Accepted translation, not permission for broad rewrite |
| Starting spikes | `$e0` ROM along coordinate | C# height 60 | Both start spikes; exact coordinate/height equivalence not established here |

**INFERENCE:** The Level 3 ready-Egg boundary, authoritative collection
snapshot, deny-before-commit behavior, and identity-preserving wait can be
reused. The unchanged `Level3MixedAdmissionPolicy` cannot: its Tanker min/max
are 1/1, its selector excludes Spikers, and its normal cargo-wrap scenario
assumes ordinarily admitted carriers. Merely changing Tanker constants to
zero would still omit Spiker admission and lifecycle handling.

The Level-4-specific data/rules must include F 1/4, S 1/2, T 0/0, inactive
Pulsar/Fuseball, descending Spiker minimum preference, bounded type choice,
seven physical admission places, and a separate terminal-Spiker conversion
decision. A generic extra abstraction is not required if a small pure
Level-4 policy is clearer.

## 8. Accepted boundaries a later diagnostic must preserve

**PROJECT DECISION / PROJECT TRANSLATION unless explicitly noted otherwise:**

- Stable original/cargo namespaces, owned ordinal cargo pair, conservation,
  exact representation checks, and distinct destroyed/escaped/rim-cleared
  outcomes. No hidden dropping or recreating a blocked identity.
- Both children on ordinary Tanker release, existing geometry/pivots,
  deferred shot activation, independent bullets, zap-delayed stillborn pair,
  and accepted score attribution. Do not copy literal lost-second-child
  behavior without separate authorization.
- Fresh-Egg death/reentry, retained terminal outcomes/selected types/cargo,
  fixed death interval/freeze/retraction, persistent shortened Spike objects,
  and restoration of their shot-collision representation.
- Accepted Super Zapper charges, targeting/order/delays, shot-preemption and
  stillborn lifecycle. A ROM-style entry pause would be an explicitly scoped
  admission change, not a reason to rewrite zap.
- Flipper/Tanker/Spiker firing, projectile independence, existing shared
  enemy-shot budget and holdoff. No concurrency implementation should retune
  shooting, score, or RNG unrelated to selection.
- Persistent Spike growth/shooting/collision/exit behavior, including
  manually accepted prevention of endless final-Spiker recycling.
  [Development notes](../../TEMPEST-DEVELOPMENT-NOTES.md), lines 1919-1980,
  and [behavior spec](../../TEMPEST-BEHAVIOR-SPEC.md), lines 1061-1086,
  expressly record the same-ID retreat/reroll translation.
- All-rim checks must continue requiring no reserve/Egg/pending activation/
  latent cargo and exact represented unresolved identity sets.
  Normal completion still requires every unresolved obligation resolved.
  Neither admission denial nor cargo reservation is a kill.
- Natural Level 3 -> 4 -> 5 installation/carry/recharge/approach behavior,
  direct diagnostic isolation, and accepted Spike-avoid exit/retry behavior.
- Fixed simulation order, seeded randomness, recorded controller events,
  startup configuration and playback determinism. A different species picker
  intentionally changes trajectories; it must not silently corrupt recording
  format/startup or nondeterministically consume draws on waiting Eggs.

**ARCADE-OBSERVED, previously documented only:** two live shot-released
Flippers ending in distinct noncarrier lanes, accepted shortened-Spike
shootability after death, and accepted visible late-wave/Level 4 behavior.
These do NOT establish Atari's hidden ledger, concurrency maxima, or exact
pending cadence.

Tests were inspected, not run:
[Level4CandidateTests](../../tests/Tempest.Game.Tests/Level4CandidateTests.cs)
includes configuration (25), same-ID retreat/reentry (328, 564, 588), zap
(364), death (410), Tanker pair/fire (489), Spike exit/retry (626 onward), and
old picker contracts (997, 1015).
The latter explicitly assert existing source-shaped four-Spiker/late-wave
selection behavior; they are not independent Atari evidence.

## 9. Smallest recommended next milestone

**INFERENCE; not implemented:** Authorize a narrowly scoped, explicitly
diagnostic **Level 4 ordinary-admission plus terminal-Spiker lifecycle**
candidate before any production acceptance.

1. Reuse ready-Egg denial/wait and authoritative snapshots. Ordinary
   unselected entries choose only F/S with Level 4 minima/maxima and shared
   physical headroom. Do not apply Level 3's Tanker-first selector or its
   wrapping-cargo explanation unchanged.
2. Preserve reserve/Egg approach timing, geometry, firing, scoring, zap,
   completion, and accounting initially. Do not port the entire ROM pending
   scheduler or shortest-spike movement merely to obtain an admission gate.
3. Explicitly authorize a bounded exception to the accepted Level 4 retreat/
   reroll translation: when no generic pending entries remain, reuse the
   existing same-original-ID `ConvertActiveSpikerToTanker` helper at the
   current retreat boundary. Allocate the normal owned cargo pair exactly
   once, preserve original supply, and reuse Tanker/cargo systems.
   The boundary's C# height/timing is an **APPROXIMATION**, not literal ROM
   along-coordinate timing. Intermediate Spiker reserve/Egg recycling can
   remain an explicitly documented translation in this first diagnostic.
4. Do NOT enable `UsesGenericRomDeathReconstruction` wholesale to obtain that
   conversion: it also changes death behavior. Gate the Level 4 terminal
   conversion independently; Levels 1-3/5-8 must remain unchanged.
5. Resolve retained-Tanker reentry explicitly. A converted Tanker surviving
   death retains its C# selected type/cargo; rejecting it forever because
   ordinary Tanker max is zero would strand the wave. Preserve its identity
   under a documented physical-headroom-only reentry exception, or obtain
   separate authorization for a different reconstruction policy. The former
   is a project translation, not ROM ordinary Tanker admission.
6. Preserve complete-pair cargo at saturation under the already accepted
   engineering principle; no strict-seven-live-object claim if a preserved
   pair produces overflow. Block subsequent ordinary hatches until room
   returns. Do not add a blanket two-Tanker cap to all C# post-death states
   merely from the fresh-ROM-life inference.

**Why not a cap-only patch:** With literal F/S minima, a last Spiker that
returns to a new unselected Egg can simply be selected as Spiker again. The
current `<4` suppression avoids that through a different rule. Removing the
suppression without the terminal-conversion decision risks reviving the
specific previously rejected endless-final-Spiker behavior. Conversely,
keeping suppression forever while claiming literal Level 4 admission is
incorrect. This lifecycle choice must be visible in the authorization.

If Neal does not authorize that localized lifecycle exception yet, the safe
smaller step is observation-only diagnostic telemetry of live species,
pending generic supply, proposed admission and retreat/conversion decisions;
do not ship a half-correction that can strand accepted identities.

Future bounded validation should cover the six arithmetic cases above,
same-tick hatches/blocked RNG, zero ordinary Tankers, two-Spiker admission,
pending versus no-pending retreat, two concurrent conversions, exactly-once
cargo allocation, complete-pair overflow, retained Tanker/cargo reentry,
rim/transition/pending-zap counts, exact completion/all-rim sets, campaign/
direct/recorded startup, and finite-seed/tick determinism. Use targeted tests
before Neal's subsequent manual arcade comparison. No such tests or gameplay
were performed in this investigation.

## 10. Unknowns and limits

- No new arcade/MAME observation establishes actual frequency of two
  conversions, late seven-Flipper occupancy, full-array cargo loss, or
  difficulty/revision on Neal's comparison machine.
- New findings are listing-based, not independently rechecked against raw
  binaries in this task. Other revisions were not compared.
- No ROM-to-60-Hz timing or coordinate calibration was attempted; shortest-
  spike hopping, growth duration, spike height and exact pending cadence
  must not be called visually validated from this analysis.
- The literal conversion counter imbalance is visible in the inspected
  instructions, but its cabinet-visible consequences beyond the exhausted-
  pending path were not measured. No synthetic impossible-state search was
  used to infer extra ordinary species.
- The accepted stable-identity death and Spiker reserve translations differ
  from generic ROM reconstruction/live hopping. Their deliberate boundaries
  require explicit acceptance, not speculative broad redesign.

## 11. Validation and final state

**PASS:** Expected baseline and local upstream verified; starting index and
tracked worktree clean. Six bounded arithmetic states plus Hard supply were
evaluated directly without executing game code.

**PASS:** Final tracked and staged diffs are empty, status remains clean,
HEAD remains the expected baseline, and both requested AI-Logs records are
ignored. No production/test/gameplay documentation changes were made.

No game launch, manual gameplay, builds, tests, network lookup, raw-ROM access,
commit, or push occurred. No temporary analysis files were created.
The only persistent additions are this report and its paired prompt.

Neal and Copilot must review the evidence and authorize the next milestone
before implementation. No manual acceptance is claimed for a nonexistent
Level 4 correction.
