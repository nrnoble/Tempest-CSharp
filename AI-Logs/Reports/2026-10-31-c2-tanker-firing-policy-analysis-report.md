# C2: isolated Tanker firing validation and policy analysis

Checkpoint: `27c43b7 Add validated controller restart and quit aliases`.
Disposition: analysis only. No gameplay code, tests, reference files, authoritative
behavior documentation, project configuration, or Git history changed.

## 1. Recommendation and disposition

**Choose B: correct the isolated Tanker's climb speed to literal Incitti
displayed-Level-3 behavior, retaining its existing firing algorithm.**

The ammunition, initial delay, countdown, chance boundary, reset, post-climb
bullet origin, and firing/release height ordering accurately implement Incitti
in eligible active-play updates. However, the current `1.85f` development climb
speed does **not** implement the literal `1.75 + lv/20` at `lv=2`: the division
of the two integer operands is integer division, producing **1.75**, not 1.85.
The floating destination does not retroactively change the nested division.

This unsupported approximation shortens the Tanker's firing window. Correcting
it is a reference-fidelity correction, **not arcade-measured tuning**. There is
no demonstrated conflict between the reference's randomized firing policy and
Neal's observation that a Tanker can fire two individual shots before release.
Neither extra ammunition nor guaranteed two-shot firing is justified.

**GO for a later, narrowly scoped C2 correction and characterization prompt.**
Keep firing development-only until the automated and qualitative manual gates
below pass. **NO-GO for production promotion now, C3, mixed Level 3 composition,
or natural Level 2 -> Level 3 progression.** No implementation was performed.

## 2. Baseline and authority

The requested opening commands ran in the repository:

```text
git status --short
(empty)

git log -3 --oneline
27c43b7 Add validated controller restart and quit aliases
a922172 Ignore local video files
11773c0 Implement Level 3 geometry and directed rotation

git diff --check
(empty)

git status --short -- Reference/incitti
(empty)
```

The worktree was clean at the requested checkpoint. C1/C1.5 and controller
manual acceptance remain intact.

