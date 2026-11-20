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

Implement a bounded DEVELOPMENT-ONLY presentation correction candidate for the
ExitZoom focal-point regression under --atari-presentation.

The candidate must make ExitZoom expand around the visible mapped far/deep tube
opening rather than allowing the opening to drift dramatically during zoom.

This is presentation-only.

Do not change exit simulation.

Do not change ordinary Playing placement.

Do not retune accepted per-level presentation values.

Final manual acceptance remains Neal's decision.

CONTROLLING INVESTIGATION

Use this report as the controlling technical handoff:

AI-Logs/Reports/2026-11-17-exit-focal-point-regression-readonly-report.md

That investigation established:

- Levels 1-4 are manually working exit controls.
- Levels 5, 8, and 9 exhibit the focal-point regression.
- Level 7 is excluded because it has separate unresolved presentation problems.
- ExitZoom simulation and K/ZOffset cadence remain shared and correct.
- AvoidSpikes is not the cause.
- The shared final portrait camera is not the cause.
- Accepted Level 8 and Level 9 Playing-placement translations are not the
  cause.
- Level 5's presentation mapping uses a live near-rim anchor that moves as
  K/ZOffset change.
- Levels 8/9 use a ROM-metadata adapter whose far-opening displacement is
  magnified through the live near-depth denominator.
- The resulting mapped far/deep opening drifts dramatically during ExitZoom.
- The regression was introduced with the whole-game AtariPresentationAdapter
  integration in commit 7fb0819cdccddbf0f3e4fbc40409023050546d9d.
- Enough evidence exists for one bounded presentation-only correction:
  capture the mapped far/deep opening at ExitZoom onset, then apply shared
  ExitZoom-only compensation that keeps that point fixed.

Do not reopen the investigation.

ACCEPTED BASELINE

Begin from the current committed baseline after the accepted Blaster
Exit/Approach scale closeout:

753728e79ed86624c9b1728dfd1c8db8f49d1e2b

Verify the actual current branch, HEAD, upstream, index, worktree, and
ahead/behind state before project changes.

The recently accepted Blaster Exit/Approach size correction is frozen.

Do not alter:

- Blaster ordinary Playing size;
- Blaster ExitZoom size;
- Blaster Approach size;
- stable-rim scale baseline;
- ordinary outward orientation;
- current yellow color;
- removed brightness diagnostic.

PRIMARY PRESENTATION CONTRACT

At the exact beginning of actual ExitZoom travel:

1. Determine the active level's visible mapped far/deep opening center through
   the same presentation route that renders the tube.
2. Store that presentation-space point as the ExitZoom focal anchor.
3. During every subsequent ExitZoom render, calculate where the same logical
   far/deep opening maps under the current live K/ZOffset state.
4. Apply one presentation-space compensation translation equal to:

   latchedOpeningAtExitStart - currentMappedOpening

5. Apply that compensation consistently to the complete ExitZoom playfield
   presentation so the visible tube expands around the latched opening.
6. Do not alter source geometry, projection, K/ZOffset, gameplay depth, or
   accepted ordinary Playing placement.

The invariant is:

During ExitZoom, the mapped far/deep opening selected at travel onset remains
fixed in presentation space while tube geometry enlarges around it.

COORDINATE-SPACE REQUIREMENT

Choose one explicit coordinate space for the latched opening and compensation.

Prefer the logical presentation space immediately:

- after the active level presentation adapter;
- after the accepted level-specific Playing placement;
- before the shared portrait host camera.

This is the seam recommended by the investigation.

Do not mix logical and host-space coordinates.

Do not include the final camera zoom in one side of the subtraction but not the
other.

If the existing architecture makes another coordinate seam demonstrably more
correct, document and test that choice.

Do not alter the camera target as a substitute unless code evidence proves that
is the narrowest equivalent implementation.

OPENING-DEFINITION CONTRACT

Use a deterministic geometry-derived opening definition.

For closed tubes in this milestone:

- use the mapped center of the far rim;
- derive it from all mapped far-rim vertices;
- do not use the near-rim center;
- do not use a screenshot-derived point.

For open tubes:

- use the mapped midpoint of the two actual deep/open terminal endpoints;
- do not use the all-vertex centroid;
- do not invent another center from visual fitting.

Use the same source point identities at ExitZoom onset and every later sample.

If the current tube builder already exposes the required far/open endpoint
geometry, reuse it.

Do not duplicate raw tube data or per-level coordinates.

LATCH LIFECYCLE

The latched ExitZoom focal anchor must have an explicit lifecycle.

