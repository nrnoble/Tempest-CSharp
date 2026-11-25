# Tempest Level 16 Shared-Center Flipper Transition Investigation

**Status: INVESTIGATION COMPLETE — NO IMPLEMENTATION AUTHORIZED OR STARTED.**
Production and test code were not changed. The interactive game was not
launched. No files were staged, committed, or pushed.

## 1. Executive summary

**ROM-BACKED:** Level 16 does not perform a Flipper lane change by asking an
inward ray to intersect the tube polygon. The ROM represents a Flipper's
between-segment phase as a 4-bit angle in `$02cc`, derives the start and target
angles from the current lane, direction bit, and `tube_angle[]`, and prepares a
rotatable/scalable vector-glyph transform from a 16-step directional lookup.
For lane 0 to lane 15, the ROM starts at phase `$0B`, advances through
`$0C,$0D,$0E,$0F,$00,$01,$02`, and completes at `$03`: eight of the ROM's
sixteen angular units, or a half-turn. The pivot reference is point 0, the
shared coordinate.

That result is not equivalent to the current C# calculation merely because
both report a 180-degree magnitude. After the C# Y-axis conversion, the ROM's
angle-table order takes the other half-turn from the C# `SpatialSweepDegrees =
+180` path. In addition, the C# renderer clamps the rotating endpoint to the
first tube-polygon boundary along every intermediate ray and throws when
there is no such boundary. No such polygon-boundary query exists in the ROM
transition path. Thus, finding a different polygon intersection would not
reproduce the ROM behavior; the mismatch begins in the chosen angular path,
then is compounded by C#'s boundary-intersection requirement.

The ROM uses the same generic movement and rendering code for Level 16; there
is no special shared-center or Level-16 branch. Its ordered geometry and
angle row supply the relevant behavior. The ROM source therefore uniquely
supports an **angle-table-driven, fixed-glyph transform about the shared
vertex**, not a piecewise lobe pivot or retract/collapse/re-expand model.
However, its P-code loops through all angle values without yielding between
them, so the exact number of separately visible rendered phases is not
established by that control-flow trace alone. The code does not encode an
eight-frame sweep.

**Recommendation for a separately authorized implementation:** translate the
ROM phase/angle-table transform for Level 16, preserving the exact lane order
and `$24` route, and do not require a polygon-boundary intersection during
that transform. Keep already accepted C# behavior on other levels unchanged
unless Neal separately authorizes reopening it. This report does not implement
that recommendation.

## 2. Starting repository state

- Branch: `master`; HEAD: `63d37adb6c0b6b4f28a422f0c6c638cf5391e88c`;
  upstream: `origin/master`.
- Index was clean. The worktree was already dirty: 24 tracked files were
  modified and several unrelated or Level 15/16 candidate files and AI-Logs
  were untracked. Relevant pre-existing edits include `Level.cs`,
  `Flipper.cs`, `LevelLoader.cs`, Level 16 configuration, and project docs.
  These were treated as existing work and preserved.
- The three required prior reports were present and read completely. The
  requested shared-center prompt and report were not present in the starting
  status.

## 3. Prior crash findings accepted as established

The prior runtime crash report is the immediate starting point; its
reproduction was not repeated. The following are retained as established
findings:

- The exact Level 16 ROM geometry is intact; the static tube looked good in
  Neal's manual testing.
- A production Tanker-cargo Flipper reproduced the projection crash headlessly
  through the production entity/render-projection path.
- The failing `$24` transition is source lane 0 to destination lane 15,
  direction `+1`, at progress 0.8 in the production-path probe.
- The current C# representation gives it a 180-degree directed turn, hinges
  at the repeated central coordinate point 0/8, and calculates no valid
  forward polygon-boundary intersection at the failing progress.
- The exception is in model-space geometry, before screen projection.
- This cannot be repaired by selecting a different existing intersection:
  the failing ray has no valid forward polygon intersection.

These are not reopened or re-proved here. The ROM trace below addresses what
representation the Atari code uses instead.

## 4. ROM `$24` execution and Flipper state trace

**ROM-BACKED.** The local disassembly defines the per-enemy P-code PC in
`enm_move_pc[]`; `move_enemies` loads it, sets `pcode_run`, and dispatches
P-ops until the `done` (`$00`) operation clears that run flag
(`Tempest Rom Source code.md`, `$9b1b-$9b52`, `$9b3a-$9bcc`). Level 16 selects
the `$24` Flipper program at `$a11b-$a138`: move, set the group count to two,
start a flip, continue the flip while its flag is set, count down, reverse,
repeat for three, reverse, and return to the two-flip group. The route counter
is `$0298,x`; it counts flips in each group, not angular progress.

The relevant state is:

| ROM state | Meaning in this transition |
|---|---|
| `enemy_seg,x` (`$02b9` base) | Current segment/lane index. The adjacent destination is inferred from this index and the direction bit; ROM does not keep a separate destination-lane field. |
| `$0283,x` bit `$40` | Segment-index direction: set increases `enemy_seg`; clear decreases it. |
| `$0283,x` bit `$80` | Between-segments / Flipper-flipping state. |
| `$02cc,x` | While flipping, high bit `$80` plus current 4-bit angle phase. Outside a flip, the ROM comment describes it as the segment last flipped from. |
| `enm_move_pc,x` | P-Code program counter for the enemy's selected movement program. |
| `$0298,x` | `$24` route's remaining-flips count (2 or 3). |
| `enemy_along,x` and `enemy_along_lsb,x` | Position along the tube; `$24` executes the move-per-speed action between angle steps. |

`eact_12` (`$9e5d`) starts the flip, applies the open-end reversal rule, sets
bit `$80`, and initializes `$02cc` through `get_angle`. Level 16 is closed
(`open_level == 0`), so `rev_if_edge` does not alter this transition.
`eact_14` (`$9d81`) increments or decrements the low angle nibble according to
bit `$40`, asks `get_angle` for the target direction, and clears bit `$80` when
the target phase is reached. For a decreasing transition, it then decrements
`enemy_seg` modulo 16. Thus, source lane 0 remains the active segment during
the flip and becomes destination lane 15 at completion.

There is no floating-angle or separate continuous pivot-progress variable in
the ROM. `$02cc` is a discrete phase, `$0283` marks the transition, and P-Code
control flow determines the phase sequence. The current phase advances one
16th-turn unit per `eact_14` execution.

**Timing qualification:** `$24` has a `done` immediately after `start flip`.
Its continuation loop runs `eact_14`, move-per-speed, and the conditional
branch repeatedly until the target is reached; there is no `done` between
those iterations. Since `move_enemies` runs P-Code until `done`, all eight
angle updates for this half-turn occur within one P-Code run after the
start-flip yield. The disassembly does not establish an eight-rendered-frame
animation or a per-angle render yield. It supports discrete internal angle
steps and a boundary state change; exact visible frame sampling is addressed
under uncertainties.

## 5. ROM Flipper vector generation and rendering

**ROM-BACKED.** `draw_enemies` dispatches the Flipper renderer at `$b5eb`.
When `$0283` bit `$80` is clear, the ROM draws the Flipper using
`draw_linegfx` on `enemy_seg` (the current lane segment). When bit `$80` is
set, it takes the other path: `$b634` prepares the transform inputs, then
`$bdcb` draws graphic 0 through the shared rotatable/scalable vector-graphic
routine.

For the flipping path, `$b634`:

1. Uses `tube_x/y[enemy_seg]` as the first transform reference and keeps the
   current along-tube position in `$57/$2f`.
2. Uses `$02cc & $0f` to index the ROM's fixed 16-phase offset tables
   (`$b687` and `$b68b`), adding those X/Y offsets to the current tube vertex
   to provide the second transform reference.
3. Loads the per-level Flipper scale from `lev_fscale[]` and
   `lev_fscale2[]`.
4. Calls the normal vector-glyph path, whose Flipper graphic is the fixed
   eight-line vector data at `$bfd2-$bfe0`.

This is an angle-indexed vector-glyph transform about a tube vertex. It is
not the ROM moving a polygon edge endpoint until a ray intersects the tube.
The source/destination segment geometry is not interpolated through a series
of polygon-boundary intersections. There is no per-phase Flipper scale
reduction in this path: the transform uses the level's Flipper scale, with no
collapse-to-zero or retract/re-extend state.

When the transition completes, the ROM clears the between-segments bit and
updates `enemy_seg`; the drawing dispatch then returns to the lane-based
`draw_linegfx` path. This is a discrete render-representation switch in the
code. The inspected code does not establish whether the final glyph handoff
has a visible positional discontinuity.

## 6. Meaning and use of the Level 16 angle bytes

The previous ROM/configuration investigation's bounded Rev. 2 K1 read
recorded the Level 16 row as:

```text
0B 09 07 05 03 01 0F 0D 0D 0F 01 03 05 07 09 0B
```

No raw ROM file was opened or reread for this task. The local disassembly
declares `lev_angle[16][16]`, loads the selected row into `tube_angle[]`
(`$c2bb-$c2be`), and documents angle entries as 0–15 representing a full
turn. `get_angle` uses the row value for a segment and, for the opposite
direction, uses the preceding segment's value plus eight modulo 16. These
bytes therefore establish lane-associated angular phases and the target phase
for a transition; they are not a complete per-frame animation description.

The separate `$b687/$b68b` lookup provides the directional X/Y reference
vector for the current phase. It is a 16-position, integer sine/cosine-like
table, not Level 16's angle row. The angle row chooses the transition phase;
the fixed lookup converts that phase into the transform reference used by the
Flipper glyph. The `$24` P-Code controls when a flip begins, direction
reversal, and the 2/3 route cadence.

## 7. Exact ROM interpretation of lane 0 ↔ lane 15

The ordered ROM geometry around both shared coordinates is:

| Point | ROM coordinate | Relationship |
|---|---|---|
| 15 | `($90,$A9)` | Lane 15 approaches point 0. |
| 0 | `($80,$80)` | Shared center for lanes 15 and 0; also repeated at point 8. |
| 1 | `($70,$57)` | Lane 0 leaves point 0. |
| 7 | `($70,$A9)` | Lane 7 approaches point 8. |
| 8 | `($80,$80)` | Repeated shared center for lanes 7 and 8. |
| 9 | `($90,$57)` | Lane 8 leaves point 8. |

The Level 16 angle row has `tube_angle[0] = $0B` and
`tube_angle[15] = $0B`. In the failing source-0-to-destination-15 direction,
the ROM direction bit `$40` is clear (decreasing segment index):

1. Start phase: `get_angle` at segment 0 with bit `$40` clear returns
   `tube_angle[0] = $0B`; `eact_12` stores `$8B` in `$02cc`.
2. Target phase: `eact_14` compares against `get_angle` with bit `$40`
   toggled. The set-bit branch backs up from segment 0 to segment 15 and
   returns `(tube_angle[15] + 8) & $0F = $03`.
3. Progression: bit `$40` clear increments the phase nibble from `$0B` to
   `$03` modulo 16: eight steps, exactly half of the 16-step turn.
4. Completion: the ROM clears the flip bit and changes `enemy_seg` from 0 to
   15. It does not change or split the Level 16 tube geometry.

The transform lookup reinforces the row relationship. At phase `$0B`, the
ROM offset is approximately `(-16,-40)`; at phase `$03`, it is approximately
`(+16,+40)`. Those vectors match the ROM point offsets from the shared center:
point 1 minus point 0 is `(-16,-41)` and point 15 minus point 0 is
`(+16,+41)`. This is direct geometric correspondence, not an arbitrary
shortest-path choice.

The two sectors meet at point 0 and are opposite rays. The ROM performs a
half-turn of its Flipper vector transform at that shared vertex. It does not
ask for an inward ray/boundary intersection, shorten the glyph to cross the
center, or switch between two separately computed lobe pivots. The analogous
point-8 junction is also represented by the unmodified generic path; its
adjacent angle entries are `$0D` and `$0D`.

## 8. Which motion model the ROM supports

**ROM-BACKED:** an angle-table-driven vector-glyph transform about the current
segment vertex, with a discrete 4-bit phase and ROM direction rules. For the
specified transition, the angular total is 180 degrees and the phase is
selected by Level 16's row and the decreasing-index direction bit.

The ROM does **not** show:

- source/destination polygon-boundary interpolation;
- an inward endpoint ray constrained to remain on the tube boundary;
- radius interpolation or a scale-to-zero/collapse/re-expand sequence;
- a separate piecewise pivot model for each lobe; or
- a Level-16-specific branch.

The renderer does switch from the between-segment transform to ordinary
segment drawing when the flip flag clears. The phase loop is synchronous
inside P-Code rather than an explicitly yielded phase-per-frame animation.
Accordingly, the algorithmic model is established, while the exact visible
handoff timing/seam remains a display-scheduling question rather than support
for a different geometric model.

## 9. Current C# model comparison

**PROJECT TRANSLATION — current C# candidate, not Level 16 arcade
acceptance:** C# stores a directed transition and continuous floating-point
turn progress, then renders a six-line body between two endpoints. Its
polygon-boundary clamp is a C# geometry rule, not a ROM rule.

**ROM state/model → current C# state/model**

| ROM | Current C# |
|---|---|
| `enemy_seg`, direction bit `$40`, between-segments bit `$80`, and angle phase `$02cc` | `SourceEdge`, `DestinationEdge`, `Direction`, `ActiveTransition`, `FlipperState`, and floating `TurnAngle` / `TotalTurnAngle` |
| P-Code derives target lane from current segment and direction; lane index changes at completion | C# constructs an explicit adjacent `DirectedLaneTransition` before the turn |
| `tube_angle[]` and a fixed directional offset table set the Flipper transform reference | C# derives hinge/interior angle from ordered polygon points and rotates a free endpoint |
| Fixed vector Flipper glyph and level scale | Six-line C# glyph formed between two projected endpoints with normal height |
| No polygon-boundary intersection | Inward endpoint radius is clamped by `CalculateFirstPolygonBoundaryDistance` |
| `$24` P-Code's 2/3 flip counter in `$0298` | The C# `Repeating24FlipCounts` route `[2,3]` and programmed-route state |

The C# route correctly represents the `$24` two/three flip cadence at the
lane-schedule level. The ROM/C# transition geometry and phase state are
different representations.

## 10. First identified ROM ↔ C# divergence

The first divergence is **the signed half-turn path selected for source lane
0 to destination lane 15**, before the boundary-intersection failure. The
ROM side is **ROM-BACKED**; the current sweep is a **PROJECT TRANSLATION**:

- C# `Level.CalculateDirectedLaneTransition` chooses its hinge at point 0,
  source free vertex 1, destination free vertex 15, and derives
  `SpatialSweepDegrees = +180` from the source-lobe winding and C# direction.
- The ROM starts from phase `$0B` and advances the 4-bit phase in increasing
  order through `$03`. The ROM offset lookup, after the project's Y-axis
  inversion, follows the opposite half-turn from the C# positive sweep.
- The C# progress-0.8 failure ray recorded in the prior report,
  approximately `(-0.253,-0.967)`, is consistent with the C# half-turn through
  the opposite side of the shared-center angular gap. The ROM uses its
  direction/angle-table sequence rather than deriving this sweep from
  polygon winding.

The next divergence is the extra C# invariant that every intermediate
rotating endpoint must meet the first polygon boundary. In ROM rendering,
the angle lookup supplies a transform reference regardless of whether a
polygon edge exists along that direction. Consequently, the ray exception is
not a missing ROM boundary rule. Merely suppressing the exception or choosing
another intersection would still fail to reproduce the ROM-selected
half-turn.

## 11. Mark Incitti — secondary evidence only

**REFERENCE-DERIVED:** `Reference/incitti/levelsets/levelset1.txt` identifies
“Level 16 - infinity” and supplies a 16-point ordered outline with the center
repeated at points 0 and 8. The generic `Flipper` in
`Reference/incitti/tempest.bmx` stores `angle` and `totangle`; `Draw` rotates
one transformed lane endpoint around the other according to direction, and
`Update` increments the angle and computes the total from local edge-angle
data before advancing the edge. The Flipper code contains no Level-16
special-case branch and no polygon-boundary-ray query.

This is a secondary engineering reference, not proof of Atari behavior. The
inspection did not establish that Incitti's Level 16 transform matches the
ROM's `tube_angle`/offset-table path at the repeated center or that it avoids
an analogous degenerate local-angle case. No implementation is authorized by
this reference.

## 12. Existing arcade evidence

No direct Level 16 or shared-center Flipper observation was found in the
authorized prior reports/specification. Existing documented arcade
acceptance covers Level 1/2 square Flippers and Level 3 plus-board directed
90/180/270-degree inward pivots; it does not establish the Level 16
self-touching junction motion.

Those observations are **ARCADE-OBSERVED** for their stated levels only.
They do not override the Level 16 ROM trace or establish Level 16's
angle-table path.

## 13. Candidate model classes and evidence

| Candidate | Evidence for / against | Classification and scope |
|---|---|---|
| ROM angle-table transform around the shared vertex | Supported directly by `$02cc`, `get_angle`, the Level 16 row, the directional offset lookup, and the flipping render path. No polygon boundary is required. | **ROM-BACKED** algorithm; preserves all 16 ROM lane indices. A Level-16-only C# translation can preserve already accepted behavior elsewhere. Exact host-space glyph/scale mapping and visible handoff remain to validate. |
| Existing endpoint hinge plus a topology-aware boundary rule | C# and ROM both have a 180-degree magnitude and use the shared vertex. But C# currently selects the opposite half-turn and requires a boundary intersection the ROM never asks for. | Existing pivot concept is only partially analogous. A new ROM-aligned phase path is needed; a “find a different intersection” fix is contradicted by the source. |
| Piecewise lobe-local pivot | Could be imagined from the repeated point, but ROM keeps one source segment/angle state and has no lobe test or special branch. | **HYPOTHESIS**, unsupported by ROM and not indicated as a C# translation. |
| Collapse/re-expand or retract through center | Prior crash analysis identified it as a mathematically possible workaround, but the ROM lookup stays nonzero and the ROM scale is not animated to zero. | **HYPOTHESIS**, contradicted as a ROM-derived model; do not choose it for convenience. |
| Interpolate source and destination segment endpoints | ROM instead recomputes a transform reference from its current discrete phase, then changes to the destination segment renderer when the flip bit clears. | Not ROM-backed; no interpolation of the two tube edges is present in the traced path. |
| Shortest-path or arbitrary 180-degree rotation | Both candidate semicircles have the same magnitude in this case, but ROM selects by the lane angle row and direction bit. The C# winding-derived sign selects the other half-turn. | Not a sufficient rule; must preserve ROM direction/phase order. |

## 14. Is one implementation model uniquely justified?

**Yes, at the algorithm/model-family level:** the local disassembly uniquely
supports the ROM angle-table-driven Flipper vector transform and its
lane/direction phase progression. It does not support a boundary-clamped
endpoint ray, lobe-local pivot, or collapse/re-expand path. The correct
translation must account for the ROM's selected half-turn, not just its
180-degree magnitude.

This does not authorize implementation. Any C# adaptation remains a
**PROJECT TRANSLATION / PROJECT DECISION** and still needs to
map the ROM phase/vector transform to the C# Flipper glyph and projection,
preserve other manually accepted level behavior, and be manually accepted by
Neal.

## 15. Smallest manual MAME/arcade observation for residual uncertainty

No observation is needed to choose between the geometric model families
above. The ROM determines the transform and the P-Code contains no
per-angle-frame yield. The remaining narrow uncertainty is how the machine's
actual display scheduling presents the between-segment state and the final
switch.

If Neal wants to resolve that before implementation, watch one Level 16
Tanker-cargo Flipper as it takes the lane 0↔15 junction during the `$24`
route. The only points to note are whether the vector glyph visibly traverses
intermediate rotations or appears to switch in one update, and whether its
handoff to lane 15 is visibly continuous. No recording or screenshot is
needed.

## 16. Recommended bounded implementation scope

**PROJECT DECISION candidate; not authorized or implemented.** After separate
authorization:

1. Keep Level 16's ROM geometry, order, and `$24` lane-count schedule
   unchanged.
2. Translate the ROM phase state for Level 16 using the row's per-lane angle
   values and ROM direction semantics; preserve the `$0B`→`$03` phase order
   for lane 0→15.
3. Render the C# Flipper through a ROM-aligned angle/vector transform about
   the shared point, using a defined C# scale mapping. Do not require a
   first-polygon-boundary intersection for this transition.
4. Keep accepted endpoint-pivot behavior on existing levels unchanged unless
   Neal separately authorizes reconsidering that shared project translation.
5. Add targeted headless production-entity/render-projection coverage for
   both directions at the two shared-center junctions and the production
   Tanker-cargo `$24` path. Do not turn ordinary gameplay tests into renderer
   tests.
6. Resolve visible phase/handoff cadence through Neal's manual acceptance;
   automated tests cannot establish arcade visual fidelity.

This is a recommendation only. No production candidate or tests were created.

## 17. Recommended next investigation if a detail remains disputed

No further geometry/configuration investigation is indicated by the current
evidence. If Neal disputes the C# glyph mapping or visible cadence, the next
bounded step is to compare one MAME transition with the phase/transform path
in section 15. Do not re-run the crash reproduction or reopen Level 16
geometry/configuration absent contrary evidence.

## 18. Headless regression-strategy recommendation

For unusual tube geometry that introduces new topology or projection behavior,
automated coverage should exercise transition projection through the
production Flipper and renderer path in addition to checking simulation state.
The prior headless Tanker-cargo reproduction demonstrates that this can catch
a dynamic projection failure without launching the game. Keep this gate
targeted to cases with new geometry/projection behavior; ordinary gameplay
tests need not execute rendering calculations.

## 19. Files and resources inspected

- Required prior reports:
  - `AI-Logs/Reports/2026-11-24-level16-rom-configuration-investigation-report.md`
  - `AI-Logs/Reports/2026-11-24-level16-bounded-implementation-report.md`
  - `AI-Logs/Reports/2026-11-24-level16-runtime-flipper-pivot-crash-correction-report.md`
- ROM disassembly: `Reference/arcade/Tempest Rom Source code.md`.
- Current C#: `src/Tempest.Game/Levels/Level.cs`,
  `src/Tempest.Game/Levels/LevelLoader.cs`,
  `src/Tempest.Game/Levels/DirectedLaneTransition.cs`,
  `src/Tempest.Game/Gameplay/Flipper.cs`, and
  `src/Tempest.Game/Rendering/VectorRenderer.cs`.
- Read-only geometry cross-check: `tests/Tempest.Game.Tests/Level16GeometryTests.cs`.
- Secondary source: `Reference/incitti/tempest.bmx` and
  `Reference/incitti/levelsets/levelset1.txt`.
- Existing project evidence: `TEMPEST-BEHAVIOR-SPEC.md` and
  `TEMPEST-DEVELOPMENT-NOTES.md`.
- No raw ROM content was accessed for this investigation. The angle-row values
  are taken from the already-read prior Level 16 ROM report.

## 20. Commands and calculations performed

- Read-only Git baseline: branch, HEAD, upstream, short status, tracked diff
  names, staged diff names, and untracked file names.
- Bounded report reads and `rg` searches over the listed text sources and
  current C# symbols. A workspace semantic search was also used for Flipper
  state/transition context.
- Read-only file-name checks were limited to `Reference/arcade`,
  `Reference/incitti`, and `Reference/incitti/levelsets`; the protected
  `Reference/arcade/roms.rev2/` directory was not enumerated.
- Hand calculation from the recorded ROM row and disassembly:
  `(tube_angle[15] + 8) & 0x0F = ($0B + 8) & $0F = $03`; `$0B` to `$03`
  in increasing modulo-16 order is eight increments, i.e. 180 degrees.
- Hand comparison of the `$b687/$b68b` offsets with ROM points:
  phase `$0B` is approximately `(-16,-40)` and phase `$03` is approximately
  `(+16,+40)`, matching point 1 and point 15 offsets from point 0 within one
  ROM coordinate unit.
- Compared the ROM phase order with the current C# Y-inverted coordinates and
  `SpatialSweepDegrees=+180`; the two half-turn directions differ.
- No scripts, builds, tests, validation runs, or interactive game launches
  were performed. They are unnecessary for this read-only source
  investigation.

## 21. Uncertainties and unresolved evidence

- The examined P-Code has no yield between angle steps; exact vector-display
  sampling and whether the one intermediate between-segment state is visibly
  rendered need direct MAME/arcade confirmation if that distinction matters.
- The source establishes a render-path switch when the flip bit clears, but
  this investigation did not prove whether the handoff is visibly seamless.
- Translating Atari's fixed vector-glyph basis and per-level scale into the
  current C# glyph/projection is a project implementation decision and still
  requires manual visual acceptance.
- No Level 16 arcade observation was found or made. No claim of Level 16
  manual acceptance is made.

## 22. Production/test change confirmation

**PASS:** no production code, tests, Level 16 geometry, durable behavior
specification, or previous report was modified. Only this paired prompt and
investigation report were created.

## 23. Git action confirmation

**PASS:** nothing was staged, committed, or pushed. Existing tracked and
untracked work was preserved.

## 24. Protected-asset and Level 15 confirmations

- **PASS:** no screenshots, images, or videos were searched or inspected.
- **PASS:** Level 15 was not used as the behavioral or implementation model.
- **PASS:** the interactive game was not launched; no MAME/arcade observation
  was performed.

**Stop condition satisfied.** Await Neal's review and authorization before
implementation.
