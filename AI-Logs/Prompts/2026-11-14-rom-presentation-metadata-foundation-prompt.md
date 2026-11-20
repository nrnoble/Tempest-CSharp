IMPORTANT COMPLETENESS CHECK

Read this entire user-provided task before taking any action.

Before using tools, reading project files, running commands, modifying files, or performing repository actions, locate the unique completion token near the end of this task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

Automatically appended VS Code/agent context after the user's token does not invalidate the token.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project or repository action.


AUTOMATIC / ACTIVE-EDITOR CONTEXT IS NOT AUTHORIZATION

Automatically surfaced active/recent/editor files are not authorized merely because VS Code supplied them.

Do not use such files unless this task expressly authorizes them or they are directly necessary to this bounded implementation.

DO NOT read TEMP/myNotes.md.


TASK

Implement a ROM-backed, data-only presentation-metadata foundation for current production Levels 1-9.

The metadata must preserve the verified Atari Rev 2 values for:

- displayed level number;
- zero-based ROM tube-table index selected through lev_remap;
- raw lev_scale byte;
- raw lev_y3d byte;
- raw lev_y2d low byte;
- raw lev_y2db high byte;
- combined signed 16-bit lev_y2d value;
- raw lev_open byte;
- open/closed interpretation.

This is an INERT METADATA milestone.

Do NOT connect this metadata to:

- Projection.Project;
- VectorRenderer;
- DisplayConfiguration;
- CenterY;
- YOffset;
- K;
- ZOffset;
- --portrait-perspective;
- playfield scale;
- portrait placement;
- level geometry;
- gameplay;
- transitions;
- recording/playback;
- HUD;
- current presentation output.

There must be no runtime visual, simulation, gameplay, control, timing, recording, or rendering change.

Do not stage, commit, or push.


CURRENT AUTHORITATIVE REPOSITORY STATE

Baseline remains:

branch:
master

HEAD:
385f603a94391756b2ca6cccd524dc17b281e782

commit:
Close out ROM-backed enemy-shot rendering

origin/master should match HEAD.

The current worktree intentionally includes:

- the uncommitted portrait/perspective candidate;
- the manually accepted portrait viewport border;
- the manually accepted F9 live pause / F10 screenshot feature;
- all directly related tests.

The latest implementation report recorded 14 modified tracked files, a clean index, and no non-ignored untracked files.

Before implementation run bounded checks of:

- branch;
- HEAD;
- origin/master;
- staged changes;
- tracked worktree changes;
- non-ignored untracked files;
- diff summary.

Read:

AI-Logs/Reports/2026-11-14-live-pause-screenshot-implementation-report.md

Use actual Git state rather than failing solely because the historical modified-file count has changed through legitimate current work.

Expected:

- master at HEAD/origin 385f603a94391756b2ca6cccd524dc17b281e782;
- clean index;
- current authorized presentation and pause/screenshot candidate only;
- no unrelated user work;
- no relevant non-ignored untracked files.

If unrelated or unexpected user work exists, STOP and report it.

Do not restore, reset, clean, discard, overwrite, stage, or commit any existing candidate work.


MANUAL ACCEPTANCE ALREADY COMPLETED

Preserve these manual findings:

- the 1-host-pixel blue portrait viewport border is perfect and frozen;
- F9 pause and F10 screenshot capture work perfectly and are frozen;
- shared portrait vertical placement -33.1 is accepted and frozen;
- endpoint-normalized perspective composition is accepted;
- Level 9 at approximately --portrait-perspective 125 is current manual calibration evidence, not a ROM value or production default;
- Levels 1-7 have fundamentally wrong base perspective that coefficient tuning does not fix;
- gameplay continues to work correctly.

Do not revisit or modify these features in this milestone.


AUTHORITATIVE REPORTS

Read:

AI-Logs/Reports/2026-11-14-rev2-presentation-table-extraction-report.md

AI-Logs/Reports/2026-11-14-rev2-presentation-mathematical-translation-report.md

