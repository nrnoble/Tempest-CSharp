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


TEMPEST LEVEL 5 POST-DEATH UNEXPECTED EXIT ROOT-CAUSE INVESTIGATION

Investigate a severe reproducible unexpected application exit associated with
Level 5 death/re-entry.

This is primarily an INVESTIGATION / ROOT-CAUSE task.

Do not begin with a speculative gameplay fix.

First determine exactly WHY the application terminates.

If a specific defect is conclusively established and a very narrow correction
is clearly justified, report the proposed fix but STOP for Neal's review before
implementing it unless this task explicitly authorizes implementation later.

Do not stage.

Do not commit.

Do not push.

Do not begin Level 8 or Level 9 work.


ACTIVE REPOSITORY

Work only in:

C:\Users\nrnob\source\repos\Tempest

Do not use the frozen former OneDrive repository.


EXPECTED CLEAN BASELINE

Expected branch:

master

Expected HEAD:

11ddd630a3c6e01c18795658c9625280ca24ca8e
Extend recorded gameplay startup metadata

Expected:

- tracked worktree clean;
- index clean;
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

If repository state differs unexpectedly, STOP without cleaning, resetting,
restoring, deleting or discarding anything.


READ FIRST

Read:

.github/copilot-instructions.md

TEMPEST-DEVELOPMENT-NOTES.md

TEMPEST-BEHAVIOR-SPEC.md

Read the reports concerning:

- accepted Level 5 implementation;
- Level 5 $53 Flipper/Spike correction;
- Level 5 acceptance closeout;
- generic direct-level / Enter quit implementation;
- generic direct-level / Enter quit acceptance closeout;
- record/playback version-2 implementation;
- record/playback version-2 acceptance closeout;
- death/re-entry reconstruction;
- Level 5 progression;
- input/controller behavior.

Inspect relevant current production source and tests including:

Program.cs

TempestGame.cs

InputSystem.cs

DebugController.cs

LevelConfiguration.cs

LevelProgression.cs

Level 5 production tests

death/re-entry tests

input tests

direct-level tests

and any other code involved in:

- main-loop termination;
- player death;
- player re-entry;
- Level 5 reconstruction;
- application shutdown;
- Raylib window lifecycle;
- audio lifecycle;
- recording/playback lifecycle.

Do not assume the root cause from historical reports.


OBSERVED BUG

Treat the following as Neal's direct manual observations.

OBSERVED / SEVERE / REPRODUCIBLE / LEVEL-5-SPECIFIC SO FAR / ROOT CAUSE UNKNOWN.

Earlier broad observation:

After active Level 5 play and a Blaster death, the death/re-entry sequence
appears normal, but within a few seconds after gameplay resumes the application
may exit unexpectedly.

The console ends with:

INFO: Window closed successfully

No crash exception or other error has been visible in the console.

The bug reproduces without recording/playback.


REFINED CONTROL VERSUS FAILURE OBSERVATION

The newest observation materially narrows the issue.

CONTROL CASE:

1. Start Level 5 fresh.

2. Provide NO gameplay input.

3. Simply allow the stationary Blaster to be destroyed normally.

4. Allow death/re-entry to complete.

5. Continue providing no gameplay input.

6. This may be repeated for multiple Blaster deaths.

OBSERVED RESULT:

The game continues normally.

The unexpected exit does NOT occur merely because the Blaster dies on Level 5.


FAILURE CASE:

1. Start Level 5 fresh.

2. Neal takes active control and actually plays Level 5.

3. After some active gameplay, the Blaster is destroyed.

4. Death/re-entry appears to complete normally.

5. After gameplay resumes, the game unexpectedly exits within a few seconds.

6. No keyboard/controller input is required AFTER re-entry for the unexpected
   exit to occur.

OBSERVED RESULT:

The unexpected exit is reproducible after active Level 5 play followed by death
and re-entry.


ADDITIONAL OBSERVATIONS

- Level 5 with no Blaster death can proceed normally to Level 6.

- A Blaster death occurring on Levels 1–4 does NOT cause the problem merely
  because the campaign later reaches Level 5.

- The relevant trigger requires state changed during Level 5 itself.

- Other tested levels have not reproduced the same defect so far.

- The defect occurs in ordinary gameplay without record/playback.

