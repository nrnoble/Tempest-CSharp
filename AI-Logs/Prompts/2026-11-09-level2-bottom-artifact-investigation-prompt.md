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

Perform a tightly bounded investigation into a newly observed visual/lifecycle
artifact in the current unstaged Level 2 four-Flipper-cap diagnostic.

Identify the exact entity, state, lifecycle transition, and rendering path
responsible.

Fix the artifact only if:

- the cause is established;
- the correction is narrow;
- the intended waiting-Egg/cap behavior remains unchanged;
- production Level 2 remains unchanged;
- accepted production Level 1 remains unchanged.

Repository:

C:\Users\nrnob\source\repos\Tempest

Committed baseline beneath the current unstaged diagnostic candidate:

f362af7f87c23a9b983937f8733f5a4ac71d0162
Implement accepted Level 1 four-Flipper maximum

The working tree contains the unstaged Level 2 four-Flipper-cap diagnostic.

Preserve that candidate.

Do not restart the implementation from the committed baseline.

Do not stage, commit, or push.

Do not promote the Level 2 cap into production.

Do not begin Level 9.


REQUIRED REPORTS

Read:

AI-Logs/Reports/2026-11-09-level2-concurrency-investigation-report.md

AI-Logs/Reports/2026-11-09-level2-four-flipper-cap-diagnostic-report.md

Also inspect the relevant accepted Level 1 cap implementation and historical
reports only as needed to compare waiting-Egg behavior.

Existing reports are historical records.

Do not rewrite them.


MANUAL LEVEL 2 A/B RESULT

Neal manually compared:

Production Level 2:

dotnet run --project src\Tempest.Game -- --level 2 --lives 10 --spinner-sensitivity 1

against the development diagnostic:

dotnet run --project src\Tempest.Game -- --level2-four-flipper-cap-test --lives 10 --spinner-sensitivity 1

The four-Flipper-cap diagnostic passed the primary manual A/B comparison.

Neal verified:

1. Fewer Flippers occupy the tube simultaneously.
2. Below-rim canflip activity is easier to follow.
3. There are fewer simultaneous pivots and converging threats.
4. Fewer Flippers reach the rim together.
5. The player has more useful breathing room.
6. Enemy bullets remain normal and visible.
7. Rim pursuit remains dangerous.
8. The wave still feels like Tempest rather than artificially sparse.
9. Queued Eggs continue entering as active slots open.
10. The full 12-enemy wave remains substantial.

Neal judges the diagnostic much closer to the original arcade Level 2.

Treat this as strong manual acceptance of the concurrency hypothesis and the
four-Flipper gameplay result.

However, final diagnostic acceptance and production promotion remain blocked
pending investigation of the artifact described below.


OBSERVED ARTIFACT

Neal observed a new trivial artifact during Level 2 diagnostic play.

Observed facts:

- An object appears stuck at the bottom/deep end of the tube.
- The object could visually be an enemy, Egg, or shot; its identity is not yet
  known.
- The object may appear at the exact same location each time.
- Neal observed it on three Level 2 diagnostic plays.
- It appeared once per wave in those observations.
- It disappears later.
- It does not interfere with gameplay.
- A normal player would probably never notice it.
- It may be associated with an Egg becoming an enemy, but that is only a
  hypothesis.
- More investigation is required before claiming the cause.

Do not convert Neal's uncertainty into a conclusion.

Do not assume it is a waiting Egg before tracing the actual state.


PRIMARY QUESTIONS

Determine:

1. What exact entity is visible?
2. What collection owns it?
3. What identity, lane, height/depth, and lifecycle state does it have?
4. Why is it rendered at the bottom/deep end of the tube?
5. Is the location deterministic?
6. Why does it appear only once per wave in Neal's observations?
7. What exact event makes it disappear?
8. Is it associated with:
   - a roaming Egg;
   - an approaching Egg;
   - a ReadyToHatch Egg;
   - a cap-blocked ready Egg;
   - a newly hatched Flipper;
   - a below-rim canflip pivot;
   - an enemy bullet;
   - a player shot;
   - an explosion/effect;
   - another presentation object?
9. Does the same state exist in uncapped production Level 2 but normally last
   too briefly to notice?
10. Is the issue purely visual, or does it affect simulation, accounting,
    collision, firing, or progression?
11. Was the behavior introduced by the four-Flipper cap diagnostic?
12. Is a correction necessary before promoting the Level 2 cap into production?


INVESTIGATION FIRST

Do not edit before identifying the entity and root cause.

Trace the current candidate through:

- Egg reserve release;
- roaming movement;
- approach selection;
- approach movement;
- readiness;
- cap-blocked ReadyToHatch behavior;
- ordinary hatching;
- removal of the Egg from the Egg collection;
- creation of the Flipper;
- first Flipper update;
- canflip direction selection;
- rendering of Eggs by state;
- rendering of Flippers by state;
- rendering of enemy bullets;
- rendering of player shots;
- rendering of destruction effects;
- projection near height/depth zero;
- any scenario-specific rendering predicates.

Compare diagnostic Level 2 with ordinary production Level 2.

Do not infer identity from appearance alone.


WAITING-EGG HYPOTHESIS

The diagnostic intentionally permits a ready Egg to remain unresolved while
four Flippers are active.

Investigate whether:

- ReadyToHatch Eggs remain rendered;
- a ready Egg is projected at the deepest tube position while waiting;
- its approach coordinates stop changing;
- its rendered shape can resemble a stationary enemy or shot;
- only one ready Egg is normally exposed at the observed point;
- the two approaching/ready limit affects how many can appear;
- the object disappears when an active Flipper slot opens and the Egg hatches.

This is the leading implementation-related hypothesis, not an established
fact.

If confirmed, determine whether the observed presentation is:

- correct but newly prolonged by the cap;
- a harmless project-translation artifact;
- or a rendering/state defect that should be corrected.


OTHER ENTITY HYPOTHESES

Explicitly rule in or rule out:

- a Flipper created at height zero but waiting for its first update;
- a Flipper paused indefinitely by a state transition;
- a cap-blocked hatch that partially creates an enemy;
- a duplicate Egg/enemy representation;
- an invisible-to-accounting presentation object;
- an enemy bullet at its source position;
- a player shot retained at depth zero;
- a destruction-effect slot;
- a stale renderer reference;
- a projection clamp artifact;
- a terminal interpolation endpoint.

Do not stop after finding a visually plausible object. Confirm the ownership,
state, and disappearance event.


REPRODUCTION

Attempt a bounded deterministic reproduction using existing repository seams.

Prefer:

- current Tempest.Game.Tests assembly;
- current diagnostic scenario;
- existing renderer mark/callback seams;
- existing simulation state summaries;
- bounded direct inspection from tests;
- existing headless APIs.

Do not create a standalone probe project or new disposable executable/DLL.

Do not rely on screenshots alone if deterministic state inspection can identify
the object.

Every reproduction loop must have an explicit maximum tick/iteration count and
a useful failure snapshot.

Capture, when available:

- simulation boundary;
- gameplay state;
- active Flipper count;
- Flipper identities/states/lanes/heights;
- reserve count;
- Egg count by lifecycle state;
- Egg lane/position/state;
- bullet count and positions;
- player-shot count and positions;
- unresolved total;
- visible render marks or entity draw calls.

Do not run an unbounded wait for the artifact.


PRODUCTION-LEVEL COMPARISON

Under identical seed/input conditions where practical, compare:

- normal uncapped production Level 2;
- capped Level 2 diagnostic.

Determine:

- whether the same object/state occurs in both;
- how long it remains visible in each;
- whether the cap merely prolongs an existing transitional state;
- whether the diagnostic introduces a new invalid state;
- whether state/accounting remains conserved.

Do not call a timing difference a defect unless the visible or lifecycle result
is actually invalid.


FIX DECISION

If the root cause is established and a correction is necessary, implement the
smallest valid fix.

A valid correction must:

- preserve the four-Flipper cap;
- preserve waiting-Egg accounting;
- preserve deterministic order;
- preserve all 12 obligations;
- preserve Egg release/approach timing;
- preserve canflip;
- preserve enemy firing;
- preserve bullet visibility;
- preserve rim speed;
- preserve death/re-entry;
- preserve Super Zapper;
- preserve completion/replay;
- preserve production Level 2 unchanged;
- preserve accepted production Level 1 unchanged.

Do not hide a real simulation object merely to remove a visual symptom unless
the presentation rule is demonstrably correct.

Do not delete or prematurely hatch a waiting Egg.

Do not reposition an entity without lifecycle evidence.

Do not consume randomness to fix presentation.


POSSIBLE ACCEPTABLE OUTCOMES

Outcome A — Narrow defect found and fixed:

- establish exact cause;
- make the smallest correction;
- add focused regression tests;
- validate;
- hand back to Neal for brief retest.

Outcome B — Harmless expected waiting presentation:

- establish exact entity and lifecycle;
- prove accounting/collision are correct;
- explain why the object appears/disappears;
- recommend either accepting the presentation or a separately justified visual
  refinement;
- do not make an arbitrary change.

