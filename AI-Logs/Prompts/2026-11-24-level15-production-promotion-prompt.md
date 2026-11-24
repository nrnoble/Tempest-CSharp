Read this entire user-provided task before taking any action.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, files, commands, Git operations, or taking repository action, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with `TEMPEST_` appears on a line by itself in the user-provided task.

VS Code/agent-generated reminders, attachment metadata, current-editor context, or other automatically appended context after the user's token do not invalidate it.

If exactly one complete token cannot be found, stop before any project or repository action and explain why.

# Task: Promote accepted Level 15 into the normal production campaign

Level 15 gameplay and presentation have already passed Neal's manual acceptance, automated validation, closeout, commit, and push.

Do not reimplement Level 15.

Do not change Level 15 gameplay.

Do not recalibrate Level 15.

This task only removes the remaining direct-start/development-only campaign distinction and integrates the already-accepted Level 15 configuration into normal production progression.

## Authoritative handoff

Use:

`AI-Logs/Reports/2026-11-24-level15-final-closeout-report.md`

as the primary factual handoff.

That closeout establishes:

- accepted Level 15 gameplay;
- accepted genuine row-10 tube;
- accepted starting-Spike correction;
- accepted terminal-Spiker correction;
- accepted Level 15 Atari calibration:
  - Y gain `1.15`
  - tube scale `1`
  - vertical offset `75`
  - depth-Y gain `1`
- successful full validation;
- successful commit/push;
- Level 15 remained direct-start/development-only after that closeout.

Do not reinterpret the accepted gameplay.

## Starting repository audit

Before editing:

1. verify branch;
2. verify HEAD;
3. verify upstream;
4. verify ahead/behind;
5. inspect index;
6. inspect tracked changes;
7. inspect untracked material;
8. identify any post-push Level 15 closeout prompt/report records or other expected historical-record updates.

Preserve unrelated local/protected material.

Do not reset, restore, stash, clean, or rebase.

Do not stage, commit, or push during this implementation task.

## Promotion objective

Promote the already-accepted Level 15 configuration so production progression becomes:

`9 -> 10 -> 11 -> 12 -> 13 -> 14 -> 15`

Natural Level 14 completion must enter accepted production Level 15.

Level 15 becomes the current terminal production level while Level 16 remains unimplemented.

Do not implement Level 16.

## Remove obsolete direct-only distinction

Inspect all places where Level 15 is currently marked or treated as:

- direct-only;
- development-only;
- excluded from production progression;
- special-cased around startup/restart;
- absent from campaign eligibility.

Remove only restrictions that are obsolete now that the exact Level 15 candidate has passed manual acceptance.

Do not remove useful generic development direct-start functionality.

Do not alter accepted Level 15 configuration values.

## `--level 15`

Preserve the established behavior of normal production `--level`.

`--level 15` must start the accepted production Level 15 configuration.

Do not redesign `--level`.

Do not impose a new isolated-level interpretation.

Use current source/tests as authority for its established semantics.

## `--start-level 15`

Promote Level 15 into genuine production start eligibility.

`--start-level 15`

must:

- start a fresh production game at Level 15;
- use accepted Level 15 geometry/gameplay/presentation;
- follow ordinary production startup semantics;
- terminate at the normal Level-16-not-implemented boundary after Level 15 completion.

Preserve:

- score initialization;
- lives semantics;
- compatible `--lives` override;
- Super Zapper initialization;
- HUD;
- Atari presentation;
- recording/playback compatibility.

## Natural Level 14 -> Level 15 progression

Add/enable natural transition:

`Level 14 -> Level 15`

Preserve:

- Level 14 completion;
- exit/Approach timing;
- score carry;
- lives carry;
- between-level Super Zapper behavior;
- starfield behavior;
- active HUD update;
- Level 15 presentation configuration.

Do not fabricate Level 16.

## Level 15 terminal boundary

After genuine production Level 15 completes:

- remain at the established terminal/not-implemented-next-level behavior;
- do not silently wrap to another level;
- do not jump to direct Level 16;
- do not restart Level 15 unless the established terminal/restart architecture explicitly does so.

Use the same production boundary design used before the next level exists.

## Preserve accepted Level 15 gameplay

Freeze without modification:

- genuine shape row 10;
- topology/lane count;
- actual Level 15 enemy behavior;
- accepted picker reachability;
- absence of Pulsars;
- Flipper behavior;
- Tanker cargo;
- Spikers;
- Fuseballs;
- raw starting Spike `$A0`;
- C# Spike height approximately `142.857` at Depth 400 through the accepted coordinate translation;
- Spike growth/depletion;
- terminal Spiker-to-Tanker conversion;
- firing;
- shot limits;
- score;
- lives;
- Super Zapper;
- death/re-entry;
- completion.

No gameplay tuning belongs in this task.

## Preserve accepted Level 15 presentation

Freeze defaults:

- Y gain `1.15`
- scale `1`
- vertical offset `75`
- depth-Y gain `1`

Natural Level 14 -> 15 progression must activate those Level 15 defaults.

Do not retain Level 14 calibration after transition.

Preserve all general diagnostic overrides.

## Background-opacity calibration

Preserve accepted:

`--background-opacity`

behavior.

Do not change it during campaign promotion.

