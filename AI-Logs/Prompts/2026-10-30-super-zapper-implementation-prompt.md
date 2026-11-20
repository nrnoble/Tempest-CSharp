Implement the complete Super Zapper milestone for Tempest.

Repository:
C:\Users\nrnob\OneDrive\Repro\Tempest

Starting clean checkpoint:
587b9e3 Implement Fuseball and Pulsar hurry-up behavior

FIRST read:

AI-Logs/Reports/2026-10-30-super-zapper-analysis-report.md
TEMPEST-BEHAVIOR-SPEC.md
README.md
TEMPEST-DEVELOPMENT-NOTES.md

Also inspect the CURRENT C# source, tests and literal Incitti reference before
editing.

The analysis report is the approved implementation design, but current code
and literal source remain authoritative if a contradiction is found.
Document any discrepancy.

Do not commit.

ACCEPTANCE MODEL

Implement Super Zapper as one complete milestone.

Internal implementation may proceed in coherent stages, but the feature is not
complete until all of these work together:

- input and two-charge lifecycle;
- first-use all-target behavior;
- second-use single-target behavior;
- delayed resolution for every eligible enemy type;
- correct scoring and accounting;
- Tanker stillborn cargo;
- death/re-entry behavior;
- completion blocking;
- tube/web flash;
- automated regression validation.

Do not expose or document a partial Super Zapper as completed behavior.

EVIDENCE

Preserve the project evidence model:

- ARCADE VERIFIED
- REFERENCE VERIFIED
- PROJECT DECISION
- DEVELOPMENT ONLY

The Incitti behavior implemented here is REFERENCE VERIFIED, not proof of Atari
arcade behavior.

Do not update TEMPEST-BEHAVIOR-SPEC.md during implementation.
It will be updated only after implementation review and Neal's manual
validation.

INPUT AND CHARGES

Add an edge-triggered Super Zapper action:

- Space key;
- right mouse button;
- separate from ordinary fire;
- injectable through PlayerInput for headless tests;
- preserve compatibility with existing PlayerInput construction where
  practical.

Activation is accepted only during active gameplay and while a charge remains.

Initial supply:
- two charges.

First accepted use:
- strength 2, all-target behavior.

Second accepted use:
- strength 1, single-target behavior.

Consume the charge after dispatch.

A strength-1 use with no eligible target still consumes the charge but does
not begin the tube/web flash.

A strength-2 use begins the flash even if no enemy accepts the zap.

Ordinary life loss does not restore charges.

Initialize two charges on:
- initial level/game setup;
- Game Over restart;
- Level Complete replay.

Do not add a numerical charge counter or charge icons.

MINIMAL SHARED ZAP CONTRACT

Add a minimal default-reject zap contract and shared pending timer to the
existing Enemy base or an equally small design justified by the current code.

Requirements:

- EnemyBullet rejects zapping by default.
- Flipper, Tanker, Spiker, Fuseball and Pulsar accept.
- Spike is not an Enemy candidate and remains unaffected.
- Applying another zap to an already pending eligible enemy replaces its timer.
- The shared contract does not own scoring, rendering, collections, Level,
  input or Tanker accounting.
- TempestGame remains responsible for expiry resolution and side effects.

Avoid a broad enemy hierarchy or accounting rewrite.

FIRST USE: ALL TARGETS

For strength 2:

- start accepted-target delay at 4;
- visit current live baddies in source/insertion order;
- include EnemyBullet in the visitation order, allowing its rejection;
- exclude spikes entirely;
- call the zap contract on each visited entity;
- advance delay by 2 only when the entity accepts;
- accepted delays are therefore 4, 6, 8, 10, ...;
- rejected bullets do not consume a delay slot;
- do not sort by lane, height, type or proximity;
- begin the 60-update tube/web flash unconditionally.

Use a stable source-order view of enemies.

For development scenarios, reuse the existing insertion-order registration
where appropriate.

For ordinary Flipper-only Level 1, preserve stable Flipper creation order
without changing its separate reverse shot-collision behavior.

