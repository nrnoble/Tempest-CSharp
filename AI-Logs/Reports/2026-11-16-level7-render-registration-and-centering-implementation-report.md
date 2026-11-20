# Level 7 render registration and centering implementation

Date: 2026-11-16
Status: **IMPLEMENTATION CANDIDATE COMPLETE; MANUAL ACCEPTANCE PENDING**

## Outcome

Implemented an opt-in, ordinary-Playing-only Level 7 presentation registration
candidate. Gameplay entities now use the same mapped Atari record-06 lane
geometry as the visible tube, with one shared translation centering the whole
composition below the persistent HUD. The simulation, collision geometry,
recording/playback data, and existing record-06 shape transform remain
unchanged.

This is a **PROJECT TRANSLATION** candidate, not an accepted fidelity change.
Neal's manual gameplay/presentation acceptance remains **PENDING**.

## Files changed for this task

- `src/Tempest.Game/Rendering/AtariLevel7Presentation.cs`
  - Preserved the existing uncentered `MapNear` / `MapFar` functions for
    prototype and reference-shape diagnostics.
  - Added centered map accessors and shared cached mapped near/far endpoints
    for ordinary Level 7 presentation.
  - Computes one shared post-shape logical translation from mapped tube bounds,
    the active playfield camera, and the HUD-cleared composition bounds.
- `src/Tempest.Game/Rendering/AtariLevel7EntityPresentation.cs`
  - Added the Level-7-only lane/depth registration authority, geometry basis,
    local scale, and endpoint-hinge Flipper transition mapping.
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
  - Routed ordinary Playing entities, Blaster rim geometry, tube/lane
    highlighting, pulsing lane boundaries, and lane-based effects through the
    shared record-06 presentation geometry.
- `tests/Tempest.Game.Tests/AtariLevel7EntityPresentationTests.cs`
  - Added focused interpolation, centering, extrapolation, transition,
    representative-anchor, and render-state-preservation tests.
- `tests/Tempest.Game.Tests/AtariTubeRecordPrototypeTests.cs`
  - Updated the candidate-tube expectation to include the new shared
    translation; prototype/reference-shape assertions remain uncentered.
- `tests/Tempest.Game.Tests/VectorRendererAtariPresentationTests.cs`
  - Updated the Level 7 candidate-tube expectation to use centered endpoints.
- `AI-Logs/Prompts/2026-11-16-level7-render-registration-and-centering-implementation-prompt.md`
  - Saved a copy of the complete substantive user task.

No durable behavior-specification or development-notes change was made; this
candidate still requires manual acceptance.

## Adapter and mapping

- The adapter reads the existing gameplay lane and projected depth. It does
  not write game, model, enemy, projectile, effect, or recording state.
- Depth is normalized as
  `t = (projectedDepth - configuration.Position) / configuration.Depth`,
  where projected depth remains `Position + Depth - height`. Thus near/rim is
  `t=0` and far/web is `t=1`; `t` is not clamped.
- Each lane uses record vertices `i` and `(i+1) mod 16`, interpolating each
  mapped near point toward its same-index mapped far point. The lane center,
  normalized tangent, perpendicular normal, and local scale are derived from
  those visible endpoints.
- Local scale is the current mapped lane-edge length divided by that same
  lane's mapped near-rim edge length.
- `MapGameplayLaneToPresentationLane(i)` explicitly implements identity
  correspondence (`i -> i`). This remains an unverified initial
  **PROJECT TRANSLATION**, not an established Atari lane correspondence.
- Record 06 remains the source for visible presentation only. The production
  C# geometry remains authoritative for simulation and collision.
- Egg `t>1` remains a bounded **DEVELOPMENT-ONLY** candidate: linear endpoint
  extrapolation preserves the current below-far depth relationship without
  clamping or changing Egg state.
- Flipper pivot endpoints are calculated on the mapped record polygon using
  the existing transition, turn progress, and spatial sweep state.

## Centering and representative probes

The shared translation is calculated from the current mapped bounds and
playfield camera, centering the complete tube in the area below the persistent
HUD. It evaluates to approximately **(+97.47, +78.19) logical units** (about
**(+122.81, +98.52) host pixels** at the current 1.26x camera zoom). The
translation is added equally to every mapped near and far endpoint and is
therefore shared by all registered entities. Automated tests verify the
centered host bounds and preservation of the uncentered record shape,
near/far rails, and relative far-set geometry.

The investigation's lane-0 logical probes before -> candidate after are:

| Probe | Old C# entity position | Old record position | Centered record position |
|---|---:|---:|---:|
| Near rim (`t=0`) | `(199.68, 374.08)` | `(405.97, 177.13)` | `(503.44, 255.32)` |
| Upper (`t=.25`) | `(316.53, 282.88)` | `(384.18, 236.00)` | `(481.65, 314.19)` |
| Mid-depth (`t=.5`) | `(347.28, 258.88)` | `(362.40, 294.87)` | `(459.87, 373.06)` |
| Deep end (`t=1`) | `(369.65, 241.42)` | `(318.83, 412.61)` | `(416.30, 490.80)` |
| Starting Spike tip (`t=.85`) | `(365.22, 244.88)` | `(331.90, 377.29)` | `(429.37, 455.48)` |
| Initial Egg (`t=1.5`) | `(378.69, 234.37)` | `(275.26, 530.35)` | `(372.73, 608.54)` |

The candidate values are checked against the accepted mapping and shared
translation with 0.1 logical-unit tolerance. They are implementation probes,
not arcade measurements.

## Routed and unchanged presentation

Routed through the adapter in ordinary Level 7 Playing:

- Blaster near-rim edge anchor and basis;
- ordinary and Tanker-cargo Flippers, including active pivots;
- Tankers, Eggs, and Spikers;
- starting/generated Spike base and tip;
- player and enemy shots;
- enemy destruction effects;
- player-lane highlighting and pulsing lane boundaries.

Optional Pulsar and Fuseball body rendering remains on its existing path for
synthetic scenarios; these are not part of normal Level 7 composition.
Player-death presentation remains outside the Playing-only registration.
Screen-oriented Egg, shot, and effect artwork remains screen-oriented.

## Preservation and validation

- Record-06 catalog data/metadata, record selection, projector, near/far
  runtime positions, accepted gains, far-set offset, topology, and point order:
  unchanged. Raw ROM files and visual assets were not opened or modified.
- Production Level 7 geometry, gameplay, collision, RNG, timing, and model
  state: unchanged. Renderer projection is read-only.
- Recording/playback formats and state: unchanged; existing recording tests
  pass.
- Other levels, option-off rendering, Approach, exit, transition, and
  Game-Over selection remain on their existing paths.
- Direct Atari prototype/reference-shape mapping remains uncentered and
  unchanged. The factor-128 diagnostic was not changed.
- Focused registration suite: **PASS**, 11 passed.
- Targeted presentation/gameplay regression selection (Level 7 registration,
  candidate and prototype mapping, adapter, shots, enemy shots, destruction
  effects, recording/playback, transitions/Approach, player death, simulation):
  **PASS**, 597 passed.
- Targeted Level 7 gameplay/entity regression selection (Level 7 production,
  Flipper movement, admission and Spiker firing; Tanker cargo; neighboring
  Level 4/5 entity regressions; destruction effects; playback):
  **PASS**, 195 passed.
- Full solution tests using VSTest `--blame-hang-timeout 5m`: **PASS**, 1,574
  passed, 0 failed.
- Release solution build: **PASS**, 0 warnings, 0 errors.
- `git diff --check`: **PASS**, exit code 0. Git emitted only existing
  line-ending notices for `LevelLoader.cs` and `InputSystem.cs`.

## Git/worktree state

Starting state: branch `master`, HEAD
`385f603a94391756b2ca6cccd524dc17b281e782`, tracking `origin/master`; index
empty. The worktree already contained 17 modified tracked files and the
untracked Atari-presentation candidate files/tests, reference documents, and
`screenshot000.png`. `AtariLevel7Presentation.cs` was pre-existing candidate
work and was preserved/integrated, not attributed to this task.

Ending state: same branch, HEAD, upstream, and empty index; no stage, commit,
or push was performed. The pre-existing modified and untracked paths remain.
This task added `AtariLevel7EntityPresentation.cs` and
`AtariLevel7EntityPresentationTests.cs`; it also updated the already-existing
candidate renderer and its tube expectation tests. Protected reference files
and screenshot were not inspected or changed. AI-Logs remain ignored and
unstaged.

## Limitations and manual acceptance

- Identity lane correspondence and the Egg extrapolation presentation are
  candidates requiring Neal's visual review.
- Visible-geometry scale uses the lane-width ratio; final perceived sizing,
  orientation, and presentation remain subject to manual acceptance.
- Optional synthetic Pulsar/Fuseball bodies are not registered.
- No screenshot or interactive game was used for validation.

Manual acceptance status: **PENDING**.

Exact manual test command:

```powershell
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation --level 7
```

Neal should evaluate whether the tube is centered below the HUD; the Blaster
sits on the visible rim; Tankers and ordinary/Tanker-released Flippers follow
visible lanes; Eggs appear at the visible deep end; starting Spikes and both
shot types follow visible lane/depth paths; entity orientation and scale look
coherent; gameplay behaves exactly as before; and the retained record-06 tube
shape remains unchanged.

Saved prompt:
`AI-Logs/Prompts/2026-11-16-level7-render-registration-and-centering-implementation-prompt.md`

Saved report:
`AI-Logs/Reports/2026-11-16-level7-render-registration-and-centering-implementation-report.md`
