# Blaster Fire, Rim Pursuit, Collision, and Difficulty ROM Investigation

## 1. Executive conclusion

**Most likely classification: G — a demonstrated combination of factors, with a remaining perceptual-validation limit.**

The strongest source-level explanation is not that C# fires more frequently. In normal Level 1, the ROM attempts one shot per gameplay pass when a slot is free and has eight player-shot slots; C# accepts a shot at most once every three 60 Hz simulation ticks and has ten slots. The C# shots do, however, persist much longer in their own coordinate system, and C# tests their collision before Flipper movement/contact. C# also treats a Flipper as shootable from both pivot lanes throughout the pivot, while the ROM uses an in-progress-state branch and shot-time position data rather than a blanket two-lane rule. Together these choices let held fire preempt a C# pivot/contact that can already be lethal in the ROM's enemy-movement phase.

The ROM's Level 1 Flipper does pursue a stationary Blaster: its level-selected movement starts with straight climbing, then hands off to a top-of-tube pursuit program. It chooses a direction toward the Blaster, with a special direction choice when another rim enemy is present, and can collide on arrival/completed movement. Its rim program uses a four-count check/pause and a level parameter of two for top-turn processing, unlike C#'s 24-tick rim pause and 8-degree turn step. The master file does not expose enough decoded Level 1 angle data to calculate a definitive full-lane cadence in gameplay passes, so the exact cadence ratio remains **UNKNOWN**.

The ROM supports these concrete conclusions:

- The Level 1 Easy/Medium/Hard options do not change player firing or player-shot speed/capacity/lifetime.
- Hard increases Level 1 enemy supply from 10 to 11, raises enemy-shot capacity from two to three, makes Flippers move faster, and enables Pulsar firing where Pulsars exist. Easy lowers enemy-shot capacity to one, reduces Flipper speed and (for early levels) the top-turn acceleration.
- `00` decodes/displays as Medium; the ROM does **not** establish that the cabinet's factory/default/normal switch setting is Medium. Neal's belief that the cabinet is probably Medium remains an unverified owner belief.
- Static source explains why a rim-arrival race can favor the C# game, but does not calculate the survival probability of Neal's full stationary strategy. A deterministic Level 1 comparison and Neal's arcade observation are still needed before tuning production behavior.

## 2. Starting repository state and baseline

At investigation start:

- Branch: `master`.
- `HEAD`: `3bebf42cbe9514846ab32bf4c8a12bcfdb048da7` (`Implement accepted production Level 7`).
- `git diff --stat` and `git diff --cached --stat`: empty.
- Tracked worktree and index: clean.
- `git status --short --branch` also showed a pre-existing untracked `comment for AI.md`. It was read as context and left untouched. It is not a tracked or staged change.
- No commit, push, reset, restore, clean, or other Git-state change was performed.

## 3. Source boundary and evidence model

The sole Atari disassembly authority used was the project-local [master ROM source](../../Reference/arcade/Tempest%20Rom%20Source%20code.md). No Internet research, external Tempest repository, other disassembly, C port, MAME source, or Incitti implementation was consulted. The current C# implementation, its tests, and the behavior/development authority documents were inspected only for the requested comparison and project context.

The master source itself cautions that some material may be wrong or missing (Markdown L1-L5); uncertain comments were not treated as proof without instruction/data flow. Evidence labels below use only:

- **ARCADE OBSERVED** — Neal's directly reported arcade observation.
- **ROM-BACKED** — behavior traced in the project-local master source.
- **CURRENT C# BEHAVIOR** — current source/tests.
- **PROJECT DECISION / APPROXIMATION** — an accepted recreation choice or non-ROM equivalence.
- **UNKNOWN** — not established by this source trace.

## 4. Neal's arcade observations

**ARCADE OBSERVED:** Neal reports that the accepted C# game is close overall but slightly easier; stationary Blaster plus held Fire is often safe in C#, while that strategy is not viable on his original cabinet, even on Level 1. He reports that a Flipper reaching the rim is dangerous enough to force movement. Neal believes his cabinet is probably set to Medium, but that is not ROM-backed.

The behavior specification independently records the observed completed-pivot rule: visual overlap during a rim pivot is safe; a Flipper becomes lethal when the pivot completes into the occupied lane, and destroying it before completion saves the Blaster (behavior spec Markdown L660-L674). Direct rim arrival is separately **REFERENCE VERIFIED**, not independently arcade-observed there.

