# Tempest Level 17 Read-Only ROM, Reused-Tube, Color, and Pulsar Investigation

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

Perform a **read-only Level 17 ROM/configuration and architecture investigation** for Tempest.

Level 17 begins the second 16-level tube cycle and introduces a new gameplay/presentation phase.

This is an investigation milestone only.

**Do not modify production code.
Do not modify tests.
Do not modify durable documentation.
Do not implement Level 17.
Do not implement Level 18 or later.
Do not stage, commit, or push.
Do not launch the interactive game.**

Stop after producing the investigation report.

Neal will review the evidence before separately authorizing Level 17 implementation.

---

# Current accepted production baseline

Begin from the completed production-presentation/default-promotion closeout.

Read:

`AI-Logs/Reports/2026-11-25-production-presentation-default-promotion-closeout-report.md`

Also consult current durable project documentation and the current committed source as necessary.

The current production boundary is Levels 1–16.

The accepted Atari presentation is now the normal interactive production presentation; normal development must not assume `--atari-presentation` needs to be supplied.

Do not reopen that completed milestone.

Do not retune Levels 1–16.

---

# Development strategy for Levels 17+

Level 17 remains an **individual level milestone**.

Do not batch Level 17 with Levels 18 or later.

Neal intends to develop the first few levels of this second cycle one at a time so new bugs or architecture gaps can be identified and fixed individually.

Only after several levels in the new cycle are successfully completed will a separate future investigation consider:

- extracting ROM configuration for several levels at once;
- implementing a small batch such as three levels;
- retaining separate manual acceptance for every level.

Do not perform that batch-development investigation in this task.

Do not implement any multi-level generator.

---

# Evidence authority

Use the established Tempest evidence hierarchy:

1. Neal's direct arcade/MAME observations where explicitly supplied.
2. Atari ROM/disassembly behavior and data.
3. Existing manually accepted shared project behavior.
4. Mark Incitti's implementation only as a secondary engineering/reference source when it does not conflict with stronger evidence.

The Atari Tempest disassembly and ROM reference material already exist locally in the project.

Use project-local resources.

Do not search the Internet for ROM material.

Clearly label findings as:

- ROM-BACKED
- ARCADE/MAME-OBSERVED
- PROJECT DECISION / PROJECT TRANSLATION
- REFERENCE-DERIVED
- HYPOTHESIS / UNRESOLVED

Do not silently convert uncertainty into a Level 17 requirement.

---

# Authorized visual evidence

Neal supplied **two specific screenshots with this task** to demonstrate the presentation/color-scheme change between the earlier first-cycle presentation and the Level-17-era presentation.

These two attached screenshots are explicitly authorized visual evidence for this investigation.

Use them only for bounded questions concerning:

- observable tube/web color;
- observable HUD/vector color changes when relevant;
- observable enemy/player colors when relevant to establishing the color scheme;
- comparison of the overall visible color scheme.

Do not use the screenshots to derive:

- tube geometry;
- ROM coordinates;
- enemy supply;
- movement programs;
- speeds;
- timing;
- collision rules;
- numerical projection parameters;
- tube calibration;
- world coordinates.

ROM/configuration evidence remains authoritative for architecture/gameplay.

Do not search directories for additional screenshots.

Do not inspect unrelated screenshots/images/videos.

Do not perform batch image analysis.

If the two supplied screenshots are not accessible to the agent from the attached task context, state that in the report and continue using ROM/source evidence rather than searching for replacements.

---

# Investigation 1: Verify Level 17 tube reuse

Neal's established project requirement is that the unique tube-shape cycle is complete with Levels 1–16.

Level 17 begins reuse of the cycle.

Example:

- Level 1 tube shape recurs at Level 17;
- then again at Level 33;
- then Level 49;
- and so forth through the campaign.

For this task, investigate **Level 17 only**.

Establish from ROM/project evidence:

1. Which ROM tube-shape row displayed Level 17 selects.
2. Whether that row is the same shape used by displayed Level 1.
3. Whether Level 17 requires any geometry difference from the already accepted Level 1 shape.
4. How the current C# architecture maps displayed level → ROM shape/tube geometry.
5. Whether Level 17 can reference the existing canonical geometry rather than defining another coordinate copy.
6. Whether any current architecture incorrectly assumes displayed-level identity equals tube-shape identity.

## Required architectural principle

Do not recommend duplicating Level 1 geometry into a Level 17-specific definition unless authoritative ROM evidence proves the geometry itself differs.

Prefer:

**displayed gameplay Level 17 → existing reusable tube shape**

rather than:

**new copied Level 17 geometry**

The purpose is that future correction to a reusable shape should be capable of applying everywhere that same shape is used.

However, distinguish this from level-specific gameplay.

Reusing the tube must **not** imply reusing Level 1:

- enemy configuration;
- Spike configuration;
- firing;
- speed/timing;
- difficulty;
- colors;
- progression state;
- other level-specific mechanics.

---

# Investigation 2: Separate tube-shape identity from gameplay-level identity

Map the current C# Level 1–16 architecture sufficiently to determine where the following are keyed:

- tube geometry;
- ROM tube-row mapping;
- model-frame mapping;
- accepted tube calibration;
- color scheme;
- gameplay/wave configuration;
- enemy admission policy;
- firing;
- Spike initialization;
- movement-program selection;
- presentation metadata.

For each, classify whether it logically belongs to:

## Shape-specific data

Shared by every gameplay level using that reusable tube.

or:

## Displayed-level/gameplay-specific data

Can differ even when the same tube geometry is reused.

or:

## Unresolved

Needs an explicit implementation decision.

This is an investigation.

Do not refactor the architecture.

---

# Investigation 3: Level 17 color scheme

There is a visible color-scheme change entering the second tube cycle.

The supplied visual references show a substantial tube/web color change between the earlier example and the new-cycle example.

Determine from ROM/disassembly and current renderer architecture:

1. What Level 17's tube/web colors are.
2. Whether colors are selected:
- per displayed level;
- per 16-level range/cycle;
- per tube shape;
- from a ROM color table/state;
- from another mechanism.
3. Whether the color scheme changes at Level 17 independently from geometry.
4. Whether the current C# rendering architecture already separates geometry from color.
5. Whether colors for:
- tube/web;
- Blaster;
- Flippers;
- Tankers;
- Spikers;
- Fuseballs;
- Pulsars;
- shots;
- Spikes;
- HUD/text
change at Level 17 according to ROM evidence.
6. Whether different entity types use fixed colors versus level/cycle-dependent colors.
7. Whether any transition/approach color behavior also changes at Level 16→17.

Do not infer the complete palette solely from the screenshots.

Use the screenshots to confirm or challenge ROM/source interpretation of visible behavior.

If the ROM contains explicit color indices/table values, report the raw values and their meaning separately from host RGB/vector-color translations.

---

# Investigation 4: Pulsar production entry

Level 17 introduces Pulsars into normal production gameplay.

The current project already contains a `Pulsar` implementation from earlier development/testing work.

Therefore do **not** assume Pulsars need to be implemented from scratch.

Determine:

1. What current `Pulsar` production and development code exists.
2. Which parts are:
- ROM-derived;
- Incitti/reference-derived;
- project translations;
- isolated development-only behavior.
3. What Level 17's ROM minimum/maximum Pulsar eligibility is.
4. How Pulsars enter the wave.
5. Their ROM movement program/state behavior.
6. Pulse timing/state.
7. Lane-change behavior.
8. Firing behavior, if Level 17 enables it.
9. Collision/hazard behavior.
10. destruction/scoring behavior where relevant.
11. lifecycle/completion obligations.
12. death/reconstruction implications.
13. whether current shared identity/ledger architecture can represent them.
14. what production integration is still missing.

Do not redesign Pulsars before determining what can be reused.

---

# Investigation 5: Pulsar rendering/presentation

Determine whether existing Pulsar rendering is suitable for normal Level 17 production presentation.

Investigate:

- vector glyph/state;
- pulsing visual states;
- color;
- tube/depth registration;
- Atari-presentation projection;
- shared tube-color changes;
- player collision/hazard presentation.

Separate:

- ROM-backed graphic/state evidence;
- existing manually accepted behavior, if any;
- development-only rendering;
- reference-derived approximations.

Do not tune Pulsar graphics from the supplied screenshots unless Pulsars are actually visible and the screenshot supplies relevant evidence.

Do not create a new rendering implementation.

---

# Investigation 6: Tanker cargo at Level 17

An earlier project Pulsar investigation noted that Tankers can carry Pulsars in later source levels, but that integration was intentionally deferred at that time.

Determine specifically for **Level 17**:

1. Which Tanker load choices are possible.
2. Whether Level 17 Tankers can carry:
- Flippers;
- Pulsars;
- another type;
- more than one possible cargo type.
3. Cargo count.
4. Whether cargo choice depends on a ROM table/random selector.
5. Cargo lane placement.
6. cargo direction/handedness.
7. admission/capacity implications.
8. identity/ledger ownership.
9. latent/pending activation behavior.
10. death/reconstruction behavior.
11. completion obligations.

