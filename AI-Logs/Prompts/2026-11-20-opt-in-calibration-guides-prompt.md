# Make accepted calibration guides opt-in

Make the existing yellow Atari-presentation calibration guides development-only
and off by default, enabled by `--calibration-guides` together with
`--atari-presentation`. Preserve their accepted host Y positions 180 and
1210, viewport span, fixed coordinates, color, and F9/F10 behavior.

Preserve the manually accepted Level 1-10 first-pass calibration table and the
independently composable `--tube-y-gain`, `--tube-scale`, and
`--tube-vertical-offset` diagnostics. Preserve the existing screenshot workflow,
F12 behavior, gameplay, geometry, projection, viewport, transitions, recording,
and deterministic playback. Do not inspect visual assets or launch the game.
Preserve unrelated work and do not stage, commit, push, reset, restore, stash,
clean, or rebase.

Add focused coverage for default and opt-in guide rendering, exact geometry,
diagnostic independence, accepted calibration values, and F9/F10 compatibility.
Run bounded focused and relevant regression tests, the complete solution test
suite, Release build, and `git diff --check`; follow security and stall rules.
Update durable documentation as necessary. Provide normal Level 1, opt-in,
and combined-diagnostics commands for Neal's manual handoff and stop.

TEMPEST_OPT_IN_CALIBRATION_GUIDES
