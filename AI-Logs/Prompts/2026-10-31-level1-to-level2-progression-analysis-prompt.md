Analyze the real Level 1 -> Level 2 progression milestone for Tempest.

Repository:
C:\Users\nrnob\OneDrive\Repro\Tempest

Starting clean checkpoint:
7416dce Implement isolated Level 2 Flipper canflip behavior

ANALYSIS ONLY.

Do not modify gameplay code, tests, README, TEMPEST-DEVELOPMENT-NOTES.md,
TEMPEST-BEHAVIOR-SPEC.md, project/configuration files, or Git state.
Do not implement progression yet.
Do not commit.

PURPOSE

Stages A, B1 and B2 are complete:

A:
- explicit Level configuration/runtime boundary;
- fresh-wave initialization separated from life reconstruction.

B1:
- isolated displayed Level 2 square;
- square endpoint-hinge geometry;
- square rendering/lifecycle manually validated.

B2:
- isolated Level 2 canflip;
- below-rim Flipper lane changes manually compared with Atari Level 2;
- square straight/corner transitions validated;
- no recurrence of the completed-pivot renderer crash.

The next milestone should connect the established Level 1 and Level 2 runtimes
through REAL normal-game progression.

Do not redesign Level 1 or Level 2.

FIRST READ

- AI-Logs/Reports/2026-10-30-progression-architecture-analysis-report.md
- AI-Logs/Reports/2026-10-30-level1-configuration-groundwork-report.md
- AI-Logs/Reports/2026-10-30-level2-square-canflip-analysis-report.md
- AI-Logs/Reports/2026-10-30-level2-square-b1-implementation-report.md
- AI-Logs/Reports/2026-10-31-level2-square-b2-implementation-report.md
- TEMPEST-BEHAVIOR-SPEC.md
- TEMPEST-DEVELOPMENT-NOTES.md
- README.md
- complete checkpoint-7416dce source and tests
- relevant literal Incitti level-transition/setup code.

Treat checkpoint 7416dce and the current behavior specification as project
authority over stale historical reports.

EVIDENCE

Maintain:

ARCADE VERIFIED
REFERENCE VERIFIED
PROJECT DECISION
DEVELOPMENT ONLY

Neal's validated observable Level 1 and Level 2 behavior takes priority over
Incitti where they disagree.

Do not turn source-derived transition details into arcade claims.

PRIMARY QUESTION

Design the smallest safe implementation that changes normal gameplay from:

Level 1
-> tube exit
-> terminal Level Complete
-> R replays Level 1

to:

Level 1
-> successful tube exit
-> fresh Level 2 runtime
-> Level 2 begins naturally

without changing isolated development-scenario replay behavior.

SUCCESSFUL-TRAVEL COMMIT POINT

Identify the exact current C# point where Level 1 exit has successfully
completed and progression can be committed exactly once.

Distinguish:

- wave cleared;
- ExitDelay;
- exit movement;
- Spike avoidance;
- reverse/retry;
- PlayerDying;
- LevelComplete;
- R replay.

Progression must NOT occur merely because the wave becomes clear.

Progression must NOT occur before reversible exit behavior has safely
completed.

Repeated updates must not advance twice.

For ordinary Level 1, determine the smallest robust progression-commit boundary.

INCITTI TRANSITION

Trace literal Incitti Level 1 -> Level 2 behavior:

- when current_level increments;
- when Level 2 board identity becomes active;
- score carry;
- lives carry;
- player start lane;
- player position/depth;
- Super Zapper recharge;
- Eggs/enemy supply reset;
- shots/bullets cleanup;
- rimit/onrim reset;
- pulse/global-clock behavior;
- Fuseball animation behavior;
- geometry/projection K/Z offset reset;
- palette;
- center/offset;
- rotation state;
- bonuses/startbonus;
- ready/begin presentation.

Separate what is required for gameplay correctness from presentation or bonus
systems that may remain deferred.

CURRENT C# NORMAL FLOW

Map the checkpoint-7416dce normal no-flag path through:

- Level 1 initialization;
- Playing;
- kill/all-rim completion;
- ExitDelay;
- ExitTravel;
- LevelComplete;
- R replay.

Identify exactly what currently happens to:

- CurrentLevel;
- active LevelConfiguration;
- Player;
- score;
- lives;
- Super Zapper charges;
- shots;
- EnemyBullets;
- Eggs;
- Flippers;
- Spikes;
- Tanker ledger;
- HurryUp;
- Pulsar clock;
- Fuseball animation;
- exit counters;
- projection state;
- renderer identity;
- window/HUD level identity.

NEXT CONFIGURATION RESOLUTION

Design a minimal production progression resolver.

For this milestone it only needs to support:

normal Level 1 -> normal Level 2.

Do not design an unnecessary generic campaign framework.

Requirements:

- normal initial launch resolves Level 1;
- successful Level 1 completion resolves validated Level 2 configuration;
- Level 2 uses the existing square and canflip behavior;
- Level 2 is Flipper-only with configured supply 12;
- no Level 3 production configuration is required in this milestone;
- development scenarios must stay isolated and terminal/replayable;
- --level2-test must remain a DEVELOPMENT ONLY direct-access path.

Determine what should happen after natural Level 2 completion while Level 3 is
still unimplemented.

Possible policies:
A. Level 2 ends at existing terminal LevelComplete;
B. Level 2 replay;
C. explicit "next level not implemented" terminal state;
D. another minimal policy.

Choose the least misleading and safest behavior.

Do not fake Level 3.

NORMAL VERSUS DEVELOPMENT LAUNCH

The current Level 2 configuration may still be named Level2SquareTest or
otherwise development-oriented.

Determine whether real progression should:

- promote/rename that same configuration to a production Level2 definition
  while --level2-test points to it;
- copy it into a production Level2 configuration;
- or use another minimal arrangement.

Avoid two divergent Level 2 configurations containing duplicate geometry and
tuning.

The development flag should remain direct access to the same validated Level 2
behavior where practical, but its direct launch/replay semantics remain
DEVELOPMENT ONLY.

SCORE AND LIVES

Determine exact current/project policy for:

- score entering Level 2;
- lives entering Level 2;
- life override --lives;
- death on Level 1 before completion;
- death during exit;
- Game Over;
- new-game restart after reaching Level 2.

Do not invent bonus lives.

Do not implement bonus score unless required for transition correctness.

If Incitti awards a between-level/start bonus, document and defer it if safe.

SUPER ZAPPER

This is critical.

Trace Incitti and current C# rules.

Determine:

- remaining Level 1 charges at successful exit;
- recharge point for a fresh Level 2;
- whether Level 2 starts with exactly two charges;
- flash state at transition;
- pending enemy zap state after wave clearing;
- behavior if completion is via all-rim clearing;
- death during exit.

The prior project decision is that fresh-level initialization restores two
charges while ordinary life loss does not.

Determine whether natural Level 2 installation can reuse that existing fresh
initialization behavior without special casing.

EGGS / ACCOUNTING

Normal Level 2 must start fresh:

- configured supply 12;
- four initial Eggs;
- eight reserve;
- destroyed = 0;
- rim-cleared = 0;
- no active Level 1 enemies;
- no Level 1 unresolved reconstruction obligations;
- no stale Tanker/cargo ledger;
- no stale pending zap target;
- no stale collision-order entry.

Even though natural Level 2 is Flipper-only, verify all scenario-specific
accounting is cleanly inactive/reset.

ALL-RIM / HURRY-UP

Natural Level 1 can complete through:

- kills;
- all-rim clearing;
- mixture.

All paths must converge on the same eventual Level 2 transition after
successful exit.

Natural Level 2 retains its implemented all-rim behavior.

HurryUp remains irrelevant to a normal Flipper-only Level 2 unless existing
logic says otherwise.

Do not enable Fuseball/Pulsar hurry-up in production Level 2.

DEATH / EXIT

Analyze carefully:

1. death before Level 1 wave completion;
2. death after wave resolution but before exit completes;
3. any lethal direct/rim Flipper interaction during the exit-start boundary;
4. Game Over before progression;
5. repeated Update after the successful transition;
6. quitting/restarting after reaching Level 2.

Progression must not consume a level advancement merely because a transient
state temporarily looked complete.

SPIKE EXIT POLICY

Normal Level 1 contains no Spikes.

Development --spiker-test must remain terminal/replayable and must NOT
naturally advance to Level 2.

