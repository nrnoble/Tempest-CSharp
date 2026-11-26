# Tempest Levels 11 / 15 / 17 Behavioral Special-Case and Generic-Lifecycle Bypass Audit

## FAIL-CLOSED COMPLETION-TOKEN CHECK

Read the entire user-provided task before taking any action.

Before tools, files, commands, modifications, tests, builds, repository actions, or temporary experiments:

1. Locate the completion token near the end of this user-provided task.
2. Verify that exactly one complete token beginning with `TEMPEST-END-` appears on a line by itself in the user-provided task.
3. The token may contain only uppercase ASCII letters, digits, and hyphens after the `TEMPEST-END-` prefix.
4. VS Code or agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context appearing after the user's completion token do not invalidate it.
5. This check exists only to detect an incomplete or truncated user task.
6. If exactly one complete standalone token cannot be found, STOP before any project or repository action and explain why.

The unique completion token is the final standalone line below.

---

# Task

Perform a **strictly read-only architectural audit** of Levels 11, 15, and 17.

The objective is narrowly defined:

> Determine whether production code contains behavioral/lifecycle logic specific to Levels 11, 15, or 17 because the normal shared implementation used by the other accepted levels could not support those levels.

The concern is **not** legitimate level-specific ROM configuration.

The concern is code that acts as a workaround or bypass for a generic behavior/lifecycle implementation that should normally work for all applicable levels.

Examples of legitimate differences include:

- ROM-selected enemy eligibility;
- ROM min/max population;
- different ROM Flipper movement programs;
- different speed/timing parameters;
- different firing parameters;
- different Tanker cargo types;
- different Spike eligibility;
- palette cycle;
- tube-shape selection.

Examples of concerning differences include:

- a special Level 11 rim-transition function because generic rim transition fails;
- a special Level 15 reconstruction path because generic reconstruction fails;
- a Level 17-only Flipper lifecycle to compensate for shared lifecycle limitations;
- duplicated Tanker cargo/release logic;
- level-specific ledger rules;
- level-specific completion rules;
- level-specific rendering used to compensate for incorrect simulation state;
- per-level actor implementations that duplicate what should be one shared actor lifecycle.

This is an audit only.

**Do not modify production code.\
Do not modify tests.\
Do not modify durable documentation.\
Do not refactor.\
Do not fix anything.\
Do not create temporary production variants.\
Do not stage, commit, or push.\
Do not implement or investigate Level 18+.\
Do not launch the game.**

Stop after producing the audit report and recommendations.

---

# Current accepted production state

Levels 1–17 are the current accepted production foundation.

Level 17 has passed final manual acceptance.

Neal reports that after the final Level 17 corrections:

- no current Level 17 defect is known;
- Level 17 has no noticeable difference from arcade gameplay/presentation that Neal can presently identify;
- Levels 1–16 recorded playback remains correct.

Do not reopen accepted behavior merely because an implementation looks unusual.

The purpose is to determine whether unusual implementation is:

- justified;
- harmless architecture specialization;
- technical debt;
- or an actual generic-lifecycle bypass/hack.

---

# Required project reports

Read the durable documentation and relevant historical reports necessary to understand why Levels 11, 15, and 17 evolved differently.

At minimum read:

`TEMPEST-DEVELOPMENT-NOTES.md`

`TEMPEST-BEHAVIOR-SPEC.md`

Relevant Level 11 reports, particularly reports concerning:

- Flipper lifecycle;
- Tanker cargo;
- runtime representation;
- ledger integration;
- death/reconstruction;
- rim behavior.

Relevant Level 15 reports concerning:

- Level 11 parameter-development baseline reuse;
- repeated Level 11-derived defects;
- production closeout.

Relevant Level 17 reports:

`AI-Logs/Reports/2026-11-25-level17-bounded-production-implementation-report.md`

`AI-Logs/Reports/2026-11-25-level17-levels1-16-shared-regression-investigation-report.md`

`AI-Logs/Reports/2026-11-25-level17-shared-regression-bounded-correction-report.md`

`AI-Logs/Reports/2026-11-25-level17-frozen-rim-flipper-correction-report.md`

`AI-Logs/Reports/2026-11-25-level17-final-closeout-report.md`

Use historical reports to understand design provenance.

Do not assume every historical workaround remains active in current production.

Trace current code.

---

# User concern and audit standard

Neal's concern is:

> If Levels 11, 15, or 17 require special functions/objects because the global/shared behavior used by other levels does not work for them, that is potentially a hack and should be identified before development continues.

Apply this standard carefully.

A named level-specific **configuration** is fine.

