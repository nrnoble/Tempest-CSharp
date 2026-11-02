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

- **PROJECT DECISION** — Normal no-flag startup begins on the closed,
  16-lane Level 1 tube. After successful Level 1 completion, normal play
  progresses automatically into the square, Flipper/canflip Level 2. The
  direct `--level2-test` path remains **DEVELOPMENT ONLY** and is distinct
  from natural Level 2. The Blaster travels around the rim; relative spinner
  movement is the primary input, with left/right keyboard movement as a
  fallback. The input mapping and current movement rates have not been
  established here as arcade-verified tuning.
- **ARCADE VERIFIED** — Neal manually confirmed the broad visible normal
  Level 1-to-Level 2 sequence: the Level 1 tube exit leads automatically to
  square Level 2, with no terminal Level Complete/R pause between levels; the
  displayed identity changes to Level 2 and no stale Level 1 tube frame was
  observed. Score and remaining lives carry across the transition. This
  records observable behavior only, not implementation internals or timing.
  In one manual run, 10 starting lives became 8 after two Level 1 deaths and
  Level 2 began with 8; this is an example, not a fixed life-count rule.
- **PROJECT DECISION (C4; visible behavior manually accepted)** — After
  successful Level 2 exit travel, normal play installs the existing
  `Level3Mixed` configuration as a fresh Level 3 wave, carrying score and
  current lives. Neal manually accepted the visible natural Level 1 -> Level 2
  -> Level 3 sequence, Level 3 entry/gameplay, and terminal/restart behavior.
  This accepts the current C# implementation; it does not establish
  arcade-observed Level 2-to-Level 3 behavior or hidden implementation details.
- **PROJECT DECISION** — Natural Level 1-to-2 and Level 2-to-3 progression
  enters a separate interactive approach state. The fresh incoming wave is
  frozen until arrival; movement uses the existing spinner/keyboard path and
  the left gamepad stick (with a project 0.15 normalized dead zone), while
  fire and Super Zapper are suppressed. Neal manually accepted the current
  approach presentation in normal campaign play: no ExitZoom handoff
  discontinuity, projection flash, wrong board, Blaster jump or new problem
  was observed. He found it impressive and close to remembered Atari behavior.
  This qualitative acceptance does not verify the provisional duration, depth
  offset, curve, controller dead zone or exact projection trajectory. The
  elevated Atari approach perspective remains a deferred fidelity refinement.
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
- **PROJECT DECISION** — Enemy-unit outcomes remain distinct: kill-scored
  destruction, existing natural/escaped outcomes where applicable, and
  non-kill all-rim clearing. A rim-cleared unit is not a destroyed enemy and
  awards no kill score. The recreation can begin exit when incoming/reserve
  Eggs are exhausted and every remaining qualifying enemy reaches the rim;
  otherwise units must resolve through their ordinary outcomes. This
  accounting describes the recreation and does not establish hidden arcade
  accounting.

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
  totals. With Egg sources exhausted, a below-rim Fuseball or Pulsar continues
  to trigger the existing Hurry-Up behavior; when every remaining qualifying
  enemy reaches the rim, all-rim completion takes precedence and begins exit.
  The recreation evaluates all-rim after enemy updates and player-collision
  checks, before Hurry-Up latching and its final kill-all check. This exact
  evaluation order is a **PROJECT DECISION** informed by the reference source,
  not a measured arcade tick/order.

### Completion and tube exit

- **ARCADE VERIFIED** — Neal manually confirmed the broad all-rim behavior in
  all five existing scenarios: ordinary Level 1 (`--lives 10`),
  `--fuseball-test --lives 10`, `--pulsar-test --lives 10`,
  `--tanker-test --lives 10`, and `--spiker-test --lives 10`. In each tested
  scenario, after incoming Eggs were exhausted and all remaining enemies
  reached the rim, those enemies disappeared and Blaster exit began; individual
  kills of those rim enemies were not required. The scenarios passed with no
  new gameplay problem or regression reported. These development compositions
  do not establish authentic arcade wave composition.
