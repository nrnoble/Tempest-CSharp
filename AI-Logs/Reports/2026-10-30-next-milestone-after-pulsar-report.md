# Next Tempest Milestone After Pulsar — Analysis

Checkpoint: `37535eb Implement Pulsar behavior and add behavior specification`

Scope: analysis only. The prompt and this report are the only requested
deliverables. No gameplay, test, README, behavior-specification,
project/configuration, or Git changes were made. No build or test run was
needed for this analysis.

## Recommendation

**Implement Incitti's `rimit` hurry-up behavior for the existing direct-hatched
Fuseball and Pulsar development scenarios.**

Keep it one focused mechanic: latch the source's end-of-Egg-supply hurry-up
condition, then pass that state to the two already implemented enemies. Do not
change the recreation's level-clear condition, enable new enemy compositions,
or add a new level.

This is the best *small* next milestone because the relevant enemy state
machines already exist and are independently testable. It adds one shared
late-wave rule, has no new renderer, score, or enemy-ledger concept, and can be
exercised in the existing `--fuseball-test` and `--pulsar-test` scenarios. It
improves the pacing and end-of-supply behavior of those playable scenarios,
although it deliberately does not change ordinary Flipper-only Level 1.

Super Zapper would have greater immediate defensive impact, including in
ordinary Level 1, but its source behavior is not a small input-only feature:
it needs different target selection on each charge, delayed per-enemy death
and scoring, a Tanker stillborn-cargo path, and web flashing. It is the next
larger shared-system milestone, not the next small one.

## Current project facts and evidence boundaries

- **PROJECT DECISION** — Normal startup remains a fixed ten-unit Flipper-only
  Level 1 on one closed tube. `Level` loads only the current Level 1 definition;
  completion ends at Level Complete rather than advancing.
- **DEVELOPMENT ONLY** — Spiker, Tanker, Fuseball and Pulsar behavior is
  selected through isolated development scenarios on that same tube. Direct
  Fuseball and Pulsar hatching is artificial composition, not evidence for
  their normal arcade eligibility or wave makeup.
- **REFERENCE VERIFIED** — The current direct-hatched Pulsar uses the source's
  first eligible-level movement parameters; Fuseball uses the unscaled 3.3
  climb term. Neither setting establishes arcade timing.
- **ARCADE VERIFIED** — Existing manual observations cover selected Fuseball
  and Pulsar behavior in those isolated scenarios. They do **not** validate
  `rimit`, its trigger, or its timing. The direct Flipper rim-arrival rule is
  also implemented from Incitti and tested in C#, but is not separately
  arcade-verified; only the completed-pivot rule has that evidence.
- The C# `SuperZapperSystem` is empty. `InputSystem` currently samples fire,
  spinner delta, and left/right movement, not Space or the right mouse button.
  Flipper/Tanker enemy bullets are implemented behind development options;
  Tanker firing requires combining `--tanker-test` and
  `--enemy-bullets-test`.
- The current fixed ten-unit completion/conservation rule is an explicit
  **PROJECT DECISION**. Preserve it; do not copy source exit logic as an
  accidental side effect of implementing hurry-up.

Historical reports are useful for locating decisions, but do not override the
current behavior specification or constitute arcade evidence. In particular,
the earlier next-milestone analysis's description of Super Zapper recharge as
"per level/life start" is too broad: literal Incitti code resets two charges on
new-game/level setup, not on ordinary life reconstruction. Also, older Tanker
README text saying Tankers do not fire is stale relative to the later
implementation; no documentation was changed in this analysis.

## Candidate comparison

The evidence label in each row describes the behavior being considered, not an
assumption that source behavior is arcade truth.

