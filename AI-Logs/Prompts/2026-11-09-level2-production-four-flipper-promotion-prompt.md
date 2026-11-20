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

Promote the manually validated Level 2 maximum-four-concurrent-Flippers rule
from the current development-only diagnostic into canonical production Level 2.

Repository:

C:\Users\nrnob\source\repos\Tempest

Committed baseline beneath the current unstaged Level 2 diagnostic candidate:

f362af7f87c23a9b983937f8733f5a4ac71d0162
Implement accepted Level 1 four-Flipper maximum

The working tree currently contains the unstaged Level 2 four-Flipper-cap
diagnostic candidate.

Preserve the correct candidate work.

Do not restore or restart implementation from the committed baseline.

Do not stage, commit, or push.

Leave the completed production Level 2 candidate unstaged for Neal's final
normal-production manual acceptance.

Do not begin Level 3 concurrency work.

Do not implement the ROM global active-enemy gate.

Do not begin Level 9.


REQUIRED REPORTS

Read fully:

AI-Logs/Reports/2026-11-09-level2-concurrency-investigation-report.md

AI-Logs/Reports/2026-11-09-level2-four-flipper-cap-diagnostic-report.md

AI-Logs/Reports/2026-11-09-level2-bottom-artifact-investigation-report.md

Also read as needed:

AI-Logs/Reports/2026-11-09-level1-production-four-flipper-correction-report.md

AI-Logs/Reports/2026-11-09-level1-four-flipper-production-closeout-report.md

Inspect:

- the complete current unstaged diagnostic candidate;
- production Level 1 cap implementation;
- canonical Level 2 configuration;
- Level 2 square/canflip implementation;
- Egg release/approach/readiness/hatch lifecycle;
- Flipper firing;
- enemy bullets and renderer;
- death/re-entry;
- Super Zapper;
- completion/progression;
- direct-level startup;
- recording/playback;
- CLI scenario selection;
- existing Level 2 diagnostic tests;
- README.md;
- TEMPEST-DEVELOPMENT-NOTES.md;
- TEMPEST-BEHAVIOR-SPEC.md.

Existing reports are historical records.

Do not rewrite them.


MANUAL LEVEL 2 A/B ACCEPTANCE FROM NEAL

Neal manually compared normal production Level 2 with the development-only
four-Flipper-cap diagnostic using his physical arcade-style spinner.

The Level 2 four-Flipper diagnostic passed strongly.

Neal reports that the capped version feels much closer to the original arcade
Level 2.

He manually verified:

1. Fewer Flippers occupy the tube simultaneously.
2. Below-rim canflip activity is easier to follow.
3. There are fewer simultaneous pivots and converging threats.
4. Fewer Flippers arrive at the rim together.
5. The player has more useful breathing room between dangerous situations.
6. Enemy bullets remain normal and visible.
7. Rim pursuit remains dangerous.
8. The level still feels like Tempest rather than artificially sparse.
9. Queued Eggs continue entering as active slots open.
10. The full 12-enemy wave remains substantial.

Treat this as strong manual acceptance of the Level 2 four-Flipper concurrency
policy.

Final normal-production Level 2 acceptance remains pending until this promoted
candidate is manually tested.


ROM-BACKED PRODUCTION RULE

Production Level 2 must now enforce:

Original supply:
12 Flippers

Maximum concurrently active Flippers:
4

The value four is:

ROM-LISTING-BACKED / REV-2-BINARY-CONFIRMED

The Atari Level 2 maximum is four across Easy, Medium, and Hard according to the
completed investigation.

The C# mechanism of keeping additional ready Eggs waiting is:

PROJECT TRANSLATION / MANUALLY ACCEPTED RESULT

Do not claim that the C# waiting-Egg data structure or timing is a literal Atari
pending-entry implementation.


GLOBAL ACTIVE-ENEMY ROM GATE

Do not implement the broader ROM max_enm mechanism in this milestone.

The investigation established:

- the ROM table value is six;
- its actual comparison is inclusive rather than a strict six-object ceiling;
- Level 2 is Flipper-only;
- the four-Flipper species maximum is stricter;
- therefore the global mechanism does not independently constrain Level 2.

Do not add:

