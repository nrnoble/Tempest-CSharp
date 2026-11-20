FAIL-CLOSED COMPLETENESS CHECK

Read this entire user-provided task before using any tools, reading files,
running commands, modifying files, staging, committing, pushing, or taking any
repository action.

Before doing anything else, locate the unique completion token near the end of
this user-provided task.

Verify exactly one complete standalone token beginning with "TEMPEST_" appears
on a line by itself in the user-provided task.

VS Code/agent-generated reminders, attachment information, tagged-file context,
or other system-generated information after the user's completion token do not
invalidate it.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before
any project or repository action.


TEMPEST SELF-DESCRIBING GAMEPLAY RECORDING VERSION 2

Implement the smallest safe enhancement that allows recorded gameplay to use
production startup options such as:

--level N

--lives N

--rim-speed N

and allows playback to restore those effective simulation values directly from
the recording.

Preserve the existing accepted record/playback mechanics:

- per-tick effective-input capture;
- seeded deterministic replay;
- normal production simulation;
- existing frame semantics;
- existing restart-input semantics;
- live-input handoff after recorded input ends;
- existing fixed simulation step;
- existing RNG implementation.

Do not implement advanced playback controls in this milestone.

Stop after automated validation and provide manual record/playback commands for
Neal.

Do not stage.

Do not commit.

Do not push.


ACTIVE REPOSITORY

Work only in:

C:\Users\nrnob\source\repos\Tempest

Do not use or modify the frozen former OneDrive repository.


EXPECTED BASELINE

Expected branch:

master

Expected HEAD:

8222acee9baf6e893499a7d6ddb859bd1b7e0903
Add direct level startup and controller quit

Expected:

- tracked worktree clean;
- index clean;
- no unexplained nonignored untracked files;
- master tracks origin/master;
- local master and origin/master identify the same commit.

Before changing anything run:

git status --short --branch
git rev-parse HEAD
git log -1 --oneline
git rev-parse origin/master
git diff --stat
git diff --cached --stat
git ls-files --others --exclude-standard
git remote -v

If repository state differs unexpectedly, STOP rather than resetting, cleaning,
restoring, deleting or discarding anything.


READ FIRST

Read:

.github/copilot-instructions.md

TEMPEST-DEVELOPMENT-NOTES.md

TEMPEST-BEHAVIOR-SPEC.md

AI-Logs/Reports/2026-11-06-record-playback-startup-options-analysis-report.md

Also read the historical reports covering:

- deterministic gameplay playback foundation;
- scenario-selectable gameplay playback;
- scenario-appropriate gameplay playback profiles;
- recorded-player input;
- recorded-input round-trip manual acceptance;
- generic direct-level startup;
- Level 5 and Game Over accepted checkpoints.

Inspect current source and tests including:

Program.cs

Debug/DebugController.cs

Debug/GameplayRecordingFile.cs

Debug/GameplayPlayback.cs

Systems/PlayerInputRecorder.cs

Systems/PlayerInputReplay.cs

Systems/InputSystem.cs

TempestGame.cs

Levels/LevelProgression.cs

Levels/LevelConfiguration.cs

GameplayRecordingTests.cs

GameplayPlaybackTests.cs

DebugControllerTests.cs

DirectLevelStartupTests.cs

InputDiagnosticTests.cs

RimSpeedDiagnosticTests.cs

Verify current code rather than implementing only from the analysis report.


ACCEPTED BASELINE TO PROTECT

The existing recorded-input round trip has received extremely strong manual
acceptance.

Neal judged recording normal manual gameplay and playing it back separately as
100% perfect and exactly what he wanted.

The replay lets Neal concentrate entirely on observing gameplay instead of
simultaneously playing.

This implementation must extend startup identity without degrading:

- input timing;
- player movement;
- firing;
- Super Zapper timing;
- death/restart input;
- RNG consumption;
- normal simulation;
- playback presentation;
- live-input handoff.

Do not redesign accepted frame capture or replay consumption.


PRIMARY PURPOSE

As later levels become faster and more difficult, Neal should be able to play a
difficult scenario successfully once and reuse that exact session for repeated
observation and regression.

Desired recording example:

dotnet run --project src\Tempest.Game -- --record-gameplay level5.json --level 5 --lives 10

Desired playback example:

dotnet run --project src\Tempest.Game -- --playback-file level5.json

Playback should automatically restore the recorded production starting level,
starting lives, rim-speed multiplier and random seed before replaying the
recorded per-tick inputs.


CANONICAL GAMEPLAY STARTUP CONFIGURATION

Introduce the smallest maintainable immutable value/type representing effective
production gameplay startup identity.

