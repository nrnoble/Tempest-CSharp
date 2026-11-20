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

Implement a tightly bounded, manually testable candidate for the common
ROM-derived type-0 destruction effect shown when a normal Blaster shot destroys
an ordinary enemy or an enemy projectile.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected accepted baseline:

4604297920f90c96731abd727ca3c73660300e88
Add manually accepted paused-playback screenshot capture

Confirm the actual Git state before modifying anything.

The previous read-only investigation report is:

AI-Logs/Reports/2026-11-07-enemy-destruction-rom-investigation-report.md

Read that report fully before designing or modifying the implementation.


LOCAL ROM REFERENCE

The project already contains the ROM-derived annotated Tempest disassembly:

Reference/arcade/Tempest Rom Source code.md

This local project file has been used several times in previous Tempest
milestones.

Use the project-local disassembly directly.

Do not search the internet for another Tempest disassembly, source listing,
reference implementation, explosion implementation, or generic particle
effect.

Do not substitute Mark Incitti's implementation or any external port for the
local ROM-derived evidence.

If a ROM detail must be rechecked, inspect the local disassembly and trace the
relevant addresses, tables, and vector data directly.


USER-OBSERVED PROBLEM

The current C# implementation immediately removes a destroyed enemy. The enemy
therefore appears to vanish when struck by a Blaster shot.

The original arcade game replaces the destroyed object with a brief animated
multicolor vector destruction effect at the object's final lane and depth.

Neal captured a MAME frame sequence demonstrating the effect, but those exact
MAME images were not included in the repository when the read-only
investigation ran.

Do not claim to have inspected those exact images unless they are now actually
available in the supplied task context or repository.

The absence of those exact images does not block this implementation candidate.
Use the ROM-derived program and vector data as the implementation authority.
Final visual judgment remains a manual acceptance step for Neal.


ROM-LISTING FINDINGS TO PRESERVE

The investigation traced these ROM paths in the local disassembly:

- pshot_hit at $a454 scans player shots and distinguishes enemy-projectile
  records from enemy records.
- An enemy projectile hit uses the $a36f handler.
- An ordinary enemy hit uses the $a38e handler.
- Both handlers eventually use the same type-0 destruction-effect allocator at
  $a3d4/$a3d6.
- The allocator contains eight effect records, indices 0 through 7.
- Each record captures:
  - lane/segment;
  - along-position/depth;
  - effect type;
  - animation frame/age.
- The effect remains at the captured hit position rather than continuing to
  move like the destroyed target.
- When all eight records are occupied, the allocator replaces the record with
  the greatest age.
- With equal ages, the ROM scan behavior deterministically selects the lowest
  record index.
- A new effect is not silently discarded when the pool is full.
- The common type-0 age sequence advances by three:
  0, 3, 6, 9, then deactivates when the next update reaches 12.
- draw_explosions at $b79a selects the hit-by-shot vector pictures from the ROM
  table using the stored effect state.
- The hit-by-shot table includes the vector entry-point sequence:
  $1b18, $1b14, $1b10, $1b0c, $1b08, $1b04, $1b0c, $1b14, $1b18.
- The shared vector body at $1b1a changes among ROM color indices 9, 11, and 10.
- The local listing establishes ROM color indices but does not, by itself,
  justify inventing RGB color names.
- Spike erosion uses a separate path and must not create this effect.
- Fuseball/Fuzzball destruction uses a different branch and effect types 2–4.
  Its exact graphic mapping remains unresolved and is not part of this
  implementation.

Recheck these findings against the local disassembly while implementing. Do not
blindly copy the report if the relevant local ROM lines reveal an error or
important qualification. Record any discrepancy before proceeding.


IMPLEMENTATION SCOPE

Implement the common type-0 normal-Blaster-hit destruction effect for:

1. ordinary Flippers;
2. Tankers;
3. Tanker-released cargo Flippers after materialization;
4. Spiker bodies, but not their Spikes;
5. Pulsars;
6. enemy projectiles/shots.

