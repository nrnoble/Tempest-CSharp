# Level 2 Concurrency Investigation

Date: 2026-11-09
Disposition: **READ-ONLY INVESTIGATION COMPLETE - LEVEL 2 FOUR-FLIPPER DIAGNOSTIC RECOMMENDED; NOT IMPLEMENTED**

## 1. Starting Git state and scope

Before repository inspection, the required prompt token was found exactly once
on its own line. The prompt was archived byte-for-byte at
[`2026-11-09-level2-concurrency-investigation-prompt.md`](../Prompts/2026-11-09-level2-concurrency-investigation-prompt.md);
its SHA-256 is `5D0D4A93DC86569C63E0613AB71134DEE6F3F0C01AB0CFC4A2D2FAE0560227BB`.

Initial repository state:

- Branch: `master`, tracking `origin/master`.
- `HEAD`: `f362af7f87c23a9b983937f8733f5a4ac71d0162`.
- Local `origin/master` and live remote `master`: the same hash.
- Index and tracked worktree: clean.
- Sole untracked path: the pre-existing protected `Reference/arcade/roms.rev2/`
  directory. It was read only and left untouched.

The investigation made no production, test, or durable-documentation changes;
nothing was staged, committed, or pushed. A temporary bounded probe under
`TEMP/` was removed.

## 2. Baseline and evidence reused

The accepted baseline is the requested Level 1 correction at
`f362af7f87c23a9b983937f8733f5a4ac71d0162`. The accepted Level 1 policy remains
frozen: ten original Flippers, at most four active, ready Eggs wait without
being consumed or rerolled, and the original release schedule, firing, rim
calibration, and other gameplay remain unchanged. Neal accepted production
Level 1 manually and played the Level 1-8 campaign without a blocking
regression. This is evidence for the method, not manual acceptance of a Level
2 change.

Reused the comparative difficulty, Level 1 cap diagnostic/correction/closeout,
Level 2 square/canflip, and Level 1-to-2 progression reports. The primary ROM
authority was the local [Tempest ROM listing](../../Reference/arcade/Tempest%20Rom%20Source%20code.md)
and local Rev. 2 chip files only; no internet disassembly was consulted.
Incitti was not used to override ROM evidence.

## 3. Current Level 2 identity and C# configuration

**CURRENT ACCEPTED C# BEHAVIOR:** production Level 2 is progression index 1,
displayed Level 2, board 1, palette 0: a closed 16-lane square. Its normal
composition is Flippers only. The current [Level 2 configuration](../../src/Tempest.Game/Levels/LevelConfiguration.cs)
has:

| Property | Current C# Level 2 |
|---|---:|
| Original supply | 12 Flippers |
| Initial Eggs / reserve | 4 / 8 |
| Reserve release interval | 50 active ticks |
| Maximum Egg queue | 4 |
| Maximum Eggs approaching | 2 |
| Approach selection interval | 50 active ticks |
| Egg update cadence | 8 updates |
| Spawn lane sequence | 12, 4, 9, 1, 14, 6, 10, 2, 15, 7, 3, 11 |
| Below-rim canflip | Enabled |
| Flipper tuning | climb 2, turn 8 degrees, lane pause 21, rim pause 24 |
| Pivot geometry | Endpoint hinge |

These are C# configuration and test facts, not a new claim of Level 2 manual
acceptance. The currently used 2.5 rim-speed calibration is unchanged.

The Level 1 production limit is explicitly Level-1-specific in
[TempestGame](../../src/Tempest.Game/TempestGame.cs): the private maximum is
four and its guard requires natural production progression with the canonical
`LevelConfiguration.Level1` object. `LevelConfiguration` has no generic active
Flipper maximum. Level 2's `MaximumActiveEggs = 4` limits Egg queueing, not
active Flippers. Level 2 has neither a species cap nor a global active-enemy
cap in C#.

## 4. Level 2 ROM table values

**ROM-LISTING-BACKED / REV-2-BINARY-CONFIRMED:** the Level 2 table row is
selected by the ROM level-parameter loader at `$92d6`, which reads the
parameter/table pointer pairs beginning at `$9607`. The local raw chip
sequences match the listing in `136002-113.d1`: Flipper min/max at offset
`0x4cd`, global maximum at `0x598`, wave supply at `0x5b3`, and the firing and
movement rows at `0x3fa`, `0x407`, `0x5e3`, and `0x41b`.

