# Tempest Production-Presentation Default Promotion — Final Closeout, Documentation, Validation, Commit, and Push

## FAIL-CLOSED COMPLETION-TOKEN CHECK

Read the entire user-provided task before taking any action.

Before tools, files, commands, modifications, tests, staging, commits, or repository actions:

1. Locate the completion token near the end of this user-provided task.
2. Verify that exactly one complete token beginning with `TEMPEST-END-` appears on a line by itself in the user-provided task.
3. The token may contain only uppercase ASCII letters, digits, and hyphens after the `TEMPEST-END-` prefix.
4. VS Code/agent-generated reminders, tagged-file information, attachment metadata, current-editor context, or other system-generated context appearing after the user's completion token do not invalidate it.
5. This check exists only to detect an incomplete or truncated user task, not appended agent context.
6. If exactly one complete standalone `TEMPEST-END-` token cannot be found, STOP before any project or repository action and explain why.

This task intentionally uses a new hyphenated completion-token protocol because underscores in previous `TEMPEST_...` tokens have sometimes been transformed into asterisks during copy/paste into the VS Code Copilot chat interface.

Do not require or search for an old `TEMPEST_...` token in this task.

The unique completion token for this task is the final standalone line below.

---

# Task

Perform final closeout of the accepted production-presentation default promotion:

- record Neal's manual acceptance;
- preserve the completed implementation;
- update durable documentation;
- update the project's AI-Logs/Git policy to Neal's new explicit policy;
- migrate the durable completion-token protocol from underscore-based `TEMPEST_...` tokens to copy/paste-safe hyphenated `TEMPEST-END-...` tokens;
- use Neal's completed Levels 1–16 gameplay recording for bounded automated regression where useful and supported;
- run a fresh final validation gate;
- reconcile the complete worktree;
- stage the intended production/test/documentation/policy changes;
- stage all legitimate AI-Logs prompts, reports, and `Copilot.txt` under the NEW policy below;
- commit;
- push to the configured upstream;
- independently verify the pushed commit through GitHub if the connector is available;
- verify representative AI-Logs are retrievable from GitHub for future M365 Copilot continuity.

This is a CLOSEOUT task.

Do not introduce new presentation behavior.

Do not retune Levels 1–16.

Do not begin or investigate Level 17 or later levels.

Do not redesign gameplay recording/playback.

Do not perform archival, ZIP consolidation, or historical-log cleanup.

---

# Required reports

Read completely before closeout:

`AI-Logs/Reports/2026-11-24-production-presentation-default-promotion-investigation-report.md`

`AI-Logs/Reports/2026-11-24-production-presentation-default-promotion-implementation-report.md`

`AI-Logs/Reports/2026-11-24-level16-final-closeout-report.md`

Use those reports to reconcile the accepted implementation and repository state.

Do not rewrite historical reports merely because current policy has changed.

Historical reports remain records of what was known and decided at their respective times.

---

# Final manual acceptance from Neal

Record the following as final **MANUAL ACCEPTANCE** for the production-presentation default promotion.

Neal reports:

- All requested manual tests pass.
- Normal interactive gameplay now correctly uses the accepted Atari presentation without requiring `--atari-presentation`.
- Neal manually played the complete current production campaign, **Levels 1–16**, under the promoted default presentation.
- All 16 levels passed manual play testing.
- The presentation/default-selection behavior passed during that complete manual campaign.
- The `--atari-presentation` compatibility behavior passed.
- The representative presentation paths and diagnostics requested by the implementation handoff passed.
- No remaining blocker has been reported for the production-presentation default-promotion milestone.

This is acceptance of the **presentation-selection/default promotion**.

Do not reinterpret it as a new calibration of Levels 1–16. Their accepted per-level calibration remains unchanged.

---

# New full Levels 1–16 gameplay recording

Neal created a gameplay recording covering manual play from Level 1 through Level 16:

`C:\Users\nrnob\source\repos\Tempest\TEMP\GameplayRecordings\lvl1-16.json`

Repository-relative local path:

`TEMP/GameplayRecordings/lvl1-16.json`

This is user-created regression evidence.

VS Code Copilot may use it for bounded automated regression/playback testing where compatible with the existing recording/playback architecture.

## Recording boundaries

- Do not modify this recording.
- Do not rewrite its input frames.
- Do not change its metadata.
- Do not change the recording format because of this file.
- Do not claim arcade input fidelity merely because playback succeeds.
- Do not commit this recording.
- Do not move it from `TEMP/`.
- Do not inspect unrelated recordings.
- Do not perform image/frame analysis.
- Do not generate screenshots from it as part of this closeout.

If the recording is unavailable at the exact path, report that fact and continue with the established automated gate rather than fabricating, synthesizing, or substituting another recording.

## Bounded automated use

If the existing headless/deterministic playback tooling safely accepts this file directly, run it with an explicit time/hang/process bound sufficient to establish:

- the recording parses successfully;
- recorded simulation proceeds deterministically;
- playback does not crash;
- the presentation-default promotion does not alter recorded simulation/input behavior;
- the recording can serve as useful campaign regression evidence.

Report only telemetry/results the existing tooling actually produces.

Do not invent counts or conclusions that the tooling does not establish.

If current tooling cannot safely consume the recording without implementation changes, do not broaden this closeout. Report the limitation and continue with the existing automated gate.

---

# CRITICAL NEW AI-LOGS / GITHUB POLICY

## Explicit supersession of older policy

Neal has changed the Tempest project's Git policy.

**The previous policy that `AI-Logs/` must remain unstaged, uncommitted, or local-only is obsolete.**

Any current project instruction stating that AI-Logs must never be committed must be updated during this closeout.

Do not follow historical instructions that say:

- `AI-Logs/` must remain unstaged;
- prompts/reports are local-only;
- prompt/report history must not be pushed;
- `Copilot.txt` should remain outside Git.

Those instructions reflected the previous policy and are superseded by this task.

Historical reports that mention the old policy remain historical records and must not be rewritten merely to hide that history.

---

# New authoritative AI-Logs policy

For now, GitHub must preserve the Tempest AI development context under:

`AI-Logs/Prompts/`

`AI-Logs/Reports/`

and:

`AI-Logs/Copilot.txt`

The purpose is development continuity.

Neal wants these files available on GitHub so M365 Copilot can recover project history, reports, prior prompts, decisions, and context after browser refreshes or other context loss.

At a future date Neal may separately authorize:

- removal of historical logs;
- cleanup/consolidation;
- ZIP/archive creation;
- movement to another archival mechanism.

**Do none of that now.**

For the current policy, preserve and push the files individually in their existing directory structure.

---

# AI-Logs inventory

Before staging, inventory:

`AI-Logs/Prompts/`

`AI-Logs/Reports/`

`AI-Logs/Copilot.txt`

Determine:

- tracked files;
- untracked files;
- modified tracked files;
- whether any relevant paths are ignored;
- whether `.gitignore` currently blocks them;
- whether `.github/copilot-instructions.md` or another current policy document still says not to commit them.

Record a bounded categorized inventory in the closeout report.

If the number of prompt/report files is large, the report may record:

- total prompt-file count;
- total report-file count;
- `Copilot.txt`;
- any exceptional files requiring separate explanation;

rather than reproducing every filename in prose, provided the actual staged-file audit still verifies all intended files.

Do not fabricate counts. Obtain them from Git.

---

# Include all legitimate prompts and reports

The scope is **not limited to this milestone's prompt/report**.

Stage and commit all legitimate currently present files under:

`AI-Logs/Prompts/`

`AI-Logs/Reports/`

and:

`AI-Logs/Copilot.txt`

This includes historical Tempest development prompt/report files currently present locally.

Do not selectively leave older prompt/report history behind merely because it predates this milestone.

Do not rewrite historical content before committing it.

Do not rename historical files merely for stylistic consistency.

---

# `AI-Logs/Copilot.txt`

Neal specifically identified:

`AI-Logs/Copilot.txt`

