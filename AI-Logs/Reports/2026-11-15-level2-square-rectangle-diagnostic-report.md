# Tempest: Level 2 Square-versus-Rectangle Presentation Diagnostic

## Executive conclusion

**Diagnostic classification: MISSING PRESENTATION ASPECT TRANSFORM.**

Level 2's C# model and the corresponding Atari raw record are both square in
their own coordinate frames. The Level 2 projection applies equal X/Y scale at
every depth; the portrait playfield camera also uses a uniform zoom. Those
equal-axis stages directly preserve the square despite the accepted placement
and depth behavior. A Level 2-only vertical aspect operation is feasible
without changing the tube model or gameplay.

The responsible shape-preserving stage is the equal-axis Level 2 depth
projection (followed by uniform portrait zoom), not the +114 translation or
the portrait viewport. A presentation aspect operation centered on the
projected tube center, before +114 placement, is the cleanest seam. Its
magnitude is not established by the available evidence and needs Neal's
manual calibration.

## Accepted Level 2 checkpoint

The following are frozen for this diagnostic: +114 vertical placement,
near/far depth and perspective, near/far center relationship, model points,
point order, lane identities, closed topology, gameplay, and deterministic
recorded playback. No visual assets were opened and no visual comparison was
performed.

## Current aspect reconstruction

The Level 2 point bounds are X/Y `-161..161`, or `322 x 322` model units
(`src/Tempest.Game/Levels/LevelLoader.cs:52-59`). The production configuration
uses `Position=50` and `Depth=400` (`Levels/Level.cs:18-19`); the runtime
baseline is `K=70`, `ZOffset=5` (`Program.cs:233-234`). “Middle” below means
`Position + Depth/2 = 250`. The accepted placement path is the opt-in Level 2
adapter; the legacy-compatible path uses the same reciprocal-depth, equal-axis
projection but does not add that adapter placement.

For `s = K/(z + ZOffset)`, the Level 2 output dimensions are `322s` on both
axes. The portrait host camera multiplies both by `1.26` (`DisplayConfiguration.cs:24-29, 54-57`).

| Rim | Depth `z` | `s` | Logical W x H | Logical Y bounds, with +114 | Portrait-host W x H | Portrait-host Y bounds | Logical / host W/H |
|---|---:|---:|---:|---:|---:|---:|---:|
| Near | 50 | 1.272727 | 409.82 x 409.82 | 207.27..617.09 | 516.37 x 516.37 | 221.44..737.81 | 1.00 / 1.00 |
| Middle | 250 | 0.274510 | 88.39 x 88.39 | 447.84..536.24 | 111.37 x 111.37 | 524.56..635.94 | 1.00 / 1.00 |
| Far | 450 | 0.153846 | 49.54 x 49.54 | 476.92..526.46 | 62.42 x 62.42 | 561.20..623.62 | 1.00 / 1.00 |

At each depth, the logical local Jacobian with respect to the C# point
coordinates is

```text
J = [ [ s, 0 ],
      [ 0, s ] ]
```

Thus `d(screenX)/d(modelX) = d(screenY)/d(modelY) = s`; the portrait-host
Jacobian is `1.26 J`. The scale derivatives at near/middle/far are
`1.272727`, `0.274510`, and `0.153846` on both axes. The near/far size ratio
is `8.272727` on each axis. Equal X/Y scaling and W/H `=1` hold at all depths.

The unplaced projected tube centers are `298.18`, `378.04`, and `387.69`
logical Y at near/middle/far. Adding +114 shifts each center by the same
amount, preserving the `89.51` logical near/far separation. The adapter adds
the placement only after projection (`AtariPresentationAdapter.cs:21, 116-125`).
The portrait camera maps logical Y to `1.26Y - 39.72`; +114 therefore adds
143.64 host pixels without changing width, height, or ratio. Placement does
not cause or correct the squareness.

