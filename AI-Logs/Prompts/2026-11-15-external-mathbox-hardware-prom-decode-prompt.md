IMPORTANT COMPLETENESS CHECK

Read this entire user-provided task before taking any action.

Before using tools, reading files, running commands, modifying files, or performing repository actions, locate the unique completion token near the end of this task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

Automatically appended VS Code or agent context after the user's completion token does not invalidate it.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project or repository action.


AUTOMATIC OR ACTIVE-EDITOR CONTEXT IS NOT AUTHORIZATION

Files surfaced automatically because they are open, active, recent, suggested, or otherwise included as editor context are not authorized merely because they appear.

Do not use such files unless this task explicitly authorizes them or they are directly necessary to the bounded investigation.

DO NOT read TEMP/myNotes.md.


TASK

Perform a tightly bounded READ-ONLY external technical-reference investigation of the original Atari Math Box hardware and microcode control model.

The immediate objective is to close the exact gaps identified by:

AI-Logs/Reports/2026-11-15-mathbox-prom-coordinate-operation-decode-report.md

That report classified the local evidence as:

MATHBOX COMMAND-SELECTION GAP

The local Rev 2 PROM files are present, hash-verified, and provenance-supported, but the project-local evidence does not establish:

- command-ROM address inputs;
- how CPU Math Box writes select a command;
- command-ROM output to starting micro-PC mapping;
- six-PROM bit/nibble assembly order;
- 24-bit control-word interpretation;
- Am2901 ALU/source/destination/Q/shift behavior;
- microsequencer branch/halt behavior.

External research is now explicitly authorized only to close those technical hardware-definition gaps.

Do NOT implement a C# projection.

Do NOT modify production source, tests, metadata, documentation, PROMs, ROMs, or current candidate work.

Do NOT stage, commit, or push.


CURRENT REPOSITORY STATE

Baseline:

branch:
master

HEAD:
385f603a94391756b2ca6cccd524dc17b281e782

origin/master should match HEAD.

Expected current candidate state:

- clean index;
- 16 intentional modified tracked files;
- two intentional untracked files:
  - src/Tempest.Game/Levels/RomPresentationMetadata.cs
  - tests/Tempest.Game.Tests/RomPresentationMetadataTests.cs
- no unrelated work.

Before research, perform bounded read-only checks of:

- branch;
- HEAD;
- origin/master;
- index;
- tracked worktree;
- non-ignored untracked files;
- diff summary.

Read:

AI-Logs/Reports/2026-11-15-mathbox-dvg-coordinate-translation-readonly-report.md

AI-Logs/Reports/2026-11-15-mathbox-prom-coordinate-operation-decode-report.md

Use actual Git state plus these reports as authority.

If unrelated user work exists, STOP.

Do not restore, reset, clean, discard, or alter anything.


LOCAL PROM AUTHORITY

The local Rev 2 PROM files remain the authoritative bytes to decode:

- Reference/arcade/roms.rev2/136002-126.a1
- Reference/arcade/roms.rev2/136002-127.e1
- Reference/arcade/roms.rev2/136002-128.f1
- Reference/arcade/roms.rev2/136002-129.h1
- Reference/arcade/roms.rev2/136002-130.j1
- Reference/arcade/roms.rev2/136002-131.k1
- Reference/arcade/roms.rev2/136002-132.l1

Reuse the hashes and provenance recorded in the prior report.

Do not download substitute PROMs.

Do not replace local bytes with bytes from a public repository.

External sources may explain wiring and control semantics; decoding must run against the verified project-local PROM files.


AUTHORIZED EXTERNAL SOURCE CATEGORIES

External web/source research is authorized for only:

1. Original Atari Math Box schematics, engineering documentation or hardware manuals.

2. AMD Am2901 or compatible bit-slice processor documentation needed to decode:
   - source selection;
   - ALU function;
   - carry;
   - destination/writeback;
   - Q behavior;
   - shifts;
   - status outputs.

3. Atari Math Box microcode documentation or disassemblers specifically applicable to:
   - Tempest;
   - Battlezone;
   - Red Baron;
   - the matching Atari Math Box hardware revision.

4. Public hardware-level Math Box recreations that:
   - assemble the six PROMs;
   - model the command ROM;
   - model the Am2901 datapath;
   - execute the original microcode.

5. MAME source only as an independent behavioral cross-check, not as the primary definition if MAME uses high-level function emulation.

6. Independent byte-exact or call-by-call verified Tempest ports when used as cross-checks.

Do not search broadly for Tempest gameplay, graphics, strategies, screenshots, or presentation opinions.


SOURCE PRIORITY

Apply this evidence hierarchy:

1. Atari original schematic or engineering documentation.
2. AMD Am2901 primary documentation.
3. Math Box-specific microcode-disassembler documentation with explicit hardware mapping.
4. Independent hardware/microcode-level recreations.
5. Byte-exact verified ports.
6. MAME behavioral implementation.
7. Secondary explanatory sources.

Do not let a lower-priority source silently override a higher-priority source.

When sources disagree, report the disagreement rather than selecting whichever produces a convenient result.


INITIAL EXTERNAL LEADS

