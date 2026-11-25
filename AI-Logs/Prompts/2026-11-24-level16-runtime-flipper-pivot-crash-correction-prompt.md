# Tempest Level 16 Runtime Flipper-Pivot Crash Investigation

Investigate and correct the Level 16 runtime Flipper-pivot rendering crash
reported during Neal's first manual gameplay test. The observed command was:

```powershell
dotnet run --project src\Tempest.Game -- --start-level 16 --lives 12
```

The game initialized, entered Level 16, and ran for a couple of seconds before
rendering failed with:

```text
System.InvalidOperationException:
An inward pivot ray did not intersect its closed tube polygon.
```

Required prior context:

- Read `AI-Logs/Reports/2026-11-24-level16-rom-configuration-investigation-report.md`.
- Read `AI-Logs/Reports/2026-11-24-level16-bounded-implementation-report.md`.
- Treat the uncommitted Level 16 implementation as the working candidate.
- Preserve unrelated edits and untracked files. Do not stage, commit, push,
  launch the interactive game, or modify protected reference/user material.

The static Level 16 shape was manually observed to look good. Preserve its exact
ROM row 11 points, all 16 lanes, point-0/point-8 repeated center, closed cyclic
adjacency, two-lobed self-touching topology, and presentation pipeline. Level
15 must not be used as a model; Level 14 is process-only. Do not implement
Level 17/recycling or perform presentation calibration.

First record branch, HEAD, upstream, staged/unstaged changes, relevant
untracked files, and the current Level 16 diff. Reproduce the actual failing
transition deterministically before proposing a correction. Record Flipper
identity/type, source/destination, direction, progress, source lobe/winding,
pivot, proposed ray, edges/intersections/distances, relation to the repeated
center, and whether rendering depth matters. If deterministic reproduction
fails, stop without speculative changes.

Correct only a demonstrated defect. Do not suppress exceptions, fabricate or
clamp endpoints arbitrarily, skip/freeze Flippers, change `$24` gameplay,
distort ROM geometry, weaken simple-tube invariants, or add renderer catches.
If the single-ray pivot model cannot represent the shared-center transition
without a broad redesign, stop and report that architectural finding.

If a safe correction is established, add an exact crash regression, bounded
coverage of every directed adjacent Level 16 transition and both lobes, a
production Flipper/render-projection integration test, and prior-level
regression protection. Validate in order: exact crash test; bounded Level 16
coverage; Level 16 integration; geometry/Flipper regressions; relevant
Tanker/cargo and `$24` tests; full solution suite; Release build; and
`git diff --check`. Use bounded commands and report incomplete/blocked
validation accurately. Do not weaken assertions to pass.

Create a paired report at
`AI-Logs/Reports/2026-11-24-level16-runtime-flipper-pivot-crash-correction-report.md`.
Record the starting/final Git state, observed evidence, reproduction and exact
values, geometric cause, static-vs-dynamic distinction, files/correction,
geometric justification, topology preservation, tests/validation, unresolved
questions, and remaining manual acceptance. Do not rewrite historical reports
or claim Level 16 acceptance. Leave AI-Logs unstaged.

If complete, stop and give Neal this manual retest command:

```powershell
dotnet run --project src\Tempest.Game -- --start-level 16 --lives 12
```

Completion token:

TEMPEST_LEVEL16_RUNTIME_FLIPPER_PIVOT_CRASH_CORRECTION_COMPLETE
