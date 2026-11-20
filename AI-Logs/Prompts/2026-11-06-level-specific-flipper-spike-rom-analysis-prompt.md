FAIL-CLOSED COMPLETENESS CHECK

Read this complete user-provided task before using any tools, reading files,
running commands, modifying files, staging, committing, pushing, or taking any
other repository action.

Before doing anything else, locate the unique completion token near the end of
this user-provided task.

Verify that exactly one complete standalone token beginning with "TEMPEST_"
appears on a line by itself in the user-provided task.

VS Code/agent-generated reminders, attachment details, tagged-file context, or
other system-generated information may appear after the completion token and do
not invalidate it.

This check detects an incomplete or truncated user task, not appended agent
context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before
any project or repository action.


TEMPEST LEVEL-SPECIFIC FLIPPER / SPIKE INTERACTION ROM ANALYSIS

Perform a tightly bounded ANALYSIS-ONLY investigation of how the original Atari
Tempest arcade ROM controls below-rim Flipper movement and Flipper interaction
with existing Spikes on individual levels/tubes.

Do not implement anything.

Do not change production code.

Do not change tests.

Do not modify the accepted Levels 1–7.

Do not stage.

Do not commit.

Do not push.

Do not launch the C# game interactively.

This investigation must determine, as far as the available ROM evidence permits,
whether Flipper movement across Spike lanes and Flipper capture/locking onto a
Spike are controlled by explicit level/tube-specific programs rather than by
one universal or random rule.


ACTIVE REPOSITORY

Work only in:

C:\Users\nrnob\source\repos\Tempest

Do not use or modify the frozen former OneDrive copy.

The repository is now backed up in a private GitHub repository, but GitHub
operations are not required for this analysis.


REQUIRED STARTING CHECKPOINT

Expected branch:

master

Expected HEAD:

aa4eb762b9e207b2014daec4533fbd5a513cffcd
Implement accepted Game Over presentation

Expected repository state:

- tracked worktree clean;
- index clean;
- no unexplained untracked files;
- local master tracking origin/master.

Before examining project behavior, run:

git status --short --branch
git rev-parse HEAD
git log -1 --oneline
git diff --stat
git diff --cached --stat
git ls-files --others --exclude-standard

If the branch, HEAD, tracked worktree, index, or untracked state differs
unexpectedly, STOP without modifying anything and report the difference.


AUTHORIZED WRITES

This task is analysis-only.

The only authorized writes are the ignored paired AI records:

AI-Logs/Prompts/2026-11-06-level-specific-flipper-spike-rom-analysis-prompt.md

AI-Logs/Reports/2026-11-06-level-specific-flipper-spike-rom-analysis-report.md

Do not modify:

- production source;
- test source;
- TEMPEST-DEVELOPMENT-NOTES.md;
- TEMPEST-BEHAVIOR-SPEC.md;
- project instructions;
- reference sources;
- recordings;
- replay files;
- Git configuration.

Recommendations for later durable-document changes belong in the report. Do not
apply them during this analysis.


ROM SOURCE AUTHORITY

Use only the checked-in Atari arcade master disassembly/source under:

Reference\arcade\

including, where applicable:

Reference\arcade\Tempest Rom Source code.md

The local Atari arcade master disassembly is the sole ROM authority for this
task.

Do not use:

- Atari 2600 Tempest material;
- console Tempest source;
- another ROM revision;
- another online disassembly;
- a third-party reconstructed C port;
- MAME source as a substitute for the Atari game program;
- Internet explanations as proof of ROM behavior.

Do not search the Internet.

Do not treat Incitti as ROM evidence.


READ FIRST

Read completely or inspect all directly relevant sections of:

.github\copilot-instructions.md

TEMPEST-DEVELOPMENT-NOTES.md

TEMPEST-BEHAVIOR-SPEC.md

AI-Logs\Reports\2026-11-04-level7-preimplementation-analysis-report.md

AI-Logs\Reports\2026-11-04-level7-implementation-report.md

and any existing reports covering:

- directed Flipper rotation;
- Level 2 canflip;
- Level 3 geometry;
- Levels 4–7 movement;
- Level 7 ordinary and Tanker-cargo route behavior;
- Spike interaction;
- death/re-entry reconstruction.

Inspect current C# production and tests only to establish existing behavior and
the future implementation seam.

Do not infer arcade behavior from the fact that a C# test passes.


EVIDENCE CLASSIFICATIONS

Use these labels rigorously.

ROM-BACKED

A conclusion demonstrated by explicit Atari arcade ROM code/data and a complete
trace between the relevant table/program and runtime behavior.

ARCADE OBSERVED