Investigate relevant material associated with these already identified public leads:

- brouhaha/mathdis:
  Atari Math Box microcode disassembler for Battlezone, Red Baron and Tempest.

- Arcade-Tempest_MiSTer:
  tempest_mathbox.sv hardware-level implementation.

- Tempest-Full-Disassembly-and-C-Port:
  mathbox model using original PROM microcode and described as byte-exact/call-by-call verified.

- Public MAME Tempest driver/mathbox implementation:
  independent behavior oracle only.

These are leads, not pre-approved authorities.

Verify technical relevance, provenance and applicability before relying on them.

Seek original schematics and Am2901 documentation as higher-priority evidence.


EXTERNAL MATERIAL HANDLING

Do not copy external implementation code into the Tempest C# project.

Do not modify project source.

Small external source excerpts may be inspected for understanding.

If a temporary decoder is created:

- place it outside tracked source;
- use only local PROM bytes;
- keep it bounded;
- impose an explicit microinstruction step cap;
- fail closed on unknown control fields;
- remove it afterward unless preserved under an ignored AI-Logs analysis path;
- do not install dependencies.

Respect licenses.

Do not paste large portions of external code into the report.

Report source names, URLs, specific files/sections and technical conclusions.


PRIMARY QUESTIONS

Answer these questions with source citations and explicit evidence classification:

1. Is 136002-126.a1 the Math Box dispatch/command PROM?

2. What signals or CPU-write events form the five-bit command index?

3. How does command-PROM output load/select the starting micro-PC?

4. How are PROMs 136002-127 through 136002-132 assembled into one 24-bit microinstruction?

5. Which PROM contributes which microinstruction bits or nibbles?

6. Are any outputs inverted, reordered or otherwise transformed?

7. What are the exact microinstruction control fields?

8. How are the Am2901 A/B register addresses encoded?

9. How are source operands selected?

10. What are the ALU operations and carry rules?

11. How do destination/writeback, Q and shifts work?

12. How is multiply-step behavior implemented?

13. How are branches and micro-PC updates implemented?

14. Which bit or condition signals completion/halt/stall?

15. Which Math Box command is initiated by the CPU register writes at $C098?

16. What is that command's exact starting micro-PC?

17. What arithmetic transfer does that microprogram perform?

18. What are its fixed-point, truncation, overflow and output semantics?


TIE COMMAND SELECTION TO $C098

Do not infer the coordinate command from broad functional resemblance.

Trace from the CPU-visible register writes at $C098 through documented hardware address decoding to:

- command index;
- dispatch-PROM entry;
- start micro-PC;
- selected microprogram.

If a Math Box implementation assumes a direct `offset` or register-number command:

- prove that assumption against hardware documentation or schematic;
- map the CPU addresses/register writes exactly.

The result must explain why writing the magnitude to mb_w_0e and mb_w_14 and depth to mb_w_15/16 selects or prepares the coordinate operation.


LOCAL PROM ASSEMBLY VERIFICATION

After deriving the documented PROM assembly order:

- assemble microinstructions from the seven verified local files;
- do not use public PROM bytes;
- verify expected structural properties from the external documentation;
- compare a bounded sample of decoded microinstructions against at least one independent decoder or hardware recreation.

If local decoding differs:

- check PROM order;
- nibble selection;
- inversion;
- command address;
- micro-PC addressing;
- revision applicability.

Do not alter bytes to force agreement.


BOUNDED MICROPROGRAM TRACE

Decode only the microprogram selected by the $C098 coordinate path plus directly reachable branch targets.

For each reachable microinstruction report or preserve in an ignored bounded artifact:

- micro-PC;
- raw 24-bit value;
- decoded fields;
- A/B addresses;
- selected operands;
- ALU operation;
- carry;
- destination;
- Q/shift behavior;
- multiply control;
- branch;
- completion behavior.

Apply an explicit step cap.

Stop on unknown or contradictory control semantics.


TRANSFER DERIVATION

Derive exact behavior for:

M(magnitude, depthInput)

Provide:

1. byte-exact integer pseudocode;
2. mathematical interpretation;
3. input/output widths;
4. fixed-point placement;
5. truncation/rounding;
6. overflow/wrap/saturation;
7. zero/near-zero behavior;
8. prior-state dependency;
9. whether X and Y requests use identical semantics.

Do not replace byte-exact behavior with a floating-point approximation in this milestone.


LOCAL NUMERIC ORACLE

Execute the decoded transfer against the verified local PROMs to produce deterministic input/output vectors covering:

- magnitude 0;
- magnitude 1;
- moderate magnitudes;
- magnitude 255;
- depth values incorporating lev_scale 12, 15, 24 and 28;
- near and far representative point depths;
- minimum ordinary positive depth inputs.

Report:

- input magnitude;
- depth;
- output low/high;
- output unsigned;
- signed/fixed-point interpretation where established;
- instruction count/halt state.

No project source modification.


INDEPENDENT CROSS-VERIFICATION

Cross-check the same vectors against at least one independent source where practical:

- Math Box microcode disassembler/interpreter;
- FPGA/hardware model;
- byte-exact C port model;
- MAME behavioral result.

