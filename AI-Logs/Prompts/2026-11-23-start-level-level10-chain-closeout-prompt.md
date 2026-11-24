Read this entire user-provided task before taking any action.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, files, commands, Git operations, or taking any repository action, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with `TEMPEST_` appears on a line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment metadata, current-editor context, or other system-generated information appearing after the user's token do not invalidate it.

If exactly one complete token cannot be found, stop before any project or repository action and explain why.

# Task: Close out, document, commit, and push accepted `--start-level` and Level 10 production-chain integration

The `--start-level` continuing-campaign feature and the associated Level 10 production-chain integration have completed automated validation and Neal's required manual acceptance.

This is a CLOSEOUT task.

Do not add features.

Do not redesign progression.

Do not change Level 10 gameplay.

Do not begin Level 13.

## Manual acceptance

Record Neal's manual acceptance accurately.

Neal verified that:

- `--start-level` can start directly at an implemented production level and continue playing normally afterward.
- Starting at Level 10 works.
- Completing Level 10 naturally proceeds to Level 11.
- Starting at Level 9 and continuing normal gameplay successfully progresses through:
  - Level 9
  - Level 10
  - Level 11
  - Level 12
- Score and lives behave normally across the tested transitions.
- No further `--start-level` or Level 10 production-chain work is needed.

Treat the feature and integration as manually accepted.

## Accepted `--start-level` semantics

Freeze the accepted semantic distinction:

### `--level N`

Retains isolated/direct-level testing behavior.

Do not change its established use as the bounded single-level development/testing path.

### `--start-level N`

Starts a fresh game directly at genuine production level N and then follows normal production progression.

It must continue until:

- the current last implemented production level reaches its established next-level-not-implemented boundary;
- normal Game Over occurs;
- or the user manually exits.

Do not make `--start-level` terminal merely because its starting level completes.

## Fresh-start semantics

Preserve the accepted fresh-game initialization behavior for `--start-level`, including the established handling of:

- score;
- lives;
- compatible `--lives` override;
- Super Zapper;
- wave initialization;
- HUD displayed level;
- deterministic startup state.

Do not create synthetic campaign history for levels skipped before the requested start.

`--start-level N` means:

**start a fresh production game at N, then continue forward normally.**

## Production eligibility

Preserve the authoritative production-level eligibility derived from the production progression architecture.

Do not introduce a second independent hard-coded production-level list unless one already exists for an established architectural reason.

Preserve rejection of:

- unimplemented levels;
- direct-only development levels that are not genuine campaign members;
- incompatible startup-mode combinations.

Preserve Level 15's current direct-only development status.

Do not insert Level 15 into ordinary campaign progression.

## Level 10 production-chain correction

Freeze the accepted production progression:

**9 -> 10 -> 11 -> 12**

Level 10 must remain a real member of the current production campaign chain.

Preserve the existing Level 10 configuration now represented as the production Level 10 configuration.

Do not restore the old:

**9 -> 11**

shortcut.

Do not restore the older diagnostic-only Level 10 campaign boundary.

## Level 10 evidence classification

Be precise about what this closeout does and does not establish.

Preserve the established distinctions:

### ROM/DISASSEMBLY BACKED

- genuine Level 10 tube geometry where previously established;
- raw Level 10 presentation metadata where previously established.

### PROJECT TRANSLATION

- the currently preserved Level-7-like Level 10 gameplay configuration/mechanics where arcade Level 10 behavior has not been independently established.

Neal's manual acceptance establishes that the current Level 10 configuration plays normally and integrates correctly into the production campaign.

Do **not** upgrade historically approximate Level 10 gameplay mechanics into ROM-backed or arcade-verified facts merely because Level 10 is now part of progression.

Do not alter those mechanics during this closeout.

## Level 10 restart behavior

Preserve the newly normalized production restart behavior.

Do not restore obsolete Level-10-specific diagnostic restart exceptions.

Where normal production Game Over/restart behavior applies, Level 10 should follow the same established campaign startup/restart architecture rather than a special diagnostic branch.

Document the accepted behavior accurately.

## Recording and playback

Preserve the existing recording format.

Do not change deterministic seed/input semantics.

