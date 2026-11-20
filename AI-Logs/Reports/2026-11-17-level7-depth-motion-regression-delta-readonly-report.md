# Level 7 depth-motion regression-delta investigation

Date: 2026-11-17
Status: **READ-ONLY INVESTIGATION COMPLETE; REPORTED MOTION TREND UNRESOLVED**

## Direct answers

1. **Regression boundary:** the latest render-only Level 7 registration
   milestone in `2026-11-16-level7-render-registration-and-centering-implementation-report.md`.
   It added `AtariLevel7EntityPresentation` and routed ordinary Level 7
   `Playing` entities in `VectorRenderer` from production-clover model-point
   projection through `AtariPresentationAdapter.Project` to record-06 mapped
   lane/depth interpolation. The new path also changed entity local scale from
   the adapter's reciprocal Jacobian to mapped lane-width / near-width.
   The registered `AtariLevel7Presentation` tube and entity routing are
   render-only. This is the narrowest boundary supported by the historical
   reports and current worktree; the source was uncommitted at the same HEAD,
   so no committed revision marks the transition.
2. **Why the preceding candidate did not exhibit it:** before registration,
   entities remained on the production C# clover and used the pre-existing
   reciprocal Level-7 adapter when Atari presentation was enabled. The tube
   alone used record 06. That path had different coordinates, orientation,
   and depth derivative; it did not use the record-06 entity lane mapper.
3. **Why reported enemies slow toward the rim / shots accelerate toward the
   deep end:** a deterministic production replay, projected twice from each
   identical state, does **not** show either trend in current fixed-lane
   anchors. The current record-06 anchor is affine in normalized depth, so
   actual constant-step Tanker, Spiker-building, and shot segments have
   constant per-tick displacement. Scale and glyph extrema do not account
   for the reported large trend. This directly conflicts with the strong
   manual regression evidence; this investigation cannot explain the
   discrepancy by the current registered anchor formula.
4. **Why rim motion remains normal:** player rim movement uses mapped near-edge
   endpoints at `t=0` and `EdgePosition`, not the depth interpolation.
   Non-transition Flippers at the rim also use `t=0`; rim pivots use the
   separate mapped transition path. These bypass the along-tube depth
   progression implicated by the report.
5. **Simulation:** no simulation change is implicated or evidenced at this
   boundary. The registration changes are renderer/mapping changes. Existing
   entity updates still produce the actual replayed height increments shown
   below.

**Classification:** **PRESENTATION POSITION REGRESSION + PRESENTATION SCALE
REGRESSION; reported depth-speed shape UNRESOLVED.** No evidence supports a
**SIMULATION REGRESSION**.

## Evidence and exact before/after path

Historical reports establish the boundary:

- The Nov. 15 adapter implementation report describes the opt-in Level 7
  render path, its `ProjectWorldPoint` selection seam, reciprocal-depth
  equation, local Jacobian, and read-only treatment of gameplay state.
- The Nov. 16 entity-registration investigation explicitly records the
  pre-registration split: the visible tube used record 06; Level 7 objects
  still used `ProjectWorldPoint` / `AtariPresentationAdapter.Project` on
  `LevelDefinition.Points`, with the common playfield camera.
- The Nov. 16 registration implementation report records the new
  `AtariLevel7EntityPresentation` and renderer routing, and states simulation,
  collision, recording/playback, and record-06 tube transform were unchanged.
- The depth-to-visible-motion report established only fixed-depth-step
  behavior; it did not capture production trajectories. It found the current
  affine transfer constant-speed and the old reciprocal transfer fastest near
  the rim, slowest at the deep end.
- The mapped-Flipper crash-fix report says it changed mapped transition
  geometry/tests only and explicitly excludes lane/depth registration,
  non-Flipper entities, simulation, timing, and `VectorRenderer`. Current
  registration/depth code remains separate. The crash fix is preserved and
  not reopened.

The old path below is supported by those pre-registration reports; it is not
inferred merely from the adapter's existence:

```text
gameplay lane/edge -> adjacent production C# clover points
  -> midpoint/point plus projected depth (Level 7 units)
  -> AtariPresentationAdapter.Project(7, x, y, z, ...)
  -> common experimental portrait playfield camera
```

In the opt-in Level 7 adapter:

```text
t = (z - position) / depthRange
  = (z - 50) / 400
  = 1 - height / 400
q = 16 + 224*t
d = q + ScaleRaw = 40 + 224*t       (record-06 metadata ScaleRaw = 24)
R(a,t) = 256*a / d
screen = fixed logical baseline + fixed logical gain * R(...)
localScale = fixed logical factor / (unitsPerRawUnit * d)
```

For entities centered on a lane, the old `a` coordinates came from the
production clover lane midpoint. The fixed camera was applied once after
projection. Depth direction was `t=1` at the far/deep end and `t=0` at the
near/rim. The record metadata, Level-7 model-frame units, and adapter path were
present before entity registration.

Current registered path:

```text
gameplay lane i; projectedDepth = position + depthRange - height
t = (projectedDepth - position) / depthRange
V_i(t) = NearRecord06_i + t * (FarRecord06_i - NearRecord06_i)
laneCenter_i(t) = (V_i(t) + V_(i+1)(t)) / 2
localScale = length(V_(i+1)(t) - V_i(t)) /
             length(NearRecord06_(i+1) - NearRecord06_i)
  -> common experimental portrait playfield camera, once
```

The mapped endpoints already include the existing record projector, visual
gains, far-set translation, and shared centering. The lane correspondence is
identity (`i -> i`), explicitly a **PROJECT TRANSLATION** candidate, not an
established Atari correspondence. `t` is not clamped. Ordinary lane anchors
use the paired mapped record endpoints; Flipper pivots use the separate
mapped-hinge transition function.

This boundary therefore changes **anchor position** from a nonlinear
projection of production geometry to a linear interpolation of mapped
record-06 lanes. It separately changes **glyph scale** from the old reciprocal
adapter Jacobian to a lane-width ratio. It also changes lane orientation:
old projected production lanes and new record-06 lane directions differ by
about 145–176 degrees in the examined lanes. It does not apply the adapter
twice, reverse the supplied height convention, or apply the camera twice.
Record lane lengths differ substantially, so lane changes can change
per-tick speed; this cannot create depth acceleration on a fixed lane.

The selected candidate controls visible record-06 tube rendering and the
registered entities in ordinary `Playing`. Approach/exit and non-Level-7
states preserve their separate paths. `VectorRenderer.ProjectLane`,
`ProjectShot`, `ProjectFlipperEndpoints`, and `GetPlayerEdge` take the new
path; Tankers, Spikers, both shot types, spikes, effects, and lane geometry use
the mapped lane helpers. Tanker, Spiker, shot, and enemy-shot artwork remains
screen-oriented; the Flipper uses mapped edge orientation. No second world or
camera transform follows the mapper.

## Same-state production playback

The analysis used the existing
`TEMP/GameplayRecordings/level7-candidate-6.json` (**read-only**): v2
production scenario, starting Level 7, 20,325 recorded input ticks, seed
879844810. A bounded external harness replayed the production
`GameplayPlaybackSession` once and projected each captured entity state both
ways. No gameplay was replayed under alternate simulation rules, and no
recording or source/test file was modified. Host coordinates apply the same
1.26x playfield camera to both projections. Object IDs below are stable within
this replay; `I#` distinguishes successive runtime instances where gameplay
reuses an original/cargo slot.

Representative complete or contiguous trajectories:

| Object / identity | Replay ticks | State, lane and depth range | Lifecycle / transition notes |
|---|---:|---|---|
| Tanker `T6I3` | 327–540 | Climbing, lane 10, height 11.75→384.5, `t=.970625→.03875` | No lane/state changes; disappears at tick 541 as its release threshold is crossed. |
| Player shot `S26` | 1851–1916 | Traveling, lane 13, height 394→4, `t=.015→.99` | First visible state is one update after spawn; removed at tick 1917 after height becomes negative. Other shots follow the same recorded path from their own first visible tick. |
| Enemy bullet `B1` | 58–190 | Traveling, lane 3, height 1.75→397.75, `t=.995625→.005625` | No lane/state changes; removed at tick 191 after passing the rim. |
| Spiker `SP3I41` | 6295–6522 | One Building ascent, lane 11, height 0→397.25, `t=1→.006875` | Same runtime Spiker also has separate retreat/build cycles. Building→Retreating occurs after the ascent; cycles were not merged as continuous ascent. |
| Tanker-released Flipper `FC4I1` | 182–1226 | Cargo Flipper, height 197.25→400; 34 lanes encountered | At least 33 lane changes and repeated Flipping/Pausing transitions; stopped at player death/reset. Do not interpret its pivot/lane motion as a single depth trajectory. |
| Ordinary Flipper fixture `FO-FIXTURE-I1` | 0–213 | Actual `Flipper.Update` with `Level7Production`, lane 7→8→…; height 0→400 | A production Level-7 ordinary Flipper has a two-unit lead-in, then its programmed route interleaves climbing and pivots. It reaches the rim; long straight climbing cannot be isolated in this route. This deterministic fixture is not mislabeled as a recorded object. |

