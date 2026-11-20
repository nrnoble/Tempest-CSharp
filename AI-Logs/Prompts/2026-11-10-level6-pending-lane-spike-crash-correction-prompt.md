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

Investigate and correct the deterministic canonical Level 6 startup crash in
the existing uncommitted combined-concurrency diagnostic candidate.

Repository:

C:\Users\nrnob\source\repos\Tempest

Accepted baseline beneath the candidate:

94ad02f52f8fe7f1eec79d60ffd7d5114b8ee933

The Level 6 candidate is already present in the worktree.

Read:

AI-Logs/Reports/2026-11-10-level6-concurrency-rom-investigation-report.md

AI-Logs/Reports/2026-11-10-level6-combined-concurrency-diagnostic-implementation-report.md

Do not redo the broader implementation.

This task is limited to identifying and correcting the pending-segment Spike
lookup defect exposed by manual testing.

Do not commit.
Do not push.
Do not launch the interactive game.


MANUAL FAILURE

Neal launched canonical Level 6 with:

dotnet run --project src\Tempest.Game -- --level 6 --lives 10

The game consistently crashed within approximately the first ten seconds.

Exact exception:

System.InvalidOperationException:
Level 6 pending Egg lane 8 has no persistent Spike.

Stack:

Tempest.Game.Levels.Level6AdmissionPolicy.GetPendingSegmentSpikeHeight(
    Level level,
    Int32 pendingLane)
in Level6AdmissionPolicy.cs:line 52

Tempest.Game.TempestGame.TrySelectLevel6DiagnosticType(
    Egg egg,
    Level3WaveLedger ledger,
    Nullable<EnemyType>& selectedType)
in TempestGame.cs:line 1083

Tempest.Game.TempestGame.CreateLevel3MixedEnemy(Egg egg)
in TempestGame.cs:line 858

Tempest.Game.Levels.Level.HatchReadyMixedEnemies(...)
in Level.cs:line 388

Tempest.Game.TempestGame.HatchEggs()
in TempestGame.cs:line 809

The failure is repeatable.

Manual Level 6 acceptance is BLOCKED.

Do not treat the previous automated validation as sufficient.


FIRST — VERIFY STATE

Before changing anything:

- verify branch, HEAD, upstream, index, and worktree;
- confirm the existing Level 6 candidate matches its implementation report;
- inspect the complete candidate diff;
- run git diff --check;
- inspect the actual Level 6 geometry, Egg lane semantics, persistent Spike
  creation, lane indexing, and GetPendingSegmentSpikeHeight implementation.

Do not make unrelated changes.


PURPOSE

Determine why a valid pending Level 6 Egg can use lane 8 while the new policy
cannot find a persistent Spike for lane 8.

Then make the smallest ROM-informed and project-consistent correction.

Do not merely suppress the exception.

Do not invent a fallback Spike height without establishing the correct mapping.


INVESTIGATION

Trace all relevant representations:

1. Canonical Level 6 lane count and lane indices.
2. Level 6 segment indices and their relationship to lane indices.
3. Initial persistent Spike creation:
   - number of Spikes;
   - lane/segment assignment;
   - omitted or special lanes;
   - whether every playable lane is expected to own one.
4. Egg lane assignment and whether an Egg lane denotes:
   - a lane;
   - a segment;
   - an edge;
   - a position between two lane/segment references;
   - another project representation.
5. The ROM meaning of the pending entry's segment Spike lookup.
6. How the C# geometry maps that ROM segment to persistent Spike state.
7. Whether lane 8 is:
   - a valid lane lacking a Spike by design;
   - mapped to a neighboring segment;
   - affected by zero-based/one-based indexing;
   - affected by closed-tube wrapping;
   - missing because of an initialization defect;
   - or another directly established cause.
8. Whether other valid Level 6 lanes can produce the same failure.
9. Why existing policy tests did not include this runtime-valid case.

Inspect the local ROM-derived listing as needed.

Do not search the internet for the ROM/disassembly.


NO SPECULATIVE FALLBACK

Do not automatically:

- return zero;
- use the initial C# Spike height;
- use the shortest global Spike;
- select the first Spike;
- choose a neighboring lane;
- wrap the lane index;
- default to Tanker;
- default to Spiker;
- catch and ignore the exception.

