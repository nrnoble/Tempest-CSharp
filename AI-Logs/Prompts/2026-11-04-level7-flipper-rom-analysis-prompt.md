IMPORTANT FAIL-CLOSED INSTRUCTION

Read this entire user-provided task before doing anything.

Before using tools, reading files, running commands, or taking repository action, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself.

Automatically appended VS Code or agent context after the token does not invalidate it.

This check detects incomplete or truncated user tasks, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project or repository action and explain why.

PROJECT

Repository:
C:\Users\nrnob\OneDrive\Repro\Tempest

Required checkpoint:
79e33886772424a31973332f33f3781b55e4c2f8

PURPOSE

Perform a read-only focused investigation of the Atari ROM Level 7 Flipper movement program.

Do not implement Level 7.
Do not change production code, tests, project documentation, or reference files.
Do not launch the game.
Do not commit or push.

Follow the existing project prompt/report logging requirements.

BASELINE

Run:

Set-Location 'C:\Users\nrnob\OneDrive\Repro\Tempest'
git rev-parse HEAD
git status --short --branch
git diff --stat
git diff --cached --stat

HEAD must exactly equal:

79e33886772424a31973332f33f3781b55e4c2f8

Tracked worktree and index must be clean.

Otherwise STOP.

REQUIRED INPUT

Read completely:

AI-Logs\Reports\2026-11-04-level7-preimplementation-analysis-report.md

Read the relevant repository copies of:

TEMPEST-DEVELOPMENT-NOTES.md
TEMPEST-BEHAVIOR-SPEC.md
Reference\arcade\Tempest Rom Source code.md
Reference\incitti\tempest.bmx
Reference\incitti\levelsets\levelset1.txt
src\Tempest.Game\Gameplay\Flipper.cs
src\Tempest.Game\Levels\DirectedLaneTransition.cs
src\Tempest.Game\Levels\LevelConfiguration.cs
src\Tempest.Game\TempestGame.cs

AUTHORITY

Use this order:

1. Observed/established Atari arcade behavior.
2. Atari ROM source and tables.
3. Incitti as an engineering reference.
4. Accepted C# behavior.
5. New project decisions only when the preceding evidence cannot answer the question.

Do not change accepted Levels 1–6.

Do not assume one universal below-rim Flipper movement policy.

LEVEL 7 MOVEMENT FINDING TO VERIFY

The Level 7 analysis reports that the ROM selects a repeating Flipper movement program:

- two flips in one direction;
- reverse direction;
- three flips in the other direction;
- reverse direction;
- repeat.

Verify this interpretation directly against the ROM source.

INVESTIGATION QUESTIONS

Determine as exactly as static evidence permits:

1. Where and how displayed Level 7 selects this movement program.
2. The program's initialization state.
3. Whether the first phase is the two-flip phase or three-flip phase.
4. How the initial direction is chosen.
5. Whether the initial direction is fixed, random, inherited, lane-dependent, player-dependent, or otherwise determined.
6. What ROM direction values mean.
7. How ROM directions map to:
   - increasing/decreasing lane indices;
   - clockwise/counterclockwise movement;
   - the C# Direction convention.
8. Whether the selected direction remains fixed throughout each phase.
9. Exactly when the two/three counter increments.
10. Exactly when reversal occurs.
11. Whether attempted, started, completed, or interrupted flips count.
12. What event or timer causes a below-rim flip attempt.
13. Whether the cadence is measured in updates, movement calls, height intervals, random decisions, or another unit.
14. Whether the ROM establishes a numeric delay comparable to the C# 20-tick LanePauseTicks.
15. Whether the sequence resets when:
   - the Flipper hatches;
   - the Flipper reaches the rim;
   - the player dies;
   - unresolved enemies reconstruct as Eggs;
   - Tanker cargo Flippers activate;
   - a new level starts.
16. Whether every Level 7 Flipper has independent phase/direction state.
17. Whether Tanker-released cargo uses the same Level 7 program.
18. Whether the movement program changes climb height or only lane.
19. Whether the clover’s irregular geometry affects only transition duration or also route choice.
20. Whether collision/firing continues normally during pauses and flips.
21. Whether the existing C# directed endpoint-hinge transition can render the complete ROM-prescribed route without geometry changes.
22. Whether the current generic C# canflip policy can remain available for Levels 2–6 while Level 7 selects a new explicit movement policy.

ROM VERSUS INCITTI

Explain precisely how Incitti differs.

Do not combine:

- ROM two/three reversal sequencing, and
- Incitti/C# 20-tick lane pause

unless the sources establish that combination.

If the cadence cannot be determined statically, identify it as unknown rather than choosing 20.

C# CONTRACT

If the evidence is sufficient, propose the narrowest explicit Level 7 movement contract.

The proposed design should preferably be level-configurable and preserve accepted Levels 1–6 unchanged.

Define, without implementing:

- required configuration/state;
- phase representation;
- remaining flips or phase counter;
- direction representation;
- initialization;
- transition trigger;
- reversal;
- rim handoff;
- death/re-entry policy;
- Tanker-cargo policy;
- deterministic testing seams;
- maintainability comments needed for non-obvious ROM behavior.

Do not introduce broad scripting or a generalized movement virtual machine unless the evidence demonstrates it is necessary.

ARCADE OBSERVATION PLAN

If any critical behavior remains unresolved, provide a concise observation plan for Neal.

Specify:

- whether arcade video or recorded C# playback would answer each question;
- which Flipper to follow;
- what lane transitions to record;
- how many consecutive flips/reversals should be observed;
- whether several Flippers or several playthroughs are needed;
- what would distinguish a fixed program from random movement;
- what evidence would establish direction mapping and cadence.

Recorded C# playback can reveal current C# behavior but cannot prove Atari behavior.

FINAL REPOSITORY CHECK

Run:

git status --short --branch
git diff --stat
git diff --cached --stat
git rev-parse HEAD

No tracked or staged changes are permitted.

Account separately for ignored prompt/report records.

FINAL REPORT

Report:

1. Verified starting HEAD.
2. Exact ROM movement program.
3. Exact Level 7 selection path.
4. Initial phase and direction behavior.
5. ROM-to-C# direction mapping.
6. Flip-count and reversal semantics.
7. Flip-trigger/cadence behavior.
8. Whether a 20-tick pause is supported.
9. Per-Flipper state requirements.
10. Tanker-cargo implications.
11. Death/re-entry and fresh-level reset behavior.
12. Geometry interaction.
13. Firing/collision interaction.
14. Differences from Incitti.
15. Narrowest proposed C# contract.
16. Automated tests required later.
17. Remaining unknowns.
18. Required arcade/video observations, if any.
19. Final Git state.
20. Confirmation that no implementation occurred.

Stop after analysis and wait for Neal.

TEMPEST_LEVEL7_FLIPPER_ROM_ANALYSIS_79E3388