Use an appropriate project-consistent name, preferably conceptually equivalent
to:

GameplayStartupConfiguration

It should contain only the simulation-affecting production values currently
required for reproducible recording:

- starting displayed production level;
- effective starting lives;
- effective rim-speed multiplier;
- deterministic random seed.

Do not serialize or embed an entire LevelConfiguration object.

The displayed starting level must resolve through the existing production
level progression/configuration catalog.

Keep existing production level initialization authoritative.

Do not create duplicate versions of Levels 1–7.


ONE RESOLVED STARTUP PATH

The objective is a bounded flow:

command-line options
->
validated effective GameplayStartupConfiguration
->
normal production TempestGame initialization

Recording:

validated effective GameplayStartupConfiguration
+
effective per-tick input
->
version-2 recording

Playback:

version-1 or version-2 recording
->
restored GameplayStartupConfiguration
+
recorded input
->
normal production TempestGame initialization and replay

Do not build a broad settings framework.

Do not include speculative future fields.


SIMULATION-AFFECTING VALUES

For supported production recordings, preserve these effective values:

1. Starting production displayed level.

2. Starting lives.

3. Rim-speed multiplier.

4. Initial random seed.

These values define recording identity for this milestone.


OBSERVATION-ONLY VALUE

The current:

--game-over-density N

is renderer-only and must not become part of deterministic gameplay identity.

It may compose with recording and playback if the current architecture can
support that cleanly without altering simulation.

Do not serialize it in the version-2 gameplay startup configuration.

A different density during playback must not change deterministic game state.


SPECIAL MODES REMAIN EXCLUDED

Do not record or reinterpret special development scenarios such as:

--level2-test

--level3-test

--level4-test

--level6-test

--level7-test

--level3-geometry-test

--spiker-test

--tanker-test

--fuseball-test

--pulsar-test

--enemy-bullets-test

--input-diagnostic

--gameplay-playback

--gameplay-playback-level4

These are not ordinary production recording startup identity.

Keep them incompatible with file recording/playback unless an already accepted
behavior requires otherwise.

Return clear command-line errors rather than silently ignoring conflicting
modes.


RECORDING FORMAT VERSION 2

Create an explicit recording format version 2.

Preserve the existing format identifier unless there is a demonstrated reason
not to.

Version 2 must retain all existing fields and input semantics, including:

- format identifier;
- random seed;
- duration ticks;
- ordered frames;
- simulation tick;
- restart pressed;
- Fire;
- MouseDeltaX;
- Left;
- Right;
- ZapPressed;
- ControllerAxis.

Version 2 must additionally store effective startup values for:

- starting displayed production level;
- starting lives;
- rim-speed multiplier.

Use clear, stable field names.

Do not serialize type names or the LevelConfiguration object graph.


VERSION 1 BACKWARD COMPATIBILITY

Continue reading existing version-1 recording files.

Interpret version 1 exactly according to its established semantics:

- starting production Level 1;
- starting lives 3;
- rim-speed multiplier 1.0;
- recorded random seed;
- existing duration and per-tick frames.

Do not rewrite or migrate version-1 files.

Do not weaken their validation.

Do not require Neal to recreate old recordings.

New recordings should be written as version 2.


OLD BUILD COMPATIBILITY

New version-2 recordings do not need to be readable by older builds.

The current build must clearly reject unsupported future versions.

Do not pretend version 2 is version 1 merely to gain superficial compatibility.


STRICT FILE VALIDATION

Preserve strict validation of:

- format identifier;
- supported version;
- required fields;
- nonnegative seed;
- nonnegative duration;
- duration/frame-count agreement;
- contiguous simulation ticks;
- complete input values;
- finite floating-point values;
- supported production starting level;
- valid positive starting lives;
- valid positive finite rim-speed multiplier;
- unknown JSON members.

Do not weaken:

UnmappedMemberHandling.Disallow

or equivalent strict unknown-member handling.

For version 1, require the original version-1 structure.

For version 2, require the complete version-2 structure.


SUPPORTED PRODUCTION LEVELS

Version-2 recordings currently support production starting Levels 1–7.

Resolve them through the same accepted production catalog used by:

--level N

Reject unsupported production Levels such as 0, 8, negative values or arbitrary
large values.

The design should naturally accommodate Level 8 later after it becomes a real
production level, without adding another recording architecture.


STARTING LIVES

Record the effective starting-lives value, not merely whether --lives was
present.

Playback must restore that exact starting-lives value.

Deaths, Game Over and full-game restart must use the same effective semantics as
the originally recorded session.

