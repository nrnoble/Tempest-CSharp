# Next Milestone Analysis After the Tanker/Cargo Milestone

Checkpoint analyzed: `7e25aea Add Tanker gameplay and improve code maintainability`
(clean tree, 146 automated tests at checkpoint).

Labels: **A** = explicitly established by the Incitti source; **B** = inferred by
connecting source paths or by C# inspection; **C** = not established as original
Atari arcade behavior and requiring manual arcade comparison. Incitti source
behavior is never treated as automatic proof of arcade accuracy.

The suspected converging-rim-Flipper issue was intentionally not investigated,
per the task specification.

## 1. Progression Recheck

`SetUpEnemies` re-derives all eligibility flags per level from zero-based
`current_level` (`lv`) and `current_board = lv Mod 16`; the HUD shows `lv + 1`.
**[A]** See [SetUpEnemies](../Reference/incitti/tempest.bmx#L4208).

| Candidate | Incitti condition | First displayed level | Class |
|---|---|---:|---|
| Flipper bullets | no gate; part of Flipper behavior | 1 | A |
| Tanker bullets | no gate; part of Tanker behavior | 3 (with Tankers) | A |
| Spiker (Spinner) retreat bullet | no gate; part of Spinner behavior | 4 (with spikes) | A |
| Tankers (two-Flipper cargo) | `current_board > 1 Or lv > 17` | 3 | A |
| Spikers/spikes | `current_board > 2 Or lv > 47` | 4 | A |
| Fuseballs (direct hatch) | `current_board > 9 Or lv > 20` | 11 | A |
| Pulsars (direct hatch) | `lv > 15` | 17 | A |
| Tanker-carried Pulsars | `lv > 26` | 28 | A |
| Tanker-carried Fuseballs | `lv > 31` | 33 | A |
| Super Zapper | always; 2 charges per level/life start | 1 | A |

Additional smaller mechanics observed while rechecking:

- `rimit` hurry-up: when reserve and roaming Eggs are exhausted but enemies
  remain below the rim, the source sets `rimit = True`, doubling Fuseball climb
  speed and forcing Fuseballs/Pulsars upward. **[A]** See
  [rimit trigger](../Reference/incitti/tempest.bmx#L3805) and uses at
  [Fuseball](../Reference/incitti/tempest.bmx#L3393) and
  [Pulsar](../Reference/incitti/tempest.bmx#L3067). Only meaningful once
  slow-resolving enemies (Fuseballs/Pulsars) exist; not needed now. **[B]**
- `canflip = False` only at `lv = 0`; mid-tube Flipper flipping starts at
  displayed Level 2. **[A]** See [canflip](../Reference/incitti/tempest.bmx#L4262).
  This is the first *progression* mechanic the C# game lacks, but it only
  matters when a Level 2 scenario is attempted.
- Score popups (`fusepoint`) exist only for Fuseball kills; nothing else needs
  them. **[A]** See [fusepoint](../Reference/incitti/tempest.bmx#L1937).
- Egg supply scales as `10 + lv/2 + current_board*2` (max 64). The fixed
  ten-unit C# supply matches only `lv = 0`. **[A]**

Whether any of these unlock levels match the original arcade remains **[C]**.

## 2. Enemy Bullet Question — Exact Source Differences

The shared `Bullets` projectile is identical for every firing enemy: created by
`bullets.Create(edgeIndex, firerHeight)`, climbs 3 per update, expires above
depth 400, 25 points when shot, `ZapIt` returns False. **[A]** See
[Type Bullets](../Reference/incitti/tempest.bmx#L2327). The C#
[EnemyBullet](../../src/Tempest.Game/Gameplay/EnemyBullet.cs) already implements
and validates exactly this for Flipper firing, including rendering, death/reset
clearing, insertion-order collision and the 25-point award.

Per-firer differences, all **[A]**:

| | Flipper (done in C#) | Tanker | Spinner/Spiker |
|---|---|---|---|
| Ammunition | `Rand(1,3)` at create | `Rand(1,2)` at create | none stored; at most one bullet |
| Initial timer | `bullettime = 16` | `bullettime = 16 + Rand(64)` | no timer |
| Timer rule | decrement on eligible call; `< 0` resets to 16 | same | n/a |
| Fire chance | `Rand(0,100) > 75` consumes one | `Rand(0,100) > 75` consumes one | `Rnd(0,100) > 70`, once |
| When eligible | while climbing and on pause-to-flip transition | every update while `height < 380`, after the climb increment | only at the random retreat transition (`growth > 100` and `Rnd(0,120) > 88`) |
| Fire location | own lane at post-move height | own lane at post-climb height | own lane at current build height |

Sources: [Tanker.Update firing](../Reference/incitti/tempest.bmx#L2574),
[Tanker.Create fields](../Reference/incitti/tempest.bmx#L2632),
[Spinner retreat bullet](../Reference/incitti/tempest.bmx#L3646),
[Flipper Shoot](../Reference/incitti/tempest.bmx#L2859) with call sites at
climbing and post-pause movement.

Conclusion: reusing the validated bullet system for the Tanker is the smallest
source-backed gameplay gap remaining in an already-implemented enemy. The
Spinner retreat bullet is equally small but belongs to the separate
`--spiker-test` scenario and was explicitly deferred by that milestone; it is a
natural tiny follow-up, not the best next milestone. General activation of
Flipper bullets in normal Level 1 is a *manual-validation promotion decision*
(and an arcade-accuracy question **[C]**), not an engineering milestone — no new
code is required to flip it on once the owner accepts the behavior against the
arcade.

## 3. Candidate Evaluations

### 3.1 Tanker-fired EnemyBullets — recommended

- First source eligibility: wherever Tankers exist (displayed Level 3). **[A]**
- Depends only on implemented mechanics: the validated development Tanker and the
  validated EnemyBullet lifecycle (movement, player collision, shot collision,
  25-point score, death-freeze/reset clearing, exit clearing, rendering). **[B]**
- No missing-mechanic dependency. No premature progression: it drops into the
  existing `--tanker-test` scenario.
- Existing scaffold usable: [Tanker](../../src/Tempest.Game/Gameplay/Tanker.cs)
  needs ammunition/timer fields and a firing step mirroring the already-proven
  [Flipper.TryFireBullet](../../src/Tempest.Game/Gameplay/Flipper.cs) seam;
  [TempestGame](../../src/Tempest.Game/TempestGame.cs) already owns
  `enemyBullets` and the insertion-ordered collision list. **[B]**
- Accounting impact: none — bullets are never enemies; the Tanker slot/cargo
  ledger is untouched. **[A/B]**
- Death/reset impact: bullets already freeze during death and clear on reset.
  A Tanker rebuilt from its slot after a life loss takes fresh `Rand(1,2)`
  ammunition, consistent with the source recreating baddies via `Create`. **[B]**
- Rendering: zero new work; the existing bullet renderer applies.
- Randomness: ammunition, initial timer offset, fire chance — all already
  controllable through the injected `Random`/decision seam used by Flipper
  firing tests.
- Arcade comparison: cadence, mid-tube bullet origin, threat feel. **[C]**

### 3.2 Spiker retreat bullet — small, wait one milestone

Source-backed single bullet at the random retreat transition (section 2).
Equally low complexity, but it modifies the Spiker scenario rather than
completing the Tanker, has lower gameplay significance (at most one bullet per
retreat), and its trigger is tangled with the already-tested retreat random
path. Sensible as the following micro-milestone or bundled with a later
general-firing activation pass. **[B]**

### 3.3 General activation of Flipper EnemyBullets — not an engineering milestone

Incitti arms every Flipper from Level 1. **[A]** The C# implementation is
complete behind `--enemy-bullets-test`; promotion to normal Level 1 requires
only the owner's manual arcade comparison verdict (does arcade Level 1 fire at
this cadence?) and a one-line enablement plus README/notes updates. Recommending
it as "the milestone" would spend the next work slot on a decision that is
blocked on manual validation, not on code. **[B/C]**

### 3.4 Fuseballs — strong later candidate, medium-high scope

Scope findings (all **[A]** unless noted, from
[Type Fuseball](../Reference/incitti/tempest.bmx#L3285)):

- Direct hatch: `fuseball.Create(edge, 0, 0)` — act 0, height 0, `w = 0`,
  `dir = 1`. Direct-hatched Fuseballs are fully independent of Tankers;
  Tanker-carried Fuseballs are a much later gate (`lv > 31`) and can be deferred.
- Movement states: act 0 lateral (8-update cadence; `w` 0..8 across the edge;
  `Rand(0,100) > 90` retargets direction toward the player via `FindShortDir`;
  wraps on continuous tubes, bounces at open-tube endpoints; at `w = 0/8`
  randomly picks climb or descend, forced to climb when `rimit` or at height 0),
  act 1 climb (`fuseclimbspeed = 3.3 + lv/20`, doubled by `rimit`, duration
  `25 * Rand(1,3)` counts), act 2 descend (same pattern), act 7 kill-player pose
  (`w = 3`).
- Player-shot collision: same edge, `0 < height - z < 16`, **and** interior band
  `1 < w < 7`; at the edge boundary it is unshootable.
- Player collision: only when lateral (act 0) at exactly full depth and in the
  interior band; kills as `KILLED_BY_FUSEBALL`.
- Scoring: `Int((depth - height)/100 + 1) * 250` → 250 at the rim up to 1250 at
  the bottom, displayed through a timed `fusepoint` score popup — a new
  renderer/HUD concept this project does not have.
- Animation: dedicated 4-frame × 5-arm line data
  ([ReadFuseballData](../Reference/incitti/tempest.bmx#L4283)) with a global
  frame timer and five distinct arm colors.
- Accounting: ordinary one-Egg-one-enemy; fits the existing ten-unit model and
  fresh-Egg death reconstruction. **[B]**
- No Super Zapper dependency: `ZapIt` is only *invoked by* the zapper. The score
  popup, animation data, `w`-band collision geometry and three-state randomness
  make this a medium-high milestone — the right next *enemy*, after the firing
  gap is closed. **[B]**

### 3.5 Pulsars — wait

From [Pulsar.Update](../Reference/incitti/tempest.bmx#L3030) and
[UpdatePulseTimers](../Reference/incitti/tempest.bmx#L3265), all **[A]**:
slide up/down at 1/update with 50-update random flip checks; flip state with
`pulseflipspeed`; separate rim and mid-tube pause delays; a global pulse
oscillator (`pulsing` window, `pulse_zh` zigzag amplitude); per-edge
`haspulser`/`pulsing` flags that change web rendering
([Edge.Draw](../Reference/incitti/tempest.bmx#L1209): pulsing lanes recolor and
the rim line is suppressed); two distinct player-collision branches (near-rim
vs. electrified-lane while pulsing). It would not force a renderer *redesign*,
but it is the only candidate that must thread per-edge state into
[VectorRenderer](../../src/Tempest.Game/Rendering/VectorRenderer.cs)'s tube
drawing plus add a global oscillator. It is also the latest direct-hatch gate
(displayed Level 17). Defer until after Fuseball. **[B]**

### 3.6 Super Zapper — safe to defer, revisit before Fuseball/Pulsar waves

From [SuperZapit](../Reference/incitti/tempest.bmx#L2257) and call site
([input](../Reference/incitti/tempest.bmx#L659)), all **[A]**: 2 charges,
recharged at each level start; first use zaps all non-bullet baddies with
staggered `ZapIt` delays (4, 6, 8, ...); second use kills the single best
threat (edge distance, then height, then type). Zapped Tankers die after their
delay, score 200 and release **stillborn** cargo (`ReleaseCargo(True)`,
children created with `dietime = 8`). Bullets and spikes return False from
`ZapIt`.

Nothing currently implemented depends on it: Tanker stillborn cargo is
*only* reachable through a zap kill, so deferring the zapper also correctly
defers stillborn cargo. It becomes genuinely valuable for survivability once
Fuseballs/Pulsars exist, so the right slot is after Tanker bullets and either
immediately before or after the Fuseball milestone. Implementing it now would
add per-enemy `ZapIt` plumbing ahead of the enemies that justify it — exactly
the "placeholder for architectural completeness" the project avoids. **[B]**

### 3.7 Progression/composition layer — not yet (answer: A)

The development-scenario approach remains appropriate. The next milestone adds
behavior to an enemy that already lives inside `--tanker-test`; no composition
change is needed. A small progression step (Level 2 geometry +
`canflip` mid-tube flipping + scaled Egg supply) becomes the smallest useful
composition move *later*, when multiple validated enemies need to coexist and
per-level constants (`flipflipspeed`, pause delays, climb speeds) start varying.
Building it now would front-load per-level constant plumbing with no new
behavior to compare manually. **[B]**

### 3.8 Smaller missing mechanics

`rimit` hurry-up, score popups, bonus/audio and the superzapper-recharge HUD
flash were each checked; none logically precedes Tanker bullets. Score popups
precede Fuseball (its scoring is unreadable without them) and should be bundled
into the Fuseball milestone, not done standalone. **[B]**

## 4. Recommended Next Milestone: Tanker-Fired EnemyBullets

Enable source-backed Tanker firing inside the existing `--tanker-test`
development scenario, reusing the validated EnemyBullet system.

**Why next.** It is the single remaining source-backed behavior of the enemy
that was just implemented, it reuses two validated systems with zero new
concepts, it has no accounting, rendering or progression impact, and it
completes the "enemy-origin projectile" picture (Flipper done, Tanker next,
Spinner trivial) before any new enemy is started. The project's small-step
pattern favors finishing the Tanker over opening the much larger Fuseball
front.

**Why the others wait.**

- Spiker retreat bullet: same-size change in a different scenario with less
  gameplay value; ideal immediate follow-up.
- Normal-Level-1 Flipper bullet activation: blocked on the owner's manual
  arcade comparison, not on engineering.
- Fuseball: correct next *enemy*, but medium-high (animation data, score
  popups, `w`-band geometry, three randomized states) — better attempted once
  projectile work is closed out.
- Pulsar: latest gate, only candidate touching per-edge web rendering and a
  global oscillator.
- Super Zapper: nothing depends on it yet; its value arrives with
  Fuseball/Pulsar threat density, and deferring it correctly defers stillborn
  cargo.
- Progression layer: no coexistence need yet; would add plumbing with nothing
  new to compare.

**Complexity:** low.

**Files involved (no new file necessary):**

- [Tanker.cs](../../src/Tempest.Game/Gameplay/Tanker.cs) — ammunition
  (`Rand(1,2)`), timer (`16 + Rand(64)` initial, 16 on reset), strict
  `Height < 380` firing window after the climb increment, `> 75` fire chance,
  consumption only on success; mirror the existing injected-decision seam used
  by [Flipper.TryFireBullet](../../src/Tempest.Game/Gameplay/Flipper.cs).
- [TempestGame.cs](../../src/Tempest.Game/TempestGame.cs) — route Tanker-created
  bullets into the existing `enemyBullets` list and insertion-ordered
  development collision list; gate firing on the existing flag composition.
- [SimulationTests.cs](../../tests/Tempest.Game.Tests/SimulationTests.cs) — new
  deterministic cases plus regressions.
- [README.md](../../README.md) and
  [TEMPEST-DEVELOPMENT-NOTES.md](../../TEMPEST-DEVELOPMENT-NOTES.md) — the
  `--tanker-test --enemy-bullets-test` combination currently documents "Tankers
  do not fire"; that statement changes.
- `Program.cs`/`DebugController.cs` only if flag semantics are adjusted; the
  recommendation is to reuse `--tanker-test --enemy-bullets-test` (both flags
  present arms both Flippers and the Tanker) rather than adding a new flag.

**Development scenario:**

```powershell
dotnet run --project src/Tempest.Game -- --tanker-test --enemy-bullets-test --lives 10
```

`--tanker-test` alone keeps the Tanker silent, preserving the already-validated
cargo-focused scenario for regression comparison.

**Automated-test strategy (deterministic via injected Random/decisions):**

- ammunition bounds 1–2 and initial timer offset 16 + (0..64);
- firing only while post-climb height is strictly below 380 (boundary case);
- timer reset to 16 after expiry; strict `> 75` threshold; failed chance does
  not consume ammunition; success consumes exactly one;
- bullet spawns on the Tanker lane at post-climb height and then follows the
  existing validated bullet lifecycle;
- no firing once release is pending/complete or after shot destruction;
- a Tanker rebuilt from its slot after life loss receives fresh ammunition;
  bullets clear on death reset and at exit start, unchanged;
- Tanker ledger, cargo accounting, release thresholds and all 146 existing
  regressions remain green.

**Manual arcade-comparison strategy:**

- observe whether/when arcade Tankers fire and compare cadence and ammunition
  feel against the 16-tick/`>75` model **[C]**;
- bullet origin mid-tube from the Tanker lane, visual scale/speed, rim danger;
- shooting the Tanker while its bullet is in flight (bullet must survive);
- shooting Tanker bullets for 25 points; natural release unaffected by firing;
- recheck spinner feel, cursor capture, Flipper regressions and exit visuals.

**Explicit out-of-scope:** Spiker retreat bullet; enabling any enemy firing in
normal Level 1; Super Zapper, `ZapIt` and stillborn cargo; Fuseballs; Pulsars;
score popups; `rimit` hurry-up; level progression/`canflip`; bonus/audio;
renderer changes; the documented converging-rim-Flipper investigation.

## 5. Remaining Uncertainties

- **C:** Whether arcade Tankers fire at all on their first level, and at what
  cadence, is not established by the source; the Incitti model is an
  engineering reference pending manual comparison.
- **C:** BlitzMax `Rand(1,2)`/`Rand(64)` interval semantics should be mirrored
  by the same conventions already chosen for Flipper firing, not re-derived.
- **B:** The choice to arm the Tanker via the existing flag composition (rather
  than a third flag) is a development-scenario decision, not source behavior.
