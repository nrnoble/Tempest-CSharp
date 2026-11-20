# Atari-to-C# Coordinate-Frame Mapping

## Executive conclusion

**MIXED.** The current Level 8 and 9 point arrays are exact source-derived conversions of Atari records 07 and 0d, and Level 1 has a same-order, common-scale correspondence with at most one C# coordinate unit of rounding. Level 7 has a bounded approximate numerical fit to record 06 only after a five-index cyclic rotation and a materially different scale; residuals remain larger than rounding. Thus one uniform exact frame mapping across all four production geometries is not established.

This is a geometry-coordinate result, not a projection-equivalence result. Atari's selected `lev_y3d` reference and Y2D offset are not the same fields as C# `CenterY` and `YOffset`; no equivalence between those placement/projection terms is asserted. No transform or implementation is proposed.

## Evidence and source locations

- **ATARI-BACKED:** Local ROM-derived disassembly table declarations and consumers: `Reference/arcade/Tempest Rom Source code.md:7461-7480,8202-8223,8244-8263,8287-8308,8551-8558`.
- **ATARI-BACKED:** Recovered raw X/Y rows and parameter bytes: `AI-Logs/Reports/2026-11-15-atari-projection-data-mathbox-investigation-report.md:17-85`. This report is the source of the numeric table values used here; raw ROM/PROM files were not reopened.
- **ATARI-BACKED / C#-SOURCE-BACKED:** Selected records, parameter values, and prior mapping conclusions: `AI-Logs/Reports/2026-11-15-atari-csharp-projection-comparison-report.md:219-242`.
- **C#-SOURCE-BACKED:** Production level selection: `src/Tempest.Game/Levels/LevelConfiguration.cs:134-142,306-314,337-345,368-376`.
- **C#-SOURCE-BACKED:** Point definitions and conversions: `src/Tempest.Game/Levels/LevelLoader.cs:7-41,43-50,97-144`; topology fields: `src/Tempest.Game/Levels/LevelDefinition.cs:4-9`.
- **C#-SOURCE-BACKED:** Projection receives `CenterY` and `YOffset` separately from point coordinates: `src/Tempest.Game/Rendering/VectorRenderer.cs:185-193`; projection formula: `src/Tempest.Game/Rendering/Projection.cs:40-96`.
- **NUMERICALLY DEMONSTRATED:** Inline bounded calculation over the four selected 16-point rows; enumerated cyclic rotations, forward/reversed order, X/Y signs, and uniform/independent nonnegative scales with translation fitted. No script file was created.

## Level-to-record verification

The local disassembly declares `lev_remap` as a 16-entry table indexed by level (`Tempest Rom Source code.md:7461-7480`). `p1_level` is passed to `get_tube_no` at `$c235-$c239`; that routine indexes `lev_remap` at `$c300-$c303` (`Tempest Rom Source code.md:8202-8205,8287-8308`). The recovered raw table is:

```text
level index: 00 01 02 03 04 05 06 07 08 09 0a 0b 0c 0d 0e 0f
record:      00 01 02 03 04 05 06 07 0d 09 08 0c 0e 0f 0a 0b
```

The four C# production configurations have progression indices 0, 6, 7, and 8 for displayed Levels 1, 7, 8, and 9 respectively (`LevelConfiguration.cs` locations above). Applying `lev_remap` verifies **Level 1 → 00, Level 7 → 06, Level 8 → 07, Level 9 → 0d**. This is consistent with the controlling handoff.

## C# geometry inventory

`LevelDefinition` stores `Continuous`, `CenterY`, `YOffset`, and an ordered point list. The open form has one fewer playable lane than vertices; the continuous form includes the closing edge (`LevelDefinition.cs:4-9`).

| Displayed level | Definition / count / order | Topology | CenterY / YOffset | Provenance and conversion |
|---|---|---|---|---|
| 1 | `LoadLevel1`, 16 literal integer points; listed below in source order (`LevelLoader.cs:43-50`) | `Continuous=true` | 400 / -80 | Project-authored static geometry (`"Level 1 test wave"`); no ROM conversion or normalization at construction. |
| 7 | `LoadLevel7Clover`, 16 literal integer points; listed below in source order (`LevelLoader.cs:97-104`) | `Continuous=true` | 310 / -10 | Project-authored static geometry (`"Level 7 - clover"`); no ROM conversion or normalization at construction. |
| 8 | `LoadLevel8OpenTube`, 16 points, same index order as its raw arrays (`LevelLoader.cs:106-131`) | `Continuous=false` | 300 / 0 | Raw arrays match record 07; construction applies `(X-128)*(25/14)` and `-(Y-128)*(25/14)`. This is a C# point conversion, not the Atari projection. |
| 9 | `LoadLevel9OpenTube`, 16 points, same index order as `Level9RomEvidence.X/Y` (`LevelLoader.cs:7-41,132-144`) | `Continuous=false` | 300 / 0 | Raw arrays match record 0d; construction applies `(X-128)*(25/14)` and `-(Y-128)*(25/14)`. This is a C# point conversion, not the Atari projection. |