Calculations are bounded to the source point extents, production depth
constants, baseline K/ZOffset, accepted placement, and uniform portrait zoom.
No image measurements or external data were used.

## Geometry versus presentation

The C# model is square: `322 x 322`. The already recovered Atari record 01
listed in the coordinate-mapping handoff has raw X/Y bounds `0x10..0xF0`,
therefore `224 x 224` in Atari coordinates (`AI-Logs/Reports/2026-11-15-levels2-6-atari-csharp-coordinate-mapping-report.md:172-174`).
Its approximate centered fit is uniform on X/Y, `1.436688312`, with an
independent-scale fit returning the same axes (`...coordinate-mapping-report.md:207-216`).
The fit is explicitly approximate, not proof of the C# literals' historical
provenance. No ROM file or disassembly was opened for this diagnostic.

Because both source frames are square but the accepted arcade/reference
observation is vertically rectangular, the reported difference is
presentation/aspect behavior, not a basis for modifying `LevelDefinition.Points`.

## Responsible stage

The path is:

1. **Immutable model points:** the 16 ordered square points are defined in
   `LevelLoader.cs:52-59`.
2. **Model-frame mapping:** the handoff's approximate Level 2 mapping is
   uniform. In the current adapter, the Level 2 branch reads/validates the
   mapping but directly projects `x` and `y`; the uniform fit is not applied
   there (`AtariPresentationAdapter.cs:42-53, 114-125, 185-190`). Applying a
   uniform mapping would still preserve a square.
3. **Depth projection:** both coordinates use the same denominator and
   reciprocal scale. The legacy projection shows this at
   `Projection.cs:64-65, 87, 112`; the Level 2 candidate uses the equivalent
   expressions at `AtariPresentationAdapter.cs:114-125`. This is the direct
   equal-axis shape-preserving stage.
4. **Placement:** +114 is a Y translation after projection, not an axis scale
   (`AtariPresentationAdapter.cs:21, 118-125`).
5. **Portrait camera and viewport:** the playfield camera uses uniform
   `PortraitPlayfieldZoom=1.26` and clips to the portrait viewport; it does not
   rescale X and Y independently (`DisplayConfiguration.cs:24-29, 44-58`;
   `VectorRenderer.cs:303`).

The first clean point in the presentation path for a rectangular result is
after depth projection and before +114 placement, scaling only the projected
Y displacement of each tube point from its depth-specific tube center. An
equivalent operation could scale model Y before depth projection while
leaving `yOffset` unscaled. Neither changes the depth denominator.

## Accepted-depth preservation

Let `f` be a future, manually calibrated vertical aspect factor, with `f>1`
for a taller tube. The shape-only form is equivalent to

```text
screenX = 400 + x*s
screenY = centerY + yOffset*s + f*y*s + 114
```

This preserves the existing depth denominator, horizontal scale, near/far
size ratio on each axis, and projected center at each depth. It changes the
Y derivative to `f*s`, making the resulting rim W/H `1/f`, while preserving
the near/far center separation and +114 placement. Rails remain straight and
share the same convergence point; their vertical slope changes with the
aspect factor, as required by a different screen-space proportion.

- **Before depth projection:** safe only when the factor applies to the model
  Y coordinate and not the existing vertical offset. Scaling the entire
  `(y + yOffset)` term would change tube-center progression.
- **After projection, before placement:** preferred; scale point Y about that
  depth's tube center, then add +114. This isolates aspect from depth and
  placement.
- **After placement, before the portrait camera:** a global stretch about a
  fixed screen origin would alter center positions/separation and could affect
  unrelated Level 2 presentation elements. A per-depth, tube-centered
  operation would be equivalent to the preferred seam, not a simpler
  alternative.
- **In the portrait camera:** changing camera axes would affect the whole
  playfield and alter vertical placement/center separation; it is not an
  isolated Level 2 correction.

No values for Position, Depth, K, ZOffset, +114, or shared -33.1 need to
change. The numeric factor is unknown and must not be fitted from unauthorized
images.

