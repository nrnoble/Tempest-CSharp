FAIL-CLOSED PROMPT CHECK

Read this entire user-provided task before doing anything.

Before using tools, reading files, running commands, modifying files, or taking
any repository action, locate the unique completion token near the end of this
user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a
line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment
metadata, or other system-generated context may appear after the user's token
and do not invalidate the task.

This check is intended to detect an incomplete or truncated USER task, not
appended agent context.

If exactly one complete TEMPEST_ token cannot be found, stop before taking any
project or repository action and explain why.


TASK

Close out the manually accepted production Level 1 four-concurrent-Flipper
correction with final documentation, validation, commit, push, and repository
verification.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected committed baseline beneath the accepted unstaged candidate:

c1a974a625eea2dfaecf30d1b8d729e022ef17e8
Close out accepted spinner sensitivity diagnostic

Confirm the actual branch, HEAD, origin tracking state, index, working tree,
and untracked state before modifying anything.

The working tree contains the complete accepted Level 1 production correction.

Preserve the candidate.

Do not redo the implementation.

Do not begin Level 2 concurrency work.

Do not implement the ROM six-total-active gate.

Do not begin Level 9.


REQUIRED REPORTS

Read these reports before closeout:

AI-Logs/Reports/2026-11-09-low-level-difficulty-comparative-investigation-report.md

AI-Logs/Reports/2026-11-09-level1-four-flipper-cap-diagnostic-report.md

AI-Logs/Reports/2026-11-09-level1-cap-enemy-bullet-visibility-fix-report.md

AI-Logs/Reports/2026-11-09-level1-production-four-flipper-correction-report.md

Use these as historical evidence.

Do not rewrite them.

Also inspect:

- README.md;
- TEMPEST-DEVELOPMENT-NOTES.md;
- TEMPEST-BEHAVIOR-SPEC.md;
- all production files modified by the candidate;
- all new/modified Level 1 tests;
- the complete current Git diff.


FINAL MANUAL ACCEPTANCE FROM NEAL

Neal has now completed final production manual acceptance.

This acceptance applies to ordinary production Level 1, not merely the earlier
development diagnostic.

Neal reports that all requested production checks pass.

Specific manual results:

- Normal production Level 1 retains the improved arcade-like threat density.
- Level 1 is much improved.
- The resulting gameplay feels the same as or close to the original arcade
  Level 1.
- No more than four Flippers are active simultaneously under the new production
  rule.
- Neal visibly notices fewer Flippers occupying the tube simultaneously.
- Additional Eggs wait and later hatch as active Flipper slots become
  available.
- All ten Level 1 enemy obligations still appear and resolve.
- The wave remains substantial and does not feel artificially sparse.
- Enemy bullets are visible.
- Enemy bullets behave normally.
- Enemy bullets move, sound, collide, and threaten the Blaster normally.
- The previously discovered invisible-bullet rendering defect is fixed.
- Rim pursuit remains threatening.
- Stationary defense has not become trivially safe.
- Level 1 now provides more arcade-like breathing room and target
  manageability.
- Spinner control remains correct.
- Player firing remains normal.
- Super Zapper remains normal.
- Death/re-entry remains coherent.
- Level 1 completes normally.
- Natural Level 1 -> Level 2 progression works.
- Level 2 remains behaviorally unchanged.

Neal also manually played and recorded the complete production campaign through
Levels 1-8.

That broader manual run found no blocking regression.

Treat this as final manual acceptance of the production Level 1
four-concurrent-Flipper correction.

Do not require another manual acceptance cycle unless final automated validation
finds a genuine candidate-caused problem.


ACCEPTED PRODUCTION RULE

Production Level 1 now has:

Original supply:
10 Flippers

Maximum concurrently active Flippers:
4

The maximum of four is:

ROM-LISTING-BACKED / REV-2-BINARY-CONFIRMED

The current C# implementation of the cap is:

PROJECT TRANSLATION / MANUALLY ACCEPTED

Specifically:

- production Level 1 uses the authoritative active Flipper collection;
- when four Flippers are active, another ready Egg remains ReadyToHatch;
- the waiting Egg remains unresolved;
- it is not discarded;
- it is not returned to reserve;
- it is not scored;
- it is not rim-cleared;
- it does not reroll;
- it does not repeatedly consume randomness while blocked;
- once an active slot opens, an eligible ready Egg can hatch through the normal
  path;
- no update may transiently create a fifth active Flipper;
- all ten original Level 1 obligations must ultimately resolve.

Do not claim this waiting-Egg representation is the literal Atari pending-entry
implementation.


PRODUCTION ENTRY PATHS

Preserve the production rule on:

- normal no-option campaign Level 1;
- generic --level 1;
- fresh new campaign;
- Game Over restart to Level 1;
- campaign restart;
- Level 1 death/re-entry;
- gameplay recording startup;
- visible playback;
- headless playback;
- deterministic simulation/testing paths using canonical production Level 1.

Do not restore the old uncapped behavior through an alternate startup path.


REMOVED DIAGNOSTIC

The temporary option:

--level1-four-flipper-cap-test

was used only to isolate and manually validate the hypothesis.

The production implementation report says it has been removed after promotion
of the rule into production.

Preserve that cleanup.

Verify:

- the old diagnostic option is no longer accepted;
- obsolete parser wiring is gone;
- obsolete scenario identity is gone;
- obsolete terminal behavior is gone;
- obsolete diagnostic recording restrictions are gone;
- obsolete README commands are gone;
- useful tests have been converted into production tests.

Do not reintroduce the diagnostic.


ENEMY-BULLET VISIBILITY CORRECTION

Preserve the accepted enemy-bullet rendering correction.

The manually discovered defect was:

- enemy bullets existed;
- enemy bullets advanced;
- enemy bullets could collide with and destroy the Blaster;
- but their renderer classification omitted them in the diagnostic.

The corrected production architecture must retain:

- visible Level 1 enemy bullets;
- existing white bullet presentation;
- existing geometry;
- existing size;
- existing rotation;
- existing projection;
- existing active bullet state shared by rendering and collision;
- existing firing cadence;
- existing two-shot Level 1 capacity;
- existing sound;
- existing collision/damage behavior.

Production Level 1 must not require --enemy-bullets-test for bullet visibility.

Preserve existing bullet behavior for Levels 2-8 and the separate development
bullet test.


NO SIX-TOTAL-ACTIVE GATE

The Atari ROM also contains a six-total-active ordinary creation gate.

That rule is NOT part of this milestone.

Do not implement it during closeout.

Do not reinterpret final acceptance of the four-Flipper maximum as acceptance
of the six-active gate.


LEVEL 2 REMAINS UNCHANGED

Do not promote the Level 1 cap rule into Level 2 during this closeout.

The comparative investigation found Level 2 has related ROM concurrency
behavior, but Level 2 has its own geometry, below-rim lane changes, and gameplay
feel.

Level 2 concurrency will be evaluated as a separate milestone.

Preserve accepted Level 2 behavior exactly.


LEVELS 3-8 REMAIN UNCHANGED

Do not introduce the Level 1 production cap into Levels 3-8.

Preserve:

- wave selection;
- active limits;
- Flipper movement;
- Tanker behavior;
- cargo;
- Spikers;
- Pulsars;
- Fuseballs;
- geometry;
- open/closed topology;
- exits;
- progression;
- recording/playback.

The full Levels 1-8 campaign has been manually played/recorded by Neal after
the Level 1 correction with no blocking regression observed.


EGG TIMING REMAINS PROJECT BEHAVIOR

Preserve:

- four starting Level 1 Eggs;
- six reserve original obligations;
- 50-tick reserve release schedule;
- existing approach-selection cadence;
- existing approach limits;
- existing Egg movement;
- existing ready timing;
- existing deterministic lane/direction behavior.

The accepted improvement does not establish that these timings literally match
the arcade.

Do not retune them during closeout.


RIM SPEED

Preserve the accepted 2.5 rim-speed calibration.

The Level 1 A/B experiment materially improved arcade feel without changing rim
speed.

