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

Implement a tightly bounded development-only Level 2 diagnostic that applies
the Atari ROM's maximum of four concurrently active Flippers.

The purpose is to test whether excessive simultaneous Flipper density is making
accepted C# production Level 2 harder than Neal's original Atari arcade Level 2.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected accepted baseline:

f362af7f87c23a9b983937f8733f5a4ac71d0162
Implement accepted Level 1 four-Flipper maximum

Confirm the actual:

- branch;
- HEAD;
- origin/master;
- tracking state;
- index;
- tracked worktree;
- untracked state;

before modifying anything.

The committed baseline must be clean except for protected intentional local
material.

Do not stage, commit, or push.

Leave the completed diagnostic candidate unstaged for Neal's manual Level 2 A/B
comparison.

Do not promote the Level 2 cap into production.

Do not begin Level 9.


PRIMARY INVESTIGATION REPORT

Read this report fully before designing or modifying the diagnostic:

AI-Logs/Reports/2026-11-09-level2-concurrency-investigation-report.md

Reuse its established findings.

Do not repeat the broad ROM investigation unless a specific implementation
detail requires confirmation.


OTHER REQUIRED CONTEXT

Read as relevant:

AI-Logs/Reports/2026-11-09-level1-production-four-flipper-correction-report.md

AI-Logs/Reports/2026-11-09-level1-four-flipper-production-closeout-report.md

Also inspect:

- current production Level 1 cap implementation;
- current production Level 2 configuration;
- Level 2 square/canflip implementation and tests;
- Egg release, approach, readiness, and hatch paths;
- death/re-entry;
- Super Zapper;
- enemy firing and bullet rendering;
- recording/playback scenario identity;
- current direct Level 2 diagnostic policy;
- CLI parsing and exclusivity rules;
- README.md;
- TEMPEST-DEVELOPMENT-NOTES.md;
- TEMPEST-BEHAVIOR-SPEC.md.

Existing reports are historical records.

Do not rewrite them.


ESTABLISHED ROM FINDINGS

The Level 2 concurrency investigation established:

ROM-LISTING-BACKED / REV-2-BINARY-CONFIRMED:

- Medium/default Level 2 original supply is 12 Flippers.
- Level 2 maximum concurrent Flippers is 4.
- The four-Flipper value is unchanged across Easy, Medium, and Hard.
- The ROM's general max_enm table value is 6.
- The max_enm runtime comparison is inclusive rather than a strict six-object
  ceiling.
- The broader global gate does not independently constrain Flipper-only Level 2
  because the four-Flipper species maximum is stricter.
- Below-rim and rim Flippers both count toward the species maximum.
- A Flipper remains counted until its active slot is actually removed.
- A pending-entry unit does not count as an active Flipper.
- Projectiles do not count as active enemies.
- Delayed Super Zapper targets remain active until normal removal.

Do not implement the broader max_enm gate in this milestone.


ESTABLISHED CURRENT C# FINDINGS

Current accepted production Level 2 has:

- displayed Level 2 identity;
- closed 16-lane square geometry;
- Flipper-only original composition;
- total original supply 12;
- four starting Eggs;
- eight reserve originals;
- 50-active-tick reserve release;
- maximum Egg queue 4;
- at most two approaching/ready Eggs;
- 50-active-tick approach-selection interval;
- eight-update Egg movement cadence;
- accepted deterministic spawn-lane sequence;
- accepted below-rim canflip behavior;
- accepted Flipper firing;
- accepted two-enemy-bullet capacity;
- accepted rim-speed calibration;
- no active-Flipper species cap;
- no global active-enemy cap.

The bounded current-C# delivery measurement established:

- one active Flipper at boundary 65;
- two at boundary 115;
- three at boundary 189;
- four at boundary 263;
- five at boundary 289;
- all 12 active by boundary 615.

These are bounded C# delivery measurements, not arcade timing measurements.

Production Level 2 therefore demonstrably exceeds the ROM-backed four-Flipper
maximum.


EXPERIMENTAL HYPOTHESIS

The diagnostic tests this single hypothesis:

Reducing Level 2 from as many as 12 concurrently active Flippers to the ROM-
backed maximum of four will reduce excessive simultaneous threat density and
produce gameplay closer to Neal's original arcade Level 2.

