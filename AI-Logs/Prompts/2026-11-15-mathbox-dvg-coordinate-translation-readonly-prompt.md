IMPORTANT COMPLETENESS CHECK

Read this entire user-provided task before taking any action.

Before using tools, reading project files, running commands, modifying files, or performing repository actions, locate the unique completion token near the end of this task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

Automatically appended VS Code/agent context after the user's token does not invalidate the token.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project or repository action.


AUTOMATIC / ACTIVE-EDITOR CONTEXT IS NOT AUTHORIZATION

Automatically surfaced active/recent/editor files are not authorized merely because VS Code supplied them.

Do not use such files unless this task expressly authorizes them or they are directly necessary to the bounded investigation.

DO NOT read TEMP/myNotes.md.


TASK

Perform a tightly bounded READ-ONLY investigation of the remaining Atari Tempest Rev 2 mathbox and DVG/vector-coordinate translation gap.

Do NOT implement anything.

The immediate purpose is to determine whether project-local evidence is sufficient to derive a faithful numerical translation from the now-preserved ROM presentation metadata:

- lev_scale;
- lev_y3d;
- signed lev_y2d;

through Atari's mathbox and vector-generator coordinate pipeline into a modern C# projection/presentation model.

The recently added RomPresentationMetadata foundation is inert and must remain inert.

Do not connect it to projection, rendering, gameplay, transitions, HUD, or the --portrait-perspective diagnostic.

Do not stage, commit, or push.


CURRENT AUTHORITATIVE REPOSITORY STATE

Baseline:

branch:
master

HEAD:
385f603a94391756b2ca6cccd524dc17b281e782

commit:
Close out ROM-backed enemy-shot rendering

origin/master should match HEAD.

The current worktree intentionally contains:

- the portrait/perspective candidate;
- manually accepted blue viewport border;
- manually accepted F9 pause/F10 screenshot functionality;
- inert ROM-backed Levels 1-9 presentation metadata;
- directly related tests.

Before analysis perform bounded read-only checks of:

- branch;
- HEAD;
- origin/master;
- index;
- tracked worktree;
- non-ignored untracked files;
- diff summary.

Read:

AI-Logs/Reports/2026-11-14-rom-presentation-metadata-foundation-report.md

Use actual Git state rather than relying only on historical modified-file counts.

Expected:

- master at HEAD/origin 385f603a94391756b2ca6cccd524dc17b281e782;
- clean index;
- current authorized candidate work only;
- the two metadata/test files may remain non-ignored untracked because the candidate has not been committed;
- no unrelated user work.

If unrelated or unexpected work exists, STOP.

Do not alter repository state.


AUTHORITATIVE REPORTS

Read:

AI-Logs/Reports/2026-11-14-rev2-presentation-table-extraction-report.md

AI-Logs/Reports/2026-11-14-rev2-presentation-mathematical-translation-report.md

AI-Logs/Reports/2026-11-14-rom-presentation-metadata-foundation-report.md

The table-extraction report is authoritative for raw Rev 2 values and displayed-level mapping.

The mathematical-translation report is authoritative for:

- CPU-side setup and coordinate flow;
- known lev_scale semantics;
- known lev_y3d semantics;
- known signed lev_y2d semantics;
- axis serialization findings;
- the unresolved mathbox transfer and DVG/vector-unit conversion.

The metadata-foundation report is authoritative for the current inert C# data model.

Do not reinterpret verified raw values.


MANUAL PRESENTATION EVIDENCE

Preserve these observations as MANUAL evidence only:

- Level 9 at approximately --portrait-perspective 125 appears about right;
- Level 8 may accept similar perspective strength but has a separate unresolved presentation problem;
- Levels 1-7 have fundamentally wrong base perspective that the diagnostic coefficient does not fix;
- gameplay remains correct;
- extreme diagnostic values distort presentation but have not changed gameplay;
- no clipping was observed during Neal's manual diagnostic exploration.

Do not use these observations to fit or guess the mathbox equation.

