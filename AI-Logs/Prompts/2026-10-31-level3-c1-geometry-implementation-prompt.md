Implement C1: isolated Level 3 plus-board geometry groundwork.

Repository:
C:\Users\nrnob\OneDrive\Repro\Tempest

Starting clean checkpoint:
f6ac233 Implement Level 1 to Level 2 progression

WORK AUTONOMOUSLY

Work autonomously through the task.

Do not pause for routine clarification when the answer can be determined safely
from:

- the repository;
- current behavior specification;
- approved analysis;
- tests;
- project documentation;
- literal reference source.

Document reasonable assumptions, discrepancies and uncertainties in the report.

Stop and request clarification only if proceeding requires an important
behavior or architectural decision that is not supported by available
evidence.

Do not expand scope while waiting for clarification.

FIRST READ

- AI-Logs/Reports/2026-10-31-level3-tanker-progression-analysis-report.md
- AI-Logs/Reports/2026-10-30-progression-architecture-analysis-report.md
- AI-Logs/Reports/2026-10-31-level1-to-level2-progression-implementation-report.md
- AI-Logs/Reports/2026-10-30-level2-square-canflip-analysis-report.md
- AI-Logs/Reports/2026-10-30-level2-square-b1-implementation-report.md
- AI-Logs/Reports/2026-10-31-level2-square-b2-implementation-report.md
- TEMPEST-BEHAVIOR-SPEC.md
- TEMPEST-DEVELOPMENT-NOTES.md
- README.md

Also inspect:

- current checkpoint-f6ac233 source and tests;
- literal Level 3 board row;
- Incitti Flipper endpoint rotation;
- current square endpoint-hinge implementation;
- Tanker cargo creation and rendering;
- relevant Super Zapper, all-rim, lifecycle and renderer code.

The Level 3 analysis report is the approved design authority.

If current source contradicts the report, investigate and document the
difference rather than forcing the report blindly.

Do not commit or push.

BASELINE MAINTENANCE

At the beginning, run and record:

git status --short
git log -1 --oneline
git diff --check
git status --short -- Reference/incitti

Verify:

- HEAD is f6ac233;
- working tree is clean;
- Reference/incitti has no tracked modifications.

Also record the baseline full-suite result and build result:

dotnet test TempestCSharp.slnx --no-restore
dotnet build TempestCSharp.slnx --no-restore

If either baseline command fails, diagnose the failure before editing.

PURPOSE

Implement only C1:

- source-derived displayed-Level-3 plus-board configuration;
- isolated DEVELOPMENT ONLY graphical geometry access;
- generalized endpoint-hinge geometry mode usable by the established Level 2
  square and the new Level 3 plus board;
- configuration-aware ordinary Flipper pivots;
- configuration-aware Tanker-cargo pivots;
- renderer and lifecycle validation.

Do NOT implement:

- natural Level 2 -> Level 3 progression;
- production Level 3 mixed composition;
- production Tanker accounting;
- per-Egg Flipper/Tanker selection;
- multiple production Tankers;
- Tanker firing changes;
- automatic Tanker bullets;
- Flipper bullet promotion;
- Level 4;
- open tubes;
- Spikers;
- Fuseballs;
- Pulsars;
- palette cycles;
- board rotation;
- bonuses;
- bonus lives;
- audio;
- between-level approach presentation;
- Blaster input/smoothness tuning.

Normal gameplay must remain Level 1 -> Level 2 -> terminal.

LEVEL 3 CONFIGURATION

Add one immutable Level 3 plus-board configuration containing:

- internal level identity: 2;
- displayed level: 3;
- board index: 2;
- palette index: 0;
- continuous/closed topology;
- 16 vertices;
- 16 playable lanes;
- center Y: 400;
- Y offset: -80;
- depth: 400;
- normal position: 50;
- player starting lane: 7;
- player fractional position: existing project value 0.5;
- configured original supply: 15;
- four initial Eggs;
- eleven reserve units;
- canflip enabled;
- Flipper tuning inherited from the established early-level configuration:
  climb 2, turn 8, below-rim pause 21, rim pause 24;
- exact Level 3 plus-board points.

Use these exact ordered points:

0  (  98,   98)
1  (  98,  197)
2  (   0,  197)
3  ( -98,  197)
4  ( -98,   98)
5  (-197,   98)
6  (-197,    0)
7  (-197,  -98)
8  ( -98,  -98)
9  ( -98, -197)
10 (   0, -197)
11 (  98, -197)
12 (  98,  -98)
13 ( 197,  -98)
14 ( 197,    0)
15 ( 197,   98)

