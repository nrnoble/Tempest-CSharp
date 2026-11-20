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

Perform a bounded READ-ONLY investigation of Level 6 enemy concurrency,
ordinary admission, and concurrency-relevant enemy lifecycle behavior in the
Tempest C# project.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected accepted baseline:

94ad02f52f8fe7f1eec79d60ffd7d5114b8ee933

Implement accepted Level 5 enemy concurrency

Levels 1-5 concurrency work is completed, manually accepted, documented,
committed, and pushed.

Do not reopen or modify Levels 1-5.

This task is investigation only.

Do not modify production code.
Do not modify tests.
Do not modify accepted gameplay documentation.
Do not implement a Level 6 correction.
Do not commit or push.
Do not launch the interactive game.


PURPOSE

Determine as accurately as practical how the original Atari Tempest controls
enemy concurrency and ordinary enemy admission on displayed Level 6.

Then compare that behavior with the current canonical C# Level 6
implementation and recommend the smallest safe next milestone.

Treat Level 6 as its own gameplay program.

Do not assume Level 6 uses:

- the Level 1/2 four-Flipper rule;
- the Level 3 Flipper/Tanker policy;
- the Level 4 Flipper/Spiker policy;
- the Level 5 Tanker/Spiker policy;
- any previous level's species minima/maxima;
- any previous level's terminal lifecycle behavior;
- a universal active-enemy cap merely because physical slots are shared;
- Mark Incitti's behavior when stronger ROM or arcade evidence differs.

Reuse engineering understanding, not gameplay constants.


PRIOR CONCURRENCY INVESTIGATIONS

Use the existing Level 3-5 ROM investigations where they already establish
generic Atari machinery.

At minimum consult as useful:

AI-Logs/Reports/2026-11-09-level3-concurrency-rom-investigation-report.md

AI-Logs/Reports/2026-11-09-level4-concurrency-rom-investigation-report.md

AI-Logs/Reports/2026-11-09-level5-concurrency-rom-investigation-report.md

Do not repeat generic allocator/selector analysis merely for completeness.

Instead determine:

1. which previously established common ROM routines apply to Level 6;
2. which Level 6 table values feed those routines;
3. which Level 6 species/lifecycle behaviors alter the resulting concurrency;
4. where Level 6 materially differs from Levels 1-5.

Reverify generic machinery only when Level 6 evidence makes that necessary.


A. LEVEL 6 ROM PARAMETER TABLE

Determine the displayed-Level-6 ROM values relevant to concurrency.

Where applicable identify:

- Easy/Medium generic pending supply;
- Hard supply;
- ordinary selectable species;
- Flipper minimum and maximum;
- Tanker minimum and maximum;
- Spiker minimum and maximum;
- Pulsar minimum and maximum;
- Fuseball minimum and maximum;
- Tanker cargo type and quantity;
- physical enemy-place capacity;
- enemy-shot capacity, only to distinguish it from enemy concurrency;
- initial Spike parameters;
- Flipper movement program;
- other Level-6-specific values that materially affect admission or lifecycle.

Clearly distinguish:

- generic pending supply;
- active physical enemy population;
- latent cargo;
- released cargo;
- derived/transformed enemies;
- project stable identities.

Do not infer a fixed final species recipe if the ROM dynamically selects
species.


B. LEVEL 6 ORDINARY ADMISSION

Apply the verified shared ROM admission machinery to Level 6's actual
parameters.

Determine:

- which species can enter ordinarily;
- species minima and maxima;
- unmet-minimum priority;
- behavior after minima are satisfied;
- physical-headroom interaction;
- which positive availability values are actually selectable;
- whether any zero-minimum species are excluded despite positive availability;
- whether Tanker latent cargo affects availability;
- whether species availability can stop ordinary admission with physical
  places unused;
- what events reopen admission.

Where useful, calculate a small bounded set of representative occupancy states.

Do not perform an exhaustive state-space search.

Avoid reducing the result to phrases such as "four-Flipper cap" or
"seven-enemy cap" unless the Level 6 ROM evidence literally establishes that.


C. LEVEL 6 TANKER / CARGO BEHAVIOR

