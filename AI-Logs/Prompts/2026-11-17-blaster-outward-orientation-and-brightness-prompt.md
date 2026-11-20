READ THIS ENTIRE USER-PROVIDED TASK BEFORE TAKING ANY ACTION.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, reading project files, running commands, inspecting images,
or taking any repository action:

1. Locate the unique completion token near the end of this user-provided task.
2. Verify that exactly one complete token beginning with "TEMPEST_" appears on
   a line by itself in the user-provided task.
3. VS Code/agent-generated reminders, tagged-file information, attachment
   metadata, or other system-generated context may appear after the token and
   do not invalidate it.
4. This check detects an incomplete/truncated user task, not appended agent
   context.
5. If exactly one complete TEMPEST_ token cannot be found, STOP before project
   or repository action and explain why.

TASK

Implement a bounded presentation correction for the Blaster under
--atari-presentation:

1. Correct the Blaster artwork orientation so the Blaster travels around the
   rim on the OUTSIDE of the visible tube rather than extending inward over
   the tube surface.

2. Add a development-only runtime option that allows Neal to test the
   Blaster's yellow color brightness/intensity using a multiplier:

   1 = current Blaster brightness
   2 = twice the current Blaster brightness
   intermediate floating-point values such as 1.25, 1.5, and 1.75 must be
   accepted.

Do not select a final brightness value in this task.

Do not change gameplay, tube geometry, entity positions, lane movement, input,
collision, firing, timing, recording/playback, or accepted level placement.

CURRENT MANUAL EVIDENCE

The current --atari-presentation Blaster is visually attached to the rim, but
its triangular body extends toward the inside of the tube. It should extend
outward from the visible rim.

The screenshot supplied with this task shows the issue clearly on displayed
Level 1:

playback-level-1-boundary-000033.png

The yellow Blaster is on the left side of the outer rim. Its body points into
the tube rather than away from the tube.

The Blaster's yellow color also needs greater brightness/intensity. Neal expects
the preferred multiplier will likely be between 1 and 2, but manual testing is
required.

Treat these as manual observations.

AUTHORIZED IMAGE SCOPE

You MAY inspect only:

playback-level-1-boundary-000033.png

Locate only that exact filename if its path is not immediately available.

Do not enumerate, open, inspect, or analyze other screenshots, images, videos,
frames, contact sheets, or visual assets.

Use the screenshot only to understand the reported orientation and intensity
symptoms. Establish the correction from the renderer's geometry and color
paths.

PRIMARY ORIENTATION REQUIREMENT

Trace how the Blaster's rendered anchor and local basis are calculated under
--atari-presentation.

Determine:

- the two visible rim-edge endpoints;
- the rim tangent;
- the perpendicular/normal used to construct the Blaster body;
- which side of the rim is the visible tube interior;
- which side is exterior;
- how open and closed tubes affect the determination;
- whether polygon winding varies by level;
- whether reflected geometry or axis conversion reverses the normal;
- whether Blaster movement direction affects artwork orientation;
- whether the highlighted lane or player edge is involved;
- and whether the current implementation simply selects the wrong normal sign.

Correct the geometry so the Blaster artwork extends outside the visible tube
for all supported --atari-presentation levels.

Do not fix this with a level-specific hardcoded sign unless current evidence
proves that a level-specific exception is necessary.

Prefer deriving the outward normal from actual visible rim geometry and tube
interior geometry.

OUTWARD-NORMAL CONTRACT

Define and test an explicit geometric contract.

For a Blaster anchor on visible rim edge i:

- the tangent follows the visible rim edge;
- the tube-interior side is determined from the visible tube geometry;
- the Blaster body extends along the opposite, outward side;
- reversing player travel direction must not move the body inside the tube;
- the artwork may reflect or rotate appropriately for movement/orientation,
  but its main radial extent must remain outside.

For closed tubes, use a robust interior reference consistent with the actual
visible near-rim geometry.

For open tubes, do not assume a closed-polygon centroid test is automatically
valid. Determine an appropriate interior-side reference from the tube's visible
near/far rail relationship or existing presentation geometry.

Do not invent replacement Blaster geometry.

Do not change the Blaster's rim anchor.

Do not move the Blaster around the rim.

Do not change edge/lane indexing.

SCOPE ACROSS LEVELS

Investigate the common --atari-presentation Blaster renderer used by supported
production Levels 1-9 and the accepted Level 10 diagnostic where applicable.

The correction should be shared when the defect shares one render-basis cause.

Verify at minimum:

- a closed tube;
- an open tube;
- geometry with reflected/winding differences;
- Levels 1, 8, and 9;
- accepted Level 10 if that path uses the same Blaster artwork renderer.

Do not alter accepted tube placement or presentation values for any level.

LEVEL 7 BOUNDARY

Level 7 remains a separate unresolved reconstruction milestone.

Do not reopen Level 7 record geometry, entity registration, depth motion,
centering, or other presentation work.

If the Blaster outward-normal correction naturally applies through a shared
Blaster rendering seam and can be proven geometrically correct for Level 7,
retain coverage without modifying Level-7-specific architecture.