Outcome C — Cause not established:

- do not guess;
- preserve the candidate;
- report exact findings and unknowns;
- provide Neal with a focused observation or recording plan;
- stop with status PARTIAL / MORE OBSERVATION REQUIRED.

Outcome D — Required validation blocked:

- follow the project's security/stall self-stop policy;
- preserve the candidate;
- report status BLOCKED or PARTIAL;
- do not claim completion.


NO PRODUCTION PROMOTION

Even if the artifact is fixed and the diagnostic remains strong, do not promote
the four-Flipper cap into production in this task.

Production promotion requires a separate prompt after Neal's corrected brief
manual retest.


TEST REQUIREMENTS IF A FIX IS MADE

Add focused tests proving, as applicable:

1. The exact artifact entity/state is identified.
2. The deterministic diagnostic reaches the prior artifact boundary.
3. The invalid visual/lifecycle condition no longer occurs.
4. A cap-blocked ready Egg remains unresolved and conserved.
5. The blocked Egg does not partially materialize a Flipper.
6. The Egg and resulting Flipper are never rendered simultaneously as duplicate
   representations.
7. The correct entity remains visible when it should remain visible.
8. The entity disappears only at the correct lifecycle transition.
9. Active Flippers never exceed four.
10. All 12 obligations remain conserved.
11. Egg release and approach timing remain unchanged.
12. Canflip remains unchanged.
13. Enemy bullets remain animated, visible, and collidable.
14. Player shots remain unchanged.
15. Death/re-entry remains coherent.
16. Super Zapper remains coherent.
17. Completion/replay remains reachable.
18. Uncapped production Level 2 remains unchanged.
19. Accepted production Level 1 remains unchanged.
20. Levels 3-8 remain unchanged.

Every state-waiting test must have an explicit bound and useful failure output.


VALIDATION IF A FIX IS MADE

Run:

1. New focused artifact regression tests.
2. Level2FourFlipperCapTests.
3. Level2SquareTests.
4. Level2CanFlipTests.
5. Egg lifecycle/accounting tests.
6. Renderer/entity tests.
7. Enemy firing/bullet tests.
8. Death/re-entry and Super Zapper tests.
9. Completion/replay tests.
10. Production Level 1 cap regressions.
11. Focused Levels 3-8 regression.
12. Expanded affected regression set.
13. Full solution:

dotnet test TempestCSharp.slnx --blame-hang-timeout 60s

14. Full build:

dotnet build TempestCSharp.slnx

15. Whitespace:

git diff --check

Do not launch the interactive game.

Neal will perform the brief visual retest.


VALIDATION IF NO FIX IS MADE

If investigation proves the artifact is expected and harmless, or if the cause
cannot yet be established:

- run only the bounded focused checks necessary to support that conclusion;
- do not run the full suite or build as ritual;
- report exactly what was and was not validated;
- preserve the existing candidate.


WINDOWS SECURITY FAIL-SAFE

Windows Application Control / Smart App Control has intermittently blocked newly
generated unsigned DLLs and EXEs.

Do not weaken, disable, bypass, or change Windows security.

Do not repeatedly regenerate or retry the same blocked artifact.

Do not create a disposable standalone probe executable.

Prefer existing repository source, existing tests, and established headless
seams.

If Windows security blocks a required artifact:

1. Record the exact command.
2. Record the exact artifact/path and error.
3. Retry only through a materially different safe route.
4. Never substitute stale binaries for the current candidate.
5. Never report the blocked command as passed.
6. If required validation cannot complete, STOP VOLUNTARILY.
7. Preserve the worktree.
8. Report completed work, blocked work, remaining validation, and the exact
   handoff Neal needs.

Do not wait for Neal to discover the block and force-stop the agent.


STALL / NO-PROGRESS FAIL-SAFE

All test, simulation, and analysis loops must have explicit:

- test timeouts;
- VSTest hang timeouts;
- tick/iteration bounds;
- process time bounds;
- clear stop conditions.

If meaningful progress stops beyond the reasonable expected duration:

1. Stop the operation.
2. Do not repeat the same unrestricted operation.
3. Inspect the result.
4. Isolate with smaller bounded runs.
5. Distinguish among:
   - stale test assumption;
   - unbounded helper;
   - production deadlock;
   - Windows security block;
   - infrastructure failure.
6. Preserve candidate files.
7. If bounded recovery is not safe, STOP VOLUNTARILY.
8. Provide Neal a blocker report and recovery recommendation.

Canceled, timed-out, stalled, hung, or blocked commands are never passes.


TEMP POLICY

TEMP/ is disposable local material.