- a six-active limit;
- a seven-active limit;
- a generalized enemy-cap system;
- another concurrency abstraction solely for the ROM global gate.

That remains separate future work.


PRODUCTION ENTRY PATHS

The production four-Flipper maximum must apply to canonical production Level 2
through all normal production paths, including:

- natural Level 1 -> Level 2 progression;
- generic --level 2 direct production startup;
- normal Level 2 death/re-entry;
- production gameplay recording startup at Level 2;
- visible production playback;
- headless production playback;
- deterministic production tests;
- any existing campaign path using canonical LevelConfiguration.Level2Square.

Do not apply the production rule only to a development scenario.


PRODUCTION LEVEL 1 IS FROZEN

Accepted production Level 1 already has its ROM-backed maximum of four
concurrently active Flippers.

Do not change:

- Level 1 supply 10;
- Level 1 four-active maximum;
- waiting-Egg behavior;
- release timing;
- bullet rendering;
- rim behavior;
- progression;
- recording/playback;
- tests except for behavior-neutral shared refactoring coverage.

If the Level 1 implementation is generalized internally, prove through tests
that its existing behavior is unchanged.


LEVELS 3-8 ARE FROZEN

Do not introduce this Level 2 policy into Levels 3-8.

Preserve their existing:

- configurations;
- species policies;
- active limits;
- geometry;
- movement;
- Tankers/cargo;
- Spikers;
- Pulsars;
- Fuseballs;
- firing;
- exits;
- topology;
- progression;
- recording/playback.

Future concurrency corrections for Levels 3-8 require their own evidence and
manual acceptance.


PRODUCTION CAP SEMANTICS

For canonical production Level 2:

- Count active Flippers using the authoritative active Flipper collection.
- A Flipper begins consuming one cap slot when ordinary hatching adds it to the
  active collection.
- A Flipper continues consuming a cap slot while:
  - climbing;
  - pausing;
  - canflip pivoting;
  - at the rim;
  - pursuing along the rim;
  - waiting for delayed Super Zapper resolution.
- A Flipper stops consuming capacity only when the existing lifecycle actually
  removes it.

At four active Flippers:

- another ready Level 2 Egg cannot hatch;
- the Egg remains ReadyToHatch;
- the Egg remains unresolved;
- it is not destroyed;
- it is not returned to reserve;
- it is not rim-cleared;
- it is not scored;
- it is not replaced;
- it does not repeatedly reroll;
- it does not repeatedly consume hatch randomness.

When capacity becomes available:

- an eligible waiting Egg may hatch through the normal hatch path;
- existing ordering semantics apply;
- no update may transiently create a fifth active Flipper.

All 12 original Level 2 obligations must eventually resolve.


IMPLEMENTATION ARCHITECTURE

Promote the validated behavior into production using the smallest maintainable
architecture.

Level 1 and Level 2 now both have an accepted production maximum of four active
Flippers.

Do not leave two arbitrary duplicated blocks if a narrowly scoped shared
mechanism clearly improves maintainability.

However:

- do not over-generalize this into a global species-cap framework for future
  levels;
- do not add unrelated per-species infrastructure;
- do not introduce the ROM global gate;
- do not alter later-level behavior.

A small production helper or explicit accepted-Level predicate is preferable to
a broad architecture redesign.

Use the authoritative active collection.

Do not add a cached counter that can drift from real state.


DIAGNOSTIC OPTION DISPOSITION

The temporary option:

--level2-four-flipper-cap-test

has served its purpose.

Preferred production-promotion outcome:

- remove the temporary diagnostic option;
- remove its development scenario identity;
- remove parser/conflict wiring specific to the diagnostic;
- remove terminal replay behavior specific to that diagnostic;
- remove its README command;
- remove diagnostic-only recording/playback rejection that no longer serves a
  purpose;
- convert useful Level2FourFlipperCapTests into production Level 2 fidelity
  tests;
- preserve useful rendering/firing/canflip/accounting regressions.

Do not retain a redundant option whose behavior now matches production Level 2.

Do not leave dead flags or misleading documentation.


WAITING-EGG BOTTOM PRESENTATION

The bottom-of-tube artifact investigation established that the tiny deep-end
object Neal observed is a real cap-blocked ReadyToHatch Egg.

