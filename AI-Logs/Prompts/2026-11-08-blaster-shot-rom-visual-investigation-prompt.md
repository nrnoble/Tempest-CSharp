FAIL-CLOSED PROMPT CHECK

Read this entire user-provided task before doing anything.

Before using tools, reading files, running commands, modifying files, or taking
any repository action, locate the unique completion token near the end of this
user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a
line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment
metadata, or other system-generated context may appear after the user's token
and do not invalidate it.

This check is intended to detect an incomplete or truncated USER task, not
appended agent context.

If exactly one complete TEMPEST_ token cannot be found, stop before any project
or repository action and explain that the supplied user task appears incomplete.


TASK

Perform a tightly bounded, read-only investigation of the original arcade
Tempest Blaster-shot shape, color, scaling, and rendering behavior.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected accepted baseline:

4ccb3cfb6cf3a1e72ac47bcfc79454ce257cb9de
Close out accepted Level 5 no-direct-Flipper correction

Confirm the actual Git state before beginning.

Do not modify production code, tests, durable documentation, configuration,
project files, screenshots, recordings, or Git state.

Do not stage, commit, push, clean, rename, move, or delete anything.


USER-OBSERVED PROBLEM

The player Blaster shots in the current C# project do not visually match the
original arcade game.

Neal wants to determine the arcade shot's actual:

- vector shape;
- vector picture/program;
- color or color sequence;
- scale;
- apparent size at different tube depths;
- orientation;
- rendering cadence;
- and any animation or flicker behavior.

The purpose of this task is to establish the ROM behavior and compare it with
the current C# rendering before implementing any change.


LOCAL ROM AUTHORITY

Use the project-local ROM-derived annotated disassembly:

Reference/arcade/Tempest Rom Source code.md

This local source has been used repeatedly in previous Tempest milestones.

Do not search the internet for another Tempest disassembly.

Do not use Mark Incitti's implementation, another port, or a generic projectile
design to establish the shot shape or color when the local ROM material answers
the question.

Atari ROM data and control flow take priority over secondary references.


VISUAL REFERENCE FRAMES

Neal placed approximately 40 arcade/MAME screenshots at:

Videos/FramesBlasterShots/

Inspect every readable image in that directory.

The frames were captured to show Blaster shots as they travel down the tube.

These images are visual evidence supporting the ROM investigation. They must not
replace ROM analysis.

Determine the filenames and ordering available in the directory. Do not assume
that filename-number differences correspond exactly to arcade simulation ticks.

MAME/video capture cadence may repeat a visual state or skip an arcade update.
Do not infer exact shot speed, lifetime, update interval, or color cadence solely
by counting screenshots.


PRELIMINARY VISUAL CAUTIONS

The supplied examples appear to contain several kinds of small graphics,
including:

- player shots aligned along a lane;
- enemies or enemy projectiles;
- accepted enemy-destruction effects;
- the Blaster;
- tube geometry and HUD.

Do not classify every colored mark in a lane as a Blaster shot.

In particular, bright white radial/starburst graphics may be the separately
implemented and accepted type-0 enemy-destruction effect. Do not include such
graphics in the Blaster-shot shape unless ROM control flow or frame sequencing
establishes that they are part of the shot.

Use temporal continuity, lane alignment, ROM behavior, and collision outcomes
to distinguish:

1. player Blaster shots;
2. enemy shots;
3. enemy shapes;
4. enemy-destruction effects;
5. visual overlap between those objects.


EVIDENCE CLASSIFICATION

Clearly classify conclusions as appropriate:

- ROM-LISTING-BACKED
- ARCADE/MAME-OBSERVED
- EXISTING C# IMPLEMENTATION
- PREVIOUSLY ACCEPTED PROJECT BEHAVIOR
- PROJECT TRANSLATION CANDIDATE
- INTERPRETATION / HYPOTHESIS
- UNKNOWN / UNRESOLVED

Do not describe an inferred RGB color, vector path, scale rule, duration, or
animation as ROM-backed unless the local ROM listing establishes it.


QUESTION 1: CURRENT C# SHOT MODEL

Trace the complete current C# player-shot lifecycle:

- input acceptance;
- shot-slot allocation;
- firing cadence;
- lane assignment;
- starting depth;
- depth movement;
- collision;
- removal;
- rendering;
- color selection;
- size/scale selection;
- projection;
- behavior during tube exit or other presentation states;
- recording/playback determinism.

