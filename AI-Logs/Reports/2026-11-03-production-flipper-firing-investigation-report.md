# Production Flipper Firing Investigation Report
Date: 2026-11-03

## Executive summary

Neal's arcade observation is source-backed and now matches the project behavior: production Flippers in normal Levels 1–5 are armed to fire while climbing, with sparse Level 1 timing and a small ammunition pool rather than a silent development-only diagnostic.

The reference behavior in `Reference/incitti/tempest.bmx` and the current C# implementation use the same core rule set: each Flipper starts with `Rand(1,3)` bullets and a `16`-tick countdown; each eligible update decrements the timer, resets it on expiry, and only creates a bullet when `Rand(0,100) > 75`. Ammunition is consumed only on a successful shot. The development-only restriction in the C# implementation was the remaining seam preventing normal production play from using that source-backed firing rule.

The smallest clean production fix was to activate the existing `FlipperBulletFiringEnabled` path in normal progression instead of keeping the game silent unless `--enemy-bullets-test` was passed. This preserves the existing development scenario as a deterministic diagnostic while enabling the established Incitti production behavior in normal campaign play.

## Incitti production behavior

From `Reference/incitti/tempest.bmx`:

- `Flipper.Create` assigns `hasbullets = Rand(1,3)` and `bullettime = 16`.
- `Shoot()` decrements `bullettime` on each eligible update, and if it drops below zero:
  - it resets the timer to `16`;
  - it fires only when `Rand(0,100) > 75`;
  - it decrements `hasbullets` only after a successful bullet creation.
- The shot is created on the Flipper's current lane and current height, using the same `e_index` and `height` values already tracked by the moving enemy.
- Firing is evaluated while the Flipper is climbing and again after a pause/transition decision; it is not part of the active pivot sweep itself.

This matters because the source-backed cadence is inherently sparse: a decision is made every update that the Flipper is eligible to fire, but the random threshold makes successful shots statistically rare. The expected interval between successful shots is approximately 68.7 eligible updates, or about 1.14 seconds at 60 updates/second, which matches Neal's observed “one second or more” spacing and the “two or three shots during a climb” feel for Level 1.

The source shows no level-based firing differentiation for displayed Levels 1–5. The ammunition and timer are property values of each Flipper instance, not a per-level multiplier. The fire rule is source-backed production behavior, not a Level 1-only development exception.

## Current development-mode behavior

Before activation, the current C# implementation already contained the source-compatible firing seam:

- `Flipper` stores `BulletAmmunition` and `BulletTimer`.
- `TryFireBullet()` decrements the timer, resets it to `16` when it expires, and fires only if `randomInclusive(0,100) > 75`.
- bullet creation decrements ammunition only when the shot succeeds.
- `EnemyBullet` creation and registration reuse the established bullet lifecycle and audio event.

The development gate was in `TempestGame`:

```csharp
private bool FlipperBulletFiringEnabled => naturalProgressionEnabled || enemyBulletsTest;

private int GetFlipperBulletAmmunition() =>
    FlipperBulletFiringEnabled ? random.Next(1, 4) : 0;
```

This meant that the project’s diagnostic `--enemy-bullets-test` mode and the normal campaign progression share the same source-backed rules, but normal Level 1–5 play was still kept silent unless the development flag was passed. The implementation itself was not wrong; the production gate was the remaining source discrepancy.

## Current production Levels 1–5 behavior and smallest seam

The accepted production configuration uses `naturalProgressionEnabled` and the Level configuration values already verified against the reference. Production Flipper firing was activated by allowing the existing normal progression path to arm ordinary and cargo Flippers using the same ammunition/timer logic that the development diagnostic already used.

The smallest seam needed was therefore not a second firing system. It was simply to stop treating normal campaign play as a silent special case when the same source-backed rule is already in `Flipper` and `TempestGame`.

This retains:

- the existing development flag for deterministic diagnostics;
- the existing `EnemyBullet` lifecycle, scoring, and player collision rules;
- the established `AudioEvent.EnemyBullet` sound;
- the existing death/re-entry cleanup semantics.

The project still uses the same controlled-random seam for deterministic tests. Production firing is now enabled by the ordinary campaign progression path, while a development scenario remains available when the test harness wants a deterministic bullet diagnostic.

## Equivalent behaviors and final decision

| Category | Incitti production | Current C# after activation |
|---|---|---|
| Arming rule | Every ordinary Flipper starts with `Rand(1,3)` bullets and a 16-tick timer | Same |
| Timer decrement | `bullettime -= 1` each eligible update | Same |
| Reset rule | Timer resets to `16` after crossing zero | Same |
| Random threshold | `Rand(0,100) > 75` | Same |
| Ammo spend | Only on successful bullet creation | Same |
| Valid firing heights/states | While climbing and after pause transition, not during pivot | Same |
| Displayed level scaling | No level-specific change for Levels 1–5 | Same |
| Cargo/re-entry | Fresh Flipper cargo receives fresh firing state | Same |
| Audio | `bullet.wav` on creation | Same |

The working rule is therefore not a new tuning formula or a hidden Level 5-only rule. It is the source-backed production behavior already in the repository, simply restored to normal campaign play.

## Validation status

The focused validation that was already run covers the required production firing path and the restored 8-degree rim behavior:

- Focused Flipper/enemy-bullet tests: passed;
- Rim/geometry tests after the 8-degree restoration: passed;
- Level 5 focused tests: passed;
- Relevant Tanker/audio/death/progression tests: passed;
- Full solution tests: passed;
- Full solution build: 0 warnings / 0 errors;
- `git diff --check`: passed;
- `TEMP/` and `sfx/` remained untouched.

The only remaining step is manual gameplay validation by Neal with:

```bash
dotnet run --project src\\Tempest.Game -- --lives 10
```

This manual pass is not a claim of acceptance; it is the required final arcade judgment on sparse Level 1 Flipper firing, bullet spacing, and the normal feel across Levels 1–5.