Do not assume Level 17 has Pulsar cargo merely because later source levels permit it.

Establish Level 17's actual ROM values.

---

# Investigation 7: Full Level 17 enemy composition and supply

Determine the complete ROM-backed Level 17 wave specification.

Include at minimum:

- ordinary wave supply;
- difficulty-specific supply changes;
- Flipper minimum/maximum;
- Tanker minimum/maximum;
- Spiker minimum/maximum;
- Fuseball minimum/maximum;
- Pulsar minimum/maximum;
- ordinary versus derived/cargo identities;
- enemy types that must not appear;
- ROM/global record-capacity behavior;
- any Level 17-specific selection/admission rules.

Do not infer enemy counts from screenshots.

---

# Investigation 8: Flipper movement

Determine Level 17's explicit Flipper movement program from ROM evidence.

Report:

- movement-program byte/identity;
- below-rim lane-change sequence;
- reversal behavior;
- direction semantics;
- any Spike sensitivity;
- cargo behavior;
- whether existing shared C# translations already represent the required program.

Do not use Level 16's `$24` program merely because Level 17 follows it numerically.

Do not assume Level 1 behavior merely because Level 17 reuses Level 1's tube.

Tube geometry reuse and enemy movement programs are independent.

---

# Investigation 9: Tanker behavior

Establish Level 17 Tanker behavior independently from previous-level assumptions.

Include:

- movement program;
- entry behavior;
- cargo load policy;
- firing eligibility/configuration;
- split/release behavior;
- lifecycle;
- identity obligations;
- any Level 17-specific threshold/parameter.

Prefer existing shared mechanics where ROM evidence shows they apply.

Do not copy Level 16 configuration.

---

# Investigation 10: Spiker behavior

Determine:

- whether Spikers occur on Level 17;
- minimum/maximum;
- ROM movement/speed parameters;
- Spike interaction;
- firing;
- lane behavior;
- terminal Spiker-to-Tanker conversion;
- resulting cargo type at Level 17;
- whether existing project translations remain applicable.

Preserve previously accepted project translations unless Level 17 evidence explicitly requires a separate investigation.

Do not reopen generic Spiker fidelity issues without Level 17 evidence.

---

# Investigation 11: Fuseball behavior

Determine whether Fuseballs occur on Level 17 and, if so:

- minimum/maximum;
- relevant raw movement parameters;
- flags/probabilities;
- lifecycle;
- whether existing shared Fuseball behavior is sufficient.

Do not redesign Fuseballs merely because a new cycle begins.

---

# Investigation 12: Spike configuration

Determine Level 17's ROM Spike configuration:

- number of playable Spike lanes;
- raw starting Spike height;
- which lanes receive Spikes;
- whether the Level 1 reused tube topology affects placement;
- Spiker interaction;
- whether existing raw-to-C# translation remains appropriate.

Distinguish raw ROM coordinate from C# translated height.

Do not copy Level 1 or Level 16 Spike values without ROM evidence.

---

# Investigation 13: Enemy firing

Determine Level 17 firing parameters, including:

- shot holdoff;
- shot-speed data;
- enemy-shot slot/index behavior;
- difficulty adjustments;
- firing-eligible species;
- Pulsar firing status;
- Tanker/Flipper/Spiker firing status;
- random-threshold behavior where relevant.

Preserve already established index-versus-count distinctions.

Do not accidentally normalize existing accepted levels during Level 17 work.

If Level 17 exposes another translation discrepancy, report it explicitly.

---

# Investigation 14: Speeds and timing

Extract Level 17 raw ROM values relevant to:

- Flipper speed;
- Tanker speed;
- Spiker speed;
- Pulsar movement/pulse timing;
- Fuseball movement;
- enemy shots;
- admission/wave timing where ROM-backed.

Separate raw values from C# translations.

Do not infer perceived difficulty from raw parameters alone.

Automated/source evidence does not substitute for Neal's later manual gameplay timing judgment.

---

# Investigation 15: Admission and concurrency

Determine the Level 17 ROM behavior relevant to:

- pending supply;
- active records;
- type minimums/maximums;
- Tanker cargo;
- Pulsar cargo if applicable;
- physical enemy-record constraints;
- admission selection;
- derived enemy capacity.

Compare with the existing C# admission/ledger architecture.

