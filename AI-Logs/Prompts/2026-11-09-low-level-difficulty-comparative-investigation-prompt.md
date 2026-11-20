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

Perform a read-only comparative investigation into why the accepted C# Tempest
campaign feels materially harder than Neal's original Atari arcade machine on
the lower levels.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected accepted baseline:

c1a974a625eea2dfaecf30d1b8d729e022ef17e8
Close out accepted spinner sensitivity diagnostic

Confirm the actual branch, HEAD, remote tracking state, index, worktree, and
untracked state before beginning.

This task is investigation-only.

Do not modify production code, tests, durable documentation, configuration,
recordings, reference files, ROMs, or Git state.

Do not stage, commit, push, clean, move, rename, or delete anything.

Do not launch the interactive game.

Do not implement Level 9.


USER OBSERVATION

Neal is a highly experienced original Atari Tempest player and owns an original
arcade machine.

The accepted C# recreation is close overall, but Neal currently finds the game
substantially harder than the arcade game on the lower levels.

The difference is not adequately explained by physical spinner sensitivity:

- the newly accepted --spinner-sensitivity feature works correctly;
- 0.8 improved physical control;
- default remains 1.0;
- sensitivity tuning did not explain the broader difficulty difference.

The question is now:

WHY DOES THE C# GAME CREATE MORE SURVIVAL PRESSURE THAN NEAL'S ARCADE MACHINE
ON THE LOWER LEVELS?

Do not minimize the observation.

Do not assume that harder automatically means better arcade fidelity.

Do not assume one constant is responsible.

Do not tune anything in this task.


DIFFICULTY-SETTING QUALIFICATION

Neal believes his arcade machine is probably configured for Medium difficulty.

The local ROM evidence establishes that the difficulty decode includes Medium,
Easy, and Hard paths.

However, previous investigation did not prove the cabinet's physical switch
setting or that the factory/default position is Medium.

Therefore classify the comparison as:

NEAL-OBSERVED ARCADE MACHINE, BELIEVED MEDIUM BUT NOT YET HARDWARE-VERIFIED

Do not claim the cabinet setting is definitively Medium unless new local
evidence establishes it.

If the cabinet setting can be verified safely through existing service-mode
documentation already stored in the project, report how Neal could check it
later.

Do not instruct Neal to alter the cabinet setting.


PRIMARY SCOPE

Analyze accepted production Levels 1-8, but prioritize the lower levels where
Neal most clearly perceives excessive difficulty.

Use this priority:

1. Level 1
2. Level 2
3. Level 3
4. Levels 4-5
5. Levels 6-8 only where shared systems or progression amplify the effect

The purpose is not to re-investigate every mechanic in the project.

The purpose is to identify and rank the concrete differences most likely to
make C# survival harder than the arcade machine during early campaign play.


REQUIRED PROJECT REPORTS

Read the relevant historical reports before new analysis, including:

AI-Logs/Reports/2026-11-04-blaster-fire-rim-difficulty-rom-investigation-report.md

AI-Logs/Reports/2026-11-04-rim-speed-2.5-default-candidate-report.md

AI-Logs/Reports/2026-11-09-spinner-sensitivity-closeout-report.md

Also locate and read reports concerning:

- accepted production Levels 1-8;
- enemy supply and composition;
- Flipper firing;
- Tanker firing;
- Spiker behavior;
- rim pursuit;
- collision ordering;
- player firing;
- player-shot behavior;
- enemy bullets;
- wave release/creation pacing;
- death/re-entry;
- Super Zapper;
- gameplay timing;
- campaign soak;
- recorded playback;
- Level 8 implementation and closeout.

Do not repeat findings already adequately established.

Reopen prior conclusions only when current accepted code has materially changed
or the earlier analysis did not answer the present harder-than-arcade question.


PRIMARY AUTHORITIES

Use evidence in this order:

1. Neal's direct observation and expert play comparison with the original
   arcade machine.
2. Actual local Atari Tempest Rev 2 ROM binaries.
3. Project-local annotated ROM listing:
   Reference/arcade/Tempest Rom Source code.md
4. Current accepted C# production code and tests.
5. Project-local Mark Incitti implementation as a secondary engineering
   reference.
