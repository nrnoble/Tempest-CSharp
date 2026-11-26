# Tempest Level 17 Missing ROM Parameters, Palette, and Speed Translation — Read-Only Follow-up Investigation

## FAIL-CLOSED COMPLETION-TOKEN CHECK

Read the entire user-provided task before taking any action.

Before tools, files, commands, modifications, tests, or repository actions:

1. Locate the completion token near the end of this user-provided task.
2. Verify that exactly one complete token beginning with `TEMPEST-END-` appears on a line by itself in the user-provided task.
3. The token may contain only uppercase ASCII letters, digits, and hyphens after the `TEMPEST-END-` prefix.
4. VS Code/agent-generated reminders, tagged-file information, attachment metadata, current-editor context, or other system-generated context appearing after the user's completion token do not invalidate it.
5. This check exists only to detect an incomplete or truncated user task, not appended agent context.
6. If exactly one complete standalone `TEMPEST-END-` token cannot be found, STOP before any project or repository action and explain why.

Do not require an older underscore-based `TEMPEST_...` token.

The unique completion token for this task is the final standalone line below.

---

# Task

Perform a **read-only follow-up investigation** to resolve the remaining implementation-critical Level 17 evidence gaps identified by the initial Level 17 investigation.

This is not a second broad Level 17 investigation.

Accept the first report's established findings unless direct authoritative evidence demonstrates a concrete contradiction.

Focus only on:

1. the exact Level 17 Flipper movement program;
2. the exact Levels 17–32 ROM color/palette block;
3. the ROM palette roles and their host-rendering implications;
4. the exact Level 17 Pulsar timing/movement parameters;
5. Level 17 speed progression and ROM-to-C# translation;
6. difficulty-specific wave adjustments;
7. remaining enemy-firing parameters necessary for implementation;
8. bounded reconciliation against the two user-supplied screenshots if those images are accessible.

**Do not modify production code.
Do not modify tests.
Do not modify durable documentation.
Do not implement Level 17.
Do not investigate or implement Level 18 or later.
Do not stage, commit, or push.
Do not launch the interactive game.**

Stop after producing the follow-up investigation report.

Neal will review the completed evidence before separately authorizing Level 17 implementation.

---

# Required starting report

Read completely:

`AI-Logs/Reports/2026-11-25-level17-rom-reuse-color-pulsar-investigation-report.md`

Also consult the current repository-local Atari ROM/disassembly references and the current production source where necessary.

Do not repeat the broad investigation sections that are already established.

---

# Established findings from the first report

Treat the following as established unless direct authoritative evidence proves otherwise.

## Tube reuse

- Displayed Level 17 begins the second 16-level tube cycle.
- Level 17 resolves through the ROM remap cycle to shape row 0.
- Shape row 0 is the canonical tube geometry already used by displayed Level 1.
- Level 17 should reuse that geometry rather than copy its coordinate data.
- Reusing Level 1's tube shape does not imply reusing Level 1 gameplay configuration.

## Color-cycle structure

- The ROM selects color data in 16-level campaign blocks.
- Level 17 begins the Levels 17–32 color block.
- The six campaign color ranges are:
- Levels 1–16
- Levels 17–32
- Levels 33–48
- Levels 49–64
- Levels 65–80
- Levels 81–99
- Level 17's palette identity is level/cycle-specific, not tube-geometry-specific.
- The current C# implementation has no complete Levels 17–32 palette catalog.

## Wave composition

Level 17's established base configuration includes:

- ordinary supply: 20;
- Flippers: minimum 0, maximum 3;
- Tankers: minimum 1, maximum 1;
- Spikers: minimum 0, maximum 2;
- Pulsars: minimum 2, maximum 2;
- Fuseballs: minimum 0, maximum 0;
- Tanker cargo: two Flippers;
- raw starting Spike height: `$A0`;
- two enemy-shot slots under the established ROM index/count interpretation;
- Pulsar firing disabled.

## Existing Pulsar status

- A substantial Pulsar actor and renderer already exist.
- Existing Pulsar behavior was developed for an isolated scenario.
- Natural production admission, Pulsar identities in the ledger, death/reconstruction, and completion integration remain missing.
- Existing Pulsar timing contains reference-derived/project-translated behavior that must not automatically be relabeled as Level 17 ROM-backed behavior.

