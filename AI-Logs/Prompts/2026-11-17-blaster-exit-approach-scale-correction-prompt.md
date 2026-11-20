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

Implement a bounded shared presentation-only correction for the Blaster's
local artwork scale during:

- tube ExitZoom;
- Approach after successful exit;
- the relevant ReverseZoom path.

Correct the regression in which the Blaster becomes grossly elongated during
tube exit and begins Approach oversized before contracting.

Do not change ordinary Playing Blaster presentation.

Do not change gameplay or tube/camera behavior.

Final manual acceptance remains Neal's decision.

CONTROLLING INVESTIGATION

Use this report as the controlling technical handoff:

AI-Logs/Reports/2026-11-17-blaster-exit-presentation-regression-readonly-report.md

That investigation established the exact root cause.

The current Atari-specific PlayerClawHeight branch effectively calculates:

height = 20 * nearRimScale / playerDepthScale

For direct-perspective Level 1, this becomes:

height = 20
       * (PlayerProjectedDepth + ZOffset)
       / (Position + ZOffset)

This is the reciprocal of the scale direction needed to keep the Blaster
proportional to its projected local edge.

As PlayerProjectedDepth increases during exit, the Blaster's perpendicular
artwork extent grows catastrophically.

The same Atari branch applies a related reversed ratio during Approach:

baselineScale / approachScale

This makes the Blaster begin Approach extremely oversized and contract as the
Approach depth offset returns to zero.

The investigation established:

- the Blaster anchor remains registered to the correct lane;
- the projected edge endpoints use one consistent depth;
- the tangent is normalized;
- the perpendicular is unit length;
- the tube itself remains geometrically coherent;
- the defect is the scalar applied to the Blaster's local perpendicular
  artwork extent;
- gameplay and exit simulation remain correct;
- the regression is shared across supported tube geometries;
- the regression was introduced by the Atari-specific PlayerClawHeight branch
  in commit 7fb0819cdccddbf0f3e4fbc40409023050546d9d;
- the later accepted ordinary-rim outward-orientation correction did not cause
  the exit defect.

Implement the justified correction without reopening the investigation.

CURRENT ACCEPTED BASELINE

The ordinary Blaster outward-orientation correction has been manually
accepted, closed, committed, and pushed.

Controlling closeout:

AI-Logs/Reports/2026-11-17-blaster-outward-orientation-closeout-report.md

Accepted closeout commit:

34eedf0cf996cfe660494e34bdff2a1499696068

Preserve that ordinary Playing behavior exactly.

The brightness diagnostic was removed and must remain removed.

PRIMARY CORRECTION REQUIREMENT

Replace the reversed Atari exit/Approach local-size calculation with a
presentation calculation that normalizes the current projected Blaster local
scale against a stable gameplay-rim baseline.

The correction must preserve the compact Blaster behavior that existed before
the regression while using the current Atari-presentation projector.

For the direct-perspective path, the controlling investigation identifies the
legacy relationship as equivalent to:

height = 20
       * currentProjectedScale
       / stableGameplayRimScale

where the stable gameplay-rim baseline corresponds to the normal production
calibration:

baselineRimScale = 70 / (Position + 5)

and currentProjectedScale reflects the actual current projected local scale at
the Blaster's live depth and active camera state.

Do not hardcode this Level 1 formula blindly across adapters.

Use the current presentation adapter's local Jacobian/scale authority so the
same conceptual relationship works across:

- direct-perspective levels;
- metadata-backed Level 8/9/10 projection paths;
- open and closed tubes;
- reflected or axis-specific model-frame mappings.

The intended contract is:

Blaster local artwork size follows the current projected local scale relative
to one stable normal-gameplay rim calibration.

Do not use a live near-rim scale that changes with exit K/ZOffset as the
baseline.

Do not merely swap nearScale and playerScale if that still compares two live
camera-dependent values and fails to reproduce the stable-baseline behavior.

STABLE BASELINE SEMANTICS

Establish one explicit baseline for the applicable level/presentation route.

The baseline must represent the Blaster's normal gameplay-rim presentation
under the stable production camera state used before exit/Approach mutation.

For ordinary production presentation, that includes the normal startup values:

- K = 70;
- ZOffset = 5;
- level Position as configured;
- the applicable level/presentation adapter mapping;
- no ApproachDepthOffset;
- no ApproachVerticalOffset.

Where an adapter uses ROM metadata or another model-frame Jacobian, derive the
baseline through the same adapter at its normal gameplay-rim state.

