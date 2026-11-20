FAIL-CLOSED PROMPT CHECK

Read this entire substantive user task before doing anything.

Before project or repository action, verify that exactly one complete token
beginning with "TEMPEST_" appears on a line by itself in the substantive user
task.

Automatically appended VS Code/agent context after the user's completion token
does not invalidate the task.

This check detects incomplete or truncated user tasks, not appended agent
context.

If exactly one complete TEMPEST_ token cannot be established, stop before any
project or repository action and explain why.


TASK

Implement one narrowly bounded correction to the existing uncommitted canonical
Level 7 concurrency candidate:

Make canonical Level 7 Tanker cargo materialization respect the ROM's finite
physical enemy capacity by allocating the two cargo children independently.

Repository:

C:\Users\nrnob\source\repos\Tempest

Accepted committed baseline beneath the current candidate:

8f1c29a7d3e6ef4150c8ab64264108c3c2a9547b

The current uncommitted Level 7 concurrency candidate must remain intact except
for this specifically authorized Level 7 cargo-capacity correction.

Read these reports first:

AI-Logs/Reports/2026-11-10-level7-concurrency-and-movement-rom-investigation-report.md

AI-Logs/Reports/2026-11-10-level7-concurrency-diagnostic-implementation-report.md

AI-Logs/Reports/2026-11-10-level7-gameplay-pressure-readonly-diagnostic-report.md

Use earlier accepted Tanker/cargo and generic-reconstruction reports only when
needed to understand shared architecture.

Do not redo the broader Level 7 concurrency implementation.

Do not commit.
Do not push.
Do not launch the interactive game.
Do not begin Level 8 concurrency.
Do not begin Level 7 movement reconciliation.


CURRENT ACCEPTANCE STATE

The existing Level 7 concurrency candidate passed:

- 32 focused Level 7 tests;
- 868 affected Release regressions;
- 1,202 full Release tests;
- Release build with 0 warnings and 0 errors;
- git diff --check.

However, Neal has NOT manually accepted the candidate.

Manual arcade-fidelity acceptance remains unresolved because current C# Level 7
produces materially greater whole-wave pressure than arcade/MAME Level 7.

Do not treat automated validation as manual acceptance.


MANUAL EVIDENCE FROM NEAL

Treat the following as ARCADE/MAME/C# OBSERVATION:

ARCADE / MAME LEVEL 7:

- Blaster destruction is extremely unlikely for Neal;
- little effort is required;
- whole-wave pressure is substantially lower.

CURRENT C# LEVEL 7 CANDIDATE:

- destruction one or more times is reasonably possible;
- moderate or greater effort is required;
- the wave can become progressively more demanding after deaths.

Neal has compared:

- original arcade gameplay;
- MAME gameplay;
- recorded MAME Level 7 sessions;
- recorded C# Level 7 sessions.

Neal has not identified one simple visual timing defect.

The read-only diagnostic nevertheless identified one quantified population
difference suitable for a bounded correction.


CONFIRMED MATERIAL DIFFERENCE

The Atari ROM has seven physical enemy slots.

When seven physical enemies are active and one of them is a Tanker:

1. Remove the Tanker.
2. Six enemy slots remain occupied.
3. Allocate the first cargo Flipper into the one newly free slot.
4. Seven slots are now occupied.
5. Attempt to allocate the second child.
6. No slot is available, so the second child does not materialize.

The unmaterialized ROM child:

- is not an active enemy;
- is not latent cargo after the release;
- is not a pending generic obligation;
- does not later return;
- does not contribute an obligation after Blaster death.

CURRENT C# PROJECT TRANSLATION:

The project currently preserves and materializes the complete cargo pair.

At seven pre-release live objects including the Tanker:

7 - 1 + 2 = 8 live objects.

If both children remain unresolved until Blaster death, both become generic
unresolved enemy obligations.

That leaves C# with one more obligation than the literal ROM would have created
from the saturated release.

