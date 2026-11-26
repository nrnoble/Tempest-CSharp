# Tempest Level 19 Bounded Medium/Default Production Implementation and Automated Validation

## FAIL-CLOSED COMPLETION-TOKEN CHECK

Read the entire user-provided task before taking any action.

Before tools, files, commands, modifications, tests, builds, or repository actions:

1. Locate the completion token near the end of this user-provided task.
2. Verify that exactly one complete token beginning with `TEMPEST-END-` appears on a line by itself in the user-provided task.
3. The token may contain only uppercase ASCII letters, digits, and hyphens after the `TEMPEST-END-` prefix.
4. VS Code/agent-generated reminders, tagged-file information, attachments, metadata, or other system-generated context after the user's completion token do not invalidate it.
5. This check exists only to detect an incomplete or truncated user task.
6. If exactly one valid standalone token cannot be found, STOP before project or repository action and explain why.

The unique completion token is the final standalone line below.

---

# Task

Implement displayed Level 19 as a bounded medium/default production milestone using:

- the completed Level 19 ROM/configuration investigation;
- the accepted shared architecture through Level 18;
- canonical Shape 3 presentation reuse;
- cycle-1 palette reuse;
- the existing shared `EightAdvanceTurn` Flipper route;
- the existing parameterized shared Pulsar actor;
- generic Tanker cargo, identity, ledger, reconstruction, completion, firing, and progression systems.

After implementation:

- run the required automated validation;
- do not launch the interactive game;
- provide exact manual launch commands;
- provide a concise neutral observation checklist;
- STOP.

Do not stage, commit, or push.

Manual acceptance remains pending until Neal independently reports results.

---

# Manual-testing interaction boundary

Do not prompt Neal to answer manual-testing questions.

Do not:

- present an interactive questionnaire;
- ask Neal to confirm individual items;
- wait for manual answers;
- request screenshots, video, recordings, or play-by-play observations;
- launch the game;
- claim manual acceptance.

At completion, provide launch commands and neutral observation labels, then state:

> Automated Level 19 implementation and validation are complete. Manual gameplay and presentation acceptance remain pending.

Then STOP.

---

# Required implementation authority

Read completely:

`AI-Logs/Reports/2026-11-26-level19-rom-configuration-readonly-investigation-report.md`

Also read as needed:

`AI-Logs/Reports/2026-11-26-level18-final-closeout-report.md`

`AI-Logs/Reports/2026-11-26-level18-bounded-production-implementation-report.md`

`AI-Logs/Reports/2026-11-26-level18-pulsar-movement-translation-investigation-report.md`

`TEMPEST-BEHAVIOR-SPEC.md`

`TEMPEST-DEVELOPMENT-NOTES.md`

`.github/copilot-instructions.md`

Use the Level 19 investigation as the immediate specification.

Do not repeat the ROM investigation unless current source materially contradicts it.

---

# Strict milestone boundary

This task authorizes:

- Level 19 medium/default production configuration;
- Level 19 wave selector;
- natural Level 18→19 progression;
- direct Level 19 startup;
- terminal Level 19 boundary;
- canonical Shape 3 reuse;
- existing Shape 3 production presentation reuse;
- cycle-1 palette reuse;
- existing shared `EightAdvanceTurn` route;
- Level 19 Flipper/Tanker speed configuration;
- existing shared Pulsar threshold/speed configuration;
- focused permanent tests;
- automated regression validation;
- required prompt/report artifacts.

This task does not authorize:

- Level 20 implementation;
- Level 20 progression;
- easy/hard Level 19 production variants;
- a new actor class;
- a new movement strategy;
- a target-level ledger or lifecycle;
- changes to accepted Levels 1–18 behavior;
- visual tuning;
- screenshot/image/video analysis;
- recording modification;
- game launch;
- stage, commit, or push.

---

# Starting Git/worktree protection

Before changing anything, record:

- branch;
- HEAD;
- upstream;
- ahead/behind;
- staged state;
- tracked modifications;
- untracked paths;
- current Level 19 investigation prompt/report;
- unrelated protected material.

Expected current accepted checkpoint is the latest clean Level 18 closeout on `master`.

