# Tempest Level 18 ROM/Configuration Read-Only Investigation

## FAIL-CLOSED COMPLETION-TOKEN CHECK

Read the entire user-provided task before taking any action.

Before tools, files, commands, modifications, tests, builds, repository actions, or implementation:

1. Locate the completion token near the end of this user-provided task.
2. Verify that exactly one complete token beginning with `TEMPEST-END-` appears on a line by itself in the user-provided task.
3. The token may contain only uppercase ASCII letters, digits, and hyphens after the `TEMPEST-END-` prefix.
4. VS Code/agent-generated reminders, tagged-file information, attachments, metadata, or other system-generated context after the user's completion token do not invalidate it.
5. The check exists only to detect an incomplete or truncated user task.
6. If exactly one valid standalone token cannot be found, STOP before any project/repository action and explain why.

The unique completion token is the final standalone line below.

---

# Task

Perform a **read-only Atari ROM/configuration investigation for displayed Level 18**.

Do not implement Level 18.

The objective is to produce an implementation-ready Level 18 specification using the architectural rules established and accepted through Level 17.

The investigation must determine:

- reusable tube shape;
- production presentation reuse;
- palette-cycle reuse;
- exact enemy/feature eligibility;
- exact enemy supply/ranges;
- Tanker cargo;
- Flipper movement program;
- speeds/timing;
- firing;
- Spike/Spiker eligibility;
- Fuseball/Pulsar eligibility;
- difficulty-relevant data where necessary;
- progression/direct-start implications;
- all required translations into the existing shared C# architecture.

The investigation must explicitly separate:

```
Is feature/enemy eligible?
        ↓ yes
What parameters does it use?
```

A parameter value is never proof of eligibility.

---

# Strict scope

This is read-only.

Do not:

- modify production code;
- modify tests;
- modify documentation;
- modify project configuration;
- implement Level 18;
- implement Level 19+;
- create a Level 18 actor class;
- create a Level 18 ledger;
- create a Level 18 lifecycle;
- tune presentation visually;
- search or inspect screenshots;
- inspect videos;
- launch the game;
- modify recordings;
- install tools/extensions;
- revisit DebugMCP;
- stage, commit, or push.

Only the required AI prompt/report artifacts may be created.

---

# Required starting authorities

Read current durable authorities:

`TEMPEST-BEHAVIOR-SPEC.md`

`TEMPEST-DEVELOPMENT-NOTES.md`

`.github/copilot-instructions.md`

Read the final Level 17 closeout documentation as needed.

Read the Level 11/15/17 architecture audit and Level 15 shared-policy closeout as needed for architectural constraints.

Use current production source to understand existing shared strategies and configuration seams.

Use the project-local Atari ROM/disassembly/reference material where required.

Do not search the Internet for ROM material.

Do not treat Mark Incitti as primary gameplay authority.

Incitti may be used as a secondary engineering/reference source only where ROM evidence is incomplete and must be classified accordingly.

---

# Accepted architectural rules

## Eligibility before parameters

Enemy types, Spikes, projectiles, cargo types, firing modes, and other gameplay features are absent unless Atari ROM eligibility logic enables them.

Parameter data is applied only after eligibility is established.

A nonzero parameter is never sufficient proof of feature presence.

## Shared core behavior

Enemy/gameplay-object core behavior remains shared across levels unless ROM evidence establishes a genuinely different behavior mode.

Level 18 should configure shared actors, not obtain Level-18-specific actor engines.

## Semantic policy

Shared lifecycle policies should be consumed through semantic predicates/helpers rather than independent per-actor lists of level flags.

Do not introduce a new policy flag merely because Level 18 exists if existing semantic configuration already describes the behavior.

## Shared rim lifecycle

Below-rim Flipper movement strategies must hand off to the common rim lifecycle unless ROM evidence explicitly establishes otherwise.

## Canonical tube cycle

The game has 16 canonical tube shapes.

Displayed Levels 17–32 reuse those shapes in sequence.

Level 18 should therefore reuse canonical Shape 2 if the established mapping is confirmed.

Do not copy Shape 2 geometry.

## Cycle palette

Levels 17–32 use the already established cycle-1 semantic palette.