| Parameter | Medium/default Level 2 | Easy | Hard | Runtime meaning |
|---|---:|---:|---:|---|
| `wave_enemies` | 12 | 12 | 14 | Original pending supply |
| `min_flippers` | 1 | 1 | 1 | Minimum-selection value for Flippers |
| `max_flippers` | 4 | 4 | 4 | Maximum active Flipper species count |
| `max_enm` | 6 | 6 | 6 | Global active-enemy gate value |
| `enm_shotmax` | 1 (two slots) | 0 (one slot) | 2 (three slots) | Maximum shot-slot index, not a direct slot count |
| `shot_holdoff` | `0x50` / 80 ROM timer units | 80 | 80 | Per-Flipper successful-shot reload delay |
| `spd_flipper_lsb` | `0xd4` base table byte | adjusted slower | adjusted faster | ROM fixed-point movement input; not C# height units |
| `flip_top_accel` | 2 | 1 | 2 | Top-rim acceleration parameter |

Difficulty bits `00` and `11` mean Medium, `01` Easy, and `10` Hard. Hard
adds approximately one eighth to the base supply (12 becomes 14), adjusts
Flipper speed, and increases the shot-slot index subject to its cap. Easy
reduces Flipper speed, the shot-slot index, and (through Level 16) top-rim
acceleration. Neither difficulty changes the four-Flipper maximum or the
global table value. Neal's cabinet is believed to be Medium, but that physical
setting remains unverified.

The project C# configuration is fixed rather than difficulty-selectable. Its
Level 2 shot capacity is two and its shot holdoff is 77 C# ticks; the ROM
Medium table uses two shot slots and an 80-unit holdoff. The shot-slot capacity
agrees; the raw ROM timer unit is not a proven 60 Hz conversion. Do not tune
either as part of a concurrency experiment.

## 5. ROM creation eligibility and active-count semantics

The ROM calls `create_enemies` during the active game pass. At entry it sums
`enemies_in + enemies_top`; it then blocks creation only when that sum compares
**greater than** `max_enm`. The `BCC` and `BEQ` branches both continue, so a
current total of exactly six still passes this first gate. The type-availability
calculation also uses `max_enm + 1`. Consequently, the table value six is not
a strict six-object ceiling: the literal control flow can admit a seventh
total active enemy where a species limit permits it, and blocks further
creation on the following pass. Earlier reports' shorthand “six total active”
does not capture this inclusive comparison. This distinction does not affect
Level 2.

After the global gate, the routine also blocks while `zap_running` is set and
walks the pending-entry table. When a pending entry becomes eligible, the
enemy-selection routine computes each species' remaining availability as
`max_flippers - n_flippers`, then applies the minimum/selection rules. Level 2
has only Flippers; when `n_flippers` reaches four, availability is zero, so a
fifth Flipper cannot be selected. The species check is independent of the
global gate and is the stricter Level 2 limit.

The meaning of active is established by the counters and their update sites:

- A unit starts counting when creation installs an enemy in an active slot,
  increments `enemies_in`, and increments its species counter (`n_flippers`
  for a Flipper).
- ROM pending entries are held separately in pending tables; C# Eggs are
  analogous pending units and do not increment either active counter.
  Projectiles use separate shot counters and are not active enemies.
- Below-rim enemies count. On rim arrival the ROM transfers the global count
  from `enemies_in` to `enemies_top`; it does not decrement `n_flippers`.
  Rim Flippers therefore still consume a Flipper slot.
- A Flipper stops counting when its active enemy slot is cleared and the
  removal path decrements `n_flippers`. A Super Zapper target remains counted
  until the zap path reaches that removal; `zap_running` also prevents new
  creation during its active sequence.

Thus Level 2's four-Flipper cap always prevents the total from reaching the
global gate. The global gate has no independent observable effect on
Flipper-only Level 2, whether described by its table value or by the
inclusive runtime comparison.

## 6. Current C# maximum and bounded measurement

The C# [hatch path](../../src/Tempest.Game/TempestGame.cs) applies the cap
before invoking the hatch factory only when the production Level 1 predicate
is true. Level 2 bypasses that condition; its `while` loop hatches every ready
Egg. Since all 12 originals can be released and hatched, the theoretical
no-kill maximum is **12 active Flippers**. The existing bounded
`ProductionLevelTwoRemainsUncappedAndLevelsThreeThroughEightKeepTheirConfigurations`
test also demonstrates that production Level 2 exceeds four.