Behavior directly observed by Neal on the original arcade cabinet or clean MAME
capture, as supplied in this task or already preserved in durable project
records.

REFERENCE VERIFIED

Behavior explicitly found in Mark Incitti's implementation. This establishes
only what Incitti's implementation does.

CURRENT C#

Behavior established by current source and tests.

PROJECT DECISION / APPROXIMATION

A deliberate C# mapping or design choice not literally established by ROM or
arcade observation.

HYPOTHESIS

A plausible explanation that has not yet been proved.

UNKNOWN

Evidence is insufficient.

Do not describe a hypothesis as ROM-backed.


PRIMARY ARCADE OBSERVATIONS TO INVESTIGATE

Treat the following as Neal's ARCADE-OBSERVED hypotheses requiring ROM
comparison and, where possible, ROM confirmation.

1. Flipper climbing behavior is consistent and level/tube-specific rather than
   selected randomly on every Flipper.

2. Players learn the characteristic below-rim Flipper pattern for each tube and
   rely on that consistency.

3. A level may have a recognizable lane-change sequence such as:

   2 - 3 - 2 - 2 - 2

4. On some tubes, when a Flipper crosses into a lane containing an existing
   Spike, the Flipper appears to become captured or locked to that Spike lane
   and follows that lane upward to the rim.

5. Level 5 appears to use this Spike-lock behavior.

6. Levels 6, 7, and 8 appear to allow Flippers to continue flipping across
   Spike lanes rather than becoming locked.

7. Level 9 appears to use Spike-lock behavior again.

8. Level 7's ordinary below-rim pattern has already received manual acceptance
   as approximately:

   2 - 3 - 2 - 2 - 2

9. Tanker-released Level 7 cargo has ROM-informed opposite-direction route
   behavior involving two lane changes, reversal, then three lane changes.

10. Spike interaction and the normal lane-change program may be separate
    mechanics, or Spike handling may be encoded within the selected movement
    program. Do not assume which explanation is correct.


CORE QUESTIONS

Resolve as many of these as the ROM supports.

A. LEVEL/TUBE PROGRAM SELECTION

1. Where does the ROM select the below-rim Flipper movement program?

2. Is selection indexed by:

   - displayed level;
   - zero-based level;
   - tube/board identity;
   - difficulty progression;
   - enemy origin;
   - another variable?

3. What ROM table or routine maps each level/tube to a movement program?

4. How many distinct Flipper movement programs exist?

5. Are these programs the CAM/p-code motion scripts already referenced in prior
   investigations?

6. Which program is assigned to each displayed Level 1 through Level 9?

7. Does the program assignment repeat later?

8. If current level and tube identity can diverge after the 16-tube cycle,
   which one controls movement?


B. MOVEMENT PROGRAM INTERPRETATION

For every ROM program used by Levels 1–9, decode:

1. Its exact byte/opcode sequence.

2. Every opcode's demonstrated meaning.

3. Lane-change counts.

4. Direction selection.

5. Reversal behavior.

6. Loop/restart behavior.

7. Termination behavior.

8. Pause or wait operations.

9. Straight-climb operations.

10. Conditional branches.

11. Any randomness and its exact decision point.

12. Whether a numeric sequence such as 2-3-2-2-2 is:

    - explicitly stored;
    - produced by looping/reversal opcodes;
    - dependent on available depth before the rim;
    - dependent on prior state;
    - a visual summary rather than a literal stored sequence.


C. SPIKE INTERACTION

Trace all below-rim Flipper code that reads or reacts to Spike state.

Determine:

1. How the ROM represents Spike presence and Spike height in a lane.

2. Whether Flipper behavior changes when:

   - starting in a Spike lane;
   - entering a Spike lane;
   - crossing a Spike lane;
   - being below the Spike tip;
   - being above the Spike tip;
   - reaching the Spike tip;
   - reaching a lane whose Spike is partially destroyed.

3. Whether the ROM has a distinct Spike-following, Spike-riding, locked-lane, or
   straight-climb state.

4. Whether entering a Spike lane:

   - changes the movement program;
   - pauses the movement program;
   - terminates the program;
   - changes an instruction pointer;
   - suppresses future lane changes;
   - merely changes climbing speed;
   - has no direct effect.

5. What exact condition would make a Flipper follow one Spike lane all the way
   to the rim.

6. What releases a captured Flipper, if anything.

7. Whether Spike capture is explicitly controlled by the selected level program.

8. Whether Spike capture is instead an emergent result of program termination,
   instruction-pointer state, available depth, Spike height, or another rule.

9. Whether Levels 5 and 9 share a ROM program or relevant flag.

10. Whether Levels 6–8 share a different program or relevant flag.

