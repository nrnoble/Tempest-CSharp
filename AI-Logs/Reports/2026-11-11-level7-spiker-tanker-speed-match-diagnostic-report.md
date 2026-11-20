# Level 7 Spiker/Tanker speed-match diagnostic

Date: 2026-11-11
Status: IMPLEMENTATION AND AUTOMATED VALIDATION COMPLETE; MANUAL ACCEPTANCE PENDING NEAL

## Scope and starting state

This adds an opt-in, development-only Level 7 Spiker movement experiment. It
does not select a production speed or change normal gameplay. The complete
substantive task is saved in the paired
[prompt record](../Prompts/2026-11-11-level7-spiker-tanker-speed-match-diagnostic-prompt.md).

Starting Git state:

- Branch: `master`, tracking `origin/master`
- HEAD: `8f1c29a7d3e6ef4150c8ab64264108c3c2a9547b`
- Index: clean; no staged changes
- Starting tracked diff: 17 files, 1,179 insertions, 186 deletions

The current source/test inventory matched the final status in the required
preceding
[expanded Level 7 diagnostic checkpoint](./2026-11-11-level7-expanded-spiker-speed-and-slow-recording-diagnostic-report.md).
The accumulated Level 7 candidate and the unrelated `.github/copilot-instructions.md`
maintenance change were preserved. No changes were staged, committed, pushed,
reset, restored, cleaned, checked out, or discarded.

Starting `git status --short --branch`:

```text
## master...origin/master
 M .github/copilot-instructions.md
 M src/Tempest.Game/Debug/DebugController.cs
 M src/Tempest.Game/Debug/GameplayRecordingFile.cs
 M src/Tempest.Game/Debug/GameplayStartupConfiguration.cs
 M src/Tempest.Game/Gameplay/Spiker.cs
 M src/Tempest.Game/Levels/Level3WaveLedger.cs
 M src/Tempest.Game/Levels/Level7WaveSelection.cs
 M src/Tempest.Game/Levels/LevelConfiguration.cs
 M src/Tempest.Game/Program.cs
 M src/Tempest.Game/TempestGame.cs
 M tests/Tempest.Game.Tests/DebugControllerTests.cs
 M tests/Tempest.Game.Tests/GameplayRecordingTests.cs
 M tests/Tempest.Game.Tests/InputDiagnosticTests.cs
 M tests/Tempest.Game.Tests/Level3WaveLedgerTests.cs
 M tests/Tempest.Game.Tests/Level7FlipperMovementTests.cs
 M tests/Tempest.Game.Tests/Level7ProductionTests.cs
 M tests/Tempest.Game.Tests/SimulationTests.cs
?? tests/Tempest.Game.Tests/Level7AdmissionPolicyTests.cs
?? tests/Tempest.Game.Tests/Level7SpikerFiringTests.cs
?? tests/Tempest.Game.Tests/Level7SpikerProjectileRelativeSpeedDiagnosticTests.cs
```

## Inspected implementation and findings

Inspected the checkpoint report; `Tanker.cs`, `EnemyBullet.cs`, `Spiker.cs`,
`TempestGame.cs`, `LevelConfiguration.cs`, and `Level7WaveSelection.cs`;
`DebugController.cs`, `GameplayStartupConfiguration.cs`,
`GameplayRecordingFile.cs`, and `Program.cs`; and the related Level 7,
Tanker, enemy-bullet, command-line, recording, playback, and simulation tests.

### Tanker movement

- `Tanker.DevelopmentClimbSpeed` is the fixed C# movement value **1.75 depth
  units per simulation step**. The Tanker constructor permits an explicit
  override, but the Level 7 production creation path supplies none and uses
  this default.
- `UpdateForDevelopment` advances a climbing Tanker by `ClimbSpeed` once per
  simulation step. Tankers start at height 10 and continue climbing until
  crossing `Level.Depth - 15`, then enter cargo release processing. There is
  no separate climbing rate by level, random per-enemy speed selection, or
  second movement phase.
- The comparable behavior is a Tanker in its `Climbing` state and an ascending
  Spiker in `Building`. The experiment uses the existing
  `Tanker.DevelopmentClimbSpeed` constant directly.
- The constant is documented in C# as Incitti-derived. It is not a measured
  ROM value and is not asserted to be arcade-correct.

### Existing Spiker and projectile movement

