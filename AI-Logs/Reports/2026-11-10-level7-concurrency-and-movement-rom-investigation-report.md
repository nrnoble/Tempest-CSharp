# Level 7 concurrency and movement ROM investigation

Date: 2026-11-10

Status: **READ-ONLY INVESTIGATION COMPLETE.** No production behavior is
authorized to change by this report.

## 1. Scope, baseline, and evidence

- Required baseline and verified starting `HEAD`:
  `8f1c29a7d3e6ef4150c8ab64264108c3c2a9547b`
  (`Implement accepted Level 6 concurrency and cargo movement`).
- The index and tracked worktree were clean. Initial `git diff --check` passed.
- This investigation used the checked-in annotated Rev. 2 ROM listing, current
  canonical C# source/tests, accepted Level 7 reports, and durable behavior
  documents. It did not inspect new ROM binaries or make new arcade
  observations. “ROM-BACKED” means a static trace of that listing.
- The prior [Level 1-7 supply report](2026-11-08-level1-7-enemy-supply-rom-investigation-report.md)
  supplies useful table values and current-C# context. The prior
  [Level 7 Flipper ROM report](2026-11-04-level7-flipper-rom-analysis-report.md)
  and [Level 7 acceptance closeout](2026-11-04-level7-acceptance-closeout-report.md)
  establish accepted project behavior and historical observations.
- **Evidence correction:** older Level 7 prose describes `max_enm=6` as a
  six-active-enemy cap and sometimes calls the selector's Spike test a
  shortest-Spike scan. The shared allocator trace and seven-entry enemy arrays
  show that `6` is the highest slot index (seven physical slots), and the
  post-minimum comparison reads the pending entry's lane Spike. This report
  uses the source trace; it does not edit or reopen accepted gameplay
  documentation.

## 2. Executive findings

1. **ROM-BACKED:** displayed Level 7 has 20 Easy/Medium generic pending
   obligations and 23 on Hard. It has Flipper 0/5, Tanker 1/2, Spiker 2/4,
   Pulsar 0/0, and Fuseball/Fuzzball 0/0 minima/maxima. Tanker loads are pairs
   of Flippers. No fixed final species recipe is implied.
2. **ROM-BACKED:** the shared enemy array has seven physical places, indices
   0 through 6. Ordinary creation requires both species/cargo availability
   and physical headroom. It can leave one place unused when only positive
   availability is for zero-minimum Flippers.
3. **ROM-BACKED:** ordinary Flippers are not selectable because their minimum
   is zero. Unmet Spiker minimum precedes Tanker minimum. Once minima are met,
   the selector compares the pending Egg's lane Spike against `$cc` when both
   Tanker and Spiker are available; it is not a global shortest-Spike scan.
4. **ROM-BACKED:** Tanker cargo is latent while carried and does not occupy
   separate places. A Tanker consumes one generic pending unit. On release,
   the carrier's place is freed before two separate child-allocation attempts;
   the children bypass ordinary species selection/maxima and do not decrement
   pending supply. At a full seven-enemy occupancy, only one of the two
   children can be allocated after the carrier is removed.
5. **ROM-BACKED:** the shared Spiker program grows/retreats/hops in its place.
   At actual `enemies_pending == 0`, a retreating Spiker can become a
   Flipper-carrying Tanker in that same place. A selector dead end while
   pending supply remains is not the terminal condition.
6. **ROM-BACKED:** death rebuilds generic supply as
   `enemies_in + enemies_top + enemies_pending`, capped at `$3f`. A carrier
   counts once, latent cargo does not add units, and each materialized child
   counts once. Species and prior cargo linkage are not restored.
7. **PROJECT-CURRENT:** Level 7 already has a ROM-informed selector,
   no-ordinary-Flipper rule, generic death reconstruction, and zero-pending
   same-slot Spiker conversion. It uses a six-object selector cap and random
   post-minimum choice, not the ROM's seven places and pending-lane Spike
   decision. C# preserves complete cargo pairs as an intentional translation.
8. **ROM-BACKED:** ordinary Level 7 Flippers use p-code `$24`: one along-tube
   move, two flips in one direction, reverse, three flips, reverse, repeat.
   They move along the tube during active flip updates, but not on the
   flip-start update. Tanker cargo uses `$07` near the rim and `$24` at or
   deeper than ROM along `$20`.