- The defect can occur with no controller or keyboard input after re-entry.

Therefore:

Do NOT begin by blaming:

- recording/playback;
- controller hardware;
- keyboard hardware;
- Enter;
- V;
- ordinary Level 5 initialization;
- death/re-entry generally.

None of those causes is established.


PRIMARY QUESTION

Determine exactly what causes the application process/main loop to terminate.

DO NOT assume that:

INFO: Window closed successfully

means the initiating cause was a legitimate window-close request.

That message may only establish that cleanup completed successfully.


APPLICATION TERMINATION AUDIT

Trace EVERY production path by which the running application can leave its main
loop or terminate.

At minimum inspect:

1. Raylib WindowShouldClose().

2. Enter/Return immediate quit handling.

3. InputSystem.IsQuitRequested or equivalent.

4. Escape/window-close handling.

5. Any application/game quit flag.

6. Any main-loop condition.

7. Any break statement that can leave the loop.

8. Any return that can leave Program/Main or the gameplay loop.

9. Environment.Exit or process-exit calls.

10. Cancellation or shutdown tokens.

11. Game-state-derived terminal conditions.

12. Recording/playback completion paths.

13. Audio/window cleanup paths.

14. finally blocks.

15. catch blocks.

16. top-level exception handling.

17. callbacks/event handlers capable of triggering shutdown.

18. secondary tasks/threads whose failure could terminate or influence the main
    process.

Produce a complete termination-path inventory.


EXCEPTION-HANDLING AUDIT

Neal is concerned that errors may currently be caught or otherwise obscured and
then followed by normal Raylib cleanup, leaving only:

INFO: Window closed successfully

Determine whether that concern is true.

Inspect all relevant:

try
catch
finally

blocks surrounding:

- startup;
- simulation;
- rendering;
- input;
- audio;
- recording;
- playback;
- main loop;
- cleanup.

Determine:

1. Can a gameplay/render/input exception be caught without a complete exception
   being shown?

2. Is any catch block empty?

3. Does any catch block log only a short message?

4. Can finally execute CloseWindow after an exception?

5. Could Raylib then issue "Window closed successfully" even though an exception
   initiated shutdown?

6. Does a fatal runtime exception currently produce a nonzero process exit code?

7. Are full exception type, message, stack trace and inner exception visible?

8. Could an unobserved Task/thread exception cause shutdown?

Do not invent answers.

Trace actual code.


EXIT-REASON OBSERVABILITY

If current source does not unambiguously expose why the main loop terminates,
design the smallest DEVELOPMENT-ONLY diagnostic necessary to distinguish exit
paths.

Conceptual examples:

Exit reason: WindowShouldClose

Exit reason: EnterQuit

Exit reason: GameTerminalState

Exit reason: MainLoopReturned

Exit reason: FatalException

These names are examples only.

Use names appropriate to actual discovered paths.

If instrumentation is necessary, it may be implemented narrowly in this
investigation, but:

- it must not change gameplay;
- it must not change which exit path is chosen;
- it must not swallow exceptions;
- it must not convert failures into successful exits;
- it must not persist in release-facing gameplay unless clearly justified.

For an actual fatal exception, ensure investigation output captures:

- exception type;
- message;
- stack trace;
- inner exception where available;
- process exit status.

Do not broadly redesign logging.


NORMAL VERSUS UNEXPECTED EXIT CODE

Determine and document process exit codes for:

- normal Enter quit;
- normal window close if practical to establish without interactive automation;
- invalid command-line startup;
- fatal exception path if one exists;
- Level 5 unexpected-exit reproduction if it can be captured deterministically.

Do not claim equivalence merely because all paths call CloseWindow.


LEVEL 5 STATE-DIFFERENCE INVESTIGATION

The control/failure split strongly suggests that some state mutated during
active Level 5 gameplay matters.

Treat this only as an investigative hypothesis.

Compare the clean no-input death case against the active-play death case.

Inventory Level 5 state that active gameplay can mutate before death, including
where relevant:

- Player state;
- player lane/position;
- player shots;
- active enemy identities;
- reserve enemies;
- Eggs;
- Flippers;
- Level 5 $53 Spike-sensitive Flipper route state;
- Spike heights/damage;
- Tankers;
- Tanker cargo;
- Spikers;
- enemy shots;
- Super Zapper state;
- score;
- random state;
- level wave/identity ledger;
- pending activations;
- destruction bookkeeping;
- rim enemies;
- death reconstruction;
- any Level 5-specific route/policy state.

Do not assume all of these are involved.

The goal is to identify the MINIMUM state difference associated with the
failure.


REDUCE “ACTIVE PLAY” TO A MINIMAL TRIGGER

Determine whether automated or deterministic tests can progressively narrow the
required pre-death action.

Candidate reductions to investigate, not assume:

A. Move Blaster only, then die.

B. Fire only, then die.

C. Move + Fire, then die.

D. Use Super Zapper, then die.

E. Destroy one ordinary enemy, then die.

F. Damage one Spike, then die.

G. Destroy one Spike if possible, then die.

H. Destroy a Tanker, then die.

I. Create active Tanker cargo, then die.

J. Destroy a Spiker, then die.

K. Allow an enemy to reach the rim, then die.

L. Change score only through an enemy destruction, then die.

M. Create one or more enemy bullets, then die.

Do not manually launch the game to try every case.

Prefer existing simulation/test seams and deterministic construction.

The purpose is delta-debugging of game state, not exhaustive random testing.


LEVEL 5-SPECIFIC DIFFERENCES

Compare Level 5 death/re-entry against adjacent accepted levels, especially
Level 4 and Level 6.

Identify every relevant behavior that differs, including:

- Level 5 $53 Flipper/Spike policy;
- starting Spike configuration;
- enemy eligibility;
- enemy supply;
- Tanker/cargo behavior;
- Spiker behavior;
- firing parameters;
- death reconstruction;
- wave ledger;
- progression bookkeeping.

Do not conclude that $53 is responsible simply because it is Level-5-specific.


DEATH / RE-ENTRY TRACE

Trace the complete production Level 5 death/re-entry path.

Determine for each mutable entity/state:

- what survives death;
- what is destroyed;
- what returns to reserve;
- what is reconstructed;
- what is reset;
- what is intentionally preserved;
- what timers/counters survive;
- what references/identities survive;
- what callbacks/state machines resume after re-entry.

Compare this against Levels 4 and 6 where practical.


RECORDING / PLAYBACK AS A DIAGNOSTIC AID

The newly accepted recording/playback v2 system is NOT suspected as the cause.

However, it may be useful as a diagnostic reproduction tool.

Analyze whether a recording that captures:

active Level 5 play
-> death
-> re-entry
-> subsequent unexpected exit

could help reproduce the exact input/state path repeatedly.

Do not modify recording/playback.

Do not blame replay if the live path reproduces independently.

If the application exits before the recording is saved under current semantics,
note that limitation rather than modifying save behavior during this task.


AUTOMATED REPRODUCTION

Attempt to construct a deterministic automated reproduction using the real
production simulation if practical.

The ideal automated test distinguishes:

CONTROL:
fresh Level 5 -> no meaningful player interaction -> death -> re-entry ->
continued simulation DOES NOT request/produce application exit.

FAILURE:
fresh Level 5 -> minimal identified active-play state change -> death ->
re-entry -> subsequent simulation DOES trigger the problematic termination
condition or corrupt state that leads to it.

Do not create a test that merely asserts a guessed root cause.

If the actual application-close signal exists only in Program/Raylib and cannot
be represented in headless TempestGame tests, introduce only the narrow seam
required for diagnostic verification, if justified.

If automated reproduction is not possible yet, state exactly why.


RAYLIB CLEANUP MESSAGE

Specifically determine where:

INFO: Window closed successfully

comes from.

Establish whether it is:

- emitted by Raylib during CloseWindow;
- emitted by project code;
- conditional on clean loop termination;
- unconditional once cleanup is invoked.

Do not use the existence of this message as evidence that gameplay code did not
fail.


NO SPECULATIVE GAMEPLAY FIX

Do not make changes such as:

- clearing all Level 5 state after death;
- disabling $53;
- removing Spikes;
- suppressing enemies;
- resetting input aggressively;
- ignoring exceptions;
- preventing shutdown;
- removing Enter quit;
- special-casing Level 5;

merely to make the symptom disappear.

