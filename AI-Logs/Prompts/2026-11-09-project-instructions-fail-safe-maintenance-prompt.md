FAIL-CLOSED PROMPT CHECK

Read this entire user-provided task before doing anything.

Before using tools, reading files, running commands, modifying files, or taking
any repository action, locate the unique completion token near the end of this
user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a
line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment
metadata, automatically supplied workspace context, or other agent-generated
material may appear after the user's token and do not invalidate the task.

This check is intended to detect an incomplete or truncated USER task, not
appended agent context.

If exactly one complete TEMPEST_ token cannot be found, stop before any project
or repository action and explain why.


TASK

Perform a small standalone Tempest project-instructions maintenance milestone.

Add durable repository-wide guidance to:

.github/copilot-instructions.md

for:

1. Windows Application Control / Smart App Control security blocks;
2. stalled, hung, or no-progress commands;
3. mandatory bounds/timeouts on potentially long-running operations;
4. voluntary agent self-stop and precise handoff when required work cannot
   safely continue.

The purpose is to make these recurring safety/workflow rules part of the
project-wide Copilot instructions so future substantial prompts do not need to
repeat them individually.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected accepted baseline:

c45ac28096a13ed03cf26827c2df367179c60553
Implement accepted Level 2 four-Flipper maximum

Confirm actual:

- branch;
- HEAD;
- origin/master;
- tracking state;
- index;
- tracked worktree;
- untracked state;

before modifying anything.

This is a PROJECT-INSTRUCTIONS-ONLY maintenance milestone.

Do not modify production code.

Do not modify tests.

Do not modify README.md.

Do not modify TEMPEST-DEVELOPMENT-NOTES.md.

Do not modify TEMPEST-BEHAVIOR-SPEC.md.

Do not modify reference files.

Do not modify ROM files.

Do not begin gameplay work.

Do not begin Level 3 concurrency.

Do not begin Level 9.


PRIMARY FILE

Read the complete existing:

.github/copilot-instructions.md

before editing it.

Understand its current organization, tone, terminology, and established
workflow rules.

Integrate the new policy cleanly into the existing instructions rather than
blindly appending repetitive or contradictory text.

Preserve every existing instruction unless a tiny wording adjustment is
strictly necessary to avoid contradiction with the new policy.


WHY THIS POLICY IS NEEDED

Tempest development has encountered several recurring agent/infrastructure
failures:

- Windows Application Control / Smart App Control has intermittently blocked
  newly generated Tempest.Game.dll, Tempest.Game.Tests.dll, and disposable
  probe DLLs.
- Repeating or regenerating the same newly blocked artifact is not a useful or
  acceptable recovery strategy.
- Neal does not want Windows security weakened merely to develop Tempest.
- A temporary Level2ConcurrencyProbe.dll was blocked during a read-only
  investigation.
- Some generated builds have later become runnable without changing security,
  so an immediate block is not proof of a production-code defect.
- An earlier Level 1 production promotion exposed an unbounded test helper that
  caused a VSTest/testhost run to remain stalled for hours.
- Neal had to manually discover and stop that stalled test process.
- Subsequent bounded isolation using VSTest hang timeouts successfully located
  the exact stale test assumption.
- Agents should not sit indefinitely waiting for Neal to notice and manually
  force-stop them.

The desired project behavior is therefore:

FAIL SAFE, PRESERVE THE WORKTREE, AND HAND CONTROL BACK TO NEAL.

Do not attempt to make every possible validation succeed at any cost.


WINDOWS SECURITY BLOCK POLICY

Add durable instructions establishing the following policy.

If Windows Application Control, Smart App Control, Code Integrity, or another
Windows security mechanism blocks a generated:

- DLL;
- EXE;
- test assembly;
- game assembly;
- analysis probe;
- temporary executable;
- command that depends on such an artifact;

the agent must:

1. Treat the security block as a security/infrastructure result, not a normal
   test failure.

2. Record the exact blocked:
   - command;
   - artifact;
   - filesystem path;
   - error/message;
   - relevant error code when available.

3. Never:
   - disable Windows security;
   - weaken Windows security;
   - change Smart App Control settings;
   - change Application Control policy;
   - change Defender policy;
   - change Code Integrity policy;
   - attempt a security bypass;
   - instruct Neal to weaken security merely to complete the task.

