# Tempest: Read-Only Level 4 Geometry Provenance and Presentation-Mapping Investigation

This is a bounded, read-only investigation of the current production Level 4
binocular geometry. Determine its provenance, intended coordinate frame,
manual-acceptance scope, gameplay dependencies, relationship to Atari record
03, minimum safe render-only mapping, placement/height constraints, Atari
parameter applicability, and one implementation disposition.

Do not implement Level 4 Atari-presentation support. Do not modify production
code, tests, geometry, configuration, durable documentation, reference files,
Git state, staging, commits, branches, or existing project content. Create
only this prompt and its paired report under `AI-Logs/`; do not stage or commit
either file.

## Accepted context and user observation

Preserve accepted Levels 1-3. Level 4 differs visibly from Levels 1-3, has a
shorter overall screen profile, and is currently properly placed. Treat that
as Neal's manual observation, not an exact measurement or proof of historical
placement acceptance. Do not copy Levels 1-3 centering, placement, height, or
aspect adjustments. Do not inspect or request the images used for this
comparison. Do not investigate the unrelated Level 1-3 Blaster-exit defect.

## Controlling evidence and scope

Read `AI-Logs/Reports/2026-11-15-levels2-6-atari-csharp-coordinate-mapping-report.md`
and verify decisive claims against current production source, durable project
records, Level 4-specific prompts/reports, relevant Git history, and already
recovered Atari record-03 data. Evidence authority is the behavior spec,
development notes, current source, preserved Level 4 records, Git history,
controlling handoff, then recovered Atari data. Do not inspect
`TEMP/myNotes.md`, search the Internet, inspect screenshots/videos/frames, or
use third-party implementations as authority. Incitti may be inspected only
to the extent project records identify it as the source of the current
geometry. Do not inspect raw ROM copies.

Record branch, HEAD, upstream, tracked modifications, untracked state, and
index state before work. Preserve all pre-existing work exactly. Do not clean,
reset, restore, stash, stage, commit, or push.

## Required investigation

1. Trace the earliest available evidence for the 16 ordered Level 4 points:
   reference/source, original coordinates, any conversion or scaling,
   ordering, lane order, origin/centering, axis orientation, intended profile,
   and whether the shorter screen profile was intentional or accepted.
   Do not infer provenance from numerical resemblance.
2. Separate acceptance of topology, shape, point/lane order, placement,
   width, height, perspective/depth, transitions, player and enemy movement,
   Tankers, Spikes, firing, collision, death/re-entry, progression, approach,
   and overall arcade likeness. Do not turn broad gameplay acceptance into
   exact geometry or presentation acceptance.
3. Trace only necessary code dependencies: lane identity/adjacency, player
   rim movement, directed pivots, Flipper routes, Tanker cargo, Spikes,
   collisions, firing/rim behavior, death/re-entry, and rendering/interpolation.
   Do not propose changed points or lane mappings.
4. Reproduce the bounded C#-to-Atari record-03 comparison: closed topology,
   cyclic correspondence, reversal, X/Y signs, uniform/independent scales,
   references/translations, and residuals. Verify the prior uniform and
   independent fits. All alignments are analysis-only; do not authorize a
   production reorder, reflection, or point change from a fit.
5. Identify minimum render-only frame features; distinguish temporary
   projection changes from model geometry and lane identity changes. Assess
   current adapter support, preserving existing C# depth progression, proper
   current placement, and Level 4's shorter profile.
6. Assess `lev_scale`, `lev_y3d`, signed Y2D, open/closed, and raw-coordinate
   applicability without applying Atari arithmetic or placement literally.
7. Select exactly one disposition: READY FOR BOUNDED PRESENTATION CANDIDATE;
   READY WITH ONE EXPLICIT ADAPTER EXTENSION; REQUIRES FURTHER EVIDENCE; or
   NOT SUITABLE FOR CURRENT PRESENTATION APPROACH. Do not block merely because
   exact arcade-pixel equivalence is unavailable.
8. For each unresolved item, state whether it could change disposition,
   adapter requirements, model/lane safety, placement/height preservation, or
   validation. Recommend exactly one smallest next implementation milestone.

## Hard boundary and validation

Do not edit source/tests/docs/reference material, modify geometry/order/
topology/placement/height/perspective, implement adapter support, alter
Levels 1-3, fix the Blaster defect, inspect visual assets, launch the game,
run a build or full test suite, or change Git state. Any calculation must be
bounded and inline/temporary. Manual gameplay acceptance remains Neal's
authority for future presentation changes; report what must still be manually
accepted. Create exactly these ignored files:

- `AI-Logs/Prompts/2026-11-15-level4-geometry-provenance-presentation-mapping-prompt.md`
- `AI-Logs/Reports/2026-11-15-level4-geometry-provenance-presentation-mapping-report.md`

The report must include executive conclusion, evidence, provenance,
acceptance, gameplay dependencies, coordinate comparison, minimum frame,
placement/height, Atari parameters, disposition, decision-relevant unknowns,
smallest milestone, and Git/scope/completion state. State that tracked files
and index are unchanged and nothing was staged, committed, or pushed.

TEMPEST_LEVEL4_GEOMETRY_PROVENANCE_PRESENTATION_MAPPING_READONLY_20261115
