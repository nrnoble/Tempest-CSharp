# All-Rim Completion Analysis

## Scope and evidence

Analysis only, against checkpoint `7f6c776` (`Implement Super Zapper and correct Tanker cargo behavior`). The checkout was clean at inspection. No gameplay code, tests, tracked documentation, project/configuration files, or Git state were changed. The supplied prompt was copied verbatim to the requested ignored prompt path.

Evidence labels follow [TEMPEST-BEHAVIOR-SPEC.md](../../TEMPEST-BEHAVIOR-SPEC.md): Neal's direct arcade observation is **ARCADE VERIFIED**; literal Incitti code is **REFERENCE VERIFIED** only; current behavior and proposed choices are **PROJECT DECISION** unless identified otherwise. The Tanker, Spiker, Fuseball, and Pulsar C# scenarios remain **DEVELOPMENT ONLY**.

## 1. Literal Incitti behavior

### Qualification and update order

`Baddies.UpdateBaddies` initializes `onrim = True`, updates each listed baddie, then sets `onrim = False` when `baddietype > BAD_SPIKE` and the post-update `height < 400` ([`tempest.bmx`](../../Reference/incitti/tempest.bmx#L116), [`UpdateBaddies`](../../Reference/incitti/tempest.bmx#L2183)). The constants are `BAD_SPIKE = 1`, `BAD_BULLET = -1`, and enemy types 2 or greater. Thus the literal test is height-only, includes enemy states regardless of animation/lane, and excludes actual spikes and bullets.

At the end of the main loop, after Egg and baddie updates/player checks, Incitti tests `eggsleft = 0` and `EGG_LIST.Count() = 0`. If so, `enemiesleft = 0 Or onrim = True` selects the exit path; otherwise it sets `rimit = True` ([main loop](../../Reference/incitti/tempest.bmx#L3749), [completion branch](../../Reference/incitti/tempest.bmx#L3811)). An Egg that is ready to hatch still blocks until it leaves the Egg list. The `rimit` latch then affects Fuseball lateral-boundary/climb choices and Pulsar's top-end choice; it is not itself an exit or a resolution ([Pulsar](../../Reference/incitti/tempest.bmx#L3030), [Fuseball](../../Reference/incitti/tempest.bmx#L3333), [level reset](../../Reference/incitti/tempest.bmx#L4253)).

### Per-type source result

| Listed source object | Effect on `onrim` |
|---|---|
| Flipper, climbing/pausing/flipping | Counts as an enemy. It qualifies at updated height `>= 400`, irrespective of state. |
| Pulsar, climbing/descending/pausing/flipping/pulsing | Counts as an enemy. Height alone matters; a height of 400 qualifies even if it is about to descend or its lane is pulsing. |
| Fuseball, lateral/climbing/descending, any `w` position | Counts as an enemy. Height alone matters; lateral position and action do not disqualify it. |
| Tanker | Counts while it remains listed. Its normal update releases cargo and removes it after passing `depth - 15` (385), so a normal Tanker does not reach 400. |
| Spinner/Spiker enemy | Counts as an enemy; it is distinct from the `BAD_SPIKE` hazard. Its current height is tested without regard to build/retreat state. |
| Actual spike hazard | Excluded from the enemy-height test; it remains a separate exit hazard. |
| Enemy bullet | Excluded (`BAD_BULLET = -1`); it does not block all-rim exit. |
| Reserve/active Eggs | Excluded from `onrim`, but independently gate the exit until both sources are empty. |

Tanker cargo is not tested while latent: the active Tanker blocks while below the rim; after release, each cargo Flipper is listed at the carrier height and participates. Source natural release occurs after the Tanker crosses 385; shot destruction releases cargo at its current height; Super-Zapper expiry releases stillborn children with an eight-tick timer ([Tanker cargo/update](../../Reference/incitti/tempest.bmx#L2450)).

### Exit, cleanup, score, and death

The condition does **not** immediately delete all remaining enemies. The level enters `LEVEL_AVOID_SPIKES` on selected boards or `LEVEL_START_ZOOM` otherwise; baddies continue normal updates during those pre-zoom states. Once `LEVEL_ZOOMING`/`LEVEL_REVERSE_ZOOM` starts, `UpdateBaddies` advances only spikes and `DrawBaddies` draws only spikes. Hidden enemies remain in `BADDIE_LIST` until successful level completion calls `ClearLevel`, which clears lists/counters without awarding kill points ([state transitions](../../Reference/incitti/tempest.bmx#L1344), [baddie update](../../Reference/incitti/tempest.bmx#L2183), [`ClearLevel`](../../Reference/incitti/tempest.bmx#L4137)).

Consequently, a pending zap target below 400 blocks exit, while one at/above 400 does not. Its timer can still expire and score during Avoid-Spikes/Start-Zoom, before zoom freezes baddies; a timer still pending at zoom is frozen and later removed by `ClearLevel` without score. Stillborn cargo follows the same timing. The precise timer outcome depends on board-specific pre-zoom duration. This is literal source behavior, not established arcade behavior.

Player collision checks occur in `UpdateBaddies` before the end-of-loop completion decision. A collision therefore precedes exit selection in the source. During zoom/reverse zoom, only spikes are player collision hazards. `ConvertBaddieToEggs` on player death removes enemy baddies, restores eligible survivors to Egg supply, separately removes bullets, and retains spikes; `ClearLevel` later removes all baddies. `enemiesleft` is an active-count mechanism, not a conserved kill ledger: normal enemy kills decrement it and score in their own update; Tanker release removes one carrier and creates cargo entries; clear/death paths reset or reconstruct counts. Neither `onrim` nor the exit decision awards score for an enemy that is merely hidden/cleared.

## 2. Neal's arcade observation and evidence boundary

**ARCADE VERIFIED:** after reserve/incoming Eggs are exhausted, if every remaining enemy reaches the top rim, the remaining enemies disappear and the Blaster proceeds into tube exit; the player need not kill each rim enemy. The current [behavior spec §2, “Completion and tube exit”](../../TEMPEST-BEHAVIOR-SPEC.md#completion-and-tube-exit) records this observation.

Neal has not observed the numeric cutoff, per-enemy/state qualification, update ordering, accounting/score internals, pending-zap interactions, or precise collision timing. In particular, do not upgrade Incitti's `< 400` comparison or its pre-zoom timer behavior to arcade facts.

## 3. Current C# behavior and where “resolved” means “killed”

### Normal Level accounting

[`Level`](../../src/Tempest.Game/Levels/Level.cs#L7) stores `EnemiesDestroyed`; `ReserveEggs` is `EnemyTotal - EnemiesSpawned`, and `RemainingEnemies` is currently `EnemyTotal - EnemiesDestroyed` ([lines 33–41](../../src/Tempest.Game/Levels/Level.cs#L33)). Hatching is a transfer from Egg to active enemy and does not change the total. Ordinary kills and zap expiries call `RecordEnemyDestroyed`; Spiker retreat instead returns a unit to reserve. `ReturnSurvivorsToReserve` rewinds `EnemiesSpawned` to `EnemiesDestroyed` for life reconstruction ([lines 98–110](../../src/Tempest.Game/Levels/Level.cs#L98)). `IsComplete` requires all ten destroyed, no reserve or active Eggs, and no active enemies ([line 234](../../src/Tempest.Game/Levels/Level.cs#L234)).

### TempestGame completion and tick order

`RunOneStep` advances display flash even during exit/death; on an active tick it updates player input, zap activation, shots, bullets, and performs an early kill-all completion check. It then releases/updates Eggs, hatches, advances active enemy/zap timers (including Tanker cargo), checks enemy/player and spike collisions, latches Hurry-Up, and performs the final completion check ([`TempestGame`](../../src/Tempest.Game/TempestGame.cs#L128), especially lines 174–207). The final check still requires kill resolution; it enters `AvoidSpikes` for the Spiker scenario or `ExitDelay` otherwise, clears shots/bullets, and retains the existing exit state machine ([lines 882–898](../../src/Tempest.Game/TempestGame.cs#L882)).

Resolved-as-killed paths include the Flipper shot/zap handlers, Spiker/Pulsar/Fuseball zap handlers, and normal `Level.RecordEnemyDestroyed` calls. Fuseball scoring also creates a kill-score popup. Natural Spiker retreat is not a resolution: it returns a pending unit to reserve. No existing normal-level non-kill exit disposition exists.

### Tanker ledger

Tanker mode stores original slots in `unresolvedOriginalSlots`, `destroyedOriginalSlots`, and `escapedOriginalSlots`; natural carrier release marks the original slot escaped, while a shot/zap marks it destroyed. Cargo has distinct integer IDs: `unresolvedCargoUnits` tracks materialized children and `resolvedCargoUnits` currently represents shot/zap-resolved children. A separate latent count lives on the active Tanker; restored original/cargo IDs travel in Eggs. The ledger/property surface is in [`TempestGame.cs`](../../src/Tempest.Game/TempestGame.cs#L40) and its reset/re-entry logic at lines 225–285, 739–782. `TankerAccountingTotal` includes resolved/unresolved original slots and materialized cargo; latent cargo is intentionally represented separately. `CheckForLevelComplete` in Tanker mode requires all original and materialized cargo units resolved plus empty Egg/enemy state. A predicate-only relaxation would orphan these IDs.

### Other gates

The complete current active enemy lists are Flippers, Tankers, Spikers, Fuseballs, and Pulsars. The `developmentCollisionOrder` list also contains bullets and spikes and is not a suitable source for rim qualification. `CurrentLevel.Eggs` includes roaming, approaching, and ready-to-hatch Eggs; `ReserveEggs` is independent. Tanker mode additionally has `PendingTankerCargoEggs`, unresolved cargo not yet materialized, and `TankerLatentCargoUnits`. `shotReleasedCargoPendingActivation` is activated before the final post-update check.

Enemy bullets are not counted in `Level.IsComplete` and are cleared at exit. Spikes are level hazards, not enemies; current Spike exit preserves them and the reverse/retry path calls `ReturnSurvivorsToReserve` ([`CheckSpikesAgainstPlayer`](../../src/Tempest.Game/TempestGame.cs#L966), [`UpdateReverseZoom`](../../src/Tempest.Game/TempestGame.cs#L987)). The standard HUD currently computes remaining as `EnemyTotal - EnemiesDestroyed`, while `Level.RemainingEnemies` has the same kill-only semantics ([`VectorRenderer.DrawHud`](../../src/Tempest.Game/Rendering/VectorRenderer.cs#L579)).

## 4. Recommended qualification

**PROJECT DECISION:** use one explicit post-update predicate over the active enemy lists:

1. `State == LevelState.Playing`.
2. `ReserveEggs == 0`, `Eggs.Count == 0`, and `PendingTankerCargoEggs == 0`.
3. No Tanker latent cargo and no unresolved materialized cargo unit that is not represented by an active cargo Flipper (the existing `PendingTankerCargoUnits`/identity state is available as a defensive invariant).
4. At least one active enemy exists; do not let an empty `All()` over no enemies convert missing/unresolved accounting into a rim clear.
5. Every active enemy in Flippers, Tankers, Spikers, Fuseballs, and Pulsars has post-update `Height >= Level.Depth` (400).

The numeric threshold is **REFERENCE VERIFIED**, not arcade-verified. Use `>=`, not equality, to preserve source meaning for above-rim heights. Use no state/lane/pulse exclusions: all listed enemy states qualify by height alone. Do not include shots, bullets, or spikes. Preserve any nonempty Egg source or latent cargo as a blocker independently of the enemy height check.

For Tanker completeness, also assert that every unresolved original/cargo identity is either represented by the active enemy set or an explicitly gated reserve/Egg/latent source. This defensive accounting check should fail closed rather than silently clear an unrepresented unit.

## 5. Evaluation and collision ordering

Keep the existing early kill-all check after shot/bullet updates: it preserves the validated same-tick last-shot behavior. Do **not** run all-rim qualification there; it would use pre-Egg/pre-enemy-update heights and precede player collision checks.

On the existing final path, after Egg release/update/hatching, all enemy and zap-timer updates, shot-released cargo activation, and every enemy/bullet/spike player-collision check:

1. If still `Playing`, test and commit all-rim completion.
2. If it did not qualify, call `LatchHurryUp` with its existing post-update behavior.
3. Run the existing final kill-all check.

This makes the final Flipper/Fuseball/Pulsar reaching 400 qualify on its actual post-update height. A ready Egg hatches before this check; its new enemy at or near height zero blocks exit. If a collision calls `KillPlayer` first, `State` is `PlayerDying` and all-rim completion must not run. Incitti likewise checks player collision before its completion branch. Keep current C# shot-before-enemy-collision ordering.

**Manual-validation uncertainty:** exact arcade simultaneity is not observed. The conservative policy is player death first for an enemy/bullet collision that is registered in the same tick, then exit only if the player remains alive. C# updates player input before collision; Incitti's baddie collision check precedes `MainPlayer.Update` in its loop, so movement onto a lane in that exact frame is not proven equivalent. Validate direct Flipper rim arrival, pivot completion, Fuseball lateral rim contact, Pulsar rim/pulse collision, and Blaster movement on the same frame.

## 6. Non-kill resolution and accounting

Do not call `RecordEnemyDestroyed`, `ResolveOriginalSlotDestroyed`, or `ResolveCargoUnit` for a rim clear. Do not award points or create Fuseball score popups.

For standard Level/Spiker/Fuseball/Pulsar scenarios, add an explicit count such as `EnemiesRimCleared`. Then:

- Keep `EnemiesDestroyed` strictly kill-scored.
- Define remaining/unresolved as `EnemyTotal - EnemiesDestroyed - EnemiesRimCleared`.
- Preserve the conservation invariant: `EnemyTotal = ReserveEggs + Eggs.Count + activeEnemies + EnemiesDestroyed + EnemiesRimCleared` (with the existing Spiker return-to-reserve behavior).
- On death reconstruction, rewind `EnemiesSpawned` and the deterministic lane index to `EnemiesDestroyed + EnemiesRimCleared`, not just `EnemiesDestroyed`.
- Make `IsComplete` accept destroyed plus rim-cleared dispositions, while still requiring no reserve/Eggs/active enemies.

In Tanker mode, add separate rim-clear ID sets for original slots and cargo. Keep original outcomes partitioned as destroyed, naturally escaped, rim-cleared, unresolved; keep materialized cargo partitioned as killed/resolved, rim-cleared, unresolved. Expose a distinct rim-cleared count/set and keep killed counts/score separate. Broaden aggregate `Resolved` properties only if their semantics are documented and the outcome-specific destroyed property remains available. Include rim-cleared IDs in `TankerAccountingTotal` and `TankerMaterializedCargoUnits`; include them in the overall resolved count, but never in destroyed count. Keep latent cargo accounted via the carrier until it materializes. IDs are resolved exactly once.

This is a local extension, not a full accounting redesign. One idempotent resolution path should accept active enemies, remove each from its owning list and `developmentCollisionOrder`, classify its Tanker identity, record the normal-wave count where applicable, and leave score unchanged. Clearing actual enemy objects also cancels their pending zap timers.

## 7. Per-enemy recommendation

- **Flipper:** climbing arrival, rim pause, and rim flipping all qualify at updated height 400. A just-arrived Flipper in the player's lane reports a lethal collision first.
- **Tanker:** current Tanker update releases/removes itself after crossing 385, so a live Tanker below 400 blocks. Its latent cargo remains represented by the carrier and also blocks. An unresolved carrier must never be dropped merely because Egg lists are empty.
- **Spiker:** current C# build condition stops at `Depth - 4` (396), then retreats and returns the unit to reserve. It cannot naturally meet a 400 threshold, so a live Spiker remains a blocker until killed/retreated/re-entered; do not silently lower the rim cutoff to 396. If a test forcibly places a Spiker at 400, the height-only rule qualifies it. Its persistent Spike remains in the level.
- **Fuseball:** any state and any lateral `W`/lane position qualifies if its post-update height is 400. A descent to 399 blocks; a lateral-at-rim Fuseball qualifies regardless of `W`, although its player collision remains limited to the existing interior lateral window.
- **Pulsar:** any state at 400 qualifies, independent of lane pause, flip, descent-resume state, and global pulse phase. Pulsing remains a collision hazard and is checked before completion.
- **Enemy bullet:** does not qualify or block. Clear it at exit start as the existing exit path does; a bullet that collides with the player in the same tick still causes death first.
- **Spike:** does not qualify or block and must remain for the existing Spiker Avoid-Spikes/reverse route.
- **Super Zapper pending target:** still an active enemy for the height test. A pending target below 400 blocks. If its timer expires in the update before the final test, its ordinary kill score/resolution wins; if still pending when all-rim resolution commits, it is removed with no later zap score.

## 8. Tanker/cargo scenarios

| Case | Expected all-rim result |
|---|---|
| A. Unresolved Tanker before cargo release | Carrier is below 400 and latent cargo is nonzero: block. Preserve original slot and latent cargo. |
| B. Natural Tanker release + two live children | Original slot remains naturally escaped. Either child below 400 blocks; both at 400 allow exit and rim-clear their two cargo IDs. |
| C. Shot-destroyed Tanker + live children | Keep the original slot destroyed and its 200 points. Children below 400 block; children at rim are cargo-rim-cleared for no additional score. |
| D. Zap-destroyed Tanker + stillborn children | Keep the carrier's 200 zap score. Children below rim block and timers continue. A timer that expires before qualification resolves/scorers normally; pending children at rim are rim-cleared without their delayed 150-point scores. |
| E. One child already killed/resolved | Preserve that outcome and score; the remaining ID alone blocks until it is killed or rim-cleared. |
| F. Death/re-entry before qualification | Existing reconstruction carries only unresolved original/cargo IDs through fresh Eggs and drops runtime timers. Do not restore destroyed, escaped, or rim-cleared IDs; do not duplicate IDs. Death before all-rim commitment therefore remains an ordinary reconstruction. |
| G. Cargo at rim vs below | Each child independently participates by height. A single below-rim child blocks even if its sibling is at the rim. |

`TankerLatentCargoUnits` is currently derived from active carrier cargo or the unresolved slot-0 fallback; `PendingTankerCargoEggs` tracks queued cargo; `PendingTankerCargoUnits` distinguishes unresolved materialized IDs not currently active. Include all these representations in the gate. The ledger should account for all 10 original slots and each materialized cargo ID after clear, including the mixed destroyed/escaped/rim-cleared case. `ResetTankerLedger` must clear both new rim-clear sets on replay/full reset.

## 9. Super Zapper precedence and flash

Use the post-enemy-update check so zap expiries that happen on this tick settle first. If an all-target zap was used shortly before the last enemy reaches the rim, every target whose delay expires before the qualification check keeps its ordinary score; every still-pending rim target is non-kill-cleared once and cannot score later. A single zap on the final pending rim survivor follows the same rule. A pending below-rim target or stillborn child blocks until it reaches the rim or its timer resolves.

Commit rim-clear immediately when the all-rim condition starts the exit, including when the Spiker mode enters `AvoidSpikes`; do not leave pending enemies alive through the exit timer. This is the deterministic policy that satisfies the stated requirement that a rim-cleared pending target cannot award later zap score. It intentionally differs from Incitti's pre-zoom update window, where a timer may still score before actual zoom. Arcade timing for this interaction is unknown.

Do not reset `SuperZapper.FlashTicksRemaining`. The existing display clock advances unconditionally at the start of every `RunOneStep`, including ExitDelay/AvoidSpikes/ExitZoom; therefore an active flash continues and expires under the existing 60-display-step policy. This is consistent with the source's global flash countdown (`superzapperdisplay` decrements in the main loop) and changes no presentation policy.

## 10. Hurry-Up interaction

When Eggs/sources are exhausted and any active Fuseball/Pulsar remains below 400, the all-rim predicate is false and the existing Hurry-Up latch can run unchanged. Its movement effects remain visible starting the next active update; no score/accounting changes occur.

When the final below-rim survivor reaches 400 in its update, the all-rim check runs before `LatchHurryUp`, commits the exit, and does not newly latch Hurry-Up. If Hurry-Up latched on an earlier below-rim tick, preserve the latch value but let all-rim completion win; it is not a veto. Existing Fuseball/Pulsar hurry-up timing below the rim remains unchanged.

## 11. Player collision precedence

The recommended precedence is:

1. Update enemies and zap timers.
2. Resolve player/enemy, bullet, and spike collisions using the existing C# order.
3. If any collision entered `PlayerDying`, skip all-rim resolution.
4. Otherwise qualify/commit all-rim exit.

This agrees with Incitti's post-update baddie collision pass before the completion decision and with the current C# final-check placement. A same-tick shot that kills a Flipper before the collision pass continues to save the player. Simultaneous movement/collision behavior needs manual validation because C# and Incitti update player motion at different points in their loop.

## 12. Spikes, exit, and reverse/retry

Use the existing state machine: normal exit begins at `ExitDelay`; Spiker development begins at `AvoidSpikes`, then the established 151-update avoidance interval and 21-update delay. Keep spikes and their collision-order entries, clear shots/enemy bullets, and remove/rim-resolve only enemy objects.

Make rim-clear resolution permanent when exit begins. If a persistent spike reverses the Spiker exit, its hit still costs a life and restores projection/player depth under current rules, but it must not reconstruct already rim-cleared enemy units. `UpdateReverseZoom` checks `CurrentLevel.IsComplete(0)` before deciding whether to retry Avoid-Spikes or rebuild after an ordinary death; the non-kill count must make that predicate remain complete. `ReturnSurvivorsToReserve` must preserve the rim-cleared count when it rewinds spawn accounting. The retry repeats the spike exit sequence with no rim-cleared enemies restored. This is a conservative **PROJECT DECISION**; Incitti's reversible pre-zoom window reconstructs enemies as Eggs, but the arcade interaction has not been observed.

## 13. Scoring and HUD

No rim-clear path calls kill-score handlers or `Score.Add`; there are no enemy kill points, Fuseball popups, or destroyed-count increments. Already earned kills/zap expiries keep their existing scores. `Level.RemainingEnemies` and the normal HUD should subtract both destroyed and rim-cleared units, so the count displays zero after the exit decision without redefining `EnemiesDestroyed`. Tanker `ORIGINAL`/`CARGO` remaining counters should reach zero from the outcome sets; expose outcome-specific clear counts for tests/debugging without redesigning the HUD.

## 14. Deterministic test plan

### Standard Level 1 and common qualification

- Reserve Eggs remain with every active enemy at rim: no all-rim exit.
- Reserve is zero but any roaming, approaching, or ready-to-hatch Egg remains: no exit.
- All Egg sources exhausted and a Flipper is below rim: no exit; at exactly 400 it exits after the update. Cover climbing arrival, Pausing, and Flipping at rim, including above-400 comparison.
- Multiple rim Flippers all disappear at one commit; score and `EnemiesDestroyed` do not change; rim-cleared count increases once; repeat checks cannot double-resolve.
- Ordinary kill-all completion remains same-tick, score-accurate, and reports zero rim-cleared.
- Empty enemy list with inconsistent unresolved accounting cannot trigger rim clear.

### Hurry-Up, enemy-specific states, bullets, and spikes

- Exhausted Eggs + below-rim Fuseball/Pulsar latches Hurry-Up and does not exit; preserve existing trigger tick movement/cadence.
- Last survivor reaches rim that tick: all-rim exit precedes new Hurry-Up latch. A previously latched flag does not prevent exit.
- Cover Fuseball lateral W endpoints/interior, climb/descent at the rim, and Pulsar climbing, pause, flip, descending and pulse-phase states. Qualification depends only on post-update height.
- Spiker at natural maximum 396 blocks and then retreats/returns to reserve; a forced 400 Spiker qualifies. Persistent spikes remain and enter spike avoidance.
- Active enemy bullets do not block and are cleared at exit; a same-tick bullet hit still wins by causing player death.

### Tanker

- Unresolved carrier/latent cargo blocks; queued original and cargo Eggs block.
- Natural release records the original slot escaped; shot/zap release preserves destroyed original slot and score.
- Materialized child below rim blocks; both at rim resolve IDs as rim-cleared; mixed destroyed/escaped/rim-cleared original/cargo ledger conserves IDs.
- One killed cargo child plus one rim-cleared child yields exactly one kill score and no score for the rim clear.
- Zap-destroyed Tanker stillborn children below rim block; all at rim clear non-kill if their timers remain pending; expired timers resolve/score exactly once before rim check.
- Death before qualification reconstructs only unresolved identities; replay/full reset empties rim-clear sets and restores initial identity supply cleanly.

### Super Zapper, death boundary, and exit/retry

- Pending rim target not yet expired is non-kill-cleared with no later timer score; pending below-rim target blocks.
- Last zap expiry on the qualification tick scores normally before the exit; no duplicate resolution/popups.
- Flash continues through exit and expires at the existing display-step count.
- Player death immediately before qualification enters `PlayerDying`, does not commit rim-clear, and reconstructs unresolved units after the existing death interval.
- Spike collision after committed all-rim exit preserves rim-clear accounting and does not recreate Eggs/enemies on retry; persistent spikes still collide.
- Every enemy type cleared solely through all-rim yields zero additional kill points.

### Regression

Keep existing kill-route, Egg, scoring, death, Super Zapper, and exit tests meaningful. Update the shared Level/Tanker conservation helpers to include the new rim-clear dispositions, and explicitly assert that kill totals remain distinct. Run the focused all-rim/Hurry-Up/Tanker/Super-Zapper/Spike tests and then the full checkpoint suite (the prompt identifies 325 tests); do not change unrelated expected values merely to make tests pass.

## 15. Existing tests whose expectations intentionally change

These current tests assert that a final, all-rim survivor remains in `Playing` instead of starting exit; update their purpose/assertions to cover the observed all-rim behavior:

1. `HurryUpDoesNotActivateWhenAllActiveEnemiesAreAtTheRim` ([`SimulationTests.cs:67`](../../tests/Tempest.Game.Tests/SimulationTests.cs#L67), both Fuseball/Pulsar theory cases): now expect ExitDelay, no new Hurry-Up latch, one non-kill resolution, and unchanged score/destroyed count.
2. `HurryUpTriggerUsesPostUpdateHeightNotThePreUpdateBelowRimHeight` ([line 170](../../tests/Tempest.Game.Tests/SimulationTests.cs#L170), both theory cases): its final enemy reaches 400 on the update; keep the post-update assertion, but expect all-rim exit rather than `Playing`.
3. `HurryUpRemainsLatchedAtTheRimWithoutResolvingTheLastEnemy` ([line 193](../../tests/Tempest.Game.Tests/SimulationTests.cs#L193), both theory cases): rename/reframe as a previously latched Hurry-Up yielding to all-rim exit; retain the latch if that is the existing lifecycle policy, but expect the enemy removed and non-kill-accounted.

`ExitCannotStartWithTheFinalActiveFlipper` remains valid because its Flipper stays below rim. `HurryUpStillRequiresFinalKillAndReplayClearsLatch` tests a below-rim final enemy and remains a valid kill/replay regression. `FinalPendingEnemyBlocksCompletionUntilItsZapExpires` has its pending target below rim and should remain unchanged. Kill-all, player-death, and ordinary exit tests should remain unchanged. Full Spiker/Tanker wave kill tests should continue to force and verify the kill path; if their deterministic drivers naturally reach all-rim first after implementation, split that case into explicit kill and rim-clear scenarios rather than weakening ledger assertions.

## 16. Expected implementation files

- `src/Tempest.Game/Levels/Level.cs` — normal non-kill count, remaining/completion semantics, reset/reconstruction conservation.
- `src/Tempest.Game/TempestGame.cs` — active-enemy qualification, Egg/latent-source gate, one non-kill removal/ledger path, Tanker original/cargo rim-clear dispositions, and all-rim-before-Hurry-Up final ordering.
- `src/Tempest.Game/Rendering/VectorRenderer.cs` — use semantic remaining count rather than `EnemyTotal - EnemiesDestroyed`.
- `tests/Tempest.Game.Tests/SimulationTests.cs` — common/standard/Hurry-Up/Spiker/bullet/death/exit cases and existing helper updates.
- `tests/Tempest.Game.Tests/SuperZapperTests.cs` — pending targets, stillborn cargo, Tanker ledger, no delayed/double score, replay.
- `README.md` and `TEMPEST-DEVELOPMENT-NOTES.md` — directly related explanation of non-kill all-rim completion after implementation.

No change to the enemy hierarchy, `SuperZapperSystem`, `Tanker.cs`, project configuration, or behavior spec is inherently required. Update `TEMPEST-BEHAVIOR-SPEC.md` only after implementation and Neal's manual validation.

## 17. Behavior-spec impact after validation

After implementation/manual validation, update:

- §2 “Eggs and Level 1 wave accounting” (currently lines 66–82): describe the three dispositions without conflating kills and rim clears.
- §2 “Late-wave hurry-up” (84–103): state below-rim Hurry-Up behavior and that all-rim completion takes precedence.
- §2 “Completion and tube exit” (105–133): retain arcade observation, identify exact C# threshold as a project/reference-based implementation choice, document non-kill scoring/accounting and when commitment occurs.
- §2 “Super Zapper” (135–152): document pending-target precedence only as a C# policy unless arcade-tested.
- §5 “Spikers and spikes” and §6 “Tankers and cargo Flippers” (214–280): record persistent spike/retry behavior and the separated carrier/cargo dispositions.
- §7 Fuseballs and §8 Pulsars (282–375): clarify that state/lateral/pulse phase do not affect the implemented height qualification, not that these exact internals are arcade-verified.
- §11 “Known uncertainties and deferred behavior” (416–453): remove all-rim implementation from deferred items after validation, but retain unknown exact arcade predicate, pending-zap, collision, and spike-reversal details that remain unobserved.

Do not edit these sections during the analysis/implementation before manual validation.

## 18. Uncertainties requiring manual validation

- Whether Atari uses exactly height 400 and the same state-agnostic rule for all enemy types.
- Whether a direct enemy collision on the same tick as all-rim qualification kills first, particularly with player movement, a Flipper pivot, Fuseball lateral contact, Pulsar flip/pulse, or an enemy bullet.
- Whether pending Super Zapper and Tanker stillborn targets can score during the Atari pre-zoom/avoidance interval; source says they can continue updating before zoom, but Neal has not observed this.
- Whether spike reversal after the rim-clear trigger restores survivors as Eggs or leaves the clear permanent. The recommendation here commits permanently at exit start to satisfy C# requirements.
- Whether active/latent Tanker cargo has any arcade edge case in which the carrier is removed without materializing/resolving cargo.
- Whether the current Spiker height cap and retreat loop correspond to an arcade Spinner's all-rim behavior.
- Whether the visual timing of flash, exit, and spike avoidance remains acceptable when enemy sprites disappear immediately at the C# all-rim decision.

## 19. Explicitly out of scope

No implementation, tests, test execution, behavior-spec edit, gameplay tuning, enemy-state redesign, level progression, score redesign, Tanker composition change, or source-equivalence claim is part of this milestone analysis. Do not change the established 60 Hz simulation, display flash policy, projection, death freeze, normal shot/collision order, score values, Egg pacing, or spike avoidance beyond the minimum accounting needed to preserve all-rim resolution.

## Recommendation

**GO**, as the next standalone gameplay milestone, with the explicit non-kill resolution and Tanker ID partitions above treated as prerequisites—not as a one-line `onrim` predicate. Implement a post-update, post-collision qualification; commit rim-clears at exit start; cancel pending timers for cleared enemies; preserve spikes; and verify all focused tests plus the full 325-test checkpoint before Neal's manual validation. The arcade observation is sufficient for the broad behavior, while numeric/state/timing details remain explicitly source-derived or project decisions.
