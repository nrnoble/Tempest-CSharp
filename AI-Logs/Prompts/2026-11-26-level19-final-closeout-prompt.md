# Tempest Level 19 Final Closeout, Documentation, Fresh Validation, Commit, and Push

## FAIL-CLOSED COMPLETION-TOKEN CHECK

Read the entire user-provided task before taking any action.

Before tools, files, commands, modifications, tests, staging, commits, pushes, or repository actions:

1. Locate the completion token near the end of this user-provided task.
2. Verify exactly one complete token beginning with `TEMPEST-END-` appears on a line by itself in the user-provided task.
3. The token may contain only uppercase ASCII letters, digits, and hyphens after `TEMPEST-END-`.
4. VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context after the user's completion token do not invalidate it.
5. This check exists only to detect an incomplete or truncated user task.
6. If exactly one valid standalone token cannot be found, STOP before any project or repository action.

The unique completion token is the final standalone line below.

---

# Task

Perform final closeout of the accepted Level 19 production milestone.

This closeout must:

- record Neal's Level 19 manual acceptance;
- record Neal's post-correction Level 3 manual regression acceptance;
- reconcile final Level 19 implementation against the ROM investigation;
- preserve the Shape 3 effective-presentation correction;
- preserve all accepted Levels 1–18 behavior;
- update durable documentation;
- document the stronger repeated-shape effective-output invariant;
- run a completely fresh final validation gate;
- selectively stage intended Level 19 source/tests/docs and required AI-Logs;
- commit;
- push normally;
- independently verify GitHub visibility where available;
- preserve post-push facts using at most one bounded report-finalization commit.

Do not change accepted gameplay or presentation unless the closeout itself exposes a concrete defect.

Do not implement Level 20.

Do not investigate Level 20 beyond already established durable facts.

Do not revisit DebugMCP.

Do not launch the interactive game.

Neal has already completed all required manual acceptance for this milestone.

---

# Required Level 19 authorities

Read completely:

`AI-Logs/Reports/2026-11-26-level19-rom-configuration-readonly-investigation-report.md`

`AI-Logs/Reports/2026-11-26-level19-bounded-production-implementation-report.md`

`AI-Logs/Reports/2026-11-26-level19-shape3-effective-presentation-correction-report.md`

Read current durable authorities:

`TEMPEST-BEHAVIOR-SPEC.md`

`TEMPEST-DEVELOPMENT-NOTES.md`

`.github/copilot-instructions.md`

Read earlier accepted reports only as necessary for regression protection, especially:

`AI-Logs/Reports/2026-11-26-level18-final-closeout-report.md`

the Level 17 final closeout;

the Levels 11/15/17 architecture audit;

the accepted Level 15 semantic-policy closeout.

Use later correction reports over superseded interim findings.

Do not repeat completed Level 19 investigations.

---

# FINAL MANUAL ACCEPTANCE — LEVEL 19

Record:

**MANUAL ACCEPTANCE — LEVEL 19**

Neal independently tested the final Level 19 candidate after the Shape 3 presentation correction and reports:

- Level 19 passes manual testing.
- Level 19 gameplay is accepted.
- Level 19 corrected presentation is accepted.
- No further Level 19 manual testing is required.

Accepted medium/default scope includes:

- canonical Shape 3 presentation;
- vertical placement;
- size/perspective;
- cycle-1 palette;
- HUD;
- Flippers;
- Tankers;
- Tanker cargo;
- Pulsars;
- firing pressure;
- absence of Spikers/Fuseballs/Spikes;
- death/re-entry;
- natural Level 18→19 progression;
- direct Level 19 startup;
- terminal Level 19 behavior.

Easy/hard Level 19 variants remain unimplemented/unaccepted.

Do not request another Level 19 manual run.

---

# MANUAL REGRESSION ACCEPTANCE — LEVEL 3

Record separately:

**MANUAL REGRESSION ACCEPTANCE — LEVEL 3**

After the shared Shape 3 projection-dispatch correction, Neal manually retested Level 3 and reports:

- Level 3 passes manual testing.
- The accepted Level 3 Shape 3 presentation remains correct.
- No further Level 3 testing is required.

