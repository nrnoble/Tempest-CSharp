# Post-Level-3 progression and roadmap analysis

## Disposition

**GO** for a bounded, source-derived Level 4 production-candidate checkpoint.
**NO-GO** for wiring natural Level 3 -> Level 4 progression before the wave,
identity, spike-exit, and source-policy gaps below are addressed and manually
accepted. This is a read-only analysis; no game feature or Level 4 was
implemented.

## Starting checkpoint and repository status

- Expected and verified HEAD: `2318ea2 Finalize player-death enemy retraction`.
- `git status --short`: only `?? TEMP/`; the existing untracked `TEMP/` remains.
- No tracked work exists. `git status --short -- Reference/incitti` is empty.
- Recent history proceeds through accepted lane highlighting, approach,
  Level 3 progression, mixed waves, and death retraction.
- Reference source was read only. No tests/build were run; this analysis
  preserves the task-supplied baseline (554/554 full solution tests, 0 build
  warnings/errors, and `git diff --check` passed at the accepted checkpoint).

Authority follows the repository's behavior specification and development
notes: direct Atari observations first, Incitti as **REFERENCE VERIFIED**
engineering evidence second, tested C# behavior third, and assumptions last.
No Level 4 claim below is promoted to **ARCADE VERIFIED**.

## Current production state

Natural play is Level 1 -> Level 2 -> Level 3 -> terminal. Levels 1 and 2 use
explicit configuration data; Level 3 uses the plus-board `Level3Mixed`
configuration. Score and lives carry through successful exits, and fresh
levels reset the Super Zapper to two charges. Level 3 remains terminal and a
normal campaign restart returns to Level 1. The accepted interactive
`LevelApproach` is shared by the existing production edges.

The current Level 3 mixed wave and its identity accounting are robustly tested,
but several production decisions are keyed directly to `IsLevel3Mixed`.
Spikers, their persistent spikes, and enemy bullets are otherwise primarily
development-scenario paths. The current static `LevelConfiguration` objects
and explicit `LevelProgression` resolver are incremental, not a complete
production-level catalog.

## Incitti Level 4 findings — REFERENCE VERIFIED

The first production set is `Original Arcade` in
[`levelset1.txt`](../../Reference/incitti/levelsets/levelset1.txt). Incitti
starts at internal `current_level = 0`, advances it by one, selects
`current_board = current_level Mod 16`, displays `current_level + 1`, and
selects `current_color = current_level / 16`. Therefore displayed Level 4 is
internal index 3, board index 3 in set 0, palette/color group 0. Its table row
is `Level 4 - binoculars`.

| Property | Level 4 source result |
|---|---|
| Geometry | New, not reused: 16 ordered vertices, closed (`continuous = 1`), 16 playable lanes. Center Y 380; Y offset -65. |
| Board points | `(-33,101), (-101,145), (-185,116), (-219,58), (-219,-29), (-185,-87), (-101,-116), (-33,-72), (33,-72), (101,-116), (185,-87), (219,-29), (219,58), (185,116), (101,145), (33,101)` |
| Enemy supply | `10 + lv/2 + board*2` = 17 original Egg/enemy slots for `lv=3`, `board=3`; the source caps this formula at 64. Tanker cargo adds separate enemies beyond these original slots. |
| Initial and reserve Eggs | Four Eggs are created immediately; 13 remain in reserve. Every 50 global updates the source attempts to release up to six reserve Eggs. Hatching is limited to two in transit, with the source's active-enemy/near-end selection rules. `hatchrate = lv/8`, hence 0 at this index. |
| Normal enemy eligibility | Flippers, Tankers, and the source's `spinner` (the C# Spiker). The normal selection is randomized, not a fixed count. Tankers are capped at six active; the spinner selection is suppressed near the end and after the source's spinner-count threshold. |
| Spikes | `hasspikes` is true when `board > 2`; Level 4 creates one height-60 Spike on each of its 16 lanes. These are persistent hazards, distinct from the active Spiker enemy. |
| Excluded enemy types | Fuseballs are disabled until `board > 9` or `lv > 20`; Pulsars until `lv > 15`. Neither is eligible at Level 4. |
| Tanker cargo | Only the two-Flipper cargo option is enabled. Tanker Pulsar cargo begins after `lv > 26` (displayed Level 28); Tanker Fuseball cargo after `lv > 31` (displayed Level 33). |
| Flipper movement | `canflip = true`; climb 2, turn 8 degrees, pauses 21 below-rim / 24 at rim. These are literal source values, not measured Atari tuning. |
| Tanker movement/firing | Climb is `1.75 + lv/20` = 1.75 at Level 4. Source gives 1–2 rounds, initial timer `16 + Rand(64)`, then an eligible post-climb `<380` check, timer reset to 16, and a shot only when `Rand(0,100) > 75`. |
| Flipper bullets | Source-created Flippers receive 1–3 rounds and timer 16; the normal Flipper update can fire them. This is not a Level 4 unlock: source eligibility begins at Level 1. |
| Zapper and entry | A fresh level resets the Super Zapper to two charges and places the player at edge 7. |
| Hurry-Up / `rimit` | After reserve Eggs and incoming Eggs are exhausted, if enemies remain and are not all at the rim, the source latches `rimit`. It changes Pulsar and Fuseball behavior, not Spiker behavior. Neither type is eligible at Level 4, so the latch has no active-enemy gameplay effect there. |
| Completion/exit | On displayed Levels 4–7, once the Eggs are exhausted and enemies are gone or satisfy the all-rim condition, source state enters `AVOID_SPIKES` before the exit zoom. Spikes remain relevant to the player during this phase. |

