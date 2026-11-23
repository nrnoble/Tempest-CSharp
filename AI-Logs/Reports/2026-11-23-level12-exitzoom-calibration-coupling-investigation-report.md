# Level 12 ExitZoom perspective and calibration-coupling investigation

Date: 2026-11-23

## Status

**INVESTIGATION COMPLETE — no correction implemented.** The current
calibration coupling, focal calculation, and Level 12 visibility failure are
established from the current source and deterministic evaluations. The
proposed correction remains a presentation candidate and requires Neal's
manual acceptance.

## 1. Calibration coupling

Level 12 uses `AtariPresentationCalibration.Resolve(displayedLevel, overrides)`
at each `VectorRenderer` projection. Its per-level default is `(1, 1, 0)`;
the reported `(1.3, .8, 0)` is supplied by the absolute command-line
overrides, not the current default.

The relevant order in `VectorRenderer.ProjectWorldPoint` is:

1. `ProjectWorldPointWithoutExitCompensation` maps the model point through
   `AtariPresentationAdapter.Project`.
2. The Level 12 adapter path applies `TubeYGain` in its model Y displacement.
   `ProjectAtariPresentation` then applies `TubeScale` around that depth's
   projected model origin.
3. `ExitZoomFocalCompensation.GetTranslation` maps the far opening through
   the same uncorrected projector. It adds the latched-opening minus
   current-opening translation to the tube/entity point.
4. `TubeVerticalOffset` is added to the resulting point Y.
5. `Draw` applies the shared experimental portrait playfield camera. Portrait
   HUD lines are rendered separately with the HUD camera.

Therefore:

| Control | ExitZoom focal calculation | Final presentation |
|---|---|---|
| `--tube-y-gain` | **Participates directly.** It changes mapped Level 12 Y coordinates used to calculate both the initial far-rim centroid and each current centroid. | Changes the tube's vertical extent and the mapped Blaster anchor/local vertical scale. |
| `--tube-scale` | **Participates directly.** It reshapes the projection before the focal callback returns its point. | Changes tube width/height and focal-anchor placement; the Blaster's local artwork offset is scaled separately too. |
| `--tube-vertical-offset` | **Does not enter the focal callback or compensation translation.** | Applied after compensation as a common final Y translation to world points, including tube and Blaster anchors. It would move both equally; this task's value is zero. |

This is not “calibration after focal calculation” for Y-gain and tube-scale.
They are upstream inputs to the mapped opening itself. The model geometry and
`(25/14, -1)` Level 12 model-frame mapping are unchanged by all three values.
The portrait playfield camera is also unchanged by them.

For the generic Atari adapter, `Projection.K` and `Projection.ZOffset` enter
the reciprocal-depth projection and its near-depth scale. `ExitZoom` also
passes `PortraitPerspective` to the focal helper's cache comparison, but the
Level 12 Atari adapter projection does not use it; the default is zero here.

## 2. Deterministic Level 12 A/B comparison

### Inputs and method

The current Level 12 production data is closed Rev. 2 shape row 12:
16 raw vertices, scale `0x18`, camera height `0x20`, signed screen Y offset
`-352`, `CenterY=300`, `YOffset=0`, `Position=50`, and `Depth=400`.
`LevelLoader` translates each raw point as
`x=(rawX-128)*25/14`, `y=-(rawY-128)*25/14`; the adapter uses model mapping
`(25/14, -1)`.

The source equations were evaluated deterministically in double precision for
reporting; values are rounded to 0.001 logical pixels. This is an equation
comparison, not a screenshot measurement. The test samples use the production
ExitZoom cadence: onset `(K=70, Z=5)`; end of the Z phase after 23 updates
`(70, -41)`; mid-K after 73 total updates `(240, -41)`; and late after 123
updates `(440, -41)`. Player projected depths at these samples are 50, 119,
279, and 439 respectively.

Model-space landmarks:

- Arithmetic centroid of the 16 production points (closed far rim):
  `(0, -20.982)`.
