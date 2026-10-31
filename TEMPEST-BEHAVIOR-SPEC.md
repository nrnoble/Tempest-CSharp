# Tempest Behavior Specification

## 1. Purpose and evidence model

This document records observable gameplay behavior and the strength of the
evidence behind each statement. It is intended to remain useful independently
of the current C# design. It does not prescribe classes, update methods,
collections, or renderer architecture.

Use these classifications literally:

- **ARCADE VERIFIED** — Neal has directly observed or validated this behavior
  against Atari Tempest. This is the strongest evidence and takes priority over
  a disagreement with the reference source or recreation.
- **REFERENCE VERIFIED** — the behavior is explicit in Mark Incitti's
  `Reference/incitti` source. This describes that reference, not necessarily
  Atari Tempest.
- **PROJECT DECISION** — an intentional recreation rule where arcade/reference
  evidence is incomplete, or where the recreation deliberately differs.
- **DEVELOPMENT ONLY** — a scenario, override, deterministic seam, artificial
  wave, or instrumentation used only to develop or test the recreation. It is
  not evidence of authentic arcade wave composition.

Direct arcade observations outrank the Incitti reference. Reference source
establishes only what that source does. Current C# behavior and passing tests
establish what the recreation does, not what the arcade does. Historical AI
reports help locate decisions and corrections but are not independent arcade
evidence. When evidence is incomplete or conflicting, use the weaker label and
record the uncertainty rather than guessing.

## 2. Global gameplay behavior

### Player, firing, and score

- **PROJECT DECISION** — The current playable configuration is one closed,
  16-lane tube based on Level 1. The Blaster travels around the rim; relative
  spinner movement is the primary input, with left/right keyboard movement as
  a fallback. The input mapping and current movement rates have not been
  established here as arcade-verified tuning.
- **PROJECT DECISION** — Player shots travel from the Blaster at the near rim
  toward the far end of the currently occupied lane. The current recreation
  limits simultaneous player shots and gives them a fixed travel rate. Those
  limits/rates describe this build, not a claim about measured arcade timing.
- **REFERENCE VERIFIED** — Scoring is awarded when a target is destroyed or
  damaged, with values varying by enemy and target type. The enemy sections
  below record the currently implemented/reference-backed values where known.
- **PROJECT DECISION** — Score survives an ordinary life loss. A full restart
  resets score.

### Lives, death, and reconstruction

- **PROJECT DECISION** — A new game starts with three lives unless a
  development override is supplied. A lethal event starts the recreation's
  60-update death interval; one life is deducted when that interval ends.
  Losing the final life enters Game Over.
- **PROJECT DECISION** — During ordinary life reconstruction, active unresolved
  enemies are discarded and re-enter as fresh Eggs. Previously destroyed
  enemies remain destroyed, score is retained, and old enemy positions/state
  are not restored. Spikes are persistent level hazards in the Spiker
  development scenario.
- **PROJECT DECISION** — The C# simulation freezes enemies and the shared
  Pulsar clock during the full death interval. Incitti advances Pulsars/global
  pulse state during much of its death presentation; the recreation keeps its
  established freeze policy rather than claiming equivalence.

### Eggs and Level 1 wave accounting

- **REFERENCE VERIFIED** — “Egg” is Incitti's name for a small incoming
  precursor point, not an egg-shaped enemy. It starts beyond the far end of the
  tube, moves while waiting, approaches the web, and hatches at the web into an
  enemy.
- **REFERENCE VERIFIED** — The Level 1 supply is ten enemy units. Four Eggs
  are released initially, reserve Eggs are released over time, no more than
  four active Eggs are maintained, and at most two are approaching/hatching.
  Hatching converts an existing unit; it does not create another enemy.
- **PROJECT DECISION** — The C# release lanes and direction choices are
  deterministic to make the current Level 1 simulation repeatable. This is not
  a claim that the arcade uses the same fixed sequence.
- **PROJECT DECISION** — The standard Level 1 wave is complete only after all
  ten units are destroyed and there are no reserve Eggs, active Eggs, or active
  enemies. This is the recreation's conservation rule, not a verified arcade
  completion rule.

