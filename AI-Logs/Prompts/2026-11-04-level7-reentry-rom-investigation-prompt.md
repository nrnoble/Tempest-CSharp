FAIL-CLOSED COMPLETENESS CHECK

Read this entire user-provided task before using any tools, reading files, running commands, or taking any repository action.

Before doing anything else, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context may appear after the user's completion token and do not invalidate it.

This check is intended to detect an incomplete or truncated user task, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project or repository action and explain that the task may be incomplete or truncated.


TEMPEST LEVEL 7 ROM INVESTIGATION: BOTTOM-ENTRY TANKERS, DEATH/RE-ENTRY, AND REMAINING-ENEMY ACCOUNTING

This task is ANALYSIS ONLY.

Do not modify production code, tests, project authority documents, reference files, or configuration.
Do not implement a fix.
Do not commit or push anything.
Do not launch the interactive game.
Do not alter accepted Levels 1-6.
Do not change the existing uncommitted Level 7 implementation.

The objective is to determine from the Atari Tempest ROM logic how displayed Level 7 handles remaining enemy supply, particularly why Tankers rather than individual Flippers appear from the bottom, including after player death.

SOURCE / EVIDENCE PRIORITY

Use evidence in this order:

1. Neal's newly established direct arcade observations below.
2. Atari ROM source/disassembly in:
   Reference/arcade/Tempest Rom Source code.md
3. Existing ROM-analysis reports and project documentation.
4. Incitti only as a comparison/reference implementation, not as authority when it conflicts with arcade or ROM behavior.
5. Current C# implementation only to identify differences after the Atari behavior has been established.

Do not infer Atari behavior from the current C# identity ledger, Egg representation, or Incitti's implementation.

IMPORTANT NEW ARCADE OBSERVATIONS

Treat the following as ARCADE VERIFIED for the purpose of this investigation:

- On displayed Level 7, repeated arcade play shows no individual Flippers entering directly from the bottom.
- Bottom-entry enemies observed are Tankers.
- Flippers appear as Tanker cargo.
- This behavior has been observed repeatedly, not as a single occurrence.
- The same general no-individual-bottom-entry-Flipper behavior has also been observed during play of Levels 8-10, although this investigation is specifically scoped to Level 7.
- Most importantly, Neal deliberately tested the death/re-entry case:
  - only one Flipper remained;
  - the player died;
  - when play resumed, that single Flipper did NOT re-enter from the bottom;
  - a Tanker subsequently entered instead.

Do not reinterpret that last observation as "two Flippers became one Tanker." That was an earlier hypothesis and is NOT established.

Instead determine what the ROM actually does with its remaining/pending enemy accounting.

SEPARATE MOVEMENT OBSERVATION

Neal has also now directly observed that the visible arcade Level 7 Flipper lane-change sequence is:

2 - 3 - 2 - 2 - 2

Do not investigate or modify movement behavior in this task except where enemy creation/re-entry logic happens to intersect with it. The purpose of this task is enemy supply and death/re-entry accounting.

KNOWN PRIOR ROM FINDING TO RECHECK, NOT BLINDLY ASSUME

A previous ROM analysis concluded that during death/re-entry:

- unresolved active/top/pending enemy counts are returned to pending state;
- active enemies are cleared/reinitialized;
- individual per-enemy movement state is not restored;
- Atari's internal representation should not automatically be equated with the C# identity ledger or Incitti Egg objects.

Re-verify the relevant source paths while performing this investigation.

The prior report identified relevant areas around:
- ROM line ~4840 for death/retraction/re-entry accounting;
- ROM line ~2201 for active/pending reset;
- player reload/reset around ROM line ~9166.

Use labels, addresses, callers/callees and actual control flow rather than relying only on approximate Markdown line numbers.

PRIMARY QUESTIONS

Answer these from executable ROM paths as far as the available source permits.

1. WHAT EXACTLY SURVIVES PLAYER DEATH?

Trace the death/retraction logic.

Determine:

- which counters or state variables represent unresolved enemies;
- how enemies currently active below the rim are counted;
- how enemies at the rim/top are counted;
- how already-pending enemies are counted;
- how Tankers and their Flipper cargo affect those counts;
- whether the ROM retains the previous enemy's species identity at all;
- whether it retains only a quantity or quantities;
- whether cargo Flippers are accounted for differently from Tankers/original supply.

