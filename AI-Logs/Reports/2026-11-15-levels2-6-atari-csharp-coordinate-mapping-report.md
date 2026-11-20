# Atari-to-C# Coordinate Mapping: Levels 2-6

Date: 2026-11-15
Status: READ-ONLY INVESTIGATION COMPLETE

## Executive conclusion

The verified `lev_remap` associations are displayed Levels 2-6 to Atari
records `01`-`05`, respectively. Levels 2 and 3 have consistent approximate
uniform frame mappings with residuals below half a C# coordinate unit. Level 5
has a centered uniform fit with a larger, non-rounding residual and can only be
supported as an explicit approximation. Levels 4 and 6 have partial structural
correspondence but require additional frame features and retain meaningful
residuals; do not authorize them for the current adapter from this evidence.

| Displayed level | Atari record | Classification | Presentation-support disposition |
|---:|---:|---|---|
| 2 | `01` | CONSISTENT APPROXIMATE FRAME MAPPING | READY WITH EXPLICIT APPROXIMATION |
| 3 | `02` | CONSISTENT APPROXIMATE FRAME MAPPING | READY WITH EXPLICIT APPROXIMATION |
| 4 | `03` | PARTIAL STRUCTURAL CORRESPONDENCE | REQUIRES SEPARATE GEOMETRY/PROVENANCE REVIEW |
| 5 | `04` | PARTIAL STRUCTURAL CORRESPONDENCE | READY WITH EXPLICIT APPROXIMATION |
| 6 | `05` | PARTIAL STRUCTURAL CORRESPONDENCE | REQUIRES SEPARATE GEOMETRY/PROVENANCE REVIEW |

No mapping was implemented. The selected index correspondences below are
comparison results only; they do not authorize lane reindexing or production
point changes.

## Evidence and source locations

- **ATARI-BACKED:** Local ROM-derived disassembly declares `lev_x`, `lev_y`,
  `lev_remap`, and the per-record parameter tables at
  `Reference/arcade/Tempest Rom Source code.md:7461-7480`. The level setup
  selects metadata at `:8202-8238`, copies selected point rows at
  `:8244-8263`, and indexes `lev_remap` at `:8287-8308`. Closed/open traversal
  branches on `lev_open` at `:8551-8558`.
- **ATARI-BACKED recovered data:** The raw table bytes and signed parameter
  interpretation below come from
  `AI-Logs/Reports/2026-11-15-atari-projection-data-mathbox-investigation-report.md:17-85`.
  This report was consulted only for its already recovered tables; no raw ROM
  file was opened and no new Math Box analysis was performed.
- **C#-SOURCE-BACKED:** Current literal point definitions and construction are
  in `src/Tempest.Game/Levels/LevelLoader.cs:52-95`; definition topology and
  lane count are in `src/Tempest.Game/Levels/LevelDefinition.cs:4-9`.
  Production identity/progression selections are in
  `src/Tempest.Game/Levels/LevelConfiguration.cs:159-294`.
- **C#-SOURCE-BACKED:** The current adapter has only a uniform scale and Y
  orientation model in `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs:159-168`.
  The render path deliberately uses legacy projection for Levels 2-6 when no
  mapping is registered in `src/Tempest.Game/Rendering/VectorRenderer.cs:216-234`.
- **C#-SOURCE-BACKED gameplay dependencies:** Closed-lane adjacency is
  index-based in `src/Tempest.Game/Levels/Level.cs:439-455`; directed lane
  transitions use ordered point coordinates at `:526-585` and
  `:675-679`. Player rim movement consumes transition angles in
  `src/Tempest.Game/Gameplay/Player.cs:65-120`; Flipper and Tanker routing use
  lane adjacency and indexed geometry in `Gameplay/Flipper.cs` and
  `Gameplay/Tanker.cs:285-325`. Rendering consumes the same points in
  `Rendering/VectorRenderer.cs:1110-1113`.
- **NUMERICALLY DEMONSTRATED:** A bounded deterministic calculation tested all
  16 point correspondences, cyclic rotations, forward/reversed order, X/Y
  signs, fitted translations, uniform scale, and independent scales where the
  uniform fit was not rounding-consistent. The residual definition matches
  `2026-11-15-atari-csharp-coordinate-frame-mapping-report.md:75-76`:
  `RMS = sqrt(mean(dx^2 + dy^2))` over 16 point pairs.