Do not make 125 a ROM value or production default.


FROZEN CURRENT IMPLEMENTATION

Do not modify or recommend immediate changes to:

- RomPresentationMetadata;
- its catalog;
- Level 9 evidence forwarding;
- Projection.Project;
- endpoint-normalized portrait perspective;
- K=70;
- ZOffset=5;
- playfield scale 1.05;
- shared vertical placement -33.1;
- viewport dimensions;
- viewport border;
- CenterY;
- YOffset;
- level geometry;
- gameplay;
- transitions;
- pause/screenshot functionality;
- recording/playback.

This is a source/evidence investigation only.


NO VISUAL-ASSET ANALYSIS

Do not inspect:

- screenshots;
- images;
- videos;
- MAME frames;
- C# frames;
- gameplay recordings;
- contact sheets;
- Gemini material;
- Incitti implementation;
- web sources.

Do not analyze the screenshots already discussed in conversation.

This investigation concerns project-local executable/source evidence and mathematics.


AUTHORIZED LOCAL EVIDENCE

Targeted bounded inspection is authorized for project-local materials directly relevant to the mathbox and vector-generator pipeline, including where present:

- annotated Tempest disassembly/source;
- mathbox PROM dumps or decoded mathbox data;
- DVG/vector-generator source or documentation stored locally;
- existing project-local ROM-analysis reports;
- existing tests or tools that decode mathbox/DVG behavior;
- current C# rendering/projection source for comparison;
- raw Rev 2 ROM bytes only if a specific bounded lookup is essential and not already available from the extraction report.

Do not perform a broad ROM audit.

Do not read unrelated gameplay tables.

Do not modify any reference material.

If raw ROM access becomes necessary, state the exact file/range and why in the report.


PRIMARY QUESTION

Can the original Atari coordinate transformation be reconstructed numerically and faithfully enough from project-local evidence to implement one shared C# projection routine driven by the preserved per-level metadata?

The required transformation stages are conceptually:

model/tube coordinate
-> relative X/Y formation
-> lev_scale-adjusted depth input
-> mathbox operation
-> sign restoration
-> signed lev_y2d placement
-> DVG/vector-coordinate serialization
-> coordinate-axis/origin/unit conversion
-> C# portrait presentation

Determine which stages are now fully known, partially known, or unknown.


MATHBOX TRANSFER INVESTIGATION

Investigate the specific mathbox operation used by the coordinate routine.

The prior mathematical report represented the unresolved operation as:

M(magnitude, depthInput)

where:

depthInput = pointDepth + lev_scale

and magnitude is either:

abs(pointY - lev_y3d)

or:

abs(pointX - 0x80)

Determine, from project-local evidence where possible:

- exact mathbox command/entry point;
- PROM sequence or microcode involved;
- input register widths;
- output register widths;
- signed or unsigned behavior;
- fixed-point radix;
- internal coefficient or normalization constant;
- numerator/denominator order;
- multiplication/division/reciprocal behavior;
- rounding/truncation;
- saturation;
- zero and near-zero denominator behavior;
- output range;
- whether X and Y requests use exactly the same transfer;
- whether aspect correction is embedded in the mathbox operation.

Do not assume:

K * magnitude / depth

unless project-local evidence proves or strongly derives it.

If full microcode decoding is required, keep the investigation bounded to this coordinate-transform operation.


MATHBOX PROM / MICROCODE EVIDENCE

If project-local mathbox PROM data or decoded microcode exists:

- identify exact files;
- identify exact offsets/commands used by the coordinate routine;
- trace only the relevant operation;
- reconstruct the arithmetic step by step;
- label any uncertain interpretation.

If only raw PROM data exists and decoding it would require a major independent reverse-engineering project:

STOP short of implementation conclusions.

Classify the remaining work accurately rather than inventing a transfer function.


KNOWN-INPUT / KNOWN-OUTPUT PAIRS

Search project-local materials for known coordinate-conversion examples or tests that provide both:

- source/model coordinate and depth inputs;
- final mathbox or DVG/vector output.

Potential evidence may include:

- emulator tests;
- mathbox tests;
- disassembly comments;
- diagnostic captures with numeric vector lists;
- existing ROM-analysis reports;
- current C# evidence comments.

Use only numeric/source artifacts. Do not inspect visual screenshots or recordings.

If known pairs exist, use them to validate the reconstructed transfer.

Do not fit an arbitrary function from too few examples.


DVG / VECTOR-COORDINATE TRANSLATION

The prior investigation established that the coordinate blocks are serialized with an axis-order relationship that is not immediately equivalent to C# screen X/Y.

Derive as far as local evidence allows:

- which mathbox result word becomes DVG/vector X;
- which becomes DVG/vector Y;
- signed bit width;
- discarded/high bits;
- vector-generator coordinate origin;
- coordinate direction/sign;
- logical vector-space range;
- center/origin used by the game;
- any scaling before display-list emission;
- whether axis swapping is purely serialization or reflects physical monitor orientation;
- how portrait monitor orientation affects modern host-space interpretation.

Do not infer final host pixels from screenshot dimensions.


VECTOR UNITS TO C# LOGICAL UNITS

Determine whether project-local evidence supports a conversion from Atari vector units to:

- C# legacy 800 x 600 logical coordinates;
- C# 600 x 800 portrait logical coordinates;
- or an intermediate canonical vector space.

If exact conversion is unavailable, determine what minimum calibration reference would be needed.

Separate:

- exact mathematical unit conversion;
- arbitrary host-window scaling;
- portrait orientation;
- current project presentation translation.

Do not use the current 1.05 or 1.2 presentation scales as substitutes for an unresolved Atari vector-unit conversion.


ATARI DISPLAY ORIENTATION

Determine from project-local source/documentation whether the original output coordinate orientation requires:

- axis swap;
- sign inversion;
- rotation;
- translation;
- coordinate clipping.

Be explicit about the distinction between:

- mathbox source components;
- DVG serialized coordinates;
- physical portrait monitor orientation;
- current C# screen-down coordinate system.

Do not guess from likely arcade-monitor rotation conventions.


LEV_SCALE NUMERICAL ROLE

Using the reconstructed mathbox transfer if available, determine exactly how these source values alter the projected result:

- 0x18
- 0x1C
- 0x0F
- 0x0C

Establish whether smaller lev_scale values produce greater or lesser apparent size/depth effect under the actual transfer.

Do not describe lev_scale as “scale” in a modern multiplicative sense without showing the source-backed numerical role.


LEV_Y3D NUMERICAL ROLE

Using the reconstructed transfer if available, determine how representative values:

- 0x50
- 0x68
- 0xB0

alter the projected camera/elevation relationship for identical model points and depths.

Show sign/origin implications.

Do not map directly to C# YOffset without a proven coordinate conversion.


LEV_Y2D NUMERICAL ROLE

Using DVG/vector-unit evidence where available, determine how:

- -224
- -192
- -144
- -128
- +96
- +320

shift the final vector output.

Establish the relevant axis after serialization/orientation.

Do not map directly to C# CenterY without proving axis, origin, units, and direction.


REPRESENTATIVE WORKED EXAMPLES

If the transfer and coordinate conversion can be established, produce worked examples for Levels:

- 1
- 4
- 7
- 8
- 9

Use a small set of representative model-relative points/depths.

Show:

- ROM metadata;
- relative-coordinate formation;
- depth input;
- mathbox input;
- mathbox output;
- signed placement;
- DVG/vector coordinates;
- converted canonical/C# coordinates where justified.

If exact model points or depths aren't available in matching ROM units, use symbolic examples and clearly state the remaining conversion gap.

Do not mix current C# point coordinates with Atari byte coordinates without a proven unit mapping.


COMPARE AGAINST CURRENT C# FORM

Current C# conceptually uses:

x = CenterX + worldX * K / (z + ZOffset)

