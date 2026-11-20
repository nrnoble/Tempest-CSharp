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


TEMPEST RECORD / PLAYBACK STARTUP-CONFIGURATION ANALYSIS

Perform a tightly bounded ANALYSIS-ONLY investigation of the accepted recorded
gameplay system and its interaction with command-line startup options.

Do not implement anything.

Do not modify production code.

Do not modify tests.

Do not modify durable documentation.

Do not alter the recording format.

Do not stage.

Do not commit.

Do not push.

Do not launch the interactive game.

The existing record/playback implementation has already received extremely
strong manual acceptance and must be treated as protected behavior.

The purpose of this investigation is to determine the smallest, safest design
that allows recorded gameplay to begin with simulation-affecting startup
options such as:

--level N

--lives N

and to establish a forward-looking mechanism for future simulation-affecting
startup options without repeatedly redesigning record/playback.


ACTIVE REPOSITORY

Work only in:

C:\Users\nrnob\source\repos\Tempest

Do not use or modify the frozen former OneDrive repository.


EXPECTED STARTING CHECKPOINT

Expected branch:

master

Expected HEAD:

8222acee9baf6e893499a7d6ddb859bd1b7e0903
Add direct level startup and controller quit

Expected:

- clean tracked worktree;
- clean index;
- no unexplained nonignored untracked files;
- master tracks origin/master;
- local master and origin/master identify the same commit.

Before investigation run:

git status --short --branch
git rev-parse HEAD
git log -1 --oneline
git rev-parse origin/master
git diff --stat
git diff --cached --stat
git ls-files --others --exclude-standard
git remote -v

If repository state differs unexpectedly, STOP without cleaning, restoring,
resetting or modifying anything.


AUTHORIZED WRITES

This task is analysis only.

The only authorized writes are the ignored paired AI records:

AI-Logs/Prompts/2026-11-06-record-playback-startup-options-analysis-prompt.md

AI-Logs/Reports/2026-11-06-record-playback-startup-options-analysis-report.md

Do not modify:

- production source;
- tests;
- README;
- TEMPEST-DEVELOPMENT-NOTES.md;
- TEMPEST-BEHAVIOR-SPEC.md;
- project instructions;
- recording files;
- replay files;
- Git configuration.


READ FIRST

Read:

.github/copilot-instructions.md

TEMPEST-DEVELOPMENT-NOTES.md

TEMPEST-BEHAVIOR-SPEC.md

Relevant historical reports and prompt records concerning:

- deterministic gameplay playback foundation;
- scenario-selectable playback;
- scenario-appropriate playback profiles;
- recorded player input;
- recorded-input round-trip acceptance;
- GameplayRecordingFile;
- PlayerInputRecorder;
- PlayerInputReplay;
- GameplayPlayback;
- DebugController;
- input diagnostics;
- command-line parsing;
- latest generic --level N direct startup;
- --lives handling;
- random/deterministic simulation setup.

Inspect current source/tests including as applicable:

Program.cs

DebugController.cs

GameplayRecordingFile.cs

PlayerInputRecorder.cs

PlayerInputReplay.cs

GameplayPlayback.cs

TempestGame.cs

InputSystem.cs

LevelProgression.cs

LevelConfiguration.cs

recording/playback tests

direct-level tests

input tests

Do not assume the architecture from historical reports. Verify current source.


PROTECTED ACCEPTED BEHAVIOR

The existing recorded-input round-trip mechanism is considered manually
excellent.

Preserve as a fundamental requirement:

A manually recorded gameplay session, when played back separately under the
same simulation conditions, should reproduce the original session
deterministically enough that Neal can concentrate on observing gameplay rather
than simultaneously playing it.

Do not recommend sacrificing current replay fidelity merely to make command-line
composition easier.


PRIMARY USER NEED

As later Tempest levels become faster and substantially more difficult, reaching
and recreating a particular gameplay condition repeatedly will become
increasingly expensive in manual play time.

The development process must not eventually stall because Neal must repeatedly
play many difficult levels merely to:

- observe a suspected defect;
- reproduce a rare condition;
- confirm a fix;
- compare enemy behavior;
- verify arcade fidelity;
- watch the same difficult sequence multiple times.

The desired long-term capability is:

play a difficult scenario successfully once
->
record the complete reproducible gameplay setup and input
->
replay it repeatedly for observation and regression work.


CURRENT IMMEDIATE REQUIREMENT

Investigate support for recording gameplay using existing startup options.

Desired examples include conceptually:

dotnet run --project src\Tempest.Game -- --record <file> --level 5 --lives 10

and eventually:

dotnet run --project src\Tempest.Game -- --record <file> --level 8 --lives 10