The ROM investigation found no separate type-0 program for Flippers, Tankers,
cargo Flippers, Spiker bodies, or Pulsars.

Enemy projectiles use a separate collision/removal handler but the same type-0
visual program and allocator.


EXPLICITLY EXCLUDED

Do not implement or change:

- Spike destruction or Spike erosion presentation;
- Fuseball/Fuzzball special destruction effects;
- Fuseball effect types 2–4;
- Super Zapper destruction presentation;
- Blaster/player death presentation;
- Game Over effects;
- scoring rules;
- score-popup behavior;
- audio selection or audio volume;
- enemy firing;
- enemy movement;
- Tanker cargo semantics;
- wave-ledger semantics;
- level geometry;
- level supply;
- level progression;
- recording-file format;
- playback-control mappings;
- screenshot-control mappings;
- packaging or installation-root behavior;
- Levels 8 or 9;
- unrelated cleanup or refactoring.

Do not introduce a universal generic particle emitter.

Do not broaden this into a general-purpose visual-effects framework unless an
extremely small shared seam is demonstrably necessary for this bounded effect.


GAMEPLAY-SEMANTIC CONSTRAINTS

The destruction presentation must be visual-only in the accepted C# project.

Preserve all existing C# behavior for:

- collision results;
- target removal;
- player-shot consumption;
- scoring;
- audio;
- enemy counts;
- ledger identity;
- Tanker parent resolution;
- Tanker cargo activation;
- projectile counts;
- level completion;
- death/re-entry;
- exit timing;
- random-number consumption.

The ROM includes active destruction effects in some arcade transition gates.
Do not adopt that transition dependency in this candidate.

The current accepted C# level-completion tick must remain unchanged.

The new effect must not:

- inherit from Enemy solely to reuse enemy infrastructure;
- enter developmentCollisionOrder;
- enter an enemy collection;
- enter an enemy-projectile collection;
- satisfy or block level completion;
- participate in collision;
- award score;
- trigger audio;
- own ledger identity;
- alter Tanker cargo ownership;
- consume the shared gameplay RNG stream.

Create the effect snapshot only after a hit has been confirmed and at the
narrow seam before the struck target is removed or Tanker cargo is released.

Capture the target's final lane and depth before mutation.


EFFECT STATE AND LIFECYCLE

Create a small, maintainable transient destruction-effect state owned by the
game simulation.

Use an explicit fixed capacity of eight active records, preserving the
ROM-derived pool semantics.

Each active common type-0 record should contain only the state required to
reproduce the ROM-derived presentation, including as appropriate:

- active/inactive state;
- captured lane;
- captured depth/along-position;
- type-0 program identity;
- age/frame state;
- deterministic slot identity or index if required.

Preserve the ROM-derived type-0 age progression:

0 → 3 → 6 → 9 → inactive on the next update reaching 12.

Carefully confirm how the existing C# simulation update order should represent
the ROM sequence in which hit processing and effect advancement occur.

Avoid accidentally skipping the initial visible state.

Document any unavoidable project translation between ROM update order and C#
render/update order.

When an effect is created with a free slot, use deterministic slot selection
consistent with the ROM.

When all eight slots are occupied:

- replace the record with the greatest age;
- preserve the ROM-derived deterministic equal-age tie behavior;
- do not discard the new effect;
- do not allocate an unlimited ninth record.

The effect must remain fixed at the captured lane and depth throughout its
lifetime.


VECTOR PROGRAM TRANSLATION

Reconstruct the common type-0 vector presentation from the project-local ROM
disassembly and vector data.

Do not draw a generic radial burst merely because the visual result seems
plausible.

Trace and translate the actual hit-by-shot vector sequence and the shared
vector body used by the ROM.

Map the ROM's selected picture states to the four reachable C# effect ages only
after reproducing the ROM's actual indexing expression and update/render order.

Do not assume that all nine table entries are displayed.

Preserve the ROM program's outward and return sequence where the reachable
indices establish one.

