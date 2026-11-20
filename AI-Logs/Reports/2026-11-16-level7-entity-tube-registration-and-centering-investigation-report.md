# Level 7 entity-to-record-06 registration and centering investigation

Date: 2026-11-16
Status: **READ-ONLY INVESTIGATION COMPLETE; implementation not performed**

## Direct answers

1. **Yes.** During ordinary Level 7 play, the visible tube is built from Atari
   record 06, but the gameplay objects are still positioned from the production
   C# clover geometry. The two paths do not share endpoint interpolation.
2. `DrawTube` dispatches the tube to `DrawAtariLevel7PresentationTube`, which
   builds record 06 and maps its near/far endpoints through
   `AtariLevel7Presentation.MapNear/MapFar`. Entity helpers instead use
   `ProjectWorldPoint` / `ProjectLane` / entity-specific helpers. With the
   requested flag active, those call `AtariPresentationAdapter.Project` for
   Level 7, using `LevelDefinition.Points`, model center/offset and depth.
   The same final playfield camera then transforms both outputs.
3. **Yes.** A render-only adapter can consume existing lane, edge-position,
   depth/height, transition and animation state, interpolate the mapped
   record-06 endpoint sets, and draw the existing vectors without changing
   simulation or collision geometry.
4. The record-06 mapping still has its prototype origin `(300,400)`, but the
   production renderer now draws it through the playfield camera, not the
   prototype HUD camera for which that origin was used. This leaves the tube
   visibly left of center and centered near the viewport center vertically
   instead of the available gameplay-area center below the HUD.

The manual description of a separate “invisible playable tube” is a
**MANUAL DIAGNOSTIC HYPOTHESIS**. The C# paths and bounded coordinate
calculations confirm the underlying split registration; they do not prove
what was visible in the unavailable failure screenshots.

## Scope and evidence

### Authorized images

- Inspected only
  `UserData/Screenshots/Level-7/Level7_frame_0090-MAME.png`. It shows the
  blue clover composed beneath the HUD in a centered portrait layout. It was
  used only as compositional evidence; no coordinates were fitted from it.
  Its filename was treated only as the supplied reference identifier. No
  MAME implementation, settings, scaling or rendering was investigated.
- The three authorized manual-failure filenames
  `live-level-7-tick-000280-001.png`,
  `live-level-7-tick-000373-001.png`, and
  `live-level-7-tick-000015-001.png` were not found in the authorized
  `TEMP/Level-7` or `UserData/Screenshots/Level-7` areas. No other visual
  asset was opened or analyzed.
- The manual observations in this report are therefore those supplied in the
  task: gameplay works; Tanker/Flipper, Eggs, starting Spikes, Blaster and
  other vectors appear associated with a different tube; the record-06 shape
  looks substantially right but is misplaced. These are **MANUAL
  OBSERVATIONS** as reported, not image measurements performed here.

### Evidence classifications

- Record selection and record-derived tube data: **ROM-BACKED** as classified
  in the existing catalog and project reports.
- The recovered record projector, vector builder and their C# dispatch:
  implementation-established facts. Their mapping to current screen space
  remains a **PROJECT TRANSLATION**, not a claim about Atari monitor
  calibration.
- The Level 7 entity-to-screen path and the mismatch calculated below:
  implementation-established facts.
- The proposed shared centering translation and render-only adapter:
  **INFERENCE / implementation candidate**, not accepted behavior.
- No new arcade behavior or gameplay behavior was established.

## Repository starting state and preservation

- Branch: `master`
- HEAD: `385f603a94391756b2ca6cccd524dc17b281e782`
- Upstream/tracking: `origin/master`
- Index: empty
- The first status snapshot contained 17 modified tracked paths and a tracked
  diff of 2,943 insertions / 177 deletions. The modified paths were:
  `.github/copilot-instructions.md`,
  `src/Tempest.Game/Debug/DebugController.cs`,
  `src/Tempest.Game/Debug/PlaybackScreenshotCapture.cs`,
  `src/Tempest.Game/Levels/LevelLoader.cs`,
  `src/Tempest.Game/Program.cs`,
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
- That first status also contained the existing untracked Atari-presentation
  candidates and tests:
  `AtariTubeRecordCatalog.cs`, `RomPresentationMetadata.cs`,
  `AtariCoordinateProjector.cs`, `AtariDvgViewportMap.cs`,
  `AtariPresentationAdapter.cs`, `AtariTubeReferenceShapeDiagnostic.cs`,
  `AtariTubeVectorListBuilder.cs`, `AtariPresentationAdapterTests.cs`,
  `AtariTubeRecordPrototypeTests.cs`, `RomPresentationMetadataTests.cs`, and
  `VectorRendererAtariPresentationTests.cs`, plus the unrelated protected
  `Reference/arcade/MBUCOD.V05.txt`, `Reference/arcade/MBUDOC.DOC`, and
  `screenshot000.png`.
