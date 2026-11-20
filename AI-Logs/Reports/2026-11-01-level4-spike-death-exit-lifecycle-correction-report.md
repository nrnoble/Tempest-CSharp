# Level 4 Spike death/exit lifecycle correction report

## Correction chronology and final manual result

The original post-death defect was reproduced: after ordinary Level 4 death
and reconstruction, surviving shortened Spikes remained in the level but no
longer responded to player shots. The correction restored those same
persistent Spike objects to the runtime shot-target order. Neal manually
confirmed that post-death shooting works and the shortened height persists.

The first automated long-Spike test was insufficient: it treated destruction
of both short and long Spikes as success and only checked that exit depth
advanced. Subsequent manual long-Spike testing exposed the real failure:
during exit, sustained Fire could shorten an initially long Spike all the way
and allow safe completion. Close-range shot-slot recycling was investigated.
A stronger sustained-fire reproduction through the real resolved Level 4
production exit path was added; against the old implementation it reproduced
the incorrect safe `Complete`, with the Spike removed.

The corrected implementation collides against the still-existing Spike before
same-update shot damage can rescue the Blaster. The production-path regression
now reaches `ReverseZoom` with the Spike still present. Neal's final manual
long-Spike revalidation passed: continuous firing still shortens the Spike,
but the Blaster reaches the remaining Spike and is destroyed. These visible
results are **ARCADE VERIFIED** for the manually tested Level 4 behavior.
Internal C# cooldown and update-order mechanisms remain project decisions.

The existing uncommitted rim-pursuit direction-latch correction and the
post-death Spike registration fix are preserved. No commit or push was
created. The pre-existing untracked `TEMP/` directory was not modified.

## Shot lifecycle investigation

### C# behavior before the correction

- At most 10 player shots can be active.
- With Fire held and an available slot, the usual cooldown permits one shot
  every three updates (`shotTimer > 2`).
- A shot spawns at the player's current depth, moves toward the far end by 6
  units per update, and is removed after a Spike hit or after its height falls
  below zero.
- `UpdatePlayerShots` processes every active shot in one update, in reverse
  list order; thus multiple shots can each resolve a collision in the same
  update. Processing occurs before new firing, so newly created shots cannot
  hit until a later update. A removed shot frees its slot immediately.
- A hit removes 10 Spike height, scores 1, and removes a depleted Spike.
- At close range, shots recycle before the 10-shot cap is reached. The
  three-update cooldown, not slot capacity, bounds the hit rate.
- That rate removes 10 height per three updates (3.33 units/update), slightly
  faster than exit movement (3 or 3.2 units/update). A still-present tip can
  retreat just ahead of the Blaster, allowing even a 390-height Spike to be
  erased before the Blaster reaches it.
- Exit movement itself never paused during firing. The old exit order advanced
  the Blaster, advanced existing shots and shortened Spikes, then tested the
  player's current depth against the already-shortened Spike. It did not test
  whether the Blaster crossed the pre-shot tip during that update. New shots
  were created afterward. Firing and shot processing did not change the
  3/3.2-unit depth advancement.

The close-range hypothesis was therefore partly right about slot recycling,
but the 10-shot capacity did not set the close-range firing rate. The actual
failure was the relative hit-rate/exit-speed balance combined with collision
against the post-shot Spike height only.

### Failure-proving regression

Before the production correction, a sustained-fire test began from the actual
resolved Level 4 AvoidSpikes -> ExitDelay -> ExitZoom path, set a long Spike in
the player's lane, held Fire continuously, and ran the real update loop. It
failed as intended: expected `ReverseZoom`, observed `Complete`, with the Spike
removed and no collision. The earlier test had been too weak: it treated both
60- and 390-height Spikes being fully destroyed while firing as success and
asserted only that exit depth continued advancing. It therefore certified the
bug instead of checking the collision boundary.

## Incitti reference findings

**REFERENCE VERIFIED** against `Reference/incitti/tempest.bmx`:

- `Player.shottimer` starts at 3; `Player.Update` permits firing when the timer
  is greater than 2 and `shot.count < 10`, then resets it to zero and
  increments it every update.
- `Player.AddShot` creates the shot at the player's current `zPos`.
- `Shot.Update` subtracts 6 from shot height, removes it below zero, and
  otherwise checks baddies in list order; a successful collision removes that
  shot and frees its slot.
- `Spikes.CheckColl` requires the same edge and shot height below the Spike
  tip, shortens by 10, awards 1, and removes the Spike at height <= 0.
- In the main loop, `theLevel.Update` advances exit depth first, then
  `shot.UpdateShots` runs, then `Baddies.UpdateBaddies` checks Spike/player
  collision, then `MainPlayer.Update` creates new shots.
