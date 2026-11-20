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
4. This check detects an incomplete or truncated user task, not appended agent
   context.
5. If exactly one complete TEMPEST_ token cannot be found, STOP before project
   or repository action and explain why.

TASK

Perform a narrowly bounded READ-ONLY investigation of the correct ExitZoom
presentation pivot for OPEN Tempest tube geometries.

The previous ExitZoom focal-compensation candidate produced a mixed manual
result:

- Level 5 is FIXED.
- Levels 8, 9, and 10 are NOT fixed.
- Levels 8/9/10 still exhibit the same general exit-perspective/focal problem,
  but the candidate moved the apparent focal point to a different location.
- Neal observes that Levels 8/9/10 appear to share the same focal behavior.
- Ordinary gameplay and ordinary Playing presentation remain good.

This strongly suggests that the compensation mechanism can work, but the
current OPEN-TUBE pivot definition may be wrong.

Do not implement a correction.

Do not alter the current candidate.

Determine what geometric/presentation point should serve as the ExitZoom pivot
for open tubes before authorizing another implementation.

CURRENT CANDIDATE

Use this report as the controlling implementation handoff:

AI-Logs/Reports/2026-11-17-exitzoom-focal-compensation-implementation-report.md

The current candidate:

- latches one mapped presentation-space opening position at actual ExitZoom
  onset;
- recalculates the corresponding mapped opening under live K/ZOffset;
- applies latchedOpening - currentOpening as one shared logical presentation
  translation;
- applies compensation after the active presentation adapter and accepted
  per-level placement and before the shared host camera;
- compensates the tube and tube-space gameplay presentation together;
- leaves ordinary Playing untouched;
- leaves gameplay/exit simulation unchanged.

For CLOSED tubes, the candidate uses the mapped far-rim center.

For OPEN tubes, the candidate currently uses:

midpoint(mapped terminal vertex 0, mapped terminal vertex N-1)

That open-tube assumption is now specifically under investigation.

Do not reopen the Level 5 compensation that passed manual testing.

MANUAL EVIDENCE

Treat the following as authoritative MANUAL OBSERVATIONS:

- Level 5 ExitZoom focal behavior now looks correct.
- Level 8 is still wrong.
- Level 9 is still wrong.
- Level 10 is still wrong.
- Levels 8/9/10 exhibit the same type of exit-perspective problem.
- The focal-compensation candidate appears to have moved the focal location for
  those levels rather than fixing it.
- Other than the exit focal/perspective problem, the levels look good and play
  normally.
- Neal currently suspects either:
  A. open geometries require a different focal rule from enclosed geometries;
  or
  B. each tube has its own correct focal point.
- At present Levels 8/9/10 appear visually to share one common type/location of
  focal behavior.

Treat A/B as diagnostic hypotheses, not established facts.

PRIMARY QUESTION

For an open Tempest tube, what geometric or presentation-space point should
remain stationary while ExitZoom enlarges the tube?

Determine whether the correct pivot is:

1. midpoint of the two open terminal/deep endpoints;
2. center of the mapped far endpoint set;
3. center of the far polyline's bounding box/envelope;
4. arithmetic centroid of mapped far vertices;
5. a weighted center of mapped far geometry;
6. intersection/convergence point implied by the tube rails;
7. model/projection focal point;
8. presentation-adapter origin/reference point;
9. a point encoded or implied by ROM presentation metadata;
10. a level-independent open-tube focal point;
11. a shape/level-specific focal point;
12. another precisely evidenced point.

Do not assume any listed candidate is correct.

OPEN TUBE SEMANTICS

Explicitly determine what "far opening" means for an open Tempest tube.

Unlike a closed tube, an open tube does not have a closed far-rim polygon.

Trace:

- ordered source vertices;
- playable lanes;
- terminal vertices;
- far projected vertices;
- rail convergence;
- any omitted closing edge;
- how Atari/C# visually represents the deep end;
- whether there is a meaningful geometric center despite the open topology;
- whether terminal midpoint has any architectural significance beyond being an
  easy measurable point.

