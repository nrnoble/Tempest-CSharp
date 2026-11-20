Implement the approved all-rim completion milestone.

Repository:
C:\Users\nrnob\OneDrive\Repro\Tempest

Starting clean checkpoint:
7f6c776 Implement Super Zapper and correct Tanker cargo behavior

FIRST read:

AI-Logs/Reports/2026-10-30-all-rim-completion-analysis-report.md
TEMPEST-BEHAVIOR-SPEC.md
README.md
TEMPEST-DEVELOPMENT-NOTES.md

Also inspect current checkpoint-7f6c776 code, tests and literal Incitti source
before editing.

The analysis report is the approved implementation design.

If current code or literal source contradicts the report, investigate the
difference and document it rather than silently forcing the report's design.

Do not commit.

EVIDENCE

Preserve:

ARCADE VERIFIED
REFERENCE VERIFIED
PROJECT DECISION
DEVELOPMENT ONLY

Neal's arcade observation establishes the broad all-rim completion behavior.

The exact numeric threshold, exact tick ordering, hidden accounting and
unobserved interactions remain source-derived/project decisions until
separately arcade-validated.

ARCADE BEHAVIOR

Implement:

- once reserve/incoming Eggs are exhausted;
- and all remaining qualifying enemies have reached the top rim;
- the player need not kill those remaining enemies;
- those enemies disappear from gameplay;
- they award no kill score merely for disappearing;
- the Blaster begins the existing tube-exit sequence.

Do not treat rim-cleared enemies as kills.

NORMAL ACCOUNTING

Add an explicit non-kill rim-clear disposition.

For normal Level accounting:

- preserve EnemiesDestroyed as kill-scored destruction only;
- add a distinct rim-cleared count/state;
- redefine semantic remaining/unresolved count as:

  EnemyTotal - EnemiesDestroyed - EnemiesRimCleared

- preserve the existing ten-unit conservation model;
- update reconstruction/spawn rewind accounting so rim-cleared units are not
  later reconstructed;
- allow completion with destroyed + rim-cleared units when reserve Eggs,
  active Eggs and active enemies are empty.

Do not rename or broaden EnemiesDestroyed so that it includes rim clears.

No rim-clear path may:
- award Score;
- create Fuseball score popups;
- call kill-score handlers;
- increment EnemiesDestroyed.

QUALIFICATION

Create one explicit all-rim qualification function/predicate.

Evaluate only while Playing.

Require:

- CurrentLevel.ReserveEggs == 0;
- Eggs.Count == 0;
- PendingTankerCargoEggs == 0;
- no unresolved latent/materialized Tanker cargo source exists outside the
  active enemy representation;
- at least one active enemy exists;
- every qualifying active enemy has post-update Height >= Level.Depth.

Qualifying active enemy lists:

- Flippers
- Tankers
- Spikers
- Fuseballs
- Pulsars

Do not include:

- player shots
- EnemyBullets
- Spike hazards

Use >= Level.Depth, not equality.

This exact numeric/state-agnostic qualification is REFERENCE VERIFIED /
PROJECT DECISION, not an arcade-verified numeric claim.

Fail closed if Tanker accounting says an unresolved unit exists but the unit is
not represented by an active enemy, reserve/Egg source, latent carrier cargo,
pending cargo Egg or other explicit unresolved representation.

UPDATE ORDER

Preserve the existing early kill-all check after shots/bullets.

Do NOT evaluate all-rim there.

Use the final active-game update path:

1. existing player/input/zap/shot/bullet processing;
2. existing early kill-all completion;
3. Egg release/update/hatching;
4. active enemy and zap-timer updates;
5. pending cargo activation;
6. existing player/enemy/bullet/spike collision checks;
7. if still Playing, evaluate and commit all-rim completion;
8. if all-rim did not qualify, run the existing Hurry-Up latch logic;
9. run the existing final kill-all completion check.

This ordering is important.

A final survivor reaching Level.Depth on its enemy update may qualify that tick.

A ready Egg hatches first, and its newly active below-rim enemy blocks exit.

If player collision changes State to PlayerDying first, all-rim completion must
not run.

Do not change the existing player-shot-before-enemy-collision rule.

ALL-RIM COMMIT

When qualification succeeds:

- classify every remaining active qualifying enemy as rim-cleared;
- remove it from its owning active collection;
- remove it from developmentCollisionOrder where applicable;
- cancel/discard any pending zap timer with that enemy object;
- award no score;
- create no enemy kill popup;
- preserve already-earned score;
- clear player shots and enemy bullets through the established exit-start path;
- preserve Spike hazards;
- enter the existing exit path:
  - Spiker development scenario -> existing AvoidSpikes path;
  - otherwise -> existing ExitDelay path.

Make the rim-clear resolution idempotent.

Repeated checks must not double-count or reclassify something already cleared.

PER-ENEMY RULES

Flipper:
- any Flipper with post-update Height >= Level.Depth qualifies regardless of
  climbing/pause/flipping state;
