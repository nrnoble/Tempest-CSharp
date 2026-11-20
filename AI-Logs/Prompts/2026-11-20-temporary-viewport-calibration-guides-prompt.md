# Task: Add two temporary viewport calibration guide lines

Continue from the current uncommitted presentation-calibration candidate.
Before rapid per-level tube calibration, add two simple temporary development-only yellow horizontal guide lines.

## Requirements

- Top line at host/viewport Y=180; bottom line at Y=1210.
- Each spans the full width of the existing portrait viewport only, not the surrounding window.
- Use final viewport/screen coordinates. The lines stay fixed when `--tube-y-gain`, `--tube-scale`, or `--tube-vertical-offset` changes. Tube presentation moves relative to them.
- Treat the guides as visual rulers only: no projection, geometry, scaling, positioning, gameplay, model, or calibration-math participation. Do not infer automatic placement or resize/reposition tubes to fit.
- Keep the implementation temporary and development-oriented. Reuse an existing debug/presentation surface; do not redesign the renderer, change viewport dimensions/HUD, accepted presentation constants, gameplay/model geometry, projection/perspective/diagnostic behavior, Approach/ExitZoom math, or deterministic gameplay and recording/playback.
- Show the guides during Atari-presentation calibration, including while paused with F9 and in displayed images captured by F10. Preserve F10 ownership, `UserData\Screenshots`, and existing collision-safe incrementing filenames. Do not redesign screenshot capture.
- Draw exactly two simple, thin, clearly visible yellow lines; add no labels, ticks, other guides, backgrounds, shading, or calibration UI.
- Do not inspect any additional visual assets. The user supplied the reference coordinates.

## Validation and handoff

Add focused coverage proving both fixed Y coordinates, viewport-only horizontal spans, independence from all three tube diagnostics, and no gameplay/model mutation. Preserve and validate existing F9/F10 behavior and run existing presentation diagnostic tests.

Run bounded focused tests, relevant presentation regressions, the complete solution test suite, Release build, and `git diff --check`. Follow project rules for blocked/stalled commands; do not weaken Windows security, retry the same blocked artifact, or launch the interactive game.

After automated validation, give Neal one concise Level 2 launch command using `--atari-presentation --level 2` and the generalized `--tube-y-gain`, `--tube-scale`, and `--tube-vertical-offset` controls. Do not choose calibration values for Neal. Save this prompt and an implementation report in ignored `AI-Logs\Prompts` and `AI-Logs\Reports`. Preserve unrelated worktree changes and user/reference assets. Do not stage, commit, push, reset, restore, stash, clean, or rebase. Stop after automated validation and provide the manual handoff.

TEMPEST_TEMPORARY_VIEWPORT_CALIBRATION_GUIDES