as part of the GitHub continuity policy.

Inspect it only sufficiently to confirm:

- it is legitimate Tempest AI/development context;
- it is text;
- it does not contain an obvious credential or secret that must block publication.

Do not rewrite it merely to modernize historical content.

If it is historical/free-form context, preserve it as-is.

---

# AI-Logs safety/secret review

Before staging the newly authorized AI-Logs, perform a bounded safety/content check.

Look for obvious accidental secret material such as:

- passwords;
- API keys;
- authentication tokens;
- private keys;
- credentials;
- connection strings containing secrets.

Also identify accidental unrelated personal/binary material if any exists under the AI-Logs paths.

Do not reproduce suspected secret values in the report.

If a suspected real secret is found:

- STOP before committing that file;
- identify the affected filename;
- identify only the secret category;
- do not print the value;
- do not push it.

Do not classify ordinary project paths, public repository URLs, commit SHAs, ROM addresses, deterministic seeds, CLI arguments, or test values as secrets without evidence.

If an available trusted repository secret-scanning mechanism can safely scan the intended text content, use it where practical.

---

# `.gitignore` reconciliation

Inspect:

`.gitignore`

Determine whether the current file ignores:

- `AI-Logs/`;
- `AI-Logs/Prompts/`;
- `AI-Logs/Reports/`;
- `AI-Logs/Copilot.txt`;
- another pattern that prevents the required files from being tracked.

Make the smallest maintainable change necessary so the intended AI-Logs content is trackable.

Do not broadly remove unrelated ignore protections.

Preserve local exclusions for things such as:

- `TEMP/`;
- videos;
- screenshots/reference captures as currently protected;
- build artifacts;
- local IDE/user files;
- other unrelated generated content.

Do not unignore `TEMP/GameplayRecordings/lvl1-16.json`.

---

# CURRENT PROJECT-INSTRUCTION POLICY UPDATE

Inspect:

`.github/copilot-instructions.md`

and any other currently authoritative project instruction file.

Update the current instructions so future VS Code Copilot work understands:

## AI-Logs policy

- Substantial prompts continue to be saved under `AI-Logs/Prompts/`.
- Corresponding reports continue to be saved under `AI-Logs/Reports/`.
- `AI-Logs/Copilot.txt` remains preserved.
- These AI-Logs are now Git/GitHub history and should be committed/pushed during authorized milestone closeouts.
- Historical reports remain historical records and are not rewritten.
- Local TEMP files, gameplay recordings, screenshots, videos, ROM/reference/user assets remain outside Git unless separately authorized.

## Git authorization policy

Do not infer that every coding task may commit.

Commit and push still require Neal's explicit authorization or a task, such as this closeout, that explicitly authorizes them.

The new AI-Logs policy changes **what belongs in an authorized commit**, not when commits are authorized.

---

# NEW COPY/PASTE-SAFE COMPLETION-TOKEN PROTOCOL

This task also changes the durable Tempest prompt-completion protocol.

## Problem being corrected

Neal has observed that completion tokens containing underscores can sometimes be transformed during copy/paste into the VS Code Copilot chat interface.

For example, an underscore token may arrive with `*` characters replacing `_`.

That triggers the fail-closed truncation protection even when the substantive prompt was copied correctly.

The fail-closed concept remains valuable.

Only the token transport format is changing.

## New authoritative completion-token format

Future substantial Tempest VS Code Copilot prompts must use a standalone completion token beginning with:

`TEMPEST-END-`

The token:

- appears near the end of the user-provided task;
- appears on a line by itself;
- uses uppercase ASCII letters, digits, and hyphens only;
- contains no underscores;
- contains no asterisks;
- needs to be unique within the user-provided task.

Examples:

`TEMPEST-END-LEVEL16-CLOSEOUT-COMPLETE`

`TEMPEST-END-PRESENTATION-DEFAULT-CLOSEOUT-COMPLETE`

`TEMPEST-END-ROM-INVESTIGATION-COMPLETE`

The token does not need to reproduce the entire task title.