- **REFERENCE VERIFIED** — Incitti's source gates the rim-clear path on
  exhausted reserve Eggs and an empty active Egg list, then starts exit when
  either no enemies remain or every listed enemy-type baddie has post-update
  height at least 400. Its predicate is height-based rather than
  animation-state-based; spikes, bullets, and Eggs are handled separately.
  This source behavior does not establish the exact arcade qualification.
- **PROJECT DECISION** — The current C# implementation checks post-update
  `Height >= Level.Depth` for each qualifying active enemy. The exact numeric
  threshold (currently 400) and state qualification are not arcade-verified;
  Neal did not measure timer ticks or exact numeric timing. Player collision
  is processed before rim-clear qualification. Rim-cleared enemies receive no
  kill score and do not increment the kill-only destroyed count. Exact
  evaluation order, pending Super Zapper timer handling, internal accounting,
  collision simultaneity, and Spike-retry accounting remain
  **REFERENCE VERIFIED / PROJECT DECISION** policies unless separately
  observed.
- **ARCADE VERIFIED** — Neal manually confirmed the broad observable
  successful Level 1 exit followed by automatic engagement with square Level
  2, without a Level Complete/R pause. The qualitative Level 2
  square/canflip behavior previously validated in direct access remained
  normal during the natural run. This does not establish exact exit/commit
  ticks or internal setup behavior.
- **PROJECT DECISION** — The C# standard Level 1 exit uses its existing exit
  delay and zoom, then installs fresh Level 2 at the successful-travel
  boundary. The exact `Player.Depth < 10` condition, update ordering,
  fresh-runtime replacement, accounting/list/callback cleanup, projection
  reset, player offset and Egg sequencing describe implementation policy;
  they were not measured as Atari internals.
- **HISTORICAL PROJECT DECISION (before C4)** — Natural Level 2 completion
  was terminal and displayed “LEVEL 2 COMPLETE / NEXT LEVEL NOT IMPLEMENTED
  / PRESS R TO START A NEW
  CAMPAIGN.” R starts a new Level 1 campaign and resets score. Direct
  `--level3-test` access to the mixed Level 3 configuration is **DEVELOPMENT
  ONLY**. This superseded Level 2 policy was a recreation decision, not a
  claim that Atari Level 2 is terminal.
- **ARCADE VERIFIED** — In the natural transition run, Neal observed Level 2
  start with four visible Eggs and the 12-unit wave, score and current lives
  carried, and Super Zapper restored to two usable charges. Previously
  validated square/canflip behavior remained normal and no regression was
  observed. This validates observable outcomes, not exact Egg sequencing or
  the internal timing of the charge/flash reset.
- **PROJECT DECISION** — The C# implementation resets Super Zapper to two
  charges and clears its flash as part of fresh Level 2 initialization. The
  exact reset timing and flash-reset behavior are implementation policy, not
  newly established Atari facts.
- **DEVELOPMENT ONLY** — The Spiker scenario adds a spike-avoidance interval
  and can reverse/retry the exit after a spike collision. This is not part of
  the standard Level 1 clear sequence.
- **ARCADE VERIFIED** — In Neal's manual validation of the isolated,
  **DEVELOPMENT ONLY** `--pulsar-test` scenario, destroying all Pulsars
  permits the established tube-exit sequence. This does not establish an
  authentic all-Pulsar Atari wave.

### Super Zapper

- **ARCADE VERIFIED** — Neal manually validated Super Zapper use in all five
  existing scenarios: ordinary Level 1 (`--lives 10`), `--spiker-test`,
  `--tanker-test`, `--fuseball-test`, and `--pulsar-test`. Super Zapper use
  succeeded in each scenario and no Super Zapper-specific regression was
  observed. This records the observed play-through result; it does not
  validate exact internal selection comparisons, timer values, accounting,
  or measured timing constants. The scenario flags remain **DEVELOPMENT ONLY**
  compositions.
