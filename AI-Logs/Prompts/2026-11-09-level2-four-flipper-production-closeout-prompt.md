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

Close out the manually accepted production Level 2 four-concurrent-Flipper
correction with final documentation, bounded validation, staging, commit, push,
and repository verification.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected committed baseline beneath the current accepted unstaged candidate:

f362af7f87c23a9b983937f8733f5a4ac71d0162
Implement accepted Level 1 four-Flipper maximum

Confirm the actual:

- branch;
- HEAD;
- origin/master;
- upstream relationship;
- tracked worktree;
- index;
- untracked state;

before modifying anything.

The current working tree contains the complete accepted production Level 2
four-Flipper candidate.

Preserve that candidate.

Do not redo the implementation.

Do not begin Level 3 concurrency work.

Do not implement the ROM global max_enm mechanism.

Do not begin Level 9.


REQUIRED REPORTS

Read:

AI-Logs/Reports/2026-11-09-level2-concurrency-investigation-report.md

AI-Logs/Reports/2026-11-09-level2-four-flipper-cap-diagnostic-report.md

AI-Logs/Reports/2026-11-09-level2-bottom-artifact-investigation-report.md

AI-Logs/Reports/2026-11-09-level2-production-four-flipper-promotion-report.md

Also read as relevant:

AI-Logs/Reports/2026-11-09-level1-production-four-flipper-correction-report.md

AI-Logs/Reports/2026-11-09-level1-four-flipper-production-closeout-report.md

Inspect:

- complete current diff;
- README.md;
- TEMPEST-DEVELOPMENT-NOTES.md;
- TEMPEST-BEHAVIOR-SPEC.md;
- current Level 1 production cap;
- current Level 2 production cap;
- Level 2 tests;
- affected shared simulation code;
- recording/playback tests;
- progression tests.

Existing reports are historical records.

Do not rewrite them.


FINAL PRODUCTION MANUAL ACCEPTANCE FROM NEAL

Neal has manually tested the promoted normal production Level 2 candidate.

Production Level 2 passes manual gameplay acceptance.

The resulting normal production behavior retains the successful diagnostic
result and is judged much closer to the original arcade Level 2.

The previously supplied manual A/B observations remain accepted:

1. Fewer Flippers occupy the tube simultaneously.
2. Below-rim canflip activity is easier to follow.
3. There are fewer simultaneous pivots and converging threats.
4. Fewer Flippers reach the rim together.
5. There is more useful breathing room.
6. Enemy bullets remain normal and visible.
7. Rim pursuit remains dangerous.
8. The level still feels like Tempest rather than artificially sparse.
9. Queued Eggs continue entering as slots become available.
10. The full 12-enemy wave remains substantial.

Final production testing additionally passed:

- ordinary production Level 2 gameplay;
- existing recording-file playback without error.

Treat production Level 2 four-concurrent-Flipper behavior as:

MANUALLY ARCADE-FIDELITY ACCEPTED

Do not require another manual acceptance cycle unless final automated
validation identifies a genuine candidate-caused defect.


ACCEPTED LEVEL 2 PRODUCTION POLICY

Production Level 2 now has:

Original supply:
12 Flippers

Maximum concurrently active Flippers:
4

The four-Flipper limit is:

ROM-LISTING-BACKED / REV-2-BINARY-CONFIRMED

The C# waiting-ready-Egg representation is:

PROJECT TRANSLATION / MANUALLY ACCEPTED RESULT

Preserve:

- four starting Eggs;
- eight reserve originals;
- existing 50-tick reserve-release timing;
- existing queue limits;
- existing approach timing;
- existing readiness timing;
- accepted square geometry;
- accepted canflip;
- accepted Level 2 firing;
- accepted two-shot enemy-bullet capacity;
- accepted bullet presentation;
- accepted 2.5 rim-speed calibration;
- accepted death/re-entry;
- accepted Super Zapper;
- accepted completion;
- natural Level 2 -> Level 3 progression;
- recording/playback behavior.


LEVEL 1 IS FROZEN

Do not change accepted production Level 1.

Preserve:

- total supply 10;
- maximum four concurrently active Flippers;
- waiting-ready-Egg behavior;
- enemy-bullet presentation;
- movement;
- rim behavior;
- firing;
- death/re-entry;
- completion;
- natural Level 1 -> Level 2 progression;
- recording/playback.

Final validation must continue proving no Level 1 regression.


LEVELS 3-8 ARE FROZEN

Do not change accepted production Levels 3-8.

Do not add four-Flipper limits to additional levels during closeout.

Do not add a generalized species-limit framework.

Do not add the ROM global active-enemy gate.

Concurrency work for later levels requires separate investigation and manual
acceptance.


