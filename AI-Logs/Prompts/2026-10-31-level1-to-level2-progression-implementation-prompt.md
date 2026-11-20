Implement real normal-game Level 1 -> Level 2 progression.

Repository:
C:\Users\nrnob\OneDrive\Repro\Tempest

Starting clean checkpoint:
7416dce Implement isolated Level 2 Flipper canflip behavior

FIRST READ

- AI-Logs/Reports/2026-10-31-level1-to-level2-progression-analysis-report.md
- AI-Logs/Reports/2026-10-30-progression-architecture-analysis-report.md
- AI-Logs/Reports/2026-10-30-level1-configuration-groundwork-report.md
- AI-Logs/Reports/2026-10-30-level2-square-b1-implementation-report.md
- AI-Logs/Reports/2026-10-31-level2-square-b2-implementation-report.md
- TEMPEST-BEHAVIOR-SPEC.md
- TEMPEST-DEVELOPMENT-NOTES.md
- README.md

Also inspect the complete checkpoint-7416dce source and tests and the relevant
literal Incitti transition/setup code before editing.

The progression analysis report is the approved implementation design.

If current code contradicts the report, investigate and document the
difference rather than forcing the report blindly.

Do not commit.

PURPOSE

Connect the already validated normal Level 1 and Level 2 configurations through
real progression.

Implement:

normal Level 1
-> successful standard tube exit
-> fresh natural Level 2 runtime

Do not redesign either level.

Do not implement:

- Level 3;
- a generic campaign framework;
- open tubes;
- production Tankers;
- production Spikers;
- production Fuseballs;
- production Pulsars;
- mixed production waves;
- bonus score;
- bonus lives;
- audio;
- source Begin/Ready entry animation;
- dynamic palettes;
- board rotation;
- Tanker firing changes;
- Blaster input/smoothness tuning.

EVIDENCE

Maintain:

ARCADE VERIFIED
REFERENCE VERIFIED
PROJECT DECISION
DEVELOPMENT ONLY

Neal has manually validated Level 1 behavior, broad all-rim completion, the
isolated Level 2 square and qualitative Level 2 canflip behavior.

The new end-to-end natural transition is not arcade-verified until Neal
manually tests it.

SUCCESSFUL-TRAVEL COMMIT

Progress exactly once at the established successful standard-exit boundary:

- ordinary standard ExitZoom;
- after the exit movement update;
- when updated Player.Depth < 10.

Do not progress at:

- wave clear;
- all-rim commitment;
- BeginExit;
- ExitDelay;
- intermediate ExitZoom;
- PlayerDying;
- life reconstruction;
- Game Over;
- LevelComplete;
- R replay;
- Spike avoidance/reverse/retry.

At the successful Level 1 exit boundary:

- resolve the validated Level 2 configuration;
- install a fresh Level 2 runtime during that same update;
- return without another Level 1 update;
- leave the game in Playing;
- ensure repeated updates cannot advance twice.

Use an explicit outgoing-state/configuration guard.

Do not create a transaction framework, event bus, generation ID or transition
queue.

PRODUCTION CONFIGURATION

Promote or rename the existing Level2SquareTest configuration to a
production-neutral Level2 or Level2Square configuration.

Use exactly one immutable Level 2 configuration object containing the already
validated:

- internal progression identity 1;
- displayed level 2;
- board 1;
- palette 0;
- closed 16-lane square;
- supply 12;
- four initial Eggs;
- eight reserve units;
- Flipper-only composition;
- B1 square endpoint-hinge geometry;
- B2 canflip behavior and tuning.

--level2-test must select this same configuration object.

Do not duplicate square coordinates or Level 2 tuning.

Keep direct-launch/replay behavior DEVELOPMENT ONLY through launch/session
policy rather than through a duplicate configuration.

PROGRESSION RESOLVER

Add the smallest resolver needed:

- production Level 1 -> production Level 2;
- production Level 2 -> no next configuration.

Do not create a broad level table unless the current architecture genuinely
requires one.

Development scenarios must not become progression-eligible merely because they
use Level 1 identity.

NORMAL VERSUS DEVELOPMENT POLICY

Natural progression is enabled for:

- normal no-flag launch;
- normal launch with --lives <n>;
- normal launch with --enemy-bullets-test;
- normal launch with both --enemy-bullets-test and --lives <n>.

Natural progression is disabled for:

- --level2-test;
- --spiker-test;
- --tanker-test;
- --fuseball-test;
- --pulsar-test.

Preserve existing scenario conflict behavior.

--enemy-bullets-test remains orthogonal and should stay enabled after natural
Level 1 -> Level 2 progression.

