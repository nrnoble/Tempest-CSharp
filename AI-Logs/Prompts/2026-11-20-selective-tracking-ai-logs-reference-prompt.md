# Task: Complete selective tracking of AI-Logs and safe Reference material

Continue from the blocked inventory milestone.

Starting accepted Git baseline:

bb9538f21bd0035e8ccafc0b45979201e2360556

The previous inventory correctly stopped before staging because:

- Reference/arcade/roms.rev2/ contains local ROM files;
- Reference/arcade/MBUDOC.DOC had an ambiguous format that was not yet identified.

Neal has now explicitly decided:

- AI-Logs/ should be tracked and pushed to GitHub.
- Safe textual/documentary material under Reference/ should be tracked and pushed to GitHub.
- Reference/arcade/MBUCOD.V05.txt is intended reference material and should be included assuming ordinary safety checks pass.
- Reference/arcade/roms.rev2/ must remain local and must **not** be committed or pushed.
- Neal has already restored /Reference/arcade/roms.rev2/ to .gitignore.
- Root-level screenshot000.png remains local and must not be committed.
- Reference/arcade/MBUDOC.DOC should be narrowly identified before deciding whether to include it.

## First verify the current repository state

Before staging anything:

1. Verify branch, HEAD, upstream and ahead/behind.
2. Verify the index.
3. Inspect the current worktree.
4. Confirm /Reference/arcade/roms.rev2/ is ignored.
5. Confirm none of the ROM files are currently staged.
6. Preserve Neal's existing .gitignore edits.
7. Preserve unrelated worktree material.

Do not open, hash, analyze or stage the files under Reference/arcade/roms.rev2/.

They are explicitly excluded from this milestone.

## Narrowly identify MBUDOC.DOC

Investigate only:

Reference/arcade/MBUDOC.DOC

The purpose is to determine what kind of file it actually is and whether it is ordinary documentary/reference material suitable for Git.

Do not execute it.

Do not assume it is Microsoft Word merely because it has a .DOC extension.

Use bounded, non-executing file-format inspection appropriate to the local environment.

Determine, as far as safely possible:

- whether it is plain text, structured text, a legacy document format, binary data, ROM-related data, or another recognizable format;
- whether its contents are ordinary Tempest development/reference documentation;
- whether there is any clear reason not to place it in this repository.

If it is ordinary textual/documentary reference material, include it.

If it appears to contain ROM data, executable material, credentials/secrets, private information, or something whose provenance/distribution remains genuinely questionable, do not stage it and stop for Neal's decision.

Do not expand this into a broad forensic investigation.

## Safety check for material to be published

Perform a bounded safety check over the intended textual material in:

AI-Logs/

and the non-ROM material selected from:

Reference/

Look for accidental credentials, private keys, access tokens, passwords or similarly obvious secrets.

Do not reproduce any secret value in logs or reports.

If a likely secret is found, stop before staging the affected file and report only its path and the category of concern.

The objective is to prevent accidental publication, not to conduct an unbounded security audit.

## Update durable repository policy

The prior project policy stated that AI-Logs/ was ignored historical/working material and must not be committed.

Neal has explicitly reversed that policy.

Update .github/copilot-instructions.md narrowly so it now reflects:

- substantial prompts continue to be saved under AI-Logs/Prompts;
- substantial implementation/investigation/closeout reports continue under AI-Logs/Reports;
- AI-Logs are historical development records;
- AI-Logs are now tracked repository material and may be staged/committed normally when appropriate;
- existing reports remain historical records and should not be rewritten merely to update history;
- durable behavior/project authority remains in the appropriate project documentation rather than automatically treating an old AI report as current truth.

Remove obsolete instructions saying AI-Logs must never be staged or committed.

Do not otherwise reorganize the project instructions.

## Preserve ignore protection

Keep these local/protected categories ignored:

- bin/
- obj/
- .vscode/
- *.user
- *.suo
- /Videos/
- /Temp/
- /UserData/
- /Reference/arcade/roms.rev2/

Do not add AI-Logs/ or the general Reference/ directory back to .gitignore.

Minor .gitignore comment cleanup is acceptable if needed to make the categories accurate.

Do not change unrelated ignore policy.

## Stage selectively

After the inspection and safety checks pass, stage only:

1. intended AI-Logs/ contents;
2. approved non-ROM Reference/ material;
3. Neal's current .gitignore policy change;
4. the necessary .github/copilot-instructions.md policy update.

Explicitly exclude:

- Reference/arcade/roms.rev2/;
- screenshot000.png;
- UserData/;
- Temp/;
- Videos/;
- build output;
- unrelated local files.

Inspect the complete staged inventory.

Run:

- staged textual-diff inspection where appropriate;
- git diff --cached --check.

Confirm specifically that no file beneath Reference/arcade/roms.rev2/ appears in the index.

Do not run the Tempest game.

Do not run the full gameplay test suite merely for this content/policy commit because production source code should not change.

If production source code unexpectedly changes, stop and report why rather than including it.

## Commit and push

If all checks pass:

Commit to the current master branch with:

Add AI logs and reference material

Push normally to:

origin/master

Do not force-push.

Do not create another branch.

Do not rewrite history.

Verify after the push that:

- local master and origin/master resolve to the same commit;
- ahead/behind is 0/0;
- representative files beneath both AI-Logs/ and Reference/ are actually tracked in the pushed commit;
- no ROM file from Reference/arcade/roms.rev2/ was committed.

## Task's own AI-Logs records

Save this prompt and its report using the established AI-Logs prompt/report convention.

Because AI-Logs are becoming tracked files in this milestone, include this prompt in the commit if it is complete before staging.

Do not create a circular commit/amend loop merely so the final report can contain the SHA of the commit that would contain that same report.

If the report must be completed after the commit/push to record the resulting SHA, leave that completed report for the next ordinary commit rather than amending or creating an unnecessary second commit solely for bookkeeping.

## Final report

Report:

- starting HEAD;
- resulting commit SHA;
- commit message;
- push result;
- number of AI-Logs files committed;
- number of Reference files committed;
- determination for Reference/arcade/MBUDOC.DOC;
- confirmation that Reference/arcade/roms.rev2/ remained excluded;
- safety-check result without exposing sensitive values;
- .gitignore final policy;
- .github/copilot-instructions.md policy change;
- any other intentionally excluded files;
- final branch/upstream ahead/behind;
- final index state;
- remaining unrelated worktree material.

Do not begin another development milestone.

TEMPEST_COMPLETE_TRACKED_PROJECT_HISTORY