y = CenterY + (worldY + YOffset) * K / (z + ZOffset)

plus an endpoint-normalized portrait-perspective correction and post-projection camera.

Determine whether the reconstructed Atari pipeline can be expressed as:

A. the same functional form with converted per-level constants;

B. a similar reciprocal-depth form but with different axes/origins/units;

C. a materially different transfer requiring a revised shared projection equation;

D. still indeterminate.

Do not modify current code.


LEVEL 1 VERSUS LEVEL 9 TEST

Use ROM-backed metadata and the reconstructed transfer to explain, if evidence permits, why Level 1 and Level 9 would have substantially different base presentation.

Level 1:
- lev_scale 0x18
- lev_y3d 0x50
- lev_y2d -192

Level 9:
- lev_scale 0x0C
- lev_y3d 0xB0
- lev_y2d +320

Identify which differences affect:

- apparent convergence/depth;
- camera elevation;
- final placement;
- apparent size.

Relate the result to manual observations cautiously.

Do not fit the equation to the observation.


CURRENT INERT METADATA

Inspect the newly added metadata type/catalog only enough to determine whether it stores every raw field needed by a future implementation.

Report:

- sufficient as-is;
- missing a required raw/source field;
- contains a naming/representation issue.

Do not modify it.

Do not add active consumers.


GAMEPLAY GEOMETRY PROTECTION

Determine whether the reconstructed Atari camera/presentation inputs can remain entirely in rendering/projection.

State explicitly whether any evidence requires changing:

- lanes;
- topology;
- LevelDefinition.Points;
- collision geometry;
- enemy/player paths.

Manual testing says gameplay remains correct.

Do not recommend geometry changes unless direct source evidence establishes an import error.


MINIMUM FUTURE IMPLEMENTATION ARCHITECTURE

If sufficient evidence exists, define the smallest future active architecture conceptually.

Possible outcome:

ROM presentation metadata
-> Atari-compatible canonical projection
-> current portrait presentation mapping
-> host viewport

Do not implement.

Identify:

- which active C# types would eventually consume metadata;
- whether existing CenterY/YOffset remain;
- whether K/ZOffset remain for transitions;
- how endpoint-normalized portrait perspective would interact;
- whether projection can be introduced behind a diagnostic selector;
- how legacy presentation remains preserved.

Do not design multiple competing systems unnecessarily.


VALIDATION ORACLE

If implementation becomes possible, identify what would serve as an objective automated oracle.

Potential oracles may include:

- known ROM/mathbox input-output pairs;
- emulator mathbox tests;
- numeric vector-list outputs;
- source-derived invariants;
- current gameplay-geometry invariants.

Manual screenshots alone are not a sufficient automated oracle.

Determine whether the project currently possesses one.

If no objective numeric oracle exists, state that limitation.


EVIDENCE CLASSIFICATION

Label conclusions:

ROM-BACKED
SOURCE-BACKED
PROM/MICROCODE-BACKED
MATHEMATICAL DERIVATION
CROSS-VERIFIED
PROJECT TRANSLATION
MANUAL OBSERVATION
HYPOTHESIS
UNKNOWN

Do not claim exactness without corresponding evidence.


DECISION MATRIX

Evaluate:

A. Existing C# reciprocal projection plus converted per-level constants.

B. New shared Atari-compatible projection path using ROM metadata.

C. Canonical Atari vector-space projection followed by current portrait mapping.

D. Metadata foundation only while keeping current presentation manually calibrated.

E. Per-level hand tuning.

F. Open/closed-specific cameras.

For each report:

- evidence support;
- implementation risk;
- maintainability;
- fidelity potential;
- validation availability;
- remaining gaps.

Do not implement.


DIAGNOSTIC CLASSIFICATION

Finish with exactly one primary classification:

READY FOR BOUNDED ATARI-COMPATIBLE PROJECTION IMPLEMENTATION

Use only if the transfer, units, orientation, and validation oracle are sufficiently established.

READY FOR CANONICAL VECTOR-SPACE FOUNDATION ONLY

