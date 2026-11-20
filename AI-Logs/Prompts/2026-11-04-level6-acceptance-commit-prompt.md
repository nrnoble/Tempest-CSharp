IMPORTANT FAIL-CLOSED INSTRUCTION

Read this entire user-provided task before doing anything.

Before using any tools, reading project files, running commands, or changing the repository, locate the unique completion token included near the end of this user-provided task.

The completion token begins with "TEMPEST_" and appears on a line by itself.

Verify that exactly one complete token beginning with "TEMPEST_" is present in the user-provided task.

VS Code or the agent environment may append reminders, tagged-file information, attachment metadata, or other system-generated context after the user-provided task. Such automatically appended material does not invalidate the completion token.

The purpose of this check is to detect an incomplete or truncated user-provided task, not to reject context automatically appended by the agent environment.

If you cannot find exactly one complete token beginning with "TEMPEST_" in the user-provided task, STOP immediately without using tools or taking project/repository action and explain why.

PROJECT

Repository:
C:\Users\nrnob\OneDrive\Repro\Tempest

Starting clean checkpoint:
dbf00313b609f81ff26860fd0fcdf0bd0f46c3b0

CURRENT STATE

The Level 6 implementation is currently uncommitted.

The implementation agent previously reported:

- focused validation: 86 tests passed
- full solution: 719 tests passed
- build: 0 warnings, 0 errors
- git diff --check: passed
- no interactive game was launched
- no commit or push was performed

Neal has now completed manual acceptance.

MANUAL LEVEL 6 ACCEPTANCE

Neal played Level 6 multiple times.

Result:

LEVEL 6 MANUAL ACCEPTANCE: PASS

Neal reports:

- Level 6 plays without issues.
- No gameplay problem was found during multiple manual playthroughs.
- Level 6 is accepted as the next production campaign milestone.

Do not reopen or retune Level 6 merely because additional improvements are possible.

DEFERRED AUDIO ISSUE

During manual gameplay Neal observed a separate, non-blocking issue that applies across current levels rather than specifically to Level 6.

Sound-effect volume balancing needs a future refinement.

The sound Neal has specifically identified is the enemy-destruction/explosion effect played when a Blaster shot destroys an enemy. It is significantly too loud relative to the other effects.

It may be the only sound requiring adjustment, but that has not yet been established.

Record this conservatively:

- enemy-destruction sound volume is known to be too loud relative to other effects;
- broader sound-effect volume balance should be reviewed when this deferred issue is addressed;
- do not assume additional individual sounds are incorrect until manually identified;
- this is non-blocking;
- Windows volume adjustment is an acceptable temporary workaround;
- do not modify audio code, WAV files, gain, pitch, gameplay timing, or event timing during this Level 6 closeout.

STEP 1 - VERIFY THE REPOSITORY

Run:

Set-Location 'C:\Users\nrnob\OneDrive\Repro\Tempest'
git rev-parse HEAD
git status --short --branch
git diff --stat
git diff --cached --stat
git diff --check

HEAD must still be exactly:

dbf00313b609f81ff26860fd0fcdf0bd0f46c3b0

Review the existing uncommitted Level 6 implementation.

Confirm there are no unrelated tracked changes.

The expected changes should be limited to the already-implemented Level 6 production milestone, its tests, and its documentation.

Do not discard, rewrite, or broadly refactor the accepted implementation.

If unrelated tracked changes are found, STOP before committing and report them.

STEP 2 - RECORD MANUAL ACCEPTANCE

Update the relevant current Level 6 sections of:

TEMPEST-DEVELOPMENT-NOTES.md
TEMPEST-BEHAVIOR-SPEC.md
README.md if the existing Level 6 status there requires reconciliation

Record that Neal manually played Level 6 multiple times and accepted it with no gameplay issues observed.

Remove or supersede statements saying Level 6 manual acceptance is pending.

Preserve evidence classifications.

Manual acceptance of the C# recreation does not automatically make implementation details ARCADE VERIFIED.

