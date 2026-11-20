READ THIS ENTIRE USER-PROVIDED TASK BEFORE TAKING ANY ACTION.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, reading project files, running commands, or taking any
repository action:

1. Locate the unique completion token near the end of this user-provided task.
2. Verify that exactly one complete token beginning with "TEMPEST_" appears on
   a line by itself in the user-provided task.
3. VS Code/agent-generated reminders, tagged-file information, attachment
   metadata, or other system-generated context may appear after the token and
   do not invalidate it.
4. This check is intended to detect an incomplete or truncated user task, not
   appended agent context.
5. If exactly one complete TEMPEST_ token cannot be found, STOP before project
   or repository action and explain why.

TASK

Perform a bounded READ-ONLY visual/technical investigation answering this
specific question:

WHY IS THE C# LEVEL 7 DIRECT-ATARI PROTOTYPE NOT RENDERING THE SAME TUBE SHAPE
AS THE MAME LEVEL 7 REFERENCE?

The focus is the SHAPE of the tube.

Neal is deliberately providing the MAME and C# screenshots as diagnostic
evidence. Use them actively.

This is not a request to tune C# until it visually resembles a screenshot.
Compare the visible structural differences, then inspect the code and existing
Atari evidence to determine what technical cause or causes could produce those
specific differences.

Do not implement anything.

AUTHORIZED SCREENSHOTS

Inspect these two images:

C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\Level-7\Prototype\Level-7 MAME.png

C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\Level-7\Prototype\Level-7 prototype-cs.png

These screenshots are explicitly authorized for analysis in this task.

Do not inspect, enumerate, search for, or compare other screenshots, videos,
frames, contact sheets, or visual assets.

PRIMARY VISUAL QUESTION

Compare the geometric SHAPE shown by the blue Level 7 tube wireframes.

Identify concretely how the C# tube shape differs from the MAME tube shape.

Concentrate on structural relationships such as:

- the four major lobes;
- the four narrow inward wedges/notches;
- relative angular relationships;
- where adjoining boundaries meet;
- which rails bound each region;
- convergence toward the center/deep rim;
- relative positions of corresponding vertices;
- symmetry;
- adjacency;
- edge orientation;
- lane/polygon shapes;
- whether apparently corresponding lines actually terminate at corresponding
  points;
- whether the visible discrepancy suggests coordinate distortion, incorrect
  endpoint interpretation, incorrect point correspondence, wrong connectivity,
  or another structural transformation.

Do NOT treat differences in the following as the question being asked:

- overall tube size;
- overall scale;
- viewport placement;
- centering;
- vertical position;
- horizontal position;
- HUD position;
- screen resolution;
- portrait presentation size;
- line thickness;
- colors;
- perspective strength as an aesthetic matter.

Perspective/depth mathematics may be examined ONLY if it explains why the
actual SHAPE or structural relationships differ.

For example, if a projection operation moves different vertices by different
relative amounts and thereby changes the shape, that is relevant.

Simply observing that one tube appears larger, smaller, nearer, farther, or
differently positioned is not an answer to this task.

COMPARE NORMALIZED SHAPE

Conceptually normalize away ordinary presentation differences before drawing
conclusions.

Ask:

If translation, overall scale, rotation/reflection where appropriate, and
ordinary final display/aspect differences are ignored, do the two tubes still
have different geometric structure?

Identify which differences survive that normalization.

Do not perform screenshot-derived coordinate fitting for implementation.

Quantitative image measurements are permitted as diagnostic evidence if useful,
but they must not become replacement tube data or implementation constants.

WORK BACKWARD FROM THE VISUAL FAILURE

For each important visible shape difference:

1. Identify the corresponding feature in MAME.
2. Identify the corresponding feature in C#.
3. State exactly how the relationships differ.
4. Determine which stage or stages of the current prototype could mathematically
   produce that type of difference.
5. Check the actual implementation and existing Atari evidence for those
   stages.
6. Separate confirmed causes from plausible hypotheses.

Do not assume the most recently investigated Math Box issue is the answer.

The previous factor-128 experiment is evidence, not a conclusion.

EXISTING INVESTIGATION CONTEXT

Use the existing relevant reports, particularly:

- the Level 7 direct-Atari wireframe prototype report;
- the Level 7 geometry-translation investigation;
- the Level 7 Math Box normal call-state investigation;
- the RN.high reset/startup-state investigation;
- the factor-128 diagnostic experiment report;
- directly relevant Atari projection/PROM reports cited by those investigations.

Do not blindly inherit their conclusions. Use them to avoid repeating already
completed work and to identify assumptions worth testing against the visual
evidence.

CURRENT FACTOR-128 EXPERIMENT

The current screenshot may represent the development-only factor-128
RN=$000F diagnostic experiment.

Verify from the supplied image/context and current implementation what is being
compared before attributing a visible feature to factor 128.

Do not assume factor 128 is normal Atari behavior.

RN.high remains unresolved unless new evidence in this task establishes
otherwise.

The key question here is:

Even after the conditional projection experiment, WHY DOES THE C# SHAPE STILL
DIFFER FROM MAME?

TRACE ONLY RELEVANT TRANSFORMATIONS

Use the screenshots to decide which portions of this chain warrant inspection:

Atari record-06 raw points
-> metadata/reference origins
-> per-axis magnitude calculation
-> divisor/depth input
-> Math Box quotient behavior
-> sign restoration
-> base application
-> coordinate width/clamp/wrap behavior
-> Atari/DVG axis representation
-> near and far endpoint generation
-> point/index correspondence
-> rail connectivity
-> near-rim connectivity
-> far-rim connectivity
-> final display mapping

Do not mechanically reinvestigate every stage from scratch.

Instead, map visible shape symptoms to the stages capable of causing them.

SHAPE VERSUS PROJECTION

Explicitly answer whether the mismatch appears to originate primarily from:

A. different underlying point geometry;

B. incorrect topology/connectivity;

C. correct points/connectivity but incorrect non-uniform projection;

D. incorrect near/far endpoint relationship;

E. incorrect coordinate-space interpretation;

F. more than one of the above;

or

G. evidence is insufficient to distinguish them.

Support the answer with both visual observations and implementation/evidence.

CORRESPONDENCE CHECK

Pay particular attention to whether the C# prototype is connecting the
correct Atari vertices to one another.

A tube can contain the correct individual coordinates yet have an incorrect
shape if:

- point indices are interpreted differently than Atari;
- rim adjacency is not Atari's actual drawing order;
- same-index near/far pairing is not Atari's actual rail relationship;
- the raw table is data rather than direct polygon order;
- open/closed semantics affect drawing structure differently than assumed;
- intermediary Atari vector construction transforms or reorders the points
  before drawing.

These are hypotheses to test, not assumptions that the existing topology is
wrong.

If existing ROM/disassembly evidence definitively establishes one of these
relationships, say so and eliminate that hypothesis.

VISUAL SYMMETRY CHECK

Record 06 has strong apparent structure.

Determine whether the C# output preserves the symmetries and repeated
relationships implied by its input data.

Compare those structural relationships with the MAME shape.

If C# is internally consistent with record 06 but MAME displays a materially
different structure, investigate whether the prototype is using the wrong
interpretation of the record rather than assuming individual coordinates are
incorrect.

LOCAL ATARI EVIDENCE

Use project-local Atari/ROM/disassembly/Math Box evidence where necessary to
test a visually motivated hypothesis.

Do not search the Internet for another Tempest disassembly.

Do not use Mark Incitti's implementation as Atari authority.

Do not alter or fit the architecture to the screenshots.

The screenshots establish observable output. Atari evidence should establish
why that output differs.

READ-ONLY REQUIREMENT

Do not modify:

- source code;
- tests;
- project files;
- project configuration;
- durable documentation;
- tube coordinates;
- projection code;
- geometry;
- viewport mapping;
- presentation;
- gameplay;
- recording/playback;
- accepted Level 1-6 work.

Do not launch the interactive game.

Do not stage, commit, push, reset, restore, stash, rebase, clean, or switch
branches.

Do not create a candidate fix.

AI-LOGS

Follow the current .github/copilot-instructions.md.

The normal ignored AI-Logs records are required and are the permitted
file-writing exception.

Save:

- the substantive prompt under AI-Logs/Prompts/;
- the final investigation report under AI-Logs/Reports/.

Use descriptive date-prefixed paired filenames.

A chat response is not a substitute for the report file.

Do not stage or commit AI-Logs.

EXPECTED REPORT

Answer the user's question directly near the beginning:

"Why is C# not rendering the Level 7 tube with the same shape as MAME?"

Then provide the supporting analysis.

The report must include:

1. A concrete description of the MAME Level 7 tube SHAPE.
2. A concrete description of the C# prototype tube SHAPE.
3. The important structural differences after disregarding overall size,
   placement, centering, and ordinary display scaling.
4. A feature-by-feature correspondence between the two shapes.
5. Which visible differences cannot be explained by a single global
   translation/scale/rotation/aspect adjustment.
6. Whether C# appears to have:
   - wrong source points;
   - wrong connectivity;
   - wrong near/far correspondence;
   - non-uniform projection distortion;
   - wrong coordinate-space interpretation;
   - or another identified structural problem.
7. For each candidate cause, the implementation evidence that supports or
   rejects it.
8. Whether the factor-128 experiment improved, worsened, or merely changed the
   relevant shape relationships, to the extent the supplied evidence can
   establish that without inventing a comparison that is not available.
9. The earliest transformation stage that best explains the observed shape
   divergence, if identifiable.
10. The strongest current explanation of the shape discrepancy in plain
    language.
11. Explicit separation of:
    - visually established facts;
    - ROM/Atari-established facts;
    - implementation-established facts;
    - diagnostic hypotheses;
    - unresolved questions.
12. Whether enough evidence now exists for a bounded correction prompt.
13. If yes, describe the narrow technical correction direction without
    implementing it.
14. If no, identify the single narrowest next investigation necessary.
15. Confirmation that no project source, tests, geometry, projection,
    viewport, presentation, gameplay, or Git state was changed.
16. Exact paths of the saved prompt and report.

DO NOT answer this task with:

"The tubes are different sizes."

"The perspectives are different."

"The tubes are positioned differently."

Those may be observable but are intentionally outside Neal's question.

Determine why the SHAPE itself differs.

Do not ask Neal questions during execution.

Do not pause for input or manual acceptance.

Complete the investigation autonomously and report the findings.

TEMPEST_LEVEL7_MAME_VS_CSHARP_SHAPE_DIAGNOSTIC_INVESTIGATION
