FAIL-CLOSED PROMPT CHECK

Read this entire user-provided task before doing anything.

Before any tools, files, commands, edits, tests, or repository actions, locate
the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a
line by itself in the user-provided task.

Agent-generated context after the user's completion token is allowed and does
not invalidate the check.

This check is intended to detect an incomplete or truncated USER task.

If exactly one complete TEMPEST_ token cannot be found, stop before taking any
project or repository action and explain why.


TASK

Correct the Level 8 Blaster spinner-direction mapping discovered during manual
testing, and investigate the two currently failing Spike/exit SimulationTests.

Repository:

C:\Users\nrnob\source\repos\Tempest

The working tree contains the existing unstaged Level 8 candidate plus the
orientation/active-exit corrections.

Preserve all correct existing work.

Do not restart Level 8 implementation from scratch.

Do not stage, commit, or push.


MANUAL OBSERVATION FROM NEAL

Levels 1-7 continue to play correctly.

The problem is specifically Level 8 after the tube's display orientation was
corrected.

The important arcade control rule is NOT simply:

"right input means visually right"

or:

"left input means visually left."

The actual invariant is spinner rotational direction.

On the original arcade game:

- turning the physical spinner CLOCKWISE always moves the Blaster CLOCKWISE
  around the tube;
- turning the physical spinner COUNTER-CLOCKWISE always moves the Blaster
  COUNTER-CLOCKWISE around the tube.

Because of the displayed orientation and open topology of Level 8, this means:

LEVEL 8:

- CLOCKWISE spinner rotation must move the Blaster toward the LEFT side of the
  displayed tube;
- COUNTER-CLOCKWISE spinner rotation must move the Blaster toward the RIGHT
  side of the displayed tube.

This visual left/right relationship is expected on Level 8.

The current candidate does the opposite:

- CLOCKWISE spinner rotation moves the Blaster visually RIGHT;
- COUNTER-CLOCKWISE spinner rotation moves the Blaster visually LEFT.

THAT is the bug.


IMPORTANT: DO NOT APPLY A GLOBAL INPUT REVERSAL

Levels 1-7 already behave correctly.

Do not globally reverse:

- mouse input;
- spinner delta;
- controller input;
- keyboard direction;
- Player input semantics;
- InputSystem direction.

The correction must preserve accepted control behavior on Levels 1-7.

Investigate how Level 8's open-tube lane ordering, orientation transform, and
clockwise/counter-clockwise traversal semantics interact.

Correct the smallest topology/orientation mapping responsible for the Level 8
direction reversal.


TUBE GEOMETRY IS FROZEN

Do not alter the Level 8 tube shape.

Neal has clarified that the tube shape itself is correct.

Preserve:

- all 16 ROM-derived vertices;
- Y-reflected displayed orientation;
- relative geometry;
- proportions;
- 15 playable lanes;
- open topology;
- both open endpoints;
- no closing edge;
- current tube scale.

Do not modify vertices to fix spinner direction.


CLOCKWISE / COUNTER-CLOCKWISE MODEL

Determine explicitly:

1. Which lane-index direction currently represents increasing lane number.
2. Which lane-index direction appears visually left/right on the corrected
   Level 8 tube.
3. Which sign of spinner delta represents physical clockwise rotation in the
   existing accepted input architecture.
4. Which sign represents counter-clockwise rotation.
5. How accepted closed levels convert that rotational direction into lane
   traversal.
6. Why Level 8 currently reverses that rotational meaning after its orientation
   translation.

Then correct the narrowest underlying cause.

Do not reason from screen-left/screen-right alone.

The testable invariant must be:

physical spinner direction -> clockwise/counter-clockwise tube traversal

Screen direction is only the visible consequence of the Level 8 geometry.


LEVEL 8 EXPECTED CONTROL RESULT

After correction:

CLOCKWISE SPINNER:
- Blaster follows the Level 8 rim clockwise;
- visually, on the corrected Level 8 orientation, the Blaster moves LEFT;
- movement stops at the clockwise open endpoint;
- it never wraps across the opening.

COUNTER-CLOCKWISE SPINNER:
- Blaster follows the Level 8 rim counter-clockwise;
- visually, on the corrected Level 8 orientation, the Blaster moves RIGHT;
- movement stops at the counter-clockwise open endpoint;
- it never wraps across the opening.

The Blaster must never occupy nonexistent lane 15.


ENDPOINTS

Preserve open-endpoint behavior:

- no lane 14 -> lane 0 wrap;
- no lane 0 -> lane 14 wrap;
- movement clamps/stops at each endpoint;
- repeated spinner movement toward an already-reached endpoint does not produce
  invalid state;
- reversing spinner direction moves safely back toward the interior.


ENTITY CONSISTENCY

If correcting traversal orientation requires changing the interpretation of
Level 8 lane direction, audit all affected consumers.

Do not allow player lane numbering to become inconsistent with:

- Flipper direction;
- Tanker cargo direction;
- Spiker lane identities;
- Spikes;
- shots;
- collisions;
- exit lane;
- playback;
- screenshot boundary state.

Do not change enemy direction semantics unnecessarily.

If player traversal can be corrected independently while preserving canonical
Level 8 lane identities, prefer that smaller solution.

Document exactly why the chosen approach is safe.


REQUIRED LEVEL 8 CONTROL TESTS

Add focused tests proving:

1. Physical CLOCKWISE spinner/input semantics produce clockwise Level 8 rim
   traversal.

2. On the corrected Level 8 display orientation, that clockwise traversal is
   visually toward the LEFT.

3. Physical COUNTER-CLOCKWISE spinner/input semantics produce
   counter-clockwise Level 8 rim traversal.

4. On the corrected Level 8 display orientation, that counter-clockwise
   traversal is visually toward the RIGHT.

5. The tests must derive visual direction from the Level 8 geometry rather than
   merely asserting a hard-coded lane-number sign.

6. Clockwise movement clamps correctly at its open endpoint.

7. Counter-clockwise movement clamps correctly at its open endpoint.

8. Reversing direction at either endpoint allows movement back inward.

9. The Blaster never occupies lane 15.

10. No traversal crosses the open gap.

11. The accepted Y reflection remains unchanged.

12. All 16 Level 8 vertices remain unchanged.

13. Levels 1-7 retain their existing accepted spinner-direction behavior.


LEVELS 1-7 REGRESSION

Neal manually reports that Levels 1-7 currently play correctly.

Treat that as behavior to preserve.

Run existing input/player movement tests plus appropriate Level 1-7 regression
tests.

Do not intentionally modify accepted closed-tube controls.


TWO CURRENTLY FAILING TESTS

Neal successfully ran:

dotnet test TempestCSharp.slnx --no-build

Result:

1052 total
1050 passed
2 failed

The failures are:

SimulationTests.ForcedRimSpikerIsNonKillClearedAndPersistentSpikeRetriesWithoutReconstruction

Expected:
AvoidSpikes

Actual:
Playing

and:

SimulationTests.SpikeTravelCollisionSweepsDepthAndLaneAndCostsOneLifeBeforeRetry

Expected:
AvoidSpikes

Actual:
Playing


INVESTIGATE, DO NOT BLINDLY UPDATE

Determine whether these failures are:

A. stale expectations from the old implementation in which active Spike-exit
simulation was incorrectly coupled to the AVOID SPIKES presentation state;

or

B. genuine production regressions introduced by the new exit behavior.

Use:

- the local Rev 2 ROM evidence;
- the orientation/exit blocked report;
- the orientation/exit correction report;
- current production code;
- existing tests and behavior specification.

Do not alter the tests merely to make the suite green.


ROM-BACKED EXIT DISTINCTION TO PRESERVE

The prior investigation established:

- Level 8 skips the pre-Level-8 AVOID SPIKES interstitial;
- this does NOT disable active exit firing;
- this does NOT disable player-shot movement;
- this does NOT disable Spike erosion;
- this does NOT disable Blaster-versus-Spike collision.

Therefore:

AVOID SPIKES presentation state

and

active Spike hazards during tube exit

must remain separate concepts.

Determine the correct retry state after Spike-related exit death from the
actual ROM-backed lifecycle.

If `Playing` is correct for a Standard Level 8 retry, update the Level 8/new
expectations accordingly.

If `AvoidSpikes` remains correct for the specific earlier-level scenarios in
those failing tests, correct production code instead.

Pay attention to which level/configuration each test actually exercises.

Do not accidentally change accepted earlier-level AvoidSpikes behavior while
fixing Level 8.


EXIT CORRECTION MUST REMAIN INTACT

Do not undo the recently implemented ROM-backed active-exit behavior.

Preserve:

- firing during active exit;
- normal cooldown;
- normal shot capacity;
- shot movement during exit;
- shot-versus-Spike erosion;
- Blaster-versus-Spike collision;
- lane switching during exit;
- Standard Level 8 presentation policy;
- absence of the Level 8 AVOID SPIKES interstitial.


VALIDATION

Run focused tests for:

- Level 8 spinner direction;
- Level 8 geometry;
- open endpoints;
- player movement;
- input;
- exit lane control;
- exit firing;
- Spike erosion;
- exit collision;
- the two previously failing SimulationTests.

Then run expanded regressions covering:

- Levels 1-7 controls;
- Level 8 production;
- geometry;
- Flippers;
- Tankers/cargo;
- Spikers/Spikes;
- player shots;
- death/re-entry;
- completion;
- Level 7 -> 8 progression;
- recording/playback;
- pause/forward/reverse;
- screenshot capture;
- --speed.

Then, if Windows Application Control permits:

dotnet test TempestCSharp.slnx

dotnet build TempestCSharp.slnx

git diff --check

If Application Control blocks a run, record the exact block rather than
repeatedly rebuilding.

Do not weaken Windows security as part of this task.


NO INTERACTIVE GAME

Do not launch the interactive game.

Neal will manually retest after automated validation.


DOCUMENTATION

Update durable documentation only as necessary to record:

- correct Level 8 clockwise/counter-clockwise spinner semantics;
- physical spinner direction is the invariant;
- visual left/right direction differs because of Level 8 orientation;
- Level 8 clockwise -> visually left;
- Level 8 counter-clockwise -> visually right;
- correct retry-state result from the two failing tests;
- evidence classification;
- manual acceptance remains pending.

Update:

TEMPEST-DEVELOPMENT-NOTES.md

TEMPEST-BEHAVIOR-SPEC.md

README.md only if genuinely necessary.


LOCAL FILE SAFETY

Do not modify or stage:

- Reference/arcade/roms.rev2/
- UserData/
- screenshots;
- gameplay recordings;
- Videos;
- unrelated files.

Do not run git clean.


AI-LOG POLICY

These instructions are explicit and non-conflicting.

1. Save this substantial prompt under AI-Logs/Prompts/.

2. Create a NEW focused correction report under AI-Logs/Reports/.

3. Existing Level 8 investigation, implementation, recovery, blocked, and
   orientation/exit correction reports remain historical records.

4. Do not rewrite existing reports.

5. AI-Logs/ is Git-ignored.

6. Do not stage or commit AI-Logs.

7. Do not put milestone reports under TEMP/ or Temp/.


CORRECTION REPORT

Record:

1. Starting Git state.
2. Neal's clarified spinner observation.
3. Current Level 8 lane/traversal mapping.
4. Clockwise/counter-clockwise input semantics.
5. Root cause of reversal.
6. Exact correction.
7. Proof tube geometry and Y reflection remain unchanged.
8. Endpoint behavior.
9. Levels 1-7 regression evidence.
10. Analysis of the two failing SimulationTests.
11. Correct retry-state semantics.
12. Tests changed.
13. Production files changed.
14. Focused results.
15. Expanded results.
16. Full-suite result or exact Application Control block.
17. Build result or exact block.
18. git diff --check.
19. Final Git state.
20. Manual retest command.
21. Remaining Level 8 acceptance work.


GIT / STOP POINT

Do not stage.

Do not commit.

Do not push.

Leave the corrected candidate unstaged.

Do not begin Level 9.


MANUAL RETEST COMMAND

Provide:

dotnet run --project src\Tempest.Game -- --level 8 --lives 10


FIRST MANUAL RETEST

Before broader testing, Neal should verify:

1. CLOCKWISE spinner rotation moves the Blaster CLOCKWISE along the tube and
   therefore visually LEFT on Level 8.

2. COUNTER-CLOCKWISE spinner rotation moves the Blaster COUNTER-CLOCKWISE along
   the tube and therefore visually RIGHT on Level 8.

3. Movement stops properly at both open endpoints.

4. Reversing the spinner at an endpoint moves the Blaster back inward.

5. No wrapping occurs.

6. Ordinary Level 8 gameplay remains stable.

Only after this passes should Neal resume manual exit firing/Spike testing.


MANUAL ACCEPTANCE STATUS

Level 8 remains MANUAL ACCEPTANCE PENDING.

STOP after automated validation and handoff.

TEMPEST_LEVEL8_CLOCKWISE_CONTROL_RETRY_CORRECTION_20261108