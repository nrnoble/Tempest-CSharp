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

Perform a tightly bounded READ-ONLY analysis of the completed SAME-RECORDING
Level 7 Atari presentation OFF versus ON framebuffer experiment.

Do NOT modify production code, tests, configuration, recordings,
instrumentation, presentation, gameplay, diagnostic captures, or Git state.

Do NOT launch the game.

Do NOT generate another capture.

Do NOT implement a correction.

ESTABLISHED MANUAL A/B RESULT

Neal has now run BOTH controlled commands using the exact same recorded input:

TEMP\GameplayRecordings\Level-7-slowmo.json

Manual observation:

ATARI OFF:
Normal playback. The known depth-motion slowdown problem was NOT present.

ATARI ON:
The known slowdown/presentation problem was clearly present.

Neal observed no obvious temporary stalls associated with the bounded early
framebuffer captures.

Neal did NOT notice an explicit framebuffer "capture complete" message or an
error/incomplete message during either run.

Do not infer capture failure merely because the completion message was not
noticed.

First verify the generated evidence directly.

Do NOT ask Neal to rerun anything unless the existing capture is demonstrably
incomplete for a specific required reason.

ESTABLISHED SIMULATION CONTROL

Read:

AI-Logs/Reports/2026-11-17-level7-atari-on-off-deterministic-state-ab-report.md

That investigation established that Atari OFF and Atari ON playback of this
exact recording have IDENTICAL underlying deterministic simulation state at
every one of the 3,511 compared boundaries.

Classification:

SIMULATION IDENTICAL THROUGH COMPLETE PLAYBACK

Therefore any OFF/ON difference in this task is presentation-side.

Do not reopen simulation determinism.

FRAMEBUFFER IMPLEMENTATION CONTEXT

Read:

AI-Logs/Reports/2026-11-17-level7-same-playback-atari-off-on-framebuffer-ab-report.md

and, only if needed:

AI-Logs/Reports/2026-11-17-level7-framebuffer-readback-instrumentation-report.md

The intended synchronized capture windows are:

99, 100, 101
149, 150, 151
199, 200, 201
249, 250, 251
299, 300, 301

Maximum expected PNG count per run: 15.

The framebuffer readback is from the production Raylib screen buffer after
production rendering/overlays and before EndDrawing.

The synchronized trace retains renderer-submitted entity geometry and
correlation metadata for these selected boundaries.

EXPECTED OUTPUTS

Verify these existing outputs:

TEMP\Diagnostics\Level7VisibleRuntime\
level7-same-playback-atari-off-20261117.jsonl

TEMP\Diagnostics\Level7Framebuffer\
level7-same-playback-atari-off-20261117\

TEMP\Diagnostics\Level7VisibleRuntime\
level7-same-playback-atari-on-20261117.jsonl

TEMP\Diagnostics\Level7Framebuffer\
level7-same-playback-atari-on-20261117\

Do not modify them.

PHASE 1 — CAPTURE COMPLETENESS

Before any pixel analysis, verify BOTH runs.

For each run determine:

- run mode;
- presentation mode;
- playback-file identity;
- capture status;
- selected boundaries requested;
- selected boundaries actually captured;
- missed boundaries;
- PNG count;
- correlation-record count;
- PNG dimensions/format;
- whether all expected PNG files are readable;
- whether each PNG has corresponding trace/capture metadata;
- whether the run reached boundary 301;
- whether an explicit capture-complete record/status exists in diagnostic
  output even though Neal did not notice a console message;
- readback/write/capture timing;
- whether any capture operation caused an obvious diagnostic timing outlier.

Expected result is 15 synchronized PNGs per mode.

If either run is genuinely incomplete:

STOP before drawing motion conclusions.

Report exactly what is missing and whether the existing evidence is still
sufficient for any bounded comparison.

Do NOT automatically request a complete rerun.

PHASE 2 — CONTROLLED FRAMEBUFFER ANALYSIS

Only if the required evidence is sufficient:

Analyze the captured framebuffer pixels programmatically.

This task explicitly authorizes analysis of ONLY the newly generated
framebuffer diagnostic images in these two specific OFF/ON output folders.

Do NOT search for, enumerate, open, inspect, or analyze any other screenshots,
images, videos, frames, contact sheets, or visual assets.

Do NOT inspect TEMP/myNotes.md.

Do NOT inspect protected ROM copies.

The analysis must remain synchronized by playback boundary.

Do not compare arbitrary OFF and ON images.

