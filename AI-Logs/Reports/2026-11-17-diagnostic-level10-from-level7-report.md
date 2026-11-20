# Diagnostic Level 10 from Level 7 — implementation report

Date: 2026-11-17

## Outcome

**Implementation and automated validation: PASS. Manual acceptance: PENDING.**

Diagnostic Level 10 is available as a direct, deterministic Level 7 clone for the requested Atari-presentation comparison. No interactive game was launched. No conclusion about the Level 7 motion defect is possible until Neal performs the comparison.

## Purpose and design

Level 10 exists only to test whether the same apparent depth-dependent enemy-motion defect occurs when the current Level 7 path runs under `--atari-presentation`. It is not an arcade-authentic Level 10 implementation.

`LevelConfiguration.Level10Diagnostic` clones `Level7Production` and changes only the displayed level identity. The clone retains Level 7's definition, indices, supply, wave/timing values, enemy composition, spikes, firing configuration, movement policy, and Level 7 wave selection. Direct Level 10 startup is kept out of `GetProductionLevel` and the existing Level 1–9 campaign chain. It uses normal production simulation behavior, but restart after completion or game over stays on the diagnostic Level 10 rather than advancing or returning to Level 1.

The Atari-presentation model-frame mapping and metadata for displayed Level 10 deliberately alias Level 7's path and values. The renderer routes Level 10 through the existing Level 7 candidate while Atari presentation is enabled. These aliases are development-only and make no claim about arcade Level 10. Level 7-only tube prototypes, framebuffer capture, and visible-runtime diagnostics remain restricted to Level 7.

The existing recording format remains unchanged. Its startup level field now accepts Level 10, so one recorded input can be replayed with Atari presentation off and on.

## Changed files

- `src/Tempest.Game/Debug/DebugController.cs`
- `src/Tempest.Game/Debug/GameplayPlaybackSession.cs`
- `src/Tempest.Game/Debug/GameplayStartupConfiguration.cs`
- `src/Tempest.Game/Levels/Level7WaveSelection.cs`
- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/Levels/LevelProgression.cs`
- `src/Tempest.Game/Levels/RomPresentationMetadata.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `src/Tempest.Game/TempestGame.cs`
- `tests/Tempest.Game.Tests/DebugControllerTests.cs`
- `tests/Tempest.Game.Tests/DiagnosticLevel10Tests.cs`
- `tests/Tempest.Game.Tests/DirectLevelStartupTests.cs`
- `tests/Tempest.Game.Tests/GameplayRecordingTests.cs`
- `tests/Tempest.Game.Tests/InputDiagnosticTests.cs`
- `tests/Tempest.Game.Tests/RomPresentationMetadataTests.cs`

Several of the listed files had pre-existing unstaged Level 7 investigation/presentation changes before this task. Those changes were preserved. Edits to those existing files were limited to additive Level 10 routing or updating assertions that had treated Level 10 as unsupported.

## Unavoidable differences and boundaries

- The HUD, window title, and recording startup metadata identify the diagnostic run as Level 10; this is necessary for Neal to distinguish it.
- The diagnostic uses Level 7's tube metadata and model mapping, not independently established Level 10 presentation data.
- Level 10 is a direct-only diagnostic, is absent from production progression, and repeats itself after completion or game-over restart.
- The Level 7 implementation and Atari presentation architecture were not tuned or refactored. Level 7 diagnostic capture and prototype selectors remain Level-7-only.
- Durable behavior specifications were not changed.

## Validation

Environment: .NET SDK 9.0.304; test project uses VSTest/xUnit.

Final focused command:

```powershell
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~DiagnosticLevel10Tests|FullyQualifiedName~DirectLevelStartupTests|FullyQualifiedName~DebugControllerTests|FullyQualifiedName~GameplayRecordingTests|FullyQualifiedName~RomPresentationMetadataTests|FullyQualifiedName~AtariPresentationAdapterTests|FullyQualifiedName~AtariLevel7EntityPresentationTests" --blame-hang-timeout 4m --verbosity minimal
```

**PASS** — 188 passed, 0 failed, 0 skipped.

Final full regression command:

```powershell
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --blame-hang-timeout 5m --verbosity minimal
```

**PASS** — 1,594 passed, 0 failed, 0 skipped.

Release build:

```powershell
dotnet build TempestCSharp.slnx --no-restore --configuration Release --verbosity minimal
```

**PASS** — build succeeded; 0 warnings, 0 errors.

Whitespace validation:

```powershell
git diff --check
```

**PASS** — no whitespace errors. Git emitted existing LF-to-CRLF working-copy warnings for `LevelLoader.cs` and `InputSystem.cs`.

Earlier focused/full attempts exposed existing test assertions that rejected Level 10 as unsupported. The corresponding unsupported-level cases were moved to Level 11; reruns passed. No validation was blocked or incomplete.

## Manual acceptance and remaining action

Manual Atari-presentation ON/OFF observation remains **PENDING**. Neal must determine whether Level 10 reproduces the characteristic Level 7 motion defect. No conclusion about that defect should be drawn before this comparison.

From the repository root, record one input run and close the window normally to save it:

```powershell
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --record-gameplay level10-motion-comparison.json --level 10
```

Replay the same input without and with Atari presentation:

```powershell
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --playback-file level10-motion-comparison.json
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --playback-file level10-motion-comparison.json --atari-presentation
```

The bare recording filename resolves under `UserData\GameplayRecordings`. Use a new filename if that recording already exists.

## Git status

Branch remains `master` tracking `origin/master` at starting HEAD `385f603a94391756b2ca6cccd524dc17b281e782`. No files were staged, and no commit or push was made. The pre-existing unstaged Level 7 investigation work remains in the worktree alongside the task changes; it was not reverted.