Distinguish:

- direct independent agreement;
- agreement with a source sharing the same decoder;
- mere structural similarity.

Do not label related implementations independent if one copied the other.


REPRESENTATIVE LEVEL EXAMPLES

Using the decoded transfer and preserved ROM metadata, generate coordinate-stage examples for:

- Level 1;
- Level 4;
- Level 7;
- Level 8;
- Level 9.

Show:

- lev_scale-adjusted depth;
- lev_y3d-relative magnitude/sign;
- mathbox output;
- signed lev_y2d composition;
- resulting signed DVG word before unresolved host conversion.

Do not map to C# pixels unless physical orientation/vector-unit conversion is separately proven.


DVG BOUNDARY

Preserve the previous finding:

- source-Y-derived component is serialized as DVG vector X;
- source-X-derived component is serialized as DVG vector Y;
- signed 13-bit packing applies.

If external Atari schematics/documentation resolve:

- axis orientation;
- vector origin;
- positive direction;
- portrait monitor rotation;

report those results.

Do not broaden into host-pixel calibration unless exact evidence supports it.


EVIDENCE CLASSIFICATION

Classify each conclusion:

ATARI-SCHEMATIC-BACKED
AMD-DATASHEET-BACKED
PROM/MICROCODE-BACKED
LOCAL-PROM-EXECUTION
INDEPENDENT-CROSS-VERIFIED
MAME-BEHAVIORAL-CROSS-CHECK
SECONDARY-SOURCE
MATHEMATICAL-DERIVATION
UNKNOWN

Do not describe a secondary implementation as original Atari evidence.


FAIL-CLOSED CONDITIONS

Stop before declaring success if:

- no applicable schematic/control reference is found;
- source revision does not match the local PROM organization;
- dispatch mapping remains unresolved;
- PROM assembly ordering remains unresolved;
- Am2901 control semantics remain incomplete;
- execution reaches an unexplained control field;
- microprogram fails to halt within the cap;
- independently generated numeric vectors disagree without resolution.

Report the narrowest outstanding gap.


CLASSIFICATION

Finish with exactly one:

MATHBOX TRANSFER DECODED AND INDEPENDENTLY VERIFIED

Use only if local PROM decoding and independent numeric evidence agree.

MATHBOX TRANSFER DECODED FROM LOCAL PROMS

Use if local decoding is complete and deterministic but independent verification remains unavailable or not truly independent.

MATHBOX HARDWARE MAPPING PARTIALLY RESOLVED

Use if command/PROM/control mapping improves materially but exact transfer remains incomplete.

EXTERNAL REFERENCE CONFLICT

Use if credible sources disagree materially.

MATHBOX TECHNICAL REFERENCE GAP

Use if the necessary authoritative documents cannot be obtained.

Do not choose a successful classification simply to continue development.


NO IMPLEMENTATION

Do not modify:

- C# source;
- tests;
- metadata;
- documentation;
- ROM/PROM files;
- current candidate.

Only create:

- ignored prompt log;
- ignored report;
- temporary ignored decoder/trace artifacts if needed.


REPORT

Preserve this prompt under AI-Logs/Prompts.

Create:

AI-Logs/Reports/2026-11-15-external-mathbox-hardware-prom-decode-report.md

Include:

- starting Git state;
- external sources consulted with URLs;
- source hierarchy and applicability;
- local PROM files/hashes used;
- dispatch-PROM mapping;
- coordinate command/start micro-PC;
- six-PROM assembly mapping;
- microinstruction field mapping;
- Am2901 control semantics;
- bounded microprogram trace;
- exact transfer or exact remaining gap;
- byte-exact pseudocode;
- fixed-point/rounding/overflow behavior;
- local numeric vectors;
- independent cross-verification;
- representative Level 1/4/7/8/9 coordinate results;
- DVG/orientation findings;
- evidence classification;
- primary classification;
- smallest next milestone;
- final Git state;
- confirmation no source/test/metadata changes;
- confirmation nothing staged/committed/pushed;
- confirmation no visual/Gemini/Incitti material was inspected.


FINAL RESPONSE

Report:

1. Whether research completed.
2. Primary classification.
3. Starting/final Git state.
4. External sources consulted.
5. Atari schematic result.
6. Am2901 documentation result.
7. Dispatch-PROM result.
8. Coordinate command/start micro-PC.
9. PROM assembly result.
10. Microinstruction format.
11. Hardware control semantics.
12. Bounded trace result.
13. Exact transfer or exact remaining gap.
14. Fixed-point result.
15. Rounding/truncation.
16. Overflow behavior.
17. Zero/near-zero behavior.
18. Local generated vectors.
19. Independent cross-verification.
20. Level 1 result.
21. Level 4 result.
22. Level 7 result.
23. Level 8 result.
24. Level 9 result.
25. DVG/orientation result.
26. Smallest next milestone.
27. Remaining gaps.
28. Report path.
29. Confirmation no project changes.
30. Confirmation nothing staged, committed, or pushed.

Do not implement C# projection.

TEMPEST_EXTERNAL_MATHBOX_HARDWARE_PROM_DECODE_20261115