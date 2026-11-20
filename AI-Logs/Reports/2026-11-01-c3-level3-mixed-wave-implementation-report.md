# C3a — Headless Level 3 mixed-wave implementation report

## Status and boundary

Implemented from the existing interrupted C3a worktree, starting at
`6d15165 Correct and validate isolated Tanker firing timing`. This is a
headless production-shaped model, not completed C3 access or manual acceptance.
The new model is available through the existing configuration-based
`TempestGame(LevelConfiguration)` construction path only.

No `--level3-test`, Program routing, production HUD/bullet-rendering policy,
graphical access, manual validation, Level 2-to-Level 3 progression,
`LevelProgression` change, or Level 2 exit-commit change was added. The
Level 3 geometry diagnostic remains Flipper-only. `Reference/incitti` and
`TEMPEST-BEHAVIOR-SPEC.md` are unchanged. No commit or push was made.

## Implementation

- `LevelConfiguration.Level3Mixed` shares the exact `Level3Plus.Definition`,
  retains the plus board and 15-original supply, and selects the mixed
  Flipper/Tanker composition. The diagnostic configuration is unchanged.
- Only the mixed configuration uses randomized lane/direction Egg release,
  six-at-50 queued releases, source-shaped approach gating and mixed type
  selection. Level 1, Level 2 and diagnostic schedules remain on their prior
  paths.
- `Level3WaveSelection` preserves inclusive source ranges, negative clamping,
  rejected enemy types, near-end overwrite draw order, the simultaneous
  six-Tanker cap and retrying two-Flipper cargo selection. Previously selected
  types survive reconstruction; blocked ready Eggs are skipped without
  spinning or consuming a reroll.
- `Level3WaveLedger` owns original IDs 0..14 and separately tagged cargo IDs
  `(owner, ordinal)`. A Tanker's pair is allocated only on first Tanker
  commitment. Explicit state transitions, owner/type checks, duplicate
  detection and exact runtime-location set validation reject missing,
  extraneous, duplicated, terminal-active and latent/materialized collisions.
- `TempestGame` routes the mixed composition through real Egg hatching, shots,
  ordered Super Zapper targets, Tanker updates/releases, cargo activation,
  death reconstruction, all-rim and completion logic. Production Tankers get
  the accepted firing ammo/timer and inclusive firing delegate without
  `--enemy-bullets-test`; ordinary and cargo Flippers remain silent unless
  that optional flag is enabled.
- Natural, shot and zap Tanker outcomes retain their distinct escape/destroy
  scoring and reuse the carrier's existing cargo IDs. Shot children stay in
  pending activation until the established activation point. Zap children
  receive timer 8. Death reconstruction retains selected types and terminal
  outcomes, re-enters unresolved materialized cargo before originals, drops
  runtime timers and validates exact unresolved-set conservation.
- Level 3 ordinary completion requires all original and cargo IDs resolved
  with no queue, Egg, active enemy, latent cargo or pending activation.
  All-rim requires exact namespace-tagged unresolved-set equality and rejects
  latent/pending obligations before resolving rim-cleared outcomes. The
  configuration-based headless model remains terminal after exit travel.
- Runtime representation validation now also runs on fresh mixed-wave setup
  and rejects more than six active Tankers at stable game boundaries.
  `RebuildAfterDeath` explicitly checks unresolved IDs, selected types,
  terminal outcomes, latent owner/ordinal pairs and identity count.

Reference-derived board, composition, scheduling and firing inputs remain
**REFERENCE VERIFIED**, not arcade-verified Level 3 behavior. Identity
retention, representation, reconstruction ordering, active-play scheduling
clock and headless-only access remain **PROJECT DECISIONS / DEVELOPMENT ONLY**.
Previously accepted C1/C1.5 geometry, cargo route/pivot and C2 firing
semantics were not retuned.

## Deterministic tests

`Level3MixedWaveTests` and `Level3WaveLedgerTests` exercise production
configuration sharing, source-ranged scheduling and selection, the six-live
cap, later Tanker selection after capacity frees, eight-step Egg motion,
reconstruction release phase, actual Tanker hatching/firing, silent ordinary
Flippers, natural/shot/zap carrier outcomes, cargo priority and repeated
identity reconstruction, multi-carrier Super Zapper resolution, invalid
representations, death-set conservation and terminal headless replay.

The new mixed-wave test class and existing test classes that create a
`TempestGame` or inspect global projection state use the existing
`"Level 2 tests"` non-parallel xUnit collection. The full suite exposed a
race between `TempestGame`'s static projection reset and the Super Zapper
rendering assertion; serializing these existing projection-sensitive tests
removes that nondeterminism without changing runtime rendering.

## Interrupted-agent recovery

- The previous VS Code agent stalled during C3a; Neal canceled the request
  after no visible progress.
- The seven-file partial worktree was recovered in place. Its original
  `git diff --check` was clean, and no existing C3a source/test work was
  restored, discarded or overwritten wholesale.
- The targeted test initially failed to compile because
  `BoundedCompositionRandom` had been declared privately inside
  `InclusiveRangeScript`, but was referenced from the containing test class.
  Its test call also passed `LevelConfiguration` into a constructor overload
  reported as expecting `bool`; the configuration-based constructor exists,
  and this was a cascading overload-resolution diagnostic from the inaccessible
  helper. The helper was moved to the test-class scope; no duplicate random
  helper was introduced.
