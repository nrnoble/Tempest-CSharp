# Manual acceptance closeout: 1600x1280 window / 960x1280 viewport

Neal manually responded **"Accepted"** to the enlarged-window and portrait-viewport candidate.

Record this acceptance in `TEMPEST-BEHAVIOR-SPEC.md` and update the current
display summary in `TEMPEST-DEVELOPMENT-NOTES.md`. Keep the initial
implementation report unchanged because it accurately records that manual
acceptance was pending when implementation validation ended. Create a paired
acceptance closeout report under `AI-Logs/Reports/`.

The accepted scope is only the physical enlargement: a `1600 x 1280` host
window and centered `960 x 1280` portrait viewport at `(320,0)`, preserving
the exact `3:4` aspect and uniform `4/3` enlargement from `720 x 960`.
Preserve the logical presentation, accepted per-level placements, one-pixel
viewport border, gameplay, input, and recording/playback behavior. Classify
the dimensions as a PROJECT DECISION and the host mapping as a PROJECT
TRANSLATION; do not imply ROM-backed or arcade-observed evidence.

Keep C# versus arcade/MAME screenshot-proportion comparison deferred. Do not
inspect screenshots, images, or videos; do not launch the game; do not stage,
commit, or push. Validate documentation changes with `git diff --check`.

TEMPEST_WINDOW_1600X1280_VIEWPORT_960X1280_IMPLEMENTATION