Do not use Level 1 constants for Level 8/9/10 when their active presentation
adapter supplies another correct local scale model.

Encapsulate this calculation in one maintainable renderer/presentation helper
rather than scattering per-state ratios.

PLAYING PROTECTION

Ordinary Atari-presentation Playing is accepted.

Preserve:

- fixed 20-unit Blaster body size in ordinary Playing;
- outward orientation;
- visible rim anchor;
- player edge/lane identity;
- highlighted lane;
- artwork topology and dimensions;
- spinner movement;
- firing;
- collision;
- gameplay state.

Do not route ordinary Playing through a new variable-scale path merely for
architectural uniformity.

A fixed 20-unit value during accepted ordinary Playing remains valid.

EXITZOOM REQUIREMENTS

During ExitZoom:

- use the Blaster's current projected local scale at PlayerProjectedDepth;
- normalize against the stable gameplay-rim baseline;
- preserve the current projected edge, anchor, normalized tangent, and
  perpendicular;
- preserve all six Blaster line segments;
- preserve current exit lane control;
- keep the Blaster compact and proportional to its projected edge;
- prevent the perpendicular artwork extent from growing independently of local
  projected geometry.

The corrected Blaster size may evolve during exit as projection changes, but it
must do so coherently with the projected edge/camera rather than spanning large
fractions of the viewport.

Do not clamp the Blaster to an arbitrary maximum size as the primary fix.

Do not hide the problem by skipping Blaster rendering during exit.

Do not replace the live projected anchor with a screen-space overlay.

APPROACH REQUIREMENTS

Correct the related reversed ratio during Approach.

At Approach start, the next tube and Blaster are distant.

The Blaster should begin correspondingly compact and become appropriately
larger as ApproachDepthOffset eases toward zero.

The corrected Approach path must:

- use current Approach projection/local scale;
- normalize it against the stable gameplay-rim baseline;
- remain continuous across the successful-exit handoff;
- reach exactly the accepted 20-unit Playing value when Approach completes;
- preserve spinner-controlled movement around the distant rim;
- preserve ApproachVerticalOffset and starfield behavior;
- preserve existing Approach timing and state.

Do not retune the accepted Approach duration, starfield, recharge message,
tube geometry, or camera trajectory.

REVERSEZOOM REQUIREMENTS

Apply and test the corrected scale relationship for the applicable ReverseZoom
path reached after a Spike collision on spike-exit configurations.

The ReverseZoom correction must:

- remain presentation-only;
- use the same stable-baseline scale contract;
- preserve player depth/state and reverse-zoom simulation;
- remain finite and continuous over the bounded production range;
- not introduce an oversized or vanishing Blaster.

Do not broaden this task into a general ReverseZoom redesign.

STATE BOUNDARIES

Explicitly cover and preserve:

1. ordinary Playing;
2. ExitDelay;
3. ExitZoom phase 1, where ZOffset changes;
4. ExitZoom phase 2, where K changes;
5. successful exit handoff;
6. Approach start;
7. Approach interior;
8. Approach completion;
9. bounded ReverseZoom;
10. return to Playing where applicable.

The scale function must not jump due solely to selecting another rendering
branch unless the state transition's existing projection itself justifies the
change.

Calculate and test continuity at branch boundaries.

ALL-TUBE SHARED SCOPE

The root cause is shared.

Implement one shared correction rather than level-specific fixes.

Cover representative geometry/presentation routes including at minimum:

- Level 1, simple closed tube;
- a nontrivial enclosed tube such as Level 4;
- Level 8, accepted open tube with metadata-backed presentation;
- Level 9, accepted open tube with different metadata scale;
- accepted diagnostic Level 10 after its AvoidSpikes gate;
- a bounded ReverseZoom-capable configuration.

Do not retune individual levels.

Do not modify accepted placement values.

LEVEL 7 BOUNDARY

Do not reopen Level 7's unresolved tube/entity/depth-motion architecture.

The shared ExitZoom path may reach Level 7 because its special record-06
presentation is selected only during Playing.

If the corrected shared scale function applies naturally to Level 7 exit,
protect it with bounded regression coverage without modifying Level-7-specific
geometry, registration, centering, or motion code.

Do not treat this task as Level 7 reconstruction.

ORDINARY ORIENTATION FIX

The accepted outward normal applies to ordinary Atari-presentation Playing.

Do not broaden or rewrite that accepted correction in this task.

During exit/Approach, preserve the existing orientation behavior unless the
scale correction requires a strictly mechanical helper extraction.

The current regression concerns size, not a newly authorized orientation
redesign.