A named implementation of a genuinely distinct **ROM behavior program** may also be fine.

A special implementation that duplicates or bypasses generic lifecycle behavior merely because the generic implementation cannot handle the level is concerning.

Do not classify something as a hack solely because its type or function name contains `Level11`, `Level15`, or `Level17`.

Classify by responsibility and cause.

---

# Durable architectural principles already accepted

Use these as audit criteria.

## Eligibility before parameters

Features/enemies are absent unless ROM eligibility enables them.

Parameters are applied only after eligibility is established.

## Shared core behavior

Enemy and gameplay-object core behavior remains shared unless ROM evidence establishes a genuinely distinct behavior mode.

## Shared rim lifecycle

A level-specific below-rim Flipper movement program should hand off to the common rim lifecycle at rim arrival unless ROM evidence establishes otherwise.

## Reusable tube geometry

The canonical 16 shapes are reused across later levels.

## Cycle-based semantic palette

Palette cycle and semantic color roles remain independent of tube geometry and gameplay configuration.

---

# Comparison population

Use accepted Levels 1–17.

Treat:

## Target levels

- Level 11
- Level 15
- Level 17

## Control levels

Use appropriate accepted levels among:

- Levels 1–10;
- Levels 12–14;
- Level 16.

Do not blindly compare every target to every control.

Choose control levels that exercise comparable mechanics:

- Flippers;
- Tankers;
- Tanker cargo;
- Spikers where relevant;
- Fuseballs where relevant;
- Pulsars where relevant;
- rim pursuit;
- death/reconstruction;
- production ledger;
- completion.

The comparison should answer:

```
Normal accepted levels use shared path X.

Level 11 uses path Y.
Why?

Level 15 uses path Z.
Why?

Level 17 uses path Q.
Why?
```

---

# Classification system

Every target-level specialization found must be placed into exactly one primary category.

## A. ROM-JUSTIFIED SPECIALIZATION

Distinct code/configuration exists because authoritative ROM behavior is genuinely different.

Examples:

- `$07` versus `$24` movement program;
- different species eligibility;
- different cargo selection;
- different firing rule.

This is not a hack if implemented as a reusable distinct behavior strategy.

## B. PROJECT-ARCHITECTURE SPECIALIZATION

A separate engineering abstraction exists, but it still feeds into the same generic actor lifecycle and does not duplicate/bypass generic responsibilities.

This may be acceptable, but evaluate whether naming/scope/generalization should be improved.

## C. HISTORICAL / OBSOLETE SPECIALIZATION

Code/comment/type remains from earlier development but no longer affects current production or has become redundant.

Identify cleanup opportunities, but do not remove them.

## D. POTENTIAL GENERIC-LIFECYCLE BYPASS / HACK

Behavior-specific production code exists because the shared implementation does not correctly support the target level.

Warning examples:

- duplicate actor lifecycle;
- duplicate rim transition;
- duplicate cargo release;
- duplicate death/reconstruction;
- separate ledger;
- separate completion system;
- per-level collision semantics without ROM basis;
- per-level rendering workaround for simulation state;
- special-case branch that repairs generic behavior only for one displayed level;
- copied methods differing only by a level check;
- state flags whose purpose is to bypass generic paths.

This is the category requiring possible remediation.

## E. UNRESOLVED

Available evidence is insufficient to classify safely.

Do not force certainty.

---

# Audit area 1: level configuration versus actor behavior

Inventory all current Level 11, 15, and 17 configuration markers.

Examples:

- `UsesLevel11RomWavePolicy`
- `UsesLevel11ParameterDevelopmentBaseline`
- Level 17 production-policy flags;
- movement-program selectors;
- route enums;
- palette/shape data;
- eligibility data.

For each determine:

- Is it data/configuration only?
- Does it select a reusable strategy?
- Does core actor code branch directly on displayed level?
- Does it cause lifecycle logic to diverge?

Prefer behavior keyed by semantic strategy/configuration rather than displayed-level integer.

List every production `if/switch` directly checking 11, 15, or 17 where practical.

Classify each.

---

# Audit area 2: Flipper construction

Trace Flipper creation for controls and targets.

Compare:

- ordinary original Flippers;
- Tanker-cargo Flippers;
- reconstructed Flippers;
- direct factory creation;
- Egg hatch creation.

Determine whether Levels 11/15/17:

- construct a different actor type;
- initialize different lifecycle fields;
- bypass generic factory logic;
- set hidden flags unrelated to ROM movement program;
- require special correction after construction.

Separate legitimate route/program selection from actor-lifecycle divergence.

---

# Audit area 3: below-rim movement programs