The Level 19 investigation report recorded a later clean HEAD whose accepted Level 18 commit remained an ancestor. Verify the actual current state rather than assuming a specific SHA.

If unexpected tracked production modifications exist, STOP and report them.

Preserve:

- `Reference/arcade/MBUCOD.V05.txt`;
- ROM/reference material;
- TEMP recordings;
- screenshots;
- videos;
- user assets;
- existing AI-Logs.

Do not clean, reset, restore, stash, delete, move, or overwrite unrelated material.

---

# Accepted architectural requirements

## Eligibility before parameters

Only eligible species/features may be instantiated.

Dormant movement, speed, firing, Spike, or other values must not activate an ineligible feature.

## Shared actors

Do not create:

- `Level19Flipper`;
- `Level19Tanker`;
- `Level19Pulsar`;
- `Level19Egg`;
- Level 19 identity types;
- Level 19 ledger;
- Level 19 reconstruction;
- Level 19 completion counters.

## Semantic configuration

Use data and semantic policies, not displayed-level checks in shared actor code.

Do not create actor conditions equivalent to:

```
if displayedLevel == 19
```

## Shared rim lifecycle

The `$19`/`EightAdvanceTurn` route must end at rim arrival and enter the existing shared rim lifecycle.

## Reusable geometry

Reuse canonical Shape 3.

Do not copy geometry.

## Cycle palette

Reuse cycle-1 semantic palette roles.

Do not create Level 19 color branches.

---

# Canonical Level 19 identity

Implement one canonical Level 19 production configuration.

Required mapping:

- displayed level: 19;
- progression index: 18 under the existing zero-based convention;
- ROM `curlevel`: `$12`;
- effective one-based `lev`: `$13`;
- cycle: 2;
- cycle-local position: 3;
- zero-based cycle/remap index: 2;
- ROM shape row: 2;
- board index: 2;
- palette index: 1;
- medium/default policy;
- terminal production level for this milestone.

Natural progression and direct startup must resolve to the same configuration.

---

# Canonical Shape 3 reuse

Level 19 must reuse:

- `LevelConfiguration.Level3Mixed`, or the current canonical equivalent;
- `LevelLoader.LoadLevel3Plus()`, or current equivalent;
- canonical Shape 3 / ROM row 2;
- board index 2;
- closed topology;
- 16 lanes;
- existing ordered vertices.

Do not copy Shape 3 coordinates.

Required reuse path:

```
Displayed Level 19
    -> canonical Shape 3 / ROM row 2
    -> existing Level 3 Plus geometry
    -> existing Level 3 model frame
    -> existing Level 3 production projection/calibration
```

Presentation routing should resolve through canonical shape/row identity, not a bespoke Level 19 transform.

Preserve Level 3 numerical presentation output.

---

# Cycle-1 palette reuse

Use the accepted cycle-1 ROM block:

```
00 07 0B 08 44 03 0C 0C
```

Preserve current semantic roles:

- background: black;
- base tube/web: red;
- score/high-score/initials: cyan;
- reserve lives: green;
- displayed-level HUD: red;
- Blaster/player: yellow;
- Flipper: magenta;
- Tanker: blue;
- Egg: yellow;
- Pulsar normal: cyan;
- Pulsar pulse: white.

Preserve transient rail/pulse restoration through the active cycle palette.

Do not add Level 19 color values.

---

# Level 19 wave configuration

Implement the ROM-backed medium/default configuration.

## Original supply

- 22 original identities.

## Physical envelope

- 7 physical enemy records/units.

## Egg/admission behavior

Reuse the accepted shared policy:

- 4 initial active Eggs;
- maximum active Eggs 4;
- maximum approaching Eggs 2;
- 50-tick hatch pacing;
- reserve/re-entry pipeline;
- deterministic lane/direction sequencing;
- generic capacity blocking.

Do not create a Level 19 Egg/admission engine.

## Species ranges

- ordinary Flippers: minimum 0, maximum 3;
- Tankers: minimum 1, maximum 1;
- Spikers: minimum 0, maximum 0;
- Pulsars: minimum 2, maximum 3;
- Fuseballs: minimum 0, maximum 0.

Minimums must be satisfied through the existing ROM-style selection architecture.

