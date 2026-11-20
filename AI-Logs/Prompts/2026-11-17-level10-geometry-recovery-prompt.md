# Read-only Level 10 geometry recovery

## Objective

Determine whether the authoritative project-local Atari Tempest Rev. 2
disassembly and ROM material uniquely establish displayed Level 10's
`lev_remap` selection, ordered tube geometry, and any directly necessary
companion geometry/topology data. Do not implement Level 10.

## Scope and evidence rules

Use the local Rev. 2 disassembly as the primary authority and bounded checks
of only plausible local Rev. 2 ROM material to resolve its `.chunk`
declarations. Existing project geometry and durable Level 8/9 evidence may
corroborate the decoding method, but are secondary. Do not use the Internet,
Incitti, screenshots, videos, visual fitting, or unrelated gameplay evidence.
Establish displayed-level versus internal indexing and point ordering; do not
assume array row 10 means displayed Level 10. Stop once the geometry is
uniquely recovered and cross-checked, or a specific missing artifact blocks
recovery.

## Read-only constraints

Do not modify source, tests, configuration, reference/ROM material, durable
documentation, existing diagnostics, recordings, screenshots, or Git state.
Do not stage, commit, push, reset, restore, checkout, clean, or stash. Preserve
the pre-existing unstaged Level 7 work exactly. Disposable bounded analysis
is allowed, but do not write derived ROM copies into reference locations.
Only the paired ignored AI-Logs prompt/report records may be written.

Do not launch the game or implement/fix Level 10 or the Level 7 presentation
defect. No gameplay manual acceptance is being requested or claimed.

## Validation and report

Use bounded decoding and cross-check at least one established geometry where
practical. Do not run the full test suite or a Release build. Run
`git diff --check` and compare final Git/worktree state with the starting
state. Write a paired report documenting sources, exact offsets/addresses,
indexing, data and decoding, cross-check, uncertainties, implementation
status, and final worktree integrity. State `RECOVERED` only if the
authoritative evidence uniquely establishes the geometry; otherwise state
`BLOCKED` and identify the smallest missing artifact.

TEMPEST_READONLY_LEVEL10_GEOMETRY_RECOVERY
