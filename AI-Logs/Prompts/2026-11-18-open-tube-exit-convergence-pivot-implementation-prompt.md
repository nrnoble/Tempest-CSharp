READ THIS ENTIRE USER-PROVIDED TASK BEFORE TAKING ANY ACTION.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, reading project files, running commands, or taking any
repository action:

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

Refine the existing DEVELOPMENT-ONLY ExitZoom focal-compensation candidate for
open tubes.

The current candidate successfully fixed Level 5, but manual testing found
that Levels 8, 9, and diagnostic Level 10 remain wrong.

The current open-tube implementation latches the midpoint between the two
terminal far vertices. Replace that open-tube pivot definition with the common
intersection of the mapped near-to-far tube rails.

Keep the existing closed-tube rule unchanged.

Specifically:

- CLOSED tubes continue using the mapped far-rim center.
- OPEN tubes use the common intersection of all mapped near-to-far rails.
- The existing presentation-space latch and compensation mechanism remains:
  latchedPivotAtExitStart - currentMappedPivot.
- Do not introduce per-level focal constants.
- Do not change accepted ordinary Playing presentation.
- Do not change gameplay or exit simulation.
- Do not stage, commit, or push.
- Stop after automated validation and provide manual test commands.

Manual acceptance remains PENDING.

CONTROLLING INVESTIGATION

Use this report as the controlling technical handoff:

AI-Logs/Reports/2026-11-18-open-tube-exit-pivot-investigation-report.md

That investigation established:

1. The terminal-endpoint midpoint is not the correct open-tube focal point.

2. The terminal midpoint is the midpoint of the omitted closing chord between
   P[0] and P[15]. The chord is not a visible lane or far-rim segment, and no
   Atari evidence assigns its midpoint a focal role.

3. The Level 8/9/10 mapped tube geometry does not distort during ExitZoom.
   Near and far point sets undergo exact uniform similarity scaling at each
   sample.

4. Translation compensation is therefore mathematically sufficient. No
   anisotropic scaling, per-point movement, geometry replacement, or
   projection redesign is indicated.

5. All 16 mapped near-to-far rails are concurrent for Levels 8/9/10. Their
   common intersection is the actual mapped perspective-convergence point.

6. The mapped rail-fit residuals at ExitZoom onset are effectively numerical
   zero.

7. Each open level has a different numeric convergence coordinate, but all use
   the same geometric rule. Do not hand-enter those coordinates.

8. Level 5 is manually fixed using the existing closed far-rim-center rule.
   Preserve Level 5 exactly.

9. The rail-convergence rule is an equation-derived PROJECT TRANSLATION. It is
   not established as literal Atari hardware or ROM behavior.

Do not repeat the investigation.

CURRENT CANDIDATE

The current uncommitted candidate is described in:

AI-Logs/Reports/2026-11-17-exitzoom-focal-compensation-implementation-report.md

It currently:

- latches a mapped focal point at actual ExitZoom onset;
- recomputes the corresponding point as K/ZOffset evolve;
- applies one logical-space compensation translation;
- applies that translation after the active presentation adapter and accepted
  per-level placement;
- applies it before the shared portrait host camera;
- moves the tube and tube-space gameplay objects together;
- remains inactive during ordinary Playing;
- does not change simulation or recording state.

Preserve this architecture.

Only replace the open-tube pivot calculation.

MANUAL RESULT TO PRESERVE

Neal manually tested the current candidate and reports:

- Level 5 is fixed.
- Levels 8, 9, and diagnostic Level 10 are not fixed.
- Levels 8/9/10 still exhibit the same exit-perspective problem, but around a
  different apparent focal location.
- Ordinary Playing presentation remains good.
- Gameplay remains normal.

Treat this as authoritative MANUAL CANDIDATE EVIDENCE.

Do not alter the successful Level 5 result.

OPEN-TUBE PIVOT

For an open tube:

1. Map every source vertex at the near tube depth through the active
   presentation adapter and accepted placement.

2. Map the corresponding source vertex at the far tube depth through the same
   route.

3. Form one line for each corresponding mapped near/far pair.

4. Calculate the common intersection of those mapped rail lines.

5. Use that mapped intersection as the open-tube ExitZoom pivot.

At ExitZoom onset, latch that convergence point.

During later ExitZoom samples, recompute the convergence point from the live
mapped near/far rails.

Apply:

compensation = latchedConvergence - currentConvergence

Apply the compensation through the existing shared logical presentation seam.

Do not use:

- terminal midpoint;
- all-vertex centroid;
- far-envelope center;
- length-weighted polyline center;
- adapter homothety origin;
- hand-entered Level 8/9/10 coordinates;
- screenshot-derived coordinates.

CLOSED-TUBE RULE

For closed tubes, preserve the current mapped far-rim-center rule exactly.

