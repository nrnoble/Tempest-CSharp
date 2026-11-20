READ THE ENTIRE USER-PROVIDED TASK BEFORE TAKING ANY ACTION.

Before using tools, opening files, running commands, inspecting repository
content, or modifying anything, locate the unique completion token near the
end of this user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on
a line by itself in the user-provided task. VS Code/agent-generated reminders,
tagged-file information, attachment metadata, or other system-generated
context may appear after the user's token and do not invalidate it.

This check is intended to detect an incomplete or truncated USER task, not
appended agent context.

If exactly one complete TEMPEST_ token cannot be found, STOP before any
project or repository action and explain why.

TASK

Implement a tightly bounded, development-only Level 7 runtime diagnostic
instrumentation facility to capture and explain the unresolved depth-motion
regression in the ACTUAL visible production runtime.

This is an instrumentation milestone, NOT a behavior-correction milestone.

Do not attempt to fix, compensate for, tune, calibrate, or otherwise alter
the reported Level 7 motion behavior in this task.

BACKGROUND

Read the relevant current project instructions, durable Tempest
documentation, and only the recent Level 7 reports required to understand
the current presentation implementation and regression.

At minimum read:

AI-Logs/Reports/2026-11-17-level7-depth-motion-regression-delta-readonly-report.md

Also read the directly relevant Level 7 record-06 entity-registration and
presentation implementation reports identified by that report.

Do not reopen accepted tube geometry, placement, perspective, centering,
gameplay, or the mapped-Flipper crash correction.

The previous investigation established an important apparent contradiction.

For representative FIXED-LANE production trajectories, the underlying
simulation increments were normal and the current calculated mapped anchor
motion was constant.

Examples from that investigation:

- a representative Tanker mapped anchor moved about 0.597 host pixels per
  tick throughout its fixed-lane climb;
- a representative player shot mapped anchor moved about 4.756 host pixels
  per tick throughout its fixed-lane travel.

The investigation therefore could not mathematically reproduce Neal's
reported depth-dependent speed behavior.

DO NOT spend this task merely proving those calculations again.

NEAL'S MANUAL OBSERVATION

Neal's observation is stronger than a subjective impression caused simply by
entity size or perspective.

During affected current normal Level 7 gameplay:

- the Blaster continues moving around the rim at normal real-time speed;
- enemies approaching the rim actually appear to traverse progressively less
  visible distance per unit of real elapsed gameplay time;
- consequently Neal has progressively more real opportunity to move the
  Blaster and shoot an approaching enemy as it gets closer to the rim;
- player shots exhibit the opposite depth-motion problem, appearing
  comparatively slow near the rim and increasingly fast toward the deep end;
- the problem is visible throughout affected Level 7 gameplay.

This behaves as though a depth-dependent slow-motion/fast-motion presentation
effect is being applied to depth-moving objects while rim movement remains
normal.

This description is an observation and diagnostic model, NOT an assertion
about the implementation mechanism.

Do not assume there literally is a slow-motion system, frame interpolation,
or specific timing defect.

IMPORTANT CONTROL OBSERVATION

Neal reports that an older Level 7 recorded-input file, recorded before this
bug was introduced, still plays back correctly and does NOT visibly exhibit
the regression.

Meanwhile current normal Level 7 gameplay does exhibit it.

Treat this as potentially important A/B evidence.

If an appropriate existing ignored recording can be identified without
inspecting prohibited user scratch files or visual assets, it may be used as
the known-good playback control.

Do not modify the recording.

Do not assume in advance why playback differs from current normal gameplay.
Instrument and compare the actual runtime paths.

PURPOSE

Instrument the actual visible Level 7 production presentation/render/runtime
path so that the numerical data being logged corresponds to the frames and
motion Neal actually sees during interactive rendering.

We need to discover where the previous mathematically correct state/projection
analysis diverges from actual visible runtime behavior.

The diagnostic must support BOTH:

A. an existing Level 7 recorded-input playback that Neal reports looks
   correct; and

B. normal manual Level 7 gameplay where Neal sees the regression.

The same instrumentation should be usable in both modes so a subsequent
investigation can directly compare them.

CRITICAL DISTINCTION

Do not substitute another headless projection-only analysis for runtime
instrumentation.

The preceding report already demonstrated what the current projection math
says should happen.

This task must instrument the actual running production presentation/render
sequence sufficiently late in the path to determine what is happening to
objects in the sequence of frames actually being presented.

At the same time, retain enough upstream information to correlate final
visible motion with simulation state and mapped presentation state.

DIAGNOSTIC QUESTIONS

Design the instrumentation to make a subsequent investigation able to answer:

1. During an affected real-time Level 7 run, how far does the FINAL presented
   position of a continuously tracked enemy advance between successive
   actually presented frames/times?

