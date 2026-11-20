# Tempest: Read-Only Level 5 Horizontal-Margin Diagnostic

This is a tightly bounded, read-only Level 5 diagnostic. Do not implement a
correction or modify production code, tests, geometry, configuration, other
documentation, visual assets, Git state, staging, commits, branches, or existing
project content.

## Authorized artifacts

Create only:

- `AI-Logs/Prompts/2026-11-15-level5-horizontal-margin-diagnostic-prompt.md`
- `AI-Logs/Reports/2026-11-15-level5-horizontal-margin-diagnostic-report.md`

Do not stage or commit these ignored working artifacts.

## Manual observation and target

Treat Neal's current manual observation of the implemented Level 5 candidate
as follows: the overall presentation is usable; centering, symmetry, HUD
separation, vertical placement, tall vertical character, and depth/perspective
look acceptable; the remaining clearly identified presentation issue is that
the left and right horizontal margins are too large.

For Level 5, use the authorized MAME Level 5 reference as the desired
horizontal-margin composition. Keep the tube centered, margins approximately
symmetrical, and target the MAME margins closely. Pixel-perfect correspondence
is ideal but not a manual-acceptance requirement. Do not generalize Level 5's
numeric ratio to other levels. Do not treat the margin observation as
permission to change geometry, Y scale, placement, depth, or another
accepted-looking characteristic.

## Authorized visual evidence

Analyze only:

`C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\Level-5\Tempest-MAME-frame_5880.png`

Do not enumerate its containing directory, inspect another visual asset, or
inspect a C# candidate screenshot. Reconstruct the C# candidate from source and
math. Treat MAME as a practical visual reference, not an exact physical-arcade
specification.

## Controlling implementation

Read `AI-Logs/Reports/2026-11-15-level5-visual-calibrated-candidate-report.md`,
but use current source as the authority for implementation details. Current
candidate facts are: net X factor `0.670`, Y factor `0.880`, calibrated Level 5
vertical placement, unchanged C# depth progression, and unchanged gameplay
and model geometry. Do not assume `0.670` is correct merely because it was
used in the first calibration.

## Objective and analysis

Determine why the first image-derived X calibration appears to leave excessive
horizontal margins, and derive the smallest justified Level 5 X-only
correction specification if the evidence supports one. Preserve horizontal
center and symmetry, current Y presentation and placement, HUD separation,
depth progression, geometry, gameplay, and playback. Do not implement it.

### A. Reconstruct current C# bounds

From Level 5 source and candidate arithmetic, calculate near-rim logical and
portrait-host X bounds, host width, left/right margins, margins normalized by
the 720-pixel viewport, middle- and far-depth bounds, center at each depth,
and the local horizontal derivative. Verify the actual `0.670` constant and
every subsequent X coordinate conversion through final host pixels.

### B. Remeasure MAME

In the authorized frame only, measure the outer near-rim blue-line left and
right edges, width, image width, normalized margins and width, and horizontal
center. Exclude gameplay objects, player/Blaster, HUD, and ambiguous glow.
State pixel uncertainty. Do not rederive Y calibration except as needed for
normalization.

### C. Reconcile coordinate domains

Trace:

`MAME normalized X -> portrait viewport normalized X -> C# logical X -> host X`

Test, rather than assume, whether the first calibration used unlike extents,
mixed window and viewport widths, compared pre-camera C# against post-camera
MAME, applied portrait zoom twice or omitted it, used the wrong C# baseline,
or treated a final normalized width as a gain on an already-transformed
displacement. Identify any other verified domain mismatch.

### D. Derive target width and X gain

From MAME normalized margins, derive desired near-rim width in normalized
720-pixel viewport units, portrait-host pixels, and C# logical units, keeping
the established center fixed. Derive the multiplier on the current implemented
near-rim X displacement. Distinguish the multiplier relative to legacy Level 5,
the multiplier relative to the current `0.670` candidate, and the final net X
gain. Do not compound equivalent corrections.

### E. Check depth consistency and target rule

Determine whether one X factor preserves center at all depths and the current
near/far size ratio, leaves the depth denominator unchanged, and keeps rails,
tube/world anchors, and local tube-oriented X dimensions coherent.
Do not change Y, perspective, or vertical placement.

Specify both MAME-derived normalized left/right margins and the equivalent
candidate near-rim width. Use raster-aware tolerances and recommend a tight,
reasonable automated numerical tolerance; final visual acceptance remains
Neal's.

## Required classification

Select exactly one:

- **FIRST CALIBRATION USED WRONG X DOMAIN** — MAME measurement was reasonable
  but translated into the wrong C# coordinate/presentation stage.
- **FIRST CALIBRATION USED WRONG C# BASELINE EXTENT** — the ratio was applied
  to a C# extent different from the actual candidate stage.
- **FIRST CALIBRATION MEASUREMENT ERROR** — prior MAME near-rim X bounds were
  materially wrong.
- **CURRENT IMPLEMENTATION DOES NOT MATCH CALIBRATION** — code applies the
  intended X factor differently than reported.
- **MULTIPLE CONTRIBUTING ERRORS** — more than one independently verified
  issue contributes.
- **NO TRANSLATION ERROR; MANUAL TARGET DIFFERS** — candidate faithfully
  reproduces measured reference ratio, but Neal's accepted visual target
  requires another composition.
- **UNRESOLVED** — evidence cannot isolate the discrepancy.

If supported, provide exactly one X-only correction specification with target
margins, portrait-host and logical near-rim widths, multiplier relative to the
current candidate, final net X factor, near/middle/far widths, center, and
tolerances. Do not specify Y or placement changes.

## Protected state and hard boundaries

Preserve Y factor `0.880`, vertical placement, HUD clearance, depth
progression, Level 5 points/order/lanes/topology, gameplay, recording/playback,
accepted Levels 1-4, Level 6 fallback, and current Levels 7-9 behavior.

Do not edit source, tests, geometry, or configuration; add tests; implement
the correction; change Y, placement, or depth; alter other levels; fix
unrelated issues; inspect other visual material; launch the game; stage,
commit, or push. No build or full test suite is required. Keep calculations
bounded and temporary.

Record starting repository state and preserve all current work. Stop after
reconstructing C# bounds, remeasuring MAME, reconciling coordinate spaces,
classifying the discrepancy, deriving one supported X-only specification,
demonstrating preservation, and recommending exactly one next milestone.

## Required report

Save `AI-Logs/Reports/2026-11-15-level5-horizontal-margin-diagnostic-report.md`
with the executive classification, confirmation of the authorized image,
C# reconstruction, MAME measurements, coordinate trace, discrepancy cause,
target rule, X-only specification, protected state, decision-relevant
unknowns, smallest next milestone, and starting/ending Git state. Confirm no
tracked or indexed content changed and nothing was staged, committed, or
pushed.

Manual gameplay/visual acceptance remains Neal's responsibility. Do not launch
the game.

TEMPEST_LEVEL5_HORIZONTAL_MARGIN_DIAGNOSTIC_READONLY_20261115
