# Level 7 Atari-Presentation Reconstruction Plan

Date: 2026-11-18

Status: **READ-ONLY INVESTIGATION COMPLETE; IMPLEMENTATION NOT PERFORMED**

## Direct answers

1. **Remove after replacement validation:** the active Level-7-only record-06
   tube presenter, the record-06 entity lane/depth mapper, its renderer
   dispatch/local-scale path, the mapped-Flipper transition implementation,
   screenshot-calibrated gains/translations and derived centering, plus the
   standalone prototype controls and helpers when their final consumers are
   removed. Do not delete the comparison path before the shared replacement
   has tests and manual acceptance.
2. **Preserve:** production Level 7 geometry and point order, all gameplay and
   simulation behavior, the raw ROM-backed record-06 points and metadata, the
   16-lane closed topology, and accepted whole-game projection, perspective,
   recording/playback, Blaster, and ExitZoom systems.
3. **Replacement:** use the Level 8–10 method: draw the tube and project all
   entities from the same authoritative `LevelDefinition.Points`, through
   `AtariPresentationAdapter.Project`, the same final camera, and one shared
   placement translation.
4. **Shared geometry authority:** retain `LevelConfiguration.Level7Production`
   and `LevelLoader.LoadLevel7Clover()` as the gameplay and presentation
   geometry. Do not replace those coordinates with record-06-derived points
   in this presentation milestone.
5. **Remove `AtariLevel7EntityPresentation` completely?** Yes, after every
   replacement path is routed and tested. It exists to bridge two different
   geometries; the shared-geometry design has no need for its lane/depth
   interpolation, identity lane map, or separate local scale.
6. **Remove special Flipper transition presentation?** Yes. Return to the
   normal directed transition renderer using production geometry and the
   shared projector. Retain the gameplay's long/reflex route semantics.
7. **Obsolete tests/diagnostics:** replace tests for record-06 entity
   interpolation, prototype rendering, candidate-specific placement and
   mapped pivots with shared-projector and data-retention tests. Keep the
   runtime and framebuffer diagnostics until the reconstruction is validated
   and Neal has completed visual acceptance; then remove their Level-7-only
   CLI and test surface if it has no continuing diagnostic purpose.
8. **Lowest-risk order:** prove shared path and gameplay invariants; route tube
   and entities; validate same-state replay; then remove candidate dispatch,
   helpers, controls, and obsolete tests; run final automated validation;
   stop for Neal's manual acceptance.
9. **Primary risks:** accidentally changing accepted gameplay geometry or
   mechanics; retaining two geometric/depth authorities; carrying forward
   screenshot-fit parameters; misplacing the whole tube; regressing directed
   Flipper transitions, open/closed focal handling, or accepted Blaster
   Approach/Exit behavior.
10. **Enough evidence for one bounded implementation prompt?** Yes, for a
    render-only Option A candidate with unchanged production geometry and
    mechanics, no new placement value chosen in advance, and manual acceptance
    required. The evidence does not establish a final Level 7 screen-space
    placement or accepted visual fidelity.

## Repository state and scope

Expected baseline was verified:

- Branch: `master`
- HEAD: `aee59b39c1d68be830d0014a7b9497d1bb40f3c7`
- Upstream: `origin/master`
- Ahead/behind: `0 / 0`
- Index: clean
- Tracked worktree: pre-existing modification to `.github/copilot-instructions.md`
- Relevant untracked state: `Reference/arcade/MBUCOD.V05.txt` and
  `Reference/arcade/MBUDOC.DOC`

The local instruction edit and untracked reference files were preserved and
not inspected beyond the required repository status. The only task writes are
the ignored prompt and report listed at the end. No source, tests, durable
documentation, configuration, CLI, recordings, user assets, or Git state were
changed. No tests or builds were run because this was a read-only planning
task. No game, screenshot, video, framebuffer, raw ROM copy, or
`TEMP/myNotes.md` was opened.

## Current architecture and regression boundary

### Current paths

```text
LevelConfiguration.Level7Production
  ├─ gameplay/collision geometry: LevelLoader.LoadLevel7Clover()
  │    └─ 16 ordered production C# points; closed, 16 playable lanes
  ├─ Atari tube (Atari presentation, Playing):
  │    AtariTubeRecordCatalog.Level7Record06
  │    -> AtariTubeVectorListBuilder
  │    -> AtariCoordinateProjector at near/far positions 16/240
  │    -> AtariLevel7Presentation.MapNear/MapFar + fitted offsets/centering
  │    -> special VectorRenderer tube dispatch
  └─ gameplay entities (Atari presentation, Playing):
       gameplay lane/depth
       -> AtariLevel7EntityPresentation
       -> linear interpolation between mapped record-06 endpoints
       -> separate mapped lane-width local scale
       -> common portrait playfield camera
```

