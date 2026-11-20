IMPORTANT COMPLETENESS CHECK

Read this entire user-provided task before taking any action.

Before using tools, reading project files, running commands, modifying files, or performing repository actions, locate the unique completion token near the end of this task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

Automatically appended VS Code/agent context after the user's token does not invalidate the token.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project or repository action.


AUTOMATIC / ACTIVE-EDITOR CONTEXT IS NOT AUTHORIZATION

Automatically surfaced active/recent/editor files are not authorized merely because VS Code supplied them.

Do not use such files unless this task explicitly authorizes them or they are directly necessary to this bounded implementation.

DO NOT read TEMP/myNotes.md.


TASK

Add a thin blue diagnostic outline showing the exact current portrait gameplay viewport inside the 1280 x 960 host window.

This is a tiny independent presentation/diagnostic milestone.

The purpose is to give Neal an immediately visible reference during manual presentation calibration for:

- actual portrait viewport boundaries;
- left/right margins;
- top/bottom boundaries;
- tube placement;
- HUD space;
- clipping;
- future perspective comparison.

Do NOT change the viewport itself.

Do NOT change any projection or presentation geometry.

Do NOT implement ROM-backed presentation metadata in this milestone.

Do NOT stage, commit, or push.


CURRENT AUTHORITATIVE STATE

Baseline remains:

branch:
master

HEAD:
385f603a94391756b2ca6cccd524dc17b281e782

commit:
Close out ROM-backed enemy-shot rendering

origin/master should match HEAD.

The current portrait/perspective candidate remains intentionally modified and unstaged.

Before implementation inspect:

- branch;
- HEAD;
- origin/master;
- index;
- tracked worktree;
- non-ignored untracked files;
- diff summary.

Compare current state with:

AI-Logs/Reports/2026-11-14-whole-game-presentation-human-visible-perspective-calibration-report.md

AI-Logs/Reports/2026-11-14-rev2-presentation-mathematical-translation-report.md

Expected:

- clean index;
- current candidate work only;
- no unrelated user work.

If unrelated unexpected work exists, STOP.

Do not restore/reset/clean/discard any existing candidate changes.


CURRENT VIEWPORT

The current experimental portrait presentation uses:

host:
1280 x 960

portrait viewport:
720 x 960

host-space viewport origin:
x = 280
y = 0

portrait logical presentation:
600 x 800

output scale:
1.2

The portrait viewport dimensions and placement are already established project translations.

Do not change them.


IMPLEMENTATION

Draw exactly one thin blue outline around the actual 720 x 960 portrait viewport.

Requirements:

- exactly follows the existing viewport boundary;
- derives its rectangle from the existing viewport/configuration values rather than introducing duplicate magic dimensions;
- thin and unobtrusive;
- blue;
- visible against the black host/window background;
- presentation-only;
- no gameplay effect;
- no projection effect;
- no camera effect;
- no clipping/scissor effect;
- no coordinate-system changes;
- no viewport-size changes;
- no HUD positioning changes;
- no tube placement changes;
- no perspective changes.

Prefer one logical/device-pixel-like outline appropriate to the current Raylib rendering path.

Do not introduce antialiasing, glow, decorative framing, thick borders, labels, measurements, or calibration marks.

This is only a boundary indicator.


DRAW ORDER

Choose the smallest safe draw location such that:

- the border represents the final host-space portrait viewport;
- the border itself is not transformed by the playfield camera;
- the border itself is not transformed by the HUD camera;
- the border is not accidentally clipped away by the portrait scissor;
- the border remains visible after normal playfield/HUD rendering;
- the border does not obscure gameplay more than the minimal outline requires.

Inspect current BeginScissorMode / EndScissorMode and camera ordering before implementing.

Do not restructure the renderer merely to add the border.


LEGACY PRESENTATION

--legacy-presentation is the established A/B reference and does not use the portrait viewport.

Do NOT draw the portrait viewport border in legacy presentation.

The border is for the experimental portrait presentation only.


ALL PORTRAIT MODES

Where the same experimental portrait viewport is used, the border should remain consistent during:

- normal play;
- direct-level startup;
- visible deterministic playback;
- recorded gameplay playback;
- Approach;
- ExitZoom;
- ReverseZoom;
- death presentation;
- Game Over presentation where the portrait viewport remains active.

Do not create state-specific borders.

Do not alter simulation behavior.


NO NEW COMMAND-LINE OPTION

Do not add a border toggle in this milestone.

