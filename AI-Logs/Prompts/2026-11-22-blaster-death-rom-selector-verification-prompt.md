# Narrow ROM Verification: Blaster-Death Picture Selector and Timing

Date: 2026-11-22

Perform a tightly bounded, read-only investigation; do not implement changes.
Use `AI-Logs/Reports/2026-11-22-blaster-death-visual-animation-readonly-report.md`
as the completed visual handoff. Do not repeat its full 45-frame analysis or
inspect any visual assets beyond the authorized frame sequence, and only if a
representative frame is needed to reconcile ROM timing.

Trace the Atari player-death cause/state, selector index and timer, graphic
table, vector picture/scale entries, and update/culling path. Establish whether
the supplied multicolor burst is the enemy-shot, Spike/Pulsar, shared, or
another death path; list exact selected pictures, scales, order, and holds.
Distinguish ROM-driven held pictures from MAME capture duplication. Examine
`$0ffc` only as needed to distinguish the separate spiked/pulsared-apart path.

Start with the tracked textual disassembly and prior report. Read
`Reference/arcade/roms.rev2/136002-118.k1` only if required bytes are absent
from the tracked disassembly; then inspect only the specific bounded ranges
needed. Do not enumerate or inspect other ROM files. Do not access
`Reference/arcade/MBUCOD.V05.txt`, search the public web, or alter any ROM,
reference, source, test, configuration, durable documentation, gameplay,
asset, or Git state.

Assess whether the recovered visual stages can be represented as a rendering-
only lookup on the accepted deterministic 60-tick C# `PlayerDeathPresentation`.
Preserve the accepted center, three-layer geometry, 150% peak size, shared
legacy/Atari rendering, gameplay/death/re-entry behavior, and final
disappearance unless direct evidence contradicts it. Do not implement or
change gameplay timing.

Write a concise evidence-specific paired report under `AI-Logs/Reports/`
covering exact effect identity, selector chain, stage order, holds/cadence,
one prescribed readiness result, an implementation table only if ready,
focused regression tests, unresolved points, validation status, and manual
acceptance still required. Save this prompt under `AI-Logs/Prompts/`.
AI-Logs are ignored; do not stage or commit. Stop after the report.

Validation: verify the source/disassembly trace and report contents only;
do not run or launch the game. This investigation makes no behavior change,
so no manual gameplay acceptance is requested.

TEMPEST_BLASTER_DEATH_ROM_SELECTOR_VERIFICATION
