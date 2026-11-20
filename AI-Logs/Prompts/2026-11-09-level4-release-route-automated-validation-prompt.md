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

Obtain the missing automated validation for the existing Level 4
ordinary-admission plus terminal-Spiker diagnostic candidate through one
materially different safe route, without weakening or bypassing Windows
security.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected accepted baseline:

af560b100bebad672ec9231d4f88332848378594

The candidate already exists as uncommitted work in the tracked worktree.

Read these existing handoff records:

AI-Logs/Reports/2026-11-09-level4-concurrency-rom-investigation-report.md

AI-Logs/Reports/2026-11-09-level4-ordinary-admission-terminal-spiker-diagnostic-implementation-report.md

Do not redo the implementation.

Neal has manually tested the current candidate. His manual arcade-fidelity
judgment is:

"As far as I can determine, Level 4 is identical to the arcade game."

Record that as strong manual acceptance of visible Level 4 gameplay, but do not
use manual acceptance as a substitute for the missing automated validation.


PRIOR BLOCKER

The prior focused Debug validation command compiled the game and test
assemblies, but VSTest could not load:

tests\Tempest.Game.Tests\bin\Debug\net9.0\Tempest.Game.dll

Windows Application Control reported:

"An Application Control policy has blocked this file"

HRESULT / error:

0x800711C7

No test code executed.

That result remains BLOCKED, not PASS or FAIL.

Do not repeat the same unrestricted Debug command against the same generated
artifact.


SECURITY BOUNDARY

Do not:

- disable or weaken Windows Application Control;
- disable or weaken Smart App Control;
- disable Code Integrity, Defender, antivirus, or other security;
- alter security policy;
- configure audit mode;
- create an allow rule or policy exception;
- unblock or trust the DLL manually;
- change file security zones or alternate data streams;
- sign or falsely mark generated code as trusted;
- run through a security bypass;
- copy blocked binaries to a different directory merely to evade policy;
- substitute stale assemblies;
- use previously built binaries as proof of the current candidate;
- repeatedly rebuild or retry equivalent blocked artifacts;
- create a disposable substitute test harness merely to avoid VSTest;
- ask Neal to weaken security.

If required validation cannot complete under the existing enforced policy,
stop with an accurate BLOCKED/PARTIAL handoff.


PHASE 1 — VERIFY CANDIDATE AND REPOSITORY STATE

Before validation, verify:

- branch;
- HEAD;
- origin/master;
- tracking state;
- index;
- tracked worktree;
- nonignored untracked state;
- complete candidate file list;
- complete candidate diff;
- git diff --check.

Confirm that the worktree still represents the existing Level 4 implementation
candidate described by the implementation report.

Do not change production code or tests merely to address the Windows security
block.


PHASE 2 — READ-ONLY WINDOWS BLOCK DIAGNOSIS

Use bounded, read-only Windows diagnostics to identify the actual policy/event
responsible for the prior block.

Inspect relevant recent entries in:

Applications and Services Logs
Microsoft
Windows
CodeIntegrity
Operational

and, only if relevant and available:

Applications and Services Logs
Microsoft
Windows
AppLocker

Focus on events temporally associated with the blocked
Tempest.Game.dll/VSTest attempt.

Look for relevant block/policy/signing information, including where present:

- event 3077, enforced App Control policy block;
- event 3089, correlated signing information;
- event 3092, origin or reputation blocking information;
- event 3099, policy loaded/activated;
- event 3114, .NET Dynamic Code Security;
- AppLocker executable/DLL block events such as 8004.

Also inspect the active App Control policy list through a supported read-only
query such as:

citool.exe -lp

if that command is available and can be run read-only without elevation or
policy changes.

Do not install tools.
Do not modify policy.
Do not require elevation merely to continue.
Do not treat inability to read a privileged log as permission to bypass it.

Record only the evidence actually found:

- event ID;
- timestamp;
- blocked artifact/path;
- process or test host where reported;
- policy name/ID where available;
- status/error code;
- signing/origin information where available.

Do not claim a specific App Control technology caused the block unless the
events establish it.


PHASE 3 — ONE MATERIALLY DIFFERENT VALIDATION ROUTE

If repository state is correct and the read-only diagnosis does not establish
that every current generated assembly will necessarily be blocked, make one
bounded validation attempt using a normal Release configuration.

