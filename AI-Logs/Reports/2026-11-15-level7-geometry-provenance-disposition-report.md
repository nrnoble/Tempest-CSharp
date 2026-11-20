# Level 7 Geometry Provenance and Disposition

Date: 2026-11-15
Status: **READ-ONLY REVIEW COMPLETE**

## Executive conclusion

**ACCEPTED GAMEPLAY, GEOMETRY PROVENANCE UNCERTAIN**

Level 7 gameplay is manually accepted, but the reviewed project record does
not establish whether its current literal points were intended as an
approximation of Atari record 06, as independent project geometry, or as a
provisional shape. The durable documentation does identify the coordinates
as the exact Incitti Original Arcade clover points. It does not claim that
they are an exact Atari record-06 coordinate conversion.

The generic durable statement that Level 7 “coordinate mappings remain
documented C# approximations” is relevant, but it does not identify this point
array, its transform, its Atari source row, or its residuals. It is not enough
to classify the new record-06 mismatch as an explicitly accepted approximation.
Nor does it contradict the new result. No durable evidence authorizes changing
these points in the current presentation milestone. Preserve the current
geometry and point order for now; that is a scope restriction, not proof that
the geometry was formally frozen or that it is provisional.

## Current Level 7 geometry

**C#-SOURCE-BACKED:** Production configuration
`LevelConfiguration.Level7Production` selects
`LevelLoader.LoadLevel7Clover()`, is continuous, and starts on lane 7. The
loader stores 16 literal points, center Y 310 and offset -10; continuous
definitions expose all 16 vertices as playable lanes. The point sequence is:

```text
(-160,160), (-179,44), (-89,0), (-179,-44),
(-160,-160), (-44,-179), (0,-89), (44,-179),
(160,-160), (179,-44), (89,0), (179,44),
(160,160), (44,179), (0,89), (-44,179)
```

