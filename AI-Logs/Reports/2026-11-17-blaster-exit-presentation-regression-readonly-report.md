# Blaster Exit Presentation Regression — Read-Only Investigation

Date: 2026-11-17

## Status and direct answers

**COMPLETE — root cause established from the renderer, adapter equations, Git
history, and deterministic exit tests.** No source, tests, configuration,
durable documentation, gameplay, presentation, recording, or Git state was
changed. The only permitted writes are this report and its paired prompt under
`AI-Logs/`.

1. **Exact cause:** the new Atari `PlayerClawHeight` branch divides a live
   near-rim local scale by the player-depth local scale. For Level 1 this is
   `20 * [K / (Position + ZOffset)] / [K / (PlayerProjectedDepth + ZOffset)]`,
   which simplifies to
   `20 * (PlayerProjectedDepth + ZOffset) / (Position + ZOffset)`. It is the
   reciprocal of the scale direction required to keep the Blaster proportional
   to its projected edge. As the player travels toward the far end, the
   numerator grows while the near-rim denominator becomes 9 during the
   zoom-out phase.
2. **Why Playing is unaffected:** at the gameplay rim, the renderer returns a
   fixed 20-unit claw height. The accepted outward-normal correction is also
   explicitly limited to `Playing`; it changes orientation, not dimensions.
3. **Why the distortion grows and later contracts:** the wrong exit ratio grows
   monotonically as `PlayerProjectedDepth` increases. On successful exit, the
   game creates the next level, restores the camera, and enters `Approach`.
   That state has a separate but likewise reversed Atari ratio
   (`baselineScale / approachScale`), so the Blaster starts oversized and
   contracts as the 960-unit approach offset eases to zero. At the end,
   `Playing` returns the fixed 20-unit body. The authorized screenshots show
   the growth but not a post-694 recovery frame.
4. **Shared across tubes:** yes. All supported levels enter the same
   `PlayerClawHeight` and `DrawPlayer` path during exit. Level adapters change
   their local Jacobian and coordinates, but the ratio is applied in the
   shared renderer; representative closed, open, accepted, and diagnostic
   levels show the same inverse-scale behavior.
5. **Gameplay/exit simulation:** no relevant exit simulation change was found
   in the Atari-presentation milestone. The Level 1 deterministic exit test
   verifies delay, both zoom phases, depth progression, input/lane behavior,
   and Approach handoff. The regression is in presentation math.
6. **Regression boundary:** the Atari-specific player-scale branch was added
   in `7fb0819cdccddbf0f3e4fbc40409023050546d9d` (2026-11-17 16:43, the
   whole-game Atari-presentation candidate/Level 10 closeout commit). Git
   blame attributes `PlayerClawHeight` lines 170-187 to that change. Its parent
   `385f603a94391756b2ca6cccd524dc17b281e782` has the prior legacy equation.
   The later orientation closeout `34eedf0` only changes the outward normal in
   ordinary `Playing`; its report expressly leaves exit outside its scope.
7. **Implementation evidence:** sufficient for a bounded shared
   presentation-only correction. Preserve the accepted Playing branch and
   camera/tube geometry; normalize the current projected Blaster size against
   a stable gameplay-rim baseline rather than dividing the two live scales.
   The separate Approach and ReverseZoom cases need explicit regression checks
   so a fix does not merely move the visible distortion to a handoff.

## Scope, evidence, and preservation

Manual regression statements in the user prompt are treated as
**MANUAL REGRESSION OBSERVATIONS**, not as evidence of arcade behavior.
The report uses only the source and Git history, the documented test paths, and
the four authorized frames below. It does not derive constants from pixels.
No other images, videos, recordings, screenshots, contact sheets, or protected
ROM files were inspected. `TEMP/myNotes.md` was not accessed.

### Authorized frames inspected