| Candidate | Prerequisites, complexity, and behavior dependencies | Rendering, accounting, death/re-entry, and progression | Deterministic/manual validation, evidence, value, and decision |
|---|---|---|---|
| **Super Zapper** | Medium-high. Requires input, two-charge state, first/all and later/single-target dispatch, and delayed zap resolution/scoring for every supported enemy. Tanker zap also needs stillborn cargo. `SuperZapperSystem` is empty and `Enemy` has no zap contract. | No charge counter is drawn by Incitti, but the tube/web flashes for 60 updates after an all-zap or successful single-target zap. Pending enemies must remain accounted for until their delays expire. Zapped Tankers release cargo that dies later; current Tanker identity/accounting must represent that. Charges persist through death, and pending targets are discarded/re-entered under the current C# death policy. No progression is needed to test it. | High deterministic testability with injected target order and delays. Existing enemy scenarios can validate it without a complete progression system, including Tanker cargo in Tanker mode. **REFERENCE VERIFIED**, not arcade-verified. High gameplay value, but too cross-cutting for the next *small* milestone; defer as the next larger shared system. Exact source behavior is detailed below. |
| **`rimit` / hurry-up** | Low-medium. Needs a latched state derived from exhausted reserve/active Eggs plus active enemies still below the rim; pass it to the existing Fuseball and Pulsar update rules. No other new subsystem. | No new rendering, score or enemy accounting. The condition must not alter the current completion rule. Incitti leaves the flag set through life reconstruction and clears it on new level setup; follow that source lifecycle explicitly. No progression dependency. | Deterministic trigger and state-machine boundary tests; manually observable in the existing isolated Fuseball/Pulsar scenarios. **REFERENCE VERIFIED** rules; the scenarios remain **DEVELOPMENT ONLY** and arcade hurry-up behavior is unverified. Small, testable improvement to late-wave pacing. **Recommended.** |
| **Begin real level progression/composition** | High. Needs more than another hatch type: level identity/current definition, next-level transition, Egg supply scaling, per-level tuning, selection gates, reset/bonus/charge rules, and HUD/renderer coordination. Current `Level` owns a fixed ten-unit ledger and fixed Level 1 definition. | New board geometry/topology and likely palette change affect rendering. Dynamic enemy totals affect accounting and re-entry. Completion must transition rather than replay. Super Zapper charges and bonuses become level-transition concerns. | Unit-testable in parts, but an end-to-end progression test must clear a wave and prove next-level setup. Manual validation is possible only through a longer full-wave clear. Source level progression is **REFERENCE VERIFIED**; authentic Atari correspondence is not established. Most useful later, but not small now. |
| **Level 2 / `canflip`** | Medium as an isolated Level 2 feature, high if called real progression. The Level 2 Flipper branch requires mid-tube edge changes, randomized direction/state transitions, and correct pivot/shoot/collision handling; current Flippers only climb and pivot at the rim. It also needs Level 2 identity/tuning and its actual geometry if presented as progression. | Reuses the Flipper renderer's pivot geometry, but mid-tube movement changes collision/render timing. Basic one-Egg/one-enemy accounting and death reconstruction can remain. A true next-level path is still required to reach it naturally. | Deterministic random-seam tests are feasible; an artificial Level 2 scenario can isolate movement but does not validate progression. **REFERENCE VERIFIED**: source disables `canflip` only at `lv=0` and enables it after that. No arcade validation of this gate. Valuable first progression step, but bundling it into the current milestone would expand scope. |
| **Tanker-carried Pulsars** | Medium-high. Requires progression eligibility, Tanker cargo selection/release to create Pulsars at carrier height, cargo identity, and a correct carrier-to-child transfer. Direct Pulsar movement exists, but its current development wave does not model cargo. | Pulsar rendering can be reused. Tanker cargo adds enemy accounting and death/re-entry cases; zap-death adds the stillborn-cargo interaction. Natural/shot release must remain distinct. Source first enables this carrier type at `lv > 26` (displayed Level 28), so a normal implementation depends on progression. | Deterministic in a dedicated artificial carrier scenario; manual isolation is possible but does not validate Level 28 composition. **REFERENCE VERIFIED** gate/cargo path, no arcade proof. Adds little before progression and Zapper integration; wait. |
| **Tanker-carried Fuseballs** | High. Same cargo/progression/identity requirements as Pulsars plus Fuseball state, lateral position and height-specific scoring on release. This is a separate late source gate (`lv > 31`, displayed Level 33). | Reuses Fuseball art but must render at cargo height and preserve its own scoring popup. Adds dynamic child accounting, reset/re-entry and stillborn-zap scoring. Depends on Tanker behavior and progression; zap consequences add further coupling. | Deterministic in a dedicated artificial scenario, but full source-level manual validation depends on progression. **REFERENCE VERIFIED** source path only. Not a small next step; defer behind progression and Tanker/Pulsar integration. |
| **Enemy-bullet activation/promotion** | No substantial code prerequisite: shared bullets and Flipper, Spiker, and Tanker firing paths already exist behind flags. Promoting Flipper fire into normal Level 1 is mostly a policy/evidence decision. | Existing renderer and projectile lifecycle are reused; bullets do not affect enemy accounting. Death clears them under the project policy. Enabling them changes normal-level hazard/death behavior, not progression. | Deterministic tests and manual test scenarios already exist. **REFERENCE VERIFIED** firing rules; **DEVELOPMENT ONLY** current opt-in use; normal arcade firing/cadence is not verified. Do not promote until arcade comparison; it is not the next engineering milestone. |
| **Remaining Spiker/Tanker gaps** | No single remaining small behavior bundle. Spiker retreat bullets and Tanker bullets have now been implemented in their opt-in paths. Remaining work includes arcade validation/tuning and zap-related death; source Tanker cargo variants are later progression work. | Spiker spikes persist and are not ordinary enemy slots; Tanker children have special dynamic accounting. Zapper integration touches both and introduces delayed resolution. No rendering/accounting change is justified merely to claim these enemies "complete." | Existing opt-in scenarios allow manual checks, but they are not authentic composition. Source-backed implementation is **REFERENCE VERIFIED**; assigned speeds/isolated waves are **DEVELOPMENT ONLY**. Separate manual validation where access allows; do not open a broad “finish enemies” milestone. |
| **Flipper direct-rim-arrival arcade validation** | No implementation prerequisite. Current C# behavior already signals death on direct arrival in the occupied lane; the issue is evidence, not missing code. | No planned rendering/accounting/progression change. A contradictory arcade observation would require a focused behavioral correction and regression tests. | The C# unit/integration tests prove recreation behavior only. Manual arcade observation is the only validation route; the distinct pivot rule is already **ARCADE VERIFIED**, while direct arrival is only **REFERENCE VERIFIED**. Important to check before promoting stronger arcade-accuracy claims or changing later Flipper behavior; not a code milestone. |
| **Other small missing core mechanic** | `rimit` is the strongest newly actionable omission because both affected enemy types now exist. Other source gaps such as bonuses, audio and open-tube geometry have broader prerequisites or lower immediate value. | Hurry-up adds no visual asset, points, count, level geometry, or framework. Preserve the existing completion/death policies. | Its trigger and effects admit exact, deterministic simulation tests and existing scenario-based manual checks. This is why it outranks a speculative abstraction or a larger cross-cutting system for the next small step. |

