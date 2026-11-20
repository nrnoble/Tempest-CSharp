# Whole-Game Presentation Tube Size Read-Only Analysis

**Investigation date:** 2026-11-14
**Status:** COMPLETED — READ-ONLY ANALYSIS
**Requested report filename retained:** `2026-11-13-whole-game-presentation-tube-size-readonly-analysis-report.md`

## Scope and evidence

This investigation used the current candidate source and diff, the five
authoritative presentation reports listed below, and Neal's supplied manual
finding. The candidate was not modified. No game, test suite, or special build
was run. No screenshots, images, videos, gameplay recordings, Gemini material,
external implementations, web sources, or raw ROM files were inspected.

Authoritative reports read:

- `2026-11-12-whole-game-presentation-level9-readonly-investigation-report.md`
- `2026-11-12-whole-game-presentation-level9-label-correction-report.md`
- `2026-11-13-whole-game-presentation-experiment-reconstruction-report.md`
- `2026-11-13-whole-game-presentation-cross-level-clipping-diagnostic-report.md`
- `2026-11-13-whole-game-presentation-shared-vertical-placement-correction-report.md`

The final shared-vertical-placement report governs current candidate bounds
and translation. The label-correction report governs the earlier image
provenance; no image-dependent claim was independently revisited here.

## Starting Git state

- Branch: `master`
- `HEAD`: `385f603a94391756b2ca6cccd524dc17b281e782`
- `origin/master`: `385f603a94391756b2ca6cccd524dc17b281e782`
- Index: clean
- Tracked worktree: exactly the four expected candidate files modified:
  - `src/Tempest.Game/Program.cs`
  - `src/Tempest.Game/Rendering/DisplayConfiguration.cs`
  - `src/Tempest.Game/Rendering/VectorRenderer.cs`
  - `tests/Tempest.Game.Tests/DisplayConfigurationTests.cs`
- Starting diff summary: 4 files changed, 429 insertions, 36 deletions
- Non-ignored untracked files: none

The supplied prompt was preserved at
`AI-Logs/Prompts/2026-11-13-whole-game-presentation-tube-size-readonly-analysis-prompt.md`.

## Files and source paths inspected

