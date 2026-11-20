# Cross-level presentation-proportion investigation

**Status: COMPLETE for source/transform mathematics; visual pixel comparison remains unverified.**

## Direct answers

1. **Is Level 7 broader before the viewport?** Its production point cloud is not intrinsically broader in X than Y: its bounding-box W:H is exactly `1.000`; its centered RMS X:Y is also `1.000`. This is also true of production Levels 1, 2, and 5.
2. **Where does Level 7 first diverge?** There is no X:Y divergence from Level 1 anywhere in the calculated geometry or presentation route. Relative to Levels 2 and 5, their adapter branches introduce vertical-only scale, making their projected W:H about `0.761`; Level 7 remains `1.000`. The first difference versus those two controls is therefore the level-specific adapter transform, not Level 7 geometry, model-frame anisotropy, shared projection, or viewport.
3. **Does production Level 7 geometry explain an additional horizontal gain?** No. It has equal X/Y bounds and equal centered RMS extents before presentation. The supplied manual observation that its visual comparison appears broad is retained, but is not explained by a Level-7-only X:Y gain in this pipeline.
4. **How does it compare with record 06?** Record 06 also has equal X/Y bounds and a centered RMS X:Y ratio of `0.994683`. Its point sequence approximately maps to the production clover by a uniform scale of `1.678958593`, a 180-degree rotation, and a cyclic point-index offset; the least-squares residual is nonzero. No meaningful anisotropic map is needed.
5. **Does the adapter add a Level-7-only X:Y gain?** No. Its generic Level 7 Jacobian is diagonal with equal-magnitude X/Y gains. Level 7's distinct model-frame value is a scalar, not an axis ratio.
6. **Does the shared camera or viewport create a Level-7-only difference?** No. The host camera is uniform; the portrait viewport is a uniform enlargement/clip and is common to all levels. It cannot change one level's ratio alone.
7. **Why do Levels 1, 2, and 5 look closer?** The math predicts that Level 2 and Level 5 are about 24% narrower in W:H than Level 1/7 due to separate, nearly matching vertical stretches (`1.314` and `.880/.670`). Level 1 is isotropic like Level 7, so this mechanism does not explain the reported Level-1/Level-7 visual difference.
8. **Do Levels 8 and 9 support the same conclusion?** Yes. Their model-frame conversions are isotropic in magnitude (Level 8/9 reflect Y only). Projection preserves their intrinsic bounds ratios: Level 8 `1.000`, Level 9 `1.750`. The supplied manual acceptance of Level 9, despite its broader geometry, reinforces that visual matching is level/reference-specific rather than a global viewport ratio.
9. **Is a Level-7-only correction justified?** Not from the available numerical evidence. A specific reference ratio cannot be measured because no comparison image was attached to this task. Do not change geometry, adapter, or viewport on this investigation alone.
10. **Smallest future experiment, if later authorized?** If a matched visual comparison establishes that Level 7 needs a ratio change, test one Level-7-only presentation-space Y gain about a fixed anchor, applied consistently to tube/entity projection and local scale. No gain value is selected here. Do not change gameplay points or global viewport.

## Scope, evidence, and method

This was a bounded read-only source and mathematics investigation. No source, tests, configuration, durable documentation, user assets, recordings, or Git state were changed. The only writes are this report and the paired prompt under ignored `AI-Logs/`.

The task supplied qualitative **MANUAL OBSERVATIONS** for Levels 1, 2, 5, 7, 8, and 9. These are treated as supplied evidence and are not newly claimed as `ARCADE-OBSERVED` findings. The current task attachment contained text only. **No comparison images were available as task attachments, no image pixels were measured, and no workspace images/videos/frames were inspected.** Therefore the exact visual ratio and capture-state explanation remain unknown.

### Sources inspected

