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

Continue the existing Tempest project-instructions modernization candidate.

Repository:

C:\Users\nrnob\source\repos\Tempest

The existing modernization candidate in:

.github/copilot-instructions.md

has been reviewed by Neal and Copilot and is accepted in overall structure and
content, subject to the three small refinements below.

Do not redesign or broadly rewrite the instruction file again.

Do not begin Level 4 concurrency.

Do not modify production code, tests, gameplay documentation, or unrelated
files.

IMPORTANT .gitignore STATE

The existing .gitignore modification was made manually and intentionally by
Neal while the previous prompt was running.

It is authorized user work.

Preserve .gitignore exactly as it currently exists.

Do not revert it.
Do not modify it.
Do not stage it.
Do not include it in the project-instructions modernization commit.

Treat the intentional unstaged .gitignore change as protected unrelated user
work and not as a blocker to completing this milestone.

REFINEMENT 1 — MAKE THE COMPLETION-TOKEN LITERAL UNAMBIGUOUS

In the Completion requirements section, replace the Markdown-escaped spelling:

TEMPEST\_

with an unambiguous inline-code literal:

`TEMPEST_`

The rule should clearly state that the substantive user task contains exactly
one complete token beginning with `TEMPEST_` on a line by itself.

The backslash must not appear to be part of the actual required token.

Preserve the rest of the completion-token semantics:

- exactly one complete token for substantial tasks using the convention;
- token on a line by itself;
- appended VS Code/agent context after the user token does not invalidate it;
- stop before project/repository action when the required token cannot be
  established;
- trivial questions/lookups/minor interactions do not require the convention.

REFINEMENT 2 — PRESERVE THE SHORT FAIL-CLOSED PROMPT HEADER POLICY

Add a concise statement to the Completion requirements section establishing
that substantial user prompts may continue to include a short fail-closed
completion-token header as transmission/truncation protection.

Use wording equivalent to:

A substantial user prompt may still include a short fail-closed token-check
header as transmission protection; do not treat that as conflicting
duplication of this repository-wide rule.

Keep this concise.

The repository instructions contain the durable policy; the short prompt
header remains an intentional additional safeguard against an incomplete or
truncated individual user task.

Do not duplicate the full repository-wide token policy inside that sentence.

REFINEMENT 3 — MAKE THE VALIDATION/COMMIT BOUNDARY EXPLICIT

In the Repository and Git safety section, strengthen the existing sentence:

Incomplete required validation blocks normal commit/push unless an explicitly
authorized partial checkpoint applies.

Make the relationship to the Command, security, and recovery section explicit.

Use wording equivalent to:

Failed, blocked, timed-out, canceled, stalled, or otherwise incomplete required
validation blocks normal commit/push unless an explicitly authorized partial
checkpoint applies.

Do not create a second large validation-failure policy.

The Command, security, and recovery section should remain the detailed
authority for PASS, FAIL, BLOCKED, TIMEOUT, CANCELED, PARTIAL, Windows security
blocks, bounded commands, stall handling, and voluntary self-stop.

This change is only intended to make the Git authorization consequence explicit.

SMALL FORMATTING CHECK

While reviewing the final file, inspect the existing PASS/FAIL outcome sentence
in the Command, security, and recovery section.

Ensure normal Markdown/readability spacing exists before **PASS** in the phrase
that currently may render as:

as**PASS**

If that missing space exists in the actual file, correct only that trivial
formatting issue.

Do not perform unrelated editorial cleanup.

SELF-REVIEW

After these refinements, reread the complete .github/copilot-instructions.md.

Confirm that:

- the evidence hierarchy remains unchanged;
- arcade/ROM/Incitti/project-translation distinctions remain unchanged;
- local ROM/disassembly guidance remains unchanged;
- one-milestone scope rules remain unchanged;
- protected-local-material rules remain unchanged;
- Neal's manual acceptance authority remains unchanged;
- AI-Logs and durable-documentation lifecycle rules remain unchanged;
- Windows security/stall/recovery protections have not been weakened;
- explicit commit/push authorization remains required;
- the completion-token policy is now unambiguous;
- the short fail-closed prompt header is explicitly permitted;
- no new duplication or policy conflict was introduced.

VALIDATION

This remains an instructions-only maintenance milestone.

Do not run gameplay tests.
Do not run solution tests.
Do not build the solution.
Do not launch the game.

Validate only what is appropriate:

- inspect the complete resulting .github/copilot-instructions.md;
- inspect its diff;
- run git diff --check;
- verify no unintended tracked files were changed by this task.

The manually modified .gitignore is expected unrelated user work and must remain
unstaged and untouched.

AI REPORT

Update the current modernization work by creating or updating the appropriate
ignored modernization report to record:

- these three requested refinements;
- any corrected PASS formatting issue;
- self-review result;
- final validation;
- the intentional unrelated .gitignore state.

Preserve historical reports according to the repository instructions. If the
existing PARTIAL report is considered a historical state that should not be
rewritten, create an appropriate follow-up/resume report instead.

AI-Logs remain ignored and must not be staged or committed.

COMMIT AND PUSH

If the refined .github/copilot-instructions.md candidate passes validation:

Stage only:

.github/copilot-instructions.md

Do NOT stage .gitignore.

Review the staged file list and staged diff.

Commit the completed modernization with an appropriate concise message such as:

Modernize Tempest Copilot project instructions

Push master to origin.

Then verify:

- pushed HEAD;
- origin/master;
- tracking state;
- tracked index/worktree state;
- .gitignore remains intentionally modified and unstaged;
- AI-Logs remain ignored/uncommitted.

The final repository is therefore expected to have a clean committed
project-instructions modernization while still showing Neal's separate
intentional unstaged .gitignore modification.

STOP after verification.

Do not begin Level 4 concurrency.

TEMPEST_PROJECT_INSTRUCTIONS_FINAL_REFINEMENT_COMPLETE