Level 18 should automatically use the same palette as Level 17 unless ROM evidence establishes otherwise.

Do not create Level 18 color overrides.

## Production presentation only

New-level development uses the accepted production presentation.

Do not develop Level 18 against legacy presentation.

Reusable shape projection/calibration should follow the canonical underlying shape.

---

# Starting Git/worktree state

Record:

- branch;
- HEAD;
- upstream;
- ahead/behind;
- staged state;
- tracked changes;
- untracked relevant files.

The accepted Level 17 and Level 15 policy closeouts should be current repository authorities.

If tracked production changes exist unexpectedly, STOP before investigation and report them.

Preserve all local recordings, screenshots, ROM/reference material, and user assets.

---

# Investigation 1: displayed/internal/cycle-local mapping

Determine exactly how displayed Level 18 maps to:

- internal ROM level index;
- zero/one-based representations;
- 16-level cycle;
- cycle-local position;
- tube-shape index;
- palette-cycle index.

Report the exact arithmetic.

Do not assume it solely from Level 17.

Explicitly establish expected relationships such as:

```
Displayed 18
cycle 2
cycle-local position 2
canonical Shape 2
palette cycle 1
```

only where code/ROM evidence confirms them.

---

# Investigation 2: canonical Shape 2 reuse

Determine the reusable geometry/presentation identity.

Establish:

- ROM shape row/index;
- corresponding existing canonical C# shape;
- topology;
- open/closed status;
- lane count;
- repeated/shared vertices if relevant;
- existing production model-frame mapping;
- existing accepted calibration;
- existing shape-specific projection behavior.

Determine which accepted previous displayed level supplies the canonical Shape 2 presentation.

Do not infer that gameplay configuration repeats with geometry.

Do not create/copied coordinates.

End with the future implementation seam:

```
Displayed Level 18
      ↓
canonical Shape 2
      ↓
existing Shape 2 geometry
      ↓
existing Shape 2 production projection/calibration
```

---

# Investigation 3: palette reuse

Confirm Level 18 selects the established Levels 17–32 palette block:

`00 07 0B 08 44 03 0C 0C`

Determine whether current cycle-palette resolver already makes Level 18 use:

- red base tube/web;
- cycle-1 HUD colors;
- player color;
- Flipper color;
- Tanker blue;
- Egg yellow;
- Pulsar colors;
- transient restoration behavior;
- other accepted cycle-1 role translations.

Do not add or recommend Level 18-specific color values.

If a currently unresolved minor semantic role becomes relevant on Level 18 because a newly eligible enemy appears, identify that as a bounded future presentation question.

---

# Investigation 4: complete feature/enemy eligibility

Before extracting parameters, determine Level 18 eligibility independently for:

- ordinary Flippers;
- Tankers;
- Spikers;
- Pulsars;
- Fuseballs;
- starting Spikes;
- dynamically grown Spikes;
- Tanker cargo species;
- enemy firing by species;
- Pulsar firing;
- any other material feature controlled by ROM policy.

For each provide:

- eligible yes/no;
- ROM routine/table/condition;
- cycle-local index;
- difficulty effect if relevant;
- confidence/classification.

Do not infer presence because a speed/height/color/movement parameter exists.

---

# Investigation 5: Spike eligibility

Use the established ROM 16-position Spike-height/eligibility cycle:

`00 00 00 E0 D8 D4 D0 C8 C0 B8 B0 A8 A0 A0 A0 A8`

Validate the mapping for displayed Level 18.

Expected hypothesis based on the established cycle:

- Level 18 → cycle-local position 2 → `$00` → no starting Spikes.

Confirm rather than assume.

Also determine separately:

- whether Spikers themselves are eligible;
- whether eligible Spikers could dynamically create Spikes despite no starting Spikes;
- whether the applicable ROM behavior permits any Spike object during Level 18.

Do not conflate starting Spikes with Spiker eligibility.

---

# Investigation 6: wave supply and species ranges

Recover Level 18 wave policy.

Determine:

- ordinary original supply;
- active/physical enemy envelope;
- Flipper minimum/maximum;
- Tanker minimum/maximum;
- Spiker minimum/maximum;
- Pulsar minimum/maximum;
- Fuseball minimum/maximum;
- difficulty adjustments;
- selection arithmetic/random thresholds;
- guaranteed versus permitted interpretations.