Do not alter:

- rim-speed calibration;
- rim pause;
- rim targeting;
- pivot duration;
- below-rim movement.

Record that the four-Flipper concurrency correction improved difficulty while
the accepted rim-speed calibration remained unchanged.

Do not interpret that as final proof that rim cadence is perfectly
arcade-equivalent.


PLAYER AND ENEMY FIRING

Do not change:

- player firing cadence;
- player-shot capacity;
- shot cooldown;
- shot lifetime;
- player-shot collision;
- enemy firing eligibility;
- enemy-shot capacity;
- enemy-shot holdoff;
- enemy-shot movement;
- enemy-shot lifetime;
- collision ordering.

These remained constant during the successful controlled experiment.


SPINNER AND GLOBAL SPEED

Preserve:

--spinner-sensitivity

default 1.0

and:

--speed

normal default 1.0

Do not change either as part of this closeout.

The Level 1 improvement was established without making either diagnostic value
part of the production correction.


DEATH / RE-ENTRY

Preserve accepted production Level 1 reconstruction.

Verify final tests continue proving:

- unresolved obligations are conserved;
- the four-Flipper rule remains active after death;
- a waiting Egg is not lost or duplicated;
- repeated deaths cannot create five active Flippers;
- progression remains reachable.


SUPER ZAPPER

Preserve accepted Super Zapper behavior.

Verify:

- delayed Zapper resolution remains unchanged;
- a target remains active until normal removal;
- removal frees an active Flipper slot;
- waiting Eggs may hatch afterward;
- the active count never exceeds four;
- supply remains conserved;
- charges/scoring/audio remain unchanged.


COMPLETION / NATURAL PROGRESSION

Preserve and validate:

- all ten original obligations eventually resolve;
- waiting Eggs prevent false early completion;
- no waiting-Egg deadlock remains when a slot becomes available;
- Level 1 completes normally;
- ordinary exit behavior remains unchanged;
- natural Level 1 -> Level 2 progression works;
- score/lives carry normally;
- Level 2 initializes normally.

Neal manually passed this behavior.


RECORDING / PLAYBACK

Neal played and recorded all eight production levels after the correction.

Preserve:

- recording schema;
- production startup identity;
- visible playback;
- headless playback;
- pause/resume;
- forward stepping;
- reverse reconstruction;
- screenshots;
- live-input handoff;
- --speed;
- spinner-sensitivity behavior.

Do not add metadata for the Level 1 cap. It is now ordinary production behavior.

Historical recordings may produce updated simulation trajectories because
production Level 1 intentionally changed.

Do not claim old recordings retain identical state at every boundary unless
tests prove it.

Fresh recordings must reproduce current production behavior.


STALL/HANG REGRESSION

The production implementation exposed one hours-long test hang.

The implementation report established the cause:

SimulationTests.HurryUpIsNotEnabledForNormalSpikerOrTankerScenarios

had an unbounded helper loop that assumed every production Level 1 Egg would
continue hatching without active-slot pressure.

With the correct four-Flipper cap, a fifth ready Egg waited, causing that stale
test helper to loop indefinitely because the test never resolved an active
Flipper.

This was:

- a stale test assumption;
- an unbounded test loop;
- NOT a production gameplay deadlock.

Preserve the corrected bounded test behavior.

Preserve explicit useful loop limits/failure diagnostics added during recovery.

Do not restore unbounded waits.

For the final full-suite validation, use a VSTest hang timeout.


DOCUMENTATION CLOSEOUT

Update:

README.md

TEMPEST-DEVELOPMENT-NOTES.md

TEMPEST-BEHAVIOR-SPEC.md

Record final manual acceptance:

- production Level 1 maximum four concurrent active Flippers;
- value four is ROM-backed;
- C# waiting-ready-Egg mechanism is a project translation;
- total Level 1 supply remains 10;
- all ten obligations still resolve;
- arcade-like threat spacing is materially improved;
- fewer simultaneous Flippers are observable;
- enemy bullets remain visible and correct;
- rim pursuit remains threatening;
- stationary defense remains unsustainable;
- natural Level 1-to-2 progression passed;
- Level 2 remained unchanged;
- Neal played and recorded Levels 1-8 with no blocking regression observed;
- the six-total-active gate is still deferred;
- Level 2 concurrency is deferred;
- no claim is made that every hidden Level 1 timing value now literally matches
  the Atari ROM.

Use appropriate evidence classifications:

ROM-LISTING-BACKED

REV-2-BINARY-CONFIRMED

PROJECT TRANSLATION

MANUALLY ARCADE-FIDELITY ACCEPTED

AUTOMATED TEST EVIDENCE

DEFERRED


FINAL VALIDATION

Run final validation against the complete manually accepted candidate.

Use bounded VSTest hang protection.

At minimum:


1. Focused production Level 1 cap tests.


2. Focused enemy-bullet firing/rendering/collision tests.


3. Focused Egg release/hatch/accounting tests.


4. Focused death/re-entry and Super Zapper tests.


5. Focused Level 1 completion and natural Level 1-to-2 progression tests.


6. Focused recording/playback/pause/step/reverse tests.


7. Focused Levels 2-8 regression tests.


8. SimulationTests with hang protection.


9. Expanded affected regression set with hang protection.


10. Full solution test suite:

dotnet test TempestCSharp.slnx --blame-hang-timeout 60s


11. Full solution build:

dotnet build TempestCSharp.slnx


12. Unstaged whitespace check:

git diff --check


13. After staging:

git diff --cached --check


Do not launch the interactive game.

Neal has already completed final manual acceptance.


WINDOWS APPLICATION CONTROL

Windows Application Control has intermittently blocked generated assemblies.

Do not weaken Windows security.

If a test or build is blocked:

- record the exact command and error;
- distinguish App Control from a test failure;
- retry only when useful;
- use --no-build only when it legitimately tests the current candidate;
- do not call a blocked command successful;
- do not commit unless final validation is genuinely complete.


DIFF REVIEW

This candidate accumulated work across:

- difficulty investigation follow-up;
- four-Flipper diagnostic;
- enemy-bullet rendering correction;
- production promotion;
- test-hang recovery.

Inspect the complete final diff carefully.

Verify every tracked modification belongs to this accepted Level 1 production
milestone.

Remove no valid historical evidence.

Do not stage unrelated files.


EXPECTED CHANGED FILES

The implementation report lists candidate changes including:

src/Tempest.Game/TempestGame.cs

src/Tempest.Game/Program.cs

src/Tempest.Game/Rendering/VectorRenderer.cs

tests/Tempest.Game.Tests/Level1ProductionFlipperCapTests.cs

tests/Tempest.Game.Tests/Level1ProductionEnemyBulletRenderingTests.cs

tests/Tempest.Game.Tests/SimulationTests.cs

tests/Tempest.Game.Tests/LevelProgressionTests.cs

tests/Tempest.Game.Tests/Level3TestModeTests.cs

tests/Tempest.Game.Tests/Level4CandidateTests.cs

README.md

TEMPEST-DEVELOPMENT-NOTES.md

TEMPEST-BEHAVIOR-SPEC.md

Verify the actual working tree instead of blindly staging this list.

Some candidate test files may be untracked until staged.

Confirm that every intended new production test file is included.


PROTECTED LOCAL FILES

Do not stage or commit:

Reference/arcade/roms.rev2/

UserData/

screenshots

gameplay recordings

Videos/

TEMP/

Temp/

AI-Logs/

test dumps or sequence files

build output

unrelated local material.

Do not run git clean.

Verify no VSTest hang dump/sequence artifact remains.


TEMP POLICY

TEMP/ is disposable local material.

Do not use TEMP/myNotes.md as authoritative evidence.

Do not move milestone reports into TEMP/.

Do not stage TEMP/.


AI-LOG POLICY

These instructions are explicit and non-conflicting.

1. Save this complete closeout prompt under:

AI-Logs/Prompts/

2. Create a NEW closeout report under:

AI-Logs/Reports/