Remaining capacity must use the existing bounded random/availability model.

Do not use a fixed hand-authored sequence.

---

# Spike and Spiker absence

Level 19 must have:

- `StartingSpikeHeight = null`, or equivalent absence;
- zero starting Spikes;
- zero eligible Spikers;
- no dynamic Spike-growth path;
- no Spiker projectile;
- no Spiker-to-Tanker terminal conversion.

Dormant parameter values must remain dormant.

Level 20 is outside scope even though it is the documented Spike-resumption boundary.

Do not add Level 20 code.

---

# Tanker cargo

Required Level 19 Tanker policy:

- exactly one Tanker;
- cargo species: Flipper;
- cargo count: exactly 2;
- two distinct cargo identities;
- cargo remains outside original supply;
- cargo ownership and ordinal remain generic;
- natural destruction/release remains generic;
- player-shot release remains generic;
- Super Zapper release remains generic;
- death/re-entry reconstruction remains generic;
- completion obligations remain generic.

No `Level19Tanker`.

---

# `$19` ordinary and cargo Flipper route

ROM movement byte:

```
$19
```

Use the existing shared semantic:

```
EightAdvanceTurn
```

or current equivalent.

Required behavior:

- eight along/depth advances;
- initiate one lane turn;
- hold depth during the turn;
- complete the turn;
- repeat;
- no Spike predicate;
- no explicit reverse program;
- terminate below-rim ownership at rim;
- enter shared rim handoff;
- use shared pause, pivot, pursuit, collision, shooting, and destruction.

Apply the same shared route to:

- ordinary original Flippers;
- Tanker-released cargo Flippers;
- reconstructed/re-entered cargo where applicable.

Do not duplicate the algorithm.

Do not create a Level 19 route enum when the semantic route already exists.

Preserve Levels 6 and 18 `EightAdvanceTurn` behavior.

---

# Level 19 Flipper/Tanker speed

ROM medium/default:

- raw byte: `$A9`;
- cracked signed 8.8 magnitude: `2.71875`.

Use the accepted host translation approach:

```
Flipper climb speed = 2.71875f
Tanker climb speed = 2.71875f
```

Classify host use as **PROJECT TRANSLATION**.

Tankers share the cracked Flipper speed.

Do not visually tune.

Do not implement easy/hard alternatives.

---

# Level 19 Pulsar configuration

Use the existing shared parameterized `Pulsar`.

Required medium/default data:

- eligible minimum 2;
- maximum 3;
- raw threshold `$14`;
- fixed ROM speed `$FE.A0`, magnitude 1.375;
- alternate ROM speed derived from Level 19 Flipper speed 2.71875;
- pulse beat `$04`;
- medium/default firing disabled.

## Host mapping

Reuse the accepted threshold conversion from Level 18:

```
Level.Depth * (0xF2 - 0x14) / (0xF2 - 0x10)
```

For `Level.Depth = 400`, expected host threshold is approximately:

```
392.92035f
```

Use the existing named conversion helper rather than scattering this decimal.

## Host movement values

- fixed host movement speed: `1.0f`;
- deep-side toward-rim host speed: `87f / 55f`;
- away/deep movement speed: `1.0f`.

The ratio derives from:

```
2.71875 / 1.375 = 87 / 55
```

Classification: **PROJECT TRANSLATION**.

## Shared movement behavior

Preserve the accepted shared semantics:

- pre-move Height selects toward-rim region;
- `Height <= threshold` uses deep-side speed;
- exact equality uses deep-side speed;
- above threshold uses fixed speed;
- crossing affects the next movement update;
- away/deep movement uses fixed speed;
- reverse boundary is evaluated after movement;
- no-pending generic supply uses the existing semantic pending-supply abstraction;
- lane/rim pauses do not move;
- terminal does not move;
- lateral lane direction does not select vertical speed.

Do not add Level 19 checks inside `Pulsar.Update`.

---

# Pulsar collision/pulse behavior

Reuse the accepted threshold-aware shared implementation introduced and manually accepted for Level 18.

Preserve:

- Level 17 no-threshold compatibility path;
- Level 18 threshold/speed behavior;
- existing near-rim collision priority;
- configured threshold pulse-region gating;
- shared pulse clock;
- shared collision/destruction;
- identity/reconstruction/completion.

