FAIL-CLOSED PROMPT CHECK

Read this entire substantive user task before doing anything.

Before project or repository action, verify that exactly one complete token
beginning with "TEMPEST_" appears on a line by itself in the substantive user
task.

Automatically appended VS Code/agent context after the user's completion token
does not invalidate the task.

If exactly one complete TEMPEST_ token cannot be established, stop before any
project or repository action and explain why.


TASK

Perform a bounded READ-ONLY diagnostic of the unresolved Level 7 gameplay-
pressure difference between the original Atari/MAME game and the current
uncommitted C# Level 7 concurrency candidate.

Repository:

C:\Users\nrnob\source\repos\Tempest

Accepted committed baseline beneath the current candidate:

8f1c29a7d3e6ef4150c8ab64264108c3c2a9547b

The existing uncommitted Level 7 concurrency candidate must remain intact.

DO NOT modify production code.
DO NOT modify tests.
DO NOT modify durable documentation.
DO NOT stage.
DO NOT commit.
DO NOT push.
DO NOT launch the interactive game.
DO NOT begin Level 8 work.


CURRENT ACCEPTANCE STATE

The current Level 7 concurrency candidate has passed automated validation:

- focused Level 7 tests: 32 passed;
- affected Release regressions: 868 passed;
- full Release solution: 1,202 passed, 0 failed, 0 skipped;
- Release build: 0 warnings, 0 errors;
- git diff --check: PASS.

However, Neal has NOT manually accepted the candidate.

Manual arcade-fidelity acceptance is currently unresolved/failed because
whole-wave gameplay pressure differs materially from the arcade/MAME game.

Do not treat automated success as gameplay acceptance.


NEAL'S NEW MANUAL EVIDENCE

Treat the following as ARCADE/MAME/C# OBSERVATION supplied by Neal.

Neal is an expert Tempest player and has recently compared:

- original arcade Tempest;
- Tempest running in MAME;
- recorded Level 7 MAME gameplay;
- recorded Level 7 C# gameplay;
- frame-by-frame playback where useful.

The strong whole-wave observation is:

ARCADE / MAME LEVEL 7:

- Blaster death during Level 7 is extremely unlikely for Neal;
- little playing effort is required;
- the wave feels substantially easier / less pressured.

CURRENT C# LEVEL 7 CANDIDATE:

- there is a reasonable chance the Blaster will be destroyed one or more
  times;
- moderate or greater effort is required;
- the game subjectively appears to sustain more enemy pressure.

This is not a small tuning complaint.

Treat it as evidence that the current C# Level 7 candidate remains materially
different in gameplay even though its mechanical tests pass.


OBSERVED DEATH / POPULATION EFFECT

Neal has isolated one important contributor.

A representative stressful state may contain materialized enemies such as:

- four Flippers;
- four Tankers;
- three Spikers.

Do not assume this exact state is always reachable through ordinary ROM
admission or occurs with those exact counts.

Use it only as an example of the observed amplification concern.

At death, currently materialized unresolved enemies become generic obligations
under the reconstruction model.

For clarity throughout this investigation, use the phrase:

"generic unresolved enemy obligation"

rather than "reconstructed unit."

One unresolved active physical enemy contributes one generic unresolved enemy
obligation after death.

A Tanker with an unreleased latent cargo pair contributes ONE active obligation,
not three.

Each already materialized cargo Flipper contributes ONE active obligation.

A surviving rim enemy also contributes one.

Prior species identity is discarded.


NEAL'S AMPLIFICATION CONCERN

The concern is NOT merely that Flippers return after death.

The concern is:

materialized Flippers
    ->
death
    ->
generic unresolved obligations
    ->
later Tanker/Spiker selection
    ->
some obligations may become Tankers
    ->
those Tankers can generate new cargo Flippers
    ->
another death can reconstruct those materialized children again

Neal observes that repeated deaths can make the remaining Level 7 wave feel
progressively larger and substantially more difficult.

DO NOT assume the example mapping:

"four Flippers become four Tankers become eight Flippers"

is a literal deterministic selector rule.

Level 7 Tanker/Spiker minima, maxima, headroom, and selector state still apply.

The diagnostic must determine whether the overall multiplication concern is
real and, if so, which C# translation is responsible.


PRIMARY QUESTION

Answer:

WHY does current C# Level 7 produce materially greater gameplay pressure than
arcade/MAME Level 7?