Do not describe this using C# identity terminology unless explicitly comparing the systems afterward.

2. HOW IS THE POST-DEATH PENDING SUPPLY RECONSTRUCTED?

Follow the death/retraction result through the reset/reinitialization logic.

Identify:

- exactly what pending variables/state are initialized;
- what information has been discarded;
- what information survives;
- whether a prior Flipper remains identifiable as a Flipper;
- whether a previous Tanker remains identifiable as a Tanker;
- whether the remaining count is simply returned to a generic supply.

Trace sufficiently far forward to show how the next enemy-generation process receives this state.

3. HOW DOES DISPLAYED LEVEL 7 SELECT A TANKER VERSUS A FLIPPER?

Trace the actual enemy-selection/materialization path for displayed Level 7.

Do not stop at an eligibility/min/max table.

Determine the executable path that chooses an enemy type.

Identify:

- Level 7 parameter values involved;
- applicable Flipper controls;
- applicable Tanker controls;
- simultaneous min/max limits or other state-dependent restrictions;
- random-selection behavior, if present;
- rejection/retry behavior, if present;
- conversion/substitution behavior, if present;
- capacity restrictions that affect the result;
- whether the remaining pending count affects which enemy types can be created.

Explain precisely why the ROM can produce the observed Tanker.

4. CAN DISPLAYED LEVEL 7 EVER GENERATE AN ORDINARY BOTTOM-ENTRY FLIPPER?

This is a critical question.

Search for every relevant executable Level 7 path capable of materializing an ordinary Flipper from the bottom.

Distinguish carefully between:

- ordinary/original bottom-entry Flippers;
- Flippers released from Tankers;
- Flippers already at the rim;
- pending/re-entry representations;
- any special creation paths.

A table saying "Flippers enabled" does NOT prove that an ordinary Flipper can actually materialize from the bottom under reachable Level 7 gameplay conditions.

Determine whether:

A. ordinary bottom-entry Flippers are impossible on Level 7;
B. the ROM technically contains such a path but normal Level 7 constraints make it unreachable;
C. the path is reachable only under a genuine corner case;
D. ordinary bottom-entry Flippers can occur normally and Neal simply has not observed one.

Do not choose among A-D without tracing the relevant control flow.

5. TRACE THE ONE-REMAINING-FLIPPER DEATH CASE

This is the most important concrete test case.

Start from the logically relevant ROM state corresponding as closely as possible to:

- displayed Level 7;
- one visible unresolved Flipper remaining;
- player dies.

Trace forward symbolically.

Determine:

- what happens to that Flipper when enemies retract/reset;
- what counters change;
- whether the ROM knows afterward that the remaining enemy used to be a Flipper;
- what pending quantity remains;
- how enemy selection consumes that quantity;
- why a Tanker can appear afterward;
- whether one remaining unit receives ANY explicit special treatment.

Specifically search for branches testing an odd remaining quantity, a value of one, insufficient quantity for Tanker cargo, or otherwise treating a final single unresolved enemy specially.

If no such special case exists, say so explicitly and show why.

Do not invent a corner-case rule merely because one might be sensible.

6. WHAT DOES "ONE TANKER" MEAN FOR SUPPLY ACCOUNTING?

Determine whether a Tanker consumes:

- one pending enemy unit;
- two Flipper-equivalent units;
- some separate Tanker count;
- one original plus separately generated children;
- or another representation.

This question is important to interpreting Neal's observation that a Tanker can appear after death even when only one visible Flipper had remained.

Trace both natural Tanker creation and Tanker child creation sufficiently to establish the accounting relationship.

Do not assume that two cargo Flippers means a Tanker necessarily represents two units in the wave accounting.

7. DISTINGUISH INITIAL LEVEL SUPPLY FROM POST-DEATH RE-ENTRY

Determine whether a fresh Level 7 and a Level 7 reconstruction after death use:

- exactly the same selection machinery;
- the same selection machinery with different counts/state;
- or separate creation paths.

