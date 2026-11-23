# Level 12 ExitZoom geometry-derived correction

Date: 2026-11-23

## Status

**IMPLEMENTED CANDIDATE — automated validation PASS; Neal's manual visual
acceptance remains pending.** No interactive game was launched, and no
Level 12 ExitZoom acceptance or durable behavior documentation was recorded.

The paired prompt is
[`2026-11-23-level12-exitzoom-geometry-derived-correction-prompt.md`](../Prompts/2026-11-23-level12-exitzoom-geometry-derived-correction-prompt.md).

## Implementation

Added a Level-12-only camera strategy, used only while Atari presentation is
rendering a live `ExitZoom` sample (not during death/re-entry). Its source
deep-end direction is the arithmetic center of the mapped vertices of Level
12's genuine closed far rim. Its current
Blaster base is mapped from the active lane, edge position, and projected
depth. The camera rigidly rotates the mapped scene about that base toward a
downward deep-end ray, with a normalized progress value derived from
`PlayerProjectedDepth` within the level's near/far depth interval. K, ZOffset,
and upstream calibration continue to participate through the existing
uncorrected projector; no gameplay or projection cadence was changed.

The complete submitted Blaster glyph is used for visibility bounds (all six
vector segments and their twelve endpoints). The viewport is inverted through
the established portrait playfield camera, with a one-host-pixel border inset.
If the geometry-aligned rotation cannot fit the glyph, a deterministic
minimal additional rotation toward the nearest vertical rim-edge orientation
is found; the smallest common translation that fits the glyph is then applied.
The Blaster is not resized or independently moved. Tube points, player edge,
Blaster glyph, spikes, and other projected world points share the same camera
transform. Tube vertical offset remains the common final Y translation and is
accounted for in the visibility constraint.

The correction is classified as a geometry-derived **PROJECT TRANSLATION
candidate** pending manual evaluation. The existing generic
`ExitZoomFocalCompensation` path remains in use for other levels. Regression
coverage now includes Level 11 and direct Level 15 in that generic path.

## Changed files

- `src/Tempest.Game/Rendering/Level12ExitZoomCamera.cs` — Level 12 path,
  geometry-derived transform, viewport and full-glyph visibility policy.
- `src/Tempest.Game/Rendering/VectorRenderer.cs` — Level 12 ExitZoom dispatch,
  sample-keyed transform reuse, and coherent Blaster glyph transform.
- `tests/Tempest.Game.Tests/Level12ExitZoomCameraTests.cs` — deterministic
  deep-end trajectory, transition continuity, calibration, full-glyph
  visibility across all 16 lanes and edge positions, common offset, unchanged
  Playing/HUD, and spinner movement coverage.
- `tests/Tempest.Game.Tests/ExitZoomFocalCompensationTests.cs` — generic path
  preservation coverage for Levels 11 and 15.

The task prompt and this report were added under `AI-Logs/`. No other
pre-existing tracked or untracked work was modified, staged, or committed.
The worktree was already dirty before this task; its unrelated candidate and
local reference changes were preserved.

## Validation

- **PASS** — focused presentation, simulation, and preservation tests
  (371 passed, 0 failed), including the bounded Level 12 production soak,
  spinner/input, Atari presentation/model-frame/HUD, playback/headless
  playback, generic ExitZoom, Levels 1–11, direct Level 15, Fuseball, and
  Blaster-death regressions. Run with a five-minute hang timeout.
- **PASS** — complete solution test suite: 1,832 passed, 0 failed, 0 skipped.
- **PASS** — Release solution build: 0 warnings, 0 errors.
- **PASS** — `git diff --check`. Git emitted an existing LF-to-CRLF warning
  for `src/Tempest.Game/Levels/LevelLoader.cs`; that file was not changed by
  this task.
- No ROMs, screenshots, videos, `UserData/`, `TEMP/`, or private notes were
  inspected. No interactive game was launched.

## Remaining manual acceptance

Neal should run the three requested commands:

```powershell
dotnet run --project src\Tempest.Game -- --level 12 --atari-presentation
dotnet run --project src\Tempest.Game -- --level 12 --atari-presentation --tube-y-gain 1.3 --tube-scale .8
dotnet run --project src\Tempest.Game -- --level 12 --atari-presentation --tube-y-gain 1.3 --tube-scale .8 --calibration-guides
```

Manual evaluation must confirm the exit progresses downward toward the deep
end, the complete Blaster stays visible and attached while spinner movement
remains normal, the tube remains coherent, no abrupt camera jump occurs,
ordinary Level 12 Playing remains unchanged, and accepted Levels 1–11 are
unaffected. The 1.3/.8 first-pass calibration preference remains for Neal to
judge. The candidate must not be recorded as accepted until that review.