The extraction report is authoritative for:

- raw per-tube bytes;
- signed lev_y2d interpretation;
- displayed-level-to-tube-table mapping;
- Level 9 cross-verification;
- evidence classifications.

The mathematical-translation report is authoritative for:

- per-field source semantics;
- current evidence limits;
- why values must remain inert;
- classification READY FOR ROM-BACKED METADATA FOUNDATION ONLY.

Do not reinterpret or convert the values beyond what those reports establish.


NO RAW-ROM READING

Do not reopen raw ROM files or enumerate raw ROM directories.

The values have already been securely extracted and cross-checked.

Use the extraction report as the ROM-backed data source.

Targeted reading of current C# source is allowed to integrate the metadata maintainably.

Do not inspect:

- screenshots;
- images;
- videos;
- MAME frames;
- Gemini material;
- Incitti source;
- web sources.


AUTHORITATIVE LEVELS 1-9 DATA

Implement exactly the following ROM-backed values.

Displayed Level 1:
- tube-table index: 0
- lev_scale: 0x18
- lev_y3d: 0x50
- lev_y2d low: 0x40
- lev_y2db high: 0xFF
- combined lev_y2d: 0xFF40
- signed lev_y2d: -192
- lev_open: 0x00
- closed

Displayed Level 2:
- tube-table index: 1
- lev_scale: 0x1C
- lev_y3d: 0x50
- lev_y2d low: 0x20
- lev_y2db high: 0xFF
- combined lev_y2d: 0xFF20
- signed lev_y2d: -224
- lev_open: 0x00
- closed

Displayed Level 3:
- tube-table index: 2
- lev_scale: 0x18
- lev_y3d: 0x50
- lev_y2d low: 0x40
- lev_y2db high: 0xFF
- combined lev_y2d: 0xFF40
- signed lev_y2d: -192
- lev_open: 0x00
- closed

Displayed Level 4:
- tube-table index: 3
- lev_scale: 0x0F
- lev_y3d: 0x68
- lev_y2d low: 0x80
- lev_y2db high: 0xFF
- combined lev_y2d: 0xFF80
- signed lev_y2d: -128
- lev_open: 0x00
- closed

Displayed Level 5:
- tube-table index: 4
- lev_scale: 0x18
- lev_y3d: 0x50
- lev_y2d low: 0x40
- lev_y2db high: 0xFF
- combined lev_y2d: 0xFF40
- signed lev_y2d: -192
- lev_open: 0x00
- closed

Displayed Level 6:
- tube-table index: 5
- lev_scale: 0x18
- lev_y3d: 0x50
- lev_y2d low: 0x40
- lev_y2db high: 0xFF
- combined lev_y2d: 0xFF40
- signed lev_y2d: -192
- lev_open: 0x00
- closed

Displayed Level 7:
- tube-table index: 6
- lev_scale: 0x18
- lev_y3d: 0x68
- lev_y2d low: 0x70
- lev_y2db high: 0xFF
- combined lev_y2d: 0xFF70
- signed lev_y2d: -144
- lev_open: 0x00
- closed

Displayed Level 8:
- tube-table index: 7
- lev_scale: 0x18
- lev_y3d: 0xB0
- lev_y2d low: 0x60
- lev_y2db high: 0x00
- combined lev_y2d: 0x0060
- signed lev_y2d: +96
- lev_open: 0xFF
- open

Displayed Level 9:
- tube-table index: 13
- lev_scale: 0x0C
- lev_y3d: 0xB0
- lev_y2d low: 0x40
- lev_y2db high: 0x01
- combined lev_y2d: 0x0140
- signed lev_y2d: +320
- lev_open: 0xFF
- open

Do not add guessed units.

Do not convert values into C# camera or screen coordinates.


METADATA TYPE DESIGN

Inspect current Levels/LevelDefinition/LevelLoader architecture before choosing the smallest maintainable data model.

Create one focused immutable metadata type conceptually representing:

RomPresentationMetadata

or another clear project-appropriate name.

