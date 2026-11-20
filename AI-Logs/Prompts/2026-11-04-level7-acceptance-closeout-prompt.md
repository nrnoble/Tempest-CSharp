FAIL-CLOSED COMPLETENESS CHECK

Read this entire user-provided task before using any tools, reading files, running commands, editing files, staging files, or taking any repository action.

Before doing anything else, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context may appear after the user's completion token and do not invalidate it.

This check is intended to detect an incomplete or truncated user task, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project or repository action and explain that the task may be incomplete or truncated.


TEMPEST LEVEL 7 MANUAL ACCEPTANCE AND FINAL CLOSEOUT

This task closes the accumulated production Level 7 milestone after Neal's manual gameplay acceptance.

This is primarily an ACCEPTANCE / VALIDATION / DOCUMENTATION / GIT CLOSEOUT task.

Do not perform additional gameplay development unless final validation reveals a genuine defect in the accumulated Level 7 milestone.

Do not broaden scope.

Levels 1-6 remain frozen.

Do not implement Level 8 or later levels.

Any future ROM/arcade-fidelity investigations of Levels 2-6 are separate future efforts and MUST NOT be mixed into this closeout.


CURRENT BASELINE AND WORKTREE

The accepted Levels 1-6 production baseline before Level 7 work was:

79e33886772424a31973332f33f3781b55e4c2f8
Implement production Level 6 progression

Level 7 was subsequently developed as an accumulated uncommitted milestone.

IMPORTANT:

The worktree is expected to contain the complete accumulated Level 7 implementation, tests, documentation, and subsequent arcade-fidelity corrections.

Do NOT assume that only the most recently edited files belong to Level 7.

Do NOT reset, revert, discard, restore, or overwrite the existing uncommitted Level 7 work.

Before editing anything:

1. Run:
   git status --short --branch
   git rev-parse HEAD
   git diff --stat
   git diff --name-status
   git diff --check

2. Verify that HEAD is still the expected pre-Level-7 accepted baseline unless the repository itself clearly documents an intentional later checkpoint.

3. Review the complete accumulated uncommitted diff from HEAD, not merely the most recent changes.

If HEAD or the worktree differs materially from the expected accumulated Level 7 milestone and the difference cannot be safely explained from repository evidence, STOP before editing, staging, or committing and report the discrepancy.

Do not silently absorb unrelated changes into the Level 7 commit.


AUTHORITATIVE PROJECT MATERIAL

Read before closeout:

- .github/copilot-instructions.md
- TEMPEST-BEHAVIOR-SPEC.md
- TEMPEST-DEVELOPMENT-NOTES.md
- README.md as relevant to the Level 7 milestone
- AI-Logs/Reports/2026-11-04-level7-arcade-corrections-implementation-report.md
- AI-Logs/Reports/2026-11-04-level7-reentry-rom-investigation-report.md
- AI-Logs/Reports/2026-11-04-level7-flipper-rom-analysis-report.md
- the previous Level 7 implementation/preimplementation reports as needed
- current Level 7 production code
- current Level 7 tests

Use repository evidence to distinguish the historical stages of Level 7 development from its final accepted state.

Historical AI reports are historical records. Do not rewrite earlier reports merely because later evidence superseded an earlier interpretation.

TEMPEST-BEHAVIOR-SPEC.md and TEMPEST-DEVELOPMENT-NOTES.md are the durable project authorities and should reflect the final accepted state.


NEAL'S FINAL LEVEL 7 MANUAL ACCEPTANCE

Neal has now manually tested the corrected Level 7 implementation.

Record the following accurately.

MANUAL PASS / ARCADE VERIFIED:

1. No individual Flippers enter directly from the bottom.

2. Tankers enter and release their Flippers normally.

3. Below-rim Flippers visibly follow the arcade-observed:

   2-3-2-2-2

   lane-change pattern.

OVERALL MANUAL ACCEPTANCE:

Neal is a highly experienced original arcade Tempest player.

His overall judgment after playing the corrected implementation is that even an experienced expert such as himself would find it difficult to distinguish the Level 7 gameplay from the original arcade gameplay.

Treat this as Neal's perceptual/manual acceptance judgment, not as a claim of cycle-exact or mathematically identical emulation.

NON-BLOCKING CORNER CASE NOT MANUALLY REPRODUCED:

Neal has not yet succeeded in manually reproducing the exact case in which only one Tanker or Flipper remains immediately before player death.

It is difficult to deliberately force that situation during normal gameplay.

Neal explicitly considers this corner case:

- non-blocking;
- not required for Level 7 acceptance;
- not an observable gameplay problem;
- something he may continue attempting to observe later.

Do NOT falsely mark this specific corner case as manually verified.

Do NOT leave overall Level 7 acceptance pending because of it.

The underlying one-unit reconstruction and final-pending behavior already have ROM-backed and automated-test evidence; preserve the distinction between that evidence and Neal's uncompleted visual reproduction.

Accordingly:

LEVEL 7 MANUAL ACCEPTANCE = PASS.

The one-enemy death/re-entry visual reproduction remains:

NOT MANUALLY REPRODUCED / NON-BLOCKING.


FINAL ACCEPTED LEVEL 7 BEHAVIOR

Preserve the completed Level 7 behavior currently implemented, including the accumulated milestone and later arcade corrections.

Important final characteristics include, as supported by the current repository:

- displayed production Level 7;
- exact closed 16-lane clover geometry;
- natural Level 6 -> Level 7 progression;
- direct --level7-test support;
- Level 7 remains the current terminal campaign boundary;
- Level 7 Flipper/Tanker/Spiker enemy set;
- no ordinary individual bottom-entry Flippers;
- Tankers carrying/releasing Flippers;
- ROM-shaped Level 7 Tanker/Spiker selection;
- Level 7 generic death/re-entry supply handling rather than preserving unresolved species;
- final-pending Spiker-to-Flipper-carrying-Tanker behavior;
- visible arcade-observed 2-3-2-2-2 below-rim Flipper behavior;
- existing Level 7 firing behavior;
- existing Spikes / Avoid Spikes behavior;
- accepted Tanker and Spiker behavior;
- accepted Super Zapper behavior;
- existing approach/progression behavior;
- score/lives carry behavior;
- accepted rim pursuit;
- existing audio behavior;
- existing replay infrastructure.

Do not alter implementation simply to restate these behaviors.

No Level 8 work is authorized.


LEVELS 1-6 FREEZE

Levels 1-6 are the accepted production foundation and remain frozen for this closeout.

Level 1 is currently considered unlikely to require further fidelity work.

Levels 2-6 may undergo separate ROM/arcade-fidelity investigations in the future similar to the Level 7 investigation.

That future possibility does NOT mean Levels 2-6 are presently rejected, broken, or reopened.

They remain the accepted baseline.

Do not modify Level 1-6 gameplay in this closeout.

If final validation unexpectedly demonstrates a Level 1-6 regression caused by Level 7 work, STOP and investigate the regression before committing.

Do not intentionally retune Level 1-6 to match newly discovered Level 7 mechanisms.


STEP 1: UPDATE DURABLE DOCUMENTATION FOR ACCEPTANCE

Update TEMPEST-BEHAVIOR-SPEC.md and TEMPEST-DEVELOPMENT-NOTES.md so the final Level 7 state is no longer described as manual acceptance pending.

Record:

- corrected Level 7 manual acceptance = PASS;
- no individual bottom-entry Flippers = manually verified;
- normal Tanker entry and Flipper release = manually verified;
- 2-3-2-2-2 below-rim movement = manually verified;
- Neal's overall perceptual judgment that Level 7 is extremely difficult for him to distinguish from arcade gameplay;
- exact one-enemy death/re-entry visual reproduction = not manually reproduced, non-blocking;
- underlying ROM-backed and automated evidence remains distinct from manual observation;
- Level 7 is accepted while C# approximations already documented for cadence/handedness/coordinate mapping remain approximations.

