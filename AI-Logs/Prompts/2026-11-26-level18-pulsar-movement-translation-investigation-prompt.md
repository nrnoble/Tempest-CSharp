# Tempest Level 18 Shared Pulsar Movement Translation — Bounded Read-Only Investigation

## FAIL-CLOSED COMPLETION-TOKEN CHECK

Read the entire user-provided task before taking any action.

Before tools, files, commands, modifications, tests, builds, repository actions, temporary variants, or implementation:

1. Locate the completion token near the end of this user-provided task.
2. Verify that exactly one complete token beginning with `TEMPEST-END-` appears on a line by itself in the user-provided task.
3. The token may contain only uppercase ASCII letters, digits, and hyphens after the `TEMPEST-END-` prefix.
4. VS Code/agent-generated reminders, tagged-file information, attachments, metadata, or other system-generated context after the user's completion token do not invalidate it.
5. This check exists only to detect an incomplete or truncated user task.
6. If exactly one valid standalone token cannot be found, STOP before any project/repository action and explain why.

The unique completion token is the final standalone line below.

---

# Task

Perform a **bounded, read-only investigation of Atari ROM Pulsar movement translation for displayed Level 18**.

The immediately preceding Level 18 ROM/configuration investigation established one remaining implementation blocker:

> The shared C# `Pulsar` currently uses one fixed host movement increment and does not model the ROM's configurable along-position threshold and two movement-speed regions. Level 18 changes the ROM threshold relative to Level 17 and therefore cannot safely inherit Level 17's accepted one-speed host approximation without an explicit translation decision.

Resolve that blocker sufficiently to produce an implementation-ready **shared Pulsar configuration/translation specification**.

Do not implement it.

The desired architecture is:

```
LevelConfiguration / PulsarConfiguration
        ↓
shared Pulsar actor
        ↓
ROM-derived movement policy / host translation
        ↓
shared Pulsar lifecycle
```

Not:

```
Level17Pulsar
Level18Pulsar
```

The investigation must preserve Level 17's accepted behavior as an explicit compatibility requirement.

---

# Strict scope

This task is read-only.

Do not:

- modify production code;
- modify tests;
- modify durable documentation;
- implement Level 18;
- alter Level 17;
- create a Level 18 Pulsar;
- create a second Pulsar lifecycle;
- modify gameplay recordings;
- visually tune speeds;
- inspect screenshots;
- inspect videos;
- launch the game;
- install tools/extensions;
- revisit DebugMCP;
- stage, commit, or push.

Only the paired AI prompt/report artifacts may be created.

---

# Required prior authorities

Read completely:

`AI-Logs/Reports/2026-11-25-level18-rom-configuration-readonly-investigation-report.md`

Also read as needed:

`AI-Logs/Reports/2026-11-25-level17-rom-to-csharp-integration-readiness-investigation-report.md`

`AI-Logs/Reports/2026-11-25-level17-bounded-production-implementation-report.md`

`AI-Logs/Reports/2026-11-25-level17-final-closeout-report.md`

`TEMPEST-BEHAVIOR-SPEC.md`

`TEMPEST-DEVELOPMENT-NOTES.md`

`.github/copilot-instructions.md`

Inspect the current shared:

- `Pulsar.cs`
- `TempestGame.cs`
- `LevelConfiguration.cs`
- Level 17 wave/configuration code
- relevant tests

Use the checked-in project-local Atari Rev. 2 disassembly:

`Reference/arcade/Tempest Rom Source code.md`

Do not search the Internet for ROM material.

Do not use Incitti as primary evidence.

Incitti may be used only as a secondary translation/reference check if a specific engineering question remains after ROM/current-project tracing, and must be classified REFERENCE-DERIVED.

---

# Accepted Level 17 freeze

Level 17 is manually accepted and closed.

Preserve as **EXISTING ACCEPTED PROJECT BEHAVIOR**:

- Pulsars are eligible 2–5.
- Medium/default Pulsar firing is disabled.
- Current shared C# Pulsar climb/descend movement uses the accepted Level 17 host translation.
- Level 17 gameplay has been manually accepted with no currently known defect.
- Level 17 pulse presentation/timing has been manually accepted in context.
- Level 17 identity, ledger, collision, Super Zapper, completion, rendering, rim behavior, and lifecycle are accepted.

Do not propose silently changing Level 17's gameplay because a more literal ROM model becomes available.

If a generic parameterization is recommended, it must be capable of expressing:

```
Level 17 compatibility mode / accepted translation
```

without changing current Level 17 output unless Neal separately authorizes a future fidelity investigation.

---

# Established Level 18 ROM facts

Treat the preceding investigation's findings as established unless direct local ROM evidence demonstrates a concrete contradiction.

Level 18 Pulsars:

- are eligible;
- minimum 2;
- maximum 5;
- use shared Pulsar actor semantics;
- medium/easy firing disabled;
- hard firing enabled through `$40`;
- `pulse_beat = $04`;
- `pulsar_fliprate / threshold = $14`;
- fixed `spd_pulsar = $FE.A0`;
- alternate movement region uses current Level 18 Flipper speed `$FD.60`;
- Level 18 Flipper/Tanker ROM magnitude = 2.625;
- fixed Pulsar ROM magnitude = 1.375.

Level 17 context:

- threshold `$28`;
- fixed Pulsar speed `$FE.A0`;
- pulse beat `$04`;
- current accepted host movement is approximately one host unit per update rather than a literal ROM 8.8 conversion.

Do not treat Level 17's host value as automatic evidence for Level 18.

---

# Central questions

Answer all of these:

1. What exactly does ROM code `$9CB5-$9D04` do?
2. What coordinate/value is compared against `pulsar_fliprate`?
3. Is `$14` literally an along-position threshold?
4. Which side of the threshold uses `spd_pulsar`?
5. Which side uses the current Flipper speed?
6. Does movement direction alter which speed applies?
7. How does reversal interact with the threshold?
8. How does lane-change timing interact with the threshold?
9. How does pulse-beat/pulse state interact with the threshold?
10. How does rim arrival interact with the threshold?
11. Does the threshold affect only movement speed or additional behavior?
12. How does Atari along-position correspond to C# `Pulsar.Height`?
13. Can the threshold be converted structurally into the C# depth domain?
14. How should ROM 8.8 speed ratios map into host fixed-step movement without arbitrary tuning?
15. Can one shared configuration express Level 17 compatibility and Level 18 ROM-derived behavior?
16. What exact Level 18 host values should be proposed?
17. What aspects still require Neal's manual acceptance?

---

# Phase 1: ROM routine trace

Perform a complete bounded trace of the relevant Pulsar movement/state routine around:

`$9CB5-$9D04`

Include enough upstream/downstream code to understand:

- entry state;
- P-code/state selection;
- along-position load;
- threshold comparison;
- branch direction;
- speed selection;
- signed arithmetic;
- movement update;
- reversal;
- lane flip;
- pause;
- pulse-state effects;
- return/loop behavior.

Do not quote huge unrelated disassembly ranges.

Produce pseudocode faithful to the 6502 behavior.

Example form only:

```
if along < threshold:
    use ...
else:
    use ...

if ...
    reverse ...

move using ...
```

Do not invent semantics from labels alone.

---

# Phase 2: determine speed-region orientation

Prove explicitly which region uses:

- `$FE.A0` Pulsar speed;
- current cracked Flipper speed `$FD.60`.

For Level 18 report the exact along-coordinate ranges if the ROM representation permits them.

Determine whether:

```
deep/bottom region
```

and:

```
rimward/shallow region
```

can be mapped unambiguously.

Do not use visual observation to decide orientation.

Trace the coordinate arithmetic.

---

# Phase 3: Atari along-coordinate domain

Determine:

- valid Pulsar along-position range;
- bottom/deep-end value;
- rim value;
- direction of increasing/decreasing coordinate;
- representation width/fixed point;
- lane transition behavior;
- clamp/wrap behavior;
- relationship to enemy movement calls.

Identify the meaning of:

- `$14`;
- `$28`;

within this domain.

Calculate their normalized positions if meaningful.

For example, if the along range is known:

```
normalizedThreshold =
    (threshold - deepEnd) /
    (rim - deepEnd)
```

Use the correct orientation/sign.

Do not assume `$14 / $FF` without proving the domain.

---

# Phase 4: C# Pulsar depth domain

Trace current `Pulsar` movement completely.

Determine:

- `Height` domain;
- deep/bottom value;
- rim value;
- direction convention;
- climb state;
- descend state;
- lane-change state;
- rim state;
- pause state;
- movement increments;
- decision timer;
- pulse timer;
- pulse/hazard state;
- interaction with tube `Depth`;
- whether `Height` maps linearly to Atari along coordinate;
- whether geometry/presentation transforms affect simulation depth.

Presentation projection must not be confused with gameplay depth.

---

# Phase 5: establish ROM-to-C# threshold mapping

Determine whether a generic mathematical mapping exists from Atari along threshold to C# `Height`.

Prefer:

- normalized depth;
- known accepted geometry conversion;
- existing ROM-to-host depth mapping.

Avoid:

- Level-18 magic host height;
- screenshot fitting;
- arbitrary visual tuning.

For Level 17 and Level 18 calculate:

- ROM threshold;
- normalized threshold;
- proposed corresponding C# threshold.

If no justified linear mapping exists, explain why and identify the smallest project translation required.

---

# Phase 6: speed conversion analysis

Compare ROM and host movement units.

At minimum include:

## Level 17

ROM:

- fixed Pulsar magnitude 1.375;
- applicable alternate Flipper speed from Level 17;
- threshold `$28`.

C#:

- accepted current Pulsar host movement value;
- update cadence;
- any pause/turn cadence.

## Level 18

ROM:

- fixed Pulsar magnitude 1.375;
- alternate Flipper magnitude 2.625;
- threshold `$14`.

Calculate:

- Level 18 fixed-Pulsar / Level 17 fixed-Pulsar ratio;
- Level 18 alternate / Level 17 alternate ratio;
- fast/slow ratio within Level 18;
- threshold ratio/normalized-position difference.

Determine which ratios can be preserved safely in C#.

Do not treat ROM 8.8 units as C# units unless the mapping is established.

---

# Phase 7: evaluate translation strategies

Evaluate these possible shared approaches.

## Strategy A: literal host scaling from ROM 8.8 magnitudes

Determine whether existing Flipper/Tanker translation provides a trustworthy conversion factor.

## Strategy B: ratio preservation against accepted Level 17 Pulsar behavior

Keep Level 17's accepted host slow/primary speed as anchor and preserve ROM ratios for Level 18.

## Strategy C: normalized threshold plus configurable host speeds

Configure:

- normalized threshold;
- slow-region host speed;
- fast-region host speed.

## Strategy D: existing one-speed project approximation

Determine whether ignoring the threshold remains defensible for Level 18.

Given the ROM parameter changes, do not select this solely because it avoids code changes.

## Strategy E: another evidence-backed shared translation

Describe only if supported.

For each report:

- fidelity;
- regression risk;
- architecture quality;
- compatibility with Level 17 freeze;
- validation complexity;
- arbitrary constants introduced;
- recommendation.

---

# Phase 8: Level 17 compatibility design

Any recommended shared parameterization must specify how Level 17 remains unchanged.

Possible designs include:

```
PulsarMovementPolicy.LegacyAccepted
```

plus:

```
PulsarMovementPolicy.RomTwoRegion
```

or a configuration whose values reproduce the existing accepted Level 17 host behavior.

Do not prescribe those names unless appropriate.

The requirement is behavioral:

- Level 17 gets identical current movement output;
- current Level 17 pulse/lane/rim behavior stays intact;
- Level 18 can use ROM-informed configurable two-region behavior;
- both use the same `Pulsar` actor.