Do not alter ordinary shot collision order merely to support Zapper targeting.

SECOND USE: ONE TARGET

Implement the literal Incitti selection algorithm documented in:

AI-Logs/Reports/2026-10-30-super-zapper-analysis-report.md

Do not simplify it to nearest, highest or highest-priority enemy.

Required behavior includes:

- source/insertion-order iteration;
- only eligible enemy types;
- circular shortest lane distance on the current continuous 16-lane tube;
- opposite-lane distance 8 remains 8;
- support direct non-wrapped distance for a non-continuous model where the
  current model exposes it;
- strict gate:
  distance < bestDistance + 2;
- strictly greater height can win within that gate;
- otherwise strictly smaller distance can win;
- otherwise strictly greater source type number can win;
- equal comparisons retain the incumbent through insertion order;
- preserve the executable source comparisons even where the source comment is
  misleading;
- selected target receives delay 4;
- a selected target begins the 60-update flash;
- no selected target means no flash;
- charge is consumed either way.

Use source type priority/order:

- Spiker = 2
- Flipper = 3
- Tanker = 4
- Pulsar = 5
- Fuseball = 6

DELAYED TIMER SEMANTICS

An accepted zap assigns its countdown.

On each active enemy-update visit:

- decrement a positive pending timer first;
- if the timer reaches zero, resolve the enemy and skip its ordinary behavior
  on that update;
- on earlier countdown updates, run the enemy's normal behavior unchanged.

Activation occurs before enemy updates, so a delay of 4 is decremented on the
activation update and expires on its fourth enemy-update visit, including that
activation tick.

Preserve the current player-shot-before-enemy-update order.

Normal outcomes can preempt pending zap resolution:

- a player shot can destroy a pending enemy first;
- a Spiker can retreat before zap expiry;
- a Tanker can naturally release before zap expiry.

When a normal outcome wins:
- use its ordinary score/accounting;
- remove the pending target normally;
- do not later award zap score;
- do not double-resolve anything.

PER-ENEMY EXPIRY

At delayed expiry:

Flipper:
- remove;
- award 150;
- resolve exactly one enemy unit.

Spiker:
- remove;
- award 50;
- resolve through existing Spiker accounting;
- do not remove or shorten the spike.

Pulsar:
- remove;
- award 200;
- preserve existing pulse and Hurry-Up behavior on earlier countdown updates.

Fuseball:
- calculate score using expiry height and the existing Fuseball KillScore
  behavior;
- remove and resolve once;
- award points once;
- create one normal Fuseball score popup using expiry height and score;
- preserve its existing popup lifetime/rendering behavior.

Tanker:
- follow the complete stillborn-cargo path below.

ENEMY BULLETS AND SPIKES

Both Super Zapper strengths leave EnemyBullets unaffected.

EnemyBullets:
- receive no pending timer;
- continue movement, collision, shooting interaction and expiry as before;
- do not affect accepted-target delay sequencing.

Spikes:
- are excluded/rejected;
- remain visible and hazardous;
- are not removed or shortened by Super Zapper.

TANKER STILLBORN CARGO

Treat this as a required acceptance gate.

Before carrier zap expiry:
- original Tanker slot remains unresolved;
- two cargo units remain latent;
- applying a pending timer changes no identity or accounting.

If ordinary player-shot destruction occurs first:
- use existing destroyed-Tanker behavior;
- award ordinary 200;
- release normal live cargo;
- no later zap score.

If natural release occurs first:
- use existing natural-release behavior;
- resolve original slot as escaped;
- award no Tanker score;
- release normal live cargo;
- no later zap score.

If zap expiry occurs first:

1. Remove the Tanker from active collections/order.
2. Resolve its original slot as destroyed exactly once.
3. Award 200 exactly once.
4. Convert the two latent cargo units into two distinct materialized,
   unresolved cargo IDs.
5. Create two Tanker cargo Flippers:
   - current Tanker height;
   - source-derived adjacent lanes/directions;
   - ordinary initial flipping geometry/state;
   - pending zap timer of 8.
