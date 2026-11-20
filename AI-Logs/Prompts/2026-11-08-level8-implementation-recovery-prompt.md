FAIL-CLOSED PROMPT CHECK

Read this complete user-provided task before doing anything.

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

Recover, complete, and validate the existing unstaged production Level 8
implementation candidate after the prior VS Code Copilot request failed with
HTTP 408:

"Timed out reading request body. Try again, or use a smaller request size."

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected committed baseline:

bc109b4597f83477f5c7998bd3a9e258c72c6edf
Close out accepted interactive speed diagnostic

The previous Level 8 agent completed substantial implementation work before the
HTTP 408 failure.

DO NOT restart the Level 8 implementation from scratch.

DO NOT undo, discard, revert, restore, or replace the existing unstaged
candidate.

Treat the current working tree as a partially completed implementation that
must first be inspected and understood.


KNOWN CURRENT WORKTREE STATE

Immediately after the failed request, Neal manually ran:

git status --short --branch

The worktree contained modifications to:

README.md
src/Tempest.Game/Gameplay/Egg.cs
src/Tempest.Game/Gameplay/Flipper.cs
src/Tempest.Game/Gameplay/Fuseball.cs
src/Tempest.Game/Gameplay/Player.cs
src/Tempest.Game/Gameplay/Pulsar.cs
src/Tempest.Game/Gameplay/Tanker.cs
src/Tempest.Game/Levels/Level.cs
src/Tempest.Game/Levels/LevelConfiguration.cs
src/Tempest.Game/Levels/LevelLoader.cs
src/Tempest.Game/Levels/LevelProgression.cs
src/Tempest.Game/Rendering/VectorRenderer.cs
src/Tempest.Game/TempestGame.cs
tests/Tempest.Game.Tests/CampaignSoakTests.cs
tests/Tempest.Game.Tests/DebugControllerTests.cs
tests/Tempest.Game.Tests/DirectLevelStartupTests.cs
tests/Tempest.Game.Tests/GameplayRecordingTests.cs
tests/Tempest.Game.Tests/InputDiagnosticTests.cs
tests/Tempest.Game.Tests/Level7ProductionTests.cs
tests/Tempest.Game.Tests/LevelConfigurationTests.cs
tests/Tempest.Game.Tests/LevelProgressionTests.cs

The failed request also left these new untracked candidate files:

src/Tempest.Game/Levels/Level8WaveSelection.cs
tests/Tempest.Game.Tests/Level8ProductionTests.cs

The local Rev 2 ROM reference directory also remains untracked:

Reference/arcade/roms.rev2/

That ROM directory existed before the Level 8 implementation and is reference
material, not part of the candidate.


KNOWN MANUAL DIFF CHECK

After the failure, Neal manually ran:

git diff --check

It reported no whitespace errors.

Git printed LF/CRLF conversion warnings for some working files. Those were
line-ending warnings, not git diff --check failures.

Do not perform unnecessary line-ending normalization simply because those
warnings appeared.


PRIMARY AUTHORITY REPORTS

Before judging or modifying the existing candidate, read:

AI-Logs/Reports/2026-11-08-level8-rom-preimplementation-investigation-report.md

AI-Logs/Reports/2026-11-08-level8-rev2-rom-geometry-continuation-report.md

The Rev 2 continuation report resolves the geometry blocker and is authoritative
where the actual Rev 2 binary data resolves an earlier unknown.

Also locate and read the saved Level 8 implementation prompt under:

AI-Logs/Prompts/

The saved original implementation prompt defines the complete intended
candidate.

Do not resend or recreate that very large prompt in this chat.

Use the existing saved prompt and reports as durable task context.


LOCAL ROM AUTHORITIES

Annotated ROM listing:

Reference/arcade/Tempest Rom Source code.md

Actual Rev 2 ROM binaries:

Reference/arcade/roms.rev2/

The ROM files are read-only reference inputs.

Do not:

- edit them;
- rename them;
- move them;
- patch them;
- rewrite them;
- stage them;
- commit them;
- intentionally alter their timestamps.

Do not search the internet for another Tempest disassembly.

Do not consult MAME, screenshots, gameplay video, Mark Incitti's implementation,
another port, online tube charts, or prior visual knowledge of Level 8.

The blind Level 8 reconstruction must remain intact until Neal performs the
first manual launch.


