# Tempest: Read-Only Presentation-Layer Atari Transform Feasibility Investigation

Date: 2026-11-15

## Task

Conduct a bounded, read-only architecture and mathematical-feasibility investigation into applying Atari's recovered per-tube coordinate transform in the C# presentation layer. Do not implement an adapter.

Determine whether immutable production `LevelDefinition.Points`, indices, lane identities, topology, gameplay geometry, collisions, enemy movement, accepted Level 7 behavior, the current portrait formula candidate, and shared portrait placement `-33.1` can be preserved while presenting tube vertices and entities with Atari-equivalent per-level parameters.

## Controlling evidence

Use these existing handoffs:

- [Atari Math Box reconciliation](../Reports/2026-11-15-atari-mathbox-c098-reconciliation-report.md)
- [Atari/C# projection comparison](../Reports/2026-11-15-atari-csharp-projection-comparison-report.md)
- [Atari/C# coordinate-frame mapping](../Reports/2026-11-15-atari-csharp-coordinate-frame-mapping-report.md)
- [Level 7 geometry provenance disposition](../Reports/2026-11-15-level7-geometry-provenance-disposition-report.md)

Inspect only relevant current C# rendering, level, metadata, and configuration paths. Use local Atari material only if a decisive source check is essential. Do not search the Internet or use third-party implementations.

## Required investigation

1. Record branch, HEAD, upstream, tracked changes, untracked paths, and index state before investigation. Preserve all pre-existing work, including the active portrait candidate and Atari reference material.
2. Trace the safe immutable model-to-presentation boundary for tube vertices, edge/lane interpolation, depth/height, entities, rims, shots, Spikes, transitions, and Approach. Identify any paths or post-projection glyph scaling that need coordinated treatment to preserve registration.
3. Inventory the minimum per-level Atari inputs: record mapping, `lev_scale`, `lev_y3d`, signed Y2D, X reference, runtime-depth correspondence, topology/traversal state, and coordinate units/orientation. Distinguish metadata already present from values requiring derivation.
4. Derive a complete candidate mapping from immutable C# coordinates, including depth, references, offsets, output units, Y orientation, open/closed topology, and common tube/entity handling. A floating-point reciprocal-depth equivalent is acceptable if its omitted integer floor/saturation effects are explicit.
5. Directly resolve Level 7: determine whether record-06 parameters can apply to the existing Incitti-derived clover without replacing, rotating, scaling in storage, or reindexing its production geometry; distinguish presentation-coordinate derivation from model edits and distinguish transformability from visual fidelity.
6. Make a deterministic inline calculation for Levels 1, 7, 8, and 9 using immutable C# points and corresponding recovered presentation parameters. Use no more than three representative depths per level; show axis displacement, near/mid/far behavior, symmetry where expected, vertical-reference/offset behavior, and shared interpolation feasibility. Do not create a script or visual artifact.
7. Classify the Atari base transform relative to the existing `--portrait-perspective` formula/value near 125 and frozen host-space placement `-33.1`. Do not tune or invalidate either.
8. Select exactly one feasibility conclusion:
   - FEASIBLE — PRESENTATION-ONLY ADAPTER
   - FEASIBLE WITH EXPLICIT LIMITATIONS
   - NOT FEASIBLE WITHOUT MODEL/GEOMETRY CHANGE
   - UNRESOLVED
9. Apply the four-question relevance test to every remaining unknown and recommend exactly one smallest next milestone.

## Hard limits

Read-only except for exactly these two ignored artifacts:

- `AI-Logs/Prompts/2026-11-15-presentation-layer-atari-transform-feasibility-prompt.md`
- `AI-Logs/Reports/2026-11-15-presentation-layer-atari-transform-feasibility-report.md`

Do not modify code, tests, durable documentation, geometry, gameplay, projection, portrait settings, configuration, Atari material, or existing project content. Do not inspect screenshots, frames, video, or unrelated images. Do not launch the game, build, test, stage, commit, push, clean, reset, restore, or stash.

At completion, verify the existing tracked-file inventory and index are unchanged, and that nothing was staged, committed, or pushed. Report evidence boundaries, formulas and numeric outputs, Level 7 and registration conclusions, limitations/unknowns, exact created files, starting and ending Git state, and remaining manual acceptance. Do not claim visual fidelity without visual evidence.

TEMPEST_PRESENTATION_LAYER_ATARI_TRANSFORM_FEASIBILITY_READONLY_20261115
