# Stage B Analysis: Level 2 Square Geometry and Canflip Flippers

Analysis date: 2026-10-31
Authority checkpoint: `f41a53c Add Level 1 configuration and initialization groundwork`
Analysis only. No gameplay, tests, authority documents, project files, or Git state were changed. The exact supplied prompt is saved beside this report.

Evidence labels follow [TEMPEST-BEHAVIOR-SPEC.md](../../TEMPEST-BEHAVIOR-SPEC.md): **ARCADE VERIFIED**, **REFERENCE VERIFIED**, **PROJECT DECISION**, and **DEVELOPMENT ONLY**. Incitti source is not proof of Atari behavior.

## 1. Executive conclusion

**Recommendation: split Stage B into B1 square geometry/configuration and B2 canflip behavior.** The current square board data can be loaded by the Stage A definition/runtime boundary, and most rendering/accounting is already definition/configuration driven. The blocker is pivot geometry: `Level.CalculateEdgeTurnAngle` measures the change in oriented lane-edge directions, but a Flipper endpoint hinged at a shared vertex needs the angle between the two rays from that vertex. On this square, the former returns zero on twelve lane pairs even though the source-derived hinge sweep is 180 degrees there.

B1 should isolate the square and validate its geometry and rim transitions before adding a new Flipper state path. B2 can then add the ordinary Level 2 canflip policy, pause/direction state, and mid-tube tests. Preserve the existing Level 1 pivot output using a topology-specific Flipper sweep; do not globally replace `CalculateEdgeTurnAngle`, which is also used by other mechanics.

Do not implement automatic progression, production mixed waves, other enemy families, open tubes, or bonus/audio work. The proposed development scenario remains terminal and replays its own configuration.

## 2. Exact Level 2 reference configuration

Source: [Incitti `levelset1.txt`](../../Reference/incitti/levelsets/levelset1.txt), row `Level 2 - square`; progression and tuning are set in `SetUpEnemies` in [Incitti `tempest.bmx`](../../Reference/incitti/tempest.bmx).

| Property | Level 2 reference value | Evidence / qualification |
|---|---:|---|
| Zero-based progression identity (`current_level` / `lv`) | 1 | **REFERENCE VERIFIED** |
| Displayed identity | 2 | Source draws `current_level + 1`; **REFERENCE VERIFIED** |
| Board index | 1 | `current_level Mod 16`; **REFERENCE VERIFIED** |
| Palette index | 0 | `current_level / 16`; **REFERENCE VERIFIED** |
| Continuous | `true` | First value in the data row; closed tube; **REFERENCE VERIFIED** |
| Vertex count / playable lanes | 16 / 16 | Closed 16-vertex tube; **REFERENCE VERIFIED** |
| Ordered points | `(161,161), (80,161), (0,161), (-80,161), (-161,161), (-161,80), (-161,0), (-161,-80), (-161,-161), (-80,-161), (0,-161), (80,-161), (161,-161), (161,-80), (161,0), (161,80)` | Exact source row |
| Center Y / Y offset | 400 / -80 | Data row; **REFERENCE VERIFIED** |
| Depth / normal position | 400 / 50 | Depth is the source Level depth; the C# configuration uses position 50. Position is a project/runtime value, not an extra board-row value. |
| Enemy supply | 12 | `10 + lv/2 + board*2`; **REFERENCE VERIFIED** |
| Initial Eggs / reserve | 4 / 8 | Source creates four Eggs, then has eight reserve units; **REFERENCE VERIFIED** |
| Maximum Eggs | 12 | Source assigns `maxeggs=eggsleft`; **REFERENCE VERIFIED** |
| Reserve release cadence | 50 global updates | Source `enemyreleaserate=50`; up to six reserve Eggs are released at a release event, subject to remaining supply. **REFERENCE VERIFIED** |
| Hatch rate | 0 | Integer `lv/8`; the source's hatch test still applies. Do not equate this with the C# Stage A hatch-selection interval. |
| Eligible normal enemies | Flippers only | At `lv=1`, all Tanker, Spiker, Fuseball, Pulsar, and Tanker-variant gates remain false; **REFERENCE VERIFIED** |
| Canflip | enabled | Source sets `canflip = false` only at `lv=0`; **REFERENCE VERIFIED** |
| Flipper climb increment | 2 per update | **REFERENCE VERIFIED**, not arcade timing |
| Flipper turn increment | 8 degrees per update | `8 + lv/5` with integer division; **REFERENCE VERIFIED**, not arcade timing |
| Below-rim pause / rim pause | 21 / 24 updates | `21 - lv/5` / `24 - lv/5`; **REFERENCE VERIFIED**, not arcade timing |
| Flipper ammunition / timer | `Rand(1,3)` / 16 | **REFERENCE VERIFIED**. Source ammunition and firing values are not measured arcade values. |
| Firing attempts | Each climbing update and when a type-3 pause expires; not each turning update | The source `Shoot()` calls and `Rand(0,100)>75` gate are reference behavior only. |
| Initial player lane | 7 | Source sets lane 7 on new-level setup; a C# sub-lane offset of 0.5 is a project choice, not established by this source statement. |
| Projection K / Z offset | 70 / 5 | Source/runtime baseline; C# Program also sets these values. |

