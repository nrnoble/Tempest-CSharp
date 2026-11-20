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

Perform a narrowly bounded READ-ONLY reconstruction of the actual Atari
record-06 vector segment endpoint list used to draw the Level 7 tube.

Trace the relevant producers and consumers of the vector-coordinate data around
$035A-$038A and the vector-building routines around $C3EE and $C43C.

Then compare Atari's reconstructed segment list directly with the C# direct-
Atari prototype's assumed construction:

- one 16-edge near rim;
- one 16-edge far rim;
- 16 rails connecting near[index] to far[index].

The central question is:

DOES THE C# DIRECT-ATARI PROTOTYPE CONNECT AND ARRANGE THE RECOVERED RECORD-06
POINTS IN THE SAME WAY THAT THE ATARI VECTOR-BUILDING PATH CONNECTS AND ARRANGES
THEM?

This task is about point arrangement, segment endpoints, connectivity, and
drawing structure.

It is not about overall size, placement, centering, final display aspect,
aesthetic perspective, or MAME emulation.

Do not implement a correction.

DO NOT USE MAME AS AN IMPLEMENTATION SUBJECT

The previously supplied comparison image is simply visual evidence of the
expected Level 7 tube shape. It could have been captured from original arcade
hardware.

Do not investigate:

- MAME source code;
- MAME emulation behavior;
- MAME video configuration;
- MAME display scaling;
- MAME aspect correction;
- MAME rendering internals;
- or differences between MAME and original hardware.

The technical comparison in this task is:

ATARI'S OWN RECORD-06 VECTOR CONSTRUCTION

versus

THE CURRENT C# DIRECT-ATARI PROTOTYPE VECTOR CONSTRUCTION.

The supplied comparison image does not establish implementation provenance and
must not be treated as evidence about how MAME works.

READ-ONLY SCOPE AND REQUIRED AI-LOGS

Follow the current .github/copilot-instructions.md.

This task is read-only with respect to project source, tests, configuration,
durable documentation, protected reference material, screenshots, geometry,
projection, presentation, gameplay, and Git state.

The required ignored AI-Logs files are the sole permitted file-writing
exception:

- Save this substantive prompt under AI-Logs/Prompts/.
- Save the completed investigation report under AI-Logs/Reports/.
- Use descriptive date-prefixed paired filenames.
- A report returned only in chat does not satisfy the report requirement.
- Do not stage or commit AI-Logs.

CONTEXT

The C# direct-Atari Level 7 prototype contains the recovered record-06 point
data and metadata.

Previous investigations established or strongly supported:

- displayed Level 7 selects Atari record 06;
- the record contains 16 X/Y point pairs;
- record 06 is closed;
- the C# catalog preserves the recovered record order;
- Atari projects far endpoints at p=$F0 and near endpoints at p=$10;
- the C# prototype projects the same 16 source points at both positions;
- the C# prototype constructs two closed cyclic rims;
- the C# prototype connects near[index] to far[index] for all 16 indices;
- Atari's closed-rim traversal appears to use the same cyclic neighbors as
  C#, although in the opposite traversal direction;
- the complete Atari rail/vector endpoint construction through the relevant
  $C3EE/$C43C path was not fully reconstructed;
- the data around $035A-$038A has additional indexed vector-coordinate use
  that must be traced to settle the remaining connectivity question.

The prior screenshot investigation concluded that the exact Atari segment list
is the narrowest remaining structural issue worth resolving before considering
any correction.

Do not assume its unresolved connectivity question is a defect. Determine the
actual segment list from evidence.

PRIMARY INVESTIGATION OBJECTIVES

Reconstruct, as completely as the available local Atari evidence permits:

1. Every Level 7 record-06 near-rim segment endpoint pair emitted by Atari.
2. Every Level 7 record-06 far-rim segment endpoint pair emitted by Atari.
3. Every rail or corresponding near-to-far segment endpoint pair emitted by
   Atari.
4. Any additional tube lines, duplicated lines, omitted lines, closure lines,
   intermediate segments, transformed endpoints, or special segments involved
   in the normal Level 7 tube wireframe.
5. The order in which those segments are emitted.
6. Whether drawing order changes geometry or only traversal.
7. The relationship between raw record point indices and emitted vector
   endpoints.
8. Whether Atari reorders, remaps, offsets, pairs, or otherwise transforms
   indices before constructing rails or rims.