2. Does that final per-frame/per-time movement change systematically with
   depth even though the previously measured mapped anchor delta is constant?

3. What happens simultaneously to the Blaster's final presented rim movement?

4. What happens simultaneously to player-shot final presented movement from
   the rim toward the deep end?

5. Do simulation ticks, update ticks, render calls, and actual presentation
   progression remain in the expected relationship, or does that relationship
   differ with mode, depth, entity, or frame?

6. Does the known-good recorded playback take a different runtime presentation
   path, timing path, state path, interpolation path, or render path from
   affected normal Level 7 gameplay?

7. At what narrowest observable stage do known-good playback and affected
   normal gameplay first diverge?

Do not invent an answer before collecting evidence.

INSTRUMENTATION DESIGN

First inspect the actual architecture and identify the smallest maintainable
development-only instrumentation seams.

Prefer existing diagnostic/logging infrastructure where suitable.

The following fields describe the evidence we need, not mandatory names or
a demand to duplicate calculations.

For each traceable relevant Level 7 entity, capture where authoritative data
is available:

- stable runtime sequence/frame identity;
- simulation/update tick;
- render invocation/frame sequence;
- elapsed/runtime timing information suitable for comparing visible movement
  against actual elapsed presentation time;
- entity identity;
- entity type;
- entity state;
- gameplay lane;
- gameplay height/depth;
- normalized mapped depth;
- mapped presentation anchor;
- final authoritative screen/host presentation position used for that
  rendered frame;
- local presentation scale;
- displacement from the previous observation of that same entity;
- elapsed time and/or relevant tick/frame difference between those
  observations;
- displacement rate against the relevant runtime time/frame basis;
- lane changes;
- state transitions;
- spawn/removal events;
- pauses or transitions that invalidate a fixed-lane depth-motion sample;
- runtime mode information sufficient to distinguish manual gameplay from
  recorded playback.

Also capture simultaneous Blaster position/motion so enemy motion can be
compared against a known rim-moving reference in the SAME runtime interval.

PLAYER SHOTS ARE REQUIRED

Player/Blaster shots must be instrumented.

They are particularly valuable because Neal observes the opposite depth
behavior from climbing enemies.

Where possible, preserve enough information to follow an individual shot
continuously from near the rim toward the deep end.

VISIBLE OUTPUT VERSUS ANCHOR

The previous investigation already found constant mapped anchor motion.

Therefore do not stop merely because runtime mapped-anchor logging reproduces
that result.

Instrument as late in the actual production visible presentation path as
reasonably possible.

Determine from the real renderer what authoritative concept corresponds most
closely to the position/geometry actually handed to the visible drawing or
presentation operation.

If there is a meaningful distinction among:

- simulation position;
- mapped presentation position;
- camera-transformed position;
- final vector geometry;
- rendering submission;
- actual presented-frame timing;

preserve those distinctions rather than collapsing them into another
calculated theoretical coordinate.

If actual framebuffer/display timing cannot be observed from the existing
architecture without invasive unrelated work, do not fake it. Instrument the
latest authoritative stage available and explicitly report the limitation.

GLYPH EXTENTS

Where authoritative final rendered geometry is readily available, capture
enough information to determine whether visible glyph extrema move
differently from anchor position.

Do NOT reproduce the previous investigation's calculated approximation if
actual renderer data is available later in the path.

Do not duplicate renderer implementation merely to manufacture an answer.

KNOWN-GOOD VERSUS AFFECTED COMPARISON

Make log records sufficiently structured for a subsequent bounded automated
analysis to compare:

CONTROL:
older Level 7 playback that Neal says still looks correct

versus

REGRESSION:
normal current Level 7 manual gameplay where Neal observes the slowing enemy
and accelerating-shot behavior.

Do not assume that the recording being old means it contains old rendering
coordinates. Determine how the current playback system actually drives the
current runtime and log the evidence needed to understand any difference.

If automatic playback can be exercised safely and non-interactively, a
bounded diagnostic playback run may be performed for instrumentation
validation.

Do not launch the interactive game yourself.

Neal will perform the visual manual run later.

LOG OUTPUT

Use an ignored/development diagnostic-output location appropriate to this
project.

Create a consistently structured format suitable for subsequent automated
analysis.

Keep the diagnostic bounded.

Do not continuously log unrelated levels, menus, approaches, exits, or
unrelated systems.

If useful, allow diagnostic output to contain explicit records identifying
fixed-lane continuous segments versus transitions so later analysis does not
mistake pivots or state changes for depth-speed changes.

Include run metadata sufficient to establish which mode generated the file.

Do not modify playback recordings.

Do not write diagnostic data into durable production documentation.

OPT-IN ONLY

Instrumentation must be development-only and opt-in.

Normal behavior with diagnostics disabled must remain unchanged.

Prefer an explicit diagnostic command-line switch consistent with current
project conventions if that is the cleanest fit.