## Progression/startup

- Level 17 is not yet available through direct startup.
- Natural progression currently stops at Level 16.
- Future natural and direct Level 17 startup must resolve to one shared production configuration.
- Level 17 should reuse Level 1 shape/projection data while retaining Level 17-specific color and gameplay data.

## Regression boundary

- Levels 1–16 are the accepted production foundation.
- `TEMP/GameplayRecordings/lvl1-16.json` is local deterministic regression evidence.
- The recording remains local and must not be modified or committed.

Do not spend this task re-proving these findings.

---

# Evidence authority

Use the established Tempest authority order:

1. Neal's explicitly supplied arcade/MAME observations.
2. Atari ROM/disassembly behavior and data.
3. Existing manually accepted shared project behavior.
4. Mark Incitti's implementation only as a secondary engineering/reference source.

Use the project-local ROM/disassembly material.

Do not search the Internet for ROM material.

Clearly classify each conclusion as:

- ROM-BACKED
- ARCADE/MAME-OBSERVED
- PROJECT DECISION / PROJECT TRANSLATION
- REFERENCE-DERIVED
- HYPOTHESIS / UNRESOLVED

Do not turn an unresolved byte, table, or conversion into an implementation requirement.

---

# Investigation 1: Locate the exact Level 17 Flipper movement-program byte

The first report could not establish the Level 17 Flipper movement program.

Resolve it from authoritative project-local evidence.

Determine:

1. the ROM table or loader that selects `flipper_move`;
2. the exact displayed/internal Level 17 table entry;
3. the movement-program byte;
4. the P-code entry reached by that byte;
5. the full below-rim movement sequence;
6. lane-flip counts;
7. reversal behavior;
8. move-versus-flip ordering;
9. any Spike-sensitive branch;
10. initial-direction semantics;
11. whether Tanker-released cargo Flippers use the same Level 17 program;
12. whether an existing C# movement-program translation already implements it.

Do not infer the result from Level 1 geometry.

Do not infer the result from Level 16 adjacency.

Do not use a disassembly comment covering only Levels 1–16 as proof of the Level 17 value unless the underlying table indexing establishes the second-cycle entry.

If the symbolic listing omits the needed byte, inspect the smallest bounded ROM binary location necessary to recover it.

Record:

- ROM file;
- load/base mapping;
- exact address/offset;
- number of bytes read;
- extracted value;
- independent consistency check where practical.

Do not enumerate or inspect unrelated ROM contents.

---

# Investigation 2: Recover the exact Levels 17–32 palette block

The first report established the six-block color-cycle mechanism but not the raw data.

Recover the exact eight-byte/nibble color block selected for Levels 17–32.

Determine:

1. the ROM routine that selects the block;
2. the source table address;
3. entry/block layout;
4. exact block-selection arithmetic;
5. the eight raw bytes or nibbles for Levels 17–32;
6. which runtime color variables receive each entry;
7. how those variables are used by:
- tube/web rendering;
- player/Blaster;
- Flippers;
- Tankers;
- Spikers;
- Pulsars;
- Fuseballs;
- shots;
- Spikes;
- HUD/text;
- Super Zapper or other effects, where relevant;
8. whether any entries are packed pairs or otherwise transformed;
9. color state during approach from Level 16 to Level 17;
10. whether the block applies immediately on transition or at another state boundary.

If the disassembly uses `.chunk 8` or otherwise omits literal values, perform a bounded binary read from the documented ROM image.

Record exact file/address/offset/length and extracted values.

Do not fetch replacement ROM material from the Internet.

---

# Investigation 3: Map ROM color values to visible roles

Do not immediately translate ROM palette values into arbitrary RGB.

Trace how the arcade code interprets each raw color value.

Determine:

- whether the value is a vector-generator color/intensity code;
- whether color and intensity are packed;
- whether values index another table;
- which bits control hue;
- which bits control intensity, if established;
- how blinking/pulsing modifies the base value;
- whether the tube/web uses one role consistently through Levels 17–32;
- which colors remain fixed by enemy type versus cycle-dependent.

For each Level 17 visible role, report:

- raw ROM value;
- semantic role;
- any transformation;
- whether the current C# renderer already has an equivalent color;
- whether a new host color mapping is required;
- whether the exact host RGB remains a PROJECT TRANSLATION requiring manual acceptance.

Do not claim that a host RGB triple is ROM-backed unless the source directly establishes that mapping.

---

# Investigation 4: Reconcile palette evidence with the two supplied screenshots

The original user task supplied two screenshots illustrating first-cycle and second-cycle color differences.

The first investigation could not access them.

For this follow-up:

- use only the two screenshots supplied with the current conversation/task if they are available through the agent attachment context;
- do not search repository folders for substitute screenshots;
- do not inspect unrelated screenshots, images, or videos;
- do not perform broad image enumeration.

If accessible, record only objective visible observations relevant to color, such as:

- tube/web line color;
- Blaster color;
- visible enemy colors;
- shot/Spike colors;
- HUD/text colors;
- whether the second-cycle example visibly agrees with the recovered ROM roles.

Do not derive geometry, movement, speed, timing, coordinates, or calibration from the screenshots.

If the screenshots remain inaccessible, state that explicitly.

Do not let screenshot unavailability block recovery of the ROM palette bytes.

---

# Investigation 5: Recover exact Level 17 Pulsar timing and movement fields

Identify every ROM field actually used by a Level 17 Pulsar.

At minimum investigate:

- movement speed input;
- pulse clock or interval;
- flip speed;
- climb/descend decision timing;
- lane pause;
- rim pause;
- reversal timing;
- movement state transitions;
- pulse hazard intervals;
- lane-change state;
- player-collision/hazard state;
- firing enable;
- any level-indexed timing table;
- any difficulty adjustment.

For each field provide:

1. ROM symbol/routine/table;
2. Level 17 raw value;
3. units/representation where established;
4. state/routine that consumes it;
5. whether the current C# Pulsar already has an equivalent parameter;
6. whether current C# behavior matches, approximates, or conflicts;
7. evidence classification.

Do not treat the current reference-derived 51-update, 181-update, or pause values as ROM-backed unless the trace establishes it.

If exact fields are not stored directly per level but are computed from the level number, reconstruct the arithmetic exactly and show the Level 17 result.

Use hexadecimal and decimal forms where helpful.

---

# Investigation 6: Level 17 speed progression

Neal reports that gameplay becomes faster in the second cycle and that manually quantifying subtle speed increases against the arcade is impractical.

The goal is to rely on ROM-derived values wherever the translation can be established.

Recover and compare Level 17 speed/timing values against the relevant earlier baseline values.

At minimum cover:

- Flipper below-rim movement;
- Tanker movement;
- Spiker movement;
- Pulsar movement;
- Fuseball if relevant to a generic table, while noting Fuseballs are excluded from Level 17;
- enemy-shot speed;
- firing holdoff;
- lane-flip transition rate;
- rim movement/pursuit inputs where level-dependent;
- Spike growth/interaction timing where level-dependent.

For each mechanism report:

- Level 1 raw value;
- Level 16 raw value where relevant;
- Level 17 raw value;
- raw representation;
- ROM cracking/conversion routine;
- resulting fixed-point displacement or timing where computable;
- level-to-level and cycle-boundary ratio/difference;
- corresponding current C# representation;
- whether a reliable C# translation already exists;
- whether the implementation can preserve the ROM ratio without manual numerical calibration.

Do not assume that one global multiplier explains every speed change.

Distinguish:

- per-enemy speed;
- update cadence;
- action holdoff;
- transition duration;
- firing pressure;
- overall host loop rate.

Do not alter the accepted host-loop timing.

---

# Investigation 7: Establish a ROM-to-C# speed translation strategy

For each Level 17 mechanic with a recovered ROM value, determine the smallest faithful C# translation class.

Possible outcomes include:

## Direct translation

The ROM fixed-point displacement can be mapped directly through an established project conversion.

## Ratio-preserving translation

Absolute ROM and C# units differ, but the Level 17-to-baseline ratio can be applied to an accepted C# baseline.

## Existing shared project translation

A current C# helper/table already represents the same ROM cracking behavior.

## Manual/project calibration required

The ROM establishes progression but not a safe C# unit mapping.

