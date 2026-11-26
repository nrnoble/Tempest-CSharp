# Tempest Level 17 Final Closeout, Documentation, Fresh Validation, Commit, and Push

## FAIL-CLOSED COMPLETION-TOKEN CHECK

Read the entire user-provided task before taking any action.

Before tools, files, commands, modifications, tests, staging, commits, pushes, or repository actions:

1. Locate the completion token near the end of this user-provided task.
2. Verify that exactly one complete token beginning with `TEMPEST-END-` appears on a line by itself in the user-provided task.
3. The token may contain only uppercase ASCII letters, digits, and hyphens after the `TEMPEST-END-` prefix.
4. VS Code/agent-generated reminders, tagged-file information, attachment metadata, current-editor context, or other system-generated context appearing after the user's completion token do not invalidate it.
5. This check exists only to detect an incomplete or truncated user task, not appended agent context.
6. If exactly one complete standalone `TEMPEST-END-` token cannot be found, STOP before any project or repository action and explain why.

Do not require the obsolete underscore-based `TEMPEST_...` token convention.

The unique completion token is the final standalone line below.

---

# Task

Perform the **final Level 17 closeout**:

- reconcile the complete accumulated Level 17 candidate;
- record Neal's final Level 17 manual acceptance;
- preserve the accepted Levels 1–16 foundation;
- update durable documentation;
- encode the architectural requirements established by Level 17 development;
- run a completely fresh final automated validation gate;
- audit the complete worktree and staged diff;
- stage only intended project changes plus all legitimate AI-Logs required by the current continuity policy;
- commit;
- push to the configured upstream;
- independently verify GitHub visibility where the connector is available;
- finalize and preserve the closeout report using at most one bounded follow-up report-only commit.

This is a **CLOSEOUT task**.

Do not implement new Level 17 behavior.

Do not retune accepted gameplay.

Do not retune accepted presentation.

Do not implement or investigate Level 18+.

Do not perform the deferred Flipper architecture audit in this task.

Do not perform debugger-extension/tooling evaluation in this task.

Do not launch the interactive game.

Neal has already completed the required manual acceptance.

---

# Required reports

Read the relevant accumulated Level 17 reports before closeout, including:

`AI-Logs/Reports/2026-11-25-level17-rom-reuse-color-pulsar-investigation-report.md`

`AI-Logs/Reports/2026-11-25-level17-missing-rom-palette-speed-followup-investigation-report.md`

`AI-Logs/Reports/2026-11-25-level17-rom-map-final-evidence-report.md`

`AI-Logs/Reports/2026-11-25-level17-rom-to-csharp-integration-readiness-investigation-report.md`

`AI-Logs/Reports/2026-11-25-level17-bounded-production-implementation-report.md`

`AI-Logs/Reports/2026-11-25-level17-levels1-16-shared-regression-investigation-report.md`

`AI-Logs/Reports/2026-11-25-level17-shared-regression-bounded-correction-report.md`

`AI-Logs/Reports/2026-11-25-level17-frozen-rim-flipper-correction-report.md`

`AI-Logs/Reports/2026-11-25-level17-production-presentation-second-cycle-palette-investigation-report.md`

`AI-Logs/Reports/2026-11-25-level17-shape1-projection-second-cycle-palette-implementation-report.md`

`AI-Logs/Reports/2026-11-25-level17-rail-tanker-egg-palette-refinement-report.md`

`AI-Logs/Reports/2026-11-25-level17-spike-eligibility-root-cause-correction-report.md`

Also read:

`AI-Logs/Reports/2026-11-25-production-presentation-default-promotion-closeout-report.md`

Use later corrective reports as authority when historical reports contain superseded interim findings.

Do not rewrite historical AI reports.

They remain provenance records of the development process.

---

# FINAL MANUAL ACCEPTANCE FROM NEAL

Record the following as **MANUAL ACCEPTANCE — LEVEL 17**.

Neal reports after the completed corrections:

- Level 17 passes manual gameplay testing.
- At this time Neal can find **no defects or noticeable differences from the arcade version**.
- Flippers climb correctly below the rim.
- Flippers move/flip correctly after reaching the rim.
- Tankers behave correctly.
- Pulsars behave correctly.
- Level 17 contains no unauthorized starting Spikes.
- Level 17 contains no unauthorized Spikers.
- Shape 1 production projection is correct.
- The second-cycle palette looks correct after refinement.
- Ordinary Level 17 tube/web rails are correct.
- Tankers are blue as observed in MAME.
- Eggs are yellow as observed in MAME.
- HUD colors are accepted.
- Flipper/Pulsar colors are accepted.
- Levels 1–16 recorded playback remains correct after the Level 17 corrections.

Treat Level 17 gameplay and presentation as manually accepted.

Do not reinterpret this as permission to redesign any accepted component during closeout.

---

# Accepted Level 17 gameplay configuration

Reconcile the final code against the latest evidence.

The final medium/default Level 17 configuration should reflect the latest corrective authority.

At minimum verify:

- displayed level: 17;
- canonical tube shape: row 0 / Shape 1;
- cycle palette: 1;
- ordinary supply: 20;
- Flippers: minimum 0, maximum 3;
- Tankers: minimum 1, maximum 1;
- Spikers: minimum 0, maximum 0;
- Pulsars: minimum 2, maximum 5;
- Fuseballs: minimum 0, maximum 0;
- Tanker cargo: exactly two Flippers;
- ordinary/cargo below-rim Flipper program: ROM `$07` straight behavior;
- Flipper/Tanker C# translated climb speed: `2.53125f`;
- Spiker core behavior remains shared but Spikers are ineligible on Level 17;
- enemy-shot holdoff: 32;
- medium/default shot capacity: 3;
- Pulsar firing: disabled at medium/default;
- starting Spikes: absent;
- Level 17 terminal after completion;
- Level 18 unsupported.

If any current code differs from the latest corrective reports, STOP and reconcile evidence before closeout.

Do not silently choose an older interim value.

---

# Accepted Spike eligibility correction

Preserve the ROM-backed cycle pattern:

`00 00 00 E0 D8 D4 D0 C8 C0 B8 B0 A8 A0 A0 A0 A8`

Requirements:

- Level 17 → cycle position 1 → no starting Spikes;
- Level 18 → position 2 → no starting Spikes;
- Level 19 → position 3 → no starting Spikes;
- Level 20 → position 4 → `$E0` starting-Spike parameter eligible;
- zero payload means absent starting-Spike eligibility;
- nonzero payload is translated only after eligibility is established;
- Level 17 min/max Spikers remain 0/0.

Do not implement Level 18–20 gameplay.

Tests/configuration helpers may protect the ROM cycle pattern without exposing those levels as playable.

---

# Accepted Shape 1 reuse and production projection

Preserve:

- Level 17 resolves to canonical ROM shape row 0;
- Level 17 reuses canonical Shape 1 geometry;
- no copied Level 17 coordinate array;
- Level 1 and Level 17 use the same accepted Shape 1 production projection family;
- shape identity selects the Shape 1 projection behavior;
- displayed-level identity remains separate.

Accepted Shape 1 host calibration remains:

- Y gain `1.4`;
- tube scale `0.8`;
- vertical offset `35`;
- depth-Y gain `1`.

Do not create Level 17-specific duplicated calibration.

---

# Accepted second-cycle palette

Preserve the verified ROM blocks.

## Cycle 0 — Levels 1–16

`00 04 08 0C C3 07 0B 0B`

## Cycle 1 — Levels 17–32

`00 07 0B 08 44 03 0C 0C`

Preserve the accepted current Level 17 host-role translation:

- black background;
- base tube/web: red;
- score/high-score/initials: cyan;
- reserve lives: green;
- level number: red;
- Blaster/player: yellow;
- Flippers: magenta;
- Tankers: blue;
- Eggs: yellow;
- Pulsar normal: cyan;
- Pulsar pulse: white.

