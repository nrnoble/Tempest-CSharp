READ THIS ENTIRE USER-PROVIDED TASK BEFORE TAKING ANY ACTION.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, reading files, running commands, or taking repository
action:

1. Locate the unique completion token near the end of this task.
2. Verify that exactly one complete token beginning with "TEMPEST_" appears on
   a line by itself.
3. Automatically appended VS Code/agent context after the token does not
   invalidate it.
4. If exactly one complete token cannot be found, STOP before repository action
   and explain why.

TASK

Close out the manually accepted ExitZoom focal-compensation correction.

Perform final validation, update durable documentation, stage only this
milestone, commit it, push master to the existing upstream, and verify the
final repository state.

Do not perform new implementation or tuning.

MANUAL ACCEPTANCE

Record Neal's authoritative manual result:

- The ExitZoom focal-point regression is fixed.
- Level 5 remains fixed.
- Level 8 now exits properly.
- Level 9 now exits properly.
- No further work is wanted on this bug.
- Ordinary Playing presentation remains correct.
- Gameplay remains normal.

Level 10 is diagnostic-only and is not required for this closeout.

Level 7 remains outside this milestone because its presentation has separate
unresolved problems.

ACCEPTED PRESENTATION RULE

Preserve the final implementation exactly:

CLOSED TUBES

- Use the mapped far-rim center as the ExitZoom focal pivot.
- Preserve the manually successful Level 5 behavior.
- Preserve working Levels 1-4 and Level 6.

OPEN TUBES

- Use the calculated common intersection of the mapped near-to-far rails.
- Do not use the terminal-endpoint midpoint.
- Do not use per-level focal constants.
- Calculate the convergence point from the actual mapped tube geometry.
- Apply the existing logical-space compensation:

  latchedPivotAtExitStart - currentMappedPivot

The compensation remains:

- active only during ExitZoom;
- calculated after the active presentation adapter and accepted per-level
  placement;
- applied before the common portrait host camera;
- shared by the tube, Blaster, Spikes, and tube-space presentation objects;
- absent from ordinary Playing and Approach.

Do not retune or rederive the accepted result.

EVIDENCE CLASSIFICATION

Document the focal rule accurately as a PROJECT TRANSLATION.

Do not claim that the ROM explicitly defines:

- the mapped closed far-rim center as the exit focal point;
- the mapped open rail-convergence point as the exit focal point;
- or the C# compensation architecture.

The correction is accepted because it resolves the demonstrated project
regression and passed Neal's manual testing.

CONTROLLING RECORDS

Inspect the current worktree and relevant AI-Logs, including:

- the original ExitZoom focal-compensation implementation report;
- the open-tube exit-pivot investigation report;
- the subsequent open-tube rail-convergence implementation prompt/report, if
  present;
- the current candidate source and tests.

Do not assume an implementation-report filename if it differs from the prompt
date or name. Locate the paired ignored report by the completion token or
substantive title if needed.

Do not modify historical reports.

FROZEN CORRECTIONS

Preserve the accepted Blaster ExitZoom/Approach scale correction.

Preserve:

- ordinary Blaster outward orientation;
- current Blaster yellow color;
- removed brightness diagnostic;
- fixed ordinary Playing Blaster size;
- accepted travel-state Blaster scale normalization;
- Blaster lane attachment;
- existing Approach behavior.

The focal compensation and Blaster scale correction address different
regressions. Do not merge or retune them.

FROZEN LEVEL PRESENTATION

Preserve:

- accepted Levels 1-6 Playing presentation;
- Level 8 final logical Y placement +212.28139;
- Level 9 final logical Y placement +180.96825;
- diagnostic Level 10 final logical Y placement +50;
- tube geometry;
- projection/perspective;
- entity registration;
- HUD placement;
- ordinary Playing coordinates.

Do not alter Level 7-specific:

- record-06 geometry;
- entity registration;
- centering;
- depth-motion behavior;
- Playing presentation architecture.

DURABLE DOCUMENTATION

Update the appropriate durable authorities:

- TEMPEST-DEVELOPMENT-NOTES.md
- TEMPEST-BEHAVIOR-SPEC.md

Record proportionally:

- the ExitZoom focal regression affected Level 5 and open Levels 8/9 during
  this milestone;
- closed tubes use the mapped far-rim-center compensation rule;
- open tubes use the mapped rail-convergence compensation rule;
- Level 5 passed manual testing;
- Levels 8 and 9 now exit properly;
- Neal considers the bug fixed and wants no further work;
- the correction is presentation-only;
- gameplay and exit simulation remain unchanged;
- accepted ordinary Playing placement remains unchanged;
- the accepted Blaster travel-scale correction remains unchanged;
- Level 10 remains diagnostic-only;
- Level 7 remains outside this acceptance.

Do not mark Level 7 ExitZoom as accepted.

NO NEW DEVELOPMENT

Do not:

- change focal rules;
- change residual tolerances without a validation necessity;
- retune per-level values;
- inspect screenshots or videos;
- open ROM binaries;
- add diagnostics;
- refactor unrelated renderer code;
- modify gameplay;
- modify exit cadence;
- modify Approach.

This is closeout only.

SOURCE AND TEST SCOPE

Determine the exact surviving candidate files from the current Git diff.

Expected candidate files may include:

- src/Tempest.Game/Rendering/VectorRenderer.cs
- src/Tempest.Game/Rendering/ExitZoomFocalCompensation.cs
- tests/Tempest.Game.Tests/ExitZoomFocalCompensationTests.cs

The final open-tube refinement may have modified those same files or added
another narrow test/helper file.

Do not stage a file merely because it is listed here.

Stage only files whose current diff belongs to the accepted focal-compensation
milestone.