The border is currently a development/presentation aid that Neal explicitly wants visible during portrait calibration.

If later the viewport is accepted and the border is no longer wanted, removal/toggle behavior can be handled separately.

Keep this implementation minimal.


FROZEN PRESENTATION

Do not change:

- 1280 x 960 host;
- 720 x 960 viewport;
- 600 x 800 portrait logical space;
- output scale 1.2;
- playfield scale 1.05;
- shared vertical placement -33.1;
- endpoint-normalized perspective formula;
- --portrait-perspective behavior;
- current perspective default;
- K;
- ZOffset;
- horizontal placement;
- HUD camera/layout;
- clipping/scissor rectangle;
- gameplay/model geometry;
- simulation;
- transitions;
- playback.

Do not modify any ROM-related code or data.


NO VISUAL-ASSET ANALYSIS

Do not inspect:

- screenshots;
- images;
- videos;
- MAME frames;
- gameplay recordings;
- Gemini material;
- Incitti source;
- web sources;
- raw ROM files.

This is a straightforward current-source implementation.


TESTS

Add/update only narrowly relevant tests.

Verify where practical:

- portrait viewport remains exactly 720 x 960 at host x=280, y=0;
- border rectangle uses the same viewport configuration rather than independent hard-coded dimensions;
- border is portrait-only;
- legacy remains unchanged;
- drawing the border does not alter simulation state;
- the border draw path occurs outside relevant world/HUD transforms and scissor behavior if testable through existing architecture.

Do not create a large new renderer-testing framework merely to test a one-line diagnostic border.

If exact draw-call testing would require disproportionate infrastructure, test the configuration/presentation predicate and verify the render placement through focused source inspection/reporting.


BOUNDED VALIDATION

Run focused tests directly affected by the viewport/border change with an explicit VSTest hang timeout no greater than 3 minutes.

If focused validation passes:

1. run full test suite with explicit 3-minute VSTest hang timeout;
2. run Release build;
3. run git diff --check.

Do not repeatedly rerun unchanged failures.

Do not use stale binaries after failed builds.

Follow established security/stall self-stop policy.


MANUAL HANDOFF

Do not launch the interactive game.

Provide Neal:

Experimental portrait Level 9:

dotnet run --project .\src\Tempest.Game -- --level 9 --portrait-perspective 125

Neutral portrait Level 9:

dotnet run --project .\src\Tempest.Game -- --level 9 --portrait-perspective 0

Legacy Level 9:

dotnet run --project .\src\Tempest.Game -- --level 9 --legacy-presentation

Expected manual result:

- portrait mode shows one thin blue rectangle outlining the actual gameplay viewport;
- left and right host margins are outside that rectangle;
- top and bottom boundaries are obvious;
- gameplay/perspective/placement are unchanged;
- legacy shows no portrait border.


AI LOGGING

Preserve this prompt under AI-Logs/Prompts.

Create:

AI-Logs/Reports/2026-11-14-portrait-viewport-border-report.md

Include:

- starting Git state;
- source inspected;
- files changed;
- exact border color/thickness;
- rectangle derivation;
- render/draw-order location;
- scissor/camera treatment;
- portrait-only behavior;
- legacy behavior;
- focused tests;
- full suite;
- Release build;
- git diff --check;
- manual commands;
- final Git state;
- confirmation nothing staged/committed/pushed;
- confirmation no visual/prohibited material was inspected.


NO DURABLE DOCUMENTATION UPDATE

Do not update TEMPEST-DEVELOPMENT-NOTES.md or TEMPEST-BEHAVIOR-SPEC.md.

This border is a development/presentation aid, not accepted gameplay behavior.


NO COMMIT OR PUSH

Do not stage.
Do not commit.
Do not push.


FINAL RESPONSE

Report:

1. Whether the viewport-border milestone completed.
2. Starting/final Git state.
3. Files changed.
4. Border rectangle.
5. Color and thickness.
6. How rectangle derives from existing viewport configuration.
7. Draw order.
8. Scissor/camera treatment.
9. Portrait-only behavior.
10. Legacy behavior.
11. Focused tests.
12. Full suite.
13. Release build.
14. git diff --check.
15. Exact manual commands.
16. Report path.
17. Confirmation nothing staged, committed, or pushed.
18. Confirmation no visual/prohibited sources were inspected.

Stop after automated validation.

TEMPEST_PORTRAIT_VIEWPORT_BORDER_20261114