For comparison, Level 1 is identity 0, board 0, palette 0, circle geometry, supply 10, four initial Eggs, and canflip disabled. Its remaining Level 2 differences are the square points and the source's enabled canflip. The source-derived integer formulas happen to leave Level 2 climb speed, turn increment, lane/rim pause values, release interval, initial Egg count, and palette unchanged from Level 1.

The Stage A [Level 1 configuration](../../src/Tempest.Game/Levels/LevelConfiguration.cs) contains additional C# scheduling/cap values (including maximum active Eggs, approaching Eggs, and hatch-selection cadence) that are project policy, not source constants. A Level 2 test should reuse the established Egg runtime with valid 0–15 spawn lanes and explicit 12-unit supply; it must not present the Stage A deterministic Egg schedule as Incitti or arcade evidence. A narrow `FlipperConfiguration` for canflip and its Level 2 values is preferable to a general-purpose tuning framework.

Incitti creates ammunition for normal Flippers, including Level 2. Current C# policy keeps enemy bullets opt-in, however. Preserve that policy in the isolated scenario: `--enemy-bullets-test` remains a separate optional flag, not an automatic Level 2 behavior or a Tanker-specific change. Manual canflip validation should first run without it; firing can be exercised separately if desired.

## 3. Square geometry analysis

Let `P[i]` be the ordered source vertex and lane `i` be the closed segment `P[i] -> P[(i+1) mod 16]`. The square has four straight sides, each represented by four consecutive lanes. Its four corners are between lanes 3/4, 7/8, 11/12, and 15/0.

The current C# helper compares the forward direction vector of the source lane with that of the destination lane, folds the unsigned angular difference into `[0,180]`, and uses this for adjacent Flipper sweeps. The following values apply equally to either direction of each pair:

| Adjacent lane pair | Current C# turn | Geometry |
|---|---:|---|
| 0 ↔ 1 | 0° | Straight side |
| 1 ↔ 2 | 0° | Straight side |
| 2 ↔ 3 | 0° | Straight side |
| 3 ↔ 4 | 90° | Corner |
| 4 ↔ 5 | 0° | Straight side |
| 5 ↔ 6 | 0° | Straight side |
| 6 ↔ 7 | 0° | Straight side |
| 7 ↔ 8 | 90° | Corner |
| 8 ↔ 9 | 0° | Straight side |
| 9 ↔ 10 | 0° | Straight side |
| 10 ↔ 11 | 0° | Straight side |
| 11 ↔ 12 | 90° | Corner |
| 12 ↔ 13 | 0° | Straight side |
| 13 ↔ 14 | 0° | Straight side |
| 14 ↔ 15 | 0° | Straight side |
| 15 ↔ 0 | 90° | Closing corner |

Thus 12 pairs return zero and four return a nonzero 90-degree turn. There is no undefined tangent in this data: every lane has nonzero length and every transition is adjacent. The unsigned direction-change helper does not encode a signed sweep; direction must come from the selected previous/next lane.

## 4. Current zero-angle failure

`Flipper.Update` increments `TurnAngle` by 8 before comparing it with `TotalTurnAngle`. With a square straight-side total of zero, the first update satisfies the completion check. The renderer clamps the displayed angle to the zero total, so it displays no pivot arc and then commits the destination lane: the change effectively snaps. This is not a value to repair with an epsilon or arbitrary fixed angle; it is a mismatch between the measured angle and the hinge being drawn.

