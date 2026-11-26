# Tempest Level 17 ROM-to-C# Translation and Production-Integration Readiness Investigation

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

Perform a **read-only ROM-to-C# translation and production-integration readiness investigation for Level 17**.

The required Level 17 ROM mapping and several critical raw values have now been established.

This task must not repeat broad ROM-byte discovery unless a narrowly bounded source trace is required to interpret an already recovered value.

The purpose is to determine exactly how the verified Level 17 ROM behavior should map into the existing C# architecture, and whether a bounded Level 17 implementation can proceed without invented values.

**Do not modify production code.
Do not modify tests.
Do not modify durable documentation.
Do not implement Level 17.
Do not investigate or implement Level 18 or later.
Do not stage, commit, or push.
Do not launch the interactive game.
Do not search the Internet for ROM material.**

Stop after producing the readiness report.

---

# Required prior reports

Read completely:

`AI-Logs/Reports/2026-11-25-level17-rom-reuse-color-pulsar-investigation-report.md`

`AI-Logs/Reports/2026-11-25-level17-missing-rom-palette-speed-followup-investigation-report.md`

`AI-Logs/Reports/2026-11-25-level17-rom-map-final-evidence-report.md`

Use the third report as the latest corrective evidence authority when earlier reports conflict.

Do not rewrite historical reports.

---

# Established evidence: do not reinvestigate broadly

Treat the following as established unless direct authoritative evidence demonstrates a concrete contradiction.

## Rev. 2 CPU ROM map

The validated linear CPU ROM layout and known-byte anchors in the third report are accepted.

Do not redo the full mapping investigation.

## Tube reuse

- Level 17 resolves to canonical ROM shape row 0.
- Level 17 reuses the accepted Level 1 tube geometry.
- Level 17 must not copy the Level 1 point array into a separate geometry.
- Geometry reuse does not imply gameplay reuse.

## Palette block

The exact Levels 17–32 raw eight-byte palette block is:

`00 07 0B 08 44 03 0C 0C`

CPU range:

`$C205-$C20C`

This is verified ROM data.

The adjacent first-cycle block is:

`00 04 08 0C C3 07 0B 0B`

Do not repeat palette-byte recovery.

## Flipper program

Level 17 selects ROM Flipper movement program:

`$07`

The verified source trace identifies it as straight movement upward with no below-rim flip/reversal sequence.

It applies to ordinary or Tanker-cargo Flippers where `$07` is assigned.

## Pulsar policy

For Level 17:

- minimum Pulsars: 2;
- maximum Pulsars: 5;
- medium/default firing: disabled;
- easy firing: disabled unless direct contrary evidence is found in the easy branch;
- hard firing: enabled through `$40`;
- `pulsar_fliprate = $28`;
- `pulse_beat = $04`;
- fixed speed initialization: `$FE.A0`.

## Tanker cargo

Level 17 Tanker cargo type `$01` means:

**two Flippers**

No Level 17 Pulsar cargo is established.

## Firing and shots

- Level 17 shot holdoff: `$20`, 32 ROM ticks.
- Flippers, Tankers, and Spikers are firing-eligible under the ROM scheduler gates.
- Pulsars fire only on hard difficulty at Level 17.
- Level 17 raw shot maximum `$02` is a last-slot index:
- medium/default: 3 slots;
- easy: 2 slots;
- hard: 4 slots.
- Fuseballs are absent.

## Speed data

- Level 17 Flipper/Tanker raw low byte: `$AF`.
- Flipper speed is cracked into Atari 8.8 movement representation.
- Tanker speed copies the cracked Flipper speed.
- Spiker raw speed-table input: `$00`, but this is not proof of zero movement.
- Pulsar fixed speed word: `$FE.A0`.
- Enemy-shot base input: `$C0`, with opcode `$0A` adding RAM `$0160`.
- No global speed multiplier is established.

## Supply

- base/medium supply: 20;
- hard supply: 23 under the established ROM arithmetic;
- exact easy supply differs only if the source trace proves an adjustment.

## Spike

- raw starting Spike height: `$A0`;
- host translation remains a project mapping, not a literal ROM coordinate.

Do not spend this task proving these values again.

---

# Evidence authority

Use:

1. verified local Atari ROM/disassembly evidence;
2. Neal's explicitly supplied arcade/MAME observations;
3. existing manually accepted shared C# behavior;
4. current project architecture/tests;
5. Mark Incitti only as a secondary reference.

