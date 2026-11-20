FAIL-CLOSED COMPLETENESS CHECK

Read this entire user-provided task before using any tools, reading files, running commands, editing files, staging files, committing, or taking any repository action.

Before doing anything else, locate the unique completion token near the end of this user-provided task.

Verify exactly one complete standalone token beginning with "TEMPEST_" exists in the user-provided task.

VS Code/agent-generated context after the user's completion token does not invalidate it.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project/repository action.


TEMPEST GAME OVER FOUR-BURST PRESENTATION EXPERIMENT

Perform one bounded experimental refinement of the current uncommitted Game Over vector presentation.

The newest arcade observation suggests that the remaining visual-density discrepancy is NOT primarily caused by insufficient density within one burst.

Instead, the original arcade presentation appears to contain approximately FOUR DISTINCT BURST EVENTS that overlap substantially in time.

Test that hypothesis while preserving the already successful individual-speck appearance and ROM-derived movement model as far as practical.

Do not commit.
Do not stage.
Do not push.
Do not launch the game.


STARTING STATE

Committed HEAD remains:

b349e0b3f3517c9ae9c2e1f22ce78f6d801f862f
Correct Super Zapper color presentation

The worktree deliberately contains the complete uncommitted Game Over milestone and all subsequent refinements/diagnostics.

Read completely:

AI-Logs/Reports/2026-11-05-game-over-rom-only-implementation-report.md

AI-Logs/Reports/2026-11-05-game-over-vector-visual-refinement-report.md

AI-Logs/Reports/2026-11-05-game-over-vector-final-calibration-report.md

AI-Logs/Reports/2026-11-05-game-over-density-diagnostic-report.md

AI-Logs/Reports/2026-11-05-game-over-speck-cluster-diagnostic-report.md

Before editing run:

git status --short --branch
git rev-parse HEAD
git log -1 --oneline
git diff --stat
git diff --name-status
git diff
git diff --cached --stat
git ls-files --others --exclude-standard
git diff --check

Verify that existing changes correspond to the documented Game Over work.

STOP on unrelated or unexplained changes.


LATEST MANUAL ARCADE OBSERVATION

Treat the following as ARCADE OBSERVED / MANUAL COMPARISON.

After repeatedly reviewing both:

- Neal's recording of his original Atari Tempest arcade machine; and
- the clean MAME reference recording/frames;

Neal now perceives approximately FOUR DISTINCT BURST PATTERNS during the high-score visual effect.

Important observations:

1. The presentation does not appear to be one burst whose density simply increases.

2. There appear to be approximately four separate burst events.

3. The bursts occur in very rapid succession.

4. Burst 2 begins when Burst 1 appears to be approximately halfway through its visible evolution.

5. Burst 3 begins when Burst 2 appears approximately halfway through.

6. Burst 4 begins when Burst 3 appears approximately halfway through.

7. The bursts therefore coexist substantially.

8. They are NOT simply the same visible geometric pattern repeated four times.

9. Their overlapping lifetimes appear to account for much of the rich overall visual density.

10. The tiny mark/speck appearance obtained around:

--game-over-density 0.5

looks closest to Neal's arcade reference at the individual-element level.

11. Increasing density within one burst is not the desired solution. Higher density values can make the effect look increasingly line-like or otherwise too continuous.

The next experiment should therefore preserve sparse tiny individual marks and test overlapping distinct burst events.


REFERENCE MATERIAL

Clean MAME-derived frames are located under:

Videos\Fireworks-MAME

Original-cabinet reference frames are also available under the existing Videos reference paths.

Do NOT perform another exhaustive 99/103-frame analysis.

The previous visual analyses already established:

- tiny bright mark character;
- broad dispersion;
- multicolor presentation;
- isolated black background;
- general lifecycle.

For this task use the clean MAME frames only for a TARGETED temporal analysis of:

- burst onset;
- overlap;
- differing burst structure;
- relative burst origin/distribution;
- approximate progression from first through fourth burst.

