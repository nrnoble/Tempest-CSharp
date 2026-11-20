# Level 7 bottom-entry, death/re-entry, and enemy accounting investigation

## Executive conclusion

- **ARCADE VERIFIED:** Neal repeatedly observes no individual Flippers entering
  from the bottom on displayed Level 7; Tankers enter and carry Flippers. After
  a test with one visible Flipper, death, and re-entry, a Tanker entered rather
  than that Flipper.
- **ROM-BACKED:** On displayed Level 7, Atari's ordinary enemy picker cannot
  select an ordinary Flipper for bottom entry. The Flipper minimum is zero,
  and every picker branch that can call the Flipper materializer requires a
  nonzero Flipper minimum. The Flipper materializer exists, but is unreachable
  through the Level 7 picker.
- **ROM-BACKED:** Death combines active-below-rim, at-rim, and already-pending
  counts into one generic pending quantity. It does not carry forward the
  previous species, Tanker cargo identity, or per-enemy movement state.
- **ROM-BACKED:** A sole unresolved unit is initially selected as a Spiker,
  not a Flipper or Tanker, after a clean Level 7 reconstruction: Level 7 has
  minimums of two Spikers and one Tanker, but zero Flippers. Once that Spiker
  consumes the last pending unit, the ROM has a special Spiker path that can
  convert it in place into a Flipper-carrying Tanker at `$f0`. This is a
  concrete explanation for a Tanker appearing after death even when the
  unresolved generic quantity was one. It is not a two-Flippers-to-one-Tanker
  accounting rule.
- **ROM-BACKED / UNKNOWN:** The source explains how the observed sequence can
  happen. Without a recorded run and the exact pre-death pending count, it
  cannot establish whether Neal's particular Tanker was this converted
  Spiker or a Tanker selected directly from a larger generic pending supply.
- **REFERENCE VERIFIED:** Incitti permits ordinary Flippers in its random
  hatch selector whenever Flippers are enabled, including Level 7; it does not
  have Atari's zero-minimum gate or this ROM Spiker-to-Tanker substitution.
- **CURRENT C# BEHAVIOR:** The uncommitted Level 7 implementation can select
  ordinary Flippers and preserves selected unresolved original species
  through death. Both behaviors differ from the traced Atari logic.
- **PROJECT DECISION / PROPOSED INTEGRATION:** A future correction should be
  Level-7-specific: use ROM-shaped generic pending counts and picker gates on
  re-entry, and model the final-pending Spiker-to-Tanker conversion. Keep the
  accepted Levels 1-6 policies unchanged. No implementation was made here.

## Evidence classification and Neal's observations

### ARCADE VERIFIED

The following are Neal's supplied observations, not inferences from the
disassembly:

- Repeated Level 7 play shows no individual Flippers entering directly from
  the bottom. Bottom entrants observed are Tankers, whose cargo includes
  Flippers.
- The same general no-individual-bottom-entry observation applies to Levels
  8-10, which are outside this investigation.
- In the death test, one visible unresolved Flipper remained; after player
  death and re-entry, that Flipper did not re-enter from the bottom, and a
  Tanker subsequently entered.
- The visible Level 7 Flipper lane-change sequence is `2 - 3 - 2 - 2 - 2`.
  Movement is out of scope here.

“One visible Flipper” does not, by itself, establish that the ROM's hidden
pending count was zero. The report therefore traces both a literal one-unit
state and the more general one-visible-Flipper state.

## ROM variables and accounting

| State | ROM role | Species retained? |
|---|---|---|
| `enemies_in` (`$0108`) | Active enemy units below the rim | No; scalar count |
| `enemies_top` (`$0109`) | Active enemy units at the rim/top | No; scalar count |
| `enemies_pending` (`$03ab`) | Generic units not yet materialized | No; scalar count |
| `pending_seg[]`, `pending_vid[]` | Pending entry segment and its entry/visibility delay | No species field |
| `n_flippers[]` (`$0142` onward) | Active counts indexed by species for selection/capacity | Yes while active |
| `enemy_along[]`, `$0283[]`, `$028a[]`, `enm_move_pc[]` | Per-enemy active slot: position, type/direction, cargo/flags, movement PC | Yes while the slot is active |

The active type is in the low three bits of `$0283`; `$028a` carries flags and,
for Tankers, a low-bit cargo-type value. The ROM keeps no per-original identity
ledger. A Flipper, Tanker, or Spiker contributes one active unit to the
below-rim/top accounting. A Tanker's carried children are not separate active
units while aboard.