6. Existing historical reports and durable specifications.

Do not search the internet for another disassembly.

Do not use another Tempest port as Atari authority.

Do not alter ROM/reference files.


CURRENT ACCEPTED PROJECT DECISIONS

Treat these as accepted current behavior, but not automatically as literal ROM
equivalence:

- fixed-step/frame-paced 60-Hz project simulation;
- current Egg/release architecture;
- current player firing cadence and shot capacity;
- current enemy firing translation;
- current collision ordering;
- current rim-turn architecture;
- current 2.5 rim-speed calibration;
- accepted Levels 1-8 geometry and topology;
- accepted Level 8 open-rim movement;
- current death/re-entry architecture;
- current Super Zapper behavior;
- current spinner sensitivity default 1.0.

The investigation may identify one of these as a plausible difficulty cause.

Do not change it.

Do not describe accepted project behavior as proven arcade equivalence unless the
evidence actually establishes equivalence.


IMPORTANT PRIOR FINDINGS

The previous difficulty investigation established several differences that must
be reconsidered in the current harder-than-arcade context:

- the ROM has eight player-shot slots;
- C# has ten player-shot slots;
- the ROM normal-play path can attempt a shot each gameplay pass when a slot is
  available;
- C# applies a project firing cooldown;
- C# shots persist differently in the project coordinate system;
- C# collision/update ordering differs from the ROM;
- C# pivoting Flippers have different shot-ownership behavior;
- the ROM rim program and C# rim-pause/turn representation differ;
- the exact cadence equivalence was not fully established.

The previous investigation was framed largely around why stationary defense had
been too easy in C#.

The present observation is different:

THE CURRENT ACCEPTED C# GAME NOW FEELS HARDER THAN THE ARCADE ON LOW LEVELS.

Re-evaluate which previously identified differences may now contribute to
excessive difficulty after subsequent calibrations and corrections.

Do not assume earlier conclusions still rank the same way.


RIM-SPEED CALIBRATION

The project uses an accepted 2.5 rim-speed calibration.

This was manually accepted because earlier C# rim enemies were insufficiently
threatening and stationary lane camping was too effective.

The 2.5 factor is:

PROJECT DECISION / MANUAL CALIBRATION

It is not a literal ROM multiplier.

Investigate the complete current rim threat, not the multiplier in isolation.

Trace:

- below-rim climb;
- rim arrival;
- pause before pursuit;
- target/direction selection;
- directed pivot duration;
- continuous rim travel;
- rechecks and retargeting;
- collision timing;
- player-shot timing;
- simultaneous multiple-rim-enemy pressure;
- level-specific rates;
- geometry-dependent lane-turn duration.

Determine whether the accepted 2.5 calibration combines with another C# timing
difference to create excessive early-level threat.

Do not recommend reversing the calibration merely because it is conspicuous.

Quantify the complete effective cadence before drawing conclusions.


PLAYER FIRING AND SHOT ECONOMY

Compare ROM and C# for Levels 1-3 at minimum:

- player-shot slot capacity;
- accepted-shot cadence;
- held-Fire behavior;
- cooldown;
- spawn position;
- initial collision check;
- shot speed;
- shot lifetime;
- removal boundary;
- slot recycling;
- lane ownership;
- pivot-lane shootability;
- collision tolerance;
- collision ordering relative to enemy movement and player contact.

Determine whether C# gives the player:

- more defensive power;
- less defensive power;
- or different power depending on enemy depth/rim state.

Do not assume that more shot slots makes the game easier if cooldown, lifetime,
or slot occupancy reduces practical availability.

Calculate effective shot availability under representative early-level play
where possible.


ENEMY FIRING PRESSURE

Compare ROM Medium-path and C# accepted behavior for Levels 1-8:

- which species can fire;
- first eligible level;
- shot-slot capacity;
- shot holdoff;
- firing probability or deterministic eligibility;
- firing while climbing;
- firing after Tanker cargo release;
- firing at the rim;
- enemy-shot speed;
- shot lifetime;
- collision size/tolerance;
- whether several species share the same global shot capacity;
- update ordering;
- sound/event timing where it may affect player perception but not difficulty.

Pay particular attention to Levels 1-3.

Determine whether C# can generate:

- enemy shots too early;
- too many simultaneous shots;
- shots too frequently;
- shots that persist too long;
- shots that travel too quickly;
- shots with too generous a collision region;
- or unfair combined pressure with rim pursuit.

Do not conclude a discrepancy from code structure alone. Compare effective
behavior.


WAVE SUPPLY AND COMPOSITION

For Levels 1-8, compare:

- total original supply;
- initial Eggs;
- reserve quantity;
- ordinary species eligibility;
- species minima and maxima;
- Tanker cargo;
- active-unit limits;
- cargo materialization outside ordinary creation;
- final-pending conversions;
- death/re-entry reconstruction;
- resolved versus unresolved obligations.

Previous work often verified total supply.

This investigation must go beyond total supply and determine whether the same
supply is presented to the player with different temporal pressure.


RELEASE AND HATCH PACING

This is a high-priority area.

Trace and compare:

- initial active Eggs;
- reserve release interval;
- maximum active Eggs;
- approaching-Egg limits;
- ready-to-hatch limits;
- hatch delays;
- six-active or other ordinary-creation gates;
- delayed activation;
- simultaneous arrivals;
- post-death release state;
- random selection timing;
- Tanker cargo release;
- whether cargo bypasses ordinary active gates;
- whether several enemies can become dangerous in a shorter real-time window.

Determine whether the C# project's accepted Egg scheduling creates a denser or
more synchronized threat than the arcade ROM.

Quantify representative Level 1, 2, 3, and 4 timelines when practical.

Do not assume matching total supply implies matching difficulty.


ENEMY MOVEMENT BELOW THE RIM

Compare per eligible species:

- initial along/depth position;
- climb speed;
- first movement delay;
- route program;
- below-rim lane changes;
- flip duration;
- pause duration;
- Spike interaction;
- endpoint behavior;
- transition into rim logic.

Identify any C# movement that reaches combat-relevant depth sooner than the ROM.

Separate:

- literal ROM values;
- C# world units;
- project translation;
- measured effective simulation ticks.

Do not compare incompatible units without showing the conversion.


RIM ARRIVAL AND COLLISION

Trace exact order within a C# update and ROM gameplay pass:

- player movement;
- player firing;
- player-shot movement/collision;
- enemy movement;
- enemy pivot progression;
- enemy/player collision;
- enemy-shot movement;
- completion checks.

Determine whether C# allows an enemy to:

- arrive and collide in the same tick unexpectedly;
- complete a pivot before the player can respond;
- collide before a visually plausible movement completes;
- remain lethal from more lanes than the ROM;
- combine with another rim enemy without the ROM's pauses/checks.

Also determine whether the ROM contains grace/timing behavior that C# lacks.

Do not invent a grace period without evidence.


PLAYER COLLISION TOLERANCE

Compare:

- player versus enemy collision;
- player versus pivoting Flipper;
- player versus direct rim arrival;
- player versus enemy projectile;
- collision bands/tolerances;
- swept versus endpoint-only collision;
- lane ownership during pivots;
- multiple collisions in one update;
- life-loss timing;
- invulnerability/re-entry protection if any.

Determine whether C# hit regions or collision timing make the Blaster easier to
kill on early levels.

Quantify where possible.


DEATH / RE-ENTRY PRESSURE

Investigate whether C# death/re-entry makes subsequent survival harder than the
arcade by changing:

- unresolved quantity;
- active enemy reconstruction;
- species reselection;
- Egg timing;
- simultaneous hatches;
- enemy bullets;
- rim enemies;
- cargo;
- Spike state;
- Super Zapper state;
- player re-entry protection;
- initial player lane;
- delay before control/firing returns.

Distinguish a harder original wave from a more punishing post-death state.


SUPER ZAPPER EFFECT ON PRACTICAL DIFFICULTY

Compare accepted project behavior with the ROM for:

- first use;
- second use;
- enemy clearance;
- enemy bullets;
- Eggs/reserve;
- Tanker cargo;
- activation delays;
- post-zap enemy pressure;
- charge reset per level.

Determine whether C# leaves more dangerous unresolved pressure after use.

Do not redesign the Super Zapper.


RANDOMNESS AND CLUSTERING

