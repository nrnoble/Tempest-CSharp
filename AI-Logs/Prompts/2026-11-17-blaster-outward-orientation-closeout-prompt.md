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
4. If exactly one complete TEMPEST_ token cannot be found, STOP before project
   or repository action and explain why.

TASK

Close out the manually accepted Blaster outward-orientation correction and
remove the temporary Blaster-brightness diagnostic experiment.

Neal's manual decision is:

1. The Blaster outward-orientation correction was correctly implemented and is
   manually accepted.

2. Brightness values below 1.0 visibly reduce brightness, but increasing above
   1.0 provides no useful visible improvement. Neal does not want to pursue a
   brightness change.

3. Preserve the existing Blaster yellow/current production brightness.

4. Remove the temporary --blaster-brightness diagnostic rather than retaining
   unnecessary CLI/configuration/test surface.

This task is cleanup, closeout, validation, documentation, commit, and push.

Do not pursue another brightness technique.

CONTROLLING IMPLEMENTATION REPORT

Use:

AI-Logs/Reports/2026-11-17-blaster-outward-orientation-and-brightness-report.md

That implementation established that the prior inward orientation came from
VectorRenderer.DrawPlayer always using one fixed perpendicular sign without
determining which side of the visible rim faced into the tube.

The accepted correction determines the tube-interior direction from the visible
near-to-far rail geometry and selects the opposite side for the Blaster body.

Preserve that correction.

MANUAL ACCEPTANCE

Record the following:

- Blaster outward orientation: PASSED.
- The Blaster now travels around the outside of the visible rim correctly.
- The correction remains correct through ordinary rim movement/direction
  changes as manually tested.
- No further orientation adjustment is desired.
- No brightness increase will be pursued.
- Existing production Blaster brightness/color remains accepted as-is for now.

Do not characterize the brightness experiment as a failure of gameplay or
presentation.

It served its diagnostic purpose and established that simple RGB multiplication
above the current value provides no worthwhile improvement.

REMOVE THE BRIGHTNESS DIAGNOSTIC

Remove all code that exists solely to support:

--blaster-brightness <multiplier>

Remove as applicable:

- CLI parsing;
- validation;
- duplicate-option handling;
- configuration/state fields;
- Program wiring;
- renderer multiplier plumbing;
- color multiplication/clamping logic used only by this diagnostic;
- DebugController support;
- visible-playback option handling specific to this diagnostic;
- README documentation for the option;
- tests whose only purpose is the brightness option;
- test seams/callbacks added solely to verify brightness multiplication IF they
  have no continuing value for the accepted orientation correction.

Do not remove a test seam if it remains useful and cleanly justified for
orientation geometry regression coverage.

After cleanup:

- there must be no --blaster-brightness CLI option;
- current Blaster production color must be the same color used at multiplier
  1.0 before cleanup;
- supplying --blaster-brightness must follow normal unknown/unsupported option
  behavior rather than silently doing something;
- recording/playback formats remain unchanged.

PRESERVE ORIENTATION FIX

Retain the geometry-derived outward-normal correction.

Preserve:

- existing Blaster rim anchor;
- player lane/edge identity;
- movement;
- spinner response;
- highlighted lane;
- Blaster artwork geometry;
- six-line artwork topology;
- ordinary size;
- firing;
- collision;
- gameplay state.

The Blaster body must continue to extend outside the visible tube during
--atari-presentation Playing.

Preserve the shared geometry approach that handles:

- closed tubes;
- open tubes;
- reflected presentation geometry;
- player movement direction changes.

Do not replace it with per-level hardcoded normal signs.

PRESENTATION SCOPE

Keep the accepted correction scoped to the appropriate Atari-presentation
Playing path established by the implementation.

Do not broaden this closeout into:

- Blaster exit rendering;
- Approach;
- transition presentation;
- death animation;
- legacy presentation;
- other unrelated Blaster fidelity work.

The separate Blaster-exit presentation defect remains separate unless it has
already independently been resolved by accepted work.

Do not investigate it here.

DOCUMENTATION

Update durable project documentation proportionally.

Inspect and update where appropriate:

- TEMPEST-DEVELOPMENT-NOTES.md
- TEMPEST-BEHAVIOR-SPEC.md

Record:

- ordinary Atari-presentation Blaster outward orientation was manually
  accepted;
- the previous inward-facing rim artwork was corrected by selecting the
  exterior side from visible rim/rail geometry;
- gameplay position/movement was not changed;
- brightness experimentation was concluded without a production brightness
  change;
- the temporary brightness diagnostic was removed;
- current Blaster color/intensity remains unchanged.

Preserve evidence classifications.

Do not describe the rendering correction as ROM-backed unless existing
evidence independently establishes that classification.

GAMEPLAY PROTECTION

Do not change:

- Blaster gameplay position;
- edge/lane movement;
- spinner input;
- speed;
- firing;
- shot slots;
- collision;
- death;
- lives;
- scoring;
- Super Zapper;
- enemies;
- timing;
- RNG;
- simulation;
- recording/playback.

Rendering must remain state-read-only.

ACCEPTED PRESENTATION PROTECTION

Preserve all accepted/frozen work, including:

- Levels 1-6;
- Level 8 +212.28139;
- Level 9 +180.96825;
- Level 10 +50;
- accepted tube geometry;
- accepted perspective/scale;
- accepted placement;
- entity registration.

