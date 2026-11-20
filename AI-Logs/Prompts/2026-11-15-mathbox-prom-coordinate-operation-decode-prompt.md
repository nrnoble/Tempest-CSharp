IMPORTANT COMPLETENESS CHECK

Read this entire user-provided task before taking any action.

Before using tools, reading project files, running commands, modifying files, or performing repository actions, locate the unique completion token near the end of this task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

Automatically appended VS Code/agent context after the user's token does not invalidate it.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project or repository action.


AUTOMATIC / ACTIVE-EDITOR CONTEXT IS NOT AUTHORIZATION

Automatically surfaced active/recent/editor files are not authorized merely because VS Code supplied them.

Do not use such files unless expressly authorized below or directly necessary to this bounded investigation.

DO NOT read TEMP/myNotes.md.


TASK

Perform one tightly bounded READ-ONLY investigation of the Atari Tempest Rev 2 mathbox command ROM and six mathbox microcode PROMs.

The previous investigation classified the remaining presentation blocker as:

MATHBOX MICROCODE DECODING REQUIRED

This milestone must determine the exact mathbox transfer used by the CPU coordinate routine at $C098, represented previously as:

M(magnitude, depthInput)

where:

depthInput = pointDepth + lev_scale

and magnitude is either:

abs(pointY - lev_y3d)

or:

abs(pointX - 0x80)

Do NOT implement any C# projection or presentation change.

Do NOT modify production source, tests, metadata, documentation, ROM files, or current candidate work.

Do NOT stage, commit, or push.


CURRENT REPOSITORY STATE

Baseline:

branch:
master

HEAD:
385f603a94391756b2ca6cccd524dc17b281e782

origin/master should match HEAD.

Expected current worktree:

- clean index;
- 16 intentional modified tracked candidate files;
- two authorized untracked files:
  - src/Tempest.Game/Levels/RomPresentationMetadata.cs
  - tests/Tempest.Game.Tests/RomPresentationMetadataTests.cs
- no unrelated changes.

Before analysis inspect:

- branch;
- HEAD;
- origin/master;
- staged changes;
- tracked worktree;
- non-ignored untracked files;
- diff summary.

Read:

AI-Logs/Reports/2026-11-15-mathbox-dvg-coordinate-translation-readonly-report.md

Use actual Git state and that report as authority.

If unexpected user work exists, STOP.

Do not restore, reset, clean, discard, or alter anything.


PRIMARY AUTHORITY AND SCOPE

The previous report established:

- CPU operand preparation at $C098;
- CPU-facing mathbox register writes and reads;
- source-side sign restoration;
- signed lev_y2d placement;
- DVG word packing;
- preserved ROM metadata;
- missing mathbox transfer function;
- missing numeric input/output oracle.

This task is authorized to close only the mathbox-transfer gap.

DVG-to-modern-host calibration remains a later milestone unless numeric PROM execution directly establishes additional facts.


AUTHORIZED PROJECT-LOCAL PROM FILES

Within the existing project-local Rev 2 ROM set, locate and inspect only:

- 136002-126.a1
- 136002-127.e1
- 136002-128.f1
- 136002-129.h1
- 136002-130.j1
- 136002-131.k1
- 136002-132.l1

Expected conceptual roles to verify from local evidence:

- 136002-126.a1: command/address ROM;
- 136002-127 through 136002-132: six mathbox microcode PROMs.

Do not assume filenames alone establish the role.

Verify sizes, hashes, local provenance, and byte organization using project-local evidence.

Do not inspect unrelated ROM contents.

Do not download or compare external ROMs.

Do not search the web.


PROM PROVENANCE

For each authorized PROM report:

- filename;
- size;
- SHA-256;
- existing local provenance or inventory evidence;
- whether it belongs to the intended Rev 2 set;
- logical nibble/bit contribution if derivable.

If provenance cannot be established adequately, STOP with:

PROM PROVENANCE BLOCKED


COMMAND ROM INVESTIGATION

Determine the exact function of the 32-byte command ROM.

Establish:

- indexing/address inputs;
- output width;
- whether the CPU register-write pattern selects an entry;
- how an entry supplies the starting microprogram address;
- which command is selected by the $C098 register-write sequence;
- exact starting micro-PC for the coordinate operation.

Do not infer the coordinate microprogram merely by choosing one that looks like division.

Trace selection from the CPU-visible writes.


SIX-PROM MICROINSTRUCTION ASSEMBLY

Determine how the six 4-bit PROM outputs combine into one microinstruction.

Establish:

- PROM-to-bit ordering;
- microinstruction width;
- address/micro-PC width;
- any inversion or bit permutation;
- whether files are packed nibbles or full bytes;
- how unused bits are handled.

Produce a bounded decoded table only for:

- the coordinate operation’s start entry;
- every microinstruction reachable from that start until completion/stall;
- any directly called/reused branch target required to understand it.

Do not dump or analyze unrelated microprograms.


HARDWARE EXECUTION MODEL

Using project-local documentation/source where available, reconstruct only the hardware elements required by the selected microprogram:

- four Am2901-equivalent bit slices / 16-bit datapath;
- A/B register-file addresses;
- Q register;
- ALU source selection;
- ALU function;
- carry input;
- destination/writeback behavior;
- shifts;
- multiply-step behavior;
- branch/jump behavior;
- stall/completion behavior;
- Y/output bus behavior;
- relevant status/sign/overflow behavior.

If local evidence does not define a control bit:

- label it unknown;
- do not guess from signal names alone.

If interpretation requires external chip documentation unavailable locally, STOP and identify the exact missing control definition rather than searching the web.


TRACE THE $C098 COORDINATE OPERATION

Start from the exact CPU-visible state created by $C098:

- magnitude byte written to mb_w_0e and mb_w_14;
- depth low/high written to mb_w_15 and mb_w_16;
- all other relevant mathbox state initialized as established by hardware/register semantics.

Trace every selected microinstruction until completion.

For each step report compactly:

- micro-PC;
- raw 24-bit microinstruction;
- decoded control fields;
- source operands;
- ALU operation;
- Q behavior;
- destination;
- branch decision;
- resulting relevant register state.

A full state table may go into a bounded ignored analysis artifact if too large for the report, with the report summarizing the arithmetic derivation.

Do not trace unrelated commands.


DERIVE THE TRANSFER

From the trace, derive the exact numerical transfer:

M(magnitude, depthInput)

Determine:

- operand widths;
- unsigned/signed interpretation;
- numerator and denominator roles;
- normalization constant;
- fixed-point radix;
- multiply/divide/reciprocal sequence;
- rounding or truncation;
- output bound;
- overflow behavior;
- zero/near-zero depth behavior;
- whether output depends on prior mathbox state;
- whether X and Y requests truly use the same transfer.

Express the result as:

1. exact integer/pseudocode behavior;
2. mathematical interpretation;
3. any remaining hardware-dependent conditions.

Do not simplify away byte-exact behavior needed for validation.


BOUNDED EXECUTABLE DECODER

If feasible from local evidence, create a temporary read-only decoder/emulator outside tracked project source.

Requirements:

- bounded to the seven authorized PROMs;
- no network;
- no dependency installation;
- no ROM mutation;
- no production integration;
- explicit instruction-step cap;
- fail closed on unknown control encoding;
- deterministic output;
- removed before completion unless preserved only under ignored AI-Logs analysis artifacts.

Use the decoder to produce numeric input/output pairs.

Do not claim correctness merely because output looks plausible.


KNOWN INPUT/OUTPUT TEST VECTORS

Generate a compact oracle table covering representative inputs such as:

- magnitude 0;
- magnitude 1;
- moderate coordinate magnitudes;
- magnitude near the byte maximum;
- depth inputs corresponding to Level 9 lev_scale 12;
- Level 4 lev_scale 15;
- common lev_scale 24;
- Level 2 lev_scale 28;
- several representative point depths;
- minimum production-positive depth;
- larger/farther depths.

For each report:

- magnitude;
- depth input;
- raw output low/high;
- unsigned output;
- relevant fixed-point interpretation.

Include enough vectors to distinguish the reconstructed operation from a guessed ordinary integer division.


INDEPENDENT CROSS-CHECKS

Seek project-local independent checks such as:

- built-in hardware self-test expected values;
- ROM mathbox diagnostic routines;
- source comments with expected outputs;
- CPU-side validation code;
- existing reference-analysis reports.

Do not inspect screenshots or video.

If a built-in self-test exists, determine whether the decoder reproduces its expected values.

Clearly distinguish:

- same-model self-consistency;
- independent cross-verification.

Do not call generated vectors independently verified unless a separate source supports them.


COORDINATE-ROUTINE ORACLE

Use the derived transfer to produce bounded coordinate examples for representative metadata:

- Level 1;
- Level 4;
- Level 7;
- Level 8;
- Level 9.

Use ROM-unit symbolic or numeric points only where source inputs are established.

At minimum demonstrate how the different lev_scale and lev_y3d values alter mathbox outputs for identical source point/depth inputs.

Then apply signed lev_y2d using the already established CPU-side behavior.

Stop at DVG signed-vector coordinates unless orientation/unit conversion is fully proven.

Do not map to C# pixels.


DVG BOUNDARY

The prior report established:

- source-Y-derived result becomes serialized vector X;
- source-X-derived result becomes serialized vector Y;
- signed 13-bit packing.