Inspect a strategically selected chronological subset first.

Use adjacent frames only where necessary to distinguish burst onset/overlap.

Do not use FFmpeg. Frames are already extracted.

Do not modify or commit reference material.


TERMINOLOGY

It is now acceptable to refer internally to the four observed components as:

Burst 1
Burst 2
Burst 3
Burst 4

Do not replace the observed presentation with a generic real-world fireworks simulation.

The term "burst" is merely a convenient name for the four visually distinct temporal events.

Reproduce the observed Atari patterns, not assumptions about how a real-world phenomenon should behave.


ROM RECONCILIATION FIRST

Before changing the implementation, perform a SMALL bounded reconciliation against:

Reference/arcade/Tempest Rom Source code.md

Ask specifically whether the four observed overlapping burst events can be explained by already-traced ROM state-$24 behavior.

Investigate only enough to answer:

1. Can the 16 indexed records naturally divide into temporal or structural groups?

2. Does record initialization/cycling produce multiple observable waves?

3. Do graphic ID $1F, table entries $47/$4A, or associated vector-program behavior establish multiple substructures per record?

4. Is there any ROM counter/state behavior that could explain four staggered visible burst onsets?

5. Could record reuse after earlier records leave bounds naturally create successive visibly different groups?

6. Is there any evidence supporting a particular number of records per visible burst?

Do NOT assume:

4 bursts = 4 groups of 4 records.

That is mathematically convenient but is NOT ROM evidence.

If the source does not establish the mapping, say so explicitly.


IMPLEMENTATION DECISION AFTER ROM CHECK

If the ROM clearly explains the four overlapping bursts:

implement that ROM mechanism as faithfully as practical.

If the ROM does NOT establish the visible four-burst grouping:

implement the four-burst structure as:

ARCADE OBSERVED / PROJECT VISUAL TRANSLATION

while continuing to use the ROM-derived parent movement model inside each burst.

Do not spend an unbounded amount of time trying to decode the unresolved ROM vector graphic program.


EXPERIMENTAL FOUR-BURST MODEL

The experimental presentation should behave conceptually as:

Burst 1 starts
    |
    + approximately halfway through Burst 1 -> Burst 2 starts
        |
        + approximately halfway through Burst 2 -> Burst 3 starts
            |
            + approximately halfway through Burst 3 -> Burst 4 starts

Earlier bursts continue evolving while later bursts begin.

All active bursts may therefore coexist temporarily.

Do NOT wait for one burst to finish before starting the next.


DISTINCT PATTERNS

The four bursts must not simply be identical copies offset in time.

Each should have deterministic variation in its visible structure.

Possible sources of difference should be prioritized in this order:

1. ROM-established random/state differences, if found.

2. Existing seeded ROM-like parent randomization.

3. Deterministic per-burst seed variation as PROJECT TRANSLATION.

4. Small deterministic origin/distribution differences supported by the arcade frames.

Do not manually design four decorative preset pictures.

Do not make Burst 1 always a circle, Burst 2 always a star, etc., unless direct visual evidence actually supports such fixed geometry.

The distinction should emerge procedurally from different deterministic parent-record arrangements.


PRESERVE ROM PARENT MODEL

Each burst should retain the useful ROM-derived mechanics as far as practical:

- 16 indexed parent records;
- ROM-inspired starting-coordinate principles;
- POKEY-like/randomized signed movement translation;
- movement bounds;
- record lifecycle;
- spawning/drain character;
- deterministic seeded tests.

Do not raise parent density simply because multiple bursts coexist.

Do not convert the model into hundreds of independent generic particles.


IMPORTANT: CURRENT 0.5 VISUAL CHARACTER

Neal currently prefers the individual-element appearance associated with:

--game-over-density 0.5

Use that as the visual-character baseline for this experiment.

Do NOT automatically make 0.5 the final command-line default yet.

But for the experimental four-burst presentation, keep each burst sparse enough that individual elements remain tiny isolated specks.