Prefer a clear but reasonably short token.

## New durable fail-closed rule

Future substantial prompts should instruct VS Code Copilot:

1. Read the entire user-provided task first.
2. Before tools/files/commands/repository actions, locate the unique standalone completion token.
3. Verify exactly one complete token beginning with `TEMPEST-END-` exists on a line by itself.
4. Accept uppercase ASCII letters, digits, and hyphens after that prefix.
5. Agent-generated reminders, attachments, metadata or other system-generated context after the user's token do not invalidate it.
6. Treat the check as truncation detection, not appended-context rejection.
7. If exactly one valid `TEMPEST-END-` token cannot be found, STOP before project/repository action and explain why.

## Old underscore protocol

The older convention based on:

`TEMPEST_...`

is now obsolete for **new prompts**.

Do not require future prompts to include both formats.

Do not create dual-token prompts.

Do not rewrite historic prompt/report files that already contain the old format.

Those files remain valid historical records.

The new protocol applies prospectively.

---

# Production-presentation implementation to preserve

The implementation already passed automated validation and Neal's manual validation.

Do not redesign it during closeout.

Preserve:

## Default interactive presentation

No-selector interactive gameplay uses the existing accepted Atari presentation.

## Compatibility alias

`--atari-presentation`

remains a redundant valueless compatibility alias.

It is not required for new ordinary commands.

## Baseline portrait diagnostic

`--baseline-portrait-presentation`

selects the old portrait projection for development A/B diagnosis.

It remains distinct from 4:3.

## Legacy 4:3

`--legacy-presentation`

keeps its existing separate 4:3 semantics.

Do not repurpose it.

## Portrait perspective

`--portrait-perspective`

remains associated with explicit baseline portrait diagnostic mode.

Do not apply it to default production Atari presentation.

## Calibration controls

Existing:

- tube Y gain;
- scale;
- vertical offset;
- depth-Y gain;
- HUD calibration;
- calibration guides

operate against effective Atari mode without requiring the redundant compatibility alias.

Preserve their value/selector/input validation.

## Headless/input diagnostic

Headless and input-diagnostic routes remain non-rendering modes.

The interactive default must not force them through Atari rendering.

## Renderer API

Preserve the centralized application selection seam.

Do not change accepted projection/calibration tables.

Do not retune any Level 1–16 presentation.

---

# Background opacity

Preserve:

`--background-opacity`

as the accepted global development-only background-transparency diagnostic.

Its default remains:

`1`

Do not make it level-specific.

Do not change its semantics.

---

# Recording/playback compatibility

Preserve the existing GameplayRecordingFile format and deterministic simulation identity.

Presentation selection remains outside recording identity.

Existing recordings must continue to load.

Visible playback uses accepted production presentation by default.

Headless playback remains presentation-independent.

The new local `lvl1-16.json` file is supplemental regression evidence, not a new committed fixture.

---

# Durable documentation updates

Review/update as appropriate:

`README.md`

`TEMPEST-DEVELOPMENT-NOTES.md`

`TEMPEST-BEHAVIOR-SPEC.md`

`.github/copilot-instructions.md`

`.gitignore`

and any other current policy/documentation file containing obsolete current instructions.

Document the accepted final state:

- Atari presentation is the default interactive production presentation.
- `--atari-presentation` is a compatibility alias.
- `--baseline-portrait-presentation` is the explicit old-portrait A/B diagnostic.
- `--legacy-presentation` remains separate 4:3 presentation.
- all requested manual promotion tests passed;
- Neal manually played Levels 1–16 under the promoted default;
- `TEMP/GameplayRecordings/lvl1-16.json` exists locally as user-created regression evidence;
- AI-Logs prompts/reports/Copilot.txt are now tracked/pushed for continuity;
- old AI-Logs local-only policy is superseded;
- new `TEMPEST-END-...` token protocol supersedes underscore-based tokens prospectively;
- future archival/removal/ZIP consolidation is deferred.

Keep evidence classifications accurate.

Do not rewrite historical reports.

---

# Test review

