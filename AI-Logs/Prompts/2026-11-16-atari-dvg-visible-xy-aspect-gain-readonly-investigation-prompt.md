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

Perform a bounded READ-ONLY investigation of the Atari Tempest
DVG-to-visible-display coordinate transformation, specifically the relative
horizontal and vertical aspect/gain applied between logical vector coordinates
and the visible arcade presentation.

The central question is:

DOES ORIGINAL ATARI TEMPEST PRESENT LOGICAL DVG X AND Y WITH EQUAL VISIBLE
GAIN, OR DOES THE ORIGINAL ARCADE DISPLAY PIPELINE APPLY A NON-SQUARE X/Y
TRANSFORMATION THAT C# CURRENTLY DOES NOT MODEL?

If a non-square transformation exists, determine where it originates and
whether the available evidence establishes its ratio or behavior strongly
enough to justify a future C# presentation correction.

This is investigation only.

Do not implement anything.

WHY THIS INVESTIGATION IS NOW JUSTIFIED

The preceding Level 7 record-06 vector-segment reconstruction established that
the C# direct-Atari prototype uses the same logical segment graph as Atari:

- same 16 record-06 source indices;
- same cyclic near-rim neighbors;
- same cyclic far-rim neighbors;
- same-index Near[i] / Far[i] rail pairing;
- same closed-rim topology.

Atari emits each rail twice while C# emits it once, but that emission
multiplicity does not rearrange endpoints or alter the logical silhouette.

Therefore segment connectivity is no longer a supported explanation for the
visible Level 7 shape difference.

A prior screenshot/code investigation also observed that:

- the C# direct-Atari prototype currently uses a provisional unit display
  aspect;
- the supplied comparison image has visibly different horizontal-versus-
  vertical proportions;
- and the current final DVG-to-display mapping is explicitly provisional.

This investigation must now determine whether that difference has an actual
Atari engineering basis rather than fitting C# to an image.

READ-ONLY SCOPE AND REQUIRED AI-LOGS

Follow the current .github/copilot-instructions.md.

This task is read-only with respect to:

- project source;
- tests;
- project configuration;
- durable documentation;
- protected reference material;
- screenshots;
- geometry;
- projection;
- viewport mapping;
- presentation;
- gameplay;
- and Git state.

The normal ignored AI-Logs prompt/report records are REQUIRED and are the sole
permitted file-writing exception.

Save:

- this substantive prompt under AI-Logs/Prompts/;
- the completed report under AI-Logs/Reports/.

Use descriptive date-prefixed paired filenames.

A report returned only in chat does not satisfy the report requirement.

Do not stage or commit AI-Logs.

DO NOT INVESTIGATE MAME EMULATION

The supplied comparison screenshot should be treated only as visual reference
evidence of the expected tube presentation.

The comparison image could have come from original arcade hardware.

Do not investigate:

- MAME source;
- MAME emulation implementation;
- MAME video configuration;
- MAME aspect settings;
- MAME scaling;
- MAME rendering;
- or MAME display internals.

The question concerns Atari's original DVG-to-visible-display pipeline and the
current C# presentation mapping.

ATARI PIPELINE TO TRACE

Trace the original presentation path far enough to distinguish logical vector
coordinates from physical visible coordinates.

Investigate, where locally documented:

1. Tempest logical/vector coordinate representation.
2. DVG coordinate width and range.
3. DVG vector serialization.
4. X and Y arithmetic before vector output.
5. Any digital X/Y scaling before DAC conversion.
6. X and Y DAC paths.
7. Analog gain stages.
8. Deflection-amplifier X/Y scaling.
9. Monitor orientation.
10. Horizontal versus vertical deflection sensitivity.
11. Any adjustable X-size or Y-size controls.
12. Any board-level X/Y gain calibration.
13. Any monitor-level horizontal/vertical size calibration.
14. Whether logical equal-length X and Y vectors are intended to produce equal
    physical lengths.
15. Whether any coordinate-axis swap occurs before physical deflection.
16. Whether cabinet rotation changes orientation without changing gain.
17. Whether Atari documentation establishes a nominal X:Y visible scale or
    leaves it cabinet-adjustable.

Do not assume that a square digital coordinate range implies equal physical
deflection.

Likewise, do not assume that a portrait monitor implies non-square vector gain.

Distinguish those concepts from evidence.

LOCAL EVIDENCE FIRST

Use the strongest available project-local Atari evidence.

Relevant evidence may include:

- Reference/arcade/Tempest Rom Source code.md;
- project-local Atari DVG/vector documentation;
- project-local hardware/service documentation;
- schematic-derived notes already present in the repository;
- existing Atari projection/DVG investigation reports;
- existing presentation architecture investigations;
- current C# DVG/display mapping code.

