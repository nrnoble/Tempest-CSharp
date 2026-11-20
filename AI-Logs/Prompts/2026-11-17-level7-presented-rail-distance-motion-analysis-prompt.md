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

Perform a tightly bounded READ-ONLY Level 7 Atari-presentation investigation
of entity motion relative to the ACTUAL PRESENTED DISTANCE ALONG THE ENTITY'S
SAME LANE TO THE RIM.

Do NOT modify production code, tests, configuration, diagnostics, recordings,
presentation, gameplay, geometry, projection, or Git state.

Do NOT implement a correction.

Do NOT launch the game.

Do NOT create another framebuffer capture.

PURPOSE

The latest framebuffer analysis established a measurable scale-relative
slowdown in Atari-ON submitted presentation geometry.

However, that analysis used displayed glyph width as a scale denominator.

Glyph width was meaningful evidence, but it is not the remaining presented
rail distance to the rim.

This task must determine whether the reported progressive slowdown is present
when entity advancement is measured relative to the authoritative presented
path remaining along that entity's actual lane to the rim.

This is the final missing geometric quantity identified by the latest report.

ESTABLISHED EVIDENCE

Read current project instructions and:

AI-Logs/Reports/2026-11-17-level7-framebuffer-off-on-analysis-report.md

Also read, only as needed:

AI-Logs/Reports/2026-11-17-level7-atari-on-off-deterministic-state-ab-report.md

AI-Logs/Reports/2026-11-17-level7-good-bad-runtime-ab-analysis-report.md

Use current production source only as required to understand the authoritative
Level 7 presentation geometry.

Do not reopen unrelated historical investigations.

The established evidence is:

1. The same Level-7-slowmo.json recording was played with Atari presentation
   OFF and ON.

2. The underlying simulation state is identical at all 3,511 playback
   boundaries.

3. Atari OFF played normally.

4. Atari ON visibly exhibited the known depth-motion problem.

5. In Atari ON, representative fixed-lane Tanker submitted-anchor movement
   remained approximately constant in absolute screen pixels per playback
   boundary.

6. For Tanker 15, Atari-ON submitted motion remained about 1.17 pixels per
   boundary while displayed glyph width grew from about 20.90 to 57.38
   pixels.

7. Movement divided by glyph width therefore fell from approximately 0.056
   to 0.020.

8. Actual framebuffer pixels faithfully reproduced the submitted geometry.

9. The measured scale-relative effect is therefore present in submitted
   Atari-ON presentation geometry and is not introduced by rasterization.

10. The previous report did NOT establish motion relative to the actual
    remaining same-lane rail distance to the presented rim.

MANUAL OBSERVATION

Neal's established observation remains:

- enemies visibly slow progressively while approaching the rim in Atari
  presentation;
- the Blaster continues moving around the rim in real time;
- this gives Neal progressively more real opportunity to move and shoot the
  approaching enemies;
- player shots display an inverse depth-motion tendency;
- the same recording appears normal with Atari presentation disabled.

Do not dismiss or reinterpret this observation merely because absolute anchor
movement can remain constant.

CENTRAL QUESTION

For a continuously climbing Level 7 entity on one fixed lane:

As gameplay height advances by equal simulation increments, what fraction of
the entity's PRESENTED SAME-LANE PATH TO THE RIM is traversed per tick or
playback boundary at:

- deep depth;
- middle depth;
- near-rim depth?

Then determine whether that quantity:

- remains appropriate and consistent;
- increases toward the rim;
- decreases toward the rim;
- or follows another measurable curve.

The analysis must use the actual presented lane geometry and actual presented
entity mapping.

Do not substitute glyph width for rail distance.

Do not use raw Euclidean distance to the rim unless the presented path is
actually a straight segment and Euclidean distance is therefore
authoritative.

AUTHORITATIVE PRESENTED LANE PATH

First determine how Level 7 record-06 presentation represents each lane from
deep endpoint to rim endpoint.

Establish from current source/data:

- the authoritative mapped deep endpoint;
- the authoritative mapped rim endpoint;
- whether each fixed lane is a straight presented segment;
- whether any camera/viewport transform changes distance nonuniformly along
  the lane;
- whether lane length differs by lane;
- how mappedAnchor is derived from normalized depth;
- how localPresentationScale is derived;
- whether an entity anchor lies directly on the lane segment;
- whether any later presentation transform changes lane-relative distance.

Do not infer these answers from screenshots.

Do not inspect screenshots, videos, frames, contact sheets, or other visual
assets.

Use code, data, diagnostics, and mathematical evidence.

If the presented fixed lane is straight and undergoes only an affine final
camera transform, document that.

