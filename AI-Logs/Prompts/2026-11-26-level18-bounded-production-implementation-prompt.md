# Tempest Level 18 Bounded Production Implementation and Automated Validation

## FAIL-CLOSED COMPLETION-TOKEN CHECK

Read the entire user-provided task before taking any action.

Before tools, files, commands, modifications, tests, builds, or repository actions:

1. Locate the completion token near the end of this user-provided task.
2. Verify that exactly one complete token beginning with `TEMPEST-END-` appears on a line by itself in the user-provided task.
3. The token may contain only uppercase ASCII letters, digits, and hyphens after the `TEMPEST-END-` prefix.
4. VS Code or agent-generated reminders, tagged-file information, attachments, metadata, or other system-generated context appearing after the user's completion token do not invalidate it.
5. This check exists only to detect an incomplete or truncated user task.
6. If exactly one complete standalone token cannot be found, STOP before any project or repository action and explain why.

The unique completion token is the final standalone line below.

---

# Task

Implement displayed Level 18 as a bounded production milestone using:

- the completed ROM/configuration investigation;
- the completed shared Pulsar movement-translation investigation;
- the accepted shared architecture through Level 17;
- the accepted Level 15 semantic-policy correction.

After implementation:

- run the required automated validation;
- do not launch the interactive game;
- provide exact manual launch commands;
- provide a concise manual observation checklist;
- STOP.

## Important manual-testing boundary

Do **not** prompt Neal to answer manual-testing questions.

Do not:

- ask Neal to confirm each checklist item;
- present a questionnaire;
- wait for interactive answers;
- request screenshots or recordings;
- ask Neal to supply observations during this task;
- launch the game for Neal;
- treat manual acceptance as completed.

Simply provide the commands and observation checklist in the final report/handoff, then stop.

Neal will perform manual testing independently and report whatever results he chooses in a later interaction.

---

# Strict milestone boundary

This task authorizes:

- Level 18 production configuration;
- natural Level 17→18 progression;
- direct Level 18 startup;
- terminal Level 18 boundary;
- canonical Shape 2 reuse;
- existing cycle-1 palette reuse;
- shared `$0B` Flipper-route selection;
- shared Tanker cargo route selection;
- shared Pulsar parameterization and Level 18 two-region movement translation;
- focused permanent tests;
- automated regression validation;
- required prompt/report artifacts.

This task does not authorize:

- Level 19 implementation;
- Level 19 progression;
- further Level 15 work;
- further Level 17 fidelity work;
- alteration of Level 17 accepted Pulsar behavior;
- broad actor/lifecycle refactoring;
- a Level 18-specific actor class;
- visual tuning from screenshots;
- image/video analysis;
- interactive game launch;
- staging;
- commit;
- push.

Stop after automated validation and manual-test handoff.

---

# Required implementation authorities

Read completely:

`AI-Logs/Reports/2026-11-25-level18-rom-configuration-readonly-investigation-report.md`

`AI-Logs/Reports/2026-11-26-level18-pulsar-movement-translation-investigation-report.md`

Also read as needed:

`TEMPEST-BEHAVIOR-SPEC.md`

`TEMPEST-DEVELOPMENT-NOTES.md`

`.github/copilot-instructions.md`

`AI-Logs/Reports/2026-11-25-level17-final-closeout-report.md`

`AI-Logs/Reports/2026-11-25-level11-15-17-behavior-special-case-architecture-audit-report.md`

`AI-Logs/Reports/2026-11-25-level15-fuseball-policy-final-closeout-report.md`

Use the two Level 18 investigation reports as the immediate specifications.

Later corrective/closeout reports take precedence over older interim findings.

Do not repeat either Level 18 investigation unless current source materially contradicts its findings.

---

# Accepted architecture requirements

## Eligibility before parameters

An enemy, feature, firing mode, cargo type, Spike, or other gameplay element is absent unless ROM eligibility enables it.

Parameters are applied only after eligibility is established.

A dormant value is not an enable flag.

## Shared core behavior

Use shared actors and shared lifecycle machinery.

Do not create:

- `Level18Flipper`;
- `Level18Tanker`;
- `Level18Pulsar`;
- Level 18 identity types;
- Level 18 ledger;
- Level 18 reconstruction;
- Level 18 completion counters.

## Semantic lifecycle policies

Use semantic configuration properties/helpers rather than independently reconstructing growing lists of level flags at actor-update call sites.

Do not add Level 18 to unrelated flag lists simply to make behavior work.

## Shared rim lifecycle

The `$0B` below-rim route must terminate at rim arrival and hand off to the existing shared Flipper rim lifecycle.

## Reusable tube definitions

Level 18 reuses canonical Shape 2 / ROM row 1 / Level 2 square geometry.

Do not copy coordinates.

## Cycle-based semantic palette

Level 18 uses palette cycle 1, independently of Shape 2.

Do not create Level 18 color overrides.

## Production presentation

Use the accepted production presentation only.

Level 18 must reuse Shape 2 presentation identity, not legacy presentation and not Level 17's Shape 1 transform.

---

# Starting Git/worktree protection

Before changing anything, record:

- branch;
- HEAD;
- upstream;
- ahead/behind;
- staged state;
- tracked modifications;
- untracked files;
- current Level 18 investigation prompt/report artifacts;
- unrelated user/reference material.

Expected pre-existing untracked continuity artifacts may include the two Level 18 investigation prompt/report pairs.

Preserve:

- `Reference/arcade/MBUCOD.V05.txt`;
- ROM/reference material;
- recordings;
- screenshots;
- videos;
- TEMP content;
- AI-Logs;
- unrelated user files.

Do not clean, reset, restore, stash, overwrite, move, or delete unrelated material.

If unexpected tracked production changes exist, STOP and report them before implementation.

---

# Canonical Level 18 identity

Implement one canonical:

`Level18Production`

or the existing project-equivalent naming convention.

Required identity:

- displayed level: 18;
- progression index: 17, zero-based under current convention;
- effective ROM level: 18 / `$12`;
- internal ROM `curlevel`: `$11`;
- cycle-local position: 2;
- zero-based remap index: 1;
- ROM shape row: 1;
- canonical Shape 2;
- board index: 1;
- palette cycle: 1;
- default difficulty: medium/default;
- terminal production level for this milestone.

Natural progression and direct startup must resolve to this same configuration instance/policy.

Do not create separate direct-start and campaign versions.

---

# Level 18 production configuration

Implement the established medium/default policy.

## Wave

- original supply: 21;
- physical enemy envelope: 7 records, indexes 0–6;
- initial active Eggs: 4;
- maximum active Eggs: retain accepted shared value 4;
- maximum approaching Eggs: retain accepted shared value 2;
- reserve/hatch pacing: retain accepted shared project behavior;
- deterministic lane/direction admission: retain shared behavior.

## Enemy ranges

- Flippers: minimum 0, maximum 3;
- Tankers: minimum 1, maximum 1;
- Spikers: minimum 0, maximum 0;
- Pulsars: minimum 2, maximum 5;
- Fuseballs: minimum 0, maximum 0.

## Spikes

- starting Spike height: absent/null;
- starting Spikes: none;
- dynamic Spikes: none;
- no Spiker is eligible to grow one.

## Tanker cargo

- cargo species: Flipper;
- cargo count: exactly 2;
- cargo identities remain distinct from original identities;
- cargo uses the Level 18 `$0B` below-rim route;
- shared Tanker destruction/release/reconstruction remains intact.

## Firing

- Flipper firing: enabled through shared firing path;
- Tanker firing: enabled through shared firing path;
- Spiker firing: dormant because Spikers are ineligible;
- Pulsar firing: disabled for medium/default;
- Fuseball firing: inapplicable because Fuseballs are ineligible;
- shot holdoff: 29;
- maximum active enemy shots: 3;
- enemy-shot host movement: retain the accepted shared 3-unit project translation;
- preserve shared depth/state/random pressure gates.

Do not implement easy/hard difficulty selection in this milestone.

Preserve the recovered easy/hard ROM data only in tests/comments/report where appropriate.