## Unresolved conflict

ROM/source evidence conflicts with repeated arcade/MAME observation or accepted project behavior.

Do not force all mechanics into one strategy.

The report must explicitly recommend the translation strategy separately for:

- Flipper/Tanker;
- Spiker;
- Pulsar;
- enemy shots;
- movement-transition timing;
- any other level-dependent speed field.

Manual acceptance should remain a sanity/fidelity gate, not a request for Neal to estimate small numeric percentages.

---

# Investigation 8: Difficulty-specific Level 17 supply and speed changes

Resolve Level 17 difficulty handling.

Determine:

- default/medium wave supply;
- easy supply;
- hard supply;
- exact arithmetic used;
- any carry/rounding behavior;
- speed adjustments;
- shot-slot adjustments;
- holdoff changes;
- Pulsar-related difficulty changes;
- species eligibility changes;
- other Level 17 difficulty effects.

Do not apply conventional mathematical rounding if the ROM uses shifts/carry behavior.

Show the actual arithmetic for Level 17.

Separate ROM behavior from current project difficulty support.

If the project does not yet expose difficulty selection, still record the ROM facts without implementing that system.

---

# Investigation 9: Complete Level 17 firing fields

The first report established:

- two enemy-shot slots under the ROM index/count interpretation;
- enemy-shot speed `$C0`;
- Pulsar firing disabled.

Resolve the remaining implementation-required firing evidence:

1. exact shot holdoff value;
2. holdoff units/loader;
3. difficulty adjustment;
4. eligible species;
5. random-threshold table/index behavior;
6. position/state gates;
7. whether Tankers, Flippers, and Spikers fire at Level 17;
8. whether current C# per-enemy ammunition concepts apply or remain absent;
9. C# translation required;
10. whether accepted prior-level firing helpers can be reused.

Do not copy Level 16 firing values.

Do not alter earlier-level behavior.

---

# Investigation 10: Validate Tanker cargo and Pulsar firing boundaries

The first report established that Level 17 Tanker cargo is two Flippers and that Pulsar firing is disabled.

Perform a bounded authoritative cross-check so these implementation boundaries are safe.

Confirm:

- exact Tanker load bytes used by Level 17;
- cargo-type interpretation;
- cargo count;
- whether any difficulty or selector changes cargo;
- exact Pulsar firing-enable condition;
- that displayed Level 17 does not meet it;
- whether hard difficulty changes Pulsar firing at Level 17.

This is a cross-check, not a broad reinvestigation.

If the cross-check contradicts the first report, report the discrepancy and stop before recommending implementation.

---

# Investigation 11: C# integration implications

After resolving the ROM evidence, update the recommended implementation boundary.

Determine:

1. whether Level 17 can reuse an existing Flipper movement-program implementation;
2. which speed values can use existing translations;
3. whether new speed configuration fields/helpers are necessary;
4. whether the Pulsar actor requires parameterization rather than redesign;
5. whether pulse timing should become level-configured;
6. whether the palette belongs in:
- a cycle palette catalog;
- level configuration;
- presentation metadata;
- another bounded shared structure;
7. how Level 17 references canonical Shape 1 while selecting Palette Block 2;
8. which Level 17 values remain project translations requiring later manual acceptance.

Do not implement anything.

Prefer a bounded data/configuration extension over per-level conditional branches.

---

# Investigation 12: Implementation-readiness gate

At the end, give one explicit result:

## READY FOR BOUNDED LEVEL 17 IMPLEMENTATION

Use only if all implementation-critical values are established or have a clearly classified, bounded project translation.

or:

## NOT READY

List the exact remaining blockers and the smallest next evidence-gathering task.

Do not declare readiness merely because most values were found.

The implementation must not require guessing:

- Flipper movement program;
- color-cycle block;
- required Pulsar timing;
- required firing fields;
- speed progression policy.

If host RGB requires manual acceptance but raw role/color data and a reasonable initial translation are established, that alone need not block an implementation candidate. Clearly leave visual color acceptance to Neal.

---

# Read-only execution and safety

Use bounded source searches, targeted ROM/disassembly reads, and small calculations.

If a raw ROM binary read is necessary:

- identify the exact expected file;
- confirm file size;
- map ROM load address to file offset carefully;
- read only the required bounded range;
- report the range;
- independently sanity-check extracted values against known neighboring data where possible.

Do not modify ROM/reference files.

Do not create permanent generated data files outside the required AI report.

Temporary analysis output may be used and removed.

Do not weaken Windows security.

Do not bypass Application Control.

Do not run broad tests/builds merely for ceremony.

Do not launch the game.

---

# Required AI artifacts

Save this prompt as:

`AI-Logs/Prompts/2026-11-25-level17-missing-rom-palette-speed-followup-investigation-prompt.md`

Create:

`AI-Logs/Reports/2026-11-25-level17-missing-rom-palette-speed-followup-investigation-report.md`

Under Neal's policy, these are GitHub continuity records, but this read-only investigation does **not** authorize a commit or push.

They will be included in the next explicitly authorized closeout/push milestone.

---

# Required report contents

The report must include:

1. Executive summary
2. Starting Git/repository state
3. Established first-report findings accepted without reinvestigation
4. Evidence sources and classifications
5. Exact Level 17 Flipper movement-program byte
6. Flipper P-code trace and movement sequence
7. Existing C# movement-program reuse assessment
8. Levels 17–32 palette-block location and exact raw values
9. Palette block-selection arithmetic
10. Runtime color-variable assignments
11. ROM color-role interpretation
12. Host RGB/project-translation boundary
13. Screenshot accessibility
14. Objective screenshot observations, if accessible
15. Screenshot/ROM reconciliation
16. Exact Level 17 Pulsar raw parameters
17. Pulsar state/timing trace
18. Current C# Pulsar comparison
19. Level 17 speed table
20. Level 1/16/17 comparisons
21. ROM cracking/fixed-point calculations
22. Per-mechanic ROM-to-C# translation strategy
23. Difficulty-specific wave supply
24. Difficulty-specific speed/firing adjustments
25. Exact firing holdoff and slot behavior
26. Firing-eligible species
27. Random/state firing gates
28. Tanker cargo authoritative cross-check
29. Pulsar firing authoritative cross-check
30. Palette architecture recommendation
31. Speed architecture recommendation
32. Pulsar parameterization recommendation
33. Updated smallest Level 17 implementation scope
34. Remaining manual-acceptance questions
35. Implementation-readiness result
36. Remaining blockers, if any
37. Files/resources inspected
38. Exact ROM files/addresses/offsets/ranges read
39. Commands/calculations performed
40. Confirmation no production code changed
41. Confirmation no tests changed
42. Confirmation no durable documentation changed
43. Confirmation nothing was staged, committed, or pushed
44. Confirmation no Level 18+ implementation/investigation occurred
45. Recommended next step

Keep the report factual.

Do not hide evidence gaps behind a recommendation.

---

# Stop conditions

STOP and report rather than guessing if:

- the correct ROM image/address mapping cannot be established;
- the Flipper movement byte remains inaccessible;
- palette block bytes cannot be recovered;
- palette roles cannot be traced sufficiently for implementation;
- Pulsar timing remains materially ambiguous;
- speed conversion conflicts with accepted evidence and cannot be bounded;
- firing fields remain ambiguous;
- first-report Tanker cargo or Pulsar firing findings are contradicted;
- supplied screenshots remain unavailable and ROM evidence is also insufficient;
- commands stall or security blocks required bounded analysis;
- unrelated repository state prevents safe read-only investigation.

Do not create an implementation prompt within this task.

---

# Completion criteria

This follow-up is complete only when it establishes or explicitly leaves blocked:

- exact Level 17 Flipper movement program;
- complete Levels 17–32 raw palette block;
- palette role assignments;
- host-color translation boundary;
- screenshot reconciliation status;
- exact required Pulsar timing/movement fields;
- Level 17 speed progression;
- ROM-to-C# translation strategy per mechanic;
- difficulty-specific Level 17 adjustments;
- complete firing fields;
- authoritative Tanker cargo boundary;
- authoritative Pulsar firing boundary;
- updated implementation scope;
- explicit READY or NOT READY result.

Then STOP.

Do not implement Level 17.

TEMPEST-END-LEVEL17-MISSING-ROM-PALETTE-SPEED-FOLLOWUP-COMPLETE