Do not treat terminal midpoint as authoritative merely because the previous
candidate used it.

LEVEL 8/9/10 GROUP

Analyze Levels 8, 9, and diagnostic Level 10 together first.

For each establish:

- source geometry;
- open topology;
- terminal endpoints;
- far mapped vertex set;
- adapter metadata;
- model-frame transform;
- accepted final placement;
- normal Playing projection center;
- current candidate terminal-midpoint pivot;
- effective mapper focal/reference point;
- tube envelope at near and far depth;
- ExitZoom trajectories under the current candidate.

Determine which structural/presentation properties are shared by all three.

Then determine whether those shared properties explain why the same pivot rule
fails on all three.

LEVEL 8

Preserve and inspect:

- recovered open shape-7 geometry;
- 16 vertices / 15 lanes;
- reflected model-frame mapping;
- Level 8 ROM metadata;
- accepted +212.28139 Playing translation.

Do not change any value.

Calculate all reasonable open-pivot candidates at normal Playing and bounded
ExitZoom samples.

LEVEL 9

Preserve and inspect:

- recovered open shape-13 geometry;
- 16 vertices / 15 lanes;
- reflected model-frame mapping;
- Level 9 ROM metadata;
- accepted +180.96825 Playing translation.

Calculate the same candidate pivots as Level 8.

Determine whether Level 8 and Level 9 produce a shared presentation-space pivot
despite having different tube geometry and metadata.

LEVEL 10

Preserve:

- current diagnostic geometry;
- ROM-metadata adapter route;
- accepted +50 placement;
- diagnostic-only status.

Calculate the same candidate pivots.

Determine whether Level 10 supports or contradicts a common open-tube focal
rule.

Do not change Level 10 or require new manual acceptance.

LEVEL 5 CONTROL

Use manually fixed Level 5 as a successful compensation control.

Determine why:

mapped closed far-rim center

is a meaningful and successful ExitZoom pivot for Level 5.

Compare the stable Level 5 pivot's geometric role with candidate open-tube
pivots.

The goal is to identify the open-tube analogue of the successful closed-tube
far-rim center, not to redesign Level 5.

LEVELS 1-4 CONTROL

Use Levels 1-4 only as needed to establish the geometric meaning of a
closed-tube far-rim center.

Do not reinvestigate their exit fidelity broadly.

Do not modify them.

GEOMETRIC CANDIDATE CALCULATIONS

For each open level, calculate in source/model, mapped logical, and host space
where appropriate:

A. TERMINAL MIDPOINT
- midpoint of Far[0] and Far[last]

B. FULL FAR-VERTEX CENTROID
- arithmetic mean of all mapped far vertices

C. FAR ENVELOPE CENTER
- center of far-vertex min/max bounds

D. FAR POLYLINE LENGTH-WEIGHTED CENTER
- center/centroid weighted by open far-rim segment lengths

E. RAIL CONVERGENCE FIT
- least-squares or equivalent common convergence point implied by mapped rails
  if the rails meaningfully converge rather than remain exactly concurrent

F. PROJECTION / ADAPTER FOCAL POINT
- per current mapping equations

G. ROM-METADATA REFERENCE POINT
- only if metadata equations establish one

H. COMPLETE TUBE PERSPECTIVE CENTER
- only if mathematically distinct and justified

Do not create screenshot-derived candidates.

For each point state:

- exact definition;
- coordinate space;
- whether it is invariant under ordinary translation;
- how it moves under live ExitZoom K/ZOffset;
- relationship to visible far geometry;
- relationship to active presentation-adapter equations.

COMMON-PIVOT TEST

Neal observes that Levels 8/9/10 appear to have the same focal behavior.

Test whether any mathematically defined candidate produces approximately or
exactly the SAME logical presentation pivot across Levels 8/9/10 after their
accepted placements.

If one does:

- identify why;
- determine whether sharing is structural or accidental;
- establish whether the candidate follows from the presentation architecture
  rather than visual coincidence.

Do not force a shared point if the equations demonstrate level-specific pivots.

