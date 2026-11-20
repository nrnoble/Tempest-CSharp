READ THIS ENTIRE USER-PROVIDED TASK BEFORE TAKING ANY ACTION.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, reading files, running commands, or taking repository
action:

1. Locate the unique completion token near the end of this task.
2. Verify that exactly one complete token beginning with "TEMPEST_" appears on
   a line by itself.
3. Automatically appended VS Code/agent context after the token does not
   invalidate it.
4. This check detects an incomplete or truncated task, not appended agent
   context.
5. If exactly one complete token cannot be found, STOP before repository action
   and explain why.

TASK

Implement a bounded whole-game host-window and portrait-viewport enlargement.

Change the application window to:

1600 x 1280

Inside that window, preserve the existing portrait gameplay viewport's 3:4
width-to-height aspect ratio and use the complete 1280-pixel window height.

The resulting centered portrait viewport must be:

960 x 1280

with:

X = 320
Y = 0

This leaves:

320 host pixels to the left
320 host pixels to the right
0 host pixels above
0 host pixels below

This milestone is ONLY a uniform physical enlargement of the existing portrait
presentation.

Do not change internal presentation proportions yet.

Do not attempt to solve the separate C# versus arcade/MAME screenshot
width-to-height comparison issue in this task.

MANUAL GOAL

The immediate goal is a larger application/presentation on Neal's display.

The current dimensions are smaller than desired.

This milestone should make the existing accepted presentation physically
larger while leaving the visual composition internally equivalent.

The next, separate milestone will investigate viewport/screenshot proportions
for direct side-by-side arcade/MAME comparison.

Do not combine those tasks.

MATHEMATICAL TARGET

Current accepted portrait viewport:

720 x 960

Requested portrait viewport:

960 x 1280

This is a uniform scale factor of:

4 / 3

in both dimensions.

Therefore this milestone must preserve the viewport's current internal aspect
ratio exactly.

The host window changes to:

1600 x 1280

Centered layout:

left = (1600 - 960) / 2 = 320
top = (1280 - 1280) / 2 = 0

right = 320
bottom = 0

Do not introduce asymmetric centering.

CURRENT BASELINE

Begin from the current committed baseline after Level 7 reconstruction
closeout.

Expected baseline:

7ba80f39df9daeaa86a7cafc81690f2f5c37bb33

on master tracking origin/master at 0 ahead / 0 behind.

Verify actual state first.

The Level 7 closeout records the current whole-game viewport/screenshot
proportion issue as a separate deferred milestone.

Do not reopen any closed level-specific presentation work.

WINDOW SIZE

Update the normal application window from its current accepted size to:

1600 x 1280

Identify the authoritative window-size configuration rather than scattering
literal replacements.

If window dimensions are defined centrally, change them centrally.

If tests or startup code contain duplicated expected values, update only those
that intentionally assert the application-window contract.

Do not change fullscreen/borderless behavior in this task.

Do not introduce resizing UI or runtime resize controls.

Do not change monitor selection or display mode.

PORTRAIT VIEWPORT

Update the normal portrait viewport to:

Width = 960
Height = 1280

and center it in the host window at:

X = 320
Y = 0

Preserve the existing 3:4 width-to-height aspect ratio.

Do not change the viewport to 4:3 landscape.

The portrait viewport is 3 units wide by 4 units high.

Do not alter portrait orientation.

UNIFORM HOST-SCALE RELATIONSHIP

The old viewport:

720 x 960

becomes:

960 x 1280

with uniform factor:

4 / 3

Preserve all internal logical coordinates and composition relationships so
that, before rasterization/host scaling, existing accepted content occupies
the same relative locations in the portrait viewport.

Where the current presentation explicitly maps logical portrait coordinates to
host pixels, update the host scale consistently.

Do not independently stretch X or Y.

Do not introduce anisotropic display scaling.

Do not use this milestone to make tubes narrower or taller relative to one
another.

LOGICAL PRESENTATION

Preserve the current logical portrait coordinate system unless changing it is
strictly required by existing architecture to produce the requested uniform
host enlargement.

Prefer:

same logical coordinate system
-> larger uniform host scale
-> 960 x 1280 centered viewport

rather than changing all logical presentation coordinates.

Do not alter Level-specific geometry or placement constants merely because host
pixels increase.

LEVEL PLACEMENT CONSTANTS ARE LOGICAL

Preserve all accepted logical presentation translations, including:

- Level 7 +228.99702 logical Y
- Level 8 +212.28139 logical Y
- Level 9 +180.96825 logical Y
- diagnostic Level 10 +50 logical Y

Do not multiply these constants by 4/3 in source if the final viewport camera
already performs the host-size scale.

The host-space result should increase naturally through the new uniform
viewport scaling.

Verify this explicitly with tests.

Do not retune individual levels.

SHARED CAMERA

Preserve the accepted portrait camera/presentation architecture.

Update only the host-size-dependent values necessary for the new viewport.

Do not change:

- projection K;
- ZOffset;
- portrait perspective;
- per-level presentation adapter inputs;
- geometry;
- near/far relationship;
- camera logical target;
- Level-specific logical placements.

If the portrait camera currently contains a hardcoded host scale derived from
720 x 960, refactor only enough to derive the correct equivalent scale for
960 x 1280.

The mathematical behavior must remain uniform.

HUD

The HUD remains part of the same portrait presentation.

Preserve:

- logical HUD coordinates;
- ordering;
- score/lives/status layout;
- separation between HUD and gameplay area.

The HUD should become uniformly larger in physical host pixels along with the
viewport.

Do not redesign or reposition HUD content in logical coordinates.

Do not start the future arcade HUD/layout milestone.

VIEWPORT BORDER

Preserve the accepted one-host-pixel blue outline around the portrait viewport.

Do NOT scale the border thickness by 4/3.

It remains one host pixel unless the current implementation establishes that
the border is part of logical content rather than a host-pixel diagnostic.

The accepted requirement is a one-host-pixel viewport outline.

Update its bounds to the new viewport:

left = 320
top = 0
right edge corresponding to viewport width 960
bottom edge corresponding to viewport height 1280

Preserve clipping/scissor correctness.

CLIPPING AND SCISSOR

Update all host-space clipping/scissor rectangles that intentionally correspond
to the portrait viewport.

Verify:

- nothing is clipped by the former 720 x 960 bounds;
- no playfield/HUD rendering leaks into the 320-pixel side margins;
- the viewport border remains visible;
- the full 1280-pixel vertical extent is available.

Do not expand tube geometry itself to fix clipping.

SCREENSHOT CAPTURE

Preserve existing screenshot behavior.

A normal full-window screenshot after this milestone should reflect the new:

1600 x 1280

host window.

If screenshot code intentionally captures only the portrait viewport, preserve
that semantic but update any host-size-dependent bounds to the new:

960 x 1280

viewport.

Do not introduce a new screenshot mode.

Do not alter recorded gameplay or screenshot naming conventions.

The future side-by-side comparison milestone will address presentation
proportions separately.

INPUT MAPPING

Verify mouse/spinner/controller-independent screen mappings are not accidentally
changed by host-window enlargement.

Gameplay spinner input is not supposed to depend on absolute viewport pixel
width unless current architecture explicitly maps host cursor position.

Do not change accepted spinner sensitivity.

If host-space pointer mapping exists, update only the viewport-bound conversion
needed to preserve equivalent logical input coordinates.

Do not retune gameplay controls.

WINDOW-DEPENDENT DIAGNOSTICS

Inspect existing display/camera/screenshot tests and any diagnostic code that
asserts:

- 1280 x 960;
- 720 x 960;
- viewport left = 280;
- output scale = 1.2;
- old host-space centers/bounds.

Classify each occurrence before changing it:

A. actual application/window contract;
B. portrait viewport contract;
C. derived host-space expectation;
D. historical report/comment;
E. unrelated legacy presentation dimension.

Update only A-C.

Do not rewrite historical AI-Logs.

Do not globally search-and-replace numbers without understanding their role.

LEGACY / OPTION-OFF BEHAVIOR

Determine the intended normal host-window behavior when --atari-presentation is
off.

