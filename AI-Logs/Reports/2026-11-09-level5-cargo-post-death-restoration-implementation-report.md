# Level 5 cargo post-death restoration implementation

Date: 2026-11-09

Status: **PARTIAL / BLOCKED.** The Level 5-specific restoration candidate and
focused regressions are implemented, but final validation is incomplete.
Windows Application Control blocked test execution (`0x800711C7`). Manual
acceptance has not occurred.

## Confirmed cause and design

The required investigation traced the defect to unresolved, materialized
Tanker cargo being changed to `ReentryReserve` during death reconstruction,
released as cargo Eggs, then hatched through `CreateFreshTankerCargo` at
height zero. This is separate from ordinary Level 5 species selection; the
Level 5 admission candidate correctly excludes fresh ordinary Flippers.

The implementation adds a canonical-Level-5-only snapshot path. Immediately
before death reset clears runtime collections, active and pending-shot cargo
Flippers that remain unresolved are captured as immutable value snapshots
keyed by stable cargo ID. The snapshot preserves lane/height, pivot and route
state, direction, firing delay, and any pending zap timer without retaining a
runtime object reference. The collision-completion signal is intentionally
cleared on restoration so the collision that caused the death is not replayed.

The ledger distinguishes snapshots awaiting restoration from ordinary
`ReentryReserve`. They remain unresolved completion obligations and are
represented by the snapshot queue, not an Egg. Restored children transition
back to Active at the existing post-death reset boundary. If the seven-place
physical limit is full, snapshots remain queued and are retried on active
simulation ticks before ordinary admission; waiting consumes no randomness.
This is a **PROJECT TRANSLATION**: captured in-tube placement is authorized
for this candidate, not claimed as literal ROM reconstruction.

The path applies only when the configuration is the canonical
`LevelConfiguration.Level5Production` instance. Latent, terminal, and resolved
cargo are excluded; pending shot activation and still-unresolved zapper cargo
are captured through their existing runtime Flipper representation. Other
levels and copied configurations keep their existing reconstruction route.

## Files changed for this correction

- `src/Tempest.Game/Gameplay/Flipper.cs` — value snapshot and restoration of
  cargo movement/lifecycle state.
- `src/Tempest.Game/Levels/Level3WaveLedger.cs` — distinct restoration-reserve
  state and identity transitions.
- `src/Tempest.Game/Levels/Level.cs` — forwards explicitly selected cargo
  identities into ledger reconstruction.
- `src/Tempest.Game/TempestGame.cs` — canonical Level 5 capture, capacity-
  gated restoration, runtime representation validation, and all-rim waiting
  guard.
- `tests/Tempest.Game.Tests/Level3WaveLedgerTests.cs` — focused resolved-
  sibling and pending-shot ledger case.
- `tests/Tempest.Game.Tests/Level5ProductionTests.cs` — replaced the
  superseded `ReconstructedLevelFiveTankerCargoHasDirectionBeforeSpikeGatedPivot`
  Egg expectation with a canonical Level 5 in-tube restoration regression;
  added movement/zap snapshot coverage.
- `AI-Logs/Prompts/2026-11-09-level5-cargo-post-death-restoration-implementation-prompt.md`
  — complete attached task prompt.

The pre-existing Level 5 admission candidate files and its prior prompt and
reports remain intact and uncommitted. No accepted behavior documentation was
changed.

## Tests and validation

- The first narrow Debug invocation compiled the game and test assemblies but
  both selected tests failed on test setup assumptions. The ledger case was
  corrected to activate the pending-shot sibling before resolving it; the
  integration case was changed to use canonical Level 5 and a deterministic
  natural cargo release.
- A subsequent focused command began with successful project compilation but
  VSTest could not load
  `tests\Tempest.Game.Tests\bin\Debug\net9.0\Tempest.Game.Tests.dll`:
  “An Application Control policy has blocked this file” (`0x800711C7`).
  No tests ran in that invocation. The runner's zero exit code did not
  represent a pass. Later test edits and the final snapshot simplification
  have not been compiled or executed.
- Following `.github/copilot-instructions.md`, no equivalent test/build retry,
  Release route, stale-binary substitution, or security bypass was attempted.
- Therefore focused cargo tests are **NOT PASSING / NOT VALIDATED**; affected
  Level 5 and death/reconstruction suites, full solution tests, and the full
  Debug build were not run.
- Final `git diff --check`: **PASS**. The complete tracked diff was reviewed.

## Remaining work and manual acceptance

Open implementation/validation questions: execute the focused tests and
required affected/full Debug suites in an authorized environment; verify the
restoration-capacity wait and pending-zap cases through gameplay tests; then
review the complete final diff and repository state. The exact restored
position remains a project-selected translation requiring Neal's arcade
comparison. Do not treat this candidate as accepted.

After automated validation is available, Neal's manual command is:

`dotnet run --project src\Tempest.Game -- --level 5 --lives 10`

Acceptance scenario: allow a Tanker to release Flippers, leave at least one
unresolved, die and reenter, then verify no individual Flipper appears from
the deep/bottom end and the surviving cargo behavior looks natural; repeat
after another death if practical and confirm ordinary Tanker/Spiker admission
and other accepted Level 5 systems remain intact. Manual acceptance status:
**PENDING**. The game was not launched.

## Git state

No commit or push was made. The existing Level 5 candidate and this correction
remain uncommitted and unstaged on top of
`62c66e0ab19436ea162935627021154be21e0a41`. No Level 6 work or unrelated
cleanup was started.