- The four candidate files and their current diffs
- `src/Tempest.Game/Rendering/Projection.cs`
- `src/Tempest.Game/Levels/LevelLoader.cs`
- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/Debug/DebugController.cs`
- `src/Tempest.Game/Debug/InteractiveSpeedOptions.cs`
- The five authoritative reports above

No source or test files were changed.

## Current presentation and transform

Current project-translation values:

| Value | Current value |
|---|---:|
| Host window | 1280 × 960 |
| Portrait logical presentation | 600 × 800 |
| Centered portrait viewport | 720 × 960, left edge 280 |
| Output scale | 1.2 |
| Shared playfield scale | 1.05 |
| Combined playfield zoom | 1.26 |
| Shared vertical translation | −33.1 portrait-internal units |
| Projection `K` / `ZOffset` | 70 / 5 |
| Horizontal projected center | x = 400 |
| HUD/screen-space camera | Separate 1.2-scale portrait camera |

`DisplayConfiguration.ExperimentalPortraitPlayfieldCamera` sets
`Target.X = 400`, `Offset = (640,480)`, and `Zoom = 1.2 × 1.05`. It derives
`Target.Y` from the unchanged vertical translation:

`Target.Y = (480 − 1.2 × (−33.1)) / 1.26 = 412.476190…`

For projected playfield point `(x,y)`, the current host mapping is therefore:

`xHost = 640 + 1.26 × (x − 400)`
`yHost = 1.26 × y − 39.72`

The **existing `PortraitPlayfieldScale` value**, multiplied by
`PortraitOutputScale`, controls the complete playfield size. It is not a
Level-9-only value. `K`, `ZOffset`, per-level `CenterY`/`YOffset`, and model
points supply the unchanged projection and are not size-only controls.

There are two distinct scale pivots worth distinguishing:

1. **Changing `PortraitPlayfieldScale` alone while retaining −33.1** makes the
   camera recompute `Target.Y`. The exact mapping scales around the projected
   origin and the fixed translation: host pivot `(640, −39.72)`. This is the
   source-faithful, one-value adjustment.
2. **Holding the current Raylib `Target` fixed while changing camera zoom**
   scales around its host anchor `(640,480)`. The current getter does not hold
   `Target.Y` fixed when `PortraitPlayfieldScale` changes; doing so would
   change the placement semantics and needs an explicit anchor rule.

Both are uniform affine scales. Neither changes model geometry, the
projection's depth formula, or relative near/far scale. A uniform transform
preserves tube shape, orientation, horizontal centering, perspective ratios,
and tube/entity registration. Separate horizontal and vertical scales would
distort tube proportions and are not appropriate before the deferred
perspective investigation.

## Current Levels 1–9 bounds and headroom

Bounds are host pixels; viewport is `x=[280,1000]`, `y=[0,960]`. The reported
production tube bounds include every tube vertex projected at both rims
(`z=50` and `z=450`). Percentages use the 720-pixel viewport width and
960-pixel viewport height. Clearances are ordered left/right/top/bottom.

| Level | Host X min/max | Host Y min/max | Width | Height | Width % | Height % | Clearance L/R/T/B |
|---:|---:|---:|---:|---:|---:|---:|---:|
| 1 | 319.27 / 960.73 | 15.26 / 656.72 | 641.46 | 641.46 | 89.09% | 66.82% | 39.27 / 39.27 / 15.26 / 303.28 |
| 2 | 381.81 / 898.19 | 77.80 / 594.17 | 516.38 | 516.37 | 71.72% | 53.79% | 101.81 / 101.81 / 77.80 / 365.83 |
| 3 | 324.08 / 955.92 | 20.07 / 651.91 | 631.84 | 631.84 | 87.76% | 65.82% | 44.08 / 44.08 / 20.07 / 308.09 |
| 4 | 288.80 / 991.20 | 148.82 / 567.37 | 702.40 | 418.55 | 97.56% | 43.60% | 8.80 / 8.80 / 148.82 / 392.63 |
| 5 | 303.24 / 976.76 | 2.09 / 675.62 | 673.52 | 673.53 | 93.54% | 70.16% | 23.24 / 23.24 / 2.09 / 284.38 |
| 6 | 365.78 / 914.22 | 0.14 / 582.26 | 548.44 | 582.12 | 76.17% | 60.64% | 85.78 / 85.78 / 0.14 / 377.74 |
| 7 | 352.95 / 927.05 | 47.79 / 621.89 | 574.10 | 574.10 | 79.74% | 59.80% | 72.95 / 72.95 / 47.79 / 338.11 |
| 8 | 319.27 / 960.73 | 17.55 / 659.01 | 641.46 | 641.46 | 89.09% | 66.82% | 39.27 / 39.27 / 17.55 / 300.99 |
| 9 | 319.27 / 960.73 | 292.46 / 659.01 | 641.46 | 366.55 | 89.09% | 38.18% | 39.27 / 39.27 / 292.46 / 300.99 |

The values reproduce the final shared-placement report. Level 6 has the
smallest top clearance (about 0.142 px); Level 4 has the smallest horizontal
clearance (8.8 px on each side); Level 5 has the largest bottom extent.

Level 9's model X bounds are ±200. Its near-rim projected width is
`2 × 200 × 70 / 55 = 509.0909…` projection units. Applying the shared 1.26
scale gives **641.4545… host pixels**, or **89.0909…%** of the 720-pixel
viewport. This verifies the reported 641.46 px / **89.09%**. That is a
measurement, not evidence that a specific target percentage is correct.

## Uniform-size probes and safe headroom

### Source-faithful adjustment: retain the current translation

Multiplying the current combined playfield zoom by factor `s` while keeping
`PortraitPlayfieldVerticalTranslation = −33.1` maps current host bounds as:

`x' = 640 + s × (x − 640)`
`y' = −39.72 + s × (y + 39.72)`

This is the effect of changing the existing playfield-scale constant alone.
Level 9's minimum-Y point moves **down** as scale increases; its bounding-box
center also moves down. Thus this leaves the translation constant unchanged,
but does **not** freeze the whole tube's apparent vertical placement.

| Added uniform scale | Level 9 width | Width % | Level 9 height | Height % | Worst L/R/T/B clearance | First clipping |
|---:|---:|---:|---:|---:|---:|---|
| +2% (`s=1.02`) | 654.29 px | 90.87% | 373.88 px | 38.95% | 1.78 / 1.78 / 0.94 / 270.07 px | None |
| +4% (`s=1.04`) | 667.12 px | 92.66% | 381.21 px | 39.71% | −5.25 / −5.25 / 1.74 / 255.77 px | Level 4 left and right |
| +6% (`s=1.06`) | 679.95 px | 94.44% | 388.54 px | 40.47% | −12.27 / −12.27 / 2.54 / 241.46 px | Level 4 left and right |
| Maximum contact (`s=1.02505695`) | 657.53 px | 91.32% | 375.73 px | 39.14% | 0.00 / 0.00 / 1.14 / 266.46 px | Level 4 touches both sides |

For this source-faithful path:

- Horizontal ceiling: `720 / 702.4 = 1.0250569476…`, a **2.505695%**
  increase. Level 4 reaches both horizontal viewport edges.
- Vertical ceiling: the Level 5 bottom bound gives
  `(960 + 39.72) / (675.62 + 39.72) = 1.397545…`; top clearance increases
  because all current tube minima lie below the host-space pivot.
