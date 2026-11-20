# Tempest: Level 6 Fresh-Capture Reconciliation and Presentation Candidate

Date: 2026-11-16

## Objective

Reconcile the fresh current-worktree C# Level 6 screenshot with the active
source/runtime projection. Compare it with the earlier C# screenshot and the
authorized MAME Level 6 reference. Continue to one bounded Level 6 render-only
candidate only if source arithmetic reproduces the fresh near-rim pixels
within raster tolerance.

Use the handoff report
`AI-Logs/Reports/2026-11-16-level6-baseline-visual-calibration-candidate-report.md`
as established evidence. The fresh screenshot is the current-output authority;
do not assume K=65 or K=70 without tracing runtime state.

## Visual authorization

Analyze only these three exact files:

- `UserData/Screenshots/Level-6/Level-6 Legacy cs.png`
- `UserData/Screenshots/Level-6/Level-6 Tube too high.png`
- `UserData/Screenshots/Level-6/Tempest-MAME-frame_8100.png`

Do not enumerate their containing directory or inspect any other image, video,
recording, extracted frame, contact sheet, or visual asset. Do not modify or
generate visual files.

## Scope and preservation

- Presentation-only, displayed Level 6, and opt-in under `--atari-presentation`.
- Preserve legacy Level 6 when the option is absent.
- Do not change Level 6 model geometry, topology, point order, lanes, gameplay,
  timing, simulation, enemies, collisions, score/lives, or deterministic
  recording/playback.
- Preserve accepted Levels 1-5 and Levels 7-9.
- Preserve Approach, HUD, Game Over, pause/stepping, screenshots, and playback.
- Keep current C# depth progression unless the authorized near/far evidence
  conclusively establishes a defect; do not guess obscured far-rim geometry.
- Do not use prior Atari/C# coordinate-correspondence fits as presentation
  gains. Derive factors only in the reconciled live C# projection domain.
- Do not launch the interactive game; do not stage, commit, or push.
- Create only this prompt and its paired report under `AI-Logs/`; do not update
  durable behavior documentation.

If fresh pixels still do not reconcile, stop **PARTIAL** without an image-only
correction. If they do, derive the smallest justified translation/extent
candidate around the established Level 6 presentation anchor, apply placement
after projection/extent treatment, and route the same world transform through
tube and world anchors. Keep intentionally screen-oriented artwork uniform.

## Validation and manual acceptance

If implemented, run Level 6 adapter/renderer tests, accepted-level and
Level 7-9 regressions, relevant gameplay/Approach/transition/playback/
recording/screenshot/CLI/display regressions, the full test suite, Release
build with zero warnings/errors, `git diff --check`, and final Git/worktree/
index verification. Prove geometry and simulation are not mutated, candidate
bounds/margins and HUD clearance meet the measured specification, local
presentation scales are finite-difference correct, depth progression and
screen-oriented artwork are preserved, and repeated rendering is
deterministic. Do not use screenshots as the sole automated oracle.

On successful validation, stop for Neal's manual evaluation and provide only:

`dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 6 --atari-presentation`

Ask only whether the candidate is appropriately placed beneath the HUD and
visually usable enough for normal evaluation of its size, shape, and
perspective. Do not ask for Level 7 testing.

TEMPEST_LEVEL6_FRESH_CAPTURE_RECONCILIATION_CANDIDATE_20261116
