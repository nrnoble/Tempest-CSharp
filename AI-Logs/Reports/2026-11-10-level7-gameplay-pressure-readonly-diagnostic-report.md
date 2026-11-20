# Level 7 gameplay-pressure read-only diagnostic

Date: 2026-11-10

Status: **READ-ONLY DIAGNOSIS COMPLETE.** No production behavior is authorized
to change by this report.

## Scope and evidence

- The accepted baseline remains `8f1c29a7d3e6ef4150c8ab64264108c3c2a9547b`.
  The uncommitted Level 7 concurrency candidate was present at inspection and
  has not been changed.
- Neal's supplied direct arcade/MAME observation is that Level 7 is much
  easier than this C# candidate: death is extremely unlikely and little effort
  is needed in arcade/MAME, while C# death is plausible and demands moderate
  or greater effort. This is strong gameplay evidence, not a hidden-counter
  measurement.
- ROM claims below come only from the checked-in annotated Rev. 2 listing,
  [Tempest Rom Source code.md](../../Reference/arcade/Tempest%20Rom%20Source%20code.md).
  C# claims come from the current source, not historical candidate summaries.
  The comparison uses the accepted project documentation as evidence for
  intentional translations, not as ROM authority.
- No internet ROM/disassembly source was used. No tests, builds, gameplay,
  source edits, or durable behavior-document edits were performed.

## Executive diagnosis

1. **Confirmed material difference: C# can exceed ROM's seven live enemy
   places on a complete Tanker pair release.** If the tube is full with a
   Tanker among its seven enemies, ROM removes the carrier and allocates its
   children separately: one child fits, the second does not. C# preserves and
   materializes both, temporarily leaving eight live objects. If both remain
   unresolved at death, the extra child becomes an extra generic unresolved
   enemy obligation. This can recur after a later obligation becomes a Tanker.
2. **Death reconstruction itself conserves obligations in both systems.**
   A live Tanker with latent cargo is one generic unresolved enemy obligation;
   each unresolved materialized child is one. Species selection and cargo
   creation after reconstruction can increase future workload in both systems.
   The extra C# workload comes from children C# materializes where ROM has no
   physical slot, not from the one-child-per-obligation reconstruction rule.
3. **Strong candidate: C# reserve/Egg cadence may leave generic supply pending
   longer and expose more repeated Spiker cycles.** ROM seeds all pending entry
   records at wave setup and advances their per-entry delays on active passes
   when admission is available. C# releases reserve Eggs in 50-tick batches,
   updates Eggs every eight ticks, and advances at most two Eggs toward
   hatching. C# restarts a returned Spiker's building cycle on its current
   lane while supply remains; ROM hops the same live Spiker to another lane.
   Whether ROM supply normally reaches zero before a second visible cycle is
   exposed is not proven without a deterministic run or further manual record.
4. **Enemy-shot relative speed is a material mismatch candidate, but the
   arcade comparison is unresolved.** Per-update C# projectiles move faster
   relative to C# Tankers than the local ROM fixed-point ratios predict for
   ROM projectiles versus ROM Tankers. Neal's direct observation says arcade
   enemy shots move faster than their firing enemies, which conflicts with
   that ROM ratio. Do not tune from this contradiction; the listing does not
   establish wall-clock interrupt frequency, and the coordinate systems are
   not interchangeable.
5. Ordinary C# hatching itself is capped at seven active objects; it does not
   exceed the ROM's seven-place capacity. It does not, however, reproduce
   ROM's pending-entry timing or Super Zapper creation pause. Missing the
   Super Zapper gate can advance already-existing obligations into the field
   during the zap, but cannot create extra wave supply.

## Accounting model

Use these distinct quantities:

- `G`: generic unresolved supply not yet materialized (ROM pending entries;
  C# reserve/Egg originals).
- `O`: unresolved active original physical enemies, including ordinary
  Tankers/Spikers and enemies at the rim.
- `C`: unresolved materialized Tanker-cargo children, including a shot-cargo
  child waiting for its one-tick C# activation.
- `L`: latent cargo carried by a live Tanker. It is not a separate obligation.
- `U = G + O + C`: unresolved obligations at that instant. Bullets and
  persistent Spikes are not enemies in this accounting.

A Tanker initially represents one generic obligation and has two latent
Flippers. When its carrier is removed and `c` children are actually
materialized, the outstanding workload changes by `c - 1`. If both children
materialize, one generic obligation becomes two enemy objects (+1 eventual
work unit); this cargo creation happens without any death. Species selection
may therefore make cumulative enemies handled exceed the initial 20 generic
units even while obligation accounting remains correct.

Death changes representation, not the unresolved count:

```text
ROM: U_after = enemies_pending + enemies_in + enemies_top
C#:  U_after = unresolved originals + unresolved materialized cargo
```

In the C# generic-reconstruction path, unresolved original species selections
are cleared, latent cargo is removed with its unresolved carrier, and each
unresolved materialized cargo child is converted into one new unselected
original. The old cargo identity is terminally marked `Reconstructed`, so a
later death does not count it a second time. This matches the count model;
stable C# ledger identities are not literal ROM identities.

## Bounded repeated-death examples

These are deterministic accounting bounds, not claims about random species
choices or exact ordinary-wave reachability. `q` below counts obligations
before a conversion; a Tanker is one `O`, not three.

| Case | ROM | Current C# candidate | Future workload implication |
|---|---|---|---|
| **1. No death** | Starts with 20 Easy/Medium generic obligations (23 on Hard). Each Tanker release independently allocates one or two children; when full, the carrier's removal leaves exactly one child slot. | Starts with 20. Every successful pair release materializes two children, even if this exceeds seven live objects. | Both can create more materialized enemies than initial supply: each full pair release adds one net unit. C# adds one per pair; ROM adds one only when both children fit. |
| **2. One death; unresolved ordinary Tanker/Spiker, no materialized cargo** | With `G` pending, one live Tanker, one live Spiker, and two latent Tanker children: `U_before = G + 2`; after death `U_after = G + 2`. The latent pair is discarded, not counted. | Same `U_before = G + 2`; generic reconstruction resets both active species to unselected obligations and removes the Tanker's latent pair. | No death-time multiplication. Post-death selection first favors the unmet two-Spiker minimum, then the unmet one-Tanker minimum when available; afterward lane-Spike state and quotas determine selection. |
| **3. One death with `K` unresolved materialized cargo Flippers** | Each child that successfully occupied a slot contributes one obligation after death: `U_after = G + O + K`. A child that could not be allocated contributes none. | Each unresolved active/pending-activation child becomes one generic original: `U_after = G + O + K`. `K` includes any C# pair child that ROM could not fit. | Death preserves the existing `K`; later Tanker selection can turn one obligation into two children if both materialize. The systems differ only where C# had already materialized children beyond ROM capacity. |
| **4. Second death after reconstructed obligations become Tankers and release cargo** | If `t` obligations become carriers and carrier `i` yields `cᵢ ∈ {0,1,2}` allocated children, the count immediately before the second death is `q + Σ(cᵢ - 1)`; the second death preserves that count. | If those `t` carriers all release their complete pair, the count becomes `q + t`; the second death preserves it. | The repeated-death workload can grow in both systems from species-to-cargo transformation. When the same release occurs at full ROM occupancy, C# gains one additional obligation per such pair compared with ROM. Whether selector state permits `t` such carriers is dynamic; this is not a fixed `Flipper → Tanker → 2 Flippers` recipe. |

The supplied example of four Flippers, four Tankers, and three Spikers is not
used as a ROM-reachable state or a deterministic selector outcome. It
illustrates why materialized population and cargo identities can amplify the
count if the C# population has grown above ROM's physical cap.

## Ordinary admission, capacity, and cadence

### ROM-backed

- The Level 7 table supplies 20 Easy/Medium generic obligations. The Hard
  branch adds the table-derived one-eighth amount, giving 23. `max_enm = 6`
  is the highest slot index; allocation scans indices 0–6, so there are seven
  physical enemy places. Active-below-rim plus top/rim occupancy is checked
  against that boundary.
- Ordinary maxima/minima are Flipper 0/5, Tanker 1/2, and Spiker 2/4;
  Pulsars/Fuzzballs are excluded. The zero-minimum rule excludes ordinary
  Flipper admission. Unmet Spiker minimum is checked before Tanker minimum.
  Tanker cargo reduces Flipper availability but is not physical occupancy.
- Once both minima are met, the selector uses the current pending entry's
  lane Spike when Tanker and Spiker are both eligible. It uses `$FF` for a
  zero/depleted Spike and the `$CC` threshold. An open physical place can
  remain unused when neither ordinary Tanker nor Spiker is available and
  Flipper is the only positive availability.
- All 20 pending-entry records are initialized at wave setup. During each
  active `state_04` pass, `create_enemies` scans the pending entries; their
  visibility/delay values advance only when physical/species admission is
  available. A successful allocation decrements generic pending supply.
  Therefore ordinary admission, not merely an available place, controls when
  pending supply drains.
- `create_enemies` also suppresses pending-entry advancement while
  `zap_running != 0`.

### Current C# candidate

- `Level7WaveSelection` now has seven physical places, 0/5 Flipper, 1/2
  ordinary Tanker, and 2/4 Spiker limits. Its snapshot counts ordinary and
  cargo Flippers, pending shot-cargo activations, ordinary/converted Tankers,
  and Spikers; latent cargo affects Flipper availability but occupies no
  place. Zero-minimum Flippers are not ordinary candidates, and the pending
  Egg lane supplies the Spike threshold.
- Ordinary admissions cannot exceed seven live units. A selector dead end
  can leave a place unused just as in ROM. The full-pair cargo path is the
  exception and can push the live count above seven; the admission cap then
  prevents further ordinary hatches until headroom returns.
- C# starts with four Eggs and 16 reserve. The production ledger releases up
  to six reserve Eggs every 50 simulation ticks. Mixed Eggs update every
  eighth update; at most two can be approaching, and the approach rises by
  two C# height units per Egg movement update. Ready Eggs wait if no species
  or physical slot is available. A ready Egg can hatch on the same gameplay
  step its update makes it ready because hatching follows Egg updates.
- Thus matching a 20-unit supply does not establish matching visible
  concurrency or time-to-zero-pending. ROM ages all initialized entries in
  parallel when admission is open; C# batches reserve release and stages Eggs
  through roaming/approach. The C# cadence can leave pending generic supply
  alive longer, but exact elapsed wave timing depends on random approach,
  pending lanes, player kills, and physical headroom. It cannot alone raise
  the ordinary active count over seven.

Relevant current source: [Level7WaveSelection.cs](../../src/Tempest.Game/Levels/Level7WaveSelection.cs#L26),
[Level.cs](../../src/Tempest.Game/Levels/Level.cs#L199),
[Egg.cs](../../src/Tempest.Game/Gameplay/Egg.cs#L44), and
[TempestGame.cs](../../src/Tempest.Game/TempestGame.cs#L308). ROM addresses:
`$9233-$926e`, `$98a2-$999c`, and `$99a5-$9a82`.

## Tanker cargo and repeated-death amplification

**ROM:** A Tanker occupies one place and carries two latent Flippers without
occupying places. At release, the carrier is removed first and the common
physical allocator is called once per child. A full seven-object population
therefore becomes six, then can admit one child; the other child fails
allocation. If another place was already open, both children fit. Children
bypass ordinary species selection/maxima and do not consume generic pending
supply.

**C#:** `ReleaseTankerCargo` requires both cargo identities and materializes
the complete pair. On a full seven-object pre-release population, carrier
removal leaves six and the pair produces eight. The second child is a real
unresolved materialized Flipper, not latent cargo or a pending obligation.
Ordinary admission subsequently waits for a slot, but this does not remove
the eighth object.

**Smallest capacity example:** Before release, both systems have seven live
objects including one Tanker and six other enemies. ROM: `7 - 1 + 1 = 7`
live after the one available child allocation. C#: `7 - 1 + 2 = 8`. If all
children remain unresolved at death, ROM rebuilds one child obligation while
C# rebuilds two: a one-obligation difference. If a later C# generic
obligation is selected as a Tanker and again releases at full occupancy, the
same one-child delta can repeat, subject to selector quotas/headroom. Death
reconstruction conserves each system's then-current count; it does not cause
the delta by itself.

This is the supported multiplication mechanism. It is conditional on a Tanker
release when the ROM physical array has only the carrier's one newly freed
place. The source does not establish how often Neal's ordinary Level 7 run
reaches that state.

## Spiker lifecycle and pressure

**ROM:** Spiker p-code moves at the Spiker speed, grows the persistent Spike,
reverses near the far end, and at its return endpoint hops to another
Spiker-eligible lane in the same live slot. If generic pending supply remains,
it continues as the same Spiker. Only when `enemies_pending == 0` does that
same slot convert into a Flipper-carrying Tanker.

**C#:** A returned Spiker remains in the same live collection/ledger identity.
While pending generic originals remain it restarts its building cycle on the
same lane; it does not implement the ROM lane hop. At actual zero pending
supply, it converts in the same slot and adds latent Tanker cargo. Neither
path adds an obligation during a repeated cycle or conversion.

The zero-pending conversion is not an ordinary Tanker selection and bypasses
the ordinary Tanker maximum in both traces. It still uses the Spiker's one
existing physical place; its two cargo Flippers remain latent and add no
obligations until materialized.

The source can explain Neal's observation if ordinary ROM admission drains
the pending entries before another Spiker endpoint is reached. It does not
prove that timing. C#'s initial 50-tick batch releases and Egg approach
staging can prolong the pending period, making an extra local build/retreat
cycle more plausible. That can lengthen the wave and renew/retain hazards but
does not multiply enemy obligations. The current C# Spiker's early
growth-driven retreat is random, so a cycle can be much shorter than its
full-height path. Without a recorded deterministic comparison, the number of
visible cycles is **not established**.

The C# Level 7 Spiker speed is `6/48 + 2.5 + random*2`, i.e. 2.625 through
less than 4.625 C# height units per update; retreat uses 90% of that rate.
The ROM Level 7 speed table selects raw `$00`, which `crack_speed` converts to
signed `$F800` (-8.0 ROM-along units per movement operation). This is a
per-operation comparison, not a real-time conversion. ROM's `$00` Spiker
program performs a move operation and Spike/reversal check each active pass;
C# updates its Spiker once per `RunOneStep`.

## Enemy projectile speed and firing pressure

### Relative movement rates

ROM `state_04` calls `move_enemies`, then `move_shots`, then `enm_shoot` once
per gameplay pass. The local listing does not establish the interrupt/main
loop rate in wall-clock time, so no milliseconds or updates/second are
assigned.

The Level 7 parameter loader gives `enm_shotspd_lsb = $C0` and a Flipper
speed of `$D4 + 6*$FB = $B6` (the displayed level uses table level 7).
`crack_speed` shifts these to signed 8.8 values:

| ROM quantity | Table input | After `crack_speed` | Relative movement |
|---|---:|---:|---|
| Enemy projectile | `$C0` | `$FE00` = -2.0 along/pass | Once per `move_shots` pass |
| Flipper | `$B6` | `$FDB0` = -2.3125 along/move operation | Level 7 `$24`: moves on active passes except flip-start passes |
| Tanker | Same as Flipper | `$FDB0` = -2.3125 along/pass | `$07`: one move operation per active pass |
| Spiker | `$00` | `$F800` = -8.0 along/move operation | `$00` p-code moves once per active pass |

At the same ROM movement cadence, projectile/Tanker magnitude is `2/2.3125
= 0.865`; the source-coded projectile is slower per pass than the Tanker.
Flipper `$24` also suppresses along movement on flip-start passes, which
lowers its effective average relative to its per-operation value. Spiker
growth/retreat changes direction, so the ratio describes displacement
magnitude, not projectile/enemy closure.

The Easy/Hard branch adjusts the raw Flipper/Tanker speed: Level 7 `$B6`
becomes `$BF` on Easy (-2.03125) or `$AD` on Hard (-2.59375) before
`crack_speed`. The enemy-shot `$C0` and Spiker `$00` table values are not
modified by those branches. C# uses its fixed Level 7 values regardless of
difficulty, so these ROM ratios are the Medium comparison unless the cabinet
setting is known.

Current canonical C# Level 7 calls each movement once per 60-Hz `RunOneStep`:

| C# quantity | C# advance | Relative projectile/enemy movement |
|---|---:|---:|
| Enemy projectile | +3 height/update | — |
| Ordinary Flipper | +2 height on lead-in and each turning update; no climb on pivot-start update | Projectile is at least 1.5x Flipper's per-update along advance, higher after pivot-start steps |
| Tanker | +1.75 height/update | Projectile/Tanker = `3/1.75 = 1.714` |
| Spiker | +2.625 to <4.625 height/update; retreat is 90% | Projectile/Spiker during growth = 0.649 to <1.143 |

**Relative-speed classification:** C# projectiles are **relatively faster
than C# Tankers** by the code-derived per-update ratios, and faster than
ROM-coded projectiles relative to ROM Tankers (`1.714` versus `0.865`).
Flipper route cadence and the different movement representations prevent a
single exact Flipper comparison. Overall C#-versus-arcade/MAME speed fidelity
is **unresolved**: Neal's direct observation says arcade shots move faster
than their firing enemies, opposite the literal ROM projectile/Tanker ratio.
Direct visible observation governs gameplay; the static listing must not
override it. The source/model discrepancy and C#'s faster relative projectile
rate make this a pressure candidate, not an authorized tuning conclusion.

The raw bytes above are ROM fixed-point values, not C# height units. Only
within-system dimensionless ratios are compared. The project ROM-along
mapping is an approximation and does not establish an arcade-to-C# speed
conversion.

### Firing rate and density

- **Capacity:** ROM Level 7 `enm_shotmax` is index 2 on Medium, so slots
  0–2 give three active projectiles. Easy/Hard change this to two/four.
  C# Level 7 also permits three. The cabinet/MAME difficulty is not supplied
  in this task, and C# has no Easy/Hard switch.
- **Flippers:** ROM Level 7 holdoff is 62. C# uses 62 and the same
  per-active-shot thresholds (`$00,$E0,$F0,$FA,$FF`). The C# Flipper firing
  ceiling of height 343 maps approximately to ROM's `$30` eligibility
  boundary. No confirmed Flipper firing-density increase is established.
- **Tankers:** ROM's shared `enm_shoot` path uses the per-enemy delay and
  shared projectile slots. No separate decrementing Tanker-ammunition field
  appears in that firing path; `tanker_load` selects the cargo type, not shot
  ammunition. C# Tankers have randomized ammunition 1–2,
  initial delay 17–80 updates, 25% shot rolls after timer expiry, and 16-tick
  retry windows. C# also permits Tanker firing through height 380; the
  project's ROM-along map puts ROM's `$30` eligibility boundary near height
  343. The extra C# near-rim window could add an opportunity, but its limited
  ammunition/random timer can reduce opportunities. Net density is
  **unknown**, not a confirmed pressure increase.
- **Spikers:** ROM Spikers carry the shooter flag and use the shared
  per-pass firing routine. C# Spikers create a bullet only on a randomized
  growth-driven retreat, with a second random roll. This C# path is less
  frequent by construction and is not a cause of higher projectile density.
- Faster C# projectiles reach the player in fewer updates but spend fewer
  updates alive. The faster travel can reduce warning time; it does not by
  itself prove greater simultaneous projectile density.

### Super Zapper entry pause

ROM gates ordinary pending-entry advancement on `zap_running != 0`.
Current C# hatching does not check the Zapper state; its Zapper flash lasts
60 simulation ticks. Therefore a ready, admissible C# Egg can hatch during
the zap, while ROM holds pending entry. Those hatches are existing obligations
that would otherwise arrive later, not added supply, and ordinary hatching
still observes the seven-place cap. This is **likely minor for total
whole-wave workload**, but may change short-lived post-zap population/timing.
The number of ready Eggs during a real zap is not established.

## ROM-versus-C# evidence table

| Mechanism | ROM-backed behavior | Current C# candidate | Likely pressure consequence | Confidence / evidence class |
|---|---|---|---|---|
| Fresh generic supply | 20 Easy/Medium; 23 Hard | 20; no difficulty mode | No mismatch at base supply; MAME difficulty unknown | ROM-backed / C# source |
| Ordinary physical capacity | Seven places, indices 0–6 | Seven ordinary places | Same ordinary ceiling | ROM-backed / C# source |
| Egg/pending-entry cadence | All pending records seeded; delays advance per active pass when admission is available | Four initial Eggs; releases up to six every 50 ticks; Egg movement every 8 ticks; at most two approaching | Could leave supply pending longer and expose more Spiker cycles; not more ordinary simultaneous enemies | Strong candidate; timing depends on play/randomness |
| Ordinary species selection | Minima/maxima; no ordinary Flipper; pending-lane Spike threshold | Level7 policy models the same minima/maxima, no ordinary Flipper, pending lane, and latent-cargo availability | No supported general increase from ordinary selector | ROM-backed / C# source |
| Tanker cargo materialization | Carrier removed, children allocated separately; full tube fits one child | Complete pair retained, can exceed seven | +1 object/work obligation at a saturated release | **Confirmed material difference** |
| Death reconstruction | Pending + below-rim + rim enemies become generic obligations; latent cargo excluded | Unresolved originals reset; each materialized child becomes one generic original; latent pair discarded | No multiplication at death itself; later Tanker transformation increases workload in both | ROM-backed / C# source |
| Repeated-death workload | Generic selection can re-create Tankers and cargo; physical allocation may limit children | Same transformation, but guaranteed complete pairs include any C# overflow child | C# surplus can compound, conditional on saturated releases and later Tanker selection | Confirmed mechanism; frequency conditional |
| Spiker lifetime | Grow, retreat, hop in same place while pending; convert only at zero pending | Restarts local build on same lane while pending; converts at zero | Can extend/repeat hazards; no obligation multiplication; more cycles not proven | Explicit project translation; strong timing candidate |
| Enemy-shot relative speed | Shot -2.0/pass; Tanker -2.3125/pass; Flipper route omits start-flip moves; Spiker -8.0/op | Shot +3; Tanker +1.75; Flipper +2 except start; Spiker +2.625–<4.625 | C# shot is much faster relative to C# Tanker; possible shorter warning; ROM ratio conflicts with Neal's direct arcade observation | Source-backed ratio; arcade fidelity unresolved |
| Enemy-shot capacity/firing | Medium has 3 slots, holdoff 62; shared firing for shooter-flagged types | 3 slots, Flipper 62/thresholds; different Tanker/Spiker rules | No confirmed overall density increase; Tanker net is unknown, Spiker path is lower | Mixed evidence / unknown |
| Super Zapper admission pause | `zap_running` blocks ordinary entry progression | No exact hatch gate; 60-tick flash | May advance existing enemies into field during zap; no added total workload | Likely minor to total workload; timing impact unmeasured |

## Ranked findings

### 1. CONFIRMED MATERIAL DIFFERENCE

**Complete-pair release can exceed ROM capacity and then inflate future
workload.** ROM's allocator frees the carrier slot before independently
allocating its children; with seven occupied beforehand, only one fits. C#
requires both cargo identities and creates both children. If unresolved at
death, each is correctly converted to an obligation, so C# carries one more
than ROM for that release. This is the only quantified, direct explanation
found for a C# population/workload above the ROM bound. How often it occurs
in Neal's normal Level 7 play remains unmeasured.

### 2. STRONG CANDIDATE

**Egg/pending cadence plus Spiker restart behavior may extend C# pressure.**
ROM starts all entry delays together and can drain pending supply as capacity
opens. C# stages pending originals through batch release and two-at-a-time
approach. Its returned Spiker restarts a build on the same lane whenever
pending supply remains, rather than performing the ROM hop. These mechanisms
can keep a Spiker active longer or expose an extra build/retreat cycle.
Neither mechanism creates extra obligations; the ordinary population cap is
still seven. The task's arcade observation (one visible trip then Tanker)
supports testing this mismatch, but does not prove how often it happens.

### 3. POSSIBLE / NEEDS MANUAL OR DETERMINISTIC EVIDENCE

- **Projectile-relative speed:** C# is 1.714x its Tanker's per-update height
  rate; the ROM listing gives 0.865x its Tanker's per-pass along rate. The
  C# projectile therefore has less relative warning time under the
  source-derived comparison, but the ROM result conflicts with Neal's
  direct observation that arcade shots are faster than their firing
  enemies. Overall arcade fidelity cannot be inferred until the observable
  speed relationship is reconciled.
- **Tanker near-rim shots:** C# keeps Tanker firing eligible to height 380
  versus ROM's `$30` boundary, approximately height 343 under the project's
  mapping. This adds opportunity, but ammo/timer limits may offset it.
- **Super Zapper pause:** C# may hatch ready existing obligations while ROM
  pauses; likely a timing difference, not added total workload.

### 4. NOT SUPPORTED AS A CURRENT CAUSE

- **Death reconstruction alone:** Both translations conserve current
  unresolved quantity. A live latent-cargo Tanker is one obligation, and
  each materialized child is one. Generic re-selection can later create
  cargo, but this is species transformation/cargo creation rather than
  duplication at death.
- **Ordinary C# hatching exceeding seven active objects:** The candidate's
  ordinary policy counts physical objects and waits for headroom. The
  observed code path that exceeds seven is complete-pair cargo release.
- **More C# Spiker firing:** The current C# Spiker only emits a bullet on a
  randomized growth-driven retreat, unlike ROM's shared shooter path. This
  does not support higher C# projectile density.
- **Higher C# base supply:** Both use 20 at Easy/Medium. Hard ROM supply is
  23, but the MAME difficulty used for Neal's comparison is unknown; no
  supply-based attribution is made.

## Recommended next correction

**Next smallest safe milestone: make only canonical Level 7 Tanker releases
respect ROM's independent physical child allocation.** When the carrier is
removed and only one of the seven places is free, materialize one child and
do not create a generic obligation for the unallocated ROM child. Keep
complete-pair behavior unchanged for accepted Levels 1–6 and for Level 7
releases with room for both children. Do not change generic death
reconstruction; it already preserves each actually materialized unresolved
child once.

This is the narrowest correction with a direct, quantified population effect.
After it, re-measure whole-wave effort before changing Egg cadence, Spiker
behavior, projectile speed, or the deferred Super Zapper gate. Preserve the
accepted Level 7 movement route and do not begin Level 8 work.

## Future manual acceptance target

After a future authorized correction, compare complete Level 7 waves against
Neal's benchmark: arcade/MAME death remains extremely unlikely with little
effort, C# whole-wave pressure is comparable, and a death does not cause
obvious escalating workload. Preserve Levels 1–6 and the currently frozen
Level 7 movement.

Precise checks:

1. With seven live objects including a Tanker, observe its release: C# should
   not show eight live enemy objects when only one physical place is free.
2. In a crowded Level 7 death after such a release, compare unresolved
   materialized children before/after reconstruction; each surviving child
   should return as one generic unresolved enemy obligation, with no
   obligation for a child that never materialized.
3. Repeat after reconstructed obligations produce a later Tanker; check that
   the workload does not gain a phantom second child at a full tube.

## Validation and integrity

- Starting `HEAD`: `8f1c29a7d3e6ef4150c8ab64264108c3c2a9547b`.
- The existing candidate files were SHA-256 recorded before report creation;
  the same hashes were checked afterward. No candidate file changed.
- The Git index tree hash was recorded and rechecked unchanged.
- `git diff --check` passes.
- No production code, tests, durable behavior documents, local ROM/reference
  material, or user assets were edited. Only the requested prompt and report
  records were added. No broad tests/builds, interactive game, commit, push,
  Level 8 work, or Level 7 movement reconciliation was performed.