If exit orientation is independently still wrong after scale correction, leave
that as a separate manual finding rather than mixing an uninvestigated
orientation change into this milestone.

GAMEPLAY AND SIMULATION PROTECTION

Do not change:

- BeginExit;
- UpdateExitZoom;
- UpdateExitSimulation;
- CompleteSuccessfulExit;
- UpdateReverseZoom;
- player depth;
- player lane;
- EdgePosition;
- spinner input;
- lane control;
- exit delay;
- exit timing;
- K/ZOffset update cadence;
- next-level selection;
- Approach timing;
- Approach offsets;
- starfield;
- score;
- lives;
- firing;
- collision;
- RNG;
- recording/playback;
- fixed-step simulation.

The renderer may read state but must not write it.

RECORDING AND PLAYBACK

Preserve gameplay recording format and deterministic playback.

Presentation selection and corrected Blaster scale must remain outside
recording metadata.

The same recording/input sequence must reproduce the same simulation before
and after the correction.

Only the rendered Blaster geometry should differ during affected presentation
states.

FROZEN PRESENTATION PROTECTION

Preserve:

- Levels 1-6 accepted presentation;
- Level 8 +212.28139;
- Level 9 +180.96825;
- Level 10 +50;
- tube geometry;
- tube projection;
- shared perspective;
- playfield camera;
- entity registration;
- HUD placement;
- accepted Approach presentation except for correcting the malformed Blaster
  local size.

Do not change global scale, perspective, camera zoom, or tube-exit geometry to
fix this Blaster-only regression.

NO IMAGE ANALYSIS

No image analysis is required or authorized for this implementation task.

Do not search for, enumerate, open, inspect, or analyze screenshots, images,
videos, frames, contact sheets, or visual assets.

The controlling investigation already established the root cause.

CODE QUALITY

Prefer one explicit helper expressing the intended scale contract, such as:

current local projected scale
divided by
stable normal-rim local projected scale

Do not preserve misleading names such as nearScale if the value actually means
a live camera-dependent baseline.

Use finite/nonzero validation appropriate to existing renderer conventions.

Do not add arbitrary aesthetic clamps.

Add concise comments explaining:

- ordinary Playing remains fixed at 20;
- exit/Approach scale is normalized against stable gameplay-rim projection;
- the correction reverses the prior erroneous ratio;
- gameplay state is not changed.

Do not narrate obvious calculations.

TESTING REQUIREMENTS

Add focused automated tests proving at minimum:

A. ROOT REGRESSION

1. The old Level 1 exit formula would reproduce the known oversized values.
2. The corrected Level 1 path remains compact over the same deterministic exit
   trajectory.
3. Exit tick/state samples corresponding to both zoom phases remain finite.
4. The corrected result tracks current projected local scale rather than its
   reciprocal.
5. Blaster anchor remains unchanged.
6. Projected player edge remains unchanged.
7. Tangent and perpendicular remain normalized/unchanged.
8. Artwork topology remains six line segments.

B. PLAYING

9. Ordinary Playing remains exactly 20 units.
10. Accepted outward orientation remains unchanged.
11. Rim anchor and highlighted lane remain unchanged.
12. Direction changes remain correct.

C. EXITZOOM

13. Early ExitZoom remains continuous with the last Playing/ExitDelay
    presentation.
14. ZOffset phase produces coherent compact scaling.
15. K phase produces coherent compact scaling.
16. Blaster-to-local-edge ratio remains bounded and appropriate.
17. No production exit sample creates nonfinite or extreme viewport-spanning
    Blaster geometry.
18. Final ExitZoom sample remains compact before handoff.

D. APPROACH

19. Initial distant Approach Blaster is compact, not oversized.
20. Blaster scale increases coherently as ApproachDepthOffset approaches zero.
21. Approach completion reaches exactly 20.
22. Spinner-controlled distant-rim movement remains registered.
23. ApproachVerticalOffset and starfield state are unchanged.
24. Successful exit-to-Approach transition remains deterministic.

E. REVERSEZOOM

25. Representative ReverseZoom values remain finite and coherent.
26. No reversed scale growth is introduced.
27. Simulation state remains unchanged.

F. SHARED LEVELS

28. Level 1 closed tube.
29. Level 4 nontrivial enclosed tube.
30. Level 8 open tube and its accepted placement.
31. Level 9 open tube and its accepted placement.
32. Level 10 diagnostic shared exit after AvoidSpikes.
33. Shared function works with direct-perspective and metadata-backed adapters.
34. No per-level exit-scale tuning exists.