- `src/Tempest.Game/Levels/LevelLoader.cs:75-176` — production point arrays and Level 8/9 normalization.
- `src/Tempest.Game/Levels/LevelConfiguration.cs:135-160,247-259,307-319,346-390` — production configurations and their selected definitions.
- `src/Tempest.Game/Levels/Level.cs:17-19` — shared depth and position (`400`, `50`).
- `src/Tempest.Game/Levels/RomPresentationMetadata.cs:57-89` — displayed-level metadata catalog.
- `src/Tempest.Game/Levels/AtariTubeRecordCatalog.cs:63-98` — preserved ROM/Atari-backed Level 7 record 06.
- `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs:75-114,128-178,232-453,467-482` — placements, Level 5 branch, generic projection, axis factors, and model-frame mappings.
- `src/Tempest.Game/Rendering/VectorRenderer.cs:215-313` — production geometry projection route and metadata dispatch.
- `src/Tempest.Game/Rendering/Projection.cs:40-118` — shared projection equation.
- `src/Tempest.Game/Rendering/DisplayConfiguration.cs:14-71` — current host, viewport, output scale, and playfield camera.
- `src/Tempest.Game/Program.cs:236-239` — ordinary projection defaults (`K=70`, `ZOffset=5`, portrait perspective default `0`).
- `TEMPEST-BEHAVIOR-SPEC.md:152-178,2926-2990,3000-3035` and `TEMPEST-DEVELOPMENT-NOTES.md:90-110,4263-4292` — evidence classifications, Level 7 acceptance boundary, and accepted viewport contract.
- `.github/copilot-instructions.md` — read to follow the task's repository and logging constraints.
- `AI-Logs/Copilot.txt` was opened incidentally but is unrelated to this investigation and was not used as evidence.

No web sources, alternative implementations, raw ROM files, or protected user visual material were inspected.

### Calculation conventions

- Geometry statistics use the 16 production points as listed, before projection. Geometric center is the arithmetic point centroid. RMS extents are the square roots of mean squared coordinate deviations from that centroid.
- Level 8 and 9 production points use the code's `(raw - 128) * 25/14` transform, with Y reflected. Level 7 record 06 is also analyzed as raw bytes centered at `(128,128)`; reflecting Y would not change the reported extent ratios.
- Playing depths are `near=Position=50` and `far=Position+Depth=450`; ordinary approach offsets are zero. `K=70`, `ZOffset=5`, and `PortraitPerspective=0`.
- For projection tables, bounds and RMS ratios are measured over each level's 16-point cross-section at that depth. Final Y placement is then shown separately; it is a translation and does not alter those bounds or ratios.

## Production geometry before projection

All values are in each `LevelDefinition.Points` model-coordinate units. The reported centroids are not substituted for the source points.

| Level | Geometry source / provenance | Normalization in `LevelLoader` | Topology | Min X | Max X | Width | Min Y | Max Y | Height | Centroid (X,Y) | RMS X | RMS Y | W:H | RMS X:Y |
|---|---|---|---|---:|---:|---:|---:|---:|---:|---|---:|---:|---:|---:|
| 1 | C# 16-point circular/test-wave definition; no ROM provenance asserted here | None | Closed | -200 | 200 | 400 | -200 | 200 | 400 | (0, 0) | 140.990 | 140.990 | 1.000000 | 1.000000 |
| 2 | C# 16-point square definition | None | Closed | -161 | 161 | 322 | -161 | 161 | 322 | (0, 0) | 133.419 | 133.419 | 1.000000 | 1.000000 |
| 5 | C# 16-point cross; Level 5 cross is labeled `REFERENCE VERIFIED` in configuration notes | None | Closed | -210 | 210 | 420 | -210 | 210 | 420 | (0, 0) | 125.499 | 125.499 | 1.000000 | 1.000000 |
| 7 | Production clover is `REFERENCE-DERIVED` authority; not a literal record-06 conversion | None | Closed | -179 | 179 | 358 | -179 | 179 | 358 | (0, 0) | 126.033 | 126.033 | 1.000000 | 1.000000 |
| 8 | Rev 2 shape row 7 raw coordinates; `ROM-BACKED` data with accepted `PROJECT TRANSLATION` | Center each raw axis at 128; multiply by 25/14; reflect Y | Open | -200 | 200 | 400 | -200 | 200 | 400 | (1.339, 0) | 127.537 | 130.931 | 1.000000 | 0.974082 |
| 9 | Rev 2 shape row 13 raw coordinates; `ROM-BACKED` data with `PROJECT TRANSLATION` | Center each raw axis at 128; multiply by 25/14; reflect Y | Open | -200 | 200 | 400 | -28.571 | 200 | 228.571 | (0, 85.714) | 130.931 | 69.985 | 1.750000 | 1.870829 |