Do not change Level 7-specific architecture or attempt to fix Level 7.

TEST CLEANUP

Remove tests dedicated solely to brightness behavior that no longer exists.

Retain or strengthen focused orientation coverage proving:

1. Blaster anchor remains unchanged.
2. Body extends outside a closed tube.
3. Body extends outside an open tube.
4. Reversing player movement does not put the body inside.
5. Reflected/winding presentation does not invert the body.
6. Levels 1, 8, and 9 remain correct.
7. Level 10 remains correct if it shares this Playing renderer.
8. Highlighted lane remains unchanged.
9. Blaster artwork topology/dimensions remain unchanged.
10. Gameplay state remains unchanged by rendering.
11. Ordinary non-Atari/option-off behavior remains unchanged.
12. Recording/playback simulation and formats remain unchanged.

Add one appropriate regression proving the removed --blaster-brightness option
is no longer recognized/supported according to normal CLI behavior if that can
be tested without introducing option-specific permanent complexity.

Remove stale test names, comments, and helper fields implying brightness
configuration still exists.

VALIDATION

After cleanup and durable documentation updates, run:

1. Focused Blaster presentation/orientation tests.

2. Relevant targeted regressions covering:
   - Levels 1-10 Atari presentation;
   - Blaster/player rendering;
   - open/closed geometry;
   - Level 8/9/10 frozen placement;
   - recording/playback;
   - Approach/transition/exit;
   - display/camera;
   - option-off behavior.

3. Full solution tests with explicit hang-timeout protection.

4. Release solution build.

5. git diff --check.

Do not weaken tests.

Canceled, stalled, blocked, timed-out or incomplete commands are not PASS.

Do not launch the interactive game. Manual orientation acceptance is already
complete.

REPOSITORY SAFETY

Before work record:

- branch;
- HEAD;
- upstream/tracking;
- ahead/behind;
- index;
- tracked worktree;
- relevant untracked state.

The controlling implementation report began from:

bcd79da63021821c4e31a2a646152a70ccc443e5

on master tracking origin/master at 0/0.

Verify current state.

Preserve unrelated local work, particularly the pre-existing:

- .github/copilot-instructions.md modification;
- Reference/arcade/MBUCOD.V05.txt;
- Reference/arcade/MBUDOC.DOC;
- screenshot000.png.

Do not reset, restore, clean, stash, rebase, or switch branches.

AI-LOGS

Follow .github/copilot-instructions.md.

Save this substantive closeout prompt under AI-Logs/Prompts/.

Save a paired Blaster-orientation closeout report under AI-Logs/Reports/.

Do not stage or commit AI-Logs.

STAGING

After validation passes, identify the exact surviving files belonging to the
accepted orientation fix and diagnostic cleanup.

Likely candidate files from the implementation report include:

- src/Tempest.Game/Rendering/VectorRenderer.cs
- src/Tempest.Game/Debug/DebugController.cs
- src/Tempest.Game/Program.cs
- tests/Tempest.Game.Tests/BlasterPresentationTests.cs
- tests/Tempest.Game.Tests/DebugControllerTests.cs
- README.md

Some of these may return completely to baseline after brightness cleanup.

Do not stage a file merely because the implementation task touched it.

Stage only files containing surviving orientation/cleanup changes plus durable
documentation actually updated.

Expected durable files may include:

- TEMPEST-DEVELOPMENT-NOTES.md
- TEMPEST-BEHAVIOR-SPEC.md

Before committing:

- inspect staged name/status;
- inspect complete staged diff;
- confirm every staged hunk belongs to this milestone;
- exclude unrelated .github/copilot-instructions.md;
- exclude local/reference material;
- exclude AI-Logs;
- run git diff --cached --check.

COMMIT AND PUSH

If:

- orientation acceptance is recorded;
- brightness diagnostic is fully removed;
- durable documentation is correct;
- all required validation passes;
- staging contains only this milestone;
- staged whitespace passes;

create one bounded commit with message:

Close out Blaster rim orientation

Push master to the existing configured upstream.

Do not alter remotes.

Verify after push:

- master remains active;
- origin/master remains upstream;
- ahead/behind is 0/0;
- HEAD is the new closeout commit;
- index is clean;
- no accepted milestone changes remain unstaged;
- unrelated local work remains preserved.

If commit or push cannot complete safely, report PARTIAL/BLOCKED rather than
forcing completion.

EXPECTED REPORT

The closeout report must include:

- manual orientation acceptance;
- decision not to pursue brightness changes;
- confirmation current Blaster color is unchanged;
- complete list of brightness diagnostic code removed;
- surviving orientation implementation;
- durable documentation changes;
- focused validation;
- targeted validation;
- full-suite validation;
- Release build;
- git diff --check;
- exact staged files;
- staged-diff review;
- git diff --cached --check;
- commit hash/message;
- push result;
- final Git/upstream state;
- unrelated work excluded;
- confirmation Levels 8/9/10 remain frozen;
- confirmation Level 7 was not changed;
- exact AI-Logs prompt/report paths.

Do not ask Neal questions during execution.

Do not pause for manual input.

Complete autonomously unless a genuine safety, validation, staging or push
blocker requires fail-closed termination.

TEMPEST_BLASTER_OUTWARD_ORIENTATION_CLOSEOUT