Capture it only when actual ExitZoom travel begins, after any AvoidSpikes wait
and after the normal ExitDelay boundary.

Preserve it throughout that exit attempt.

Clear or replace it on:

- fresh level/wave initialization;
- game reset;
- successful exit completion;
- canceled/fatal exit lifecycle as appropriate;
- a new ExitZoom attempt.

A fatal Spike collision or exit retry must not reuse stale focal data from a
previous attempt.

Do not add the presentation latch to gameplay recording metadata.

Do not let the latch affect deterministic simulation state.

WORKING CONTROL BEHAVIOR

Levels 1-4 are manually working controls.

The candidate should be implemented globally/shared, but it must not create a
new visible regression for those levels.

For Levels 1-4:

- latch their mapped far-rim center through the same shared helper;
- apply the same compensation invariant;
- verify that their already coherent exit remains coherent;
- do not introduce level-specific exceptions merely to preserve working
  behavior;
- do not retune their ordinary Playing placement.

Their existing mathematical opening drift is smaller than the failing levels,
but the global invariant may remove it.

The correction must remain continuous at ExitZoom onset.

LEVEL 5 REQUIREMENTS

Level 5 uses a special presentation transform around a live projected near
anchor.

Do not rewrite or retune that accepted ordinary Playing transform.

During ExitZoom:

- calculate Level 5's mapped far-rim center through its existing adapter;
- latch that mapped point at travel onset;
- compensate the complete mapped ExitZoom presentation so the opening stays
  fixed;
- do not attempt to freeze or replace the adapter's internal live near anchor;
- do not alter Level 5 CenterY, YOffset, .67 horizontal gain, .88 vertical
  gain, placement, geometry, or AvoidSpikes behavior.

The presentation compensation must correct the exit result without changing
the ordinary Level 5 mapping.

LEVEL 8 AND LEVEL 9 REQUIREMENTS

Preserve the accepted frozen Playing placements:

- Level 8: +212.28139 logical Y
- Level 9: +180.96825 logical Y

Do not change or conditionally disable those translations.

Use each open tube's actual deep/open terminal endpoint midpoint.

Apply compensation after the existing ROM-metadata adapter and accepted
placement, before the host camera.

Do not change:

- recovered geometry;
- metadata;
- model-frame mapping;
- entity registration;
- projection scale;
- perspective;
- open topology;
- lane count.

The same compensation must move the entire ExitZoom presentation consistently,
not only the blue tube.

LEVEL 10 DIAGNOSTIC COMPARISON

Level 10 uses the same general ROM-metadata adapter family and a frozen +50
placement.

If production/deterministic infrastructure permits a bounded ExitZoom test
through its existing AvoidSpikes gate, include Level 10 as regression coverage.

Do not retune or change Level 10.

Do not require Level 10 manual acceptance for this candidate unless Neal later
chooses to test it.

LEVEL 7 BOUNDARY

Do not reconstruct or repair Level 7.

If the globally shared ExitZoom path reaches Level 7, ensure the new code:

- does not change Level-7-specific Playing geometry;
- does not change Level-7 entity registration;
- does not change Level-7 centering;
- does not change Level-7 depth-motion behavior.

If a valid Level 7 opening anchor cannot be derived cleanly from its special
presentation path, isolate that limitation explicitly rather than inventing a
mapping.

LEVEL 7 must not block the Levels 1-5/8/9 candidate unless shared code cannot be
made safe.

COMPLETE-PLAYFIELD COMPENSATION

The compensation should apply consistently to every playfield element that
must stay registered during ExitZoom, including as applicable:

- tube rims and rails;
- Blaster anchor and artwork;
- Spikes;
- lane highlight;
- exit/death effects anchored in tube space;
- other presentation-space objects drawn during ExitZoom.

Do not compensate the tube alone while leaving the Blaster behind.

Do not compensate HUD/screen-space text.

Do not move the `AVOID SPIKES` message or other HUD-like overlays unless code
evidence establishes that they are intentionally part of the tube-space exit
view.

Preserve existing rendering boundaries.

BLASTER SCALE CORRECTION PROTECTION

The focal compensation and the accepted Blaster travel-size correction solve
different regressions.

Preserve the accepted scale function exactly.

The Blaster should:

- remain compact;
- retain its corrected local size;
- stay attached to its exiting lane;
- receive the same ExitZoom focal compensation as the tube anchor.

Do not apply focal compensation inside the Blaster local-size calculation.

Do not double-translate Blaster coordinates.

APPROACH AND REVERSEZOOM BOUNDARY