## Tube-attached objects

World anchors must follow the same Level 2 point transform to stay registered
to the altered tube. Existing shared paths project rim/tube points and player
(Blaster) endpoints through `ProjectWorldPoint`/`ProjectApproachGeometry`
(`VectorRenderer.cs:216-244, 1179-1184`). Lane/world anchors for Eggs, shots,
Flippers, Tankers, Spikers, Fuseballs, Pulsars, enemy bullets, Spikes, and
world effects likewise use `ProjectWorldPoint` or `ProjectLane`
(`VectorRenderer.cs:505-603, 616-633, 755-890, 909-918, 1041-1053, 1095-1112`).
Those anchors must receive the same presentation transform as the tube.

Local glyph dimensions are built separately in several render paths using a
scalar depth scale and screen-space offsets. A coherent full aspect treatment
would also apply its Y factor to local vertical dimensions/offsets of
tube-attached geometry where those dimensions are intended to follow the tube;
changing anchors alone does not stretch glyphs already constructed around a
projected center. This is a render-only consequence, not a reason to alter
gameplay geometry or redesign objects.

Keep intentionally screen-oriented vector artwork and portrait HUD/overlays
distinct. For example, the player-shot picture is explicitly described as
static and screen-oriented (`VectorRenderer.cs:1003-1038`); its world anchor
must move with the tube, but its art should not be distorted by assumption.
Portrait overlays use a separate HUD camera (`VectorRenderer.cs:396-414`) and
are not tube-attached.

## Relationship to Level 1

**Classification: same mechanism.** Equal X/Y projection preserves a circular
C# Level 1 tube as circular just as it preserves the Level 2 square as square;
the accepted observed reference difference is also vertical screen-space
elongation. Level 1 remains accepted and frozen. This diagnostic does not
authorize or recommend any Level 1 change.

## Candidate-correction feasibility

One Level 2-only, presentation-space Y aspect operation can transform both
near and far rims consistently while preserving their depth progression,
centers, placement, and world anchors. Applying the same operation to
tube-attached local geometry where appropriate can maintain visual
registration. Model points, lane identities/order, topology, gameplay, and
playback need not change. The correction's form is established; its magnitude
and final visual acceptance require manual calibration.

## Decision-relevant unknowns

1. **Numeric aspect factor / accepted target proportion.** It cannot be
   grounded without authorized visual calibration.
   - Could resolution change classification? **No.**
   - Could it change the eventual code layer? **No;** the isolated Level 2
     presentation seam remains the same.
   - Could it invalidate accepted placement/depth? **No,** if the operation
     remains tube-centered and excludes `yOffset`/placement.
   - Could it require model/gameplay geometry changes? **No.**
   - Could it materially change the next milestone? **No;** it supplies the
     candidate factor for that same bounded milestone.
2. **Exact historical provenance of the C# Level 2 literals.** The mapping
   handoff marks this undocumented, but its measured fit is uniform and
   approximate; resolving the history is not needed for this diagnosis.
   - Could resolution change classification? **No** for the measured current
     equal-axis projection and square bounds.
   - Could it change the code layer? **No** for the isolated presentation
     operation supported by current source and the handoff.
   - Could it invalidate accepted placement/depth? **No.**
   - Could it require model/gameplay geometry changes? **No.**
   - Could it materially change the next milestone? **No;** it is not a
     blocker and no further provenance investigation is recommended.

The renderer's distinction between world anchors and local screen-oriented
glyph dimensions is established by source, not an unresolved geometric
question. The bounded candidate should preserve the anchors and apply the Y
factor to attached local dimensions only where they are intended to follow the
tube; intentional screen-oriented artwork and HUD remain separate.

## Smallest next milestone