Document the production-chain consequence accurately:

Existing recordings involving Level 10 that continue beyond Level 10 completion or Game Over now follow the corrected production progression/restart semantics rather than the former diagnostic boundary.

Do not rewrite existing recordings merely because campaign progression changed.

Preserve compatibility with established playback infrastructure.

## Atari presentation

Preserve compatibility between `--start-level` and Atari presentation.

For example:

`--start-level 10 --atari-presentation`

must:

- start genuine production Level 10;
- use Level 10's current accepted presentation behavior;
- continue naturally to Level 11;
- update presentation configuration to the active level;
- update the Atari HUD to the active level;
- continue onward according to normal production progression.

Do not latch the initial level's presentation configuration across later levels.

Preserve all accepted per-level calibration values.

## Diagnostic presentation controls

Preserve compatibility with existing presentation diagnostics:

- `--tube-y-gain`
- `--tube-scale`
- `--tube-vertical-offset`
- `--calibration-guides`

Do not change their semantics.

Do not fine-tune any level visually during this task.

## Preserve Level 12 accepted state

Level 12 is already accepted, closed, committed, and pushed.

Do not modify accepted Level 12 behavior during this closeout.

Preserve:

- genuine row-12 geometry;
- closed topology;
- production wave;
- headless production soak;
- accepted Atari calibration;
- vector Fuseball bonus;
- deferred ExitZoom disposition;
- HUD;
- progression boundary at unimplemented Level 13.

Level 12 remains the current terminal production level until Level 13 exists.

## Preserve open-rim Flipper correction

Preserve the manually accepted open-end rim-Flipper pursuit-latch correction.

Do not alter:

- endpoint reversal;
- `RimPursuitDirection` synchronization;
- closed-rim behavior;
- below-rim Flipper routes;
- Tanker cargo routes.

No additional rim-pursuit work belongs in this closeout.

## Runtime capability probe

The recent VS Code Copilot runtime capability probe was an investigation only.

Do not change project architecture based on that probe in this task.

Preserve its historical AI-Logs prompt/report.

Do not implement:

- richer per-entity runtime telemetry;
- .NET debugger integration;
- AI gameplay harness;
- autonomous gameplay;
- new playback controls.

Those remain possible future diagnostics work only.

## Durable documentation

Update the appropriate durable project documentation to record the accepted state.

At minimum ensure:

### `TEMPEST-DEVELOPMENT-NOTES.md`

records:

- purpose and semantics of `--start-level`;
- distinction between `--start-level` and `--level`;
- production progression now includes Level 10;
- current accepted production chain through Level 12;
- Level 15 remains direct-only;
- manual acceptance of Level 9 -> 10 -> 11 -> 12 progression;
- Level 10 gameplay evidence classification remains project-translation where applicable.

### `TEMPEST-BEHAVIOR-SPEC.md`

record only durable behavior appropriate to that specification, including:

- continuing campaign startup behavior if it belongs in the behavior spec;
- Level 10's production campaign membership;
- normal carry/progression semantics.

Do not clutter the behavior spec with implementation history better suited to development notes or AI-Logs.

### README / CLI documentation

If existing README/help documentation describes direct level startup, update it as necessary so users/developers can distinguish:

`--level N`

from:

`--start-level N`

Keep descriptions concise and accurate.

Do not add speculative future-level behavior.

## Repository inspection

Before final validation or staging:

1. Verify branch and HEAD.
2. Verify upstream.
3. Verify ahead/behind.
4. Inspect index.
5. Inspect tracked worktree changes.
6. Inspect untracked files.
7. Identify exactly which changes belong to:
   - `--start-level`;
   - Level 10 production-chain correction;
   - related tests;
   - related durable documentation;
   - related AI-Logs.
8. Preserve unrelated pre-existing changes.

The Level 12 closeout report may have an intentional post-push tracked update still pending.

If it is clearly a normal historical report update, it may ride naturally with this commit.

Do not create a separate bookkeeping commit for it.

Preserve:

`Reference/arcade/MBUCOD.V05.txt`

as local/untracked.

Do not add protected/local assets.

## GitHub baseline

Use GitHub repository:

`nrnoble/Tempest-CSharp`

as the master reference for the last pushed state when available.

If GitHub retrieval returns no content, malformed output, or otherwise unexpectedly fails:

- do not silently proceed as though GitHub was successfully verified;
- clearly report the connector failure;
- use local Git state for the actual repository operation if it is authoritative and safe;
- do not fabricate remote verification;
- tell Neal if the GitHub connector needs manual reset/reconnection.

A connector failure alone does not require modifying source code.

## Final validation

Run a fresh bounded closeout gate from the exact manually accepted candidate.

At minimum run:

1. `--start-level` parser tests.
2. Startup configuration tests.
3. Direct `--level` regression tests.
4. `--start-level` semantic tests.
5. Production Level 10 startup tests.
6. Production Level 10 recording/playback identity tests.
7. Level 9 -> Level 10 progression tests.
8. Level 10 -> Level 11 progression tests.
9. Level 11 -> Level 12 progression tests.
10. Score/lives carry tests.
11. Restart/Game Over startup tests.
12. Atari-presentation startup/progression tests.
13. HUD active-level tests.
14. Recording/playback tests.
15. Level 12 headless production soak.
16. Level 1-12 production regression coverage.
17. Direct-only Level 15 preservation tests.
18. Rim-pursuit regression tests.
19. Fuseball bonus-vector tests.
20. Fuseball-animation/terminal regressions.
21. Generic ExitZoom accepted regressions.
22. Complete solution test suite.
23. Release solution build.
24. `git diff --check`.

Use explicit hang/time bounds.

Canceled, timed-out, stalled, or blocked commands do not count as passes.

Do not weaken Windows security.

Do not enter same-artifact retry loops.

Do not launch the interactive game.

Neal's required manual integration acceptance has already passed.

If a genuine production regression appears, stop before staging and report it.

## Stage only the accepted milestone

After validation passes:

1. Stage the accepted `--start-level` implementation.
2. Stage the Level 10 production-chain correction.
3. Stage associated tests.
4. Stage appropriate durable documentation.
5. Stage the paired relevant AI-Logs prompt/report records.
6. Include clearly pending tracked historical-report updates only where they naturally belong.
7. Do not stage unrelated work.
8. Inspect the staged inventory.
9. Inspect staged source/text diffs.
10. Run `git diff --cached --check`.
11. Confirm no protected/local material entered the index.

Do not stage:

- ROM files;
- `Reference/arcade/MBUCOD.V05.txt`;
- UserData;
- Temp;
- Videos;
- screenshots;
- gameplay recordings;
- personal scratch material;
- build output.

If staged ownership is ambiguous, stop before commit.

## Commit

If and only if final validation and staged review pass:

Commit to the current `master` branch with:

`Add continuing start-level campaign`

Do not amend previous commits.

Do not rewrite history.

Do not create another branch.

## Push

Push normally to:

`origin/master`

Do not force-push.

After push verify through local Git:

- local `master` equals local `origin/master`;
- ahead/behind is `0/0`;
- index is empty.

Where the GitHub connector is functioning, independently verify remote `master`.

If the connector fails, report the connector failure separately from the Git push result.

Do not claim connector verification occurred when it did not.

## Final report

Save this closeout prompt and a paired final closeout report under tracked AI-Logs.

Report:

- starting HEAD;
- final commit SHA;
- commit message;
- push result;
- branch/upstream;
- validation results;
- full-suite count;
- Release-build result;
- whitespace checks;
- exact committed file inventory;
- `--start-level` accepted semantics;
- preserved `--level` semantics;
- final production chain;
- Level 10 evidence classification;
- manual acceptance:
  - Level 10 start-and-continue passed;
  - Level 9 -> 10 -> 11 -> 12 passed;
- recording/playback implications;
- Level 15 direct-only preservation;
- excluded/local material;
- GitHub connector status;
- final index/worktree state.

If the final report is updated after the commit to insert the resulting SHA, do not create a commit/amend loop merely to commit its own post-push update.

Allow that tracked update to ride with the next normal project commit.

Do not begin Level 13.

TEMPEST_START_LEVEL_AND_LEVEL10_CHAIN_CLOSEOUT