---

# Flipper/Tanker speed

Use the established Level 18 medium/default shared host value:

`2.625f`

for both:

- Flipper climbing;
- Tanker climbing.

Classification:

- ROM raw byte `$AC`;
- cracked ROM word `$FD.60`;
- ROM magnitude 2.625;
- host use is a ratio-preserving project translation consistent with the accepted Level 17 translation.

Do not visually tune the speed.

Do not create separate Level 18 Flipper and Tanker speed systems.

---

# Shared `$0B` Flipper movement

Level 18 ordinary Flippers and Tanker-released cargo Flippers use ROM program `$0B`.

Established behavior:

- eight along/depth advances;
- start one lane flip;
- hold depth during the turn;
- complete the flip;
- repeat;
- no Spike predicate;
- no explicit reverse instruction;
- common rim handoff at rim arrival.

The implementation already contains the matching mechanism in the Level 6 deep Tanker-cargo route.

Refactor/expose the smallest semantic shared strategy needed so it can be selected by:

- Level 6's existing eligible path;
- Level 18 ordinary Flippers;
- Level 18 cargo Flippers;
- reconstructed/re-entered Level 18 cargo where applicable.

Do not duplicate the algorithm.

Do not name the reusable behavior `Level18...`.

Prefer a semantic `$0B`/eight-advance route name consistent with project conventions.

Preserve all accepted Level 6 behavior.

---

# `$0B` route requirements

The selected route must:

- use the existing shared `Flipper` actor;
- preserve identity;
- work for ordinary originals;
- work for released cargo;
- work after reconstruction/re-entry;
- hold the actor's depth during lane-turn animation;
- preserve lane topology;
- preserve directed turning;
- hand off once to the shared rim-entry path;
- clear below-rim route ownership at rim;
- use shared pause/pivot/pursuit;
- not continue `$0B` below-rim ownership at rim;
- not add Level 18 rim code.

Add deterministic tests for exact cadence and handoff.

---

# Shared Pulsar movement parameterization

Implement the investigation's minimal shared movement configuration.

The shared `Pulsar` actor should receive a small semantic movement policy/configuration containing only what this milestone requires:

- optional host threshold;
- fixed movement speed;
- deep-side toward-rim movement speed.

Do not add configurable fields for unrelated accepted constants.

Do not create a new Pulsar subclass.

Do not put displayed-level checks in `Pulsar.Update`.

Do not couple movement policy to rendering.

---

# Level 17 Pulsar compatibility freeze

Level 17 must remain behaviorally identical.

Its movement policy must produce:

- optional threshold: null/inactive;
- fixed speed: `1.0f`;
- fallback/deep movement: behaviorally equivalent to `1.0f`;
- existing climb increment unchanged;
- existing descend decrement unchanged;
- existing 51-update decision behavior unchanged;
- lane pause unchanged;
- rim pause unchanged;
- turn cadence unchanged;
- pulse clock unchanged;
- collision unchanged;
- rim behavior unchanged;
- reconstruction unchanged.

Do not activate Level 17's raw `$28` threshold.

Add deterministic compatibility tests strong enough to detect changed Level 17 movement/state output.

Do not weaken existing Level 17 tests.

---

# Level 18 Pulsar movement translation

Implement the bounded project translation.

## Configuration values

- raw ROM threshold: `$14`;
- mapped host threshold: `392.92035f`, or the exact calculation from the approved endpoint formula;
- fixed host movement speed: `1.0f`;
- deep-side toward-rim host speed: `21f / 11f`;
- pulse beat raw value: `$04`, retained as ROM data;
- medium/default firing: disabled.

Prefer calculating the host threshold through a named pure conversion helper from:

- ROM rim coordinate `$10`;
- ROM deep coordinate `$F2`;
- raw threshold `$14`;
- `Level.Depth`.

Do not scatter the decimal constant through production code.

## Toward-rim movement

Using current C# orientation:

- C# Height 0 is deep end;
- C# Height `Level.Depth` is rim;
- the deep-side ROM region corresponds to `Height <= threshold`;
- at exact threshold equality, use the deep-side speed;
- use the pre-move Height to select speed;
- deep-side movement uses `21f / 11f`;
- rimward region uses `1.0f`;
- clamp to `Level.Depth`;
- crossing the threshold affects the next movement update.

## Away/deep movement

- use fixed speed `1.0f` regardless of region;
- clamp to the deep end;
- evaluate return/reversal conditions after movement;
- use the shared pending-supply abstraction where the ROM immediately returns toward the rim when no generic enemies remain pending;
- do not add a Level 18 check.

## State applicability

Apply the resolver only where current shared behavior actually moves vertically:

- Climbing: thresholded toward-rim movement;
- Descending: fixed away/deep movement;
- Flipping: use toward-rim resolver only if the existing state currently changes Height;
- FlippingWithoutClimb: no vertical movement;
- LanePause: no vertical movement;
- RimPause: no vertical movement;
- Terminal: no vertical movement.

Do not change lateral lane direction from the vertical movement policy.

---

# Pulsar threshold and reversal semantics

The ROM `$14` threshold participates in:

- toward-rim speed selection;
- away/deep return boundary;
- pulse-region logic.

Implement the smallest evidence-backed shared translation.

For the away/deep return:

- evaluate the mapped threshold after movement;
- reverse toward the rim at the translated boundary when generic supply remains;
- when no generic enemies remain pending, use the shared pending-supply abstraction to represent the ROM's immediate-return condition;
- do not introduce a Level 18 policy flag list.

Preserve existing behavior when the optional threshold is absent.

---

# Pulsar collision/pulse boundary

The investigation found that the mapped Level 18 threshold lies close to the rim and overlaps the existing host rim-collision region.

Do not redesign the entire Pulsar collision model.

For Level 18:

- use the configured threshold where the bounded ROM-derived pulse-region policy requires it;
- preserve the existing host rim-collision priority;
- preserve Level 17 collision exactly;
- add focused tests documenting the overlap/priority;
- classify any mismatch as a project translation requiring manual acceptance.

Do not claim literal ROM collision equivalence.

Do not tune the threshold from gameplay or screenshots.

---

# Pulsar lifecycle preservation

Movement configuration is runtime configuration, not identity.

Preserve:

- Pulsar original identity;
- active Pulsar ledger state;
- death/re-entry reconstruction;
- fresh actor timers after re-hatch;
- shared/global pulse clock lifecycle;
- Super Zapper behavior;
- shot destruction;
- player collision;
- completion obligations;
- rendering and color behavior.

A reconstructed Level 18 Pulsar must receive the current Level 18 movement policy through the same configuration path as a newly hatched Pulsar.

Do not store level identity in the actor ledger.

---

# Canonical Shape 2 geometry reuse

Level 18 must reuse:

- `LevelConfiguration.Level2Square`, or the canonical project equivalent;
- `LevelLoader.LoadLevel2Square()`;
- board index 1;
- closed topology;
- 16 lanes;
- existing vertices.

Do not copy or redefine Shape 2 points.

Gameplay geometry must be shared with displayed Level 2.

---

# Shape 2 production presentation reuse

Extend presentation lookup/routing by canonical shape identity rather than a displayed-Level-18 branch.

Level 18 must reuse the accepted Shape 2 / Level 2 production presentation:

- ROM shape row 1;
- existing Level 2 model-frame mapping;
- accepted projection family;
- Tube Y gain: `1`;
- Tube scale: `1.1`;
- vertical offset: `35`;
- Tube depth-Y gain: `1`.

Required chain:

```
Displayed Level 18
    -> canonical Shape 2 / row 1
    -> existing Shape 2 geometry
    -> existing Level 2 model frame
    -> existing Level 2 projection/calibration
```

Do not create Level 18 calibration values.

Do not retune Level 2.

Preserve displayed Level 2 numerical output.

Add structural and representative numerical tests.

---

# Cycle-1 palette reuse

Level 18 selects the established cycle-1 ROM palette:

`00 07 0B 08 44 03 0C 0C`

