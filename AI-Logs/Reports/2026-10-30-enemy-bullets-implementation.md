# Enemy Bullet Implementation Report

## 1. Summary

Implemented Incitti-style Flipper-fired EnemyBullets in an explicit development
scenario. Normal Level 1 and the Spiker-only development scenario do not enable
Flipper ammunition. Enemy bullets remain projectiles, not enemies: they do not
change wave accounting, enemy score totals or level-clear eligibility.

## 2. Source-Backed Behavior Implemented

- Opt-in Flippers receive an inclusive 1-3 ammunition count and initial timer 16.
- Firing is attempted after the existing climb update and at the existing
  pause-to-edge transition, without changing Flipper state transitions. Each
  eligible call decrements the timer. Below zero, it resets to 16. A controlled
  random result strictly greater than 75 fires and consumes one round; failure
  consumes none.
- Bullets use the Flipper's current post-update height. On the pause transition,
  they use the destination lane selected for that rim-edge change.
- Bullets move outward by 3 height units per active update and expire only when
  height exceeds 400. They rotate by 15 degrees per update.
- Player collision requires the player's exact lane and `400 - bullet.Height < 8`.
  A bullet hit invokes the existing 60-update player-death lifecycle. Existing
  lives handling deducts one life exactly once.
- Player shots hit only on the same lane when shot height is strictly less than
  bullet height. There is no 16-unit band. Hits consume the shot and bullet and
  award 25 points without affecting enemy counts.
- Bullets are removed at life reset and when level exit begins. During the
  established death animation they remain frozen, then are cleared at reset.
- Bullets render separately as white radial segments using the existing tube
  projection and source scale based on 6 units. Player-shot rendering is unchanged.

## 3. Files Changed

Modified:
- `README.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `src/Tempest.Game/Debug/DebugController.cs`
- `src/Tempest.Game/Gameplay/EnemyBullet.cs`
- `src/Tempest.Game/Gameplay/Flipper.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `src/Tempest.Game/TempestGame.cs`
- `tests/Tempest.Game.Tests/SimulationTests.cs`

Created:
- `AI-Logs/2026-10-30-enemy-bullets-implementation.md` (this report)

Nothing under `Reference/incitti` was changed. No other files were created.

## 4. Command-Line Activation Instructions

Normal validated Level 1, with Flipper bullets disabled:

```powershell
dotnet run --project src/Tempest.Game
```

Flipper bullet development scenario:

```powershell
dotnet run --project src/Tempest.Game -- --enemy-bullets-test
dotnet run --project src/Tempest.Game -- --enemy-bullets-test --lives 10
```

Spiker scenario without bullets, or with bullets explicitly enabled:

```powershell
dotnet run --project src/Tempest.Game -- --spiker-test --lives 10
dotnet run --project src/Tempest.Game -- --spiker-test --enemy-bullets-test --lives 10
```

The new flag is independent of the existing flags. Flipper ammunition is only
assigned when `--enemy-bullets-test` is provided.

## 5. Automated Build/Test Results

- Baseline build passed.
- Baseline tests: 90 passed, 0 failed, 0 skipped.
- Final build passed.
- Final tests: 111 passed, 0 failed, 0 skipped.
- `git diff --check` passed.
- Editor diagnostics reported no errors.
- No test opens or controls a Raylib window; the game was not automatically launched.
- Reference-path status check was empty: Incitti reference files remain untouched.

Focused tests cover flag combinations, disabled normal/Spiker-only behavior,
1-3 ammunition selection, timer and random thresholds, firing sites, projectile
movement/expiry, strict shot/player collisions, score, accounting, cleanup,
exit and render projection. All previous Flipper, Spiker, lives and exit tests
continue to pass.

## 6. Manual Validation Checklist

- Run without a flag and verify the normal Level 1 Flippers do not fire bullets.
- Run with `--enemy-bullets-test` and confirm white radial bullets visibly leave
  Flippers on their lane during climbs or after pause transitions.
- Compare ammunition count, cadence, random pauses, bullet speed, scale, rotation
  and depth perspective with the original arcade game.
- Verify bullets threaten only the same lane near the rim; test spinner evasion.
- Shoot bullets and confirm shot consumption and 25-point score presentation.
- Confirm bullet deaths use the existing explosion/death duration and deduct one
  life, with bullets cleared on the subsequent life reset.
- Confirm bullets do not affect enemy count, score already earned, Egg supply,
  level completion or exit travel.
- Recheck cursor capture, spinner direction/sensitivity, Flipper pivot safety,
  Spiker development behavior, and normal level-clear timing.

## 7. Collision-Order Policy

To avoid broad-refactoring the Enemy hierarchy or changing validated normal-play
Flipper collision behavior, test modes maintain the existing development target
list in insertion order. Hatched Flippers are inserted at hatch; bullets append
when fired. A player shot that can hit both therefore checks whichever target was
created first, not nearest depth. A Flipper precedes bullets fired by itself.
Normal Level 1 retains its pre-existing reverse-order Flipper-only collision path.
Enemy bullets are advanced after player shots and before enemy updates; bullets
created during a Flipper update first advance on the next active update. This
avoids changing shot-before-enemy-update behavior and makes same-tick creation
behavior deterministic.

## 8. Known Limitations and Deliberate Omissions

The behavior is source-backed to Incitti's implementation but still requires
arcade comparison. Source iteration over a mutable baddie list does not establish
whether a newly appended bullet receives an update in that same pass; this C#
implementation intentionally defers its first movement until the next update.
Collision ties between a completed Flipper-to-player hit and a bullet-to-player
hit are resolved by the existing Flipper check first, then bullet checks; this
preserves the known Flipper collision handling. Verify both details against arcade
play before treating them as canonical.

Enemy bullets are disabled in normal launch pending manual acceptance. Tankers,
Tanker cargo/bullets, Spiker-fired bullets, Fuseballs, Pulsars, Super Zapper,
audio, bonuses and full level progression remain intentionally out of scope.

## 9. Todo Reconciliation

- Complete: baseline status/build/test recording.
- Complete: EnemyBullet lifecycle, rendering, collision, scoring and cleanup.
- Complete: opt-in Flipper ammunition, timer, random decisions and command-line
  composition with `--spiker-test` and `--lives`.
- Complete: deterministic tests, regression suite, diff review and reference safety.
- Complete: README, development notes and this final report.
- Pending manual validation: visual bullet appearance, pacing, collision feel,
  spinner/cursor behavior and arcade comparison listed above; not automated.
- Intentionally deferred: enabling bullets in normal launch until manual arcade
  comparison; all unrelated enemy types/systems listed above. No code task remains
  in progress.