Do not redesign collision.

Add Level 19 configuration tests proving the shared policy receives the Level 19 ratio/value.

---

# Fuseball policy

Fuseballs are ineligible.

Do not enable:

- Fuseball actor;
- Fuseball firing;
- Fuseball pending-supply policy;
- `TerminalRimward`;
- Fuseball speed/timing.

No Level 19 semantic-policy addition is required for Fuseballs.

---

# Medium/default firing

Required:

- Flipper firing enabled;
- Tanker firing enabled;
- Pulsar firing disabled;
- Spiker/Fuseball firing unreachable;
- shot holdoff: 29;
- inclusive final slot index: 2;
- actual capacity: 3 active enemy shots;
- enemy-shot raw speed: `$C0`;
- shared host enemy-shot movement: 3 units/update;
- existing random/depth/state/capacity gates preserved.

Do not enable hard-only Pulsar firing.

Do not add a Level 19 shooter loop.

---

# Identity and ledger invariants

At every boundary:

- each unresolved original identity has exactly one valid representation;
- each unresolved cargo identity has exactly one valid representation;
- original supply remains 22;
- cargo does not increase original supply;
- reserve/Egg/active/rim/terminal/cleared states remain consistent;
- Tanker cargo ownership remains exact;
- no ineligible species can appear;
- no duplicate allocation;
- no missing runtime representation;
- completion remains blocked by unresolved obligations.

Add invariant-focused tests for:

- initial wave;
- Tanker cargo;
- destruction;
- death/re-entry;
- Pulsar reconstruction;
- completion.

---

# Death and reconstruction

Use the generic reconstruction architecture.

Required:

- unresolved originals return through reserve/Egg re-entry;
- cargo identities remain cargo identities;
- active destroyed identities remain destroyed;
- movement policies are reapplied through Level 19 configuration;
- fresh actors receive fresh runtime timers/state;
- shared pulse clock lifecycle remains unchanged;
- no Level 19 resurrection/reconstruction branch.

---

# Completion

Reuse generic completion obligations.

Level 19 completion depends on:

- all applicable originals resolved;
- Tanker cargo resolved;
- reserves/Eggs/pending activations resolved;
- active/rim/terminal obligations resolved;
- no missing representation.

Do not create a Level 19 completion counter.

---

# Natural progression

Implement:

```
Level 18
    -> ExitZoom / transition
    -> Level 19 Approach
    -> Level 19 Playing
```

Preserve:

- score;
- lives;
- Super Zapper recharge;
- cycle-1 palette;
- canonical Shape 3 transition;
- normal starfield/Approach;
- displayed HUD level 19;
- one canonical Level 19 production configuration.

Do not route through a temporary or test-only configuration.

---

# Direct startup

Support:

```
--start-level 19
```

and the currently supported diagnostic form:

```
--level 19
```

Both must resolve to the exact same Level 19 production configuration used by natural progression.

Preserve existing startup score/lives/Super Zapper/restart conventions.

---

# Terminal boundary

Level 19 is terminal for this milestone.

After completion:

- remain at the existing terminal/complete boundary;
- do not progress to Level 20;
- direct Level 20 remains unsupported/rejected;
- recording validation rejects unsupported Level 20 where applicable.

Do not implement Level 20.

---

# Permanent automated coverage

Add a bounded Level 19 test suite.

## Mapping and reuse

- displayed/internal/cycle-local mapping;
- ROM row 2;
- canonical Shape 3 reuse;
- no copied coordinates;
- board index 2;
- palette index 1;
- Level 3 projection/calibration reuse;
- displayed HUD 19.

## Eligibility

- Flipper eligible;
- Tanker eligible;
- Pulsar eligible;
- Spiker ineligible;
- Fuseball ineligible;
- no starting Spikes;
- no dynamic Spikes;
- dormant parameters do not enable actors.

## Wave

- 22 original supply;
- seven physical slots;
- Flippers 0–3;
- exactly one Tanker;
- Pulsars 2–3;
- minima and maxima respected;
- cargo not counted as original supply.

## Tanker cargo