No visual content was searched for, opened, or analyzed. The required Git
status inventory returned worktree path names; visual-asset names are not
repeated here. No Internet or third-party implementation was consulted.

## Level-to-record verification

The ROM-derived source calls `lev_remap` with the current level index; it does
not equate a displayed level label with a record number. Its recovered
level-index table is:

```text
level index: 00 01 02 03 04 05 06 07 08 09 0a 0b 0c 0d 0e 0f
record:      00 01 02 03 04 05 06 07 0d 09 08 0c 0e 0f 0a 0b
```

Current production configurations use progression indices 1, 2, 3, 4, and 5
for displayed Levels 2, 3, 4, 5, and 6. Applying the recovered table verifies:

| Displayed level | C# progression index | `lev_remap` lookup | Atari record |
|---:|---:|---:|---:|
| 2 | 1 | `lev_remap[01] = 01` | `01` |
| 3 | 2 | `lev_remap[02] = 02` | `02` |
| 4 | 3 | `lev_remap[03] = 03` | `03` |
| 5 | 4 | `lev_remap[04] = 04` | `04` |
| 6 | 5 | `lev_remap[05] = 05` | `05` |

These five happen to map to the same numerical record index. This result was
verified through `lev_remap`, not assumed from the displayed labels.

## C# geometry inventory

All five production definitions contain 16 ordered points, set
`Continuous=true`, and therefore expose 16 playable lanes with an implicit
closing edge. The points are literal integer `Vector2` values. Levels 3-6 wrap
their arrays with `Array.AsReadOnly`; Level 2 passes its array through the
`IReadOnlyList<Vector2>` property. No Atari conversion formula is applied by
these five loader methods.

| Level | Factory / source line | `CenterY` / `YOffset` | Geometry provenance |
|---:|---|---:|---|
| 2 | `LoadLevel2Square`, `LevelLoader.cs:52-59` | 400 / -80 | Project-authored literal in current C#; the earlier implementation report calls it an approved ordered square but does not establish a ROM or named reference-coordinate conversion. Historical numeric provenance is undocumented. |
| 3 | `LoadLevel3Plus`, `LevelLoader.cs:61-68` | 400 / -80 | REFERENCE-DERIVED ordered plus-board points, according to `2026-10-31-level3-c1-geometry-implementation-report.md:49-55`; not thereby ROM-derived. |
| 4 | `LoadLevel4Binoculars`, `LevelLoader.cs:70-77` | 380 / -65 | REFERENCE-DERIVED source board per the Level 4 production configuration and implementation report; no Atari-record-03 conversion is documented in the loader. |
| 5 | `LoadLevel5Cross`, `LevelLoader.cs:79-86` | 415 / -90 | REFERENCE-DERIVED Original Arcade cross; the implementation report records selection of the cross from the Original Arcade reference set. |
| 6 | `LoadLevel6Triangle`, `LevelLoader.cs:88-95` | 360 / -40 | REFERENCE-DERIVED Original Arcade triangle with 16 source-ordered points, as documented in `2026-11-04-level6-preimplementation-analysis-report.md:27-45`. |

Current production selections are `Level2Square` (progression 1, displayed 2),
`Level3Mixed` based on `Level3Plus` (2/3), `Level4Candidate` (3/4),
`Level5Production` (4/5), and `Level6Production` (5/6), in
`LevelConfiguration.cs:159-294`. Those selections establish the record lookup
indices; they do not establish point-array provenance.

Ordered C# points, as currently stored:

```text
Level 2:
(161,161), (80,161), (0,161), (-80,161), (-161,161), (-161,80),
(-161,0), (-161,-80), (-161,-161), (-80,-161), (0,-161), (80,-161),
(161,-161), (161,-80), (161,0), (161,80)

Level 3:
(98,98), (98,197), (0,197), (-98,197), (-98,98), (-197,98),
(-197,0), (-197,-98), (-98,-98), (-98,-197), (0,-197), (98,-197),
(98,-98), (197,-98), (197,0), (197,98)

Level 4:
(-33,101), (-101,145), (-185,116), (-219,58), (-219,-29), (-185,-87),
(-101,-116), (-33,-72), (33,-72), (101,-116), (185,-87), (219,-29),
(219,58), (185,116), (101,145), (33,101)

Level 5:
(-30,210), (-60,120), (-120,60), (-210,30), (-210,-30), (-120,-60),
(-60,-120), (-30,-210), (30,-210), (60,-120), (120,-60), (210,-30),
(210,30), (120,60), (60,120), (30,210)

Level 6:
(-116,145), (-171,145), (-137,72), (-102,0), (-68,-72), (-34,-145),
(0,-218), (34,-145), (68,-72), (102,0), (137,72), (171,145),
(116,145), (54,145), (0,145), (-54,145)
```

Point order and coordinates materially affect lane identity, closed wraparound,
player lane transitions, directed Flipper pivots, and Tanker cargo lane
selection. The render path also projects these points and attached entities.
These dependencies are why this report does not propose point reordering,
geometry replacement, or lane remapping.

## Atari coordinate inventory

Pairs below are the recovered unsigned raw bytes `(X,Y)` in stored Atari
record order. They are not projected coordinates. Atari X centering is
`$80`/128. For these five records the point comparison tests Y centering at
128 or a fitted reference; `lev_y3d` is listed separately and is not substituted
for that coordinate reference. Signed `lev_y2d/lev_y2db` is the little-endian
signed 16-bit value. `lev_open=$00` means closed.

| Record | `lev_scale` | `lev_y3d` | `lev_y2d` low/high | Signed Y2D | `lev_open` |
|---:|---:|---:|---:|---:|---|
| `01` | `1c` | `50` | `20/ff` | -224 | `00` closed |
| `02` | `18` | `50` | `40/ff` | -192 | `00` closed |
| `03` | `0f` | `68` | `80/ff` | -128 | `00` closed |
| `04` | `18` | `50` | `40/ff` | -192 | `00` closed |
| `05` | `18` | `50` | `40/ff` | -192 | `00` closed |

Stored point pairs, indices `00`-`0f`:

```text
Record 01:
(f0,80) (f0,b8) (f0,f0) (b8,f0) (80,f0) (48,f0) (10,f0) (10,b8)
(10,80) (10,48) (10,10) (48,10) (80,10) (b8,10) (f0,10) (f0,48)

Record 02:
(f0,80) (f0,b8) (b8,b8) (b8,f0) (80,f0) (48,f0) (48,b8) (10,b8)
(10,80) (10,48) (48,48) (48,10) (80,10) (b8,10) (b8,48) (f0,48)

Record 03:
(ec,94) (d5,b0) (b1,b8) (90,a7) (70,a7) (4f,b8) (2b,b0) (14,94)
(14,6c) (2b,50) (4f,48) (70,59) (90,59) (b1,48) (d5,50) (ec,6c)

Record 04:
(f0,96) (c0,a3) (a0,c5) (94,f0) (6c,f0) (60,c5) (40,a3) (10,96)
(10,6a) (40,5d) (60,3b) (6c,10) (94,10) (a0,3b) (c0,5d) (f0,6a)

Record 05:
(d9,3d) (c2,6a) (ac,97) (97,c4) (80,f0) (69,c4) (52,97) (3c,6a)
(27,3d) (10,10) (35,10) (5a,10) (80,10) (a6,10) (ca,10) (f0,10)
```

The source setup copies each selected 16-byte coordinate row in index order
(`Tempest Rom Source code.md:8244-8263`). No endpoint is duplicated to encode
closure: all five Atari rows have 16 points and `lev_open=00`, matching the
16-point continuous C# topology.

## Per-level mapping results

The transform notation is `C# ~= (aX * signX * AtariX + tx,
aY * signY * AtariY + ty)`, with correspondence index `j` as stated. For
centered fits, references are reported as `-tx/(aX*signX)` and
`-ty/(aY*signY)`. RMS is the Euclidean point residual RMS over all 16 points.
Translations and scales are fitted to all 16 correspondences; none is a
per-point adjustment. Index correspondences are not production lane changes.

### Level 2 / record 01

- **Mapping:** For C# index `i`, compare to Atari point
  `j=(i+2) mod 16`; forward order. `x ~= 1.436688312*(X-128)` and
  `y ~= 1.436688312*(Y-128)`.