once future levels exist.

Do not assume exact argument order or syntax until current parsing is inspected.

The analysis must determine the safest actual CLI design.


CORE DESIGN QUESTION

Determine whether simulation-affecting startup configuration should become part
of the recording itself.

The preferred conceptual model to investigate is:

NORMAL STARTUP OPTIONS
->
CANONICAL GAMEPLAY STARTUP CONFIGURATION
->
GAME INITIALIZATION

and, when recording:

CANONICAL GAMEPLAY STARTUP CONFIGURATION
+
RECORDED INPUT
->
SELF-DESCRIBING RECORDING FILE

and, during playback:

RECORDING FILE
->
RESTORE RECORDED STARTUP CONFIGURATION
+
RECORDED INPUT
->
DETERMINISTIC REPRODUCTION

Do not implement this model.

Determine whether it fits the current architecture or whether a smaller seam is
preferable.


SIMULATION-AFFECTING VERSUS OBSERVATION-ONLY OPTIONS

Inventory every currently supported command-line option.

Classify each option into one of these categories:

A. SIMULATION-AFFECTING STARTUP STATE

An option belongs here if changing it can change the actual gameplay simulation
or the player inputs that would be appropriate.

Examples to investigate include:

--level

--lives

random seed if exposed or internally controlled

gameplay scenario selection if applicable

other startup options that alter gameplay state.


B. OBSERVATION / PRESENTATION / DIAGNOSTIC

An option belongs here if it can change how the same simulation is displayed,
observed, logged or diagnosed without changing deterministic gameplay state.

Possible examples must be verified from source rather than assumed.


C. RECORD/PLAYBACK TRANSPORT

Options that select:

- recording;
- playback;
- recording filenames;
- playback filenames;
- other recording mechanics.


D. INCOMPATIBLE / SPECIAL-MODE OPTIONS

Options whose architecture intentionally conflicts with recording or playback
and should remain mutually exclusive.

Do not force every option to compose merely because it exists.


OPTION-CLASSIFICATION RULE

Evaluate this proposed long-term rule:

If changing an option could change the gameplay state or the inputs the player
would need, the recording should preserve that option's effective simulation
value.

If an option changes only the observation/presentation of the same deterministic
simulation, it may remain a playback-time option rather than part of recorded
gameplay identity.

Report whether this rule fits the existing architecture and identify exceptions.


LEVEL OPTION

Investigate exactly how the newly accepted:

--level N

enters the production startup path.

Determine:

1. What effective startup state it creates.

2. Whether recording currently rejects or ignores it.

3. Whether the selected displayed level can be preserved in recording metadata.

4. Whether playback can restore exactly the same production-level
   initialization.

5. Whether natural onward progression remains deterministic.

6. Whether the direct-level startup uses any information not currently captured
   by the recording format.


LIVES OPTION

Investigate:

--lives N

Determine:

1. Where it is parsed.

2. Where it enters fresh-game initialization.

3. Whether recording currently rejects or ignores it.

4. Whether playback assumes a fixed/default lives value.

5. Whether the effective starting-lives value should be recorded.

6. Whether an unusual lives value affects deterministic playback after deaths,
   Game Over, or progression.


RANDOMNESS / SEED

Determine exactly how recording/playback currently controls randomness.

Identify:

- seed source;
- whether seed is serialized;
- whether replay reconstructs the same random state;
- whether direct-level startup affects random consumption;
- whether startup configuration must be restored before random initialization;
- whether future options could alter random-number consumption.

Do not change the random model.


RECORDING FILE FORMAT

Inspect the exact current recording format.

Report:

1. Current version identifier, if any.

2. Current metadata fields.

3. Input representation.

4. Random/deterministic metadata.

5. Startup-state metadata.

6. How files are validated.

7. How malformed or unsupported recordings are handled.

8. Whether unknown fields can be ignored safely.

9. Whether adding metadata requires a format-version change.

10. Whether existing recordings would remain readable.

11. Whether new recordings could remain readable by old builds, if relevant.

Do not modify the format.


BACKWARD COMPATIBILITY

Existing accepted recordings must not be casually broken.

Determine a concrete compatibility strategy.

Possible outcomes include:

A. New optional metadata can be added while old recordings retain existing
   default startup semantics.

B. Recording format should receive an explicit new version and playback should
   support both old and new versions.

C. Another existing mechanism already provides compatible extensibility.

D. The current format cannot safely evolve without a migration boundary.

Recommend the smallest safe choice.

Do not implement it yet.


PLAYBACK COMMAND-LINE OVERRIDES

Analyze whether simulation-affecting options should be accepted alongside
playback.

Example conflict:

recording says:

StartingLevel = 5
StartingLives = 10

