# Atari Presentation Adapter Implementation Report

Date: 2026-11-15

## Outcome

**READY FOR MANUAL ACCEPTANCE**

The opt-in render-only candidate is implemented and all required automated
validation completed successfully. No interactive game was launched. Neal's
manual visual and gameplay acceptance remains pending; the candidate is not the
default and has not been committed.

## Starting state

- Branch: `master`
- HEAD: `385f603a94391756b2ca6cccd524dc17b281e782`
- Upstream: `origin/master`
- Index: empty
- Starting tracked modifications, preserved:
  - `src/Tempest.Game/Debug/DebugController.cs`
  - `src/Tempest.Game/Debug/PlaybackScreenshotCapture.cs`
  - `src/Tempest.Game/Levels/LevelLoader.cs`
  - `src/Tempest.Game/Program.cs`
  - `src/Tempest.Game/Rendering/DisplayConfiguration.cs`
  - `src/Tempest.Game/Rendering/Projection.cs`
  - `src/Tempest.Game/Rendering/VectorRenderer.cs`
  - `src/Tempest.Game/Systems/InputSystem.cs`
  - `tests/Tempest.Game.Tests/DebugControllerTests.cs`
  - `tests/Tempest.Game.Tests/DisplayConfigurationTests.cs`
  - `tests/Tempest.Game.Tests/GameOverDensityDiagnosticTests.cs`
  - `tests/Tempest.Game.Tests/GameplayRecordingTests.cs`
  - `tests/Tempest.Game.Tests/InputSystemTests.cs`
  - `tests/Tempest.Game.Tests/Level9ProductionTests.cs`
  - `tests/Tempest.Game.Tests/PlaybackScreenshotCaptureTests.cs`
  - `tests/Tempest.Game.Tests/TransitionPresentationTests.cs`
- Starting untracked paths, preserved:
  - `Reference/arcade/MBUCOD.V05.txt`
  - `Reference/arcade/MBUDOC.DOC`
  - `src/Tempest.Game/Levels/RomPresentationMetadata.cs`
  - `tests/Tempest.Game.Tests/RomPresentationMetadataTests.cs`

## Work performed

- Added [AtariPresentationAdapter.cs](../../src/Tempest.Game/Rendering/AtariPresentationAdapter.cs) for the continuous presentation transform, numeric/metadata validation, supported frame mappings, logical coordinates, and local Jacobian.
- Updated [VectorRenderer.cs](../../src/Tempest.Game/Rendering/VectorRenderer.cs) to select the adapter only when explicitly enabled, retain exact legacy formulas otherwise, emit one bounded legacy-fallback diagnostic for Levels 2-6, transform world anchors consistently, use the candidate Jacobian for depth-responsive world glyphs, and explicitly route Game Over vectors through legacy projection.
- Updated [DebugController.cs](../../src/Tempest.Game/Debug/DebugController.cs) and [Program.cs](../../src/Tempest.Game/Program.cs) for the opt-in `--atari-presentation` option, option incompatibilities, portrait-only mode, interactive record/playback composition, and rejection in headless analysis and input diagnostics.
- Added [AtariPresentationAdapterTests.cs](../../tests/Tempest.Game.Tests/AtariPresentationAdapterTests.cs) and [VectorRendererAtariPresentationTests.cs](../../tests/Tempest.Game.Tests/VectorRendererAtariPresentationTests.cs).
- Extended [DebugControllerTests.cs](../../tests/Tempest.Game.Tests/DebugControllerTests.cs) and [DisplayConfigurationTests.cs](../../tests/Tempest.Game.Tests/DisplayConfigurationTests.cs).
- Preserved this prompt at `AI-Logs/Prompts/2026-11-15-atari-presentation-adapter-implementation-prompt.md`.
- No other AI-Logs files were created. AI-Logs remain ignored and unstaged.

## Architecture implemented

- `ProjectWorldPoint` is the render-only selection seam. The adapter receives only model coordinates, render depth, definition/configuration values, render-only Approach offsets, current `Projection.K`/`ZOffset`, and the displayed level's existing ROM metadata.
- The adapter is stateless and keeps the model-frame table separate from ROM metadata:
  - Level 1: `u=25/14`, `e=+1`
  - Level 7: `u=1.678958593`, `e=+1`
  - Levels 8 and 9: `u=25/14`, `e=-1`
