# Level 6 preimplementation analysis

Date: 2026-11-04. Analysis only; awaiting Neal's review.

## 1. Baseline and authority

- Verified starting HEAD: `dbf00313b609f81ff26860fd0fcdf0bd0f46c3b0`.
- Initial `git status --short --branch`: `## master`, no file entries.
  Both unstaged and cached diff statistics were empty.
- Exactly one complete standalone completion token was found in the substantive
  user task before any tool use.
- Read the project [Copilot instructions](../../.github/copilot-instructions.md),
  relevant [development notes](../../TEMPEST-DEVELOPMENT-NOTES.md),
  [behavior specification](../../TEMPEST-BEHAVIOR-SPEC.md), and
  [README](../../README.md).
- Levels 1-5 are the accepted production foundation. Development notes lines
  1738-1789 and behavior specification lines 981-1038 establish that acceptance.
  Older text remains in these documents: README lines 83-86 still describe
  terminal Level 3, and lines 730-732 say Level 5 manual acceptance is pending.
  These are stale relative to the explicit later acceptance, not grounds to
  reopen the foundation. No authority document was edited.

Evidence labels below distinguish **REFERENCE FACT**, **ACCEPTED C# REUSE**,
**MISSING C#**, and **UNRESOLVED / DECISION**. Source-derived constants are not
new arcade measurements.

## 2. Level 6 reference geometry and palette

**REFERENCE FACT:** The Original Arcade campaign's sixth board is **triangle**,
not the separate custom level set in `levelset6.txt`.

The authoritative data is [levelset1.txt](../../Reference/incitti/levelsets/levelset1.txt),
lines 12-13:

- Internal progression index 5; displayed level 6; board index 5; palette 0.
- Continuous/closed = 1; **16 ordered vertices and 16 playable lanes**.
- Center Y = **360**; Y offset = **-40**.
- Ordered vertices, preserving the source order exactly:

```text
(-116,145), (-171,145), (-137,72), (-102,0),
(-68,-72), (-34,-145), (0,-218), (34,-145),
(68,-72), (102,0), (137,72), (171,145),
(116,145), (54,145), (0,145), (-54,145)
```

[tempest.bmx](../../Reference/incitti/tempest.bmx), lines 4184-4202, installs the
16 points, connects adjacent points, and closes the last lane for a continuous
board. The source level fields at lines 1279-1281 use depth 400; ordinary ready
position is 50 (lines 1316-1321).

Palette 0 is explicit at lines 4008-4027: white bullets, yellow Blaster, magenta
Tankers, red Flippers, cyan Pulsars (not eligible here), green Spikers, blue
tube/information. No palette-cycle capability is needed.

**ACCEPTED C# REUSE:** The closed-polygon endpoint-hinge geometry, nonuniform
lane lengths, collinear vertices, projection offsets, player lane highlight,
and entity rendering already support this data. See
[Level.cs](../../src/Tempest.Game/Levels/Level.cs), lines 443-497 and 609-624;
[VectorRenderer.cs](../../src/Tempest.Game/Rendering/VectorRenderer.cs), lines
481-500. No triangle-specific renderer or new open-tube capability is indicated.

A read-only numeric sanity calculation on the actual source row found 35
values/16 vertices, signed area 61,860, minimum edge length 54, and interior
angles approximately 49.948 to 180.647 degrees. Thus source rounding introduces
slightly reflex vertices: do not replace the coordinates with an ideal triangle
or assume every side subdivision is perfectly collinear. All satisfy the
existing finite/nonzero-ray, 0-to-360-degree interior-angle guards. This was
arithmetic inspection, not execution of a new C# Level 6 test.

## 3. Enemy eligibility, supply, and Egg policy

**REFERENCE FACT:** At `lv = 5`, `current_board = 5`,
[tempest.bmx](../../Reference/incitti/tempest.bmx), lines 4210-4268, gives:

| Item | Exact Level 6 result |
|---|---|
| Original wave supply | `10 + 5/2 + 5*2 = 22` (integer division) |
| Initial active Eggs | 4 |
| Initial reserve | 18 |
| Flippers | Enabled; `canflip = True` |
| Tankers | Enabled |
| Spikers ("Spinner" in source) | Enabled through `hasspikes` |
| Fuseballs | Disabled |
| Pulsars | Disabled |
| Fuseball/Pulsar Tanker cargo | Disabled |
| Tube rotation | Disabled (`lv <= 53`) |
| Release interval | 50 main-loop ticks |
| Reserve batch | Up to six Eggs each release, not one |
| Hatch-rate tier | `5/8 = 0` |

The batch release is at lines 3737-3743. The four initial Eggs are not a
four-active-Egg cap for the entire reference wave.

There are **no fixed per-species counts**. Egg hatches select eligible types
randomly, so 22 is the number of original obligations, not a promise of 22
simultaneously active enemies or a fixed total after cargo materializes.
Spiker retreat recycles an unresolved obligation, and Tankers add two cargo
units under the accepted C# ledger.

Literal source selection at lines 1979-2055:

- Draw `Rand(-6, upper)`, clamp negatives to zero; upper is 4 below six active
  Tankers, otherwise 3.
- If active enemies plus active Eggs is strictly less than four, or active
  Spikers exceeds three, draw from 1 through upper instead.
- Candidate 0 = Spiker, 1 = Flipper, 2 = Pulsar, 3 = Fuseball, 4 = Tanker.
  Disabled types are rejected and selected again.
- Six active Tankers prevents new Tanker selection; four active Spikers
  suppresses additional Spikers. Reserve Eggs do not enter the near-end sum.
- Eggs advance roaming/approach logic when their counter exceeds seven;
  approaching Eggs gain two height units on that cadence. Roaming Eggs can
  start approaching only with fewer than nine active enemy units and fewer
  than two hatching Eggs, on `Rand(0,100) > 95` or the near-end condition
  (lines 2064-2112).

**ACCEPTED C# REUSE:** Composition
`FlippersTankersAndSpikers` selects the production ledger and existing mixed
wave systems independently of displayed level. See
[Level.cs](../../src/Tempest.Game/Levels/Level.cs), lines 54-58, 181-190,
272-341, and [TempestGame.cs](../../src/Tempest.Game/TempestGame.cs), lines
708-795. Production-ledger release already releases up to six every 50 ticks;
the configuration's `MaximumActiveEggs = 4` is not its effective hard cap.
Accepted C# hatch selection is source-shaped, not a literal replay of all
reference Egg update timing.

**UNRESOLVED / DECISION - shared picker discrepancy:** The accepted
[Level3WaveSelection.cs](../../src/Tempest.Game/Levels/Level3WaveSelection.cs),
lines 45-82, accepts candidate **3 or 4 as Tanker**, whereas the literal
Incitti hatch rejects candidate 3 when Fuseballs are disabled. This changes
selection weighting; at the active-Tanker cap it can leave an Egg waiting
when the game refuses a selected Tanker. Existing Level 4 tests explicitly
exercise candidate 3 as Tanker
([Level4CandidateTests.cs](../../tests/Tempest.Game.Tests/Level4CandidateTests.cs),
lines 999-1006). It is existing accepted behavior, not a Level 6 implementation
defect introduced by this task.

Recommendation: explicitly authorize reuse of the accepted picker for the
bounded milestone; do not describe its probabilities as exact Incitti behavior.
If literal picker equivalence is required instead, that is a separate,
explicitly approved shared-behavior change, not a silent Level 1-5 correction.
No exact arcade species distribution has been established.

## 4. Spiker and persistent Spike policy

**REFERENCE FACT:** All 16 lanes start with height-60 Spikes
([tempest.bmx](../../Reference/incitti/tempest.bmx), lines 4245-4248).
Spikes are hazards, not enemy obligations.

Source Spiker speed selection is
`5/48.0 + Rand(2.5,4.5)` (line 2003). Do not claim the fractional-looking
`Rand` call establishes an arcade-equivalent continuous distribution.
Building extends an existing Spike to builder height minus four; after growth
exceeds 100, a random retreat can occur, with an independent retreat-shot
decision. Retreat descends at 90% of build speed, then returns one unit to
reserve without a kill score (lines 3636-3670).