Do not read TEMP/myNotes.md or other TEMP files as evidence.

Do not put the report under TEMP/ or Temp/.

Do not stage TEMP/.


PROTECTED FILES

Do not modify, stage, move, or delete:

- Reference/arcade/roms.rev2/;
- UserData/;
- screenshots;
- gameplay recordings;
- Videos/;
- existing AI reports;
- unrelated local files;
- test dumps/sequences;
- build output.

Do not run git clean.


DOCUMENTATION

This task is primarily investigation/focused correction.

Do not update README.md, TEMPEST-DEVELOPMENT-NOTES.md, or
TEMPEST-BEHAVIOR-SPEC.md unless:

- a correction is implemented; and
- the current diagnostic documentation would otherwise be inaccurate.

If no fix is made, leave durable tracked documentation unchanged and record
findings only in the new ignored investigation report.


AI-LOG POLICY

1. Save this complete prompt under:

AI-Logs/Prompts/

2. Create a NEW focused investigation/correction report under:

AI-Logs/Reports/

3. Do not rewrite existing reports.

4. AI-Logs is Git-ignored.

5. Do not stage or commit AI-Logs.

6. Do not place reports under TEMP/ or Temp/.


REPORT REQUIREMENTS

Create a descriptive date-prefixed report under AI-Logs/Reports/.

Include:

1. Starting Git state.
2. Accepted baseline.
3. Existing unstaged diagnostic inherited.
4. Manual Level 2 A/B acceptance evidence.
5. Exact artifact observation.
6. Files and lifecycle paths inspected.
7. Reproduction method and explicit bound.
8. Exact entity identity.
9. Collection ownership.
10. Lane/height/depth/state.
11. Renderer path.
12. Why it appears at the bottom.
13. Why it appears once per wave, if established.
14. Exact disappearance event.
15. Production-versus-diagnostic comparison.
16. Accounting/collision impact.
17. Waiting-Egg hypothesis result.
18. Other hypotheses ruled out.
19. Root cause.
20. Fix decision.
21. Files changed, if any.
22. Tests added/updated, if any.
23. Focused validation.
24. Expanded/full validation, if applicable.
25. Build result, if applicable.
26. git diff --check.
27. Windows security status.
28. Stall/timeout status.
29. Protected files unchanged.
30. Final Git state.
31. Exact manual retest command, if needed.
32. Focused manual observation checklist.
33. Remaining unknowns.
34. Recommendation on later production promotion.
35. Final status:
   - FIXED — MANUAL RETEST PENDING;
   - EXPECTED/HARMLESS — NO FIX;
   - PARTIAL — MORE OBSERVATION REQUIRED;
   - or BLOCKED.


GIT / STOP POINT

Do not stage.

Do not commit.

Do not push.

Do not promote the Level 2 cap into production.

Leave any corrected diagnostic unstaged for Neal's brief retest.

Do not begin Level 9.


MANUAL RETEST IF A FIX IS MADE

Provide:

dotnet run --project src\Tempest.Game -- --level2-four-flipper-cap-test --lives 10 --spinner-sensitivity 1

Ask Neal to verify:

1. The bottom-of-tube object no longer appears incorrectly.
2. Four-Flipper threat density remains improved.
3. Fewer simultaneous canflip pivots remain observable.
4. Enemy bullets remain visible and normal.
5. Rim pursuit remains dangerous.
6. Waiting Eggs continue hatching as slots open.
7. All 12 obligations appear.
8. Death/re-entry remains coherent.
9. Super Zapper remains normal.
10. Wave completion/replay remains normal.

A brief Level 2 run is sufficient unless the investigation identifies a state
that requires a particular reproduction procedure.


BEFORE FINAL RESPONSE

Confirm every applicable item is complete or explicitly reported partial or
blocked:

[ ] Artifact observation preserved accurately
[ ] Exact entity identified or explicitly unknown
[ ] Reproduction bounded
[ ] Waiting-Egg hypothesis tested
[ ] Other entity hypotheses considered
[ ] Accounting/collision impact established
[ ] Production Level 2 unchanged
[ ] Production Level 1 unchanged
[ ] Diagnostic cap preserved
[ ] Any fix narrowly validated
[ ] All loops bounded
[ ] No blocked/canceled command reported as passed
[ ] Prompt saved
[ ] New report saved
[ ] Protected files checked
[ ] Final Git state checked
[ ] Nothing staged, committed, or pushed
[ ] Production promotion NOT started
[ ] Manual retest command provided if needed

Stop after the focused investigation/correction and handoff.

TEMPEST_LEVEL2_BOTTOM_ARTIFACT_INVESTIGATION_20261109