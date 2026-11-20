# Level 1 to Level 2 progression implementation

Implementation date: 2026-10-31
Starting checkpoint: `7416dce Implement isolated Level 2 Flipper canflip behavior`
Scope: normal production Level 1 successful-exit progression to the existing validated square Level 2.
Git: no commit created.

## 1. Purpose and starting checkpoint

Connected normal Level 1 to a fresh natural Level 2 runtime. The approved
progression analysis was the implementation design. The checkpoint was clean
at `7416dce`; the implementation did not change `TEMPEST-BEHAVIOR-SPEC.md`.
The exact supplied prompt is preserved at
`AI-Logs/Prompts/2026-10-31-level1-to-level2-progression-implementation-prompt.md`.

The current source matched the approved design's key lifecycle assumptions:
standard `ExitZoom` already distinguishes successful travel from clear,
delay, development Spike exit, death and replay; `InitializeFreshWave` already
owns entity, projection, callback, ledger and Super Zapper reset. No conflicting
source behavior required a design change. One normal-Level-1 terminal/replay
assumption also appeared in a Hurry-Up test, but that test uses the Fuseball
and Pulsar development scenarios; it was retained as terminal/replayable rather
than treated as a normal-campaign expectation.

## 2. Evidence boundary

Evidence labels remain separate:

- **ARCADE VERIFIED**: Neal's prior Level 1 observations, qualitative
  isolated Level 2 canflip behavior, and the broad observable natural Level
  1-to-2 transition recorded after this implementation.
- **REFERENCE VERIFIED**: literal Incitti transition/setup and source-derived
  tuning, not proof of Atari behavior.
- **PROJECT DECISION**: transition, fresh-level resource reset, carry policy,
  and terminal Level 2 behavior selected for this recreation.
- **DEVELOPMENT ONLY**: direct scenario launches, test fixtures and automated
  tests.

Neal manually validated the broad observable natural transition after
implementation. This does not promote implementation internals or unmeasured
timing to **ARCADE VERIFIED**.

## 3. Successful-travel commitment

Progression is evaluated only in the ordinary successful standard `ExitZoom`
branch after the movement update, when the updated `Player.Depth < 10`.
An explicit outgoing-state guard (`ExitZoom`) and configuration guard
(`LevelConfiguration.Level1`) require normal-campaign eligibility before
resolving the next production configuration.

The next Level is installed through fresh initialization during that same
update. The outgoing Level is replaced, projection is restored, the new state
is `Playing`, and the method returns without another outgoing Level update.
The installed Level cannot be advanced twice by later updates.

Wave clear, all-rim commitment, `BeginExit`, `ExitDelay`, intermediate
`ExitZoom`, `PlayerDying`, reconstruction, Game Over, terminal Level Complete,
R replay, and Spike avoidance/reverse/retry do not resolve production
progression.

## 4. Production resolver and configuration reuse

Promoted `LevelConfiguration.Level2SquareTest` to the production-neutral
`LevelConfiguration.Level2Square`. It is still the only square Level 2 object.
The new narrow resolver maps the production Level 1 object to this object and
returns no next configuration for Level 2 or other identities.

The configuration retains internal progression identity 1, displayed level 2,
board 1, palette 0, closed 16-lane square geometry, supply 12, four initial
Eggs, eight reserve units, Flipper-only composition, B1 endpoint hinges and B2
canflip behavior. `--level2-test` selects this exact same configuration
object; it does not duplicate geometry or tuning.

## 5. Normal and development policy

Natural progression is enabled for normal launches, including `--lives`,
`--enemy-bullets-test`, and both options together. The enemy-bullet behavior
remains active after progression. Neither option selects a development
campaign.

Progression is disabled for `--level2-test`, `--spiker-test`, `--tanker-test`,
`--fuseball-test` and `--pulsar-test`, independent of their Level 1
configuration identity. Scenario conflict handling is unchanged.

## 6. Score and lives carry

Successful Level 1 exit carries the current score and `Player.Lives`
unchanged. The configured starting-lives value is not reapplied during
progression. No level-completion bonus, bonus life or other score award is
added.

