# Level 15 terminal-Spiker lifecycle correction

Date: 2026-11-24

## Status

**AUTOMATED VALIDATION PASS. MANUAL TERMINAL-SPIKER ACCEPTANCE PENDING.**

Level 15 remains a direct-start/development-only candidate. No campaign,
movement, ordinary enemy-selection, tube-presentation, or unrelated gameplay
work was performed. Nothing was staged, committed, or pushed.

## Starting repository state

- Branch `master`, HEAD `045aeb240fce91a3eaacf9424da30cd7359927bf`.
- Upstream `origin/master`; ahead/behind `0/0`.
- Index empty.
- Tracked worktree modifications at start:
  - `AI-Logs/Reports/2026-11-24-background-opacity-closeout-report.md`
  - `TEMPEST-BEHAVIOR-SPEC.md`
  - `TEMPEST-DEVELOPMENT-NOTES.md`
  - `src/Tempest.Game/Levels/LevelConfiguration.cs`
  - `src/Tempest.Game/Levels/RomPresentationMetadata.cs`
  - `src/Tempest.Game/Program.cs`
  - `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`
  - `src/Tempest.Game/Rendering/AtariPresentationCalibration.cs`
  - `tests/Tempest.Game.Tests/AtariPresentationCalibrationTests.cs`
  - `tests/Tempest.Game.Tests/Level15TubeCandidateTests.cs`
- Pre-existing untracked records included the prior Level 15 prompts/reports
  and the authorized read-only handoff records:
  - Prompts: `2026-11-24-level15-baseline-spike-reconciliation-prompt.md`,
    `2026-11-24-level15-spike-arcade-evidence-correction-prompt.md`,
    `2026-11-24-level15-spike-height-correction-prompt.md`, and
    `2026-11-24-level15-terminal-spiker-readonly-prompt.md`.
  - Reports: `2026-11-24-level15-baseline-spike-reconciliation-report.md`,
    `2026-11-24-level15-spike-arcade-evidence-correction-report.md`,
    `2026-11-24-level15-spike-height-correction-report.md`, and
    `2026-11-24-level15-terminal-spiker-readonly-report.md`.
  `Reference/arcade/MBUCOD.V05.txt` and
  `temo/gameplayrecordings/level-13` were left unopened and unchanged.
- No pre-existing changes were reverted or absorbed. The only source/test
  changes for this correction are listed below.

## Handoff and cause

Used
`AI-Logs/Reports/2026-11-24-level15-terminal-spiker-readonly-report.md`
as the primary factual handoff; no repeat disassembly investigation was
performed. It classifies this as an existing shared terminal-Spiker mechanism
not wired for the Level 15 candidate, **not an established regression**.

Level 15 intentionally retains
`UsesLevel11ParameterDevelopmentBaseline = true` and
`UsesLevel11RomWavePolicy = false`. The existing C# terminal path did not
include the former when reading generic pending supply, restarting the same
Spiker with pending originals, or converting the same active identity at zero
pending. The generic fallback could return the Spiker's original to Reserve,
recreate pending supply, and permit reselection. The correction does not change
Spiker movement or the accepted C# return-boundary approximation.

## Changes and preserved behavior

- `src/Tempest.Game/TempestGame.cs`: added the development-baseline flag to
  the existing generic pending count eligibility, Level-11-equivalent
  pending-positive restart, and zero-pending in-place conversion predicate.
  The existing `ConvertActiveSpikerToTanker` ledger path and Tanker
  construction are reused.
- `tests/Tempest.Game.Tests/Level15TerminalSpikerTests.cs`: added five
  deterministic, identity-specific cases for restart, same-ID conversion and
  cargo, ordinary Tanker capacity bypass, unrelated active enemies, and
  completion/all-rim obligations.
- `AI-Logs/Prompts/2026-11-24-level15-terminal-spiker-correction-prompt.md`:
  saved the task scope, constraints, validation requirements, manual handoff,
  and completion token.

