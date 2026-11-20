FAIL-CLOSED COMPLETENESS CHECK

Read this entire user-provided task before using any tools, reading files, running commands, editing files, staging files, committing, or taking any repository action.

Before doing anything else, locate the unique completion token near the end of this user-provided task.

Verify exactly one complete standalone token beginning with "TEMPEST_" exists in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context may appear after the user's completion token and do not invalidate it.

This check is intended to detect an incomplete or truncated user task, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project or repository action.


TEMPEST GAME OVER MILESTONE FINAL ACCEPTANCE AND COMMIT

Close out and commit the current Game Over presentation milestone.

No further Game Over visual experimentation is requested.

Neal has manually accepted the current presentation as GOOD ENOUGH FOR NOW.

This task is limited to:

1. finalizing the accepted density default;
2. reconciling durable documentation;
3. validating the complete accumulated Game Over milestone;
4. reviewing and cleaning the milestone worktree;
5. staging only the intended files;
6. committing a clean local checkpoint.

DO NOT configure GitHub.

DO NOT add a Git remote.

DO NOT push.

GitHub setup will be a completely separate task after this milestone is finished.


ACTIVE REPOSITORY

The active development repository is now:

C:\Users\nrnob\source\repos\Tempest

Perform ALL repository work only in that directory.

Do NOT modify or operate on:

C:\Users\nrnob\OneDrive\Repro\Tempest

The old OneDrive copy is a frozen temporary safety copy.

The project was moved out of OneDrive because recurring Windows Application Control blocking occurred while development/build outputs were being generated under the OneDrive-synchronized location.

After copying the repository to the local non-OneDrive path, removing generated bin/obj output, and rebuilding:

- the project built successfully;
- the test assembly loaded normally;
- all tests executed successfully;
- the game launched successfully.

Do not claim that OneDrive has been conclusively proven as the underlying Windows security root cause.

Record only the observed behavior and the practical development decision to use the local repository.


STARTING COMMITTED BASELINE

Expected HEAD:

b349e0b3f3517c9ae9c2e1f22ce78f6d801f862f
Correct Super Zapper color presentation

The worktree intentionally contains the complete accumulated uncommitted Game Over milestone and its subsequent refinements and diagnostics.

Do NOT revert it.

Before making any changes run:

git status --short --branch
git rev-parse HEAD
git log -1 --oneline
git diff --stat
git diff --name-status
git diff
git diff --cached --stat
git ls-files --others --exclude-standard
git diff --check

Require:

- branch master;
- exact expected HEAD;
- no staged changes;
- existing modifications correspond to documented Game Over work;
- no unexplained unrelated modifications.

If unexplained work exists, STOP rather than resetting, restoring, cleaning, deleting or discarding anything.


DO NOT CHECK OR CONFIGURE GITHUB

GitHub is explicitly outside this task.

Do not:

- run git remote add;
- create a GitHub repository;
- configure origin;
- fetch from GitHub;
- push;
- install/configure GitHub tooling;
- change branch names for GitHub;
- make any GitHub-related repository modification.

Finish the local Game Over milestone first.


READ THE GAME OVER HISTORY

Read the relevant Game Over reports under:

AI-Logs/Reports/

including the reports covering:

- ROM-only implementation;
- arcade visual refinement;
- final visual calibration;
- density diagnostic;
- speck-cluster diagnostic;
- four-burst experiment.

Treat these AI reports as historical evidence.

Do not rewrite historical reports merely because later observations refined earlier conclusions.

Durable current truth belongs in:

TEMPEST-DEVELOPMENT-NOTES.md

TEMPEST-BEHAVIOR-SPEC.md


FINAL MANUAL ACCEPTANCE

Record:

MANUAL ACCEPTANCE: GOOD ENOUGH FOR NOW.

Neal does not want further Game Over visual tuning at this time.

The accepted current presentation is:

final one-player death
->
Tempest-style GAME OVER text over the tube
->
ordinary tube/text/gameplay presentation disappears
->
isolated multicolor vector celebration effect on black
->
approximately four overlapping burst patterns
->
all burst records drain
->
terminal Game Over
->
R starts a fresh Level 1 game


SPECIFIC MANUAL JUDGMENTS

Record accurately:

1. The GAME OVER text is excellent.

2. The GAME OVER lettering visually fits Tempest's distinctive vector style.