This is important because the production correction changed shared Shape 3 projection dispatch from displayed-level identity to canonical shape-row identity.

Do not request another Level 3 run.

---

# Level 19 final identity

Verify final production source:

- displayed level: 19;
- progression index: 18;
- ROM `curlevel`: `$12`;
- effective one-based `lev`: `$13`;
- cycle: 2;
- cycle-local position: 3;
- zero-based remap index: 2;
- ROM shape row: 2;
- board index: 2;
- palette index: 1;
- medium/default policy;
- terminal production level;
- Level 20 unsupported.

Natural campaign and direct startup must resolve to the same Level 19 configuration.

---

# Final Level 19 wave

Verify:

## Supply/envelope

- original supply: 22;
- physical enemy envelope: 7;
- initial active Eggs: 4;
- maximum active Eggs: 4;
- maximum approaching Eggs: 2;
- hatch interval: 50 ticks.

## Species

- Flippers: 0–3;
- Tanker: exactly 1;
- Pulsars: 2–3;
- Spikers: 0;
- Fuseballs: 0.

## Spikes

- no starting Spikes;
- no dynamic Spikes;
- no eligible Spiker.

Eligibility remains authoritative before dormant parameters.

---

# Tanker cargo

Verify:

- exactly one Tanker;
- cargo type: Flipper;
- cargo count: exactly two;
- cargo identities separate from original supply;
- ownership/ordinal preserved;
- natural release shared;
- shot-destruction release shared;
- Super Zapper behavior shared;
- death/re-entry shared;
- completion shared.

No Level 19 Tanker actor or lifecycle.

---

# `$19` / EightAdvanceTurn

Verify Level 19 uses the shared semantic `EightAdvanceTurn` route for:

- ordinary Flippers;
- Tanker cargo;
- applicable reconstruction/re-entry.

Required semantics:

- eight depth advances;
- one held-depth lane turn;
- repeat;
- below-rim route ends at rim;
- shared pause/pivot/rim pursuit.

Preserve Levels 6 and 18 behavior.

Do not add target-level route mechanics.

---

# Flipper/Tanker speed

Verify:

- raw ROM byte `$A9`;
- cracked ROM magnitude 2.71875;
- host Flipper climb `2.71875f`;
- host Tanker climb `2.71875f`.

Host value remains **PROJECT TRANSLATION**.

Do not claim literal Atari-to-host unit equality.

---

# Medium/default firing

Verify:

- Flippers eligible to fire;
- Tanker eligible to fire;
- Pulsars do not fire;
- Spiker/Fuseball firing unreachable;
- shot holdoff 29;
- inclusive final slot 2;
- maximum active enemy shots 3;
- shared enemy-shot host movement 3;
- shared random/depth/state/capacity gates preserved.

Do not add easy/hard firing.

---

# Shared Level 19 Pulsar configuration

Verify shared actor configuration:

- Pulsars 2–3;
- raw threshold `$14`;
- mapped threshold approximately `392.92035f`;
- fixed host speed `1.0f`;
- deep-side toward-rim host speed `87f / 55f`;
- away/deep speed `1.0f`;
- pulse beat `$04`;
- medium Pulsar firing disabled.

Preserve:

- threshold-aware climbing;
- post-move descending reversal;
- pause-state no-movement;
- lane behavior;
- rim behavior;
- pulse/collision boundary;
- identity;
- reconstruction;
- completion.

No `Level19Pulsar`.

---

# Level 17 and Level 18 Pulsar preservation

Verify:

## Level 17

- no-threshold compatibility behavior unchanged;
- fixed 1 host movement;
- accepted collision/pulse/lane/rim behavior.

## Level 18

- existing `$14` threshold;
- fixed 1 host speed;
- deep-side `21f / 11f`;
- accepted pulse/collision behavior;
- natural progression now continues to Level 19.

No shared Pulsar regression is allowed.

---

# Canonical Shape 3 reuse

Preserve:

- canonical first-cycle Level 3 geometry;
- no copied Level 19 geometry;
- ROM row 2;
- board index 2;
- closed 16-lane topology;
- existing model frame;
- existing Level 3 production calibration/projection.

