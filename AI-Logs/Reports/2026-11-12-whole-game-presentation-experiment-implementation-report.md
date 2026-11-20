# Whole-Game Portrait Presentation Experiment

**Date:** 2026-11-12
**Status:** EXPERIMENTAL CANDIDATE — PENDING NEAL'S MANUAL ACCEPTANCE
**Completion token:** `TEMPEST_WHOLE_GAME_PORTRAIT_PRESENTATION_EXPERIMENT_20261112`

## Starting Git state

- Branch: `master`
- `HEAD`: `385f603a94391756b2ca6cccd524dc17b281e782` — “Close out ROM-backed enemy-shot rendering”
- `origin/master`: the same commit
- Index, tracked worktree, and non-ignored untracked state: clean
- No files were staged, committed, or pushed.

## Scope and evidence

The implementation follows the supplied bounded presentation task. The read-only investigation and label-correction reports were reviewed; the latter controls all image provenance. The only visual asset opened or analyzed was [`TEMP/Comparision of screen demisions.png`](../../TEMP/Comparision%20of%20screen%20demisions.png). **LEFT is current C#; RIGHT is MAME/reference; RIGHT is the target.** No other images, screenshots, recordings, videos, frames, or contact sheets were searched for or inspected. No screenshots were generated, no game was launched, and no ROM/reference files were opened.

The corrected report describes the MAME tube as substantially more prominent in its portrait pane than the C# tube in the landscape pane, with a different vertical extent and shoulder/far-rim relationship. Those are approximate observations from this one composition, not pixel specifications or proof of matching gameplay phases. The values below are provisional **PROJECT TRANSLATION** calibration choices, not accepted behavior, ROM/hardware values, or arcade-exact results.

## Implementation

The existing 800 x 600 logical/projection coordinates, `Projection.K = 70`, `Projection.ZOffset = 5`, and fixed 1280 x 960 host window remain available. A new small [`PresentationConfiguration`](../../src/Tempest.Game/Rendering/PresentationConfiguration.cs) holds the experimental viewport, cameras, playfield transform, and HUD layout.

- Internal portrait space: **600 x 800** (3:4).
- Fitted viewport in the accepted host: **720 x 960**, centered at `(280, 0)`, with **280-pixel side margins** and uniform **1.2** output scaling. The 720 x 960 hypothesis was retained.
- Playfield: one uniform camera at scale **1.05** about projected anchor **(400, 300)**, with internal translation **(0, -78)**. The resulting output camera zoom is **1.26** and its offset is approximately **(640, 386.4)**.
- The scene and portrait HUD each render through the same fitted viewport boundary. Every playfield renderer call in `VectorRenderer.Draw` shares the scene camera, including the tube, Blaster, enemies, shots, Spikes, effects, popups, transition stars, and game-over vectors. This camera-only mapping does not rewrite projected or model coordinates.
- HUD and screen messages use a separate portrait camera. Score, lives, and level anchors are `(20,16)`, `(20,50)`, and `(440,16)` in internal portrait coordinates; established colors/content are retained.
- No depth-dependent adjustment was implemented. The simple scale/translation cannot reverse the shoulder/far-rim ordering visible in the reference; this remains a visual limitation for Neal to assess rather than an unverified projection change.
- Normal startup selects the experimental candidate. `--legacy-presentation` selects the established 4:3 camera and HUD layout for comparison.
- `FitUniformly` supplies a centered, uniform whole-presentation fit for future host dimensions; fullscreen, borderless fullscreen, and user-dragged resizing are not implemented.

The accepted 720 x 960 host viewport and camera were tested geometrically against all nine production layouts. Tube vertices at near and far rims fit inside the viewport; Level 9 is centered and occupies at least 88% of viewport width under the test invariant. This does not constitute visual acceptance of all gameplay objects or effects.

## Gameplay boundary

No level points, lane topology/counts, player/enemy paths, movement, collision, trajectories, spawning, level configuration, score/lives, speed/difficulty, simulation timing, or recording/playback input behavior were changed. The new presentation tests compare original level points and verify production vertex/playable-lane counts. Projection parameters and transition restoration logic were not modified.