REPOSITORY SAFETY

Before documentation edits or staging, record:

- branch;
- HEAD;
- upstream/tracking;
- ahead/behind;
- index;
- tracked worktree state;
- relevant untracked state.

Expected committed baseline beneath the candidate:

753728e79ed86624c9b1728dfd1c8db8f49d1e2b

Verify rather than assume.

Preserve unrelated local work, including if still present:

- .github/copilot-instructions.md
- Reference/arcade/MBUCOD.V05.txt
- Reference/arcade/MBUDOC.DOC

Do not reset, restore, clean, stash, rebase, or switch branches.

If candidate and unrelated changes cannot be safely separated, STOP with a
precise BLOCKED/PARTIAL report.

FINAL VALIDATION

After durable documentation is finalized, run bounded final validation.

At minimum run:

1. Focused ExitZoom focal-compensation tests covering:
   - closed far-rim-center pivot;
   - open rail-convergence pivot;
   - convergence residual/tolerance;
   - Levels 5, 8, and 9;
   - latch lifecycle;
   - tube/Blaster shared compensation;
   - state preservation.

2. Targeted regressions covering:
   - ExitZoom;
   - AvoidSpikes;
   - exit retry/Spike collision;
   - Blaster scale and presentation;
   - Levels 1-6;
   - Levels 8/9;
   - diagnostic Level 10;
   - AtariPresentationAdapter;
   - VectorRenderer;
   - Approach/transition;
   - recording/playback;
   - display/camera;
   - option-off/legacy behavior;
   - Level 7 Playing protection.

3. Full solution test suite with explicit VSTest hang-timeout protection.

4. Release solution build.

5. git diff --check.

Use existing validated filters where practical.

Do not weaken tests.

Canceled, stalled, blocked, timed-out, or incomplete commands are not PASS.

Do not launch the interactive game. Manual acceptance is already complete.

FINAL TEST EXPECTATIONS

Ensure automated evidence continues to establish:

- Level 5 retains closed far-rim-center behavior.
- Level 8 uses mapped rail convergence.
- Level 9 uses mapped rail convergence.
- Open-tube convergence is calculated, not hardcoded.
- Terminal midpoint is not used as the open focal pivot.
- Tube and Blaster receive identical compensation.
- Blaster local size/orientation remains unchanged.
- Ordinary Playing coordinates remain unchanged.
- Approach does not retain ExitZoom compensation.
- Retry/reset cannot reuse stale pivot state.
- Gameplay state remains read-only during rendering.
- Recording/playback schemas remain unchanged.
- Level 8 placement remains +212.28139.
- Level 9 placement remains +180.96825.
- Level 10 placement remains +50.
- Level 7-specific architecture remains unchanged.

STAGING

After all required validation passes:

1. Stage only the accepted focal-compensation source/test files.
2. Stage the durable documentation files actually updated.
3. Do not stage unrelated local work.
4. Do not stage AI-Logs.
5. Do not stage local references, screenshots, recordings, TEMP, or UserData.

Before committing:

- inspect `git diff --cached --name-status`;
- inspect the complete staged diff;
- verify every staged hunk belongs to this milestone;
- run `git diff --cached --check`.

Expected durable documentation files:

- TEMPEST-DEVELOPMENT-NOTES.md
- TEMPEST-BEHAVIOR-SPEC.md

Do not automatically stage `.github/copilot-instructions.md`.

AI-LOGS

Follow `.github/copilot-instructions.md`.

Save:

- this substantive closeout prompt under `AI-Logs/Prompts/`;
- a paired ExitZoom focal-compensation closeout report under
  `AI-Logs/Reports/`.

AI-Logs remain ignored and unstaged.

A chat-only summary is not a substitute for the report file.

COMMIT

If and only if:

- manual acceptance is recorded accurately;
- durable documentation is correct;
- focused validation passes;
- targeted validation passes;
- full suite passes;
- Release build passes;
- unstaged whitespace check passes;
- staging contains only this milestone;
- staged whitespace check passes;

create one bounded commit with message:

Close out ExitZoom focal compensation

Do not amend or rewrite existing history.

PUSH

Push master to the existing configured upstream.

Do not add or change remotes.

After push verify:

- branch remains master;
- upstream remains origin/master;
- HEAD is the new closeout commit;
- ahead/behind is 0/0;
- index is clean;
- no accepted milestone changes remain unstaged;
- unrelated local work remains preserved.

If commit or push cannot complete safely, report PARTIAL/BLOCKED rather than
forcing completion or repeatedly retrying an unchanged failing operation.

EXPECTED REPORT

The paired closeout report must include:

- exact manual acceptance statement;
- final closed-tube pivot rule;
- final open-tube pivot rule;
- open convergence algorithm and tolerance;
- evidence classification;
- durable documentation changes;
- exact final validation commands and results;
- focused test count;
- targeted test count;
- full-suite count;
- Release build result;
- git diff --check result;
- exact staged file list;
- staged-diff review result;
- git diff --cached --check result;
- commit hash;
- commit message;
- push result;
- upstream and ahead/behind verification;
- final branch/HEAD/index/worktree state;
- unrelated local work excluded;
- confirmation Level 5 remains fixed;
- confirmation Levels 8 and 9 are manually accepted for exit;
- confirmation Level 10 remains diagnostic-only;
- confirmation Level 7 ExitZoom was not accepted or reconstructed;
- confirmation accepted Blaster behavior remains frozen;
- exact AI-Logs prompt/report paths.

Do not ask Neal questions during execution.

Do not pause for manual input.

Complete autonomously unless a genuine validation, staging, safety, commit, or
push blocker requires fail-closed termination.

TEMPEST_EXITZOOM_FOCAL_COMPENSATION_CLOSEOUT