A temporary deterministic schedule probe used the existing `Level` release,
approach, and hatch APIs, with a 2,000-tick ceiling and a clean failure if all
12 were not reached. It advanced Egg delivery but intentionally did not update
enemy movement, collisions, firing, or Super Zapper. The ordinary Level 2
delivery path has no random selection, so a random seed is unnecessary.

| First active count | Boundary | Reserve | Eggs | Egg states | Unresolved |
|---:|---:|---:|---:|---|---:|
| 1 | 65 | 8 | 3 | 3 roaming | 12 |
| 2 | 115 | 7 | 3 | 3 roaming | 12 |
| 3 | 189 | 6 | 3 | 3 roaming | 12 |
| 4 | 263 | 5 | 3 | 2 roaming, 1 approaching | 12 |
| 5 | 289 | 5 | 2 | 2 roaming | 12 |
| 12 / peak | 615 | 0 | 0 | none | 12 |

These are **BOUNDED HEADLESS MEASUREMENTS** of the accepted C# delivery
schedule, not a full combat run or Atari timing. The probe did not simulate
rim arrivals or bullets, so those counts are unavailable rather than
meaningful zeroes. A follow-up attempt to execute a temporary capped-queue
probe was blocked by Windows Application Control; no bypass was attempted.

## 7. Egg waiting and whether it isolates the cap

The current Level update keeps releasing reserve Eggs when the Egg collection
has room and continues Egg approach/update regardless of active Flipper count.
The two-Egg approach limit includes ready Eggs, so at most two Eggs can be
approaching/ready at once; a four-Egg queue can also contain roaming Eggs.
Therefore multiple Eggs can become ready while four Flippers occupy all
active slots, but a large unbounded ready burst cannot accumulate.

The existing Level 1 approach is reusable: check the authoritative
`flippers.Count` before calling the hatch factory; leave blocked Eggs in
`ReadyToHatch`; do not consume them, return them to reserve, resolve supply,
or draw randomness while waiting. With two ready Eggs and two newly free
slots, the ordinary hatch loop can fill both in the same hatch phase. A
player-shot removal happens before that phase and can allow same-tick hatch;
delayed Super Zapper removal happens later in the tick, so a waiting Egg can
hatch on the next active tick. This can refill the field quickly, but only
back to four active Flippers; release and approach timing remain unchanged.

This is sufficient to isolate active density without retiming Eggs. It does
not make the Level 2 result feel identical to Level 1: four canflip Flippers
can change lanes below the rim, potentially clustering, pivoting, crossing
shot lanes, and approaching the rim in varied directions. The cap places no
separate limit on simultaneous pivots and changes the number and order of
Flipper movement/firing random calls over time. Waiting itself should consume
no random draw; a later-hatched Flipper still uses the normal canflip
direction-selection path. Preserve all lane, pivot, collision, and random
rules for the experiment.

## 8. Firing and rim pressure are separate variables

Production Level 2 uses the existing normal Flipper firing path. C# configures
two concurrent enemy bullets and a 77-tick holdoff. The firing loop uses the
shared active-shot pool and pressure thresholds `00, E0, F0, FA, FF` indexed
by the current bullet count (a C# random byte at or above the threshold
passes). The two-shot pool does not change when Flipper count changes; a
capacity-blocked shooter remains ready because its holdoff reloads only when
a bullet is created. Below-rim Climbing/Pausing Flippers can reach firing
checks below the C# height cutoff; an active `Flipping` pivot does not fire,
although its shot-delay timer continues advancing and a ready shot waits for
pivot completion.

Enemy bullets remain visible in production Level 2 and are not counted as
enemies. They advance three C# units per tick and expire beyond rim depth.
Player shots remove a bullet when they share its lane and the shot is below
it; a bullet hits the player in the same lane within the existing strict
eight-unit rim gap. These C# projectile speed/lifetime and timer-unit mappings
remain project/reference comparisons, not new ROM-equivalence claims.

A four-Flipper maximum naturally reduces the maximum number of simultaneous
eligible firing sources, but does not reduce the two-shot C# capacity or
change cooldown, visibility, bullet geometry, or collision. It may also reduce
the number of simultaneous climbers and rim pursuers, and increase the time
between arrivals. No exact count or timing for those effects follows from the
queue schedule alone. Keep the accepted 2.5 rim-speed calibration separate.

## 9. Death, Super Zapper, completion, and progression

