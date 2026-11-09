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

Atari ROM statements in this document refer to the local master disassembly
under `Reference/arcade`, including `Tempest Rom Source code.md`. Other Tempest
disassemblies, especially console versions such as Atari 2600, are not
interchangeable evidence for the arcade ROM. Do not consult alternate online
disassemblies as evidence for arcade behavior.

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
  -> Level 3 sequence and Level 3 entry/gameplay. This accepts the current C#
  implementation; it does not establish
  arcade-observed Level 2-to-Level 3 behavior or hidden implementation details.
- **PROJECT DECISION (D1; listed visible behavior manually accepted)** — The
  isolated `--level4-test` diagnostic loads a source-derived Level 4 production
  configuration. Neal accepted the late-wave Spiker behavior and overall
  Level 4 feel. Neal manually confirmed that shortened persistent Spikes remain
  shootable after ordinary death/reconstruction. He also accepted the
  corrected long-Spike exit behavior: continuous fire still damages the Spike,
  but the Blaster reaches the remaining Spike and is destroyed. These are
  visible-behavior observations, not claims about hidden Atari identity
  accounting or general arcade fidelity.
- **PROJECT DECISION (D2; visible behavior manually accepted)** — Natural Level 3
  completion now proceeds through the existing approach into the same Level 4
  production configuration used by `--level4-test`. A successful Level 4
  Avoid Spikes exit enters production Level 5. At that earlier milestone,
  Level 5 was temporarily terminal; the later Level 6 implementation below
  supersedes that boundary. Neal manually accepted the visible natural
  Level 1 -> 2 -> 3 -> 4 campaign; this does not establish hidden implementation
  details or general arcade fidelity.
- **REFERENCE VERIFIED (Level 5 source findings)** — The Original Arcade
  campaign's displayed Level 5 is the closed cross on board 4, palette 0,
  with 16 lanes, center Y 415, offset -90, 20 original obligations, four
  initial Eggs, and 16 height-60 Spikes. It uses Flippers, Tankers, and
  Spikers; Pulsars and Fuseballs are excluded. This is the fifth row of
  `Reference/incitti/levelsets/levelset1.txt`; the separate custom
  `levelset5.txt` is not the Original Arcade campaign. The ROM type-minimum
  table further qualifies the Flipper composition: its Level 5 minimum is zero,
  which blocks ordinary bottom Flipper selection while Tanker cargo remains
  possible.
- **PROJECT DECISION (Level 5 implementation; visible behavior manually accepted)** —
  Natural Level 4 installs this configuration only after the successful
  Avoid Spikes exit; score/lives carry and fresh-level state follow the
  existing setup path. The transition remains blue because star colors use
  the exited level. Level 5 was terminal at this earlier implementation
  checkpoint; the Level 6 milestone below supersedes that boundary.
  Automated tests protect the configuration, ledger reconstruction, handoff,
  fresh Super Zapper, and terminal boundary. Neal completed manual acceptance
  through natural production progression:
  - **Level 4 -> 5:** Level 4 completed normally, with normal avoid/shoot
    Spikes exit behavior. The blue-star transition, upcoming cross-shaped
    Level 5 tube, `SUPERZAPPER RECHARGE` message, and spinner-controlled
    Blaster during approach appeared normal; arrival was seamless.
  - **Level 5 startup/gameplay:** The HUD, cross-shaped board, carried score
    and remaining lives, and starting Spikes were correct. Flippers, Tankers
    and released cargo, and Spikers behaved normally. Pulsars and Fuseballs
    did not appear. Two fresh Super Zapper charges were available at startup,
    and Super Zapper behavior appeared normal.
  - **Level 5 completion:** Neal finished the wave normally and reached the
    existing terminal behavior without Level 6 beginning. No crash, bad
    transition, observed accounting problem, malformed board, or other
    obvious regression occurred.
  Together with the previously reconciled manual validation, this completes
  manual acceptance of the implemented production campaign through Level 5
  in the recreation at that checkpoint. These observations do
  not establish arcade equivalence, hidden accounting, or exact timing.
- **REFERENCE VERIFIED (Level 6 source findings)** — The Original Arcade
  campaign's displayed Level 6 is the closed triangle on board 5, palette 0,
  with 16 lanes, center Y 360, offset -40, 22 original obligations, four
  initial Eggs, 18 reserve obligations, and 16 height-60 Spikes. Its ordered
  points are `(-116,145), (-171,145), (-137,72), (-102,0), (-68,-72),
  (-34,-145), (0,-218), (34,-145), (68,-72), (102,0), (137,72), (171,145),
  (116,145), (54,145), (0,145), (-54,145)`. Flippers, Tankers, and Spikers
  are eligible; Fuseballs and Pulsars are excluded. The source-derived Flipper
  turn/lane-pause/rim-pause values are 9/20/23; the ROM-backed production
  firing values are 65 updates and three enemy-shot slots. Avoid Spikes uses
  71 updates at Level 6; the accepted C# 151-update behavior remains for
  Levels 4 and 5. These are reference findings, not new arcade observations.