## 5. Difficulty option decoding and gameplay adjustment inventory

### Decode and setup path

- `get_diff_bits` reads the low three bits of hardware byte `zap_fire_starts` (`$60d8`); its comment calls these “difficulty/rating bits.” The low two bits are difficulty, while bit 2 is the rating setting (master source Markdown L11256-L11265).
- The documented decode is `00=Medium`, `01=Easy`, `10=Hard`, `11=Medium`; `diff_str_tbl` maps those four indices to the corresponding labels (L255-L258, L2041-L2044). No physical DIP-switch number/name or factory switch position is established in this file.
- `read_optsws` reads hardware options and stores the result in RAM `diff_bits` at `$016a` (L10528-L10558). The per-level parameter routine at `$92c5` reads the level tables and applies the difficulty branch at `$9328` before final speed/tolerance conversion (L2471-L2479, L2597-L2677). New-level setup calls this parameter path (L2190-L2200, L2378-L2383, L9009-L9029): it is a per-level/wave setup adjustment, not a per-shot or per-enemy runtime switch.
- The service/settings display also reads and records the difficulty for display/high-score settings; that is not a second gameplay-tuning path (L5712-L5735, L10709-L10714).

### Complete gameplay-affecting adjustment inventory

| Parameter/path | Easy (`01`) | Hard (`10`) | Scope/effect |
|---|---|---|---|
| `enm_shotmax` | Decrement once | Increment once, capped at 3 | Enemy projectile slot index/capacity. Level 1's table value is 1, and `enm_shoot` scans slots 1 down to 0: Medium = 2 active shots, Easy = 1, Hard = 3 (L2703-L2706, L2597-L2622, L4560-L4581). |
| `spd_flipper_lsb` | Apply the Easy fractional-speed adjustment | Apply the Hard fractional-speed adjustment | Flipper speed is converted by `crack_speed` and is also copied to Tanker speed. For Level 1 the table value `D4` becomes `D9` on Easy and `CF` on Hard; the signed fixed-point direction is toward the player, so Easy is slower and Hard faster. These are ROM-coordinate values, not C# units (L2601-L2629, L2649-L2657, L2677-L2694, L2707-L2710). |
| `flip_top_accel` | Decrement when `curlevel < $11` | No direct change | For Level 1 the table value is 2, so Easy changes it to 1; the top-rim program consumes this value in its turn loop. This changes rim-turn processing on early levels (L2612-L2614, L2849-L2851, L4317-L4332). |
| `wave_enemies` | No direct change | Add approximately one eighth, using the code's shift/ADC calculation | Hard changes total pending enemy supply; Level 1's base `$0a` becomes `$0b` (10 to 11) before `set_enm_and_spikes` copies it to `enemies_pending` (L2630-L2635, L2840-L2842, L2471-L2479). |
| `pulsar_fire` | No direct change | Set bit `$40` | Enables Pulsar firing where Pulsars are eligible. Level 1 has no Pulsars, so this part does not affect the Level 1 Flipper scenario (L2636-L2638, L2853-L2855, L3488-L3490). |
| `hit_tol` derived from Flipper speed | Recomputed after the speed adjustment | Recomputed after the speed adjustment | `crack_speed` returns the speed and a hit-tolerance value; the converted Flipper speed supplies Flipper/Pulsar/Tanker tolerance entries. Level 1 remains tolerance 7 for all three settings because the adjusted Level 1 values retain the same high byte. This is an indirect derived path, not a separate difficulty tolerance table (L2649-L2657, L2677-L2694). |

Not adjusted by the difficulty branch: player Fire sampling/cadence, player-shot slot count/speed/lifetime, enemy-shot speed or holdoff, Spiker speed, `max_enm`, per-type min/max enemy limits, spike height/behavior, or a pending-enemy timing interval. Hard does change total enemy supply; it does not change the concurrent enemy-slot maximum or the pending release schedule. `shot_holdoff` is level-table data and is not changed in the Easy/Hard branch (L154-L163, L2702-L2711, L2840-L2849, L2623-L2638).

**Default conclusion:** `00=Medium` is established as the decode/display value. A hardware cabinet being set to `00` by default is **UNKNOWN**. The zero initializer on the RAM shadow is not evidence of the physical switch setting because `read_optsws` refreshes it from hardware.

## 6. Fire-button sampling and accepted-shot cadence

### ROM-BACKED

