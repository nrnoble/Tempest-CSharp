# Level 7 MAME vs. C# Tube-Shape Investigation

Date: 2026-11-16
Status: **READ-ONLY INVESTIGATION COMPLETE**

## Direct answer

**Why is C# not rendering the Level 7 tube with the same shape as MAME?**

The supplied screenshots do not establish a remaining structural tube-shape
mismatch after the requested normalization. Both show the same four broad
lobes, four inward notches, 16-point boundary pattern, and central convergence.
The conspicuous difference is the C# output's square-looking final raster
shape versus MAME's taller raster shape; the measured blue-tube bounding-box
aspect ratios are 1.00 for C# and 0.763 for MAME. That difference is consistent
with the C# prototype's provisional unit-aspect DVG-to-pixel map versus the
MAME screenshot's raster/display aspect. It is an ordinary final aspect
difference, which the task explicitly excludes from the shape question.

The C# screenshot is also the factor-256 control, not the factor-128
experiment: its measured blue-line bounds match the control's predicted
coordinates exactly under the current viewport. The factor-128 experiment
would change overall scale, not correct the normalized point relationships.

Accordingly, the evidence does not justify a geometry or topology correction.
The one structural detail still worth resolving before any correction prompt
is the exact record-06 near/far rail endpoint list emitted by the Atari
vector-building routine; the visible screenshots do not show a convincing
wrong-index connection, and the disassembly strongly supports the prototype's
index correspondence, but the complete `$c3ee`/`$c43c` vector construction was
not fully decoded here.

## Scope and evidence inspected

Only the two user-authorized images were opened:

- `UserData/Screenshots/Level-7/Prototype/Level-7 MAME.png`
- `UserData/Screenshots/Level-7/Prototype/Level-7 prototype-cs.png`

Relevant current implementation was inspected in:

- `src/Tempest.Game/Levels/AtariTubeRecordCatalog.cs`
- `src/Tempest.Game/Levels/RomPresentationMetadata.cs`
- `src/Tempest.Game/Rendering/AtariCoordinateProjector.cs`
- `src/Tempest.Game/Rendering/AtariTubeVectorListBuilder.cs`
- `src/Tempest.Game/Rendering/AtariDvgViewportMap.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `src/Tempest.Game/Rendering/DisplayConfiguration.cs`

Relevant project-local evidence was inspected in:

- `Reference/arcade/Tempest Rom Source code.md`, especially `$c098`,
  `$c235-$c2c3`, `$c30d-$c3b9`, `$c473-$c4e0`, and `$c4e1-$c542`.
- Existing Level 7 prototype, factor-128, normal Math Box call-state,
  RN.high startup-state, Atari projection-data, DVG-translation, and Level 7
  geometry-provenance reports under `AI-Logs/Reports/`.

No other screenshots, videos, frames, or visual assets were inspected. Raw
ROM/PROM contents were not opened. No game launch, build, or test run was
needed for this read-only visual/code comparison.

## What the two screenshots show

### MAME

The blue tube has four broad outer lobes arranged around the center. Four
narrow inward wedges/notches alternate with those lobes. The boundary has the
same repeated 16-point rhythm seen in record 06, with opposite features
repeating across the center. Long rails converge toward the small deep rim;
the four wedge boundaries and the broad lobe boundaries remain distinct.

The dominant blue tube-line pixels span approximately `x=98..381`,
`y=171..542` (284 by 372 pixels; aspect 0.763). This is an observation about
this image's raster, not proof of the physical Atari monitor's calibrated
aspect.

### C# direct-Atari prototype

The blue wireframe likewise shows four broad lobes, four inward notches, a
16-point perimeter pattern, and convergence toward a small deep rim. Its
perimeter and rails appear to preserve the same cyclic relationships; after
allowing a global rotation/reflection and a non-uniform final display-aspect
normalization, I do not see a reliable residual reversal, crossing, missing
boundary, or different adjacency in these screenshots.

Measured blue-tube bounds are `x=319..970`, `y=154..805` (about 652 by 652
pixels; aspect 1.00). The current C# viewport uses `displayAspect: 1.0` and uniform
scale; its output map does not apply the MAME screenshot's apparent
non-square axis ratio. That accounts for the most visible shape impression:
the C# lobes are relatively broader on the raster.

### Corresponding features

| Feature | MAME | C# | After normalization |
|---|---|---|---|
| Major lobes | Four broad lobes around the center | Same four-lobe arrangement | Same count and order; raster proportions differ |
| Inward wedges/notches | Four narrow inward features alternating with lobes | Same four features | No demonstrated different adjacency or termination |
| Opposite-side relationships | Repeated opposing vertices and boundaries | Same repeated structure | Both are consistent with record 06's opposite-point pairs |
| Near/deep rim convergence | Rails run from the outer structure toward a small central rim | Same broad convergence pattern | No clear wrong-index rail or topology defect visible |
| Boundary order | 16-point rhythm | 16-point rhythm | Consistent with a cyclic record-06 perimeter |

No screenshot-visible difference was found that cannot be explained by a
global orientation plus final horizontal/vertical aspect scaling and ordinary
scale/placement differences. In particular, the images do not establish that
the four wedges attach to different lobes or that corresponding rails end at
different record vertices.

## Which C# projection mode produced the supplied image?

The image is consistent with the normal factor-256 control and inconsistent
with the factor-128 diagnostic under the current code's fixed viewport.

`AtariDvgViewportMap.DevelopmentOnlyProvisional` places the DVG origin at
logical `(300,400)` and applies scale `0.4`. The prototype's portrait HUD
camera applies scale `1.2`, with the portrait viewport beginning at screen
X=280. Thus, for the current 1280x960 output, the screen mapping is:

```text
screenX = 640 + 0.48 * DVG_X
screenY = 480 - 0.48 * DVG_Y
```

Using the cataloged record-06 points, Level 7 metadata, near position 16, and
the current control quotient, the projected DVG bounds are
`X=-668..688`, `Y=-678..678`. The mapping predicts pixel bounds
`X=319.36..970.24`, `Y=154.56..805.44`, matching the screenshot's measured
blue bounds to raster rounding.

The conditional factor-128 output instead predicts approximately
`X=445.12..770.56`, `Y=317.28..642.72` under the same map. It would be about
half the size and plainly does not match the supplied image. The screenshot
does not carry launch-argument metadata, so this is a code-and-pixel
consistency check rather than a claim about an external capture log; the
bounds nevertheless identify the displayed geometry as the factor-256
control for the current implementation.

The factor-128 substitution applies the same multiplier to both per-axis
magnitudes at both depths. For a given point, changing 256 to 128 therefore
scales its projected displacement about the common source base
`(X=0,Y=-144)` by approximately one half; integer truncation can leave a
one-coordinate-unit residual. It does not change point order, rim adjacency,
rail correspondence, or the normalized tube outline. Thus, even if a separate
factor-128 screenshot were available, that experiment could not by itself
repair a large normalized topology/shape difference.

## Trace from ROM record to rendered lines

### Source points and metadata

`AtariTubeRecordCatalog` contains the 16 raw X/Y bytes recovered for ROM record
06, with `lev_scale=$18`, `lev_y3d=$68`, signed `lev_y2d=$ff70` (-144), and
`lev_open=$00` (closed). The local ROM table extraction report records those
bytes and the Level-7-to-record-06 remap. The direct prototype does not use
the production C# Level 7 clover points.

The record has exact opposite-point pairs: point `i+8` complements point `i`
about raw `(128,128)`. Its repeated four-sector appearance is strong but not
perfect 90-degree symmetry; some corresponding raw components differ by four
units. The C# catalog preserves the ROM row order, and the projector applies
one common per-depth divisor and multiplier to both axes, so it preserves
those repeated relationships apart from integer truncation.

**Disposition — wrong source points:** rejected for the prototype's visible
record. The prototype's source row is ROM record 06, not the unrelated
production clover. No source-point mismatch is supported by this evidence.

### Projection and coordinate interpretation

The local `$c098` routine establishes these relevant operations:

- X magnitude is formed relative to `$80`; Y magnitude is formed relative to
  `lev_y3d`.
- Both axes use the same prepared depth word derived from runtime position and
  `lev_scale`.
- The signed Y2D pair is applied after the Y-axis Math Box result; the X base
  is zero in this setup.
- The source documents the vector serialization order as source-Y-derived
  component to DVG X and source-X-derived component to DVG Y.

The C# projector follows those origin, sign, base, and axis-order choices. It
uses the same divisor for X and Y. At the two selected positions, its
coordinates remain well within the signed-16 and signed-13 boundaries; no
saturation or 13-bit wrap is responsible for a changed Level 7 outline.
Within each rim, this is a uniform scale plus translation of the raw points,
not a per-vertex non-uniform warp. The final DVG-to-pixel map is where the
observed non-square presentation enters.

`AtariDvgViewportMap` explicitly labels its sign, rotation, scale, and aspect
as provisional. `displayAspect=1.0` is an implementation-established
development choice, not an Atari-calibrated value. The source establishes
the DVG serialization axis swap, but does not establish a final physical
monitor orientation or gain. Axis swap/sign/rotation alone is a global
orientation change; it does not explain a different adjacency graph.

**Disposition — wrong coordinate-space interpretation:** the C# final map is
not calibrated to the MAME raster aspect. This explains the measured
square-versus-tall appearance. It does not establish an internal record-06
point or topology error after ordinary aspect/rotation normalization.

### Rim adjacency and near/far correspondence

The Atari closed-outline path at `$c4e1-$c542` loads the record's start point,
draws it, then decrements through the point indices for 16 vector steps. For a
closed Level 7 row this is the same undirected cyclic set of edges as C#'s
`index -> (index+1) mod 16`, just traversed in the opposite direction.
`lev_open=$00` means C#'s 16 edges per rim agree with the closed-record case.

The normal endpoint builder at `$c30d` requests far position `$f0` and near
position `$10`; `$c473` projects the same 16 `tube_x/tube_y` entries on each
pass, preserving the record index while storing the two endpoint sets in
separate indexed regions. The C# builder uses positions 240 and 16 and builds
each rim from the same cyclic record order. Its rails pair `near[index]` and
`far[index]`. The Atari routines then iterate the corresponding index range
when constructing vectors, which strongly supports this pairing.

The complete rail-list interpretation through `$c3ee`/`$c43c` was not
independently reconstructed: those routines read additional indexed vector
arrays (`$035a-$038a`), and this investigation did not trace all their
producers and emitted DVG words. The screenshot itself shows no definite
wrong-index crossing. Accordingly:

- **Rim adjacency:** ROM-backed and consistent; wrong rim order is rejected.
- **Open/closed semantics:** ROM-backed and consistent; Level 7 is closed.
- **Near/far point identity:** strongly supported by the same-index
  construction, not conclusively resolved through the full vector writer.
- **Wrong rail connectivity:** plausible only as the remaining narrow
  structural hypothesis; not demonstrated by the screenshots.

## Factor-128 and prior numeric report disposition

The normal Math Box investigation establishes `RN.low=$0f` but leaves
`RN.high` unresolved at the first normal record-06 divide. The local PROM
trace supports factor 128 conditionally on `RN=$000f`; it does not establish
that condition for normal arcade startup. The existing factor-128 option is
therefore correctly classified as **DEVELOPMENT-ONLY**, not normal Atari
behavior.

The supplied C# screenshot is factor 256, not factor 128. No paired
factor-128 screenshot was supplied, so visual improvement/worsening from the
experiment cannot be claimed. The code calculation does establish that
factor-128 changes overall projected scale with at most small truncation
residuals; it does not repair point order or normalized structural
relationships.

One arithmetic error was found in the prior normal-call-state report's table
(rows 07 and 15). Both raw Y values are `$80` (128), which is greater than
record 06's `lev_y3d=$68` (104), so their Y sign is positive. For the current
factor-256 near projection, each has source-Y `-144+153=9`, not `-144-153`
(-297). Under conditional factor 128, it is `-144+76=-68`, not `-220`.
The current C# implementation uses the positive sign, as does `$c098`; the
report table's two entries do not describe the implementation output. This
does not affect the screenshot-mode identification or the overall factor
conclusion, but those two rows should not be used as a coordinate oracle.
The historical report was left unchanged.

## Cause matrix and evidence classification

| Candidate | Finding | Classification |
|---|---|---|
| Wrong ROM source row/point values | Prototype catalogs record 06 and uses its 16 raw points; Level 7 remaps to row 06. | ROM-BACKED / implementation-established; rejected |
| Wrong cyclic rim connectivity | Atari closed-outline traversal uses the same cyclic neighbors in reverse order; C# uses forward order. | ROM-BACKED / implementation-established; rejected |
| Wrong open/closed behavior | Record 06 is closed; both render 16 edges per rim. | ROM-BACKED; rejected |
| Wrong near/far index pairing | Atari constructs both endpoint sets from same-index point inputs; C# uses same-index rails. Full DVG writer path not fully decoded. | Strongly supported; residual uncertainty |
| Non-uniform per-vertex projection | C# applies same divisor/multiplier per axis at each depth; current coordinates do not saturate/wrap. | Implementation-established; not supported as cause |
| Factor-256 vs conditional factor-128 | The supplied image matches factor 256; changing the common factor is essentially a global scale change. Normal RN.high is unresolved. | Image/code established; Atari arithmetic remains conditional |
| DVG-to-pixel aspect | C# map is isotropic (`displayAspect=1`); the two screenshot tube bounds have different aspect ratios. | Visually and implementation established; best explanation for apparent shape difference |
| Other topology/coordinate mismatch | No normalized residual is established from the pair; no vector-list capture is available to prove every Atari rail endpoint. | UNKNOWN |

Evidence categories:

- **Visually established:** both images show the same four-lobe/four-notch
  structural pattern and central convergence; C# and MAME blue bounds have
  aspect ratios 1.00 and 0.763 respectively.
- **ROM/Atari-established:** record 06's points/metadata, closed status,
  projection origins and base placement, DVG serialization order, endpoint
  depths `$10/$f0`, and reverse cyclic closed-rim traversal are established
  by the local extraction/disassembly evidence.
- **Implementation-established:** prototype source row, quotient mode,
  point/rim ordering, same-index rail construction, output map, and exact
  screenshot-bound consistency with the factor-256 control.
- **Diagnostic hypotheses:** the final map's non-square aspect correction
  likely accounts for the visible proportion difference; the full rail-list
  writer may still be worth tracing to conclusively verify endpoint pairing.
- **Unresolved:** normal `RN.high`; exact normal Math Box quotient if that byte
  is nonzero; complete `$c3ee`/`$c43c` DVG rail endpoint semantics; calibrated
  physical DVG aspect/orientation.

## Correction readiness and narrow next investigation

**There is not enough evidence for a bounded geometry/projection correction
prompt.** The supplied screenshots do not establish a normalized structural
mismatch, and the current shape data and cyclic rim connections agree with the
local Atari evidence. Changing points, topology, or the projection factor now
would be speculative.

The single narrowest next investigation is to trace the producers and
consumers of `$035a-$038a` through `$c3ee`/`$c43c` and reconstruct the actual
record-06 DVG segment endpoint list. Compare that list against the C# 16
same-index rails plus two closed rims, ignoring final orientation/aspect. That
would settle the only remaining specific connectivity hypothesis without
reopening unrelated gameplay, geometry, or viewport work.

## Preservation, validation, and completion

- Starting repository state: branch `master`, HEAD
  `385f603a94391756b2ca6cccd524dc17b281e782`, upstream `origin/master`,
  empty index, 17 pre-existing modified tracked paths, and 12 relevant
  non-visual untracked paths. All pre-existing work was preserved.
- No project source, tests, project configuration, durable documentation,
  geometry, projection, viewport, presentation, gameplay, recording/playback,
  protected reference material, or Git index/branch/commit state was changed.
- The required ignored prompt and this report are the only files created.
  Nothing was staged or committed.
- **Validation:** read-only screenshot bounds, current renderer mapping,
  record-06 projections, and source/disassembly correspondence were checked.
  Build/tests were not applicable. The interactive game was not launched.
- No manual acceptance is claimed or requested for this read-only
  investigation.
- Prompt:
  `C:\Users\nrnob\source\repos\Tempest\AI-Logs\Prompts\2026-11-16-level7-mame-vs-csharp-shape-diagnostic-investigation-prompt.md`
- Report:
  `C:\Users\nrnob\source\repos\Tempest\AI-Logs\Reports\2026-11-16-level7-mame-vs-csharp-shape-diagnostic-investigation-report.md`