- Probe tube vertex `Points[11]`: `(0, 200)`.
- Level 12 initial player lane 7, offset 0.5 Blaster base point: midpoint of
  `Points[7]` and `Points[8]`, `(-200, -39.286)`.

### Far-rim trajectory and compensation pivot

The “raw center” is the far-rim centroid before ExitZoom compensation. Each
calibration run latches its own onset point. The “translation” is the
ExitZoom-only `anchor - current centroid`; after it is applied, the centroid
stays at that run's latched target.

| Sample (updates) | K / ZOffset | Raw deep center, identity | Raw deep center, 1.3/.8 | Translation, identity | Translation, 1.3/.8 | Final deep-center target, identity / 1.3/.8 |
|---|---|---:|---:|---:|---:|---:|
| Onset (0) | 70 / 5 | (400, 606.078) | (400, 605.916) | (0, 0) | (0, 0) | (400, 606.078) / (400, 605.916) |
| End Z (23) | 70 / -41 | (400, 2170.476) | (400, 2169.487) | (0, -1564.398) | (0, -1563.571) | (400, 606.078) / (400, 605.916) |
| Mid K (73) | 240 / -41 | (400, 6713.059) | (400, 6709.668) | (0, -6106.981) | (0, -6103.752) | (400, 606.078) / (400, 605.916) |
| Late (123) | 440 / -41 | (400, 12057.275) | (400, 12051.058) | (0, -11451.197) | (0, -11445.142) | (400, 606.078) / (400, 605.916) |

This proves both that calibration participates in the raw focal calculation
and that the current compensation holds the far-rim center at its onset
coordinate. Identity versus candidate changes the onset pivot by only
`-0.162` logical Y, but changes the projected tube shape and off-axis
positions. The large raw Y excursion is not a moving final focal target:
the compensation cancels it for the centroid.

### Projected tube point and Blaster

`Points[11]=(0,200)` is projected at the far rim. “Before calibration”
means identity; “after calibration” means Neal's `1.3/.8/0` candidate.
Columns show uncorrected projection, then the rendered logical point after
each run's focal translation.

| Sample | Probe before calibration | Probe after candidate calibration | Probe after focal translation, identity / candidate |
|---|---:|---:|---:|
| Onset | (400, 648.691) | (400, 650.234) | (400, 648.691) / (400, 650.234) |
| End Z | (400, 2430.892) | (400, 2440.320) | (400, 866.494) / (400, 876.749) |
| Mid K | (400, 7605.916) | (400, 7638.240) | (400, 1498.935) / (400, 1534.487) |
| Late | (400, 13694.180) | (400, 13753.439) | (400, 2242.983) / (400, 2308.297) |

The Blaster base uses the projected midpoint of the active lane edge at
`PlayerProjectedDepth` and shares `ProjectWorldPoint` and its focal
translation. Its rendered host-space base positions are:

| Sample | Identity calibration | Neal candidate |
|---|---:|---:|
| Onset | (372.36, 577.04) | (457.89, 573.68) |
| End Z | (-529.26, -54.29) | (-263.41, -63.34) |
| Mid K | (-1330.29, 26.90) | (-904.23, 15.59) |
| Late | (-1748.35, 674.83) | (-1238.68, 664.98) |

The portrait playfield host mapping is
`xHost=128+1.68*xLogical`,
`yHost=-52.96+1.68*yLogical`; the visible viewport is
`x=[320,1280]`, `y=[0,1280]`. At the end-Z sample the complete projected
Blaster glyph's calculated host X bounds are approximately `[-634,-529]`
for identity and `[-347,-263]` for Neal's candidate. Both are entirely left
of the viewport. The Blaster is not intentionally hidden during ExitZoom:
`Draw` still calls `DrawPlayer`; the base and glyph are simply projected off
screen. Tube anchors and Blaster anchors share the focal translation, but the
Blaster artwork also has local calibration scaling.