Level 1 points, indices 00-0f:

```text
(200,0), (184,76), (141,141), (76,184),
(0,200), (-76,184), (-141,141), (-184,76),
(-200,0), (-184,-76), (-141,-141), (-76,-184),
(0,-200), (76,-184), (141,-141), (184,-76)
```

Level 7 points, indices 00-0f:

```text
(-160,160), (-179,44), (-89,0), (-179,-44),
(-160,-160), (-44,-179), (0,-89), (44,-179),
(160,-160), (179,-44), (89,0), (179,44),
(160,160), (44,179), (0,89), (-44,179)
```

For Levels 8/9, the formula and exact raw rows in the next section reconstruct every current `Vector2` value. No additional geometry normalization is found on the inspected path. `VectorRenderer` supplies the point coordinates and passes `CenterY`/`YOffset` separately to `Projection.Project`; the projection applies its own depth scaling and center/offset formula. These fields are not folded into the per-point coordinate comparison.

## Atari coordinate inventory

Raw values below are unsigned hex bytes `(X,Y)` in stored record order; they are not projected coordinates. Atari's normal X reference is `$80`. The selected Y reference is `lev_y3d`. Parameter values are from the recovered tables; `lev_y2d`/`lev_y2db` are shown as their signed 16-bit value. `lev_open=$00` is closed and `$ff` is open.

| Record | Normal X ref | `lev_y3d` | Signed Y2D | `lev_open` | Stored Atari point pairs, indices 00-0f |
|---|---:|---:|---:|---:|---|
| 00 | `$80` | `$50` | -192 | `$00` closed | `(f0,80) (e7,aa) (cf,cf) (aa,e7) (80,f0) (56,e7) (31,cf) (19,aa) (10,80) (19,56) (31,31) (56,19) (80,10) (aa,19) (cf,31) (e7,56)` |
| 06 | `$80` | `$68` | -144 | `$00` closed | `(ea,a0) (e0,e0) (9c,ea) (80,b0) (64,ea) (20,e0) (16,a0) (50,80) (16,60) (20,20) (64,16) (80,50) (9c,16) (e0,20) (ea,60) (b0,80)` |
| 07 | `$80` | `$b0` | +96 | `$ff` open | `(10,f0) (1e,d0) (2c,b0) (3a,90) (48,70) (56,50) (64,30) (70,10) (90,10) (9e,30) (ac,50) (ba,70) (c8,90) (d6,b0) (e4,d0) (f0,f0)` |
| 0d | `$80` | `$b0` | +320 | `$ff` open | `(10,90) (10,70) (30,70) (30,50) (50,50) (50,30) (70,30) (70,10) (90,10) (90,30) (b0,30) (b0,50) (d0,50) (d0,70) (f0,70) (f0,90)` |

The disassembly copies the selected stored coordinate bytes into `tube_x/y` while counting both source and destination indices downward from 15 to 0 (`Tempest Rom Source code.md:8244-8263`); the resulting tube arrays retain stored point-index order. `lev_open` also affects traversal (`:8551-8558`). Thus the comparison uses forward stored order, with only a candidate cyclic rotation/reversal where numerically tested. Atari Y3D references above are distinct from the `$80` centering used in candidate point-frame equations.

## Per-level mapping results

RMS is defined as `sqrt(mean(dx²+dy²))` over all 16 matched points. Residuals are current C# coordinates minus the candidate transformed Atari coordinates. Translation is fitted where testing general candidates; the selected simple formulas below have zero fitted translation. All findings are **NUMERICALLY DEMONSTRATED** unless explicitly marked source-backed.

### Level 1 / record 00

- **Classification: CONSISTENT APPROXIMATE FRAME MAPPING.**
- **Mapping:** For C# index `i`, use Atari point `i`; `Cx=(25/14)*(X-128)`, `Cy=(25/14)*(Y-128)`.
- **Scale/reflection/index:** uniform `25/14`; no X or Y reflection; forward order; no rotation/reversal.
- **Residual:** max absolute X = 1; max absolute Y = 1; Euclidean RMS = 0.710705312. The unconstrained least-squares uniform scale is 1.787470867 with max residual 0.926223580 on each axis; the source-neat `25/14` formula is within one integer C# unit.
- **Topology:** matches independently of coordinates: Atari closed, C# continuous.
- **Provenance:** Static project-authored values; no evidence in the definition that they were generated from record 00. The numerical match supports an approximate frame relation, not historical ROM provenance.
- **Evidence:** C# source-backed; Atari row Atari-backed; residual numerically demonstrated.

