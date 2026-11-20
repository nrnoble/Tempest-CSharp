# Tempest: Read-Only Atari-to-C# Coordinate-Frame Mapping

Perform a tightly bounded, read-only investigation of displayed Levels 1, 7, 8, and 9. Determine whether their current production `LevelDefinition.Points` can be related to Atari tube records 00, 06, 07, and 0d using only centering/translation, axis reflections/sign convention, uniform or axis-specific scaling, point-index rotation/reversal, and open/closed endpoint convention.

Use `AI-Logs/Reports/2026-11-15-atari-csharp-projection-comparison-report.md` as the controlling handoff. Verify the level-to-record map through `lev_remap`; use the local disassembly and recovered rows/parameters in the existing Atari reports. Do not search the Internet, use third-party implementations, reopen Math Box analysis, inspect media, or read raw ROM/PROM files.

For each level, inventory the exact C# point list or its source conversion, point count/order, topology, `CenterY`, `YOffset`, provenance, and any pre-projection normalization. Inventory all 16 raw Atari X/Y pairs, normal X reference `$80`, selected `lev_y3d`, `lev_open`, and ordering. Keep raw coordinates distinct from projected coordinates and do not use Math Box projection to force point correspondence.

Run a bounded deterministic calculation across all corresponding points. Report the chosen index map and coordinate formula, scale/reflections, maximum absolute X/Y residuals, and defined RMS residual. Test a common scale before separate axis scales. Do not use nonlinear warping, per-point correction, arbitrary movement, or perspective projection. Classify each level as EXACT FRAME MAPPING, CONSISTENT APPROXIMATE FRAME MAPPING, PARTIAL STRUCTURAL CORRESPONDENCE, NO CONSISTENT FRAME MAPPING, or UNKNOWN. Do not infer ROM provenance from shape resemblance.

Evaluate cross-level conventions for X centering, Y orientation, unit scale, index direction/start, topology, the status of Levels 1/7 versus ROM-derived Levels 8/9, and whether a mathematical presentation-frame conversion can preserve gameplay topology/lane identity. Select one overall implication: FRAME-MAPPING SUPPORTS PARAMETERIZATION PATH, FRAME-MAPPING REVEALS GEOMETRY GAP, MIXED, or UNRESOLVED. Recommend exactly one smallest next milestone; do not design or implement a transform.

Before investigation, record branch, HEAD, upstream/tracking, tracked modifications, untracked paths, and index state. Preserve the active portrait-presentation worktree and all existing user/reference material. Do not modify project code/tests/docs/configuration, stage, commit, push, clean, reset, restore, or stash. Create exactly these two ignored artifacts and no others:

- `AI-Logs/Prompts/2026-11-15-atari-csharp-coordinate-frame-mapping-prompt.md`
- `AI-Logs/Reports/2026-11-15-atari-csharp-coordinate-frame-mapping-report.md`

No build/test is required. Do not launch the game. The report must record sources and line locations, geometry and Atari inventories, per-level calculations/classifications, cross-level answers, overall implication, one next milestone, calculations, created files, and starting/ending Git state. Confirm tracked files and index stayed unchanged and nothing was staged, committed, or pushed. Stop after the four samples and report are complete.

TEMPEST_ATARI_CSHARP_COORDINATE_FRAME_MAPPING_READONLY_20261115
