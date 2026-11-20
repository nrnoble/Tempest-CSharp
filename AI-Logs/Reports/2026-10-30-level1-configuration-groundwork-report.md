# Level 1 configuration and initialization groundwork

## 1. Purpose and starting checkpoint

Implemented the approved behavior-preserving configuration and initialization
milestone from checkpoint `2741f2a Implement arcade all-rim completion behavior`.
No commit was created. The behavior specification was not modified.

## 2. Configuration model

Added `LevelConfiguration.Level1` with internal/displayed identity 0/1,
board/palette 0/0, the existing Level 1 definition, closed topology, depth 400,
position baseline 50, player start lane 7 at offset 0.5, supply 10, four
initial Eggs, release/approach timing and caps, deterministic spawn/direction
sequences, standard exit policy, and Flipper-only normal composition.

Current Egg cap, cadence and deterministic choices are represented as project
configuration; they are not described as literal Incitti behavior.

## 3. Level runtime changes

`Level` remains the per-wave mutable runtime and now accepts a
`LevelConfiguration`. The default constructor delegates to the explicit
Level 1 configuration. Definition, Egg release, initial Egg count, reserve,
completion and remaining-enemy accounting now use configured values. The
Tanker development ledger uses the installed runtime's configured supply.

## 4. Fresh initialization ownership

`TempestGame.InitializeFreshWave` creates and installs one new `Level` from the
launch configuration for initial setup, Complete replay and Game Over restart.
Complete remains terminal and replay reinstalls the same Level 1 configuration;
there is no advancement.

## 5. Reconstruction separation

Ordinary life reconstruction and Spike reverse/retry retain the existing
`Level` instance/configuration and use its reconstruction methods. They do not
run fresh initialization. Score, resolved outcomes, Super Zapper charges,
Hurry-Up, persistent Spikes and unresolved-unit re-entry retain their prior
policies.

## 6. Event and callback lifecycle

Fresh initialization detaches `SpikeCreated` from the outgoing Level, clears
game-owned collision/entity state, creates the replacement, subscribes one
handler and registers its already-created initial Spikes once. A replay test
repeats this cycle and verifies an old Level's later Spike creation cannot
affect the active scenario.

## 7. Scenario and flag preservation

No command-line parsing, mutual exclusion, scenario composition, bullet policy,
or restart routing changed. Normal startup remains Flipper-only. Development
replay remains the same artificial wave and flags do not enable production
progression.

## 8. Accounting and reset preservation

Configured supply replaces the runtime's hardcoded total. Destroyed and
rim-cleared outcomes remain separate. Tanker original/cargo ledgers reset on
fresh initialization and retain their established life-reconstruction
behavior. Super Zapper charges reset on fresh setup and persist across life
reconstruction; pending target timers remain tied to discarded enemy objects.
Spike hazards are restored on fresh replay and persist through death/retry.

Pulse phase and the random source continue across fresh initialization, as at
the checkpoint. Fuseball animation resets on fresh initialization and is not
reset by life reconstruction. No generic clock reset was introduced.

## 9. Topology contract

`LevelDefinition` exposes `VertexCount` and `PlayableLaneCount`. The current
closed definition has 16 vertices and 16 playable lanes. A geometry-only open
fixture has 16 vertices and 15 lanes. This does not implement full open-tube
gameplay or alter current closed adjacency, movement, collision or rendering.

## 10. Files changed

- `src/Tempest.Game/Levels/LevelConfiguration.cs` — new Level 1 configuration.
- `src/Tempest.Game/Levels/LevelDefinition.cs` — vertex/lane counts.
- `src/Tempest.Game/Levels/Level.cs` — configuration-backed runtime accounting
  and Egg release.
- `src/Tempest.Game/TempestGame.cs` — fresh-wave installation, callback
  lifecycle and configured composition/supply.
- `src/Tempest.Game/Gameplay/Player.cs` — configuration-selected reset pose.
- `tests/Tempest.Game.Tests/LevelConfigurationTests.cs` — new configuration,
  accounting, lifecycle and callback tests.
- `tests/Tempest.Game.Tests/SimulationTests.cs` — topology fixtures now supply
  their definition through configuration.
- `README.md` and `TEMPEST-DEVELOPMENT-NOTES.md` — current architecture and
  targeted historical-truth cleanup.
- `AI-Logs/Prompts/2026-10-30-level1-configuration-groundwork-prompt.md` —
  exact copy of the supplied 445-line prompt.
- `AI-Logs/Reports/2026-10-30-level1-configuration-groundwork-report.md` —
  this report.

