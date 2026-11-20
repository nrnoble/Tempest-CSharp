# Level 7 Spiker/projectile relative-speed calibration diagnostic

Date: 2026-11-11

## Scope and starting worktree

This was a diagnostic-only Level 7 task. The initial worktree was on `master`, tracking `origin/master`, with nine modified tracked files and two untracked Level 7 candidate test files. Those changes were present before this task and were preserved. Only the diagnostic test was added to the worktree; no production gameplay code was changed.

The prompt record is [2026-11-11-level7-spiker-projectile-relative-speed-calibration-diagnostic-prompt.md](../Prompts/2026-11-11-level7-spiker-projectile-relative-speed-calibration-diagnostic-prompt.md). Both AI-Logs files are ignored by Git and were not staged.

## Files and evidence examined

- [Spiker.cs](../../src/Tempest.Game/Gameplay/Spiker.cs), [EnemyBullet.cs](../../src/Tempest.Game/Gameplay/EnemyBullet.cs), and [Enemy.cs](../../src/Tempest.Game/Gameplay/Enemy.cs)
- [TempestGame.cs](../../src/Tempest.Game/TempestGame.cs), [LevelConfiguration.cs](../../src/Tempest.Game/Levels/LevelConfiguration.cs), and [Level.cs](../../src/Tempest.Game/Levels/Level.cs)
- Existing Level 7 production, admission, firing, and movement tests
- [Tempest Rom Source code.md](../../Reference/arcade/Tempest%20Rom%20Source%20code.md) and [Incitti's tempest.bmx](../../Reference/incitti/tempest.bmx)
- [TEMPEST-BEHAVIOR-SPEC.md](../../TEMPEST-BEHAVIOR-SPEC.md) and [TEMPEST-DEVELOPMENT-NOTES.md](../../TEMPEST-DEVELOPMENT-NOTES.md)

## Diagnostic design and exact C# measurements

[Level7SpikerProjectileRelativeSpeedDiagnosticTests.cs](../../tests/Tempest.Game.Tests/Level7SpikerProjectileRelativeSpeedDiagnosticTests.cs) directly advances a `Spiker` using `Spiker.Update` and the projectile using `EnemyBullet.Advance`; it does not duplicate their movement formulas. It initializes a Level 7 production Spiker, gives it the same initialization and first climb updates as the production update sequence, then creates a same-lane projectile at the Spiker's current depth. The reference state is immediately after this shot creation. Both are advanced once per measured active step, with growth-driven retreat disabled and no random firing/cadence path involved.

The test samples the reference state, then 1, 10, and 30 subsequent active steps. It covers a low, equal-to-shot, and higher Spiker speed (2.625, 3.000, and 4.500 per step); all are within the current Level 7 production selection range. The table reports absolute along-tube depths, distances moved from the reference, signed separation, separation change from reference, and projectile/Spiker travel ratio.

Project depth increases toward the rim: `Level.Depth` is 400, with 0 at the far end. Separation is `projectile depth - Spiker depth`; positive means the projectile is farther toward the rim than its shooter, and negative means it is behind it.

### Spiker speed 2.625; projectile speed 3.000 per active step

| Active step | Spiker depth | Projectile depth | Spiker moved | Projectile moved | Separation | Change from reference | Travel ratio |
|---:|---:|---:|---:|---:|---:|---:|---:|
| 0 | 2.625 | 2.625 | 0.000 | 0.000 | 0.000 | 0.000 | n/a |
| 1 | 5.250 | 5.625 | 2.625 | 3.000 | 0.375 | 0.375 | 1.142857 |
| 10 | 28.875 | 32.625 | 26.250 | 30.000 | 3.750 | 3.750 | 1.142857 |
| 30 | 81.375 | 92.625 | 78.750 | 90.000 | 11.250 | 11.250 | 1.142857 |

### Spiker speed 3.000; projectile speed 3.000 per active step

| Active step | Spiker depth | Projectile depth | Spiker moved | Projectile moved | Separation | Change from reference | Travel ratio |
|---:|---:|---:|---:|---:|---:|---:|---:|
| 0 | 3.000 | 3.000 | 0.000 | 0.000 | 0.000 | 0.000 | n/a |
| 1 | 6.000 | 6.000 | 3.000 | 3.000 | 0.000 | 0.000 | 1.000000 |
| 10 | 33.000 | 33.000 | 30.000 | 30.000 | 0.000 | 0.000 | 1.000000 |
| 30 | 93.000 | 93.000 | 90.000 | 90.000 | 0.000 | 0.000 | 1.000000 |

### Spiker speed 4.500; projectile speed 3.000 per active step

| Active step | Spiker depth | Projectile depth | Spiker moved | Projectile moved | Separation | Change from reference | Travel ratio |
|---:|---:|---:|---:|---:|---:|---:|---:|
| 0 | 4.500 | 4.500 | 0.000 | 0.000 | 0.000 | 0.000 | n/a |
| 1 | 9.000 | 7.500 | 4.500 | 3.000 | -1.500 | -1.500 | 0.666667 |
| 10 | 49.500 | 34.500 | 45.000 | 30.000 | -15.000 | -15.000 | 0.666667 |
| 30 | 139.500 | 94.500 | 135.000 | 90.000 | -45.000 | -45.000 | 0.666667 |

These are deterministic C# measurements of the existing methods, not an arcade timing claim. They show that the C# projectile is faster than a 2.625-speed Spiker, matches a 3.000-speed Spiker, and is slower than a 4.500-speed Spiker. The Level 7 production Spiker selection expression is `ProgressionIndex / 48f + 2.5f + (float)(random.NextDouble() * 2)`; with progression index 6, its nominal range spans approximately 2.625 to 4.625. The existing enemy projectile advances by `+3` height per call. No production rate or firing behavior was changed.

## ROM evidence and limits

**ROM-BACKED:** The local Rev. 2 disassembly's Spiker movement program at `$A0F7` begins with the `$0C` “move per speed” operation. It stores Spiker speed in `spd_spiker_lsb`; initialization processes that value through `crack_speed` at `$9382`–`$938B`. Enemy shots have a distinct `enm_shotspd_lsb` value, also processed through `crack_speed` at `$9391`–`$939A`. In `move_shots` (`$A1C0`–`$A1CD`), an enemy shot's fractional and integer along-position fields are incremented by the enemy-shot speed fields. The disassembly's `$9607` data also labels separate tables for `enm_shotspd_lsb` and `spd_spiker_lsb`.

This establishes separate ROM speed inputs and update paths. It does not by itself establish the measured MAME separation for one identified shot and its shooter, nor establish a conversion between those fixed-point ROM values, C# depth units, and MAME video frames.

**MAME-OBSERVED (reported in Neal's task):** A Spiker projectile was seen overtaking a Tanker ahead of it. This report treats that as empirical evidence from Neal's direct MAME observation; the session did not independently launch or replay MAME. The Tanker overtake is not itself a measured start/end separation between the projectile and its firing Spiker.

**Conflict/limits:** The task does not include the numbers or full derivation from the earlier static calculation, so that exact calculation was not reconstructed here. The ROM evidence above confirms distinct speed variables and step paths, while the provided MAME observation concerns rendered runtime behavior. Without the prior calculation's inputs and a controlled MAME observation of both the Spiker and its projectile, this diagnostic cannot reconcile the numerical disagreement. It therefore does not dismiss Neal's MAME observation or promote an unverified explanation to ROM-backed behavior.

## MAME comparison procedure for Neal

1. In MAME, reach Level 7 and wait for a Spiker to build in a lane. Prefer a mid-tube shot with enough clear lane ahead that the projectile and Spiker are both visible for at least 30 frames.
2. Pause on a shot fired by that Spiker. Confirm the projectile is in the Spiker's lane/path and note the first frame you use as the reference. If the firing frame is missed, record the initial gap in the reference image rather than assuming it is zero.
3. Record the Spiker's position and projectile's position along that same lane, using recognizable lane points between the far end and the rim as rough landmarks. Avoid comparing raw screen-pixel distances; the projection is not a proven linear depth scale.
4. Advance exactly 30 MAME frames, then record both positions and whether the projectile-to-Spiker gap grew toward the rim, stayed about constant, or shrank. If both remain visible and unobstructed, optionally repeat for another 30 frames.
5. Estimate each object's along-lane travel from the same visual landmarks. A rough projectile-travel / Spiker-travel ratio is useful when both displacements are large enough to estimate; otherwise report only the direction of gap change. Note any collision, disappearance, or occlusion.
6. Send the initial and ending frame counts, rough positions or landmark descriptions, gap trend, and any estimated ratio. The previously observed Tanker overtake can be noted separately, but should not substitute for tracking the firing Spiker.

No evidence examined establishes a one-to-one mapping between a MAME frame and a C# active simulation step. Compare relative displacement and gap trend over a fixed MAME frame interval; do not treat 30 MAME frames as 30 proven C# steps.

Manual/MAME comparison is pending Neal.

## Validation

- Focused diagnostic: `dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-build --no-restore --filter FullyQualifiedName~Level7SpikerProjectileRelativeSpeedDiagnosticTests --logger "console;verbosity=detailed"` — passed, 1/1. This run printed the exact measurements above.
- Level 7 test subset: `dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter FullyQualifiedName~Level7 --logger "console;verbosity=detailed"` — passed, 52/52, including the diagnostic and existing Level 7 production/admission/firing/movement tests.
- Solution build: `dotnet build TempestCSharp.slnx --no-restore` — succeeded, 0 warnings, 0 errors.
- `git diff --check` — succeeded with no output.

The interactive game was not launched. No MAME/manual comparison was performed in this task.

## Files changed and final status

Diagnostic code added:

- `tests/Tempest.Game.Tests/Level7SpikerProjectileRelativeSpeedDiagnosticTests.cs`

Ignored, unstaged historical records added:

- `AI-Logs/Prompts/2026-11-11-level7-spiker-projectile-relative-speed-calibration-diagnostic-prompt.md`
- `AI-Logs/Reports/2026-11-11-level7-spiker-projectile-relative-speed-calibration-diagnostic-report.md`

No existing production file was modified by this diagnostic. The pre-existing candidate work remains untouched. Final `git status --short --branch`:

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

**No production gameplay speed correction was made.** Do not proceed to tuning until Neal completes the MAME comparison and manual assessment.