The desired increase in overall screen richness should come primarily from overlapping bursts, NOT denser individual bursts.


DENSITY DIAGNOSTIC

Retain:

--game-over-density <multiplier>

The option remains useful.

Do not remove it.

Do not redefine it unnecessarily.

Neal should still be able to compare:

0.5
1
2
4

However, with four overlapping bursts, advise Neal to begin testing at:

--game-over-density 0.5

because that's the manually preferred individual-speck appearance.

Do not assume previously useful high values remain sensible after introducing burst overlap.


OPTIONAL TEMPORAL DIAGNOSTIC

Do NOT add another command-line tuning parameter unless clearly necessary.

Prefer implementing one reasonable four-burst overlap based on the observed sequence first.

If the exact half-overlap proves difficult to calibrate later, a separate timing diagnostic can be considered in another milestone.

Keep this experiment bounded.


BURST ORIGINS / SPATIAL RELATIONSHIP

Use the clean MAME frames to determine whether the four observed bursts appear to originate:

- from the same region;
- from different nearby regions;
- from substantially different screen regions;
- or cannot be determined confidently.

Do not guess.

If distinct origins are visibly established, model them as ARCADE OBSERVED.

If their exact coordinates are unclear, use a restrained deterministic PROJECT APPROXIMATION.

Do not position origins purely for aesthetic balance.


COLOR

Preserve the successful multicolor Tempest-vector approach unless targeted MAME-frame review establishes a specific burst-related color rule.

Do not give each entire burst one fixed color unless supported.

Do not make the four bursts artificially color-coded.

Preserve tiny independently colored marks.


BLACK ISOLATED PRESENTATION

Preserve the accepted correction:

When GameOverAnimation begins:

- tube/web is absent;
- GAME OVER text is absent;
- HUD is absent;
- Blaster is absent;
- enemies are absent;
- shots are absent;
- all ordinary gameplay presentation is absent.

Only the bright vector effect appears against black.

Do not change this.


GAME OVER TEXT

Neal considers the current GAME OVER vector lettering excellent.

Do not modify:

- glyphs;
- font;
- scale;
- centering;
- color;
- drawing style.

Preserve the current GAME OVER-over-tube phase exactly unless required for state transition correctness.


GAMEPLAY SCOPE

Preserve:

final single-player death
-> GAME OVER over tube
-> tube/text disappear
-> four overlapping burst presentation
-> effects drain
-> terminal Game Over
-> R restarts Level 1

The real arcade gates this presentation behind leaderboard/high-score qualification.

The C# project continues to show it unconditionally as a PROJECT DECISION because leaderboard handling is not implemented.

Do not implement leaderboard qualification.


DO NOT IMPLEMENT

Do not implement:

- leaderboard;
- initials entry;
- top-score screen;
- credits;
- coins;
- attract mode;
- two-player;
- new frontend;
- generic real-world fireworks physics;
- textures;
- sprites;
- raster particles;
- bloom;
- blur;
- gravity;
- arbitrary emitter physics.


NO UNRELATED GAMEPLAY CHANGES

Do not alter:

- Levels 1-7;
- 2.5 rim calibration;
- --rim-speed;
- Super Zapper;
- Blaster firing;
- player shots;
- enemy firing;
- below-rim movement;
- spinner;
- score;
- lives;
- ordinary death;
- approaches;
- exits;
- progression;
- audio.


TESTING

Add/update focused tests proving:

1. There are four experimental burst instances/events.

2. Burst 1 starts first.

3. Burst 2 starts before Burst 1 completes.

4. Burst 3 starts before Burst 2 completes.

5. Burst 4 starts before Burst 3 completes.

6. Approximate half-life overlap is represented by the chosen project translation.

7. Burst patterns are deterministic but not identical.

8. Each burst preserves the intended maximum parent-record capacity.

9. Burst overlap does not mutate gameplay.

10. Individual visible marks remain tiny.