FIRST ACTION: RECOVER THE CURRENT CANDIDATE

Before making further edits:

1. Verify the current Git branch, HEAD, and status.

2. Inspect every current tracked and untracked Level 8 candidate change.

3. Inspect the complete current diff.

4. Read:
   - Level8WaveSelection.cs;
   - Level8ProductionTests.cs;
   - all modified topology/movement/rendering files;
   - all modified tests;
   - current durable documentation.

5. Search AI-Logs/Reports for any Level 8 implementation report that the failed
   session may already have created.

6. If an implementation report already exists:
   - read it;
   - determine whether it is complete or partial;
   - do not rewrite history unnecessarily;
   - finish it only if it is clearly the pending implementation report from
     the failed request and remained incomplete because of the 408.

7. Determine explicitly which original Level 8 implementation requirements are:
   - COMPLETE;
   - PARTIAL;
   - INCORRECT;
   - MISSING;
   - UNVALIDATED.

Do this before altering the candidate.

Do not redo correct work merely because this is a fresh chat.


RECOVERY PRINCIPLE

This task is NOT permission to redesign Level 8.

Preserve correct work left by the failed session.

Make only changes required to:

- complete missing functionality;
- correct demonstrable defects;
- finish tests;
- restore consistency;
- complete documentation;
- successfully validate the candidate.

If the candidate already implements a requirement correctly, leave it alone.


LEVEL 8 GEOMETRY REQUIREMENTS

The Rev 2 investigation establishes:

- Level 8 selects shape row 7;
- `lev_remap[7] = $07`;
- `lev_open[7] = $FF`;
- 16 ordered vertices;
- exactly 15 playable lanes;
- open topology;
- no edge from vertex 15 to vertex 0.

Recovered normalized C# geometry candidate:

0   (-200.0,      200.0)
1   (-175.0,      142.8571)
2   (-150.0,       85.7143)
3   (-125.0,       28.5714)
4   (-100.0,      -28.5714)
5   ( -75.0,      -85.7143)
6   ( -50.0,     -142.8571)
7   ( -28.5714,  -200.0)
8   (  28.5714,  -200.0)
9   (  53.5714,  -142.8571)
10  (  78.5714,   -85.7143)
11  ( 103.5714,   -28.5714)
12  ( 128.5714,    28.5714)
13  ( 153.5714,    85.7143)
14  ( 178.5714,   142.8571)
15  ( 200.0,      200.0)

Verify that the existing candidate implements the documented transform with
sufficient precision and preserves exact vertex ordering.

Do not visually tune these values.


OPEN TOPOLOGY REQUIREMENTS

Level 8 valid playable lanes are:

0 through 14

Lane 15 is not playable.

The candidate must ensure that:

- the Blaster cannot occupy lane 15;
- enemies cannot occupy lane 15;
- Tanker cargo cannot occupy lane 15;
- Spikes cannot occupy lane 15;
- no transition wraps lane 14 to lane 0;
- no transition wraps lane 0 to lane 14;
- tube rendering does not draw a closing edge;
- endpoint behavior is topology-aware.

At the same time:

- accepted closed Levels 1-7 must retain their existing wraparound behavior.

Inspect the prior candidate carefully for accidental global behavior changes.


BLASTER ENDPOINT REQUIREMENTS

For Level 8:

- Blaster traverses lanes 0..14;
- movement clamps/stops at both open endpoints;
- Blaster never wraps across the gap;
- Blaster never occupies lane 15;
- spinner direction remains consistent.

For existing closed Levels 1-7:

- preserve existing wrap behavior.


ENEMY ENDPOINT REQUIREMENTS

For relevant Level 8 enemies:

- do not enter lane 15;
- do not cross the open gap;
- reverse/select a valid inward neighbor at endpoints according to the
  ROM-derived topology behavior;
- do not create a fictitious endpoint pivot.

Preserve closed-level movement semantics.


OPEN-TUBE RENDERING REQUIREMENTS

Level 8 must draw:

- exactly 15 near-rim lane edges;
- exactly 15 far-rim lane edges;
- normal depth connections for the 16 vertices;
- no connection from vertex 15 to vertex 0.

Existing closed tubes must still draw their closing edges.


LEVEL 8 SUPPLY AND SELECTION

Level 8 Medium/default original/generic wave supply:

24

Global ordinary active creation gate:

6