Do not stop at the first plausible explanation.

Compare all pressure-producing mechanisms below sufficiently to rank the
supported causes.


A. WAVE / OBLIGATION ACCOUNTING

Trace the complete Level 7 accounting lifecycle in both ROM and current C#.

For ROM establish:

- fresh generic supply;
- active physical enemies;
- pending generic entries;
- ordinary Tankers;
- latent cargo;
- materialized cargo;
- Spiker-derived Tankers;
- rim enemies;
- death reconstruction;
- subsequent ordinary species selection.

For C# trace the equivalent project representations.

Answer whether an uninterrupted Level 7 wave can create more materialized
enemies than its initial generic supply due to Tanker cargo.

Then answer whether death/reconstruction can cause the future number of
materialized enemies generated from the remaining unresolved obligations to
increase relative to the pre-death population.

Clearly distinguish:

- obligation conservation;
- species transformation;
- cargo creation;
- simultaneous active population;
- cumulative number of enemies the player eventually has to destroy.

Do not equate those concepts.


B. REPEATED-DEATH AMPLIFICATION

Perform bounded deterministic accounting examples.

At minimum model:

CASE 1:
No death.

CASE 2:
One death with unresolved ordinary Tankers/Spikers but no materialized cargo.

CASE 3:
One death with several unresolved materialized cargo Flippers.

CASE 4:
A second death after some reconstructed generic obligations have become new
Tankers and released cargo.

For each case compare:

- unresolved generic obligations before death;
- active physical objects before death;
- latent cargo;
- materialized cargo;
- reconstructed generic obligation count;
- eligible post-death species;
- eventual cargo creation possibilities;
- whether total future enemies-to-resolve can increase.

Do this separately for:

1. literal ROM behavior; and
2. current C# project translations.

Do not simulate unknown random choices as facts.

Where selection is dynamic, give bounded outcomes/ranges or representative
states only when directly supported.


C. COMPLETE-PAIR CARGO TRANSLATION

This is a particularly important question.

ROM:

A releasing Tanker is removed first and then its two children are allocated
independently into the finite physical enemy slots.

When physical capacity allows only one child, the second child need not become
a live enemy.

C# PROJECT TRANSLATION:

The project preserves the complete pair and both cargo identities.

Complete-pair release can temporarily cause the C# live population to exceed
the ROM's physical limit, after which ordinary admission waits.

Investigate whether this translation can compound across death/reconstruction.

Specifically determine:

- whether C# preserves more materialized Flippers than the ROM would under
  crowded conditions;
- whether those additional surviving materialized Flippers become additional
  generic unresolved obligations after a death;
- whether those obligations can later become Tankers;
- whether the resulting Tankers can create another complete cargo pair;
- whether repeated deaths can therefore increase total future workload in C#
  relative to literal ROM behavior.

Do not alter complete-pair behavior.

This is analysis only.

If this is a material source of compounding pressure, quantify it with the
smallest defensible examples.


D. ORDINARY ADMISSION / POPULATION PRESSURE

Compare the current Level 7 candidate's new concurrency rules with ROM
semantics.

Investigate:

- seven physical places;
- Tanker max 2;
- Spiker max 4;
- zero-minimum Flipper exclusion;
- pending-lane Spike selector;
- latent-cargo availability;
- open-place dead end;
- same-tick hatch behavior;
- C# Egg/release cadence;
- approach cadence;
- ready-Egg waiting behavior.

Determine whether C# can maintain a greater sustained active population than
the ROM even when no death occurs.

Pay particular attention to the project Egg/release scheduling.

Earlier project investigation found C# ledger-backed levels use a project
Egg/release model rather than literal ROM pending-entry timing.

Do not assume matching supply means matching visible concurrency.


E. SPIKER PRESSURE

Compare literal ROM Level 7 Spiker lifecycle with the current candidate.

ROM:

- grow;
- retreat;
- hop/continue in the same physical enemy slot while generic pending supply
  remains;
- convert in place to a Flipper-carrying Tanker only at actual zero pending
  supply.

CURRENT C# CANDIDATE:

- while generic pending supply remains, the same Spiker remains live/in the
  same physical place but restarts its local building cycle;
- this is an explicit PROJECT TRANSLATION;
- the ROM lane hop is not implemented;
- at zero pending supply, same-place Tanker conversion remains.

