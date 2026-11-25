# Tempest Level 16 Atari-Presentation Blaster Interior-Side Crash Investigation and Bounded Correction

## Task

Investigate and correct the Level 16 Atari-presentation runtime crash in
Blaster claw orientation. This is a bounded presentation-path investigation;
do not recalibrate Level 16 or change ROM geometry, gameplay, shared-center
Flipper correction, or Levels 17 and later. Preserve Level 16 row-11 geometry,
all 16 lanes and cyclic topology, repeated center points 0 and 8, the current
model-frame mapping, neutral calibration and all four `--tube-*` controls,
`$24` Flipper routing, shared-center Flipper projection, gameplay state,
normal presentation, and Levels 1–15 Atari presentation.

Do not use Level 15 as a Level 16 model. Do not select or encode final Level 16
calibration values. Do not stage, commit, push, or launch the interactive game.
Stop after the smallest evidence-backed correction and bounded automated
validation for Neal's manual calibration retest.

## Required prior reports

Read completely before investigation:

- `AI-Logs/Reports/2026-11-24-level16-rom-configuration-investigation-report.md`
- `AI-Logs/Reports/2026-11-24-level16-bounded-implementation-report.md`
- `AI-Logs/Reports/2026-11-24-level16-runtime-flipper-pivot-crash-correction-report.md`
- `AI-Logs/Reports/2026-11-24-level16-shared-center-flipper-transition-investigation-report.md`
- `AI-Logs/Reports/2026-11-24-level16-rom-aligned-shared-center-flipper-transition-implementation-report.md`
- `AI-Logs/Reports/2026-11-24-level16-atari-presentation-calibration-enablement-report.md`

Treat the current uncommitted candidate as the working Level 16 implementation.
Before editing, record branch, HEAD, upstream, staged/unstaged state, relevant
untracked files, and the current Level 16 candidate diff. Preserve unrelated
work and leave AI-Logs unstaged.

## Supplied failure evidence

Neal launched:

```powershell
dotnet run --project src\Tempest.Game -- --start-level 16 --atari-presentation
```

Startup reported `DEVELOPMENT ONLY — Atari presentation candidate enabled for
production Levels 1-16.` The application initialized, then threw
`System.InvalidOperationException: The Blaster rim edge has no determinable
interior side from its presentation rails.` The stack ran through
`VectorRenderer.GetOutwardBlasterClawOffset`, `DrawPlayer`, and `Draw`.
The model-frame mapping resolves; this is neither the previous missing-mapping
exception nor the prior Flipper transition crash.

## Investigation and implementation requirements

1. Reproduce deterministically and headlessly through production
   `DrawPlayer`/`GetOutwardBlasterClawOffset`, using direct-start Level 16,
   Atari presentation, its initial Blaster rim state, and actual production
   points. Record lane, near/far projected rail endpoints, current claw offset,
   all interior-side candidates, dot/cross values and tolerances, repeated
   center adjacency, and whether the side test sees both sides alike or is
   numerically indeterminate. Determine every failing lane. The permanent
   regression must exercise the substantive production failure before the
   correction; it must not merely assert exception text.
2. Sweep all 16 lanes under neutral presentation. Check exceptions, finite
   claws, unique outward orientation, representative nearby Blaster offsets
   where applicable, and both repeated-center junctions. Report exact failing
   lanes and common geometry.
3. Determine whether the old rule assumes a simple polygon/global interior.
   Consider the zero-global-area, self-touching two-lobed Level 16 ordered
   closed walk, but treat possible causes as hypotheses until measured.
4. Derive a local outward-side rule justified by ordered topology/metadata.
   Preserve the existing rule for ordinary simple tubes. Do not use screen
   quadrants, hard-coded lane lists, guessed signs, or `DisplayedLevel == 16`
   where topology suffices. If authoritative geometry cannot distinguish the
   side, stop and report the ambiguity.
5. Implement the smallest correction. No exception suppression, skipped
   drawing, constant guessed sign, screen-center direction, changed lane,
   hidden player, prior-level behavior change, Level 16 geometry/calibration
   change, or gameplay-state change. Keep fail-fast for unsupported invalid
   geometry.
6. Add headless production-path tests for the exact initial crash, all 16
   lanes and repeated-center-adjacent lanes, representative non-neutral values
   of each existing control (`--tube-y-gain`, `--tube-scale`,
   `--tube-vertical-offset`, `--tube-depth-y-gain`), simple closed/open
   prior-level Blaster orientation, and complete bounded Level 16 geometry
   generation for tube, Blaster, shared-center Flipper, Tanker, Fuseball, Egg,
   Spike, and shot entities. Use draw callbacks where available; do not open a
   graphics window or turn general gameplay tests into renderer tests.
7. Update durable documentation only if a maintainable local-orientation rule
   is introduced. Do not claim final visual acceptance or encode calibration.

## Validation order

Run bounded VSTest commands with explicit hang bounds, in this order:

1. Exact Blaster crash regression.
2. All 16 Level 16 lanes.
3. Non-neutral calibration-control compatibility.
4. Bounded complete Level 16 presentation draw-path coverage.
5. Level 16 Flipper/entity presentation regressions.
6. Levels 1–15 Blaster/presentation regressions.
7. Level 16 gameplay regressions.
8. Full automated solution suite.
9. Release build.
10. `git diff --check`.

Do not launch the interactive game. A prior-level presentation regression is a
blocker. A blocked, stalled, canceled, timed-out, or incomplete validation is
not a pass; stop and report the exact status. Do not bypass Windows security.

## Required artifacts

Save this substantive prompt as this file and create
`AI-Logs/Reports/2026-11-24-level16-atari-presentation-blaster-interior-side-crash-correction-report.md`.
The report must record:

1. starting and final Git/worktree state;
2. supplied manual failure evidence;
3. deterministic production-path reproduction;
4. every failing lane and projected rail/claw values;
5. geometric cause and selected local orientation rule, with justification;
6. files changed, correction, and confirmation no exception suppression;
7. all-lane, calibration-control, complete draw-path, and Levels 1–15 coverage;
8. focused tests, full-suite, Release-build, and diff-check results;
9. unresolved visual questions and exact manual retest command;
10. confirmation nothing was staged/committed/pushed, Level 17+ was not
    implemented or investigated, no final Level 16 calibration was encoded,
    and Level 15 was not used as the correction model.

Stop if headless reproduction fails, topology cannot establish outward, a
geometry change or broad renderer redesign is needed, prior-level behavior
must change, exception suppression/player hiding is the only fix, validation is
blocked, or the worktree prevents safe isolation. Preserve all work and report
the blocker.

Completion requires the reproduced crash to be corrected; all lanes,
representative offsets, and calibration controls to render finite outward
claws; the complete bounded draw path and prior-level/gameplay regressions to
pass; the full suite, Release build, and diff check to pass; and no staging,
commit, push, final calibration, or Level 17+ work.

After automated validation, stop and provide Neal this exact command:

```powershell
dotnet run --project src\Tempest.Game -- --start-level 16 --atari-presentation
```

TEMPEST_LEVEL16_ATARI_PRESENTATION_BLASTER_INTERIOR_SIDE_CRASH_CORRECTED