Inventory all current production below-rim Flipper movement strategies.

For each strategy record:

- semantic name;
- ROM program/authority;
- levels using it;
- whether ordinary and cargo Flippers can use it;
- fields it owns;
- termination condition;
- whether it owns only below-rim motion or additional lifecycle state.

Pay particular attention to:

- Level 11 repeating `$24`;
- Level 15's relationship to the Level 11 development baseline;
- Level 17 straight `$07`;
- generic `Straight07`;
- `Level17Straight07`;
- any special endpoint-hinge path.

Determine whether multiple implementations represent:

- genuinely distinct ROM programs;
- historical duplicate translations;
- target-level workarounds.

---

# Audit area 4: rim-entry handoff

Trace every Flipper below-rim strategy into rim arrival.

Determine:

- which method detects rim arrival;
- which method clamps depth;
- which method clears below-rim ownership;
- which method initializes pursuit direction;
- which method enters pause;
- which method begins pivot;
- whether every strategy converges onto the same shared rim-entry function.

Produce a matrix:

Strategy | Levels | Rim detector | Handoff method | Shared pause | Shared pivot | Shared pursuit | Exception

Any Level 11/15/17 strategy that bypasses the shared handoff must be justified by ROM evidence or classified as potential hack.

---

# Audit area 5: rim pursuit

Trace shared Flipper behavior after rim arrival.

Compare targets and controls for:

- lane selection;
- direction;
- `RimPursuitDirection`;
- player-relative pursuit;
- pause timing;
- pivot/flip;
- open versus closed tube behavior;
- collision;
- shooting/destroyability;
- edge/open-tube limits.

Determine whether Level 11/15/17 contain special rim-pursuit branches.

If so, establish whether each is:

- ROM-supported;
- topology-supported;
- architecture workaround.

Do not modify anything.

---

# Audit area 6: Tanker core behavior

Trace Tanker construction and lifecycle across levels.

Compare:

- entry;
- movement;
- firing;
- destruction;
- cargo ownership;
- cargo release;
- latent cargo;
- completion;
- reconstruction.

Determine whether Level 11/15/17 instantiate:

- a different Tanker actor;
- level-specific cargo-release logic;
- special identity paths;
- special movement behavior beyond configuration.

Identify branches involving target-level flags.

---

# Audit area 7: Tanker cargo

This is high priority because Levels 11/15/17 recently exposed cargo-route regressions.

Trace:

- cargo identity allocation;
- cargo type;
- owner/ordinal tracking;
- latent state;
- release condition;
- released-actor construction;
- below-rim route assignment;
- runtime representation;
- destruction;
- death/re-entry reconstruction;
- completion.

Determine specifically:

- what `Straight07` currently means;
- what `Level17Straight07` currently means;
- why both exist;
- whether each represents a true reusable semantic behavior;
- whether either includes responsibilities outside route selection;
- whether Level 11/15 depend on historical semantics that should instead have a clearer reusable name.

Do not rename/refactor.

---

# Audit area 8: identity ledger

Trace how current original/cargo identities work across target and control levels.

Determine whether target levels have:

- separate identity state;
- extra level-specific identity fields;
- special missing-runtime handling;
- special commit logic;
- separate unresolved-set behavior;
- special terminal transitions.

Identify every target-level branch in ledger code.

Classify whether it is ROM configuration, architecture specialization, obsolete history, or bypass.

The desired architecture is one generic identity lifecycle parameterized by species/cargo semantics.

---

# Audit area 9: death and reconstruction

Compare target/control levels for:

- active actor reconstruction;
- reserve;
- Egg;
- cargo;
- latent cargo;
- pending activation;
- rim actors;
- fresh runtime state;
- terminal identities.

Look for branches that mention:

- Level 11;
- Level 15;
- Level 17;
- `$24`;
- `$07`;
- special cargo types.

Determine whether reconstruction knows about semantic actor states or improper displayed-level identities.

A movement program should not normally require a completely separate death/reconstruction engine.

---

# Audit area 10: completion

Trace completion requirements.

Determine whether Levels 11/15/17 use:

- generic unresolved identities;
- generic runtime representation;
- generic reserve/Egg/cargo handling;
- special completion counters;
- per-level completion branches.

Any specialized completion behavior must have explicit ROM/gameplay justification.

Look particularly for fixes created after earlier Level 11/15 bugs.

---

# Audit area 11: firing

Compare firing logic across targets and controls.

Determine whether target levels use:

- regular shared shooter eligibility;
- configuration-dependent holdoff/capacity;
- special species paths;
- level-specific firing workarounds.