Preserve current accepted semantic role translations:

- background: black;
- base tube/web: red;
- score/high-score/initials: cyan;
- reserve lives: green;
- displayed-level number: red;
- Blaster/player: yellow;
- Flipper: magenta;
- Tanker: blue;
- Egg: yellow;
- Pulsar normal: cyan;
- Pulsar pulse: white.

Preserve:

- player-lane/transient rail behavior;
- Super Zapper rail cycling;
- restoration through active cycle palette.

Do not add Level 18 color branches.

---

# Wave selection and admission

Implement Level 18 wave selection using ROM-backed ranges and the existing generic identity/selector architecture.

Requirements:

- total original supply 21;
- one Tanker required;
- 2–5 Pulsars;
- 0–3 Flippers;
- no Spikers;
- no Fuseballs;
- no starting or dynamic Spikes;
- seven-record physical envelope;
- Tanker latent cargo reduces applicable capacity;
- minima are satisfied through the existing ROM-style selection policy;
- remaining species selection uses the bounded existing random/availability model;
- cargo does not add to original supply.

Do not use a fixed hand-authored sequence that merely reaches the desired totals.

Do not rely on manual enemy counting.

Use a Level 18 selector/configuration seam consistent with Level 17 but do not copy inappropriate Level 17 data.

---

# Identity and representation invariants

At every boundary:

- each unresolved original identity must have exactly one valid representation;
- each unresolved cargo identity must have exactly one valid representation;
- reserve/Egg/active/terminal/rim/cleared states remain consistent;
- Tanker cargo ownership remains exact;
- no missing runtime representation;
- no duplicate actor allocation;
- no dormant ineligible species;
- completion remains blocked by unresolved obligations.

Add invariant-focused tests for natural play, death, reconstruction, Tanker release, Pulsars, and completion.

---

# Natural progression and direct startup

Implement one shared Level 18 configuration for:

## Natural progression

- Level 17 completes;
- normal ExitZoom/transition occurs;
- approach uses Shape 2;
- palette remains cycle 1;
- displayed level becomes 18;
- score/lives carry;
- Super Zapper recharges normally;
- Level 18 wave begins.

## Direct startup

Support:

`--start-level 18`

and the currently supported direct diagnostic form:

`--level 18`

Both must resolve to the same Level 18 production configuration.

Do not make different direct-start gameplay.

## Terminal boundary

Level 18 is terminal for this milestone.

After completion:

- remain at the existing terminal/complete boundary;
- do not progress to Level 19;
- Level 19 remains unsupported/rejected.

---

# Permanent automated tests

Add focused permanent coverage for all material Level 18 behavior.

## Mapping and reuse

- displayed/internal/cycle-local mapping;
- Shape 2 row/index;
- canonical geometry object/data reuse;
- Shape 2 model-frame/projection/calibration reuse;
- palette cycle 1;
- displayed HUD remains 18.

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

- supply 21;
- seven physical slots;
- species ranges;
- one Tanker;
- 2–5 Pulsars;
- cargo not counted as original supply.

## Tanker cargo

- exactly two Flippers;
- identity/ordinal ownership;
- `$0B` route;
- natural release;
- shot destruction release;
- Super Zapper behavior;
- death/re-entry;
- completion.

## `$0B` Flippers

- eight movement advances;
- one lane flip;
- depth held during turn;
- repeat;
- ordinary originals;
- cargo;
- reconstruction/re-entry;
- shared rim handoff;
- shared rim pursuit;
- Level 6 behavior unchanged.

## Speeds

- raw Level 18 values;
- cracked values;
- host Flipper/Tanker `2.625f`;
- shot holdoff 29;
- shot capacity 3;
- shared enemy-shot host value retained.

## Pulsars

- Level 17 compatibility fingerprint unchanged;
- Level 18 threshold conversion;
- deep-side exact-equality branch;
- rimward branch;
- threshold crossing;
- away/deep fixed speed;
- return boundary;
- pending-supply immediate-return translation;
- lane pause no movement;
- rim pause no movement;
- flip-state movement behavior;
- pulse/collision boundary;
- reconstruction receives same policy;
- identity/completion unchanged;
- medium/default firing disabled.

