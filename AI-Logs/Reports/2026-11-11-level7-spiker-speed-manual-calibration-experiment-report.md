# Level 7 Spiker-speed manual calibration experiment

Date: 2026-11-11
Status: DEVELOPMENT-ONLY CONTROLS IMPLEMENTED; MANUAL ACCEPTANCE PENDING NEAL

## Starting worktree and preservation

The initial worktree was on `master`, tracking `origin/master`. It contained nine modified tracked files and three untracked Level 7 test files. The three untracked files included `Level7SpikerProjectileRelativeSpeedDiagnosticTests.cs`; this differs from the preceding report's two-untracked-file summary, but the file was present before this task began. No existing candidate changes were reset, discarded, staged, or overwritten.

Initial `git status --short --branch`:

```text
## master...origin/master
 M src/Tempest.Game/Gameplay/Spiker.cs
 M src/Tempest.Game/Levels/Level3WaveLedger.cs
 M src/Tempest.Game/Levels/Level7WaveSelection.cs
 M src/Tempest.Game/Levels/LevelConfiguration.cs
 M src/Tempest.Game/TempestGame.cs
 M tests/Tempest.Game.Tests/Level3WaveLedgerTests.cs
 M tests/Tempest.Game.Tests/Level7FlipperMovementTests.cs
 M tests/Tempest.Game.Tests/Level7ProductionTests.cs
 M tests/Tempest.Game.Tests/SimulationTests.cs
?? tests/Tempest.Game.Tests/Level7AdmissionPolicyTests.cs
?? tests/Tempest.Game.Tests/Level7SpikerFiringTests.cs
?? tests/Tempest.Game.Tests/Level7SpikerProjectileRelativeSpeedDiagnosticTests.cs
```

The complete user task was copied verbatim from the read-only attachment into [the prompt record](../Prompts/2026-11-11-level7-spiker-speed-manual-calibration-experiment-prompt.md); its SHA-256 matched the source attachment. Both AI log records are ignored under `/AI-Logs/` and were not staged.

## Evidence and current implementation verified

Inspected the preceding [relative-speed diagnostic report](./2026-11-11-level7-spiker-projectile-relative-speed-calibration-diagnostic-report.md), `TempestGame`, `Spiker`, `EnemyBullet`, `DebugController`, `Program`, Level 7 configuration and production code, and the relevant Level 7 production, movement, admission, firing, startup, and diagnostic tests.

The current Level 7 Spiker creation path still selects its production speed as:

```csharp
ProgressionIndex / 48f + 2.5f + (float)(random.NextDouble() * 2)
```

For Level 7's progression index 6, this gives a nominal range of 2.625 up to (but not including) 4.625 depth units per active step. `Spiker.Update` advances an ascending Spiker by its selected `Speed`; retreat uses `Speed * .90f`. `EnemyBullet.Advance` still advances the projectile by exactly `+3` depth units per active step. These paths and values agree with the prior diagnostic; no material discrepancy was found.

The current Level 7 production path uses the canonical `UsesLevel7RomWavePolicy` configuration. The optional fixed speed is selected only when that policy is active. Without the option, the existing formula and its random draw remain in use. The setting is stored as a fixed per-Spiker movement speed; it does not change `Spiker.Update`, projectile advancement, firing cadence/holdoff, ammunition, admission, or other enemy movement.

## Development-only control and calibration values

Added `--level7-spiker-speed <value>`. The accepted range is bounded to 2.625–3.000, parsed with invariant numeric formatting, and rejects repeated, malformed, non-finite, or out-of-range values. The supported manual comparison set is:

| Case | Fixed Level 7 Spiker speed | Purpose |
|---|---:|---|
| Current/control | No option; production randomized range | Baseline; retains normal behavior |
| Low | 2.625 | Existing measured lower endpoint |
| Intermediate | 2.8125 | Midpoint of 2.625 and 3.000; separates smaller from larger gap growth |
| Equal | 3.000 | Matches the current projectile's per-step displacement |

For a fixed-speed run, production speed variation is deliberately suppressed for every Level 7 Spiker created during that run. Other levels retain their existing speed selection; Level 1–3 do not use a Spiker wave, and tests verify the existing selection path on Levels 4–6. The option is rejected for input-only diagnostics and gameplay recording/playback because the override is not part of recording startup metadata.

## C# measured / derived relative movement

The existing diagnostic still advances the actual `Spiker.Update` and `EnemyBullet.Advance` methods. The added 2.8125 case uses the same-depth shot reference and measures 30 subsequent active steps. Positive separation change means the projectile pulls farther toward the rim; zero means constant separation.

| Fixed Spiker speed | Spiker travel / 30 steps | Projectile travel / 30 steps | Separation change | Projectile/Spiker travel | C# derived trend |
|---:|---:|---:|---:|---:|---|
| 2.625 | 78.750 | 90.000 | +11.250 | 1.142857 | Projectile pulls ahead |
| 2.8125 | 84.375 | 90.000 | +5.625 | 1.066667 | Projectile pulls ahead |
| 3.000 | 90.000 | 90.000 | 0.000 | 1.000000 | Separation remains constant |

