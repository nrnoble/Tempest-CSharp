IMPORTANT COMPLETENESS CHECK

Read this entire user-provided task before taking any action.

Before using tools, reading project files, running commands, modifying files, or performing repository actions, locate the unique completion token near the end of this task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code or agent-generated reminders, tagged-file information, attachment metadata, active/current editor files, or other automatically appended context may appear after the user's completion token and do not invalidate it.

This check exists to detect an incomplete or truncated user task, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before taking any project or repository action and explain why.


AUTOMATIC / ACTIVE-EDITOR CONTEXT IS NOT AUTHORIZATION

Files automatically surfaced by VS Code because they are open, active, recently viewed, suggested, or otherwise included as editor/chat context are NOT authorized merely because VS Code shows them to the agent.

Do not read or use automatically surfaced files unless this task expressly authorizes them or they are directly necessary within the bounded investigation below.

In particular:

DO NOT read TEMP/myNotes.md.


TASK

Perform a tightly bounded READ-ONLY Rev 2 ROM verification/extraction milestone to close the specific source-data gap identified by:

AI-Logs/Reports/2026-11-14-levels1-9-rom-camera-perspective-readonly-investigation-report.md

Do NOT implement anything.

The sole purpose is to recover and verify the actual Rev 2 per-tube data required to understand Atari's tube presentation system.

Target fields:

- lev_remap
- lev_scale
- lev_y3d
- lev_y2d
- lev_y2db
- lev_open

Then determine how displayed production Levels 1-9 map through lev_remap to tube-table indices.

No C# behavior changes.
No geometry changes.
No presentation changes.
No perspective changes.
No test changes.
No staging, commit, or push.


CURRENT PROJECT STATE

Baseline:

branch:
master

HEAD:
385f603a94391756b2ca6cccd524dc17b281e782

commit:
Close out ROM-backed enemy-shot rendering

origin/master should match HEAD.

The current portrait/perspective candidate is intentionally modified and unstaged.

Before analysis:

- inspect branch;
- HEAD;
- origin/master;
- index;
- tracked worktree;
- non-ignored untracked files;
- diff summary.

Compare the actual candidate state with:

AI-Logs/Reports/2026-11-14-whole-game-presentation-human-visible-perspective-calibration-report.md

and:

AI-Logs/Reports/2026-11-14-levels1-9-rom-camera-perspective-readonly-investigation-report.md

Expected:

- clean index;
- current portrait/perspective candidate only;
- no unrelated user work.

If unrelated unexpected work exists, STOP.

Do not alter repository state.


AUTHORITATIVE PRIOR FINDING

Read:

AI-Logs/Reports/2026-11-14-levels1-9-rom-camera-perspective-readonly-investigation-report.md

That report established from the textual disassembly that Atari has distinct per-tube fields:

- lev_scale: reciprocal level-scale/projection input;
- lev_y3d: 3-D Y offset / camera height before projection;
- lev_y2d + lev_y2db: signed 2-D Y placement after projection;
- lev_open: topology/open-edge input;
- lev_remap: displayed-level-to-tube-table mapping.

The textual disassembly contains .chunk placeholders rather than the table bytes.

This task is expressly authorized to inspect the project-local Rev 2 ROM data only to close that table-value gap.


STRICT RAW-ROM AUTHORIZATION

Raw ROM access is authorized ONLY for the smallest set of project-local Rev 2 ROM files/bytes necessary to recover and verify:

- lev_remap;
- lev_scale;
- lev_y3d;
- lev_y2d;
- lev_y2db;
- lev_open;

plus any immediately required address/bank mapping needed to locate those tables correctly.

Do NOT perform a broad ROM reverse-engineering audit.

Do NOT:

- modify any ROM;
- copy ROMs into tracked project locations;
- add ROM data to production code;
- dump unrelated ROM regions;
- inspect unrelated gameplay tables;
- inspect graphics/sound data;
- inspect screenshots/images/videos;
- inspect Gemini material;
- inspect Incitti source;
- search the web.