3. Creating these ignored records is authorized and expected.

4. Existing investigation, diagnostic, correction, and implementation reports
   remain historical records.

5. Do not rewrite existing reports.

6. AI-Logs/ is intentionally Git-ignored.

7. Do not stage or commit AI-Logs.

8. Do not place the closeout report in TEMP/ or Temp/.


STAGING SAFETY

After all final validation passes:

1. Run:

git status --short --branch


2. Review:

git diff --stat

git diff


3. Derive the intended tracked-file allowlist from the reviewed candidate.


4. Stage only those intended files.

Do not use:

git add .

or:

git add -A


5. Print:

git diff --cached --name-only


6. Compare the staged list with the intended allowlist.


7. Stop on any mismatch.


8. Run:

git diff --cached --check


9. Review:

git diff --cached --stat

and the staged diff as needed.


COMMIT

If all validation and staged safety checks pass, commit the accepted production
Level 1 correction.

Suggested commit subject:

Implement accepted Level 1 four-Flipper maximum

Use the project's established Copilot co-author trailer if that remains normal
project policy.


PUSH

Push:

master

to the configured GitHub origin.

After push verify:

- local HEAD;
- local origin/master;
- live remote master;
- all three hashes match;
- tracked working tree is clean;
- index is clean;
- protected local/untracked material remains excluded.


PUBLIC GITHUB

The repository is public at:

https://github.com/nrnoble/Tempest-CSharp

After successful push:

- record the exact full commit hash;
- verify the Git remote directly;
- do not wait for GitHub web-search indexing;
- provide Neal the full commit hash so Microsoft 365 Copilot can separately
  attempt to retrieve the exact public commit.


CLOSEOUT REPORT

Create a NEW closeout report under:

AI-Logs/Reports/

Record:

1. Starting branch/HEAD.
2. Accepted baseline.
3. Reports reviewed.
4. Final manual acceptance supplied by Neal.
5. Full Levels 1-8 manual play/record regression result.
6. Accepted production Level 1 four-Flipper policy.
7. ROM-backed versus project-translation classification.
8. Egg waiting behavior.
9. Enemy-bullet visibility correction.
10. Removal of the temporary diagnostic.
11. Test-hang root cause and bounded-test correction.
12. Final documentation changes.
13. Final changed-file list.
14. Focused validation results.
15. SimulationTests result.
16. Expanded regression result.
17. Full-suite result.
18. Build result.
19. Unstaged diff-check result.
20. Staged diff-check result.
21. Final staged allowlist.
22. Commit hash.
23. Commit message.
24. Push result.
25. Local HEAD.
26. origin/master.
27. Live remote master.
28. Final Git status.
29. Protected local files deliberately excluded.
30. AI-Logs deliberately excluded.
31. TEMP and test dumps deliberately excluded.
32. Remaining Level 1 unknowns.
33. Deferred Level 2 concurrency work.
34. Deferred six-total-active gate.
35. Confirmation that the production Level 1 correction is manually accepted
    and closed.

If no blocking issue remains within this milestone, state that explicitly.


STOP POINT

After successful:

- documentation;
- final validation;
- staged review;
- commit;
- push;
- remote verification;
- closeout report;

STOP.

Do not start:

- Level 2 concurrency implementation;
- the six-total-active gate;
- Level 9;
- rim-speed retuning;
- Egg-schedule retuning;
- broader difficulty tuning;
- unrelated cleanup.


FINAL RESPONSE TO NEAL

Provide a concise final summary containing:

- commit hash;
- commit subject;
- focused validation results;
- SimulationTests result;
- expanded validation result;
- full-suite result;
- build result;
- diff-check results;
- push status;
- final Git status;
- confirmation protected local material was excluded;
- confirmation Level 1 four-Flipper maximum is manually accepted and closed;
- confirmation Levels 1-8 remain the accepted production campaign foundation;
- confirmation Level 2 concurrency and the six-active gate remain separate
  future work.

TEMPEST_LEVEL1_FOUR_FLIPPER_PRODUCTION_CLOSEOUT_20261109