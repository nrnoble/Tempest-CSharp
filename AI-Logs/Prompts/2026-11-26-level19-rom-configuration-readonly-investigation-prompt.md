# Tempest Level 19 ROM/Configuration Read-Only Investigation

## FAIL-CLOSED COMPLETION-TOKEN CHECK

Read the entire user-provided task before taking any action.

Before tools, files, commands, modifications, tests, builds, repository actions, or implementation:

1. Locate the completion token near the end of this user-provided task.
2. Verify exactly one complete token beginning with `TEMPEST-END-` appears on a line by itself in the user-provided task.
3. The token may contain only uppercase ASCII letters, digits, and hyphens after the `TEMPEST-END-` prefix.
4. VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context after the user's completion token do not invalidate it.
5. This check exists only to detect an incomplete or truncated user task.
6. If exactly one valid standalone token cannot be found, STOP before any project/repository action and explain why.

The unique completion token is the final standalone line below.

---

# Task

Perform a **read-only Atari ROM/configuration investigation for displayed Level 19**.

Do not implement Level 19.

The objective is to produce a complete, implementation-ready Level 19 specification using the accepted architecture through Level 18.

Determine:

- displayed/internal/cycle-local mapping;
- canonical tube shape;
- production presentation reuse;
- palette-cycle reuse;
- every enemy/feature eligibility rule;
- original wave supply;
- species min/max ranges;
- physical enemy envelope;
- starting/dynamic Spike eligibility;
- Tanker cargo;
- ordinary and cargo Flipper movement program;
- Flipper/Tanker speed;
- Spiker policy if eligible;
- Pulsar movement parameters;
- Fuseball policy if eligible;
- firing species;
- shot holdoff/capacity/speed;
- relevant difficulty effects;
- Egg/admission behavior;
- identity/ledger compatibility;
- reconstruction;
- completion;
- progression/direct startup;
- terminal Level 19 boundary;
- presentation implications;
- all project translations needed by existing shared actors.

The investigation must follow:

```
Is an enemy/feature eligible?
        ↓ yes
What parameters apply?
```

Never infer eligibility from dormant parameter data.

---

# Strict read-only scope

Do not:

- modify production source;
- modify tests;
- modify durable documentation;
- implement Level 19;
- implement Level 20+;
- create target-level actors;
- create target-level ledger/reconstruction/completion;
- tune presentation;
- inspect screenshots/images;
- inspect videos;
- launch the game;
- change recordings;
- install tools;
- revisit DebugMCP;
- stage, commit, or push.

Only the paired prompt/report artifacts may be created.

---

# Required authorities

Read current:

`TEMPEST-BEHAVIOR-SPEC.md`

`TEMPEST-DEVELOPMENT-NOTES.md`

`.github/copilot-instructions.md`

Read completely:

`AI-Logs/Reports/2026-11-26-level18-final-closeout-report.md`

`AI-Logs/Reports/2026-11-25-level18-rom-configuration-readonly-investigation-report.md`

`AI-Logs/Reports/2026-11-26-level18-pulsar-movement-translation-investigation-report.md`

Use the current production implementation to understand reusable configuration/strategy seams.

Use:

`Reference/arcade/Tempest Rom Source code.md`

as the local ROM/disassembly authority.

Do not search the Internet for ROM data.

Do not use Mark Incitti as primary gameplay authority.

Incitti may be used only as a secondary engineering/reference check where ROM evidence remains insufficient, and any such finding must be REFERENCE-DERIVED.

---

# Accepted architectural baseline

## Eligibility before parameters

Features and enemy types are absent unless ROM eligibility enables them.

Dormant speed, movement, firing, height, palette or other parameter values do not create actors/features.

## Shared actors

Level 19 should configure existing:

- Flipper;
- Tanker;
- Spiker;
- Pulsar;
- Fuseball;
- Egg;
- projectiles;

rather than create Level 19 actors.

## Semantic lifecycle policies

Use semantic configuration/policy seams.

Do not introduce new independent lists of:

```
Level17 || Level18 || Level19
```

at actor-update sites.

## Shared rim lifecycle

Below-rim Flipper strategies end at rim and enter the shared rim lifecycle unless ROM evidence proves otherwise.

