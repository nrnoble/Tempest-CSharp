# Level 7 conditional factor-128 diagnostic experiment

## Outcome

**Candidate ready for Neal's manual A/B comparison. Manual acceptance: PENDING.**
The diagnostic is an opt-in presentation-only experiment; no claim of visual
improvement or Atari fidelity is made.

## Implementation

Added `--atari-tube-rn000f-diagnostic`, accepted only with
`--atari-tube-prototype`. The existing prototype command and default output
remain the control. The diagnostic selects the integer transfer

    floor((128 * magnitude) / depth)

in the existing Atari coordinate projector; the ordinary prototype continues
to use its previous

    floor((256 * magnitude) / depth)

transfer. Division remains integer/truncating. Raw differences, sign
restoration, X/Y bases, signed handling, DVG axis ordering, record-06 point
order, vector topology, near/far positions, and viewport mapping use the
existing paths.

### Evidence classification

The factor-128 calculation is **DEVELOPMENT-ONLY**, supported conditionally on
RN=$000F. The available local evidence establishes RN.low=$0F but does not
establish RN.high at the first normal Level 7 record-06 DIVZ operation. RN.high
therefore remains unresolved there; assuming RN.high=0 for this experiment does
not establish normal Atari behavior or an accepted project translation. This
is not an Atari-exact production correction.

### Representative numeric tests

The test vectors verify exact integer truncation:

| magnitude | position operand | scale | divisor | factor-128 quotient |
|---:|---:|---:|---:|---:|
| 0 | 13 | 13 | 26 | 0 |
| 1 | 13 | 13 | 26 | 4 |
| 64 | 32 | 32 | 64 | 128 |
| 255 | 13 | 13 | 26 | 1255 |
| 64 | 283 | 283 | 566 | 14 |

For record-06 point 0, the factor-128 diagnostic yields p=$10: X/Y quotients
339/179, source coordinates (339, 35), DVG (35, 339); and p=$F0: quotients
51/27, source coordinates (51, -117), DVG (-117, 51). Sign/base checks verify
negative and positive source values remain correctly restored. The control's
existing p=$10 and p=$F0 projection vectors are still tested unchanged.

## Files changed for this task

- `src/Tempest.Game/Debug/DebugController.cs` — selector parsing, prototype
  coupling, and context rejection.
- `src/Tempest.Game/Program.cs` — selector wiring and DEVELOPMENT-ONLY
  diagnostic notice.
- `src/Tempest.Game/Rendering/AtariCoordinateProjector.cs` — integer factor
  selection and RN.high qualification.
- `src/Tempest.Game/Rendering/AtariTubeVectorListBuilder.cs` — forwards the
  opt-in only through the existing record-06 prototype builder.
- `src/Tempest.Game/Rendering/VectorRenderer.cs` — enables the diagnostic only
  while the Level 7 prototype is selected.
- `tests/Tempest.Game.Tests/AtariTubeRecordPrototypeTests.cs` — exact numeric,
  topology, CLI context, option-off, and presentation-only coverage.
- `AI-Logs/Prompts/2026-11-16-level7-factor128-diagnostic-experiment-prompt.md`
  and this report; both are ignored and were not staged.

The record-06 geometry data and metadata, point order, topology, viewport map,
HUD/presentation constants, existing `--atari-presentation` path, and gameplay
simulation were not modified. The diagnostic does not write recording data.

## Validation

- Focused Release prototype/projector/option tests: **PASS**, 43/43.
- Targeted presentation, CLI, recording/playback, Level 7, prototype, and
  Atari-presentation regression tests: **PASS**, 434/434.
- Full Release test suite: **PASS**, 1,548/1,548.
- Release solution build (`dotnet build .\TempestCSharp.slnx
  --configuration Release --no-restore`): **PASS**, zero warnings and errors.
- `git diff --check`: **PASS**, no whitespace errors. Git emitted existing
  LF-to-CRLF working-copy warnings for `LevelLoader.cs` and `InputSystem.cs`.
- The first Debug test attempt was **BLOCKED** because an already-running
  `Tempest.Game` process (PID 127052) held the Debug apphost. It was left
  untouched; final tests and build completed using the separate Release output.
- The interactive game was not launched.

## Git and worktree state

At start: branch `master`, HEAD
`385f603a94391756b2ca6cccd524dc17b281e782`, tracking `origin/master`, and a
clean index. Pre-existing tracked modifications were:

`.github/copilot-instructions.md`,
`src/Tempest.Game/Debug/DebugController.cs`,
`src/Tempest.Game/Debug/PlaybackScreenshotCapture.cs`,
`src/Tempest.Game/Levels/LevelLoader.cs`,
`src/Tempest.Game/Program.cs`,
`src/Tempest.Game/Rendering/DisplayConfiguration.cs`,
`src/Tempest.Game/Rendering/Projection.cs`,
`src/Tempest.Game/Rendering/VectorRenderer.cs`,
`src/Tempest.Game/Systems/InputSystem.cs`,
`tests/Tempest.Game.Tests/DebugControllerTests.cs`,
`tests/Tempest.Game.Tests/DisplayConfigurationTests.cs`,
`tests/Tempest.Game.Tests/GameOverDensityDiagnosticTests.cs`,
`tests/Tempest.Game.Tests/GameplayRecordingTests.cs`,
`tests/Tempest.Game.Tests/InputSystemTests.cs`,
`tests/Tempest.Game.Tests/Level9ProductionTests.cs`,
`tests/Tempest.Game.Tests/PlaybackScreenshotCaptureTests.cs`, and
`tests/Tempest.Game.Tests/TransitionPresentationTests.cs`.

Pre-existing untracked files were:

`Reference/arcade/MBUCOD.V05.txt`, `Reference/arcade/MBUDOC.DOC`,
`screenshot000.png`,
`src/Tempest.Game/Levels/AtariTubeRecordCatalog.cs`,
`src/Tempest.Game/Levels/RomPresentationMetadata.cs`,
`src/Tempest.Game/Rendering/AtariCoordinateProjector.cs`,
`src/Tempest.Game/Rendering/AtariDvgViewportMap.cs`,
`src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`,
`src/Tempest.Game/Rendering/AtariTubeVectorListBuilder.cs`,
`tests/Tempest.Game.Tests/AtariPresentationAdapterTests.cs`,
`tests/Tempest.Game.Tests/AtariTubeRecordPrototypeTests.cs`,
`tests/Tempest.Game.Tests/RomPresentationMetadataTests.cs`, and
`tests/Tempest.Game.Tests/VectorRendererAtariPresentationTests.cs`.

At end: branch, HEAD, upstream, and clean-index state remain unchanged. The
task's source/test edits are within the pre-existing modified/untracked files
listed above; no pre-existing files were reset, staged, or committed. The
prompt and report are ignored by Git. No commit or push was performed.

## Manual A/B handoff

**A — control, current factor-256 prototype:**

    dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 7 --atari-tube-prototype

**B — experiment, conditional factor-128 diagnostic:**

    dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 7 --atari-tube-prototype --atari-tube-rn000f-diagnostic

Compare A's unchanged current quotient behavior against B's conditional
RN=$000F factor-128 quotient behavior. Visual comparison and acceptance remain
**PENDING** Neal's manual review.
