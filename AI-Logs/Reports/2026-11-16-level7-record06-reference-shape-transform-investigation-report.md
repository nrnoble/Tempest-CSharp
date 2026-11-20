# Level 7 Record-06 Reference-Shape Transform Investigation

Date: 2026-11-16
Status: **READ-ONLY INVESTIGATION COMPLETE**

## Direct answer

**Can the recovered Atari record-06 geometry be used to draw the Level 7 tube
with the same visible shape shown in `Level7_frame_0090-MAME.png`?**

**YES, CONDITIONALLY.** Record 06 already provides the correct ordered
16-point rims and same-index rails. A single global translation, similarity,
anisotropic scale, affine transform, or planar projective transform does **not**
align both the outer and deep structures in the two authorized images. A
depth-conditioned presentation transform does: a shared anisotropic gain,
with a different center offset for the far endpoint set, brings the visible
line arrangement to about 3.6 pixels RMS against the reference's blue-line
mask. Allowing separate near/far gains reduces that diagnostic residual only
slightly, to about 3.5 pixels.

The smallest useful candidate is therefore **horizontal compression relative
to vertical, plus a far-set-specific presentation offset**. No record-06 point,
index, adjacency, rail pairing, or topology change is indicated. The fitted
parameters are visual diagnostics only. They do not establish original Atari
monitor calibration or prove that this staged transform was historically used.

## Scope and evidence boundary

Only these two images were opened or measured:

1. Reference: `C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\Level-7\Level7_frame_0090-MAME.png`
2. Current direct-Atari C# prototype: `C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\Level-7\Prototype\Level-7 prototype-cs.png`

The nested prototype image is the intended comparison: the prior
[direct-Atari prototype report](./2026-11-16-level7-direct-atari-wireframe-prototype-report.md)
identifies that exact path. Other candidate files found by filename were not
opened. No other image, video, screenshot, frame, or visual asset was
inspected. The reference filename was treated only as the supplied shape
reference; no MAME implementation, settings, rendering, or behavior was
investigated.

Relevant implementation inspected:

- [AtariTubeRecordCatalog.cs](../../src/Tempest.Game/Levels/AtariTubeRecordCatalog.cs)
- [AtariCoordinateProjector.cs](../../src/Tempest.Game/Rendering/AtariCoordinateProjector.cs)
- [AtariTubeVectorListBuilder.cs](../../src/Tempest.Game/Rendering/AtariTubeVectorListBuilder.cs)
- [AtariDvgViewportMap.cs](../../src/Tempest.Game/Rendering/AtariDvgViewportMap.cs)
- [AtariPresentationAdapter.cs](../../src/Tempest.Game/Rendering/AtariPresentationAdapter.cs)
- [VectorRenderer.cs](../../src/Tempest.Game/Rendering/VectorRenderer.cs)
- [Projection.cs](../../src/Tempest.Game/Rendering/Projection.cs)
- [DisplayConfiguration.cs](../../src/Tempest.Game/Rendering/DisplayConfiguration.cs)
- [Program.cs](../../src/Tempest.Game/Program.cs)

Relevant project-local evidence inspected:

- [Tempest Rom Source code.md](../../Reference/arcade/Tempest%20Rom%20Source%20code.md),
  including `$c098`, `$c30d-$c3b9`, `$c3ee-$c452`, and `$c473-$c4e0`.
