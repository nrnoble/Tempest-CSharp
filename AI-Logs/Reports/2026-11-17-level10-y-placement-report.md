# Diagnostic Level 10 downward placement — implementation report

Date: 2026-11-17

## Outcome

**Implementation and automated validation: PASS. Manual acceptance: PENDING.**

Level 10 now has a final screen-space downward translation in its Atari-presentation adapter. No interactive game was launched. No conclusion is drawn about visual acceptance or enemy-motion behavior; Neal's manual replay remains required.

## Placement change

`AtariPresentationAdapter.Project` now adds `Level10VerticalPlacement` (`+50` logical pixels) to `screenY` for displayed Level 10 only, after the existing Level 10 projection equation and before the projected result is returned. The previous additive placement was `0`; the new placement is `+50`. The translation is applied in final screen space, so it actually moves the output consumed by `VectorRenderer`.

The focused renderer test fixes the startup projection context at `K=70`, `ZOffset=5`, and verifies an actual near-rim endpoint moves from the previous-candidate Y of `125` to Y `175`. This is a `50`-pixel downward movement in the rendered coordinates, not only a changed configuration value.

The same test averages the actual rendered near- and far-rim vertex X coordinates: both remain centered at X `400` within `0.001`. It also verifies a projected enemy bullet at the far end equals the midpoint of the corresponding rendered tube lane, preserving tube/entity registration.

## Preserved scope

Only the Level 10 final screen-space Y value and focused regression coverage changed. No horizontal equation or center, origin, scale, normalization, geometry, projection depth/perspective, gameplay configuration, simulation behavior, enemy movement/timing, Level 7 behavior, or Levels 1-9 were intentionally changed.

## Changed files

- `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`
- `tests/Tempest.Game.Tests/DiagnosticLevel10Tests.cs`
- This report and its paired prompt under ignored `AI-Logs/`.

The adapter and test files were already untracked work in progress when this task began. They remain unstaged; unrelated existing worktree changes were preserved.

## Validation

Focused Level 10 tests:

```powershell
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~DiagnosticLevel10Tests" --blame-hang-timeout 4m --verbosity minimal
```

**PASS** — 4 passed, 0 failed, 0 skipped.

Full regression suite:

```powershell
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --blame-hang-timeout 5m --verbosity minimal
```

**PASS** — 1,595 passed, 0 failed, 0 skipped.

Release build:

```powershell
dotnet build TempestCSharp.slnx --no-restore --configuration Release --verbosity minimal
```

**PASS** — build succeeded; 0 warnings, 0 errors.

Whitespace validation:

```powershell
git diff --check
```

**PASS** — no whitespace errors. Git reported existing LF-to-CRLF working-copy warnings for `LevelLoader.cs` and `InputSystem.cs`.

The test pins and restores static projection values so prior test state cannot change the measured rendered coordinates.

## Neal's manual acceptance

Replay the existing genuine-geometry Level 10 recording from the repository root:

```powershell
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --playback-file level10-genuine-geometry.json --atari-presentation
```

The bare recording filename resolves under `UserData\GameplayRecordings`.

Please judge both:

1. Is Level 10 now vertically positioned correctly while retaining its current correct horizontal center?
2. Does gameplay remain normal, with the characteristic depth-dependent enemy slowdown still absent?

Automated projection tests do not answer either manual question. Stop here for Neal's judgment. No commit or push was made.
