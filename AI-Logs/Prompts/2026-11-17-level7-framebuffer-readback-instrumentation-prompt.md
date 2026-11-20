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

Implement a tightly bounded, development-only Level 7 diagnostic capable of
observing ACTUAL RASTERIZED FRAMEBUFFER OUTPUT for a small number of
synchronized frames during the unresolved depth-motion regression.

This is an instrumentation milestone only.

Do NOT fix, compensate for, tune, recalibrate, or otherwise alter the Level 7
motion behavior.

CURRENT EVIDENCE

Read the current project instructions and only the reports immediately
necessary to understand the existing instrumentation and latest boundary.

At minimum read:

AI-Logs/Reports/2026-11-17-level7-visible-runtime-motion-instrumentation-report.md

AI-Logs/Reports/2026-11-17-level7-good-bad-runtime-ab-analysis-report.md

Read earlier Level 7 reports only if one of those reports identifies a
specific fact needed for this implementation.

The latest A/B investigation used two real visible captures:

GOOD CONTROL:
recorded playback that Neal watched and confirmed did NOT exhibit the
depth-motion regression.

BAD MANUAL:
normal manual Level 7 gameplay during which Neal clearly DID observe the
depth-motion regression. Neal intentionally allowed many Tankers to travel
all or nearly all the way toward the rim.

The A/B analysis established that the visibly bad run remains numerically
normal through every currently measured motion stage.

For qualifying fixed-lane Tanker trajectories in the bad run:

- simulation height progression was constant;
- mapped-anchor motion was constant across depth;
- final camera-transformed anchor motion was constant across depth;
- geometry submitted by VectorRenderer moved at a constant rate across depth;
- submitted geometry extrema did not reproduce progressive slowdown;
- simulation/render progression remained one tick per Playing frame;
- elapsed renderer timing did not contain a depth-correlated slowdown.

Player shots likewise remained essentially constant through the currently
instrumented renderer stage despite Neal visibly observing the inverse
near-to-deep motion problem.

The measurements therefore do NOT reproduce the bug at or before the current
latest observable renderer stage.

This does NOT invalidate Neal's manual observation.

The existing diagnostic stops before observing actual rasterized framebuffer
pixels.

PURPOSE

Extend the diagnostic boundary by ONE stage only:

from geometry submitted to Raylib

to

actual rasterized framebuffer pixels produced for selected Level 7 frames.

The purpose is to determine whether the visible depth-motion regression is
already present in actual framebuffer output even though the submitted
geometry remains mathematically constant.

Do not investigate Windows composition, monitor timing, physical refresh, or
scanout in this milestone unless required only to accurately explain a
readback limitation.

Do not implement broad display diagnostics.

FIRST: DETERMINE FEASIBILITY

Inspect the existing Raylib/render architecture and determine the smallest
safe development-only way to read back the actual rasterized Level 7 frame.

Use an existing supported Raylib/framebuffer/image readback mechanism if the
current project/runtime provides one.

Do NOT assume an API name or implementation technique from this prompt.
Determine it from the actual project dependencies/API.

The captured pixels must correspond to the already rendered production frame,
not to a separately reconstructed scene or duplicate diagnostic renderer.

If actual framebuffer readback at the required stage is technically
unavailable in the current architecture, STOP before implementing a fake,
approximate, duplicate, or off-screen reproduction.

Report precisely what can and cannot be observed and identify the next
smallest real observable boundary.

BOUNDED CAPTURE, NOT CONTINUOUS VIDEO

Do NOT create another massive per-frame visual capture.

The previous JSONL diagnostics were intentionally comprehensive and became
very large. This milestone requires a narrowly sampled framebuffer diagnostic.

Design a bounded capture strategy.

The target is only enough consecutive framebuffer evidence to measure actual
rasterized Tanker motion at representative:

- deep;
- middle;
- near-rim

depths.

Prefer short bursts of consecutive frames around selected qualifying
conditions instead of continuously saving every Level 7 frame.

The diagnostic should have explicit hard limits on:

- number of captured bursts;
- frames per burst;
- total frames;
- disk use where practical.

Report those limits.

Do not silently continue capturing after the limit.

CORRELATE PIXELS WITH EXISTING DIAGNOSTICS

Reuse the existing Level 7 runtime diagnostic concepts wherever practical.

Each selected framebuffer frame must be correlatable with:

- render-frame sequence;
- simulation tick;
- Tanker runtime identity;
- Tanker lane;
- Tanker state;
- Tanker gameplay height;
- normalized depth;
- mapped anchor;
- camera-transformed/submitted anchor;
- submitted geometry bounds;
- applicable render timing information.