Type limits:

Flipper:
min 0
max 5

Tanker:
min 1
max 2

Spiker:
min 2
max 4

Pulsar:
min 0
max 0

Fuseball/Fuzzball:
min 0
max 0

Ordinary bottom entrants:

- Tankers;
- Spikers.

Not ordinary bottom entrants:

- Flippers;
- Pulsars;
- Fuseballs/Fuzzballs.

Tanker cargo may still create Flippers.

Verify the existing candidate implements this correctly.


TANKER CARGO

Each Level 8 Tanker carries two Flippers.

Preserve:

- carrier accounting;
- two cargo identities;
- cargo ownership;
- no additional original/generic supply consumption for latent cargo;
- correct destruction/release behavior;
- correct score/audio/destruction effects;
- valid endpoint lane selection.

At endpoints:

- cargo must remain on valid lanes;
- no child may enter lane 15;
- no child may wrap across the gap;
- no cargo identity may be duplicated or lost.


ROM `$07` / `$19` CARGO BOUNDARY

Recovered ROM rule:

ROM-equivalent along < $20:
    use default straight Flipper entry `$07`

ROM-equivalent along >= $20:
    use Level 8 Flipper program `$19`

Equality at `$20` belongs to `$19`.

Natural Tanker release takes place below `$20`, at qualifying ROM values in
`$11..$1F`, so natural release uses `$07`.

A sufficiently deep player-shot-destroyed Tanker uses `$19`.

Verify the prior candidate uses an explicit, maintainable ROM-to-C# boundary
translation.

Do not compare unrelated C# height directly with decimal 32 without the
documented mapping.

Tests must cover ROM-equivalent:

$1F
$20
$21


LEVEL 8 `$19` FLIPPER PROGRAM

Deep Level 8 Tanker cargo selected into `$19` must repeat:

1. one along-movement interval;
2. begin a lane flip;
3. continue/complete the flip;
4. repeat.

The program does NOT contain:

- Level 7's 2/3 reversal sequence;
- a programmed direction reversal;
- a Spike test.

Direction remains stable unless topology endpoint behavior or existing rim/top
behavior requires reversal.

Do not copy Level 7 `$24` movement into Level 8.


NATURAL `$07` CARGO

Natural below-`$20` Tanker cargo must:

- climb straight below the rim;
- not perform `$19` flips below the rim;
- reach normal top/rim behavior normally.


STARTING SPIKES

Recovered ROM raw value:

$C8

Bounded C# translation candidate:

height =
    (0xF0 - rawAlong) * 400 / (0xF0 - 0x10)

For Level 8:

height ≈ 71.428571

Starting Spikes belong on playable lanes:

0 through 14

No gameplay Spike should be created on lane 15.

The height remains:

PROJECT TRANSLATION / MANUAL ACCEPTANCE PENDING

Do not silently substitute 60 or 200.


SPIKERS

Use:

minimum 2
maximum 4

Preserve appropriate existing production Spiker behavior.

Constrain all Level 8 Spiker and Spike lanes to 0..14.

Preserve final/no-pending same-unit Spiker-to-Tanker conversion.

No new original supply is created by that conversion.


PULSARS / FUSEBALLS

Production Level 8 contains:

no Pulsars;
no Fuseballs/Fuzzballs.

Do not add them.

Do not broaden the task into deferred Fuseball destruction work.


DEATH / RE-ENTRY

Level 8 uses generic ROM-shaped reconstruction closest to accepted Level 7.

Verify the existing candidate preserves:

- unresolved generic quantity;
- cleared unresolved species;
- cleared movement-program identity;
- latent cargo lost with the carrier;
- released unresolved cargo returned through generic reconstruction;
- resolved outcomes remain resolved;
- no duplicate allocation.

Do not use Level 3-6 species-preserving reconstruction for Level 8.


LEVEL 7 -> LEVEL 8 PROGRESSION

Verify that accepted production Level 7 naturally progresses into Level 8.

Preserve:

- score;
- lives;
- fresh Super Zapper charges;
- existing between-level approach;
- existing starfield policy;
- spinner-responsive Blaster during approach.

Do not alter accepted Level 7 behavior.


DIRECT LEVEL 8 STARTUP

Verify generic:

dotnet run --project src\Tempest.Game -- --level 8 --lives 10

resolves to the production Level 8 configuration.