- **Fit:** uniform scale `1.436688312`; independent-scale test returned the
  same scales. X/Y signs `+1/+1`; references `128/128`; fitted translations
  `-183.896104/-183.896104`.
- **Residual:** max absolute X `0.454545`; max absolute Y `0.454545`; RMS
  `0.337100` C# units.
- **Topology:** exact closed/continuous, 16 vertices/lanes; no duplicate
  endpoint or omitted-edge convention is needed.
- **Provenance:** Current geometry is a static project literal. Earlier
  project notes call it an approved ordered square but do not document the
  numeric ROM conversion. The numerical fit does not establish historical
  ROM provenance.
- **Classification:** CONSISTENT APPROXIMATE FRAME MAPPING. The residual is
  within half a C# coordinate unit; do not call it exact.
- **Disposition:** READY WITH EXPLICIT APPROXIMATION. It fits the current
  adapter's centered, uniform-scale, positive-Y frame, but must be labeled as
  approximate and manually validated if implemented.

### Level 3 / record 02

- **Mapping:** For C# index `i`, compare to Atari point
  `j=(i+2) mod 16`; forward order. `x ~= 1.756696429*(X-128)` and
  `y ~= 1.756696429*(Y-128)`.
- **Fit:** uniform scale `1.756696429`; independent-scale test returned the
  same scales. X/Y signs `+1/+1`; references `128/128`; fitted translations
  `-224.857143/-224.857143`.
- **Residual:** max absolute X `0.375000`; max absolute Y `0.375000`; RMS
  `0.433013` C# units.
- **Topology:** exact closed/continuous, 16 vertices/lanes; no endpoint or
  closing-edge convention difference.
- **Provenance:** The points are REFERENCE-DERIVED from the ordered plus
  board documented in the Level 3 implementation report, not a direct Atari
  conversion.
- **Classification:** CONSISTENT APPROXIMATE FRAME MAPPING. Residuals are
  below half a C# coordinate unit; they are not zero.
- **Disposition:** READY WITH EXPLICIT APPROXIMATION. The fitted centered,
  uniform-scale frame is compatible with the current adapter's scale and
  positive-Y representation; future support still requires per-level scale
  and manual validation.

### Level 4 / record 03

- **Best uniform fit:** C# index `i` compares to Atari
  `j=(3-i) mod 16`; reversed order, X sign `-1`, Y sign `+1`, uniform scale
  `2.119815270`, references `128/121.159781`, translations
  `271.336355/-256.836355`. Max absolute residuals are X `9.940049`,
  Y `11.790345`; RMS `8.456060`.
- **Best independent-scale fit (used for classification):** same index
  rotation/reversal and signs; `aX=2.081778520`,
  `aY=2.231150659`, references `128/121.501111`, translations
  `266.467651/-271.087284`. Max absolute residuals are X `8.048826`,
  Y `5.595232`; RMS `6.395377`.
- **Current-adapter constraint check:** with the fixed centered origin,
  uniform scale, positive X, and either Y orientation, the best fit has
  `u=2.119815270`, `e=+1`, correspondence `j=(4+i) mod 16`, max residuals
  X `9.940049`, Y `26.290345`, and RMS `16.785558`.
- **Topology:** exact closed/continuous, 16 vertices/lanes. No endpoint or
  closing-edge convention difference.
- **Provenance:** The project identifies this as the reference Level 4
  binoculars board, but the loader has no ROM point-conversion formula. This
  comparison does not establish Atari-record-03 provenance for the literals.
- **Classification:** PARTIAL STRUCTURAL CORRESPONDENCE. Independent scaling
  improves the fit but still leaves non-rounding residuals and a shifted Y
  reference. The best fit also needs an X reflection not represented by the
  current adapter's `(uniform scale, Y orientation)` mapping.
- **Disposition:** REQUIRES SEPARATE GEOMETRY/PROVENANCE REVIEW. A numerical
  structural fit is not sufficient to authorize a current-adapter mapping.

### Level 5 / record 04

- **Adapter-compatible centered fit:** C# index `i` compares to Atari
  `j=(i+4) mod 16`; forward order, X/Y signs `+1/+1`, uniform scale
  `1.841523275`, references `128/128`, translations
  `-235.714979/-235.714979`. This is a symmetry-equivalent fit to a
  reverse/X-reflected solution; the selected equivalent uses the current
  adapter's orientation.