Search locally for evidence concerning terms such as:

- DVG;
- vector generator;
- X DAC;
- Y DAC;
- X size;
- Y size;
- X gain;
- Y gain;
- deflection;
- vector monitor;
- monitor;
- analog;
- DAC;
- scale;
- aspect;
- display;
- rotation;
- calibration.

Do not assume filenames in advance.

Do not search the Internet for substitute Tempest disassemblies.

If important original Atari hardware documentation is absent locally, identify
the missing evidence precisely rather than filling the gap with assumptions.

CURRENT C# PATH

Trace the current direct-Atari prototype from canonical DVG coordinates to
visible pixels.

Identify every operation affecting:

- X;
- Y;
- axis permutation;
- axis sign;
- rotation;
- translation;
- common scale;
- per-axis scale;
- display aspect;
- portrait viewport transformation;
- final pixel coordinates.

Determine exactly where the current implementation assumes equal X/Y visible
gain.

Identify whether that assumption is:

- required by Atari evidence;
- an accepted project translation;
- a provisional development choice;
- or unsupported.

Do not change it.

SEPARATE COORDINATE SYSTEMS

Explicitly distinguish at least:

A. raw tube-record coordinates;

B. projected Atari source coordinates;

C. DVG logical coordinates;

D. DVG command/vector deltas;

E. DAC/analog deflection quantities, if documented;

F. physical monitor coordinates;

G. current C# logical portrait coordinates;

H. final C# raster/pixel coordinates.

Do not collapse these spaces into one another.

For every discovered X/Y scaling operation, state which coordinate-space
boundary it belongs to.

ATARI X/Y GAIN QUESTION

Determine whether original Atari evidence supports any of the following:

1. equal logical DVG units produce equal physical X/Y deflection;
2. X and Y use different nominal digital gains;
3. X and Y use equal digital values but different analog gains;
4. X/Y gain is explicitly adjustable;
5. final visible proportions depend on cabinet/monitor calibration;
6. a fixed nominal ratio exists but can be adjusted;
7. the available evidence does not establish a unique ratio.

If there are independent X-size/Y-size controls, pots, resistors, DAC scaling,
amplifier gains, or monitor adjustments, trace their function sufficiently to
determine whether they materially alter visible geometry.

Do not infer a numeric ratio merely from component names.

NUMERIC RATIO

If primary or sufficiently strong Atari evidence establishes a nominal relative
X/Y visible gain, calculate it.

State clearly:

Visible X per DVG unit
relative to
Visible Y per DVG unit.

Use a normalized ratio such as:

X:Y = 1.0:k

or equivalent.

Show the evidence and calculation.

If the electronics permit adjustment around a nominal value, distinguish:

- circuit nominal;
- adjustment range, if documented;
- factory/service calibration target, if documented;
- and unknowable cabinet-specific final setting.

Do not claim a unique arcade ratio when the hardware evidence establishes only
an adjustable relationship.

COMPARISON IMAGE ROLE

The two authorized Level 7 images may be inspected if useful:

C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\Level-7\Prototype\Level-7 MAME.png

C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\Level-7\Prototype\Level-7 prototype-cs.png

Treat the first file solely as the supplied correct-shape comparison image,
regardless of its filename.

Do not infer anything about MAME implementation from it.

The images may be used to ask:

"Is the visually required kind of X/Y transformation consistent with the
independently established Atari hardware/display evidence?"

The images must NOT be used to establish Atari architecture or invent a gain
ratio.

Do not fit implementation constants to the image.

If useful, a measured image ratio may be reported separately as:

VISUAL DIAGNOSTIC MEASUREMENT

It must not be labeled:

ATARI HARDWARE RATIO

unless independent Atari evidence establishes the same ratio.

SHAPE FOCUS

Neal's concern is the visible Level 7 tube shape.

The relevant question is whether correct record-06 geometry and correct segment
connectivity can appear with the wrong proportions solely because the final
DVG-to-visible-display X/Y relationship is wrong.

Explain mathematically whether a non-uniform X/Y gain can convert the current
C# arrangement into the type of shape shown by the comparison image while
preserving:

- point indices;
- adjacency;
- near/far pairing;
- topology;
- relative ordering;
- and all underlying tube geometry.

This is diagnostic analysis, not authorization to implement or tune it.

DO NOT REOPEN SETTLED STRUCTURAL WORK

Do not re-investigate unless directly contradicted by new evidence:

- record-06 raw point values;
- record-06 ordering;
- near/far same-index correspondence;
- cyclic rim adjacency;
- closed-rim topology;
- rail connectivity;
- the segment reconstruction.

