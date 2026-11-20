READ THIS ENTIRE USER-PROVIDED TASK BEFORE TAKING ANY ACTION.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, reading project files, running commands, opening images, or
taking repository action:

1. Locate the unique completion token near the end of this user-provided task.
2. Verify that exactly one complete token beginning with "TEMPEST_" appears on
   a line by itself.
3. Automatically appended VS Code/agent context after the user's token does not
   invalidate it.
4. If exactly one complete token cannot be found, STOP before project or
   repository action and explain why.

TASK

Implement one bounded DEVELOPMENT-ONLY outer-tube-proportion experiment for
Levels 1 and 7 under --atari-presentation.

Add a rendering-only diagnostic Y gain that:

- defaults to 1.00;
- affects Levels 1 and 7 only;
- accepts experimental values from 1.00 through 1.32;
- scales model cross-section Y displacement around source center (0,0);
- applies before depth projection in the active presentation adapter;
- propagates consistently to tube geometry, entity anchors, and local
  presentation basis/scale;
- leaves gameplay geometry, depth, perspective, final placement, viewport,
  simulation, recording, and playback unchanged.

This experiment tests outer cross-section proportion only.

It does NOT claim to fix Level 7's unresolved inner/core/perspective
relationship.

Do not perform another broad investigation.

Do not select a final production value.

Do not stage, commit, or push.

Stop after automated validation and provide exact manual A/B commands.

CONTROLLING PLAN

Use:

AI-Logs/Reports/2026-11-19-cross-level-proportion-perspective-planning-report.md

as the controlling technical handoff.

That report established enough evidence for this one reversible experiment.

Do not redo the planning investigation.

ESTABLISHED EXPERIMENT SCOPE

Affected levels:

- Level 1
- Level 7

Unaffected controls:

- Level 2
- Level 5

Do not broaden the diagnostic to:

- Levels 3, 4, 6, 8, 9;
- diagnostic Level 10;
- all closed tubes;
- all isotropic tubes;
- all 99 displayed levels.

No inference beyond Levels 1 and 7 is authorized.

DIAGNOSTIC VALUES

The diagnostic must support at least:

- 1.00: identity/current presentation
- 1.21: prior Level 7 contour-fit checkpoint
- 1.23: prior Level 1 contour-fit checkpoint
- 1.31: shared reference-envelope checkpoint

Allowed experimental range:

1.00 through 1.32 inclusive

No final value is selected.

No value above 1.00 is accepted production behavior.

The default remains exactly 1.00.

VISUAL-EVIDENCE CONTEXT

The controlling reports established these VISUAL DIAGNOSTIC findings:

- Level 1 C# outer W:H is approximately 1.000.
- Level 1 reference outer W:H is approximately 0.762.
- Level 7 C# outer W:H is approximately 0.999.
- Level 7 reference outer W:H is approximately 0.765.
- Level 1 contour-fit diagnostic Y gain was approximately 1.230.
- Level 7 contour-fit diagnostic Y gain was approximately 1.210.
- The four measured reference outer ratios cluster around 0.76325.
- Ratio-derived checkpoints for Levels 1 and 7 are approximately 1.31.
- Levels 2 and 5 already have vertically emphasized presentation and visually
  compare closely in outer proportion.

These are image-derived diagnostics, not ROM constants or accepted project
translations.

Do not embed image coordinates or fitted landmarks in production code.

UNRESOLVED LEVEL 7 PERSPECTIVE

The Level 7 inner/core relationship remains unresolved.

The prior static comparison suggested a possible difference in:

- far/core-center displacement;
- upper-notch-to-core relationship;
- rail convergence.

Those measurements were lower confidence because gameplay vectors obscured
parts of the reference core.

Therefore:

- do not change depth projection;
- do not change near/far scale;
- do not change far-center displacement;
- do not change portrait perspective;
- do not change Level 7 point geometry;
- do not claim that a Y gain fixes the complete Level 7 presentation.

The experiment is allowed to improve only the outer cross-section.

CLI OPTION

Add one narrowly scoped development-only command-line option.

Preferred syntax:

--tube-y-gain <value>

Use another name only if an existing diagnostic naming convention clearly
requires it.