Compare:

OFF boundary 99 with ON boundary 99
OFF boundary 100 with ON boundary 100
OFF boundary 101 with ON boundary 101

and equivalently for the other selected windows.

Remember that OFF and ON intentionally use different presentation geometry.

Raw pixel-coordinate equality between modes is NOT expected.

The question is motion/presentation behavior, not whether the pictures are
identical.

PRIMARY QUESTIONS

Determine:

1. Does each framebuffer faithfully contain the geometry that the
   corresponding renderer says it submitted?

2. Within each three-boundary window, how far do representative rasterized
   entity features move from:

   N-1 -> N
   N -> N+1

3. Does Atari OFF framebuffer motion correspond to the OFF submitted
   geometry?

4. Does Atari ON framebuffer motion correspond to the ON submitted geometry?

5. Is the known ON presentation defect already numerically observable in the
   rasterized framebuffer pixels?

6. If the ON framebuffer reflects exactly the submitted ON geometry, is the
   relevant OFF/ON divergence already present in submitted presentation
   geometry rather than introduced by rasterization?

7. If submitted geometry behaves one way but rasterized pixels behave
   differently, identify the first measurable renderer-to-framebuffer
   divergence.

ENTITY CORRELATION

Use the synchronized trace to identify entities present in consecutive
boundaries.

Prefer stable entity identity and authoritative submitted geometry.

Do not attempt general computer-vision object recognition if the trace already
identifies which submitted geometry belongs to which entity.

Use the trace to constrain any pixel-level search to the relevant
renderer-submitted region.

Primary probes should include, where available in the selected windows:

- Tankers;
- player shots;
- other continuously moving depth entities useful as corroboration;
- Blaster only as a same-frame reference if useful.

Do not require every selected window to contain a Tanker.

Use whatever continuously identifiable entities the synchronized recording
actually supplies.

ATARI OFF VERSUS ON

These two modes intentionally use different projection/presentation systems.

Therefore analyze each mode internally first:

For the SAME entity across boundaries:

OFF:
submitted geometry movement
versus
OFF framebuffer movement

ON:
submitted geometry movement
versus
ON framebuffer movement

Only then compare normalized motion behavior between OFF and ON.

Do not compare absolute screen positions as though the two projection systems
should coincide.

MOTION NORMALIZATION

Where rail lengths, lanes, scale, or presentation geometry differ, normalize
appropriately.

Possible useful quantities include:

- raster displacement per playback boundary;
- submitted-anchor displacement per boundary;
- raster displacement divided by submitted displacement;
- fraction of presented rail/depth distance traversed;
- within-trajectory adjacent-frame motion ratio;
- movement of stable rasterized glyph features/extrema;
- distance remaining to the presented rim versus apparent movement rate.

Use the actual diagnostic schema and geometry rather than inventing fields.

The objective is to test Neal's observation:

ATARI OFF:
normal depth motion.

ATARI ON:
enemies visibly slow as they approach the rim, while the underlying simulation
remains identical.

Do not assume the expected mathematical form in advance.

BOUNDARIES 99–301 ARE EARLY

The selected snapshots are intentionally early because Neal had already found
visible OFF/ON presentation differences from the beginning.

However, distinguish carefully between:

A. general OFF/ON spatial presentation differences;

and

B. the specific progressive depth-speed regression.

If these early 15-frame captures establish only spatial mapping differences
but do NOT contain enough trajectory/depth span to measure the slowdown
itself, say so explicitly.

Do NOT manufacture a slowdown conclusion from insufficient temporal span.

If the captures faithfully establish where rasterization sits in the pipeline
but lack enough duration/depth range to measure the progressive speed curve,
identify the smallest additional bounded capture needed.

Do not request broad continuous video or another massive diagnostic.

SUBMITTED GEOMETRY VERSUS FRAMEBUFFER

This is the most important localization boundary.

Classify the evidence into one of these outcomes:

A. DEFECT PRESENT IN SUBMITTED GEOMETRY

The Atari-ON renderer already submits geometry whose progression contains the
problem, and framebuffer pixels faithfully rasterize it.

B. DEFECT INTRODUCED BETWEEN SUBMISSION AND FRAMEBUFFER

Submitted geometry does not contain the measured behavior, but framebuffer
pixels do.

C. SUBMISSION AND FRAMEBUFFER BOTH NORMAL

Neither submitted geometry nor framebuffer pixels reproduce the manually
observed ON slowdown during the captured boundaries.