Do not silently fall back to 3 when a version-2 file contains invalid lives.


RIM-SPEED MULTIPLIER

Record the effective rim-speed multiplier because it changes Flipper simulation.

Playback must restore it from the recording.

Use the same validation semantics as the accepted runtime option:

--rim-speed N

Do not reinterpret it as observation-only.

Do not change the accepted default or calibrated value.


RANDOM SEED ORDERING

The recording already stores an initial random seed.

Preserve the existing RNG implementation.

The restored startup configuration, production level and seed must be applied
before any startup construction that consumes gameplay randomness.

Do not generate the game using default Level 1 and later replace it with the
recorded level.

Do not consume extra random values while parsing or translating metadata.

Do not serialize internal mutable RNG state in this milestone.


RECORD COMMAND SEMANTICS

Support a production recording command conceptually equivalent to:

dotnet run --project src\Tempest.Game -- --record-gameplay level5.json --level 5 --lives 10

Also support:

dotnet run --project src\Tempest.Game -- --record-gameplay level5.json --level 5 --lives 10 --rim-speed 2.5

and, if cleanly supported:

dotnet run --project src\Tempest.Game -- --record-gameplay level5.json --level 5 --lives 10 --game-over-density 2

Use the existing accepted option names.

Do not rename --record-gameplay or --playback-file.


ARGUMENT ORDER

Choose and document one clear grammar.

Preferred behavior:

- transport flag and filename identify record/playback mode;
- recognized compatible options may follow in any ordinary supported order;
- duplicate simulation-affecting options are rejected;
- missing option values are rejected;
- malformed option values are rejected;
- unknown options are rejected;
- conflicting scenario modes are rejected.

If fully order-independent parsing would require a disproportionately broad
rewrite, preserve a simple documented order around the transport flag and path.

Do not silently ignore extra arguments.


RECORD DEFAULTS

Recording without explicit startup options should remain valid:

dotnet run --project src\Tempest.Game -- --record-gameplay level1.json

Its version-2 recording should contain the effective defaults:

- Level 1;
- 3 lives;
- rim speed 1.0;
- generated deterministic seed.

This should preserve the practical behavior of the accepted Level 1 recording
workflow.


PLAYBACK COMMAND SEMANTICS

Playback should ordinarily require only:

dotnet run --project src\Tempest.Game -- --playback-file level5.json

Playback must obtain all simulation-affecting startup values from the recording.

The version-2 file is authoritative for:

- starting level;
- starting lives;
- rim-speed multiplier;
- seed.


REJECT PLAYBACK SIMULATION OVERRIDES

Reject explicit simulation-affecting playback overrides, including:

--level

--lives

--rim-speed

special scenario/test options

even if the supplied value happens to equal the recording metadata.

Do not:

- silently ignore the override;
- accept it with only a warning;
- allow it to replace recorded metadata.

A playback session should have one unambiguous simulation identity.


PLAYBACK OBSERVATION OPTIONS

An observation-only option such as:

--game-over-density N

may be accepted with playback if it does not alter game simulation and can be
supported without complicating the parser.

If accepted, test that changing density does not alter deterministic simulation
state.

If current parsing makes this unsafe or disproportionately broad, keep it
incompatible and document that as a bounded decision.

Do not serialize density as gameplay identity.


PRODUCTION INITIALIZATION

Recording and playback must initialize through the same accepted production path
as ordinary/direct startup.

Direct production Level 5 recording must include:

- real Level 5 geometry;
- real Level 5 starting Spikes;
- accepted Level 5 $53 Flipper/Spike behavior;
- real Level 5 enemy supply and eligibility;
- normal onward progression to Level 6.

Do not create a special recorded Level 5 mode.

The same principle applies to all supported Levels 1–7.


NATURAL PROGRESSION

A recording that starts at Level 5 may continue naturally into Level 6 and Level
7 if the recorded session reaches them.

Playback must reproduce that normal progression from the recorded starting
configuration.

Do not make the starting level terminal.


FULL-GAME RESTART

Preserve the recorded effective starting configuration when the recorded session
contains a full-game restart request, if that is the accepted current behavior
for a recording session.

Investigate and test the current restart semantics carefully:

- ordinary live-game restart policy;
- direct-start restart policy;
- recorder's RestartPressed field;
- playback reset behavior.

Do not casually change accepted restart semantics.

If current direct-start and recorded restart semantics conflict, choose the
smallest behavior that reproduces the originally recorded session and document
the project decision.


RECORDING WRITE SEMANTICS

Preserve:

- writing only at the established normal completion/window-close point;
- temporary sibling-file creation;
- non-overwriting final move;
- existing failure handling.