The application window itself should use the requested 1600 x 1280 size unless
current architecture explicitly has separate window contracts.

Preserve legacy logical rendering behavior.

Do not change legacy projection mathematics.

If legacy presentation uses the full host window differently from the portrait
viewport, update its host bounds consistently without redesigning it.

Report the exact behavior.

GAMEPLAY AND SIMULATION PROTECTION

Do not change:

- gameplay geometry;
- lane topology;
- movement;
- collision;
- enemy behavior;
- timing;
- firing;
- score;
- lives;
- RNG;
- recording/playback;
- level progression;
- ExitZoom;
- Approach;
- ReverseZoom;
- input cadence.

This is a display-sizing milestone only.

FROZEN PRESENTATION PROTECTION

Preserve all closed presentation work:

- Levels 1-6;
- Level 7 reconstruction;
- Level 7 +228.99702 logical Y;
- Level 8 +212.28139 logical Y;
- Level 9 +180.96825 logical Y;
- diagnostic Level 10 +50 logical Y;
- Blaster outward orientation;
- Blaster ExitZoom/Approach scale;
- ExitZoom focal compensation;
- tube geometry;
- entity registration;
- shared perspective;
- HUD logical layout.

Do not reopen any individual level.

NO IMAGE ANALYSIS

No screenshot/image/video analysis is required or authorized for this
implementation.

Do not inspect comparison images.

Do not attempt to visually match MAME in this task.

The geometry is specified numerically:

Window = 1600 x 1280
Viewport = 960 x 1280
Viewport origin = (320,0)

CODE QUALITY

Prefer deriving viewport dimensions and origin from authoritative window size
and portrait aspect where the existing architecture cleanly supports it.

For example, if current architecture already has a viewport-layout helper,
update that authority rather than adding duplicated 960/1280/320 constants.

Do not over-engineer a general arbitrary aspect-ratio system unless one already
exists.

The requested dimensions may remain explicit accepted configuration values.

Use comments only where they preserve an important invariant:

- portrait viewport remains 3:4;
- viewport is centered;
- host enlargement is uniform;
- logical presentation remains unchanged.

TESTING REQUIREMENTS

Add/update tests proving at minimum:

A. WINDOW

1. Application window is 1600 x 1280.
2. Window center is (800,640).

B. VIEWPORT

3. Portrait viewport is 960 x 1280.
4. Viewport origin is (320,0).
5. Left margin is 320.
6. Right margin is 320.
7. Top margin is 0.
8. Bottom margin is 0.
9. Viewport remains exactly 3:4.
10. Viewport is horizontally and vertically centered in the host window.

C. UNIFORM SCALING

11. Old-to-new viewport scale is exactly 4/3 on X.
12. Old-to-new viewport scale is exactly 4/3 on Y.
13. Existing logical portrait points map to host coordinates by one uniform
    scale/translation.
14. No new X/Y anisotropy is introduced.
15. The logical portrait center maps to host center of the portrait viewport.

D. ACCEPTED PRESENTATION

16. Level 7 logical placement remains +228.99702.
17. Level 8 remains +212.28139.
18. Level 9 remains +180.96825.
19. Level 10 remains +50.
20. Representative tube logical coordinates are unchanged.
21. Representative entity logical anchors are unchanged.
22. Host coordinates differ only by the expected viewport enlargement
    transform.
23. Tube/entity registration remains invariant.

E. HUD

24. HUD logical coordinates remain unchanged.
25. HUD host coordinates scale uniformly.
26. HUD/gameplay relative composition remains unchanged.

F. BORDER / CLIPPING

27. Viewport border is one host pixel.
28. Border surrounds the 960 x 1280 viewport.
29. Scissor/clip uses the new viewport rectangle.
30. Playfield rendering cannot leak into the left/right margins.
31. Bottom of viewport is not clipped by old 960-pixel host height.

G. SCREENSHOT / DISPLAY

32. Full-window screenshot contract uses 1600 x 1280 where applicable.
33. Viewport-only capture uses 960 x 1280 where applicable.
34. No stale 1280 x 960 / 720 x 960 bounds remain in active portrait display
    code except intentionally preserved legacy/logical constants.