Do not let unrelated Level 7 problems broaden this task.

BLASTER BRIGHTNESS DIAGNOSTIC

Add one narrowly scoped development-only command-line option for Blaster
brightness/intensity.

Use a concise name consistent with current diagnostic CLI conventions.
A suitable name is:

--blaster-brightness <multiplier>

unless the existing option architecture establishes a more consistent name.

Required behavior:

- omitted option = multiplier 1.0;
- 1.0 = exactly current Blaster color/intensity;
- 2.0 = twice the current Blaster channel intensity before channel saturation;
- fractional values are supported;
- values such as 1.25, 1.5, and 1.75 are valid;
- malformed values are rejected clearly;
- NaN and positive/negative infinity are rejected;
- zero and negative values are rejected;
- use invariant-culture parsing consistent with other numeric diagnostics.

Do not impose an arbitrary narrow aesthetic upper limit solely because the
expected manual range is 1-2.

If the rendering/color representation has a finite meaningful maximum, handle
overflow by safe final color-channel saturation rather than integer wrapping.

Do not weaken safety or permit invalid numeric state.

BRIGHTNESS APPLICATION SCOPE

The multiplier must affect only the Blaster's rendered artwork color/intensity.

It must not alter:

- tube blue;
- highlighted player lane;
- HUD text;
- life glyphs;
- player shots;
- enemy shots;
- enemies;
- explosions/effects;
- Super Zapper visuals;
- transition stars;
- diagnostic overlays;
- background;
- gameplay state.

If the Blaster uses multiple yellow shades or alpha/intensity channels, apply
one coherent multiplier while preserving their relative proportions unless the
current renderer establishes a different required treatment.

Preserve alpha unless the renderer's intensity model explicitly requires alpha
scaling. Do not make the Blaster more opaque as a substitute for increasing
RGB/intensity.

Do not alter Blaster size, line thickness, or geometry as part of brightness
testing.

PRESENTATION-MODE BOUNDARY

The reported issue concerns --atari-presentation.

Prefer making the brightness diagnostic effective for the Blaster under
--atari-presentation only, unless the existing shared rendering architecture
makes presentation-independent application materially simpler and harmless.

If the option is supplied outside a supported presentation context, either
reject it explicitly or document and test the intentionally shared behavior.
Do not silently create ambiguous behavior.

The orientation correction should apply at the correct shared presentation
seam determined by the code trace.

RECORDING AND PLAYBACK

The Blaster brightness diagnostic is rendering-only.

Do not add it to gameplay recording metadata or recorded input.

Do not change recording format or playback determinism.

Playback with different Blaster-brightness values must reproduce identical
simulation and inputs while changing only displayed Blaster intensity.

The outward-orientation correction must also be render-only.

GAMEPLAY PROTECTION

Do not change:

- player edge position;
- spinner input;
- lane movement;
- movement speed;
- firing;
- shot slots;
- collisions;
- deaths;
- lives;
- score;
- Super Zapper;
- enemy state;
- timing;
- RNG;
- fixed-step simulation;
- recording/playback state.

Do not mutate gameplay state during rendering.

ACCEPTED PRESENTATION PROTECTION

Preserve all accepted/frozen presentation work, including:

- Levels 1-6;
- Level 8 at +212.28139 logical Y;
- Level 9 at +180.96825 logical Y;
- Level 10 at +50 logical Y;
- accepted tube geometry, scale, perspective, and placement.

Do not alter Level 8, Level 9, or Level 10 centering constants.

Do not begin other minor bug work.

Do not address the separate Blaster tube-exit defect unless the reported
outside/inside inversion is proven to be the same shared normal-sign cause.
Ordinary Playing is the required scope.

If exit rendering requires separate work, leave it unchanged and report that
boundary.

CODE QUALITY

Keep the correction small and maintainable.

Do not scatter brightness multiplication across multiple draw calls if one
Blaster color-construction seam can own it.

Do not scatter outward-normal sign rules across levels if one geometry helper
can establish exterior consistently.

Add concise comments only where they preserve important reasoning:

- how the outward side is determined;
- why the diagnostic is rendering-only;
- multiplier 1.0 preserves current intensity.

TESTING REQUIREMENTS

Add focused automated coverage proving at minimum:

A. ORIENTATION

1. The Blaster anchor remains on the same visible rim edge.
2. The Blaster body extends to the exterior side of a closed tube.
3. The Blaster body extends to the exterior side of an open tube.
4. Movement direction reversal does not invert the body into the tube.
5. Relevant reflected/winding geometry still produces an outward body.
6. Levels 1, 8, and 9 use the correct outward side.
7. Accepted Level 10 remains geometrically correct if it shares this path.
8. The highlighted lane remains unchanged.
9. Blaster dimensions and artwork topology remain unchanged.
10. Gameplay/model state is unchanged before and after rendering.

B. BRIGHTNESS PARSING