Sources: [LevelLoader.cs](../../src/Tempest.Game/Levels/LevelLoader.cs#L97),
[LevelConfiguration.cs](../../src/Tempest.Game/Levels/LevelConfiguration.cs#L306),
[LevelDefinition.cs](../../src/Tempest.Game/Levels/LevelDefinition.cs#L4).

**DURABLE-PROJECT-DECISION / REFERENCE-DERIVED:** The behavior specification
identifies this closed 16-lane clover and its source-ordered coordinates as
Incitti “Original Arcade” board findings. The earlier Level 7 preimplementation
analysis cites Incitti’s `levelset1.txt` and recommends using its exact clover
coordinates. The implementation prompt likewise supplies these values and
requires the exact source order. This establishes the engineering reference
for the literal values; it does not establish an Atari-ROM coordinate
conversion.

Sources: [TEMPEST-BEHAVIOR-SPEC.md](../../TEMPEST-BEHAVIOR-SPEC.md#L152),
[2026-11-04-level7-preimplementation-analysis-report.md](./2026-11-04-level7-preimplementation-analysis-report.md#L26),
[2026-11-04-level7-implementation-prompt.md](../Prompts/2026-11-04-level7-implementation-prompt.md#L94).

## Provenance history

1. **Earliest inspected project record — 2026-11-04 preimplementation
   analysis.** It calls the Original Arcade set’s board 7 the “Level 7 -
   clover,” records the 16 points in source order, and cites Incitti’s level
   set. Its recommendation is to use the exact Incitti clover coordinates.
   The report does not call those literals a ROM-coordinate conversion or
   characterize their residual from Atari record 06.
2. **Introduction to production source — commit `3bebf42` (2026-11-04).**
   The Level 7 implementation added the static `LoadLevel7Clover` array.
   The implementation report describes the source-ordered closed clover,
   without saying how its numeric values were converted from Atari data.
3. **Later history.** Subsequent `LevelLoader.cs` commits inspected here add
   Level 8 and Level 9 data; their diffs leave the Level 7 point list intact.
   No later geometry edit or explicit Level 7 geometry-freeze decision was
   found in the reviewed history or Level 7 closeouts.
4. **No exact-conversion claim found.** Neither the durable Level 7 source
   record nor the inspected implementation and acceptance reports claims
   exact point-for-point equivalence with Atari record 06. The durable record
   does contain the broader sentence “coordinate mappings remain documented
   C# approximations.” That statement is not scoped to this array and does
   not identify an intended record-06 fit. Its specific application here is
   **UNKNOWN**.

Sources: [preimplementation analysis](./2026-11-04-level7-preimplementation-analysis-report.md#L26),
[implementation report](./2026-11-04-level7-implementation-report.md#L14),
[Level 7 acceptance closeout](./2026-11-04-level7-acceptance-closeout-report.md#L17),
[behavior specification](../../TEMPEST-BEHAVIOR-SPEC.md#L236).

## Manual acceptance record

| Aspect | Evidence-supported acceptance |
|---|---|
| Tube shape/topology and point coordinates | The source and durable reference record specify a closed 16-point clover. The initial implementation report asked Neal to compare “clover geometry/closure,” but the final acceptance record does not separately say that he inspected or accepted the literal point coordinates, closure, or topology. Do not infer coordinate-level acceptance from the overall gameplay result. |
| Lane ordering and directed movement | Neal manually verified the visible below-rim `2-3-2-2-2` lane-change pattern. This accepts the visible route behavior, not a C#-to-Atari lane-number correspondence. |
| Ordinary Flippers and Tankers | Neal verified that individual Flippers do not enter from the bottom and that Tankers enter and release Flippers. |
| Deep Tanker cargo | Durable records report Neal’s observation of cargo siblings moving apart, reversing after two lane changes, then continuing at least three more. This is scoped observation, not acceptance of point-coordinate equivalence. |
| Spikers, speeds, and firing | The later closeout records manual acceptance of the visible Tanker/Spiker movement relationship and projectile speeds, and continued Spiker firing during later ascent cycles. The speed judgment is perceptual, not a numeric ROM measurement. |
| Death/re-entry | The exact one-enemy death/re-entry visual case was explicitly **not manually reproduced** and was deemed non-blocking. |
| Natural Level 6-to-7 transition | The inspected Level 7 closeout establishes automated progression coverage and describes the transition implementation; it does not record a distinct manual acceptance result for that exact natural handoff. |
| Overall arcade likeness | Neal judged the corrected Level 7 gameplay extremely difficult to distinguish from the original arcade. This is an overall perceptual acceptance, not proof of mathematical coordinate identity. |
| Screen-space presentation/perspective | No Level 7 point-specific screen-space or perspective acceptance is recorded in the inspected sources. No media or visual analysis was used in this review. |

Sources: [2026-11-04 acceptance closeout](./2026-11-04-level7-acceptance-closeout-report.md#L11),
[TEMPEST-BEHAVIOR-SPEC.md](../../TEMPEST-BEHAVIOR-SPEC.md#L203),
[TEMPEST-DEVELOPMENT-NOTES.md](../../TEMPEST-DEVELOPMENT-NOTES.md#L3098).

## Gameplay/model dependencies

**C#-SOURCE-BACKED:** Lane identity is index-based. `PlayableLaneCount` is 16
for this continuous definition. `Level.TryGetAdjacentLane` advances or
decrements the lane index and wraps at the closed boundary. The player’s rim
movement follows those lane relationships.

**C#-SOURCE-BACKED:** The below-rim Flipper route selects adjacent lane
indices, then `CalculateDirectedLaneTransition` uses the indexed previous,
hinge, and next points to determine winding, hinge angle, sweep, and pivot
endpoints. Its inward pivot is clipped against the polygon boundary. Thus
both point order and point coordinates participate in accepted Flipper
movement; reordering points would change lane associations and directed
hinges, while changing coordinates can change turn geometry.

**C#-SOURCE-BACKED:** Tanker cargo source/destination lanes are selected
through the same adjacent-lane topology. Cargo Flippers then use the
directed-transition geometry above.

**C#-SOURCE-BACKED:** Rim traversal uses lane adjacency and the geometry-based
lane-transition angle. Spike/player contact checks compare `EdgeIndex`; the
collision predicate does not use a raw point-coordinate distance. Reordering
points can nevertheless change which physical segment a lane index denotes.
The renderer also consumes the same ordered points for tube and entity
placement.

Sources: [LevelDefinition.cs](../../src/Tempest.Game/Levels/LevelDefinition.cs#L4),
[Level.cs](../../src/Tempest.Game/Levels/Level.cs#L429),
[Level.cs](../../src/Tempest.Game/Levels/Level.cs#L439),
[Level.cs](../../src/Tempest.Game/Levels/Level.cs#L526),
[Level.cs](../../src/Tempest.Game/Levels/Level.cs#L634),
[Level.cs](../../src/Tempest.Game/Levels/Level.cs#L711),
[Player.cs](../../src/Tempest.Game/Gameplay/Player.cs#L65),
[Tanker.cs](../../src/Tempest.Game/Gameplay/Tanker.cs#L285),
[TempestGame.cs](../../src/Tempest.Game/TempestGame.cs#L2450).

## Reconciliation with Atari record 06

**NEW-COMPARISON-RESULT:** The controlling coordinate-frame report maps
displayed Level 7 to Atari record 06 and finds matching closed topology, but
classifies the point relation only as **PARTIAL STRUCTURAL CORRESPONDENCE**.
Its best simple fit requires a +5 cyclic comparison alignment and a different
scale; residuals exceed rounding. The report identifies the C# points as
static project-authored values and finds no evidence that the fit was intended
as a ROM-derived approximation.

- The new result does **not** contradict a previously found explicit claim of
  exact record-06 conversion: no such claim was found.
- The original implementation history instead says to use the exact Incitti
  clover points. That does not prove that the clover was intentionally
  independent of Atari or deliberately approximate to Atari.
- The +5 correspondence is a numerical comparison alignment only. No
  inspected history establishes that C# lane numbering was intentionally
  rotated relative to Atari.
- The durable phrase “coordinate mappings remain documented C#
  approximations” acknowledges approximations in general, but does not name
  these points or residuals. Whether it was intended to cover this specific
  array is **UNKNOWN**; the point residuals cannot be called an explicitly
  acknowledged approximation on that wording alone.
- The record-06 mismatch therefore changes the numerical understanding, not
  any prior point-specific acceptance. It does not itself authorize changing
  model geometry.

Source: [2026-11-15-atari-csharp-coordinate-frame-mapping-report.md](./2026-11-15-atari-csharp-coordinate-frame-mapping-report.md#L89).

## Required conclusions

1. **Documented provenance:** Exact Incitti Original Arcade clover coordinates
   copied as static C# points; no more specific intent for their relation to
   Atari record 06 is documented.
2. **Exact record-06 equivalence ever claimed?** No explicit prior claim was
   found.
3. **Manually accepted geometry/topology:** No standalone coordinate-array,
   closure, or topology acceptance is recorded. Neal accepted the visible
   `2-3-2-2-2` movement and overall gameplay likeness while using this
   production candidate.
4. **Gameplay dependencies:** Lane identity/order, closed wraparound,
   rim-transition angles, below-rim Flipper pivots, and Tanker cargo lane
   routing depend on the current indexed topology; Flipper pivot geometry and
   presentation also use the actual point coordinates. Lane-based collisions
   use lane indices rather than raw point distances.
5. **Does mismatch invalidate prior acceptance?** No. It disproves neither
   the recorded gameplay acceptance nor a specific coordinate claim, because
   no point-specific equivalence claim or acceptance was recorded.
6. **Authorized to change during current presentation milestone?** No. The
   review task authorizes provenance investigation only; the geometry is
   gameplay/model input, and no durable project decision authorizes changing
   it in this presentation work.
7. **Future presentation assumption:** Preserve current Level 7 points and
   their order for now. This is the selected uncertainty disposition, not a
   claim that the point geometry is formally frozen or that it is proven
   provisional.

## Evidence classifications

- **DURABLE-PROJECT-DECISION:** The behavior specification records the
  Incitti-derived closed clover and source-order coordinates; it also records
  the broad, unscoped C# coordinate-mapping approximation statement. Generic
  project instructions prohibit speculative gameplay changes and reserve
  manual acceptance authority, but do not freeze or reopen this geometry.
- **HISTORICAL-REPORT-BACKED:** 2026-11-04 analysis and implementation records
  identify the Incitti clover and introduce the literal array; the final
  acceptance closeout records the specific gameplay observations and overall
  judgment. The 2026-11-11 closeout adds accepted speed/firing observations.
- **C#-SOURCE-BACKED:** Current production source stores 16 ordered literal
  points in a continuous Level 7 configuration. Lane topology, Flipper pivot
  geometry, rim transitions, cargo selection, and lane-based collision
  dependencies are described above.
- **MANUALLY-ACCEPTED:** Specific observations and the perceptual overall
  gameplay judgment listed above. No point-by-point equivalence was claimed.
- **NEW-COMPARISON-RESULT:** Record 06 is the mapped Atari row; the best fit
  has cyclic offset +5, a different scale, and non-rounding residuals.
- **STRONG INFERENCE:** Because accepted production gameplay uses these
  points and systems depend on their ordering, changing them without a
  separately authorized milestone risks changing accepted behavior.
- **UNKNOWN:** Whether the broad durable “coordinate mappings” approximation
  statement specifically refers to this point array; whether its residuals
  were consciously accepted; whether the geometry was deliberately
  independent, Atari-approximate, or expected to be replaced; and whether
  Neal separately accepted its literal coordinates, closure, or screen-space
  presentation.

## Smallest next milestone

Continue the existing presentation milestone while preserving Level 7’s
current points and order. Reopen their provenance or geometry only in a
separately authorized geometry milestone.

## Scope and completion

- Prompt: [2026-11-15-level7-geometry-provenance-disposition-prompt.md](../Prompts/2026-11-15-level7-geometry-provenance-disposition-prompt.md).
- Report: this file.
- Files created: exactly the two authorized ignored AI-Logs artifacts above.
- Durable project files inspected: `.github/copilot-instructions.md`,
  `TEMPEST-BEHAVIOR-SPEC.md`, and `TEMPEST-DEVELOPMENT-NOTES.md`.
- Level 7 historical reports inspected: 2026-11-04 preimplementation,
  implementation, arcade-corrections implementation, and acceptance closeout;
  2026-11-11 complete accepted closeout; and the controlling 2026-11-15
  coordinate-frame mapping report. The 2026-11-04 implementation prompt was
  also inspected.
- Source files inspected: `src/Tempest.Game/Levels/LevelLoader.cs`,
  `LevelConfiguration.cs`, `LevelDefinition.cs`, `Level.cs`,
  `Gameplay/Player.cs`, `Gameplay/Flipper.cs`, `Gameplay/Tanker.cs`, and
  `TempestGame.cs`.
- Starting Git state: branch `master`, HEAD
  `385f603a94391756b2ca6cccd524dc17b281e782`, tracking `origin/master`;
  index empty. Pre-existing tracked modifications:
  `src/Tempest.Game/Debug/DebugController.cs`,
  `src/Tempest.Game/Debug/PlaybackScreenshotCapture.cs`,
  `src/Tempest.Game/Levels/LevelLoader.cs`,
  `src/Tempest.Game/Program.cs`,
  `src/Tempest.Game/Rendering/DisplayConfiguration.cs`,
  `src/Tempest.Game/Rendering/Projection.cs`,
  `src/Tempest.Game/Rendering/VectorRenderer.cs`,
  `src/Tempest.Game/Systems/InputSystem.cs`,
  `tests/Tempest.Game.Tests/DebugControllerTests.cs`,
  `DisplayConfigurationTests.cs`, `GameOverDensityDiagnosticTests.cs`,
  `GameplayRecordingTests.cs`, `InputSystemTests.cs`,
  `Level9ProductionTests.cs`, `PlaybackScreenshotCaptureTests.cs`, and
  `TransitionPresentationTests.cs`. Pre-existing untracked paths:
  `Reference/arcade/MBUCOD.V05.txt`, `Reference/arcade/MBUDOC.DOC`,
  `src/Tempest.Game/Levels/RomPresentationMetadata.cs`, and
  `tests/Tempest.Game.Tests/RomPresentationMetadataTests.cs`.
- Ending Git state: branch `master`, HEAD
  `385f603a94391756b2ca6cccd524dc17b281e782`, tracking `origin/master`;
  the tracked modifications and four untracked paths exactly match the
  starting inventory above, and the index remains empty. No tracked file
  changed, nothing was staged, committed, or pushed, and both new artifacts
  are ignored.
- No build or test was required or run. No game, media, ROM/PROM, staging,
  commit, or push was used.
- Selected disposition: **ACCEPTED GAMEPLAY, GEOMETRY PROVENANCE UNCERTAIN**.
- Most important remaining unknown: whether the durable approximation label
  specifically covers the current Level 7 point array.
