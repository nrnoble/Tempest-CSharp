# Level 3 Tanker progression analysis

Analysis checkpoint: `f6ac233 Implement Level 1 to Level 2 progression`
Scope: natural Level 1 -> Level 2 -> Level 3 progression with the Incitti Level 3 board and its Flipper/Tanker wave.
Disposition: analysis only; no gameplay, tests, project configuration, behavior specification, or Git state was changed.

## 1. Baseline Git and maintenance result

At the start:

```text
git status --short
(empty)

git log -1 --oneline
f6ac233 Implement Level 1 to Level 2 progression

git diff --check
(no output; exit 0)

git status --short -- Reference/incitti
(empty)
```

HEAD and the clean-worktree requirement matched. No full build or test suite
was run: this is analysis-only, the baseline is clean, and no conclusion below
depends on current execution results. Focused geometry and lifecycle tests are
specified for the future implementation checkpoints.

## 2. Executive recommendation

**Natural Level 3 is not a safe single milestone.** The current direct Tanker
scenario is a controlled Level 1 diagnostic, not a production mixed-wave
model. Natural Level 3 requires stable identities for all original Eggs,
random per-Egg Flipper/Tanker selection, up to six simultaneously active
Tankers, separately identified cargo children, and exact reconstruction and
completion accounting. At the same time, Level 3 has a different concave
board whose straight runs expose a zero-angle Tanker-cargo pivot defect if
the current legacy angle helper is reused.

**Exactly one next implementation milestone: C1, isolated Level 3 board and
geometry groundwork.** It should add and test the Level 3 board data and a
generalized endpoint-pivot path (including cargo), while leaving production
composition, Tanker accounting, Tanker firing promotion, and natural
progression unchanged. Use a plainly **DEVELOPMENT ONLY** geometry access
mode for manual rendering checks; do not make a Flipper-only or fixed-slot
wave masquerade as production Level 3.

Then validate Tanker firing separately; implement the production mixed wave
behind terminal direct access only after the geometry and firing gates; enable
Level 2 -> Level 3 only after that complete direct Level 3 path passes its
accounting, death, Super Zapper, all-rim, and manual checks.

**GO** for C1 from the verified clean checkpoint. **NO-GO** for natural Level
3 integration or progression at this checkpoint.

## 3. Evidence model and authority

This report distinguishes:

- **ARCADE VERIFIED** — Neal's stated direct arcade observations and already
  documented behavior-spec observations. This does not claim hidden internal
  state or exact timing.
- **REFERENCE VERIFIED** — literal Incitti source/data behavior. It describes
  Incitti, not necessarily Atari Tempest.
- **PROJECT DECISION** — current C# behavior, or a proposed recreation policy
  where reference behavior is not a safe identity/accounting contract.
- **DEVELOPMENT ONLY** — controlled diagnostic behavior, not natural wave
  composition or production progression.

Current authority is the clean `f6ac233` checkpoint and
`TEMPEST-BEHAVIOR-SPEC.md`; older reports are supporting history only.
Passing C# tests prove C# invariants, not arcade fidelity. Direct arcade
observations take precedence over a conflicting Incitti detail.

## 4. Exact Incitti displayed-Level-3 configuration

The source starts `current_level` at zero and displays `current_level + 1`.
After Level 2, `current_level` is `2`, `current_board` is `current_level Mod
16` (`2`), and `current_color` is `current_level / 16` (`0`). The board data
is row `Level 3 - plus` in `Reference/incitti/levelsets/levelset1.txt`.
`SetUpLevel` reads that row directly.

| Property | Level 3 reference value | Evidence / C# boundary |
|---|---|---|
| Internal level identity | `current_level = 2` (zero-based) | Source display is `current_level + 1`. |
| Displayed level | `3` | Source HUD uses `current_level + 1`. |
| Board index | `2` in set 0 | `current_board = current_level Mod 16`. |
| Palette/color index | `0` | `current_color = current_level / 16`; C# `PaletteIndex` should be 0. |
| Board row/name | `Level 3 - plus` | `levelset1.txt`, third board row after the set header. |
| Closed/open | Closed (`1`) | First row value; source `SetUpLevel` closes the last edge when true. |
| Ordered points | 16 points, listed in §5 | Literal board row; not inferred from a sketch. |
| Vertices / playable lanes | 16 / 16 | Closed board; `e_cnt` is 16. |
| `CenterY` / `CCenterY` | 400 | Level 3 row; no Level 3-specific formula at steady state. |
| `YOffset` | -80 | Level 3 row. |
| Depth | 400 | `Level.depth` default. |
| Position | 50 at ready/play; 1500 during source `LEVEL_BEGIN` zoom-in | `Level.position` starts at 1500 and decrements to 50. Current C# configuration uses the steady-state value 50 and omits the source intro. |
| Player starting lane | 7 | Source calls `MainPlayer.SetEdge(7)` at level selection and successful level transition. C# uses lane 7 and fractional lane position 0.5. |
| Initial enemies / reserve | 4 active Eggs / 11 reserve, 15 original units total | `eggsleft = 10 + lv/2 + board*2 = 15` for `lv=2`, `board=2`; four are created initially. |
| `maxeggs` | 15 | Set to the original `eggsleft` total. It is not an active-Egg cap. |
| Reserve release | Every 50 `globalclock` ticks, a loop attempts up to six Egg releases, decrementing `eggsleft` per Egg; at most 11 reserve units are released over two batches | `enemyreleaserate=50`; the source loop is `For a = 0 To 5`. It does not match C#'s one-Egg-per-50-tick and four-active-Egg policy. |
| Initial Egg lanes | Independently random `Rand(0,15)` | A closed 16-lane board. The current C# lane sequence is deterministic project policy, not this source behavior. |
| Hatch gating | At most two in-transit Eggs; only when `enemiesleft < 9`; an Egg checks on its 8th update | Source `hatchingeggs < 2`, `enemiesleft < 9`, and `cnt > 7`. |
| Hatch selection rate | `hatchrate = lv/8 = 0` at `lv=2` | In addition to the near-end condition described in §6. |
| Spikes | None | `current_board > 2 Or lv > 47` is false for `(2,2)`. |
| Flippers / Tankers | Flippers enabled; Tankers enabled | Flippers are set true; tanker gate is `current_board > 1 Or lv > 17`. |
| Fuseballs / Pulsars | Neither enabled | Fuseball gate is false (`board > 9 Or lv > 20`); Pulsar gate is false (`lv > 15`). |
| Tanker cargo selection | Two Flippers only | Fuseball- and Pulsar-cargo Tankers are gated at `lv > 31` and `lv > 26`; neither is enabled at Level 3. |
| Super Zapper | Two charges on new-level setup | Literal source resets `superzapper = 2`; current C# fresh-wave policy also resets to two. |

