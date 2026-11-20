# All-Rim Completion Implementation Report

Starting checkpoint: `7f6c776` (`Implement Super Zapper and correct Tanker cargo behavior`). No commit was created.

## 1. Implemented arcade behavior and evidence boundary

Implemented Neal's **ARCADE VERIFIED** broad observation: after reserve/incoming Eggs are exhausted, all remaining rim enemies may disappear without individual kills and the Blaster begins the existing tube exit. The exact height threshold, state-agnostic list, ordering, hidden accounting, zap outcome, and spike-retry behavior remain **REFERENCE VERIFIED / PROJECT DECISION**, not arcade-verified claims.

The inspected Incitti source tests post-update enemy heights against 400 and selects the exit when Egg sources are empty. It does not immediately remove the enemy objects when exit is selected: they continue updating through the pre-zoom state and become hidden during zoom. This implementation deliberately follows the approved design instead, removing and non-kill-accounting them when the C# all-rim check commits exit. Pending zap targets therefore cannot score during a later exit delay.

## 2. Exact implemented qualification

One predicate runs only while `Playing`. It requires:

- `CurrentLevel.ReserveEggs == 0`, `Eggs.Count == 0`, and `PendingTankerCargoEggs == 0`;
- for Tanker mode, no latent or queued cargo and every unresolved original/cargo ID represented by an active enemy;
- at least one active enemy; and
- post-update `Height >= Level.Depth` for every active Flipper, Tanker, Spiker, Fuseball, and Pulsar.

Player shots, EnemyBullets, and Spike hazards are excluded. Normal waves also fail closed unless semantic remaining units equal the active enemy count after Egg sources are exhausted. The `>=` cutoff is the implementation decision based on the source's 400 boundary, not a measured arcade threshold.

## 3. Evaluation ordering

The early kill-all check after player shots and enemy bullets is unchanged. The final active-tick path remains Egg release/update/hatching, enemy and zap-timer updates, pending cargo activation, and existing player/enemy/bullet/spike collisions. If still `Playing`, all-rim resolution runs before Hurry-Up latching; if it does not qualify, the existing Hurry-Up latch and final kill-all check run. Thus an Egg hatch blocks exit, a final enemy can qualify on its rim-arrival update, and the early shot-kill route is preserved.

## 4. Normal non-kill accounting

Added `EnemiesRimCleared`. `EnemiesDestroyed` remains kill-only; `RemainingEnemies` is `EnemyTotal - EnemiesDestroyed - EnemiesRimCleared`. `IsComplete` accepts the two resolved dispositions together, and death/reconstruction rewinds `EnemiesSpawned` and its lane index to their sum so cleared units do not reappear. The standard HUD now uses `RemainingEnemies`.

## 5. Tanker ledger changes

Added independent rim-clear ID sets for original slots and materialized cargo. Original destroyed, escaped, rim-cleared, and unresolved outcomes remain separate; cargo killed/resolved, rim-cleared, and unresolved outcomes remain separate. `TankerCargoUnitsKilled` preserves a score-bearing count; aggregate resolved and accounting totals include rim-cleared IDs. Latent and queued cargo block qualification, and identity-set equality rejects unresolved units missing from all explicit representations. Reset/replay clears both rim-clear sets.

## 6. Per-enemy treatment

- **Flipper:** qualifying at any state at/above the rim; existing player collision runs first.
- **Tanker:** latent cargo blocks with its carrier; normal release remains escaped, and live cargo qualifies independently.
- **Spiker:** its natural below-threshold behavior remains unchanged; a qualifying forced rim Spiker is non-kill-cleared while its Spike remains.
- **Fuseball/Pulsar:** qualification uses height only; existing lateral, pulse, and player-collision behavior is unchanged.
- **EnemyBullet:** neither blocks nor gets rim-accounted; exit cleanup removes it.
- **Spike:** neither blocks nor gets rim-accounted; it remains a hazard through Spiker exit/retry.

## 7. Super Zapper interaction

Zap expiry in the enemy-update pass still scores normally before all-rim evaluation. A target still pending at commit is removed with its enemy object, so it cannot later award kill score or a Fuseball popup. The all-rim path does not reset the shared flash; display-step advancement continues through exit.

## 8. Hurry-Up interaction

Below-rim Fuseballs/Pulsars still trigger the existing Hurry-Up latch. All-rim completion wins before a new latch when the final survivor reaches the rim; an already latched value is preserved but does not veto exit.

## 9. Collision precedence

Existing player/enemy, bullet, and spike collision checks precede all-rim resolution. A collision that enters `PlayerDying` prevents rim commitment that tick. The existing player-shot-before-enemy-collision rule remains unchanged. Exact arcade simultaneity, especially with player movement, remains unverified.

## 10. Spikes, exit, and retry