It remains development-only visual overlay tooling.

## ExitZoom

Use the existing generic accepted ExitZoom behavior.

Do not start an ExitZoom investigation.

If Level 15 shows the already-known presentation imperfection, that remains separate from production promotion unless Neal later declares it blocking.

## HUD

Preserve accepted vector HUD.

Natural entry into Level 15 must display level 15 and carry current score/lives correctly.

Do not alter HUD layout or digits.

## Recording/playback

Preserve recording format and deterministic input semantics.

Level 15 must now be treated as production where level validation/eligibility depends on campaign membership.

Existing Level 15 direct recordings should remain interpretable where compatible with the established recording schema.

Do not rewrite recordings.

## Headless production validation

Promote Level 15 into the established production headless validation architecture.

Run at least one bounded deterministic 1,200-tick production Level 15 soak using the accepted production configuration.

This should verify:

- normal production initialization;
- admission;
- movement;
- identities;
- cargo;
- firing;
- lanes;
- ledger consistency;
- no representation exceptions.

This is supplemental automated evidence; manual Level 15 gameplay acceptance has already passed.

## Production progression regression

Add a deterministic progression test proving:

`Level 14 -> Level 15`

using the actual production progression path.

Verify:

- active level becomes 15;
- score carries;
- lives carry;
- fresh Level 15 wave initializes;
- HUD identity changes appropriately;
- Level 15 presentation/calibration lookup resolves correctly.

Do not simulate Level 16 implementation.

## Automated tests

Add/update focused coverage proving:

1. Level 15 is a production campaign level.
2. Level 15 is no longer marked direct-only where campaign eligibility is concerned.
3. `--level 15` resolves to accepted production Level 15.
4. `--start-level 15` is accepted.
5. Natural Level 14 -> 15 progression works.
6. Score carries.
7. Lives carry.
8. Super Zapper transition behavior remains correct.
9. HUD displays 15.
10. Level 15 calibration resolves to `1.15 / 1 / 75 / 1`.
11. Genuine row-10 geometry remains unchanged.
12. Starting Spike remains corrected.
13. Terminal Spiker conversion remains correct.
14. Level 15 picker/enemy behavior remains unchanged.
15. Pulsars remain absent before Level 17.
16. Level 15 1,200-tick production soak passes.
17. Level 15 completion reaches Level-16-not-implemented boundary.
18. Level 14 remains unchanged.
19. Levels 1-13 remain unchanged.
20. `--start-level` earlier starts may progress into Level 15 structurally.
21. Recording/playback validation accepts genuine production Level 15 appropriately.
22. direct/startup parser regressions remain passing.
23. Atari presentation/model frame/topology remain unchanged.
24. background-opacity remains compatible.

## Durable documentation

Update documentation only as required to reflect promotion.

### `TEMPEST-DEVELOPMENT-NOTES.md`

Record:

- Level 15 is now part of normal production progression;
- production chain extends through Level 15;
- Level 15 is terminal until Level 16;
- manual gameplay/presentation acceptance was already completed before promotion;
- accepted Level 15 calibration;
- corrected starting Spike;
- terminal-Spiker behavior.

Remove obsolete statements that Level 15 remains direct-only.

### `TEMPEST-BEHAVIOR-SPEC.md`

Record campaign membership/progression where appropriate.

Do not rewrite evidence classification.

Do not convert project translations into ROM facts.

## Validation

Run bounded:

1. Level 15 promotion/eligibility tests.
2. Level 14 -> 15 progression tests.
3. `--start-level 15`.
4. direct Level 15 startup.
5. Level 15 production tests.
6. Level 15 Spike tests.
7. terminal Spiker tests.
8. Flipper/Tanker/Spiker/Fuseball regressions.
9. Level 15 1,200-tick headless production soak.
10. HUD/presentation/calibration tests.
11. background-opacity regressions.
12. recording/playback tests.
13. progression tests across Levels 9-15.
14. Levels 1-14 regression coverage.
15. complete solution test suite.
16. Release solution build.
17. `git diff --check`.

Use explicit hang/time limits.

Canceled, timed-out, stalled, or security-blocked commands are not passes.

Do not launch the interactive game.

Neal's Level 15 manual gameplay/presentation acceptance has already passed.

## Manual handoff

No new Level 15 gameplay-fidelity acceptance is required unless implementation changes gameplay unexpectedly.

Provide commands for Neal to verify campaign promotion if desired:

`dotnet run --project src\Tempest.Game -- --start-level 14 --lives 10`

and:

`dotnet run --project src\Tempest.Game -- --start-level 15 --lives 10`

Optional Atari presentation versions may also be provided.

The expected manual observation is only:

- Level 14 completes into Level 15;
- Level 15 presentation/configuration activates correctly;
- Level 15 remains otherwise identical to the accepted direct-start version.

Do not require Neal to replay the full Level 15 acceptance checklist.

## Records and Git

Save this prompt and paired promotion report under tracked AI-Logs.

Do not stage, commit, or push yet.

Preserve post-push Level 15 closeout records and unrelated local material.

Do not reset, restore, stash, clean, or rebase.

Stop after production promotion, validation, headless soak, and optional manual handoff.

TEMPEST_LEVEL15_PRODUCTION_PROMOTION
