# Tempest: Read-Only Atari Presentation Adapter Implementation Plan

Date: 2026-11-15

## Task and authorization

Create a complete, bounded implementation plan for one render-only Atari-style
presentation candidate. This is planning only: do not implement, prototype,
format, or modify production code, tests, durable documentation, configuration,
Atari references, ROM/PROM material, Git state, staging, commits, or branches.
Create exactly these two ignored files and no others:

- `AI-Logs/Prompts/2026-11-15-atari-presentation-adapter-implementation-plan-prompt.md`
- `AI-Logs/Reports/2026-11-15-atari-presentation-adapter-implementation-plan-report.md`

Do not stage or commit either file. Do not launch the game, inspect screenshots,
frames, or videos, search the Internet, inspect raw ROM/PROM files, or use third-
party implementations.

## Objective and invariants

Plan an explicitly selected, render-only transform. It must preserve production
`LevelDefinition.Points`, point indices and lane identity, lane order,
`PlayableLaneCount`, open/closed topology, gameplay/model geometry, collision,
enemy routes and directed pivots, entity coordinates, depth/height, waves,
timing, score, lives, recorded-input semantics, deterministic simulation state,
and current mechanics. Preserve the current Level 7 coordinates and point order,
the shared portrait vertical placement `-33.1`, existing Transition and Approach
behavior except for a necessary presentation-path adjustment, and the accepted
portrait diagnostic.

The controlling handoff is
`AI-Logs/Reports/2026-11-15-presentation-layer-atari-transform-feasibility-report.md`.
Consult only as needed:

- `AI-Logs/Reports/2026-11-15-atari-mathbox-c098-reconciliation-report.md`
- `AI-Logs/Reports/2026-11-15-atari-csharp-projection-comparison-report.md`
- `AI-Logs/Reports/2026-11-15-atari-csharp-coordinate-frame-mapping-report.md`
- `AI-Logs/Reports/2026-11-15-level7-geometry-provenance-disposition-report.md`

Do not reopen Atari archaeology, Math Box analysis, geometry provenance, or
general feasibility. Verify decisive details against current C# source.

## Required plan

1. Identify the exact source and test files a future bounded candidate would
   modify, and important nearby files that must remain untouched.
2. Define one render-only adapter: responsibility, typed inputs and outputs,
   metadata access, model-frame conversion, normalized depth, per-level
   divisor, Y reference, signed post-projection Y offset, logical units,
   glyph-local scale/Jacobian, Approach offsets, out-of-range behavior,
   finite/denominator validation, and missing-data behavior. Choose the smallest
   safe seam and keep the established projection path intact.
3. Define explicit legacy/candidate selection. State defaults, development-only
   command-line selection, composition with `--portrait-perspective`, landscape
   behavior, and use with recording, playback, pause, frame stepping, and
   screenshots. Do not make the candidate the unconditional default.
4. Account for metadata for every production Level 1-9 without inventing
   values. Keep Atari record mapping, ROM parameters, model-frame orientation,
   model-units-per-Atari-unit conversion, and topology distinct. Do not encode
   Level 7's comparison-only `+5` cyclic alignment.
5. Inventory registered gameplay anchor and glyph paths. Specify endpoint
   projection, a justified local scalar/Jacobian, or preserved screen-space
   sizing for each. Preserve tube/entity and projectile/entity registration,
   shots, effects, and rim/below-rim consistency.
6. Separately address Approach, transition stars, Game Over vectors, HUD/text,
   playback, pause/stepping, and screenshot capture. Keep Game Over on a
   deliberate safe path if its coordinate conversion is not established.
7. State the mathematical contract and intentionally omitted Atari details,
   with evidence-based reasons and possible later tests.
8. Map at least these invariants to focused tests: unchanged legacy projection;
   explicit candidate selection; unchanged points, lanes, topology, and
   simulation; exact Level 1-9 ROM metadata; missing-data behavior;
   near/mid/far calculations for Levels 1, 7, 8, 9; expected symmetry; Y
   reference and signed offset; tube/entity registration; local glyph scale;
   Approach composition; playback/screenshot option composition;
   portrait-diagnostic interaction; frozen `-33.1` host placement; invalid
   inputs; and deterministic repeated output.
9. Give the future bounded validation order: focused tests, relevant
   presentation tests, full suite, Release build with no warnings/errors,
   `git diff --check`, Git/index/worktree verification, and Neal's manual
   acceptance. Do not launch the interactive game.
10. Define rollback, deferred work, decision-relevant unknowns using the
    four-question test, exactly one readiness decision, and exactly one
    smallest implementation milestone. Stop that future milestone for Neal's
    manual acceptance; do not activate the candidate unconditionally or commit
    before acceptance.

Classify major claims as `CONTROLLING-HANDOFF-BACKED`, `C#-SOURCE-BACKED`,
`IMPLEMENTATION-PLAN DECISION`, `MANUAL-ACCEPTANCE REQUIREMENT`, or `UNKNOWN`.
Do not represent plan decisions as current behavior or infer visual acceptance
from calculations.

## Scope and completion record

The report must include the executive decision; evidence used; architecture
and mathematics; exact modification/exclusion surface; option behavior;
per-level metadata; glyph/entity handling; special paths; automated tests;
future validation and manual commands; rollback and deferred work; unknowns;
the one implementation milestone; and starting/ending Git state. Record that
this planning task makes no source/test changes, runs no build or test suite,
does not launch or inspect media, and stages/commits/pushes nothing.

TEMPEST_ATARI_PRESENTATION_ADAPTER_IMPLEMENTATION_PLAN_READONLY_20261115