## Files changed

- [`src/Tempest.Game/Rendering/PresentationConfiguration.cs`](../../src/Tempest.Game/Rendering/PresentationConfiguration.cs) — new centralized experimental and legacy profiles.
- [`src/Tempest.Game/Rendering/VectorRenderer.cs`](../../src/Tempest.Game/Rendering/VectorRenderer.cs) — shared scene camera, viewport confinement, and independently anchored HUD.
- [`src/Tempest.Game/Program.cs`](../../src/Tempest.Game/Program.cs) — experimental default and `--legacy-presentation` comparison option.
- [`tests/Tempest.Game.Tests/ExperimentalPresentationTests.cs`](../../tests/Tempest.Game.Tests/ExperimentalPresentationTests.cs) — viewport, scaling, HUD, Level 1–9 bounds, Level 9 prominence, and geometry invariants.
- [`TEMPEST-DEVELOPMENT-NOTES.md`](../../TEMPEST-DEVELOPMENT-NOTES.md) — narrowly records the accepted host window and provisional candidate.
- [`AI-Logs/Prompts/2026-11-12-whole-game-presentation-experiment-prompt.md`](../Prompts/2026-11-12-whole-game-presentation-experiment-prompt.md) — preserved user prompt.
- This ignored implementation report.

## Validation

- Focused display, presentation, Level 9, and transition tests: **PASS**, 69 passed, 0 failed.
- Full command `dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --blame-hang-timeout 3m`: **PASS**, 1348 passed, 0 failed, 0 skipped. The Blame collector's “All tests finished running, Sequence file will not be generated” message was informational.
- Solution command `dotnet build TempestCSharp.slnx --no-restore`: **PASS**, 0 warnings, 0 errors.
- `git diff --check`: **PASS**.

An intermediate full-suite run exposed one test-only parallel failure because the new geometry test temporarily changed shared `Projection` statics. The test was revised to use local baseline projection arithmetic without mutating global state; the final full-suite run passed.

## Manual launch commands

Do not launch the game from the coding session. Neal can compare these commands:

```powershell
dotnet run --project src\Tempest.Game -- --level 9
dotnet run --project src\Tempest.Game -- --level 9 --legacy-presentation
```

The first uses the portrait candidate; the second uses the established presentation. The interactive game has not been launched for acceptance.

## Manual acceptance checklist

- [ ] Level 9 tube prominence versus the **RIGHT-side MAME target**; ensure the tube was not made too small.
- [ ] Tube width and height; horizontal centering; vertical placement; black space above and below.
- [ ] No clipping of tube, Blaster, enemies, shots, Spikes, or effects at the portrait viewport edges.
- [ ] Shoulder and far-rim perspective relationship; note whether the missing depth adjustment is material.
- [ ] Blaster, enemies, player shots, enemy shots, Spikes, explosions, popups, and transition effects remain aligned.
- [ ] HUD score, lives, level, and development indicators are readable and do not overlap gameplay.
- [ ] Exit, transition, and reverse-zoom presentation remain visually coherent.
- [ ] Levels 1–8 show no visual regression.
- [ ] Movement, collisions, shot behavior, timing, and gameplay remain unchanged.
- [ ] The internal presentation is suitable for future uniform fullscreen scaling.

## Unresolved questions and final state

The exact reference capture bounds/phases and exact desired tube height, perspective, and HUD placement remain uncertain. Global scale/translation alone cannot express the observed shoulder/far-rim ordering; no depth offset was guessed. Neal's manual review is required before any values or visual behavior are accepted.

Final Git status at closeout:

```text
## master...origin/master
 M TEMPEST-DEVELOPMENT-NOTES.md
 M src/Tempest.Game/Program.cs
 M src/Tempest.Game/Rendering/VectorRenderer.cs
?? src/Tempest.Game/Rendering/PresentationConfiguration.cs
?? tests/Tempest.Game.Tests/ExperimentalPresentationTests.cs
```

The prompt and report are ignored under `AI-Logs/`. No files are staged; no commit or push occurred.