Preserve existing accepted/project translations for unresolved minor roles unless later evidence separately authorizes a change.

The compact blue diamond-like vector previously discussed remains an unresolved likely player-shot visual role unless source evidence in the current code has already established it.

Do not assign it an enemy identity during closeout.

---

# Accepted rail/transient behavior

Preserve:

- cycle-1 ordinary/base tube rails restore to red;
- cycle-0 ordinary/base rails retain their accepted prior color;
- player-lane yellow highlight remains bounded;
- Super Zapper temporary rail cycling remains functional;
- after transient completion, rail color restores through the active cycle palette;
- Pulsar pulse/transient behavior remains independent.

Do not flatten every rail into one color.

---

# Level 17 recordings and regression material

Local recordings:

`TEMP/GameplayRecordings/lvl1-16.json`

`TEMP/GameplayRecordings/lvl-17-test.json`

These are local user-created regression evidence.

Do not modify them.

Do not commit them.

Do not move them.

## Levels 1–16 recording

This remains the accepted-foundation oracle.

The established accepted boundary includes:

- 35,350 ticks;
- score 134039;
- lives 9;
- seed 1990198549;
- no early GameOver/divergence;
- accepted Level 16 completion boundary before candidate continuation into Level 17.

## Level 17 recording

The recording was created during earlier Level 17 development and has survived the subsequent bounded corrections.

The latest Spike-correction report records:

- 2609/2609 ticks consumed;
- `RecordingCompleted`;
- Level 17 Complete;
- score 5975;
- lives 8;
- zero Spikes.

Use the latest established recording result as the expected candidate oracle where deterministic state remains unchanged by closeout.

Do not update the recording during this task.

---

# ARCHITECTURAL REQUIREMENT 1: eligibility before parameters

Add durable project guidance equivalent to:

> Enemy types, Spikes, projectiles, cargo types, firing modes, and other gameplay features are absent unless Atari ROM eligibility logic enables them for the current level. Parameter data is applied only after eligibility is established. A nonzero or otherwise valid parameter value is never, by itself, proof that the corresponding feature is active.

This requirement exists because Level 17 incorrectly received Spikes when a later-cycle `$A0` height parameter was treated as feature eligibility.

Apply this principle prospectively to future level extraction and implementation.

---

# ARCHITECTURAL REQUIREMENT 2: shared core behavior

Add durable project guidance equivalent to:

> Enemy and gameplay-object core behavior remains shared across levels unless Atari ROM evidence establishes a genuinely distinct behavior mode. Level configurations select eligibility, supply, movement program, speed/timing, firing, cargo relationships, and other ROM parameters rather than creating independent per-level actor/lifecycle implementations.

This does not prohibit named implementations of genuinely distinct ROM programs.

It prohibits per-level duplicated actor engines used only to bypass a broken generic lifecycle.

---

# ARCHITECTURAL REQUIREMENT 3: below-rim program versus shared rim lifecycle

Add durable guidance equivalent to:

> Level-specific Flipper below-rim movement programs terminate at rim arrival unless ROM evidence explicitly says otherwise. Rim arrival hands the Flipper to the established shared rim-pursuit lifecycle. Below-rim route selection must not continue owning the actor at rim depth merely because a level uses a distinct movement program.

Document the historical Level 17 lesson without rewriting historical reports.

---

# ARCHITECTURAL REQUIREMENT 4: cycle-based semantic palettes

Add durable guidance equivalent to:

> Persistent gameplay and HUD colors resolve through the active 16-level palette cycle and semantic rendering role, independently of tube shape and gameplay configuration. Subsequent 16-level cycles should supply role palettes rather than per-level color branches. Transient effects may temporarily override palette roles and must restore to the active cycle palette afterward.

Also record:

- tube shape repeats independently from palette cycle;
- Shape 1 projection/calibration remains shape-specific;
- color is cycle/role-specific.