- [playback-level-1-boundary-000607.png](../../UserData/Screenshots/Level-1/playback-level-1-boundary-000607.png)
- [playback-level-1-boundary-000694.png](../../UserData/Screenshots/Level-1/playback-level-1-boundary-000694.png)
- [playback-level-1-boundary-000647.png](../../UserData/Screenshots/Level-1/playback-level-1-boundary-000647.png)
- [playback-level-1-boundary-000623.png](../../UserData/Screenshots/Level-1-boundary-000623.png)

Visual characterization:

- **607:** full tube is visible; yellow Blaster is a small, recognizable rim
  glyph with a plausible edge anchor.
- **623:** the tube remains coherent and the Blaster still appears compact.
- **647:** the tube is zoomed; two Blaster strokes lengthen into a diagonal
  pair while the smaller edge-following parts remain comparatively short.
- **694:** the tube rails and rim remain geometrically coherent, though the
  tube is strongly zoomed/cropped. The Blaster's near-parallel diagonal strokes
  are extremely long; its edge anchor remains near the lane while the outer
  artwork extends far from it.
- The Blaster does **not** lengthen uniformly. Its edge-tangent components
  remain tied to fractions of the projected edge; the large increase is in the
  perpendicular normal displacement. The normal appears diagonal because that
  lane's edge is diagonal. This agrees with the actual renderer construction,
  rather than relying on a pixel-derived orientation.
- The change is continuous across the exit equations. No later contraction is
  shown by these four frames: 623 precedes the malformed 647/694 boundaries
  when the values refer to one recording. The user's separate observation of
  later recovery is consistent with the subsequent Approach scale path below.

Playback screenshot capture names files with the current playback boundary;
`GameplayPlaybackSession` defines boundary 0 as startup and increments the
boundary after each consumed frame. Thus increasing boundary IDs establish
simulation order within one recording, but not the order in which the user
captured the images. The 607 and 623 frames display a total of 17,145 ticks;
the exact recording path and common recording identity for all four frames
could not be established without searching protected recording collections.
Consequently, the exact game state of each supplied frame is **UNKNOWN**;
the numerical state trajectory below comes from the deterministic production
exit path, not an assumed mapping from screenshot IDs.

## Regression boundary and last-known-good path

`git status` at start showed:

- Branch `master`, HEAD
  `34eedf0cf996cfe660494e34bdff2a1499696068`.
- Upstream `origin/master`, 0 ahead / 0 behind.
- Empty index.
- Pre-existing tracked modification: `.github/copilot-instructions.md`.
- Pre-existing untracked items: `Reference/arcade/MBUCOD.V05.txt`,
  `Reference/arcade/MBUDOC.DOC`, and `screenshot000.png`.

All were preserved.

The Atari presentation code was introduced in the large change committed as
`7fb0819`; `VectorRenderer.PlayerClawHeight`'s Atari branch is attributed to
that commit. The previous tracked implementation is available at its parent
without changing the worktree. Its exit formula was:

```text
20 * ((Level.Position + 5) / 70)
   / ((PlayerProjectedDepth + Projection.ZOffset) / Projection.K)
```

For Level 1, this is equivalent to multiplying the current projected
player-depth scale by the current camera factor relative to the stable
`K=70`, `ZOffset=5` rim calibration. It makes the player glyph track the
projected camera/edge scale during exit. In the new Atari path,
`GetAtariLocalScale` correctly returns the adapter's local Jacobian, but
`PlayerClawHeight` uses `nearScale / playerScale` instead of normalizing the
player scale against that stable reference. The adapter's Level 1 projection
itself has the expected local Jacobian `K / (depth + ZOffset)`.

The later `34eedf0` orientation commit does not change `PlayerClawHeight`.
Its conditional is limited to `atariPresentationEnabled && State ==
Playing`; the closeout also explicitly says exit/Approach remain outside that
orientation correction. It is not the regression source.

## Current rendering equations and branch differences