The recording contains many real Tankers, Spikers, bullets, cargo Flippers,
and shots. These same-state samples cover the requested entity classes except
that no ordinary non-cargo Flipper exists in the selected replay; its
production-faithful fixture is reported separately. State and lane changes
were retained as events, not included in fixed-lane motion-rate averages.

### Same-state coordinates and anchor displacement

All coordinates below are host pixels; scale is local logical glyph scale
before the common camera. Values are rounded. Each old/new row is the same
object state at that tick.

| Object / tick | State; lane; height; `t` | Old host anchor | New host anchor | Old / new scale |
|---|---|---:|---:|---:|
| Tanker / 327 | Climbing; 10; 11.75; .970625 | (673.39, 238.71) | (613.26, 626.20) | .1978 / .1759 |
| Tanker / 435 | Climbing; 10; 200.75; .498125 | (696.71, 247.81) | (549.88, 614.28) | .3359 / .5770 |
| Tanker / 540 | Climbing; 10; 384.5; .03875 | (816.57, 294.59) | (488.26, 602.68) | 1.0458 / .9671 |
| Shot / 1851 | Traveling; 13; 394; .015 | (672.55, 468.73) | (511.19, 286.43) | 1.1741 / .9873 |
| Shot / 1883 | Traveling; 13; 202; .495 | (649.35, 295.53) | (564.87, 428.85) | .3374 / .5798 |
| Shot / 1916 | Traveling; 13; 4; .99 | (645.39, 265.94) | (620.23, 575.72) | .1945 / .1596 |
| Enemy bullet / 58 | Traveling; 3; 1.75; .995625 | (598.66, 208.19) | (665.10, 626.83) | .1936 / .1562 |
| Enemy bullet / 124 | Traveling; 3; 199.75; .500625 | (568.54, 195.45) | (730.57, 614.34) | .3346 / .5757 |
| Enemy bullet / 190 | Traveling; 3; 397.75; .005625 | (376.48, 114.20) | (796.03, 601.85) | 1.2339 / .9952 |
| Spiker / 6295 | Building; 11; 0; 1 | (681.18, 257.83) | (617.20, 615.21) | .1928 / .1509 |
| Spiker / 6409 | Building; 11; 199.5; .50125 | (711.40, 281.41) | (550.30, 569.66) | .3343 / .5744 |
| Spiker / 6522 | Building; 11; 397.25; .006875 | (901.74, 429.97) | (483.99, 524.52) | 1.2255 / .9942 |

For a fixed lane and unchanged state, the old path's anchor displacement
increases strongly toward the rim; the current path's displacement stays
constant. Per-tick host-pixel measurements:

| Trajectory / direction | Depth band (`t`) | Height delta | Old anchor / tick | New anchor / tick | New glyph leading-edge / tick |
|---|---:|---:|---:|---:|---:|
| Tanker lane 10, climbing | deep `t>=.8` | +1.75 | .160 (range .137–.187) | .597 (range .597–.597) | .717 |
|  | middle `.2<t<.8` | +1.75 | .486 (range .189–1.239) | .597 | .717 |
|  | near `t<=.2` | +1.75 | 2.187 (range 1.268–3.741) | .597 | .717 |
| Shot lane 13, toward deep end | deep `t>=.8` | -6 | .626 (range .528–.740) | 4.756 (range 4.756–4.756) | 4.691 |
|  | middle `.2<t<.8` | -6 | 1.958 (range .763–4.905) | 4.756 | 4.691 |
|  | near `t<=.2` | -6 | 9.846 (range 5.315–17.636) | 4.756 | 4.691 |
| Enemy bullet lane 3, toward rim | deep `t>=.8` | +3 | .344 (range .289–.409) | 1.010 (range 1.010–1.010) | 1.032 |
|  | middle `.2<t<.8` | +3 | 1.065 (range .416–2.696) | 1.010 | 1.044 |
|  | near `t<=.2` | +3 | 5.659 (range 2.804–11.195) | 1.010 | 1.118 |
| Spiker lane 11, Building only | deep `t>=.8` | usually +1.75 | .233 | .710 | .768 |
|  | middle `.2<t<.8` | usually +1.75 | .718 | .710 | .790 |
|  | near `t<=.2` | usually +1.75 | 3.800 | .710 | .670 |

