# Enlarged portrait viewport Git closeout

## Status

**PASS — committed and pushed.** This closes out the already manually
accepted whole-game host-window and portrait viewport enlargement. No new
display behavior was implemented.

## Prior manual acceptance

The authoritative manual-acceptance record is
`AI-Logs/Reports/2026-11-18-window-viewport-enlargement-acceptance-report.md`.
Neal accepted the candidate. The accepted host client area is `1600 x 1280`;
the centered portrait viewport is `960 x 1280`, origin `(320,0)`, exact
`3:4`, with 320-pixel left/right margins and no top/bottom margins. The prior
`720 x 960` viewport at `(280,0)` is enlarged uniformly by exactly `4/3` on
both axes. The blue border remains one host pixel. Logical presentation and
accepted Level 7/8/9/10 placements remain unchanged. The separate
C#-versus-arcade/MAME screenshot-proportion investigation remains deferred.

The host dimensions and viewport layout remain a **PROJECT DECISION**; uniform
host-space mapping remains a **PROJECT TRANSLATION**. Neither is asserted to
be ROM-backed or arcade-observed.

## Baseline and change classification

- Starting branch: `master`
- Starting HEAD: `7ba80f39df9daeaa86a7cafc81690f2f5c37bb33`
- Upstream: `origin/master`; ahead/behind `0/0`
- Starting index: clean
- Candidate changes traced to the implementation report and acceptance
  report; final staged patch contains only the viewport milestone.

Included milestone files:

- `src/Tempest.Game/Rendering/DisplayConfiguration.cs` — host, portrait
  viewport, and camera mapping.
- `src/Tempest.Game/Rendering/VectorRenderer.cs` — border thickness is
  centralized and remains one host pixel; scissor call sites use the shared
  viewport bounds.
- `tests/Tempest.Game.Tests/AtariPresentationAdapterTests.cs`
- `tests/Tempest.Game.Tests/DisplayConfigurationTests.cs`
- `tests/Tempest.Game.Tests/Level8AtariPresentationTests.cs`
- `tests/Tempest.Game.Tests/Level9AtariPresentationTests.cs`
- `tests/Tempest.Game.Tests/PreviousPortraitHostTransform.cs` — staged
  because the changed adapter and Level 8/9 assertions use it to compare
  previous host-space points with the accepted uniform enlargement.
- `TEMPEST-BEHAVIOR-SPEC.md` — records acceptance and clarifies that the
  separate screenshot-proportion investigation remains open.
- `TEMPEST-DEVELOPMENT-NOTES.md` — updates the accepted display contract and
  its environment/acceptance status.

Excluded and preserved:

- `.github/copilot-instructions.md` — unrelated pre-existing tracked change;
  left unstaged.
- `Reference/arcade/MBUCOD.V05.txt` and `Reference/arcade/MBUDOC.DOC` —
  unrelated untracked local references; not inspected or staged.
- `AI-Logs/` — ignored task records; not staged.

No screenshots, videos, gameplay recordings, TEMP, or UserData were inspected
or changed.

## Validation

The test platform is VSTest on .NET SDK `9.0.304` with xUnit. Each test
command used `--blame-hang-timeout 5m`.

**PASS — focused viewport/display/capture tests:** 157 passed, 0 failed,
0 skipped. The filter covered
`DisplayConfigurationTests`, `AtariPresentationAdapterTests`,
`Level7AtariPresentationTests`, `Level8AtariPresentationTests`,
`Level9AtariPresentationTests`, and `PlaybackScreenshotCaptureTests`.
These exercised viewport dimensions/layout, camera mapping, the shared
old-to-new host transform helper, accepted placement invariants, and
screenshot capture behavior. The renderer scissor call sites were also
verified to use `PortraitViewportLeft`, width, and height; capture semantics
remain full-window and unchanged.

