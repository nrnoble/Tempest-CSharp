# Task: Encode accepted first-pass per-level Atari-presentation calibrations

Continue from the current uncommitted presentation-calibration candidate. Neal
manually tested all ten implemented/diagnostic tube presentations with fixed
yellow viewport guides and selected these intentionally good-enough values.
Encode them as the automatic defaults when Atari presentation is active and
none of the corresponding CLI options is supplied:

| Level | Tube Y gain | Tube scale | Vertical offset |
|---:|---:|---:|---:|
| 1 | 1.4 | 0.8 | +35 |
| 2 | 1.0 | 1.1 | +35 |
| 3 | 1.2 | 0.97 | +35 |
| 4 | 1.0 | 1.0 | +35 |
| 5 | 1.0 | 1.2 | +15 |
| 6 | 1.0 | 1.28 | +15 |
| 7 | 1.3 | 0.9 | -25 |
| 8 | 1.0 | 1.0 | -75 |
| 9 | 1.5 | 1.05 | -150 |
| 10 (diagnostic) | 1.2 | 0.9 | +15 |

Resolve calibration from the currently displayed level during direct startup,
normal progression, visible playback, Approach, ExitZoom, death/re-entry, and
Game Over where its existing Atari path applies. Keep explicit
`--tube-y-gain`, `--tube-scale`, and `--tube-vertical-offset` values as absolute
overrides of only their matching dimensions. Preserve permissive finite parser
behavior, legacy presentation, Level 10's diagnostic gameplay role, frozen
ExitZoom focal behavior, deterministic playback, and all existing registration
and screenshot behavior. The values are manually selected first-pass project
decisions, not ROM-derived geometry, and may be fine-tuned in a later milestone.
Do not derive formulas, recenter, normalize, or tune these values.

Keep the accepted fixed yellow guides at host Y=180 and Y=1210 unchanged.
Do not inspect visual assets or `TEMP\myNotes.md`; do not change unrelated
presentation issues, geometry, gameplay, timing, recording schemas, or viewport
layout. Preserve all pre-existing worktree changes and user/reference assets.
Do not stage, commit, push, reset, restore, stash, clean, rebase, or launch the
interactive game.

Add focused coverage for the full table, per-dimension absolute and partial
overrides, legacy behavior, progression and visible multi-level playback,
registration/model immutability, deterministic playback boundaries,
Approach/ExitZoom, screenshot F9/F10 behavior, and fixed guide behavior. Run
bounded focused tests, relevant regressions, the complete solution test suite,
a Release build, and `git diff --check`. Update relevant durable documentation.
Save a paired implementation report under `AI-Logs\Reports\`. Finish with
manual launch commands for Levels 1-10 using only `--atari-presentation
--level N`, one partial override, the full identity comparison
`--tube-y-gain 1 --tube-scale 1 --tube-vertical-offset 0`, and a documented
visible multi-level playback recording path. Do not invent a recording name.
Stop after automated validation for Neal's manual verification.

TEMPEST_ENCODE_FIRST_PASS_LEVEL_CALIBRATIONS
