# Level 7 good/bad visible-runtime A/B analysis

Date: 2026-11-17

## Task

Perform a tightly bounded, read-only A/B analysis of the already captured
Level 7 visible-runtime diagnostics: the visually good recorded-playback
control and the visually bad normal manual run. Treat Neal's two visual
classifications and his observation that Tankers were deliberately allowed
to approach the rim as established evidence. Do not ask for repeat captures.

Use the actual diagnostic files in the worktree and the two immediately
relevant Level 7 runtime-instrumentation and depth-motion reports. Read current
project instructions. Do not inspect `TEMP/myNotes.md`, screenshots, videos,
frames, contact sheets, or other visual assets; do not inspect recordings.

## Scope and constraints

- Read-only analysis only. Do not change production code, tests, configuration,
  instrumentation, gameplay, presentation, recordings, diagnostic files, or
  Git state.
- Do not launch the game or create diagnostic captures.
- Process the large JSONL data incrementally with practical bounds. Preserve
  only aggregate results and representative samples; do not dump the logs.
- Verify run identity and structural completion from embedded metadata before
  interpreting motion.
- Analyze fixed-lane continuous Tanker trajectories first, by deep, middle,
  and near-rim normalized depth. Compare simulation height/ticks, rendered
  frames, elapsed time, mapped anchors, camera-transformed anchors, and
  submitted geometry/extrema. Also analyze simultaneous Blaster motion and
  continuous player shots.
- Compare equivalent trajectory classes, not unrelated object IDs. Account for
  lane-specific record-06 rail length. Separate simulation, mapped position,
  submitted geometry, timing, and input-mode effects.
- Inspect only source paths required to interpret a measured mode/timing
  difference. Do not infer causation from a mode difference alone.
- Preserve Neal's manual observation if the numeric measurements disagree.
  Distinguish facts from logs, manual observation, interpretation, and
  unresolved behavior.
- If submitted geometry remains normal, identify the exact downstream
  observable that the instrumentation does not capture and the narrowest next
  investigation. Do not implement or recommend a motion-model correction.

## Required output and validation

Save this prompt and a paired evidence-rich report in the date-prefixed
`AI-Logs/Prompts/` and `AI-Logs/Reports/` paths. Report run metadata, the
number of qualifying Tanker and shot trajectories, depth-band evidence,
timing and playback-boundary results, the latest measured renderer stage, and
the narrowest next step.

Check that both diagnostic sources remain byte-for-byte unchanged; no
production, test, or configuration files changed; and branch, HEAD, index,
tracked modifications, and pre-existing untracked state remain unchanged.
Do not stage or commit the AI-Logs records. Stop after the report. No new
manual acceptance or repeat capture is requested.

TEMPEST_LEVEL7_GOOD_BAD_RUNTIME_AB_ANALYSIS
