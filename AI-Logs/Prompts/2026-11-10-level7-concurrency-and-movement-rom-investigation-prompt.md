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

Perform a bounded READ-ONLY Level 7 ROM investigation covering:

1. enemy concurrency and ordinary admission;
2. concurrency-relevant enemy lifecycle;
3. death/reconstruction behavior;
4. Level-7-specific Flipper movement, including Tanker cargo movement paths.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected accepted baseline:

8f1c29a7d3e6ef4150c8ab64264108c3c2a9547b

Implement accepted Level 6 concurrency and cargo movement

Levels 1-6 concurrency work is completed, manually accepted, documented,
committed, and pushed.

Do not reopen or modify Levels 1-6.

This task is investigation only.

Do not modify production code.
Do not modify tests.
Do not modify accepted gameplay documentation.
Do not implement Level 7 corrections.
Do not commit.
Do not push.
Do not launch the interactive game.


PURPOSE

Determine the Atari ROM-backed Level 7 rules needed before any concurrency or
movement correction is authorized.

Treat Level 7 as its own gameplay program.

Do not assume Level 7 behaves like Level 6 or any earlier tube.

The investigation must explicitly cover BOTH concurrency and Flipper movement.

Level 6 demonstrated that a generic manual check such as "Flippers move
normally" is insufficient when the ROM defines a tube-specific movement
program.

For Level 7, identify the specific ROM movement behavior first so Neal later
receives a concrete movement-pattern acceptance checklist.


PRIOR LEVEL 7 WORK

Level 7 already has substantial existing project history.

Consult the relevant existing Level 7 reports and durable documentation before
new analysis, including where available:

- Level 7 preimplementation analysis;
- Level 7 Flipper ROM analysis;
- Level 7 implementation/acceptance reports;
- TEMPEST-DEVELOPMENT-NOTES.md;
- TEMPEST-BEHAVIOR-SPEC.md.

Do not assume those historical investigations answered the current concurrency
question.

Do not discard previously accepted Level 7 behavior merely because this
investigation uses newer ROM evidence.

Identify what is already strongly established and what still requires
investigation.


PRIOR GENERIC CONCURRENCY WORK

Reuse the generic ROM machinery already established during Levels 3-6.

Do not repeatedly reverse engineer:

- the seven physical enemy slots;
- common species availability calculation;
- minima/maxima mechanics;
- generic pending supply;
- shared allocation mechanics;
- general death reconstruction;
- general terminal Spiker machinery;

unless Level 7 introduces a material difference.

Concentrate on Level-7-specific data and behavior.


A. LEVEL 7 ROM PARAMETER TABLE

Determine the displayed-Level-7 ROM values relevant to gameplay.

At minimum identify:

- Easy/Medium generic pending supply;
- Hard supply;
- Flipper minimum/maximum;
- Tanker minimum/maximum;
- Spiker minimum/maximum;
- Pulsar minimum/maximum;
- Fuseball minimum/maximum;
- ordinary selectable species;
- Tanker cargo type and quantity;
- physical enemy capacity;
- enemy-shot capacity;
- initial Spike parameters;
- Flipper movement program;
- any additional Level 7 table entries materially affecting admission,
  movement, or lifecycle.

Distinguish generic pending supply from cargo-derived population.


B. ORDINARY LEVEL 7 ADMISSION

Apply the shared ROM selector to Level 7's values.

Determine:

- which species can enter ordinarily;
- which species have zero minima and therefore are not normally selectable;
- ordinary minima/maxima;
- unmet-minimum order;
- selection after minima are satisfied;
- physical headroom;
- cargo availability interaction;
- whether admission can stall with physical places unused;
- what conditions reopen admission.

Calculate a small bounded set of representative states where they materially
clarify the behavior.

Do not infer a fixed final enemy recipe.


C. LEVEL 7 TANKER / CARGO CONCURRENCY

Determine:

- ordinary Tanker availability;
- cargo type/quantity;
- latent cargo effect on availability;
- physical occupancy;
- materialized cargo behavior;
- whether released cargo can exceed ordinary species maxima;
- whether terminal transformations can increase Tanker population beyond
  ordinary maxima;
- literal ROM saturated child-allocation behavior.

Preserve the project's accepted complete-pair cargo translation as a separate
PROJECT TRANSLATION unless current evidence requires reconsideration.

