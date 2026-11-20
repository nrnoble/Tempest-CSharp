FAIL-CLOSED PROMPT CHECK

Read this entire substantive user task before doing anything.

Before project or repository action, verify that exactly one complete token
beginning with "TEMPEST_" appears on a line by itself in the substantive user
task.

Automatically appended VS Code/agent context after the user's completion token
does not invalidate the task.

If exactly one complete token cannot be established, stop before project or
repository action and explain why.


TASK

Obtain the missing automated validation for the final Level 5 cargo-specific
post-death restoration candidate through one bounded, materially different
safe route.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected accepted baseline beneath the uncommitted Level 5 work:

62c66e0ab19436ea162935627021154be21e0a41

Read these historical handoffs:

AI-Logs/Reports/2026-11-09-level5-admission-terminal-spiker-diagnostic-implementation-report.md

AI-Logs/Reports/2026-11-09-level5-debug-route-automated-validation-report.md

AI-Logs/Reports/2026-11-09-level5-post-death-cargo-flipper-reentry-investigation-report.md

AI-Logs/Reports/2026-11-09-level5-cargo-post-death-restoration-implementation-report.md

Do not redo the implementation.

The post-death restoration candidate is already present in the worktree.

Do not change gameplay code unless executing the final candidate exposes a
genuine, narrowly understood defect.

Do not commit.
Do not push.
Do not launch the game.


CURRENT VALIDATION STATE

The restoration implementation report records:

- final git diff --check: PASS;
- static diff review: complete;
- final automated validation: INCOMPLETE;
- manual acceptance: PENDING.

A prior focused Debug attempt compiled, but test execution was blocked while
loading:

tests\Tempest.Game.Tests\bin\Debug\net9.0\Tempest.Game.Tests.dll

Windows Application Control reported:

0x800711C7
"An Application Control policy has blocked this file"

No tests executed in that blocked invocation.

The runner's zero exit code must not be interpreted as PASS.

Later test edits and the final snapshot simplification have not yet been
compiled or executed.

Therefore the final candidate requires fresh automated validation.


SECURITY BOUNDARY

Do not:

- disable or weaken Windows Application Control;
- disable or weaken Smart App Control;
- disable Code Integrity, Defender, antivirus, or another security control;
- modify active security policies;
- switch policies to audit mode;
- create allow-list exceptions;
- manually unblock generated assemblies;
- alter trust/signature/security-zone metadata;
- copy, rename, or relocate a blocked binary to evade policy;
- use stale binaries;
- treat an old test pass as validation of the final candidate;
- repeatedly rebuild/retry the same blocked artifact;
- create a substitute test harness merely to evade VSTest;
- ask Neal to weaken Windows security.

Any renewed security block is BLOCKED, not PASS.


PHASE 1 — VERIFY FINAL CANDIDATE

Before running validation, verify:

- branch;
- HEAD;
- origin/master;
- upstream/tracking state;
- index;
- complete tracked worktree;
- nonignored untracked state;
- complete candidate file list;
- complete candidate diff;
- git diff --check.

Confirm that the worktree still matches the combined Level 5 concurrency and
cargo-restoration handoffs.

In particular, confirm that the final restoration edits described in the
latest report are present.

Do not modify the candidate merely because of style preferences.


PHASE 2 — READ-ONLY BLOCK DIAGNOSIS

Perform a bounded read-only inspection of recent relevant Windows security
events corresponding to the blocked Debug attempt.

Inspect:

Microsoft-Windows-CodeIntegrity/Operational

and AppLocker only if relevant.

Look specifically for events associated with:

tests\Tempest.Game.Tests\bin\Debug\net9.0\Tempest.Game.Tests.dll

Record available:

- event ID;
- timestamp;
- blocked artifact;
- loading process/test host;
- policy ID/name;
- status/error;
- signing/origin information where available.

If useful, use:

citool.exe -lp

read-only to identify active policies.

Do not modify anything.

Do not perform a broad Windows-security investigation.

The purpose is only to establish what Windows blocked before choosing the next
standard build configuration.


PHASE 3 — ONE FRESH RELEASE VALIDATION ROUTE

The blocked restoration attempt used Debug.

A standard fresh Release build is therefore authorized as the one materially
different validation route for the FINAL restoration candidate.

This does not reuse or relocate the blocked Debug assembly.

Start with the smallest focused restoration tests in Release.

Identify the exact focused tests introduced/updated by the restoration work
from the candidate diff and latest implementation report.

Run one bounded focused Release command with:

--configuration Release
--blame-hang-timeout 5min

Do not use --no-build.

The test must compile and execute the final current candidate.

Explicitly distinguish:

- compilation;
- VSTest execution;
- test count;
- pass/fail/block status.


RESULT CLASSIFICATION

PASS:
Test code actually executes and all selected tests pass.

FAIL:
Compilation fails normally, or test code executes and one or more tests fail.