- **Uniform residual:** max absolute X `6.830466`; max absolute Y
  `10.513512`; RMS `8.074509`.
- **Independent-scale comparison:** `aX=1.866696191`,
  `aY=1.817608078`; references `128/128`; max absolute residuals X
  `7.333924`, Y `9.987378`; RMS `7.721225`. This tied
  symmetry-equivalent fit uses `j=(3-i) mod 16`, reversed order, signs
  `-1/+1`, and translations `238.937112/-232.653834`. It does not reduce
  the residual to rounding.
- **Topology:** exact closed/continuous, 16 vertices/lanes. No endpoint or
  closing-edge convention difference.
- **Provenance:** REFERENCE-DERIVED Original Arcade cross; project history
  records the selected reference geometry. It does not claim that these
  coordinates are an exact conversion of Atari record 04.
- **Classification:** PARTIAL STRUCTURAL CORRESPONDENCE. The centered uniform
  fit is bounded but has multi-unit residuals, not ordinary integer rounding.
- **Disposition:** READY WITH EXPLICIT APPROXIMATION. A centered uniform
  scale and positive orientation can be supplied to the current adapter
  without changing stored points or lane identities, but the mismatch must
  remain explicit and the candidate must be manually validated before
  acceptance.

### Level 6 / record 05

- **Best uniform fit:** C# index `i` compares to Atari
  `j=(10-i) mod 16`; reversed order, X sign `+1`, Y sign `-1`, uniform
  scale `1.577015249`, references `127.687500/106.343338`, translations
  `-201.365135/167.705065`. Max absolute residuals are X `6.419397`,
  Y `7.221406`; RMS `4.686681`.
- **Best independent-scale fit (used for classification):** same index
  rotation/reversal and signs; `aX=1.527806332`,
  `aY=1.613641266`, references `127.687500/105.887264`, translations
  `-195.081771/170.864059`. Max absolute residuals are X `4.534080`,
  Y `1.590155`; RMS `1.935895`.
- **Current-adapter constraint check:** with the current adapter's fixed
  `$80` centering, uniform scale, and Y-sign-only mapping, the best fit has
  max residuals X `8.205471`, Y `54.851524`, RMS `31.890892`. The much
  better unconstrained fit needs independent axes and a substantially
  different Y reference.
- **Topology:** exact closed/continuous, 16 vertices/lanes. No endpoint or
  closing-edge convention difference.
- **Provenance:** REFERENCE-DERIVED Original Arcade triangle according to
  the Level 6 preimplementation record. That source establishes Incitti
  reference geometry, not an Atari record-05 coordinate conversion.
- **Classification:** PARTIAL STRUCTURAL CORRESPONDENCE. The independent
  affine fit retains residuals beyond rounding and does not fit the current
  adapter's centered uniform model.
- **Disposition:** REQUIRES SEPARATE GEOMETRY/PROVENANCE REVIEW. Do not
  compensate by altering Level 6 points, point order, or gameplay geometry
  in this presentation milestone.

## Cross-level conventions

| Property | Level 2 | Level 3 | Level 4 | Level 5 | Level 6 |
|---|---|---|---|---|---|
| Best-fit X reference | 128 | 128 | 128 | 128 | 127.6875 |
| Best-fit Y reference | 128 | 128 | 121.5011 | 128 | 105.8873 |
| Selected X/Y signs | `+ / +` | `+ / +` | `- / +` | `+ / +` (symmetric fit) | `+ / -` |
| Selected index correspondence | `i+2`, forward | `i+2`, forward | `3-i`, reversed | `i+4`, forward | `10-i`, reversed |
| Best-fit scale | 1.436688 | 1.756696 | 2.081779 / 2.231151 independent | 1.841523 uniform | 1.527806 / 1.613641 independent |
| Topology | closed | closed | closed | closed | closed |

There is no shared unit scale across Levels 2-6. Levels 2 and 3 share the
same positive-axis orientation, centered reference, and `+2` forward index
alignment, but have different scales. Levels 4-6 do not share that start-index
or axis-orientation convention. All five have matching closed topology.
Level 2's source provenance is undocumented; Levels 3-6 have reference-derived
geometry provenance, but that does not make their coordinates ROM-derived.