9. Whether the data at $035A-$038A represents:
   - projected point arrays;
   - vector deltas;
   - endpoint coordinates;
   - temporary/intermediate coordinate sets;
   - reordered point data;
   - or another structure.
10. Whether the C# model of two cyclic rims plus same-index rails is exactly
    equivalent to the Atari segment graph.

TRACE THE ACTUAL DATA FLOW

Trace the relevant data flow rather than relying on variable names or prior
summaries.

As applicable, identify:

- where record-06 raw points are copied;
- where the p=$F0 endpoint set is projected and stored;
- where the p=$10 endpoint set is projected and stored;
- the exact memory layouts of those endpoint sets;
- all producers of $035A-$038A;
- all consumers of $035A-$038A;
- the purpose and indexing of related arrays near those addresses;
- how $C3EE obtains the coordinates or indices it uses;
- how $C43C obtains the coordinates or indices it uses;
- any relevant helper routines invoked by those paths;
- loop counters and traversal direction;
- wraparound or closure handling;
- record-open/closed branches;
- index arithmetic;
- pointer arithmetic;
- coordinate serialization;
- DVG command construction;
- and the final logical segment endpoints implied by the commands.

Follow calls and branches far enough to reconstruct the segment relationships,
but do not broaden into unrelated vector effects or gameplay systems.

RECONSTRUCT A LOGICAL SEGMENT LIST

Produce a normalized logical segment list independent of final display
orientation, scale, placement, or raster aspect.

Use a notation such as:

- Near[i] -> Near[j]
- Far[i] -> Far[j]
- Near[i] -> Far[j]

or another equally explicit representation.

For every segment class, state:

- source point index;
- destination point index;
- endpoint set used;
- traversal direction;
- whether the segment is emitted once or multiple times;
- whether the segment is conditional;
- and the source evidence establishing it.

Where the emitted Atari command uses deltas rather than absolute endpoints,
resolve the deltas back to logical endpoint relationships when evidence allows.

Do not substitute assumptions merely because the output visually resembles a
tube.

COMPARE WITH THE CURRENT C# BUILDER

Inspect the current relevant C# implementation, including as necessary:

- AtariTubeRecordCatalog;
- AtariCoordinateProjector;
- AtariTubeVectorListBuilder;
- the direct-prototype VectorRenderer path;
- and tests that assert point ordering, rim edges, or rail pairing.

Compare the reconstructed Atari logical segment list with the C# list.

Answer separately:

1. Does C# use the same near-rim adjacency?
2. Does C# use the same far-rim adjacency?
3. Does C# pair each near point with the same far point as Atari?
4. Does C# omit any Atari segment?
5. Does C# add any segment Atari does not draw?
6. Does C# duplicate any segment Atari draws once?
7. Does C# close a rim Atari leaves open?
8. Does C# use a direct endpoint where Atari uses an intermediate or reordered
   endpoint?
9. Does the difference in traversal direction have any geometric consequence?
10. Is the C# segment graph exactly isomorphic to Atari's segment graph after
    ignoring drawing order and final display mapping?

SHAPE-ARRANGEMENT FOCUS

The user believes the required geometry points are present but are not being
arranged correctly in the C# output.

Test that proposition directly.

Do not respond merely that the screenshots differ in aspect ratio.

Do not normalize away a segment-connectivity error.

Do not assume the user is correct that connectivity is wrong, but determine
whether the C# arrangement is or is not equivalent to Atari's actual vector
construction.

If the segment graphs are equivalent, say so clearly and prove the equivalence
from the reconstructed endpoint list.

If the segment graphs differ, identify the first exact difference and explain
how it changes the visible tube shape.

SOURCE AUTHORITY

Use project-local primary Atari evidence first, including:

- Reference/arcade/Tempest Rom Source code.md
- relevant project-local Atari documents;
- relevant recovered table data;
- existing local investigation reports only as navigation and prior context.

Do not search the Internet for another Tempest disassembly.

Do not inspect MAME source.

Do not use Mark Incitti's implementation as Atari authority.

Do not promote comments or inferred variable names to ROM-backed conclusions
without tracing the instructions and data flow that support them.

If raw local ROM verification is genuinely required, follow the current
project instructions and keep verification narrowly bounded. Do not open or
enumerate protected ROM files unnecessarily.

SCREENSHOTS

This task should normally be resolved from Atari vector construction and C#
code.