Root cause comes first.


ALLOWED CHANGES IN THIS INVESTIGATION

This is primarily analysis.

Allowed production changes are limited to small reversible DEVELOPMENT-ONLY
diagnostic seams if necessary to identify:

- exit reason;
- exception path;
- relevant state transition.

Allowed tests may be added if they establish the reproduction or prove a
root-cause hypothesis.

Do NOT fix the root cause unless separately authorized after Neal reviews the
investigation result.

Do not stage.

Do not commit.

Do not push.


TESTING

No interactive launch by Copilot.

Use headless tests and deterministic simulation.

If diagnostic code is added, test that it does not change normal gameplay
behavior.

Run targeted tests necessary to validate the investigation.

If production/test code changes during diagnosis, also run:

dotnet test TempestCSharp.slnx

dotnet build TempestCSharp.slnx

git diff --check

Report actual counts.

If no tracked changes are required, do not manufacture unnecessary full-suite
validation.


DOCUMENTATION POLICY

Do not rewrite durable behavior specifications with an unproven root cause.

The investigation report is the primary record for hypotheses and findings.

Only update durable documentation if a tracked diagnostic change requires brief
documentation.

Do not mark a fix as accepted.


AI LOGS

Preserve this complete task as:

AI-Logs/Prompts/2026-11-06-level5-post-death-exit-root-cause-prompt.md

Create:

AI-Logs/Reports/2026-11-06-level5-post-death-exit-root-cause-report.md

The report must contain:

1. Starting Git state.

2. Complete known manual reproduction evidence.

3. Control-case reproduction.

4. Failure-case reproduction.

5. Main-loop termination-path inventory.

6. Exception-handling audit.

7. Meaning of "INFO: Window closed successfully".

8. Normal exit behavior.

9. Unexpected exit behavior.

10. Exit-code findings.

11. Whether hidden/swallowed exceptions are possible.

12. Any exit-reason instrumentation added.

13. Level 5 mutable-state inventory.

14. Level 5 death/re-entry trace.

15. Level 4/5/6 comparison.

16. Active-play reduction experiments.

17. Minimum trigger found, if established.

18. Automated reproduction, if achieved.

19. Root cause, if proven.

20. Competing hypotheses if root cause is not proven.

21. Evidence for and against each remaining hypothesis.

22. Exact narrow fix recommended, if root cause is proven.

23. Regression risks of the recommended fix.

24. Required automated tests for a later fix.

25. Required manual acceptance scenarios for a later fix.

26. Recording/playback diagnostic usefulness/limitations.

27. Production files changed, if any.

28. Tests changed, if any.

29. Validation performed.

30. Final Git state.

31. Explicit confirmation no fix, staging, commit, push, Level 8/9 work, or
    unrelated gameplay change occurred.


FINAL GIT CHECK

Finish with:

git status --short --branch
git rev-parse HEAD
git rev-parse origin/master
git diff --stat
git diff --name-status
git diff --check
git diff --cached --stat
git ls-files --others --exclude-standard

Expected HEAD remains:

11ddd630a3c6e01c18795658c9625280ca24ca8e


FINAL RESPONSE

Return:

1. Whether the Level 5 exit was reproduced automatically.

2. Exact minimum trigger established, if any.

3. Exact main-loop termination mechanism.

4. Whether an exception is involved.

5. Whether exceptions can currently be hidden behind normal cleanup.

6. Meaning of the Raylib successful-close message.

7. Exit-code findings.

8. Level 5 state difference associated with the failure.

9. Death/re-entry state finding.

10. Level 4/6 comparison.

11. Root cause and evidence classification.

12. If root cause is not proven, remaining hypotheses ranked by evidence,
    explicitly labeled hypotheses rather than facts.

13. Recommended narrow correction, if evidence supports one.

14. Whether temporary diagnostic code was added.

15. Production files changed.

16. Test files changed.

17. Targeted/full validation results.

18. Report filename.

19. Final Git status.

20. Confirmation no fix, stage, commit, push, Level 8/9 work, or unrelated
    gameplay change occurred.

Stop for Neal's review before implementing a root-cause fix.

TEMPEST_LEVEL5_POST_DEATH_EXIT_ROOT_CAUSE_COMPLETE