Before registration, the record-06 tube was already separate, but gameplay
entities remained on production clover geometry projected by
`AtariPresentationAdapter.Project`. That presentation looked mechanically
normal but entities and tube were visibly misregistered. The registration
milestone added `AtariLevel7EntityPresentation`, switched entity anchors to
record-06 lane/depth interpolation, and changed local scale from the adapter
Jacobian to mapped lane-width / near-width. Tube and entities aligned, but
Neal reported the severe depth-motion presentation problem. The
`2026-11-17-level7-depth-motion-regression-delta-readonly-report.md` identifies
this renderer-only registration as the narrowest supported boundary; no
committed revision isolates it because the relevant changes had been
uncommitted at the same HEAD in that investigation.

The later motion investigations qualify, rather than erase, that boundary:

- The deterministic Atari OFF/ON replay compared all 3,511 playback
  boundaries and found identical simulation state. This supports a
  presentation-specific regression and does not support a simulation change.
- On the currently registered record-06 path, a fixed-lane anchor is linear
  in normalized depth; its absolute submitted motion per tick is constant.
  The framebuffer investigation measured Tanker motion relative to the
  expanding glyph width falling from about `0.056` to `0.020` across four
  depth windows; raster-centroid normalized motion fell about 55%. The
  presented-rail analysis found constant `0.004375` of total same-lane path
  per boundary. Thus scale-relative easing is measured in the candidate,
  while the precise subjective speed defect and its relation to gameplay
  remain Neal's visual observation, not a fully explained depth-velocity law.
- The older depth-step analysis predicted the opposite absolute-depth trends
  for the prior reciprocal route and a flat anchor rate for the registered
  linear route. Do not state that a single formula explains every reported
  symptom. The reconstruction removes the unaccepted split path; it is not
  permission to retune simulation or infer a new Atari motion rule.

The mapped-Flipper crash correction is separate from the registration
boundary. Its special mapping exists because the mapped record-06 hinge can
have different convexity from the production gameplay hinge. Preserve the
crash-fix evidence; remove the special implementation only when returning
Flippers to shared production geometry and its normal directed transition
renderer.

## Successful Levels 8–10 comparison

The successful path uses one `LevelDefinition.Points` geometry for gameplay,
tube and entity coordinates. `VectorRenderer.DrawTube` projects each near/far
vertex through the common world projection; entity helpers project anchors
through the same `ProjectWorldPoint` / `AtariPresentationAdapter.Project`.
Tube-edge and entity lane registration follows from the shared points and
projector. The adapter's local Jacobian is the local-scale authority. The
final camera and any accepted level translation occur once; simulation
remains independent from rendering.

Level 8 and 9 closeouts preserve their recovered ROM-derived geometry and
metadata, shared tube/entity projector, and individually accepted final
placements (`+212.28139` and `+180.96825` logical Y respectively). Diagnostic
Level 10 uses the same architecture with its genuine open geometry and frozen
`+50` placement. Those values and shapes are examples of the method, not
Level 7 constants to copy. The accepted ExitZoom focal compensation remains
after the active adapter and accepted placement, before the shared host
camera; closed geometry uses the mapped far-rim center.

## Level 7 data authority and disposition