Do not repeat the Math Box RN.high investigation.

Do not repeat factor-128 experimentation.

Those are outside this task.

STRICT EXCLUSIONS

Do not modify or investigate unrelated:

- gameplay geometry;
- gameplay mechanics;
- enemies;
- collision;
- timing;
- RNG;
- recording/playback;
- HUD layout;
- accepted Levels 1-6 presentation;
- Levels 8-9 presentation;
- Blaster-exit rendering;
- Approach;
- the existing --atari-presentation candidate except where necessary to
  identify the current final display-map assumption.

Do not implement a new diagnostic.

Do not modify the viewport.

Do not modify display aspect.

Do not alter projection.

Do not alter record data.

Do not launch the interactive game.

EVIDENCE CLASSIFICATION

Use the project's established evidence classifications.

In particular, distinguish:

- ATARI-DOCUMENTED / ROM-BACKED facts;
- hardware-derived facts, with exact source;
- implementation-established C# behavior;
- accepted PROJECT TRANSLATION;
- DEVELOPMENT-ONLY provisional behavior;
- visual diagnostic measurements;
- inference;
- unknown.

Do not label a screenshot-derived ratio as Atari-documented.

Do not label a provisional C# map as evidence about Atari hardware.

BOUNDED CALCULATION

Bounded temporary calculations outside tracked project content are allowed if
needed to:

- calculate relative X/Y gains from documented electronics;
- compare coordinate ranges;
- transform existing known point sets;
- determine whether a global anisotropic transform preserves topology;
- compare a documented hardware ratio with a visual diagnostic ratio.

Do not write temporary analysis files into tracked project paths.

Any potentially long command, search, calculation, or trace must be bounded.

No interactive launches.

DECISION QUESTIONS

The final investigation must answer:

1. Does Atari's original display pipeline apply equal or unequal visible X/Y
   gain to logical vector coordinates?
2. If unequal, where does that inequality originate?
3. Is the relative gain fixed, nominal-but-adjustable, fully adjustable, or
   unresolved?
4. Does project-local evidence establish a numeric nominal ratio?
5. Does the current C# direct-Atari prototype model that relationship?
6. Could the missing/different relationship explain the supplied Level 7
   shape discrepancy without changing record-06 geometry or connectivity?
7. Is there now enough evidence for a bounded implementation candidate?

REPOSITORY SAFETY

Before investigation, record:

- branch;
- HEAD;
- upstream/tracking;
- index;
- tracked worktree state;
- relevant untracked state.

Preserve all pre-existing work.

Do not:

- reset;
- restore;
- clean;
- stash;
- stage;
- commit;
- push;
- rebase;
- switch branches.

At completion, verify that no project or Git state changed except for the
required ignored paired AI-Logs records.

EXPECTED REPORT

Save the paired report in AI-Logs/Reports/ and summarize the result in chat.

The report must include:

1. Direct answer to:
   "What X/Y aspect or gain relationship did Atari apply between DVG logical
   coordinates and the visible Tempest display?"
2. Local sources inspected.
3. Atari coordinate-space chain from DVG to visible deflection.
4. Any X/Y digital scaling found.
5. Any X/Y DAC or analog scaling found.
6. Any X/Y deflection-amplifier gain difference found.
7. Any separate X/Y size/gain adjustments found.
8. Monitor orientation and its relevance or irrelevance to gain.
9. Whether equal logical X/Y units produce equal visible lengths.
10. Any established nominal X:Y gain ratio and its derivation.
11. Any documented calibration/adjustability boundary.
12. Current C# DVG-to-visible mapping.
13. Exact point where C# assumes unit/equal display aspect.
14. Classification of that C# assumption.
15. Whether the comparison-image shape difference is mathematically consistent
    with the missing Atari X/Y transformation.
16. Clear separation between Atari-established ratio and screenshot-derived
    diagnostic measurement.
17. Whether this explains the Level 7 shape discrepancy without changing
    geometry or connectivity.
18. Whether enough evidence exists for a bounded implementation candidate.
19. If yes, describe the smallest technically justified future correction
    direction without implementing it.
20. If no, identify the single narrowest remaining evidence gap.
21. Confirmation that no source, tests, geometry, projection, viewport,
    presentation, gameplay, protected material, or Git state changed.
22. Exact paths of the saved prompt and report.

Do not ask Neal questions during execution.

Do not pause for manual input or acceptance.

Complete this bounded read-only investigation autonomously and stop after
reporting the evidence.

TEMPEST_DVG_VISIBLE_XY_ASPECT_GAIN_READONLY_INVESTIGATION