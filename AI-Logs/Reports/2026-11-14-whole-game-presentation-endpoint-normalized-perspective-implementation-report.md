# Whole-Game Presentation Endpoint-Normalized Perspective Implementation

**Date:** 2026-11-14
**Status:** IMPLEMENTED — AUTOMATED VALIDATION PASS; MANUAL ACCEPTANCE PENDING
**Completion token:** `TEMPEST_PRESENTATION_ENDPOINT_NORMALIZED_PERSPECTIVE_20261114`

## Scope and starting Git state

Implemented the endpoint-normalized baseline-referenced correction in the
existing shared projection path. This milestone does not expand the
human-visible coefficient range and does not claim manual or arcade-fidelity
acceptance.

- Branch: `master`
- `HEAD` and `origin/master`: `385f603a94391756b2ca6cccd524dc17b281e782`
- Index: clean
- Tracked worktree: the nine expected candidate files were modified.
- Non-ignored untracked files: none.
- Starting diff: 9 files changed, 1,070 insertions, 39 deletions.

The substantive prompt was copied verbatim to
`AI-Logs/Prompts/2026-11-14-whole-game-presentation-endpoint-normalized-perspective-implementation-prompt.md`.

## Materials inspected

Read the three requested reports:

- `AI-Logs/Reports/2026-11-14-whole-game-presentation-shared-perspective-experiment-report.md`
- `AI-Logs/Reports/2026-11-14-whole-game-presentation-perspective-range-expansion-report.md`
- `AI-Logs/Reports/2026-11-14-whole-game-presentation-perspective-transition-interaction-readonly-report.md`

Inspected the current projection and renderer, active level configuration and
approach context, command-line presentation/playback options, and directly
affected tests. The interaction report's endpoint-normalized formulation was
used; the pointwise stable-baseline candidate was not implemented.

No screenshots, images, videos, gameplay recordings, MAME frames, contact
sheets, Gemini material, Incitti source, web sources, or raw ROMs were
inspected.

## Formulation and context plumbing

The former additive correction used the animated camera directly:

```text
qLive(z) = 1 / (z + ZOffset)
qLiveN   = 1 / (near + ZOffset)

deltaYCurrent = a * K * (qLive(z) - qLiveN)
```

The base projection remains:

```text
xProjected = 400 + x * K / (z + ZOffset)
yBase      = CenterY + (y + YOffset) * K / (z + ZOffset)
```

The replacement uses the live projective fraction and stable baseline
endpoints:

```text
qLive(z) = 1 / (z + ZOffset)
qLiveN   = 1 / (near + ZOffset)
qLiveF   = 1 / (far + ZOffset)

u(z)     = (qLiveN - qLive(z)) / (qLiveN - qLiveF)

q0N      = 1 / (near + 5)
q0F      = 1 / (far + 5)

deltaY   = a * 70 * (q0F - q0N) * u(z)
yProjected = yBase + deltaY
```

The baseline K and ZOffset are named constants. No production near/far values
are hard-coded in the active shared call path. `VectorRenderer.ProjectWorldPoint`
supplies the active configuration's `Position` and `Position + Depth`; in
Approach it adds the established offset to the point and both endpoint depths
and retains the established vertical offset. Tube vertices, rails, player and
shot anchors, enemies, Spikers, Spikes, effects, score popups, transition
geometry, and Game Over vectors all use this helper. The neutral direct
projection path remains available without context; a nonzero perspective
coefficient requires both near and far values and fails explicitly if either
is missing or singular.

ExitZoom and ReverseZoom still use the existing live K/ZOffset and established
world-depth interval. No simulation, timing, transition, gameplay coordinate,
or recording behavior was changed.

## Mathematical and automated results

### Stable equivalence, near rim, and X

At `K=70`, `ZOffset=5`, the live reciprocals equal the baseline reciprocals:

```text
u(z) = (q0N - q0(z)) / (q0N - q0F)
```

Therefore:

```text
70 * (q0F - q0N) * u(z) = 70 * (q0(z) - q0N)
```

and the new correction equals the previous stable correction at every depth.
Focused tests compare this result within `0.0001` logical units for
coefficients `0`, `+5`, and `-5`, Levels 1, 6, and 9, all representative tube
vertices, near/far, intermediate points, and relevant extrapolated depths.
Existing exact neutral-projection tests also pass.

At the near endpoint `u=0`, so the correction is zero and near-rim placement
is preserved. The term has no X component; X projection remains unchanged.

### Projective-line coherence

For a fixed rail vertex, let `q=qLive(z)`. The base X and Y coordinates are
affine in `q`; `u` is also affine in `q`. Thus corrected X/Y remain affine in
the same parameter, and every intermediate point lies on the projected
near/far line. The shared portrait camera is affine and preserves this
incidence.

Tests check near, far, and intermediate depths on closed Levels 1 and 6 and
open Level 9 across stable play, early ExitZoom, the `K=70/ZOffset=-41`
boundary, high-K ExitZoom, representative early/middle/later ReverseZoom
states, full/late Approach offsets, and restored stable play. Maximum tested
perpendicular departure is within `0.001` logical units; intermediate
projective fractions remain on the segment. No clamping is used.

### Transition amplification, continuity, and registration

For in-tube depths, `u` is in `[0,1]`; therefore the correction is bounded by
its stable far-end displacement regardless of animated K or ZOffset. At
coefficient `+5`, the stable far correction is approximately `-5.59` logical
units (about `-7.05` host pixels after the existing portrait scale), with zero
near correction. Focused tests retain that far value at `K=70/ZOffset=-41`,
`K=440/ZOffset=-41`, and representative ReverseZoom camera pairs. Intermediate
correction remains within the endpoint bound. This bounds only the additive
perspective correction; it does not assert that the pre-existing base
transition camera cannot clip.

