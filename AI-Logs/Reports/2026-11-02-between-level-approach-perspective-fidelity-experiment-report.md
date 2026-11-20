# Between-level Approach Perspective-Fidelity Experiment

## Follow-on validation update

The later [starfield presentation implementation report](2026-11-02-between-level-approach-starfield-presentation-implementation-report.md)
supersedes the validation blocker recorded below: ordinary commands now
execute successfully, with 40 combined transition/perspective cases, 398
affected cases, and 635 full-solution cases passing, zero failures/skips, and
a clean solution build/diff check. No security policy was changed or bypassed.
The 96-unit bias is retained. Neal subsequently manually accepted the final
combined presentation; the follow-on report now records final acceptance
and its final 45/398/640 passing validation gate. The initial blocked results
below remain historical, not passing test runs.

## Status / checkpoint

- HEAD remains `15933a85f8ac7e9b1390a5436d092d96774c3213` (`15933a8 Refine accepted Blaster death animation`).
- The bounded rendering experiment and its tests are implemented, but automated test execution is **BLOCKED** by Windows Application Control. This is not a validated or manually accepted checkpoint.
- The solution builds with zero warnings and zero errors. `git diff --check` passes.
- No staging or commit. The pre-existing `.gitignore` modification was left untouched and is not part of this implementation. `Temp/` was not investigated.
- No durable arcade-behavior documentation was changed. The bias is an experimental PROJECT DECISION, not measured Atari geometry or accepted fidelity.

## Exact implementation

Only production file changed: `src/Tempest.Game/Rendering/VectorRenderer.cs`.

- `InitialApproachVerticalOffset = 96` is a pre-projection Y offset.
- `ApproachVerticalOffset = 96 * (ApproachDepthOffset / 240)`.
- The existing depth offset remains `240 * (1 - smoothstep(progress))`, with the unchanged 120-tick lifecycle. Thus the new vertical input follows that same smoothstep envelope, with no second timer.
- A small private `ProjectApproachGeometry` helper delegates to the existing `Projection.Project`, supplying `depth + ApproachDepthOffset` and the level's normal `YOffset + ApproachVerticalOffset`. Projection math is not duplicated.
- The tube's near/far rims, Blaster edge, and lane-centered geometry use that helper. This includes the incoming Level 4 starting spikes, which remain visible during approach: they now receive the same depth offset and vertical bias as the web rather than appearing at an unrelated projection.
- Outside Approach, both offsets are zero. The normal gameplay projection, projection globals, gameplay coordinates, display configuration, and input/lifecycle code are unchanged.
- The Blaster retains its existing claw-height scaling and spinner movement path. The vertical translation is uniform within each rim plane, so it does not change that rim's shape, edge lengths, or Blaster-edge direction.

## Magnitude and curve

The positive pre-projection offset places the tube slightly lower relative to the unchanged viewing center, with a larger displacement for closer geometry. It is intended as a small elevated-viewpoint cue, not a literal pitch rotation.

For remaining approach depth `d`, a point at normal depth `z` receives the screen-space vertical displacement:

```text
96 * (d / 240) * 70 / (z + d + 5)
```

At approach start, the near rim (`z = 50`) shifts about 22.78 logical pixels and the far rim (`z = 450`) about 9.67 logical pixels. At the accepted 1.6 display scale these are approximately 36.45 and 15.47 physical pixels. The displacement is strongest initially and decreases monotonically for these positive depths, with smooth start/end easing. It is exactly zero at arrival.

This leaves horizontal growth, per-rim dimensions, the 240-unit depth start, the smoothstep depth curve, and the 120-tick duration unchanged. The effect changes vertical positioning and the relative near/far framing only. Neal must judge whether the result improves viewpoint travel or merely looks like tube movement.

## Files changed

Tracked implementation/test changes:

- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `tests/Tempest.Game.Tests/LevelApproachTests.cs`
- `tests/Tempest.Game.Tests/BlasterLaneHighlightTests.cs`

Ignored milestone records:

- `AI-Logs/Prompts/2026-11-02-between-level-approach-perspective-fidelity-experiment-prompt.md`
- `AI-Logs/Reports/2026-11-02-between-level-approach-perspective-fidelity-experiment-report.md`

The unrelated existing `.gitignore` change still appears in worktree status; it was neither edited nor staged.

## Tests added / adjusted

- Exercise natural incoming Levels 2, 3, and 4 at every tick from start through arrival.
- Protect the exact existing depth curve and 120-tick duration, finite deterministic geometry, positive bias before arrival, monotonic diminishing pre-/post-projection bias, and small smooth endpoint changes.
- Check that X coordinates retain the accepted depth projection, and that both rims use the expected vertical projection with stronger near-rim parallax.
- Check Blaster edge registration to the corresponding rim vertices while spinner input advances the player.
- Check exact zero offsets and ordinary projection at arrival, frozen Eggs through arrival, and ordinary wave advancement only on the next step.
- Cover absence of bias in Playing, ExitDelay, ExitZoom, ReverseZoom, PlayerDying, and Complete, even if an approach model remains present.
- Check Level 4 starting-spike endpoints and tip markers use the same presentation transform throughout approach and at arrival.
- Update the lane-highlighting approach test's expected geometry.
- Existing freeze, input, natural progression, display, and rendering tests remain in place. No tests claim that the chosen value is arcade accurate.

## Automated validation results

Detected .NET SDK 9.0.304, xUnit v2 and VSTest. No MTP settings were found in applicable build configuration.

| Validation | Result |
|---|---|
| Focused LevelApproach tests | BLOCKED before discovery; no tests executed |
| Affected progression/rendering tests | BLOCKED before discovery; no tests executed |
| Full solution tests | BLOCKED before discovery; no tests executed |
| Solution build | PASS: 0 warnings, 0 errors |
| `git diff --check` | PASS; only Git line-ending conversion advisories |
| Tracked implementation diff review | Reviewed; production changes confined to the renderer |
| Index | Unchanged; nothing staged |

All three test commands reported that Windows Application Control blocked loading `Tempest.Game.Tests.dll` with error `0x800711C7`. The test runner returned exit code zero despite executing no tests; that is **not** a passing result. No policy settings were changed or bypassed. Test execution must succeed in an authorized environment before treating automated validation as complete.

Commands used from the repository root:

```powershell
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~LevelApproachTests"
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-build --no-restore --filter "FullyQualifiedName~LevelProgressionTests|FullyQualifiedName~LevelConfigurationTests|FullyQualifiedName~BlasterLaneHighlightTests|FullyQualifiedName~DisplayConfigurationTests|FullyQualifiedName~Level4CandidateTests|FullyQualifiedName~SimulationTests"
dotnet test TempestCSharp.slnx --no-build --no-restore
dotnet build TempestCSharp.slnx --no-restore
git diff --check
```

## Manual comparison pending

Automated validation has not passed because of the policy blocker. No graphical acceptance is claimed. Once that blocker is resolved and tests pass, use the normal campaign command from the repository root:

```powershell
dotnet run --project src\Tempest.Game -- --lives 10
```

Compare against the accepted checkpoint's presentation and actual arcade/MAME at the same display settings, preferably across all natural approaches (Level 1 to 2, 2 to 3, and 3 to 4):

1. Does the distant tube appear to be approached from somewhat above?
2. Is the player/viewpoint-travel impression stronger?
3. Does tube growth remain smooth and natural?
4. Is the Blaster convincingly attached to the rim?
5. Does spinner movement look natural?
6. Is arrival into normal gameplay completely smooth?
7. Is there any arrival snap or vertical jump?
8. Any projection flash?
9. Any momentary wrong board?
10. Any new Blaster jump?
11. Is the result clearly better than the previously accepted approach?

Also retain the accepted no-premature-gameplay/input restrictions, campaign progression, and 1280x960 scaling.

If #11 is no, retaining the existing accepted presentation is preferable. Do not escalate to more complicated corrections or describe this experiment as accepted without Neal's evaluation.
