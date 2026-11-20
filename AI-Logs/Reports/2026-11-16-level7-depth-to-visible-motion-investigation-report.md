# Level 7 depth-to-visible-motion investigation

Date: 2026-11-16

## Status and direct answers

**READ-ONLY INVESTIGATION COMPLETE. No correction is justified by the current
evidence; no source, test, presentation, or gameplay code was changed.**

The manual symptoms remain **MANUAL OBSERVATIONS**. They are not reproduced by
the current mapped depth transfer in a constant-depth-step probe:

1. **Why do enemies visually decelerate toward the rim?** For registered Level
   7 entities that use `ProjectEnemyDepth` and the mapped lane adapter, the
   current transfer predicts constant anchor displacement for a constant
   height increment, not deceleration. The prior reciprocal renderer predicts
   acceleration toward the rim, the opposite trend.
2. **Why do Blaster shots visually accelerate toward the deep end?** The
   current shot path uses the same linear mapped lane transfer, so a constant
   `-6` height step predicts constant displacement, not acceleration. The
   prior reciprocal renderer predicts deceleration toward the deep end, again
   the opposite trend.
3. **Why do Blaster and Flipper rim movements remain normal?** Normal Blaster
   movement uses `EdgePosition` on the mapped near rim at `t=0`. A Flipper at
   the rim also maps to `t=0`; its lane changes use the separate near-crossing
   transition geometry. Neither path traverses the depth parameter.
4. **Is simulation speed correct while presentation speed is distorted?**
   This investigation did not recalibrate gameplay speeds. The source has
   distinct, explicit per-state simulation increments. However, the current
   registered presentation path does not distort constant increments along
   one fixed lane. Thus the proposed “valid simulation speed, distorted
   presentation transfer” explanation is not established by this code trace.
5. **What exact transfer causes the result?** The active registered transfer
   is `P(t)=N+t(F-N)`, with `t=1-height/400` for ordinary enemy/shot height
   conventions. It has constant derivative. It cannot alone produce the
   reported opposing trends. The previous Level 7 projection was reciprocal
   and would produce the reverse trends. No single depth transfer in these
   paths explains both observations.

The narrowest remaining uncertainty is whether the manual observation was
made on the exact mapped anchor paths with the assumed per-tick height deltas.
The source trace and deterministic mapping probe rule out depth-dependent
acceleration in those paths; they do not capture an actual gameplay object’s
state sequence at the moment of the observation. Therefore there is not enough
evidence for a bounded implementation prompt or a render-transfer change.

## Scope, evidence, and preservation

The investigation was limited to the current opt-in
`--atari-presentation --level 7` path during ordinary `Playing`, its previous
projection paths, relevant entity updates, camera mapping, and recording /
playback architecture. It did not launch the game, inspect screenshots or
video, inspect protected `TEMP/` notes, or open raw ROM copies. The record-06
catalog and prior task reports were used as code/documentation context; no
new claim about Atari entity-motion timing is made.

Evidence classifications:

- The reported enemy, shot, and rim-motion symptoms are **MANUAL
  OBSERVATIONS**, as provided in the task.
- Record 06 and the prior rendering/registration choices retain the
  classifications in their existing reports. The current lane-depth mapping,
  far-set translation, and centering are **PROJECT TRANSLATION** candidates.
- The prior reciprocal equation is behavior implemented by the former
  presentation path. Its being arcade-faithful for entity motion on the new
  record-06 rails is **UNKNOWN**; it is not promoted here to Atari motion
  evidence.
- The synthetic depth-step calculations below are **DEVELOPMENT-ONLY**.

## Starting Git state and crash-fix separation

At investigation start:

- Branch: `master`
- HEAD: `385f603a94391756b2ca6cccd524dc17b281e782`
- Upstream: `origin/master`
- Index: empty
- Tracked worktree: 17 pre-existing modified files
- Non-ignored untracked state: 17 pre-existing paths, including the current
  candidate source/tests, two local arcade reference files, and
  `screenshot000.png` (not opened)

The current candidate sources therefore were not a clean committed baseline.
They were read in place and left untouched.

