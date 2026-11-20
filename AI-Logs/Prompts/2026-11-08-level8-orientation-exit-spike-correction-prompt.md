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
project or repository action and explain that the supplied user task appears
incomplete.


TASK

Investigate and correct two manually discovered bugs in the existing unstaged
production Level 8 candidate:

1. The ROM-derived tube has substantially correct geometry, but its displayed
   orientation is wrong.

2. During Level 8 tube exit:
   - the Blaster can travel down a lane containing a Spike and exit without
     being destroyed;
   - pressing Fire during exit produces no player shots.

Repository:

C:\Users\nrnob\source\repos\Tempest

Committed baseline beneath the unstaged Level 8 candidate:

bc109b4597f83477f5c7998bd3a9e258c72c6edf
Close out accepted interactive speed diagnostic

The current working tree contains the existing unstaged Level 8 implementation
candidate.

Preserve that candidate.

Do not undo, discard, restore, restart, or reimplement Level 8 from scratch.

Do not stage, commit, or push.

Leave the corrected candidate unstaged for Neal's continued manual testing.


MANUAL TEST RESULTS FROM NEAL

Neal completed the first blind launch of the independently ROM-derived Level 8
candidate.

Observed results:

- For a first attempt, the reconstructed Level 8 tube is very good.
- The tube shape is approximately 90% correct.
- The general Level 8 gameplay behaves as expected.
- Ordinary gameplay showed no other blocking problem during this first test.
- The tube's displayed orientation is wrong, although the basic shape is
  substantially correct.
- During tube exit, the Blaster can travel down a lane containing a Spike and
  leave the tube without being destroyed.
- During tube exit, pressing Fire does not create player shots.
- Neal has not yet completed full Level 8 acceptance testing.
- These two bugs must be corrected before broader manual validation continues.

Do not record Level 8 as manually accepted.


REQUIRED PROJECT RECORDS

Read these reports before investigation or editing:

AI-Logs/Reports/2026-11-08-level8-rom-preimplementation-investigation-report.md

AI-Logs/Reports/2026-11-08-level8-rev2-rom-geometry-continuation-report.md

AI-Logs/Reports/2026-11-08-level8-implementation-recovery-report.md

Also inspect:

- the saved original Level 8 implementation prompt;
- the saved Level 8 recovery prompt;
- current unstaged code and tests;
- TEMPEST-DEVELOPMENT-NOTES.md;
- TEMPEST-BEHAVIOR-SPEC.md;
- relevant earlier exit, Spike, player-shot, geometry, projection, and
  transition tests/reports.

Use the current working tree as the candidate being corrected.


LOCAL ROM AUTHORITIES

Annotated ROM listing:

Reference/arcade/Tempest Rom Source code.md

Raw Rev 2 binary ROMs:

Reference/arcade/roms.rev2/

The binaries are read-only reference inputs.

Do not modify, move, rename, patch, stage, or commit them.

Do not search the internet for another disassembly.


NO VISUAL REFERENCE YET

Neal intentionally has not explained how the orientation is wrong.

Do not ask Neal for the correction yet.

Do not inspect:

- a screenshot of the correct arcade Level 8;
- MAME Level 8;
- gameplay video;
- Mark Incitti's implementation;
- another Tempest port;
- an online tube chart;
- any external visual reference.

First determine from the local Rev 2 ROM data and annotated listing whether the
wrong orientation comes from an incorrectly translated ROM transform or
presentation parameter.

Do not simply guess a rotation, reflection, inversion, or point-order change.

Do not tune by trial and error.

If the local ROM data and current C# projection cannot establish a defensible
orientation correction, stop and explain exactly what remains unresolved.
Neal can then provide a screenshot.

Preserve the blind experiment unless the evidence genuinely runs out.


BUG 1: LEVEL 8 TUBE ORIENTATION

The current Level 8 candidate uses the correct recovered shape-row coordinates
and open topology, but its displayed orientation does not match Neal's arcade
knowledge.

The shape itself is approximately 90% correct.

Investigate the complete ROM-to-screen transform before changing geometry.

Trace, as applicable:

- raw `lev_x` and `lev_y` byte interpretation;
- coordinate origin;
- whether ROM Y increases up or down;
- whether the vector monitor/cabinet orientation changes screen axes;
- ROM vector coordinate conventions;
- `lev_angle`;
- point order;
- winding/order significance;
- `lev_scale`;
- `lev_y3d`;
- `lev_y2d`;
- `lev_y2db`;
- tube center and viewpoint;
- 2-D and 3-D setup routines;
- Mathbox projection;
- vector-display rotation conventions;
- any axis swap;
- any X reflection;
- any Y reflection;
- any quarter-turn or other fixed rotation;
- cabinet/display orientation;
- current C# logical-screen orientation;
- current renderer transforms applied after LevelDefinition coordinates.

Determine whether the existing C# translation incorrectly:

- treated ROM Y as screen-down rather than screen-up;
- omitted a rotation;
- swapped axes;
- reflected an axis;
- reversed point order;
- applied a correct raw shape in the wrong display coordinate system;
- or made another specifically evidenced transform error.

Do not alter the recovered topology:

- 16 vertices;
- 15 playable lanes;
- open endpoints;
- no closing edge.

Do not distort individual vertices merely to make the result look plausible.

Prefer correcting one clearly established transform over manually editing all
coordinates.


ORIENTATION EVIDENCE REQUIREMENT

Before implementing an orientation correction, document:

1. The ROM coordinate convention.
2. The arcade/vector display convention.
3. The current C# coordinate convention.
4. Every transform currently applied.
5. The missing or incorrect transform.
6. The corrected mathematical mapping.
7. Why the correction is supported by ROM/code evidence.
8. Why competing transforms were rejected.

Classify the result as appropriate:

- REV-2-BINARY-CONFIRMED;
- ROM-LISTING-BACKED;
- EXISTING C# IMPLEMENTATION;
- PROJECT TRANSLATION;
- INTERPRETATION / HYPOTHESIS;
- UNKNOWN / UNRESOLVED.

Do not call an orientation ROM-backed if it is only inferred from visual
expectation.


ORIENTATION REGRESSION SAFETY

The correction must apply only where appropriate.

Do not accidentally rotate, reflect, or reorder accepted Levels 1-7.

If the problem is in Level 8's coordinate translation, keep it localized to the
Level 8 definition.

If the problem is a genuinely general ROM-to-C# conversion defect that does not
affect existing handcrafted levels, introduce the smallest maintainable
boundary.

Preserve:

- open topology;
- lane ordering;
- intuitive spinner direction;
- endpoint clamping;
- enemy direction rules;
- Tanker cargo endpoint correction;
- collision lane identity;
- transition rendering.

After any orientation change, verify that lane indices still correspond
consistently across player movement, enemies, Spikes, collision, rendering,
recording, and playback.


BUG 2: LEVEL 8 EXIT, SPIKES, AND FIRING

Investigate why the Blaster can exit through a lane containing a Spike without
being destroyed and why Fire produces no shots during exit.

Do not assume these are two unrelated defects.

Trace the entire current C# exit state and the corresponding ROM behavior.


TRACE CURRENT C# EXIT FLOW

Inspect, as applicable:

- state transition from Playing into exit delay and active exit;
- player input sampling during exit;
- Fire acceptance during exit;
- shot cooldown and shot-slot allocation;
- shot creation during exit;
- shot movement during exit;
- Spike collision/erosion during exit;
- Blaster movement down the tube;
- current exit lane tracking;
- current lane changes during exit;
- Spike lookup by lane;
- player-versus-Spike collision;
- player death during exit;
- restart/re-entry after an exit death;
- rendering and sound during exit;
- level-completion transition after successful exit.

Identify exactly:

- which state or condition suppresses Fire;
- whether shots are intentionally cleared when exit begins;
- whether RunOneStep continues player-shot simulation during exit;
- whether Spike collision is skipped, tested against the wrong coordinate, or
  disabled by the Level 8 exit policy;
- whether open-tube lane indexing contributes to the problem;
- whether the translated Level 8 Spike height exposes a previously hidden seam.


TRACE ROM EXIT FLOW

Using the local annotated listing and Rev 2 binaries, trace:

- player movement during tube exit;
- Fire/input handling during exit;
- player-shot creation during exit;
- player-shot movement and Spike erosion during exit;
- player and Spike along-position comparison;
- conditions that destroy the Blaster during exit;
- lane/segment selection;
- behavior after an exit death;
- conditions for successful level completion;
- whether Level 8 skipping the earlier `AVOID SPIKES` interstitial changes
  Spike collision or merely changes the presentation/state before exit.

