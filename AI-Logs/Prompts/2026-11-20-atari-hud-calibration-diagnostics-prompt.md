# Atari HUD Calibration Diagnostics Prompt

Date: 2026-11-20

Scope:
- Continue the uncommitted Atari vector HUD presentation-refinement candidate.
- Add development-only command-line controls for independent current-score,
  high-score/initials, level, and reserve-life HUD calibration.
- Preserve all existing refined defaults unless an override is explicitly
  supplied.
- Use the existing 600x800 portrait logical HUD coordinate system.
- Positive X moves right, negative X moves left, positive Y moves down, and
  negative Y moves up.
- Reject malformed, non-finite, duplicate, and Atari-context-invalid options.

Required control groups:
- `--hud-score-x`, `--hud-score-y`, `--hud-score-scale`
- `--hud-high-x`, `--hud-high-y`, `--hud-high-scale`
- `--hud-level-x`, `--hud-level-y`, `--hud-level-scale`
- `--hud-lives-x`, `--hud-lives-y`, `--hud-lives-scale`,
  `--hud-lives-advance`

Each value is an absolute override in separate option/value form. X/Y accept
finite signed invariant-culture values. Scales accept positive finite
invariant-culture values. Lives advance accepts finite signed
invariant-culture values.

Preservation requirements:
- Keep ROM-informed HUD and reserve-Blaster geometry, formatting, colors,
  reserve count semantics, 25-symbol cap, and gameplay state unchanged.
- Keep the portrait viewport, tube calibration, projection, viewport guides,
  ExitZoom, Approach, entity registration, and legacy presentation unchanged.
- Preserve development diagnostics outside the portrait viewport.
- Do not place HUD values in gameplay recordings or alter deterministic
  simulation, playback, pause, or screenshot behavior.
- Legacy presentation remains unchanged and HUD controls require
  `--atari-presentation`.

Validation:
- Add focused parser and vector/HUD calibration tests.
- Run relevant portrait/playback/screenshot/Approach/ExitZoom/tube
  regressions, the complete solution suite, Release build, and
  `git diff --check`.
- Do not launch the interactive game.
- Do not stage, commit, push, reset, restore, stash, clean, or rebase.

Completion token: `TEMPEST_ATARI_HUD_CALIBRATION_DIAGNOSTICS`