The mapped-Flipper crash-fix report identifies the change as
`AtariLevel7EntityPresentation.MapFlipperTransition` plus its tests and
explicitly excludes `VectorRenderer`, record-06 data, lane/depth registration,
centering, non-Flipper entities, simulation, timing, recording, and playback.
The current source is consistent with that separation: the corrected
`MapFlipperTransition` is called only for `FlipperState.Flipping`; a
non-transition Flipper goes through `GetLevel7LaneGeometry` and the common
linear lane-depth mapping. Tanker cargo uses the same render path. The current
tracked `VectorRenderer.cs` has pre-existing candidate changes, but the crash
fix report does not attribute those changes to the crash correction. The
crash fix is not reopened here.

## Current mapped depth transfer

In ordinary Level 7 `Playing`, the candidate is selected only when Atari
presentation is enabled, the displayed level is 7, and the game state is
`Playing`. The registered lane anchor path is:

```text
projectedDepth = Position + Depth - height
t = (projectedDepth - Position) / Depth
  = 1 - height / Depth
V_i(t) = Near_i + t * (Far_i - Near_i)
C_lane(t) = (V_i(t) + V_(i+1)(t)) / 2
```

Level 7 uses `Position=50` and `Depth=400`. Thus `height=400` gives near/rim
`t=0`, `height=0` gives far/deep `t=1`, and negative Egg heights give `t>1`.
`NormalizeDepth` is a float subtraction/division. It does not clamp; the
mapped `Vector2.Lerp` also permits extrapolation.

For a fixed lane, let
`D_lane = C_lane(1) - C_lane(0)`. Then:

```text
C_lane(t) = C_lane(0) + t * D_lane
delta C per tick = D_lane * delta t
|delta host| = 1.26 * |D_lane| * |delta height| / 400
```

The final factor is the shared portrait playfield camera zoom. For a fixed
simulation height step, neither the logical nor host displacement depends on
the starting depth `t`.

The mapped endpoints already include the record-06 record projection, the
anisotropic horizontal/vertical presentation gains, the far-set offset, and a
shared centering translation. Their relevant constants are:

- Horizontal and vertical reference gains: `0.4215` and `0.575`.
- Shared global scale: `0.345`; shared horizontal aspect: about `0.7330435`.
- Far-set logical translation before common centering: `(+33.75, +46.25)`.
- Common centering translation: about `(+97.471, +78.1905)` logical units.
- Final playfield camera: zero rotation, uniform zoom `1.26`.

The far-set translation contributes a constant term to `Far_i-Near_i`; the
anisotropic gains shape that vector. Both can change the total speed and angle
from one lane to another, but neither makes a fixed lane’s speed vary with
depth. The shared centering is additive and cancels from every displacement.

### Representative mapped rails

Near/far lane-center coordinates below include centering. Host coordinates use
the shared playfield camera. Lane 13 is a long visible rail, lane 6 a short
one, and lane 0 has a substantially different direction from lane 13.

| Lane | Near center (logical) | Far center (logical) | Far-minus-near vector | Logical rail length | Host pixels per unit `t` | Direction |
|---:|---:|---:|---:|---:|---:|---:|
| 13 (long) | (296.44, 255.32) | (385.19, 490.80) | (88.76, 235.48) | 251.654 | 317.084 | 69.35 deg |
| 6 (short) | (425.67, 648.10) | (404.54, 549.97) | (-21.13, -98.13) | 100.381 | 126.481 | -102.15 deg |
| 0 (different angle) | (503.44, 255.32) | (416.30, 490.80) | (-87.14, 235.48) | 251.087 | 316.370 | 110.31 deg |

These lane differences explain different constant speeds between lanes. They
do not explain acceleration or deceleration along a fixed lane.

## Deterministic depth-step telemetry

The following probe advances `t` by exactly `0.01` (four Level 7 depth units)
on lane 13 at each requested sample. Coordinates are the current mapped
logical lane-center and final host/screen positions. Every row has the same
`2.5165` logical / `3.1708` host-pixel displacement.