Preserve evidence classifications.

Do not rewrite historical AI reports to pretend their earlier pending state never existed.

Update README.md only if its current Level 7 status would otherwise incorrectly remain manual-acceptance-pending or contradict the durable authorities.

Do not make unrelated documentation cleanup.


STEP 2: REVIEW THE COMPLETE ACCUMULATED LEVEL 7 DIFF

Before staging or committing, inspect the entire diff from HEAD.

At minimum run and review:

git status --short --branch
git diff --stat
git diff --name-status
git diff
git diff --check

The review must cover the accumulated Level 7 implementation, not just today's documentation edits.

Determine whether every tracked change belongs to the Level 7 milestone or its required documentation/tests.

Also inspect untracked files.

Classify every untracked file as one of:

- intended Level 7 source/test/project artifact that belongs in the commit;
- ignored/local/generated artifact that must remain uncommitted;
- unrelated or uncertain.

Do NOT stage an unrelated or uncertain file.

AI-Logs are expected to be ignored historical records and should remain subject to the repository's established logging policy.

If an unexpected tracked modification or untracked file makes commit scope uncertain, STOP before staging and report it.


STEP 3: FINAL AUTOMATED VALIDATION GATE

Run validation against the final documentation/code state.

Use repository-supported commands and preserve the existing solution conventions.

Required:

A. Appropriate focused Level 7 tests covering at least:
- Level 7 production;
- Level 7 movement;
- Level 7 picker/supply;
- death/re-entry;
- Tanker/cargo;
- firing;
- progression/terminal behavior.

B. Appropriate shared-seam regressions covering the accepted Levels 1-6 behavior affected by shared implementation seams.

C. Complete solution test suite:

dotnet test TempestCSharp.slnx

D. Complete solution build:

dotnet build TempestCSharp.slnx

E. Whitespace validation:

git diff --check

Do not treat a zero-tests-discovered result as success.

Record actual passing/failing/skipped counts reported by the commands.

Build must complete successfully.

If build or tests fail:

- investigate;
- correct only defects legitimately belonging to this Level 7 milestone;
- rerun the affected validation and required final gates.

Do not weaken valid tests merely to obtain a pass.

Do not modify accepted Levels 1-6 behavior to force the closeout through.

If a required fix would materially broaden scope or reopen Levels 1-6, STOP and report the issue instead of committing.


STEP 4: FINAL PRE-COMMIT REVIEW

ONLY after all final validation passes:

Run again:

git status --short --branch
git diff --stat
git diff --name-status
git diff --check

Review the complete final diff.

Verify that:

- the implementation represents the accepted Level 7 milestone;
- manual acceptance is recorded accurately;
- the unverified corner case is not falsely marked as manually passed;
- Levels 1-6 were not intentionally changed;
- Level 8 was not implemented;
- there are no debug artifacts;
- there are no generated gameplay recordings accidentally included;
- there are no temporary files accidentally included;
- there are no unrelated changes;
- there are no secrets, credentials, machine-specific files, binaries, or inappropriate build outputs being committed;
- AI-Logs remain handled according to repository policy.

If any concern remains about commit scope, STOP before staging.


STEP 5: STAGE EXACTLY THE ACCEPTED LEVEL 7 MILESTONE

Stage only the files belonging to the accumulated accepted Level 7 milestone.

Do not blindly use a command that would stage unrelated files simply for convenience.

After staging, inspect:

git status --short
git diff --cached --stat
git diff --cached --name-status
git diff --cached
git diff --cached --check

This staged diff review is mandatory.

Verify the staged diff is exactly the complete Level 7 milestone intended for acceptance.

Verify no required Level 7 tracked/new source or test file was accidentally omitted.