- Therefore the exact all-level safe ceiling is the horizontal one:
  **1.0250569476…**, with any strict interior scale smaller than that.
  Practical enlargement is limited by **width**, not height.

At +2%, Level 9's bounds become approximately `y=299.10..672.98`; its
bounding-box center moves from 475.74 to 486.04 px (**10.31 px downward**).
The Level 9 minimum-Y point moves from 292.46 to 299.10 px (**6.64 px
downward**). At +4% and +6%, that point moves down 13.29 px and 19.93 px,
respectively. This is a material placement consequence despite retaining
−33.1.

### Alternative: keep the current camera target as the scale pivot

If zoom is instead multiplied around the current target's screen anchor
`(640,480)`, then
`x'=640+s(x−640)`, `y'=480+s(y−480)`. Level 9's bounding-box center remains
nearly fixed because it is currently near y=480, but the upper tube bounds
move upward. Level 6's top bound reaches the viewport boundary after only:

`480 / (480 − 0.1418181818…) = 1.0002955419…`

That is only a **0.029554%** increase. The horizontal Level 4 ceiling remains
1.02505695, so this pivot is limited by **Level 6, top edge**, and allows no
meaningful size increase while retaining all-level containment.

| Added uniform scale | Level 9 width | Width % | Level 9 height | Height % | Worst L/R/T/B clearance | Clipping |
|---:|---:|---:|---:|---:|---:|---|
| +2% (`s=1.02`) | 654.29 px | 90.87% | 373.88 px | 38.95% | 1.78 / 1.78 / −9.45 / 280.47 px | Levels 5 and 6 top |
| +4% (`s=1.04`) | 667.12 px | 92.66% | 381.21 px | 39.71% | −5.25 / −5.25 / −19.05 / 276.56 px | Levels 1, 5, 6, 8 top; Level 4 sides |
| +6% (`s=1.06`) | 679.95 px | 94.44% | 388.54 px | 40.47% | −12.27 / −12.27 / −28.65 / 272.64 px | Levels 1, 3, 5, 6, 8 top; Level 4 sides |
| Maximum contact (`s=1.00029554`) | 641.65 px | 89.12% | 366.66 px | 38.19% | 8.70 / 8.70 / 0.00 / 284.32 px | Level 6 touches top |

The target-pivot option moves Level 9's minimum-Y point upward by 3.75, 7.50,
and 11.25 px at +2%, +4%, and +6%. It keeps the near-centered bounding-box
center within 0.26 px at +6%, but clips the all-level envelope almost
immediately.

These are different camera-anchor semantics, not conflicting arithmetic.
The existing scale constant with fixed translation yields the first table;
the second table requires holding the current Raylib target fixed. No existing
anchor preserves the Level 9 minimum-Y point while also giving a useful
all-level-safe increase: pivoting at its current host Y of 292.46 px would
still make Level 6's top edge the limit at
`292.46 / (292.46 − 0.141818…) = 1.00048515…` (+0.048515%). Choosing a pivot
at that feature would be a new calibration decision, not a value already
encoded by the current camera.

## Perspective, model geometry, and registration

Uniform post-projection scaling multiplies near- and far-depth projected
coordinates by the same factor around its chosen pivot. It leaves the
`K=70`, `ZOffset=5`, depth formula, near/far scale ratio, and projected line
relationships unchanged. It can move features relative to the viewport
because of its pivot, but does not itself alter perspective.

The `VectorRenderer.Draw` playfield camera wraps the tube and gameplay-space
rendering in the same pass: lane boundaries, Blaster/player, enemies and
Spikers, player/enemy shots, Spikes, destruction effects, score popups,
transition stars, and applicable Game Over vectors. Their relative
tube/entity registration therefore remains coherent under a uniform
playfield-camera scale. Portrait HUD and screen-space overlays use a separate
camera and are intentionally not scaled with the playfield. No gameplay or
model geometry change is needed or recommended.

## Current command-line options and composability

Options relevant to manual presentation testing, verified in `Program.cs` and
`DebugController`:

- `--level N`: fresh production start at a level from 1 through 9, then normal
  progression.
- `--lives N`: starting-life override.
- `--speed N`: interactive host-loop target FPS multiplier (0.25–4.0), not
  simulation speed; it is not part of recording identity.
- `--rim-speed N`: gameplay rim-speed multiplier; recording may store it as
  startup metadata.
- `--record-gameplay <file>` and `--playback-file <file>`: mutually exclusive
  deterministic file modes.
- `--gameplay-playback` and `--gameplay-playback-level4`: seeded playback
  profiles, distinct from file playback.
