# Level 5 Presentation-Baseline Investigation

Date: 2026-11-16
Status: READ-ONLY INVESTIGATION COMPLETE

## Executive conclusion

**Disposition: REQUIRES VISUAL CALIBRATION BEFORE IMPLEMENTATION.**

Level 5's current C# projection and depth progression are finite and coherent.
Its near-rim model origin is substantially above the accepted Level 1-4
presentation center. A post-projection translation of `+112.021645` logical Y
would align that origin with the common center, but the resulting full
near-rim vertical envelope extends `22.364` host pixels into the normal HUD's
vertical band. Conversely, moving the full-size tube down enough to clear that
band displaces its origin `22.364` pixels below the common center.

The source establishes the conflict and a mathematical fit bound, but not the
actual visual collision with HUD text or an acceptable positive clearance.
No exact size, aspect, or center exception is therefore selected. No MAME
image was requested or inspected. Level 5 gameplay/model behavior remains
untouched.

## Geometry and provenance

`LevelLoader.LoadLevel5Cross` defines the production shape as 16 literal
integer points, in this exact order:

```text
(-30,210), (-60,120), (-120,60), (-210,30),
(-210,-30), (-120,-60), (-60,-120), (-30,-210),
(30,-210), (60,-120), (120,-60), (210,-30),
(210,30), (120,60), (60,120), (30,210)
```

`Continuous=true`, so `LevelDefinition.PlayableLaneCount` gives 16 playable
lanes with an implicit closing edge. The points are REFERENCE-DERIVED from the
Original Arcade cross selected in the Incitti reference set; the project
history does not claim that these C# coordinates are a ROM-exact conversion.
The existing Atari comparison found the same closed 16-point topology but
non-rounding residuals. The points and their order feed gameplay lane identity,
movement, and collision; no model edit is proposed.

Current configuration: progression index 4, displayed Level 5, board 4,
palette 0, `Depth=400`, `Position=50`, `CenterY=415`, and `YOffset=-90`.
Production starts at the accepted player lane/position and retains its
previously accepted Level 5 gameplay configuration.

**Acceptance distinction:** Level 5 production gameplay has separate manual
acceptance records, including Neal's 2026-11-10 statement that Level 5 plays
as expected with no regressions in Levels 1-4. This is gameplay acceptance;
no Level 5 presentation acceptance was found or inferred. The task explicitly
freezes Level 4 as manually accepted. The older Level 4 center report still
labels that checkpoint as awaiting a manual check; this investigation follows
the current task's accepted/frozen premise and does not reopen Level 4.

## Legacy presentation baseline

`Program.cs` sets production `K=70`, `ZOffset=5`; default portrait
`PortraitPerspective=0`. With Level 5 near/middle/far depths `50/250/450`,
the legacy projection is:

```text
xLogical = 400 + x * 70 / (z + 5)
yLogical = 415 + (y - 90) * 70 / (z + 5)
```

The portrait camera maps logical `(x,y)` to host
`(640 + 1.26*(x-400), 480 + 1.26*(y-412.476190))`.
The normal production HUD's lowest line ends at host Y `165.6`. Negative
clearance below means the vertical envelope overlaps that Y band; it does not
by itself prove horizontal overlap with HUD text.

### Level 5 as currently projected (no translation)

Logical bounds and sizes:

| Depth | Logical X bounds | Logical Y bounds | Width x height | BBox center / origin |
|---:|---:|---:|---:|---:|
| Near 50 | 132.727..667.273 | 33.182..567.727 | 534.545 x 534.545 | (400, 300.455) |
| Middle 250 | 342.353..457.647 | 332.647..447.941 | 115.294 x 115.294 | (400, 390.294) |
| Far 450 | 367.692..432.308 | 368.846..433.462 | 64.615 x 64.615 | (400, 401.154) |

Portrait-host bounds, dimensions, centers, and margins:

| Depth | Host X bounds | Host Y bounds | Width x height | Host center | L/R/T/B viewport margins | HUD vertical clearance |
|---:|---:|---:|---:|---:|---:|---:|
| Near 50 | 303.236..976.764 | 2.089..675.616 | 673.527 x 673.527 | (640, 338.853) | 23.236 / 23.236 / 2.089 / 284.384 | -163.511 |
| Middle 250 | 567.365..712.635 | 379.415..524.686 | 145.271 x 145.271 | (640, 452.051) | 287.365 / 287.365 / 379.415 / 435.314 | +213.815 |
| Far 450 | 599.292..680.708 | 425.026..506.442 | 81.415 x 81.415 | (640, 465.734) | 319.292 / 319.292 / 425.026 / 453.558 | +259.426 |

Local derivatives are equal on both model axes: logical pixels per model
unit are `1.272727`, `0.274510`, and `0.153846` at near/middle/far;
portrait-host derivatives are `1.603636`, `0.345882`, and `0.193846`.
Near/far width and height ratios are both `8.272727`. The origin progresses
from host Y `338.853` near to `465.734` far, a finite `+126.881` pixels.

The live `AtariPresentationAdapter` does not register a Level 5 model-frame
mapping. `VectorRenderer` therefore retains this legacy projection for Level
5, including when the opt-in Atari presentation candidate is enabled.

