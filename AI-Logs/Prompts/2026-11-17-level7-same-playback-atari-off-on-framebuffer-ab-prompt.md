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

Perform a tightly bounded investigation/preparation milestone for a
SAME-RECORDING Level 7 presentation A/B comparison:

A. Atari presentation OFF
B. Atari presentation ON

The underlying deterministic simulation identity has now been established
through the complete recording.

The task is to determine the smallest safe way to use the already implemented
framebuffer/readback instrumentation, adapting it only if necessary, so the
same recording can be compared at equivalent playback boundaries through:

simulation state
-> renderer-submitted geometry
-> actual rasterized framebuffer pixels.

Do NOT change gameplay or presentation behavior.

Do NOT attempt to fix the motion regression.

ESTABLISHED EVIDENCE

Read current project instructions and these immediately relevant reports:

AI-Logs/Reports/2026-11-17-level7-atari-on-off-deterministic-state-ab-report.md

AI-Logs/Reports/2026-11-17-level7-framebuffer-readback-instrumentation-report.md

Read the prior visible-runtime A/B report only if required to interpret the
existing diagnostic schema.

The deterministic state A/B report established:

- exact recording:
  TEMP\GameplayRecordings\Level-7-slowmo.json
- production v2 recording;
- 3,510 playback ticks;
- 3,511 boundaries including boundary 0;
- same recording/startup configuration used for Atari OFF and ON;
- identical restored simulation state at boundary 0;
- identical authoritative simulation fingerprints at boundaries
  100, 150, 200, 250, and 300;
- no divergence before boundary 100;
- no simulation divergence anywhere through the complete recording;
- --atari-presentation is presentation-only in the current worktree.

Classification:

SIMULATION IDENTICAL THROUGH COMPLETE PLAYBACK

Neal has separately observed visible presentation differences between Atari
OFF and Atari ON when watching this exact recording at equivalent playback
boundaries, including from very early in playback.

No screenshots or image analysis are authorized in this task.

CONTROLLED EXPERIMENT

From this point forward, do NOT use manual gameplay versus recorded playback
as the primary A/B comparison.

The cleaner experiment is:

Same recording.
Same restored game state.
Same recording seed.
Same playback boundary.
Same current source/worktree.

Only presentation mode differs:

Atari OFF
versus
Atari ON.

PURPOSE

Determine how to capture comparable actual production framebuffer output for
the SAME recording under both presentation modes at equivalent playback
boundaries.

The experiment should allow a subsequent analysis to answer:

Given identical underlying simulation state at boundary N,

1. what geometry does the OFF renderer submit?
2. what geometry does the ON renderer submit?
3. what pixels are actually rasterized in the framebuffer?
4. where does the visible OFF/ON motion/presentation difference first become
   measurable?

IMPORTANT: REVIEW EXISTING FRAMEBUFFER TOOL FIRST

The framebuffer diagnostic already implemented was intentionally restricted
to --atari-presentation and primarily designed around a known-bad manual run.

Do not immediately create another framebuffer system.

Inspect the existing implementation and determine whether it can be safely
generalized/reused for deterministic recorded playback with:

Atari OFF
and
Atari ON.

Prefer the smallest modification necessary.

Do not duplicate the framebuffer readback architecture.

The existing implementation uses the production render path and captures the
Raylib screen buffer after VectorRenderer drawing/overlays and before
EndDrawing.

Preserve that production-path readback design.

PLAYBACK-BOUNDARY SELECTION

For this controlled A/B experiment, depth-triggered Tanker capture alone is
not sufficient.

We need equivalent playback boundaries across OFF and ON.

Support bounded capture keyed to deterministic playback boundary numbers.

At minimum support these boundaries:

100
150
200
250
300

These are manually established early comparison points.

For motion measurement, a single image at a boundary is insufficient.

Therefore capture a SHORT consecutive window around each selected boundary.

Choose the smallest technically useful window based on the architecture.

The intent is enough consecutive actual framebuffer frames to measure
movement around each boundary, not continuous video.