| `t` before -> after | Logical before -> after | Host before -> after | Displacement |
|---:|---|---|---:|
| 0.00 -> 0.01 | (296.44,255.32) -> (297.33,257.68) | (509.51,281.98) -> (510.63,284.95) | 2.5165 / 3.1708 |
| 0.10 -> 0.11 | (305.31,278.87) -> (306.20,281.22) | (520.69,311.65) -> (521.81,314.62) | 2.5165 / 3.1708 |
| 0.25 -> 0.26 | (318.63,314.19) -> (319.51,316.55) | (537.47,356.16) -> (538.59,359.13) | 2.5165 / 3.1708 |
| 0.50 -> 0.51 | (340.82,373.06) -> (341.70,375.42) | (565.43,430.34) -> (566.55,433.30) | 2.5165 / 3.1708 |
| 0.75 -> 0.76 | (363.00,431.93) -> (363.89,434.29) | (593.39,504.51) -> (594.50,507.48) | 2.5165 / 3.1708 |
| 0.90 -> 0.91 | (376.32,467.25) -> (377.21,469.61) | (610.16,549.02) -> (611.28,551.99) | 2.5165 / 3.1708 |
| 1.00 -> 1.01 | (385.19,490.80) -> (386.08,493.16) | (621.34,578.69) -> (622.46,581.66) | 2.5165 / 3.1708 |

The `t=1.01` row is a synthetic extrapolation, not a claim that every
production object remains alive beyond the far end. Player shots are removed
after their height drops below zero; enemy bullets are removed after advancing
past `Level.Depth`. Eggs are the explicit production path that remains
renderable at `t>1`.

### Per-class simulation steps on the same long lane

The next table samples each registered class at multiple depths on lane 13.
All logical and host coordinates are the current mapped positions. `height`
is the class’s C# along-tube height; for the shot its decrease moves `t`
toward 1. Local scale is the mapped lane-edge ratio at the `t` before the
step; it affects glyph size, not the listed anchor displacement. The table is
a deterministic fixed-lane probe. It does not assert that every synthetic
height/depth combination is an independently reachable gameplay state.

