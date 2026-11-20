Read this entire user-provided task before taking any action.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, reading files, running commands, or taking any repository action, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with TEMPEST_ appears on a line by itself in the user-provided task. If exactly one complete token cannot be found, stop before any project or repository action and explain why.

# Task: Track AI-Logs and Reference and push them to GitHub

The accepted Atari-presentation milestone has already been committed and pushed.

Neal now explicitly wants these directories and their intended existing contents tracked by Git and pushed to GitHub:

AI-Logs/

Reference/

The purpose is to make this historical development evidence and project reference material available through the GitHub repository, including for retrieval by Neal's M365 Copilot in future discussions.

## Important current state

Neal has already manually removed AI-Logs/ and Reference/ from .gitignore.

Do **not** re-add either directory to .gitignore.

The remaining local exclusions such as:

- bin/
- obj/
- .vscode/
- *.user
- *.suo
- /Videos/
- /Temp/
- /UserData/

must remain protected.

The .gitignore comments may be cleaned up narrowly if they no longer accurately describe the remaining rules, but do not otherwise broaden or change ignore policy.

## Policy change

This task intentionally supersedes the previous project rule that AI-Logs/ must remain ignored and must never be staged or committed.

From this milestone forward, the established prompt/report history under AI-Logs/ is intended to be normal tracked repository material.

Update .github/copilot-instructions.md as necessary so the durable project instructions reflect that change and no longer prohibit tracking or committing AI-Logs.

Preserve the existing prompt/report organization and historical records.

Do not rewrite, consolidate, rename, or delete old reports or prompts merely because they are now being tracked.

Reference/ is likewise now intentionally authorized for Git tracking.

Preserve its existing structure and filenames.

## Inventory and safety check before staging

Before staging anything:

1. Verify branch, HEAD, upstream, ahead/behind, index and worktree state.
2. Inventory AI-Logs/ recursively.
3. Inventory Reference/ recursively.
4. Report file counts and approximate total size for each.
5. Identify unusually large files, binaries, archives, executables, actual ROM images/dumps, credentials/secrets, personal scratch files, generated output, or obviously accidental material.

Do not blindly stage questionable material simply because it resides within an authorized directory.

Text prompts, reports, documentation, disassembly text, reference text, and similar intentional development/reference material are expected candidates for tracking.

If an actual ROM binary, credential, access token, private key, password, or other clearly sensitive/questionable item is encountered, stop before committing that item and report the affected path and type of concern without reproducing sensitive content.

Do not open or analyze visual assets merely to conduct this inventory.

## Protect unrelated local material

This authorization applies to:

- AI-Logs/
- Reference/
- the minimum .gitignore comment cleanup, if useful;
- .github/copilot-instructions.md changes required for the new tracked-AI-Logs policy.

Do not add:

- /UserData/
- /Temp/
- /Videos/
- root-level screenshots
- unrelated gameplay recordings
- build output
- personal scratch files
- unrelated untracked files.

In particular, do not add screenshot000.png.

Preserve unrelated worktree material.

## Stage and inspect

After the inventory and safety checks pass:

1. Stage the intended AI-Logs/ contents.
2. Stage the intended Reference/ contents.
3. Stage Neal's existing .gitignore change.
4. Stage only the necessary .github/copilot-instructions.md policy update.
5. Inspect the complete staged file inventory.
6. Inspect the relevant staged textual diffs.
7. Run git diff --cached --check.
8. Verify that no unrelated files or deletions entered the index.
9. Explicitly verify that UserData, Temp, Videos, root screenshots and other unrelated local artifacts remain out of the index.

This is a repository-content/policy milestone. Do not run the Tempest test suite or launch the game unless production source code is unexpectedly changed.

Production source code should not change in this task.

## Commit and push

If the inventory, safety checks and staged inspection pass:

Commit the intended material to the current master branch.

Use commit message:

Add AI logs and reference material

Push normally to origin/master.

Do not force-push.

Do not create a branch.

Do not rewrite history.

Verify after pushing that local master and origin/master are at the same commit and ahead/behind is 0/0.

## Post-push verification

Verify that representative committed files from both:

AI-Logs/

and:

Reference/

are actually tracked in the resulting commit.

The objective is to make the contents available from GitHub, not merely to alter ignore policy.

## Final report

Report:

- starting HEAD;
- final commit SHA;
- commit message;
- push result;
- number of AI-Logs files committed;
- number of Reference files committed;
- safety/inventory results;
- .gitignore change;
- .github/copilot-instructions.md policy change;
- any deliberately excluded files and why;
- final branch/upstream ahead/behind;
- final index state;
- unrelated tracked/untracked material left untouched.

Save this prompt and final report using the established AI-Logs prompt/report convention.

Because AI-Logs are now intentionally tracked, include the completed task prompt in this commit. If the final report requires the resulting commit SHA and therefore cannot be completely finalized until after the commit, do not amend or rewrite the commit merely to include its own final SHA. It may remain as the immediate post-commit report for inclusion in the next normal commit.

TEMPEST_TRACK_AI_LOGS_REFERENCE