Required behavior:

- omitted option means exactly 1.00;
- explicit 1 and 1.00 are valid;
- 1.21 is valid;
- 1.23 is valid;
- 1.31 is valid;
- 1.32 is valid;
- values below 1.00 are rejected;
- values above 1.32 are rejected;
- malformed values are rejected;
- missing values are rejected;
- duplicate occurrences are rejected;
- NaN is rejected;
- positive and negative infinity are rejected;
- invariant-culture parsing is required.

The option requires:

--atari-presentation

The option is valid only with:

--level 1

or:

--level 7

For normal natural campaign progression, determine the safest bounded behavior
from the existing CLI architecture.

Prefer fail-closed explicit-level use for this experiment unless the current
option model can clearly and safely apply the diagnostic only while displayed
Level 1 or 7 is active.

Do not silently apply the gain to other levels.

Do not add the value to gameplay recordings.

Do not change recording or playback schema.

Do not allow the gain to affect headless simulation.

If visible recorded playback is permitted by the existing option architecture,
keep the diagnostic rendering-only and outside recording metadata.

TRANSFORM SEAM

Apply the gain in AtariPresentationAdapter at the model cross-section seam,
before depth-dependent projection.

For affected Levels 1 and 7:

Given model coordinate:

(x, y, z)

calculate diagnostic model Y:

yDiagnostic = sourceCenterY + gain * (y - sourceCenterY)

For both affected levels:

sourceCenterY = 0

Then pass:

(x, yDiagnostic, z)

through the existing level-specific depth/presentation projection.

Do not mutate LevelDefinition.Points.

Do not mutate gameplay entity state.

Do not alter simulation coordinates.

Do not alter collision geometry.

Do not write transformed coordinates back into model objects.

SOURCE-SPACE ANCHOR

Use source-space cross-section center:

(0,0)

For the Y operation:

sourceCenterY = 0

This is established by the current production point-cloud centroids for Levels
1 and 7.

Do not use as the gain anchor:

- projected near-envelope center;
- complete screen-space envelope center;
- final placement;
- LevelDefinition.CenterY;
- LevelDefinition.YOffset;
- HUD/gameplay-region center;
- screenshot-derived coordinates.

The source-space center must remain fixed when the gain changes.

DEPTH AND CENTERLINE PROTECTION

The diagnostic must scale only source/model cross-section Y displacement.

Do not scale or alter:

- z/depth;
- Level.Position;
- Level.Depth;
- Projection.K;
- Projection.ZOffset;
- PortraitPerspective;
- CenterY;
- YOffset;
- ApproachDepthOffset;
- ApproachVerticalOffset;
- Y2D metadata;
- final level placement;
- host camera;
- viewport.

At source point `(0,0)`, changing gain must not move the projected centerline.

Near and far cross-sections should receive the same model-space Y gain while
retaining the existing depth relationship.

LEVEL 1 REQUIREMENTS

Level 1 currently uses its accepted shared presentation path.

For Level 1:

- preserve current geometry;
- preserve point order;
- preserve lane topology;
- preserve all gameplay;
- apply the gain only in presentation;
- preserve Level 1 placement;
- preserve Blaster placement/orientation;
- preserve entity registration;
- preserve Approach and ExitZoom behavior except for the intended transformed
  cross-section presentation.

At gain 1.00, output must be exactly equivalent to current behavior.

LEVEL 7 REQUIREMENTS

Level 7's clean reconstruction is accepted and frozen.

Preserve:

- production 16-point clover;
- point order;
- closed topology;
- Level7VerticalPlacement = 228.99702f;
- zero horizontal correction;
- single shared geometry;
- shared AtariPresentationAdapter architecture;
- directed Flipper transition geometry;
- gameplay timing and motion;
- deterministic playback;
- raw record-06 evidence data;
- resolved depth-motion behavior.

Do not restore any removed record-06 runtime mapper or prototype architecture.

Apply the diagnostic gain through the current shared production-geometry
presentation path.

At gain 1.00, output must remain exactly equivalent to the accepted Level 7
presentation.

LEVEL 2 AND LEVEL 5 CONTROLS