Preserve the ROM-derived multicolor structure.

Do not invent named arcade RGB colors from ROM indices 9, 11, and 10.

Use the project's existing accepted vector color vocabulary only after tracing
how equivalent ROM color indices are already interpreted in the project or
after establishing a narrowly justified project translation.

Clearly comment and document the distinction among:

- ROM-derived picture sequence;
- ROM-derived color-index changes;
- existing project color mapping;
- any necessary C# rendering translation.

Keep comments useful and maintainable. Do not add large speculative comment
blocks.


RENDERING

Integrate the effect through the existing vector-rendering conventions.

The renderer should receive simulation-owned effect state and draw it at the
captured lane/depth.

The renderer must not own or advance the effect lifetime.

Do not use wall-clock animation.

Do not introduce nondeterministic rendering behavior.

Ensure the effect can be drawn during the relevant exit presentation so that
the final-enemy hit does not visually disappear merely because level completion
began.

Do not delay C# level completion to accomplish this.

Do not change unrelated enemy, tube, HUD, transition, player-shot, or player
rendering.


DETERMINISTIC PLAYBACK REQUIREMENTS

The implementation must naturally remain compatible with the accepted gameplay
recording/playback and inspection tools.

The effect must:

- advance through normal fixed simulation ticks;
- freeze when visible playback is paused;
- advance when playback steps forward;
- reconstruct correctly when playback scrubs backward and replays forward;
- produce equal state at equal playback boundaries;
- run correctly during headless playback;
- appear in paused screenshots;
- require no gameplay-recording schema change.

Do not store visual-effect state in the input recording.

Do not add special cases to screenshot capture merely for this effect.


REQUIRED AUTOMATED TESTS

Add focused tests sufficient to establish at least the following:

1. Ordinary Flipper
   - A confirmed Blaster hit creates exactly one type-0 effect.
   - The effect captures the Flipper's pre-removal lane and depth.
   - Existing removal, score, audio, enemy count, and completion behavior remain
     unchanged.

2. Enemy projectile
   - A confirmed Blaster hit creates the same type-0 visual program.
   - Existing projectile removal and 25-point behavior remain unchanged.
   - Do not introduce an enemy kill, enemy-ledger update, or new destruction
     sound if current C# behavior does not do so.

3. Tanker
   - The effect captures the Tanker's position, not a cargo child's position.
   - Existing Tanker resolution and cargo activation/release remain unchanged.
   - No cargo child is struck by the same killing shot.

4. Cargo Flipper, Spiker body, and Pulsar
   - Each supported normal Blaster hit creates the common type-0 effect.
   - Existing per-target score, sound, identity, and removal behavior remains
     unchanged.

5. Exclusions
   - Spike erosion does not create a type-0 enemy-destruction effect.
   - Fuseball destruction does not incorrectly receive the common type-0 effect
     in this candidate.
   - Super Zapper and player death do not accidentally create these records.

6. Lifecycle
   - Effect age progresses through the intended states.
   - The initial visible state is not skipped.
   - The effect deactivates at the correct update.
   - Captured lane and depth remain unchanged.

7. Capacity and replacement
   - The pool contains no more than eight records.
   - A ninth effect replaces the oldest active record.
   - Equal-age replacement follows the ROM-derived deterministic tie rule.
   - The new effect is never silently discarded merely because the pool is
     full.

8. Isolation
   - Effects do not participate in collision.
   - Effects do not enter enemy or projectile accounting.
   - Effects do not block or satisfy level completion.
   - The last-enemy completion tick remains unchanged.

9. Rendering
   - Reachable type-0 ages select the intended ROM-derived vector program
     states.
   - Render placement uses the captured lane/depth.
   - Multicolor vector-program transitions are represented as justified by the
     local ROM listing and project color mapping.
   - Rendering does not mutate simulation state.

10. Playback
    - Equal playback boundaries produce equal effect state.
    - Headless playback advances the effect deterministically.
    - Pause holds state.
    - Forward stepping advances state.
    - Reverse reconstruction restores prior state through replay.
    - The gameplay recording format remains unchanged.