The two previously authorized Level 7 comparison screenshots may be used only
as bounded visual context if necessary to explain the consequence of a
confirmed segment-list difference.

Do not inspect or enumerate other images, screenshots, videos, frames, or
visual assets.

Do not measure or fit replacement coordinates from screenshots.

Do not derive implementation constants from images.

EXCLUSIONS

Do not investigate or modify:

- factor 128 versus factor 256;
- RN.high;
- Math Box power-on/reset state;
- record-06 raw coordinate values;
- record-06 metadata values;
- p=$10 or p=$F0;
- projection magnitude formulas;
- Y3D or Y2D;
- coordinate signs or bases;
- DVG-to-display aspect;
- viewport translation;
- viewport scale;
- viewport rotation;
- HUD layout;
- tube size;
- tube placement;
- aesthetic perspective calibration;
- gameplay geometry;
- lane behavior;
- entities;
- collision;
- timing;
- RNG;
- recording/playback;
- accepted Levels 1-6;
- Levels 8-9;
- the existing --atari-presentation path;
- the separate Blaster-exit presentation defect.

These matters are outside this segment-connectivity investigation.

Do not launch the interactive game.

Do not implement a diagnostic or candidate correction.

EVIDENCE STANDARD

For each conclusion, distinguish:

- ROM-BACKED or Atari-documented behavior;
- implementation-established C# behavior;
- inference;
- hypothesis;
- unresolved behavior.

Do not call same-index rail pairing ROM-backed unless the traced segment
construction actually establishes same-index pairing.

Do not call a visual resemblance proof of endpoint equivalence.

If an address range or routine remains ambiguous, state exactly what could and
could not be reconstructed.

BOUNDED TECHNICAL CALCULATIONS

Bounded temporary calculations outside tracked project content are permitted
if useful for:

- decoding index arithmetic;
- reconstructing loops;
- converting pointer offsets to point indices;
- comparing segment sets;
- checking graph equivalence;
- or detecting crossings or mismatched pairings.

Do not write analysis scripts into tracked project paths.

Do not modify source or tests.

Any long trace or command must have an explicit bound. Stop and report rather
than entering an unbounded reconstruction.

REPOSITORY SAFETY

At the beginning, record:

- branch;
- HEAD;
- upstream/tracking state;
- index state;
- tracked worktree changes;
- relevant untracked state.

Preserve all pre-existing work.

Do not reset, restore, clean, stash, rebase, switch branches, stage, commit, or
push.

At completion, verify that no project or Git state changed except for the
required ignored paired AI-Logs records.

EXPECTED REPORT

Save the report under AI-Logs/Reports/ and summarize it in chat.

Answer this question directly near the beginning:

"Does the C# Level 7 direct-Atari prototype arrange and connect record-06 points
the same way Atari's vector-building code does?"

The report must include:

1. The exact local evidence sources inspected.
2. The relevant routine and address call graph.
3. The memory layout and meaning of the endpoint/vector arrays involved.
4. All established producers and consumers of $035A-$038A.
5. The reconstructed record-06 near-rim segment list.
6. The reconstructed record-06 far-rim segment list.
7. The reconstructed record-06 rail segment list.
8. Any additional or special Atari-emitted tube segments.
9. Traversal direction and closure behavior.
10. A normalized graph representation of Atari's segment construction.
11. A normalized graph representation of C#'s segment construction.
12. A segment-by-segment or set-based comparison.
13. Whether the graphs are exactly equivalent after ignoring drawing order and
    final display mapping.
14. Any first confirmed mismatch in indexing, pairing, adjacency, omission,
    duplication, or closure.
15. If a mismatch exists, how that mismatch changes the tube's visible shape.
16. If no mismatch exists, proof sufficient to eliminate segment connectivity
    as the shape cause.
17. Whether enough evidence exists for a bounded correction prompt.
18. If yes, the smallest technically justified correction direction, without
    implementing it.
19. If no, the single narrowest unresolved issue remaining.
20. Confirmation that no geometry, projection, viewport, presentation,
    gameplay, source, tests, or Git state changed.
21. Exact paths of the saved prompt and report files.

Do not ask Neal questions during execution.

Do not pause for input or manual acceptance.

Complete the read-only reconstruction autonomously and stop after reporting the
evidence.

TEMPEST_LEVEL7_ATARI_RECORD06_VECTOR_SEGMENT_RECONSTRUCTION