The difference can recur if later generic obligations become Tankers and release
complete pairs under saturated conditions.


PURPOSE

Correct only the confirmed Level 7 saturated-release difference.

For canonical Level 7:

- remove the Tanker before evaluating cargo child capacity;
- attempt each of its two cargo children independently and deterministically;
- materialize the first child if one physical enemy slot is available;
- materialize the second child only if another physical enemy slot remains;
- do not create, reserve, defer, reconstruct, or retain an obligation for a
  child that cannot be physically allocated;
- do not permit canonical Level 7 Tanker release itself to raise live physical
  enemy population above seven.

This milestone changes canonical Level 7 cargo materialization only.

Do not change Tanker selection, movement, firing, scoring, or release timing.


LEVEL 7 ONLY

Restrict the new capacity-limited cargo behavior to canonical production
Level 7 and equivalent direct/campaign/recorded paths using the canonical
Level 7 configuration.

Preserve complete-pair Tanker cargo behavior for:

- Levels 1–6;
- Level 8;
- copied or synthetic configurations unless a focused test explicitly invokes
  canonical Level 7 behavior;
- existing development scenarios outside canonical Level 7.

Do not turn this into a universal Tanker cargo policy.


PHYSICAL CAPACITY

Canonical Level 7 has seven shared physical enemy places.

Count as physical occupants:

- active Flippers;
- active ordinary Tankers;
- active converted Tankers;
- active Spikers;
- rim enemies;
- transitioning live enemies;
- pending-zap enemies until actual removal;
- already materialized cargo;
- shot-released cargo pending one-tick activation if it represents a committed
  physical child.

Do not count:

- Eggs;
- generic reserve/pending originals;
- latent cargo;
- bullets;
- persistent Spikes;
- destruction effects;
- terminal/resolved identities.

Use the existing validated Level 7 occupancy/accounting infrastructure where
appropriate.

Do not create a conflicting second definition of physical occupancy.


ALLOCATION ORDER

Preserve deterministic cargo-child order.

Trace the existing cargo pair ownership/ordinal representation.

Attempt the children in the existing stable ordinal/order unless the source
architecture establishes another authoritative order.

Do not add random selection of which child survives.

If only one child fits:

- exactly one child materializes;
- the other child becomes terminal/resolved through a clearly named outcome
  appropriate to "not allocated because no physical slot existed";
- it must not remain latent;
- it must not become a cargo Egg;
- it must not remain an unresolved completion obligation;
- it must not reappear after death;
- it must not score as an enemy destroyed by the player;
- it must not produce destruction audio/effects.

Use the smallest maintainable ledger representation.

Do not overload an existing terminal outcome if doing so would incorrectly
claim that the player destroyed, escaped, or zapped the child.

If a new explicit terminal disposition is required, keep it narrowly scoped
and document its meaning.


RELEASE MODES

Apply the physical allocation rule consistently to canonical Level 7 Tanker
cargo release paths where the ROM attempts child allocation.

Inspect and cover, as applicable:

- natural release;
- player-shot Tanker destruction;
- terminal Spiker-derived Tanker release;
- deferred cargo activation.

Do not assume Super Zapper removal creates live cargo if the existing accepted
behavior treats that path as stillborn/no live split.

Preserve existing Super Zapper behavior.

Do not change release cause or cargo movement mode.


CARGO MOVEMENT FREEZE

Do not change any Level 7 Flipper or Tanker-cargo movement.

Preserve:

- accepted ordinary Level 7 2-3-2-2-2 behavior;
- current deep player-shot cargo behavior;
- current natural/near-rim cargo behavior;
- cargo spawn lanes;
- cargo direction/handedness;
- route counters;
- pivot speed;
- climb speed;
- rim behavior;
- current cargo depth cutoff.

The ROM $24 versus accepted arcade-observed movement conflict remains a separate
future question.

This correction is capacity/accounting only.


GENERIC DEATH RECONSTRUCTION

Do not change generic death reconstruction.