Clearly classify every conclusion:

- ROM-BACKED
- ARCADE/MAME-OBSERVED
- EXISTING ACCEPTED PROJECT BEHAVIOR
- PROJECT TRANSLATION / PROJECT DECISION
- REFERENCE-DERIVED
- UNRESOLVED

Do not present a convenient C# mapping as literal ROM behavior.

---

# Investigation 1: Inventory the existing C# Level 17-relevant architecture

Inspect the current production architecture, including at minimum:

- `LevelConfiguration`
- `LevelLoader`
- `LevelProgression`
- ROM tube/remap metadata
- Atari presentation calibration/mapping
- wave-selection policies
- identity/ledger structures
- enemy factories
- `Flipper`
- `Tanker`
- `Spiker`
- `Pulsar`
- enemy-shot scheduling
- death/reconstruction
- completion
- renderer color selection
- Super Zapper color handling

Create an architecture map showing:

- existing reusable mechanisms;
- current hard-coded per-level policies;
- development-only Pulsar seams;
- missing Level 17 integration points;
- places where later-level generalization is justified now;
- changes that should remain deferred.

Do not refactor anything.

---

# Investigation 2: Reconcile `$07` with current C# Flipper behavior

The ROM establishes straight movement program `$07`.

Determine exactly:

1. what current C# `Straight07` does;
2. whether it is used only for Tanker cargo or more generally;
3. whether its update cadence matches existing accepted `$07` translation;
4. whether it changes lanes;
5. whether it reverses;
6. how initial direction is represented when the route has no lane change;
7. whether ordinary Level 17 Flippers can select it without new movement logic;
8. whether Tanker-cargo Flippers can use the same route;
9. how Spike sensitivity interacts with straight movement;
10. whether any Level 1-specific straight behavior should remain separate.

End with one:

- exact reusable translation;
- reusable with bounded parameterization;
- new `$07` translation required;
- unresolved conflict.

Do not infer route equivalence from matching names alone.

---

# Investigation 3: Trace Atari cracked Flipper/Tanker speed into movement

The Level 17 raw Flipper/Tanker speed is verified as `$AF`.

Trace the Atari path from:

- parameter-table byte;
- `crack_speed`;
- resulting signed 8.8 word;
- enemy along-position update;
- update cadence;
- direction/sign handling;
- depth/rim termination.

Express the effective Atari movement per active update exactly where possible.

Then inspect current C# Flipper/Tanker movement:

- configured speed representation;
- randomization, if any;
- per-update movement;
- level-index formulas;
- route-specific speed handling;
- existing ROM-speed helpers;
- accepted level-specific project translations.

Determine the smallest defensible Level 17 C# mapping.

Evaluate:

## Direct fixed-point translation

## Ratio against an existing accepted C# baseline

## Existing shared helper/table

## Bounded project translation

## Unresolved conflict

If recommending ratio preservation, state exactly:

- source baseline level/mechanic;
- ROM baseline value;
- accepted C# baseline value;
- Level 17 ROM value;
- computed ratio;
- resulting Level 17 C# value;
- why that baseline is appropriate.

Do not pick a baseline merely because it produces an aesthetically convenient value.

---

# Investigation 4: Trace Spiker speed from raw `$00`

The ROM input `$00` must not be interpreted as no movement.

Trace:

- parameter loading;
- any cracking/default/complement behavior;
- Spiker initialization fields;
- `eact_0c` movement;
- effective signed/integer/fractional update;
- ascent/retreat sign behavior;
- level dependency;
- terminal conversion interaction.

Then compare with current C# Spiker movement and existing manually accepted rules.

Determine whether Level 17 uses:

- an existing shared translated Spiker speed;
- another ROM-derived value;
- a level-specific configuration;
- an unresolved project translation.

Preserve the accepted cross-level rule that Spiker projectiles must visibly travel faster than their firing Spiker unless compelling evidence leads to a separate explicitly reported conflict.

Do not silently reintroduce a slower-shot relationship.

---

# Investigation 5: Resolve effective enemy-shot speed

Trace opcode `$0A` and RAM `$0160`.

Determine:

1. what initializes `$0160`;
2. whether it is difficulty-dependent;
3. whether it is level-dependent;
4. whether it represents a global speed adjustment;
5. arithmetic/carry behavior;
6. resulting Level 17 shot-speed value;
7. resulting movement update;
8. current C# enemy-shot speed representation;
9. whether existing accepted shot speed should be reused or parameterized.

