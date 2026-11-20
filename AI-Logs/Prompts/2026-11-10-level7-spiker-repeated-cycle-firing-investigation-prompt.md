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

Perform a small READ-ONLY investigation of Level 7 Spiker firing across
repeated grow/retreat cycles.

Repository:

C:\Users\nrnob\source\repos\Tempest

Accepted committed baseline beneath the current uncommitted Level 7 candidate:

8f1c29a7d3e6ef4150c8ab64264108c3c2a9547b

The current Level 7 concurrency candidate plus the later saturated-cargo
capacity correction are already present in the worktree.

Preserve that complete uncommitted candidate exactly.

This task is investigation only.

Do not modify production code.
Do not modify tests.
Do not modify durable documentation.
Do not stage.
Do not commit.
Do not push.
Do not launch the interactive game.
Do not begin Level 8 work.
Do not begin Level 7 movement reconciliation.


READ FIRST

Read as relevant:

AI-Logs/Reports/2026-11-10-level7-concurrency-and-movement-rom-investigation-report.md

AI-Logs/Reports/2026-11-10-level7-concurrency-diagnostic-implementation-report.md

AI-Logs/Reports/2026-11-10-level7-gameplay-pressure-readonly-diagnostic-report.md

AI-Logs/Reports/2026-11-10-level7-saturated-cargo-allocation-correction-report.md

TEMPEST-BEHAVIOR-SPEC.md

TEMPEST-DEVELOPMENT-NOTES.md

Use the project-local Atari ROM listing as the primary internal arcade source:

Reference/arcade/Tempest Rom Source code.md

Do not search the Internet for ROM behavior.


CURRENT MANUAL EVIDENCE

Neal has now performed extensive comparison using:

- the original arcade game;
- MAME;
- MAME recorded gameplay;
- C# recorded gameplay;
- playback/pause/frame stepping.

Treat the following as ARCADE/MAME OBSERVED:

A Level 7 Spiker can remain active through repeated up/down cycles when wave
conditions prevent the wave from progressing normally.

On arcade/MAME:

- the Spiker retains its ability to fire on later climbs;
- firing does not permanently cease merely because the Spiker has already
  completed one grow/retreat cycle;
- Neal can visibly observe Spiker shots on subsequent climbs.

CURRENT C# OBSERVATION:

- the Spiker can continue going up and back down the same lane through repeated
  cycles;
- after the first cycle, its firing eventually stops;
- subsequent cycles remain silent;
- the Spiker still moves, but no longer presents the same firing threat.

Neal can reproduce this reliably.

A useful manual setup is:

- keep approximately three Flippers alive on the rim;
- keep moving around the rim so they do not kill the Blaster;
- this can hold the wave in a state where no new ordinary enemy arrives;
- Spikers continue repeated grow/retreat cycles;
- in C# the Spikers eventually stop firing;
- in arcade/MAME the Spikers retain firing ability on later climbs.

Do not treat the exact three-Flipper count as a hidden ROM rule. It is a
practical reproduction setup.


WORKING HYPOTHESIS

Neal suspects that when a Spiker reaches the bottom of its retreat and begins
another climb, some firing state may need to be refreshed or reinitialized.

This may involve:

- ammunition;
- firing timer/holdoff;
- shot counter;
- random firing state;
- another species-specific state.

This is ONLY a hypothesis.

Do not implement or assert a reset mechanism unless source tracing establishes
it.


PRIMARY QUESTION

Determine exactly why current C# Level 7 Spikers stop firing on later
grow/retreat cycles while arcade/MAME Spikers remain capable of firing.

Answer:

1. How does the Atari ROM initialize Spiker firing state?
2. What state limits or governs how many shots a Spiker may fire?
3. What firing state changes while the Spiker climbs?
4. What happens to that firing state when the Spiker retreats?
5. What happens when the Spiker reaches the bottom/retreat endpoint while
   generic pending supply remains?
6. Does ROM code explicitly reinitialize ammunition, timer, or other firing
   state before the next climb/hop cycle?
7. Does the ROM lane-hop/restart path itself call a more general enemy
   initialization path that incidentally refreshes firing state?
