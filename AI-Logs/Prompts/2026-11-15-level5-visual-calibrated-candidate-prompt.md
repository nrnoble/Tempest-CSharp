# Tempest: Level 5 Visual-Calibrated Presentation Candidate

Implement one tightly bounded, render-time Level 5 presentation experiment from
the Level 5 baseline and visual-calibration handoffs.

## Controlling evidence

Read completely:

- `AI-Logs/Reports/2026-11-15-level5-presentation-baseline-investigation-report.md`
- `AI-Logs/Reports/2026-11-15-level5-visual-calibration-report.md`

Use current source for the implementation present. Do not reopen ROM, Math Box,
geometry-provenance, broad cross-level, or visual investigations. Do not inspect
any image, video, screenshot, recording, extracted frame, or contact sheet.

The calibration is **PROJECT VISUAL CALIBRATION FROM THE AUTHORIZED MAME
REFERENCE**, not ROM-backed, exact arcade behavior, accepted behavior, or model
coordinates. Implement the first candidate with net X extent scale about
`0.670`, Y extent scale about `0.880`, a horizontally centered near-rim anchor,
and a near-rim anchor about `53.2%` of portrait viewport height (a bounded
downward exception about `3.2%` of viewport height). Apply extent scaling about
the render-time near-rim anchor.

## Scope and preservation

- Implement only Level 5's opt-in candidate under `--atari-presentation`.
- Without the option, Level 5 remains exact legacy output.
- Levels 1-4 are manually accepted and frozen; preserve their exact outputs.
- Level 6 remains legacy fallback. Levels 7-9 and Game Over remain unchanged.
- Keep the candidate render-only and removable.
- Preserve Level 5 model points, point order, lanes, closed topology, model
  configuration, depth progression, gameplay, simulation, timing, scoring,
  input, recording/playback format and behavior, deterministic playback, and
  HUD. Do not change LevelLoader, level/configuration data, Projection,
  DisplayConfiguration, ROM metadata, gameplay, input, recording/playback,
  screenshot infrastructure, HUD, Game Over, or durable project documentation.
- Do not alter the global portrait camera or implement separate HUD/tube
  viewports. Preserve the existing render-only Approach path and transition
  stars.
- Keep tube and all world-positioned anchors aligned: rims, rails, player,
  enemies, shots, Spikes, lane objects, below-rim objects, and computed pivot
  endpoints.
- Apply X/Y local presentation gains to tube-oriented local artwork. Keep
  intentionally screen-oriented artwork and effects undistorted; preserve
  minimum pixel sizes and existing effects. Do not redesign artwork.
- If safe axis distinction requires a broad renderer redesign, stop PARTIAL.
- Do not retune another level or fix unrelated defects.

## Required tests and validation

Add focused tests for Level 5 opt-in versus legacy behavior, unchanged model
geometry/state/depth, anchored X/Y transforms and local Jacobians, lack of
Atari correspondence gains, candidate center and positive normal-HUD
clearance, consistent tube/world-anchor mapping, screen-oriented artwork,
accepted Levels 1-4, Level 6 fallback, Levels 7-9, Game Over, Approach
determinism, recording/playback independence, repeated determinism, and
explicit invalid-value failures. Do not use screenshot pixels as an oracle.

Run bounded tests with hang-timeout protection:

1. Level 5 candidate-focused tests.
2. Renderer/world-anchor tests.
3. Accepted Levels 1-4 regressions.
4. Level 6 fallback and Levels 7-9 regressions.
5. Relevant Level 5 gameplay, Approach, transition, playback, recording, and
   screenshot regressions.
6. The full test suite.
7. Release build with zero warnings and errors.
8. `git diff --check` and final repository/index verification.

Do not launch the interactive game. If all automated validation passes, stop
for Neal's manual check and provide only this launch command:

```powershell
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 5 --atari-presentation
```

First manual question: "Is the Level 5 candidate now appropriately placed
beneath the HUD and visually usable enough for normal evaluation of its shape,
size, and perspective?" Do not suggest a desired appearance, request another
image/recording, or ask Neal to test Level 6.

## Git and authorized artifacts

Preserve the starting repository state and all existing work. Do not clean,
reset, restore, stash, stage, commit, or push. Create exactly:

- `AI-Logs/Prompts/2026-11-15-level5-visual-calibrated-candidate-prompt.md`
- `AI-Logs/Reports/2026-11-15-level5-visual-calibrated-candidate-report.md`

Do not create other AI-Logs artifacts. Keep AI-Logs unstaged and uncommitted.
The report must include outcome, starting state, exact transform and value
provenance, calibrated logical/host bounds and HUD clearance, unchanged model
and gameplay evidence, world-object and local-art treatment, preservation of
other levels, validation, limitations/manual acceptance, and ending Git state.

Completion token:

TEMPEST_LEVEL5_VISUAL_CALIBRATED_CANDIDATE_20261115