## Why `rimit` is the smallest useful next milestone

Literal Incitti behavior is explicit in `Reference/incitti/tempest.bmx`:

- The main loop sets `rimit = True` after reserve Eggs and active Egg entries
  are exhausted while enemies remain below the rim (`tempest.bmx:3805-3823`).
  `onrim` is reset/recomputed from active baddies; a baddie below depth 400
  makes it false (`tempest.bmx:2184-2207`). The source also uses `onrim` in
  level-exit eligibility. That exit rule is **not** part of this proposal.
- A Fuseball at a lateral boundary is forced into its climbing state when
  `rimit` is set or its height is zero, and its climb adds the source climb
  term twice when `rimit` is true (`tempest.bmx:3393-3401`). With the current
  development speed of 3.3, the source-derived development effect is 6.6 per
  climbing update, not a new speed constant.
- At its top endpoint, a Pulsar takes the flip branch when the source's random
  choice succeeds **or** `rimit` is true (`tempest.bmx:3060-3074`). This
  prevents the ordinary choice to descend; it does not double Pulsar's
  one-unit vertical movement speed.
- `rimit` is cleared in `SetUpEnemies` for a new level (`tempest.bmx:4253`),
  not in the death conversion path. The source therefore keeps hurry-up active
  through same-level death/re-entry. This lifecycle is **REFERENCE VERIFIED**,
  not yet arcade-validated.