Do not prejudge manual acceptance.

Level 2 differs from Level 1 because its Flippers can change lanes below the
rim. Four Level 2 Flippers may still produce substantial pressure through:

- simultaneous climbing;
- below-rim canflip pivots;
- lane clustering;
- firing;
- grouped rim arrival;
- multi-direction rim pursuit.

The purpose of the diagnostic is to let Neal judge those interactions directly.


DIAGNOSTIC OPTION

Implement exactly:

--level2-four-flipper-cap-test

This is:

- development-only;
- Level 2 only;
- terminal/replayable according to existing direct-scenario convention;
- a controlled comparison;
- not a production difficulty setting.

Do not add aliases such as:

--easy
--level2-cap
--flipper-cap
--difficulty


DIAGNOSTIC STARTUP

The diagnostic must use the canonical accepted production Level 2
configuration:

LevelConfiguration.Level2Square

Preserve:

- Level 2 displayed/internal identity;
- square geometry;
- 16 closed playable lanes;
- total supply 12;
- four starting Eggs;
- eight reserve originals;
- accepted spawn-lane sequence;
- accepted Egg release/approach behavior;
- accepted canflip behavior;
- accepted firing;
- accepted two-shot capacity;
- accepted movement;
- accepted rim pursuit;
- accepted 2.5 rim-speed calibration;
- accepted scoring;
- accepted lives behavior;
- accepted Super Zapper;
- accepted death/re-entry;
- accepted rendering/audio;
- accepted spinner behavior;
- accepted --speed behavior.

The only experimental gameplay difference must be:

No more than four active Level 2 Flippers may coexist.


PRODUCTION LEVEL 2 MUST REMAIN UNCHANGED

Without the diagnostic option:

- normal production Level 2 remains uncapped;
- generic --level 2 remains accepted production Level 2;
- natural Level 1 -> Level 2 progression remains unchanged;
- production Level 2 may still exceed four active Flippers;
- natural Level 2 -> Level 3 progression remains unchanged.

Do not silently promote the cap into production.

Do not modify LevelConfiguration.Level2Square globally unless the experimental
override is provably inactive for every production path.


LEVEL 1 IS FROZEN

Production Level 1 is manually accepted, committed, and closed.

Preserve exactly:

- total supply 10;
- maximum four active Flippers;
- ready-Egg waiting behavior;
- enemy-bullet visibility;
- completion;
- death/re-entry;
- recording/playback;
- Level 1 -> Level 2 progression.

Do not generalize the Level 1 production predicate in a way that changes its
behavior.

Use Level 1 only as an architectural reference.


LEVELS 3-8 ARE FROZEN

Do not change accepted Levels 3-8.

The Level 2 diagnostic must not alter:

- configuration;
- species selection;
- active limits;
- geometry;
- movement;
- firing;
- cargo;
- Spikes;
- exits;
- topology;
- progression;
- recording/playback.


CAP SEMANTICS

In the diagnostic only:

- Count active Flippers from the authoritative active Flipper collection.
- A Flipper begins counting when the ordinary hatch path adds it to that
  collection.
- A Flipper stops counting only when the existing lifecycle removes it.
- Below-rim Flippers count.
- Actively pivoting Flippers count.
- Paused Flippers count.
- Rim Flippers count.
- Delayed Super Zapper targets count until normal removal.
- At four active Flippers, another ready Egg cannot hatch.
- The blocked Egg remains ReadyToHatch.
- The blocked Egg remains unresolved.
- The Egg is not destroyed.
- The Egg is not rim-cleared.
- The Egg is not returned to reserve.
- The Egg is not replaced.
- The Egg does not repeatedly reroll or consume hatch randomness.
- Once an active slot opens, an eligible waiting Egg may hatch through the
  ordinary hatch path.
- No update may transiently create a fifth active Flipper.
- All 12 original Level 2 obligations must eventually resolve.

Do not add a second queue.

Do not add a cached active counter that can drift from the actual collection.

Do not alter the existing Egg state machine unnecessarily.


WAITING-EGG ORDER

Preserve deterministic and maintainable waiting behavior.

When capacity opens:

- use existing collection/order semantics;
- hatch eligible waiting Eggs through the ordinary path;
- do not reorder waiting Eggs merely because they were blocked;
- do not consume random state while still blocked;
- do not retarget lane/direction/type every blocked tick.