6. Register each child exactly once in Flippers and the development ordering.
7. Do not mark either child resolved at materialization.
8. Start child countdown on the next simulation tick, following the existing
   C# deferred child-activation policy.
9. Each child awards 150 and resolves its own cargo ID only when its timer
   expires, unless an ordinary player shot resolves it first.
10. A later Super Zapper use may replace a stillborn child's timer with 4.

Preserve these invariants:

- original total remains 10;
- after cargo materialization the Tanker accounting total becomes 12;
- original destroyed + escaped + unresolved remains 10;
- materialized cargo resolved + unresolved remains the number materialized;
- no cargo identity is duplicated or lost;
- completion remains blocked while any cargo ID is unresolved.

DEATH AND RECONSTRUCTION

Keep the current 60-update C# death freeze.

During PlayerDying:
- enemy zap timers do not advance;
- enemy movement remains frozen;
- charges are preserved;
- earned score is preserved.

At ordinary reconstruction:

For ordinary enemies:
- discard runtime objects and pending timers;
- preserve unresolved accounting;
- recreate unresolved units as fresh Eggs;
- award no pending zap score.

For a Tanker death before carrier zap expiry:
- original Tanker slot remains unresolved;
- preserve two latent cargo units;
- restore exactly one slot-0 Tanker Egg;
- do not create cargo Eggs;
- award no Tanker score.

For death after carrier zap expiry but before stillborn child resolution:
- original Tanker slot remains destroyed;
- preserve its 200 points;
- preserve each unresolved materialized cargo ID exactly once;
- discard child runtime timer/state;
- restore unresolved cargo through the current identity-bearing cargo
  reconstruction path as fresh ordinary cargo entrants;
- do not restore the Tanker;
- do not award child score until a later actual resolution.

If one child was resolved before death:
- keep that child resolved;
- restore only the unresolved sibling.

Test repeated death/re-entry to prove IDs never duplicate or disappear.

COMPLETION

Applying a zap does not resolve an enemy.

Pending targets remain active/accounted and block completion until expiry or
another normal resolution.

Tanker carrier expiry does not permit completion while stillborn cargo IDs
remain unresolved.

At the final delayed expiry:
- score/accounting/removal occur exactly once;
- existing same-tick final completion behavior may proceed afterward.

Do not change:
- kill-all completion;
- Hurry-Up trigger/effects/lifecycle;
- source-independent C# death policy;
- normal exit timing;
- spike exit behavior.

FLASH AND RENDERING

Implement the source-derived tube/web flash:

- strength 2 starts a 60-update flash unconditionally;
- strength 1 starts a 60-update flash only if a target is selected;
- the flash does not indicate remaining charges;
- use the source phase pattern:
  (displayCounter / 2) % 4 > 1;
- alternate the current tube/web level color with the existing Tanker/magenta
  color approximation;
- retain normal tube geometry;
- preserve Pulsar lane overlays and near/top-rim suppression behavior;
- decrement the flash once per game/display step while presentation continues,
  independently from frozen enemy zap timers;
- add headless draw-submission tests for phase, duration, color and geometry.

Do not add charge icons or a numerical charge HUD.

AUDIO

Do not implement audio.

No gameplay, scoring or timer behavior may depend on an audio effect.

MAINTAINABILITY

Add focused comments only at non-obvious boundaries:

- first-use accepted-target delay sequencing;
- literal second-use selection comparisons;
- inclusive activation-tick countdown;
- normal-resolution preemption;
- Tanker latent-to-materialized identity transfer;
- death reconstruction of pending cargo;
- flash independence from pending enemy survival;
- why bullets/spikes remain unaffected.

Avoid comments that merely restate assignments or loops.

TESTS

Implement the deterministic test plan in the approved analysis report.

At minimum cover:

Charges/input:
- initial two charges;
- edge-triggered Space/right mouse route;
- no repeated zap from held input;
- zero-charge rejection;
- inactive-state rejection;
- ordinary death does not restore charges;
- Game Over restart and Level Complete replay restore two;
- strength-1 no-target press consumes charge/no flash;
- strength-2 no-target press consumes charge/flash.