On ordinary C# death/re-entry, the same Level/configuration is retained,
runtime Flippers and bullets are cleared, and only unresolved supply is
reconstructed as fresh Eggs. The current Level 2 canflip setting and
configuration remain in force; resolved kill/rim outcomes remain resolved;
the shared random object is not reset. No Level 2-specific re-entry obstacle
to a future hatch cap was found. Existing
`LifeReconstructionRetainsLevel2RuntimeAndPersistentState` coverage establishes
the Level 2 lifecycle baseline.

The C# Super Zapper assigns pending delays to active Flippers. They remain in
the authoritative active list and consume a future cap slot until
`AdvanceZapTimer` removes them. Waiting Eggs are not Zapper targets. The
existing C# hatch phase continues during the Zapper sequence; this is a
project behavior to preserve, not a claim that the ROM uses the same policy.
The ROM itself blocks creation while `zap_running` is active.

Normal completion still requires every original unit to be destroyed or
rim-cleared, reserve and Egg collections empty, and active Flippers removed.
A waiting Egg therefore prevents false completion. Natural production Level
2 can progress to Level 3 after its normal successful exit; direct
`--level2-test` remains terminal and restarts the same Level 2 configuration.
A Level 2 experiment must preserve both behaviors and leave Level 1 and
Levels 3-8 unchanged.

## 10. Test/helper hang-risk audit

The Level 1 closeout documents the previous failure mode:
`SimulationTests.HurryUpIsNotEnabledForNormalSpikerOrTankerScenarios` had a
production-Level-1 case that waited indefinitely for every Egg to hatch
without resolving the four active Flippers. It was corrected to assert the
cap/wait boundary with a bound; it was a stale helper assumption, not a
production deadlock.

For a future Level 2 cap:

- Change the bounded
  `Level1ProductionFlipperCapTests.ProductionLevelTwoRemainsUncappedAndLevelsThreeThroughEightKeepTheirConfigurations`
  assertion from “eventually exceeds four” to a diagnostic/production-scoped
  expectation. Its 1,000-tick maximum is already explicit.
- Reuse the bounded Level 1 cap-test pattern (3,000-tick limits and
  conservation assertions) for Level 2 waiting, re-entry, Zapper, and
  completion tests. Add a useful state snapshot to every timeout.
- `CampaignSoakTests` has bounded campaign orchestration, but resolves
  counter-based waves by accounting rather than natural hatching. It is not a
  density test; retain a separate bounded Level 2 hatch/combat regression.
- `SimulationTests.HatchHurryUpWave` contains reserve/Egg `while` loops, but
  its Fuseball/Pulsar development waves are not capped Flipper waves.
  `PulsarCompletionWaitsForFinalActiveUnit` and the Egg sequence/accounting
  helpers directly use `Level.HatchReadyEnemy` / `Level.HatchReadyEgg`, bypassing
  `TempestGame.HatchEggs`. They do not currently hang from a Level-2-only cap;
  do not reuse them as capped-production evidence. If the gate were moved into
  shared Level-level APIs, those loops would need explicit bounds and
  cap-aware semantics.
- The Level 2 canflip tests' state-transition loops operate on one Flipper,
  not wave supply or waiting Eggs. The Level 2 rendering/Zapper test readies
  one Egg directly. Neither is an active-cap integration test.
- Recording life-loss, campaign transition, and Level 1 cap gameplay loops
  already have explicit tick bounds.

No other current unbounded production-Level-2 wait-for-Eggs helper was found.
Every new full-game loop for the cap experiment should have a hard tick bound;
do not use “wait until all Eggs hatch” while active slots are intentionally
held.

## 11. Recording/playback

`GameplayStartupConfiguration` records starting displayed level, lives, rim
multiplier, and random seed; version-2 recordings identify a production
scenario, not a per-level cap policy. Promoting a Level 2 cap into ordinary
production would require no format or metadata change: new recordings use the
new production simulation and existing recordings replay under the current
production rules.

A separate development-only diagnostic should follow the established
Level 1 diagnostic boundary: keep it terminal and reject recording/playback
that cannot preserve its mode, rather than silently recording it as ordinary
production. That also requires no new recording format.

## 12. Architecture and recommendation

The existing reusable Level 1 infrastructure is the hatch-phase check against
the authoritative active Flipper list and the waiting-Egg semantics. The
current limit itself is intentionally not reusable production configuration:
it is a Level-1-specific constant and predicate in `TempestGame`.