If Level 6 uses Tankers, determine:

- ordinary Tanker eligibility;
- Tanker minima/maxima;
- cargo type;
- cargo quantity;
- how latent cargo influences species availability;
- whether released cargo bypasses selection;
- physical-slot behavior at release;
- whether carrier removal plus cargo materialization can increase live
  population;
- whether cargo can cause species counts to exceed ordinary admission maxima;
- whether terminal/late lifecycle creates additional Tankers.

Separate literal ROM physical-allocation behavior from the project's accepted
complete-pair cargo translation.

Do not reopen accepted complete-pair behavior during this investigation.


D. LEVEL 6 SPIKER / SPIKE LIFECYCLE

If Level 6 uses Spikers, establish whether the shared Spiker action investigated
on Levels 4 and 5 applies.

Determine:

- grow/retreat behavior relevant to concurrency;
- whether the live Spiker keeps its physical slot through retreat/hop;
- behavior while generic pending supply remains;
- behavior when generic pending supply reaches zero;
- whether terminal Spiker-to-Tanker conversion applies on Level 6;
- cargo assigned by such a conversion;
- whether conversion can raise Tanker population beyond ordinary Tanker maxima;
- whether Level-6-specific data changes this path.

Do not assume Level 4/5 terminal conversion merely because they use the same
enemy class.

Establish applicability from the ROM.


E. PULSAR / FUSEBALL AND OTHER LIFECYCLE EFFECTS

If Pulsars, Fuseballs, or another enemy type are present on Level 6, determine
their concurrency relevance.

For each applicable type establish:

- ordinary selection eligibility;
- minima/maxima;
- physical-slot occupancy;
- transitions/lifecycle states;
- whether lane changes or hazard phases preserve occupancy;
- whether any transformation bypasses ordinary selection;
- what actually frees the physical enemy place.

Do not investigate visual/audio fidelity unrelated to concurrency.


F. DEATH / RECONSTRUCTION

Level 5 established that death reconstruction can be critical to visible
concurrency behavior.

Determine the ROM-backed Level 6 death/reconstruction consequence.

Specifically assess:

- whether active Level 6 enemies return to generic pending supply;
- whether species identity survives;
- how Tanker cargo contributes to reconstructed unresolved quantity;
- how latent versus materialized cargo should be understood;
- whether Level 6 has any species-specific reconstruction consequence;
- whether reconstructed generic obligations subsequently use normal Level 6
  selection.

Compare this with the current C# Level 6 stable-ledger reconstruction behavior.

Do not assume Level 5's accepted C# generic-obligation translation should be
copied to Level 6 merely because the ROM uses shared reconstruction machinery.

Identify whether an analogous correction is actually required and what arcade
behavior would need manual verification.


G. PHYSICAL CAPACITY

Determine precisely what Level 6 counts as occupying shared physical enemy
capacity.

Where applicable distinguish:

- climbing enemies;
- rim enemies;
- transitioning enemies;
- Tankers;
- Spikers;
- Pulsars;
- Fuseballs;
- materialized cargo;
- latent cargo;
- pending zap targets;
- Eggs/pending originals;
- enemy bullets;
- persistent Spikes/hazards.

Determine exactly what frees capacity.

Do not infer release merely from a visual state transition.


H. SUPER ZAPPER / ENTRY INTERACTION

Determine whether Level 6 uses the shared ROM entry pause while zap_running is
active.

Compare this with current C# behavior only as needed to classify the mismatch.

Do not broaden this investigation into a Super Zapper redesign.

The equivalent Level 5 mismatch was deliberately deferred at Level 5 closeout.
Do not automatically reopen that accepted deferment for earlier levels.


I. CURRENT C# LEVEL 6

Inspect canonical production Level 6.

Determine:

- configured original supply;
- eligible species;
- initial Eggs/reserve configuration;
- approach/hatch scheduling;
- species selector currently used;
- current species caps/guards;
- active-population conditions;
- physical-headroom enforcement, if any;
- Tanker/cargo handling;
- Spiker/Spike behavior;
- Pulsar/Fuseball behavior where applicable;
- terminal conversion behavior;
- death/reconstruction behavior;
- Super Zapper interaction;
- rim/transition/pending-zap occupancy;
- completion/all-rim obligations;
- whether any accepted Level 1-5 concurrency policy currently applies;
- whether Level 6 remains on older Incitti-shaped/shared selection logic.

