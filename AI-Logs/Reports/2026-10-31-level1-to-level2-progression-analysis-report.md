# Level 1 to Level 2 progression analysis

Analysis date: 2026-10-31
Authority checkpoint: `7416dcef4a82d5eae9f69ce7a5530b6de06fdb3b`
Scope: analysis only; no gameplay, tests, authority documents, configuration, or Git state changed. The supplied prompt is preserved verbatim at [2026-10-31-level1-to-level2-progression-analysis-prompt.md](../Prompts/2026-10-31-level1-to-level2-progression-analysis-prompt.md).

Evidence labels follow [TEMPEST-BEHAVIOR-SPEC.md](../../TEMPEST-BEHAVIOR-SPEC.md): **ARCADE VERIFIED**, **REFERENCE VERIFIED**, **PROJECT DECISION**, and **DEVELOPMENT ONLY**. Incitti source describes Incitti, not Atari behavior. Passing C# tests establish C# contracts, not arcade fidelity.

## 1. Executive recommendation

**GO** for a bounded implementation of normal Level 1 → Level 2 progression.

The smallest safe change is to resolve the already validated square/canflip configuration at the single successful standard-exit completion point, install it through the existing fresh-wave lifecycle, and update level identity in the HUD/window title. Keep Level Complete terminal for direct development scenarios and natural Level 2. Do not advance on accounting completion, exit start, life reconstruction, spike reverse/retry, or Game Over.

Reuse and promote the existing `Level2SquareTest` data as the sole square Level 2 configuration; do not copy it. Keep `--level2-test` as a development-only direct-access/replay mode. A narrow `Level 1 → Level 2` resolver is sufficient; Level 3, a generic campaign framework, and changes to either level's gameplay are unnecessary.

The exact end-to-end progression transition has not yet been manually validated. Existing **ARCADE VERIFIED** evidence covers broad all-rim behavior and qualitative Level 2 canflip behavior in their already-tested contexts, not the new transition, its setup, score/lives carry, or zap recharge.

## 2. Current C# transition flow