TEMPORARY LEVEL 2 DIAGNOSTIC

The temporary:

--level2-four-flipper-cap-test

served its purpose and was removed during production promotion.

Preserve that cleanup.

Verify:

- parser no longer accepts the option;
- obsolete scenario identity is gone;
- diagnostic-specific terminal/replay behavior is gone;
- obsolete conflict wiring is gone;
- obsolete recording/playback rejection is gone;
- README no longer advertises the diagnostic;
- useful tests have been converted into production behavior tests.

Do not reintroduce the diagnostic.


WAITING READY-TO-HATCH EGG PRESENTATION

The focused investigation established that the tiny object sometimes visible at
the far/deep end of Level 2 is a legitimate cap-blocked ReadyToHatch Egg:

- it remains in CurrentLevel.Eggs;
- it remains unresolved;
- Height remains zero;
- the shared renderer draws Eggs in that state;
- it is not a Flipper;
- it is not an enemy bullet;
- it is not a player shot;
- it is not a destruction effect;
- it is not duplicate Egg/Flipper representation;
- it is not stale renderer state;
- it is not a collision or accounting defect;
- it disappears through legitimate lifecycle transitions such as eventual
  hatching or death reconstruction.

The investigation disposition remains:

EXPECTED/HARMLESS PROJECT PRESENTATION — NO FIX

Do not alter this presentation during closeout.


NEAL'S ADDITIONAL ARCADE RECOLLECTION

Record this carefully and without overstating it.

Neal now notes:

- There is a reasonable possibility that a waiting Egg at the far/deep end may
  also be consistent with the original arcade game.
- If the arcade can display such a waiting/pending object, the phenomenon would
  likely be very rare during normal gameplay because the player continually
  destroys active enemies, opening creation capacity before an Egg remains
  visibly blocked for long.
- Neal has a very vague memory that he may possibly have seen something similar
  on the arcade machine.

This is NOT arcade verification.

Classify it as:

VAGUE ARCADE RECOLLECTION / UNVERIFIED

Do not classify it as:

- ARCADE VERIFIED;
- ROM BACKED rendering behavior;
- evidence that the C# 3x3 red mark is exact arcade presentation;
- evidence that the arcade uses the same waiting-Egg object model.

The correct durable conclusion remains:

The C# waiting-Egg presentation is harmless and accepted for now. It may or may
not correspond to a rare arcade-visible pending state. Evidence is insufficient
to decide.

The prior future-level concern remains useful:

If this presentation becomes common, conspicuous, or broadly recurring on
later production levels, revisit it.

Do not turn a vague memory into a new implementation requirement.


ROM GLOBAL ACTIVE GATE

Do not implement the broader ROM max_enm mechanism.

The Level 2 investigation established that the four-Flipper species maximum is
already the effective Level 2 restriction.

Do not broaden this closeout into later concurrency architecture.


DOCUMENTATION CLOSEOUT

Update as necessary:

README.md

TEMPEST-DEVELOPMENT-NOTES.md

TEMPEST-BEHAVIOR-SPEC.md

Record final Level 2 manual acceptance.

Durable documentation should state:

- production Level 2 total original supply remains 12;
- production Level 2 maximum concurrent active Flippers is four;
- value four is ROM-backed;
- waiting ReadyToHatch Eggs are a C# project translation;
- manual A/B diagnostic passed strongly;
- promoted normal production Level 2 also passed manual gameplay testing;
- Level 2 feels much closer to the arcade;
- canflip remains accepted;
- enemy bullets remain visible/normal;
- rim pursuit remains dangerous;
- all 12 obligations remain substantial;
- existing playback files play without error;
- Level 1 remains unchanged;
- Levels 3-8 remain unchanged;
- broader concurrency work remains deferred;
- the global ROM active gate remains unimplemented;
- the deep-end waiting Egg remains a harmless accepted presentation;
- Neal's possible arcade memory is vague/unverified only;
- revisit waiting-Egg presentation if it becomes broadly visible on future
  levels.

Do not claim hidden timing or presentation equivalence unsupported by evidence.


TEST-HANG POLICY

Preserve all current bounded test loops.

The project recently discovered an hours-long test hang caused by a stale
unbounded helper.

Do not reintroduce unbounded:

- while loops;
- wait-until-hatched loops;
- wait-until-Eggs-empty loops;
- completion loops;
- state-transition loops.

Every potentially long state-waiting test must have:

- explicit tick/iteration bound;
- useful failure message;
- useful state snapshot.


FINAL VALIDATION

Run final validation against the complete manually accepted candidate.

Use bounded VSTest hang protection.

Run at minimum:


1. Production Level 2 cap tests.


