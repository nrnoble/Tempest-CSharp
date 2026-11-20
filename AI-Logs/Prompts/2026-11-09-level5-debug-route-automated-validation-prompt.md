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

Obtain the missing automated validation for the final 28-test Level 5
admission-and-terminal-Spiker diagnostic candidate through one materially
different safe route, without weakening or bypassing Windows security.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected accepted baseline:

62c66e0ab19436ea162935627021154be21e0a41

The uncommitted Level 5 candidate already exists in the worktree.

Read these historical handoff records:

AI-Logs/Reports/2026-11-09-level5-concurrency-rom-investigation-report.md

AI-Logs/Reports/2026-11-09-level5-admission-terminal-spiker-diagnostic-implementation-report.md

Do not redo or revise the implementation unless automated test execution
reveals a genuine candidate defect.

Do not launch the interactive game.
Do not commit.
Do not push.


CURRENT VALIDATION STATE

The final candidate's focused Release command was:

dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj `
  --configuration Release `
  --filter "FullyQualifiedName~Level5AdmissionPolicyTests|FullyQualifiedName~Level5ProductionTests" `
  --blame-hang-timeout 5min

The build phase passed, but VSTest was prevented from loading:

tests\Tempest.Game.Tests\bin\Release\net9.0\Tempest.Game.dll

Windows Application Control reported:

An Application Control policy has blocked this file

Error:

0x800711C7

All 28 selected tests were blocked before test execution.

That result remains BLOCKED, not PASS or FAIL.

An earlier Release run passed 24 tests, but it occurred before the final
waiting, terminal-conversion, and copied-configuration tests were added. It
does not validate the final candidate and must not be substituted for it.


SECURITY BOUNDARY

Do not:

- disable or weaken Windows Application Control;
- disable or weaken Smart App Control;
- disable Code Integrity, Defender, antivirus, or another security control;
- change an enforcement policy to audit mode;
- modify or remove an active security policy;
- create a supplemental allow policy or exception;
- alter trust, signing, security-zone, or alternate-data-stream metadata;
- manually unblock the generated DLL;
- copy or move the blocked Release DLL to evade policy;
- rename the blocked DLL;
- use a stale or earlier assembly;
- treat the earlier 24-test result as final validation;
- repeatedly rebuild or retry the same Release artifact;
- create a substitute test harness merely to avoid VSTest;
- ask Neal to weaken Windows security.

Do not reuse, modify, or execute the blocked Release DLL as the proposed
different route.


PHASE 1 — VERIFY THE CANDIDATE

Before validation, verify:

- branch;
- HEAD;
- origin/master;
- upstream/tracking state;
- index;
- tracked worktree;
- nonignored untracked state;
- exact Level 5 candidate file list;
- complete candidate diff;
- git diff --check.

Confirm that the candidate still matches the implementation report.

The expected production/test candidate includes:

src/Tempest.Game/Levels/Level5AdmissionPolicy.cs
src/Tempest.Game/Levels/Level3WaveLedger.cs
src/Tempest.Game/TempestGame.cs
tests/Tempest.Game.Tests/Level5AdmissionPolicyTests.cs
tests/Tempest.Game.Tests/Level5ProductionTests.cs

Verify actual Git state rather than assuming this list is complete.

If the candidate materially differs from the reported final candidate, stop
and report the discrepancy.


PHASE 2 — READ-ONLY BLOCK DIAGNOSIS

Perform bounded, read-only diagnosis of the final Release block.

Inspect recent relevant events in:

Applications and Services Logs
Microsoft
Windows
CodeIntegrity
Operational

and, only if useful:

Applications and Services Logs
Microsoft
Windows
AppLocker

Correlate events with the blocked Level 5 Release attempt and the path:

tests\Tempest.Game.Tests\bin\Release\net9.0\Tempest.Game.dll

Look for applicable evidence such as:

- 3077 — enforced Code Integrity policy block;
- 3089 — signing information associated with the block;
- 3092 — origin/reputation block information;
- 3099 — active policy information;
- 3114 — .NET Dynamic Code Security;
- 8004 — AppLocker executable/DLL block.

If available, use a supported read-only query such as:

citool.exe -lp

to identify active App Control policies.

Do not install diagnostic tools.
Do not modify policies.
Do not require elevation merely to continue.
Do not collect broad unrelated system information.

Record only evidence actually found:

- event ID and timestamp;
- blocked path;
- loading process/test host;
- policy name and ID where available;
- status/error code;
- signing and origin information where available.

Do not claim that every generated assembly will be blocked unless the evidence
establishes that.


PHASE 3 — ONE MATERIALLY DIFFERENT DEBUG VALIDATION ATTEMPT

If the repository state is correct and the diagnosis does not establish that
a normal Debug artifact is necessarily prohibited, make exactly one focused
Debug-configuration validation attempt.

This is the authorized materially different route:

- the blocked final candidate used the standard Release output;
- this attempt uses the project's standard Debug pipeline and separate
  bin\Debug\net9.0 output;
- it does not copy, rename, alter, trust, or reuse the blocked Release DLL;
- it does not change Windows policy or project behavior.

Run a command equivalent to:

dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj `
  --configuration Debug `
  --filter "FullyQualifiedName~Level5AdmissionPolicyTests|FullyQualifiedName~Level5ProductionTests" `
  --blame-hang-timeout 5min