Tests establish that pending-positive return keeps the original as
`Spiker/ActiveSpiker`, restarts that same runtime object, creates no Tanker,
and does not inflate pending count. At zero pending, the identified original
becomes `Tanker/ActiveTanker`; no Spiker with that identity remains; exactly
two distinct latent cargo IDs are owned by it with ordinals 0 and 1; and
original/pending counts remain unchanged. The Tanker-capacity case converts
while the ordinary Level 11-based maximum is already occupied. The unrelated
enemy case keeps another Spiker and Tanker active during conversion.

Runtime ledger representation validation is retained and exercised. The
converted original and latent cargo remain unresolved; normal completion and
all-rim completion remain blocked. Existing Level 11 terminal tests were not
changed.

The accepted `$A0` starting-Spike correction, including its raw evidence,
height translation, count, lane placement, and lifecycle coverage, was not
modified. The accepted Level 15 presentation calibration remains Y gain
`1.15`, scale `1`, vertical offset `75`, and depth-Y gain `1`; calibration
source/tests were not modified. Level 15's candidate flags and direct-only
progression status were not changed.

## Validation

All commands below completed; none was blocked, skipped, or timed out.

1. Focused new tests (first attempt, before fixture adjustment):

   ```powershell
   dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --configuration Release --no-restore --filter "FullyQualifiedName~Level15TerminalSpikerTests" --blame-hang-timeout 2min
   ```

   **FAIL:** 3 passed, 2 failed. The accelerated fixture overshot the rim and
   entered the existing all-rim path before reaching the terminal return
   decision. This was a test-fixture issue, not a production failure.

2. Focused new tests, after seeding the same Spiker just before the return
   boundary and running the regular update path (same command):

   **PASS:** 5 passed, 0 failed, 0 skipped.

3. Focused Level 15, Level 11, presentation, ledger, completion/reconstruction,
   Level 4-9, and production Level 12-14 regression matrix:

   ```powershell
   dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --configuration Release --no-restore --filter "FullyQualifiedName~Level15TerminalSpikerTests|FullyQualifiedName~Level15TubeCandidateTests|FullyQualifiedName~AtariPresentationCalibrationTests|FullyQualifiedName~Level11ProductionCorrectionTests|FullyQualifiedName~Level3WaveLedgerTests|FullyQualifiedName~Level3MixedWaveTests|FullyQualifiedName~Level3MixedAdmissionPolicyTests|FullyQualifiedName~Level4|FullyQualifiedName~Level5|FullyQualifiedName~Level6|FullyQualifiedName~Level7|FullyQualifiedName~Level8|FullyQualifiedName~Level9|FullyQualifiedName~Level12ProductionTests|FullyQualifiedName~Level13ProductionTests|FullyQualifiedName~Level14ProductionTests|FullyQualifiedName~AllRim|FullyQualifiedName~Reconstruct" --blame-hang-timeout 3min
   ```

   **PASS:** 520 passed, 0 failed, 0 skipped. The Level 15 candidate class
   includes the seeded 1,000-tick headless simulation; that broad simulation
   does not itself establish terminal conversion. The new explicit tests
   establish the endpoint lifecycle.

4. Complete Release solution test suite:

   ```powershell
   dotnet test TempestCSharp.slnx --configuration Release --no-restore --blame-hang-timeout 5min
   ```

   **PASS:** 1,966 passed, 0 failed, 0 skipped.

5. Release solution build:

   ```powershell
   dotnet build TempestCSharp.slnx --configuration Release --no-restore
   ```

   **PASS:** 0 warnings, 0 errors.

6. `git diff --check` was run twice, including after validation.

   **PASS** both times.

## Manual acceptance and remaining action

No interactive game was launched. Manual terminal-Spiker acceptance remains
pending. Neal should run:

```powershell
dotnet run --project src\Tempest.Game -- --atari-presentation --level 15 --tube-y-gain 1.15 --tube-scale 1 --tube-vertical-offset 75 --tube-depth-y-gain 1
```

Verify that starting Spikes remain correct; Spikers behave normally while
generic supply remains; terminal Spikers convert to Tankers rather than cycle
forever; Tankers release cargo normally; wave completion remains normal;
Level 15 gameplay still passes; accepted tube presentation is unchanged; and
no new regression is visible.

No Level 16 work began. No unrelated gameplay or presentation work was
performed. No files were staged, committed, or pushed.
