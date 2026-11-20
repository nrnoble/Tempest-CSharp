FAIL-CLOSED PROMPT CHECK

Read this entire substantive user task before doing anything.

Before project or repository action, verify that exactly one complete token
beginning with "TEMPEST_" appears on a line by itself in the substantive user
task.

Automatically appended VS Code/agent context after the user's completion token
does not invalidate the task.

If exactly one complete token cannot be established, stop before project or
repository action and explain why.


TASK

Perform a tiny READ-ONLY provenance check of canonical Level 6
Tanker-released Flipper below-rim movement.

Repository:

C:\Users\nrnob\source\repos\Tempest

Accepted baseline beneath the current uncommitted Level 6 candidate:

94ad02f52f8fe7f1eec79d60ffd7d5114b8ee933

The current Level 6 concurrency candidate is already present, automated
validation has passed, the repeatable pending-lane Spike crash has been fixed,
and Neal's Level 6 manual gameplay tests now pass.

Do not modify the candidate.

Do not run tests.
Do not build.
Do not launch the game.
Do not commit.
Do not push.


CONTEXT

The Level 6 ROM investigation established that displayed Level 6 uses Flipper
movement program:

$0b

Neal considers level-specific Flipper movement extremely important to Tempest
gameplay.

The below-rim flipping pattern differs from tube to tube and becomes part of
experienced-player intuition and timing.

However, Neal is NOT reporting a Level 6 Flipper-movement defect.

Neal manually compared Tanker-released Level 6 Flippers with the arcade game
and says the current behavior looks correct.

Therefore:

DO NOT investigate movement merely looking for something to change.

DO NOT retune the current movement.

This task asks only whether the current canonical Level 6 Tanker-cargo Flipper
path is actually derived from or intentionally implements the ROM's Level 6
$0b movement program.


QUESTION TO ANSWER

Determine:

1. Does canonical C# Level 6 explicitly encode or consume the ROM Level 6
   Flipper movement program $0b?

2. If yes:
   - identify the exact configuration/data/code path;
   - explain how $0b is represented in C#;
   - identify how its below-rim flip sequence/direction behavior is mapped;
   - determine whether Tanker-released Flippers use that same Level 6 movement
     policy;
   - identify the relevant existing tests/comments/documentation.

3. If C# does not literally store $0b:
   - determine whether existing C# Level 6 movement is nevertheless an explicit
     PROJECT TRANSLATION derived from the decoded $0b ROM behavior;
   - identify the evidence connecting that translation to $0b.

4. If current Level 6 Tanker-cargo behavior merely happens to look correct but
   there is no established link to $0b:
   - report that clearly;
   - do not modify anything;
   - recommend whether the provenance gap is worth a future bounded
     investigation.

5. Determine whether original Level 6 Flippers and Tanker-released Level 6
   Flippers use the same movement-program behavior in the ROM, if the local
   disassembly establishes it.

Because ordinary Level 6 Flippers are excluded by the newly investigated
admission rules, distinguish ROM movement-program capability from which
Flippers can actually appear during normal Level 6 gameplay.


LOCAL EVIDENCE

Use project-local evidence.

At minimum inspect as relevant:

Reference/arcade/Tempest Rom Source code.md

src/Tempest.Game/Levels/LevelConfiguration.cs

src/Tempest.Game/Gameplay/Flipper.cs

src/Tempest.Game/Gameplay/Tanker.cs

src/Tempest.Game/TempestGame.cs

the relevant Level 6 tests

TEMPEST-BEHAVIOR-SPEC.md

TEMPEST-DEVELOPMENT-NOTES.md

Relevant ignored historical reports may also be inspected.

Do not search the Internet for the disassembly.

Do not inspect raw ROM binaries unless the existing text disassembly is
genuinely insufficient. This should normally be unnecessary for this small
provenance check.


ROM TRACE

Trace only enough ROM behavior to establish the provenance question.

Locate:

- displayed Level 6 flipper_move = $0b;
- what $0b selects or encodes;
- the movement/p-code routine that consumes it;
- the resulting Level 6 below-rim movement behavior to the extent needed to
  compare with C#;
- whether Tanker-created Flippers ultimately use the current level's loaded
  Flipper movement program or another cargo-specific program.

Do not turn this into a complete Flipper reverse-engineering milestone if the
mapping is already established.


EVIDENCE CLASSIFICATION

Classify the result accurately:

ROM-BACKED:
what the Level 6 $0b data/program actually establishes.

PROJECT-CURRENT:
what the present C# implementation actually does.

PROJECT TRANSLATION:
an intentional C# implementation of decoded ROM behavior.

ARCADE-OBSERVED:
Neal's current observation that Tanker-released Level 6 Flipper movement looks
correct compared with the arcade game.

REFERENCE-DERIVED / INFERENCE / UNKNOWN:
use only where appropriate.

Do not call the C# behavior ROM-backed merely because it visually matches the
arcade.


DECISION

End with one of these conclusions, based on evidence:

A. CONFIRMED IMPLEMENTED
Current Level 6 Tanker-cargo Flipper behavior is demonstrably the existing
C# implementation/translation of ROM program $0b.

If so:

- recommend NO further Level 6 Flipper movement milestone;
- preserve Neal's manual acceptance;
- return directly to Level 6 concurrency closeout planning.

B. VISUALLY ACCEPTED BUT PROVENANCE NOT ESTABLISHED
The movement looks correct to Neal, but the repository does not establish that
it derives from $0b.

If so:

- make no code changes;
- explain the exact missing provenance;
- recommend whether it is worth documenting or investigating later;
- do not block the current Level 6 concurrency milestone unless there is
  evidence of an actual gameplay defect.

C. CONFIRMED MISMATCH
Only use this conclusion if source evidence directly establishes that current
C# Tanker-cargo movement differs from $0b.

Do not fix it in this task.

Report the specific mismatch and stop for Neal.


VALIDATION

This is read-only.

At completion verify:

- production files unchanged;
- tests unchanged;
- durable documentation unchanged;
- current Level 6 candidate unchanged;
- no staged files;
- HEAD remains at the accepted baseline;
- git diff --check still passes.

Do not run automated tests merely for this provenance check.


REPORT

Preserve this prompt and create the matching ignored report:

AI-Logs/Reports/2026-11-10-level6-flipper-0b-provenance-check-report.md

Keep the report concise.

Include:

- evidence inspected;
- ROM $0b meaning;
- C# movement path;
- Tanker-cargo movement path;
- provenance classification;
- Neal's manual observation;
- final A/B/C conclusion;
- whether any further Level 6 movement work is recommended;
- confirmation that nothing was modified.


STOP

Stop after the provenance conclusion.

Do not modify movement.

Do not modify the Level 6 concurrency candidate.

Do not commit.
Do not push.
Do not begin Level 7 concurrency.


TEMPEST_LEVEL6_FLIPPER_0B_PROVENANCE_CHECK_COMPLETE