The address labels and definitions are in
[Tempest Rom Source code.md](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L137),
[the enemy-slot definitions](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L316),
and [the counters and type limits](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L172).

## Death/retraction and reset control flow

1. In the gameplay state, `$970e-$9726` calls `create_enemies`, enemy movement,
   collision processing, and `$a504` once per update.
2. `$a513-$a529` moves each occupied enemy slot toward the retraction boundary
   and clears its `enemy_along` occupancy as it leaves. It does not decrement
   `enemies_in` or `enemies_top` while doing this.
3. When the player reaches the death transition, `$a563-$a57b` enters state
   `$06`, clears shots, computes
   `enemies_in + enemies_top + enemies_pending`, caps the result at `$3f`,
   and stores that total back into `enemies_pending`.
4. The ordinary-life-loss handler `state_06` (`$c9b0-$c9f0`) decrements a life,
   sets the timed transition to state `$02`, and does not reconstruct species.
   The `state_0a` countdown transitions to that state.
5. `state_02` calls `$9025`. That setup resets the player, recomputes current
   level parameters, then calls `$926f` to clear active occupancy and all five
   species counters, `$9246` to rebuild generic pending segment/visibility
   entries from `enemies_pending`, and `$929f` to clear enemy-death work state.
   It does not replace the death-computed pending quantity with `wave_enemies`.
6. On the next gameplay updates, the same `$98a2` `create_enemies` routine and
   `$99a5` selector materialize that generic pending supply.

Thus, for ordinary re-entry, the quantity survives; the active slot species,
carrier/cargo association, and movement program do not. Inactive slot bytes
may remain in RAM, but the reset clears their occupancy and species counts;
new allocations overwrite the fields they use. Final-life Game Over does not
take the ordinary re-entry path.

Evidence: [death total and cap, ROM `$a56a-$a57b`](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L4886);
[life/state transition, `$c9b0-$c9f0`](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L9196);
[state `$02` reset call, `$c975`](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L9170);
[reset and generic pending initialization, `$9025-$928b`](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L2201).

### What the counters include

- `enemies_in` counts active non-top units; `enemies_top` counts units handed
  off at the rim. `$9d2c-$9d63` moves a normal enemy from the former count to
  the latter. At the `$20` boundary, `$a06f-$a08b` decrements the appropriate
  active count.
- Existing `enemies_pending` is included unchanged in the death sum. It is
  generic supply, not a count of pending Flippers versus Tankers.
- Cargo is not separately summed while aboard a Tanker. Released cargo becomes
  active child slots and then contributes through the active counters.

## Displayed Level 7 parameters and enemy selection

`curlevel=6` denotes displayed Level 7; `$92c5-$92d4` derives effective
table level 7. The parameter tables supply:

| Parameter | Level 7 value | Consequence |
|---|---:|---|
| `wave_enemies` | `$14` = 20 in the table | Fresh generic supply is 20 before difficulty adjustment |
| `max_enm` | 6 | Generic creation stops when below-rim plus top active count reaches 6 |
| Flipper minimum / maximum | 0 / 5 | Flipper is not eligible for the generic picker |
| Tanker minimum / maximum | 1 / 2 | Tanker is eligible and has an unmet minimum after reset |
| Spiker minimum / maximum | 2 / 4 | Spiker is eligible and is the first unmet minimum after reset |
| Pulsar and Fuzzball minima | 0 | Neither participates on Level 7 |
| `tanker_load[0..3]` | all type 1 | Level 7 Tankers carry Flippers |

The hard-difficulty branch at `$934d-$9377` adds one eighth of `wave_enemies`;
for 20, the carry-aware integer calculation produces 23. The table value is
20; the actual initialized total therefore depends on difficulty. The
Level-7 species gates above are table-driven independently.

`create_enemies` (`$98a2`) considers a pending entry only while active occupancy
is below `max_enm` and the zap is not blocking creation. When that pending
entry's delay completes, `$9923` sets its initial along-position to `$f0` and
calls `$99a5`. The selector:

1. Computes per-type availability from `max_* - n_*`.
2. For each active Tanker, subtracts two more units from the availability of
   its cargo species. This is a type-capacity adjustment, not a decrement of
   wave supply.
3. Bounds each availability by remaining global active capacity.
4. If exactly one type is available, only tries it when its minimum is
   nonzero. If multiple types are available, first scans for available types
   whose active count is below their minimum, again requiring a nonzero
   minimum.