If two ready Eggs exist and two slots become available, the existing hatch
phase may fill both slots if that is already the ordinary loop behavior.

Do not add an artificial one-hatch-per-tick rule.


NO GLOBAL ACTIVE GATE

Do not implement:

- a six-total-active gate;
- a seven-object interpretation of the ROM inclusive comparison;
- a generic global enemy cap;
- species caps for other enemy types.

The four-Flipper species maximum fully isolates Level 2's relevant ROM policy.


RELEASE AND APPROACH TIMING

Do not alter:

- four starting Eggs;
- reserve quantity;
- 50-tick reserve release;
- maximum Egg queue;
- approach-selection interval;
- maximum approaching/ready Eggs;
- Egg update cadence;
- deterministic spawn lanes;
- readiness timing.

Reserve delivery and Egg approach may continue while four Flippers are active.

Ready Eggs wait only at the final hatch-eligibility boundary.

This diagnostic must not retime the wave to imitate unknown ROM entry cadence.


CANFLIP

Preserve every accepted Level 2 canflip behavior:

- initial direction selection;
- active-pivot direction latching;
- destination latching;
- endpoint-hinge geometry;
- directed sweep;
- lane pause;
- below-rim movement;
- rim clamp;
- completed-pivot lethality;
- shot ownership;
- firing suppression during active pivots;
- shot-delay advancement while pivoting;
- ready shot after pivot completion;
- deterministic randomness behavior.

The diagnostic changes the number of concurrent Flippers, not the behavior of
an individual Flipper.


FIRING AND BULLETS

Preserve:

- accepted Level 2 Flipper firing eligibility;
- two concurrent enemy-shot slots;
- accepted 77-tick C# holdoff;
- pressure thresholds;
- behavior while climbing/pausing;
- behavior during active canflip pivots;
- bullet spawn;
- bullet speed;
- bullet lifetime;
- bullet visibility;
- bullet geometry;
- bullet sound;
- bullet collision;
- shared active bullet state used by simulation and rendering.

A four-Flipper maximum naturally reduces the maximum number of simultaneous
eligible shooting sources, but must not change shot capacity or firing rules.

Do not require --enemy-bullets-test for ordinary diagnostic bullet visibility.

Do not repeat the prior invisible-bullet defect.


RIM BEHAVIOR

Preserve:

- accepted 2.5 rim-speed calibration;
- rim pause;
- direction selection;
- persistent rim direction;
- directed pivot geometry;
- player collision;
- player-shot ordering.

Do not tune rim behavior during this experiment.


DEATH / RE-ENTRY

The diagnostic must remain coherent across death.

After death/re-entry:

- unresolved supply remains conserved;
- the four-Flipper diagnostic cap remains enabled;
- no more than four active Flippers may coexist;
- unresolved units reconstruct through the existing Level 2 lifecycle;
- canflip configuration remains active;
- no identity/obligation is lost or duplicated;
- enemy bullets follow accepted clearing policy;
- score/lives/Super Zapper remain normal;
- gameplay remains completable.

Do not redesign Level 2 reconstruction.


SUPER ZAPPER

Preserve accepted Level 2 Super Zapper behavior.

A Zapper target remains active and consumes a cap slot until normal delayed
removal.

After removal:

- an active slot is genuinely available;
- waiting Eggs may hatch according to existing phase ordering;
- no update exceeds four active Flippers;
- unresolved accounting remains conserved.

Do not make waiting Eggs or reserve supply Zapper targets.


COMPLETION AND TERMINAL POLICY

The development diagnostic must remain completable.

Preserve all 12 obligations.

The cap must not cause:

- false early completion;
- missing Flippers;
- duplicated Flippers;
- permanent ready-Egg deadlock;
- unresolved supply after a slot becomes available;
- a terminal scenario that cannot replay.

Use the existing direct Level 2 terminal/replay convention:

- successful completion remains terminal for the development diagnostic;
- replay/restart uses the same diagnostic configuration;
- it does not progress into production Level 3;
- normal production Level 2 still progresses naturally to Level 3.

Document this clearly.


COMMAND-LINE COMPOSITION

Allow sensible orthogonal live options according to existing policy:

- --lives;
- --speed;
- --spinner-sensitivity;
- --rim-speed if currently accepted for direct diagnostic gameplay;
- --enemy-bullets-test only if it already composes with the existing direct
  Level 2 scenario without changing ordinary production bullet behavior.

Reject:

- duplicate diagnostic options;
- other exclusive scenario selectors;
- conflicting direct-level selectors;
- input-diagnostic mode;
- visible playback selectors;
- headless playback selectors;
- unsupported recording combinations.

Follow existing strict parser conventions.


RECORDING / PLAYBACK POLICY

Do not silently record the diagnostic as ordinary production Level 2.

Preferred bounded policy:

- reject --record-gameplay with the diagnostic;
- reject visible/headless playback combinations with the diagnostic;
- provide a clear message that the development-only scenario is not represented
  by the existing production recording identity;
- do not change the recording schema;
- do not add a new recording version;
- do not add cap metadata.

Preserve all production recording/playback behavior.

If the current architecture already provides a truly generic safe scenario
identity that can represent the diagnostic without schema expansion, inspect it
carefully before deciding. Do not broaden scope merely to support recording the
experiment.


DIAGNOSTIC TITLE / FEEDBACK

Use a clear development-only title or startup message identifying the capped
Level 2 scenario.

Do not add a gameplay HUD overlay.

Do not add permanent telemetry.

The presentation should otherwise resemble ordinary production Level 2.


IMPLEMENTATION ARCHITECTURE

Prefer the smallest design that preserves the accepted Level 1 implementation.

Likely approach:

- add an explicit Level 2 diagnostic scenario selector;
- use the canonical LevelConfiguration.Level2Square object;
- scope the four-Flipper hatch guard to either:
  - accepted production Level 1; or
  - the Level 2 diagnostic;
- retain authoritative flippers.Count semantics;
- retain existing ready-Egg waiting;
- avoid a general production configuration property until Level 2 manual
  acceptance determines whether promotion is justified.

Do not create divergent duplicate Level 2 simulation logic.

Do not turn the temporary diagnostic into a global difficulty system.


REQUIRED AUTOMATED TESTS

Add focused tests proving all of the following.


1. PRODUCTION LEVEL 2 UNCHANGED

- production Level 2 remains uncapped;
- generic --level 2 remains production behavior;
- current deterministic production delivery can exceed four active Flippers;
- natural Level 2 -> Level 3 progression remains unchanged.


2. DIAGNOSTIC STARTUP

- the option selects displayed Level 2;
- canonical LevelConfiguration.Level2Square is reused;
- square geometry is unchanged;
- supply remains 12;
- four starting Eggs remain;
- reserve remains 8;
- canflip remains enabled;
- only the diagnostic hatch cap differs.


3. FOUR-FLIPPER CAP

- diagnostic active Flippers never exceed four;
- the fifth ready Egg waits;
- no transient fifth Flipper appears;
- the waiting Egg remains ReadyToHatch and unresolved;
- the waiting Egg does not disappear or return to reserve;
- a freed slot allows normal hatching.


4. ACCOUNTING

At meaningful boundaries prove exact conservation among:

- reserve originals;
- Eggs in all states;
- active Flippers;
- destroyed outcomes;
- rim-cleared outcomes;
- any other legitimate existing terminal state.

All 12 original obligations must be represented exactly once.


5. DETERMINISM / RANDOMNESS

- blocked ready Eggs consume no repeated hatch randomness;
- identical seed/input produces identical waiting/hatching behavior;
- accepted canflip randomness and direction latching remain deterministic;
- production Level 2 remains unchanged.


6. RELEASE / APPROACH ISOLATION

- four starting Eggs remain;
- reserve release remains 50 ticks;
- approach selection remains unchanged;
- maximum approaching/ready policy remains unchanged;
- the experiment gates only hatch eligibility;
- no global six-active rule exists.


7. CANFLIP PRESERVATION

- climb behavior remains unchanged;
- pause remains unchanged;
- pivot direction/destination remain latched;
- endpoint-hinge geometry remains unchanged;
- collision behavior remains unchanged;
- shot-delay behavior during pivots remains unchanged.


8. FIRING / BULLETS