Preserve that finding.

Do not broaden this task into viewport/monitor/C# calibration unless the PROM trace itself resolves a previously unknown point.

The next milestone may address DVG orientation and vector-unit conversion after numeric mathbox vectors exist.


CURRENT METADATA AND GAMEPLAY PROTECTION

Do not modify:

- RomPresentationMetadata;
- catalog values;
- LevelLoader;
- projection;
- renderer;
- geometry;
- gameplay;
- transition behavior;
- pause/screenshot;
- tests.

The current metadata is sufficient for the target raw fields.

No gameplay geometry change is authorized.


NO VISUAL OR EXTERNAL SOURCES

Do not inspect:

- images;
- screenshots;
- videos;
- MAME frames;
- recordings;
- Gemini material;
- Incitti source;
- web sources.

Do not use public MAME source in this milestone.

The purpose is to determine what the project-local PROM evidence independently supports.


FAIL-CLOSED CONDITIONS

STOP before arithmetic conclusions if:

- PROM provenance is uncertain;
- command-ROM mapping is unresolved;
- PROM nibble/bit ordering is unresolved;
- required Am2901 control definitions are unavailable locally;
- microprogram start address cannot be tied to $C098 writes;
- execution reaches unknown control behavior;
- step cap is reached;
- generated outputs cannot be distinguished from speculative decoding.

Report the narrowest exact missing evidence.


CLASSIFICATION

Finish with exactly one:

MATHBOX TRANSFER DECODED AND CROSS-VERIFIED

Use only if command selection, PROM decode, execution, arithmetic transfer, and independent numeric evidence all agree.

MATHBOX TRANSFER DECODED, ORACLE GENERATED

Use if the PROM execution and transfer are confidently reconstructed and deterministic vectors are generated, but independent verification remains limited.

MATHBOX CONTROL-DEFINITION GAP

Use if PROM bytes are available but required hardware control semantics are unavailable locally.

MATHBOX COMMAND-SELECTION GAP

Use if the $C098 register writes cannot be tied to a microprogram start.

MATHBOX PROM-ASSEMBLY GAP

Use if nibble/bit ordering cannot be established.

PROM PROVENANCE BLOCKED

Use if files cannot be identified confidently.

PARTIAL MICROCODE DECODE

Use if some arithmetic is established but exact byte-level behavior remains unresolved.

Do not select a successful classification merely to proceed.


NEXT-MILESTONE DECISION

If transfer decoding succeeds:

- recommend the smallest next read-only DVG/vector-unit orientation and numeric conversion milestone;
- do not implement projection yet unless the classification includes an adequate objective oracle and all required translation stages are already proven.

If decoding fails:

- identify exactly what additional local evidence or specifically named external technical source would be required;
- do not suggest broad internet searching.


REPORT

Preserve this prompt under AI-Logs/Prompts.

Create:

AI-Logs/Reports/2026-11-15-mathbox-prom-coordinate-operation-decode-report.md

Include:

- starting Git state;
- authorized PROM files read;
- provenance;
- hashes and sizes;
- command-ROM role;
- selected command/start address;
- PROM assembly order;
- microinstruction format;
- relevant hardware model;
- bounded microprogram trace;
- exact transfer or exact gap;
- integer/pseudocode representation;
- fixed-point/rounding/overflow behavior;
- generated input/output vectors;
- independent verification result;
- representative Level 1/4/7/8/9 coordinate results;
- DVG boundary result;
- classification;
- smallest next milestone;
- remaining uncertainties;
- final Git state;
- confirmation no project source/test/metadata changes;
- confirmation nothing staged/committed/pushed;
- confirmation no prohibited visual/external material was inspected.


FINAL RESPONSE

Report:

1. Whether investigation completed.
2. Primary classification.
3. Starting/final Git state.
4. PROM files read.
5. Provenance result.
6. Command-ROM result.
7. Coordinate command/start micro-PC.
8. PROM assembly order.
9. Microinstruction format.
10. Hardware control-model sufficiency.
11. Microprogram trace result.
12. Exact transfer or exact gap.
13. Fixed-point behavior.
14. Rounding/truncation.
15. Overflow/saturation behavior.
16. Zero/near-zero behavior.
17. Generated oracle vectors.
18. Independent cross-verification.
19. Level 1 example.
20. Level 4 example.
21. Level 7 example.
22. Level 8 example.
23. Level 9 example.
24. DVG boundary conclusion.
25. Smallest next milestone.
26. Remaining evidence gaps.
27. Report path.
28. Confirmation no source/test/metadata changes.
29. Confirmation nothing staged, committed, or pushed.

Do not implement C# projection.

TEMPEST_MATHBOX_PROM_COORDINATE_OPERATION_DECODE_20261115