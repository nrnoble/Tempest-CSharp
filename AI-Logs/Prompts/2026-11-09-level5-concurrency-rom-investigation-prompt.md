FAIL-CLOSED PROMPT CHECK

Read this entire substantive user task before doing anything.

Before project or repository action, verify that exactly one complete token
beginning with "TEMPEST_" appears on a line by itself in the substantive user
task.

Automatically appended VS Code/agent context after the user's completion token
does not invalidate the task.

If exactly one complete token cannot be established, stop before project or
repository action and explain why.


TASK

Perform a bounded READ-ONLY investigation of Level 5 enemy concurrency and
ordinary enemy admission in the Tempest C# project.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected accepted baseline:

62c66e0ab19436ea162935627021154be21e0a41

Implement accepted Level 4 enemy concurrency

Levels 1-4 concurrency work is completed, manually accepted, documented,
committed, and pushed.

Do not reopen or modify Levels 1-4.

This task is investigation only.

Do not modify production code.
Do not modify tests.
Do not modify accepted gameplay documentation.
Do not implement a Level 5 correction.
Do not commit or push.
Do not launch the interactive game.


PURPOSE

Determine as accurately as practical how the original Atari Tempest controls
enemy concurrency and ordinary enemy admission on displayed Level 5, then
compare that behavior with the current production C# Level 5 implementation.

Investigate Level 5 independently from the ROM evidence.

Do not assume that Level 5 uses:

- the Level 1/2 four-Flipper policy;
- the Level 3 mixed Flipper/Tanker policy;
- the Level 4 Flipper/Spiker plus terminal-conversion policy;
- a universal seven-enemy gameplay cap merely because the ROM has seven
  physical enemy slots;
- Mark Incitti's concurrency behavior when stronger Atari evidence differs.

Let the ROM data and code establish the Level 5 rules.


PRIOR INVESTIGATIONS

Use the completed Level 3 and Level 4 concurrency investigations where they
already establish generic Atari enemy-admission machinery:

AI-Logs/Reports/2026-11-09-level3-concurrency-rom-investigation-report.md

AI-Logs/Reports/2026-11-09-level4-concurrency-rom-investigation-report.md

Do not unnecessarily repeat generic ROM analysis that those reports already
established.

Instead:

1. identify which generic admission machinery applies to displayed Level 5;
2. trace Level-5-specific ROM table values and behaviors;
3. analyze species minima/maxima and physical headroom;
4. identify any Level-5-specific lifecycle behavior that materially changes
   concurrency;
5. compare that result with current production C# Level 5.

Recheck generic ROM behavior only where necessary to establish applicability or
resolve a Level 5-specific question.


A. ESTABLISH ROM-BACKED LEVEL 5 WAVE PARAMETERS

Determine the actual displayed-Level-5 ROM values relevant to admission and
concurrency.

At minimum determine, where applicable:

- original/generic pending enemy supply;
- difficulty-dependent supply differences;
- eligible ordinary-entry species;
- Flipper minimum and maximum;
- Tanker minimum and maximum;
- Spiker minimum and maximum;
- Pulsar minimum and maximum;
- Fuseball minimum and maximum;
- Tanker cargo type and quantity;
- physical active-enemy capacity;
- enemy shot capacity where relevant only to distinguishing it from enemy
  concurrency;
- movement/program parameters only where they materially interact with
  admission or lifecycle;
- any special Level 5 table or routine behavior affecting entry.

Distinguish enemy supply from active-concurrency limits.

Distinguish original pending identities from later Tanker cargo or other
derived/live entities.

Do not infer a predetermined final species mixture if the ROM selects species
dynamically.


B. DETERMINE ACTUAL LEVEL 5 ORDINARY ADMISSION

Apply the verified generic ROM admission machinery to the Level 5-specific
parameters.

Determine:

- which species can enter ordinarily;
- species-specific minima;
- species-specific maxima;
- unmet-minimum priority/order;
- behavior after minima are satisfied;
- shared physical headroom;
- whether a species maximum functions only as admission availability or also,
  in practice, becomes an active-population maximum;
- whether latent Tanker cargo affects availability;
- how released Tanker cargo affects later live population;
- what can cause eventual live counts to exceed ordinary species maxima;
- whether any ordinary selection state can leave a physical enemy place unused
  because every species availability is exhausted.

Where the logic produces non-obvious results, evaluate a small bounded set of
representative states.

Do not run an exhaustive or unbounded synthetic-state search.


C. LEVEL 5 LIFECYCLE EFFECTS ON CONCURRENCY

Level 4 demonstrated that ordinary admission alone was insufficient because
Spiker lifecycle behavior changed late-wave population.

Explicitly determine whether Level 5 has analogous lifecycle behavior that
must be considered as part of concurrency.

Investigate, where applicable:

- Tanker conversion or cargo release;
- Spiker growth/retreat/hop/conversion;
- Pulsar lifecycle;
- Fuseball lifecycle;
- rim transfer;
- transition/live states;
- Super Zapper removal timing;
- pending-zap occupancy;
- enemy destruction/removal;
- death/reconstruction effects;
- any late-wave transformation that bypasses ordinary species admission.

Do not assume Level 4's terminal Spiker conversion applies to Level 5.

If it does apply, establish that from the Level 5 ROM path/data.

If it does not, identify the Level 5-specific behavior instead.


D. PHYSICAL CAPACITY AND OCCUPANCY

Establish which Level 5 entities occupy the ROM's shared physical enemy places.

