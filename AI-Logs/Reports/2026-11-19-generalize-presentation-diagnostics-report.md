# Generalize presentation diagnostics

**Status:** PASS — implementation and automated validation complete; Neal's manual visual acceptance remains pending.

## Scope and implementation

Generalized the development-only presentation controls across all direct-start
Atari-presentation levels: production Levels 1-9 and diagnostic Level 10.
No level-specific production calibration values were added.

- `--tube-y-gain` now accepts any positive finite invariant-culture value and
  transforms vertical model proportions on all supported presentation paths.
- Replaced `--level1-tube-scale` with `--tube-scale`. Positive finite values
  uniformly scale projected tube coordinates and local presentation scales
  around each depth's existing zero-coordinate anchor. The anchor is preserved;
  no automatic recentering or repositioning is introduced.
- `--tube-vertical-offset` now translates the complete projected presentation
  on every supported level, after ExitZoom compensation. Positive values move
  down; negative values move up.
- Kept Level 5 and 6's dedicated projection paths intact and applied the
  diagnostics through those paths. Level 6's legacy Game Over projection also
  receives the presentation transforms. No technical restriction prevents
  applying the three controls across implemented Atari-presentation levels.
- Extended diagnostic parsing, renderer registration, and focused tests.
  Settings remain presentation-only and are not added to gameplay recordings.
- Updated the README with the generalized options and example invocations.

## Files changed for this task

- `src/Tempest.Game/Debug/DebugController.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `tests/Tempest.Game.Tests/TubeYGainTests.cs`
- `tests/Tempest.Game.Tests/TubeVerticalOffsetTests.cs`
- `README.md`

The substantive prompt was saved at
`AI-Logs/Prompts/2026-11-19-generalize-presentation-diagnostics-prompt.md`.
Existing unrelated worktree changes and user/reference assets were preserved;
nothing was staged, committed, pushed, reset, restored, stashed, or cleaned.

## Validation

- Focused diagnostics, Approach, ExitZoom, and screenshot regressions:
  **PASS** — 181 passed, 0 failed.
- Complete solution tests (`dotnet test TempestCSharp.slnx --no-restore`):
  **PASS** — 1,702 passed, 0 failed.
- Release build (`dotnet build TempestCSharp.slnx -c Release --no-restore`):
  **PASS** — 0 warnings, 0 errors.
- `git diff --check`: **PASS**.

No interactive game was launched and no visual assets were opened or analyzed.

## Manual acceptance remaining

Neal should visually evaluate the diagnostics and confirm the resulting
presentation. Example starting points (not selected production values):

```powershell
dotnet run --project src\Tempest.Game -- --atari-presentation --level 1 --tube-y-gain 1.4 --tube-scale .8 --tube-vertical-offset +35
dotnet run --project src\Tempest.Game -- --atari-presentation --level 7 --tube-y-gain 1.4 --tube-scale .8 --tube-vertical-offset +35
dotnet run --project src\Tempest.Game -- --atari-presentation --level 4 --tube-y-gain 1.2 --tube-scale .9 --tube-vertical-offset -20
```

Level 1 reference from Neal:

```powershell
dotnet run --project src\Tempest.Game -- --atari-presentation --level 1 --tube-y-gain 1.4 --tube-scale .8 --tube-vertical-offset +35
```

Gameplay, HUD, projection, and other visual calibration concerns remain out of
scope for this diagnostic-capability milestone.
