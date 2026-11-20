FAIL-CLOSED PROMPT CHECK

Read this entire user-provided task before doing anything.

Before using tools, reading files, running commands, modifying files, or taking
any repository action, locate the unique completion token near the end of this
user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a
line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment
metadata, automatically supplied workspace context, or other agent-generated
material may appear after the user's token and do not invalidate the task.

This check is intended to detect an incomplete or truncated USER task, not
appended agent context.

If exactly one complete TEMPEST_ token cannot be found, stop before any project
or repository action and explain why.


TASK

Close out the manually accepted Level 3 mixed-admission concurrency milestone.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected starting baseline before the uncommitted candidate:

7acac60035383bfb6e0cb170c0adfa0dd88b6c4b

The implementation candidate is already present in the worktree.

Read and use these existing ignored records as the implementation/investigation
handoff:

AI-Logs/Reports/2026-11-09-level3-concurrency-rom-investigation-report.md

AI-Logs/Reports/2026-11-09-level3-mixed-admission-diagnostic-implementation-report.md

Also inspect the matching saved prompts when useful.

Do NOT redo the implementation from scratch.

First verify:

- branch;
- HEAD;
- origin/master;
- tracking state;
- index;
- tracked working tree;
- relevant untracked state;
- actual candidate diff.

Confirm that the candidate corresponds to the previously investigated and
manually tested Level 3 mixed-admission work before modifying anything.


MANUAL ACCEPTANCE

Neal manually tested the candidate using the Level 3 diagnostic.

Manual acceptance PASSED.

Neal's overall judgment:

"As far as I can determine, it is perfect. Exact game play as the arcade game."

The following manual checks all passed:

1. Level 3 feels less overcrowded and matches the arcade gameplay.
2. More than one Tanker was not observed simultaneously through normal entry.
3. Tanker destruction still releases both Flippers normally.
4. Enemy-entry cadence looks natural, with no obvious freezing or unnatural
   gaps.
5. Flipper population varies naturally rather than appearing rigidly capped
   at four.
6. Death and reentry behave normally.
7. Super Zapper behavior remains normal.
8. Rim combat and eventual level completion remain normal.

Treat this as Neal's manual acceptance of the Level 3 mixed-admission
concurrency candidate.


EVIDENCE / FIDELITY CLASSIFICATION

Preserve the distinctions established by the ROM investigation.

ROM-backed findings include the verified Rev. 2 Level 3 ordinary-admission
mechanism, including:

- Flipper/Tanker ordinary entry;
- one-Tanker ordinary-entry maximum;
- seven physical enemy slots;
- Level 3 Flipper availability arithmetic;
- latent Tanker cargo affecting Flipper availability;
- wrapping byte subtraction behavior;
- rim/live enemies continuing to occupy capacity;
- released cargo bypassing ordinary species selection.

Do NOT describe the Level 3 rule as a simple hard four-Flipper cap.

The accepted C# complete-pair Tanker cargo behavior remains a deliberate
project translation/decision.

In particular, the candidate preserves both Tanker cargo identities even in a
saturation case where literal ROM physical-slot allocation could fail to
allocate the second child.

Therefore:

- do not claim that the C# implementation is a literal strict seven-live-object
  reproduction in every cargo-release edge case;
- document ordinary admission as ROM-informed/ROM-backed where supported;
- document preservation of the complete cargo pair at saturation as a PROJECT
  DECISION / PROJECT TRANSLATION;
- preserve the distinction between Neal's manually observed gameplay fidelity
  and internal behavior not directly observed during manual play.


SCOPE

This is a CLOSEOUT milestone.

Do not redesign accepted behavior.

Do not alter Levels 1 or 2.

Do not broaden this into Level 4 concurrency.

Do not perform unrelated refactoring.

Do not reopen the Level 3 ROM investigation unless a specific documentation or
validation question requires a narrowly bounded evidence check.

Do not change the accepted Level 3 mixed-admission implementation merely to
make the design cleaner.

Only make changes needed for:

- durable documentation of manual acceptance;
- accurate evidence classification;
- tests/validation if a genuine closeout issue is discovered;
- final milestone closeout;
- commit and push.