The purpose is to compare:

EXPECTED/SUBMITTED MOVEMENT

against

ACTUAL RASTERIZED PIXEL MOVEMENT

for the same Tanker over the same consecutive frames.

Do not create separate unsynchronized diagnostics that make that correlation
ambiguous.

PRIMARY PROBE: TANKER

Tankers are the primary diagnostic target.

Prefer fixed-lane, continuously climbing Tankers with:

- unchanged lane;
- unchanged relevant state;
- positive tick progression;
- no pause;
- no spawn/removal boundary;
- no death/state-transition interval.

Capture short consecutive-frame bursts for Tankers when normalized depth
falls into representative deep, middle, and near-rim regions.

The diagnostic should collect enough consecutive framebuffer frames in each
burst to measure actual rasterized displacement rather than relying on a
single still image.

Do not attempt to determine arcade fidelity.

The only question is whether actual framebuffer motion corresponds to the
constant submitted movement already measured.

PLAYER SHOTS

If the architecture allows the same bounded facility to capture player-shot
motion without materially expanding scope, support the same synchronized
framebuffer observation for representative player shots.

Shots are useful as an inverse probe because Neal observes them becoming
faster toward the deep end.

However, do not make shot support block the Tanker framebuffer milestone if
it would require substantial additional architecture.

BLASTER

Where already available in the synchronized frame, retain the existing
Blaster submitted-position diagnostic as a real-time reference.

Do not build special Blaster raster tracking unless it is naturally available
from the same bounded framebuffer analysis.

ACTUAL PIXELS, NOT A SECOND THEORETICAL PROJECTION

Do not satisfy this task by:

- recalculating entity anchors;
- calculating expected pixel bounds from VectorRenderer geometry;
- repeating camera transformations;
- rendering entities into a separate diagnostic representation;
- taking the previously submitted geometry and mathematically predicting its
  framebuffer location.

The existing diagnostic already does those things sufficiently to establish
the current boundary.

This milestone must observe pixels produced by the production rendering path.

CAPTURE FORMAT

Choose the smallest maintainable diagnostic representation supported by the
actual implementation.

Possible approaches may include bounded framebuffer images plus synchronized
metadata, or another lossless representation of actual framebuffer pixels.

Do not select a format merely because it is suggested here.

Preserve enough pixel information to support later movement analysis.

Avoid lossy compression if it would interfere with measuring vector-object
movement.

Output must remain in an ignored TEMP diagnostic location.

Do not modify or overwrite the existing control/manual JSON diagnostic files.

Do not modify gameplay recordings.

NO AUTOMATIC IMAGE/FRAME ANALYSIS IN THIS TASK

This task authorizes implementation and automated validation of the
framebuffer-capture mechanism.

It does NOT authorize Copilot to search for, enumerate, inspect, visually
analyze, batch-process, or interpret unrelated repository images, screenshots,
videos, frames, or contact sheets.

For the newly generated diagnostic framebuffer output:

Automated programmatic validation may verify file existence, dimensions,
format, identifiers, hashes, bounded count, correlation metadata, and other
non-visual structural properties necessary to establish that capture works.

Do not visually inspect or perform motion analysis on the newly captured
frames in this implementation milestone.

Actual framebuffer motion analysis will be a separate task after Neal has
performed the visible run and confirmed the bug occurred during capture.

Do not inspect TEMP/myNotes.md.

Do not inspect protected ROM copies.

OPT-IN ONLY

This must remain development-only and explicitly opt-in.

Normal gameplay and existing --atari-presentation behavior with the new
diagnostic disabled must remain unchanged.

Reuse or safely extend the existing Level 7 visible-runtime diagnostic switch
if appropriate, or add a narrowly scoped companion option if cleaner.

Do not make framebuffer readback part of normal rendering.

PERFORMANCE / OBSERVER EFFECT

Framebuffer readback can potentially affect runtime performance.

Explicitly investigate this risk while designing the diagnostic.

Minimize readback frequency and capture volume.

Record or clearly report enough timing information to determine whether a
capture burst itself causes substantial stalls.

Do not claim that a captured slowdown represents the original bug if the
diagnostic itself materially stalls rendering.

If readback necessarily changes runtime behavior enough to invalidate the
experiment, report that limitation rather than pretending the diagnostic is
transparent.

NO BEHAVIOR CORRECTION

Do NOT:

- implement nonlinear depth mapping;
- change record-06 coordinates;
- change Level 7 tube geometry;
- change tube placement;
- change perspective;
- alter camera configuration;
- change entity speeds;
- change shot speeds;
- change Tanker behavior;
- change Blaster movement;
- change simulation timing;
- change fixed-step behavior;
- change recording/playback;
- change presentation interpolation;
- restore the previous Level 7 projection;
- alter the mapped-Flipper crash correction;
- make a visual improvement as part of diagnostics.

If implementation reveals a suspected cause, record it in the report and
STOP short of correcting it.

VALIDATION

Perform bounded automated validation appropriate to the new diagnostic.

At minimum establish:

1. project builds successfully;
2. relevant existing tests remain passing;
3. new diagnostic option/configuration validation works;
4. diagnostics-disabled behavior remains unchanged;
5. capture limits are enforced;
6. correlation metadata can associate a framebuffer capture with the
   corresponding simulation/render/entity state;
7. output location is ignored;
8. existing diagnostic logs and recordings are not modified;
9. git diff --check passes.

If possible without launching the interactive game, test serialization,
selection logic, capture limits, metadata generation, and filename/path
validation.

Do NOT launch the interactive game.

All commands/tests/operations must have explicit bounds or hang timeouts.

Follow the project's established Windows-security block/stall policy.

Never report a canceled, timed-out, stalled, or blocked validation as passed.

MANUAL HANDOFF

At completion provide Neal with the exact command needed to run normal Level 7
with:

- --atari-presentation;
- existing visible-runtime diagnostic correlation as required;
- the new bounded framebuffer diagnostic enabled.

Do not ask Neal to perform another broad control/manual pair unless actual
implementation evidence demonstrates it is necessary.

The immediate priority is to capture the BAD manual condition because the
good control behavior and previous A/B data are already established.

Explain exactly:

- where the framebuffer samples will be written;
- how many samples/bursts can be generated;
- whether old diagnostic files must have unique filenames;
- what message indicates the bounded capture has completed;
- whether Neal can continue playing afterward;
- what Neal should report after the run.

Neal's task should be simple:

1. run the supplied command;
2. manually play Level 7;
3. confirm whether the known depth-motion regression was visibly occurring
   during the run;
4. preserve the resulting new framebuffer diagnostic output.

Do not require Neal to inspect the captured frames.

NEXT ANALYSIS, NOT THIS TASK

After Neal provides a capture from a run in which the bug was visibly present,
a separate read-only analysis will compare actual rasterized framebuffer
movement against synchronized submitted geometry.

Do not perform that analysis in this implementation task.

GIT PRESERVATION

This worktree contains substantial uncommitted presentation and diagnostic
work.

Record initial Git state before modifications.

Preserve existing tracked modifications, index state, and pre-existing
untracked files.

Do not stage, commit, push, reset, restore, stash, rebase, clean, checkout,
or otherwise rewrite existing Git state.

At completion verify preservation.

AI LOGGING

This is substantial Tempest work.

Preserve the substantive task as:

AI-Logs/Prompts/2026-11-17-level7-framebuffer-readback-instrumentation-prompt.md

Write the paired implementation report as:

AI-Logs/Reports/2026-11-17-level7-framebuffer-readback-instrumentation-report.md

The report must record:

- exact framebuffer/readback architecture;
- why it represents actual production rasterized output;
- exact point in the render lifecycle being captured;
- capture-selection conditions;
- burst/frame/total limits;
- output format and location;
- synchronization/correlation metadata;
- performance/observer-effect considerations;
- files changed;
- automated validation and results;
- manual command for Neal;
- manual-validation status PENDING;
- any suspected cause discovered but not corrected;
- limitations;
- initial/final Git state;
- explicit confirmation that no behavior correction occurred.

AI-Logs are ignored working/historical artifacts and must not be staged.

Do not rewrite existing historical reports.

Do not update durable project documentation yet.

SUCCESS CONDITION

This milestone succeeds when the project has a bounded, opt-in diagnostic
capable of capturing actual rasterized production framebuffer pixels for
carefully selected consecutive Level 7 frames, synchronized with the existing
Tanker/depth/runtime diagnostic state.

It does NOT succeed by fixing the regression.

It does NOT succeed by deriving expected pixels from existing geometry.

It does NOT succeed by capturing enormous continuous frame sequences.

At completion STOP and give Neal the exact bounded manual diagnostic procedure.

TEMPEST_LEVEL7_FRAMEBUFFER_READBACK_INSTRUMENTATION