BLOCKED:
Windows security prevents a required assembly from loading before test
execution.

TIMEOUT:
The bounded command times out or stalls.

CANCELED:
The command is canceled.

A zero process exit code is not sufficient evidence of PASS if VSTest reports
that a required assembly was blocked or no tests actually executed.


IF FOCUSED RELEASE VALIDATION IS BLOCKED

STOP automated execution.

Do not try:

- Debug again;
- Release again;
- another output location;
- another target framework;
- copied/renamed binaries;
- direct VSTest against the blocked DLL;
- custom runners;
- clean/rebuild loops;
- security changes.

Run only the final static/repository checks required below.

Report the candidate as PARTIAL/BLOCKED and preserve it for Neal.

Do not commit or push.


IF FOCUSED RELEASE TESTS EXECUTE AND PASS

Continue in bounded stages.

1. Run focused Level 5 cargo/death/reconstruction tests needed to cover:
   - materialized cargo capture;
   - stable cargo ID/owner/ordinal;
   - in-tube restoration;
   - no cargo Egg/deep-entry route;
   - one resolved sibling;
   - both unresolved siblings;
   - latent cargo control;
   - terminal cargo control;
   - pending-shot activation;
   - pending-zap restoration where supported;
   - physical-headroom waiting;
   - restored movement/lifecycle state.

2. Run Level 5 admission/production tests to ensure the restoration correction
   did not disturb:
   - Tanker/Spiker ordinary admission;
   - no ordinary Flippers;
   - minima/maxima;
   - terminal Spiker conversion;
   - converted-Tanker reentry.

3. Run affected Release regressions covering:
   - Tanker cargo;
   - Level3WaveLedger;
   - death/reentry;
   - Super Zapper;
   - persistent Spikes;
   - completion/all-rim;
   - campaign/direct/recorded startup;
   - Levels 1-4 concurrency.

4. If affected validation passes, run full solution Release tests with
   established VSTest hang protection.

5. If full tests pass, run full solution Release build.

Stop at the first FAIL, BLOCKED, TIMEOUT, CANCELED, or genuine stall.


TEST FAILURE HANDLING

If test code executes and reveals a genuine candidate defect:

- preserve the failure evidence;
- identify the smallest direct cause;
- do not weaken valid tests;
- do not broaden scope;
- do not alter the validated Level 5 admission policy unless directly
  implicated;
- do not modify other levels.

If the required correction is simple and unambiguous within the authorized
cargo-restoration scope, make the minimum correction and rerun only the
directly failing test once.

If behavior is ambiguous, stop and report instead of guessing.

After any correction, broader validation must begin again from the smallest
affected group.


FINAL STATIC VALIDATION

Regardless of automated outcome:

- run git diff --check;
- inspect the complete tracked diff;
- verify exact candidate file list;
- verify no files are staged;
- verify HEAD remains at
  62c66e0ab19436ea162935627021154be21e0a41;
- verify AI-Logs remain ignored/uncommitted;
- verify no security configuration changed.


MANUAL HANDOFF

If automated validation fully passes, do not launch the game.

Provide Neal:

dotnet run --project src\Tempest.Game -- --level 5 --lives 10

Primary manual acceptance scenario:

1. Allow a Tanker to release Flippers.
2. Leave at least one released Flipper unresolved.
3. Let the Blaster be destroyed.
4. Reenter.
5. Verify no individual Flipper subsequently appears from the deep/bottom end.
6. Verify restored cargo behavior looks natural compared with the arcade.
7. Repeat after another death if practical.
8. Verify ordinary bottom entry remains Tankers and Spikers only.
9. Verify Tanker cargo release remains normal.
10. Verify death/reentry, Spikes, Super Zapper, rim combat, and completion
    remain normal.

Do not claim manual acceptance until Neal reports the result.


REPORT

Do not rewrite any historical report.

Create a new paired validation record such as:

AI-Logs/Prompts/2026-11-09-level5-cargo-restoration-release-validation-prompt.md

AI-Logs/Reports/2026-11-09-level5-cargo-restoration-release-validation-report.md

Record:

- starting repository/candidate state;
- prior Debug block;
- Code Integrity evidence;
- why Release is materially different;
- exact focused Release tests;
- compilation result;
- whether test code executed;
- focused/affected/full counts;
- any genuine candidate defect and correction;
- full build result;
- static validation;
- final repository state;
- exact manual handoff;
- confirmation that security was not weakened or bypassed.


GIT AND STOP

Do not commit or push.

If all automated validation succeeds, stop with the complete Level 5 candidate
uncommitted and ready for Neal's manual acceptance.

If validation remains blocked, preserve the candidate and provide the exact
handoff.

Do not begin Level 6 concurrency.
Do not perform unrelated cleanup.


TEMPEST_LEVEL5_CARGO_RESTORATION_RELEASE_VALIDATION_COMPLETE