4. Never enter a retry loop that:
   - repeatedly rebuilds the same source into effectively the same artifact;
   - repeatedly runs the same blocked artifact;
   - repeatedly creates equivalent disposable probe DLLs/EXEs hoping one will
     be accepted.

5. Retry only when there is a MATERIALLY DIFFERENT SAFE ROUTE.

Examples of materially different safe routes may include, where appropriate:

- source/static analysis;
- an existing already-established test seam;
- an existing current headless API;
- an existing already-authorized repository tool;
- a narrower existing test assembly;
- another route that does not circumvent security.

Do not enumerate these examples as guarantees that they will always work.

6. Never substitute stale binary results for the current candidate.

7. Never run old binaries and claim they validate newly edited source.

8. Never report a security-blocked command as:
   - passed;
   - validated;
   - successful;
   - equivalent to a successful current-candidate test.

9. If all required validation can still be obtained through legitimate
   materially different safe routes, continue.

10. If required validation cannot be obtained safely:

    STOP VOLUNTARILY.

11. Preserve the current worktree exactly.

12. Do not reset, restore, discard, clean, or otherwise destroy correct
    candidate work merely because validation was blocked.

13. Report status explicitly as:

    BLOCKED

    or:

    PARTIAL

    as appropriate.

14. Report:
    - work completed;
    - work not completed;
    - validation completed;
    - validation blocked;
    - remaining validation;
    - exact security blocker;
    - exact next handoff Neal needs.

15. Do not wait indefinitely for Neal to discover the block and force-stop the
    agent.


LONG-RUNNING OPERATIONS POLICY

Add durable instructions requiring explicit bounds for potentially long-running
operations.

This applies especially to:

- tests;
- VSTest;
- simulation loops;
- campaign/headless simulation;
- state-search loops;
- analysis probes;
- scripts scanning simulation boundaries;
- process waits;
- polling loops;
- repeated command loops;
- any operation whose completion depends on reaching a gameplay state.

Use an explicit appropriate bound such as:

- VSTest --blame-hang-timeout;
- wall-clock/process timeout;
- simulation tick maximum;
- iteration maximum;
- bounded number of seeds;
- bounded number of levels;
- bounded polling attempts;
- another clearly defined termination condition.

The exact bound should be proportional to the task.

Do not impose one arbitrary universal timeout on every command.


TEST LOOP POLICY

Any test/helper that waits for gameplay state must have an explicit hard bound.

Prohibit patterns conceptually equivalent to:

while (!desiredState)
{
    RunOneStep();
}

unless there is also an explicit maximum iteration/tick bound.

On bound exhaustion, fail with useful diagnostic state when practical.

Useful state may include, as relevant:

- current tick/boundary;
- gameplay state;
- displayed/current level;
- active enemy count;
- active Flipper count;
- Egg counts by lifecycle state;
- reserve count;
- unresolved count;
- current player state;
- another state directly relevant to the expected transition.

Do not require irrelevant giant state dumps.

The purpose is to make a stale test expectation fail clearly rather than hang.


STALL / NO-PROGRESS POLICY

Add durable guidance for commands that stop making meaningful progress.

If an operation has made no meaningful progress beyond its reasonable expected
duration:

1. Stop the operation.

2. Do not immediately rerun the same unrestricted operation.

3. Inspect available output/process state.

4. Isolate the problem using progressively smaller bounded runs where
   appropriate.

For example, test diagnosis may proceed from:

- full suite;
- affected namespace/class;
- smaller filter;
- individual test/theory row;

with explicit hang protection.

Do not prescribe this exact hierarchy when another bounded isolation method is
more suitable.

5. Determine, when practical, whether the failure is:

- stale test assumption;
- unbounded helper loop;
- genuine production deadlock;
- Windows security block;
- external infrastructure/tool failure;
- another specifically identified problem.

6. Correct a stale or unbounded test only when the expected production
   behavior is established.

7. Do not weaken/remove a valid test merely because the test exposed a real
   production defect.

8. Do not change production behavior merely to satisfy a stale test assumption.

9. After isolating and correcting the exact problem, rerun:
   - the isolated case;
   - affected bounded regressions;
   - broader validation only after the smaller case is proven healthy.