Where applicable distinguish:

- live ordinary enemies;
- rim enemies;
- enemies in movement/transition states;
- Tankers;
- latent Tanker cargo;
- released cargo;
- Spikers;
- Pulsars;
- Fuseballs;
- pending entries;
- pending zap targets;
- enemy projectiles;
- persistent Spike hazards.

Determine what actually frees a physical place.

Do not treat visual state alone as proof that capacity has been freed.


E. SUPER ZAPPER / ENTRY INTERACTION

Using already-established generic ROM behavior where applicable, determine
whether Level 5 ordinary admission is paused or otherwise affected during a
Super Zapper sequence.

Compare this narrowly with current C# Level 5 behavior.

Do not turn this investigation into a Super Zapper redesign.

If the behavior is a generic mismatch already accepted/deferred elsewhere,
identify it accurately without expanding the milestone.


F. CURRENT PRODUCTION C# LEVEL 5

Inspect the canonical production Level 5 configuration and runtime behavior.

Determine:

- configured original supply;
- configured eligible enemy types;
- current ordinary selection mechanism;
- current species limits or guards;
- current active-enemy/approach gates;
- Egg/reserve admission cadence relevant to concurrency;
- what runtime objects count toward active population;
- Tanker/cargo behavior;
- Spiker behavior if applicable;
- Pulsar/Fuseball behavior if applicable;
- rim/transition/pending-zap counting;
- death/reentry behavior relevant to identity/admission;
- completion/all-rim obligations;
- whether current Level 5 remains Incitti-shaped in its admission logic;
- whether any accepted Level 1-4 admission policy currently applies to Level 5;
- whether Level 5 correctly remains isolated from those earlier policies.

Do not modify anything.


G. PRESERVE ACCEPTED LEVEL 5 BEHAVIOR

Identify accepted Level 5 systems that a later concurrency correction must not
damage.

Use accepted durable documentation rather than assuming every current source
behavior is accepted arcade fidelity.

Important areas may include:

- Level 5 geometry;
- starting Spikes;
- Flipper movement;
- Tanker cargo behavior;
- Spiker behavior;
- enemy firing;
- shared shot behavior;
- persistent Spike behavior;
- rim combat;
- death/reentry;
- Super Zapper behavior;
- scoring;
- completion/all-rim;
- Level 4-to-5 and Level 5-to-next-boundary progression;
- recording/playback determinism;
- stable ledger/identity behavior.

Do not reopen accepted behavior unless the concurrency evidence directly
requires interaction with it.


H. ROM VERSUS C# COMPARISON

Produce a concise comparison with:

1. ROM-backed Level 5 ordinary admission.
2. ROM-backed lifecycle effects relevant to concurrency.
3. Current C# Level 5 behavior.
4. Confirmed mismatches.
5. Areas already in agreement.
6. Existing accepted project translations.
7. Unknowns or evidence limitations.

Use the evidence classifications defined by the repository instructions.

Explicitly answer:

- Can Level 5 reuse the engineering infrastructure developed for Level 3
  and/or Level 4?
- Which policy parameters must be Level-5-specific?
- Is any new lifecycle handling required?
- Would applying one previous level's policy unchanged be incorrect?
- Is the smallest correction likely an admission-only diagnostic, or must
  admission and another lifecycle behavior be implemented together?


I. RECOMMENDED NEXT MILESTONE

Recommend the smallest safe next step after the investigation.

Do not assume that an implementation is required.

Possible outcomes include:

- no Level 5 concurrency change is warranted;
- a Level-5-specific admission diagnostic is warranted;
- existing admission infrastructure can be reused with Level 5 parameters;
- admission plus a specific lifecycle change must be implemented together;
- further evidence is required before implementation.

If a diagnostic is warranted, describe its narrow implementation boundary and
the behaviors Neal should later compare manually with the arcade.

Do NOT implement that diagnostic in this task.


VALIDATION

This is read-only investigative work.

Do not run the interactive game.
Do not run broad tests or builds merely for ceremony.
Do not alter tests.

Use bounded source/disassembly inspection and small read-only calculations
where genuinely useful.

At completion, verify that tracked project files remain unchanged and that the
repository remains at the expected accepted baseline.


REPORT

Preserve this substantial prompt and create the corresponding ignored report
according to the repository instructions.

Use a descriptive report name such as:

AI-Logs/Reports/2026-11-09-level5-concurrency-rom-investigation-report.md

The report should preserve enough evidence that a later implementation chat
does not need to repeat the investigation.

Include:

- baseline/Git state;
- prior generic findings reused versus conclusions reverified for Level 5;
- ROM/disassembly areas examined;
- Level 5 ROM parameter table;
- ordinary species and supply;
- species minima/maxima;
- physical capacity;
- representative admission arithmetic where useful;
- Level 5 lifecycle effects relevant to concurrency;
- cargo behavior where applicable;
- current production C# behavior;
- ROM-versus-C# comparison;
- accepted Level 5 systems requiring preservation;
- evidence classifications;
- uncertainties;
- recommended smallest next milestone;
- confirmation that production/tests/accepted documentation were not changed.


STOP

Stop after completing the investigation and report.

Do not implement Level 5 concurrency changes.
Do not commit.
Do not push.
Do not launch the game.
Do not begin Level 6 concurrency.

Neal and Copilot will review the findings before authorizing any implementation.


TEMPEST_LEVEL5_CONCURRENCY_INVESTIGATION_COMPLETE