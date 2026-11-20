# Game Over vector final calibration prompt

Perform one narrow visual-calibration pass on the existing uncommitted Game
Over implementation. Keep the current worktree as the foundation, preserve the
accepted GAME OVER vector text, and do not redo the previous 99-frame analysis.

Use only a representative chronological sample of the new clean reference set
at `Videos/Fireworks-MAME` to compare mark prominence and verify the isolated
black-display presentation. Do not modify reference frames or add them as
runtime assets. Record which frames were sampled and whether they materially
contradicted the previous analysis.

Neal reports that the existing two-mark-per-record effect is substantially
successful in structure but that the marks are too dim and/or small. Make the
smallest conservative adjustment within existing vector-rendering conventions.
Do not redesign color, geometry, movement, distribution, spawning, bounds,
drain behavior, seeded determinism, or any ROM-derived mechanics without
concrete contradictory evidence. Keep evidence classified as ROM-backed,
arcade-observed, or project translation.

Neal observes that the tube and all other ordinary presentation disappear
before the effect; while active, the effect appears alone on the black display
and GAME OVER is no longer visible. Implement this as rendering-only visibility,
not by deleting state or changing gameplay. Keep the existing
GameOverPresentation tube/text, terminal Game Over and R-to-restart behavior,
and unconditional C# effect decision after every final one-player death.

Add focused tests for the unchanged Game Over text/presentation, isolated
animation rendering, calibrated primitive size, unchanged mechanics and
determinism, no gameplay mutation, terminal restart, and non-final deaths.
Run focused, neighboring, and full test suites, then the full solution build and
`git diff --check`. Update only the experimental Game Over documentation and
save a report with actual counts.

Do not add leaderboard, initials, credits, coins, attract mode, two-player,
frontend, MAME behavior, CRT effects, screenshots, or videos. Do not launch the
game interactively. Do not stage, commit, or push. Finish with Git status and
the requested manual command/checklist, then stop for Neal's comparison.
