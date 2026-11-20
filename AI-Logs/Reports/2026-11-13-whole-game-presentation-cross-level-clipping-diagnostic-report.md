# Whole-Game Presentation Cross-Level Clipping Diagnostic

**Date:** 2026-11-13
**Status:** READ-ONLY DIAGNOSTIC — READY FOR ONE SHARED CORRECTION
**Completion token:** `TEMPEST_PRESENTATION_CROSS_LEVEL_CLIPPING_DIAGNOSTIC_20261113`

## Starting state and scope

- Branch: `master`
- `HEAD` and `origin/master`: `385f603a94391756b2ca6cccd524dc17b281e782`
- Index: clean
- Exactly four tracked files modified: `Program.cs`, `DisplayConfiguration.cs`,
  `VectorRenderer.cs`, and `DisplayConfigurationTests.cs`
- Non-ignored untracked files: none
- Starting diff summary: 4 files changed, 373 insertions, 35 deletions

The candidate remained unchanged. This was a source/report/bounds-only
diagnostic: no game launch, tests, visual-asset inspection, production/test
edits, staging, commit, or push.

The preserved user prompt is
[`2026-11-13-whole-game-presentation-cross-level-clipping-diagnostic-prompt.md`](../Prompts/2026-11-13-whole-game-presentation-cross-level-clipping-diagnostic-prompt.md).

## Authorities and assumptions

