Read this entire user-provided task before taking any action.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, files, commands, Git operations, or taking repository action, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with `TEMPEST_` appears on a line by itself in the user-provided task.

VS Code/agent-generated reminders, attachment metadata, current-editor context, or other automatically appended context after the user's token do not invalidate it.

If exactly one complete token cannot be found, stop before any project or repository action and explain why.

# Task: Final closeout, commit, and push of accepted Level 15 production promotion

The already accepted Level 15 configuration has now been promoted into the normal production campaign and has passed Neal's manual promotion test.

This is a CLOSEOUT task.

Do not change Level 15 gameplay.

Do not recalibrate Level 15.

Do not implement Level 16.

## Authoritative handoff

Use:

`AI-Logs/Reports/2026-11-24-level15-production-promotion-report.md`

as the implementation/automated-validation handoff.

That report establishes:

- accepted Level 15 configuration is now the production successor to Level 14;
- Level 15 is the current terminal production level;
- Level 16 is unimplemented;
- natural Level 14 -> Level 15 production progression is implemented;
- both startup selectors accept Level 15;
- accepted Level 15 gameplay/configuration itself was not changed by promotion;
- focused validation passed 1,204 tests;
- complete Release suite passed 1,973 tests;
- Release build passed with 0 warnings and 0 errors;
- two deterministic 1,200-tick Level 15 production soaks passed;
- `git diff --check` passed.

Do not repeat the implementation work.

## Manual promotion acceptance

Record Neal's manual acceptance:

**Level 15 production promotion passes manual testing.**

Neal verified that:

- Level 14 completes naturally into Level 15;
- Level 15 presentation/configuration activates correctly;
- Level 15 otherwise behaves like the already accepted direct-start version;
- no further production-promotion work is required.

This is campaign-integration acceptance.

Do not claim new gameplay-fidelity evidence beyond the previously completed Level 15 manual acceptance.

## Freeze production progression

Freeze the accepted production chain:

`9 -> 10 -> 11 -> 12 -> 13 -> 14 -> 15`

Preserve:

- natural Level 14 -> Level 15 transition;
- score carry;
- lives carry;
- between-level Super Zapper behavior;
- Approach;
- active HUD transition;
- active Level 15 calibration.

Level 15 remains terminal while Level 16 is unimplemented.

Do not implement or fabricate Level 16.

## Remove obsolete direct-only terminology

Review current source/comments/tests/documentation for obsolete statements that Level 15 is:

- direct-only;
- development-only;
- excluded from production progression.

Where those statements refer to current campaign eligibility, update them to the accepted production status.

Do not erase historical AI-Logs showing that Level 15 originally existed as a direct-only development candidate.

Historical records remain factual history.

## Preserve accepted Level 15 gameplay

Do not change:

- genuine row-10 geometry;
- topology/lane count;
- current accepted gameplay configuration;
- picker behavior;
- ordinary Flipper reachability;
- Tanker behavior/cargo;
- Spiker behavior;
- Fuseball behavior;
- Pulsar absence;
- raw starting Spike `$A0`;
- approximately 142.857 C# starting Spike height;
- Spike growth/depletion;
- terminal Spiker -> Tanker conversion;
- firing;
- shot limits;
- score;
- lives;
- Super Zapper;
- death/re-entry;
- wave completion.

Do not reinterpret the accepted gameplay baseline during promotion closeout.

## Preserve accepted Level 15 presentation

Freeze:

- tube Y gain `1.15`
- tube scale `1.0`
- tube vertical offset `75`
- tube depth-Y gain `1.0`

Do not change these values.

Preserve all development calibration overrides.

## `--level` and `--start-level`

Preserve accepted current semantics.

Level 15 is now a genuine production level for startup eligibility.

Verify:

- `--level 15` resolves to accepted production Level 15;
- `--start-level 15` resolves to accepted production Level 15;
- earlier valid production starts may naturally progress into Level 15;
- completion of Level 15 stops at the established Level-16-not-implemented boundary.

Do not redesign either selector.

## Recording/playback

Preserve the current recording schema.

Preserve Level 15 recording/playback compatibility added during promotion.

Do not rewrite existing recordings.

Preserve deterministic behavior and level identity.

## Background-opacity diagnostic

Preserve the accepted global:

`--background-opacity`

feature.

Do not modify it during promotion closeout.

If its post-push closeout report update is pending and naturally belongs in this normal commit, include it according to the established historical-report convention.

## ExitZoom

Preserve generic ExitZoom.

Do not investigate or alter the known recurring presentation imperfection.

It remains non-blocking.

## Headless production soak

Preserve the new Level 15 production soak.

The final accepted automated evidence includes two seeded 1,200-tick Level 15 production runs.

