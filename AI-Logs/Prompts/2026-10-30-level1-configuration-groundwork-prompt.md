Implement the approved behavior-preserving Level 1 configuration and
initialization groundwork milestone.

Repository:
C:\Users\nrnob\OneDrive\Repro\Tempest

Starting clean checkpoint:
2741f2a Implement arcade all-rim completion behavior

FIRST read:

AI-Logs/Reports/2026-10-30-progression-architecture-analysis-report.md
TEMPEST-BEHAVIOR-SPEC.md
TEMPEST-DEVELOPMENT-NOTES.md
README.md

Also inspect the complete current source, tests and relevant Incitti reference
before editing.

The progression architecture report is the approved design authority for this
milestone.

If current code contradicts the report, investigate and document the
difference rather than forcing the report blindly.

Do not commit.

PURPOSE

Establish a behavior-preserving configuration and initialization boundary for
the existing Level 1 implementation.

This is progression groundwork only.

Do not implement:
- Level 2;
- square geometry;
- canflip;
- natural level advancement;
- mixed production waves;
- open-tube gameplay;
- dynamic palettes;
- bonuses;
- extra lives;
- audio;
- progression entry presentation.

The running game must remain observably identical at the end of this milestone.

CONFIGURATION MODEL

Introduce the smallest explicit configuration needed to describe the current
supported Level 1 wave.

The configuration must provide or resolve:

- zero-based progression identity: 0;
- displayed level identity: 1;
- board identity/index: 0;
- palette identity/index: 0;
- the existing Level 1 LevelDefinition;
- continuous/closed topology;
- vertex count;
- playable lane count;
- Level.Depth;
- Level.Position baseline;
- initial player lane;
- enemy supply: 10;
- initial active Eggs: 4;
- reserve release interval and current active-Egg cap;
- current deterministic spawn-lane sequence;
- current deterministic Egg direction sequence;
- current exit policy;
- current Level 1 Flipper-only normal composition.

Do not claim the current Egg cap, pacing or deterministic sequences are
literal Incitti behavior where the architecture report found differences.

These remain established PROJECT DECISIONS unless already classified
otherwise.

LEVEL RUNTIME

Retain Level as the per-wave runtime object.

Do not add another LevelRuntime hierarchy solely for naming.

Change Level so it accepts the existing Level 1 definition/configuration
rather than always constructing Level 1 internally.

Preserve convenient existing construction only if it delegates to the explicit
Level 1 configuration and cannot create a second conflicting initialization
path.

The configured enemy supply must replace the hardwired ten-unit total in
runtime accounting.

Do not make Level 2 reachable.

LEVEL DEFINITION / TOPOLOGY CONTRACT

Use the existing LevelDefinition/LevelSet concepts where practical.

Establish an explicit distinction between:

- vertex count;
- playable lane count.

For the current closed Level 1 tube:
- 16 ordered vertices;
- 16 playable lanes.

Permit geometry/configuration tests to demonstrate that an open definition
would expose 15 playable lanes from 16 vertices.

Do not claim or implement full open-tube support.

Do not alter current closed adjacency, player movement, Egg movement, enemy
movement, renderer closure or collision behavior.

The topology contract is groundwork only.

FRESH INITIALIZATION VERSUS RECONSTRUCTION

Create one clearly named fresh-wave initialization path owned by TempestGame.

It is used for:

- initial game setup;
- Level Complete development replay;
- Game Over restart.

It must install/create a fresh Level instance from the current launch
configuration and then initialize the game-owned per-wave state.

Keep life reconstruction as a separate path that retains the same Level
runtime/configuration and preserves established same-attempt outcomes.

Document this distinction with focused maintainability comments.

FRESH-WAVE RESET CONTRACT

Fresh initialization must preserve current behavior:

- current launch scenario and development flags;
- score/lives policies for initial start, Complete replay and Game Over restart;
- current player lane and lane position;
- current projection K and Z offset;
- current Egg supply and deterministic release data;
- current enemy accounting;
- current Tanker original/cargo ledger behavior;
- current starting Spikes in the Spiker development scenario;
- current Super Zapper two-charge restore and flash reset;
- current Hurry-Up reset;
- current shot/bullet/entity/list cleanup;
- current queued cargo cleanup;
- current replay and restart behavior;
- current player-depth and exit counters;
- current cursor/input behavior.

