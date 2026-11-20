# Tempest: Read-Only Level 5 Visual Calibration

Date requested: 2026-11-15
Execution date: 2026-11-16

## Objective

Use the completed Level 5 presentation-baseline investigation and exactly one
authorized MAME Level 5 frame to decide whether there is sufficient evidence
for one bounded, render-only Level 5 presentation candidate. Record measured
image bounds, normalized proportions, apparent center, HUD interaction,
comparison with the source-derived C# baseline, aspect determination, one
candidate specification or an insufficient-evidence disposition, decision-
relevant unknowns, and exactly one smallest next milestone.

## Controlling material

Read completely and treat its source/math conclusions as established:

`AI-Logs/Reports/2026-11-15-level5-presentation-baseline-investigation-report.md`

Do not redo its source investigation, reopen ROM/Math Box/coordinate-provenance
or gameplay questions, or infer gameplay geometry from the image. Keep accepted
Levels 1-4 frozen. MAME is a practical visual reference, not a pixel-perfect
specification or ROM-backed evidence.

## Complete visual authorization

Inspect only this exact image:

`C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\Level-5\Tempest-MAME-frame_5880.png`

Do not enumerate its containing directory or inspect, compare, search for, or
analyze any other visual material. Do not request another image or create,
modify, or generate visual files.

Measure the image and the tube/HUD bounds, margins, normalized extents, visual
center, separation, and visible line overlap. Distinguish the tube from HUD
elements and temporary gameplay objects. Estimate the far rim only where
measurable and state uncertainty. Compare normalized image measurements with
the report's legacy and common-center Level 5 calculations, including the
full-envelope HUD-clearance constraint. Determine whether the evidence favors
full extent, placement exception, uniform reduction, axis-specific adjustment,
or insufficient evidence. Do not transfer raw image pixels into C# constants
without reconciling coordinate systems.

If sufficient, derive exactly one minimum render-only presentation candidate.
Classify every numeric value as existing accepted C# presentation, source/math
constraint, normalized MAME calibration, or project translation. Do not
change model points/order, lanes, topology, gameplay, depth progression,
recording/playback, or Levels 1-4.

Select exactly one disposition:

- READY FOR BOUNDED LEVEL 5 CANDIDATE
- READY FOR TRANSLATION-ONLY CANDIDATE
- READY FOR SCALE-AND-PLACEMENT CANDIDATE
- READY FOR ONE AXIS-SPECIFIC CANDIDATE
- INSUFFICIENT VISUAL EVIDENCE

For each remaining unknown, assess whether it could change the disposition,
candidate architecture, model/gameplay, HUD/center handling, or first manual
test. Recommend exactly one smallest next milestone and do not recommend
further analysis for nonblocking unknowns.

## Hard scope and repository safety

This is READ-ONLY. Do not modify source, tests, geometry, configuration,
documentation, reference material, or Git state. Do not implement the
candidate, launch the game, run gameplay, search the Internet, build, run a
test suite, stage, commit, push, clean, reset, restore, or stash. Preserve all
existing work and record the starting Git branch, HEAD, upstream/tracking,
index, tracked worktree, and relevant untracked state.

Create exactly these two artifacts, and no others:

- `AI-Logs/Prompts/2026-11-15-level5-visual-calibration-prompt.md`
- `AI-Logs/Reports/2026-11-15-level5-visual-calibration-report.md`

Do not stage or commit either artifact. The report must include the selected
disposition, authorized visual evidence, measurements, center and HUD analysis,
baseline comparison, aspect analysis, candidate specification, unknowns,
smallest next milestone, source/handoff material, calculations, starting and
ending Git state, and manual-acceptance status. State explicitly that Neal's
manual visual acceptance has not been performed.

TEMPEST_LEVEL5_VISUAL_CALIBRATION_READONLY_20261115