Do not alter Level 2 or Level 5 behavior.

Preserve exactly:

- Level 2 Y presentation factor 1.314;
- Level 5 .670 X and .880 Y presentation gains;
- their local-scale handling;
- their placement;
- their gameplay and presentation.

Supplying --tube-y-gain with Level 2 or Level 5 must be rejected rather than
silently ignored.

Tests must prove that Level 2 and Level 5 rendering is unchanged by this
implementation.

TUBE AND ENTITY REGISTRATION

The diagnostic gain must apply consistently to the presentation of:

- tube near-rim vertices;
- tube far-rim vertices;
- near-to-far rails;
- Blaster anchor;
- Blaster local geometry as appropriate;
- Tankers;
- Flippers;
- Tanker-released Flippers;
- Spikers;
- Eggs;
- Spikes;
- player-shot anchors;
- enemy-shot anchors;
- tube-space effects;
- lane highlight.

No entity center may remain on the old unscaled presentation route while the
tube uses the scaled route.

Do not add per-entity offsets.

LOCAL PRESENTATION BASIS

Propagate the vertical Jacobian consistently.

At gain g:

- presentation LocalScaleY must reflect the Y gain at the selected adapter
  seam;
- LocalScaleX must remain unchanged by the diagnostic;
- local geometry derived from model-space X/Y axes must remain registered;
- screen-oriented glyphs should remain screen-oriented where the renderer
  intentionally defines them that way.

Study the existing Level 2 vertical-factor implementation as the closest
architectural precedent.

Do not simply multiply final rendered glyph height independently of anchor
projection.

BLASTER

Preserve the accepted Blaster behavior:

- outward orientation;
- near-rim attachment;
- lane/edge registration;
- current yellow color;
- ordinary Playing size behavior;
- ExitZoom/Approach scale normalization;
- six-line artwork topology;
- movement and firing;
- gameplay collision.

The Blaster's presentation anchor must follow the transformed projected rim.

The gain must not invert the outward normal.

At gain 1.00, Blaster output must remain unchanged.

FLIPPER TRANSITIONS

Preserve gameplay-selected directed transitions.

For Level 7 in particular:

- preserve long/reflex paths;
- preserve representative 270-degree transitions;
- project transition geometry through the same diagnostic presentation route;
- do not change transition timing;
- do not change transition progress;
- do not reintroduce special Level 7 transition code.

The diagnostic gain must not move Flippers off the visible tube.

SPIKES AND SHOTS

Preserve gameplay depths and lanes.

Spike bases and tips must remain registered to the transformed rails.

Shot centers must remain registered.

Do not change shot speed, size, timing, lifetime, or collision.

EXITZOOM FOCAL COMPENSATION

The accepted ExitZoom focal compensation remains frozen.

Closed tubes use the mapped far-rim-center pivot.

Levels 1 and 7 are closed tubes.

Ensure the focal pivot is calculated from the gain-transformed mapped geometry.

Do not change the accepted focal rule.

If the diagnostic gain is fixed for the entire application run, no dynamic
cache invalidation should be necessary beyond normal state lifecycle.

Do not support changing the gain interactively during an active run unless the
existing diagnostic architecture already provides a safe immutable startup
value.

APPROACH

Preserve:

- Approach timing;
- ApproachDepthOffset;
- ApproachVerticalOffset;
- starfield;
- recharge message;
- Blaster movement;
- accepted Blaster scale behavior.

The source cross-section Y gain may affect the displayed tube shape during
Approach through the same adapter route.

Do not scale the Approach centerline or vertical offset.

At Approach completion, the displayed cross-section must transition
continuously to ordinary Playing at the same gain.

RECORDING AND PLAYBACK

Do not modify:

- recording schema;
- recorded inputs;
- gameplay seed;
- playback state;
- deterministic summaries.

The diagnostic is rendering-only.

Do not serialize the gain.

The same recording with gain 1.00 and gain 1.31 must produce identical
simulation and playback boundaries.

Only rendering may differ.

GAMEPLAY PROTECTION

Do not change:

- source geometry;
- model coordinates;
- collision;
- movement;
- timing;
- enemy behavior;
- firing;
- wave completion;
- progression;
- score;
- lives;
- RNG;
- fixed-step simulation.

