# Tempest Level 16 Read-Only ROM / Configuration Investigation

## Scope

Perform a read-only investigation establishing an evidence-backed Level 16
specification for a later, separately authorized implementation. Start with the
project-local Atari Tempest Rev. 2 disassembly and configuration evidence.
Examine the ROM tube mapping and geometry, enemy supply and type limits, Spike
configuration, explicit below-rim Flipper movement program, Tanker cargo,
Spiker movement and conversion, Fuseball/Pulsar presence, firing parameters,
admission/concurrency, wave completion, level transition/direct-start behavior,
and presentation metadata. Assess which current C# shared systems can be
reused and identify any gaps and regression-sensitive invariants.

## Evidence and reference boundaries

Classify findings as ROM-BACKED, ARCADE-OBSERVED, existing PROJECT
DECISION/TRANSLATION, REFERENCE-DERIVED, or unresolved. Do not claim Level 16
arcade observation. Do not search the Internet for the disassembly already in
the repository. Level 14 may be consulted for development-process discipline,
but its behavior must not be copied. **Do not use Level 15 as Level 16's
implementation model, behavioral model, or development template.** Treat
Level 15 only as existing production code that must remain regression-safe.

## Constraints

- Investigation only: do not modify production code, tests, durable behavior
  documentation, or unrelated files.
- Do not implement Level 16 or Level 17/recycling.
- Do not stage, commit, or push.
- Keep existing local changes and user/reference material untouched.
- Read raw ROM data only through a narrowly bounded verification when needed;
  do not hash or enumerate ROM contents.
- Do not choose presentation calibration values or launch the interactive game.
- Do not run broad tests/builds for ceremony. Manual gameplay and visual
  acceptance remains Neal's responsibility after a future implementation.

## Deliverable

Write the substantial investigation report to
`AI-Logs/Reports/2026-11-24-level16-rom-configuration-investigation-report.md`.
Cover the ROM specification, architecture mapping, regression-sensitive areas,
uncertainties/conflicts, comparison with the Level 14 development approach,
bounded implementation recommendation, files and commands inspected, and
confirmation that no production/test files changed. Stop after the report;
Neal must review it and separately authorize implementation.

TEMPEST_LEVEL16_ROM_CONFIGURATION_READONLY_COMPLETE