- The NMI/input path samples `zap_fire_starts` and updates `zap_fire_debounce` (`$d73e` onward); it separately accumulates edge changes into `zap_fire_new` (master source Markdown L10565-L10616).
- Normal active gameplay calls `player_fire` once from `state_04` after `move_player` (L3016-L3026). `player_fire` reads the debounced Fire level (`$10`), not `zap_fire_new`, so stable held Fire remains eligible each gameplay pass; no press/release edge is required (L4500-L4546).
- The routine returns when the player-state byte at `$0201` is negative. In normal player-controlled play it checks the stable Fire bit, scans slots 7 down to 0, and creates at most one shot in the first empty slot. Eight `ply_shotpos` entries and the scan through slot 7 establish a maximum of eight simultaneous player shots (L335-L345, L4500-L4546).
- There is no player-shot cooldown or per-slot delay in this path. When no slot is free, the scan finds none and accepts no shot. The separate non-player/attract path has an additional `$0106`/nearby-enemy gate; it does not replace the ordinary active-play held-level path (L4500-L4525).
- Successful creation copies `player_along`, `player_seg`, and `$0201` into the shot slot, plays the shot sound, then immediately calls the collision routine at the spawn position. A sound is therefore after slot allocation, not merely an attempted fire (L4526-L4546).

Input sampling/debounce frequency and gameplay frequency are distinct: the input state is refreshed in the interrupt path, while the Fire-level test/shot attempt occurs at the active-gameplay pass.

### CURRENT C# BEHAVIOR