Rendering must remain state-read-only.

FROZEN VIEWPORT AND SHARED PRESENTATION

Preserve:

- host window 1600x1280;
- portrait viewport 960x1280;
- viewport origin (320,0);
- one-host-pixel border;
- common host camera;
- Level 7 +228.99702;
- Level 8 +212.28139;
- Level 9 +180.96825;
- diagnostic Level 10 +50;
- accepted ExitZoom behavior;
- accepted Blaster behavior;
- Levels 2 and 5 controls.

Do not begin perspective tuning.

NO IMAGE ANALYSIS

No new image analysis is needed or authorized for this implementation task.

Do not inspect screenshots, images, videos, contact sheets, or frames.

Use the controlling reports as evidence.

CODE QUALITY

Keep the diagnostic small and removable.

Prefer:

- one validated immutable startup configuration value;
- one narrow adapter-level helper for Levels 1 and 7;
- existing parser/configuration patterns;
- focused tests.

Do not scatter level checks through renderer call sites.

Do not add per-level hardcoded production gains.

Do not describe 1.21, 1.23, or 1.31 as accepted values.

Comment only where needed to explain:

- render-only source-cross-section Y scaling;
- source-center anchor;
- depth/centerline preservation;
- Levels 1/7 experimental scope.

TESTING REQUIREMENTS

Add focused automated coverage proving at minimum:

A. OPTION PARSING

1. Omitted option produces exactly 1.00.
2. 1 and 1.00 parse.
3. 1.21 parses.
4. 1.23 parses.
5. 1.31 parses.
6. 1.32 parses.
7. Values below 1.00 are rejected.
8. Values above 1.32 are rejected.
9. Missing value is rejected.
10. Malformed value is rejected.
11. Duplicate option is rejected.
12. NaN is rejected.
13. Positive/negative infinity are rejected.
14. Parsing is invariant-culture.
15. Option requires --atari-presentation.
16. Option rejects unsupported levels.
17. Option does not enter recording metadata.

B. IDENTITY

18. Gain 1.00 is numerically equivalent to current Level 1 presentation.
19. Gain 1.00 is numerically equivalent to accepted Level 7 presentation.
20. Level 2 output is unchanged.
21. Level 5 output is unchanged.
22. Levels 3/4/6/8/9/10 are unchanged.
23. Legacy/option-off output is unchanged.

C. CROSS-SECTION

24. Level 1 source-space X coordinates are unchanged.
25. Level 1 source-space Y displacement scales by gain.
26. Level 7 source-space X coordinates are unchanged.
27. Level 7 source-space Y displacement scales by gain.
28. Source center (0,0) remains fixed.
29. Near cross-section W:H responds as expected.
30. Far cross-section W:H responds by the same source-space gain.
31. Depth remains unchanged.
32. Centerline remains unchanged.
33. Final per-level placement remains unchanged.

D. LOCAL BASIS

34. LocalScaleX remains unchanged.
35. LocalScaleY reflects the diagnostic gain.
36. Tube/entity anchors remain registered.
37. Blaster remains attached to the rim.
38. Blaster outward orientation remains correct.
39. Spike base/tip remain on the transformed rail.
40. Shots remain lane-registered.
41. Flipper transitions remain registered.
42. No double scaling occurs.

E. STATES

43. Ordinary Playing uses the gain.
44. Approach uses the same source-cross-section gain.
45. Approach centerline/offset remains unchanged.
46. Approach completion remains continuous.
47. ExitZoom uses the gain-transformed geometry.
48. ExitZoom focal compensation remains stable.
49. Retry/reset does not alter the configured gain.
50. Renderer state remains read-only.

F. DETERMINISM

51. Gain 1.00 and 1.31 produce identical gameplay state and playback
    boundaries.
52. Recording schema remains unchanged.
53. RNG/timing remain unchanged.
54. Gameplay geometry remains byte/value identical.

G. FROZEN SYSTEMS

55. Level 7 placement remains +228.99702.
56. Level 8 remains +212.28139.
57. Level 9 remains +180.96825.
58. Level 10 remains +50.
59. Host/viewport values remain unchanged.
60. Level 2 factor 1.314 remains unchanged.
61. Level 5 .670/.880 gains remain unchanged.

