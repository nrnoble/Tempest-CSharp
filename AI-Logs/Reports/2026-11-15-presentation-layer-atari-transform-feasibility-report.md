# Presentation-Layer Atari Transform Feasibility

Date: 2026-11-15
Status: **READ-ONLY INVESTIGATION COMPLETE**

## Executive conclusion

**FEASIBLE WITH EXPLICIT LIMITATIONS.** A render-time, per-level transform can consume the existing ordered C# geometry and the recovered Atari references, scale, and signed Y offset without changing production points, lane identity, topology, or simulation state. Level 7's residual prevents a claim of point-for-point Atari geometry, but does not prevent applying the same presentation arithmetic to its existing clover. Whole-game visual fidelity is not established; several renderer paths need coordinated treatment before implementation.

## Evidence boundary

Controlling handoffs used:

- [Atari Math Box `$c098` reconciliation](./2026-11-15-atari-mathbox-c098-reconciliation-report.md)
- [Atari/C# projection comparison](./2026-11-15-atari-csharp-projection-comparison-report.md)
- [Atari/C# coordinate-frame mapping](./2026-11-15-atari-csharp-coordinate-frame-mapping-report.md)
- [Level 7 geometry provenance disposition](./2026-11-15-level7-geometry-provenance-disposition-report.md)

Current C# sources examined:

- [Projection.cs](../../src/Tempest.Game/Rendering/Projection.cs)
- [VectorRenderer.cs](../../src/Tempest.Game/Rendering/VectorRenderer.cs)
- [TransitionStarfield.cs](../../src/Tempest.Game/Rendering/TransitionStarfield.cs)
- [DisplayConfiguration.cs](../../src/Tempest.Game/Rendering/DisplayConfiguration.cs)
- [LevelDefinition.cs](../../src/Tempest.Game/Levels/LevelDefinition.cs)
- [LevelLoader.cs](../../src/Tempest.Game/Levels/LevelLoader.cs)
- [LevelConfiguration.cs](../../src/Tempest.Game/Levels/LevelConfiguration.cs)
- [RomPresentationMetadata.cs](../../src/Tempest.Game/Levels/RomPresentationMetadata.cs)
- [Program.cs](../../src/Tempest.Game/Program.cs)

The numeric source values and earlier provenance conclusions below are taken from the controlling handoffs; the original disassembly and ROM/PROM files were not reopened. No Internet, third-party implementation, media, screenshot, frame, or game launch was used. No build or test was required or run.

Evidence classifications used: **ATARI-BACKED**, **C#-SOURCE-BACKED**, **NUMERICALLY DEMONSTRATED**, **ARCHITECTURAL CONCLUSION**, **STRONG INFERENCE**, and **UNKNOWN**.

## Immutable model-to-presentation seam

**C#-SOURCE-BACKED / ARCHITECTURAL CONCLUSION:** `VectorRenderer.ProjectWorldPoint` is the earliest common safe seam. It gathers the current level configuration and render-only Approach offsets, then calls the sole `Projection.Project` call found under `src`. The renderer supplies coordinates to this seam; the result is consumed for drawing and is not written back to `LevelDefinition`, entities, or simulation state. The point arrays remain model/gameplay input.

The traced paths include:

- Tube vertices, both rims, longitudinal rails, and pulsing lane boundaries: `DrawTube` and the boundary renderer project the current indexed `Definition.Points`.
- Lane-centered positions: `ProjectLane` interpolates the adjacent ordered vertices before projection. Shots, Spikes, Tankers, bullets, Spikers, Eggs, Fuseballs, popups, and destruction effects use this or equivalent point interpolation.
- Flippers and Pulsars: lane endpoints or gameplay-computed pivot endpoints are projected individually at the entity depth.
- Entity depth/height: `ProjectEnemyDepth` and the per-object depth calculations map heights to projection depth; the projection seam then converts depth to screen coordinates.
- Player and player shots: lane endpoints/midpoints use the same seam. Spike tips and tube rims likewise use it.
- Approach: tube/player world geometry uses `ProjectApproachGeometry` and `ProjectWorldPoint`; the Approach depth and vertical offsets are supplied there. Other enemies are hidden during Approach by the existing draw path.
- Transition stars and HUD/overlays are a separate screen-space presentation layer, not lane-registered tube geometry.
- Game-over vector records call the world projector but begin in a separate raw-record coordinate frame, so their input-unit conversion must be handled explicitly rather than assumed to be a `LevelDefinition.Points` value.

At a fixed depth the candidate is affine in X/Y. Therefore midpoint/edge interpolation before projection yields the same result as interpolating the projected endpoints at that depth. Point order and `PlayableLaneCount` remain untouched; Atari open/closed metadata must not change C# topology.

**Coordinated rendering paths:** Several glyphs are assembled around an already-projected center using local screen-space vectors and depth scales based on the old global `K/(z+ZOffset)` rule (for example Flippers, Tankers, Fuseballs, Spikers, player shots, enemy bullets, and player/claw presentation). Their anchor can use the shared seam, but retaining their current local scales unchanged could make them inconsistent with the new tube. They need a render-only local scale/Jacobian derived from the same presentation transform or endpoint projection through it. The independent transition starfield remains a separate effect layer; it should not be mistaken for a tube entity.

## Required per-level presentation inputs

| Input | Current source/status | Role |
|---|---|---|
| Displayed level → Atari record | `RomPresentationMetadataCatalog`; handoff verifies 1→00, 7→06, 8→07, 9→0d | Select per-level values, not point order |
| `lev_scale` | `ScaleRaw` in metadata: 0x18, 0x18, 0x18, 0x0c for Levels 1/7/8/9 | Additive reciprocal-depth divisor term |
| `lev_y3d` | `CameraHeightRaw`: 0x50, 0x68, 0xb0, 0xb0 | Y axis reference before division |
| Signed `lev_y2d/lev_y2db` | Combined little-endian signed `ScreenYOffsetSigned`: -192, -144, +96, +320 | Post-division vertical displacement |
| X reference / X offset | Atari handoff: X reference 0x80; normal X offset zero; not stored in metadata | Center X displacement; then restore sign |
| Runtime depth | C# `LevelConfiguration.Position/Depth` (50/400 in production); Atari endpoint evidence `$57=0x10/0xf0`, with `$80` as an analytic midpoint | Establish a normalized render-depth correspondence |
| Coordinate unit and orientation | Prior mapping: 25/14 C# units per Atari axis unit for Levels 1/8/9; Level 7 best-fit scale 1.678958593; Y orientation + for 1/7 and reflected for 8/9 | Convert immutable model coordinates into a temporary Atari-like axis frame |
| Open/closed traversal | `OpenRaw` metadata and current `LevelDefinition.Continuous` are both present | Keep topology/traversal distinct from projection arithmetic; preserve the C# choice |
| Host placement | `LevelDefinition.CenterY/YOffset`, `Projection.K/ZOffset`, and the portrait camera | Convert the transformed result into logical screen coordinates; not Atari metadata |

Metadata is inert in the current renderer. The C# geometry-to-frame factors are derived from the earlier bounded comparison, not stored presentation inputs.

## Candidate mathematical mapping

This is a candidate render-time mapping, not an implementation or visual-equivalence claim.

Let `u` be C# model units per Atari raw coordinate unit, and let `e` encode the C# Y orientation (`+1` for Levels 1/7, `-1` for Levels 8/9). For model position `(x,y)`, with current render-only C# vertical offset `v = Definition.YOffset + ApproachVerticalOffset`:

```text
Xraw = 128 + x/u
Yraw = 128 + e*(y+v)/u
dX   = Xraw - 128 = x/u
dY   = Yraw - lev_y3d
```

`v` remains a pre-projection C# translation expressed in model units; it is not treated as Atari Y2D. `CenterY` remains a host/logical vertical anchor. This keeps the existing point coordinates immutable while replacing the current projection's role for these inputs.

Map C# depth to a normalized Atari runtime-depth counterpart using the established drawing endpoints:

```text
zNear = Position + ApproachDepthOffset
zFar  = Position + Depth + ApproachDepthOffset
t     = (z - zNear)/(zFar - zNear)
p     = 16 + 224*t
```

Thus near/mid/far samples map to `$10/$80/$f0`. The midpoint is analytic, and this normalized mapping does not assert that a C# depth is a physical Atari `$57` value. Outside the normal tube interval, the continuous mapping is extrapolated rather than clamped; such extrapolation is not an Atari-backed runtime claim.

The recovered Atari arithmetic is:

```text
q(d,p,s) = sign(d) * floor(256*abs(d)/(p+s))
Xresult  = q(dX,p,lev_scale)
Yresult  = q(dY,p,lev_scale) + signed_Y2D
```

For a floating-point adapter, use `Q(d,p,s)=256*d/(p+s)`. This is the continuous counterpart of the recovered reciprocal-depth equation; it intentionally omits Atari integer floor/quantization and signed-16 saturation. Those effects would matter to bit/pixel-level matching but do not prevent a coherent render transform.

One data-derived logical-unit conversion, anchored to the current C# near projection rather than an aesthetic constant, is:

```text
lambda = u * Projection.K * (16 + lev_scale)
         / (256 * (zNear + Projection.ZOffset))
screenX = 400 + lambda*Q(dX,p,lev_scale)
screenY = B + e*lambda*(signed_Y2D + Q(dY,p,lev_scale))
```

Choose the host baseline so that the model-origin near point retains the existing C# near anchor before the ROM Y2D displacement:

```text
QyOriginNear = Q(e*v/u + 128 - lev_y3d, 16, lev_scale)
B = CenterY + v*Projection.K/(zNear+Projection.ZOffset)
    - e*lambda*QyOriginNear
```

This explicitly keeps the signed Y2D after projection and the orientation conversion on the output Y axis. Under the normal sample state `Projection.K=70`, `ZOffset=5`, `zNear=50`, the resulting `lambda` is 0.355113636 for Levels 1/8, 0.333883811 for Level 7, and 0.248579545 for Level 9.

No point is rotated or reindexed. Open/closed traversal remains solely a topology decision. A single `Q` rule applies to tube vertices and lane-based entities at the same `p`; because it is affine at fixed depth, current midpoint interpolation remains valid. The conversion does not assert exact Level 7 point correspondence or pixel fidelity.

## Level 7 feasibility

- **Can `lev_scale`, Y-reference behavior, and signed Y offset apply independently of exact record-06 point identity?** Yes. They are per-level projection parameters; the equation consumes axis displacements and depth, not a lane-indexed lookup into Atari's point row.
- **Does the transform require literal Atari record coordinates?** No. It requires displacements in a compatible axis/unit frame. C# coordinates can be converted temporarily at render time.
- **Can a temporary frame be derived from the existing clover without altering lanes or stored points?** Yes. Use its reported best-fit model-units-per-ROM-unit factor (1.678958593) and Y orientation. The prior +5 cyclic alignment was used to compare shapes and estimate the fit; it is not an instruction to rotate the C# array, lane indices, or entity state.
- **Would that preserve tube/entity registration?** Yes, if vertices and every lane/entity anchor use the same per-level conversion and depth function. The renderer's shared world-point seam provides that path; local glyph sizes still need coordinated handling as described above.
- **Does the record-06 residual prevent Atari-style presentation?** No. It prevents claiming exact point-for-point arcade geometry. The best-fit residuals (max absolute X 8.409987560, Y 9.726674960; RMS 6.695326333) remain after comparison alignment and exceed rounding. Applying a projection to the unchanged clover is mathematically defined, not proof of visual fidelity.

**Evidence:** The record mapping, residuals, and geometry provenance are in the controlling coordinate-frame and Level 7 disposition handoffs. The Level 7 array and its closed lane topology remain immutable for this task.

## Tube/entity registration

**ARCHITECTURAL CONCLUSION:** Every traced tube vertex, rim/rail, lane midpoint, shot/Spike anchor, and the principal lane-based entity endpoints can use the same `ProjectWorldPoint` transform without touching gameplay/model state. Depth derives from the existing render-only conversion of entity height; interpolation can remain in model space at a fixed depth.

Before implementation, adapt post-projection vector glyph dimensions to the transform's local scale. Treat Game Over vector records with their own coordinate-unit conversion; do not silently interpret their raw vector bytes as level-model units. Transition stars and HUD text are independent screen-space presentation, not lane-registered objects. During Approach, preserve the current shared depth/vertical offset treatment for tube/player world geometry and leave the separate starfield layer outside the tube transform.

These are bounded rendering-path limitations, not a contradiction requiring model changes.

## Bounded numeric check

**NUMERICALLY DEMONSTRATED:** The following inline calculation used the immutable production point values, the mapping factors above, `Projection.K=70`, `ZOffset=5`, production near depth 50/depth 400, and exactly three samples. For Level 7, `u` is the reported approximate fit; all other listed coordinate mappings are as in the handoff. No script file was created.

Coordinate inputs for representative opposite-X points:

| Level / record | `u`, `e` | `lev_scale`, `lev_y3d`, signed Y2D | C# center / Y offset | Chosen points → `(dX,dY)` |
|---|---|---|---|---|
| 1 / 00 | 1.785714286, +1 | 24, 80, -192 | 400 / -80 | index 0 `(200,0)` → `(112,3.2)`; index 8 `(-200,0)` → `(-112,3.2)` |
| 7 / 06 | 1.678958593, +1 | 24, 104, -144 | 310 / -10 | index 0 `(-160,160)` → `(-95.297169,113.341096)`; index 8 `(160,-160)` → `(95.297169,-77.253242)` |
| 8 / 07 | 1.785714286, -1 | 24, 176, +96 | 300 / 0 | index 0 `(-200,-200)` → `(-112,64)`; index 15 `(200,-200)` → `(112,64)` |
| 9 / 0d | 1.785714286, -1 | 12, 176, +320 | 300 / 0 | index 0 `(-200,-28.571429)` → `(-112,-32)`; index 15 `(200,-28.571429)` → `(112,-32)` |

Depth denominators `p+lev_scale` are 40/152/264 for Levels 1/7/8 and 28/140/252 for Level 9. The resulting continuous Atari-coordinate outputs and logical screen points are:

| Level | `Qx` for point 0, near / mid / far | `Qy` for point 0, near / mid / far | Screen point 0 `(x,y)`, `p=16 / 128 / 240` |
|---|---|---|---|
| 1 | 716.800 / 188.632 / 108.606 | 20.480 / 5.389 / 3.103 | (654.545,230.000) / (466.986,224.641) / (438.567,223.829) |
| 7 | -609.902 / -160.500 / -92.409 | 725.383 / 190.890 / 109.907 | (196.364,452.830) / (346.411,274.371) / (369.146,247.332) |
| 8 | -716.800 / -188.632 / -108.606 | 409.600 / 107.789 / 62.061 | (145.455,11.364) / (333.014,118.541) / (361.433,134.780) |
| 9 | -1024.000 / -204.800 / -113.778 | -292.571 / -58.514 / -32.508 | (145.455,184.091) / (349.091,125.909) / (371.717,119.444) |

For the partner points, the X values are the mirror about logical X=400. Y remains equal for the selected pairs on Levels 1/8/9; Level 7's partner has its own Y displacement and screen outputs (603.636/45.557, 453.589/167.194, 430.854/185.624). Thus the signed horizontal equation is symmetric even where the Level 7 point pair is not a two-axis mirror.

The vertical reference and signed offset both participate: each `dY` includes `128-lev_y3d`, while the output adds signed Y2D after the reciprocal term. In logical screen Y, the Y2D contributions are `e*lambda*Y2D`: -68.182, -48.079, -34.091, and -79.545 for Levels 1, 7, 8, and 9 respectively. Level 7's two chosen points retain different Y displacements; this is a property of the unchanged clover, not an index remap.

At a fixed sampled depth, all points/edge endpoints use the same affine transform; therefore applying it to tube endpoints and to an interpolated lane/entity point is internally consistent. Atari's integer floor and saturation are omitted by the continuous values above. These calculations establish architectural consistency only; they are not pixel matching or visual evidence.

## Relationship to the current portrait candidate

- The candidate Atari transform supplies the missing per-level base denominator, Y reference, and signed post-projection Y offset. Structurally, that replaces the role currently approximated by a shared `--portrait-perspective` correction; the correction is not an Atari parameter.
- The candidate base makes `--portrait-perspective 125` unnecessary as a component of Atari-style projection. This does **not** invalidate the manually useful Level 9 value or establish its visual effect. It could remain a separately gated diagnostic, but it must not be folded into Atari metadata.
- The frozen shared `-33.1` remains later host-space camera placement after logical projection (including any future base transform); at the current portrait output scale this is `1.2 * -33.1 = -39.72` host-window pixels. It is not equivalent to Y2D and is not tuned here.
- No screenshot/video/frame was examined, so there is no visual acceptance claim.

## Feasibility decision

**FEASIBLE WITH EXPLICIT LIMITATIONS**

The transform is mathematically coherent and can operate on immutable C# geometry. The renderer has a common world-point seam for tube/entity anchors, and the per-level parameters are independent of exact record point identity. No model geometry, lane, topology, collision, enemy, or simulation change is required.

Limitations to resolve or explicitly accept before implementation:

1. Level 7 is only an approximate frame fit, not a proven Atari record-06 point conversion; presentation arithmetic must not be presented as visual equivalence.
2. The depth mapping is an endpoint-normalized correspondence. Extrapolation beyond the tube interval is not evidence of literal Atari runtime values.
3. The near-anchored logical-unit conversion is a coherent C# translation, not a measured DVG-to-host pixel calibration. Integer floor/saturation are not reproduced by the floating-point form.
4. Post-projection vector glyph sizes and the separate Game Over coordinate frame need coordinated rendering treatment. Transition stars/HUD remain independent presentation layers.
5. Neal's manual visual/gameplay acceptance remains necessary after any future implementation; automated calculation cannot establish fidelity.

## Decision-relevant unknowns

| Unknown | Could change feasibility? | Could change seam? | Could require model geometry changes? | Could materially change validation plan? |
|---|---|---|---|---|
| Whether Level 7's current clover was intended as an Atari approximation, and whether its residuals were consciously accepted | No; a pointwise transform works either way | No | No; geometry is held fixed | No; Level 7 still needs the same manual presentation acceptance |
| Exact pixel/DVG-to-logical mapping and the actual visual effect of portrait value 125 | No; it changes calibration/evidence, not transformability | No | No | No; visual acceptance remains required in either case |
| Actual Math Box retained high-byte state and exact integer-call state described as unresolved in the handoff | No for the continuous, endpoint-normalized architecture; it limits literal integer equivalence | No | No | No for this milestone; only a future bit-exact comparison would depend on it |

These unknowns are non-blocking for the architecture decision. The most important non-blocking uncertainty is the eventual visual calibration, especially on Level 7; it cannot be resolved by this read-only numeric check.

## Smallest next milestone

Prepare one bounded implementation plan for a render-only adapter at `ProjectWorldPoint`, explicitly covering normalized depth, per-level frame conversion, post-projection glyph scaling, the separate Game Over coordinate input, preserved topology/geometry, automated registration invariants, and Neal's manual visual-acceptance step. Do not implement it in that milestone unless separately authorized.

## Scope and completion

- Prompt: [2026-11-15-presentation-layer-atari-transform-feasibility-prompt.md](../Prompts/2026-11-15-presentation-layer-atari-transform-feasibility-prompt.md).
- Report: this file.
- Files created: exactly the two authorized ignored AI-Logs artifacts above.
- Source files inspected: `Projection.cs`, `VectorRenderer.cs`, `TransitionStarfield.cs`, `DisplayConfiguration.cs`, `LevelDefinition.cs`, `LevelLoader.cs`, `LevelConfiguration.cs` (targeted lookup), `RomPresentationMetadata.cs`, and `Program.cs`; `.gitignore` was checked for the authorized artifact paths. The four controlling reports listed above were also used.
- Calculations: one inline deterministic four-level mapping calculation at depths 50/250/450 and normalized runtime positions 16/128/240; no script, chart, image, build, or test artifact.
- Starting Git state: branch `master`, HEAD `385f603a94391756b2ca6cccd524dc17b281e782`, upstream `origin/master`; index/staged diff empty. Tracked modifications: `src/Tempest.Game/Debug/DebugController.cs`, `src/Tempest.Game/Debug/PlaybackScreenshotCapture.cs`, `src/Tempest.Game/Levels/LevelLoader.cs`, `src/Tempest.Game/Program.cs`, `src/Tempest.Game/Rendering/DisplayConfiguration.cs`, `src/Tempest.Game/Rendering/Projection.cs`, `src/Tempest.Game/Rendering/VectorRenderer.cs`, `src/Tempest.Game/Systems/InputSystem.cs`, and the existing modified test files `DebugControllerTests.cs`, `DisplayConfigurationTests.cs`, `GameOverDensityDiagnosticTests.cs`, `GameplayRecordingTests.cs`, `InputSystemTests.cs`, `Level9ProductionTests.cs`, `PlaybackScreenshotCaptureTests.cs`, and `TransitionPresentationTests.cs`. Pre-existing untracked paths: `Reference/arcade/MBUCOD.V05.txt`, `Reference/arcade/MBUDOC.DOC`, `src/Tempest.Game/Levels/RomPresentationMetadata.cs`, and `tests/Tempest.Game.Tests/RomPresentationMetadataTests.cs`.
- Ending Git state: branch `master`, HEAD `385f603a94391756b2ca6cccd524dc17b281e782`, upstream `origin/master`; the tracked modifications and four pre-existing untracked paths exactly match the starting inventory above. Index/staged diff remains empty. The two created AI-Logs artifacts are ignored and do not appear in Git status.
- Index, staging, commit, and push: unchanged; nothing staged, committed, or pushed. No existing tracked file changed.
- Selected conclusion: **FEASIBLE WITH EXPLICIT LIMITATIONS**.
- Most important decision-relevant unknown: none identified.
- Most important non-blocking unknown: exact visual/DVG-to-logical calibration and Level 7's point-specific presentation fidelity.
- Manual acceptance: not performed; no game or media was opened.