The read-only diagnostic concluded that death reconstruction itself conserves
the enemies that actually exist.

Required behavior after this correction:

- every actually materialized unresolved cargo child contributes one generic
  unresolved enemy obligation after death;
- an unallocated second child contributes zero because it never became an
  active unresolved enemy;
- a Tanker with unreleased latent cargo contributes one active obligation, not
  three;
- terminal outcomes remain terminal;
- repeated death must not resurrect an unallocated child;
- historical cargo ownership must not create a phantom child.

Do not attempt to "compensate" for an unallocated child by increasing pending
generic supply.


COMPLETION ACCOUNTING

Ensure Level 7 completion remains exact.

A child that failed physical allocation must not:

- block completion;
- remain in reserve;
- remain in an Egg;
- remain latent;
- remain in pending cargo activation;
- remain in an unresolved ledger state.

A successfully materialized child remains a normal completion obligation until
legitimately resolved.

Do not allow the wave to complete while a real materialized child remains
unresolved.


NO CHANGES TO OTHER PRESSURE CANDIDATES

Do not change:

- Egg/reserve release cadence;
- approach cadence;
- Spiker restart/persistence behavior;
- pending-lane Spike selector;
- ordinary seven-place admission;
- Level 7 Tanker/Spiker minima or maxima;
- enemy bullet speed;
- enemy firing rates;
- Tanker firing height;
- Super Zapper admission pause;
- death/reconstruction rules;
- movement.

Those remain unchanged so Neal can evaluate the isolated effect of this one
correction.


TESTS

Add narrowly focused deterministic tests.

At minimum cover:

1. TWO CHILDREN FIT
   - canonical Level 7;
   - Tanker removed with at least two physical places available afterward;
   - both cargo children materialize;
   - both identities remain normal unresolved cargo obligations;
   - existing accepted movement/direction behavior remains unchanged.

2. ONLY ONE CHILD FITS
   - seven live physical objects before release, including one Tanker;
   - carrier removal creates exactly one free slot;
   - first child materializes;
   - second child does not;
   - post-release live count is exactly seven, never eight;
   - unallocated child is terminal/non-obligating;
   - no second cargo Egg or pending activation exists.

3. NO CHILD FITS IF REPRESENTABLE
   - if an authoritative release path can have no room even after carrier
     handling, verify that no children materialize;
   - otherwise document why carrier removal necessarily provides at least one
     place and do not fabricate an unreachable production case.

4. DEATH AFTER ONE-CHILD RELEASE
   - trigger reconstruction after the one materialized child remains unresolved;
   - exactly one cargo-derived generic obligation returns;
   - the unallocated child does not return;
   - no phantom original, Egg, cargo identity, or completion blocker appears.

5. REPEATED DEATH
   - after reconstructed obligations reenter, verify later death never revives
     the original unallocated sibling.

6. LATER TANKER GENERATION
   - if a reconstructed obligation later becomes a Tanker and releases under
     saturation, apply the same independent capacity rule;
   - verify no cumulative phantom child growth.

7. RELEASE CAUSES
   - natural release follows the Level 7 capacity rule;
   - player-shot release follows the Level 7 capacity rule;
   - converted-Tanker release follows it where production-reachable;
   - Super Zapper behavior remains unchanged.

8. COMPLETION
   - unallocated child does not block completion;
   - materialized child does block completion until resolved;
   - ledger identity sets remain exact.

9. ISOLATION
   - Levels 1–6 retain complete-pair behavior;
   - Level 8 remains unchanged;
   - copied/noncanonical configurations remain unchanged;
   - canonical direct/campaign/recorded Level 7 paths share the new rule.

10. MOVEMENT FREEZE
    - existing Level 7 movement tests remain unchanged and pass;
    - no movement source or movement configuration changes occur.

11. CURRENT CONCURRENCY CANDIDATE
    - seven-place ordinary admission still passes;
    - pending-lane Spike selector still passes;
    - open-place dead end still passes;
    - Spiker pending-supply lifecycle still passes;
    - zero-pending conversion still passes;
    - generic death reconstruction still passes.