## Canonical shapes

Displayed Levels 17–32 reuse the 16 canonical shapes.

Determine Level 19's canonical row/shape from ROM remapping.

Do not copy geometry.

## Palette cycles

Displayed Levels 17–32 use cycle-1 palette unless ROM evidence establishes otherwise.

Do not create Level 19 palette overrides.

## Production presentation

Use canonical shape presentation reuse.

Do not use legacy presentation.

---

# Starting repository state

Record:

- branch;
- HEAD;
- upstream;
- ahead/behind;
- staged state;
- tracked modifications;
- untracked paths.

Expected accepted Level 18 checkpoint:

```
3034c227ae855061c6f2ae8bb193cc9e43405566
Complete accepted Level 18 production
```

Verify rather than assume.

If tracked production modifications exist, STOP before investigation.

Preserve protected recordings/assets/reference material.

---

# Investigation 1: level mapping

Determine exactly:

- displayed Level 19;
- ROM `curlevel`;
- effective one-based `lev`;
- cycle number;
- cycle-local position;
- zero-based cycle index;
- remap index;
- shape row;
- board index;
- palette index.

Show the arithmetic.

Do not infer simply by adding one to Level 18.

---

# Investigation 2: canonical shape reuse

Determine:

- ROM shape row;
- corresponding canonical first-cycle shape;
- existing C# LevelConfiguration/LevelDefinition;
- lane count;
- topology;
- repeated/shared vertices if any;
- open/closed state;
- model-frame mapping;
- accepted production projection;
- accepted calibration.

End with the required future reuse path:

```
Displayed Level 19
    -> canonical Shape N
    -> existing geometry
    -> existing model frame
    -> existing production projection/calibration
```

Do not create coordinates.

---

# Investigation 3: palette reuse

Determine Level 19 palette index/block.

Verify whether it uses the accepted cycle-1 block:

```
00 07 0B 08 44 03 0C 0C
```

Verify existing semantic color resolver coverage for every enemy actually eligible on Level 19.

If an eligible species introduces a role whose cycle-1 translation remains unresolved, identify that as a bounded presentation question.

Do not tune colors.

---

# Investigation 4: complete enemy/feature eligibility table

Determine eligibility independently for:

- ordinary Flippers;
- Tankers;
- Spikers;
- Pulsars;
- Fuseballs;
- starting Spikes;
- dynamically grown Spikes;
- Tanker cargo;
- Flipper firing;
- Tanker firing;
- Spiker firing;
- Pulsar firing;
- Fuseball firing;
- other material ROM-gated features.

For each include:

- yes/no;
- exact ROM table/routine/condition;
- cycle-local position/index;
- difficulty dependency;
- classification.

Do not infer from speed/movement bytes.

---

# Investigation 5: Spike/Spiker policy

Use and verify the established Spike cycle:

```
00 00 00 E0 D8 D4 D0 C8 C0 B8 B0 A8 A0 A0 A0 A8
```

Determine Level 19's cycle-local entry.

Independently determine:

- Spiker min/max;
- whether any Spiker can be selected;
- whether dynamic Spike growth is reachable;
- whether dormant Spiker speed/firing data exists;
- whether any Spike may appear at all.

This is the last level before the user-observed Level 20 Spike-resumption boundary.

Explicitly verify that boundary but do not implement Level 20.

---

# Investigation 6: original wave supply

Recover medium/default Level 19 original supply.

Trace:

- table opcode;
- base;
- increment;
- effective `lev`;
- result.

Also derive easy/hard supply where the same ROM logic provides it, but do not make those production scope unless explicitly implemented later.

Distinguish:

- original identities;
- active physical slots;
- Tanker cargo identities.

---

# Investigation 7: species min/max

Recover Level 19:

- Flipper min/max;
- Tanker min/max;
- Spiker min/max;
- Pulsar min/max;
- Fuseball min/max.

Explain ROM interpretation:

- minimum target;
- maximum eligibility/capacity;
- simultaneous physical envelope;
- latent Tanker cargo effects;
- random selection after minima.

Do not replace ROM selection with a fixed sequence.

---

# Investigation 8: physical envelope and admission

