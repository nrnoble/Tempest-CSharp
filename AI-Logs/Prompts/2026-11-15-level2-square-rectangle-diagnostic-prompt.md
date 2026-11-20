# Tempest: Read-Only Level 2 Square-versus-Rectangle Presentation Diagnostic

Perform a tightly bounded, read-only investigation of the Level 2 presentation
shape. Do not implement a correction. The only authorized repository artifacts
are this prompt and its paired report:

- `AI-Logs/Prompts/2026-11-15-level2-square-rectangle-diagnostic-prompt.md`
- `AI-Logs/Reports/2026-11-15-level2-square-rectangle-diagnostic-report.md`

Do not edit source, tests, configuration, geometry, documentation, Git state,
or other project content. Do not stage or commit these ignored files.

## Frozen checkpoint and scope

Treat Level 2 vertical placement (+114), depth/perspective, near/far center
relationship, gameplay, and recorded playback as manually accepted and frozen.
Investigate only why the arcade/reference Level 2 is vertically rectangular
while the C# tube is square. This is a presentation/aspect question, not
permission to change model geometry.

Use only these handoffs as needed:

- `AI-Logs/Reports/2026-11-15-level2-atari-presentation-candidate-report.md`
- `AI-Logs/Reports/2026-11-15-level2-placement-only-correction-report.md`
- `AI-Logs/Reports/2026-11-15-levels2-6-atari-csharp-coordinate-mapping-report.md`

Inspect current source for decisive formulas. Do not reopen ROM archaeology,
Math Box analysis, Level 2 mapping provenance, placement derivation, accepted
Level 1 presentation, or Levels 3-9 work. Do not inspect visual assets, launch
the game, run gameplay, search the Internet, or infer a correction magnitude
from unauthorized images. Use bounded inline calculations only.

## Required investigation

1. Reconstruct near, middle, and far Level 2 model-space, logical-screen, and
   portrait-host dimensions; report aspect ratios, X/Y derivatives, the local
   Jacobian, and whether equal-axis scaling holds at every depth. Treat +114
   as translation unless source proves otherwise.
2. Trace the points through model-frame conversion, depth projection,
   placement, portrait camera, and viewport mapping. Identify the first stage
   where a rectangular presentation can be added without changing depth.
3. Compare the C# Level 2 point bounds with the corresponding Atari raw-record
   bounds using the existing handoff. Do not alter points, order, lanes, or
   topology.
4. Evaluate candidate aspect-transform seams against near/far size ratio,
   center separation, rail convergence, placement, and entity/tube alignment.
   Do not change the depth denominator, Position, Depth, K, ZOffset, +114, or
   shared -33.1.
5. Identify which tube-attached object anchors and local dimensions need the
   same treatment; distinguish intentionally screen-space effects.
6. Classify the relation to the previously observed Level 1 aspect difference
   without modifying Level 1 or generalizing a future fix to it.
7. Select exactly one required diagnostic classification:
   MISSING PRESENTATION ASPECT TRANSFORM, MODEL-FRAME CONVERSION ISSUE,
   PORTRAIT-CAMERA ASPECT ISSUE, MIXED, or UNRESOLVED.
8. Establish presentation-only correction feasibility, assess every
   decision-relevant unknown with the five specified questions, and recommend
   exactly one smallest next milestone.

## Git and completion

Before substantive work, record branch, HEAD, upstream, tracked modifications,
untracked paths, and index state. Preserve all pre-existing work. Stop after
the investigation and report; no tests/build are required.

The report must include the executive conclusion; frozen checkpoint;
dimensions, ratios, derivatives, and source locations; geometry-versus-
presentation finding; responsible stage; accepted-depth analysis; object
consequences; Level 1 relationship; correction feasibility; unknowns; one
smallest next milestone; created/inspected files; calculations; starting and
ending Git state; and confirmation that no tracked file or index entry changed
and nothing was staged, committed, or pushed.

TEMPEST_LEVEL2_SQUARE_RECTANGLE_DIAGNOSTIC_READONLY_20261115
