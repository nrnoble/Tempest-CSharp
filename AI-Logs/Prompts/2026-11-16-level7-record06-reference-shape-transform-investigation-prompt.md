READ THIS ENTIRE USER-PROVIDED TASK BEFORE TAKING ANY ACTION.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, reading project files, running commands, inspecting images,
or taking any repository action:

1. Locate the unique completion token near the end of this user-provided task.
2. Verify that exactly one complete token beginning with "TEMPEST_" appears on
   a line by itself in the user-provided task.
3. VS Code/agent-generated reminders, tagged-file information, attachment
   metadata, or other system-generated context may appear after the token and
   do not invalidate it.
4. This check is intended to detect an incomplete or truncated user task, not
   appended agent context.
5. If exactly one complete TEMPEST_ token cannot be found, STOP before any
   project or repository action and explain why.

TASK

Perform a bounded READ-ONLY visual, geometric, and implementation investigation
answering this practical rendering question:

CAN THE RECOVERED ATARI RECORD-06 TUBE GEOMETRY BE USED TO DRAW THE LEVEL 7
TUBE WITH THE SAME VISIBLE SHAPE SHOWN IN THE SUPPLIED REFERENCE SCREENSHOT?

If yes, determine exactly what transformation, coordinate interpretation, or
presentation operation is required and what the current C# direct-Atari
prototype is missing or applying incorrectly.

If no, explain precisely why the recovered record-06 geometry cannot produce
the supplied reference shape under the currently established transformation
model.

Do not implement anything.

REFERENCE IMAGE MEANING

The reference image is:

C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\Level-7\Level7_frame_0090-MAME.png

If that exact path is not present, locate only the specifically named
Level7_frame_0090-MAME.png file within the authorized Level-7 screenshot area.

The filename identifies the source of the capture only.

Do not investigate MAME.

Do not investigate or discuss:

- MAME source code;
- MAME emulation behavior;
- MAME vector implementation;
- MAME video settings;
- MAME aspect settings;
- MAME scaling;
- MAME rendering internals;
- or differences between MAME and original arcade hardware.

Treat Level7_frame_0090-MAME.png solely as a visual reference showing the
Level 7 tube shape C# should be capable of reproducing.

The reference image could equivalently have been captured from Neal's original
arcade game. Its role in this task is unchanged.

C# COMPARISON IMAGE

Use the current C# Level 7 direct-Atari prototype screenshot that corresponds
to the comparison, if present in the authorized Level-7 screenshot area.

Relevant known filenames may include:

- Level-7 prototype-cs.png
- level-7-atari-tube-prototype.png

Identify which available authorized image represents the current direct-Atari
record-06 prototype. Do not inspect unrelated screenshots.

AUTHORIZED IMAGE SCOPE

You MAY inspect only:

1. Level7_frame_0090-MAME.png as the correct-shape visual reference.
2. The directly corresponding current C# Level 7 direct-Atari prototype image.

Do not enumerate, open, inspect, compare, or analyze other repository images,
screenshots, videos, frames, contact sheets, or unknown visual assets.

If more than one possible C# prototype screenshot exists, use only the one
whose filename and current task context clearly identify it as the intended
comparison. Do not conduct a broad image search.

CENTRAL QUESTION

The project has recovered Atari record 06, including its 16 source points and
metadata.

Previous investigations established that the current C# prototype:

- uses record 06 for displayed Level 7;
- preserves the 16-point record order;
- projects the same source index into near and far endpoint sets;
- reproduces Atari's cyclic near-rim adjacency;
- reproduces Atari's cyclic far-rim adjacency;
- connects Near[i] and Far[i] as Atari does;
- treats record 06 as a closed tube;
- and therefore constructs the same logical simple segment graph as Atari.

Given those established facts, answer:

Can those record-06 points and connections be transformed into the visible tube
shape shown in Level7_frame_0090-MAME.png?

Do not answer merely that the images have different aspect ratios.

Determine whether a specific global or staged mathematical transformation
applied to the recovered record-06 geometry produces the reference shape.

SHAPE IS THE FOCUS

The focus is the visible geometric SHAPE of the tube.

Ignore absolute:

- size;
- placement;
- centering;
- vertical position;
- horizontal position;
- viewport margins;
- HUD position;
- screen resolution;
- line brightness;
- line thickness;
- color;
- and final crop.

Do not reject a candidate transformation merely because the resulting tube
would initially require separate size or placement calibration. Those are not
part of this task.

Concentrate on whether corresponding record-06 features can be placed into the
same relative arrangement as the reference:

- four broad lobes;
- four narrow inward wedges/notches;
- lobe width and height relationships;
- wedge direction and proportions;
- corresponding vertex angles;
- relative horizontal and vertical distances;
- near-rim shape;
- far-rim shape;
- rail directions;
- convergence relationships;
- repeated/opposite-point relationships;
- and the normalized overall silhouette.

Do not assume the reference shape is merely a taller version of the C# shape.
Test that proposition mathematically.