Neal's arcade/MAME observation:

- a visible Spiker appears to travel up and back down once;
- when it reaches the bottom, it becomes a Tanker;
- Neal is not observing obvious repeated building cycles in ordinary Level 7
  arcade/MAME play.

Investigate whether the difference can arise because ROM generic pending supply
normally drains to zero before another visible Spiker cycle is exposed.

Compare ROM pending-entry/admission cadence against C# cadence sufficiently to
evaluate that possibility.

Also determine whether current C# repeated local Spiker cycles increase
gameplay duration or pressure even though they conserve the same obligation.

Do not implement exact ROM lane-hop behavior.


F. ENEMY-SHOT RELATIVE SPEED

Investigate enemy projectile travel speed as a separate pressure mechanism.

Neal observes in the arcade that enemy shots move up the tube faster than the
enemies that fired them.

That qualitative relationship is expected.

The unresolved question is whether C# has the correct RELATIVE rate.


ROM TRACE

Using only the project-local ROM listing, trace:

- enm_shotspd_msb;
- enm_shotspd_lsb;
- the routine that calculates/loads Level 7 enemy-shot speed;
- the routine that advances enemy shots;
- Level 7 Flipper speed;
- Level 7 Tanker speed;
- Level 7 Spiker speed;
- any difficulty adjustment relevant to those speeds;
- cadence/frequency at which enemy shots and enemies receive their movement
  operations.

Do not equate raw ROM fixed-point bytes directly with C# height units.


C# TRACE

Establish current canonical Level 7:

- EnemyBullet height advance per simulation update;
- Flipper along-tube/climb advancement;
- Tanker climb advancement;
- Spiker advancement;
- whether movement updates happen at equal simulation frequency;
- whether flip/turn states affect effective Flipper along-tube advancement.

Prior project evidence states:

- C# uses one RunOneStep per nominal 60-Hz host-loop iteration;
- EnemyBullet historically advances +3 height units per active update;
- Tanker climb values are per-update quantities.

Reverify current source rather than assuming historical constants remain
unchanged.


RELATIVE COMPARISON

Do NOT invent real-time arcade milliseconds if the local evidence does not
establish the interrupt frequency.

Instead calculate whatever relative movement ratios are defensible within each
system.

For example, if supported:

ROM enemy-shot along movement relative to:
- Flipper along movement;
- Tanker along movement;
- Spiker along movement.

C# enemy-shot height movement relative to:
- Flipper;
- Tanker;
- Spiker.

Account for movement cadence when a species does not advance every gameplay
pass.

Then classify whether C# enemy shots are:

- relatively slower;
- approximately comparable;
- relatively faster;
- or UNKNOWN due to incompatible/unresolved movement representations.

Do not tune anything.


G. ENEMY-SHOT FIRING PRESSURE

Separate travel speed from firing frequency.

Inspect only enough existing evidence to determine whether Level 7 pressure may
also differ because of:

- enemy shot slot capacity;
- firing holdoff;
- Tanker ammunition/timing;
- Flipper firing;
- Spiker firing if applicable.

Do not perform a broad firing redesign investigation.

State clearly whether any confirmed existing difference could increase C#
projectile density relative to ROM Level 7.

If evidence is insufficient, mark it UNKNOWN rather than guessing.


H. SUPER ZAPPER ENTRY PAUSE

ROM ordinary enemy creation pauses while zap_running is active.

Current C# Level 7 ordinary hatching does not reproduce that exact gate.

This remains a known deferred mismatch.

For this diagnostic ONLY:

evaluate whether the missing pause could materially increase active/pending
enemy pressure during/after Super Zapper use.

Do not implement it.

Do not redesign Super Zapper.

Classify it as:

- likely material;
- likely minor;
- or unresolved;

only if supported by control-flow/accounting evidence.


I. MOVEMENT FREEZE

Do not alter or attempt to resolve the separate Level 7 $24 versus accepted
2-3-2-2-2 Flipper movement conflict.

That is not this task.

However, where Flipper along-tube movement frequency is required for the
enemy-shot relative-speed comparison, use the ROM $24 trace and current C#
movement separately without deciding which visible route should replace the
other.

No movement implementation changes are permitted.


J. DIAGNOSTIC COMPARISON TABLE

Produce a concise evidence table with rows such as:

- fresh generic supply;
- ordinary physical capacity;
- Egg/pending-entry cadence;
- Tanker cargo materialization;
- complete-pair behavior;
- death reconstruction;
- repeated-death workload;
- Spiker lifetime behavior;
- enemy-shot relative speed;
- enemy-shot capacity/firing pressure;
- Super Zapper admission pause.

Columns:

- ROM-backed behavior;
- current C# candidate;
- likely pressure consequence;
- confidence/evidence class.

Do not assign a pressure consequence where evidence does not support one.


K. RANKED FINDINGS

At the end, rank ONLY supported contributors to the observed Level 7 gameplay
pressure into:

1. CONFIRMED MATERIAL DIFFERENCE
2. STRONG CANDIDATE
3. POSSIBLE / NEEDS MANUAL OR DETERMINISTIC EVIDENCE
4. NOT SUPPORTED AS A CURRENT CAUSE

For every item explain the exact evidence.

Do not force every investigated mechanism into a causal category if the answer
is UNKNOWN.


L. RECOMMENDED NEXT CORRECTION

Recommend ONE smallest safe next implementation milestone.

Possible examples:

- correct C# complete-pair behavior only for saturated Level 7 cargo;
- revise generic reconstruction/accounting to avoid compounding workload;
- correct Egg/admission cadence;
- revise Spiker pending-supply translation;
- correct enemy-shot relative speed;
- add Super Zapper admission pause;
- another narrowly supported correction.

Do not implement anything.

If two corrections are tightly coupled, explain why.

Otherwise choose one.


M. MANUAL ACCEPTANCE TARGET AFTER FUTURE CORRECTION

Keep the eventual acceptance target simple and gameplay-oriented.

Preserve Neal's high-level benchmark:

ARCADE/MAME LEVEL 7:
- extremely unlikely Blaster death;
- little effort required.

C# LEVEL 7 AFTER CORRECTION SHOULD:
- return to comparable whole-wave pressure;
- not produce obvious escalating workload after deaths;
- preserve accepted Levels 1-6;
- preserve the currently frozen accepted Level 7 movement until separately
  resolved.

Also provide only a few precise checks connected to the recommended correction.


SOURCE AUTHORITY / EVIDENCE DISCIPLINE

Use:

1. Neal's direct arcade/MAME observations for observable gameplay.
2. Project-local Atari ROM listing for ROM internal behavior.
3. Accepted project documentation/history for intentional translations.
4. Current C# source/tests for project behavior.
5. Inference only where explicitly labeled.

Do not use Internet ROM/disassembly sources.

Do not substitute Mark Incitti's implementation for ROM authority.

Do not conclude that C# is wrong merely because it differs internally if Neal's
observable arcade evidence supports the C# translation.

Conversely, automated tests do not override a strong manual gameplay-fidelity
failure.


VALIDATION

This is READ-ONLY.

Do not run broad automated tests or builds merely for ceremony.

Small bounded calculations or existing deterministic/headless diagnostic
inspection are allowed only if they do not modify the candidate.

Do not create permanent development switches or telemetry.

Do not launch the interactive game.

At completion verify:

- HEAD still equals the accepted Level 6 baseline;
- existing Level 7 candidate is byte-for-byte unchanged;
- index unchanged;
- no production changes;
- no test changes;
- no durable documentation changes;
- git diff --check passes;
- protected local ROM/reference/user assets untouched.


REPORT

Preserve this prompt and create:

AI-Logs/Reports/2026-11-10-level7-gameplay-pressure-readonly-diagnostic-report.md

Include:

- manual evidence from Neal;
- obligation/population model;
- repeated-death analysis;
- complete-pair cargo analysis;
- ordinary admission/cadence findings;
- Spiker pressure analysis;
- enemy-shot relative-speed analysis;
- enemy-shot firing-pressure analysis;
- Super Zapper boundary;
- ROM-vs-C# comparison table;
- ranked supported causes;
- one recommended next correction;
- future manual acceptance target;
- confirmation that the candidate was not modified.


STOP

Stop after the read-only diagnosis and recommendation.

Do not implement a correction.
Do not commit.
Do not push.
Do not begin Level 8 concurrency.
Do not begin Level 7 movement reconciliation.


TEMPEST_LEVEL7_GAMEPLAY_PRESSURE_READONLY_DIAGNOSTIC_COMPLETE