9. **ARCADE-OBSERVED / PROJECT-CURRENT:** Neal's accepted visible ordinary
   route is `2-3-2-2-2`, not the ROM `$24` repeating 2/3 sequence. This is a
   documented evidence conflict, not permission to replace accepted movement.
   Only deep player-shot C# cargo uses its accepted programmed route; natural
   and near-rim C# cargo retain the existing release-pivot/rim path.
10. A full Level 7 concurrency assessment is not admission-only: cargo,
    terminal Spikers, and life-loss reconstruction affect population. Keep
    any future movement correction separate from concurrency until the
    observed-vs-ROM route conflict is explicitly resolved.

## 3. Displayed Level 7 ROM parameter table

`curlevel=6` is displayed Level 7; the parameter loader selects effective
table level 7. Table entries are in the [ROM parameter tables](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L2702)
and the shared [parameter loader](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L2547).

| Parameter | Displayed Level 7 | Meaning / qualification |
|---|---:|---|
| Easy/Medium generic pending supply | 20 (`$14`) | Generic obligations, not a fixed species distribution |
| Hard generic pending supply | 23 | Hard branch adds the table-derived one-eighth adjustment |
| Flipper minimum / maximum | 0 / 5 | Positive max-derived availability does not make zero-minimum Flippers selectable |
| Tanker minimum / maximum | 1 / 2 | Ordinary selection thresholds; not a hard cap on later derived Tankers |
| Spiker minimum / maximum | 2 / 4 | Ordinary selection thresholds |
| Pulsar minimum / maximum | 0 / 0 | Not ordinarily available on Level 7 |
| Fuseball (ROM Fuzzball) minimum / maximum | 0 / 0 | Not ordinarily available on Level 7 |
| Tanker cargo | Flipper type 1; two children | All four load alternatives are Flipper pairs |
| Physical enemy capacity | 7 | `max_enm=6` is highest slot index; array indices are 0-6 |
| Enemy-shot capacity | 2 on Easy; 3 on Medium; 4 on Hard | `enm_shotmax` stores highest index; Easy decrements it and Hard increments it |
| Initial Spike parameter | `$d0` | Raw ROM value; C# uses 16 height-60 Spikes without an established coordinate equivalence |
| Flipper movement entry | `$24` | Level's p-code entry; decoded in section 9 |
| Ordinary Tanker movement | `$07` | Straight along-tube p-code |
| Level 7 Flipper shot holdoff | 62 | Separate from enemy-shot capacity |

The annotated source records the type tables, shot maxima, supply, enemy
capacity and `$d0` Spike data at [the Level tables](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L2702).
The Easy branch decrements and the Hard branch increments `enm_shotmax` at
[the difficulty adjustment](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L2615).
This is the table's Level 7 parameter value, not a claim about the difficulty
setting used in any past arcade observation.

## 4. Ordinary admission and representative states

The common `create_enemies` path blocks ordinary admission while active
below-rim plus top occupancy exceeds `max_enm`, or while `zap_running` is
nonzero. With `max_enm=6`, the condition allows seven indexed enemy places
(0-6). Allocation scans those seven slots. Pending supply is decremented only
after a successful enemy allocation. See [creation gate and allocator](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3217).

For active counts `F` (materialized Flippers), `T` (Flipper-carrying Tankers),
and `S` (Spikers), the Level 7 selector's relevant counts are:

```text
physical headroom H = max(0, 7 - F - T - S)
Flipper availability = min(H, byte-clamp(max(5 - F, 0) - 2*T))
Tanker availability  = min(H, max(2 - T, 0))
Spiker availability  = min(H, max(4 - S, 0))
```

The Flipper byte subtraction can wrap before availability is bounded by
physical headroom. Tankers reserve two Flipper-availability units apiece, but
do not reserve two physical enemy places when selected. The Tanker path also
requires positive availability for its cargo type. These are selector
availabilities, not guarantees of the final counts. The implementation is
visible in [availability calculation and selection](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3333).

Selection order and restrictions:

- The selector checks available species minima in descending species index;
  Spiker's minimum of 2 is tested before Tanker's minimum of 1.