## Progression/startup

- natural Level 17→18;
- direct `--start-level 18`;
- direct `--level 18`;
- score/lives carry;
- Super Zapper recharge;
- Level 18 terminal;
- Level 19 unsupported;
- restart remains correct.

---

# Level 17 compatibility gate

Level 17 is frozen.

Run focused tests proving no change to:

- Pulsar +1/-1 movement;
- decision cadence;
- lane pause;
- rim pause;
- turn behavior;
- pulse phase;
- collision;
- identity;
- reconstruction;
- completion;
- palette;
- Shape 1 projection;
- gameplay wave policy.

The Level 17 recording must remain valid.

Any Level 17 deterministic divergence requires investigation before completion.

Do not excuse Level 17 divergence as an expected result of shared Pulsar parameterization.

---

# Levels 1–17 regression gate

Run focused regression tests for shared areas affected by Level 18:

- Level 2 Shape 2 presentation;
- Level 6 `$0B` route;
- Level 11/15 semantic lifecycle policy;
- Level 15 seed-1982;
- Level 16 presentation/shared behavior;
- Level 17 gameplay/presentation/Pulsars;
- generic identity/ledger/reconstruction/completion;
- Tanker cargo;
- Flipper rim handoff;
- enemy firing/capacity;
- palette cycle 0 and cycle 1.

Use explicit VSTest hang timeout.

---

# Recording regression

Run unmodified:

`TEMP/GameplayRecordings/lvl1-16.json`

Require complete playback and accepted final telemetry.

Run unmodified:

`TEMP/GameplayRecordings/lvl-17-test.json`

Require complete playback and accepted Level 17 terminal result.

Do not modify, replace, stage, or commit either recording.

If deterministic divergence occurs, isolate the first difference with bounded runs.

Do not continue to broad validation until it is explained.

---

# Full automated validation

After focused gates pass:

1. complete solution test suite;
2. fresh Release build;
3. `git diff --check`.

Use explicit time/hang bounds.

Require:

- no failed tests;
- zero build errors;
- warnings reported accurately;
- no whitespace errors.

Canceled, timed-out, stalled, hung, or blocked commands are not passes.

---

# Manual gameplay/presentation handoff

After automated validation, do not launch the game.

Provide these commands, corrected only if actual project CLI syntax differs:

## Direct Level 18

```
dotnet run --project src\Tempest.Game -- --start-level 18 --lives 10
```

## Optional overlay comparison

```
dotnet run --project src\Tempest.Game -- --start-level 18 --lives 10 --background-opacity .5
```

## Natural transition

Provide the simplest supported command for starting at Level 17 and naturally continuing into Level 18.

Do not require Neal to play Levels 1–16 to reach Level 18.

## Manual observation checklist

Provide a concise checklist covering:

- canonical Shape 2 / Level 2 square presentation;
- cycle-1 palette;
- Level 18 HUD;
- `$0B` ordinary Flipper cadence;
- `$0B` cargo Flippers;
- Flipper/Tanker speed;
- Tanker cargo release;
- Pulsar deep-region faster climb;
- Pulsar near-rim transition;
- Pulsar descending/reversal;
- Pulsar lane/pulse/rim behavior;
- absence of Spikers, Fuseballs, and Spikes;
- shooting pressure;
- death/re-entry;
- completion;
- natural Level 17→18 transition;
- direct Level 18 startup;
- Level 18 terminal boundary;
- no obvious Levels 1–17 regression.

### No-question requirement

Do not phrase the checklist as questions addressed to Neal.

Do not write:

- “Can you verify…?”
- “Does…?”
- “Please answer…”
- “Tell me whether…”
- “Reply with…”

Instead use neutral observation labels, for example:

```
Observe: Pulsars climb faster in the deep region and transition to the fixed rate near the rim.
```

After providing commands and checklist, state:

> Automated implementation is complete. Manual gameplay and presentation acceptance remain pending.

Then STOP.

Do not wait for an answer.

---