5. If no minimum is outstanding and both Tanker and Spiker are available, it
   uses the shortest-spike height threshold at `$9a40-$9a60` to choose between
   them. Otherwise it starts at a `pokey2_rand`-derived index and loops over
   types, trying only types with nonzero minima and availability.
6. On a successful materialization, `$994d` allocates one active slot and
   increments its active/type counts; only then `$993c` decrements
   `enemies_pending` once. Failed allocation/selection retries the pending
   visual entry; it does not consume supply.

With freshly reset Level 7 counters, the first available unmet minimum is
Spiker (type index 3, minimum 2); Flipper's minimum is zero and Tanker's
minimum is 1. Thus a literal single generic pending unit is created as a
Spiker, not directly as a Flipper or a Tanker. Once the Spiker minimum has
been met, a Tanker can be selected as the next generic unit. Its selection
does not require two pending units.

Evidence: [table parameter evaluation, `$92c5-$9326`](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L2547);
[difficulty adjustment, `$9328-$937f`](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L2615);
[Level 7 type tables](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L2740);
[active cap and pending entry path, `$98a2-$999c`](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3217);
[selection and type dispatch, `$99a5-$9afc`](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3333).

## Tanker supply and cargo semantics

There are two distinct Tanker paths:

1. **Ordinary selection:** a Tanker is one successful pending materialization;
   `$993c` decrements `enemies_pending` once. The Level 7 load vector contains
   type value 1 (Flipper) in all four choices. Availability reserves two
   Flipper-capacity units for each active Flipper-carrying Tanker.
2. **Spiker substitution when no supply remains:** the Spiker movement action
   checks `enemies_pending` at `$a009`. If it is zero, `$a00e-$a01f` rewrites
   that same occupied enemy's type to Tanker and its cargo type to Flipper,
   then returns it to along-position `$f0`. This adds no pending unit and is
   not a conversion of two Flippers into one Tanker.

When a Tanker reaches the `$20` rim boundary, `$a06f-$a0f3` retires its active
slot/type count and invokes the slot allocator twice for its two Flipper
children. Those children start at the parent's along-position and adjacent
segments; they are not ordinary bottom-entry units and do not decrement
`enemies_pending`. The parent accounts as one active/pending wave unit; the
two carried Flippers are materialized as children separately from the wave
pending counter. The cargo pair affects the species-availability calculation,
not the number of wave units consumed by the carrier. Generic creation is
gated at six active units; cargo creation instead uses the slot allocator
(indices 6 down to 0) and can fail if it finds no free slot. The two child
allocations are attempted separately.

The in-place Spiker conversion visibly rewrites `$028a` and `$0283` but does
not visibly adjust the `n_spikers`/`n_tankers` counters in that block. The
source shows no further pending type selection after the trigger (pending is
zero); any downstream implications of those type counters are not established
here. The ordinary life reset clears all type counters.

Evidence: [Tanker load initialization, `$93d7-$93df`](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L2671);
[Tanker load table and selector, `$9abb-$9aec`](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3495);
[capacity accounting, `$99c0-$99dc`](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3351);
[Spiker-to-Tanker branch, `$9fc6-$a027`](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L4098);
[Tanker children, `$a06f-$a0f3`](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L4179).

## Ordinary bottom-entry Flipper reachability

| Path | Result on Level 7 |
|---|---|
| Generic pending entry selects type 0 and calls `$9a9d` Flipper setup | **Unreachable:** all routes to the type materializer are gated by `min_flippers != 0`; Level 7 minimum is zero. |
| Flippers released by a Flipper-carrying Tanker | Reachable as cargo, but child creation is at the Tanker's `$20` rim position, not `$f0` bottom entry. |
| Existing Flipper reaches the rim | It is an already-active unit; not a new bottom entrant. |
| Pending entry after death | Species is not retained; it re-enters the generic selector and is subject to the same zero-minimum gate. |
| Spiker reaches the no-pending conversion branch | Can become a Flipper-carrying **Tanker** at `$f0`; it does not create an ordinary Flipper there. |

**Conclusion: B.** The ROM contains an ordinary Flipper materializer, but the
Level 7 selection controls make that path unreachable in the traced normal
creation flow. No odd-count, one-count, or low-cargo corner branch bypassing
the minimum gate was found. Tanker cargo release and the Spiker-to-Tanker
substitution are different paths, not ordinary bottom-entry Flippers.

## Symbolic one-visible-Flipper death trace

Let `I=enemies_in`, `T=enemies_top`, and `P=enemies_pending` immediately before
death retraction. The visible Flipper contributes one to `I` or `T`; any
unseen reserve already pending contributes to `P`.

