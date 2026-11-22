# Fuseball Terminal Behavior and All-Rim Investigation

Date: 2026-11-22

## Outcome

**INVESTIGATION COMPLETE — READY FOR BOUNDED CORRECTION.**

The Atari ROM does not count extra Fuseball traversals. It checks generic
pending enemy supply from the Fuseball movement program at a particular
directional endpoint. With no pending supply, the program skips its ordinary
direction/reversal handling and retains motion toward the player/rim. The
condition is not an active-enemy count, all-rim flag, or special “last
Fuseball” test.

The current C# `Fuseball` has no corresponding terminal state or pending-supply
check. Its separate Incitti-derived `HurryUp` behavior accelerates climbing in
development scenarios; it neither implements the Atari terminal branch nor
activates in normal production Level 11. The shared C# omission is global to
Fuseball behavior, but current production exposure is Level-11-specific:
Level 11 is the first production wave configured to admit a Fuseball.

No source, test, configuration, durable documentation, or Git index changes
were made. The supplied task was copied to the paired prompt record.

## 1. Atari Fuseball terminal mechanism

**ROM-BACKED:** The Fuseball movement program starts at p-code `$61`, whose
first operation is `eact_1e`; the per-type creation table assigns that
movement-program entry to a Fuseball. The movement handler uses
`enemy_along`, `enemy_along_lsb`, the `$80` direction bit in `$028a,x`, the
level's Fuseball speed, and level movement flags. It adds speed while moving
toward the player and subtracts speed while moving away. Ordinary direction
changes are selected through the `fuzz_move_flg`/random path. See the [ROM
disassembly](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L4010),
[Fuseball p-code](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L4350),
and [type-entry table](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3491).

The terminal test is in the toward-player leg of `eact_1e`: after that tick's
along-tube movement, the updated `enemy_along` is compared with
`player_along`. If it has reached or passed that boundary, the handler reads
`enemies_pending`. Zero pending supply returns from the movement handler
before the ordinary endpoint/reversal logic runs. This leaves the Fuseball's
toward-player direction intact, so it continues toward the rim instead of
starting another ordinary traversal. The disassembly itself annotates the
test “if no pending, rush to top.”

This is a per-movement-update check, but **not** an unconditional poll of
pending supply: it is reached only on the toward-player branch at/through the
`player_along` boundary. The away-from-player branch does not test
`enemies_pending`. No terminal flag, enemy-type conversion, or traversal
counter is set by the zero-pending branch. Do not conflate it with the
separate exceptional path at `$9fbc`, which writes p-code `$66` (`done`) under
other movement circumstances.

`enemies_pending` is the generic enemy supply count: wave setup initializes
it from `wave_enemies`, and successful pending-entry creation decrements it.
`enemies_in` and `enemies_top` are separate active-enemy counts. The
Fuseball's terminal branch reads neither of those counts, nor a count of
Flippers, nor a rim flag.

Level 11 is below the ROM's level-17 threshold in this same handler. With
pending supply still present, its normal endpoint behavior takes the
level-specific `$20`/`fuzz_move_flg` path; the zero-pending early return
bypasses that ordinary direction decision.

The ROM's type-selection data sets Level 11's active Fuseball minimum and
maximum to one. Its generic enemy creation path also has a split-to-Fuseballs
type code, but the terminal movement branch itself does not change enemy type.

## 2. Atari all-rim relationship

Rim Flippers do not contribute to `enemies_pending`: pending supply is
initialized and decremented separately from active enemies, while rim
occupancy is represented by active-enemy state and `enemies_top`. The
Fuseball terminal branch does not inspect `enemies_top`, `enemies_in`, or
whether it is the last below-rim enemy. Thus, if pending supply reaches zero
while other Flippers are at the rim, those Flippers do not block the
Fuseball's terminal movement. The same pending-zero branch can apply with
other active enemies still below the rim.

Wave exit remains a separate condition. After pending supply and the
additional transient counter `$0116` are zero, the exit code scans active
enemy positions and refuses exit if any nonempty slot has
`enemy_along >= $11`. Consequently, a below-rim Fuseball still blocks exit
even when every other remaining Flipper is already at the rim. `$0116` is
associated in the disassembly with eight transient enemy-death slots; it is
not the Fuseball terminal trigger or an all-rim flag. The Fuseball itself must
reach the rim-side position before the positional completion scan can pass.

The tracked disassembly does not establish a separate “all enemies at rim”
flag that changes Fuseball motion. It shows a local pending-supply decision
for Fuseball motion and a distinct global positional condition for exit.

## 3. Extra-cycle behavior