| Data or assumption | Evidence classification | Reconstruction disposition |
|---|---|---|
| `LevelLoader.LoadLevel7Clover()` 16 literal points, `CenterY=310`, `YOffset=-10` | **Current production gameplay authority**; the literals are Incitti Original Arcade clover points (**REFERENCE-DERIVED**), not an established Atari coordinate conversion | Preserve exactly, including order and values. Do not edit the gameplay geometry in this milestone. |
| Level 7 accepted gameplay and lane identity | **Current production authority / manually accepted gameplay behavior** | Preserve lane topology, all Level 7 mechanics, ordering, timings, RNG and deterministic playback. The accepted perceptual gameplay result is not proof of point-for-point Atari equivalence. |
| `AtariTubeRecordCatalog.Level7Record06` raw X/Y bytes, record index 6 | **ROM/Atari-backed** Rev. 2 record-06 data | Preserve verbatim as data/provenance even if it is no longer the active tube geometry. It must not be reindexed to fit the C# points. |
| Record 06 metadata: table row 6, scale `0x18`, camera height `0x68`, Y2D bytes `0x70/0xFF` (`-144` signed), open `0x00` | **ROM/Atari-backed raw metadata** | Retain in `RomPresentationMetadataCatalog`; the adapter's use is a separate **PROJECT TRANSLATION**. Metadata is shared by the generic adapter and is not Level-7 experimental code. |
| 16 point order, same-index rails, cyclic rims, closed record topology | **ROM/Atari-backed structural relation**, confirmed in the prior segment reconstruction; C# Level 7 is also closed with 16 lanes | Keep the raw order and closed topology. Do not equate this with a proven C# lane-number correspondence. |
| Normal near/far record projector positions `$10`/`$F0` and raw record inputs | Positions and source fields are **ROM/Atari-backed**; the specific normal projection translation has implementation assumptions | Preserve as diagnostic/provenance only if the standalone prototype remains. The ordinary Level 7 renderer should not depend on the special DVG pipeline. |
| `AtariCoordinateProjector` quotient count / RN-high interpretation | **DEVELOPMENT-ONLY / unresolved assumption** for the special prototype | Do not use this experiment to set gameplay, geometry or shared adapter behavior. |
| Existing Level 7 adapter model frame `(1.678958593, +1)` and reciprocal projection using metadata | Existing **PROJECT TRANSLATION**, already supported by `AtariPresentationAdapter` | Reuse unchanged as the starting shared path. Do not claim the coefficients are ROM constants. |
| `AtariLevel7EntityPresentation` identity lane mapping `i -> i` | **Unresolved assumption / PROJECT TRANSLATION candidate**; record comparison finds partial structural correspondence | Remove with the special mapper. With Option A, gameplay index refers directly to the same production point pair, so no cross-geometry lane mapping is needed. |
| `AtariLevel7Presentation` gains `.4215/.575`, far offsets `+27/+37`, shared aspect/global scale, provisional DVG viewport and calculated centering (about `(+97.47,+78.19)` logical) | **Screenshot-derived calibration / unaccepted PROJECT TRANSLATION candidate** | Do not carry forward. Recalculate placement only after the shared projector is active; do not fit points or tube shape to screenshots. |
| Record-06 to production point relation | **Unresolved**: matching closed 16-point topology, but comparison found a best simple fit with cyclic offset `+5`, different scale and residuals above rounding; no intentional lane rotation is documented | No record-06-derived replacement of production coordinates. The `+5` fit is comparison-only, not a production lane map. |

The literal Level 7 points are not established as an exact conversion of
record 06, but they are the active gameplay geometry with accepted mechanics.
Point coordinates participate in Flipper pivot geometry and renderer
placement. A previous lane-0 comparison between the pre-registration adapter
path and the record-06 target found host-space offsets of 359.36 px at the
rim, 103.70 px at depth 150, 49.19 px at depth 250 and 225.00 px at the far
end. This demonstrates that one constant entity offset cannot reconcile two
different geometries; it is not a measurement of gameplay collision error.
Record-06 lane lengths also varied materially (100.381 to 251.654 logical
units across examined lanes). The geometry-provenance investigation found
lane collision predicates index-based rather than raw-coordinate-distance
based, but actual point coordinates determine directed Flipper pivots and
their timing. Therefore replacing production points could preserve the
integer lane count while still changing accepted movement/pivot geometry.

The raw record-06 arrays that must remain unchanged are:

```text
X: EA E0 9C 80 64 20 16 50 16 20 64 80 9C E0 EA B0
Y: A0 E0 EA B0 EA E0 A0 80 60 20 16 50 16 20 60 80
```

They are data evidence, not a prescription to replace the production C#
clover or use the experimental quotient/viewport path at runtime.

## Architecture alternatives

| Option | Evaluation |
|---|---|
| **A — production Level 7 geometry + shared adapter** | **Recommend.** Gameplay/collision geometry, lane identity/order and closed topology stay unchanged. Tube vertices and entity anchors both pass through the existing Level 7 reciprocal `AtariPresentationAdapter.Project`; local scale is the adapter Jacobian. Flipper transitions use production directed geometry and the normal renderer. No second tube, record lane mapper or per-class scale. ExitZoom uses the shared closed-tube focal rule; placement is one post-projection translation. Implementation is the smallest coherent change and best preserves accepted mechanics. Main risk is that its projected clover may not reproduce the unaccepted record-06 screenshot shape; manual visual acceptance is mandatory. |
| **B — replace production points with record-06-derived coordinates** | Not justified for this presentation task. It changes active geometry used by Flipper pivot construction and possibly other coordinate-based behavior; collision predicates are generally lane-index based, but that does not prove mechanics remain exactly equivalent. The record-to-C# fit has a `+5` comparison offset, scale difference and non-rounding residuals; same 16 closed lanes do not prove physical identity. Entity registration would be easy once shared, but preserving timing, pivots, collision outcomes and accepted gameplay would require a separately authorized geometry/gameplay milestone and extensive equivalence evidence. |
| **C — keep separate geometries with a generic coordinate mapper** | Reject. It retains two visual/model tubes and still needs a lane/depth correspondence, transform and scale authority. Renaming or generalizing the Level 7 mapper would preserve the root architectural risk and would not make the registration evidence stronger. Open/closed topology and ExitZoom would remain difficult to reconcile across two definitions. |
| **D — another evidence-established architecture** | No distinct alternative is needed. The established shared-geometry adapter path is Option A. Using record 06 as active geometry would collapse to B; retaining record 06 as a second display shape collapses to C. |

