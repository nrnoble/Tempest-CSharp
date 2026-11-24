# Development-only window-opacity diagnostic

First inspect the installed Raylib-cs version and supported API. If it does
not expose desktop window opacity, stop before production changes and report
the capability and platform work needed; do not use P/Invoke or a workaround.

Otherwise add `--window-opacity <value>` as startup-only whole-window opacity:
default 1, finite invariant-culture values from 0 to 1 inclusive, reject
missing, malformed, non-finite, out-of-range and duplicate options before
creating a window. Apply the supported raylib call only after window
initialization. Keep gameplay, projection, framebuffer, screenshot and recording
content unchanged; compose with existing interactive CLI options. Reject
headless combinations explicitly; preserve the input diagnostic and playback.
Document the diagnostic and platform limitations without claiming automatic
alignment or unsupported platform guarantees.

Add focused parser, lifecycle, compatibility and independence tests. Run
bounded focused CLI/display/screenshot/calibration/playback/production tests,
the complete solution suite, Release build and `git diff --check`. One bounded
graphical smoke launch is permitted, not gameplay acceptance; do not claim
visual translucency without Neal's manual observation. Provide commands for
opaque, half and quarter opacity and calibration, and document manual checks.
Save a paired report; preserve the accepted Level 14 baseline and local files.
Do not stage, commit, push, reset, restore, stash, clean, rebase or begin Level 15.

TEMPEST_WINDOW_OPACITY_DIAGNOSTIC