## 7. Super Zapper recharge

Fresh Level 2 setup reuses `InitializeFreshWave`'s existing
`SuperZapper.Reset()`: two charges and zero flash ticks, regardless of whether
Level 1 entered exit with zero, one or two charges. Active flash is cleared at
installation. This fresh-level reset is a **PROJECT DECISION** aligned with
the established fresh-wave policy; it is not a claim about exact Atari flash
behavior. Ordinary life reconstruction continues to preserve remaining
charges.

## 8. Fresh Level 2 accounting and runtime state

Automated checks verify the installed Level 2 configuration identity,
`Playing`, display 2, board 1/palette 0, square topology, canflip, and the
12-unit wave with four Eggs, eight reserve, zero destroyed, zero rim-cleared
and 12 remaining. The runtime has no Level 1 Flippers, shots, enemy bullets,
Spikes, Tankers, Spikers, Fuseballs, Pulsars or pending Tanker cargo ledger.
Hurry-Up and death state are cleared; the player returns to configured lane 7,
offset 0.5 and depth 400; projection is K70/Z5. Fresh initialization resets
the Zapper and existing callbacks/collections/ledgers without adding a generic
clock reset.

## 9. Death and Game Over policy

Level 1 death/reconstruction remains on Level 1 and cannot advance. Natural
Level 2 nonfinal death reconstructs the same Level 2 configuration, preserves
score and remaining Zapper charges, and does not fall back to Level 1.
Game Over in a normal campaign restarts at the original Level 1 launch
configuration, resets score and restores configured starting lives.

## 10. Natural Level 2 terminal and R policy

Level 2 kill and all-rim completion both reach terminal `LevelComplete`.
There is no Level 3 configuration or synthesized Level 3 transition. Natural
Level 2 displays **LEVEL 2 COMPLETE**, **NEXT LEVEL NOT IMPLEMENTED**, and
**PRESS R TO START A NEW CAMPAIGN**. R starts a new Level 1 campaign, resets
score and restores configured starting lives.

Direct `--level2-test` remains terminal and replays its selected Level 2 with
existing score/lives replay semantics. Other development scenarios remain
terminal and replay their own launch scenario.

## 11. Projection, renderer and UI

The new configuration is active before `RunOneStep` returns at the successful
exit boundary, so the first following draw uses square geometry. Existing
fresh initialization restores K70/Z5 and player pose; no renderer-owned level
cache was added.

The HUD shows the active displayed level. The window title changes when the
active displayed level changes; direct `--level2-test` remains explicitly
development-only in both its title and HUD. Normal Level 1 is identified
before progression.

## 12. B2 behavior reuse

Natural Level 2 uses the same immutable configuration and the existing B2
Flipper update path. No canflip decision, direction sampling, turn increment,
pause timing, height behavior, square hinge, collision, optional bullets,
Super Zapper or all-rim gameplay was retuned or forked.

## 13. Development-scenario preservation

Direct Level 2, Spiker, Tanker, Fuseball and Pulsar replay/terminal behavior
remains covered by the existing tests. Spiker reverse/retry remains separate
from the standard progression handler. Normal Level 1 with
`--enemy-bullets-test` remains progression-eligible and retains the option.
The Hurry-Up Fuseball/Pulsar replay test was verified to remain terminal.

## 14. Bonus and presentation omissions

Explicit omissions remain: no level-completion bonus, `startbonus`, bonus
life, source Begin/Ready entry animation, source entry-position bonus shift,
or audio. Score simply carries into Level 2. These are incomplete
presentation/economy features, not arcade-equivalent behavior.

## 15. Files changed

Gameplay/configuration/UI:

- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/Levels/LevelLoader.cs`
- `src/Tempest.Game/Levels/LevelProgression.cs` (new)
- `src/Tempest.Game/TempestGame.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`

Tests:

- `tests/Tempest.Game.Tests/LevelConfigurationTests.cs`
- `tests/Tempest.Game.Tests/LevelProgressionTests.cs` (new)
- `tests/Tempest.Game.Tests/Level2SquareTests.cs`
- `tests/Tempest.Game.Tests/Level2CanFlipTests.cs`
- `tests/Tempest.Game.Tests/SimulationTests.cs`
- `tests/Tempest.Game.Tests/SuperZapperTests.cs`

Documentation:

- `README.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`

Requested prompt/report files are under `AI-Logs/`, which is ignored by Git
in this checkout.

## 16. Obsolete tests intentionally updated

Updated the approved normal-Level-1 terminal/replay expectations:

- `FreshReplayReplacesLevelAndPreservesScoreAndLives` became
  `SuccessfulExitInstallsLevel2AndPreservesScoreAndLives`.
- `ExitZoomIgnoresInputRestartAndFurtherClearTriggers` now verifies
  progression only after successful travel.
- `ZoomCompletesOnlyBelowTenThenRestoresProjectionAndFreezes` now verifies
  fresh Level 2 installation below the threshold.
- `ExitReplayRestoresProjectionDepthEggsAndCountersButKeepsScoreLives` became
  `SuccessfulExitCarriesSessionAndInstallsFreshAccounting`.
- `ExitRendererTracksPlayerDepthAndAllProjectedVerticesStayFinite` now checks
  circle exit geometry before commit and square geometry after it.
- `LevelCompletionFreezesAndReplayRetainsScoreAndLives` became
  `NaturalLevel2CompletionIsTerminalAndRStartsANewCampaign`.
- `EntireEggWaveCanBeDestroyedWithSameTickExitThenCompletion` now verifies a
  fresh Level 2 instead of normal Level 1 terminal completion.
- `RestartAndReplayRestoreChargesWhileInactiveInputIsRejected` now explicitly
  covers direct-development Level 2 replay and Game Over restart.

The additional Hurry-Up test found during search was determined to use
development Fuseball/Pulsar scenarios; its terminal replay expectation remains
unchanged and is covered by the final suite.

## 17. Tests added

Added deterministic checks for the exact post-movement commit, the no-advance
clear/delay/intermediate-zoom boundaries, once-only replacement, kill-only,
all-rim and mixed routes, resolver limits, shared Level 2 configuration
identity, session carry, `--lives`/enemy-bullet orthogonality, zero/one/two
remaining Zapper charges and flash clearing, stale Flipper/zap-target cleanup,
fresh accounting/projection/pose, death and Game Over policy, Level 2 terminal
kill/all-rim completion, and new-campaign R behavior.

## 18. Focused/full/build/diff results

| Validation | Result |
|---|---:|
| Progression + Level configuration selectors | 27 passed, 0 failed |
| New progression selector | 17 passed, 0 failed |
| Combined Simulation, Level 2 B1/B2, Super Zapper, configuration and progression selectors | 409 passed, 0 failed |
| Full solution tests (`dotnet test TempestCSharp.slnx --no-restore`) | 409 passed, 0 failed, 0 skipped |
| Solution build (`dotnet build TempestCSharp.slnx --no-restore`) | Succeeded, 0 warnings, 0 errors |
| `git diff --check` | Passed |

Focused filters overlap and are not additive. The full suite includes the
existing 391-test checkpoint coverage plus new/expanded tests.

## 19. Intermediate failures

- The first focused test build caught a misplaced test-method brace; the next
  compile caught an extra closing brace and missing test references. These
  were corrected before the focused suites passed.
- The initial progression tests had an incorrect exit-update count and a
  mixed-route fixture that did not hatch already-released Eggs. The test
  boundary and fixture were corrected; production behavior was not changed to
  satisfy those assertions.
- The first combined 408-test run exposed two Hurry-Up development cases being
  routed through a normal-campaign test helper. The tests were corrected to
  preserve their terminal Fuseball/Pulsar replay behavior; the rerun passed
  408/408.
- The final full suite and build passed with no failures or warnings.

## 20. Manual validation results

Neal completed end-to-end manual validation on 2026-10-31 by running:

```powershell
dotnet run --project src/Tempest.Game -- --lives 10
```

### Natural transition

Neal verified:

- Normal Level 1 tube exit occurs.
- No terminal Level Complete/R pause occurs between Level 1 and Level 2.
- Square Level 2 begins automatically and the displayed identity changes.
- Square geometry is immediate; no stale Level 1 frame was observed.
- Score and current remaining lives carry unchanged.
- In the observed example, the run began with 10 lives, lost two in Level 1,
  finished Level 1 with 8, and began Level 2 with 8. This is an example, not a
  fixed life-count rule.
- Four initial Level 2 Eggs are visible and the wave has 12 units.
- Super Zapper is restored to two usable charges.
- Previously validated Level 2 square/canflip behavior remained normal; no
  regression was observed.

These broad observable results are **ARCADE VERIFIED** within Neal's manual
comparison. No transition ticks were measured. The exact
`Player.Depth < 10` commit boundary, update ordering, fresh-runtime and
accounting reset, exact Super Zapper recharge/flash reset timing, Egg setup
sequencing, player start offset, callback/list cleanup and projection reset
remain **REFERENCE VERIFIED / PROJECT DECISION** as applicable, not arcade
observations.

### Level 2 terminal behavior

Neal verified that no Level 3 appears, the terminal presentation shows
**LEVEL 2 COMPLETE**, **NEXT LEVEL NOT IMPLEMENTED**, and
**PRESS R TO START A NEW CAMPAIGN**, and R starts a fresh Level 1 campaign
with score reset. The terminal policy is a **PROJECT DECISION**; it does not
claim that Atari Level 2 is terminal.

### Deferred Atari between-level approach

Neal separately reports that Atari shows a brief first-person approach
through space after the Blaster exits a tube. The next tube begins
small/distant and grows as the player approaches; the Blaster is visible on
the distant rim and spinner movement moves it around the rim during approach,
before combat/enemy engagement begins. The rim movement makes this
gameplay-relevant, but Neal does not consider its absence a blocker for current
progression development. This presentation is deferred and not implemented.
Duration, zoom rate, projection formulas, enemy-start timing and firing
enablement are not inferred or claimed as measured.

Regression commands:

```powershell
dotnet run --project src/Tempest.Game -- --level2-test --lives 10
dotnet run --project src/Tempest.Game -- --level2-test --enemy-bullets-test --lives 10
dotnet run --project src/Tempest.Game -- --spiker-test --lives 10
dotnet run --project src/Tempest.Game -- --tanker-test --lives 10
dotnet run --project src/Tempest.Game -- --tanker-test --enemy-bullets-test --lives 10
dotnet run --project src/Tempest.Game -- --fuseball-test --lives 10
dotnet run --project src/Tempest.Game -- --pulsar-test --lives 10
dotnet run --project src/Tempest.Game -- --enemy-bullets-test --lives 10
```

## 21. Explicit out-of-scope items

No Level 3, generic campaign framework, open tubes, dynamic palettes, board
rotation, production Tankers/Spikers/Fuseballs/Pulsars, mixed waves, bonus
score/lives, audio, source entry animation, Tanker firing changes, Blaster
input/smoothness tuning, or gameplay tuning was implemented. The behavior
specification was updated after the manual validation; no code or tests were
changed in that documentation update. No commit was created.

## 22. Remaining arcade uncertainties

The broad visible natural Level 1-to-Level 2 sequence and terminal/restart
presentation are manually validated within the evidence scope above.
Implementation internals remain unverified against Atari: the successful
`Player.Depth < 10` boundary, exact update ordering, fresh-runtime/accounting
reset, Zapper recharge/flash timing, Egg sequence, player offset, callbacks,
list cleanup and projection reset. The between-level approach presentation is
also deferred and awaits future implementation; its duration, zoom rate,
projection formulas, enemy-start timing and firing enablement remain
unmeasured. Previously recorded isolated Level 1/all-rim and qualitative
Level 2 canflip observations retain their original scope. Incitti-derived
values remain **REFERENCE VERIFIED**; deferred Tanker-firing and
Blaster-smoothness observations remain out of scope.