The accepted owner is Level 3.

---

# Shape 3 effective-presentation correction

Preserve the accepted correction:

Before:

```
displayedLevel == 3
```

selected the accepted Shape 3 projection branch.

This caused displayed Level 19 to resolve correct Shape 3 metadata/calibration but then fall into the generic reciprocal projection path.

After:

```
RomTubeMappingCatalog.ResolveShapeRow(displayedLevel) == 2
```

selects the accepted Shape 3 composition.

Verify:

- no Level 19-specific vertical offset;
- no Level 19-specific calibration;
- no copied coordinates;
- no screenshot-derived value;
- Level 3 numerical output unchanged;
- Level 19 effective output matches canonical Shape 3.

---

# Durable repeated-shape effective-output requirement

Add durable project guidance equivalent to:

> Reusing a canonical tube shape means reusing its effective production presentation, not merely resolving the same geometry, metadata, model frame, or calibration objects. When a supported displayed level repeats a canonical shape, equivalent canonical model-space tube and entity/lane inputs must produce equivalent effective production screen-space coordinates, except for explicitly level/cycle-specific roles such as HUD identity and palette.

Also record:

> Shape-specific presentation dispatch must be keyed by canonical shape identity/ROM shape row rather than the displayed level that first introduced the shape.

This requirement exists because Level 19 structurally resolved Shape 3 correctly but still entered a Level-3-only projection branch.

---

# Repeated-shape regression seam

Preserve the new effective-output equivalence coverage:

```
Level 1 ↔ Level 17 — Shape 1
Level 2 ↔ Level 18 — Shape 2
Level 3 ↔ Level 19 — Shape 3
```

For representative near/middle/far tube and entity/lane points, verify:

- canonical geometry;
- model frame;
- calibration;
- projection family;
- logical screen coordinates;
- portrait/host coordinates;
- entity/tube registration.

Do not remove the original structural-resolution tests.

The stronger numerical-equivalence coverage complements them.

---

# Future repeated-shape development requirement

For each future second-cycle level:

```
Level 20 ↔ Level 4
Level 21 ↔ Level 5
...
Level 32 ↔ Level 16
```

the implementation milestone must add the newly supported pair to the effective-output equivalence regression seam.

Do not implement those levels now.

Do not create fake unsupported configurations just to pre-fill the tests.

---

# Cycle-1 palette

Preserve:

```
00 07 0B 08 44 03 0C 0C
```

and accepted semantic roles:

- black background;
- red base tube/web;
- cyan score/high-score/initials;
- green lives;
- red displayed-level HUD;
- yellow Blaster/player;
- magenta Flipper;
- blue Tanker;
- yellow Egg;
- cyan Pulsar;
- white Pulsar pulse.

No Level 19-specific color branch.

---

# Natural progression

Verify:

```
Level 18
    -> ExitZoom
    -> Level 19 Approach
    -> Level 19 Playing
```

Preserve:

- score;
- lives;
- Super Zapper recharge;
- cycle-1 palette;
- canonical Shape 3 transition;
- HUD 19;
- one canonical Level 19 production configuration.

---

# Direct startup and restart

Verify:

```
--start-level 19
```

and:

```
--level 19
```

use the same production configuration.

Preserve expected restart behavior.

---

# Terminal boundary

Level 19 remains terminal.

Level 20 must remain:

- unsupported;
- unimplemented;
- unreachable through natural progression;
- rejected through supported direct-start/recording paths.

Do not add Level 20.

---

# Recording semantics

## `lvl1-16.json`

Remains the accepted foundation recording.

Require complete playback and existing accepted result.

## `lvl-17-test.json`

Currently exhausts its recorded input in Level 18 and does not establish Level 19 gameplay.

Require its accepted current result to remain unchanged.

Do not:

- modify recordings;
- create playback-only compatibility behavior;
- infer Level 19 acceptance from old recordings.

A future Level 19 deterministic recording may be created only if separately useful after acceptance; it is not required by this closeout.

---

# Durable documentation updates

Review/update where appropriate:

`TEMPEST-DEVELOPMENT-NOTES.md`