Do not expose this as natural Level 3.

Do not create a duplicate future production configuration.

If a diagnostic marker is needed, keep it in launch/session policy rather than
copying the board configuration.

C1 does not claim the eventual production Level 3 Egg schedule or composition.

Use explicit C1 project settings only as needed to exercise geometry.

Do not claim existing deterministic Egg release behavior matches Incitti Level
3.

GEOMETRY MODE

Generalize the existing square-specific endpoint-hinge concept into a narrowly
named configuration geometry mode suitable for adjacent-edge endpoint pivots.

Examples of acceptable concepts:

- LegacyEdgeDirection
- EndpointHinge

Choose names consistent with the current code.

Requirements:

- Level 1 retains its established legacy geometry.
- Level 2 square uses the same validated endpoint-hinge behavior as before.
- Level 3 plus board uses endpoint-hinge behavior.
- Do not globally replace Level.CalculateEdgeTurnAngle.
- Do not alter existing Level 1 pivot values, timing or renderer output.
- Do not introduce separate square and plus renderer implementations.
- Do not add a broad geometry strategy framework.

For adjacent lanes:

- identify the shared vertex;
- keep the shared vertex as hinge;
- rotate the source free endpoint toward the destination free endpoint;
- straight-run endpoint rays produce 180 degrees;
- right-angle endpoint rays produce 90 degrees;
- sweep sign is deterministic;
- completion reaches the exact destination endpoint.

Enumerate all directed adjacent transitions.

Level 3 straight-run transitions are:

1 <-> 2
5 <-> 6
9 <-> 10
13 <-> 14

All other adjacent transitions, including 15 <-> 0, use 90-degree endpoint
hinges.

Do not use epsilon fixes.

Do not use midpoint interpolation.

Do not disable geometry validation to avoid exceptions.

TANKER-CARGO GEOMETRY

Correct the architectural bypass identified in the analysis.

Current Tanker cargo creation must not directly use the legacy
CalculateEdgeTurnAngle when the installed geometry requires endpoint hinges.

Route Tanker-cargo initial pivots through the same configuration-selected
Flipper transition-angle policy used by ordinary Flippers.

Preserve all currently validated cargo behavior:

- two live children survive a player-shot Tanker destruction;
- children occupy distinct non-Tanker lanes;
- deterministic outward route remains the PROJECT DECISION;
- cargo does not dynamically track the Blaster below the rim;
- cargo retains its assigned direction below rim;
- rim pursuit remains established;
- cargo identity/accounting is unchanged;
- delayed activation after shot release is unchanged;
- same shot cannot kill newly released cargo;
- Super Zapper stillborn cargo behavior is unchanged.

C1 must not generalize the Tanker ledger or create production Tankers.

CONFIGURATION / DIAGNOSTIC ACCESS

Add:

--level3-geometry-test

This is DEVELOPMENT ONLY.

It exists only to validate:

- plus-board rendering;
- rim movement;
- Eggs and shots;
- ordinary Flipper canflip/rim pivots;
- controlled Tanker and cargo rendering/pivots if needed;
- Super Zapper flash;
- finite projection.

Requirements:

- mutually exclusive with:
  --level2-test
  --spiker-test
  --tanker-test
  --fuseball-test
  --pulsar-test
- --lives remains composable;
- --enemy-bullets-test may remain separately composable if current flag policy
  allows it safely;
- normal no-flag campaign remains Level 1 -> Level 2 -> terminal;
- diagnostic completion is terminal;
- R replays the Level 3 geometry diagnostic;
- Game Over restart remains in the diagnostic;
- the UI clearly says geometry diagnostic or equivalent;
- do not call it a production Level 3 mixed wave;
- do not add natural Level 2 -> Level 3 progression.

Use the exact Level 3 configuration object.

Do not create Level3Geometry and Level3Production copies.

DIAGNOSTIC COMPOSITION

C1 must not pretend to implement authentic Level 3 composition.

Choose the smallest controlled setup needed for geometry validation.

It may:

- use Flippers only for ordinary plus-board movement; and
- expose one controlled Tanker/cargo geometry fixture only if required for
  manual C1 validation.

If one runtime scenario cannot safely expose both without introducing mixed-wave
accounting, prefer:

- plus-board ordinary Flippers in the graphical diagnostic; and
- deterministic headless Tanker-cargo plus-board geometry tests.

Do not reuse the fixed slot-0 Tanker development composition as “Level 3.”