Any such mapping must first be justified by the ROM representation and current
C# geometry.

If the correct mapping cannot be established, stop with a read-only finding
rather than guessing.


CORRECTION BOUNDARY

If the exact mapping is established, make only the smallest correction needed
so every valid canonical Level 6 pending Egg can obtain the intended
pending-segment Spike value.

Preserve:

- pending-lane rather than global-shortest-Spike selection;
- Level 6 Tanker/Spiker minima and maxima;
- zero ordinary Flipper admission;
- seven-place headroom;
- Tanker cargo availability;
- terminal Spiker conversion;
- generic-obligation death reconstruction;
- deterministic behavior;
- Levels 1–5 and 7–8.

Do not redesign Level 6 geometry or persistent Spike behavior unless the
investigation proves that initialization itself is defective.


TESTS

Add a deterministic regression reproducing the exact failure:

- canonical Level 6;
- valid pending Egg on lane 8;
- minima satisfied so post-minimum Tanker/Spiker selection is reached;
- no exception;
- correct translated segment-Spike value used;
- expected Tanker/Spiker result obtained.

Also cover:

- every valid canonical Level 6 pending Egg lane;
- closed-tube boundary/wrap lanes where applicable;
- lanes with directly corresponding persistent Spikes;
- any valid lane whose mapping is indirect;
- proof that the decision uses the pending entry's mapped segment rather than
  a global Spike scan;
- copied configuration isolation;
- invalid/out-of-range lane behavior remaining fail-closed.

If lane 8 is valid but has no direct persistent Spike by design, encode the
established mapping explicitly in the tests.

Do not weaken the original invariant merely to avoid the exception.


VALIDATION

Use Release configuration.

Start with the exact lane-8 regression.

If it passes, run all Level6AdmissionPolicyTests.

Then run:

- Level6ProductionTests;
- persistent-Spike and Spiker tests;
- Tanker/cargo tests;
- death/reconstruction tests;
- direct Level 6 startup tests;
- campaign and recording/playback Level 6 paths;
- Levels 1–5 and 7–8 policy-isolation regressions.

If affected tests pass, run:

dotnet test TempestCSharp.slnx --configuration Release --blame-hang-timeout 5min

Then:

dotnet build TempestCSharp.slnx --configuration Release

Also run:

- git diff --check;
- complete candidate-diff review;
- final repository-state verification.

Explicitly report whether VSTest executed and exact counts.

Stop on FAIL, BLOCKED, TIMEOUT, CANCELED, or meaningful stall.

Do not weaken or bypass Windows security.


MANUAL HANDOFF

If automated validation passes:

- leave the candidate uncommitted;
- do not launch the game;
- provide Neal:

dotnet run --project src\Tempest.Game -- --level 6 --lives 10

The first manual acceptance gate is:

- Level 6 must run beyond the previous repeatable first-ten-second crash window
  without throwing.

Only after that passes should Neal resume the broader concurrency and
death-reconstruction checklist.

Do not claim Level 6 manual acceptance.


FLIPPER MOVEMENT SCOPE

Do not investigate or modify the Level 6 `$0b` Flipper movement program in
this crash-correction task.

The exact Tanker-released Flipper flipping pattern remains a separately
identified Level 6 fidelity investigation.

Do not let that important future work broaden this crash fix.


REPORT

Preserve this prompt and create a new ignored report:

AI-Logs/Reports/2026-11-10-level6-pending-lane-spike-crash-correction-report.md

Record:

- starting state;
- exact exception;
- confirmed root cause;
- ROM lane/segment mapping;
- C# lane/Spike mapping;
- why lane 8 failed;
- whether other lanes were vulnerable;
- smallest correction;
- tests added/updated;
- focused/affected/full validation;
- build result;
- static/repository state;
- exact manual command;
- manual acceptance pending.


GIT AND STOP

Do not commit.
Do not push.
Do not begin Level 7 concurrency.
Do not perform unrelated cleanup.

Stop after automated validation and manual handoff.


TEMPEST_LEVEL6_PENDING_LANE_SPIKE_CRASH_CORRECTION_COMPLETE
