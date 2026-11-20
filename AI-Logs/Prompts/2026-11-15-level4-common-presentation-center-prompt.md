# Level 4 common presentation-center candidate

Implement a Level 4-only placement correction for opt-in `--atari-presentation`.
Establish the accepted Levels 1-3 visual presentation center from their current
outputs and underlying presentation anchor, not arbitrary bounding-box averages.
If a defensible common center cannot be established, or HUD clearance conflicts,
stop with PARTIAL. The corrected legacy-equivalent Level 4 projection is the
shape and depth baseline; apply only the uniform global placement necessary to
align its anchor. Preserve its extent, depth, local scales, topology and model,
and consistently translate all tube-attached world points. Preserve accepted
Levels 1-3, fallback Levels 5-6, Levels 7-9, gameplay, legacy output, camera,
HUD, Approach, recording/playback, transitions and Game Over.

Use source calculations only: no image/video/recording inspection. Add focused
tests for the common center, unchanged outputs and all listed geometry,
world-anchor and deterministic behavior invariants. Run focused tests,
renderer/world-anchor and cross-level regressions, relevant gameplay and
playback/recording/screenshot/transition regressions, the full suite, Release
build with zero warnings/errors, `git diff --check`, and final Git verification;
use bounded tests with established hang-timeout protection. Report incomplete
validation honestly. Do not launch the game, update durable documentation,
stage, commit, push, clean, reset, restore or stash. Preserve existing work.

Create only this prompt and its paired dated report under AI-Logs. Report the
starting and ending Git state, center evidence, pre/post anchor, delta, HUD
clearance, preservation evidence, validation, limitations, and manual command.
After successful validation, provide only the Level 4 candidate launch command
and ask whether placement is appropriate; do not request fine perspective or
size judgment. Outcomes: READY FOR LEVEL 4 CENTER CHECK, PARTIAL, or BLOCKED.

TEMPEST_LEVEL4_COMMON_PRESENTATION_CENTER_20261115