`TEMPEST-BEHAVIOR-SPEC.md`

`.github/copilot-instructions.md`

Update README only if user-facing invocation/support needs a real update.

Document at minimum:

- Level 19 manually accepted;
- Level 3 post-correction regression manually accepted;
- Level 19 medium/default configuration;
- canonical Shape 3 reuse;
- corrected shape-row projection dispatch;
- stronger effective-output equivalence invariant;
- cycle-1 palette reuse;
- 22-supply wave;
- species ranges;
- no Spikers/Fuseballs/Spikes;
- two-Flipper Tanker cargo;
- shared `EightAdvanceTurn`;
- 2.71875 host Flipper/Tanker translation;
- shared Level 19 Pulsar configuration;
- firing configuration;
- natural Level 18→19 progression;
- direct startup;
- terminal Level 19;
- Level 20 unsupported;
- recording semantics.

Maintain evidence classifications.

Do not claim easy/hard acceptance.

---

# AI-Logs continuity set

Include the complete Level 19 continuity records.

## ROM/configuration investigation

`AI-Logs/Prompts/2026-11-26-level19-rom-configuration-readonly-investigation-prompt.md`

`AI-Logs/Reports/2026-11-26-level19-rom-configuration-readonly-investigation-report.md`

## Production implementation

`AI-Logs/Prompts/2026-11-26-level19-bounded-production-implementation-prompt.md`

`AI-Logs/Reports/2026-11-26-level19-bounded-production-implementation-report.md`

## Shape 3 correction

`AI-Logs/Prompts/2026-11-26-level19-shape3-effective-presentation-correction-prompt.md`

`AI-Logs/Reports/2026-11-26-level19-shape3-effective-presentation-correction-report.md`

## Final closeout

Save this prompt as:

`AI-Logs/Prompts/2026-11-26-level19-final-closeout-prompt.md`

Create:

`AI-Logs/Reports/2026-11-26-level19-final-closeout-report.md`

Also preserve/update `AI-Logs/Copilot.txt` under current continuity policy if required.

Do not rewrite historical reports.

---

# AI-Logs safety scan

Before staging new AI-Logs, scan boundedly for obvious:

- passwords;
- API keys;
- auth tokens;
- private keys;
- credential strings;
- connection-string secrets.

Do not reproduce suspected values.

If credible secret material is found, STOP before staging that file and report filename/category only.

Normal project paths, ROM addresses, localhost metadata, SHAs, test seeds, and command lines are not automatically secrets.

---

# Starting repository audit

Before closeout edits/staging record:

- branch;
- HEAD;
- upstream;
- ahead/behind;
- index;
- tracked modifications;
- untracked paths;
- ignored protected material at metadata level only.

Classify each change as:

1. Level 19 production;
2. Level 19 tests;
3. Shape 3 generic correction;
4. durable documentation;
5. authorized AI-Logs;
6. unrelated tracked;
7. unrelated untracked/reference;
8. recordings/assets.

Do not reset, clean, stash, restore, delete, or absorb unrelated work.

---

# Closeout diff review

Before validation inspect all production/test changes.

Verify no:

- Level 20 code;
- Level 19-specific actor;
- Level 19-specific ledger;
- Level 19-specific completion;
- copied Shape 3 coordinates;
- Level 19 vertical offset;
- Level 19 calibration;
- target-level palette branch;
- displayed-level Shape 3 projection branch;
- regression in Level 3;
- regression in Levels 17/18 Pulsars;
- duplication of `EightAdvanceTurn`.

Resolve only concrete closeout inconsistency.

Do not redesign accepted code.

---

# Permanent test inventory

Ensure coverage exists for:

## Level 19 configuration

- mapping;
- Shape 3;
- palette;
- 22 supply;
- 7 physical envelope;
- 0–3 Flipper;
- exactly 1 Tanker;
- 2–3 Pulsars;
- Spiker/Fuseball/Spike absence;
- cargo;
- speeds;
- firing;
- Pulsars;
- terminal boundary.

## Shared behavior

- EightAdvanceTurn exact cadence;
- ordinary and cargo use;
- shared rim handoff;
- Level 6/18 preservation;
- generic identity/reconstruction/completion.

