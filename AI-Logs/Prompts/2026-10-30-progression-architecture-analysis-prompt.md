Perform a whole-system progression architecture analysis for the Tempest
recreation.

Repository:
C:\Users\nrnob\OneDrive\Repro\Tempest

Starting clean checkpoint:
2741f2a Implement arcade all-rim completion behavior

ANALYSIS ONLY.

Do not modify gameplay code, tests, README, TEMPEST-DEVELOPMENT-NOTES.md,
TEMPEST-BEHAVIOR-SPEC.md, project/configuration files, or Git state.
Do not implement Level 2 or progression yet.
Do not commit.

PURPOSE

Before adding real multi-level progression, establish a coherent architectural
map of Mark Incitti's level/progression system and compare it with the CURRENT
C# architecture.

The goal is to prevent a late redesign caused by implementing Level 2 before
understanding systems that become interconnected through progression.

This should be a broader architectural analysis than previous enemy-specific
milestones.

FIRST READ

Current project authority/state:

- TEMPEST-BEHAVIOR-SPEC.md
- TEMPEST-DEVELOPMENT-NOTES.md
- README.md
- current C# source
- all current tests
- Reference/incitti/tempest.bmx
- Reference/incitti/Readme.txt
- all level/board/tube data files used by Incitti
- relevant recent AI-Logs reports

Especially review milestone history for:

- Eggs/accounting
- Flippers
- Spikers/spikes
- enemy bullets
- Tankers/cargo
- Fuseballs
- Pulsars
- Hurry-Up/rimit
- Super Zapper
- all-rim completion

Treat checkpoint 2741f2a and TEMPEST-BEHAVIOR-SPEC.md as current project
authority over stale historical reports.

EVIDENCE MODEL

Preserve:

ARCADE VERIFIED
REFERENCE VERIFIED
PROJECT DECISION
DEVELOPMENT ONLY

Incitti explains Incitti.

Do not infer that Incitti progression, tuning, geometry or composition is
arcade-correct merely because it exists in the source.

Neal's existing arcade observations take precedence.

GOAL

Answer:

"What architecture do we need to understand and establish before implementing
real Level 2 and subsequent progression without painting the C# recreation into
a corner?"

Do not answer only "how to implement Level 2."

Map the progression system as a whole, then recommend the smallest safe first
implementation milestone.

INCITTI LEVEL IDENTITY

Trace precisely:

- current_level
- displayed level number
- current_board
- current_color
- current_spin
- current_set
- level/tube set selection
- modulo/wrap behavior
- rotation behavior
- progression beyond one 16-board cycle
- progression after later cycles
- applicable level caps/maximums
- how board geometry is selected
- how level colors are selected
- how geometry/color/spin interact

Distinguish:

- actual gameplay progression;
- editor/tube-set features;
- alternate level sets that are not necessarily relevant to Atari recreation.

BOARD / TUBE ARCHITECTURE

Analyze:

- number of source boards/tubes relevant to progression;
- closed versus open tubes;
- how many edge/lane points each board uses;
- whether all boards actually use 16 playable edges;
- open-board endpoint behavior;
- player movement at open endpoints;
- FindShortDir behavior;
- enemy wrap versus bounce behavior;
- Flipper/Pulsar movement implications;
- Tanker cargo lane placement;
- Super Zapper distance calculations;
- renderer assumptions;
- collision assumptions;
- projection parameters stored per board;
- Y center / Y offset or equivalent board parameters;
- tube rotation/spin behavior;
- whether existing C# Level abstraction can represent these concepts.

Identify every current C# assumption that implicitly means:
"Level 1 closed 16-lane tube."

Do not change it yet.

LEVEL SETUP AND TUNING

Trace SetUpLevel / SetUpEnemies and all related source state.

Produce a table/formula map by zero-based and displayed level for:

- EnemyTotal / max Eggs
- initial Eggs
- active Egg limits
- hatching limits
- release rate
- hatch/approach rate
- Flipper speed
- Flipper pivot speed
- Flipper pauses
- canflip
- Spiker availability
- Spiker tuning
- Tanker availability
- Tanker speed
- Tanker ammunition/firing
- direct Fuseball availability
- Fuseball speed
- direct Pulsar availability
- Pulsar movement/flip speed
- Pulsar pauses
- pulse settings
- Tanker cargo type eligibility
- Super Zapper recharge
- Hurry-Up reset
- any other level-sensitive gameplay value currently implemented in C#.

For each value identify:

- exact source formula/gate;
- first affected displayed level;
- whether C# currently hardcodes a development approximation;
- whether arcade behavior has been manually verified;
- whether it must be part of initial Level 2 progression or can wait.