## Common presentation center and HUD clearance

The accepted Levels 1-4 common target is the portrait viewport center,
host `(640,480)`, or approximately logical `(400,412.476190)`. The reference
point is the projected near-rim model origin, not an average of tube bounds.

Level 5's current near-origin is logical Y `300.455`, host Y `338.853`;
it is `141.147` host pixels above that target. The uniform post-projection
translation to align it is `+112.021645` logical Y (`+141.147` host pixels).
With only that translation, the near bounds become logical Y
`145.203..679.749` and host Y `143.236..816.764`; the near origin/bbox center
is host `(640,480)`. Its host top is `22.364` pixels above the HUD's lowest
normal-gameplay line at `165.6`, so the conservative full-envelope HUD
clearance is `-22.364` pixels. No bound averaging or Level 4 geometry reuse
was used.

For comparison, preserving the unscaled Level 5 tube and its full vertical
envelope would require at least `+129.770563` logical Y from the current
projection merely to meet the HUD line. That puts the near origin at host
Y `502.364`, `22.364` below the common center; strictly positive clearance
requires still more. Alternatively, if centered, the uniform post-projection
scale must be strictly below `0.933592` to create positive full-envelope
clearance (a reduction greater than `6.641%`). This is only a mathematical
bound, not an accepted size, margin, or screen-space correction.

Accordingly: Level 5 does **not** currently align with the common center or
preserve the conservative full-envelope HUD clearance. Center alignment by
translation alone does not satisfy both constraints at current scale. The
source does not establish whether the tube actually intersects HUD text
horizontally, nor which visual trade-off is acceptable.

## Level 4 / Level 5 numerical comparison

For this comparison only, Level 4 uses its accepted common-center placement
and its unchanged C# model displacements under the same `K=70`, `ZOffset=5`,
portrait camera, and depth range. The Level 4 reference geometry has bounds
X `-219..219`, Y `-116..145`; its bbox is vertically asymmetric, so its
origin and bbox center progression are distinct. Level 5 is symmetric, and
its bbox center equals its origin.

| Depth | Level 4 host W x H | Level 5 natural host W x H | Level 5 normalized width / height difference |
|---:|---:|---:|---:|
| Near 50 | 702.393 x 418.549 | 673.527 x 673.527 | -4.110% / +60.920% |
| Middle 250 | 151.496 x 90.275 | 145.271 x 145.271 | -4.110% / +60.920% |
| Far 450 | 84.905 x 50.594 | 81.415 x 81.415 | -4.110% / +60.920% |

The Level 4 width/height ratio is `1.678161`; Level 5 is `1.000000`,
a relative ratio difference of `-40.411%`. Both have the same near/far size
ratio `8.272727`, since their depth range and camera are shared.

Using projected model-origin movement, Level 4 progresses `91.636` host
pixels near-to-far; Level 5 progresses `126.881`, or `+38.462%`. The
asymmetric Level 4 bbox-center progression is `71.194` pixels, while
Level 5's bbox center follows its origin at `126.881` pixels (`+78.218%`).
At the near rim, accepted Level 4 host margins are L/R `8.804`, top `293.978`,
bottom `247.473`, and HUD clearance `+128.378`. Level 5's common-center
translation-only margins are L/R `23.236`, top/bottom `143.236`, and
conservative HUD clearance `-22.364`. These are extent measurements, not
visual-fidelity judgments or reasons to normalize Level 5 to Level 4.

Normalized by portrait viewport dimensions (720 wide, 960 high), Level 4
near-rim L/R margins are `1.223%` each and top/bottom are `30.623% / 25.778%`.
Level 5's common-center L/R margins are `3.227%` each and top/bottom are
`14.920% / 14.920%`. Relative to Level 4, Level 5's margins differ by
`+2.005` percentage points horizontally, `-15.702` points at the top, and
`-10.858` points at the bottom. The negative HUD-clearance result above is
the additional comparison against the HUD boundary, not the viewport edge.

## Atari correspondence and unit-domain risk

The saved Levels 2-6 mapping report relates displayed Level 5 to Atari record
04. The bounded fit was reproduced from its saved Atari point table; no ROM
or Math Box investigation was reopened.

- Both shapes have closed 16-point topology. The uniform comparison aligns
  C# index `i` with Atari index `(i+4) mod 16`, forward, with X/Y orientation
  `+1/+1`.
- Uniform fit: `1.841523275` C# units per Atari unit, references `128/128`.
  In raw Atari coordinates the translations are `-235.714979` on each axis.
  Maximum absolute residuals are X `6.830466`, Y `10.513512`; RMS is
  `8.074509`.
- Independent-scale alternative: alignment `(3-i) mod 16`, reversed, axes
  `-1/+1`; `aX=1.866696191`, `aY=1.817608078`, references `128/128`,
  raw translations `+238.937112/-232.653834`. Maximum absolute residuals
  are X `7.333924`, Y `9.987378`; RMS is `7.721225`.
