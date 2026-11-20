# Diagnostic Level 10 vertical placement experiment

Continue the tightly bounded Level 10 Atari-presentation placement experiment. The latest manual test confirmed that Level 10's horizontal placement is correct and gameplay remains good, but the tube is still visibly too high. The characteristic depth-dependent enemy-motion slowdown did not reproduce. Preserve that current no-slowdown candidate state.

Move the Level 10 tube downward by an effective visible Y-only adjustment. Trace the full presentation route to ensure the adjustment changes the final rendered tube position. Freeze the current horizontal center: do not change X translation, center, origin, scale, normalization, projection, viewport mapping, geometry coordinates, or margins. If the transform couples X and Y, preserve the effective X center.

Levels 1-2 are composition references only. Do not copy their unrelated geometry, scale, perspective, projection, or horizontal constants. Preserve Level 10's ROM-backed shape-row-9 geometry, open topology, tube shape, center, scale, perspective/depth behavior, gameplay configuration, enemy movement/timing, projection registration, and open-shape mapping. Do not change Level 7, Levels 1-9, simulation/gameplay behavior, or unrelated presentation.

Add focused automated evidence that the actual rendered Level 10 tube moved down relative to the manually tested candidate, its effective horizontal center remains unchanged, and tube/entity registration remains intact. Run bounded focused tests, the full regression suite if possible, a Release build, and `git diff --check`. Do not launch the game, commit, or push.

Save a report under `AI-Logs/Reports/`. Report the changed vertical mechanism and effective values, actual rendered movement, unchanged center, preserved variables, validation results, and the exact command to replay the existing Level 10 recording with `--atari-presentation`. Stop for Neal's manual judgment of both vertical placement/X center and normal gameplay with depth slowdown still absent.

TEMPEST_LEVEL10_MOVE_DOWN_X_FROZEN