- During zoom/reverse zoom, baddie updates and player collision are limited to
  Spikes. `Spikes.CheckPlayerColl` kills when the same-lane Spike height is
  greater than the player's current depth.
- The source checks the current post-shot height and has no explicit swept
  crossing test. This is literal source behavior, not proof of Atari behavior.

Neal's direct arcade observation has higher authority: even during later
invisible levels, continuous firing cannot safely erase an arbitrarily long
Spike while exiting. A sufficiently short Spike can be destroyed before
contact; a remaining long Spike is lethal when the Blaster reaches it.

## Correction and tests

ExitZoom now uses a four-update shot cooldown rather than the ordinary
three-update cooldown. This C# project decision makes damage per update lower
than the established 3/3.2-unit exit movement; it does not claim that Atari
uses a four-update timer.

On each Spike-exit travel update, C# now checks the Blaster's previous and
current depth against the still-existing Spike height before processing
shots. Reaching or crossing that pre-shot tip causes collision, reverse zoom,
and death before same-update damage can rescue the Blaster. This swept
collision boundary is a C# project decision; it is not claimed to be
Atari's internal update order.

Coverage now includes:

- The sustained-fire long-Spike reproduction; it reaches `ReverseZoom` with
  the damaged Spike still present rather than safely completing.
- A short Spike destroyed before contact; the Blaster safely completes exit.
- A same-update movement/shot race; collision wins and the shot does not
  shorten the Spike or award score on that update.
- A complementary crossing after a Spike was removed earlier; no collision
  occurs.
- Reverse/retry with accumulated Spike damage and score retained, one life
  lost, the resolved wave not rebuilt, and another exit attempt available.
- The existing final-life Game Over case, without retry or wave reconstruction.
- The existing post-death regression for same-Spike identity, shortened
  height, renewed shootability, and score.

Neal manually accepted post-death Spike shooting after reconstruction and the
long-Spike sustained-fire collision. Short-Spike safe exit, same-update race,
retry/persistence, and final-life edge cases are covered by automated tests;
the manual acceptance report does not claim that those internal test cases
were individually exercised by Neal.

## Validation

- Pre-correction sustained-fire reproduction on the resolved Level 4 exit:
  failed as intended with `Complete` instead of `ReverseZoom`; the long Spike
  was depleted.
- Exact sustained-fire regression after correction: 1 passed, 0 failed.
- Focused `Level4CandidateTests`: 23 passed, 0 failed.
- Spike/Spiker `SimulationTests`: 27 passed, 0 failed.
- `PlayerDeathPresentationTests`: 5 passed, 0 failed.
- `LevelProgressionTests`: 22 passed, 0 failed.
- `SuperZapperTests`: 31 passed, 0 failed.
- `RimPursuitTests`: 7 passed, 0 failed.
- Tanker tests: 87 passed, 0 failed.
- `dotnet test TempestCSharp.slnx --no-restore`: 591 passed, 0 failed.
- `dotnet build TempestCSharp.slnx --no-restore`: 0 warnings, 0 errors.
- `git diff --check`: passed.

The rim-pursuit direction-latch changes and post-death Spike registration fix
remain in the worktree. `HEAD` remains `09a558e`; no Level 5, Level 8, later
invisible-level, or open-rim gameplay was implemented. `Reference/incitti`
was not modified, and `TEMP/` was left untouched.

Changed files for this correction:

- `src/Tempest.Game/TempestGame.cs`
- `tests/Tempest.Game.Tests/Level4CandidateTests.cs`
- `tests/Tempest.Game.Tests/SimulationTests.cs`
- `README.md`
- `TEMPEST-BEHAVIOR-SPEC.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- This ignored supplemental report under `AI-Logs/Reports/`

The existing uncommitted rim-pursuit work remains in
`src/Tempest.Game/Gameplay/Flipper.cs`,
`src/Tempest.Game/Gameplay/Pulsar.cs`,
`tests/Tempest.Game.Tests/Level2CanFlipTests.cs`,
`tests/Tempest.Game.Tests/Level2SquareTests.cs`,
`tests/Tempest.Game.Tests/Level3GeometryTests.cs`,
`tests/Tempest.Game.Tests/RimPursuitTests.cs`, and the same three documentation
files.

Manual handoff command:

```powershell
dotnet run --project src/Tempest.Game -- --level4-test --lives 10
```

Verify post-death Spike shooting/shortened height; continuously fire at a long
Spike and confirm descending contact, one life loss, retry, and retained
damage; confirm a short Spike destroyed before contact allows safe exit; check
final-life Game Over and briefly recheck latched rim pursuit.
