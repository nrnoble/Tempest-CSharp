# Atari Presentation Adapter Implementation Plan

Date: 2026-11-15
Status: READ-ONLY PLANNING COMPLETE

## Executive decision

**READY FOR BOUNDED IMPLEMENTATION**

The renderer has a render-only shared world-point seam, and a continuous
per-level transform can be isolated there without changing simulation or stored
geometry. The first candidate can be limited to Levels 1, 7, 8, and 9, whose
model-frame conversions are represented in the controlling mapping work. Levels
2-6 have ROM presentation bytes but no established C# model-frame conversion;
they must use an explicitly reported legacy fallback, never another level's
mapping. This is a scope boundary, not a blocker to the bounded candidate.

**Readiness is not arcade-fidelity acceptance.** Neal's manual visual/gameplay
acceptance remains required after implementation.

## Controlling evidence

### Reports consulted

- [Presentation-layer Atari transform feasibility](./2026-11-15-presentation-layer-atari-transform-feasibility-report.md) — controlling architectural handoff; selected `FEASIBLE WITH EXPLICIT LIMITATIONS`.
- [Atari Math Box `$c098` reconciliation](./2026-11-15-atari-mathbox-c098-reconciliation-report.md) — continuous equation and limits of exact retained-state reconstruction.
- [Atari versus C# projection comparison](./2026-11-15-atari-csharp-projection-comparison-report.md) — established C# projection and non-equivalence limits.
- [Atari-to-C# coordinate-frame mapping](./2026-11-15-atari-csharp-coordinate-frame-mapping-report.md) — record associations, known frame factors, and Level 7's comparison-only rotation.

The feasibility handoff references the Level 7 geometry-provenance disposition.
That report was not reopened; its bounded conclusion is used only through the
controlling handoff. No new Atari archaeology or external research was done.

### Current C# sources examined

- [Projection.cs](../../src/Tempest.Game/Rendering/Projection.cs)
- [VectorRenderer.cs](../../src/Tempest.Game/Rendering/VectorRenderer.cs)
- [DisplayConfiguration.cs](../../src/Tempest.Game/Rendering/DisplayConfiguration.cs)
- [Program.cs](../../src/Tempest.Game/Program.cs)
- [DebugController.cs](../../src/Tempest.Game/Debug/DebugController.cs)
- [RomPresentationMetadata.cs](../../src/Tempest.Game/Levels/RomPresentationMetadata.cs) (pre-existing untracked file)
- [RomPresentationMetadataTests.cs](../../tests/Tempest.Game.Tests/RomPresentationMetadataTests.cs) (pre-existing untracked file)
- [LevelDefinition.cs](../../src/Tempest.Game/Levels/LevelDefinition.cs)
- [LevelLoader.cs](../../src/Tempest.Game/Levels/LevelLoader.cs) (pre-existing modified file)
- [LevelConfiguration.cs](../../src/Tempest.Game/Levels/LevelConfiguration.cs)
- [LevelApproach.cs](../../src/Tempest.Game/Levels/LevelApproach.cs)
- [TransitionStarfield.cs](../../src/Tempest.Game/Rendering/TransitionStarfield.cs)
- [PlaybackScreenshotCapture.cs](../../src/Tempest.Game/Debug/PlaybackScreenshotCapture.cs)
- [InputSystem.cs](../../src/Tempest.Game/Systems/InputSystem.cs)

Relevant existing test surfaces were located in `DebugControllerTests`,
`DisplayConfigurationTests`, `RomPresentationMetadataTests`,
`BlasterLaneHighlightTests`, `PlayerShotRenderingTests`,
`Level1ProductionEnemyBulletRenderingTests`, `EnemyDestructionEffectTests`,
`TransitionPresentationTests`, `PlaybackScreenshotCaptureTests`,
`GameplayRecordingTests`, `Level7ProductionTests`, `Level8ProductionTests`,
and `Level9ProductionTests`.

### Evidence classifications

- **CONTROLLING-HANDOFF-BACKED:** `VectorRenderer.ProjectWorldPoint` is the
  earliest common render-only seam; Atari's continuous reciprocal-depth
  equation, known mappings, Level 7 limitation, and glyph/Game Over caveats are
  recorded in the handoffs.
- **C#-SOURCE-BACKED:** `ProjectWorldPoint` passes render-only Approach offsets
  into the sole `Projection.Project` call in the renderer; it does not write
  geometry or entity state. `LevelDefinition` retains ordered points and its
  `Continuous` topology. `RomPresentationMetadataCatalog` holds raw metadata
  for displayed Levels 1-9. `Program` sets `Projection.K=70` and
  `Projection.ZOffset=5`, wires presentation mode, and owns interactive
  playback/screenshot control.
- **IMPLEMENTATION-PLAN DECISION:** Keep `Projection.Project` as the unchanged
  legacy implementation and opt into a separate adapter only at
  `ProjectWorldPoint`.
- **MANUAL-ACCEPTANCE REQUIREMENT:** Screen appearance, registration as
  experienced in play, and preservation of accepted shots/effects require
  Neal's review.
- **UNKNOWN:** Exact DVG-to-logical pixel calibration, the future visual
  calibration of Level 7's approximate model mapping, and literal retained
  Math Box state are not established by this continuous candidate.

## Proposed architecture

Add a stateless `AtariPresentationAdapter` in
`src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`. Keep a separate,
explicit model-frame mapping table in that candidate implementation; do not
relabel or rewrite ROM metadata to make it look like model-frame evidence.

Select it from `VectorRenderer.ProjectWorldPoint` only when an explicit
development option enables the candidate and the current level has both valid
ROM presentation metadata and an established model-frame mapping. Do not replace
arithmetic inside `Projection.Project`, pre-transform points before calling it,
or put candidate selection into global mutable projection state. With the
candidate disabled, route through the existing `Projection.Project` call with
the same values and near/far context.

Adapter inputs:

- immutable model `(x,y)` and the existing render depth;
- current `LevelDefinition.CenterY` and `YOffset`;
- `LevelConfiguration.Position` and `Depth`;
- current render-only `ApproachDepthOffset` and `ApproachVerticalOffset`;
- `Projection.K` and `ZOffset`;
- this displayed level's existing ROM metadata;
- a separately keyed frame mapping `(u,e)` for supported levels.

Outputs:

- finite logical-screen `Vector2` for the supplied point;
- finite local logical scale (equivalently, a diagonal 2D Jacobian) at that
  depth for registered glyph construction.

Do not return or mutate a model point, entity, level definition, gameplay
depth, lane, or topology. Reuse existing exception/error conventions for invalid
numeric data; never silently replace invalid data with zero or a neighboring
level's values.

## Mathematical contract

Definitions:

- `u`: C# model units per Atari raw coordinate unit from the separate,
  level-keyed frame mapping.
- `e`: model-to-Atari Y orientation sign, exactly `+1` or `-1`.
- `v = Definition.YOffset + ApproachVerticalOffset`.
- `s = metadata.ScaleRaw`; `r = metadata.CameraHeightRaw`;
  `Y2D = metadata.ScreenYOffsetSigned`.
- `zNear = Position + ApproachDepthOffset`.
- `zFar = Position + Depth + ApproachDepthOffset`.
- `z = inputDepth + ApproachDepthOffset`.

First convert the immutable model point to a temporary Atari-like frame:

```text
Xraw = 128 + x/u
Yraw = 128 + e*(y+v)/u
dX   = Xraw - 128 = x/u
dY   = Yraw - r
```

`CenterY` remains a host/logical vertical anchor. `v` remains the current
render-only C# model-unit translation; it is not the signed Atari Y2D value.

Normalize depth by the existing tube interval without clamping:

```text
t = (z-zNear)/(zFar-zNear)
p = 16 + 224*t
```

Near, midpoint, and far correspond to `p=16,128,240`; this is an
endpoint-normalized correspondence, not a claim that C# depth is a literal
Atari runtime `$57` value. Outside the interval, extrapolate the continuous
equation. This is not a claim about Atari runtime behavior outside the sampled
interval.

Use the recovered reciprocal form continuously:

```text
Q(d,p,s) = 256*d/(p+s)
lambda    = u*K*(16+s)/(256*(zNear+ZOffset))
QyOrigin  = Q(e*v/u + 128-r, 16, s)
B         = CenterY + v*K/(zNear+ZOffset) - e*lambda*QyOrigin
screenX   = 400 + lambda*Q(dX,p,s)
screenY   = B + e*lambda*(Y2D + Q(dY,p,s))
```

The `400` center is the current logical width midpoint. `lambda` is anchored
to the current C# near projection and is data-derived, not an aesthetic
constant. `B` retains the model-origin near anchor before the signed ROM Y2D
placement. Y2D is applied after reciprocal projection. The existing portrait
camera then maps logical output to the host window; the shared `-33.1`
placement remains in that later camera stage.

At fixed depth this transform is affine with no shear. Its model-space local
Jacobian is diagonal and isotropic:

```text
Jxx = Jyy = lambda*256/(u*(p+s))
Jxy = Jyx = 0
```

Return this local logical-units-per-model-unit value. Do not use the old
`K/(z+ZOffset)` scale in candidate glyph paths. Preserve existing per-glyph
base dimensions and fixed pixel floors, replacing only the depth-responsive
factor. The portrait camera's existing uniform zoom acts after these logical
dimensions.

Validate finite coordinates/offsets, positive finite `u`, `e` in `{-1,+1}`,
finite `K>0`, finite `ZOffset`, finite and nonzero depth interval, finite
`zNear+ZOffset`, finite normalized depth and `p`, finite nonzero `p+s`, and
finite output/Jacobian. Do not clamp out-of-interval depths. A zero or
non-finite denominator is an explicit error. Valid negative extrapolated
denominators remain algebraic extrapolation, not an Atari claim.

Intentionally omitted in the first candidate:

- Atari integer floor/quantization and signed-16 saturation: the continuous
  equation is sufficient for a coherent visual candidate; these matter for
  bit-/pixel-level comparisons. They can be tested later with isolated
  arithmetic vectors, without changing simulation.
- Bit-exact Math Box retained state: the handoff does not establish all
  retained state; it is unnecessary for the continuous endpoint-normalized
  transform. A later hardware-faithful comparison can test controlled states.
- Literal DVG-to-host-pixel calibration: no measured conversion is established.
  The near-anchor normalization avoids arbitrary scaling for this first
  candidate; Neal's manual review determines whether later calibration is
  warranted.
- Level 7 point-for-point Atari equivalence: applying the same arithmetic to
  its unchanged clover is defined, but the residual remains. Do not rotate
  points by the comparison-only `+5` or claim visual equivalence.

## Exact implementation surface

### Files proposed for modification

1. **New** `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs` — owns the
   continuous transform, returned local scale, and a separate explicit mapping
   table for the supported model frames. No simulation references or mutation.
2. **`src/Tempest.Game/Rendering/VectorRenderer.cs`** — select the adapter at
   `ProjectWorldPoint`; route registered anchors/endpoints consistently; use
   its local scale for only depth-scaled world-anchored glyph dimensions; keep
   Game Over vectors on the legacy path.
3. **`src/Tempest.Game/Debug/DebugController.cs`** — parse an explicit
   development-only `--atari-presentation` switch, reject duplicates and
   incompatible projection options, and permit the presentation-only switch
   with interactive file recording/playback without serializing it.
4. **`src/Tempest.Game/Program.cs`** — validate the option's presentation
   context, wire it into the renderer, reject use in headless playback analysis
   and the standalone input diagnostic, and visibly identify the candidate as
   development-only.
5. **New** `tests/Tempest.Game.Tests/AtariPresentationAdapterTests.cs` — focused
   math, per-level data, validation, determinism, and local-Jacobian tests.
6. **New** `tests/Tempest.Game.Tests/VectorRendererAtariPresentationTests.cs`
   — renderer integration, registration, glyph-scale, Approach, and
   non-mutation tests without framebuffer pixel snapshots.
7. **`tests/Tempest.Game.Tests/DebugControllerTests.cs`** — option presence,
   duplicate/error, exclusivity, and file-mode composition tests.
8. **`tests/Tempest.Game.Tests/DisplayConfigurationTests.cs`** — assert the
   unchanged disabled legacy path and that the existing `-33.1` camera
   translation remains after logical projection.

The existing `RomPresentationMetadataTests.cs` already checks all nine
catalog entries, signed Y2D decoding, invalid topology encoding, unsupported
levels, and agreement with production topology. Reuse it as a prerequisite
regression; do not rewrite raw metadata. Existing renderer, transition,
playback, and screenshot tests can be extended only where the new mode needs an
integration assertion; do not broadly alter those tests.

### Important files explicitly excluded

- `src/Tempest.Game/Rendering/Projection.cs`: preserve the accepted current
  projection and portrait diagnostic as the legacy implementation.
- `src/Tempest.Game/Rendering/DisplayConfiguration.cs`: keep the cameras,
  portrait output scale, and frozen `-33.1` placement unchanged.
- `src/Tempest.Game/Levels/RomPresentationMetadata.cs`: raw ROM metadata for
  Levels 1-9 is already present; keep model-frame factors separate.
- `src/Tempest.Game/Levels/LevelLoader.cs`,
  `src/Tempest.Game/Levels/LevelConfiguration.cs`, and
  `src/Tempest.Game/Levels/LevelDefinition.cs`: no point, order, coordinate,
  topology, level, or gameplay edits.
- `src/Tempest.Game/Levels/LevelApproach.cs` and
  `src/Tempest.Game/Rendering/TransitionStarfield.cs`: preserve deterministic
  transition timing/sequence and independent stars.
- `src/Tempest.Game/Debug/PlaybackScreenshotCapture.cs`,
  `src/Tempest.Game/Systems/InputSystem.cs`, recording formats, simulation,
  collision, and gameplay systems: presentation selection must not enter input,
  capture identity, playback state, or model state.
- Durable behavior spec/development notes, ROM/PROM, screenshots, frames,
  videos, user data, and existing worktree changes: outside implementation
  scope.

## Legacy and candidate selection

**IMPLEMENTATION-PLAN DECISION:** The default remains the existing path:
`--atari-presentation` absent means the current renderer and projection values
are used unchanged. The new option is explicit, development-only, portrait-only,
and opt-in; reject combination with `--legacy-presentation`. Do not change the
landscape/legacy camera or make the candidate a release/default mode.

Reject `--atari-presentation` when `--portrait-perspective` is explicitly
present, even if its value is zero. The current portrait diagnostic remains
available unchanged when the Atari option is absent. This prevents combining
the depth-only calibration with an adapter that supplies a different
per-level depth transform. With the Atari option selected, portrait perspective
is neutral and not an Atari parameter.

Allow the option alongside interactive `--record-gameplay` and
`--playback-file`; it changes rendered output only and is not serialized.
Playback users must select it again when they want candidate presentation.
Pause, forward/reverse frame stepping, simulation inputs, recording identity,
and screenshot capture remain unchanged. Live F10 and paused playback
Ctrl+Shift+S continue capturing the completed frame. Reject the option in
headless playback analysis and `--input-diagnostic`, which do not render game
world geometry.

For Levels 2-6, ROM bytes exist but the model-to-presentation mapping does not.
Use the unchanged legacy projection for those levels and emit a clear,
once-per-level development diagnostic. This is an explicit safe fallback, not
silent behavior and not substitution of another level's mapping. Missing or
malformed ROM metadata for an otherwise supported level is an error, not a
fallback. Levels outside the catalog remain unsupported.

## Per-level metadata plan

The existing catalog supplies ROM fields for all nine displayed production
levels. It does not supply the C# model-frame conversion. Keep `TubeTableIndex`,
`ScaleRaw`, `CameraHeightRaw`, signed Y2D bytes, and `OpenRaw` distinct from
`u`, `e`, C# `CenterY`/`YOffset`, and topology.

| Level | Record | `s` | `r` | signed Y2D | ROM open | Model-frame mapping | First candidate |
|---:|---:|---:|---:|---:|---|---|---|
| 1 | 0 | 24 | 80 | -192 | closed | `u=25/14`, `e=+1`; Level 1 correspondence is same-order with up to one C# coordinate unit of rounding | candidate |
| 2 | 1 | 28 | 80 | -224 | closed | not established | explicit legacy fallback |
| 3 | 2 | 24 | 80 | -192 | closed | not established | explicit legacy fallback |
| 4 | 3 | 15 | 104 | -128 | closed | not established | explicit legacy fallback |
| 5 | 4 | 24 | 80 | -192 | closed | not established | explicit legacy fallback |
| 6 | 5 | 24 | 80 | -192 | closed | not established | explicit legacy fallback |
| 7 | 6 | 24 | 104 | -144 | closed | `u=1.678958593`, `e=+1`; approximate best fit only | candidate, with fidelity limitation |
| 8 | 7 | 24 | 176 | +96 | open | `u=25/14`, `e=-1`; current point conversion is source-derived | candidate |
| 9 | 13 | 12 | 176 | +320 | open | `u=25/14`, `e=-1`; current point conversion is source-derived | candidate |

These ROM values and mappings are handoff-backed and are independently
represented by the current metadata catalog/tests. The Level 7 `+5` cyclic
comparison alignment is not part of `u`, `e`, point order, or the adapter.
For Levels 1-7, current C# `Continuous=true`; for Levels 8-9,
`Continuous=false`. `OpenRaw` must not override those existing C# decisions.

Levels 2-6 are deliberately not assigned a copied or guessed scale/orientation
conversion. Their missing frame data cannot alter the first candidate's
architecture or gameplay; new mapping support requires separate evidence-backed
entries and focused tests.

## Tube and entity registration plan

All listed anchors use the same selected transform at equal depth. Since the
transform is affine in model X/Y at fixed depth, current midpoint interpolation
before projection remains registered with projected endpoints.

| Render path | Candidate treatment |
|---|---|
| Tube vertices, both rims, rails, pulsing-lane boundaries, Spike start/tip | Project every existing model endpoint through the adapter. Keep point indices and lane loops unchanged; extrapolate below/behind the rim without clamping. |
| Lane interpolation for Eggs, shots, Spikes, Tankers, bullets, Spikers, Fuseballs, popups, destruction effects | Keep current interpolation and lane identity; project its result through the same adapter. |
| Flipper endpoints and gameplay-calculated pivot endpoints | Project each endpoint through the adapter. Preserve directed-pivot inputs and ordering. |
| Flipper body | Keep six existing segments and proportions; replace only its depth-responsive `20 * oldScale` term with the returned local scale. |
| Tanker, Fuseball, Spiker vector glyphs | Keep current vector programs, colors, rotation/animation, base dimensions, and fixed one-/two-pixel floors. Substitute the local Jacobian for the old `K/(z+ZOffset)` multiplier only. Preserve Fuseball's separate existing additive term. |
| Player shots and enemy bullets | Keep anchors and current vector geometry/orientation. Scale their world-anchored local vectors using the adapter Jacobian; preserve accepted point/shot design and existing base constants. |
| Enemy-destruction glyph | Project its lane anchor through the adapter and apply the Jacobian to its existing depth-responsive local vectors. Keep age, geometry, and color. |
| Blaster/claw | Project the existing player edge endpoints. Derive its depth/Approach size ratio from the same adapter local scale, retaining the current base height and the current distinction between normal, exit, and Approach contexts. No player coordinate changes. |
| Pulsar line endpoints | Project the edge/pivot endpoints through the adapter. Preserve `PulseZh` as its current post-projection screen-space oscillation; do not reinterpret it as tube geometry. |
| Egg markers, Spike-tip pixel, score-popup text, player-death burst, and transition stars | Keep current screen-space sizes/effect geometry and timing; transform only world anchors where they are currently world-anchored. These effects do not acquire a new glyph scale. |
| Game Over vector records | Explicit legacy projection exemption, detailed below. |

Tube/entity registration, projectile placement, accepted shot appearance, and
rim/below-rim consistency are manual comparison requirements. Automated tests
can establish shared transforms and scale equations, not visual acceptance.

## Special paths

### Approach and transitions

At each render, consume the existing `ApproachDepthOffset` and
`ApproachVerticalOffset` from `VectorRenderer`. Add the depth offset to the
render point and both active tube interval endpoints; include the vertical
offset in `v`. This reproduces the existing render-time composition through
the new mapping without advancing `LevelApproach` or changing its deterministic
tick sequence. The player edge and tube share the transform; preserve
spinner-controlled Blaster behavior. Keep the starfield as its independent
screen-space layer and preserve transition timing. No enemies or other
Approach visibility rules change.

### Game Over vectors

Use a deliberate legacy-path exemption for Game Over vector records in the
first milestone. Their raw record `(X,Y,Z)` values are not `LevelDefinition`
model coordinates, and their Atari-to-C# input conversion is not established.
Keep their existing conversion and old `Projection.Project` input path, with
the candidate option mutually exclusive with the separate portrait correction.
Do not adapt, scale, or redesign the accepted Game Over effect. The Game Over
message remains screen-space.

### HUD and text

Keep score, lives, HUD, overlays, warning, text, and playback status outside the
adapter. Existing paths use independent presentation coordinates/cameras;
where text is anchored at a world point, retain its existing post-anchor font
size. Do not start the deferred HUD-layout milestone.

### Playback, pause, stepping, and screenshots

Presentation selection is a command-line rendering choice, never input or
recording state. Existing interactive file playback, Space pause, Right forward
step, Left reverse reconstruction while paused, and screenshot chord/capture
continue through their current paths. Live F10 continues to request the current
rendered frame. Test the option parser and renderer composition; do not change
recording schema or screenshot naming/format.

## Automated test plan

| Invariant | Proposed test location |
|---|---|
| 1. Disabled candidate returns the exact existing `Projection.Project` result and leaves portrait diagnostic numerics unchanged | `DisplayConfigurationTests.cs`; keep `Projection.cs` unchanged |
| 2. Candidate is explicit, opt-in, development-only; default and duplicate/invalid options are covered | `DebugControllerTests.cs` |
| 3. Stored points for production Levels 1-9 are identical before/after repeated candidate projection | `VectorRendererAtariPresentationTests.cs` |
| 4. Point count/order, lane index, `PlayableLaneCount`, `Continuous`, and Level 7 ordering stay unchanged | `VectorRendererAtariPresentationTests.cs`; existing `RomPresentationMetadataTests.cs` |
| 5. Projection leaves level/entity/model state unchanged | `VectorRendererAtariPresentationTests.cs` |
| 6. Exact raw metadata lookup for Levels 1-9 and unsupported-level rejection | Existing `RomPresentationMetadataTests.cs`; adapter-level lookup tests in `AtariPresentationAdapterTests.cs` |
| 7. Near/mid/far (`p=16/128/240`) calculations and representative screen outputs for Levels 1, 7, 8, 9 | `AtariPresentationAdapterTests.cs` |
| 8. X symmetry for equal/opposite displacements; do not incorrectly assert Level 7's point pair is geometrically symmetric | `AtariPresentationAdapterTests.cs` |
| 9. `r` reference and signed Y2D contribution, including both negative and positive offsets | `AtariPresentationAdapterTests.cs` |
| 10. Tube endpoints and same-depth interpolated lane anchors remain registered | `VectorRendererAtariPresentationTests.cs` |
| 11. Jacobian agrees with finite differences and replaces legacy depth scaling for each registered glyph category without dropping fixed floors | `AtariPresentationAdapterTests.cs` and `VectorRendererAtariPresentationTests.cs`; existing shot/bullet/effect tests remain regressions |
| 12. Approach offsets compose without advancing approach ticks; tube/player anchors remain registered | `VectorRendererAtariPresentationTests.cs`; existing `BlasterLaneHighlightTests.cs` and `TransitionPresentationTests.cs` |
| 13. Candidate composes with file recording/playback and screenshot capture without becoming recording metadata | `DebugControllerTests.cs`; existing `GameplayRecordingTests.cs` and `PlaybackScreenshotCaptureTests.cs` |
| 14. `--portrait-perspective` and candidate are rejected together; portrait diagnostic remains usable alone | `DebugControllerTests.cs` |
| 15. Existing `-33.1` translation remains a later portrait camera placement | `DisplayConfigurationTests.cs` |
| 16. Non-finite inputs, invalid `u/e`, degenerate interval, and zero/non-finite denominators are rejected explicitly | `AtariPresentationAdapterTests.cs` |
| 17. Repeated equal inputs produce equal output and do not consume simulation randomness | `AtariPresentationAdapterTests.cs`; existing `TransitionPresentationTests.cs` |
| 18. Open endpoints, rim, and below-rim anchors use the same transform; no topology change | `VectorRendererAtariPresentationTests.cs`; existing Level 8/9 production tests |

Do not make framebuffer screenshot pixels the primary automated oracle.

## Future validation and manual acceptance

No build or tests are required or run for this read-only planning milestone.
For the future implementation, validate in this order:

1. Run focused `AtariPresentationAdapterTests`, renderer registration tests,
   and `DebugControllerTests`.
2. Run relevant existing projection, Approach, shot, enemy-bullet,
   destruction-effect, transition, playback, and screenshot tests.
3. Run the full test suite.
4. Build `TempestCSharp.slnx` in Release and require zero warnings/errors.
5. Run `git diff --check`.
6. Verify branch, HEAD, upstream, index, tracked changes, and untracked paths;
   stage only files explicitly authorized by that implementation task.
7. Stop for Neal's manual acceptance. The agent must not launch the game.

After implementation, Neal can compare the candidate and unchanged path using
these commands (candidate commands are proposed and will not work until
implemented):

```powershell
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 1 --atari-presentation
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 1
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 7 --atari-presentation
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 8 --atari-presentation
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 9 --atari-presentation
$recording = Join-Path $env:TEMP 'tempest-atari-presentation-20261115.json'
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 7 --atari-presentation --record-gameplay $recording
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation --playback-file $recording
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --playback-file $recording
```

Use Level 7 for the natural Level 7-to-8 transition and Approach; use the same
new recording with and without the candidate for deterministic comparison. The
recording command's path is under the system temporary directory; choose a fresh
name if that file already exists. Space pauses playback, Right steps forward,
Left reconstructs backward while paused, Ctrl+Shift+S captures paused playback,
and F10 requests a live screenshot. Compare candidate and no-option runs for
Levels 1, 7, 8, and 9. Do not combine `--portrait-perspective` with the
candidate; test that existing diagnostic separately if needed.

Neal's review must separately judge: unchanged geometry and point order;
perspective/depth; vertical placement; anchor/glyph scale and shot appearance;
open/closed topology; Approach and natural transition; tube/entity registration;
rim and below-rim objects; and gameplay/mechanics. No evidence-supported
numeric visual-acceptance threshold is prescribed.

## Rollback, boundaries, and deferred work

The first implementation milestone is exactly: add the separate adapter,
explicit portrait-only candidate selection, supported mappings for Levels 1,
7, 8, and 9, clear legacy fallback for Levels 2-6, registered glyph-local
scale treatment, Game Over legacy exemption, and the focused invariant/parser
tests above. Stop when automated validation is complete and Neal has the
candidate/no-option launch commands for manual acceptance.

With the option absent, the old projection remains the default. A rejected
candidate can therefore be disabled immediately; remove only its new adapter,
option wiring, renderer branch, and new tests in a separately authorized
rollback. Do not revert or rewrite existing changes. Do not commit the
candidate before manual acceptance, and no commit is authorized by this plan.

Deferred: mapping Levels 2-6; bit-exact Math Box emulation; integer
quantization/saturation; DVG-to-host calibration; final unconditional
activation; HUD/layout, fullscreen, leaderboard, geometry changes, topology or
lane remapping, and unrelated renderer cleanup.

## Decision-relevant unknowns

| Unknown | Could change architecture? | Could require gameplay/model change? | Could change this first milestone's code surface? | Could materially change its validation? | Disposition |
|---|---|---|---|---|---|
| No established model-frame conversion for Levels 2-6 | No; keyed mapping plus explicit legacy fallback handles absence | No | No; first milestone is restricted to 1/7/8/9 | No; support for those levels would need later evidence/tests | Non-blocking |
| Level 7's approximate fit and comparison-only `+5` alignment | No | No | No; never rotate or rewrite its array | It requires focused Level 7 manual judgment, already planned | Non-blocking |
| Exact DVG-to-logical calibration and visual impact of the current portrait correction | No | No | No | Requires Neal's visual review, already planned | Non-blocking |
| Exact retained Math Box state and integer rounding | No for the continuous candidate | No | No | Only a later bit-exact test would depend on it | Non-blocking |

No prerequisite blocks a safe, isolated first implementation. Manual acceptance
is a planned stop condition, not a readiness blocker.

## Scope and completion

- Prompt: `AI-Logs/Prompts/2026-11-15-atari-presentation-adapter-implementation-plan-prompt.md`.
- Report: `AI-Logs/Reports/2026-11-15-atari-presentation-adapter-implementation-plan-report.md`.
- Authorized files: exactly those two ignored AI-Logs artifacts.
- Production/test/durable-doc changes: none. No build/test suite, game launch,
  screenshot/frame/video inspection, staging, commit, or push.
- Starting branch: `master`; HEAD `385f603a94391756b2ca6cccd524dc17b281e782`;
  upstream `origin/master`.
- Starting index: empty staged diff.
- Starting tracked modifications: `src/Tempest.Game/Debug/DebugController.cs`,
  `src/Tempest.Game/Debug/PlaybackScreenshotCapture.cs`,
  `src/Tempest.Game/Levels/LevelLoader.cs`, `src/Tempest.Game/Program.cs`,
  `src/Tempest.Game/Rendering/DisplayConfiguration.cs`,
  `src/Tempest.Game/Rendering/Projection.cs`,
  `src/Tempest.Game/Rendering/VectorRenderer.cs`,
  `src/Tempest.Game/Systems/InputSystem.cs`,
  and tests `DebugControllerTests.cs`, `DisplayConfigurationTests.cs`,
  `GameOverDensityDiagnosticTests.cs`, `GameplayRecordingTests.cs`,
  `InputSystemTests.cs`, `Level9ProductionTests.cs`,
  `PlaybackScreenshotCaptureTests.cs`, and `TransitionPresentationTests.cs`.
- Starting untracked paths: `Reference/arcade/MBUCOD.V05.txt`,
  `Reference/arcade/MBUDOC.DOC`,
  `src/Tempest.Game/Levels/RomPresentationMetadata.cs`, and
  `tests/Tempest.Game.Tests/RomPresentationMetadataTests.cs`.
- Starting tracked work and untracked reference/source material are protected.
- Ending Git state: branch `master`, HEAD
  `385f603a94391756b2ca6cccd524dc17b281e782`, upstream `origin/master`; the
  tracked modifications and four untracked paths exactly match the starting
  inventory. No tracked file changed and the index remains empty.
- Exactly the two authorized AI-Logs files were created; both are ignored by
  `.gitignore`. Nothing was staged, committed, or pushed.
- Readiness: **READY FOR BOUNDED IMPLEMENTATION**.
- Prerequisite: none.
- Smallest implementation milestone: the exact scoped candidate defined above;
  stop for Neal's manual acceptance before any activation or commit.
