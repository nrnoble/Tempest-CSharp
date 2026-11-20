# Levels 1–9 ROM Camera / Perspective Read-Only Investigation

**Date:** 2026-11-14
**Status:** COMPLETED — READ-ONLY INVESTIGATION
**Primary diagnostic classification:** ROM DATA EXTRACTION GAP

## Executive finding

The source establishes that Atari has distinct per-tube reciprocal-scale,
3-D camera-height, signed post-projection Y-offset, and open/closed inputs.
Current C# has no direct per-level equivalents for the first three as a
coherent ROM-backed set. Its `YOffset` is applied before perspective division
(the stage at which a camera-height term belongs), while `CenterY` is applied
after division (the stage at which a 2-D placement term belongs), but neither
is populated from Atari's per-level table in the current loaders. The property
names alone therefore do not establish a correct translation.

There is also a material construction-path difference: Levels 1–7 use literal
C# point arrays and hand-authored `CenterY`/`YOffset`; Levels 8–9 construct
centered, scaled, Y-reflected points from separate byte arrays and share
`CenterY=300`, `YOffset=0`. All nine then use the same C# projection path.
This is a demonstrated provenance/transform inconsistency, not proof that the
Level 1–7 geometry values themselves are wrong.

The authorized disassembly contains the field declarations, setup, and runtime
uses, but only `.chunk` placeholders for the 16-entry tables. It exposes no
per-level values for `lev_scale`, `lev_y3d`, `lev_y2d`, `lev_y2db`, or
`lev_open`. Consequently the source supports identifying missing semantics,
but not a responsible per-level translation or a definitive numeric root
cause for the Level 1–7 appearance. Do not tune the shared coefficient as a
substitute for this missing evidence.

## Starting state, scope, and preservation

Starting repository state:

- Branch: `master`; upstream: `origin/master`.
- `HEAD` and `origin/master`: `385f603a94391756b2ca6cccd524dc17b281e782`.
- Index clean.
- Ten expected tracked candidate files modified and unstaged:
  `src/Tempest.Game/Debug/DebugController.cs`,
  `src/Tempest.Game/Program.cs`,
  `src/Tempest.Game/Rendering/DisplayConfiguration.cs`,
  `src/Tempest.Game/Rendering/Projection.cs`,
  `src/Tempest.Game/Rendering/VectorRenderer.cs`,
  `tests/Tempest.Game.Tests/DebugControllerTests.cs`,
  `tests/Tempest.Game.Tests/DisplayConfigurationTests.cs`,
  `tests/Tempest.Game.Tests/GameOverDensityDiagnosticTests.cs`,
  `tests/Tempest.Game.Tests/GameplayRecordingTests.cs`, and
  `tests/Tempest.Game.Tests/TransitionPresentationTests.cs`.
- This exactly matches the latest human-visible calibration report. The
  starting candidate diff hash was
  `f3bf422e9f38b61ebca93265f822faca29a24fd9`.
- No non-ignored untracked files.

Only the requested ignored prompt and report were created. No code, tests,
tracked documentation, candidate values, or project configuration were
changed. No source diff was staged, committed, or pushed. No build, test, or
interactive game was run.

## Materials inspected

Project-local Atari evidence:

- `Reference/arcade/Tempest Rom Source code.md`: bounded reads of the level
  array declarations/comments, `get_tube_no`, level setup around `$c235`,
  mathbox coordinate setup around `$c098`, mathbox result/offset handling
  around `$c0f6`, `open_level` uses, and the level-change state around `$904d`.

Current C# source:

- `src/Tempest.Game/Levels/LevelLoader.cs`
- `src/Tempest.Game/Levels/LevelDefinition.cs`
- `src/Tempest.Game/Levels/LevelConfiguration.cs` (production Level 1–9
  definitions)