`AI-Logs/` is ignored by Git in this checkout; the requested prompt and report
were saved locally but therefore do not appear in `git status`.

## 11. Tests added or updated

Added nine focused tests for Level 1 identity/geometry, topology counts,
configured values and sequences, default construction, configured-total
accounting, replay/restart replacement, reconstruction identity, and repeated
Spiker callback rebinding. Existing isolated open-topology fixtures now pass
their topology through `LevelConfiguration`.

## 12. Validation results

| Validation | Result |
|---|---:|
| Configuration and initialization | 9 passed, 0 failed |
| Reset and reconstruction selection | 8 passed, 0 failed |
| Scenario and flag selection | 10 passed, 0 failed |
| Tanker/accounting selection | 57 passed, 0 failed |
| Super Zapper tests | 31 passed, 0 failed |
| Hurry-Up/all-rim/Spiker-exit selection | 67 passed, 0 failed |
| Full solution tests | 339 passed, 0 failed, 0 skipped |
| Solution build | 0 warnings, 0 errors |
| `git diff --check` | Passed |

Focused filters overlap; their totals are not additive. Git reported LF-to-CRLF
normalization warnings for edited files; `git diff --check` reported no
whitespace errors.

## 13. Intermediate failures

The first focused build exposed that existing topology tests assigned the
formerly mutable `Level.Continuous` property, and the new test file initially
omitted the `PlayerInput` namespace import. The fixtures were changed to create
explicit topology configurations and the import was added. The focused run
then passed 9/9; the final full suite and build also passed.

## 14. Manual regression results

Neal completed the requested graphical regression pass. All five commands
passed for the Stage A behavior-preservation purpose, and no regression
attributable to the Level 1 configuration/initialization groundwork was
identified:

```powershell
dotnet run --project src/Tempest.Game -- --lives 10                         # PASS
dotnet run --project src/Tempest.Game -- --spiker-test --lives 10           # PASS
dotnet run --project src/Tempest.Game -- --tanker-test --lives 10           # PASS
dotnet run --project src/Tempest.Game -- --fuseball-test --lives 10         # PASS
dotnet run --project src/Tempest.Game -- --pulsar-test --lives 10           # PASS
```

The Tanker scenario passed for Stage A regression checking. The separate
combined Tanker/bullet command and observations are recorded below; they do
not change the Stage A result.

## 15. Manual observations outside Stage A scope

Neither observation below was introduced by Stage A. They are separate,
deferred arcade-fidelity/playability items; no code or tuning was changed.

### Tanker firing

Current development policy remains: `--tanker-test` alone leaves the Tanker
silent, and Tanker firing is enabled by combining
`--tanker-test --enemy-bullets-test`.

Neal also ran:

```powershell
dotnet run --project src/Tempest.Game -- --tanker-test --enemy-bullets-test --lives 10
```

The one Tanker fired one shot during the observed run. Neal could not determine
whether there was enough time for it to fire a second shot before reaching its
release/rim area. Neal reports from arcade experience that Atari Tankers fire
individual shots with intervals and that at least two shots can occur before
the Tanker reaches the rim/release area. This is an existing arcade-fidelity
observation, not a Stage A failure.

Do not infer exact arcade ammunition count, firing interval, random
probability, initial delay or firing-height cutoff from this observation. The
exact current values remain Incitti-derived/reference behavior unless
separately measured.

### Blaster rim-movement smoothness

Neal reports that Blaster movement around the rim works, but he perceives a
very small delay between lane movements and says it is not completely as smooth
as the arcade spinner. The cause is unknown; no possible cause has been
established. This is a separate deferred observation, not a Stage A regression
or blocker. Revisit before higher-level gameplay, where tighter movement timing
may affect playability. No movement, input, sensitivity or timing changes are
part of this milestone.

## 16. Explicitly out of scope

No Level 2, square geometry, canflip, natural advancement, mixed production
waves, open-tube gameplay, dynamic palettes, bonus/extra-life policy, audio,
progression-entry presentation or next-level message was implemented.

## 17. Roadmap relationship

This completes Stage A groundwork from the approved architecture report. Square
hinge/canflip isolation and a real Level 1-to-2 transition remain separate
future milestones requiring their own behavior and validation gates.

## 18. Remaining architectural risks

The game launch configuration remains fixed to Level 1; configurable supply
fixtures validate runtime accounting but do not expose another playable level.
Static Level 1 depth/position assumptions, closed adjacency, renderer closure,
projection globals, and scenario-specific Tanker identity accounting remain
intentional constraints. The open topology count is metadata only. Numeric
pacing, deterministic sequences and future progression behavior remain
project decisions or unverified arcade behavior.