`flipflipspeed`, `flippausedelay`, and `flippausedelayrim` are assigned from
`8+lv/5`, `21-lv/5`, and `24-lv/5`; `flipclimbspeed` is 2. The existing
Level 2 configuration and source-reading policy use the integer-valued
`8/21/24` tuning at these early levels. Level 3 has `lv=2`; source/global
numeric conversion and its exact timing remain reference-derived rather than
arcade-measured. C# should start from the existing configured Flipper values
and preserve the B2 Level 2 path, not alter it globally.

The Level 3 Tanker speed formula is `1.75 + lv/20`; for `lv=2`, Incitti's
floating Tanker field gives `1.85`. Tankers start at height 10 and release
after `height > depth - 15` (`>385`). Exact gameplay cadence is not arcade
validated.

Incitti creates four initial Eggs in `SetUpEnemies`; later Egg releases are
randomized. The C# Level 1/2 policy instead uses a configured deterministic
lane sequence, active Egg cap 4, and one reserve release per 50 ticks. Those
are **PROJECT DECISIONS** and must not be reported as Level 3 reference facts.
Level 3's source start lane is known; fractional Blaster position and exact
transition pose remain project/arcade validation concerns.

## 5. Level 3 board and geometry analysis

The exact ordered coordinates from the `Level 3 - plus` row are:

```text
0  (  98,   98)    1  (  98,  197)
2  (   0,  197)    3  ( -98,  197)
4  ( -98,   98)    5  (-197,   98)
6  (-197,    0)    7  (-197,  -98)
8  ( -98,  -98)    9  ( -98, -197)
10 (   0, -197)   11  (  98, -197)
12 (  98,  -98)   13  ( 197,  -98)
14 ( 197,    0)   15  ( 197,   98)
```

Lane `i` joins point `i` to `(i+1) Mod 16`; lane 15 closes to lane 0. All 16
vertices are distinct, and every lane has nonzero length (98 or 99). There
are no repeated coordinates or zero-length lanes. Segment headings are axis
aligned. Consecutive lane headings are either straight/collinear or turn 90
degrees; the ordered path is a concave, closed plus outline. It has straight
runs at lane transitions 1->2, 5->6, 9->10, and 13->14. The other adjacent
transitions, including wrap 15->0, are right-angle corners. No adjacent
transition is geometrically zero-length or a zero-angle endpoint ray.

This is not a regular square. The existing legacy `CalculateEdgeTurnAngle`
compares lane heading vectors and returns 0 degrees on straight runs. The
validated B1/B2 endpoint-hinge path instead rotates the free endpoint around
the shared vertex; a straight run means opposing endpoint rays and a 180
degree sweep, while a corner yields a 90 degree sweep. That is the safer
representation for the plus outline, but current production code labels and
guards that helper as `SquareEndpointHinge`, and its tests only validate the
square. It is not yet validated for this concave board or against Atari.

There is a specific cargo defect to address before mixing Tankers into Level
3: `Flipper.CreateTankerCargo` initializes its mandatory pivot with
`CalculateEdgeTurnAngle`, not the configured `CalculateFlipperTurnAngle`.
On a Level 3 straight-run transition that can set `TotalTurnAngle` to zero;
the endpoint renderer/pivot path can then have an invalid progress ratio or
an immediate, visually wrong completion. Ordinary Flipper pivots already
dispatch through the configured geometry helper. Generalize/extract the
endpoint-hinge operation and route ordinary and cargo pivots through the
same geometry-specific calculation. Do not globally replace the validated
Level 1 legacy or Level 2 square path.

Geometry checkpoint coverage must enumerate both adjacent directions for all
16 lanes, including 15<->0, and cover:

- Player rim movement, Eggs, and shots on the closed 16-lane topology.
- Ordinary canflip Flippers on straight, corner, and wrap transitions.
- Rim pivots and collision boundaries without altering B2 behavior.
- Tanker glyph projection and cargo's initial outward pivots on straight and
  corner sections. Cargo remains non-canflip below the rim and retains its
  direction there.
- Super Zapper flash and all projected endpoints remaining finite.
- No zero vector passed to `Vector2.Normalize`, zero total-turn division,
  NaN, or infinity. Projection depth remains positive at all Level3 enemy
  heights with the existing projection offset.

The current B1 endpoint method is mathematically capable of joining the
existing nonzero adjacent lane endpoints, but its square-specific assumptions
and cargo bypass make this a real isolated checkpoint, not a reason to rewrite
all validated geometry. Manual arcade comparison remains required for visual
hinge arcs, especially the concave corners.

## 6. Literal Level 3 hatch composition