- Normal Level 7 Spiker selection remains
  `ProgressionIndex / 48f + 2.5f + Random.NextDouble() * 2`; with Level 7's
  progression index of 6, this is the existing randomized range beginning at
  2.625. The existing numeric `--level7-spiker-speed` override remains
  available and unchanged at 1.000 through 3.000.
- `--level7-spiker-match-tanker-speed` selects **1.75** for Level 7 Spiker
  movement only. The diagnostic avoids the otherwise-used random speed draw.
  At other levels it does not select a Tanker speed or alter the Spiker's
  existing selection.
- Tanker shots and Level 7 Spiker shots already construct the same
  `EnemyBullet` type. `EnemyBullet.Advance()` increases height by 3 units per
  simulation step. Existing game update order advances each live projectile
  once per fixed simulation step; shots created later in an update first move
  on the following step. No projectile implementation, displacement, or
  cadence was changed.
- The `EnemyBullet` summary comment was corrected to describe its shared use.
  The only change in that file is documentation.

### Diagnostic and recording semantics

- The new boolean startup state is `Level7SpikerMatchesTankerSpeed`. It is
  mutually exclusive with the numeric Spiker override in command parsing,
  startup configuration, and game construction.
- The option may appear once. Playback rejects either direct Spiker diagnostic
  option because startup identity is restored from the recording.
- Version-2 recording metadata now has an optional
  `Level7SpikerMatchesTankerSpeed` boolean. False is omitted from new files;
  older version-2 recordings without the property deserialize to false.
  Version-1 recordings and the format version are unchanged. Playback restores
  the relationship experiment automatically.
- `--speed` remains presentation pacing only and is not recorded. The half-
  speed experiment uses `--speed 0.5` independently of startup metadata.
- Repeated-cycle Spiker firing, firing holdoff, shot creation, Tanker movement,
  and projectile behavior were not changed by the diagnostic.

## Evidence classification

- **MAME/ARCADE MANUAL OBSERVATION:** Neal reports qualitatively matching
  Spiker/Tanker movement and Spiker-shot/Tanker-shot movement, with a Spiker
  shot pulling ahead of its firing Spiker. This is not a numerical speed
  measurement.
- **MANUAL C# CALIBRATION RESULT:** Normal/current Level 7 Spiker movement and
  fixed values 3.000, 2.8125, and 2.625 looked too fast.
- **MANUAL C# OBSERVATION:** At fixed Spiker speed 3.000, the Spiker and its
  shot moved together; reversal produced the rejected visual “release” effect.
- **C# MEASURED:** Level 7 production Tankers use a fixed 1.75 climb rate;
  Tanker and Spiker shots use the same `EnemyBullet` movement implementation
  at 3 units per simulation step.
- **PROJECT TEST HYPOTHESIS:** Matching the Level 7 Spiker movement to the
  existing Level 7 Tanker movement may reproduce Neal's qualitative
  arcade/MAME relationship.
- **PROJECT DECISION:** None. No production behavior or permanent Spiker
  speed was selected.

## Files changed specifically for this diagnostic

