# Tempest: Level 1 Placement-Only Correction Candidate

Date: 2026-11-15

## Scope and accepted baseline

Implement one Level 1-only presentation-placement experiment. Preserve the
manually accepted corrected Level 1 tube shape and depth relationship exactly.
The remaining observed issue is that the entire presentation sits too high.
Provide substantial visual space above the tube for future persistent game
information while keeping the tube contained and leaving lower margin.

The controlling handoffs are:

- `AI-Logs/Reports/2026-11-15-level1-atari-presentation-failure-diagnostic-report.md`
- `AI-Logs/Reports/2026-11-15-level1-atari-presentation-correction-candidate-report.md`

Treat the second as the controlling candidate state. Do not revisit Atari
arithmetic, Math Box, geometry provenance, or depth mapping.

## Boundaries

- Identify the narrowest candidate-only post-projection placement seam.
- Do not change the frozen shared portrait translation `-33.1`, portrait
  camera, CenterY, or YOffset.
- Apply one uniform vertical translation only to displayed Level 1 while
  `--atari-presentation` is active. X must remain unchanged.
- Keep all tube/world anchors together, including enemies, shots, Spikes, and
  Blaster anchors. Preserve local glyph sizes and the projection Jacobian.
- Preserve Approach composition, model points/order/topology, gameplay, and
  recording/playback formats.
- Do not alter Levels 7-9 numerics, Levels 2-6 legacy fallback, legacy Level 1,
  or Game Over presentation.
- Do not implement HUD elements, viewports, clipping, or renderer partitions.
- Prefer only the existing adapter and its focused tests. Do not modify the
  projection, display configuration, level data, gameplay, input, recording,
  screenshot, transition, or HUD surfaces.

## Visual evidence restriction

If visual reference is necessary, inspect only:

- `UserData/Screenshots/Level-1/live-level-1-tick-000091-001.png`
- `UserData/Screenshots/Level-1/Level-1 Paint-modified legacy with Atari Score with boarders.png`

The Paint image is a rough composition illustration, not a pixel target; its
green lines are conceptual. Do not inspect any other visual assets. If no
single defensible placement can be derived, stop with PARTIAL rather than
inventing multiple tuning constants.

## Required tests and validation

Prove unchanged corrected Level 1 projection before translation; unchanged X,
width, height, near/far scale and center separation, and local Jacobian; one
identical Y delta for tube points and world anchors; unchanged geometry and
gameplay; frozen portrait placement; unchanged Levels 7-9 numerics; Levels
2-6 fallback; legacy Level 1; Game Over; deterministic Approach and repeated
inputs; presentation-independent recording/playback.

Run focused adapter tests, candidate renderer tests, candidate Level 7-9
regressions, relevant Level 1/Approach/playback/recording/screenshot
regressions, the full test suite, a zero-warning/zero-error Release build,
`git diff --check`, and final Git-state verification. Use bounded validation
with hang protection. Never launch the game.

Before repository work, record branch, HEAD, upstream, tracked changes,
untracked paths, and index state. Preserve all prior work. Do not stage,
commit, push, clean, reset, stash, or modify durable project documentation.

Create only this prompt and its paired report under `AI-Logs/`, and do not
stage or commit either. The report must include outcome, starting state,
placement seam and derivation, implementation, shape/world-object/regression
protection, validation results, the manual command below, the first manual
question, and Git state.

If all automated validation passes, provide only this first manual command:

`dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 1 --atari-presentation`

Ask only whether the tube now has reasonable vertical placement and visual
space above while retaining its already-good shape. Do not ask for another
recording, other-level tests, or a broad checklist; do not tune again before
recording Neal's manual observation.

Select exactly one outcome: **READY FOR LEVEL 1 PLACEMENT CHECK**, **PARTIAL**,
or **BLOCKED**. Nothing may be staged, committed, or pushed.

TEMPEST_LEVEL1_PLACEMENT_ONLY_CORRECTION_20261115
