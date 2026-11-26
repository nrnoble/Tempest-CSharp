# Level 19 Shape 3 Effective-Presentation Correction

Date: 2026-11-26

## 1. Executive summary

**PASS — bounded presentation correction complete.** Displayed Level 19 was
resolving canonical Shape 3 metadata and calibration, but the adapter's
projection branch was keyed by displayed Level 3. Level 19 consequently entered
the generic reciprocal projection path instead of the accepted Shape 3
production composition. The branch is now keyed by canonical ROM shape row 2,
preserving Level 3 and making repeated Shape 3 use the same projection.

## 2. Starting Git/worktree state

- Branch: `master`
- HEAD: `9a8f788a41e42ea578fb1dcad90dd74a9a3cc60b`
- Upstream: `origin/master`
- Ahead/behind: `0/0`
- Existing tracked modifications were the bounded Level 19 implementation and
  related test/startup/documentation changes listed in the implementation
  report.
- Existing untracked items included the prior Level 19 prompt/report files,
  `Reference/arcade/MBUCOD.V05.txt`, `Level19WaveSelection.cs`, and
  `Level19ProductionTests.cs`.
- Protected `TEMP/`, `UserData/`, `Videos/`, ROM/reference material, and
  gameplay recordings were preserved.

## 3. Manual defect evidence

Neal reports that Level 19 otherwise plays correctly, Shape 3 remains
recognizable, and only the vertical screen placement is clearly wrong relative
to accepted Level 3 production placement.

## 4. Prior implementation claim

The prior report claimed that Level 19 reused the Level 3 model frame,
projection, and calibration. Structural reuse was present, but effective
screen-space equivalence was not tested.

## 5. Accepted Level 3 Shape 3 authority

- Canonical shape/row: Shape 3 / ROM tube-table row 2
- Model-frame mapping: canonical 1.756696429 X units/Atari unit, Y orientation
  +1
- Level 3 calibration: Tube Y gain 1.2, tube scale 0.97, vertical offset 35
- Projection: established C# depth progression, not the reciprocal branch
- Vertical composition: `Level3VerticalPlacement = 114.29437`
- Metadata: scale raw `0x18`, camera height raw `0x50`, signed Y2D `-192`,
  closed

Level 3 remains the reference owner; no Level 3 numerical values were changed.

## 6. Level 3 end-to-end presentation trace

Level 3 resolves to shape row 2, canonical model frame, Level 3 calibration,
the C# reciprocal-denominator depth progression, projected screen X/Y, and
the +114.29437 screen-Y placement, followed by the existing viewport transform.
Representative tube vertices and entity/lane points share this projector.

## 7. Level 19 end-to-end presentation trace

Level 19 resolves shape row 2, canonical shape/model-frame metadata, and the
Level 3 calibration. Before the correction, the adapter received displayed
level 19 and bypassed the `displayedLevel == 3` branch, then entered generic
reciprocal projection. It therefore diverged before final viewport translation.

## 8. First numeric divergence

| Stage | Level 3 | Level 19 | Equal? |
|---|---|---|---|
| Shape row | 2 | 2 | Yes |
| Model frame | 1.756696429 / +1 | canonical 1.756696429 / +1 | Yes |
| Calibration | 1.2 / .97 / 35 | resolved from shape owner | Yes |
| Metadata table index | 2 | 2 | Yes |
| Projection branch | Shape 3 C# branch | generic reciprocal branch | No |
| Numeric screen Y | accepted branch | generic reciprocal Y | No |
| Viewport transform | same portrait transform | same portrait transform | X/Y translates after divergence |

The first effective divergence is the projection-branch condition, not model
geometry, metadata, calibration, or viewport translation.

## 9. Legacy/generic-fallback audit

Level 19 satisfied the generic reciprocal fallback because it did not satisfy
the old displayed-level 3 condition. It did not enter the legacy 4:3, baseline
portrait, neutral calibration, unavailable-shape, or unmapped-level fallback.
The Level 19 discrepancy was a shape-owner dispatch failure.

## 10. Displayed-level-keying audit

The only relevant production presentation check found by the bounded audit was
`displayedLevel == 3` in `AtariPresentationAdapter.Project`. It is a
shape-specific projection branch and was incorrectly keyed by displayed level.
Palette/HUD/displayed-level logic remains correctly keyed by displayed level.

## 11. Level 2/18 successful-reuse comparison

Level 2 and 18 resolve canonical Shape 2. Their branch is already keyed by
`ResolveShapeRow(displayedLevel) == 1`, so Level 18 correctly enters the same
Shape 2 branch. Level 1/17 similarly use the shape-owner branch in the adapter.

## 12. Why Level 18 worked

Shape 2 reuse worked because the existing projection dispatch used canonical
shape row 1, not displayed level 2.

## 13. Why Level 19 failed

Shape 3 alone used a direct `displayedLevel == 3` condition. Level 19's canonical
shape row was correct, but that branch did not recognize it.

## 14. Existing Level 19 test analysis

`PresentationAndPaletteResolveThroughCanonicalShapeThree` proved that Level 19
shared the Level 3 definition, shape-row metadata, and calibration.

## 15. Exact test gap

The existing test was a structural-resolution test. It did not compare
Level 3 and Level 19 adapter outputs, branch selection, or final screen-space
coordinates.