Do not alter it during this investigation.


D. SPIKER / SPIKE LIFECYCLE

Determine whether Level 7 uses the shared Spiker grow/retreat/hop/terminal
conversion machinery.

Identify:

- behavior while generic pending supply remains;
- behavior at actual pending zero;
- whether same-slot Tanker conversion applies;
- resulting cargo;
- concurrency effects;
- whether a temporary species dead-end differs from true zero pending supply.

Distinguish ROM lifecycle behavior from existing C# Level 7 implementation.


E. PULSAR / FUSEBALL LIFECYCLE

If Level 7 includes either species, explicitly investigate concurrency-relevant
behavior.

Determine:

- ordinary eligibility;
- minima/maxima;
- physical occupancy;
- lifecycle states that remain in the shared slot;
- any transformation or release path influencing population.

Do not investigate visual/audio fidelity unrelated to concurrency.


F. DEATH / GENERIC OBLIGATION RECONSTRUCTION

Determine the Level 7 consequence of the shared ROM death path.

Explicitly answer:

- how unresolved active enemies contribute to post-death generic supply;
- whether pre-death species commitments survive in the ROM;
- treatment of a Tanker with latent cargo;
- treatment of materialized cargo children;
- treatment of rim enemies;
- treatment of derived/converted Tankers;
- subsequent Level 7 species selection after reconstruction.

Compare carefully with current canonical C# Level 7 behavior.

Level 5 and Level 6 generic-obligation translations are engineering precedent,
not automatic authorization to copy them unchanged.

Identify the exact Level 7 manual arcade checks Neal should later perform.


G. PHYSICAL OCCUPANCY

For every Level 7 species/state, determine whether it occupies a shared enemy
place.

Distinguish:

- climbing enemies;
- rim enemies;
- transitions;
- Tankers;
- Spikers;
- Pulsars;
- Fuseballs;
- latent cargo;
- materialized cargo;
- pending-zap targets;
- generic pending entries/Eggs;
- enemy bullets;
- persistent Spikes/hazards.

Determine what actually releases capacity.


H. SUPER ZAPPER ENTRY INTERACTION

Determine whether Level 7 uses the shared ROM zap_running admission pause.

Classify the current C# difference if present.

Do not redesign Super Zapper.

Levels 5 and 6 deliberately deferred this fidelity boundary. Do not silently
change that policy during a Level 7 concurrency investigation.


I. LEVEL 7 FLIPPER MOVEMENT PROGRAM

This section is REQUIRED even if current Level 7 movement already looks good.

Identify the exact ROM flipper_move value for displayed Level 7.

Decode its actual ROM p-code/program behavior sufficiently to state a concrete
movement pattern.

Do not merely record the movement byte.

Determine where evidence permits:

- number/cadence of straight along-tube movement operations;
- flip timing;
- whether depth advances while flipping;
- lane-travel direction behavior;
- scripted reversals, if any;
- repeat cycle;
- rim/top handoff;
- open-end correction where applicable.

Level 7 historical arcade observation has previously indicated a characteristic
below-rim Flipper pattern. Compare that prior observation with the ROM program
rather than assuming either automatically controls.


J. TANKER-CARGO FLIPPER MOVEMENT

Explicitly trace Tanker cargo.

Determine whether Level 7 Tanker cargo:

- uses the current level's Flipper movement program;
- uses a depth-dependent alternate program;
- has a special cargo-specific pattern;
- differs between player-shot and natural release;
- starts its two children with opposite directions;
- reverses direction according to the level movement program;
- preserves or changes direction at each phase.

Do not assume Level 6's $0b/$07 depth distinction exists on Level 7.

Establish Level 7 behavior independently.


K. COMPARE WITH CURRENT C# LEVEL 7 MOVEMENT

Trace current canonical Level 7 Flipper and Tanker-cargo movement.

Determine:

- what movement mode/program C# currently uses;
- whether ordinary and cargo Flippers differ;
- whether current C# behavior is explicitly ROM-derived;
- whether it implements the Level 7 ROM program;
- whether any accepted historical Level 7 movement approximation differs from
  the ROM.

Classify:

ROM-BACKED
PROJECT-CURRENT
PROJECT TRANSLATION
ARCADE-OBSERVED
INFERENCE
UNKNOWN

Do not call current movement ROM-backed solely because Neal previously judged
it correct.


