# C3: Level 3 mixed-wave pre-implementation analysis

Baseline: `6d15165 Correct and validate isolated Tanker firing timing`.
Scope: analysis only. All designs below are proposals, not implemented behavior.

## 1. Disposition

**Recommend B: split C3 into two engineering checkpoints.**

1. **C3a: complete headless Level 3 mixed-wave model.** Implement production
   configuration/selection/scheduling, original and owned-child identity
   conservation, all carrier outcomes, multi-carrier Zapper, death and completion.
   Exercise the actual game orchestration through a configuration-based headless
   entry. No new command-line access and no campaign progression.
2. **C3b: direct production-wave access and manual acceptance.** Wire
   `--level3-test`, bullet visibility, accurate HUD, titles/conflicts and terminal
   replay/restart to that same completed model. Run regressions and Neal's manual
   checklist. Do not change composition or lifecycle policy to make manual
   outcomes easier.

The split is not a silent-Tanker production checkpoint or a larger fixed-slot
diagnostic. C3a must be internally complete, with armed Tankers and integrated
lifecycle tests, before C3b exposes it. The reason is the current concentration
of fixed-slot accounting assumptions and development-flag-dependent dispatch:
identity faults need deterministic isolation before graphical/manual integration.

**GO for a separately authorized C3a implementation prompt with the boundaries
below. NO-GO for implementing during this analysis, an atomic all-surfaces C3
prompt, or C4/natural Level 2 -> Level 3.** C3 overall is not manually accepted.
The recommendation includes a specifically labeled, Level-3-only source-shaped
Egg scheduling policy; it does not imply literal global Incitti timing.

## 2. Baseline and evidence

Opening checks, also reconfirmed during analysis:

```text
git status --short
(empty)
git log -3 --oneline
6d15165 Correct and validate isolated Tanker firing timing
27c43b7 Add validated controller restart and quit aliases
a922172 Ignore local video files
git diff --check
(empty)
git status --short -- Reference/incitti
(empty)
```

Authority remains, in descending order:

1. Neal's direct Atari observations and specific manual validations.
2. Literal Incitti source.
3. Current C# behavior and automated tests.
4. Explicit necessary project decisions.