2. Level 2 square/canflip tests.


3. Enemy firing and visible-bullet tests.


4. Egg lifecycle/accounting tests.


5. Death/re-entry and Super Zapper tests.


6. Completion and Level 2 -> Level 3 progression tests.


7. Production Level 1 regressions.


8. Levels 3-8 affected regressions.


9. Recording/playback/pause/step/reverse tests.


10. SimulationTests:

dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --filter "FullyQualifiedName~SimulationTests" --blame-hang-timeout 60s


11. Expanded affected regression set with an appropriate hang timeout.


12. Full solution:

dotnet test TempestCSharp.slnx --blame-hang-timeout 60s


13. Full build:

dotnet build TempestCSharp.slnx


14. Unstaged whitespace check:

git diff --check


15. After staging:

git diff --cached --check


Do not launch the interactive game.

Neal has already completed final manual acceptance.


WINDOWS SECURITY FAIL-SAFE

Follow the established Tempest policy.

If Windows Application Control / Smart App Control blocks a generated DLL or
EXE:

- do not weaken or bypass security;
- do not repeatedly regenerate or retry the same blocked artifact;
- do not substitute stale binaries;
- retry only through a materially different safe route;
- never report a blocked command as passed.

If required validation cannot complete safely:

STOP VOLUNTARILY.

Preserve the worktree and report:

- completed work;
- blocked work;
- remaining validation;
- exact command;
- exact artifact;
- exact error;
- exact handoff Neal needs.


STALL / NO-PROGRESS FAIL-SAFE

Do not allow a command or analysis operation to run indefinitely.

Use explicit:

- VSTest hang timeouts;
- simulation tick bounds;
- iteration bounds;
- command/process bounds;
- clear stop conditions.

If meaningful progress stops:

1. Stop the operation.
2. Do not rerun the same unrestricted command.
3. Inspect available evidence.
4. Isolate with smaller bounded runs.
5. Determine if practical whether the cause is:
   - stale test assumption;
   - unbounded helper;
   - production deadlock;
   - Windows security block;
   - infrastructure/tool failure.
6. Preserve the candidate.
7. If safe recovery is unavailable, STOP VOLUNTARILY and report BLOCKED or
   PARTIAL.

Canceled, timed-out, hung, stalled, or blocked commands are never passes.


SELF-STOP REQUIREMENT

If any required closeout step cannot be completed:

- stop before staging/commit/push if validation is incomplete;
- preserve the candidate;
- do not conceal partial validation;
- do not claim success;
- write a BLOCKED/PARTIAL closeout report;
- state exact remaining work;
- give Neal the precise next action.

A precise handoff is preferable to waiting for Neal to force-stop the agent.


DIFF REVIEW

This milestone accumulated changes across:

- Level 2 diagnostic implementation;
- artifact investigation;
- production promotion.

Inspect the entire current tracked diff.

Verify every tracked modification belongs to the accepted Level 2 concurrency
milestone.

Do not blindly stage from a historical filename list.


EXPECTED CURRENT TRACKED CHANGES

The production promotion report listed:

README.md

TEMPEST-BEHAVIOR-SPEC.md

TEMPEST-DEVELOPMENT-NOTES.md

src/Tempest.Game/Program.cs

src/Tempest.Game/TempestGame.cs

tests/Tempest.Game.Tests/Level1ProductionFlipperCapTests.cs

tests/Tempest.Game.Tests/Level2FourFlipperCapTests.cs

Verify actual Git state.

Do not stage an unrelated change merely because it appears in this historical
list.

Confirm intended new test files are included if currently untracked.


PROTECTED LOCAL MATERIAL

Do not stage or commit:

Reference/arcade/roms.rev2/

UserData/

screenshots

gameplay recordings

Videos/

TEMP/

Temp/

AI-Logs/

build output

test dumps

sequence files

unrelated local material.

Do not run git clean.


AI-LOG POLICY

1. Save this complete substantive closeout prompt under:

AI-Logs/Prompts/

2. Create a NEW closeout report under:

AI-Logs/Reports/

3. Existing reports remain historical records.

4. Do not rewrite existing reports.

5. AI-Logs is Git-ignored.

6. Do not stage or commit AI-Logs.

7. Do not place the closeout report under TEMP/ or Temp/.


STAGING SAFETY

Only after final validation succeeds:

1. Run:

git status --short --branch


2. Review:

git diff --stat

git diff


3. Derive an explicit intended tracked-file allowlist from the actual reviewed
candidate.


4. Stage only the intended files.

Do not use:

git add .

git add -A


5. Print:

git diff --cached --name-only


6. Compare the staged list with the reviewed allowlist.


7. Stop on any mismatch.


8. Run:

git diff --cached --check