L. CONCRETE FUTURE MANUAL MOVEMENT CHECK

Based on the ROM program, provide a short, specific future manual checklist.

Avoid vague language such as:

- "Flippers move normally";
- "cargo behaves naturally";
- "no obvious movement regression."

Instead state the actual distinguishing pattern Neal should observe.

Examples of the needed specificity:

- number/cadence of flips between straight intervals;
- whether Flippers climb during a flip;
- when direction reverses;
- whether opposite cargo children remain/opposite or later reverse;
- whether deep versus near-rim release differs.

Only include behaviors actually established by the Level 7 evidence.


M. CURRENT C# LEVEL 7 CONCURRENCY

Inspect canonical Level 7.

Determine:

- configured supply;
- eligible species;
- ordinary selector;
- existing species caps/guards;
- Egg/reserve scheduling;
- physical headroom;
- cargo handling;
- Spiker terminal behavior;
- death reconstruction;
- Super Zapper interaction;
- completion/all-rim;
- whether an existing Level 7 ROM policy already implements part of the
  desired behavior;
- which current behaviors are historical project approximations.


N. ACCEPTED LEVEL 7 BEHAVIOR TO PROTECT

Use durable documentation and historical acceptance reports.

Identify already accepted Level 7 behavior, including as applicable:

- 16-lane clover geometry;
- supply;
- initial Eggs;
- Spikes;
- accepted Flipper pattern;
- Tanker behavior;
- cargo behavior;
- Spiker behavior;
- firing;
- Super Zapper;
- death/reentry;
- rim combat;
- completion;
- progression;
- recording/playback.

Do not reopen these unless current evidence identifies a specific conflict
relevant to this milestone.


O. ROM VERSUS C# COMPARISON

Produce a concise comparison covering:

1. Level 7 ordinary admission.
2. Species minima/maxima.
3. Physical capacity.
4. Tanker/cargo lifecycle.
5. Spiker terminal lifecycle.
6. Death reconstruction.
7. Super Zapper entry behavior.
8. Level 7 Flipper movement program.
9. Tanker-cargo movement.
10. Current C# equivalents.
11. Confirmed mismatches.
12. Existing accepted translations.
13. Unknowns.

Explicitly answer:

- Can Level 7 reuse Level 3-6 admission infrastructure?
- Which rules must remain Level-7-specific?
- Is any existing Level 7 ROM policy already close to the target?
- Is admission-only work sufficient?
- Is death reconstruction work required?
- Is Flipper movement already correct or does it need a separate correction?
- Can concurrency and movement safely be implemented in one later milestone,
  or should they remain separate?


P. RECOMMENDED NEXT MILESTONE

Recommend the smallest safe next step.

Do not implement it.

Possible recommendations include:

- no Level 7 concurrency change required;
- bounded admission diagnostic;
- admission plus lifecycle/death diagnostic;
- separate movement correction;
- combined Level 7 fidelity candidate;
- additional arcade observation before implementation.

Be conservative about changing previously accepted Level 7 movement unless a
specific mismatch is established.


VALIDATION

Read-only only.

Do not:

- launch the game;
- modify production code;
- modify tests;
- modify durable documentation;
- run broad test/build suites;
- stage;
- commit;
- push.

Use bounded local source/disassembly/document inspection.

At completion verify:

- HEAD remains at expected baseline;
- index clean;
- tracked worktree clean;
- git diff --check passes;
- protected files untouched.


REPORT

Preserve this prompt and create:

AI-Logs/Reports/2026-11-10-level7-concurrency-and-movement-rom-investigation-report.md

Include:

- starting baseline;
- ROM Level 7 parameter table;
- concurrency/admission findings;
- cargo behavior;
- Spiker/Pulsar/Fuseball lifecycle;
- death reconstruction;
- Super Zapper boundary;
- exact Level 7 Flipper movement program;
- decoded movement pattern;
- Tanker-cargo movement path;
- current C# comparison;
- concrete future manual movement checklist;
- accepted behavior to protect;
- evidence classifications;
- unknowns;
- recommended next milestone;
- confirmation nothing was changed.


STOP

Stop after the investigation/report.

Do not implement Level 7 changes.
Do not commit.
Do not push.
Do not begin Level 8 concurrency.


TEMPEST_LEVEL7_CONCURRENCY_MOVEMENT_INVESTIGATION_COMPLETE