- **Concurrent-state discrepancy:** `AtariLevel7Presentation.cs` existed and
  was read for this investigation, but it did not appear in the first
  `git status` output. A later status showed it as untracked. The investigation
  did not create or edit it. It is preserved; its appearance between status
  snapshots is recorded rather than attributed to this work.
- No source, tests, configuration, durable documentation, protected assets,
  staging area, or commits were intentionally changed. The required prompt
  and this report are the only files written by this investigation. The
  concurrent appearance above means the repository-wide no-change condition
  cannot be certified beyond the captured status snapshots.

## Current tube path

For `--atari-presentation --level 7` in `Playing`, `VectorRenderer.Draw`
chooses the experimental portrait playfield camera and draws the tube and
entities in the same `BeginMode2D`/scissor scope. The Level 7 tube dispatch
uses:

1. `AtariTubeRecordCatalog.Level7Record06`, independent of
   `LevelDefinition.Points`.
2. `AtariTubeVectorListBuilder.Build`, which uses the unchanged
   `AtariCoordinateProjector` to create 16 near points at runtime position 16
   and 16 far points at position 240, with same-index rails and cyclic rims.
3. `AtariLevel7Presentation.MapNear/MapFar` for those endpoint sets.
4. `DrawAtariLevel7PresentationTube`, which draws the mapped record rails and
   rims and uses the gameplay lane index directly for player-lane highlighting
   and pulsing.
5. The common `ExperimentalPortraitPlayfieldCamera` and portrait viewport.

The accepted record-06 source, topology, point order, near/far pairing,
projector and shape mapping were not reopened or changed.

## Current gameplay-entity paths

The current flag does **not** make entity helpers call
`AtariLevel7Presentation.MapNear/MapFar`. The relevant Level 7 model path is:

```text
LevelConfiguration.Level7Production
  -> LevelLoader.LoadLevel7Clover (16 C# points; centerY=310, yOffset=-10)
  -> entity lane/edge/depth position from gameplay state
  -> ProjectWorldPoint
  -> AtariPresentationAdapter.Project (Level 7 model-frame path)
  -> common playfield camera
```

In the opt-in candidate, `ProjectWorldPoint` selects
`AtariPresentationAdapter.Project` because Level 7 has a model-frame mapping.
It does not call raw `Projection.Project` for ordinary in-level candidate
entities. With presentation disabled, the legacy fallback is
`Projection.Project`. The active Level 7 adapter still consumes the production
clover points, Level 7 `CenterY`/`YOffset`, Level `Position`/`Depth`, the Level
7 metadata, and the adapter's approximate Level 7 model-frame scale and
reciprocal-depth projection. It does not consume the record-06 `Near[i]` and
`Far[i]` arrays.

The two outputs share the **same final camera**, so the root cause is not
separate final cameras. They differ before the camera:

| Property | Record-06 tube | Gameplay entities |
|---|---|---|
| Geometry source | ROM-backed Atari record 06 | `LevelDefinition.Points` for the C# clover |
| Depth representation | Two DVG endpoint sets, then separate near/far maps | Continuous model depth and model-point projection |
| Position mapping | `AtariCoordinateProjector` -> DVG points -> `AtariLevel7Presentation` | `AtariPresentationAdapter.Project` |
| Horizontal/vertical transform | Shared calibrated aspect/scale; separate far-set offset | Adapter's model-frame reciprocal-depth projection and local Jacobian |
| Record lane interpolation | Yes, for the tube only | No |
| Final camera | Shared experimental portrait playfield camera | Same camera |

### Per-class tracing

- **Blaster/player:** `GetPlayerEdge` projects the production model vertices at
  `PlayerProjectedDepth`. `DrawPlayer` derives the anchor, edge tangent and
  perpendicular claw basis from those projected endpoints. Claw height is
  normally 20 at the rim and uses the model adapter scale during approach or
  exit; it is not derived from the record map. The Level 7 local
  presentation-offset switch adds no Level 7 transform. Its position,
  orientation, basis and local size therefore remain model-derived, not
  visible-record-derived.