Do not change:

- Level 1;
- Level 2;
- Level 3;
- Level 4;
- manually fixed Level 5;
- Level 6.

The shared helper may branch on the existing open/closed topology flag.

Do not add per-level conditionals.

RAIL-INTERSECTION ALGORITHM

Use a bounded, deterministic, maintainable rail-intersection calculation.

The implementation must:

- consume mapped logical presentation coordinates;
- use all valid near-to-far rail lines;
- produce a finite logical-space point;
- tolerate ordinary floating-point error;
- verify that the rail set is sufficiently concurrent;
- fail explicitly for malformed or geometrically inconsistent input;
- avoid selecting one arbitrary pair of rails when all rails are available.

A least-squares common-line intersection is acceptable if it is implemented
clearly and tested against the known concurrent geometry.

Do not silently accept a large residual.

Define and test a reasonable numerical tolerance based on the mapped geometry
and floating-point precision, not visual preference.

Do not add fallback per-level coordinates.

If valid production Levels 8/9/10 do not satisfy the required residual after
the actual implementation is traced, STOP and report the discrepancy rather
than hiding it.

LINE DEGENERACY

Handle invalid rail input explicitly.

Reject or safely exclude only genuinely degenerate lines whose mapped near and
far points coincide within an established numerical tolerance.

Production Levels 8/9/10 should provide enough valid lines to determine the
intersection.

Require sufficient independent line directions to solve the intersection.

Do not return `(0,0)` or the terminal midpoint as a silent fallback.

COORDINATE SPACE

Calculate and store the convergence point in logical presentation space:

- after the active Atari presentation adapter;
- after the accepted level-specific Playing placement;
- before the shared portrait host camera;
- before applying ExitZoom focal compensation.

Use the same space for:

- latched convergence;
- current convergence;
- compensation subtraction;
- compensated world-point rendering.

Do not mix host pixels with logical presentation coordinates.

Do not apply the final portrait camera inside the convergence helper.

LEVEL 8 REQUIREMENTS

Level 8 uses:

- open shape-7 geometry;
- 16 vertices;
- 15 visible/playable segments;
- accepted +212.28139 logical Y placement.

Preserve all of those values.

At normal ExitZoom onset, the investigation calculated rail convergence near:

logical (400, 369.100)

This value is a test oracle derived from the current equations, not a constant
to embed in production code.

The production implementation must calculate the point from mapped rails.

During compensated ExitZoom, that calculated convergence point must remain at
its latched onset coordinate while all tube geometry expands uniformly around
it.

LEVEL 9 REQUIREMENTS

Level 9 uses:

- open shape-13 geometry;
- 16 vertices;
- 15 visible/playable segments;
- accepted +180.96825 logical Y placement.

Preserve all of those values.

At normal ExitZoom onset, the investigation calculated rail convergence near:

logical (400, 292.332)

Use this only as a test expectation.

Do not embed it in production code.

LEVEL 10 REQUIREMENTS

Diagnostic Level 10 uses:

- open geometry;
- 16 vertices;
- 15 visible/playable segments;
- accepted +50 logical Y placement.

At normal ExitZoom onset, the investigation calculated rail convergence near:

logical (400, 538.636)

Use this only as a test expectation.

Do not add a Level-10-specific pivot.

Level 10 remains diagnostic-only.

LEVEL 5 PROTECTION

Level 5 is manually fixed by the current candidate.

Keep:

- closed far-rim-center pivot;
- Level 5 ordinary Playing adapter;
- .67 horizontal presentation gain;
- .88 vertical presentation gain;
- Level 5 placement;
- AvoidSpikes behavior;
- existing ExitZoom compensation behavior.

The revised open-tube branch must not affect Level 5.

LEVEL 1-4 AND LEVEL 6 PROTECTION

Preserve the existing closed-tube focal rule and current candidate behavior.

Do not retune or special-case working controls.

Verify their ExitZoom onset coordinates remain continuous with ordinary
Playing and ExitDelay.

LEVEL 7 BOUNDARY

Do not reconstruct or repair Level 7.

Do not change:

- record-06 Playing geometry;
- Level 7 entity registration;
- Level 7 centering;
- Level 7 depth-motion behavior;
- Level 7 presentation adapters.

If Level 7 is closed under the current definition, it should continue through
the existing closed-tube branch without Level-7-specific changes.

Level 7 manual ExitZoom acceptance is not part of this milestone.

FAR/NEAR GEOMETRY

Use the complete mapped near and far endpoint sets already used to render the
tube.

Do not duplicate geometry or reconstruct raw ROM coordinates inside the focal
helper.

The rail set must use the exact same mapped points as the rendered tube.

This prevents the focal calculation from drifting away from visible geometry.

GLOBAL COMPENSATION

