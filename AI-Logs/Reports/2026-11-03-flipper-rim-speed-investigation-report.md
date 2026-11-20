# Flipper Rim-Speed Investigation

Date: 2026-11-03

## Conclusion

Incitti's source does not explain the reported slow rim-chase feel as a
Level 1–5 C# speed mismatch. Both implementations use an 8-degree angular
increment per simulation update for Flipper pivots on the first five displayed
levels. No gameplay change is justified by this source comparison. Neal's
25–50% estimate remains a perceptual estimate, not a reference-backed value.

The existing uncommitted Level 5 implementation was left intact. No gameplay
source, tests, or tracked documentation were changed for this investigation.

## Incitti reference

In `Reference/incitti/tempest.bmx`, `SetUpLevel` sets `lv = current_level`
(`tempest.bmx:4210`) and assigns:

- `flipclimbspeed = 2` (`:4263`), or 2 height units per update below the rim;
- `flipflipspeed = 8 + lv/5` degrees (`:4255`);
- `flippausedelayrim = 24 - lv/5` (`:4262`).

The source `current_level` is zero-based. For displayed Levels 1–5, `lv` is
0–4; integer division makes `lv/5` zero. Therefore each has:

- below-rim climb speed: **2 units/update**;
- rim pause: **24 updates**;
- angular pivot speed: **8 degrees/update**.

In `Flipper.Update` (`:2767-2804`), type 1/2 pivots add the fixed
`flipflipspeed` to `angle` each update and compare it with a total angle
derived from the selected edge geometry. A rim pivot finishes when
`angle >= totangle`. Consequently, transition duration is
`ceil(totangle / flipflipspeed)` updates, with the final update allowed to
overshoot before `angle` resets. For example, a 270-degree directed pivot takes
`ceil(270/8) = 34` updates.

The speed increment is not changed by continuous versus open geometry. Geometry
determines the edge's required angle; `continuous` affects edge connectivity,
endpoint reversal, and edge-angle setup (`:1451-1455`), not
`flipflipspeed`. There is no acceleration on reaching the rim: the rim uses
the same fixed angular increment as below-rim canflip pivots.

The source formula does increase at later levels: integer `lv/5` adds one
degree per update every five internal levels. The first increase is displayed
Level 6 (9 degrees/update). That does not affect the current Level 1–5 scope.
The source rim pause also decreases at those boundaries, from 24 to 23 at
displayed Level 6.

## Current C# implementation

`Flipper.Update` reads `level.Configuration.Flipper` and uses:

- below-rim climb: `ClimbSpeed = 2` units/update;
- rim pause: `RimPauseTicks = 24`;
- rim and below-rim pivot: `TurnSpeedDegrees = 8` degrees/update.

The production configurations for Levels 1–5 all specify those values in
`Levels/LevelConfiguration.cs`. During `FlipperState.Flipping`, the C# code
adds `TurnSpeedDegrees` and completes when `TurnAngle >= TotalTurnAngle`.
`TotalTurnAngle` comes from the directed transition's geometry. This yields
the same `ceil(TotalTurnAngle/8)` update duration, including 34 updates for a
full directed 270-degree sweep. The shared speed does not depend on level
identity or continuous topology.

## Direct comparison

| Measure | Incitti Levels 1–5 | Current C# Levels 1–5 |
|---|---:|---:|
| Below-rim climb | 2 units/update | 2 units/update |
| Rim pause | 24 updates | 24 updates |
| Rim angular speed | 8 degrees/update | 8 degrees/update |
| Level scaling within Levels 1–5 | None; `lv/5 = 0` | None; fixed configuration |
| Duration rule | `ceil(total edge angle / speed)` | `ceil(directed sweep angle / speed)` |
| 270-degree transition | 34 updates | 34 updates |
| Rim-arrival acceleration | None | None |
| Speed change for open geometry | None | None |

**Mathematical difference over Levels 1–5:** `8 - 8 = 0 degrees/update`,
or **0%**. The requested 25–50% adjustment would instead mean 10–12 degrees
per update; there is no support for that adjustment in Incitti's Level 1–5
formula.

## Preserved behavior and evidence

No changes were made to pursuit direction, directed sweeps, geometry,
collision order, direct rim-arrival collision, completed-pivot collision,
player-shot behavior, or below-rim movement.

Existing focused coverage includes:

- `RimPursuitTests`: latched pursuit direction and subsequent pivot route;
- `DirectedRotationInvestigationTests`: full 270-degree sweeps and
  geometry-dependent duration;
- `Level3GeometryTests`: directed plus-board pivots and canonical completion;
- `Level2SquareTests` and `Level2CanFlipTests`: arrival/completed-pivot
  collision, player shots, and collision ordering;
- `LevelProgressionTests` and `Level5ProductionTests`: Level 1–5 production
  configuration and progression context.

These are source and automated-test findings, not a claim of manual acceptance
for any new speed tuning.

## Decision requested

The source comparison found no bounded speed correction. Keep the existing
reference-matched 8-degree value unless Neal chooses a separate perceptual
tuning experiment. Any such experiment should be isolated from the uncommitted
Level 5 change and must retain the already accepted direction, sweep,
geometry, pause, collision, shot-ordering, and below-rim behavior.
