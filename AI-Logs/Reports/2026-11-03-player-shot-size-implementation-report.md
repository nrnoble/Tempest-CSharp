# Player-Shot Size Refinement — Implementation Report

## Result

Increased the player-shot star's rendering-only base radius from 4 to 4.5
projection units (12.5%). This is the first bounded adjustment in response to
Neal's observation that C# shots should be slightly larger. The number is a
project tuning choice, not a measurement of the arcade game. Neal manually
accepted the 4.5-unit shots as a noticeable visual improvement and requested
that this size be retained; additional subjective tuning is not worthwhile.

## Existing and reference behavior

- C# rendered each shot as eight white radial line segments, 45 degrees apart,
  centered at its lane midpoint. The radius was
  `4 / ((projectedDepth + Projection.ZOffset) / Projection.K)`, with projected
  depth `level depth - shot height + level position`. As shot height falls by
  6 each update, its projected depth increases and the star shrinks with
  distance into the tube.
- Mark Incitti's `Shot.Draw` uses the same eight-spoke shape, 45-degree
  spacing, depth projection, and a base size of 4 via `TFormSZ(4, zz)`.
  Incitti was used as engineering context, not as the numerical authority for
  this adjustment.
- Shot rotation changes by 15 degrees per simulation update and is used only
  by rendering. Collision checks use shot lane and height; they do not use
  rendered star radius.

## Change and preserved behavior

`VectorRenderer.PlayerShotBaseSize` now centralizes the 4.5-unit draw radius.
The existing projection scaling, lane-center placement, eight-line shape, and
rotation remain unchanged. A callback-based drawing entry point follows the
existing enemy-bullet rendering/test pattern and lets the rendering test
capture the actual line geometry.

No simulation or collision code changed. Shot movement speed (6 per update),
three-tick firing cooldown, ten-shot cap, lifetime, collision boundaries,
scoring, Spike/enemy handling, and other gameplay systems remain as before.

## Validation

- Focused player-shot rendering/gameplay filter: **12 passed, 0 failed**,
  including the new near/far geometry and finite-coordinate check, firing
  cadence/cap, immediate movement, rotation/lifetime, strict collision
  boundaries, and Spike scoring/depletion tests.
- Complete solution tests: **641 passed, 0 failed, 0 skipped**.
- Complete solution build: **succeeded**, 0 warnings, 0 errors.
- `git diff --check`: passed; no whitespace errors.
- Git status lists the renderer and development notes as modified and the new
  rendering test as untracked. The requested prompt and report are saved under
  `AI-Logs`, which this repository ignores. Nothing was staged or committed.

## Manual acceptance

Neal manually accepted the 4.5-unit shots as a noticeable visual improvement
and requested that this size be retained. The value is a project decision
based on that visual acceptance, not a measured Atari constant.

```powershell
dotnet run --project src/Tempest.Game
```

Check that shots are visibly somewhat larger but still look like Tempest
vector projectiles; remain sensible near the Blaster and deeper in the tube;
render smoothly; hit enemies and applicable Spikes normally; show no
perceptible change to firing cadence or travel speed; and produce no rendering
artifacts or unrelated regressions. Neal reports the accepted appearance is
good and further subjective/perceptual iteration is not worthwhile. No further
shot-size tuning is requested.