Level 7's equal bounds and equal RMS extents are already present in its source coordinates. No projection or viewport is needed to create that equality. Levels 1/2/5 likewise start at `1:1`; Levels 8/9 demonstrate that the production geometry itself can have a non-square proportion.

## Level 7 production clover vs. preserved record 06

The record catalog identifies raw record 06 as ROM/Atari-backed and independent of the production definition. It is not activated by ordinary Level 7 presentation.

| Dataset | X bounds / width | Y bounds / height | Centroid in analyzed coordinates | RMS X | RMS Y | W:H | RMS X:Y |
|---|---|---|---|---:|---:|---:|---:|
| Production clover | [-179,179] / 358 | [-179,179] / 358 | (0,0) | 126.033 | 126.033 | 1.000000 | 1.000000 |
| Raw record 06 | [22,234] / 212 | [22,234] / 212 | (128,128) raw; (0,0) centered | 74.813 | 75.213 | 1.000000 | 0.994683 |

**Point-order and fit test:** a best point correspondence is record point `i` to production point `(i+3) mod 16`, without reversing point order. The symmetric data also permits equivalent cyclic shifts/rotations. The centered record points fit production with the uniform similarity:

`production[(i+3) mod 16] ≈ -1.678958593 × (record[i] - (128,128))`

The negative scalar is a 180-degree rotation, not an X:Y correction. This similarity fit has RMS point residual `6.6953` and maximum residual `9.7811` model units, so the two point sets are close but not exactly related by one global map. The fitted scale matches the Level 7 adapter's `1.678958593` units-per-raw-unit value to rounding.

An unconstrained affine fit only reduces RMS residual to `6.6739` (maximum residual `9.5789`); its centered axis scales are approximately `-1.68403` and `-1.67394`, an absolute gain ratio of `1.00602`, with zero fitted translation. It still has nonzero residual. **Classification:** approximately a uniform similarity; neither anisotropic scale nor general affine behavior is needed to explain a material X:Y difference. Production's RMS X:Y ratio is only about `0.535%` more horizontal than record 06's; their bounding-box ratios are identical. This comparison does not establish exact coordinate identity or promote the production clover to ROM-backed status.

## Active configuration, metadata, model frame, and placement

The shared gameplay configuration has `Position=50`, `Depth=400`. `CenterY` and `YOffset` below come from each loaded definition. They affect screen placement/centerline behavior, not the within-ring X:Y Jacobian.

| Level | CenterY / YOffset | Geometry source and normalization | Active metadata | Active model-frame behavior | Final adapter Y placement |
|---|---|---|---|---|---:|
| 1 | 400 / -80 | Direct C# circle; none | Row 0, scale `18`, camera `50`, signed Y2D `-192`, closed. Fetched/validated; Level 1 branch does not use its ROM projection values. | Getter reports units/raw `25/14`, Y orientation `+1`; Level 1 branch projects source model X/Y directly, so effective pre-projection mapping is identity. | +122 |
| 2 | 400 / -80 | Direct C# square; none | Row 1, scale `1C`, camera `50`, signed Y2D `-224`, closed. Fetched/validated; Level 2 branch does not use its ROM projection values. | Getter reports `1.436688312`, `+1`; branch projects model X/Y directly. Effective model-frame mapping is identity; Level 2 then applies its separate Y presentation factor. | +114 |
| 5 | 415 / -90 | Direct C# cross; none | Catalog row 4: scale `18`, camera `50`, signed Y2D `-192`, closed; bypassed by the special `ProjectLevel5` route. | Identity model input; special projection multiplies projected X/Y displacements by `.670` / `.880`. | +136.4026 |
| 7 | 310 / -10 | Reference-derived production clover; none | Row 6, scale `18`, camera `68`, signed Y2D `-144`, closed; consumed by generic Atari adapter. | Units/raw `1.678958593`, Y orientation `+1`; `dRaw/dModel = (.595607,.595607)`, isotropic. | +228.99702 |
| 8 | 300 / 0 | ROM row 7, centered/scaled/reflected as above | Row 7, scale `18`, camera `B0`, signed Y2D `+96`, open; consumed by generic adapter. | Units/raw `25/14`, Y orientation `-1`; `dRaw/dModel = (.56,-.56)`, isotropic magnitude plus Y reflection. | +212.28139 |
| 9 | 300 / 0 | ROM row 13, centered/scaled/reflected as above | Row 13, scale `0C`, camera `B0`, signed Y2D `+320`, open; consumed by generic adapter. | Units/raw `25/14`, Y orientation `-1`; `dRaw/dModel = (.56,-.56)`, isotropic magnitude plus Y reflection. | +180.96825 |

