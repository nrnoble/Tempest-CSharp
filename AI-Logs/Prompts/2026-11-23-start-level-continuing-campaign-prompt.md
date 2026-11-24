Read this entire user-provided task before taking any action.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, files, commands, or taking repository action, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with `TEMPEST_` appears on a line by itself in the user-provided task.

If exactly one complete token cannot be found, stop before any project or repository action and explain why.

# Task: Add a production `--start-level` option that continues normal progression

Implement one small development/convenience feature before Level 13.

Neal needs to be able to start Tempest directly at any implemented production level and then continue playing normally through subsequent implemented production levels.

The existing `--level N` behavior is useful for isolated level testing and should retain its current terminal semantics.

Add a new option:

`--start-level N`

## Required behavior

`--start-level N` means:

1. Initialize a fresh game directly at production displayed level N.
2. Use that level's normal production configuration.
3. Allow the level to complete normally.
4. Continue through normal production progression afterward.
5. Continue until:
   - the current last implemented production campaign level reaches its established terminal/unimplemented-next-level boundary;
   - normal Game Over occurs;
   - or the user manually exits.

Do not automatically exit merely because the startup level completes.

## Preserve `--level`

Do not change the established meaning of:

`--level N`

It remains the direct isolated-level testing option.

If the current architecture makes `--level` terminal after the selected level, preserve that behavior.

This gives the project two explicitly different workflows:

### Isolated testing

`--level 12`

Starts Level 12 and retains the established isolated/direct-level behavior.

### Campaign-from-here testing

`--start-level 12`

Starts Level 12, then proceeds to Level 13 once Level 13 exists, then later levels through normal production progression.

For the current project state, if Level 12 is the last implemented production campaign level, `--start-level 12` should behave normally through Level 12 and then stop at the established Level-13-not-implemented boundary.

Once later levels are added, no parser redesign should be required for `--start-level` to continue through them.

## Fresh-game semantics

`--start-level N` begins a fresh game at the requested level.

Use the project's established direct-level initialization semantics for:

- starting score;
- starting lives;
- Super Zapper state;
- enemy/wave initialization;
- random/deterministic startup;
- HUD displayed level.

Preserve existing compatible development overrides such as `--lives` where already supported.

Do not invent saved campaign state or fabricate score/lives that would theoretically have been accumulated on earlier levels.

The purpose is:

**start a fresh test/game at N and then continue forward naturally.**

## Production levels only

`--start-level` must operate only on genuine implemented production campaign levels.

Do not treat diagnostic or direct-only levels as normal campaign starts merely because `--level` can access them.

In particular preserve the current project distinction around:

- diagnostic Level 10;
- direct-only Level 15 development candidate;
- unimplemented future levels.

Inspect the current production progression architecture and define `--start-level` eligibility from the same authoritative production-level knowledge rather than duplicating a fragile hard-coded list if possible.

Do not silently insert direct-only Level 15 into campaign progression.

## Current production progression

Preserve the current accepted production campaign order and terminal boundaries.

Do not use this feature to redesign or renumber existing production progression.

If the current campaign progresses Level 11 -> Level 12 and then stops because Level 13 is not implemented, preserve exactly that behavior.

When Level 13 is implemented later, the same normal production progression architecture should naturally allow:

`--start-level 11`

to continue:

11 -> 12 -> 13

without another special `--start-level` implementation change.

## Atari presentation

`--start-level` must remain compatible with:

`--atari-presentation`

For example:

`dotnet run --project src\Tempest.Game -- --start-level 12 --atari-presentation`

must:

- initialize genuine production Level 12;
- use Level 12's accepted Atari-presentation calibration;
- use the Atari HUD;
- proceed through future production levels using each level's normal presentation configuration.

Do not make presentation settings stick to the initially selected level after progression.

## Calibration overrides

Existing presentation diagnostics must remain compatible where already supported:

- `--tube-y-gain`
- `--tube-scale`
- `--tube-vertical-offset`
- `--calibration-guides`

Preserve their existing semantics.

Do not redesign whether explicit diagnostic overrides remain active across level transitions. Inspect and retain the current project rule.

## Playback and recording

Inspect current compatibility rules for:

- `--playback-file`;
- recording;
- direct-level startup;
- headless playback.

Do not combine incompatible startup modes silently.

At minimum:

- reject simultaneous `--level` and `--start-level`;
- reject duplicate `--start-level`;
- preserve existing playback-file ownership of startup state;
- do not let `--start-level` override the starting level embedded in a playback recording.

If `--start-level` and playback are semantically incompatible, reject the combination clearly before window creation.

Do not change recording-file format.

## Restart behavior

Inspect the existing meaning of `R`/restart after Game Over or terminal state.