H. GAMEPLAY / DETERMINISM

35. Rendering does not modify gameplay state.
36. Recording/playback schema remains unchanged.
37. Deterministic playback remains unchanged.
38. Input behavior remains unchanged.
39. ExitZoom/Approach behavior remains unchanged.
40. Option-off behavior remains mechanically unchanged.

Do not use screenshot pixels as automated visual-fidelity tests.

VALIDATION

Run bounded focused tests covering:

- DisplayConfiguration;
- viewport layout;
- camera mapping;
- viewport border;
- screenshot capture bounds;
- host/logical coordinate mappings.

Run targeted presentation regressions covering:

- Levels 1-10;
- Level7AtariPresentationTests;
- Level8AtariPresentationTests;
- Level9AtariPresentationTests;
- DiagnosticLevel10Tests;
- AtariPresentationAdapterTests;
- VectorRendererAtariPresentationTests;
- Blaster presentation/scale;
- ExitZoomFocalCompensationTests;
- Approach/transition;
- recording/playback;
- screenshot capture;
- option-off/legacy display behavior.

Then run:

- full solution tests with explicit VSTest hang-timeout protection;
- Release solution build;
- git diff --check.

Do not weaken tests.

Canceled, blocked, stalled, timed-out, or incomplete commands are not PASS.

Do not launch the interactive game.

REPOSITORY SAFETY

Record before work:

- branch;
- HEAD;
- upstream/tracking;
- ahead/behind;
- index;
- tracked worktree;
- relevant untracked state.

Expected committed baseline:

7ba80f39df9daeaa86a7cafc81690f2f5c37bb33

on master tracking origin/master at 0/0.

Verify rather than assume.

Preserve unrelated local work, including if present:

- .github/copilot-instructions.md;
- Reference/arcade/MBUCOD.V05.txt;
- Reference/arcade/MBUDOC.DOC.

Do not reset, restore, clean, stash, rebase, switch branches, stage, commit, or
push.

This implementation stops before manual acceptance.

AI-LOGS

Follow .github/copilot-instructions.md.

Save:

- this substantive prompt under AI-Logs/Prompts/;
- the paired 1600x1280 window / 960x1280 viewport implementation report under
  AI-Logs/Reports/.

Do not stage or commit AI-Logs.

A chat response is not a substitute for the report.

MANUAL HANDOFF

After automated validation, STOP.

Do not launch the interactive game.

Provide the normal manual command:

dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation

and any additional established direct-level commands useful for bounded
regression checks.

Ask Neal to judge only this milestone:

- application window is the desired larger size;
- portrait viewport is visibly centered;
- left/right margins are equal;
- viewport uses the full window height;
- accepted tubes appear uniformly larger, not reshaped;
- HUD and tube composition remain proportionally identical;
- Blaster/entities remain registered;
- gameplay remains unchanged;
- no clipping;
- viewport border remains correct;
- exit/Approach remain correct.

Do not ask Neal to judge arcade/MAME proportion matching yet.

That is the next separate milestone.

Manual acceptance remains PENDING.

EXPECTED REPORT

The paired implementation report must include:

- exact starting repository state;
- exact files changed;
- authoritative old/new window dimensions;
- authoritative old/new viewport dimensions;
- exact viewport origin/margins;
- exact old/new host scale;
- logical coordinate system preservation;
- camera changes, if any;
- screenshot-capture impact;
- border/scissor changes;
- legacy/option-off behavior;
- accepted per-level placement preservation;
- deterministic gameplay/recording preservation;
- focused validation results;
- targeted regression results;
- full-suite result;
- Release build result;
- git diff --check result;
- ending Git state;
- exact manual command;
- known limitations;
- manual acceptance PENDING;
- exact AI-Logs prompt/report paths.

Do not stage, commit, or push.

Complete autonomously and stop with the enlarged-window/viewport candidate
ready for Neal's manual acceptance.

TEMPEST_WINDOW_1600X1280_VIEWPORT_960X1280_IMPLEMENTATION