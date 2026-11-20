# Level 3 Atari-Presentation Candidate

Date: 2026-11-15

## Outcome

**READY FOR LEVEL 3 PLACEMENT CHECK**

Level 3 now has an opt-in approximate model-frame mapping and a conservative
presentation candidate. Automated validation passed. This does not constitute
manual acceptance of Level 3 placement, depth, perspective, or shape.

## Starting state

- Branch: `master`
- HEAD: `385f603a94391756b2ca6cccd524dc17b281e782`
- Upstream: `origin/master`
- Index: clean; no staged paths.
- Tracked worktree modifications already present at start:
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
- Untracked paths already present at start:
  - `Reference/arcade/MBUCOD.V05.txt`
  - `Reference/arcade/MBUDOC.DOC`
  - `screenshot000.png`
  - `src/Tempest.Game/Levels/RomPresentationMetadata.cs`
  - `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`
  - `tests/Tempest.Game.Tests/AtariPresentationAdapterTests.cs`
  - `tests/Tempest.Game.Tests/RomPresentationMetadataTests.cs`
  - `tests/Tempest.Game.Tests/VectorRendererAtariPresentationTests.cs`

The existing work was preserved. The already-running game, PID 103208, locked
the normal Debug apphost; it was not terminated. Validation used isolated
artifacts under the system temporary directory. No visual assets were opened,
searched, or analyzed.

## Level 3 mapping

The handoff maps displayed Level 3 to Atari record `02` with uniform
`u=1.756696429`, X/Y orientation `+1/+1`, raw references `128/128`, and
residuals max X/Y approximately `0.375`, RMS approximately `0.433013`.
Disposition is **READY WITH EXPLICIT APPROXIMATION**. This is an
approximate frame mapping, not an exact conversion or Atari-proven C# mapping.

Current Level 3 metadata was verified without changing it: table index `2`,
`lev_scale=0x18`, `lev_y3d=0x50`, signed Y2D `-192`, and closed topology.
The comparison-only `j=(i+2) mod 16` correspondence is not encoded in
production. Points, order, lanes, and topology remain unchanged.

## Pre-candidate reconstruction

Inputs: current Level 3 plus-board points, `Position=50`, `Depth=400`,
`CenterY=400`, `YOffset=-80`, baseline `K=70`, `ZOffset=5`, and the existing
portrait camera (`Zoom=1.26`, host X origin `136`, host Y origin `-39.72`).
Bounds below are axis-aligned. Portrait-host coordinates are in the 960-pixel
viewport.

### Legacy C# projection

| Depth | Logical bounds `(minX,minY)..(maxX,maxY)` | Portrait-host bounds | Size (logical / host) | Center (logical / host) | Host top / bottom margin |
|---|---|---|---|---|---|
| Near rim, 50 | `(149.272727,47.454545)..(650.727273,548.909091)` | `(324.083636,20.072727)..(955.916364,651.905455)` | `501.454545² / 631.832727²` | `(400,298.181818) / (640,335.989091)` | `20.072727 / 308.094545` |
| Middle, 250 | `(345.921569,323.960784)..(454.078431,432.117647)` | `(571.861176,368.470588)..(708.138824,504.748235)` | `108.156863² / 136.277647²` | `(400,378.039216) / (640,436.609412)` | `368.470588 / 455.251765` |
| Far rim, 450 | `(369.692308,357.384615)..(430.307692,418.000000)` | `(601.812308,410.584615)..(678.187692,486.960000)` | `60.615385² / 76.375385²` | `(400,387.692308) / (640,448.772308)` | `410.584615 / 473.040000` |

The complete unplaced tube envelope is the near-rim envelope:
logical `(149.272727,47.454545)..(650.727273,548.909091)`, host
`(324.083636,20.072727)..(955.916364,651.905455)`. It is 631.832727 host
pixels tall, centered at host Y `335.989091`, with only `20.072727` pixels
above and `308.094545` below. Near/far size ratio is `8.272727`. The near-to-far
center advances downward `89.510490` logical / `112.783217` host pixels.

The C# X and Y derivatives are equal at each depth: `1.272727` near,
`0.274510` middle, and `0.153846` far.

### Counterfactual generic Atari candidate

These are the bounds if the generic Atari reciprocal-depth and signed-Y2D
terms were applied to Level 3 using its established mapping and metadata. Level
3 did not previously route through that generic branch.

| Depth | Logical bounds `(minX,minY)..(maxX,maxY)` | Portrait-host bounds | Size (logical / host) | Center (logical / host) | Host top / bottom margin |
|---|---|---|---|---|---|
| Near rim, 50 | `(149.272727,-19.619318)..(650.727273,481.835227)` | `(324.083636,-64.440341)..(955.916364,567.392386)` | `501.454545² / 631.832727²` | `(400,231.107955) / (640,251.476023)` | `-64.440341 / 392.607614` |
| Middle, 250 | `(334.019139,161.074462)..(465.980861,293.036184)` | `(556.864115,163.233822)..(723.135885,329.505592)` | `131.961722² / 166.271770²` | `(400,227.055323) / (640,246.369707)` | `163.233822 / 630.494408` |
| Far rim, 450 | `(362.011019,188.452307)..(437.988981,264.430269)` | `(592.133884,197.729907)..(687.866116,293.462138)` | `75.977961² / 95.732231²` | `(400,226.441288) / (640,245.596023)` | `197.729907 / 666.537862` |

This treatment clips the near rim by about `64.44` host pixels and changes
near-to-far center travel to an upward `4.666667` logical / `5.88` host pixels,
reversing the established C# progression. Generic near/far size ratio would be
`6.6`. Generic X/Y derivatives are `1.272727` near, `0.334928` middle, and
`0.192837` far. Applying those terms would reproduce the harmful upward/
reversed presentation failure class, so the first candidate does not use them.

