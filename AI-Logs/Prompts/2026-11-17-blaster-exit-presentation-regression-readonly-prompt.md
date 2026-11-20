READ THIS ENTIRE USER-PROVIDED TASK BEFORE TAKING ANY ACTION.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, reading project files, running commands, inspecting images,
or taking any repository action:

1. Locate the unique completion token near the end of this user-provided task.
2. Verify that exactly one complete token beginning with "TEMPEST_" appears
   on a line by itself in the user-provided task.
3. VS Code/agent-generated reminders, tagged-file information, attachment
   metadata, or other system-generated context may appear after the token and
   do not invalidate it.
4. This check detects an incomplete/truncated user task, not appended agent
   context.
5. If exactly one complete TEMPEST_ token cannot be found, STOP before project
   or repository action and explain why.

TASK

Perform a bounded READ-ONLY regression investigation of the Blaster
presentation during tube exit under --atari-presentation.

The current regression affects tube exit across the presentation milestone,
not only one Level 1 geometry.

During ordinary Playing the Blaster now renders correctly outside the rim.

During tube exit, however, the Blaster becomes grossly stretched/elongated
across the tube during intermediate exit frames before becoming recognizable
again.

This did not occur before the current whole-game Atari-presentation milestone.

Determine the exact cause before implementing a correction.

Do not modify source, tests, presentation, gameplay, or Git state.

REGRESSION STATUS

The Blaster ordinary-rim orientation fix has already been manually accepted,
closed, committed, and pushed.

Controlling closeout:

AI-Logs/Reports/2026-11-17-blaster-outward-orientation-closeout-report.md

Commit:

34eedf0cf996cfe660494e34bdff2a1499696068

That correction explicitly applies only to ordinary Atari-presentation Playing.

Approach, transition, death, exit, and legacy presentation were deliberately
left outside that correction.

Do not reopen the accepted ordinary-rim orientation milestone.

The exit problem existed separately and is the subject of this investigation.

MANUAL REGRESSION EVIDENCE

Neal reports:

- Blaster tube exit is visually wrong.
- The defect affects all tubes in this presentation milestone.
- This is a regression that did not occur before the milestone.
- The tube exit/zoom sequence itself continues, but the Blaster artwork is
  severely malformed during part of the exit.
- The Blaster can appear recognizable before the malformed interval, become
  enormously stretched during intermediate frames, contract again, and later
  return to a recognizable compact shape.
- The defect concerns Blaster presentation, not a reported exit-gameplay
  mechanics failure.

Treat these as MANUAL REGRESSION OBSERVATIONS.

AUTHORIZED IMAGE EVIDENCE

You MAY inspect exactly these four supplied frames:

- playback-level-1-boundary-000607.png
- playback-level-1-boundary-000694.png
- playback-level-1-boundary-000647.png
- playback-level-1-boundary-000623.png

Locate only these exact filenames if necessary.

Do not enumerate, open, inspect, analyze, or compare any other screenshot,
image, frame, video, contact sheet, or visual asset.

Use these frames to characterize the visible defect and sequence.

Do not derive implementation constants from pixels.

Do not fit Blaster geometry from screenshots.

The technical cause must be established from the renderer and deterministic
exit state.

PRIMARY QUESTION

Why does the Blaster's local rendered geometry become enormously elongated
during intermediate tube-exit frames under --atari-presentation?

Determine whether the regression is caused by:

- anchor position;
- local tangent;
- local perpendicular/normal;
- local scale;
- projected edge length;
- projected rim geometry;
- depth denominator;
- ApproachDepthOffset;
- ApproachVerticalOffset;
- exit/reverse-zoom state;
- presentation adapter mapping;
- portrait perspective;
- playfield camera;
- duplicated application of scale/zoom;
- mixing model-space and already-projected coordinates;
- using two endpoints evaluated at inconsistent depths;
- exit interpolation;
- or another evidenced transformation error.

Do not assume any one of these in advance.

VISUAL CHARACTERIZATION

From the four authorized frames, objectively describe:

- Blaster location;
- approximate orientation;
- whether the anchor remains plausible;
- whether the tube itself remains coherent;
- whether Blaster line segments lengthen uniformly or non-uniformly;
- whether the artwork appears to stretch primarily along the edge tangent,
  radial normal, or both;
- whether the distortion develops continuously or appears discontinuously;
- and whether contraction/recovery is visible later in the supplied sequence.

Do not infer timing/order solely from filenames if the code/playback evidence
establishes another order.

