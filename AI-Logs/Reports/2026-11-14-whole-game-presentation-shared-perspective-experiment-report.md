# Whole-Game Presentation Shared Perspective Experiment

**Date:** 2026-11-14
**Status:** CALIBRATION MECHANISM READY — DEFAULT PENDING MANUAL CHOICE
**Completion token:** `TEMPEST_PRESENTATION_SHARED_PERSPECTIVE_EXPERIMENT_20261114`

## Scope and starting Git state

Implemented one shared, reversible portrait projection candidate. The default
remains neutral because the available evidence does not justify selecting a
non-zero default; the signed runtime calibration and bounded manual probes are
available for Neal's comparison. This is not accepted arcade behavior, a ROM
mapping, or a final visual calibration.

- Branch: `master`
- `HEAD` and `origin/master`: `385f603a94391756b2ca6cccd524dc17b281e782`
- Index: clean
- Tracked worktree: the four expected portrait candidate files were modified.
- Starting diff: 4 files changed, 429 insertions, 36 deletions.
- Non-ignored untracked files: none.

The substantive prompt was copied verbatim to
`AI-Logs/Prompts/2026-11-14-whole-game-presentation-shared-perspective-experiment-prompt.md`.

## Materials inspected

The four named authorities were read:

- `AI-Logs/Reports/2026-11-13-whole-game-presentation-cross-level-clipping-diagnostic-report.md`
- `AI-Logs/Reports/2026-11-13-whole-game-presentation-shared-vertical-placement-correction-report.md`
- `AI-Logs/Reports/2026-11-13-whole-game-presentation-tube-size-readonly-analysis-report.md`
- `AI-Logs/Reports/2026-11-14-whole-game-presentation-perspective-envelope-readonly-analysis-report.md`

Relevant source/test paths inspected:

- `src/Tempest.Game/Rendering/Projection.cs`
- `src/Tempest.Game/Rendering/DisplayConfiguration.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Debug/DebugController.cs`
- `src/Tempest.Game/TempestGame.cs`
- `src/Tempest.Game/Levels/LevelApproach.cs`
- `tests/Tempest.Game.Tests/DisplayConfigurationTests.cs`
- `tests/Tempest.Game.Tests/TransitionPresentationTests.cs`
- `tests/Tempest.Game.Tests/DebugControllerTests.cs`
- `tests/Tempest.Game.Tests/GameplayRecordingTests.cs`
- `tests/Tempest.Game.Tests/Tempest.Game.Tests.csproj`

## Implementation and projective-line proof

For model point `(x,y)` at depth `z`, the original projection remains intact
when the coefficient `a` is zero. With the candidate enabled:

```text
q = 1 / (z + ZOffset)
qNear = 1 / (nearDepth + ZOffset)

xProjected = 400 + x * K * q
yProjected = CenterY + (y + YOffset) * K * q
             + a * K * (q - qNear)
```

For every normal production level, `nearDepth` is the established position
50. During the existing approach projection, the shared helper supplies
`Level.Position + ApproachDepthOffset` for both the point and anchor depth.
No gameplay depth, level data, `K`/`ZOffset` animation, or transition timing
was changed.

For a fixed tube rail vertex, let `t = 1/(z+ZOffset)`. Then:

```text
xProjected = 400 + K*x*t
yProjected = (CenterY - a*K*qNear) + K*(y + YOffset + a)*t
```

Both coordinates are affine in the same depth parameter `t`. As depth changes
monotonically between the near and far rims, every intermediate point lies
on the segment between those projected endpoints. The existing portrait
camera is an affine uniform transform, so it preserves that incidence.
Tests capture actual tube endpoints and check all vertex rails at intermediate
depths on closed Level 1 and 6 and open Level 9, for negative, neutral, and
positive coefficients.

At the anchor depth, `q-qNear` is exactly zero. Thus near-rim points retain
their established placement. The term has no X component; horizontal
coordinates remain unchanged. The coefficient is applied at the shared
`Projection.Project` entry used by tube geometry, player/enemy/shot/Spike
anchors, effects, score popups, transition vectors, and projected Game Over
vectors. HUD and screen-space overlays retain their separate camera.