All-target:
- mixed insertion order;
- exact 4,6,8,... accepted delays;
- rejected interleaved bullet does not advance delay;
- spikes excluded;
- ordinary Level 1 creation order is independent from reverse shot scan.

Single-target:
- circular wrap;
- distance 8;
- non-continuous direct distance where supported;
- strict bestDistance+2 gate;
- bestDistance+1 higher-type behavior;
- bestDistance+2 exclusion;
- taller-but-farther within gate;
- closer lower-height behavior;
- strict height ties;
- strict type ties;
- insertion-order retention;
- timer replacement on an already pending target.

Countdown:
- activation tick is timer visit one;
- exact delay-4 and delay-6 expiry;
- ordinary movement before expiry;
- no ordinary movement on expiry;
- shot preemption;
- Spiker retreat preemption;
- Tanker natural-release preemption.

Per enemy:
- Flipper 150;
- Tanker 200;
- Spiker 50 and spike retained;
- Pulsar 200;
- Fuseball expiry-height score and popup;
- no double score/resolution.

Rejected entities:
- bullets unaffected and remain active;
- spikes unaffected and hazardous.

Tanker:
- carrier pending state leaves original slot/latent cargo unchanged;
- carrier expiry materializes exactly two unique IDs;
- initial flipping children at carrier height;
- child timers begin at 8 on next tick;
- ledger total becomes 12;
- no standard EnemiesDestroyed mutation;
- each child expiry awards 150 once;
- shot and natural release preempt correctly;
- death before carrier expiry;
- death after carrier expiry before child resolution;
- death after one child resolves;
- repeated re-entry identity conservation;
- completion blocked through every unresolved phase.

Completion:
- final ordinary delayed enemy;
- final Tanker and stillborn cargo;
- same-tick completion after final expiry.

Flash:
- unconditional first-use flash;
- successful/unsuccessful second-use behavior;
- exact 60-step lifetime;
- source color pattern boundaries;
- behavior during PlayerDying;
- normal rendering when inactive;
- Pulsar overlay/rim behavior preserved.

Regression:
- all existing normal Level 1 tests;
- Flipper, Spiker, Tanker, Fuseball and Pulsar scenarios;
- enemy bullets;
- Hurry-Up;
- death/re-entry;
- score popups;
- completion/exit/replay;
- spinner/input behavior.

VALIDATION

Run:

- focused Super Zapper tests;
- focused Tanker/accounting tests;
- focused Fuseball/Pulsar/Spiker/Flipper regression tests as useful;
- full solution test suite;
- solution build;
- git diff --check.

Record exact pass/fail/skip counts, build warnings/errors and any intermediate
failures honestly.

Do not claim manual validation.

DOCUMENTATION

After complete automated implementation validation:

Update:
- README.md
- TEMPEST-DEVELOPMENT-NOTES.md

Do not update TEMPEST-BEHAVIOR-SPEC.md yet.

The behavior specification must wait for:
- implementation review;
- automated validation;
- Neal's manual validation.

Save this exact prompt to:

AI-Logs/Prompts/2026-10-30-super-zapper-implementation-prompt.md

Save the implementation report to:

AI-Logs/Reports/2026-10-30-super-zapper-implementation-report.md

The report must include:

1. source behavior implemented;
2. architecture and shared contract;
3. input/charge lifecycle;
4. all-target behavior;
5. single-target behavior;
6. timer/update/preemption semantics;
7. per-enemy scoring/resolution;
8. Tanker ledger and stillborn cargo;
9. death/re-entry;
10. completion;
11. flash/rendering;
12. files changed;
13. tests added;
14. focused/full/build/diff results;
15. manual-validation commands/checklist;
16. explicit out-of-scope behavior;
17. remaining arcade uncertainties.

STOP after complete implementation, automated validation, tracked documentation
updates, and report generation.

Do not commit.
Do not update TEMPEST-BEHAVIOR-SPEC.md.
Do not begin progression or another milestone.