Keep total capture tightly bounded.

Document:

- exact selected boundaries;
- consecutive frames captured around each;
- maximum total images;
- maximum disk usage;
- behavior if a selected boundary is missed;
- behavior if the output destination already exists.

Do not generate another enormous per-frame capture.

SAME BOUNDARIES IN BOTH MODES

The OFF and ON runs must capture equivalent playback boundaries.

Do not independently select entities/depth bands in ways that cause the two
runs to sample unrelated simulation moments.

The playback boundary is the primary synchronization key.

Retain simulation tick and entity information as secondary correlation data.

RUNTIME COMMANDS

The final implementation/handoff must provide exact commands for:

A. SAME RECORDING + ATARI PRESENTATION OFF + framebuffer diagnostic

B. SAME RECORDING + ATARI PRESENTATION ON + framebuffer diagnostic

Both must use:

TEMP\GameplayRecordings\Level-7-slowmo.json

Do not substitute a different recording.

Do not launch either interactive run during this implementation task.

Neal will perform the two visible runs afterward.

PRESENTATION-OFF SUPPORT

The existing framebuffer diagnostic currently requires --atari-presentation.

If framebuffer readback itself is technically independent of presentation
mode, generalize the diagnostic narrowly so it can capture production
framebuffer output in both OFF and ON modes.

Do not weaken unrelated command-line validation.

Do not change normal presentation selection.

Do not cause Atari-specific entity mapping to execute in the OFF run.

The OFF run must remain the existing non-Atari presentation.

The ON run must remain the current Atari presentation.

DIAGNOSTIC CORRELATION

For every captured framebuffer image retain sufficient metadata to correlate:

- run mode: Atari OFF or ON;
- playback file identity;
- playback boundary;
- simulation tick;
- Level/state;
- render-frame sequence;
- entity identities/state where already available;
- gameplay lane and height/depth where already available;
- renderer-submitted geometry where already available;
- framebuffer image filename;
- readback/capture timing;
- normal render timing.

Do not force Atari-specific mapped coordinates onto the OFF run merely for
schema symmetry.

Record the authoritative renderer/presentation values that actually apply to
each mode.

If schemas necessarily differ between legacy and Atari presentation,
document those differences explicitly.

FRAMEBUFFER OUTPUT

Capture actual production rasterized framebuffer pixels.

Do not:

- reconstruct a diagnostic scene;
- render a second copy of gameplay;
- mathematically synthesize expected pixels;
- substitute submitted vector geometry for framebuffer readback.

Use the existing production framebuffer readback mechanism if valid.

Preserve lossless output.

Keep all generated diagnostic output under ignored TEMP diagnostic
directories with separate OFF and ON run names.

OBSERVER EFFECT

Framebuffer readback/PNG export can stall rendering.

Preserve existing readback/write/capture timing instrumentation.

Because OFF versus ON will be compared, use equivalent capture policy in both
runs.

The later analysis must be able to distinguish capture-induced stalls from
normal motion.

Do not claim timing equivalence merely because both modes use the same
capture policy.

NO IMAGE ANALYSIS IN THIS IMPLEMENTATION TASK

This task authorizes implementation/preparation only.

Do not open, visually inspect, compare, batch-process, or analyze resulting
framebuffer images.

Do not inspect Neal's existing screenshots.

Do not search for unrelated screenshots, videos, frames, or user visual
assets.

Do not inspect TEMP/myNotes.md.

Do not inspect protected ROM copies.

AUTOMATED VALIDATION

Perform bounded automated validation of the A/B capture facility.

At minimum validate:

1. project builds;
2. relevant existing tests continue to pass;
3. framebuffer diagnostic can be configured for recorded playback in Atari
   OFF mode;
4. framebuffer diagnostic can be configured for the same playback in Atari
   ON mode;
