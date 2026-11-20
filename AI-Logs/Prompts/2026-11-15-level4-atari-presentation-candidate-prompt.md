# Tempest Level 4 Atari-Presentation Candidate

Implement one tightly bounded, opt-in Level 4 presentation candidate based on
the controlling handoff `AI-Logs/Reports/2026-11-15-level4-geometry-provenance-presentation-mapping-report.md`.
Do not reopen geometry provenance, ROM archaeology, Math Box work, or broad
cross-level mapping; do not use Internet or third-party implementations.

## Scope and constraints

- Extend only the render-time Atari presentation adapter, renderer if narrowly
  required, and their focused tests.
- Preserve Level 4 stored points, point order, lane identities, closed topology,
  gameplay, timing, deterministic playback, and legacy rendering when
  `--atari-presentation` is absent.
- Keep Levels 1-3 accepted outputs exactly unchanged; preserve Level 2's
  `1.314` aspect and `+114` placement. Keep Levels 5-6 on legacy fallback and
  Levels 7-9 unchanged.
- Use Level 4's positive-axis approximate model frame:
  `aX=2.081778520`, `aY=2.231150659`, `xReference=128`,
  `yReference=121.501111`, with forward production point order and no
  reflection, rotation, or lane reindexing.
- Keep C# depth progression `K / (depth + ZOffset)`. Preserve the current
  Level 4 center/placement baseline and shorter-profile distinction; add no
  Level 4 placement translation or visual aspect calibration.
- Apply the mapping to tube points and all world anchors. Use returned
  per-axis local scale only for tube-oriented geometry; leave screen-oriented
  artwork, HUD, overlays, static shots, and unrelated effects unstretched.
- Keep Atari metadata, model-frame conversion, screen placement, optional
  visual aspect calibration, and topology distinct.
- Do not modify projection/display configuration, level data, gameplay,
  controls, recording/playback, screenshots, HUD, Game Over, or durable docs.
- Do not inspect visual assets, launch the game, stage, commit, or push.

## Validation and manual acceptance

Run bounded VSTest commands with `--blame-hang-timeout 2m` for adapter,
renderer/world-anchor, Levels 1-3, Levels 5-9, Level 4/Approach/transition/
playback/recording/screenshot regressions, and the full suite. Run a Release
solution build with zero warnings and errors, `git diff --check`, and final
branch/HEAD/upstream/index/worktree/untracked checks. Report incomplete or
blocked validation accurately.

Do not launch the game. After all automated gates pass, stop for Neal to judge
only whether Level 4 remains properly placed, retains its shorter profile, and
has no obvious new shape failure. Do not request a recording or Level 5 test.
The candidate remains an approximation pending that manual acceptance.

TEMPEST_LEVEL4_ATARI_PRESENTATION_CANDIDATE_20261115