but user invokes conceptually:

--playback <file> --level 7 --lives 3

Determine the safest policy.

Strongly consider:

SIMULATION-AFFECTING OPTIONS MUST COME FROM THE RECORDING DURING PLAYBACK.

If so, identify whether conflicting playback CLI arguments should:

- be rejected clearly;
- be ignored with a warning;
- be prohibited by parsing before startup.

Prefer deterministic safety over convenience.

Do not allow silent changes that could make playback appear valid while
reproducing a different simulation.


FUTURE STARTUP OPTIONS

The user specifically wants to avoid revisiting the recording architecture every
time another simulation-affecting startup option is added.

Determine whether a small canonical structure such as:

GameplayStartupConfiguration

or equivalent current-project naming would be beneficial.

Potential conceptual contents might eventually include:

- starting displayed level;
- starting lives;
- deterministic seed;
- future difficulty/settings that materially affect simulation.

Do not invent fields not currently required.

Do not introduce a large generic configuration framework.

The report should distinguish:

- useful small abstraction;
- premature overengineering.


CURRENT RECORD CLI COMPOSITION

Determine exactly why the current record command cannot compose with options
such as --level and --lives.

Identify whether the limitation comes from:

- parser organization;
- early mutually exclusive mode selection;
- alternate construction path;
- fixed recorder initialization;
- recording-file schema;
- playback assumptions;
- another specific design.

Provide exact code locations.


CURRENT PLAYBACK CLI COMPOSITION

Determine what options, if any, currently compose with playback.

Do not assume none do.

Identify what is currently legal, rejected, ignored, or semantically dangerous.


LONG-TERM PLAYBACK / OBSERVATION CAPABILITIES

The following capabilities are considered potentially valuable in the future as
higher-level gameplay becomes harder:

1. playback speed;

2. pause;

3. single-step / frame-step;

4. automatic replay loop;

5. playback start frame;

6. playback end frame;

7. observation overlays;

8. deterministic-state verification;

9. headless playback verification.

DO NOT IMPLEMENT ANY OF THESE.

Analyze them only enough to answer:

- whether the existing architecture would naturally support each one;
- whether each affects simulation identity or only observation;
- whether implementing it later would require recording-format changes;
- whether any architectural choice made now for startup metadata should avoid
  making these harder later.

Do not turn this analysis into a large replay-system redesign.


PLAYBACK SPEED

Determine conceptually whether playback speed can be implemented later as an
observation/presentation pacing change while preserving the underlying fixed
simulation step.

Do not propose changing simulation tick duration just to make playback faster.


PAUSE / SINGLE-STEP

Determine whether recorded input is already indexed sufficiently by simulation
tick/frame to permit:

- pause without advancing simulation;
- advance exactly one simulation step.

Do not implement controls.


AUTOMATIC LOOP

Determine what simulation reset state would have to be restored to replay the
same recording repeatedly without accumulating state.

Do not implement looping.


START / END FRAME

Analyze whether beginning playback at an arbitrary recorded frame would require
a full deterministic replay from the beginning or serialized state snapshots.

Do not assume random seeking is cheap.

Distinguish:

- observation trimming;
- true arbitrary seeking;
- replaying from frame 0 while suppressing display until a target frame.

No implementation.


OBSERVATION OVERLAYS

Identify possible future diagnostic overlays only at the architectural level.

Examples may include deterministic information already available in the
simulation such as:

- displayed level;
- simulation tick;
- enemy route/program information;
- Spike state;
- Flipper state;
- RNG state/hash;
- recording frame.

Do not design a UI and do not add overlays now.


DETERMINISTIC-STATE VERIFICATION

Determine whether playback could later compare periodic or final deterministic
state signatures against values captured during recording.

Analyze possible scope:

- game-state hash;
- score/lives/level;
- enemy state;
- RNG state;
- final-state assertion.

Do not implement hashing or change the recording format now.


HEADLESS VERIFICATION

Determine whether the recorded-input mechanism could eventually feed the normal
production simulation without Raylib presentation for automated deterministic
regression.

Relate this to the project's existing four-layer validation strategy.

Do not implement a new headless runner.


HIGHER-LEVEL TESTING WORKFLOW

Evaluate this intended future workflow:

1. Pre-analyze a difficult production level.

2. Implement the level.

3. Direct-launch it with --level N.

4. Neal manually plays the difficult scenario successfully.

5. Record that gameplay together with all simulation-affecting startup state.

6. Replay it visibly as many times as necessary for detailed observation.

7. Use future pause/single-step/speed/loop tools if and when those are separately
   implemented.

8. Potentially reuse the same recording for deterministic/headless regression.