Inspect whether deterministic/random selection differences create excessive
threat clustering.

Consider:

- lane clustering;
- species clustering;
- simultaneous hatches;
- multiple pursuing rim enemies;
- repeated enemy-shot eligibility;
- Tanker cargo placement;
- whether the same seeded simulation produces unusually dense situations.

Do not judge difficulty from one seed alone.

If useful, run bounded headless comparisons over multiple deterministic seeds,
but do not create permanent tests or modify code.

Report synthetic/headless evidence separately from arcade evidence.


GAME SPEED AND REAL-TIME CADENCE

Verify the accepted live cadence:

- host frame target;
- simulation updates per frame;
- behavior during slow frames;
- whether catch-up occurs;
- relation to ROM gameplay/NMI cadence where known;
- effect of `--speed`;
- whether default speed is actually equivalent to the intended 60-Hz project
  assumption.

Do not use `--speed` to tune the investigation.

Determine whether a global cadence mismatch could explain broad difficulty.

If not, rule it down explicitly.


SPINNER SENSITIVITY

Treat spinner sensitivity as a partially tested contributor, not the full
explanation.

Record:

- 0.8 improved Neal's physical control;
- default remains 1.0;
- sensitivity changes player control but not enemy pressure;
- broader difficulty remained a concern.

Do not recommend changing the default during this investigation.


LEVEL-SPECIFIC ANALYSIS

Produce a concise difficulty profile for every accepted production level:

Level 1 through Level 8.

For each level identify:

- original supply;
- eligible species;
- active limits;
- firing species;
- movement pressure;
- rim pressure;
- projectile pressure;
- Spike/exit pressure;
- any C# versus ROM difference likely to affect difficulty;
- confidence.

Do not write eight repetitive mini-reports.

Focus on differences relevant to survival.


MEASUREMENT APPROACH

Use read-only analysis and bounded commands.

You may run existing tests or existing headless tooling.

You may create temporary analysis output only under an ignored working
location, and remove it before final status if safe.

Do not add production code or permanent tests.

Where possible, measure representative effective timelines such as:

- first enemy becoming active;
- first enemy reaching the rim;
- first eligible enemy shot;
- number of active threats over time;
- number of simultaneous rim enemies;
- shot-slot occupancy;
- time from rim arrival to lethal contact.

Clearly distinguish:

- ROM-derived timing;
- C# measured timing;
- inferred comparison;
- unknown conversion.


PUBLIC GITHUB SOURCE

The accepted repository is public at:

https://github.com/nrnoble/Tempest-CSharp

The public GitHub repository may be used as a secondary verification of the
pushed accepted source.

The local repository remains the primary source because it provides exact
current files and local references.

Do not use a stale public web view to override verified local Git state.


RANKING REQUIREMENT

At the end, rank the plausible harder-than-arcade causes.

Use categories:

CRITICAL / STRONG CANDIDATE
MODERATE CANDIDATE
WEAK CANDIDATE
UNLIKELY
RULED DOWN
UNKNOWN

For each candidate provide:

1. Exact C# behavior.
2. Exact ROM/arcade comparison.
3. Why it could make C# harder.
4. Levels affected.
5. Evidence strength.
6. Interactions with other candidates.
7. Whether a controlled experiment is possible.


DO NOT RECOMMEND MANY SIMULTANEOUS CHANGES

The final recommendation must select at most:

- one primary controlled experiment;
- optionally one secondary experiment only if the first cannot isolate the
  cause.

Do not propose tuning several constants together.

The experiment should:

- be development-only or diagnostic where practical;
- preserve the accepted production default initially;
- isolate one clearly named mechanism;
- have focused automated checks;
- have a bounded manual comparison;
- be easily reversible;
- avoid contaminating Levels 1-8 accepted behavior before manual judgment.


PRIMARY EXPERIMENT SELECTION

Select the one strongest candidate that plausibly explains excessive lower-level
difficulty.

Examples may include, but are not limited to:

- rim-pursuit cadence;
- enemy projectile pressure;
- release/hatch density;
- collision ordering;
- hit tolerance;
- player-shot availability;
- death/re-entry pressure.

Do not prejudge the result.

The recommendation must follow the evidence.


MANUAL COMPARISON PLAN