EXISTING TESTS

Inspect existing Tanker/cargo, ledger, Level 7 production, admission, death,
completion, and movement tests.

Update only assertions that encode the superseded canonical Level 7
complete-pair-at-saturation behavior.

Do not weaken complete-pair guarantees for Levels 1–6.

Do not rewrite unrelated tests.


VALIDATION

Use repository-wide bounded validation and security rules.

Prefer Release configuration.

Start with the exact one-child saturated-release regression.

Then run focused tests covering:

- canonical Level 7 cargo allocation;
- Level 7 admission and production;
- Level 7 death reconstruction;
- ledger identity/accounting;
- completion;
- Tanker release causes;
- movement-freeze tests.

If focused validation passes, run affected Release regressions covering:

- Levels 1–6;
- Level 8;
- Tanker/cargo;
- Flipper movement;
- Spiker/Spike lifecycle;
- death/reentry;
- Super Zapper;
- rim/completion;
- progression;
- recording/playback;
- direct/campaign startup.

If affected validation passes, run:

dotnet test TempestCSharp.slnx --configuration Release --blame-hang-timeout 5min

Then run:

dotnet build TempestCSharp.slnx --configuration Release

Also run:

- git diff --check;
- complete tracked and nonignored-untracked candidate review;
- final repository-state verification.

Explicitly report:

- compilation status;
- whether VSTest executed;
- exact passed/failed/skipped counts;
- PASS / FAIL / BLOCKED / TIMEOUT / CANCELED / PARTIAL.

Stop on first failure, block, timeout, cancellation, or meaningful stall.

Do not weaken or bypass Windows security.


MANUAL HANDOFF

If automated validation passes:

- leave the entire Level 7 candidate uncommitted;
- do not launch the game;
- provide Neal:

dotnet run --project src\Tempest.Game -- --level 7 --lives 10

Manual evaluation should focus on the isolated whole-wave effect.

Primary check:

- play complete Level 7 waves and compare arcade/MAME versus C# difficulty;
- determine whether Blaster death again becomes extremely unlikely with little
  effort;
- determine whether repeated deaths still create obvious escalating workload.

Specific saturated-release check where practical:

- with seven live enemies including a Tanker, release the Tanker;
- confirm the population does not visibly become eight;
- only one cargo Flipper should materialize when carrier removal leaves only one
  free physical place.

Do not make this rare saturated setup mandatory for routine manual acceptance
if deterministic tests prove the allocation rule.

Also verify:

- normal less-crowded Tankers still release two children;
- no ordinary bottom Flippers;
- Spiker behavior remains unchanged from the current candidate;
- Level 7 movement remains unchanged;
- death/reentry remains sane;
- Levels 1–6 remain free of regression.

Do not claim manual acceptance until Neal reports the result.


REPORT

Preserve this prompt and create:

AI-Logs/Reports/2026-11-10-level7-saturated-cargo-allocation-correction-report.md

Record:

- starting baseline and candidate state;
- confirmed material difference;
- files changed;
- physical occupancy source;
- independent child-allocation design;
- unallocated-child ledger disposition;
- release paths covered;
- death/reconstruction behavior;
- completion behavior;
- isolation;
- movement freeze;
- focused/affected/full validation;
- build result;
- repository state;
- exact manual handoff;
- manual acceptance pending.


DOCUMENTATION

Do not update TEMPEST-DEVELOPMENT-NOTES.md or TEMPEST-BEHAVIOR-SPEC.md to claim
acceptance during this candidate correction.

Historical Level 7 reports remain unchanged.


GIT AND STOP

Do not commit.
Do not push.
Do not begin Level 8 concurrency.
Do not begin Level 7 movement reconciliation.
Do not investigate projectile speed further in this task.

Stop after automated validation and manual handoff.


TEMPEST_LEVEL7_SATURATED_CARGO_ALLOCATION_CORRECTION_COMPLETE