- It implements the continuous reciprocal-depth contract without depth clamping or Atari integer quantization. It returns finite logical coordinates and local scale; non-finite arithmetic, invalid denominators, unsupported mappings, and inconsistent supported-level metadata fail explicitly.
- Endpoint-normalized depth is documented as a project translation, not literal Atari runtime depth. ROM signed Y2D is applied after reciprocal projection and remains distinct from C# `YOffset`.
- Levels 2-6 retain the exact legacy projection and receive one development-only diagnostic per level per renderer.
- Existing C# topology remains authoritative. ROM `OpenRaw` is cross-checked for supported candidate levels but never changes topology.
- Candidate rendering is opt-in and portrait-only. It is rejected with `--legacy-presentation`, any explicit `--portrait-perspective` value including zero, headless playback analysis, and `--input-diagnostic`. Interactive recording/playback remain supported; presentation selection is not added to recordings.
- Registered world anchors use the same transform. Depth-responsive glyph scale uses the adapter's local Jacobian while preserving vector programs, colors, animation, geometry, and fixed floors. Fuseball's additive term remains unchanged.
- Game Over raw vector records deliberately use the unchanged legacy projection because their coordinate frame is not ordinary level model geometry.

## Geometry and gameplay protection

The adapter receives no gameplay services or mutable entity references. It does
not write to level point arrays, point order, lane identity, playable-lane
count, topology, entity state, simulation state, timing, input, scoring,
collisions, recording data, or playback state. Tests render repeatedly and
compare points/order/topology, check same-depth lane interpolation, and verify
Game Over output remains identical to the legacy renderer.

`Projection.Project`, `Projection.cs`, `DisplayConfiguration.cs`,
`LevelDefinition`, `LevelConfiguration`, `LevelLoader`, Approach simulation,
transition starfield, screenshot capture, input, and recording formats were
not functionally modified. The shared portrait placement `-33.1` remains in
the later portrait camera stage.

## Tests added or changed

- Adapter tests cover Levels 1/7/8/9 at near/mid/far depths, the formula and
  local Jacobian, symmetry, signed orientation/Y2D behavior, Approach offset
  composition, deterministic output, negative-denominator extrapolation,
  invalid arithmetic, metadata consistency, and supported-level boundaries.
- Renderer tests cover all production levels' point order/topology and
  non-mutation, explicit legacy fallback for Levels 2-6, one bounded fallback
  diagnostic, same-depth interpolation, candidate shot scale, portrait-only
  renderer enforcement, and Game Over's legacy exemption.
- Option tests cover opt-in/default behavior, duplicates, malformed values,
  exclusivity, file recording/playback composition without recording metadata,
  and rejection in headless/input-diagnostic contexts.
- Display tests verify candidate logical output still goes through the frozen
  portrait camera placement.
- Existing ROM metadata, renderer, transition/Approach, shot, enemy-bullet,
  destruction-effect, production Level 1/7/8/9, playback, screenshot, and
  recording tests were run as regressions.

## Validation

All VSTest invocations used `--blame-hang-timeout 2m`.

- Initial focused adapter test invocation: **BLOCKED before test execution**.
  MSBuild could not copy `Tempest.Game.exe` because an already-running
  `Tempest.Game` process (PID 95860) held it.
- Retrying with `-p:UseAppHost=false`: **BLOCKED before test execution** because
  the same process held `Tempest.Game.dll`.
- Switched to an isolated output directory under
  `$env:TEMP\Tempest-AtariPresentation-20261115` with
  `-p:BaseOutputPath=... -p:UseAppHost=false`; this avoided the locked build
  artifacts without touching the running process.
- The two blocked test commands were:
  `dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --filter "FullyQualifiedName~AtariPresentationAdapterTests" --blame-hang-timeout 2m`
  and the same command with `-p:UseAppHost=false`.
- Initial isolated adapter run using that filter and the isolated-output
  properties: **PASS**, 14 tests. The final run below includes the completed
  offset and extrapolation assertions.
- First isolated renderer run with
  `--filter "FullyQualifiedName~VectorRendererAtariPresentationTests"`:
  **FAIL**, one test fixture had not advanced the Game Over animation far
  enough to submit records. The test setup was corrected to advance
  deterministic simulation ticks; production behavior was unchanged.
