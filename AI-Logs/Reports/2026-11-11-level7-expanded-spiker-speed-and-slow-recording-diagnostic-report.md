# Expanded Level 7 Spiker speed and slow recording diagnostic

Date: 2026-11-11
Status: IMPLEMENTATION AND AUTOMATED VALIDATION COMPLETE; MANUAL ACCEPTANCE PENDING NEAL

## Scope and starting state

This was a development-only calibration and recording milestone. The complete
substantive task was saved to
[the paired prompt record](../Prompts/2026-11-11-level7-expanded-spiker-speed-and-slow-recording-diagnostic-prompt.md).

Starting branch and revision:

- Branch: `master`, tracking `origin/master`
- HEAD: `8f1c29a7d3e6ef4150c8ab64264108c3c2a9547b`
- Index: clean; no staged changes

The starting source/test inventory matched the final candidate inventory in
the preceding
[manual calibration report](./2026-11-11-level7-spiker-speed-manual-calibration-experiment-report.md).
The only additional tracked change was the expected
`.github/copilot-instructions.md` maintenance change, which was preserved.
No candidate file was reset, restored, cleaned, checked out, staged, committed,
or discarded.

Starting `git status --short --branch`:

```text
## master...origin/master
 M .github/copilot-instructions.md
 M src/Tempest.Game/Debug/DebugController.cs
 M src/Tempest.Game/Gameplay/Spiker.cs
 M src/Tempest.Game/Levels/Level3WaveLedger.cs
 M src/Tempest.Game/Levels/Level7WaveSelection.cs
 M src/Tempest.Game/Levels/LevelConfiguration.cs
 M src/Tempest.Game/Program.cs
 M src/Tempest.Game/TempestGame.cs
 M tests/Tempest.Game.Tests/DebugControllerTests.cs
 M tests/Tempest.Game.Tests/Level3WaveLedgerTests.cs
 M tests/Tempest.Game.Tests/Level7FlipperMovementTests.cs
 M tests/Tempest.Game.Tests/Level7ProductionTests.cs
 M tests/Tempest.Game.Tests/SimulationTests.cs
?? tests/Tempest.Game.Tests/Level7AdmissionPolicyTests.cs
?? tests/Tempest.Game.Tests/Level7SpikerFiringTests.cs
?? tests/Tempest.Game.Tests/Level7SpikerProjectileRelativeSpeedDiagnosticTests.cs
```

## Inspected architecture and evidence

Inspected the preceding calibration report; `.github/copilot-instructions.md`;
`Program.cs`, `DebugController.cs`, `TempestGame.cs`,
`GameplayStartupConfiguration.cs`, `GameplayRecordingFile.cs`,
`PlayerInputRecorder.cs`, `GameplayPlaybackSession.cs`,
`InteractiveSpeedOptions.cs`; and the related CLI, Level 7, recording,
playback, and simulation tests.

The interactive host loop performs one `RunOneStep` and one render per loop
iteration. `TempestGame.SimulationStepSeconds` remains 1/60 second, and
`Raylib.SetTargetFPS` controls the loop's presentation pacing. Thus
`--speed 0.5` presents the same fixed-step simulation at a 30 FPS target;
`--speed 1.0` uses the normal 60 FPS target. No gameplay speed, timer,
collision, or simulation-state value is scaled.

Recordings contain sequential simulation-tick inputs and deterministic startup
metadata, not wall-clock pacing. Playback reconstructs a game from that startup
metadata and consumes one recorded input per simulation step. This supports a
separate presentation-speed choice during playback.

## Implementation

### Level 7 Spiker calibration

Expanded the development-only `--level7-spiker-speed <value>` diagnostic range
to **1.000 through 3.000 inclusive**. It uses invariant-culture numeric
parsing; malformed, non-finite, repeated, below-range, and above-range values
are rejected. Values are ordinary numeric inputs, not separate modes.