- **Ordinary Flippers and Tanker-released Flippers:** both are in
  `game.Flippers` and share `DrawFlipper`. Normal lane endpoints come from
  `Definition.Points`; pivot endpoints come from
  `CalculateFlipperPivotEndpoints` on the same model geometry; both are
  projected with `ProjectWorldPoint`. `DrawFlipperShape` derives its tangent
  and normal from those projected endpoints. `IsTankerCargo` does not select
  another renderer. Both position and orientation are model-derived.
- **Tankers:** `ProjectTankerCenter` calls `ProjectLane`, which takes the
  midpoint of adjacent production points and projects it at
  `Position + Depth - Height`. The existing diamond/lattice is screen-axis
  oriented; its anchor and scale use the model projection, not record lane
  geometry.
- **Eggs:** `ProjectEgg` takes the production-point lane midpoint and uses
  `Position + Depth - Egg.Height * 20`. A new Egg starts at height `-10`, so
  its representative projected depth is 650, beyond the far depth 450.
  The red 3-pixel marker is screen-oriented and uses the model projection for
  its anchor.
- **Spikers:** `ProjectLane` supplies the model-derived anchor and the adapter
  supplies local scale. The spiral rotation comes from the gameplay
  `Spiker.Rotation` and is rendered in screen axes; it is not aligned to the
  visible record lane.
- **Spikes, including starting Spikes:** `DrawSpikes` projects the lane
  midpoint at the far model depth and again at `farDepth - Spike.Height`.
  Starting height 60 means depths 450 to 390. Its line direction is therefore
  the model projection's depth direction, not the mapped record rail.
- **Player shots:** `ProjectShot` takes the model lane midpoint at
  `Position + Depth - Shot.Height`. The existing point-group art is
  screen-oriented; its position and local scale are model-adapter derived.
- **Enemy shots:** `ProjectEnemyBullet` uses the model lane midpoint at
  `Position + Depth - Bullet.Height`. The existing vector program is
  screen-oriented; its center and scale are model-adapter derived.
- **Enemy destruction effects:** `DrawEnemyDestructionEffects` uses
  `ProjectLane` and the model adapter's scale for its vector offsets.
- **Player destruction effects:** `DrawPlayerExplosion` anchors from
  `GetPlayerEdge`, hence from projected production-model endpoints; its
  starburst layers remain screen-space artwork.
- **Rim/lane markers:** the tube's player highlight and pulsing-lane rim
  dispatch operate on record endpoint indices, but compare them directly to
  gameplay `EdgeIndex`. Pulsing lane rails and Pulsar/Fuseball geometry use
  the production points and model projection. Pulsars and Fuseballs are not
  normal Level 7 composition, but if rendered their anchors/bases still do not
  use record-06 endpoints.

Thus a render-only adapter must handle more than a center point. The Blaster
and Flipper bases/orientation must come from the corresponding mapped lane
geometry; Spikes need a mapped depth-direction segment; Tankers, Spikers,
shots and effects retain their existing vector art and animation but need
mapped anchors and appropriate visible-geometry local scale. Eggs are
screen-oriented markers; they need mapped anchors and a deliberate below-far
depth policy.

## Bounded registration calculations

The following calculations reproduce the current constants and formulas,
rounded to 0.01 logical or host pixels. They are not screenshot-derived.
`LevelDefinition.Points[0]` and `[1]` are `(-160,160)` and `(-179,44)`;
the gameplay lane-0 center is their midpoint `(-169.5,102)`. For comparison,
the visible record lane-0 center is the midpoint between mapped record
endpoints 0 and 1 at each end, interpolated by
`t=(projectedDepth-50)/400`. The table therefore tests the current implicit
identity lane mapping; it does not establish that lane 0 is semantically the
same Atari lane.

Host coordinates apply the common camera (`Zoom=1.26`,
`Target=(400,412.476)`, `Offset=(640,480)`) to both logical draw coordinates.
The discrepancy is record-target minus current-entity position.