Do not extrapolate unverified host RGB values for future cycles.

---

# ARCHITECTURAL REQUIREMENT 5: reusable tube definitions

Preserve/document:

> The game's unique tube geometry cycle is the canonical 16 shapes. Later displayed levels reuse those shape definitions rather than maintain copied geometry. Shape-specific geometry/projection/calibration corrections should propagate to every displayed level using that shape, while gameplay configuration and cycle palette remain independently resolved.

Do not implement future levels during closeout.

---

# Deferred architecture audit

Record as a future milestone, not current work:

**Read-only Flipper/Tanker movement/lifecycle architecture audit before Level 18 implementation.**

Purpose:

- determine whether Level 11/15/17 historical special routes contain duplicated lifecycle behavior;
- verify below-rim movement strategies converge onto one generic rim lifecycle;
- identify any hidden level-specific actor engines or hacks;
- do nothing if the current architecture is already clean.

Do not perform that audit in this closeout.

---

# Deferred debugger/tooling evaluation

Record separately as a future tooling milestone:

**Read-only VSC/.NET debugger capability assessment at a natural development pause.**

Purpose:

- identify whether debugger-aware VSC tooling would materially improve diagnosis;
- inventory installed C#/.NET/debugger tools;
- evaluate breakpoint/stepping/locals/call-stack/expression needs;
- assess suitable VS Code/MCP debugger bridges before installing anything.

Do not install or evaluate extensions during this closeout.

---

# Repository safety and complete ownership audit

Before editing/staging:

Record:

- branch;
- HEAD;
- upstream;
- ahead/behind;
- staged state;
- tracked modifications;
- untracked files;
- ignored local material;
- complete Level 17 accumulated diff;
- AI-Logs inventory;
- pre-existing unrelated modifications.

Level 17 accumulated through many bounded tasks.

Therefore inspect the entire candidate carefully.

Classify:

1. intended Level 17 production changes;
2. intended Level 17 tests;
3. durable documentation/policy changes;
4. AI-Logs continuity records;
5. pre-existing unrelated tracked changes;
6. untracked local reference/user assets;
7. TEMP recordings;
8. ROM/reference material.

Do not clean, reset, restore, overwrite, or silently absorb unrelated work.

If ownership of a changed production/documentation file cannot be established safely, STOP before staging.

---

# Historical AI-Logs policy

Under Neal's current policy, GitHub should preserve:

`AI-Logs/Prompts/`

`AI-Logs/Reports/`

`AI-Logs/Copilot.txt`

For this explicitly authorized closeout:

- inventory all legitimate currently present prompts;
- inventory all legitimate currently present reports;
- preserve `Copilot.txt`;
- perform a bounded secret/safety scan;
- stage and commit all legitimate current AI-Logs;
- do not rewrite historical reports;
- do not archive or ZIP them.

Do not interpret this as authorization to commit unrelated user/reference/TEMP material.

---

# AI-Logs safety review

Before staging current AI-Logs, perform a bounded scan for obvious accidental:

- passwords;
- API keys;
- authentication tokens;
- private keys;
- credentials;
- connection-string secrets;
- unrelated personal binary material.

Do not reproduce any suspected secret value.

If a credible secret is detected:

- STOP before committing the affected file;
- identify filename and category only;
- do not push it.

Do not treat normal project paths, commit SHAs, ROM addresses, test seeds, CLI switches, or public repository references as secrets without evidence.

---

# Durable documentation updates

Review and update where appropriate:

`TEMPEST-DEVELOPMENT-NOTES.md`

`TEMPEST-BEHAVIOR-SPEC.md`

`README.md`

`.github/copilot-instructions.md`

Do not modify a document merely for ceremony.

Record final Level 17 accepted behavior and evidence classifications.

At minimum durable documentation should reflect:

- Level 17 manually accepted;
- Neal currently observes no defects or noticeable differences from the arcade version;
- canonical Shape 1 reuse;
- cycle-1 palette;
- Level 17 enemy eligibility/composition;
- no starting Spikes;
- no Spikers;
- Pulsars active;
- Tanker cargo;
- Flipper `$07`;
- shared rim handoff;
- accepted palette roles;
- eligibility-before-parameters;
- shared core behavior;
- shared rim lifecycle;
- cycle palette requirement;
- reusable tube geometry requirement;
- recordings available locally for regression;
- Level 18 remains unimplemented.

Keep ROM facts, project translations, arcade observations, and manual acceptance clearly distinguished.

---

# Closeout test review

Before running final validation, inspect the accumulated Level 17 tests.

Ensure permanent coverage exists for the defects discovered during Level 17 development:

1. canonical Shape 1 reuse;
2. accepted Shape 1 projection sharing;
3. Level 17 cycle palette;
4. `$07` straight below-rim behavior;
5. shared rim handoff;
6. ordinary/cargo rim movement;
7. Pulsar admission;
8. Pulsar ledger/reconstruction/completion;
9. Tanker two-Flipper cargo;
10. Tanker color;
11. Egg color;
12. base rail/transient restoration;
13. no Level 17 starting Spikes;
14. no Level 17 Spikers;
15. ROM Spike eligibility cycle;
16. enemy-shot holdoff/capacity;
17. natural Level 16→17 progression;
18. direct Level 17 startup;
19. terminal Level 17 boundary;
20. Levels 1–16 regression protection.

Add/update only narrowly necessary tests.

Do not increase test count for its own sake.

---

# Fresh final validation gate

Run fresh final validation **after** all closeout source/test/documentation changes.

Use explicit VSTest/process/hang bounds.

## 1. Level 17 focused gameplay

Run:

- eligibility/wave configuration;
- Flipper `$07`;
- rim handoff;
- Tanker cargo;
- Pulsars;
- firing;
- Spike absence/Spiker absence;
- ledger;
- reconstruction;
- completion;
- direct startup;
- natural progression.

## 2. Level 17 focused presentation

Run:

- Shape 1 projection;
- calibration reuse;
- cycle palette;
- HUD colors;
- tube/web base rails;
- transient restoration;
- Tanker blue;
- Egg yellow;
- Flipper magenta;
- Pulsar cyan/white;
- registration/finite projection.

## 3. Levels 1–16 regression

Run relevant accepted prior-level suites affected by shared changes.

Particularly include:

- Levels 11 and 15 Flipper/Tanker/cargo behavior;
- Level 15 seed-1982 soak;
- Level 16 presentation/shared-center behavior;
- shared ledger/reconstruction;
- presentation/palette cycle-0 behavior.

## 4. Levels 1–16 recording

Run:

`TEMP/GameplayRecordings/lvl1-16.json`

with the established strict accepted boundary.

Require the accepted result.

## 5. Level 17 recording

Run:

`TEMP/GameplayRecordings/lvl-17-test.json`

using the latest accepted deterministic result.

Record exact resulting telemetry.

Do not modify the recording.

## 6. Full solution

Run the complete automated suite with explicit VSTest hang timeout.

## 7. Release build

Run fresh Release build.

Require zero errors.

Report warnings exactly.

## 8. Whitespace

Run:

`git diff --check`

before staging.

After staging run:

`git diff --cached --check`.

Canceled, timed-out, stalled, hung, or blocked commands are not passes.

---

# Final diff audit before staging

Inspect:

- `git status`;
- complete changed-file list;
- complete tracked diff;
- untracked files;
- AI-Logs inventory.

Verify no unintended changes to:

- Level 18+;
- ROM/reference binaries;
- screenshots;
- videos;
- TEMP recordings;
- generated outputs;
- unrelated user files.

Pay special attention to the pre-existing unrelated `TEMPEST-BEHAVIOR-SPEC.md` modification that has been preserved through Level 17 development.

If it remains unrelated and cannot safely be attributed to Level 17, do not stage that unrelated hunk.