## Pulsars

- Level 17 compatibility;
- Level 18 threshold policy;
- Level 19 ratio/config;
- pause/reversal/pulse/collision;
- reconstruction.

## Presentation

- Level 3 numerical preservation;
- Level 19 Shape 3 equality;
- Level 1/17 equality;
- Level 2/18 equality;
- Level 3/19 equality;
- representative tube and entity/lane points;
- cycle palette separated from geometry/HUD.

## Progression

- natural Level 18→19;
- direct Level 19;
- terminal Level 19;
- Level 20 unsupported.

Do not add tests solely for count.

---

# Fresh final validation

Run after final documentation/test reconciliation.

Use explicit VSTest hang timeout.

## 1. Level 19 focused gameplay

Run configuration/wave/cargo/route/speed/Pulsar/firing/ledger/reconstruction/completion/progression tests.

## 2. Shape 3 presentation

Run:

- Level 3;
- Level 19;
- repeated-shape effective-output theory;
- adapter;
- calibration;
- metadata;
- renderer/entity registration.

## 3. Shared regression

Run:

- Level 1/17 Shape 1;
- Level 2/18 Shape 2;
- Level 3/19 Shape 3;
- Level 6 EightAdvanceTurn;
- Level 15 semantic policy;
- Level 15 seed-1982;
- Levels 17–18 Pulsars;
- Tanker cargo;
- rim lifecycle;
- identity/reconstruction/completion;
- firing;
- palette cycle 0/1.

## 4. Levels 1–16 recording

Run unmodified:

`TEMP/GameplayRecordings/lvl1-16.json`

Require accepted complete result.

## 5. Level 17-origin recording

Run unmodified:

`TEMP/GameplayRecordings/lvl-17-test.json`

Require accepted current Level 18 result and complete frame consumption.

## 6. Full suite

Run complete solution tests with bounded hang timeout.

## 7. Release build

Run fresh non-incremental Release build.

Require zero errors.

Report warnings exactly.

## 8. Whitespace

Run:

`git diff --check`

before staging and:

`git diff --cached --check`

after staging.

Canceled/timed-out/stalled/blocked commands are never passes.

---

# No additional manual testing

Do not launch the game.

Do not request:

- another Level 19 run;
- another Level 3 run;
- screenshots;
- overlays;
- recordings;
- visual comparisons.

Neal has explicitly stated:

- Level 19 passes manual testing.
- Level 3 passes manual testing.
- No further manual testing is required.

Preserve that as the authoritative manual gate.

---

# Final diff/staging audit

Before staging:

- inspect `git status`;
- inspect full diff;
- inspect untracked inventory;
- inspect AI-Logs inventory.

Selectively stage only intended closeout files.

Verify exclusions:

- `TEMP/GameplayRecordings/lvl1-16.json`;
- `TEMP/GameplayRecordings/lvl-17-test.json`;
- other TEMP;
- screenshots;
- videos;
- MAME captures;
- raw ROMs;
- `Reference/arcade/MBUCOD.V05.txt`;
- unrelated reference files;
- editor/user state;
- generated output.

Do not delete excluded files.

---

# Expected staged set

May include:

- Level 19 production source;
- Level 19 tests;
- generic Shape 3 adapter correction;
- repeated-shape regression tests;
- durable documentation;
- Level 19 ROM investigation prompt/report;
- Level 19 implementation prompt/report;
- Level 19 Shape 3 correction prompt/report;
- this closeout prompt/report;
- `AI-Logs/Copilot.txt` if appropriate.

No Level 20+.

---

# Staging verification

After staging verify:

- staged file list;
- staged stat;
- `git diff --cached`;
- `git diff --cached --check`;
- all intended source/tests/docs present;
- all required Level 19 AI-Logs present;
- protected assets absent;
- unrelated hunks absent;
- Level 20 path absent.

Unstage unexpected content rather than deleting it.

---

# Primary closeout commit

Create one primary commit.

Suggested subject:

`Complete accepted Level 19 production`

or another accurate concise subject.

Record:

- SHA;
- subject;
- committed path categories;
- AI-Logs inventory/count;
- exclusions;
- remaining unstaged/untracked state.

