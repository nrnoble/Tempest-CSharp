READ THIS ENTIRE USER-PROVIDED TASK BEFORE TAKING ANY ACTION.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, reading project files, running commands, or taking any
repository action:

1. Locate the unique completion token near the end of this user-provided task.
2. Verify that exactly one complete token beginning with "TEMPEST_" appears on
   a line by itself in the user-provided task.
3. VS Code/agent-generated reminders, tagged-file information, attachment
   metadata, or other system-generated context may appear after the token and
   do not invalidate it.
4. This check is intended to detect an incomplete or truncated user task, not
   appended agent context.
5. If exactly one complete TEMPEST_ token cannot be found, STOP before project
   or repository action and explain why.

TASK

Perform a narrowly bounded READ-ONLY investigation to determine the value and
provenance of the Math Box RN high byte when the first normal Atari Level 7
record-06 DIVZ coordinate operation executes.

This investigation must resolve, or precisely bound, the one remaining Math
Box startup-state uncertainty identified by the preceding Level 7 normal
call-state investigation.

Do not implement any correction.

Do not change geometry, projection, viewport, presentation, or gameplay.

READ-ONLY SCOPE AND REQUIRED AI-LOGS

Follow the current .github/copilot-instructions.md.

This task is read-only with respect to project source, tests, project
configuration, durable documentation, protected user/reference material, and
Git state.

The required ignored AI-Logs records are the sole permitted file-writing
exception:

- Save this substantive prompt under AI-Logs/Prompts/.
- Save the completed investigation report under AI-Logs/Reports/.
- Use descriptive date-prefixed paired filenames.
- The report must be written to the report file even if the same findings are
  also returned in chat.
- A report returned only in Copilot chat does not satisfy this task.
- Do not stage or commit AI-Logs.

CONTEXT ESTABLISHED BY THE PREVIOUS INVESTIGATION

The preceding report is:

AI-Logs/Reports/2026-11-16-level7-mathbox-normal-callstate-readonly-investigation-report.md

That investigation established the following relevant facts:

- Normal Level 7 selects record 06.
- The normal coordinate path invokes the Math Box DIVZ / Z/X' operation for
  each axis and point.
- The operation is selected by the write to Math Box offset $14 / SZXD.
- $0F is the low byte of the quotient-bit count RN; it is not the Math Box
  operation mode.
- Normal initialization writes $0F to offset $0C / NL, establishing RN.low.
- The reviewed normal CPU path does not write or otherwise establish RN.high.
- The DIVZ microcode copies RN into a work counter and decrements the work
  counter rather than modifying RN itself.
- For RN=$000F, the existing local PROM-backed trace produces:

  floor(128 * magnitude / depth)

- The current C# prototype produces:

  floor(256 * magnitude / depth)

- The factor-of-two discrepancy is therefore a strong conditional explanation
  for the visible Level 7 geometry problem, but it is not yet established as
  normal Atari behavior because RN.high remains unresolved.
- No geometry, point-order, near/far correspondence, rail, rim-topology,
  viewport, or gameplay correction is authorized by this task.

Do not repeat the completed general call-path analysis unless needed to trace
RN.high provenance.

PRIMARY QUESTION

What value does RN.high have when the first normal record-06 DIVZ coordinate
operation executes during an ordinary Atari Level 7 tube construction?

Determine whether that value is established by:

- Math Box hardware reset;
- register-file reset or initialization;
- CPU initialization;
- Math Box microcode;
- a write through another mapped register or alias;
- a prior startup operation;
- a prior gameplay/presentation operation;
- documented retained state;
- an emulator/device model;
- or another evidenced mechanism.

The investigation must distinguish:

1. Math Box power-on state.
2. Math Box hardware-reset state.
3. CPU-written initialization state.
4. State after any pre-Level-7 Math Box operations.
5. State at the first normal Level 7 record-06 DIVZ call.
6. State retained for subsequent axes, points, and near/far passes.

Do not assume these states are identical.

STRICT INVESTIGATION BOUNDARY

Focus on RN.high and only the minimum surrounding state needed to establish its
value.

Do not reopen or re-investigate these areas unless new RN.high evidence directly
contradicts the existing conclusion:

- record-06 table extraction;
- record-06 point ordering;
- near p=$10 and far p=$F0 selection;
- near/far index correspondence;
- rail pairing;
- rim closure;
- Y3D;
- Y2D/base semantics;
- axis ordering;
- coordinate signs;
- viewport mapping;
- Level 7 gameplay geometry.