Do not accidentally turn Complete replay into level advancement.

Do not reset every clock generically.

Preserve current pulse-phase, random-source and Fuseball-animation reset/carry
semantics exactly as they exist at checkpoint 2741f2a unless the current code
shows that the report misstated one.

If those lifetimes are inconsistent, preserve them and document them rather
than normalizing them in this milestone.

EVENT / CALLBACK OWNERSHIP

Level exposes SpikeCreated behavior used by the game.

When replacing a Level during fresh initialization:

- detach from the previous Level where necessary;
- attach exactly once to the new Level;
- prevent stale or duplicate SpikeCreated callbacks;
- ensure old Spike objects or callbacks cannot enter the new runtime;
- ensure Spiker development replay creates only the intended initial hazards.

Do not add broad event infrastructure.

SCENARIOS AND FLAGS

Preserve without semantic change:

--spiker-test
--tanker-test
--fuseball-test
--pulsar-test
--enemy-bullets-test
--lives

Preserve existing mutual exclusions and error behavior.

Each development scenario must remain a fixed replayable artificial wave.

Do not convert flags into production progression eligibility.

Do not combine scenarios.

Normal no-flag startup remains Level 1 Flippers only.

ACCOUNTING

Normal accounting:

- configured original supply replaces the hardcoded ten where appropriate;
- destroyed and rim-cleared outcomes remain distinct;
- reserve/active Eggs and active enemies remain conserved;
- death reconstruction and all-rim behavior remain unchanged.

Tanker scenario:

- preserve the existing controlled original-slot and cargo ledger;
- retain the current one-Tanker development design;
- do not generalize multiple production Tankers yet;
- fresh replay/restart resets its identity sets and queued cargo exactly once;
- life reconstruction preserves established resolved outcomes.

Super Zapper:

- preserve charges through life reconstruction;
- fresh replay/restart restores two charges;
- pending target timers remain runtime-owned and are discarded through
  established reconstruction/fresh-level cleanup;
- all-rim cancellation and flash behavior remain unchanged.

SPIKES

Preserve:

- starting Spike construction in the Spiker development scenario;
- persistent Spike hazards through death and exit/retry;
- fresh replay/restart restoring initial hazards;
- all-rim enemy clearing without clearing Spike hazards prematurely;
- no duplicate hazard callbacks after repeated fresh initialization.

GAME/RENDERER ACCESS

Current rendering should continue to read the currently installed Level through
TempestGame.

Do not make the renderer load level data.

Do not change geometry or projection.

Update HUD/title identity only if necessary to consume explicit displayed Level
1 identity while producing exactly the same visible Level 1 text.

Do not add a next-level message.

Do not change Program's terminal Complete/replay route.

TESTS

Add focused tests for the new architecture without weakening existing behavior.

At minimum:

CONFIGURATION
- default Level 1 config identity is internal 0 / displayed 1 / board 0 /
  palette 0;
- configured LevelDefinition matches the existing 16 ordered points,
  continuous flag, center and offset;
- closed definition exposes 16 vertices and 16 playable lanes;
- an isolated open-definition fixture exposes 16 vertices and 15 playable
  lanes without claiming open gameplay support;
- configured supply is 10;
- four initial Eggs and six reserve remain;
- deterministic spawn lanes and Egg directions remain unchanged;
- release/approach timing remains unchanged.

CONSTRUCTION
- default construction delegates to explicit Level 1 configuration if retained;
- no second hidden Level 1 definition path can diverge;
- a DEVELOPMENT ONLY configured-total fixture can initialize a different
  total without becoming reachable gameplay;
- Level completion/accounting uses configured total rather than constant 10.

FRESH INITIALIZATION
- initial setup creates one current Level and one correct event subscription;
- Complete replay replaces the Level once and preserves existing score/lives;
- Game Over restart replaces Level once and resets score/starting lives;
- repeated replay/restart does not duplicate Spike callbacks or hazards;
- fresh initialization clears queued cargo, runtime entities, shots/bullets,
  pending timers, outcome ledgers and exit state as currently intended;
- charges, Hurry-Up and projection reset according to existing policy;
- player starts on the established valid lane and position.

RECONSTRUCTION
- ordinary death retains the same Level instance/configuration;
- score, prior destroyed/rim-cleared/escaped outcomes, charges, Hurry-Up and
  persistent Spikes retain current semantics;
