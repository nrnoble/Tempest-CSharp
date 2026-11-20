# Post-Super-Zapper Arcade Findings Analysis

## Scope and evidence

This is an analysis report only. The Super Zapper milestone, working tree,
tests, behavior specification, development notes, README, and project files were
not changed. No Super Zapper regression was observed in the five manual
scenarios, and this investigation does not reopen that checkpoint.

Evidence labels follow [the behavior specification](../../TEMPEST-BEHAVIOR-SPEC.md):
Neal's direct arcade observations are **ARCADE VERIFIED**; literal Incitti
behavior is **REFERENCE VERIFIED** only; current C# behavior is evidence about
this recreation, not by itself about the arcade. The Tanker test remains
**DEVELOPMENT ONLY**.

## Finding 1 — all remaining enemies on the rim

### Incitti source behavior

`Baddies.UpdateBaddies` resets `onrim` to true, updates each baddie, then sets
`onrim` false if a baddie with `baddietype > BAD_SPIKE` has `height < 400`.
The comparison is after that enemy's update. With the source constants,
`BAD_SPIKE` is 1, `BAD_BULLET` is -1, and enemy types begin at 2. Thus the
predicate is effectively: **every currently listed enemy-type baddie has
height at least 400**. It is not a test of the enemy's animation/state, lane,
or whether it was shot. See
[UpdateBaddies](../../Reference/incitti/tempest.bmx#L2183) and the
[baddie-type constants](../../Reference/incitti/tempest.bmx#L116).

The exit decision separately requires both `eggsleft = 0` and an empty
`EGG_LIST`. Only then does `enemiesleft = 0 Or onrim = True` start the exit
path. If neither condition is met, it latches `rimit = True`. This check
follows Egg and baddie updates in the main loop, so a ready Egg that has not
yet hatched still prevents exit. See the
[main update order](../../Reference/incitti/tempest.bmx#L3749),
[completion check](../../Reference/incitti/tempest.bmx#L3811), and
[Pulsar rimit behavior](../../Reference/incitti/tempest.bmx#L3067),
[Fuseball rimit behavior](../../Reference/incitti/tempest.bmx#L3393), and
[new-level reset](../../Reference/incitti/tempest.bmx#L4253).

The rim predicate is deliberately broad and height-only:

| Source object/state | Counts toward `onrim`? | Qualification |
|---|---:|---|
| Flipper climbing, paused, or pivoting | Yes | Its post-update height must be at least 400. A pivot or pause does not disqualify it. |
| Pulsar in any state, including descending, pausing, or flipping | Yes | Height only; at 400 it qualifies even if its state would later descend. |
| Fuseball in any state or sub-lane position | Yes | Height only; lateral position and action do not disqualify it. |
| Tanker | Yes | Height only while it remains listed. Normally it releases cargo and removes itself after crossing `depth - 15` (385), before it can itself qualify at 400. |
| Spinner/Spiker enemy | Yes | This is an enemy type (`BAD_SPINNER = 2`), distinct from the spike hazard. Its current height is tested regardless of build/retreat state. |
| Actual spike hazard | No | `BAD_SPIKE = 1`, excluded by the strict `> BAD_SPIKE` test; spikes persist for the spike-avoidance route. |
| Enemy bullet | No | `BAD_BULLET = -1`, excluded. |
| Reserve or active Egg | Not part of `onrim` | Independently blocks the decision until reserve and `EGG_LIST` are both empty. |

Consequently, `Height == Level.Depth` is not a complete porting rule by itself:
the source uses the literal threshold `height < 400`, so heights above the rim
also qualify, and only listed enemy baddies participate. More importantly,
Tanker cargo is not tested while latent inside its carrier. The carrier itself
blocks the test while below the rim; when it releases, the new Flippers enter
the baddie list at the carrier height. Any child below 400 then blocks exit.
Natural Tanker release is strictly after updated height exceeds 385; shot
release is immediate at the Tanker's current height. See
[Tanker.ReleaseCargo/Update](../../Reference/incitti/tempest.bmx#L2467).

### Removal, spikes, pending targets, and accounting

Incitti does not explicitly remove or destroy all rim enemies when the
predicate first becomes true. It transitions to `LEVEL_AVOID_SPIKES` on the
specified boards, or `LEVEL_START_ZOOM` otherwise. Baddies continue their
ordinary updates during spike avoidance and start-zoom; only once the level is
actually `LEVEL_ZOOMING` or `LEVEL_REVERSE_ZOOM` does `UpdateBaddies` update
only spikes, and `DrawBaddies` draw only spikes. The remaining enemies are
therefore hidden/frozen for the zoom rather than being kill-scored at the
all-rim decision. They remain in `BADDIE_LIST` until successful level
completion reaches `ClearLevel`, which deletes baddies without scoring them.
A spike collision can reverse the zoom and call
`ConvertBaddieToEggs`, which removes live non-spike baddies and returns
survivors to reserve. See
[state transitions](../../Reference/incitti/tempest.bmx#L1344),
[zoom update/render filtering](../../Reference/incitti/tempest.bmx#L2183),
[death conversion](../../Reference/incitti/tempest.bmx#L2224), and
[ClearLevel](../../Reference/incitti/tempest.bmx#L4137).

`onrim` itself does not inspect `dietime`. A pending zap target below the rim
still makes `onrim` false. A pending target at or above 400 qualifies, but
whether its delayed zap scores depends on whether it expires during the
avoid-spikes/start-zoom updates before actual zooming freezes baddies. It must
not be awarded a kill merely because it was later hidden as an all-rim
survivor. The same timing applies to stillborn Tanker cargo: `ReleaseCargo(True)`
creates Flippers with an eight-tick death timer; they block while below 400,
and at the rim their timer can resolve during the pre-zoom states. See
[stillborn cargo creation](../../Reference/incitti/tempest.bmx#L2467) and
[baddie update filtering](../../Reference/incitti/tempest.bmx#L2183).

The source's `enemiesleft` is an active-count mechanism, not a conserved
destroyed-total ledger. Natural Tanker release removes one carrier and adds
its cargo Flippers; shot destruction scores/removes the carrier and also
releases cargo. Neither `onrim` nor the exit condition awards score or
increments a destroyed count for survivors that simply stop being shown.
The source does not preserve per-enemy slot identities or a fixed total score
accounting through this transition.

### Current C# behavior and comparison

The normal C# Level uses a fixed ten-unit conservation ledger, and
`Level.IsComplete` requires `EnemiesDestroyed == EnemyTotal`, no reserve or
active Eggs, and no active Flippers. `TempestGame.CheckForLevelComplete`
likewise requires every original Tanker-mode slot and materialized cargo unit
to be resolved. These are explicit current project rules, not an accidental
translation of `onrim`. See
[Level.IsComplete](../../src/Tempest.Game/Levels/Level.cs#L234) and
[CheckForLevelComplete](../../src/Tempest.Game/TempestGame.cs#L882).

Therefore Neal's observation—after reserve and active Eggs are gone, if every
remaining enemy reaches the rim the survivors disappear and exit begins—is a
new **ARCADE VERIFIED** completion behavior that differs from the current
kill-all **PROJECT DECISION**. It should not be modeled by incrementing
`EnemiesDestroyed`, awarding kill score, or merely weakening the completion
predicate while leaving unresolved identities behind.

### Safe incorporation constraints

This can be implemented within the existing game-owned accounting design,
without an enemy-hierarchy rewrite, but it is a cross-cutting completion and
ledger change rather than a predicate-only edit:

1. Keep kill destruction separate from non-kill resolution. Add an explicit
   rim-cleared/escaped disposition (or equivalent resolved-but-not-destroyed
   count) so ordinary completion can recognize either kills or rim clears
   without changing `EnemiesDestroyed` or score.
2. In Tanker mode, preserve original-slot and cargo IDs and distinguish
   rim-cleared from shot/zap destruction and natural carrier escape. A cargo
   ID is resolved once only when its child is actually cleared; do not silently
   count latent cargo as destroyed or lose a carrier's identity.
3. Include latent Tanker cargo through its active Tanker carrier. Include
   released cargo, including children pending their stillborn timer, in the
   height test. A below-rim Tanker or child must block exit.
4. Pending Super Zapper targets that expire before the all-rim decision retain
   their existing kill score and destroyed accounting. Targets that remain
   pending when the field is finally cleared must be resolved as non-kills,
   with no later timer expiry, score, or duplicate accounting.
5. Do not finalize rim-clear accounting when entering spike avoidance or a
   reversible exit phase. Incitti continues updating enemies before actual
   zoom and can reconstruct them as Eggs if a spike reverses the exit. Preserve
   unresolved identities through that boundary; commit them as rim-cleared
   only when the exit can no longer be reversed / the level completes.
   The recreation may retain its established death reconstruction policy, but
   all survivors must re-enter exactly once if that policy rolls the exit back.
6. Apply the Egg exhaustion gate independently and keep `rimit`/Hurry-Up a
   behavior acceleration, never a completion or resolution event. The final
   rim test must not make Hurry-Up score, resolve, or remove anything.
7. Actual spikes remain outside the rim test and persist through spike exit.
   The enemy that builds/retreats as a Spinner/Spiker is still an enemy and
   must participate in the height test.

The current C# Tanker ledger already separates destroyed, escaped, unresolved
original slots, and unresolved/resolved cargo identities, so it provides a
usable foundation. It would need another explicit resolution outcome and an
exit-commit boundary; it does not require changing identity ownership or
Super Zapper target dispatch. See the
[Tanker ledger and reset paths](../../src/Tempest.Game/TempestGame.cs#L84)
and [spike/reverse-exit paths](../../src/Tempest.Game/TempestGame.cs#L966).

## Finding 2 — Tanker-cargo Flippers follow the Blaster mid-tube

### Current C# trace and exact cause

There is no current `tankerCargoInitialPivotPending` flag or
`CompleteTankerCargoReleaseTransition` method in the checked-in C# source. The
current path is direct:

1. `Tanker.ReleaseFlipperCargo` chooses deterministic opposite adjacent source
   lanes, constructs each spawn's initial source/destination/direction, and
   retains the release height.
2. `TempestGame.ReleaseTankerCargo` assigns a cargo ID and calls
   `Flipper.CreateTankerCargo`.
3. That factory initializes `SourceEdge`, `DestinationEdge`, `Direction`,
   `TotalTurnAngle`, and `State = Flipping`; this makes the initial pivot
   mandatory. It does not initialize an ordinary climbing Flipper.
4. Each `Flipper.Update` in `Flipping` advances the turn and, while below the
   rim, also increases Height. When the initial pivot completes below the rim,
   `CompletePivot(playerEdge)` cannot report a lethal collision (the collision
   branch is gated by `Height >= Level.Depth`), so it calls `BeginPause`.
5. After that pause, `BeginPivot(level, playerEdge)` always calls
   `Level.FindShortestDirection(EdgeIndex, playerEdge)`. It has no cargo or
   height guard. Each later pivot below the rim repeats this cycle, and
   changing `Player.EdgeIndex` before a subsequent pivot changes its direction.

The exact cause is the shared C# `Pausing -> BeginPivot` path recalculating
toward the player at every pivot start, including cargo pauses below the rim.
The initial cargo pivot is not the cause of the pursuit. See
[Tanker release](../../src/Tempest.Game/Gameplay/Tanker.cs#L132),
[cargo factory and update](../../src/Tempest.Game/Gameplay/Flipper.cs#L60),
[BeginPivot](../../src/Tempest.Game/Gameplay/Flipper.cs#L175),
[CompletePivot](../../src/Tempest.Game/Gameplay/Flipper.cs#L187), and
[game-owned release](../../src/Tempest.Game/TempestGame.cs#L739).

There is also a separate initial-direction discrepancy in the current
release code that should be resolved as part of the same cargo geometry
correction. In Incitti, `ReleaseCargo` steps a child to an adjacent lane, sets
its initial Flipper direction to the *opposite* of the side used to place it,
and creates it as `typ = 1`. In source Flipper's sign convention,
`dir = -1` steps to the next edge and `dir = +1` to the previous edge when
the delayed transition applies its lane step. Thus the mandatory first pivot
from either adjacent lane is toward the Tanker lane; subsequent steps proceed
away from the carrier in the same fixed direction. Current C# uses the
placement-side sign as `Direction` and sets `DestinationLane` to the next
outward lane, so its initial pivot is outward instead. The current tests and
implementation report codify that outward destination, but that is not the
literal `ReleaseCargo`/`typ=1` path. Compare
[Incitti ReleaseCargo](../../Reference/incitti/tempest.bmx#L2467),
[source Flipper state transitions](../../Reference/incitti/tempest.bmx#L2748),
[source Flipper creation](../../Reference/incitti/tempest.bmx#L2924),
[current Tanker spawn geometry](../../src/Tempest.Game/Gameplay/Tanker.cs#L138),
and [current geometry tests](../../tests/Tempest.Game.Tests/SimulationTests.cs#L2207).
This is a source comparison; whether the initial inward pivot is also what the
arcade displays should be confirmed in manual play before labeling it
**ARCADE VERIFIED**.

### Literal Incitti behavior, `canflip`, and progression

Incitti creates Tanker-released Flippers with `typ = 1` (`flipping`) at the
Tanker's height. Their initial pivot is mandatory regardless of `canflip`.
While `typ = 1`, every pivot adds the ordinary Flipper climb increment while
Height is below 400. Completing a pivot below 400 sets `oldtyp = 1`, enters
the lane-change pause, and on pause completion moves one edge in the existing
direction. Crucially, that pause only recalculates `dir` toward
`MainPlayer.e_Index` when `height >= depth`; below 400 it retains its previous
direction. The child therefore continues its prescribed directional sequence
up the tube but does **not** retarget the player below the rim. Once the rim is
reached, subsequent pause completion recalculates toward the player.
See [ReleaseCargo](../../Reference/incitti/tempest.bmx#L2467) and the
[Flipper update state machine](../../Reference/incitti/tempest.bmx#L2732).

`canflip` is level-dependent: `SetUpEnemies` sets it false only for
zero-based `lv = 0` and true for later levels. However, the `canflip` check is
in the ordinary `typ = 0` sliding-up branch, where it enables a normal
Flipper's mid-tube transition into flipping. Tanker cargo is born in `typ = 1`,
so `canflip` does not enable its direction to track the player below the rim.
At the rim, the separate `height >= depth` direction recalculation applies.
See the [`canflip` gate](../../Reference/incitti/tempest.bmx#L2760) and
[level setup](../../Reference/incitti/tempest.bmx#L4266).

The first Tanker eligibility is `current_board > 1 || lv > 17`: on the normal
first set, that is zero-based `lv = 2`, displayed Level 3. Its Tanker cargo
choice is two Flippers, and `canflip` is already true at that level. See
[Egg.Hatch Tanker selection](../../Reference/incitti/tempest.bmx#L2035) and
[Tanker progression flag](../../Reference/incitti/tempest.bmx#L4236).
The `--tanker-test` C# mode deliberately runs a later-level Tanker on Level 1
geometry and Level 1 enemy tuning. That is a development-scenario mix, but it
does **not** explain mid-tube player pursuit: Incitti's Tanker cargo remains
non-pursuing below the rim even at its first eligible level. Normal Flipper
behavior is different: at Level 1, `canflip` is false and ordinary Flippers
climb without changing lanes; they begin rim pursuit after reaching the top.
This agrees with Neal's Level 1 observation.

### Arcade comparison

Neal's observation that released Tanker cargo does not track Blaster movement
mid-tube agrees with the literal Incitti cargo state machine. Incitti does not
appear to disagree with this particular arcade observation. Current C# pursuit
is a **C# translation bug**, not a necessary consequence of `canflip` or a
reason to wait for progression. The separate current initial-pivot direction
also differs from the source and merits direct arcade confirmation.

The correction is local to cargo pivot direction selection/state setup:
preserve the mandatory release pivot and cargo identity, preserve the
release-side direction across all below-rim pivots, and choose a new
player-directed pivot only once rim behavior begins. Do not change ordinary
Flipper pursuit. The initial lane/destination/direction should follow the
literal source in tests; keep the arcade uncertainty explicit until observed.
Super Zapper stillborn timers and cargo IDs are in the same entity path, so add
targeted regression coverage for them without reopening the already-passed
five-scenario Super Zapper checkpoint.

## Architectural assessment and classification

| Finding | Classification | Architectural assessment |
|---|---|---|
| All-rim completion | **B — previously deferred arcade behavior** | Current kill-all behavior is an explicit project policy, now contradicted by a direct arcade observation. The architecture is adequate, but completion and accounting need an explicit non-kill resolution disposition and a reversible-exit boundary. A predicate-only change would leave accounting, Tanker identities, pending zaps, and spike re-entry inconsistent. |
| Tanker-cargo pursuit | **A — C# translation bug**, plus a source-vs-arcade uncertainty on initial pivot geometry | The pursuit cause is a local unconditional `BeginPivot` target choice. Correcting it need not alter the entity hierarchy or ordinary Flippers. The present `--tanker-test` has later-level Tanker composition on Level 1 geometry (**D**), but that is not the cause of the pursuit because source cargo does not use `canflip` for below-rim target selection. |

Neither finding reveals a fundamental architectural failure. Finding 1
touches the central ledger because the existing normal completion invariant is
kill-only, but the project already has game-owned resolution paths and a
separate Tanker ID ledger. Finding 2 is a contained Flipper cargo-state issue;
the most important related test correction is that existing expected initial
geometry currently follows the C# implementation rather than the literal
source.

## Recommended sequencing relative to Super Zapper

Recommend **option 3: correct Tanker-cargo behavior now**, including the
source-backed fixed below-rim direction and the initial pivot geometry
comparison, while leaving ordinary Flippers and Super Zapper behavior
unchanged. This is a narrow correction directly supported by source and
Neal's observation. Run focused cargo and stillborn-cargo tests; do not repeat
the five full manual Super Zapper validations unless that focused regression
coverage finds an interaction.

Defer the all-rim completion change to its own accounting/exit milestone.
It is a real arcade behavior, not a reason to preserve kill-all forever, but
safe implementation requires a deliberate outcome ledger and spike/death
boundary. Do not fold it into the Super Zapper checkpoint or update the
behavior specification before implementation and acceptance of those rules.

## Proposed deterministic tests

### All-rim completion

- With reserve Eggs remaining, exit is blocked even if every active enemy is
  at the rim. With no reserve but any active Egg—including a ready-to-hatch
  Egg—exit remains blocked.
- Exhaust both Egg sources, leave one active Flipper below 400, and assert it
  blocks exit. Move the final survivor to 400 (and separately above 400) and
  assert rim eligibility occurs only after its update.
- Cover paused and pivoting Flippers, climbing/descending/paused Pulsars,
  Fuseballs with different lateral positions, and Spiker enemy states. Assert
  state is not itself a disqualifier. A real Spike and an enemy bullet do not
  block; a below-rim Spinner/Spiker enemy does.
- Rim-cleared survivors produce no score, do not increment
  `EnemiesDestroyed`, and resolve exactly once through the separate rim-clear
  disposition. Repeated exit checks must not duplicate resolution.
- For Tanker mode, an unreleased below-rim Tanker blocks while carrying latent
  cargo. After natural/shot release, each below-rim child blocks. Resolve the
  original Tanker as escaped or destroyed according to its actual release
  cause; preserve both cargo IDs. On rim clear, resolve children without
  kill score and conserve the original/cargo ledger totals.
- For a pending Tanker zap that releases stillborn cargo, test below-rim
  children block. Test a timer that expires before clear keeps ordinary zap
  score, and a still-pending timer at committed clear produces no score and no
  second resolution.
- Exercise a pending Super Zapper target below the rim (blocks), then at the
  rim (qualifies); verify an earlier zap expiry scores exactly once and an
  unexpired target cleared by exit is not later scored.
- In spike mode, retain spikes and choose the existing spike-avoidance path.
  Do not commit rim-clear accounting merely on entering AvoidSpikes or a
  reversible exit. Force a spike collision during avoidance/exit and verify
  survivor identities return to reserve/Eggs once; then complete a later
  successful exit and verify no unit is recreated.
- Verify Hurry-Up remains a latch/acceleration only: no early exit while an
  enemy remains below rim, no score or accounting change when it latches, and
  all-rim remains gated by exhausted reserve and active Eggs.
- Cause player death immediately before the final rim decision and during a
  reversible spike exit. Confirm death/re-entry preserves unresolved IDs and
  does not duplicate or lose units; a completed irreversible exit resolves
  each survivor once.

### Tanker cargo Flippers

- Normal Flippers remain unchanged: Level 1 climbing never changes lanes in
  response to player movement, and rim pursuit still selects the player lane.
- Assert the Tanker release side, source lane, initial destination, direction,
  height, mandatory Flipping state, angle, and opposite-child identity against
  Incitti's `ReleaseCargo` and `typ = 1` transition. Include wraparound
  lanes. Keep the deterministic side choice as a project test seam, not an
  arcade claim.
- Complete the initial pivot below the rim. Then move `playerEdge` repeatedly
  while the cargo continues its below-rim pivots. Assert its source direction
  and lane progression remain fixed and do not react to those player moves.
  Verify the source's pause/lane-step timing and ongoing climb between pivots.
- Drive the cargo to rim height, move the player, and assert subsequent
  rim-behavior pivot choices pursue the current player lane.
- Verify cargo ID and original Tanker resolution are unchanged by the
  correction. Check natural release, player-shot release, delayed
  Super-Zapper release/stillborn expiry, and player death/re-entry; no child
  is duplicated, lost, or awarded a kill score merely by the behavior change.

## Behavior-spec changes that would eventually be needed

After implementation and validation, revise the completion section to
distinguish the arcade-observed all-rim exit from kill-all completion, state
the Egg exhaustion gate and height-based enemy rule, and document that rim
survivors are not kill-scored. Clarify spike avoidance/reverse-exit and
death/re-entry handling, including the point at which non-kill accounting is
committed. Keep the current fixed ten-unit conservation rule labeled as a
project/Tanker-ledger concern rather than implying it describes the arcade.

Revise Flipper/Tanker sections to distinguish ordinary Flippers from
Tanker-released Flippers: cargo is created in the initial pivot state, keeps
its release direction below the rim, and tracks the Blaster only after rim
behavior begins. Record `canflip` as a progression flag for ordinary
Flippers, not as the explanation for Tanker-cargo pursuit. Record initial
release-pivot geometry as source-verified and leave its arcade status open
until Neal observes it.

## Remaining arcade uncertainties

- Whether the arcade's all-rim predicate uses the same exact height boundary
  as Incitti's `< 400` test for every enemy state, including Pulsar/Fuseball
  pauses, Spiker/Spinner retreat, and Tanker release timing.
- Whether a pending zap or stillborn cargo can finish scoring during the
  arcade's spike-avoidance/start-zoom interval, and what is displayed if its
  timer is still pending when zoom begins.
- Whether a spike reversal after the all-rim trigger restores all hidden
  survivors in the arcade exactly as Incitti's `ConvertBaddieToEggs` does.
- Whether Atari Tanker cargo's initial pivot passes inward through the
  Tanker's lane as the literal source indicates, or whether the arcade uses
  different lane geometry/direction.
- Whether Tanker cargo continues climbing and pivoting in a fixed direction
  below the rim at the same cadence as Incitti. Neal's report establishes no
  player-responsive mid-tube pursuit, but does not yet establish every
  intermediate lane transition or exact timing.
- Whether Incitti's Tanker eligibility (first displayed Level 3) and
  `canflip` progression exactly match arcade progression. The Level 1
  development scenario is not evidence for that progression.
