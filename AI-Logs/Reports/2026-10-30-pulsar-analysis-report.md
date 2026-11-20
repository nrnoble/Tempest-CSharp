# Direct-Hatched Pulsar Milestone Analysis

**Checkpoint:** `b3bd0d4 Implement direct-hatched Fuseball behavior`
**Scope:** Analysis only. No gameplay, test, README, project, or configuration files changed.
**Conclusion:** A direct-only `--pulsar-test` wave is the smallest useful next milestone. It can reuse the current ten-unit Egg wave and existing development-enemy infrastructure; it does not require normal level progression, Tanker cargo, or Super Zapper.

## 1. Evidence labels and authority

- **[Incitti]** means the behavior is directly expressed in `Reference/incitti/tempest.bmx`. This establishes what this particular reference implementation does, not what the Atari arcade necessarily does.
- **[C# inference]** means a design recommendation based on the current C# implementation and its tested conventions.
- **[Arcade check]** means automated tests cannot establish the real arcade behavior; compare the implementation with Atari Tempest during manual gameplay.

The project guidance and development notes put observed arcade behavior above Incitti, then the current tested C# implementation, then assumptions. The Incitti Readme identifies the reference as Tempest 3.1 (April 2008) and documents that its author changed behavior across versions; it is not an arcade-accuracy guarantee.

## 2. Source behavior

### Creation and eligibility

- **[Incitti]** `Egg.Hatch` can select type 2 (Pulsar) only when `theLevel.haspulsars` is true. `Pulsar.Create(e_Index, 0, 0)` attaches the Pulsar to that edge, starts it at height 0/type 0, adds it to the baddie list, and increments `enemiesleft` (`tempest.bmx:2014-2019`, `2943-2948`, `3254-3265`).
- **[Incitti]** `SetUpEnemies` sets `lv = current_level` and enables direct Pulsars only when `lv > 15`. Since the displayed level is `current_level + 1`, the first eligible displayed level is 17. Eligibility means the normal hatch chooser may select one; it does not mean every hatch is a Pulsar (`tempest.bmx:137`, `4210-4240`, `PickEnemy`/`Hatch` around `1937-2020`).
- **[Incitti]** Tanker-carried Pulsars are a separate later path: Tanker Pulsar cargo is enabled when `lv > 26` and is released by Tanker code. It is not required for direct hatching (`tempest.bmx:2046-2051`, `2522-2546`, `4258-4260`).
- **[C# inference]** A test wave that hatches all ten existing ordinary enemy units as Pulsars is an explicitly artificial composition, like `--fuseball-test`; it exercises the enemy without asserting normal board composition or progression.

### Movement states and transitions

`typ` is a numeric Incitti state, not a C# enum. The reachable direct-hatch path starts at 0. `oldtyp` records the movement state to resume after a pause.

| Incitti `typ` | Meaning and behavior | Transition details |
|---|---|---|
| `0` | Sliding/climbing up. Height increases by exactly 1 per update until `Depth`. | While below the rim, `timetoflip > 50` rolls once: a 1 enters state 3 with `oldtyp=1` (mid-tube pause, then lane change/flip); a 0 enters state 5 with `oldtyp=0` (rim-style pause on the same edge). At the rim, another random roll chooses state 3/`oldtyp=1`, or state 5/`oldtyp=4` to begin descending after the pause. |
| `4` | Sliding/descending. Height decreases by exactly 1 per update until 0. | The `>50` mid-tube decision is analogous to state 0, with a 0 entering state 5/`oldtyp=4`. At height 0, a 1 enters state 3/`oldtyp=1`; a 0 enters state 5/`oldtyp=0` to resume climbing. |
| `3` | Mid-tube delay before changing to an adjacent edge. Height is held. | Decrement `pause`; when it reaches 0, restore `oldtyp`, reverse direction if an open tube would run past an endpoint, step one edge, replace `OnEdge`, and recompute the turn angle. The lane change occurs on the transition out of the pause, not at pause entry. |
| `5` | Rim-style delay without an edge change. Height is held. | Decrement `pause`; when it reaches 0, reset `timetoflip` and resume `oldtyp`. |
| `1` | Angular flip while moving upward. | Advance `angle` by `pulseflipspeed`; increase height by 1 while below `Depth`; when angle reaches `totangle`, reset the angle and enter state 5 with `oldtyp=0`. |
| `2` | Alternate angular-flip branch. It rotates like state 1 but does not increment height. | The inspected direct creation and transition code never assigns state 2; it is referenced by draw/collision/flip code but appears unreachable in this path. Do not invent a normal transition into it without more evidence. |
| `7` | Terminal “killing player” state. Its update branch does not move. | Near-rim player collision sets it. It is not a destruction/removal state; life-loss conversion later removes the Pulsar. The draw switch has no geometry case for 7, so copying this detail literally may produce a degenerate draw during death. |
| `6` | No Pulsar behavior found. | Not used in the inspected Pulsar state machine. |

**[Incitti] Timing and direction**

- `timetoflip` increments before the state switch. The strict `> 50` test therefore takes the decision on the 51st update after its counter was reset. It is reset on that decision and when a pause expires.
- Up/down speed is one height unit per update (the C# simulation is 60 Hz). Default global flip speed is 15 degrees, but `SetUpEnemies` sets `pulseflipspeed = 10 + lv/8`. For the first eligible source level (`lv=16`), this is 12 degrees/update. A regular 16-edge turn is 22.5 degrees, so this takes roughly two updates to pass the angle threshold, subject to update ordering and the edge's actual angle.
- At `lv=16`, the source pause expressions are `20 - lv/6` and `16 - lv/6`. Both are assigned to integer fields; confirm BlitzMax's division/assignment conversion before translating them into exact tick counts. Do not silently substitute 18/14. Values at level 0 (10-degree flip, source expressions based on 20/16) would also differ from the first eligible normal Pulsar level.
- At each state-0/state-4 update, `FindShortDir` re-evaluates direction toward the player's edge. Closed tubes use the shorter wrapped route; open tubes use direct index direction. Same-edge ties use `Rand(0,1)`. Thus random calls can occur on successive vertical updates while Pulsar and player share an edge, not just at pause/boundary transitions.
- Transition rolls use `Rand(0,1)` at the `>50` threshold and at the top/bottom. With the source default `pulsespeed=1`, each call to the global pulse updater advances one phase count.

**[C# inference]** Keep this state machine explicit in `Pulsar.cs`; pass the current `Level`, player edge, and the existing inclusive random delegate into update logic. Do not make `Pulsar` own a second `Random` or global pulse clock. Give the development wave fixed parameters corresponding to `lv=16` if matching first eligibility is the goal, after resolving the pause-field conversion; record clearly that applying those values to current Level 1 geometry is a development setup, not natural progression or verified arcade tuning.

### Closed/open edge movement

- **[Incitti]** A closed tube wraps at both ends. An open tube reverses `dir` if the next edge would be out of range, then takes a step without wrapping (`tempest.bmx:3139-3150`).
- **[Incitti]** In a closed tube `FindShortDir` chooses the shortest wrapped direction and randomly breaks equal/same-edge ties. In an open tube it uses the direct index direction and randomly breaks the same-edge tie (`tempest.bmx:3987-4000`).
- **[C# inference]** The existing `Level.NextEdge`/`PreviousEdge` helpers always wrap, and the current renderer always closes the tube. Pulsar movement can retain the source's open-endpoint decision for model tests, but a visually accurate open-board Pulsar is not deliverable until the project has open-tube geometry and rendering. The first manual scenario should stay on the existing closed Level 1 tube.

## 3. Global pulse timing and presentation

### Oscillator

- **[Incitti]** `pulsecount`, `pulsespeed`, `pulsing`, `pulse_zh`, and `pulsesalive` are globals shared by all Pulsars. The game loop calls `Pulsar.UpdatePulseTimers` once per loop before baddie updates (`tempest.bmx:176-180`, `3740-3754`).
- The updater adds `pulsespeed`, resets the count to 0 only when it becomes greater than 180, and sets `pulsing` only for `pulsecount > 70 && pulsecount < 110` (counts 71 through 109 inclusive). With defaults, the period is 181 update calls, not 180.
- `pulse_zh = Abs((pulsecount Mod 90)-45)/4.0`, then values below 2 are forced to 0 and values above 12 are clamped to 12. With the default integer phase range the computed maximum is 11.25, so the upper clamp is not reached. This is a shared screen-space zigzag amplitude, not each Pulsar's height.
- At exactly phase 71 a pulse sound is attempted only if `pulsesalive` was true on the preceding update. The timer then clears `pulsesalive`; each surviving Pulsar sets it true during its subsequent update. Audio is not implemented in the C# project and is not required to test the visual/collision timing.

**[C# inference]** Store one oscillator in the simulation owner (`TempestGame`), expose read-only phase/active/amplitude/color-phase values to the renderer, and advance it once per simulation tick before Pulsars update. Do not duplicate counters per enemy. Decide and test its behavior during the C# death freeze: Incitti advances the global timer and updates enemies during most of its death state, then pauses gameplay in its vortex state; the current C# simulation freezes gameplay entities throughout its 60-tick `PlayerDying` interval. Preserving C#'s enemy freeze is the lower-risk choice; letting the shared oscillator continue during the death timer is closer to its source global-loop behavior. This discrepancy must be made explicit rather than accidentally arising from update order.

### Pulsar and tube geometry

- **[Incitti]** The Pulsar is a six-segment zigzag across its current edge at `height`: project the two endpoints, interpolate the five inner vertices at 20%, 35%, 50%, 65%, and 80% along the edge, and alternate the normal offset by `pulse_zh`. Draw the connected line from one endpoint through the zigzag to the other. The glyph uses `COL_BULLETS` while globally pulsing and `COL_PULSARS` otherwise (`tempest.bmx:2960-3010`).
- **[Incitti]** `Pulsar.Update` sets `OnEdge.haspulser = True`; if `height > 16`, it sets that edge's `pulsing` flag to the global `pulsing` state (`tempest.bmx:3028-3032`). The level update clears edge `pulsing`/`haspulser` flags before the next baddie update (`tempest.bmx:1436-1443`).
- **[Incitti]** A pulsing `Edge.Draw` highlights its two radial boundary lines and alternates `COL_PULSARS`/`COL_BULLETS` according to `pulsecount Mod 8 > 3`; it does not replace the ordinary near/far rim lines (`tempest.bmx:1196-1220`).
- **[C# inference]** The current renderer already projects arbitrary edge endpoints and contains Flipper pivot and Fuseball custom-polyline examples. Pulsar artwork therefore fits the current projection and callback-based line rendering. `DrawTube` currently draws every tube line in blue and has no edge-state model, so add only a narrow extension: draw the normal tube unchanged, then overlay the two projected radial boundaries for each edge that is currently pulsing. Use a small testable renderer seam/callback for the overlay and a Pulsar-specific polyline method. No new generic edge, animation, or rendering framework is justified.
- **[Arcade check]** Compare glyph size/offset, pulse colors, phase period and boundary flashing visually. Incitti's palette varies by board color; do not assume its level-17 color mapping proves the Atari palette or intensity.

## 4. Collision, scoring, death, and accounting

### Player collision

- **[Incitti]** Collision requires `MainPlayer.e_Index == e_index`; there is no sub-lane check.
- When `Depth - height < 8` (strictly less than 8), a non-flipping Pulsar (any `typ` other than 1 or 2) kills immediately and sets `typ=7`. A flipping Pulsar only kills when `Abs(totangle-angle) < 30`. These rim contacts use `KILLED_BY_FLIPPER`.
- At distance 8 or greater from the rim, only state 0 or state 4 is lethal, and only while the global `pulsing` flag is true. This test is lane-based and does not compare the Pulsar's height to the player. It uses `KILLED_BY_PULSAR`. Mid-tube/rim pauses and angular flipping are not lethal through this branch.
- The visual edge flag's `height > 16` cutoff is not the collision cutoff: the source collision branch can kill in a pulsing phase while the Pulsar is below height 16.
- **[C# inference]** Preserve the strict inequalities and branch ordering in unit tests. Use the existing `Player.EdgeIndex`, `Level.Depth`, and central `KillPlayer` lifecycle. The C# player-death lifecycle currently uses one generic death state/visual and a 60-tick freeze; adding separate source death-animation types is unnecessary for this isolated milestone.
- **[Arcade check]** Validate the actual lane-hazard rule and feel against Atari gameplay. The source's lane-wide rule can look surprising if interpreted as a point collision, but its edge rendering indicates the whole lane boundary is electrified.

### Player-shot collision and scoring

- **[Incitti]** A shot hits only when its edge equals `OnEdge` and `sp = height - shotHeight` is strictly greater than 0 and strictly less than 16. The check is not restricted to a movement state. During state 1/2 the code projects the turning edge and checks its midpoint; otherwise it projects the edge midpoint. A hit removes the Pulsar, awards 200 points, and decrements `enemiesleft` (`tempest.bmx:3188-3221`).
- **[C# inference]** Reuse `Shot`, the scenario's existing insertion-ordered `developmentCollisionOrder`, `ScoreSystem`, and `Level.RecordEnemyDestroyed`. Add a Pulsar case to the existing development-target collision path, rather than a second shot loop. Preserve shot-before-enemy-update/player-collision ordering so a shot on the last safe tick can save the player. In a turn, `EdgeIndex` remains the source/current edge until the state-3 lane-change transition; tests should pin that boundary.
- **[Arcade check]** Confirm the hit window, points and visual hit target with gameplay.

### Death, re-entry, completion, and Super Zapper

- **[Incitti]** Pulsar electrical deaths and rim deaths select different death causes, but both enter the player-dying state. Later, `Baddies.ConvertBaddieToEggs` clears `enemiesleft`, removes surviving non-spike baddies, and returns unresolved enemies to the Egg supply. A shot-destroyed Pulsar is already resolved and does not return. The Super Zapper can call Pulsar `ZapIt(t)`; after its delayed death it is removed, awards 200 points, and decrements enemy count (`tempest.bmx:2224-2250`, `3025-3029`, `3214-3219`, `3956-3982`).
- **[C# inference]** Reuse the normal death reconstruction: clear active Pulsars, shots, and development collision entries; preserve destroyed count and score; reset survivors to fresh Eggs using `Level.ResetAfterDeath`. Include Pulsars in the active-enemy readout and active count passed to completion logic for invariant clarity. Shot kills alone increment `EnemiesDestroyed`; player death does not.
- **[C# inference]** Do not add a placeholder `ZapIt` or Super Zapper implementation. The Pulsar can be shot and tested without it. Super Zapper, its charges/display, and staggered delayed destruction are a separate milestone. The absent source audio/sound hooks likewise do not block this work.
- **[Arcade check]** Compare life-loss timing/re-entry. Incitti's vortex delay and C#'s immediate fresh-Egg rebuild after its death ticks are not the same presentation.

## 5. Interactions and scope dependencies

- **Flippers:** Pulsars share tube lanes, the player edge, `Shot`, enemy accounting, and player death. Keep Pulsar's own two collision rules; do not route it through Flipper's rim-arrival event.
- **Fuseballs:** No source coupling. Keep `--pulsar-test` mutually exclusive with `--fuseball-test`; reuse the same development-wave selection pattern, not Fuseball state or rendering.
- **Tankers:** Tankers can carry Pulsars in later source levels. Do not add cargo creation/release, cargo identity, or Tanker ledger rules here.
- **Spikers/spikes:** No special Pulsar interaction appears in the source. Keep scenario selection mutually exclusive with the existing Spiker/Tanker/Fuseball waves; do not alter spike exits.
- **EnemyBullets:** Pulsars do not fire them in the source. `--enemy-bullets-test` should not create Pulsar ammunition or otherwise change the Pulsar scenario.
- **Eggs:** Reuse the current deterministic release/approach/hatch schedule and ordinary ten-unit accounting; only the hatch factory changes in the dedicated scenario.
- **rimit/hurry-up:** `rimit` is set by the source when Eggs/reserve are exhausted but enemies are not all on the rim; it forces the Pulsar's top decision toward a flip (`tempest.bmx:3800-3824`, `3065-3078`). It depends on wave progress/on-rim tracking and is not needed to validate movement/pulsing in isolation. Keep `rimit=false`; document the omitted behavior. Add it only with later wave pacing.
- **Progression:** Pulsar eligibility at displayed level 17 is useful source context, not a reason to implement level 17 composition. The C# project still has a single Level 1 geometry and no normal next-level progression.
- **Bonuses/exit:** The existing C# exit/completion state can remain unchanged. No level bonus, next board, bonus sequence, or production-level unlock is required.

## 6. Recommended C# implementation architecture

### Scenario and wiring

Add a dedicated `--pulsar-test` option following the established Spiker/Tanker/Fuseball scenario pattern:

1. Keep the existing ten-unit Level/Egg supply and hatch cadence.
2. In the scenario hatch factory only, create a height-zero, type-0 Pulsar for each ordinary hatch. Do not create Tankers or use normal random enemy composition.
3. Keep the option mutually exclusive with `--spiker-test`, `--tanker-test`, and `--fuseball-test`; allow `--lives` to compose. The EnemyBullets flag has no Pulsar effect.
4. Add Pulsars to `TempestGame`'s owned list/read-only view, hatch/update/shot collision/player collision/death reset/completion/HUD paths.
5. Add a Pulsar line-glyph renderer and the minimal pulsing-lane overlay described above. Keep simulation independent of `VectorRenderer`.
6. Put comments at the non-obvious source boundaries: `>50` means 51 updates, edge movement occurs after the mid-pause, pulse timing is global, pulse hazard is lane-based, and the source/type-2 uncertainty. Do not comment obvious assignments or loops.

### Likely files

Expected implementation files (not changed in this analysis):

- `src/Tempest.Game/Gameplay/Pulsar.cs` — replace the stub with a small explicit movement state machine and collision predicates.
- `src/Tempest.Game/TempestGame.cs` — scenario ownership/wiring, shared pulse oscillator, target ordering, collision, death reconstruction, accounting.
- `src/Tempest.Game/Debug/DebugController.cs` and `src/Tempest.Game/Program.cs` — flag parsing, exclusivity, and composition-root selection.
- `src/Tempest.Game/Rendering/VectorRenderer.cs` — Pulsar glyph and pulsing lane boundaries.
- `tests/Tempest.Game.Tests/SimulationTests.cs` — deterministic source-rule and regression tests.
- `README.md` — concise scenario command/limitations and manual comparison note.

No new gameplay framework or separate pulse-clock class appears necessary. If the renderer test seam becomes too large, a small dedicated helper/type is only justified after the actual callback/API need is visible. `Level.cs` likely needs no behavior change: its normal ten-enemy ledger already supports a new enemy kind if `TempestGame` supplies the complete active count and calls `RecordEnemyDestroyed` on shot kills.

### Reuse rather than duplicate

Reuse `Enemy`, `Shot`, `Player`, `Level` geometry/depth/accounting, `Level.NextEdge`/`PreviousEdge` where closed wrapping applies, `TempestGame`'s injected `Random`/inclusive delegate, `developmentCollisionOrder`, `ScoreSystem`, `KillPlayer`, the existing 60-tick death/re-entry path, `Projection`, `VectorRenderer` callback patterns, `DebugController` scenario flags, and `MinimumRandom`/related test doubles.

Do not introduce a general enemy hierarchy rewrite, component model, generic animation framework, global level configuration service, open-tube renderer, audio layer, or Zapper abstraction for this feature.

## 7. Deterministic automated test proposal

Use direct `Pulsar.Update` tests with scripted inclusive-random delegates for state rules; use `TempestGame` tests for integration. Assert the exact boundary tick/value, not merely that a transition eventually occurs.

1. **Scenario and hatch:** flag recognized, conflicts rejected, default game unaffected; a ready Egg becomes exactly one height-0/state-0 Pulsar on its lane; no Flippers/Tankers/Spikers hatch; `remaining = reserve + Eggs + active Pulsars + destroyed`.
2. **Up/down movement:** height changes by exactly +1/-1; clamps at `Level.Depth` and zero; test top and bottom decisions with both random outcomes.
3. **Decision cadence:** no random transition through tick 50, exactly one on tick 51; verify the mid-pause value and selected `oldtyp`. Also verify the source first-eligible configuration (12-degree flip and the pause expressions after confirming BlitzMax integer conversion) versus any documented development override.
4. **Pause and edge transition:** state 3 holds edge/height until its exact expiry tick, then changes one edge and resumes; state 5 holds the same edge and resumes without moving lanes. Test closed wrap both directions and open endpoint reflection. Test same-edge direction tie with each random result.
5. **Flip geometry/state:** angle advances by the configured step, state 1's height rises, turn completes at `totangle`, the Pulsar remains associated with the source edge until the delayed edge transfer, and a type-2 path is not accidentally introduced.
6. **Pulse boundaries:** assert phases 70/71, 109/110, 180/0; 181 calls per default period; check exact `pulse_zh` formula, `<2` dead zone, and color alternation at `pulsecount % 8` values 3/4. Verify one oscillator is shared by multiple Pulsars and decide/test whether it advances during the C# death freeze.
7. **Lane rendering:** callback tests assert a six-segment Pulsar line at projected `EdgeIndex`/height, finite coordinates, correct zigzag offsets at pulse heights 0 and nonzero, normal/pulsing glyph colors, and that only the active edge's two radial boundaries receive the alternating overlay. Verify `height == 16` versus `height > 16`.
8. **Player collision:** exact same-edge requirement; far-from-rim state 0/4 lethal only during the global pulsing window; state 3/5 and angular flip nonlethal in that branch; no lane mismatch collision; `Depth-height == 8` follows the non-near-rim branch and `<8` the near-rim branch. Test non-flip near-rim collision, flip angle just below/equal/above 30 degrees, and the type-7 terminal behavior.
9. **Shot collision and score:** same edge and strict `0 < Pulsar.Height - Shot.Height < 16`; exact 0/16 boundaries miss; different edge misses; flipping/ordinary states both hit. A hit consumes the shot, removes only that Pulsar, awards 200 once, and increments destroyed accounting once.
10. **Death/reconstruction:** trigger both electrical and near-rim death; verify one life decrement after the established death duration, surviving Pulsars become fresh Eggs, destroyed Pulsars stay destroyed, score persists, bullets/shots/collision-order entries are cleared, and player position resets. Re-kill the player and assert no duplicate enemy accounting.
11. **Completion/accounting:** active Pulsars keep the ten-unit wave incomplete; reserve/Egg/active/destroyed totals conserve ten; after all ten are shot, existing completion/exit behavior occurs exactly once. Confirm no next-level progression is introduced.
12. **Regression:** run all existing simulation tests and build. Smoke-test normal Level 1 plus Spiker/Tanker/Fuseball scenarios to confirm the new flag/path does not change established behavior.

## 8. Manual validation checklist for Neal

Run the scenario with extra lives:

```powershell
dotnet run --project src\Tempest.Game -- --pulsar-test --lives 10
```

Compare against an Atari Tempest reference/play session (and separately use Incitti as an implementation reference):

- Does the Pulsar glyph read as a zigzag across one lane, with the expected change in amplitude/color during pulse phases?
- Do only the two radial boundaries of the Pulsar's lane flash, and does the lane flash begin only after it is above the source's height-16 visual threshold?
- Compare climb/descent speed, ~51-update mid-tube decision cadence, first-eligible pause durations after resolving source integer conversion, turn direction/arc, and lane association while turning.
- Observe both rim contact and pulsing-lane collision; verify off-lane movement avoids the lane hazard and that the pulse can be shot.
- Verify a hit awards 200, death costs one life, unresolved enemies re-enter as Eggs, and destroyed Pulsars do not return.
- Confirm scenario stays on the current tube, does not create Tankers or other enemy kinds, and does not advance to another level. Replay/compare normal Level 1 and existing development modes afterward.

Automated tests establish the chosen Incitti-derived rules and C# invariants; this checklist is necessary to assess arcade feel and visual fidelity.

## 9. Risks, uncertainties, and explicit out-of-scope

### Risks/uncertainties

- The reference's numeric state 2 has flip/render/collision branches but no assignment in the inspected direct-hatch state path. Treating it as a reachable state would be speculative.
- The source timer is 181 ticks per wrap, the pulse window is 39 ticks, and pulse geometry amplitude has a dead zone. Easy-to-"clean up" 180/40/sine approximations would change observed source behavior.
- `pulsesalive` is tied to update order; audio is absent, so do not emulate sound via hidden timing side effects.
- Incitti updates Pulsars/global pulse during much of player death, while current C# freezes gameplay state for its death timer. Choose and test the shared-clock behavior explicitly; do not silently claim equivalence.
- Source `typ=7` has no normal draw geometry case. Its near-rim behavior is a terminal collision handoff, not a regular enemy animation to reproduce blindly.
- The project has only a closed Level 1 renderable tube. Open-edge state logic can be tested, but open-board presentation cannot yet be validated.
- All-ten-Pulsar direct hatching, first-eligible parameters on the existing tube, simplified death presentation, and no rimit behavior are development choices, not source-normal composition or arcade-verified facts.
- Atari pulse cadence, glyph geometry, collision feel, lane electrification, and scoring still need manual comparison.

### Explicitly OUT OF SCOPE

- Normal level progression, Level 17 composition, unlocks, board sets, level bonuses, and production hatch probabilities.
- Tanker-carried/released Pulsars and Tanker accounting changes.
- `rimit`/hurry-up/on-rim pacing.
- Super Zapper, Pulsar `ZapIt`, charge count, delayed Zapper kills, effects, and audio.
- New audio/sound support or distinct death animations.
- Open-tube board data/rendering.
- New enemy abstractions, ECS, general animation/render pipelines, broad renderer or `Level` redesign.
- Changes to Flippers, Fuseballs, Tankers, Spikers, EnemyBullets, Eggs, spikes, or existing death/exit behavior beyond wiring Pulsars into shared extension points.
- Any assertion that Incitti's behavior proves Atari arcade accuracy.
