# Level 4 Candidate Scale/Frame Diagnostic

Date: 2026-11-15
Status: READ-ONLY DIAGNOSTIC COMPLETE

## Executive conclusion

**Primary classification: AFFINE FRAME APPLIED IN WRONG UNIT DOMAIN.**

The first material divergence is after the Level 4 depth scale is computed,
when the adapter converts an already-C# model displacement to the Atari
comparison frame and feeds that Atari-unit value directly to the C# logical
screen projection. The fitted references cancel; the actual position gains
are `1/aX` horizontally and `1/aY` vertically. Nothing converts those
displacements back to the C# model units expected by the legacy projection.
The portrait camera then uniformly multiplies the reduced extents by 1.26; it
does not restore them.

The adapter arithmetic implements the preceding candidate prompt, but the
prompt promoted a correspondence scale into a literal presentation scale
without evidence that one Atari comparison unit should occupy one C# logical
model unit. This is not a float/formula implementation defect relative to
that prompt. The decisive problem is the unit-domain choice. The user-reported
manual failure is consistent with the large, numerically established
contraction; this report makes no visual-fidelity judgment.

No Level 4 presentation characteristic is accepted from this candidate.

## Implementation trace

Relevant current source:

- [AtariPresentationAdapter.cs](../../src/Tempest.Game/Rendering/AtariPresentationAdapter.cs#L96)
- [VectorRenderer.cs](../../src/Tempest.Game/Rendering/VectorRenderer.cs#L216)
- [Projection.cs](../../src/Tempest.Game/Rendering/Projection.cs#L40)
- [LevelLoader.cs](../../src/Tempest.Game/Levels/LevelLoader.cs#L70)
- [LevelConfiguration.cs](../../src/Tempest.Game/Levels/LevelConfiguration.cs#L220)
- [Level.cs](../../src/Tempest.Game/Levels/Level.cs#L18)
- [DisplayConfiguration.cs](../../src/Tempest.Game/Rendering/DisplayConfiguration.cs#L24)

`VectorRenderer.ProjectWorldPoint` selects this branch only for the opt-in
candidate. It passes the existing C# model `x,y`, point depth, `CenterY`,
`YOffset`, configuration depths, and `Projection.K/ZOffset` into the adapter.
The Level 4 definition remains the listed 16 integer points, closed, with
`CenterY=380`, `YOffset=-65`; its configuration uses `Position=50`,
`Depth=400`. The evaluated near/mid/far depths are therefore `50, 250, 450`.

The descriptor stores `aX=2.081778520`, `aY=2.231150659`, references
`128` and `121.501111`, and positive orientations. With no approach offsets,
the adapter computes:

```text
depthScale = K / (depth + ZOffset)
rawX = 128 + x / aX
rawY = 121.501111 + y / aY
modelFrameX = (rawX - 128) * 1 = x / aX
modelFrameY = (rawY - 121.501111) * 1 = y / aY
screenX = 400 + modelFrameX * depthScale
screenY = 380 + (modelFrameY - 65) * depthScale
localScaleX = depthScale / aX
localScaleY = depthScale / aY
```

Both references cancel exactly; neither is a screen translation. `CenterY`
remains the logical-screen center/baseline and `YOffset` remains an
untransformed placement offset. The independent model-point displacement is
the only part divided by its axis scale. The local Jacobians agree with the
derivatives of this contracted presentation.

Legacy `Projection.Project` uses
`depthScale=(depth+ZOffset)/K`, then
`screenX=400+x/depthScale` and
`screenY=CenterY+(y+YOffset)/depthScale`. This is algebraically equivalent
to multiplying both C# model coordinates by `K/(depth+ZOffset)`. The first
divergence is therefore the inverse-frame model-coordinate conversion, not
depth selection or depth progression. At that point the values change from
C# model units to Atari comparison units; they are then treated as C# logical
projection units.

The portrait playfield camera is applied after logical projection:

```text
zoom = 1.2 * 1.05 = 1.26
hostX = 640 + 1.26 * (logicalX - 400)
targetY = (480 - 1.2 * -33.1) / 1.26 = 412.476190
hostY = 480 + 1.26 * (logicalY - targetY)
      = 1.26 * logicalY - 39.72
```

This same transform applies to legacy and candidate. Neither portrait zoom nor
the `-33.1` portrait translation compensates for the missing unit restoration.

## Legacy reconstruction

Calculation conditions: production `K=65`, `ZOffset=5`, no approach/exit or
zoom animation, and all 16 current Level 4 production vertices projected at
each listed depth. Bounds are vertex bounds; center is the bounding-box
midpoint. Logical margins are relative to 800x600. Host margins are relative
to the portrait playfield viewport `(x=280..1000, y=0..960)`.

| Depth | Logical X bounds | Logical Y bounds | Logical W x H | Logical center | Logical margins L/R/T/B |
|---|---:|---:|---:|---:|---:|
| Near 50 | 141.182..658.818 | 166.091..474.545 | 517.636 x 308.455 | 400.000, 320.318 | 141.182 / 141.182 / 166.091 / 125.455 |
| Mid 250 | 344.176..455.824 | 333.863..400.392 | 111.647 x 66.529 | 400.000, 367.127 | 344.176 / 344.176 / 333.863 / 199.608 |
| Far 450 | 368.714..431.286 | 354.143..391.429 | 62.571 x 37.286 | 400.000, 372.786 | 368.714 / 368.714 / 354.143 / 208.571 |

| Depth | Portrait-host X bounds | Portrait-host Y bounds | Host W x H | Host center | Host margins L/R/T/B |
|---|---:|---:|---:|---:|---:|
| Near 50 | 313.889..966.111 | 169.555..558.207 | 652.222 x 388.653 | 640.000, 363.881 | 33.889 / 33.889 / 169.555 / 401.793 |
| Mid 250 | 569.662..710.338 | 380.947..464.774 | 140.675 x 83.827 | 640.000, 422.861 | 289.662 / 289.662 / 380.947 / 495.226 |
| Far 450 | 600.580..679.420 | 406.500..453.480 | 78.840 x 46.980 | 640.000, 429.990 | 320.580 / 320.580 / 406.500 / 506.520 |

Legacy local X and Y derivatives are equal:
`K/(depth+ZOffset)` = `1.181818`, `0.254902`, `0.142857` at near, mid,
and far. Near/far width and height ratios are both `8.272727`. The projected
origin/baseline center `(x=0,y=0)` moves from logical Y `303.182` near to
`370.714` far, a `67.532` logical-unit progression (`85.091` host units).
The bounding-box center instead progresses `52.468` logical units because
the point bounds are asymmetric around `y=0`.

## Candidate reconstruction

The same points, depths, bounds, margins, and camera are used; only the
implemented inverse-frame factors change the projected model displacement.

| Depth | Logical X bounds | Logical Y bounds | Logical W x H | Logical center | Logical margins L/R/T/B |
|---|---:|---:|---:|---:|---:|
| Near 50 | 275.674..524.326 | 241.738..379.987 | 248.651 x 138.249 | 400.000, 310.862 | 275.674 / 275.674 / 241.738 / 220.013 |
| Mid 250 | 373.185..426.815 | 350.179..379.997 | 53.631 x 29.818 | 400.000, 365.088 | 373.185 / 373.185 / 350.179 / 220.003 |
| Far 450 | 384.972..415.028 | 363.287..379.998 | 30.057 x 16.711 | 400.000, 371.643 | 384.972 / 384.972 / 363.287 / 220.002 |

| Depth | Portrait-host X bounds | Portrait-host Y bounds | Host W x H | Host center | Host margins L/R/T/B |
|---|---:|---:|---:|---:|---:|
| Near 50 | 483.350..796.650 | 264.870..439.063 | 313.300 x 174.194 | 640.000, 351.967 | 203.350 / 203.350 / 264.870 / 520.937 |
| Mid 250 | 606.213..673.787 | 401.505..439.076 | 67.575 x 37.571 | 640.000, 420.291 | 326.213 / 326.213 / 401.505 / 520.924 |
| Far 450 | 621.064..658.936 | 418.022..439.078 | 37.871 x 21.056 | 640.000, 428.550 | 341.064 / 341.064 / 418.022 / 520.922 |

| Depth | Candidate/legacy width | Candidate/legacy height | Candidate/legacy local X derivative | Candidate/legacy local Y derivative | BBox-center Y C/L (logical; host) |
|---|---:|---:|---:|---:|---:|
| Near | 0.480358 | 0.448199 | 0.480358 | 0.448199 | 0.970480; 0.967257 |
| Mid | 0.480358 | 0.448199 | 0.480358 | 0.448199 | 0.994445; 0.993923 |
| Far | 0.480358 | 0.448199 | 0.480358 | 0.448199 | 0.996934; 0.996651 |

Candidate bounding-box center Y differs from legacy by `-9.456`, `-2.039`,
and `-1.143` logical units near/mid/far (`-11.914`, `-2.570`, `-1.440`
host units). X center remains exactly `400` logical / `640` host. The
projected-origin center is unchanged at all three depths because `YOffset`
was deliberately left outside the model-frame conversion.

Candidate bounding-box center progresses `60.780` logical units near-to-far
(`76.583` host), versus legacy `52.468` (`66.109` host). This difference
comes from scaling the asymmetric point Y coordinates while leaving the
placement offset unchanged; it does not come from a changed camera or
`CenterY`. The origin-baseline progression remains `67.532` logical
(`85.091` host) in both.

Near/far size ratios remain `8.272727` for both candidate axes, so the
candidate/legacy ratio of each near/far size ratio is `1.000000`. The depth
progression itself was not changed. Candidate local X/Y derivatives are
`0.567696 / 0.529690` near, `0.122444 / 0.114247` mid, and
`0.068623 / 0.064028` far.

## Unit-domain analysis

The controlling fit is stated as `C# = a * Atari + translation`; hence
`aX/aY` are C# units per Atari unit. Its inverse
`Atari = reference + C#/a` is valid for interpreting an existing C# point in
the Atari comparison frame. The adapter then projects the resulting Atari
displacement as if it were still a C# model displacement. There is no
compensating multiplication by `aX/aY` before C# logical-screen projection.

Resulting screen-space gains relative to the legacy C# projection are:

- X: `1/aX = 0.4803584965` (51.964% narrower).
- Y: `1/aY = 0.4481992267` (55.180% shorter).
- Portrait-host camera: common additional factor `1.26` on both; no unit
  restoration.
- Depth: same `K/(depth+ZOffset)` at each depth; same near/far size ratio.
- References: X `128` and Y `121.501111` cancel exactly.
- Placement: `CenterY=380` and `YOffset=-65` remain in the baseline; no
  screen translation is introduced by either reference.

The first divergence is therefore a direct consequence of using a
correspondence transform as a presentation gain, not an arithmetic defect in
the conversion. The previous candidate prompt requested this inverse-frame
application to rendered points, but the fit and handoff did not establish
that Atari comparison units should be treated as C# logical presentation
units. That promotion was insufficiently justified.

## Correspondence versus presentation

**Coordinate correspondence** answers how the existing C# points relate to
Atari record-03 coordinates. The fitted frame and residuals establish an
approximate relation; its references are model-frame origins.

**Presentation transformation** changes final C# screen coordinates. No
separate evidence establishes `1/aX` and `1/aY` as desired C# render gains,
and no target dimensions or calibrated final scale were established. Thus the
correspondence is valid to compare coordinates, but it was not validly
established as a literal screen-space scaling operation. The candidate
instruction conflated those purposes.

## Protected state

This diagnosis does not implicate or authorize changing:

- Level 4 stored points, their order, lanes, topology, or gameplay geometry;
- the existing C# depth progression, `CenterY=380`, or `YOffset=-65`;
- portrait placement `-33.1`, portrait zoom `1.26`, or camera placement;
- gameplay, timing, recording, or playback;
- accepted Levels 1-3 presentation;
- the shared candidate-specific Blaster-exit rendering defect.

No placement correction follows from an overall size discrepancy. No
perspective correction is supported by these calculations.

## Correction derivation

The uniquely supported correction form for restoring the established C#
presentation scale is to keep the affine correspondence out of final render
gains. If the conversion is retained in the render pipeline, restore units
before projection:

```text
Atari displacement = C# displacement / a
C# displacement restored = Atari displacement * a = original C# displacement
screenX = 400 + x * K/(depth + ZOffset)
screenY = 380 + (y - 65) * K/(depth + ZOffset)
localScaleX = localScaleY = K/(depth + ZOffset)
```

The references still cancel; no post-screen reference translation is needed.
This round trip is identity, so no extra normalization is required to retain
legacy extents. It also means the correspondence scales alone cannot alter
shape while preserving both existing width and height: per-axis normalization
to both original bounds cancels the scale entirely. Any deliberate aspect or
shape change would itself alter at least one relative extent and needs a
separately established target; no aesthetic constant or visual target can be
derived here. Placement remains unchanged.

## Decision-relevant unknowns

| Unknown | Could change primary classification? | Could change correction architecture? | Require geometry/gameplay change? | Require placement/perspective change? | Materially change next candidate? |
|---|---|---|---|---|---|
| No measured/manual target extent for a desired non-legacy Level 4 shape. | No; the unit mismatch is explicit. | Yes, only if a non-legacy shape is required; that would need a new calibrated presentation transform. | No evidence supports one. | No evidence supports one. | Yes for a deliberately reshaped candidate; not for restoring C# units. |
| Whether a future presentation should intentionally differ from the legacy C# size. | No; it does not change the unit trace. | Potentially, but only with an authorized target/measurement. | No. | No. | Yes if such a change is required; otherwise no. |
| Exact arcade-pixel equivalence of the fit. | No; the current mismatch occurs before arcade equivalence is relevant. | No for unit restoration; yes only for a later exact arcade-fit effort. | No. | No. | No for the smallest next candidate. |

No further investigation is recommended for the last item in the absence of
an exact-equivalence requirement. No geometry, placement, or perspective
uncertainty blocks the unit-restoring next candidate.

## Smallest next milestone

Create one bounded, opt-in Level 4 candidate that retains the existing C#
logical-unit projection (equivalently, applies the correspondence conversion
and immediately restores the units), test that its near/mid/far projections
match the legacy Level 4 numerical baseline, then stop for Neal's manual
acceptance. Do not tune a different shape or placement without a separately
measured target.

## Scope and completion

- Prompt: [2026-11-15-level4-candidate-scale-frame-diagnostic-prompt.md](../Prompts/2026-11-15-level4-candidate-scale-frame-diagnostic-prompt.md).
- Report: this file.
- Files created: exactly the authorized prompt and this report under ignored
  `AI-Logs/`; no other files.
- Handoffs inspected: the named geometry-provenance/presentation-mapping
  report and Atari-presentation candidate report, plus the preceding
  candidate implementation prompt to verify what was promoted.
- Current source inspected: Level 4 definition/configuration and depth
  constants, adapter, renderer, legacy projection, portrait display
  configuration, and focused adapter tests.
- Calculations: inline, bounded reconstruction of all 16 Level 4 points at
  near/mid/far depths for both legacy and candidate, including logical/host
  bounds, extents, centers, margins, local derivatives, and ratios.
- Visual/reference restrictions: no visual asset search/opening or image,
  video, recording, or frame analysis; no game launch, Internet search, or ROM
  inspection. The untracked `screenshot000.png` appeared in the required
  initial Git inventory only and was not opened or followed up.
- Validation: no build or test suite was required or run. Numerical
  calculations completed; no production or test source was changed.
- Starting Git state: branch `master`, HEAD
  `385f603a94391756b2ca6cccd524dc17b281e782`, upstream `origin/master`,
  index empty, 16 tracked modified paths and 8 untracked paths. All
  pre-existing changes were preserved.
- Ending Git state: verified after creating the two ignored artifacts; same
  branch, HEAD, upstream, tracked worktree, untracked inventory, and index
  state. No tracked file changed; nothing was staged, committed, or pushed.
- Manual acceptance: the user's reported first candidate test did not pass;
  no Level 4 presentation characteristic is accepted from it. The next
  candidate remains subject to Neal's manual acceptance.
- Primary classification: **AFFINE FRAME APPLIED IN WRONG UNIT DOMAIN**.