The existing exit-start path is retained: normal scenarios enter `ExitDelay`; the Spiker development scenario enters `AvoidSpikes`. Enemy objects are removed, while Spike objects and their collision-order entries remain. Reverse/retry preserves the rim-clear count and resolved spawn position, so it does not reconstruct rim-cleared units.

## 11. Scoring and HUD

Rim clearing awards no points, creates no Fuseball kill popup, and does not increment a destroyed count. Previously earned score remains. The normal remaining-enemy HUD reaches zero through semantic accounting; Tanker ORIGINAL/CARGO counts use the updated outcome ledger.

## 12. Files changed

- `src/Tempest.Game/Levels/Level.cs`
- `src/Tempest.Game/TempestGame.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `tests/Tempest.Game.Tests/SimulationTests.cs`
- `tests/Tempest.Game.Tests/SuperZapperTests.cs`
- `README.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `AI-Logs/Prompts/2026-10-30-all-rim-completion-implementation-prompt.md` (exact supplied prompt)
- This report

`TEMPEST-BEHAVIOR-SPEC.md` was not changed.

## 13. Tests changed or added

Added coverage for multiple non-kill Flipper clears and idempotence, reserve blocking, post-update Fuseball/Pulsar completion, prior Hurry-Up latch precedence, Tanker mixed killed/rim-cleared cargo accounting, pending zap cancellation and continued flash, and a forced rim Spiker with persistent Spike reverse/retry. Updated accounting helpers to include rim clears. Reframed the final-pivot kill tests to drive the entity directly to the target tick, preserving explicit kill/collision regression routes without triggering all-rim exit earlier.

## 14. Obsolete expectations intentionally replaced

Updated and renamed the expectations in:

- `HurryUpDoesNotActivateWhenAllActiveEnemiesAreAtTheRim`
- `HurryUpTriggerUsesPostUpdateHeightNotThePreUpdateBelowRimHeight`
- `HurryUpRemainsLatchedAtTheRimWithoutResolvingTheLastEnemy`

They now expect non-kill rim resolution and exit. Existing below-rim and kill-all assertions remain in place.

## 15. Focused, full, build, and diff validation

- All-rim/Hurry-Up focused filter: **41 passed, 0 failed, 0 skipped**.
- Tanker/accounting focused filter: **57 passed, 0 failed, 0 skipped**.
- Super Zapper focused filter: **31 passed, 0 failed, 0 skipped**.
- Spiker/exit focused filter: **50 passed, 0 failed, 0 skipped**.
- Full solution tests: **330 passed, 0 failed, 0 skipped**.
- Solution build: **0 warnings, 0 errors**.
- `git diff --check`: **passed; no whitespace errors**. Git emitted only its line-ending normalization warnings for modified files.

Intermediate test-fixture failures were corrected and rerun: one multi-Flipper fixture initially left the player in a Flipper's lane; the forced-Spiker fixture initially hatched with too few active Eggs to select a Spiker; and an existing final-pivot test advanced through ordinary ticks, correctly entering all-rim exit before reaching its intended collision assertion. Fixtures were adjusted to isolate their intended conditions. The final full suite and focused Spiker/exit group passed.

## 16. Manual validation checklist

Manual graphical validation remains pending. Run:

```powershell
dotnet run --project src\Tempest.Game -- --lives 10
dotnet run --project src\Tempest.Game -- --fuseball-test --lives 10
dotnet run --project src\Tempest.Game -- --pulsar-test --lives 10
dotnet run --project src\Tempest.Game -- --tanker-test --lives 10
dotnet run --project src\Tempest.Game -- --spiker-test --lives 10
```

Check:

1. Normal Level 1: after Eggs are exhausted, observe rim enemies disappear and the existing exit begin.
2. Verify disappearing enemies do not award kill score.
3. Keep a survivor below the rim and confirm exit waits.
4. In Fuseball/Pulsar scenarios, observe below-rim Hurry-Up followed by rim completion.
5. In Tanker mode, observe cargo below the rim blocking and live cargo at the rim clearing.
6. Where practical, activate Super Zapper near rim completion and verify no later score for a cleared pending target.
7. In Spiker mode, verify Spike hazards remain during exit and reverse/retry does not restore cleared enemies.
8. Observe same-tick player collision versus rim completion where practical; do not treat the current precedence as arcade-verified.

## 17. Explicitly out of scope

No behavior-spec edit, manual arcade validation, progression/next-level work, enemy tuning, score redesign, enemy-state redesign, or change to the established shot/collision order was included. No commit was created.

## 18. Remaining arcade uncertainties

The exact numeric rim threshold and qualifying enemy states; precise same-tick movement/collision precedence; pending Super Zapper/stillborn scoring during arcade pre-zoom; and whether a Spike reversal restores rim-cleared enemies remain unobserved. The C# implementation uses 400/state-agnostic qualification, post-collision resolution, immediate pending-zap cancellation, and permanent rim-clear accounting across retry as explicit recreation policies.