Read the 2026-11-12 original investigation, its label-correction report, and
the 2026-11-13 candidate reconstruction report. The label correction governs
visual provenance (LEFT = C#, RIGHT/MAME = target); the reconstruction report
governs the candidate and reported failures.

This calculation uses the current production definitions and the experimental
values as project translations: 1280 × 960 host; legacy 800 × 600 projection
coordinates; portrait 600 × 800 logical area inside a centered 720 × 960
viewport; 1.2 output scale; 1.05 playfield scale; −78 internal vertical
translation; `K=70`; `ZOffset=5`. All production definitions use depth 400 and
position 50. The projection bounds below span every tube vertex projected at
both `z=50` and `z=450`, covering the near rim, far rim, and rail endpoints.
These are geometric calculations, not arcade pixel specifications.

## Source paths inspected

- `src/Tempest.Game/Rendering/DisplayConfiguration.cs`
- `src/Tempest.Game/Rendering/Projection.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Levels/LevelDefinition.cs`
- `src/Tempest.Game/Levels/LevelLoader.cs`
- `src/Tempest.Game/Levels/Level.cs`
- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `tests/Tempest.Game.Tests/DisplayConfigurationTests.cs`
- The three authoritative reports named above

The actual per-level `CenterY`/`YOffset` values come from `LevelLoader`:
L1–L3 `(400,-80)`, L4 `(380,-65)`, L5 `(415,-90)`, L6 `(360,-40)`,
L7 `(310,-10)`, and L8–L9 `(300,0)`.

## Transform sequence and anchor

For each projected tube point `(x,y)`, the unchanged projection is:

`x = 400 + modelX * 70 / (depth + 5)`
`y = CenterY + (modelY + YOffset) * 70 / (depth + 5)`

The portrait playfield camera has zoom `1.2 * 1.05 = 1.26`, target
`(400, 455.2381)`, and offset `(640,480)`. Equivalently:

- Before output scaling, `xInternal = 300 + 1.05 * (x - 400)`.
- Before output scaling, `yInternal = 1.05 * y - 78`.
- Host coordinates are `xHost = 280 + 1.2 * xInternal` and
  `yHost = 1.2 * yInternal`.
- Thus the final affine mapping is `xHost = 640 + 1.26 * (x - 400)` and
  `yHost = 1.26 * y - 93.6`.

Horizontally, the transform scales about the legacy logical center `x=400`
and recenters that point at portrait logical `x=300`. Vertically, it scales
about projected origin `y=0`, then applies −78 portrait-internal units. The
Raylib camera target is a camera-space target, not a tube centroid, near-rim
center, model origin, or all-level bounding-box center. The effective vertical
translation is the same for every level.

## Levels 1–9 bounds

Bounds are inclusive `[min,max]`. “Projected” is the 800 × 600 projection;
“legacy host” applies the established 1.6 scale. “Portrait internal” is after
the shared 1.05 scale/−78 placement but before output scaling. “Portrait host”
includes the 1.2 scale and 280-pixel horizontal viewport offset. The viewport
is `x=[280,1000]`, `y=[0,960]`. Overflow is ordered left/right/top/bottom.

| Level | Model X / Y | CenterY / YOffset | Projected X / Y | Legacy host X / Y | Portrait internal X / Y | Portrait host X / Y | Overflow L/R/T/B | Point order |
|---|---|---:|---|---|---|---|---|---|
| 1 | `[-200,200] / [-200,200]` | `400 / -80` | `[145.45,654.55] / [43.64,552.73]` | `[232.73,1047.27] / [69.82,884.36]` | `[32.73,567.27] / [-32.18,502.36]` | `[319.27,960.73] / [-38.62,602.84]` | `0/0/38.62/0` | preserved, + |
| 2 | `[-161,161] / [-161,161]` | `400 / -80` | `[195.09,604.91] / [93.27,503.09]` | `[312.15,967.85] / [149.24,804.95]` | `[84.85,515.15] / [19.94,450.25]` | `[381.81,898.19] / [23.92,540.29]` | `0/0/0/0` | preserved, + |
| 3 | `[-197,197] / [-197,197]` | `400 / -80` | `[149.27,650.73] / [47.45,548.91]` | `[238.84,1041.16] / [75.93,878.25]` | `[36.74,563.26] / [-28.17,498.35]` | `[324.08,955.92] / [-33.81,598.03]` | `0/0/33.81/0` | preserved, + |
| 4 | `[-219,219] / [-116,145]` | `380 / -65` | `[121.27,678.73] / [149.64,481.82]` | `[194.04,1085.96] / [239.42,770.91]` | `[7.34,592.66] / [79.12,427.91]` | `[288.80,991.20] / [94.94,513.49]` | `0/0/0/0` | preserved, + |
| 5 | `[-210,210] / [-210,210]` | `415 / -90` | `[132.73,667.27] / [33.18,567.73]` | `[212.36,1067.64] / [53.09,908.36]` | `[19.36,580.64] / [-43.16,518.11]` | `[303.24,976.76] / [-51.79,621.74]` | `0/0/51.79/0` | preserved, + |
| 6 | `[-171,171] / [-218,145]` | `360 / -40` | `[182.36,617.64] / [31.64,493.64]` | `[291.78,988.22] / [50.62,789.82]` | `[71.48,528.52] / [-44.78,440.32]` | `[365.78,914.22] / [-53.74,528.38]` | `0/0/53.74/0` | preserved, + |
| 7 | `[-179,179] / [-179,179]` | `310 / -10` | `[172.18,627.82] / [69.45,525.09]` | `[275.49,1004.51] / [111.13,840.15]` | `[60.79,539.21] / [-5.07,473.35]` | `[352.95,927.05] / [-6.09,568.01]` | `0/0/6.09/0` | preserved, + |
| 8 | `[-200,200] / [-200,200]` | `300 / 0` | `[145.45,654.55] / [45.45,554.55]` | `[232.73,1047.27] / [72.73,887.27]` | `[32.73,567.27] / [-30.27,504.27]` | `[319.27,960.73] / [-36.33,605.13]` | `0/0/36.33/0` | preserved, − (open) |
| 9 | `[-200,200] / [-28.57,200]` | `300 / 0` | `[145.45,654.55] / [263.64,554.55]` | `[232.73,1047.27] / [421.82,887.27]` | `[32.73,567.27] / [198.82,504.27]` | `[319.27,960.73] / [238.58,605.13]` | `0/0/0/0` | preserved, − (open) |

`+`/`−` records the signed area of the existing ordered point list (using an
implicit closing chord for the open definitions). The portrait transform has
positive uniform scale and no reflection or rotation, so it preserves each
sign and the vertex/lane order. Levels 8–9 are open and have 15 playable lanes;
their implicit closing chord is not a playable lane.

The candidate's Level 9 width is approximately 641.46/720 = **89.09%** of the
portrait viewport. None of the nine tubes exceeds either horizontal edge or
the bottom edge. The observed failures are strictly top-edge overflow: L1
38.62 px, L3 33.81 px, L5 51.79 px, L6 53.74 px, L7 6.09 px, and L8 36.33 px.

## Root cause and feasibility of a shared correction

**Primary cause:** pre-existing differences in production model Y bounds and
per-level `CenterY`/`YOffset` projection inputs. The candidate then applies
one fixed −78 internal-unit translation chosen around Level 9's placement,
not around the vertical envelope of all levels. Under the same projection,
Level 9's projected minimum Y is 263.64; the clipped levels' minima range from
31.64 to 69.45. This is visible directly in the matrix before the portrait
transform. For example, Level 5 combines model `minY=-210`, `YOffset=-90`,
and `CenterY=415`; Level 6 combines `minY=-218`, `YOffset=-40`, and
`CenterY=360`. Level 9 instead has `minY=-28.57`, `YOffset=0`, and
`CenterY=300`.

The fixed vertical translation is therefore not safe for the all-level
envelope. The transform's x anchor is well-defined at the legacy canvas
center; its y placement is not anchored to a shared tube feature. This does
not implicate changed gameplay geometry: the definitions are unchanged, and
the renderer applies the presentation transform after projection.

The measured union of all portrait-host tube bounds is
`x=[288.80,991.20]`, `y=[-53.74,621.74]`. A single downward translation of
the shared playfield can remove the maximum top overflow without changing
scale, x placement, point geometry, or the Level 9 width invariant; the
minimum shift to reach exactly zero top margin is 53.74 host pixels
(44.78 portrait-internal units), after which the union's bottom is 675.48,
still within the 960-pixel viewport. This is a feasibility bound derived
from the all-level envelope, **not** a selected final calibration value or a
claim that the resulting Level 9 vertical placement matches MAME. A later
implementation should derive its one vertical placement from an explicit
all-level safe envelope and still require Neal's manual presentation
acceptance.

No evidence supports per-level offsets, level-specific transforms, point
rewrites, or projection-sign changes. A shared fit/translation based on only
Level 9 is rejected because its upper bound does not represent Levels 1–8.
Shrinking the whole playfield is not required to resolve the measured
vertical overflow and would reduce the currently measured Level 9 horizontal
prominence.

### Projection constants

`K` and `ZOffset` enter the projection formula and therefore mathematically
affect the projected bounds. They are the same global values for all nine
levels and were not altered by this candidate. The observed cross-level
difference is already explained by the production points and
`CenterY`/`YOffset`; there is no evidence here that K or ZOffset should be
changed. This diagnostic does not test the separate shoulder/far-rim
perspective question.

## Tube and gameplay-entity alignment

In the actual `VectorRenderer.Draw` path, portrait mode begins one scissor
rectangle and one `BeginMode2D(GetPlayfieldCamera(...))` before drawing the
transition stars, tube, Blaster, enemies, shots, spikes, explosions, score
popups, and game-over vector records. The same mode ends after the entire
world/playfield pass. `DrawGameOverAnimation` also uses that same selected
playfield camera. The portrait HUD and screen messages are drawn afterward
under their separate HUD camera by design.

Projection entry points differ, but in ordinary play use the same level
definition, `CenterY`/`YOffset`, and global projection before the shared
camera: tube rims use `ProjectApproachGeometry`; the Blaster uses it through
`GetPlayerEdge`; Spikes, Spikers, Tankers, bullets, and effects use
`ProjectLane`/`ProjectApproachGeometry`; Eggs, shots, Flippers, Pulsars,
Fuseballs, and game-over records call `Projection.Project` directly. All
draw submissions remain inside the same playfield camera. No rendering path
was found that moves tube lines to the portrait camera while drawing
tube-relative entities with the legacy presentation camera.

There are pre-existing state-specific projection inputs: approach offsets
are incorporated by `ProjectApproachGeometry`, while some direct projection
helpers do not use that helper. This is distinct from bypassing the
presentation camera and did not produce the measured production tube bounds.
For the failure described in discarded experimentation—tube transformed
while gameplay entities remain under the legacy output mapping—the current
candidate's shared `BeginMode2D` structurally prevents that camera split in
`Draw`. This finding does not assert that every transition-phase object has
identical pre-camera projection logic.

## Secondary test findings

1. **Expected 48, actual 46:** the exact failed assertion is
   `Assert.Equal(originalPoints.Length * 3, lines.Count)` at line 77 of
   `DisplayConfigurationTests.cs`. The expected 48 is 16 rails plus 16 near
   rim and 16 far rim segments for a closed 16-lane tube. `DrawTube` emits
   16 rails plus 15 playable near-rim and 15 far-rim segments for the open
   production Levels 8 and 9, which is 46. The newly broadened legacy test
   incorrectly assumes every 16-vertex level is closed. This is a test
   assumption error unrelated to portrait clipping; it reveals the existing
   open-tube topology, not a projection-coordinate mismatch. No test was
   changed.
2. **Expected 960, actual 960.00006:** this is ordinary float rounding from
   applying the 1.2 camera scale to the HUD's 800-logical-unit bottom
   coordinate. The separate HUD viewport is 960 physical pixels high; the
   difference is about 0.00006 pixel and is not evidence of a meaningful
   bounds or clipping problem. No tolerance or calculation was changed.

## Diagnostic classification

**READY FOR ONE SHARED CORRECTION.** The source and full Level 1–9 bounds
identify one bounded correction family: replace the Level-9-derived fixed
vertical placement with a single placement derived from the union of all
production tube bounds, keeping the shared camera, portrait viewport, scale,
gameplay geometry, and independent HUD. The calculated shift above proves
that a vertical translation alone can satisfy the point-bound viewport
invariant without level-specific rules; it is not final visual tuning.

## Final state

Final branch/`HEAD`/`origin/master` remain `master` /
`385f603a94391756b2ca6cccd524dc17b281e782`. The index is clean; the same
four intentional candidate files remain modified; no non-ignored untracked
files were created. Only the allowed ignored diagnostic prompt and report
were created. Candidate source and test files were not edited. Nothing was
staged, committed, or pushed. No visual assets were analyzed.