## Recommended replacement and safety contract

Use the existing Level 7 production configuration and the existing Level 7
`AtariPresentationAdapter` mapping. Change only the renderer's Level 7
Atari-presentation selection so that:

1. Every tube point at near and far depth is passed through the same shared
   world projection as all other world anchors.
2. Every entity anchor, endpoint and depth segment is derived from the same
   production points and gameplay lane/depth state, then passed through that
   same projector.
3. Every world-space local scale comes from the same adapter Jacobian. Keep
   only existing artwork-local vector geometry/offsets.
4. The final per-level presentation placement is one translation in the shared
   projection stage. It is not a second camera or a transform added separately
   to entity classes.
5. The shared portrait camera/perspective is applied once after projection
   and placement.
6. Render operations remain read-only with respect to game/model state.

**Explicit regression-prevention invariant:** for an identical Level 7
gameplay state, tube points and all gameplay entity anchors use one coherent
presentation coordinate system. No entity type may use a separate
linearly-interpolated visual tube, depth convention, camera, duplicated
placement transform, or unsupported local-scale formula. A test should
compare projected anchors and tube points to the shared adapter results and
should fail if the Level-7-only mapper remains in the active path.

This contract intentionally does not assert constant pixels per tick through
depth. The shared reciprocal projector and its Jacobian determine scale and
position together; genuine projected displacement may vary with depth. Tests
must establish common mapping, stable gameplay state and no special path, not
replace arcade observation with an arbitrary constant-speed visual rule.

## Entity registration plan

| Entity/render item | Authoritative gameplay state | Shared presentation operation |
|---|---|---|
| Blaster | `Player.EdgeIndex`, `EdgePosition`, rim/approach/exit state | Project the two indexed production rim endpoints with the shared adapter; interpolate the same edge and derive tangent/outward normal from its projected near/far rails. Preserve accepted Blaster orientation and scale behavior. |
| Tanker | `EdgeIndex`, `Height`, state | Project the production lane midpoint at `Position + Depth - Height`; derive glyph scale from the same adapter Jacobian. |
| Ordinary Flipper | `EdgeIndex`, `Height`, active directed transition/progress | Project production lane endpoints at current depth; while pivoting, obtain endpoints from normal `CalculateFlipperPivotEndpoints` and project both with the adapter. |
| Tanker cargo Flipper | Same Flipper state plus cargo identity | Same `DrawFlipper` and shared projection as ordinary Flippers; cargo identity must not choose another camera or geometry. |
| Spiker | `EdgeIndex`, `Height`, rotation/state | Project lane midpoint through the adapter; preserve gameplay animation/rotation and existing vector artwork. |
| Egg | `EdgeIndex`, `Height` (including existing below-far initialization) | Project the lane midpoint at the existing projected depth. Preserve any existing extrapolation policy in the generic adapter; do not clamp or alter Egg state merely for rendering. |
| Spike | `EdgeIndex`, `Height` | Project its existing base and tip depths on the same lane through the adapter; no record-06 rail interpolation. |
| Player shot | `EdgeIndex`, `Height` | Project its lane midpoint at the existing shot depth and use adapter scale for artwork; do not alter update cadence/direction. |
| Enemy shot | `EdgeIndex`, `Height` | Same lane/depth projector and adapter scale; preserve vector program and timing. |
| Effects | Existing world/lane/height source and animation state | Project anchor through the same adapter; apply existing offsets only inside the effect's local vector drawing. |
| Lane highlights/pulsing | `Player.EdgeIndex`, pulse state, production tube topology | Draw the highlight and pulse from the very same projected tube endpoints; never compare a gameplay lane index against a separately mapped record index. |

## Flipper transitions

Remove `AtariLevel7EntityPresentation.MapFlipperTransition` and its mapped
hinge/radius/sweep implementation with the old two-geometry path. Use the
existing `DirectedLaneTransition` calculated from production geometry and the
standard renderer's `CalculateFlipperPivotEndpoints` result, projected by the
shared adapter. This restores the gameplay-derived directed sweep, including
complementary long/reflex routes such as 270 degrees. Do not select a short
route merely because a mapped record-06 hinge appears convex.

Tests must cover every Level 7 source lane and both legal directions at
representative and per-tick progress, endpoint continuity, pivot anchor,
render-state preservation, ordinary and cargo Flippers, and long routes. The
special mapped-Flipper crash test is obsolete once this geometry is absent;
its original failure evidence remains historical. Existing gameplay
`Level7FlipperMovementTests` and directed transition tests remain.

## Placement, perspective and scale

Do not set a Level 7 placement constant during this investigation. In the
implementation milestone, after the shared projector renders the production
geometry:

1. Project the full near/far tube envelope using the same adapter and current
   accepted perspective.