Do not add any calibration or correction control.

NO VISUAL-ASSET ANALYSIS

Do not inspect, enumerate, open, search for, or analyze screenshots, videos,
frames, contact sheets, or other image/video assets.

No image/frame analysis is authorized in this task.

Do not inspect TEMP/myNotes.md.

Do not inspect raw protected ROM copies.

Use project-local disassembly only if genuinely necessary to understand an
existing runtime path, but this task should not require new ROM behavior
research.

NO BEHAVIOR CORRECTION

Absolutely do not:

- implement a nonlinear depth transfer;
- change record-06 coordinates;
- change tube geometry;
- change Level 7 centering;
- change perspective;
- change gameplay speed;
- change enemy movement;
- change shot speed;
- change Blaster movement;
- change simulation timing;
- change interpolation merely to make the problem look better;
- compensate for one unexplained behavior with another transformation;
- revert the mapped-Flipper crash correction.

If investigation while implementing the instrumentation happens to expose an
obvious suspected cause, RECORD IT in the report.

Do not fix it in this milestone.

VALIDATION

Perform bounded automated validation appropriate to the instrumentation
change.

At minimum establish:

1. project builds successfully;
2. relevant automated tests remain passing;
3. diagnostics-disabled behavior remains unchanged;
4. instrumentation can be enabled;
5. a bounded noninteractive run can produce structurally valid diagnostic
   output if the architecture permits;
6. instrumentation does not mutate simulation or playback state;
7. git diff --check passes.

Use explicit time/tick/process/test bounds.

If Windows Application Control or similar security blocks generated
artifacts or commands, follow the project's existing security-block policy.
Do not weaken or bypass Windows security.

If validation stalls or makes no meaningful progress beyond its expected
duration, stop it and isolate with progressively smaller bounded runs.

Never report a canceled, blocked, timed-out, or stalled validation as passed.

Do not launch the interactive game for manual acceptance.

HANDOFF TO NEAL

At completion provide exact commands for Neal to run:

A. the known-good Level 7 recorded playback with diagnostics enabled; and

B. normal Level 7 manual gameplay with diagnostics enabled.

Identify the exact diagnostic output file/location produced by each.

If separate filenames or run identifiers are required for the A/B comparison,
make that procedure simple and explicit.

Do not ask Neal to analyze numbers during gameplay.

The manual requirement is simply:

- play/watch the control playback and confirm it still looks correct;
- play affected normal Level 7 and confirm the regression is visible;
- preserve the resulting diagnostic logs.

After that, Neal can provide the runtime diagnostic data for a separate
analysis milestone.

Do not perform that future analysis now.

GIT PRESERVATION

This worktree contains substantial uncommitted presentation work.

Before changing anything, record the initial Git state.

Preserve all pre-existing tracked modifications and nonignored untracked
files.

Do not stage, commit, push, reset, restore, stash, rebase, clean, checkout,
or otherwise rewrite existing Git state.

At completion verify that unrelated pre-existing work and the index remain
preserved.

AI LOGGING

This is substantial Tempest work.

Save the substantive task under AI-Logs/Prompts with a descriptive
2026-11-17-prefixed paired filename.

Write a corresponding implementation report under AI-Logs/Reports.

The report must record:

- files changed;
- instrumentation architecture;
- exact runtime stages instrumented;
- which values are authoritative existing renderer/runtime values versus
  diagnostic calculations;
- timing/frame/tick information captured;
- how enemies, player shots, and the Blaster are correlated;
- how fixed-lane segments and transitions are distinguished;
- log format and output location;
- playback/manual mode identification;
- known-good playback command;
- affected manual Level 7 command;
- automated validation and results;
- limitations, especially anything later than the latest instrumentable
  renderer stage;
- any suspected cause discovered but NOT corrected;
- initial/final Git state;
- explicit confirmation that no behavior correction was made;
- manual diagnostic status as PENDING.

Do not rewrite historical AI reports.

AI-Logs remain ignored working/historical artifacts and must not be staged
or committed.

SUCCESS CONDITION

This milestone succeeds when Neal can perform two visible runs through the
current production runtime:

1. a known-good older Level 7 playback; and
2. affected current normal Level 7 gameplay;

and obtain comparable diagnostic logs capable of showing where their actual
runtime presentation behavior first diverges.

The milestone does NOT require explaining or fixing the bug.

It specifically must not declare the regression disproven merely because the
simulation increments or mapped anchor equations remain mathematically
constant.

If the latest authoritative renderer-stage measurements remain constant while
Neal still confirms visible slowing/acceleration, preserve that contradiction
as evidence and identify what downstream presentation/timing stage remains
unobserved.

At completion STOP and hand the diagnostic procedure to Neal.

TEMPEST_LEVEL7_VISIBLE_RUNTIME_MOTION_INSTRUMENTATION