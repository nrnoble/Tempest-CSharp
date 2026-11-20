# Level 7 depth-motion regression-delta investigation prompt

Perform a bounded, read-only investigation of the severe Level 7 depth-motion
regression reported after entity-to-record-06 presentation registration.
Determine the exact before/after rendering-path change using historical
reports, current Git/worktree state, source, tests, and an existing deterministic
production Level 7 playback. Compare old and current presentation coordinates
for the same replayed simulation states; do not rerun gameplay under different
rules or alter the worktree.

The supplied manual evidence is strong: the defect was absent before
registration and appeared after it; enemies appear to slow as they approach
the rim, Blaster shots appear to accelerate toward the deep end, and rim
movement remains normal. Do not dismiss that evidence because the earlier
synthetic fixed-depth calculation predicted constant current anchor speed.
Explain any discrepancy explicitly.

Track complete or sufficiently long production trajectories for a Tanker,
ordinary Flipper if present, Tanker-released Flipper, Spiker, multiple player
shots from first visible tick through removal/deep end, and enemy bullet if
available. Capture depth/state/lane transitions, old/current logical and host
anchors, per-tick displacement, local scale, and representative glyph extrema.
Distinguish anchor position from glyph scale. Include shot spawn and enemy
rim-approach behavior, lane-length differences, camera stages, and Blaster /
Flipper rim controls. Do not inspect `TEMP/myNotes.md`, images, screenshots,
videos, or visual assets, and do not launch the game.

Use the required ignored paired records in `AI-Logs/Prompts/` and
`AI-Logs/Reports/`. Do not modify source, tests, project/configuration files,
durable documentation, gameplay, recordings, protected material, or Git state.
Do not stage, commit, reset, restore, stash, rebase, clean, or switch branches.
Record initial and final Git state. State validation and any limitations
precisely; manual gameplay acceptance remains Neal's responsibility. The
investigation ends with a regression classification and only a technically
justified future correction direction; do not implement a correction.

TEMPEST_LEVEL7_DEPTH_MOTION_REGRESSION_DELTA_READONLY
