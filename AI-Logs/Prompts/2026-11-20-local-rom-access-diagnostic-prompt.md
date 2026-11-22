FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, files, commands, or taking repository action, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with TEMPEST_ appears on a line by itself in the user-provided task.

If exactly one complete token cannot be found, stop before any project or repository action and explain why.

# Task: Determine whether local Rev. 2 ROM evidence remains accessible for bounded development analysis

This is a tightly bounded READ-ONLY diagnostic.

Do not implement Level 11.

The objective is to determine why the Level 11 geometry recovery is blocked and whether the local ROM evidence used successfully by the historical Level 10 geometry-recovery milestone remains available for the same kind of bounded read-only engineering analysis.

## Background

Historical project evidence establishes that the successful Level 10 geometry recovery used the local program-ROM image:

Reference/arcade/roms.rev2/136002-118.k1

The historical Level 10 recovery used that local image only as primary read-only evidence to recover geometry bytes omitted from the textual disassembly.

Recent Level 11 prompts explicitly prohibited access to:

Reference/arcade/roms.rev2/

Those tasks therefore could not reproduce the successful Level 10 geometry-recovery method.

Neal has now explicitly authorized a tightly bounded read-only check of the specific local ROM file above.

This authorization does NOT authorize publication, copying, modification, staging, committing, pushing, redistribution, broad ROM mining, or unrestricted exploration.

## Primary question

Determine which of these situations applies:

1. **PROMPT-IMPOSED BLOCK ONLY**
- The previous Level 11 tasks were blocked because their own instructions prohibited access to the local ROM directory.
- With that prohibition removed for this bounded task, the specific local ROM can still be read for engineering evidence just as it was during Level 10 recovery.
2. **TOOL / PLATFORM / POLICY BLOCK**
- Despite Neal's explicit authorization and the historical Level 10 precedent, VS Code Copilot or an underlying tool refuses or prevents read-only access to the specific local ROM.
- If so, capture the exact refusal/blocking behavior and explain what component produced it if that can be determined from observable output.
3. **TECHNICAL ACCESS PROBLEM**
- The ROM cannot be read for a non-policy technical reason, such as missing file, path problem, permissions, I/O failure, or changed local repository state.
- Identify the concrete technical problem.

Do not speculate about licensing enforcement.

Report only observable evidence.

## Extremely narrow authorization

You are authorized to access only this ROM file:

Reference/arcade/roms.rev2/136002-118.k1

Do not enumerate the containing ROM directory.

Do not inspect other ROM files.

Do not inspect MBUCOD.V05.txt.

Do not inspect screenshots, UserData, video, Temp, recordings, or unrelated reference material.

You may use the tracked historical Level 10 recovery report and tracked textual disassembly solely to establish the already-recorded signature/address method.

## Minimal access test

First perform the smallest safe read-only operation needed to determine whether the specific ROM file is accessible.

Do not modify the file.

Do not copy the ROM to another location.

Do not encode or reproduce its full contents.

Do not print a full hex dump.

Do not hash the entire file merely for this accessibility test unless an existing tool intrinsically requires a hash.

If read-only access itself is refused, stop immediately and report the exact observable refusal.

Do not attempt bypasses, alternate tools, file conversion, renaming, copying, archive creation, encoding, or other workarounds.

The purpose is to understand the block, not circumvent one.

## If access succeeds

If the specific ROM file can be read normally, proceed with one narrow engineering extraction using the exact historical Level 10 methodology.

Use the tracked Level 10 recovery record to recover only the Level 11 geometry facts required to unblock the next milestone:

- geometry lookup index for displayed Level 11;
- lev_remap[10];
- selected shape row;
- 16 ordered X values;
- 16 ordered Y values;
- 16 ordered angle values;
- corresponding lev_open value / open-closed topology;
- directly adjacent row-specific presentation metadata only if the historical Level 10 extraction method already establishes how to locate it.

Cross-check the extraction using an already-known row if the historical Level 10 method did that and the check requires no broader ROM exploration.

Do not extract unrelated ROM regions.

Do not search for additional gameplay routines.

The tracked textual disassembly has already supplied substantial Level 11 gameplay evidence. This task is about the missing geometry/table values.

## If access fails

If access is refused or blocked:

Stop.

Do not attempt another access route.

Record:

- the exact file requested;
- the exact operation attempted;
- the exact observable error/refusal message;
- whether the refusal appears before any file read;
- which tool/process reported the refusal, if observable;
- whether the evidence supports a platform/policy restriction, technical path/access problem, or remains inconclusive.

Do not claim that licensing caused the refusal unless the observable error explicitly states that.

Do not weaken security or attempt to circumvent a restriction.

## Repository protection

This task is read-only except for its own AI-Logs prompt/report records.

Do not modify:

- ROM files;
- production source;
- tests;
- configuration;
- durable behavior documentation;
- reference files.

Do not stage, commit, push, reset, restore, stash, clean, or rebase.

Do not add the ROM to Git.

Do not change .gitignore.

The ROM directory remains intentionally local and ignored.

## Report

Produce a concise diagnostic report with:

### Access result

State exactly one:

- PROMPT-IMPOSED BLOCK CONFIRMED
- TOOL / PLATFORM / POLICY BLOCK OBSERVED
- TECHNICAL ACCESS PROBLEM
- INCONCLUSIVE

### Evidence

Describe exactly what operation was attempted and what happened.

### Level 11 geometry

If access succeeded, record the narrowly recovered Level 11 geometry facts and evidence chain.

### Implications for Levels 12-16

State only what the result actually establishes.

If the same local evidence method remains usable, note that the remaining levels can use the same bounded recovery workflow where necessary.

If access is blocked independently of prompt restrictions, identify the exact observable blocker so a future plan can be based on facts rather than speculation.

Do not implement Level 11.

Save this prompt and report under the tracked AI-Logs convention.

Stop after the report.

TEMPEST_LOCAL_ROM_ACCESS_DIAGNOSTIC