ENEMY COMPOSITION

Trace the literal source decision path from Egg hatch to enemy-selection type.

Determine:

- which enemy categories are eligible at each progression point;
- relative/random selection logic;
- restrictions near the end of a wave;
- maximum active counts;
- type-specific gates;
- how active enemies/Egg counts affect selection;
- whether source composition is determined independently for each hatch;
- whether Tankers replace an original unit or conceptually introduce cargo
  beyond it;
- how released cargo affects counting;
- later Tanker-carried Flipper/Pulsar/Fuseball eligibility;
- interaction with rimit/onrim completion.

Produce a clear progression/composition map.

Do not convert that map into an arcade-fidelity claim.

LEVEL 1 -> LEVEL 2 TRANSITION

Trace exactly what the source does after successful tube exit:

- state transition;
- level increment;
- board selection;
- score preservation;
- lives preservation;
- Super Zapper recharge;
- bonus/startbonus behavior;
- player position;
- projection reset;
- Egg/enemy reset;
- spike reset;
- bullets/shots cleanup;
- Pulsar oscillator reset or preservation;
- Hurry-Up reset;
- color update;
- renderer state;
- any intro/ready state;
- any delay between levels.

Compare this with the current C# LevelComplete + R-to-replay model.

Identify the minimal architectural changes required to make completion naturally
advance to Level 2 rather than replay Level 1.

BONUS / EXTRA-LIFE SYSTEM

Trace source bonus behavior separately.

Determine:

- level-completion bonus;
- bonus-life thresholds if present;
- startbonus / bonuscollected or related fields;
- when score bonuses apply;
- whether any bonus mechanic is required for progression correctness;
- whether it may safely be deferred from the first Level 2 milestone.

Do not implement it.

SUPER ZAPPER ACROSS LEVELS

Trace:

- exact recharge point;
- whether remaining charges carry across exit;
- setup/reset interactions;
- death behavior;
- Game Over restart;
- interaction with level transition.

Compare to the current C# two-charge lifecycle.

Identify what must change once real progression exists.

ACCOUNTING ACROSS LEVELS

Analyze how checkpoint-2741f2a accounting should generalize:

- EnemiesDestroyed
- EnemiesRimCleared
- ReserveEggs
- active Eggs
- enemy lists
- Tanker original-slot IDs
- materialized cargo IDs
- escaped/rim-cleared/killed outcomes
- pending zap state
- death reconstruction
- new-level reset.

Determine whether current IDs can remain level-local.

Determine whether Level should become:
- reusable per-level state on one object,
- replaced by a new Level instance,
- driven by a LevelDefinition + LevelRuntime split,
- or another minimal design.

Do NOT recommend abstraction merely because it is architecturally fashionable.

Recommend it only if the source/progression requirements justify it.

RENDERING / GEOMETRY

Map what the renderer will need for real board progression:

- closed/open topology;
- variable edge count if applicable;
- color/palette;
- board coordinates;
- Y center / offset;
- spin/rotation;
- lane adjacency;
- near/far rim handling;
- Pulsar lane-overlay semantics;
- Spike geometry;
- player path;
- exit zoom on non-Level-1 boards.

Determine which rendering assumptions must be addressed before Level 2 can be
shown correctly.

Differentiate:

A. required for Level 2;
B. required for the first open tube;
C. required only for later rotations/sets.

LEVEL 2 / CANFLIP

Deeply analyze Incitti's first transition from displayed Level 1 to displayed
Level 2.

Specifically determine:

- Level 2 board geometry;
- closed/open status;
- colors/tuning;
- enemy total;
- composition eligibility;
- whether Level 2 still contains only Flippers;
- canflip activation;
- exact Flipper mid-tube state changes;
- random decisions;
- lane-transition geometry;
- collision behavior during mid-tube flips;
- firing behavior;
- pause/timing;
- player pursuit;
- what differs from Level 1;
- whether those differences can be isolated and manually validated.

Compare with Neal's previous observation that Level 1 Flippers largely climb
straight up and do not pursue below the rim.

Do not assume later-level Flipper behavior has been arcade-verified.

OPEN-TUBE FIRST OCCURRENCE

Identify the first progression level/board where open-tube semantics become
necessary.

If that is Level 2, say so explicitly and determine its consequences.

If it is later, identify exactly what can safely be deferred.

DEVELOPMENT FLAGS

Recommend what happens eventually to:

--spiker-test
--tanker-test
--fuseball-test
--pulsar-test
--enemy-bullets-test
--lives

