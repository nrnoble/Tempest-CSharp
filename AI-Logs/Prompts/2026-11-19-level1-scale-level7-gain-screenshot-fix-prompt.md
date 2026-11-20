# Task prompt: Level 1 scale, Level 7 gain, paused screenshots

Continue the existing Level 1 / Level 7 presentation calibration candidate.
The existing `--tube-y-gain 1.32` produces an approximately right Level 1 oval,
but the full tube is too large and overlaps the HUD. Preserve that oval ratio
and add a development-only uniform Level 1 presentation scale, defaulting to
identity, which keeps the tube and attached entities registered and never
changes model geometry or simulation state. Do not solve the size issue with
vertical translation or choose a production scale.

Remove the aesthetic upper limit on finite `--tube-y-gain` values so Level 7
can be tested well beyond 1.32. Continue rejecting malformed and non-finite
values. Do not impose a narrow aesthetic bound on the new scale diagnostic;
apply normal positive-finite parser safety and scope the control to Level 1.
These are diagnostic tools, not accepted production constants.

Investigate the paused-screenshot regression against existing project behavior
before changing it. Preserve the established screenshot destination and
incrementing unique filenames, including while paused, and retain the existing
pause/screenshot workflow. Do not redesign screenshot naming or destinations.
Keep screenshot changes separate from presentation mathematics.

Do not alter Level 7 core/far-center, notch, rail convergence, depth or
portrait projection; accepted viewport/HUD; gameplay geometry, enemy behavior,
collision; other levels; or frozen ExitZoom behavior. Do not inspect visual
assets. Preserve unrelated worktree changes and user/reference material.

Add focused coverage for identity defaults, oval-preserving uniform scale,
finite gains beyond 1.32, malformed/non-finite rejection, diagnostic scope,
gameplay-state invariance, shared tube/entity registration, deterministic
playback equivalence, and paused plus ordinary screenshot destinations and
unique filenames. Run focused tests, the full solution test suite, Release
build, and `git diff --check`. Do not launch the game, stage, commit, push,
reset, restore, stash, clean, or rebase.

Save this prompt and a paired implementation report under ignored
`AI-Logs/Prompts/` and `AI-Logs/Reports/`. After automation, provide Neal
manual commands to vary Level 1 scale and Level 7 gain (including widely
spaced finite probes) and verify F9 pause/repeated screenshots. Stop for
Neal's manual acceptance; do not claim calibration values are accepted.

TEMPEST_LEVEL1_SCALE_LEVEL7_GAIN_SCREENSHOT_FIX
