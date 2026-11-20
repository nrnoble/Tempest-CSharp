# Tempest whole-system progression architecture analysis

Analysis date: 2026-10-31. Requested filename retained.

Authority checkpoint: `2741f2a Implement arcade all-rim completion behavior`.
The checkout was clean at initial inspection. Analysis only: no gameplay,
tests, authority documents, project/configuration files, or Git state changed.
Only the requested ignored prompt and this report are deliverables.

## 1. Executive architectural summary

**Recommendation: establish a behavior-preserving Level 1 configuration and
initialization boundary before implementing real Level 2.**

The necessary architecture is not "increment a number and reset enemies."
Progression brings together:

```text
Session: score, lives, initial selection, progression identity
  -> resolved level configuration:
       board/topology + palette + supply + eligibility + tuning + exit policy
  -> runtime attempt:
       Eggs + enemies + cargo obligations + outcomes + spikes + clocks/latches
  -> wave-clear commitment
  -> exit travel, possibly reverse/death/retry
  -> successful-travel commitment
  -> next configuration and fresh runtime
```

The current mechanics are reusable. The architectural risks are concentrated
in fixed Level 1 configuration, scenario-dependent dispatch/accounting, and
reset ownership. A universal engine or enemy-hierarchy rewrite is not needed.

Two particularly important findings:

1. **The square already exposes a pivot-geometry mismatch.** Twelve adjacent
   lane pairs produce zero sweep under the current C# turn calculation.
   Simply injecting square coordinates is not sufficient.
2. **Production mixed waves cannot be created by removing scenario mutual
   exclusion.** Ordinary counters and the Tanker identity ledger currently
   use different resolution paths.

Evidence classifications throughout:

- **ARCADE VERIFIED:** only Neal's observations recorded in the current
  [behavior specification](../../TEMPEST-BEHAVIOR-SPEC.md).
- **REFERENCE VERIFIED:** literal Incitti behavior; not proof of Atari behavior.
- **PROJECT DECISION:** current C# rules or explicitly proposed architecture.
- **DEVELOPMENT ONLY:** artificial waves, flags, deterministic choices/seams,
  and validation instrumentation.

All recommendations are proposed **PROJECT DECISIONS**, not newly verified
arcade behavior. Exact Incitti gates, composition, geometry and numeric tuning
remain **REFERENCE VERIFIED** unless separately observed.

### Evidence inspected

The authority documents, all authored C# source, both authored test files and
test project, the complete Incitti main source, reference readme, related
projection/audio source, all seven levelsets, and relevant milestone reports
were inspected. A read-only test-contract exploration covered both complete
test files. No build, test execution, gameplay launch or arcade observation
was performed in this analysis.

The previous partial-prompt chat analysis is expanded and superseded by this
report where this report supplies a more exact qualification.

## 2. Incitti progression state model