```powershell
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --filter "FullyQualifiedName~DisplayConfigurationTests|FullyQualifiedName~AtariPresentationAdapterTests|FullyQualifiedName~Level7AtariPresentationTests|FullyQualifiedName~Level8AtariPresentationTests|FullyQualifiedName~Level9AtariPresentationTests|FullyQualifiedName~PlaybackScreenshotCaptureTests" --blame-hang-timeout 5m --verbosity minimal
```

**PASS — targeted presentation and determinism regressions:** 409 passed,
0 failed, 0 skipped. The filter additionally covered Atari and vector-renderer
presentation, diagnostic Level 10, Blaster presentation/scale, ExitZoom focal
compensation, Approach/transitions, recording/playback sessions, headless
playback, and visible playback/legacy behavior.

```powershell
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --filter "FullyQualifiedName~AtariPresentationAdapterTests|FullyQualifiedName~VectorRendererAtariPresentationTests|FullyQualifiedName~Level7AtariPresentationTests|FullyQualifiedName~Level8AtariPresentationTests|FullyQualifiedName~Level9AtariPresentationTests|FullyQualifiedName~DiagnosticLevel10Tests|FullyQualifiedName~BlasterPresentationTests|FullyQualifiedName~BlasterScaleRegressionTests|FullyQualifiedName~ExitZoomFocalCompensationTests|FullyQualifiedName~LevelApproachTests|FullyQualifiedName~TransitionPresentationTests|FullyQualifiedName~GameplayRecordingTests|FullyQualifiedName~GameplayPlaybackTests|FullyQualifiedName~GameplayPlaybackSessionTests|FullyQualifiedName~HeadlessGameplayPlaybackTests|FullyQualifiedName~VisiblePlaybackControllerTests|FullyQualifiedName~DisplayConfigurationTests|FullyQualifiedName~PlaybackScreenshotCaptureTests" --blame-hang-timeout 5m --verbosity minimal
```

**PASS — full solution tests:** command:

```powershell
dotnet test TempestCSharp.slnx --blame-hang-timeout 5m --verbosity minimal
```

Result: 1,586 passed, 0 failed, 0 skipped.

**PASS — Release build:**

```powershell
dotnet build TempestCSharp.slnx --configuration Release --verbosity minimal
```

Result: 0 warnings, 0 errors.

**PASS** `git diff --check`.
**PASS** `git diff --cached --check`.

No interactive game was launched; manual acceptance was already complete.
No screenshot-proportion correction, per-level placement change, geometry
change, or logical presentation change was introduced. Tests preserve Level 7
`+228.99702`, Level 8 `+212.28139`, Level 9 `+180.96825`, and diagnostic
Level 10 `+50`.

## Staging review

The staged file list was reviewed with `git diff --cached --name-status`, and
the complete staged diff was reviewed. All nine staged files listed above
belong to the accepted viewport milestone; no mixed or unrelated hunk was
staged. The helper is an intentional test-only transform utility used by the
updated host-coordinate assertions.

## Commit and push

- Commit message: `Close out enlarged portrait viewport`
- Commit: `c8bc40416eff4ffaac17862f7c894e5a34d133f9`
- Push: **PASS** — `git push` updated `master` on the existing
  `origin/master` (`7ba80f3..c8bc404`).

## Final repository state

- Branch: `master`
- HEAD: `c8bc40416eff4ffaac17862f7c894e5a34d133f9`
- Upstream: `origin/master`
- Ahead/behind: `0/0`
- Index: clean
- No accepted viewport changes remain unstaged.
- Unrelated local work remains preserved: `.github/copilot-instructions.md`
  is modified and the two arcade reference files remain untracked.
- No remote was created or changed.

## AI-Logs records

- Prompt: `AI-Logs/Prompts/2026-11-19-window-viewport-git-closeout-prompt.md`
- Report: `AI-Logs/Reports/2026-11-19-window-viewport-git-closeout-report.md`

TEMPEST_ENLARGED_PORTRAIT_VIEWPORT_GIT_CLOSEOUT