Established behavior:

- it remains in CurrentLevel.Eggs;
- it remains unresolved;
- it is at height zero;
- DrawEggs renders all Egg lifecycle states;
- height zero projects to the far/deep web end;
- the diagnostic cap prolongs that normally transient state across rendered
  frames;
- the object is not a Flipper;
- not an enemy bullet;
- not a player shot;
- not an explosion;
- not duplicate Egg/Flipper representation;
- not stale renderer state;
- not a collision object;
- not an accounting defect.

The investigation concluded:

EXPECTED/HARMLESS — NO FIX

Preserve that decision.

Do not:

- hide the waiting Egg;
- move it;
- delete it;
- prematurely hatch it;
- change its height merely for presentation;
- add a special Level 2 rendering exception.

Neal accepts this as a trivial harmless project presentation artifact for now.


FUTURE-LEVEL PRESENTATION CONCERN

Document the deferred concern clearly:

If visible cap-blocked ReadyToHatch Eggs at the far/deep tube end begin appearing
broadly or repeatedly across future production levels, revisit the presentation
because Neal considers that recurring visible behavior inconsistent with the
arcade game.

The current Level 2 occurrence is not a production blocker.

Do not attempt that broader presentation redesign now.


SUPPLY AND EGG TIMING

Preserve canonical Level 2:

- total supply 12;
- four starting Eggs;
- eight reserve originals;
- 50-active-tick reserve release;
- maximum Egg queue 4;
- maximum approaching/ready Eggs 2;
- approach-selection interval 50 active ticks;
- existing Egg update cadence;
- existing deterministic spawn-lane sequence;
- existing readiness timing.

The only production gameplay change is hatch eligibility at four active
Flippers.

Do not implement Atari pending-entry timing.

Do not retime releases.

Do not suppress reserve release because ready Eggs are waiting.


CANFLIP

Preserve every accepted Level 2 canflip mechanic:

- direction selection;
- direction latching;
- destination latching;
- square endpoint-hinge geometry;
- directed pivot sweeps;
- turn timing;
- lane pause;
- below-rim climb;
- rim clamp;
- shot ownership;
- completed-pivot lethality;
- player collision;
- firing suppression during active pivot;
- shot-delay advancement while pivoting;
- ready-shot behavior after pivot completion;
- randomness semantics.

The concurrency change affects how many Flippers exist simultaneously, not how
an individual Flipper moves.


ENEMY FIRING AND BULLETS

Preserve accepted production Level 2:

- Flipper firing;
- two concurrent enemy-shot capacity;
- 77-tick C# holdoff;
- pressure thresholds;
- climb/pause firing eligibility;
- pivot firing suppression;
- bullet spawn;
- bullet speed;
- bullet lifetime;
- bullet visibility;
- bullet geometry;
- bullet sound;
- bullet collision/damage.

Production Level 2 bullets must remain visible without requiring:

--enemy-bullets-test

Do not alter firing to compensate for reduced active Flipper count.


RIM BEHAVIOR

Preserve:

- accepted 2.5 rim-speed calibration;
- rim pause;
- targeting;
- persistent direction behavior;
- pivot geometry;
- player contact rules;
- player-shot ordering.

The manual A/B comparison improved arcade similarity without changing rim
behavior.

Do not retune it during promotion.


DEATH / RE-ENTRY

Production Level 2 must retain valid reconstruction under the new cap.

Verify:

- unresolved obligations remain conserved;
- the four-Flipper production cap remains enforced after death;
- active Flippers are reconstructed according to existing Level 2 policy;
- canflip remains enabled;
- repeated deaths cannot produce five concurrent Flippers;
- waiting Eggs are not lost or duplicated;
- score/lives remain correct;
- firing remains correct;
- progression remains reachable.


SUPER ZAPPER

Preserve accepted Level 2 Super Zapper behavior.

A delayed Zapper target remains active and occupies a cap slot until normal
removal.

After the target is actually removed:

- capacity becomes available;
- waiting Eggs may hatch according to existing phase ordering;
- active Flippers remain <= 4;
- supply remains conserved.

Do not change charges, scoring, sound, or targeting.


COMPLETION AND NATURAL PROGRESSION