Its reverse/retry behavior must remain exactly as currently validated.

Do not generalize production progression to Spiker scenarios.

DEVELOPMENT SCENARIOS

Preserve:

--level2-test
--spiker-test
--tanker-test
--fuseball-test
--pulsar-test
--enemy-bullets-test
--lives

Rules:

- normal no-flag game can progress Level 1 -> Level 2;
- enemy-specific development scenarios do not progress;
- --level2-test remains direct Level 2 access and terminal/replayable;
- --enemy-bullets-test should preserve its existing semantics and not itself
  make a run a different progression campaign unless the existing launch
  design requires it.

Determine whether normal Level 1 + --enemy-bullets-test should naturally
progress to Level 2 with bullets still enabled.

Prefer orthogonal preservation of that flag if safe.

LEVEL COMPLETE / UI

Determine the visible flow.

Current behavior uses LevelComplete as terminal/replay state.

For successful Level 1 progression, decide whether LevelComplete should:

A. be skipped and Level 2 begins directly;
B. be entered briefly then automatically advance;
C. become a transition-ready state;
D. another source/current-architecture-consistent behavior.

Use literal Incitti and current C# state ownership.

Keep the milestone small.

Do not build a new intermission screen unless necessary.

HUD/window title must show Level 2 once Level 2 is installed.

Do not leave stale Level 1 text.

PROJECTION / GEOMETRY TRANSITION

Confirm fresh Level 2 installation resets:

- Projection.K;
- Z offset;
- Level position;
- center Y;
- Y offset;
- geometry;
- player starting depth/lane;
- renderer active definition.

The first rendered Level 2 frame must use the square configuration rather than
one stale Level 1 frame.

Determine whether the existing fresh-wave initialization already guarantees
this or requires one explicit transition reset.

B2 CANFLIP

Natural Level 2 must use the already validated B2 canflip behavior.

Do not fork another Flipper behavior path.

Do not retune:
- turn increment;
- pauses;
- direction sampling;
- square hinge geometry;
- collisions;
- optional bullets.

Natural progression should consume the same validated configuration/behavior.

REPLAY AND GAME OVER AFTER LEVEL 2

Design precise behavior.

For a natural run that has reached Level 2:

- ordinary life loss stays Level 2;
- Game Over restart: determine whether it starts a fresh game at Level 1 or
  restarts Level 2 under current/Incitti policy;
- completing Level 2 while Level 3 is unavailable must be terminal and must
  not fabricate progression.

For --level2-test:

- Game Over restart/replay should retain existing isolated Level 2 behavior.

Keep direct development semantics separate from normal campaign semantics.

R KEY

Determine what R should mean after:

- terminal natural Level 2 completion;
- Game Over in normal campaign;
- direct --level2-test completion.

Do not accidentally turn R into a hidden level selector.

BONUS / STARTBONUS

Trace the source behavior and explicitly decide whether this milestone can
defer it.

If deferred:
- preserve score through transition;
- award no invented level bonus;
- describe this as an explicit incomplete presentation/scoring feature.

Do not silently suggest arcade-equivalent scoring if bonus is absent.

TANKER FIRING AND BLASTER SMOOTHNESS

Keep the two known deferred observations visible:

- Tanker firing activation/cadence fidelity.
- slight perceived Blaster rim-movement delay/smoothness concern.

Do not fix either in this milestone.

Level 2 natural progression will provide a useful later playability
observation point for Blaster movement, but it is not an input-tuning task.

TEST PLAN

Design deterministic implementation tests.

NORMAL PROGRESSION
- initial normal launch Level 1;
- kill-complete Level 1 -> eventual Level 2;
- all-rim-complete Level 1 -> eventual Level 2;
- mixed kill/rim-clear completion -> eventual Level 2;
- progression occurs exactly once;
- correct transition-commit state;
- no Level 2 before successful exit completion;
- fresh Level 2 config identity/board/supply/canflip;
- first Level 2 wave Flipper-only.

SESSION STATE
- score preserved;
- remaining lives preserved;
- --lives override carries appropriately;
- no bonus invented.