- **REFERENCE VERIFIED** — Incitti's target-selection comparisons, accepted
  target delays (4, 6, 8, and onward for all-target use; 4 for single-target
  use), exact timer/update ordering, eight-update stillborn Tanker cargo
  timers, and 60-step flash lifetime are source-derived. Neal did not measure
  these exact values during manual validation.
- **PROJECT DECISION** — Hidden cargo identities, delayed activation,
  accounting conservation, and completion blocking are recreation mechanics.
  Automated tests establish those C# invariants, not arcade-visible proof.
- **PROJECT DECISION** — In the C# all-rim path, a pending Super Zapper target
  that is still active when rim clearing commits is removed without its later
  kill score. A zap that resolves before that check follows its ordinary
  resolution path. Neal did not validate or measure this timer precedence;
  exact timer/update ordering remains source-derived or a recreation policy.
- **PROJECT DECISION** — For all-rim completion, a Tanker or cargo unit can
  receive a non-kill rim-clear outcome, distinct from a destroyed original
  Tanker, a naturally escaped original, or kill-resolved cargo. Neal's
  scenario validation confirms the visible broad all-rim result, not these
  hidden outcome categories or their accounting.

## 3. Flippers

### Entry and movement

- **REFERENCE VERIFIED** — A Level 1 Flipper hatches at the far end of a lane
  and climbs toward the rim without changing lanes in mid-tube. At the rim it
  pauses, then pivots between adjacent rim lanes while pursuing the Blaster.
  Incitti's relevant reference values are a climb increment of 2, a 24-update
  rim pause, and an 8-degree pivot increment. These exact values have not been
  validated against arcade timing.
- **REFERENCE VERIFIED** — Incitti accepts shots on the Flipper's `OnEdge`
  within the strict band `0 < FlipperHeight - ShotHeight < 16`, awarding
  150 points. Its lane is assigned to the destination when a pivot starts.
- **PROJECT DECISION** — The recreation keeps Flippers shootable in their
  lane while climbing or paused and from either participating lane throughout
  a transition. Two-lane shot acceptance is not literal Incitti ownership.

### Level 2 below-rim lane changes

- **ARCADE VERIFIED** — Displayed Level 2 permits Flippers to change lanes
  while below the rim. On 2026-10-31, Neal manually tested the isolated B2
  scenario and found its broad below-rim lane-changing behavior consistent
  with Atari Level 2.
- **ARCADE VERIFIED** — Moving the Blaster during an already-active
  transition does not visibly retarget it halfway through. Later transitions
  may respond to the Blaster's then-current position. This qualitative
  observation does not establish exact player-sampling points or tick timing.
- **ARCADE VERIFIED** — Below-rim transition geometry itself is not lethal
  to the Blaster, even when it visually crosses the Blaster. Straight-side
  and corner transitions appeared mechanically correct; no snapping or
  wrong-lane landing was observed. Rim pursuit and the previously validated
  completed-pivot collision behavior remained normal.

### Directed enemy pivots and C1/C1.5 manual validation

- **ARCADE VERIFIED** — Neal's 2026-10-31 manual validation of the C1/C1.5
  correction confirmed that Flippers swing inward through the tube/web on
  normal Level 1, Level 2 square, and the Level 3 plus-board geometry
  diagnostic. On the plus board, 90-degree convex and full 270-degree reflex
  pivots swing inward; the reflex route takes the long route rather than the
  opposite 90-degree shortcut. The 180-degree transitions looked sensible,
  Flippers landed on the intended lane, and no outward arc, strange tube
  crossing, snapping or crash was observed. The longer 270-degree transition
  retains the observed opportunity to shoot or move underneath. These are
  observed outcomes, not claims about Atari's internal pivot representation
  or exact tick timing.
- **ARCADE VERIFIED** — In the isolated Pulsar diagnostic, Neal observed
  inward lane changes through the web, sensible direction, and transition
  duration that varied with geometry rather than appearing constant. Pulsing
  and its hazard remained normal; no unrelated regression or crash was
  observed. This does not establish exact Pulsar rates, pauses or timer values.
