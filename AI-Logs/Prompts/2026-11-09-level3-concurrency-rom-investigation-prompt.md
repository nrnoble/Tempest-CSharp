FAIL-CLOSED PROMPT CHECK

Read this entire user-provided task before doing anything.

Before using tools, reading files, running commands, modifying files, or taking
any repository action, locate the unique completion token near the end of this
user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a
line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment
metadata, automatically supplied workspace context, or other agent-generated
material may appear after the user's token and do not invalidate the task.

This check is intended to detect an incomplete or truncated USER task, not
appended agent context.

If exactly one complete TEMPEST_ token cannot be found, stop before any project
or repository action and explain why.


TASK

Perform a bounded READ-ONLY investigation of Level 3 enemy concurrency in the
Tempest C# project.

Repository:

C:\Users\nrnob\source\repos\Tempest

This is the next concurrency milestone after Levels 1 and 2.

Levels 1 and 2 concurrency work is completed and accepted. Do not reopen,
retune, or modify Levels 1 or 2.

This task is investigation only.

DO NOT modify production code.
DO NOT modify tests.
DO NOT modify gameplay behavior.
DO NOT commit or push.
DO NOT launch the interactive game.

An ignored AI investigation report may be created under AI-Logs/Reports in
accordance with the repository instructions.

PURPOSE

Determine as accurately as possible how the original Atari Tempest controls
simultaneous/concurrent enemies on Level 3, and compare that behavior with the
current C# Level 3 implementation.

The central question is not merely the total Level 3 enemy supply.

We specifically need to understand how many unresolved enemies may be active
or otherwise concurrently represented during gameplay, how the game decides
when additional enemies may enter/hatch, and whether concurrency is governed
by one shared limit or by rules involving particular enemy types or lifecycle
states.

Level 3 is especially important because it is a mixed wave. Do not assume that
the Level 1 or Level 2 concurrency rule automatically applies to Level 3.


SOURCE PRIORITY

Use evidence in this order:

1. Atari ROM behavior/data and the project-local ROM-derived disassembly.
2. Other project-local raw ROM/reference material when needed to resolve what
   the disassembly means.
3. Existing documented arcade observations/manual acceptance evidence.
4. Existing C# implementation and tests, to determine current project behavior.
5. Other reference implementations only as secondary engineering references.

Do not search the Internet for the Tempest disassembly. The public ROM-derived
Tempest disassembly already exists locally in the project and has been used
repeatedly as a reference.

The repository also contains local raw ROM reference copies. Treat them as
reference material only. Do not modify, stage, relocate, normalize, or commit
them.

Do not assume Mark Incitti's implementation represents Atari behavior if it
differs from ROM-backed evidence.


INVESTIGATION

First confirm the actual repository state and current HEAD.

Then inspect the accepted Level 1 and Level 2 concurrency implementation and
its associated tests/documentation only as much as necessary to understand:

- what was changed;
- what concurrency concept the C# project currently implements;
- how the accepted Level 1 and Level 2 limits are represented;
- which parts are level-specific versus shared infrastructure.

Do not change those accepted behaviors.

Then analyze Level 3 independently.


A. ESTABLISH LEVEL 3 WAVE COMPOSITION

Identify the actual Level 3 enemy composition used by the Atari game and the
current C# implementation.

Account for important distinctions such as:

- original Flippers;
- Tankers;
- Tanker-released cargo Flippers;
- Pulsars;
- any other Level 3 enemy type if actually applicable;
- Eggs/reserve/inactive identities;
- active climbing enemies;
- rim enemies;
- latent cargo or other unresolved identities.

Do not infer a concurrency rule simply from the total wave supply.


B. ROM-BACKED CONCURRENCY INVESTIGATION

Trace the relevant ROM/disassembly logic that controls whether another Level 3
enemy can be introduced into active play.

Determine, where the evidence permits:

- which counter(s), table(s), variables, or state checks govern concurrency;
- whether Level 3 has an explicit numeric maximum;
- whether that maximum is shared across enemy types;
- whether different types have different limits;
- whether Tankers consume the same concurrency capacity as Flippers;
- how Tanker cargo is treated before release;
- how released cargo Flippers are counted;
- whether Pulsars consume the same capacity;
- whether rim enemies continue to occupy capacity;
- whether enemies in transitions/lifecycle states occupy capacity;
- what event frees capacity for another enemy;
- whether enemy firing state has any relevance to concurrency;
- whether spawning/hatching is additionally constrained by timers, randomness,
  enemy-type selection, or existing active population.

If the ROM uses something more complicated than a conventional "maximum active
enemies" counter, describe the actual mechanism instead of forcing it into the
terminology used by the C# project.

Trace sufficiently far through callers/data tables to establish what applies
specifically to Level 3.


C. CURRENT C# LEVEL 3 BEHAVIOR

Inspect the current production implementation and determine:

- what Level 3 concurrency limit currently exists, if any;
- exactly what entities count toward it;
- whether all Level 3 enemy types share it;
- how Eggs/reserve identities are handled;
- how Tankers and latent/released cargo affect it;
- how Pulsars affect it;
- whether rim enemies count;
- what frees capacity;
- whether current Level 3 behavior differs materially from the ROM mechanism.

Be particularly careful not to disturb the already extensively validated
Level 3 identity/ledger, Tanker cargo, death/reconstruction, Super Zapper, and
completion behavior.


D. COMPARE ROM AND C#

Produce a concise comparison showing:

1. ROM-backed behavior.
2. Current C# behavior.
3. Confirmed mismatches.
4. Areas where they already agree.
5. Anything the available evidence cannot establish.

Classify every important conclusion as one of:

ROM-BACKED
PROJECT-CURRENT
ARCADE-OBSERVED
REFERENCE-DERIVED
INFERENCE
UNKNOWN

Do not present an inference as ROM-backed.


E. RECOMMENDATION

Based on the investigation, recommend whether Level 3 concurrency should be
changed.

If a change is warranted, describe the smallest safe implementation strategy
but DO NOT IMPLEMENT IT in this task.

The recommendation should explicitly address whether we can implement a simple
Level 3 concurrency cap using the infrastructure developed for Levels 1 and 2,
or whether Level 3's mixed enemy/Tanker/cargo/Pulsar behavior requires a more
specific rule.

Preserve all accepted Level 3 lifecycle and identity semantics unless ROM
evidence conclusively demonstrates that concurrency requires interaction with
them.


VALIDATION

Because this is read-only:

- do not run the game;
- do not perform manual gameplay;
- do not modify tests;
- do not run broad test/build suites merely for ceremony.

Use bounded repository/disassembly inspection and any narrow read-only or
diagnostic commands genuinely necessary for the investigation.

At completion verify that tracked production/test/documentation files remain
unchanged.


REPORT

Create an ignored report under:

AI-Logs/Reports/

Use a descriptive filename similar to:

2026-11-09-level3-concurrency-rom-investigation-report.md

The report must preserve enough evidence that a later implementation chat does
not need to repeat the ROM investigation.

Include:

- repository starting state/HEAD;
- files and ROM/disassembly areas examined;
- relevant ROM addresses/symbols/tables/routines where determinable;
- Level 3 wave composition;
- ROM concurrency mechanism;
- treatment of each Level 3 enemy/lifecycle category;
- current C# concurrency behavior;
- ROM-vs-C# comparison;
- evidence classifications;
- uncertainties;
- recommended bounded implementation, if applicable;
- confirmation that no production/test/gameplay changes were made.

STOP after the investigation and report.

Do not implement a Level 3 concurrency correction.
Do not commit.
Do not push.

Neal will review the findings before authorizing implementation.


TEMPEST_LEVEL3_CONCURRENCY_INVESTIGATION_COMPLETE