If a substantive production defect is discovered during closeout, stop and
report it rather than silently expanding scope.


DURABLE DOCUMENTATION

Update the appropriate durable Tempest documentation, including:

TEMPEST-DEVELOPMENT-NOTES.md

TEMPEST-BEHAVIOR-SPEC.md

Record:

- the completed Level 3 ROM concurrency investigation;
- the important ROM-backed admission findings;
- why Level 3 is not a simple Level 1/2-style four-Flipper cap;
- the accepted Level 3 mixed-admission implementation;
- the one-Tanker ordinary-entry behavior;
- the ROM-informed Flipper/cargo availability behavior;
- the seven-place ordinary admission concept;
- waiting Egg behavior where relevant;
- preservation of stable C# ledger identities;
- preservation of complete two-Flipper Tanker cargo;
- the deliberate saturation-edge project translation;
- Neal's eight passed manual gameplay checks;
- Neal's overall judgment that the resulting Level 3 gameplay is effectively
  identical to the arcade game.

Keep evidence classifications precise.

Do not convert PROJECT DECISION / PROJECT TRANSLATION behavior into ROM-BACKED
behavior merely because it passed manual acceptance.

Do not rewrite unrelated historical sections unnecessarily.


AI RECORDS

Preserve the existing investigation and implementation reports as historical
records.

Do not rewrite those historical reports merely to change their former
"manual acceptance pending" state.

Create a new ignored closeout report under:

AI-Logs/Reports/

Use a descriptive filename such as:

2026-11-09-level3-mixed-admission-concurrency-closeout-report.md

Preserve this substantive closeout prompt under:

AI-Logs/Prompts/

using the corresponding date-prefixed closeout filename.

AI-Logs remain ignored working/history artifacts and must not be staged or
committed.


FINAL VALIDATION

After documentation updates, run final bounded validation appropriate for the
accepted candidate.

At minimum verify the changed Level 3 concurrency/admission tests and the
important affected Level 3 lifecycle/regression tests.

Then, provided focused validation succeeds and no repository/security blocker
prevents it, run:

- full solution tests with the established VSTest hang protection;
- full solution build;
- git diff --check;
- review of the complete tracked diff;
- staged diff verification before commit.

The implementation candidate previously passed 1,139 full-solution tests,
0 failures, and a build with 0 warnings and 0 errors. Treat those results as
historical candidate evidence, not as a substitute for required final
closeout validation.

Do not launch the interactive game. Neal's manual acceptance has already
been completed.

Follow the repository-wide Copilot command/security/stall/self-stop rules if
anything blocks validation.


COMMIT AND PUSH

If and only if:

- the candidate still matches the manually accepted Level 3 implementation;
- documentation accurately records the acceptance and evidence;
- required final validation passes;
- no blocking discrepancy exists;

then stage exactly the intended tracked files.

Do not stage:

- AI-Logs;
- Reference/arcade/roms.rev2/;
- ROM/reference binaries;
- TEMP;
- UserData;
- recordings;
- screenshots;
- videos;
- unrelated local/user files.

Review the staged diff and staged file list.

Commit the accepted milestone with an appropriate concise message, for example:

Implement accepted Level 3 enemy concurrency

Push master to origin.

Then verify:

- pushed HEAD;
- origin/master;
- branch/tracking state;
- clean tracked index/worktree;
- protected/untracked local material remains uncommitted.

Do not claim success if commit or push did not complete.


FINAL REPORT

The closeout report should include:

- starting and final commit hashes;
- files committed;
- concise implementation summary;
- ROM-backed findings preserved;
- project translations/decisions preserved;
- Neal's manual acceptance and all eight passed checks;
- focused validation results;
- full test count/result;
- build result;
- whitespace/diff checks;
- commit result;
- push result;
- final repository state;
- any deliberately unresolved fidelity edge cases.

STOP after the accepted Level 3 concurrency milestone is committed, pushed,
and verified.

Do not begin the planned .github/copilot-instructions.md maintenance milestone.

Do not begin Level 4 concurrency.

Neal and Copilot will handle those as separate subsequent milestones.


TEMPEST_LEVEL3_CONCURRENCY_ACCEPTED_CLOSEOUT_COMPLETE