Shots shorten a Spike by 10 and score 1; zero/below removes it; a same-lane
Spike extending beyond the Blaster is lethal during exit. Spikers score 50
when destroyed (lines 3536-3567 and 3673-3684).

**ACCEPTED C# REUSE:** Explicit `StartingSpikeHeight = 60`, existing production
Spiker creation, persistent hazards, strict near-end eligibility, retreat and
reselection, shot targeting after reconstruction, swept exit collision, and
reverse/retry accounting. See [Level.cs](../../src/Tempest.Game/Levels/Level.cs),
lines 82-102 and 124-143;
[Spiker.cs](../../src/Tempest.Game/Gameplay/Spiker.cs), lines 34-88; and
[TempestGame.cs](../../src/Tempest.Game/TempestGame.cs), lines 736-746 and
1796-1855. Current C# speed selection is `5/48f + 2.5 + NextDouble()*2`;
its distribution remains a project approximation. No permanent
`DisplayedLevel >= 4` Spike rule should be introduced.

## 5. Firing and level-dependent movement/timing

**REFERENCE FACT - literal Incitti, evaluated at internal level 5:**

| Parameter | Level 6 value |
|---|---|
| Flipper normal climb | 2 per update |
| Flipper climb on a Spike, source movement branch | 3 per update |
| Flipper turn increment | `8 + 5/5 = 9` degrees |
| Flipper lane pause | `21 - 5/5 = 20` ticks |
| Flipper rim pause | `24 - 5/5 = 23` ticks |
| Tanker climb | `1.75 + 5/20 = 1.75` |
| Spiker speed | Expression/distribution distinction in section 4 |
| Enemy bullet travel | 3 per update |
| Pulse turn / lane pause / rim pause | 10 / 20 / 16; ineligible species |
| Fuseball climb | 3.3; ineligible species |
| Hatch tier / release | 0 / 50 ticks |

Setup expressions: [tempest.bmx](../../Reference/incitti/tempest.bmx), lines
4251-4266. Literal Flipper Spike handling is at lines 2749-2769; bullet travel
is at line 2352. Accepted C# Flipper movement is not literal Spike-aware
Incitti movement; retain the shared accepted state machine rather than adding
a new Level 6-only movement branch.

Literal Incitti Flippers start with 1-3 rounds and timer 16, attempt shooting
from selected movement branches, reset on timer expiry to 16, and fire on
`Rand(0,100) > 75` (lines 2859-2872 and 2920-2933). This is **not** the
accepted production C# firing authority: it previously caused below-rim firing
silence and was superseded by the arcade/ROM-backed independent firing phase.

**REFERENCE FACT - applicable ROM-backed production extension:** Level 6
holdoff is **65**, and its Flipper emission capacity is **3**, not 5 and not
Level 5's 4.

Evidence: [Tempest Rom Source code.md](../../Reference/arcade/Tempest%20Rom%20Source%20code.md),
lines 2695-2706, 2864-2867, 2936-2949, and 4546-4588.
The holdoff table is opcode 08, levels 1-20, start `$50`, increment `$FD`:
`80 + (6-1)*(-3) = 65`. The Level 6 `enm_shotmax` entry is `$02`;
the firing routine searches slots 2, 1, 0, yielding three slots. This is the
same default/medium convention used by accepted Levels 1-5, not a new
difficulty selection. Do not copy the monotonically increasing early capacities.

**ACCEPTED C# REUSE:** Keep ready-on-spawn/reconstruction, timer advancement
during pivots, blocked emission during a pivot, successful-shot-only reload,
height cutoff 343, inclusive pressure thresholds `00/E0/F0/FA/FF`, shared
bullet collection/audio, and existing Tanker/Spiker policies. See
[TempestGame.cs](../../src/Tempest.Game/TempestGame.cs), lines 1284-1320.
The height conversion remains a project approximation, not exact ROM geometry.