Every Egg independently calls `PickEnemy` when it hatches. At Level 3,
Flippers and Tankers are eligible; spinner, Pulsar, and Fuseball selections
are rejected and retried because their level flags are false. In source
selection order, candidate values are spinner 0, Flipper 1, Pulsar 2,
Fuseball 3, Tanker 4:

1. `tnk` is 4 while `tanker.numtankers < 6`, so `Rand(-6,4)` can select any
   candidate after negative values are clamped to 0.
2. With six Tankers already active, `tnk` becomes 3 and the range ends at 3,
   so another Tanker cannot be selected until one of the active Tankers
   leaves. This is a concurrent cap, not a six-Tankers-per-wave cap.
3. If `enemiesleft + egg_list.Count() < 4` or the spinner count exceeds 3,
   the source instead draws `Rand(1,tnk)`, suppressing spinners near the end.
   At Level 3 no spinner is eligible in the first place.
4. An Egg enters approach only when `enemiesleft < 9`, fewer than two Eggs
   are already in transit, and the source random/near-end condition succeeds.
   It checks this on its roaming update cadence before eventually hatching.
5. Once Tanker is selected, its nested random cargo selection retries choices
   until an eligible cargo type is found. At Level 3 only the two-Flipper
   cargo branch is eligible.

Thus normal Level 3 source composition can produce zero, one, or multiple
Tankers. Up to six can coexist; there is no six-total cap, and up to 15
original Egg identities can hatch as Tankers in one wave. Each carrier adds
two child obligations only when cargo materializes. The number of Tankers is
not deterministic from the level number alone; it depends on hatch selections,
current Tanker count, active enemy count, Eggs, and random draws. Incitti does
not give every Egg a persistent ID.

The current C# `--tanker-test` is not this behavior: original slot 0 is
deterministically the only Tanker, other slots are Flippers, and the mode uses
Level 1 configuration. Do not reuse it as normal Level 3 or invent a fixed
production Tanker slot/composition. The implementation needs source-shaped
random selection with deterministic test seams, while keeping production
selection unforced.

## 7. Current C# Tanker and accounting map

Current normal Level 1/2 accounting is the configured-supply counter in
`Level`: `EnemiesDestroyed`, `EnemiesRimCleared`, reserve, Eggs, and active
units. It is sufficient for Flipper-only waves because every ordinary unit
resolves as one configured unit.

Current `--tanker-test` uses a separate scenario-specific ledger in
`TempestGame`:

| Obligation/outcome | Current implementation |
|---|---|
| Original units | IDs in `unresolvedOriginalSlots`; IDs 0..supply-1 are seeded for Tanker mode. |
| Destroyed originals | `destroyedOriginalSlots`; a shot/zap is score-bearing. |
| Escaped originals | `escapedOriginalSlots`; natural Tanker release is not a kill. |
| Rim-cleared originals | `rimClearedOriginalSlots`; no kill score. |
| Egg identity | Tanker-mode Egg carries an original slot ID or cargo ID; ordinary waves do not. |
| Flipper | A development original slot hatches as a Flipper and carries its slot ID. |
| Tanker | Only slot 0 can hatch as Tanker; its original slot is resolved on shot, zap, natural release, or all-rim clearing. |
| Latent cargo | Two are on a Tanker; a special fallback adds two latent units for unresolved slot 0 before its Tanker object exists. |
| Materialized cargo | `nextCargoUnitId` assigns separate monotonically increasing IDs; active, pending, killed, and rim-cleared cargo are tracked separately. |
| Shot carrier | Carrier gets 200 points and is destroyed; two live children are created but wait one update before activation. |
| Natural release | Carrier escapes with no Tanker kill score; two live children appear. |
| Super Zapper carrier | On delayed expiry carrier gets 200 points; two children appear with separate delayed Flipper resolution timers. |
| Stillborn cargo | The two identities are materialized, then each can resolve for the normal 150-point Flipper score. |
| Death | Unresolved original and cargo IDs are rebuilt as fresh Eggs; resolved outcomes remain resolved, and child zap timers are discarded. |
| Completion | Tanker mode requires all original and materialized cargo IDs resolved, zero reserve/Eggs, and no active Tankers, Flippers, or Spikers. |

The controlled ledger is scenario-specific for concrete reasons: the
`tankerTest` boolean selects it; `Level.TankerSlotId` is always 0; the hatch
factory recognizes only slot 0 as Tanker; latent cargo fallback is hard-coded
to slot 0; the tanker complete/all-rim branches are gated on `tankerTest`;
and the level uses original slots plus extra cargo rather than a production
mixed wave. Its current seed and `TankerAccountingTotal` do not define a
general production identity contract.

The existing structures do provide useful prior art: identity is carried by
Egg through hatch; the carrier's original slot is separate from child IDs;
`nextCargoUnitId` does not reset on ordinary death; and death gathers
unresolved materialized cargo before discarding runtime objects. Preserve
these properties, but remove the one-slot and scenario-only assumptions for
Level 3.

## 8. Production mixed-wave accounting requirements

The production wave begins with 15 original unit identities. At any time,
each original identity is exactly one of: reserve, Egg, active Flipper,
active Tanker, destroyed, escaped, or rim-cleared. A Tanker can own two latent
child obligations. On release, the children receive unique stable IDs and
move from latent to materialized, not from nonexistent to counted. For each
carrier, conservation must hold:

```text
original identity:
  unresolved in reserve/Egg/active OR exactly one of destroyed/escaped/rim-cleared

each cargo identity:
  latent under one unresolved carrier OR materialized unresolved OR
  exactly one of killed/rim-cleared
```

No source unit can be duplicated or silently lost when a Tanker is shot,
escapes, is zap-resolved, is rim-cleared, or life reconstruction occurs.
`EnemiesDestroyed`-style kill scoring must not absorb natural escape or
all-rim resolution. Live cargo increases the unresolved population only once
when materialized; before release it is represented by its carrier. Completion
must block on reserve/active Eggs, a carrier with latent cargo, queued cargo
identity, any active child, and any pending zap-resolution obligation.

