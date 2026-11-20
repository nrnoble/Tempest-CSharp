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

Perform a tightly bounded, read-only ROM and code investigation for the next
Tempest milestone: arcade-faithful destruction effects when a Blaster shot
destroys an enemy or enemy shot.

Repository:

C:\Users\nrnob\source\repos\Tempest

CURRENT ACCEPTED BASELINE

The accepted paused-playback screenshot milestone is commit:

4604297920f90c96731abd727ca3c73660300e88

Confirm the current Git state and baseline before beginning.

Do not modify production code, tests, documentation, configuration, project
files, or Git state during this investigation.

Do not stage, commit, push, clean, rename, or delete anything.


PROBLEM

In the current C# Tempest implementation, an enemy struck and destroyed by a
Blaster shot simply disappears.

The original arcade game replaces the destroyed target with a visible animated
destruction effect.

Neal supplied a frame-by-frame MAME screenshot sequence showing an example from
early gameplay. The sequence visually includes:

- a bright white radial/starburst-like component near the target's final
  position;
- a smaller yellow/orange speckled or fragment-like component that remains
  visible across subsequent captured frames;
- enemy geometry being replaced by the destruction presentation rather than
  disappearing without visual feedback.

Treat these descriptions as preliminary observations only. Inspect the supplied
images directly if they are available to the agent.

Do not derive exact arcade timing merely by counting the MAME PNG files.
Consecutive MAME captures may contain repeated visual states because the arcade
game update cadence and MAME screenshot cadence may differ.


REQUIRED SCOPE

This investigation covers destruction caused by a normal Blaster shot hitting:

1. an enemy;
2. an enemy projectile/shot.

This investigation explicitly excludes:

- Spikes and Spike erosion;
- Blaster/player death animation;
- Game Over effects;
- Super Zapper destruction unless the ROM investigation shows that it directly
  shares a routine essential to understanding normal shot destruction;
- sound-volume tuning;
- score-system redesign;
- unrelated renderer changes;
- Levels 8 and 9 implementation;
- implementation of the destruction effect itself.

The user currently expects Spikes not to use this effect.

Do not assume all enemy types use identical graphics, colors, timing, or
destruction programs. In particular, investigate whether Pulsars or other
higher-level enemies use different destruction behavior.


AUTHORITY ORDER

Use the following evidence order:

1. Atari Tempest ROM behavior and data available in the repository.
2. Direct arcade/MAME visual evidence supplied by Neal.
3. Existing accepted project behavior and durable specifications.
4. Secondary reference implementations or public disassemblies only as
   cross-checks.

Do not let Mark Incitti's implementation, another port, a generic particle
system, or an internet description override explicit ROM evidence or Neal's
direct arcade evidence.

Clearly classify every conclusion as one of:

- ROM-BACKED;
- ARCADE/MAME-OBSERVED;
- EXISTING PROJECT BEHAVIOR;
- SECONDARY REFERENCE;
- PROJECT TRANSLATION CANDIDATE;
- UNKNOWN / UNRESOLVED.

Do not present inferred names, durations, vector shapes, colors, random behavior,
or state transitions as ROM-backed unless the relevant ROM path actually
establishes them.


INVESTIGATION QUESTIONS

A. Current C# destruction path

Trace the complete current production path for:

- a player shot colliding with an ordinary enemy;
- a player shot colliding with an enemy shot;
- removal or terminal resolution of the struck object;
- score award;
- enemy destruction sound;
- wave-ledger or identity resolution;
- Tanker cargo release, where relevant;
- rendering after destruction.

Identify the narrowest seam where a visual destruction state could later be
created without changing score, sound, ledger identity, cargo release,
completion, or collision semantics.

Determine whether the current code has any reusable visual-effect lifecycle or
rendering abstraction, but do not redesign the architecture in this task.


B. ROM control flow

Locate and trace the relevant ROM routines and tables for shot collision and
object destruction.

Determine, where the ROM permits:

- which routine dispatches destruction after a player shot hits an object;
- whether enemy shots use the same or a different effect path;
- whether the original enemy object changes state/type or whether a separate
  transient object/effect record is allocated;