- **ARCADE VERIFIED** — In the Tanker diagnostic, ordinary Tanker destruction
  released two live cargo Flippers on distinct non-Tanker lanes. Their
  established outward lane route remained correct while their visible pivots
  swung inward; below-rim direction remained stable, and rim arrival and
  subsequent behavior looked correct. Neal observed no new collision problem
  or crash. The exact intermediate Atari lane sequence and timing remain
  unknown.
- **ARCADE VERIFIED** — In the Level 3 diagnostic, Neal reported that Blaster
  direction and movement felt correct while observing the enemy-pivot fix.
  This does not establish exact Blaster sensitivity or timing and does not
  apply the enemy pivot rule to the Blaster.
- **PROJECT DECISION** — Destination lane/direction, full directed
  sweep/duration, and spatial pivot side are separate properties in this
  recreation. A required 270-degree route retains its full directed sweep and
  duration while its visible enemy arc swings inward; the spatial correction
  must not replace it with a 90-degree shortcut. This is an implementation
  representation, not a claim that the arcade uses the same data model.
- **REFERENCE VERIFIED** — Incitti disables `canflip` only at zero-based
  level 0 and enables it thereafter. Its ordinary Level 2 path adds 2 height
  on the first climb, enters an initially zero pause, attempts firing, then
  chooses direction relative to the current player lane. The next pause
  update adds 1 height and begins an adjacent turn. Below-rim direction stays
  latched across turns and pauses; rim pause expiry samples the player again.
- **REFERENCE VERIFIED** — Source-derived Level 2 tuning is an 8-degree
  turn increment, 21-update below-rim pause and 24-update rim pause. Turning
  adds 2 height and below-rim pauses add 1. Exact random/tie behavior,
  random-call ordering, state decisions, transition boundaries, height
  increments and numeric shot band were not manually measured and are not
  arcade-verified by this run.
- **PROJECT DECISION** — B1/B2 use square endpoint hinges with radial
  correction for the rounded source segment lengths, clamp height at the rim,
  anchor active rendering to the source lane and commit the destination on
  completion. This math and internal Flipper representation do not describe
  observed arcade internals. Two-lane shot acceptance, shot-before-player
  collision ordering and exact collision/state boundaries remain recreation
  policy where the manual run did not distinguish them.
- **DEVELOPMENT ONLY** — `--level2-test` remains direct Level 2 access and
  replays that scenario; it does not select or advance a campaign. The square
  configuration and B1/B2 behavior are shared with natural Level 2. Automated
  tests establish C# lifecycle and accounting contracts, not arcade facts.
  Neal reported no B2 crash, and the B1 completed-pivot renderer crash did
  not recur; this is scenario stability evidence, not proof of arcade
  geometry or internal implementation.

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

### Manual arcade validation

- **ARCADE VERIFIED** — In the ordinary Level 1 scenario, Flippers travel
  directly up their lanes without reacting to Blaster left/right movement
  below the rim, then begin pursuing the Blaster after reaching rim behavior.
- **ARCADE VERIFIED** — Tanker-released cargo Flippers do not dynamically
  follow Blaster movement while traveling below the rim.

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
- **DEVELOPMENT ONLY** — Enemy bullets are not enabled in the ordinary
  Level 1/2 launch; `--enemy-bullets-test` enables development firing policies
  in scenarios that support them. Bullet hits do not count as enemy kills.
  The recreation clears bullets on life reconstruction and when exit begins.
- **DEVELOPMENT ONLY** — Spiker retreat bullets require the Spiker and enemy
  bullet test options together. Isolated `--tanker-test` bullets require the
  enemy-bullet option; production-shaped Tanker bullets in direct
  `--level3-test` access are visible without it. Neither development route
  establishes unknown arcade firing probabilities or timers.

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
- **DEVELOPMENT ONLY / PROJECT DECISION** — Spike hazards are separate from
  enemy all-rim qualification and remain available during the existing Spiker
  exit behavior. Their exact interaction with rim clearing, reverse, and retry
  accounting is not established by Neal's all-rim observations.