Do not weaken them.

Preserve the corrected soak assertion that allows legitimate ordinary Flippers where the accepted picker makes them reachable.

Do not change production code to satisfy an outdated assumption that every Level 15 Flipper must be Tanker cargo.

## Durable documentation

Update durable documentation only as needed to reflect the accepted promotion.

### `TEMPEST-DEVELOPMENT-NOTES.md`

Ensure it records:

- Level 15 is now a normal production campaign level;
- production progression extends through Level 15;
- Level 15 is terminal while Level 16 remains unimplemented;
- manual promotion acceptance passed;
- accepted Level 15 calibration remains unchanged;
- corrected `$A0` starting Spike remains accepted;
- terminal-Spiker behavior remains accepted.

### `TEMPEST-BEHAVIOR-SPEC.md`

Update campaign progression/membership where appropriate.

Preserve evidence classifications.

Do not relabel project translations as ROM-backed facts.

## Repository audit

Before fresh validation/staging:

1. verify branch;
2. verify HEAD;
3. verify upstream;
4. verify ahead/behind;
5. inspect index;
6. inspect tracked worktree;
7. inspect untracked files;
8. identify exactly the promotion changes;
9. identify paired promotion prompt/report records;
10. identify post-push Level 15 closeout records and earlier historical Level 15 records;
11. preserve unrelated local/reference material.

Do not stage:

- ROM files;
- `Reference/arcade/MBUCOD.V05.txt`;
- Temp/TEMP;
- UserData;
- screenshots;
- Videos;
- gameplay recordings;
- build outputs;
- personal scratch material.

Do not modify historical reports merely to modernize wording.

## Fresh final validation

Run a fresh bounded closeout gate from the exact manually accepted promotion candidate.

At minimum run:

1. Level 15 production eligibility tests.
2. Level 14 -> Level 15 progression tests.
3. `--level 15` startup.
4. `--start-level 15` startup.
5. production-chain tests through Level 15.
6. score/lives carry tests.
7. Super Zapper transition tests.
8. Level 15 HUD tests.
9. Level 15 calibration tests.
10. Level 15 Spike tests.
11. terminal-Spiker tests.
12. Level 15 picker/reachability tests.
13. Flipper/Tanker/Spiker/Fuseball regressions.
14. Level 15 recording/playback tests.
15. Level 15 two-seed 1,200-tick production soak.
16. background-opacity regressions.
17. Atari-presentation/model-frame/topology tests.
18. Levels 1-14 regression coverage.
19. complete solution test suite.
20. Release solution build.
21. `git diff --check`.

Use explicit hang/time bounds.

Canceled, timed-out, blocked, or stalled commands are not passes.

Do not weaken Windows security.

Do not launch the interactive game.

Manual acceptance has already passed.

If a genuine production regression appears, stop before staging.

## Stage only accepted promotion work

After validation passes:

1. stage production-chain/promotion source;
2. stage associated tests;
3. stage durable documentation;
4. stage relevant promotion AI-Logs prompt/report records;
5. include clearly pending accepted post-push historical report updates where they naturally belong;
6. inspect staged inventory;
7. inspect staged source/text diffs;
8. run `git diff --cached --check`;
9. verify no protected/unrelated material is staged.

If file ownership is ambiguous, stop before commit.

## Commit

If and only if final validation and staged review pass:

Commit on current `master` with:

`Promote accepted Level 15 to production`

Do not amend earlier commits.

Do not rewrite history.

Do not create another branch.

## Push

Push normally to:

`origin/master`

Do not force-push.

After push verify locally:

- local `master` equals `origin/master`;
- ahead/behind is `0/0`;
- index is empty.

Where GitHub connector retrieval works, independently verify remote `master`.

If connector retrieval fails, report that explicitly rather than claiming remote connector verification.

## Final report

Save this closeout prompt and paired final promotion-closeout report under tracked AI-Logs.

Record:

- starting HEAD;
- final commit SHA;
- commit message;
- push result;
- GitHub connector result;
- branch/upstream;
- validation results;
- focused count;
- full-suite count;
- Release build;
- whitespace gates;
- exact committed file inventory;
- manual promotion acceptance;
- final production chain;
- Level 15 terminal status;
- accepted Level 15 calibration;
- corrected starting Spike preservation;
- terminal-Spiker preservation;
- production soak preservation;
- recording/playback status;
- excluded/local/protected files;
- final index/worktree state.

Do not create an amend/report/commit loop merely to commit the new SHA into its own report.

Allow a post-push report update to ride with the next normal project commit.

Do not begin Level 16.

TEMPEST_LEVEL15_PRODUCTION_PROMOTION_CLOSEOUT