Do not claim that exact Level 6 timing, random composition, Spiker speed, firing constants, internal accounting, or other hidden mechanics were arcade-verified merely because the overall gameplay passed manual acceptance.

Record the Level 6 production foundation as accepted.

Level 7 remains unimplemented.

STEP 3 - RECORD THE DEFERRED AUDIO ISSUE

Add a narrow deferred audio-fidelity note to the appropriate development documentation.

Record:

- Sound-effect volume balance requires future review.
- Neal specifically identified the enemy-destruction/explosion effect triggered when a Blaster shot destroys an enemy as significantly too loud relative to other effects.
- It is not yet established whether any additional sound effects require adjustment.
- The issue occurs across the current game rather than being Level-6-specific.
- The issue is non-blocking.
- No audio change is authorized during this closeout.

Do not modify AudioSystem, audio events, WAV assets, gameplay code, or tests merely to address this deferred issue.

STEP 4 - FINAL VALIDATION

Because this checkpoint contains substantial production Level 6 changes, rerun the complete final validation gate after the documentation acceptance update:

dotnet test TempestCSharp.slnx

Then:

dotnet build TempestCSharp.slnx

Then:

git diff --check

If tests or build fail, investigate only the failure.

Do not use a failure as an opportunity for unrelated cleanup.

If Windows security produces a notification but the final validation completes successfully, report the successful validation result and continue. Do not investigate or change Windows security configuration as part of this task.

Do not launch the interactive game. Neal has already completed manual acceptance.

STEP 5 - REVIEW THE COMPLETE DIFF

Run:

git status --short --branch
git diff --stat
git --no-pager diff

Review all changed tracked paths.

Verify:

- Level 6 implementation matches the already manually accepted milestone.
- Level 6 manual acceptance is documented.
- The deferred audio-volume issue is documented but not implemented.
- Levels 1–5 accepted behavior remains protected.
- Level 7 is not implemented.
- No reference source was modified.
- No generated gameplay recording is included.
- No unrelated refactoring or cleanup occurred.
- No accidental line-ending churn exists.
- git diff --check passes.

STEP 6 - STAGE THE LEVEL 6 MILESTONE

Stage only the intended Level 6 implementation, its tests, and the corresponding project documentation.

Do not stage ignored AI logs merely because they exist.

Do not stage generated recordings or unrelated files.

Then run:

git diff --cached --check
git status --short
git --no-pager diff --cached

Review the complete staged diff.

If any unrelated file or change is staged, STOP before committing.

STEP 7 - COMMIT THE ACCEPTED LEVEL 6 MILESTONE

If and only if all validation and staged-diff checks pass, commit with:

git commit -m "Implement production Level 6 progression"

Then run:

git status --short --branch
git rev-parse HEAD
git --no-pager log -1 --oneline
git diff --check
git diff --cached --check

The final tracked worktree and index must be clean.

Do not amend another commit.
Do not push.
Do not begin Level 7.
Do not begin the deferred audio-volume fix.

PROMPT AND REPORT LOGGING

Follow the existing project-wide logging instructions in:

.github/copilot-instructions.md

Update the existing Level 6 implementation report as appropriate so the durable report includes final automated validation and Neal's manual acceptance.

Do not create unnecessary duplicate logs.

FINAL REPORT

Report:

1. Starting HEAD.
2. Files committed.
3. Level 6 manual-acceptance documentation update.
4. Deferred audio-volume issue documentation.
5. Focused validation already associated with the implementation.
6. Final full solution test count from this closeout run.
7. Build result including warnings/errors.
8. git diff --check result.
9. New commit hash and subject.
10. Final Git status.
11. Confirmation that no generated gameplay recording or reference-source modification was committed.
12. Confirmation that Level 6 is now accepted.
13. Confirmation that Levels 1–6 are now the accepted production campaign foundation.
14. Confirmation that Level 7 remains unimplemented.
15. Confirmation that the enemy-destruction sound-volume issue remains deferred and no audio implementation was changed.
16. Stop. Do not begin another milestone.

TEMPEST_LEVEL6_ACCEPTANCE_COMMIT_DBF0031