Determine whether Level 17 can be represented primarily by a new configuration/policy or whether a generic lifecycle seam needs extension.

Do not add a level-specific parallel accounting system.

---

# Investigation 16: Death and reconstruction

Determine what Level 17 requires from the existing reconstruction system.

Include:

- original identities;
- Eggs/reserve;
- Tankers;
- Flipper cargo;
- Pulsar cargo if applicable;
- Spiker conversion;
- Fuseballs;
- Pulsars;
- latent/pending actors;
- fresh runtime state after re-entry.

Identify missing generic coverage, if any.

Do not implement it in this task.

---

# Investigation 17: Completion obligations

Determine all Level 17 states that must block wave completion.

Include:

- pending original supply;
- reserve/Egg identities;
- active enemies;
- Tankers;
- cargo;
- pending cargo activation;
- converted Spikers/Tankers;
- Pulsars;
- Fuseballs;
- reconstruction states.

Compare this with the current production completion/ledger model.

Report any Level 17 gap.

Do not invent a new completion counter unless future implementation evidence requires one.

---

# Investigation 18: Level 16 → Level 17 progression

Determine requirements for natural progression from accepted Level 16 into Level 17.

Include:

- Level 16 completion transition;
- selection of reused tube geometry;
- new color scheme;
- Level 17 wave initialization;
- score/lives continuity;
- HUD displayed-level identity;
- Super Zapper recharge/state;
- approach/starfield behavior;
- presentation calibration resolution;
- any cycle-dependent transition presentation.

Do not implement progression.

---

# Investigation 19: Direct Level 17 startup

Determine what is required for:

`--start-level 17`

and any established direct-level equivalent.

Direct startup and natural progression must ultimately reach the same Level 17 production configuration.

Identify current resolver boundaries that stop at Level 16.

Do not modify them.

---

# Investigation 20: Presentation calibration reuse versus level-specific color

Level 17 reuses the Level 1 tube geometry, but do not automatically assume every presentation value is shape-specific.

Determine whether the accepted Level 1:

- model-frame mapping;
- tube Y gain;
- scale;
- vertical offset;
- depth-Y gain

can and should be reused for Level 17 because the geometry is the same.

Separately determine whether Level 17's color scheme must remain displayed-level/cycle-specific.

Report clearly:

- what Level 17 should inherit from Tube Shape 1;
- what must remain specific to gameplay Level 17.

Do not perform manual calibration.

Do not encode defaults.

---

# Investigation 21: Levels 1–16 regression boundary

The accepted Levels 1–16 foundation must remain frozen unless concrete Level 17 evidence demonstrates a shared defect.

Use current automated coverage and the local full-campaign recording as available evidence.

Local recording:

`TEMP/GameplayRecordings/lvl1-16.json`

The recent closeout established that this recording replays deterministically through Level 16.

For this read-only investigation:

- do not modify it;
- do not commit it;
- do not rerun it merely for ceremony unless a read-only architecture question genuinely requires it.

For future Level 17 implementation, recommend how it should supplement the normal automated regression gate.

---

# Investigation 22: Architecture readiness for later batch development

Do **not** design or implement batching.

Only identify facts from Level 17 that will matter to a future batching investigation.

Report whether the current architecture appears to separate:

- reusable tube geometry;
- displayed-level ROM configuration;
- color/palette data;
- wave policy;
- movement programs;
- firing;
- presentation calibration.

Identify any obvious per-level duplication that would make future ROM-table-driven multi-level implementation harder.

Do not refactor it in this task.

The future batching investigation remains separately authorized only after several Levels 17+ are completed individually.

---

# Read-only project architecture review

After establishing the ROM-backed Level 17 requirements, inspect the C# architecture sufficiently to answer:

1. Can Level 17 reuse Level 1's canonical tube geometry without copying coordinates?
2. Can it reuse Level 1's accepted geometry/presentation mapping while applying a different color scheme?
3. Does the renderer have an appropriate level/cycle color-selection seam?
4. How much existing Pulsar functionality can be reused unchanged?
5. What Pulsar production integration is missing?
6. Does Tanker cargo need generic extension?
7. Does the identity ledger already support the required Level 17 actors/cargo?
8. Does reconstruction already understand Pulsars in production?
9. Does completion already understand Pulsars in production?
10. What is the smallest bounded Level 17 implementation surface?

Prefer configuration and existing shared mechanics over duplicating code.

---

# Validation during investigation

This task is read-only.

Do not modify production/test files.

Do not run the full suite merely for ceremony.

Use:

- bounded source inspection;
- targeted ROM/disassembly reads;
- focused calculations;
- narrowly useful existing tests only when source inspection cannot establish an architectural fact.

Do not launch the game.

Do not inspect unrelated screenshots/videos/images.

Any command that may run long must have an explicit time/process/test bound.

---

# Required AI artifacts

Save this prompt as:

`AI-Logs/Prompts/2026-11-25-level17-rom-reuse-color-pulsar-investigation-prompt.md`

Create:

`AI-Logs/Reports/2026-11-25-level17-rom-reuse-color-pulsar-investigation-report.md`

Under Neal's current policy, these are GitHub continuity records, but **this investigation does not authorize a commit or push**.

They will be included in the next explicitly authorized closeout/push milestone.

Do not commit merely because AI-Logs now belong in Git history.

---

# Required report contents

The report must include:

1. Executive summary
2. Starting Git/repository state
3. Evidence sources and classifications
4. Authorized screenshot observations
5. Level 17 ROM tube-shape mapping
6. Proof of reuse or difference from Level 1 geometry
7. Current C# reusable-tube architecture
8. Shape-specific versus level-specific data classification
9. Level 17 color/palette ROM findings
10. Current renderer color architecture
11. Screenshot/ROM color reconciliation
12. Existing Pulsar implementation inventory
13. Level 17 Pulsar eligibility
14. Pulsar movement/state behavior
15. Pulsar rendering/presentation
16. Pulsar firing/hazard behavior
17. Pulsar lifecycle/ledger/reconstruction/completion requirements
18. Level 17 Tanker cargo table/behavior
19. Whether Tankers carry Pulsars on Level 17
20. Complete Level 17 enemy supply/composition
21. Flipper movement program
22. Tanker behavior
23. Spiker behavior/conversion
24. Fuseball behavior
25. Spike configuration
26. Enemy firing configuration
27. Speeds/timing raw values and translation boundaries
28. Admission/concurrency
29. Death/reconstruction
30. Completion obligations
31. Level 16→17 progression requirements
32. Direct `--start-level 17` requirements
33. Presentation mapping/calibration reuse assessment
34. Color-specific presentation requirements
35. Current production architecture gaps
36. Smallest recommended Level 17 implementation scope
37. Levels 1–16 regression strategy
38. Facts relevant to eventual batch-development feasibility
39. Uncertainties/evidence conflicts
40. Files/resources inspected
41. Commands/calculations/tests run, if any
42. Confirmation no production code changed
43. Confirmation no tests changed
44. Confirmation no durable documentation changed
45. Confirmation nothing was staged, committed, or pushed
46. Confirmation no Level 18+ implementation/investigation occurred
47. Recommended next step

If the report is long, use clear sections and concise tables where appropriate.

Keep ROM facts, arcade/MAME observations, current project behavior, project translations, reference-derived behavior and hypotheses explicitly separated.

---

# Stop conditions

STOP rather than expanding scope if:

- displayed Level 17 ROM mapping cannot be established reliably;
- Level 17 geometry appears to conflict with the expected reused tube and the conflict cannot be resolved from authoritative evidence;
- color behavior cannot be distinguished from geometry without broader investigation;
- Pulsar production requirements conflict materially with the current implementation and need a separate deeper investigation;
- Level 17 Tanker cargo behavior remains unresolved;
- required ROM data is unavailable;
- source evidence materially conflicts;
- a command stalls or is security-blocked;
- unrelated repository state prevents safe read-only analysis.

Report the blocker precisely.

Do not improvise a production design merely to reach an implementation recommendation.

---

# Completion criteria

The Level 17 investigation is complete only when the available evidence establishes or explicitly leaves unresolved:

- Level 17 tube-shape mapping;
- whether it reuses Level 1 geometry;
- shape-specific versus level-specific presentation data;
- Level 17 color behavior;
- current color architecture;
- Pulsar production requirements;
- Level 17 Tanker cargo;
- complete enemy composition/supply;
- movement programs;
- Spike configuration;
- firing;
- speed/timing parameter boundaries;
- admission/concurrency;
- reconstruction;
- completion;
- Level 16→17 progression requirements;
- direct Level 17 startup requirements;
- presentation mapping/calibration reuse;
- smallest bounded implementation surface;
- appropriate Levels 1–16 regression strategy.

Then STOP.

Do not implement Level 17.

Neal will review the investigation before authorizing implementation.

TEMPEST-END-LEVEL17-ROM-REUSE-COLOR-PULSAR-INVESTIGATION-COMPLETE