The four corners do receive a 90-degree current sweep and therefore animate for 12 updates at 8 degrees per update. A nonzero value alone does not establish that the current formula is the correct geometric construction for all layouts.

## 5. Correct pivot/hinge model

Incitti builds `Edge.angle` from three consecutive vertices (`p2`, `p1`, and the next edge's `p1`) in `UpdateAngles`, then uses the angle while rotating one projected endpoint around the other in the Flipper rendering/collision transform. This is a vertex/hinge construction, not merely the difference between the directions of two oriented lane segments.

For adjacent lanes, the hinge is their shared vertex at that Flipper's projected depth. Measure the angle between the ray from the shared vertex to the old lane's unshared endpoint and the ray from it to the destination lane's unshared endpoint. For this convex square, that hinge angle is the supplement of the current oriented-edge turn:

- Straight-side neighbors: 180-degree hinge sweep. The lanes are collinear but lie on opposite sides of their shared endpoint; the Flipper's free endpoint must turn over to the neighboring segment.
- Corner neighbors: 90-degree hinge sweep.
- Wrap pair 15/0: 90-degree hinge sweep at the closing corner.

Use the signed lane direction to select which endpoint is fixed and which sign to rotate. At exactly 180 degrees both signs have the same final endpoint, but trace opposite arcs; resolve that sign deterministically from the source/destination direction, not by inspecting a zero cross product or adding an epsilon. This is an endpoint hinge in the projected cross-section, not a lane-midpoint interpolation.

On the current Level 1 circle, the existing C# oriented-edge measure is not exactly constant because its points are integer-rounded: it ranges from about 21.60° to 23.78° per adjacent pair. The corresponding vertex hinge is about 156.22° to 158.40°. A generalized hinge therefore has very different numeric and temporal behavior from the current Level 1 C# pivot, even though its endpoints are geometrically meaningful.

## 6. Level 1 compatibility

Use a topology/geometry-specific Flipper sweep for the isolated square. Keep the existing Level 1 `CalculateEdgeTurnAngle` path and visible pivot untouched until the user separately approves an arcade revalidation. The existing helper is also used by Tanker cargo and other pivoting mechanics; changing it globally would broaden risk and violate the Level 1 regression requirement.

The renderer already projects the two lane endpoints, rotates one about an endpoint, and accepts line callbacks for headless captures. That is sufficient as a rendering mechanism if supplied a correct sweep, signed direction, and consistent source/destination phase. B1 should verify the actual projected square arcs before B2 adds the new ordinary Flipper transitions.

## 7. Incitti canflip state machine

The following is **REFERENCE VERIFIED**, not proof of Atari behavior. Source Flipper types are: 0 climbing, 1 below-rim flipping while climbing, 2 rim flipping, 3 delaying, and 7 killed.

1. A Level 2 Flipper hatches at height 0 on its Egg lane. Source creation draws ammunition with `Rand(1,3)` and sets a 16-update shot timer.
2. On the first type-0 update below the rim and without a blocking spike, it adds 2 height. Because `canflip` is true, it sets `oldtyp=1` and changes to type 3. It calls `Shoot()` and chooses `dir=FindShortDir(currentLane, playerLane)`. There is no periodic random canflip roll or counter.
3. Type 3 decrements its pause and adds 1 height, clamped to depth 400. The initial pause is zero, so the next update reaches the move branch. It changes one lane in the retained direction, assigns the new `OnEdge`, computes a geometry angle, attempts a shot, then enters type 1 when no spike interrupts it.
4. Type 1 advances its angle by 8 and climbs by 2, clamped at 400. On reaching the angular total it returns to type 3 with a 21-update pause below the rim or a 24-update pause at the rim.
5. Type 2 is the rim-flipping form: it does not climb. The same 8-degree step ends in a type-3 pause of 24 updates.
6. On each type-3 expiry at or above the rim, it samples the current player lane again before moving one adjacent lane. Below the rim it retains the direction established by the initial type-0 transition.
7. Type 0 calls `Shoot()` on each climbing update; type 3 calls it when its pause expires and the lane changes. It does not fire during type-1/type-2 turning updates. The source decrements the 16 timer on attempts and rolls `Rand(0,100)>75` only when it expires. Exact source mechanics are not validated Atari cadence.

With the proposed hinge geometry, a square straight-side transition takes `ceil(180/8)=23` turning updates and a corner transition `ceil(90/8)=12`; these are source-derived update counts, not a requirement for Neal to measure by eye.

### Collision ownership

- Source `CheckColl` requires the checked edge to equal `OnEdge` and uses the strict height separation `0 < FlipperHeight - ShotHeight < 16`. Source changes `OnEdge` as the pause expires and uses the transformed Flipper for the turn; source-lane ownership therefore needs explicit treatment when representing an in-progress pivot.
- The current C# model accepts shots in either participating lane while flipping. The current behavior specification is the authority for established C# / arcade-verified Level 1 behavior; do not silently narrow that policy in B1. Record this source/current-policy difference and resolve the Level 2 mid-tube rule by manual comparison before changing the specification.
- Source player collision is rim-only: a climbing direct arrival is lethal in the occupied lane; a type-1/type-2 Flipper can be lethal near the end of its rim sweep under the source angle rule. Below-rim transitions cannot kill the player because the Blaster remains at the rim. The project’s **ARCADE VERIFIED** Level 1 completed-pivot safety rule takes precedence over importing the source’s angle threshold.

## 8. Player-pursuit behavior

Incitti Level 2 canflip does cause a below-rim lane change whose **initial direction** is chosen in relation to the player lane. It does **not** retarget on every update or during each below-rim transition:

- The player lane is sampled on the initial type-0 update when canflip starts.
- That direction remains fixed for the successive below-rim transitions.
- Moving the Blaster during an already-started turn cannot change its selected destination.
- The player lane is sampled again when a type-3 pause expires at the rim; later rim transitions then pursue the current lane.
- `FindShortDir` chooses the shorter direction on the closed ring; a same-lane tie chooses randomly. Exact antipodal direction follows the sign of the source index difference.

Neal's **ARCADE VERIFIED** Level 1 observation was only that Level 1 Flippers climb without dynamic pursuit below the rim; it does not establish Level 2 arcade behavior. The Tanker-cargo route is separate: cargo has a prescribed release direction below the rim and resumes player pursuit at the rim. Do not encode ordinary canflip capability by reusing `IsTankerCargo`.

The C# `FindShortestDirection` currently resolves all equal-distance ties as `+1`, unlike Incitti's same-lane random tie and signed antipode. Preserve Level 1’s tested helper; use a Level 2-specific direction policy with injected randomness if source behavior is the chosen reference target.

## 9. Isolated Stage B access design

Add one explicit `--level2-test` **DEVELOPMENT ONLY** launch scenario. It selects a fixed Level 2 configuration at construction and remains terminal; it does not change level identity or create a Level 1-to-2 path. In B1, keep canflip disabled in the geometry-only fixture; in B2, turn on the Level 2 reference setting. Do not present the B1 fixture as a complete arcade Level 2.

Make it mutually exclusive with `--spiker-test`, `--tanker-test`, `--fuseball-test`, and `--pulsar-test`. `--lives` and the existing separately opt-in `--enemy-bullets-test` may compose with it. Mark the game-window title/HUD as development Level 2 while this flag is active. No flags must continue to launch normal Level 1. On Level Complete, `R` reinstalls the same selected Level 2 configuration; Game Over restart does likewise. Do not add a flag that merely turns on Level 2 in a production progression path.

For example, the baseline B2 manual run would be `dotnet run --project src/Tempest.Game -- --level2-test --lives 10`; adding `--enemy-bullets-test` is an explicit separate variant.

This materially improves manual isolation: Neal can see and play the square without first clearing Level 1, while testing lifecycle behavior without making Level 2 naturally reachable.

## 10. Accounting/reset behavior

Stage A's configuration-backed `Level` supports supply 12 without a second runtime type:

- `EnemySupply=12`, four initial Eggs, and eight reserve units feed existing spawn/hatch runtime; kills, rim clears, and active/reserve obligations use the configured total.
- Ordinary death reconstruction retains the same `Level` and configuration, preserves resolved kill/rim-clear counts, and re-enters only unresolved units as Eggs. It should not restore four initial Eggs unless that is the configured maximum reconstruction batch; the existing reset policy uses `MaximumActiveEggs`.
- All-rim completion checks exhausted reserve, no Eggs, at least one active enemy, every remaining enemy at/above depth, and remaining-supply accounting. It resolves active Flippers as rim-cleared (not kills), awards no false kill score, then starts the normal exit.
- Level Complete replay creates a fresh Level from the launch configuration; Level 2 remains Level 2 and no advancement occurs. It preserves current C# score/lives policy and resets fresh-wave state.
- Game Over restart also uses the launch configuration, while resetting score and restoring starting lives.
- Super Zapper charges persist through life reconstruction and reset on fresh replay/restart; score and resolved outcomes remain under existing policies.
- Hurry-Up is not a Level 2 Flipper feature in current C#: it is latched only in Fuseball/Pulsar test modes. Incitti's `rimit` flag also affects other source enemy behavior, not the ordinary Flipper canflip path. Do not add hurry-up behavior to this milestone.

## 11. Super Zapper/all-rim interaction

Super Zapper targeting already receives the definition point count and `Continuous` flag. The square is closed with 16 lanes, so the current shortest closed-ring distance and wrap behavior remain valid. All-target selection still visits the configured Flippers; single-target selection uses their lane and height. No square-specific targeting change is indicated.

The flash recolors `DrawTube` output, whose near/far rims and rails are generated from the current definition. Palette 0 matches Level 1; later palette bands and rotating boards are not needed. Fresh isolated replay resets the two charges; life loss leaves them persistent.

All-rim semantics are geometry-independent for this 16-lane closed Level 2 and use configured supply/height. Preserve non-kill resolution and the existing shot-before-player-collision ordering. No Level 1 geometry constant is required beyond the current shared depth 400 and position 50, which Level 2 also uses.

## 12. Rendering requirements

| Surface | Square impact / required work |
|---|---|
| Board lines and near/far rims | Existing loops consume current points and close the ring. Square needs its 16-point definition; no special square renderer is needed. |
| Projection center/offset | Existing definition-driven `CenterY=400`, `YOffset=-80` are suitable. Depth 400, position 50, K 70, and Z offset 5 match the current baseline. |
| Flipper climb glyph | Uses the two endpoints of its lane and current definition points; geometry loads dynamically. |
| Mid-tube and rim pivot glyph | Existing endpoint-rotation callback can show a hinge. Supply the correct geometry-specific sweep and signed direction; verify the rotated endpoint reaches the intended adjacent lane. |
| Player path | Closed 16-lane movement wraps correctly. Its current edge-count input is point count; that is correct for this closed square. No input/mouse change is justified. |
| Shots and Eggs | Lane-midpoint projection uses adjacent points and works for a closed square. |
| Enemy bullets | Renderer already uses the lane midpoint and is hidden unless the existing bullet test flag is enabled. |
| Super Zapper flash | Existing tube-line recolor works on current points; palette 0 introduces no new palette requirement. |
| HUD/window | The Level Complete text is hard-coded to “LEVEL 1 COMPLETE”, and the window title is Level 1-specific. Make the isolated development identity explicit; no normal-launch claims need to change. |

The first open tube remains a separate problem: current drawing and lane helpers close with modulo indexing, and open topology needs its own rendering/player/Egg/edge policies. Later palette and spin changes are also deferred.

## 13. Manual arcade-validation plan

Compare the B2 isolated scenario to an Atari Level 2, without asking Neal to estimate simulation ticks:

1. Square proportions, lane count, near/far rims, and Blaster placement at the four sides/corners.
2. Blaster movement around the square, paying particular attention to perceived smoothness at side-to-corner and corner-to-side changes.
3. Flipper ascent from an Egg lane and whether mid-tube lane changes occur.
4. Direction relative to the Blaster, including moving the Blaster during an active transition and before the next transition.
5. Visual transition on straight sides and around corners; whether the square hinge path looks plausible and lands cleanly on the destination lane.
6. Shots during the transition from each participating lane; record which actually destroy the Flipper.
7. Confirm no below-rim player death, then check direct rim arrival and subsequent rim pursuit.
8. Complete a wave by kills and by all-rim arrival; verify the visible exit.
9. Lose a life during the wave; confirm unresolved units return and resolved outcomes stay resolved.
10. Use Super Zapper as an all-target and single-target charge.

Only observed Atari differences should become **ARCADE VERIFIED**. Do not use manual impressions to claim exact ammo, interval, roll probability, first-shot delay, or firing-height cutoff.

## 14. Deferred Tanker/input observations

- Tanker firing activation/cadence is a separate deferred arcade-fidelity issue from Stage A. Stage B contains no Tankers and must not alter their code or enable them in the Level 2 composition.
- Neal's slight perceived Blaster rim-movement delay is unresolved and not a Stage A blocker. The square provides a useful second geometry for reassessing movement near its corners and lane transitions, especially before higher-difficulty progression. This is an observation opportunity, not evidence for a cause or permission to tune sensitivity, interpolation, input timing, mouse handling, or Player movement.

## 15. Deterministic test plan

### Configuration

- Level 2 identity 1/display 2, board 1, palette 0, closed 16-vertex/16-lane square; assert all 16 ordered points.
- Depth/position/center/offset, supply 12, four initial Eggs, eight reserve, Flipper-only composition, and Level 2 canflip setting.
- All spawn lanes remain in 0–15. Explicitly label the C# Egg release/selection schedule as project policy.

### Geometry

- Assert the current legacy helper's 16 adjacent square outputs are twelve zeroes and four 90s; do not accept epsilon patches.
- Assert the new square hinge outputs are 180 on the twelve straight-side lane pairs and 90 on the four corners, including 15↔0.
- Test both signed directions and that each rotated endpoint terminates on the intended adjacent lane; check forward/reverse symmetry of magnitude and deterministic 180-degree sign handling.
- Pin the Level 1 definition and legacy Flipper turn-angle outputs so B1 cannot change current circular pivots. Keep Tanker cargo and Pulsar helper coverage.

### Canflip

- Level 1 cannotflip path remains unchanged; Level 2 enters its first below-rim turn at the source boundary.
- Verify no per-tick random canflip test, initial player-lane direction, same-lane tie injection, antipodal direction, direction retained below rim, and stable destination during active turning.
- Verify height +2 while type-1 turning, +1 during type-3 pause, lane/rim pause durations, clamp at depth, and rim direction resampling only after pause completion.
- Check the source-reference shot lane/strict height contract separately from the established C# two-lane Level 1 policy; do not silently change the latter.
- Assert no below-rim player death; retain direct rim-arrival and completed-pivot safety regression tests.
- Pin `Shoot` attempt cadence and RNG call ordering at climb, pause expiry, and turns (no turn-update fire).

### Isolated lifecycle / Super Zapper / all-rim

- Normal launch is Level 1; `--level2-test` launches square Level 2; incompatible scenario flags reject deterministically.
- Level 2 Complete replay and Game Over restart stay Level 2; death reconstruction retains the same configuration/runtime and resolved accounting.
- Assert score/lives, charges, and Egg/reserve behavior on each path. Verify all-target/single-target Zapper on wrap lanes, flash rendering, charge persistence on death, and recharge on fresh replay.
- Verify all-rim needs exhausted supply/no Eggs and all active Flippers at depth; clear awards no kill score and begins exit.

### Rendering/regression

- Capture exactly 16 rails, 16 near-rim segments, and 16 far-rim segments for the non-pulsing closed square; all projected coordinates must be finite.
- Capture square player, shot, Egg, Flipper climb, straight/corner pivot, and flash submissions.
- Retain the full 339-test Stage A regression suite and Level 1 manual assumptions; keep Tanker cargo direction, Spiker, Fuseball, and Pulsar development behavior unchanged.

## 16. Architecture conclusions

- **Flipper state representation:** the current three states can host the movement, but they do not encode the distinction between the 21-update below-rim pause and the 24-update rim pause or the ordinary Flipper's latched below-rim direction. Add explicit canflip/pause-phase policy and a direction-initialized/latching rule; do not create a broad enemy hierarchy. `IsTankerCargo` is not the ordinary canflip capability.
- **Pivot geometry:** yes, separate the Level 2 Flipper hinge sweep from current Level 1 `CalculateEdgeTurnAngle`. Do not change the shared helper used by current Level 1, Tanker cargo, or Pulsar paths.
- **Renderer:** existing callbacks and endpoint rotation are sufficient if the transform uses the correct hinge endpoint/sign and the state/collision lane model agrees. Do not rewrite `VectorRenderer` broadly.
- **Configuration:** a small Flipper-specific configuration is warranted for isolated Level 2 (canflip, lane/rim pause, climb and turn increments). Do not build generic tuning for every enemy or future level yet.
- **Stage A:** no architectural rollback is indicated. The Stage A configuration/runtime boundary is sufficient; Level 2 simply needs its own definition/config and launch selection. The new square geometry helper is a Stage B prerequisite, not a defect in Stage A's Level 1 boundary.
- **Legacy total:** `Level.EnemyTotal` still returns the Level 1 total, but it has no current C# call sites. New Level 2 logic must use the installed runtime's configured `EnemySupply`; no Stage A change is needed solely for the unused compatibility property.

## 17. Exactly one recommended implementation scope

**Choose C: split into B1 geometry and B2 canflip.**

- **B1 — square/configuration isolation:** add the closed square definition and isolated development launch/replay; use a geometry-only test setting with canflip off; add the square hinge sweep and renderer geometry while preserving all Level 1 behavior; verify square movement, endpoint pivots, projection, accounting, Zapper, and replay manually.
- **B2 — canflip:** enable the Level 2 setting; implement the type-0-to-below-rim transition, lane pause, latched direction, height/timing, mid-tube shots, and rim re-target behavior. Run deterministic tests and compare the behavior with Atari before considering any production progression.

The geometry checkpoint is independently inspectable and correctable before canflip creates new Flipper states. Do not advance to natural Level 2 progression as part of either checkpoint.

## 18. Expected files

Likely B1 files:

- `src/Tempest.Game/Levels/LevelLoader.cs` or a small square-definition data file; `LevelConfiguration.cs` for explicit Level 2 data.
- `src/Tempest.Game/TempestGame.cs`, `src/Tempest.Game/Program.cs`, and `src/Tempest.Game/Debug/DebugController.cs` for the isolated launch selection only.
- `src/Tempest.Game/Levels/Level.cs` for the dedicated square Flipper hinge geometry if that is the smallest suitable owner; avoid changing the existing shared turn helper.
- `src/Tempest.Game/Gameplay/Flipper.cs` and `src/Tempest.Game/Rendering/VectorRenderer.cs` only for isolated square pivot sweep plumbing.
- `tests/Tempest.Game.Tests/LevelConfigurationTests.cs`, `SimulationTests.cs`, and focused renderer tests.

Likely B2 adds to `Gameplay/Flipper.cs`, the Level 2 Flipper settings, and targeted simulation/render tests. `README.md`, `TEMPEST-DEVELOPMENT-NOTES.md`, and `TEMPEST-BEHAVIOR-SPEC.md` should change only if needed after the implementation/manual-validation checkpoint. No Tanker, input, audio, bonus, open-topology, or unrelated production files are in scope.

## 19. Behavior Spec impact

Do not edit the spec during this analysis. After B2 Atari comparison, record only the observed Level 2 behavior and keep it scoped to that level: square lane appearance, whether/when Flippers change lanes below the rim, observed player tracking, visual transition, shot/player collision, and rim pursuit. Mark observations **ARCADE VERIFIED** only when Neal has actually compared them to Atari. Keep source formulas and exact timings **REFERENCE VERIFIED** unless independently measured; preserve the distinction between B1 development fixtures and normal gameplay.

## 20. Remaining arcade uncertainties

The source does not establish Atari canflip behavior, exact lane-change frequency or pauses, turn speed, climb increments, bullet count/cadence/randomness/initial delay/firing-height cutoff, shot-lane ownership during a transition, or how the square hinge appears on hardware. The current Level 1 completed-pivot rule remains the project’s arcade-verified safety constraint. The observed Level 1 lack of below-rim pursuit cannot settle Level 2 behavior. Neal need not measure update ticks; observations should remain qualitative unless a separate measurement is performed.

## 21. GO / NO-GO implementation recommendation

**GO for a separately approved B1 implementation only.** Its acceptance must include square geometry and hinge validation without canflip and with existing Level 1 pivots unchanged. Then proceed to B2 only after B1 is visually acceptable. **NO-GO** for one combined geometry/state-machine implementation, automatic Level 1-to-2 advancement, or any production composition/progression work in this milestone. This report is analysis only and does not authorize implementation now.