They should remain useful as deterministic development scenarios even after
natural progression exists unless there is a concrete reason otherwise.

Do not remove them during the first progression milestone.

CURRENT DOCUMENTATION QUALITY

While analyzing, identify stale or contradictory statements in:

- README.md
- TEMPEST-DEVELOPMENT-NOTES.md
- TEMPEST-BEHAVIOR-SPEC.md

For example, current documents may contain historical statements saying normal
completion is kill-all or that manual validation is pending even though later
milestones superseded them.

Do not edit documentation during this analysis.

List cleanup items separately so history is not confused with current truth.

ARCHITECTURAL RISK REVIEW

Explicitly answer these questions:

1. Does current Level.cs need significant redesign before progression?
2. Is TempestGame too Level-1-specific to extend safely?
3. Can current enemy classes accept per-level tuning without redesign?
4. Can current Tanker/cargo ledger survive changing wave size/composition?
5. Can all-rim completion generalize to later levels?
6. Can Super Zapper generalize across levels?
7. What breaks first on an open tube?
8. Is the current renderer capable of loading a second board definition?
9. Are any development shortcuts now architectural blockers?
10. Is there anything that should be refactored BEFORE Level 2 rather than
    while implementing Level 2?

Use concrete evidence from current code/source.

PROGRESSION ROADMAP

Recommend a staged progression roadmap.

Do not assume the whole progression system should be one milestone.

Evaluate a sequence such as:

A. level-definition/runtime groundwork
B. Level 2 geometry + transition
C. Level 2 canflip Flipper behavior
D. first open-tube support
E. first Tanker-natural progression
F. first Spiker-natural progression
G. later Fuseball/Pulsar composition
H. bonus/audio/presentation

But derive the actual recommended order from evidence rather than accepting
that example.

For each proposed stage provide:

- scope;
- prerequisites;
- files likely affected;
- automated tests;
- manual arcade checks;
- behavior-spec impact;
- rollback/checkpoint boundary.

NEXT MILESTONE

Recommend exactly ONE next implementation milestone.

The milestone should be:

- small enough to review and manually validate;
- architecturally useful;
- not intentionally incorrect;
- not dependent on unimplemented future machinery;
- compatible with checkpoint recovery;
- clear about what remains DEVELOPMENT ONLY versus real progression.

Possibilities include:
- progression groundwork without changing visible gameplay;
- real Level 1 -> Level 2 transition;
- Level 2 definition/geometry only;
- Level 2 canflip only;
- another prerequisite discovered by the analysis.

Choose one.

TEST STRATEGY

Design tests for the recommended next milestone.

Include:
- Level 1 regression;
- level identity;
- completion -> next-level transition where applicable;
- score/lives preservation;
- Zapper recharge;
- Egg supply reset/scaling;
- accounting reset;
- all-rim reset;
- Hurry-Up reset;
- projection reset;
- board geometry;
- player lane validity;
- enemy eligibility;
- replay/Game Over semantics;
- development flags still working.

MANUAL VALIDATION

Specify what Neal would need to observe.

Separate:
- visually testable arcade behavior;
- source-derived constants;
- architectural state that automated tests, not gameplay observation, should
  establish.

BEHAVIOR SPECIFICATION

Identify future spec sections that would change after implementation/manual
validation.

Do not edit the specification now.

OUTPUT

Create a whole-system report with:

1. executive architectural summary;
2. Incitti progression state model;
3. board/tube model;
4. level-setup and tuning matrix;
5. enemy composition matrix;
6. Level 1 -> Level 2 transition;
7. bonus/extra-life analysis;
8. Super Zapper lifecycle across levels;
9. accounting/reset model;
10. rendering/topology requirements;
11. Level 2/canflip specifics;
12. first open-tube dependency;
13. development-flag strategy;
14. current documentation inconsistencies;
15. architectural risk assessment;
16. staged roadmap;
17. exactly one recommended next milestone;
18. automated-test strategy;
19. manual-validation strategy;
20. behavior-spec impact;
21. explicit out-of-scope items;
22. remaining arcade uncertainties.

Conclude with:

- whether significant architectural refactoring is required before Level 2;
- whether we are currently at risk of having built ourselves into a corner;
- GO / NO-GO for beginning progression work.

Save this exact prompt to:

AI-Logs/Prompts/2026-10-30-progression-architecture-analysis-prompt.md

Save the analysis to:

AI-Logs/Reports/2026-10-30-progression-architecture-analysis-report.md

AI-Logs remains gitignored.

STOP after analysis/report generation.

Do not implement.
Do not modify tracked files.
Do not commit.