| Probe | Gameplay lane/depth | Model points / state | Current entity logical -> host | Record-06 lane/depth logical -> host | Host discrepancy (dx,dy); distance |
|---|---|---|---|---|---|
| Rim center | lane 0, depth 50 (`t=0`) | midpoint `(-169.5,102)` | `(199.68,374.08)` -> `(387.60,431.62)` | `(405.97,177.13)` -> `(647.52,183.46)` | `(+259.92,-248.16); 359.36` |
| Upper tube | lane 0, depth 150 (`t=.25`) | midpoint `(-169.5,102)` | `(316.53,282.88)` -> `(534.83,316.71)` | `(384.18,236.00)` -> `(620.07,257.64)` | `(+85.24,-59.07); 103.70` |
| Mid-depth | lane 0, depth 250 (`t=.5`) | midpoint `(-169.5,102)` | `(347.28,258.88)` -> `(573.58,286.47)` | `(362.40,294.87)` -> `(592.62,331.82)` | `(+19.04,+45.35); 49.19` |
| Deep end | lane 0, depth 450 (`t=1`) | midpoint `(-169.5,102)` | `(369.65,241.42)` -> `(601.76,264.47)` | `(318.83,412.61)` -> `(537.72,480.17)` | `(-64.03,+215.70); 225.00` |
| Starting Spike tip | lane 0, depth 390 (`t=.85`) | midpoint `(-169.5,102)`; Spike height 60, base at depth 450 | `(365.22,244.88)` -> `(596.18,268.83)` | `(331.90,377.29)` -> `(554.19,435.67)` | `(-41.99,+166.84); 172.04` |
| Initial Egg | lane 0, depth 650 (`t=1.5`) | midpoint `(-169.5,102)`; Egg height `-10`; record endpoints extrapolated beyond far | `(378.69,234.37)` -> `(613.15,255.58)` | `(275.26,530.35)` -> `(482.83,628.53)` | `(-130.32,+372.94); 395.06` |

The magnitude and depth-dependent direction changes demonstrate that adding
one entity-only constant offset would not register gameplay over the tube.
The visible record position needs to be evaluated from the entity's lane and
depth against the mapped record near/far geometry. The Egg row deliberately
uses unbounded linear endpoint extrapolation solely to show the current
initial below-far state; clamping the presentation at the far endpoint is an
unresolved visual choice and must not mutate Egg height or simulation state.

The old model path is not bounded to the HUD-cleared playfield. Projecting
all 16 production near-rim vertices through the current camera yields host Y
`13.19..546.28`, while the persistent HUD bottom is `165.6`; therefore
some model-rim positions can occupy the HUD band. The sampled starting-Spike
lane midpoints at depth 390 span host Y `198.64..286.28`, and the default
Egg lane midpoints at depth 650 span `212.57..266.27`, both below that HUD
boundary. This establishes that the old transform can place objects outside
the intended gameplay composition for some positions, not that any specific
object in the unavailable manual screenshots did so. The renderer's
portrait scissor clips to the full 960-pixel viewport, not to the
HUD-cleared gameplay rectangle.

## Lane identity and registration design

- Both production Level 7 and record 06 contain 16 closed points. The Level 7
  entity code interprets lane `i` as the edge between model points `i` and
  `NextEdge(i)`; the record builder's tube lane is the corresponding edge
  between record endpoints `i` and `(i+1) mod 16`.
- The current tube renderer directly compares gameplay lane indices to
  record rail indices for highlighting/pulsing. This is an **identity
  assumption**, not an adapter or tested model-to-record correspondence.
- The C# model-frame mapping does not reindex the C# point list. The record
  builder preserves record indices. Their displayed polygon winding has the
  same signed orientation under the current transforms; no global reversal is
  evident. This does not establish the index-zero phase or that model edge
  `i` is physically record edge `i`.
- No explicit lane correspondence table exists. The implementation should
  make identity correspondence explicit as the initial candidate because
  that is what the current highlighter already assumes, but it must not claim
  it verified. A non-identity table is not justified by current source
  evidence; if later evidence disproves identity, add a small explicit
  mapping rather than changing gameplay indices or model topology.
- The necessary interpolation variable is the existing projected depth
  derived from model height: `z=Position+Depth-height`, normalized against
  `Position` and `Position+Depth`. At normal play this gives `t=0` at the
  near rim and `t=1` at the far web. A lane edge's record near/far midpoint
  can be interpolated at `t`; the Blaster's edge position is an additional
  coordinate along the near-rim edge.
- Starting Spikes naturally use the far endpoint (`t=1`) and their current
  height-derived tip (`t=.85` for height 60). Deep Eggs require `t>1` to
  preserve their current “below the web” state. The visible adapter should
  preserve that model depth as render input; whether the resulting marker
  should extrapolate beyond the far rim or visually cap at the far region
  requires later manual acceptance.
- A render-only adapter can read the existing gameplay state and mapped
  record endpoint sets. It need not write simulation fields, collision
  geometry, RNG, score, timing, recording or playback state. With no
  simulation mutation, recording/playback determinism is not at risk; the
  visual output changes only when the opt-in presentation is active.

## Tube centering calculation