Required behavior by path:

1. **Ordinary unit:** preserve the original ID from reserve to Egg to Flipper
   or Tanker and then to exactly one terminal outcome.
2. **Tanker release:** terminate the carrier identity as escaped with no kill
   points; transfer its two uniquely owned latent child identities into two
   active Flippers. The arcade-validated outward route remains unchanged.
3. **Shot Tanker:** terminate only the carrier as destroyed, score 200 once,
   materialize two live children, and do not let the shot also affect a child.
4. **Super Zapper Tanker:** terminate the carrier as destroyed and score 200
   on its own delayed timer; create two stillborn children with child IDs and
   their own delayed resolutions; each Flipper score/accounting outcome occurs
   once.
5. **Children:** killed child and rim-cleared child remain distinct terminal
   dispositions; a child is never scored as a carrier and never reused as an
   original slot.
6. **Death:** rebuild unresolved obligations, not resolved outcomes or stale
   objects. Do not turn a Tanker's two latent children into independent Eggs
   while also rebuilding the carrier.
7. **Wave completion:** no completion while any production obligation is
   unrepresented, regardless of type or whether the child is latent, queued,
   active, or pending a zap.

Do not fold the model into a single counter: stable carrier and child identity
is needed for repeated death, multiple zap timers, exact-once scoring, and
all-rim set equality. Do not introduce a general ECS. Extend the existing
Level/Egg identity and the narrow Tanker/cargo ledgers with the smallest
Level-3 mixed-unit policy.

## 9. Identity model recommendation

Use **A: configured original wave-slot IDs for every original Egg**, with the
existing separate child identity concept. This is also the narrowest
generalization of the useful part of current model C; it does not require
migrating normal Level 1/2 accounting.

- Assign IDs 0..14 to the 15 original production obligations at fresh Level 3
  setup. Reserve and Egg entries retain those IDs.
- On the first hatch, commit the selected unit type to that identity. A
  Flipper or Tanker retains the same original ID. An unhatched Egg has not yet
  selected a type.
- A Tanker owns two latent child identities. Derive them from the carrier
  identity plus child ordinals, or use a monotonic child-ID namespace that is
  never reset on life reconstruction. Keep original IDs and child IDs
  distinguishable in the type system/ledger; do not rely on equal numeric IDs
  being interchangeable.
- On natural release, shot destruction, or zap expiry, transfer the child's
  identity onto the materialized Flipper. Re-entry Eggs carry that same child
  identity.
- Record exactly one terminal state for each ID. Death reconstruction keeps
  selected type and unresolved child ownership; fresh-level initialization
  creates a new, empty ledger.

This makes identity/type commitment an explicit **PROJECT DECISION**. Incitti's
Egg source chooses an enemy at each hatch but does not persist original IDs or
selected types through life conversion. The project should retain a Tanker's
selected type and latent cargo across a life loss, otherwise a carrier could
become an ordinary Flipper and lose or duplicate its two obligations. An
unhatched Egg may select when it next hatches. If arcade evidence later
establishes a different type-reselection rule, update the policy with an
identity-preserving transition rather than dropping obligations.

Levels 1 and 2 should remain on their established simple configured-supply
accounting. A Level-3 mixed-wave branch/policy is safer than migrating tested
earlier waves to a generalized ledger. Shared small helpers may be extracted
only where they preserve existing outcomes and do not change their accounting.

## 10. Tanker tuning and configuration

Incitti Level 3 Tankers start at height 10, climb by 1.85, retain their selected
lane while climbing, and release after height exceeds 385. Tanker shot
collision is on the same lane when the shot height is below the Tanker's
height; the Tanker does not kill the player directly. Source Tanker shot
destruction scores 200; natural cargo release scores no Tanker kill. Tanker
glyph is a vector diamond/lattice at the lane midpoint.

The current C# development Tanker already uses height 10, fixed climb 1.85,
threshold 385, fixed lane, same-lane/below-Tanker shot hit, 200-point
destruction, no player collision, and a projected magenta lattice glyph.
These are suitable Level 3 starting values as **REFERENCE VERIFIED** /
project-derived behavior, not arcade-validated constants. Add them to
Tanker/Level configuration rather than silently relying on development
constants if Level 3 starts using the production entity.

The source Tanker creates two neighboring cargo Flippers and sets their
directions inward toward the carrier's old lane. Do not copy that conflicting
route: Neal directly observed two live children in distinct lanes other than
the former Tanker lane, and the current deterministic outward route is the
**PROJECT DECISION** matching that result. Preserve their fixed direction
below the rim; do not dynamically retarget them to the Blaster. Exact
intermediate route and pause timing remain unknown.

Development approximations that must not silently become arcade facts:

- C# cargo direction is deterministic outward; source direction is random and
  inward.
- C# Tanker bullet activation is currently opt-in. Current development
  ammunition/timer/chance are source-derived but not arcade-validated.
- Cargo below-rim pivot uses a separate helper and currently does not use the
  configured ordinary Flipper pivot-angle method.
- Tanker geometry is Level 1 in `--tanker-test`, not the plus board.
- C# fixed projectile collision and update ordering are established project
  behavior, not full arcade collision equivalence.

## 11. Tanker bullet policy

Neal's direct arcade observation is qualitative but material: Tankers fire
individual shots at intervals, and at least two shots can occur before the
Tanker reaches the rim/release area. The current C# Tanker is silent with
`--tanker-test` alone and is armed only by combining
`--tanker-test --enemy-bullets-test`. One observed C# Tanker fired one shot
before release. The exact present ammunition, initial delay, chance, and
height cutoff are Incitti-derived, not arcade validated.