Make the distinction explicit in UI/documentation.

Do not add a second new flag unless truly required.

ACCOUNTING / LIFECYCLE

Use the existing configured-supply accounting only for the geometry diagnostic.

The diagnostic can remain one-original-unit/one-Flipper accounting if that is
the safest honest C1 scope.

Do not add cargo obligations to a normal configured-supply wave unless the
existing Tanker development ledger owns that controlled fixture.

Natural production Level 3 accounting remains deferred.

Preserve:

- death reconstruction;
- score/lives;
- Super Zapper charges;
- all-rim completion;
- fresh replay;
- Game Over;
- projection reset;
- callback cleanup;
- shot/bullet cleanup.

No Level 3 production identity ledger is part of C1.

RENDERER

Use the current renderer architecture.

Validate:

- plus near rim;
- plus far rim;
- 16 rails;
- closed wrap;
- player around concave and convex turns;
- Eggs;
- shots;
- climbing Flippers;
- mid-tube canflip;
- rim pivots;
- optional controlled Tanker glyph;
- controlled cargo pivots;
- flash recoloring;
- finite coordinates.

Do not introduce a new renderer.

Do not mutate global geometry as a workaround.

Do not add transition animation.

PLAYER / COLLISION

Preserve existing behavior:

- no below-rim Flipper transition death;
- direct rim arrival collision;
- completed-pivot collision;
- shot-before-player-collision ordering;
- two-lane shot acceptance during active C# Flipper pivots;
- Level 2 B1/B2 canonical non-pivot state after completion.

C1 does not retune collision policy.

SUPER ZAPPER / ALL-RIM

For the diagnostic:

- preserve two charges;
- preserve fresh replay recharge;
- preserve ordinary life-loss charge persistence;
- preserve flash behavior;
- preserve all-rim completion for the diagnostic's controlled active entities.

Do not add production multi-Tanker zap/accounting behavior.

Testing a controlled Tanker cargo object directly is acceptable; claiming
multi-carrier production support is not.

LEVEL 1 / LEVEL 2 REGRESSION

Mandatory preservation:

- Level 1 legacy pivot geometry and timing;
- Level 1 normal behavior;
- Level 1 -> Level 2 progression;
- Level 2 square/endpoint-hinge geometry;
- Level 2 canflip;
- natural Level 2 terminal policy;
- direct --level2-test behavior;
- existing --tanker-test cargo behavior;
- Spiker/Fuseball/Pulsar scenarios;
- existing bullet policies.

No current Behavior Specification claim should become false.

TESTS

Add deterministic C1 tests.

BASELINE:
- confirm clean f6ac233 baseline;
- record actual baseline full-suite/build results.

CONFIGURATION:
- identity internal 2/display 3;
- board 2/palette 0;
- exact 16 ordered plus-board points;
- continuous true;
- vertices/playable lanes 16/16;
- CenterY 400;
- YOffset -80;
- Depth 400;
- Position 50;
- player lane 7 / fractional position 0.5;
- supply 15;
- four initial Eggs / eleven reserve;
- endpoint-hinge mode;
- canflip enabled;
- no production mixed-wave claim.

FLAGS / LIFECYCLE:
- --level3-geometry-test selects the plus board;
- conflicts reject deterministically;
- --lives composes;
- optional bullets compose only if approved by existing policy;
- normal launch remains Level 1;
- normal Level 2 stays terminal;
- diagnostic replay stays plus board;
- diagnostic Game Over restart stays plus board;
- life reconstruction retains plus-board runtime/configuration.

GEOMETRY:
- all 32 directed adjacent lane transitions;
- four straight-run pairs -> 180 degrees;
- other pairs -> 90 degrees;
- 15 <-> 0 wrap;
- deterministic sign;
- source/destination endpoint completion;
- start/intermediate/end finite;
- no zero vector;
- no zero-turn division;
- no NaN/infinity.

ORDINARY FLIPPER:
- plus-board mid-tube canflip on straight run;
- plus-board corner canflip;
- rim pivot;
- no active retargeting;
- destination completion;
- player collision policies preserved.

TANKER CARGO:
- CreateTankerCargo uses installed geometry policy;
- Level 3 straight-run cargo pivot is 180, not zero;
- Level 3 corner cargo pivot is 90;
- wrap works;
- outward route preserved;
- fixed below-rim direction preserved;
- no changed identity/accounting;
- current Level 1 tanker diagnostic remains unchanged;
- Level 2 cargo geometry behavior, where exercised, remains unchanged.