For the C# milestone, compute/latch the condition only when the reserve and
active Egg queues are empty, there is at least one active enemy, and at least
one active enemy is below `Level.Depth`. Apply the latched value only to the
existing Fuseball/Pulsar behavior. The game has no normal progression today,
so clear it when a new game/replay initializes the level; preserve it through
ordinary life reconstruction to match the source rule.

The source couples `rimit` with an exit condition that can start exit when all
remaining enemies are already on the rim. Do **not** copy that behavior: the
current specification intentionally requires the ten-unit supply to be
destroyed before this recreation exits. Keep `Level.IsComplete`, kill counts,
and exit timing unchanged. `rimit` should accelerate/redirect enemies only; it
must neither award points nor resolve enemy units.

This is not a claim that the current direct-hatched development waves represent
the source's Level 11 Fuseball or Level 17 Pulsar composition. Their scenarios
are simply controlled places to observe the new mechanic without first
building progression.

## Why Super Zapper should follow, not precede, this small milestone

Super Zapper is a strong subsequent milestone and does not inherently require
full level progression. Existing `--fuseball-test`, `--pulsar-test`,
`--spiker-test`, and `--tanker-test` modes could each validate it in isolation.
However, a faithful implementation is materially wider than adding Space input.
The exact Incitti behavior to preserve is:

1. **Activation/input — REFERENCE VERIFIED.** `Player.Update` accepts
   `Space` or right mouse button, only when the player is not dying and a charge
   remains (`tempest.bmx:659-665`). Current C# input has no zap action. On an
   accepted press, dispatch uses the current strength and decrements the charge
   even if the single-target search finds nothing.
2. **Charges/recharge — REFERENCE VERIFIED.** There are two charges. The first
   accepted press uses strength 2; the next uses strength 1. The source
   initializes/recharges to two when a new game/level is set up
   (`tempest.bmx:157`, `1737-1740`, `1429-1431`). It does **not** replenish
   charges on ordinary life loss. A full reset/new game does initialize the
   new level's supply.
3. **First use / target set — REFERENCE VERIFIED.** Strength 2 attempts to zap
   every baddie in shared-list order except spikes. Bullets can be visited by
   that pass, but their `ZapIt` returns false, so they are unaffected. Each
   successful enemy zap receives a delay starting at 4 updates; the delay
   advances by 2 only after a target accepts it, producing 4, 6, 8, ... for
   accepted targets (`tempest.bmx:2257-2270`, `2327-2387`).
4. **Subsequent use / selection — REFERENCE VERIFIED.** Strength 1 searches
   enemy types (`baddietype > 1`), excluding bullets and spikes. It uses
   circular lane distance on a continuous tube and a height/distance/type
   comparison heuristic, then calls `ZapIt(4)` on one selected threat
   (`tempest.bmx:2271-2313`). It is not simply “kill the nearest enemy” or
   “kill the highest enemy”; deterministic tests must pin the literal
   comparison and source iteration order.