Production Level 2 must remain fully completable.

Verify:

- all 12 obligations eventually resolve;
- waiting Eggs prevent false early completion;
- no capacity deadlock exists once a slot is free;
- successful Level 2 completion uses existing exit behavior;
- natural Level 2 -> Level 3 progression remains correct;
- score/lives carry;
- Super Zapper recharge remains correct;
- incoming Level 3 configuration remains unchanged.

Generic --level 2 must follow its existing accepted production direct-start
policy.

Do not convert production Level 2 into the diagnostic's terminal behavior.


RANDOMNESS

Preserve deterministic semantics:

- waiting at the cap consumes no repeated hatch randomness;
- a blocked Egg does not reroll;
- later-hatched Flippers use the ordinary canflip/random paths;
- identical seed/input is deterministic;
- production Level 1 random behavior is unchanged;
- Levels 3-8 random behavior is unchanged except for no behavior-neutral
  difference.

Do not globally reorder random draws unnecessarily.


RECORDING / PLAYBACK

Now that the cap becomes ordinary production Level 2 behavior:

- do not add scenario metadata;
- do not add a cap flag to recordings;
- do not change recording version;
- do not change recording schema.

Production recordings starting at Level 2 naturally use the new four-Flipper
production rule.

Verify:

- live production recording;
- visible production playback;
- headless playback;
- pause/resume;
- forward stepping;
- reverse reconstruction;
- screenshots;
- live-input handoff.

Historical recordings replay under the production simulation implemented by the
current version.

Do not claim exact old-trajectory compatibility unless tests establish it.

A fresh Level 2 production recording should be used for final manual validation.


TEST-HANG SAFETY

Promotion will invalidate any test/helper that assumes every Level 2 Egg can
hatch while active Flippers remain occupied.

Audit tests for patterns such as:

- wait until Eggs empty;
- wait until all 12 active simultaneously;
- wait until fifth Flipper appears;
- unbounded while loop waiting for hatching;
- direct calls that bypass the production hatch gate but are incorrectly used
  as production evidence.

Any state-waiting loop must have an explicit tick/iteration bound.

Any bound failure must report useful state including as appropriate:

- tick/boundary;
- gameplay state;
- active Flippers;
- reserve count;
- Eggs by state;
- unresolved count.

Do not allow the suite to hang indefinitely.


REQUIRED TEST CONVERSION

Review current Level2FourFlipperCapTests.

Classify tests into:

1. Production Level 2 fidelity tests to retain/rename.
2. Temporary diagnostic CLI tests to remove.
3. Temporary terminal/replay scenario tests to remove or convert.
4. Useful production bullet/canflip/accounting tests to retain.
5. Redundant tests removable only if stronger coverage remains.

Do not leave production tests referring to a removed diagnostic as the
behavioral authority.


REQUIRED PRODUCTION TESTS

Add/convert tests proving:


1. PRODUCTION STARTUP

- no-option natural Level 2 uses the four-Flipper maximum;
- generic --level 2 uses the same production maximum;
- canonical LevelConfiguration.Level2Square remains in use;
- total supply is 12;
- four starting Eggs;
- reserve 8;
- accepted geometry/canflip remain unchanged.


2. FOUR-FLIPPER MAXIMUM

- production Level 2 never exceeds four active Flippers;
- the fifth ready Egg waits;
- no transient fifth Flipper appears;
- the Egg remains ReadyToHatch and unresolved;
- after a slot opens, normal hatching resumes.


3. ACCOUNTING CONSERVATION

At meaningful boundaries prove exact representation of all 12 obligations
among:

- reserve;
- Eggs in all lifecycle states;
- active Flippers;
- destroyed outcomes;
- rim-cleared outcomes;
- other legitimate existing terminal states.

No duplication or loss.


4. RELEASE / APPROACH ISOLATION

- four starting Eggs unchanged;
- 50-tick reserve release unchanged;
- approach cadence unchanged;
- queue/approach limits unchanged;
- only hatch eligibility differs;
- global max_enm mechanism remains absent.


5. CANFLIP

- movement/pivot behavior unchanged;
- RNG behavior unchanged when hatching occurs;
- direction and destination latching preserved;
- collisions preserved.


6. FIRING / BULLETS