## 16. Root cause

The root cause was a shape-specific projection dispatch bug:
`displayedLevel == 3` was used where canonical shape row 2 was intended.

## 17. Production correction

`AtariPresentationAdapter.Project` now uses
`RomTubeMappingCatalog.ResolveShapeRow(displayedLevel) == 2` for the accepted
canonical Shape 3 composition.

No Level 19 calibration, vertical offset, geometry, or gameplay branch was
added.

## 18. Why the correction is shape-generic

It applies through canonical ROM shape identity to the first cycle and any
supported repeated owner of row 2. It does not contain Level 19-specific
coordinates or a Level 19-only branch.

## 19. Level 3 numerical preservation

Level 3 still resolves and executes the same branch and constants. The focused
adapter suite passed with the same values.

## 20. Level 19 output equivalence

The new theory test compares tube and lane points at near, middle, and far
depths, and representative entity/lane points. Level 3 and 19 final logical
and portrait-camera host coordinates are equal within 0.001/0.01 numeric
tolerance.

## 21. Shape 1 repeat equivalence result

**PASS:** Level 1 / Level 17 share canonical Shape 1, calibration, model
geometry, projection, logical coordinates, and host-camera coordinates.

## 22. Shape 2 repeat equivalence result

**PASS:** Level 2 / Level 18 share canonical Shape 2, calibration, model
geometry, projection, logical coordinates, and host-camera coordinates.

## 23. Shape 3 repeat equivalence result

**PASS:** Level 3 / Level 19 share canonical Shape 3, calibration, model
geometry, projection, logical coordinates, and host-camera coordinates.

## 24. Entity-registration result

The new repeated-shape test registers representative lane midpoint points over
near, middle, and far entity depths through the same adapter. Tube vertices,
lanes, and entity/lane registrations remain shared. Existing renderer entity
projection coverage remained green.

## 25. Gameplay-freeze result

No gameplay source changed during this correction. The full suite and Level 19
configuration/selector/pulsar/cargo tests passed.

## 26. Level 19 focused gameplay result

The Level 19 production tests passed for identity, shape/palette, 22-supply
envelope, eligibility, cargo, `$19`/EightAdvanceTurn reuse, `$A9` translation,
Pulsar threshold/speed, firing, progression, and terminal boundary.

## 27. Prior presentation regressions

**PASS:** 174 focused presentation and repeated-level tests passed across
Levels 17–19, adapter/calibration/metadata, VectorRenderer, and Level 16.

## 28. Levels 1–16 recording result

**PASS:** unmodified `TEMP/GameplayRecordings/lvl1-16.json`

- achieved boundary: 35,350
- remaining frames: 0
- completion reason: `RecordingCompleted`
- final displayed level: 17
- level state: `Playing`
- score: 134,039
- lives: 9

## 29. Level 17-origin recording result

**PASS:** unmodified `TEMP/GameplayRecordings/lvl-17-test.json`

- achieved boundary: 2,609
- remaining frames: 0
- completion reason: `RecordingCompleted`
- final displayed level: 18
- level state: `PlayerDying`
- score: 8,175
- lives: 8
- accepted actor telemetry remained unchanged

## 30. Full-suite result

**PASS:** 2,113 tests passed, 0 failed, 0 skipped.

## 31. Release-build result

**PASS:** fresh non-incremental Release build succeeded with 0 warnings and
0 errors.

## 32. `git diff --check`

**PASS:** no whitespace errors.

## 33. Files changed

Production:
- `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`

Tests:
- `tests/Tempest.Game.Tests/Level19ProductionTests.cs`

AI-Logs:
- `AI-Logs/Prompts/2026-11-26-level19-shape3-effective-presentation-correction-prompt.md`
- `AI-Logs/Reports/2026-11-26-level19-shape3-effective-presentation-correction-report.md`

## 34. Tests added/changed

Added a theory covering effective 1/17, 2/18, and 3/19 repeated-shape
equivalence, including model points, representative entity/lane points,
logical screen coordinates, and portrait-camera host coordinates.

## 35. Manual commands/checklist

```text
dotnet run --project src\Tempest.Game -- --start-level 19 --lives 10
dotnet run --project src\Tempest.Game -- --start-level 19 --lives 10 --background-opacity .5
dotnet run --project src\Tempest.Game -- --start-level 3 --lives 10
```

Observe Level 19 matching accepted Level 3 Shape 3 vertical composition,
unchanged Shape 3 size/perspective, co-registered entities, cycle-1 palette,
HUD correctness, unchanged gameplay, and no obvious prior-shape regression.

## 36. Manual acceptance status

Pending. **Automated correction is complete. Manual Level 19 presentation
acceptance remains pending.**

## 37. Remaining uncertainties

- None established for the presentation root cause or bounded correction.
- Final Level 19 vertical placement remains subject to Neal's manual acceptance.

## 38. Final Git/worktree state

Tracked/untracked implementation work remains present. No index entries were
created for this correction. Existing pre-task work and protected
`Reference/arcade/MBUCOD.V05.txt` remain untouched.

## 39. Stage/commit/push

None.

## 40. Level 20+ untouched

No Level 20 or later implementation was added or changed.

## 41. Recommended next step

Run the listed Level 19 and Level 3 commands for direct manual A/B acceptance.
