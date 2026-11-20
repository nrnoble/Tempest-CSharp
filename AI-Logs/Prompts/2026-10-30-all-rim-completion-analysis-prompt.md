Analyze the standalone all-rim completion milestone for Tempest.

Repository:
C:\Users\nrnob\OneDrive\Repro\Tempest

Starting clean checkpoint:
7f6c776 Implement Super Zapper and correct Tanker cargo behavior

ANALYSIS ONLY.

Do not modify gameplay code, tests, README, TEMPEST-DEVELOPMENT-NOTES.md,
TEMPEST-BEHAVIOR-SPEC.md, project/configuration files, or Git state.
Do not implement all-rim completion yet.
Do not commit.

FIRST READ

- TEMPEST-BEHAVIOR-SPEC.md
- README.md
- TEMPEST-DEVELOPMENT-NOTES.md
- current C# gameplay/accounting/exit code
- tests/Tempest.Game.Tests/SimulationTests.cs
- tests/Tempest.Game.Tests/SuperZapperTests.cs
- Reference/incitti/tempest.bmx
- relevant current AI-Logs reports

Especially inspect:

AI-Logs/Reports/2026-10-30-rimit-hurry-up-implementation-report.md
AI-Logs/Reports/2026-10-30-super-zapper-implementation-report.md
AI-Logs/Reports/2026-10-30-post-super-zapper-arcade-findings-analysis.md
AI-Logs/Reports/2026-10-30-tanker-cargo-direction-correction-report.md

Treat checkpoint 7f6c776 and the current behavior specification as authoritative
project state.

EVIDENCE

Maintain:

ARCADE VERIFIED
REFERENCE VERIFIED
PROJECT DECISION
DEVELOPMENT ONLY

Neal's direct Atari observations outrank Incitti where they disagree.

ARCADE OBSERVATION TO IMPLEMENT

Neal has established:

- once reserve/incoming Eggs are exhausted;
- and all remaining enemies have reached the top rim;
- the player does not need to kill every remaining rim enemy;
- those remaining rim enemies disappear from play;
- the Blaster proceeds into the tube-exit sequence.

This broad behavior is ARCADE VERIFIED.

Neal has NOT established by observation:
- the exact numeric height predicate;
- exact update ordering;
- enemy-specific animation states that qualify;
- score/accounting internals;
- interactions with pending zap timers;
- exact timing relative to collision checks.

Use Incitti and current architecture to determine those details conservatively.

INCITTI ANALYSIS

Trace the complete literal source path involving:

- onrim initialization/reset;
- calculation/recalculation of onrim;
- eggsleft;
- EGG_LIST;
- enemiesleft;
- BADDIE_LIST;
- enemy Height/height checks;
- qualifying/non-qualifying baddie types;
- bullets;
- spikes;
- Tankers;
- released Tanker cargo;
- Fuseballs;
- Pulsars;
- Spikers/Spinners;
- Flippers;
- rimit interaction;
- the branch choosing rimit versus level exit;
- ClearBaddies or equivalent cleanup;
- score consequences;
- level state transition;
- spike avoidance / reverse zoom;
- player collision ordering;
- death ordering.

Establish whether literal Incitti:
- clears remaining rim enemies without score;
- decrements or simply discards enemiesleft;
- treats bullets separately;
- retains spikes for exit hazard;
- can enter all-rim exit while zap-delayed enemies are present;
- has any equivalent of C# Tanker latent/materialized cargo.

Do not assume Incitti's accounting model can be copied directly.

CURRENT C# COMPLETION MODEL

Map the complete checkpoint-7f6c776 completion architecture.

Identify every relevant concept:

- Level.EnemyTotal
- Level.EnemiesDestroyed
- reserve Eggs
- active Eggs
- active enemy lists
- developmentCollisionOrder
- standard Level 1 completion
- Spiker scenario completion
- Tanker original-slot ledger
- Tanker escaped/destroyed states
- latent cargo
- materialized cargo IDs
- unresolved cargo
- Fuseball scenario
- Pulsar scenario
- HurryUp
- pending Super Zapper delays
- stillborn cargo
- EnemyBullets
- spikes
- ExitDelay
- tube exit
- spike avoidance
- reverse/retry exit
- death/reconstruction
- Game Over
- replay.

Determine exactly which existing code currently treats "resolved" as synonymous
with "killed."

DESIGN GOAL

Design a non-kill rim-clear resolution path.

The design must distinguish at least:

1. destroyed/scored enemy;
2. naturally escaped/resolved enemy where already modeled;
3. removed because the arcade all-rim exit condition became true.