- unresolved units return through existing Egg/cargo reconstruction;
- no fresh-level reset occurs during life loss or Spike reverse/retry.

SCENARIOS
- normal startup remains Flipper-only;
- every existing scenario launches the same composition and hazards;
- flag conflicts still reject;
- relevant enemy-bullet combinations remain unchanged;
- development replay remains same-scenario rather than advancing.

REGRESSION
- spinner/wrap;
- firing and shot collisions;
- direct Flipper arrival;
- completed-pivot safety;
- Eggs/hatching;
- Tanker cargo movement/accounting;
- Fuseball/Pulsar behavior and Hurry-Up;
- Super Zapper charge/timer/flash behavior;
- all-rim completion;
- Spiker exit/reverse/retry;
- Complete replay;
- Game Over restart.

Do not add fake Level 2 gameplay tests.

Do not add progression assertions beyond explicit Level 1 identity and
configured initialization.

MAINTAINABILITY

Add comments only where they explain:

- configuration versus runtime ownership;
- fresh-wave initialization versus life reconstruction;
- event detach/rebind;
- why development replay remains terminal/same-level;
- why vertex count differs from playable lane count.

Do not add comments that merely restate code.

DOCUMENTATION

Update README.md and TEMPEST-DEVELOPMENT-NOTES.md after implementation to
describe:

- explicit Level 1 configuration;
- Level remaining the runtime object;
- fresh initialization versus reconstruction;
- vertex/lane-count groundwork;
- no visible progression yet;
- no Level 2 support yet.

Also perform only the targeted current-truth cleanup identified in the approved
analysis where directly related to this milestone:

README:
- remove duplicate Flipper bullet section;
- consolidate duplicate Tanker sections using the final outward cargo route
  and current combined Tanker-bullet behavior;
- mark old milestone test totals/pending-manual language as historical or
  replace with current truth where safe.

TEMPEST-DEVELOPMENT-NOTES:
- mark early kill-all-only completion and pending all-rim status as superseded;
- correct blanket Flipper-arrival wording so it does not contradict the
  current direct-arrival versus completed-pivot distinction;
- do not delete useful milestone history.

Do not broadly rewrite historical reports.

Do not update TEMPEST-BEHAVIOR-SPEC.md during implementation.

The behavior specification records no new visible behavior for this milestone.
After manual regression validation, add only a narrowly scoped history entry
if useful.

VALIDATION

Run:

- focused configuration/initialization tests;
- focused reset/reconstruction tests;
- focused scenario tests;
- focused Tanker/accounting tests;
- focused Super Zapper tests;
- focused all-rim/Hurry-Up/Spiker-exit tests;
- full solution test suite;
- solution build;
- git diff --check;
- git status --short.

Record exact counts and intermediate failures honestly.

MANUAL VALIDATION

Provide a regression-focused checklist.

Neal should rerun:

dotnet run --project src/Tempest.Game -- --lives 10
dotnet run --project src/Tempest.Game -- --spiker-test --lives 10
dotnet run --project src/Tempest.Game -- --tanker-test --lives 10
dotnet run --project src/Tempest.Game -- --fuseball-test --lives 10
dotnet run --project src/Tempest.Game -- --pulsar-test --lives 10

Also run relevant bullet combinations if automated results or manual behavior
suggest an interaction concern.

This milestone is a regression check, not arcade validation of progression.

REPORTING

Save this exact prompt to:

AI-Logs/Prompts/2026-10-30-level1-configuration-groundwork-prompt.md

Save the implementation report to:

AI-Logs/Reports/2026-10-30-level1-configuration-groundwork-report.md

Report:

1. purpose and starting checkpoint;
2. configuration model;
3. Level runtime changes;
4. fresh initialization ownership;
5. reconstruction separation;
6. event/callback lifecycle;
7. scenario/flag preservation;
8. accounting preservation;
9. topology vertex/lane contract;
10. files changed;
11. tests added/updated;
12. focused/full/build/diff/status results;
13. intermediate failures;
14. manual checklist;
15. explicit out-of-scope items;
16. progression roadmap relationship;
17. remaining architectural risks.

STOP after implementation, automated validation, documentation updates and
report generation.

Do not commit.
Do not modify TEMPEST-BEHAVIOR-SPEC.md.
Do not begin square/canflip work.
Do not begin real progression.