Determine:

- maximum physical enemy record count;
- interaction with Tanker latent cargo;
- Egg admission;
- reserve behavior;
- minimum-selection order;
- random-start behavior;
- capacity blocking.

Determine whether Level 18's selector architecture is directly reusable or needs new semantic configuration.

Do not change source.

---

# Investigation 9: Tanker cargo

Recover all applicable Level 19 cargo entries.

Determine:

- Tanker eligibility;
- cargo species;
- cargo count;
- difficulty variants;
- ordinary/cargo identity implications;
- movement program assigned after release;
- reconstruction/re-entry route.

Classify cargo data independently from shared Tanker lifecycle.

No Level19Tanker.

---

# Investigation 10: ordinary Flipper movement

Recover Level 19's ROM movement byte from the 16-position movement table.

Fully trace its P-code semantics:

- movement cadence;
- flips;
- reversals;
- direction;
- depth holds;
- Spike checks;
- rim handoff.

Compare with existing shared C# movement strategies.

Classify as:

- configuration-only existing route;
- existing route needing semantic exposure;
- genuinely new reusable ROM strategy;
- unresolved.

Do not implement.

---

# Investigation 11: cargo Flipper route

Determine whether released Tanker cargo uses:

- ordinary Level 19 Flipper program;
- another cargo-specific ROM route;
- parameters selected by source table.

Compare with current semantic cargo-route options.

Avoid assuming ordinary/cargo are identical without ROM evidence.

---

# Investigation 12: Flipper/Tanker movement speed

Recover medium/default Level 19 speed:

- raw byte;
- cracked 8.8 word;
- magnitude;
- Tanker copy rule;
- easy/hard variations.

Determine the appropriate host translation using accepted cross-level translation rules.

Do not visually tune.

Report whether the existing speed conversion/generalization now supports Level 19 without another level-specific constant.

---

# Investigation 13: Pulsar ROM policy

If Pulsars are eligible, determine:

- min/max;
- raw fixed Pulsar speed;
- raw threshold;
- mapped threshold;
- alternate/Flipper speed;
- pulse beat;
- firing eligibility;
- hard/easy changes;
- movement/reversal regions.

Compare directly with the newly parameterized shared Pulsar movement infrastructure accepted in Level 18.

Determine whether Level 19 is now:

- configuration only;
- needs another generic shared capability;
- blocked.

Do not alter Level 18/17 accepted behavior.

---

# Investigation 14: Fuseball policy

If Fuseballs are eligible, determine:

- min/max;
- movement behavior;
- pending-supply policy;
- terminal behavior;
- firing eligibility;
- speed;
- shared semantic lifecycle property.

The new Level 15 semantic-policy work should prevent independent flag lists.

Determine whether Level 19 naturally participates through an existing semantic policy or requires a new semantic concept.

Do not add Level 19 to arbitrary policy expressions.

---

# Investigation 15: Spiker policy

If eligible, recover:

- speed;
- movement;
- projectile behavior;
- Spike growth;
- retreat;
- terminal Tanker conversion;
- pending-supply policy;
- identity lifecycle.

Preserve the accepted faster-projectile-than-Spiker invariant unless ROM/arcade evidence requires resolving a conflict.

If ineligible, classify all parameters dormant.

---

# Investigation 16: firing policy

Recover:

- shot holdoff;
- inclusive last-slot index;
- actual capacity;
- shot-speed raw value;
- cracked value;
- firing species;
- random thresholds;
- depth/state gate;
- difficulty changes.

Compare to shared C# firing architecture.

Do not introduce species-specific firing code unless ROM behavior truly differs.

---

# Investigation 17: Pulsar firing

Independently determine whether Pulsars fire on Level 19 medium/default.

Do not infer firing from Pulsar eligibility.

If firing remains hard-only, record dormant hard behavior but do not enable it.

---

# Investigation 18: Egg/admission behavior

Determine whether Level 19 requires a new Egg or admission implementation.

Expected result is reuse of:

- 4 active Eggs;
- reserve;
- approach limit;
- deterministic lane/direction sequence;
- generic species selector;
- shared actor factory.

Classify these as existing project behavior unless ROM-backed.

No Level19Egg.