Design a bounded future manual comparison for the selected experiment.

It should tell Neal:

- which level to use first;
- whether to use direct startup;
- lives setting;
- whether to use a fixed recording or normal play;
- exact observable behaviors;
- what would support the hypothesis;
- what would refute it;
- how to compare arcade and C# without relying only on death count;
- how to avoid confounding spinner sensitivity.

Prefer Level 1 first unless evidence points strongly elsewhere.


EVIDENCE CLASSIFICATION

Use:

ARCADE OBSERVED
CABINET SETTING BELIEVED MEDIUM / UNVERIFIED
ROM-LISTING-BACKED
REV-2-BINARY-CONFIRMED
CURRENT ACCEPTED C# BEHAVIOR
AUTOMATED TEST EVIDENCE
HEADLESS MEASUREMENT
SECONDARY ENGINEERING REFERENCE
PROJECT DECISION / MANUAL CALIBRATION
INFERENCE
UNKNOWN

Do not overstate static analysis as measured gameplay.


NO IMPLEMENTATION

Do not:

- alter difficulty;
- add a difficulty option;
- change enemy supply;
- change release pacing;
- change movement speed;
- change rim speed;
- change firing;
- change shot behavior;
- change collision;
- change death/re-entry;
- change spinner sensitivity;
- add diagnostics;
- modify tests;
- update durable documentation;
- stage;
- commit;
- push.

Stop after investigation and recommendation.


AI-LOG POLICY

These instructions are explicit and non-conflicting.

1. Save this complete substantive prompt under:

AI-Logs/Prompts/

2. Create a NEW investigation report under:

AI-Logs/Reports/

3. Creating these ignored files is authorized.

4. Existing reports remain historical records and must not be rewritten.

5. AI-Logs is Git-ignored.

6. Do not stage or commit AI-Logs.

7. Do not put the report under TEMP/ or Temp/.


REPORT REQUIREMENTS

Create a descriptive date-prefixed report under AI-Logs/Reports/.

Include:

1. Starting Git state.
2. Accepted baseline.
3. Neal's current observation.
4. Cabinet difficulty qualification.
5. Prior reports reused.
6. Files, tests, ROM regions, and references inspected.
7. C# update-order map.
8. ROM gameplay-pass order.
9. Player-shot comparison.
10. Enemy-shot comparison.
11. Supply/composition comparison.
12. Release/hatch pacing comparison.
13. Below-rim movement comparison.
14. Rim-pursuit comparison.
15. Collision comparison.
16. Death/re-entry comparison.
17. Super Zapper comparison.
18. Randomness/clustering findings.
19. Global cadence findings.
20. Spinner-sensitivity qualification.
21. Level 1-8 difficulty profile.
22. Ranked candidate causes.
23. Causes ruled down.
24. Exact unknowns.
25. Primary controlled experiment recommendation.
26. Optional secondary experiment, if justified.
27. Proposed automated checks for that experiment.
28. Proposed manual comparison.
29. Final Git state proving no tracked change.


FINAL REPOSITORY VERIFICATION

Before stopping, verify:

- HEAD remains c1a974a625eea2dfaecf30d1b8d729e022ef17e8;
- branch remains master;
- local and remote tracking state is unchanged;
- index is clean;
- tracked worktree is clean;
- ROM files are unchanged;
- UserData and recordings are unchanged;
- only authorized ignored AI prompt/report files were added.


STOP POINT

Stop after the read-only investigation and recommendation.

Do not implement the selected experiment.

Do not begin Level 9.

At completion, provide Neal a concise summary answering:

- What is the strongest explanation for excessive low-level difficulty?
- Is the cause likely one mechanism or a combination?
- Which accepted project decisions contribute?
- Is rim-speed 2.5 implicated, exonerated, or still uncertain?
- Is enemy projectile pressure too high?
- Is release/hatch density too high?
- Is collision timing too punishing?
- Is player-shot availability too limited?
- Does death/re-entry amplify difficulty?
- Could global cadence explain it?
- Which causes were ruled down?
- What single controlled experiment should happen next?
- What should Neal observe during that experiment?

TEMPEST_LOW_LEVEL_DIFFICULTY_COMPARATIVE_INVESTIGATION_20261109