11. Density multiplier still works.

12. Density 0.5 remains deterministic.

13. Increased overall richness comes from concurrently active bursts rather than enlarged marks.

14. Each burst drains normally.

15. Overall GameOverAnimation completes only after the final burst and remaining records drain.

16. GAME OVER text remains unchanged.

17. black isolated presentation remains unchanged.

18. terminal R restart remains unchanged.

19. ordinary non-final death remains unchanged.

Do not encode incidental pixels from reference images.


AUTOMATED VALIDATION

Run focused:

- Game Over;
- burst sequencing;
- density;
- death;
- renderer.

Run neighboring regression:

- Levels 1-7;
- progression;
- campaign soak;
- Super Zapper;
- rim pursuit;
- restart.

Then run:

dotnet test TempestCSharp.slnx
dotnet build TempestCSharp.slnx
git diff --check

Record actual counts.


DOCUMENTATION

Record clearly:

ARCADE OBSERVED:
- approximately four visibly distinct burst events;
- rapid staggered succession;
- each subsequent burst begins around halfway through the preceding burst;
- bursts visually differ rather than repeating one identical pattern;
- overlapping bursts contribute substantially to apparent total density;
- 0.5 currently gives Neal the preferred individual-speck character.

ROM-BACKED:
only the internal behavior actually established from state $24.

PROJECT DECISION / APPROXIMATION:
any four-burst grouping/timing/origin/seed interpretation not proven by ROM.

Do not mark the presentation accepted yet.


AI LOGS

Save:

AI-Logs/Prompts/2026-11-05-game-over-four-burst-experiment-prompt.md

AI-Logs/Reports/2026-11-05-game-over-four-burst-experiment-report.md


NO GAME LAUNCH

Do not launch the interactive game.

Stop for Neal's manual comparison.


MANUAL COMMANDS

PRIMARY:

dotnet run --project src\Tempest.Game -- --lives 1 --game-over-density 0.5

COMPARISON:

dotnet run --project src\Tempest.Game -- --lives 1 --game-over-density 1

OPTIONAL:

dotnet run --project src\Tempest.Game -- --lives 1 --game-over-density 2

Do not encourage higher density first.

The main question is now burst overlap, not individual-burst density.


MANUAL ACCEPTANCE QUESTIONS

Ask Neal:

1. Can four distinct burst events now be perceived?

2. Does each appear different from the others?

3. Does Burst 2 begin while Burst 1 is still visibly active?

4. Likewise for 3 over 2 and 4 over 3?

5. Is the overall screen richness now substantially closer to the arcade without increasing individual-speck density?

6. Does 0.5 still give the best individual-speck appearance?

7. Does the total display become too busy or remain natural?

8. Do colors and dispersion still look right?

9. Does the fourth burst drain naturally into terminal Game Over?

10. What remains visibly wrong?


GIT

Do not stage.
Do not commit.
Do not push.

Preserve the entire evolving uncommitted Game Over milestone.

Report final state accurately.


FINAL RESPONSE

Return:

1. ROM reconciliation result regarding four burst events.
2. Whether four-burst grouping is ROM-backed or arcade-observed/project-translated.
3. Exact burst scheduling model.
4. Overlap model.
5. How four patterns are made distinct.
6. Parent-record semantics per burst.
7. Density semantics.
8. Confirmation individual mark geometry remains unchanged.
9. Color behavior.
10. Origin/distribution behavior.
11. Production files changed.
12. Tests changed.
13. Documentation changed.
14. Focused test count.
15. Neighboring regression count.
16. Full-suite count.
17. Build warnings/errors.
18. git diff --check.
19. Manual commands.
20. Concise manual checklist.
21. Report filename.
22. Final Git state.
23. Confirmation no unrelated gameplay work, staging, commit, push or interactive launch occurred.

Stop for Neal's manual comparison.

TEMPEST_GAME_OVER_FOUR_BURST_EXPERIMENT_COMPLETE