5. **Delayed effects and scoring — REFERENCE VERIFIED.** `ZapIt(t)` sets a
   per-enemy death timer; enemies continue their ordinary update while it
   counts down. At expiry, the enemy-specific update removes the enemy and
   awards the appropriate score: Flipper 150, Tanker 200, Pulsar 200,
   Fuseball's height-dependent 250-point bands with its score popup, and
   Spiker/Spinner 50. Accounting resolves at expiry, not at zap activation.
   The literal handlers are at `tempest.bmx:2551-2588`, `2727-2745`,
   `3025-3048`, `3328-3350`, and `3616-3635`.
6. **Tanker cargo — REFERENCE VERIFIED, high integration risk.** A Tanker
   reaching zap expiry awards 200, releases cargo with `stillborn=True`, and
   removes itself (`tempest.bmx:2551-2568`). Released children receive an
   eight-update death timer (`tempest.bmx:2467-2505`). They remain separately
   represented during that interval and then resolve through their own delayed
   scoring/removal path. In the current C# Tanker development ledger, a
   Tanker slot and its latent cargo are distinct; a zap path must materialize
   stable cargo identities, preserve the original-slot resolution, award the
   children's later scores, and handle player death during the delay without
   duplicating or losing cargo.
7. **Affected and unaffected objects — REFERENCE VERIFIED.** Flippers,
   Tankers, Pulsars, Fuseballs, and Spikers accept zap. Spikes are excluded
   from the all-target pass and reject `ZapIt`; bullets reject `ZapIt` and are
   not eligible for the single-target pass. Thus zap does not clear
   projectiles or persistent spikes.
8. **Death/re-entry and completion.** The source does not reset charges during
   life reconstruction. A pending zap is still an active enemy until expiry;
   if player death converts it back to Eggs first, it is unresolved rather
   than already scored. The C# project freezes active gameplay throughout its
   60-update death interval, then reconstructs unresolved enemies as fresh
   Eggs and preserves earned score. Keep that **PROJECT DECISION**. Completion
   must wait for delayed enemy/cargo resolutions and cannot be triggered by
   bullets or spikes.
9. **HUD/rendering.** The inspected source has no numeric charge counter in
   the gameplay HUD. It does show the Space=Zap input hint, and toggles the
   tube/web line color for 60 updates after the all-target zap, or after a
   successful single-target zap (`superzapperdisplay` at
   `tempest.bmx:2261`, `2317`, `1445`, and the draw branch around
   `tempest.bmx:1219-1249`). C# would need the flash in `VectorRenderer` and
   input wiring; adding visible charge icons would be a **PROJECT DECISION**,
   not a source requirement.
10. **Manual isolation without progression.** After implementation, each
    existing enemy-development scenario can exercise the common input and
    flash. Pulsar/Fuseball modes cover their score/death paths; Tanker mode
    must validate stillborn cargo and its ledger; Spiker mode confirms spikes
    survive. The Egg reserve allows enemies that hatch after the first use to
    exercise the second targeted use. This can all be tested on the current
    tube, but the breadth and Tanker accounting work make it a larger
    milestone than `rimit`.

All of those source rules are **REFERENCE VERIFIED** only. The development
compositions remain **DEVELOPMENT ONLY**; no Atari Zapper behavior is claimed
as **ARCADE VERIFIED** here.

## Why the other candidates should wait

- **Progression and Level 2 / `canflip`:** They are the most valuable route to
  authentic playable waves, but the existing program cannot advance beyond its
  fixed Level 1 definition. Proper progression needs geometry, level identity,
  variable enemy totals, per-level parameters and exit/restart policy, not just
  a `canflip` boolean. Level 2's source gate (`lv=1`, displayed Level 2) is
  **REFERENCE VERIFIED** (`tempest.bmx:4266`); whether it matches Atari is
  unverified. A separate Level 2 development scenario would test the Flipper
  branch but would not itself be real progression. Do progression after small
  cross-enemy mechanics are settled, and do not conflate Level 2 with all later
  enemy composition.
