# Level 10 Atari-presentation milestone closeout

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, opening files, running commands, or taking any repository action, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context may appear after the user's token and do not invalidate it.

This check is intended to detect an incomplete or truncated user task, not appended agent context.

If exactly one complete TEMPEST_ token cannot be found, STOP before any project or repository action and explain why.

TASK: CLOSE OUT THE MANUALLY ACCEPTED LEVEL 10 ATARI-PRESENTATION MILESTONE

The current Level 10 Atari-presentation candidate has now passed Neal's manual acceptance.

MANUAL ACCEPTANCE RESULT

Neal replayed the existing genuine-geometry Level 10 recording using --atari-presentation and reports:

- Level 10 placement is perfect.
- Gameplay is perfect.
- No further bug regression was observed.
- The characteristic depth-dependent enemy slowdown problem is absent.
- The Level 10 candidate is approved for closeout.

The accepted placement includes the current final screen-space downward translation of +50 logical pixels. Do not retune it.

The preceding implementation report recorded automated validation as:

- Focused DiagnosticLevel10Tests: 4 passed, 0 failed, 0 skipped.
- Full regression suite: 1,595 passed, 0 failed, 0 skipped.
- Release build: succeeded with 0 warnings and 0 errors.
- git diff --check: passed.

Treat those numbers as historical results from the preceding implementation task. Re-run the appropriate closeout validation below rather than merely copying those results forward.

CLOSEOUT SCOPE

1. Inspect the current Git/worktree state before making changes.

2. Preserve the manually accepted Level 10 implementation exactly unless a closeout-only documentation or validation issue requires attention.

Do NOT:
- retune Level 10 placement;
- change its horizontal center;
- change projection/perspective;
- change tube geometry;
- change entity registration;
- change gameplay, enemy movement, timing, or simulation behavior;
- reopen the Level 7 slowdown investigation;
- modify accepted Levels 1-9;
- perform unrelated cleanup or refactoring.

3. Update the durable project documentation as appropriate, including TEMPEST-DEVELOPMENT-NOTES.md and TEMPEST-BEHAVIOR-SPEC.md where the accepted Level 10 milestone belongs.

Record clearly that:

- Level 10 --atari-presentation manual acceptance passed on 2026-11-17.
- Neal judged its placement perfect.
- Neal judged gameplay perfect.
- No further bug regression was observed.
- The depth-dependent enemy slowdown defect was absent during the accepted replay.
- The accepted Level 10 final screen-space vertical placement is +50 logical pixels.
- This Level 10 presentation is now frozen unless a future independently demonstrated regression requires reopening it.

Preserve the project's evidence classifications. Do not upgrade project decisions, diagnostics, manual observations, or reference-derived behavior into ROM-backed facts without evidence.

4. Preserve the substantial prompt and produce the corresponding closeout report under the established ignored AI-Logs/Prompts and AI-Logs/Reports convention.

The closeout report should record:
- work performed;
- files changed;
- Neal's manual acceptance;
- automated validation performed during closeout;
- final Git/worktree/staging state;
- commit hash and commit message if committed;
- push result if pushed;
- any genuinely unresolved issue, if one exists.

Do not rewrite prior historical reports.

5. Run bounded final validation appropriate to this accepted milestone.

At minimum run:
- focused DiagnosticLevel10Tests;
- full regression suite;
- Release build;
- git diff --check.

Use the project's normal explicit hang/time bounds. A canceled, timed-out, hung, stalled, blocked, or incomplete validation must NOT be reported as passed.

Do not launch the interactive game. Neal has already completed manual acceptance.

6. Review the final diff carefully before committing.

Ensure the commit contains only the intended Level 10 milestone and its durable documentation.

AI-Logs are ignored historical/working artifacts and must not be staged or committed.

Preserve unrelated pre-existing worktree changes rather than sweeping them into this commit.

If the Level 10 implementation files were previously untracked work in progress, verify carefully that all files actually required for the accepted Level 10 implementation and its tests are included in the intended commit while unrelated work remains excluded.

7. If and only if closeout validation passes and the intended Level 10 changes can be safely isolated, commit the accepted Level 10 milestone and push it to the existing configured remote.

Use a concise commit message appropriate to the completed Level 10 Atari-presentation milestone.

Do not amend or rewrite previous commits.

8. Finish with a concise closeout report stating:
- validation results;
- exact files committed;
- commit hash and message;
- push result;
- final tracked Git status;
- confirmation that Level 10 manual acceptance is recorded;
- confirmation that no interactive game was launched;
- any remaining unrelated/uncommitted work that was intentionally preserved.

IMPORTANT SAFETY / STOP CONDITION

If the intended Level 10 changes cannot be cleanly separated from unrelated existing work, or validation encounters a security block, hang, unexplained failure, or regression, DO NOT force the closeout and DO NOT commit questionable work.

Stop safely, preserve the worktree, and report exactly what prevented closeout and what Neal needs to do next.

Do not weaken Windows security or bypass Application Control.

This is a closeout milestone, not another Level 10 investigation.

TEMPEST_LEVEL10_ATARI_PRESENTATION_CLOSEOUT_20261117