2. Compute one final translation that preserves horizontal center and centers
   the complete tube vertically within the gameplay composition below the
   persistent HUD.
3. Apply that translation once to tube geometry and all entity anchors in the
   shared projection stage.
4. Keep model points, lane geometry and projection formula untouched to fix
   placement. Do not copy Level 8, 9 or 10 placement values.

Remove/recalculate the present Level 7 center derived from the separately
fitted record-06 endpoints. Do not repeat the record-06 screenshot fit, DVG
aspect experiment, factor-128/RN.high experiment, analog-hardware theory, or
far-set translation tuning. The current `.4215/.575` gains and `+27/+37`
reference offsets are visual-candidate constants, not raw Atari data. If a
per-level input is required, use only existing Level 7 metadata and the
already-established adapter model-frame mapping; mark it as a project
translation.

## ExitZoom and Approach compatibility

Do not modify `ExitZoomFocalCompensation`. With shared Level 7 geometry, its
generic closed-tube focal rule obtains the mapped far-rim center from the
active definition and adapter. The accepted compensation remains
`latchedPivotAtExitStart - currentMappedPivot`, after adapter/placement and
before the host camera, and is shared by tube, Blaster, Spikes and tube-space
objects. Level 7 ExitZoom has not received separate manual acceptance; prove
it remains mechanically compatible by tests, then include it in Neal's manual
acceptance.

Keep the accepted Blaster Exit/Approach scale correction unchanged. Approach,
ReverseZoom/retry and exit transitions must continue to use existing
projection lifecycle and not inherit a Playing-only translation twice.
Regression coverage must retain `ExitZoomFocalCompensationTests`,
`BlasterScaleRegressionTests`, and transition/Approach tests, including their
accepted Levels 5, 8 and 9 behavior.

## Component and consumer inventory