### Late-wave hurry-up

- **REFERENCE VERIFIED** — Incitti latches `rimit` when reserve Eggs and active
  Eggs are exhausted, active enemies remain, and at least one active enemy is
  below the rim. Its decision follows Egg and enemy updates. It remains set
  through ordinary life conversion and clears on new-level setup.
- **PROJECT DECISION** — The recreation evaluates that trigger after hatching,
  enemy updates and player-collision checks, before its final completion check.
  Enemy updates on the trigger tick use the previous latch; the next active
  tick uses hurry-up. The latch survives the full player-death freeze and
  fresh-Egg reconstruction, even when reconstruction restores Egg supply.
  Full game restart or replay initialization clears it. These exact C# timing
  and lifecycle rules are not arcade-verified by manual gameplay.
- **DEVELOPMENT ONLY** — Hurry-up is enabled only in the existing direct-hatched
  Fuseball and Pulsar scenarios. Normal Level 1, Spiker and Tanker scenarios
  remain unchanged; these artificial waves do not establish Atari composition.
- **PROJECT DECISION** — Hurry-up awards no points, increments no destroyed
  count, removes or resolves no enemies, and does not alter Egg/accounting
  totals. The recreation retains kill-all completion rather than Incitti's
  separate `onrim`-based exit; hurry-up or all-rim survivors cannot start exit.

### Completion and tube exit

- **PROJECT DECISION** — After the standard Level 1 supply is cleared, the
  recreation enters a short exit delay and then moves the Blaster down the tube
  through a zoom sequence. It ends at Level Complete; another level or arcade
  progression is not implemented.
- **DEVELOPMENT ONLY** — The Spiker scenario adds a spike-avoidance interval
  and can reverse/retry the exit after a spike collision. This is not part of
  the standard Level 1 clear sequence.
- **ARCADE VERIFIED** — In Neal's manual validation of the isolated,
  **DEVELOPMENT ONLY** `--pulsar-test` scenario, destroying all Pulsars
  permits the established tube-exit sequence. This does not establish an
  authentic all-Pulsar Atari wave.

## 3. Flippers

### Entry and movement

- **REFERENCE VERIFIED** — A Level 1 Flipper hatches at the far end of a lane
  and climbs toward the rim without changing lanes in mid-tube. At the rim it
  pauses, then pivots between adjacent rim lanes while pursuing the Blaster.
  Incitti's relevant reference values are a climb increment of 2, a 24-update
  rim pause, and an 8-degree pivot increment. These exact values have not been
  validated against arcade timing.
- **REFERENCE VERIFIED** — The Flipper is shootable in its lane while
  climbing or paused. During a pivot it can be hit from either of the two
  participating lanes, within the reference's strict shot-height band.
  Destroying a Flipper awards 150 points.

### Player collision

- **ARCADE VERIFIED** — A rim Flipper does not kill merely because it visually
  overlaps the Blaster during its visible pivot. It becomes lethal when the
  pivot completes into the lane currently occupied by the Blaster. Destroying
  it before that completion saves the Blaster.
- **REFERENCE VERIFIED** — A climbing Flipper that reaches the rim directly
  in the Blaster's lane is lethal at that arrival. This is distinct from the
  observed pivot rule above. The C# behavior was corrected after reference
  review; the direct-arrival case has not been separately established here as
  an arcade observation.
- **PROJECT DECISION** — Shot-versus-Flipper checks precede Flipper-to-player
  collision in the recreation. A shot that destroys a Flipper on the otherwise
  lethal update prevents that Flipper from also costing a life.

## 4. Enemy bullets

- **REFERENCE VERIFIED** — Incitti gives Flippers one to three rounds and a
  firing countdown. Eligible Flipper updates can fire during a climb or after
  a rim-pause transition. The bullet travels outward on a lane, expires beyond
  the rim, can be shot for 25 points, and threatens the Blaster only on its
  lane near the rim. The reference's exact ammunition, countdown, random
  threshold, speed, and collision distances have not been validated against
  Atari gameplay.
