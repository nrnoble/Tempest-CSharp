# Final Blaster-Death Geometry Investigation Prompt

Date: 2026-11-22

## Scope and constraints

Final read-only investigation of whether the Atari enemy-shot player-death
fragment program at `$3b1a–$3b82` can be directly translated into the existing
C# vector renderer to create a justified visible improvement over its current
three-layer starburst. Do not implement anything, investigate timing, begin a
follow-up investigation, or inspect protected ROMs unless bytes in this exact
range are missing or ambiguous in the tracked disassembly. Do not read
`Reference/arcade/MBUCOD.V05.txt`.

The current stepped schedule was manually tested and looked effectively
identical to the prior accepted effect; that observation is authoritative.
Investigate geometry only. Decode every vector operation, reconstruct ordered
visible line segments with raw local coordinates and color/stat groups,
calculate bounds/topology, compare with current C# geometry, and provide a
numeric implementation translation or recommend stopping.

Do not modify production source, tests, configuration, durable documentation,
reference material, or historical records. Do not stage, commit, push, reset,
restore, stash, clean, or rebase. Preserve all pre-existing work. The paired
AI-Logs prompt/report records are the only authorized writes.

If implementation-ready, specify immutable local segment data, color per
segment/group, screen-axis mapping, coordinate scaling/centering, unchanged
stage schedule, center, projections, determinism, gameplay boundaries, and
focused regression tests. Finish the report with exactly one of:
`READY FOR FINAL BOUNDED IMPLEMENTATION` or
`STOP BLASTER-DEATH REFINEMENT`. Do not recommend further investigation.

Completion token:

TEMPEST_BLASTER_DEATH_FINAL_GEOMETRY_INVESTIGATION
