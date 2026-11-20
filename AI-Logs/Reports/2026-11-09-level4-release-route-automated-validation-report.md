# Level 4 Release-route automated validation

Date: 2026-11-09

Status: **AUTOMATED VALIDATION PASS. Candidate remains uncommitted.**

## Starting state

- Expected baseline, `HEAD`, and `origin/master`:
  `af560b100bebad672ec9231d4f88332848378594`.
- Branch: `master`, tracking `origin/master`; no ahead/behind commits.
- The existing Level 4 candidate was present in the worktree. The index was
  empty; there were four modified tracked files and two nonignored untracked
  candidate files, with no other nonignored untracked files.
- The complete substantive task is preserved in the matching
  [validation prompt](../Prompts/2026-11-09-level4-release-route-automated-validation-prompt.md).
  Its SHA-256 matches the read-only pasted-task snapshot.

## Prior Debug block and read-only diagnosis

The prior focused Debug command was:

```text
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --filter "FullyQualifiedName~Level4AdmissionPolicyTests|FullyQualifiedName~Level4CandidateTests" --blame-hang-timeout 5min
```

It built the assemblies but VSTest did not execute test code: it could not load
`tests\Tempest.Game.Tests\bin\Debug\net9.0\Tempest.Game.dll`; the reported
Windows error was `0x800711C7` (“An Application Control policy has blocked
this file”).

Read-only Code Integrity Operational events correlated with the prior attempt:

- Event **3077**, 2026-11-09 at **18:00:11.0279686 -08:00** and
  **18:00:14.2664960 -08:00**: `testhost.exe` attempted to load
  `tests\Tempest.Game.Tests\bin\Debug\net9.0\Tempest.Game.dll`.
- Both records identify the `VerifiedAndReputableDesktop` policy,
  GUID `{0283ac0f-fff1-49ae-ada1-8a933130cad6}`. Event data reports requested
  signing level 2, validated signing level 1, and status `0xc0e90002`.
- Correlated event **3089** records report no signatures, publisher and issuer
  `Unknown`, and validated signing level 0.
- The AppLocker EXE/DLL log was enabled; no matching block events were found
  in the inspected recent window.
- Read-only `citool.exe -lp` listed `VerifiedAndReputableDesktop` as a signed,
  authorized, currently enforced platform policy with status 0.

The event evidence attributes the prior denial to the named enforced Code
Integrity policy; it does not establish that every generated assembly or the
Release artifact must be blocked.

## Release validation

Release was materially different from the blocked Debug attempt: the standard
test command built the current candidate into the separate
`bin\Release\net9.0` outputs and did not reuse or alter the blocked Debug DLL.

Focused command:

```text
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj -c Release --filter "FullyQualifiedName~Level4AdmissionPolicyTests|FullyQualifiedName~Level4CandidateTests" --blame-hang-timeout 5min
```

**PASS:** Release compilation completed, test code executed, and all **39**
focused tests passed (0 failed, 0 skipped).

The complete affected test project was then run in Release to cover the
requested admission, Spiker/conversion, Tanker cargo, ledger/accounting,
death/reentry, Super Zapper, persistent Spike, completion/all-rim,
campaign/direct-startup, recording/playback, and Levels 1-3 regression areas.
**PASS:** all **1,154** tests passed (0 failed, 0 skipped).

Full solution tests:

```text
dotnet test TempestCSharp.slnx -c Release --blame-hang-timeout 5min
```

**PASS:** all **1,154** tests passed (0 failed, 0 skipped).

Full solution Release build:

```text
dotnet build TempestCSharp.slnx -c Release
```

**PASS:** build succeeded with 0 warnings and 0 errors.

No timeout, stall, renewed security block, or test infrastructure failure
occurred. No retry was needed.

## Manual acceptance and final checks

Neal's manual arcade-fidelity judgment is: “As far as I can determine, Level 4
is identical to the arcade game.” This is recorded as **PASS — strong manual
acceptance of visible Level 4 gameplay**. It is separate from, and was not
used as a substitute for, the automated results above.

- **PASS:** `git diff --check`; complete tracked candidate diff reviewed.
- **PASS:** exact candidate file list remains:
  - `src/Tempest.Game/Levels/Level3WaveLedger.cs`
  - `src/Tempest.Game/Levels/LevelConfiguration.cs`
  - `src/Tempest.Game/TempestGame.cs`
  - `tests/Tempest.Game.Tests/Level4CandidateTests.cs`
  - `src/Tempest.Game/Levels/Level4AdmissionPolicy.cs` (untracked)
  - `tests/Tempest.Game.Tests/Level4AdmissionPolicyTests.cs` (untracked)
- **PASS:** no staged files; no unrelated nonignored untracked files.
- **PASS:** `HEAD` remains at the expected baseline on `master`, tracking
  `origin/master`; the candidate remains uncommitted.
- The prompt and this report are ignored by `AI-Logs/`; neither is staged or
  committed.
- No security settings or policies were changed, weakened, or bypassed. No
  blocked artifact was copied, altered, unblocked, or reused for validation.
- The interactive game was not launched. No commit or push was made.

## Closeout

The candidate is ready for a separate accepted closeout prompt using Neal's
already completed manual acceptance. This validation task did not commit or
push the candidate.
