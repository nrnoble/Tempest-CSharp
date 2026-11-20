# C3b Level 3 mixed-wave implementation report

## Status and scope

Implemented direct graphical `--level3-test` access to the existing
`LevelConfiguration.Level3Mixed` model. Neal completed manual graphical C3b
acceptance across multiple runs; the route remains development-only. Starting checkpoint:
`233bf25 Document Level 3 mixed-wave architecture`; C3a checkpoint:
`2b2e50c Implement headless Level 3 mixed-wave lifecycle`.

No campaign progression, `LevelProgression`, Level 2 exit behavior, Level 4,
gameplay composition/scheduling, Tanker firing mechanics, cargo route,
identity/lifecycle, geometry, controller mapping, or Super Zapper behavior
was changed. No C3a production defect was exposed. No commit or push was made.

## Implementation

- Added the mutually exclusive `--level3-test` scenario, preserving
  `--level3-geometry-test`, `--lives`, `--enemy-bullets-test`, and standalone
  input-diagnostic behavior. Program routes to the exact existing
  `Level3Mixed` configuration, disables natural progression, and sets a
  development-specific window title.
- The renderer now draws actual Level 3 mixed-wave bullets through its
  existing projection and callback path even when the optional bullet flag is
  off. Existing exit hiding and other modes' visibility policy remain intact.
- Mixed-wave HUD rows use unresolved original/cargo counts from
  `Level3WaveLedger` and count active Eggs, Flippers, and Tankers. The HUD
  labels direct access as development-only; no Super Zapper charge counter
  was added.
- Terminal replay and Game Over restart use the established launch
  configuration and reset behavior. Geometry diagnostic behavior remains
  unchanged.

## Evidence and manual acceptance

**AUTOMATED VERIFIED:** CLI routing and conflicts, direct configuration and
terminal policy, ledger-backed HUD values, actual mixed-wave Tanker bullet
projection/draw callback with finite coordinates, prior-mode visibility
policy, replay/Game Over reset behavior, and the listed C3a/regression suite.
Automated tests do not establish framebuffer fidelity or arcade behavior.

**MANUALLY ACCEPTED (visible C# behavior):** Across multiple graphical runs
using `dotnet run --project src/Tempest.Game -- --level3-test --lives 10`,
without relying on `--enemy-bullets-test`, Neal reported:

- Entry/HUD: PASS.
- Tankers observed; production Tanker bullets visible: PASS.
- Ordinary/cargo Flippers silent by default: PASS.
- Player-shot Tanker cargo release: PASS.
- Natural Tanker cargo release: PASS.
- Super Zapper behavior: PASS.
- Death/re-entry: PASS.
- Completion/replay: PASS.
- Crashes: none. Other observed problems: none.
- Overall rating: **5/5 — "Impressive."**

These observations accept the visible C# graphical outcomes only. They do not
verify hidden ledger identity sets, deterministic RNG ordering, exact hidden
timers, Tanker probabilities, or other unobserved internals, and do not by
themselves establish unknown Atari behavior.

Files inspected included `DebugController.cs`, `Program.cs`, `TempestGame.cs`,
`VectorRenderer.cs`, `LevelConfiguration.cs`, `Level.cs`,
`Level3WaveLedger.cs`, related Level 3/rendering/input/restart tests, and the
C3a analysis/implementation reports and development documentation.

## Changed files

- `src/Tempest.Game/Debug/DebugController.cs`
- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `src/Tempest.Game/TempestGame.cs`
- `tests/Tempest.Game.Tests/Level3GeometryTests.cs`
- `tests/Tempest.Game.Tests/Level3TestModeTests.cs` (new)
- `README.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `TEMPEST-BEHAVIOR-SPEC.md` (narrow development-only/manual-visible evidence update)

The exact prompt is saved at
`AI-Logs/Prompts/2026-11-01-c3b-level3-mixed-wave-implementation-prompt.md`.
This report is saved at
`AI-Logs/Reports/2026-11-01-c3b-level3-mixed-wave-implementation-report.md`.
Both remain under ignored `AI-Logs/`; prior history was not overwritten.

## Validation

- Focused mode tests: 5 passed.
- C3b/C3a, geometry, configuration, progression, renderer, Tanker/cargo,
  Super Zapper, input and diagnostic regression group: 475 passed.
- Final focused C3b test command:
  `dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~Level3TestModeTests"`
  — 5 passed, 0 failed, 0 skipped.
- Final full solution:
  `dotnet test TempestCSharp.slnx --no-restore` — 525 passed, 0 failed,
  0 skipped.
- Final build:
  `dotnet build TempestCSharp.slnx --no-restore` — succeeded, 0 warnings,
  0 errors.
- Final `git diff --check` — passed.

`Reference/incitti/` is unchanged. The narrow behavior-spec update records
only the manually accepted visible C# outcomes as development-only evidence.
Existing untracked `TEMP/` was left untouched. Direct access remains
**DEVELOPMENT ONLY**; reference-derived behavior remains **REFERENCE
VERIFIED**, C# identity and lifecycle policy remains **PROJECT DECISIONS**,
and no new behavior was upgraded to **ARCADE VERIFIED**.

## C3b finalization

Manual acceptance passed using the primary command without
`--enemy-bullets-test`:

```powershell
dotnet run --project src/Tempest.Game -- --level3-test --lives 10
```

The reported observations included entry/HUD, Tanker bullet visibility,
default Flipper silence, both Tanker cargo release paths, Super Zapper,
death/re-entry, and completion/replay. No crashes or other observed problems
were reported. Hidden identity/timer/RNG evidence remains automated or
reference/project-policy evidence; no new manual claim is made for those.
No remaining C3b blocker is known.