FRAME NUMBERS ARE IDENTIFIERS

Treat 607, 694, 647, and 623 primarily as frame/boundary identifiers until
their actual playback order/state is confirmed.

Do not assume numeric filename ordering is chronological without verifying the
capture/playback semantics.

ESTABLISH THE LAST-KNOWN-GOOD PATH

This is a REGRESSION investigation.

Identify the last known presentation path before the whole-game
Atari-presentation work where Blaster tube exit rendered normally.

Use:

- Git history;
- durable development notes/spec;
- AI-Logs reports;
- current source;
- historical current-source architecture descriptions;
- relevant existing tests.

Do not reset, checkout, restore, stash, or modify the working tree to inspect
older code.

Use read-only Git history commands where helpful.

Determine the narrowest regression boundary where Blaster exit rendering
changed.

Do not assume the recently closed ordinary-rim orientation fix caused it; that
report explicitly says exit behavior remained outside its correction.

CURRENT EXIT STATE MACHINE

Trace the actual Level 1 exit sequence and the shared production exit path used
by other supported tubes.

Identify relevant states and variables, including:

- wave completion;
- exit delay;
- exit start;
- Blaster exit progression;
- tube zoom/reverse zoom;
- transition-to-Approach boundary;
- Level.Position;
- Level.Depth;
- player edge/lane state;
- player exit depth;
- any ExitProgress;
- ApproachDepthOffset;
- ApproachVerticalOffset;
- current presentation mode;
- current portrait camera state;
- transition starfield state where relevant.

Do not broaden into unrelated between-level visual fidelity.

BLASTER RENDER PATH

Trace the Blaster rendering path separately for:

A. ordinary Playing;

B. tube exit;

C. final transition/Approach handoff.

For each state identify:

- source geometry used for rim edge;
- projected endpoint A;
- projected endpoint B;
- projection depth of each;
- edge tangent;
- tangent normalization;
- perpendicular/normal;
- anchor;
- Blaster offset from edge;
- claw/body dimensions;
- local Blaster scale;
- camera scale;
- presentation offset;
- portrait perspective;
- final host coordinates.

Identify every conditional branch that differs between Playing and exit.

PLAYING CONTROL

Ordinary Atari-presentation Playing is now manually accepted for Blaster rim
orientation.

Use that path as a control.

Do not change it.

Compare the same lane/edge's Playing basis with its exit basis at representative
exit depths.

Determine where the exit basis mathematically departs from a coherent compact
Blaster.

TUBE CONTROL

Determine whether tube rails/rims themselves remain mathematically coherent
during the same exit frames.

If the tube scales/zooms correctly while only Blaster artwork explodes, isolate
the transformation unique to Blaster local geometry.

If the same underlying projected edge also becomes enormous, determine why the
tube rendering remains visually coherent while Blaster artwork uses that edge
differently.

Do not alter tube exit geometry in this investigation.

DETERMINISTIC REPRODUCTION

Use existing deterministic gameplay/playback infrastructure or existing exit
tests to reproduce the Level 1 exit sequence noninteractively.

Do not launch the interactive game.

Prefer the existing Level 1 recording/playback associated with the supplied
frames if its exact source can be identified without searching arbitrary
recordings.

Do not inspect TEMP/myNotes.md.

Do not create new gameplay recordings.

If an existing deterministic test can drive production Level 1 to exit more
cleanly, use that instead.

Track the exit state over bounded ticks covering:

- last normal Playing frames;
- exit onset;
- early exit;
- distortion onset;
- maximum/near-maximum distortion;
- contraction;
- final recognizable Blaster before handoff;
- transition/Approach boundary.

NUMERICAL TRAJECTORY

For representative exit ticks record:

- tick/state identifier;
- gameplay lane/edge;
- exit depth/progress;
- ApproachDepthOffset if active;
- projected edge A/B;
- edge length;
- normalized tangent;
- selected normal;
- Blaster anchor;
- Blaster local scale;
- six artwork endpoints or their bounding envelope;
- tube near-rim projected size;
- camera zoom;
- final Blaster host-space bounds.

Calculate:

- Blaster width/height or maximum local radius;
- ratio of Blaster size to local projected rim edge;
- ratio of Blaster size to tube envelope;
- change per tick.

The expected correct presentation should remain a compact Blaster whose local
size evolves coherently with exit perspective rather than spanning large
fractions of the tube.

Do not choose a target size from screenshots.

REGRESSION DELTA

Compare the current exit equations/code with the last-known-good exit path.

Enumerate only changes capable of affecting Blaster presentation, such as:

- new AtariPresentationAdapter dispatch;
- model-frame scale;
- portrait presentation;
- projection/perspective changes;
- camera changes;
- exit-local scaling;
- player-edge projection;
- transition changes.

Identify the first changed operation where current output diverges materially
from the last-known-good compact Blaster behavior.

Do not merely say "Atari presentation changed."

Name the exact field/function/formula and explain the mathematical consequence.

ALL-TUBE SCOPE

Neal reports that the defect affects all tubes in the presentation milestone.

Test whether the root cause is shared.

Use bounded representative coverage including:

- a simple closed tube;
- an open tube;
- a nontrivial enclosed shape;
- accepted Level 8;
- accepted Level 9;
- accepted diagnostic Level 10 where ordinary exit semantics are applicable.

Do not manually retune each level.

The eventual correction should be shared if the shared root cause is proven.

Do not modify or investigate unresolved Level 7-specific gameplay/presentation
architecture beyond establishing whether the shared exit path also reaches it.

SIMULATION PROTECTION

Determine whether underlying exit simulation remains unchanged.

Do not modify:

- wave completion;
- exit timing;
- player lane;
- player exit progression;
- transition timing;
- stars;
- next-level selection;
- score;
- lives;
- RNG;
- recording/playback;
- gameplay geometry.

The expected future correction should be presentation-only unless evidence
proves otherwise.

RECORDING/PLAYBACK

Determine whether recorded playback reproduces the same malformed Blaster exit
while preserving deterministic simulation.

The supplied frames suggest playback was used, but establish the current
source/path rather than assuming.

Do not change recording formats.

Do not change playback inputs.

Do not add this defect to recording metadata.

PERSPECTIVE / SCALE SAFETY

Do not retune accepted:

- whole-game perspective;
- Level 8 placement;
- Level 9 placement;
- Level 10 placement;
- tube geometry;
- tube projection;
- gameplay entity registration.

Do not solve Blaster stretching by globally shrinking the Blaster during all
states.

The future fix must address the erroneous exit transformation specifically.

READ-ONLY REQUIREMENT

Do not modify:

- source;
- tests;
- configuration;
- project files;
- durable documentation;
- gameplay;
- presentation;
- recordings;
- accepted constants;
- Git state.

Do not add production diagnostics.

Temporary bounded calculations outside tracked project paths are permitted.

Do not stage, commit, push, reset, restore, clean, stash, rebase, or switch
branches.

AI-LOGS

Follow .github/copilot-instructions.md.

Save:

- this prompt under AI-Logs/Prompts/;
- the paired exit-regression investigation report under AI-Logs/Reports/.

These are the sole permitted file-writing exception.

Do not stage or commit AI-Logs.

REPOSITORY SAFETY

Record at start:

- branch;
- HEAD;
- upstream;
- ahead/behind;
- index;
- tracked worktree;
- relevant untracked state.

The Blaster-orientation closeout report identifies the committed baseline as:

34eedf0cf996cfe660494e34bdff2a1499696068

on master tracking origin/master at 0/0.

Verify rather than assume.

Preserve unrelated local/reference work.

EXPECTED REPORT

Near the beginning directly answer:

1. What exact operation causes the Blaster to stretch during exit?
2. Why is ordinary Playing unaffected?
3. Why does the distortion grow and later contract?
4. Is the defect shared across tube geometries?
5. Did gameplay/exit simulation remain correct?
6. What exact milestone/change introduced the regression?
7. Is there enough evidence for a bounded shared presentation correction?

The report must include:

- exact authorized frames inspected;
- objective visual sequence characterization;
- actual playback/state ordering of those frames if determinable;
- last-known-good path;
- regression boundary;
- current Playing Blaster equations/path;
- current exit Blaster equations/path;
- current tube-exit equations/path;
- deterministic exit trajectory table;
- projected edge lengths across exit;
- Blaster local scale across exit;
- Blaster host-space bounds across exit;
- tube bounds across exit;
- first divergent transformation;
- exact root cause;
- all-tube applicability;
- simulation/determinism evidence;
- recording/playback impact;
- smallest justified future correction direction;
- risks and edge cases;
- whether evidence is sufficient for an implementation prompt;
- confirmation no project or Git state changed except required AI-Logs;
- exact prompt/report paths.

Do not ask Neal questions during execution.

Do not pause for manual input.

Complete autonomously and stop after reporting the evidence.

TEMPEST_BLASTER_EXIT_PRESENTATION_REGRESSION_READONLY