| Component / surface | Current role and consumers | Planned disposition |
|---|---|---|
| `AtariLevel7Presentation` | Builds the mapped record-06 endpoint arrays; its center/gain/far-offset constants feed Level 7 tube dispatch, entity mapping and reference-shape control. | **Remove** after replacement tests pass. Do not retain its fitted endpoint authority. |
| `AtariLevel7EntityPresentation` | `VectorRenderer` lane/depth identity mapping, linear interpolation, lane-width local scale and Flipper transition geometry; tested by `AtariLevel7EntityPresentationTests`. | **Remove completely** after all render consumers are switched. Do not generalize it into a generic second mapper. |
| `AtariTubeRecordCatalog.Level7Record06` | Raw 16-point record and metadata validation; used by prototype builder/tests. | **Retain data** as ROM/Atari provenance. It need not drive the ordinary Level 7 tube. Keep raw-data tests, moved/rewritten if the prototype test file is removed. |
| `RomPresentationMetadataCatalog` | Supplies Level 7 and other levels' raw metadata to the shared adapter and topology validation. | **Retain and share unchanged.** It is an active generic adapter input. |
| `AtariTubeVectorListBuilder` | Builds only the direct record-06 near/far endpoints, rails and rims for the Level 7 prototype/tests. | **Remove after replacement validation** if no diagnostic consumer remains; it is not the replacement tube builder. Preserve equivalent raw-data/topology assertions without retaining a dead runtime builder. |
| `AtariCoordinateProjector` | Projects record-06 bytes for the standalone DVG prototype; builder is its runtime caller. | **Remove after replacement validation** if no other consumer exists. Preserve ROM-backed raw data and clearly classify any retained projector test fixture as development-only. |
| `AtariDvgViewportMap` | Provisional direct-DVG viewport map for prototype/reference diagnostics. | **Remove** with prototype path after acceptance; do not reuse its provisional `0.4` scale or translation. |
| `AtariTubeReferenceShapeDiagnostic` | Wrapper for the screenshot-derived near/far shape mapping; selected only with prototype CLI. | **Remove** after replacement validation. Its constants and CLI are not shared projection inputs. |
| Level-7-specific `VectorRenderer` dispatch and helpers | `UsesAtariLevel7PresentationCandidateForCurrentLevel`, `DrawAtariLevel7PresentationTube`, `GetLevel7LaneGeometry`, `GetLanePresentationScale`, special Blaster/lane/shot/Flipper branches. | **Remove** after all entity types and tube use the common projector. Preserve generic adapter dispatch and normal renderer logic. |
| Mapped Flipper transition renderer | Special `MapFlipperTransition` path and candidate-only geometry/scale. | **Remove**; use production directed transition renderer, with long-route tests retained. |
| Centering and presentation constants | `CenteringTranslationLogical`, `.4215/.575`, `+27/+37`, shared aspect/scale and provisional prototype view values. | **Remove/recalculate** after shared projection; no current numeric value is accepted for Level 7. |
| `--atari-tube-prototype` and reference-shape selector | Development-only prototype controls parsed in `DebugController`, wired in `Program`, selected in renderer; tests in `AtariTubeRecordPrototypeTests` and `DebugControllerTests`. | **Remove after replacement/manual validation** if no remaining diagnostic need. Update public CLI/help and parser/context tests at the same time. |
| `--atari-tube-rn000f-diagnostic` | Conditional factor-128 quotient experiment on the prototype; RN.high normal startup state remains unresolved. There is no separate `--factor-128` flag. | **Remove** with prototype after validation; retain the report as historical evidence only. |
| `Level7VisibleRuntimeDiagnostic` and `--level7-visible-runtime-diagnostic` | Runtime trace instrumentation in `Program`, `VectorRenderer`, `DebugController`; tests in `Level7VisibleRuntimeDiagnosticTests`. | **Keep through same-playback implementation validation and Neal's acceptance**, since it can compare submitted geometry/state; then remove if no continuing targeted diagnostic use. Otherwise explicitly retain as DEVELOPMENT-ONLY and keep its CLI/tests. |
| `Level7FramebufferDiagnostic` and `--level7-framebuffer-diagnostic` | Bounded pre-swap readback, synchronized playback-window capture, tests in `Level7FramebufferDiagnosticTests`. | **Keep through manual A/B acceptance** if needed; remove its command/help/wiring/tests only after acceptance and confirming no outstanding diagnostic question. It is a useful diagnostic until then, not implementation architecture. |
| `AtariLevel7EntityPresentationTests` | Direct tests of identity map, linear/unclamped depth interpolation, record centering, candidate pivots, renderer registration and model-state preservation. | **Replace/remove** the special mapper tests. Preserve model-state invariants and transition coverage in new shared-path tests and existing gameplay tests. |
| `AtariTubeRecordPrototypeTests` | Mixes raw record/projector/closed topology assertions with prototype CLI, screenshot mapping and renderer selection. | **Split disposition:** retain/re-home record-06 raw bytes/metadata/point-order/topology tests; remove prototype, factor-128, calibrated map and candidate renderer tests after replacement. |
| `VectorRendererAtariPresentationTests` | Generic Levels 1–6 adapter/legacy tests plus Level 7 candidate and shared adapter integration tests. | **Keep generic tests; rewrite only Level 7 candidate assumptions** as common-tube/entity projection tests. |
| `DiagnosticLevel10Tests` Level 7 selection assertions | Currently asserts the experimental Level 7 candidate selects only Level 7. | **Update** to assert Level 7 uses the shared adapter and Level 10 remains diagnostic; keep all Level 10 geometry/presentation checks. |
| `DebugControllerTests`, `Program` help/CLI | Expose prototype, factor-128, reference-shape, visible-runtime and framebuffer switches. | Remove only selectors whose diagnostics are retired; keep recording/playback and `--atari-presentation` behavior consistent. Do not silently leave stale help or permissive parsing. |
| `Level7ProductionTests`, `Level7AdmissionPolicyTests`, `Level7FlipperMovementTests`, `Level7SpikerFiringTests`, `GameplayRecordingTests` | Production gameplay, directed transitions, admission, firing, deterministic recordings/playback. | **Retain unchanged** as gameplay-protection coverage. Do not delete tests to make the new renderer pass. |

Removing prototype/diagnostic switches changes public CLI/help text and
parser tests. Diagnostic cleanup is a later step than replacement and manual
acceptance; it must not be bundled into the first route change.

## Ordered replacement/removal plan

1. Add shared Level 7 tube/entity projection tests and explicit gameplay-state
   equivalence coverage before changing dispatch. Add data tests proving the
   16 production points, point order, closed topology, lane count, metadata
   and raw record-06 evidence are not mutated.
2. Implement the shared tube path from `LevelDefinition.Points`, projecting
   both rims and rails through the same adapter used by entities. Do not use
   the record builder or add new calibration values.
3. Route every Level 7 entity, effect, Blaster basis and lane highlight through
   the same production points, depth convention, projector and local Jacobian.
   Keep simulation/render separation.
4. Calculate Level 7 placement from the complete shared projected envelope;
   apply one final translation to tube and anchors. Do not change geometry to
   compensate for placement.
5. Run deterministic same-recording Atari OFF/ON state comparison and focused
   renderer tests. Confirm no active path calls
   `AtariLevel7EntityPresentation` and no second record-06 visual tube remains.
6. Validate gameplay snapshots/mechanics, recording/playback, collision,
   lane identity and transitions. If any change is found, stop and repair the
   implementation before deleting the comparison path.
7. Remove the special renderer dispatch, `AtariLevel7Presentation`,
   `AtariLevel7EntityPresentation`, candidate local-scale/pivot helpers and
   candidate centering constants. Replace old candidate assertions rather
   than leaving misleading tests green.