`InputSystem.ReadPlayer` uses `IsKeyDown`/`IsMouseButtonDown`, so Fire is level-triggered, and `Program` samples it for each simulation step. `TempestGame` runs at 60 Hz (`SimulationStepSeconds = 1/60`); the initial timer is 4, so the first held-fire shot can be accepted immediately, then the global timer allows one shot every three simulation ticks while a slot is free. Maximum is ten active shots. Sound fires only after a shot is added. (C# source: [InputSystem.cs](../../src/Tempest.Game/Systems/InputSystem.cs#L27), [TempestGame.cs](../../src/Tempest.Game/TempestGame.cs#L22), [TempestGame.cs](../../src/Tempest.Game/TempestGame.cs#L27), [TempestGame.cs](../../src/Tempest.Game/TempestGame.cs#L80), [TempestGame.cs](../../src/Tempest.Game/TempestGame.cs#L605).)

**Cadence assessment:** C# does not accept shots more frequently than the ROM in source-level gameplay passes: C# has a three-tick global cooldown; the ROM has no such cooldown and can fill/recycle at most one of eight slots per gameplay pass. C# has two more slots. Therefore “C# fires too rapidly” is not established as the primary cause.

## 7. Player-shot lifecycle, speed, and slot reuse

### ROM-BACKED

- Normal Level 1 initializes `player_along` to `$10` (master source Markdown L2465-L2470; RAM comment L303). A player shot starts at that along coordinate and the player's segment.
- `move_shots` moves player shots outward by adding `$09` to the along byte per movement invocation. A shot is retired at `$f0` or later. From `$10`, the ordinary unimpeded lifetime is `ceil(($f0-$10)/9) = 25` player-shot movement invocations. The first movement occurs in the same `state_04` pass as creation because `move_shots` follows `player_fire` (L3016-L3026, L4411-L4452).
- A hit on an ordinary enemy does not immediately clear the player-shot slot; `pshot_hit` keeps scanning targets. A projectile can therefore be tested against multiple objects and survive ordinary enemy hits. Spike contact has a separate slow/retirement path: after a Spike hit the movement increment is reduced, and the special counter can clear the shot after its Spike interactions (L4411-L4470, L4755-L4805).
- The exact free-slot reuse point is after movement reaches `$f0` or a special Spike-retirement condition clears `ply_shotpos`; the slot can then be used on a later `player_fire` pass. Normal enemy hits alone do not release it (L4430-L4452, L4800-L4805).
- Player-shot speed/lifetime are not adjusted by the Easy/Medium/Hard branch.

### CURRENT C# BEHAVIOR

C# creates at `Player.EdgeIndex` and `Player.Depth`, then updates the new shot in the same `RunOneStep`. `Shot.Update` subtracts 6 height units per step; at Level 1's depth 400 it is removed below zero after 67 movement invocations (`ceil(400/6)`). It is removed on a successful target hit, unlike the ROM's ordinary enemy-hit path, and the slot becomes reusable on the next step because firing is checked before movement/removal. Current behavior is in [Shot.cs](../../src/Tempest.Game/Gameplay/Shot.cs#L1), [TempestGame.cs](../../src/Tempest.Game/TempestGame.cs#L605), and [TempestGame.cs](../../src/Tempest.Game/TempestGame.cs#L617).

The 25 ROM calls and 67 C# ticks are not convertible to seconds or directly comparable as raw units: the coordinate ranges and scheduler rates differ. They do show that a C# shot remains in the tube for substantially more C# simulation ticks and can coexist with more shots; that maintains a longer-lived stream in the stationary lane.

## 8. Player-shot collision rules

### ROM-BACKED

- `pshot_hit` scans each live player shot. For enemy targets, the absolute along-coordinate difference must be strictly less than the target's `hit_tol`; the ordinary Level 1 Flipper tolerance derives to 7 from the Level 1 speed table and `crack_speed` (master source Markdown L2677-L2694, L2707-L2710, L4755-L4805).
- The routine checks segment/state data (`enemy_seg`, the shot's `ply_shotseg`, and, while the enemy's in-progress bit is set, the shot-time `$02c0` value). It does not encode a general “always hit from either source and destination lane for the entire pivot” rule. The exact geometric interpretation of `$0201`/`$02c0` is not fully explained by the source comments, so a precise mapping of that branch to two visual pivot lanes is **UNKNOWN** (L4763-L4805).
- The shot is first checked at its creation position inside `player_fire`, then moved. The later moving-shot enemy-hit pass (`pshot_hit`) is after `move_enemies`. This birth-time scan is an important exception: a newly accepted shot can hit a target already at its spawn coordinate before enemy movement (L4526-L4546, L3016-L3026).
- Flipper/player contact is evaluated inside the enemy movement-program actions. `eact_18` skips while the Flipper is in its in-progress state; after its lane/angle is committed the program can test contact again. `eact_20` tests exact enemy/player along and segment equality. In active state the order is `player_fire` (including birth hit check), `move_enemies`/contact, `move_shots`, enemy fire, then `pshot_hit`. Therefore a pre-existing projectile that only hits in the post-movement pass cannot save the player if contact already triggered during `move_enemies`; a birth-time hit can win if its pre-movement check already matches (L3016-L3026, L3920-L3944, L4526-L4546, L4755-L4805).

### CURRENT C# BEHAVIOR

C# moves each shot, then checks Flippers before enemy updates. A Flipper accepts a shot when the shot lane equals its current lane, or either `SourceEdge`/`DestinationEdge` during a pivot, and when `0 < Flipper.Height - shot.Height < 16`. A successful hit removes the shot and the first matching Flipper; it does not continue to another target with that shot. (C# source: [Flipper.cs](../../src/Tempest.Game/Gameplay/Flipper.cs#L192), [TempestGame.cs](../../src/Tempest.Game/TempestGame.cs#L617), [TempestGame.cs](../../src/Tempest.Game/TempestGame.cs#L661).)

The C# collision check is before enemy movement and completed Flipper/player contact. The existing test explicitly protects the shot-before-contact last-pivot-tick outcome and both-lane pivot shootability ([SimulationTests.cs](../../tests/Tempest.Game.Tests/SimulationTests.cs#L5054), [SimulationTests.cs](../../tests/Tempest.Game.Tests/SimulationTests.cs#L5379)).

**Mismatch:** C# provides a full-pivot two-lane hit window and checks it before contact. ROM has a state-sensitive hit test and enemy movement/contact before its post-movement hit pass, although it also performs the immediate birth-position scan. This is the strongest specific mechanism by which C# can make stationary held fire safer.

## 9. Flipper rim arrival and pursuit

### Level 1 ROM trace

- The level table selects Flipper movement program `$07` for Level 1; `$a0fe` is the “just move up” program. The Level 1 Flipper therefore climbs without below-rim lane changes, using the per-level Flipper speed (master source Markdown L2857-L2865, L3484-L3486, L4250-L4255, L2707-L2710).
- `eact_0c` advances `enemy_along` by the fixed-point Flipper speed. When the position reaches the player end it calls `0c_at_top` and transfers the p-code PC to the shared top program (L3706-L3735, L3785-L3792, L4317-L4332).
- At arrival, `eact_24` selects direction toward the player using the signed/wrapped segment difference. If the rim-enemy count indicates another Flipper already on the rim, the arrival branch finds that enemy and uses its opposite direction; this is a genuine enemy-dependent selection, not a general random choice (L3785-L3825, L3827-L3838).
- The chosen direction is retained by the rim program: it does not call `eact_24` again on each lane move. The common top routine at `$a139` checks contact, uses a four-count loop, starts/continues a flip, and reloads `flip_top_accel`; Level 1's table value is 2, causing two `eact_14` angle actions per execution pass before the loop resumes. The actual number of angle actions required to finish a lane is dependent on the level angle data, which the master source represents as a `.chunk`, so exact full lane-to-lane timing is **UNKNOWN** (L3842-L3929, L3930-L3965, L4317-L4332, L2849-L2851, L7470-L7480).
- Closed-tube segment arithmetic wraps around; open-tube endpoints use the separate `rev_if_edge` path. Levels 1-7 use closed tube definitions, so the Level 1 pursuit route wraps (L3938-L3970, L7470-L7480).
- Collision is checked at direct rim arrival and after a completed pivot. The enemy's in-progress flag suppresses the top grab check during the pivot; the enemy segment is committed at completion. This agrees with the project's directly observed rule for completed pivots, while the direct-arrival detail remains source-derived in the project spec (master source L3785-L3825 and L3920-L3957; behavior spec Markdown L660-L674).

### Current C# Level 1 trace and levels 1-7

Level 1 uses `ClimbSpeed=2`, `TurnSpeedDegrees=8`, `RimPauseTicks=24`, and `CanFlip=false`. At the rim it selects shortest direction to the Blaster once, latches that pursuit direction, and proceeds to adjacent lanes; it does not account for another rim Flipper when selecting that direction. A Level 1 regular lane pivot is about 22.5 degrees, so the 8-degree update takes three simulation ticks, followed by the 24-tick pause before the next lane move. C# therefore uses about 27 ticks per completed adjacent move under that geometry. (C# source: [LevelConfiguration.cs](../../src/Tempest.Game/Levels/LevelConfiguration.cs#L29), [LevelConfiguration.cs](../../src/Tempest.Game/Levels/LevelConfiguration.cs#L101), [Flipper.cs](../../src/Tempest.Game/Gameplay/Flipper.cs#L74), [Flipper.cs](../../src/Tempest.Game/Gameplay/Flipper.cs#L347).)

Levels 1-5 use 8-degree/24-tick rim settings; Level 6 and 7 use 9-degree/23-tick settings. Level 7 also has a distinct below-rim route. In the ROM, level-selected Flipper movement programs can change the climb/approach path, but reaching the rim hands off to the shared top program; level tables also select speed/acceleration values. Thus the general rim handoff applies across the implemented levels, while arrival timing and below-rim movement can vary. Exact ROM Level 1-7 lane cadence is not comparable to the C# values from this file alone.

## 10. Main gameplay scheduling and update order

### ROM-BACKED

- The main loop waits until counter `$53` has reached at least 9, resets it, and dispatches a game-state routine. The NMI path increments `$53`; input/debounce is sampled in that path. This establishes source-level scheduling of one game-state pass per nine counter increments, not a physical frame rate (master source Markdown L8924-L8963, L10565-L10616).
- `state_04` active play order is: `move_player` -> `player_fire` -> zap check -> enemy creation -> `move_enemies` -> `move_shots` -> enemy shooting -> `pshot_hit` -> hit/death queue processing -> final state/wave check (L3016-L3026).
- Each active enemy is visited by `move_enemies` once per state pass and its p-code runs according to its program. Each player shot slot is visited by `move_shots` once per pass. Enemy shot firing has its own per-enemy holdoff, random test, and slot scan; this is not the player-shot cadence (L3554-L3560, L4411-L4452, L4546-L4581).
- The level-end descending state (`state_20`) also calls player movement, firing, and shot movement; it is not the same ordering as active enemy combat (L3026-L3035). The death-state routine is separate from state_04. The complete input/fire behavior for every transition/zoom substate is not proven here; where no direct active-fire call was traced it is not used to infer the Level 1 stationary scenario.

### CURRENT C# BEHAVIOR

`Program` samples input once for each `RunOneStep`; simulation is fixed at 60 Hz. In active play, C# does player firing then player movement, player shots and enemy bullets, then Egg/enemy updates, Flipper firing, and player-collision checks. Consequently shot collision precedes enemy movement and Flipper/player contact. C# approach updates movement without firing; ordinary exit travel updates movement without firing, while the special Spike-exit controller permits firing; death clears/freezes active combat shots. (C# source: [Program.cs](../../src/Tempest.Game/Program.cs#L148), [TempestGame.cs](../../src/Tempest.Game/TempestGame.cs#L236), [TempestGame.cs](../../src/Tempest.Game/TempestGame.cs#L314), [TempestGame.cs](../../src/Tempest.Game/TempestGame.cs#L555), [TempestGame.cs](../../src/Tempest.Game/TempestGame.cs#L1811).)

## 11. ROM-versus-C# mismatch matrix

| Mechanic | Master ROM behavior | Current C# behavior | Match? | Likely stationary-fire consequence | Evidence |
|---|---|---|---|---|---|
| Fire trigger/cadence | Debounced held level; one attempt per gameplay pass; no player cooldown | Held level; global 3-tick cooldown; first shot immediate | Mismatch | C# cadence alone is slower, so it does not explain easier play by itself | ROM-BACKED / CURRENT C# BEHAVIOR |
| Player-shot capacity | 8 slots | 10 slots | Mismatch | C# can maintain two more lane shots, but ROM can fire faster | ROM-BACKED / CURRENT C# BEHAVIOR |
| Player-shot lifetime | +9 along per move; `$10` to `$f0`, 25 movement calls absent hits/Spike slowing | -6 height per tick; 400 to below 0, 67 calls | Mismatch; coordinate systems differ | C# keeps lane coverage in flight longer and for more ticks | ROM-BACKED / CURRENT C# BEHAVIOR |
| Ordinary enemy hit consumes shot | Normal enemy hit does not retire slot; scan continues | C# removes shot on hit and returns after first target | Mismatch | This ROM behavior can be more effective against clustered targets; not an explanation for C# being easier | ROM-BACKED / CURRENT C# BEHAVIOR |
| Flipper pivot shootability | Tolerance plus in-progress state and shot-time coordinate checks; exact two-lane mapping not established | Both source/destination lanes accepted throughout pivot | Mismatch/ROM mapping partly unknown | C# gives stationary lane fire more chances to hit a Flipper entering that lane | ROM-BACKED / CURRENT C# BEHAVIOR / UNKNOWN |
| Shot/contact ordering | New-shot spawn check first; then enemy movement/contact; moving-shot hit after movement | Shot moves/hits before enemy movement/contact | Mismatch | C# can destroy a Flipper on the same tick its pivot would become lethal | ROM-BACKED / CURRENT C# BEHAVIOR |
| Rim pursuit | Level 1 straight climb; shared top program; target direction selected at arrival, with another-rim-enemy branch; top count 4 and acceleration 2 | Rim pause 24; 8-degree pivots; shortest direction latched; no other-enemy rule | Mismatch; exact ROM lane cadence unknown | Longer C# pauses may change threat timing and provide more opportunities for parked-lane shots | ROM-BACKED / CURRENT C# BEHAVIOR / UNKNOWN |
| Difficulty | Easy/Medium/Hard modify enemy threat and supply as listed above; no player-fire changes | No selectable difficulty/DIP policy; per-level fixed settings | Mismatch in feature surface | Actual cabinet setting cannot be compared until verified; Level 1 hard/easy mainly changes enemy supply, movement and enemy shots | ROM-BACKED / CURRENT C# BEHAVIOR / UNKNOWN |
| Level variants | `flipper_move`, speed, top acceleration are table-driven; common rim handoff | C# Levels 1-5 share rim settings; Levels 6-7 use 9-degree/23-tick config; Level 7 has its own below-rim route | Partial match | Level 1 behavior is directly relevant; Levels 1-7 should not be assumed identical below the rim | ROM-BACKED / CURRENT C# BEHAVIOR |

## 12. Level 1 stationary hold-fire symbolic comparison

### A. Master ROM

1. Hold Fire until the NMI path has stabilized the debounced Fire bit. On each active `state_04` pass, `player_fire` checks that level and allocates at most one of eight free slots.
2. The shot starts at the player's along coordinate (normally `$10`) and segment. It is checked against targets immediately at birth, then it moves +9 during `move_shots`. With no target/Spike interaction, it retires after 25 movement invocations at `$f0`.
3. A Level 1 Flipper climbs in its selected lane using the `07` straight-move program. On reaching the player end, it is lethal if its arrival checks match the Blaster. Otherwise the top handoff selects a pursuit direction toward the Blaster; if another rim Flipper is already there, the direction-selection branch uses that enemy's opposite direction.
4. The Flipper repeats the shared rim program, which checks contact, waits on a four-count, and executes top-accelerated turning. It retains that direction in the rim program and wraps around the closed tube.
5. A shot fired in the Blaster's lane can hit only when ROM's position tolerance and lane/state comparisons match. If an already-moving shot's hit is first recognized after a rim contact in the same pass, the contact has already run. A newly created shot can instead win through its earlier birth-position check if the enemy already matches that test.

Direction branches are thus: no other rim Flipper -> toward the player by signed/wrapped segment difference; another rim Flipper at the top -> opposite-direction selection. The direction is not re-sampled each lane. The ROM helper is arithmetic, not a random draw; unresolved geometric interpretation of the pivot hit branch remains **UNKNOWN**.

### B. Current C#

1. Held Fire yields an immediate first shot and then one accepted shot every three ticks until ten are active. A new shot is created at the player's current lane/depth, moves 6 units in the same step, and remains until below zero (67 moves at Level 1) or it hits.
2. The Level 1 Flipper climbs at 2 height units per tick. On arriving at height 400 in a different lane it pauses 24 ticks, picks shortest direction toward the stationary Blaster, and latches that direction. On a regular Level 1 adjacent pivot it takes three 8-degree updates, then pauses before its next lane.
3. Each C# shot is checked before Flipper movement/contact. During a pivot it can hit from either participating lane over the 1-15-unit strict height separation band. A hit removes the Flipper before the later player-collision phase.
4. Therefore, a stream of long-lived shots occupying the Blaster lane can destroy a C# Flipper on a pivot step before contact resolution. In the ROM, the equivalent moving-shot hit pass follows enemy movement/contact; its immediate birth scan is a distinct earlier opportunity, not a blanket pivot-lane hit.

**Result:** the source supports a mechanism for C# being safer: longer-lived lane coverage, more active slots, broad pivot-lane shootability, and shot-before-contact ordering. ROM's higher attempt frequency means cadence itself is not a one-direction explanation. Exact collision probabilities and which factor dominates remain unmeasured.

## 13. Root-cause assessment

**Selected category: G — demonstrated combination of identified factors.**

Ranked candidates for the stationary hold-fire discrepancy:

1. **Collision ordering and pivot shootability (strongest demonstrated mechanism):** C# tests a shot against both pivot lanes before the Flipper moves and before lethal contact is consumed. The ROM's moving-shot hit pass is after enemy movement/contact, though a new shot's immediate birth scan can win in the narrower pre-movement case.
2. **Shot occupancy/lifetime (strong contributor):** C# has 10 slots and a 67-tick shot lifetime versus the ROM's eight slots and 25 movement invocations. The project and ROM units differ, but C# maintains more persistent lane coverage. C#'s three-tick cooldown is lower throughput than one attempt per ROM pass, so do not blame a faster C# fire cadence.
3. **Rim pursuit timing (plausible contributor):** C# has a 24-tick pause; ROM Level 1 uses a four-count rim program and top-turn factor 2. Since the master angle data is not decoded in this file, the full ROM lane-change interval and exact relative speed are **UNKNOWN**.
4. **Difficulty (not established as the cause):** the C# game has no selection and the cabinet's actual switch setting is not confirmed. The ROM's difficulty changes are mostly enemy threat/supply, not player shooting.

The source explains that stationary play is not safe from eventual ROM Flipper pursuit/contact, but it does not prove that the arcade must kill a parked player under every random/wave state. A deterministic Level 1 trace and manual comparison are needed to measure the overall survival difference.

## 14. Values/routines for later implementation attention

ROM: option extraction `$dbe0`/`get_diff_bits`, `$d6bb`/`read_optsws`; per-level difficulty `$92c5` and `$9328-$93df`; active scheduling `$c7a7`, `state_04` `$970b-$9726`; player firing `$a241-$a2a5`; shot movement `$a18f-$a1e3`; immediate and moving shot tests `$a463`/`pshot_hit`; Flipper movement `$9c57`/`$9d05`, direction `$9d67`, pivot `$9d82`, contact `$9e2e-$9e5b`, rim program `$a139-$a149`.

C#: `TempestGame.RunOneStep`, `UpdatePlayerFiring`, `UpdatePlayerShots`, `CheckShotAgainstFlippers`, `CheckFlippersAgainstPlayer`; `Flipper.Update`, `BeginPivot`, `IsHitBy`; Level 1/6/7 `FlipperConfiguration` and LevelConfiguration shot policy.

## 15. Remaining unknowns

- The arcade cabinet's actual difficulty bits and whether its normal/factory setting is Medium.
- The physical frequency of an NMI and the conversion from nine counter increments to real time. No seconds conversion was attempted.
- Exact ROM lane-to-lane rim-turn duration: the source's level-angle table is represented as `.chunk` data, and the author's explanatory comments are not independently sufficient to fill the missing values.
- Exact mapping of the in-progress Flipper collision compare against `$02c0` to a visual source/destination lane.
- The probability that a parked Blaster survives a full Level 1 wave under arcade RNG, including the birth-time shot check and enemy crowding.
- Whether difficulty-shifted derived hit tolerances numerically differ on every later level; Level 1 remains 7 in all three modes.
- A measured C# versus arcade count/interval of visible Blaster shots or rim lane changes.

## 16. Recommended next milestone

Do not change production firing cadence or add DIP/menu support yet. First create a deterministic Level 1 held-fire comparison harness that isolates (a) the immediate birth-position shot check, (b) an in-flight hit on the final pivot/contact pass, (c) pivot lane eligibility, and (d) shot-slot/lifetime behavior. Preserve the ROM/C# ordering distinction and record the arcade's actual difficulty setting before treating Medium as the expected baseline.

If that comparison confirms the source-level race is the perceptual driver, the narrow production milestone should align Level 1 Flipper shot/contact semantics and rim pacing to the ROM-derived path, with each intentional approximation explicitly labeled. Do not change global cadence or shot speed based only on the current mismatch matrix.

## 17. Suggested future automated tests (not added)

- ROM-model held-Fire test: one accepted shot per gameplay pass when a slot is free, eight slots, spawn coordinate/segment, no cooldown, and correct empty-slot reuse.
- Unimpeded ROM-model lifespan: `$10` to `$f0` at +9 gives exactly 25 movement invocations; separate Spike-slow/retirement cases.
- Birth-position collision versus post-movement collision ordering, including a final-pivot contact race where contact wins only if the moving-shot test is later.
- Flipper exact hit-tolerance and pivot-state/shot-time-lane branches; do not encode blanket two-lane acceptance without evidence.
- Level 1 straight climb, rim-arrival direction selection, the other-rim-enemy branch, latched direction, four-count pause, and top factor 2. Keep exact lane-turn expected ticks **UNKNOWN** until angle data is available.
- Difficulty decode and Level 1 parameter tests: `00/11=Medium`, `01=Easy`, `10=Hard`; enemy-shot capacity 2/1/3; Flipper speed fraction and Easy top factor; Hard Level 1 supply 11; no player-shot parameter changes.
- Preserve accepted C# regression coverage for Levels 1-7 and test any future implementation separately from the arcade-model expectations.

## 18. Suggested manual acceptance

After deterministic tests and any later approved change, Neal should use Level 1, verify the cabinet's actual difficulty setting, park the Blaster in one lane, and hold Fire continuously on both machines. Observe whether a rim Flipper forces movement or kills the stationary Blaster. Separately compare approximate visible shot count/interval, rim lane-change cadence, survival duration on the rim, and whether a Flipper survives long enough to reach the parked lane. Hidden counters or frame analysis should not be required unless the deterministic test cannot distinguish the ordering candidates.

## 19. Final repository/Git state

The only investigation-created files are the intended ignored AI records:

- `AI-Logs/Prompts/2026-11-04-blaster-fire-rim-difficulty-rom-investigation-prompt.md`
- `AI-Logs/Reports/2026-11-04-blaster-fire-rim-difficulty-rom-investigation-report.md`

Final requested validation was run:

- `git status --short --branch`: `## master`, plus only the pre-existing `?? "comment for AI.md"`.
- `git rev-parse HEAD`: `3bebf42cbe9514846ab32bf4c8a12bcfdb048da7`.
- `git diff --stat` and `git diff --cached --stat`: empty.
- `git check-ignore -v` confirmed both new AI records are ignored under `.gitignore:6:AI-Logs/`.

The accepted baseline is unchanged; the pre-existing untracked `comment for AI.md` remains untouched. No production, test, authority-document, or reference file was changed. The game was not launched; no build/test was run; no commit/push or Internet/disassembly search occurred.