## 6. Tankers and cargo Flippers

- **ARCADE VERIFIED** — Neal's Atari observation is that Tankers fire
  individual shots at intervals and at least two can occur before the
  release/rim area. In the 2026-10-31 C2 manual validation, Neal observed one
  uninterrupted recreation Tanker naturally fire two individual, separated
  shots before release, matching that expectation. The carrier kept climbing,
  with no firing burst at release; existing bullets survived carrier release
  or destruction. Cargo, Super Zapper and input remained normal, with no
  unrelated regression. The silent control remained silent. This validates
  observable capability only, not exactly two shots for every Tanker.
  Exact Atari ammunition, initial delay, probability, interval, cutoff and
  per-Tanker shot count remain unknown; source constants are not promoted.
- **REFERENCE VERIFIED** — The Incitti Tanker can carry two Flippers. It
  climbs on a lane and releases live cargo either naturally near the rim or
  when destroyed by a player shot. A shot destruction awards 200 points;
  natural release awards no Tanker kill score. Incitti creates each child in a
  mandatory initial pivot toward the Tanker's former lane. These source rules
  describe Incitti, not necessarily Atari Tanker behavior.
- **ARCADE VERIFIED** — When a player shot destroys a Tanker, the Tanker is
  destroyed but both released cargo Flippers survive and appear in two
  different lanes other than the Tanker's former lane. This ordinary shot
  release produces live cargo, distinct from the Super Zapper's delayed
  Tanker resolution and stillborn cargo path described below. The final
  observed recreation behavior is two live children emerging in distinct
  non-Tanker lanes; the exact intermediate Atari lane sequence is not
  established.
- **ARCADE VERIFIED** — In Neal's manual test, another player shot already
  traveling through the tube when the Tanker was shot caused no unusual cargo
  behavior.
- **PROJECT DECISION** — The recreation uses deterministic outward initial
  pivots to produce the observed two-live-child, distinct non-Tanker-lane
  result and keep the former Tanker lane clear. Neal's 2026-10-31 manual
  validation confirmed this lane-routing result and separately observed that
  the children's visible pivots swing inward through the web. The route and
  its distinction from pivot side are recreation behavior; the complete
  unseen Atari route remains unknown. Exact pause timing, climb increments,
  and Tanker release timing remain unverified.
- **REFERENCE VERIFIED** — The source-derived route has an initial inward
  pivot; this conflicts with Neal's direct arcade observation of the final
  visible release result. Direct arcade evidence takes precedence.
- **REFERENCE VERIFIED** — Super Zapper delayed Tanker resolution uses the
  Tanker cargo path to create stillborn children with delayed resolution.
  Exact timer values and accounting transitions are source/test details, not
  visually verified arcade timing or hidden-ledger behavior.
- **ARCADE VERIFIED** — Neal manually validated Super Zapper use in the
  development Tanker scenario. This scenario-level validation does not by
  itself verify exact stillborn timer duration, internal cargo accounting, or
  delayed-resolution update order.
- **REFERENCE VERIFIED** — Incitti's early-tier Tanker climb expression
  `1.75 + lv / 20` uses integer division. At displayed Level 3 (`lv = 2`),
  this yields 1.75, not 1.85. The recreation uses that literal increment;
  it is not arcade-measured timing.