Do not award kill score for an enemy removed solely by all-rim completion.

Do not falsely increment a "destroyed" count merely to make existing completion
predicates pass unless that field's semantics are explicitly broadened and
renamed.

Prefer preserving semantic accuracy over forcing all-rim resolution through an
existing kill API.

QUALIFYING RIM CONDITION

Determine the safest predicate for current C#.

Analyze each currently supported active type:

FLIPPER
- climbing arrival at depth;
- pausing at rim;
- flipping at rim;
- Tanker cargo below rim;
- Tanker cargo at rim.

TANKER
- normally releases before full depth;
- latent cargo consequences;
- whether an active Tanker can ever qualify as on-rim under current tuning.

SPIKER
- build/retreat behavior;
- relation between enemy height and persistent spike;
- whether a Spiker at qualifying height can vanish while its spike remains.

FUSEBALL
- lateral/climb/descent states;
- exact meaning of Height == Level.Depth;
- whether lateral-at-rim qualifies regardless of lateral position.

PULSAR
- climbing/descent/pause/flip states;
- state with Height == Level.Depth;
- pulsing hazard;
- whether pulse phase/state changes qualification.

SUPER ZAPPER PENDING TARGET
- whether a pending zap target at the rim still counts as a rim survivor;
- what happens to its pending score if all-rim exit wins first;
- no delayed zap score should occur after it has been non-kill-cleared.

ENEMY BULLET
- must not block all-rim completion if source/arcade evidence says projectiles
  are not enemies;
- determine exit cleanup timing.

SPIKES
- must not count as enemies;
- preserve existing spike-exit avoidance behavior where applicable.

EGG EXHAUSTION

Define precisely what "incoming Eggs exhausted" means using current C# state.

At minimum analyze:
- reserve Egg count;
- active/roaming/approaching Eggs;
- Tanker original-slot Egg queues;
- reconstructed Eggs after life loss;
- pending cargo that is not represented as an Egg.

All-rim completion must not trigger while a genuine unresolved incoming unit
remains below the gameplay surface merely because one particular Egg collection
is empty.

TANKER ACCOUNTING — HIGHEST RISK

Design exact all-rim behavior for:

A. unresolved Tanker before cargo release;
B. naturally released Tanker + two live cargo Flippers;
C. shot-destroyed Tanker + two live cargo Flippers;
D. Super-Zapper-destroyed Tanker + two pending stillborn children;
E. one cargo child already resolved, one still unresolved;
F. death/re-entry reconstructed cargo;
G. cargo at rim versus cargo below rim.

Requirements:

- no IDs duplicated or lost;
- no kill score for all-rim removal;
- resolved original Tanker slots remain resolved;
- latent cargo cannot silently disappear if its carrier situation should make
  the rim predicate false;
- materialized cargo IDs must be resolved through a non-kill completion outcome
  if all-rim exit consumes them;
- accounting invariants remain meaningful;
- replay resets cleanly.

Determine whether the current ledger needs:
- a new "rim escaped" state/set;
- broadening of existing escaped semantics;
- or another minimal representation.

Do not use a fake destroyed state if it would corrupt future statistics or
behavior-spec semantics.

SUPER ZAPPER INTERACTION

Analyze:

1. all-rim condition becomes true while enemies carry pending zap timers;
2. first/all Super Zapper used shortly before rim exit;
3. second/single zap targets final rim survivor;
4. Tanker stillborn cargo timers pending;
5. flash still active when exit begins.

Recommend deterministic precedence.

General behavioral requirement:
- enemies removed because all-rim completion won must not later award delayed
  Super Zapper score;
- disappearing rim enemies should not be converted into kills simply because
  they were pending zap targets.

Determine whether ongoing tube flash should continue during exit according to
current presentation policy/source evidence.

HURRY-UP INTERACTION

The current C# HurryUp trigger was deliberately implemented without onrim exit.

Design the new relationship:

- if Egg sources are exhausted and at least one enemy is below rim:
  HurryUp may latch/use its existing behavior;
- if Egg sources are exhausted and every qualifying survivor is at rim:
  all-rim completion should win instead of merely leaving HurryUp latched.

Determine exact evaluation ordering using the current simulation tick.

Preserve previously validated Fuseball/Pulsar hurry-up behavior below the rim.

PLAYER COLLISION ORDER

Determine whether a last enemy reaching the player's occupied rim lane on the
same tick should:
- kill the player first;
- cause all-rim exit first;
- depend on enemy type/state.

Use literal source ordering and existing arcade observations to recommend the
behavior.

Do not invent an answer silently if evidence is ambiguous.
Flag any ambiguity for manual validation.