Use selective staging where necessary.

Do not discard it.

---

# Expected commit inclusion

The authorized closeout may include:

- final Level 17 production source;
- final Level 17 tests;
- shared generic corrections required by Level 17;
- cycle palette/render changes;
- reusable Shape 1 presentation changes;
- Spike eligibility resolver;
- durable documentation updates;
- `.github/copilot-instructions.md` updates for architectural policy where appropriate;
- all legitimate current `AI-Logs/Prompts/*`;
- all legitimate current `AI-Logs/Reports/*`;
- `AI-Logs/Copilot.txt`.

---

# Expected exclusions

Do not stage/commit unless separately authorized:

- `TEMP/GameplayRecordings/lvl1-16.json`;
- `TEMP/GameplayRecordings/lvl-17-test.json`;
- any other TEMP material;
- screenshots;
- videos;
- MAME captures;
- ROM binaries;
- unrelated reference files;
- generated build output;
- editor/user state;
- unrelated local assets.

---

# Staging audit

Stage only intended closeout content plus the authorized AI-Logs inventory.

After staging verify:

- staged file list;
- `git diff --cached --stat`;
- `git diff --cached --check`;
- all legitimate AI-Logs expected by policy are staged;
- `Copilot.txt` remains tracked/preserved;
- TEMP recordings are not staged;
- screenshots are not staged;
- ROM/reference binaries are not staged;
- unrelated tracked hunks remain unstaged;
- Level 18+ is absent.

If anything unexpected is staged, unstage it rather than deleting it.

---

# Commit

After all final validation and staging audits pass, create the primary Level 17 closeout commit.

Preferred concise subject:

`Complete accepted Level 17 production`

or another equally accurate concise subject.

Do not amend prior commits.

Record:

- SHA;
- subject;
- categorized committed paths;
- prompt/report counts from Git;
- `Copilot.txt` inclusion;
- excluded TEMP recordings;
- remaining unstaged/untracked material.

---

# Push

Push normally to the configured upstream.

Do not force-push.

Do not rewrite history.

Record:

- push command;
- push result;
- local HEAD;
- upstream;
- ahead/behind after push.

Do not claim independent GitHub visibility merely because `git push` succeeds.

---

# Independent GitHub verification

If the GitHub connector is available:

Verify:

1. primary Level 17 commit SHA and subject;
2. repository branch head;
3. representative final Level 17 source;
4. current Level 17 closeout prompt;
5. current Level 17 closeout report;
6. at least one earlier Level 17 investigation prompt/report;
7. `AI-Logs/Copilot.txt`.

If GitHub retrieval fails, is empty, invalid, or unexpectedly unavailable, explicitly report it.

Do not substitute assumptions.

---

# Closeout AI artifacts

Save this prompt as:

`AI-Logs/Prompts/2026-11-25-level17-final-closeout-prompt.md`

Create:

`AI-Logs/Reports/2026-11-25-level17-final-closeout-report.md`

Under Neal's current policy, these must be committed and pushed.

---

# Bounded report-finalization rule

The final report needs post-commit/push facts that do not exist before committing.

Avoid a recursive commit loop.

Use at most:

## Primary commit

Include:

- source;
- tests;
- docs;
- policies;
- all legitimate AI-Logs;
- this closeout prompt;
- closeout report containing all pre-commit facts.

Push and independently verify.

## One report-finalization commit

After primary push:

- update only the closeout report with:
- primary commit SHA;
- push result;
- GitHub verification;
- representative AI-Logs retrieval result;
- post-push repository state;
- create exactly one report-only follow-up commit if necessary;
- push it;
- verify it if available.

Do not create a third commit merely to record the verification of the report-finalization commit.

Return that last verification in the agent's final response.

Maximum:

- one primary closeout commit;
- one report-finalization commit.

---

# Required final report contents

The closeout report must include:

1. Executive summary
2. Starting Git/worktree state
3. Level 17 final manual acceptance
4. Neal's no-noticeable-difference arcade judgment
5. Final gameplay configuration
6. Enemy eligibility/composition
7. Spike/Spiker eligibility correction
8. Flipper `$07` behavior
9. shared rim handoff
10. Tanker cargo
11. Pulsar production integration
12. firing/shot configuration
13. canonical Shape 1 reuse
14. Shape 1 projection/calibration
15. cycle palette architecture
16. final accepted cycle-1 host colors
17. HUD colors
18. base/transient rail behavior
19. Tanker/Egg colors
20. architectural eligibility requirement
21. architectural shared-core requirement
22. architectural shared-rim requirement
23. architectural cycle-palette requirement
24. architectural reusable-shape requirement
25. deferred Flipper architecture audit
26. deferred debugger/tooling evaluation
27. AI-Logs inventory/safety result
28. files changed during closeout
29. tests added/updated during closeout
30. Level 17 focused gameplay validation
31. Level 17 focused presentation validation
32. Levels 1–16 regression results
33. Level 15 seed-1982 result
34. Levels 1–16 recording result
35. Level 17 recording result
36. full-suite result
37. Release-build result
38. whitespace results
39. staged-file audit
40. excluded local/reference assets
41. primary commit SHA/subject
42. primary push result
43. independent GitHub verification
44. representative AI-Logs retrieval verification
45. report-finalization commit SHA/result if used
46. final local repository state
47. remaining unrelated worktree material
48. confirmation Level 18+ remains unimplemented
49. confirmation no archive/ZIP cleanup occurred
50. recommended next milestone

Do not abbreviate the report merely because closeout encounters a blocker. If stopped, document all completed evidence in detail.

---

# Security and stall policy

Follow durable Tempest security and bounded-execution policy.

Do not weaken Windows security.

Do not bypass Application Control.

Do not loop repeatedly on blocked artifacts.

Do not substitute stale binaries.

Use explicit bounds for long-running operations.

Blocked, canceled, timed-out, hung, or stalled commands are never passes.

If required validation or repository operations cannot complete, STOP with explicit PARTIAL/BLOCKED status.

---

# Manual launch boundary

Do not launch the interactive game.

Neal has already manually accepted Level 17.

Do not ask Neal to repeat gameplay or visual testing unless closeout unexpectedly changes runtime behavior beyond documentation/policy maintenance.

---

# Level 18+ boundary

Do not implement, investigate, configure, or promote Level 18 or later levels.

The Spike eligibility helper may encode the verified repeating ROM cycle because that is part of the already established Level 17 correction, but do not expose later levels as playable.

Do not perform multi-level batch implementation work.

---

# Completion criteria

Level 17 final closeout is complete only when:

- Neal's final manual acceptance is documented;
- no currently known Level 17 defect remains;
- final gameplay configuration matches latest evidence;
- Level 17 has no unauthorized Spikes or Spikers;
- eligibility-before-parameters is durable policy;
- shared enemy/object core behavior is durable policy;
- shared Flipper rim lifecycle is durable policy;
- cycle-based semantic palettes are durable policy;
- reusable 16-shape geometry/presentation is durable policy;
- accepted Level 17 projection/palette remains unchanged;
- focused gameplay validation passes;
- focused presentation validation passes;
- Levels 1–16 regressions pass;
- Level 15 seed-1982 passes;
- both local recording gates pass;
- full suite passes;
- Release build succeeds with zero errors;
- whitespace checks pass;
- final diff is correctly scoped;
- all legitimate current AI-Logs are included;
- TEMP recordings remain excluded;
- screenshots/reference assets remain excluded;
- primary commit is created and pushed;
- independent GitHub verification is attempted;
- representative AI-Logs are verified remotely when possible;
- finalized closeout report is preserved using at most one bounded follow-up commit;
- no recursive commit loop occurs;
- Level 18+ remains untouched.

Then STOP.

TEMPEST-END-LEVEL17-FINAL-CLOSEOUT-COMPLETE