Do not weaken save safety.

Do not overwrite existing recording files unless that is already accepted
behavior.


INPUT FRAME SEMANTICS

Do not change:

- one effective recorded frame per RunOneStep;
- neutral-frame recording;
- restart capture;
- simulation tick numbering;
- input field meanings;
- playback cursor advancement;
- live-input handoff after the final recorded frame.

These are protected accepted mechanics.


SMALL TYPE DESIGN

If introducing GameplayStartupConfiguration or equivalent:

- make it immutable or effectively immutable;
- validate it at a clear boundary;
- avoid nullable/partially resolved state;
- store effective values rather than raw command-line strings;
- keep transport filename/mode outside gameplay startup identity;
- keep renderer-only density outside simulation identity;
- avoid broad dependency injection or configuration-framework changes.

Use maintainability-focused comments only where the simulation/recording
boundary would otherwise be unclear.


AUTOMATED TESTS — FILE FORMAT

Add focused coverage for:

1. Version-2 save/load round trip preserves:
   - starting level;
   - starting lives;
   - rim speed;
   - seed;
   - duration;
   - every existing frame/input/restart field.

2. Version-1 fixture still loads with:
   - Level 1;
   - 3 lives;
   - rim speed 1.0;
   - original seed/input behavior.

3. Version-1 unknown fields remain rejected.

4. Version-2 unknown fields are rejected.

5. Unsupported versions are rejected.

6. Invalid version-2 level is rejected.

7. Invalid version-2 lives are rejected.

8. Invalid/nonfinite/nonpositive version-2 rim speed is rejected.

9. Existing malformed-frame and noncontiguous-tick validation remains.

10. Save safety/non-overwrite behavior remains.


AUTOMATED TESTS — COMMAND LINE

Cover:

1. Default recording:
   --record-gameplay <file>

2. Recording with:
   --level 5 --lives 10

3. Recording with:
   --level 5 --lives 10 --rim-speed 2.5

4. Supported option order according to the chosen grammar.

5. Duplicate options rejected.

6. Missing values rejected.

7. Malformed values rejected.

8. Unsupported levels rejected.

9. Unknown extra arguments rejected.

10. Special test/scenario modes rejected with recording.

11. Playback restores metadata without CLI simulation options.

12. Playback plus --level rejected.

13. Playback plus --lives rejected.

14. Playback plus --rim-speed rejected.

15. Playback plus special mode rejected.

16. Renderer-only density behavior according to the chosen bounded policy.


AUTOMATED TESTS — DETERMINISM

Protect existing accepted tests and add:

1. Version-2 Level 1/default-start same-seed replay remains equivalent to the
   accepted existing behavior.

2. Version-2 direct Level 5, 10 lives, same seed and same recorded input produces
   equivalent selected game state.

3. Version-2 direct Level 7 uses accepted production Level 7 startup and route
   behavior.

4. Lives/deaths remain reproducible.

5. A recorded restart remains reproducible.

6. Natural Level 5-to-6 progression remains reproducible if exercised by the
   test.

7. Rim-speed metadata affects both recording and playback identically.

8. Game-over density does not alter deterministic game state if allowed as an
   observation option.

Compare meaningful simulation state rather than presentation-only state.

Do not add fragile assertions over incidental internal implementation details.


REGRESSION PROTECTION

Run neighboring regression covering:

- existing GameplayRecordingTests;
- existing GameplayPlaybackTests;
- direct-level startup;
- Level 5 $53 behavior;
- Level 7 movement/cargo;
- progression/campaign soak;
- death/re-entry;
- restart;
- Super Zapper;
- rim pursuit;
- input handling;
- normal live-input handoff.

The accepted Level 1 record/playback tests must remain intact.


MANUAL ACCEPTANCE COMMANDS

After automated validation, provide exact ready-to-copy commands based on the
implemented parser grammar.

At minimum provide commands for:

A. Normal Level 1 recording with defaults.

B. Playback of that Level 1 recording.

C. Direct Level 5 recording with 10 lives.

D. Playback of the Level 5 recording.

E. Direct Level 5 recording with explicit rim speed if supported.

F. An intentionally invalid playback override command demonstrating rejection.

Use practical filenames under an ignored/disposable path already appropriate to
the repository, or clearly note that generated recordings must not be committed.


MANUAL ACCEPTANCE EXPECTATIONS

Ask Neal to verify:

1. Normal Level 1 record/separate-playback remains visually as perfect as the
   previously accepted behavior.

2. A direct Level 5 recording starts at real production Level 5 with 10 lives.