Do not invent a `locksToSpike` flag unless the ROM contains equivalent state or
the report clearly labels it as a future C# project translation.


D. ORIGINAL VERSUS TANKER-RELEASED FLIPPERS

Determine separately whether movement and Spike behavior differ for:

- original wave Flippers;
- Tanker-released cargo Flippers;
- reconstructed Flippers after player death/re-entry;
- any other Flipper source represented by the ROM.

Trace:

1. Program assignment.

2. Starting instruction pointer.

3. Initial direction/handedness.

4. Parent/Tanker-provided state.

5. Whether cargo siblings receive different directions or programs.

6. Whether Spike capture applies equally to original and cargo Flippers.

7. Whether re-entry restores the prior movement-program position or restarts it.

Do not generalize Level 7 cargo findings to all original Flippers or all levels
without proof.


E. RANDOMNESS VERSUS CONSISTENCY

Neal's major gameplay observation is that each tube's pattern is consistent and
learnable.

Determine exactly where randomness exists.

Possible examples to verify, not assume:

- initial handedness only;
- same-lane tie breaking;
- cargo sibling direction;
- program selection;
- opcode branch;
- no relevant randomness.

For each random input, explain whether it changes:

- the characteristic pattern;
- only its mirrored direction;
- its starting phase;
- its initial lane;
- nothing observable in lane-count sequence.

A program that is deterministic except for left/right mirroring can still
produce a consistent player-learnable pattern. State that distinction clearly
if supported.


LEVEL-BY-LEVEL MATRIX

Produce a matrix for displayed Levels 1 through 9.

Include a row for every level and columns for:

- displayed level;
- tube/board identity;
- Spike presence/configuration relevant to the question;
- selected ROM movement program/script;
- decoded or summarized lane-change sequence;
- initial direction rule;
- reversal/loop rule;
- random inputs;
- Spike-entry behavior;
- condition for straight/locked climbing;
- original Flipper behavior;
- Tanker-cargo behavior;
- current C# behavior;
- Incitti behavior;
- agreement/conflict;
- evidence classification;
- confidence/unknowns.

Do not leave a row out merely because the project has not implemented that
level yet.

For Levels 8–9, analyze ROM behavior only and clearly state that production C#
levels are not yet implemented.


REQUIRED LEVEL-SPECIFIC COMPARISONS

LEVEL 1

Verify the already accepted behavior that original Flippers climb directly
without below-rim lane changes.

Determine whether that comes from:

- a unique ROM program;
- program disabling;
- instruction-pointer initialization;
- a level flag;
- some other mechanism.

LEVEL 5

Investigate Neal's observation that entering a Spike lane appears to capture a
Flipper and make it follow that lane to the rim.

Identify the exact ROM mechanism if present.

LEVEL 6

Determine why Flippers appear able to flip across Spike lanes.

Compare its program and Spike handling directly with Level 5.

LEVEL 7

Reconcile:

- accepted ordinary 2-3-2-2-2 observation;
- ROM-informed 2/reverse/3 cargo behavior;
- Spike lanes;
- current C# implementation.

Do not alter the accepted Level 7 implementation.

LEVEL 8

Decode the ROM program even though C# Level 8 is not implemented.

Determine whether its behavior genuinely matches Levels 6–7 with respect to
crossing Spike lanes.

LEVEL 9

Investigate Neal's observation that Spike-lock behavior returns.

Compare its ROM program and state transitions directly against Level 5 and
Levels 6–8.


INCITTI COMPARISON

Use Incitti only as a secondary engineering reference.

Establish explicitly:

- whether Incitti contains per-tube program data;
- whether Incitti instead uses a universal `canflip` rule;
- how Incitti changes movement in a Spike lane;
- whether Incitti can reproduce level-specific Spike capture;
- whether any apparent similarity is incidental.

Do not allow Incitti behavior to override explicit ROM data or Neal's direct
arcade observation.


CURRENT C# AUDIT

Without modifying source, identify:

1. How current C# selects below-rim Flipper routes for Levels 1–7.

2. Whether Spike interaction is generic or level-specific.

3. Whether an existing generic rule could incorrectly lock or release Flippers.

4. Whether the accepted Level 7 program is isolated or entangled with older
   levels.

5. What code/data seams would support a future level-specific implementation.

6. Which existing tests protect accepted Levels 1–7.

7. Which missing tests would be necessary before any future implementation.

Do not recommend a broad rewrite if a small data-driven seam would suffice.


NO IMPLEMENTATION IN THIS TASK

Do not:

- add Level 8;
- add Level 9;
- change Level 5;
- change Level 6;
- change Level 7;
- alter Spike collision;
- alter lane geometry;
- change Tanker cargo;
- change enemy supply;
- change firing;
- change speed;
- add command-line diagnostics;
- add tests;
- update durable specs;
- commit;
- push.

This task discovers the rules first.


ANALYSIS METHODS

Permitted:

- source searches;
- ROM table decoding;
- opcode/p-code tracing;
- small read-only scripts for table extraction;
- deterministic hand traces;
- calculated instruction-pointer traces;
- comparison against existing tests and reports.

Any temporary analysis scripts or output must go under an ignored disposable
location and must be removed before completion unless preserved in the ignored
AI report.

Do not patch ROMs.

Do not modify reference files.

Do not launch the C# game.

Do not launch MAME.

Do not rely on visual guessing where ROM logic can answer the question.


VALIDATION OF THE ANALYSIS

Perform a consistency check before concluding:

1. Verify table indices against displayed and zero-based level numbering.

2. Check for off-by-one errors.

3. Confirm program bytes from their original ROM locations.

4. Confirm every opcode interpretation from its executor/dispatcher.

5. Trace at least one concrete example from each distinct program used by
   Levels 1–9.

6. Trace Levels 5, 6, 7, 8, and 9 specifically through any Spike-related branch.

7. Separate original and Tanker-cargo paths.

8. Check whether available tube depth truncates the visible sequence.

9. Compare conclusions against Neal's observations without forcing agreement.

10. Where ROM and observation appear inconsistent, preserve the conflict rather
    than rationalizing it away.


RECOMMENDATION BOUNDARIES

The final report must recommend one of these outcomes, supported by evidence:

A. Existing C# is already compatible; only future levels need new data.

B. A narrow level-program table/seam should be added later.

C. A narrow Spike-interaction correction is needed for specified implemented
   levels.

D. The ROM evidence remains insufficient and more arcade observation or
   debugger tracing is required.

E. A combination, separated into independent future milestones.

Do not implement the recommendation.

Keep accepted Levels 1–7 frozen unless the evidence demonstrates a specific,
material fidelity defect.

If a defect is found, identify:

- exact affected levels;
- exact incorrect behavior;
- ROM evidence;
- likely minimal correction;
- regression risks;
- manual acceptance scenarios.

Do not mix future Level 8/9 implementation with corrections to accepted
Levels 1–7.


REPORT REQUIREMENTS

Create:

AI-Logs/Reports/2026-11-06-level-specific-flipper-spike-rom-analysis-report.md

The report must include:

1. Starting Git state.

2. Sources examined.

3. Evidence classification rules.

4. Flipper movement-program selection mechanism.

5. CAM/p-code interpreter and opcode semantics.

6. Complete movement-program inventory relevant to Levels 1–9.

7. Level-by-level matrix for Levels 1–9.

8. Detailed Level 5 trace.

9. Detailed Level 6 trace.

10. Detailed Level 7 original-Flipper trace.

11. Detailed Level 7 Tanker-cargo trace.

12. Detailed Level 8 trace.

13. Detailed Level 9 trace.

14. Spike-state representation.

15. Exact Spike-entry/capture/lock behavior.

16. Original-versus-cargo comparison.

17. Randomness-versus-consistency findings.

18. Incitti comparison.

19. Current C# audit.

20. Confirmed ROM/arcade/C# agreements.

21. Conflicts and unresolved questions.

22. Recommended future implementation boundaries.

23. Proposed automated tests for a later implementation.

24. Proposed manual arcade/MAME acceptance scenarios.

25. Files written.

26. Final Git state.

27. Explicit confirmation that production, tests, durable documentation,
    references, Git history, and GitHub were not modified.


FINAL RESPONSE

Return a concise but technically specific summary containing:

1. Whether the ROM supports level/tube-specific Flipper movement programs.

2. How the program is selected.

3. Whether the player's expected pattern is deterministic, mirrored, random, or
   otherwise.

4. The decoded Level 1–9 mapping.

5. Whether Levels 5 and 9 share a Spike-lock mechanism.

6. Why Levels 6–8 cross Spikes, if established.

7. Exact meaning of “locked to a Spike,” if ROM-backed.

8. Original-versus-Tanker-cargo differences.

9. How Level 7's 2-3-2-2-2 observation maps to ROM behavior.

10. Where Incitti differs.

11. Where current C# differs.

12. Whether accepted Levels 1–7 need any future correction.

13. Recommended smallest next milestone.

14. Report filename.

15. Final Git status.

Do not claim certainty beyond the evidence.

Stop after analysis and wait for Neal's review.

TEMPEST_LEVEL_SPECIFIC_FLIPPER_SPIKE_ROM_ANALYSIS_COMPLETE