`rimit` is a source behavior, not proof of exact arcade timing. Likewise, the
exact Level 4 random composition, Egg cadence, enemy supply formula, and tuning
remain **REFERENCE VERIFIED**, not arcade-certified.

## Subsequent progression boundaries

The first set avoids an isolated Level 4 design: later boards continue to
change, and the normal enemy catalog expands.

| Displayed level | Source boundary relevant to architecture |
|---|---|
| 5–7 | New closed boards: Cross, Triangle, Clover. Spikes and the special spike-avoid exit continue through Level 7. |
| 8 | First open board: Vee (`continuous = 0`), with 15 playable lanes from 16 vertices. Open-tube movement/topology needs a separate readiness checkpoint before Level 8 progression. |
| 11 | First Fuseball eligibility (`board > 9`; the alternate `lv > 20` route starts at displayed Level 22). |
| 17 | First Pulsar eligibility (`lv > 15`) and first palette group change (`current_color = 1`). |
| 28 / 33 | Tankers may first carry Pulsars / Fuseballs, respectively. |

Board data continues to supply a distinct geometry at each level in this set.
Level 4 does not need open-tube support. It also does not justify implementing
the later Fuseball, Pulsar, enemy-cargo, or 16-level palette behaviors now.

## Existing C# readiness

### Geometry and renderer

**READY WITHOUT REDESIGN, with a Level 4 data/configuration addition.** The
Level 4 board is a new ordered point set, not a new geometric primitive.
`LevelDefinition`, `LevelConfiguration`, `LevelLoader`, projection, and
rendering already consume per-level points and center/offset values. The
closed 16-lane topology and directed endpoint-hinge transition code are
generic. Level 3's plus-board tests already exercise all 32 directed
neighbor transitions, including 270-degree reflex corners. Level 4 adds
irregular convex turns and approximately 213-degree interior reflex turns;
it introduces no exact 90-, 180-, or 270-degree pivot case and no open
boundary.
Still, its own 32-transition and rendering-finiteness tests are required:
the Level 3 tests do not establish that the exact new points render/pivot
correctly.

Level 4 needs the exact source points plus center/offset 380/-65. A second
generic geometry engine or special screen-coordinate renderer is unnecessary.

### Wave, identity, and enemies

**NEEDS SMALL PRODUCTION-WAVE GENERALIZATION.** The current
`Level3WaveLedger` is not just cosmetically misnamed. Original types/states,
runtime validation, completion, cargo ownership, and death reconstruction
explicitly recognize Level 3 Flippers and Tankers. A Level 4 Spiker can retreat
and return to reserve, so it must retain its original identity without being
mistaken for a kill, an escape, or a terminal resolution. Tanker cargo must
remain independently identified and conserved. This is the first production
composition requiring the ledger's original-identity lifecycle to include a
third enemy type. Therefore classification is **C: small production-wave
generalization before Level 4**, not “already general enough” and not a
full-game/all-99-level framework.

