# Level 12 ExitZoom calibration-coupling investigation

Date: 2026-11-23

## Scope and evidence

Perform a tightly bounded, read-only investigation of the current production
Level 12 Atari-presentation and ExitZoom transform chain. Neal reports that
Level 12 otherwise plays as expected after its closed-topology correction, but
its exit looks straight-forward instead of following downward toward the deep
end; the Blaster must remain visible throughout the exit.

Compare Level 12 with identity calibration (`1 / 1 / 0`) and Neal's current
candidate (`tube-y-gain 1.3`, `tube-scale .8`, vertical offset `0`) using
genuine row-12 production geometry and deterministic ExitZoom samples. Trace
normal tube rendering, exit tube and Blaster rendering, model-frame mapping,
ROM presentation metadata, K/Z projection, focal compensation, calibration
defaults/overrides, and the final portrait/HUD cameras. Identify the first
semantic or geometric divergence and distinguish the shared ExitZoom
approximation from Level-12-specific visibility.

Review the previously accepted ExitZoom history and representative accepted
closed/open levels without reopening their accepted behavior.

## Constraints

- Read-only except for this prompt and its paired report under `AI-Logs/`;
  do not implement a correction.
- Do not change source, tests, configuration, durable documentation, gameplay,
  presentation constants, or Git state. Do not stage, commit, push, reset,
  restore, stash, clean, or rebase.
- Do not inspect screenshots, video, UserData, Temp, unrelated visual assets,
  or local ROM files. Do not investigate gameplay mechanics or change exit,
  completion, transition, or recording timing/schema.
- Preserve all pre-existing worktree changes and untracked material.
- Do not launch the interactive game. Automated validation supplements, but
  does not replace, Neal's manual acceptance of visual behavior.

## Required report

Answer whether each calibration value participates before, within, or after
the ExitZoom focal calculation. Provide a deterministic A/B numeric comparison
at several ExitZoom progresses, including the Level 12 model source point,
mapped deep-rim center and pivot, projected tube point, Blaster point, and
portrait/HUD handling. Separately state effects on the focal trajectory,
pivot, convergence target, model mapping, tube dimensions/placement, and
Blaster registration/visibility.

Diagnose the Level 12 exit path from source and equations; compare a small
representative set of accepted Levels 1-11; classify the scope as
LEVEL-12-SPECIFIC, GLOBAL EXITZOOM DEFECT, SHARED APPROXIMATION EXPOSED BY
LEVEL 12, or INCONCLUSIVE. Select one implementation-readiness status from
the task's required list and provide a bounded correction only if ready.
Recommend regression tests for calibration isolation, registration, accepted
levels, the Level 12 focal path, and determinism.

Run only the smallest relevant existing automated validation. Record its
outcome, the initial Git state, and any remaining manual acceptance.

TEMPEST_LEVEL12_EXITZOOM_CALIBRATION_COUPLING_READONLY