- destruction lifetime and update cadence;
- vector picture identifiers or vector-ROM addresses;
- color changes;
- position/depth behavior;
- size or scale behavior;
- animation-frame progression;
- deterministic versus random or POKEY-derived elements;
- whether multiple effect components coexist;
- whether effect state affects collision, scoring, wave completion, enemy
  counts, shot slots, or gameplay;
- what occurs if no transient-effect slot is available;
- what happens when several objects are destroyed close together.

Trace concrete branches, tables, constants, and addresses. Include enough local
context for another developer to reproduce the reasoning.


C. Per-object behavior matrix

Investigate at least these target categories where present in the implemented
Levels 1-7:

- ordinary Flipper;
- Tanker;
- Tanker-released cargo Flipper;
- Spiker body, excluding the Spike itself;
- Fuseball;
- Pulsar;
- enemy projectile/shot.

For every category, report:

- whether a normal Blaster shot can destroy it;
- routine/path used;
- effect identity or vector picture sequence;
- color;
- lifetime;
- position/depth behavior;
- distinctive behavior;
- confidence/evidence classification.

Do not collapse categories merely because they appear similar in one MAME
capture.


D. Visual evidence

Analyze the supplied MAME frame sequence carefully.

Record only directly visible findings, including:

- approximate location relative to the destroyed target;
- visible colors;
- major shape phases;
- repeated identical visual states;
- apparent motion or persistence;
- whether the original enemy shape remains visible during any captured phase;
- whether more than one visual component appears to coexist.

Do not infer the target enemy type unless it can be established from the images
or accompanying evidence.

Do not use image counting alone to declare an exact ROM tick duration.


E. Implementation planning

Without changing source code, propose the smallest maintainable implementation
candidate that preserves all accepted gameplay behavior.

The proposal should address:

- a transient destruction-effect state or entity;
- capturing lane, depth, target category, and effect program at destruction;
- simulation update ownership;
- renderer ownership;
- deterministic playback and recording compatibility;
- pause, forward scrub, reverse scrub, and screenshot compatibility;
- headless execution;
- collision and completion isolation;
- simultaneous effects;
- tests required before manual acceptance.

Prefer explicit ROM-backed per-type effect programs or data where available.
Do not propose one universal generic explosion if the ROM establishes multiple
programs.


VALIDATION AND REPOSITORY SAFETY

This is investigation-only.

- Do not implement.
- Do not edit tests.
- Do not update durable documentation yet.
- Do not run the interactive game.
- Do not commit or push.
- Do not alter generated screenshots or recordings.
- Do not remove the preserved generic screenshot files.

Read-only code-search commands, ROM-analysis tools, and narrowly relevant
existing tests may be run if useful.

If running tests would not add evidence to this read-only investigation, do not
run them merely as ritual.

At the end, verify that tracked Git state is unchanged from the starting state.


REPORT

Preserve the complete substantive prompt under the ignored AI-Logs/Prompts
directory according to project policy.

Create a paired ignored report under AI-Logs/Reports with a descriptive
date-prefixed filename.

The report must include:

1. starting Git state;
2. source files and ROM materials inspected;
3. current C# collision/destruction flow;
4. ROM routines, addresses, tables, constants, and control flow;
5. per-object destruction behavior matrix;
6. MAME screenshot observations;
7. known facts separated from interpretations;
8. unresolved questions;
9. recommended bounded implementation shape;
10. proposed automated tests;
11. proposed manual acceptance scenarios;
12. final Git state confirming no tracked changes.

STOP POINT

Stop after the investigation and implementation recommendation.

Do not implement the effect in this prompt.

At completion, provide a concise summary identifying:

- whether the ROM establishes one or multiple destruction programs;
- whether enemy shots use the same path;
- what is known about Pulsar and other special-case destruction;
- the recommended implementation boundary;
- unresolved issues requiring further evidence.

TEMPEST_ENEMY_DESTRUCTION_ROM_INVESTIGATION_20261107