- firing rules unchanged;
- two-shot capacity unchanged;
- holdoff unchanged;
- bullets visible;
- bullets simulated/collided from same active state;
- no optional bullet flag needed.


7. RIM PURSUIT

- 2.5 calibration unchanged;
- rim pause unchanged;
- targeting unchanged;
- direct production Level 2 remains threatening.


8. DEATH / RE-ENTRY

- unresolved accounting conserved;
- four-Flipper cap remains active;
- repeated deaths cannot exceed four;
- wave remains completable.


9. SUPER ZAPPER

- delayed target consumes cap until removal;
- removal frees slot normally;
- waiting Egg resumes;
- no update exceeds four.


10. COMPLETION / PROGRESSION

- all 12 obligations resolve;
- no false completion;
- no deadlock;
- natural Level 2 -> Level 3 works.


11. LEVEL 1 REGRESSION

- production Level 1 still uses accepted four-Flipper maximum;
- Level 1 supply remains 10;
- bullet visibility remains accepted;
- Level 1 -> 2 progression still works.


12. LEVELS 3-8 REGRESSION

- configurations remain unchanged;
- no Level 2 cap leaks into later levels;
- Level 8 open topology/spinner semantics remain unchanged.


13. DIAGNOSTIC REMOVAL

If --level2-four-flipper-cap-test is removed:

- parser rejects it;
- README no longer advertises it;
- no obsolete scenario identity remains;
- no obsolete recording restriction remains;
- no dead restart/replay branch remains.


14. RECORDING / PLAYBACK

- production Level 2 recording works;
- visible playback works;
- headless playback works;
- pause/resume works;
- forward/reverse works;
- no cap metadata exists;
- fresh recording reproduces current production simulation.


BASELINE-VERSUS-NEW-PRODUCTION TEST

Retain bounded automated evidence documenting the intentional production change:

Prior production Level 2 could reach five active Flippers at boundary 289 under
the established deterministic delivery schedule and all 12 by boundary 615.

New production Level 2 remains at four while subsequent ready Eggs wait.

Do not require checkout/execution of the historical commit.

Use the historical result only as characterization while asserting current
production behavior.

Do not make the test unnecessarily brittle if exact boundary 289 is not
essential.


BOTTOM WAITING-EGG REGRESSION

Retain or add a narrow test establishing the investigated deep-end waiting Egg
behavior without treating it as a defect:

- cap-blocked ready Egg remains a real Egg;
- Height remains zero while ReadyToHatch if that is existing semantics;
- it remains unresolved;
- it does not collide/fire as a Flipper;
- no partial Flipper exists;
- no duplicate representation exists;
- hatch or death reconstruction removes/replaces it correctly.

Do not assert that the deep-end mark is arcade-equivalent.

This test exists to prevent future developers from "fixing" the presentation by
breaking accounting.


VALIDATION

Run narrow tests first.

Then run, with explicit timeouts/bounds:

1. Production Level 2 cap/accounting tests.
2. Level2SquareTests.
3. Level2CanFlipTests.
4. Enemy firing/bullet tests.
5. Death/re-entry tests.
6. Super Zapper tests.
7. Completion and Level 2-to-3 progression.
8. Production Level 1 regressions.
9. Levels 3-8 regressions.
10. Recording/playback/pause/forward/reverse tests.
11. CLI cleanup tests.
12. SimulationTests with VSTest hang timeout.
13. Expanded affected regressions.
14. Full solution:

dotnet test TempestCSharp.slnx --blame-hang-timeout 60s

15. Full build:

dotnet build TempestCSharp.slnx

16. Whitespace:

git diff --check

Do not launch the interactive game.

Neal will perform final production Level 2 manual acceptance.


WINDOWS SECURITY FAIL-SAFE

Follow the project's established Windows-security self-stop policy.

If Application Control / Smart App Control blocks a DLL/EXE:

- do not weaken or bypass security;
- do not repeat the same blocked artifact in a retry loop;
- do not substitute stale binary results;
- retry only through a materially different safe route;
- record exact command, path, artifact, and error;
- a blocked command is not a pass.

If required validation cannot safely complete:

STOP VOLUNTARILY.

Preserve the worktree and report:

- completed work;
- blocked work;
- remaining validation;
- exact handoff Neal needs.


STALL / NO-PROGRESS FAIL-SAFE

All potentially long tests, loops, and analyses must have explicit:

- VSTest hang timeouts;
- tick bounds;
- iteration bounds;
- command/process bounds;
- useful failure snapshots.

If meaningful progress stops:

- stop the operation;
- do not rerun the same unrestricted command;
- isolate with smaller bounded runs;
- identify stale test assumption, unbounded helper, production deadlock,
  security block, or infrastructure failure when practical.

STOP VOLUNTARILY if safe bounded recovery cannot continue.

Do not wait hours for Neal to force-stop the agent.

Canceled, timed-out, hung, stalled, or blocked commands are not passes.


SELF-STOP REQUIREMENT

If any required implementation or validation step cannot be completed safely:

- stop before staging or claiming success;
- preserve the candidate;
- mark the implementation report BLOCKED or PARTIAL;
- state exactly what completed;
- state what remains incomplete;
- state the exact blocker;
- state Neal's next action.

A precise voluntary handoff is preferable to an unresponsive session.


DOCUMENTATION

Update:

README.md

TEMPEST-DEVELOPMENT-NOTES.md

TEMPEST-BEHAVIOR-SPEC.md

Record:

- production Level 2 maximum four active Flippers;
- ROM-backed value four;
- C# waiting Egg as project translation;
- total supply remains 12;
- accepted Egg schedule remains unchanged;
- canflip remains unchanged;
- firing/bullets remain unchanged;
- rim speed remains unchanged;
- six/global gate remains unimplemented;
- temporary diagnostic removed;
- manual A/B diagnostic passed strongly;
- deep-end waiting Egg investigated as expected/harmless project presentation;
- future-level recurring waiting-Egg display concern remains deferred;
- production Level 2 final manual acceptance pending.

Do not claim final production acceptance before Neal retests normal Level 2.


EVIDENCE CLASSIFICATION

Use:

ROM-LISTING-BACKED

REV-2-BINARY-CONFIRMED

PROJECT TRANSLATION

DIAGNOSTIC MANUALLY ARCADE-FIDELITY SUPPORTED

CURRENT PRODUCTION CANDIDATE

EXPECTED/HARMLESS PROJECT PRESENTATION

FINAL PRODUCTION MANUAL ACCEPTANCE PENDING

DEFERRED

Do not state that the C# waiting-Egg rendering is arcade behavior.


PROTECTED FILES

Do not modify, stage, commit, move, or delete:

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

Do not read TEMP/myNotes.md as evidence.


AI-LOG POLICY

1. Save this complete prompt under:

AI-Logs/Prompts/

2. Create a NEW production Level 2 implementation report under:

AI-Logs/Reports/

3. Existing reports remain historical records.

4. Do not rewrite existing reports.

5. AI-Logs is Git-ignored.

6. Do not stage or commit AI-Logs.

7. Do not place reports in TEMP/ or Temp/.


IMPLEMENTATION REPORT

Create a descriptive date-prefixed report under AI-Logs/Reports/.

Include:

1. Starting Git state.
2. Committed baseline.
3. Existing diagnostic candidate inherited.
4. Reports reviewed.
5. Neal's manual Level 2 A/B acceptance.
6. Bottom artifact investigation result.
7. Production policy adopted.
8. Diagnostic option disposition.
9. Exact cap semantics.
10. Waiting-Egg behavior.
11. Future-level presentation concern.
12. Randomness preservation.
13. Supply/accounting.
14. Release/approach preservation.
15. Canflip preservation.
16. Enemy firing/bullet preservation.
17. Rim behavior preservation.
18. Death/re-entry.
19. Super Zapper.
20. Completion/progression.
21. Recording/playback.
22. Level 1 isolation.
23. Levels 3-8 isolation.
24. Files changed.
25. Tests converted/added/removed.
26. Test-hang audit.
27. Focused validation results.
28. SimulationTests result.
29. Expanded validation results.
30. Full-suite result.
31. Build result.
32. git diff --check.
33. Windows security status.
34. Stall/timeout status.
35. Protected files unchanged.
36. Final Git state.
37. Manual commands.
38. Final production acceptance checklist.
39. Remaining Level 2 unknowns.
40. Deferred broader concurrency work.
41. Blocker/partial status if applicable.