- A lone available type is tried only if its minimum is nonzero. The same
  nonzero-minimum gate applies when scanning unsatisfied minima and in the
  fallback. Thus ordinary Flippers (minimum 0), Pulsars, and Fuseballs cannot
  be selected, even if an availability byte for them is positive.
- If both Tanker and Spiker are available after minima are satisfied, the
  selector reads `spike_ht` for the *current pending entry's segment*.
  A zero Spike value is treated as `$ff`; values at/above `$cc` select
  Spiker, values below `$cc` select Tanker. If that path does not select,
  the fallback starts at a `pokey2_rand`-derived type and tries types with
  nonzero minima and availability. This is not a fixed recipe or a global
  shortest-Spike scan.
- A successful Tanker consumes one pending entry, not three. Its two future
  cargo Flippers are not new generic pending supply.

Bounded arithmetic examples (not a simulation or a fixed wave recipe):

| F / T / S | Free places | F / T / S availability after physical bound | Result |
|---|---:|---:|---|
| 0 / 0 / 0 | 7 | 5 / 2 / 4 | Spiker minimum unmet; select Spiker |
| 0 / 0 / 2 | 5 | 5 / 2 / 2 | Spiker minimum met; Tanker minimum unmet; select Tanker |
| 0 / 1 / 2 | 4 | 3 / 1 / 2 | Both minima met; if both available, pending-lane Spike threshold chooses |
| 0 / 2 / 4 | 1 | 1 / 0 / 0 | Only zero-minimum Flipper availability remains; admission stalls with one place open |

The final state demonstrates a real pre-zero-supply stall: six active
Tankers/Spikers leave one physical place, but neither ordinary species is
available and Flipper minimum zero blocks the remaining type. The pending
count does not become zero merely because no type is presently selectable.
Admission can resume after an actual removal changes species counts or opens
headroom, or after a zap gate clears. Rim transfer alone does not release a
place.

## 5. Tanker and cargo concurrency

**ROM-BACKED**

- An ordinary Level 7 Tanker is selectable after its minimum is due and
  while its maximum/cargo availability permit. It consumes one generic
  pending unit and one physical slot. Two latent Flippers occupy no slots.
- The Tanker cargo type is Flipper. Its latent pair reduces Flipper
  availability by two per active Tanker, but a Tanker does not reserve
  physical child slots or require two pending units.
- At the `$20` release boundary, the carrier slot and type count are cleared;
  ROM then calls the physical allocator once for each child. Both children
  bypass ordinary selection and do not decrement `enemies_pending`. If all
  seven slots were occupied before release, removal of the carrier opens one
  slot: the first child can use it and the second allocation fails. If there
  was another free slot, both may be allocated. No all-or-nothing pair
  guarantee exists in this literal ROM path.
- Released children can exceed ordinary Flipper maximum 5. Likewise, a
  no-pending Spiker conversion can create a Tanker outside ordinary maximum
  2; it reuses the Spiker's slot and adds latent cargo, not a new active slot.

The child attempts and direction/segment setup are in the
[ROM split routine](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L4178).
Allocation failure and slot search are in the
[shared allocator](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3274).

**PROJECT TRANSLATION TO PRESERVE:** canonical C# gives each Tanker two
stable cargo identities and insists on complete-pair release. This is
deliberate project accounting/behavior, not literal ROM independent
allocation. Pair release can exceed the C# ordinary cap and can produce more
than seven live C# enemy objects. Do not silently change it in a Level 7
diagnostic.

## 6. Spiker, Pulsar/Fuseball, and physical occupancy

The ROM uses the shared Spiker p-code: grow the persistent Spike, retreat,
hop to a shortest-Spike lane, and continue in the same live enemy slot.
Growth, retreat, and hop do not release capacity. While generic pending
supply is nonzero, the Spiker continues this lifecycle. At actual
`enemies_pending == 0`, the conversion branch rewrites that same slot as a
Flipper-carrying Tanker and resets its along position; this is not an ordinary
Tanker selection or a two-Flipper merge. Source: [Spiker p-code/conversion](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L4243)
and [zero-pending conversion](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L4132).