8. Is the firing refresh Level-7-specific or part of shared Spiker machinery?
9. What does current C# do at the corresponding lifecycle boundary?
10. Which exact state is missing or incorrectly preserved in C#?


ROM TRACE

Trace the relevant local ROM path.

At minimum inspect:

- Spiker creation/initialization;
- enemy firing initialization;
- any per-species ammo/shot-count setup;
- firing delay/holdoff setup;
- Spiker climbing action;
- Spiker retreat action;
- bottom/retreat completion;
- pending-supply branch;
- lane-hop/restart behavior;
- zero-pending conversion to Tanker;
- shared enemy-fire routine.

Identify the actual variables/fields used.

Do not infer that a generic shot_holdoff value alone represents ammunition.

Distinguish:

- global enemy shot-slot capacity;
- per-enemy firing eligibility;
- per-enemy firing countdown;
- per-enemy ammunition/shot count;
- random firing probability;
- projectile lifetime.

Only report mechanisms actually established by source.


CURRENT C# TRACE

Inspect at minimum as relevant:

src/Tempest.Game/Gameplay/Spiker.cs

src/Tempest.Game/TempestGame.cs

src/Tempest.Game/Levels/Level7WaveSelection.cs

src/Tempest.Game/Levels/LevelConfiguration.cs

the current Level 7 production/admission tests

Spiker/Spike/firing-related tests

enemy bullet implementation

Trace:

- Spiker constructor/factory firing state;
- ammo/shot count;
- initial firing delay;
- countdown/reset;
- random firing logic;
- firing eligibility by Spiker phase;
- behavior during retreat;
- behavior when the current Level 7 candidate restarts the same live Spiker's
  local building cycle;
- whether firing state is reset at that point;
- whether the candidate intentionally preserves exhausted ammo from the first
  climb.

Determine whether the new Level 7 Spiker-persistence translation introduced or
exposed the silent-repeat behavior.

If the same issue predates the candidate, say so explicitly.


LEVEL 7 CANDIDATE CONTEXT

The current concurrency candidate changed Level 7 behavior so that, while
authoritative generic pending supply remains, a retreating Spiker stays the
same live enemy in the same physical place and restarts its local building
cycle rather than being removed and returned through reserve.

That behavior is explicitly a PROJECT TRANSLATION and does not implement the
ROM lane hop.

Investigate whether:

- keeping the same live C# Spiker object also keeps exhausted firing state;
- the previous return-to-reserve/re-hatch path happened to create a fresh
  firing state;
- the ROM same-slot hop/restart refreshes firing state without creating a new
  generic obligation.

Do not conclude this is the cause until traced directly.


ZERO-PENDING BOUNDARY

Keep the zero-pending conversion path separate.

At actual generic pending zero:

- the Spiker converts to a Tanker under the current Level 7 policy.

Determine whether firing refresh is relevant only when pending supply remains
and another Spiker cycle begins.

Do not propose refreshing Spiker state immediately before conversion unless ROM
evidence establishes that.


ENEMY SHOT SPEED IS OUT OF SCOPE

Neal has now reproduced in MAME that an enemy projectile originating behind a
Tanker can catch and pass that Tanker.

Current C# visibly has the same qualitative behavior.

Therefore:

Do not investigate enemy projectile speed in this task.

Do not modify or recommend changing EnemyBullet speed.

The earlier static ROM speed-ratio interpretation remains unresolved but is not
a present gameplay blocker.


SATURATED CARGO IS OUT OF SCOPE

The Level 7 saturated-cargo correction has completed automated validation and
has had subsequent manual testing.

Do not change it here.

Do not reopen repeated-death workload growth.

Neal has determined that extreme workload amplification requires an artificial
many-life repeated-death scenario and does not warrant further investigation
for normal gameplay.


TANKER NATURAL-VS-SHOT CARGO IS SEPARATE

Neal has another concrete observation that will be investigated later:

- with two Tankers naturally reaching the top/release region, arcade/MAME can
  produce three total Flippers;
- destroying Tankers earlier with player shots can produce four Flippers;
- current C# appears unable to reproduce the same four-rim-Flipper case after
  the new capacity correction.