9. Manual play remains the final arcade-fidelity authority.

The objective is to prevent higher-level complexity from making verification
prohibitively time-consuming.


NO IMPLEMENTATION

Do not:

- modify recording format;
- change playback behavior;
- change parser behavior;
- add --level to recording;
- add --lives to recording;
- add playback speed;
- add pause;
- add frame-step;
- add looping;
- add seeking;
- add overlays;
- add hashes;
- add headless playback;
- modify gameplay;
- modify tests;
- update durable documentation;
- stage;
- commit;
- push.

This task is analysis only.


TEST / BUILD POLICY

Do not run the interactive game.

Because this is read-only analysis with no production/test changes, a full build
or test run is not required unless needed to answer a concrete architecture
question.

If any automated command is run, explain why.

Do not manufacture validation activity merely to produce test counts.


ANALYSIS VALIDATION

Before concluding:

1. Trace normal startup.

2. Trace --level startup.

3. Trace --lives startup.

4. Trace record startup.

5. Trace playback startup.

6. Trace random/seed initialization.

7. Trace recording serialization.

8. Trace playback deserialization.

9. Verify current exclusivity rules from actual parser code.

10. Verify old recording compatibility requirements from actual format code and
    tests.

11. Identify every current startup value that materially affects deterministic
    reproduction.

12. Check conclusions against currently accepted round-trip recording tests.

13. Ensure no recommendation degrades the existing manually accepted replay.


REPORT

Create:

AI-Logs/Reports/2026-11-06-record-playback-startup-options-analysis-report.md

The report must contain:

1. Starting Git state.

2. Accepted record/playback baseline.

3. Files/source paths examined.

4. Current CLI architecture.

5. Current normal-start path.

6. Current --level path.

7. Current --lives path.

8. Current record path.

9. Current playback path.

10. Current randomness/seed path.

11. Current recording format and version behavior.

12. Current incompatibility matrix for command-line options.

13. Exact reason record cannot currently compose with --level / --lives.

14. Classification of every current CLI option.

15. Minimum state required for self-describing deterministic recordings.

16. Recommended recording metadata.

17. Recommended format/version strategy.

18. Backward-compatibility strategy.

19. Recommended record CLI semantics.

20. Recommended playback CLI semantics.

21. Conflict behavior for playback plus simulation-affecting overrides.

22. Whether a canonical startup-configuration type is warranted.

23. Smallest implementation boundary.

24. Regression risks.

25. Required automated tests for later implementation.

26. Required manual acceptance tests for later implementation.

27. Long-term playback feature compatibility analysis:
    - speed;
    - pause;
    - single-step;
    - loop;
    - start/end frame;
    - overlays;
    - deterministic verification;
    - headless verification.

28. Recommended ordering of those future enhancements, without implementing
    them.

29. Higher-level testing workflow implications.

30. Files written.

31. Final Git state.

32. Explicit confirmation that no production, tests, durable documentation,
    recording files, Git history, or GitHub state were modified.


AI PROMPT RECORD

Preserve this complete task as:

AI-Logs/Prompts/2026-11-06-record-playback-startup-options-analysis-prompt.md


FINAL GIT CHECK

Finish with:

git status --short --branch
git rev-parse HEAD
git rev-parse origin/master
git diff --stat
git diff --cached --stat
git ls-files --others --exclude-standard

Expected HEAD remains:

8222acee9baf6e893499a7d6ddb859bd1b7e0903

Expected:

- clean tracked worktree/index;
- local master still matches origin/master;
- only ignored prompt/report records were written.


FINAL RESPONSE

Return a concise technical summary containing:

1. Why recording currently cannot compose with --level / --lives.

2. Current recording metadata.

3. Current randomness/seed handling.

4. Minimum additional metadata required.

5. Recommended canonical startup configuration, if warranted.

6. Recommended record command semantics.

7. Recommended playback command semantics.

8. Backward-compatibility approach.

9. Whether new format versioning is required.

10. How conflicting playback CLI options should be handled.

11. Which current options are simulation-affecting.

12. Which current options are observation/debug-only.

13. Whether future playback speed is architecturally straightforward.

14. Whether pause/single-step is architecturally straightforward.

15. Loop implications.

16. Start/end-frame implications.

17. Observation-overlay implications.

18. Deterministic-state-verification implications.

19. Headless-verification implications.

20. Smallest safe implementation milestone.

21. Proposed later implementation test requirements.

22. Report filename.

23. Final Git status.

24. Confirmation no implementation, staging, commit or push occurred.

Stop for Neal's review before any recording/playback modification.

TEMPEST_RECORD_PLAYBACK_STARTUP_OPTIONS_ANALYSIS_COMPLETE