- existing player collision runs first.

Tanker:
- a normal active carrier below Level.Depth blocks;
- latent cargo blocks implicitly/explicitly with its unresolved carrier;
- do not silently drop latent cargo.

Spiker:
- natural current Spiker behavior below the threshold continues to block;
- retreat/return-to-reserve remains unchanged;
- a qualifying Spiker rim-clear removes the enemy but retains its Spike hazard.

Fuseball:
- qualification is based on post-update Height >= Level.Depth;
- lateral W/action does not alter the implemented qualification;
- existing Fuseball player collision happens first.

Pulsar:
- qualification is based on post-update Height >= Level.Depth;
- pause/flip/pulse state does not alter qualification;
- pulse/rim player collision happens first.

EnemyBullet:
- does not block qualification;
- same-tick player bullet collision still occurs before completion;
- existing exit cleanup clears bullets.

Spike:
- does not block qualification;
- remains for spike-exit hazard behavior.

TANKER LEDGER

Extend the Tanker ledger semantically rather than faking destruction.

Add distinct rim-clear outcome sets/state for:

- original Tanker slots where applicable;
- materialized cargo IDs.

Original slot outcomes must remain partitioned:

- unresolved
- destroyed
- naturally escaped
- rim-cleared

Materialized cargo outcomes must remain partitioned:

- unresolved
- killed/resolved
- rim-cleared

Do not include a rim-cleared ID in a destroyed/killed set.

Preserve TankerAccountingTotal conservation.

Reset/replay clears new rim-clear sets.

Cases:

A. Carrier alive with latent cargo:
   must block all-rim exit while below rim.

B. Natural carrier release:
   original slot remains escaped;
   live cargo individually blocks/qualifies.

C. Shot-destroyed carrier:
   original slot remains destroyed and its 200 points remain;
   live cargo can later rim-clear for zero points.

D. Super-Zapper-destroyed carrier:
   carrier remains destroyed and keeps its 200 points;
   stillborn children below rim block;
   pending children at qualifying rim height can rim-clear without their
   delayed 150-point scores.

E. One child already killed:
   preserve its score/outcome;
   only unresolved sibling participates in later rim clear.

F. Death before qualification:
   preserve existing reconstruction of unresolved identities;
   do not restore destroyed/escaped/rim-cleared IDs.

SUPER ZAPPER

Pending target remains an active enemy.

- pending enemy below rim blocks all-rim exit;
- pending enemy at qualifying rim height may be rim-cleared;
- rim-cleared pending target receives no later zap score;
- no later delayed resolution may occur for its removed object.

Zap expiry earlier on the same enemy-update tick wins normally:
- score once;
- resolve once;
- then the final all-rim check sees the resulting state.

For Tanker stillborn cargo:
- preserve IDs/timers until ordinary resolution or rim clear;
- rim-clear pending child for zero child score;
- never double resolve or later award 150.

Do not reset or alter Super Zapper flash merely because all-rim completion
started.

Preserve current flash advancement through exit states.

HURRY-UP

Preserve existing Hurry-Up below the rim.

If Egg sources are exhausted and at least one Fuseball/Pulsar remains below
rim:
- all-rim predicate is false;
- existing Hurry-Up behavior continues unchanged.

If the final below-rim survivor reaches the qualifying rim height:
- all-rim completion wins before a new Hurry-Up latch decision.

If Hurry-Up was already latched:
- preserve its value/lifecycle;
- it does not veto all-rim completion.

PLAYER COLLISION PRECEDENCE

Preserve:

enemy updates
-> player/enemy, bullet and spike collision checks
-> all-rim qualification

If a collision changes State to PlayerDying:
- do not commit all-rim completion that tick.

This exact same-tick behavior remains a manual-validation item; do not claim
arcade equivalence.

SPIKER EXIT / REVERSAL

Preserve Spike hazards during all-rim exit.

When all-rim qualification commits in Spiker mode:

- rim-clear enemy objects permanently;
- leave Spike objects;
- enter existing AvoidSpikes path.

If the Blaster later hits a Spike and the existing reverse/retry behavior runs:

- do not reconstruct rim-cleared enemy units;
- preserve rim-clear accounting;
- retry the exit/spike sequence with the cleared wave;
- ReturnSurvivorsToReserve must not resurrect rim-cleared units.

This is a PROJECT DECISION where arcade behavior is unobserved.

HUD

Keep EnemiesDestroyed semantically kill-only.

For normal HUD remaining count, use the new semantic remaining/unresolved value
rather than EnemyTotal - EnemiesDestroyed.

After all-rim commitment, remaining should be zero without pretending every
enemy was killed.

For Tanker mode, make existing ORIGINAL/CARGO remaining counts respect
rim-cleared outcomes.

Do not redesign the HUD.

TESTS

Implement the deterministic test plan from:

AI-Logs/Reports/2026-10-30-all-rim-completion-analysis-report.md

At minimum cover:

STANDARD
- reserve Eggs block;
- active Egg blocks;
- below-rim Flipper blocks;
- final Flipper reaching rim starts exit;
- multiple rim Flippers all non-kill-clear;
- no added score;
- EnemiesDestroyed unchanged;
- rim-clear count correct;
- repeated checks idempotent;
- ordinary kill-all still works.

HURRY-UP
- below-rim Fuseball and Pulsar cause Hurry-Up rather than exit;
- final survivor reaching rim causes all-rim exit;
- already-latched Hurry-Up does not block exit.

ENEMY STATES
- Flipper rim pause/flip;
- Fuseball lateral/climb/descent at rim;
- Pulsar pause/flip/pulse/rim state;
- natural Spiker below threshold blocks;
- qualifying forced Spiker at rim clears while Spike remains.

BULLETS
- active enemy bullet does not block;
- same-tick bullet player collision causes death first.

TANKER
- carrier/latent cargo blocks;
- queued original/cargo Eggs block;
- natural release original remains escaped;
- shot/zap carrier original remains destroyed;
- live cargo below rim blocks;
- all live cargo at rim rim-clears IDs;
- killed + rim-cleared mixed cargo outcomes preserve score/accounting;
- stillborn pending cargo at rim clears with no delayed child score;
- death/re-entry restores only unresolved IDs;
- replay/full reset resets rim-clear sets.

SUPER ZAPPER
- pending rim target clears non-kill;
- pending below-rim target blocks;
- timer expiry before final check scores normally;
- no double score;
- no delayed score after rim clear;
- flash continues through exit.

SPIKES
- persistent Spike survives enemy rim clear;
- existing spike exit/avoidance remains;
- reverse/retry does not resurrect rim-cleared enemies.

DEATH
- PlayerDying prevents all-rim commit;
- reconstruction remains correct.

NO SCORE
- per enemy type, sole rim-clear resolution awards zero additional kill
  points/popups.

OBSOLETE TESTS

Explicitly update the existing expectations identified in the analysis report:

- HurryUpDoesNotActivateWhenAllActiveEnemiesAreAtTheRim
- HurryUpTriggerUsesPostUpdateHeightNotThePreUpdateBelowRimHeight
- HurryUpRemainsLatchedAtTheRimWithoutResolvingTheLastEnemy

Rename/reframe tests where needed so names describe the new intended behavior.

Do not weaken unrelated kill-route, completion or accounting tests.

If deterministic full-wave tests begin naturally taking the all-rim path,
separate explicit kill-route and rim-clear-route tests rather than removing
kill/accounting assertions.

MAINTAINABILITY

Add focused comments explaining:

- why rim clear is not destruction;
- why player collision precedes all-rim qualification;
- why Tanker identities need distinct rim-clear dispositions;
- why a pending zap loses its future score when rim-cleared;
- why Spike reverse/retry does not recreate rim-cleared enemies.

Avoid obvious comments.

DOCUMENTATION

After automated validation, update:

- README.md
- TEMPEST-DEVELOPMENT-NOTES.md

Do NOT update TEMPEST-BEHAVIOR-SPEC.md yet.

That specification waits for Neal's manual validation.

VALIDATION

Run:

- focused all-rim completion tests;
- focused Hurry-Up tests;
- focused Tanker/accounting tests;
- focused Super Zapper tests;
- focused Spiker/exit tests;
- full solution test suite;
- solution build;
- git diff --check.

Record exact pass/fail/skip counts and intermediate failures honestly.

MANUAL VALIDATION REPORT

Provide practical manual commands/checklist, prioritizing visually observable
behavior rather than source timing constants.

Ensure the checklist includes:

- normal Level 1 all-rim clear;
- no kill score for disappearing rim enemies;
- below-rim survivor prevents exit;
- Fuseball/Pulsar Hurry-Up then rim completion;
- Tanker cargo below rim blocks and all-rim cargo eventually clears;
- Super Zapper pending interaction where practical;
- Spiker Spike hazards remain during exit;
- same-tick player collision uncertainty where practical to observe.

REPORTING

Save this exact prompt to:

AI-Logs/Prompts/2026-10-30-all-rim-completion-implementation-prompt.md

Save implementation report to:

AI-Logs/Reports/2026-10-30-all-rim-completion-implementation-report.md

Report sections:

1. implemented arcade behavior/evidence boundary;
2. exact implemented qualification;
3. evaluation ordering;
4. normal non-kill accounting;
5. Tanker ledger changes;
6. per-enemy treatment;
7. Super Zapper interaction;
8. Hurry-Up interaction;
9. collision precedence;
10. spikes/exit/retry;
11. scoring/HUD;
12. files changed;
13. tests changed/added;
14. obsolete expectations intentionally replaced;
15. focused/full/build/diff validation results;
16. manual checklist;
17. explicit out-of-scope items;
18. remaining arcade uncertainties.

STOP after implementation, automated validation, README/development-note
updates and report generation.

Do not commit.
Do not update TEMPEST-BEHAVIOR-SPEC.md.
Do not begin progression or another milestone.