11. Omitted option produces exactly 1.0.
12. 1, 1.0, 1.25, 1.5, 1.75, and 2 parse correctly.
13. Malformed text is rejected.
14. NaN is rejected.
15. Positive and negative infinity are rejected.
16. Zero is rejected.
17. Negative values are rejected.
18. Parsing is culture-stable.

C. BRIGHTNESS RENDERING

19. Multiplier 1.0 reproduces the current Blaster color exactly.
20. Multiplier 1.5 scales applicable Blaster channels correctly.
21. Multiplier 2.0 doubles nonsaturated applicable channels.
22. Final channels saturate safely rather than wrapping.
23. Alpha remains unchanged unless existing renderer evidence requires
    otherwise.
24. Tube, lane highlight, HUD, life glyph, shots, enemies, and effects do not
    change.
25. Blaster geometry does not change with brightness.
26. Simulation does not change with brightness.

D. PROTECTION

27. Recording format is unchanged.
28. Playback simulation is unchanged.
29. Option-off behavior is unchanged.
30. Accepted Levels 8, 9, and 10 placement constants remain unchanged.
31. Existing Level 1-10 presentation-selection tests pass.
32. Approach/transition/exit behavior remains unchanged unless the ordinary
    Playing Blaster helper is intentionally shared and proven safe.
33. Level 7-specific unresolved work is not modified.

Do not use screenshot pixels as automated test expectations.

VALIDATION

Run bounded focused tests for:

- Blaster rendering geometry;
- outward-normal selection;
- numeric option parsing;
- color/intensity multiplication;
- render-state preservation.

Run relevant targeted regressions covering:

- Levels 1-10 Atari presentation;
- Blaster/player rendering;
- open and closed tube geometry;
- display/camera;
- recording/playback;
- Approach/transition/exit;
- option-off behavior;
- accepted Level 8/9/10 placement.

Then run:

- full solution tests with explicit VSTest hang-timeout protection;
- Release solution build;
- git diff --check.

Do not weaken valid tests.

Canceled, stalled, timed-out, blocked, or incomplete commands are not PASS.

Do not launch the interactive game.

STARTING REPOSITORY SAFETY

Inspect and record:

- branch;
- HEAD;
- upstream/tracking;
- ahead/behind;
- index;
- tracked worktree state;
- relevant untracked state.

The Level 9 closeout report identifies the current committed HEAD as:

bcd79da63021821c4e31a2a646152a70ccc443e5

Verify current state rather than assuming it.

Preserve the unrelated local modification to:

.github/copilot-instructions.md

and the untracked local/reference files if still present:

Reference/arcade/MBUCOD.V05.txt
Reference/arcade/MBUDOC.DOC
screenshot000.png

Do not reset, restore, clean, stash, rebase, switch branches, stage, commit, or
push.

This task stops before closeout and manual acceptance.

IMAGE POLICY

The one supplied screenshot is authorized for understanding the reported
ordinary-playing Blaster issue:

playback-level-1-boundary-000033.png

Do not inspect any other image.

Do not fit geometry or brightness constants from screenshot pixels.

Do not select a final multiplier from the screenshot.

Manual testing will determine final intensity.

AI-LOGS

Follow .github/copilot-instructions.md.

Save:

- this substantive prompt under AI-Logs/Prompts/;
- the paired Blaster orientation/brightness implementation report under
  AI-Logs/Reports/.

A chat response is not a substitute for the report.

Do not stage or commit AI-Logs.

MANUAL HANDOFF

After automated validation, STOP.

Do not launch the interactive game.

Provide exact manual commands for ordinary Level 1 testing under
--atari-presentation at these values:

- default / omitted multiplier;
- 1.0;
- 1.25;
- 1.5;
- 1.75;
- 2.0.

Use the current validated CLI syntax.

Clearly identify:

- 1.0 as the current-brightness control;
- higher values as visual diagnostic experiments;
- the orientation correction as active in all test commands;
- final brightness acceptance as PENDING.

Ask Neal to judge manually:

- Blaster body remains outside the rim throughout movement;
- no inversion as movement direction changes;
- anchor remains on the rim;
- highlighted lane remains correct;
- preferred intensity value;
- no color clipping or unwanted color shift;
- no gameplay regression.

Do not ask Neal questions during execution.

Do not pause for manual input.

EXPECTED REPORT

The paired report must include:

- root cause of inward Blaster orientation;
- exact geometry correction;
- closed/open-tube exterior determination;
- files changed;
- exact diagnostic option syntax;
- validation rules and parser behavior;
- exact color scaling/saturation behavior;
- whether alpha changes;
- affected and unaffected visual elements;
- recording/playback behavior;
- focused test results;
- targeted test results;
- full-suite result;
- Release build;
- git diff --check;
- starting/ending Git state;
- exact manual commands;
- known limitations;
- orientation acceptance PENDING;
- final brightness value PENDING;
- exact AI-Logs prompt/report paths.

Do not stage, commit, or push.

Complete autonomously and stop with the candidate ready for Neal's manual
orientation and brightness testing.

TEMPEST_BLASTER_OUTWARD_ORIENTATION_AND_BRIGHTNESS_DIAGNOSTIC