3. Separate playback automatically starts at Level 5 with the same lives without
   requiring --level or --lives.

4. Movement, firing and Super Zapper timing reproduce the recording.

5. Deaths/restart reproduce if exercised.

6. Natural progression reproduces if the recording reaches Level 6.

7. Playback rejects simulation-affecting CLI overrides before opening gameplay.

8. Live controls resume normally after recorded input ends, if the recording
   reaches that boundary.

Do not require Neal to test every automated malformed-file case manually.


DOCUMENTATION

Update durable documentation to record the implementation candidate, but mark
manual acceptance as pending.

Document:

- recording format version 2;
- version-1 compatibility;
- self-describing production startup metadata;
- supported recorded startup values;
- recording command syntax;
- playback command syntax;
- rejection of playback simulation overrides;
- classification of game-over density;
- protected existing replay mechanics;
- generated recordings remain uncommitted/disposable unless deliberately
  preserved;
- future advanced playback controls remain deferred.

Do not claim the new version-2 round trip is manually accepted before Neal tests
it.


DEFERRED FEATURES

Do not implement:

- playback speed;
- pause;
- single-step;
- loop;
- start frame;
- end frame;
- seeking;
- snapshots;
- overlays;
- deterministic signatures;
- new headless replay runner;
- attract mode;
- playlist/browser UI.

Record them as deferred only if documentation already has an appropriate
roadmap location.

Do not expand this milestone.


AI LOGS

Preserve the complete task as:

AI-Logs/Prompts/2026-11-06-record-playback-startup-options-implementation-prompt.md

Create:

AI-Logs/Reports/2026-11-06-record-playback-startup-options-implementation-report.md

The report must contain:

1. Starting Git state.
2. Accepted baseline protected.
3. Canonical startup configuration design.
4. Exact version-2 schema.
5. Version-1 compatibility behavior.
6. CLI grammar.
7. Recording defaults.
8. Recording option composition.
9. Playback metadata restoration.
10. Playback override rejection.
11. Density policy.
12. Production initialization path reused.
13. RNG initialization ordering.
14. Restart semantics.
15. Input/frame semantics preservation.
16. Production files changed.
17. Test files changed.
18. Documentation changed.
19. Focused format/parser test counts.
20. Determinism test counts.
21. Neighboring regression counts.
22. Full-suite counts.
23. Build warnings/errors.
24. git diff --check result.
25. Manual commands.
26. Manual acceptance checklist.
27. Deferred features.
28. Final Git state.
29. Confirmation no staging, commit or push occurred.


AUTOMATED VALIDATION

Run focused format, parser, startup and deterministic replay tests.

Run neighboring record/playback and gameplay regressions.

Then run:

dotnet test TempestCSharp.slnx

dotnet build TempestCSharp.slnx

git diff --check

Tests must actually execute.

Require:

- 0 failed;
- 0 unexpected skipped;
- build 0 errors;
- report build warning count accurately;
- git diff --check passes.


NO INTERACTIVE GAME LAUNCH

Do not launch the interactive game.

Neal performs manual acceptance.

Do not stage.

Do not commit.

Do not push.


FINAL DIFF REVIEW

Before finishing run:

git status --short --branch
git rev-parse HEAD
git diff --stat
git diff --name-status
git diff
git diff --check
git ls-files --others --exclude-standard

Verify there is no:

- Level 8/9 implementation;
- gameplay retuning;
- advanced playback feature;
- generated recording committed;
- reference modification;
- Videos/TEMP content;
- bin/obj content;
- Git configuration change.


EXPECTED FINAL HEAD

HEAD must remain:

8222acee9baf6e893499a7d6ddb859bd1b7e0903


FINAL RESPONSE

Return:

1. Canonical startup configuration implemented.
2. Exact version-2 metadata.
3. Version-1 compatibility.
4. Record command syntax.
5. Supported record options.
6. Playback command syntax.
7. Playback override policy.
8. Density policy.
9. RNG ordering.
10. Restart semantics.
11. Input/frame preservation.
12. Production files changed.
13. Tests changed.
14. Documentation changed.
15. Focused format/parser counts.
16. Determinism counts.
17. Neighboring regression counts.
18. Full-suite counts.
19. Build warnings/errors.
20. git diff --check result.
21. Manual commands.
22. Manual acceptance checklist.
23. Report filename.
24. Deferred feature list.
25. Final Git status.
26. Confirmation no Level 8/9, advanced playback controls, staging, commit or
    push occurred.

Stop for Neal's manual acceptance.

TEMPEST_RECORD_PLAYBACK_STARTUP_OPTIONS_IMPLEMENTATION_COMPLETE