| Class / direction | `t` before -> after | Height before -> after | Logical before -> after | Host before -> after | `|delta host|` | Local scale |
|---|---:|---:|---|---|---:|---:|
| Tanker, climbing | 0.100 -> 0.09563 | 360 -> 361.75 | (305.31,278.87) -> (304.92,277.84) | (520.69,311.65) -> (520.21,310.36) | 1.3872 | 0.915 |
| Tanker, climbing | 0.250 -> 0.24563 | 300 -> 301.75 | (318.63,314.19) -> (318.24,313.16) | (537.47,356.16) -> (536.98,354.86) | 1.3872 | 0.788 |
| Tanker, climbing | 0.500 -> 0.49563 | 200 -> 201.75 | (340.82,373.06) -> (340.43,372.03) | (565.43,430.34) -> (564.94,429.04) | 1.3872 | 0.576 |
| Tanker, climbing | 0.750 -> 0.74563 | 100 -> 101.75 | (363.00,431.93) -> (362.62,430.90) | (593.39,504.51) -> (592.90,503.22) | 1.3872 | 0.363 |
| Tanker, climbing | 0.900 -> 0.89563 | 40 -> 41.75 | (376.32,467.25) -> (375.93,466.22) | (610.16,549.02) -> (609.67,547.72) | 1.3872 | 0.236 |
| Tanker, climbing | 1.000 -> 0.99563 | 0 -> 1.75 | (385.19,490.80) -> (384.80,489.77) | (621.34,578.69) -> (620.85,577.39) | 1.3872 | 0.151 |
| Ordinary Flipper, climbing | 0.100 -> 0.09500 | 360 -> 362 | (305.31,278.87) -> (304.87,277.69) | (520.69,311.65) -> (520.14,310.17) | 1.5854 | 0.915 |
| Ordinary Flipper, climbing | 0.250 -> 0.24500 | 300 -> 302 | (318.63,314.19) -> (318.18,313.01) | (537.47,356.16) -> (536.91,354.68) | 1.5854 | 0.788 |
| Ordinary Flipper, climbing | 0.500 -> 0.49500 | 200 -> 202 | (340.82,373.06) -> (340.37,371.88) | (565.43,430.34) -> (564.87,428.85) | 1.5854 | 0.576 |
| Ordinary Flipper, climbing | 0.750 -> 0.74500 | 100 -> 102 | (363.00,431.93) -> (362.56,430.75) | (593.39,504.51) -> (592.83,503.03) | 1.5854 | 0.363 |
| Ordinary Flipper, climbing | 0.900 -> 0.89500 | 40 -> 42 | (376.32,467.25) -> (375.87,466.08) | (610.16,549.02) -> (609.60,547.54) | 1.5854 | 0.236 |
| Ordinary Flipper, climbing | 1.000 -> 0.99500 | 0 -> 2 | (385.19,490.80) -> (384.75,489.63) | (621.34,578.69) -> (620.78,577.21) | 1.5854 | 0.151 |
| Tanker-released Flipper, climbing component | 0.100 -> 0.09500 | 360 -> 362 | (305.31,278.87) -> (304.87,277.69) | (520.69,311.65) -> (520.14,310.17) | 1.5854 | 0.915 |
| Tanker-released Flipper, climbing component | 0.250 -> 0.24500 | 300 -> 302 | (318.63,314.19) -> (318.18,313.01) | (537.47,356.16) -> (536.91,354.68) | 1.5854 | 0.788 |
| Tanker-released Flipper, climbing component | 0.500 -> 0.49500 | 200 -> 202 | (340.82,373.06) -> (340.37,371.88) | (565.43,430.34) -> (564.87,428.85) | 1.5854 | 0.576 |
| Tanker-released Flipper, climbing component | 0.750 -> 0.74500 | 100 -> 102 | (363.00,431.93) -> (362.56,430.75) | (593.39,504.51) -> (592.83,503.03) | 1.5854 | 0.363 |
| Tanker-released Flipper, climbing component | 0.900 -> 0.89500 | 40 -> 42 | (376.32,467.25) -> (375.87,466.08) | (610.16,549.02) -> (609.60,547.54) | 1.5854 | 0.236 |
| Tanker-released Flipper, climbing component | 1.000 -> 0.99500 | 0 -> 2 | (385.19,490.80) -> (384.75,489.63) | (621.34,578.69) -> (620.78,577.21) | 1.5854 | 0.151 |
| Spiker, building (default Level 7 speed) | 0.100 -> 0.09563 | 360 -> 361.75 | (305.31,278.87) -> (304.92,277.84) | (520.69,311.65) -> (520.21,310.36) | 1.3872 | 0.915 |
| Spiker, building (default Level 7 speed) | 0.250 -> 0.24563 | 300 -> 301.75 | (318.63,314.19) -> (318.24,313.16) | (537.47,356.16) -> (536.98,354.86) | 1.3872 | 0.788 |
| Spiker, building (default Level 7 speed) | 0.500 -> 0.49563 | 200 -> 201.75 | (340.82,373.06) -> (340.43,372.03) | (565.43,430.34) -> (564.94,429.04) | 1.3872 | 0.576 |
| Spiker, building (default Level 7 speed) | 0.750 -> 0.74563 | 100 -> 101.75 | (363.00,431.93) -> (362.62,430.90) | (593.39,504.51) -> (592.90,503.22) | 1.3872 | 0.363 |
| Spiker, building (default Level 7 speed) | 0.900 -> 0.89563 | 40 -> 41.75 | (376.32,467.25) -> (375.93,466.22) | (610.16,549.02) -> (609.67,547.72) | 1.3872 | 0.236 |
| Spiker, building (default Level 7 speed) | 1.000 -> 0.99563 | 0 -> 1.75 | (385.19,490.80) -> (384.80,489.77) | (621.34,578.69) -> (620.85,577.39) | 1.3872 | 0.151 |
| Spiker, retreating | 0.100 -> 0.10394 | 360 -> 358.425 | (305.31,278.87) -> (305.66,279.80) | (520.69,311.65) -> (521.13,312.82) | 1.2485 | 0.915 |
| Spiker, retreating | 0.250 -> 0.25394 | 300 -> 298.425 | (318.63,314.19) -> (318.98,315.12) | (537.47,356.16) -> (537.91,357.33) | 1.2485 | 0.788 |
| Spiker, retreating | 0.500 -> 0.50394 | 200 -> 198.425 | (340.82,373.06) -> (341.16,373.99) | (565.43,430.34) -> (565.87,431.51) | 1.2485 | 0.576 |
| Spiker, retreating | 0.750 -> 0.75394 | 100 -> 98.425 | (363.00,431.93) -> (363.35,432.86) | (593.39,504.51) -> (593.83,505.68) | 1.2485 | 0.363 |
| Spiker, retreating | 0.900 -> 0.90394 | 40 -> 38.425 | (376.32,467.25) -> (376.67,468.18) | (610.16,549.02) -> (610.60,550.19) | 1.2485 | 0.236 |
| Spiker, retreating | 1.000 -> 1.00000 | 0 -> 0 | (385.19,490.80) -> (385.19,490.80) | (621.34,578.69) -> (621.34,578.69) | 0.0000 | 0.151 |
| Enemy bullet, toward rim | 0.100 -> 0.09250 | 360 -> 363 | (305.31,278.87) -> (304.65,277.10) | (520.69,311.65) -> (519.86,309.43) | 2.3781 | 0.915 |
| Enemy bullet, toward rim | 0.250 -> 0.24250 | 300 -> 303 | (318.63,314.19) -> (317.96,312.42) | (537.47,356.16) -> (536.63,353.94) | 2.3781 | 0.788 |
| Enemy bullet, toward rim | 0.500 -> 0.49250 | 200 -> 203 | (340.82,373.06) -> (340.15,371.30) | (565.43,430.34) -> (564.59,428.11) | 2.3781 | 0.576 |
| Enemy bullet, toward rim | 0.750 -> 0.74250 | 100 -> 103 | (363.00,431.93) -> (362.34,430.17) | (593.39,504.51) -> (592.55,502.29) | 2.3781 | 0.363 |
| Enemy bullet, toward rim | 0.900 -> 0.89250 | 40 -> 43 | (376.32,467.25) -> (375.65,465.49) | (610.16,549.02) -> (609.32,546.80) | 2.3781 | 0.236 |
| Enemy bullet, toward rim | 1.000 -> 0.99250 | 0 -> 3 | (385.19,490.80) -> (384.53,489.04) | (621.34,578.69) -> (620.50,576.47) | 2.3781 | 0.151 |
| Blaster shot, toward far end | 0.100 -> 0.11500 | 360 -> 354 | (305.31,278.87) -> (306.64,282.40) | (520.69,311.65) -> (522.37,316.11) | 4.7563 | 0.915 |
| Blaster shot, toward far end | 0.250 -> 0.26500 | 300 -> 294 | (318.63,314.19) -> (319.96,317.72) | (537.47,356.16) -> (539.15,360.61) | 4.7563 | 0.788 |
| Blaster shot, toward far end | 0.500 -> 0.51500 | 200 -> 194 | (340.82,373.06) -> (342.15,376.59) | (565.43,430.34) -> (567.10,434.79) | 4.7563 | 0.576 |
| Blaster shot, toward far end | 0.750 -> 0.76500 | 100 -> 94 | (363.00,431.93) -> (364.34,435.46) | (593.39,504.51) -> (595.06,508.97) | 4.7563 | 0.363 |
| Blaster shot, toward far end | 0.900 -> 0.91500 | 40 -> 34 | (376.32,467.25) -> (377.65,470.79) | (610.16,549.02) -> (611.84,553.47) | 4.7563 | 0.236 |
| Blaster shot, toward far end | 1.000 -> 1.01500 | 0 -> -6 | (385.19,490.80) -> (386.52,494.34) | (621.34,578.69) -> (623.02,583.14) | 4.7563* | 0.151 |