**Does the Atari ROM encode an explicit number of extra Fuseball up/down
traversals after the terminal condition becomes true? No.**

The examined Fuseball movement code contains no extra-cycle counter. The
apparent delay depends on when `enemies_pending` becomes zero relative to the
Fuseball's direction and progress. If supply reaches zero while the Fuseball
is moving away, the test is not taken until it returns to the toward-player
boundary. If the count changes just after that boundary was processed, it
must reach the test point again. The movement program's current along position
and direction therefore produce variable apparent extra motion. Neal's
approximate one-or-two traversals must not become a fixed implementation
constant.

## 4. Current C# mechanism

The shared C# state machine has `Lateral`, `Climbing`, `Descending`, and
`KillingPlayer`; it has no terminal/rim-directed state. At lateral lane
boundaries it normally chooses climbing or descending randomly, except that
height zero forces climbing. Climbing advances toward `Level.Depth`;
descending clamps at zero. Vertical duration is checked each update against a
new random threshold, after which the Fuseball returns to lateral motion.
Neither the movement state nor `Fuseball.Update` reads generic pending
enemy units or other enemies' rim states.

`TempestGame.UpdateFuseballs` passes the game-owned `HurryUp` bool to the
Fuseball. `LatchHurryUp` is guarded by `FuseballTest || PulsarTest` and
requires exhausted reserve/active Eggs plus a below-rim test Fuseball or
Pulsar. It runs after enemy updates and collisions, so a newly latched value
is used on a later update. Normal production Level 11 does not set this
development-only latch.

When true, C# Hurry-Up is not the ROM terminal mechanism: it forces climbing
at a lateral boundary (or at height zero) and adds a second climb increment
while climbing. An already-descending Fuseball continues descending, and the
latch does not remove a direction reversal or permanently prevent another
cycle. Its exact C# behavior is documented as **DEVELOPMENT ONLY /
PROJECT DECISION**, with the underlying acceleration **REFERENCE-DERIVED**,
not ROM-backed terminal behavior.

Current Level 11 production sets the live Fuseball minimum and maximum to one.
Production all-rim completion is independent
of movement: it requires no reserve Eggs, active Eggs, cargo reserve or
pending activation, no latent cargo, active identities matching the
unresolved ledger, and every active enemy at `Height >= Level.Depth`. If
other Flippers are at the rim but the Fuseball is below it, all-rim completion
fails. Once the Fuseball also reaches the rim and the ledger conditions hold,
the game rim-clears the active identities and begins exit. Ordinary
production completion likewise cannot proceed while the Fuseball remains an
unresolved active identity.

No normal production level sets this latch; only the `--fuseball-test` and
`--pulsar-test` development scenarios qualify. This is not a production
terminal condition being evaluated at the wrong lifecycle boundary. The
terminal predicate is absent from the C# Fuseball path; Hurry-Up's end-of-tick
latch and the all-rim exit test are separate decisions with separate state
requirements.

## 5. Atari and C# state-transition comparison

| Phase | Atari ROM | Current C# |
|---|---|---|
| Ordinary cycle | P-code `$61` calls `eact_1e`; `$028a,x` direction and along position drive toward/away movement; ordinary endpoint behavior uses level flags/randomness. | `Lateral` alternates with `Climbing`/`Descending`; random duration and boundary choices control repeated excursions. |
| Trigger read | On a movement update, after moving toward the player and reaching/passing `player_along`, read `enemies_pending`. | No equivalent read in Fuseball movement. `PendingGenericEnemyUnits` is available on `Level`, but the Fuseball path does not use it. |
| Trigger condition | Generic pending supply equals zero; does not test active count, rim count, or whether the Fuseball is the last below-rim enemy. | No Fuseball terminal condition. Development-only Hurry-Up checks Egg exhaustion plus a below-rim Fuseball/Pulsar. |
| State change | Zero-pending branch returns before the normal endpoint/reversal path; direction remains toward the player/rim. No extra-cycle state/counter is written. | No state change for pending exhaustion. If Hurry-Up is active, only climb selection/speed changes; ordinary descent/duration behavior still exists. |
| Subsequent motion | Continues the existing toward-player motion instead of beginning another ordinary reversal. | Continues the periodic C# state machine; normal Level 11 does not receive Hurry-Up. |
| Rim and exit | Exit separately requires pending supply and `$0116` clear and all active positions below the `$11` blocking threshold. A Fuseball below the rim blocks exit. | All-rim separately requires every active enemy at `Level.Depth` and all ledger obligations represented; a below-rim Fuseball blocks exit. |

**First semantic divergence:** the first toward-player Fuseball update that
reaches/passes `player_along` while `enemies_pending == 0`. Atari skips the
ordinary reversal path. C# has no pending-supply query or terminal movement
state at this boundary, so its ordinary lateral/vertical cycle can continue.