Evidence hierarchy follows the [behavior specification](../../TEMPEST-BEHAVIOR-SPEC.md#L3):

1. Direct Atari observations / Neal's arcade observations.
2. Literal Incitti engineering/reference source.
3. Current C# behavior and its existing tests.
4. Explicit new assumptions only where needed.

**ARCADE VERIFIED, qualitatively:** Atari Tankers fire individual shots at
intervals, and at least two can occur before a Tanker reaches its release/rim
area. This does not establish every Tanker's shot count or hidden ammunition.
The existing [deferred observation](../../TEMPEST-DEVELOPMENT-NOTES.md#L1096)
also records one C# shot with insufficient observation of the remaining window.

**New qualitative development observation, supplied in this prompt:** Neal
recently replayed Incitti's recreation after several years away. Based on his
extensive original Atari Tempest experience, he considers its overall gameplay
timing extremely close to the arcade; he expects to notice small deviations
and noticed no meaningful timing differences during this replay.

This is useful comparative evidence, not a controlled timing measurement,
not a Tanker-specific measurement, and **does not promote Incitti constants to
ARCADE VERIFIED**. It increases the practical importance of checking literal
reference semantics instead of carrying forward an unexplained approximation.

## 3. Exact current C# algorithm and integration

### Initialization and active update

See [Tanker](../../src/Tempest.Game/Gameplay/Tanker.cs#L42) and the
[Tanker-wave factory](../../src/Tempest.Game/TempestGame.cs#L498).

- Constructor: lane and original-slot identity supplied by the caller; height
  `10`; `Climbing`; default climb speed `1.85f`; two latent Flipper cargo units.
  Default ammo is zero, clamped nonnegative, and default timer is `16`.
- In `--tanker-test --enemy-bullets-test`, the factory draws fresh ammo through
  `NextRandomInclusive(1,2)`, and timer `16 + NextRandomInclusive(1,64)`.
  Thus actual armed initial timer is **17..80**, not 16..80 or 16..79.
  [Inclusive wrapper](../../src/Tempest.Game/TempestGame.cs#L907) calls
  `random.Next(minimum, maximum + 1)`; normal randomness is
  [Random.Shared](../../src/Tempest.Game/TempestGame.cs#L141), unless injected.
- Eggs hatch before Tanker updates in the same game step, so the hatch step
  already adds the first climb increment and decrements an eligible timer.
  The timer observed immediately after an armed hatch is initially **16..79**.
- `CargoReleased` returns `Active` with no motion/fire. `CargoReleasePending`
  returns `NaturalCargoRelease` with no motion/fire.
- Ordinary `Climbing` update first adds `ClimbSpeed` to height.
- Only post-climb `Height < 380` calls the firing helper. Exactly 380 is silent.
  The helper does nothing unless ammo is positive and the random delegate exists.
- Eligible helper: decrement timer by one. If it is still `>=0`, return no bullet.
  On `<0`, reset timer to **16 before rolling** the inclusive integer `0..100`.
- Roll `<=75`: no shot, no ammo loss. Roll `76..100`: consume one ammo unit and
  create one `EnemyBullet(EdgeIndex, Height)`. At most one bullet per update.
- A failed roll still resets the timer; consecutive decision windows are
  **17 eligible updates apart**, not 16. A successful shot has the same reset.
  Zero ammo freezes the countdown and stops all further decision rolls.
- After possible firing, test `Height > 385` (`Level.Depth - 15`); mark pending
  release and return `NaturalCargoRelease`. Height exactly 385 does not release.
  The game consumes the release result and removes the carrier that step.
- Motion never pauses because of firing. A release-triggering update cannot
  attempt firing: its post-climb height is already above 385, hence not below 380.
  The last below-380 updates may fire, but the intervening near-release updates
  are silent; no rim-firing state exists for the Tanker.

### Bullet ownership, direction, removal, death and Zapper

The [game step](../../src/Tempest.Game/TempestGame.cs#L210) advances existing
bullets before hatching/updating enemies. The [Tanker update integration](../../src/Tempest.Game/TempestGame.cs#L760)
registers a returned bullet in the independent bullet collection and ordered
development collision list. It has no carrier pointer or owner lifetime.
It starts on the carrier's lane at the post-climb height; height increases
toward the rim, not inward and not across lanes.

[EnemyBullet](../../src/Tempest.Game/Gameplay/EnemyBullet.cs#L8):

- First motion is **the next game step**, then `+3` height each active step.
- Expires only at `Height > 400`; exactly 400 is not expired.
- Player shot on the same lane with shot height strictly below bullet height
  removes it for 25 points, without resolving an enemy ledger unit.
- Same player lane and `400 - Height < 8` kills the player. Exactly an 8-unit
  gap is safe. Expired bullets are removed before player collision.
- Player-shot carrier destruction, natural cargo release, and carrier zap
  expiry do **not** remove existing bullets.
- The Tanker itself never directly collides with the player.

[Zap integration](../../src/Tempest.Game/TempestGame.cs#L766) advances the
carrier's accepted delayed zap before its normal update. A pending, non-expiring
zap does not prevent climb/fire. On expiry, removal/release occurs without that
step's normal movement or firing. Player-shot collisions precede zap expiry.
Bullets reject zaps through the base
[Enemy.TryZap](../../src/Tempest.Game/Gameplay/Enemy.cs#L9); they neither
consume accepted first-use delay slots nor become second-use threats.
No proposed change to any of these semantics.

[KillPlayer](../../src/Tempest.Game/TempestGame.cs#L398) enters a 60-step death
freeze, clearing player shots immediately. A lethal bullet is removed;
other bullets and carriers remain frozen, including firing timers, until
[reconstruction](../../src/Tempest.Game/TempestGame.cs#L324) clears them.
Unresolved original identities return as fresh Eggs. An unreleased unresolved
Tanker re-hatches with newly drawn ammo/timer, not its old firing state.
Released cargo returns independently, not as a reconstructed carrier.
Game Over and full restart also clear the field.

[All-rim eligibility](../../src/Tempest.Game/TempestGame.cs#L1030) ignores
bullets for enemy accounting, but requires all active enemies at height >=400,
no reserve/Eggs/queued cargo, and a consistent Tanker identity ledger with no
latent cargo. A normally climbing Tanker releases above 385 before it could
qualify as an at-rim carrier. Its below-rim cargo remains a blocker.
Lethal collision checks precede all-rim completion.
[BeginExit](../../src/Tempest.Game/TempestGame.cs#L1084) clears residual bullets
on either ordinary completion or all-rim clear.

### Development gating

[Scenario construction](../../src/Tempest.Game/TempestGame.cs#L132):
`--tanker-test` uses a controlled Level 1 board/supply and stable Tanker slot
zero; other original slots remain Flippers. With no enemy-bullets flag the
Tanker has zero ammo and remains silent, including a frozen timer.
`--enemy-bullets-test` alone does not introduce Tankers.
The combined flags arm Tankers **and ordinary/cargo Flippers**.
They do not represent Tanker-only firing promotion.
The Level 3 geometry scenario is mutually exclusive with `--tanker-test`;
combining them is not a valid C2 launch command.

## 4. Literal Incitti implementation

The actual local [tempest.bmx](../../Reference/incitti/tempest.bmx), not a
historical summary, establishes:

- [Baddies fields](../../Reference/incitti/tempest.bmx#L2166): height, ammo
  (`hasbullets`), timer (`bullettime`), and delayed zap (`dietime`).
- [Tanker.Create](../../Reference/incitti/tempest.bmx#L2622): height `10`;
  ammo `Rand(1,2)`; timer `16 + Rand(64)`; insert carrier in `BADDIE_LIST`.
  The one-argument integer `Rand(64)` means **1..64 inclusive**.
- [Tanker.Update](../../Reference/incitti/tempest.bmx#L2555): decrement positive
  `dietime`; if it reaches zero, score 200, release stillborn cargo, remove
  carrier, and return before climbing/firing.
- Otherwise add `tankerclimbspeed`, then only if height `<380` and ammo `>0`
  decrement timer. On timer `<0`, reset to 16, roll `Rand(0,100)>75`, create
  a bullet and decrement ammo only on success.
- **There is no Tanker `Shoot()` method or invocation.** The firing block is
  inline at [2574](../../Reference/incitti/tempest.bmx#L2574). The separate
  Flipper `Shoot()` method must not be mistaken for Tanker behavior.
- After the firing block, [natural release](../../Reference/incitti/tempest.bmx#L2586)
  tests height `> depth-15`, releases cargo and removes the carrier.
  No Tanker lateral motion, firing pause, or later rim-firing state.
- [Projectile creation](../../Reference/incitti/tempest.bmx#L2390) records the
  lane/edge and supplied height, plays bullet audio, and independently appends
  a `BAD_BULLET` to `BADDIE_LIST`; it keeps no carrier reference.
- [Bullet update/collision](../../Reference/incitti/tempest.bmx#L2351):
  `+3` height per update; remove above depth; same-lane player-shot destruction
  strictly below bullet for 25; same-lane player hit with gap `<8`;
  `ZapIt` returns false. Natural release and shot/zap carrier removal do not
  delete previously created bullets.
- [Baddies.UpdateBaddies](../../Reference/incitti/tempest.bmx#L2183) iterates
  one linked list for updates, then player collisions. It updates non-spike
  baddies during `LEVEL_PLAYER_DYING` but suppresses their player collisions.
  During zoom/reverse zoom only spikes update/collide. The
  [main loop](../../Reference/incitti/tempest.bmx#L3750) skips that block during
  `LEVEL_INTO_VORTEX`.
- [Death conversion](../../Reference/incitti/tempest.bmx#L2224) removes bullets
  and returns remaining non-spike enemy units to reserve subject to its cap;
  new Tanker creation draws fresh ammo/timer. Unlike C#, this is not a stable
  carrier/cargo-identity restoration contract.
- [Reference all-rim calculation](../../Reference/incitti/tempest.bmx#L2184)
  uses enemies above `BAD_SPIKE`; bullets do not keep `onrim` false. When supply
  and Eggs are empty, [main-loop completion](../../Reference/incitti/tempest.bmx#L3811)
  accepts `enemiesleft=0` or `onrim`, then starts travel. Existing bullets are
  not explicitly deleted at that transition; [ClearLevel](../../Reference/incitti/tempest.bmx#L4145)
  later removes all baddies. Do not replace C#'s accepted exit cleanup with this.

### Displayed Level 3 numeric semantics: correcting the old report

[Displayed number](../../Reference/incitti/tempest.bmx#L3805) is
`current_level+1`; [board identity](../../Reference/incitti/tempest.bmx#L1426)
is `current_level Mod 16`. Thus displayed Level 3 has `lv=2`, board=2.
[SetUpEnemies](../../Reference/incitti/tempest.bmx#L4210) makes Tankers
available at board>1; only Flipper cargo is enabled at this tier.
The [speed assignment](../../Reference/incitti/tempest.bmx#L4258) is
`1.75 + lv/20`. There is no level-dependent Tanker timer/ammo/chance formula.
The initial global `1.5` is overwritten by enemy setup.

`lv` and `20` are Int operands. `/` has higher precedence than `+`, so the
division evaluates as an integer expression before adding 1.75:
**1.75 + (2 / 20) = 1.75 + 0 = 1.75**.
The speed's `#` Float declaration does not make the nested division fractional.
The expression increases the speed in whole-unit steps every 20 internal
levels, not by 0.05 per level.

To establish language behavior beyond the local game text, inspected public
classic BlitzMax engineering sources (not arcade evidence):

- [Variables documentation](https://blitzmax.org/docs/en/language/variables/):
  omitted variable types default to Int.
- [Operator precedence documentation](https://blitzmax.org/docs/en/language/expressions/):
  divide precedes add.
- [Classic compiler arithmetic](https://github.com/blitz-research/blitzmax/blob/master/_src/compiler/exp.cpp#L781):
  evaluates child operands independently, balances their types, and uses integer
  division for integer operands; see lines 811, 824, 834 and 890.
- [Classic compiler type balancing](https://github.com/blitz-research/blitzmax/blob/master/_src/compiler/val.cpp#L146):
  Int with Int remains Int; a float in a later addition only promotes that
  addition.
- [Classic BRL.Random](https://github.com/blitz-research/blitzmax/blob/master/mod/brl.mod/random.mod/random.bmx):
  `Rand(min_value,max_value=1)` establishes inclusive integer ranges and the
  one-argument 1..x convention.

The old [Level 3 report](2026-10-31-level3-tanker-progression-analysis-report.md#L117)
incorrectly claimed the floating destination made this formula 1.85. This
report supersedes that interpretation; the earlier
[Tanker analysis](2026-10-30-tanker-analysis.md#L23) had correctly left numeric
semantics open. No old report or authoritative document was edited here.
These public language sources establish classic source semantics, not that
Neal's particular replay executable is byte-identical to the local reference.

### New-bullet update ordering

Reference projectile movement on the creation step depends on list position.
The public classic
[linked-list implementation](https://github.com/blitz-research/blitzmax/blob/master/mod/brl.mod/linkedlist.mod/linkedlist.bmx#L68)
pre-advances the enumerator to the current item's successor in `NextObject`;
`AddLast` inserts before the head sentinel (lines 155 and 403 also inspected).

- If the firing Tanker was the last item when fetched, the enumerator already
  points at the sentinel: its new bullet waits until the next update pass.
- If it had a successor, an appended bullet can be visited later in that
  same pass and already gain `+3` before player collision.

This is conditional executable list behavior, not an unconditional
"all reference bullets update immediately" rule. C# deliberately always
defers first movement. Tanker shot-creation counts and timer expiry are not
changed by this one-step projectile-movement difference.

## 5. Side-by-side comparison

RV = REFERENCE VERIFIED; PD = PROJECT DECISION; DEV = DEVELOPMENT ONLY.
Labels describe authority, not a claim that a C# test validates Atari internals.

| Property | Literal Incitti | Current C# | Evidence / difference |
|---|---|---|---|
| Individual interval shots; two can occur | Compatible with source algorithm | Compatible with armed algorithm | **ARCADE VERIFIED** qualitative observation only; exact constants below are not arcade verified |
| Armed ammunition | Rand(1,2) | Inclusive 1..2 | **RV** match; **DEV** arming gate |
| Initial timer | 16+Rand(64), 17..80 | 16+inclusive(1,64), 17..80 | **RV** match; first eligible decrement is already on hatch step |
| Eligible updates | Normal update after non-expiring zap; post-climb h<380, ammo>0 | Climbing; post-climb h<380, ammo>0, non-null random seam | **RV** active-play match; **PD** death freeze; **DEV** null seam |
| Timer decrement | Once per eligible update | Same | **RV** match; none at/above ceiling or with exhausted ammo |
| Random gate | Rand(0,100)>75 | Inclusive(0,100)>75 | **RV** match, 25 accepted integers out of 101; PRNG implementations/sequences differ |
| Timer reset | 16 on every expiry, success or failure | Same | **RV** match; 17-update retry spacing |
| Ammo consumption | One only after bullet creation | One only when returning a created bullet | **RV** match; failure costs nothing |
| Spawn height / lane | 10, supplied lane | 10, supplied lane | **RV** match; **DEV** slot-zero deterministic scenario |
| Climb rate at displayed Level 3 | 1.75+integer(2/20) = **1.75** | **1.85f** fixed | **RV** reference; **DEV** unsupported approximation, proposed correction |
| Firing ceiling | Strict h<380 | Strict h<380 | **RV** match |
| Release threshold | Strict h>depth-15, depth=400 | Strict h>385 | **RV** match |
| Movement/fire/release order | Climb, fire if eligible, release if above threshold | Same | **RV** match; never fires on natural-release-triggering update |
| Shot creation | Independent Bullets on carrier lane at advanced height | Independent EnemyBullet at same lane/height | **RV** gameplay match; audio/render implementation outside C2 |
| Bullet movement / expiry | +3, expires h>400 | Same | **RV** match after creation |
| First projectile movement | Same pass if carrier had successor; otherwise next pass | Always next step | **PD** existing deferred-update contract; not a Tanker ammo/timer defect |
| Shot lifetime independence | Survives carrier shot/zap removal or release | Same | **RV** match, subject to ordinary bullet/level/death cleanup |
| Pending zap | May climb/fire until expiry; expiry returns before ordinary update | Same in active play | **RV** match; existing Zapper contract unchanged |
| Player death | Baddies keep updating during dying, then freeze in vortex; conversion later removes bullets | All entities freeze for 60 dying steps; reconstruction clears bullets | **PD** established C# lifecycle, not literal global reference timing |
| All-rim / exit | Bullets excluded from onrim; retained until later cleanup | Bullets excluded from eligibility; cleared at BeginExit | **PD** established lifecycle; do not change |
| Wall-clock pacing | Loop delays toward 16 ms; nominal <=62.5 updates/s | One simulation step/render frame, target 60 FPS | **PD** existing global pacing; rates depend on actual runtime load |
| Development flags | No equivalent flags; source Tankers armed at creation | Tanker scenario silent unless combined bullet flag; combined also arms Flippers | **DEV**, not production Level 3 |
| Exact Atari ammo/delay/chance/ceiling/count | Not established by this source | Not established by tests | **unresolved**; no constant is ARCADE VERIFIED |

**Conclusion:** C# accurately ports the Tanker's *firing decision block and
initialization ranges*, but is **not a literal end-to-end timing implementation**
because of climb speed and existing global lifecycle/frame/update contracts.
Only the unsupported Tanker climb approximation is recommended for correction
in C2; broad frame-loop, death, collision-order, audio or rendering changes
would violate the isolated scope.

## 6. Deterministic window arithmetic and randomized reachable outcomes

Count from constructor height 10; update 1 includes the hatch step. Assume
uninterrupted ordinary climbing, no carrier shot/zap destruction, no player
death, and no premature exit. These conditions describe an observation window,
not a claim that every manual run follows it.

For exact real arithmetic, eligible n satisfies `10+n*s<380`; release n is
the first with `10+n*s>385`. Binary32 repeated additions were also checked with
PowerShell casts after every addition, matching the C# field arithmetic.
No gameplay source, randomness, tests, or game state was altered for that check.

| Quantity | Current C# s=1.85f | Literal Level-3 reference s=1.75 |
|---|---|---|
| Eligible countdown updates | **199**, n=1..199 | **211**, n=1..211 |
| Last eligible height | About 378.150757 (binary32) | 379.25 exactly |
| First silent near-release update | n=200, about 380.000763 | n=212, 381 |
| Natural release | n=203, about 385.550781 | n=215, 386.25 |
| Silent end-of-climb updates | n=200..203 | n=212..215 |
| Armed initial timer T | 17..80 | 17..80 |
| First random decision | n=T+1, **18..81** | Same |
| Retry/next-shot decisions | T+1+17k while eligible and ammo>0 | Same |
| Available decision windows if ammo never exhausts | **7..11** depending on T | **8..12** depending on T |
| Earliest two successful shots with ammo=2 | n=18 and 35 (T=17) | Same |
| With maximum initial delay, first two possible successes | n=81 and 98 (T=80) | Same |

For eligible-update count E, the maximum number of decision rolls before the
ceiling, assuming remaining ammo, is
`1 + floor((E - (T+1))/17)`. Actual rolls stop early if ammo is depleted.
The slower literal climb adds 12 eligible updates, sometimes one extra decision
window for the same initial timer (not an extra guaranteed bullet).

### Reachable outcomes, not an exact expected count

| Policy / ammo | Zero shots before release | One shot | Two or more |
|---|---|---|---|
| Current silent --tanker-test | Deterministic | Impossible | Impossible |
| Armed current C#, ammo=1 | All available rolls fail | At least one succeeds | Impossible |
| Armed current C#, ammo=2 | All available rolls fail | Exactly one succeeds before cutoff | **Two possible; >2 impossible** |
| Literal Incitti, ammo=1 | All available rolls fail | At least one succeeds | Impossible |
| Literal Incitti, ammo=2 | All available rolls fail | Exactly one succeeds before cutoff | **Two possible; >2 impossible** |

Here "succeeds" means roll 76..100. All-failure, one-success, and two-success
decision sequences are supported by the algorithm; no per-Tanker result is
deterministically guaranteed by its 25-of-101 gate. This characterizes the
algorithm's allowed draw outcomes, not exhaustive enumeration of every seed of
either shared PRNG or an assumption of identical random streams.

The reference and current armed policy can both **reasonably produce multiple
individual shots**: even the largest initial delay permits a second success at
update 98, well before either cutoff. They also legitimately produce zero or
one. A Tanker initialized with one ammo unit can never produce two; Neal's
arcade observation says two *can* occur, not that all Tankers must fire twice.

### A concrete divergence that could affect the earlier observation

Use supported values ammo=2, T=80. Fail at decision updates
81, 98, 115, 132, 149, 166; succeed at 183 and 200:

- Current C# creates the first at 183, then its `<380` gate excludes update
  200, leaving one shot and one unused ammo unit at release.
- Literal Level-3 speed still permits update 200 (height 360), so it creates
  the second before release at 215.

This is a **controlled reachable example**, not a reconstruction of Neal's
random run. It establishes that the speed mismatch can remove a second-shot
opportunity. There is no evidence that this was the actual cause of his run;
ammo=1, failed rolls, carrier destruction, death or incomplete observation
also explain seeing one.

### Wall-clock qualification

C# targets [60 FPS](../../src/Tempest.Game/Program.cs#L35) and does not have a
separate elapsed-time accumulator. Incitti's
[loop](../../Reference/incitti/tempest.bmx#L3808) aims for 16 ms per iteration.
At ideal targets, 17 eligible updates take about 0.283 s in C# versus 0.272 s
in Incitti; initial decisions span about 0.300..1.350 s versus 0.288..1.296 s.
These are **engineering arithmetic**, not measured Atari timing.

The current release count corresponds nominally to 203/60 = 3.383 s, while
literal reference is 215*0.016 = 3.440 s. Matching speed in C# gives
215/60 = 3.583 s, not an exact wall-clock reproduction of Incitti.
Frame load, scheduler behavior and intro/death phases can change real times.
Do not "fix" global FPS or compensate with invented Tanker timer constants
in C2. Manual comparison should assess the combined qualitative result.

## 7. Existing automated protection and gaps

Tests were inspected, **not rerun**. No build/test suite was necessary for
analysis-only changes. The following are source-level coverage findings, not
a new claim that tests passed at this checkpoint.

| Existing tests / location | Already protected |
|---|---|
| [CombinedTankerAndBulletFlagsArmBothTankerAndFlippers](../../tests/Tempest.Game.Tests/SimulationTests.cs#L1965), [TankerTestAloneKeepsTankerUnarmedAndSilent](../../tests/Tempest.Game.Tests/SimulationTests.cs#L1988) | Combined flag wiring, ammo=2/max-offset case, hatch-step countdown; silent default and unchanged timer |
| [TankerFiringTimerDecrementsResetsAndUsesStrictDecisionBoundary](../../tests/Tempest.Game.Tests/SimulationTests.cs#L2001) | Timer reaches zero without firing; below-zero reset; 75 fails/76 fires; failure retains ammo; next expiry after 17 updates; exhausted ammo freezes |
| [UnarmedTankerNeverRunsTimerOrDrawsDecisions](../../tests/Tempest.Game.Tests/SimulationTests.cs#L2046) | No random draw with zero ammo |
| [TankerFiringRequiresPostClimbHeightStrictlyBelow380](../../tests/Tempest.Game.Tests/SimulationTests.cs#L2064) | Post-climb 379/380/381 boundary; origin at advanced height; timer skipped outside ceiling |
| [TankerCannotFireOncePendingOrAfterCargoRelease](../../tests/Tempest.Game.Tests/SimulationTests.cs#L2088) | Oversized release-triggering step skips fire; pending/released states cannot fire |
| [ArmedTankerUsesControlledInclusiveAmmunitionAndTimerOffset](../../tests/Tempest.Game.Tests/SimulationTests.cs#L2112), [TankerDecisionBoundaryMatchesFlipperConventionInGame](../../tests/Tempest.Game.Tests/SimulationTests.cs#L2124) | Controlled ammo 1/2 and offset 7; integration uses inclusive 75/76 boundary |
| [TankerFiresFromLaneAtPostClimbHeightThroughGameUpdate](../../tests/Tempest.Game.Tests/SimulationTests.cs#L2141) | Creation step does not advance new bullet; lane/height, reset and unchanged accounting/score |
| [TankerBulletSurvivesTankerShotDestructionAndIsWorthTwentyFive](../../tests/Tempest.Game.Tests/SimulationTests.cs#L2156), [TankerBulletSurvivesNaturalCargoRelease](../../tests/Tempest.Game.Tests/SimulationTests.cs#L2182) | Independent projectile survives carrier removal; bullet shot score/accounting |
| [LifeLossClearsTankerBulletsAndRearmsFreshTankerWithNewValues](../../tests/Tempest.Game.Tests/SimulationTests.cs#L2201) | Death freeze retains then clears bullet; fresh carrier identity/runtime ammo/timer |
| [CombinedModeCompletesWithBulletsAndClearsResidualsAtExit](../../tests/Tempest.Game.Tests/SimulationTests.cs#L2235) | Full deterministic combined mode can complete; some bullet seen; no residual exit bullets |
| [TankerFixedSpeedKeepsLaneAndReleasesStrictlyAbove385](../../tests/Tempest.Game.Tests/SimulationTests.cs#L2292), [TankerUsesDocumentedDevelopmentSpeedAndNeverDirectlyHitsPlayer](../../tests/Tempest.Game.Tests/SimulationTests.cs#L2309) | Strict release boundary using explicit speed 1; default speed currently locked to **1.85** |
| [Shared bullet boundaries](../../tests/Tempest.Game.Tests/SimulationTests.cs#L3308) | +3, expiry above 400, same-lane gap<8, shot-below-height; other shared tests protect death and exit |
| [Zapper eligibility/order](../../tests/Tempest.Game.Tests/SuperZapperTests.cs#L14) | Carrier accepts zap; bullets reject; rejected bullets do not consume first-use delay slots |

Important limitations:

- Existing focused timer test fires **one** shot. Most Tanker lifecycle fixtures
  deliberately use ammo=1. The combined full-wave test requires "a bullet seen",
  not two bullets created by the same Tanker before natural release.
- No default-speed whole-climb proof of zero/one/two creation counts, eligible
  update total, or full initial-offset endpoints through the entire lifetime.
- Current tests positively protect the 1.85 approximation rather than proving
  the literal Level-3 integer-speed formula.
- Natural-release survival fast-forwards the Tanker outside the game loop;
  useful removal coverage, but not a natural timed multi-shot acceptance test.
- Generic Zapper tests do not explicitly demonstrate armed Tanker firing while
  pending, no firing on carrier expiry, and retained existing carrier bullets.
- Existing shared tests do not establish seed-for-seed Incitti equivalence,
  reference wall-clock timing, or arcade authenticity.

### Deterministic seams are sufficient

The entity accepts `Func<int,int,int>`; the game accepts an injected `Random`.
[BulletRandom](../../tests/Tempest.Game.Tests/SimulationTests.cs#L4404) and
[TankerBulletModeRandom](../../tests/Tempest.Game.Tests/SimulationTests.cs#L4425)
already demonstrate both seams. No production randomness change is required.
For new acceptance tests, prefer a strict range-aware test double that asserts
requested ranges and fails on unexpected draws. The old queue double clamps
values and defaults to the minimum, which can hide a broken test sequence.
Count **bullet creation events**, not only simultaneous live-bullet count:
earlier bullets can expire before later bullets are emitted.

### Proposed focused C2 automated gate

1. Before/alongside the correction, characterize current default speed:
   199 eligible updates, release at 203, and the ammo=2/T=80 late-window example.
   Do not retain a permanent acceptance assertion requiring the old approximation
   after intentionally correcting it.
2. Final acceptance for default speed **1.75f**: hatch height 11.75; constructor
   height 10; 211 eligible updates; last eligible height 379.25; first silent
   height 381; release at update 215, height 386.25. Keep the existing explicit
   speed-1 strict-385-boundary test.
3. Whole-climb deterministic matrix: ammo 1 and 2, offsets 1 and 64; all failures
   => zero, exactly one success => one, two successes with ammo=2 => two.
   Ammo=1 with all successes => exactly one. Check no further rolls after ammo
   exhaustion and no timer change/draw at the height cutoff.
4. Assert first decision at T+1 and subsequent decisions at +17. With failed
   rolls and ammo remaining, endpoint offsets produce 12 and 8 windows at
   corrected speed. Include 75/76 and range `(0,100)` assertions.
5. Make the late-window example a regression: ammo=2/T=80, six failures then
   success at 183 and 200 => two creations before release, no guaranteed shots
   under arbitrary production randomness.
6. One game-level timed multiple-shot path through normal updates, safely
   avoiding player death and carrier destruction; verify creation count/lane/
   origin, continuous climb, first projectile motion next step, natural carrier
   removal, unchanged accounting, and no Tanker shot on release.
7. Armed pending zap: firing remains possible on non-expiry steps; expiry
   prevents normal movement/fire; existing bullets survive. Retain accepted
   first/second Zapper behavior, scoring and cargo disposition.
8. Retain/re-run focused Tanker/cargo, enemy-bullet, Zapper, completion/death and
   development-flag tests. Existing default hatch-height expectations near
   11.85 must intentionally become 11.75. If a focused failure identifies a
   tightly coupled default-speed assumption, update only that assumption.

Future validation commands, run from the repository (not executed here):

```powershell
dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --filter "FullyQualifiedName~Tanker|FullyQualifiedName~Bullet|FullyQualifiedName~SuperZapper|FullyQualifiedName~AllRim"
dotnet build .\src\Tempest.Game\Tempest.Game.csproj --no-restore
```

Use the repository's existing test runner; expand only if focused results
require broader regression validation. No new testing tool or random seed
configuration is needed.

## 8. Narrow next implementation scope

Only upon a later implementation request:

- [Tanker.cs](../../src/Tempest.Game/Gameplay/Tanker.cs): change the isolated
  default development climb constant from 1.85f to **1.75f** and correct its
  explanatory summary to identify literal early reference-tier integer semantics.
  Do not rename/restructure the entity or alter timer/ammo/chance/ceiling.
- [SimulationTests.cs](../../tests/Tempest.Game.Tests/SimulationTests.cs):
  correct directly coupled default-speed/hatch assertions and add the focused
  whole-lifetime and integration acceptance cases above using existing seams.
- [SuperZapperTests.cs](../../tests/Tempest.Game.Tests/SuperZapperTests.cs)
  only if the narrowly armed-carrier regression belongs there rather than the
  existing firing section. No production Zapper source change.
- Future directly related documentation should record the reference correction,
  gate results and still-development-only firing status in the
  [development notes](../../TEMPEST-DEVELOPMENT-NOTES.md) and implementation AI
  report; update any actively authoritative statement claiming 1.85 as literal.
  The [behavior specification](../../TEMPEST-BEHAVIOR-SPEC.md) may record the
  reference value without promoting it to arcade evidence. The
  [README](../../README.md) needs a change only if documenting new gate results
  or commands; no new command is necessary for this correction.

No changes expected to game orchestration, board definitions, cargo routes,
inward pivot/directed sweeps, player input/spinner, renderer, global FPS,
Flipper ammo/firing, composition factories, ledger model or progression.
No need to combine the Level 3 geometry and Tanker scenario flags, introduce
production Level 3, or add a manual forced-success randomness option.

## 9. Qualitative manual validation gate

Current baseline comparison, and again after the separately authorized C2
correction, from the repository:

```powershell
dotnet run --project .\src\Tempest.Game -- --tanker-test --lives 10
dotnet run --project .\src\Tempest.Game -- --tanker-test --enemy-bullets-test --lives 10
```

Run separately. The first is the silent control; the second already provides
a repeatable single-carrier scenario, **not** a pure bullet-only wave.

1. In the silent control, confirm the Tanker remains silent and the accepted
   release/cargo motion remains unchanged.
2. In combined mode, identify the magenta carrier and watch it continuously
   from first appearance to its natural two-cargo release. Avoid shooting it
   or using Zapper during this observation. Move off its lane so its bullet
   does not kill the player; stay alive and avoid other Flippers.
3. Count distinct bullets visibly originating at that carrier while it is
   still present, not white bullets from other lanes/Flippers or shots after
   cargo release. A bullet travels toward the rim in the carrier's lane.
   Two need not be simultaneously visible to count as two individual shots.
4. Repeat fresh runs until a complete uninterrupted carrier climb with two
   individual shots has actually been observed. Natural randomness does not
   guarantee two in a given run; recording zero and one is valid too.
   Fresh process launches are a practical replay method, avoiding an
   undocumented mid-wave restart assumption. Extended lives help, but a death
   invalidates that carrier's uninterrupted observation window.
5. Compare qualitatively with the corresponding early Tanker tier in Incitti,
   then Neal's Atari experience: plausible climb duration, opportunity to see
   separated shots, continued motion during firing, and a silent near-release
   region rather than a rim burst. Do not require a measured Atari interval,
   a fixed shot count, exact ammo, or a quantitative probability.
6. As separate observations, fire once then destroy the carrier / let it
   release, checking its existing bullet remains independent; verify death
   cleanup and a fresh silent/armed re-hatch as appropriate. Retain accepted
   cargo, input and Zapper behavior; these are regression checks, not new rules.

**Manual pass:** Neal can observe at least one unbroken carrier instance emit
two individual, separated shots before natural release, considers the overall
behavior qualitatively compatible with Atari and the strong Incitti comparison,
and sees no boundary regressions. This validates observable capability, not
every Tanker's result or exact constants.

If repeated complete observations still cannot establish that capability,
record the circumstances and keep firing development-only; do not tune on
absence of a random outcome. The deterministic acceptance test should first
distinguish emission capability from observation difficulties. Any further
instrumentation or tuning requires a new narrow authorization.

Passing C2 does not itself authorize C3 or production promotion; later production
integration must explicitly wire Tanker arming without implicitly promoting
Flipper bullets through the existing combined development flag.

## 10. Unresolved uncertainties and preserved boundaries

- Exact Atari ammo, initial delay, interval, random mechanism/probability,
  cutoff and total shots per Tanker remain unknown.
- Neal's recent overall Incitti timing impression is not a measured
  Tanker-specific result; the exact replay executable/compiler version is not
  established. Source semantics are verified, binary reproduction is not.
- The earlier single C# shot cannot be attributed to speed, ammo, chance or
  observation duration without recorded state. A speed-based explanation is
  possible, not proven.
- Global 60-FPS versus nominal 16-ms pacing, death freeze, creation-step
  projectile movement and exit cleanup remain project differences. Correcting
  one Tanker speed will not make the whole game frame/wall-clock identical.
- No exact source seed equivalence is promised between BlitzMax and .NET.
- Manual natural multi-shot capability on the corrected path is still pending.

No C3 work or natural Level 3 progression was started. No production mixed
wave, C1/C1.5 geometry, cargo routing/inward pivot, Zapper semantics, Flipper
firing policy, normal Level 1/2, spinner movement, rendering fidelity,
controller mapping, or reference content was changed.

## 11. Final maintenance

Only this report and the [exact supplied prompt](../Prompts/2026-10-31-c2-tanker-firing-policy-analysis-prompt.md)
were created. Both exist persistently under the established local AI-log
convention. [.gitignore](../../.gitignore#L6) ignores the entire AI-Logs directory,
so these requested deliverables do not appear as untracked Git changes.
`git check-ignore -v` confirmed that rule for both new files.

Final requested commands:

```text
git status --short
(empty)

git diff --check
(empty)

git diff --stat
(empty)

git status --short -- Reference/incitti
(empty)

git log -1 --oneline
27c43b7 Add validated controller restart and quit aliases
```

Tracked worktree remains clean. No gameplay/test files changed; no build/test
run, commit, staging operation or push was performed. C3 and natural Level 3
were not started.