9. Review:

git diff --cached --stat

and enough staged diff detail to ensure correctness.


COMMIT

If and only if final validation and staging checks succeed, commit the manually
accepted production Level 2 correction.

Suggested commit subject:

Implement accepted Level 2 four-Flipper maximum

Use the normal Copilot co-author trailer if that remains established project
policy.


PUSH

Push master to the configured GitHub origin.

After push verify:

- local HEAD;
- local origin/master;
- live remote master;
- all three hashes match;
- tracked worktree is clean;
- index is clean;
- only intentional protected/untracked local material remains.


PUBLIC GITHUB

The public repository remains:

https://github.com/nrnoble/Tempest-CSharp

Record the exact full commit hash after push.

Direct Git remote verification is authoritative.

Do not spend substantial time waiting for external GitHub search/index caches
to expose the new commit.


CLOSEOUT REPORT

Create a NEW closeout report under:

AI-Logs/Reports/

Record:

1. Starting branch/HEAD.
2. Accepted committed baseline.
3. Reports reviewed.
4. Diagnostic manual A/B acceptance.
5. Final production manual acceptance.
6. Existing recording playback manual result.
7. Level 2 production policy.
8. ROM-backed versus project-translation classification.
9. Supply/accounting.
10. Waiting-Egg behavior.
11. Deep-end artifact investigation result.
12. Vague/unverified arcade recollection classification.
13. Future-level presentation concern.
14. Diagnostic removal.
15. Canflip preservation.
16. Firing/bullet preservation.
17. Rim behavior preservation.
18. Death/re-entry.
19. Super Zapper.
20. Completion/progression.
21. Recording/playback.
22. Level 1 regression.
23. Levels 3-8 regression.
24. Test-bound/hang safety.
25. Final documentation changes.
26. Final changed-file list.
27. Focused validation.
28. SimulationTests result.
29. Expanded validation.
30. Full-suite result.
31. Build result.
32. Unstaged diff-check result.
33. Staged diff-check result.
34. Staged allowlist.
35. Windows security status.
36. Stall/timeout status.
37. Commit hash.
38. Commit message.
39. Push result.
40. Local HEAD.
41. origin/master.
42. Live remote master.
43. Final Git status.
44. Protected material excluded.
45. Remaining Level 2 uncertainties.
46. Deferred future concurrency work.
47. Confirmation that production Level 2 concurrency correction is manually
    accepted and closed.

If no blocking issue remains, state that explicitly.


STOP POINT

After successful:

- documentation;
- bounded final validation;
- diff review;
- staging review;
- commit;
- push;
- remote verification;
- closeout report;

STOP.

Do not begin:

- Level 3 concurrency work;
- global max_enm implementation;
- waiting-Egg visual redesign;
- Level 9;
- rim-speed tuning;
- Egg-schedule retuning;
- unrelated cleanup.


FINAL RESPONSE TO NEAL

Provide a concise final summary containing:

- commit hash;
- commit subject;
- focused validation results;
- SimulationTests result;
- expanded regression result;
- full-suite result;
- build result;
- diff-check results;
- push status;
- final Git status;
- confirmation protected local material was excluded;
- confirmation Level 1 remained accepted;
- confirmation production Level 2 four-Flipper maximum is manually accepted and
  closed;
- confirmation Levels 1-8 remain the accepted production campaign foundation;
- confirmation deeper concurrency work remains separate future work.


BEFORE FINAL RESPONSE

Verify every applicable item is complete or explicitly reported BLOCKED/PARTIAL:

[ ] Final manual Level 2 acceptance recorded
[ ] Vague arcade recollection classified as unverified
[ ] Waiting-Egg presentation left unchanged
[ ] Level 1 unchanged
[ ] Levels 3-8 unchanged
[ ] Global gate NOT implemented
[ ] Diagnostic remains removed
[ ] All long tests bounded
[ ] Focused validation passed
[ ] SimulationTests passed
[ ] Expanded validation passed
[ ] Full suite passed with hang timeout
[ ] Build passed
[ ] Unstaged diff check passed
[ ] Durable documentation updated
[ ] Prompt saved
[ ] Closeout report created
[ ] Protected paths checked
[ ] Intended files only staged
[ ] Staged diff check passed
[ ] Commit created
[ ] Push succeeded
[ ] Local/remote/live hashes agree
[ ] Final tracked worktree/index clean
[ ] No security block misreported
[ ] No stall/cancel/timeout misreported
[ ] No unrelated next milestone started

Do not say that a promised prompt, report, validation, commit, or push exists
unless it actually exists.

TEMPEST_LEVEL2_FOUR_FLIPPER_PRODUCTION_CLOSEOUT_20261109