**REFERENCE VERIFIED.** See
[globals](../../Reference/incitti/tempest.bmx#L136),
[completion](../../Reference/incitti/tempest.bmx#L1403),
[selection](../../Reference/incitti/tempest.bmx#L1544), and
[HUD](../../Reference/incitti/tempest.bmx#L3805).

Let `lv = current_level`, `d = lv + 1`, and `b = lv Mod 16`.

| Source state | Meaning and derivation |
|---|---|
| `current_level` | Zero-based position 0..95 within a source cycle |
| displayed level | `current_level + 1`, including Game Over |
| `current_board` | `lv Mod 16`; geometry row, not an enemy difficulty profile |
| `current_color` | Integer `lv / 16`; palette 0..5 |
| `current_set` | Independently selected geometry collection |
| `current_spin` | 1 when `lv > 53`, otherwise 0 |

| Displayed d | Internal lv | Board b | Palette | Rotation eligible |
|---:|---:|---:|---:|---|
| 1 | 0 | 0 | 0 | No |
| 2 | 1 | 1 | 0 | No |
| 16 | 15 | 15 | 0 | No |
| 17 | 16 | 0 | 1 | No |
| 33 | 32 | 0 | 2 | No |
| 49 | 48 | 0 | 3 | No |
| 55 | 54 | 6 | 3 | Yes |
| 65 | 64 | 0 | 4 | Yes |
| 81 | 80 | 0 | 5 | Yes |
| 96 | 95 | 15 | 5 | Yes |

Sixteen geometries repeat through six palette/difficulty bands in the same
set. Displayed Level 17 is not "levelset2 board 1." After 96 the source resets
`lv` to zero and increments the set, also resetting the derived difficulty
and rotation eligibility. There is no independent monotonically increasing
global level number or difficulty counter across sets.

### Selection, editor, and caps

- The selector offers internal `0,2,...,94`: displayed `1,3,...,95`.
  The readme's "all levels available" is not a literal every-level selector.
- Up/down selects a geometry set; the current loaded set is retained when
  choosing a start level. Preview drawing mutates global palette/center/offset,
  then actual selection re-establishes gameplay values.
- The integrated editor changes coordinates, continuity, center/offset,
  comments and descriptions; it saves the same text format. Mutation and
  manual rotate keys are debug/editor capabilities, not normal progression.
- Seven supplied sets contain 112 board definitions; normal Atari-oriented
  work initially needs the sixteen rows in the source-named "Original Arcade"
  set. Other collections are alternate content, not automatic Atari authority.
- The loader reserves capacity for 100 sets and reads filesystem entries
  without sorting or extension filtering. Capacity is not a meaningful
  approved progression maximum.
- The source wraps automatic set progression with `current_set > number_of_sets`
  rather than `>= number_of_sets`. Selection correctly uses count minus one.
  With seven loaded sets, automatic progression admits unpopulated set index 7.
  This source defect must not be adopted as recreation policy.

### Rotation

[Rotation handling](../../Reference/incitti/tempest.bmx#L3767) operates while
Ready or PlayerDying when rotation is enabled. When idle it rolls
`Rand(1000) > 995 - integer(lv/8)`, then rotates for 30 updates at randomly
signed 3 degrees per update (90 degrees total). `rotit` is cleared during
Begin. Point coordinates are mutated, then angles/midpoints are refreshed
through the normal geometry update path; lane identities do not get shuffled.
The rotation is not just a decorative renderer angle independent of gameplay
geometry. Do not infer any of this is arcade-correct.

**Required identity contract:** separate stable set identity, board index,
palette index and progression position. Do not use comment text or filesystem
enumeration order as authoritative gameplay identity.

## 3. Board/tube model

**REFERENCE VERIFIED:** [loader](../../Reference/incitti/tempest.bmx#L4326)
and [construction](../../Reference/incitti/tempest.bmx#L4178).

Every supplied set has sixteen comment/data pairs. Each numeric row has
35 fields: continuity, Y center, Y offset, and sixteen ordered `(x,y)` pairs.
Read-only shape checking found no wrong field counts in the 112 rows.

| File | Description | Closed | Open |
|---|---|---:|---:|
| [levelset1](../../Reference/incitti/levelsets/levelset1.txt) | Original Arcade | 10 | 6 |
| [levelset2](../../Reference/incitti/levelsets/levelset2.txt) | Tempest Tubes | 7 | 9 |
| [levelset3](../../Reference/incitti/levelsets/levelset3.txt) | Minty Fresh Goodness | 10 | 6 |
| [levelset4](../../Reference/incitti/levelsets/levelset4.txt) | Get Ready! | 9 | 7 |
| [levelset5](../../Reference/incitti/levelsets/levelset5.txt) | Something For Everyone | 9 | 7 |
| [levelset6](../../Reference/incitti/levelsets/levelset6.txt) | Classic Arcade Icons | 15 | 1 |
| [levelset7](../../Reference/incitti/levelsets/levelset7.txt) | a dimension of sound | 8 | 8 |

The original-set closed rows are 1-7, 12, 13 and 16. Its open rows are
8-11, 14 and 15. "Original Arcade" is a reference label, not independent
verification of every coordinate.

Closed boards have **16 vertices and 16 playable lanes**. Open boards have
**16 vertices and 15 playable lanes**, with no closing last-to-first segment.
Repeated coordinates and self-crossing shapes exist: continuity cannot be
inferred from visual coincidence, and intersecting lines do not imply lane
adjacency. Adjacency follows ordered data.

Depth remains 400 across source boards. Normal position reaches 50 after
entry. K is restored to 70; Z offset normally 5. Y center and Y offset vary
per board; they must remain distinct from world geometry and zoom state.

### Endpoint consumers

| Consumer | Literal source open-board behavior |
|---|---|
| Player | Stops at the terminal sub-lane endpoint instead of wrapping |
| Eggs | Reverse roaming direction at the first/last playable lane |
| `FindShortDir` | Direct signed lane difference; same-lane tie random +/-1 |
| Flipper/Pulsar | Reverse direction before a lane step outside valid range |
| Fuseball | Bounces with boundary `w` retained appropriately at 0/8 |
| Tanker cargo | Adjacent placement does not wrap; unavailable side can remain on carrier lane |
| Super Zapper | Direct distance when open |

Closed `FindShortDir` uses a fixed half-ring threshold 8, reverses the chosen
sign for farther destinations, and randomizes same-lane ties. C# ordinary
Flipper pursuit instead has deterministic equal-distance ties.

### Important source distance discrepancy

Incitti's single-zap closed distance code negates a raw lane difference when
its magnitude exceeds eight, then takes absolute value. Negation does not
shorten that magnitude. Thus across-wrap lanes can remain distance 15 in the
literal source. Current C#
[CalculateLaneDistance](../../src/Tempest.Game/Systems/SuperZapperSystem.cs#L104)
correctly uses `edgeCount - distance` for a closed ring.

Preserve the current tested C# distance policy. Earlier reports describing
that source expression as an exact shortest-distance calculation were too
broad; do not "correct" C# to the source defect during progression.

### Current implicit closed-Level-1 assumptions

| Surface | Concrete assumption |
|---|---|
| [Level](../../src/Tempest.Game/Levels/Level.cs#L17) | Constant ten-unit total, position/depth, fixed ten-entry spawn sequence |
| [Definition](../../src/Tempest.Game/Levels/Level.cs#L33) | Always loads Level 1 internally; runtime continuity independently mutable |
| [Adjacency](../../src/Tempest.Game/Levels/Level.cs#L243) | Uses point count and circular modulo for every lane step/distance |
| [Player](../../src/Tempest.Game/Gameplay/Player.cs#L44) | Circular movement, point count supplied as lane count, fixed lane 7 start |
| [Egg](../../src/Tempest.Game/Gameplay/Egg.cs#L48) | Readonly roaming direction and wrapping adjacency; no endpoint reversal |
| [Flipper](../../src/Tempest.Game/Gameplay/Flipper.cs#L22) | Fixed Level 1 rates; ordinary entrants only straight-climb below rim |
| [Pulsar](../../src/Tempest.Game/Gameplay/Pulsar.cs#L230) | Closed direction threshold 8; open movement partially supported |
| [Fuseball](../../src/Tempest.Game/Gameplay/Fuseball.cs#L41) | Open movement partly supported but relies on shared adjacency helpers |
| [Tanker](../../src/Tempest.Game/Gameplay/Tanker.cs#L132) | Closed previous/next cargo routing; exactly two Flipper cargo units |
| [Game](../../src/Tempest.Game/TempestGame.cs#L118) | Mutually exclusive artificial composition and immutable scenario flags |
| [Game dispatch](../../src/Tempest.Game/TempestGame.cs#L408) | Flags choose shot path, hatch factory, ledger, hurry-up and animation |
| [Renderer](../../src/Tempest.Game/Rendering/VectorRenderer.cs#L118) | Circular next-point helper; closes all rims; Level 1 palettes |
| [HUD](../../src/Tempest.Game/Rendering/VectorRenderer.cs#L577) | Scenario labels and hardcoded Level 1 Complete |
| [Program](../../src/Tempest.Game/Program.cs#L24) | Level 1 window title; terminal Complete routes to replay |
| [Projection](../../src/Tempest.Game/Rendering/Projection.cs#L9) | Shared mutable globals; no per-level entry/rotation runtime |
| [Tests](../../tests/Tempest.Game.Tests/SimulationTests.cs#L5155) | Default constructors, ten-unit assertions, lane bounds 0..15 |

A continuity flag alone cannot make those surfaces coherent.

## 4. Level-setup and tuning matrix

All literal formulas below are **REFERENCE VERIFIED**. Let `lv=d-1`,
`b=lv%16`, `q_n=integer(lv/n)`. Integer-only source division is stepwise.
Exact values have not been manually arcade-measured. "L2" means a dependency
of a later approved source-informed Level 2 implementation, not authorization
to implement it now.

Primary evidence:
[setup](../../Reference/incitti/tempest.bmx#L4208),
[Egg](../../Reference/incitti/tempest.bmx#L1970),
[Flipper](../../Reference/incitti/tempest.bmx#L2646),
[Tanker](../../Reference/incitti/tempest.bmx#L2407),
[Pulsar](../../Reference/incitti/tempest.bmx#L2943),
[Fuseball](../../Reference/incitti/tempest.bmx#L3285),
[Spinner](../../Reference/incitti/tempest.bmx#L3581).

| Value | Exact source rule | First affected displayed level | Current C# / verification / staging |
|---|---|---|---|
| Original supply/max Eggs | `min(64,10+q_2+2*b)` | 1; differs at 2 | Fixed 10; PROJECT DECISION. Configurable supply needed for L2; actual 12 needs approval |
| Initial Eggs | Four created, reserve decremented four | 1 | Four; preserve baseline in groundwork |
| Reserve release | On global clock mod 50, up to six Eggs | 1 | One per 50 active ticks under four-Egg cap; preserve intentional current pacing |
| Active Egg limit | No four-Egg cap in main release loop; death trims active Eggs to 16 | 1 / death | C# four cap. Source maximum initial supply 64 is not a universal active-object cap |
| Hatching concurrency | Approach allowed if `enemiesleft<9` and `hatchingeggs<2` | 1 | C# two in transit, no source active-nine hatch guard |
| Hatch-rate modifier | `q_8`; approach roll `Rand(0,100)>95-q_8`, or late-wave sum `<4` | 1; modifier first changes 9 | C# deterministic oldest roaming Egg each 50 ticks; promotion can wait |
| Egg movement | `cnt>7`; roam +/-1 depth, approach +2; roam may reverse vertically on random >95 | 1 | Eight-update cadence, deterministic vertical boundary reversal; no random vertical reversal |
| Flipper normal climb | 2 per climbing/pivot update | 1 | 2; exact arcade rate unmeasured; preserve |
| Flipper spike climb | 3 below spike tip; tip pause 8 | First spikes at 4 | Not implemented as ordinary Flipper behavior; required before production spikes |
| Flipper angular increment | `8+q_5` | 1; first increment 6 | Fixed 8; parameter contract needed, L2 remains 8 |
| Flipper lane pause | `21-q_5` | Ordinary branch first used 2; first decrement 6 | Cargo uses fixed rim-pause path 24; ordinary L2 requires separate lane pause |
| Flipper rim pause | `24-q_5` after pivot | 1; first decrement 6 | Fixed 24; source first arrival/initial pause differs, see section 11 |
| `canflip` | False only at lv=0 | 2 | No ordinary mid-tube branch; required for actual source-informed L2 |
| Spiker eligibility | `b>2 OR lv>47` | 4; permanent 49 | Only Spiker development wave; wait for production-spike milestone |
| Spiker speed | `lv/48.0 + Rand(2.5,4.5)` | 4 | Fixed 3 development speed; fractional Rand coercion/distribution unresolved |
| Spiker retreat | Growth >100 and `Rnd(0,120)>88`, or upper limit; descent 0.90*speed | 4 | Implemented with injected seam; arcade timing unmeasured |
| Spiker retreat bullet | Independent `Rnd(0,100)>70` on growth-driven retreat only | 4 | Opt-in; C# integer inclusive seam differs from source continuous roll |
| Initial spikes | Height 60 on every playable lane when spike-enabled | 4 | Development spikes on current 16 lanes; wait for production spikes |
| Tanker eligibility | `b>1 OR lv>17` | 3; permanent 19 | One deterministic original slot 0; production dispatch/ledger needed at 3 |
| Tanker climb | `1.75+q_20`; start height 10 | 3; first speed increment 21 | Development 1.85, not earliest source 1.75; parameterized already |
| Tanker release/fire ceiling | Updated height >385 releases; fire only below 380 | 3 | Implemented; exact arcade thresholds unmeasured |
| Tanker ammo/timer | `Rand(1,2)`, `16+Rand(64)`; expiry <0 resets16, roll >75 fires | 3 | Combined development bullet flag; preserve, promotion separate |
| Flipper ammo/timer | `Rand(1,3)`, initial16; expiry <0 resets16, roll >75 fires | 1 | Opt-in bullets; no source unlock gate; do not silently promote in L2 |
| Direct Fuseball eligibility | `b>9 OR lv>20` | 11; permanent22 | Artificial all-Fuseball wave only; wait |
| Fuseball climb term | `3.3+q_20`, twice on hurried climbing updates | 11; first increment21 | Configurable default3.3; visible hurry-up acceleration ARCADE VERIFIED, numeric rates not |
| Fuseball lateral cadence | `cnt>8` after increment; 9 updates | 11 | Implemented, exact cadence REFERENCE VERIFIED only |
| Direct Pulsar eligibility | `lv>15` | 17 | Artificial all-Pulsar wave only; wait |
| Pulsar vertical/pivot climb | 1 | 17 | Implemented; observed movement ARCADE VERIFIED, exact rate not |
| Pulsar angular increment | `10+q_8` | First usable17 (12); next25 (13) | Fixed12 development tuning; parameterize before production Pulsars |
| Pulsar lane/rim pauses | `20-q_6`, `16-q_6` | First usable17:18/14; next decrement19:17/13 | Fixed18/14; exact timings not arcade-verified |
| Pulse oscillator | Speed1; reset only >180; active window >70 and <110; amplitude `abs((count%90)-45)/4.0`, zero<2, cap12 | Global from1; relevant17 | 181 active-tick cycle; death/exit freeze PROJECT DECISION; no level scaling |
| Tanker Flipper cargo | Two, when ordinary Tankers eligible | 3 | Implemented development pair; survival/non-pursuit result ARCADE VERIFIED |
| Tanker Pulsar cargo | `lv>26`; Rand(1,2) units | 28 | Missing typed cargo/re-entry accounting; wait |
| Tanker Fuseball cargo | `lv>31`; Rand(1,2) units | 33 | Missing typed cargo/initial-height support; wait |
| Super Zapper supply | Two at level selection and successful next-level setup | 1 / every setup | Two initial/replay/restart; death preserves; add recharge at future advancement |
| Hurry-Up | Reset false in SetUpEnemies; latch after exhausted reserve/active Eggs | Each setup | Enabled only direct Fuseball/Pulsar scenarios; mixed production gate must become capability-based |
| Avoid-spike interval | Only lv3..6; `cnt>150-(lv-3)*40` | 4:151,5:111,6:71,7:31 updates | Always151 in Spiker scenario; exit policy must not equal "has spikes" |
| Exit delay | `cnt>20` | 1 | 21 subsequent updates; preserve |
| Rotation | `lv>53`; random burst rule section2 | 55 | Missing; wait |
| Score formulae | Flipper150, Tanker200, Spiker50, Pulsar200, bullet25, spike1; Fuseball height bands*250 | Respective eligibility | Implemented; broad shootability/scoring observations do not verify every exact source formula |

Source does not scale player speed, shot speed6, maximum shots10, bullet speed3,
death countdown60, depth400, or pulse speed with level. Preserve current input
sensitivity and freeze policy rather than importing source controls/death flow.
At lv95 the listed pauses remain positive: Flipper lane/rim2/5, Pulsar5/1.
No inferred extra scaling past lv95 is justified.

### Selected supply/configuration checkpoints

| d | lv | b | Original supply | Hatch modifier | Flipper turn | Flipper lane/rim pause |
|---:|---:|---:|---:|---:|---:|---|
| 1 | 0 | 0 | 10 | 0 | 8 | 21/24 |
| 2 | 1 | 1 | 12 | 0 | 8 | 21/24 |
| 3 | 2 | 2 | 15 | 0 | 8 | 21/24 |
| 4 | 3 | 3 | 17 | 0 | 8 | 21/24 |
| 8 | 7 | 7 | 27 | 0 | 9 | 20/23 |
| 11 | 10 | 10 | 35 | 1 | 10 | 19/22 |
| 16 | 15 | 15 | 47 | 1 | 11 | 18/21 |
| 17 | 16 | 0 | 18 | 2 | 11 | 18/21 |
| 28 | 27 | 11 | 45 | 3 | 13 | 16/19 |
| 33 | 32 | 0 | 26 | 4 | 14 | 15/18 |
| 49 | 48 | 0 | 34 | 6 | 17 | 12/15 |
| 55 | 54 | 6 | 49 | 6 | 18 | 11/14 |
| 65 | 64 | 0 | 42 | 8 | 20 | 9/12 |
| 81 | 80 | 0 | 50 | 10 | 24 | 5/8 |
| 96 | 95 | 15 | 64 (capped) | 11 | 27 | 2/5 |

Supply drops at board-cycle boundaries. Neither monotonic difficulty nor
"ten enemies forever" captures this reference.

## 5. Enemy composition matrix

**REFERENCE VERIFIED:** eligibility is configuration, while actual composition
is selected independently at each hatch under then-current active counts.

| Displayed range | Eligible direct categories / cargo |
|---|---|
| 1-2 | Flippers only; ordinary mid-tube branch enabled at2 |
| 3 | Flippers and two-Flipper Tankers |
| 4-10 | Above plus Spikers |
| 11-16 | Above plus direct Fuseballs |
| 17-18 | Flippers and direct Pulsars; board gate disables Tankers/Spikers/Fuseballs |
| 19 | Flippers, Pulsars, Tankers |
| 20-21 | Above plus Spikers |
| 22-27 | All direct categories; Tankers still Flipper cargo |
| 28-32 | All direct categories; Tanker Pulsar cargo also eligible |
| 33-48 | All except Spikers on board indices0..2; both later cargo types eligible |
| 49-96 | All categories and all cargo types eligible; rotation starts55 |

This is an eligibility map, not fixed quotas or verified arcade composition.

### Literal selection

`PickEnemy` draws `Rand(-6,tnk)`, with tnk4 below six active Tankers and tnk3
otherwise. Negative results become0. Ordinary candidates:
0 Spinner, 1 Flipper, 2 Pulsar, 3 Fuseball, 4 Tanker.

Before eligibility rejection, the normal draw has seven zero-result outcomes
and one for each positive candidate. With tnk4 that is 7/11 versus1/11 each;
with tnk3 it is7/10 versus1/10. Rejected disabled categories are rerolled.
Those raw ratios are not final probabilities for all levels.

If active enemies plus active Eggs <4, or active Spinners >3, an additional
draw from1..tnk excludes Spinner. Reserve Eggs are not in that sum. The
Spinner rule admits a fourth, not an unlimited number. The Tanker selection
range excludes a seventh when six remain active. No analogous enforced
Flipper/Pulsar/Fuseball cap exists in this hatch selector; the source's
numfuseballs counter alone is not a cap.

Tanker selection rerolls cargo type0..2: Flipper2, Fuseball1..2, Pulsar1..2,
subject to the separate cargo flags. There is no mixed cargo within a single
normally selected source Tanker.

Hatching replaces an Egg with one active carrier/enemy. Releasing a Tanker
creates n cargo enemies and removes one carrier, changing active count by
n-1. The source does not maintain C#'s stable original/cargo identity ledger.
Its maxeggs is an initial supply/reconstruction ceiling, not a conservation
total including all possible spawned cargo.

`rimit/onrim` observe exhausted reserve and active Eggs plus resulting active
enemies. Cargo participates once listed; below-rim children block all-rim.
Latent and queued C# cargo obligations must remain explicit blockers even
though the source has no identity sets for them.

## 6. Level 1 -> Level 2 transition

**REFERENCE VERIFIED:** source
[zoom](../../Reference/incitti/tempest.bmx#L1366),
[completion setup](../../Reference/incitti/tempest.bmx#L1403),
[cleanup](../../Reference/incitti/tempest.bmx#L4137).

The zoom completion check enters Complete when player depth is below10.
The next Level.Update visit executes setup:

1. Position becomes1500. If `bonuscollected` false, calculate outgoing-level
   startbonus, add score, mark collected, and add200 to entry position.
2. Reset Z offset5 and player depth400; state becomes Begin.
3. Clear Eggs, all baddies including bullets/spikes, shots/count, points,
   edges and active counters.
4. Increment level, perform source cycle/set wrap, derive board and palette.
5. Construct new geometry; K70, new board center/offset.
6. Set up supply, four initial Eggs, eligibility/tuning, initial spikes if
   enabled, hatch count0, hurry-up false, rotation enablement.
7. Recharge zap supply to2; player lane becomes7.
8. Reset level counter0; set animated entry center; flush keyboard input.

Score and remaining lives are not reset. The player's sub-lane `scl` and
shot timer are not explicitly reset by SetEdge. Pulse oscillator and Fuseball
global frame counter are not reset. Super Zapper display counter is not
explicitly reset by this setup either. ClearLevel does not clear explosion or
Fusepoint lists; they continue their own lifetimes. These details are source
semantics, not a recommended leak policy for C#.

Begin advances position by10 while above50 and updates screen center. Normal
1500 entry requires145 moving updates and a following Ready transition;
1700 requires165 moving updates and the following transition. Eggs/global
timers can advance during entry under the source loop, although approach
initiation requires Ready. It is not an inert fixed "wait two seconds."

For outgoing Level1, CalcBonus(0,0)=0, but the first guarded completion still
adds200 to position and marks the guard. Level2 geometry is the square,
palette0, supply12, no spikes, Flippers only, canflip true.

### Current C# comparison

[RunOneStep](../../src/Tempest.Game/TempestGame.cs#L139) stops at Complete.
R calls ResetLevel, keeps score/lives, resets the same Level1/scenario.
There is no identity increment, incoming board construction, Begin entry or
natural next-level dispatch.

Minimum eventual advancement changes:

- Explicit progression identity and resolved configuration.
- Fresh runtime initialization separate from reconstruction/replay.
- A successful-exit handler executed exactly once; not advancement at
  wave-clear or delay start.
- Next definition/supply/tuning, valid player placement, zap recharge,
  fresh accounting/hazards, explicit clock/presentation reset policy.
- HUD/input routing able to distinguish terminal scenario replay from normal
  progression entry/play.
- Complete Level2 Flipper behavior and square geometry before presenting the
  result as real Level2, not a straight-climbing placeholder.

Bonuses, source entry length and source timer carry need not all be copied.
Omissions must be named PROJECT DECISIONS rather than silently claimed faithful.

## 7. Bonus/extra-life analysis

**REFERENCE VERIFIED:** [CalcBonus](../../Reference/incitti/tempest.bmx#L1790),
[guard](../../Reference/incitti/tempest.bmx#L1403),
[score/lives handling](../../Reference/incitti/tempest.bmx#L738).

For outgoing board b and palette c, let n=b+16*c:

`bonus = n*3000 + n*c*500 + c*2000 + (n>3 ? n*4100 : 0)`.

Examples: displayed1 bonus0; displayed2 bonus3000; displayed4 bonus9000;
displayed5 bonus28400.

The crucial literal guard: `bonuscollected` is cleared at starting-level
selection, set on first completion, and is not cleared on subsequent level
setup in the inspected source. Therefore do not describe the executable
source as unconditionally awarding that formula every level. `startbonus`
is cleared at entry completion; display occurs during early Begin, conditioned
on positive score/current level/startbonus. This looks like a start-selection
reward path but its intended arcade meaning is not established.

When integer(score/10000) exceeds bonusmencnt, Player.Update adds **one** man,
sets visual/audio feedback, and updates the threshold counter. It does not
loop over every crossed threshold. Source new game sets men2 and ends below0,
which represents three deaths available, unlike C#'s direct Lives3 counter.

[ScoreSystem.CalculateLevelBonus](../../src/Tempest.Game/Systems/ScoreSystem.cs#L9)
exists but has no gameplay caller. No bonus-life/high-score economy is wired.

**Decision:** both bonus score and extra lives may be deferred from the first
Level2 milestone, explicitly. They do not gate geometry/setup/clear correctness.
Establish exactly-once successful-travel result ownership now so later bonuses
cannot repeat on spike retry, replay or repeated Complete visits.

## 8. Super Zapper lifecycle across levels

**REFERENCE VERIFIED:** source recharges at selection and after successful
new-level enemy setup, not in ordinary Baddie-to-Egg reconstruction.
Remaining charges survive exit until recharge; next level has2 irrespective
of unused supply. Source input is deathcount-gated rather than strictly
Ready-gated, so some source entry/pre-zoom uses are possible.

**PROJECT DECISION:** current C# accepts activation only during Playing.
Initial construction, Game Over restart and Complete replay reset supply2
and flash0. Death preserves charges, freezes pending enemy countdowns, and
reconstruction discards those countdowns with runtime objects.

All-rim at checkpoint2741f2a removes pending targets without later zap score;
expiry already processed earlier in that tick scores normally. Shared flash
continues during death/exit.

Required for eventual advancement:

- Recharge in fresh-level initialization at successful travel, not at clear.
- Preserve charges through death/retry.
- Define whether flash resets on new-level installation; recommend reset for
  fresh C# initialization, retaining source distinction in documentation.
- Keep inactive-phase input rejected until intentionally approved.
- Do not replay/recharge because a cleared wave is merely retrying spikes.

No new targeting hierarchy is required. Mixed-wave enumeration must include
all supported enemies in stable order independent of scenario flags.

## 9. Accounting/reset model

Current [normal Level accounting](../../src/Tempest.Game/Levels/Level.cs#L7)
conserves ten across reserve, Egg/active, destroyed and rim-cleared outcomes.
Return-to-reserve is not a kill. Destroyed and rim-cleared remain separate.

Current [Tanker ledger](../../src/Tempest.Game/TempestGame.cs#L39) separately
tracks unresolved/destroyed/escaped/rim-cleared original slots and unresolved/
killed/rim-cleared materialized cargo. Latent cargo is inferred around the
single designated slot0. Cargo IDs are assigned at materialization.

### Required generalization

- Original wave total becomes configuration data, not global const10.
- A runtime unit's identity/outcome is independent of its current Egg/enemy.
- Every production enemy resolver must update the same relevant accounting
  authority. Spiker/Fuseball/Pulsar ordinary-counter handlers do not currently
  resolve Tanker-mode original IDs.
- Multiple carriers need per-carrier cargo obligations, not a slot0 inference.
- Later cargo IDs need a retained kind/count/re-entry description, not only
  "IsTankerCargo means Flipper."
- Completion includes reserve/active/pending Eggs, latent and pending cargo,
  counted active enemies and unresolved identities.
- Spikes and bullets remain outside counted enemy obligations.
- Runtime pending zap timers die with discarded runtime objects.

Original and cargo IDs **can remain level-local**. A new runtime owns the ID
namespace and no old callbacks/entities may act on it. Add a level-generation
identity only if retained cross-level events later require it; a global GUID
scheme is unnecessary now.

### Ownership/reset matrix

| Event | Preserve | Fresh/reset |
|---|---|---|
| Life reconstruction | Identity/config, score/lives except one loss, killed/rim-cleared/escaped outcomes, charges, hurry-up, spikes | Runtime survivors as fresh Eggs, player pose, shots/bullets, per-enemy timers |
| Failed spike exit | Same level, terminal outcomes, score, spikes, remaining charges | Reverse/retry presentation, one life loss; no advancement/reward |
| New production level | Session score/lives and initial-selection policy | New identity/config, ledger, hazards, Eggs, player placement, level resources |
| Development replay | Same scenario, score/lives | Fresh same-wave runtime, initial hazards/resources |
| Game Over restart | Launch options | Initial identity, score/lives, fresh runtime/resources |

Pulse phase currently survives ResetLevel; Fuseball frame does not. Random
source survives resets. Release/hatch timers reset on reconstruction.
These are actual current differences, not proof all clocks share one lifetime.
Groundwork must preserve baseline behavior; future progression should state
its clock policy explicitly rather than accidentally resetting or carrying all.

### Minimal design choice

Reuse existing LevelDefinition as geometry data and Level as runtime:
**parameterized definition/configuration plus a fresh Level instance for a
new wave**, with TempestGame owning replacement and a named reconstruction
operation retaining the same runtime. Level already provides the runtime role;
do not introduce a duplicate LevelRuntime class solely for naming.

This is justified by the current immutable hardwired definition, per-level
hazards/queues, and need to distinguish next-wave initialization from life
reconstruction. A reusable Level with an explicit Load operation is possible
but requires carefully replacing all readonly/config fields; it offers little
benefit over fresh instances here.

On replacement, detach/rebind SpikeCreated subscriptions and clear game-owned
entities/order/queued cargo before they can refer to a new ledger. Renderer
already reads through game.CurrentLevel rather than storing a Level.

Before Level2, configure supply/config/initialization. Do not generalize all
carrier ledgers until production Tankers justify that additional change.

## 10. Rendering/topology requirements

The renderer can project arbitrary supplied points/center/offset, but it does
not load data. Current Level cannot accept a second definition, and tube draw
always closes all points.

### A. Required for Level2

- Select/install square definition through game initialization.
- Keep palette0: blue tube, yellow player, red Flippers/Eggs, white shots.
- Dynamic level HUD/title rather than Level1 terminal label.
- Square pivot/hinge and nonuniform adjacent segment alignment tests.
- Player and shots follow actual square lanes; normal lane7 is valid.
- Exit zoom uses current definition center/offset, no hidden circle assumptions.
- Preserve K70/Z5 baseline; do not tune projection to hide a geometry defect.

**Concrete pivot blocker:** current
[CalculateEdgeTurnAngle](../../src/Tempest.Game/Levels/Level.cs#L259) compares
lane direction vectors and folds the result to0..180. For the square,
adjacent pairs 0,1,2,4,5,6,8,9,10,12,13,14 yield0 degrees; corner pairs
yield90. Incitti's GetAngle uses vertex-angle logic, explicitly returns180
for collinear cases, and can select reflex sweeps. The two methods are not
equivalent, including their regular-circle conventions.

This is a verified mathematical mismatch, not a measured arcade sweep.
Blindly substituting source GetAngle would alter validated Level1 animation.
Establish oriented hinge/end-position behavior and isolate the necessary new
geometry handling while pinning Level1 output.

### B. Required for first open tube

- One authoritative lane count = points.Count-1 when open.
- Do not close near or far rims; retain all radial vertex boundaries.
- Player clamps at endpoints, Eggs reverse, enemies reflect as specified.
- Shared valid lane endpoints/distance for cargo and zap targeting.
- Render last valid lane using its real two vertices, never a fake last-first.
- Pulsar overlay removes only the affected near-rim connection during visual
  pulse; unchanged lanes remain. Preserve ARCADE VERIFIED narrower rule rather
  than Incitti's broader suppression.
- Spike lane centers and collision indices use playable lanes.
- Validate large spinner deltas, endpoints and exit/retry on open geometry.

### C. Later rotations/palettes/sets

- Palette by category, including Fuseball arms/flash/HUD, not just tube color.
- Ordered stable geometry catalog, not unsorted filesystem set number.
- Runtime transformed points or a consistent board transform shared by
  entity rendering and pivot calculations; immutable catalog coordinates stay
  unchanged.
- Rotation during allowed phases, updated angles/midpoints, no lane-ID swaps.
- Explicit handling of repeated coordinates, concave/reflex paths and set
  cycle exhaustion without importing source boundary defects.

## 11. Level2/canflip specifics

**REFERENCE VERIFIED**, not arcade-confirmed later Flipper behavior.

Original-set row2: closed square,16 lanes, center400, offset-80. Coordinates
subdivide four square sides at +/-161 with intermediate80/0/-80 positions.
Source internal lv1: supply12, initial4, palette0, no Tanker/Spiker/Fuseball/
Pulsar, canflip true, climb2, angular increment8, lane pause21, rim pause24.

### Exact ordinary source path

1. Hatch creates Flipper type0 at height0, initial ammunition1..3, timer16.
2. First type0 update climbs to2 and, because canflip true, sets oldtyp1 and
   typ3. That branch does **not assign pause**. Its newly created pause is0.
   Shoot is attempted and FindShortDir chooses direction from the player lane.
3. Next typ3 update decrements pause to-1, adds1 height, immediately steps to
   adjacent lane in retained direction, and enters type1 flipping.
4. Type1 updates advance angle8 and climb2. When the sweep completes, angle
   resets; typ3 pauses21 below rim or24 at rim, with oldtyp1 or2 respectively.
5. Below-rim pause adds1 height each update and retains direction. At/above
   rim, pause completion chooses direction toward the current player lane.
6. At a spike on a destination lane, the source can return to straight climb
   below the spike tip; that branch is relevant to later spike levels, not2.

This is not "randomly choose whether to flip every few ticks." The ordinary
canflip transition is deterministic once enabled. Direction comes from
FindShortDir; same-lane ties and ammunition/firing are random. Below-rim
direction is retained once the initial climb branch supplies it. Random hatch
rejection still selects only Flippers at Level2.

### Collision/presentation differences to preserve consciously

Incitti changes its edge index at pause expiration and renders the new edge
with residual-angle rotation. Current C# anchors Flipper drawing at source
lane and commits destination on pivot completion. These representations are
not interchangeable without aligning collision timing/render endpoints.

Source player shots collide only with OnEdge/current source-model lane in the
strict `0 < height-shot <16` band. C# pivoting Flippers accept both source and
destination lanes. Source player collision at rim can occur when remaining
angle<30; C# deliberately retains Neal's completed-pivot safety rule.
Mid-tube flipping is not directly lethal because player remains at rim.

Use current ARCADE VERIFIED pivot safety, current shot-before-player-collision
policy and current cargo survival geometry. Do not import the source's rim
angle death rule while implementing canflip.

Firing is attempted from climbing and pause-to-lane transitions, not every
pivot update. The existing optional firing seam is reusable; bullet promotion
into normal play remains an independent acceptance decision.

Current ordinary Flipper lacks separate lane pause and canflip behavior;
current cargo uses the shared24 pause below rim. Do not repurpose cargo ID
as the general ordinary mid-tube capability. Cargo's prescribed outward
route and non-pursuit are separate from ordinary canflip initialization.

Neal's Level1 observation confirms straight non-pursuing climb on Level1,
not later-level path or source canflip gate. A controlled square/one-Flipper
DEVELOPMENT ONLY scenario can isolate new animation and collision behavior
before the configuration is reachable through real progression.

## 12. First open-tube dependency

For the original reference set the first open geometry is displayed **Level8,
internal7, board7, Vee**. Level2 is closed, so complete open-end support can
safely wait until before8. It is not needed to show the square.

Alternate sets can be open at their first row (for example Minty Fresh
Goodness). They must remain unsupported production content until topology is
complete; selecting them early would invalidate the "open can wait until8"
scope.

Establish the vertex/lane distinction during groundwork. Implement all endpoint
behavior later as a coherent stage, not one enemy at a time called open support.
Before reaching8, natural Tankers and spikes are already needed at3/4.

## 13. Development-flag strategy

Preserve all flags in the first milestone and beyond:

| Flag | Proposed eventual role |
|---|---|
| `--spiker-test` | Fixed isolated spike/Spiker/Flipper wave with its existing artificial hazards/exit policy |
| `--tanker-test` | Fixed controlled original/cargo scenario, not production Level3 |
| `--fuseball-test` | Direct-hatch ten-unit validation scenario with development tuning |
| `--pulsar-test` | Direct-hatch ten-unit validation scenario with development tuning |
| `--enemy-bullets-test` | Explicit development override; preserve composition/arming combinations |
| `--lives` | Starting-life override orthogonal to scenario/progression |

Keep scenario mutual exclusion. Production mixed-wave eligibility is not
implemented by making these flags combinable. Keep scenario completion/replay
available rather than silently progressing an artificial wave into a real
next level. Injected random seams remain useful even where runtime movement
is randomized; existing scenarios are not all fully deterministic.

Eventually separate development selection from production configuration so
animation, collision, ledger and hurry-up are capability/runtime decisions,
not `FuseballTest`/`TankerTest` dispatch. That migration must preserve each
scenario's tested behavior and can be staged.

## 14. Current documentation inconsistencies

No authority documents were edited. These are future cleanup items, not new
gameplay requirements. Preserve history with explicit "historical/superseded"
labels instead of rewriting every old milestone as though it never happened.

| Document/location | Cleanup item / current truth |
|---|---|
| [README opening](../../README.md#L1) | Calls project based on Incitti; should foreground arcade observations as higher authority |
| [README tick order](../../README.md#L27) | Flipper-only summary omits shared pulse/zap, development enemies, all-rim/hurry-up phases |
| [README duplicate bullets](../../README.md#L68) | Two repeated Flipper bullet sections |
| [README Tanker sections](../../README.md#L100) | Duplicate descriptions disagree about inward/outward route and firing; final outward route and combined Tanker bullets are current |
| [README hurry-up](../../README.md#L183) | Manual pending/not arcade-verified text superseded by spec's scoped manual observations |
| [README zap](../../README.md#L216) | Old test totals and pending manual statements should be marked milestone-era |
| [Notes early Flipper rule](../../TEMPEST-DEVELOPMENT-NOTES.md#L127) | Blanket arrival-is-not-lethal statement conflicts with corrected direct-arrival REFERENCE VERIFIED rule |
| [Notes completion goal](../../TEMPEST-DEVELOPMENT-NOTES.md#L208) | Kill-all-only statement superseded by all-rim |
| [Notes early direction/work status](../../TEMPEST-DEVELOPMENT-NOTES.md#L58) | Eggs awaiting commit/early next-direction text is historical |
| [Notes Tanker histories](../../TEMPEST-DEVELOPMENT-NOTES.md#L490) | Snapshot reconstruction, no Tanker bullets, inward correction and later no-manual-validation descriptions superseded |
| [Notes all-rim validation](../../TEMPEST-DEVELOPMENT-NOTES.md#L764) | Pending manual/spec-unchanged statements superseded by checkpoint/spec |
| [Spec Egg wording](../../TEMPEST-BEHAVIOR-SPEC.md#L71) | Four-active-Egg cap labelled REFERENCE VERIFIED conflicts with literal six-per-release source loop; current C# cap must still be preserved |
| [Spec completion](../../TEMPEST-BEHAVIOR-SPEC.md#L138) | Generic "applicable outcomes" and all-rim sections are authoritative; avoid resurrecting older kill-only interpretation |
| [Spec history](../../TEMPEST-BEHAVIOR-SPEC.md#L551) | Correctly records final outward cargo manual result; historical report pending statements cannot override it |

Spec broadly distinguishes arcade observations from exact thresholds/timers.
There is no reason to downgrade its recorded manual results merely because
an older implementation report still says "pending."

Additional historical source-analysis corrections: zap recharge is not per
life; source reserve release is not a four-active cap; source closed single-zap
distance expression is not actually shortest-distance arithmetic. These
should be corrected explicitly in any future source summaries.

## 15. Architectural risk assessment

| Question | Assessment |
|---|---|
| 1. Significant Level redesign before progression? | Focused configuration/runtime boundary required; no wholesale rewrite. Definition input and configured total are essential; existing Level can remain runtime |
| 2. TempestGame too Level1-specific? | Extendable, but flags currently drive many phases. New-level lifecycle must be explicit; mixed-wave dispatch needs staged migration |
| 3. Can enemies accept tuning? | Fuseball/Tanker/Spiker already accept speed; Flipper/Pulsar need explicit settings and ordinary Flipper needs new state behavior, not merely constants moved |
| 4. Can Tanker ledger survive wave/composition changes? | Its identity/outcome principle can; slot0/two-Flipper assumptions and ordinary-counter resolvers cannot without change |
| 5. Can all-rim generalize? | Yes: count obligations, exhausted incoming supply, all counted active enemies at rim after collisions. Mixed-type identity representation helper currently accepts only Tankers/Flippers in Tanker mode |
| 6. Can Zapper generalize? | Yes: existing shared timer/selection, explicit next-level recharge, stable mixed-wave order. Preserve current distance and all-rim cancellation policies |
| 7. What breaks first open? | False16th lane and phantom closing rim; player/Egg wrap, invalid last-lane spawns and cargo steps follow. Partial Pulsar reflection is not end-to-end support |
| 8. Renderer load second board? | It reads points dynamically but does not load. Definition input needed; square pivot geometry remains a blocker even once injected |
| 9. Development shortcuts blockers? | Yes if treated as production: ten-total, fixed lanes, Tanker slot0, cargo-as-Flipper, flags-as-capabilities and closed adjacency |
| 10. Refactor before Level2? | Configuration/identity/fresh-init versus reconstruction first. Address square hinge behavior as bounded prerequisite before playable Level2; do not generalize all enemies now |

Other important preservation risks: changing ordinary reverse shot scan to
insertion order would alter Level1; resetting every clock would alter current
pulse behavior; replacing projection blindly could change validated exit;
source all-rim retention would restore delayed score C# intentionally cancels.

## 16. Staged roadmap

Each row is a separately reviewable future checkpoint. No stage is implemented
by this report. Stage order follows first production eligibility, not the order
in which development enemy scenarios were built.

| Stage | Scope and prerequisites | Likely files | Automated/manual/spec/checkpoint boundary |
|---|---|---|---|
| A: configuration groundwork | Explicit Level1 identity/config, definition input, configured supply and named fresh initialization; preserve visible behavior | Levels/Level, LevelDefinition, LevelLoader, LevelSet as needed; TempestGame; tests; related docs | Existing regressions + init/reconstruction contracts; manual five-scenario comparison; no new arcade claims; checkpoint only after identical behavior |
| B: square/canflip isolation | Prerequisite A; oriented hinge/collinear/corner behavior and ordinary below-rim states/tuning in explicit DEVELOPMENT ONLY fixture | Flipper, Level geometry helper, renderer, tests; existing debug infrastructure only if useful | Exact lane/height/pause/shot safety and circle regression; manual square pivot/route observation; spec development-only additions only after validation; no normal advance |
| C: real1->2 | A+B plus approval of Level2 config and reset policies; success-commit install12-unit square Flipper wave, recharge, HUD/input | TempestGame, Levels, Flipper settings/factory, renderer/HUD, Program routing, tests/docs | Kill/all-rim exit advances once; carry/reset matrix; manually clear1 and play2; spec global progression + Flipper2 with evidence labels; checkpoint ends at supported2 |
| D: production3 Tankers | C; multiple carrier original IDs and consistent mixed-wave resolvers, Flipper cargo behavior preserved | Level, TempestGame, Egg/Flipper/Tanker identity/config, renderer HUD/tests | Multiple carriers, shot/natural/zap/death/cargo accounting; manual cargo survival/no mid-tube pursuit; spec composition is reference/project, not automatically arcade; checkpoint supported3 |
| E: production4-7 spikes | D; spike-aware ordinary Flippers, configured spike eligibility and4-7 avoidance/retry | Level, Flipper, Spiker, TempestGame exit, renderer/tests | Growth/return identity, persistence, avoidance151/111/71/31 if approved, retry no re-award; manual spike/Flipper travel; spec hazards/exit policies; stop before unsupported8 |
| F: first open8 | E; shared topology used by every consumer | Level/definition topology, Player/Egg/Flipper/Pulsar/Fuseball/Tanker, zap callsites, renderer/tests | All endpoints/large spinner/cargo/distance/rims/exit; manual Vee boundary and lane presentation; spec open rules only supported evidence; checkpoint supports8 |
| G: direct Fuseballs11 | F; independent hatch eligibility, configurable animation/hurry-up without flag gates | Config resolver, TempestGame dispatch/ledger, Fuseball, renderer/tests | Mixed Flipper/Spiker/Tanker/Fuseball accounting, eligibility boundary and latch; manual wave/acceleration/score; spec direct production composition scoped |
| H: direct Pulsars17 | G; palette band and spike/Tanker eligibility gaps, configurable pulse/pause policies | Resolver, Pulsar, TempestGame clocks/dispatch, renderer palette/tests | 16->17 board repeat/supply drop/no early carriers; pulse visuals/hazards and freeze; manual red-band board and pulse; spec source gates separate from observations |
| I: typed cargo28/33 | H; kind/count-preserving identity and fresh-Egg re-entry, source-height factories, stillborn zap | Tanker, Egg/cargo descriptor, enemies, ledger/renderer/tests | 1/2 children for all three types, delayed zap, repeat deaths, all-rim; manual cargo placement/hazards; spec typed-cargo evidence explicit |
| J: later cycle/rotation/set policy | H or I as needed; all relevant geometry support and explicit approved set scope | Config/catalog, geometry runtime/renderer, TempestGame/tests | 48/49,54/55,80/81,95/wrap boundaries; rotations/midpoint/hinges; manual later visuals; no source off-by-one copy; separately bounded checkpoint |
| K: bonus/audio/entry presentation | Lifecycle commit from C; exact policies require approval, may run independently of later enemy stages | ScoreSystem, Player/session reward state, AudioSystem, renderer, entry states/tests | Exactly-once successful-travel bonus, threshold behavior, retry/restart isolation; manual presentation/audio; spec distinguishes economy/presentation decisions |

Do not enable progression into a not-yet-supported next configuration. A
deliberate supported-content endpoint with a clear message is safer than an
implicit Flipper-only fallback. Do not call a placeholder square/straight-wave
"real Level2" while its defining ordinary behavior remains absent.

Rollback means returning to the last user-approved known-good checkpoint by
the user's normal workflow, not agent resets, commits or automated reversions.
Each implementation should be separately reviewed and manually validated.

## 17. Exactly one recommended next milestone

**A: behavior-preserving Level1 configuration and initialization groundwork.**

Included:

- Explicit zero-based/one-based Level1 identity independent of geometry comment.
- Level accepts the existing Level1 definition/config rather than constructing
  it internally; baseline supply10 represented as runtime/config data.
- Fresh wave initialization has one owner, distinct from life reconstruction.
- Reuse Level as runtime, existing LevelDefinition/LevelSet as data; avoid
  adding another runtime hierarchy.
- Rebind per-level hazard events safely; preserve existing entity ownership.
- Preserve all scenario factories, flags, shot ordering, clocks, projection,
  replay/Game Over behavior and exact Level1 visible outcomes.
- Establish lane-count/vertex-count contract without claiming endpoint support.

Excluded: no normal next-level transition, square playable content, canflip
implementation, bulk levelset import requirement, generalized multi-Tanker
ledger, new palette, bonus/audio/entry presentation or bullet promotion.

Why this and not direct1->2: it removes hardwired configuration without
coupling geometry, new Flipper states, accounting, clocks and entry to the
same patch. It needs no unimplemented future enemy machinery and does not
ship intentionally incomplete Level2.

No-flag and scenario entrypoints remain behavior-identical. This milestone is
architectural groundwork, **not real progression and not arcade validation**.

## 18. Automated-test strategy

No tests were executed in this analysis. Historical all-rim implementation
records330 passing tests; that is a prior result, not a new validation claim.

Existing anchors:

- [ten-unit initialization](../../tests/Tempest.Game.Tests/SimulationTests.cs#L5155)
- [fiftieth-tick reserve release](../../tests/Tempest.Game.Tests/SimulationTests.cs#L5216)
- [accounting across deaths/restart](../../tests/Tempest.Game.Tests/SimulationTests.cs#L5275)
- [Complete replay score/lives](../../tests/Tempest.Game.Tests/SimulationTests.cs#L5045)
- [open Pulsar isolation](../../tests/Tempest.Game.Tests/SimulationTests.cs#L1024)
- [spike reverse/Game Over](../../tests/Tempest.Game.Tests/SimulationTests.cs#L4029)
- [zap death freeze](../../tests/Tempest.Game.Tests/SuperZapperTests.cs#L578)
- [zap replay/restart](../../tests/Tempest.Game.Tests/SuperZapperTests.cs#L601)

Tests are C# invariants/DEVELOPMENT ONLY fixtures, not arcade oracles.

### Required for recommended A

1. Default and explicit Level1 initialization produce the same16 ordered
   points, continuous flag, center400, offset-80, identity0/display1.
2. Four Eggs/six reserve/ten obligations; release and approach tick thresholds
   unchanged; deterministic spawn-lane/direction sequence unchanged.
3. No-flag Flippers only; each scenario/flag combination still creates its
   established composition and bullet policy; conflicts still reject.
4. Definition/config replacement for fresh same-wave replay is coherent,
   old SpikeCreated subscriptions do not append duplicate/stale targets.
5. Fresh initialization zeroes killed/rim-cleared and appropriate identity
   ledgers, restores initial hazards/Eggs, clears queued cargo/runtime timers.
6. Death preserves score, resolved outcomes, charges, hurry-up and hazards;
   unresolved units reconstruct as fresh Eggs with stable local IDs.
7. Replay preserves score/lives, resets charges2/hurry-up/ledger/projection;
   Game Over restart resets score/starting lives/initial identity.
8. Pulse phase/random lifetime and Fuseball animation resets retain existing
   semantics rather than being changed by initialization extraction.
9. All-rim still cancels pending kills without score, retains flash through
   exit, preserves spike-retry terminal outcomes, and resets on replay.
10. Projection K70/Z5 and valid player lane7/offset0.5 match baseline.
11. Large spinner/wrap, shots, direct rim arrival and completed-pivot safety
    regressions unchanged.
12. Geometry-only topology checks establish closed16/open15 lanes; they must
    not claim open player/Egg/renderer support is implemented.
13. Complete remains terminal: no hidden increment or unsupported next wave.

Scaling/actual advancement are **not applicable to A's gameplay**. Do not add
a fake production Level2 test to satisfy a checklist. A configured-total
initialization test may use a DEVELOPMENT ONLY fixture with sufficient total
and valid lanes, but it must not imply12-unit Level2 is reachable.

### Gates for future real progression C

- Final-shot clear and all-rim clear each lead to exactly one success commit.
- Before successful travel identity stays1; a failed spike exit never advances.
- After success: internal1/display2/board1/palette0, approved12-unit supply,
  four initial Eggs, correct Flipper-only eligibility/canflip and player lane.
- Earned score/lives preserved; charges2; outcomes/new IDs cleared; no old
  Eggs/bullets/spikes/cargo/timers; hurry-up false; projection and clocks follow
  explicit approved policy.
- Repeated Complete/entry visits cannot reinitialize/reward twice.
- Game Over restart returns initial selection; development replay does not
  accidentally enter production progression.
- Later matrices pin eligibility gaps, capped supply and board-cycle drops.

Use existing headless runner and focused selectors first; run the full suite
and build for significant cross-system changes. Rendering callbacks prove
submissions/alignment, not framebuffer fidelity. No new test tooling required.

## 19. Manual-validation strategy

### Recommended groundwork A

Neal should run normal Level1 and all four enemy scenarios with existing
life overrides; additionally run relevant bullet combinations.

Observe no change in:

- Physical spinner direction/sensitivity, cursor capture, fire/zap buttons.
- Tube/player/shot/Egg geometry, colors and entry pacing.
- Flipper straight climb and full visible pivot shootability/safety.
- Cargo children surviving on distinct non-Tanker lanes and not pursuing
  below rim.
- Fuseball motion, score display, late-wave acceleration and rim danger.
- Pulsar glyph/pulse, affected near-rim omission, lane hazard and no vertical
  hurry-up speed increase.
- Death/re-entry, all-rim disappearance/exit, spike persistence/reverse/retry,
  Complete replay and Game Over restart.

This is regression observation, not verification of new arcade progression.

### Later Level2 manual gate

Compare square orientation/scale/lane layout, player travel, straight-side
and corner pivot hinges, initial below-rim route/pause, nonlethal mid-tube
shootability, rim pursuit/pivot safety, wave rhythm,1->2 exit/entry, resource
presentation and valid controls. Compare Atari directly rather than treating
Incitti output as the oracle.

### What observation cannot establish without measurement

Exact supply/tick constants, integer speed steps, random distributions, pause
values, pending zap expiry order, numeric rim cutoff, ID conservation and
once-only commit behavior require automated assertions or explicit measurement.
Source-derived constants remain REFERENCE VERIFIED unless separately measured;
hidden accounting is PROJECT DECISION. Seeing a wave complete cannot validate
all of those internals.

## 20. Behavior-spec impact

No behavior specification was edited.

For A, no visible behavior claim should change. Related README/development
architecture descriptions may be updated during that future implementation;
spec only needs a narrowly labelled history entry if desired after validation.

Future sections affected:

- Section2 global behavior: supported production identity/config, advancement,
  resources, death versus retry versus fresh-level reset, entry/exit policy.
- Section3 Flippers: ordinary later-level mid-tube behavior separate from
  Level1 and cargo; retain ARCADE VERIFIED pivot safety.
- Sections4-8: approved production bullet/enemy eligibility, tuning, typed
  cargo and spike/late-wave interactions, with source gates explicitly labelled.
- Section9 rendering: square/open topology, palette, entry/rotation observations.
- Section10 development scenarios: remain artificial and independently replayable.
- Section11 uncertainties: remove only implemented/validated deferrals; retain
  exact arcade gate/tuning/clock/economy uncertainty.
- Section12 history: distinguish automated invariants, literal reference
  comparisons and Neal's new observations.

Do not convert a source matrix or a passing progression test into ARCADE
VERIFIED composition, geometry or tuning.

## 21. Explicit out-of-scope items

This analysis implements nothing. Specifically:

- No Level2, canflip, real advancement, loading/topology implementation.
- No gameplay/tests/README/development-notes/behavior-spec/config edits.
- No Git staging, commit, reset, checkout, branch change or push.
- No builds/tests/gameplay launches or Windows policy changes.
- No bullet promotion, enemy tuning, bonus/extra-life/audio/presentation changes.
- No broad refactor, generic entity engine, or speculative arcade correction.
- No repair of suspected converging rim Flippers or unrelated pre-existing bugs.
- No automatic adoption of alternate/editor sets, source rollover defects,
  source distance defect, source all-rim retention, or source rim-angle death.

Only the exact requested prompt and this analysis are saved under ignored
AI-Logs. Their requested dates/paths are retained.

## 22. Remaining arcade uncertainties and conclusion

Unverified against Atari:

- Actual Level2 canflip eligibility, route, initial pause, exact square geometry,
  numeric pacing/supply and enemy-bullet promotion.
- Incitti's repeated eligibility gaps, later composition/cargo counts and
  stepped tuning as arcade equivalents.
- Exact first-open lane/end/cargo behavior and irregular/reflex pivot geometry.
- Source set/palette/rotation cycles and maximum/wrap correspondence.
- Completion-bonus/start-selection interpretation and extra-life thresholds.
- Entry duration/input and oscillator/animation/flash carry across levels.
- Exact direct climbing rim-arrival collision (separate from verified pivot
  safety), simultaneous collision/all-rim timing, pending-zap timing and spike
  retry accounting.
- Fractional Spiker random coercion and source continuous versus integer seams.

**Significant refactoring before Level2?** A focused configuration/runtime and
fresh-initialization boundary is required, plus bounded square/canflip
geometry/state work before a playable Level2. A wholesale architecture or
enemy rewrite is not justified. Dynamic mixed-wave carrier accounting is
required before Level3, not as a speculative prerequisite to groundwork.

**Have we built into a corner?** No. The established headless simulation,
read-only renderer access, entity state machines, explicit outcomes and
identity conservation provide a recoverable foundation. We are at meaningful
risk only if development flags, ten-unit constants, closed adjacency and
single-carrier assumptions are promoted directly into production progression.

**GO / NO-GO:** **GO for the single recommended behavior-preserving
configuration groundwork milestone. NO-GO for jumping directly to real Level2
or unrestricted multi-level progression at this checkpoint.** After groundwork,
square/canflip isolation and explicit lifecycle/config acceptance, a bounded
Level1->2 milestone becomes a reasonable next production step.
