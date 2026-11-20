# Task: Final closeout, commit, and push of the accepted Atari-presentation milestone

The accumulated Atari-presentation work has completed automated validation and Neal's required manual acceptance. This is a closeout task. Do not perform further presentation redesign, calibration, investigation, or feature work.

## Accepted state

Treat the current manually accepted Atari-presentation candidate as the implementation to preserve. Neal manually verified all Levels 1-10 using the encoded first-pass per-level defaults and all ten passed.

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
| 10 | 1.2 | 0.9 | +15 |

These are manually accepted first-pass project presentation decisions. Do not fine-tune them or derive replacement formulas from them. Preserve the development-only absolute diagnostic overrides `--tube-y-gain`, `--tube-scale`, and `--tube-vertical-offset`.

Preserve the accepted opt-in calibration guides: off by default; `--calibration-guides` restores two yellow horizontal guides at host Y=180 and Y=1210, spanning the portrait viewport and fixed in final presentation coordinates. Neal manually tested the default-off behavior and confirmed the yellow lines are removed from normal presentation.

Preserve the screenshot workflow: F9 pauses, F10 requests a Tempest-owned screenshot under `UserData\Screenshots` with collision-safe/incrementing filenames, and F12 retains Raylib-native behavior.

Preserve previously accepted Atari-presentation behavior, including Approach, ExitZoom focal behavior, entity/tube registration, playback determinism, and legacy/non-Atari behavior.

## Closeout and safety

Before staging, inspect branch, HEAD, upstream, ahead/behind, index, working tree, and untracked files. Use the actual diff, documentation, tests, and current repository state to identify the complete milestone candidate. Do not stage unrelated or ignored/local/reference/user artifacts, including AI-Logs, UserData, Temp, screenshots, recordings, videos, ROM/reference material, and personal notes. Do not modify or remove unrelated user files.

Durable documentation must state that all ten first-pass defaults passed Neal's manual visual testing; the values are manually selected project presentation decisions, not ROM-derived gameplay geometry; fine-tuning is possible but outside this closeout; the three diagnostic overrides remain available; calibration guides are development-only and off by default; later levels may reuse the tooling; manual gameplay remains the final visual/playability authority.

Run fresh bounded closeout validation before staging: focused presentation/calibration/guide/Approach/ExitZoom/playback/screenshot regressions, the complete solution test suite, Release solution build, and `git diff --check`. Do not launch the interactive game. If validation finds a genuine regression, stop before staging/committing.

After validation, stage only tracked source, tests, and durable documentation in this milestone. Inspect the staged diff and file list; run `git diff --cached --check`; confirm there are no unrelated files or deletions. Stop if attribution is unclear.

If validation and staged-diff inspection pass, commit on the current branch with a concise message such as `Complete accepted Atari presentation calibration`, then push to the configured upstream. Do not force-push, create a branch, rewrite history, reset, restore, stash, clean, rebase, or alter unrelated worktree material.

Report the final commit SHA, message, branch/upstream, push result, validation results, committed file list, intentionally excluded changes/untracked files, index and tracked-worktree status, manual acceptance status, and genuinely deferred presentation issues. Save this prompt and a final closeout report under ignored `AI-Logs/Prompts/` and `AI-Logs/Reports/`; do not stage AI-Logs.

TEMPEST_ATARI_PRESENTATION_FINAL_CLOSEOUT
