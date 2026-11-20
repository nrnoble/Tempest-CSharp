# Diagnostic Level 10 with genuine Level 10 tube geometry

## Objective

Modify the existing diagnostic Level 10 experiment to use the project's authoritative, actual Level 10 tube geometry while retaining its deliberately Level-7-like gameplay configuration. This is a diagnostic experiment, not an arcade-authentic Level 10 gameplay milestone.

The experiment asks whether the existing Atari-presentation depth-dependent motion defect still occurs when Level 7 gameplay runs on genuine Level 10 tube geometry. The already-confirmed Level 7 control result is historical evidence: do not investigate or reinterpret it, and do not modify Level 7 to address the defect.

## Geometry and gameplay requirements

- Establish Level 10's tube definition from the authoritative existing arcade/ROM-derived project resources and established tube-definition architecture. Implement it using the same representation as other real tubes. Do not alias, call, map, relabel, approximate, or visually fit Level 7 geometry. If the evidence does not uniquely establish the shape, stop and report the conflict rather than guessing.
- Keep the diagnostic's Level-7-like enemy composition and supply, wave/timing and firing configuration, movement policies/programs, Spikes, initialization, and other existing gameplay characteristics wherever possible. Do not research or implement authentic Level 10 gameplay.
- Make only geometry-forced gameplay/routing changes and identify each in the final report. Keep Level 7 frozen and preserve all pre-existing worktree edits and untracked evidence; do not reset, clean, stash, checkout, restore, stage, commit, or push.
- Ensure the Level 10 model/presentation path remains geometrically distinct with `--atari-presentation`; do not substitute Level 7 geometry. This is not a visual-calibration task: provide a clearly visible, playable tube without screenshot tuning. Identify diagnostic/provisional presentation values.
- Preserve direct Level 10 startup and deterministic recording/playback so the same recording can be replayed with Atari presentation off and on, without redesigning recording.

## Scope exclusions

Do not diagnose/correct the Level 7 defect, repeat framebuffer analysis, alter accepted Levels 1-6 or accepted tube presentations, address Blaster exit rendering, implement authentic Level 10 enemies/difficulty/progression or Levels 8/9/11+, refactor broad presentation code, or perform unrelated cleanup. Do not modify framebuffer capture tooling unless strictly necessary for compilation.

## Validation and manual acceptance

Add/update focused tests proving: diagnostic Level 10 uses genuine geometry distinct from Level 7; intended Level-7-like gameplay remains; Atari presentation does not substitute Level 7 geometry; direct startup and recording/playback still work; and Level 7's established behavior is not intentionally changed.

Run bounded focused validation first, then the bounded full regression suite and Release build if permitted, plus `git diff --check`. Follow Application Control and stall self-stop requirements; report blocked, timed-out, canceled, hung, or incomplete checks explicitly, never as passing. Do not launch the interactive game; Neal performs manual acceptance.

After automated validation, provide exact commands to create/record a diagnostic Level 10 run if needed, replay it without `--atari-presentation`, and replay the identical run with `--atari-presentation`. The sole requested manual judgment is whether the characteristic apparent depth-dependent motion defect reproduces on genuine Level 10 geometry with Atari presentation enabled. Do not request visual-fidelity approval.

## Reporting

Create a new paired report; do not rewrite the prior diagnostic Level 10 report or modify durable behavior specifications to imply authentic Level 10 gameplay. The report must state the authoritative geometry source, implemented tube definition, proof it is not aliased to Level 7, intentionally reused Level 7 gameplay, all geometry-forced differences, Atari-presentation handling, files changed, exact validation commands/results, blockers/incomplete items, final Git status, exact manual commands, and manual result `PENDING`. Keep AI-Logs ignored and unstaged.

Completion token: `TEMPEST_DIAGNOSTIC_LEVEL10_GENUINE_GEOMETRY`