Determine whether a generic policy flag is truly required or whether parameter values alone can express both.

Prefer data over level-number branches.

---

# Phase 9: classify existing Pulsar constants

Inventory current C# Pulsar values:

- movement increments;
- 51-update decision timing;
- 181-update pulse cycle;
- 18-tick lane pause;
- 14-tick rim pause;
- 12-degree pulse-flip constant;
- firing state;
- pulse color state;
- other relevant constants.

For each classify:

- direct ROM correspondence;
- scaled ROM translation;
- reference-derived;
- project decision;
- development-only;
- unrelated to Level 18 movement blocker.

Do not reopen accepted constants that the Level 18 ROM change does not require.

---

# Phase 10: determine exact configuration surface

Propose the smallest generic future configuration needed.

Possible fields to evaluate:

- ROM threshold;
- host/normalized threshold;
- primary/slow movement speed;
- alternate/fast movement speed;
- direction-region mapping;
- pulse beat;
- firing enable;
- existing pause/timing data.

Only include fields needed by actual ROM/project behavior.

Do not build an over-generalized `PulsarConfiguration` object with unused options.

For every proposed field state:

- raw ROM source;
- host translation;
- default;
- Level 17 value/mode;
- Level 18 value/mode;
- owning class.

---

# Phase 11: interaction with movement states

Determine how two-region speed selection applies during:

- climbing;
- descending;
- lane flip;
- lane pause;
- rim pause;
- rim pursuit;
- pulse hazard;
- terminal/destruction state.

The speed resolver must not inadvertently move Pulsars during states that currently pause them.

Determine exactly which movement calls should use the two-region policy.

---

# Phase 12: interaction with rendering

Verify movement policy is simulation-only.

Do not couple threshold/speed to:

- projection;
- palette;
- pulse-white rendering;
- tube geometry;
- displayed level;
- presentation calibration.

Rendering consumes resulting simulation position/state.

No presentation change should be required for this blocker.

---

# Phase 13: identity/ledger/reconstruction

Determine whether parameterizing Pulsar movement affects:

- identity;
- ledger state;
- death/reconstruction;
- fresh runtime state after death;
- pulse clock reset;
- movement-direction reset;
- completion.

Expected result:

```
movement policy is actor runtime configuration,
not identity architecture
```

If reconstruction requires preserving or reinitializing new threshold/speed state, specify how.

Do not create a new Pulsar identity type.

---

# Phase 14: deterministic-test design

Design permanent tests for future implementation.

At minimum:

## Level 17 compatibility

- existing accepted movement fingerprint/state progression unchanged;
- same climb/descend increments;
- same pause/turn behavior;
- same rim behavior.

## Level 18 threshold

- threshold maps to expected host depth;
- one side selects fixed Pulsar speed;
- other side selects Level 18 alternate speed;
- crossing the threshold changes the selected increment at the correct boundary.

## Direction

- climbing region behavior;
- descending region behavior;
- reversal does not invert region selection incorrectly.

## Pauses/transitions

- no movement during lane pause;
- no movement during rim pause;
- lane-turn behavior remains correct.

## Lifecycle

- death/reconstruction;
- Super Zapper;
- collision;
- completion;
- identity conservation.

Do not implement tests now.

---

# Phase 15: manual acceptance design

Specify what Neal should judge after future Level 18 implementation.

Avoid asking Neal to judge ROM ticks numerically.

Manual questions should be observable:

- Do Pulsars move naturally relative to other Level 18 enemies?
- Is there an obvious movement-rate change in the appropriate depth region?
- Does reversal look natural?
- Do Pulsars still pause/change lanes normally?
- Does pulse/hazard behavior remain normal?
- Does rim behavior remain normal?
- Is gameplay pressure comparable to arcade/MAME?
- Does Level 17 remain unchanged?

Do not require screenshots unless a later visual issue actually warrants them.

---

# Phase 16: implementation-ready specification