This candidate is specifically for ExitZoom focal stability.

Do not automatically apply the compensation to Approach.

At successful exit handoff:

- clear/end the ExitZoom compensation at the existing state boundary;
- preserve the accepted Approach presentation and Blaster scale correction;
- verify no one-frame positional discontinuity is introduced beyond the
  existing intended handoff.

For ReverseZoom after a fatal Spike collision or other retry behavior:

- determine from the current state machine whether the same exit attempt's
  latched focal anchor remains appropriate;
- preserve simulation and current accepted behavior;
- do not redesign ReverseZoom.

If evidence shows ReverseZoom requires inverse compensation to retrace the same
presentation path, implement only the mathematically necessary shared inverse
behavior and test it.

If the current fatal-collision path restores the full tube immediately, clear
the latch according to that established lifecycle.

GAMEPLAY AND SIMULATION PROTECTION

Do not change:

- ExitDelay duration;
- AvoidSpikes behavior;
- player depth;
- player lane/edge state;
- spinner input;
- K/ZOffset evolution;
- successful-exit threshold;
- Spike collision behavior;
- retry behavior;
- scoring;
- lives;
- RNG;
- level progression;
- recording/playback simulation;
- fixed-step timing.

Presentation may read state but must not alter simulation.

RECORDING AND PLAYBACK

The focal anchor/compensation is rendering state only.

Do not add it to gameplay recordings.

Do not change recording schema or playback inputs.

The same recording must reproduce the same gameplay state progression before
and after the correction.

Only ExitZoom presentation coordinates should change.

FROZEN PRESENTATION PROTECTION

Preserve:

- accepted Levels 1-6 Playing presentation;
- Level 8 +212.28139;
- Level 9 +180.96825;
- Level 10 +50;
- ordinary Blaster outward orientation;
- Blaster Exit/Approach scale correction;
- shared perspective;
- tube geometry;
- entity registration;
- HUD;
- option-off/legacy behavior.

Do not globally change projection origin, camera target, tube coordinates, or
per-level placement to solve an ExitZoom-only problem.

NO IMAGE ANALYSIS

No image analysis is required or authorized for this implementation task.

Do not search for, enumerate, open, inspect, or analyze screenshots, images,
videos, frames, contact sheets, or visual assets.

The controlling report already established the cause.

CODE QUALITY

Prefer one shared presentation helper/structure that represents:

- ExitZoom focal anchor active/inactive state;
- mapped opening calculation;
- current compensation vector.

Avoid scattering compensation logic across tube and entity renderers.

Do not create per-level focal constants.

Derive opening geometry from the active level's mapped far/open endpoints.

Add concise comments explaining:

- the opening is latched when travel begins;
- compensation keeps the mapped opening fixed;
- ordinary Playing placement remains untouched;
- state is presentation-only.

Do not narrate obvious code.

TESTING REQUIREMENTS

Add focused coverage proving at minimum:

A. OPENING SELECTION

1. Closed tube opening uses mapped far-rim center.
2. Open tube opening uses mapped terminal-endpoint midpoint.
3. Opening selection uses active mapped geometry, not source/raw coordinates.
4. No screenshot-derived or per-level focal constants exist.

B. LATCH LIFECYCLE

5. Focal point captures at actual ExitZoom onset.
6. AvoidSpikes wait does not capture prematurely.
7. ExitDelay does not capture prematurely.
8. Same exit attempt reuses one stable latch.
9. Successful handoff clears it.
10. Reset/fresh-level initialization clears it.
11. Fatal/canceled exit does not leave stale state.
12. New retry captures a new focal anchor.

C. WORKING LEVELS

13. Levels 1-4 remain continuous at ExitZoom onset.
14. Their mapped far-rim centers remain stable under compensated ExitZoom.
15. No ordinary Playing coordinate changes.
16. No tube/entity misregistration is introduced.

D. FAILING LEVELS

17. Level 5 mapped far-rim center remains fixed through both ExitZoom phases.
18. Level 8 mapped open midpoint remains fixed.
19. Level 9 mapped open midpoint remains fixed.
20. The large drift values reproduced from the old equations are eliminated by
    compensation.
21. Level 5's special live near-anchor mapper remains otherwise unchanged.
22. Level 8/9 accepted placement constants remain unchanged.

E. SHARED PRESENTATION

23. Tube and Blaster receive identical compensation.
24. Spikes/lane-space effects remain registered where rendered.
25. HUD/screen-space content is not compensated.
26. Blaster local scale is unchanged by focal compensation.
27. Blaster remains compact through exit.
28. No double translation occurs.