Do not inspect screenshots unless a newly established RN.high result requires
comparison with the already documented visible discrepancy. This task should
normally be resolved from startup/reset/register-state evidence rather than
visual analysis.

LOCAL EVIDENCE FIRST

Use the strongest available project-local primary or previously verified
evidence, including as relevant:

- Reference/arcade/Tempest Rom Source code.md
- Reference/arcade/MBUDOC.DOC
- Reference/arcade/MBUCOD.V05.txt
- existing local Math Box PROM and coordinate-operation investigation reports;
- existing local reports concerning $C098, DIVZ, mode $0F, register state,
  initialization, and reset;
- current emulator or hardware-model source already present in the repository,
  if any;
- bounded local ROM/PROM verification only when genuinely necessary and allowed
  by the current project instructions.

Do not search the Internet for substitute Tempest disassemblies or projection
implementations.

Do not use Mark Incitti's implementation as proof of Atari Math Box reset
state.

Do not treat comments, symbol names, prior report conclusions, or emulator
defaults as primary evidence unless their provenance establishes the relevant
hardware behavior.

STARTUP AND RESET TRACE

Trace the relevant startup/reset sequence far enough to answer:

1. What reset signal or initialization path affects the Math Box?
2. Does reset initialize the Math Box register file?
3. Does reset initialize RN as a full word, only RN.low, or neither?
4. Is RN.high addressable by the CPU?
5. Can RN.high be written indirectly through a mapped register, alias,
   sequencer path, or microcode operation?
6. Does the documented write to NL / offset $0C affect only RN.low or the full
   RN word?
7. Does any startup Math Box operation clear, normalize, copy, overwrite, or
   otherwise establish RN.high?
8. Does any operation before the first normal Level 7 coordinate DIVZ retain or
   alter RN.high?
9. Is the Level 7 direct-start diagnostic path materially different from a
   natural campaign startup with respect to the Atari state being modeled?
10. If multiple real Atari startup paths exist, do they converge on the same
    RN.high value before tube projection?

Trace writes and micro-operations rather than relying only on named routines.

REGISTER AND MICROCODE SEMANTICS

Determine the actual width and semantics of RN.

Where the evidence permits, establish:

- RN bit width;
- RN.low and RN.high storage locations;
- whether RN.high physically exists as retained state;
- whether RN.high participates in DIVID loop count or termination;
- whether the work-counter copy consumes the full RN word;
- behavior if RN.high is nonzero;
- whether only a bounded subset of RN participates despite a wider named
  register;
- whether writing NL implicitly clears, preserves, replaces, or leaves
  undefined the high byte;
- whether any microcode path such as CLRNH is relevant to the normal startup or
  coordinate path.

Do not infer behavior from register names alone.

Resolve any apparent conflict between interface documentation, microcode,
existing PROM traces, and prior reports by identifying which source directly
establishes each claim.

EMULATOR OR HARDWARE-MODEL EVIDENCE

If a project-local trusted emulator/device implementation exists and explicitly
models the relevant Math Box register reset and write semantics, inspect it as
secondary corroborating evidence.

Clearly record:

- the implementation examined;
- whether the model initializes RN.high;
- whether an NL write changes RN.high;
- whether the model's behavior is documented or merely an implementation
  choice;
- and whether that evidence can establish Atari behavior or only support an
  inference.

Do not search the Internet for emulator source unless the local evidence
explicitly identifies an authoritative missing source and the current project
rules permit that bounded lookup. This task should use project-local evidence
first.

BOUNDED DYNAMIC EVIDENCE

If static local evidence cannot establish RN.high, determine whether a bounded,
noninteractive, read-only capture from an existing trusted emulator,
instrumentation path, debugger trace, or device model can establish the actual
state without changing project source or configuration.

Do not create production instrumentation.

Do not launch the interactive C# game.

Do not require Neal to play, answer questions, provide screenshots, or perform
manual steps.

Do not claim that a synthetic calculation or C# default records Atari hardware
state.

If no existing bounded capture path is available, report exactly what the
required capture would need to observe, at what operation or address, and why
that observation would resolve the uncertainty. Do not implement that capture
during this task.

NATURAL START VERSUS DIRECT LEVEL 7

The current C# prototype launches Level 7 directly, but the question concerns
normal Atari state.

Determine whether RN.high could differ between:

- Atari power-on/reset followed by natural gameplay progression to Level 7;
- Atari power-on/reset followed by any built-in level selection or service
  path;