- **REFERENCE VERIFIED** — Incitti's Spiker (“Spinner” in the source) can
  fire a bullet after a growth-driven retreat decision; its separate firing
  roll is strictly greater than 70. This optional behavior is also not
  arcade-verified.
- **DEVELOPMENT ONLY** — Enemy bullets are opt-in in the C# recreation through
  `--enemy-bullets-test`; they are not enabled in the ordinary launch. Bullet
  hits do not count as enemy kills. The recreation clears bullets on life
  reconstruction and when exit begins.
- **DEVELOPMENT ONLY** — Spiker retreat bullets require the Spiker and enemy
  bullet test options together. Tanker bullets require the Tanker and enemy
  bullet test options together. Neither combination establishes normal arcade
  wave behavior.

## 5. Spikers and spikes

- **REFERENCE VERIFIED** — The reference calls the Spiker a “Spinner.” It
  builds a persistent spike in its lane, then retreats. A retreat returns a
  pending enemy unit without awarding a kill or points. Shooting a Spiker
  awards 50 points. Shooting a spike shortens it by 10 and awards 1 point;
  spikes persist after their builder leaves.
- **REFERENCE VERIFIED** — A spike is lethal to a Blaster in the same lane
  when the spike extends farther toward the rim than the Blaster's current
  depth. Spiker speed selection in the source uses a fractional/random
  expression whose precise arcade-equivalent distribution is not established.
- **DEVELOPMENT ONLY** — The C# Spiker test wave starts height-60 spikes on
  every lane, uses a fixed Spiker speed of 3, and permits Spikers alongside
  Flippers on the existing Level 1 tube. Spikes do not count as enemies and
  do not block this scenario's wave-clear test. These choices do not describe
  authentic Level 1 or later-level composition.
- **PROJECT DECISION** — In the development scenario, spikes remain visible
  and hazardous after their Spiker retreats or the Blaster loses a life.
  Arcade timing and feel for growth, retreat, spike damage, and exit avoidance
  remain unverified.

## 6. Tankers and cargo Flippers

- **REFERENCE VERIFIED** — The Incitti Tanker can carry two Flippers. It
  climbs on a lane and releases live cargo either naturally near the rim or
  when destroyed by a player shot. A shot destruction awards 200 points;
  natural release awards no Tanker kill score. The cargo appears on adjacent
  lanes and begins moving outward. These rules describe Incitti, not validated
  Atari Tanker behavior.
- **DEVELOPMENT ONLY** — The C# Tanker test scenario creates one deterministic
  Tanker among the ten original enemy units; the other slots hatch as
  Flippers. The test Tanker starts at height 10, uses a fixed 1.85 climb
  increment, and releases cargo after its height passes 385. This is an
  artificial test wave on Level 1 geometry, not normal Level 3 composition.
- **DEVELOPMENT ONLY** — The Tanker scenario tracks its original enemy slot
  separately from released cargo so a life loss can reconstruct unresolved
  units without duplicating resolved units. The optional combined
  `--tanker-test --enemy-bullets-test` mode also arms Tanker bullets. The
  older README description that Tankers do not fire is stale relative to the
  later source-backed combined-mode work.

## 7. Fuseballs

- **REFERENCE VERIFIED** — Incitti Fuseballs move laterally across a lane
  boundary, can wrap or reverse at tube endpoints, and alternate lateral
  travel with climbing or descending excursions. Their shot window excludes
  the outer portions of the lane; a valid hit awards 250 points at the rim,
  increasing by 250-point depth bands up to 1250 at height zero. The 1250
  endpoint is not normally reachable by shooting in the current direct-hatch
  path because shots expire before that collision. The source uses a
  five-color, four-frame vector animation.
- **REFERENCE VERIFIED** — The source increments its lateral counter before
  testing `cnt > 8`, so lateral steps are nine updates apart, not eight. The
  vertical-duration threshold is re-rolled on each update rather than chosen
  once on entry. These exact timings/math have not been validated against
  arcade gameplay.
- **DEVELOPMENT ONLY** — `--fuseball-test` hatches all ten ordinary units
  directly as Fuseballs on the existing tube. It uses the unscaled 3.3 source
  climb term and the existing Egg/accounting/death-reconstruction rules. It is
  not normal level composition.