EXIT AND SPIKES

Preserve existing spike exit behavior.

Analyze:

- surviving Spiker enemy removed because all-rim condition qualifies;
- its persistent spike remains;
- Blaster begins exit;
- spike collision can trigger existing reverse/retry behavior;
- retry must not reconstruct rim-cleared enemies if the wave has already
  transitioned into the exit phase unless current architecture/source requires it.

Determine whether the non-kill all-rim resolution should be permanent at the
moment exit starts.

SCORING AND HUD

Determine what counts should display after rim-clear transition.

No rim-clear disappearance should award ordinary enemy kill score.

If HUD currently derives "remaining" only from destroyed counts, identify the
smallest semantic correction needed.

Do not redesign the HUD unless necessary.

TEST PLAN

Design deterministic tests covering at minimum:

STANDARD LEVEL 1
- reserve remains -> no all-rim exit;
- active Egg remains -> no all-rim exit;
- below-rim Flipper remains -> no exit;
- final survivor reaches qualifying rim -> exit begins;
- multiple rim Flippers -> all vanish without kill score;
- destroyed count is not falsely credited;
- ordinary kill-all completion still works.

HURRY-UP
- below-rim Fuseball/Pulsar with exhausted Eggs -> hurry-up rather than exit;
- final below-rim survivor reaches rim -> all-rim completion wins;
- no regression to hurry-up timing below rim.

TANKER
- unresolved carrier/latent cargo cannot disappear incorrectly;
- live materialized cargo below rim blocks exit;
- all materialized cargo at rim permits exit;
- rim-clear resolves cargo IDs non-kill;
- mixed destroyed/escaped/rim-cleared ledger remains conserved;
- repeated replay/reset clean.

SUPER ZAPPER
- pending rim target cleared by all-rim exit gets no delayed zap score;
- pending below-rim target blocks all-rim exit;
- stillborn cargo below rim blocks exit;
- all stillborn cargo at rim permits correct non-kill completion;
- no later timer resolution/double score after exit.

SPIKER/SPIKES
- Spiker rim qualification;
- persistent spike remains;
- exit still enters spike avoidance;
- spike collision reverse/retry retains resolved all-rim state appropriately.

BULLETS
- active enemy bullets do not block all-rim completion if that matches source;
- exit cleanup remains correct.

DEATH BOUNDARY
- player death immediately before qualification;
- reconstruction retains unresolved units correctly;
- no all-rim resolution applied while PlayerDying.

NO SCORE
- every enemy type cleared through rim exit awards zero additional kill points.

REGRESSION
- all 325 checkpoint tests remain meaningful or are deliberately updated where
  they encode the obsolete kill-all-only PROJECT DECISION.

IDENTIFY OBSOLETE TESTS

List every existing test whose expected behavior should intentionally change.

Do not silently rewrite tests simply to make implementation pass.

BEHAVIOR SPECIFICATION

Identify precise sections of TEMPEST-BEHAVIOR-SPEC.md that should be updated
AFTER implementation and Neal's manual validation.

Do not edit the specification during analysis.

MAINTAINABILITY / ARCHITECTURE

Conclude whether all-rim completion can be implemented locally with:
- one explicit qualification function;
- one explicit non-kill resolution path;
- minimal Tanker ledger extension;
- existing exit state machine;

or whether a larger accounting refactor is genuinely necessary.

Avoid speculative generalization for future progression unless required to make
the current behavior semantically correct.

OUTPUT

Produce an implementation-ready analysis containing:

1. literal Incitti behavior;
2. Neal's arcade observation/evidence boundary;
3. current C# behavior;
4. exact qualifying condition recommendation;
5. exact evaluation/update ordering;
6. non-kill resolution/accounting design;
7. per-enemy treatment;
8. Tanker/cargo ledger design;
9. Super Zapper interaction;
10. Hurry-Up interaction;
11. player collision precedence;
12. spikes/exit/retry behavior;
13. scoring/HUD impact;
14. test plan;
15. obsolete tests to update;
16. expected files for implementation;
17. behavior-spec impact;
18. uncertainties requiring manual validation;
19. explicit out-of-scope items.

Conclude with a GO / NO-GO recommendation for implementing all-rim completion
as the next standalone milestone.

Save this exact prompt to:

AI-Logs/Prompts/2026-10-30-all-rim-completion-analysis-prompt.md

Save the analysis to:

AI-Logs/Reports/2026-10-30-all-rim-completion-analysis-report.md

AI-Logs remains ignored.

STOP after analysis/report generation.

Do not implement.
Do not modify tracked files.
Do not commit.