- `src/Tempest.Game/Levels/Level.cs`
- `src/Tempest.Game/Gameplay/Player.cs`
- `src/Tempest.Game/Rendering/Projection.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `src/Tempest.Game/Rendering/DisplayConfiguration.cs`
- `src/Tempest.Game/Program.cs`

Required current reports:

- `AI-Logs/Reports/2026-11-14-whole-game-presentation-perspective-envelope-readonly-analysis-report.md`
- `AI-Logs/Reports/2026-11-14-whole-game-presentation-endpoint-normalized-perspective-implementation-report.md`
- `AI-Logs/Reports/2026-11-14-whole-game-presentation-human-visible-perspective-calibration-report.md`

No screenshots, images, videos, MAME/C# frames, gameplay recordings, or
contact sheets were inspected. No Gemini material, Incitti source, web source,
raw ROM file, or ROM directory was inspected. The supplied manual findings
below are retained as manual evidence, not promoted to ROM-backed evidence.

## Manual evidence preserved

**MANUAL / ARCADE-COMPARISON CALIBRATION:** Neal judges Level 9 at
approximately `+125` to be about right. This is a C# project-calibration
observation, not an Atari constant, and is not selected as a production value.

**MANUAL C# OBSERVATION:** Level 8 may plausibly use similar perspective
strength, but has another unresolved presentation problem.

**MANUAL C# OBSERVATION:** Levels 1–7 have fundamentally wrong-looking
perspective before meaningful coefficient tuning; changing the coefficient
changes or exaggerates the existing presentation rather than fixing its root.

**MANUAL GAMEPLAY OBSERVATION:** tested levels continue to play correctly
while perspective changes; no gameplay problem or clipping was observed;
extreme values distort presentation as expected.

## Atari table fields and runtime flow

The disassembly comments at the level-array declarations identify 16 entries
per field. The `get_tube_no` routine maps the current level through
`lev_remap`, stores the result in `curtube`, and the setup at `$c235` indexes
the per-tube arrays with that `curtube`. The source text does not include the
contents of the arrays.

| Field | Textually established representation and runtime use | Presentation/gameplay role |
|---|---|---|
| `lev_scale` | One byte per tube; source comment calls it the reciprocal of level scale. Setup at `$c240` negates the byte into `$5f`/`$5d`, derives `$a0`, and the common coordinate path uses the scale-derived value in the mathbox-relative input at `$c098`. | **ROM-BACKED / SOURCE INTERPRETATION:** projection/mathbox scale or reference input, not a C#-style direct vertex multiplication. Its exact full mathbox meaning is not recoverable from the table declaration alone. |
| `lev_y3d` | One byte per tube; source comment explicitly calls it a 3-D Y offset, “camera height.” Setup copies it to `y3d`; `$c0b5–$c0c7` compares the point Y against it and sends signed magnitude to mathbox inputs. | **ROM-BACKED:** camera-height/reference input before projection. The textual routine proves it is distinct from the 2-D offset. |
| `lev_y2d`, `lev_y2db` | Two bytes per tube: `lev_y2d` is the low byte, `lev_y2db` the high byte; source comment identifies the combined value as signed 2-D Y offset. Setup loads `$68/$69`. Mathbox output at `$c0f6–$c12d` is then adjusted by this signed offset. | **ROM-BACKED:** 2-D placement after mathbox projection. During level change, setup computes a signed difference divided by 16 into `$0121`; state `$904d` advances `$68/$69`, so this placement participates in a transition. |
| `lev_open` | One byte per tube; `$00` means closed, `$ff` means open. Setup copies it to `open_level`. | **ROM-BACKED:** topology/edge handling, including omitting a closing rim segment and open-edge movement behavior. The setup loads it alongside the other fields; it does not select a separate camera path. |
| `lev_remap` | One byte per entry; `get_tube_no` uses it to map the level cycle to `curtube`. | Determines table indexing, not projection itself. Its values are also omitted from the shown textual table. |
| `lev_x`, `lev_y`, `lev_angle` | Per-tube point/angle arrays; the comments identify coordinates and sector angles. Setup copies the selected row to runtime tube arrays. | Tube/model geometry and lane relationships, distinct from camera inputs. |
| `lev_fscale`, `lev_fscale2` | One byte per entry each; comments identify flipper scale components. | Related per-level rendering/gameplay geometry input, not the tube camera fields under investigation. |

The normal drawing path uses the selected level's setup values through the
shared mathbox coordinate conversion. The scale and `y3d` values are
available to that common conversion; `y2d` is applied to its output. The
textual source shows the 2-D offset explicitly changing during level
transition. It does not show a distinct open-versus-closed projection rule:
the same setup loads scale, `y3d`, both `y2d` bytes, and open status for each
tube without branching on topology to select different camera metadata.

`lev_open` does affect ordinary geometry and movement. The drawing code
reduces the closing-rim iteration for an open tube (`$c395`, `$c5e6`), and
the source labels `rev_if_edge` at `$9eab` “Reverse motion direction if level
open and we've run into an edge.” These are topology/edge behaviors, not
camera selection.

### Per-level values in the authorized textual source

`NOT AVAILABLE FROM AUTHORIZED TEXT SOURCE` means the array declaration is
present but its per-index data bytes are not. This applies to every entry:

| Tube table index | `lev_scale` | `lev_y3d` | `lev_y2d` / `lev_y2db` | `lev_open` |
|---:|---|---|---|---|
| 0–15 (all 16 entries) | NOT AVAILABLE FROM AUTHORIZED TEXT SOURCE | NOT AVAILABLE FROM AUTHORIZED TEXT SOURCE | NOT AVAILABLE FROM AUTHORIZED TEXT SOURCE | NOT AVAILABLE FROM AUTHORIZED TEXT SOURCE |

Therefore, for production Levels 1–9, none of those per-level table values
can be reported as textually established from the authorized disassembly.
The relationship from displayed level number to table index also requires
the omitted `lev_remap` data. No C# constant was substituted for a missing
Atari source value.

## Current C# Levels 1–9 presentation matrix

All rows use the same `LevelDefinition` fields and the same
`VectorRenderer.ProjectWorldPoint` → `Projection.Project` path. “No extra
scale” means the C# point arrays are directly constructed at the listed
coordinates; it does not claim that their provenance is arcade-verified.

| Level | Point construction / topology | Vertices / playable lanes | `CenterY` / `YOffset` | Point transform |
|---:|---|---:|---:|---|
| 1 | Literal C# 16-point polygon; closed (`Continuous=true`) | 16 / 16 | 400 / −80 | None in loader |
| 2 | Literal C# square; closed | 16 / 16 | 400 / −80 | None in loader |
| 3 | Literal C# plus; closed | 16 / 16 | 400 / −80 | None in loader |
| 4 | Literal C# binoculars outline; closed | 16 / 16 | 380 / −65 | None in loader |
| 5 | Literal C# cross; closed | 16 / 16 | 415 / −90 | None in loader |
| 6 | Literal C# triangle; closed | 16 / 16 | 360 / −40 | None in loader |
| 7 | Literal C# clover; closed | 16 / 16 | 310 / −10 | None in loader |
| 8 | Literal byte arrays identified in C# comments as Rev 2 shape row 7; open | 16 / 15 | 300 / 0 | `(rawX−128, −(rawY−128)) × 25/14` |
| 9 | `Level9RomEvidence` byte arrays identified as Rev 2 shape row 13; open | 16 / 15 | 300 / 0 | `(rawX−128, −(rawY−128)) × 25/14` |

Levels 1–7 point coordinates are already the literal `Vector2` values in
`LevelLoader`; there is no shared import-scale or reflection step in those
methods. Levels 8–9 each center the byte coordinates at 128, apply the
`25/14` geometry scale, and negate centered Y to match the C# screen-down
coordinate convention. These are geometry-import/normalization operations,
not the Atari `lev_scale` translation.

The loader marks Levels 1–7 closed and Levels 8–9 open. This yields 16
vertices/16 playable lanes for Levels 1–7 and 16 vertices/15 playable lanes
for Levels 8–9. The renderer draws both open and closed tubes through the same
projection helper; lane wrapping/topology is handled separately.

Common current C# projection and presentation values:

| Input/stage | Current value and meaning |
|---|---|
| `Position` / near depth | 50 |
| `Depth` / tube span | 400; far depth is 450 |
| `K` / `ZOffset` | 70 / 5 at normal program initialization; shared camera inputs |
| Base vertical projection | `CenterY + (y + YOffset) * K / (z + ZOffset)` |
| Portrait perspective | One shared endpoint-normalized additive correction; default coefficient remains 0. `+125` is not adopted here. |
| Portrait output / playfield scale | 1.2 output × 1.05 playfield = 1.26 shared scale |
| Portrait playfield placement | Shared vertical translation −33.1; centered 720×960 viewport in the 1280×960 host |
| HUD | Separate portrait HUD camera |

`CenterY`, `YOffset`, the Levels 1–7 arrays, and the Levels 8–9 coordinate
normalization are current project/legacy C# definitions; the source does not
identify them as literal Atari camera table values. `K=70`, `ZOffset=5`,
scales 1.2/1.05, and −33.1 are current shared project presentation settings,
not Atari table values. The `25/14` factor is an import/geometry
normalization, not evidence of `lev_scale`.

The C# `Level9RomEvidence` block records `Scale=0x0C`, `Y3d=0xB0`,
`Y2d=0x0140` (`Y2dLow=0x40`, `Y2dHigh=0x01`), and `Open=0xFF`. Those are
explicit C#-recorded ROM evidence, not values visible in the authorized
textual table excerpt. They are not equal by name or unit to `K`,
`ZOffset`, `CenterY`, or `YOffset`. The Level 9 definition nevertheless uses
`CenterY=300`, `YOffset=0`; it does not wire these evidence constants into
those fields. Level 8 has no corresponding `lev_scale`/`lev_y3d`/`lev_y2d`
constants in its loader.

## Semantic comparison and pipeline

### `CenterY` and `YOffset`

In C#, `YOffset` is inside the numerator and is divided by depth. It changes
vertical displacement by different amounts at near and far depth. Its
**mathematical stage** is the stage where a camera-height term must enter.
However, the current values are not mapped from Atari `lev_y3d`; the sign and
coordinate conversion are not established. Treating the name `YOffset` as
proof that it is Atari 2-D Y placement is incorrect.

Atari `lev_y3d` is subtracted from the 3-D point Y before the mathbox
projection. It is a camera-height/reference input, not a post-projection
translation. Current C# has no separate named, sourced, per-level
camera-height field. `YOffset` could mathematically carry a converted
camera-height value, but its current per-level values do not demonstrate that
translation.

In C#, `CenterY` is added after division and shifts both rims uniformly. That
stage is more like Atari's signed 2-D offset. But `CenterY` is used as the
projection's vertical center, rather than a separately represented Atari
`lev_y2d` value; there is no sourced conversion, offset sign, or current
per-level table wiring. The apparent one-to-one mapping
`CenterY ↔ lev_y2d` is therefore only a stage analogy.

Atari `lev_y2d`/`lev_y2db` are applied after mathbox projection and their
signed delta is interpolated during a level change. C# `YOffset` enters at
the wrong stage to be a direct equivalent of that field. `CenterY` enters at
the right broad stage but is not a verified translation of the Atari value.

### Scale semantics

Atari's `lev_scale` is explicitly described by the source as reciprocal
level scale, and setup uses it as a mathbox-relative projection input. It is
not shown multiplying the stored `lev_x`/`lev_y` point arrays. The table
comment, negation, and mathbox setup support “reciprocal scale / projection
input”; they do not justify equating it to a direct world-geometry multiplier
or to C# `K`.

C# has three distinct scale-like stages: Levels 8–9 point normalization
`25/14` occurs before projection; `K=70` is the shared projection numerator;
and 1.05 playfield / 1.2 output scales occur after projection. `Position` and
`Depth` provide a shared z interval. None is a demonstrated per-level
translation of Atari `lev_scale`, and there is no per-level scale input in
the C# projection signature.

### Atari → C# conceptual pipeline

| Atari | C# | Mapping |
|---|---|---|
| Displayed level → `get_tube_no` → `lev_remap` → `curtube` | `LevelConfiguration` directly selects a `LevelDefinition` | Both select a tube/layout, but C# does not reproduce the Atari remap table in the rendering path. |
| `lev_x`/`lev_y`/`lev_angle` | `LevelDefinition.Points` plus discrete lane indices | Conceptually related model geometry; Levels 1–7 are literal C# arrays, Levels 8–9 use centered/reflected/scaled byte arrays. |
| `lev_scale` reciprocal-scale/mathbox input | Global `K`, shared z interval, and separate import/post-projection scales | No established per-level equivalent; not numerically interchangeable. |
| `lev_y3d` camera height before mathbox | `YOffset` before reciprocal-depth division | Same broad stage, but no established per-level values/sign translation and no distinct camera-height property. |
| Signed `lev_y2d` applied after mathbox | `CenterY` added after C# projection | Similar stage only; no established Atari value mapping. `YOffset` is not its equivalent. |
| `lev_open` topology and open-edge rules | `Continuous` and `PlayableLaneCount` | Direct conceptual analogue for topology; no camera rule established. |
| Atari mathbox/vector output | `Projection.Project`, then portrait playfield camera | Different implementations. C# endpoint-normalized correction is a project calibration term, not an Atari source field. |

## Levels 1–7 versus Levels 8–9

The loader paths are structurally different and have different evidence
status. Levels 1–7 provide points directly as C# `Vector2` literals with
individually selected `CenterY`/`YOffset` values. Levels 8–9 provide raw
byte-coordinate arrays, center at 128, scale by `25/14`, reflect Y, and use
the same `300/0` projection offsets. All use `Position=50`, `Depth=400`,
`K=70`, `ZOffset=5`, and the same projection method.

Thus C# has effectively encoded different geometry construction and
vertical-placement choices into its level definitions, rather than loading
the Atari per-level camera/scale inputs through a uniform metadata path. This
is a plausible contributor to the observed disparity. The evidence does not
prove that the 1–7 coordinates themselves are wrongly imported, nor that
open/closed status caused the presentation difference.

## Level 9 `+125`, Level 8, and gameplay separation

Level 9 `+125` is a useful **MANUAL / PROJECT CALIBRATION** diagnostic: the
manually judged result shows that Level 9's current C# base geometry and
shared correction can produce a convincing visual response. It does not
show that Atari uses 125, establish the right value for any other level, or
validate Level 1–7's base camera semantics. Because the correction is a
shared render term, increasing it changes the presentation rather than
reconstructing omitted Atari scale/camera/2-D offset inputs.

**LEVEL 8 ADDITIONAL PROBLEM NOT YET ISOLATED.** Level 8 and Level 9 share
the same C# projection method, `CenterY=300`, `YOffset=0`, open topology,
and centered/reflected/`25/14` point transform. Their raw shape coordinates
and geometry differ, but the inspected source establishes no distinct
Level 8 camera rule or specific separate presentation defect. Do not fix or
tune Level 8 from this evidence.

The C# perspective coefficient is applied in the render projection helper;
it does not rewrite `LevelDefinition.Points` or simulation state. Player
movement is stored as lane/edge index, within-lane position, and depth;
lane transition geometry can use the level points, but the portrait
perspective term is not used by movement or collision. The required
human-calibration report also records a 240-step paired simulation test with
coefficient 0 and +50 producing identical gameplay state, recorded inputs,
startup metadata, and RNG progression. This supports presentation/gameplay
separation without authorizing model-geometry changes.

If sourced per-level Atari inputs are translated later, they conceptually
belong in the shared projection/render path used for tube and entity anchors,
with the active near/far interval and established transition/Approach
composition. No transition, playback, or timing redesign is indicated here.

## Decision matrix

| Explanation | Supporting evidence | Contrary / limit | Evidence class | Confidence |
|---|---|---|---|---|
| A. Only coefficient calibration is missing | Level 9 responds to a shared coefficient; endpoint-normalized correction is shared. | Manual evidence says Levels 1–7 are already fundamentally wrong before tuning; Atari has distinct per-tube camera/scale inputs absent from the current loader/projection interface. | Manual observation; ROM-backed field definitions; C# source | LOW as a complete explanation |
| B. C# lacks/misplaces camera-height semantics | Atari `lev_y3d` is explicit camera height before mathbox; C# has no sourced per-level camera-height field. `YOffset` is at the pre-division stage but is not proven to contain converted `y3d`. | The two operations have a broad mathematical stage resemblance; exact values/sign conversion absent. | ROM-backed; source interpretation; project translation | MEDIUM-HIGH |
| C. C# lacks/misplaces 2-D Y-placement semantics | Atari signed `lev_y2d` is applied after projection; C# `YOffset` is pre-division. Current `CenterY` is post-division but not mapped from the Atari table. | A post-projection per-level translation mechanism exists in C# (`CenterY`), so the concept is not wholly absent. | ROM-backed; source interpretation; project translation | MEDIUM |
| D. Incorrect per-level scale translation | Atari has a per-tube reciprocal-scale mathbox input; C# has no per-level scale input and uses global K plus separate geometry/output scales. | Actual table values and full conversion are absent; cannot say which C# numeric is wrong. | ROM-backed field definition; source interpretation | MEDIUM |
| E. Different geometry import paths contribute | Levels 1–7 literal points/varied offsets differ from Levels 8–9 centered, reflected, scaled byte arrays and shared offsets. | Path difference alone does not prove a geometry or presentation defect; all levels share projection. | C# source; manual symptom observation | HIGH structurally, MEDIUM causally |
| F. Open/closed topology directly changes camera rules | `lev_open` is loaded per tube. | Source uses it for edge/rim topology and open-edge behavior, not to choose camera metadata; camera fields load independently. | ROM-backed | HIGH confidence no open/closed camera rule is established |
| G. Multiple factors contribute | Missing distinct scale/camera/2-D inputs and inconsistent geometry construction are both directly visible. | Missing table bytes prevent assigning exact per-level causal weight. | ROM-backed concepts; C# source; manual observation | HIGH structurally; MEDIUM for exact visual cause |

## Root-cause assessment and evidence gaps

The strongest supported diagnosis is a **multi-factor presentation mismatch
with a ROM data extraction gap**:

1. Atari has per-level projection inputs that the C# `LevelDefinition` and
   `Projection.Project` do not represent as separate, sourced fields.
2. The current C# vertical properties enter at different mathematical
   stages; their names and chosen values do not establish a ROM translation.
3. Levels 1–7 and 8–9 are created through materially different geometry and
   placement paths.
4. The authorized textual source omits the per-tube values and the
   level-to-tube remap data needed to check the modern definitions against
   Atari values.

The evidence does **not** establish exact wrong values, a numeric mapping
from Atari units to `K`/`ZOffset`, or an open-tube camera rule. It also does
not establish that model-space geometry must change. Preserve gameplay
coordinates pending stronger evidence.

## Classification and smallest next milestone

**ROM DATA EXTRACTION GAP.** Concepts and rough mathematical stages are
established, but required per-tube values are absent from the authorized
textual source. A ROM-backed per-level presentation implementation would
otherwise require guessing.

Smallest next milestone: obtain or authorize a textual, verifiable listing of
the Rev 2 `lev_remap`, `lev_scale`, `lev_y3d`, `lev_y2d`, `lev_y2db`, and
`lev_open` values needed for production Levels 1–9; map displayed levels to
table indices; then write a bounded translation table without changing
geometry or presentation behavior. If no permitted textual source contains
the bytes, request separate explicit authorization before any bounded ROM
verification. Do not use the protected ROMs in this investigation.

Additional arcade/MAME frames or Gemini video observations would not
materially close the source-data gap or establish the table values. Matched
visual comparisons may help later manual acceptance after source inputs are
known, but are not the next prerequisite.

## Validation and final state

Read-only investigation; no build/test validation was applicable. No
implementation was performed.

Final state:

- Branch: `master`; upstream: `origin/master`.
- `HEAD` and `origin/master`: `385f603a94391756b2ca6cccd524dc17b281e782`.
- Index remains clean.
- The same ten expected candidate files remain modified and unstaged; their
  diff is unchanged from the captured starting candidate.
- No non-ignored untracked files were introduced.
- Only the requested ignored prompt and report were created.
- Nothing was staged, committed, or pushed.
- No visual assets, Gemini, Incitti, web, or raw-ROM material was inspected.
