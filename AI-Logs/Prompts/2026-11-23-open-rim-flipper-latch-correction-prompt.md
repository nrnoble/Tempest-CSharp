# Open Rim Flipper Pursuit-Latch Correction Prompt

Date: 2026-11-23

## Scope

Continue from the current uncommitted Level 12 candidate and correct only the
open-tube rim-Flipper endpoint pursuit-latch defect. The completed read-only
handoff at
`AI-Logs/Reports/2026-11-23-rim-flipper-right-endpoint-oscillation-readonly-report.md`
was used without repeating the runtime investigation.

## Constraints

- Synchronize `RimPursuitDirection` only after a rim-pursuit open-end fallback
  physically reverses `Direction`.
- Do not change Level 12, rim pause duration, pivot speed, collisions,
  transition completion, geometry, or endpoint predicates.
- Preserve closed-tube wrapping, player-crossover latch behavior, below-rim
  programmed routes, Tanker-cargo routes, Level 11 behavior, and all existing
  unrelated accepted work.
- Perform a bounded read-only Git-history investigation of the latch and
  open-end fallback mechanisms.
- Add focused deterministic open-end and closed-control regressions.
- Run targeted validations, the complete solution test suite, Release build,
  and `git diff --check`.
- Do not launch the interactive game, stage, commit, push, reset, restore,
  stash, clean, rebase, or alter protected user material.

## Validation and acceptance

Automated validation can establish state invariants only. Neal must manually
observe Level 10 and Level 12 rim travel. The following are the required
manual checks:

1. Level 10 right/left open endpoints reverse once and continue inward for
   more than one lane.
2. Level 12 remains coherent and closed; no newly introduced endpoint reversal
   appears.

## Completion token

TEMPEST_OPEN_RIM_FLIPPER_LATCH_CORRECTION