This may explain why arcade observations show Tankers entering in both situations.

8. CHECK AGAINST NEAL'S OBSERVATION

At the end of the ROM trace, explicitly compare the result with:

Level 7
one visible Flipper remains
→ player death
→ restart/re-entry
→ Tanker comes from bottom
→ no individual bottom-entry Flipper observed

State whether the ROM:

- directly explains this behavior;
- is consistent with it but leaves some mechanism unresolved;
- or appears to contradict it.

If there is a contradiction, investigate before concluding that the arcade observation or ROM is wrong.


INCITTI COMPARISON

After establishing the ROM behavior, briefly inspect Incitti's corresponding logic.

Neal observed that Incitti DOES release individual Flippers from the bottom on Level 7, unlike the arcade.

Identify the precise architectural/selection difference responsible if the source permits it.

Do not use Incitti behavior to reinterpret Atari behavior.


CURRENT C# COMPARISON

Only after the Atari mechanism is established, compare it with the current uncommitted Level 7 C# implementation.

Determine specifically:

- whether C# can release individual original Flippers from the bottom;
- whether death reconstruction preserves unresolved Flipper identities in a way Atari does not;
- whether the existing identity ledger/Egg reconstruction is responsible for the visible mismatch;
- what conceptual change would eventually be required to reproduce Atari behavior.

DO NOT implement that change.

Do not propose a broad redesign if a narrow Level-7-specific integration could reproduce the behavior while preserving accepted Levels 1-6.


REQUIRED EVIDENCE CLASSIFICATION

Clearly label conclusions as:

ARCADE VERIFIED
ROM-BACKED
REFERENCE VERIFIED (Incitti)
CURRENT C# BEHAVIOR
PROJECT DECISION / PROPOSED INTEGRATION
UNKNOWN

Do not elevate a static inference to ARCADE VERIFIED.

Do not characterize Neal's observations as ROM facts unless the ROM trace independently establishes them.


OUT OF SCOPE

Do not:

- modify production code;
- modify tests;
- modify TEMPEST-BEHAVIOR-SPEC.md;
- modify TEMPEST-DEVELOPMENT-NOTES.md;
- modify ROM/reference files;
- implement Level 7 corrections;
- modify accepted Levels 1-6;
- implement Level 8+;
- retune movement timing;
- change Flipper firing;
- change Tanker movement or firing;
- change Spikers;
- change Super Zapper behavior;
- change Spike behavior;
- change audio;
- modify replay;
- launch the game;
- commit;
- push.

This is a read-only investigation.


DELIVERABLE

Create a detailed investigation report under:

AI-Logs/Reports/

Use a descriptive date-prefixed filename paired with the saved prompt under:

AI-Logs/Prompts/

The report must include:

1. Executive conclusion.
2. Arcade-verified facts supplied by Neal.
3. Relevant ROM variables/counters.
4. Death/retraction control-flow trace.
5. Pending/reset control-flow trace.
6. Level 7 enemy-selection trace.
7. Tanker supply/accounting semantics.
8. Ordinary bottom-entry Flipper reachability analysis.
9. Exact symbolic trace of the one-Flipper-remains death case.
10. Fresh-Level-7 versus post-death selection comparison.
11. Incitti comparison.
12. Current C# comparison.
13. Any remaining unknowns.
14. Narrow implementation implications, WITHOUT making changes.
15. Exact ROM labels/addresses and source-line references supporting the conclusions.

Use concise pseudocode or a Mermaid diagram only if it materially clarifies the state/count flow.

If the ROM does not establish something, say so. Do not speculate.


FINAL RESPONSE

Return a concise summary containing:

- the central ROM finding;
- whether ordinary bottom-entry Flippers are actually reachable on Level 7;
- exactly what happens to the one-remaining-Flipper case, to the extent proven;
- why a Tanker appears after death, if proven;
- whether Incitti differs and why;
- whether the current C# implementation differs;
- remaining unknowns;
- report filename;
- confirmation that no production/test/reference/project-authority changes, commit, push, or game launch occurred.

Stop after the investigation and await Neal's review.

TEMPEST_LEVEL7_REENTRY_ROM_INVESTIGATION_COMPLETE