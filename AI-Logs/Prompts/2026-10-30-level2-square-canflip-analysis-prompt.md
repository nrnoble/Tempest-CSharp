Analyze Stage B of Tempest progression: Level 2 square geometry and canflip
Flipper behavior in isolation.

Repository:
C:\Users\nrnob\OneDrive\Repro\Tempest

Starting clean checkpoint:
f41a53c Add Level 1 configuration and initialization groundwork

ANALYSIS ONLY.

Do not modify gameplay code, tests, README, TEMPEST-DEVELOPMENT-NOTES.md,
TEMPEST-BEHAVIOR-SPEC.md, project/configuration files, or Git state.
Do not implement Stage B yet.
Do not commit.

PURPOSE

Stage A established explicit Level 1 configuration and fresh-wave/runtime
boundaries.

Stage B must determine the exact design needed to represent and test:

- the displayed Level 2 square tube;
- Level 2 Flipper mid-tube canflip behavior;

without yet implementing:

- automatic Level 1 -> Level 2 progression;
- open tubes;
- Tanker production waves;
- Spiker production waves;
- Fuseball/Pulsar production waves;
- bonuses/audio;
- later-level composition.

The goal is to solve square geometry and Level 2 Flipper movement independently
before natural progression depends on them.

FIRST READ

- AI-Logs/Reports/2026-10-30-progression-architecture-analysis-report.md
- AI-Logs/Reports/2026-10-30-level1-configuration-groundwork-report.md
- TEMPEST-BEHAVIOR-SPEC.md
- TEMPEST-DEVELOPMENT-NOTES.md
- README.md
- current checkpoint-f41a53c C# source and tests
- Reference/incitti/tempest.bmx
- Incitti board/tube data used by displayed Level 2
- relevant Flipper milestone reports

Treat checkpoint f41a53c and current TEMPEST-BEHAVIOR-SPEC.md as the project
authority over historical reports.

EVIDENCE MODEL

Maintain:

ARCADE VERIFIED
REFERENCE VERIFIED
PROJECT DECISION
DEVELOPMENT ONLY

Incitti's Level 2 behavior is reference evidence, not automatic Atari proof.

Neal has manually established Level 1 Flippers as largely climbing their hatch
lane without dynamically pursuing the Blaster below the rim.

Do not extrapolate that Level 1 observation to Level 2.

LEVEL 2 DEFINITION

Establish the exact Incitti displayed-Level-2 configuration:

- zero-based level identity;
- displayed identity;
- board index;
- palette;
- geometry points;
- continuous/closed state;
- vertex count;
- playable lane count;
- depth;
- center/offset/projection data;
- enemy supply;
- initial Eggs;
- release timing;
- enemy eligibility;
- canflip state;
- Flipper climb speed;
- Flipper turn speed;
- pause values;
- bullet behavior;
- any other value that differs from Level 1.

Identify which values Stage B needs in an isolated Level 2 configuration and
which should remain deferred until real progression.

SQUARE GEOMETRY

The progression architecture report identified a blocker in the existing
Flipper turn-angle calculation on the Level 2 square.

Reproduce and explain the issue precisely.

For every adjacent playable lane pair on the square:

- calculate the current C# turn geometry;
- identify which pairs yield zero, non-zero, corner or ambiguous turns;
- compare the renderer's representation with Incitti's geometry model;
- distinguish lane-edge geometry from vertex geometry;
- determine what Incitti means visually when a Flipper moves between adjacent
  lanes along a straight side of the square.

Do not patch zero-degree turns with an arbitrary epsilon or fake fixed angle.

Derive the correct conceptual model.

Determine whether a mid-tube Flipper transition should:

- rotate around a shared far/near endpoint;
- interpolate across adjacent lane geometry;
- use a signed hinge around one endpoint;
- use another source-derived construction.

Explain how it should appear on:

- straight square sides;
- square corners;
- current Level 1 circular tube.

LEVEL 1 REGRESSION REQUIREMENT

The existing manually validated Level 1 Flipper pivot presentation must remain
unchanged.

If the correct generalized geometry would numerically differ from Level 1,
recommend either:

A. one generalized method proven equivalent for Level 1;
B. a topology/geometry-specific helper;
C. another minimal design.

Do not broadly rewrite VectorRenderer unless justified.

INCITTI CANFLIP STATE MACHINE

Trace literal Level 2 Flipper behavior.

Establish:

- where canflip is checked;
- which Flipper states can initiate a mid-tube flip;
- counter/random decision cadence;
- direction selection;
- player-lane influence;
- whether direction is chosen toward the Blaster;
- whether direction remains fixed once a transition starts;
- height behavior during transition;
- height behavior during pauses;
- destination lane change timing;
- collision lane ownership while turning;
- player collision behavior below rim;
- shot collision from source/destination lanes;
- firing behavior during mid-tube states;
- interaction with reaching the rim during a transition;
- rim behavior after canflip;
- random-call ordering;
- exact source timing constants.

Identify which behavior is REFERENCE VERIFIED and which remains an arcade
manual-validation item.

LEVEL 2 PLAYER PURSUIT QUESTION

Pay special attention to player following.

Neal observed Level 1 normal Flippers do not dynamically follow the Blaster
while climbing.

Determine whether Incitti Level 2 canflip behavior causes below-rim lane
changes specifically in response to the player's lane.

If yes, identify:
- when the player lane is sampled;
- how often;
- whether moving the Blaster during an already-started flip changes the
  destination;
- whether this differs conceptually from the Tanker-cargo bug that was already
  corrected.

Do not assume Incitti matches Atari.

ISOLATED STAGE B ACCESS

Recommend the safest way to run Stage B manually without enabling progression.

Possibilities include an explicit DEVELOPMENT ONLY command such as a Level 2
test scenario, an internal test configuration, or another approach.

Choose one approach.

Requirements:

- must not make Level 2 naturally reachable yet;
- must clearly be DEVELOPMENT ONLY;
- must coexist with existing scenario flags without ambiguous combinations;
- must allow Neal to run the square/Flipper wave graphically;
- must allow death/re-entry, Super Zapper, all-rim completion and replay to be
  exercised on the isolated Level 2 definition;
- replay must remain Level 2 within this isolated test rather than advance to
  Level 3;
- normal no-flag startup must remain Level 1.

Do not add a development flag unless it materially improves manual isolation.

LEVEL 2 COMPOSITION

Confirm whether the isolated Stage B Level 2 should contain only Flippers.

If Incitti Level 2 composition is Flipper-only, Stage B should not introduce
Tankers, Spikers, Fuseballs or Pulsars simply to exercise infrastructure.

Determine whether enemy bullets should:
- follow current development gating;
- be naturally enabled in the Level 2 isolation;
- remain separately opt-in.

Use source evidence and project evidence discipline.

ACCOUNTING / RESET

Analyze how Stage A configuration and current accounting support an isolated
Level 2:

- configured total;
- initial/reserve Eggs;
- destroyed;
- rim-cleared;
- death reconstruction;
- all-rim completion;
- Super Zapper charges;
- Hurry-Up applicability;
- Level Complete;
- isolated replay;
- Game Over restart.

No Level 1 -> Level 2 transition yet.

Ensure Level 2 identity does not leak into normal Level 1 replay/restart.

SUPER ZAPPER

Super Zapper must remain usable on isolated Level 2.

Analyze:
- distance calculations on the closed 16-lane square;
- all-target behavior;
- single-target behavior;
- flash/render implications on a different board;
- recharge on fresh isolated replay;
- life-loss persistence.

Do not change Super Zapper targeting unless square topology proves an actual
issue.

ALL-RIM COMPLETION

Confirm that checkpoint-2741f2a/f41a53c all-rim semantics generalize to
isolated Level 2:

- Egg supply exhausted;
- below-rim Flipper blocks;
- all remaining Flippers qualify at rim;
- non-kill clear;
- no false scoring;
- exit starts;
- replay returns to isolated Level 2.

Identify any dependency on Level 1 geometry/constants.

RENDERER

Analyze every renderer assumption Stage B must address:

- board-line submission;
- projection center/offset;
- closed near/far rim;
- Flipper climbing glyph;
- mid-tube pivot geometry;
- rim pivot geometry;
- player path on square;
- shots;
- Eggs;
- Super Zapper flash;
- HUD level identity.

Separate:
- changes required to display isolated Level 2 correctly;
- changes only needed for the first open tube;
- later dynamic palette/spin changes.

MANUAL VALIDATION TARGET

Define what Neal should compare with Atari Level 2.

At minimum:

- square shape/proportions;
- Blaster movement around square lanes;
- normal Flipper ascent;
- presence and frequency of mid-tube lane changes;
- whether those changes follow player movement;
- visual transition along straight square sides;
- transitions around corners;
- shot collision during a transition;
- player collision below rim;
- rim pursuit;
- all-rim exit;
- death/re-entry;
- Super Zapper.