The current C# can allow several more cycles because its production Fuseball
never learns that generic supply has ended. Its random direction/duration
rules remain active, and production Hurry-Up is not latched. If all other
obligations are already resolved, C# does eventually exit when the Fuseball
first reaches `Level.Depth`; if the ledger still has pending or active
obligations, all-rim correctly waits and the Fuseball may leave the rim again
before a later qualifying update.

## 6. Scope classification and historical evidence

**GLOBAL FUSEBALL DEFECT — production-observable only on Level 11.** The
missing terminal decision is in the shared Fuseball movement implementation,
not a Level-11-specific state machine. Earlier implemented production levels
do not expose it: tracked Level 4, 5, 6, and 9 policy records exclude ordinary
Fuseballs, and the ROM's minimum-Fuseball table first enables them at Level
11. The current Level 11 policy sets its live Fuseball minimum and maximum
to one. Earlier
production levels have no separate Fuseball implementation that already
contains the ROM mechanism; the same shared C# class is used by the
development test path.

Relevant historical records distinguish the evidence:

- The direct-hatched Fuseball report implements Incitti-derived movement and
  explicitly defers authentic composition and Atari terminal behavior
  ([Fuseball implementation report](2026-10-30-fuseball-implementation-report.md)).
- The Hurry-Up report records Neal's **ARCADE-OBSERVED** late-wave
  acceleration, while its precise speed, branch, and C# latch are
  **REFERENCE VERIFIED / PROJECT DECISION** and its all-Fuseball scenario is
  **DEVELOPMENT ONLY** ([Hurry-Up report](2026-10-30-rimit-hurry-up-implementation-report.md)).
- The behavior spec likewise says the existing late-wave latch is
  development-only and does not establish arcade timing/order
  ([behavior spec](../../TEMPEST-BEHAVIOR-SPEC.md#L644)).
- The all-rim report classifies the broad observed exit result separately
  from its C# height cutoff and ordering, which are reference-informed
  **PROJECT DECISIONS** rather than a Fuseball terminal mechanism
  ([all-rim report](2026-10-30-all-rim-completion-implementation-report.md)).
- Level 4-6 and 9 production records preserve zero/excluded Fuseball
  availability; they do not supply an earlier accepted Fuseball correction
  ([Level 4](../../TEMPEST-BEHAVIOR-SPEC.md#L2470),
  [Level 5](../../TEMPEST-BEHAVIOR-SPEC.md#L2522),
  [Level 6](../../TEMPEST-BEHAVIOR-SPEC.md#L2597),
  [Level 9](../../TEMPEST-BEHAVIOR-SPEC.md#L2678)).
- The Level 11 production record adds a production Fuseball selection and
  runtime identity conservation, not the terminal movement decision
  ([Level 11 implementation report](2026-11-21-genuine-level11-production-implementation-report.md)).

The current worktree was already substantially modified before this
investigation. In particular, `Fuseball.cs` has a pre-existing uncommitted
`OriginalSlotId` change used by Level 11 identity accounting; this
investigation did not alter it. The untracked `Reference/arcade/MBUCOD.V05.txt`
was not opened or searched, consistent with the task boundary.

## 7. Implementation readiness

**READY FOR BOUNDED CORRECTION.**

The evidence-supported correction is to introduce a terminal decision for the
shared Fuseball path, supplied by the production wave's generic pending
obligation state and evaluated at the ROM-equivalent toward-player boundary.
It should preserve ordinary cycling until that boundary, then suppress a new
reversal and continue toward the rim. Do not encode one or two extra cycles,
do not use active-enemy count or all-rim state as the trigger, and keep the
separate all-rim/exit decision unchanged.

Focused regression coverage should establish:

1. Pending supply prevents the terminal branch and preserves ordinary
   movement.
2. Zero pending supply during the away phase does not switch immediately;
   the switch occurs at the toward-player boundary, without a cycle counter.
3. Rim Flippers do not count as pending supply and do not independently alter
   Fuseball movement.
4. A below-rim Fuseball still blocks all-rim exit; reaching the rim allows
   the existing ledger/all-rim path to proceed.
5. Normal Levels 1-10 and the development-only Hurry-Up behavior remain
   unchanged.

No correction was implemented, and no tests/build or interactive game were
run. Static source/disassembly tracing resolved the requested mechanism; the
approximate arcade traversal count remains deliberately unquantified. Any
future timing correction still requires Neal's manual gameplay acceptance.

No files were staged, committed, pushed, reset, restored, stashed, cleaned, or
rebased.