If `$0160` cannot be resolved, determine whether implementation can still safely use an existing accepted shared project translation without claiming literal ROM equivalence.

Do not let this remain an unspecified value in the final configuration specification.

Classify it explicitly.

---

# Investigation 6: Reconcile Pulsar ROM behavior with `Pulsar.cs`

Perform a field-by-field state comparison.

At minimum compare:

- speed;
- movement direction;
- climb;
- descend;
- lane pause;
- rim pause;
- lane-change trigger;
- flip rate;
- pulse variable;
- pulse beat;
- pulse reversal bounds;
- hazard state;
- player collision;
- firing;
- rim pursuit;
- destruction;
- scoring;
- Super Zapper;
- terminal lifecycle;
- rendering pulse state.

For each existing C# field/timer/state, classify:

- exact ROM correspondence;
- scaled ROM translation;
- reference-derived approximation;
- project decision;
- development-only behavior;
- conflict.

Specifically assess current:

- 51-update decision timing;
- 181-update pulse cycle;
- 18-tick lane pause;
- 14-tick rim pause;
- 12-degree pulse-flip constant.

Do not assume these must all be replaced.

Determine which are safe existing translations and which require Level 17 parameterization or another bounded implementation decision.

---

# Investigation 7: Define Pulsar production-admission policy

Using min 2/max 5 and current C# admission architecture, determine:

1. how Pulsars enter original supply;
2. whether Pulsars hatch directly from Eggs;
3. whether minimum means guaranteed admitted identities under the established wave-selection model;
4. how maximum interacts with active slots;
5. random species selection;
6. Pulsar identity representation;
7. active/reserve/pending states;
8. whether Pulsars consume ordinary original-wave identities;
9. whether any Pulsar is derived cargo at Level 17;
10. difficulty effects.

No Level 17 Pulsar cargo should be introduced.

Provide a deterministic C# policy recommendation consistent with ROM evidence and existing project selection conventions.

Do not implement it.

---

# Investigation 8: Ledger and identity model extension

Inspect current ledger identity/type/state structures.

Determine the smallest generic extension needed for Pulsars as original Level 17 enemies.

Cover:

- identity allocation;
- original species;
- reserve;
- Egg;
- ready-to-hatch;
- active Pulsar;
- lane transition;
- rim state;
- destroyed;
- rim-cleared;
- death/re-entry reconstruction;
- Super Zapper;
- completion;
- representation validation.

Determine whether the existing type names tied historically to Level 3 should be generalized now or whether a bounded compatible extension is safer.

Avoid parallel Level 17-only accounting.

Do not redesign the entire ledger if extending existing enums/policies is sufficient.

---

# Investigation 9: Death and reconstruction with Pulsars

Determine how current death/re-entry reconstruction must treat original Pulsar identities.

Establish:

- active Pulsar reconstruction;
- reserved/unhatched Pulsar reconstruction;
- Pulsar Egg state;
- Pulsar lane/direction runtime state;
- fresh versus restored timing;
- firing state by difficulty;
- pulse-clock state;
- rim-state handling;
- no duplicate allocation;
- conservation of unresolved identities.

Compare to ROM behavior where established and existing project reconstruction principles otherwise.

Classify fresh runtime state as a project translation where literal ROM restoration differs or is unavailable.

---

# Investigation 10: Completion obligations

Determine how Pulsars must participate in the production completion model.

Cover:

- original reserve;
- Egg;
- ready-to-hatch;
- active below rim;
- lane transition;
- active rim;
- destroyed;
- rim-cleared;
- pending reconstruction;
- missing representation.

Verify that cargo Flippers, Tankers, possible Spikers, and Pulsars all remain unresolved obligations until terminal.

Do not create a Pulsar-specific completion counter.

Recommend the minimum generic ledger/completion extension.

---

# Investigation 11: Difficulty policy

Define a bounded Level 17 difficulty policy from verified ROM behavior and existing project support.

At minimum specify:

## Medium/default

- supply 20;
- Pulsars 2–5;
- Pulsar firing off;
- 3 enemy-shot slots;
- `$AF` Flipper/Tanker input;
- shot holdoff `$20`.

## Easy

- supply adjustment or no adjustment;
- Flipper/Tanker speed adjustment;
- Pulsar firing;
- 2 shot slots;
- any pulse/movement changes;
- species limits.

## Hard