1. Death stores `R=min(63,I+T+P)` in `enemies_pending`. If that Flipper is the
   only unresolved unit in the whole wave, then `R=1`; if other units remain
   pending, `R` is larger. Its Flipper species bit and movement state are not
   part of `R`.
2. `$9025` clears active slots and all `n_*` counters, then initializes generic
   pending entry records. No per-enemy identity or old movement PC is restored.
3. For `R=1`, the first generic entry is a Spiker because `n_spikers=0` is
   below Level 7's minimum 2. It is not an ordinary Flipper. Successful
   creation consumes the one pending unit (`enemies_pending: 1 -> 0`).
4. When that Spiker later takes the `$9fc6-$a027` movement path and reaches
   its `$f2` condition, the code checks `enemies_pending`. With zero pending,
   it changes its own type/cargo fields to a Flipper-carrying Tanker and
   places the same slot at `$f0`. The Tanker is therefore a one-unit
   substitution after the Spiker's movement, not an immediate type
   reconstruction of the prior Flipper.
5. For larger `R`, the picker can select Tankers directly once the Spiker
   minimum has been satisfied; the exact type also depends on active
   availability, spike height, and random start. The zero-pending Spiker
   conversion remains a separate possible path.

No branch in the traced selection or death code tests `R==1`, oddness, or
whether there are enough pending units for Tanker cargo. The relevant special
condition is `enemies_pending==0` in the Spiker action. That condition is
reached after the final generic unit has been consumed, including when
`R==1`.

**ROM-BACKED comparison to Neal's observation:** the ROM loses the Flipper
identity, prevents ordinary Flipper bottom entry, and contains a reachable
last-pending Spiker-to-Tanker route. This directly explains how the reported
Tanker can appear after a one-unit reconstruction. The report cannot identify
which Tanker path occurred in the particular arcade run without its hidden
pending count and a recording.

## Fresh Level 7 versus post-death re-entry

Fresh entry loads `wave_enemies` into `enemies_pending` at `$9233-$9237`, then
initializes pending entry segments/visibility. Death instead replaces the
pending count with active-plus-pending unresolved quantity and invokes the
player-life reset without reloading `wave_enemies`. Both paths subsequently
use `create_enemies` and `$99a5`; there is not a separate species-selection
algorithm for re-entry. The different input is the fresh supply count versus
the death-reconstructed count. The Level 7 quota tables and final-pending
Spiker conversion apply in either case.

## Incitti comparison

**REFERENCE VERIFIED:** Incitti's `Egg.PickEnemy` uses a random candidate
range; it maps negative choices to 0 and returns candidate 1 (Flipper) when
selected. `Egg.Hatch` creates a Flipper whenever `theLevel.hasflippers`, and
on Level 7 `SetUpEnemies` enables Flippers and Tankers. Incitti's Tanker cargo
loop permits the two-Flipper carrier while the other Tanker cargo types remain
disabled. Thus Incitti can ordinarily materialize an individual Flipper from
an Egg on Level 7; Atari's ROM picker suppresses that type via its zero
minimum.

Incitti's `ConvertBaddieToEggs` drops active species counters and converts
surviving baddies to generic Egg reserve (subject to `maxeggs`); later Egg
hatching randomizes types again. It therefore also loses prior species on
death, but unlike the Atari selector it does not impose the ROM's
minimum-nonzero type gate or the traced no-pending Spiker-to-Tanker path.