Do not amend history.

---

# Push

Push normally to configured upstream.

Do not force-push.

Record:

- command;
- result;
- local HEAD;
- upstream;
- ahead/behind.

Do not treat push success alone as independent GitHub verification.

---

# Independent GitHub verification

If GitHub connector is available verify:

1. primary Level 19 commit SHA/subject;
2. branch head;
3. Level 19 configuration;
4. Level 19 wave selector;
5. shared Pulsar configuration/use;
6. Shape 3 adapter correction;
7. repeated-shape tests;
8. Level 19 production tests;
9. durable documentation;
10. ROM investigation report;
11. implementation report;
12. Shape 3 correction report;
13. closeout prompt/report;
14. `AI-Logs/Copilot.txt` where applicable.

If connector fails, explicitly report it.

Do not infer success.

---

# Bounded report finalization

Use at most:

## Primary commit

Contains:

- production;
- tests;
- generic presentation correction;
- docs;
- AI-Logs;
- closeout prompt;
- closeout report with all pre-commit facts.

Push and verify.

## Optional one report-only finalization commit

After primary verification:

- update only the closeout report with commit/push/GitHub/post-push facts;
- make exactly one report-only commit if needed;
- push it;
- verify where available.

Do not create a third commit.

---

# Required closeout report contents

1. Executive summary
2. Starting repository state
3. Level 19 manual acceptance
4. Level 3 regression manual acceptance
5. final Level 19 mapping
6. Shape 3 reuse
7. cycle-1 palette
8. wave supply/ranges
9. Spike/Spiker absence
10. Tanker cargo
11. EightAdvanceTurn
12. rim lifecycle
13. speed
14. firing
15. Level 19 Pulsars
16. Level 17/18 Pulsar preservation
17. natural Level 18→19
18. direct startup
19. terminal boundary
20. original Shape 3 defect
21. exact Shape 3 root cause
22. generic shape-row correction
23. Level 3 numerical preservation
24. Level 19 output equivalence
25. repeated Shape 1/2/3 equivalence
26. durable effective-output invariant
27. future repeated-shape requirement
28. files changed
29. tests added/updated
30. durable docs
31. AI-Logs inventory/safety
32. focused Level 19 validation
33. focused presentation validation
34. shared regressions
35. Level 15 seed-1982
36. Levels 1–16 recording
37. Level 17-origin recording
38. full suite
39. Release build
40. whitespace
41. staging audit
42. excluded assets
43. primary commit SHA/subject
44. push result
45. independent GitHub verification
46. representative AI-Logs verification
47. report-finalization commit if used
48. final repository state
49. remaining unrelated material
50. Level 20+ untouched
51. recommended next milestone

Do not shorten the report if a closeout gate blocks.

---

# Security and stall policy

Follow durable Tempest security policies.

Do not weaken Windows security.

Do not bypass Application Control.

Use bounded commands/tests.

Do not wait indefinitely.

Canceled/timed-out/stalled/blocked operations are not passes.

---

# Level 20+ boundary

Do not:

- investigate Level 20 ROM configuration;
- implement Level 20;
- add Level 20 progression;
- add Level 20 presentation;
- add Level 20 wave configuration.

The already established fact that Level 20 is the next Spike boundary may remain documented but is not implementation authority in this task.

---

# Completion criteria

Complete only when:

- Level 19 manual acceptance is documented;
- Level 3 post-correction acceptance is documented;
- Shape 3 correction remains generic;
- strongest repeated-shape effective-output tests pass;
- Level 19 gameplay remains unchanged;
- Levels 1–18 remain protected;
- recordings pass;
- focused tests pass;
- full suite passes;
- Release build succeeds with zero errors;
- whitespace passes;
- durable docs capture effective-output reuse;
- required AI-Logs are included;
- protected local/reference assets remain excluded;
- primary commit is created/pushed;
- independent GitHub verification is attempted;
- finalization uses at most one additional report-only commit;
- no additional manual testing is requested;
- Level 20+ remains untouched.

Then STOP.

TEMPEST-END-LEVEL19-FINAL-CLOSEOUT-COMPLETE