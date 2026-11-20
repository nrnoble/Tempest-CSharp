# Blaster Exit/Approach Scale Closeout

Date: 2026-11-17

## Status

**CLOSED — manual acceptance recorded, validation passed, committed, and pushed.**

## Manual acceptance

Neal's authoritative manual result:

- Blaster exit presentation: **PASSED**.
- The previous extreme Blaster stretching during tube exit is fixed.
- The correction works across all manually tested levels.
- No further work is wanted on this defect.

The accepted ordinary Playing outward orientation and current yellow
color/intensity remain unchanged. The brightness diagnostic remains removed.

## Accepted correction and freeze

Preserved the accepted shared Atari-presentation scale correction:

```text
BlasterHeight =
    20
    * CurrentAdapterLocalScale(
        live Blaster depth,
        active presentation offsets,
        active K/ZOffset)
    / StableGameplayRimAdapterLocalScale(
        normal gameplay rim,
        no Approach offsets,
        baseline K=70,
        baseline ZOffset=5)
```

Both scales use the applicable presentation adapter. The stable baseline is
evaluated without mutating live camera state. This is a presentation-only
project correction for the established ExitZoom/Approach scale regression,
not a claim about literal Atari ROM or hardware behavior.

The correction is accepted and frozen. No further tuning was done. Ordinary
Playing, exit, and Approach sizes; scale normalization and baseline; exit and
Approach cameras; tube geometry and projection; and accepted level placements
are not to be retuned absent a separately demonstrated regression or an
explicitly authorized milestone.

Gameplay and exit simulation remained unchanged. No gameplay timing, player
depth, lane/edge state, Approach timing/offsets, transition stars, collision,
score, lives, RNG, or recording/playback simulation was changed. Level 7
architecture was not changed.

Accepted placements remain frozen: Level 8 +212.28139 logical Y, Level 9
+180.96825 logical Y, and Level 10 +50 logical Y. The existing ordinary
Blaster orientation and yellow color/intensity remain accepted; the brightness
diagnostic remains removed.

## Durable documentation

Updated:

- `TEMPEST-DEVELOPMENT-NOTES.md`
- `TEMPEST-BEHAVIOR-SPEC.md`

Both now record manual acceptance, the frozen presentation-only correction,
the unchanged gameplay/exit simulation, accepted orientation and brightness
status, unchanged Level 8/9/10 placements, and the Level 7 boundary.

## Validation

- Focused Blaster scale/presentation tests:

  ```powershell
  dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~BlasterScaleRegressionTests" --blame-hang-timeout 5min --verbosity minimal
  ```

  **PASS — 13 passed, 0 failed, 0 skipped.** Covers both exit zoom phases,
  Approach growth, active adapter scale across Levels 1-6 and 8-10,
  render-state read-only behavior, and ReverseZoom.

- Targeted rendering, adapter, exit, level presentation, Approach, transition,
  display, recording/playback, and legacy regressions:

  ```powershell
  dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~BlasterPresentationTests|FullyQualifiedName~VectorRendererAtariPresentationTests|FullyQualifiedName~AtariPresentationAdapterTests|FullyQualifiedName~Level8AtariPresentationTests|FullyQualifiedName~Level9AtariPresentationTests|FullyQualifiedName~DiagnosticLevel10Tests|FullyQualifiedName~LevelApproachTests|FullyQualifiedName~TransitionPresentationTests|FullyQualifiedName~DisplayConfigurationTests|FullyQualifiedName~GameplayRecordingTests|FullyQualifiedName~GameplayPlayback|FullyQualifiedName~ExitLaneControlTests|FullyQualifiedName~ReverseZoom|FullyQualifiedName~Legacy" --blame-hang-timeout 5min --verbosity minimal
  ```

  **PASS — 362 passed, 0 failed, 0 skipped.**

- Full solution tests:

  ```powershell
  dotnet test TempestCSharp.slnx --no-restore --blame-hang-timeout 5min --verbosity minimal
  ```

  **PASS — 1,634 passed, 0 failed, 0 skipped.**

- Release solution build:

  ```powershell
  dotnet build TempestCSharp.slnx --configuration Release --no-restore --verbosity minimal
  ```

  **PASS — 0 warnings, 0 errors.**

- `git diff --check`: **PASS.**

No canceled, stalled, blocked, timed-out, or incomplete validation was counted
as a pass. The interactive game was not launched; manual acceptance had
already been completed by Neal.

## Staging and commit

Exact staged file list:

- `TEMPEST-BEHAVIOR-SPEC.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`
- `src/Tempest.Game/Rendering/Projection.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `tests/Tempest.Game.Tests/BlasterScaleRegressionTests.cs`

Reviewed the complete staged diff. All staged hunks belong to the accepted
Blaster Exit/Approach scale milestone; no gameplay or unrelated changes were
staged. `git diff --cached --check`: **PASS**.

- Commit: `753728e79ed86624c9b1728dfd1c8db8f49d1e2b`
- Message: `Close out Blaster exit presentation`
- Push: **PASS** — pushed `master` to the existing `origin/master`.

## Final repository state

- Branch: `master`
- Upstream: `origin/master`
- HEAD: `753728e79ed86624c9b1728dfd1c8db8f49d1e2b`
- Ahead/behind: `0/0`
- Index: clean
- No accepted milestone changes remain unstaged.
- Unrelated local work remains preserved and unstaged:
  `.github/copilot-instructions.md`,
  `Reference/arcade/MBUCOD.V05.txt`, and `Reference/arcade/MBUDOC.DOC`.
- `screenshot000.png` was absent at the closeout's initial and final state
  checks. It was not opened, restored, recreated, deleted, staged, or otherwise
  modified; its prior disappearance remains unexplained.

AI-Logs are ignored and were not staged or committed.

## AI-Logs

- Prompt: `AI-Logs/Prompts/2026-11-17-blaster-exit-approach-scale-closeout-prompt.md`
- Report: `AI-Logs/Reports/2026-11-17-blaster-exit-approach-scale-closeout-report.md`