FRESH STATE
- Level 2 starts with two Super Zapper charges;
- HurryUp reset;
- Level 1 entities/projectiles cleared;
- Level 1 accounting cleared;
- fresh Level 2 accounting totals;
- no stale cargo/pending zap/collision entries;
- fresh player pose/projection.

DEATH
- Level 1 death/reconstruction does not advance;
- PlayerDying does not advance;
- Game Over before clear does not advance;
- Level 2 death remains Level 2;
- normal Game Over restart follows chosen campaign policy.

LEVEL 2
- B2 canflip enabled;
- square configuration exact;
- all-rim works;
- kill completion works;
- completing Level 2 stops at the explicitly chosen terminal policy;
- no Level 3 config synthesized.

DEVELOPMENT FLAGS
- --level2-test still direct Level 2;
- R replay stays direct Level 2;
- enemy-specific scenarios stay terminal;
- Spiker reverse/retry does not progress;
- normal + --enemy-bullets-test preserves progression if chosen;
- conflict behavior unchanged.

SUPER ZAPPER
- one/zero remaining charge at end of Level 1 does not prevent fresh Level 2
  recharge;
- flash/pending state reset according to chosen fresh-level policy;
- both charges work in natural Level 2.

UI/RENDER
- no stale Level 1 geometry frame after install;
- displayed level identity changes to 2;
- square rail/render contract preserved;
- no B1/B2 renderer crash regression.

REGRESSION
- all 391 B2 tests remain green or are intentionally updated only where
  terminal-Level-1 assumptions are obsolete;
- explicit direct Level 1 replay tests remain only for development scenarios
  where appropriate;
- Tanker/Spiker/Fuseball/Pulsar scenarios unchanged.

IDENTIFY OBSOLETE TESTS

List every existing test whose expectation must intentionally change when
normal Level 1 becomes progressive.

Do not silently rewrite terminal/replay tests to pass.

ARCHITECTURAL REVIEW

Conclude whether implementation can remain local to:

- configuration resolution;
- a progression/session identity field if needed;
- successful-exit commitment;
- fresh-wave installation;
- terminal Level 2 policy;

or whether another refactor is genuinely required first.

We should NOT add a large campaign framework if Level 1 -> Level 2 can cleanly
fit the Stage A boundaries.

MANUAL VALIDATION PLAN

Define Neal's end-to-end test.

Primary:

dotnet run --project src/Tempest.Game -- --lives 10

Neal should:

- complete Level 1 naturally;
- observe automatic entry into the square Level 2;
- verify Level 2 is displayed/recognizable;
- verify score carries;
- verify lives carry;
- verify Super Zapper is recharged;
- verify Eggs begin a 12-unit Level 2 wave;
- verify below-rim Level 2 canflip behavior;
- verify death/re-entry remains in Level 2;
- verify Level 2 all-rim/kill completion;
- verify no invented Level 3 appears;
- note Blaster smoothness but do not tune it.

Also define regression commands for development scenarios.

Do not require timer measurement.

BEHAVIOR SPEC

Identify exact sections that should change only AFTER implementation and manual
validation.

Likely:
- current normal-start/progression statement;
- Level 1 completion;
- Level 2 development-only qualification;
- Super Zapper recharge across levels;
- validation history.

Do not edit the specification during analysis.

OUTPUT

Save this exact prompt to:

AI-Logs/Prompts/2026-10-31-level1-to-level2-progression-analysis-prompt.md

Save the analysis to:

AI-Logs/Reports/2026-10-31-level1-to-level2-progression-analysis-report.md

Report:

1. executive recommendation;
2. current C# transition flow;
3. literal Incitti transition;
4. successful-travel commit point;
5. production configuration resolution;
6. normal versus development launch policy;
7. score/lives policy;
8. Super Zapper policy;
9. accounting/reset policy;
10. death/Game Over policy;
11. Level 2 terminal policy;
12. R/replay semantics;
13. projection/render transition;
14. B2 reuse;
15. bonus deferral decision;
16. development scenario preservation;
17. obsolete tests;
18. automated test plan;
19. manual validation plan;
20. architecture/refactor conclusion;
21. behavior-spec impact;
22. explicit out-of-scope items;
23. remaining arcade uncertainties;
24. GO / NO-GO implementation recommendation.

STOP after analysis/report generation.

Do not implement.
Do not modify tracked files.
Do not commit.