Current configuration has only `FlippersOnly` and `FlippersAndTankers`.
Normal Flipper/Tanker selection, static spikes, Spiker creation, and several
completion and renderer paths branch on `IsLevel3Mixed` or
`SpikerDevelopmentWave`. `Spiker` currently has no production identity
attachment; when it retreats, the C# development path returns supply to the
reserve, while the production ledger cannot validate that transition. The
configuration must also express Level 4's static starting Spikes separately
from its random Spiker enemy eligibility.

The existing Spiker, Tanker, Flipper, Egg, bullet, and Super Zapper entities
are useful implementation building blocks, but the development wave's fixed
Spiker speed/composition is not Level 4 source configuration. Level 4
requires source-eligible randomized Spikers (the source formula includes
`lv/48.0 + Rand(2.5,4.5)`), Flippers, and Tankers, with the source limits and
only Flipper cargo. Source Spikers can also fire a bullet on a random
growth-driven retreat; the C# path only arms this under the development bullet
option. Do not copy the artificial Spiker/Tanker/Fuseball/Pulsar test waves
into production.

### Bullet policy and Tanker firing

**Tanker firing is not a Level 4 blocker.** Level 4 requires Tanker firing, and
the current Level 3 production mixed-wave path already arms Tankers, displays
their bullets, and uses the source timer/chance/cutoff policy. The C2
correction and later C3/C4 manual acceptance found no currently identified
firing defect requiring a Tanker-specific pre-Level-4 correction. Neal's
qualitative observations support separated shots and the possibility of two
before release, not exact per-Tanker shot counts or exact arcade random
constants.

**Flipper bullets are a separate cross-level discrepancy.** Incitti gives
ordinary Flippers 1–3 rounds beginning at Level 1 and uses the 16-tick /
greater-than-75 random firing policy. Current C# normal Flippers (including
Level 3 ordinary/cargo Flippers) remain silent unless the development bullet
option is set; Level 3 Tankers are the exception. Thus Level 4 source
composition does not introduce a new bullet eligibility boundary, but an
authentic Level 4 cannot silently treat Flipper bullets as a Level 4-only
feature. Decide and validate the cross-level production policy before
claiming source-equivalent normal Flipper behavior; if retained as a project
deferral, record that uncertainty explicitly rather than enabling bullets only
on Level 4.

### Progression, approach, completion, and presentation

- **Approach/progression: READY WITHOUT REDESIGN.** Fresh configuration
  installation, score/lives carry, Super Zapper reset, player setup, and the
  interactive approach are generic. `LevelProgression` currently encodes only
  Level 1 -> 2 -> 3; Level 3 terminal/restart logic must be extended only
  after a Level 4 candidate is validated. Do not retune accepted approach
  perspective or claim Atari-exact timing.
- **Completion: NEEDS SMALL GENERALIZATION.** Preserve destroyed, escaped,
  rim-cleared, unresolved, latent cargo, pending shot activation, and delayed
  Zapper outcomes. Extend all-rim and complete-state identity validation to
  Spikers without allowing fixed Spikes to count as enemies. For source
  Levels 4–7, configure the existing spike-avoidance exit seam for production;
  it is presently gated to the Spiker development wave.
- **Hurry-Up: NOT A LEVEL 4 BLOCKER; REQUIRED BEFORE LATER TYPES.** C#
  `LatchHurryUp` currently enables the source-inspired latch only for
  Fuseball/Pulsar development scenarios. Incitti latches `rimit` in ordinary
  play, but Level 4 has neither eligible type, and the source does not apply
  `rimit` to Spikers. Preserve that boundary; add production behavior before
  Fuseballs become eligible at displayed Level 11 (and Pulsars at Level 17),
  with source-order, death/reconstruction, and all-rim tests.
- **Player death/reconstruction: READY WITH IDENTITY EXTENSION.** The accepted
  render-only retraction applies generically to active enemies; fixed Spikes
  are separate and already remain visible during death. Keep logical enemy
  positions frozen and do not retract static hazards. Level 4 needs identity
  conservation tests for a retreating/re-entering Spiker and persistent
  Spikes, not a death-architecture redesign.