Verify no unrelated file was accidentally added.

If the staged diff is not correct, fix the staging set and review it again before committing.


STEP 6: COMMIT THE ACCEPTED LEVEL 7 MILESTONE

Only after:

- manual acceptance is correctly documented;
- focused tests pass;
- shared regression tests pass;
- full solution tests pass;
- build passes;
- unstaged diff checks pass;
- complete diff review passes;
- cached/staged diff review passes;
- cached diff whitespace check passes;

create one commit containing the complete accepted Level 7 milestone.

Use a concise descriptive commit message such as:

Implement accepted production Level 7

If repository history strongly establishes a clearly preferable convention, follow that convention while keeping the meaning equivalent.

Do NOT push.


STEP 7: VERIFY THE COMMIT

After committing, run:

git status --short --branch
git log -1 --oneline
git rev-parse HEAD
git diff --stat
git diff --cached --stat

Verify:

- the commit exists;
- HEAD points to the new Level 7 commit;
- no intended Level 7 changes remain unstaged;
- no intended Level 7 changes remain staged;
- tracked working tree/index are clean;
- any remaining untracked files are intentional ignored/local artifacts and do not represent omitted milestone content.

If the repository is not clean, investigate why.

Do not create an additional cleanup commit merely to hide a mistake. Determine whether the remaining state represents omitted milestone content, an unrelated file, or an intentional ignored/local artifact.


STEP 8: SAVE CLOSEOUT AI RECORDS

Follow .github/copilot-instructions.md.

Preserve this substantive prompt under:

AI-Logs/Prompts/

and create the corresponding closeout report under:

AI-Logs/Reports/

using descriptive paired date-prefixed filenames.

The closeout report should record:

- starting HEAD;
- starting status;
- manual acceptance evidence;
- corner-case qualification;
- documentation changes;
- complete accumulated diff review;
- files staged/committed;
- focused validation result and actual counts;
- shared regression result and actual counts;
- full test-suite result and actual counts;
- build warning/error result;
- unstaged and staged git diff --check results;
- commit message;
- resulting commit hash;
- final Git status;
- confirmation that Levels 1-6 remained frozen;
- confirmation that no Level 8 work occurred;
- confirmation that no push occurred;
- any intentionally remaining local/ignored artifacts.

AI reports are historical records. The durable behavior/development documents remain the project authority.


DO NOT

- Do not launch the interactive game. Neal has already completed the required manual acceptance.
- Do not perform additional gameplay tuning merely because something could theoretically be improved.
- Do not reopen the manually accepted Level 7 implementation without a demonstrated defect.
- Do not alter Levels 1-6.
- Do not implement Level 8.
- Do not retune audio.
- Do not retune spinner controls.
- Do not perform rendering-fidelity work.
- Do not work on attract mode or replay enhancements.
- Do not implement high scores.
- Do not implement end-game animation.
- Do not perform unrelated refactoring or cleanup.
- Do not push.


FINAL RESPONSE

Return a concise but complete closeout summary containing:

1. Level 7 manual acceptance status.
2. The non-blocking one-enemy death/re-entry qualification.
3. Documentation files updated.
4. Complete accumulated Level 7 milestone files committed.
5. Focused test result with actual count.
6. Shared regression result with actual count.
7. Full solution test result with actual passing/failing/skipped counts.
8. Build warnings/errors.
9. Final unstaged and staged diff-check results.
10. Commit message.
11. New commit hash.
12. git log -1 --oneline result.
13. Final git status.
14. Confirmation that Levels 1-6 remained frozen.
15. Confirmation that Level 8 was not implemented.
16. Confirmation that no push occurred.
17. Any remaining intentionally local/ignored artifacts or unresolved non-blocking observations.

If everything passes and the repository is clean, explicitly state that the accepted production baseline is now the new Level 7 commit.

Stop there.

TEMPEST_LEVEL7_ACCEPTANCE_CLOSEOUT_COMPLETE