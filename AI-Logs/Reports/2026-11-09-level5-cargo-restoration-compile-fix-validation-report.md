# Level 5 cargo restoration compile-fix validation

Date: 2026-11-09

Status: **AUTOMATED VALIDATION PASS; MANUAL ACCEPTANCE PENDING.** The Level 5
candidate remains uncommitted. The game was not launched.

## Starting state and diagnosis

- Branch: `master`; upstream: `origin/master`.
- HEAD: `62c66e0ab19436ea162935627021154be21e0a41`.
- No staged changes. The existing candidate consisted of the six modified
  tracked files and two untracked admission-policy files documented by the
  prior validation handoff.
- The three requested historical handoffs were read. The candidate diff and
  exact declarations/call sites were inspected before changes.
- The prior fresh Release route compiled `Tempest.Game.dll` but failed in the
  test project before VSTest: `Flipper.ReloadShotDelay` at line 620,
  `FlipperTankerCargoRestorationSnapshot` at line 622,
  `Flipper.CaptureDeathRestorationSnapshot` at line 623, and
  `Flipper.RestoreTankerCargo` at line 624 were inaccessible. No tests
  executed in that attempt.
- `FlipperTankerCargoRestorationSnapshot`, `CaptureDeathRestorationSnapshot`,
  `RestoreTankerCargo`, and `ReloadShotDelay` are intentionally `internal`.
  The tests are in a separate assembly, and the project had no existing
  `InternalsVisibleTo` declaration. Making the members public would have
  expanded the production API unnecessarily.

## Correction

Added `src/Tempest.Game/InternalsVisibleTo.cs` with an assembly-level
`InternalsVisibleTo("Tempest.Game.Tests")`. This preserves the implementation
members' internal visibility while enabling the deterministic snapshot
regression to verify pivot, movement, shot-delay, and pending-zap state. No
gameplay code or behavior was changed for the compile fix.

The first attempt to replace the deterministic snapshot test with a runtime
firing scenario did not observe a firing delay before the cargo left the
active simulation; that rewrite was discarded. After restoring the
deterministic regression, its first execution exposed a test setup error:
Tanker release yields two cargo spawns, but the test asserted there was one.
The test now asserts the expected pair and selects one child for the
snapshot round-trip. This does not weaken the state-preservation assertions.

## Validation

- `git diff --check`: **PASS**.
- Corrected snapshot regression, isolated Release rerun: **PASS**, 1/1.
- Required three-test Release filter: production compilation **PASS**;
  test-project compilation **PASS**; VSTest **executed**; **3 passed, 0
  failed, 0 skipped**.
- Focused Level 5 production/admission/restoration and ledger groups
  (`Level5ProductionTests`, `Level5AdmissionPolicyTests`,
  `Level3WaveLedgerTests`, `Level3MixedWaveTests`): **84 passed, 0 failed,
  0 skipped**.
- Affected Release regressions covering Tanker cargo, death/reentry,
  ledger/identity conservation, terminal conversion, pending cargo, Super
  Zapper, completion/rim behavior, startup/recording, and Levels 1-4
  concurrency: **723 passed, 0 failed, 0 skipped**.
- Full solution Release tests with `--blame-hang-timeout 5min`: **1,167
  passed, 0 failed, 0 skipped**. VSTest executed tests.
- Full solution Release build: **PASS**, 0 warnings, 0 errors.
- No Windows Application Control block occurred during this validation.
  No security settings were changed or bypassed.

The initial exploratory runtime-test rewrite produced 2 passes and 1 test
failure because it did not create an observable shot-delay state; it did not
reveal a production restoration defect. The deterministic regression was
restored and then passed after correcting the two-spawn test setup.

## Final repository state

The original eight candidate paths remain, and the friend-assembly file is
the only new implementation path:

- `src/Tempest.Game/Gameplay/Flipper.cs`
- `src/Tempest.Game/Levels/Level.cs`
- `src/Tempest.Game/Levels/Level3WaveLedger.cs`
- `src/Tempest.Game/TempestGame.cs`
- `tests/Tempest.Game.Tests/Level3WaveLedgerTests.cs`
- `tests/Tempest.Game.Tests/Level5ProductionTests.cs`
- `src/Tempest.Game/Levels/Level5AdmissionPolicy.cs` (untracked)
- `tests/Tempest.Game.Tests/Level5AdmissionPolicyTests.cs` (untracked)
- `src/Tempest.Game/InternalsVisibleTo.cs` (untracked; compile-fix seam)

The complete candidate diff was reviewed. Final `git diff --check` passed;
no files are staged; HEAD remains
`62c66e0ab19436ea162935627021154be21e0a41`; no unrelated files or Windows
security configuration changed. The prompt and this report are under ignored
`AI-Logs/` and remain uncommitted. No commit or push was made.

## Manual handoff

Manual gameplay acceptance remains **PENDING**. Neal should launch:

`dotnet run --project src\Tempest.Game -- --level 5 --lives 10`

Verify Tanker-released cargo that remains unresolved across Blaster death
returns in-tube rather than appearing from the deep end; compare presentation
with the arcade, and check ordinary Tanker/Spiker entry, normal cargo release,
death/reentry, Spikes, Super Zapper, rim combat, and completion. Repeat after
another death if practical. No manual acceptance is claimed.