The Level 7 camera-height term and signed Y2D enter its vertical baseline/displacement, but neither supplies a distinct horizontal or vertical local gain. All Level-specific placements listed here are vertical translations after the projected point calculation; they do not change per-depth width, height, W:H, or RMS X:Y.

## Adapter Jacobian and projected proportions

The Jacobian below maps production model-coordinate deltas `(dx,dy)` to pre-camera logical presentation deltas `(dScreenX,dScreenY)`. Values are logical pixels per model unit. For Levels 7-9, the scalar before the sign is `K(16+s)/(55(r+s))`, where `s` is metadata scale and `r` is the adapter reciprocal depth (`16` at near and `240` at far). Cross terms are zero.

| Level | Near Jacobian `(dX/dx, dY/dy)` | Far Jacobian `(dX/dx, dY/dy)` | Axis swaps/reflections | |X gain| / |Y gain| |
|---|---|---|---|---:|
| 1 | (1.272727, 1.272727) | (0.153846, 0.153846) | None | 1.000000 |
| 2 | (1.272727, 1.672364) | (0.153846, 0.202154) | None; Y-only factor 1.314 | 0.761035 |
| 5 | (0.852727, 1.120000) | (0.103077, 0.135385) | None; post-projection scales .670/.880 | 0.761364 |
| 7 | (1.272727, 1.272727) | (0.192837, 0.192837) | None | 1.000000 |
| 8 | (1.272727, -1.272727) | (0.192837, -0.192837) | Y reflected, no axis swap | 1.000000 |
| 9 | (1.272727, -1.272727) | (0.141414, -0.141414) | Y reflected, no axis swap | 1.000000 |

At each individual depth, projection results are:

| Level | Near projected W × H | Near W:H | Near RMS X:Y | Far projected W × H | Far W:H | Far RMS X:Y |
|---|---:|---:|---:|---:|---:|---:|
| 1 | 509.091 × 509.091 | 1.000000 | 1.000000 | 61.538 × 61.538 | 1.000000 | 1.000000 |
| 2 | 409.818 × 538.501 | 0.761035 | 0.761035 | 49.538 × 65.094 | 0.761035 | 0.761035 |
| 5 | 358.145 × 470.400 | 0.761364 | 0.761364 | 43.292 × 56.862 | 0.761364 | 0.761364 |
| 7 | 455.636 × 455.636 | 1.000000 | 1.000000 | 69.036 × 69.036 | 1.000000 | 1.000000 |
| 8 | 509.091 × 509.091 | 1.000000 | 0.974082 | 77.135 × 77.135 | 1.000000 | 0.974082 |
| 9 | 509.091 × 290.909 | 1.750000 | 1.870829 | 56.566 × 32.323 | 1.750000 | 1.870829 |

Applying the per-level Y translations does not alter any of the preceding cross-section dimensions or ratios. As a secondary check, the union of the 32 endpoint vertices (16 at near plus 16 at far, after final placement) has W:H `1.000` for Level 1, `0.761` for Level 2, `0.761` for Level 5, `1.000` for Level 7, `1.000` for Level 8, and `1.432` for Level 9. This endpoint-cloud check is not a pixel measurement of the rendered rail strokes.

### Cross-level normalization summary