The current shared map's origin/translation is inherited from
`AtariDvgViewportMap.DevelopmentOnlyProvisional`: `(300,400)` in the
600x800 portrait logical dimensions. After the accepted common mapping and
far-set translation, its mapped origins are near `(300,400)` and far
`(333.75,446.25)`. The 32 visible near/far endpoint coordinates have:

| Space | Bounds | Bounding-box center |
|---|---|---|
| Mapped logical draw coordinates | X `131.06..474.00`, Y `166.09..633.91` | `(302.53,400.00)` |
| Host output through current playfield camera | X `301.14..733.23`, Y `169.55..759.01` | `(517.19,464.28)` |

The tested portrait viewport is 720x960, centered in the 1280x960 host window.
The accepted composition test uses a persistent HUD bottom of
`(120+18)*1.2 = 165.6` host pixels. Centering the complete tube bounds in the
remaining vertical playfield gives:

```text
target host center = ((165.6 + 960)/2) = 562.8
target horizontal center = 1280/2 = 640
current host center = (517.19,464.28)
```

The exact shared post-map translation for bounding-box centering is therefore
approximately:

```text
host output:       (+122.81, +98.52)
logical draw space: (+97.47,  +78.19)  [divide by camera zoom 1.26]
```

Applied to all mapped record endpoints after the accepted shape transform,
the resulting host bounds are approximately X `423.95..856.05`,
Y `268.07..857.53`. This retains the complete geometry, near/far relationship
and far-set offset. It produces equal left/right margins and equal top/bottom
margins within the available area below the HUD.

The exact cause is a coordinate-origin/camera mismatch. The direct Atari
prototype uses the portrait HUD camera, which maps the map origin `(300,400)`
to host center `(640,480)`. The ordinary Level 7 presentation candidate runs
inside the playfield camera instead: its logical center is X=400 and its
vertical target includes the shared `-33.1` portrait-playfield translation.
The candidate continues to use the prototype's `(300,400)` origin in that
world space. The same camera does not by itself center the record map; nor
does it account for the HUD-reserved composition area. This is a placement
issue, not a reason to change tube shape, gain, or far-set translation.

The reference image supports the composition target qualitatively. The
calculated translation is a future **PROJECT TRANSLATION** candidate, not an
accepted final value. It is suitable for a bounded implementation prompt,
followed by Neal's manual visual acceptance.

## Required follow-up boundaries

- Smallest entity correction: add a Level-7-only rendering adapter that maps
  lane/edge-coordinate and projected depth onto the mapped record-06 near/far
  endpoints. Route only Level 7 `Playing` presentation through it; retain
  gameplay/collision geometry and all simulation state.
- The adapter must address the Blaster and Flipper local tangent/normal from
  visible mapped lane geometry, not only their anchors. Preserve their
  current shapes and gameplay animations. Retain the screen-oriented existing
  glyphs for Tankers, Spikers, Eggs and shot/effect vectors while updating
  their anchors and visible-geometry local scale. Map Spike endpoints along
  the visible lane's depth direction. Handle Eggs' `t>1` as an explicit
  rendering policy.
- Pulsing-lane/player-rim markers need the same lane mapping as entities.
  Fuseballs/Pulsars are not normal Level 7 entities, but their optional
  render paths also use model geometry and would need the adapter if those
  scenarios are included.
- Smallest centering correction: add the common logical translation
  `(97.47,78.19)` after the accepted record-06 shape transform and before
  final camera mapping, subject to manual confirmation. Do not change any
  shape parameters.
- No gameplay change, recording/playback change, RNG change, timing change,
  or collision change is justified.
- Enough evidence exists for a bounded implementation prompt covering the
  render-only adapter and shared placement translation. Lane identity and
  below-far Egg display policy must be explicitly tested and manually
  accepted; neither is upgraded to an established behavior here.
- Record 06 and the accepted tube-shape mapping remain frozen. No shape,
  source-byte, order, connectivity, projector, gain or far-offset
  investigation was repeated.

## Validation and closeout

- Source/render path trace and bounded formula calculations: **PASS**.
- Build/tests: not run; this was a read-only investigation and no code changed.
- Interactive game: not launched.
- Manual Level 7 failure screenshots: unavailable in the authorized folders.
  The supplied reference image was the only image inspected.
- No implementation was made. A future visual candidate still requires
  Neal's manual gameplay/presentation acceptance.
- Saved prompt:
  `AI-Logs/Prompts/2026-11-16-level7-entity-tube-registration-and-centering-investigation-prompt.md`
- Saved report:
  `AI-Logs/Reports/2026-11-16-level7-entity-tube-registration-and-centering-investigation-report.md`