Use correct PowerShell quoting.

Do not use --no-build for this first Debug attempt. The command must compile
and execute the current final candidate rather than reuse an unknown artifact.

This is one attempt. Do not automatically rerun it.


CLASSIFY THE FOCUSED RESULT

Report the result accurately:

- PASS only if test code executes and all 28 tests pass;
- FAIL if compilation fails or test code executes and one or more tests fail;
- BLOCKED if Windows security prevents loading an artifact before execution;
- TIMEOUT if the bounded operation times out or stalls;
- CANCELED if it is canceled.

Explicitly report:

- whether compilation completed;
- whether VSTest executed test code;
- exact test count;
- exact failed test names if any;
- exact blocked artifact/error/event evidence if blocked.

Do not interpret a successful build as successful test execution.


IF THE DEBUG ATTEMPT IS BLOCKED

Stop all automated execution.

Do not try:

- Release again;
- another output directory;
- another target framework;
- copying or renaming assemblies;
- dotnet vstest against the same artifact;
- a custom test runner;
- repeated clean/rebuild cycles;
- security-policy changes;
- stale binaries.

Preserve the worktree.

Report:

- static validation: PASS where already established;
- final focused automated validation: BLOCKED;
- manual acceptance: still pending;
- candidate: uncommitted and not ready for normal closeout;
- exact safe handoff required from Neal.

Do not commit or push.


IF ALL 28 FOCUSED DEBUG TESTS PASS

Proceed in bounded stages using Debug configuration.

1. Run the relevant affected test groups covering:
   - Level 5 admission and production behavior;
   - Spiker terminal conversion;
   - Tanker cargo;
   - ledger and identity conservation;
   - converted-Tanker death/reentry;
   - waiting Eggs and deterministic RNG behavior;
   - persistent Spikes;
   - Super Zapper lifecycle;
   - completion/all-rim;
   - direct/campaign/recorded Level 5 startup;
   - Levels 1-4 concurrency regressions.

2. If affected validation passes, run full solution Debug tests with existing
   VSTest hang protection:

   dotnet test TempestCSharp.slnx `
     --configuration Debug `
     --blame-hang-timeout 5min

3. If full tests pass, run the full solution Debug build:

   dotnet build TempestCSharp.slnx --configuration Debug

At the first renewed Windows security block, test failure, timeout, stall, or
other incomplete result, stop and report it accurately.

Do not retry an equivalent failed or blocked command.


DEFECT HANDLING

If tests execute and expose a genuine Level 5 candidate defect:

- diagnose only the smallest relevant cause;
- do not silently broaden scope;
- do not weaken a valid test;
- do not modify Levels 1-4 or 6-8;
- do not automatically implement a speculative correction unless the defect
  and required fix are unambiguous and remain within the authorized Level 5
  candidate scope.

If a code correction is necessary, preserve the failure evidence, make only the
smallest justified change, rerun the smallest directly affected test once, and
then follow the bounded validation sequence.

If the proper behavior is ambiguous, stop and report instead of guessing.


FINAL STATIC VALIDATION

Whether automated validation passes, fails, or is blocked:

- run git diff --check;
- inspect the complete tracked diff;
- verify candidate file state;
- verify no files are staged;
- verify HEAD remains at the expected accepted baseline;
- verify AI-Logs remain ignored and uncommitted;
- verify no security configuration or policy changed.


MANUAL ACCEPTANCE

Manual Level 5 acceptance remains pending.

Do not launch the game in this validation task.

If automated validation passes, provide Neal the supported manual command:

dotnet run --project src\Tempest.Game -- --level 5 --lives 10

State that Neal should then perform the previously defined arcade comparison.

Do not treat prior general Level 5 acceptance as acceptance of this new
concurrency candidate.


REPORT

Do not rewrite the historical implementation report.

Create a new ignored follow-up report such as:

AI-Logs/Reports/2026-11-09-level5-debug-route-automated-validation-report.md

Preserve this prompt under the matching AI-Logs/Prompts filename.

The report must include:

- starting repository state;
- final candidate identity/file list;
- prior blocked Release command, artifact, and error;
- read-only Code Integrity/AppLocker evidence;
- active policy information actually found;
- why Debug was materially different;
- exact focused Debug command;
- compilation status;
- whether test code executed;
- focused test count/result;
- affected/full test and build results if reached;
- any defect and correction;
- final static validation;
- final repository state;
- remaining manual acceptance or blocker;
- confirmation that Windows security was not weakened or bypassed.


GIT AND STOP

Do not commit or push.

If automated validation passes, stop with the candidate uncommitted and report
that it is ready for Neal's manual Level 5 arcade comparison.

If validation remains blocked or tests fail, preserve the candidate and provide
the exact next handoff.

Do not launch the game.
Do not begin Level 6 concurrency.
Do not perform unrelated cleanup.


TEMPEST_LEVEL5_SAFE_DEBUG_VALIDATION_COMPLETE