**Recommendation: D, validate the isolated Tanker firing policy before
production integration.** Do not silently enable the current source-derived
random policy in Level 3, and do not invent exact arcade ammunition or
cadence. Keep the existing combined development flag as the comparison
fixture. Neal should compare C# against the qualitative arcade observation
and decide whether the existing policy is acceptable or requires a revised
policy. A later manual sign-off can authorize promotion; it need not claim an
exact shot interval.

Natural Level 3 with silent Tankers could be an honest temporary checkpoint
only if explicitly scoped as incomplete and documented as such. It should
not be the final natural Level 3 rollout because it omits an observed
Tanker's central behavior. The preferred sequence avoids needing that
temporary production compromise: resolve firing separately before enabling
natural mixed Tankers.

## 12. Flipper bullet policy

Incitti creates Flipper ammunition and can fire bullets, but that is not
sufficient reason to change current C# launch policy. Keep
`--enemy-bullets-test` orthogonal and opt-in for natural Level 3 unless Neal
provides direct arcade evidence and approves a separate promotion. Preserve
the existing Level 1/2 no-bullet behavior and do not couple Flipper bullets
to the new Tanker firing decision.

## 13. Super Zapper and multiple carriers

The current zapper has two charges, a first all-target activation with
staggered 4, 6, 8, ... accepted-target delays, then a second single-target
selection. `Enemy` stores a timer per runtime target; `SuperZapperSystem`
does not own persistent enemy IDs. Tankers already accept zap timers. The
single-Tanker development scenario validates the observable zap path but does
not establish simultaneous production-carrier safety.

The object/timer mechanism can schedule multiple Tankers, but production
accounting cannot yet safely resolve them: Tanker identity is slot 0 only,
the latent fallback assumes that slot, and normal Egg composition carries no
identity. A mixed-wave implementation must prove:

- Multiple pending Tankers receive independent staggered timers and resolve
  the correct original IDs.
- Carriers expiring on different ticks each materialize two unique children;
  no child ID collides with another carrier or an original slot.