Do not confuse:

- the `AVOID SPIKES` text/interstitial;
- actual Spike hazards during travel down the tube;
- Spike erosion by shots;
- ordinary gameplay collision.

Determine whether the arcade permits firing during active exit.

If the ROM establishes it, implement it.

If the ROM does not establish the behavior confidently, report the uncertainty
before making a speculative gameplay change.


EXPECTED FUNCTIONAL TARGET IF ROM-CONFIRMED

If the ROM trace confirms the behavior that Neal describes:

- Fire remains operational during active tube exit.
- Player shots are created through the appropriate existing shot mechanism.
- Shots continue down the selected exit lane.
- Shots can erode/shorten a Spike during exit.
- The Blaster collides with a remaining Spike when their positions overlap.
- A collision destroys the Blaster instead of allowing successful exit.
- Exit death uses the appropriate existing death/re-entry behavior.
- A sufficiently shortened Spike allows safe passage.
- Lane changes during exit remain supported as previously accepted.
- Sound, score, shot-slot limits, and cooldown remain coherent.
- Successful exit occurs only if the Blaster survives the Spike hazard.

Do not add infinite shots or bypass normal shot capacity.

Do not alter ordinary Playing-state firing behavior.


LEVEL 8 VERSUS EARLIER-LEVEL SCOPE

Determine whether the exit defect is:

- Level-8-specific;
- open-tube-specific;
- caused by Level 8's translated starting Spike height;
- or a general exit-state defect previously hidden by existing tests.

Fix the actual architectural seam at the narrowest correct level.

Do not unnecessarily special-case Level 8 if the ROM-backed behavior is general
to all Spike-bearing exits.

At the same time, do not alter accepted Levels 1-7 without evidence and
regression coverage.

Preserve the previously accepted behavior that Blaster lane switching works
during exit on implemented levels.


REQUIRED TESTS: ORIENTATION

Add focused tests that verify:

1. The corrected Level 8 transform is explicit and mathematically testable.
2. The 16 recovered ROM vertices remain represented exactly through the
   transform.
3. Point order/topology remains valid.
4. The tube remains open with 15 playable lanes.
5. No closing edge is introduced.
6. Endpoint identities remain stable.
7. Spinner direction and lane traversal remain coherent after the transform.
8. Existing Levels 1-7 geometry is unchanged.
9. Recording/playback uses the same corrected Level 8 geometry deterministically.

Avoid brittle screenshot/pixel tests where geometry assertions are sufficient.


REQUIRED TESTS: EXIT FIRING

If ROM-confirmed, add tests proving:

1. Fire is accepted during active exit.
2. A shot is created using normal slot/cooldown rules.
3. Holding Fire does not bypass accepted firing limits.
4. Shots advance during exit.
5. Shots remain in the current exit lane or follow the established exit-lane
   behavior.
6. Shot sound behavior remains correct.
7. Ordinary Playing-state firing behavior is unchanged.
8. Exit firing remains deterministic in recording/playback.


REQUIRED TESTS: EXIT SPIKE HAZARD

Add tests proving:

1. A Blaster traveling down an exit lane collides with an unshortened Spike.
2. Spike collision destroys the Blaster before successful tube exit.
3. A shot can erode/shorten a Spike during exit if the ROM confirms that path.
4. A sufficiently shortened Spike permits safe passage.
5. A Spike in another lane does not destroy the Blaster.
6. Lane switching during exit changes which Spike is relevant.
7. Open Level 8 endpoint lanes use valid Spike indices.
8. No lane-15 access occurs.
9. Death during exit produces valid re-entry/recovery.
10. Successful exit still progresses normally when no blocking Spike remains.
11. Accepted earlier Spike-bearing levels retain correct exit behavior.
12. Level 8 does not incorrectly receive the pre-Level-8 `AVOID SPIKES`
    interstitial merely to fix collision.


REQUIRED REGRESSION COVERAGE

Run focused and expanded tests covering:

- Level 8 geometry;
- Level 8 open topology;
- Blaster movement;
- endpoint clamping;
- rendering;
- player input;
- firing;
- shot slots and cooldown;
- shot movement;
- Spike erosion;
- exit lane control;
- exit collision;
- player death;
- re-entry;
- level completion;
- Level 7 -> Level 8 progression;
- Level 8 terminal completion;
- recording/playback;
- pause/step/reverse;
- screenshot capture;
- `--speed`;
- accepted Levels 1-7 geometry and gameplay.

Then run:

dotnet test TempestCSharp.slnx

dotnet build TempestCSharp.slnx

git diff --check

Do not launch the interactive game.


DOCUMENTATION

Update durable documentation only after the causes and fixes are established:

- TEMPEST-DEVELOPMENT-NOTES.md
- TEMPEST-BEHAVIOR-SPEC.md
- README.md only if user-facing behavior/commands genuinely require an update.

Record:

- Neal's partial manual Level 8 result;
- geometry approximately 90% correct;
- orientation defect;
- evidence-derived orientation correction;
- exit Fire defect and cause;
- exit Spike-collision defect and cause;
- implemented corrections;
- automated validation;
- continued MANUAL ACCEPTANCE PENDING status.

Do not claim full Level 8 acceptance.


ROM AND LOCAL FILE SAFETY

Do not modify, stage, or commit:

- Reference/arcade/roms.rev2/
- AI-Logs/
- UserData/
- gameplay recordings;
- Videos;
- screenshots;
- unrelated local files.

Do not run git clean.


AI-LOG POLICY

These instructions are explicit and non-conflicting:

1. Save this substantial prompt under:

   AI-Logs/Prompts/

2. Create a NEW focused correction report under:

   AI-Logs/Reports/

3. Existing investigation and implementation/recovery reports remain historical
   records and must not be rewritten.

4. Creating the new ignored prompt/report files is authorized and expected.

5. Do not stage or commit AI-Logs.

6. Do not place the correction report under TEMP/ or Temp/.


CORRECTION REPORT

Create a descriptive date-prefixed report under AI-Logs/Reports/.

Include:

1. Starting Git state.
2. Manual observations from Neal.
3. Existing Level 8 candidate/report context.
4. ROM and C# orientation investigation.
5. Proven orientation cause.
6. Competing transforms considered and rejected.
7. Corrected transform.
8. Exit Fire trace.
9. Exit Spike-hazard trace.
10. ROM evidence.
11. C# defects identified.
12. Files changed.
13. Tests added/updated.
14. Focused results.
15. Expanded regression results.
16. Full-suite result.
17. Build result.
18. `git diff --check`.
19. Levels 1-7 regression evidence.
20. ROM/local-file safety verification.
21. Final Git state.
22. Updated manual-test command.
23. Focused manual retest checklist.
24. Remaining Level 8 acceptance work.
25. Whether a screenshot is still required to resolve orientation.


GIT / STOP POINT

Do not stage.

Do not commit.

Do not push.

Leave the corrected Level 8 candidate unstaged.

Stop after automated validation and manual-test handoff.

Do not begin broader Level 8 tuning.

Do not begin Level 9.


MANUAL RETEST COMMAND

Provide Neal:

dotnet run --project src\Tempest.Game -- --level 8 --lives 10


FOCUSED MANUAL RETEST

Ask Neal to verify only these immediate corrections first:

1. Tube orientation now matches the arcade expectation.
2. Overall shape remains correct.
3. Open endpoints remain correct.
4. Spinner direction remains correct.
5. Blaster still clamps at both endpoints.
6. Fire works during active tube exit.
7. Exit shots travel down the selected lane.
8. Exit shots erode the selected lane's Spike.
9. The Blaster is destroyed if it reaches a remaining Spike.
10. A sufficiently shortened/cleared Spike permits successful exit.
11. Lane switching during exit remains functional.
12. Exit death/re-entry behaves normally.
13. No regression is visible in ordinary Level 8 gameplay.

If orientation remains unresolved after exhaustive local ROM/code analysis,
stop without guessing and state that a screenshot is now required.


MANUAL ACCEPTANCE STATUS

Level 8 remains partially tested and not accepted.

This correction is a prerequisite for continued Level 8 manual validation.


STOP

After automated validation and manual-test handoff:

STOP.

Do not launch the game.

Do not stage.

Do not commit.

Do not push.

Do not implement Level 9.

TEMPEST_LEVEL8_ORIENTATION_EXIT_SPIKE_CORRECTION_20261108