- **DEVELOPMENT ONLY** — The C# Tanker test scenario creates one deterministic
  Tanker among the ten original enemy units; the other slots hatch as
  Flippers. The test Tanker starts at height 10, uses a fixed 1.75 climb
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
- **PROJECT DECISION** — In the implemented all-rim path, Fuseball
  qualification is height-based and does not depend on lateral position or
  action state. The all-rim observation in the Fuseball development scenario
  validates the broad disappearance/exit result, not a numeric cutoff or
  state-specific qualification.
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
- **PROJECT DECISION** — In the implemented all-rim path, Pulsar qualification
  is height-based and does not depend on pause, flip, pulse, or other action
  state. The all-rim observation in the Pulsar development scenario validates
  the broad disappearance/exit result, not a numeric cutoff or
  state-specific qualification.
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
| `--level2-test` | Direct access to the shared closed 16-lane square, supply-12 Flipper-only canflip configuration; terminal development replay, no progression selection. |
| `--level3-geometry-test` | Direct access to the Flipper-only Level 3 plus-board geometry diagnostic; separate from the mixed production-shaped wave. |
| `--level3-test` | Direct graphical access to the existing production-shaped mixed Level 3 configuration; terminal replay without enabling natural progression. Production Tanker bullets are visible without `--enemy-bullets-test`; ordinary/cargo Flippers are silent by default. |
| `--spiker-test` | Replaces the normal wave with a Spiker/Flipper test wave and height-60 spikes on every lane. |
| `--tanker-test` | Replaces the normal wave with one controlled Tanker plus ordinary Flipper slots and cargo. |
| `--fuseball-test` | Direct-hatches the ten-unit supply as Fuseballs, with source-derived late-wave hurry-up. |
| `--pulsar-test` | Direct-hatches the ten-unit supply as Pulsars, with source-derived late-wave hurry-up. |
| `--enemy-bullets-test` | Enables the development enemy-bullet behavior; combinations with Spiker or Tanker scenarios enable their corresponding source-backed firing paths. |
| `--lives <n>` | Overrides the starting life count for longer development sessions. |

