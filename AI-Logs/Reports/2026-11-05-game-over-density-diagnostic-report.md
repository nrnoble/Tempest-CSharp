# Game Over visible-density diagnostic report

## Result

Added development-only `--game-over-density <multiplier>` parsing and
renderer-only density translation. An omitted option and explicit `1` preserve
the current visible marks exactly. Neal's tentative 4x estimate remains
unaccepted; manual calibration is pending.

The option accepts positive finite invariant-culture numeric values. Missing,
duplicate, malformed, zero, negative, NaN and infinite values fail with a
clear diagnostic before window initialization. A direct invalid-value
invocation returned exit code 2 and did not launch the game window.

## Implementation and invariants

- The ROM-backed animation remains at 16 indexed parent records. Density is
  not ROM-derived and does not change record capacity, initialization,
  movement, bounds, spawn countdown/cadence, drain, completion, or seeded
  parent trajectories.
- The renderer scales visible short vector marks only. For each of the two
  existing mark slots, a deterministic cumulative-floor allocation based on
  spawn ordinal and density selects or replicates marks without frame-to-frame
  random flicker.
- At 0.5x, 16 marks are emitted for 16 active records instead of the current
  32; 2x emits 64, 2.5x emits 80, and 4x emits 128. These exact test counts use
  the initial 16 active parents.
- Additional marks are distributed along the existing parent movement offset,
  with total local spread bounded to six logical pixels. Mark length,
  horizontal vector vocabulary, color selection and parent movement are
  unchanged.
- The GAME OVER message and its geometry, effect transition, isolated black
  presentation, tube suppression and terminal restart behavior are unchanged.
  The option is not a gameplay setting, is not persisted, and has no UI.
- No density calibration is accepted or marked as final.

## Files changed for this diagnostic

### Production

- `src/Tempest.Game/Debug/DebugController.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `TEMPEST-DEVELOPMENT-NOTES.md`

### Tests

- `tests/Tempest.Game.Tests/GameOverDensityDiagnosticTests.cs`

### Paired ignored AI records

- `AI-Logs/Prompts/2026-11-05-game-over-density-diagnostic-prompt.md`
- `AI-Logs/Reports/2026-11-05-game-over-density-diagnostic-report.md`

## Validation

- Focused Game Over/density/rendering selection: **49 passed, 0 failed,
  0 skipped**.
- Neighboring death, Levels 1-7, progression, campaign soak, Super Zapper,
  rim-pursuit, restart and related regression selection: **673 passed,
  0 failed, 0 skipped**.
- Full `dotnet test TempestCSharp.slnx`: **803 passed, 0 failed, 0 skipped**.
- `dotnet build TempestCSharp.slnx`: **0 warnings, 0 errors**.
- `git diff --check`: **passed**.

## Manual calibration

Start with 1 versus 4; Neal need not exhaustively test values. The arcade/MAME
comparison remains the acceptance criterion.

```powershell
dotnet run --project src\Tempest.Game -- --lives 1 --game-over-density 1
dotnet run --project src\Tempest.Game -- --lives 1 --game-over-density 2
dotnet run --project src\Tempest.Game -- --lives 1 --game-over-density 4
dotnet run --project src\Tempest.Game -- --lives 1 --game-over-density 3
dotnet run --project src\Tempest.Game -- --lives 1 --game-over-density 2.5
```

No interactive launch was performed.