This is the requested one-row-per-level summary. “Model-frame gains” are effective raw-coordinate gains for a production model delta (identity where the special branch bypasses the getter mapping); the separate effective X/Y gain ratio is the adapter's projected gain magnitude ratio.

| Level | Geometry source | Model width × height | Model W:H | RMS X:Y | Model-frame X / Y gain | Effective adapter X/Y gain ratio | Near projected W:H | Far projected W:H | Final host X / Y gain | Supplied visual manual classification |
|---|---|---:|---:|---:|---:|---:|---:|---:|---:|---|
| 1 | C# circular/test-wave definition | 400 × 400 | 1.000000 | 1.000000 | 1 / +1 | 1.000000 | 1.000000 | 1.000000 | 1.68 / 1.68 | Reasonably close |
| 2 | C# square definition | 322 × 322 | 1.000000 | 1.000000 | 1 / +1 | 0.761035 | 0.761035 | 0.761035 | 1.68 / 1.68 | Reasonably close |
| 5 | C# cross definition | 420 × 420 | 1.000000 | 1.000000 | 1 / +1 | 0.761364 | 0.761364 | 0.761364 | 1.68 / 1.68 | Reasonably close |
| 7 | Reference-derived production clover | 358 × 358 | 1.000000 | 1.000000 | 0.595607 / +0.595607 | 1.000000 | 1.000000 | 1.000000 | 1.68 / 1.68 | Noticeably broader vs. reference |
| 8 | ROM row 7 + centering/scale/Y reflection | 400 × 400 | 1.000000 | 0.974082 | 0.56 / -0.56 | 1.000000 | 1.000000 | 1.000000 | 1.68 / 1.68 | Generally acceptable under enlarged viewport |
| 9 | ROM row 13 + centering/scale/Y reflection | 400 × 228.571 | 1.750000 | 1.870829 | 0.56 / -0.56 | 1.000000 | 1.750000 | 1.750000 | 1.68 / 1.68 | Generally acceptable under enlarged viewport |

Final host gain is the current total playfield camera gain, uniform on both axes; it includes the base `1.6` output scale and shared `1.05` playfield factor. The separate `4/3` value is the accepted enlargement relative to the former viewport, also uniform on both axes.

### First-divergence finding

- **Geometry stage:** Levels 1, 2, 5, and 7 all have production W:H `1.000` and RMS X:Y `1.000`. Level 7 does not become wider here.
- **Model-frame stage:** Level 7 applies a different *uniform* scalar from Levels 1/2/8/9. A uniform scalar cannot change X:Y. Level 1/2 retrieve mappings but their special branches consume model X/Y directly. Level 8/9 use equal-magnitude X/Y conversion with a Y reflection.
- **Per-level adapter stage:** Level 2 applies Y factor `1.314`; Level 5 applies projected displacement gains `.670` X and `.880` Y. These make both about `31.4%` narrower in W:H than an isotropic `1:1` result. Level 7 adds no such axis factor. This is the first mathematical distinction between Level 7 and Levels 2/5, but not between Level 7 and Level 1.
- **Depth projection:** Levels 1 and 7 have different near-to-far scale progressions (Level 7's far gain is about `25.34%` larger than Level 1's), but each level's local X and Y gains remain equal. The Level 7 depth law is not a Level-7-only anisotropic gain.
- **Placement and host transform:** translations and common camera/viewport transforms do not introduce a Level-7-only ratio.

Thus the equations do not prove why the supplied Level 7 side-by-side image looks broad while Level 1's looks close. The remaining possibilities include reference-specific geometry/perspective or differing capture/composition state; choosing among them needs the authorized comparison images and matched capture state.

## Shared portrait camera and viewport proof

The frozen display contract is host `1600×1280`, viewport `960×1280` at `(320,0)`, aspect `3:4`, with equal `320`-pixel side margins. `PortraitOutputScale` is `960/600 = 1280/800 = 1.6`. The playfield `Camera2D.Zoom` is `PortraitPlayfieldZoom = 1.6 × 1.05 = 1.68`; this is the total logical-to-host playfield scale, not a second factor to multiply after the camera. Its X and Y gains are both `1.68`; camera target/offset translate but do not change either gain.