Do not require Neal to measure update ticks by eye.

TANKER FIRING AND BLASTER SMOOTHNESS

These two deferred observations were recorded during Stage A:

1. Tanker firing activation/cadence may differ from Atari.
2. Blaster rim movement has a slight perceived smoothness/delay issue that may
   matter at high difficulty.

Do not fix either in Stage B analysis or implementation.

However, identify whether Level 2 square testing creates a useful manual point
to reassess Blaster movement smoothness.

Do not alter input code unless Stage B geometry actually requires lane-count
correctness.

TEST PLAN

Design deterministic tests for eventual Stage B implementation.

Include:

CONFIGURATION
- isolated Level 2 identity/configuration;
- square points;
- closed 16-lane topology;
- correct configured supply/Egg values;
- Flipper-only composition;
- canflip enabled.

GEOMETRY
- every adjacent lane pair has a valid non-degenerate transition;
- expected straight-side transition;
- expected corner transition;
- wrap pair 15<->0;
- signed direction;
- source/destination symmetry where appropriate;
- current Level 1 geometry unchanged.

CANFLIP
- disabled Level 1 behavior unchanged;
- enabled Level 2 can initiate mid-tube transitions;
- exact decision boundary;
- random-call ordering;
- direction/player relationship;
- stable destination during active transition;
- continued climb/pause behavior;
- shot collision;
- no below-rim player death unless source establishes it;
- rim arrival and pursuit.

ISOLATED LIFECYCLE
- normal launch remains Level 1;
- isolated Level 2 launches square;
- Level 2 replay stays Level 2;
- Game Over restart stays Level 2;
- death reconstruction stays same runtime/configuration;
- score/lives policies preserved;
- Super Zapper lifecycle preserved;
- all-rim completion works;
- flags/conflicts remain deterministic.

RENDER
- square tube submission;
- player path;
- Flipper mid-tube transitions;
- corner transitions;
- shot/Egg projection;
- flash recolor;
- finite coordinates.

REGRESSION
- all 339 Stage A tests remain meaningful;
- Level 1 manual behavior assumptions unchanged;
- Tanker cargo correction unchanged;
- Spiker/Fuseball/Pulsar development scenarios remain Level 1.

ARCHITECTURE QUESTION

Explicitly conclude:

- whether Stage B requires changing Flipper's state representation;
- whether Stage B requires separating mid-tube pivot geometry from rim pivot
  geometry;
- whether current renderer callbacks are sufficient;
- whether LevelConfiguration needs tuning fields now or whether those can stay
  local until real progression;
- whether any newly discovered issue requires revisiting Stage A before
  implementation.

NEXT IMPLEMENTATION SCOPE

Produce an implementation-ready recommendation for Stage B.

Decide whether Stage B should be:

A. square geometry + isolated Level 2 configuration only;
B. square geometry + canflip behavior in one milestone;
C. split into B1 geometry and B2 canflip;
D. another scope justified by the analysis.

Choose exactly one.

Favor checkpoints small enough that a visual geometry mistake can be corrected
without entangling the Flipper state machine unnecessarily.

BEHAVIOR SPEC

Identify what, if anything, should be added to TEMPEST-BEHAVIOR-SPEC.md after
Stage B manual validation.

Do not edit it now.

OUTPUT

Save this exact prompt to:

AI-Logs/Prompts/2026-10-30-level2-square-canflip-analysis-prompt.md

Save analysis to:

AI-Logs/Reports/2026-10-30-level2-square-canflip-analysis-report.md

Report sections:

1. executive conclusion;
2. exact Level 2 reference configuration;
3. square geometry analysis;
4. current zero-angle failure;
5. correct pivot/hinge model;
6. Level 1 compatibility;
7. Incitti canflip state machine;
8. player-pursuit behavior;
9. isolated Stage B access design;
10. accounting/reset behavior;
11. Super Zapper/all-rim interaction;
12. rendering requirements;
13. manual arcade-validation plan;
14. deferred Tanker/input observations;
15. deterministic test plan;
16. architecture conclusions;
17. exactly one recommended implementation scope;
18. expected files;
19. Behavior Spec impact;
20. remaining arcade uncertainties;
21. GO / NO-GO implementation recommendation.

STOP after analysis/report generation.

Do not implement.
Do not modify tracked files.
Do not commit.