Different ROM firing parameters are configuration, not hacks.

Duplicated shooter logic is potentially concerning.

---

# Audit area 12: collision and destruction

Trace:

- player-shot hit;
- player collision;
- enemy destruction;
- score;
- death;
- rim clearing.

Identify target-level special cases.

Determine whether any exist because generic lifecycle/state representation fails for target levels.

---

# Audit area 13: rendering dependencies on behavior

Inspect only behavioral coupling, not presentation fidelity.

Determine whether rendering contains Level 11/15/17 checks used to:

- hide invalid gameplay states;
- select alternate actor representation because generic simulation state is malformed;
- compensate for lifecycle differences.

Legitimate shape/palette/projection specialization is out of concern unless it is masking simulation behavior.

---

# Audit area 14: direct numeric level checks

Search current production code for semantic equivalents of:

- `level == 11`
- `level == 15`
- `level == 17`
- displayed-level comparisons;
- progression-index equivalents;
- target-specific flag checks.

For each production match record:

- file;
- method;
- condition;
- behavior changed;
- classification;
- whether it has a ROM reason;
- whether it could instead be semantic configuration.

Do not count tests/report/docs as production special cases.

Do not treat level lookup/selection tables as automatically concerning.

---

# Audit area 15: target-specific types and enums

Inventory current production identifiers containing or semantically tied to:

- Level11;
- Level15;
- Level17;
- Straight07;
- Level17Straight07;
- repeating `$24`;
- parameter-development baseline;
- target-specific policies.

For each determine whether it represents:

- data/configuration;
- reusable behavior strategy;
- generic actor lifecycle fork;
- historical naming debt;
- actual workaround.

Names are evidence to inspect, not proof of bad design.

---

# Audit area 16: comparison with smoother levels

Use Levels 14 and 16 as particularly useful engineering controls where comparable, because Level 14 development was smoother and Level 16 intentionally avoided Level 15 as a model.

Determine whether comparable mechanics in those levels use cleaner generic paths.

Do not assume smoother development means better architecture.

Trace actual code.

Where Level 11/15/17 differ from Level 14/16, explain why.

---

# Audit area 17: test architecture

Inspect tests for signs of special-case architecture.

Look for:

- tests that encode a target-level workaround;
- duplicated near-identical tests for special implementations;
- comments describing exceptions;
- tests that bypass generic factories;
- tests that directly manipulate internal state only for 11/15/17.

Classify whether each test protects:

- ROM-required behavior;
- a legitimate architecture boundary;
- historical workaround;
- potential hack.

Do not delete/change tests.

---

# Audit area 18: current dependency graph

Produce a conceptual behavior dependency graph.

At minimum show:

```
LevelConfiguration
    ↓
wave/admission policy
    ↓
shared actor type
    ↓
movement strategy
    ↓
shared lifecycle
    ↓
ledger / reconstruction / completion
```

Show exactly where Levels 11/15/17 diverge.

Highlight any branch where displayed-level identity reaches too deeply into core lifecycle code.

Use Mermaid only if it genuinely clarifies the architecture.

---

# Audit area 19: future-level risk

For every potential hack identified, assess the risk that it will resurface in Levels 18–99.

Questions:

- Will another ROM movement program require another per-level special object?
- Will another Tanker cargo combination duplicate Level 11/15 logic?
- Will additional enemy eligibility force another ledger fork?
- Will another rim route need another handoff implementation?
- Will future levels likely trigger dormant target-level flags accidentally?

Rank risk qualitatively:

- HIGH
- MEDIUM
- LOW

Do not speculate beyond code/evidence.

---

# Audit area 20: determine whether refactoring is actually needed

End with one of these architectural outcomes:

## CLEAN SHARED ARCHITECTURE

Levels 11/15/17 contain legitimate configuration/strategy specializations but converge correctly onto shared lifecycle behavior.

No refactor is recommended before Level 18.

## MINOR ARCHITECTURAL DEBT

Some historical naming/duplication exists, but no active behavioral bypass threatens future development.

Recommend defer or small later cleanup.

## BOUNDED GENERIC-LIFECYCLE PROBLEM

One or more active target-level behavioral implementations bypass generic functionality and are likely to cause recurring bugs.

Recommend a separate bounded refactor before Level 18.

## MATERIAL ARCHITECTURAL PROBLEM

Multiple target levels have independent actor/lifecycle implementations or duplicated systems.

Recommend stopping future level development until a separately authorized architectural correction is designed.

Do not perform that correction.

---

# Required remediation proposal if problems are found

For every Category D item, propose:

1. desired generic responsibility;
2. current target-level workaround;
3. ROM behavior that must remain distinct;
4. shared lifecycle that should remain common;
5. smallest possible future correction;
6. migration order;
7. regression tests/recordings needed;
8. risk of changing accepted Levels 1–17.

Do not write code.

Do not propose rewriting the whole game when a bounded correction is sufficient.

---

# Regression assets for any future refactor

If refactoring is recommended, require future work to preserve:

`TEMP/GameplayRecordings/lvl1-16.json`

and:

`TEMP/GameplayRecordings/lvl-17-test.json`

plus:

- Level 15 seed-1982 soak;
- focused Level 11 Flipper/Tanker tests;
- Level 17 Flipper/rim tests;
- full automated suite;
- Neal's manual gameplay acceptance.

Do not run the recordings merely for ceremony in this read-only audit unless necessary to answer an architecture question.

---

# Execution constraints

This is read-only.

Allowed:

- source inspection;
- tests inspection;
- Git history/diff inspection;
- AI report inspection;
- local ROM/disassembly inspection where a behavior needs authority;
- small calculations;
- bounded search.

Not allowed:

- source edits;
- test edits;
- documentation edits;
- project configuration changes;
- builds for ceremony;
- full test runs for ceremony;
- debugger installation;
- DebugMCP work;
- temporary behavioral variants;
- game launch;
- screenshots/videos;
- staging;
- committing;
- pushing.

Use explicit bounds for any potentially long search/command.

---

# Required AI artifacts

Save this prompt as:

`AI-Logs/Prompts/2026-11-25-level11-15-17-behavior-special-case-architecture-audit-prompt.md`

Create:

`AI-Logs/Reports/2026-11-25-level11-15-17-behavior-special-case-architecture-audit-report.md`

This audit does not authorize staging, commit, or push.

Keep it separate from the completed Level 17 closeout unless a later authorized documentation/closeout task includes it.

---

# Required report contents

1. Executive summary
2. Starting Git/worktree state
3. Audit question and definition of hack
4. Evidence sources
5. Control-level selection
6. Level configuration inventory
7. Direct target-level production checks
8. Target-specific type/enum inventory
9. Flipper construction comparison
10. Below-rim strategy inventory
11. `$24` behavior assessment
12. `$07` behavior assessment
13. `Straight07` assessment
14. `Level17Straight07` assessment
15. Endpoint-hinge assessment
16. Rim-entry handoff matrix
17. Rim-pursuit comparison
18. Tanker core comparison
19. Tanker cargo comparison
20. Cargo identity/lifecycle comparison
21. Ledger comparison
22. Death/reconstruction comparison
23. Completion comparison
24. Firing comparison
25. Collision/destruction comparison
26. Rendering-behavior coupling
27. Level 11 specializations
28. Level 15 specializations
29. Level 17 specializations
30. Level 14/16 control comparison
31. Test architecture findings
32. Classification table for every specialization
33. Category D potential hacks
34. Historical/obsolete specializations
35. Naming debt
36. Conceptual dependency graph
37. Future-level risk assessment
38. Refactor necessity result
39. Bounded remediation proposal if needed
40. Required future regression gates
41. Remaining uncertainties
42. Files/resources inspected
43. Commands/searches run
44. Production code changed: no
45. Tests changed: no
46. Durable documentation changed: no
47. Project configuration changed: no
48. Stage/commit/push: none
49. Level 18+ work: none
50. Recommended next step

Do not compress the report into a short summary. The value of this milestone is the detailed architectural inventory and classification.

---

# Stop conditions

STOP and report rather than guessing if:

- current code differs materially from the accepted Level 17 closeout in a way that cannot be attributed;
- historical source ownership cannot be established;
- a specialization cannot be classified without running or changing production code;
- ROM evidence materially conflicts with accepted gameplay;
- worktree state makes the audit unsafe;
- commands stall.

Do not refactor to resolve uncertainty.

---

# Completion criteria

This audit is complete only when:

- all meaningful Level 11/15/17 behavioral specializations are inventoried;
- configuration is clearly separated from lifecycle code;
- legitimate ROM strategies are distinguished from project workarounds;
- Flipper and Tanker construction/lifecycle paths are traced;
- below-rim strategies are mapped to rim handoff;
- ledger/reconstruction/completion differences are classified;
- direct numeric level checks are classified;
- potential hacks are explicitly identified;
- future-level recurrence risk is assessed;
- refactor necessity is stated clearly;
- no project behavior or code changed.

Then STOP.

TEMPEST-END-LEVEL11-15-17-BEHAVIOR-SPECIAL-CASE-AUDIT-COMPLETE