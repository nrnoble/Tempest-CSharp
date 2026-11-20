# ExitZoom Focal Compensation Closeout

Date: 2026-11-18

## Status

**COMPLETE.** Neal's manual acceptance was recorded, durable documentation was
updated, all required validation passed, only this milestone was staged and
committed, and the commit was pushed to the existing `origin/master` upstream.
No implementation or tuning was performed during this closeout.

## Manual acceptance

Neal's authoritative result:

- The ExitZoom focal-point regression is fixed.
- Level 5 remains fixed.
- Level 8 now exits properly.
- Level 9 now exits properly.
- No further work is wanted on this bug.
- Ordinary Playing presentation remains correct.
- Gameplay remains normal.

Level 10 remains diagnostic-only and was not required for this acceptance.
Level 7 remains outside this milestone; its ExitZoom was not accepted or
reconstructed. No interactive game was launched.

## Accepted rule and evidence

The final implementation is preserved:

- **Closed tubes:** use the arithmetic center of the mapped far-rim vertices,
  preserving the manually successful Level 5 behavior.
- **Open tubes:** calculate the common intersection of the mapped near-to-far
  rails using current mapped tube geometry. The omitted terminal-endpoint
  chord midpoint is not used. No per-level focal constants are used.
- For open tubes, the least-squares fit uses the non-degenerate mapped rails
  in double precision, requires independent rail directions, and verifies
  the maximum perpendicular residual. The numerical tolerance is
  `max(1e-5, coordinateScale * 32 * float epsilon)`, where `coordinateScale`
  is the largest absolute mapped coordinate.
- The existing compensation remains
  `latchedPivotAtExitStart - currentMappedPivot`. It is evaluated after the
  active presentation adapter and accepted level placement, then applied in
  logical space before the common portrait host camera. The shared
  translation applies to the tube, Blaster, Spikes, and tube-space
  presentation objects. It is active only during ExitZoom, not ordinary
  Playing or Approach.

These pivot rules and the C# compensation architecture are an accepted
**PROJECT TRANSLATION**, not literal Atari or ROM rules. The ROM does not
designate either pivot or this architecture.

The correction is presentation-only. Gameplay and exit simulation, ordinary
Playing placement, and the accepted Blaster ExitZoom/Approach scale correction
remain unchanged. Level 8 `+212.28139`, Level 9 `+180.96825`, and diagnostic
Level 10 `+50` placements remain frozen.

## Durable documentation

Updated:

- `TEMPEST-DEVELOPMENT-NOTES.md`
- `TEMPEST-BEHAVIOR-SPEC.md`

Both records state the manual acceptance, pivot rules and compensation
lifecycle, PROJECT TRANSLATION evidence classification, presentation-only
boundary, unchanged Blaster behavior and Playing placement, Level 10
diagnostic-only status, and Level 7 acceptance boundary.

## Validation

- **PASS — focused, 19 passed, 0 failed, 0 skipped:**
  ```powershell
  dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~ExitZoomFocalCompensationTests" --blame-hang-timeout 4m --verbosity minimal
  ```
  The tests cover the closed far-rim pivot; open rail convergence and
  residual/tolerance for Levels 8/9/10; Level 5 and Levels 1-4/6 controls;
  mapped geometry rather than terminal midpoint; shared tube/Blaster/Spikes
  translation with unchanged Blaster size; latch, retry, and handoff lifecycle;
  and rendering-state preservation.
- **PASS — targeted regressions, 1,048 passed, 0 failed, 0 skipped:**
  ```powershell
  $filter = 'FullyQualifiedName~ExitZoom|FullyQualifiedName~ExitLaneControlTests|FullyQualifiedName~AvoidSpikes|FullyQualifiedName~Spike|FullyQualifiedName~Blaster|FullyQualifiedName~Level1|FullyQualifiedName~Level2|FullyQualifiedName~Level3|FullyQualifiedName~Level4|FullyQualifiedName~Level5|FullyQualifiedName~Level6|FullyQualifiedName~Level7|FullyQualifiedName~Level8|FullyQualifiedName~Level9|FullyQualifiedName~Level10|FullyQualifiedName~AtariPresentationAdapterTests|FullyQualifiedName~VectorRenderer|FullyQualifiedName~LevelApproachTests|FullyQualifiedName~TransitionPresentationTests|FullyQualifiedName~Recording|FullyQualifiedName~Playback|FullyQualifiedName~DisplayConfigurationTests|FullyQualifiedName~Camera|FullyQualifiedName~Projection'
  dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter $filter --blame-hang-timeout 4m --verbosity minimal
  ```
  This selection covers ExitZoom, AvoidSpikes, retry/Spike collision,
  Blaster presentation and scale, Levels 1-6/8/9 and diagnostic Level 10,
  AtariPresentationAdapter, VectorRenderer, Approach/transition,
  recording/playback, display/camera/projection, option-off/legacy behavior,
  and Level 7 presentation protection.
- **PASS — full solution with VSTest hang-timeout protection, 1,653 passed,
  0 failed, 0 skipped:**
  ```powershell
  dotnet test .\TempestCSharp.slnx --no-restore --blame-hang-timeout 4m --verbosity minimal
  ```
- **PASS — Release solution build, 0 warnings, 0 errors:**
  ```powershell
  dotnet build .\TempestCSharp.slnx --configuration Release --no-restore --verbosity minimal
  ```
- **PASS —** `git diff --check`
- **PASS —** `git diff --cached --check`

No validation command was canceled, blocked, or incomplete.

## Staging and commit

The staged diff was reviewed in full. All staged changes belong to this
milestone; no AI-Logs, reference, screenshot, recording, TEMP, UserData, or
unrelated local files were staged.

Exact staged file list:

- `TEMPEST-BEHAVIOR-SPEC.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `src/Tempest.Game/Rendering/ExitZoomFocalCompensation.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `tests/Tempest.Game.Tests/ExitZoomFocalCompensationTests.cs`

**PASS — staged whitespace check;** no staged-path edits remained outside the
index.

- Commit: `aee59b39c1d68be830d0014a7b9497d1bb40f3c7`
- Message: `Close out ExitZoom focal compensation`
- Push: **PASS**, pushed `master` to the existing `origin/master`.

## Git state and preservation

Starting state: branch `master`, HEAD
`753728e79ed86624c9b1728dfd1c8db8f49d1e2b`, upstream `origin/master`,
ahead/behind `0/0`, and clean index. The surviving candidate consisted of the
`VectorRenderer.cs` modification and untracked focal-compensation helper and
test. The unrelated `.github/copilot-instructions.md` modification and
untracked `Reference/arcade/MBUCOD.V05.txt` and `Reference/arcade/MBUDOC.DOC`
were preserved and excluded from the commit.

Verified post-push state:

- Branch `master`; HEAD is `aee59b39c1d68be830d0014a7b9497d1bb40f3c7`.
- Upstream remains `origin/master`; ahead/behind is `0/0`.
- Index is clean.
- No accepted milestone changes remain unstaged.
- The unrelated `.github/copilot-instructions.md` modification and both
  untracked local references remain preserved.

## AI-Logs

- Prompt: `AI-Logs/Prompts/2026-11-18-exitzoom-focal-compensation-closeout-prompt.md`
  (byte-identical to the supplied task; SHA-256
  `932D3FCEC3851B14859B230553531249F362EC0A49BFD1740145466BCBBB64CF`)
- Report: `AI-Logs/Reports/2026-11-18-exitzoom-focal-compensation-closeout-report.md`