The Spiker's longest stationary-lane building segment reaches the rim region;
its normal state reversal and later restart are separate. Spiker rotation
advances 45 degrees per update, so its extremal point moves with animation
independently of its anchor.

Five-/ten-tick fixed-state windows reinforce the result: Tanker new-anchor
totals are 2.986 / 5.972 pixels throughout the sampled depth; shot totals are
23.781 / 47.563 pixels throughout. The corresponding old anchor totals vary
with depth. The actual current presentation therefore does not exhibit
fixed-lane anchor deceleration for an enemy or acceleration for a shot.

### Glyph scale and visual extrema

Current lane 10's Tanker scale changes from about `.176` at deep depth to
`.967` near the rim, while its mapped anchor moves `.597` host pixels per
tick. The Tanker glyph size is `26*localScale+1`; its mapped leading-extremity
advance is approximately `.717` pixel/tick, also effectively constant with
depth. The old adapter scale grows from about `.198` to `1.046`; it does not
make the current path appear to slow near the rim.

For shot `S26`, current local scale falls from `.987` to `.160` as it recedes;
its maximum rendered point radius correspondingly falls from approximately
`4.44` to `.72` logical units (host scale included: `5.59` to `.91` pixels).
Despite that substantial size change, the computed point-program leading
extremity moves about `4.691` pixels/tick versus `4.756` anchor pixels/tick
throughout the lane-13 trajectory. The scale change cannot account for
slow-at-spawn followed by strong acceleration toward the deep end. It makes
the shot shrink, but does not produce the reported speed curve.

Flipper glyphs use the mapped lane width and orientation; the selected cargo
track spends most time in state pauses/pivots and changes lanes repeatedly.
Its visual extremes are therefore dominated by pivot/lane transitions, not
by depth. Spiker extrema also vary with its 45-degree rotation. Neither is
evidence of a smooth fixed-lane depth-speed trend. The player shot's spawn
state confirms `Height=394` and `t=.015` at its first visible tick: gameplay
creates it at player height 400, then `Shot.Update` subtracts 6 before the
renderer next observes it. There is no extra stay-near-rim interval or
depth-normalization discontinuity.

### Lane, state, and render-path alternatives

- **Variable simulation increments:** Tanker and representative shot segments
  have exactly constant `+1.75` and `-6` deltas. Enemy bullet is `+3`.
  Spiker Building uses its production speed (with the normal state boundary
  handled separately). Changes at state boundaries exist, but do not cause a
  within-state smooth curve.
- **Lane transitions:** fixed-lane Tanker, shot, bullet and the Spiker Building
  segment establish that lane transitions are not required for the mismatch.
  Across all record-06 lanes, mapped rail lengths range from `100.381` logical
  units (lane 6) to `251.654` (lane 13), a 2.51x span. A lane change can
  therefore make speed and direction jump; it is not the reported gradual
  single-lane trend.
- **Depth units/endpoints:** the replayed state feeds `projectedDepth =
  50+400-height`; normalization yields `t=1-height/400`. This agrees with
  implementation tests and the documented near (`t=0`) / far (`t=1`)
  endpoint meanings. No reversed height, `t`, or position argument was found.
- **Glyph scale/orientation:** scale varies strongly with depth, but captured
  Tanker and shot extrema do not reproduce the reported magnitude/direction.
  Screen-oriented shot art and Tanker art do not rotate with the rail. The
  Spiker's animated long-feature extremity is not a clean measure of anchor
  speed.
- **Additional transform/camera:** there is one common affine playfield camera
  after each path, not a second adapter or duplicated camera. The new lane
  interpolation is the first changed anchor stage; map endpoints already
  contain the accepted visual transforms.
- **Manual source/run mismatch:** the exact binary Neal manually ran was not
  available for inspection; the source was uncommitted. A mismatch between
  that run and this current source remains possible, but is not established.
- **Uncaptured presentation effects:** the headless dual-projection sequence
  excludes rasterization/perceptual observation, but it includes the
  production entity updates, state, camera, scale and vector extrema. It
  cannot explain away the supplied manual observation.

## Rim control, crash fix, and simulation evidence