Do NOT investigate or modify that in this task.

Keep this Spiker investigation isolated.


TEST-GAP ANALYSIS

Inspect existing tests and identify why automated validation did not detect the
repeat-cycle firing problem.

Determine whether tests currently cover:

- first-cycle Spiker firing;
- Spiker ammo exhaustion;
- retreat;
- restart while pending supply remains;
- firing on a second climb;
- firing on a third climb;
- firing state across lane hop/restart;
- zero-pending conversion.

Do not modify tests.

Specify the smallest deterministic future regression that should fail on the
current candidate if the manual observation is represented correctly.


FUTURE REGRESSION DESIGN

If a C# defect is confirmed, describe but DO NOT IMPLEMENT a deterministic
test such as:

1. canonical Level 7;
2. known Spiker identity;
3. deterministic firing state/random source;
4. allow first climb to consume/exhaust its firing allowance;
5. retreat while generic pending supply remains;
6. cross the lifecycle boundary into the next Spiker cycle;
7. verify the ROM-required firing state is refreshed;
8. allow second climb;
9. verify the same Spiker can fire again;
10. optionally repeat for a third cycle;
11. verify identity/physical occupancy remain conserved;
12. verify no new generic obligation is created;
13. verify zero-pending Tanker conversion remains unchanged.

Adapt this only to the actual mechanism established by source.


EVIDENCE CLASSIFICATION

Use:

ARCADE/MAME OBSERVED

ROM-BACKED

PROJECT-CURRENT

PROJECT TRANSLATION

INFERENCE

UNKNOWN

Do not label the suspected firing reset ROM-BACKED until an explicit source
path supports it.


OUTPUT / DECISION

Conclude with one of:

A. CONFIRMED NARROW DEFECT

The ROM explicitly refreshes or reinitializes firing state across a continuing
Spiker lifecycle, while C# fails to do so.

Recommend one small implementation milestone.

B. CONFIRMED DIFFERENT MECHANISM

The visible arcade/MAME behavior is established, but the ROM achieves it by a
different mechanism than Neal's reset hypothesis.

Explain the exact mechanism and recommend the smallest correction.

C. C# BEHAVIOR EXPLAINED BUT ROM REFRESH UNPROVEN

Current C# clearly exhausts firing state, but the local ROM listing does not
establish how later-cycle firing is restored.

Recommend the smallest additional evidence needed.

D. NO SOURCE-BACKED DEFECT FOUND

Only use this if the source truly fails to support a discrepancy.

Do not dismiss Neal's reproducible arcade/MAME observation; clearly state the
evidence conflict.


RECOMMENDED NEXT STEP

If the cause is established, recommend exactly ONE bounded implementation
milestone.

Do not combine it with:

- Tanker cargo allocation;
- Flipper movement;
- enemy bullet speed;
- Super Zapper;
- broader concurrency;
- Level 8 work.


VALIDATION

This is READ-ONLY.

Do not:

- modify production files;
- modify tests;
- modify durable documentation;
- run broad tests/builds;
- launch the interactive game;
- stage;
- commit;
- push.

Small read-only calculations and source searches are allowed.

At completion verify:

- HEAD remains the accepted Level 6 baseline;
- current uncommitted Level 7 candidate is unchanged;
- index unchanged;
- git diff --check passes;
- protected local files untouched.


REPORT

Preserve this prompt and create:

AI-Logs/Reports/2026-11-10-level7-spiker-repeated-cycle-firing-investigation-report.md

Include:

- Neal's reproducible arcade/MAME/C# observation;
- ROM firing-state trace;
- ROM repeat-cycle trace;
- C# firing-state trace;
- relationship to the current Spiker-persistence translation;
- exact confirmed cause or evidence gap;
- existing-test gap;
- future regression design;
- one recommended next milestone;
- confirmation that nothing was modified.


STOP

Stop after the read-only investigation/report.

Do not implement the correction.
Do not commit.
Do not push.
Do not begin the Tanker natural-vs-shot cargo investigation.
Do not begin Level 8 work.


TEMPEST_LEVEL7_SPIKER_REPEAT_FIRING_INVESTIGATION_COMPLETE