The metadata should expose, at minimum:

- int DisplayedLevel
- int TubeTableIndex
- byte ScaleRaw
- byte CameraHeightRaw
- byte ScreenYOffsetLowRaw
- byte ScreenYOffsetHighRaw
- short ScreenYOffsetSigned
- byte OpenRaw
- bool IsOpen

Naming should make these distinctions clear:

- raw ROM byte;
- source-backed semantic interpretation;
- no active C# translation yet.

Avoid names that imply converted C# units, such as:

- ProjectionScale
- CameraY
- ScreenPixels
- CenterY
- YOffset

unless strongly qualified as raw/source metadata.

Prefer names such as:

- ReciprocalScaleRaw
- CameraHeightRaw
- ScreenYOffsetRaw
- ScreenYOffsetSigned
- OpenRaw

or similarly precise equivalents.

Do not add setters.

Do not add mutable static state.


DERIVED-FIELD POLICY

Store the authoritative raw bytes.

The combined signed 16-bit lev_y2d value may be:

A. stored explicitly and validated against the raw low/high bytes;

or

B. derived from low/high bytes through a small pure conversion.

Prefer the design that minimizes inconsistency.

The conversion must use:

- little-endian combination;
- signed 16-bit two's-complement interpretation.

For example:

combined unsigned = low | (high << 8)

signed = unchecked((short)combined)

Do not rely on host endianness.

IsOpen should be derived from the source-backed encoding:

- 0x00 = closed
- 0xFF = open

Reject or fail fast for unexpected OpenRaw values if construction is centralized and doing so does not complicate an inert data type.

Do not treat arbitrary nonzero as open unless source evidence supports that interpretation.


ASSOCIATION WITH LEVELS 1-9

The metadata must be authoritatively associated with each production Level 1-9.

Choose the smallest maintainable architecture after inspecting current source.

Acceptable approaches include:

1. a metadata property on LevelDefinition or the directly appropriate level descriptor;

2. a centralized immutable lookup keyed by displayed production level;

3. another existing configuration mechanism that naturally owns per-level source evidence.

Requirements:

- one authoritative metadata instance per displayed Level 1-9;
- no duplicate separate lookup tables containing the same values;
- easy retrieval for future projection-translation work;
- no renderer dependency;
- no debug-controller dependency;
- no gameplay-state mutation;
- no effect on level identity or progression;
- no implicit fallback values.

Do not associate values solely by open/closed topology.

Use the ROM-backed displayed-level-to-table-index mapping.


LEVELS 10-16

Do not implement production LevelDefinitions for Levels 10-16.

Do not create fake gameplay levels.

The extraction report contains all 16 table rows, but this requested foundation is specifically for current production Levels 1-9.

If the chosen metadata architecture can cleanly preserve the complete 16-entry ROM table without implying Levels 10-16 are implemented, evaluate that option carefully.

Default scope remains:

- authoritative metadata attached/available for Levels 1-9;
- no requirement to encode Levels 10-16 yet.

Do not broaden scope merely because the raw table exists.


LEVEL 9 EXISTING ROM EVIDENCE

Current LevelLoader source already contains Level 9 ROM-evidence constants.

Do not leave two independently authoritative copies of the same Level 9 values.

Inspect the existing Level9RomEvidence structure/constants and choose the smallest consolidation strategy.

Requirements:

- existing Level 9 tests and documentation remain meaningful;
- Level 9 values agree exactly with the new shared metadata foundation;
- no duplicated constants can drift independently;
- do not delete useful Level 9 evidence without replacing its role cleanly.

Possible clean outcomes:

- Level9RomEvidence references or exposes the centralized metadata;
- tests migrate to the shared metadata while retaining Level 9-specific assertions;
- another minimal consolidation supported by current structure.

Do not change Level 9 behavior.


OPEN/CLOSED TOPOLOGY SEPARATION

Do not make lev_open replace existing gameplay/topology truth in this milestone.

Existing LevelDefinition/open-tube behavior remains authoritative for current gameplay.