If locating the table requires reading a ROM file containing unrelated bytes, restrict interpretation/output to the explicitly authorized table ranges and their immediate address-mapping evidence.


VERIFY ROM REVISION / PROVENANCE

Before extracting values:

- identify the project-local raw files actually used;
- establish that the files correspond to the intended Tempest Rev 2 ROM set using existing project-local provenance/documentation where available;
- report filenames and existing known identifiers/checksums only if already available locally or computed read-only;
- do not download comparison ROMs;
- do not search the web.

If the local files cannot be established as the intended Rev 2 set with reasonable confidence, STOP and classify ROM PROVENANCE BLOCKED.


ADDRESS-MAPPING VALIDATION

Do not assume textual source addresses map directly to offsets within one ROM file.

Determine the required address/bank/layout mapping from project-local documentation/source and the ROM organization itself.

For each extracted table:

- source label;
- CPU/source address where established;
- backing ROM file;
- file offset;
- entry width;
- entry count;
- raw bytes;
- signed interpretation where applicable.

Cross-check adjacent or known data only as minimally necessary to establish that the mapping is not off by one, reversed, or using the wrong ROM bank.

Do not infer a table merely because a candidate byte sequence looks plausible.


EXTRACT THE SIX TARGET TABLES

Recover all available entries for:

1. lev_remap
2. lev_scale
3. lev_y3d
4. lev_y2d
5. lev_y2db
6. lev_open

Expected conceptual size from the prior investigation is 16 entries per per-tube field where the source says so.

Do not force that count if raw/source evidence establishes something different.

Present raw hexadecimal values.

Also provide interpreted values only where the signed/combined representation is established by the source.

For lev_y2d + lev_y2db:

- combine low/high bytes exactly according to source semantics;
- report combined 16-bit hexadecimal;
- report signed decimal interpretation if source semantics establish signed use.

Do not invent physical units.


MAP DISPLAYED LEVELS TO TUBE INDICES

Use lev_remap and the established get_tube_no/runtime mapping to determine displayed Levels 1-16 to the corresponding tube-table index where source/ROM evidence permits.

At minimum provide exact mappings for current production Levels 1-9.

Be explicit about any zero-based versus one-based conversion.

Do not infer mapping from the current C# LevelLoader order if ROM evidence is available.


LEVELS 1-9 ROM PRESENTATION MATRIX

Produce a compact matrix for displayed Levels 1-9 containing:

- displayed level;
- mapped tube-table index;
- lev_scale raw byte;
- lev_y3d raw byte;
- lev_y2d low;
- lev_y2db high;
- combined signed lev_y2d value;
- lev_open raw byte and open/closed interpretation.

If additional directly adjacent data is necessary to interpret one of these fields, include only that minimum evidence.

Do not include unrelated tables.


CROSS-CHECK EXISTING LEVEL 9 EVIDENCE

Current C# source reportedly records Level 9 ROM evidence:

- Scale = 0x0C
- Y3d = 0xB0
- Y2d = 0x0140
- Y2dLow = 0x40
- Y2dHigh = 0x01
- Open = 0xFF

Use this as a strong cross-check.

Determine whether the extracted Rev 2 data for displayed Level 9 agrees.

If it does not agree:

STOP before drawing presentation conclusions.

Diagnose only enough to determine whether:

- displayed-level mapping is wrong;
- ROM address mapping is wrong;
- revision/provenance differs;
- existing C# evidence is inconsistent.

Do not silently choose whichever values seem preferable.


CROSS-CHECK LEVEL 8 IF LOCAL EVIDENCE EXISTS

Search current C# source only for existing explicitly labeled ROM evidence associated with Level 8 presentation fields.

If explicit evidence exists, compare it.

If not, state that no independent C# Level 8 camera-table cross-check was available.

Do not manufacture one from CenterY/YOffset.


NO C# TRANSLATION YET

Do NOT map raw table values into:

- CenterY;
- YOffset;
- K;
- ZOffset;
- --portrait-perspective;
- playfield scale;
- geometry coordinates.

Do NOT implement a translation.

Do NOT propose numeric C# constants beyond reporting the extracted Atari data.

This milestone closes the evidence gap only.

A separate subsequent investigation should derive the proper mathematical translation.


EVIDENCE CLASSIFICATION

Classify each finding explicitly:

ROM-BACKED:
direct raw-ROM value with verified table mapping.

SOURCE-BACKED:
textual disassembly semantics/address/routine behavior.

CROSS-VERIFIED:
raw value agrees with independently recorded current C# ROM evidence.

PROJECT TRANSLATION:
existing C# values not established as literal ROM fields.

HYPOTHESIS:
anything not directly established.

Do not blur these categories.


NO VISUAL ANALYSIS

Do not inspect:

- screenshots;
- MAME frames;
- C# frames;
- videos;
- recordings;
- leaderboard images;
- other visual assets.

Manual visual findings remain external evidence and are not needed to extract bytes.


BOUNDED TOOLING

Read-only scripts/commands are allowed for:

- checksums;
- byte extraction;
- address conversion;
- formatting tables;
- bounded cross-check calculations.

Any temporary script must remain outside tracked source and be removed before completion.

Do not modify raw ROM files.

Do not install dependencies.

Keep all reads narrowly bounded.


DIAGNOSTIC CLASSIFICATION

Finish with exactly one primary classification:

ROM TABLE EXTRACTION COMPLETE

Use if the six required tables and Levels 1-9 mappings are securely extracted and cross-checked enough for a mathematical translation investigation.

ROM ADDRESS MAPPING BLOCKED

Use if the correct ROM bytes cannot be mapped confidently from source addresses.

ROM PROVENANCE BLOCKED

Use if the local files cannot be established as the intended revision.

ROM/C# EVIDENCE CONFLICT

Use if verified ROM values conflict materially with existing Level 9 evidence and the discrepancy is not resolved.

PARTIAL ROM DATA

Use if some required target fields can be verified but others cannot.

Do not implement anything regardless of classification.


REPORT

Preserve this substantive prompt under AI-Logs/Prompts according to project instructions.

Create:

AI-Logs/Reports/2026-11-14-rev2-presentation-table-extraction-report.md

Include:

- starting Git state;
- authorized raw-ROM files read;
- provenance/revision evidence;
- address/bank/file-offset method;
- target table labels and locations;
- raw table bytes;
- interpreted signed values where source-backed;
- full lev_remap mapping where available;
- displayed Levels 1-9 presentation matrix;
- Level 9 cross-check;
- Level 8 cross-check if available;
- conflicts or uncertainties;
- evidence classification;
- primary diagnostic classification;
- exact evidence now available for the next translation milestone;
- final Git state;
- confirmation candidate files were unchanged;
- confirmation nothing staged/committed/pushed;
- confirmation no visual assets, Gemini material, Incitti source, or web sources were inspected.


FINAL RESPONSE

Report:

1. Whether extraction completed.
2. Primary classification.
3. Starting/final Git state.
4. Raw ROM files read.
5. Revision/provenance result.
6. Address-mapping method.
7. lev_remap values.
8. lev_scale values.
9. lev_y3d values.
10. lev_y2d low values.
11. lev_y2db high values.
12. combined signed lev_y2d values.
13. lev_open values.
14. displayed Levels 1-9 to tube-table mapping.
15. Levels 1-9 presentation matrix.
16. Level 9 cross-check result.
17. Level 8 cross-check result if available.
18. Any unresolved discrepancy.
19. What evidence is now available for mathematical translation.
20. Report path.
21. Confirmation candidate source/test files were unchanged.
22. Confirmation nothing staged, committed, or pushed.
23. Confirmation no unauthorized assets/sources were inspected.

Do not implement any C# translation.

TEMPEST_REV2_PRESENTATION_TABLE_EXTRACTION_20261114