Previously established mappings remain unchanged:

- Level 1: approximate `u=25/14`, `e=+1`, same stored order.
- Level 7: approximate structural fit, distinct scale, comparison alignment
  `+5`; preserve production points and order.
- Levels 8-9: exact `u=25/14`, `e=-1`, same stored order.

The new results do not authorize a common mapping for all levels. In
particular, implementing Levels 2 and 3 together as a single shared mapping
would be incorrect because their unit scales differ.

## Relationship to accepted Level 1

The accepted Level 1 checkpoint is preserved and was not reopened: tube shape
and placement remain accepted, clipping is resolved, the upper visual area
remains suitable for eventual persistent game information, gameplay remains
stable, and recorded playback works. No Level 1 geometry, depth progression,
placement, or adapter behavior was changed. The separate `--atari-presentation`
Blaster-exit drawing defect remains outside scope.

No visual acceptance was performed or claimed for Levels 2-6.

## Decision-relevant unknowns

1. **Level 4:** Whether its reference binoculars literals were intended as an
   approximate Atari record-03 conversion, or as a separate reference-derived
   geometry whose coordinate origin/order should not be mapped to that record.
   - Could this change the mapping classification? **Yes**, if project records
     establish a documented approximation or a different intended record
     correspondence.
   - Could it change adapter safety? **Yes**; the current adapter cannot
     express the best fit's X reflection, independent scales, and Y reference.
   - Could it require gameplay/model geometry changes? **Potentially**; any
     model change would affect geometry-dependent movement and is not
     authorized here.
   - Could it materially change the next implementation milestone? **Yes**;
     it could justify a presentation-only extension or keep Level 4 deferred.
2. **Level 6:** Why its reference triangle has a large Y-origin mismatch from
   record 05, and whether that relation is an intentional approximation.
   - Could this change the mapping classification? **Yes**, if a documented
     source conversion or accepted approximation is found.
   - Could it change adapter safety? **Yes**; the current centered uniform
     adapter fit is materially worse than the independent, translated fit.
   - Could it require gameplay/model geometry changes? **Potentially**; do
     not make such a change without a separate authorized gameplay milestone.
   - Could it materially change the next implementation milestone? **Yes**;
     it determines whether Level 6 can be supported by a presentation mapping
     or needs a separate design.

Level 2's historical numeric provenance remains undocumented, but resolving
that history cannot change the measured residual or the selected
READY-WITH-APPROXIMATION disposition. It is not a reason to delay its bounded
presentation candidate. Level 3's reference-derived origin is documented; no
additional provenance research is needed to use its explicitly approximate
fit.

## Smallest next milestone

Implement and validate **Level 2 only** in the render-only candidate, using
the centered positive-axis approximate fit (`u=1.436688312`, `e=+1`) and
leaving stored points, lane identities/order, topology, gameplay, and
playback untouched. Add focused adapter/renderer tests for the Level 2
mapping, explicit fallback for unsupported levels, and non-mutation; then
provide Neal the Level 2 candidate launch for manual acceptance. Levels 3-6
remain unchanged in this milestone. Levels 2 and 3 do not share a unit scale,
so evidence does not justify grouping them.

The most important unresolved item for later work is the Level 4/6 mismatch
intent, not a blocker to the Level 2-only milestone.

## Scope and completion

- Prompt: `AI-Logs/Prompts/2026-11-15-levels2-6-atari-csharp-coordinate-mapping-prompt.md`.
- Report: `AI-Logs/Reports/2026-11-15-levels2-6-atari-csharp-coordinate-mapping-report.md`.
- Files created: exactly the authorized prompt copy and this report. Both are
  ignored `AI-Logs/` artifacts; neither was staged or committed.
- Handoffs consulted: the projection-data/Math Box recovered-table report,
  Atari-to-C# coordinate-frame mapping report, Level 7 geometry-provenance
  disposition report, adapter implementation-plan report, Level 1 correction
  candidate report, and Level 1 placement-only correction report.
- Other project records consulted for geometry provenance: the Level 2 square
  implementation report, Level 3 geometry implementation report, Level 4
  production candidate report, Level 5 implementation report, and Level 6
  preimplementation/implementation reports.