Sources: [Incitti Egg selector/hatch](../../Reference/incitti/tempest.bmx#L1982),
[death conversion](../../Reference/incitti/tempest.bmx#L2224),
[Level 7 flags and supply](../../Reference/incitti/tempest.bmx#L4208).

## Current C# comparison

**CURRENT C# BEHAVIOR:**

- `Level7Production` has supply 20, four initial Eggs, and
  `FlippersTankersAndSpikers` composition.
- That composition uses `Level3Ledger` and `CreateLevel3MixedEnemy`; when an
  original has no selected type, `SelectLevel4OriginalType` can return
  Flipper, Tanker, or Spiker. It does not apply the ROM's Level 7
  `min_flippers=0` rule. Ordinary individual Flippers can therefore hatch
  from bottom Eggs on current C# Level 7.
- `ResetAfterDeath` calls `RebuildAfterDeath`. The ledger retains selected
  species types for unresolved original entries, and the same original type
  is used when the fresh Egg hatches. Thus an unresolved Flipper is
  reconstructed as a Flipper, unlike Atari's generic-count reconstruction.
- Current C# Tanker cargo has separate ledger identities and is not the same
  accounting model as the ROM's carrier contributing one pending unit and
  later releasing two active children.

The ordinary-entry mismatch comes from the current shared random mixed-wave
selector; the post-death mismatch is additionally reinforced by the ledger's
species preservation. Neither needs a global ledger rewrite to address.

Sources: [Level 7 configuration](../../src/Tempest.Game/Levels/LevelConfiguration.cs#L267),
[death reset](../../src/Tempest.Game/Levels/Level.cs#L120),
[Level 7 type selection](../../src/Tempest.Game/TempestGame.cs#L724),
[mixed selector](../../src/Tempest.Game/Levels/Level3WaveSelection.cs#L45),
[ledger reconstruction](../../src/Tempest.Game/Levels/Level3WaveLedger.cs#L252).

## Narrow implementation implications (no changes made)

**PROJECT DECISION / PROPOSED INTEGRATION:**

- Add a Level-7-only generation/re-entry policy rather than changing the
  accepted Level 1-6 picker or the shared identity ledger globally.
- Keep generic unresolved quantity across a Level 7 life loss; clear/reselect
  species for unresolved original entries instead of retaining the prior
  Flipper/Tanker/Spiker choice.
- Match the ROM's nonzero-minimum gate so ordinary Level 7 bottom Flippers are
  never selected, and model the explicit no-pending Spiker-to-Tanker
  conversion as a same-unit in-place transformation.
- Keep Tanker cargo as two children created from the carrier, outside the
  original pending-unit count; preserve the distinction between bottom
  entrants and rim-released cargo.
- Add focused tests for: a one-unit death reconstruction selecting Spiker;
  final pending reaching zero and converting the Spiker into a Tanker;
  no ordinary bottom Flipper selection across reachable Level 7 counts;
  Tanker selection after quotas/capacity permit it; and two cargo children not
  consuming pending supply.

These are implications, not an implementation plan approved by Neal.

## Remaining unknowns

- The exact hidden `enemies_pending` value in Neal's arcade death test, and
  therefore whether its observed Tanker was directly selected or resulted
  from the final-pending Spiker conversion.
- Whether a particular observed Tanker was visually tracked from the Spiker
  conversion point or entered through the ordinary Tanker selector.
- The source's Spiker-to-Tanker branch rewrites the enemy type and cargo but
  does not visibly update the per-type counters in that block; the broader
  consequence for later type-count bookkeeping is not established by this
  trace.
- Actual run-specific difficulty and timing are not supplied. The ROM's
  static trigger and parameter logic are established; arcade visual timing of
  the conversion is not.

## Source references by ROM address

- RAM definitions: `enemies_in` / `enemies_top` `$0108-$0109`, type limits and
  counts `$0129-$0146`, `tanker_load` `$0149-$014c`, `wave_enemies` `$015b`,
  `flipper_move` `$015d`, `enemies_pending` `$03ab`.
- Level parameter loader and difficulty: `$92c5-$9326`, `$9328-$937f`;
  table pointers `$9617-$9655`; Level 7 tables `9607_13`, `9607_17`,
  `9607_23`, `9607_27`, `9607_2b`, `9607_2f`, `9607_4b`, `9607_4f`.
- Fresh supply / re-entry reset: `$9009-$904a`, `$9025-$928b`,
  `$91a1-$91a4`, `$9233-$9237`, `$9246-$926e`, `$926f-$928b`.
- Gameplay creation / selection: `$970e-$9726`, `$98a2-$999c`,
  `$99a5-$9afc`, especially availability `$99b1-$99f9`, minimum/random
  selection `$99fb-$9a86`, and type dispatch `$9a87-$9afc`.
- Death reconstruction: `$a504-$a57b`; life-state handlers `$c9b0-$c9f0`,
  `$c80f-$c818`, `$c942-$c978`.
- Tanker accounting/release: `$99c0-$99dc`, `$a06f-$a0f3`.
- Special final-pending conversion: `$9fc6-$a027`, with the Spiker program
  comment and branch at `$a0f7-$a0fb`.

Markdown source line anchors for these paths are linked in the corresponding
sections above. ROM addresses are included because the source file contains
disassembly, tables, and data in a layout where physical line numbers alone
are not an adequate control-flow locator.

## Completion

This was a read-only investigation. No production code, tests, project
authority documents, ROM/reference files, or configuration were changed. No
game was launched. No commit or push was made. The requested prompt copy and
this report are saved under `AI-Logs/Prompts/` and `AI-Logs/Reports/`;
`AI-Logs/` is ignored by the repository's `.gitignore`.