- While repairing that test, `Assert.NotEmpty` was replaced with
  `Assert.Contains`, removing xUnit2030.
- Additional incomplete test edits were found and corrected: a production
  firing script omitted the scheduled six-Egg tick-50 release; a multi-carrier
  assertion incorrectly assumed only six Tankers could ever be selected in a
  wave, despite the approved simultaneous-only cap. The script now includes
  the release draws, and the test verifies that later carriers are selected
  while the active count remains at or below six.
- A named `PlayerInput` test construction initially omitted required positional
  fields; it was corrected to provide all required arguments.
- The interrupted runtime path had no explicit six-active-Tanker assertion in
  its stable representation validator and no startup representation check;
  both were added. Death reconstruction now verifies exact identity/type/
  terminal-set conservation after rebuilding.
- The broader suite initially reproduced a projection race in
  `TubeFlashChangesOnlyColorAndExpiresAfterSixtyDisplaySteps`. The isolated
  test passed, while reruns could pass or fail depending on parallel game
  initialization. Projection-mutating game tests were placed in the repository's
  existing non-parallel collection; the complete suite then passed.

## Validation results

- Recovered target:
  `dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~ProductionWaveEnforcesSixSimultaneousTankers"`
  — 1 passed.
- Mixed-wave and ledger tests — 29 passed after recovery additions.
- C3a focused group (mixed wave, ledger, Tanker, cargo, Super Zapper, all-rim,
  completion, geometry, directed rotation and progression) — 209 passed.
- Configuration/Level 2/input focused group — 93 passed.
- Full solution tests:
  `dotnet test TempestCSharp.slnx --no-restore` — 520 passed, 0 failed,
  0 skipped.
- Solution build:
  `dotnet build TempestCSharp.slnx --no-restore` — succeeded, 0 warnings,
  0 errors.
- `git diff --check` — passed. `Reference/incitti` and
  `TEMPEST-BEHAVIOR-SPEC.md` are unchanged. HEAD remains
  `6d15165 Correct and validate isolated Tanker firing timing`; no commit or
  push was made.

## Final reconciliation and waived all-rim fixtures

The current implementation and tests cover the requested deterministic
acceptance checkpoints, including retained Tanker selection at capacity,
independent hatching, overlapping carrier outcomes, Zapper charge/timer
ordering, firing after re-entry, completion blockers, exact identity-set
conservation, all-rim mixtures, player-collision ordering and terminal Level 3
behavior. No genuine executable acceptance gap or C3a blocker remains.

Two direct all-rim fixtures are intentionally waived as production-unreachable:

1. An active Tanker cannot reach the all-rim threshold while retaining latent
   cargo. Its natural-release threshold is `Level.Depth - 15` (385); on the
   first update with height greater than 385, Tanker processing releases the
   cargo and removes the carrier. All-rim evaluation runs later in that same
   active step, so no active latent-cargo Tanker can be evaluated at height
   400 or above.
2. Pending shot-activation cargo cannot remain pending at all-rim evaluation.
   Shot processing queues it earlier in the active step; `ActivateShotReleasedCargo`
   transfers it to the active Flipper list and clears the pending queue before
   player-collision checks and all-rim evaluation.

Neither state is manufactured using reflection, inaccessible-internal
mutation, fake simulation, a test-only production path/API or changed gameplay
ordering. The explicit all-rim latent/pending guards remain defensive. Their
safety is supported by lifecycle ordering, exact namespace-tagged ledger/runtime
representation validation and duplicate/missing-identity rejection, plus real
all-rim orchestration tests for valid mixtures, zero scoring, pending-Zapper
timer removal and player-collision precedence.

## Changed paths and review status

Expected tracked changes:

- `src/Tempest.Game/Gameplay/Egg.cs`
- `src/Tempest.Game/Gameplay/Flipper.cs`
- `src/Tempest.Game/Gameplay/Tanker.cs`
- `src/Tempest.Game/Levels/Level.cs`
- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/TempestGame.cs`
- `tests/Tempest.Game.Tests/InputSystemTests.cs`
- `tests/Tempest.Game.Tests/Level3GeometryTests.cs`
- `tests/Tempest.Game.Tests/LevelConfigurationTests.cs`
- `tests/Tempest.Game.Tests/SimulationTests.cs`
- `tests/Tempest.Game.Tests/SuperZapperTests.cs`
- `TEMPEST-DEVELOPMENT-NOTES.md`

New source/tests:

- `src/Tempest.Game/Levels/Level3WaveLedger.cs`
- `src/Tempest.Game/Levels/Level3WaveSelection.cs`
- `tests/Tempest.Game.Tests/Level3MixedWaveTests.cs`
- `tests/Tempest.Game.Tests/Level3WaveLedgerTests.cs`

The existing untracked `TEMP/Chat1.json` is preserved and excluded from the
C3a commit scope. The updated
`AI-Logs/Reports/2026-11-01-c3-level3-mixed-wave-implementation-report.md` is
ignored and not listed by Git. Additional tracked test attributes serialize
classes that share global projection state. `Reference/incitti` and the
behavior spec are unchanged. C3b/manual graphical access and C4/natural
progression were not implemented.

The interrupted work was safely recovered and the current implementation
passes the required gates. C3a is **complete and ready for a selective
commit**. No manual Level 3 gameplay acceptance is claimed. Suggested commit
message: `Implement headless Level 3 mixed-wave lifecycle`. No commit or push
was made.