G. PROTECTION

35. Gameplay state is unchanged before/after render.
36. Recording format is unchanged.
37. Playback simulation is unchanged.
38. Option-off/legacy behavior remains unchanged.
39. Levels 8/9/10 frozen placement values remain unchanged.
40. Level 7-specific architecture is unchanged.
41. Blaster brightness diagnostic remains removed.
42. Approach/exit timing remains unchanged.
43. Tube projection/camera output remains unchanged.

Use geometric and numerical assertions, not screenshot pixels.

Use the deterministic Level 1 exit cadence established by existing production
tests and the controlling report.

VALIDATION

Run bounded focused tests for:

- PlayerClawHeight/local scale;
- Playing;
- ExitDelay;
- ExitZoom phase 1;
- ExitZoom phase 2;
- Approach start/interior/end;
- ReverseZoom;
- representative shared levels;
- state preservation.

Run targeted regressions covering:

- Blaster/player rendering;
- Level 1 exit;
- Level 4 exit;
- Levels 8/9/10 presentation;
- Atari presentation adapter;
- Approach;
- transition;
- exit;
- ReverseZoom;
- recording/playback;
- display/camera;
- option-off behavior.

Then run:

- full solution test suite with explicit VSTest hang-timeout protection;
- Release solution build;
- git diff --check.

Do not weaken tests.

Canceled, stalled, blocked, timed-out, or incomplete commands are not PASS.

Do not launch the interactive game.

STARTING REPOSITORY SAFETY

Record:

- branch;
- HEAD;
- upstream/tracking;
- ahead/behind;
- index;
- tracked worktree;
- relevant untracked state.

The expected committed baseline is:

34eedf0cf996cfe660494e34bdff2a1499696068

on master tracking origin/master at 0/0.

Verify current state rather than assuming it.

Preserve unrelated local work, including if still present:

- .github/copilot-instructions.md;
- Reference/arcade/MBUCOD.V05.txt;
- Reference/arcade/MBUDOC.DOC;
- screenshot000.png.

Do not reset, restore, clean, stash, rebase, switch branches, stage, commit, or
push.

This task stops before manual acceptance and closeout.

AI-LOGS

Follow .github/copilot-instructions.md.

Save:

- this substantive prompt under AI-Logs/Prompts/;
- the paired Blaster exit/Approach scale-correction report under
  AI-Logs/Reports/.

A chat response is not a substitute for the report.

Do not stage or commit AI-Logs.

MANUAL HANDOFF

After automated validation, STOP.

Do not launch the interactive game.

Provide Neal with exact manual commands for:

1. a direct ordinary Level 1 run under --atari-presentation;
2. the available deterministic Level 1 playback/recording path if its exact
   existing command can be established without inspecting arbitrary recording
   collections;
3. representative Level 8 and Level 9 exit tests if direct starts are suitable;
4. diagnostic Level 10 only if its existing validated command is already known.

Do not invent recording filenames.

The essential manual test may use Neal's existing known playback command.

Ask Neal to judge:

- ordinary Playing Blaster remains accepted;
- Blaster stays compact throughout ExitDelay and both ExitZoom phases;
- no viewport-spanning yellow strokes;
- anchor continues following the exiting lane;
- successful handoff to Approach;
- distant Approach Blaster begins appropriately small;
- Approach Blaster grows coherently toward the next rim;
- final Playing Blaster returns to accepted size;
- representative open and closed tubes behave similarly;
- gameplay and timing remain unchanged;
- no regression to accepted Levels 8, 9, or 10 placement.

Do not ask Neal questions during execution.

Do not pause for manual input.

Manual acceptance remains PENDING.

EXPECTED REPORT

The implementation report must include:

- exact corrected formula/contract;
- exact stable-baseline definition;
- files changed;
- why simple numerator/denominator swapping was insufficient or sufficient in
  the final implementation;
- Playing behavior;
- ExitZoom phase behavior;
- Approach behavior;
- ReverseZoom behavior;
- representative before/after numerical values;
- shared-level coverage;
- state-preservation evidence;
- focused test results;
- targeted test results;
- full-suite result;
- Release build result;
- git diff --check result;
- starting/ending Git state;
- exact manual commands;
- known limitations;
- manual acceptance status PENDING;
- exact AI-Logs prompt/report paths.

Do not stage, commit, or push.

Complete autonomously and stop with the shared Blaster exit/Approach scale
correction ready for Neal's manual test.

TEMPEST_BLASTER_EXIT_APPROACH_SCALE_CORRECTION