- Level 2 firing rules remain unchanged;
- two-shot capacity remains unchanged;
- holdoff remains unchanged;
- bullets remain visible;
- bullet simulation/rendering/collision use the same state;
- the diagnostic does not require --enemy-bullets-test.


9. RIM BEHAVIOR

- accepted rim-speed calibration remains unchanged;
- rim pause remains unchanged;
- targeting/direction remains unchanged;
- four active Flippers may still produce simultaneous rim pressure.


10. DEATH / RE-ENTRY

- unresolved supply is conserved through diagnostic death;
- the cap remains active;
- repeated deaths do not duplicate or lose obligations;
- completion remains reachable.


11. SUPER ZAPPER

- accepted Zapper lifecycle remains unchanged;
- delayed targets retain cap slots until actual removal;
- removal can free a slot;
- waiting Eggs resume;
- active count never exceeds four.


12. COMPLETION / REPLAY

- all 12 obligations can resolve;
- no false completion occurs;
- no waiting-Egg deadlock occurs;
- diagnostic reaches its terminal direct-scenario boundary;
- replay/restart preserves the diagnostic cap.


13. LEVEL 1 REGRESSION

- accepted production Level 1 remains capped at four;
- Level 1 supply remains ten;
- Level 1 progression remains normal;
- enemy bullets remain visible;
- no Level 1 behavior changes.


14. LEVELS 3-8 REGRESSION

- no Level 2 diagnostic cap affects Levels 3-8;
- configurations remain unchanged;
- Level 8 open topology and spinner direction remain unchanged.


15. CLI

Test:

- diagnostic alone;
- plus --lives;
- plus --speed;
- plus --spinner-sensitivity;
- accepted --rim-speed composition if applicable;
- duplicate diagnostic;
- conflicts with other scenario selectors;
- recording rejection;
- visible playback rejection;
- headless playback rejection;
- input diagnostic conflict.


BOUNDED TEST REQUIREMENT

Every new simulation/test loop must have an explicit:

- maximum tick count;
- iteration count;
- or test timeout.

Every bound failure must report a useful state snapshot such as:

- current tick/boundary;
- active Flipper count;
- reserve count;
- Egg count by state;
- unresolved count;
- gameplay state.

Do not write:

while condition not reached

without a hard bound.

Do not let a stale expectation hang the test suite.


BASELINE / DIAGNOSTIC COMPARISON TEST

Add a bounded test under identical accepted C# scheduling conditions proving:

- production Level 2 reaches five active Flippers;
- diagnostic remains at four;
- the fifth diagnostic Egg waits;
- state matches before the cap becomes relevant where appropriate;
- supply remains 12 in both;
- release/approach configuration remains identical;
- movement/firing/rim settings remain identical.

This is C# experiment isolation evidence, not an arcade test.


VALIDATION

Run:

1. Narrow CLI and startup tests.

2. Narrow Level 2 cap/wait/accounting tests.

3. Level2SquareTests.

4. Level2CanFlipTests.

5. Enemy-firing/bullet visibility tests.

6. Death/re-entry and Super Zapper tests.

7. Completion/replay tests.

8. Level 1 production cap regressions.

9. Levels 3-8 configuration/topology regressions.

10. Recording/playback rejection and production recording regressions.

11. Expanded affected regression set.

12. Full solution:

dotnet test TempestCSharp.slnx --blame-hang-timeout 60s

13. Full build:

dotnet build TempestCSharp.slnx

14. Whitespace:

git diff --check

Do not launch the interactive game.

Neal will perform manual Level 2 A/B acceptance.


WINDOWS SECURITY FAIL-SAFE

Windows Application Control / Smart App Control has intermittently blocked
newly generated unsigned DLLs and EXEs, including the investigation's temporary:

Level2ConcurrencyProbe.dll

Do not weaken, disable, bypass, or change Windows security.

Do not ask Neal to weaken security.

Do not repeatedly regenerate or retry the same blocked artifact.

Prefer:

- existing repository source;
- existing Tempest.Game.Tests assembly;
- established test seams;
- established headless APIs;
- bounded source analysis.

Avoid creating disposable standalone probe projects or new temporary executable
assemblies merely for measurement.

If Windows security blocks a required build/test artifact:

1. Record the exact command.
2. Record the exact artifact/path.
3. Record the complete error or relevant policy message.
4. Determine whether an existing already-built/current test seam can
   legitimately complete the validation.
5. Retry only when a materially different safe route exists.
6. Never report the blocked command as passed.
7. Never substitute stale binary results for the current candidate.
8. If required validation cannot be completed, STOP VOLUNTARILY.
9. Preserve the worktree.
10. Report:
    - completed work;
    - blocked work;
    - remaining validation;
    - exact handoff Neal needs.

Do not wait indefinitely for Neal to discover the block and force-stop the
agent.


STALL / NO-PROGRESS FAIL-SAFE

The agent must not allow a command or analysis loop to run for hours without
meaningful progress.

Use explicit:

- test timeouts;
- VSTest --blame-hang-timeout;
- tick/iteration bounds;
- process time bounds;
- clear stop conditions.

If a test or command makes no meaningful progress beyond its reasonable
expected duration:

1. Stop the operation.
2. Do not immediately repeat the same unrestricted command.
3. Inspect the process/result.
4. Isolate using progressively smaller bounded filters.
5. Identify the exact hanging test or operation if practical.
6. Distinguish:
   - stale test assumption;
   - unbounded helper loop;
   - production deadlock;
   - Windows security block;
   - infrastructure/tool failure.
7. Preserve all candidate files.
8. If safe bounded recovery is not possible, STOP VOLUNTARILY.
9. Provide Neal a concise blocker report and recovery recommendation.

A stopped, canceled, timed-out, hung, or blocked command is not a pass.


SELF-STOP REQUIREMENT

If any required step cannot be completed safely:

- stop before staging or claiming completion;
- do not keep running indefinitely;
- do not conceal partial validation;
- do not improvise a security bypass;
- preserve the current candidate;
- create the implementation report with status BLOCKED or PARTIAL;
- clearly state why the task could not finish;
- provide exact next steps for Neal.

It is preferable to halt with a precise explanation than to require Neal to
force-stop an unresponsive agent.


DOCUMENTATION

Update:

README.md
TEMPEST-DEVELOPMENT-NOTES.md
TEMPEST-BEHAVIOR-SPEC.md

Document:

- development-only Level 2 diagnostic name;
- canonical production Level 2 configuration is reused;
- total supply remains 12;
- maximum four active Flippers in the diagnostic;
- value four is ROM-backed;
- C# waiting-ready-Egg mechanism is a project translation;
- production Level 2 remains unchanged;
- Level 1 remains frozen;
- no global max_enm gate is implemented;
- the ROM's max_enm comparison is inclusive;
- canflip, firing, bullets, rim speed, collisions, release timing, and Egg
  approach timing remain unchanged;
- recording/playback policy;
- manual A/B acceptance pending;
- no conclusion yet that the cap belongs in production.


EVIDENCE CLASSIFICATION

Use:

ROM-LISTING-BACKED

REV-2-BINARY-CONFIRMED

CURRENT ACCEPTED C# BEHAVIOR

DEVELOPMENT-ONLY CONTROLLED EXPERIMENT

PROJECT TRANSLATION

AUTOMATED TEST EVIDENCE

MANUAL LEVEL 2 ACCEPTANCE PENDING

Do not call the diagnostic arcade-accurate before Neal tests it.


PROTECTED FILES

Do not modify, stage, or commit:

Reference/arcade/roms.rev2/
UserData/
screenshots
gameplay recordings
Videos/
TEMP/
Temp/
historical AI reports
unrelated local files
test dumps/sequences
build output.

Do not run git clean.

Do not read TEMP/myNotes.md or other TEMP files as evidence.


AI-LOG POLICY

1. Save this complete substantive prompt under:

AI-Logs/Prompts/

2. Create a NEW implementation report under:

AI-Logs/Reports/

3. Existing reports remain unchanged historical records.

4. AI-Logs is Git-ignored.

5. Do not stage or commit AI-Logs.

6. Do not place reports under TEMP/ or Temp/.


IMPLEMENTATION REPORT

Create a descriptive date-prefixed report under AI-Logs/Reports/.

Include:

1. Starting Git state.
2. Accepted baseline.
3. Investigation report reused.
4. ROM policy being isolated.
5. Diagnostic CLI and scenario identity.
6. Production Level 2 preservation.
7. Level 1 preservation.
8. Active-count semantics.
9. Waiting-Egg implementation.
10. Determinism/randomness behavior.
11. Release/approach preservation.
12. Canflip preservation.
13. Firing/bullet preservation.
14. Rim behavior preservation.
15. Death/re-entry.
16. Super Zapper.
17. Completion/replay.
18. Recording/playback policy.
19. Files changed.
20. Tests added/updated.
21. Bounded baseline/diagnostic comparison.
22. Focused validation.
23. Expanded validation.
24. Full-suite result.
25. Build result.
26. git diff --check.
27. Windows security status.
28. Any stalls/timeouts and bounded recovery.
29. Protected files unchanged.
30. Final Git state.
31. Manual commands.
32. Manual A/B checklist.
33. Known limitations.
34. Manual acceptance status.
35. Any blocker or incomplete validation.


GIT / STOP POINT

Do not stage.

Do not commit.

Do not push.

Leave the diagnostic candidate unstaged for Neal's manual A/B comparison.

Do not promote the cap into production.

Do not implement the global active gate.

Do not begin Level 9.


MANUAL COMMANDS

Provide exact commands supported by the finished implementation.

Production Level 2:

dotnet run --project src\Tempest.Game -- --level 2 --lives 10 --spinner-sensitivity 1

Capped Level 2 diagnostic:

dotnet run --project src\Tempest.Game -- --level2-four-flipper-cap-test --lives 10 --spinner-sensitivity 1

Do not provide unsupported recording commands.


MANUAL A/B CHECKLIST

Ask Neal to use the same:

- physical spinner;
- spinner sensitivity 1.0;
- lives setting;
- display/window configuration;
- firing strategy;
- general Super Zapper strategy;

for both runs.

Observe:

1. Number of simultaneously visible Flippers.
2. Whether a fifth Flipper appears while four remain active.
3. Number of simultaneous below-rim canflip pivots.
4. Lane clustering.
5. Multi-direction threat convergence.
6. Number of Flippers approaching the rim together.
7. Number of simultaneous rim pursuers.
8. Enemy-bullet pressure.
9. Recovery/breathing room.
10. Ability to track targets deliberately.
11. Whether the diagnostic feels too sparse.
12. Whether stationary defense becomes too easy.
13. Whether rim pursuit remains dangerous.
14. Whether queued Eggs continue hatching as slots open.
15. Whether all 12 obligations eventually appear.
16. Whether the wave length remains substantial.
17. Death/re-entry coherence.
18. Super Zapper follow-up pressure.
19. Enemy bullet visibility and collision.
20. Wave completion and replay.
21. Similarity to original arcade Level 2.

Do not judge primarily by exact death count.

The central acceptance question is whether threat density, spacing, canflip
pressure, and rim convergence feel closer to the arcade.


MANUAL ACCEPTANCE STATUS

The diagnostic remains:

MANUAL LEVEL 2 ACCEPTANCE PENDING

Do not claim production Level 2 should change.

Stop after automated validation and manual-test handoff.


BEFORE FINAL RESPONSE

Re-read this checklist.

Confirm every applicable item is complete or explicitly reported blocked:

[ ] Required diagnostic implementation complete
[ ] Production Level 2 unchanged
[ ] Production Level 1 unchanged
[ ] Levels 3-8 unchanged
[ ] Cap/wait tests complete
[ ] Canflip tests complete
[ ] Firing/bullet tests complete
[ ] Death/Zapper/completion tests complete
[ ] CLI tests complete
[ ] Recording policy tested
[ ] All loops bounded
[ ] Expanded validation complete
[ ] Full suite complete with hang timeout
[ ] Build complete
[ ] git diff --check complete
[ ] Durable docs updated
[ ] Prompt saved
[ ] New report saved in AI-Logs/Reports
[ ] Protected files checked
[ ] Git state checked
[ ] Interactive game NOT launched
[ ] Nothing staged, committed, or pushed
[ ] Manual test commands provided
[ ] Manual acceptance still marked pending

Do not announce that the report or prompt follows unless the promised content
has actually been created.

Do not provide a success response while an applicable item is silently
unfinished.

TEMPEST_LEVEL2_FOUR_FLIPPER_CAP_DIAGNOSTIC_20261109