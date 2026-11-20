Implement Stage B1: isolated Level 2 square configuration and Flipper hinge
geometry.

Repository:
C:\Users\nrnob\OneDrive\Repro\Tempest

Starting clean checkpoint:
f41a53c Add Level 1 configuration and initialization groundwork

FIRST read:

AI-Logs/Reports/2026-10-30-level2-square-canflip-analysis-report.md
AI-Logs/Reports/2026-10-30-progression-architecture-analysis-report.md
AI-Logs/Reports/2026-10-30-level1-configuration-groundwork-report.md
TEMPEST-BEHAVIOR-SPEC.md
TEMPEST-DEVELOPMENT-NOTES.md
README.md

Also inspect the complete current C# source, tests, Incitti Level 2 board data
and relevant literal Incitti Flipper geometry before editing.

The Stage B analysis is the approved design authority.

If current source contradicts the report, investigate and document the
difference rather than forcing the report blindly.

Do not commit.

PURPOSE

Implement only Stage B1:

- isolated Level 2 square configuration;
- DEVELOPMENT ONLY graphical access;
- correct square Flipper endpoint-hinge geometry;
- renderer/configuration/lifecycle validation.

Do NOT implement:

- canflip;
- ordinary below-rim Level 2 Flipper lane-changing behavior;
- natural Level 1 -> Level 2 progression;
- production Level 2;
- Tankers;
- Spikers;
- Fuseballs;
- Pulsars;
- open-tube behavior;
- dynamic palette progression;
- bonus/extra lives;
- audio;
- Tanker firing changes;
- Blaster input/smoothness tuning.

Normal no-flag startup must remain observably Level 1.

LEVEL 2 DEVELOPMENT CONFIGURATION

Add an explicit isolated Level 2 configuration based on the literal reference
data established by the approved analysis:

- internal level identity: 1;
- displayed level: 2;
- board index: 1;
- palette index: 0;
- continuous/closed;
- 16 vertices;
- 16 playable lanes;
- depth 400;
- position baseline 50;
- center Y 400;
- Y offset -80;
- initial player lane 7;
- enemy supply 12;
- four initial Eggs;
- eight reserve units;
- normal composition: Flippers only.

Use the exact ordered square points from the approved analysis/reference:

(161,161)
(80,161)
(0,161)
(-80,161)
(-161,161)
(-161,80)
(-161,0)
(-161,-80)
(-161,-161)
(-80,-161)
(0,-161)
(80,-161)
(161,-161)
(161,-80)
(161,0)
(161,80)

Use valid 0..15 deterministic spawn lanes/directions compatible with the
existing Stage A runtime.

Do not present the existing deterministic C# Egg scheduling choices as Incitti
or Atari behavior.

Keep canflip DISABLED in B1.

This configuration exists only to isolate geometry and current lifecycle
behavior before B2.

ISOLATED ACCESS

Add:

--level2-test

This is DEVELOPMENT ONLY.

Requirements:

- no flags -> normal Level 1 exactly as before;
- --level2-test -> isolated square Level 2 configuration;
- mutually exclusive with:
  --spiker-test
  --tanker-test
  --fuseball-test
  --pulsar-test
- --lives remains composable;
- --enemy-bullets-test may remain separately composable;
- preserve existing flag conflicts and deterministic error behavior.

Do not add any natural progression path.

Level Complete replay with R must recreate the isolated Level 2 configuration.

Game Over restart must recreate the isolated Level 2 configuration.

Life loss must retain the same Level 2 runtime/configuration through the
existing reconstruction path.

Make the window/HUD visibly identify this development run as Level 2 where the
current Level 1 text would otherwise be misleading.

Do not add production progression messaging.

CONFIGURATION/LIFECYCLE

Use the Stage A configuration/runtime boundary.

Do not create another runtime hierarchy.

Level remains the mutable wave runtime.

Fresh isolated Level 2 creation must use the existing fresh-wave installation
path and correct SpikeCreated/event lifecycle.

Do not duplicate reset logic.

Accounting must use configured supply 12 rather than Level 1 assumptions.

Preserve:
- destroyed versus rim-cleared outcomes;
- score/lives policy;
- Super Zapper lifecycle;
- shot/bullet cleanup;
- death reconstruction;
- all-rim completion;
- replay semantics.

SQUARE FLIPPER HINGE GEOMETRY

Do not change Level.CalculateEdgeTurnAngle globally.