A species dead end with `enemies_pending > 0` is distinct: no conversion
occurs yet, even if the selector cannot use its remaining physical headroom.
The eventual conversion depends on pending supply actually reaching zero.

| ROM entity/state | Uses one of seven enemy places? |
|---|---|
| Flipper, Tanker, Spiker; climbing, flipping, pause, transition, retreat, or hop | Yes, one |
| Enemy transferred to the rim / `enemies_top` | Yes, until removed |
| Pending-zap target | Yes, until zap removal |
| Spiker converted to Tanker | Same existing place |
| Tanker with latent cargo | Tanker uses one; latent children use none |
| Each successfully materialized cargo child | Yes, one each |
| Pending generic entry / its approach display | No active enemy place until allocation |
| Pulsar and Fuseball | Not Level 7 ordinary selections |
| Enemy bullet or persistent Spike | No; separate projectile/hazard storage |

Level 7 has no ordinary Pulsar/Fuseball lifecycle to add to this analysis;
their zero minima/maxima exclude them. Enemy shots are separately capped
(two on Easy, three on Medium, four on Hard) and do not consume enemy places.

## 7. Death and generic obligation reconstruction

After the death cleanup conditions, the ROM sets generic pending supply to:

```text
min(0x3f, enemies_in + enemies_top + enemies_pending)
```

The source is [death reconstruction](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L4886).
This counts every unresolved active slot, including rim/top enemies, plus
already pending generic entries:

- An ordinary active Flipper, Tanker, Spiker, or derived Tanker contributes
  one.
- A Tanker carrying two latent Flippers contributes one, not three.
- Each materialized cargo child still alive contributes one separately.
- A rim enemy that is still in `enemies_top` contributes one.
- Active species choices, Tanker ownership, and movement phase are discarded;
  the rebuilt entries use ordinary Level 7 selection again.
- Pending generic entries are added once through `enemies_pending`; latent
  cargo and persistent Spikes are not added as generic pending units.

A one-unit re-entry with cleared active species first selects Spiker because
its minimum is the first unmet positive minimum. If the eventual final
pending Spiker reaches the conversion branch, that same unit becomes a
Flipper-carrying Tanker.

