# Report: Direct Flipper Rim-Arrival Collision Fix

## Root cause

In `Flipper.Update`, the `Climbing` branch always called `BeginPause()` on
reaching `Level.Depth`, even when the blaster occupied that lane. Only completed
pivots set `CompletedIntoPlayer`, so a direct arrival never killed the player.

## Incitti reference

`Flipper.CheckPlayerColl` in `Reference/incitti/tempest.bmx` kills the player when
the player's lane equals the Flipper's lane and `height = depth`. For a climbing
Flipper (type other than 1 or 2) that is unconditional. For a pivoting Flipper
(type 1 or 2) it kills only when `Abs(totangle - angle) < 30`. The C# pivot
behavior (kill on completed pivot into the occupied lane at full depth) was left
unchanged.

## Change

- `Gameplay/Flipper.cs`: when a climbing Flipper reaches `Level.Depth` and
  `EdgeIndex == playerEdge`, it sets `CompletedIntoPlayer` instead of beginning the
  rim pause. Otherwise it pauses as before. The same existing signal now covers
  both lethal outcomes; no new flag was added.
- `TempestGame.cs`: unchanged. `CheckFlippersAgainstPlayer` already consumes the
  signal and calls the established `KillPlayer` path. The update order is
  unchanged, so player-shot collision still runs before enemy-to-player collision
  and a shot kill cannot also produce a death.
- Climb speed, rim pause, pivot speed, pivot lanes, shot collision, bullets and
  Tanker cargo initialization were not touched.

## Tests

Added to `SimulationTests.cs`:

- `ClimbingFlipperReachingRimInPlayerLaneSignalsLethalCollision`
- `ClimbingFlipperReachingRimInAnotherLaneStillPauses`
- `DirectRimArrivalKillsPlayerOnceAndShotOnFinalTickWinsInstead` (direct arrival
  reaches `PlayerDying`; a shot on the final tick destroys the Flipper with no
  death and unchanged lives)

- `DirectRimArrivalRunsFullDeathLifecycleAndRestartsUnresolvedEnemiesAsFreshEggs`
  (a prior destroyed slot and earned score, then a direct rim arrival in the
  blaster's lane: `PlayerDying` with `DeathTicks` 60; after 59 death updates
  `DeathTicks` is 1 and lives are unchanged; the 60th update deducts exactly one
  life and returns to `Playing`; score and the destroyed slot are preserved; no
  pre-death Flipper remains; unresolved slots return as distinct fresh Eggs at
  height -10 in the Roaming state, none reused from before death, with the Tanker
  slot back as one Egg carrying two latent cargo units; Egg plus reserve count
  is 9 and the full Tanker accounting invariants hold)

Two existing tests encoded the buggy scenario (player still in the Flipper's lane
at rim arrival, expecting a pause): `UndestroyedFinalFlipperStillKillsPlayerAtCompletedPivot`
and `FinalPivotTickKillStartsExitInsteadOfKillingPlayer`. The player now leaves the
lane earlier (tick 190 instead of 199) with total step counts preserved, so they
still exercise pivot-completion behavior.

## Validation

- `dotnet build`: passed, 0 errors.
- `dotnet test`: 146 total, 146 passed, 0 failed, 0 skipped (142 prior plus 4 new).
- `git diff --check`: clean. `Reference/incitti` unmodified.
- Not committed or pushed.

## Remaining manual validation

- Let a Flipper climb to the rim in the blaster's lane and confirm the death
  sequence runs, one life is lost, and unresolved enemies return as fresh Eggs.
- Confirm a shot on the final climbing tick still destroys the Flipper without a
  death.
- The full death lifecycle after a direct rim arrival is now covered by an
  automated test; no production defect was found, so production code is unchanged
  from the original fix. A visual check in the game window is still worthwhile.