Prefer focused state/delegate tests over brittle pixel comparisons.

Do not manufacture synthetic ROM facts merely to satisfy a test.


VALIDATION

After implementation:

1. Run the narrowest directly affected tests during development.
2. Run an expanded focused filter covering:
   - the new destruction-effect tests;
   - player-shot collision;
   - Flippers;
   - Tankers and cargo;
   - Spikers and Spikes;
   - Pulsars;
   - enemy projectiles;
   - scoring;
   - audio behavior where relevant;
   - wave ledger;
   - level completion;
   - rendering;
   - gameplay recording/playback;
   - visible playback and scrubbing.
3. Run the full solution test suite.
4. Run the full solution build.
5. Run git diff --check.
6. Inspect the final diff carefully.
7. Verify generated screenshots, recordings, UserData, AI-Logs, and the original
   generic screenshot files are not staged or modified.

Do not launch the interactive game. Neal will perform manual gameplay and visual
acceptance.


DOCUMENTATION

Update durable documentation only to describe the implementation candidate and
its evidence accurately.

Update as appropriate:

- TEMPEST-DEVELOPMENT-NOTES.md;
- TEMPEST-BEHAVIOR-SPEC.md;
- README.md only if the user-facing gameplay/playback description genuinely
  requires it.

Preserve evidence classification.

Use descriptions such as:

- ROM-LISTING-BACKED;
- EXISTING PROJECT BEHAVIOR;
- PROJECT TRANSLATION;
- MANUAL ACCEPTANCE PENDING;
- UNKNOWN / UNRESOLVED.

Do not describe the candidate as arcade-accepted before Neal manually tests it.

Record that:

- common type-0 destruction is implemented for the supported targets;
- Spikes are excluded;
- Fuseball/Fuzzball special destruction remains deferred;
- exact visual fidelity remains pending manual MAME/C# comparison;
- C# completion timing intentionally remains unaffected by effect lifetime.


GIT AND STOP POINT

Do not stage, commit, or push.

Leave the complete candidate unstaged for Neal's manual acceptance.

Do not delete, move, rename, or stage:

- screenshot000.png through screenshot013.png;
- UserData screenshots;
- gameplay recordings;
- AI-Logs content;
- unrelated local files.

At the end, report:

- starting Git state;
- files changed;
- exact ROM paths and vector tables translated;
- effect architecture and lifecycle;
- supported target categories;
- excluded categories;
- focused test results;
- full test results;
- build result;
- git diff --check result;
- final Git state;
- manual launch command;
- concise manual acceptance checklist;
- unresolved issues.

Preserve the complete substantive prompt under the ignored AI-Logs/Prompts
directory and create a paired date-prefixed report under ignored
AI-Logs/Reports according to project policy.

MANUAL ACCEPTANCE CHECKLIST TO PREPARE

Provide Neal a bounded checklist that includes:

- kill an ordinary Flipper and confirm it is replaced by a brief multicolor
  vector destruction effect at its final lane/depth;
- compare the visible phase progression with MAME;
- confirm the enemy does not simply disappear;
- destroy an enemy projectile and check for the same visual program;
- destroy a Tanker and verify the effect remains at the parent position while
  cargo behavior remains normal;
- destroy a Spiker body and confirm the Spike remains unaffected;
- verify Pulsar destruction uses the common effect;
- confirm shooting a Spike does not create the enemy effect;
- confirm Fuseball receives no newly guessed type-0 implementation;
- kill the final enemy and verify the effect remains visibly present without
  delaying progression;
- use pause, forward step, reverse scrub, and screenshot capture to inspect the
  effect;
- check normal gameplay, scoring, sound, death/re-entry, and level progression
  for regressions.

Stop after automated validation and presentation of the manual test command.

TEMPEST_COMMON_TYPE0_DESTRUCTION_IMPLEMENTATION_20261107