- [Level 7 direct-Atari wireframe prototype report](./2026-11-16-level7-direct-atari-wireframe-prototype-report.md).
- [Level 7 screenshot shape diagnostic report](./2026-11-16-level7-mame-vs-csharp-shape-diagnostic-investigation-report.md).
- [Record-06 segment reconstruction report](./2026-11-16-level7-record06-segment-reconstruction-report.md).
- [Atari visible X/Y aspect/gain report](./2026-11-16-atari-dvg-visible-xy-aspect-gain-readonly-investigation-report.md).
- [Atari/C# projection comparison report](./2026-11-15-atari-csharp-projection-comparison-report.md)
  and [coordinate-frame mapping report](./2026-11-15-atari-csharp-coordinate-frame-mapping-report.md).

No raw ROM copy was opened. The source and report inspection did not repeat
the RN.high/reset investigation. Factor-128 was not used to explain the
comparison; the selected C# screenshot is the factor-256 control already
identified in the prior report.

## Starting repository state

- Branch: `master`
- HEAD: `385f603a94391756b2ca6cccd524dc17b281e782`
- Upstream/tracking: `origin/master`, 0 ahead / 0 behind
- Index: clean
- Pre-existing tracked worktree modifications: 17 paths, including
  `.github/copilot-instructions.md`, rendering/gameplay code, and tests.
- Pre-existing nonignored untracked paths: 13, including direct-Atari
  prototype code/tests, local reference documents, and `screenshot000.png`.

All pre-existing changes and untracked material were preserved. The
task-required ignored prompt and report are the only paths created for this
investigation.

## Record-06 geometry and settled connectivity

Record 06 contains 16 ordered X/Y byte pairs:

```text
X: EA E0 9C 80 64 20 16 50 16 20 64 80 9C E0 EA B0
Y: A0 E0 EA B0 EA E0 A0 80 60 20 16 50 16 20 60 80
scale=$18 (24), y3d=$68 (104), signed y2d=$ff70 (-144), open=$00 (closed)
```

The catalog obtains the record data from the recovered row and Level 7
metadata. The builder projects the same source index at near runtime position
`$10` (16) and far position `$f0` (240), creates `Near[i]` to `Far[i]` rails,
and creates 16 cyclic edges for each closed rim.

The settled structural findings remain unchanged: point order, closed cyclic
rim adjacency, and same-index rail pairing match the reconstructed Atari
segment graph. The earlier segment report notes Atari's opposite traversal
direction and duplicate rail emission; neither changes the simple visible
segment graph. This investigation found no need for per-point movement,
reindexing, or connectivity changes.

## Current C# transformation chain

| Stage | Current operation |
|---|---|
| Record projection | For each endpoint position `p`, compute integer quotients from `abs(rawX-128)` and `abs(rawY-104)` over `p+24`, restore axis signs, and apply Y base `-144`. |
| Coordinate interpretation | The projector places the source-Y result in DVG X and source-X in DVG Y, then packs signed 13-bit values. Thus the raw axes are permuted at this boundary. |
| Vector construction | Near and far points retain their 16 indices. Same-index rails and cyclic rims are built without additional per-point transforms. |
| DVG viewport map | One shared `DevelopmentOnlyProvisional` map uses translation `(300,400)`, scale `0.4`, `displayAspect=1`, no swap, no rotation, X sign `+1`, and Y sign `-1`. The map supports uniform scale, X-only aspect, signs, axis swap, rotation, and translation, but carries no endpoint depth. |
| Prototype draw/camera | The direct branch draws all rails and both rims through that same map, then the portrait HUD camera applies zoom `1.2` and X offset `280`; the 720x960 portrait scissor is active. The playfield camera is not used for this prototype. |

For the current 1280x960 prototype image, the final screen mapping is:

```text
screenX = 640 + 0.48 * DVG_X
screenY = 480 - 0.48 * DVG_Y
```

This final map has equal X/Y pixel gain. The direct prototype does not call
`Projection.Project` or `AtariPresentationAdapter.Project`. The latter
contains a reciprocal, depth-normalized presentation projection for model
coordinates in other renderer paths, but it does not operate on this already
projected DVG vector list. Reapplying it here without an explicit coordinate
derivation would risk projecting the coordinates twice.

The direct prototype's immediate omission is therefore not a missing record
point or rim. It is that one equal-gain, one-origin final map is used for both
endpoint sets.

## Visual diagnostic measurements

**VISUAL DIAGNOSTIC MEASUREMENTS — not ROM data, Atari hardware calibration,
or production geometry.**

Blue-line pixel bounds were measured from only the two authorized images.
The reference mask excluded rows above 150 to omit the HUD. The line-pixel
predicate was `B > 80`, `B > 1.35*R`, and `B > 1.1*G`; this includes the
reference's antialiased blue shades.

| Image | Raster size | Blue tube bounds | Bounds aspect |
|---|---:|---:|---:|
| Reference `Level7_frame_0090-MAME.png` | 480x640 | x=97..382, y=170..543 | about 0.765 |
| C# `Prototype\Level-7 prototype-cs.png` | 1280x960 | x=319..970, y=154..805 | 1.000 |

Both show four broad lobes alternating with four inward wedges/notches, a
16-point perimeter rhythm, and a compact deep rim. The outer silhouette's
first-order difference is horizontal width relative to height. However,
aligning the outer rim alone does not align the deep rim: the reference's
deep-rim center is displaced down and toward the near-rim center compared
with the C# map's deep-rim center. This changes rail directions and
convergence, so it is not merely a different absolute placement of the whole
tube.

### Landmarks and fitting method

The comparison used all 16 near-rim and all 16 far-rim record indices, plus
the 16 same-index rails and both 16-edge cyclic rims. The C# pixel coordinates
were reconstructed from the current projector, builder, map, and camera
equations and checked against the selected C# image's blue bounds. Candidate
transforms were fitted against the reference blue-pixel mask. Residuals below
are the RMS distance, in reference-raster pixels, from 2-pixel-spaced
transformed segment samples to the nearest qualifying reference blue pixel.
This is a one-sided raster-distance diagnostic, not a claim of exact analog
hardware calibration. Overlaid/occluded central strokes and antialiasing can
contribute to residuals.

## Transformation tests

All parameters below are diagnostic fits between current C# screenshot
coordinates `(x,y)` and the 480x640 reference raster. Absolute size and
placement were allowed to vary. They must not be copied into record geometry.

| Model | Degrees of freedom / fitted form | Residual | Finding |
|---|---|---:|---|
| Translation only | 2; identity linear part, bounds-center alignment | ~21.3 px RMS | Fails: raster width/height and internal arrangement remain different. |
| Uniform scale + translation | 3; `sR(x,y)+t`; best orientation-preserving fit was about `s=0.452`, rotation about 0 degrees | ~10.4 px RMS | Fails: the tall reference cannot be reached with one uniform scale. |
| Reflection + uniform scale + translation | 4; reflected similarity, with axis/rotation alternatives checked | best ~11.9 px RMS | Fails; reflection/rotation does not solve the relative axis gain or deep-rim placement. |
| Global anisotropic X/Y scale + translation | 4; approximately `diag(0.435, 0.573)` with translation | ~6.3 px RMS | Corrects most outer width/height difference (relative X/Y gain about 0.76), but far-rim and rail convergence still miss. |
| General global affine | 6; approximately `[[0.433,0.003],[0.001,0.574]]` plus translation | ~6.3 px RMS | No meaningful improvement over axis-aligned anisotropic scale; fitted shear/rotation terms are near zero. |
| Global planar projective map | 8; affine terms plus two denominator terms | ~6.3 px RMS | No useful improvement; fitted denominator terms returned to zero within the search resolution. |
| Separate near/far maps with shared anisotropic gains | 6; common gains about `(0.4215,0.575)`, separate translations | ~3.6 px RMS | Sufficient practical candidate. Far set needs roughly `(+27,+37)` reference-raster pixels relative to the near set after the common gains. |
| Separate near/far anisotropic maps | 8; near gains about `(0.428,0.573)`, far `(0.400,0.517)`, separate translations | ~3.5 px RMS | Slightly lower residual, but only about 0.2 px better; extra per-depth gain freedom is not justified by this diagnostic. |
| Separate near/far general affine maps | 12 | ~3.4 px RMS | Only a further negligible reduction; no per-point fit is needed. |

The smallest effective family is consequently a **single common anisotropic
gain plus a far-endpoint-set-specific translation**. A convenient
visual-diagnostic parameterization is:

```text
Tnear(x,y) = (aX*x + tNearX, aY*y + tNearY)
Tfar (x,y) = (aX*x + tNearX + 27, aY*y + tNearY + 37)
aX/aY ≈ 0.73 to 0.77
```

The ratio range reflects the bounds and line-mask fits; the small spread is
expected from screenshot rasterization, line overlap, and the different
weighting of endpoints versus long segments. This is a candidate family, not a
selected Atari constant.

Translation, uniform similarity, global anisotropic scale, general affine,
global projective, and the depth-conditioned maps all preserve point identity
and cyclic adjacency when applied to the existing endpoints. The depth
condition changes far endpoint placement and therefore rail angle/convergence,
but rails remain `Near[i]` to `Far[i]`; each rim remains cyclic and closed.
Within each rim, an affine transform preserves opposite/repeated relationships.
No independent per-vertex adjustment is required.

## Atari evidence versus practical sufficiency

### Atari/ROM-established

- The local source identifies record selection and projects the point set at
  the two distinct positions (`$10` and `$f0`) through `$c098`.
- The projector uses the record's X/Y references and common depth/scale
  arithmetic, applies the Y2D base, and serializes source-Y-derived data to
  vector X and source-X-derived data to vector Y.
- Closed record 06 has cyclic rim edges; the record-specific near/far passes
  retain corresponding source indices.
- These are software/vector-list facts. The local materials inspected do not
  establish the physical monitor's final X/Y gain or a depth-specific
  post-vector center shift matching the screenshot fit.

### C# implementation-established

- The catalog, coordinate projector, builder, shared provisional map, direct
  renderer branch, and HUD camera have the operations described above.
- The map's `displayAspect=1.0` and one shared viewport instance are the
  explicit cause of equal final pixel gain and a shared origin in this
  prototype.
- The map type can express anisotropic X/Y gain and translation. The renderer
  could use separate map instances for near and far endpoints without
  redesigning the record or segment model; the current draw path does not do
  so.
- The ordinary `AtariPresentationAdapter.Project` is depth-aware, but it is
  not used by the direct DVG prototype and is not, by itself, evidence that
  this staged adjustment is Atari-correct.

### Mathematical and visual-diagnostic findings

- A global transform is not enough to place both rims and rails into the
  reference arrangement.
- A common horizontal-vs-vertical gain plus a far-set-specific offset
  substantially reduces the residual without changing geometry or topology.
- The screenshot fit supports a bounded **DEVELOPMENT-ONLY** implementation
  candidate to test this presentation family. It does not establish that the
  fitted gains or offset were used by Atari hardware.

### Historical hardware unknowns

- The original cabinet's calibrated X/Y analog gain and monitor mapping remain
  unresolved.
- The screenshot-derived far-set offset has no independent Atari hardware
  provenance. Its historical cause or location in the original display chain
  remains unknown.

## Disposition and candidate direction

The prior shape diagnostic correctly identified the square-versus-tall outer
bounds, but treating that as the complete explanation was insufficient:
fitting the geometry generated from all 32 endpoints and the rails shows a
residual near/far center and convergence mismatch under every single global
transform tested.

The direct plain-language cause is: **the prototype applies the same
equal-gain, same-origin viewport to both already-projected endpoint sets.**
The reference shape is consistent with compressing X relative to Y and
presenting the far set with a different origin. The local ROM evidence
already accounts for the record projection at near/far positions; it does not
prove the additional final display mapping.

There is enough evidence for a bounded **DEVELOPMENT-ONLY** candidate:

1. Keep all record-06 bytes, projection, order, and connections unchanged.
2. At the presentation boundary, use a shared anisotropic X/Y viewport gain.
3. Apply a depth-conditioned translation to the far endpoints relative to the
   near endpoints; keep each rail paired by the existing index.
4. Treat the fitted values only as visual calibration candidates, never as
   ROM or hardware constants. Do not add screenshot-derived point coordinates.

This is a recommendation only. No code, tests, geometry, projection, viewport,
presentation, gameplay, or durable project documentation was changed, and no
candidate was implemented.

## Validation and preservation

- Quantitative comparison completed for the two named images only.
- Source/read-only coordinate calculations and line-mask fits completed.
- Build and tests were not applicable to this read-only investigation.
- The interactive game was not launched; no manual gameplay acceptance is
  claimed or requested.
- No protected image, recording, video, reference asset, or other user
  material was changed.
- No branch, commit, index, tracked worktree file, or pre-existing untracked
  file was changed. No source, tests, geometry, projection, viewport,
  presentation, or gameplay state was modified.
- Only the required ignored prompt and this report were created; neither was
  staged or committed.

Saved records:

- Prompt: `C:\Users\nrnob\source\repos\Tempest\AI-Logs\Prompts\2026-11-16-level7-record06-reference-shape-transform-investigation-prompt.md`
- Report: `C:\Users\nrnob\source\repos\Tempest\AI-Logs\Reports\2026-11-16-level7-record06-reference-shape-transform-investigation-report.md`