- exactly two Flippers;
- identity/ordinal ownership;
- release paths;
- reconstruction;
- Super Zapper;
- completion.

## `$19`/`EightAdvanceTurn`

- eight movement advances;
- held-depth turn;
- repeat;
- ordinary originals;
- cargo;
- reconstruction;
- generic rim handoff;
- Levels 6 and 18 remain unchanged.

## Speeds

- raw `$A9`;
- cracked `2.71875`;
- shared Flipper/Tanker host rate `2.71875f`.

## Pulsars

- threshold conversion;
- shared `$14` threshold;
- fixed speed `1f`;
- deep speed `87f/55f`;
- exact-equality branch;
- crossing;
- descending fixed speed;
- reversal;
- pause states;
- pulse/collision boundary;
- medium firing disabled;
- reconstruction;
- identity/completion.

## Firing

- holdoff 29;
- capacity 3;
- Flipper/Tanker only;
- Pulsar off;
- shared shot speed retained.

## Progression/startup

- natural Level 18→19;
- direct `--start-level 19`;
- direct `--level 19`;
- score/lives carry;
- Super Zapper recharge;
- Level 19 terminal;
- Level 20 unsupported;
- restart remains correct.

---

# Levels 1–18 regression gate

Run focused shared regressions for:

- Level 3 Shape 3 presentation;
- Level 6 `EightAdvanceTurn`;
- Level 15 semantic pending-supply policy;
- Level 15 seed-1982;
- Level 17 no-threshold Pulsar compatibility;
- Level 18 threshold-aware Pulsars;
- Level 18 progression/presentation;
- Level 18 terminal expectations updated only where Level 19 now follows naturally;
- common Flipper rim handoff;
- Tanker cargo;
- identity/ledger/reconstruction/completion;
- firing/capacity;
- cycle-0 and cycle-1 palettes.

Use explicit VSTest hang timeout.

Any deterministic change to previously accepted behavior must be investigated.

---

# Recording regression

Run unmodified:

`TEMP/GameplayRecordings/lvl1-16.json`

Require complete playback and accepted foundation telemetry.

Run unmodified:

`TEMP/GameplayRecordings/lvl-17-test.json`

Require complete input consumption and its currently accepted Level 18 outcome unless the old input demonstrably completes Level 18 and naturally enters Level 19.

Do not assume it should reach Level 19.

If its result changes:

- isolate the exact first changed boundary;
- determine whether natural Level 18→19 progression caused it;
- do not create playback-only compatibility behavior;
- do not modify the recording;
- STOP for user direction only if two explicit requirements genuinely conflict.

Do not turn expected progression into a regression.

---

# Full automated validation

After focused gates pass:

1. full solution test suite with explicit hang timeout;
2. fresh non-incremental Release build;
3. `git diff --check`.

Require:

- no failed tests;
- zero build errors;
- warnings reported accurately;
- no whitespace errors.

Canceled, timed-out, stalled, hung, or blocked commands are not passes.

---

# Manual gameplay/presentation handoff

Do not launch the game.

After automated validation, provide these commands, corrected only if actual CLI syntax differs.

## Direct Level 19

```
dotnet run --project src\Tempest.Game -- --start-level 19 --lives 10
```

## Optional overlay comparison

```
dotnet run --project src\Tempest.Game -- --start-level 19 --lives 10 --background-opacity .5
```

## Natural transition

```
dotnet run --project src\Tempest.Game -- --start-level 18 --lives 10
```

## Neutral observation checklist

Use neutral statements, not questions:

- Observe canonical Shape 3 / Level 3 presentation.
- Observe cycle-1 palette and Level 19 HUD.
- Observe ordinary Flipper eight-advance/held-depth-turn cadence.
- Observe cargo Flippers using the same route.
- Observe Level 19 Flipper/Tanker climb speed.
- Observe one Tanker with two Flipper cargo identities.
- Observe Pulsar deep-region climb and near-rim speed transition.
- Observe Pulsar descending, reversal, lane, pulse, and rim behavior.
- Observe Pulsar population remaining within the expected 2–3 selection range without requiring manual counting as an acceptance authority.
- Observe absence of Spikers, Fuseballs, and all Spikes.
- Observe Flipper/Tanker firing pressure and shot capacity.
- Observe death/re-entry and reconstruction.
- Observe natural Level 18→19 transition.
- Observe direct Level 19 startup and restart.
- Observe terminal Level 19 completion with no Level 20.
- Observe no obvious regression in Levels 1–18.

