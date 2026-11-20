# Level 1 Atari-Presentation Correction Candidate

Date: 2026-11-15

## Outcome

**READY FOR LEVEL 1 MANUAL CHECK**

The Level 1 correction is implemented and all required automated validation
completed successfully. This remains an experimental presentation candidate;
no arcade-fidelity acceptance is claimed.

## Starting state

- Branch: `master`
- HEAD: `385f603a94391756b2ca6cccd524dc17b281e782`
- Upstream: `origin/master`
- Index: empty
- Tracked modifications already present: `src/Tempest.Game/Debug/DebugController.cs`,
  `src/Tempest.Game/Debug/PlaybackScreenshotCapture.cs`,
  `src/Tempest.Game/Levels/LevelLoader.cs`, `src/Tempest.Game/Program.cs`,
  `src/Tempest.Game/Rendering/DisplayConfiguration.cs`,
  `src/Tempest.Game/Rendering/Projection.cs`,
  `src/Tempest.Game/Rendering/VectorRenderer.cs`,
  `src/Tempest.Game/Systems/InputSystem.cs`,
  `tests/Tempest.Game.Tests/DebugControllerTests.cs`,
  `DisplayConfigurationTests.cs`, `GameOverDensityDiagnosticTests.cs`,
  `GameplayRecordingTests.cs`, `InputSystemTests.cs`,
  `Level9ProductionTests.cs`, `PlaybackScreenshotCaptureTests.cs`, and
  `TransitionPresentationTests.cs`.
- Untracked paths already present: `Reference/arcade/MBUCOD.V05.txt`,
  `Reference/arcade/MBUDOC.DOC`, `screenshot000.png`,
  `src/Tempest.Game/Levels/RomPresentationMetadata.cs`,
  `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`,
  `tests/Tempest.Game.Tests/AtariPresentationAdapterTests.cs`,
  `RomPresentationMetadataTests.cs`, and
  `VectorRendererAtariPresentationTests.cs`.

All pre-existing changes and local reference files were preserved.

## Correction implemented

- **Depth/shape:** Level 1 now uses the established C# perspective
  denominator, `depth + ZOffset`, continuously over its depth interval. Near
  rim projection remains equal to legacy; the far rim recedes with the legacy
  far/near scale relationship and is below the near rim. This avoids the
  diagnosed endpoint-normalized Atari reciprocal mapping, which collapsed and
  reversed the center separation.
- **Vertical placement:** Level 1 no longer applies signed ROM `Y2D` as a
  direct post-projection shift. Its existing `CenterY`, `YOffset`, and
  render-only Approach vertical offset remain the placement inputs. This
  independently removes the diagnosed candidate-specific upward displacement.
- The Level 1 branch introduces no tuning coefficient or image-space warp.
  Atari metadata validation and topology checks remain in place.

## What remained frozen

Level 1 points, point order, lanes, topology, `Position`, `Depth`, and gameplay
configuration were not changed. The legacy projection implementation,
portrait camera, frozen `-33.1` translation, viewport, gameplay, simulation,
playback, and recording format were not changed. Tests confirmed the model
points and configuration values were not mutated.

## Files changed

- `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs` — added the
  displayed-Level-1-only legacy-depth presentation branch.
- `tests/Tempest.Game.Tests/AtariPresentationAdapterTests.cs` — added Level 1
  near/far scale, center progression, continuity, and invalid-denominator
  checks; retained the numerical reference checks for Levels 7-9.
- `tests/Tempest.Game.Tests/VectorRendererAtariPresentationTests.cs` — proved
  Level 1 candidate tube output equals the legacy path and checked model and
  configuration immutability.
- Created only the authorized prompt and this report under `AI-Logs/`.

## Tests

- Level 1 adapter invariants cover legacy near scale, corrected far scale,
  far-center-below-near, continuous finite depth progression, symmetry,
  local Jacobian, determinism, Approach composition, invalid inputs, and
  non-mutation.
- Renderer candidate tests cover exact Level 1 legacy parity, tube anchors,
  shot glyph scaling, Game Over legacy exemption, and unchanged geometry.
- Adapter reference tests cover unchanged numerical candidate projections
  for Levels 7-9. Renderer tests cover unchanged Levels 2-6 legacy fallback.
- Portrait configuration tests confirm the later `-33.1` placement remains
  unchanged. Related Level 1, Approach, playback, recording, and screenshot
  regressions were included in the targeted run.

## Validation

SDK: .NET `9.0.304`, VSTest, `net9.0`. Every test invocation used
`--blame-hang-timeout` and an outer process timeout. The existing game process
PID `110284` held `src/Tempest.Game/bin/Debug/net9.0/Tempest.Game.exe`;
the ordinary-output test attempt failed with MSB3027 after MSB3026 retries.
The process was not stopped. Subsequent builds/tests used the isolated
`%TEMP%\TempestValidation-0096cd0f-Level1Candidate` output path.

- Adapter and candidate renderer tests, with Level 7-9 regressions:
  **PASS**, 32 passed, 0 failed.
- Related Level 1, presentation, Approach, playback, recording, and screenshot
  regressions: **PASS**, 272 passed, 0 failed.
- Full test suite: **PASS**, 1463 passed, 0 failed, 0 skipped.
- Release solution build: **PASS**, 0 warnings, 0 errors.
- `git diff --check`: **PASS**. Git noted existing LF-to-CRLF conversion
  warnings for untouched `LevelLoader.cs` and `InputSystem.cs`; there were no
  whitespace errors. The untracked candidate source/test files were also
  checked for trailing whitespace with none found.
- Final index remained empty. Final branch, HEAD, tracked modifications, and
  untracked paths were unchanged from the recorded starting state.

The first compile attempt exposed a local-name collision, which was corrected.
The first focused run also exposed test expectations tied to the old Level 1
reciprocal mapping and exact float operation order; the Level 1 assertions
were updated to check the required legacy projection while the Levels 7-9
reference remained unchanged. The final validation above was rerun and passed.

## Regression protection

Levels 7-9 retain the pre-correction adapter calculations and passed their
existing numerical reference tests. Levels 2-6 continue to use their
unchanged legacy fallback. Game Over remains on its legacy exemption.

## Deviations

Validation used isolated temporary output after the existing game locked the
normal output executable. No source or project scope was broadened.

## Remaining limitations

This correction preserves the known-good C# Level 1 tube relationship; it does
not reproduce MAME pixel geometry or establish arcade fidelity. Manual
evaluation remains required, and no tuning should occur during the first
observation.

## Manual command

dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 1 --atari-presentation

## First manual question

Does the corrected Level 1 presentation move materially closer to the desired
overall tube shape, looking-down-the-tube character, and reasonable screen
placement without introducing an obvious new problem?

## Git state

No files were staged, committed, or pushed. The pre-existing worktree and
index state were preserved.
