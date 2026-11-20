FAIL-CLOSED COMPLETENESS CHECK

Read this entire user-provided task before using tools, reading files, running
commands, modifying files, staging, committing, pushing, or taking repository
action.

Before anything else, locate the unique completion token near the end.

Require exactly one complete standalone token beginning with "TEMPEST_" on a
line by itself.

VS Code/agent-generated context after the user's token does not invalidate it.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before
any repository action.


TEMPEST PAUSED-PLAYBACK SCREENSHOT CAPTURE

Implement one small development-only playback-analysis feature:

While recorded gameplay playback is PAUSED, allow Neal to capture the current
rendered Tempest game window/content as a PNG image.

The screenshot represents exactly the paused simulation boundary being examined.

This is the final planned playback-inspection convenience before returning to
normal gameplay milestones.

Stop after automated validation for Neal's manual acceptance.

Do not stage.
Do not commit.
Do not push.

Do not begin Level 8/9 work.


ACTIVE REPOSITORY

Work only in:

C:\Users\nrnob\source\repos\Tempest


EXPECTED CLEAN BASELINE

Expected branch:

master

Expected HEAD and origin/master:

50be0b88c9f8f2f1ac9229db54dc0cfa7a292f50
Add reverse recorded playback scrubbing

Require:

- clean tracked worktree;
- clean index;
- master tracking origin/master;
- local HEAD matching origin/master;
- no unexplained nonignored untracked files.

Before modifying anything run:

git status --short --branch
git rev-parse HEAD
git log -1 --oneline
git rev-parse origin/master
git diff --stat
git diff --cached --stat
git ls-files --others --exclude-standard
git remote -v

If repository state differs unexpectedly, STOP rather than resetting, cleaning,
restoring, deleting, discarding, or overwriting anything.


READ FIRST

Read:

.github/copilot-instructions.md

README.md

TEMPEST-DEVELOPMENT-NOTES.md

TEMPEST-BEHAVIOR-SPEC.md

Read relevant reports:

AI-Logs/Reports/2026-11-07-visible-playback-controls-phase2-acceptance-closeout-report.md

AI-Logs/Reports/2026-11-07-headless-playback-runner-phase3a-acceptance-closeout-report.md

AI-Logs/Reports/2026-11-07-reverse-playback-scrubbing-phase3b-acceptance-closeout-report.md

Inspect current source/tests including:

Program.cs

InputSystem.cs

VisiblePlaybackController.cs

PlaybackOverlayStatus.cs

VectorRenderer.cs

GameplayPlaybackSession.cs

GameplayStateSummary.cs

and relevant playback/input/rendering tests.

Inspect the actual Raylib-cs 6 binding/API available to the project for:

- TakeScreenshot;
- Print Screen key representation if present;
- F12;
- key press-edge handling;
- any Raylib automatic screenshot-key behavior relevant to the installed build.

Current source and installed binding are authoritative.


USER REQUIREMENT

Neal wants a simple screenshot capability while examining a paused recording.

Typical workflow:

recorded playback
->
Space to pause
->
Left/Right and Shift+Left/Right to scrub to exact interesting boundary
->
screenshot
->
continue scrubbing.

The captured image should contain the rendered Tempest game window/content at
that exact paused boundary.

Neal does NOT need screenshot capture during ordinary real-time gameplay.

Neal does NOT need video capture.

Neal does NOT need continuous screenshots.

Neal does NOT need screenshots during headless playback.


RAYLIB CAPTURE MECHANISM

Prefer the installed Raylib/Raylib-cs screenshot facility if available,
conceptually:

TakeScreenshot(filename)

Verify actual API signature in the installed binding.

Do not implement custom framebuffer copying, Windows GDI capture, desktop
capture, or external screenshot tools if Raylib already provides the required
window/render capture.


PRINT SCREEN INVESTIGATION

Neal would ideally like to use the Print Screen key.

Investigate whether the current Raylib-cs/GLFW/Windows input stack exposes
Print Screen as a normal key that the game can detect reliably.

Also determine whether using Print Screen would still invoke Windows screenshot
behavior outside the game.

Do NOT:

- install a global keyboard hook;
- intercept Windows input globally;
- use low-level Windows keyboard APIs merely to suppress Print Screen;
- make platform-specific invasive changes.

If Print Screen works cleanly as an application key without undesirable OS
behavior, it may be used.

If Print Screen cannot be cleanly application-owned, use F12 instead.

F12 is an acceptable development-only screenshot key.

Prefer simplicity and reliability over Neal's first-choice key.

Report which key was selected and why.


RAYLIB BUILT-IN SCREENSHOT KEY

Inspect whether the installed Raylib build already has automatic screenshot-key
behavior, such as a built-in F12 screenshot mechanism.

If so, avoid double-capturing one key press.

Either:

- deliberately use the existing behavior if it meets all project requirements;
  or
- select/configure a project-owned key path that gives deterministic filename
  and directory control.

Do not accidentally create two screenshots from one press.


ACTIVE ONLY WHILE PAUSED FILE PLAYBACK

Screenshot capture is permitted only when:

- visible recorded-file playback is active;
- playback remains under VisiblePlaybackController;
- playback is paused.

It must NOT activate during:

- running recorded playback;
- ordinary live gameplay;
- headless playback;
- recording mode;
- built-in unrelated debug scenarios;
- after live-input handoff.

Do not globally turn the screenshot key into gameplay input.


SIMULATION MUST NOT MOVE

Taking a screenshot must NOT:

- call RunOneStep;
- consume a recorded frame;
- change GameplayPlaybackSession boundary;
- change recording cursor;
- change RNG state;
- resume playback;
- pause playback if it was not already paused;
- change game state;
- change PlayerInput;
- trigger Super Zapper;
- move the Blaster.

The screenshot is observational only.


EXACT PAUSED BOUNDARY

If paused at boundary N:

screenshot must depict boundary N.

Immediately after capture:

- session boundary remains N;
- recording cursor remains N;
- overlay remains N / Duration;
- subsequent Left/Right scrubbing continues normally.

No hidden simulation tick may occur merely to capture.


SCREENSHOT CONTENT

Capture the actual rendered Tempest game presentation currently shown inside the
game window.

The screenshot should include the current visible presentation, including the
PAUSED boundary/duration overlay if Raylib's capture ordering naturally includes
what is currently presented.

Investigate capture timing relative to:

BeginDrawing
render
overlay
EndDrawing
TakeScreenshot

Choose ordering deliberately so the PNG corresponds to what Neal is actually
examining.

Do not capture the Windows desktop, PowerShell window, VS Code, window chrome,
or another monitor.

Do not create a second renderer solely for screenshot output.


OUTPUT DIRECTORY

Save screenshots to:

Temp\Screenshots\

relative to the repository root.

This directory is disposable development output.

Verify it is ignored by Git through the existing Temp/ ignore policy.

Create the directory when first needed if it does not already exist.

Do not add screenshot PNG files to Git.

Do not stage them.

Do not commit them.


FILENAME

Use deterministic, informative filenames identifying the paused playback
location.

Prefer something conceptually similar to:

playback-level-7-boundary-004820.png

or, if recording name is useful and safely available:

levels1-7-level-7-boundary-004820.png

Use actual current displayed level and exact simulation boundary where
practical.

Requirements:

- `.png`;
- filesystem-safe;
- identifies boundary;
- avoids ambiguous overwrite;
- repeated screenshots at the same boundary must not silently destroy a previous
  screenshot.

If needed, add a simple sequence suffix:

levels1-7-level-7-boundary-004820-002.png

Do not use wall-clock timestamps unless they are actually helpful.

Prefer deterministic/debug-useful naming.


NO OVERWRITE

Do not silently overwrite an existing screenshot.

Choose the next free suffix or otherwise fail clearly.

Prefer automatic unique naming because repeated capture of one interesting
boundary is a realistic use case.


CAPTURE CONFIRMATION

Provide Neal a small, nonintrusive indication that a screenshot was successfully
saved.

Preferred possibilities:

- concise console line containing the relative filename;
- very brief development-only in-window confirmation if trivially available.

Do not add an elaborate notification system.

Console confirmation is sufficient.

Example concept:

Screenshot saved: Temp\Screenshots\levels1-7-level-7-boundary-004820.png

Do not allow confirmation state to alter simulation.


FAILURE BEHAVIOR

If screenshot capture fails:

- keep playback paused;
- keep boundary unchanged;
- report a useful error;
- do not crash gameplay merely because screenshot file output failed, unless the
  Raylib API itself produces an unavoidable fatal failure.

Prefer a clear development diagnostic.

Do not convert unrelated simulation errors into screenshot errors.


KEY INPUT SEPARATION

Screenshot command is a playback-analysis command.

It must remain outside:

- PlayerInput;
- recording frames;
- gameplay simulation;
- recording format;
- GameplayPlaybackSession.

The shared playback session must remain Raylib-independent.


PHASE 1/2/3 PRESERVATION

Preserve all accepted playback functionality:

SPACE
- pause/resume.

RIGHT tap/hold
- +1 scrubbing.

SHIFT+RIGHT tap/hold
- +5-group scrubbing.

LEFT tap/hold
- -1 deterministic reconstruction.

SHIFT+LEFT tap/hold
- -5 reconstruction.

PAUSED boundary/duration overlay.

Phase 3A headless runner.

Phase 3B replay-from-start reconstruction.

Live-input handoff.

Do not alter any accepted scrubbing behavior.


HEADLESS PRESERVATION

Headless playback must remain entirely unaffected.

Do not add screenshot dependencies to:

GameplayPlaybackSession

HeadlessGameplayPlaybackRunner

GameplayStateSummary

or the headless CLI.


NO RECORDING FORMAT CHANGE

Do not add screenshot metadata to recording files.

Do not add screenshot events to recorded input.

Do not add paths to recordings.

Recordings remain v1/v2 unchanged and read-only during playback.


NO SCREENSHOT DURING LIVE GAME

This milestone intentionally supports screenshot capture only while visible
recorded playback is paused.

Do not broaden the feature to normal gameplay.

If future general screenshots are desired, handle separately.


NO SCREENSHOT DURING RUNNING PLAYBACK

If the selected screenshot key is pressed while playback is running:

do nothing.

Do not automatically pause and capture.

Neal explicitly wants screenshots from deliberately paused analysis states.


TESTABLE SCREENSHOT SEAM

Avoid making testing depend on physically pressing a keyboard key or inspecting
a native PNG produced by a real GPU.

Introduce a narrow testable screenshot-request/file-naming seam if needed.

Tests should establish:

- screenshot allowed only when paused active playback;
- selected key requests one capture;
- running playback does not capture;
- live gameplay does not capture;
- boundary remains unchanged;
- cursor remains unchanged;
- deterministic filename generation;
- duplicate filenames receive unique suffix;
- output directory is Temp\Screenshots;
- capture command does not enter PlayerInput.

Do not build a screenshot framework.


PNG NATIVE CAPTURE TESTING

If Raylib TakeScreenshot cannot be unit tested without a live window, do not
manufacture brittle native tests.

Test the surrounding policy/naming/control seams automatically.

Neal will manually verify actual PNG output.


DOCUMENTATION

Update durable documentation with candidate status:

PAUSED-PLAYBACK SCREENSHOT CAPTURE:
IMPLEMENTED / MANUAL ACCEPTANCE PENDING.

Record:

- selected screenshot key and why;
- capture only while paused recorded playback is active;
- Raylib TakeScreenshot path;
- Temp\Screenshots output;
- boundary-aware filenames;
- no overwrite;
- no simulation/cursor advancement;
- PAUSED overlay capture behavior;
- debugger-control/PlayerInput separation;
- recording format unchanged;
- headless runner unchanged;
- gameplay unchanged.

Do not claim manual acceptance before Neal tests real screenshots.


DEFERRED ENEMY EXPLOSION

Do not implement the deferred enemy-destruction explosion.

It remains a later visual-fidelity milestone.


NO GAMEPLAY CHANGES

Do not change:

- Levels 1–7;
- enemies;
- Flipper behavior;
- Tankers/cargo;
- Spikes;
- score;
- firing;
- Super Zapper gameplay;
- death/re-entry;
- progression;
- Game Over;
- Avoid Spikes;
- direct-level startup;
- recording semantics;
- playback simulation semantics;
- audio behavior.