D. CAPTURE INSUFFICIENT

The synchronized capture does not span enough useful entity motion/depth to
make the distinction.

Use the classification best supported by the evidence.

Do not force category A, B, or C if D is the honest result.

IMPORTANT RELATION TO PREVIOUS ANALYSIS

Earlier runtime analysis reported constant fixed-lane Atari-ON submitted
motion across depth during a different capture.

Do not merely restate that result.

The current experiment is stronger because:

- same exact recording;
- identical underlying simulation;
- synchronized playback boundaries;
- Atari OFF/ON comparison;
- actual framebuffer pixels.

If the new evidence conflicts with the earlier analysis, investigate and
preserve the conflict rather than selecting whichever result is convenient.

PIXEL ANALYSIS METHOD

Use bounded programmatic image analysis.

Do not manually eyeball hundreds of pixels.

Use the trace geometry to restrict analysis regions.

Prefer deterministic measurements such as:

- non-background/vector-pixel masks;
- connected/localized vector feature positions where unambiguous;
- centroids/extrema only where those quantities meaningfully represent the
  entity;
- displacement of a stable glyph component across consecutive frames.

Be cautious because vector antialiasing, scale changes, rotation, overlapping
entities, tube rails, spikes, bullets, and HUD elements can contaminate naïve
pixel masks.

Reject ambiguous samples rather than forcing a measurement.

Document the measurement method.

Do not create synthetic replacement images.

Temporary derived analysis data may be written outside tracked repository
state or under ignored TEMP if necessary.

Do not alter original PNGs.

OBSERVER EFFECT

Analyze the recorded framebuffer readback/export timing.

Neal observed no obvious stalls.

Determine whether capture-frame timing contains measurable stalls.

If PNG export causes substantial timing spikes, account for them before
interpreting consecutive-frame motion.

Do not mistake diagnostic-induced delay for the original defect.

REPORT

Produce a concise, evidence-rich report containing:

1. Capture-completeness result for OFF.
2. Capture-completeness result for ON.
3. Exact synchronized boundaries available.
4. Capture/readback/write timing and observer-effect assessment.
5. Which entities/trajectory fragments were usable.
6. How framebuffer entity positions were measured.
7. OFF submitted-versus-framebuffer motion result.
8. ON submitted-versus-framebuffer motion result.
9. Whether the known ON slowdown is numerically present in these framebuffer
   samples.
10. Earliest measured stage where OFF/ON motion behavior meaningfully
    diverges.
11. Classification A/B/C/D.
12. Narrowest justified next step.

Clearly separate:

ESTABLISHED MANUAL OBSERVATION
LOG/METADATA FACT
FRAMEBUFFER MEASUREMENT
INTERPRETATION
UNRESOLVED

Do not claim the bug is absent because a particular capture failed to measure
it.

Do not claim framebuffer causality merely because OFF and ON images look
different.

NO FIX

Do NOT modify:

- projection;
- presentation;
- geometry;
- entity movement;
- simulation;
- playback;
- camera;
- framebuffer capture;
- timing;
- record-06 data;
- perspective;
- Blaster;
- Tankers;
- shots;
- mapped-Flipper correction.

If a likely source of the defect becomes evident, record the precise evidence
and recommend the smallest correction/investigation boundary.

Do not implement it.

VALIDATION / PRESERVATION

Verify original diagnostic JSONL files and framebuffer PNGs remain unmodified.

Do not modify tracked production/test/configuration files.

Preserve Git branch, HEAD, upstream, index, tracked modifications, and
pre-existing untracked files.

Do not stage, commit, push, reset, restore, stash, rebase, clean, or checkout.

All scripts/commands must have explicit bounds/timeouts.

Do not launch the game.

AI LOGGING

Save this task as:

AI-Logs/Prompts/2026-11-17-level7-framebuffer-off-on-analysis-prompt.md

Save the final report as:

AI-Logs/Reports/2026-11-17-level7-framebuffer-off-on-analysis-report.md

These ignored AI-Logs files are the only repository records this task may
create or modify, apart from temporary ignored analysis outputs if genuinely
needed.

Do not rewrite historical reports.

Do not update durable project documentation.

STOP CONDITION

Stop after the read-only analysis report.

Do not implement a fix.

Do not request a rerun unless a specific missing observable makes the
existing captures insufficient.

TEMPEST_LEVEL7_FRAMEBUFFER_OFF_ON_ANALYSIS