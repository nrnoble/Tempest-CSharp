# Level 4 Geometry Provenance and Presentation-Mapping Investigation

Date: 2026-11-15
Status: READ-ONLY INVESTIGATION COMPLETE

## Executive conclusion

**Disposition: READY WITH ONE EXPLICIT ADAPTER EXTENSION.**

The current 16-point Level 4 binocular definition is an unchanged, ordered
copy of the Incitti `Original Arcade` Level 4 reference row. Project records
call it source-derived/reference-derived; neither the loader nor its
implementation history documents an Atari record-03 conversion. The numerical
fit is useful for an explicitly approximate presentation candidate, not proof
of exact arcade equivalence.

A bounded, render-only Level 4 frame can be specified without changing stored
points or lane identity. The best independent-axis fit uses a positive-axis,
forward-order representative with C#-units-per-Atari-unit scales
`aX=2.081778520`, `aY=2.231150659`, X reference `128`, and Y reference
`121.501111`. It has non-rounding residuals (RMS `6.395377` C# units). The
current adapter does not provide a Level 4 mapping, independent model-frame
scales, or this Y reference. The smallest required extension is a per-level,
render-only affine model-frame mapping with separate X/Y scales and an
explicit Y reference; retain the existing C# depth progression and do not
reorder or otherwise change model lanes.

The reference configuration's `CenterY=380` and `YOffset=-65` are not proof
that a specific screen placement was manually accepted. Neal's current
observation that Level 4 is properly placed and shorter than Levels 1-3 is a
constraint for the first candidate: preserve its current placement and
shorter-profile distinction; do not transfer Levels 1-3 placement or aspect
values. No numeric screen-space placement or height acceptance was found.
Neal must manually accept any future candidate; this investigation did not
launch the game or inspect visual material.

## Evidence and authority

Evidence actually used:

- Durable project records: [TEMPEST-BEHAVIOR-SPEC.md](../../TEMPEST-BEHAVIOR-SPEC.md)
  and [TEMPEST-DEVELOPMENT-NOTES.md](../../TEMPEST-DEVELOPMENT-NOTES.md), notably
  their Level 4 source/progression, gameplay acceptance, and diagnostic
  descriptions.
- Current working-tree source:
  [LevelLoader.cs](../../src/Tempest.Game/Levels/LevelLoader.cs#L70),
  [LevelConfiguration.cs](../../src/Tempest.Game/Levels/LevelConfiguration.cs#L159),
  [LevelDefinition.cs](../../src/Tempest.Game/Levels/LevelDefinition.cs#L4),
  [Level.cs](../../src/Tempest.Game/Levels/Level.cs#L439),
  [Player.cs](../../src/Tempest.Game/Gameplay/Player.cs#L65),
  [Flipper.cs](../../src/Tempest.Game/Gameplay/Flipper.cs#L450),
  [Tanker.cs](../../src/Tempest.Game/Gameplay/Tanker.cs#L245),
  [Spike.cs](../../src/Tempest.Game/Gameplay/Spike.cs#L1),
  [AtariPresentationAdapter.cs](../../src/Tempest.Game/Rendering/AtariPresentationAdapter.cs#L1),
  [VectorRenderer.cs](../../src/Tempest.Game/Rendering/VectorRenderer.cs#L212),
  and [Projection.cs](../../src/Tempest.Game/Rendering/Projection.cs#L1).
- The named geometry source row, [levelset1.txt](../../Reference/incitti/levelsets/levelset1.txt#L8),
  because the Level 4 implementation prompt expressly names
  `Reference/incitti` as the point source.
- Level 4 records:
  [D1 implementation prompt](../Prompts/2026-11-01-level4-d1-production-candidate-implementation-prompt.md#L120),
  [D1 implementation report](2026-11-01-level4-d1-production-candidate-implementation-report.md#L126),
  [D2 implementation report](2026-11-01-level4-d2-natural-progression-implementation-report.md#L1),
  and [post-Level-3 roadmap report](2026-11-01-post-level3-progression-roadmap-analysis-report.md#L45).
- The controlling
  [Levels 2-6 coordinate-mapping report](2026-11-15-levels2-6-atari-csharp-coordinate-mapping-report.md),
  including its recovered Atari record-03 row and prior fit results.
- Git history for `LevelLoader.cs`, including commit
  `a906574aaa5fe5d9b0e12a937f373b270389d762` (“Implement accepted Level 4
  production candidate”), and its Level 4 point-data diff.

The current starting worktree was dirty. The source and adapter described here
are the current working-tree versions, not an assertion that all local
presentation work is committed. No visual asset was opened or analyzed; no
Internet, raw ROM copy, `TEMP/myNotes.md`, or game process was used.

## Geometry provenance

The earliest retained project analysis located for this board is the
2026-11-01 post-Level-3 roadmap record, which lists the ordered points and
`380/-65` center/offset and says to use the exact source points. The Level 4
D1 prompt explicitly says to add the “source-derived” ordered point set from
`Reference/incitti` and to preserve its exact order. The Nov. 2 implementation
commit adds `LoadLevel4Binoculars` and the Level 4 configuration. The current
literal list matches the named `Original Arcade` Level 4 row exactly, in the
same sequence:

```text
(-33,101), (-101,145), (-185,116), (-219,58), (-219,-29), (-185,-87),
(-101,-116), (-33,-72), (33,-72), (101,-116), (185,-87), (219,-29),
(219,58), (185,116), (101,145), (33,101)
```

The loader uses these integer points directly, with `Continuous=true`,
`CenterY=380`, and `YOffset=-65`. It applies no Atari conversion formula,
scale, coordinate reflection, index rotation, or lane reordering. The source
row is closed and the current C# definition exposes 16 playable lanes.
Historical records establish the Incitti reference as the source and exact
source ordering as intentional. They do **not** document the intended C#-to-
Atari coordinate transform, an Atari-record-03 conversion, or a separately
chosen screen-space height profile.

The point array was committed as the Level 4 production candidate and is not
marked provisional in the implementation record. That does not make its
rendered profile final: the deliberate intent or manual acceptance of the
shorter screen-space height remains undocumented, and it is not established as
either provisional or finally accepted.

**Direct answers:** Exact Atari record-03 equivalence was never found claimed.
The documented intent is an exact copy of a reference-derived binocular board
for the C# candidate, not a documented approximate conversion of Atari record
03. Calling the points `REFERENCE-DERIVED` is supported; promoting them to
ROM-backed or arcade-verified geometry is not.

## Acceptance record

| Category | Evidence and scope |
|---|---|
| Topology | Reference configuration and tests use a closed 16-point/16-lane board. No specific manual topology acceptance was found. |
| Shape / ordered points | Source-derived literals and point-order tests are documented. Neal's overall Level 4 feel acceptance does not establish exact tube-shape, point-order, or Atari-equivalence acceptance. |
| Lane identity | Current production keeps its ordered array and index-based lanes. No separate manual acceptance of Atari lane correspondence was found. |
| Placement / horizontal position / width | The reference values are `CenterY=380`, `YOffset=-65`. Neal's current observation says the Level 4 view is properly placed; no historical numeric screen-space acceptance or horizontal-placement/width acceptance was found. Preserve the current placement in the first candidate as a present user constraint, not as a recovered numeric project rule. |
| Height | Neal's current observation says Level 4 is shorter than Levels 1-3. No exact screen measurement, explicit historical height acceptance, or record that the shorter *screen-space* profile was itself a deliberate final presentation decision was found. Its different model geometry is deliberate; the rendered-height intent remains undocumented. |
| Perspective / depth | No Level 4-specific manual perspective calibration was found. Keep established C# depth progression; do not infer acceptance of Atari reciprocal-depth arithmetic. |
| Gameplay | Level 4 visible behavior, late-wave Spiker behavior, Spike-exit behavior, corrected firing boundary, and natural Level 1-to-4 campaign were manually accepted in the historical records. This is visible gameplay acceptance, not exact presentation or general arcade-fidelity acceptance. |
| Transitions / movement / enemy and hazard behavior | The D1 report records tests for all 32 directed transitions, player projection/highlighting, rendering finiteness, and pivot geometry. Neal's overall gameplay acceptance does not turn these tests into manual coordinate acceptance. |
| Warning presentation | The uppercase Avoid Spikes warning has separate explicit manual acceptance. This does not accept the binocular tube's shape, screen dimensions, or camera frame. |

Thus, prior documentation does not establish that the current Level 4 screen
placement was already project-accepted. The current user observation is the
controlling preservation requirement for a first candidate.

## Gameplay dependencies

The current definition's point array and lane order are part of gameplay
structure, not merely drawing data:

- `LevelDefinition` derives 16 playable lanes from the closed 16-point list.
  `Level.TryGetAdjacentLane` selects neighbors by ordered lane index and wraps
  the closed ring. Reordering points would change which lanes are adjacent
  and which point/edge each lane index names.
- Directed transitions use indexed neighboring points, polygon winding,
  vertex angles, and point coordinates to choose hinges and calculate sweeps.
  Player rim movement consumes transition angles. Flipper pivots use those
  transitions and coordinates; changing stored points can change pivot
  geometry and movement angles.
- Flipper and Tanker routing use lane-index adjacency. Tanker cargo selects
  neighboring lanes from the Tanker's indexed edge. Spawn, direction, and
  route state are also lane-indexed.
- Spikes, shots, enemies, and player collision/rim tests compare lane/edge
  indices (for example `Spike.IsHitBy` and `Spike.HitsPlayer`); they do not
  spatially re-intersect a transformed tube polygon. Reordering would therefore
  change lane identity and collision relationships.
- Death/re-entry reuses the Level 4 configuration and its fixed Spikes while
  rebuilding level state; it does not establish a separate point mapping.
  Changing the stored definition would therefore affect the restored gameplay
  geometry as well as a fresh Level 4.
- Rendering and interpolation read the same ordered points: rails/rims,
  lane highlights, player/shot/enemy positions, and Flipper pivot endpoints
  are projected from indexed vertices or edges. A render-only mapping must
  apply consistently to those coordinates and tube-oriented glyph scales.

Consequently, do not rotate/reverse the production array or reflect, replace,
or rescale stored Level 4 geometry. Reordering directly changes index identity
and adjacency; reflection can change winding/spatial direction; replacement
changes transition angles; and non-uniform stored scaling changes geometry-
derived angles. Keep comparisons and any frame transform outside the gameplay
model. Uniform stored scaling may preserve some angles, but would still change
the shared geometry and presentation and is unnecessary here.

## Coordinate comparison

The controlling report verifies displayed Level 4 uses progression index 3
and `lev_remap[3] = 03`. Atari record `03` and C# Level 4 both have 16 points
and closed topology (`lev_open=00` / `Continuous=true`); there is no endpoint
or closing-edge mismatch.

I reproduced the bounded least-squares comparisons from the ordered C# points
and recovered record-03 byte pairs. The relation tested was
`C# = (aX * signX * AtariX + tx, aY * signY * AtariY + ty)`, with cyclic
correspondence and optional reversal. Fit references are
`-tx/(aX*signX)` and `-ty/(aY*signY)`. RMS is the Euclidean point-residual
RMS across all 16 pairs.

| Fit | Correspondence representative | Scales | References | Max absolute residual X / Y | RMS |
|---|---|---|---|---:|---:|
| Best uniform | `j=(3-i) mod 16`, reversed, X sign `-1`, Y sign `+1` | `a=2.119815270` | `128 / 121.159781` | `9.940049 / 11.790345` | `8.456060` |
| Best independent axes | `j=(3-i) mod 16`, reversed, X sign `-1`, Y sign `+1` | `aX=2.081778520`, `aY=2.231150659` | `128 / 121.501111` | `8.048826 / 5.595232` | `6.395377` |
| Current-adapter constraint | `j=(i+4) mod 16`, forward, X/Y signs `+1/+1`, references fixed at `128/128` | Uniform `2.119815270` | `128 / 128` | `9.940049 / 26.290345` | `16.785558` |

The prior report's reversed/X-reflected best-fit representation is verified.
There is also a symmetry-equivalent forward representation
`j=(i+4) mod 16` with positive X/Y signs and identical scales, references,
and residuals. Thus reversal and X reflection are choices of correspondence
representation, not required presentation operations. Neither form authorizes
reordering production lanes. The fit has meaningful multi-unit residuals and
is an approximation, not exact conversion.

The minimum positive-axis frame from the independent fit can be written
`AtariX ~= 128 + C#X/aX` and
`AtariY ~= 121.501111 + C#Y/aY`. X requires no additional fitted translation;
the Y reference differs from 128. That fitted reference is a point-frame
parameter, not Atari `lev_y3d` and not signed `lev_y2d`.

## Required presentation-frame features

For a bounded approximation that retains the current stored model and lane
identity, the minimum is:

1. A Level 4-only render-time frame with separate X and Y model-unit scales
   corresponding to the independent fit.
2. An explicit Level 4 Y coordinate reference (`121.501111` in the
   Atari-to-C# fit; use the inverse relation consistently in an implementation).
   X reference remains 128; no fitted horizontal translation is needed.
3. Keep the existing C# depth progression. Keep presentation placement
   separate from the model-frame conversion; retain the current screen center
   and baseline rather than applying a reference translation as a post-screen
   placement adjustment.
4. Apply the same render-time transformation to every Level 4 tube point and
   dependent projected coordinate, and use matching per-axis local scales for
   tube-oriented vector glyphs. Do not change collision or lane-index code.

The following are **analysis-only**: `j` rotation/reversal, any production
point reorder, and the optional reflected fit representation. A positive-
axis, forward correspondence exists, so an X reflection is not required.
Translations in the fitted model equation are not instructions to translate
the finished tube on screen. No nonlinear warp or per-point correction is
supported by this comparison.

The current working-tree `AtariPresentationAdapter` maps only Levels 1, 2, 3,
7, 8, and 9; Level 4 falls back to the unchanged legacy projection. Its model
frame is one uniform scale plus Y orientation. It has Level 2-specific
vertical aspect handling and a `LocalScaleY` result used by some
tube-oriented glyphs, but no Level 4 mapping, separate Level 4 X/Y frame
scales, or fitted Y reference. It therefore cannot express the minimum
Level 4 frame as-is. The smallest adapter extension is one explicit,
per-level affine model-frame descriptor (independent axes and reference),
without model/lane mutation. The existing per-axis local-glyph pathway is
useful prior art but does not itself implement this Level 4 mapping.

## Placement and height preservation

`CenterY=380` and `YOffset=-65` are source-derived configuration values, not
numeric proof of accepted screen placement. Neal's current observation that
Level 4 is properly placed is enough to prohibit copying Levels 1-3 placement
corrections into its first candidate. Keep the current Level 4 placement
baseline and projected center; do not apply Level 1 `+122`, Level 2 `+114`,
Level 3's derived placement, or Level 2's `1.314` vertical aspect.

The current point set and its shorter profile must remain distinct from
Levels 1-3. Do not envelope-center it or force equal top/bottom margins or
equal height. The independent Y scale changes vertical geometry in the
candidate, so exact pixel-height preservation cannot be inferred from the
fit. Preserve the current placement as a baseline, avoid height-matching
adjustments, and leave the final shorter-profile judgment to Neal's manual
acceptance. This is not a decision blocker because the requested constraint
is qualitative and no exact pixel target was supplied.

## Atari parameter applicability

Recovered record-03 values (`lev_scale=0x0F`, `lev_y3d=0x68`,
signed `lev_y2d=-128`, `lev_open=0x00`) are comparison evidence only:

- `lev_open=0x00` safely agrees with the C# closed topology.
- Raw record coordinates support the bounded fit above, but do not by
  themselves define a historical conversion performed by the C# loader.
- `lev_scale` must not be inserted as literal C# reciprocal-depth arithmetic.
  The coordinate fit does not validate the Atari camera equation in the
  project's current C# projection.
- `lev_y3d` is Atari camera-height metadata, not the fitted C#/Atari Y point
  reference (`121.5`) and not a direct C# placement value.
- Signed `lev_y2d=-128` is Atari post-projection data, not a direct
  `CenterY`, `YOffset`, or screen translation.

Preserve established C# depth progression and use no ROM parameter literally
without a separately validated translation. In particular, do not knowingly
introduce clipping or reverse the current near/far center progression.

## Disposition

### READY WITH ONE EXPLICIT ADAPTER EXTENSION

The geometry provenance is established as reference-derived, the structural
comparison and approximate affine frame are reproducible, and a render-only
mapping can preserve the stored geometry, lane indices, gameplay dependencies,
and current placement baseline. The current adapter lacks the required
independent-axis frame and Y reference, so Level 4 is not ready for the
current adapter unchanged. Meaningful residuals require the candidate to be
explicitly approximate and manually validated; exact Atari pixel equivalence
is not a prerequisite for defining that candidate.

## Decision-relevant unknowns

| Unknown | Could change disposition? | Adapter features? | Model/lane changes? | Placement/height? | Validation? |
|---|---|---|---|---|---|
| No record found of manual acceptance specifically covering the Level 4 rendered tube shape, lane-origin correspondence, or exact shorter screen height; the records accept gameplay/overall feel and separate warning presentation. | Potentially, if a future primary acceptance record says this presentation frame must not change; current observation instead supplies a preserve-placement/short-profile constraint, so not a blocker now. | No change to measured minimum frame; it may constrain how it is accepted. | No, unless new evidence explicitly authorizes a model change; none is proposed. | Could clarify exact preservation expectations, but no numeric screen target exists. | Yes; Neal's manual acceptance is required for the first candidate. |
| Exact arcade-pixel equivalence and exact Atari lane-origin identity are not established; the fit retains multi-unit residuals and admits equivalent cyclic/reflection representations. | Only if exact arcade equivalence is made a requirement; the task explicitly does not make its absence a blocker. | Could matter for a later exact lane-correspondence effort, not for this bounded approximation. | No production reorder is authorized or needed for this candidate. | No numeric placement/height change follows. | The candidate must be reviewed manually as an approximation. |

No further investigation is recommended before the bounded candidate. Do not
request or inspect the comparison images.

## Smallest next milestone

Implement and focused-test one **Level 4-only render-time affine model-frame
mapping** in the adapter: independent X/Y scales and an explicit Y reference,
with positive-axis forward correspondence, unchanged stored points/lane order,
unchanged C# depth progression, and the current Level 4 placement baseline.
Keep the candidate opt-in. Test transform repeatability, non-mutation,
per-lane geometry/render consistency, and unchanged Levels 1-3/fallback
behavior. Then stop for Neal's manual presentation acceptance of Level 4
placement, shorter profile, and overall arcade likeness. Do not update durable
acceptance documentation until that manual acceptance occurs.

## Scope and completion

- Prompt: [2026-11-15-level4-geometry-provenance-presentation-mapping-prompt.md](../Prompts/2026-11-15-level4-geometry-provenance-presentation-mapping-prompt.md).
- Report: this file.
- Files created: exactly the authorized prompt and report under ignored
  `AI-Logs/`; neither is staged or committed.
- Durable/history/source records inspected: behavior spec; development notes;
  Level 4 D1/D2 and post-Level-3 roadmap records; controlling coordinate
  report; current LevelLoader, LevelConfiguration, LevelDefinition, Level,
  Player, Flipper, Tanker, Spike, adapter, renderer, and projection; named
  Incitti Level 4 source row; LevelLoader Git history and the Level 4
  implementation commit.
- Calculation: bounded inline comparison of 16 C# points against 16
  recovered Atari record-03 points, including cyclic/reverse correspondences,
  axis signs, uniform/independent scales, references, residuals, and current
  adapter constraints.
- Visual material: no images, screenshots, recordings, or videos were
  intentionally searched for, opened, or analyzed, and the game was not
  launched. One untracked visual filename surfaced incidentally in the
  mandatory starting Git status inventory; it was not followed up or opened.
- Validation: no build or test suite was required or run. No source changes
  were made.
- Starting Git state: branch `master`, HEAD
  `385f603a94391756b2ca6cccd524dc17b281e782`, upstream `origin/master`;
  16 tracked files modified and 8 untracked paths; index empty. Several local
  presentation files relevant to current adapter work were already modified
  or untracked and were preserved.
- Ending Git state: same branch, HEAD, upstream, tracked worktree/index state,
  and pre-existing untracked state; only the two authorized ignored AI-Logs
  artifacts were created. No tracked file changed; index remained unchanged;
  nothing was staged, committed, or pushed.
- Selected disposition: **READY WITH ONE EXPLICIT ADAPTER EXTENSION**.
- Most important decision-relevant unknown: whether Neal previously accepted
  the precise rendered Level 4 tube frame/shorter height, beyond documented
  overall gameplay feel. Current placement and qualitative height remain
  explicit constraints for manual review of any candidate.