- supply 23;
- Flipper/Tanker speed adjustment;
- Pulsar firing on;
- 4 shot slots;
- any pulse/movement changes;
- species limits.

Determine whether the project currently exposes selectable difficulty.

If not, distinguish:

- production Level 17 default behavior to implement now;
- ROM difficulty facts to encode/test for future support;
- changes that should remain deferred.

Do not build an entire difficulty-selection feature in Level 17 unless already supported and required.

---

# Investigation 12: Palette-runtime role trace

The raw block is known:

`00 07 0B 08 44 03 0C 0C`

Trace the runtime arrays populated by `setcolours`.

Determine how each low/high nibble array is consumed.

Identify as many roles as authoritative evidence permits:

- tube/web;
- player/Blaster;
- Flipper;
- Tanker;
- Spiker;
- Pulsar;
- shot;
- Spike;
- HUD/text;
- effects.

Use source cross-references to runtime color variables rather than guessing from nibble values.

Where a value role remains ambiguous, name the exact consumer paths that are known.

Do not force all eight entries into entity names if the ROM uses them for animation or indirect lookup.

---

# Investigation 13: Host palette translation

Inspect current C# color constants and assignment paths.

Determine:

1. which current colors correspond to first-cycle ROM roles;
2. whether the first-cycle block:
`00 04 08 0C C3 07 0B 0B`
can be reconciled with current accepted visible colors;
3. whether that reconciliation provides a defensible translation for:
`00 07 0B 08 44 03 0C 0C`;
4. whether intensity/blinking requires multiple host colors;
5. where the cycle palette should be selected;
6. how Super Zapper temporary colors interact with the base palette;
7. how to avoid coupling palette selection to tube geometry.

End with a proposed **PROJECT TRANSLATION** table for initial Level 17 colors.

Exact host RGB may remain subject to Neal's manual visual acceptance.

The mapping must still be non-arbitrary and traceable to ROM roles/current accepted translations.

---

# Investigation 14: Presentation reuse for Shape 1

Determine precisely what Level 17 should inherit from canonical Shape 1:

- geometry;
- topology;
- model-frame mapping;
- accepted Y gain;
- accepted scale;
- accepted vertical offset;
- accepted depth-Y gain;
- local entity scaling.

Determine what must not be inherited:

- displayed-level number;
- palette block;
- gameplay wave;
- movement program;
- Spike configuration;
- firing;
- speeds;
- difficulty.

Assess whether current presentation calibration is keyed too narrowly by displayed level and requires a shape-based resolver extension.

Do not perform visual tuning.

Neal will manually judge whether Level 1 calibration is acceptable when reused for Level 17.

---

# Investigation 15: Progression and direct startup

Define exact changes required for:

- natural Level 16→17 progression;
- direct `--start-level 17`;
- direct diagnostic `--level 17`, if still supported;
- HUD Level 17 identity;
- score/lives carry;
- Super Zapper recharge;
- approach/starfield;
- default accepted presentation;
- shape-row 0 selection;
- palette block 17–32;
- Level 17 configuration.

Natural and direct startup must share one configuration.

Level 17 remains terminal after completion for this milestone.

Do not design Level 18 progression.

---

# Investigation 16: Testing strategy

Recommend permanent automated coverage for implementation.

At minimum:

## Shape reuse

- Level 17 geometry object/data derives from canonical Shape 1;
- no copied Level 17 point array;
- future Shape 1 correction can affect both.

## Palette

- verified raw block;
- cycle selection;
- role mapping;
- Level 1 remains unchanged;
- Level 17 uses second-cycle scheme;
- Super Zapper override remains correct.

## Flipper/Tanker

- `$07` straight behavior;
- `$AF` translated speed;
- Tanker speed relation;
- two-Flipper cargo;
- firing.

## Pulsar

- min/max policy;
- direct admission;
- state/timing behavior;
- pulse hazard;
- firing-by-difficulty;
- ledger;
- reconstruction;
- completion;
- rendering.

## Spiker/shot

- effective movement;
- relative shot relationship;
- holdoff;
- inclusive slot semantics.

## Startup/progression

- natural 16→17;
- direct Level 17;
- HUD;
- carry/recharge;
- terminal Level 17.

## Regression

- Levels 1–16 focused regression;
- complete existing suite;
- local `TEMP/GameplayRecordings/lvl1-16.json` deterministic playback;
- Release build;
- whitespace checks.

Do not require the local recording to become a committed fixture.

---

# Investigation 17: Produce an implementation specification

