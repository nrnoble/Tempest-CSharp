# Normalized portrait Y-origin implementation

Date: 2026-11-13
Completion token: `TEMPEST_Implement Normalized Y-Origin`
Status: **PARTIAL** - implementation and targeted validation complete; full-suite
validation has unrelated failures and manual acceptance remains outstanding.

## Scope and starting state

- Prompt saved verbatim in
  `../Prompts/2026-11-13-implement-normalized-y-origin.md`.
- Branch `master`, upstream `origin/master`, HEAD
  `385f603a94391756b2ca6cccd524dc17b281e782`.
- Index was empty. Existing tracked changes in development notes, LevelState,
  Program, Projection, VectorRenderer, TempestGame, ExitLaneControlTests,
  Level4CandidateTests, Level8ProductionTests, and LevelProgressionTests were
  preserved. PresentationConfiguration and ExperimentalPresentationTests
  already existed as untracked files.
- No ROMs, user recordings, screenshots, TEMP material, or unrelated assets
  were inspected or modified. No commit, push, staging, or game launch.

## Clarification and design

The original formula and requested rim alignment were incompatible with the
existing board data. At baseline K=70 / ZOffset=5, with CenterY normalized to
400, scale 1.15, and fixed 155 near-rim translation, Level 5 would reach portrait
Y=640.05 while Levels 8/9 would reach Y=732.55. The maximum model Y plus YOffset
is 120 for Level 5 and 200 for Levels 8/9.

Neal selected:

> Align Level 5 and Levels 8/9 at approximately Y=648; permit additional portrait-only placement calibration

The implementation normalizes raw projected Y with
`raw.Y + 400 - definition.CenterY` before scaling in both normal and traveling
exit mapping. Both axis scales are 1.15. All production levels use one
geometry-derived placement rule rather than the previous Level 1-7 zero shift
and Level 8/9 155 shift:

```text
normalizedMaxY = 400 + 70 * (maxModelY + YOffset) / (nearDepth + 5)
translationY = (648 - 288) / 0.79 - 1.15 * (normalizedMaxY - 300)
mappedY = 300 + 1.15 * (normalizedRawY - 300) + translationY * w(q)
```

Placement is calculated from baseline geometry, not animated approach or exit
camera depth. The existing inverse-depth weights remain unchanged, preserving
straight rails. The output is portrait logical Y=648, host Y=777.6 at the
current 1.2 viewport scale. The fixed normalized projection center does not
mean every board's model centroid is at the same final screen point: model
shape and YOffset remain intact.

Evidence: **PROJECT TRANSLATION**, user-authorized implementation candidate.
No new arcade observation or ROM-backed claim. Manual acceptance is pending.

## Changed files

- `../../src/Tempest.Game/Rendering/PresentationConfiguration.cs`: unified 1.15
  scales, normalized portrait center, normal/exit center validation and mapping,
  geometry-derived near-rim placement helper.
- `../../src/Tempest.Game/Rendering/VectorRenderer.cs`: pass actual board center
  and baseline-derived translation through the shared mapping for tube and
  aligned entity geometry, including approach and exit.
- `../../tests/Tempest.Game.Tests/ExperimentalPresentationTests.cs`: updated
  calibration assertions; added all-level normal/exit origin normalization,
  submitted near-rim Y=648, legacy identity, and invalid-center regressions.
- `../../TEMPEST-DEVELOPMENT-NOTES.md`: candidate design, authorized deviation
  from fixed 155 translation, evidence classification, validation limitations,
  and pending manual review.
- This report and the paired prompt are ignored AI-Logs material.

## Validation

SDK 9.0.304, VSTest 17.14.1, xUnit v2, net9.0. No dependency installation or
restore was needed.

1. **BLOCKED**:
   `dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~ExperimentalPresentationTests" --blame-hang-timeout 2min --verbosity minimal`
   Build failed with MSB3027 / MSB3021 because the running game, PID 66808,
   locked `src\Tempest.Game\bin\Debug\net9.0\Tempest.Game.exe`.
   The existing game process was not stopped or modified.
2. **PASS**: same targeted command with
   `-p:BaseOutputPath=C:\Users\nrnob\.copilot\session-state\35fc727e-35a1-4dd5-bd79-7b0b295f6207\files\normalized-y-validation\`.
   Fresh isolated build succeeded; 23 passed, 0 failed, 0 skipped.
   Actual submitted near-rim vertices for all nine levels are within 0.01
   portrait pixel of 648. Tests also cover all production lanes/depths,
   approach, exit clipping, fixed exit player glyph, and legacy behavior.
3. **FAIL**:
   `dotnet test TempestCSharp.slnx --no-restore -p:BaseOutputPath=C:\Users\nrnob\.copilot\session-state\35fc727e-35a1-4dd5-bd79-7b0b295f6207\files\normalized-y-validation\ --blame-hang-timeout 2min --verbosity minimal`
   Completed: 1361 passed, 4 failed, 0 skipped (1365 total).
4. **FAIL**: serialized full-suite diagnostic:
   `dotnet test TempestCSharp.slnx --no-build --no-restore -p:BaseOutputPath=C:\Users\nrnob\.copilot\session-state\35fc727e-35a1-4dd5-bd79-7b0b295f6207\files\normalized-y-validation\ --blame-hang-timeout 2min --verbosity minimal -- xUnit.ParallelizeTestCollections=false`
   Completed: 1362 passed, 3 failed, 0 skipped (1365 total).
5. **PASS**: `git diff --check`.

The three repeatable failures expect ReverseZoom but receive ExitSpikeDying:

- Level4CandidateTests.SpikeReverseZoomDrawsBlasterEffectWithoutOrdinaryEnemyRetraction
  (line 797).
- SimulationTests.SpikeReverseZoomKeepsDeathDepthUntilOffsetRestoresAndHandlesGameOver
  (line 4455).
- SimulationTests.SpikeTravelCollisionSweepsDepthAndLaneAndCostsOneLifeBeforeRetry
  (line 4421).

These assertions fail before rendering. The starting worktree's TempestGame
changes explicitly route fatal exit contacts to ExitSpikeDying rather than
ReverseZoom; this task did not modify simulation or those tests.

The fourth failure, GameOverDensityDiagnosticTests.OmittedAndExplicitTwoRenderIdenticalGameOverMarks
(line 95), compared two legacy render captures with different Y values during
the parallel suite. It passed in the serialized run. Shared mutable Projection
state is a plausible cause, not a separately proven diagnosis. Left unchanged.
No claim that the full suite passes.

## Manual handoff

Close the existing game normally before rebuilding through its default output.
Run separately, from the repository root:

```powershell
dotnet run --project src\Tempest.Game -- --level 5
dotnet run --project src\Tempest.Game -- --level 9
```

Compare the lowest near-rim horizontal segments at the same viewport size;
both should cross portrait logical Y=648 / host Y=777.6. Check Level 8 and
Levels 1-7, entity/rail alignment, approach, and exit presentation as visual
regressions. Interactive acceptance was intentionally left to Neal.
No acceptance or arcade-fidelity closeout is claimed.