Review existing implementation tests and add/update only what the final accepted state and policy changes require.

Preserve automated coverage for:

- no-selector default Atari selection;
- default/compatibility-alias equivalence;
- baseline portrait mode;
- separate 4:3 legacy mode;
- portrait-perspective gating;
- tube/HUD/calibration-guide controls;
- recording/visible playback;
- headless/input-diagnostic routing;
- Levels 1–16 presentation;
- gameplay/progression isolation;
- background opacity.

Do not create runtime tests for the text completion token unless a simple existing project-instruction validation mechanism naturally supports it.

The completion-token protocol is primarily a durable Copilot-instruction/prompt-generation policy.

Do not build unnecessary production machinery around it.

---

# Fresh final validation

After all source/test/documentation/policy edits are complete, run a fresh final gate.

All test commands must have explicit hang bounds consistent with the project policy.

## 1. Presentation selection/parser

Run relevant:

- selection;
- default;
- alias;
- baseline;
- legacy;
- calibration;
- perspective;
- file-mode parser tests.

## 2. Recording/playback

Run existing recording/playback tests.

Where safely supported, run:

`TEMP/GameplayRecordings/lvl1-16.json`

through existing bounded automated/headless playback.

Report actual results only.

Do not modify the recording.

## 3. Levels 1–16 presentation

Run relevant accepted presentation regression.

## 4. Gameplay/progression

Run relevant gameplay/progression regression.

## 5. Policy/docs mechanical validation

Perform appropriate bounded checks for:

- `.gitignore` behavior;
- intended AI-Logs no longer ignored;
- `TEMP/` still ignored/protected as intended;
- project instruction text contains the new current AI-Logs and completion-token policies;
- no unintended broad unignore.

Do not add brittle prose snapshot tests unnecessarily.

## 6. Full suite

Run:

`dotnet test TempestCSharp.slnx --blame-hang-timeout 2min --verbosity minimal`

or the current equivalent established solution command if the repository has changed.

## 7. Release build

Run a fresh Release build:

`dotnet build TempestCSharp.slnx --configuration Release --no-restore --verbosity minimal`

Require zero errors.

Report warnings accurately.

## 8. Whitespace

Run:

`git diff --check`

After staging also run:

`git diff --cached --check`.

---

# Worktree ownership audit

Before staging:

- inspect full tracked diff;
- inspect every changed path;
- inspect untracked paths;
- inventory AI-Logs;
- identify source/test/docs/policy changes;
- identify unrelated user/reference/generated files.

The previous Level 16 closeout deliberately left some unrelated tracked/untracked material.

Do not accidentally absorb unrelated source/reference files simply because this commit now intentionally includes AI-Logs.

---

# Expected commit inclusions

The authorized commit may include:

- production-presentation promotion implementation;
- associated tests;
- current README/durable-documentation updates;
- `.github/copilot-instructions.md`;
- `.gitignore` changes necessary for new log policy;
- current policy amendments;
- all legitimate `AI-Logs/Prompts/*`;
- all legitimate `AI-Logs/Reports/*`;
- `AI-Logs/Copilot.txt`;
- the new implementation/closeout prompt and report.

The AI-Logs inclusion is intentional.

---

# Expected exclusions

Unless separately authorized, do not stage or commit:

`TEMP/GameplayRecordings/lvl1-16.json`

or any other `TEMP/` material.

Also exclude:

- screenshots;
- videos;
- MAME assets;
- ROM/reference binaries;
- unrelated Reference files;
- generated build artifacts;
- unrelated personal/user assets;
- IDE/user-specific files.

Do not broaden repository scope beyond the explicit new AI-Logs policy.

---

# AI-Logs staged audit

After staging, verify:

- all legitimate `AI-Logs/Prompts/*` are staged;
- all legitimate `AI-Logs/Reports/*` are staged;
- `AI-Logs/Copilot.txt` is staged;
- no suspected secret-containing file is staged;
- no TEMP gameplay recording is staged;
- historical logs remain unmodified unless they already had legitimate pre-existing edits that are intentionally being preserved and can be safely attributed.