- **Tanker-carried Pulsars/Fuseballs:** These need actual progression gates
  and cargo identity/re-entry work; the direct enemy implementations do not
  imply cargo support. Tanker Pulsars first appear in the source at displayed
  Level 28 (`lv > 26`); Tanker Fuseballs at displayed Level 33 (`lv > 31`).
  Both are **REFERENCE VERIFIED** gates only. Wait for progression and the
  appropriate carrier/Zapper accounting.
- **Enemy-bullet promotion:** The projectile and all requested source firing
  paths are already coded in opt-in modes. Enabling normal Level 1 Flipper
  bullets is primarily a manual arcade-acceptance decision, not a fresh
  engineering milestone. Keep the current launch unchanged until that
  comparison is made.
- **Other Spiker/Tanker gaps:** The remaining meaningful source behavior is
  either the larger delayed-zap/cargo path or arcade tuning/validation. Avoid
  bundling those into a vague “finish Tankers/Spikers” task.
- **Direct Flipper rim arrival:** The source says a climbing Flipper reaching
  the occupied lane is lethal; C# and tests implement that. It remains an
  unresolved arcade-validation implication, not a code gap. Separately test
  direct arrival on Atari before claiming it as **ARCADE VERIFIED** or using it
  as evidence for progression. The already arcade-verified pivot-completion
  behavior must remain distinct.

## Expected files for the recommended implementation

Expected, not edited in this analysis:

- `src/Tempest.Game/TempestGame.cs` — own the hurry-up latch, evaluate its
  source-backed trigger after active enemy/Egg updates, preserve it across
  ordinary death reconstruction, and clear it on new-game/replay level setup.
- `src/Tempest.Game/Gameplay/Fuseball.cs` — accept the hurry-up state; force a
  climb at lateral boundaries and apply the second climb-speed increment.
- `src/Tempest.Game/Gameplay/Pulsar.cs` — accept the state; force the source's
  top-end flip choice without changing the existing vertical speed or shared
  pulse clock.
- `tests/Tempest.Game.Tests/SimulationTests.cs` — deterministic trigger,
  latch/reset/re-entry, Fuseball and Pulsar transition, and regression tests.
- `README.md` and `TEMPEST-DEVELOPMENT-NOTES.md` — document the opt-in
  scenarios' hurry-up behavior after the implementation is validated.
- `TEMPEST-BEHAVIOR-SPEC.md` — update only after implementation **and** manual
  comparison, as described below.

No new command-line flag, entity type, shared abstraction, or renderer change
should be needed. Prefer a simple boolean/read-only state passed to the two
existing update methods over a generic hurry-up subsystem.

## Automated-test strategy

1. Verify the trigger is false while reserve Eggs remain, active Eggs remain,
   no active enemies remain, or all active enemies are at the rim.
2. Verify it latches only after both Egg sources are exhausted and an active
   enemy remains below `Level.Depth`; verify it does not change
   `EnemiesDestroyed`, score, reserve counts, or the current completion rule.
3. Drive a Fuseball at a lateral boundary with controlled random results:
   hurry-up selects climbing even when the ordinary phase roll would descend.
   On subsequent climb updates, assert the source term is applied twice (6.6
   for the current 3.3 development speed), including clamping at the rim.
4. Drive a Pulsar at the top endpoint with a random result that would normally
   descend; with hurry-up, assert the lane-flip branch. Assert its climb and
   descent increments remain one, and that the pulse oscillator is unchanged.
5. Verify hurry-up survives the existing player-death freeze and same-level
   Egg re-entry, then resets on full game/replay initialization.
6. Run existing completion, scoring, death, Egg conservation, Flipper,
   Fuseball and Pulsar regression tests. Tests establish C# behavior, not
   arcade accuracy.

## Manual-validation strategy

Use the existing, mutually exclusive scenarios:

```powershell
dotnet run --project src\Tempest.Game -- --fuseball-test --lives 10
dotnet run --project src\Tempest.Game -- --pulsar-test --lives 10
```

Let the reserve Eggs and active Eggs drain while keeping at least one enemy
alive. Observe that hurry-up begins only after the supply is exhausted and
enemies remain below the rim. In Fuseball mode, check the forced climb at
lane-boundary decisions, doubled climb rate, rim clamp and resulting player
threat. In Pulsar mode, observe top-end lane transitions instead of descent;
confirm the existing one-unit motion, pulse visuals, scoring, death freeze and
re-entry remain intact. Repeat after a life loss to check the latch behavior.
Compare against Atari Tempest at the relevant late-wave states if accessible.

Because these are artificial all-Fuseball/all-Pulsar waves on Level 1 geometry,
their successful manual behavior check does not validate normal level
composition or prove arcade equivalence. The direct Flipper rim-arrival check
is a separate arcade-validation item.

## Behavior-specification impact after implementation and manual validation

Do not edit the specification during implementation analysis. Once code and
manual checks are complete, revisit:

- **Section 2, Global gameplay / Eggs and completion** — document the
  source-derived hurry-up trigger and explicitly retain the C# kill-all
  completion policy if source exit-on-rim behavior is not adopted.
- **Section 7, Fuseballs** — document forced climbing at lateral boundaries,
  doubled source climb term during hurry-up, and the evidence boundary.
- **Section 8, Pulsars** — document the top-end forced flip decision and that
  vertical speed remains unchanged.
- **Section 10, Development-only scenarios** — note that the existing
  Fuseball/Pulsar test modes can exercise hurry-up but remain artificial
  compositions.
- **Section 11, Known uncertainties/deferred behavior** — remove `rimit` from
  deferred items only if implemented; list exact arcade hurry-up behavior as
  unverified unless directly confirmed on Atari.
- **Section 12, Validation history** — record tests as C# evidence and manual
  observations separately. Only direct Atari observations qualify as
  **ARCADE VERIFIED**; Incitti-derived rules remain **REFERENCE VERIFIED**.

## Explicitly out of scope

- Super Zapper, charges, zap timers, score-on-zap, web flash, and Tanker
  stillborn cargo.
- Level progression, Level 2/canflip, level geometry or palette changes,
  scaled enemy totals, bonuses, or new enemy composition.
- Tanker-carried Pulsars or Fuseballs.
- Promotion of enemy bullets into normal play.
- New Spiker/Tanker tuning or a broad arcade-accuracy pass.
- Changing the existing kill-all completion rule, enemy accounting, scoring,
  rendering, pulse clock, death freeze, or player controls.
- Marking any Incitti-derived behavior as **ARCADE VERIFIED** without direct
  arcade evidence.

## Risks and uncertainties

- `rimit` is a global, latched source state. The exact point at which the C#
  owner should evaluate it must preserve the order of Egg hatching and enemy
  updates and avoid one-tick trigger drift.
- Incitti combines `rimit` with `onrim`-based exit eligibility. This
  recommendation deliberately ports only hurry-up behavior; divergence must
  remain an explicit **PROJECT DECISION**.
- The source persists `rimit` through life conversion. The C# project rebuilds
  unresolved enemies as fresh Eggs and freezes all gameplay during death, so
  the timing of when restored enemies first experience hurry-up must be pinned
  in tests and compared manually.
- Source speed, random choices and level settings are not arcade proof. The
  direct scenarios use fixed ten-unit supplies and development parameters,
  not actual Level 11/17 compositions.
- Hurry-up improves the current enemy-development waves but does not make
  normal Level 1 more varied. The later Super Zapper and genuine progression
  remain important, larger milestones.
- Direct Flipper rim arrival remains arcade-unverified; do not silently
  upgrade its evidence label when documenting this unrelated mechanic.