### Level 7 / record 06

- **Classification: PARTIAL STRUCTURAL CORRESPONDENCE.**
- **Best simple uniform mapping:** For C# index `i`, use Atari point `(i+5) mod 16`; `Cx=1.678958593*(X-128)`, `Cy=1.678958593*(Y-128)`.
- **Scale/reflection/index:** uniform fitted scale 1.678958593; no reflections; forward direction; cyclic rotation +5; no reversal; fitted translation is zero.
- **Residual:** max absolute X = 8.409987560; max absolute Y = 9.726674960; Euclidean RMS = 6.695326333. The independent-scale fit is only marginally better (X=1.684027157, Y=1.673943786; maxima 8.166696445 and 9.566201167; RMS 6.673930026). At the other levels' nominal `25/14` scale, the same index rotation has maxima 11.428571429 / 13.142857143 and RMS 13.156243183.
- **Topology:** matches independently of coordinates: Atari closed, C# continuous.
- **Provenance:** C# points are static project-authored values. The fit demonstrates a structural numerical relationship, but no source evidence establishes that this was intended as a ROM-derived approximation. Residuals exceed integer rounding, so it is not classified as an approximate conversion.
- **Evidence:** C# source-backed; Atari row Atari-backed; fit and residuals numerically demonstrated.

### Level 8 / record 07

- **Classification: EXACT FRAME MAPPING.**
- **Mapping:** For C# index `i`, use Atari point `i`; `Cx=(25/14)*(X-128)`, `Cy=-(25/14)*(Y-128)`.
- **Scale/reflection/index:** uniform `25/14`; X unchanged, Y reflected; forward order; no rotation/reversal.
- **Residual:** max absolute X = 0; max absolute Y = 0; Euclidean RMS = 0. The C# raw arrays equal record 07 and its constructor uses the stated expression directly, so this is source-traced identity apart from normal floating-point representation.
- **Topology:** matches independently of coordinates: Atari open, C# non-continuous/open.
- **Evidence:** C# source-backed, Atari row Atari-backed, and exact correspondence numerically demonstrated/source-traced.

### Level 9 / record 0d

- **Classification: EXACT FRAME MAPPING.**
- **Mapping:** For C# index `i`, use Atari point `i`; `Cx=(25/14)*(X-128)`, `Cy=-(25/14)*(Y-128)`.
- **Scale/reflection/index:** uniform `25/14`; X unchanged, Y reflected; forward order; no rotation/reversal.
- **Residual:** max absolute X = 0; max absolute Y = 0; Euclidean RMS = 0. `Level9RomEvidence.X/Y` equal record 0d and the constructor uses the stated expression directly, so this is source-traced identity apart from normal floating-point representation.
- **Topology:** matches independently of coordinates: Atari open, C# non-continuous/open.
- **Evidence:** C# source-backed, Atari row Atari-backed, and exact correspondence numerically demonstrated/source-traced.

## Cross-level consistency

1. **X centering/reference:** Yes. Each selected mapping centers raw X at `$80`/128; no fitted X translation is needed.
2. **Y orientation/reflection:** No. Levels 1 and 7 use positive centered Y in their best mappings; Levels 8 and 9 use reflected centered Y.
3. **Unit scale:** No. Levels 1, 8, and 9 use the nominal `25/14` scale (Level 1 with ≤1-unit point rounding); Level 7's best uniform fit is approximately 1.679 and retains larger residuals.
4. **Index direction/start:** Forward direction agrees. Levels 1, 8, and 9 start at stored point 00; Level 7's best correspondence requires a +5 cyclic rotation. No reversal is selected.
5. **Open/closed topology:** Yes. Records 00/06 and C# Levels 1/7 are closed/continuous; records 07/0d and C# Levels 8/9 are open/non-continuous.
6. **ROM-derived special status:** Levels 8/9 are special in source provenance: their raw arrays are explicitly converted from the corresponding ROM rows. Level 1 can be expressed numerically in the same centered `25/14` coordinate scale with positive Y, but its literal source points remain project-authored and differ by up to one unit. Level 7 has a numeric structural fit but needs a different scale, a cyclic start shift, and has non-rounding residuals; it cannot be claimed as a ROM conversion on this evidence.
7. **Presentation-frame conversion without changing model geometry/topology/lane identity:** A mathematical per-point coordinate relation exists for Levels 1, 8, and 9 (approximate for Level 1), and an approximate best fit exists for Level 7. A single exact, shared conversion for all four that preserves current point-index/lane associations is **not established**: Level 7 needs the +5 index correspondence and does not fit the shared scale within rounding. Applying any transform has not been implemented; this investigation does not authorize changing geometry or remapping gameplay lanes.