The accepted enlargement from `720×960` to `960×1280` is `960/720 = 1280/960 = 4/3` on both axes. The viewport clips the host output; it does not apply an additional nonuniform coordinate transform. Consequently every level's W:H and RMS X:Y remain unchanged through the shared camera and viewport. These common transforms cannot explain a Level-7-only proportion difference. **The viewport remains frozen and is not recommended for reconsideration.**

## Evidence classification, boundaries, and future work

- User-provided cross-level visual statements: **MANUAL OBSERVATIONS**; no images or pixel measurements were independently reviewed here.
- Production Level 7 point coordinates: **REFERENCE-DERIVED** production authority, per accepted project documentation.
- Preserved record 06 and Levels 8/9 source coordinate bytes: **ROM-BACKED**.
- Level 8/9 centering, scaling, and Y reflection; Level-specific placements; Level 2/5 axis gains; shared viewport/camera decisions: existing **PROJECT TRANSLATIONS / PROJECT DECISIONS**, not Atari presentation constants.
- Calculated ratios, Jacobians, fits, and point-cloud envelope: **MATHEMATICAL DATASET COMPARISON / INFERENCE** about the current implementation, not arcade observation.

The production Level 7 gameplay geometry, preserved record 06, accepted translations, and viewport are all left unchanged. No correction is approved by these results. If a later, authorized, image-grounded comparison confirms a Level-7-specific vertical proportion mismatch, test only a presentation-space Y gain about a fixed anchor and ensure all tube/entity presentation and local-scale consumers remain consistent. Do not change Level 7 gameplay/collision points or the global viewport.

The Level 2 prototype is recorded only as qualitative long-term context: a large dominant portrait tube, a stable HUD/status region above it, the tube occupying most of the gameplay area below the HUD, and a presentation suitable for direct arcade/reference comparison. It was not inspected or measured and supplies no current constants or acceptance criteria.

## Validation and Git preservation

**Calculation check: PASS.** Bounded Python arithmetic evaluated the production point statistics, Level 7/record-06 point-order similarity and affine fits, adapter Jacobians, near/far projected extents, and uniform viewport ratios. No project tests or build were run because this task changes no executable code.

Starting and finishing Git state:

| Field | Start | Finish |
|---|---|---|
| Branch | `master` | `master` |
| HEAD | `7ba80f39df9daeaa86a7cafc81690f2f5c37bb33` | `7ba80f39df9daeaa86a7cafc81690f2f5c37bb33` |
| Upstream | `origin/master` | `origin/master` |
| Ahead / behind | `0 / 0` | `0 / 0` |
| Index | Empty | Empty |
| Tracked worktree | Pre-existing modifications to `.github/copilot-instructions.md`, `TEMPEST-BEHAVIOR-SPEC.md`, `TEMPEST-DEVELOPMENT-NOTES.md`, `src/Tempest.Game/Rendering/DisplayConfiguration.cs`, `src/Tempest.Game/Rendering/VectorRenderer.cs`, `tests/Tempest.Game.Tests/AtariPresentationAdapterTests.cs`, `tests/Tempest.Game.Tests/DisplayConfigurationTests.cs`, `tests/Tempest.Game.Tests/Level8AtariPresentationTests.cs`, and `tests/Tempest.Game.Tests/Level9AtariPresentationTests.cs` | Same pre-existing modifications; no tracked files changed by this task |
| Relevant untracked state | `Reference/arcade/MBUCOD.V05.txt`, `Reference/arcade/MBUDOC.DOC`, and `tests/Tempest.Game.Tests/PreviousPortraitHostTransform.cs` | Same; no untracked project files changed by this task |

The existing enlarged-viewport candidate and all unrelated instruction/reference/test-helper changes were preserved. The only task writes are the ignored log files:

- Prompt: `AI-Logs/Prompts/2026-11-19-cross-level-presentation-proportion-investigation-prompt.md`
- Report: `AI-Logs/Reports/2026-11-19-cross-level-presentation-proportion-investigation-report.md`

No staging, commit, push, branch switch, stash, reset, restore, cleanup, game launch, or manual gameplay acceptance was performed.