The metadata's IsOpen is ROM evidence only.

Add tests confirming that the ROM metadata agrees with current LevelDefinition topology for Levels 1-9.

Do not wire metadata IsOpen into movement, lane count, tube drawing, collision, or progression.


EVIDENCE COMMENTS

Add concise maintainability-focused comments explaining:

- values are extracted from Atari Tempest Rev 2 per-tube presentation tables;
- displayed level maps to a zero-based tube-table index through lev_remap;
- lev_scale is source-described as reciprocal level scale;
- lev_y3d is source-described as 3-D Y offset / camera height;
- lev_y2d/lev_y2db form a signed 16-bit post-projection placement value;
- lev_open is 0x00 closed / 0xFF open;
- no active mathematical translation to C# projection units is implemented yet;
- metadata is intentionally inert pending mathbox/DVG translation work.

Do not paste long report text into source comments.

Do not claim exact physical-monitor units.


INERTNESS REQUIREMENT

Prove through source scope and tests that adding metadata does not alter:

- LevelDefinition.Points;
- CenterY;
- YOffset;
- Position;
- Depth;
- K;
- ZOffset;
- endpoint-normalized perspective;
- playfield size or placement;
- viewport;
- gameplay;
- lane count;
- movement;
- collision;
- enemy behavior;
- wave behavior;
- scoring;
- progression;
- transitions;
- recording;
- playback;
- RNG;
- pause/screenshot behavior.

The new metadata must have zero consumers in active projection/rendering/gameplay code.

If a constructor/property addition causes large unrelated churn, reconsider the ownership design rather than broadening edits.


TEST REQUIREMENTS

Add focused tests covering at minimum:

1. Levels 1-9 metadata exists.

2. Displayed level values are exactly 1 through 9.

3. Tube-table indices are exactly:
   0, 1, 2, 3, 4, 5, 6, 7, 13.

4. Raw lev_scale values match:
   0x18, 0x1C, 0x18, 0x0F, 0x18, 0x18, 0x18, 0x18, 0x0C.

5. Raw lev_y3d values match:
   0x50, 0x50, 0x50, 0x68, 0x50, 0x50, 0x68, 0xB0, 0xB0.

6. Raw low/high lev_y2d bytes match the authoritative matrix.

7. Signed lev_y2d values match:
   -192, -224, -192, -128, -192, -192, -144, +96, +320.

8. Raw lev_open values match:
   0x00 for Levels 1-7;
   0xFF for Levels 8-9.

9. IsOpen derives correctly.

10. Metadata open/closed evidence agrees with current production topology.

11. Level 9 values remain exactly cross-verified:
    scale 0x0C;
    camera height 0xB0;
    screen Y offset 0x0140 / +320;
    open 0xFF;
    tube index 13.

12. Signed combination is explicitly little-endian and correct for negative and positive examples.

13. Invalid lev_open values are rejected if the design centralizes construction validation.

14. Metadata lookup has no silent fallback for unsupported displayed levels.

15. Existing LevelLoader/LevelDefinition behavior remains unchanged.

16. Existing presentation, projection, gameplay, progression, playback, and pause/screenshot tests remain unchanged and pass.

Do not add snapshot-heavy tests if direct value assertions are clearer.


IMPLEMENTATION SCOPE

Prefer a small scope such as:

- one new Levels metadata type/file;
- LevelLoader or one centralized level-metadata catalog;
- one or a small number of directly relevant test files;
- minimal LevelDefinition/configuration association if justified.

Do not modify:

- Projection.cs;
- VectorRenderer.cs;
- DisplayConfiguration.cs;
- Program.cs;
- InputSystem.cs;
- screenshot code;
- DebugController.cs;
- gameplay systems;
- recording/playback schema;
- project files;
- .gitignore.

If integrating metadata requires edits to those prohibited active paths, STOP and reconsider the metadata ownership design.


NO ACTIVE PRESENTATION EXPERIMENT

Do not:

- change --portrait-perspective;
- change its default;
- set Level 9 to 125;
- add per-level perspective values;
- translate lev_y3d into YOffset;
- translate lev_y2d into CenterY;
- translate lev_scale into K/ZOffset/geometry scale;
- add open/closed camera branches;
- change enclosed-tube perspective;
- fix the separate Level 8 issue.

Those are future milestones.


BOUNDED VALIDATION

Run focused tests for:

- new metadata;
- LevelLoader/LevelDefinition;
- topology agreement;
- Level 9 existing evidence;
- directly affected level configuration.

Use a VSTest hang timeout of no more than 3 minutes.

If focused validation passes:

1. run the full test suite with a 3-minute hang timeout;
2. run Release build;
3. run git diff --check.

Do not repeatedly rerun unchanged failures.

Do not use stale binaries after a failed build.

Follow the established Windows-security/stall self-stop policy.


MANUAL ACCEPTANCE

No interactive game launch is required for an inert metadata-only milestone.

Automated validation and code review are the acceptance evidence.

Do not claim any visual, gameplay, or projection improvement.

The expected runtime result is deliberately:

NO VISIBLE OR GAMEPLAY CHANGE.


DOCUMENTATION AND AI LOGGING

Preserve this prompt under AI-Logs/Prompts.

Create:

AI-Logs/Reports/2026-11-14-rom-presentation-metadata-foundation-report.md

Include:

- starting Git state;
- reports/source inspected;
- files changed;
- metadata type/ownership design;
- exact Levels 1-9 values;
- raw-to-signed lev_y2d derivation;
- lev_open validation;
- displayed-level/tube-index mapping;
- Level 9 consolidation/cross-verification;
- topology agreement;
- inertness proof;
- zero active consumers;
- tests added/updated;
- focused test result;
- full-suite result;
- Release build;
- git diff --check;
- final Git state;
- confirmation no runtime behavior changed;
- confirmation nothing staged/committed/pushed;
- confirmation no visual/Gemini/Incitti/web/raw-ROM material was inspected.


DURABLE DOCUMENTATION

Do not update TEMPEST-BEHAVIOR-SPEC.md because this milestone does not change behavior.

Do not broadly rewrite TEMPEST-DEVELOPMENT-NOTES.md.

If a brief durable development note is genuinely useful, add only a narrow factual statement that:

- ROM-backed Levels 1-9 presentation metadata is now preserved as inert source data;
- active mathematical translation remains unresolved and unused.

However, prefer leaving durable documentation unchanged unless existing project conventions clearly require the note.

Do not make accepted presentation claims.


NO COMMIT OR PUSH

Do not stage.
Do not commit.
Do not push.


FAIL-CLOSED CONDITIONS

STOP rather than broaden scope if:

- exact values conflict with the extraction report;
- Level 9 does not cross-verify;
- metadata association would change runtime behavior;
- metadata requires active projection/rendering consumers;
- a large constructor migration becomes necessary;
- gameplay topology would be changed;
- Levels 10-16 must be fabricated;
- formula translation would be required;
- existing candidate work cannot be preserved.

Report PARTIAL or BLOCKED and leave a diagnosable worktree.


FINAL RESPONSE

Report:

1. Whether metadata foundation completed.
2. Starting/final Git state.
3. Files changed.
4. Metadata type and ownership.
5. Levels 1-9 values preserved.
6. Displayed-level-to-tube-index mapping.
7. lev_y2d signed derivation.
8. lev_open handling.
9. Level 9 evidence consolidation.
10. Topology-agreement result.
11. Inertness/zero-consumer result.
12. Confirmation no presentation/gameplay behavior changed.
13. Focused tests.
14. Full suite.
15. Release build.
16. git diff --check.
17. Any blocked or partial work.
18. Report path.
19. Confirmation nothing staged, committed, or pushed.
20. Confirmation no prohibited sources/assets were inspected.

Do not implement active projection translation.

TEMPEST_ROM_PRESENTATION_METADATA_FOUNDATION_20261114