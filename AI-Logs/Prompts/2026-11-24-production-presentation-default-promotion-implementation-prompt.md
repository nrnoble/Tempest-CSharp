# Tempest Accepted Atari Presentation — Promote to Production Default

## Scope and decision

Implement a bounded selection/default promotion for the accepted Atari
presentation. Ordinary interactive gameplay, direct starts through production
Level 16, and visible playback should select the existing accepted Atari path
and its existing per-level calibration without requiring
`--atari-presentation`. This default-selection change is a PROJECT DECISION
candidate pending Neal's manual acceptance; do not describe the promotion as
manually accepted.

Retain `--atari-presentation` as a valueless redundant compatibility alias.
Add `--baseline-portrait-presentation` as an explicit development-only A/B
diagnostic selecting the existing old portrait projection and portrait
viewport. Keep `--legacy-presentation` distinct as the existing 4:3 mode.
`--portrait-perspective` applies only with the explicit baseline diagnostic.
Keep explicit-option presence separate from effective presentation selection,
including headless playback and `--input-diagnostic`.

Use the existing presentation adapter, calibration, HUD, camera, geometry,
gameplay and transition paths unchanged. Do not alter the renderer constructor
default, accepted Level 1–16 behavior, recording format, simulation, background
opacity semantics, or unrelated controls. Preserve old-projector internal
uses, file-mode constraints, headless/input routing, and the 4:3 presentation.
Do not begin Level 17+, modify protected/local material or unrelated AI-Logs,
stage, commit, push, or launch the interactive game.

## Starting-state protection and evidence

Before editing, record branch, HEAD, upstream, ahead/behind, index, tracked
modifications and task-relevant untracked files. Preserve unrelated worktree
material. Read completely:

- `AI-Logs/Reports/2026-11-24-production-presentation-default-promotion-investigation-report.md`
- `AI-Logs/Reports/2026-11-24-level16-final-closeout-report.md`

The implementation checkpoint is Level 16 closeout commit
`03346581b0fa3253bbb010022e8c617ce414d466`. Do not rewrite historical reports
or modify raw/local references.

## Implementation and tests

Use a centralized, testable presentation-selection seam for: default Atari
portrait, explicit Atari compatibility alias, explicit old portrait
diagnostic, existing legacy 4:3, headless playback, and input diagnostic.
Validate duplicates, valued forms, conflicts, calibration prerequisites and
file-mode allowlists against effective selection while preserving all other
validation. Normal launch must not print a candidate/development warning just
because Atari presentation is now the default.

Add focused coverage for default/alias equivalence and accepted calibration
and projection on Levels 1, 5, 6, 9, 13 and 16; baseline projection/viewport,
perspective semantics and conflicts; legacy 4:3 distinction; calibration,
guide and HUD controls without the alias; visible recording/playback; unchanged
recording and deterministic simulation; and headless/input routing. Preserve
existing numeric projection/calibration and gameplay fingerprints.

Update current README/help/startup wording and durable technical documentation
only enough to describe the candidate accurately. Distinguish the already
accepted Levels 1–16 Atari path from this pending default-selection decision.

## Validation order

Run bounded VSTest validation in this order: selection/parser; alias
equivalence; baseline; legacy; calibration controls; recording/visible
playback; headless/input; Levels 1–16 presentation regression; gameplay and
progression regression; full solution suite; Release build; `git diff --check`.
Use an explicit VSTest hang bound consistent with project policy. Diagnose
failures rather than weakening tests. A blocked, timed-out, canceled, hung or
stalled validation is not a pass and requires an explicit PARTIAL/BLOCKED
handoff.

## Manual acceptance boundary

After automated validation, stop for Neal; do not launch the game. Provide
exact commands for: default launch; direct starts at Levels 5, 6, 13 and 16;
one `--atari-presentation` alias comparison; one baseline portrait diagnostic;
and visible playback without the alias. Ask Neal to verify selection and
existing accepted appearance, not to repeat historical calibration.

## Completion

Create this paired report:
`AI-Logs/Reports/2026-11-24-production-presentation-default-promotion-implementation-report.md`.
Record starting/final Git state, changed files, selection architecture,
preserved behavior, tests and ordered validation outcomes, unresolved issues,
manual commands, and confirmations that nothing was staged/committed/pushed
and Level 17+ was not implemented or investigated. Stop after automated
validation for manual acceptance.

TEMPEST_PRODUCTION_PRESENTATION_DEFAULT_PROMOTION_IMPLEMENTED
