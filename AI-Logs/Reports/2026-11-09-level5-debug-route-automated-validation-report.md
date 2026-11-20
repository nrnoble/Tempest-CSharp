# Level 5 Debug-route automated validation

Date: 2026-11-09

Status: **AUTOMATED VALIDATION PASS.** All focused, affected, and full Debug
test runs passed after one narrowly scoped regression fix. Neal's Level 5
arcade comparison remains pending.

## Starting repository and candidate

- Branch: `master`; upstream: `origin/master`.
- Starting HEAD and `origin/master` matched the expected accepted baseline:
  `62c66e0ab19436ea162935627021154be21e0a41`.
- No staged changes. The uncommitted Level 5 candidate matched the prior
  implementation report and consisted of:
  - `src/Tempest.Game/Levels/Level5AdmissionPolicy.cs`
  - `src/Tempest.Game/Levels/Level3WaveLedger.cs`
  - `src/Tempest.Game/TempestGame.cs`
  - `tests/Tempest.Game.Tests/Level5AdmissionPolicyTests.cs`
  - `tests/Tempest.Game.Tests/Level5ProductionTests.cs`
- The pre-run tracked diff passed `git diff --check`. No other nonignored
  untracked files were present.
- The complete substantive task was preserved byte-for-byte in the matching
  ignored [prompt record](../Prompts/2026-11-09-level5-debug-route-automated-validation-prompt.md);
  its standalone completion token was verified exactly once.

## Prior Release block and read-only diagnosis

The final candidate's earlier focused Release command was:

```powershell
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj `
  --configuration Release `
  --filter "FullyQualifiedName~Level5AdmissionPolicyTests|FullyQualifiedName~Level5ProductionTests" `
  --blame-hang-timeout 5min
```

Its build succeeded, but VSTest did not execute tests because it could not
load:

`tests\Tempest.Game.Tests\bin\Release\net9.0\Tempest.Game.dll`

The reported error was `0x800711C7` (“An Application Control policy has
blocked this file”). The result was correctly treated as **BLOCKED**, not a
test failure or pass.

Bounded, read-only event queries found Code Integrity event **3077** at
**7:16:57 PM** and **7:17:01 PM** on 2026-11-09. Both identify
`tests\Tempest.Game.Tests\bin\Release\net9.0\testhost.exe` attempting to load
the Release `Tempest.Game.dll`; the event says the DLL did not meet Enterprise
signing-level requirements or violated code integrity policy
`0283ac0f-fff1-49ae-ada1-8a933130cad6`. The event text did not provide signing
or origin details. No correlated AppLocker 8004 event was found. No
correlated signing/origin event was found in the bounded query.

`citool.exe -lp` was used read-only. The relevant active policy was the signed,
authorized `VerifiedAndReputableDesktop` policy
(`0283ac0f-fff1-49ae-ada1-8a933130cad6`), reported as currently enforced with
status 0. Its `VerifiedAndReputableDesktopFlightSupplemental` policy was also
reported enforced and authorized. The query also listed other enforced
Windows Code Integrity, virtualization-based-security, and endpoint-security
policies; no policy was changed.

Older Code Integrity 3077 events at **6:00:11 PM** and **6:00:14 PM** named
Debug-path assemblies under `tests\Tempest.Game.Tests\bin\Debug\net9.0`.
Those events concerned prior artifacts and did not establish that every
freshly compiled Debug artifact was necessarily prohibited. The authorized
Debug attempt below used a standard fresh Debug build and did not reuse the
blocked Release DLL.

## Focused Debug attempt

The one authorized focused Debug command was:

```powershell
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj `
  --configuration Debug `
  --filter "FullyQualifiedName~Level5AdmissionPolicyTests|FullyQualifiedName~Level5ProductionTests" `
  --blame-hang-timeout 5min
```

- Compilation: **PASS**; both game and test assemblies compiled to
  `bin\Debug\net9.0`.
- VSTest executed test code: **YES**.
- Result: **PASS — 28 passed, 0 failed, 0 skipped**.
- No matching Code Integrity or AppLocker block was recorded during this
  attempt.

## Defect found and correction

The first affected-regression run executed 345 tests and exposed one genuine
candidate regression:

`SuperZapperTests.SpikerRetreatPreemptsPendingZapAndKeepsTheSpike`

It threw `NullReferenceException` in `TempestGame.UpdateSpikers` while
evaluating the new Level 5 terminal-conversion check. This non-Level-5
development path has no production ledger. The candidate had evaluated the
ledger's pending count before establishing that the canonical Level 5 policy
applied.

The minimal correction gates that pending-count evaluation behind
`HasLevel5AdmissionDiagnostic`. No behavior outside the canonical Level 5
diagnostic was changed. The single failing test was rerun once after the
correction and passed: **1 passed, 0 failed**.

## Bounded validation after correction

1. Affected regression groups covering Level 5 admission/production, ledger
   identity and cargo, terminal conversion/reentry, waiting Eggs, persistent
   Spikes, Super Zapper, completion/rim, startup/recording/playback, and
   Levels 1–4 concurrency:
   - **PASS — 345 passed, 0 failed, 0 skipped.**
2. Full solution Debug tests:
   - Command: `dotnet test TempestCSharp.slnx --configuration Debug --blame-hang-timeout 5min`
   - **PASS — 1,165 passed, 0 failed, 0 skipped.**
3. Full solution Debug build:
   - Command: `dotnet build TempestCSharp.slnx --configuration Debug`
   - **PASS — 0 warnings, 0 errors.**

No retry followed a security block; the Release artifact was not reused.

## Final static and repository state

- Final `git diff --check`: **PASS**.
- The complete tracked diff and both new candidate files were inspected.
- Branch remains `master`; HEAD and `origin/master` remain
  `62c66e0ab19436ea162935627021154be21e0a41`; upstream remains `origin/master`.
- No files are staged. The five candidate files remain uncommitted.
- The prompt and this report are ignored under `AI-Logs/` and are not part of
  the candidate or Git changes.
- Windows Application Control, Smart App Control, Code Integrity, Defender,
  antivirus, security-zone/trust metadata, and policy configuration were not
  modified or bypassed. The game was not launched. No commit or push was made.

## Manual acceptance and handoff

Automated validation is complete, but manual Level 5 acceptance is still
pending. The candidate is ready for Neal's manual Level 5 arcade comparison;
prior general Level 5 acceptance does not cover this concurrency candidate.
Neal may launch it with:

```powershell
dotnet run --project src\Tempest.Game -- --level 5 --lives 10
```

Compare Level 5 admission/concurrency, terminal Spiker conversion, Tanker
cargo under limited headroom, death/reentry, persistent Spikes, rim behavior,
Super Zapper interaction, and completion against the previously defined
arcade comparison. Level 6 concurrency was not started.