- **Super Zapper: READY WITH TARGET/LEDGER INTEGRATION.** The target selector
  recognizes Spikers and fresh levels reset to two charges. Ensure Level 4
  production insertion order, identity resolution, and pending zap outcomes
  use the generalized wave path.
- **Lane highlight/palette: READY.** Level 4 is internal index 3, so its
  source palette group remains 0 (yellow); palette group 1 starts at displayed
  Level 17. No palette cycling work is justified for Level 4.

## Configuration recommendation

Add a Level 4 configuration/definition using the existing immutable
`LevelConfiguration` pattern, and extend the current progression resolver
incrementally. Do not build an all-99-level loader/table framework now. The
Level 5–7 boards are additional data, while the actual architectural breakpoints
are Level 4's Spiker/spike policy and Level 8's open topology. Reassess a common
production catalog once repeated enemy-eligibility and geometry data show a
real need.

## Recommended immediate checkpoint and roadmap

### D1 — Level 4 production candidate, isolated diagnostic (recommended next)

Implement the smallest production-wave generalization that supports the exact
Level 4 source configuration and expose it through a direct diagnostic;
leave natural Level 3 terminal behavior unchanged. Include the Spiker original
identity/return-to-reserve path, independent persistent Spikes, eligible
Flipper/Tanker/Spiker composition, 17-unit source supply, Tanker cargo, the
source Level 4 board, and the Level 4–7 spike-avoid exit policy. Decide and
test the production Flipper/Spiker-bullet policies explicitly; do not silently
promote development modes or enable Flipper bullets only at Level 4. Keep
Fuseballs/Pulsars and palette cycling disabled. `rimit` is not a Level 4
Spiker behavior; schedule its production integration before Level 11.

This is safer than direct natural progression because it exercises the new
composition, identities, hazards, geometry, and completion behavior before
campaign state can enter them. Do not rename/generalize the ledger merely for
aesthetics; the new Spiker identity lifecycle is the concrete reason to do it.

### D2 — Natural Level 3 -> Level 4

After D1 automated and manual acceptance, extend the production resolver and
approach tests for the one new edge. Verify successful exit commit, fresh Level
4 setup, score/lives carry, two Zapper charges, player rim setup, approach
freeze/movement/fire suppression, death/restart, and Level 4 terminal behavior
until its next successor is implemented.

### D3 — Levels 5–7 data and spike-exit progression

Add each source board incrementally using the same closed geometry path and
eligible enemy set, and retain the reference spike-avoid exit range. Do not
invent a broad level-table abstraction or change Level 4's accepted behavior.

### D4 — Open-tube readiness before Level 8; next enemy boundary before 11

Implement and test open topology before wiring Level 8. The current open
fixture proves only `VertexCount` versus `PlayableLaneCount`; `Level.NextEdge`
and `PreviousEdge` still wrap modulo the vertex count, so open-lane movement,
directed transitions, rendering, enemy traversal, and all-rim completion are
not yet established. Before displayed Level 11, separately add normal
Fuseball eligibility and production `rimit`; Pulsars and their palette group
follow at Level 17.

## D1 automated-test outline

1. **Configuration/source data:** exact index/display/board/palette, points,
   center/offset, closed 16-lane topology, depth/position/player start, supply
   17, four initial Eggs, 13 reserve, and configured source eligibility.
2. **Geometry:** all 32 directed Level 4 endpoint-hinge transitions, including
   reflex vertices; player movement and lane neighbors; finite tube, lane
   highlight, Flipper/Spiker/Tanker projection and pivot endpoints.
3. **Wave supply/composition:** reserve release and Egg scheduling, eligible
   Flipper/Tanker/Spiker selection, six-Tanker limit, Spiker selection limit,
   no Level 4 Fuseballs/Pulsars, and only two-Flipper Tanker cargo.
4. **Identity conservation:** original Spiker retreat returns the same
   unresolved identity to reserve; kills/escapes/rim clears remain distinct;
   Tanker original/cargo ownership, latent and pending cargo, zap resolution,
   and repeated death reconstruction have no duplicate or lost identities.
5. **Spikes and completion:** 16 starting Spikes, Spiker growth/retreat,
   shot/hazard behavior, fixed Spike geometry through death, all-rim accounting,
   and Level 4–7 avoid-spikes entry/exit/reversal. Spikes never resolve as
   enemies or retract with enemies.