--lives remains orthogonal.

Do not turn either option into a development-campaign selector.

SCORE AND LIVES

At natural Level 1 -> Level 2 transition:

- carry Score unchanged;
- carry current Player.Lives unchanged;
- do not reapply the --lives starting value;
- award no level bonus;
- award no bonus life;
- do not reset the session's configured starting-lives value.

Death before successful exit remains on Level 1 and follows existing
reconstruction.

Death on Level 2 remains on Level 2.

SUPER ZAPPER

Reuse the existing fresh-wave reset.

Natural Level 2 begins with:

- exactly two Super Zapper charges;
- flash state reset to zero;
- no stale pending target.

Do not reset charges when the Level 1 wave first clears or when exit begins.

Ordinary life loss still preserves remaining charges.

Test transition with:

- two Level 1 charges remaining;
- one remaining;
- zero remaining;
- active or recently completed flash where practical.

All must enter Level 2 with two fresh charges and cleared flash.

This reset is a PROJECT DECISION aligned with the existing fresh-wave policy.
Do not claim exact Atari flash behavior.

FRESH LEVEL 2 STATE

After natural installation, verify:

- CurrentLevel uses production Level 2 configuration;
- State is Playing;
- displayed level is 2;
- board 1 / palette 0;
- square geometry active;
- canflip enabled;
- enemy supply 12;
- four active Eggs;
- eight reserve;
- EnemiesDestroyed 0;
- EnemiesRimCleared 0;
- RemainingEnemies 12;
- no Level 1 Flippers;
- no old Eggs;
- no shots;
- no EnemyBullets;
- no Spikes;
- no Tankers;
- no Spikers;
- no Fuseballs;
- no Pulsars;
- no pending Tanker cargo;
- no stale Tanker original/cargo ledger;
- no old collision-order entry;
- no stale pending zap target;
- no stale Level callback;
- HurryUp false;
- death and exit counters reset;
- player at configured Level 2 start lane, offset and depth;
- Projection.K 70;
- Projection.ZOffset 5.

Use the existing fresh-wave cleanup rather than duplicating these resets.

Do not generically reset clocks that Stage A intentionally preserved.

LEVEL 2 BEHAVIOR

Natural Level 2 must reuse the exact validated B2 path.

Do not create or fork another Flipper update path.

Do not change:

- canflip decision behavior;
- direction sampling;
- turn increment;
- pause timing;
- height behavior;
- square hinge geometry;
- radial endpoint correction;
- shot collision policy;
- player collision policy;
- optional enemy-bullet mechanics;
- Super Zapper behavior;
- all-rim behavior.

Natural progression is a lifecycle milestone, not a Level 2 gameplay retune.

ALL-RIM AND KILL COMPLETION

Natural Level 1 may clear through:

- all kills;
- all-rim clearing;
- a mixture of kills and rim-cleared units.

Every valid route must converge on the same standard exit and eventual Level 2
installation.

Do not distinguish the transition based on how the Level 1 accounting was
resolved.

Natural Level 2 retains:

- kill completion;
- all-rim completion;
- no false score for rim-cleared enemies.

LEVEL 2 TERMINAL POLICY

Level 3 is not implemented.

When natural Level 2 successfully completes its exit:

- enter terminal LevelComplete;
- do not synthesize Level 3;
- do not replay Level 2 automatically;
- do not return silently to Level 1.

Present an explicit message equivalent to:

LEVEL 2 COMPLETE
NEXT LEVEL NOT IMPLEMENTED

Include a clear R instruction for starting a new campaign.

Do not add a Level 3 configuration.

R / RESTART POLICY

Normal campaign:

- successful Level 1 exit has no LevelComplete pause or R prompt;
- Level 2 starts automatically;
- R after natural Level 2 terminal completion starts a new campaign at Level 1;
- reset score;
- restore configured starting lives;
- restore two Zapper charges through normal fresh initialization.

Game Over in a normal campaign, including after reaching Level 2:

- R starts a new campaign at Level 1;
- reset score;
- restore starting lives.

Direct --level2-test:

- LevelComplete R continues to replay Level 2;
- preserve existing direct-development score/lives replay semantics;
- Game Over restart preserves the existing direct Level 2 policy.

Other development scenarios:

- preserve same-scenario terminal replay;
- preserve current score/lives replay behavior;
- never progress naturally.

R must not become a level selector.

SPIKER EXIT

Do not alter any Spiker behavior.

--spiker-test remains terminal and replayable.

Spike avoidance, collision, reverse and retry must never call the production
progression resolver.