These are deterministic C# results only. The existing 4.500 diagnostic control is retained: at that speed the Spiker gains on its 3.000 projectile by 45.000 depth units over 30 steps. No exact MAME speed, ratio, or active-step/frame conversion is inferred.

## Evidence classification

- **ROM-BACKED:** As summarized by the preceding report, the local Rev. 2 disassembly has separate Spiker and enemy-shot speed inputs/update paths. This experiment establishes no new ROM-backed numeric speed or ratio.
- **MAME-OBSERVED:** Neal reported that a Level 7 Spiker's projectile visibly pulls away from its firing Spiker while both approach the rim. This is qualitative and is not converted into C# units or an exact ratio.
- **ARCADE-OBSERVED:** No direct arcade comparison was performed or supplied for this experiment.
- **C# MEASURED:** The fixed-speed values and relative movement table above are deterministic results from C# movement methods/tests.
- **PROJECT TEST HYPOTHESIS:** Current C# Level 7 Spikers may climb too quickly. This remains unproven and was not promoted to arcade/MAME/ROM evidence.
- **PROJECT DECISION:** None. No production calibration value was selected.

## Validation

- Focused parser, speed-selection, projectile, and override-specific firing tests:
  `dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --filter "FullyQualifiedName~Level7SpikerProjectileRelativeSpeedDiagnosticTests|FullyQualifiedName~DebugControllerTests|FullyQualifiedName~Level7SpikerFiringTests" --verbosity minimal`
  — passed, 36/36.
- Level 7 regression tests:
  `dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --filter "FullyQualifiedName~Level7" --verbosity minimal`
  — passed, 72/72. This includes the relative-speed diagnostic, Level 7 firing tests, and existing Level 7 movement/production/regression tests.
- Solution build:
  `dotnet build TempestCSharp.slnx --no-restore`
  — succeeded, 0 warnings, 0 errors.
- `git diff --check` — succeeded with no output.

The interactive game and MAME were not launched. Neal's manual comparison remains pending.

## Manual launch commands

Run each command from the repository root in Windows PowerShell or a terminal:

```powershell
# CURRENT CONTROL — ordinary randomized Level 7 Spiker speeds
dotnet run --project src\Tempest.Game -- --level 7 --lives 10

# FIXED 3.000 — projectile-equal comparison
dotnet run --project src\Tempest.Game -- --level 7 --lives 10 --level7-spiker-speed 3.000

# FIXED 2.625 — low-end comparison
dotnet run --project src\Tempest.Game -- --level 7 --lives 10 --level7-spiker-speed 2.625

# FIXED 2.8125 — intermediate comparison
dotnet run --project src\Tempest.Game -- --level 7 --lives 10 --level7-spiker-speed 2.8125
```

## Manual acceptance checklist for Neal

Compare all four settings against MAME/arcade and answer the same questions for each:

1. **Spiker climb speed (primary):** Ignoring the projectile initially, does the Spiker look clearly too fast, somewhat too fast, approximately like MAME/arcade, somewhat too slow, or clearly too slow?
2. **Projectile relationship:** Does the projectile pull away strongly, moderately, slightly, approximately equally, or does the Spiker catch it?
3. **Level 7 pressure:** Without intentionally dying or creating artificial stress, does ordinary Level 7 feel closer to MAME/arcade difficulty? Do not tune concurrency or other mechanics from this test.
4. **Repeated cycles:** When a Spiker ascends, retreats, and ascends again, does its movement remain plausible, and does the corrected repeated-cycle firing behavior remain intact?

## Changed files and pre-existing candidate work

Files changed specifically for this calibration experiment:

- `src/Tempest.Game/Debug/DebugController.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/TempestGame.cs` (already modified before this task; the existing candidate changes were preserved)
- `tests/Tempest.Game.Tests/DebugControllerTests.cs`
- `tests/Tempest.Game.Tests/Level7SpikerFiringTests.cs` (untracked before this task; retained and extended)
- `tests/Tempest.Game.Tests/Level7SpikerProjectileRelativeSpeedDiagnosticTests.cs` (untracked before this task; retained and extended)
- `AI-Logs/Prompts/2026-11-11-level7-spiker-speed-manual-calibration-experiment-prompt.md`
- `AI-Logs/Reports/2026-11-11-level7-spiker-speed-manual-calibration-experiment-report.md`

All other pre-existing candidate files listed in the starting status remain as found. No commit, staging, push, cleanup, reset, or checkout was performed.

Final `git status --short --branch`:

```text
## master...origin/master
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

## Explicit scope confirmations

- Normal no-option Level 7 behavior was not intentionally changed; the existing randomized speed formula remains active.
- Enemy projectile production speed was not changed.
- No permanent Spiker production-speed choice was made.
- The Stage 2 value-swap experiment was **NOT** performed. It remains available only with separate explicit authorization if Neal rejects all Stage 1 candidates.
- Manual acceptance status: **PENDING NEAL**.