8. Remove direct prototype helpers (`AtariTubeVectorListBuilder`,
   `AtariCoordinateProjector`, `AtariDvgViewportMap`, and reference-shape
   wrapper) only after confirming remaining consumers. Keep the raw
   record-06 catalog data and metadata tests.
9. Keep visible-runtime/framebuffer diagnostic CLI and tests until the
   replacement has passed automated validation and Neal's manual A/B.
   Afterwards remove these controls only if their specific role has ended;
   otherwise retain them explicitly as development-only tools.
10. Run final targeted and full validation, Release build and
    `git diff --check`; stop without launching the game and hand Neal the
    task-appropriate manual acceptance commands.

## Test migration plan

### A. Data and geometry

- Verify `Level7Production.Definition` remains the exact 16-point ordered
  clover with `Continuous=true`, 16 playable lanes, unchanged `CenterY` and
  `YOffset`; verify `Position`, `Depth`, initial lane and production identity.
- Verify record-06 raw X/Y arrays, row 6, scale `0x18`, camera height `0x68`,
  signed Y2D `-144`, closed flag `0x00`, same-index rails and cyclic topology
  remain preserved as data.
- Verify tube geometry is generated from production `Definition.Points`, not
  screenshot-derived literals, a per-point correction, a cyclic `+5` remap,
  or a second record-06 endpoint set.
- Verify metadata topology agrees with the production closed definition.

### B. Shared presentation

- At near and far depths, assert every tube endpoint equals the corresponding
  production point projected by the shared adapter.
- At representative depths, assert Blaster, Tanker, ordinary/cargo Flipper,
  Spiker, Egg, Spike, both shots and effects use their state-derived points
  projected by that same adapter.
- Assert one depth convention, one host camera, one Level 7 placement
  translation and one local-scale/Jacobian authority.
- Assert player-lane highlighting and pulsing edges share the tube point
  indices, and render calls leave geometry/model state unchanged.
- Assert Level 7 uses the existing adapter mapping (`1.678958593`, orientation
  `+1`) and raw Level 7 metadata without introducing screenshot coefficients.

### C. Gameplay protection

- Compare deterministic tick-by-tick Level 7 state for identical seed/input
  with presentation disabled/enabled, including full recording/playback
  boundaries. Compare player, lanes, enemy states, cargo identities, shots,
  spikes, collision outcomes, wave/ledger supply, completion, Super Zapper,
  score, lives, timing and RNG-derived outcomes.
- Run existing Level 7 production/admission/Flipper/Spiker tests; retain
  `Level7ProductionTests`, `Level7AdmissionPolicyTests`,
  `Level7FlipperMovementTests`, `Level7SpikerFiringTests`,
  `GameplayRecordingTests` and relevant playback tests.
- No gameplay geometry, update, collision, RNG, recording schema or tick
  cadence changes are in scope.

### D. Depth-motion regression

- Use the same genuine Level 7 recording for Atari OFF and ON; compare
  identical simulation state and playback boundary, not different manual and
  recorded runs.
- Assert the active Level 7 renderer does not route any entity through
  `AtariLevel7EntityPresentation` or any second depth interpolation.
- For identical depth states, compare anchors against the shared adapter and
  tube rail projections. Verify current gameplay movement deltas, player shot
  direction/timing, enemy shot direction/timing and normal rim movement are
  unchanged.
- Do not impose a constant screen-pixels-per-tick expectation on the shared
  reciprocal projection. Automated tests prove coordinate consistency and
  simulation identity; Neal determines perceived speed and visual fidelity.

### E. Entity coverage

Cover Blaster; Tanker; ordinary and tanker-cargo Flippers; Spiker; Egg;
starting and moving Spike; player shot; enemy shot; enemy/player destruction
effects; lane highlight/pulse. Assert each anchor/base and any world-space
segment is derived from production geometry and shared projection. Local
artwork geometry remains local.

### F. Transitions

- All Level 7 directed Flipper pivots, including long/reflex 270-degree
  routes, endpoints, continuity, both directions, and cargo Flippers.
- ExitZoom closed-tube far-rim focal compensation and shared translation.
- Approach, ReverseZoom and exit retry/reset, including accepted Blaster
  scale. Do not modify the focal or scale systems.

### G. Cross-level protection

Run Level 1–6 legacy/accepted presentation coverage; Levels 8 and 9 accepted
Atari presentation; diagnostic Level 10; Blaster outward orientation and
travel/Approach scale; accepted ExitZoom focal behavior; and option-off
rendering. Do not copy or retune other levels' placements.

## Bounded future validation commands

Use the focused new shared-path test class plus existing suites, with the
project's VSTest platform and a hang timeout:

```powershell
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~Level7AtariPresentationTests" --blame-hang-timeout 4m --verbosity minimal
```