Create a concise, implementation-ready specification covering:

- canonical shape reuse;
- cycle palette;
- presentation calibration inheritance;
- Level 17 configuration;
- species policy;
- movement routes;
- all translated speeds/timing;
- Tanker cargo;
- firing;
- difficulty scope;
- Pulsar parameterization;
- identity/ledger changes;
- reconstruction;
- completion;
- progression;
- direct startup;
- terminal boundary;
- tests.

Every field must include:

- value/behavior;
- source/classification;
- target C# location/mechanism;
- whether manual acceptance is required.

Do not write production code.

---

# FINAL READINESS GATE

End with exactly one:

## READY FOR BOUNDED LEVEL 17 IMPLEMENTATION

Use if:

- every required production-default value has a defensible translation;
- medium/default Level 17 behavior is fully specified;
- Pulsar integration has an architecture path;
- palette has a non-arbitrary initial translation;
- progression/startup are bounded;
- remaining uncertainty is appropriate for later manual acceptance.

or:

## NOT READY

List only concrete implementation blockers.

Do not keep Level 17 blocked merely because hard/easy difficulty or exact host RGB remains deferred, provided production medium/default behavior is fully defined and those deferred boundaries are explicit.

Do not declare readiness if production-default movement, admission, firing, identity, or completion still requires guessing.

---

# Execution limits

Use read-only source/tests/ROM inspection and small calculations.

Do not modify files other than the required paired AI prompt/report.

Do not run the full suite or game.

Do not inspect unrelated screenshots/videos/images.

If the previously supplied screenshots are not accessible, continue using ROM/current C# role mapping and leave visual acceptance to Neal.

Use explicit bounds for potentially long commands.

Follow Windows security/stall policy.

---

# Required AI artifacts

Save this prompt as:

`AI-Logs/Prompts/2026-11-25-level17-rom-to-csharp-integration-readiness-investigation-prompt.md`

Create:

`AI-Logs/Reports/2026-11-25-level17-rom-to-csharp-integration-readiness-investigation-report.md`

These are GitHub continuity records, but this investigation does not authorize commit or push.

---

# Required report contents

The report must include:

1. Executive summary
2. Starting Git state
3. Established ROM evidence accepted
4. Existing C# architecture inventory
5. `$07` C# correspondence
6. Flipper/Tanker Atari movement equation
7. Current C# Flipper/Tanker movement
8. Recommended Level 17 Flipper/Tanker translation
9. Spiker effective ROM movement
10. Current C# Spiker comparison
11. Recommended Level 17 Spiker translation
12. Effective enemy-shot speed trace
13. Recommended C# enemy-shot translation
14. Pulsar ROM/C# field comparison
15. Pulsar timing decisions
16. Pulsar production-admission policy
17. Pulsar identity/ledger design
18. Pulsar reconstruction
19. Pulsar completion obligations
20. Difficulty policy
21. Palette runtime-role trace
22. First-cycle/current-C# reconciliation
23. Proposed second-cycle host palette
24. Shape-calibration inheritance
25. Natural progression requirements
26. Direct-start requirements
27. Complete implementation specification
28. Recommended test matrix
29. Manual-acceptance questions
30. Deferred whole-game issues
31. READY/NOT READY result
32. Remaining blockers, if any
33. Files/resources inspected
34. Commands/calculations run
35. Production code changed: no
36. Tests changed: no
37. Durable docs changed: no
38. Stage/commit/push: none
39. Level 18+ investigation/implementation: none
40. Recommended next step

Keep facts separate from recommendations.

---

# Stop conditions

STOP with NOT READY if:

- medium/default Flipper/Tanker movement cannot be translated;
- effective Spiker movement cannot use an established project translation;
- enemy-shot speed lacks any defensible existing translation;
- Pulsar production state cannot be mapped without redesign/guessing;
- ledger integration remains structurally ambiguous;
- palette-role mapping cannot support a non-arbitrary initial host palette;
- progression/direct startup require unrelated architecture work;
- evidence conflicts cannot be bounded.

Do not invent values just to achieve READY.

---

# Completion criteria

This investigation is complete when it either:

- produces a complete medium/default Level 17 C# implementation specification and ends READY;

or:

- identifies the exact remaining blockers and ends NOT READY.

Then STOP.

Do not implement Level 17.

TEMPEST-END-LEVEL17-ROM-TO-CSHARP-INTEGRATION-READINESS-COMPLETE