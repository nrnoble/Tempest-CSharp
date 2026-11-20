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
4. This check detects an incomplete/truncated user task, not appended agent
   context.
5. If exactly one complete TEMPEST_ token cannot be found, STOP before project
   or repository action and explain why.

TASK

Close out the manually accepted shared Blaster Exit/Approach scale correction.

Neal has completed manual testing and explicitly reports:

- the Blaster tube-exit defect is fixed on all tested levels;
- the extreme exit stretching is gone;
- no further work is needed on this defect.

Record that manual acceptance, preserve the accepted implementation without
further tuning, update durable documentation, perform final closeout
validation, stage only the bounded milestone, commit, push, and verify the
final repository state.

Do not perform new Blaster development.

CONTROLLING IMPLEMENTATION

Use:

AI-Logs/Reports/2026-11-17-blaster-exit-approach-scale-correction-report.md

Preserve the implementation described there.

The accepted presentation contract is:

BlasterHeight =
    20
    * CurrentAdapterLocalScale(
        live Blaster depth,
        active presentation offsets,
        active K/ZOffset)
    / StableGameplayRimAdapterLocalScale(
        normal gameplay rim,
        no Approach offsets,
        baseline K=70,
        baseline ZOffset=5)

Both values come through the applicable presentation adapter.

The stable baseline is evaluated without mutating live camera state.

Do not simplify, retune, replace, or reinterpret the accepted relationship.

MANUAL ACCEPTANCE

Record Neal's manual result as authoritative project acceptance:

- Blaster exit presentation: PASSED.
- The previous extreme Blaster stretching during tube exit is fixed.
- The correction works across all manually tested levels.
- No further work is wanted on this defect.

Preserve the accepted ordinary Playing Blaster orientation and current yellow
color/intensity.

Do not reopen brightness.

FREEZE THIS CORRECTION

After successful closeout, treat this shared Blaster travel-scale correction as
accepted/frozen.

Do not retune:

- ordinary Playing size;
- exit size;
- Approach size;
- local-scale normalization;
- stable baseline;
- exit camera;
- Approach camera;
- tube geometry;
- tube projection;
- accepted level placements.

Future changes require a separately demonstrated regression or explicitly
authorized milestone.

DURABLE DOCUMENTATION

Update the appropriate durable project documentation, including where
appropriate:

- TEMPEST-DEVELOPMENT-NOTES.md
- TEMPEST-BEHAVIOR-SPEC.md

Record proportionally:

- the shared Blaster ExitZoom/Approach scale regression was corrected;
- manual acceptance passed across all tested levels;
- the prior extreme exit stretching is gone;
- no further work is requested;
- the correction is presentation-only;
- gameplay/exit simulation was unchanged;
- ordinary Playing orientation remains accepted;
- the brightness diagnostic remains removed;
- accepted Level 8/9/10 placements remain unchanged.

Preserve evidence classifications.

Do not claim this scale relationship is literal Atari ROM/hardware behavior
unless existing evidence independently establishes that.

The accepted correction is a project presentation correction based on the
established regression and project architecture.

NO NEW INVESTIGATION

Do not:

- inspect screenshots;
- inspect videos;
- reopen the previous visual frames;
- search for alternative scale formulas;
- reopen exit architecture;
- retune Approach;
- touch Level 7 reconstruction;
- adjust tube geometry;
- adjust camera values;
- add diagnostics;
- refactor unrelated code.

This is closeout only.

PRESERVE IMPLEMENTATION FILES

The implementation report identifies:

- src/Tempest.Game/Rendering/VectorRenderer.cs
- src/Tempest.Game/Rendering/AtariPresentationAdapter.cs
- src/Tempest.Game/Rendering/Projection.cs
- tests/Tempest.Game.Tests/BlasterScaleRegressionTests.cs

Preserve the accepted changes in these files.

Do not stage a file merely because it is listed here; verify its current diff
belongs to this milestone.

GAMEPLAY PROTECTION

Do not change:

- BeginExit;
- UpdateExitZoom;
- UpdateExitSimulation;
- CompleteSuccessfulExit;
- UpdateReverseZoom;
- LevelApproach;
- player depth;
- lane/edge state;
- exit timing;
- K/ZOffset update cadence;
- Approach timing;
- Approach offsets;
- transition stars;
- gameplay geometry;
- collision;
- score;
- lives;
- RNG;
- recording/playback.

This closeout must not alter simulation behavior.

ACCEPTED PRESENTATION PROTECTION

Preserve accepted/frozen presentation behavior including:

- Levels 1-6;
- Level 8 +212.28139 logical Y;
- Level 9 +180.96825 logical Y;
- Level 10 +50 logical Y;
- accepted ordinary Blaster outward orientation;
- existing tube geometry and projection;
- shared perspective;
- HUD placement.

Do not modify Level 7-specific architecture.

REPOSITORY SAFETY

Before editing documentation or staging anything, inspect:

- branch;
- HEAD;
- upstream/tracking;
- ahead/behind;
- index;
- tracked worktree;
- relevant untracked files.

Expected committed baseline before this candidate:

34eedf0cf996cfe660494e34bdff2a1499696068

on master tracking origin/master at 0/0.

Verify current state rather than assuming it.

Expected candidate source/test changes are those identified in the controlling
implementation report.

Preserve unrelated local work, including the pre-existing:

- .github/copilot-instructions.md modification;
- Reference/arcade/MBUCOD.V05.txt;
- Reference/arcade/MBUDOC.DOC.

The implementation report recorded an unrelated workspace discrepancy:
screenshot000.png existed at its starting-state check but was later absent for
an unknown reason. The implementation task did not intentionally modify,
restore, or delete it.

Do not attempt to restore, delete, recreate, stage, or otherwise "fix"
screenshot000.png during this closeout.

Record its current state factually.

Do not reset, restore, clean, stash, rebase, or switch branches.

CLOSEOUT VALIDATION

After durable documentation is finalized, run bounded final validation.

At minimum:

1. Focused Blaster scale/presentation tests.

2. Relevant targeted regressions covering:
   - Blaster/player rendering;
   - Level 1 exit;
   - representative enclosed/open exits;
   - Levels 8/9/10;
   - Atari presentation adapters;
   - Approach;
   - transition;
   - ReverseZoom;
   - display/camera;
   - recording/playback;
   - option-off/legacy behavior.

3. Full solution tests with explicit hang-timeout protection.

4. Release solution build.

5. git diff --check.

Use the existing validated filters from the implementation task where
appropriate.

Do not weaken valid tests.

Canceled, stalled, blocked, timed-out, or incomplete commands are not PASS.

Do not launch the interactive game. Manual acceptance is already complete.

STAGING

After validation passes, stage only the accepted Blaster Exit/Approach scale
milestone plus its durable documentation.

Candidate implementation/test files are expected to include:

- src/Tempest.Game/Rendering/VectorRenderer.cs
- src/Tempest.Game/Rendering/AtariPresentationAdapter.cs
- src/Tempest.Game/Rendering/Projection.cs
- tests/Tempest.Game.Tests/BlasterScaleRegressionTests.cs

Also stage the durable documentation files actually updated, expected to
include:

- TEMPEST-DEVELOPMENT-NOTES.md
- TEMPEST-BEHAVIOR-SPEC.md

Do NOT stage:

- .github/copilot-instructions.md;
- local ROM/reference material;
- screenshots;
- UserData;
- TEMP;
- AI-Logs;
- unrelated changes.

Before committing:

- inspect git diff --cached --name-status;
- inspect the complete staged diff;
- verify every staged hunk belongs to this milestone;
- run git diff --cached --check.

If unrelated changes are inseparably mixed into a candidate file, STOP rather
than including them blindly.

AI-LOGS

Follow .github/copilot-instructions.md.

Save:

- this closeout prompt under AI-Logs/Prompts/;
- a paired Blaster Exit/Approach closeout report under AI-Logs/Reports/.

Do not stage or commit AI-Logs.

A chat response is not a substitute for the report.

COMMIT

If and only if:

- manual acceptance is recorded;
- documentation is correct;
- required validation passes;
- staging contains only the accepted milestone;
- staged whitespace passes;

create one bounded commit with message:

Close out Blaster exit presentation

Do not amend or rewrite history.

PUSH

Push master to the existing configured upstream.

Do not create or modify remotes.

After push verify:

- branch is master;
- upstream is origin/master;
- ahead/behind is 0/0;
- HEAD is the new closeout commit;
- index is clean;
- no accepted milestone changes remain unstaged;
- unrelated local work remains preserved.

If push fails, diagnose without repeated unchanged retries and report the
status as PARTIAL/BLOCKED.

EXPECTED REPORT

The paired closeout report must include:

- exact manual acceptance statement;
- accepted scale contract;
- frozen status;
- durable documentation changes;
- confirmation gameplay/exit simulation remained unchanged;
- focused validation command/result;
- targeted validation command/result;
- full-suite result;
- Release build result;
- git diff --check result;
- exact staged file list;
- staged-diff review;
- git diff --cached --check result;
- commit hash/message;
- push result;
- upstream verification;
- final branch/HEAD/index/worktree state;
- unrelated work excluded;
- factual current status of screenshot000.png without modifying it;
- confirmation Levels 8/9/10 remain frozen;
- confirmation ordinary Blaster orientation remains accepted;
- confirmation brightness diagnostic remains removed;
- confirmation Level 7-specific architecture was not changed;
- exact AI-Logs prompt/report paths.

Do not ask Neal questions during execution.

Do not pause for manual input.

Complete autonomously unless a genuine safety, validation, staging, or push
blocker requires fail-closed termination.

TEMPEST_BLASTER_EXIT_PRESENTATION_CLOSEOUT