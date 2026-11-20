# First-pass per-level Atari-presentation calibration

## Status

**IMPLEMENTED — AUTOMATED VALIDATION PASS — MANUAL VERIFICATION PENDING.**

## Work completed

- Added one explicit calibration table for Levels 1-10, including diagnostic
  Level 10, with the manually selected values unchanged.
- Atari presentation now resolves those defaults from the currently displayed
  level. Explicit `--tube-y-gain`, `--tube-scale`, and
  `--tube-vertical-offset` values remain absolute overrides of only their
  respective dimensions. Legacy/non-Atari presentation keeps its identity
  transform.
- Resolution occurs as presentation is projected, so progression and playback
  use the active displayed level rather than the startup level. Calibration
  values remain out of gameplay and recording state.
- Kept Level 10 diagnostic-only, preserved presentation registration and the
  existing ExitZoom compensation order, and did not change the fixed yellow
  guides or screenshot workflow.
- Updated the README and behavior specification to classify the table as a
  manually selected first-pass project decision, not ROM-derived geometry, and
  to document per-dimension absolute overrides.
- Added table/default, override, legacy, registration/model-immutability,
  progression, and visible-playback coverage. Updated older projection tests
  to request explicit identity values where they verify the pre-calibration
  adapter math.

## Changed for this task

- `src/Tempest.Game/Rendering/AtariPresentationCalibration.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `src/Tempest.Game/Program.cs`
- `tests/Tempest.Game.Tests/AtariPresentationCalibrationTests.cs`
- `tests/Tempest.Game.Tests/TubeYGainTests.cs`
- `tests/Tempest.Game.Tests/TubeVerticalOffsetTests.cs`
- `tests/Tempest.Game.Tests/DiagnosticLevel10Tests.cs`
- `tests/Tempest.Game.Tests/ExitZoomFocalCompensationTests.cs`
- `tests/Tempest.Game.Tests/Level7AtariPresentationTests.cs`
- `tests/Tempest.Game.Tests/Level8AtariPresentationTests.cs`
- `tests/Tempest.Game.Tests/Level9AtariPresentationTests.cs`
- `tests/Tempest.Game.Tests/VectorRendererAtariPresentationTests.cs`
- `README.md`
- `TEMPEST-BEHAVIOR-SPEC.md`

The worktree already contained other uncommitted candidate changes and
untracked local/reference material at task start. Those were preserved;
nothing was staged, committed, pushed, reset, restored, stashed, cleaned,
rebased, or launched interactively.

## Validation

- **PASS** — focused presentation/calibration regressions: 178 passed, 0 failed.
- **PASS** — complete solution test suite (`dotnet test TempestCSharp.slnx`):
  1,719 passed, 0 failed.
- **PASS** — Release build (`dotnet build TempestCSharp.slnx --configuration
  Release`): 0 warnings, 0 errors.
- **PASS** — `git diff --check`.
- Fixed yellow guide, screenshot F9/F10, ExitZoom, and related presentation
  tests were included in the focused/full-suite runs.

## Evidence and remaining acceptance

The table encodes Neal's stated manual selections as presentation-only project
decisions. It does not claim ROM provenance, gameplay geometry, or exact
arcade equivalence. The automated tests verify mechanical selection,
overrides, registration, and unchanged simulation; they do not replace Neal's
manual verification of visual fidelity and perceived presentation.

No unresolved implementation blocker remains. Neal's manual verification of
Levels 1-10 with the options omitted is pending; stop here for that review.