Continue applying one shared compensation vector to the complete tube-space
ExitZoom presentation.

The same compensation must move:

- near rim;
- far geometry;
- rails;
- Blaster anchor and artwork;
- Spikes;
- tube-space effects;
- other playfield objects rendered through the shared world-point path.

Do not compensate only the blue tube.

Do not compensate HUD or screen-space overlays.

Do not double-compensate the Blaster.

BLASTER PROTECTION

Preserve the accepted Blaster behavior:

- ordinary outward rim orientation;
- fixed ordinary Playing size;
- accepted ExitZoom/Approach scale normalization;
- lane attachment;
- artwork topology;
- current yellow color;
- removed brightness option.

Changing the open-tube pivot must translate the complete Blaster geometry
without changing its local size or orientation.

APPROACH BOUNDARY

The focal compensation remains ExitZoom-only.

Clear it at the existing successful-exit handoff.

Do not apply the open convergence pivot during Approach.

Preserve the accepted Approach Blaster scale and presentation.

Verify the handoff does not retain stale compensation.

RETRY AND RESET LIFECYCLE

Preserve the current candidate's latch lifecycle.

Ensure the convergence latch is cleared or replaced on:

- successful exit;
- failed/fatal exit;
- ReverseZoom or restored full-tube state as applicable;
- retry;
- fresh level;
- game reset;
- renderer game replacement.

Do not reuse a Level 8 convergence point after advancing to Level 9.

GAMEPLAY AND DETERMINISM

Do not change:

- level state;
- ExitDelay;
- AvoidSpikes;
- player depth;
- lane movement;
- spinner input;
- K/ZOffset cadence;
- collision;
- Spike behavior;
- exit threshold;
- progression;
- score;
- lives;
- RNG;
- fixed-step simulation;
- recording format;
- playback input.

The convergence latch is render/presentation state only.

Do not serialize it.

FROZEN PRESENTATION

Preserve:

- Levels 1-6 ordinary Playing presentation;
- Level 8 +212.28139;
- Level 9 +180.96825;
- Level 10 +50;
- tube geometry;
- projection/perspective;
- entity registration;
- HUD placement;
- ordinary Blaster presentation;
- accepted Blaster exit scale.

Do not change global projection or camera values.

NO IMAGE ANALYSIS

No image analysis is required or authorized.

Do not inspect screenshots, images, videos, frames, contact sheets, or visual
assets.

The controlling investigation provides the required geometry and equations.

CODE QUALITY

Refine the existing `ExitZoomFocalCompensation` rather than creating another
parallel compensation system.

Prefer small helpers with explicit responsibilities:

- select closed/open rule;
- map tube endpoint sets;
- calculate closed far-rim center;
- calculate open rail convergence;
- validate convergence residual;
- manage latch lifecycle;
- return compensation.

Add concise comments explaining:

- closed tubes retain far-rim-center behavior;
- open tubes use mapped rail convergence;
- terminal midpoint was not a valid perspective focus;
- calculations are presentation-only.

Do not narrate obvious code.

TESTING REQUIREMENTS

Add or update focused tests proving at minimum:

A. OPEN CONVERGENCE

1. Level 8 mapped rails are concurrent.
2. Level 9 mapped rails are concurrent.
3. Level 10 mapped rails are concurrent.
4. Calculated intersection matches the expected onset coordinate for Level 8.
5. Calculated intersection matches the expected onset coordinate for Level 9.
6. Calculated intersection matches the expected onset coordinate for Level 10.
7. No production constant contains those coordinates.
8. Rail-fit residual remains below the justified tolerance.
9. Terminal midpoint differs from rail convergence.
10. All 16 rails participate where valid.

B. EXITZOOM INVARIANT

11. Level 8 convergence remains fixed during ZOffset phase.
12. Level 8 convergence remains fixed during K-growth phase.
13. Level 9 convergence remains fixed in both phases.
14. Level 10 convergence remains fixed in both phases.
15. Near and far point sets expand uniformly around the fixed convergence.
16. No shape distortion is introduced.
17. Output remains finite through late ExitZoom.

C. CLOSED-TUBE PROTECTION

18. Closed tubes retain mapped far-rim-center rule.
19. Level 5 compensation remains byte/numerically equivalent where practical.
20. Levels 1-4 and 6 retain current candidate behavior.
21. No open convergence routine is used for closed tubes.

D. REGISTRATION

22. Tube and Blaster receive the same compensation.
23. Spikes and tube-space effects remain registered.
24. Blaster local scale is unchanged.
25. Blaster local orientation is unchanged.
26. No double translation occurs.
27. HUD/screen-space content remains uncompensated.

E. LIFECYCLE