Use if mathbox output/vector orientation is understood but final host conversion still needs manual calibration.

MATHBOX MICROCODE DECODING REQUIRED

Use if the transfer cannot be responsibly derived without decoding additional PROM/microcode behavior.

VECTOR-UNIT / ORIENTATION GAP

Use if mathbox transfer is understood but DVG-to-C# conversion remains unresolved.

NUMERIC VALIDATION ORACLE GAP

Use if a plausible translation exists but cannot yet be tested objectively.

INSUFFICIENT LOCAL EVIDENCE

Use if project-local materials do not support responsible next implementation.

Do not choose a ready classification merely to continue coding.


BOUNDED TOOLING

Small read-only scripts are authorized for:

- PROM/microcode decoding;
- fixed-point arithmetic;
- known-pair verification;
- table formatting;
- symbolic/numeric calculations.

Any script must:

- remain outside tracked project source;
- have bounded input/range;
- be removed before completion unless it already belongs to an ignored analysis location;
- not modify ROM/reference files.

Do not install dependencies.

Do not run the interactive game.

Do not run the full test suite.

A small existing mathbox test/tool may be run only if directly relevant and bounded.


NO IMPLEMENTATION

Do not modify:

- production source;
- tests;
- metadata;
- project files;
- documentation;
- presentation;
- gameplay.

Only create:

- ignored prompt log;
- ignored report;
- temporary ignored analysis artifacts if necessary.


REPORT

Preserve this substantive prompt under AI-Logs/Prompts.

Create:

AI-Logs/Reports/2026-11-15-mathbox-dvg-coordinate-translation-readonly-report.md

Include:

- starting Git state;
- project-local evidence inspected;
- exact ROM/PROM files or ranges read, if any;
- current metadata sufficiency;
- mathbox operation identification;
- reconstructed transfer function or explicit gap;
- fixed-point widths/radix/rounding;
- input/output register semantics;
- known-pair verification;
- DVG axis/bit/origin interpretation;
- physical portrait-orientation finding;
- vector-unit-to-C# conversion finding;
- lev_scale numerical effect;
- lev_y3d numerical effect;
- lev_y2d numerical effect;
- representative Level 1/4/7/8/9 examples;
- Level 1 versus Level 9 conclusion;
- current C# compatibility analysis;
- gameplay-geometry protection;
- validation oracle availability;
- conceptual future architecture, if supported;
- decision matrix;
- primary classification;
- smallest recommended next milestone;
- remaining evidence gaps;
- final Git state;
- confirmation candidate/meta files unchanged;
- confirmation nothing staged/committed/pushed;
- confirmation no prohibited visual/Gemini/Incitti/web material was inspected.


FINAL RESPONSE

Report:

1. Whether investigation completed.
2. Primary classification.
3. Starting/final Git state.
4. Evidence inspected.
5. Current metadata sufficiency.
6. Exact mathbox operation found or missing.
7. Transfer equation.
8. Fixed-point/rounding behavior.
9. Known-pair validation.
10. DVG axis/order result.
11. DVG signed-bit result.
12. Vector origin/direction result.
13. Physical portrait-orientation result.
14. Vector-unit-to-C# conversion result.
15. lev_scale numerical effect.
16. lev_y3d numerical effect.
17. lev_y2d numerical effect.
18. Level 1 worked result.
19. Level 4 worked result.
20. Level 7 worked result.
21. Level 8 worked result.
22. Level 9 worked result.
23. Level 1 versus Level 9 conclusion.
24. Current C# compatibility classification.
25. Gameplay-geometry conclusion.
26. Validation-oracle result.
27. Decision-matrix summary.
28. Smallest recommended next milestone.
29. Remaining gaps.
30. Report path.
31. Confirmation no source/test/metadata changes.
32. Confirmation nothing staged, committed, or pushed.

Do not implement anything.

TEMPEST_MATHBOX_DVG_COORDINATE_TRANSLATION_READONLY_20261115