Do not change anything.


J. ACCEPTED LEVEL 6 BEHAVIOR TO PROTECT

Use TEMPEST-DEVELOPMENT-NOTES.md and TEMPEST-BEHAVIOR-SPEC.md to identify
already accepted Level 6 behavior that a later correction must preserve.

At minimum consider, where documented:

- Level 6 geometry;
- natural Level 5-to-6 progression;
- enemy composition;
- movement;
- Tanker cargo;
- Spiker/Spike behavior;
- firing;
- Super Zapper behavior;
- death/reentry;
- rim combat;
- completion/progression;
- score/lives carry;
- deterministic recording/playback.

Do not treat every current source behavior as accepted merely because it exists.

Preserve the distinction between accepted gameplay and implementation detail.


K. ROM VERSUS C# COMPARISON

Produce a concise comparison addressing:

1. ROM-backed Level 6 ordinary admission.
2. ROM-backed concurrency-relevant lifecycle behavior.
3. ROM-backed death/reconstruction behavior.
4. Current canonical C# Level 6 behavior.
5. Confirmed mismatches.
6. Areas already aligned.
7. Existing accepted project translations.
8. Unknowns requiring further evidence.

Use repository evidence classifications.

Explicitly answer:

- Can Level 6 reuse engineering infrastructure from Levels 3-5?
- Which rules/parameters must remain Level-6-specific?
- Would copying any previous level's policy unchanged be incorrect?
- Is an admission-only diagnostic sufficient?
- Must admission be coupled with Spiker/cargo/death or another lifecycle
  correction?
- Is a Level-6-specific generic-obligation death translation likely required?
- Which visible arcade behaviors should Neal later verify?


L. RECOMMENDED NEXT MILESTONE

Recommend the smallest safe next step.

Do not assume code must change.

Possible outcomes include:

- no Level 6 change required;
- Level-6-specific admission diagnostic;
- admission plus terminal lifecycle diagnostic;
- admission plus death-reconstruction diagnostic;
- telemetry/observation required before implementation;
- another evidence-driven bounded milestone.

If implementation is warranted, describe exactly which previously developed
infrastructure can be reused while keeping Level 6 gameplay rules explicit.

Do NOT implement it in this task.


VALIDATION

This is read-only.

Do not launch the game.
Do not run gameplay tests.
Do not run broad test/build suites merely for ceremony.
Do not modify tests.

Use bounded source/disassembly inspection and small read-only calculations when
useful.

At completion verify:

- no production changes;
- no test changes;
- no durable documentation changes;
- no staged files;
- HEAD remains at the expected baseline;
- git diff --check passes;
- local protected material remains untouched.


REPORT

Preserve the substantial prompt and create the paired ignored investigation
report according to repository instructions.

Use a descriptive report name such as:

AI-Logs/Reports/2026-11-10-level6-concurrency-rom-investigation-report.md

Include:

- starting baseline/Git state;
- generic ROM machinery reused from prior investigations;
- Level 6 ROM parameter table;
- ordinary species eligibility;
- minima/maxima;
- physical capacity;
- bounded representative admission states;
- Tanker/cargo behavior;
- Spiker lifecycle;
- Pulsar/Fuseball lifecycle where applicable;
- death/reconstruction findings;
- Super Zapper entry interaction;
- current C# Level 6 behavior;
- accepted Level 6 behavior to protect;
- ROM-versus-C# comparison;
- evidence classifications;
- uncertainties;
- recommended smallest next milestone;
- confirmation that no implementation, tests, documentation, commit, push,
  or game launch occurred.


STOP

Stop after the investigation and report.

Do not implement Level 6 concurrency.
Do not commit.
Do not push.
Do not launch the game.
Do not begin Level 7 concurrency.

Neal and Copilot will review the Level 6 evidence before authorizing the next
milestone.


TEMPEST_LEVEL6_CONCURRENCY_INVESTIGATION_COMPLETE