## Calibration semantics and safety

`--portrait-perspective <signed-number>` is parsed with invariant culture,
accepts one finite value, rejects duplicates and values outside `[-5, 5]`,
and reports the valid range. `--portrait-perspective 0` is the neutral value
and reproduces the pre-experiment projection exactly. The value exists only
for the current process and is not part of gameplay or recording metadata.

- **Positive:** farther points move upward on screen, strengthening the
  looking-down-the-tube convergence as a calibration hypothesis.
- **Negative:** farther points move downward, weakening that presentation.
- **Range:** `-5` through `+5`, selected as a conservative bound from
  production Levels 1–9 and the frozen projection values.
- **Default:** `0`. No evidence-supported non-zero default was selected.
- **Manual probes:** neutral `0`, stronger `+2` and `+5`, opposite direction
  `-2`. At the normal far depth 450, `+2` moves the far anchor upward about
  2.82 host pixels, `+5` about 7.05 pixels, and `-2` downward about 2.82
  pixels. These are calibration deltas, not arcade measurements.

With `K=70`, `ZOffset=5`, and the portrait zoom of 1.26, the maximum
normal-depth shift at `a=±5` is about 7.05 host pixels at depth 450. The
projection remains finite over tested production depth, enemy-retraction,
and approach ranges; the approach anchor follows its existing depth offset.
No level, geometry, topology, or screenshot-specific branch was added.

### Levels 1–9 bounds

The table gives host-pixel bounds in the portrait viewport
`x=[280,1000]`, `y=[0,960]`. The displayed tube extrema are the same for each
tested coefficient `-5, -2, 0, +2, +5`: the near rim defines the vertical
extrema and is fixed by the anchor rule. Tests checked every production tube
endpoint at all five values; X bounds are unchanged.

| Level | X bounds | Y bounds at each tested value |
|---:|---:|---:|
| 1 | `[319.27, 960.73]` | `[15.26, 656.72]` |
| 2 | `[381.81, 898.19]` | `[77.80, 594.17]` |
| 3 | `[324.08, 955.92]` | `[20.07, 651.91]` |
| 4 | `[288.80, 991.20]` | `[148.82, 567.37]` |
| 5 | `[303.24, 976.76]` | `[2.09, 675.62]` |
| 6 | `[365.78, 914.22]` | `[0.14, 582.26]` |
| 7 | `[352.95, 927.05]` | `[47.79, 621.89]` |
| 8 | `[319.27, 960.73]` | `[17.55, 659.01]` |
| 9 | `[319.27, 960.73]` | `[292.46, 659.01]` |

All bounds remain inside the viewport. The tightest horizontal margin remains
Level 4's 8.8 pixels per side; the smallest top clearance remains Level 6's
approximately 0.14 pixels; Level 5 remains the lowest extent. The separate
portrait HUD camera and accepted shared placement are unchanged, and no
candidate moves principal tube geometry above its neutral top envelope. The
existing placement nearly reaches the top at Level 6, so this report does not
claim a new numeric blank band or final HUD layout.

### Registration, transitions, and gameplay neutrality

- Intermediate rail incidence passed for every vertex on Levels 1, 6, and 9
  at representative depths between 50 and 450, for coefficients `-5`, `0`,
  and `+5`.
- At depth 250, a player-shot anchor and its equivalent tube-space lane
  midpoint remain aligned within 0.001 host pixels on Levels 1, 6, and 9.
- The approach test confirms the effective near rim remains fixed and that
  rendering does not advance approach depth or transition ticks.
- Existing exit/reverse behavior continues to animate only its established
  `Projection.K` and `Projection.ZOffset` values; the shared formula uses
  those live values without changing simulation-side animation or timing.
- A 240-step paired simulation test with identical input confirms matching
  gameplay state, random-call progression, recording frames, and startup
  metadata under neutral and `+5` presentation values.
- The legacy camera/projection remains the unchanged A/B reference. An
  explicit perspective option with `--legacy-presentation` is rejected
  clearly. Perspective calibration is also rejected for headless analysis
  and the standalone input diagnostic because neither renders the portrait
  playfield.