`*` The final Blaster-shot row is the arithmetic next point only. In normal
simulation the shot is removed after its height falls below zero, before a
subsequent render. A live shot therefore does not persist to visible `t>1`.
At `t=0`, Flippers cap at `Level.Depth`, Spikers leave Building near their
upper limit, Tankers enter release handling, and an enemy bullet that advances
past `Depth` is expired. Those state boundaries can create a stop, handoff, or
removal; they are not smooth depth-dependent acceleration.

The Level 7 Tanker’s default climb and the default Level 7 Spiker speed are
both `1.75` units/tick (Spiker can have an explicit diagnostic override).
Flipper climbing uses `2` units/tick; Tanker cargo inherits its release
height and uses the Flipper route. The Flipper cargo table is the depth
component at fixed lane/transition progress; while a pivot is actively
progressing, lane-transition motion is an additional independent component.

### Lane, scale, and object-specific boundaries

For the same per-tick height changes, host displacements on lanes 13 / 6 / 0
are:

| Object / state | Height change per tick (or event) | `delta t` | Lane 13 px | Lane 6 px | Lane 0 px |
|---|---:|---:|---:|---:|---:|
| Tanker climbing | +1.75 | -0.004375 | 1.3872 | 0.5534 | 1.3841 |
| Flipper climbing (ordinary or cargo) | +2 | -0.005000 | 1.5854 | 0.6324 | 1.5818 |
| Spiker building (default Level 7 speed) | +1.75 | -0.004375 | 1.3872 | 0.5534 | 1.3841 |
| Spiker retreating | -1.575 (`-0.90 * 1.75`) | +0.003938 | 1.2485 | 0.4980 | 1.2457 |
| Enemy bullet | +3 | -0.007500 | 2.3781 | 0.9486 | 2.3728 |
| Blaster shot | -6 | +0.015000 | 4.7563 | 1.8972 | 4.7455 |
| Egg roaming, on its eighth-tick update | +1 height | -0.050000 | 15.8542 | 6.3240 | 15.8185 |
| Egg approaching, on its eighth-tick update | +2 height | -0.100000 | 31.7084 | 12.6481 | 31.6370 |

