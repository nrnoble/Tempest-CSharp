# Level 11 Spinner and Atari Mapping Fix Report

Date: 2026-11-21

## Scope

This correction continues the uncommitted genuine Level 11 production candidate.
It addresses the legacy spinner-direction reversal and the missing Atari
model-frame mapping for displayed Level 11 without changing gameplay supply or
presentation calibration.

## Code changes

- [Level.cs](../../src/Tempest.Game/Levels/Level.cs)
  - Replaced the Level 11 collinear neutral winding fallback from `+1` to `-1`.
  - Added an explicit PROJECT TRANSLATION comment explaining that the horizontal
    row-8 lane order maps clockwise physical spinner movement to logical left
    movement.
  - Row-8 ordered points, Y values, open topology, and 15-lane geometry remain
    unchanged.
- [AtariPresentationAdapter.cs](../../src/Tempest.Game/Rendering/AtariPresentationAdapter.cs)
  - Added displayed Level 11 to `HasModelFrameMapping`.
  - Added Level 11 to the established open-tube model-frame lookup using
    `25/14` units per raw unit and Y orientation `-1`.
  - Preserved all existing Levels 1-10 and Level 15 mappings and the fail-closed
    exception for unsupported levels.
- [Level8ProductionTests.cs](../../tests/Tempest.Game.Tests/Level8ProductionTests.cs)
  - Added semantic legacy Level 11 clockwise-left and counterclockwise-right
    coverage.
  - Added explicit collinear-fallback, lane-range, open-endpoint, and neutral
    winding assertions.
- [AtariPresentationAdapterTests.cs](../../tests/Tempest.Game.Tests/AtariPresentationAdapterTests.cs)
  - Added Level 11 runtime model-frame and world-point projection coverage.
  - Added preservation checks for Level 10, Level 15, Levels 1-10, and
    unsupported levels.

## Evidence classification

- Legacy Level 11 reversal: ARCADE-OBSERVED by Neal's manual test as a
  counterclockwise/clockwise physical-spinner result on legacy presentation.
- Collinear row-8 orientation translation: PROJECT TRANSLATION.
- Level 11 model-frame mapping: PROJECT TRANSLATION based on the genuine row-8
  geometry and the existing open-tube normalization; no new arcade visual
  calibration is claimed.
- Level 11 neutral calibration remains `1 / 1 / 0`.

## Validation

- Focused Level 11 and Atari presentation tests: PASS, 81 tests.
- Relevant input, Level 11/15, cargo/Fuseball, presentation and HUD regressions:
  PASS, 368 tests.
- Complete solution test suite: PASS, 1795 tests.
- Release solution build: PASS, 0 warnings and 0 errors.
- `git diff --check`: PASS.

No interactive game process was launched.
No files were staged or committed.

## Remaining manual acceptance gates

### Gate 1: legacy Level 11 spinner

```powershell
dotnet run --project src\Tempest.Game -- --level 11
```

Neal verifies clockwise -> left, counterclockwise -> right, normal previously
accepted spinner feel, and no crash recurrence.

### Gate 2: Atari-presentation crash

```powershell
dotnet run --project src\Tempest.Game -- --atari-presentation --level 11
```

Neal verifies that the tube renders, the game continues beyond the prior failure,
and then separately reports physical-spinner direction without pre-classification.

### Gate 3: calibration readiness

```powershell
dotnet run --project src\Tempest.Game -- --atari-presentation --level 11 --calibration-guides
```

This only establishes that Level 11 is ready for later presentation calibration.
No final Level 11 visual acceptance is requested in this milestone.

Completion token: `TEMPEST_LEVEL11_LEGACY_SPINNER_AND_ATARI_MAPPING_FIX`