**CURRENT C# Level 7:** `UsesGenericRomDeathReconstruction` is enabled. On
death, C# clears active enemy objects, resets unresolved original species to
unselected, removes latent cargo, and converts each unresolved materialized
cargo child into one generic re-entry obligation. Resolved outcomes stay
resolved. Still-active rim enemies remain unresolved in the ledger and are
rebuilt; a completed all-rim exit has already left gameplay. Re-entering
Level 7 uses the same dedicated selector, so a one-unit fresh state selects a
Spiker. The model preserves stable project identity/accounting; it does not
claim the ROM has an identity ledger. See [Level reset](../../src/Tempest.Game/Levels/Level.cs#L150),
[ledger reconstruction](../../src/Tempest.Game/Levels/Level3WaveLedger.cs#L262),
[death reset](../../src/Tempest.Game/TempestGame.cs#L530), and
[existing Level 7 tests](../../tests/Tempest.Game.Tests/Level7ProductionTests.cs#L70).

The broad quantity translation is already present and is closer to the ROM
than the historical pre-correction Level 7 implementation. The exact
one-enemy death/re-entry visual case was not manually reproduced in the
accepted Level 7 closeout. Neal's later manual checks should distinguish an
active Tanker with latent cargo from two materialized children and include
still-live rim enemies before death.

## 8. Super Zapper entry boundary

**ROM-BACKED:** `create_enemies` pauses ordinary pending-entry processing
while `zap_running != 0`; active targets still occupy places until removed.
See the [shared admission gate](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3217).

**PROJECT-CURRENT:** Level 7's C# Egg/hatch selector has no corresponding
Super-Zapper-active admission pause. This is a confirmed difference, not a
reason to redesign Super Zapper. The Levels 5/6 reports explicitly defer this
fidelity boundary; leave it separately authorized and unchanged here.

## 9. Level 7 Flipper movement program

### Ordinary Level 7 Flippers

**ROM-BACKED:** The table's p-code is `$24` for displayed Level 7. The
interpreter executes one movement program per enemy each gameplay pass and
stores its program counter per enemy. The program and interpreter are at
[the p-code table/interpreter](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3565)
and [entry `$24`](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L4243).

Decoded behavior:

1. At entry, perform one along-tube movement operation and halt that
   invocation.
2. Start a two-flip counter. Each flip-start operation halts that invocation.
   While a flip is active, each continuation advances the turn and moves
   along the tube; the completing invocation also performs its along move.
3. After the second completed flip, reverse lane direction and start the first
   of three opposite-direction flips. After those three complete, reverse
   again, branch to entry, perform the cycle's one along operation, then
   begin the next two-flip block.
4. The repeating route is therefore **2 toward decreasing ROM segment
   indices, reverse, 3 toward increasing indices, reverse, repeat**. Direction
   changes are scripted, not random or player-relative. Closed Level 7 clover
   has no open-end edge correction.
5. The along tube advances during active flips, but not on the invocation
   that starts a flip. There is one extra along operation at initial entry and
   at each five-flip repeat boundary. Rim arrival hands off to the separate
   top program at `$42`; a current flip is completed before handoff.

For initial ROM segment `S`, the first ten completed lane destinations are:

```text
S-1, S-2, S-1, S, S+1, S, S-1, S, S+1, S+2   (modulo 16)
```

The ROM gives an ordinary Flipper a fixed clear direction bit, which means
decreasing ROM segment indices. Translating that bit to C# lane handedness is
not the same as proving a screen-space direction or numeric timing.

### Tanker cargo movement

The ROM's cargo parameter selector uses the current level's `flipper_move`
when Tanker release along is at or beyond `$20`; below `$20` it selects the
generic `$07` straight-move entry. See [depth-dependent movement selection](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3542)
and [default `$07` / Level `$24` p-code](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L4243).

- Deep Level 7 player-shot release uses `$24`. The two children are placed on
  opposite adjacent segments and begin with opposite directions; each starts
  a fresh program phase rather than inheriting the Tanker's route state.
- Natural release occurs near the rim, below ROM along `$20`, and uses `$07`:
  straight to the rim, not the ordinary Flipper's `$24` route.
- This is a depth-based distinction on Level 7 too. The distinction is
  ROM-coordinate based; it does not establish a C# height threshold.

## 10. ROM versus canonical C# Level 7

| Area | ROM-backed Level 7 | Current canonical C# | Classification |
|---|---|---|---|
| Supply | 20 Easy/Medium; 23 Hard | Fixed 20, four initial Eggs and 16 reserve; no Hard setting | ROM table / PROJECT-CURRENT |
| Ordinary species | Flipper 0/5, Tanker 1/2, Spiker 2/4; Pulsar/Fuseball 0/0 | Dedicated selector excludes ordinary Flippers; caps Tanker 2/Spiker 4; excludes Pulsar/Fuseball | Largely aligned, not exact selector |
| Admission minima/order | Spiker min first, then Tanker; zero-minimum species skipped | Spiker min 2 then Tanker min 1; same broad intent | PROJECT-CURRENT approximation |
| Post-minimum choice | Pending-lane Spike threshold `$cc`, then random-start eligible fallback | Random choice between available Tanker/Spiker; no pending-lane Spike input | Confirmed mismatch |
| Physical capacity | Seven slots, indices 0-6 | `Level7WaveSelection.MaximumActiveEnemies = 6` | Confirmed mismatch; ordinary C# gate is one lower |
| Tanker cargo availability | Two latent Flippers subtract from Flipper availability | Ledger owns two latent cargo identities; Level 7 selector does not account for that availability | Different mechanics |
| Cargo release | Independently allocate up to two children after carrier removal; may truncate at capacity | Accepted complete-pair translation; may exceed ordinary/physical cap | PROJECT TRANSLATION; preserve |
| Spiker with pending supply | Grows/retreats/hops in same slot | C# Spiker returns to reserve at its retreat endpoint and can re-hatch | Confirmed lifecycle difference |
| Zero-pending Spiker | Converts same slot into Flipper-carrying Tanker | Converts same original identity to Tanker when generic pending is zero; C# height-zero boundary is approximate | Broadly aligned; timing/coordinates differ |
| Death reconstruction | Active below-rim + top + pending, species-free | Generic re-entry; unresolved species cleared, latent cargo dropped, each materialized child becomes one generic unit | Broadly aligned translation; stable IDs are project-only |
| Super Zapper entry | `zap_running` pauses ordinary entry | No matching hatch pause | Confirmed deferred mismatch |
| Ordinary Flipper route | `$24`: 2/reverse/3/reverse repeat | Arcade-observed `2-3-2-2-2`, per-Flipper counters; fixed `+1`, 9-degree turns, 2 height/update | Accepted visible route conflicts with ROM route |
| Cargo movement | Deep `$24`; near-rim/natural `$07`; opposite initial directions | Deep player-shot only (C# `Height <= 370`) uses accepted programmed route; natural/near-rim use existing pivot/rim behavior | Partial project translation; cutoff/near-rim differ |
| Enemy-shot capacity | 2 Easy, 3 Medium, 4 Hard | 3 | Matches Medium only; no Easy/Hard setting |
| Initial Spikes | `$d0` | 16 at C# height 60 | Counts align; coordinate mapping unknown |

The relevant current implementations are [Level7WaveSelection](../../src/Tempest.Game/Levels/Level7WaveSelection.cs#L3),
[Level 7 configuration](../../src/Tempest.Game/Levels/LevelConfiguration.cs#L287),
[Flipper route](../../src/Tempest.Game/Gameplay/Flipper.cs#L43),
[Tanker cargo threshold](../../src/Tempest.Game/Gameplay/Tanker.cs#L136),
and [gameplay selection/cargo/conversion](../../src/Tempest.Game/TempestGame.cs#L893).

**Current Level 7 concurrency details:** C# uses four starting Eggs and 16
reserve, releases ledger-backed Eggs on its project cadence, and limits
ordinary selection to six active units. Its max-6 selector can pause ordinary
admission, while the accepted complete-pair release can go beyond that limit.
The C# picker has the correct zero-ordinary-Flipper and Tanker/Spiker cap
shape but lacks ROM's seventh physical place, latent-cargo availability
calculation, and pending-lane Spike rule. Its generic death and final-pending
conversion are already implemented. Its Spiker retreat-with-pending path,
Super Zapper admission gate, and `2-3-2-2-2` movement remain behaviorally
distinct from the static ROM trace.

## 11. Accepted Level 7 behavior to protect

The [accepted closeout](2026-11-04-level7-acceptance-closeout-report.md),
[durable behavior specification](../../TEMPEST-BEHAVIOR-SPEC.md#L170),
and [development notes](../../TEMPEST-DEVELOPMENT-NOTES.md#L3116) record:

- closed 16-lane clover geometry and natural Level 6-to-7 progression;
- 20-original C# wave, four initial Eggs, 16 reserve, 16 starting Spikes;
- no ordinary individual bottom Flippers; Tankers enter and release Flippers;
- Neal's accepted visible ordinary route `2-3-2-2-2`;
- separate observed deep player-shot cargo behavior, with opposite siblings;
- accepted firing, scoring, Spike/Spiker, Super Zapper, death/re-entry,
  rim-combat, completion, progression and recording/playback behavior;
- fixed C# movement handedness/cadence and `$370` cargo cutoff remain
  approximations, not arcade-timing/coordinate claims.

The Level 7 implementation was perceptually accepted, but the exact
one-enemy death/re-entry visual case was not manually reproduced. Existing
acceptance is not evidence that the C# route is byte-for-byte ROM movement.
Do not change the accepted `2-3-2-2-2` route merely to match `$24` without
reconciling Neal's direct observation with this ROM trace.

## 12. Concrete future manual checks

Only the following behaviors are established strongly enough to make these
checks specific; they do not assert hidden counters are directly observable.

### Concurrency and death

1. Record cabinet difficulty first. Compare 20 base obligations against 23
   Hard obligations; separately count ordinary entrants, active enemies,
   Tankers, and released cargo.
2. On fresh Level 7, verify no ordinary individual Flipper enters from the
   bottom, while Tankers and Spikers do. Track whether the initial unmet
   species order visibly favors Spikers; do not infer a fixed total recipe.
3. Observe a saturated tube and a species-max dead end. In particular,
   determine whether a pending obligation can remain while a physical place
   is visibly unused. Record Tanker/Spiker counts and whether any Flipper is
   an ordinary entrant or released cargo.
4. Release Tanker cargo with limited room: compare a full seven-place
   occupancy before release with one pre-existing open place. ROM predicts
   carrier removal followed by two independent child allocations; a full
   pre-release tube can materialize only one child, whereas another open
   place permits the pair. Keep this check separate from C#'s accepted
   complete-pair translation.
5. Observe Spiker growth/retreat/hop while generic supply remains, then the
   final-pending conversion. Check that temporary lack of an eligible type
   does not itself trigger conversion and that the terminal Tanker occupies
   the Spiker's former place.
6. Compare death/re-entry after (a) ordinary active enemies, including an
   at-rim enemy, (b) a Tanker with latent cargo, and (c) a Tanker whose
   children have materialized. Compare only unresolved quantity and
   subsequent species behavior; ROM does not preserve old species/cargo
   identity.
7. The ROM admission pause during Super Zapper remains a separately deferred
   observation/authorization boundary; do not redesign Zapper behavior here.

### Movement

1. On the closed clover, record the first ten ordinary Flipper lane
   destinations and direction changes, starting from the initial lane:
   ROM predicts `S-1,S-2,S-1,S,S+1,S,S-1,S,S+1,S+2` (mod 16), i.e. two one
   way, three the other, then repeat. Compare that directly with the accepted
   C#-observed `2-3-2-2-2` sequence rather than treating them as equivalent.
2. Note whether each active pivot climbs along the tube and whether a
   flip-start update has no along movement; ROM `$24` predicts climb during
   active flip continuation and no climb on the start operation.
3. Watch rim handoff separately: the below-rim phase yields to the top/rim
   program, with an in-progress flip completed first.
4. Observe Tanker cargo by release depth and cause. Deep player-shot children
   should begin on opposite sides/directions and use the `$24` pattern;
   natural/near-rim release should use straight `$07` movement. Do not infer
   that C# height 370 is the ROM `$20` coordinate.

## 13. Recommended next milestone

**Smallest safe next step: a bounded Level 7 concurrency diagnostic, followed
by any authorized correction; keep movement separate.** Reuse the already
understood shared ROM allocator/slot/death machinery rather than reverse
engineering it again, but keep Level 7's table values, cargo type, pending
lane Spike rule, Spiker terminal condition, and movement entry explicit.
Cover admission plus cargo, Spiker lifecycle, and death reconstruction;
admission-only validation would miss population-changing lifecycle paths.

Current Level 7 generic death reconstruction and same-slot terminal
conversion are already close at the quantity/identity-translation level, so
verify rather than rewrite them. Treat the six-vs-seven capacity, pending
lane selection, Spiker return-to-reserve behavior, complete-pair cargo
exception, and zap pause as individually scoped differences. Preserve the
accepted complete-pair cargo translation and Super Zapper boundary unless
Neal separately authorizes a behavior change.

Do not combine a concurrency candidate with a Flipper movement correction.
The movement trace is clear ROM evidence, but it conflicts with a direct,
already-accepted arcade observation. Resolve that evidence conflict with a
targeted arcade observation first; retain current accepted movement until
then.

## 14. Unknowns, validation, and final state

Unknowns and limits:

- No new arcade/MAME observation, ROM-binary verification, or numeric
  ROM-to-C# along/depth calibration was performed.
- `$d0` does not prove equivalence to C# height 60; ROM `$20` does not prove
  equivalence to C# height 370.
- The ROM movement program's lane pattern is statically decoded, but exact
  visual timing depends on the p-code movement/geometry and has not been
  timed against arcade video in this task.
- Previously accepted Level 7 visible movement (`2-3-2-2-2`) and ROM `$24`
  (`2/reverse/3/reverse/repeat`) remain in explicit tension.
- The source trace does not prove the observed cabinet's difficulty or the
  exact frequency of post-minimum species choices.

Validation performed: verified starting `HEAD`; verified clean index and
tracked worktree; initial `git diff --check` passed. No build, test suite, or
interactive game was run, as required for this bounded read-only
investigation.

No production code, tests, accepted gameplay documentation, or ROM/reference
material was changed. The required prompt copy and this report are the only
requested project records. No commit or push was made.