- **ARCADE VERIFIED** — In manual gameplay of the isolated
  **DEVELOPMENT ONLY** `--fuseball-test` scenario, Neal verified that
  Fuseballs move up and down the lanes, can be shot, and show a score display
  when hit. A Fuseball reaching the rim kills the Blaster when both occupy
  the same lane. After all Fuseballs are destroyed, the Blaster travels down
  the tube through the established exit behavior. These observations do not
  establish authentic all-Fuseball arcade wave composition.
- **REFERENCE VERIFIED** — Exact movement cadence, climb/descent duration,
  numeric collision boundaries, score formula and thresholds, animation frame
  timing, and reference speed remain source-derived; they were not established
  by the manual observations above.
- **ARCADE VERIFIED** — In Neal's hurry-up validation of
  `--fuseball-test --lives 10`, a noticeable late-wave increase in upward
  speed was visible and consistent with his Atari Tempest expectation.
  This validates the observable acceleration, not a measured numeric rate.
- **REFERENCE VERIFIED** — Under `rimit`, Incitti forces climbing at a lateral
  boundary even when the ordinary random result selects descent, and applies
  its existing climb term twice before clamping at the rim. The development
  value 3.3 therefore produces 6.6 per climbing update; neither numeric rate
  nor the exact decision boundary is arcade-verified. Descent speed, lateral
  cadence, collision rules, scoring and animation are unchanged.

## 8. Pulsars

### Arcade-validated behavior

The following observations were made in the **DEVELOPMENT ONLY** isolated
`--pulsar-test` composition on the existing tube. They validate the observed
Pulsar behavior in that scenario, not an authentic all-Pulsar Atari wave.

- **ARCADE VERIFIED** — Pulsars appear as six-segment zigzags in their lanes.
  They climb and descend, and can pause or transition into neighboring lanes.
- **ARCADE VERIFIED** — During a pulse, the Pulsar appearance changes and the
  affected lane's radial boundaries flash. The affected lane is hazardous to
  the Blaster even when the Pulsar itself is farther down the tube.
- **ARCADE VERIFIED** — Rim contact destroys the Blaster. Pulsars can be shot
  for 200 points.
- **ARCADE VERIFIED** — After a life loss, unresolved Pulsars return through
  the Egg/re-entry behavior while already destroyed Pulsars remain destroyed.
  Destroying all Pulsars allows the established tube-exit sequence.
- **ARCADE VERIFIED** — While a lane is actively pulsing, the near/top rim
  connection spanning that affected lane disappears and returns when the
  pulse ends. Unaffected near/top connections remain visible, and the affected
  lane's radial boundaries continue flashing. Occupying that lane during the
  pulse destroys the Blaster.
- **ARCADE VERIFIED** — In Neal's hurry-up validation of
  `--pulsar-test --lives 10`, the observable late-wave Pulsar behavior appeared
  correct, with no vertical speed increase expected or observed. This does
  not establish an exact top-end decision, random boundary or movement rate.

### Reference behavior and recreation choices

- **REFERENCE VERIFIED** — The current source-derived Pulsar configuration
  uses a decision after 51 updates, a shared 181-update pulse cycle, and
  18-update lane and 14-update rim pauses. The pause values come from the
  source's first Pulsar-eligible level setup (`lv = 16`) and its integer
  arithmetic. They are not arcade-verified timing constants.
- **REFERENCE VERIFIED** — Incitti's `rimit` redirects the top-end choice to
  the lane-pause/flip branch rather than newly selecting descent. It does not
  increase vertical speed: climbing, descending and climbing flips retain
  their one-unit movement term. Existing pauses, lane transfer, flip behavior
  and shared pulse oscillator remain unchanged. These exact source rules are
  distinct from Neal's observable late-wave validation.
- **DEVELOPMENT ONLY** — `--pulsar-test` hatches all ten units directly as
  Pulsars on the closed Level 1 tube, using the source's `lv = 16` movement
  settings. It is not Level 17 composition or progression.
- **PROJECT DECISION** — The recreation freezes Pulsars and the shared pulse
  clock during its 60-update death interval. Incitti advances the relevant
  state during much of its death presentation.
