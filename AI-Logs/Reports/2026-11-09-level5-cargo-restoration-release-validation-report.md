# Level 5 cargo restoration Release validation

Date: 2026-11-09

Status: **FAIL / INCOMPLETE.** The authorized fresh Release validation reached
test-project compilation but failed on four compile errors in the cargo
snapshot regression. VSTest did not execute; no tests passed or failed. The
candidate was not changed. Manual acceptance remains pending.

## Starting candidate and prior block

- Branch: `master`; upstream: `origin/master`.
- HEAD and `origin/master` matched the expected baseline:
  `62c66e0ab19436ea162935627021154be21e0a41`.
- No staged changes. The uncommitted candidate comprised six tracked files
  and two nonignored untracked files:
  - `src/Tempest.Game/Gameplay/Flipper.cs`
  - `src/Tempest.Game/Levels/Level.cs`
  - `src/Tempest.Game/Levels/Level3WaveLedger.cs`
  - `src/Tempest.Game/TempestGame.cs`
  - `src/Tempest.Game/Levels/Level5AdmissionPolicy.cs` (untracked)
  - `tests/Tempest.Game.Tests/Level3WaveLedgerTests.cs`
  - `tests/Tempest.Game.Tests/Level5ProductionTests.cs`
  - `tests/Tempest.Game.Tests/Level5AdmissionPolicyTests.cs` (untracked)
- The candidate matched the admission/concurrency and cargo-restoration
  handoffs. The final restoration capture is value-only, preserves cargo ID
  and in-tube movement/lifecycle state, and queues restorations when physical
  headroom is full. No implementation changes were made during this
  validation.
- The prior focused Debug attempt reported Windows Application Control error
  `0x800711C7` while loading
  `tests\Tempest.Game.Tests\bin\Debug\net9.0\Tempest.Game.Tests.dll`; no tests
  executed in that invocation.

## Read-only security diagnosis

A bounded query of
`Microsoft-Windows-CodeIntegrity/Operational` found two event 3077 entries
at 8:27:03 PM and 8:27:04 PM on 2026-11-09. Both report that
`tests\Tempest.Game.Tests\bin\Debug\net9.0\testhost.exe` attempted to load
`tests\Tempest.Game.Tests\bin\Debug\net9.0\Tempest.Game.Tests.dll`, which did
not meet Enterprise signing-level requirements or violated Code Integrity
policy `0283ac0f-fff1-49ae-ada1-8a933130cad6`. The event did not provide
signing or origin details. No matching AppLocker event was found.

Read-only `citool.exe -lp` output identified the signed, authorized,
currently enforced `VerifiedAndReputableDesktop` policy with that ID. No
security setting was changed.

## Fresh focused Release route

Release was materially different from the prior blocked Debug attempt: it
requested a fresh Release configuration build in the normal test project
output, without reusing or relocating the Debug artifact and without
`--no-build`.

Command:

```powershell
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --configuration Release --filter "FullyQualifiedName~Level3WaveLedgerTests.ExplicitDeathRestorationKeepsOnlySelectedUnresolvedCargoOutOfEggReserve|FullyQualifiedName~Level5ProductionTests.DeathRestoresUnresolvedLevelFiveCargoInTubeWithoutCargoEggs|FullyQualifiedName~Level5ProductionTests.CargoRestorationSnapshotPreservesPivotAndPendingZapState" --blame-hang-timeout 5min
```

- Restore: **PASS**; projects were up to date.
- Game project compilation: **PASS**; `Tempest.Game.dll` was produced in
  `bin\Release\net9.0`.
- Test project compilation: **FAIL** with four errors in
  `Level5ProductionTests.cs`:
  - line 620: `Flipper.ReloadShotDelay` is not accessible;
  - line 622: `FlipperTankerCargoRestorationSnapshot` is inaccessible;
  - line 623: `Flipper.CaptureDeathRestorationSnapshot` is not accessible;
  - line 624: `Flipper.RestoreTankerCargo` is not accessible.
- VSTest execution: **NO**. Test count: **0 executed**. This was a normal
  compilation failure, not a security block or a test assertion failure.
- Per the stop-on-first-failure instruction, no broader focused groups,
  affected regressions, full solution tests, or full solution build were run.
  No candidate correction was attempted.

## Final static and repository checks

- `git diff --check`: **PASS**.
- The complete tracked candidate diff and both untracked candidate files were
  inspected.
- Final candidate file list remains the eight paths above. No candidate file
  was changed by this validation.
- HEAD and `origin/master` remain
  `62c66e0ab19436ea162935627021154be21e0a41`; upstream remains `origin/master`.
- No files are staged. The prompt and this report are ignored under
  `AI-Logs/` and are not part of the candidate or Git changes.
- No security configuration, signing/trust metadata, or policy was changed or
  bypassed. The game was not launched. No commit or push was made.

## Manual acceptance and remaining status

Manual gameplay acceptance remains **PENDING**. It was not performed because
automated validation did not pass. Once the compile issue is resolved and
automated validation passes, Neal's requested command is:

`dotnet run --project src\Tempest.Game -- --level 5 --lives 10`

Do not treat this candidate as accepted until the focused and required
broader automated validation succeeds and Neal completes the manual
Level 5 arcade comparison. Level 6 work was not started.
