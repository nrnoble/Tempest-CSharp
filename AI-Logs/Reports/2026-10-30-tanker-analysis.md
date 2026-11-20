# Tanker Milestone Analysis

Labels: **A** = explicitly established by Incitti source; **B** = inferred from interacting source paths; **C** = not established as original Atari behavior and requires manual arcade comparison. Source behavior is not automatically arcade behavior.

## 1. Progression and Eligibility

Incitti uses zero-based `current_level` (`lv`) and `current_board = current_level Mod 16`; the HUD displays `current_level + 1`. Tankers are enabled by `current_board > 1 || lv > 17`. The first eligible original-set level is therefore displayed Level 3 (zero-based board index 2). The later `lv > 17` branch starts at displayed Level 19. **[A]** See [SetUpEnemies](../Reference/incitti/tempest.bmx#L4208) and [level index mapping](../Reference/incitti/tempest.bmx#L1426).

At the earliest eligible tier (`lv=2`, `current_board=2`), Flippers are enabled; Fuseballs and Pulsars are disabled. The Tanker hatch cargo selector rerolls unavailable types, leaving the two-Flipper carrier choice. Tanker-carried Pulsars are not enabled until `lv > 26` (displayed Level 28); Tanker-carried Fuseballs until `lv > 31` (displayed Level 33). **[A]** See [Egg.Hatch](../Reference/incitti/tempest.bmx#L1995) and [progression flags](../Reference/incitti/tempest.bmx#L4225).

Spikes are enabled at `current_board > 2 || lv > 47`, so they are absent on the earliest Tanker tier and first occur at displayed Level 4 by board progression. **[A]**

The seven supplied levelsets contain tube geometry/topology and comments, not enemy composition. The reference readme describes an older three-set control even though seven levelset files are present. **[A/B]** No C# Tanker wave should be inferred from geometry files.

Whether Incitti's unlock levels match the original Atari arcade is **[C]**; the table above is only the source progression.

## 2. Tanker Spawn and Movement

An Egg is selected through `PickEnemy`; Tanker is candidate type 4. The type range includes candidate 4 while `numtankers < 6`, and excludes it at six. If selected, Tanker cargo is chosen by a separate random `Rand(0,2)` loop, rerolling disabled cargo. At the earliest Tanker tier, only cargo case 0 is enabled and calls `Tanker.Create(edge, 2, 0, 0)`. **[A]** See [selection and hatch](../Reference/incitti/tempest.bmx#L1982).

The Tanker is created on the Egg's lane at height `10`, with cargo fields set, an initial bullet attempt count of `Rand(1,2)`, and `bullettime = 16 + Rand(64)`. It registers in the shared baddie list, increments `enemiesleft`, and increments `numtankers`. **[A]** See [Tanker.Create](../Reference/incitti/tempest.bmx#L2622).

It remains on that lane and has no ordinary pause or lateral movement state. Each normal update adds `tankerclimbspeed`, configured as `1.75 + lv/20`. At `lv=2` this is approximately 1.85 if BlitzMax evaluates the division fractionally; verify the original runtime's numeric semantics before treating that decimal as exact. **[A/B]**

After increasing height, it may fire bullets while height is below 380. It naturally releases cargo and exits only when its updated height is strictly greater than `depth - 15`, i.e. greater than 385 for depth 400. There is no separate retreat state. **[A]** See [Tanker.Update](../Reference/incitti/tempest.bmx#L2556).

## 3. Cargo Representation and Release

Cargo is stored as integer counts on the Tanker, not as separate active Flipper instances before release. At the first Tanker tier, `cargo_flipper=2`; those passengers are not separately inserted into the baddie list or counted in `enemiesleft` while aboard. **[A]**

`ReleaseCargo(False)` chooses an initial direction of either -1 or +1. For each of two cargo Flippers it starts from the Tanker's lane and moves to an adjacent lane. On the continuous Level 1 circle, edge boundaries wrap. Each child is created at the Tanker's current height using the source's Flipper type value `1`; its direction is set opposite the selected side. Direction flips for the second child. Thus, on a continuous tube, the children are placed on opposite adjacent sides of the Tanker lane, at equal starting heights, and each starts in the source's flipping state rather than ordinary climbing state. Each `Flipper.Create` increments `enemiesleft`. **[A]** See [ReleaseCargo](../Reference/incitti/tempest.bmx#L2467) and [Flipper.Create](../Reference/incitti/tempest.bmx#L2918).

On an open tube, endpoint movement does not wrap: an out-of-range adjacent step leaves that child at the endpoint lane, while the other child can occupy the inward neighbor. The two cargo children can therefore include one child on the Tanker lane at an edge. **[A]**

On the source's initial Level 3 (board 2), 2 Flippers are the only possible cargo. The Tanker line-art draw has explicit center indicators for Fuseball and Pulsar cargo but no Flipper-cargo indicator. Consequently its two Flippers are represented in gameplay data but are not separately displayed inside the Tanker glyph. **[A]**

## 4. Player-Shot Collision and Scoring

A Tanker is hit when the shot's edge equals its edge and shot depth `z < tanker.height`. Unlike Flippers, there is no 16-unit height band. A hit consumes the shot, awards 200 points, creates an explosion, releases live cargo, and removes/decrements the Tanker. **[A]** See [Tanker.CheckColl](../Reference/incitti/tempest.bmx#L2594).

The cargo Flippers remain independently shootable and each may award the existing Flipper score (150 in this project/source behavior). Thus a player may score 200 for shooting the Tanker and later score for each released child; the Tanker hit does not destroy its cargo. **[A/B]** The source code explicitly performs these separate score paths; presentation and exact arcade scoring still require comparison **[C]**.

Shots are checked against the shared baddie list in insertion order, stopping at the first successful collision; this is not nearest-depth priority. On a Tanker shot, its newly released cargo is created during the Tanker collision handler and the Tanker is removed. How a mutable BlitzMax list iterator handles appended cargo during that same shot/update pass is not established by static source alone. **[A/C]**

## 5. Enemy and Wave Accounting

The Tanker itself increments active `enemiesleft` by one. Cargo is not counted until each Flipper is created. At cargo release, two children each increment `enemiesleft`, then the Tanker removal decrements it once: active count increases by one overall. This occurs on both natural release and ordinary player-shot destruction. **[A]**

On a player-shot Tanker kill, the Tanker has been destroyed and gets 200 points, but two live cargo Flippers become active. On natural rim release, the Tanker is removed without score and two live Flippers become active. The source tracks only current active `enemiesleft` and reserve `eggsleft`, not a fixed `EnemiesDestroyed == originalEggTotal` invariant. **[A]**

The current C# Level uses a fixed `EnemyTotal=10` and an accounting model where one Egg becomes one active enemy. It cannot represent two latent cargo Flippers becoming two new targets without extending its accounting. **[B]** For a development wave, a coherent conservation model should explicitly distinguish:

- initial Egg slots and reserve;
- active Tankers and Flippers;
- destroyed enemies;
- Tankers that naturally leave without a kill;
- dynamically materialized cargo slots.

At each Tanker release, add two child slots to the development-wave total. A shot Tanker resolves one carrier slot as destroyed; a naturally exiting Tanker resolves one carrier slot as escaped/removed without score. Then two children become active. This keeps accounting auditable for both paths and life reset; do not simply increment `EnemiesDestroyed` for natural exit or pretend the two children were already included in the original ten slots. **[B]**

The HUD's fixed remaining count and `IsComplete` equality to ten need a development-only dynamic total/resolved rule. Normal Level 1 accounting must remain unchanged. **[B]**

## 6. Player Death, Reset and Level Completion

The source death conversion removes every live non-spike baddie, including an unreleased Tanker or released Flipper cargo, and returns one Egg for each removed active baddie if `eggsleft + activeEggCount < maxeggs`. An unreleased Tanker therefore returns as one Egg; its still-attached cargo does not become two additional Eggs. Once cargo is released, each surviving child is independently eligible to return to the Egg supply. The source caps the restored pool to the level's original `maxeggs` and caps active Eggs at 16, so extreme populations may be discarded by source rules. **[A]** See [ConvertBaddieToEggs](../Reference/incitti/tempest.bmx#L2224).

In the controlled C# test wave, reset must not duplicate or lose its chosen accounting slots. A Tanker removed on natural release is not recreated on death; its released surviving cargo returns independently. A still-active Tanker returns as one Egg and retains its two-cargo behavior if rehatched. Document whether to emulate the source `maxeggs` cap or preserve the C# development invariant; do not silently conflate the two. **[B/C]**

A Tanker never directly collides with or kills the player: `CheckPlayerColl()` returns false. Released Flippers use the existing Flipper player-collision rules and may threaten the player. At source release height (>385), they enter the rim-flipping state, so they can approach the established rim collision much sooner than a newly hatched climbing Flipper. **[A]**

Incitti clears when Egg reserve/list is exhausted and either active enemy count is zero or `onrim` is true. `onrim` is based on all counted baddies being at height at least 400, not on all enemies being destroyed. Earliest Tanker level 3 takes the non-spike normal zoom route; source spike avoidance applies to displayed levels 4-7. The current C# validated exit instead requires all modeled enemy units resolved and clears projectiles on entry. **[A/B]** Preserve current C# exit semantics for a focused development wave unless deliberately changing and testing them; Tankers/cargo must be included among active enemies so they cannot be skipped by completion.

During source forward/reverse zoom, only spikes are updated, rendered and player-collision checked. Tankers and cargo Flippers are not processed as active baddies during these states. **[A]** C# should clear/freeze Tankers, cargo and bullets at its existing exit transition in line with current behavior, without modifying ordinary Level 1 exit timing.

## 7. Tanker Bullet Analysis

Tankers have 1-2 bullet attempts and start a timer at `16 + Rand(64)`. The Tanker first increases its height each update. Only below height 380 does it decrement the timer. When it drops below zero, it resets to 16, then fires if `Rand(0,100) > 75`; only successful creation consumes an attempt. A bullet starts on the Tanker's current lane and post-movement height. **[A]** See [Tanker.Create/Update](../Reference/incitti/tempest.bmx#L2556).

The existing C# EnemyBullet already models the projectile's lane/height, +3 update, strict expiry, player/shot hit rules and rendering. Tanker firing still needs its own ammo/timer and controlled random decisions, but can reuse the projectile class without changing its source-backed behavior. **[B]**

**Recommendation: defer Tanker-fired bullets from the first Tanker/cargo milestone.** Existing projectile reuse makes inclusion feasible, but Tanker-specific firing adds another random schedule and player threat while cargo lanes, initial Flipper pivot state, dynamic wave accounting, and death conversion are being validated. This is a scope/isolation choice, not a claim that source Tankers do not fire. Keep the existing Flipper `--enemy-bullets-test` feature separately available; Tanker-test composition is specified below. **[B]**

## 8. Super Zapper Boundary

Ordinary player-shot Tanker destruction and natural release can be implemented without Super Zapper. Do not add a fake `ZapIt`, zap timer, input, or placeholder zap behavior.

Future Super Zapper integration must distinguish:

- ordinary Tanker shot: immediate 200 points, release live cargo, remove Tanker;
- natural release: no Tanker score, release live cargo, remove Tanker;
- Tanker zap: delayed removal; at zap expiry award 200 points, release cargo with `stillborn=true`, and remove Tanker.

Source stillborn cargo gets a Flipper death timer of 8 updates. Implementing this later will need a supported initial death state/timer on Flipper and tests for delayed child removal/scoring, rather than abusing the ordinary live-cargo constructor. **[A]**

Tests for this milestone should verify that only ordinary shot and natural release paths exist and that no zap behavior is reachable. Future Super Zapper work can then add a separate release mode without redesigning normal cargo release. **[B]**

## 9. Rendering Behavior

Tankers render in `COL_TANKERS` (magenta in the source's first palette) as a fixed line-art center glyph on the tube lane. The center is projected at `position + depth - height`; scale is `TFormSZ(26,z) + 1`. The Tanker itself has no per-frame rotation field or rotation update. **[A]** The glyph includes an outer diamond/cross-like outline and internal lines. It does not display a Flipper-cargo icon; center cargo symbols exist only for Fuseball/Pulsar payload branches. **[A]**

Released cargo uses the Flipper rendering, with the source Flipper's pivot interpolation. A child begins at the Tanker's current height, so should be rendered at that depth, not reset to zero. **[A]**

The C# renderer can expose a callback/helper for finite projected Tanker line geometry in headless tests, then draw it in the active scene. Preserve existing Flipper, bullet, Spiker and spike drawing. **[B]** Whether the reference's first-palette magenta, exact glyph proportions and same-frame cargo visibility resemble the original arcade is **[C]**.

## 10. Current C# Integration Points

- `Gameplay/Tanker.cs`: empty scaffold. Add lane, height 10, fixed two-Flipper cargo, climb/update, release threshold, player-shot hit predicate, and a release result distinguishing natural exit from player-shot destruction. No direct player collision.
- `Gameplay/Flipper.cs`: add a narrow tanker-cargo initializer that starts a Flipper on the calculated adjacent lane at Tanker height in the source's initial flipping state/direction. Keep the normal constructor and all current Flipper behavior unchanged. The source cargo `typ=1` is not equivalent to current C#'s ordinary `Climbing` constructor.
- `TempestGame.cs`: add an active Tanker list, hatch selection only in the Tanker test wave, update/shot/render/collision routing, cargo insertion, death reset and completion accounting. Existing `EnemyBullet` can be reused, but Tanker bullets remain disabled in this milestone.
- `Level.cs`: add development-only dynamic cargo/release accounting and appropriate Egg restoration. Do not change fixed normal Level 1 enemy total or invariants.
- `Debug/DebugController.cs` and `Program.cs`: add `--tanker-test`, retaining `--lives`. Explicitly disallow the Spiker scenario combination. Keep the existing EnemyBullet flag a separate optional Flipper-fire modifier only if intentionally combined; recommended baseline Tanker scenario keeps it off.
- `VectorRenderer.cs`: draw Tanker line geometry and leave cargo Flippers to existing Flipper rendering.
- `SimulationTests.cs`: deterministic Tanker and cargo cases plus all current regressions.
- README/development notes and the requested analysis/report logs only when implementation is later approved. No new code file is needed; no Enemy hierarchy rewrite is justified.

## 11. Recommended Tanker Milestone Scope

Implement one opt-in `--tanker-test` development scenario on the existing continuous circle:

- exactly one deterministically selected Tanker, carrying exactly two Flippers;
- the remaining base Egg supply hatches as ordinary Flippers;
- deterministic Tanker lane and deterministic cargo-side choice in tests and manual scenario;
- Tanker natural release and ordinary player-shot destruction both create the two source-state Flippers at the Tanker's current height;
- ordinary shot kill awards 200; natural release awards no Tanker points;
- no Tanker direct player collision;
- Tanker-origin bullets deferred; Super Zapper/zap/stillborn paths deferred;
- dynamic development accounting as described in section 5, while default Level 1 and existing `--spiker-test` behavior remain untouched.

A one-Tanker wave keeps carrier release visible and repeatable without pretending a Tanker is part of Level 1 or implementing general enemy selection/progression. **[B]**

## 12. Proposed Development Scenario

Example:

```powershell
dotnet run --project src/Tempest.Game -- --tanker-test --lives 10
```

`--lives` is orthogonal and supported. Treat `--tanker-test` and `--spiker-test` as mutually exclusive scenario selectors; reject the combination clearly or document deterministic precedence. Keep `--enemy-bullets-test` disabled in the default Tanker scenario so Flipper bullets do not confound cargo timing. If explicitly composed later, it should still only enable Flipper-fired bullets, never Tanker-fired bullets. No Tanker source bullets should fire until a separately approved follow-up.

The test policy should guarantee one Tanker on a fixed eligible Egg, two cargo Flippers and Flipper-only remaining Eggs. Use injected Random/decision values in tests; the manual scenario should either be deterministic or clearly seeded/repeatable so the user can observe both natural release and player-shot release on separate runs. Avoid a general scenario or level-selection framework.

## 13. Proposed Automated Tests

- Tanker test flag parsing/composition with `--lives`; reject/define conflict with `--spiker-test`; normal Level 1 and Spiker-only remain unchanged.
- Tanker hatches on chosen Egg lane, starts at height 10, has exactly two Flipper cargo, and receives expected initial bullet fields only if future bullet scope is enabled.
- Controlled update cadence adds the configured climb speed; lane stays fixed; no pause/reversal; strict natural release boundary (`height > 385`, not equality).
- Natural release emits exactly two children at current height, with source initial flipping state, opposing directions, and correct adjacent lanes. Test wrap on continuous geometry and clamping behavior for a synthetic open tube if supported.
- Tanker player-shot collision requires same lane and strict shot height below Tanker height, with no 16-unit band; hit consumes shot, scores 200 once, removes Tanker and releases live cargo.
- Natural release scores no Tanker points. Child Flippers can then be killed for existing Flipper points; verify total score can include both Tanker and child awards after the shot-release path.
- Verify no direct Tanker/player collision; cargo Flippers retain established collision rules.
- Test dynamic slot accounting for natural escape vs shot destruction and child release; assert no premature exit while Tanker/cargo remain and completion once all modeled slots resolve.
- Player death before cargo release returns one Tanker Egg, not two cargo Eggs. Death after release returns each surviving Flipper independently, preserves destroyed enemies and score, and does not duplicate supply.
- Tanker shot collision order versus Flippers/bullets uses documented insertion order; a colliding shot is consumed once and does not also hit cargo.
- Residual bullets do not gate completion and are cleared at exit. Spike-free normal exit timing remains byte-for-byte behaviorally unchanged.
- Tanker and cargo projection/render callback coordinates are finite and use expected depth/color/shape scale without opening Raylib.
- Existing Flipper pivot, Spiker/Spike, EnemyBullet, `--lives`, death/reset, Spiker scenario and exit tests remain green.

## 14. Manual Arcade-Comparison Checklist

- Compare Tanker color, line-art geometry, projected scale, apparent orientation and whether/how cargo is visibly signaled.
- Check fixed-lane climb speed and exact release position near the rim.
- Observe two cargo Flippers' initial adjacent lanes, pivot appearance, height and timing; verify edge behavior on an open-tube test if later supported.
- Compare natural release (no carrier points) against shooting the carrier (200 points) and then shooting released Flippers.
- Verify Tanker does not kill directly but released cargo can soon threaten the player under the established Flipper collision rule.
- Recheck death/reset while a Tanker still carries cargo versus after cargo release.
- Confirm active/remaining HUD and exit trigger account for Tanker, cargo and natural carrier exit correctly.
- Recheck the established spinner, player shot, EnemyBullet, Spiker/Spike, lives, and spike-free exit behavior.
- If Tanker bullets are later enabled, separately compare their 1-2 ammunition, delayed timer, firing threshold, launch lane/height, player threat and shootability.

## 15. Remaining Uncertainties

- **C:** Exact Tanker unlock level and cargo rules in Incitti do not prove original arcade progression. Compare against original Tempest.
- **C:** The source's mutable-list iteration may or may not update newly appended cargo in the same frame; choose/test deterministic C# ordering and compare release timing manually.
- **C:** BlitzMax numeric semantics for `1.75 + lv/20`, the `Rand` interval conversions, and same-frame update order should be verified before claiming bit/frame-exact reproduction.
- **C:** Source `onrim` allows clear with enemies at/above the rim, while C# currently requires full enemy resolution. Preserve C# unless arcade observation specifically requires adjusting it.
- **C:** Tanker and cargo presentation, speed, collision feel, points and survival danger need direct arcade comparison.
- **B:** Current fixed `EnemyTotal=10`, `EnemiesDestroyed`, and `ReserveEggs` invariant cannot represent cargo multiplicity without a development-only dynamic slot/resolution model. That design is required before implementation, not a reason to refactor all normal Level 1 accounting.

## 16. Explicitly Deferred Features

- Tanker-fired bullets in the first Tanker milestone (reuse existing EnemyBullet later; source Tanker firing remains acknowledged).
- Super Zapper input/charges, Tanker `ZapIt`, delayed zap score, stillborn cargo, and Flipper delayed-death support.
- Fuseball or Pulsar cargo and all other enemy types.
- General randomized Tanker selection, Tanker caps/progression, multiple level sets, and full arcade wave rules.
- Audio, explosion/fuse score popups, bonuses, later levels and changes to existing level-clear behavior.
- Normal Level 1, Spiker scenario, Flipper, player controls and ordinary exit changes.
