# Next Tempest Gameplay Milestone Analysis

Labels used below: **A** means explicitly specified by Incitti source; **B** means inferred by connecting source paths; **C** means the source does not establish original Atari behavior, so it requires arcade comparison. A source fact is not by itself proof of arcade accuracy.

## 1. Progression Findings

Incitti uses zero-based `current_level` (`lv`) and `current_board = current_level Mod 16`; the HUD displays `current_level + 1`. Sixteen-board geometry sets repeat with palette changes. The enemy eligibility flags are assigned in `SetUpEnemies`, not encoded in levelset rows. Each of the seven supplied levelsets has 16 comment/data pairs; each data row has 35 fields for continuity, center/offset and 16 coordinate pairs. **[A]** See [level setup](../Reference/incitti/tempest.bmx#L4208), [level index mapping](../Reference/incitti/tempest.bmx#L1426), and representative [levelset1](../Reference/incitti/levelsets/levelset1.txt).

Eligibility by first displayed level in the original set:

| Feature | Incitti flag/condition | Earliest displayed level | Classification |
|---|---|---:|---|
| Flippers | Always enabled | 1 | A |
| Tankers | `current_board > 1 || lv > 17` | 3, at board index 2 | A |
| Spikers (called Spinners in source) and spikes | `current_board > 2 || lv > 47` | 4, at board index 3 | A |
| Fuseballs | `current_board > 9 || lv > 20` | 11, at board index 10 | A |
| Pulsars | `lv > 15` | 17 | A |
| Tanker-carried Pulsars | `lv > 26` | 28 | A |
| Tanker-carried Fuseballs | `lv > 31` | 33 | A |
| Rotation mode | `lv > 53` | 55 | A |

Therefore Tankers are the next gated enemy after Flippers in Incitti's progression and are available before Spikers. The recent C# Spiker wave is explicitly development-only, not evidence that Spikers are the next normal enemy. **[B]** The source/readme describe Incitti's implementation; whether these exact unlock levels match the original arcade remains **[C]**.

Hatches choose among enabled types using `PickEnemy`. Negative random choices map to Spinner; disabled types are rerolled. Tanker count limits the selection range at six. When the remaining-enemy plus active-Egg count is below four, or there are more than three Spinners, the selection rerolls without Spinners. **[A]** See [enemy selection and hatch dispatch](../Reference/incitti/tempest.bmx#L1982).

## 2. Candidate Milestones

### Enemy bullets/projectiles

- Bullets have no level unlock: they are already part of source Flipper behavior from the start, and Tankers and Spinners can also fire them. **[A]** The C# `EnemyBullet` scaffold is empty, and current C# Flippers do not fire them.
- They are a compact, separately countable projectile, not an enemy in wave accounting. Their mechanics provide a prerequisite for a faithful Tanker implementation. **[B]**
- They can be built around the existing `EnemyBullet` scaffold, `Shot`, player death/reset, and renderer without adding another enemy type or cargo system. This is the narrowest candidate. **[B]**

### Tankers

- Tankers are the next gated enemy: first eligible on displayed Level 3. At that tier, Flippers are enabled but Fuseball/Pulsar cargo is not, so the eligible Tanker hatch carries two Flippers. Later carrier cargo choices are separately gated. **[A]** See [hatch/cargo selection](../Reference/incitti/tempest.bmx#L1995) and [eligibility flags](../Reference/incitti/tempest.bmx#L4236).
- A Tanker is a fixed-lane carrier that climbs from height 10. It either releases cargo near the rim, is shot for 200 points and releases live cargo, or is zap-killed for 200 points and releases stillborn cargo. It has no direct player collision. It can emit bullets. **[A]**
- The first Tanker tier still needs cargo release, enemy-count transfer, Tanker firing, and the shared enemy-bullet projectile. Tanker collision/scoring also has a zap path coupled to the currently empty SuperZapper system. **[B]** This is a sensible following enemy milestone but larger than the bullet system.

### Fuseballs

- Direct Fuseball hatching first becomes eligible on displayed Level 11 (board index 10); the alternate `lv > 20` condition first applies on displayed Level 22. Tanker Fuseball cargo is later, first enabled on displayed Level 33. **[A]**
- Fuseballs move laterally across edge segments, wrap on continuous tubes or bounce on open tubes, and alternate lateral motion with climbing/descending. The source uses a `w` position from 0 to 8, an interior collision band `w > 1 && w < 7`, and random direction/phase choices. **[A]**
- Hits award height-based 250-point increments from 250 to 1250 and create a timed score popup. Fuseballs can kill the player only when moving laterally at the rim in the interior band. They have their own animation data and frame timer. Zap death also scores. **[A]**
- They are independent when directly hatched, but their animation data, score-popup rendering, zap behavior, state machine, and lateral collision geometry are considerably broader than bullets. **[B]**

### Pulsars

- Direct Pulsars first become eligible on displayed Level 17; Tanker-carried Pulsars first become eligible on displayed Level 28. **[A]**
- Pulsars slide up/down, pause at rim/intermediate points, and flip between lanes using shortest-path direction on continuous tubes. Random choices select whether they flip or reverse at endpoints. They share a global pulse oscillator; while pulsing they change color and set edge-pulse flags that alter the tube-web rendering. **[A]**
- Same-edge shots in the strict 0-to-16 height band award 200 points. Player collision includes a near-rim branch and a pulsing climb/descent branch with different source death classifications. **[A]**
- A Pulsar needs a shared timer, edge flags, pulse rendering, collision states and random phase decisions. It is not a small isolated entity addition. **[B]**

### Other relevant mechanics

- SuperZapper has two charges in the reference and is used by Space/right mouse. Strength 2 attempts to zap all non-spike baddies; strength 1 selects one threat by edge distance, height and type. Each enemy's `ZapIt` controls its delayed death/scoring. Bullets return false from `ZapIt`, so they are not zapped. **[A]** See [SuperZapper dispatch](../Reference/incitti/tempest.bmx#L2257). The C# `SuperZapperSystem` is empty.
- Spike avoidance is a level state for source levels 4–7, not a new enemy. Pulsars, Fuseballs, Tankers, bullets and other baddies are treated specially during zoom/reverse-zoom; only spikes update, collide and render in those states. **[A]** See [baddie state filtering](../Reference/incitti/tempest.bmx#L2183) and [exit-state handling](../Reference/incitti/tempest.bmx#L1344).
- Bonuses, audio cues and source score popups accompany several entities but are not prerequisites for a first projectile milestone. The source `Readme.txt` is version 3.1 and notes historical Fuseball hitbox/spike-damage adjustments; it does not define enemy composition. The readme says three tube sets, while seven files are present, so that control text is stale. **[A/B]**

## 3. Dependencies

The existing C# project has empty `EnemyBullet`, `Tanker`, `Fuseball`, and `Pulsar` subclasses of `Enemy`. `TempestGame` currently owns shots, Flippers, Spikers and a development collision-order list; it has no bullet collection. `Flipper` has no ammunition, bullet timer or firing method. `VectorRenderer` draws player shots and enemy bodies but no enemy bullets. The source-faithful level progression and SuperZapper are not implemented. **[A: current C# inspection]**

A bullet does not increment the ten-enemy supply, and must not be added to `EnemiesDestroyed` or active-enemy completion counts. Death reset removes bullets while returning surviving enemies to Eggs. **[A]** If a projectile remains at level clear, the C# exit should freeze or clear it in a narrowly specified way; it must not alter the already validated spike-free exit controls or timing. **[B]**

Incitti tests shots against a shared ordered `BADDIE_LIST` and stops at the first successful hit; collision is insertion order, not nearest depth. Current C# Flipper collisions iterate a dedicated list in reverse. **[A/B]** A bullet integration must make its interaction priority explicit and test conflicts without silently broad-refactoring the enemy hierarchy or changing known Flipper pivot rules.

## 4. Recommended Next Milestone

**Implement EnemyBullet gameplay for the existing Flipper as the next milestone.**

This is one cohesive projectile milestone, not a Tanker implementation. Incitti's Flippers already fire bullets from the earliest level, so this fills a source-backed Level 1 gameplay gap before introducing a new enemy. It is also the smallest reusable foundation for the Tanker, which is the next gated enemy at displayed Level 3 and itself can fire bullets. Tanker cargo and carrier accounting can then be tackled as a separate focused milestone.

Keep the Flipper climb/pause/pivot/collision state machine and player controls unchanged. Initially expose deterministic firing in a controlled development/test scenario (for example `--enemy-bullets-test`) so bullet timing and arcade feel can be manually checked without silently changing the usual launch experience. Promote source-faithful Flipper firing to normal Level 1 only after manual comparison. This temporary opt-in staging is a development choice, not source behavior. **[B/C]**

Do not include Tankers, cargo, Fuseballs, Pulsars, SuperZapper, enemy bullets from Spikers, or enemy bullets from Tankers in this milestone. They are later integration work; the projectile abstraction should be reusable but should not activate those sources yet.

## 5. Source-Backed Behavior for Recommended Milestone

- **Flipper ammunition and timing [A]:** On creation, a Flipper gets `Rand(1,3)` bullet attempts and `bullettime = 16`. Its `Shoot()` method is called during its climbing state and after a rim-pause edge transition. Each call decrements the timer; once it is below zero, the timer resets to 16. A bullet is created when `Rand(0,100) > 75`, and only a successful creation consumes one attempt. `Rand` is the integer API; tests should control both ammunition and random outcomes rather than rely on a probabilistic test.
- **Bullet creation and movement [A]:** A bullet stores the firing lane and starting enemy height. Each update adds 3 height units. It is removed when its height becomes greater than the level depth (400); height exactly 400 is not expired by that condition.
- **Player collision [A]:** The bullet must share the player's edge and satisfy `Level.Depth - bullet.Height < 8`. The comparison is strict; a gap of exactly 8 does not hit. The player is killed as `KILLED_BY_BULLET`; source death handling starts the 60-update death countdown.
- **Player-shot collision [A]:** A shot must share the bullet's edge and have `shot.Z < bullet.Height`. There is no 16-unit separation band. A hit consumes the shot, removes the bullet, and awards 25 points; it does not count as an enemy kill.
- **Accounting/reset [A/B]:** Bullets are not counted as enemies and do not gate level completion. Death conversion explicitly removes bullet baddies while preserving/resetting enemy supply separately. Score from shooting a bullet persists through life loss.
- **Ordering [A]:** The source updates player shots before Eggs and baddies; baddies update before their separate player-collision pass. The local C# game already processes shots before completed Flipper-to-player collision. Add bullet movement and bullet/player collision at a deliberately chosen nearby point; do not globally adopt Incitti's entire update order.
- **Rendering [A]:** Bullets are white, projected on their lane at `position + depth - height`, drawn as rotating radial line segments with projected size based on `TFormSZ(6,z)`. Rotation advances 15 degrees per draw. Existing player shots use a related but distinct size and orientation; avoid changing their established rendering.
- **SuperZapper [A]:** Bullet `ZapIt` returns false, so it is not destroyed by the reference SuperZapper. The C# SuperZapper remains out of scope.
- **Flipper state [C]:** Source bullet firing is intertwined with climbing and post-pause movement. Whether the exact cadence, ammo count, chance, collision reach, score, and visual effect match original Atari Tempest requires manual comparison. The source also seeds randomness from `MilliSecs`, so its run sequence is intentionally nondeterministic.

## 6. Expected C# Integration Points

- `Gameplay/EnemyBullet.cs`: replace the empty scaffold with lane, height, rotation/render state, update/expiry, `IsHitBy(Shot)`, and `HitsPlayer(Player)` behavior. It may reuse `Enemy` data shape if kept out of enemy accounting; do not increment level enemy totals for bullets.
- `Gameplay/Flipper.cs`: add bullet ammunition/timer and a narrow firing callback/decision input at the source's existing Shoot call points. Preserve all existing movement and pivot formulas. Avoid embedding Raylib, audio, or global randomness in the entity.
- `TempestGame.cs`: own active bullets, create them on requested Flipper shots, update them, process their shot hits and player collision, award 25 points, clear them during death reset, and ensure they do not prevent level clear. Use a deterministic random seam compatible with the existing injected `Random` constructor; the controlled test path should fix ammo and firing decisions.
- `Rendering/VectorRenderer.cs`: draw bullets using the existing tube projection and a separate source-sized white radial shape. Keep player shot geometry unchanged.
- `Debug/DebugController.cs` and `Program.cs`: only if a separate `--enemy-bullets-test` path is chosen; reuse the current small argument parsing rather than adding a general configuration system.
- `tests/Tempest.Game.Tests/SimulationTests.cs`: focused projectile tests plus the existing gameplay regressions below. No new files are necessary.

## 7. Proposed Automated Tests

- Flipper initializes the expected bounded ammunition count and timer under controlled random decisions; disabled/no-ammo Flippers never fire.
- A deterministic schedule proves the 16-tick timer boundary and the `> 75` firing condition; failed chance does not consume ammo, successful fire consumes one.
- Bullet spawns on the Flipper's current lane and height; it advances exactly 3 per update and expires only after exceeding depth 400.
- Player collision requires same lane and gap strictly less than 8; gap exactly 8 misses. A hit removes the bullet and enters the existing 60-tick death state once.
- Player shots use strict `shot.Z < bullet.Height`, require same lane, consume the shot and award 25; equality and wrong-lane shots miss.
- Bullet shooting never increments `EnemiesDestroyed`, never changes remaining-enemy accounting, and residual bullets do not prevent level clear.
- Explicitly test bullet/Flipper collision priority under the chosen source-compatible insertion-order policy; do not rely on distance sorting.
- Death reset removes bullets, preserves score and previously defeated enemies, and applies the existing single-life decrement. Game Over/restart and exit behavior stay unchanged.
- Rendering callback/projection tests verify finite coordinates, correct lane/depth, white radial shape and rotation progression without Raylib initialization.
- Existing Flipper climbing/pause/pivot/shootability/collision tests, Spiker/spike tests, normal Level 1 clear/exit tests and the prior 90-test suite remain green.

## 8. Manual Arcade-Comparison Checklist

- Confirm Flipper bullets originate at the Flipper's lane/height and move outward toward the rim at the expected speed.
- Compare ammunition count, launch cadence and random pauses while a Flipper climbs and changes lanes.
- Check white bullet size, rotation, perspective and visual contrast against player shots.
- Verify exact same-lane collision, short rim danger zone, player explosion/death timing and bullet removal.
- Shoot bullets and verify shot consumption and 25-point score presentation.
- Confirm bullets do not alter enemy counts or clear/replay accounting.
- Recheck spinner direction/sensitivity, cursor capture, Flipper pivot safety, Spiker development scenario, and normal spike-free exit timing.
- After manual acceptance, decide whether to enable source-backed Flipper firing in the ordinary launch path; do not infer acceptance from tests alone.

## 9. Remaining Uncertainties

- **C:** The source documents Incitti's implementation, not verified Atari ROM/arcade behavior. Bullet ammo/cadence, random thresholds, hit reach and score need direct arcade comparison.
- **C:** Whether Flippers fire bullets from the exact initial conditions/frequency shown in this source on the original arcade machine is not established by this repository.
- **C:** The source's baddie list iteration while Flippers append bullets may affect whether a newly created bullet is updated in the same pass; inspect/run the original BlitzMax runtime or compare gameplay before depending on same-tick behavior.
- **C:** The source's `LEVEL_COMPLETE` path permits `onrim` completion with remaining enemies in some cases, unlike the current tested C# kill-all condition. Do not copy that unrelated rule into the bullet milestone.
- **C:** Tanker's first-level progression and its resemblance to original Atari progression are source facts only; arcade comparison remains authoritative.

## 10. Explicitly Deferred Features

- Tanker carrier, cargo release, Tanker-origin bullets and Tanker zap scoring; Tanker is the next gated enemy after the recommended projectile milestone.
- Fuseball states, animation data, height-based score popups and collision band.
- Pulsar movement, global pulse oscillator, edge flags and pulse collision/rendering.
- Spiker-origin retreat bullets.
- SuperZapper input, targeting, charges and delayed `ZapIt` lifecycle.
- Audio, bonuses, full levelset progression, later enemies and Level Complete/exit redesign.
- Normal-launch activation of new Flipper firing until manual arcade comparison accepts the feature.