After this read-only report, implement one **opt-in, Level 2-only
presentation-aspect candidate** at the tube-centered post-projection /
pre-placement seam, with no numerical tuning from unauthorized images.
Validate invariants for the unchanged depth denominator, center locations,
near/far progression, +114 translation, and all tube-attached anchors; then
provide Neal the task-appropriate Level 2 manual visual check to select and
accept the factor. Keep Level 1 and all model/gameplay geometry unchanged.

## Scope and completion

- Prompt: `AI-Logs/Prompts/2026-11-15-level2-square-rectangle-diagnostic-prompt.md`
- Report: `AI-Logs/Reports/2026-11-15-level2-square-rectangle-diagnostic-report.md`
- Files created: exactly the authorized prompt and this report. Both are
  ignored AI-Logs artifacts; neither was staged or committed.
- Handoffs inspected: the three authorized Level 2 presentation, placement,
  and Levels 2-6 coordinate-mapping reports. No visual material, Level 1-9
  assets, raw ROM files, or disassembly were inspected.
- Source inspected: `LevelLoader.cs`, `LevelConfiguration.cs`, `Level.cs`,
  `Program.cs`, `Projection.cs`, `AtariPresentationAdapter.cs`,
  `DisplayConfiguration.cs`, `RomPresentationMetadata.cs`, and relevant
  `VectorRenderer.cs` projection/render paths.
- Calculations: near/middle/far scale, dimensions, ratios, centers, and host
  bounds from source constants and Level 2 point extents; no persistent
  calculation file was created.
- Starting Git state: branch `master`; HEAD
  `385f603a94391756b2ca6cccd524dc17b281e782`; upstream `origin/master`;
  branch tracking showed no ahead/behind divergence. Index empty. The
  pre-existing tracked modifications were:
  `src/Tempest.Game/Debug/DebugController.cs`,
  `src/Tempest.Game/Debug/PlaybackScreenshotCapture.cs`,
  `src/Tempest.Game/Levels/LevelLoader.cs`, `src/Tempest.Game/Program.cs`,
  `src/Tempest.Game/Rendering/DisplayConfiguration.cs`,
  `src/Tempest.Game/Rendering/Projection.cs`,
  `src/Tempest.Game/Rendering/VectorRenderer.cs`,
  `src/Tempest.Game/Systems/InputSystem.cs`,
  `tests/Tempest.Game.Tests/DebugControllerTests.cs`,
  `tests/Tempest.Game.Tests/DisplayConfigurationTests.cs`,
  `tests/Tempest.Game.Tests/GameOverDensityDiagnosticTests.cs`,
  `tests/Tempest.Game.Tests/GameplayRecordingTests.cs`,
  `tests/Tempest.Game.Tests/InputSystemTests.cs`,
  `tests/Tempest.Game.Tests/Level9ProductionTests.cs`,
  `tests/Tempest.Game.Tests/PlaybackScreenshotCaptureTests.cs`, and
  `tests/Tempest.Game.Tests/TransitionPresentationTests.cs`.
  Pre-existing untracked paths were
  `Reference/arcade/MBUCOD.V05.txt`,
  `Reference/arcade/MBUDOC.DOC`,
  `src/Tempest.Game/Levels/RomPresentationMetadata.cs`,
  `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`,
  `tests/Tempest.Game.Tests/AtariPresentationAdapterTests.cs`,
  `tests/Tempest.Game.Tests/RomPresentationMetadataTests.cs`, and
  `tests/Tempest.Game.Tests/VectorRendererAtariPresentationTests.cs`. One
  additional uninspected local visual asset was present and is intentionally
  not named or examined.
- Ending Git state: same branch, HEAD, upstream, tracked modifications,
  untracked state, and empty index. No tracked file changed; the index remained
  unchanged. Nothing was staged, committed, or pushed.
- Validation: no build or test run was required for this read-only
  investigation.
- Diagnostic classification: **MISSING PRESENTATION ASPECT TRANSFORM**.
- Most important remaining decision-relevant unknown: the manually accepted
  numeric vertical aspect factor.