Do not ask Neal to reply to individual items.

After commands/checklist state:

> Automated Level 19 implementation and validation are complete. Manual gameplay and presentation acceptance remain pending.

Then STOP.

---

# Documentation boundary

Level 19 remains a candidate.

Do not update durable documentation to claim Level 19 is accepted.

The implementation report may record:

- ROM facts;
- project translations;
- production changes;
- automated validation;
- manual acceptance pending.

Do not stage, commit, or push.

---

# Required AI artifacts

Save this prompt as:

`AI-Logs/Prompts/2026-11-26-level19-bounded-production-implementation-prompt.md`

Create:

`AI-Logs/Reports/2026-11-26-level19-bounded-production-implementation-report.md`

Do not stage, commit, or push.

---

# Required report contents

1. Executive summary
2. Starting Git/worktree state
3. Authorities read
4. Level 19 identity/mapping
5. canonical Shape 3 reuse
6. production presentation reuse
7. cycle-1 palette reuse
8. production configuration
9. eligibility-before-parameters
10. supply and physical envelope
11. species ranges
12. Spike/Spiker absence
13. Tanker cargo
14. `$19`/`EightAdvanceTurn` reuse
15. ordinary Flipper behavior
16. cargo behavior
17. shared rim handoff
18. Flipper/Tanker speed
19. firing policy
20. shared Pulsar configuration
21. Level 17 compatibility
22. Level 18 compatibility
23. Level 19 threshold/speed behavior
24. Pulsar firing
25. Pulsar identity/reconstruction
26. progression/direct startup
27. terminal boundary
28. recording implications
29. files changed
30. tests added/changed
31. Level 3 presentation regression
32. Level 6 route regression
33. Level 15 semantic-policy regression
34. Level 15 seed-1982
35. Level 17 Pulsar regression
36. Level 18 Pulsar/progression regression
37. Levels 1–16 recording result
38. Level 17-origin recording result
39. focused test result
40. full-suite result
41. Release-build result
42. `git diff --check`
43. unresolved issues
44. evidence classifications
45. manual launch commands
46. neutral observation checklist
47. manual acceptance status: pending
48. final Git/worktree state
49. stage/commit/push: none
50. Level 20+ untouched
51. recommended next step after Neal independently reports results

Do not abbreviate because implementation passes.

---

# Stop conditions

STOP rather than guessing if:

- current source materially contradicts the Level 19 investigation;
- canonical Shape 3 reuse requires copied geometry;
- `$19` requires a new route despite the established shared match;
- Level 17 or 18 Pulsar behavior changes;
- Level 19 requires displayed-level logic inside shared actors;
- identity/cargo/completion invariants fail;
- recording divergence cannot be explained;
- Level 20 becomes reachable;
- tests/builds stall;
- Windows security blocks validation.

Do not weaken security.

Do not report partial validation as passed.

---

# Completion criteria

Complete only when:

- medium/default Level 19 is implemented;
- mapping/eligibility/wave match the investigation;
- canonical Shape 3 is reused;
- cycle-1 palette is reused;
- `$19` uses the shared `EightAdvanceTurn`;
- ordinary and cargo Flippers use the correct route;
- Flipper/Tanker speed is configured;
- Pulsars use existing shared threshold-aware movement with the Level 19 ratio;
- Level 17 remains unchanged;
- Level 18 remains unchanged;
- identities/reconstruction/completion remain generic;
- natural Level 18→19 works;
- direct Level 19 works;
- Level 19 remains terminal;
- Level 20 remains unsupported;
- recordings pass under their current meanings;
- focused tests pass;
- full suite passes;
- Release build succeeds with zero errors;
- `git diff --check` passes;
- nothing is staged, committed, or pushed;
- exact launch commands and neutral checklist are provided;
- manual acceptance remains pending.

Then STOP.

TEMPEST-END-LEVEL19-BOUNDED-PRODUCTION-IMPLEMENTATION-COMPLETE