- and the conceptual direct Level 7 construction modeled by the prototype.

Do not assume natural progression is necessary if the Atari initialization and
subsequent operations establish invariant RN state.

Do not assume direct-level startup is equivalent if evidence shows retained
state depends on prior Math Box operations.

If the paths cannot be proven equivalent, state the distinction clearly.

DECISION STANDARD

Classify the RN.high result as exactly one of:

- CONFIRMED ZERO
- CONFIRMED NONZERO, with the established value
- PATH-DEPENDENT, with each established path/value
- UNDEFINED BY HARDWARE/RESET BUT ESTABLISHED BEFORE LEVEL 7
- UNDEFINED OR UNRESOLVED AT FIRST LEVEL 7 DIVZ

Use CONFIRMED only when primary or sufficiently direct evidence establishes the
value.

A report or implementation that merely initializes a C#, emulator, or test
field to zero is not proof of Atari hardware state.

If RN.high remains unresolved, identify the single narrowest missing
observation or source needed to resolve it.

CONDITIONAL NUMERIC CONSEQUENCE

Do not repeat the full 16-point coordinate tables unless the newly established
RN.high result changes or confirms their interpretation.

If RN.high is confirmed zero, state that the preceding report's
RN=$000F factor-128 table is now supported for normal Level 7 and explain why.

If RN.high is confirmed nonzero or path-dependent, determine how the established
value changes the quotient-cycle behavior and whether the previous factor-128
or current factor-256 result applies.

If the effect of the established RN.high value requires further PROM execution,
perform only the bounded calculation necessary to classify the consequence.

Do not implement the result.

NO IMPLEMENTATION OR PROJECT CHANGES

Do not:

- edit source code;
- edit tests;
- edit project files;
- edit configuration;
- edit durable project documentation;
- alter Atari tube data;
- alter Math Box projection code;
- alter coordinate formulas;
- alter geometry;
- alter viewport mapping;
- alter presentation;
- alter gameplay;
- add production diagnostics;
- add test seams;
- launch the interactive game;
- stage, commit, push, reset, restore, stash, rebase, clean, or switch branches.

Temporary calculations outside tracked project content are permitted when
bounded and necessary.

Long searches, traces, simulations, or command loops must have explicit
bounds. Stop and report rather than entering an unbounded evidence search.

REPOSITORY PRESERVATION

At the beginning, record:

- branch;
- HEAD;
- upstream/tracking state;
- index state;
- tracked worktree modifications;
- relevant untracked state.

At completion, verify that all project and Git state remains unchanged except
for the required ignored paired AI-Logs prompt/report files.

Do not discard or overwrite pre-existing candidate work.

EXPECTED REPORT

Save the required report under AI-Logs/Reports/ and summarize it in chat.

The report must include:

1. The exact evidence sources inspected.
2. The relevant reset and startup path.
3. The established width and storage semantics of RN.
4. CPU accessibility of RN.low and RN.high.
5. The exact effect of the NL / $600C write.
6. Any reset behavior that affects RN.high.
7. Every pre-coordinate operation that can materially affect RN.high.
8. The RN.high state at the first normal Level 7 record-06 DIVZ call.
9. Whether later axes, points, and near/far passes preserve that state.
10. Whether natural progression and direct Level 7 construction are equivalent
    for this state.
11. One of the required RN.high classifications:
    - CONFIRMED ZERO
    - CONFIRMED NONZERO
    - PATH-DEPENDENT
    - UNDEFINED BY HARDWARE/RESET BUT ESTABLISHED BEFORE LEVEL 7
    - UNDEFINED OR UNRESOLVED AT FIRST LEVEL 7 DIVZ
12. The evidence classification and confidence boundary for each important
    conclusion.
13. The consequence for the factor-128 versus factor-256 projection question.
14. Whether the observed Level 7 geometry discrepancy now has an established
    cause.
15. Whether enough evidence exists for a bounded correction implementation
    prompt.
16. If not, the single narrowest additional evidence-gathering step required.
17. Confirmation that no geometry, projection, viewport, presentation, or
    gameplay code was changed.
18. Confirmation that no project or Git state changed except the required
    ignored AI-Logs records.
19. The exact paths of the saved prompt and report files.

Do not ask Neal questions during execution.

Do not pause for manual input or acceptance.

Complete the investigation autonomously and stop after reporting the evidence.

TEMPEST_LEVEL7_RN_HIGH_RESET_STARTUP_STATE_READONLY_INVESTIGATION