---

# Investigation 19: identity and ledger compatibility

For every eligible species determine whether existing generic identity representation supports:

- original identity;
- cargo identity;
- reserve;
- Egg;
- active;
- rim;
- terminal;
- destruction/escape;
- reconstruction;
- completion.

If any eligible combination would require a Level 19-specific ledger rule, investigate whether that indicates a missing generic architecture seam.

Do not implement one.

---

# Investigation 20: death/reconstruction

Trace Level 19 eligible species through:

- player death;
- reserve;
- Egg reconstruction;
- cargo ownership;
- fresh runtime state;
- movement-policy reapplication;
- terminal state.

Determine whether current generic reconstruction is sufficient.

Do not create Level 19 reconstruction paths.

---

# Investigation 21: completion

Determine whether Level 19 can use the existing generic completion obligations.

No special completion counter should be added unless authoritative behavior requires it.

Identify any unresolved obligation representation as a blocker.

---

# Investigation 22: semantic policy map

Produce a table of every shared policy Level 19 should participate in.

Examples:

- species selection;
- generic reconstruction;
- pending supply;
- terminal conversion;
- Tanker cargo;
- rim lifecycle;
- firing;
- Pulsar parameterization;
- Spike eligibility.

For each identify:

- semantic property/seam;
- configuration value;
- new code required or not.

Flag any need for target-level numeric checks.

---

# Investigation 23: natural progression

Determine requirements for:

```
Level 18 -> Level 19
```

Preserve:

- ExitZoom;
- starfield/Approach;
- score/lives carry;
- Super Zapper recharge;
- palette cycle 1;
- canonical shape transition;
- one Level 19 production configuration.

Do not implement.

---

# Investigation 24: direct startup

Determine how:

```
--start-level 19
```

and supported:

```
--level 19
```

should resolve to the same Level 19 configuration.

Do not create separate direct behavior.

---

# Investigation 25: terminal boundary

For the bounded future implementation:

- Level 19 becomes terminal;
- Level 20 remains unsupported.

Do not implement Level 20.

The Level 20 Spike boundary may be documented as evidence only.

---

# Investigation 26: recording implications

Determine how existing recordings should be interpreted after future Level 19 progression.

At minimum:

## `lvl1-16.json`

Should remain an accepted foundation oracle.

## `lvl-17-test.json`

Currently naturally enters Level 18 and exhausts its input there.

Determine whether adding Level 19 should alter its expected result based on whether that old input reaches Level 18 completion.

Do not modify recordings.

Do not invent playback compatibility behavior.

Recommend recording policy for Level 19 only if useful after manual acceptance.

---

# Investigation 27: implementation-readiness matrix

Produce a complete future Level 19 table covering:

- displayed/internal level;
- cycle-local position;
- shape;
- palette;
- topology;
- supply;
- Egg pipeline;
- physical envelope;
- species min/max;
- Spike eligibility;
- Spiker eligibility;
- cargo;
- movement programs;
- movement speeds;
- Pulsar data;
- Fuseball data;
- firing;
- projectile capacity/speed;
- reconstruction;
- completion;
- presentation;
- progression;
- direct startup;
- terminal boundary.

For every field state:

- value;
- evidence;
- classification;
- existing C# seam;
- new code required yes/no;
- manual acceptance needed yes/no.

---

# Investigation 28: implementation complexity classification

Separate all Level 19 work into:

## Configuration only

## Existing shared strategy

## New reusable ROM strategy

## Generic architecture extension

## Project translation

## Manual acceptance only

The goal is for Level 19 to remain configuration-driven where the shared architecture already supports the ROM behavior.

No target-level actor/lifecycle workaround is acceptable merely for convenience.

---

# Investigation 29: regression plan

Recommend focused permanent coverage for future implementation.

At minimum:

- mapping;
- canonical shape reuse;
- cycle-1 palette;
- eligibility;
- supply/ranges;
- Spikes/Spikers;
- Tanker cargo;
- Flipper movement;
- Pulsar behavior;
- Fuseball behavior if applicable;
- speeds;
- firing;
- identity;
- reconstruction;
- completion;
- natural Level 18→19;
- direct Level 19;
- terminal Level 19;
- Level 20 unsupported;
- Levels 1–18 regression;
- Level 17 Pulsar compatibility;
- Level 18 Pulsar movement;
- Level 15 semantic-policy regression;
- recordings.