UpdateSpikeExit / completeWhenFinished false remains outside natural
progression.

PROJECTION / RENDERER / UI

At successful Level 1 transition:

- install Level 2 before RunOneStep returns;
- ensure the first following draw uses square Level 2 geometry;
- do not render one stale Level 1 frame;
- restore Projection.K and ZOffset through existing fresh initialization;
- use Level 2 center/offset and active definition immediately.

Update visible level identity:

- HUD identifies Level 2 after installation;
- window title updates when active displayed level changes;
- direct --level2-test remains visibly development-only;
- normal Level 1 remains correctly identified before progression.

Do not add a renderer-owned level cache.

Do not add a full intermission screen.

LEVEL 2 GAME OVER / LIFE LOSS

On nonfinal Level 2 death:

- retain the same Level 2 runtime/configuration;
- reconstruct unresolved units through the existing life-loss path;
- preserve score;
- preserve remaining Zapper charges;
- do not fall back to Level 1.

On final Level 2 death:

- enter Game Over;
- normal-campaign R starts Level 1 from the configured beginning.

BONUS / PRESENTATION OMISSIONS

Explicitly preserve these omissions:

- no level-completion bonus;
- no startbonus;
- no bonus life;
- no source Begin/Ready entry animation;
- no source entry-position bonus shift;
- no audio.

Score simply carries into Level 2.

Document this as incomplete presentation/economy, not arcade-equivalent
behavior.

OBSOLETE TESTS

Update only the obsolete normal-Level-1 terminal/replay expectations identified
in the approved analysis:

- FreshReplayReplacesLevelAndPreservesScoreAndLives
- ExitZoomIgnoresInputRestartAndFurtherClearTriggers
- ZoomCompletesOnlyBelowTenThenRestoresProjectionAndFreezes
- ExitReplayRestoresProjectionDepthEggsAndCountersButKeepsScoreLives
- ExitRendererTracksPlayerDepthAndAllProjectedVerticesStayFinite
- LevelCompletionFreezesAndReplayRetainsScoreAndLives
- EntireEggWaveCanBeDestroyedWithSameTickExitThenCompletion
- RestartAndReplayRestoreChargesWhileInactiveInputIsRejected

Rename or split them so test names describe the new intended behavior.

Do not weaken the tested invariants.

Preserve terminal/replay tests for direct development scenarios.

Search for any additional test that assumes normal Level 1 terminal completion.
If found, report and update it explicitly rather than silently.

TESTS

Add deterministic progression coverage.

COMMIT POINT:
- initial normal launch is Level 1;
- wave clear alone does not advance;
- ExitDelay does not advance;
- intermediate ExitZoom does not advance;
- updated depth below 10 installs Level 2;
- transition occurs exactly once;
- repeated updates do not skip or reinstall.

CLEAR ROUTES:
- kill-only Level 1 -> Level 2;
- all-rim Level 1 -> Level 2;
- mixed destroyed/rim-cleared Level 1 -> Level 2.

CONFIGURATION:
- natural Level 2 is the same configuration object selected by --level2-test;
- board/palette/topology/supply/canflip exact;
- first natural Level 2 wave is Flipper-only.

SESSION:
- score carries unchanged;
- current lives carry unchanged;
- --lives override is not reapplied;
- no bonus score;
- no bonus life.

FRESH STATE:
- four Eggs/eight reserve/12 remaining;
- destroyed and rim-cleared zero;
- old entities/projectiles/cargo/ledgers/collision entries absent;
- HurryUp false;
- player pose correct;
- projection 70/5;
- two Zapper charges;
- flash cleared.

DEATH:
- Level 1 death stays Level 1;
- PlayerDying does not advance;
- Game Over before advancement does not advance;
- Level 2 nonfinal death reconstructs Level 2;
- normal-campaign Game Over after Level 2 restarts Level 1.

LEVEL 2:
- validated B2 canflip active;
- square rendering active;
- kill/all-rim completion terminal;
- explicit no-Level-3 policy;
- no Level 3 identity/configuration.

DEVELOPMENT:
- --level2-test remains direct and replays Level 2;
- --spiker-test remains terminal/replayable;
- Tanker/Fuseball/Pulsar scenarios remain terminal/replayable;
- Spiker reverse/retry never advances;
- normal + --enemy-bullets-test advances and retains the option;
- conflict handling unchanged.

SUPER ZAPPER:
- transition from zero/one/two Level 1 charges always yields two Level 2 charges;
- flash cleared;
- both Level 2 charges usable;
- ordinary Level 2 death preserves remaining charges.

