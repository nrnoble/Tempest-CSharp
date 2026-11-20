# Game Over visible-density diagnostic

Add a development-only `--game-over-density <multiplier>` command-line option
to calibrate the unresolved visual translation between the ROM state-`$24`
parent records and the arcade-observed number of visible marks.

The default and explicit `1` must preserve the current presentation exactly.
Keep the ROM record model at 16, and do not alter parent movement, seeded
randomness, spawn/drain timing, GAME OVER text, rendering transition, black
effect presentation, or gameplay. Generate only deterministic, bounded vector
marks local to active parent records; support fractional density without
frame-to-frame flicker and reject invalid option values.

Add focused coverage for parsing, emitted counts/geometry, fixed capacity,
parent trajectories and lifecycle, text, and gameplay-state invariants. Run
focused and neighboring regressions, the full solution tests and build, and
`git diff --check`. Save a report. Do not mark any density accepted, launch the
game interactively, stage, commit, or push.

Neal's initial 4x estimate is a manual-calibration starting point only; no
density other than the existing default is accepted by this work.