Do not change established Level 1 Flipper pivot output.

Implement a narrowly scoped Flipper transition geometry helper appropriate for
the isolated square.

For two adjacent square lanes:

- identify their shared vertex;
- treat that shared projected vertex as the hinge;
- identify the old lane's unshared endpoint;
- identify the destination lane's unshared endpoint;
- calculate the hinge sweep between those two rays.

Expected magnitude:

- straight-side adjacent lane pairs -> 180 degrees;
- corner adjacent pairs -> 90 degrees;
- wrap pair 15 <-> 0 -> 90 degrees.

Required straight-side pairs include:

0/1, 1/2, 2/3,
4/5, 5/6, 6/7,
8/9, 9/10, 10/11,
12/13, 13/14, 14/15.

Required corner pairs:

3/4, 7/8, 11/12, 15/0.

Do not use epsilon fixes.

Do not substitute midpoint interpolation.

At 180 degrees, choose the sweep sign deterministically from the
source/destination lane direction so the same transition does not randomly
change arc.

Verify geometrically that the transformed free endpoint lands on the intended
destination endpoint.

LEVEL 1 COMPATIBILITY

This is mandatory.

The current Level 1 circular tube must retain:

- existing Flipper pivot angles;
- existing pivot timing;
- existing renderer output;
- existing direct-arrival collision;
- existing completed-pivot collision behavior;
- Tanker cargo behavior;
- Pulsar behavior.

If plumbing is needed in Flipper/renderer to choose the square hinge sweep,
isolate it behind the installed definition/configuration rather than
reinterpreting all existing pivots.

Do not change the broader shared turn helper used by established mechanics.

B1 FLIPPER BEHAVIOR

Canflip remains disabled.

Isolated Level 2 Flippers therefore use the current basic climb-to-rim
behavior for B1.

B1 is NOT intended to claim complete Atari Level 2 behavior.

The purpose is to verify:

- square geometry;
- climb presentation;
- rim pivot geometry;
- player movement;
- lifecycle/accounting;
- projection/rendering.

Do not implement the Incitti Level 2 below-rim canflip state machine yet.

RENDERING

Use the existing renderer architecture.

Verify/render:

- 16 rails;
- near rim;
- far rim;
- Player around the square;
- Eggs;
- shots;
- climbing Flipper;
- square Flipper hinge transitions;
- Super Zapper flash.

Do not add a generic new renderer.

Do not implement open-tube drawing.

Do not modify Blaster movement/input sensitivity.

The square is a useful MANUAL observation point for the previously noted
movement-smoothness concern, but B1 must not attempt to fix it.

SUPER ZAPPER

The isolated square remains a closed 16-lane tube.

Preserve existing:

- two charges;
- all-target behavior;
- single-target behavior;
- circular lane-distance semantics;
- death persistence;
- fresh replay recharge;
- flash behavior.

Do not change targeting.

ALL-RIM COMPLETION

Preserve the implemented all-rim behavior with configured supply 12:

- reserve/incoming Eggs exhausted;
- below-rim survivor blocks exit;
- all remaining Flippers at qualifying rim condition -> non-kill clear;
- no false kill score;
- exit begins.

Replay must return to isolated Level 2.

ENEMY BULLETS

Preserve the current opt-in policy.

--level2-test alone should not automatically enable Flipper bullets solely
because Incitti gives source Flippers ammunition.

--level2-test --enemy-bullets-test may exercise the existing bullet system.

Do not retune bullet cadence/ammunition.

TESTS

Add deterministic B1 tests.

CONFIGURATION:
- identity internal 1/display 2;
- board 1/palette 0;
- exact 16 square points;
- continuous true;
- vertex count 16;
- playable lanes 16;
- Depth 400;
- Position 50;
- CenterY 400;
- YOffset -80;
- player lane 7;
- supply 12;
- four initial Eggs / eight reserve;
- Flipper-only composition;
- canflip explicitly false for B1 fixture.

SCENARIO:
- normal startup still Level 1;
- --level2-test selects isolated square Level 2;
- incompatible scenario flags reject;
- --lives composes;
- --enemy-bullets-test composes without being implicitly enabled;
- replay stays Level 2;
- Game Over restart stays Level 2;
- death reconstruction remains same Level 2 runtime/configuration.

GEOMETRY:
- current legacy square helper demonstrates the known twelve 0° / four 90°
  outputs so the failure remains explicit;