Tankers retain 1-2 rounds, initial delay `16 + Rand(64)` (C# offset 1-64),
countdown/reset 16, successful roll above 75, post-climb firing below height
380, and live cargo release above height 385. Each carries exactly two
Flippers, with the accepted distinct non-carrier-lane release/pivot routing,
not a return to the conflicting literal Incitti route. No new cargo kind is
needed. See [Tanker.cs](../../src/Tempest.Game/Gameplay/Tanker.cs), lines 44-127;
[TempestGame.cs](../../src/Tempest.Game/TempestGame.cs), lines 748-759.

Spikers keep their independent retreat-shot rule. The accepted Flipper
capacity is not a universal emission cap on Tankers/Spikers; their bullets
contribute to the pressure seen by Flippers. Do not redesign that integration.

**MISSING C#:** Both firing property switches currently throw for displayed
Level 6, even if a configuration is manually constructed. Add explicit 6 =>
65 and 6 => 3 cases. The existing `FlipperConfiguration` already represents
the Level 6 movement tuple `(true, 2, 9, 20, 23)` without new mechanics.

## 6. Super Zapper and fresh start

**REFERENCE FACT:** Initial selection and each successful next-level setup
assign two Super Zapper charges and player edge 7:
[tempest.bmx](../../Reference/incitti/tempest.bmx), lines 1730-1745 and
1426-1434. The source resets `rimit = False` during enemy setup. There is no
Level 6-specific extra charge or recharge interval.

**ACCEPTED C# REUSE:** [InitializeFreshWave](../../src/Tempest.Game/TempestGame.cs),
lines 355-391, creates a new Level/ledger; clears old enemies, shots, bullets,
pending cargo, zap targets, labels, death state, and Hurry-Up; resets
projection and player position; detaches/rebinds Spike callbacks; and resets
Super Zapper to **two charges, zero flash**. Initial lane position remains 0.5,
depth 400, position 50. Fresh Level 6 must have 22 unresolved original IDs,
four Eggs, 18 reserve, no cargo identities yet, and 16 fresh height-60 Spikes.
Life loss must retain the Level, score, remaining charges, resolved identities,
and surviving/shortened Spikes rather than repeat fresh initialization.

Clearing flash at the exact handoff is established C# policy; the source's
charge assignment alone does not establish that precise flash-reset timing.

## 7. Natural Level 5-to-6 transition and exit timing

Commit progression only after successful Level 5 Avoid Spikes travel, never at
wave clear, warning entry, or a failed Spike exit. Reuse
[CompleteSuccessfulExit](../../src/Tempest.Game/TempestGame.cs), lines
1731-1743, by adding the singleton production successor edge.

Carry score and actual remaining lives without reset. Any points earned
shortening Level 5 Spikes must carry too. Install Level 6 once, at approach
start; freeze its wave until arrival; permit normal approach movement; suppress
fire and zap; retain the rim position selected during approach at arrival.

**ACCEPTED C# REUSE:** [LevelApproach.cs](../../src/Tempest.Game/Levels/LevelApproach.cs)
already provides the accepted 181-update arrival, and
[TransitionStarfield.cs](../../src/Tempest.Game/Rendering/TransitionStarfield.cs),
line 44, already selects the white/red/green/blue mixture when **exited level
is 5**. No starfield retuning is needed. This is the first natural production
handoff to exercise that mixed-color branch; Level 4-to-5 must remain blue.

**REFERENCE FACT / CONFLICT - avoidance wait:** Internal level 5 is within
`current_level > 2 And current_level < 7`, so reference Level 6 uses Avoid
Spikes. Its formula is `cnt > 150 - (5-3)*40`, i.e. **71 updates** before
StartZoom, then **21 updates** before travel.
See [tempest.bmx](../../Reference/incitti/tempest.bmx), lines 1344-1363 and
3811-3820.

Current C# [UpdateSpikeExit](../../src/Tempest.Game/TempestGame.cs), lines
1776-1790, hardcodes `++exitDelayTicks > 150`: **151 avoidance updates** on
every configured Spike level, plus 21 delay updates. Accepted Level 5 also
differs from literal source here. This is not a reason to change Level 5.

Recommendation for a source-derived Level 6: one narrow configuration property
for avoidance-wait threshold, default **150**, Level 6 **70**, consumed by the
existing exit state machine. This preserves Levels 4-5 and the diagnostic while
giving Level 6 its source-specific 71-update wait. If Neal instead chooses
unchanged accepted exit pacing, explicitly classify 151 updates on Level 6 as
a project decision; do not silently label it reference-exact.

Keep the accepted arcade-observed firing boundary: movement enabled during
AvoidSpikes/ExitDelay, no new shots until actual travel, in-flight shots
preserved, and travel shots at the accepted Spike-exit cadence. Literal Incitti
permits new firing during the wait; this known conflict is already resolved
in favor of arcade observation. Keep collision-before-shot shortening during
travel, reverse/retry and life accounting unchanged.

## 8. Scoring and reference behavior after Level 6

**REFERENCE FACT:** Ordinary relevant scores remain Flipper 150, Tanker 200
when destroyed (not naturally released), Spiker 50, bullet 25, Spike hit 1.
There is no Level 6-only enemy score or mandatory new bonus mechanism.

The source's `CalcBonus(board,col)` is a **one-time selected-start-level**
bonus path, not an unconditional per-board clear bonus. It is guarded by
`bonuscollected`, reset at level selection but not each natural transition:
[tempest.bmx](../../Reference/incitti/tempest.bmx), lines 1403-1435,
1730-1734, 1790-1793. A direct selected Level 6 would calculate
`5*3000 + 5*4100 = 35,500` on its first successful completion; a natural run
starting Level 1 consumes the guard with a zero bonus on its first completion,
so no such award appears at Level 6. Generic 10,000-point extra lives at lines
738-745 are existing deferred work, not a Level 6-specific prerequisite.

After successful Level 6, the reference clears the old level, increments to
internal 6/displayed **Level 7, clover**, palette 0, installs that board/enemies,
recharges to two charges, resets edge 7, and begins the next approach.
See [tempest.bmx](../../Reference/incitti/tempest.bmx), lines 1418-1434, and
[levelset1.txt](../../Reference/incitti/levelsets/levelset1.txt), lines 14-15.
The reference does **not** terminate at Level 6.

For the bounded milestone, terminal Level 6 is an intentional **PROJECT
DECISION**, just as terminal Level 5 was. Reference continuation is not evidence
that implementing Level 7 is necessary in the same change.

## 9. Reusable systems and exact blockers

| Surface | Audit result |
|---|---|
| [LevelConfiguration.cs](../../src/Tempest.Game/Levels/LevelConfiguration.cs), lines 32-78, 193-218 | Data shape already represents identity, geometry, composition, Spikes and tuning. Missing Level 6 singleton and explicit firing entries; both firing getters throw beyond 5. |
| [LevelLoader.cs](../../src/Tempest.Game/Levels/LevelLoader.cs), lines 34-52 | Missing exact triangle factory. Add data only; no new loader architecture needed. |
| [LevelProgression.cs](../../src/Tempest.Game/Levels/LevelProgression.cs), lines 5-14 | Reference-equality chain ends at Level 4 -> Level 5; Level 5 returns null. Add Level 5 -> Level 6; leave Level 6 null. |
| [LevelSet.cs](../../src/Tempest.Game/Levels/LevelSet.cs), line 2 | Description/list record, no five-level ceiling and not the current production progression registry. No change required. |
| [TempestGame.cs](../../src/Tempest.Game/TempestGame.cs), lines 355-391, 708-795, 1693-1790 | Initialization, mixed enemies, progression commit, and Spike exit reusable. Only source-exact avoidance timing needs the narrow new data seam described above. No new enemy subsystem. |
| [Level.cs](../../src/Tempest.Game/Levels/Level.cs), lines 54-58, 82-143 | Composition-based ledger, initial Spike setup and reconstruction already support this level, despite historical Level3 names. Do not rename/rewrite them. |
| [VectorRenderer.cs](../../src/Tempest.Game/Rendering/VectorRenderer.cs), lines 481-500, 792-814 | Palette 0 supported. Terminal heading uses displayed level dynamically. No Level 6-specific HUD/render system required. |
| [TempestGame.cs](../../src/Tempest.Game/TempestGame.cs), lines 241-253 | Natural terminal R already resets score/lives to a new Level 1 campaign; follows the successor boundary dynamically. |
| [CampaignSoakTests.cs](../../tests/Tempest.Game.Tests/CampaignSoakTests.cs), lines 12-82 | Soak array/endpoint stops at 5; test-only resolver explicitly allows fresh Levels 1-4 only. Extend it to resolve outgoing Level 5, not arbitrary future waves. |
| [Level5ProductionTests.cs](../../tests/Tempest.Game.Tests/Level5ProductionTests.cs), line 57; [LevelProgressionTests.cs](../../tests/Tempest.Game.Tests/LevelProgressionTests.cs), lines 451-506 | Explicit null-successor/terminal-Level-5 expectations must move to Level 6; preserve earlier entry/reconstruction assertions. |

Audio, controls, Super Zapper, score carry, enemy bullet rendering/collision,
Tanker cargo identities, death/re-entry, all-rim completion, Hurry-Up, and
approach projection need no redesign.

## 10. Unknowns and decisions before implementation

1. Approve retaining the accepted mixed picker rather than claiming exact
   Incitti candidate weighting (section 3). Do not quietly fix shared Levels
   4-5 probabilities or cap behavior.
2. Approve Level 6's source-derived 9-degree/20/23 Flipper tuning, rather than
   copying Level 5's 8-degree/21/24 tuple. The existing configuration supports
   it; exact arcade timing remains unmeasured.
3. Choose the recommended source-derived 71-update avoidance wait via a narrow
   configuration seam, or explicitly retain accepted 151-update pacing.
   Either choice must leave Levels 1-5 unchanged.
4. Exact arcade Level 6 species distribution, Spiker random-speed distribution,
   hidden accounting and numeric timing have not been independently measured.
   Existing accepted C# approximations are not to be relabeled arcade facts.
5. If adding direct Level 6 access, decide whether it must exercise production
   Flipper firing. Existing isolated modes do not: the gate at
   [TempestGame.cs](../../src/Tempest.Game/TempestGame.cs), lines 116-118,
   enables that phase for natural progression, not configuration-only mixed
   launches, even with the bullet diagnostic flag. A selector alone would
   therefore be insufficient as a production-firing validation substitute.

These are review items, not permission for implementation in this analysis.
No unknown requires inventing a new geometry, enemy, cargo type, palette,
bonus, or Level 7 dependency.

## 11. Recommended smallest milestone

1. Add an exact Original Arcade triangle definition and immutable production
   Level 6 singleton: identity 5/6/5/0, supply 22, four initial Eggs, 18 reserve,
   accepted production mixed composition/scheduling, height-60 Spikes in all
   lanes, AvoidSpikes, endpoint hinges, Flipper tuple `(true,2,9,20,23)`.
2. Add explicit Level 6 ROM-backed firing entries **65 / 3**.
3. Add only the production Level 5 -> Level 6 successor edge. Reuse the
   successful Spike-exit handoff, mixed-star approach, state carry, and fresh
   initialization; keep Level 6 terminal.
4. Subject to Neal's timing decision, add the single avoidance-wait
   configuration seam (default 150; Level 6 70), preserving all earlier values.
5. Add focused tests and extend the campaign orchestration soak through
   **arrival at Level 6**. Update directly related authority/README statements
   and historical prompt/report logs during implementation, not in this task.

A development-only `--level6-test` selector would materially reduce repeated
campaign traversal for triangle/Spike/death/exit inspection. It is optional
and does not replace natural transition acceptance. Follow existing
[DebugController.cs](../../src/Tempest.Game/Debug/DebugController.cs), lines
89-125, and [Program.cs](../../src/Tempest.Game/Program.cs), lines 82-113,
for exclusivity, configuration routing, window title, replay/restart semantics,
and diagnostic identification. If approved, narrowly enable production-like
Flipper firing for that Level 6 diagnostic without changing existing modes;
test the distinction. Do not introduce a generic level selector framework.

## 12. Explicit exclusions

Level 7+, broad later-level architecture, open tubes, palette cycling,
attract/replay/recording changes, selected-start bonuses and unrelated extra-life
work, high scores, end-game animation, spinner retuning, accepted audio or
approach/presentation retuning, entity-rendering fidelity, shared enemy-policy
corrections, historical documentation cleanup unrelated to this milestone,
and unrelated refactoring. No idealized triangle coordinates.

## 13. Proposed automated validation (not changed or run here)

Use the existing xUnit/headless test seams; no new tooling or interactive game.

- **Exact configuration/geometry:** all 16 ordered coordinates, closedness,
  identity, center/offset, depth/position/start lane, 22 obligations, 4/18
  Egg/reserve split, palette, composition, 16 height-60 Spikes, tuning 9/20/23,
  firing 65/3, and chosen wait threshold. Exercise both directed transitions
  at every hinge including 15/0, finite projections and exact endpoint arrival;
  protect rounded slightly reflex vertices rather than asserting an ideal
  convex triangle.
- **Natural 5 -> 6:** reach Level 5 via the real campaign; resolve test
  obligations using existing helpers, then execute real warning/delay/travel.
  No successor before safe travel; Spike collision/reverse/retry stays on 5.
  Successful exit installs exactly one fresh Level 6 and enters approach.
- **State carry and fresh initialization:** nonzero score and reduced lives,
  including exit Spike-hit points, carry unchanged at commit. Check a fresh
  ledger/4 Eggs/18 reserve/16 new Spikes, no old cargo/shots/bullets/zap targets
  or callback leakage, Hurry-Up/death reset, two charges and zero flash for
  each outgoing 0/1/2-charge state.
- **Approach:** mixed colors for exited 5, prior exited 4 remains blue;
  incoming wave frozen, movement allowed, fire/zap suppressed, normal projection
  and retained rim position at arrival, gameplay begins afterward.
- **Eligibility/composition:** deterministic draws produce each allowed type;
  exclude Fuseballs/Pulsars and their cargo; verify two-Flipper Tanker cargo,
  active-Tanker and Spiker boundaries, strict near-end Spiker suppression and
  retreat/reselection. Encode the approved picker policy, not guessed counts.
- **Firing/tuning:** add Level 6 to exact firing table coverage; exercise a
  natural Level 6 Flipper and cargo firing with 65 reload/three-slot pressure,
  pivot gating, cutoff and audio. Protect existing Tanker/Spiker behavior.
  A configuration-only game currently cannot establish production Flipper
  firing because of the gate noted above.
- **Death/re-entry/accounting:** resolve some originals and cargo, shorten a
  Spike, spend one charge, then lose a life. Retain Level/configuration, score,
  Spike identity/height, remaining charge and resolved dispositions; rebuild
  only unresolved identities with fresh Eggs/runtime firing state. No duplicate
  carrier/cargo or accounting loss; Spike shooting still works afterward.
- **Exit and terminal 6:** complete kill and all-rim routes; unresolved cargo
  blocks exit. Verify chosen wait threshold at its exact boundary (71 updates
  if threshold 70), 21 delay updates, wait movement/no new fire, preserved
  in-flight shots, travel firing/damage/1-point score, swept Spike death,
  reverse/retry and lives. Safe exit ends at Complete on 6, no Level 7 or new
  wave, correct terminal heading, and natural R starts fresh Level 1 with score
  reset/configured lives. Game Over also restarts the campaign correctly.
- **Soak:** append Level 6, explicitly allow fresh outgoing Level 5 in the
  fail-closed test resolver, and move endpoint assertions to 6/exited 5.
  Keep the soak's declared orchestration-only limits: it bypasses combat,
  does not validate difficulty/species distribution, and is not manual acceptance.
- **Regression:** retain existing Level 1-5 entry/gameplay/configuration,
  approach, exit-control, Spike lifecycle, Super Zapper, firing, cargo and
  reconstruction tests. Change only expectations intentionally superseded by
  the new successor/terminal boundary.
- **Optional selector:** reject incompatible flags, compose with `--lives`,
  load the identical singleton, remain isolated, replay correctly, and verify
  the explicitly chosen firing behavior without changing old diagnostics.

Likely focused existing suites:
[Level4CandidateTests.cs](../../tests/Tempest.Game.Tests/Level4CandidateTests.cs),
[Level5ProductionTests.cs](../../tests/Tempest.Game.Tests/Level5ProductionTests.cs),
[LevelProgressionTests.cs](../../tests/Tempest.Game.Tests/LevelProgressionTests.cs),
[LevelApproachTests.cs](../../tests/Tempest.Game.Tests/LevelApproachTests.cs),
[ExitLaneControlTests.cs](../../tests/Tempest.Game.Tests/ExitLaneControlTests.cs),
[ArcadeFlipperFiringTests.cs](../../tests/Tempest.Game.Tests/ArcadeFlipperFiringTests.cs),
[CampaignSoakTests.cs](../../tests/Tempest.Game.Tests/CampaignSoakTests.cs),
plus new Level 6 tests. Run a focused `dotnet test` filter during implementation,
then the project-required `dotnet build` and `dotnet test` before handoff.
Existing shared ledger/Super Zapper/simulation tests supply regression coverage;
do not duplicate their entire matrices for the new board.

## 14. Manual acceptance for Neal after implementation

Neal launches, never VS Code Copilot:

```powershell
dotnet run --project src\Tempest.Game -- --lives 10
```

1. Play natural Levels 1-5; confirm no changed movement, firing, audio, Spike
   lifecycle or prior transitions. Clear Level 5 and safely avoid/shoot its
   Spikes; confirm no premature progression or terminal pause.
2. Observe mixed-color stars, the incoming triangle, recharge message,
   spinner-movable rim Blaster, smooth arrival and no flash/jump/malformed tube.
3. Check Level 6 HUD, carried score/lives, fresh starting Spikes, two usable
   zap charges, and normal controls/audio. Observe Flippers, Tankers/two live
   cargo children, and Spikers; no Fuseballs/Pulsars.
4. Check lane changes and pivots around all triangle corners and the closing
   lane, sensible sparse enemy fire, Spike shortening/1-point scoring, and
   no clipping or stuck entities.
5. Spend a charge, shorten a Spike and lose a life; verify same board,
   retained score/remaining charge/Spike state, sensible enemy re-entry and
   no obvious accounting stall. Do not infer hidden identity correctness from
   visual observation alone.
6. Clear Level 6. Check approved avoidance pacing, movement but no new firing
   during the wait, firing when travel starts, long-Spike collision/reverse/
   retry and eventual safe completion.
7. Confirm terminal Level 6 without Level 7; R returns to a fresh Level 1
   campaign with reset score and configured starting lives. Check Game Over
   restart if practical. A direct diagnostic, if added, supplements but cannot
   replace the natural Level 5 -> 6 run.

## 15. Work performed and final repository state

Performed source/test inspection and one read-only numeric geometry
calculation. No build/test suite was run: this task changes no implementation
or tests and asks for proposed validation. No interactive game was launched.

Only the project-instruction-required prompt/report records were created:

- [Prompt](../Prompts/2026-11-04-level6-preimplementation-analysis-prompt.md).
- [This report](2026-11-04-level6-preimplementation-analysis-report.md).

Final Git check: `## master`, with no file entries; unstaged tracked and cached
diff statistics empty. The two logging files exist on disk but are ignored by
the existing `AI-Logs/` rule at [.gitignore](../../.gitignore), line 6
(confirmed separately with `git check-ignore -v`). They were not force-added.
HEAD remains the required checkpoint. No production, test,
gameplay-documentation, project or reference file modifications; no staging or
commits.

**No Level 6 implementation was performed. Stop here for Neal's review.**