`GetPlayerEdge` selects the record-06 lane at `PlayerProjectedDepth`, which is
the near/rim position during normal play. The Blaster is placed by
`EdgePosition` on this mapped near edge; it bypasses `t` progression.
Non-transition Flippers use the same near endpoint lane when at height 400.
Their rim pivots use `MapFlipperTransition` rather than the lane-depth
transfer. This accounts for the control remaining outside the along-tube
depth path, while acknowledging the registration changes the visible rim
geometry itself.

The crash-fix report and current call sites separate transition geometry from
normal lane-depth mapping. No crash-fix files were reverted or altered.

The boundary adds no calls to or changes in simulation update methods.
`TempestGame.RunOneStep`, `Shot.Update`, `EnemyBullet.Advance`, Tanker and
Spiker update routines, and Flipper state updates produced the captured
increments. The registration reads those states and returns presentation
coordinates/scales; it writes no gameplay state. This is evidence against a
simulation regression, not proof about an unavailable manually run binary.

## Regression classification and future direction

The **presentation position** changed at the exact registration boundary:
the old production-point reciprocal path was replaced by the current
record-06 mapped interpolation. The **presentation scale** also changed from
the adapter Jacobian to lane-width ratio. Same-state projection shows very
large anchor and direction changes between old and new paths; the registered
rail direction is nearly opposite the old projected direction in the sampled
lanes. These are clear presentation changes at the manual regression
boundary.

However, neither the actual production replay nor its glyph extrema exhibits
the reported paired acceleration/deceleration. The current fixed-lane formula
is affine, and production increments are constant across the measured
trajectories. The previous reciprocal transfer predicts the *opposite*
depth-dependent trend. Therefore the exact cause of the severe perceived
speed curve remains **UNRESOLVED**, despite the boundary association and
strong manual evidence.

Smallest technically justified future direction, **not an implementation
specification**: retain the record-06 anchor registration, current tube and
gameplay, and investigate a render-only nonlinear reparameterization of
normalized depth that preserves both endpoints. The manual trend would require
less visible progression per depth unit near the rim and more near the deep
end (a monotone transfer with smaller derivative near `t=0` and larger near
`t=1`). No coefficient, curve, or implementation is supported by the current
production replay; more evidence must identify the mismatch before writing a
correction prompt. The evidence is **not sufficient for a behavior-fix
implementation prompt**.

## Validation and limitations

- Existing v2 Level 7 recording metadata checked read-only: **PASS**.
- Bounded deterministic production playback and same-state dual projection
  through the full recorded duration: **PASS**. The session-state harness
  compiled against the current project; tracked code was not changed.
- Trajectory and glyph-extrema calculations: **PASS** as analysis of the
  captured source geometry. They do not establish native framebuffer output
  or arcade fidelity.
- No gameplay test suite was rerun because no production code was changed.
- No interactive game was launched. No screenshots, videos, frames, or other
  visual assets were opened. `TEMP/myNotes.md` and raw ROM copies were not
  inspected.
- No correction was implemented. Manual acceptance is not claimed. Neal's
  reported behavior remains the controlling manual evidence, and the
  discrepancy remains unresolved.
- A long ordinary non-cargo Flipper climb was unavailable in the production
  playback: the recorded Level-7 mix contains Tanker cargo Flippers. The
  separate production `Flipper.Update` fixture follows the configured
  2-3-2-2-2 route and necessarily interleaves pivots; this limitation is
  explicit rather than presenting it as an uninterrupted normal climb.

## Git state and preservation

At start: branch `master`; HEAD
`385f603a94391756b2ca6cccd524dc17b281e782`; upstream `origin/master`; empty
index; 17 modified tracked paths; 17 nonignored untracked paths. Those paths
were preserved. No source, tests, configuration, project files, durable
documentation, recordings, protected assets, or Git state were changed.
Only the required ignored AI-Logs prompt and this report were created in the
repository; analysis harness and captures were outside the repository.

The final branch, HEAD, upstream, index, tracked modifications, and
nonignored untracked set were checked and match the initial state. No file was
staged, committed, pushed, reset, restored, stashed, rebased, cleaned, or
checked out.

Prompt: `AI-Logs/Prompts/2026-11-17-level7-depth-motion-regression-delta-readonly-prompt.md`
Report: `AI-Logs/Reports/2026-11-17-level7-depth-motion-regression-delta-readonly-report.md`

Completion token: `TEMPEST_LEVEL7_DEPTH_MOTION_REGRESSION_DELTA_READONLY`