- Final focused commands, each with
  `--blame-hang-timeout 2m -p:BaseOutputPath="$outputRoot\\" -p:UseAppHost=false`,
  where `$outputRoot = Join-Path $env:TEMP 'Tempest-AtariPresentation-20261115'`:
  - `--filter "FullyQualifiedName~AtariPresentationAdapterTests"`: **PASS**,
    18 passed.
  - `--filter "FullyQualifiedName~VectorRendererAtariPresentationTests"`:
    **PASS**, 13 passed.
  - `--filter "FullyQualifiedName~DebugControllerTests"`: **PASS**, 54 passed.
  - Relevant renderer/presentation/playback filter (DisplayConfiguration,
    BlasterLaneHighlight, PlayerShotRendering, Level1ProductionEnemyBulletRendering,
    EnemyDestructionEffect, TransitionPresentation, PlaybackScreenshotCapture,
    GameplayRecording, Level7/8/9Production, RomPresentationMetadata,
    GameOverVectorBurstSequence, and GameOverDensityDiagnostic): **PASS**,
    317 passed.
  - No filter (complete test project suite): **PASS**, 1,462 passed, 0 failed,
    0 skipped.
- Release solution command:
  `dotnet build .\TempestCSharp.slnx --configuration Release -p:BaseOutputPath="$outputRoot\\" -p:UseAppHost=false`:
  **PASS**, 0 warnings, 0 errors.
- `git diff --check`: **PASS**. Git emitted only the pre-existing LF-to-CRLF
  working-copy warnings for `LevelLoader.cs` and `InputSystem.cs`.
- No command was canceled or timed out. The interactive game was not launched.

## Deviations from plan

- Validation output was redirected to a unique temporary output root because
  the pre-existing interactive `Tempest.Game` process held the normal Debug
  apphost and DLL. No process was stopped, replaced, or otherwise disturbed.
- The first Game Over integration test run exposed an incomplete test fixture;
  deterministic ticks were added so active vector records exist. No production
  design or behavior deviation was needed.

## Remaining limitations

- Levels 2-6 intentionally retain legacy projection pending evidence-backed
  model-frame mappings.
- Level 7's model-frame fit remains approximate; point order and lane indices
  are unchanged, with no comparison-only cyclic rotation.
- The candidate uses continuous arithmetic, not bit-exact Math Box state,
  integer floor/quantization, or signed-16 saturation.
- Literal DVG-to-host calibration is not established.
- Game Over vector records remain on the legacy path.
- Neal's manual visual/gameplay acceptance is pending. No candidate tuning or
  activation is authorized before that review.

## Manual-validation commands

Run these commands from the repository root in PowerShell after choosing
manual acceptance. The recording guard prevents overwriting an existing file.

```powershell
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 1 --atari-presentation
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 1
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 7 --atari-presentation
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 8 --atari-presentation
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 9 --atari-presentation

$recording = Join-Path $env:TEMP 'tempest-atari-presentation-20261115.json'
if (Test-Path -LiteralPath $recording) { throw "Choose a fresh recording path; refusing to overwrite $recording" }
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 7 --atari-presentation --record-gameplay $recording
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation --playback-file $recording
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --playback-file $recording
```

## Manual-acceptance checklist

- [ ] Tube geometry and lane order are unchanged.
- [ ] Perspective/depth behavior and vertical placement are acceptable.
- [ ] Open/closed topology remains correct.
- [ ] Tube/entity registration is acceptable.
- [ ] Local glyph scale and player-shot appearance are acceptable.
- [ ] Rim objects and below-rim objects remain coherent.
- [ ] Spikes and their tips remain registered.
- [ ] Approach rendering, spinner-controlled Blaster behavior, and the natural
      Level 7-to-8 transition remain acceptable.
- [ ] Pause and forward/reverse frame stepping remain correct.
- [ ] Live and paused-playback screenshot capture remain correct.
- [ ] Recorded playback is deterministic with the candidate explicitly
      selected; replay without the candidate uses legacy presentation.
- [ ] Gameplay mechanics, timing, and input behavior are unchanged.

Do not tune candidate constants during the first manual run. Keep portrait
placement `-33.1` frozen.

## Git and repository state

- Ending branch: `master`
- Ending HEAD: `385f603a94391756b2ca6cccd524dc17b281e782`
- Ending upstream: `origin/master`
- Index: empty; nothing staged.
- The starting tracked modifications and four pre-existing untracked paths
  listed above remain preserved. The only additional untracked production/test
  files are the authorized adapter and its two focused test files.
- The two authorized AI-Logs artifacts are ignored.
- Nothing was committed or pushed.

## Most important implementation result

The candidate now provides an explicit, render-only, per-level Atari-style
continuous projection for Levels 1, 7, 8, and 9, with shared anchor/local-scale
handling and unchanged legacy fallback/default behavior.

## Most important remaining uncertainty

Only Neal's manual play can establish whether the candidate's visual
perspective, vertical placement, registration, glyph scale, and overall
gameplay presentation are acceptable.