- new square hinge gives 180 degrees on all twelve straight-side pairs;
- 90 degrees on four corners;
- 15<->0 works;
- both travel directions;
- deterministic 180-degree sign;
- rotated free endpoint reaches correct destination endpoint;
- no NaN/infinite coordinates.

LEVEL 1 REGRESSION:
- pin current Level 1 adjacent pivot values/output;
- normal Level 1 Flipper behavior unchanged;
- Tanker cargo unchanged;
- Pulsar geometry unchanged.

ACCOUNTING/LIFECYCLE:
- supply 12 accounting;
- kill route;
- rim-clear route;
- no false score;
- life reconstruction;
- replay/restart;
- score/lives policy;
- Super Zapper recharge/persistence.

RENDER:
- exact 16 square rails;
- 16 near-rim segments;
- 16 far-rim segments;
- finite Player/Egg/shot/Flipper square projections;
- straight-side hinge capture;
- corner hinge capture;
- destination endpoint reached;
- flash recolor does not alter geometry.

REGRESSION:
- full current suite remains green;
- existing five development scenarios remain Level 1;
- all-rim/Hurry-Up;
- Spiker exit;
- Tanker accounting;
- Super Zapper.

Do not add B2 canflip tests.

MAINTAINABILITY

Add focused comments explaining:

- why the square requires endpoint-hinge geometry;
- why Level 1 keeps its existing pivot calculation;
- why B1 disables canflip;
- why --level2-test is DEVELOPMENT ONLY and terminal.

Avoid broad explanatory comments elsewhere.

DOCUMENTATION

Update README.md and TEMPEST-DEVELOPMENT-NOTES.md after implementation.

Clearly state:

- --level2-test is DEVELOPMENT ONLY;
- square geometry/configuration is present;
- B1 deliberately disables canflip;
- Level 2 is not naturally reachable;
- R replays isolated Level 2;
- no natural progression exists yet;
- Level 1 remains default.

Do not update TEMPEST-BEHAVIOR-SPEC.md yet.

No authentic Level 2 behavior should be promoted until Neal manually compares
the isolated square.

VALIDATION

Run:

- focused Level 2 configuration tests;
- focused square geometry tests;
- focused render tests;
- focused lifecycle/reset tests;
- focused Level 1 Flipper/Tanker/Pulsar regression;
- focused Super Zapper/all-rim tests;
- full solution tests;
- solution build;
- git diff --check;
- git status --short.

Record exact pass/fail/skip counts and intermediate failures honestly.

MANUAL VALIDATION

Provide a B1-specific checklist.

Primary command:

dotnet run --project src/Tempest.Game -- --level2-test --lives 10

Optional bullet smoke check:

dotnet run --project src/Tempest.Game -- --level2-test --enemy-bullets-test --lives 10

Ask Neal to observe:

- square proportions;
- 16-lane presentation;
- Blaster path around sides/corners;
- any smoothness difference near corners;
- Egg/shot projection;
- Flipper climb;
- Flipper rim pivots on straight sections;
- Flipper rim pivots at corners;
- whether transitions visibly snap, swing incorrectly, cross the center, or
  land on the wrong lane;
- direct rim collision and completed-pivot safety;
- Super Zapper;
- all-rim exit;
- death/re-entry;
- R replay remains square Level 2.

Explicitly remind Neal:

B1 does NOT contain normal Level 2 mid-tube canflip behavior yet.

Do not ask Neal to validate `canflip` in B1.

REPORTING

Save this exact prompt to:

AI-Logs/Prompts/2026-10-30-level2-square-b1-implementation-prompt.md

Save report to:

AI-Logs/Reports/2026-10-30-level2-square-b1-implementation-report.md

Report:

1. purpose/start checkpoint;
2. isolated Level 2 configuration;
3. launch/flag behavior;
4. square hinge geometry design;
5. Level 1 compatibility;
6. renderer changes;
7. accounting/lifecycle;
8. Super Zapper/all-rim preservation;
9. files changed;
10. tests added/updated;
11. focused/full/build/diff/status results;
12. intermediate failures;
13. manual validation checklist;
14. explicit B1 limitations;
15. B2 relationship;
16. remaining geometry/arcade uncertainties.

STOP after B1 implementation, automated validation, documentation updates and
report generation.

Do not commit.
Do not modify TEMPEST-BEHAVIOR-SPEC.md.
Do not implement canflip.
Do not implement natural Level 1 -> Level 2 progression.