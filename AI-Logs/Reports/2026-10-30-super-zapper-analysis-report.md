# Super Zapper Analysis — Current Post-Hurry-Up Project

## Status and recommendation

**Analysis only.** Starting checkpoint: `587b9e3 Implement Fuseball and Pulsar
hurry-up behavior`. The only files created for this task are this report and the
saved prompt. No gameplay, test, README, behavior-specification,
project/configuration, or tracked files were changed; no build/test was run and
no commit was created.

**Recommendation: A — implement Super Zapper as one complete implementation
milestone.** It is a cross-cutting feature, but not a progression prerequisite:
all five zap-accepting enemy types and the ordinary reset/completion paths
already exist. Treat stillborn Tanker cargo as a required acceptance gate, not a
later enhancement. Splitting out a player-visible “partial Zapper” would either
make its first/all-use wrong in `--tanker-test`, leave pending kills without
their correct score/accounting, or require hiding/disabling the feature in an
existing scenario. Those are unsafe, misleading intermediate semantics.

Engineering can proceed internally in this order—shared timer and tests,
input/charges/targeting, each enemy resolver, Tanker ledger/cargo, rendering,
then regression/manual checks—but acceptance should be atomic: all target
types, both charges, delayed resolutions, and death/re-entry invariants work
before the feature is described as implemented.

## Evidence and source locations

Use the project’s labels literally. The mechanics below are **REFERENCE
VERIFIED** when they describe Incitti’s code, not Atari truth. Existing
development compositions remain **DEVELOPMENT ONLY**. Current simulation
behavior and proposed lifecycle decisions are **PROJECT DECISION** until
implemented and tested. No Super Zapper behavior is currently **ARCADE
VERIFIED**.

The literal source paths most relevant to implementation are:

- [`Player.Update` activation](../../Reference/incitti/tempest.bmx#L658)
- [`Baddies.SuperZapit` selection and dispatch](../../Reference/incitti/tempest.bmx#L2257)
- [base baddie types and reset conversion](../../Reference/incitti/tempest.bmx#L2171)
- [bullet `Zapit`](../../Reference/incitti/tempest.bmx#L2385)
- [Tanker release, zap expiry and ordinary release](../../Reference/incitti/tempest.bmx#L2467)
- [Flipper zap expiry and ordinary update](../../Reference/incitti/tempest.bmx#L2727)
- [Pulsar zap expiry and ordinary update](../../Reference/incitti/tempest.bmx#L3025)
- [Fuseball zap expiry and ordinary update](../../Reference/incitti/tempest.bmx#L3328)
- [Spike rejection](../../Reference/incitti/tempest.bmx#L3528)
- [Spinner/Spiker zap expiry and ordinary update](../../Reference/incitti/tempest.bmx#L3616)
- [charge setup and display countdown](../../Reference/incitti/tempest.bmx#L1429)
- [new-game/level charge setup](../../Reference/incitti/tempest.bmx#L1737)

The specified historical report
[`2026-10-30-next-milestone-after-pulsar-report.md`](2026-10-30-next-milestone-after-pulsar-report.md)
already outlines the broad feature, but its source summary is not a substitute
for this literal comparison. The current behavior specification and current
post-587b9e3 code take precedence. In particular, the current C# player-death
freeze, kill-all completion, Tanker identities, and Fuseball/Pulsar hurry-up
must remain intact.

## Literal Incitti behavior

### Input, charges, and lifecycle

- **Activation — REFERENCE VERIFIED.** One edge-triggered press of Space or
  right mouse (`KeyHit(KEY_SPACE)` or `MouseHit(2)`) is considered. It activates
  only while `deathcount = 0` and `superzapper > 0`. A held key does not repeat
  the zap. The ordinary fire input is separate.
- **Charges — REFERENCE VERIFIED.** The initialized supply is two. The first
  accepted press dispatches strength 2 (all-target); the next dispatches
  strength 1 (single-target). `superzapper` is decremented after dispatch, even
  if a strength-1 search finds no target.
- **Recharge — REFERENCE VERIFIED.** Two charges are restored during new-game
  and level setup (`superzapper = 2` at source lines 1429 and 1739). The death
  conversion path does not replenish them. The source’s “SUPERZAPPER
  RECHARGE” message is a level-begin presentation, not a live numeric charge
  counter. There is no gameplay-HUD display of the remaining count.
- **Current C# lifecycle — PROJECT DECISION.** `ResetLevel` is called for
  initial construction, Level Complete replay, and Game Over restart.
  Initialize charges there. Do not reset them in `ResetAfterDeath` or
  `ResetTankerScenarioAfterDeath`. Thus ordinary life loss preserves the
  remaining count; a replay/new game restores two, while preserving the
  existing C# replay policy for score/lives and the Game Over policy for
  score/lives.
- If an input arrives during C# `PlayerDying`, exit, Game Over, or Complete, the
  existing `RunOneStep` state routing does not run active player input. Keep
  that behavior; do not make a separate zap-input path that bypasses it.

### First use: strength 2 / all-target

`Baddies.SuperZapit(2)` initializes delay `cnt = 4`, sets the web/tube flash to
60, then visits `BADDIE_LIST` in insertion order. For each entry with
`baddietype <> BAD_SPIKE`, it calls `Zapit(cnt)`. It increments `cnt` by 2
**only when `Zapit` returns true**, producing delays 4, 6, 8, 10, ... for
accepted entities. A rejected bullet does not consume a delay slot. Spikes are
not called at all. The display timer is set even if no target accepts.

The ordered list includes bullets, so “excluded” needs precision: bullets are
visited by the first-use loop but their `Zapit` returns false. Use the existing
creation/insertion order; do not sort by lane, height, or proximity. This is
distinct from the current development shot-collision code, which also has an
insertion-ordered list but must not be confused with the reverse Flipper shot
scan in ordinary Level 1.

### Second use: strength 1 / one target

The literal algorithm is not a simple nearest- or highest-enemy search:

1. Initialize `i = mainplayer.e_index`, `b_best = Null`, `best_d = 16`,
   `best_h = 0`, and `best_b = 0`.
2. Visit `BADDIE_LIST` in insertion order, considering only
   `baddietype > 1` (enemy types; this excludes spikes and bullets).
3. Compute signed lane displacement as `d = -(i - b.e_index)`. On a
   continuous tube, if `Abs(d) > 8`, negate `d`; then take `Abs(d)`. Thus a
   16-lane tube uses circular shortest distance, with the opposite-lane tie
   at 8 unchanged. On a non-continuous tube, use the absolute direct lane
   difference without wrap.
4. Only candidates satisfying the strict gate `d < best_d + 2` are compared.
5. Within that gate, a strictly greater height than `best_h` wins immediately,
   even if it is one lane farther than the current best; all best fields are
   replaced.
6. Otherwise, a strictly smaller distance than `best_d` wins, regardless of
   type. Otherwise, a strictly greater type number than `best_b` wins.
   Equal height does not satisfy the height branch; equal type does not replace
   the incumbent. Iteration order therefore breaks otherwise-equal cases.
7. If a target was selected, call `Zapit(4)` and set the display timer to 60.
   If none exists, the display is not set. The outer player input still
   consumes the charge.

The source comment says “same d” for the final type tie-break, but the code
does not check equality there. Because the outer gate allows distances through
`best_d + 1`, a non-taller candidate one lane farther can win on type number;
`best_d + 2` is excluded. Tests must encode the executable comparisons, not
the comment. The initial `best_d = 16` gate admits all valid lane distances in
the current 16-lane tube. `best_h` and type comparisons are strict.

Source type ordering is `Spinner/Spiker=2`, `Flipper=3`, `Tanker=4`,
`Pulsar=5`, `Fuseball=6`. A taller lower-type enemy can therefore outrank a
shorter higher-type enemy; type is only reached through the final comparison
branch.

There is no source guard against re-zapping an already pending enemy:
each accepting `Zapit(t)` simply overwrites `dietime`. If the second use
selects a target already counting down (including stillborn Tanker cargo), its
timer becomes 4. Preserve this unless direct arcade evidence justifies a
change.

### Eligible and rejected entities

The source’s `Baddies` type codes and `Zapit` implementations establish:

| Entity | Zap selection | Result on delayed expiry | Evidence |
|---|---|---|---|
| Flipper, including released Tanker cargo | Accepted; type 3 | Remove, score 150 | REFERENCE VERIFIED |
| Tanker | Accepted; type 4 | Remove, score 200, release stillborn cargo | REFERENCE VERIFIED |
| Spinner (C# Spiker) | Accepted; type 2 | Remove, score 50; its spike remains | REFERENCE VERIFIED |
| Fuseball | Accepted; type 6 | Remove, height-based score and Fuseball score popup | REFERENCE VERIFIED |
| Pulsar | Accepted; type 5 | Remove, score 200 | REFERENCE VERIFIED |
| Enemy bullet | First pass calls `Zapit`, which returns false; second pass excludes type -1 | Unchanged | REFERENCE VERIFIED |
| Spike | Excluded from first pass (`type <> 1`); second pass excludes type 1 | Unchanged | REFERENCE VERIFIED |

These are all entity types represented in the current C# implementation. A
spike is not an `Enemy` in C#, while bullets are `Enemy` subclasses; the
shared zap contract must consequently default to rejection so bullets do not
become zappable accidentally. Eggs are stored separately from
`BADDIE_LIST`/the C# enemy lists and are not candidates; neither are player
shots or visual explosions. No other source baddie type is defined in the
inspected source.

### Delayed timer, ordinary behavior, scoring, and pre-emption

Each accepting source `Zapit(t)` assigns `dietime = t`. At the start of each
enemy update, the handler decrements a positive timer. If it becomes zero,
the enemy awards its zap score, removes itself, and returns **without running
ordinary behavior on that expiry update**. On all earlier countdown updates
it proceeds into its normal state machine. In the first-use player update,
zapping happens before `Baddies.UpdateBaddies`, so delay 4 is first decremented
on that same simulation update and expires on the fourth enemy update
including the activation tick. Delay 6 similarly expires on its sixth
enemy-update visit. The second-use delay 4 has the same inclusive timing.

Ordinary behavior continues during the countdown: movement, turning,
shooting, retreat/release transitions, Pulsar pulse-lane flags/collisions, and
player threat checks still happen. The zap does not make an enemy invulnerable.
Player shots are processed before enemy updates; if a shot removes a
zap-pending enemy first, its ordinary shot score/accounting wins and the
pending zap has no later effect. Likewise, a Spiker can retreat to reserve
without zap score, and a Tanker can naturally release before zap expiry. For a
Tanker, if the zap timer reaches zero first, the zap branch takes precedence
over that update’s climb/release. If it does not, normal Tanker movement and
the natural `height > 385` release check run. Death also pre-empts pending zap
resolution under the C# reset policy described below.

Exact expiry awards:

- Flipper: 150.
- Tanker carrier: 200 at carrier-zap expiry.
- Pulsar: 200.
- Spiker/Spinner: 50.
- Fuseball: `Int((400 - height) / 100 + 1) * 250` at expiry height. C# already
  uses the equivalent `Fuseball.KillScore`; resolve using the height when the
  timer reaches zero, not the height at activation.
- Each stillborn cargo Flipper: its ordinary Flipper zap/kill score, 150,
  when its own timer expires.

Fuseball expiry must use the existing `FuseballScorePopup` route. Create the
popup with the expiry height and points, and award those points once at the
same resolution moment. The popup’s existing 30-update lifespan and rendering
should remain unchanged.

### Tanker and stillborn cargo source path

At Tanker zap expiry, source order is: play zap effect, award the carrier’s
200, call `ReleaseCargo(True)`, then remove/decrement the Tanker. For each
Flipper cargo unit, `ReleaseCargo(True)` creates a Flipper on the source
adjacent lane at the Tanker’s current height, sets its ordinary Flipper type
to 1 (the initial flipping state), sets its direction, then sets `dietime`
to 8. The source uses its normal lane-wrapping behavior on a continuous tube.
The new child is a real baddie and increments active enemy count; it is not
latent cargo any longer. Its first update decrements that 8-tick timer; if it
survives until the eighth visit, it receives the ordinary Flipper delayed-zap
resolution and 150 points. Before its timer expires it continues its Flipper
state and can be shot or threaten the player. A second zap can reset its timer
to 4 because `Zapit` has no “already pending” rejection.

Static inspection does not establish whether BlitzMax’s mutable list iteration
updates children appended during the carrier’s current update. The current C#
Tanker code already adds release children after the Flipper update phase (and
defers shot-released cargo activation); use that established **PROJECT
DECISION** consistently. A zap-expiry child is registered immediately as an
accounted stillborn Flipper but starts its eight-update countdown on the next
simulation tick. This is deterministic and avoids same-frame list-mutation
ambiguity; record it as C# behavior, not arcade truth.

## Current C# architecture and integration design

### Existing surfaces

- [`Systems/SuperZapperSystem.cs`](../../src/Tempest.Game/Systems/SuperZapperSystem.cs)
  exists but is an empty scaffold.
- [`Systems/InputSystem.cs`](../../src/Tempest.Game/Systems/InputSystem.cs)
  defines `PlayerInput(Fire, MouseDeltaX, Left, Right)` and samples fire as
  held input. Add a distinct edge-triggered `ZapPressed`, sampled from
  `KeyboardKey.Space` and `MouseButton.Right`. Make it an optional trailing
  primary-constructor parameter (default `false`) or an init property so
  existing four-argument `PlayerInput` test calls keep compiling. Keep
  simulation tests able to inject the input without Raylib.
- [`Gameplay/Enemy.cs`](../../src/Tempest.Game/Gameplay/Enemy.cs) currently
  contains only `EdgeIndex`, `Height`, and abstract `Update()`. Flipper,
  Tanker, Spiker, Fuseball, Pulsar, and EnemyBullet derive from it. Spike is a
  separate type.
- [`TempestGame.cs`](../../src/Tempest.Game/TempestGame.cs) owns the typed
  active-enemy lists, score, input/update order, `developmentCollisionOrder`,
  all reset/completion logic, and the special Tanker ledger. Development
  shot collision order is already preserved in a `List<object>`; ordinary
  Level 1 does not populate that list and its shot scan is reverse Flipper
  order.
- [`ScoreSystem.cs`](../../src/Tempest.Game/Systems/ScoreSystem.cs) owns
  additive score and reset, but enemy-specific kill scores are currently
  applied by `TempestGame` at collision resolution.
- [`Level.cs`](../../src/Tempest.Game/Levels/Level.cs) preserves the normal
  ten-unit conservation ledger. Tanker development mode separately tracks
  original slot IDs and pending cargo Egg IDs through `TempestGame` plus
  `Level.RestoreTankerEntries`.
- `HurryUp` is a `TempestGame` latch, triggered after hatch/enemy/player
  collision updates. It persists across ordinary life loss and resets in
  `ResetLevel`. Do not change its trigger, effect, or reset semantics.
- Completion is a C# kill-all rule: `Level.IsComplete` considers active
  Flippers, Spikers, Fuseballs, and Pulsars; Tanker mode also requires all
  original slots and cargo IDs resolved, and no active Tankers/Flippers/
  Spikers or Eggs. Do not copy Incitti’s `onrim` exit condition.
- [`VectorRenderer.cs`](../../src/Tempest.Game/Rendering/VectorRenderer.cs)
  draws the fixed blue tube, pulse overlays, enemies and HUD. The gameplay HUD
  shows score/lives and active/remaining entities but no controls or Zapper
  charges. `AudioSystem` is empty.

### Minimal shared zap contract

**Recommendation: add a small polymorphic contract and timer to the existing
`Enemy` base, not a new hierarchy.** All five valid enemies need identical
accept/reassign-delay and countdown rules; EnemyBullet must reject. A shared
default-reject `TryZap(delay)` (or equivalent) with a base-owned zap-tick
counter makes that invariant explicit and prevents five divergent timer
implementations. The five enemy types opt in; EnemyBullet inherits rejection.
Spikes are outside the contract and never enter eligible candidate selection.

Keep scoring, collection mutation, Tanker cargo creation, popup creation, and
ledger changes in `TempestGame`. The base contract should not know about
`ScoreSystem`, `Level`, rendering, input, or audio. `TempestGame` should
advance a pending timer before that entity’s existing type-specific update:
if it expires, resolve and skip ordinary update for that tick; otherwise run
the unchanged ordinary update. This mirrors source timer priority and avoids
duplicating timer arithmetic in each state machine.

### Target iteration and activation

Use `SuperZapperSystem` for two charges, the flash counter, activation
strength, and the literal second-use selection routine. It should operate on
an explicitly ordered view of live `Enemy` objects and request
`TryZap(delay)`; it should not own game entities or resolve scores.

For strength 2, enumerate all source-order baddies in the development
insertion-order list, including bullets (their contract rejects), but omit
spikes. In ordinary Level 1, use Flippers in their existing creation order.
For strength 1, enumerate the enemy candidates in source insertion order and
apply the exact type/distance/height comparison above; bullets and spikes are
ineligible. Do not reuse the reverse ordinary shot loop or a height sort.

The current `developmentCollisionOrder` is the strongest reusable order source
for development scenarios, including entity/bullet registration chronology.
Verify that all accepted live enemies—including any deferred-activation Tanker
cargo—appear there exactly once before a zap can inspect them. Normal
Flipper-only Level 1 needs its separate stable Flipper order. Do not broaden
shot collision routing just to support Zapper targeting.

`TempestGame` should accept `input.ZapPressed` in the same active player-input
phase as firing/movement. Source does not require a HUD charge counter; do not
add one as part of this source-faithful milestone. Do not add a scenario flag:
exercise the same feature in all existing enemy modes.

### Timer processing and ordinary outcomes

Wire a common “advance zap timer or run normal update” check into each enemy
update loop. Preserve existing per-type loop order, player-shot-before-enemy
update order, player-collision-after-enemy-update order, enemy bullet handling,
and list cleanup. An entity that expires by zap is removed exactly once,
scored exactly once, removed from `developmentCollisionOrder`, and resolved
through its existing accounting route.

A normal shot that removes a pending target still uses the existing normal
score and bookkeeping path. A Spiker retreat and Tanker natural release that
occur before timer expiry retain their ordinary reserve/escaped outcomes and
do not earn a zap score. A player death before expiry clears the pending timer
with the discarded entity; the unresolved unit is reconstructed as a fresh
Egg/cargo Egg and receives no zap score. A display flash may outlast the
target’s ordinary resolution.

### Score and enemy accounting

- Normal/Spiker/Fuseball/Pulsar-mode zap expiry increments the existing
  `EnemiesDestroyed` once, awards that class’s source score, and removes that
  active enemy. For Spiker, the spike is not removed or counted.
- Tanker mode must not route Tanker or Tanker-cargo zaps through normal
  `CurrentLevel.EnemiesDestroyed`; preserve its original-slot/cargo ledger.
- If a pending-zap Tanker expires naturally first, resolve its original slot
  as escaped and materialize live cargo as the existing natural-release path
  does. If a player shot destroys it first, resolve as destroyed, award the
  ordinary 200, and create live cargo. Neither path also awards a later zap
  score.
- If the zap timer expires first, resolve its original slot as destroyed,
  award 200, and materialize exactly two new unresolved cargo IDs as stillborn
  Flippers. Their score/resolution occurs individually, only when each child
  reaches its own eight-update expiry (or ordinary shot outcome).
- Pending zap entities remain active/accounted until one of these existing
  resolution paths actually removes/resolves them. Merely applying a zap must
  not increment destroyed, escaped, or cargo-resolved counts.

## Tanker ledger: explicit conservation design

The current equations are the correct basis:

```text
original slot total = destroyed originals + escaped originals + unresolved originals
materialized cargo = resolved cargo + unresolved cargo
remaining cargo = unresolved materialized cargo + latent cargo
TankerAccountingTotal = 10 original units + materialized cargo units
```

In the current one-Tanker development scenario, before carrier release, its
slot remains unresolved and its two cargo units remain latent. The `Tanker`
object’s zap timer changes neither identity nor either count.

At Tanker zap expiry, in one game-owned resolution:

1. Remove the Tanker from its active list/order.
2. Resolve original slot 0 as **destroyed** exactly once.
3. Award 200 points exactly once.
4. Convert its two latent units into two distinct materialized IDs in
   `unresolvedCargoUnits`.
5. Create two distinct `Flipper.CreateTankerCargo` children at the source
   adjacent lanes/current Tanker height, with their ordinary initial
   `Flipping` state, lane direction/pivot geometry, and a fresh zap timer of
   8. Register each once in `flippers` and `developmentCollisionOrder`.
6. Leave both IDs unresolved until an ordinary player shot resolves that child
   (150 points), its stillborn timer expires (150 points), or a later life
   reconstruction re-enters it as an Egg. Never mark latent cargo resolved
   merely because the carrier died.

The original Tanker slot stays resolved after expiry; it must not return as a
Tanker Egg. This differs from death **before** its zap expires: on that death,
the Tanker is still an unresolved original slot with two latent passengers.
The current `ResetTankerScenarioAfterDeath` should capture that identity as
slot 0, discard the runtime object/timer, keep `TankerLatentCargoUnits == 2`,
and restore one slot-0 Egg—not two cargo Eggs.

For death **after** Tanker expiry but before one or both children resolve,
retain the destroyed original slot and its 200 points. Capture every still
unresolved cargo ID exactly once, discard its active child and remaining
stillborn timer, and restore each as an identity-bearing cargo Egg/pending
cargo unit. Those entrants hatch as fresh ordinary climbing cargo Flippers
under the current C# life-reconstruction policy; the expired stillborn effect
does not persist across death. A child already resolved and scored before
death is not restored. If one child is resolved and one remains pending, the
ledger therefore shows one resolved ID and one returning ID.

The conservation proof is the existing ledger invariant at each boundary:

- Before release, `10 = (destroyed + escaped + unresolved originals)` and
  both latent units are represented by the unresolved Tanker slot, so no
  materialized cargo is added.
- At zap expiry, moving slot 0 from unresolved to destroyed leaves the
  original total at 10; materializing two cargo IDs makes the accounting total
  12 and adds exactly two unresolved cargo units.
- At each child expiry/shot, move exactly one cargo ID from unresolved to
  resolved; total remains 12.
- At life loss, replace each unresolved runtime object with its same Egg
  identity. Do not add/remove ledger IDs. Resolved IDs and score remain
  resolved/retained.
- Natural escape before zap expiry resolves the original as escaped and
  materializes the same two cargo identities as live units; it does not add
  carrier-zap score.

The existing `TankerAccountingTotal`, `TankerCargoUnitsRemaining`,
`TankerLatentCargoUnits`, `PendingTankerCargoUnits`, and
`RestoreTankerEntries` concepts already support these states. Keep identity
sets authoritative; do not infer IDs from object counts or rebuild cargo from
the Tanker slot after it has resolved. Completion remains blocked while
`unresolvedCargoUnits` contains either child, including during its eight-tick
delay and after it has become a returning cargo Egg.

## Lifecycle, completion, Hurry-Up, flash, and audio

- **Player death — PROJECT DECISION.** The current C# game freezes enemies and
  the shared Pulsar clock for all 60 `PlayerDying` updates. Keep that freeze.
  Pending zap timers do not advance during the freeze. At life reconstruction,
  discard unresolved zapped enemy objects, retain their unresolved accounting,
  preserve earned score/charges, and recreate fresh Eggs. Tanker scenarios
  follow the slot/cargo cases above. Persistent spikes remain; enemy bullets
  clear as they do now.
- **Charges — PROJECT DECISION aligned to reference.** No charge reset in
  either ordinary-death reset path. Initialize two in `ResetLevel`, covering
  constructor/new game, Game Over restart, and Level Complete replay. Current
  project has no multi-level transition.
- **Completion — PROJECT DECISION.** Pending ordinary zapped enemies stay in
  their typed active list, and Tanker children stay unresolved in the ledger,
  so current completion checks naturally block until expiry or another
  ordinary resolution. At final zap expiry, score/accounting/removal happens
  before the final same-tick completion check. Do not copy Incitti’s separate
  all-on-rim completion behavior.
- **Hurry-Up — PROJECT DECISION.** Do not alter the post-Hurry-Up latch,
  enemy-specific effects, its normal Fuseball/Pulsar-only gate, or its
  death/replay lifecycle. Zaps neither trigger nor clear Hurry-Up and must not
  change its test outcomes.
- **Web/tube flash — REFERENCE VERIFIED.** First-use sets a 60-update
  `superzapperdisplay` unconditionally; the second use sets 60 only if it
  found a target. While positive, source line colors alternate between
  `COL_TANKERS` and `COL_LEVEL` using
  `(superzapperdisplay / 2) Mod 4 > 1`—two-tick halves in a four-update color
  period—on its tube/web line drawing. It does not indicate remaining
  charges. The counter is decremented once per level update and the source
  level update runs during the player-death presentation; model this visual
  timer independently from frozen enemy timers, decrementing once per game
  step while the simulation/display is still advancing. Reuse the current
  renderer’s blue level color and the existing Tanker/magenta color as the
  current palette approximation. Add headless draw-submission assertions for
  flash phase/duration; do not claim native framebuffer or arcade fidelity.
- **HUD — REFERENCE VERIFIED.** No numerical charge count or charge icons in
  the gameplay HUD. A conditional “SUPERZAPPER RECHARGE” level-begin message
  exists in source when entering a later level; it is not a per-life recharge
  or current charge gauge. With no C# progression, neither that message nor a
  charge counter is required.
- **Audio — REFERENCE VERIFIED but out of scope.** Activation has no required
  source sound hook in the inspected dispatch; each delayed enemy expiry calls
  the zap sound, and Tanker/cargo death may also use source effects. This does
  not determine gameplay/accounting. `AudioSystem` is currently empty; do not
  add audio as a prerequisite or make expiry semantics depend on it. Keep
  audio explicitly deferred.

## Expected implementation files and scope

Expected code/test changes after approval (none made now):

- `src/Tempest.Game/Systems/SuperZapperSystem.cs` — charges, source-order
  dispatch, exact single-target algorithm, display counter/state.
- `src/Tempest.Game/Systems/InputSystem.cs` — sample Space/right mouse press
  and add the injected `ZapPressed` input member.
- `src/Tempest.Game/Gameplay/Enemy.cs` — minimal default-reject zap contract
  and shared timer advancement.
- `src/Tempest.Game/Gameplay/Flipper.cs`,
  `Tanker.cs`, `Spiker.cs`, `Fuseball.cs`, `Pulsar.cs` — opt in to zap and
  expose existing ordinary update behavior to game-owned expiry resolution.
  `EnemyBullet.cs` should remain rejected by default; change it only if an
  explicit rejection override is clearer/testable.
- `src/Tempest.Game/TempestGame.cs` — inject/own the system, route active input,
  process timers before ordinary enemy updates, apply per-type scores/removal,
  resolve Tanker stillborn cargo IDs, and preserve reset/completion policy.
- `src/Tempest.Game/Rendering/VectorRenderer.cs` — source-pattern tube/web
  flash and optional headless line-color assertions; no charge counter.
- `tests/Tempest.Game.Tests/SimulationTests.cs` — deterministic source and
  accounting tests below.
- `README.md` and `TEMPEST-DEVELOPMENT-NOTES.md` — update only after the
  completed implementation and validation. No new CLI flag or project
  configuration is needed.
- `TEMPEST-BEHAVIOR-SPEC.md` — update only after implementation review and
  Neal’s manual validation, per the section below.

Do not refactor all enemy classes, replace the typed lists, alter ordinary
shot collision order, normalize all accounting into a new general system, add
normal multi-level progression, or introduce other carrier cargo types.

## Deterministic automated-test plan

Keep tests headless and inject `PlayerInput`/randomness as the current
`SimulationTests` suite does. Test direct system selection where necessary and
production `RunOneStep` behavior for integration:

1. **Charges and input:** initial count is two; Space and right mouse edge
   presses trigger once; held input does not repeat; no activation at zero
   charges or outside Playing. A strength-1 press with no candidate consumes
   its charge and does not start flash. A strength-2 press with no accepted
   target consumes its charge and does start flash.
2. **Lifecycle:** use one charge, lose a life with targets still pending,
   reconstruct, and assert the remaining charge is unchanged and no delayed
   score was awarded. Game Over restart resets to two; Level Complete replay
   resets to two while preserving the current score/lives policy. Constructor
   starts at two.
3. **All-target order:** provide mixed accepted enemy types, an interleaved
   bullet, and spikes. Assert the accepted enemies receive 4, 6, 8, ... in
   exact insertion order; bullet is visited/rejected and does not advance the
   delay; spikes are not targets. Include ordinary Level 1 creation order
   independently of its reverse shot-collision order.
4. **Second-target algorithm:** deterministic target tables for circular
   wrap distances, opposite-lane distance 8, non-continuous direct distance,
   strict `d < best_d + 2`, a candidate at `best_d+1` winning through higher
   type when not taller, `best_d+2` exclusion, a strictly taller candidate
   winning within the gate even when farther, strictly closer lower-height
   override, strict height tie, strict type tie, and stable insertion-order
   retention. Assert the selected target receives exactly delay 4. Include
   the source order dependence.
5. **Countdown/update order:** a first-use delay 4 remains active after the
   first three timer visits (activation tick included) and expires on visit
   four; check delay 6 similarly. Assert ordinary movement and state continue
   on non-expiry ticks, while movement does not run on the expiry tick.
   Verify player shots resolve first if they hit a pending target, with only
   ordinary score/accounting and no later zap score.
6. **Per-type score and accounting:** delayed Flipper 150, Tanker 200,
   Spiker 50, Pulsar 200, and Fuseball’s exact height-band formula at expiry.
   Each enemy remains unresolved until expiry, is removed/resolved exactly
   once, and blocks completion until then. Fuseball expiry creates one popup
   using expiry height/score with the existing 30-tick lifespan.
7. **Enemy-specific behavior:** Pulsars continue movement/pulse-lane behavior
   while pending and stop on expiry; Spiker zap removes only the Spiker and
   leaves its spike and spike collision intact; Flipper/Tanker/Fuseball normal
   behavior continues. A Spiker that retreats while pending resolves to
   reserve without a zap award.
8. **Rejected projectiles/hazards:** bullets remain active, moving,
   shootable/lethal under their existing rules and do not receive a timer or
   delay slot. Spikes remain persistent and unaffected by either strength.
9. **Tanker ledger:** zap-pending carrier keeps slot 0 unresolved and two
   latent cargo units. At carrier expiry, assert one original destroyed, two
   unique materialized child IDs, two initial-Flipping children with eight
   ticks, 200 points only, total ledger 12, and no standard `EnemiesDestroyed`
   mutation. At each child expiry, assert 150 once and one ID resolved. Test
   ordinary shot/natural-release pre-emption while a Tanker is pending.
10. **Tanker deaths:** death before carrier expiry restores exactly one
    unresolved slot-0 Egg with two latent units and no carrier score. Death
    after carrier expiry but before either child resolves restores both unique
    cargo IDs as Eggs, retains carrier score/destroyed slot, and awards no
    child score. Also test death after one of two children resolves: only the
    other ID returns. Repeat after re-entry to prove IDs do not duplicate or
    disappear; check the three ledger equations throughout.
11. **Completion:** final ordinary delayed enemy blocks exit to expiry;
    Tanker expiry alone does not permit exit while stillborn cargo remains;
    completion starts only after all unresolved cargo/eggs/original units
    resolve under the existing C# kill-all rules.
12. **Flash/rendering:** first-use flash even with no valid target; second-use
    flash only when a target is selected; exact 60-update decrement and
    two-tick/4-tick color pattern, including tick boundaries and death-state
    display timing. Assert normal tube geometry/color is unchanged when off.
13. **Hurry-Up/regression:** preserve existing trigger, `HurryUp` persistence,
    Fuseball/Pulsar behavior, movement, collision, death and reset tests.
    Re-run current normal Level 1, Spiker, Tanker, Fuseball, Pulsar, bullet,
    score-popup, exit/replay, and accounting regressions.

## Manual validation plan

Use the existing builds; no full progression or complete wave clear is
required. Extra lives are useful for repeatable observation. Scenario flags
remain mutually exclusive; `--enemy-bullets-test` composes only where its
existing implementation does.

```powershell
dotnet run --project src\Tempest.Game -- --lives 10
dotnet run --project src\Tempest.Game -- --spiker-test --lives 10
dotnet run --project src\Tempest.Game -- --tanker-test --lives 10
dotnet run --project src\Tempest.Game -- --fuseball-test --lives 10
dotnet run --project src\Tempest.Game -- --pulsar-test --lives 10
dotnet run --project src\Tempest.Game -- --enemy-bullets-test --lives 10
dotnet run --project src\Tempest.Game -- --spiker-test --enemy-bullets-test --lives 10
dotnet run --project src\Tempest.Game -- --tanker-test --enemy-bullets-test --lives 10
```

### Reasonably visible by eye

- In ordinary Level 1, Space/right mouse causes the first use to begin
  staggering Flipper removals, then the second use to remove one selected
  threat after its delay. The two uses are distinct; a third press has no
  effect until replay/restart.
- The source-style tube/web color flash appears after first use, and after
  successful second use. It does not appear for an unsuccessful second use.
- Spiker testing leaves its spike visible/hazardous after the Spiker is zapped.
- Tanker testing shows a delayed carrier zap, then two short-lived stillborn
  cargo Flippers rather than live children; the carrier and children disappear
  on different delays. Repeat with death before carrier expiry and death
  during child delay; unresolved units return without losing resolved score.
- Fuseball testing shows its normal movement during the pending delay and a
  score label when the delayed zap resolves it.
- Pulsar testing shows its existing pulse glyph/lane presentation while
  pending, followed by removal/score at expiry.
- Enemy-bullet combinations show bullets and spikes are not removed by either
  zap. The bullet scenario still permits shooting bullets as before.
- Ordinary life loss leaves the remaining charge count as-is (there is no
  source-style numeric counter, so infer by using the next charge); replay or
  Game Over restart restores both.
- No scenario exits while delayed targets/stillborn cargo remain. Existing
  normal Level 1, Spiker exit, hurry-up, score, lives, spinner, and reset
  behavior appears unchanged.

### Exact values for automated tests, not visual stopwatch claims

Manual play should not be used to certify exact 4/6/8 delays, 8-tick cargo
death, 60-step flash lifetime, the second-use comparison boundaries, strict
lane/height ties, 150/200/50 scores at every exact boundary, or the exact
Fuseball expiry-height score formula. Those are deterministic source/C#
simulation assertions. Manual play can confirm that a delay, score display,
and flash are perceptible and correctly associated.

### Atari comparison required before ARCADE VERIFIED

Compare first/all-use and second-target behavior, per-enemy timing/score,
movement during zap delay, Tanker stillborn cargo appearance/lifetime,
death/re-entry handling, charge persistence/recharge, charge input mapping,
flash appearance/duration, and any gameplay collision interactions against
Atari Tempest. Incitti-derived values remain **REFERENCE VERIFIED** until
Neal confirms them on the arcade. A successful artificial development scenario
is still **DEVELOPMENT ONLY** evidence for its composition.

## Behavior-specification impact after implementation and manual validation

Do not edit the spec during this analysis. After code review, automated tests,
and Neal’s arcade comparison, update only applicable parts of
`TEMPEST-BEHAVIOR-SPEC.md`:

- **Section 1 (evidence model):** retain the current authority order; label
  tests as C# behavior and source mechanics as reference only. No global
  evidence-policy change.
- **Section 2 (player controls, lives/death, completion):** record Space/right
  mouse activation, two charges, new-game/replay reset, ordinary-life
  persistence, pending-target freeze/reconstruction, and continued kill-all
  completion.
- **Section 3 (Flippers):** add delayed zap scoring/expiry and any relevant
  pending-zap movement or ordinary-shot pre-emption.
- **Section 4 (enemy bullets):** explicitly record that both Zapper strengths
  leave bullets unaffected.
- **Section 5 (Spikers/spikes):** document Spiker zap behavior and the
  independent persistent spike.
- **Section 6 (Tankers/cargo):** distinguish live shot/natural cargo release
  from zap-expiry stillborn cargo; document slot/cargo identity conservation
  and life reset.
- **Section 7 (Fuseballs):** add expiry-time score and score-popup behavior.
- **Section 8 (Pulsars):** add delayed zap behavior without changing the
  existing pulse/Hurry-Up rules.
- **Section 9 (rendering):** document the web/tube flash presentation, its
  evidence level, and the lack of a numeric charge HUD if relevant.
- **Section 10 (development scenarios):** note Zapper testing across existing
  scenarios and `--lives 10`; do not recast these artificial enemy waves as
  authentic composition.
- **Section 11 (known uncertainties/deferred behavior):** remove Super Zapper
  from deferred only after complete implementation; keep exact arcade
  correspondence unresolved until direct comparison.
- **Section 12 (validation history):** separate automated evidence, manual
  observation, and any direct Atari confirmation. Only the latter upgrades
  behavior to **ARCADE VERIFIED**.

README and development notes should describe the feature only after the
implementation has passed the project’s normal review and validation. Do not
record source facts as arcade facts.

## Risks, uncertainties, and explicit out-of-scope

### Risks and uncertainties

1. **Mutable list and child timing:** Incitti appends Tanker children during
   baddie update. Exact same-pass update timing is not established by static
   inspection. The recommendation follows current C# deferred activation;
   record it as a project policy and compare manually.
2. **Selection is order-dependent:** strict comparisons and the `best_d + 2`
   gate produce unintuitive ties; a “closest/highest” rewrite will fail source
   parity.
3. **Normal resolution can pre-empt zap:** shots occur first; Spiker retreat
   and Tanker natural release can occur during countdown. Their normal outcome
   must win rather than double-scoring.
4. **Tanker ledger has two distinct identities:** an unresolved carrier slot
   with latent cargo is not equivalent to a resolved carrier plus materialized
   cargo IDs. Zap expiry is a transfer boundary that must be atomic.
5. **Death clears pending state, not the unit:** preserve charges and score,
   discard timer/entity state, and reconstruct unresolved IDs using current
   C# fresh-Egg policy.
6. **First-use target iteration includes rejected bullets:** omitting bullets
   is score/delay-equivalent only if rejection cannot advance the accepted
   delay; tests should pin actual call/result semantics. Never pass spikes.
7. **Flash is independent of target survival:** first-use flash is unconditional;
   second-use flash requires a selected target. A pending target can be shot,
   retreat, escape, or be cleared by life loss while visual flash continues.
8. **Arcade evidence is absent:** all described exact constants come from
   Incitti/current C# mechanics, not Atari observation.

### Explicitly out of scope

- New levels, level progression, board geometry, enemy eligibility, authentic
  wave composition, or a full-life progression loop.
- New enemy types, Tanker-carried Pulsars/Fuseballs, generalized cargo, or a
  broad enemy/accounting hierarchy rewrite.
- Changes to normal Flipper/Tanker/Spiker/Fuseball/Pulsar movement other than
  allowing their existing updates during zap delay.
- Changes to enemy bullets, spikes, ordinary shot priority, death duration,
  the C# death-freeze policy, Hurry-Up, score bonuses/lives, or kill-all exit.
- Charge HUD icons/counters, new gameplay flags, audio implementation,
  explosions/death animation, or new sound dependencies.
- Promoting Incitti-derived behavior to **ARCADE VERIFIED** without Neal’s
  direct Atari comparison.
- Any gameplay implementation, tests, tracked documentation edits, project
  changes, or commit in this analysis task.