The core implementation is in [TempestGame](../../src/Tempest.Game/TempestGame.cs#L125), with command-line and window setup in [Program.cs](../../src/Tempest.Game/Program.cs#L1).

| Phase | Current behavior | Progression implication |
|---|---|---|
| Initial launch | Constructor selects `LevelConfiguration.Level1` unless `level2Test` is set, then `InitializeFreshWave` installs a new runtime in `Playing`. | No-flag startup is Level 1. |
| Active play | `RunOneStep` processes player input, zap, shots/bullets, Egg release/hatching, enemies, collision, all-rim completion, Hurry-Up, and a final kill-completion check. | Completion is based on resolved wave accounting, not an advancement event. |
| Kill completion | `Level.IsComplete` requires all configured units resolved, no reserve/Eggs, and no active counted enemies. `CheckForLevelComplete` calls `BeginExit`. | The exit begins only after the wave is genuinely resolved. |
| All-rim completion | `TryCompleteAllRim` waits for exhausted incoming supply and all active counted enemies to reach the rim, then records non-kill rim clears and calls the same `BeginExit`. Player collision checks precede this all-rim commit. | Kill, all-rim, and mixed outcomes converge on the same exit path. |
| `ExitDelay` | Normal standard exit waits 21 updates (`exitDelayTicks > 20`), then enters `ExitZoom`. | Too early to advance; no new level is installed. |
| `ExitZoom` | Standard exit moves the player down the tube. `UpdateExitZoom` changes the state to `Complete` only after the updated `Player.Depth < 10`, then restores projection. | This is the current successful-travel boundary. |
| `Complete` | State freezes. R invokes `InitializeFreshWave`, which reinstalls the original `launchConfiguration`, preserving score and current lives. | No current-level increment or production next-level dispatch exists. |
| `PlayerDying` / Game Over | A life is deducted at the end of the death interval. Nonfinal death reconstructs unresolved units on the same `Level`; Game Over clears runtime entities. R calls `ResetGame`, clearing score and restoring `startingLives`, then installs the launch configuration. | Neither death nor restart constitutes successful travel. |

`BeginExit` clears shots and enemy bullets and removes bullet collision-order entries. Standard exit does not update enemies or run their collision checks, and ordinary Level 1 has no Spikes. Exit travel therefore has no normal Flipper interaction that can newly kill the player. The Spiker scenario is different: it has `AvoidSpikes`, reverse/retry, and a separate exit path.

Current fresh-wave initialization creates a new runtime but is bound to the readonly launch configuration. It resets projection, player pose and shot timer, death/exit counters, Hurry-Up, Super Zapper, shots, bullets, enemy collections, collision-order entries, Fuseball animation, popups, and Tanker ledgers. It does **not** reset the score or lives. The implementation and ownership boundary are visible in [InitializeFreshWave](../../src/Tempest.Game/TempestGame.cs#L243).

Clock ownership is intentionally mixed: `pulsePhase`, `Pulsing` and `pulseZh` survive fresh-wave installation; `AdvancePulseClock` advances the phase during active play only. `fuseballAnimationTicks` resets on fresh initialization and advances only in the direct Fuseball development wave. Natural Level 1/2 is Flipper-only, so that animation is inactive. Keep these current C# policies rather than resetting every clock as part of this transition.

## 3. Literal Incitti Level 1 → Level 2 transition

**REFERENCE VERIFIED**, not an arcade claim. The relevant literal code is the Level state update in [tempest.bmx](../../Reference/incitti/tempest.bmx#L1311), its `LEVEL_COMPLETE` branch at [line 1403](../../Reference/incitti/tempest.bmx#L1403), and `ClearLevel` / setup at [line 4137](../../Reference/incitti/tempest.bmx#L4137).

| Concern | Literal source behavior at the first normal transition | What is not established about Atari |
|---|---|---|
| Commit and identity | The player crosses below depth 10 during `LEVEL_ZOOMING`; the source first sets `LEVEL_COMPLETE`. On the next Level update, the `LEVEL_COMPLETE` branch runs, increments `current_level` from 0 to 1, derives `current_board = current_level Mod 16` and `current_color = current_level / 16`, then sets up the next board. | The source is not proof of the arcade's internal state sequencing. |
| Score, lives, bonus | Ordinary score and remaining men are not reset by `ClearLevel` or setup. The first completion guard calculates `startbonus` from the outgoing level, adds it to score, and adds 200 to entry `position`; Level 1's formula result is zero. `bonuscollected` is not cleared by ordinary next-level setup, so do not claim a recurring per-level award from this source. Score-threshold extra-men logic is separate. | Bonus intent, bonus display, and arcade scoring/lives policy are not validated here. |
| Player | Setup sets the player lane to 7 and player depth to the source depth. `SetEdge` changes the lane but does not itself reset the sub-lane field. | C#'s offset `0.5` and its precise entry pose are project choices. |
| Super Zapper | Remaining charges survive until the next-level setup, which assigns supply 2. `superzapperdisplay` is a separate global counter and is not explicitly zeroed there. | C# may deliberately reset flash as part of its established fresh-wave policy. |
| Eggs/enemies and projectiles | `ClearLevel` removes Eggs, all baddies, shots, point/edge geometry, and the active counters. `SetUpEnemies` creates four Eggs from the new supply. At internal level 1 / board 1 the formula yields supply 12; reserve is 8. | Source RNG, exact Egg sequencing, and source list internals do not establish C# or arcade requirements. |
| Composition and tuning | Board 1 / internal level 1 is Flipper-only; source `canflip` is false only at internal level 0. The source-derived square config enables canflip. | Numeric source tuning remains **REFERENCE VERIFIED**, not measured arcade timing. |
| Accounting | Setup resets `hatchingeggs` and `rimit`; `SetUpEnemies` resets normal enemy counters and sets `rimit = False`. | Hidden accounting need not match source if the recreation preserves tested visible behavior. |
| Clocks | `globalclock` and Fuseball animation counters are global and are not reset by this transition. The Level counter `cnt` is reset. | Clock carry is source behavior, not an arcade guarantee. |
| Geometry/projection | `SetUpLevel` sets `K = 70`, board `CCenterY` and `YOFFSET`, and builds the incoming board geometry. The first transition's board and palette are square and palette 0. The source entry uses `position` 1500, or 1700 when its first bonus guard adds 200. | Entry zoom/position animation is not needed to validate a C# gameplay transition and is not directly established by Neal's existing observations. |
| Rotation/eligibility | `SetUpEnemies` sets `current_spin` from level index (zero at internal level 1), selects Flipper eligibility and source tuning, and clears hurry-up. | Later rotations and eligibility are outside this milestone. |
| Presentation | The source enters `LEVEL_BEGIN`, then `LEVEL_READY`; `position` and center animate during entry. | The C# build does not currently have this full Level Begin entry presentation. |

At source level 1 the normal formula is `10 + lv/2 + board*2 = 12`, with four initial Eggs. `SetUpEnemies` sets Flippers on, Tankers/Spikers/Fuseballs/Pulsars off, `hatchrate = lv/8`, `flipflipspeed = 8 + lv/5`, `flippausedelay = 21 - lv/5`, `flippausedelayrim = 24 - lv/5`, `flipclimbspeed = 2`, and `canflip = True`. These are source facts, not an arcade timing claim. C#'s four-active-Egg cap and deterministic Egg schedules remain its own policy.

## 4. Successful-travel commit point

The exact current C# boundary is the `Player.Depth < 10` branch of [UpdateExitZoom](../../src/Tempest.Game/TempestGame.cs#L1082). It runs after the exit movement update, not when the wave clears, `BeginExit` selects `ExitDelay`, or the delay expires.

Implement a single exit-result handler there, before the existing successful standard exit becomes terminal `Complete`:

1. Require that the outgoing runtime is the normal production Level 1 and that its state is the standard `ExitZoom` path.
2. Resolve only the validated Level 2 configuration.
3. Install a fresh Level 2 runtime in the same update, with projection restored and state `Playing`; return without allowing another Level 1 update.
4. If the run is a terminal development scenario or the outgoing level has no implemented next production configuration, retain the existing terminal `Complete` behavior.

The state replacement makes repeated exit updates impossible; retain an explicit state guard in the handler so a future call path cannot commit twice. There is no need for a persistent transaction ID or callback queue.

| State/event | Advance? | Reason |
|---|---:|---|
| Wave accounting reaches zero | No | This only qualifies the clear; it starts the exit. |
| `ExitDelay` | No | The travel has not succeeded. |
| `ExitZoom` before updated depth is below 10 | No | Travel is still reversible/incomplete. |
| Successful standard exit at updated depth below 10 | **Yes, exactly once** | Irreversible successful Level 1 tube exit. |
| Spike avoidance or reverse/retry | No | Spiker scenario is development-only; retry must retain its current Level. |
| `PlayerDying`, reconstruction, or Game Over | No | These do not resolve a new-level exit. |
| Terminal Level Complete / R | No | Replay/restart is a separate user action, never an implicit level increment. |

`UpdateSpikeExit` calls `UpdateExitZoom(completeWhenFinished: false)` and handles spike collision/reversal separately. Keep that route terminal. The ordinary Level 1 success handler must not be generalized to it.

## 5. Production configuration resolution

Use the existing Level runtime and explicit configuration boundary. The production resolver needs exactly one supported mapping:

| Current production identity | Result |
|---|---|
| `LevelConfiguration.Level1` (`ProgressionIndex == 0`) | Existing validated square/canflip Level 2 configuration (`ProgressionIndex == 1`) |
| Level 2 | No next configuration; remain terminal at completion |

Promote/rename `LevelConfiguration.Level2SquareTest` to a production-neutral name such as `Level2Square` or `Level2`. Keep one immutable configuration object with the established square points, supply 12, Flipper-only composition, canflip enabled and the B1 hinge mode. `--level2-test` must select that **same** object, not a copied set of geometry/tuning. Keep the direct-launch, replay and HUD semantics development-only in the launch/session mode, not in the configuration identity.

The Level 2 first wave must use the existing configuration and Flipper behavior: 12 total units, four Eggs and eight reserve, zero destroyed/rim-cleared outcomes, and no other enemy type. No Level 3 config or generic campaign table is needed.

## 6. Normal versus development launch policy

| Launch | Initial config | Natural 1→2 progression |
|---|---|---|
| No flags | Level 1 | Yes |
| `--lives <n>` | Level 1 | Yes |
| `--enemy-bullets-test` (with optional `--lives`) | Level 1 with the existing optional bullet behavior | **Yes**; the flag remains orthogonal |
| `--level2-test` | Direct Level 2 square | No; terminal and replayable at Level 2 |
| `--spiker-test`, `--tanker-test`, `--fuseball-test`, `--pulsar-test` | Existing isolated development setup | No |

Eligibility should exclude the enemy-specific/Level2 development scenarios, not `enemyBulletsTest` or `startingLives`. Existing flag conflict behavior remains unchanged. This preserves normal Level 1 + `--enemy-bullets-test` as the normal campaign with bullets enabled, rather than accidentally turning a gameplay option into a separate campaign.

## 7. Score and lives policy

**PROJECT DECISION:** carry the current score and remaining `Player.Lives` unchanged from successful Level 1 exit into Level 2. Preserve any `--lives` starting override as the session's configured starting lives; it neither blocks progression nor gets reapplied at the Level 2 boundary. Deaths before completion continue to deduct one life and reconstruct unresolved enemies on the same level. Death during ordinary standard exit is not reachable with current update/collision ownership; the development Spiker reversal is not progression.

Do not add bonus score or extra lives. C# has no active level-bonus caller, while Incitti's completion bonus/startbonus and score-threshold extra-man logic are source-specific and not needed for setup correctness. No additional lives should be invented.

## 8. Super Zapper policy

| Point | Policy |
|---|---|
| During Level 1, including exit | Keep remaining charges. Ordinary life loss does not recharge. |
| Fresh natural Level 2 installation | Reuse `InitializeFreshWave`'s existing `SuperZapper.Reset()`: exactly two charges and flash zero. No special-case recharge code. |
| Pending target at clear | All-rim resolution removes the target enemy object; its pending zap cannot later score. A kill-resolved enemy is already removed through its usual path. |
| Flash at installation | Reset to zero as the current C# fresh-level policy. This is a **PROJECT DECISION**; Incitti carries its separate display counter through setup. |
| Development Level 2 | Preserve current direct launch, replay, and fresh-charge semantics. |

The existing reset is the correct reusable point: it sets `InitialCharges = 2` and clears flash. Verify transition tests with zero and one remaining charge, both Level 2 activations, all-rim completion, pending zap removal, and flash reset. Do not make a new charge reset at clear or during death.

## 9. Accounting and fresh-state policy

Use a new `Level` created from the Level 2 configuration, plus the already existing fresh-wave cleanup. Do not reuse or mutate the Level 1 ledger.

| State | Expected immediately after install |
|---|---|
| Configuration and identity | Square Level 2, progression 1 / displayed 2 / board 1 / palette 0 |
| Supply | `EnemySupply = 12`; four initial Eggs; reserve 8 |
| Outcomes | `EnemiesDestroyed = 0`, `EnemiesRimCleared = 0`, `RemainingEnemies = 12` |
| Active entities | No Level 1 Flippers, Spikes, Tankers, Spikers, Fuseballs, Pulsars, Eggs, shots or enemy bullets; then exactly the four fresh Level 2 Eggs |
| Ledgers and queued work | Tanker original/cargo ledgers empty/inactive; pending cargo, shot-released cargo, old collision-order entries and old callbacks removed |
| Zap/death/exit | Two zap charges; flash 0; `DeathTicks = 0`; `exitDelayTicks = 0`; player at configured start pose |
| Hurry-Up | False |
| Score/lives | Carried unchanged |

`Level.Reset` already zeroes kill/rim-clear accounting and creates configured initial Eggs. `InitializeFreshWave` clears runtime entity collections, collision order, pending cargo, bullets/shots, Tanker ledgers, death/exit counters and Hurry-Up. It unsubscribes the old `SpikeCreated` handler before replacing the Level. Level 2 has no Spikes and is Flipper-only; ensure those scenario-specific ledgers remain empty/inactive rather than carrying an old run.

`CurrentLevel` is the authoritative runtime/renderer identity. No stale Level 1 enemy, pending zap target, shot, bullet or collision entry may survive replacement.

## 10. Death, exit, Game Over and restart policy

1. **Death before Level 1 wave completion:** `PlayerDying` freezes the simulation; a nonfinal life loss reconstructs on the same Level 1 instance. No resolver is called.
2. **Wave resolved but exit not successful:** `ExitDelay` and `ExitZoom` do not run normal enemy/player collision. Do not commit until the depth threshold.
3. **Lethal Flipper interaction at the exit-start boundary:** kill completion requires no remaining active counted Flipper; all-rim checks collisions before removing/clearing rim enemies. Both routes converge on the same safe exit. Do not move the commit earlier.
4. **Spiker direct/rim interaction:** this is development-only and may reverse/retry; retain its exact existing behavior and do not advance.
5. **Game Over before Level 1 commit:** remain Level 1. R resets score and restores configured starting lives, and starts the normal launch configuration.
6. **Death on natural Level 2:** reconstruct on the same Level 2 runtime/configuration; do not fall back to Level 1.
7. **Game Over after reaching natural Level 2:** R starts a new campaign at Level 1, resetting score and restoring `startingLives`; do not resume Level 2.
8. **Quit and relaunch:** no persistent campaign save exists; a no-flag launch starts Level 1.

The existing `launchConfiguration` remains useful for new-game restart and development replay. Fresh production advancement needs to pass the resolver's configuration into fresh installation rather than permanently changing the original launch selection.

## 11. Natural Level 2 terminal policy

Choose **A: Level 2 ends at terminal Level Complete**. The project implements no Level 3; do not invent a board, setup, composition, or transition. Use explicit presentation such as **“LEVEL 2 COMPLETE — NEXT LEVEL NOT IMPLEMENTED”** and identify the key action clearly. This is less misleading than silently looping to Level 1, replaying Level 2 as though it advanced, or synthesizing Level 3.

Natural Level 1 success skips its old terminal `LevelComplete` state and installs Level 2 immediately at the successful-travel boundary. The new Level starts in `Playing`, matching the current fresh-wave behavior. Do not add a new intermission or claim to reproduce Incitti's Begin/Ready entry animation.

## 12. R and replay semantics

| Context | R behavior |
|---|---|
| Natural Level 1 successful exit | No terminal state/R prompt; Level 2 installs automatically. |
| Game Over in normal campaign, including after Level 2 | Existing new-game behavior: score resets, starting lives restore, Level 1 launch configuration installs. |
| Natural Level 2 terminal completion | Start a new campaign at Level 1, resetting score and restoring `startingLives`. This is a **PROJECT DECISION**. |
| `--level2-test` terminal completion | Preserve existing development replay: reinstall the same direct Level 2 config, preserve score/lives. |
| Other enemy development scenario completion | Preserve its existing same-scenario replay behavior. |

R must not select a level or turn the development shortcut into a normal campaign selector.

## 13. Projection, geometry and rendering transition

The fresh Level 2 config already contains the square `LevelDefinition`; the renderer reads points, center and offset from `game.CurrentLevel` on each draw. `RestoreProjection` sets `Projection.K = 70` and `Projection.ZOffset = 5`. `ResetPlayerPosition` uses the incoming configuration's lane/offset and restores depth to 400. `Level.Position` is currently the shared constant 50; both configs use that same position, and there is no per-level animated position runtime in C#.

Therefore the existing fresh-wave reset plus installing a new configured Level is sufficient for board geometry and projection. Verify that the new Level is active before `RunOneStep` returns from the successful exit, so the first rendered frame after the commit reads the square, never one stale Level 1 frame. Do not add a parallel renderer cache or another Flipper path.

The current window title is chosen once at launch and names normal Level 1; the ordinary HUD does not display a production level number. Update both at the point where active `DisplayedLevel` changes. The HUD should show Level 2 identity after installation, while direct `--level2-test` remains visibly labeled development-only. Palette remains 0 for both current boards; no palette feature is needed.

## 14. B2 behavior reuse

**ARCADE VERIFIED** (qualitative, from Neal's isolated B2 comparison): below-rim Level 2 Flippers change lanes; straight-side/corner movement looked sensible; active transitions did not visibly retarget when the Blaster moved; below-rim visual crossing was not lethal. This is not validation of a natural wave or exact numeric timing.

**REFERENCE VERIFIED:** `LevelConfiguration.Level2SquareTest` already enables `Flipper.CanFlip`, square endpoint hinges, the 12-unit supply and Flipper-only composition. `Flipper.Update` consumes the installed configuration instead of checking displayed level. Promote the same configuration object and retain this behavior; do not change turn increment, pause timing, direction sampling, hinge geometry, collision behavior or optional bullet mechanics in the progression milestone.

## 15. Bonus and startbonus deferral

Defer outgoing-level bonus score, extra lives, startbonus display and Incitti's entry-position increment. The first source completion computes a zero Level 1 bonus, but its source-specific one-time guard still shifts entry position by 200; later guard behavior is not a basis for inventing a C# per-level award. C# currently has no gameplay caller for `ScoreSystem.CalculateLevelBonus`.

Explicitly describe the accepted omission as: **score is carried into Level 2, no level bonus or bonus life is awarded, and the source Begin/startbonus presentation is not implemented.** Do not imply arcade-equivalent scoring. The omission does not prevent correct configuration, accounting, charge reset or board activation.

## 16. Development scenario preservation

Keep all current command-line options, scenario conflict rules and tests:

- `--level2-test` stays direct Level 2 access and same-Level-2 replay; it never becomes a production campaign.
- `--spiker-test` stays terminal/replayable; Spike reverse/retry is unchanged and never advances.
- `--tanker-test`, `--fuseball-test` and `--pulsar-test` stay isolated and terminal/replayable.
- `--enemy-bullets-test` remains orthogonal. Normal Level 1 with this option should advance naturally and retain bullets in the validated Level 2 configuration path.
- `--lives` remains orthogonal and carries the current lives through progression.

No development enemy scenario should progress just because it starts with the Level 1 configuration identity.

## 17. Tests whose expectations must intentionally change

At the checkpoint, B2 records 391 passing tests. The following tests assert that a normal Level 1 exit is terminal, that R replays normal Level 1, or that its renderer remains on Level 1 after successful exit; update those expectations rather than weakening them:

1. [FreshReplayReplacesLevelAndPreservesScoreAndLives](../../tests/Tempest.Game.Tests/LevelConfigurationTests.cs#L99) — normal Level 1 terminal replay assumption. Replace with natural transition carry/reset assertions. Same-scenario development replay remains covered separately.
2. [ExitZoomIgnoresInputRestartAndFurtherClearTriggers](../../tests/Tempest.Game.Tests/SimulationTests.cs#L4488) — currently waits for a Level 1 `LevelComplete`; assert no early transition, then the Level 2 install at the successful travel boundary.
3. [ZoomCompletesOnlyBelowTenThenRestoresProjectionAndFreezes](../../tests/Tempest.Game.Tests/SimulationTests.cs#L4514) — crossing below depth 10 must install Level 2 rather than freeze Level 1 in `Complete`.
4. [ExitReplayRestoresProjectionDepthEggsAndCountersButKeepsScoreLives](../../tests/Tempest.Game.Tests/SimulationTests.cs#L4533) — normal Level 1 R replay is obsolete; replace with Level 1 → Level 2 fresh-state assertions and keep direct-development replay coverage.
5. [ExitRendererTracksPlayerDepthAndAllProjectedVerticesStayFinite](../../tests/Tempest.Game.Tests/SimulationTests.cs#L4616) — currently caches Level 1 geometry across the commit. Split precommit exit geometry from postcommit renderer identity/square checks.
6. [LevelCompletionFreezesAndReplayRetainsScoreAndLives](../../tests/Tempest.Game.Tests/SimulationTests.cs#L5045) — normal Level 1 terminal/replay semantics are obsolete; test natural Level 2 terminal behavior and the chosen new-campaign R policy instead.
7. [EntireEggWaveCanBeDestroyedWithSameTickExitThenCompletion](../../tests/Tempest.Game.Tests/SimulationTests.cs#L5323) — kill-clear exit should finish in Level 2 `Playing`, not Level 1 `Complete`; assert score carry and Level 2 supply.
8. [RestartAndReplayRestoreChargesWhileInactiveInputIsRejected](../../tests/Tempest.Game.Tests/SuperZapperTests.cs#L601) — its replay phase forces a normal Level 1 `Complete`; replace with fresh production-transition recharge plus direct development replay assertions.

Other `LevelComplete` tests in `SimulationTests` are explicitly Fuseball/Pulsar/Tanker/Spiker development runs and should retain terminal behavior. `Level2SquareTests` direct replay/Game Over tests, `RepeatedSpikerReplayRebindsOnlyTheCurrentSpikeCallback`, and normal Game Over restart tests remain valid. Exit-delay and early-exit-zoom phase tests that do not cross the success threshold should remain unchanged.

## 18. Deterministic automated test plan

Use the existing headless xUnit suite. Test observable state and configuration identity; do not substitute a proxy such as the displayed text for a fresh-runtime assertion.

**Progression commit**

- No-flag construction starts with Level 1.
- Kill-complete Level 1 transitions after successful exit; all-rim completion and a mixed kill/rim-clear wave do the same.
- Before depth is below 10, Level 1 remains active; wave clear, ExitDelay and intermediate ExitZoom do not advance.
- Repeated updates after commit do not install another Level or skip to another identity.
- Lethal collision ordering is preserved at clear/exit start; Spike reverse/retry remains terminal and never progresses.

**Configuration and session**

- Natural Level 2 is the same promoted square configuration selected by `--level2-test`; it has board 1, palette 0, closed 16-lane square, supply 12, canflip, and Flipper-only composition.
- Score/current lives/`--lives` override carry unchanged. No bonus or bonus life is awarded.
- Normal + `--enemy-bullets-test` remains eligible and retains the option. Other development scenarios remain ineligible.

**Fresh Level 2 state**

- Four initial Eggs, eight reserve, 12 remaining, destroyed/rim-cleared zero.
- No old Level 1 entities, bullets, shots, Spikes, Tanker/cargo entries, pending zap target, collision-order entry or callback survives.
- Two Zapper charges and cleared flash; both charges work. Death preserves remaining charges and reconstructs on Level 2.
- Hurry-Up false, exit/death counters clear, player lane/depth/configured pose correct, projection 70/5.

**Level 2 completion and UI**

- Level 2 all-rim and kill completion both work, then stop in the explicit terminal policy; no Level 3 config is synthesized.
- Natural Level 2 R starts a fresh Level 1 campaign; Game Over restart does likewise. Direct `--level2-test` still replays Level 2 and Game Over restarts that direct scenario.
- Current Level renderer identity changes before the first postcommit draw; HUD and window title show Level 2; square endpoint rendering and B1/B2 crash regression remain covered.

Run the focused changed lifecycle/configuration tests, then all 391 baseline tests plus updates, and `dotnet build TempestCSharp.slnx --no-restore`. Only obsolete terminal-Level-1 expectations should change.

## 19. Manual validation plan

Primary command (Neal's end-to-end acceptance):

```powershell
dotnet run --project src/Tempest.Game -- --lives 10
```

Neal should clear Level 1 naturally and observe automatic square Level 2 entry; verify displayed identity, score and remaining lives carry, the Super Zapper recharges, a fresh 12-unit wave begins with four Eggs, and below-rim Level 2 canflip matches the existing qualitative observation. Lose a life and confirm reconstruction remains Level 2. Complete Level 2 through kill and/or all-rim behavior; confirm the explicit no-Level-3 terminal message and the new-campaign R behavior. Note Blaster rim-movement smoothness but do not tune it. No timer measurement is required.

Development/regression runs:

```powershell
dotnet run --project src/Tempest.Game -- --level2-test --lives 10
dotnet run --project src/Tempest.Game -- --level2-test --enemy-bullets-test --lives 10
dotnet run --project src/Tempest.Game -- --spiker-test --lives 10
dotnet run --project src/Tempest.Game -- --tanker-test --lives 10
dotnet run --project src/Tempest.Game -- --tanker-test --enemy-bullets-test --lives 10
dotnet run --project src/Tempest.Game -- --fuseball-test --lives 10
dotnet run --project src/Tempest.Game -- --pulsar-test --lives 10
dotnet run --project src/Tempest.Game -- --enemy-bullets-test --lives 10
dotnet test TempestCSharp.slnx --no-restore
```

Confirm Level2 direct replay stays Level2; Spiker reverse/retry does not progress; other scenarios remain isolated; the normal bullet option does not disable progression. The separate Tanker firing activation/cadence observation remains deferred.

## 20. Architectural/refactor conclusion

The implementation can stay local to:

- promote/reuse the existing Level 2 configuration and add a one-edge production resolver;
- make fresh-wave installation accept the resolved configuration while retaining the original launch configuration for restarts;
- commit only from successful standard `ExitZoom` completion;
- keep direct-development and natural-campaign restart policy distinct;
- update HUD/window identity and focused tests.

`Level` is already a per-wave runtime, `LevelConfiguration` already owns identity/geometry/supply/Flipper behavior, `CurrentLevel` is renderer-facing, and fresh initialization already owns resource resets. No large `Campaign`, `LevelRuntime`, event bus, GUID/generation identity, accounting rewrite, or enemy redesign is justified. Add a minimal session-mode boolean only if the existing scenario flags cannot express the progression eligibility clearly; do not make `--enemy-bullets-test` a campaign selector.

## 21. Behavior-spec impact after implementation and manual validation

Do not edit the spec during analysis. After code acceptance **and Neal's manual validation**, update only the directly affected claims:

- Section 2 global behavior: normal no-flag Level 1 → Level 2 progression, exit commit, score/lives carry, no bonus, Level 2 terminal policy.
- Section 2 completion/exit and Super Zapper: transition point and two-charge fresh-level reset, labeled **PROJECT DECISION** or **REFERENCE VERIFIED** as appropriate.
- Section 3 Level 2: clarify B2 behavior is reused by natural Level 2; preserve exact evidence scope.
- Section 10 development-only scenarios: `--level2-test` remains direct-access/replay, while production Level 2 is naturally reachable.
- Section 11 uncertainties and Section 12 history: record that natural transition/setup was manually tested, without upgrading source-derived facts into arcade claims.

Update README/development notes only as directly needed after implementation; do not rewrite historical reports as if the isolated B2 stage had not existed.

## 22. Explicit out-of-scope items

- Level 3 or any broader campaign framework, board rotation/set cycles, open tubes, palette systems or source `Begin`/`Ready` presentation.
- Bonus score, `startbonus`, extra lives, audio, high-score changes or source-equivalent economy.
- New Level 2 Flipper tuning, collision rules, geometry, bullet rules or a fork from the validated B2 path.
- Production Tanker/Spiker/Fuseball/Pulsar eligibility, mixed waves, Tanker firing cadence changes, or generalized enemy accounting.
- Blaster movement tuning, including the deferred perceived rim-movement delay/smoothness concern.
- Changing any Spiker reverse/retry behavior, flag conflict rules, or existing development scenario's isolated terminal replay.
- Claiming source-derived setup and timing values are arcade-verified.

## 23. Remaining arcade uncertainties and deferred observations

The new transition has not been compared end-to-end with Atari. In particular, Neal has not validated natural Level 1 → Level 2 presentation, exact start pose, score/lives carry, charge recharge timing, flash behavior, Level 2 accounting, or what follows Level 2 completion. Incitti's current-level increment, bonus guard, clocks, setup values and source Begin/Ready sequence remain **REFERENCE VERIFIED** only.

Existing Neal observations still apply only to their documented scopes: broad all-rim completion and qualitative isolated B2 canflip. Exact Flipper timings, random behavior, collision/update boundaries and setup internals remain unmeasured. Keep these deferred observations visible: **Tanker firing activation/cadence fidelity** and the **slight perceived Blaster rim-movement delay/smoothness concern**. Neither belongs in this milestone.

## 24. GO / NO-GO implementation recommendation

**GO.** Stage A provides the config/runtime and fresh-vs-reconstruction boundary; B1/B2 provide the validated square and canflip behavior. A once-only commit at the post-travel depth threshold, using one promoted configuration and the existing fresh-wave cleanup, is sufficient. Preserve terminal development scenarios, explicitly stop natural Level 2 without fabricating Level 3, and keep the bonus/presentation omissions labeled. The only required gate after implementation is focused/full automated validation followed by Neal's manual end-to-end progression check before updating behavior claims.