The file-mode allowlist recognizes the presentation option narrowly for
recording/playback parsing. Recording startup values and schema do not contain
the coefficient. The option composes with valid level/lives/speed/rim-speed,
file recording/playback, seeded visible playback, and ordinary portrait
scenario selectors.

## Changed files

Tracked:

- `src/Tempest.Game/Debug/DebugController.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Rendering/DisplayConfiguration.cs` (existing candidate retained)
- `src/Tempest.Game/Rendering/Projection.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs` (existing candidate retained)
- `tests/Tempest.Game.Tests/DebugControllerTests.cs`
- `tests/Tempest.Game.Tests/DisplayConfigurationTests.cs`
- `tests/Tempest.Game.Tests/GameplayRecordingTests.cs`
- `tests/Tempest.Game.Tests/TransitionPresentationTests.cs`

Ignored AI log material:

- `AI-Logs/Prompts/2026-11-14-whole-game-presentation-shared-perspective-experiment-prompt.md`
- `AI-Logs/Reports/2026-11-14-whole-game-presentation-shared-perspective-experiment-report.md`

No durable behavior documentation was updated pending manual acceptance.

## Validation

Test platform: .NET SDK 9.0.304, VSTest 17.14.1, xUnit 2.9.2.

- Focused:
  `dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --filter "FullyQualifiedName~DisplayConfigurationTests|FullyQualifiedName~TransitionPresentationTests|FullyQualifiedName~DebugControllerTests|FullyQualifiedName~GameplayRecordingTests" --blame-hang-timeout 3m --verbosity minimal`
  - **PASS:** 179 passed, 0 failed, 0 skipped.
- Full suite:
  `dotnet test TempestCSharp.slnx --blame-hang-timeout 3m --verbosity minimal`
  - **PASS:** 1,393 passed, 0 failed, 0 skipped.
- Release:
  `dotnet build TempestCSharp.slnx --configuration Release --no-restore --verbosity minimal`
  - **PASS:** 0 warnings, 0 errors.
- `git diff --check`: **PASS**; no whitespace errors.

No interactive game was launched.

## Remaining questions and Neal's manual comparison

The numeric relationship to ROM `lev_y3d`, the arcade-exact camera model,
and a preferred non-zero default remain unknown. No ROM field was mapped to
this coefficient. The candidate is not accepted until Neal evaluates its
visual presentation and gameplay feel.

Level 9 commands:

```powershell
dotnet run --project .\src\Tempest.Game -- --level 9 --portrait-perspective 0
dotnet run --project .\src\Tempest.Game -- --level 9 --portrait-perspective 2
dotnet run --project .\src\Tempest.Game -- --level 9 --portrait-perspective 5
dotnet run --project .\src\Tempest.Game -- --level 9 --legacy-presentation
```

Composition with existing options:

```powershell
dotnet run --project .\src\Tempest.Game -- --level 9 --lives 8 --speed 0.5 --portrait-perspective 2
dotnet run --project .\src\Tempest.Game -- --playback-file "UserData\GameplayRecordings\your-recording.json" --portrait-perspective 5
```

Reusable Levels 1–8 pattern (replace `N` with the selected level and `VALUE`
with `0`, `2`, `5`, or `-2`):

```powershell
dotnet run --project .\src\Tempest.Game -- --level N --portrait-perspective VALUE
```

Manual checks: stable near-rim and overall placement; available upper gameplay
text region; direction and strength of looking-down perspective; no clipping,
inversion, or mirroring; straight rails and registration of Blaster, enemies,
Spikers, shots, Spikes, effects, and lanes; coherent response across different
tube geometries; unchanged legacy presentation; and unchanged gameplay feel
and timing.

## Final Git state and evidence boundary

Branch remains `master`; `HEAD` and `origin/master` remain
`385f603a94391756b2ca6cccd524dc17b281e782`. The index is clean; the nine
tracked files listed above remain modified, and no non-ignored untracked files
were introduced. AI log files are ignored. Nothing was staged, committed, or
pushed.

No visual assets, Gemini material, Incitti source, web sources, or raw ROM
files/directories were searched, opened, or inspected. Automated validation
is complete; manual calibration acceptance remains with Neal.