UI/RENDER:
- active identity changes to displayed Level 2;
- no stale circle frame after commit;
- square render finite;
- no B1/B2 renderer crash regression.

REGRESSION:
- all 391 checkpoint tests remain green except intentionally updated obsolete
  Level 1 terminal assumptions;
- all existing enemy development scenarios retain their behavior;
- Tanker cargo and bullets unchanged;
- Pulsar/Fuseball/Spiker behavior unchanged.

MAINTAINABILITY

Add focused comments explaining:

- successful-travel progression commit versus wave clear;
- why development scenarios remain terminal;
- why launch configuration remains the new-game restart target;
- why natural Level 2 is terminal without Level 3.

Do not add comments that merely restate code.

DOCUMENTATION

After implementation and automated validation, update:

- README.md
- TEMPEST-DEVELOPMENT-NOTES.md

Document:

- normal Level 1 now progresses automatically to Level 2;
- Level 2 uses the validated square/canflip behavior;
- score and current lives carry;
- Zapper recharges through fresh Level 2 setup;
- no bonus/bonus life;
- Level 2 is terminal because Level 3 is unavailable;
- --level2-test remains direct development access/replay;
- other development scenarios remain terminal.

Do NOT update TEMPEST-BEHAVIOR-SPEC.md during implementation.

Wait for Neal's end-to-end manual validation before changing behavior claims.

VALIDATION

Run:

- focused progression lifecycle tests;
- focused Level configuration/reset tests;
- focused exit tests;
- focused B1/B2 tests;
- focused Super Zapper tests;
- focused all-rim tests;
- focused development-scenario replay tests;
- full solution test suite;
- solution build;
- git diff --check;
- git status --short.

Record exact counts and intermediate failures honestly.

MANUAL VALIDATION CHECKLIST

Primary:

dotnet run --project src/Tempest.Game -- --lives 10

Neal should:

1. Complete Level 1.
2. Confirm automatic entry into square Level 2 after successful exit.
3. Confirm no LevelComplete/R pause between Level 1 and Level 2.
4. Confirm displayed identity changes to Level 2.
5. Confirm score carries unchanged.
6. Confirm remaining lives carry unchanged.
7. Confirm Super Zapper is restored to two charges.
8. Confirm Level 2 begins with a fresh 12-unit wave.
9. Confirm Level 2 below-rim canflip behavior.
10. Lose a life and confirm reconstruction remains in Level 2.
11. Complete Level 2 by kills and/or all-rim resolution.
12. Confirm no Level 3 appears.
13. Confirm explicit terminal Level 2 message.
14. Press R and confirm a fresh Level 1 campaign begins with reset score and
    configured starting lives.

Regression:

dotnet run --project src/Tempest.Game -- --level2-test --lives 10
dotnet run --project src/Tempest.Game -- --level2-test --enemy-bullets-test --lives 10
dotnet run --project src/Tempest.Game -- --spiker-test --lives 10
dotnet run --project src/Tempest.Game -- --tanker-test --lives 10
dotnet run --project src/Tempest.Game -- --tanker-test --enemy-bullets-test --lives 10
dotnet run --project src/Tempest.Game -- --fuseball-test --lives 10
dotnet run --project src/Tempest.Game -- --pulsar-test --lives 10
dotnet run --project src/Tempest.Game -- --enemy-bullets-test --lives 10

Manual validation need not measure timing constants.

Keep the known Tanker-firing and Blaster-smoothness observations deferred.

REPORTING

Save this exact prompt to:

AI-Logs/Prompts/2026-10-31-level1-to-level2-progression-implementation-prompt.md

Save the implementation report to:

AI-Logs/Reports/2026-10-31-level1-to-level2-progression-implementation-report.md

Report:

1. purpose/start checkpoint;
2. evidence boundary;
3. successful-travel commitment;
4. production resolver/configuration reuse;
5. normal/development policy;
6. score/lives carry;
7. Super Zapper recharge;
8. fresh Level 2 accounting/state;
9. death/Game Over policy;
10. natural Level 2 terminal/R policy;
11. projection/render/UI transition;
12. B2 reuse;
13. development-scenario preservation;
14. bonus/presentation omissions;
15. files changed;
16. obsolete tests intentionally updated;
17. tests added;
18. focused/full/build/diff results;
19. intermediate failures;
20. manual checklist;
21. explicit out-of-scope items;
22. remaining arcade uncertainties.

STOP after implementation, automated validation, README/development-note
updates and report generation.

Do not commit.
Do not modify TEMPEST-BEHAVIOR-SPEC.md.
Do not implement Level 3.
Do not begin another milestone.