Do not use screenshot pixels as automated test oracles.

Do not add tests asserting that any diagnostic gain is visually correct.

VALIDATION

Run bounded focused tests covering:

- tube-y-gain option parsing;
- Levels 1 and 7 adapter projection;
- identity equivalence;
- local-scale/Jacobian behavior;
- tube/entity registration;
- Approach;
- ExitZoom;
- deterministic playback.

Run targeted regressions covering:

- Level 1;
- Level 2;
- Level 5;
- Level 7;
- AtariPresentationAdapter;
- VectorRenderer;
- Blaster presentation/scale;
- Flipper transitions;
- Spikes/shots;
- ExitZoomFocalCompensation;
- LevelApproach;
- recording/playback;
- DisplayConfiguration;
- Levels 8/9/10 protection;
- option-off/legacy behavior.

Then run:

- full solution tests with explicit VSTest hang-timeout protection;
- Release solution build;
- git diff --check.

Do not weaken tests.

Canceled, blocked, stalled, timed-out, or incomplete commands are not PASS.

Do not launch the game.

REPOSITORY SAFETY

Record start and finish:

- branch;
- HEAD;
- upstream;
- ahead/behind;
- index;
- tracked worktree;
- relevant untracked state.

Expected baseline:

c8bc40416eff4ffaac17862f7c894e5a34d133f9

on master tracking origin/master at 0/0.

Verify rather than assume.

Preserve unrelated local work, including:

- .github/copilot-instructions.md;
- local reference files;
- UserData comparison images.

Do not inspect UserData images in this task.

Do not stage, commit, push, reset, restore, clean, stash, rebase, or switch
branches.

This task stops before manual acceptance.

AI-LOGS

Follow `.github/copilot-instructions.md`.

Save:

- this substantive implementation prompt under AI-Logs/Prompts/;
- the paired diagnostic Y-gain implementation report under AI-Logs/Reports/.

Do not stage or commit AI-Logs.

MANUAL HANDOFF

After automated validation, STOP.

Do not launch the game.

Provide exact commands using the implemented option for:

LEVEL 1

- gain omitted/default;
- 1.00;
- 1.21;
- 1.23;
- 1.31.

LEVEL 7

- gain omitted/default;
- 1.00;
- 1.21;
- 1.23;
- 1.31.

Also provide Level 2 and Level 5 control commands without the option.

Manual testing should judge:

- outer-tube visual proportion;
- horizontal center;
- vertical placement;
- tube/entity registration;
- Blaster rim attachment and outward direction;
- enemy lane registration;
- shots and Spikes;
- Flipper transitions;
- ordinary Playing;
- exit;
- Approach;
- gameplay/timing;
- which gain, if any, improves Level 1;
- which gain, if any, improves Level 7;
- whether the Level 7 inner/core relationship remains independently wrong.

State clearly:

- this is a DEVELOPMENT-ONLY experiment;
- no gain above 1.00 is accepted yet;
- perspective correction is not included;
- manual acceptance remains PENDING.

EXPECTED REPORT

The report must include:

- exact option syntax;
- parser and compatibility behavior;
- exact model-space transform;
- anchor;
- affected levels;
- unaffected controls;
- files changed;
- local basis/Jacobian treatment;
- tube/entity registration evidence;
- Blaster preservation;
- Approach behavior;
- ExitZoom behavior;
- deterministic playback evidence;
- recording-schema preservation;
- focused test command/count/result;
- targeted test command/count/result;
- full-suite count/result;
- Release build result;
- git diff --check result;
- repository start/finish state;
- exact manual commands;
- known limitations;
- explicit perspective exclusion;
- manual acceptance PENDING;
- exact AI-Logs prompt/report paths.

Do not ask Neal questions during execution.

Do not pause for manual input.

Complete autonomously and stop with the diagnostic Levels 1/7 Y-gain
experiment ready for Neal's manual A/B testing.

TEMPEST_LEVEL1_LEVEL7_TUBE_Y_GAIN_DIAGNOSTIC