Choose behavior consistent with established direct startup configuration.

Prefer restart to create another fresh game using the configured `--start-level N` rather than unexpectedly returning to Level 1, if that matches how current startup configuration is retained.

Document and test the actual chosen behavior.

Do not guess if the existing startup architecture already defines this.

## Parser behavior

Follow existing CLI parsing conventions.

Accept a valid integer production level:

`--start-level N`

Reject:

- missing value;
- malformed integer;
- out-of-range/unimplemented production level;
- duplicate `--start-level`;
- simultaneous `--level` and `--start-level`;
- incompatible playback/startup combinations.

Return clear errors before interactive window creation where existing CLI architecture supports that.

Do not add `--start-level=N` syntax unless the parser already supports assignment syntax consistently.

## Headless validation

Add a bounded deterministic production test proving the important semantic difference between `--level` and `--start-level`.

Do not rely only on parser unit tests.

Once a scenario has at least two consecutive implemented production levels available, test that:

- direct `--level N` retains isolated terminal behavior;
- `--start-level N` completes N and proceeds to the next normal production level.

For the current project state, use the latest consecutive implemented production levels that actually exist.

If current progression provides Level 11 -> Level 12, that is a suitable production transition.

Do not fabricate Level 13 for testing.

## Automated tests

Add focused coverage proving at minimum:

1. `--start-level 1` is accepted.
2. `--start-level 11` is accepted if Level 11 is production.
3. `--start-level 12` is accepted if Level 12 is production.
4. Unsupported/unimplemented levels are rejected.
5. Diagnostic/direct-only levels are rejected unless they are genuine production campaign levels.
6. `--level` behavior remains unchanged.
7. `--level` and `--start-level` together are rejected.
8. Duplicate `--start-level` is rejected.
9. Missing value is rejected.
10. Malformed value is rejected.
11. Playback startup state is not overridden.
12. Fresh score state is correct.
13. Starting lives follow established direct-start semantics and compatible `--lives` override behavior.
14. Super Zapper starts correctly.
15. HUD displays the requested starting level.
16. Production enemy/wave initialization is correct.
17. `--start-level 11` can naturally progress to Level 12.
18. Score/lives carry normally from the chosen start level to the next level.
19. Presentation follows the active level after progression.
20. Atari HUD follows the active level after progression.
21. Terminal behavior occurs only at the current last implemented production boundary.
22. Restart behavior follows the documented startup rule.
23. Existing direct-level tests remain passing.
24. Existing campaign/progression tests remain passing.
25. Existing playback/recording tests remain passing.

## Preserve accepted project behavior

Do not change:

- Level gameplay;
- geometry;
- enemy selection;
- scoring;
- lives mechanics;
- Super Zapper mechanics;
- presentation calibration;
- HUD;
- Fuseball behavior;
- Flipper behavior;
- playback format;
- current terminal-boundary behavior.

This feature controls **where a fresh production campaign test begins**, not how levels behave.

## Validation

Run bounded:

1. New `--start-level` parser tests.
2. Direct/start-level semantic tests.
3. Production transition tests.
4. Appropriate headless production progression test.
5. Existing direct-level startup tests.
6. Gameplay recording/playback tests.
7. Progression tests.
8. HUD/presentation tests.
9. Levels 1-12 regression coverage.
10. Complete solution test suite.
11. Release solution build.
12. `git diff --check`.

Use established explicit hang timeouts.

Do not weaken Windows security.

Do not enter same-artifact retry loops.

Do not launch the interactive game.

## Manual handoff

After automated validation provide concise commands demonstrating:

### Existing isolated mode

`dotnet run --project src\Tempest.Game -- --level 11`

### New continue-forward mode

`dotnet run --project src\Tempest.Game -- --start-level 11`

### Atari presentation

`dotnet run --project src\Tempest.Game -- --start-level 11 --atari-presentation`

### Current terminal production level

`dotnet run --project src\Tempest.Game -- --start-level 12`

Neal should verify:

- starting directly at the requested production level works;
- the game plays normally;
- completing Level 11 naturally proceeds to Level 12;
- score and lives carry;
- Level 12 remains terminal while Level 13 is unimplemented;
- existing `--level` continues to provide isolated level testing.

## Records and Git

Save this substantial prompt and implementation report under tracked AI-Logs.

Do not stage, commit, or push this small candidate yet unless Neal explicitly requests closeout afterward.

Preserve unrelated tracked/untracked material and the pending post-push Level 12 closeout-report update.

Do not reset, restore, stash, clean, or rebase.

Do not begin Level 13.

Stop after implementation, automated validation, and manual handoff.

TEMPEST_START_LEVEL_CONTINUING_CAMPAIGN