Identify all relevant production code and tests, including as applicable:

- Shot.cs;
- TempestGame.cs;
- VectorRenderer.cs;
- PlayerShotRenderingTests.cs;
- simulation/collision tests;
- recording/playback-related tests;
- durable development notes and behavior specifications.

Determine exactly how the current C# shot is drawn.

Report:

- primitive type;
- number of points/segments;
- dimensions;
- base radius or equivalent size;
- color;
- perspective scaling;
- orientation;
- whether rendering changes with depth;
- whether rendering is animated;
- whether each visible mark is one shot or several overlapping shot primitives.

The project previously accepted a player-shot base radius of 4.5. Treat that as
PREVIOUSLY ACCEPTED PROJECT BEHAVIOR, not proof of arcade correctness.

Do not modify it in this investigation.


QUESTION 2: ROM SHOT STATE AND LIFECYCLE

Using the local ROM listing, trace player-shot creation, storage, updating,
collision, removal, and drawing.

Identify concrete routines, addresses, tables, constants, RAM records, and
vector-ROM entries.

Determine where possible:

- the number of simultaneous player-shot slots;
- how a slot is marked active/inactive;
- shot lane/segment storage;
- shot depth/along-position storage;
- starting depth;
- movement/update rule;
- removal boundary;
- collision interaction;
- whether the shot has an animation state;
- whether shot appearance depends on age;
- whether shot appearance depends on depth;
- whether shot appearance depends on firing cadence or slot number;
- whether multiple rapid shots visually form the apparent dotted trail;
- whether shots use a static vector picture or generated geometry;
- whether the shape is drawn once or redrawn with multiple positions/components;
- whether the ROM varies color by depth, frame, slot, or state.

Trace the shot renderer all the way to its vector data rather than stopping at
a high-level draw call.


QUESTION 3: ROM VECTOR SHAPE

Locate the actual vector definition or generated-vector instructions for the
player Blaster shot.

Report:

- vector entry address or addresses;
- graphic-table pointer/index;
- exact vector instructions where readable;
- line moves and draws;
- relative geometry;
- color-state changes;
- intensity/scale commands;
- return/end behavior;
- any shared vector subroutines;
- any reuse by enemy projectiles or other objects.

Translate the vector instructions into a neutral geometric description without
implementing them.

Examples of appropriate descriptions:

- a single point/dot;
- two crossed line segments;
- a short axial line;
- a small multi-segment spark;
- several disconnected dots;
- a depth-scaled shared vector object.

Do not call the shot a circle, square, star, spark, or cluster unless the vector
program supports that description.


QUESTION 4: ROM COLOR

Identify the ROM color index or indices used by player shots.

Determine:

- whether the shot uses one color or changes colors;
- whether the selected color depends on shot depth or age;
- whether color changes occur inside the vector program;
- whether color is inherited from prior vector-generator state;
- whether color differs between player shots and enemy shots;
- whether any apparent multicolor shot in the screenshots is actually overlap
  among different objects.

Do not invent a literal RGB mapping from a ROM color index.

Search the current project for established translations of the same ROM color
index and document whether that mapping is already accepted or merely a project
choice.

If the screenshots provide clear visual color evidence, classify it separately
as ARCADE/MAME-OBSERVED.


QUESTION 5: SCALE, ORIENTATION, AND DEPTH

Determine how the arcade shot is placed and scaled in tube perspective.

Trace:

- shot lane endpoints or lane-center interpolation;
- along-position/depth projection;
- transform or Mathbox usage;
- vector scale selection;
- whether the shot grows toward the Blaster/rim;
- whether the shot rotates with lane orientation;
- whether the vector picture itself is oriented or transformed;
- whether the apparent shape changes because of perspective rather than
  animation.

Compare that behavior with the current C# projection and base-radius model.


QUESTION 6: VISUAL FRAME ANALYSIS

Inspect all screenshots in Videos/FramesBlasterShots/.

Create a frame-by-frame or grouped-phase account that records only directly
visible evidence.

For each useful frame or group, record:

- filename;
- visible player-shot candidates;
- lane;
- approximate depth ordering;
- visible color components;
- approximate shape;
- size relative to nearby lane width;
- whether an apparent mark persists unchanged across multiple captures;
- whether several shot candidates coexist;
- whether a target or explosion overlaps a shot;
- confidence that the object is a player shot.

Look for visual evidence answering:

- Is one arcade shot visually one dot, several dots, or several short vectors?
- Does the shot appear yellow, red, orange, blue, white, or multicolor?
- Do separate colors belong to separate simultaneous shots at different depths?
- Does apparent size increase near the rim?
- Does the shot rotate or remain screen-oriented?
- Does rapid firing create a dotted line because several independent shots are
  visible together?
- Is there a bright impact phase that belongs to collision/destruction rather
  than the shot?

Do not use screenshot antialiasing, bloom, phosphor persistence, or captured
pixel color alone as proof of the ROM vector definition.


QUESTION 7: ROM VERSUS C# DISCREPANCY

Produce a direct comparison of:

- shape;
- segment count;
- color;
- size;
- depth scaling;
- lane orientation;
- animation;
- simultaneous-shot presentation;
- collision/impact handoff;
- rendering cadence.

Identify which differences are:

- proven by ROM data;
- visually supported by MAME frames;
- caused by a known C# project translation;
- still unresolved.

Determine whether the previously accepted C# base radius of 4.5 should likely:

- remain but use different geometry;
- be replaced by ROM-derived scale behavior;
- be reinterpreted as a project-space conversion;
- or be revisited entirely.

Do not change the radius or make an implementation decision in this task.


SCOPE EXCLUSIONS

Do not investigate or modify:

- enemy projectile shape except where necessary to distinguish it from the
  player shot;
- enemy-destruction effects except to distinguish them visually;
- Blaster shape;
- firing cadence;
- shot-slot count behavior beyond documenting it;
- collision sizes;
- scoring;
- sound;
- enemy behavior;
- level supply or selection;
- level progression;
- Super Zapper;
- recording/playback format or controls;
- screenshot functionality;
- unrelated rendering;
- Levels 8 or 9.

If the ROM investigation reveals another issue, record it as deferred rather
than expanding the task.


NO IMPLEMENTATION

This prompt is investigation-only.

Do not change:

- Shot.cs;
- TempestGame.cs;
- VectorRenderer.cs;
- tests;
- documentation;
- configuration;
- project files;
- image files;
- recordings;
- Git state.

Do not add a diagnostic mode.

Do not launch the interactive game.

Do not stage, commit, or push.


VALIDATION AND REPOSITORY SAFETY

Read-only file inspection, image inspection, ROM tracing, and narrowly useful
read-only commands are allowed.

Do not run the full test suite or build merely as ritual for a no-change
investigation.

At completion verify:

- HEAD remains at the accepted baseline;
- no tracked or staged changes were created;
- the screenshots in Videos/FramesBlasterShots/ remain untouched;
- screenshot000.png through screenshot013.png remain untouched and untracked;
- UserData and recordings remain untouched.


REPORT

Preserve this complete substantive prompt under the ignored AI-Logs/Prompts
directory according to project policy.

Create a paired date-prefixed report under ignored AI-Logs/Reports.

The report must include:

1. Starting Git state.
2. Files, tests, documentation, ROM regions, and screenshots inspected.
3. Current C# player-shot lifecycle and rendering.
4. ROM player-shot lifecycle.
5. ROM drawing routine and vector entry points.
6. Exact vector shape reconstruction.
7. ROM color-index behavior.
8. Scale, orientation, and perspective behavior.
9. Screenshot-by-screenshot or grouped visual findings.
10. Clear separation of shots, enemy shots, enemies, and explosions.
11. ROM-versus-C# comparison.
12. Proven discrepancies.
13. Unknown or unresolved details.
14. Smallest maintainable future implementation recommendation.
15. Automated tests recommended for that candidate.
16. Manual frame-by-frame acceptance plan using recording/playback.
17. Final Git state proving no tracked change.

The future implementation recommendation must remain bounded to player-shot
rendering unless ROM evidence proves that a small shared projection primitive
is necessary.

Prefer translating the actual vector program over approximating the screenshots
with a generic shape.


STOP POINT

Stop after investigation and recommendation.

Do not implement the shot-rendering correction.

At completion, provide Neal a concise summary answering:

- What shape does the ROM actually draw for one Blaster shot?
- What ROM color index or sequence does it use?
- Does color or shape change with depth or age?
- Is the dotted on-screen appearance one shot or several simultaneous shots?
- How does the current C# drawing differ?
- What is the smallest safe rendering correction to implement next?
- What remains uncertain pending manual comparison?

TEMPEST_BLASTER_SHOT_ROM_VISUAL_INVESTIGATION_20261108