10. If safe bounded recovery is not possible:

    STOP VOLUNTARILY.

11. Preserve all candidate work.

12. Provide Neal a concise blocker report and recovery recommendation.


NO-FALSE-PASS RULE

Add an explicit invariant:

A command that is:

- canceled;
- timed out;
- hung;
- stalled;
- security-blocked;
- manually terminated;
- killed;
- unable to load the required current assembly;
- otherwise incomplete;

must NEVER be reported as passed.

Distinguish:

PASS

FAIL

BLOCKED

TIMEOUT

CANCELED

PARTIAL

where relevant.

Do not combine incomplete runs into a statement such as "validation passed"
unless every validation required for that claim actually completed
successfully.


VOLUNTARY SELF-STOP POLICY

Add a durable project-wide self-stop requirement.

If any required task step cannot be completed safely or reliably, the agent
must prefer stopping voluntarily rather than continuing indefinitely.

The agent should:

- preserve the candidate;
- preserve unrelated local files;
- preserve Git state unless staging/commit was already explicitly authorized
  and reached safely;
- avoid pretending the task finished;
- create the expected AI report when possible;
- mark the report BLOCKED or PARTIAL;
- explain the blocker;
- explain what completed;
- explain what remains;
- identify Neal's exact next action.

The agent must not require Neal to notice an unresponsive process hours later
and manually force-stop it before receiving a useful explanation.


STAGING / COMMIT SAFETY WHEN VALIDATION IS INCOMPLETE

Add guidance that applies to prompts which authorize staging/commit/push.

If required final validation is:

- blocked;
- stalled;
- timed out;
- canceled;
- incomplete;

then the agent must not stage/commit/push unless the user prompt explicitly
authorizes a partial checkpoint under those conditions.

For normal Tempest milestones, incomplete required validation means:

STOP BEFORE COMMIT.

Preserve the worktree and report the blocker.


MANUAL GAME POLICY

Preserve the existing rule that VS Code Copilot does not launch the interactive
game for Neal's manual acceptance.

Do not change that policy.

The new self-stop policy complements it:

- automated validation should finish or stop with a useful handoff;
- Neal performs interactive gameplay afterward when appropriate.


RELATION TO INDIVIDUAL PROMPTS

Add guidance making these rules the project-wide default.

Future user prompts do not need to repeat the full:

- Windows-security block policy;
- stall/no-progress policy;
- timeout policy;
- voluntary-stop policy;

unless a task requires a special exception or tighter bound.

A future prompt may simply rely on the established project instructions.

Task-specific prompts still define:

- implementation scope;
- specific required tests;
- manual acceptance criteria;
- Git authorization;
- evidence requirements.

Project instructions define the recurring failure-handling behavior.


MAINTAINABILITY

Keep the new instruction section concise enough to remain useful in every
Copilot request.

Do not simply paste the entire previous Level 2 prompt into
copilot-instructions.md.

Consolidate repeated rules.

Prefer clear headings and direct requirements.

Avoid excessive historical narrative.

Include enough context to explain WHY:

- security is never weakened;
- operations are bounded;
- blocked/stalled commands are not passes;
- self-stop is preferable to indefinite waiting.

Do not make the instruction file so large that these critical rules become
buried.


EXISTING INSTRUCTIONS

Preserve all existing established Tempest workflow requirements, including as
applicable:

- project authority/evidence order;
- durable documentation;
- prompt/report logging;
- manual acceptance;
- Git safety;
- not launching the game;
- protected local/reference material;
- current development directory;
- maintainability comments;
- tests/build expectations.

Do not remove or weaken an existing policy while adding this one.

If an existing instruction overlaps, consolidate only when meaning is preserved
or strengthened.


AI-LOG POLICY FOR THIS MAINTENANCE TASK

Save this substantial prompt under:

AI-Logs/Prompts/

Create a NEW maintenance report under:

AI-Logs/Reports/

Do not rewrite previous reports.

AI-Logs is Git-ignored.

Do not stage or commit AI-Logs.

Use descriptive date-prefixed filenames.

The report should record:

- starting branch/HEAD;
- exact file modified;
- old relevant instruction structure;
- new section/placement;
- security-block policy added;
- timeout/bounded-loop policy added;
- stall-isolation policy added;
- no-false-pass rule added;
- self-stop policy added;
- staging/commit safety added;
- confirmation existing requirements were preserved;
- validation performed;
- final Git state;
- whether commit/push occurred.


SCOPE

Modify only:

.github/copilot-instructions.md

plus ignored AI prompt/report files.

Do not modify:

- production source;
- tests;
- solution/project files;
- README.md;
- TEMPEST-DEVELOPMENT-NOTES.md;
- TEMPEST-BEHAVIOR-SPEC.md;
- ROM/reference files;
- UserData;
- recordings;
- screenshots;
- TEMP;
- Videos;
- unrelated files.


VALIDATION

This is documentation/instructions-only.

Do not run gameplay tests or the full .NET test suite unless the instructions
file unexpectedly participates in build/test inputs.

Instead:

1. Inspect the final diff carefully.
2. Run:

git diff --check

3. Verify only .github/copilot-instructions.md is a tracked modification.
4. Verify AI prompt/report files are ignored.
5. Verify protected local/untracked material remains untouched.
6. Verify no production/test/durable gameplay documentation changed.
7. Verify the resulting Markdown is readable and non-contradictory.
8. Verify the security/stall/self-stop policy is explicit rather than implied.


COMMIT

This maintenance milestone is authorized for commit and push after the narrow
documentation validation succeeds.

Stage only:

.github/copilot-instructions.md

Do not use:

git add .

git add -A

Print:

git diff --cached --name-only

It must contain exactly:

.github/copilot-instructions.md

Run:

git diff --cached --check

Review the staged diff.


COMMIT MESSAGE

Use a concise subject such as:

Add Copilot security and stall fail-safe policy

Use the project's normal Copilot co-author trailer if that remains established
policy.


PUSH

Push master to the configured GitHub origin.

After push verify:

- local HEAD;
- origin/master;
- live remote master;
- all hashes agree;
- tracked worktree/index are clean;
- only intentional protected/untracked local material remains.


WINDOWS SECURITY DURING THIS TASK

The new policy is being added by this task, but follow the same policy during
the task itself.

If Windows security blocks an unexpected required command:

- do not weaken security;
- do not retry indefinitely;
- stop safely if required work cannot complete.

This maintenance task should not require generating a new .NET artifact.


STOP POINT

After:

- instruction update;
- narrow validation;
- staged review;
- commit;
- push;
- remote verification;
- maintenance report;

STOP.

Do not begin:

- Level 3 concurrency;
- Level 9;
- gameplay implementation;
- unrelated cleanup.


FINAL RESPONSE TO NEAL

Provide:

- commit hash;
- commit subject;
- exact tracked file changed;
- location/title of the new instruction section;
- concise description of the permanent policy;
- git diff/check result;
- push result;
- final Git status;
- confirmation future prompts no longer need to repeat the full fail-safe text.


BEFORE FINAL RESPONSE

Confirm:

[ ] Complete user prompt was read
[ ] Completion token validated
[ ] Existing copilot instructions read completely
[ ] Only .github/copilot-instructions.md modified
[ ] Existing instructions preserved
[ ] Security-block policy added
[ ] Retry-only-if-materially-different-safe-route rule added
[ ] No stale-binary substitution rule added
[ ] No-false-pass rule added
[ ] Long operations require bounds
[ ] State-wait loops require bounds
[ ] Stall/no-progress isolation policy added
[ ] Voluntary self-stop policy added
[ ] BLOCKED/PARTIAL handoff requirements added
[ ] Incomplete validation prevents normal commit
[ ] Existing no-interactive-game rule preserved
[ ] Future prompts may rely on project-wide policy
[ ] git diff --check passed
[ ] Only intended file staged
[ ] git diff --cached --check passed
[ ] Commit created
[ ] Push succeeded
[ ] Local/remote/live hashes agree
[ ] New AI report created and ignored
[ ] Final tracked worktree/index clean
[ ] No unrelated milestone started

Do not state that a promised file, report, commit, validation, or push exists
unless it actually exists.

TEMPEST_PROJECT_INSTRUCTIONS_FAIL_SAFE_MAINTENANCE_20261109