- `--legacy-presentation`: selects the established 4:3 presentation.
- Scenario selectors documented by `Program.cs`: `--level2-test`,
  `--level3-test`, `--level4-test`, `--level6-test`, `--level7-test`,
  `--level3-geometry-test`, `--spiker-test`, `--tanker-test`,
  `--fuseball-test`, and `--pulsar-test`. These scenario selectors are
  mutually exclusive with each other and with `--level N`.
- Other composable diagnostics include `--enemy-bullets-test`,
  `--game-over-density N`, `--level7-spiker-speed N`,
  `--level9-spiker-speed N`, and `--spinner-sensitivity N`; these are not
  presentation-size controls.

No presentation-size option currently exists. A future option is **unsupported
by the current parser**. Ordinary launches do not have a general unknown
argument validator; in file modes, `TryParseGameplayFileCommand` has a strict
allowlist and rejects unknown arguments. A future presentation-only option
must be parsed explicitly and added to that file-mode allowlist if it is to
compose with recording/playback.

| Future size option combined with | Classification | Existing behavior / requirement |
|---|---|---|
| `--level N` | SAFE TO COMPOSE | Orthogonal presentation setting; do not make size selection force a level. |
| `--lives N` | SAFE TO COMPOSE | Valid for normal play and recording. Existing file playback rejects `--lives` because startup values are restored. |
| `--speed N` | SAFE TO COMPOSE | Changes host-loop target FPS only; recognized in file modes and not recorded as simulation identity. |
| `--rim-speed N` | SAFE TO COMPOSE | Orthogonal to presentation; recording can store it. File playback rejects a direct override because the recording restores it. |
| `--record-gameplay <file>` | SAFE TO COMPOSE | Presentation size should remain outside simulation recording identity. Explicitly allow the option in the file-mode parser. Playback must receive its own size selection if the same presentation is desired. |
| `--playback-file <file>` | SAFE TO COMPOSE | A presentation-only override does not conflict with restored simulation startup. Explicitly allow it in the strict parser; `--level`, `--lives`, and `--rim-speed` remain disallowed with playback. |
| `--legacy-presentation` | SEMANTICALLY CONFLICTING | Recommend explicit rejection of the combination; see below. |
| Current parser without adding the option | UNSUPPORTED BY CURRENT PARSER | No size option exists; file modes reject unknown options, while normal launches do not provide a reliable parsed setting. |

File recording and file playback themselves are mutually exclusive. Seeded
playback and the listed scenario selectors are separate scenarios; a new size
setting should remain orthogonal rather than becoming another scenario
selector.

## Legacy behavior and runtime-option recommendation

Recommend **rejecting** a future size option when combined with
`--legacy-presentation`. The legacy mode is the A/B reference; silently
ignoring a portrait-only size value in legacy mode would look like a successful
but ineffective calibration. Explicit rejection makes the mode distinction
observable and keeps legacy unchanged.

A special executable/test build is not warranted. The first experiment can
change the single `PortraitPlayfieldScale` candidate value in the uncommitted
worktree and use the ordinary launch with `--level N`; this avoids adding
parser surface before anchor semantics are settled. If a runtime override is
later justified, it should be numeric, affect only the portrait playfield,
compose with supported gameplay/file options, remain outside recording
identity, and reject `--legacy-presentation`. No option name or numeric target
is proposed here.

## Conclusion and smallest next experiment

**Final classification: SIZE REQUIRES ANCHOR DECISION**

The current scale constant is a clean uniform size control, and changing only
it leaves projection and game geometry intact. However, the existing
translation-preserving transform moves the Level 9 apparent bounds downward;
holding the current camera target instead preserves the near-centered overall
placement but permits only a 0.029554% increase before Level 6 clips. Neither
existing behavior simultaneously supplies a useful safe increase and
preserves the approved vertical placement. An anchor/placement priority must
be explicitly chosen before implementation.

If Neal authorizes the translation-preserving behavior despite its predicted
vertical movement, the smallest useful analytical candidate is a +2% uniform
increase (`PortraitPlayfieldScale` 1.05 → 1.071) with −33.1, K, ZOffset, and
all model/gameplay values unchanged. It remains inside the current
Levels 1–9 envelope, but Level 9's bounding-box center moves down about
10.31 px. This is **not recommended as placement-neutral** and was not
implemented. If the placement must remain centered on the current camera
target, no meaningful safe size experiment is established by the current
all-level bounds.

## Final state

- Branch remains `master`.
- `HEAD` and `origin/master` remain
  `385f603a94391756b2ca6cccd524dc17b281e782`.
- The same four tracked candidate files remain modified; their contents were
  not changed by this investigation.
- No other tracked files were changed; no non-ignored untracked files were
  introduced.
- Nothing was staged, committed, or pushed.
- No game or tests were run.
- No visual assets, Gemini material, external source, or raw ROM files were
  analyzed.