- **PROJECT DECISION (Level 6 implementation; visible C# gameplay manually accepted)** —
  Natural Level 5 progresses to a fresh Level 6 only after successful Avoid
  Spikes travel, reusing the accepted ledger, exit, initialization, and
  approach paths. The implementation preserves the accepted C# mixed picker,
  including candidate 3 as Tanker; it does not claim literal Incitti picker
  equivalence. The Level 6 avoid threshold is configured as 70 for exactly 71
  updates; Level 4 and 5 retain their 151-update waits. `--level6-test` is
  DEVELOPMENT ONLY, uses the exact production configuration and Flipper
  firing phase, and remains isolated/replayable. At that implementation
  checkpoint, successful Level 6 completion was temporarily terminal, with
  natural restart returning to a fresh Level 1 campaign. The Level 7
  implementation below supersedes that boundary. Focused validation passed 86 tests; `dotnet test
  TempestCSharp.slnx` passed all 719 tests, and `dotnet build
  TempestCSharp.slnx` succeeded with zero warnings/errors. These tests are
  evidence of C# behavior only and do not upgrade these findings to ARCADE
  VERIFIED.
- **MANUALLY ACCEPTED (Level 6; 2026-11-04)** — Neal played Level 6 multiple
  times with no gameplay issues observed and accepted it as the next production
  campaign milestone. **LEVEL 6 MANUAL ACCEPTANCE: PASS.** Levels 1-6 are the
  manually accepted production foundation; Level 7 acceptance is recorded below.
  This acceptance does not establish exact arcade timing, random composition,
  Spiker speed, firing constants, internal accounting, or other hidden
  mechanics as ARCADE VERIFIED. Existing evidence classifications remain.
- **REFERENCE VERIFIED (Level 7 Incitti board findings)** — The Original Arcade
  campaign's displayed Level 7 is the closed clover on board 6, palette 0,
  with 16 vertices/lanes, center Y 310, offset -10, depth 400, ready position
  50, and player reference lane 7. The source-ordered coordinates are
  `(-160,160), (-179,44), (-89,0), (-179,-44), (-160,-160), (-44,-179),
  (0,-89), (44,-179), (160,-160), (179,-44), (89,0), (179,44), (160,160),
  (44,179), (0,89), (-44,179)`.
- **ROM-BACKED (Level 7 source tables)** — The ROM table has 20 base wave
  obligations before difficulty adjustment and selects Flippers, Tankers, and
  Spikers; Pulsars and Fuseballs are excluded. The C# production choice uses
  the table value 20 (four initial Eggs, 16 reserve), rather than Incitti's
  conflicting 25-original formula. The ROM movement program is `$24`, which
  traces as two completed adjacent flips, reverse, three, reverse, repeat,
  with a lead-in at cycle start and no generic 20-tick lane pause. The ROM
  firing values are holdoff 62 and three shot slots. Avoid Spikes threshold 30
  means 31 updates, then the separate 21-update pretravel. These are source
  findings, not new arcade observations; see
  `Reference/arcade/Tempest Rom Source code.md` and the Original Arcade level set.
- **ARCADE VERIFIED (scoped direct observation)** — Neal observed deep
  player-shot Level 7 Tanker cargo siblings moving away in opposite
  directions for two lane changes, reversing, then traveling at least three
  more lane changes before another apparent reversal or rim arrival. This
  records only the described cargo behavior; it does not establish exact
  timing, height threshold, ordinary-Flipper initial handedness, or numeric
  cadence.
- **ARCADE VERIFIED (Level 7 corrections)** — Neal directly observed that
  ordinary individual Flippers do not enter displayed Level 7 from the
  bottom, Tankers enter and release Flippers, and the visible below-rim lane
  changes follow `2-3-2-2-2`. The movement sequence supersedes the earlier
  interpretation of a repeating two/three route for visible Level 7 behavior.
  It does not revise accepted Levels 1-6 or establish numeric movement timing.
- **ROM-BACKED (Level 7 supply and re-entry)** — The ROM table has Flipper
  minimum/maximum 0/5, Tanker 1/2, Spiker 2/4, and a six-active-enemy cap.
  The normal Level 7 picker cannot select an ordinary Flipper because its
  selection paths require a nonzero species minimum. A Tanker consumes one
  generic pending unit; its two Flipper children are allocated separately.
  Death combines unresolved active-below-rim, at-rim, and already-pending
  quantities into generic pending supply and does not restore prior species,
  cargo association, or movement program. A literal one-unit reconstruction
  selects a Spiker; when the no-pending condition is reached, the ROM can
  convert that same Spiker slot into a Flipper-carrying Tanker at the bottom.
  These are ROM traces, not observations of hidden arcade counters.
- **PROJECT DECISION (Level 7 implementation)** —
  Natural Level 6 progression commits to fresh Level 7 only after safe Spike
  exit travel. It carries score/lives, starts a new 20-original ledger and
  16 height-60 Spikes, restores two Zapper charges, clears stale state, and
  uses the accepted mixed-color approach. Level 7 is terminal; natural
  restart starts a fresh Level 1 campaign, while `--level7-test` remains an
  isolated, production-configured diagnostic that replays Level 7 and
  composes with `--lives`. The Level 7 route implements the arcade-observed
  repeating `2-3-2-2-2` blocks with per-Flipper counters and a reversal at
  each block boundary. Its repeat boundary, fixed initial direction `+1`,
  9-degree turn/2-height cadence and zero below-rim lane pause are C# project
  approximations, not arcade-verified timing or handedness. Ordinary bottom
  Flippers are excluded by the Level 7 picker; Tanker cargo Flippers remain.
  Existing 1-6 movement and selection remain on their accepted paths.
  Tankers remain straight-lane climbers. Only deep player-shot Level 7 cargo
  opts into the scoped route with opposite initial directions;
  near-rim/natural cargo retains the accepted path. C# height `<=370` is a
  named approximation for the ROM's along-coordinate threshold, not an exact
  conversion. On Level 7 death, unresolved original selection is cleared and
  released cargo is transferred to generic re-entry records; the C# identity
  ledger remains for accounting. The final-pending Spiker conversion preserves
  its identity and adds only latent Tanker cargo. Using the existing Spiker
  return-to-height-zero boundary as the ROM `$f2`/`$f0` conversion point is a
  project approximation. Fresh Eggs recreate route state rather than restoring
  runtime phase. At this 2026-11-04 Level 7 checkpoint, Level 8 was outside
  scope at that checkpoint; the later ROM-derived Level 8 implementation and
  final manual acceptance are documented below. Automated tests are C#
  behavior evidence only: 37 focused Level 7/firing tests, 190 focused Level
  1-7/shared-seam regression tests, and all 743 solution tests passed. The
  solution build succeeded with zero warnings/errors and `git diff --check`
  passed. Neal's manual acceptance is recorded separately below.
- **MANUALLY ACCEPTED (Level 7; 2026-11-04)** — Neal's final gameplay
  comparison passed. He manually verified that individual Flippers do not
  enter from the bottom, Tankers enter and release Flippers normally, and
  below-rim Flippers follow the visible `2-3-2-2-2` lane-change pattern.
  As a highly experienced original Tempest arcade player, Neal judged the
  corrected Level 7 gameplay extremely difficult to distinguish from the
  original arcade. This is a perceptual/manual acceptance judgment, not a
  claim of cycle-exact or mathematically identical emulation. The specific
  one-enemy death/re-entry visual case was **NOT MANUALLY REPRODUCED**; Neal
  considers it non-blocking, not an observable gameplay problem, and may try
  again later. Its underlying one-unit reconstruction and final-pending
  behavior retain separate ROM-backed and automated-test evidence. Level 7
  is accepted despite this unobserved visual corner case. Cadence, handedness,
  repeat-boundary and coordinate mappings remain documented C# approximations.
- **REFERENCE VERIFIED (D1 source findings)** — Displayed Level 4 is internal
  source index 3, board 3 ("binoculars"), palette group 0, with a closed
  16-lane board, center Y 380, offset -65, 17 original enemy obligations,
  four initial Eggs, and 16 starting height-60 Spikes. Flippers, Tankers, and
  Spikers are eligible; Tankers carry two Flippers. Fuseballs and Pulsars are
  not eligible. The Level 4-7 exit uses spike avoidance. These describe the
  Incitti reference, not arcade-verified C# behavior.
- **PROJECT DECISION — exit policy and warning:** Spike presence is configured
  independently from exit policy. The Level 4 candidate uses the explicit
  `AvoidSpikes` policy; Levels 1-3 use `Standard`. The prominent “Avoid
  Spikes” warning stays visible through the controllable AvoidSpikes and
  pretravel ExitDelay states; it hides when ExitZoom begins, and during death,
  retry, and terminal states.
  **AVOID SPIKES VECTOR TEXT: MANUALLY ACCEPTED.** The previous Raylib default
  font/title-case warning is now uppercase `AVOID SPIKES`, rendered by the
  shared C# `VectorFont` pathway used by GAME OVER. Neal visually tested the
  presentation on all currently implemented Spike levels and reports,
  "Looks perfect." Its approximately 56-logical-pixel height, center at
  `(400,288)`, white color, warning timing, and tube-exit presentation are
  preserved. GAME OVER remains visually unchanged. The original arcade
  presentation's uppercase in-game lettering is **ARCADE OBSERVED**; using the
  shared C# vector font for future arcade-style messages is a **PROJECT
  PRESENTATION DECISION**. This rule does not globally convert the HUD,
  score/lives, diagnostics, development labels, approach/recharge text, or
  other accepted presentation.
  The verified current natural progression has no Spikes on Levels 1-3; Level
  4 is its first normal Spike level, and many later levels also contain them.
  Neal recalls recycled Levels 17 and 18 without Spikes. At this earlier
  checkpoint the Level 8 production candidate used the standard exit on an
  open tube; its later accepted behavior is documented below. Other
  later-level configurations remain unknown. Spike presence must be configured
  per level,
  not inferred from a permanent `displayedLevel >= 4` threshold. Neal's
  earlier recollection of Level 8 as open and V-shaped is separate from the
  later ROM-derived geometry and is not its evidence.
- **ARCADE VERIFIED (visible behavior only; Neal's direct observation)** —
  During the Avoid Spikes warning/pretravel interval, rim movement remains
  available but firing is disabled. Firing becomes available once down-tube
  exit travel begins. During travel, shots damage Spikes, successful hits score
  1, and Spike collision/reverse/retry remains functional. A short Spike may
  be destroyed before contact, but a sufficiently long Spike is lethal if the
  Blaster reaches its remaining tip. Neal's final Level 4 manual revalidation
  passed both post-death shooting and the sustained-fire long-Spike exit
  behavior. A shot already in flight when the enemy wave completes
  is not arbitrarily erased as Avoid Spikes begins. These visible observations
  supersede Incitti's conflicting firing behavior; they do not establish
  hidden implementation details.
- **PROJECT DECISION (Level 4 implementation)** — The configured AvoidSpikes
  path suppresses new shots throughout AvoidSpikes and ExitDelay, enabling
  them only on an update that begins in ExitZoom. Shots already traveling at
  the transition into AvoidSpikes are retained and continue updating and
  colliding with Spikes. Exit travel uses a four-update shot cooldown (the
  ordinary C# cooldown is three) so sustained firing cannot outpace the
  descending Blaster indefinitely. The exit update checks swept player
  contact against the Spike height at update start before advancing shots;
  contact wins over same-update shot damage. This ordering and cooldown are
  C# project decisions, not claims about Atari's hidden mechanism. Neal
  hypothesized that close-range shot-slot recycling caused the failure. The
  C# trace found the 10-shot capacity is not the close-range bottleneck: the
  three-update cooldown permits 10-point damage every three updates, slightly
  faster than 3/3.2-unit travel, and the old collision test used the
  post-shot tip without swept crossing. This cadence/collision explanation is
  a C# implementation finding; the four-update cooldown and swept contact are
  **PROJECT DECISIONS**, not measured Atari constants or verified internal
  ordering. Levels 1-3 standard-exit behavior is unchanged. Neal manually
  accepted post-death Spike shooting and the observable long-Spike collision.
- **PROJECT DECISION** — Natural Level 1-to-2 and Level 2-to-3 progression
  enters a separate interactive approach state. The fresh incoming wave is
  frozen until arrival; movement uses the existing spinner/keyboard path and
  the left gamepad stick (with a project 0.15 normalized dead zone), while
  fire and Super Zapper are suppressed. Neal manually accepted the current
  approach presentation in normal campaign play: no ExitZoom handoff
  discontinuity, projection flash, wrong board, Blaster jump or new problem
  was observed. He found it impressive and close to remembered Atari behavior.
  Neal subsequently accepted the combined elevated-perspective, brighter
  288-point starfield and recharge-message refinement. This qualitative
  acceptance does not establish measured Atari constants, distribution,
  controller dead zone or exact projection trajectory. No further visual
  tuning of this accepted transition is requested.
- **PROJECT DECISION** — Player shots travel from the Blaster at the near rim
  toward the far end of the currently occupied lane. The current recreation
  limits simultaneous player shots and gives them a fixed travel rate. Those
  limits/rates describe this build, not a claim about measured arcade timing.
- **ROM-LISTING-BACKED** — Player shots use the static sparkling vector picture
  at `$1594`: nine point positions in ROM color index 8 and eight in index 1,
  with blank movement and no visible connecting segments. The picture stays
  screen-oriented and does not rotate with lane or shot age; the Mathbox
  supplies depth-dependent external scale through `graphic_at_mid`.
- **ARCADE/MAME-OBSERVED** — Supplied frames show small warm-colored lane marks;
  the bright white target-destruction burst is a separate effect. The captures
  do not establish exact point locations or literal palette RGB values.
- **PROJECT TRANSLATION** — C# maps the decoded
  relative point program to compact pixel marks, converts ROM Y-up to screen
  Y-down, and retains the prior 4.5 scale as the maximum point radius
  `sqrt(11^2+11^2)` under the existing depth projection. The project color
  translations are amber `#FFB000` for index 8 and red-orange `#FF4000` for
  index 1; these are not verified Atari RGB values.
- **ROM-LISTING-BACKED / COORDINATE QUALIFICATION** — The last four relative
  moves decode from raw words at `$3600`, `$3608`, `$3610`, and `$3618` as
  `(0,-11)`, `(0,-11)`, `(+11,0)`, `(+11,0)`, yielding final points
  `(-11,0)`, `(-11,-11)`, `(0,-11)`, `(11,-11)`. Conflicting inline guesses
  are not used to invent a symmetric outer pattern.
- **PREVIOUSLY ACCEPTED PROJECT BEHAVIOR PRESERVED** — The rendering change
  does not alter firing, capacity, cooldown, movement, collision, scoring,
  Spike behavior, recording format, playback, or shot lifecycle.
- **MANUAL ARCADE-FIDELITY ACCEPTANCE** — Neal has manually accepted the
  resulting shot shape, appearance, size, colors, scaling, and overall
  presentation as perfectly recreated. No further Blaster-shot visual tuning
  is currently required. The white type-0 enemy-destruction effect remains a
  separate rendering pass. This acceptance does not turn project RGB values or
  the 4.5 scale conversion into literal Atari hardware constants.
- **UNKNOWN / UNRESOLVED** — Literal ROM RGB values and exact hardware
  sparkle/flicker behavior are not established by the local listing.
- **ARCADE VERIFIED** — Neal observes that the Blaster's occupied lane is
  identified by recoloring its two side boundaries along the full tube depth;
  the highlight follows rim movement, and early implemented levels use yellow.
  This is a gameplay-readable cue that restores the remembered deep-tube
  lane-awareness benefit. Neal manually reproduced and accepted the visible
  behavior across the implemented boards and assessed the feature as identical
  to Atari.
- **REFERENCE VERIFIED** — Incitti selects one indexed player edge and colors
  its two longitudinal rails with `COL_CLAW`; palette 0 maps that color to
  yellow. Pulsar pulse coloring takes precedence over the player color, and
  the player color takes precedence over ordinary/Super Zapper tube coloring.
  These details describe Incitti, not necessarily every Atari palette/effect.
- **PROJECT DECISION** — C# uses `Player.EdgeIndex` as the single highlighted
  lane and colors the longitudinal rails at that vertex and
  `Level.NextEdge(EdgeIndex)`. Fractional `EdgePosition` does not select or
  interpolate lanes; the selected pair changes when the indexed lane changes.
  Palette 0 (current Levels 1-3) resolves to yellow; later palette colors and
  16-level cycling are deferred. Pulsar overlays draw over the player color;
  the player color wins over normal/Super Zapper tube coloring. Neal manually
  accepted the full-depth rails, spinner tracking, lane-change behavior,
  square/corner and plus-board behavior, both level approaches, and the
  Super Zapper/Pulsar interaction, with no wrong adjacent lane, flicker,
  rendering artifact or crash. This accepts the current C# rendering, not
  unimplemented palette behavior.
- **PROJECT DECISION** — Show the lane cue during Playing, Approach,
  PlayerDying and active development AvoidSpikes; hide it during ExitDelay,
  ExitZoom, GameOver and Complete. Approach uses the current player lane and
  existing projected tube geometry. Atari state-by-state visibility beyond
  the observed lane-following behavior has not been established; the visibility
  policy remains a project decision.
- **REFERENCE VERIFIED** — Scoring is awarded when a target is destroyed or
  damaged, with values varying by enemy and target type. The enemy sections
  below record the currently implemented/reference-backed values where known.
- **PROJECT DECISION** — Score survives an ordinary life loss. A full restart
  resets score.

### Lives, death, and reconstruction

- **PROJECT DECISION** — A new game starts with three lives unless a
  development override is supplied. A lethal event starts the recreation's
  60-update death interval; one life is deducted when that interval ends.
  Losing the final life now enters the accepted Game Over presentation
  below before the existing terminal Game Over state.
- **PROJECT DECISION** — During ordinary life reconstruction, active unresolved
  enemies are discarded and re-enter as fresh Eggs. Previously destroyed
  enemies remain destroyed, score is retained, and old enemy positions/state
  are not restored. Level 4 and the Spiker development scenario retain each
  Spike's object and height; production reconstruction also restores surviving
  Spikes to the player-shot collision order. Neal manually confirmed that
  shortened Spikes remain shootable after ordinary Level 4 death.
- **PROJECT DECISION** — The C# simulation freezes enemies and the shared
  Pulsar clock during the full death interval. Incitti advances Pulsars/global
  pulse state during much of its death presentation; the recreation keeps its
  established freeze policy rather than claiming equivalence.
- **ARCADE VERIFIED** — Neal observed active enemies retract backward/down
  the tube during player death and remain inactive while doing so. Existing
  Spikes stay fixed on the web. The complete visible retraction takes no more
  than approximately two seconds; this is a qualitative upper bound, not an
  exact measurement, frame count or interpolation curve.
- **PROJECT DECISION** — The recreation applies a deterministic, linear,
  render-only depth displacement over 60 updates (one second at the fixed
  60 Hz simulation rate), within that observed approximate upper bound. Logical
  enemy height/lane/state and identity/accounting remain unchanged. This
  tuning is not an arcade-exact duration or curve. Eggs and other projectile/
  hazard visibility retain their established C# policy pending footage.
- **REFERENCE VERIFIED** — Incitti uses a shared projection/depth shift during
  its death/vortex presentation; the inspected source has no dedicated
  enemy-specific retreat routine. The Atari observation, not Incitti, is the
  behavioral authority for visible enemy retraction.
- **ARCADE VERIFIED** — The supplied sequential Atari frames 0041-0065 show
  the Blaster-location death effect developing as a compact, angular,
  layered multicolor presentation; expanding substantially; contracting to a
  smaller remnant; and diminishing until it disappears. The tube remains
  visible behind it. Exact geometry, outline count, color schedule, and timing
  have not been measured.
- **PROJECT DECISION** — The C# renderer approximates that observed progression
  with deterministic, layered vector outlines sampled from the existing
  60-update death countdown. Its accepted presentation uses 150% of the
  original implementation's visual radius. The chosen geometry, layer
  scales/orientations, 150% multiplier, and interpolation envelope are project
  implementation values, not arcade-verified measurements. The effect remains
  render-only and does not change ordinary death or Spike reverse-zoom gameplay
  semantics.
- **MANUALLY ACCEPTED** — Neal judged the 150%-scale Blaster death effect
  nearly perfect and accepted its fidelity; no further visual adjustments are
  needed. The project aims to reproduce the arcade experience to an acceptably
  faithful level with reasonable compromises, not to make a frame-perfect or
  identical clone. This acceptance does not upgrade the implementation's
  geometry, timing, or scale constants to arcade-verified facts.
- **MANUALLY ACCEPTED** — Neal accepted the visible C# enemy retraction on
  Levels 1-3, including fixed Spike persistence and final-life Game
  Over terminal/restart behavior at that earlier checkpoint. The accepted
  presentation below is recorded separately; neither acceptance makes the C#
  one-second duration or linear interpolation arcade-verified.

### Game Over presentation (manual acceptance: good enough for now)

- **ROM-BACKED** — The local master disassembly's shared presentation selects
  centered vector text `GAME OVER`, scale 1, palette index 5; selection depends
  on play-state condition and `timectr & $20`. It defines no independent
  Game-Over-only duration. State `$24` is a high-score explosion reached only
  on the qualifying score route, not an unconditional post-Game-Over state.
  It iterates 16 indexed records, seeds each from `$80` coordinates, assigns
  POKEY-derived movement values in the signed 0-7 range. Its bounded
  coordinates use `$10`/`$F0` limits, and it stops spawning
  when `$010E` counts down from `$20` on alternating `timectr` values (32
  decrements over 64 ticks). Existing records continue moving until the
  active-record marker drains to zero, then the state hands off to `$12`.
  The pending-vector renderer uses palette index `$0C`; graphic ID `$1F`
  selects a 16-entry template with two nonzero subrecord values, `$47` and
  `$4A`. This supports multiple vector elements per parent record, but these
  values do not establish the literal displayed geometry.
- **ARCADE OBSERVED (99 sequential frames)** — The ordered filenames
  `Fireworks_frame_0001.png` through `Fireworks_frame_0099.png` show a bright
  central point developing into an asymmetric, changing cloud of many
  point-like marks. The cluster spreads into uneven rays/fans and small
  groupings; multiple groups coexist while new marks appear and older ones
  disperse. The broad extent grows and redistributes through the middle frames,
  then thins to sparse colored remnants in the final frames. White/blue,
  cyan, green, yellow, red/orange and magenta/violet hues visibly coexist.
  These are visual categories, not measured Atari RGB values. People, phone,
  hands, clothing, room, bezel, glass reflections, glare and photographic
  artifacts were excluded from the interpretation.
- **ARCADE OBSERVED (clean MAME sample and direct arcade comparison)** — A
  chronological sample of the 103 files under `Videos/Fireworks-MAME`
  (including frames 0003, 0012, 0020, 0025, 0050, 0075, 0090 and 0103) shows
  the tiny multicolor marks against an otherwise empty black display while
  active. It materially supports, and does not contradict, the prior 99-frame
  structure analysis. Neal directly observes that the tube and GAME OVER
  presentation disappear immediately before this effect and remain absent
  during it. The clean sample also supports the assessment that the marks
  should remain tiny but be more prominent in the C# rendering.
- **ARCADE OBSERVED / MANUAL COMPARISON** — The celebration is associated with
  qualifying for the leaderboard/high-score path.
- **MANUAL ACCEPTANCE: GOOD ENOUGH FOR NOW** — Neal considers the GAME OVER
  text excellent and Tempest-like; no glyph or font tuning is wanted. Tube
  suppression before the celebration is correct, and during the celebration
  only the isolated effect appears against black. The accepted sequence is
  final one-player death, GAME OVER over the tube, isolated celebration on
  black, all burst records draining, terminal Game Over, then R starting a
  fresh Level 1 game. The earlier single-burst result was too sparse. The
  current presentation is sufficiently close for this project stage; further
  visual tuning is deferred.
- **CURRENT C# BEHAVIOR** — After the existing 60-update final-life death
  presentation, the simulation enters `GameOverPresentation`, then
  `GameOverAnimation`, and finally the existing terminal `GameOver` state.
  Gameplay objects are cleared/returned to reserve before the sequence;
  presentation updates do not advance combat, score, lives or wave progression.
  `GameOverPresentation` retains the existing tube and accepted GAME OVER text.
  `GameOverAnimation` clears to the normal black background and emits only the
  vector effect; the tube, web, player, entities, projectiles, explosions, HUD
  and gameplay messages are not rendered. The existing R restart remains
  enabled only in the terminal state. Each active ROM-informed record renders
  as a compact cluster of short vector specks instead of a four-edge diamond.
- **PROJECT DECISION / APPROXIMATION** — The C# sequence is deliberately
  unconditional after every final one-player death, despite the ROM's
  high-score qualification because the recreation does not implement the
  leaderboard/front-end system. The text holds for 120 simulation ticks and
  preserves a `$20`-bit blink cycle using a local phase. Temporary records
  retain the 16-slot capacity, `$80` initial coordinates, graphic ID `$1F`,
  signed movement range, alternating spawn-counter countdown, bounds and
  drain-before-completion structure. ROM coordinates are projected through
  the existing renderer. Each record is translated to a deterministic compact
  two-dimensional cluster of tiny vector specks; child positions do not follow
  the parent's velocity. The renderer-side `--game-over-density` multiplier
  changes child-speck count only: 1 is the nominal two-speck baseline and 0.5
  emits one speck per parent. The accepted default 2 emits four specks per
  parent; fractional values distribute integer counts deterministically across
  parent spawn ordinals. Child offsets use a
  stable parent rotation and golden-angle sequence, with a 3-logical-pixel
  maximum radius. Existing 2.0-logical-pixel line length and deterministic
  cycle through white, yellow, green, red, cyan, magenta and blue vector colors
  are unchanged. Density 2 is **MANUALLY CALIBRATED / PROJECT DECISION**, not
  ROM-derived. The `--game-over-density <positive finite multiplier>` option
  remains a development-only diagnostic; it is not shown in gameplay or
  persisted. Explicit 2 matches the default, and values such as 0.5, 1, and 4
  remain available for future investigation. Shape and color cycle are
  **ARCADE-OBSERVED / PROJECT TRANSLATIONS**, not decoded `$47/$4A` geometry or
  ROM palette assignments; additional cluster children represent unresolved
  visible structure, not extra ROM simulation records. Exact Atari RGB values
  remain unknown. The rare case where both bounded velocities are zero is
  given a one-unit motion so the presentation cannot hang. The frame sequence
  has no established frame rate, so C# duration remains a tick-based
  approximation; the ROM's alternating spawn counter is preserved.
- **ARCADE OBSERVED / MANUAL COMPARISON** — Neal identifies approximately four
  distinct burst patterns in rapid succession. Each later pattern appears to
  begin around halfway through the preceding pattern's evolution; the patterns
  overlap, differ rather than simply repeat, and their overlap contributes
  substantially to perceived richness. With this presentation, density 2
  gives the best overall brightness/richness; 0.5 was preferred for individual
  specks before the overlapping bursts were introduced.
- **ROM-BACKED LIMIT** — State `$24` has one 16-slot record loop, a 64-tick
  spawn window from `$010E`, and reuses empty slots as records leave bounds.
  Each parent selects graphic `$1F`, whose template has two nonzero values
  (`$47` and `$4A`). The inspected code does not establish four record groups,
  four staggered onsets, a records-per-burst count, or four origins.
  Randomized record initialization and later slot reuse can produce
  successive activity, but do not prove Neal's observed four-event grouping.
- **ARCADE OBSERVED (targeted clean MAME sample)** — Frames 0001, 0003-0008,
  0012, 0020, 0025, 0032, 0050, 0075, 0090 and 0103 were sampled
  chronologically, with adjacent early frames used to distinguish onset. The
  sampled marks develop from a central region and spread asymmetrically; they
  do not confidently establish four separate screen origins or a four-group
  mapping. No exhaustive frame-by-frame analysis was repeated.
- **ARCADE OBSERVED / PROJECT TRANSLATION (four-burst model)** — The C# effect runs
  four independent instances of the ROM-informed 16-parent animation. Their
  start offsets are 0, 32, 64 and 96 simulation ticks. The ROM-inspired
  alternating counter gives each instance a 64-tick spawn window, so a new
  instance starts halfway through the preceding instance's spawn phase while
  its records continue moving and draining. Each instance retains its own
  16-slot capacity, movement bounds, randomized signed velocities, spawn/drain
  lifecycle and deterministic seeded behavior. All use the same central
  `$80` starting coordinates; no unsupported per-burst origin offsets are
  added. The seeded random stream and a stable burst-index variation in the
  compact speck arrangement produce distinct procedural patterns without
  preset pictures or fixed per-burst colors. Density remains a per-burst
  child-speck multiplier, and the 2.0-logical-pixel mark geometry is unchanged.
  The observed grouping is translated into a project schedule; the four
  instances, exact timing and pattern variation are project choices, not ROM
  findings. Neal manually accepts the current presentation as good enough for
  now; exact fidelity may be revisited later.
- **UNKNOWN / PENDING** — Exact vector paths behind `$47/$4A`, literal RGB
  values and color assignment, whether apparent short trails are geometry or
  display/camera persistence, and timing in real seconds remain unresolved.
  The final supplied frame still contains sparse marks, so it does not prove
  the complete arcade end state. The accepted C# presentation is not a claim of
  exact vector, color, or real-time fidelity.

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
  tick uses hurry-up. If a collision starts PlayerDying during those checks,
  remaining same-tick Hurry-Up/all-rim/completion processing is skipped.
  An already latched value survives the death freeze and fresh-Egg
  reconstruction. Full game restart or replay initialization clears it. These
  exact C# timing and lifecycle rules are not arcade-verified by manual
  gameplay.
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

- **PROJECT DECISION — ordinary-exit lane control correction (2026-11-03)** —
  Actual `ExitZoom` travel on Levels 1-3 polls spinner, keyboard and controller
  input and uses the same normal player movement path as the working Level 4-5
  Spike exit. Ordinary `ExitDelay` remains frozen; firing gates are unchanged.
  The existing 21-update delay, 124-update travel, projection progression and
  successful between-level approach handoff are unchanged. Avoid Spikes
  waiting, Spike collision/reversal/retry and Spike firing/scoring are unchanged.
  Automated coverage passes. Neal manually accepted the C# correction on
  2026-11-03: lane changes work during actual tube-exit travel on every
  implemented Level 1-5, including corrected ordinary exits on Levels 1-3 and
  the previously working Spike/Avoid Spikes exits on Levels 4-5. No new
  gameplay issue was observed. This is C# acceptance, not new arcade verification.
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

#### Accepted color presentation (2026-11-05)

- **MANUAL ACCEPTANCE: PASS** — Neal manually tested the corrected Super
  Zapper visual presentation and reports that it is very good. No further
  visual or gameplay changes are needed.
- **ROM-BACKED** — While `zap_running` is positive, unoccupied longitudinal
  rails use `phase = timectr & 7`, `index = phase == 7 ? 1 : phase`, producing
  the repeating slot sequence `0,1,2,3,4,5,6,1`. This uses the free-running
  global time-counter phase; activation does not restart the cycle, so the
  first visible slot depends on the current phase. Pulsar lane handling wins
  first, occupied Blaster boundaries remain slot 1, and other longitudinal
  rails receive the active Super Zapper slot. Near/far circumferential rim
  connections retain normal slot 6. Player and enemies retain their ordinary
  colors, and normal rail colors return when the presentation becomes inactive.
  Both successful Super Zapper presentations use this same color rule.
- **PROJECT TRANSLATIONS** — C# maps slots 0-6 to existing white, palette-0
  Blaster yellow, green, red, cyan, magenta and blue. The available ROM
  listing omits the packed palette bytes at `$c1fd` as `.chunk 8`; exact Atari
  RGB output remains UNKNOWN. These C# hues are project translations, not
  literal ROM-derived RGB values.
- **PROJECT DECISION** — The accepted C# presentation envelope and Super
  Zapper gameplay semantics are unchanged; this is a visual-fidelity
  correction only.

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
- **ARCADE VERIFIED (closed rims only)** — For Flippers on the currently
  implemented closed-rim Levels 1-5, once pursuit direction is established it
  does not reverse merely because the Blaster crosses behind and makes the
  opposite route shorter. The observation verifies persistence, not the hidden
  direction-selection implementation or exact selection tick, and does not
  establish a universal rule for later levels.
- **PROJECT DECISION** — C# Flippers, including Tanker cargo, store rim pursuit
  direction separately from any below-rim route direction. The first rim
  destination on current closed-rim geometries uses the existing player-relative
  selection; later rim destinations retain that direction until the enemy is
  destroyed or replaced. Fresh Eggs/hatches, cargo reconstruction, new levels
  and restarts create new enemy instances and therefore start without stale
  pursuit   state. Future open-tube endpoints may legitimately require reversal.
  At the time of this earlier checkpoint, Level 8 topology-aware endpoint
  handling was a project translation still pending manual acceptance; the
  accepted result is documented in the Level 8 section below. This does not
  establish a universal later-level rule.
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
  latched across turns and pauses. At the rim, Incitti's Flipper calls
  `FindShortDir` again at each pause expiry before stepping to the next lane;
  this literal source behavior differs from Neal's arcade-verified persistence
  observation.
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

### Rim pursuit direction correction

- **ARCADE VERIFIED (closed rims only)** — Neal manually accepted persistent
  pursuit direction for the currently implemented closed-rim Levels 1-5: a
  Flipper does not reverse merely because the Blaster moves behind it and the
  opposite route becomes shorter. This does not establish a universal rule
  across later difficulty levels.
- **OPEN-RIM NOTE** — Physical endpoints on open/unconnected tubes can
  legitimately require reversal. Neal's earlier recollection of Level 8 as
  open and V-shaped is not the geometry evidence; the later candidate uses
  local Rev 2 ROM data; Neal later manually accepted the Level 8 endpoint
  behavior as documented in its production section. Do not extend the
  closed-rim pursuit latch to unspecified later levels.
- **REFERENCE VERIFIED** — Incitti's Fuseball has separate lateral movement
  and vertical excursions; its direction can change on a probabilistic
  lateral update. Tankers climb and release cargo but do not chase on the rim.
  Spikers build and retreat on their lane. These three types have no
  rim-pursuit direction latch in C#.
- **PROJECT DECISION** — C# Tanker cargo retains its outward below-rim lane
  route, then uses the Flipper rim-pursuit latch after arrival. Cargo's initial
  release direction and later player-pursuit direction are distinct state.

### Rim pursuit speed calibration and manual acceptance (2026-11-05)

**ARCADE/MANUAL ACCEPTED:** Neal manually tested the normal 2.5-calibrated
rim-pursuit behavior across implemented Levels 1-7 and reported that the
levels feel right. The calibration materially corrects the original
stationary-defense problem: camping in one rim lane and single-tapping or
holding Fire no longer makes it extremely easy to destroy nearly every
Flipper after rim arrival. Chasing Flippers create meaningful pressure, so
normal spinner movement, awareness and shot timing matter again. Temporary
stationary defense and careful shooting remain useful, and Neal observed no
regression across Levels 1-7. This is perceptual manual acceptance of the
implemented behavior, not proof of hidden arcade internals.

- **PROJECT DECISION / MANUAL CALIBRATION** — The accepted normal C# rim
  angular rate is 2.5 times each level's former configured Flipper turn rate.
  Neal's direct arcade comparison is the authority for choosing 2.5. It is
  not a literal multiplier established by the Atari ROM.
- **ROM-BACKED** — Atari has distinct top-of-tube Flipper processing and
  level-derived top acceleration. This evidence supports a distinct,
  accelerated rim phase but does not establish the C# 2.5 calibration.
- **DEVELOPMENT DIAGNOSTIC** — `--rim-speed <multiplier>` remains available
  around the accepted normal rate. Omitting it and specifying `--rim-speed 1`
  produce identical accepted behavior. `--rim-speed 0.4` approximately
  reproduces the former pre-calibration C# rim rate; other positive finite
  values remain diagnostic multipliers.
- **BOUNDED ROM FINDING** — The traced master-ROM rim-pursuit flow contains no
  explicit one-lane-from-player speed, acceleration, delay, or top-turn-factor
  branch and no special faster adjacent-lane pounce state. The ordinary
  top-rim movement continues until contact in the inspected flow. This bounded
  result does not prove that no such behavior exists elsewhere; geometry,
  directed-pivot angle, contact timing or perception could explain the
  reported pounce.
- **REJECTED EXPERIMENT** — Neal's A/B test found no useful perceptual benefit
  from the player-shot/Flipper contact-ordering experiment and found its shot
  timing/feel subtly wrong. Do not treat that experiment as accepted behavior
  or restore it. The accepted production ordering remains the behavior from
  baseline `3bebf42cbe9514846ab32bf4c8a12bcfdb048da7`.

The acceptance covers the implemented closed-rim Levels 1-7 only. Open-tube
endpoints on future levels may require different pursuit behavior; do not
extrapolate the closed-rim pursuit rule to them. Rim pause, below-rim movement
(including Level 7's accepted `2-3-2-2-2` route), Flipper and Blaster firing,
player shots, Super Zapper strategy, and all other gameplay behavior remain
unchanged by this calibration.

## 4. Enemy bullets

- **ARCADE/MANUAL ACCEPTED (production Flipper firing)** — Neal observed that
  Level 1 Flippers fire while climbing, with reasonably sparse shots, no
  machine-gun behavior or obvious bullet flood, and one bullet sound per
  actual shot. In Level 2 they continue normal below-rim lane changing, visibly
  fire before reaching the rim, and do not leave the entire level silent.
  Lane transitions may temporarily block emission but do not cause prolonged
  firing droughts; individual Flippers generally produce only a small number
  of shots. In Levels 3-5, ordinary and Tanker-released cargo Flippers fire
  sensibly, Tanker firing remains normal, and shared enemy-shot pressure
  prevents excessive bullet volume. Flipper movement, geometry, directed
  pivots, rim pursuit, death/re-entry, progression, and Level 5 behavior
  remain unchanged. No new issues were observed. This is Neal's visible
  gameplay acceptance, not manual verification of numeric ROM constants.
- **REFERENCE VERIFIED (Atari ROM)** — The global `enm_shoot` phase runs after
  enemy movement and existing-shot movement. A newly initialized enemy has
  `shot_delay = 0`; eligible enemies decrement it before the between-segments
  check. Thus lane changing advances the timer but blocks emission, and an
  expired delay remains ready until the transition ends. Successful shots
  alone reload the per-level holdoff (80/77/74/71/68 ticks on displayed Levels
  1-5). Medium/default active-shot capacities are 2/2/2/3/4. The chance table
  accepts a random byte greater than or equal to thresholds 00/E0/F0/FA/FF for
  0/1/2/3/4 active shots. These source facts are anchored at `enm_shoot`
  `$a2a5-$a303`, the normal gameplay call sequence `$9717-$9723`, and enemy
  initialization `$9977-$9979` in
  `Reference/arcade/Tempest Rom Source code.md`.
- **PROJECT DECISION (C# approximation)** — Natural production Levels 1-5
  use a separate `TempestGame` Flipper-fire phase after movement. Flipper
  movement does not create bullets. `Height <= 343` approximates the ROM
  `enemy_along >= $30` cutoff by mapping arcade `$f0/$10` to C# entry/rim
  heights 0/400; exact coordinate equivalence is unavailable. Successful
  shots reuse `EnemyBullet`, its established travel/collision/scoring/rendering
  behavior, and emit `AudioEvent.EnemyBullet` once.
- **PROJECT DECISION (Tanker boundary)** — Existing C# Tanker firing is
  unchanged. Tanker-created bullets share the global active-shot count used
  by Flipper probability and capacity. Exact arcade Tanker firing integration
  remains deferred.
- **REFERENCE VERIFIED (Incitti divergence)** — The former C#/Incitti-derived
  movement-state firing countdown advanced only in selected Flipper states,
  starving below-rim Level 2 lane changes and allowing a silent level. This
  differs from the ROM's separate global firing phase and Neal's observations;
  finite Flipper ammunition and that countdown are no longer the production
  firing policy.
- **REFERENCE VERIFIED** — Incitti's Spiker (“Spinner” in the source) can
  fire a bullet after a growth-driven retreat decision; its separate firing
  roll is strictly greater than 70. This optional behavior is also not
  arcade-verified.
- **PROJECT DECISION** — Natural progression enables the ROM-derived Flipper
  firing policy on Levels 1-5. `--enemy-bullets-test` remains a development
  seam for isolated scenarios that support it; it does not arm Flippers in
  isolated Tanker or Spiker tests. Bullet hits do not count as enemy kills.
  The recreation clears bullets on life reconstruction and when exit begins.
- **DEVELOPMENT ONLY** — Spiker retreat bullets require the Spiker and enemy
  bullet test options together in the isolated Spiker wave. Isolated
  `--tanker-test` bullets require the enemy-bullet option; production-shaped
  Tanker bullets in direct `--level3-test` and the Level 4 candidate are
  visible without it. Neither development route establishes unknown arcade
  firing probabilities or timers.

## 5. Spikers and spikes

- **REFERENCE VERIFIED** — The reference calls the Spiker a “Spinner.” It
  builds a persistent spike in its lane, then retreats. A retreat returns a
  pending enemy unit without awarding a kill or points. Shooting a Spiker
  awards 50 points. Shooting a spike shortens it by 10 and awards 1 point;
  spikes persist after their builder leaves.
- **REFERENCE VERIFIED** — Incitti's `Egg.PickEnemy` suppresses Spikers when
  `enemiesleft + egg_list.Count() < 4` or `numspinners > 3`; equality at four
  still permits them. The active count includes active enemy units (including
  materialized Tanker cargo and enemies awaiting delayed Super Zapper
  resolution), but not bullets or Spikes. `egg_list.Count()` includes roaming,
  approaching, and hatching Eggs, including the Egg currently hatching.
  Unreleased `eggsleft` reserve is separate and excluded. Retreat removes the
  Spiker from `enemiesleft` and returns one unit to `eggsleft`; the later Egg
  hatch invokes the general picker again.
- **ARCADE VERIFIED (visible behavior only)** — Neal observed that endless
  final-Spiker recycling does not match his Atari recollection. This does not
  verify the hidden Egg/identity mechanism by which Atari prevents it.
- **REFERENCE VERIFIED** — Player-shot updates continue through spike
  avoidance, exit zoom, and reverse zoom, except the vortex freeze; the
  reference does not clear in-flight shots when avoidance begins. `Player.Update`
  remains eligible to fire in AvoidSpikes and StartZoom as well as ExitZoom.
  Neal's direct observation above overrides this for new firing during the
  configured Level 4 AvoidSpikes/ExitDelay interval. During zoom/reverse zoom,
  the reference updates only Spike hazards as baddies. A player shot in the
  Spike's lane hits when its depth is below the Spike tip, subtracts 10 height,
  scores 1, and removes the Spike at zero or below. Spike/player collision
  remains lethal in the same lane when the Spike extends beyond the Blaster.
- **REFERENCE VERIFIED** — Spiker speed selection in the source uses a
  fractional/random expression whose precise arcade-equivalent distribution
  is not established.
- **DEVELOPMENT ONLY** — The C# Spiker test wave starts height-60 spikes on
  every lane, uses a fixed Spiker speed of 3, and permits Spikers alongside
  Flippers on the existing Level 1 tube. Spikes do not count as enemies and
  do not block this scenario's wave-clear test. These choices do not describe
  authentic Level 1 or later-level composition.
- **REFERENCE VERIFIED (D1 source findings)** — The Level 4 production
  configuration uses the source binoculars board, 17-unit original supply,
  Flipper/Tanker/Spiker eligibility, and one starting height-60 Spike on each
  lane. Incitti identifies Spikers as enemies; persistent Spikes are separate
  hazards and do not count as enemy supply.
- **PROJECT DECISION (D2)** — Natural progression and the direct diagnostic
  share the accepted Level 4 configuration and gameplay.
- **PROJECT DECISION (D1 correction)** — C# retreat preserves the same
  unresolved original ID and clears its selected runtime type so the returned
  Egg re-enters the source-backed general picker. Spike-exit shots resolve
  against persistent Spikes only; hits do not alter enemy or identity
  accounting. Automated tests cover the strict selection boundary, retreat
  and re-hatch, exact Spike damage/scoring, depletion, exit collision
  prevention, and the warning's configured visibility boundary. Neal manually
  accepted the late-wave Spiker behavior. Post-death Spike shooting after
  reconstruction and long-Spike collision during sustained exit fire are
  manually accepted visible behavior; the four-update cooldown and swept
  collision remain C# project decisions. This does not verify the hidden Atari
  identity mechanism.
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
- **REFERENCE VERIFIED** — Incitti's Pulsar stores one `dir` field and
  recalculates it with `FindShortDir` on each climbing and descending update.
  Lane-pause transfer uses that value; open tubes reverse it at an endpoint.
  The source has no separate persistent rim-chase field.
- **PROJECT DECISION** — The arcade-verified rim-pursuit persistence rule also
  applies to development-only Pulsars while they continue rim pursuit. C#
  latches direction when a rim lane destination is first selected and clears
  it when the Pulsar chooses to descend. The source's repeated shortest-route
  calculation differs; pulse timing, pulse hazards, and directed geometry are
  unchanged. Pulsars remain development-only before natural Level 17.
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
- **ROM-LISTING-BACKED / PROJECT TRANSLATION** — Normal Blaster hits and enemy
  projectile hits use the shared type-0 effect allocator at `$a3d4/$a3d6`.
  The eight records capture lane/depth, advance age by 3 to the threshold 10,
  and replace the greatest-age entry when full (equal-age ties select the
  lowest index). `draw_explosions` `$b79a` combines `(age >> 1) & $fe` with
  type-0 table offset `$00`, selecting graphic-table entries 0, 0, 1, 2 at
  ages 0, 3, 6, 9. These are the single-color-index-0 size-1, size-2, and
  size-3 vector programs at `$14c2`, `$14f8`, and `$1522`. The separate
  `$1b18`…`$1b04` hit-by-shot sequence and `$1b1a` color changes are not the
  common type-0 path. C# maps ROM color index 0 to existing `Color.White` as a
  narrowly scoped project translation; it does not claim an Atari RGB match.
- **PROJECT TRANSLATION** — C# snapshots the
  final lane/depth after confirmed Blaster hits on Flippers (including
  materialized Tanker cargo), Tankers, Spiker bodies, Pulsars, and enemy
  projectiles. Spike erosion, Fuseball/Fuzzball hits, Super Zapper, and player
  death remain outside this type-0 effect. The effect is visual-only: it does
  not enter enemy/projectile collections, collision, score/audio, ledgers, or
  completion checks. C# advances existing effects before shot collision so a
  new age-zero record is drawable; the ROM's ordinary update sequence advances
  a newly created record before drawing. Ages 0 and 3 select the same size-1
  picture, so this preserves the initial visible state with a one-tick
  project-boundary translation. Completion timing is unchanged.
- **NEAL MANUAL ARCADE-FIDELITY ACCEPTANCE (2026-11-08)** — Neal reports that
  the common type-0 explosion looks identical to the arcade game, with correct
  geometry/drawing and perfect animation timing. He paused recorded gameplay
  and used forward/reverse frame stepping to inspect the individual phases.
  This accepts the resulting C# appearance and timing to Neal's satisfaction;
  it does not turn the C# color mapping or perspective conventions into ROM
  facts. Treat the effect as closed and frozen unless a future regression or
  new arcade/ROM evidence justifies revisiting it.
- **DEFERRED** — Fuseball/Fuzzball special destruction and ROM effect types
  2-4 remain a separate investigation and implementation milestone.
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
| `--level N` | **DEVELOPMENT ONLY:** fresh start at the real production level for a currently implemented displayed level (1–7). Uses normal production initialization and onward campaign progression; it is not a terminal diagnostic. |
| `--level2-test` | Direct access to the shared closed 16-lane square, supply-12 Flipper-only canflip configuration; terminal development replay, no progression selection. |
| `--level3-geometry-test` | Direct access to the Flipper-only Level 3 plus-board geometry diagnostic; separate from the mixed production-shaped wave. |
| `--level3-test` | Direct graphical access to the existing production-shaped mixed Level 3 configuration; terminal replay without enabling natural progression. Production Tanker bullets are visible without `--enemy-bullets-test`; the natural progression Flipper firing phase is not enabled by this diagnostic alone. |
| `--level4-test` | Direct access to the isolated Level 4 production candidate: binoculars board, 17 original obligations, randomized Flipper/Tanker/Spiker eligibility, persistent starting Spikes, and spike-avoid exit with its configured Avoid Spikes warning; terminal replay without natural progression. Visible behavior manually accepted; no claim of general arcade fidelity. |
| `--level6-test` | Direct access to the production Level 6 triangle, 22-unit Flipper/Tanker/Spiker wave, starting Spikes, Level 6 Avoid Spikes timing, and production Flipper firing; terminal replay without natural progression. DEVELOPMENT ONLY; Level 6 C# gameplay manually accepted, not a claim of general arcade fidelity. |
| `--level7-test` | Direct access to the production Level 7 clover, ROM-backed 20-original Flipper/Tanker/Spiker wave, starting Spikes, `2-3-2-2-2` ordinary and programmed deep-cargo Flipper routes, production firing, and Level 7 Avoid Spikes timing; isolated terminal replay, composes with `--lives`. DEVELOPMENT ONLY; Level 7 gameplay manually accepted, not a claim of general arcade fidelity. |
| `--spiker-test` | Replaces the normal wave with a Spiker/Flipper test wave and height-60 spikes on every lane. |
| `--tanker-test` | Replaces the normal wave with one controlled Tanker plus ordinary Flipper slots and cargo. |
| `--fuseball-test` | Direct-hatches the ten-unit supply as Fuseballs, with source-derived late-wave hurry-up. |
| `--pulsar-test` | Direct-hatches the ten-unit supply as Pulsars, with source-derived late-wave hurry-up. |
| `--enemy-bullets-test` | Enables the development enemy-bullet behavior; combinations with Spiker or Tanker scenarios enable their corresponding source-backed firing paths. |
| `--lives <n>` | Overrides the starting life count for longer development sessions. |
| `--speed <multiplier>` | **DEVELOPMENT / DIAGNOSTIC PROJECT FEATURE:** changes only interactive host-loop pacing from the accepted default 60 FPS; supports 0.25–4.0 inclusive, with target FPS rounded to nearest integer and exact halves away from zero. Per-tick simulation remains unchanged. |
| `--spinner-sensitivity <multiplier>` | **DEVELOPMENT / DIAGNOSTIC / MANUAL-CALIBRATION PROJECT FEATURE:** scales only live horizontal mouse/spinner delta; default 1.0, range 0.1–4.0 inclusive. Recorded input is scaled before recording; playback and input-diagnostic modes reject this option. |
| `--record-gameplay <file>` | Records effective per-tick input for ordinary production gameplay; new files use strict format version 2 and can include `--level`, `--lives`, `--rim-speed`, and live `--spinner-sensitivity`. |
| `--playback-file <file>` | Restores production startup identity and seed from a version-1 or version-2 recording; rejects simulation overrides. |

`--level N` exists to support the one-level-at-a-time manual arcade testing
workflow. It selects only the starting level; after a successful clear, normal
campaign progression continues. The current supported range deliberately ends
at production Level 7; no placeholder support for later levels is implied.
Neal manually accepted this development-only startup on 2026-11-06. The
controller exit convenience is also manually accepted: V has no production
action, Enter/Return exits immediately without confirmation, and Escape/window
close remain available; other controller mappings are unchanged. These are
development controls, not arcade gameplay behavior.

**Observed / unresolved / non-blocking / deferred (2026-11-06):** During
manual testing, the game unexpectedly exited on multiple occasions and the
console ended with `INFO: Window closed successfully`. The initiating event is
unknown; the shutdown message does not establish a cause. A possible controller
interaction is only a hypothesis. Neal noted ordinary Alt, Control, and Shift
events from other controller buttons and wondered whether a combination could
be relevant, but that is not established evidence. This observation was not
investigated, and no logging, mapping changes, or input diagnostics were added
for it. No level or trigger was isolated in this earlier entry; whether it is
related to the separate Level 5 post-death report below is unknown.

The Level 2 and enemy-specific wave options are mutually exclusive. Deterministic random
seams, controlled selection, artificial wave composition, extra lives, and
test-only rendering/collision instrumentation are all **DEVELOPMENT ONLY**.
Passing a simulation test establishes a C# invariant, not arcade fidelity.

`--speed` requests `60 * multiplier` interactive loop iterations per second.
The default and explicit `--speed 1` both preserve the existing 60-FPS target.
The loop remains one simulation step and one draw per ordinary pass, without
catch-up or elapsed-time scaling. Consequently, every tick-driven gameplay
event changes together in wall-clock time while its per-tick amount remains
unchanged. Input sampling and rendering follow the selected loop cadence; a
host that cannot sustain the request runs slower. Audio samples retain normal
rate and pitch. Recording and visible playback accept the option, but
recordings remain tick-indexed and do not store speed. Headless playback
rejects `--speed` and remains unthrottled. This is not Atari functionality,
not a difficulty setting, and not evidence for changing the accepted 1.0
rate.

**MANUALLY ACCEPTED BY NEAL:** Live gameplay and existing recorded-gameplay
files work correctly at slower and faster settings, with no problems reported.
Pause, forward/grouped stepping and reverse scrubbing preserve their existing
simulation-boundary semantics across selected speeds. Faster recorded
playback is especially useful for rapidly inspecting several levels, and the
diagnostic is expected to help test harder future levels. The accepted default
remains 1.0. Neal's subjective observation that approximately 0.95 may feel
slightly closer to his arcade machine is minor and does not justify a default
change; it is not evidence establishing arcade timing.
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

### Live spinner/mouse sensitivity diagnostic

`--spinner-sensitivity <multiplier>` is a **DEVELOPMENT / DIAGNOSTIC /
MANUAL-CALIBRATION PROJECT FEATURE**, not Atari ROM behavior, an arcade
difficulty setting, tube geometry, or automatic hardware calibration. It
defaults to `1.0` and accepts finite invariant-culture values from `0.1`
through `4.0`, inclusive. Explicit values are printed once at startup.

The physical USB spinner is observed to provide horizontal mouse movement to
the game. The multiplier is applied only to the live
`Raylib.GetMouseDelta().X` sample before the effective `PlayerInput` reaches
simulation and recording. Ordinary horizontal mouse movement is included;
vertical mouse movement and the wheel are irrelevant. The multiplier changes
magnitude only, preserving clockwise/counter-clockwise movement, closed-tube
wrapping and Level 8 open-endpoint clamping/reversal. Keyboard, controller,
fire and Super Zapper input are unchanged. No clamp, dead zone, smoothing,
filtering or nonlinear response is introduced.

The option is independent of `--speed`, which changes host-loop pacing rather
than mouse-delta magnitude. It does not modify Windows pointer settings or
spinner hardware configuration. Live recordings store the effective scaled
per-tick `MouseDeltaX` without a schema or metadata change. Visible and
headless playback do not rescale recorded values and reject the option;
`--input-diagnostic` also rejects it because that mode does not run gameplay
movement. Historical recordings retain their existing behavior. Manual
physical-spinner calibration is **MANUALLY ACCEPTED BY NEAL**: the feature
works correctly, all manual tests passed, clockwise/counter-clockwise behavior
remains correct, and no blocking gameplay regression was found. Neal manually
tested approximately `0.8` and found it improved control with his physical
spinner. This is a manually observed optional calibration alternative, not a
universal, arcade-verified, ROM-backed, accepted-default, or required hardware
value; the default remains `1.0`. The multiplier does not represent a literal
Atari spinner-count conversion. No further tuning is required for this
milestone.

### Recorded production gameplay files

The original Level 1 record/playback round trip is manually accepted and its
input semantics remain protected: one effective input frame, including
neutral and restart values, is captured for every `RunOneStep`; playback uses
the normal seeded production simulation, preserves tick order, and returns to
live controls exactly when recorded input ends. New recordings can start at an
implemented production Level 1–7 and store the effective starting level,
lives, rim-speed multiplier and initial seed. The level resolves through the
same production catalog used by `--level N`; special test/scenario modes
remain incompatible.

Version 2 retains the `tempest-gameplay-input` identifier and the version-1
seed, duration, frame and input/restart fields. It adds
`StartingDisplayedLevel`, `StartingLives`, and `RimSpeedMultiplier`, and
retains `StartingScenario` with the version-2 value `production`. Strict
unknown-member handling and complete required-field validation remain in
force. Version 1 stays readable with its original schema and means Level 1,
3 lives, rim speed 1.0, and its original seed/frames; version-1 files are not
rewritten. Older builds are not required to read version 2.

The file command grammar is `--record-gameplay <file>` or
`--playback-file <file>`, followed by compatible options. Recording permits
`--level N`, `--lives N`, `--rim-speed N`, `--speed N`,
`--spinner-sensitivity N`, and `--game-over-density N` in any order. Playback
accepts renderer-only `--game-over-density N` and interactive `--speed N`; it rejects `--level`,
`--lives`, `--rim-speed`, `--spinner-sensitivity`, and special scenarios before
opening the game.
Neither renderer density nor interactive speed is part of recording identity.
Defaults are Level 1, 3 lives and rim speed 1.0 plus a generated nonnegative
seed.

The recorded configuration and seed are applied before startup consumes
gameplay randomness. Direct Level 5 uses the accepted production geometry,
starting Spikes, `$53` behavior and ordinary progression to Level 6; the
recording does not create a terminal scenario. The existing campaign
full-game restart remains a fresh Level 1 restart with the effective starting
lives restored and rim speed retained. Save behavior is unchanged: normal
window close only, temporary sibling file, non-overwriting move. Generated
files belong under ignored `Temp/` and are disposable unless deliberately
preserved.

**RECORD / PLAYBACK VERSION 2: MANUALLY ACCEPTED.** Neal tested a direct
production Level 5 recording and separate playback. He verified Level 5
startup, 10 lives, Blaster movement/firing, Super Zapper use, deaths if any
occurred, Level 5 enemy/Spike behavior, correct Level 6 progression if reached
in the recording, and normal live-input handoff when recorded input ended. He
reported no problems. This is manual acceptance of the C# behavior, not new
arcade evidence. Malformed-file cases and every automated edge case were not
manually verified.

Playback speed, backward stepping, rewind, automatic looping, playback
start/end frame, true seeking, snapshots, deterministic state signatures, a
dedicated headless playback runner, playlist/browser UI, and attract-mode
integration remain deferred.

**PHASE 1 AUTOMATED ACCEPTANCE PASSED. VISIBLE RECORDED-PLAYBACK CONTROLS
PHASE 2: MANUALLY ACCEPTED.** The controls are project debugger decisions,
not arcade behavior evidence.

The Raylib-independent `GameplayPlaybackSession` owns validated startup,
production simulation and forward replay execution. Boundary 0 is the restored
startup before frame 0; consuming frame N calls `TempestGame.RunOneStep` once
and yields boundary N+1. `RunToBoundary(N)` advances via those same steps,
rejects invalid or earlier targets, and stops at the requested boundary or
recording duration. The immutable bounded `GameplayStateSummary` includes
playback/startup metadata, selected current game state and stably ordered
Flipper identity, movement and pivot values. The session defaults to the
existing silent audio adapter.

Visible `--playback-file` now uses `GameplayPlaybackSession` as the
authoritative recording cursor, boundary, frame source, input-gating,
production-step and completion path. The compatible built-in Level 1 profile
also uses the session; the built-in Level 4 profile retains its terminal
diagnostic startup because that startup contract is not represented by the
production playback session.

Neal manually accepted the initial visible playback and the repeat/group
navigation refinement. Space toggles pause/resume on a press edge. While
paused, each Right Arrow tap advances exactly one recorded tick; Neal considers
one tick per tap ideal. Holding Right repeats that same +1 operation on
Raylib's native `IsKeyPressedRepeat` events, and releasing Right stops
stepping. He accepts the native repeat cadence without retuning: its incidental
near-real-time feel is not a playback-speed requirement. This combination
supports scrubbing quickly through uninteresting portions, releasing near an
event, and then inspecting it tick by tick.

Shift+Right advances five sequential ordinary ticks, and each repeated Right
event while Shift is held starts another five-tick group. At recording end,
groups stop cleanly at the final boundary without consuming a nonexistent
frame. While running, Right events add no steps; ordinary playback still
advances one recorded frame per loop iteration.

**REVERSE RECORDED-PLAYBACK SCRUBBING PHASE 3B: MANUALLY ACCEPTED.** Neal
tested the real 12,761-tick Levels 1–7 recording and reports, "This is
perfect. It does everything I need it to do." The complete editing-style
workflow is hold Left/Right to scrub, release near an event, tap an arrow for
one-tick inspection, and hold Shift with an arrow for faster five-tick groups;
Space remains pause/resume. Native keyboard-repeat timing is unchanged.

While paused, Left tap/repeat reconstructs -1 boundary and Shift+Left
tap/repeat reconstructs -5 ticks, clamped at zero. Right/Shift+Right perform
their established +1/+5 sequential forward steps. The protected round trips
are N -> Left -> N-1 -> Right -> N and N -> Shift+Left -> N-5 ->
Shift+Right -> N. The final boundary remains rewindable while the paused
playback debugger owns the recording.

Reconstruction restores recorded startup in a fresh session, silently replays
ordinary frames from boundary zero through the exact target, and publishes
that session only after success. It does not reverse game operations.
Intermediate ticks are not rendered, the existing window stays open, and the
paused overlay shows the reconstructed boundary. Historical audio is
suppressed; the visible audio sink is restored for subsequent forward steps,
including after repeated reconstruction. Debugger controls are not
`PlayerInput`, are not serialized, and do not change the Blaster or recording.
Live-input handoff remains one-way: after resume, arrows control movement and
Space activates Super Zapper; unrecorded live play cannot be rewound.

Neal confirms reverse scrubbing remains responsive late in the real recording.
Snapshots are not currently needed; replay-from-start remains the accepted
design, with no benchmark claim. Recording v1/v2, metadata, frame format,
strict validation and read-only semantics are unchanged, as are Phase 3A's
`--headless-playback`, `--boundary` and `--json` semantics. This is accepted
C# debugger behavior, not new arcade behavior evidence.

### Paused playback screenshot capture — manually accepted (2026-11-07)

**PROJECT DECISION / MANUALLY ACCEPTED C# DEVELOPMENT TOOLING (2026-11-07).**
Neal confirmed screenshots save correctly under
`C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\`, Ctrl+Shift+S
captures only during paused visible file playback, and pause/resume plus
forward and reverse stepping work as designed. He manually inspected the
saved screenshots and confirmed valid gameplay captures at the selected
playback boundaries. This is final acceptance of the screenshot/playback
milestone, not ROM-backed behavior or new arcade fidelity evidence.

The implementation captures the rendered game window after drawing the PAUSED
boundary/duration overlay and before `EndDrawing`. A bounded diagnostic
during an earlier function-key attempt observed exactly one logical F12,
zero F11, no project request, and a new native `screenshot000.png`; that
function-key assumption was discarded. The pre-correction Ctrl+Shift+S
diagnostic observed Control and Shift, one S press edge, F12 false, a project request, and
no generic native screenshot, but no project file was created because Raylib
failed on the absolute Windows destination. Neal's final acceptance above
supersedes the earlier corrected-path manual-retest gate.

The installed Raylib-cs 8.1.0 package uses native Raylib 6.0. Its native
`rcore.c` checks `KEY_F12` and writes `screenshot%03i.png` in the working
directory. The binding exposes `TakeScreenshot(string)`, but no screenshot-key
setter or runtime disable setting; `SetConfigFlags` does not control this
shortcut. It cannot be disabled through the installed supported API without
rebuilding/replacing Raylib; no native patch, Windows hook or low-level
interception is introduced. The project command is now Ctrl+Shift+S: either
Control key, either Shift key, and an S press edge. The project has no
conflicting S or Ctrl+Shift+S command. Ctrl normally fires during live play,
but paused file playback does not read `PlayerInput`. Function keys are not
used by Tempest for screenshots; F12 remains native Raylib behavior and is
not a project command.

The opt-in `--screenshot-key-diagnostic` samples Control/Shift held state and
S/F12 press edges for one 18-frame window after the first S or F12 edge during
paused visible file playback. It reports the chord observations, project
request, Raylib-relative path, expected full destination, file existence/length,
and new or overwritten generic screenshots since diagnostic start. It reports
once and does not log unrelated keys.

The current development root is the process working directory, distinct from
`AppContext.BaseDirectory` (the application binary root). From the repository
root, project PNGs go beside `Temp\` to `UserData\Screenshots\`, using
displayed level, exact boundary and a numeric suffix to avoid overwrite. The
first accepted capture creates the directory; startup does not. .NET reserves
and verifies the full destination, while Raylib receives a forward-slash
relative path such as
`UserData/Screenshots/playback-level-2-boundary-000988.png`. The absolute
Windows destination previously passed to Raylib was mangled by Raylib 6,
which prefixed the current working directory. Failure output includes both
the relative path and expected full path. The root-level `UserData/` directory
is ignored. Packaging can later provide an application data root appropriate
to a standalone build; packaged-root detection is out of scope.

Bare recording filenames resolve consistently under development-root
`UserData\GameplayRecordings\` for `--record-gameplay`, `--playback-file` and
`--headless-playback`. Explicit relative paths such as
`Temp\GameplayRecordings\levels1-7.json` retain their process-working-directory
meaning, and absolute paths remain unchanged. `Temp\GameplayRecordings\`
continues to be the ignored disposable development location; existing
recordings are not moved or rewritten. No recording format, startup metadata,
frame contents, strict validation, gameplay or playback semantics changed.
Taking a screenshot does not create the separate GameplayRecordings directory.

Ctrl+Shift+S capture is limited to paused visible file playback and remains
separate from `PlayerInput`. It does not advance simulation, boundary, cursor or
RNG; running playback, live gameplay and headless playback do not capture.
Capture failures are reported without resuming or advancing playback. The
pre-correction diagnostic established the chord request and absence of native
F12 capture. The corrected relative-path handoff is now manually accepted for
valid saved PNGs at the selected boundaries. Unique repeat suffixes remain
covered by automated tests.

The enemy-destruction visual remains deferred: the C# Blaster-hit enemy
disappears immediately, while Neal observes a brief small arcade explosion.

Pause keeps the window responsive and redraws the current game state while
simulation, recording cursor, tick-based presentation and simulation audio
events remain frozen; already-playing voices may finish naturally. Each
executed step may emit its ordinary audio events once. The uppercase
`PAUSED  boundary / duration` status uses the shared vector font and shows the
resulting boundary after one- or five-tick navigation.

Completion is reported once. If the final recorded frame is stepped while
paused, the final boundary is rendered and remains paused, no nonexistent
frame or live input is consumed, and forward navigation is inert. Neal
accepted the `PAUSED  boundary / duration` overlay, final-boundary behavior,
audio policy and live-input restoration. Space resumes to the existing
live-input path on the following outer iteration; ordinary Space/Super Zapper
and Left/Right movement mappings then return. No recording format, simulation
or Phase 1 headless/Raylib-independent contract changed. Phase 3B remains
replay-from-start backward reconstruction and performance measurement.
Rewind, speed control, seeking and snapshots are not part of Phase 2.

### Headless recorded-playback analysis — Phase 3A manually accepted

**HEADLESS RECORDED-PLAYBACK RUNNER PHASE 3A: MANUALLY ACCEPTED.** Neal
tested the CLI with real recordings stored under ignored
`Temp\GameplayRecordings\`; these files remain disposable and were not
committed. For `testlevel5-3.json`, the boundary-4820 run reached exactly
4820/7260, leaving 2,440 frames with `RequestedBoundaryReached`. Completion
reached 7,260/7,260 with zero remaining frames and `RecordingCompleted`;
the final state was Level 7 Complete, score 21,805 and 8 lives. Boundary
999,999 was rejected as outside 0 through 7,260 with exit code 5.

The `level1.json` recording completed at 797/797 in Level 2 Approach with
score 1,725 and 20 lives. The `levels1-7.json` recording completed at
12,761/12,761 in Level 7 Complete with score 39,545 and 15 lives. Neal
observed no perceptible delay processing the 12,761 recorded ticks in
ordinary use. No formal timing instrumentation or benchmark was used or is
needed for this milestone.

`HeadlessGameplayPlaybackRunner` accepts a validated recording or recording
path and delegates to `GameplayPlaybackSession`, the same production session
used by visible file playback. It does not create another replay loop.
Boundary 0 is restored startup before frame 0; consuming each frame advances
one boundary. `RunToBoundary(N)` stops exactly at N and rejects targets below
zero, beyond duration, or earlier than the current runner boundary. Running
without a target consumes all recorded frames, even if the game enters Game
Over, Level Complete, death or another non-Playing state. Headless playback
has no live input and no live-input handoff.

The development CLI is dispatched before Raylib window, audio-device, input,
renderer or frame-pacing setup. It uses `SilentAudioEvents`, runs sequentially
without real-time pacing, and leaves the input recording read-only. Its
explicit camel-cased JSON schema contains requested/achieved boundary,
duration, remaining frames, completion reason, selected copied game-state
values and stably ordered Flipper diagnostics. Human-readable output is also
available. Exit codes distinguish success (0), invalid command (2), invalid
recording (3), simulation exception (4) and out-of-range boundary (5); a
simulation exception is printed with its full exception details. The optional
in-process observer receives boundary 0 and each reached boundary with an
immutable `GameplayStateSummary`; it cannot access the runner's mutable game.
There is no assertion language.

Example commands:

```powershell
dotnet run --project src\Tempest.Game -- --headless-playback <recording-file>
dotnet run --project src\Tempest.Game -- --headless-playback <recording-file> --boundary 4820 --json
```

For Copilot-assisted inspection, invoke separate fresh runs at N-1, N and
N+1, compare their structured summaries, then write a focused typed regression
for a confirmed behavioral change. Recording v1 remains Level 1, three lives
and rim speed 1.0; v2 retains its explicit startup metadata. Neither recording
schema nor accepted Space pause/resume, Right +1 or Shift+Right +5 changed.
Headless playback is read-only. The accepted CLI
provides VS Code Copilot a project-owned way to inspect exact boundaries and
bounded state from the same recordings Neal can scrub visually; nearby fresh
runs can inform focused regression tests. Phase 3B backward visual scrubbing
is manually accepted using deterministic reconstruction from boundary 0;
Neal confirmed responsiveness on the real 12,761-tick recording, so snapshots
are not currently needed. Paused-playback screenshot capture and its corrected
UserData path are manually accepted as of 2026-11-07, including pause/resume
and forward/reverse stepping. The existing immediate disappearance of a
Blaster-hit enemy, compared with the arcade's brief small explosion, remains a separate deferred
visual-fidelity issue. This is acceptance of C# development tooling, not new
arcade behavior evidence.

**NEAL-OBSERVED / DEFERRED VISUAL-FIDELITY GAP:** In the C# game, an enemy hit
by a Blaster shot currently disappears immediately. Neal observes that the
original arcade briefly replaces a destroyed enemy with a small explosion
animation. This separate presentation issue is deferred; it was not
implemented as part of Phase 2.

### Level 5 post-death invalid-direction fix (manually accepted)

**NEAL-OBSERVED / PRODUCTION-LIFECYCLE-REPRODUCED / C# ROOT CAUSE PROVEN /
FIX IMPLEMENTED / MANUALLY ACCEPTED.** The original unhandled
`ArgumentOutOfRangeException("direction")` passed from `Flipper.BeginPivot()`
to `Level.CalculateDirectedLaneTransition()`. The production-realistic
regression established this sufficient lifecycle: a Level 5 Tanker released
two cargo Flippers; one remained unresolved when its collision killed the
Blaster; death/re-entry cleared runtime entities but retained cargo identity;
that identity returned through a fresh Egg; and `CreateFreshTankerCargo`
reconstructed a Flipper whose default `Direction == 0` later reached a
Spike-gated below-rim pivot. The geometry guard correctly rejected the invalid
direction.

The `$53` Spike-sensitive policy exposed the missing initialization invariant;
it was not shown to be incorrect. The accepted correction gives reconstructed
Level 5 cargo a valid reference-shortest direction toward the player and
validates the direction domain at `BeginPivot` before constructing a directed
transition. The strict geometry guard remains unchanged. The `$53` gate,
threshold, climbing behavior and timing are unchanged; existing Level 7
programmed-direction restoration remains unchanged. This is a C# lifecycle
defect and correction, not new arcade evidence.

Neal extensively tested production Level 5, repeatedly exercised the former
failure sequence, and verified stable death/re-entry, normal gameplay and
Level 5-to-6 progression without an obvious Flipper, Spike, Tanker or cargo
regression. All other currently implemented production levels were manually
tested without an unexpected exit or obvious neighboring-level regression.
Several recordings and playbacks were retested. One supplied Level 5 playback
completed after 7,260 simulation ticks, reported live controls resumed, and
later exited with `Exit reason: WindowShouldClose`; no exception occurred and
Raylib cleanup completed normally. The normal-exit diagnostic remains
nonintrusive, and gameplay exceptions remain uncaught and visible. Neal
accepted the correction; manual testing does not claim to isolate every
automated cargo identity or direction edge case. Automated validation and
remaining limitations are recorded in
`AI-Logs/Reports/2026-11-06-level5-post-death-invalid-direction-acceptance-closeout-report.md`.

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
- **PROJECT DECISION** — The former temporary terminal after Level 6 was
  superseded by the accepted Level 7 production boundary. Authentic
  post-Level-7 composition, open-tube rendering, bonus rules, broader
  enemy/cargo combinations, and source-specific death effects remain out of
  scope or deferred. Natural Level 1-to-Level 2-to-Level 3 progression has
  passed manual acceptance of visible C# behavior. Neal also manually accepted
  the natural Level 3 -> Level 4 handoff and Level 4 campaign, including the
  corrected firing boundary. Neal has now manually accepted natural Level
  4 -> 5, Level 5 startup/gameplay, and completion into the existing terminal
  boundary. Together with the prior acceptance reconciliation, the implemented
  Level 1-5 production campaign completed manual acceptance in the recreation.
  Neal also played Level 6 multiple times and accepted it with no gameplay
  issues observed. Levels 1-6 are the manually accepted production foundation;
  the later Level 7 implementation extends natural progression, with Neal's
  Level 7 manual acceptance was pending at that earlier checkpoint and was
  completed later as recorded above. This does not establish hidden
  implementation details or general arcade fidelity; remaining uncertainties
  are recorded above. Audio fidelity remains a long-term roadmap item; the
  accepted C# audio system is documented separately.
- **DEFERRED / NON-BLOCKING (sound-effect volume balance; 2026-11-04)** —
  Neal identified the enemy-destruction/explosion effect triggered when a
  Blaster shot destroys an enemy as significantly too loud relative to other
  effects across the current game, not specifically Level 6. Broader
  sound-effect balance needs future review; no other individual effect is
  established as incorrect. Windows volume adjustment is an acceptable
  temporary workaround. No audio implementation change is authorized during
  this closeout.
- **ARCADE VERIFIED** — Neal observed a brief Atari between-level approach
  after the Blaster exits a tube: a first-person view moves through space
  toward the next tube, which begins small/distant and grows as the player
  approaches; the Blaster is visible on the distant rim and spinner movement
  moves it around that rim during the approach, before combat engagement.
  Supplied actual Atari transition frames establish tiny point stars around
  the distant tube, blue early-level stars, diminishing stars as the tube
  grows, the visible rim Blaster, and exact `SUPERZAPPER RECHARGE` wording.
  Neal directly observes onset around halfway through the current tube's
  exit, disappearance shortly before arrival, blue exits 1-4, and a
  white/red/green/blue mixture from exit 5 onward. His approximately
  four-second first-star-to-arrival target and roughly 30-45-degree elevated
  impression are qualitative observations, not frame/angle measurements.
  These observations do not establish projection formulas, exact thresholds,
  enemy-start timing or exact firing enablement.
- **PROJECT DECISION (implementation; combined presentation manually accepted)** —
  Natural production progression enters an explicit approach state after the
  successful exit commit for Level 1 -> 2, Level 2 -> 3, Level 3 -> 4, and
  Level 4 -> 5. The incoming
  configuration and fresh wave are installed at approach start, but gameplay
  simulation is frozen until arrival. Spinner, keyboard and left-stick (with a project 0.15 normalized dead zone)
  movement remain enabled; fire and Super Zapper are suppressed. Rendering
  applies a transition-only depth offset to the incoming tube and Blaster and
  reaches the exact normal projection at arrival. Accepted implementation
  choices are 288 deterministic point stars with 1.6x clamped RGB gain,
  960 initial depth offset, 96-unit approach-only vertical bias and a
  181-update smoothstep approach. Stars begin after 50% remaining exit depth
  and reach full visibility at 20%; actual first nonzero-alpha submission is
  ExitZoom update 65. The following 59 exit intervals plus 181 approach updates
  give 240 frame intervals, approximately four seconds under the normal
  one-update-per-frame 60-FPS cadence, not wall-clock catch-up timing.
  The starfield remains continuous across the handoff; removal follows
  `1 - smoothstep(progress / 0.85)` and finishes by approach tick 154.
  The existing 192-star-reference zero-alpha gate preserves visibility
  boundaries independently of density. All counts, coordinates, color
  proportions/RGB values, gain, thresholds, ticks, interpolation and text
  styling are PROJECT DECISIONS, not measured Atari internals.
  The exact uppercase recharge message is lower-center during Approach;
  the normal top HUD is preserved. Rendering consumes no gameplay randomness.
  Tests protect state, freeze, movement, deterministic geometry/color policy,
  timing and exact arrival projection, not subjective fidelity. Neal manually
  accepted the combined presentation and requested it be retained. The
  Level 4 -> 5 approach remains blue because colors follow the exited level.
  Neal has now manually accepted the Level 4 -> 5 approach through natural
  production progression: blue stars, the upcoming cross-shaped tube, recharge
  message, spinner-controlled Blaster, and seamless arrival all appeared normal.
  This accepts the recreation's presentation without establishing new arcade
  evidence or measured timing.

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
  configuration's two-charge Super Zapper state; at the C4 checkpoint Level 3
  had no successor. Direct development scenarios remained isolated. Neal
  manually accepted the visible natural progression, Level 3 board/display/HUD,
  fresh mixed wave, two usable Super Zapper charges, gameplay spot-check,
  terminal behavior and restart at that checkpoint. D2 now supersedes that
  terminal policy. This validates visible C# behavior only; neither tests nor
  manual acceptance establish hidden scheduling/internal behavior or new
  arcade evidence.
- **Between-level approach presentation** — Neal reports that Atari shows a
  brief first-person approach through space to the next tube. The distant
  tube grows in view; the Blaster appears on its distant rim and remains
  spinner-movable during approach, then combat begins. This gameplay-relevant
  approach is implemented for natural production transitions. Neal manually
  accepted the visible natural Level 1 -> 2 -> 3 -> 4 campaign approach.
  He observed no ExitZoom handoff discontinuity, projection flash, wrong board,
  Blaster jump or new
  problem, and found it impressive and close to remembered Atari behavior.
  These observations accept the current implementation; they do not establish
  exact arcade timing or projection parameters. Neal subsequently manually
  accepted the refined perspective, distant tube, brighter 288-star field,
  recharge message, spinner movement and smooth arrival, including the
  approximately-four-second journey under normal cadence. The exact constants
  remain PROJECT DECISIONS; the target is acceptable arcade fidelity with
  reasonable compromises, not a frame-perfect clone. Do not continue tuning
  accepted transition details. Incitti remains an engineering reference, not
  behavioral authority.
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

## 13. Level 5 Spike-sensitive Flipper behavior (2026-11-06)

**ROM-BACKED:** The checked-in arcade ROM selects movement program `$53` for
displayed Level 5. `$53` moves the Flipper, then tests the live Spike height of
its current lane. Opcode `$1c` treats zero Spike height as `$ff` and otherwise
allows the flip when `spike_ht >= enemy_along`, including equality. A false
predicate returns to the move-and-test loop; a true predicate starts the normal
flip, with along movement during the turn. The newly occupied lane is tested
after the completed flip and a subsequent along movement. There is no explicit
lock state or random lock/unlock decision.

**ARCADE OBSERVED / MANUALLY ACCEPTED:** Neal manually played production
Level 5 after the `$53` correction and accepted the corrected behavior. Ordinary
Flippers continue climbing below the relevant live Spike tip while the next
below-rim lane change is suppressed; lane changing resumes once the live
threshold permits it. The resulting Spike-lane-following behavior looks and
feels acceptable against Neal's arcade expectation, with no blocking Level 5
gameplay regression observed. This overall acceptance does not claim that Neal
manually isolated every automated shortened-Spike, destroyed-Spike,
Tanker-cargo-depth, or reconstruction edge case.

**CURRENT C# / PROJECT TRANSLATION:** The former Level 5 generic `CanFlip`
behavior did not consult Spike state. The implementation uses a narrow
Level-configuration movement policy. C# `Flipper.Height` grows from 0 at the
far end toward the 400-unit rim, and `Spike.Height` identifies the live tip in
that coordinate model; after each climb step, a below-rim lane change is
permitted when the current-lane Spike is absent/depleted or `Flipper.Height >=
Spike.Height`. This is a C# coordinate translation, not a literal conversion of
ROM bytes. Shortened Spikes therefore release the gate sooner, and live Spike
state is read again rather than latched. Ordinary death/re-entry creates fresh
Flippers using the active Level 5 policy, not an old movement-program phase.

The policy is level-specific: Levels 4 (`$24`), 6 (`$0b`), and 7 (`$24`) do not
inherit the Spike gate; Level 7's manually accepted `2-3-2-2-2` route remains
separate. Natural/near-rim Tanker cargo retains straight-climb behavior
corresponding to ROM `$07`; deep player-shot Level 5 cargo uses the Level 5
policy. The C# `Height <= 370` cargo cutoff reuses the established project
deep-cargo threshold and is an approximation of the ROM `$20` release-depth
selection, not an exact coordinate conversion. Tanker sibling identity and
direction ownership are unchanged.

Automated tests protect below-tip climbing, equality/above-tip release,
destroyed and shortened Spikes, live reevaluation, entry into a Spike lane,
Level 4/6/7 isolation, Tanker release distinctions, re-entry and existing
progression. These tests establish C# behavior only; the listed edge cases are
not represented as individually observed manual checks.

**MANUAL ACCEPTANCE: MANUALLY ACCEPTED.** The accepted gameplay correction
remains Level-5-specific; preserve Levels 4, 6, and 7 as described above.

### Level 5 ordinary Flipper selection correction (2026-11-08)

**ROM-LISTING-BACKED:** The local ROM `min_flippers` table sets Level 5's
minimum to zero, and the ordinary selection paths at `$9a15-$9a86` require a
nonzero minimum. Level 5 therefore has no ordinary bottom-entry Flippers;
its Flipper maximum of five continues to account for active cargo. Tanker
selection and its two Flipper cargo children are separate from that ordinary
Flipper minimum.

**PROJECT TRANSLATION:** Level 5 reuses the C# mixed selector with ordinary
Flipper selection disabled. Cargo Flippers continue through their existing
identity-aware path. On death, an unresolved original Flipper species choice
is cleared and must be selected again under the Level 5 policy; Tanker cargo
is not cleared by this rule.

**EXISTING PROJECT BEHAVIOR PRESERVED:** Production Level 5 keeps its original
supply of 20, identity ledger, four initial Eggs, reserve/release schedule,
Tanker cargo ownership and reconstruction, and completion obligations.
Tanker-released cargo Flippers remain valid. Levels 4, 6, and 7 keep their
independent selection behavior. The ledger, death reconstruction, and cargo
ownership are C# project mechanisms, not claims about literal ROM internals.

**NEAL MANUAL ACCEPTANCE (2026-11-08):** After several production Level 5
playthroughs, Neal accepted this correction. He did not observe any ordinary
Flipper entering directly from the bottom. This is manual acceptance of the
visible Level 5 behavior, not a broader claim of arcade fidelity.

Level 6 remains unchanged and is a separate future evaluation. The broader
observation that C# levels can take substantially longer to clear than the
arcade remains unresolved and is not explained by this correction.

## Production Level 8 ROM-derived behavior — manually accepted (2026-11-08)

**REV-2-BINARY-CONFIRMED / RAW-ROM-DATA:** Displayed Level 8 is internal level
index 7 and selects shape row 7, with `lev_open[7]=$FF`. The ordered shape has
16 vertices and 15 playable lanes, indexed 0-14, with no vertex-15-to-vertex-0
edge. Raw X bytes are
`10 1E 2C 3A 48 56 64 70 90 9E AC BA C8 D6 E4 F0`; raw Y bytes are
`F0 D0 B0 90 70 50 30 10 10 30 50 70 90 B0 D0 F0`. The angle sectors are
`0D 0D 0D 0D 0D 0D 0D 00 03 03 03 03 03 03 03 00`.

**ROM-LISTING-BACKED / REV-2-BINARY-CONFIRMED:** Presentation values include
scale `$18`, Y3D `$B0`, Y2D `$0060`, Flipper scale `$0240`, tube-color index
`$05`, and Level 1-16 color block `00 04 08 0C C3 07 0B 0B`. These do not
specify C# pixels or RGB values.

**PROJECT TRANSLATION / SCREENSHOT-SUPPORTED ORIENTATION:** C# subtracts `$80`
from each raw coordinate and scales by `25/14`, then reflects centered Y
(`yScreen = -yCentered`) for the screen-down projection. This single Y
reflection preserves all 16 vertices, their order, relative distances and
angles, and the 15-lane open topology. Neal's
`UserData/Screenshots/Level8-orientation.png` shows the endpoints upper-left
and upper-right with the short connecting segment at the bottom. The existing
unreflected mapping put the endpoints below that segment. A Y reflection
corrects only that vertical inversion; X reflection leaves the inversion
unchanged, while 180-degree rotation also swaps the source left/right endpoint
identities and is not required by the evidence. Axis swaps and quarter-turns
put an endpoint on a side rather than placing both endpoints across the top.
`CenterY=300`, `YOffset=0`, blue tube color, initial Blaster lane 7, and
starting Spike height approximately 71.428571 remain project choices. The
Spike value translates ROM `$C8` with `(0xF0 - rawAlong) * 400 /
(0xF0 - 0x10)`. Fifteen starting Spikes occupy lanes 0-14; erosion remains
the existing C# position/height approximation.

**ROM-LISTING-BACKED / PROJECT CORRECTION:** Active ExitZoom now advances
normal player firing and shots, resolves shot-versus-Spike erosion, and checks
the exiting Blaster against the remaining Spike in its current lane. Existing
shot capacity, cooldown, sound, and deterministic input recording remain in
use; enemy-wave simulation remains frozen. These active hazards are independent
of the `AvoidSpikes` presentation policy: Level 8 stays on Standard exit and
does not show the earlier AVOID SPIKES interstitial. Spike collision during
exit uses the existing C# depth-height mapping and starts the normal reverse
zoom/life reconstruction path. If the cleared Level 8 wave remains complete
after a life loss, it retries through Standard exit delay rather than
substituting the Avoid Spikes interstitial.

**PROJECT CONTROL MAPPING / MANUALLY ACCEPTED:** The user-facing
rotational invariant is spinner direction, not screen-left/right. Positive
mouse delta represents physical clockwise rotation; negative delta is
counter-clockwise. Levels 1-7 have clockwise increasing lane order. Level 8's
screenshot-supported Y reflection reverses the signed winding while preserving
ROM vertex and lane identities, so increasing lane indices there follow the
rim counter-clockwise. The movement system derives the clockwise lane-index
step from the tube outline and applies it to spinner delta only. Thus Level 8
clockwise moves toward decreasing lane indices (visually left), and
counter-clockwise toward increasing indices (visually right). Open endpoints
still clamp; no lane wraps across the gap. Keyboard and controller inputs keep
their established screen-direction mapping, and enemy/cargo lane semantics
are unchanged. Levels 1-7 retain their existing spinner mapping.

The generic topology model exposes 15 Level 8 lanes, clamps Blaster motion at
both endpoints, prevents enemy/cargo gap wrapping, and omits the false closing
rim edge. The renderer retains 16 depth rails and draws 15 near- and far-rim
edges. Closed Levels 1-7 retain wraparound behavior.

**REV-2-BINARY-CONFIRMED:** Medium/default supply is 24; the ordinary active
gate is six; Flippers are 0/5, Tankers 1/2, Spikers 2/4, Pulsars 0/0, and
Fuzzballs 0/0. Ordinary bottom selection is limited to Tankers and Spikers;
Tankers carry two Flippers without adding generic supply. Active/released
Flippers and two latent Flipper slots per active Tanker reserve capacity
against the Flipper maximum before another Tanker is selectable. The
deterministic post-minimum selector is a **PROJECT TRANSLATION**, not the exact
ROM Spike-height/random-start policy.

ROM-equivalent Tanker along positions below `$20` use straight Flipper entry
`$07`; positions at or above `$20`, including equality, use Level 8 program
`$19`. Natural Tanker release below `$20` uses `$07`; deep player-shot
destruction uses `$19`. The `$19` route repeats one along interval and a
directed flip, without Level 7's 2/3 reversal program or a Spike test.
Movement speed, firing cadence, direction-bit mapping, and Spike erosion are
not claimed as literal ROM timing.

Death reconstructs generic unresolved supply: species/program choices clear,
latent cargo is lost with its Tanker, released unresolved cargo re-enters as
generic units, and resolved outcomes remain resolved. Level 7 naturally
progresses to Level 8 through the existing approach with score/lives carried
and fresh Super Zapper charges. Generic `--level 8` and recording v2 startup
use the production configuration. Level 8 is terminal; no Level 9 is loaded,
and existing restart returns to fresh Level 1.

**EXISTING PROJECT BEHAVIOR:** Recording v2, visible/headless playback,
pause/forward/reverse navigation, screenshot capture, and interactive
`--speed` use the generic startup-level representation; `--speed` is not
serialized.

**MANUALLY ARCADE-FIDELITY ACCEPTED:** Neal's final Level 8 play test accepts
the ROM-derived tube shape, corrected screen orientation, open topology and
endpoint presentation, clockwise/counter-clockwise spinner behavior, endpoint
clamping/reversal, and absence of wrap across the gap. Level 8 gameplay also
passes, including the accepted supply/species/cargo behavior, the shallow
`$07` versus deep `$19` Tanker-cargo route, the repeating
one-along-interval/one-flip `$19` result, and starting-Spike presentation.
Active-exit firing and shot movement, shot-versus-Spike interaction, exit
Spike hazards, and corrected retry behavior pass; no blocking Level 8 gameplay
issue remains and no further tuning is currently required. These observations
accept the resulting project behavior, not literal C# coordinate/color/control
translations as Atari hardware constants.

Neal also manually verified an existing Levels 1-7 recording, playback
pause/resume, forward/reverse stepping, and `--speed`, with no observed
regression. Automated tests cover natural Level 7-to-8 progression; no separate
manual result for that exact transition is asserted here. ROM-derived behavior
and project translations above retain their evidence classifications. Level 8
is the terminal production level; Level 9 is not part of this milestone.