Relevant source: [VectorRenderer.cs:168](../../src/Tempest.Game/Rendering/VectorRenderer.cs#L168)
and [AtariPresentationAdapter.cs](../../src/Tempest.Game/Rendering/AtariPresentationAdapter.cs#L300).

- `PlayerProjectedDepth = Position + Depth - Player.Depth`. For Level 1 this
  begins at 50 and increases toward 450 as `Player.Depth` decreases from 400
  toward 0.
- In Atari `Playing`, `PlayerClawHeight` returns 20 because
  `Player.Depth == Level.Depth`. Both rim endpoints are projected at
  `PlayerProjectedDepth`; the Blaster anchor is interpolated along that same
  projected edge. Only `Playing` invokes the outward-normal selector.
- In `ExitZoom`, `ApproachDepthOffset` is zero. Both edge endpoints use the
  same `PlayerProjectedDepth`; there is no mixed-depth endpoint pair. The
  tangent is computed from the projected edge and normalized before deriving
  its perpendicular. The anchor remains `p2 - edgeVector * EdgePosition`.
  The six line segments use fractions of the projected edge for their
  tangential coordinates and `PlayerClawHeight` for their normal displacement.
  `DrawPlayer` does not apply the adapter's point projection a second time to
  those already-projected artwork endpoints.
- During exit, the `Playing` outward-normal branch is not taken. The default
  perpendicular may differ in sign, but it has unit length and cannot explain
  the large magnitude.
- `DrawTube` projects all near-rim points at the configured near depth and all
  far-rim points at the configured far depth, then joins corresponding rails.
  Those are intentional, consistent per-rim depths. The animated camera
  changes their overall size, but does not introduce the Blaster-only local
  offset.
- The Atari adapter's Level 1 screen translation (+122), Level 8/9 accepted
  vertical placements, Level 10 +50 placement, `Projection.PortraitPerspective`
  (default 0 for this mode), and the portrait host camera are not the
  root cause. The host camera applies one uniform 1.26 scale to both tube and
  Blaster geometry.

The actual faulty Level 1 calculation is:

```text
nearScale   = K / (Position + ZOffset)
playerScale = K / (PlayerProjectedDepth + ZOffset)
height      = 20 * nearScale / playerScale
            = 20 * (PlayerProjectedDepth + ZOffset)
                  / (Position + ZOffset)
```

It uses the live near denominator `Position + ZOffset`, which reaches 9 at
`ZOffset=-41`, and reverses the intended depth response. `K` cancels out of
the height even as it is increased by the exit zoom; the projected tube and
edge still respond to `K`. This is not a tangent, anchor, normal-normalization,
edge-projection, or second-projection failure.

### Approach and reverse-zoom handoff

At successful exit, [`CompleteSuccessfulExit`](../../src/Tempest.Game/TempestGame.cs#L2384)
initializes the next level, creates `LevelApproach`, restores `K=70` and
`ZOffset=5`, resets the Blaster to the new rim, and enters `Approach`.
`ApproachDepthOffset` then starts at 960 and eases to zero over 181 ticks;
`ApproachVerticalOffset` starts at 96.
The Atari branch uses `baselineScale / approachScale`, again opposite to the
legacy path. On Level 1 this gives a calculated initial Approach claw height
of 369.09 logical units, which contracts to 20 by the end. The previous
non-Atari equation gives about 1.08 at the initial far offset and scales up
with the approaching tube. This is a related presentation discrepancy and
must not be mistaken for contraction during the preceding Level 1 `ExitZoom`.

`ReverseZoom` is not part of ordinary Level 1 exit. It is reachable after a
Spike collision on spike-exit configurations. The same Atari exit-scale branch
is used while the player remains below the rim, so it merits a bounded
regression check if the future correction changes shared exit scaling.

## Deterministic Level 1 exit trajectory

Basis: Level 1 (`CenterY=400`, `YOffset=-80`), starting lane 7 at
`EdgePosition=0.5`, no lane movement, Atari presentation, `K=70`,
`ZOffset=5`, portrait perspective 0. Exit ticks
below count calls to the existing `ExitZoom` update after the 20-tick delay
has transitioned into `ExitZoom`. The existing production test confirms the
state/depth/camera cadence and Approach handoff. Values below are bounded
evaluation of the current production equations using Level 1's actual
`LevelLoader` points; they are not fitted from screenshots.

At t=23, the 23rd exit update reaches `ZOffset=-41`. At t=24 the second camera
phase starts. After t=123, Player.Depth is 11; the next update crosses below
10 and hands off.

| Exit tick | State | Player.Depth / projected depth | K / ZOffset | Player denominator / near denominator | Local scale | Current / legacy claw height | Projected lane-7 edge length | Height / edge |
|---:|---|---:|---:|---:|---:|---:|---:|---:|
| 0 | Playing | 400 / 50 | 70 / 5 | 55 / 55 | 1.2727 | 20.00 / 20.00 | 98.85 | 0.20 |
| 1 | ExitZoom | 397 / 53 | 70 / 3 | 56 / 53 | 1.2500 | 21.13 / 19.64 | 97.08 | 0.22 |
| 23 | ExitZoom | 331 / 119 | 70 / -41 | 78 / 9 | 0.8974 | 173.33 / 14.10 | 69.70 | 2.49 |
| 24 | ExitZoom | 327.8 / 122.2 | 72 / -41 | 81.2 / 9 | 0.8867 | 180.44 / 13.93 | 68.88 | 2.62 |
| 60 | ExitZoom | 212.6 / 237.4 | 188 / -41 | 196.4 / 9 | 0.9572 | 436.44 / 15.04 | 74.34 | 5.87 |
| 100 | ExitZoom | 84.6 / 365.4 | 348 / -41 | 324.4 / 9 | 1.0728 | 720.89 / 16.86 | 83.32 | 8.65 |
| 123 | ExitZoom | 11 / 439 | 440 / -41 | 398 / 9 | 1.1055 | 884.44 / 17.37 | 85.86 | 10.30 |

The current height rises by about 7.11 logical units per tick after the camera
enters its second phase (`ZOffset=-41`, Player.Depth decrement 3.2). The legacy
comparison remains a compact edge-relative value. At t=123 the current height
is about 51 times the legacy value.

The level-1 lane-7 edge tangent is `(-0.2060, -0.9785)` after projection. The
default exit normal is its perpendicular `(0.9785, -0.2060)`; ordinary
`Playing` flips that normal outward without changing its magnitude. Projected
edge endpoints A/B, logical anchor, host-space Blaster envelope, and near-rim
host diameter:

| Tick | Projected edge A → B (logical) | Anchor (host) | Blaster host bounds W x H | Near rim / tube envelope (host) | Max Blaster extent / tube width |
|---:|---|---:|---:|---:|---:|
| 0 | (165.82, 516.91) → (145.45, 420.18) | (332.10, 550.65) | 32.36 x 73.13 | 641.45 | 11.4% |
| 1 | (170.00, 517.00) → (150.00, 422.00) | (337.60, 551.85) | 33.62 x 71.82 | 665.66 | 10.8% |
| 23 | (234.87, 518.41) → (220.51, 450.21) | (422.89, 570.51) | 219.14 x 70.77 | 3,920 | 5.6% |
| 60 | (223.87, 518.17) → (208.55, 445.42) | (408.43, 567.34) | 543.91 x 140.79 | 10,528 | 5.2% |
| 100 | (202.61, 517.71) → (185.45, 436.18) | (380.48, 561.23) | 895.32 x 217.94 | 19,488 | 4.6% |
| 123 | (196.58, 517.58) → (178.89, 433.56) | (372.55, 559.50) | 1,097.18 x 261.34 | 24,640 | 4.5% |

Host bounds use the current experimental-portrait camera: zoom 1.26, target
approximately `(400, 412.476)`, offset `(640, 480)`. Its playfield viewport is
720 pixels wide. At t=100 and t=123 the Blaster envelope is wider than that
viewport even though the mathematically projected near tube rim is much
larger and cropped far outside the viewport. The anchor remains in the
projected lane; the outer artwork is what escapes it. Tube near-rim dimensions
grow coherently with the shared camera, whereas the Blaster's radial/normal
offset grows independently. The decreasing percentage of the enormous
uncropped tube envelope therefore does not indicate a correct Blaster scale.

### Bounded shared-level comparison

All rows use the same exit ticks and shared renderer. The projection scale
figures follow `GetAtariLocalScale`; values are logical units. Level 4 uses
the same direct perspective Jacobian as Level 1. Levels 8-10 use their
metadata-backed reciprocal-depth Jacobian. The Level 7 custom entity/tube path
is selected only in `Playing`, not during `ExitZoom`.

| Level / geometry | Exit policy before zoom | Atari local-scale path | Claw height at t=23 | t=60 | t=123 |
|---|---|---|---:|---:|---:|
| 1 / simple closed circle | Standard | `K / (depth + ZOffset)` | 173.33 | 436.44 | 884.44 |
| 4 / nontrivial enclosed binoculars | AvoidSpikes, then shared ExitZoom | `K / (depth + ZOffset)` | 173.33 | 436.44 | 884.44 |
| 8 / accepted open tube | Standard | metadata `ScaleRaw=0x18` reciprocal Jacobian | 39.32 | 72.47 | 128.92 |
| 9 / accepted open tube | Standard | metadata `ScaleRaw=0x0C` reciprocal Jacobian | 47.60 | 94.96 | 175.60 |
| 10 / diagnostic open tube | AvoidSpikes policy, then shared ExitZoom | metadata `ScaleRaw=0x18` reciprocal Jacobian | 39.32 | 72.47 | 128.92 |

This is not per-level geometry tuning: the player-scale ratio is shared.
The absolute values differ because the accepted Level 8/9/10 adapter Jacobian
uses the corresponding raw scale metadata. The level-specific presentations
and geometry remain untouched.

## Tube, camera, and alternative-cause findings

- **Anchor:** remains the interpolation of the same lane's projected endpoints;
  no anchor drift is needed to produce the observed stretch.
- **Tangent/normal:** tangent is the exact projected edge difference and is
  normalized; the perpendicular is unit length. The wrong scalar multiplies
  that normal displacement. The result is chiefly normal-direction growth,
  not tangent growth.
- **Projected edge:** A and B use one consistent `PlayerProjectedDepth`.
  Their length follows the adapter projection. No inconsistent endpoint
  depths were found.
- **Tube:** near and far rims are each internally coherent and their rails
  connect corresponding points. The active `K`/`ZOffset` zooms/crops the tube,
  but the Blaster is uniquely malformed because its local artwork offset is
  derived from the reciprocal scale ratio.
- **Not causes:** the lane anchor, edge projection, endpoint depth, local
  tangent normalization, portrait placement, default portrait perspective,
  display camera, level placements, and the accepted Playing outward-normal
  correction. Approach offsets are zero during ExitZoom; they become relevant
  only after the handoff.
- **Denominators:** Level 1's smallest active near denominator is 9, so this
  trajectory has no singularity or invalid return. The defect is a finite,
  steadily amplified ratio, not a divide-by-zero.

## Simulation and recording/playback

`git diff 7fb0819^ 7fb0819 -- TempestGame.cs` changes Level 10 direct-start and
terminal-restart selection only; it does not change `BeginExit`,
`UpdateExitZoom`, `UpdateExitSimulation`, `CompleteSuccessfulExit`, or
`UpdateReverseZoom`. The commit's `GameplayPlaybackSession` change is a
description update for diagnostic recordings, not an input, recording-format,
or stepping change.

The Level 1 [`ExitLaneControlTests`](../../tests/Tempest.Game.Tests/ExitLaneControlTests.cs#L32)
scenario drives production wave completion,
the standard delay, the 23-tick `ZOffset` phase, the `K` phase, and the next
level's Approach. It asserts the expected player-depth, lane, camera, score,
lives, shot, and state invariants. Rendering is read-only with respect to those
simulation values. `GameplayPlaybackSession.StepForward` consumes the same
recorded input through `TempestGame.RunOneStep`; visible playback uses the
same renderer with `atariPresentationEnabled`. Presentation selection is not
stored in the gameplay recording.

Therefore the deterministic renderer will reproduce the same malformed shape
for the same state and camera values in playback, without changing simulation
determinism. The specific 17,145-tick recording associated with the supplied
frames was not identified or opened, so a direct replay comparison of those
exact images is **UNKNOWN**. No recording was created or modified.

## Validation

Test platform: .NET SDK 9.0.304, VSTest, xUnit 2.9.2.

- **PASS:** targeted Blaster, exit-state, Atari vector-renderer, playback,
  recording, and headless-playback tests — 160 passed, 0 failed, 0 skipped.
- **PASS:** Atari presentation adapter and diagnostic Level 10 tests — 49
  passed, 0 failed, 0 skipped.
- **Total:** 209 targeted tests passed.
- **BLOCKED (optional probe, not counted as validation):** loading the .NET 9
  game assembly through the host PowerShell reflection runtime failed because
  `System.Runtime, Version=9.0.0.0` could not be loaded. No runtime assertion
  depended on that probe; bounded values were calculated from the source
  equations and checked against the existing production exit test cadence.
- No interactive game was launched. No test currently asserts Blaster
  dimensions during Atari `ExitZoom` or Approach; existing Blaster coverage is
  an ordinary-Playing control. The numerical trajectory is a source-equation
  evaluation cross-checked against the deterministic exit test's production
  state cadence, not a new renderer instrumentation build.

## Future correction direction and risks

Evidence supports one shared, presentation-only correction. For exit and
approach travel, compare the current projected local Jacobian to the stable
gameplay-rim baseline (the legacy Level 1 relationship is
`20 * currentScale / (70 / (Position + 5))`). Do not merely globally shrink
the Blaster, change the tube/camera/geometry, or tune individual levels.
Simply swapping `nearScale` and `playerScale` would still use live `K` and
`ZOffset` as the baseline and would not match the pre-milestone camera response.

Keep the accepted fixed-size ordinary `Playing` behavior, Level 8/9/10
placements, axis-specific Level 2/5/6 transforms, tube projection, and
gameplay state untouched. Add a renderer-only regression covering Playing,
ExitDelay, both ExitZoom phases, Approach start/end, and a bounded
ReverseZoom case; cover representative closed/open geometries and Level 10's
shared zoom after its AvoidSpikes gate. Neal's manual visual acceptance remains
necessary after any future implementation.

The scale inversion in Approach is proven by the same source branch and
explains a later contraction, but whether a future implementation should
change that separately accepted transition presentation must be explicit in
the implementation scope. No behavior was changed here.

## Final preservation record

The initial branch, HEAD, upstream, ahead/behind, index, tracked modification,
and relevant untracked names are recorded above. Automated tests did not alter
tracked source or Git state. `.github/copilot-instructions.md` and the three
untracked items remain untouched. Only the paired prompt and this report were
written under ignored `AI-Logs/`; neither is staged or committed.

Prompt: [2026-11-17-blaster-exit-presentation-regression-readonly-prompt.md](../Prompts/2026-11-17-blaster-exit-presentation-regression-readonly-prompt.md)

Report: [2026-11-17-blaster-exit-presentation-regression-readonly-report.md](./2026-11-17-blaster-exit-presentation-regression-readonly-report.md)
