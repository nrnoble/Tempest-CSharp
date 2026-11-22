# Task: Reconcile Level 10 geometry recovery with the Level 11 evidence block

Perform a tightly bounded, read-only investigation. Do not implement Level 11.

Establish exactly how genuine Level 10 geometry was recovered: identify its
original evidence source, the role of the tracked ROM disassembly, the
reconstruction/extraction steps, whether textual values or existing C# supplied
the data, whether an extracted text artifact was created, and what the
historical reports and later implementation report independently establish.

Compare that method with
`AI-Logs/Reports/2026-11-20-level11-fast-production-evidence-block-report.md`.
Apply only methods and sources supported by authorized, project-local material.
Recover Level 11 facts where the evidence supports them, keeping geometry and
gameplay configuration separate; do not invent values or substitute another
level's data.

Authorized material: tracked source, tests, AI-Logs, behavior/development
documentation, and tracked textual references. Do not access or enumerate
`Reference/arcade/roms.rev2/`; do not open or use
`Reference/arcade/MBUCOD.V05.txt`; do not inspect visual/local artifacts; do not
search the public web for ROMs or source material.

Do not modify production source, tests, configuration, durable behavior
documentation, or references. Preserve existing worktree changes. Do not stage,
commit, push, reset, restore, stash, clean, or rebase. No build, test, game
launch, or manual gameplay acceptance is requested; report validation status
explicitly and stop after the report.

Report the Level 10 recovery chain, source inventory, differences from the
Level 11 block, recovered Level 11 facts, remaining evidence gaps, and one
recommendation: READY FOR LEVEL 11 IMPLEMENTATION, PARTIALLY READY, or STILL
BLOCKED. Classify findings as HISTORICAL PROJECT EVIDENCE,
ROM/DISASSEMBLY BACKED, TRACKED TEXTUAL REFERENCE, CURRENT C# REPRESENTATION,
PROJECT TRANSLATION, or UNRESOLVED. Create this prompt and a paired,
date-prefixed report under `AI-Logs/`; do not stage or commit either record.

TEMPEST_LEVEL10_LEVEL11_EVIDENCE_RECONCILIATION