- `src/Tempest.Game/Debug/DebugController.cs`
- `src/Tempest.Game/Debug/GameplayRecordingFile.cs`
- `src/Tempest.Game/Debug/GameplayStartupConfiguration.cs`
- `src/Tempest.Game/Gameplay/EnemyBullet.cs` (summary comment only)
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/TempestGame.cs`
- `tests/Tempest.Game.Tests/DebugControllerTests.cs`
- `tests/Tempest.Game.Tests/GameplayRecordingTests.cs`
- `tests/Tempest.Game.Tests/Level7ProductionTests.cs`
- `tests/Tempest.Game.Tests/Level7SpikerFiringTests.cs`
- `tests/Tempest.Game.Tests/Level7SpikerProjectileRelativeSpeedDiagnosticTests.cs`

Other modified or untracked candidate files shown in the final Git status were
pre-existing and were not changed for this diagnostic. The
`.github/copilot-instructions.md` maintenance change also predates this task.

## Automated validation

Test platform: VSTest with xUnit v2 on .NET SDK 9.0.304.

Focused parsing, Level 7 Spiker movement/firing/production/admission, Tanker,
EnemyBullet, recording/playback, and simulation tests:

```powershell
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~DebugControllerTests|FullyQualifiedName~InputDiagnosticTests|FullyQualifiedName~InteractiveSpeedOptionsTests|FullyQualifiedName~GameplayRecordingTests|FullyQualifiedName~GameplayPlaybackSessionTests|FullyQualifiedName~GameplayPlaybackTests|FullyQualifiedName~HeadlessGameplayPlaybackTests|FullyQualifiedName~Level7SpikerProjectileRelativeSpeedDiagnosticTests|FullyQualifiedName~Level7SpikerFiringTests|FullyQualifiedName~Level7ProductionTests|FullyQualifiedName~Level7FlipperMovementTests|FullyQualifiedName~Level7AdmissionPolicyTests|FullyQualifiedName~SimulationTests|FullyQualifiedName~Tanker|FullyQualifiedName~EnemyBullet" --verbosity minimal
```

Result: **PASS**, 607 passed, 0 failed, 0 skipped.

Level 7 regression subset:

```powershell
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-build --no-restore --filter "FullyQualifiedName~Level7" --verbosity minimal
```

Result: **PASS**, 96 passed, 0 failed, 0 skipped.

Recording/playback regression subset:

```powershell
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-build --no-restore --filter "FullyQualifiedName~GameplayRecording|FullyQualifiedName~GameplayPlayback|FullyQualifiedName~HeadlessGameplayPlayback" --verbosity minimal
```

Result: **PASS**, 97 passed, 0 failed, 0 skipped.

Solution build:

```powershell
dotnet build TempestCSharp.slnx --no-restore
```

Result: **PASS**, 0 warnings, 0 errors.

`git diff --check`: **PASS**, no whitespace errors.

An initial focused run caught an output-state issue on repeated-option parse
failure. The parser was corrected; the final focused and regression runs above
all pass.

## Manual recording and playback commands

Run from the repository root in PowerShell. The recording captures the
Tanker-match diagnostic; the playback command restores it from metadata while
selecting half-speed presentation independently.

Record:

```powershell
dotnet run --project src\Tempest.Game -- --level 7 --lives 10 --speed 0.5 --record-gameplay Temp\GameplayRecordings\lvl7-SpikerTankerMatch.json --level7-spiker-match-tanker-speed
```

Playback:

```powershell
dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\lvl7-SpikerTankerMatch.json --speed 0.5
```

## Final status and acceptance

Final `git status --short --branch`:

```text
## master...origin/master
 M .github/copilot-instructions.md
 M src/Tempest.Game/Debug/DebugController.cs
 M src/Tempest.Game/Debug/GameplayRecordingFile.cs
 M src/Tempest.Game/Debug/GameplayStartupConfiguration.cs
 M src/Tempest.Game/Gameplay/EnemyBullet.cs
 M src/Tempest.Game/Gameplay/Spiker.cs
 M src/Tempest.Game/Levels/Level3WaveLedger.cs
 M src/Tempest.Game/Levels/Level7WaveSelection.cs
 M src/Tempest.Game/Levels/LevelConfiguration.cs
 M src/Tempest.Game/Program.cs
 M src/Tempest.Game/TempestGame.cs
 M tests/Tempest.Game.Tests/DebugControllerTests.cs
 M tests/Tempest.Game.Tests/GameplayRecordingTests.cs
 M tests/Tempest.Game.Tests/InputDiagnosticTests.cs
 M tests/Tempest.Game.Tests/Level3WaveLedgerTests.cs
 M tests/Tempest.Game.Tests/Level7FlipperMovementTests.cs
 M tests/Tempest.Game.Tests/Level7ProductionTests.cs
 M tests/Tempest.Game.Tests/SimulationTests.cs
?? tests/Tempest.Game.Tests/Level7AdmissionPolicyTests.cs
?? tests/Tempest.Game.Tests/Level7SpikerFiringTests.cs
?? tests/Tempest.Game.Tests/Level7SpikerProjectileRelativeSpeedDiagnosticTests.cs
```

- Normal Level 7 production Spiker-speed selection is unchanged when the
  option is absent.
- Level 7 production Tanker movement is unchanged.
- No production projectile behavior or projectile speed was changed.
- No permanent Spiker speed was selected; this is a development-only
  experiment, not an accepted production decision.
- No commit or push occurred.
- Interactive Tempest was not launched.
- Manual gameplay acceptance: **PENDING NEAL**.

Implementation and automated validation are **COMPLETE**. Stop here for
Neal's manual half-speed recorded comparison.