The HUD uses a separate 600x800 portrait camera (host scale 1.6). Its logical
center `(300,400)` remains host `(800,640)` in both calibration runs and at
all ExitZoom samples. Tube calibration does not move or scale the HUD.

### What changes and what does not

- **Underlying raw focal trajectory:** changes with Y-gain/scale because the
  callback uses the calibrated projector; the table quantifies the difference.
- **ExitZoom translation:** changes as the difference between each run's own
  current far center and its own latched onset center.
- **Displayed pivot:** each run's far center remains fixed at its own onset
  position. The candidate onset is 0.162 logical pixels higher.
- **Far-end target:** the model target remains the arithmetic centroid of all
  16 closed far-rim vertices; the mapped target changes slightly with
  calibration. There is no open-rail convergence calculation for Level 12.
- **Model geometry/frame:** identical in both runs; points and mapping are not
  rewritten.
- **Tube size:** changes materially. Whole near/far projected bounds are
  `509.091 x 490.909` logical pixels at onset for identity versus
  `407.273 x 510.545` for the candidate. At the late sample they are
  `19555.556 x 18857.143` versus `15644.444 x 19611.429`.
- **Vertical placement:** Y-gain/scale can change mapped geometry and onset
  placement (as above); vertical offset adds its exact value after focal
  compensation. With offset zero there is no extra translation.
- **Blaster:** its rim base remains mathematically attached to its mapped lane
  under each run's shared projection/translation; local glyph scale also
  changes. Mathematical registration does not keep it inside the viewport.

## 3. Level 12 ExitZoom root cause

Level 12 takes the ordinary ROM-metadata adapter path, not a level-specific
ExitZoom projector. Its raw row-12 shape is normalized in `LevelLoader`, and
its model mapping is the same `(25/14,-1)` open-tube normalization used by
Levels 8-11. Its topology is closed (`Continuous=true`).

`ExitZoomFocalCompensation.MapOpening` uses the arithmetic center of every
mapped far-rim vertex for closed definitions; open definitions use mapped
near-to-far rail convergence. The Level 12 pivot is therefore the mapped
far-rim centroid above. `UpdateExitZoom` changes only K/ZOffset and player
depth; it has no tube-direction-aware rotation or moving down-tube target.
Once compensation activates, the far-rim centroid is deliberately held at
the onset screen position. This is a shared PROJECT TRANSLATION, not a
ROM-backed Atari camera rule.

**First semantic divergence:** the current visual rule treats exit focal
compensation as a stationary far-rim anchor. Neal's stated desired behavior
requires the view to progress downward toward the deep end. A fixed target
cannot itself represent that moving direction. Separately, scalar zoom about
the adapter's fixed screen-forward axis expands off-axis geometry; the
centroid translation does not keep a remote Blaster lane in frame.

Level 12's ROM camera height (`0x20`) and signed Y offset (`-352`) contribute
large values to the *uncorrected* screen-Y baseline and raw centroid path.
In the mapped-point differences used after far-centroid compensation, those
metadata terms are common vertical terms and cancel; they do not supply a
direction vector or rotate the exit toward the deep tube. They affect the
absolute onset placement (the center is around host Y=965), not the
Blaster-to-focal relative path that produces the visibility failure.

## 4. Blaster path and visibility

The production player starts on lane 7 at edge position 0.5. In row 12,
vertices 7 and 8 both have raw X `0x10`, so their midpoint is model X `-200`,
far from the fixed logical projection axis X=400. Its screen displacement
grows with the ExitZoom scale. By 23 updates, the whole Blaster glyph is
outside the left edge in both calibration runs; it is not a separate
transform-registration bug and is not hidden by `Draw`.

The Blaster therefore remains correctly registered in model/projection
mathematics but **does not remain visibly registered in the portrait
viewport**. A future correction must preserve the explicit visibility
requirement, not only assert shared translation equality.

## 5. Accepted-level comparison and scope