Produce a concise future implementation specification defining:

- shared actor changes;
- configuration fields;
- Level 17 compatibility values/mode;
- Level 18 values;
- threshold conversion;
- speed conversion;
- state applicability;
- reconstruction behavior;
- tests;
- regression gates;
- manual validation.

Every proposed host value must carry an evidence classification.

---

# Readiness gate

End with exactly one:

## READY FOR BOUNDED LEVEL 18 IMPLEMENTATION

Use if:

- ROM branch semantics are established;
- threshold orientation/domain are known;
- C# threshold mapping is defensible;
- host speed translation is explicit;
- same shared actor can express Level 17 and Level 18 correctly;
- Level 17 can remain behaviorally frozen;
- no target-level Pulsar subclass/hack is required.

or:

## NOT READY

List only exact unresolved blockers.

Do not implement anything during this task.

---

# Required AI artifacts

Save this prompt as:

`AI-Logs/Prompts/2026-11-26-level18-pulsar-movement-translation-investigation-prompt.md`

Create:

`AI-Logs/Reports/2026-11-26-level18-pulsar-movement-translation-investigation-report.md`

Do not stage, commit, or push.

---

# Required report contents

1. Executive summary
2. Starting Git/worktree state
3. Prior blocker
4. Level 17 freeze boundary
5. Level 17 ROM Pulsar data
6. Level 18 ROM Pulsar data
7. `$9CB5-$9D04` trace
8. Faithful ROM pseudocode
9. Threshold comparison semantics
10. Speed-region orientation
11. Atari along-coordinate domain
12. `$28` Level 17 threshold interpretation
13. `$14` Level 18 threshold interpretation
14. Current C# Height domain
15. Current C# Pulsar state machine
16. ROM-to-C# threshold mapping
17. Level 17 mapped threshold for reference
18. Level 18 mapped threshold
19. ROM speed arithmetic
20. Level 17 host movement baseline
21. Level 18 speed-ratio analysis
22. Strategy A assessment
23. Strategy B assessment
24. Strategy C assessment
25. Strategy D assessment
26. Recommended translation
27. Level 17 compatibility design
28. Current Pulsar constant classifications
29. Proposed configuration surface
30. Movement-state applicability
31. Rendering separation
32. identity/ledger/reconstruction impact
33. deterministic-test specification
34. manual-acceptance specification
35. implementation-ready specification
36. evidence-classification table
37. READY/NOT READY result
38. exact blockers if any
39. files/resources inspected
40. commands/calculations run
41. production code changed: no
42. tests changed: no
43. durable docs changed: no
44. stage/commit/push: none
45. Level 18 implementation: none
46. recommended next step

Do not abbreviate the report if the result is READY.

---

# Stop conditions

STOP rather than guessing if:

- speed-region orientation cannot be established;
- Atari along-coordinate domain remains ambiguous;
- threshold cannot be mapped into C# without arbitrary tuning;
- required Level 18 speed translation depends on screenshots/manual fitting;
- preserving Level 17 accepted behavior would require duplicating the Pulsar actor;
- a proposed solution requires `if level == 18` behavior in `Pulsar.Update`;
- ROM evidence conflicts materially with accepted Level 17 behavior and cannot be separated through configuration;
- commands stall or security blocks evidence gathering.

Do not resolve uncertainty with a magic constant.

---

# Completion criteria

Complete only when:

- two-region ROM behavior is understood;
- threshold orientation is proven;
- Level 17 and Level 18 threshold meanings are established;
- C# threshold mapping is defensible;
- ROM speed ratios are translated explicitly;
- Level 17 can remain frozen;
- Level 18 can use the shared Pulsar actor;
- required configuration fields are minimal and semantic;
- implementation and test plans are complete;
- READY or exact blockers are reported;
- no production/test/docs changes occurred.

Then STOP.

TEMPEST-END-LEVEL18-PULSAR-MOVEMENT-TRANSLATION-INVESTIGATION-COMPLETE