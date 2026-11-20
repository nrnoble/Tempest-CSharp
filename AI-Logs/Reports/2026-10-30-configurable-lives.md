# Configurable Starting Lives Report

## 1. Original Starting-Life Definition

The normal starting count was defined in `Gameplay/Player.cs` as the initializer
`Lives { get; set; } = 3`. The same literal was repeated in
`TempestGame.ResetGame()`, which assigned `Player.Lives = 3` when restarting after
Game Over.

## 2. Files Changed

- `src/Tempest.Game/Gameplay/Player.cs`
- `src/Tempest.Game/Debug/DebugController.cs`
- `src/Tempest.Game/TempestGame.cs`
- `src/Tempest.Game/Program.cs`
- `tests/Tempest.Game.Tests/SimulationTests.cs`
- `AI-Logs/2026-10-30-configurable-lives.md` (this report)

No Incitti reference files were modified.

## 3. Implementation

`Player.DefaultStartingLives` is the single default constant (`3`). Player accepts
an optional starting count and falls back to that constant for non-positive
values. `DebugController.GetStartingLives` parses `--lives <number>` independently
of the existing `--spiker-test` switch. `Program` passes both selected options to
`TempestGame`.

TempestGame stores the validated count for the current run, initializes Player
with it, and uses that selected count on Game Over restart. Ordinary life loss
still subtracts exactly one. Level Complete replay retains its established
behavior and does not reset the life count.

## 4. Command-Line Examples

```powershell
dotnet run --project src/Tempest.Game -- --lives 10
dotnet run --project src/Tempest.Game -- --spiker-test --lives 10
dotnet run --project src/Tempest.Game -- --lives 20
```

The `--lives` option works without `--spiker-test`.

## 5. Validation and Fallback

A missing value, malformed integer, zero, negative number, or integer overflow
falls back to the normal default of three. Direct TempestGame/Player construction
also protects against non-positive values. Positive values such as 10 and 20 are
accepted. The parsing uses invariant integer format.

## 6. Build and Test Results

- Baseline build: passed.
- Baseline tests: 81 passed, 0 failed, 0 skipped.
- Final build: passed.
- Final tests: 90 passed, 0 failed, 0 skipped.
- `git diff --check`: passed.
- Incitti reference path status check: no changes.

Focused tests cover default initialization, `--lives 10`, combined Spiker flag,
invalid and missing values, selected-life Game Over restart, and unchanged
one-life loss/score behavior.

## 7. Known Limitations

The parser supports the requested two-token form `--lives <number>`; equals-form
syntax is not implemented. The count is bounded by the positive `Int32` range.
No graphical game window was launched for validation. No scoring, enemy, death
animation, level completion, exit, input, spinner, rendering, or cursor behavior
was changed.

## 8. Todo Reconciliation

- Complete: identify and centralize the normal default.
- Complete: parse and validate the optional lives argument, independently of the
  Spiker development switch.
- Complete: initialize and restore the selected count on Game Over restart.
- Complete: focused tests, full build/test validation, diff review and reference
  safety verification.
- Deferred: none. No implementation tasks remain in progress.
- Not performed: graphical manual validation; this feature changes only starting
  lives and the Raylib window was not used for automated checks.