3. No further GAME OVER glyph/font tuning is wanted.

4. Tube suppression before the celebration effect is correct.

5. During the celebration effect, ordinary presentation is correctly suppressed and the effect appears by itself against black.

6. The earlier single-burst presentation was too sparse.

7. Arcade/MAME observation showed approximately four distinct burst patterns occurring in rapid overlapping succession.

8. Each later burst appears to begin around halfway through the preceding burst.

9. The four patterns do not appear to be the same pattern simply repeated four times.

10. The overlapping bursts account for much of the arcade presentation's perceived richness.

11. Earlier diagnostic testing found density 0.5 produced the best individual-speck character before the four-overlapping-burst presentation was introduced.

12. With the current four-burst presentation, density 2 produces the best overall brightness/richness in Neal's manual comparison.

13. The current presentation is sufficiently close to the arcade for the present project stage.

14. Exact fidelity may be revisited later.

15. Stop further Game Over visual tuning now.


ROM-BACKED EVIDENCE

Preserve the established ROM-backed findings without embellishment.

State $24 is associated with the arcade score/high-score-qualifying route.

It is NOT established as an unconditional ordinary Game Over effect.

Preserve documented ROM-backed mechanics including:

- 16 indexed parent records;
- $80 starting-coordinate behavior;
- POKEY-derived signed movement;
- established movement bounds;
- record spawning/countdown behavior;
- drain-after-spawning behavior;
- completion only after active records clear;
- documented relevant graphic/palette identifiers.

Do not promote unresolved visual interpretations to ROM-backed status.


ARCADE OBSERVED EVIDENCE

Record as arcade-observed/manual evidence:

- the celebration is associated with qualifying for the leaderboard/high-score path;
- GAME OVER is presented before the celebration;
- the tube and ordinary presentation disappear before the celebration;
- the celebration appears isolated against black;
- the visible presentation consists of many tiny multicolor vector specks;
- approximately four visually distinct burst patterns occur;
- these bursts overlap substantially;
- later bursts begin roughly halfway through preceding bursts;
- the patterns visibly differ;
- overlap contributes substantially to perceived brightness/richness.


PROJECT DECISION

The C# recreation intentionally shows this celebration after EVERY final one-player death.

That is NOT literal arcade behavior.

The project currently does not implement the leaderboard/high-score/front-end system.

Neal wants the distinctive arcade celebration included in the current simplified single-player flow.

Therefore unconditional post-Game-Over use is:

PROJECT DECISION.


PROJECT TRANSLATION / APPROXIMATION

Keep clearly classified as project translations/approximations where applicable:

- child-speck representation;
- deterministic compact two-dimensional child offsets;
- exact mark geometry;
- exact C# color translation where ROM RGB is unavailable;
- explicit four-burst grouping;
- burst scheduling/timing;
- burst seed/random differentiation;
- C# timing translation;
- unresolved displayed geometry associated with graphic $1F / $47 / $4A;
- unconditional celebration after final one-player death.

Do not claim those are literal ROM implementations.


DEFAULT DENSITY

Change the normal/default Game Over visual density to:

2

Semantics:

no --game-over-density argument
=
accepted density 2

Explicit:

--game-over-density 2
=
same normal accepted presentation.

Retain:

--game-over-density <positive finite multiplier>

as a DEVELOPMENT-ONLY diagnostic.

Keep values such as:

0.5
1
2
4

usable for future investigation.

Do not expose this in normal gameplay UI.

Do not persist the value.

Do not remove the diagnostic.

Document density 2 as:

MANUALLY CALIBRATED / PROJECT DECISION

It is not ROM-derived.


FREEZE THE CURRENT FOUR-BURST PRESENTATION

No more visual tuning.

Do not change:

- number of bursts;
- current burst onset schedule;
- burst origins;
- parent randomization;
- parent movement;
- parent bounds;
- parent lifetime;
- child-speck offsets;
- speck size;
- color behavior;
- rendering style;
- spawning behavior;
- drain behavior.

The current four-burst mapping remains:

ARCADE OBSERVED / PROJECT TRANSLATION.

Do not try to reverse-engineer $1F / $47 / $4A further in this closeout.


PRESERVE GAME OVER TEXT

The current GAME OVER text is manually accepted as excellent.

Do NOT modify:

- glyph geometry;
- vector-font design;
- centering;
- scale;
- color;
- timing except where required by an actual correctness defect;
- rendering style.


DEVELOPMENT PATH DOCUMENTATION

Update current durable documentation to establish:

ACTIVE DEVELOPMENT PATH:

C:\Users\nrnob\source\repos\Tempest

The former:

C:\Users\nrnob\OneDrive\Repro\Tempest

is no longer the active development repository.

Record the observed practical reason:

recurring Windows Application Control blocking affected generated development assemblies under the synchronized OneDrive project location.

After migration to the local non-OneDrive repository and regeneration of bin/obj:

- build succeeded;
- tests executed normally;
- game launched normally.

Do not state that the underlying Windows/OneDrive mechanism has been conclusively proven.


RECOVERED TEST VALIDATION

The most recent successful validation from the NEW LOCAL repository was:

dotnet test TempestCSharp.slnx

809 total
809 passed
0 failed
0 skipped

The standalone build also succeeded.

git diff --check passed with only LF-to-CRLF conversion advisories.

Treat this as evidence that the corrected Spike reverse-zoom test executes successfully from the local repository.

Do not rely only on this prior run for final closeout.

Rerun final validation after the density-default/documentation changes.


SPIKE REVERSE-ZOOM TEST CORRECTION

Preserve the previously established diagnosis:

The earlier Spike reverse-zoom test failure was caused by a stale fixed 300-tick wait.

The longer four-burst Game Over presentation required more time before terminal GameOver.

The test was corrected to:

- retain depth/offset checks;
- verify the relevant intermediate presentation states;
- use the existing bounded state-based completion helper;
- verify final burst completion and no remaining effect records.

No production behavior was changed to satisfy that failure.

Do not revert this correction.


UNTRACKED FILE REVIEW

Expected Game Over implementation/test files currently include:

src/Tempest.Game/GameOverVectorAnimation.cs

src/Tempest.Game/GameOverVectorBurstSequence.cs

tests/Tempest.Game.Tests/GameOverDensityDiagnosticTests.cs

tests/Tempest.Game.Tests/GameOverVectorBurstSequenceTests.cs

Review these and include them if they match the documented milestone.


TEMPORARY COMMENT FILE

There is also currently an untracked file:

comment for AI.md

Inspect it.

Do NOT automatically commit it.

Determine whether it is merely temporary development/AI commentary.

If it contains no unique durable information:

remove it from the milestone/worktree.

If it contains useful durable information:

move that information into the proper durable project documentation before removing the temporary file.

If its disposition is genuinely uncertain:

STOP and report rather than deleting it.


NO FURTHER GAMEPLAY CHANGES

Do not change:

- Levels 1-7;
- enemy supply;
- Flipper movement;
- Level 7 2-3-2-2-2 behavior;
- 2.5 rim-speed calibration;
- --rim-speed diagnostic;
- Super Zapper gameplay;
- Super Zapper colors;
- Blaster firing;
- player shots;
- enemy firing;
- spinner;
- scoring;
- ordinary player death;
- approaches;
- exits;
- progression;
- audio;
- geometry outside the Game Over presentation;
- R restart semantics.

Only:

- accepted default density 2;
- necessary test expectation adjustment for that default;
- documentation reconciliation;
- path documentation;
- milestone cleanup;

are authorized.


NO NEW FRONT-END FEATURES

Do not implement:

- leaderboard;
- high-score qualification;
- initials entry;
- top-player list;
- credits;
- coin handling;
- start-button front end;
- attract mode;
- two-player support.

These remain outside the milestone.


TEST EXPECTATIONS

Adjust tests only as necessary to establish:

1. omitted --game-over-density now uses 2;

2. explicit --game-over-density 2 matches omitted/default behavior;

3. diagnostic overrides such as 0.5 and 1 continue to work;

4. ROM parent semantics remain unchanged;

5. four-burst sequencing remains unchanged;

6. density does not change gameplay state progression;

7. GAME OVER text remains unchanged;

8. isolated-black celebration presentation remains unchanged;

9. Spike reverse-zoom final death reaches terminal GameOver using the corrected bounded wait;

10. R restart remains unchanged;

11. normal non-final death remains unchanged.

Do not add redundant tests solely because this is a closeout.


FOCUSED VALIDATION

Run focused tests covering:

- Game Over presentation;
- Game Over vector animation;
- four-burst sequence;
- density diagnostic;
- player death;
- Spike reverse-zoom final death;
- Game Over rendering;
- restart.

Record actual total/passed/failed/skipped counts.


NEIGHBORING REGRESSION

Run a reasonable neighboring regression covering:

- Levels 1-7;
- progression;
- campaign soak;
- ordinary death/re-entry;
- Super Zapper;
- rim pursuit;
- restart.

Record actual counts.


FULL VALIDATION

Then run:

dotnet test TempestCSharp.slnx
dotnet build TempestCSharp.slnx
git diff --check

Requirements:

- tests must actually execute;
- zero executed tests is FAILURE;
- 0 failed;
- 0 skipped unless an already-known intentional skipped test exists and is explicitly explained;
- build 0 errors;
- report build warning count accurately;
- git diff --check must pass.

Do not count a build-only success as test success.


FINAL DIFF REVIEW

Before staging run:

git status --short --branch
git rev-parse HEAD
git diff --stat
git diff --name-status
git diff
git diff --check
git ls-files --others --exclude-standard

Review the COMPLETE accumulated Game Over diff.

Verify there are no:

- reference-image additions;
- Videos files;
- TEMP files;
- bin/obj outputs;
- OneDrive generated artifacts;
- ROM/reference edits;
- leaderboard work;
- high-score implementation;
- two-player work;
- credit/coin work;
- attract-mode work;
- Windows security scripts/settings;
- unrelated gameplay tuning.


AI LOGS

Create a final paired milestone closeout record according to project instructions.

Suggested filenames:

AI-Logs/Prompts/2026-11-05-game-over-acceptance-closeout-prompt.md

AI-Logs/Reports/2026-11-05-game-over-acceptance-closeout-report.md

Record:

- final presentation;
- manual acceptance;
- density 2 calibration;
- retained density diagnostic;
- four-burst interpretation;
- ROM-backed evidence;
- arcade-observed evidence;
- project decisions/translations;
- deferred exact-vector fidelity;
- local repository migration;
- recovered validation;
- final validation;
- files committed;
- commit hash;
- final repository state.

Preserve historical AI reports.

AI-Logs remain ignored according to established policy.


STAGING

Only after validation and diff review succeed:

stage ONLY the accepted Game Over milestone files.

Then inspect:

git status --short

git diff --cached --stat

git diff --cached --name-status

git diff --cached

git diff --cached --check

Require:

- staged diff contains only this milestone;
- no reference material;
- no generated files;
- no temporary comment file;
- no unrelated modifications;
- no unexpected deletion.


COMMIT

If staged review is correct, commit locally with:

git commit -m "Implement accepted Game Over presentation"

DO NOT PUSH.

DO NOT CONFIGURE GITHUB.

DO NOT ADD A REMOTE.


POST-COMMIT CHECK

After commit run:

git status --short --branch
git rev-parse HEAD
git log -1 --oneline
git diff --stat
git diff --cached --stat
git ls-files --others --exclude-standard

Expected:

- master;
- new local commit at HEAD;
- clean tracked worktree;
- clean index;
- no unintended untracked files.

If anything unexpected remains:

do not clean blindly;
report it.


FINAL RESPONSE

Return:

1. Manual acceptance status.

2. Final accepted Game Over sequence.

3. Density 2 default behavior.

4. Retained --game-over-density diagnostic behavior.

5. Four-burst evidence classification.

6. ROM-backed behavior summary.

7. Arcade-observed behavior summary.

8. Project decisions/approximations.

9. Deferred Game Over fidelity work.

10. Active development path.

11. comment for AI.md disposition.

12. Production files committed.

13. Test files committed.

14. Documentation files committed.

15. Focused validation counts.

16. Neighboring regression counts.

17. Full-suite counts.

18. Build warnings/errors.

19. git diff --check result.

20. staged git diff --check result.

21. Commit message.

22. New commit hash.

23. Final Git status.

24. Closeout report filename.

25. Confirmation no leaderboard/high-score entry/credits/coins/attract/two-player/reference-image/reference-ROM/unrelated gameplay work was added.

26. Confirmation no GitHub setup, remote configuration, or push occurred.

STOP after the clean local commit.

The next task, in a separate prompt/session, will be GitHub setup.

TEMPEST_GAME_OVER_ACCEPTANCE_COMMIT_COMPLETE