If the path is curved or piecewise, compute distance along the actual
presented path rather than its endpoint chord.

SAME-LANE REMAINING DISTANCE

For each qualifying sample, calculate:

1. total presented path length from the lane's deep endpoint to its rim
   endpoint;

2. entity's presented distance already traveled from the deep endpoint;

3. entity's presented distance remaining to the rim;

4. adjacent-boundary presented movement along that same lane;

5. movement divided by total presented lane length;

6. movement divided by remaining presented distance to the rim, where
   mathematically meaningful;

7. change in remaining presented distance per simulation tick/playback
   boundary;

8. the relationship between those quantities and localPresentationScale.

Be precise about which denominator answers which question.

Do not treat movement divided by remaining distance as interchangeable with
movement divided by total lane length.

Do not divide by a near-zero remaining distance without an explicit safe
boundary.

PRIMARY ENTITY AND DATA

Use Tanker 15 from the synchronized same-recording captures as the initial
reference because the framebuffer report identifies it as the longest stable
trajectory across the early selected windows.

Its sampled center boundaries include:

100
150
200
250

with corresponding gameplay heights approximately:

60.75
148.25
235.75
323.25

The framebuffer report records Atari-ON normalized depths approximately:

0.8481
0.6294
0.4106
0.1919

Do not rely only on those four points if the existing full runtime trace
contains a longer continuous Tanker 15 trajectory.

Prefer the complete authoritative trajectory from the existing diagnostic
logs when available.

The synchronized framebuffer samples may remain corroborating evidence.

Do not modify existing diagnostic files.

BROADER REPRESENTATIVENESS

After analyzing Tanker 15, examine enough additional qualifying fixed-lane
Tanker trajectories to determine whether the result is:

- lane-specific;
- entity-specific;
- or representative of the Level 7 Atari-ON fixed-lane mapping.

Account for different presented rail lengths between lanes.

Use within-lane normalized quantities before aggregating across lanes.

Report the actual number of qualifying trajectories and lanes examined.

Do not impose an arbitrary large sample count.

Stop when the evidence is sufficient to classify the mapping behavior.

ATARI OFF COMPARISON

Use Atari OFF only as a controlled presentation comparison.

The same recording has identical simulation state in OFF and ON.

For equivalent qualifying trajectories, calculate the same meaningful
presented-path quantities under the OFF presentation where the authoritative
legacy projection permits it.

Do not require absolute screen coordinates or lane shapes to match across
modes.

The point is to compare how equal simulation progress is translated into
progress along each mode's own presented path.

Determine whether Atari OFF produces:

- increasing absolute progression toward the rim;
- approximately constant fractional progression;
- decreasing progression;
- or another curve.

Do not assume Atari OFF is mathematically ideal merely because Neal reports
that it looks normal.

PLAYER SHOTS

If the existing diagnostics provide sufficient complete fixed-lane
player-shot trajectories, perform the same presented-path analysis for shots
traveling from rim toward the deep end.

The goal is to test Neal's inverse observation.

For a shot, reverse the path orientation appropriately:

- distance traveled from the rim;
- distance remaining to the deep endpoint;
- movement as a fraction of the applicable presented path.

Do not let shot analysis block the primary Tanker result if suitable shot
trajectories are unavailable or ambiguous.

Clearly report whether shot evidence is sufficient.

LOCAL PRESENTATION SCALE

Compare same-lane presented-path progression against
localPresentationScale.

Determine precisely whether:

- anchor progression is affine in normalized depth;
- local scale is affine, nonlinear, or otherwise related to depth;
- constant anchor displacement combined with changing local scale creates
  the measured scale-relative effect;
- presented rail-distance progression and scale progression use compatible
  or incompatible depth parameterizations.

Do not assume that movement should be proportional to glyph scale.

The purpose is to identify the mathematical relationship, not prescribe the
fix in advance.

DISTINGUISH THREE CONCEPTS

Keep these separate throughout the report:

1. ABSOLUTE SCREEN MOTION
   Pixels traveled per tick/boundary.

2. PRESENTED PATH PROGRESS
   Fraction or distance traveled along the entity's own presented rail.

3. SCALE-RELATIVE MOTION
   Screen movement relative to displayed entity size/local scale.

The previous investigation established:

- constant Atari-ON absolute anchor motion;
- declining Atari-ON scale-relative motion.

This task must establish presented-path progress.

Do not collapse these into one concept.

CONTINUOUS CURVE

Where the full trace permits it, evaluate the continuous relationship rather
than only the four sampled framebuffer windows.

Produce a compact numerical characterization such as:

- progression by normalized-depth bands;
- fitted or exact functional relationship supported directly by current
  presentation code;
- derivative of presented distance with respect to gameplay height or
  normalized depth;
- derivative of local scale with respect to the same parameter.

Do not fit arbitrary high-order curves.

Prefer exact implementation-derived relationships when available.

Any mathematical derivation must be checked against actual diagnostic
samples.

Do not prescribe constants or corrections in this task.

CRITICAL INTERPRETATION QUESTION

Determine which statement is best supported:

A. Atari-ON entities advance normally along the presented rail, and only
   scale-relative motion changes.

B. Atari-ON entities advance abnormally along the presented rail as they
   approach the rim.

C. Both presented-rail progression and scale-relative progression contribute
   to the defect.

D. Existing evidence is insufficient to distinguish these.

Do not force A, B, or C if D is correct.

If answer A is supported, explain why Neal can nevertheless experience
progressively easier interception near the rim.

If that gameplay-time consequence cannot be explained by the measured
presentation quantities, preserve it as unresolved rather than inventing an
explanation.

NO NEW CAPTURE

Do not launch the game.

Do not generate screenshots.

Do not generate framebuffer captures.

Do not ask Neal to replay Level 7.

Use existing diagnostics, current source, and bounded mathematical analysis.

No image/frame analysis is authorized.

Do not inspect the previously captured PNGs again unless a specific numeric
fact from the existing framebuffer report cannot otherwise be interpreted.
The current task is about presented path geometry, not another pixel study.

Do not inspect TEMP/myNotes.md.

Do not inspect protected ROM copies.

DO NOT IMPLEMENT A FIX

Do not:

- change mappedAnchor;
- change localPresentationScale;
- introduce nonlinear depth transfer;
- restore the legacy projection;
- change record-06 coordinates;
- alter lane endpoints;
- alter camera transforms;
- change entity speed;
- change player-shot speed;
- change Tanker behavior;
- modify simulation;
- modify playback;
- change perspective;
- modify the accepted Level 7 tube presentation;
- modify the mapped-Flipper crash correction.

If the exact defect mechanism becomes established, report the narrowest
candidate correction boundary and the evidence it would need.

Do not implement it.

BOUNDED ANALYSIS

The existing diagnostic files are large.

Use bounded streaming/incremental analysis.

Do not dump complete JSONL files into terminal output or agent context.

Gather all needed aggregates in as few passes as practical.

All scripts and commands require explicit timeouts.

Temporary analysis scripts/data must remain outside tracked source or under
ignored TEMP.

Do not modify original diagnostics.

VALIDATION AND PRESERVATION

Verify:

- original diagnostics remain byte-for-byte unchanged;
- no production/test/configuration source is modified;
- no visual capture is created;
- no game is launched;
- Git branch, HEAD, upstream, index, tracked modifications, and pre-existing
  untracked state remain preserved.

Do not stage, commit, push, reset, restore, stash, rebase, clean, checkout, or
otherwise rewrite Git state.

AI LOGGING

Save this task as:

AI-Logs/Prompts/2026-11-17-level7-presented-rail-distance-motion-analysis-prompt.md

Save the report as:

AI-Logs/Reports/2026-11-17-level7-presented-rail-distance-motion-analysis-report.md

These ignored AI-Logs records are the only repository files this task may
create or modify, apart from temporary ignored analysis output if genuinely
required.

Do not rewrite historical reports.

Do not update durable project documentation.

REPORT REQUIRED ANSWERS

The final report must directly answer:

1. What is the authoritative Atari-ON presented path for a fixed Level 7
   lane?

2. Is that path straight, curved, or piecewise?

3. What final transforms affect distance along it?

4. How is mappedAnchor related to normalized depth?

5. How is localPresentationScale related to normalized depth?

6. For Tanker 15, what are:
   - absolute motion;
   - fraction of total presented lane traversed;
   - distance remaining to the rim;
   - movement relative to remaining distance;
   - scale-relative motion;
   across deep, middle, and near-rim regions?

7. Do additional Tanker trajectories and lanes show the same relationship?

8. What does Atari OFF show for equivalent simulation progress?

9. Do player shots provide sufficient inverse-path evidence?

10. Does the presented-rail-distance analysis classify the mechanism as
    A, B, C, or D?

11. What exact mathematical/presentation mechanism is established?

12. What remains unresolved?

13. Is there now enough evidence for a narrowly specified correction
    candidate, or is another investigation required?

14. What is the smallest justified next step?

Stop after the read-only report.

TEMPEST_LEVEL7_PRESENTED_RAIL_DISTANCE_MOTION_ANALYSIS