- The centered fit is PARTIAL STRUCTURAL CORRESPONDENCE, not a rounding
  conversion. Both fitted scale(s), references/translations, signs, and
  point-index alignment are coordinate-correspondence metadata only. They do
  not establish a Level 5 screen-space gain or authorize reindexing.

Applying the inverse uniform fit directly to existing C# model displacements
would multiply them by `0.543029`: the current half-extent `210` would become
`114.036` (a `45.697%` contraction; full width `420` to `228.072`). Applying
the forward correspondence scale as a screen gain would instead expand the
half-extent to `386.720`. This is the unit-domain failure mode: a valid
coordinate correspondence is not a renderer gain.

## Depth baseline

At current startup values, the camera denominators at near/middle/far are
`55/255/455`, all finite and nonzero. Near is the largest rim, far is
recessed, and the origin moves coherently down-screen by `126.881` host
pixels from near to far. Tube points and world anchors use the same legacy
projection/depth interval and approach offsets. No Level 5-specific failure
supports changing this progression. **The established C# depth progression is
the safe first baseline**; literal Atari reciprocal-depth arithmetic or
signed Y2D is not justified here.

## Frame / aspect / placement / size decisions

- **Model-frame conversion — NO.** Level 5 already has project/reference
  geometry. Its record-04 fit is approximate correspondence metadata, not
  evidence for a render-time conversion.
- **Presentation aspect — NO, not from available nonvisual evidence.** The
  independent fit does not support screen-axis gains; no source-derived
  presentation aspect is established.
- **Placement — YES for center alignment only.** `+112.021645` logical Y
  aligns the near model origin with the accepted target. It does not preserve
  conservative global HUD clearance at current size; the required visual
  priority/exception is unresolved.
- **Size — UNKNOWN / requires visual calibration for a positive clearance.**
  The source bounds impose a strict uniform-scale ceiling below `0.933592`
  if common center and full-envelope HUD clearance are both mandatory, but
  no minimum positive gap or acceptable size is established. This is not a
  justification to apply an arbitrary scale before manual comparison.

### Decision-relevant unknowns

| Unknown | Could change disposition? | Architecture? | Gameplay/model? | Center/HUD handling? | Next candidate? |
|---|---|---|---|---|---|
| Whether the centered near apex intersects actual HUD text horizontally | Yes | Could select translation-only versus extent correction; still render-only | No | Yes | Yes |
| Neal's acceptable positive HUD gap and center exception, if any | Yes | Could select placement or uniform size only | No | Yes | Yes |
| Whether an aspect adjustment is visually required | Yes | Could add an axis-specific render transform | No | No gameplay change | Yes |

No ROM, gameplay, or further source investigation is recommended for these
unknowns. The remaining decision is visual calibration, expressly outside
this read-only task.

## Disposition and smallest next milestone

**REQUIRES VISUAL CALIBRATION BEFORE IMPLEMENTATION.** The source baseline is
complete, but it does not decide the trade-off between the accepted common
center and Level 5's unusually tall near envelope. Implementing a guessed size,
aspect, or center exception would exceed the evidence.

Exactly one next milestone: **a bounded, Neal-owned Level 5 visual-calibration
and acceptance session** to decide whether the common-center presentation
needs a uniform render-only extent adjustment or a documented placement
exception to retain positive HUD clearance. Keep the existing points, order,
lanes, topology, gameplay, depth progression, score/lives, and deterministic
playback unchanged. No image was requested or inspected in this investigation.

## Scope and completion

- Prompt: `AI-Logs/Prompts/2026-11-15-level5-presentation-baseline-investigation-prompt.md`.
- Report: `AI-Logs/Reports/2026-11-15-level5-presentation-baseline-investigation-report.md`.
- Exactly these two authorized, ignored artifacts were created; neither was
  staged or committed.
- Source/handoffs read: current `LevelLoader`, `LevelConfiguration`,
  `LevelDefinition`, `Level`, `Program`, `Projection`, `DisplayConfiguration`,
  `AtariPresentationAdapter`, `VectorRenderer`; the Level 4 common-center
  report; the Levels 2-6 coordinate-mapping report; Level 5 implementation,
  no-direct-Flipper, and concurrency-acceptance reports; and relevant durable
  behavior-spec acceptance notes.
- Calculations: bounded inline projection/bounds/host-camera/derivative/HUD
  calculations; bounded uniform and independent coordinate fits reproduced
  from the saved mapping report's point table. No temporary files were made.
- Starting state: branch `master`, HEAD
  `385f603a94391756b2ca6cccd524dc17b281e782`, upstream `origin/master`,
  ahead/behind `0/0`; index empty. The worktree already had 16 modified
  tracked files and 8 untracked paths. Their state was preserved; one
  protected visual asset reported by Git was not inspected or re-listed.
- Ending state: branch, HEAD, upstream divergence, pre-existing tracked and
  untracked worktree/index state unchanged. Only the two authorized ignored
  AI-Logs artifacts were added.
- No source/test/configuration/documentation/reference or visual files were
  modified; no build, test suite, game, or gameplay was run. Nothing was
  staged, committed, pushed, cleaned, reset, restored, or stashed.
- Manual visual acceptance: **NOT PERFORMED**.