## Candidate depth treatment

The candidate retains the existing C# reciprocal-depth formula
`K / (depth + ZOffset)` and uses no Atari reciprocal-depth or signed Y2D term
for Level 3. This keeps the near rim largest and the far rim recessed, with
continuous downward center progression. The Level 3 frame mapping remains
explicitly approximate and is not substituted for production geometry.

## Placement derivation

**PROJECT TRANSLATION FOR PRESENTATION COMPOSITION**

Using Level 3's own complete unplaced host envelope
`Y=20.072727..651.905455`, its center is `335.989091`. Centering that envelope
at the 960-pixel viewport center gives:

`(480 - 335.989091) / 1.26 = +114.29437` logical Y pixels.

The resulting host bounds are `Y=164.083636..795.916364`, leaving approximately
`164.08` pixels above and below. X remains centered at `640`. This value was
derived from Level 3's bounds and the current portrait camera; it was not copied
from Level 1 `+122`, Level 2 `+114`, or the shared `-33.1` portrait translation.

Placement is added after Level 3 shape/depth projection. Tests verify a uniform
Y-only delta, unchanged dimensions, near/far center separation, and local
Jacobian at near/middle/far. No aspect factor or perspective tuning was added.

## Placement seam

When the existing opt-in candidate is enabled for Level 3, the renderer now
selects the Level 3 adapter mapping and dedicated C#-depth branch. Existing
`ProjectWorldPoint`/Approach render-only offsets remain the common seam for
tube and tube-attached anchors. Candidate-disabled rendering continues through
the unchanged legacy projection. No `VectorRenderer.cs` edit was needed.

## Geometry/gameplay protection

No Level 3 production points, ordering, lanes, topology, or level metadata were
changed. No gameplay, input, collision, enemy, simulation, recording, or
playback code was changed. Adapter and renderer tests verify point identity,
topology, lane count, deterministic projection, and unchanged game state.
The candidate is presentation-only.

## World-object consistency

The candidate renderer test verifies the same Level 3 placement for tube rims
and rails, player edge, shot, egg (including its below-rim depth), lane-centered
Tanker and enemy-bullet anchors, Flipper endpoints, and Fuseball center.
Pulsar and Spike rendering continue to use the unchanged shared
`ProjectWorldPoint`/`ProjectLane` path when present; no Level 3 gameplay routing
or artwork was added.

## Accepted Levels 1 and 2 preservation

Level 1 `+122` and its accepted projection remain unchanged. Level 2 retains its
`1.314` vertical aspect, `+114` placement, and existing geometry. The adapter,
renderer, and Level 1/2 regression tests passed. No changes were made to
`Projection.K`, `Projection.ZOffset`, the portrait camera, viewport, or shared
portrait placement.

## Other-level preservation

Levels 4-6 continue to use legacy fallback; Levels 7-9 retain their existing
candidate mappings and behavior. Game Over remains on the exact legacy path.
Existing regression coverage for Levels 4-9, Game Over, Approach, playback,
recording, and screenshot-capture behavior passed.

## Tests and validation

Test platform was VSTest with xUnit v2 on .NET SDK `9.0.304`. Commands used
`--blame-hang-timeout 3min` and isolated artifacts at
`C:\Users\nrnob\AppData\Local\Temp\TempestLevel3Validation-20261115`.

- `dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --filter "FullyQualifiedName~AtariPresentationAdapterTests"` — **PASS**, 33 passed.
- `dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --filter "FullyQualifiedName~VectorRendererAtariPresentationTests"` — **PASS**, 22 passed.
- Accepted Level 1/2 regressions (`FullyQualifiedName~Level1|FullyQualifiedName~Level2`) — **PASS**, 112 passed.
- Levels 4-9 fallback/candidate regressions — **PASS**, 296 passed.
- Level 3, Approach, playback, recording, and screenshot-capture regressions — **PASS**, 290 passed.
- Full solution test suite, `dotnet test .\TempestCSharp.slnx` — **PASS**, 1,486 passed, 0 failed, 0 skipped.
- Release build, `dotnet build .\TempestCSharp.slnx --configuration Release` — **PASS**, 0 warnings, 0 errors.
- `git diff --check` — **PASS**, exit code 0. Existing LF-to-CRLF notices concerned pre-existing `LevelLoader.cs` and `InputSystem.cs` changes. No trailing whitespace was found in the candidate source/tests or prompt artifact.

The first normal-output Debug test attempt was blocked by the already-running
game's apphost lock. The game was not stopped; isolated artifacts resolved the
environment conflict. An initial test compilation error from an unqualified
`Camera2D` type was corrected by using inferred camera type. The placement
derivation was also corrected to use the actual portrait camera's 960-pixel
host offset; the final focused, regression, full-suite, and Release validations
all passed.

## Manual command

`dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 3 --atari-presentation`

The game was not launched.

## First manual question

Does Level 3 sit at a reasonable vertical location with appropriate stable
visual room above the tube and useful lower margin, without an obvious
tube-shape failure?

## Next manual step

Judge depth/perspective only after placement is accepted. Do not judge aspect
correction yet or request another recording.

## Git state

Final branch/HEAD/upstream remained `master` /
`385f603a94391756b2ca6cccd524dc17b281e782` / `origin/master`. Pre-existing
tracked modifications and untracked paths were preserved. The index remains
empty. The authorized prompt and this report are ignored AI-Logs artifacts.
Nothing was staged, committed, or pushed.