F. EXIT STATES

29. ZOffset phase keeps opening fixed.
30. K-growth phase keeps opening fixed.
31. Late ExitZoom remains finite.
32. Successful handoff does not retain ExitZoom compensation into Approach.
33. ReverseZoom/fatal-exit behavior follows the established lifecycle.
34. Exit timing and state evolution remain unchanged.

G. PROTECTION

35. Gameplay state is unchanged before/after rendering.
36. Recording/playback schema remains unchanged.
37. Deterministic simulation remains identical.
38. Option-off/legacy behavior remains unchanged.
39. Level 8 placement remains +212.28139.
40. Level 9 placement remains +180.96825.
41. Level 10 placement remains +50.
42. Accepted Blaster scale correction remains unchanged.
43. Level 7-specific architecture remains unchanged.
44. Approach presentation remains unchanged except for a clean existing
    handoff boundary.
45. Levels 1-6 Playing presentation remains unchanged.

Use exact mathematical/geometric assertions, not screenshot pixels.

VALIDATION

Run bounded focused tests for:

- focal opening selection;
- focal latch lifecycle;
- Levels 1-5 ExitZoom;
- Levels 8/9 ExitZoom;
- shared tube/Blaster compensation;
- successful handoff;
- reset/retry;
- state preservation.

Run targeted regressions covering:

- ExitZoom;
- AvoidSpikes;
- Spike exit collision/retry;
- Blaster scale;
- Levels 1-5;
- Levels 8/9/10 presentation;
- AtariPresentationAdapter;
- VectorRenderer;
- Approach/transition;
- recording/playback;
- display/camera;
- option-off/legacy behavior.

Then run:

- full solution test suite with explicit VSTest hang-timeout protection;
- Release solution build;
- git diff --check.

Do not weaken tests.

Canceled, stalled, blocked, timed-out, or incomplete commands are not PASS.

Do not launch the interactive game.

REPOSITORY SAFETY

Record at start:

- branch;
- HEAD;
- upstream/tracking;
- ahead/behind;
- index;
- tracked worktree;
- relevant untracked state.

Expected committed baseline:

753728e79ed86624c9b1728dfd1c8db8f49d1e2b

on master tracking origin/master at 0/0.

Verify rather than assume.

Preserve unrelated local work, including if still present:

- .github/copilot-instructions.md;
- Reference/arcade/MBUCOD.V05.txt;
- Reference/arcade/MBUDOC.DOC.

Do not reset, restore, clean, stash, rebase, switch branches, stage, commit, or
push.

This implementation candidate stops before manual acceptance and closeout.

AI-LOGS

Follow .github/copilot-instructions.md.

Save:

- this substantive prompt under AI-Logs/Prompts/;
- the paired ExitZoom focal-compensation implementation report under
  AI-Logs/Reports/.

A chat response is not a substitute for the report.

Do not stage or commit AI-Logs.

MANUAL HANDOFF

After automated validation, STOP.

Do not launch the interactive game.

Provide exact manual commands for:

- Level 1 working closed control;
- Level 4 working control with AvoidSpikes;
- Level 5 failing-case retest;
- Level 8 open-tube retest;
- Level 9 open-tube retest;
- diagnostic Level 10 only if its existing validated command remains useful.

Use established CLI syntax.

Do not invent recording filenames.

State that Neal should judge:

- ExitZoom remains focused on the far/deep opening;
- opening no longer drifts as perspective increases;
- tube expands outward around the opening;
- Blaster remains attached to the exiting lane;
- Blaster remains compact;
- Level 1-4 exits remain correct;
- Level 5/8/9 regressions are fixed;
- Approach handoff remains coherent;
- gameplay/timing remains unchanged;
- accepted ordinary Playing placement remains unchanged.

Manual acceptance remains PENDING.

EXPECTED REPORT

The paired report must include:

- exact implementation architecture;
- coordinate space used for latch/compensation;
- closed/open opening definitions;
- latch lifecycle;
- files changed;
- Level 1-4 control results;
- Level 5 before/after focal trajectory;
- Level 8 before/after focal trajectory;
- Level 9 before/after focal trajectory;
- Level 10 comparison if tested;
- tube/Blaster registration evidence;
- Blaster-scale preservation;
- gameplay/simulation preservation;
- recording/playback preservation;
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

Complete autonomously and stop with the ExitZoom focal-compensation candidate
ready for Neal's manual testing.

TEMPEST_EXITZOOM_FAR_OPENING_FOCAL_COMPENSATION