```powershell
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~Level7AtariPresentationTests|FullyQualifiedName~Level7ProductionTests|FullyQualifiedName~Level7AdmissionPolicyTests|FullyQualifiedName~Level7FlipperMovementTests|FullyQualifiedName~Level7SpikerFiringTests|FullyQualifiedName~GameplayRecordingTests|FullyQualifiedName~GameplayPlayback" --blame-hang-timeout 5m --verbosity minimal
```

```powershell
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~AtariPresentationAdapterTests|FullyQualifiedName~VectorRendererAtariPresentationTests|FullyQualifiedName~Level8AtariPresentationTests|FullyQualifiedName~Level9AtariPresentationTests|FullyQualifiedName~DiagnosticLevel10Tests|FullyQualifiedName~BlasterPresentationTests|FullyQualifiedName~BlasterScaleRegressionTests|FullyQualifiedName~ExitZoomFocalCompensationTests|FullyQualifiedName~TransitionPresentationTests|FullyQualifiedName~DirectedRotationInvestigationTests" --blame-hang-timeout 5m --verbosity minimal
```

```powershell
dotnet test TempestCSharp.slnx --no-restore --blame-hang-timeout 5m --verbosity minimal
dotnet build TempestCSharp.slnx --no-restore --configuration Release --verbosity minimal
git diff --check
```

If a test/build is blocked, times out or is otherwise incomplete, report that
outcome explicitly; do not call it a pass. Full solution tests and Release
build are final implementation validation, not required for this read-only
planning report.

## Bounded manual acceptance sequence

Neal's manual testing remains the final authority for visual shape, placement,
motion feel, lane registration, and arcade fidelity. After automated
validation, do not launch the game on Neal's behalf. Give these commands and
stop:

1. Direct Level 7 play:
   `dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation --level 7`
2. Replay the same genuine recording without Atari presentation, then with it:

   ```powershell
   dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --playback-file TEMP\GameplayRecordings\Level-7-slowmo.json
   dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --playback-file TEMP\GameplayRecordings\Level-7-slowmo.json --atari-presentation
   ```

3. Confirm gameplay timing/progression remains the same; tube shape/placement;
   Blaster and all entities stay on visible lanes; enemy deep-to-rim motion;
   player-shot rim-to-deep motion; rim movement; Flipper transitions; exit and
   Approach. Compare presentation off/on for the same recording and do not
   substitute a different run.

Manual acceptance is not established by automated coordinate tests,
instrumentation or screenshot fitting. No new visual acceptance is claimed
by this plan.

## Evidence reviewed, unresolved issues and stop conditions

Historical reports and current source reviewed include:

- Level 7 geometry provenance/disposition; record-06 segment reconstruction,
  shape transform, presentation implementation, entity/tube registration,
  render-registration implementation and mapped-Flipper crash correction.
- Depth-to-visible-motion, regression-delta, deterministic state OFF/ON,
  visible-runtime instrumentation, good/bad runtime A/B, framebuffer
  readback/A-B and presented-rail motion analyses.
- Level 8 and Level 9 Atari presentation implementation/closeouts; diagnostic
  Level 10 geometry/presentation closeouts; Atari adapter implementation;
  accepted Blaster Exit/Approach scale and ExitZoom focal compensation.
- Current `LevelLoader`, `LevelConfiguration`, `RomPresentationMetadata`,
  record catalog, Atari adapter, `VectorRenderer`, CLI/debug wiring and
  relevant Level 7, transition, Blaster, recording and presentation tests.

The complete motion mechanism remains partly unresolved: numerical
instrumentation confirms identical gameplay and shows scale-relative
presentation easing, while Neal's perceptual report remains authoritative.
No new assertion about Atari's intended depth-motion law is made. The record
06-to-production lane correspondence is not established; the numerical `+5`
fit is not a lane-mapping decision. Level 7's final presentation placement
and visual fidelity also remain unaccepted.

Stop implementation if shared-projector routing requires changing production
geometry, gameplay timing/state, collision rules, RNG/recording data,
accepted Blaster/ExitZoom systems, or adding another entity-only depth map.
Stop before selecting a placement if the shared near/far envelope or HUD
composition cannot be computed from existing projection inputs. Report
unknowns instead of fitting to unauthorized imagery.

## Completion

- Status: **READ-ONLY INVESTIGATION COMPLETE**
- Source/tests/configuration/durable docs/CLI/gameplay/recordings: unchanged
- Git index, branch and HEAD: unchanged; no stage/commit/push/branch switch
- Tests/build: not run, by design
- Game/media/raw ROM/private notes: not opened
- Prompt: `AI-Logs/Prompts/2026-11-18-level7-presentation-reconstruction-plan-readonly-prompt.md`
- Report: `AI-Logs/Reports/2026-11-18-level7-presentation-reconstruction-plan-report.md`
- Completion token: `TEMPEST_LEVEL7_PRESENTATION_RECONSTRUCTION_PLAN_READONLY`