The override remains confined to Spiker movement on the Level 7 production
wave. With no override, the existing randomized production formula and random
draw are unchanged. The enemy-projectile speed, Spiker firing behavior,
ammunition, and other gameplay systems were not changed.

### Presentation speed

`--speed` already existed as a presentation-only development control, so its
existing implementation and accepted range (**0.25 through 4.0 inclusive**)
were retained. It parses finite invariant-culture numbers, rejects malformed,
non-positive, out-of-range, or repeated values, and maps the multiplier to a
rounded integer target FPS. The supported required values map to 30 FPS at
`0.5` and 60 FPS at `1.0`.

The multiplier remains outside recording metadata. It affects neither
tick-indexed inputs nor deterministic gameplay startup/simulation state.
Recordings and playback may independently select presentation speed.
Automated comparison of identical 120-tick input sequences produced equivalent
game state and recorded frames at the 0.5 and 1.0 pacing selections.

### Recording startup metadata and compatibility

The fixed Spiker diagnostic changes deterministic startup/simulation behavior:
it selects Spiker speed and suppresses the production random speed draw.
Therefore recording it only as a command-line option would not reproduce the
same simulation. The recording now stores the selected value as the optional
`Level7SpikerSpeedOverride` startup field.

The field was added to the existing version-2 format without a format-version
bump. When absent, it defaults to `null`; recordings without the diagnostic
retain their prior startup behavior, and version-1 recordings remain unchanged.
When null, the field is omitted from newly saved JSON. Invalid stored values
are rejected. Playback loads the recorded startup value and applies it
automatically; specifying `--level7-spiker-speed` alongside `--playback-file`
is rejected to avoid overriding restored simulation identity. Playback may
still use its own `--speed`.

The file-mode parser now accepts the recording selector alongside compatible
startup and pacing options, including the requested order where
`--record-gameplay <file>` follows `--level`, `--lives`, and `--speed`.

### Files changed specifically for this implementation

- `src/Tempest.Game/Debug/DebugController.cs`
- `src/Tempest.Game/Debug/GameplayRecordingFile.cs`
- `src/Tempest.Game/Debug/GameplayStartupConfiguration.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/TempestGame.cs`
- `tests/Tempest.Game.Tests/DebugControllerTests.cs`
- `tests/Tempest.Game.Tests/GameplayRecordingTests.cs`
- `tests/Tempest.Game.Tests/InputDiagnosticTests.cs`
- `tests/Tempest.Game.Tests/Level7SpikerProjectileRelativeSpeedDiagnosticTests.cs`

The last test file was already present as an untracked Level 7 candidate and
was extended, not replaced. Other pre-existing Level 7 candidate files were
left unchanged. The `.github/copilot-instructions.md` change is the prior
maintenance change, not part of this implementation.

## Evidence classification and calibration status

- **MAME-OBSERVED:** A Level 7 Spiker projectile visibly pulls away from its
  firing Spiker.
- **MANUAL C# CALIBRATION RESULT:** Neal judged normal/current, 3.000, 2.8125,
  and 2.625 Spiker movement too fast.
- **PROJECT TEST HYPOTHESIS:** A Level 7 Spiker speed below 2.625 may better
  match arcade/MAME.
- **C# MEASURED:** Automated tests verify parser boundaries, deterministic
  selection, projectile movement, metadata round-trip, and tick-based
  simulation invariants.
- **PROJECT DECISION:** None. No production Spiker speed was selected.

No new diagnostic value is claimed as ROM-backed, arcade-correct, or
MAME-correct.

## Automated validation

Focused parsing, Level 7 movement/firing/production, recording/playback, and
simulation command:

```powershell
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --filter "FullyQualifiedName~DebugControllerTests|FullyQualifiedName~InputDiagnosticTests|FullyQualifiedName~InteractiveSpeedOptionsTests|FullyQualifiedName~GameplayRecordingTests|FullyQualifiedName~GameplayPlaybackSessionTests|FullyQualifiedName~GameplayPlaybackTests|FullyQualifiedName~HeadlessGameplayPlaybackTests|FullyQualifiedName~Level7SpikerProjectileRelativeSpeedDiagnosticTests|FullyQualifiedName~Level7SpikerFiringTests|FullyQualifiedName~Level7ProductionTests|FullyQualifiedName~Level7FlipperMovementTests|FullyQualifiedName~Level7AdmissionPolicyTests|FullyQualifiedName~SimulationTests" --verbosity minimal
```

Result: **PASS**, 524 passed, 0 failed, 0 skipped.

Level 7 regression command:

```powershell
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-build --filter "FullyQualifiedName~Level7" --verbosity minimal
```

Result: **PASS**, 85 passed, 0 failed, 0 skipped.

Recording/playback regression command:

```powershell
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-build --filter "FullyQualifiedName~GameplayRecording|FullyQualifiedName~GameplayPlayback|FullyQualifiedName~HeadlessGameplayPlayback" --verbosity minimal
```

Result: **PASS**, 95 passed, 0 failed, 0 skipped.

Solution build:

```powershell
dotnet build TempestCSharp.slnx --no-restore
```

Result: **PASS**, 0 warnings, 0 errors.

`git diff --check`: **PASS**, no output.

No interactive Tempest game was launched.

## Manual commands for Neal

Run from the repository root in PowerShell. Each recording command uses the
requested argument order and creates a new file; playback uses the recorded
Spiker speed and a separately selected half-speed presentation.

**Normal-speed fixed Spiker control**

```powershell
dotnet run --project src\Tempest.Game -- --level 7 --lives 10 --level7-spiker-speed 2.0
```

**Half-speed recording at Spiker 1.0**

```powershell
dotnet run --project src\Tempest.Game -- --level 7 --lives 10 --speed 0.5 --record-gameplay Temp\GameplayRecordings\lvl7-SpikerTest-1.0.json --level7-spiker-speed 1.0
```

Playback:

```powershell
dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\lvl7-SpikerTest-1.0.json --speed 0.5
```

**Half-speed recording at Spiker 1.5**

```powershell
dotnet run --project src\Tempest.Game -- --level 7 --lives 10 --speed 0.5 --record-gameplay Temp\GameplayRecordings\lvl7-SpikerTest-1.5.json --level7-spiker-speed 1.5
```

Playback:

```powershell
dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\lvl7-SpikerTest-1.5.json --speed 0.5
```

**Half-speed recording at Spiker 2.0**

```powershell
dotnet run --project src\Tempest.Game -- --level 7 --lives 10 --speed 0.5 --record-gameplay Temp\GameplayRecordings\lvl7-SpikerTest-2.0.json --level7-spiker-speed 2.0
```

Playback:

```powershell
dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\lvl7-SpikerTest-2.0.json --speed 0.5
```

**Half-speed recording at Spiker 2.5**

```powershell
dotnet run --project src\Tempest.Game -- --level 7 --lives 10 --speed 0.5 --record-gameplay Temp\GameplayRecordings\lvl7-SpikerTest-2.5.json --level7-spiker-speed 2.5
```

Playback:

```powershell
dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\lvl7-SpikerTest-2.5.json --speed 0.5
```

## Final status and remaining acceptance

Final `git status --short --branch`:

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

The pre-existing Level 7 candidate changes remain in the worktree. The
`.github/copilot-instructions.md` change remains the prior maintenance change.
This implementation did not change enemy-projectile speed or the normal
Level 7 production Spiker-speed selection. No permanent Spiker speed was
selected, and Stage 2 value swapping was not performed.

Implementation and automated validation are **COMPLETE**. Manual gameplay
acceptance is **PENDING NEAL**. Neal's manual calibration remains the next
step; no manual gameplay acceptance was performed in this task.