The Level 2 and enemy-specific wave options are mutually exclusive. Deterministic random
seams, controlled selection, artificial wave composition, extra lives, and
test-only rendering/collision instrumentation are all **DEVELOPMENT ONLY**.
Passing a simulation test establishes a C# invariant, not arcade fidelity.
- **DEVELOPMENT ONLY — C3b manual graphical acceptance (C# behavior only)** —
  Across multiple direct Level 3 runs, Neal accepted the entry/HUD, visible
  production Tanker bullets without `--enemy-bullets-test`, silent default
  ordinary/cargo Flippers, player-shot and natural Tanker cargo release,
  Super Zapper behavior, death/re-entry, and completion/replay. No crashes or
  other observed problems were reported; overall rating: **5/5 —
  "Impressive."** This does not verify hidden identity sets, RNG ordering,
  exact timers/probabilities, or unknown Atari behavior.
- **DEVELOPMENT ONLY** — Neal manually checked that the isolated Pulsar
  scenario does not spawn unrelated enemy types. This validates the test
  scenario's isolation, not authentic arcade wave composition.

## 11. Known uncertainties and deferred behavior

- **ARCADE VERIFIED scope** — Direct arcade observations explicitly listed
  above cover the Flipper pivot collision rule, the qualitative Level 2
  below-rim transition observations, inward Flipper/Pulsar/cargo pivots and
  the observed full 270-degree route, the itemized Fuseball
  behaviors, the Pulsar behaviors Neal observed in the isolated development
  scenario, the listed Super Zapper scenario validation, the Tanker-cargo
  release and movement observations, and the broad all-rim behavior Neal
  manually confirmed in all five existing scenarios. No additional behavior
  is implied.
- **REFERENCE VERIFIED only** — Exact Flipper, bullet, Spiker, Tanker,
  Fuseball, and Pulsar constants remain source-derived unless explicitly
  identified above as arcade-validated. This includes the Pulsar 51/181/18/14
  timings, Fuseball's nine-update lateral cadence and its 3.3-to-6.6 hurry-up
  climb terms. Exact rimit trigger timing, random decision boundaries and
  source lifecycle remain source-derived; the C# evaluation/freeze/reset
  policy remains **PROJECT DECISION**, not newly arcade-verified. Neal did not
  measure all-rim timer ticks or exact numeric timing. The exact
  `Height >= Level.Depth` threshold, update order relative to hidden internal
  state, pending Super Zapper timer behavior, Tanker identity/accounting,
  collision simultaneity, and Spike-reversal accounting are not
  **ARCADE VERIFIED**.
- **Unresolved** — The direct climbing-Flipper rim-arrival collision is in
  Incitti and current C#, but has not been separately confirmed against the
  arcade. The older development note described rim death only at completed
  pivot; later source review distinguished direct arrival from pivoting.
- **Unresolved** — The repository records a suspected case of two rim
  Flippers converging on the same lane, but it has not been reproduced or
  compared with the arcade.
- **ARCADE VERIFIED** — Neal observed that once reserve/incoming Eggs are
  exhausted, all remaining enemies reaching the top rim is sufficient for the
  remaining enemies to disappear and tube exit to begin. Neal manually
  confirmed this broad result in normal Level 1 and the Fuseball, Pulsar,
  Tanker, and Spiker development scenarios. Exact qualifying heights,
  timer/tick timing, and enemy-state rules are not established by those
  observations.
- **Unresolved** — Exact player input sensitivity, shot cadence, death
  presentation/timing, Egg pacing, normal enemy-bullet frequency, Spiker and
  Tanker arcade eligibility/composition, and exit visuals need direct arcade
  comparison where they matter.
- **PROJECT DECISION** — Progression beyond terminal Level 3 and authentic
  later-wave composition, open-tube rendering, bonus rules, broader
  enemy/cargo combinations, and source-specific death effects remain out of
  scope or deferred. Natural Level 1-to-Level 2-to-Level 3 progression has
  passed manual acceptance of visible C# behavior. This acceptance does not
  establish hidden implementation details or new arcade behavior; remaining
  uncertainties are recorded above. Audio and sound fidelity remain a
  long-term roadmap item; no audio playback is implemented.
- **ARCADE VERIFIED** — Neal observed a brief Atari between-level approach
  after the Blaster exits a tube: a first-person view moves through space
  toward the next tube, which begins small/distant and grows as the player
  approaches; the Blaster is visible on the distant rim and spinner movement
  moves it around that rim during the approach, before combat engagement.
  This qualitative observation does not establish duration, zoom rate,
  projection formulas, enemy-start timing or exact firing enablement.
- **PROJECT DECISION (implementation; graphical acceptance pending)** —
  Natural production progression enters an explicit approach state after the
  successful exit commit for both Level 1 -> 2 and Level 2 -> 3. The incoming
  configuration and fresh wave are installed at approach start, but gameplay
  simulation is frozen until arrival.   spinner, keyboard and left-stick (with a project 0.15 normalized dead zone)
  movement remain enabled; fire and Super Zapper are suppressed. Rendering
  applies a transition-only depth offset to the incoming tube and Blaster and
  reaches the exact normal projection at arrival. The deterministic
  120-simulation-tick duration, 240-unit initial depth offset and smoothstep
  curve are provisional project presentation tuning, not arcade-verified
  values. Automated tests validate state, freeze, movement and projection
  endpoints; Neal's graphical acceptance of both natural approaches is still
  required. Do not infer exact Atari timing, curve, projection constants or
  internal input policy from this implementation.

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
- **Super Zapper manual validation** — Neal manually validated Super Zapper in
  the ordinary, Spiker, Tanker, Fuseball, and Pulsar scenarios. No
  Super-Zapper-specific regression was observed. This is **ARCADE VERIFIED**
  only for the exercised observable gameplay; exact selection comparisons,
  4/6/8... delays, timer ordering, eight-update stillborn countdowns, the
  60-step flash lifetime, and internal accounting remain
  **REFERENCE VERIFIED** or **PROJECT DECISION**, not measured arcade facts.
- **Tanker-cargo correction** — Neal observed that ordinary Level 1 Flippers
  climb their lanes without below-rim pursuit and pursue at the rim. He
  separately confirmed that Tanker cargo does not track the Blaster below the
  rim, that a player-shot Tanker destruction leaves both cargo Flippers alive
  in distinct non-Tanker lanes, and that another in-flight shot produced no
  unusual cargo behavior in his test. The 2026-10-31 C1/C1.5 manual validation
  additionally confirmed the children's visible pivots swing inward through
  the web while the established outward lane route, fixed below-rim
  direction, rim arrival and subsequent behavior remain correct. These
  observed results are **ARCADE VERIFIED**. The exact intermediate Atari
  route, pause timing, climb increments, Tanker release timing, unlock level,
  and `canflip` progression remain unverified. Outward initial lane routing
  remains a **PROJECT DECISION** matching the observed result; it is distinct
  from the spatial direction of the pivot.
- **All-rim completion implementation and manual validation** — Neal manually
  ran and passed normal Level 1 (`--lives 10`) and the Fuseball, Pulsar, Tanker,
  and Spiker development scenarios. He observed that after incoming Eggs were
  exhausted, all remaining enemies reaching the rim caused them to disappear
  and Blaster exit to begin; no new gameplay problem or regression was
  reported. This broad visible result is **ARCADE VERIFIED**. The C# uses
  post-update height qualification and processes player collisions first, but
  Neal did not measure the exact threshold or timing. Exact ordering relative
  to hidden state, pending zap behavior, Tanker accounting, collision
  simultaneity, and Spike-retry accounting remain **REFERENCE VERIFIED /
  PROJECT DECISION**, not newly arcade-verified.
- **Natural Level 1-to-Level 2 progression** — Neal manually ran
  `dotnet run --project src/Tempest.Game -- --lives 10`. He observed the
  normal Level 1 tube exit, automatic square Level 2 entry without a terminal
  pause, Level 2 identity/render change, unchanged score and remaining lives,
  four visible Eggs, a 12-unit wave, two usable Super Zapper charges, and
  previously validated square/canflip behavior without a reported regression.
  In that run, 10 starting lives became 8 after two Level 1 deaths and Level 2
  began with 8. This observable behavior is **ARCADE VERIFIED** within the
  manual comparison; the example is not a fixed life rule. No transition ticks
  were measured. The exact `Player.Depth < 10` commit, update ordering,
  runtime replacement, internal accounting, callback/list cleanup, projection
  reset, player offset, Egg sequencing, and Zapper recharge/flash-reset timing
  remain **REFERENCE VERIFIED / PROJECT DECISION** as applicable, not arcade
  observations.
- **Pre-C4 Level 2 terminal behavior and restart** — Neal observed no Level 3, the
  explicit terminal message, and R returning to a fresh Level 1 with score
  reset. This historical C# terminal and R policy was a **PROJECT DECISION**;
  it did not claim that Atari Level 2 is terminal and was replaced by C4.
- **C4 natural Level 2-to-Level 3 implementation** — Automated tests verify
  that successful natural Level 2 exit installs the existing `Level3Mixed`
  configuration with fresh wave state, carried score/lives, and the
  configuration's two-charge Super Zapper state; Level 3 has no successor.
  Direct development scenarios remain isolated. Neal manually accepted the
  visible natural progression, Level 3 board/display/HUD, fresh mixed wave,
  two usable Super Zapper charges, gameplay spot-check, terminal behavior and
  restart in normal campaign play. This validates visible behavior of the
  current C# implementation; neither tests nor manual acceptance establish
  hidden scheduling/internal behavior or new arcade evidence.
- **Between-level approach presentation** — Neal reports that Atari shows a
  brief first-person approach through space to the next tube. The distant
  tube grows in view; the Blaster appears on its distant rim and remains
  spinner-movable during approach, then combat begins. This gameplay-relevant
  approach is implemented for both natural production transitions and Neal
  manually accepted its current C# presentation. He observed no ExitZoom
  handoff discontinuity, projection flash, wrong board, Blaster jump or new
  problem, and found it impressive and close to remembered Atari behavior.
  These observations accept the current implementation; they do not establish
  exact arcade timing or projection parameters. The elevated Atari approach
  perspective (approximately 30-45 degrees by Neal's observation) remains a
  deferred fidelity refinement; Incitti's approach appears sharper and is an
  engineering reference, not behavioral authority.
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
  nor authentic wave composition (**DEVELOPMENT ONLY**). The later all-rim
  completion milestone superseded the former kill-all-only completion policy.