[The evidence definitions](../../TEMPEST-BEHAVIOR-SPEC.md#L3) apply throughout.
**ARCADE VERIFIED** is limited to the visible outcomes Neal actually validated.
Selection probabilities, hidden identities, scheduling internals and source
constants are not upgraded by passing C1/C1.5 or C2.

C1/C1.5 acceptance establishes inward visible pivots, sensible directed
90/180/270 routes and qualitative duration, and the accepted outward cargo
lane route: [specification](../../TEMPEST-BEHAVIOR-SPEC.md#L252).
C2 acceptance establishes capability for two separated shots before release,
continued climb, no release burst, independent existing bullets and unchanged
cargo/Zapper/input: [specification](../../TEMPEST-BEHAVIOR-SPEC.md#L384),
[completed implementation report](2026-10-31-c2-tanker-firing-correction-implementation-report.md#L207).
It does not guarantee two shots for every Tanker. The 1.75 increment remains
**REFERENCE VERIFIED**, not arcade-measured.

Neal's recent qualitative Incitti replay found overall timing extremely close
to Atari, without meaningful deviations. That makes literal reference timing a
particularly useful engineering comparator, not an arcade measurement or a
promotion of any constant.

## 3. Current architecture and obsolete prior advice

### Current surfaces requiring attention

| Surface | Actual post-C2 behavior | C3 consequence |
|---|---|---|
| Configuration | `Level3Plus` is Flipper-only geometry diagnostic; composition enum supports only Flippers | Add a distinct production mixed configuration sharing existing board data |
| Original identity | Diagnostic original IDs, fixed Tanker slot 0; normal waves have no original IDs | Mixed originals must all have stable IDs and individually selected types |
| Latent cargo | Active-carrier sum plus hardcoded unresolved-slot-0 fallback | Production requires persistent ownership, including when a selected carrier is an Egg/reserve |
| Materialized cargo | Monotonic IDs allocated on release; unresolved/killed/rim-cleared sets | Preserve diagnostic behavior; production must retain parent/child ownership |
| Pending cargo | Pending count is a subtraction; shot-born children also have an activation queue | Do not use this count as representation proof |
| Death gathering | Gathers pending cargo reserve, cargo Eggs and active cargo, but not shot activation queue explicitly | Production snapshots must include every location, not silently infer survivors from counts |
| Hatch/shot dispatch | Identity factory and ledger resolution are gated by `tankerTest`; mixed ordered collision path uses development flags | Select production behavior by actual composition, independently of CLI flags |
| Tanker fire | Ammo/timer and firing delegate require enemy-bullet development flag | Arm production Tankers without arming Flippers |
| Zapper input | Ordered mixed target list selected only under development conditions | Supply the same ordered target sequence for production mixed play |
| Completion | Tanker diagnostic uses unresolved sets; other waves use supply counters | Production requires its own complete original/child set predicates |
| Bullet renderer | `Draw()` renders bullets only if `EnemyBulletsTest` | C3b must show production Tanker bullets even without that flag |
| HUD | Tanker diagnostic ledger or simple `RemainingEnemies`/Flipper label | Production cannot display the simple 15-unit remaining counter as cargo accounting |
| Progression | Resolver only maps Level 1 -> Level 2; commit path explicitly checks Level 1 | Leave both untouched for C3 |

Primary source anchors:
[game ledger/properties](../../src/Tempest.Game/TempestGame.cs#L40),
[fresh/death reset](../../src/Tempest.Game/TempestGame.cs#L253),
[Zapper source order](../../src/Tempest.Game/TempestGame.cs#L423),
[hatch/factory/collision](../../src/Tempest.Game/TempestGame.cs#L475),
[carrier release/resolution](../../src/Tempest.Game/TempestGame.cs#L760),
[completion](../../src/Tempest.Game/TempestGame.cs#L958),
[Level queues/scheduling](../../src/Tempest.Game/Levels/Level.cs#L17),
[configuration](../../src/Tempest.Game/Levels/LevelConfiguration.cs#L105),
[renderer bullet gate](../../src/Tempest.Game/Rendering/VectorRenderer.cs#L96),
[HUD](../../src/Tempest.Game/Rendering/VectorRenderer.cs#L603),
[progression resolver](../../src/Tempest.Game/Levels/LevelProgression.cs#L5).

### Stale historical plan material

The [earlier Level 3 analysis](2026-10-31-level3-tanker-progression-analysis-report.md)
and [C1 report](2026-10-31-level3-c1-geometry-implementation-report.md) remain
historical, not current prescriptions:

- Their baseline `f6ac233`, "next C1" recommendation and older test totals are
  superseded by accepted C1/C1.5, accepted C2 and current `6d15165`.
- The former 1.85 Tanker approximation is corrected; do not introduce a
  general Tanker-tuning framework or reopen the accepted firing algorithm.
- Pending/single-shot C2 uncertainty is superseded by manual two-shot PASS.
- Square-only helpers, ordinary Level 1 legacy rendering, zero-angle cargo
  bypasses and pre-C1.5 pivot limitations no longer describe current source.
  Level 1, Level 2 and the diagnostic use the accepted endpoint/inward path.
- "Exact plus-board pivots not manually validated" is stale for the specific
  visible behaviors Neal passed; hidden Atari math/timing still is unknown.
- A production Level 3 wave with silent Tankers is no longer the recommended
  intermediate deliverable. C2 is already accepted.
- A count-only latent/materialized transfer is insufficient for multi-carrier
  ownership. Retain the useful original-ID proposal but specify child states.
- C4 transition/recharge/carry advice is future context only, not C3 scope.
- Some authoritative documents still use older "no Level 3 configuration"
  shorthand. Current source has diagnostic Level 3, not production Level 3.
  This analysis does not rewrite those documents.

## 4. Exact proposed production Level 3

Use one immutable production configuration, provisionally `Level3Mixed`,
derived from the existing `Level3Plus` board definition rather than copied
coordinates. The diagnostic retains its Flipper-only configuration and schedule.
Future natural access must reference the same production object/selection path
as direct access, but no resolver edge is added now.

| Setting | Proposed effective value | Evidence classification |
|---|---|---|
| Internal/displayed level | 2 / 3 | REFERENCE VERIFIED |
| Board/palette | 2 / 0 | REFERENCE VERIFIED |
| Source board | `Level 3 - plus`, original set | REFERENCE VERIFIED |
| Topology | Closed, 16 vertices and 16 playable lanes | REFERENCE VERIFIED |
| Center Y / Y offset | 400 / -80 | REFERENCE VERIFIED |
| Depth / playing position | 400 / 50 | REFERENCE VERIFIED values; immediate playing startup is PROJECT DECISION |
| Player start | Lane 7, fractional 0.5 | Lane 7 REFERENCE VERIFIED; initial source `scl=0.5` is REFERENCE VERIFIED, resetting that fraction on every fresh/reconstructed C# entry is PROJECT DECISION |
| Original supply | 15 (`10 + 2/2 + 2*2`) | REFERENCE VERIFIED; integer division |
| Initial entrants/reserve | 4 Eggs / 11 originals | REFERENCE VERIFIED |
| Eligible hatched originals | Flipper or Tanker only | REFERENCE VERIFIED |
| Tanker cargo | Two Flippers only | REFERENCE VERIFIED; final route follows accepted PROJECT DECISION/ARCADE VERIFIED observable result |
| Tanker climb | 1.75 from height 10 | REFERENCE VERIFIED; C2 manually accepted observable firing capability |
| Tanker firing | Ammo 1..2, timer 17..80, strict post-climb ceiling 380, reset 16, inclusive roll >75 | REFERENCE VERIFIED; exact Atari internals unresolved |
| Flipper tuning | canflip true, climb 2, turn 8, pause 21 / rim pause 24 | REFERENCE VERIFIED early-tier constants |
| Pivot | Existing endpoint/inward geometry, full directed sweep duration | PROJECT DECISION representation; accepted visible observations ARCADE VERIFIED |
| Zapper fresh setup | Two charges, zero flash | REFERENCE VERIFIED charge count; C# reset boundary PROJECT DECISION |
| Exit | Existing standard delay/zoom, no spikes | Source Level 3 no spikes REFERENCE VERIFIED; C# exact exit sequencing PROJECT DECISION |
| Direct launch | `--level3-test`; terminal after Level 3, no campaign edge | DEVELOPMENT ONLY / PROJECT DECISION |
| Flipper ammo default | Zero, both ordinary and cargo | PROJECT DECISION; optional development flag retained |

Exact board points, in existing order:

```text
(98,98), (98,197), (0,197), (-98,197),
(-98,98), (-197,98), (-197,0), (-197,-98),
(-98,-98), (-98,-197), (0,-197), (98,-197),
(98,-98), (197,-98), (197,0), (197,98)
```

Literal evidence:
[board row](../../Reference/incitti/levelsets/levelset1.txt#L6),
[loader](../../src/Tempest.Game/Levels/LevelLoader.cs#L29),
[source level/board/palette/start/recharge](../../Reference/incitti/tempest.bmx#L1729),
[player fractional field](../../Reference/incitti/tempest.bmx#L604),
[SetupEnemies and tuning](../../Reference/incitti/tempest.bmx#L4210),
[Tanker update/create](../../Reference/incitti/tempest.bmx#L2555).

### Production scheduling: an explicit choice, not diagnostic inference

**Recommend Level-3-only source-shaped supply/approach scheduling**, while
preserving the project's active-play update pipeline and death freeze.

Why not silently inherit the diagnostic's schedule? It releases one Egg per
50 active steps with a four-Egg ceiling and chooses one approach per 50 steps.
Those are existing project policies, not the literal main loop, which releases
up to six reserve Eggs per global 50 ticks. Moreover, ordinary diagnostic
hatches are spaced 50 steps apart. A 215-update carrier lifespan permits at
most five overlapping uninterrupted default carriers; a six-carrier cap
would be tested only through fixtures, not normally exercised by that schedule.
C3 should not hide this limitation inside "production reference behavior."

Proposed exact effective scheduling rules:

1. Four initial original Eggs; each new Egg chooses an inclusive random lane
   0..15 and direction from source 0..1, mapped to C# signed direction.
2. Every 50 **active Playing** steps, release up to six queued entrants, in
   queue order, limited by available obligations. Do not apply the old
   four-active-Egg cap to this production path.
3. Use existing eight-step Egg motion cadence and source-shaped randomized
   roaming/approach decisions: approach is eligible only with fewer than nine
   active enemy units and fewer than two approaching/hatching Eggs.
   At `lv=2`, `hatchrate=2/8=0`; the approach gate is inclusive Rand(0,100)>95
   or active enemy units plus active Eggs <4. Preserve the source's roaming
   height-boundary/random-reversal behavior in this Level-3-only path.
4. Count active carrier/ordinary/materialized-child enemy objects, not bullets,
   reserve originals or latent children, for the source active-enemy gate.
   Pending activation children count as active obligations for this calculation.
   Do not impose a new nine-at-hatch cap: source tests <9 before approach,
   so already approaching Eggs can later hatch above that count.
5. Preserve C# pipeline conventions: ready Eggs hatch in the current hatch
   pass; new originals update later in that same game step; released cargo
   first updates the next step. This is not source-linked-list tick equivalence.
6. Death/reconstruction freezes the scheduling clock, discards old Egg motion,
   resets the local schedule phase and releases up to four fresh surviving
   entrants initially, with materialized cargo prioritized as already accepted.
   Later six-at-50 releases drain remaining re-entry obligations.

Rules 1, the source burst size/interval, source approach thresholds and source
roaming conditions are **REFERENCE VERIFIED** engineering inputs:
[Egg update/create](../../Reference/incitti/tempest.bmx#L2065),
[literal release loop](../../Reference/incitti/tempest.bmx#L3741).
The active-play local clock, immediate C# hatch/update order and identity-based
four-Egg reconstruction are explicit **PROJECT DECISIONS**. Incitti's
globalclock releases through presentation/death phases; C# must keep its
accepted freeze, not copy that global behavior.

Represent this with one explicit mixed-wave scheduling branch/policy in Level
and Egg, not arbitrary new tuning controls. Existing deterministic scheduling
fields/sequences continue to apply to Level 1/2 and the geometry diagnostic,
not as effective production caps. Do not duplicate the plus-board coordinates.
Range-aware tests must protect scheduler RNG decisions separately from
PickEnemy draw ordering; matching global seeded Incitti PRNG sequences is not
claimed. This is a necessary new policy choice for the later implementation
prompt, not an already accepted production timing observation.

## 5. Literal and proposed mixed selection

[Incitti PickEnemy/Hatch](../../Reference/incitti/tempest.bmx#L1982) is literal
authority. At displayed Level 3:

```text
repeat for an unselected original ready Egg:
    upper = 4 if active Tankers < 6, otherwise 3
    candidate = inclusive Rand(-6, upper)
    if candidate < 0: candidate = 0
    if active enemy units + active Eggs < 4 OR active Spinners > 3:
        candidate = inclusive Rand(1, upper)  // overwrites FIRST draw
    reject 0 (Spinner), 2 (Pulsar), 3 (Fuseball); repeat
    accept 1 (Flipper), or 4 (Tanker)

if newly selected Tanker:
    repeat inclusive Rand(0,2) until 0
    // 1 Fuseball cargo and 2 Pulsar cargo are disabled at this level
    commit two-Flipper cargo, then create Tanker with fresh C2 ammo/timer
```

At Level 3 Spinners are disabled, so the Spinner-count branch is not normally
reachable; the near-end branch remains important. Do not collapse its two
draws into a single alternative draw. The active-Egg count includes the Egg
being hatched, as the reference removes it after creating the enemy.
For the C# equivalent, active enemy units include pending-activation children;
latent children, terminal IDs, bullets and reserve queues are excluded.

The six cap is **simultaneous**, not a six-per-wave quota. Count live Tanker
objects including pending-zap carriers; decrement by actual removal on natural,
shot or zap resolution. Recalculate before each next selection. A Tanker
removed in the later enemy-update phase frees capacity for the next hatch pass;
a player shot before hatching can free capacity in the current pass.

Under independent uniform reference draws and active count below six, rejection
leaves two equally weighted accepted candidates: Flipper and Tanker. The
near-end overwrite does not change that conditional 1:1 result, but does change
draw consumption. At six, only Flipper is accepted. This is deterministic
arithmetic about reference ranges, **not measured Atari probability**. No fixed
wave count follows: zero, one, multiple, and more than six sequentially selected
Tankers are possible. Per-wave selected Tankers cannot exceed 15 originals.

**PROJECT DECISION:** Commit selected type on first successful hatch. After
death, do not reroll an unresolved selected Tanker as a Flipper. If such a
reconstructed ready Tanker encounters the six-active cap, leave its Egg ready
and skip it while processing other hatchable Eggs. Do not reroll, consume its
latent children, or spin inside `while Hatch...` on the blocked Egg. An
unselected Egg at the cap can still select Flipper. Awaiting capacity remains
a completion blocker. Child re-entry always selects a fresh cargo Flipper
without original-type selection.

Type/cargo selection use the existing injected Random/inclusive adapter; strict
finite test scripts fail on wrong ranges or unexpected draws. No fixed slot,
seed, guaranteed mixed ratio or forced-count command-line option. Literal
rejection is retained rather than adding an arbitrary retry ceiling or
weighted shortcut. Adversarial never-accepting test scripts must fail in the
test double, not cause production fallback enemy types.

## 6. Narrow identity model and invariants

Stable originals **0..14 remain the narrowest safe choice**. Do not introduce
universal entities or migrate Level 1/2. Keep the isolated diagnostic's public
counts, slot-0 policy and release-allocated child IDs intact.

Add a small **Level-3-only wave ledger** owning original selected types,
terminal outcomes and child ownership. Use current Egg/Flipper/Tanker identity
fields for runtime references; Level owns entrant queues. Game orchestrates
runtime-to-ledger transitions. Do not keep independent counter-based survivor
truth alongside it.

Recommended production child identity is `(ownerOriginalId, ordinal 0 or 1)`,
with an integer adapter `CargoUnitId = 2*ownerOriginalId + ordinal` if reusing
current fields. Create the two **latent records at first Tanker commitment**,
not again at release. Store owner and ordinal explicitly in the ledger.
Original and cargo IDs have different namespaces: `Original(7)` is not
`Cargo(7)`. An Egg/Flipper may hold exactly one namespace (other field -1);
a Tanker holds one original ID. No raw integer union across namespaces.

This changes production allocation semantics structurally but not cargo
gameplay; the diagnostic remains on its existing allocation semantics.
Proof: equal child IDs imply equal owner and ordinal by integer quotient/
remainder; different owners cannot share a child; namespace tags prevent
original/child collisions even where their integer values overlap.

### Legal states

- Original: reserve -> Egg -> active Flipper or active Tanker -> destroyed,
  escaped (Tanker only), or rim-cleared. Death returns any unresolved original
  to reserve/Egg with selected type retained; unselected originals stay
  unselected. Terminal outcomes never re-enter.
- Cargo: latent under unresolved selected Tanker -> pending activation or
  active child -> destroyed or rim-cleared. Life loss sends unresolved
  materialized children to re-entry reserve -> fresh Egg -> active cargo.
  Latent children remain under the reconstructed carrier, not child Eggs.
- Queued child states distinguish shot activation from re-entry reserve.
  Terminal cargo never becomes latent, queued, Egg or active again.

### Required invariants at stable step/lifecycle boundaries

1. The disjoint union of original states equals exactly `{0..14}`.
2. Each unresolved original has exactly one location: reserve queue, Egg or
   active runtime enemy. Selected type agrees with that enemy.
3. Each committed Tanker original owns exactly two child records, forever
   within this wave. Unselected/Flipper originals own none.
4. Every unresolved child has exactly one representation: latent owned by
   one unresolved Tanker original (even in reserve/Egg), shot activation queue,
   re-entry reserve, Egg or active child. Terminal children have none.
5. Latent records are obligations, not independent enemies. Never count a
   latent record again as materialized, or release a second pair on re-hatch.
6. Terminal carrier implies no latent children. Natural/shot/zap commit and
   materialization transition the existing pair exactly once.
7. At most six active Tankers, regardless of selected-but-not-active Tankers.
8. Kill scoring occurs only with a successful nonterminal -> killed transition:
   carrier 200, Flipper/child 150. Natural and rim-clear outcomes score zero.
9. Runtime lists and insertion-ordered shot/Zapper targets agree. Pending shot
   children exist only in their pending queue until the activation point.
10. Within a life, total ledger identities are `15 + 2*T`, where T is count of
    originals ever committed as Tankers. This grows only on new Tanker
    commitment; death/release does not change it. Do not use the diagnostic's
    `10 + materialized children` display as the production conservation rule.

Use actual set equality **and duplicate detection**, not `Distinct()` alone.
Equal cardinalities can conceal one missing and one extraneous ID. Transition
helpers must reject impossible ID/state/ownership combinations explicitly
using existing exception conventions; do not hide faults with clamps, silent
score returns, missing-unit reconstruction or a success-shaped completion.
No new logging framework is needed.

## 7. Carrier/cargo lifecycle table

All rows preserve the existing route, inward spatial pivot, release threshold,
next-step cargo movement and shot-before-enemy ordering.

| Event | Carrier outcome / score | Existing owned pair and timers | Reconstruction / completion |
|---|---|---|---|
| A. Natural release | Escaped / 0 | Latent -> two active live children; no child zap timers | Carrier never returns; each surviving child can score 150 later; children block clear |
| B. Player shot | Destroyed / 200 once | Latent -> two queued live children, then activation; no zap timers; killing shot consumed | No carrier re-entry; each child independently unresolved/scorable; pending queue blocks clear |
| C. Zapper expiry | Destroyed / 200 once | Latent -> two active stillborn children, each fresh timer 8; expiry skips carrier movement/fire | Children remain unresolved until own kill/rim outcome; normal child expiry scores 150 each once |
| D. All-rim | Rim-cleared / 0, only when eligible | An ordinary unreleased Tanker still has latent children and must BLOCK this path, not erase them | See qualification below; no child materialization or latent forgiveness through rim clearing |
| E. Death before release | Carrier stays unresolved; 0 new score | Same two latent IDs remain owned; carrier timer/ammo/height discarded | Same selected Tanker returns as fresh Egg/carrier, fresh firing draws; no child Eggs |
| F. Death after release | Preserve escaped/destroyed carrier and earned score | Each unresolved materialized child retains ID, discards timer/motion | Fresh independent cargo Eggs/reserve; no carrier return or extra children |
| G. Death after one child resolved | Preserve carrier and child terminal outcomes/scores | Only unresolved sibling returns; old timer discarded | Exactly one child obligation remains, not a replacement pair |
| H. Repeated deaths | Preserve every terminal set and selected type | Same latent/materialized ownership and unresolved IDs; no allocator reset | Rebuild same unresolved identity sets; no growth/shrink except independently completed gameplay events |

**D is an important limit, not a new release mode.** Normal carriers naturally
release at h>385, below the h>=400 rim predicate. Thus all-rim ordinarily clears
remaining ordinary/cargo Flippers, while carriers have already escaped or
been killed. Even a forced at-rim carrier with latent children cannot qualify:
current [representation predicate](../../src/Tempest.Game/TempestGame.cs#L1060)
already blocks latent cargo. A carrier with no latent cargo would normally
already have been removed; do not manufacture a reachable production
rim-carrier state to add a fourth release path. Preserve the distinct
rim-cleared original outcome for legitimate qualifying originals and any
compatible non-latent carrier fixture, with zero score and canceled timer.

Existing fired bullets are independent of all carrier removal rows. They stay
in the bullet list/ordered targets until expiry, player-shot destruction,
player impact, life reconstruction or exit cleanup. Death freezes them; fresh
reconstruction clears them. Do not retain a carrier ownership handle that
would erase them on release.

## 8. Multiple carriers and Super Zapper

Zero Tankers gives 15 originals, no child records, ordinary mixed-policy
accounting. One/several/six carriers use the same per-ID operations; there is
no special original 0. A seventh ready selected carrier waits at capacity;
new unselected originals can still select Flippers. Later removals restore
Tanker eligibility without resetting the wave's committed ownership.

Example of disjoint ownership, not a forced production composition:
originals 2, 7, 14 own cargo 4/5, 14/15, 28/29 respectively. Natural release
of 2, shot release of 7 and zap expiry of 14 affect different records even in
one step. Surviving bullets are not any of those cargo IDs.

[SuperZapperSystem](../../src/Tempest.Game/Systems/SuperZapperSystem.cs#L29)
already operates over arbitrary target sequences with one timer on each Enemy.
**No global Zapper redesign is warranted.** Wire the mixed composition into
the ordered source list and keep:

- First charge: accepted target delays 4,6,8,... in insertion order; bullets
  reject without taking a slot. Acceptance step advances existing target
  countdown in its subsequent enemy update.
- Pending non-expiring Tanker continues climb and firing. Expiry is checked
  first and skips movement/fire, kills that carrier once and materializes
  only its pair.
- Children created after that step's Flipper loop retain timer 8 that step.
  Next active step changes it to 7; expiry on their eighth subsequent active
  step skips that child update and awards 150 once, unless preempted.
- Second charge: preserve literal current distance/height/type selection;
  Tanker type 4, Flipper type 3. It can replace an existing child/carrier timer
  with 4, consistent with the current accepted timer replacement contract.
- Player shot precedes enemy timer advancement and preempts a pending carrier
  with live children. No later carrier zap score or stillborn pair.
- Natural release before a non-expiring pending timer removes that timer and
  creates live children, no carrier points. If timer expires on the release
  update, expiry wins and creates stillborn children instead.
- New children are not retroactively included in the earlier all-target
  activation. Their own timer 8 comes only from carrier zap expiry.
- Removed runtime timers cannot resolve ledger records later. Death clears
  them, preserves charge state/score, and returns unresolved IDs as fresh
  entrants; display flash follows its existing independent tick behavior.
- All-rim removal of a qualifying pending target cancels its future score;
  pending timer alone does not block a represented at-rim Flipper. Latent,
  queued or below-rim obligations still block.

Tests must schedule multiple carriers with other targets and rejected bullets
interleaved, not assume all carriers have consecutive delay slots. Assert the
specific original ID, owner/child IDs and cumulative score at each expiry.
In an isolated all-expiry fixture with n carriers and no preemption, the
carrier/child subtotal is `n*(200+2*150)=500*n`; this is a test condition,
not a promise about a natural Zapper play-through.

## 9. Death/re-entry and completion

### Reconstruction transaction

Before discarding runtime objects, validate the ledger against reserve
queues, Eggs, all active originals/cargo and shot activation queue. Snapshot
unresolved original IDs, selected types, latent owner/ordinal IDs, unresolved
materialized child IDs and terminal sets.

Then discard shots/bullets/enemies/pending activation and their runtime timers;
preserve the same Level/configuration, score, charge state and accepted life
loss policy. Requeue each unresolved original exactly once, and each unresolved
materialized child exactly once. Prioritize materialized cargo for the initial
up-to-four fresh Eggs, then deterministically order original IDs/re-entry IDs
for queue reconstruction; actual fresh Egg lanes/directions use production
random choices. Do not reset child ownership on ordinary death.

Validate after rebuilding:

```text
original unresolved set before = original unresolved reserve/Egg set after
materialized unresolved child set before = cargo reserve/Egg set after
latent (owner,ordinal) set before = latent (owner,ordinal) set after
all terminal sets before = corresponding terminal sets after
all selected original types before = corresponding types after
```

No old enemy object, zap timer or firing ammo/timer is restored. Unresolved
stillborn child returns as a fresh unzapped cargo Flipper: inherited
**PROJECT DECISION**, not Atari death-internal evidence. Repeated death while
the child is still a reserve/Egg is explicitly included.

Fresh replay/new game creates a new wave ledger, not the surviving one;
charges reset to two, flash zero, player/projection reset. Game Over remains
terminal until full restart; do not evaluate playing conservation after its
intentional runtime teardown.

### Ordinary completion and all-rim

Ordinary kill/escape completion requires no unresolved originals **and** no
unresolved latent/materialized children, no incoming queues/Eggs, no active
original/child enemies and no shot activation queue. Validate terminal state
coverage, not merely two empty hash sets whose missing records are unknown.
Enemy bullets are hazards, not enemy-unit completion obligations; preserve
existing exit cleanup.

All-rim requires Playing, zero original reserve, zero re-entry cargo reserve,
no Eggs, no latent records and no activation queue. Every remaining enemy is
at post-update h>=400; no below-rim ordinary, carrier or child. Build
namespace-tagged represented original/child lists, reject duplicate IDs, and
require exact set equality with the corresponding unresolved ledger sets.
Then commit only non-kill rim-cleared outcomes, remove runtime/order targets
and begin the existing standard exit. No natural score or delayed zap score.

A count-correct missing-ID state must never pass. Invalid representation is
surfaced rather than silently resolving the unknown unit. Player collisions
precede all-rim as now; a death transition prevents the clear. Existing early
completion/shot/bullet ordering is not globally redesigned.

## 10. Accepted firing integration and direct access

### Production arming

Define Tanker arming from **actual mixed production composition**, not from
`--enemy-bullets-test`:

```text
production mixed Tanker: fresh inclusive ammo 1..2 and timer 16+inclusive(1,64)
isolated Tanker: armed only when the existing enemy-bullets flag is enabled
ordinary/cargo Flipper: ammo only when enemy-bullets flag is enabled
```

Pass the inclusive RNG delegate to armed production Tanker updates even when
the flag is false. Preserve [Tanker algorithm](../../src/Tempest.Game/Gameplay/Tanker.cs#L77):
height +=1.75; if post-climb h<380 and ammo remains, timer--;
when timer<0 reset16 and draw inclusive0..100; roll>75 creates one bullet at
same lane/post-climb height and consumes one ammo. Release at h>385; no release
update firing. Decisions are 17 eligible updates apart.

Default uninterrupted climb has 211 eligible updates and release on update
215/h386.25. Zero, one or two shots remain reachable; >2 is impossible with
that initialized ammo. Do not alter constants, probability, release cutoff,
bullet movement, carrier collision, route or Zapper ordering.

Arming must be independent of the direct flag so future natural Level 3 uses
the identical path. The combined isolated diagnostic continues to arm both
Tanker and ordinary/cargo Flippers as today; default normal Level 1/2 remain
silent. Optional `--level3-test --enemy-bullets-test` adds development Flipper
bullets, not a different mixed selection policy.

### Direct terminal mode (C3b only)

- Add `--level3-test` to DebugController parsing/conflict checks and constructor
  conflict validation. Exclusive with geometry/Level2/Tanker/Spiker/Fuseball/
  Pulsar scenarios; lives and optional enemy-bullet flag remain composable.
- Program routes to the completed production configuration/model, labels the
  window as direct Level 3 development access, and preserves input routing.
- Renderer shows all actual enemy bullets during non-exit play, not only when
  the optional flag is true. Its existing line/projector helper remains; no
  new fidelity algorithm. Default old modes have no new bullet entities.
- HUD must derive mixed remaining obligations from ledger states, distinguish
  originals/cargo/active, include Tankers and label the production-shaped test.
  Never print "FLIPPERS 15" as if materialized cargo were absent.
- Geometry flag stays Flipper-only, with its current diagnostic schedule,
  wording and replay behavior. Merely sharing plus-board definition does not
  share its composition override.
- Completion travels through the standard exit, then terminal Level 3 test
  complete; R replays a fresh randomized wave using existing replay semantics
  (score/lives retained at successful replay). Game Over R performs existing
  full restart semantics (score zero, configured starting lives). Death keeps
  current production Level/configuration and unresolved identities.
- `NaturalProgressionEnabled` false for this mode. No edits to the
  LevelProgression resolver or Level-2 successful-exit commit condition.
  Normal campaign remains Level 1 -> Level 2, terminal at Level 2.

Only after implementation:

```powershell
dotnet run --project .\src\Tempest.Game -- --level3-test --lives 10
```

Optional development Flipper-bullet regression:

```powershell
dotnet run --project .\src\Tempest.Game -- --level3-test --enemy-bullets-test --lives 10
```

Neither command is implemented or launched during this analysis.

## 11. Existing protection and new automated acceptance matrix

Current tests already protect significant **diagnostic**, not production,
behavior:

- [C2 whole climb/bounds/late windows](../../tests/Tempest.Game.Tests/SimulationTests.cs#L1988):
  strict scripted zero/one/two outcomes, late second shot, independent bullets,
  timers, cutoff and pending-zap firing.
- [Diagnostic carrier/cargo outcomes and death](../../tests/Tempest.Game.Tests/SimulationTests.cs#L2553):
  fixed slot 0, original Flippers, live child route/activation/scoring,
  fresh carrier and independent child reconstruction including repeated death.
- [Zapper tests](../../tests/Tempest.Game.Tests/SuperZapperTests.cs#L349):
  shot/natural preemption, carrier expiry/two child timers, child timer
  replacement, partial/repeated reconstruction and no stale rim score.
- [Geometry tests](../../tests/Tempest.Game.Tests/Level3GeometryTests.cs#L22):
  exact diagnostic board/config, conflicts/replay/death, directed geometry,
  finite headless projections and diagnostic supply accounting.
- [Progression tests](../../tests/Tempest.Game.Tests/LevelProgressionTests.cs#L14):
  natural Level1->2 boundary/carry, terminal old scenarios and reset.
- Input tests validate mappings/router logic; they are not USB hardware tests.

They do **not** establish production random type selection, persistent selected
types, multi-owner latent records, six-cap re-entry, source-shaped production
schedule or independent production arming/visibility. Single-carrier passing
tests cannot prove these by multiplying diagnostic supply.

| Gate | Required deterministic scenarios/assertions | Checkpoint |
|---|---|---|
| Config | Exact proposed identity/board/shared points/supply/tuning/charges; diagnostic remains Flipper-only; effective scheduling policy explicit | C3a |
| Schedule | Four initial, eleven reserve; up-to-six at step50, no four-Egg ceiling; random lane/direction bounds; eight-step approach, strict <9 and <2 gates, >95 boundary/near-end; freeze/reset; no changes old schedules | C3a |
| Type choice | Rejected 0/2/3, negative clamp; inclusive bounds; first near-end discarded draw and retry sequence; cargo retry1/2 until0; only Flipper/Tanker | C3a |
| Counts/cap | Script zero, one, several, six overlapping actual game carriers; seventh unselected selects Flipper at cap; later Tanker after natural/shot/zap removal; >6 sequential permitted | C3a |
| Selected re-entry cap | Selected Tanker retains pair/type, waits ready at six without RNG reroll; other ready Egg can hatch; no hatch-loop hang; capacity restores | C3a |
| Original coverage | All 15 IDs in exactly one state/location, including mixed reserve/Egg/active/terminal sets; deliberately missing+extraneous equal-count IDs and duplicates rejected | C3a |
| Cargo coverage | Owner/ordinal uniqueness; sparse overlapping integer namespaces; latent under reserve/Egg carrier, pending activation, re-entry reserve/Egg, active, killed/rim; pair allocated once | C3a |
| Natural | Strict release boundary, escaped carrier/no200; two live existing IDs; next-step movement and accepted outward route/inward pivot; bullets survive | C3a |
| Shot | One200, two queued live IDs; killing shot cannot hit own new children; other same-step shots/activation ordering; pending carrier zap canceled | C3a |
| Zap | Multiple carriers with interleaved Flippers/bullets, correct4/6/... timers; each pair timer8/new-step7; exact per-ID200/150; no global scheduler changes | C3a |
| Second charge/preemption | Existing literal selector with several carriers/children, timer replacement; shot/natural-vs-expiry boundaries; no duplicate pair/score/stale timer | C3a |
| Overlap | One carrier natural, another shot, third zap; distinct terminal originals/pairs/scores and independent bullets | C3a |
| Death | Before release, after natural, after shot, after zap, during child timers, one terminal sibling, multiple carriers, queue/Egg child, two consecutive deaths; equality of all identity/type/terminal sets | C3a |
| Completion blockers | One fixture per reserve original, original Egg, below-rim original Flipper/Tanker, latent child, activation child, re-entry reserve/Egg child, below-rim active child; no completion if missing representation | C3a |
| All-rim | Kill/rim mixtures, active exact-set match, at-rim pending target zero later score; latent carrier blocks; no natural/rim kill score; exit cleanup | C3a |
| Firing integration | Production armed without flag; ordinary/cargo zero ammo without flag; unchanged C2 strict random paths/constants; isolated silent/combined unchanged; fresh draws on carrier re-entry | C3a |
| Fresh/terminal model | Exit ends terminal Level3; fresh replay/new game clears old ledger/enemies/timers; death preserves same config; normal1->2 stays terminal2 | C3a |
| CLI/UI routing | New flag/conflicts/lives/optional bullets, constructor exclusivity, direct config identity, title/HUD/replay labels; geometry diagnostic unchanged | C3b |
| Bullet visibility | Headless draw-policy/callback seam proves production bullets selected without optional flag; finite geometry via existing renderer callbacks | C3b |
| Regression | Level1/2, isolated Tanker, directed/inward geometry, C2 firing, Spiker/Fuseball/Pulsar/Zapper/all-rim, input/controller aliases/diagnostic | Both |

Use existing injected Random and strict finite range-aware scripts, extending
their enumerated draw ranges only as needed. Force **test** choices, not
production/manual composition. Do not use statistical assertions that a
particular unseeded run must include a Tanker. Do not add artificial live-count
controls or USB/graphical "automation" that cannot prove the requested behavior.
Headless projection/callback tests are not visual fidelity acceptance.

Suggested focused groups after each implementation, adjusted to actual added
class names, with xUnit v2/VSTest syntax:

```powershell
dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~Level3MixedWave|FullyQualifiedName~Level3WaveLedger|FullyQualifiedName~Tanker|FullyQualifiedName~Cargo|FullyQualifiedName~SuperZapper|FullyQualifiedName~AllRim|FullyQualifiedName~Level3Geometry|FullyQualifiedName~DirectedRotation|FullyQualifiedName~LevelProgression"
dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~LevelConfiguration|FullyQualifiedName~Level2|FullyQualifiedName~InputSystem|FullyQualifiedName~InputDiagnostic"
dotnet test TempestCSharp.slnx --no-restore
dotnet build TempestCSharp.slnx --no-restore
git diff --check
```

Record actual results, not old totals. C2 finalization reported 141 focused,
467 full and zero-warning/error build; these are **historical C2 results**.
No C3 tests/build were run for documentation-only analysis.

## 12. Qualitative manual gate after C3b

Use default direct `--level3-test --lives 10` first, without the Flipper-bullet
option, so Tanker arming/visibility is actually tested independently.

1. Confirm plus outline, rim movement/wrap and accepted inward ordinary/cargo
   pivots. Reflex route keeps its long duration/opportunity, not a90 shortcut.
2. Play/replay randomized waves; observe Flippers and Tankers when naturally
   present and variability across runs. A zero-Tanker run is allowed, not fail.
3. Observe Tanker separate shots while climbing and no release burst. When
   naturally seen, confirm multiple shots before release; not every carrier
   must do this. Existing bullet survives carrier removal. Flippers remain
   silent without the option.
4. Shoot a Tanker: two live children in distinct non-Tanker lanes, accepted
   outward route/inward visible pivot, stable below-rim direction; carrier
   points appear once. Other existing shots cause no strange cargo behavior.
5. Let another Tanker release naturally: live children, no carrier kill score.
6. Lose a life before release and after materialization/partial resolution;
   fresh unresolved entrants, no visible duplicate carriers/children or
   resurrection of resolved units. Hidden set equality is automated evidence.
7. Use first and second Zapper charges; observe delayed carrier/child behavior,
   no obvious duplicate points, and sensible behavior after a pending death.
8. Observe several simultaneous carriers if naturally encountered; do not
   demand six or manually force capacity. Deterministic tests cover that cap.
9. Exhaust incoming entrants and let all qualifying survivors reach the rim;
   disappear/exit without kill points and without leaving orphan children.
10. Confirm terminal Level3 test, successful R replay, GameOver R restart,
    correct Level3 display and no unintended campaign advance.
11. Verify spinner/controller/keyboard/restart/quit remain normal, and no
    obvious rendering artifact/crash. These are manual hardware/visual checks.
12. Regression spot-check silent isolated Tanker, combined armed Tanker and
    geometry diagnostic, then normal Level1->2 still terminal2.

Do not demand exact timer measurements, a guaranteed Tanker count, every
preemption race or six naturally concurrent carriers. Missing random outcomes
are recorded as not observed, not inferred from source or forced in a manual
launch. If a meaningful visible mismatch arises, stop before acceptance.

## 13. Exact next implementation scope and checkpoint gates

### C3a: authorized later, complete headless model

Expected changes, bounded by actual need:

- [LevelConfiguration](../../src/Tempest.Game/Levels/LevelConfiguration.cs):
  distinct mixed composition/config using existing plus definition; explicit
  production scheduling policy, not copied board data.
- [Level](../../src/Tempest.Game/Levels/Level.cs) and
  [Egg](../../src/Tempest.Game/Gameplay/Egg.cs):
  production entrant queues/scheduling and hatch eligibility, preserve old
  modes; identity references and selected-carrier capacity waiting.
- One small proposed Level-3 ledger file under the existing Levels area;
  original states/types and owned latent/materialized child transitions only.
- [TempestGame](../../src/Tempest.Game/TempestGame.cs):
  configuration-based headless entry, composition-gated dispatch/arming,
  production ledger lifecycle, representation/completion checks and
  reconstruction; leave isolated accounting externally unchanged.
- [Flipper](../../src/Tempest.Game/Gameplay/Flipper.cs) only if necessary for
  identity wiring; no motion/firing/pivot changes.
- [Tanker](../../src/Tempest.Game/Gameplay/Tanker.cs) only for directly related
  development-only API wording if reused; no executable firing/timing/route
  tuning required.
- Focused mixed-wave/ledger tests plus directly related existing test fixtures.
  Do not alter SuperZapperSystem semantics or global score/input/rendering.

A public/internal configuration-based entry should execute the same gameplay
model C3b will launch; do not make a separate test simulation or a test-only
fixed composition. Use the repository's access conventions rather than casts
or inaccessible internal setters. All existing constructors/flags stay valid.

C3a gate: deterministic acceptance matrix through actual orchestration,
complete ID/type/terminal equality including two deaths, six concurrent
carriers and all resolution races, full regression test/build/whitespace
results. Existing manual modes remain available for regression checks; new
direct Level3 manual acceptance awaits C3b. No provisional incomplete ledger
is exposed. Report C3a as headless groundwork, not completed C3.

### C3b: access/visibility/manual checkpoint

Expected files:
[DebugController](../../src/Tempest.Game/Debug/DebugController.cs),
[Program](../../src/Tempest.Game/Program.cs),
[VectorRenderer](../../src/Tempest.Game/Rendering/VectorRenderer.cs),
[TempestGame](../../src/Tempest.Game/TempestGame.cs) only launch/conflict wiring,
focused access/render-policy/replay tests and directly related documentation.
The completed C3a policy must not fork by direct/manual access.

C3b gate: automated access/visibility/regressions/full solution gates, then
Neal's manual checklist PASS, accurate evidence documentation, separate clean
checkpoint under Neal's commit control. If lifecycle changes prove necessary,
explicitly reopen the C3a gate rather than silently burying them in UI glue.

README/development notes may describe implemented access after authorization;
behavior spec additions follow actual accepted observations and retain
reference/project classifications. None is edited in this analysis.

### Explicit exclusions for both

No natural Level2->3 resolver/commit edge, C4, Level4, mixed old levels, global
death pacing, bonuses/intro/palette cycles, rendering fidelity algorithm,
spinner/controller remapping, Flipper-bullet default promotion, cargo route/
inward pivot change, Zapper semantics, C2 constants or reference edits.

## 14. Uncertainties and GO/NO-GO limits

- Actual Atari Level3 selection weights/caps/scheduling internals remain
  unknown. The reference-shaped production proposal is engineering evidence,
  not ARCADE VERIFIED composition.
- Exact Atari Tanker ammo/delay/probability/interval/cutoff/per-carrier shot
  count remain unknown. Two-before-release is a validated capability.
- Literal 1.75 remains REFERENCE VERIFIED; global60FPS versus Incitti nominal
  16ms and C# pipeline/list-enumeration differences are not retuned.
- Type retention, latent IDs, queue priority, fresh stillborn-child re-entry
  and identity conservation are explicit PROJECT DECISIONS, not arcade memory.
- Source Egg burst/approach behavior is a useful comparator; the proposed
  active-play local clock/reconstruction/pipeline are deliberate C# differences.
  The implementation prompt must explicitly carry this scheduling choice.
- Exact Atari route/pause/pivot math, release timing, Zapper delays and
  all-rim hidden state are not upgraded beyond Neal's visible validations.
- Flipper bullets remain a separate unpromoted question.
- No production Level3 manual composition/arming/death/all-rim result exists
  yet; C3b is the future gate, not a pass inherited from isolated C2.

**GO: plan C3a as the next separately authorized implementation.
NO-GO: execute implementation now, call C3 accepted, or start C4.**
There is no current repository blocker; remaining gates are the future
implementation/tests/manual acceptance and explicit authorization.

## 15. Deliverables and final maintenance

Only these ignored deliverables were created:

- [Exact C3 prompt](../Prompts/2026-11-01-c3-level3-mixed-wave-preimplementation-analysis-prompt.md)
- This report.

The attachment is read-only. Prompt text is verified against it; file encoding/
newline conventions are checked separately from text equality. AI-Logs is
ignored by the existing repository rule, not force-added.

Final requested maintenance was executed successfully:

```text
git status --short
(empty)
git diff --check
(empty; exit 0)
git diff --stat
(empty)
git status --short -- Reference/incitti
(empty)
git log -1 --oneline
6d15165 Correct and validate isolated Tanker firing timing
git ls-files --others --exclude-standard
(empty)
```

Both new deliverables are confirmed ignored by `git check-ignore`. There are
no tracked changes or unexpected untracked files. Initial prompt verification
found only an extra terminal CRLF introduced by file creation. A byte-preserving
copy of the read-only attachment removed that difference; source and saved
prompt now have identical SHA-256:

```text
B22E43BE1D446CF1B1BC514B61C4BEEBA606FE12E1C68D442F74F13F7E61CE7A
```

No source/tests/project/README/spec/development notes/reference files changed.
No commit or push. **Natural Level 2 -> Level 3/C4 was not started.**