Egg values are per active vertical update, not per simulation tick; the other
seven ticks have no vertical increment. Its `projectedDepth` is
`Position + Depth - 20 * Egg.Height`, so `t=1-Egg.Height/20`. The initial
height `-10` gives `t=1.5`; an ordinary roaming update from `-10` to `-9`
moves from `(429.57,608.54)` to `(425.13,596.77)` logical, or
`(677.26,727.05)` to `(671.67,712.21)` host, on lane 13. The t>1 trajectory
is explicit linear extrapolation, not a clamped position.

For lane 13, `LocalScale = current mapped rim-edge length / near-rim edge
length` is approximately `1.000, 0.915, 0.788, 0.576, 0.363, 0.236, 0.151`
at `t=0, .1, .25, .5, .75, .9, 1`. Tankers, Flippers, Spikers, and shot
glyphs use this scale for their shapes. It changes apparent glyph size, but
not the anchor coordinates in the tables. Eggs use a fixed point size.

The configured starting Spike height is `60`. Its base is at the far endpoint
(`t=1`) and its tip at `t=1-60/400=.85`. On lane 13 the tip is
`(371.88,455.48)` logical / `(604.57,534.19)` host and the base is
`(385.19,490.80)` logical / `(621.34,578.69)` host. The Spike does not move
per tick; a hit shortens its height by `10`, moving only the tip by
`delta t=+0.025` while the base stays fixed.

There is no concrete class called “normal enemy” with one universal speed.
Level 7 production composition is Flippers, Tankers, and Spikers, which are
traced separately above. Optional Fuseball-test entities are not normal Level
7 production composition: `ProjectFuseballCenter` uses the non-lane
`ProjectWorldPoint` path and therefore the Atari reciprocal presentation
projection instead of this record-06 lane adapter. Pulsar drawing has similar
non-lane projection calls. Those paths are not the registered production
entities in the reported observation and also do not predict the reported
reciprocal pair of trends.

Enemy-destruction effects inherit a destroyed enemy’s height and use
`ProjectEnemyDepth` / `ProjectLane`; the captured anchor is fixed unless the
effect state explicitly changes it. Tanker cargo is a Flipper with the
Tanker’s release height and lane, then uses the Flipper update/projection
path. It does not have a separate height-to-depth function.

## Prior production transfer and reciprocal controls

Before record-06 registration, `ProjectWorldPoint` for Level 7 used
`AtariPresentationAdapter.Project` when Atari presentation was enabled, or
`Projection.Project` on the legacy route. The Atari adapter normalizes depth
to `t` and computes:

```text
reciprocalDepth = 16 + 224*t
pointDenominator = reciprocalDepth + ScaleRaw
                 = 40 + 224*t       (Level 7 ScaleRaw = 0x18 = 24)
ReciprocalProjection(a, t) = 256*a / (40 + 224*t)
screen coordinate = constant baseline + constant gain * ReciprocalProjection(...)
```

The normalized position of this old reciprocal response between its endpoints
is:

```text
u(t) = (1/40 - 1/(40+224*t)) / (1/40 - 1/264)
```

It is not the current adapter’s `t`. Its derivative falls strongly as `t`
increases; the ratio of endpoint derivatives is
`u'(0)/u'(1) = (264/40)^2 = 43.56`. A normalized reciprocal control remapped
onto the new rails gives the following host displacement for equal
`delta t=.01`:

| `t` interval | Reciprocal `delta u` | Lane 13 host px | Lane 6 host px |
|---:|---:|---:|---:|
| 0.00 -> 0.01 | 0.0625000 | 19.8177 | 7.9050 |
| 0.10 -> 0.11 | 0.0261805 | 8.3014 | 3.3113 |
| 0.25 -> 0.26 | 0.0111971 | 3.5504 | 1.4162 |
| 0.50 -> 0.51 | 0.0045043 | 1.4282 | 0.5697 |
| 0.75 -> 0.76 | 0.0024148 | 0.7657 | 0.3054 |
| 0.90 -> 0.91 | 0.0017925 | 0.5684 | 0.2267 |
| 1.00 -> 1.01 | 0.0015024 | 0.4764 | 0.1900 |

This old response is fastest near and slowest far. Thus it predicts a Blaster
shot slowing as it travels toward the far end, and an enemy speeding up as it
approaches the rim. The legacy `Projection.Project` path has the same
qualitative reciprocal-depth derivative (`K/(z+ZOffset)`) for a fixed point.
It too predicts the opposite of the manual observations.

The new mapped adapter replaces that old nonlinear object-coordinate path
with linear interpolation between already mapped near/far endpoints for the
registered Level 7 anchors. The tube rails are straight in both cases; this
change is specifically a change in progression along the rail. The previous
nonlinear behavior was part of the old C# presentation, but its correctness
for Atari object motion on record 06 is not established. Reintroducing it
would also not match the reported trend.

## Answers to the remaining mathematical questions

- **Linear endpoint interpolation?** Yes: each mapped vertex is
  `Vector2.Lerp(Near_i, Far_i, t)`; each lane center is the average of its
  two adjacent vertices.
- **Is `t` linear in simulation depth?** Yes for ordinary enemy/shot heights:
  `t=1-height/400`. Entity height is an along-tube model coordinate, not a
  projected screen coordinate.
- **Direction and endpoints?** Yes: `t=0` near/rim, `t=1` far/deep. Enemy
  heights increasing move toward smaller `t`; Blaster shot height decreasing
  moves toward larger `t`.
- **Consistent height conversion?** Not for every class. Most mapped enemies,
  enemy bullets, Flippers, shots, and Spike tips use a one-to-one height
  conversion. Eggs multiply height by 20. A starting Spike has independent
  base and tip depths. The Blaster uses player depth rather than enemy height.
- **Height versus projected depth?** Both occur, but all paths listed above
  reduce to a normalized `t` before lane interpolation, except the optional
  non-lane Fuseball/Pulsar paths that still use the Atari reciprocal
  `ProjectWorldPoint` route.
- **Height multiplier?** Yes, Eggs use `height * 20`; this creates `t>1`.
- **Far-set translation?** Yes, baked into the far endpoints before common
  centering. It changes each lane’s fixed `D_lane` and thus fixed lane speed,
  not the depth derivative.
- **Anisotropic gain?** It affects vector lengths and angles between lanes,
  not speed variation along one fixed lane.
- **Final camera?** It is a fixed affine, uniform `1.26` zoom with zero
  rotation. It multiplies all displacements by the same constant and cannot
  introduce depth-dependent acceleration.
- **Reciprocal functions still active?** Yes for non-lane `ProjectWorldPoint`
  callers (including optional Fuseball/Pulsar rendering) and legacy/previous
  paths. The registered Level 7 production anchors listed above bypass them.
- **Was nonlinear progression replaced?** Yes for the registered anchors:
  their prior point projection had reciprocal progression; the current
  record-06 adapter uses linear paired endpoint interpolation.
- **What space calibrated gameplay speed?** The source increments height in
  Level depth units per simulation step. Neither current code nor this
  investigation establishes pixel-per-tick gameplay calibration.
- **Does an Atari-style straight rail require nonlinear interpolation?**
  Not from straightness alone. Straight endpoints allow linear or nonlinear
  travel. Record-06 projection and the prior renderer are not sufficient
  evidence to impose one as historically accurate entity motion.
- **Can lane length/orientation explain the symptoms?** They explain
  lane-dependent constant displacement and direction. They cannot explain
  changing displacement with depth on one fixed lane.