Do not assume it is more complicated than a global transform. Test the
simplest sufficient transformation first.

RECORD-06 GEOMETRY MUST REMAIN AUTHORITATIVE

Do not replace, redraw, trace, fit, or invent record-06 geometry points.

Do not derive a new 16-point tube from screenshot pixels.

Do not create per-point offsets.

Do not move individual vertices independently to force a match.

Do not introduce Level-7-specific geometry corrections.

The question is whether the correct visible shape is already latent in Atari
record 06 and is being presented incorrectly by C#.

The screenshot may diagnose the missing transformation. It must not become a
replacement source of tube geometry.

TRANSFORMATION ANALYSIS

Evaluate the smallest transformation classes in a disciplined order.

At minimum, test whether the reference shape can be explained by:

1. translation only;
2. uniform scale plus translation;
3. rotation/reflection plus uniform scale and translation;
4. global anisotropic X/Y scale plus rotation/reflection and translation;
5. a general global affine transform;
6. an established Atari projection or coordinate-space operation currently
   missing from C#;
7. a difference applied separately to near and far endpoint sets;
8. another mathematically justified transformation already present in Atari's
   pipeline.

Do not jump to a general affine or point-specific fit if a simpler global
transformation is sufficient.

For every model tested, state:

- parameters or degrees of freedom;
- what source points or landmarks were compared;
- residual geometric mismatch after alignment;
- whether opposite/repeated relationships are preserved;
- whether near and far structures are both explained;
- whether the model preserves topology and same-index rail pairing;
- and whether the model is supported by Atari evidence, C# architecture,
  mathematics, visual diagnostics, or only inference.

QUANTITATIVE IMAGE ANALYSIS

Bounded quantitative image analysis is permitted and expected if it helps
answer the question reliably.

You may measure from the two authorized screenshots:

- blue tube-line bounds;
- corresponding visible vertices;
- normalized feature positions;
- relative lobe extents;
- wedge locations;
- near/deep rim landmarks;
- rail slopes;
- and residuals after applying candidate global transformations.

Use enough well-distributed corresponding landmarks to distinguish:

- simple X compression;
- rotation or reflection;
- global affine transformation;
- near/far-specific transformation;
- and true point-arrangement mismatch.

Do not use only outer bounding boxes if internal landmarks can distinguish the
models.

Clearly label all screenshot-derived measurements as:

VISUAL DIAGNOSTIC MEASUREMENTS

They are not Atari ROM data and not Atari hardware calibration facts.

Do not copy measured screenshot coordinates into production geometry.

MATHEMATICAL DECISION

Determine whether there exists a transformation T such that, within reasonable
screenshot and rasterization tolerance:

T(record-06 C# rendered geometry) = reference-image tube shape

where T does not alter:

- source point identity;
- cyclic adjacency;
- near/far index correspondence;
- rail connectivity;
- topology;
- or record-06 geometry data.

If a sufficiently simple T exists:

- identify the smallest sufficient transformation;
- state its mathematical form;
- state which coordinate-space boundary should own it;
- explain why it reproduces the reference shape;
- and distinguish evidence for the transformation's sufficiency from evidence
  about its historical Atari origin.

If no such sufficiently simple T exists:

- identify the residual mismatches;
- state why global display mapping is insufficient;
- and identify the earliest remaining coordinate or transformation stage that
  must differ.

CURRENT C# PIPELINE

Trace the current C# direct-Atari path sufficiently to identify where the
required transformation could or should occur:

- AtariTubeRecordCatalog;
- AtariCoordinateProjector;
- AtariTubeVectorListBuilder;
- AtariDvgViewportMap;
- direct-prototype VectorRenderer path;
- portrait logical-to-screen camera mapping;
- and final raster coordinates.

Identify every current operation affecting:

- axis permutation;
- axis sign;
- rotation;
- translation;
- uniform scale;
- X-only scale;
- Y-only scale;
- display aspect;
- and final camera zoom.

Determine whether the smallest sufficient transformation is already
representable by AtariDvgViewportMap or another existing presentation boundary.

Do not modify that code.

ATARI EVIDENCE VERSUS PRACTICAL SUFFICIENCY

Separate these two questions:

A. Can record 06 mathematically produce the reference shape?

B. Does available Atari evidence establish why or where that transformation
   occurred on original hardware?

Question A may be answered yes even if Question B remains unresolved.

Do not refuse to answer Question A merely because original AVG analog hardware
documentation is unavailable.

Likewise, do not claim Question B is answered merely because a visual transform
matches the screenshot.

Classify conclusions carefully:

- ROM-BACKED or ATARI-DOCUMENTED;
- implementation-established;
- mathematically established;
- visual diagnostic;
- DEVELOPMENT-ONLY;
- inference;
- unresolved.

Do not describe a screenshot-derived transform as Atari hardware-backed.

Do not describe a mathematically sufficient transformation as historically
proven unless Atari evidence independently supports it.

PRIOR REPORTS

Use relevant existing reports to avoid repeating settled work, including:

- the Level 7 direct-Atari wireframe prototype report;
- the Level 7 shape diagnostic report;
- the record-06 vector segment reconstruction report;
- the Atari visible X/Y aspect/gain investigation;
- relevant projection and coordinate-translation reports.

The latest structural findings to preserve unless new direct evidence
contradicts them are:

- record-06 point order is preserved;
- cyclic rim connectivity is equivalent;
- same-index rail pairing is equivalent;
- no unique visible Atari segment is omitted;
- Atari's duplicated rail emission changes emission multiplicity, not endpoint
  arrangement;
- original Atari physical X/Y gain remains unresolved;
- C# currently uses provisional equal X/Y final gain.

Do not repeat the RN.high/reset investigation.

Do not repeat factor-128 diagnosis unless necessary to identify which C#
comparison image is being measured.

Do not assume factor 128 is Atari-correct.

PRACTICAL ANSWER REQUIRED

Answer the practical rendering question directly.

Do not stop at:

"The Atari hardware aspect ratio is unknown."

That fact is already established.

The report must determine whether the known record-06 geometry can, as a matter
of geometry and rendering, be transformed into the supplied reference shape.

Acceptable direct-answer forms include:

- YES: transformation X is sufficient;
- YES CONDITIONALLY: transformation family X is sufficient, but parameter Y
  remains visually derived or historically unproven;
- NO: transformation classes X through Y fail, with identified residual Z;
- UNRESOLVED: available image correspondence cannot distinguish between
  specifically identified alternatives.

Do not answer with a general statement that more evidence would be helpful
without performing the bounded comparison authorized here.

NO IMPLEMENTATION

Do not:

- edit source;
- edit tests;
- edit project files;
- edit configuration;
- edit durable documentation;
- alter record-06 data;
- alter projection code;
- alter vector construction;
- alter viewport mapping;
- alter presentation;
- alter gameplay;
- add a diagnostic option;
- generate replacement points;
- launch the interactive game;
- stage, commit, push, reset, restore, stash, rebase, clean, or switch branches.

Temporary bounded analysis outside tracked project content is permitted.

Do not save temporary measurement or analysis artifacts in tracked project
paths.

REPOSITORY SAFETY

At the beginning, record:

- branch;
- HEAD;
- upstream/tracking state;
- index state;
- tracked worktree changes;
- relevant untracked state.

Preserve all pre-existing candidate work and protected assets.

At completion, verify that no project or Git state changed except for the
required ignored paired AI-Logs records.

AI-LOGS

Follow the current .github/copilot-instructions.md.

Save:

- this substantive prompt under AI-Logs/Prompts/;
- the completed report under AI-Logs/Reports/.

Use descriptive date-prefixed paired filenames.

A chat response is not a substitute for the report file.

Do not stage or commit AI-Logs.

EXPECTED REPORT

Save the report under AI-Logs/Reports/ and summarize the result in chat.

Near the beginning, directly answer:

"Can the recovered Atari record-06 geometry be used to draw the Level 7 tube
with the same visible shape shown in Level7_frame_0090-MAME.png?"

The report must include:

1. The exact authorized images inspected.
2. The relevant source and prior-report evidence inspected.
3. A concise description of record-06 geometry and settled connectivity.
4. The current C# transformation chain.
5. The reference image's normalized tube-shape characteristics.
6. The C# image's normalized tube-shape characteristics.
7. The corresponding landmarks selected for comparison.
8. All screenshot-derived measurements clearly labeled as visual diagnostics.
9. The candidate transformation classes tested.
10. The residual mismatch for each relevant candidate class.
11. Whether translation or uniform similarity is sufficient.
12. Whether anisotropic X/Y scaling is sufficient.
13. Whether a general affine transform is necessary or sufficient.
14. Whether near and far endpoint sets require different treatment.
15. Whether any per-point or connectivity change would be required.
16. The smallest sufficient transformation, if one exists.
17. Its mathematical form and coordinate-space location.
18. Whether the current C# map can represent it without architectural redesign.
19. Why the current C# output does not already apply it.
20. Clear separation between:
    - Atari/ROM-established facts;
    - C# implementation-established facts;
    - mathematical findings;
    - visual diagnostic findings;
    - historical hardware unknowns.
21. A direct plain-language explanation of why C# currently does not draw the
    reference shape.
22. Whether enough evidence exists for a bounded DEVELOPMENT-ONLY
    implementation candidate.
23. If yes, describe the smallest candidate direction without implementing it.
24. If no, identify the single narrowest remaining obstacle.
25. Confirmation that no project source, tests, geometry, projection, viewport,
    presentation, gameplay, protected material, or Git state changed.
26. Exact paths of the saved prompt and report files.

Do not ask Neal questions during execution.

Do not pause for manual input or acceptance.

Complete the investigation autonomously and stop after reporting the evidence.

TEMPEST_LEVEL7_RECORD06_REFERENCE_SHAPE_TRANSFORM_INVESTIGATION