RENDER:
- exact 16 rails;
- 16 near-rim segments;
- 16 far-rim segments;
- player on all lanes;
- Egg/shot/Flipper/Tanker finite projection;
- straight/corner pivot captures;
- destination endpoint reached;
- flash color does not alter geometry.

ACCOUNTING / VALIDATION:
- diagnostic configured supply remains coherent;
- death/replay/restart;
- Super Zapper lifecycle;
- all-rim completion;
- no production cargo ledger introduced.

REGRESSION:
- all 409 checkpoint tests remain meaningful;
- Level 1 geometry output pinned;
- Level 2 B1/B2 geometry output pinned;
- natural progression remains only Level 1 -> Level 2;
- Level 3 remains unreachable naturally;
- existing development scenarios unchanged.

MAINTAINABILITY

Add focused comments explaining:

- endpoint-hinge mode versus legacy Level 1 geometry;
- why the plus board requires endpoint pivots on straight runs;
- why Tanker cargo must use configuration-selected transition geometry;
- why --level3-geometry-test is not production Level 3.

Avoid comments that restate code.

DOCUMENTATION

After implementation and automated validation, update only:

- README.md
- TEMPEST-DEVELOPMENT-NOTES.md

Clearly state:

- --level3-geometry-test is DEVELOPMENT ONLY;
- it exposes the source-derived plus board for geometry validation;
- it is not a production Level 3 mixed wave;
- natural campaign still stops after Level 2;
- Tanker mixed accounting and Tanker firing remain deferred;
- Level 1 and Level 2 remain unchanged.

Do not update TEMPEST-BEHAVIOR-SPEC.md during implementation.

Wait for Neal's manual geometry validation before adding any behavioral history,
and do not promote Incitti board values to arcade-verified merely because the
diagnostic renders correctly.

VALIDATION

During implementation, use focused tests as appropriate.

At final validation run:

dotnet test TempestCSharp.slnx --no-restore
dotnet build TempestCSharp.slnx --no-restore
git diff --check
git status --short
git diff --stat
git status --short -- Reference/incitti

Record:

- actual focused-test counts;
- actual full-suite total;
- build warnings/errors;
- intermediate failures;
- every modified/untracked file;
- confirmation that Reference/incitti is unchanged.

Do not rerun successful final commands repeatedly without a concrete reason.

MANUAL VALIDATION CHECKLIST

Primary:

dotnet run --project src/Tempest.Game -- --level3-geometry-test --lives 10

Ask Neal to observe:

- plus-board outline and proportions;
- near/far alignment;
- 16-lane presentation;
- Blaster rim path around concave and convex turns;
- Egg and shot alignment;
- ordinary Flipper climb;
- mid-tube straight-run transitions;
- mid-tube corner transitions;
- rim pivots;
- wrap transition;
- no snapping;
- no wrong-lane landing;
- no center-crossing or excessively strange arc;
- no below-rim collision regression;
- Super Zapper flash;
- all-rim exit;
- death/re-entry;
- R replay remains the plus-board diagnostic;
- no crash or non-finite rendering.

If controlled Tanker/cargo is not graphically exposed in the single diagnostic,
state clearly that cargo geometry is automated-only in C1 and will be manually
validated in a later direct production Level 3 checkpoint.

Do not ask Neal to validate production composition, multiple Tankers, Tanker
firing, or Level 2 -> Level 3 progression in C1.

REPORTING

Save this exact prompt to:

AI-Logs/Prompts/2026-10-31-level3-c1-geometry-implementation-prompt.md

Save the implementation report to:

AI-Logs/Reports/2026-10-31-level3-c1-geometry-implementation-report.md

Report:

1. baseline Git/build/test result;
2. purpose and start checkpoint;
3. evidence boundary;
4. Level 3 configuration;
5. diagnostic launch policy;
6. endpoint-hinge generalization;
7. Tanker-cargo geometry correction;
8. renderer/lifecycle behavior;
9. Level 1/2 preservation;
10. files changed;
11. tests added/updated;
12. focused/full/build/diff results;
13. intermediate failures;
14. manual checklist;
15. explicit C1 limitations;
16. C2/C3/C4 relationship;
17. remaining geometry/arcade uncertainties;
18. final Git/maintenance result.

STOP after implementation, automated validation, documentation updates, report
generation and final maintenance checks.

Do not commit or push.
Do not modify TEMPEST-BEHAVIOR-SPEC.md.
Do not implement production Level 3 composition.
Do not implement Tanker firing changes.
Do not implement natural Level 2 -> Level 3 progression.