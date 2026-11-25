# Tempest Level 16 Atari-Presentation Calibration Enablement

## Task

Enable the existing Atari-presentation candidate and existing `--tube-*`
calibration controls for production Level 16. This is a bounded
presentation-integration milestone. Level 16 must launch under
`--atari-presentation` and respond to all existing controls so Neal can
manually compare it against MAME/arcade.

Do not choose or freeze accepted Level 16 presentation settings, perform
visual calibration, or change Level 16 gameplay, geometry, enemy behavior, or
shared-center Flipper correction. Do not implement or investigate Level 17+
or use Level 15 as Level 16's presentation model. Do not stage, commit, push,
or launch the interactive game. Stop after automated validation and provide
Neal the manual calibration command.

## Required prior context

Read completely before modifying code:

- `AI-Logs/Reports/2026-11-24-level16-rom-configuration-investigation-report.md`
- `AI-Logs/Reports/2026-11-24-level16-bounded-implementation-report.md`
- `AI-Logs/Reports/2026-11-24-level16-runtime-flipper-pivot-crash-correction-report.md`
- `AI-Logs/Reports/2026-11-24-level16-shared-center-flipper-transition-investigation-report.md`
- `AI-Logs/Reports/2026-11-24-level16-rom-aligned-shared-center-flipper-transition-implementation-report.md`

Treat the existing uncommitted candidate as the working Level 16
implementation. Before editing record branch, HEAD, upstream, staged and
unstaged changes, relevant untracked files, and the current Level 16 candidate
diff. Preserve unrelated work and keep AI-Logs unstaged.

## Scope and implementation requirements

The observed failure is that
`dotnet run --project src\Tempest.Game -- --start-level 16 --lives 12 --atari-presentation`
prints that production support ends at Level 15 and throws
`InvalidOperationException` because the Atari-presentation candidate has no
model-frame mapping for Level 16, through
`VectorRenderer.ProjectWorldPointWithoutExitCompensation`.

1. Add a bounded headless regression that exercises the production Level 16
   mapping/projection path and establishes the missing mapping failure before
   correction.
2. Trace candidate admission and startup message, model-frame mappings, raw
   ROM metadata, displayed-level calibration defaults, runtime overrides, and
   mapping keying. Change only what Level 16 needs.
3. Add the smallest justified mapping for Level 16's exact closed ROM row-11
   geometry. Preserve all 16 ordered lanes, both repeated center vertices,
   self-touching two-lobed geometry, and cyclic topology. Do not require
   nonzero global polygon area, fabricate/simplify geometry, or alter Levels
   1–15 mappings.
4. Ensure all four existing controls affect Level 16:
   `--tube-y-gain`, `--tube-scale`, `--tube-vertical-offset`, and
   `--tube-depth-y-gain`. Preserve parser validation and behavior for Levels
   1–15; impose no aesthetic limits. Use neutral initial values
   `(1, 1, 0, 1)` unless architecture requires otherwise. Label them as an
   initial project translation, not manual acceptance.
5. Update the development-only startup message to production Levels 1–16;
   do not claim Level 17+ support.
6. Add headless presentation tests for model-frame resolution, finite near
   and deep projections, both lobes, repeated centers, shared-center
   Flippers, Tanker cargo, representative entity registration, observable
   override effects, and unchanged gameplay-state geometry. Coordinate tests
   are not visual-fidelity acceptance.
7. Preserve accepted Levels 1–15 presentation mappings/defaults, approach and
   play projection, CLI behavior, direct Level 16 startup, and normal Level 16
   gameplay. Update durable documentation only to say Level 16 is available
   for calibration and awaits Neal's accepted settings. Do not modify
   historical AI reports.

Do not guess calibration defaults, mark Level 16 presentation accepted, or
claim an arcade/MAME visual match. Do not change Level 16 gameplay/geometry,
Flipper transition behavior, or implement Level 17+.

## Validation order and safety

Run in this order: (1) exact missing-mapping regression; (2) Level 16
model-frame tests; (3) all four override tests; (4) Level 16 entity/Flipper
projection tests; (5) Levels 1–15 Atari-presentation regression tests; (6)
relevant Level 16 gameplay regressions; (7) full automated solution suite;
(8) Release build; (9) `git diff --check`.

Use explicit VSTest hang bounds and bounded execution. Do not launch the
interactive game. If a security control blocks validation, do not weaken or
bypass it. If work stalls or required validation cannot complete, report
BLOCKED or PARTIAL rather than claiming success.

## Required artifacts and completion

Save this prompt as
`AI-Logs/Prompts/2026-11-24-level16-atari-presentation-calibration-enablement-prompt.md`
and create
`AI-Logs/Reports/2026-11-24-level16-atari-presentation-calibration-enablement-report.md`.
The report must capture starting/final Git state, observed failure, exact
integration point and architecture/keying, changed files, Level 16 mapping
and geometry handling, supported range/message, each override, initial
neutral values, projection/entity/Flipper coverage, Levels 1–15 and gameplay
regressions, all validation results, unresolved visual questions, and the
manual command template. Confirm no staged/committed/pushed changes, no
Level 17+ work/investigation, no Level 15 presentation model, and no final
calibration values.

Completion requires the headless integration and focused/full validation to
pass with Level 16 available for calibration and Levels 1–15/gameplay
regression-safe. Then stop for Neal's manual calibration using:

```powershell
dotnet run --project src\Tempest.Game -- --start-level 16 --lives 12 --atari-presentation --tube-y-gain VALUE --tube-scale VALUE --tube-vertical-offset VALUE --tube-depth-y-gain VALUE
```

Neal will determine and report accepted values through manual MAME/arcade
comparison.

TEMPEST_LEVEL16_ATARI_PRESENTATION_CALIBRATION_ENABLED