AI LOGS

Preserve this task as:

AI-Logs/Prompts/2026-11-07-paused-playback-screenshot-prompt.md

Create:

AI-Logs/Reports/2026-11-07-paused-playback-screenshot-report.md

Report must include:

1. Starting Git state.
2. Raylib screenshot API investigation.
3. Print Screen investigation.
4. Selected key and rationale.
5. Built-in Raylib screenshot-key interaction.
6. Capture timing/order.
7. Paused-only policy.
8. Boundary/cursor preservation.
9. Output directory.
10. Filename format.
11. Duplicate-name policy.
12. Confirmation behavior.
13. Failure behavior.
14. PlayerInput separation.
15. Recording-format preservation.
16. Headless preservation.
17. Phase 1/2/3 scrubbing preservation.
18. Production files changed.
19. Tests changed.
20. Documentation changed.
21. Focused test counts.
22. Neighboring regression counts.
23. Full-suite counts.
24. Build warnings/errors.
25. git diff --check.
26. Manual launch command.
27. Manual acceptance checklist.
28. Final Git state.
29. Confirmation no enemy-explosion implementation, gameplay change, recording
    change, snapshot, Level 8/9, stage, commit, or push occurred.


AUTOMATED VALIDATION

Run focused screenshot policy/naming/input tests.

Run neighboring playback/input/rendering regressions.

Run Phase 2/3 playback regressions.

Then run:

dotnet test TempestCSharp.slnx

dotnet build TempestCSharp.slnx

git diff --check

Require:

- tests actually execute;
- 0 failed;
- 0 unexpected skipped;
- build 0 errors;
- warning count accurately reported;
- diff check passes.


NO INTERACTIVE GAME LAUNCH

Copilot must not launch the game.

Neal performs real screenshot acceptance.

Do not stage.
Do not commit.
Do not push.


MANUAL TEST

Use the long recording:

dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\levels1-7.json

Ask Neal to:

1. pause with Space;
2. scrub to an interesting boundary;
3. press the selected screenshot key;
4. verify a PNG appears under Temp\Screenshots\;
5. open the PNG;
6. verify it matches the paused Tempest presentation;
7. verify PAUSED boundary/duration appears as intended;
8. verify screenshot filename identifies the boundary;
9. take another screenshot at the same boundary;
10. verify the first file was not overwritten;
11. verify the second receives a unique filename;
12. verify playback boundary did not move due to capture;
13. continue Left/Right scrubbing normally;
14. verify screenshot key during running playback does nothing;
15. verify normal live gameplay is unaffected after handoff.

No further gameplay testing is required.


FINAL GIT CHECK

Finish with:

git status --short --branch
git rev-parse HEAD
git rev-parse origin/master
git diff --stat
git diff --name-status
git diff
git diff --check
git diff --cached --stat
git ls-files --others --exclude-standard

Expected HEAD remains:

50be0b88c9f8f2f1ac9229db54dc0cfa7a292f50

Require candidate unstaged and uncommitted.


FINAL RESPONSE

Return:

1. Screenshot API.
2. Selected key.
3. Print Screen findings.
4. Capture timing.
5. Paused-only policy.
6. Output directory.
7. Filename scheme.
8. Duplicate policy.
9. Boundary/cursor preservation.
10. PAUSED overlay behavior.
11. Confirmation behavior.
12. Failure behavior.
13. PlayerInput separation.
14. Recording-format preservation.
15. Headless preservation.
16. Phase 1/2/3 preservation.
17. Production files changed.
18. Tests changed.
19. Documentation changed.
20. Focused counts.
21. Neighboring counts.
22. Full-suite counts.
23. Build warnings/errors.
24. git diff --check.
25. Manual launch command.
26. Manual acceptance checklist.
27. Report filename.
28. Final Git status.
29. Confirmation no enemy-explosion implementation, gameplay change, recording
    change, snapshot, Level 8/9, stage, commit, or push occurred.

Stop for Neal's manual acceptance.

TEMPEST_PAUSED_PLAYBACK_SCREENSHOT_COMPLETE