LEVEL-SPECIFIC TEST

Also test the alternative:

Each level has an independently derived pivot from its own tube geometry.

Determine whether such candidate pivots preserve the same geometric invariant,
for example:

"center of far open polyline"

even when their coordinates differ.

Distinguish:

SAME NUMERIC PIVOT

from:

SAME GEOMETRIC RULE.

The likely architecture should prefer one geometric rule over per-level
hand-entered coordinates.

ROM / ATARI EVIDENCE

Inspect existing local ROM/disassembly/presentation evidence only as needed to
answer whether open-tube vector construction or presentation metadata defines
a center/reference useful for exit.

Do not search the Internet for other Tempest implementations.

Do not use Mark Incitti as Atari authority.

Do not reopen raw ROM files unless current project rules and a specific missing
fact genuinely require bounded verification.

If Atari evidence does not define the focal rule, state that clearly.

A mathematically justified C# PROJECT TRANSLATION may still be acceptable.

EXITZOOM STATE

Preserve the established production cadence:

- AvoidSpikes where applicable;
- ExitDelay;
- ZOffset phase;
- K-growth phase;
- successful handoff.

Do not reinvestigate simulation.

Use bounded samples sufficient to compare candidate pivots.

CURRENT COMPENSATION DIAGNOSTIC

For each candidate pivot, analytically calculate what the current generic
compensation would do if that candidate were latched instead of terminal
midpoint.

Measure:

- candidate's uncompensated drift;
- compensation required;
- whether the tube expands coherently around that point;
- whether all mapped far geometry behaves consistently;
- whether one side of the open tube would be artificially favored;
- whether the candidate remains inside/relevant to the visual tube envelope.

Do not implement any candidate.

PERSPECTIVE-CENTER TEST

The visual defect may not be reducible to "hold one existing vertex-derived
point fixed."

Test whether the open-tube ROM adapter itself uses the wrong ExitZoom depth
normalization such that pinning any final translation point cannot restore a
coherent zoom.

Specifically determine:

- does holding a candidate pivot fixed make all other far vertices expand
  coherently?
- do relative distances among far vertices change appropriately?
- does the open far geometry undergo a similarity-like expansion around the
  candidate?
- or does the adapter non-uniformly distort the far geometry as K/ZOffset
  change?

This is critical.

If the ROM adapter's ExitZoom transformation changes SHAPE rather than merely
scale/translate the open far structure, then another pivot alone cannot fix the
problem.

Do not assume translation compensation is sufficient merely because it fixed
Level 5.

TRANSFORM-DECOMPOSITION ANALYSIS

For Levels 8/9/10, compare mapped far-geometry point sets at:

- ExitZoom onset;
- end of ZOffset phase;
- mid K-growth;
- late K-growth.

Determine whether each later set relative to onset is representable as:

1. translation only;
2. translation + uniform scale about one pivot;
3. translation + anisotropic scale;
4. a more complex non-uniform transformation.

Use all mapped far vertices, not only terminal points.

Report fitting residuals or exact algebraic equivalence where practical.

If one fixed pivot + uniform scale explains the full far set, identify that
pivot.

If not, state that the compensation mechanism is insufficient for open tubes.

NEAR/FAR CONSISTENCY

Repeat enough analysis on near geometry to ensure that an open-tube candidate
does not correct the far end while producing an incoherent near rim.

Determine how near and far presentation geometry relate under ExitZoom.

The desired correction must preserve the sense of looking down the tube toward
one stable deep focal region.

CURRENT CANDIDATE PRESERVATION

Do not edit or remove the current ExitZoomFocalCompensation candidate in this
read-only task.

The fact that Level 5 is manually fixed is useful evidence.

Do not revert it during investigation.

Do not stage or commit it.

The next implementation prompt can refine the candidate after this report if
evidence warrants.

BLASTER FIX PRESERVATION

Do not modify or reopen:

- accepted Blaster ordinary outward orientation;
- accepted Blaster ExitZoom/Approach scale;
- Blaster color/brightness;
- Blaster gameplay.