This is authorized as the materially different safe route because it produces
the candidate through the project's standard Release build/test pipeline
rather than retrying the original Debug artifact.

Do not delete, alter, unblock, or reuse the blocked Debug DLL.

Start with the focused Level 4 tests using Release configuration, no parallel
retry loop, and the established VSTest hang timeout.

Use a command equivalent to:

dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj `
  -c Release `
  --filter "FullyQualifiedName~Level4AdmissionPolicyTests|FullyQualifiedName~Level4CandidateTests" `
  --blame-hang-timeout 5min

Use correct PowerShell quoting for the actual environment.

Do not add --no-build on the first Release attempt. The Release test must
compile and exercise the current candidate rather than reuse an unknown or
stale artifact.

This is one Release-route attempt.

Classify outcomes accurately:

- If Release compilation fails normally, report FAIL with the compiler errors.
- If test code executes and tests fail, report FAIL with the failing tests.
- If tests execute and pass, report PASS with the exact count.
- If Application Control blocks a Release artifact before test execution,
  report BLOCKED with the exact artifact, command, event evidence, and error.
- If the command hangs or times out, report TIMEOUT, not PASS.
- If canceled, report CANCELED.

Do not immediately repeat the same Release command or rebuild the same blocked
Release artifact.


IF FOCUSED RELEASE TESTS ARE BLOCKED

Stop automated execution.

Do not try:

- another output directory;
- another target framework;
- Debug again;
- dotnet vstest against the same DLL;
- copying the DLL;
- renaming the DLL;
- an equivalent custom runner;
- multiple rebuilds;
- security/policy changes.

Preserve the candidate and report the milestone as:

- manual gameplay acceptance: PASS;
- static validation: PASS where already established;
- automated focused validation: BLOCKED;
- candidate status: uncommitted and incomplete for normal closeout.

Provide the exact remaining handoff to Neal.

Do not commit or push.


IF FOCUSED RELEASE TESTS PASS

Proceed in bounded stages:

1. Run the relevant affected Release test groups for:
   - Level 4 admission;
   - Spiker retreat/conversion;
   - Tanker cargo;
   - ledger/accounting;
   - death/reentry;
   - Super Zapper;
   - persistent Spikes;
   - completion/all-rim;
   - campaign/direct startup;
   - recording/playback;
   - Levels 1-3 concurrency regressions.

2. If affected validation passes, run the full solution tests in Release with
   the established VSTest hang protection.

3. If full tests pass, run the full solution Release build.

Use the normal project commands and standard output paths.

Do not create repeated retries. At the first renewed security block, timeout,
stall, or infrastructure failure, stop and report it accurately.


FINAL STATIC VALIDATION

Whether automated execution passes or is blocked:

- run git diff --check;
- inspect the complete tracked diff;
- verify the exact modified/new tracked files;
- verify no files are staged;
- verify HEAD remains at the accepted baseline;
- verify AI-Logs remain ignored and uncommitted;
- verify no security settings or policies changed.


REPORT

Do not rewrite the historical implementation report.

Create a new ignored validation follow-up report under AI-Logs/Reports, with a
name such as:

2026-11-09-level4-release-route-automated-validation-report.md

Preserve this prompt under AI-Logs/Prompts with a matching filename.

The report must include:

- starting repository state;
- prior blocked Debug command/artifact/error;
- current manual acceptance from Neal;
- Code Integrity/AppLocker evidence actually found;
- active-policy information actually found;
- why Release configuration was materially different;
- exact Release command;
- whether test code executed;
- focused test result/count or exact blocker;
- broader/full test and build results if reached;
- static validation;
- final repository state;
- remaining validation or closeout status;
- confirmation that security was not weakened or bypassed.


GIT AND STOP

Do not commit or push in this validation task.

Even if all automated validation passes, stop with the candidate uncommitted.

If validation passes, report that the candidate is ready for a separate
accepted closeout prompt using Neal's already completed manual acceptance.

If validation remains blocked, preserve the worktree and provide the precise
safe handoff.

Do not launch the interactive game.
Do not begin Level 5 concurrency.
Do not perform comment cleanup or unrelated work.


TEMPEST_LEVEL4_SAFE_RELEASE_VALIDATION_COMPLETE