- Child timers begin according to the existing same-tick policy (cargo added
  after that tick's Flipper update), and child resolution scores once.
- A shot before timer advancement pre-empts the pending zap exactly as current
  update ordering intends; no later 200-point zap score is awarded for a
  removed carrier.
- Death during carrier or child timers clears runtime timers but reconstructs
  all unresolved identities; a resolved carrier remains resolved.
- All-rim removal cancels a pending score by removing the timer target, while
  latent or queued cargo still blocks completion.
- First all-target and second single-target uses work with several carriers,
  independent of carrier insertion order. Existing type-priority/target
  selection remains the current project policy.

For an all-zap, carrier resolution scores 200 once per Tanker; stillborn
Flippers remain separate child obligations and resolve later. Do not assume
the zapper needs a universal identity-aware scheduler; the essential stable
identity belongs in the Level-3 unit ledger and its Tanker/cargo ownership.

## 14. All-rim completion

The broad behavior Neal directly observed is that, after incoming Eggs are
exhausted, the remaining enemies reaching the rim is sufficient for them to
disappear and exit to begin; kills are not required. Current C# preserves
kill, natural escape, and rim-clear as distinct outcomes. Its exact height
threshold, update order, pending-zap disposition, and internal identity rules
are project/reference policies, not directly observed arcade details.

The Level 3 predicate must require:

- Zero unresolved reserve and active Eggs, including pending re-entry cargo
  Eggs.
- No below-rim active Flipper or Tanker; each qualifying active entity is at
  or beyond the established rim height.
- Latent cargo remains represented by its unresolved carrier and cannot be
  mistaken for resolved children. Released below-rim cargo blocks the clear.
- Pending zap targets remain active obligations until either their timer
  resolves or the established all-rim path removes them.
- Identity-set equality: every unresolved original/cargo identity is
  represented exactly once; there is no stale or duplicated Tanker/cargo ID.
- Rim-cleared Tankers and Flippers record non-kill outcomes, award no kill
  score, and cannot later award delayed zap score.
- Shot-destroyed, naturally released, and rim-cleared Tankers remain separate
  outcomes; cargo is created/resolved exactly once for each applicable path.
- Completion begins the existing standard exit and commits progression only
  after successful travel.

The source Level 3 has no Spikes, so there is no Spiker avoidance/reverse
exit in normal progression. Do not import Spiker retry behavior.

## 15. Death and reconstruction

Use the existing project rule: ordinary life loss discards runtime enemy
objects and pending enemy timers, retains score and resolved outcomes, and
reconstructs unresolved units as fresh Eggs. Apply it by stable identity:

| Life-loss point | Required reconstruction |
|---|---|
| Before a Tanker releases | Restore the same unresolved Tanker original ID and its two latent cargo obligations; do not create duplicate child Eggs. |
| After natural release | Keep the escaped carrier resolved; restore only unresolved live cargo child IDs. |
| After shot release | Keep the destroyed carrier/200-point outcome resolved; restore only unresolved live cargo IDs. |
| After zap carrier resolution | Keep carrier destruction and its score resolved. Restore each unresolved materialized child once; preserve killed children as killed. The existing C# policy rebuilds unresolved child IDs as fresh Eggs and drops their old zap timers. |
| Multiple mixed carriers | Rebuild each unresolved original and child identity once, regardless of whether another carrier escaped, was shot, was zap-resolved, or had cargo killed. |
| Repeated deaths | Repeat the same identity-set reconstruction without growing or shrinking obligations; never reset child IDs on life loss. |

The chosen behavior for an unresolved stillborn child after death is a
**PROJECT DECISION** inherited from the current development policy: it is an
unresolved cargo unit, so it returns as a fresh Egg/Flipper without the old
zap timer. The exact arcade treatment is not established. A fresh new Level
3, in contrast, clears all old identities and timers and creates a new 15-unit
wave.

Tests must compare identity sets before and after each reconstruction, not
only counts. Include a carrier with latent cargo, released live children,
zapped stillborn children, some already destroyed children, several carriers,
and at least two consecutive deaths.

## 16. Level 2 -> Level 3 transition

The existing resolver returns Level 2 only from Level 1. Standard production
exit commits the next configuration after successful `ExitZoom` travel reaches
the depth threshold; wave clear and `ExitDelay` are not commitment points.
Extend that resolver to Level 1 -> Level 2 -> Level 3 only after the complete
direct Level 3 wave is safe.

At the successful Level 2 exit, use the same fresh-wave initializer to install
Level 3 exactly once. Preserve score and current lives; reset Super Zapper to
two charges and zero flash; restore projection; create fresh Level 3 accounting
and Eggs; set the Level 3 player start; clear shots, bullets, Flippers,
Tankers, pending cargo activation, and old zap targets; and ensure the first
rendered Level 3 frame uses the plus geometry. Do not award a bonus or add
unimplemented Begin/Ready presentation by implication.

`--level2-test`, Tanker and other development scenarios remain terminal and
replay their launch configuration. A direct `--level3-test` must also be
terminal/replayable and must not alter natural progression. After natural
Level 3 completes, keep the current explicit no-next-level policy:
display a Level 3 terminal message and let R start a new Level 1 campaign
with score/lives reset. This is a **PROJECT DECISION**, not a claim that
Atari Level 3 is terminal.

## 17. Development access recommendation

For C1, use a narrowly named `--level3-geometry-test` that exposes the exact
Level 3 board/pivot geometry for manual rendering without pretending to be
the production mixed wave. It should be **DEVELOPMENT ONLY**, mutually
exclusive with other wave-selection scenarios, terminal/replayable, and
should not be a second Level 3 board/configuration. It may provide controlled
Flippers or geometry probes only to inspect lanes and pivots; display that it
is a geometry diagnostic and do not use it to claim production composition.

Do not add a production-shaped `--level3-test` until the full mixed-wave path
is implemented. At the production-wave checkpoint, add `--level3-test` as
direct terminal access to the exact same `LevelConfiguration.Level3` object
and natural randomized selection policy used by progression. It must not
create a second fixed Tanker composition. Deterministic seeds/seams belong in
tests; any manual debug controls/overlays are explicitly development-only and
must not change production RNG, selection, or accounting.

Keep the existing `--tanker-test` available and unchanged as a one-carrier
diagnostic on Level 1 geometry. Keep Spiker, Fuseball, Pulsar, Level 2-test,
and enemy-bullet options orthogonal/terminal as currently documented, subject
only to explicit flag conflict tests.

## 18. Staged roadmap

### C1 — Isolated Level 3 board and geometry groundwork (next)

- **Purpose:** Install source-derived plus-board points and safely represent
  all ordinary and cargo adjacent-lane pivots without changing production
  composition or Levels 1/2.
- **Prerequisites:** Clean `f6ac233`; existing B1/B2 tests and geometry remain
  green.
- **Expected files:** `Levels/LevelConfiguration.cs` / board loading, `Levels/Level.cs`,
  `Gameplay/Flipper.cs`, `Rendering/VectorRenderer.cs`, focused Level 3 geometry
  tests, and development access plumbing only if required. Update README and
  development notes for the new diagnostic; do not update behavior claims.
- **Automated checks:** Exact identity/board/coordinates/topology tests;
  32 directed adjacency transitions; canflip, wrap, cargo pivots, finite
  projection, no zero-angle/zero-length math; Level 1 legacy and Level 2 B1/B2
  regressions.
- **Manual check:** Launch the geometry diagnostic, inspect the plus outline,
  traverse the rim, and observe ordinary/cargo pivots on straight runs,
  concave corners, and the wrap. Do not judge composition or progression.
- **Boundary:** One geometry/configuration checkpoint. No production Tanker
  selection, ledger, firing promotion, or campaign edge.

### C2 — Isolated Tanker firing validation/policy

- **Purpose:** Resolve whether current source-derived Tanker firing is
  acceptable against Neal's qualitative arcade observation before any
  production Tanker is enabled.
- **Prerequisites:** C1 may be complete; the existing
  `--tanker-test --enemy-bullets-test` fixture remains available.
- **Expected files:** Tanker firing configuration/implementation only if
  evidence warrants a change, focused Tanker tests, and the related
  development/spec history after manual validation.
- **Automated checks:** Ammo bounds and depletion; delayed attempts and
  release-height cutoff; no firing after release; Tanker bullets remain
  independent of their carrier; no regression when `--tanker-test` is silent
  or when enemy bullets are otherwise opt-in.
- **Manual check:** Compare Tanker firing qualitatively with Atari, especially
  whether multiple shots can occur before release. Do not require invented
  exact cadence or timer measurements.
- **Boundary:** Decide and validate the policy; no mixed production wave or
  natural progression. If evidence is insufficient, keep firing opt-in and
  record a blocker rather than silently promoting it.

### C3 — Level 3 mixed-wave identities and direct production-wave access

- **Purpose:** Add the exact production Level 3 configuration, source-shaped
  Flipper/Tanker hatching, stable original/cargo identities, multi-carrier
  Super Zapper, death reconstruction, and all-rim/completion accounting.
- **Prerequisites:** C1 geometry passed; C2 Tanker firing policy is resolved
  or explicitly deferred with the observable omission documented.
- **Expected files:** `LevelConfiguration`, `Level`, `Egg`, `Flipper`, `Tanker`,
  `TempestGame`, composition/debug CLI/rendering as needed, and focused
  mixed-wave, death, Super Zapper, and all-rim tests. Do not move Level 1/2
  onto the new ledger.
- **Automated checks:** Selection boundaries/eligibility and six-active-
  Tanker cap; zero/one/multiple Tankers; all identity conservation paths;
  multiple delayed carriers and children; all-rim; repeated death; no
  duplicate/lost IDs; exact-once score; unchanged old scenarios.
- **Manual check:** Direct `--level3-test` with the production randomized
  composition; observe Tanker release, cargo, death/re-entry, Zapper, and
  all-rim. The direct mode remains terminal.
- **Boundary:** Production Level 3 is testable but still unreachable by
  normal Level 2 progression.

### C4 — Natural Level 2 -> Level 3 progression

- **Purpose:** Add the one natural campaign edge after successful travel.
- **Prerequisites:** C3 automated and manual gates passed; exact first-frame
  geometry, fresh accounting, and selected firing policy are accepted.
- **Expected files:** `LevelProgression`, `TempestGame`, CLI displayed-level
  handling, progression/configuration tests, README, and development notes.
- **Automated checks:** Commit only after exit travel; score/lives carry;
  fresh two-charge zapper and projection; no stale Level 2 objects; terminal
  Level 3; all development modes remain terminal; full regression suite.
- **Manual check:** Complete Level 2 through normal play and observe
  uninterrupted entry to Level 3, then separately validate the Level 3
  terminal behavior.
- **Boundary:** Level 2 -> 3 only. No Level 4 or cycle/bonus architecture.

Behavior-spec additions belong after implementation and the relevant manual
validation, not during this analysis or before evidence exists.

## 19. Exactly one next implementation milestone

**C1 — Isolated Level 3 board and geometry groundwork.**

Starting point is the verified clean `f6ac233` checkpoint. This is
analysis-derived groundwork rather than visible natural Level 3. It has one
clear manual target—the plus-board geometry and all lane-pivot paths—and does
not depend on a fake production composition, generalized accounting, or
unimplemented firing/progression. Keep it bounded to one board definition,
one generalized endpoint-pivot path for ordinary and cargo Flippers, tests,
and a development-only geometry harness if needed.

The C1 approval gate is: all 16 directed adjacent transitions work with finite
projection; Tanker cargo avoids the current zero-angle shortcut; Level 1
legacy geometry and Level 2 square behavior remain unchanged; Neal accepts
the visual board/pivots. Do not let C1 accidentally add a normal Level 3 wave.

## 20. Deterministic test strategy

### Baseline and maintenance

- Record the clean starting commit, status, and `git diff --check`.
- Run focused tests during each checkpoint.
- At the final implementation checkpoint, run the solution build and the full
  409-test-or-later suite; record actual totals rather than reusing historical
  counts.
- Run `git diff --check`, `git status --short`, and `git diff --stat`.
- Confirm `Reference/incitti` is unchanged and review every unexpected or
  untracked file. Keep commits under Neal's control.

### Configuration and geometry

- Assert internal/displayed identity 2/3, board/palette 2/0, exact row points,
  closed topology, 16 vertices/lanes, center/offset, depth/position,
  player lane, enemy supply 15, four initial Eggs, and 11 reserve units.
- Test source-vs-project release distinction: reference burst release (up to
  six per 50 ticks) versus whichever explicitly chosen C# policy is
  implemented.
- Exercise all 32 directed lane transitions; finite coordinates at start,
  intermediate, and end; straight and corner arcs; wrap; no zero vector,
  zero-turn divide, NaN, or infinity.
- Verify canflip, rim pivots, Tanker rendering, initial cargo pivots, cargo
  fixed below-rim direction, Super Zapper flash, and player shots/Egg motion.
- Run Level 1 and Level 2 geometry tests unchanged.

### Composition

- Deterministically test eligible/rejected hatch choices and random range
  boundaries, including Tanker count below/at six and near-end selection.
- Verify only Flippers/Tankers and only two-Flipper cargo occur at Level 3.
- Exercise source-permitted zero, one, and multiple Tankers, including several
  sequential carriers after earlier ones release; do not fix production slot
  0 as Tanker.

### Accounting, life loss, Super Zapper, and all-rim

- Assert conservation for original units, destroyed, escaped, and rim-cleared
  outcomes; latent cargo; materialized child IDs; killed and rim-cleared
  children.
- Test shot, natural, and zap release; exact-once carrier/child score; same
  shot cannot hit newly released cargo; no carrier score for natural escape or
  rim clear.
- Test multiple carriers with simultaneous/staggered zap timers; unique child
  IDs; first all-target/second single-target use; pending-shot pre-emption;
  death during carrier/child timers; exact target score order.
- Test all-rim blockers for Eggs/reserve, latent cargo, below-rim Tanker,
  released below-rim children, queued cargo, and pending targets. Verify
  scoreless rim clear removes pending timers and records distinct outcomes.
- Test death before release, after natural/shot release, after zap expiry,
  partially destroyed stillborn cargo, several carriers, and repeated
  reconstruction. Compare ID sets, not just totals.
- Completion remains blocked until each unresolved obligation is represented
  and resolved. Level 3 normal wave contains no Spikes.

### Progression and regression

- Only successful Level 2 exit travel installs Level 3; no transition on
  wave clear, delay, death, or development mode.
- Score/current lives carry; Super Zapper has two fresh charges; projection
  and first frame are correct; no stale Level 2 entity/shot/zap remains.
- Level 3 remains terminal until Level 4 exists; R starts a new campaign.
- Preserve `--tanker-test`, `--spiker-test`, `--fuseball-test`,
  `--pulsar-test`, `--level2-test`, optional bullets, and their terminal
  replay behavior.
- Keep any Tanker firing observation documented if firing remains deferred.

## 21. Manual validation strategy

Run manual checks in this order; do not use exact timer measurements as a
pass criterion:

1. **Geometry:** Inspect the plus board, 16-lane wrap, player rim travel,
   ordinary Flipper turns on straight runs and corners, and cargo pivots.
   Confirm no snapping, wrong-lane landing, or non-finite rendering.
2. **Mixed composition:** In direct production `--level3-test`, observe that
   Flippers and Tankers can both appear and that Tanker count can vary;
   distinguish this from the fixed single-Tanker development scenario.
3. **Live cargo:** Shoot a Tanker; verify only the Tanker is destroyed,
   200 points are awarded, two live children appear in distinct non-Tanker
   lanes, continue outward, and do not dynamically track the Blaster below
   the rim.
4. **Natural release:** Let a Tanker reach its release zone; observe live
   cargo and no Tanker kill points.
5. **Death/re-entry:** Lose a life before release, after release, and after
   child materialization. Check that resolved units stay resolved and each
   unresolved carrier/cargo obligation returns once.
6. **Super Zapper:** Use the first all-target charge with multiple enemies
   and, if available, multiple Tankers; later use the second charge. Observe
   delayed Tanker resolution, children, no duplicate score, and a life loss
   during pending resolution.
7. **All-rim:** Exhaust incoming Eggs and allow all remaining qualifying
   enemies/cargo to reach the rim. Verify they disappear without kill score
   and exit begins only when no latent or released cargo obligation is lost.
8. **Tanker firing:** Separately compare the validated policy to the
   qualitative Atari observation; do not infer exact ammunition or cadence.
9. **Progression:** After the direct wave is accepted, complete Level 2
   naturally and confirm score/life carry, fresh two-charge zapper, new plus
   geometry, no terminal pause/stale entities, and the explicit Level 3
   terminal/restart policy.

## 22. Expected files

No source files are expected to change during this analysis. The requested
deliverables are:

- `AI-Logs/Prompts/2026-10-31-level3-tanker-progression-analysis-prompt.md`
- `AI-Logs/Reports/2026-10-31-level3-tanker-progression-analysis-report.md`

For future implementation, see the per-stage expected-file lists in §18.
No duplicate board data, copied production configuration, or generated
implementation files should be added merely to support this report.

## 23. Documentation and Behavior Spec impact

Do not edit `TEMPEST-BEHAVIOR-SPEC.md` during analysis or before a future
implementation and manual validation establish behavior. C1 may update
README/development notes to explain a clearly labeled geometry-only
development flag and its non-production scope. C2 may update Tanker firing
history only after manual comparison. C3 may document that direct Level 3
composition is **DEVELOPMENT ONLY** and distinguish reference values from
project choices. C4 may document natural progression after Neal validates it.

Keep current Level 2 terminal wording and behavior until C4. Any future spec
change must preserve the evidence labels and must not upgrade Incitti-only
values to **ARCADE VERIFIED**.

## 24. Explicit out-of-scope items

- Implementing any gameplay, tests, project files, or configuration in this
  analysis.
- Natural Level 3, its mixed production accounting, or Level 2 -> Level 3
  progression in C1.
- Migrating Level 1/2 accounting or changing their validated geometry,
  composition, Flipper tuning, bullets, death, or progression.
- Replacing the Level 1 legacy pivot path or changing B1/B2 globally.
- Enabling Flipper bullets by default.
- Promoting Tanker bullets before their separate validation decision.
- Reverting the arcade-validated outward cargo result to Incitti's inward
  route.
- Adding Spikes, Fuseballs, Pulsars, Fuseball/Pulsar Tanker cargo, Level 4,
  open boards, palette cycles, source bonus/intro presentation, or a universal
  entity-component accounting system.
- Creating a fixed or deterministic production Tanker count/slot.
- Committing or pushing.

## 25. Remaining arcade/reference uncertainties

- Incitti source composition, wave timing, source random ranges, and
  constraints do not prove original Atari Level 3 composition.
- Exact arcade plus-board pivot arcs, projectile/cargo geometry, Tanker climb
  and release timing, and Tanker glyph are not manually validated here.
- Tanker firing's exact ammo, chance, delay, and observed second-shot behavior
  are not established beyond Neal's qualitative observation.
- Flipper bullet behavior remains opt-in and unpromoted.
- Exact death reconstruction/type reselection, internal carrier/cargo
  accounting, Super Zapper timer order, score order, and all-rim internals are
  project/reference details rather than hidden arcade observations.
- Level 3 start fractional lane position, any between-level bonus/presentation,
  exact transition timing, and post-Level-3 Atari campaign behavior remain
  unverified.
- Source float/integer conversion nuances and exact timing constants remain
  Incitti-derived unless independently checked.

## 26. Final Git and maintenance result

After saving this report and the exact requested prompt copy, the final checks
must show:

```text
git log -1 --oneline
f6ac233 Implement Level 1 to Level 2 progression

git status --short
(empty)

git diff --check
(no output; exit 0)

git diff --stat
(empty)

git status --short -- Reference/incitti
(empty)
```

Both requested deliverables exist in the workspace. The repository-wide
`.gitignore` rule ignores `AI-Logs/`, so Git status remains clean and does not
list them. No tracked files or Git metadata were changed; no commit or push
was made.

## 27. GO / NO-GO

**GO:** C1, the isolated Level 3 board/geometry groundwork from the clean
`f6ac233` checkpoint.

**NO-GO:** natural Level 3 production mixing or Level 2 -> Level 3 progression
until the plus geometry/cargo path is validated, Tanker firing is separately
resolved, and multi-carrier identity/accounting, death, Super Zapper,
completion, and all-rim behavior pass the direct Level 3 test and manual
gates. Neal retains control of any future implementation commit.
