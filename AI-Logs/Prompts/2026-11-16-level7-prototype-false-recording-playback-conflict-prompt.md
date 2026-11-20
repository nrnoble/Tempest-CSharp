# Level 7 prototype false recording/playback conflict

Correct the false recording/playback conflict that prevents the completed
development-only Level 7 direct-Atari wireframe prototype from launching for
Neal's manual validation.

Trace argument parsing, option validation, recording/playback state detection,
and startup selection to establish the cause. Permit
`--level 7 --atari-tube-prototype` when no recording or playback was requested,
while preserving rejection of genuine prototype + recording and prototype +
playback combinations and existing option conflicts.

Keep the change within command-line/runtime option handling and narrowly
targeted regression tests. Preserve all pre-existing work. Do not change
gameplay, geometry, Atari source data, rendering or calibration; do not launch
the interactive game; do not stage, commit, or push.

Validate the focused tests, relevant broader regression tests, full suite and
Release build if practical, and `git diff --check`. Report validation outcomes,
Git/worktree state, the manual command for Neal, and leave visual acceptance to
Neal.

TEMPEST_LEVEL7_PROTOTYPE_FALSE_RECORDING_PLAYBACK_CONFLICT_FIX