Do not add a Level-8-specific command-line option.


LEVEL 8 COMPLETION

Level 9 is not implemented.

Verify:

- all Level 8 obligations resolve;
- terminal Level 8 behavior occurs cleanly;
- Level 9 is not loaded;
- no crash or hang occurs;
- restart returns to fresh Level 1 through existing policy.


SUPER ZAPPER

Preserve accepted Super Zapper behavior.

Verify Level 8 accounting/completion remains correct after use.


RECORDING / PLAYBACK / SPEED

Verify Level 8 compatibility with:

- gameplay recording;
- playback version 2;
- visible playback;
- headless playback;
- pause/resume;
- forward/grouped stepping;
- reverse/grouped scrubbing;
- screenshot capture;
- live input handoff;
- `--speed`.

Do not modify recording schema unless absolutely necessary.

The existing generic startup level representation should be sufficient.

Do not serialize `--speed`.


LEVELS 1-7 ARE FROZEN

Do not intentionally change accepted Levels 1-7.

Open-topology implementation must be conditional on topology.

Pay particular attention to the fact that the failed candidate modified several
general gameplay classes.

Review those modifications for unintended effects on closed levels.

Automated regressions must demonstrate that accepted Levels 1-7 remain intact.


DOCUMENTATION

The failed request already edited documentation.

Inspect the existing documentation diff before changing it.

Update only what remains incomplete or inaccurate.

Durable project authorities:

TEMPEST-DEVELOPMENT-NOTES.md

TEMPEST-BEHAVIOR-SPEC.md

README.md only where production Level 8/direct-level documentation genuinely
requires it.

Preserve evidence classifications:

RAW-ROM-DATA

ROM-LISTING-BACKED

REV-2-BINARY-CONFIRMED

EXISTING PROJECT BEHAVIOR

PROJECT TRANSLATION

MANUAL ACCEPTANCE PENDING

UNKNOWN / UNRESOLVED

Do not claim Level 8 manual acceptance.


REQUIRED TEST COVERAGE

Ensure the candidate has focused automated coverage for:

1. exact Level 8 geometry;
2. open topology;
3. 15 playable lanes;
4. no lane 15;
5. no false closing edge;
6. Blaster endpoint clamping;
7. closed-level wrap regression;
8. enemy endpoint behavior;
9. Tanker cargo endpoint correction;
10. supply 24;
11. active gate 6;
12. Tanker 1/2;
13. Spiker 2/4;
14. no ordinary Flipper;
15. no Pulsar;
16. no Fuseball;
17. two Flipper cargo children;
18. `$1F/$20/$21` cargo boundary;
19. natural `$07` cargo;
20. deep `$19` cargo;
21. `$19` move/flip loop;
22. no Level 7 2/3 movement;
23. starting Spike height approximately 71.428571;
24. Spikes only on lanes 0..14;
25. final-pending Spiker conversion;
26. generic death/re-entry;
27. Level 7 -> Level 8 progression;
28. `--level 8`;
29. Level 8 completion/terminal behavior;
30. Super Zapper;
31. recording/playback;
32. headless playback;
33. pause/forward/reverse;
34. screenshots;
35. `--speed`;
36. accepted Levels 1-7 regression behavior.


VALIDATION

After inspecting and completing the candidate:

1. Run the narrow Level 8/open-topology tests.

2. Run expanded affected regression tests covering:
   - geometry;
   - topology;
   - player movement;
   - rendering;
   - Flippers;
   - Tankers/cargo;
   - Spikers/Spikes;
   - wave selection;
   - accounting;
   - death/re-entry;
   - completion;
   - Super Zapper;
   - Level 7 progression;
   - direct startup;
   - recording/playback;
   - headless playback;
   - visible playback controls;
   - screenshots;
   - speed diagnostic;
   - closed Levels 1-7.

3. Run:

dotnet test TempestCSharp.slnx

4. Run:

dotnet build TempestCSharp.slnx

5. Run:

git diff --check

Diagnose and repair candidate-caused failures.

Do not launch the interactive game.


ROM FILE SAFETY

Verify before finalizing that:

Reference/arcade/roms.rev2/

remains unchanged.

Do not stage or commit it.

Do not intentionally alter ROM timestamps.

Do not generate modified ROM files.


LOCAL FILE SAFETY

Preserve:

- UserData;
- gameplay recordings;
- Videos;
- screenshots;
- unrelated local files;
- existing historical AI reports.

Do not use git clean.


AI-LOG POLICY

These instructions are explicit and non-conflicting.

1. Save this recovery prompt under:

AI-Logs/Prompts/

2. Preserve the original failed-session implementation prompt already stored
   there.

3. The implementation/closeout history belongs under:

AI-Logs/Reports/

4. Search for an implementation report created by the failed session.

5. If a Level 8 implementation report already exists and is clearly incomplete
   because of the 408 failure, finish that same implementation report so it
   accurately records the recovered candidate and validation.

6. If no implementation report exists, create a new descriptive Level 8
   implementation/recovery report under AI-Logs/Reports/.

7. Do not rewrite the two historical Level 8 investigation reports.

8. AI-Logs/ is Git-ignored.

9. Do not stage or commit AI-Logs.

10. Do not place milestone reports under TEMP/ or Temp/.


IMPLEMENTATION/RECOVERY REPORT

Ensure the final implementation record includes:

1. Original starting baseline.
2. HTTP 408 failure/recovery context.
3. Candidate state inherited from failed session.
4. Work already complete when recovery began.
5. Work corrected/completed during recovery.
6. Geometry implementation.
7. Open-topology architecture.
8. Blaster endpoint behavior.
9. Enemy endpoint behavior.
10. Rendering behavior.
11. Supply/selector.
12. Tanker/cargo.
13. `$07/$19` boundary.
14. `$19` movement.
15. Starting-Spike translation.
16. Spiker conversion.
17. Death/re-entry.
18. Progression.
19. Direct startup.
20. Terminal boundary.
21. Recording/playback/tooling.
22. Presentation translations.
23. Files changed.
24. Tests added/updated.
25. Narrow test results.
26. Expanded regression results.
27. Full-suite result.
28. Build result.
29. `git diff --check`.
30. Levels 1-7 regression evidence.
31. ROM/local-file safety checks.
32. Final Git state.
33. Manual commands.
34. Manual acceptance checklist.
35. Remaining presentation questions.


GIT / STOP POINT

Do not stage.

Do not commit.

Do not push.

Leave the completed candidate unstaged for Neal's manual test.

Stop after automated validation and manual-test handoff.

Do not implement Level 9.


MANUAL TEST COMMANDS

Provide Neal:

Direct Level 8:

dotnet run --project src\Tempest.Game -- --level 8 --lives 10

Slower Level 8:

dotnet run --project src\Tempest.Game -- --level 8 --lives 10 --speed 0.75

Natural campaign:

dotnet run --project src\Tempest.Game -- --lives 10

Fresh Level 8 recording:

dotnet run --project src\Tempest.Game -- --record-gameplay Temp\GameplayRecordings\level8-candidate.json --level 8 --lives 10

Visible playback:

dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\level8-candidate.json

Accelerated playback:

dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\level8-candidate.json --speed 2


MANUAL ACCEPTANCE CHECKLIST

Neal's first Level 8 launch must remain the blind visual test.

Do not inspect MAME, screenshots, video, another implementation, or online tube
charts before manual handoff.

Ask Neal to verify:

- ROM-derived tube shape;
- open endpoints;
- no false closing edge;
- Blaster endpoint behavior;
- spinner traversal;
- Tanker and Spiker ordinary entry;
- absence of ordinary bottom Flippers;
- absence of Pulsars/Fuseballs;
- Tanker cargo;
- natural straight `$07` cargo;
- deep `$19` move/flip cargo;
- open-end reversal;
- starting Spike height;
- Spiker growth;
- Spike erosion;
- enemy firing;
- shots/explosions;
- score/sound;
- Super Zapper;
- death/re-entry;
- natural Level 7 -> Level 8 progression;
- Level 8 terminal completion;
- fresh recording;
- visible/headless playback;
- pause;
- forward stepping;
- reverse scrubbing;
- screenshot capture;
- slower/faster `--speed` playback.


MANUAL ACCEPTANCE STATUS

Do not claim Level 8 accepted.

Manual acceptance remains pending.


STOP

After successful automated validation and manual-test handoff:

STOP.

Do not launch the interactive game.

Do not stage.

Do not commit.

Do not push.

Do not begin Level 9.

TEMPEST_LEVEL8_IMPLEMENTATION_RECOVERY_20261108