Do not run broad tests during this read-only investigation.

---

# Evidence classifications

Use:

- ROM-BACKED
- ARCADE/MAME-OBSERVED
- EXISTING ACCEPTED PROJECT BEHAVIOR
- PROJECT TRANSLATION
- PROJECT DECISION
- REFERENCE-DERIVED
- UNRESOLVED

Do not promote previous project translations into ROM facts.

---

# Readiness result

End with exactly one:

## READY FOR BOUNDED LEVEL 19 IMPLEMENTATION

Use only if:

- eligibility is established;
- complete wave configuration is known;
- movement programs are known;
- cargo is known;
- required movement translations are known;
- Pulsar/Fuseball/Spiker behavior is representable through existing shared actors or clearly specified reusable strategies;
- no target-level behavior hack is required;
- shape/palette/presentation reuse is known;
- progression/startup is bounded.

or:

## NOT READY

List exact blockers only.

Do not implement from this task.

---

# Required AI artifacts

Save this prompt as:

`AI-Logs/Prompts/2026-11-26-level19-rom-configuration-readonly-investigation-prompt.md`

Create:

`AI-Logs/Reports/2026-11-26-level19-rom-configuration-readonly-investigation-report.md`

Do not stage, commit, or push.

---

# Required report contents

1. Executive summary
2. Starting Git/worktree state
3. Evidence authorities
4. Level mapping
5. canonical shape reuse
6. production presentation reuse
7. palette reuse
8. eligibility table
9. Spike/Spiker policy
10. Level 20 Spike-boundary confirmation
11. original supply
12. species min/max
13. physical envelope/admission
14. Tanker cargo
15. ordinary Flipper movement byte
16. Flipper P-code semantics
17. existing C# movement-strategy match
18. cargo route
19. Flipper/Tanker speed
20. Pulsar policy
21. shared Pulsar configuration match
22. Fuseball policy
23. Spiker policy
24. firing policy
25. Pulsar firing
26. shot holdoff/capacity/speed
27. Egg/admission reuse
28. identity/ledger compatibility
29. death/reconstruction
30. completion
31. semantic policy map
32. natural Level 18→19 progression
33. direct Level 19 startup
34. terminal boundary
35. recording implications
36. presentation implications
37. implementation-readiness matrix
38. configuration-only work
39. shared-strategy work
40. new reusable-strategy work
41. architecture-extension work
42. project translations
43. manual-acceptance requirements
44. regression plan
45. READY/NOT READY result
46. exact blockers
47. files/resources inspected
48. commands/calculations run
49. production changed: no
50. tests changed: no
51. durable docs changed: no
52. stage/commit/push: none
53. Level 20+ implementation: none
54. recommended next step

Do not abbreviate because the result is READY.

---

# Stop conditions

STOP rather than guessing if:

- indexing/mapping is ambiguous;
- any species eligibility is ambiguous;
- Spike versus Spiker behavior cannot be separated;
- supply/min/max semantics remain ambiguous;
- cargo cannot be established;
- Flipper movement program cannot be established;
- Pulsar/Fuseball/Spiker rules cannot be represented safely;
- shared architecture would require a Level 19-specific bypass;
- a required host translation lacks an evidence-backed basis;
- ROM evidence conflicts materially with accepted arcade observations;
- commands stall or security blocks evidence gathering.

Do not use neighboring-level assumptions as a substitute.

---

# Completion criteria

Complete only when:

- Level 19 is fully specified or exact blockers are known;
- eligibility is separate from parameters;
- enemy composition comes from ROM evidence;
- shape/palette reuse is established;
- movements/cargo/firing are known;
- shared lifecycle compatibility is verified;
- no Level 19 actor hack is proposed;
- progression/startup/terminal boundary are bounded;
- a detailed implementation/regression plan exists;
- no production/test/docs changes occurred.

Then STOP.

TEMPEST-END-LEVEL19-ROM-CONFIGURATION-READONLY-INVESTIGATION-COMPLETE