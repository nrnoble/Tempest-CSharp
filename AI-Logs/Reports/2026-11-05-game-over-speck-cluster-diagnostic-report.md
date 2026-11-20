# Game Over speck-cluster density diagnostic report

## Result

Replaced renderer-side replication along a ROM parent's movement direction
with a deterministic compact 2D child-speck cluster. Every child remains
positioned relative to its active parent; density no longer creates a longer
or more continuous parent-aligned streak.

## Manual evidence and calibration status

Neal found the density diagnostic useful and preferred the tiny-speck
appearance at `--game-over-density 0.5`. Higher values with the old
velocity-aligned replication looked increasingly like lines. This is
`ARCADE-COMPARISON / MANUAL TEST` evidence. The cluster implementation remains
experimental; no density value is accepted.

## Implementation

- ROM simulation remains exactly 16 parent records. Parent coordinates,
  movement, bounds, spawn countdown/cadence, drain, completion and seeded
  trajectories are unchanged.
- Each active parent has two child specks at density 1. `0.5` emits one speck
  per parent. Fractional multipliers deterministically distribute integer
  speck counts across parent spawn ordinals using cumulative-floor allocation.
- A stable hash of the parent spawn ordinal selects its rotation. Child
  ordinals use a golden-angle sequence with radius `min(3, 0.85 * sqrt(index+1))`
  logical pixels. Offsets are deterministic, 2D, independent of parent
  velocity, and stable as density changes.
- Every mark remains a 2.0-logical-pixel horizontal line; requesting more
  density does not enlarge marks. The existing deterministic Tempest color
  cycle is preserved.
- GAME OVER text, black isolated-effect presentation, gameplay and restart
  behavior are unchanged. The child geometry is a project visual translation
  of unresolved ROM graphic data, not additional ROM simulation records.

## Files changed for this diagnostic

### Production

- `src/Tempest.Game/Debug/DebugController.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`

### Tests

- `tests/Tempest.Game.Tests/GameOverDensityDiagnosticTests.cs`

### Documentation

- `TEMPEST-BEHAVIOR-SPEC.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`

### Paired AI records

- `AI-Logs/Prompts/2026-11-05-game-over-speck-cluster-diagnostic-prompt.md`
- `AI-Logs/Reports/2026-11-05-game-over-speck-cluster-diagnostic-report.md`

## Automated validation

- Focused Game Over/density/vector-rendering selection: **26 passed, 0 failed,
  0 skipped**.
- Neighboring death, Levels 1-7, progression, campaign soak, Super Zapper,
  rim-pursuit and restart regression selection: **657 passed, 0 failed,
  0 skipped**.
- `dotnet test TempestCSharp.slnx`: **806 passed, 0 failed, 0 skipped**.
- `dotnet build TempestCSharp.slnx`: **0 warnings, 0 errors**.
- Final `git diff --check`: **passed**.

## Manual comparison

The agent did not launch the game. Neal subsequently reported that marks stayed
tiny and independent at all tested densities, higher density added
independent-looking specks, large-scale motion looked correct, and colors
remained broadly right. Density `0.5` best resembled the arcade, but the
overall effect still felt too sparse. No exact project calibration was
provided or accepted; the visual-density search remains open.

Start with these values for future comparison:

```powershell
dotnet run --project src\Tempest.Game -- --lives 1 --game-over-density 0.5
dotnet run --project src\Tempest.Game -- --lives 1 --game-over-density 2
dotnet run --project src\Tempest.Game -- --lives 1 --game-over-density 4
```

Other requested diagnostic values:

```powershell
dotnet run --project src\Tempest.Game -- --lives 1 --game-over-density 1
dotnet run --project src\Tempest.Game -- --lives 1 --game-over-density 6
```

Neal can provide any positive finite decimal. Judge whether individual marks
stay tiny, whether increased density adds independent-looking specks, how
closely the effect matches the arcade, and whether any density is too sparse,
about right or too busy. No preferred final value is recorded.

## Git and scope

The pre-existing Game Over milestone remains uncommitted and unstaged. HEAD
remains `b349e0b3f3517c9ae9c2e1f22ce78f6d801f862f`; no unrelated gameplay
changes, staging, commit, push or interactive launch were performed.