- **Are normalized increments constant?** Within a state with a fixed
  height increment, yes. Tanker/Spiker-building `delta t=-.004375`,
  Flipper `-.005`, enemy bullet `-.0075`, and Blaster shot `+.015`.
  Spiker retreat uses `delta t=+.0039375` at default Level 7 speed. Egg
  movement is intermittent and uses its `x20` conversion.
- **Can truncation, clamping, extrapolation, or transitions matter?** The
  adapter itself has no depth clamp or integer truncation. Production state
  updates can clamp Flipper height at the rim, change Spiker state/speed,
  pause or pivot Flippers, remove shots/bullets at endpoints, and update Egg
  vertical position only on every eighth tick. Enemy retraction adds a
  render-only offset and clamps its projected depth. These cause boundaries,
  pauses, reversals, or state-dependent increments, not a smooth depth curve
  in the lane adapter.
- **Where could constant motion become non-uniform?** For a fixed lane and
  fixed simulation increment, nowhere in the current registered anchor path:
  the interpolation, presentation map, and camera are affine. Non-uniform
  samples require a changing simulation increment/state, an alternate
  reciprocal render path, lane changes, glyph-size cues, or another runtime
  path not represented by this controlled probe.

The manual pair of trends would qualitatively require a transfer whose
visible derivative grows with `t` (faster toward far, slower toward near).
The current linear transfer has constant derivative; the previous reciprocal
transfer has a decreasing derivative. No code change or specific nonlinear
formula is supported by this discrepancy.

## Rim-motion control and Flipper crash-fix separation

During ordinary play `Player.Depth == Level.Depth`, so
`PlayerProjectedDepth == Position` and the Blaster maps at `t=0`.
`GetPlayerEdge` obtains the mapped near-edge endpoints, then the Blaster’s
`EdgePosition` places it linearly along that rim edge. Changes to the player’s
edge coordinate therefore bypass depth interpolation.

A non-transition Flipper at `Height == Level.Depth` also maps to `t=0`.
Flipper lane pivots use `MapFlipperTransition` at the Flipper’s normalized
depth; the crash correction changes the pivot’s mapped angular path, not its
depth normalization or ordinary fixed-lane position. The manual rim-motion
control is consistent with depth mapping being a distinct path, but it does
not establish what depth transfer is arcade-correct.

## Recording and playback

Visible deterministic playback can be used later without changing the
gameplay recording format. `GameplayRecordingFile` version 2 stores startup
configuration and recorded player input frames; `GameplayPlaybackSession`
replays those frames through `TempestGame.RunOneStep`. The render-only adapter
reads the resulting game state and does not write simulation or recording
state. The Level 7 presentation tests also assert render-state preservation.

Validation run for this investigation:

- `dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter FullyQualifiedName~AtariLevel7EntityPresentationTests --verbosity minimal`
  — **PASS**, 15 passed.
- `dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter FullyQualifiedName~GameplayRecordingTests --verbosity minimal`
  — **PASS**, 51 passed.

No recording format or playback code was changed.

## Conclusion and handoff

There is not enough evidence for a bounded implementation prompt. The
smallest remaining uncertainty is the exact runtime state sequence for the
reported objects: whether the observed entities were on the registered
record-06 lane-center path in ordinary `Playing`, and their actual
per-tick height changes at the compared depths. The current source-level
deterministic probe does not show the reported trend, and neither the old
reciprocal transfer nor a generic perspective correction is a justified
replacement.

No gameplay speed, timing, height increment, collision, RNG, score, lives,
recording/playback behavior, source, tests, geometry, projection, or
presentation code changed. No protected image/video material was inspected.
The existing candidate’s manual presentation acceptance remains pending.

If a later manual check is requested, the task-appropriate command is:

```powershell
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation --level 7
```

The interactive game was not launched in this investigation.

## Final preservation check

At completion, branch, HEAD, upstream, index, pre-existing tracked changes,
and pre-existing untracked paths were unchanged. Only the required paired
files under ignored `AI-Logs/` were written; neither was staged or committed.

- Prompt: `AI-Logs/Prompts/2026-11-16-level7-depth-to-visible-motion-investigation-prompt.md`
- Report: `AI-Logs/Reports/2026-11-16-level7-depth-to-visible-motion-investigation-report.md`