The current problem is tube focal/perspective geometry, not Blaster artwork.

FROZEN PLAYING PRESENTATION

Do not change or recommend retuning during this investigation:

- Level 8 +212.28139;
- Level 9 +180.96825;
- Level 10 +50;
- Level 5 ordinary Playing mapping;
- Level 1-6 accepted Playing placement;
- perspective values;
- tube geometry;
- entity registration;
- HUD.

Ordinary Playing is accepted.

Only ExitZoom is under investigation.

NO IMAGE ANALYSIS

No new image analysis is required or authorized.

Do not search for, enumerate, open, inspect, or analyze screenshots, images,
videos, frames, contact sheets, or visual assets.

The new manual observations supplied in this task are sufficient.

Do not revisit prior frames.

READ-ONLY REQUIREMENT

Do not modify:

- source;
- tests;
- project files;
- configuration;
- durable documentation;
- presentation;
- gameplay;
- recordings;
- Git state.

Temporary bounded calculations outside tracked project paths are permitted.

Python/PowerShell or another local calculation tool may be used solely for
bounded arithmetic/geometric analysis.

Do not write analysis artifacts into tracked project paths.

Do not launch the interactive game.

Do not stage, commit, push, reset, restore, clean, stash, rebase, or switch
branches.

AI-LOGS

Follow .github/copilot-instructions.md.

Save:

- this substantive prompt under AI-Logs/Prompts/;
- the paired open-tube pivot investigation report under AI-Logs/Reports/.

These are the only permitted file writes.

Do not stage or commit AI-Logs.

REPOSITORY SAFETY

Record:

- branch;
- HEAD;
- upstream;
- ahead/behind;
- index;
- tracked worktree;
- relevant untracked state.

The current candidate is expected to remain uncommitted on top of baseline:

753728e79ed86624c9b1728dfd1c8db8f49d1e2b

Verify actual state.

Preserve:

- current ExitZoomFocalCompensation candidate files;
- .github/copilot-instructions.md local modification;
- local arcade-reference files.

Do not attribute pre-existing candidate work to this investigation.

EXPECTED REPORT

Answer near the beginning:

1. Is terminal-endpoint midpoint the correct ExitZoom pivot for open tubes?
2. If not, why not?
3. Do Levels 8/9/10 share one numeric pivot, or only one geometric rule?
4. What candidate pivot best corresponds to the open-tube analogue of a closed
   far-rim center?
5. Can one translation-based focal compensation correctly render open-tube
   ExitZoom?
6. Or does the ROM-metadata adapter distort open far geometry in a way that one
   pivot cannot repair?
7. What exact transformation should a future implementation test?
8. Can Level 5's now-correct candidate remain unchanged?

The report must include:

- manual result classification;
- all evidence sources;
- open-tube geometry definitions;
- Level 8 candidate-point table;
- Level 9 candidate-point table;
- Level 10 candidate-point table;
- Level 5 closed-control comparison;
- terminal midpoint analysis;
- far centroid analysis;
- far-envelope-center analysis;
- length-weighted far-polyline analysis;
- rail-convergence analysis;
- adapter/projection focal-point analysis;
- any ROM-metadata reference-point analysis;
- common-numeric-pivot test;
- common-geometric-rule test;
- source/logical/host-space distinction;
- ExitZoom trajectories for each candidate;
- full far-point transform decomposition;
- fitting residuals/algebraic equivalence where practical;
- near/far consistency analysis;
- whether translation compensation is fundamentally sufficient;
- smallest technically justified future correction direction;
- whether evidence supports an implementation prompt;
- confirmation no Blaster accepted behavior was changed;
- confirmation accepted Playing placements remain frozen;
- confirmation no project/Git state changed except AI-Logs;
- exact prompt/report paths.

Do not ask Neal questions during execution.

Do not pause for manual input.

Complete autonomously and stop after reporting the evidence.

TEMPEST_OPEN_TUBE_EXIT_PIVOT_READONLY_INVESTIGATION