## Implication for projection work

**MIXED.** Levels 8/9 map exactly, and Level 1 maps approximately, but Level 7 does not share the same scale/start-index convention and retains measurable residuals under the allowed transformations. The results therefore do not support claiming one common Atari-frame parameterization for all four unchanged production geometries. This is not a finding that Level 7 must be changed; its intended relation remains unproven.

## Smallest next milestone

Perform one bounded, read-only Level 7 geometry-provenance/disposition review using only its durable project decision records and the existing record-06 comparison: establish whether the static clover is an accepted independent project geometry or an intended approximation, then record that disposition before any all-level presentation parameterization work. Do not inspect media or implement a transform in that follow-up.

## Scope and completion

- Prompt: `AI-Logs/Prompts/2026-11-15-atari-csharp-coordinate-frame-mapping-prompt.md`.
- Report: `AI-Logs/Reports/2026-11-15-atari-csharp-coordinate-frame-mapping-report.md`.
- Files created: exactly those two ignored AI-Logs artifacts. No source, test, durable documentation, configuration, Atari reference, ROM/PROM, or other existing project content was modified.
- Source files inspected: `Reference/arcade/Tempest Rom Source code.md`; `src/Tempest.Game/Levels/LevelConfiguration.cs`; `src/Tempest.Game/Levels/LevelLoader.cs`; `src/Tempest.Game/Levels/LevelDefinition.cs`; `src/Tempest.Game/Rendering/VectorRenderer.cs`; `src/Tempest.Game/Rendering/Projection.cs`.
- Reports consulted: the controlling projection-comparison report and the recovered Atari data/Math Box handoff report. No new Math Box analysis was performed; raw ROM/PROM files, screenshots, frames, and videos were not inspected.
- Calculations performed: all 16 points for each selected row; cyclic rotations and reversals; both axis signs; centered translations; uniform and independent scale fits; selected formula residuals; topology comparison.
- Validation: no build or test was required by scope. No game launch occurred.
- Starting Git state: branch `master`, HEAD `385f603a94391756b2ca6cccd524dc17b281e782`, tracking `origin/master`; staged/index diff empty. Pre-existing tracked modifications: `src/Tempest.Game/Debug/DebugController.cs`, `src/Tempest.Game/Debug/PlaybackScreenshotCapture.cs`, `src/Tempest.Game/Levels/LevelLoader.cs`, `src/Tempest.Game/Program.cs`, `src/Tempest.Game/Rendering/DisplayConfiguration.cs`, `src/Tempest.Game/Rendering/Projection.cs`, `src/Tempest.Game/Rendering/VectorRenderer.cs`, `src/Tempest.Game/Systems/InputSystem.cs`, `tests/Tempest.Game.Tests/DebugControllerTests.cs`, `tests/Tempest.Game.Tests/DisplayConfigurationTests.cs`, `tests/Tempest.Game.Tests/GameOverDensityDiagnosticTests.cs`, `tests/Tempest.Game.Tests/GameplayRecordingTests.cs`, `tests/Tempest.Game.Tests/InputSystemTests.cs`, `tests/Tempest.Game.Tests/Level9ProductionTests.cs`, `tests/Tempest.Game.Tests/PlaybackScreenshotCaptureTests.cs`, and `tests/Tempest.Game.Tests/TransitionPresentationTests.cs`. Pre-existing untracked paths: `Reference/arcade/MBUCOD.V05.txt`, `Reference/arcade/MBUDOC.DOC`, `src/Tempest.Game/Levels/RomPresentationMetadata.cs`, and `tests/Tempest.Game.Tests/RomPresentationMetadataTests.cs`.
- Ending Git state: same branch, HEAD, tracked modifications, untracked paths, and empty index; the two new artifacts are ignored. No tracked files were changed, the index remained unchanged, and nothing was staged, committed, or pushed.
- Most important mapping result: Levels 8/9 are exact source conversions; Level 7 is the outlier under common-frame testing.
- Most important remaining unknown: the intended provenance/acceptance of the Level 7 static clover geometry and whether its numerical residual is an intentional approximation.