5. selected playback-boundary windows are identical between configurations;
6. hard capture/frame/disk bounds are enforced;
7. OFF mode does not accidentally enable Atari presentation;
8. ON mode does enable the existing Atari presentation;
9. correlation metadata contains playback boundary and mode;
10. diagnostics disabled leaves normal behavior unchanged;
11. existing recordings and previous diagnostics remain unmodified;
12. output locations are ignored;
13. git diff --check passes.

Do not launch the interactive game.

All tests/builds/analysis commands require explicit practical timeouts/hang
bounds.

Follow existing Windows-security and stalled-command policies.

Do not report blocked/timed-out/canceled validation as passed.

MANUAL HANDOFF

At completion give Neal ONLY the two exact commands necessary for the
controlled experiment:

COMMAND A:
same Level-7-slowmo recording, Atari OFF, bounded framebuffer capture.

COMMAND B:
same Level-7-slowmo recording, Atari ON, bounded framebuffer capture.

Explain:

- which command should be run first, if order matters;
- exact output folders/files;
- capture-complete message;
- whether playback continues after capture;
- what Neal should visually verify.

Neal's visual task is simple:

1. Run OFF.
2. Confirm OFF presentation behaves as expected.
3. Run ON.
4. Confirm the known Atari-presentation motion/presentation problem is
   visible.
5. Preserve both bounded capture outputs.

Neal should NOT manually inspect or compare captured PNGs during this step.

If the ON/OFF difference itself is obvious during visible playback, report
that observation afterward.

NEXT ANALYSIS

Do not analyze framebuffer pixels in this task.

After Neal has produced both bounded captures, a separate read-only analysis
will compare:

identical simulation boundary
-> OFF/ON submitted geometry
-> OFF/ON rasterized framebuffer movement.

That analysis should determine whether the framebuffer faithfully reflects
the geometry submitted by each presentation mode and where the observed
difference originates.

NO BEHAVIOR CORRECTION

Do not:

- modify simulation;
- modify playback;
- change entity speed;
- change Tanker behavior;
- change shots;
- change Blaster movement;
- change record-06 mappings;
- change tube geometry;
- change Level 7 centering;
- change perspective;
- add nonlinear depth transfer;
- alter timing;
- alter camera behavior;
- change the mapped-Flipper crash correction;
- restore old presentation code;
- tune anything to make the game look better.

If a likely defect is discovered while preparing the diagnostic, report it
but do not fix it.

GIT PRESERVATION

The worktree contains substantial uncommitted presentation and diagnostic
work.

Record initial Git state.

Preserve branch, HEAD, upstream, index, tracked modifications, and
pre-existing untracked files.

Do not stage, commit, push, reset, restore, stash, rebase, clean, checkout, or
otherwise rewrite Git state.

AI LOGGING

Save this substantial task as:

AI-Logs/Prompts/2026-11-17-level7-same-playback-atari-off-on-framebuffer-ab-prompt.md

Write the paired report as:

AI-Logs/Reports/2026-11-17-level7-same-playback-atari-off-on-framebuffer-ab-report.md

AI-Logs remain ignored.

Do not rewrite historical reports.

Do not update durable project documentation yet.

REPORT REQUIRED ANSWERS

The final report must answer:

1. Could the existing framebuffer diagnostic be safely reused/generalized for
   Atari OFF and ON recorded playback?

2. What minimal implementation changes were necessary?

3. What exact playback boundaries/windows will be captured?

4. What is the hard maximum framebuffer-image count and disk bound per run?

5. How are OFF and ON capture frames synchronized?

6. What authoritative renderer geometry/state is retained for each mode?

7. How is actual framebuffer readback performed?

8. What observer-effect timing information is retained?

9. What automated validation passed or failed?

10. What exact commands should Neal run for OFF and ON?

11. What should Neal visually confirm during each playback?

12. What files/folders should Neal preserve for the next analysis?

Manual A/B capture status must remain PENDING.

Stop after implementation/preparation and handoff.

TEMPEST_LEVEL7_SAME_PLAYBACK_ATARI_OFF_ON_FRAMEBUFFER_AB