- **PROJECT DECISION** — Rendering omits only the affected lane's near/top
  rim connection during its active visual pulse. Neal's arcade observation
  takes precedence over the earlier analysis that assumed the ordinary rim
  lines remained. The recreation does not copy Incitti's broader line
  suppressions where those disagree with the observed arcade presentation.

## 9. Rendering and visual behavior that affects gameplay

- **ARCADE VERIFIED** — Pulsar pulse presentation includes the changed
  six-segment glyph, flashing radial boundaries, and temporary omission of
  only the affected lane's near/top rim connection. This is gameplay-relevant:
  the same lane is lethal during the pulse.
- **REFERENCE VERIFIED** — Fuseball's reference visual is a five-color,
  four-frame vector figure. Exact arcade appearance and animation cadence have
  not been independently recorded here.
- **PROJECT DECISION** — The recreation uses projected vector line art for its
  tube and enemies. Automated geometry/render-submission tests do not establish
  native framebuffer output or arcade visual fidelity.
- **PROJECT DECISION** — Fuseball score labels, the current tube projection,
  and the simplified death presentation are recreation choices. They should
  not be interpreted as verified arcade presentation.
- **DEVELOPMENT ONLY** — Spikes remain visible during Spiker exit travel
  because they can still be lethal in that scenario. Other enemy rendering
  during exits is not part of the standard gameplay contract.

## 10. Development-only scenarios

These controls are test conveniences, not arcade level-selection or composition:

| Option | Development behavior |
|---|---|
| `--spiker-test` | Replaces the normal wave with a Spiker/Flipper test wave and height-60 spikes on every lane. |
| `--tanker-test` | Replaces the normal wave with one controlled Tanker plus ordinary Flipper slots and cargo. |
| `--fuseball-test` | Direct-hatches the ten-unit supply as Fuseballs, with source-derived late-wave hurry-up. |
| `--pulsar-test` | Direct-hatches the ten-unit supply as Pulsars, with source-derived late-wave hurry-up. |
| `--enemy-bullets-test` | Enables the development enemy-bullet behavior; combinations with Spiker or Tanker scenarios enable their corresponding source-backed firing paths. |
| `--lives <n>` | Overrides the starting life count for longer development sessions. |

The enemy-specific wave options are mutually exclusive. Deterministic random
seams, controlled selection, artificial wave composition, extra lives, and
test-only rendering/collision instrumentation are all **DEVELOPMENT ONLY**.
Passing a simulation test establishes a C# invariant, not arcade fidelity.
- **DEVELOPMENT ONLY** — Neal manually checked that the isolated Pulsar
  scenario does not spawn unrelated enemy types. This validates the test
  scenario's isolation, not authentic arcade wave composition.

## 11. Known uncertainties and deferred behavior

- **ARCADE VERIFIED scope** — Direct arcade observations explicitly listed
  above cover the Flipper pivot collision rule, the itemized Fuseball
  behaviors, and the Pulsar behaviors Neal observed in the isolated
  development scenario. No additional behavior is implied.
- **REFERENCE VERIFIED only** — Exact Flipper, bullet, Spiker, Tanker,
  Fuseball, and Pulsar constants remain source-derived unless explicitly
  identified above as arcade-validated. This includes the Pulsar 51/181/18/14
  timings, Fuseball's nine-update lateral cadence and its 3.3-to-6.6 hurry-up
  climb terms. Exact rimit trigger timing, random decision boundaries and
  source lifecycle remain source-derived; the C# evaluation/freeze/reset
  policy remains **PROJECT DECISION**, not newly arcade-verified.
- **Unresolved** — The direct climbing-Flipper rim-arrival collision is in
  Incitti and current C#, but has not been separately confirmed against the
  arcade. The older development note described rim death only at completed
  pivot; later source review distinguished direct arrival from pivoting.
- **Unresolved** — The repository records a suspected case of two rim
  Flippers converging on the same lane, but it has not been reproduced or
  compared with the arcade.
- **Unresolved** — Current C# Level 1's kill-all completion rule differs from
  some Incitti level-completion paths. The current recreation rule is
  intentional; its arcade equivalence is not established.