- Source inspected: the local Atari disassembly; `LevelLoader.cs`,
  `LevelDefinition.cs`, `LevelConfiguration.cs`, `RomPresentationMetadata.cs`,
  `AtariPresentationAdapter.cs`, `VectorRenderer.cs`, `Level.cs`, `Player.cs`,
  `Flipper.cs`, and `Tanker.cs`.
- Calculations performed: verified all five `lev_remap` associations;
  inventoried all 16 C# points and all 16 raw Atari point pairs per level;
  compared topology; tested rotations, reversal, signs, fitted translations,
  uniform scale, and independent axis scales; recorded residual maxima and
  Euclidean RMS. No script, chart, image, or visual artifact was created.
- Validation: no build or test suite was required. No game launch, gameplay,
  recording playback, visual-content inspection, Internet search, source
  edit, or manual acceptance occurred. The required Git status inventory
  returned worktree path names; visual-asset names are intentionally not
  repeated here under the no-enumeration constraint.
- Starting Git state: branch `master`; HEAD
  `385f603a94391756b2ca6cccd524dc17b281e782`; upstream `origin/master`;
  ahead/behind `0/0`; index empty. Existing tracked modifications were
  `src/Tempest.Game/Debug/DebugController.cs`,
  `src/Tempest.Game/Debug/PlaybackScreenshotCapture.cs`,
  `src/Tempest.Game/Levels/LevelLoader.cs`,
  `src/Tempest.Game/Program.cs`,
  `src/Tempest.Game/Rendering/DisplayConfiguration.cs`,
  `src/Tempest.Game/Rendering/Projection.cs`,
  `src/Tempest.Game/Rendering/VectorRenderer.cs`,
  `src/Tempest.Game/Systems/InputSystem.cs`,
  `tests/Tempest.Game.Tests/DebugControllerTests.cs`,
  `tests/Tempest.Game.Tests/DisplayConfigurationTests.cs`,
  `tests/Tempest.Game.Tests/GameOverDensityDiagnosticTests.cs`,
  `tests/Tempest.Game.Tests/GameplayRecordingTests.cs`,
  `tests/Tempest.Game.Tests/InputSystemTests.cs`,
  `tests/Tempest.Game.Tests/Level9ProductionTests.cs`,
  `tests/Tempest.Game.Tests/PlaybackScreenshotCaptureTests.cs`, and
  `tests/Tempest.Game.Tests/TransitionPresentationTests.cs`. Existing
  untracked non-visual paths were `Reference/arcade/MBUCOD.V05.txt`,
  `Reference/arcade/MBUDOC.DOC`,
  `src/Tempest.Game/Levels/RomPresentationMetadata.cs`,
  `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`,
  `tests/Tempest.Game.Tests/AtariPresentationAdapterTests.cs`,
  `tests/Tempest.Game.Tests/RomPresentationMetadataTests.cs`, and
  `tests/Tempest.Game.Tests/VectorRendererAtariPresentationTests.cs`.
- Ending Git state: branch, HEAD, upstream, tracked modifications, pre-existing
  untracked paths, and empty index remained unchanged. No tracked file changed;
  no file was staged, committed, or pushed.
- Per-level classification/disposition: Level 2 — CONSISTENT APPROXIMATE
  FRAME MAPPING / READY WITH EXPLICIT APPROXIMATION; Level 3 — CONSISTENT
  APPROXIMATE FRAME MAPPING / READY WITH EXPLICIT APPROXIMATION; Level 4 —
  PARTIAL STRUCTURAL CORRESPONDENCE / REQUIRES SEPARATE
  GEOMETRY/PROVENANCE REVIEW; Level 5 — PARTIAL STRUCTURAL CORRESPONDENCE /
  READY WITH EXPLICIT APPROXIMATION; Level 6 — PARTIAL STRUCTURAL
  CORRESPONDENCE / REQUIRES SEPARATE GEOMETRY/PROVENANCE REVIEW.
- Most important decision-relevant unknown: whether the Level 4 and Level 6
  non-rounding differences reflect intended C# reference geometries or
  documented Atari-frame approximations; that determines whether later
  support can use the current adapter model without changing model geometry.