GIT / STOP POINT

Do not stage.

Do not commit.

Do not push.

Leave the complete production Level 2 candidate unstaged for Neal's final
manual acceptance.

Do not begin wider concurrency work.

Do not implement Level 3 caps.

Do not implement global max_enm.

Do not begin Level 9.


MANUAL TEST COMMANDS

Provide exact commands supported by the completed candidate.

At minimum:


Normal production campaign from Level 1:

dotnet run --project src\Tempest.Game -- --lives 10 --spinner-sensitivity 1


Direct production Level 2:

dotnet run --project src\Tempest.Game -- --level 2 --lives 10 --spinner-sensitivity 1


Fresh production Level 2 recording:

dotnet run --project src\Tempest.Game -- --record-gameplay Temp\GameplayRecordings\level2-four-flipper-production.json --level 2 --lives 10 --spinner-sensitivity 1


Visible playback:

dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\level2-four-flipper-production.json


Do not provide the removed diagnostic command if it no longer exists.


FINAL MANUAL ACCEPTANCE CHECKLIST

Ask Neal to verify:

1. Normal production Level 2 now has the same improved threat-density behavior
   as the accepted diagnostic.
2. No more than four Flippers are active simultaneously.
3. Fewer simultaneous canflip pivots remain observable.
4. Grouped/multi-direction pressure remains closer to arcade Level 2.
5. Fewer Flippers arrive at the rim together.
6. Breathing room remains materially improved.
7. Rim pursuit remains dangerous.
8. Stationary defense is not trivially safe.
9. Enemy bullets remain visible and normal.
10. Canflip behavior remains unchanged.
11. Additional Eggs wait and later hatch.
12. All 12 obligations appear and resolve.
13. The wave remains substantial.
14. Super Zapper remains normal.
15. Death/re-entry remains coherent.
16. Level 2 completes normally.
17. Natural Level 2 -> Level 3 progression works.
18. Level 1 remains accepted and unchanged.
19. Level 3 remains behaviorally unchanged on entry.
20. Fresh recording/playback works.
21. Pause/resume/forward/reverse remain correct.
22. No obvious Levels 3-8 regression appears.
23. The tiny deep-end waiting-Egg mark may still appear and is currently
    accepted as harmless.
24. Report if the same waiting-Egg presentation begins appearing broadly on
    later levels.

Do not use exact death count as the fidelity authority.

Judge threat timing, spacing, canflip density, rim convergence, control, and
overall arcade resemblance.


MANUAL ACCEPTANCE STATUS

The production Level 2 candidate remains:

FINAL PRODUCTION MANUAL ACCEPTANCE PENDING

Do not stage/commit/push before Neal reports this pass.


BEFORE FINAL RESPONSE

Confirm every applicable item is complete or explicitly reported BLOCKED/PARTIAL:

[ ] Production Level 2 cap implemented
[ ] Temporary diagnostic removed
[ ] Level 1 unchanged
[ ] Levels 3-8 unchanged
[ ] Global max_enm NOT implemented
[ ] Egg schedule unchanged
[ ] Canflip unchanged
[ ] Firing/bullets unchanged
[ ] Rim behavior unchanged
[ ] Death/re-entry validated
[ ] Super Zapper validated
[ ] Completion/progression validated
[ ] Waiting-Egg presentation preserved
[ ] All state loops bounded
[ ] SimulationTests bounded and passed
[ ] Expanded regressions passed
[ ] Full suite completed with hang timeout
[ ] Build completed
[ ] git diff --check completed
[ ] Security block handled per project policy
[ ] No stalled command treated as passed
[ ] Durable docs updated
[ ] Prompt saved
[ ] New report saved
[ ] Protected files checked
[ ] Nothing staged
[ ] Nothing committed
[ ] Nothing pushed
[ ] Interactive game NOT launched
[ ] Manual commands provided
[ ] Production manual acceptance still pending

Do not announce successful completion while any applicable required item is
silently unfinished.

TEMPEST_LEVEL2_PRODUCTION_FOUR_FLIPPER_PROMOTION_20261109