28. AvoidSpikes does not capture early.
29. ExitDelay does not capture early.
30. Latch occurs at actual ExitZoom onset.
31. One attempt reuses one latched convergence point.
32. Successful handoff clears it.
33. Fatal/canceled exit clears it as required.
34. Retry captures a fresh value.
35. Level change cannot reuse the prior level's convergence point.
36. Game reset/renderer replacement clears it.

F. PROTECTION

37. Ordinary Playing coordinates are unchanged.
38. Accepted Level 8 placement remains +212.28139.
39. Accepted Level 9 placement remains +180.96825.
40. Accepted Level 10 placement remains +50.
41. Gameplay state is unchanged by rendering.
42. Deterministic simulation is unchanged.
43. Recording/playback schema is unchanged.
44. Option-off and legacy behavior remain unchanged.
45. Approach remains unchanged.
46. Accepted Blaster-scale correction remains unchanged.
47. Level 7-specific architecture remains unchanged.

Use exact mathematical/geometric assertions.

Do not use screenshot pixels.

VALIDATION

Run bounded focused tests for:

- open rail-convergence calculation;
- convergence residual;
- Level 8/9/10 trajectories;
- closed Level 5 preservation;
- latch lifecycle;
- shared tube/Blaster compensation;
- state preservation.

Run targeted regressions covering:

- ExitZoomFocalCompensation;
- ExitZoom;
- AvoidSpikes;
- Spike exit collision/retry;
- Blaster scale and presentation;
- Levels 1-6;
- Levels 8/9/10;
- AtariPresentationAdapter;
- VectorRenderer;
- Approach/transition;
- recording/playback;
- display/camera;
- option-off/legacy.

Then run:

- full solution tests with explicit VSTest hang-timeout protection;
- Release solution build;
- git diff --check.

Do not weaken tests.

Canceled, stalled, blocked, timed-out, or incomplete commands are not PASS.

Do not launch the interactive game.

REPOSITORY SAFETY

Record:

- branch;
- HEAD;
- upstream/tracking;
- ahead/behind;
- index;
- tracked worktree;
- relevant untracked state.

Expected committed baseline:

753728e79ed86624c9b1728dfd1c8db8f49d1e2b

The current focal-compensation candidate is expected to remain uncommitted on
top of this baseline.

Preserve:

- current `VectorRenderer.cs` focal candidate changes;
- current `ExitZoomFocalCompensation.cs`;
- current `ExitZoomFocalCompensationTests.cs`;
- unrelated `.github/copilot-instructions.md`;
- local reference files.

Do not attribute prior candidate changes to this task.

Do not reset, restore, clean, stash, rebase, switch branches, stage, commit, or
push.

This task stops before manual acceptance.

AI-LOGS

Follow `.github/copilot-instructions.md`.

Save:

- this substantive prompt under `AI-Logs/Prompts/`;
- the paired open-tube convergence-pivot implementation report under
  `AI-Logs/Reports/`.

Do not stage or commit `AI-Logs`.

A chat response is not a substitute for the report.

MANUAL HANDOFF

After automated validation, STOP.

Do not launch the game.

Provide exact manual commands for:

- Level 5, to confirm the closed-tube fix remains correct;
- Level 8;
- Level 9;
- optional diagnostic Level 10.

Use the established syntax:

dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation --level 5

dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation --level 8

dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation --level 9

dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation --level 10

Ask Neal to judge:

- Level 5 remains fixed;
- Level 8 ExitZoom expands around the visible rail-convergence point;
- Level 9 does the same;
- optional Level 10 does the same;
- open-tube perspective no longer shifts to another apparent focal point;
- tube and Blaster remain registered;
- Blaster remains compact;
- ordinary Playing placement remains unchanged;
- Approach handoff remains coherent;
- gameplay and timing remain normal.

Manual acceptance remains PENDING.

EXPECTED REPORT

The paired report must include:

- exact open-tube convergence algorithm;
- residual/tolerance contract;
- files changed;
- confirmation closed-tube rule is unchanged;
- Level 5 preservation;
- Level 8 onset convergence and compensated trajectory;
- Level 9 onset convergence and compensated trajectory;
- Level 10 onset convergence and compensated trajectory;
- terminal midpoint versus convergence distinction;
- shared registration evidence;
- Blaster-scale preservation;
- lifecycle behavior;
- gameplay/determinism preservation;
- focused validation;
- targeted validation;
- full-suite validation;
- Release build;
- `git diff --check`;
- starting and ending Git state;
- manual commands;
- known limitations;
- manual acceptance status PENDING;
- exact AI-Logs prompt/report paths.

Do not stage, commit, or push.

Complete autonomously and stop with the revised open-tube ExitZoom convergence
candidate ready for Neal's manual testing.

TEMPEST_OPEN_TUBE_EXIT_CONVERGENCE_PIVOT_IMPLEMENTATION