If historical tracked-report edits are ambiguous, determine their ownership before staging rather than silently discarding or committing them.

---

# Commit staging audit

After staging run:

- `git status`;
- staged file list;
- `git diff --cached --stat`;
- `git diff --cached --check`.

Inspect enough of the staged diff to confirm:

- implementation is correctly scoped;
- policy changes are correct;
- AI-Logs inclusion is intentional;
- no Level 17+ work is present;
- no TEMP or unrelated reference assets slipped in.

Unstage unexpected content rather than deleting it.

---

# Commit

After all validation and staged audits pass, create a commit.

Use a concise subject such as:

`Make accepted Atari presentation the default`

Do not amend the Level 16 closeout commit.

The commit may legitimately contain many files because the new policy introduces the accumulated AI-Logs history into Git.

Record:

- commit SHA;
- subject;
- categorized committed inventory;
- prompt/report counts obtained from Git;
- `Copilot.txt` inclusion;
- policy-file inclusion;
- TEMP recording exclusion.

---

# Push

Push to the configured upstream.

Do not force-push.

Do not rewrite remote history.

After push, record:

- push command;
- push result;
- local HEAD;
- upstream;
- ahead/behind state where available.

A successful Git push is evidence of Git transport success.

It is not automatically independent GitHub visibility evidence.

---

# Independent GitHub verification

If the GitHub connector is available:

1. verify the new commit SHA exists remotely with the expected subject;
2. verify representative AI-Logs are remotely retrievable, including:
- this closeout prompt;
- the implementation report;
- at least one earlier historical prompt;
- at least one earlier historical report;
- `AI-Logs/Copilot.txt`;
3. verify repository source remains at the expected pushed commit.

If GitHub retrieval fails, is empty, invalid or unexpectedly unavailable, report that explicitly.

Do not silently claim GitHub continuity verification.

Do not substitute assumptions.

---

# Required closeout AI artifacts

Save this exact substantive task under:

`AI-Logs/Prompts/2026-11-25-production-presentation-default-promotion-closeout-prompt.md`

Create:

`AI-Logs/Reports/2026-11-25-production-presentation-default-promotion-closeout-report.md`

Under Neal's NEW policy, these records must be committed and pushed.

---

# Final closeout-report recursion rule

The closeout report must eventually contain:

- commit SHA;
- push outcome;
- GitHub verification.

Those facts do not exist until after the first closeout commit/push.

Avoid an endless self-referential commit cycle.

Use this bounded process:

## First commit

Before the first commit, prepare the closeout report with:

- manual acceptance;
- implementation state;
- policy changes;
- AI-Logs inventory;
- validation;
- staging audit;
- all facts available before commit/push.

Stage and commit:

- source;
- tests;
- docs;
- policy files;
- AI-Logs inventory;
- `Copilot.txt`;
- this prompt;
- report in its pre-final state.

Push.

## Finalize report

After the push and independent GitHub verification:

- update only the closeout report with final commit SHA, push result, GitHub verification and final repository state;
- if other files do not require changes, create exactly **one** follow-up commit containing the finalized report;
- push that follow-up commit;
- independently verify the follow-up commit if connector access permits.

The finalized report should identify both:

1. the primary implementation/policy commit;
2. the follow-up report-only commit.

Do not then update the report yet again merely to record the follow-up verification SHA in another commit.

The report may state the follow-up commit SHA based on the local commit result before its push; after the follow-up push, return the final push/verification result in the agent's final response without creating a third report-update commit.

Maximum:

- one main closeout commit;
- one report-finalization commit.

No recursive loop.

---

# Final closeout report requirements

The report must include:

1. Starting branch, HEAD, upstream and worktree state
2. Neal's complete manual acceptance
3. Manual Levels 1–16 playthrough acceptance
4. Local `lvl1-16.json` existence and any automated use
5. Production-presentation accepted final behavior
6. `--atari-presentation` compatibility status
7. baseline portrait diagnostic status
8. legacy 4:3 status
9. recording/playback compatibility
10. headless/input-diagnostic status
11. new AI-Logs GitHub policy
12. explicit supersession of old local-only policy
13. AI-Logs inventory/counts
14. `Copilot.txt` status
15. secret/safety audit outcome
16. `.gitignore` changes
17. `.github/copilot-instructions.md` changes
18. new `TEMPEST-END-...` token policy
19. old underscore-token policy supersession
20. focused presentation-selection validation
21. recording/playback validation
22. local full-campaign recording result if run
23. Levels 1–16 presentation regression
24. gameplay/progression regression
25. full-suite result
26. Release-build result
27. whitespace-check results
28. staged-file audit
29. TEMP recording exclusion
30. primary commit SHA/subject
31. primary push result
32. independent GitHub primary-commit verification
33. representative AI-Logs GitHub retrieval verification
34. follow-up report-finalization commit SHA
35. follow-up push/verification result as far as available without recursive report commit
36. final local repository state
37. remaining unrelated local material
38. confirmation Level 17+ was neither implemented nor investigated
39. confirmation no archive/ZIP cleanup occurred

---

# Security and stall policy

Follow the durable Tempest security and bounded-execution policy.

Do not weaken Windows security.

Do not bypass Application Control.

Do not loop repeatedly on blocked artifacts.

Do not substitute stale binaries.

All potentially long commands must have explicit bounds.

Canceled, timed-out, stalled, hung, or blocked operations are never passes.

If final validation, log safety audit, staging, commit or push cannot safely complete, STOP with explicit PARTIAL or BLOCKED status.

---

# Interactive launch boundary

Do not launch the game.

Neal has already completed manual acceptance, including playing current production Levels 1–16 under the promoted default presentation.

Do not ask Neal to repeat the campaign unless closeout unexpectedly changes runtime behavior beyond documentation/policy/log tracking.

---

# Level 17+ boundary

Do not implement, investigate, scope, design or document Level 17 or later gameplay/tube development in this closeout.

Do not begin tube recycling/reuse architecture.

Those matters remain explicitly deferred.

---

# Completion criteria

This closeout is complete only when:

- Neal's manual default-promotion acceptance is documented;
- Levels 1–16 complete manual playthrough is documented as passed;
- local `TEMP/GameplayRecordings/lvl1-16.json` is documented;
- bounded use of that recording is attempted only if supported by existing tooling;
- accepted production presentation remains unchanged;
- normal gameplay defaults to accepted Atari presentation;
- compatibility alias remains functional;
- baseline portrait diagnostic remains functional;
- existing 4:3 legacy presentation remains distinct;
- recording format remains unchanged;
- headless/input-diagnostic routing remains valid;
- background-opacity semantics remain unchanged;
- new AI-Logs GitHub policy is implemented;
- all legitimate current prompts are trackable and staged;
- all legitimate current reports are trackable and staged;
- `AI-Logs/Copilot.txt` is trackable and staged;
- obvious secret/safety review passes;
- `.gitignore` reflects the new policy without broadly unignoring unrelated artifacts;
- `.github/copilot-instructions.md` reflects the new AI-Logs policy;
- `.github/copilot-instructions.md` reflects the new `TEMPEST-END-...` completion-token protocol;
- the old underscore-token convention is prospectively retired;
- focused validation passes;
- recording/playback validation passes;
- relevant Levels 1–16 presentation regressions pass;
- gameplay/progression regressions pass;
- full solution suite passes;
- Release build succeeds with zero errors;
- whitespace checks pass;
- TEMP recording remains uncommitted;
- unrelated reference/user assets remain uncommitted;
- primary closeout commit is created and pushed;
- independent GitHub verification is attempted;
- representative AI-Logs retrieval from GitHub is attempted;
- finalized report is pushed using at most one follow-up report-only commit;
- no recursive commit/report loop occurs;
- Level 17+ remains untouched;
- no archive/ZIP cleanup occurs.

Then STOP.

TEMPEST-END-PRESENTATION-DEFAULT-CLOSEOUT-COMPLETE