# Documentation boundary

This remains a candidate until Neal independently reports manual acceptance.

Do not update durable documentation to claim Level 18 acceptance.

Do not close Level 18.

The implementation report may record:

- ROM-backed facts;
- project translations;
- automated validation;
- manual acceptance pending.

Do not commit or push.

---

# Required AI artifacts

Save this prompt as:

`AI-Logs/Prompts/2026-11-26-level18-bounded-production-implementation-prompt.md`

Create:

`AI-Logs/Reports/2026-11-26-level18-bounded-production-implementation-report.md`

Do not stage, commit, or push.

---

# Required implementation report contents

1. Executive summary
2. Starting Git/worktree state
3. Authorities read
4. Level 18 identity/mapping
5. Canonical Shape 2 reuse
6. Shape 2 presentation reuse
7. cycle-1 palette reuse
8. Level 18 production configuration
9. eligibility-before-parameters result
10. supply and physical envelope
11. species ranges
12. Spike/Spiker absence
13. Tanker cargo
14. `$0B` route extraction/reuse
15. ordinary Flipper `$0B` behavior
16. cargo `$0B` behavior
17. shared rim handoff
18. Flipper/Tanker speed translation
19. firing/shot configuration
20. shared Pulsar configuration design
21. Level 17 Pulsar compatibility
22. Level 18 threshold conversion
23. Level 18 speed-region behavior
24. descending/reversal behavior
25. pulse/collision boundary
26. Pulsar identity/reconstruction
27. progression/direct startup
28. terminal Level 18 boundary
29. files changed
30. tests added/changed
31. Level 2 presentation regression
32. Level 6 `$0B` regression
33. Level 15 semantic-policy regression
34. Level 15 seed-1982 result
35. Level 17 focused regression
36. Level 17 Pulsar fingerprint
37. Levels 1–16 recording result
38. Level 17 recording result
39. focused test result
40. full-suite result
41. Release-build result
42. `git diff --check`
43. unresolved issues
44. evidence classifications
45. manual launch commands
46. neutral manual observation checklist
47. manual acceptance status: pending
48. final Git/worktree state
49. stage/commit/push: none
50. Level 19+ untouched
51. recommended next step after Neal independently reports results

Do not abbreviate the report because implementation passes.

---

# Stop conditions

STOP rather than guessing if:

- current source materially contradicts either Level 18 investigation;
- Level 17 cannot remain deterministic and behaviorally unchanged;
- canonical Shape 2 reuse requires copied geometry;
- `$0B` reuse requires a Level 18 actor or duplicated algorithm;
- Pulsar parameterization requires displayed-level checks in the actor;
- threshold mapping cannot use the approved endpoint formula;
- pulse/collision behavior cannot be kept bounded;
- cargo or identity invariants fail;
- recording regression cannot be explained;
- Level 19 becomes reachable;
- tests/builds stall;
- Windows security blocks validation.

Do not weaken security.

Do not substitute stale binaries.

Do not report partial validation as passed.

---

# Completion criteria

Complete only when:

- Level 18 production configuration is implemented;
- eligibility and parameters match the investigation;
- canonical Shape 2 is reused;
- cycle-1 palette is reused;
- `$0B` route is shared, not duplicated;
- ordinary and cargo Flippers use the correct route;
- Flipper/Tanker speed is configured;
- Pulsars use shared parameterized movement;
- Level 17 Pulsar behavior remains unchanged;
- Level 18 threshold/speed/reversal behavior is tested;
- identities/reconstruction/completion remain generic;
- natural Level 17→18 works;
- direct Level 18 works;
- Level 18 is terminal;
- Level 19 remains unsupported;
- recordings pass;
- focused tests pass;
- full suite passes;
- Release build succeeds with zero errors;
- `git diff --check` passes;
- nothing is staged, committed, or pushed;
- exact manual commands and a neutral checklist are provided;
- no manual-testing questions are asked;
- manual acceptance remains pending.

Then STOP.

TEMPEST-END-LEVEL18-BOUNDED-PRODUCTION-IMPLEMENTATION-COMPLETE