Do not rely on visual enemy counting.

Do not assume Level 17 counts continue.

Show underlying ROM evidence.

---

# Investigation 7: Tanker cargo

Determine Level 18 Tanker cargo.

Establish:

- Tanker eligibility;
- cargo type;
- cargo count;
- cargo load-vector/table entry;
- difficulty effects;
- cargo movement-program selection;
- release-condition inputs.

Explicitly distinguish:

```
cargo species/data
```

from:

```
shared Tanker release lifecycle
```

Do not create new release behaviors unless ROM evidence requires a genuinely new strategy.

Determine whether an existing shared cargo movement strategy already represents the Level 18 ROM behavior.

---

# Investigation 8: ordinary Flipper movement program

Recover the exact Level 18 ordinary-Flipper movement-program byte.

Trace:

- table/index;
- byte;
- P-code entry;
- route semantics;
- lane-change sequence;
- reversals;
- initial direction;
- Spike interactions;
- termination at rim.

Then compare it with existing C# movement strategies.

End with:

- exact existing reusable strategy;
- reusable strategy requiring parameter selection only;
- genuinely new ROM strategy required;
- unresolved.

Do not implement it.

Whatever below-rim program applies must converge on shared rim handoff unless ROM evidence says otherwise.

---

# Investigation 9: Flipper/Tanker movement speed

Recover Level 18 Flipper/Tanker raw movement value.

Trace through existing Atari speed cracking.

Report:

- raw byte;
- resulting Atari fixed-point representation;
- Level 17 value for context;
- Level 18 value;
- current C# translation strategy;
- whether an exact/shared C# conversion already exists;
- whether ratio-preservation is required;
- whether an existing project translation should be reused.

Do not manually tune speed.

---

# Investigation 10: Spiker policy

If Spikers are eligible:

Determine:

- min/max;
- raw/effective speed;
- movement;
- retreat;
- terminal conversion;
- firing;
- projectile relationship;
- interaction with Spike eligibility.

Preserve the accepted project invariant that Spiker shots must travel visibly faster than the firing Spiker unless authoritative evidence establishes and resolves a conflict.

If Spikers are ineligible, record dormant parameters separately and do not promote them into features.

---

# Investigation 11: Pulsar policy

Determine:

- eligibility;
- min/max;
- movement speed;
- flip/pulse parameters;
- firing eligibility;
- difficulty effect;
- existing `Pulsar` configuration reuse.

Level 18 must use the shared Pulsar actor.

Do not create `Level18Pulsar`.

Determine whether Level 17's project translations remain applicable or Level 18 selects different ROM parameters.

---

# Investigation 12: Fuseball policy

Determine:

- eligibility;
- min/max;
- speed/timing parameters;
- pending-supply/terminal-policy applicability;
- shared semantic pending-supply predicate;
- whether Level 18 uses an existing shared lifecycle policy.

This is important after the Level 15 correction.

Do not add Level 18 to a list of policy flags merely to make it work.

Determine the semantic rule first.

If Fuseballs are ineligible, dormant parameters remain dormant.

---

# Investigation 13: firing and enemy-shot pressure

Recover Level 18 firing policy:

- shot holdoff;
- slot field and count/index semantics;
- actual capacity;
- enemy-shot speed;
- eligible shooter species;
- random/depth/state gates;
- Pulsar firing;
- difficulty changes.

Compare against existing shared shooter configuration.

Do not duplicate firing loops.

---

# Investigation 14: Egg/admission behavior

Determine whether Level 18 requires any new admission/hatching behavior.

Expected architecture:

```
shared Egg
   ↓
ROM-selected species
   ↓
shared actor factory
```

Determine:

- initial Eggs;
- reserve behavior;
- species selection;
- admission capacity;
- whether all eligible species use existing factory/ledger paths.

No Level 18 Egg should be created as a behavior-specific class.

---

# Investigation 15: death/reconstruction

Read-only verify that Level 18's eligible species can all use the existing generic identity/reconstruction architecture.

For every eligible species answer:

- original identity already supported?
- cargo identity if relevant?
- reserve/Egg reconstruction supported?
- active representation supported?
- terminal state supported?
- no Level-specific reconstruction required?

If a new species combination reveals a missing generic representation, identify it as an implementation blocker.

Do not implement the correction.

---

# Investigation 16: completion obligations

Determine whether Level 18 requires any new completion behavior.

Expected:

- generic original identities;
- generic cargo identities;
- reserves;
- Eggs;
- pending activations;
- active actors;
- terminal actors;
- runtime representation;

remain sufficient.

If a Level 18 feature would require another completion counter or special branch, investigate whether that indicates a missing generic lifecycle.

Do not add one.

---

# Investigation 17: semantic policy mapping

Inventory every shared lifecycle policy Level 18 should participate in.

Examples:

- generic pending-supply lifecycle;
- terminal enemy conversion/movement;
- generic death reconstruction;
- ordinary Flipper selection;
- Tanker cargo selection;
- firing;
- Spike eligibility.

For each, identify the **semantic policy/property**, not a list of level numbers.

Determine whether current configuration architecture can express Level 18 without adding repetitive independent flag checks.

If not, flag the smallest architectural gap before implementation.

---

# Investigation 18: progression and direct startup

Determine requirements for:

- natural Level 17→18 progression;
- `--start-level 18`;
- diagnostic `--level 18` if still supported;
- HUD 18;
- score/lives carry;
- Super Zapper recharge;
- existing approach/starfield;
- palette remains cycle 1;
- Shape 2 production presentation;
- Level 18 terminal after completion for this milestone;
- Level 19 remains unsupported.

Natural and direct startup must use one shared Level 18 configuration.

Do not implement.

---

# Investigation 19: presentation implications

Presentation should primarily be reuse.

Determine:

- Shape 2 canonical presentation source;
- model frame;
- projection;
- calibration;
- entity scaling;
- cycle-1 palette;
- whether any newly eligible Level 18 enemy has an unresolved cycle-1 semantic color.

Do not tune projection.

Do not look at screenshots.

If existing shape/palette architecture fully covers Level 18, say so explicitly.

---

# Investigation 20: Level 18 implementation-readiness matrix

Produce one consolidated table containing every future configuration field.

At minimum:

- displayed level;
- internal/cycle-local index;
- shape;
- palette;
- supply;
- initial Eggs;
- active envelope;
- Flipper min/max;
- Tanker min/max;
- Spiker min/max;
- Pulsar min/max;
- Fuseball min/max;
- starting-Spike eligibility/value;
- dynamically generated Spike policy;
- Tanker cargo;
- Flipper movement program;
- Flipper/Tanker speed;
- Spiker speed;
- Pulsar parameters;
- shot holdoff;
- shot capacity;
- shot speed;
- firing species;
- difficulty-related changes;
- reconstruction policy;
- completion policy;
- production presentation reuse;
- progression/startup;
- terminal boundary.

For every row include:

- value;
- evidence source;
- classification;
- existing C# seam;
- new code required yes/no;
- manual acceptance needed yes/no.

Do not leave a required field implicit.

---

# Investigation 21: compare implementation complexity with Level 17

Explicitly identify which Level 18 behavior requires:

## Configuration only

## Existing shared strategy

## New reusable ROM strategy

## Generic architecture extension

## Project translation

## Manual visual/gameplay acceptance only

The goal is to make Level 18 implementation as data-driven as the current architecture permits.

Do not create implementation code.

---

# Investigation 22: regression plan

Recommend permanent automated coverage for future implementation.

At minimum:

- Level 18 eligibility;
- enemy composition ranges;
- Spike eligibility;
- Flipper movement program;
- Tanker cargo;
- movement speeds;
- firing/capacity;
- semantic lifecycle policies;
- identity ledger;
- reconstruction;
- completion;
- Shape 2 reuse;
- cycle-1 palette;
- natural Level 17→18;
- direct Level 18;
- Level 19 remains unsupported;
- Levels 1–17 regression;
- Level 15 policy regression;
- gameplay recordings where applicable.

Do not run broad regression tests during this investigation.

---

# Evidence classifications

Every material finding must use an explicit classification:

- ROM-BACKED
- ARCADE/MAME-OBSERVED
- EXISTING ACCEPTED PROJECT BEHAVIOR
- PROJECT TRANSLATION / PROJECT DECISION
- REFERENCE-DERIVED
- UNRESOLVED

Do not convert a project translation into a ROM fact merely because it worked on Level 17.

---

# Readiness gate

End with exactly one:

## READY FOR BOUNDED LEVEL 18 IMPLEMENTATION

Use only if:

- eligibility is known for every relevant enemy/feature;
- wave ranges are established;
- Spike/Spiker distinction is established;
- movement programs are known;
- cargo is known;
- required speeds/timing/firing are established or use explicitly accepted translations;
- shared policy mapping is known;
- no actor/lifecycle hack is required;
- Shape 2/palette reuse is established;
- progression/direct startup are bounded.

or:

## NOT READY

List only concrete blockers.

Do not block implementation merely because manual visual acceptance will still be required.

Do not implement Level 18 from within this task.

---

# Required AI artifacts

Save this prompt as:

`AI-Logs/Prompts/2026-11-25-level18-rom-configuration-readonly-investigation-prompt.md`

Create:

`AI-Logs/Reports/2026-11-25-level18-rom-configuration-readonly-investigation-report.md`

Do not stage, commit, or push.

---

# Required report contents

1. Executive summary
2. Starting Git/worktree state
3. Evidence authorities
4. Displayed/internal/cycle-local mapping
5. Shape 2 reuse
6. Shape 2 production presentation reuse
7. Cycle-1 palette reuse
8. Eligibility-before-parameters table
9. Starting Spike eligibility
10. Spiker eligibility
11. Dynamic Spike policy
12. Wave supply
13. Species min/max ranges
14. Difficulty effects
15. Tanker cargo
16. Flipper movement byte
17. Flipper P-code trace
18. Existing C# movement-strategy match
19. Flipper/Tanker speed
20. Spiker speed/behavior
21. Pulsar policy
22. Fuseball policy
23. Firing eligibility
24. Shot holdoff/capacity/speed
25. Egg/admission behavior
26. Ledger/identity compatibility
27. Death/reconstruction compatibility
28. Completion compatibility
29. Semantic lifecycle-policy mapping
30. Natural Level 17→18 progression
31. Direct Level 18 startup
32. terminal boundary
33. presentation implications
34. consolidated implementation-readiness matrix
35. configuration-only items
36. reusable shared-strategy items
37. new reusable-strategy requirements
38. architecture-extension requirements
39. project translations
40. manual-acceptance requirements
41. recommended regression/test plan
42. READY/NOT READY result
43. exact blockers if any
44. files/resources inspected
45. commands/calculations run
46. production code changed: no
47. tests changed: no
48. durable docs changed: no
49. stage/commit/push: none
50. Level 19+ implementation: none
51. recommended next step

Do not shorten the report because the result is READY.

---

# Stop conditions

STOP rather than guessing if:

- feature eligibility cannot be established;
- displayed/internal indexing remains ambiguous;
- an enemy min/max interpretation remains ambiguous;
- starting-Spike versus Spiker behavior cannot be separated;
- Flipper movement program cannot be established;
- Tanker cargo cannot be established;
- required shared lifecycle policy cannot be expressed without another target-level workaround;
- ROM evidence conflicts materially with accepted arcade observation;
- required data can only be obtained by changing production code;
- commands stall or security blocks evidence gathering.

Do not compensate with neighboring-level assumptions.

---

# Completion criteria

Complete only when:

- Level 18 configuration is fully specified or exact blockers are identified;
- every enemy/feature has explicit eligibility;
- parameters are applied only after eligibility;
- Wave composition comes from ROM evidence rather than manual counting;
- movement and cargo strategies are established;
- shared lifecycle compatibility is verified;
- no Level-18-specific actor workaround is proposed;
- canonical Shape 2 and cycle-1 palette reuse are established;
- progression/startup scope is bounded;
- a detailed implementation/test plan exists;
- no project behavior or code changed.

Then STOP.

TEMPEST-END-LEVEL18-ROM-CONFIGURATION-READONLY-INVESTIGATION-COMPLETE