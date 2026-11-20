# Game Over speck-cluster density diagnostic

Refine the development-only Game Over visual-density experiment using Neal's
manual calibration evidence:

- `--game-over-density 0.5` produced the preferred tiny-speck appearance.
- Higher values using velocity-aligned replication looked increasingly like
  lines, so density must not be increased by extending parent trajectories.

Keep the ROM-backed state-$24 parent model fixed at 16 records, preserving its
coordinates, movement, bounds, spawn cadence, drain, completion and seeded
trajectories. Do not change gameplay, GAME OVER text, effect presentation,
parent dispersion, palette or individual 2.0-logical-pixel mark geometry.

Replace renderer-side line replication with deterministic, compact,
two-dimensional child-speck clusters associated with each active parent.
Retain `--game-over-density` as a child-speck count multiplier, with 1 as the
two-speck baseline and 0.5 as one speck per parent. Preserve arbitrary
positive finite decimal parsing and do not accept a final density value.

Add focused coverage for density counts and determinism, parent invariants,
local 2D offsets, unchanged mark size/color/text/presentation/gameplay, and
run focused and neighboring regressions, full solution tests/build, and
`git diff --check`. Save this prompt/report pair. Do not launch the game,
stage, commit or push.