6. **Bullets:** deterministic source-boundary tests for the chosen normal
   Flipper/Spiker bullet policy; verify bullets neither alter supply nor
   survive reset contrary to existing lifecycle. Include regressions for
   Levels 1–3 if the policy is promoted across the campaign.
7. **Zapper/death/restart:** enemy selection, two-charge fresh reset,
   pending outcomes, fixed Spike persistence, enemy retraction, reconstruction,
   score/lives, and Game Over remain conserved.
8. **Progression seam:** D1 stays direct/terminal; D2 reuses existing
   `LevelApproachTests` and `LevelProgressionTests` to verify the natural edge.

Existing focused suites to reuse include configuration/progression/approach,
Level 3 mixed-wave and ledger, geometry, Simulation/death, Spiker, EnemyBullet,
Super Zapper, lane highlight, and death-presentation tests. No test-only
architecture is needed.

## D1 manual acceptance outline

Use the direct Level 4 diagnostic (not a normal campaign run) after automated
gates pass. Check:

- the binoculars tube's shape, scale, center, and continuous rim;
- spinner movement across all lanes and Flipper pivots at the convex/reflex
  sections, with no snap, inversion, wrong lane, or renderer artifact;
- randomized Flipper/Spiker/Tanker presence, visible Spiker glyph and
  persistent web spikes, and the Flipper-only Tanker cargo release;
- normal shots, bullet visibility/frequency under the selected policy,
  Super Zapper, and no unexpected Fuseballs/Pulsars;
- Spike collision/avoidance and exit presentation, including that Spikes
  remain fixed when enemies retract on player death;
- the gameplay feel and any visible regression against the accepted Levels
  1–3 baseline.

Manual validation should address appearance, movement, firing/hazards, death,
and exit feel. It need not duplicate automated identity/accounting checks.
After D2, run the normal `--lives 10` campaign through the Level 3 -> 4
approach and confirm the same visual/setup behaviors in natural progression.

## Documentation and evidence boundary

A later implementation should update tracked documentation narrowly:

- **ARCADE VERIFIED:** only Neal's direct observations already established in
  the behavior specification (including generic death retraction and fixed
  Spike persistence). No Level 4 board/composition claim is currently
  arcade-verified.
- **REFERENCE VERIFIED:** Level 4 board points/metadata, source enemy
  eligibility and supply, source tuning/selection, Tanker cargo/firing rules,
  palette group, `rimit`, and Level 4–7 avoid-spikes state.
- **PROJECT DECISION:** any C# wave randomness, identity-model generalization,
  production Flipper-bullet policy, exact exit/hurry implementation, and
  behavior where Atari evidence remains absent.
- **DEVELOPMENT ONLY:** existing direct Spiker/Fuseball/Pulsar/bullet scenarios
  and any new isolated Level 4 diagnostic until it is wired into normal play.

Preserve the accepted Level 1–3 behaviors, death/retraction architecture,
approach tuning, Tanker/cargo accounting, and lane highlight. Keep the final
Blaster-location death animation, shot-size refinement, elevated approach
perspective, broad rendering fidelity, spinner refinement, audio, bonuses,
later palette cycling, open tubes until their boundary, end-game animation,
and high-score persistence deferred.

## Final status

- **Immediate blocker:** Level 4 cannot safely use the current Level 3-only
  identity/composition path; it requires Spiker identity/return-to-reserve,
  persistent production Spikes, and the source Level 4 spike-avoid exit. The
  normal Flipper/Spiker-bullet policies also require explicit resolution or
  documented deferral before claiming source-equivalent play. `rimit` is not
  a Level 4 Spiker behavior; production `rimit` work is due before Fuseballs
  enter at Level 11.
- **Tanker firing:** required by Level 4 and already present in the current
  Level 3 production mixed path with no specific known correction blocker.
- **Recommendation:** D1 direct Level 4 production-candidate diagnostic, then
  D2 natural Level 3 -> 4 only after D1 gates and manual acceptance.
- **Reference:** read-only; `git status --short -- Reference/incitti` is clean.
- **Tracked changes:** none. No tests/build were run and no commit/push occurred.