- **Unresolved** — Exact player input sensitivity, shot cadence, death
  presentation/timing, Egg pacing, normal enemy-bullet frequency, Spiker and
  Tanker arcade eligibility/composition, and exit visuals need direct arcade
  comparison where they matter.
- **Deferred** — Normal multi-level progression and authentic later-wave
  composition, open-tube rendering, Super Zapper, audio,
  bonus rules, broader enemy/cargo combinations, and source-specific death
  effects are not specified as implemented gameplay here.

## 12. Behavior-validation history

- **Level 1 Flipper baseline** — The project established a playable Flipper
  wave, shots, scoring, and life handling. **REFERENCE VERIFIED** mechanics
  were used for source-derived values; automated tests protect recreation
  behavior. The manually observed Flipper pivot collision rule is
  **ARCADE VERIFIED**.
- **Egg and death reconstruction** — Egg entry, hatching, and reconstruction
  of unresolved enemies were added and regression-tested. Tests verify C#
  accounting and lifecycle, not arcade truth; the 50-update pacing remains
  **REFERENCE VERIFIED**.
- **Direct Flipper rim arrival** — Source review found that a climbing
  Flipper reaching the occupied rim lane must be lethal. The implementation
  and lifecycle tests were corrected without changing the separately
  arcade-verified completed-pivot rule. Direct arrival remains
  **REFERENCE VERIFIED**, pending its own arcade check.
- **Enemy bullets and Spiker/Tanker work** — Milestone reports and tests
  established opt-in bullet paths, persistent spikes, cargo release, and
  accounting behavior in the C# recreation. These are **REFERENCE VERIFIED**
  where tied to Incitti and **DEVELOPMENT ONLY** where enabled by scenario
  flags; the reports do not establish arcade fidelity.
- **Fuseball milestone** — The implementation report corrected an earlier
  eight-update description to nine updates by checking the literal counter
  order. Neal's manual observations establish lane travel, shootability, a hit
  score display, same-lane rim lethality, and the exit after the final kill as
  **ARCADE VERIFIED**. Exact cadence, duration, collision bounds, scoring
  formula/thresholds, animation timing, and speed remain
  **REFERENCE VERIFIED**.
- **Pulsar analysis and implementation** — Source analysis first concluded
  that ordinary rim lines were preserved during a pulse. Neal's direct arcade
  observation showed that the affected near/top rim connection disappears
  while unaffected connections remain. Re-reading Incitti also exposed a
  broader source-rendering behavior that did not match that arcade
  observation. The C# renderer was corrected to the observed, narrower rule.
  This is **ARCADE VERIFIED** and demonstrates why arcade observation
  outranks an analysis report or a source interpretation.
- **Pulsar movement and timing** — Simulation tests establish the current
  source-derived 51-update decision, 181-update oscillator, and 18/14 pauses.
  These remain **REFERENCE VERIFIED**, not arcade-verified exact timings.
- **rimit / Hurry-Up milestone** — Automated validation passed 121 focused
  hurry-up/Fuseball/Pulsar cases and all 289 suite cases, with no failures or
  skips; build had zero warnings/errors. Tests protect exact C# trigger order,
  latch/reconstruction/reset behavior, enemy changes and accounting, not
  arcade fidelity. Neal subsequently manually tested
  `--fuseball-test --lives 10` and `--pulsar-test --lives 10`. Observable
  Fuseball late-wave acceleration matched his Atari expectation
  (**ARCADE VERIFIED** within that observation). Pulsar late-wave behavior
  appeared correct without an expected or observed vertical speed increase;
  the exact top-end redirect remains **REFERENCE VERIFIED**. Existing movement,
  shooting/scoring, rim interaction, life-loss/re-entry and exit appeared
  normal; Pulsar pulse visuals, disappearing affected near/top connection
  and lane hazard also appeared normal. Everything else tested appeared
  normal, with no new problem or regression observed. These manual findings
  do not verify exact numeric rates, internal trigger ticks or latch timing,
  nor authentic wave composition (**DEVELOPMENT ONLY**). Kill-all exit remains
  **PROJECT DECISION**.