If approved for comparison, add one development-only terminal option such as
`--level2-four-flipper-cap-test`, using the existing
`LevelConfiguration.Level2Square` object. Scope the four-Flipper check only to
that option and the existing hatch phase. Do not add a generic production cap
property, six-active gate, new counter, or generalized queue before manual
acceptance. Only hatch eligibility should differ from current production
Level 2.

Proposed automated coverage:

- Baseline production Level 2 remains unchanged until promotion; diagnostic
  uses the same accepted square/canflip configuration.
- Keep supply 12, four starting Eggs, eight reserve, and all 12 obligations.
- Never exceed four active diagnostic Flippers; the fifth ready Egg waits
  without being discarded, rerolled, scored, or consuming random state.
- A freed slot hatches the oldest ready Egg; check same-tick shot removal and
  next-tick delayed-Zapper removal.
- Verify conservation, canflip direction/pivot behavior, firing cadence,
  two-shot capacity, bullet rendering/collision/lifetime, and rim-speed
  calibration remain unchanged.
- Verify death/re-entry, Super Zapper, wave completion, terminal diagnostic
  replay/restart behavior, natural production Level 2-to-3 progression, and
  recording/playback policy.
- Verify production Level 1 remains identically accepted and Levels 3-8
  remain unchanged. Bound every simulation loop and report timeout state.

Proposed manual A/B:

```powershell
dotnet run --project src\Tempest.Game -- --level 2 --lives 10 --spinner-sensitivity 1
dotnet run --project src\Tempest.Game -- --level2-four-flipper-cap-test --lives 10 --spinner-sensitivity 1
```

Compare simultaneous visible Flippers, below-rim canflip density and pivot
overlap, grouped rim arrivals/pursuers, bullets, multi-direction convergence,
recovery/breathing room, deliberate target tracking, whether the wave becomes
too sparse or stationary defense too easy, queued-Egg refill, all 12
obligations, death/re-entry, Zapper follow-up pressure, completion, and arcade
resemblance. Do not use exact death count as the primary fidelity metric.
Manual Level 2 A/B acceptance is required before promotion.

## 13. Conclusions and remaining uncertainty

- ROM Level 2 has a four-Flipper maximum; the max table is unchanged across
  Easy, Medium, and Hard.
- The global `max_enm` value is six, but its inclusive comparison is looser
  than a strict six-object cap. It cannot independently constrain Flipper-only
  Level 2 because the species cap is four.
- Current C# production Level 2 has no active cap; the bounded no-kill
  delivery schedule reaches five at boundary 289 and all 12 at boundary 615.
- Level 2 canflip makes the manual comparison materially different from Level
  1, but does not prevent a clean hatch-eligibility-only experiment.
- Existing Level 1 waiting, active-list, re-entry, and Zapper mechanisms are
  structurally reusable without changing their accepted production behavior.
- No Level 2-specific death/re-entry or Zapper lifecycle blocker and no
  recording-format change are indicated.
- Recommend a separate Level 2 four-Flipper-cap diagnostic; do not promote
  directly or modify any gameplay now.

Remaining uncertainties are Neal's actual cabinet difficulty setting,
Level 2 arcade feel under a four-Flipper cap, and direct equivalence between
ROM fixed-point/timer units and C# 60 Hz values. The capped-wait extension was
not dynamically measured because Application Control blocked its temporary
executable; its queue bound and release order above are source/code-traced,
not measurement claims.

## 14. Validation and final Git state

- Targeted `Level2SquareTests`, `Level2CanFlipTests`, and the existing
  uncapped-Level-2 test: **50 passed, 0 failed**.
- Bounded no-kill Level 2 Egg delivery probe: **passed**, all 12 active by
  tick 615 within the 2,000-tick bound.
- Capped-queue extension: **not run** after Application Control blocked the
  temporary executable; no attempt was made to bypass the policy.
- No full suite, interactive game, build-only ritual, or internet research was
  performed.
- Final `HEAD`, `origin/master`, and live `master` remain
  `f362af7f87c23a9b983937f8733f5a4ac71d0162`; branch remains `master`.
- Index and tracked worktree remain clean. The only untracked path remains the
  pre-existing protected `Reference/arcade/roms.rev2/` directory. No source,
  tests, durable docs, ROM data, user data, screenshots, recordings, videos,
  or unrelated TEMP files were changed.