The focal helper and common world-point compensation are shared across levels.
The topology selects the pivot rule: Level 5 and Level 12 use closed far-rim
centroids; accepted open Levels 8/9 use rail convergence. The Level 18
closeout records Neal's acceptance of Level 5 remaining fixed and Levels 8/9
exiting properly. Their lane-7 source geometry is centered near X=0 (Level 5
lane 7 midpoint X=0; Levels 8/9 likewise), unlike Level 12's X=-200. This
explains why a shared off-axis expansion can be much less visible there.

Level 7's accepted work is presentation reconstruction; its ExitZoom was
explicitly outside that acceptance. Level 11 is an accepted production level
and presentation, but its ExitZoom was not part of that manual closeout.
Neither is described here as an accepted or defective exit. No accepted
Levels 1-11 behavior was reopened.

**Classification: SHARED APPROXIMATION EXPOSED BY LEVEL 12.** The shared
static-focal/zoom architecture is a PROJECT TRANSLATION; Level 12's particular
row-12 lane geometry makes the Blaster visibility failure numerically
unavoidable under the current path. Prior accepted centered geometries can
hide that aspect of the approximation.

## 6. Implementation readiness

**READY FOR BOUNDED CORRECTION.**

Smallest candidate: add a Level-12-only, ExitZoom-only, deterministic
presentation focal/camera path derived from the mapped row-12 tube and current
Blaster base, so the far end advances downward as requested and the full
Blaster remains within the existing portrait viewport. Keep ordinary Playing,
all geometry, calibration defaults, gameplay/exit timing, and accepted
Levels 1-11 unchanged. Do not hard-code a Y adjustment. Treat the result as a
candidate until Neal manually accepts the visual path and Blaster visibility.
No implementation was made.

## 7. Regression-test recommendation

1. Assert Level 12's underlying focal input is unchanged by calibration unless
   a future design explicitly intends that coupling; specifically distinguish
   Y-gain/scale inputs from post-focal vertical offset.
2. At multiple K/Z/player-depth samples, assert the tube and Blaster anchors
   receive the same world translation and the rendered Blaster glyph's host
   bounds remain inside the portrait viewport throughout the corrected exit.
3. Lock representative accepted Levels 1-11 projection/focal outputs, with
   explicit acceptance boundaries for Levels 5/8/9 and no new ExitZoom claim
   for Levels 7/11.
4. Use genuine row-12 production points to assert the corrected deep-end
   trajectory moves in the required down-tube direction without arbitrary
   per-frame Y constants.
5. Repeat the same deterministic camera/player samples and require identical
   focal path, Blaster bounds, and rendered coordinates.

## Validation and preservation

- **PASS** — focused existing tests:
  `dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~ExitZoomFocalCompensationTests|FullyQualifiedName~Level12ProductionTests|FullyQualifiedName~AtariPresentationAdapterTests" --verbosity minimal`
  Result: 76 passed, 0 failed, 0 skipped.
- No interactive game was launched; Neal's manual visual acceptance of a
  future candidate remains required.
- Starting state: branch `master`, HEAD
  `aa6b7110fe845cf36173a80b7dc5d11c1f067806`, tracking `origin/master`.
  The worktree already had unrelated edits and task-related uncommitted Level
  12 integration changes, including `LevelLoader`, ROM metadata, adapter,
  calibration, and `Level12ProductionTests`. They were preserved. No source,
  test, configuration, durable documentation, or Git state was changed here;
  no files were staged or committed. The paired prompt/report were created
  under `AI-Logs/`; Git reports these and pre-existing AI-Logs material as
  untracked rather than ignored in this worktree.

## Evidence boundary

The user's symptom and requested arcade direction are Neal-reported manual
evidence; they were not independently observed in this read-only task.
Recovered Level 12 row data and metadata are reported as ROM-BACKED by the
existing Level 12 integration record; no ROM files were accessed. Current
projection, camera, and focal math are source-backed PROJECT TRANSLATIONS.
The correction described above is a candidate, not an accepted behavior.
