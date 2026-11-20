# Tempest: Level 4 Unit-Domain Correction Candidate

## Scope

Make only the diagnosed unit-domain correction to the opt-in `--atari-presentation` Level 4 candidate. The first candidate was not playable, and no presentation characteristic from it was accepted. The controlling diagnostic classifies the issue as **AFFINE FRAME APPLIED IN WRONG UNIT DOMAIN**: Level 4 points already arrive in C# model units, but the prior candidate divided them by fitted C#-per-Atari correspondence coefficients and sent those Atari comparison displacements into the C# projection without restoring units.

Restore the C# model-unit domain before final projection, or simplify the correspondence round trip to identity. Keep the fitted Level 4 mapping values available only as correspondence metadata, documentation, or validation; make clear they are not render-space gains. The corrected candidate must match legacy Level 4 projection for all 16 points at near/middle/far depths (50, 250, 450), within strict floating-point tolerance.

For the ordinary Level 4 state (`Position=50`, `Depth=400`, `CenterY=380`, `YOffset=-65`, `K=65`, `ZOffset=5`), preserve:

```text
screenX = 400 + x * K / (depth + ZOffset)
screenY = 380 + (y - 65) * K / (depth + ZOffset)
LocalScaleX = LocalScaleY = K / (depth + ZOffset)
```

Keep the existing depth progression, viewport, portrait zoom and shared portrait placement `-33.1`. Add no aspect factor, Level 4 placement, envelope centering, or visual tuning. Do not alter Levels 1-3 accepted behavior, Levels 5-9, Game Over, Approach, or the renderer/adapter architecture beyond the required correction.

## Protected state and investigation limits

Do not change Level 4 geometry, point order, lanes, topology, gameplay, simulation, timing, scoring, shots, enemies, collisions, recording, playback, input, screenshots, transitions, or HUD. Ensure all tube-attached world anchors use the corrected projection and Level 4 depth scale; do not alter local artwork or screen-oriented vectors.

Read the controlling scale/frame diagnostic completely and consult the preceding Level 4 candidate report as needed. Treat current source as the authority for code present. Do not revisit geometry provenance, ROM/Math Box analysis, broad cross-level mapping, or visual calibration. Do not inspect or enumerate any visual assets, request MAME images, search the Internet, or launch the game.

Prefer changes to `AtariPresentationAdapter.cs` and focused adapter/renderer tests. Do not edit projection/configuration/level/gameplay/input/playback/recording sources or durable behavior documentation. If broader changes are necessary, stop with `PARTIAL`.

## Validation and repository protection

Prove point-by-point candidate/legacy agreement, bounds, dimensions, centers, near/far progression and size ratios, local derivatives, placement/depth invariants, unchanged model state, corrected tube/world-anchor units, accepted Levels 1-3, Levels 5-9 fallback/candidate paths, Game Over, Approach, recording/playback, determinism, and explicit invalid-input failures. Use bounded commands with the established VSTest two-minute hang timeout; run focused tests, regressions, the full suite, a Release build with zero warnings/errors, `git diff --check`, and final Git-state verification. A timeout, block, cancellation, hang, or incomplete run is not a pass.

Before work, record branch, HEAD, upstream, worktree modifications, untracked paths, and index state. Preserve all pre-existing work. Do not clean, reset, restore, stash, stage, commit, or push.

Create exactly this prompt and its paired report under ignored `AI-Logs/`; do not create other log files or stage/commit the logs. The report must include outcome, starting state, correction, numerical evidence, protected state, validation/results, deviations, limitations, the manual command, first manual question, and Git state.

## Manual boundary and outcome

Do not launch the game. If all required validation passes, provide only this launch command and ask whether Level 4 is now usable enough for normal presentation evaluation:

```text
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 4 --atari-presentation
```

Do not suggest what visual result to expect, ask for another level, or request screenshots. Numerical legacy equivalence is a corrected baseline, not manual arcade-fidelity acceptance. Choose exactly one final outcome: `READY FOR LEVEL 4 MANUAL RECHECK`, `PARTIAL`, or `BLOCKED`.

TEMPEST_LEVEL4_UNIT_DOMAIN_CORRECTION_CANDIDATE_20261115
