# Rim-Speed Command-Line Diagnostic

## Result

Added the temporary development-only `--rim-speed <multiplier>` option. Omission defaults to `1.0`; values are parsed once at startup as positive, finite, invariant-culture doubles. Invalid, missing, or repeated values fail before the game window starts.

The multiplier is passed explicitly from `Program` through `TempestGame` into each runtime `Level`. It scales that level's configured Flipper turn rate only during rim turning. The levels retain their own base rates; multiplier `1` preserves the accepted Level 1 rate of 8 degrees/update and the existing per-level defaults.

Rim pauses, below-rim angular movement, climb speeds, the Level 7 2-3-2-2-2 programmed route through its rim handoff, firing, shots, Super Zapper, and progression are not scaled. Very large finite values safely complete the current pivot by clamping its final angular step to the remaining sweep.

No HUD, menu, persistence, or production tuning was added. No multiplier is characterized as arcade-correct.

## Starting worktree and rejected experiment

- Started at accepted `HEAD` `3bebf42cbe9514846ab32bf4c8a12bcfdb048da7`.
- The initial worktree contained a fixed Level 1 16-degree override, related test edits, and an untracked analysis-model test file. The fixed override was removed; the analysis-only material was retained and updated to express the comparison with multipliers.
- The rejected shot/contact-ordering production experiment was absent from the initial diff and was not reintroduced. `TempestGame.cs` changes only pass the multiplier into runtime Level construction; shot/contact ordering was not modified.
- No shared `LevelConfiguration` objects are cloned or replaced, preserving natural progression's existing configuration identity.

## Files changed

Production:

- `src/Tempest.Game/Debug/DebugController.cs` — invariant-culture parsing and fail-fast validation.
- `src/Tempest.Game/Program.cs` — startup parsing and explicit propagation.
- `src/Tempest.Game/TempestGame.cs` — validated runtime multiplier and per-Level wiring.
- `src/Tempest.Game/Levels/Level.cs` — runtime effective rim rate using each level's configured base.
- `src/Tempest.Game/Gameplay/Flipper.cs` — rim-only angular scaling and safe final-step clamp.

Tests:

- `tests/Tempest.Game.Tests/RimSpeedDiagnosticTests.cs` — parsing, invalid inputs, default equivalence, progression, and large finite rate coverage.
- `tests/Tempest.Game.Tests/RimPursuitTests.cs` — measured rim rates, independent pause, per-level base rates, below-rim behavior, and Level 7 route isolation.
- `tests/Tempest.Game.Tests/SimulationTests.cs` and `tests/Tempest.Game.Tests/PlayerDeathPresentationTests.cs` — use the runtime effective rate for pivot timing.
- `tests/Tempest.Game.Tests/Level1StationaryRimCombatComparisonTests.cs` — preserved test-only analysis and expressed its A/B cases as multiplier 1 versus 2.

The comparison test file was untracked before this task and remains untracked; it is analysis-only, not production shot/contact behavior.

## Validation

- Focused rim-speed, pursuit, and stationary-comparison tests: **49 passed, 0 failed, 0 skipped**.
- Combined Levels 1-7 movement, firing, shots, Super Zapper, progression, campaign, and rim-speed regression selection: **624 passed, 0 failed, 0 skipped**.
- `dotnet test TempestCSharp.slnx`: **786 passed, 0 failed, 0 skipped**.
- `dotnet build TempestCSharp.slnx`: succeeded, **0 warnings, 0 errors**.
- `git diff --check`: passed.

## Manual A/B testing for Neal

No interactive game was launched. Suggested first contrast: try `0.5`, then `4`; if that contrast is perceptible, compare `1` and `2`.

```powershell
dotnet run --project src\Tempest.Game -- --lives 10
dotnet run --project src\Tempest.Game -- --rim-speed 1 --lives 10
dotnet run --project src\Tempest.Game -- --rim-speed 0.5 --lives 10
dotnet run --project src\Tempest.Game -- --rim-speed 2 --lives 10
dotnet run --project src\Tempest.Game -- --rim-speed 4 --lives 10
dotnet run --project src\Tempest.Game -- --rim-speed 3 --lives 10
dotnet run --project src\Tempest.Game -- --rim-speed 2.5 --lives 10
```

Any positive finite value is accepted. Compare visible rim pursuit and single-/multiple-pursuer stationary defense; no permanent production decision is made here.

## Final boundaries

`HEAD` remains the accepted baseline. No authority/reference document changed. No game launch, commit, push, or Internet disassembly research occurred. Manual A/B judgment remains with Neal.