A deterministic projection-state sweep covers the ExitZoom ZOffset descent,
high-K progression, ReverseZoom restoration, full-to-complete Approach
offsets, and stable restoration. It verifies repeatable same-state output,
near/far endpoint values, and bounded frame-to-frame correction changes.
The formula adds no gates, fades, history, snapshots, or mutable interpolation.
Existing transition tests continue to pass without changes to transition
simulation.

Registration tests compare actual tube rails with Blaster/player endpoints,
player-shot lane anchors, enemy-shot lane anchors, and an enemy-centered
Fuseball anchor across stable and representative exit/reverse cameras. The
Approach test checks the shifted near/far tube endpoints, player endpoint, and
shot anchor against the same active projection context. Tube-space and
gameplay-space anchors remain registered.

### Approach, extended depths, playback, and legacy

Approach uses `near=Position+DepthOffset` and
`far=Position+Depth+DepthOffset`; both are passed consistently with the
shifted point depth. The actual Approach renderer test confirms endpoint and
anchor alignment without advancing transition state.

Finite, coherent extrapolation is covered for enemy retraction depth,
Game Over record depths, player exit depths, and Approach depths. `u` is not
clamped. A singular point denominator, singular near/far interval,
non-finite coordinate, invalid camera K/ZOffset, or missing active interval
when the coefficient is nonzero fails explicitly.

Projection remains a process-level presentation choice. The existing
240-step paired simulation/recording test passes under neutral and `+5`
coefficients; existing playback rewind/forward reconstruction tests pass in
the full suite. Same-state projection is repeatable, and no recording schema
or metadata was added. Existing parser and legacy-conflict tests pass;
`--legacy-presentation --portrait-perspective <value>` remains rejected.

The coefficient range remains `[-5,+5]`, default remains `0`, parser semantics
are unchanged, and no range expansion was made.

## Files changed

Milestone implementation and directly affected tests:

- `src/Tempest.Game/Rendering/Projection.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `tests/Tempest.Game.Tests/DisplayConfigurationTests.cs`
- `tests/Tempest.Game.Tests/TransitionPresentationTests.cs`
- `tests/Tempest.Game.Tests/GameOverDensityDiagnosticTests.cs`

The Game Over test file was added to the existing non-parallel projection
test collection after a full-suite run exposed a race against process-global
projection settings. No gameplay or durable behavior documentation was
changed. Other pre-existing candidate modifications were preserved.

## Validation

Test platform: .NET SDK `9.0.304`, VSTest `17.14.1`, xUnit `2.9.2`.

- Focused Release:
  `dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --configuration Release --filter "FullyQualifiedName~DisplayConfigurationTests|FullyQualifiedName~TransitionPresentationTests|FullyQualifiedName~DebugControllerTests|FullyQualifiedName~GameplayRecordingTests" --blame-hang-timeout 3m --verbosity minimal`
  - **PASS:** 183 passed, 0 failed, 0 skipped.
- Full Release suite:
  `dotnet test TempestCSharp.slnx --configuration Release --blame-hang-timeout 3m --verbosity minimal`
  - **PASS:** 1,397 passed, 0 failed, 0 skipped.
  - One earlier run exposed the shared-static test race noted above; after
    serializing the projection-dependent test, the full suite passed.
- Release build:
  `dotnet build TempestCSharp.slnx --configuration Release --no-restore --verbosity minimal`
  - **PASS:** 0 warnings, 0 errors.
- `git diff --check`
  - **PASS.**

No interactive game was launched. Automated validation does not establish
manual visual or arcade-fidelity acceptance.

## Manual acceptance handoff

Run from the repository root in PowerShell; these commands are provided for
Neal and were not launched:

1. Level 9 neutral portrait:
   `dotnet run --project src\Tempest.Game -- --level 9 --portrait-perspective 0`
2. Level 9 maximum current positive:
   `dotnet run --project src\Tempest.Game -- --level 9 --portrait-perspective 5`
3. Level 9 maximum current negative:
   `dotnet run --project src\Tempest.Game -- --level 9 --portrait-perspective -5`
4. Legacy reference:
   `dotnet run --project src\Tempest.Game -- --level 9 --legacy-presentation`
5. Spike-exit transition check on production Level 6:
   `dotnet run --project src\Tempest.Game -- --level 6 --portrait-perspective 5`
6. Deterministic seeded playback with `+5`:
   `dotnet run --project src\Tempest.Game -- --gameplay-playback --portrait-perspective 5`

Manual checks should focus on no projection explosion or coefficient-induced
extreme far-geometry launch during ExitZoom; no new clipping attributable to
the coefficient; no boundary jump caused by gating; straight rails and
registered Blaster/shots/Spikes; continuous ReverseZoom; coherent successful
exit, Approach, and Approach completion; unchanged neutral stable presentation;
and unchanged legacy presentation.

The current `[-5,+5]` range is not intended to judge the final strength of
stable-play perspective. Manual acceptance here is limited to transition
composition. The human-visible calibration range remains deferred until this
candidate is accepted.

## Final Git state and remaining acceptance

- Branch: `master`
- `HEAD` and `origin/master`: `385f603a94391756b2ca6cccd524dc17b281e782`
- Index: clean
- Tracked worktree: the original nine candidate files plus the directly
  related Game Over projection-test serialization change.
- Final tracked diff: 10 files changed, 1,673 insertions, 88 deletions.
- Non-ignored untracked files: none.
- The prompt and this report are ignored `AI-Logs` records.
- Nothing was staged, committed, or pushed.
- No visual assets or prohibited references were inspected.

**Remaining:** Neal's manual transition-composition acceptance. The stable
human-visible calibration choice and any later range expansion remain
unresolved and outside this milestone.
