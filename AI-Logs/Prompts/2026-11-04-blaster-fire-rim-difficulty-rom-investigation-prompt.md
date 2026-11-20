FAIL-CLOSED COMPLETENESS CHECK

Read this entire user-provided task before using any tools, reading files, running commands, searching content, or taking any repository action.

Before doing anything else, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context may appear after the user's completion token and do not invalidate it.

This check is intended to detect an incomplete or truncated user task, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project or repository action and explain that the task may be incomplete or truncated.


TEMPEST BLASTER FIRE, RIM PURSUIT, COLLISION, AND DIFFICULTY ROM INVESTIGATION

Perform a tightly bounded, READ-ONLY investigation into why the accepted C# game plays slightly easier than Neal's original Atari arcade cabinet.

Do not implement changes.

Do not modify production code, tests, project-authority documents, references, configuration, or Git state.

Do not launch the interactive game.

Do not commit.

Do not push.

Stop after the investigation report and await Neal's review.


REQUIRED BASELINE

The expected accepted production baseline is:

3bebf42cbe9514846ab32bf4c8a12bcfdb048da7
Implement accepted production Level 7

Before any investigation:

1. Run:
   git status --short --branch
   git rev-parse HEAD
   git log -1 --oneline
   git diff --stat
   git diff --cached --stat

2. Verify:
   - branch is master;
   - HEAD is exactly the accepted Level 7 commit above;
   - tracked worktree and index are clean.

Ignored AI-Logs are expected and are not a dirty tracked worktree.

If HEAD differs or tracked/staged changes exist, STOP before further repository activity and report the discrepancy.

Do not reset, restore, clean, discard, or alter anything.


MASTER DISASSEMBLY REFERENCE

The sole authoritative disassembly reference for this investigation is:

Reference/arcade/Tempest Rom Source code.md

Treat this project-local file as the master ROM source reference.

DO NOT:

- search the Internet for another Tempest disassembly;
- use GitHub or another repository for Tempest ROM code;
- use an Atari 2600 Tempest disassembly;
- use a home-console or computer port;
- use another arcade ROM revision;
- use another C port or translated implementation as ROM evidence;
- use another AI-generated Tempest source analysis as a substitute for tracing the project-local master file;
- mix symbols, addresses, tables, constants, or behavior from another version.

This restriction is important because another source may represent a different hardware platform, ROM revision, port, or interpretation.

If the master reference does not establish a detail, classify it as UNKNOWN rather than filling the gap from the Internet or another disassembly.

The current C# project may be examined to compare its behavior with the master ROM source after Atari behavior is established.

Project-local historical reports and authority documents may be read for existing project context, but they do not replace the master disassembly for new ROM conclusions.

Incitti's implementation is not needed for this investigation. Do not use it to infer or validate Atari firing, difficulty, rim-pursuit, or collision behavior.


PROJECT AUTHORITY AND SCOPE

Read:

- .github/copilot-instructions.md
- TEMPEST-BEHAVIOR-SPEC.md
- TEMPEST-DEVELOPMENT-NOTES.md
- README.md where relevant
- current player-shot, Flipper, rim-pursuit, collision, input, simulation-timing, and level-configuration code
- relevant existing tests
- prior reports only where they document accepted C# behavior or known approximations

The project authority order remains:

1. Neal's direct arcade observations.
2. The project-local master ROM source.
3. Accepted project behavior where it does not conflict with 1 or 2.
4. Project decisions and approximations, clearly labeled.

Do not reopen accepted behavior unrelated to the discrepancy.

Levels 1-7 remain accepted while this read-only investigation is performed.


ARCADE-OBSERVED DISCREPANCY

Treat the following as Neal's direct arcade observation:

- The accepted C# game now plays very close to the original arcade game overall.
- Across the implemented levels, it nevertheless feels slightly too easy.
- In the C# game, Neal can often leave the Blaster stationary in one rim lane, hold Fire continuously, and rarely die.
- The C# Blaster appears able to destroy rim enemies quickly enough that remaining stationary is unusually effective.
- On Neal's original arcade cabinet, this stationary hold-fire strategy is not viable, even on Level 1.
- An enemy that reaches the rim presents enough danger in the arcade that the player must move rather than safely defend one lane indefinitely.

Neal's cabinet uses its normal/default gameplay setting, which he believes is probably Medium.

That belief is not yet a ROM-backed conclusion. Determine from the master source what the options mean and whether a default/normal difficulty can be established.


CENTRAL INVESTIGATION QUESTION

Determine why:

STATIONARY BLASTER + HELD FIRE

is far safer in the current C# game than on the original arcade game.

Do not assume the cause is player-shot cadence.

The cause may be one or more of:

- fire-button repeat behavior;
- player-shot capacity;
- shot velocity;
- shot lifetime or slot retirement;
- shot creation position;
- shot collision tolerance;
- same-lane or adjacent-lane hit semantics;
- update ordering;
- rim-enemy target selection;
- rim lane-change frequency;
- rim pause/cadence;
- enemy transition vulnerability;
- Blaster/enemy collision timing;
- global simulation timing;
- difficulty adjustment;
- another ROM behavior discovered during the trace.

Let the master source determine the responsible mechanism.


QUESTION 1: WHAT DOES THE DIFFICULTY OPTION MEAN?

Trace the master source's cabinet option-switch processing for difficulty.

Identify:

- the relevant option-switch bits;
- how Easy, Medium, and Hard are decoded;
- the RAM variable or state representing difficulty;
- the initialization/default or normal setting, if the source establishes one;
- every code path that reads difficulty during gameplay parameter setup;
- every parameter adjusted by difficulty;
- whether the adjustment occurs globally, per level, per wave, per enemy, or at another boundary.

Produce a complete inventory of gameplay-affecting difficulty changes found in the master source.

Specifically determine whether difficulty affects any of:

- player firing;
- player-shot speed;
- player-shot capacity;
- enemy speed;
- Flipper climbing;
- Flipper rim pursuit;
- enemy firing;
- enemy supply;
- enemy limits;
- pending-enemy timing;
- Spike behavior;
- collision tolerance;
- another relevant parameter.

Do not infer that an option affects something merely because it would make design sense.

Show the actual data/control path.

If the source establishes that Medium is the default or normal setting, cite the exact switch decoding/table/comment evidence.

If it does not establish that, say so explicitly.


QUESTION 2: PLAYER FIRE BUTTON AND ACCEPTED-SHOT CADENCE

Trace the complete Blaster fire-button path from input sampling to a successfully created player shot.

Determine:

- how the fire button is sampled;
- whether it is edge-triggered or level/held-triggered;
- whether button debounce or latching influences firing;
- whether holding Fire attempts a shot every gameplay update;
- all gates that can reject a shot;
- the maximum number of simultaneously active player shots;
- how a free shot slot is found;
- when a new shot becomes active;
- whether firing has an explicit cooldown or delay;
- whether the cooldown is global or per slot;
- whether difficulty or displayed level affects firing cadence or capacity;
- whether Fire behaves differently during ordinary play, approach, exit, death, or other states.

Distinguish carefully among:

- input sampling frequency;
- gameplay update frequency;
- attempted firing frequency;
- accepted firing frequency;
- apparent audiovisual firing frequency.

Do not equate a sound trigger with successful shot creation unless the source proves that relationship.


QUESTION 3: PLAYER-SHOT MOVEMENT AND SLOT LIFETIME

Trace the master source's complete player-shot lifecycle.

Determine:

- initial along/depth position;
- initial segment/lane;
- movement direction;
- fixed-point or integer speed;
- movement frequency;
- retirement condition at the far end;
- retirement on enemy impact;
- retirement on Spike impact;
- behavior when no target is hit;
- whether one shot can hit more than one object;
- exact point at which its slot becomes reusable;
- whether difficulty or level affects any of these values.

Calculate the arcade shot's lifetime in gameplay movement invocations where the source permits it.

Do not convert that lifetime into C# 60 Hz updates unless the scheduling relationship is established.

If hardware/main-loop scheduling is required, trace it from this same master source only.


QUESTION 4: PLAYER-SHOT COLLISION RULES

Trace player-shot collision against all relevant enemy states, with emphasis on Flippers at or near the rim.

Determine:

- what player-shot coordinates/state are compared;
- enemy along/depth tolerance;
- lane/segment tolerance;
- any angular/transition-state checks;
- whether a Flipper changing lanes can be hit from one lane, both lanes, or according to another rule;
- whether the target can be hit during the entire pivot;
- whether rim enemies use the same collision routine as below-rim enemies;
- whether projectile collision is checked before or after enemy movement;
- whether player-shot hits occur before or after Flipper/Blaster collision;
- whether a shot fired at point-blank rim distance can destroy an enemy before that enemy kills the Blaster;
- whether firing while stationary creates a timing advantage in the current C# order that Atari does not provide.

Trace actual call order in the active gameplay state.

Do not stop at isolated collision routines. Establish their order relative to:

- player input;
- player-shot creation;
- enemy movement;
- player-shot movement;
- player-shot/enemy hit processing;
- enemy/Blaster collision;
- enemy destruction;
- score and sound processing.


QUESTION 5: FLIPPER RIM ARRIVAL AND PURSUIT

Trace what happens from the moment a Flipper reaches the near rim.

Determine:

- the movement-program handoff used at the rim;
- initial rim direction selection;
- how the Blaster's position affects direction;
- whether another rim enemy affects direction;
- rim pause/delay values;
- how pause expiration is processed;
- how often the Flipper changes rim lanes;
- whether it retargets the Blaster after each movement;
- whether it keeps a latched direction;
- open-tube endpoint behavior;
- closed-tube wrap behavior;
- whether displayed level changes rim cadence;
- whether difficulty changes rim cadence;
- when the player collision/grab test occurs;
- whether the Flipper can kill the Blaster while between lanes;
- whether firing suppresses or delays any part of pursuit.

Separate:

- ROM/data-backed movement timing;
- movement-program semantics;
- comments or labels in the disassembly that may be uncertain.

Because this discrepancy occurs even on Level 1, provide a concrete Level 1 trace.

Also determine whether the same rim behavior applies generally across Levels 1-7 or whether level-selected movement programs affect the rim path.


QUESTION 6: MAIN GAMEPLAY SCHEDULING

Trace the relevant scheduling from interrupt/input through a gameplay update.

Establish, from the master source only:

- interrupt or timing counter involved;
- condition under which a gameplay state pass runs;
- whether player input is sampled more frequently than gameplay logic;
- ordering of player movement, firing, enemy movement, player-shot movement, hit processing, and player collision;
- whether every enemy and every player shot moves once per relevant gameplay pass;
- whether some systems run at different divisors/cadences.

Do not import a timing rate from another disassembly or C port.

If a physical frequency cannot be proven from this file, report scheduling in source-level ticks/invocations rather than seconds.


QUESTION 7: CURRENT C# IMPLEMENTATION COMPARISON

Only after completing the Atari trace, inspect current C# behavior.

Identify the current values and semantics for:

- fixed simulation rate;
- held-fire handling;
- maximum active player shots;
- shot creation position;
- shot speed;
- shot lifetime;
- shot slot reuse;
- player-shot collision tolerance;
- transitioning-Flipper shootability;
- rim pursuit pause/cadence;
- rim direction/retarget policy;
- Blaster/enemy collision order;
- shot-before-player-collision order;
- any C# difficulty policy;
- whether the C# game currently has a fixed default difficulty equivalent;
- any level-specific variations.

Compare source path to source path.

Create a concise mismatch matrix with columns conceptually equivalent to:

- Mechanic
- Master ROM behavior
- Current C# behavior
- Match / mismatch / unknown
- Likely gameplay consequence
- Evidence classification

Do not present a mismatch as the cause merely because it exists.

Rank candidate causes only after evaluating how each difference could affect the stationary hold-fire scenario.


QUESTION 8: STATIONARY HOLD-FIRE SYMBOLIC SCENARIO

Construct a source-grounded symbolic scenario for Level 1:

- Blaster remains stationary in one lane;
- Fire remains held;
- a Flipper reaches the rim;
- the Flipper is not initially occupying the Blaster's lane.

Trace both:

A. Master ROM behavior.
B. Current C# behavior.

For each path, identify:

- shot attempts and accepted shots;
- number of shots that can exist;
- time/updates before slots recycle;
- Flipper rim movements;
- target-direction decisions;
- moment the Flipper enters or approaches the Blaster lane;
- moment a player shot can hit it;
- moment player collision can occur;
- which event wins when shot impact and player contact are close.

If exact random choices prevent one deterministic trace, enumerate the relevant branches rather than selecting one arbitrarily.

Explain which differences, if any, make stationary firing safer in C#.


QUESTION 9: DETERMINE THE MOST LIKELY ROOT CAUSE

Based only on the completed source traces, classify the result as one of:

A. Primarily player-fire cadence/capacity.
B. Primarily player-shot speed/lifetime.
C. Primarily player-shot collision semantics.
D. Primarily Flipper rim-pursuit cadence.
E. Primarily enemy/player collision ordering.
F. Primarily difficulty-setting behavior.
G. A demonstrated combination of identified factors.
H. No cause established from static analysis.

Do not choose a category without showing the supporting control flow and values.

If several mismatches exist but static analysis cannot determine their perceptual importance, say so and recommend a narrowly instrumented or deterministic comparison as a prerequisite to implementation.


DIFFICULTY IMPLEMENTATION SCOPE

Do not implement DIP switches or a difficulty menu in this investigation.

The eventual project target is the default/normal arcade behavior, expected by Neal to be Medium but subject to verification from the master source.

A selectable Easy/Medium/Hard feature may remain deferred.

If the source shows that supporting all three difficulty settings would later be trivial, mention that as an implementation observation only.

Do not let possible DIP-switch support broaden the current investigation.


EVIDENCE CLASSIFICATION

Use these exact categories consistently:

ARCADE OBSERVED
ROM-BACKED
CURRENT C# BEHAVIOR
PROJECT DECISION / APPROXIMATION
UNKNOWN

Do not call Neal's observation ROM-backed unless the source independently confirms it.

Do not call static source analysis arcade-observed.

Do not elevate uncertain comments in the disassembly to proven behavior without tracing the corresponding instructions/data.


NO INTERNET / EXTERNAL DISASSEMBLY RULE

This task must be completed without Internet research.

Do not use:

- web search;
- GitHub search;
- external code search;
- another Tempest repository;
- another disassembly;
- another C port;
- MAME source;
- Atari 2600 source;
- a console/computer port;
- a gameplay wiki;
- a strategy guide;
- an online service manual.

If the project-local master source and current repository cannot establish a detail, report UNKNOWN.

Do not weaken this restriction in order to make the report appear more complete.


OUT OF SCOPE

Do not:

- modify production code;
- modify tests;
- modify README.md;
- modify TEMPEST-BEHAVIOR-SPEC.md;
- modify TEMPEST-DEVELOPMENT-NOTES.md;
- modify the master ROM reference;
- modify Incitti/reference files;
- implement firing changes;
- implement rim-pursuit changes;
- implement difficulty switches;
- implement a menu;
- retune enemies;
- change simulation timing;
- adjust collision tolerances;
- alter Levels 1-7;
- implement Level 8;
- launch the game;
- create gameplay recordings;
- commit;
- push.

This is an analysis-only milestone.


DELIVERABLE

Save the substantive prompt under:

AI-Logs/Prompts/

Save a paired detailed report under:

AI-Logs/Reports/

Use matching descriptive date-prefixed filenames, for example:

2026-11-04-blaster-fire-rim-difficulty-rom-investigation-prompt.md
2026-11-04-blaster-fire-rim-difficulty-rom-investigation-report.md

Follow .github/copilot-instructions.md.

The report must include:

1. Executive conclusion.
2. Starting repository state and verified baseline.
3. Source-boundary statement confirming that only the project-local master disassembly was used.
4. Neal's arcade observations.
5. Difficulty option decoding and complete gameplay adjustment inventory.
6. Fire-button and player-shot creation trace.
7. Player-shot capacity, cadence, speed, lifetime, and slot-reuse trace.
8. Player-shot collision semantics.
9. Flipper rim-arrival and pursuit trace.
10. Main gameplay scheduling and update order.
11. Current C# implementation trace.
12. ROM-versus-C# mismatch matrix.
13. Level 1 stationary hold-fire symbolic comparison.
14. Root-cause assessment.
15. Specific values/routines/addresses requiring later implementation attention.
16. Remaining unknowns.
17. Narrow recommended implementation milestone, if the evidence supports one.
18. Suggested automated tests for a future implementation.
19. Suggested manual acceptance procedure.
20. Final repository/Git state.

Include exact master-source labels, ROM addresses, and Markdown line references where available.

Use concise pseudocode or a Mermaid diagram only if it materially clarifies update order or the shot/rim interaction.

Do not create a generalized architecture proposal before establishing the actual mismatch.


FUTURE TEST DESIGN

Do not add tests now, but recommend focused future tests where supported.

Potential test areas include:

- held-fire accepted-shot cadence;
- maximum active shots;
- exact slot recycling;
- shot lifespan;
- Level 1 rim pursuit cadence;
- stationary-Blaster threat interval;
- transitioning-Flipper shootability;
- point-blank shot-versus-player-collision order;
- Medium/default difficulty parameters;
- accepted Levels 1-7 regression coverage.

Do not invent expected values. Expected values must come from the master source or be explicitly labeled project decisions.


MANUAL ACCEPTANCE DESIGN

Recommend a short later comparison that Neal can perform on both the arcade machine and C# implementation.

The primary visible scenario should remain:

- Level 1;
- default/normal difficulty;
- Blaster parked in one lane;
- Fire held continuously;
- observe whether a Flipper reaching the rim forces movement or kills the stationary Blaster.

Where practical, suggest separately observing:

- approximate number of simultaneous visible Blaster shots;
- approximate interval between shots;
- rim lane-change cadence;
- whether a Flipper survives long enough on the rim to reach the Blaster;
- behavior during a rim lane transition.

Do not require Neal to measure hidden counters or perform frame analysis unless static investigation proves that visual observation alone cannot separate the candidates.


FINAL VALIDATION

Because this is read-only:

- do not run the interactive game;
- builds/tests are not required unless needed to inspect existing test discovery or behavior without changing files;
- do not claim new gameplay validation from existing historical test results.

At the end run:

git status --short --branch
git rev-parse HEAD
git diff --stat
git diff --cached --stat

Confirm:

- HEAD remains at the accepted Level 7 baseline;
- tracked worktree and index remain clean;
- no production, test, authority-document, or reference file changed;
- only the intended ignored AI prompt/report records were created;
- no commit or push occurred.


FINAL RESPONSE

Return a concise but complete summary containing:

1. The most likely cause or combination of causes, if established.
2. Whether Blaster shot cadence/capacity differs.
3. Whether player-shot speed/lifetime differs.
4. Whether player-shot collision rules differ.
5. Whether Flipper rim pursuit differs.
6. What Easy/Medium/Hard actually change.
7. Whether Medium/default was established.
8. Whether the ROM explains Neal's stationary hold-fire observation.
9. Remaining unknowns.
10. Recommended next implementation milestone, or why implementation is premature.
11. Report filename.
12. Confirmation that only Reference/arcade/Tempest Rom Source code.md was used as the disassembly authority.
13. Confirmation that no code/test/document/reference changes, game launch, commit, push, or Internet disassembly search occurred.

Stop after the report and await Neal's review.

TEMPEST_BLASTER_RIM_DIFFICULTY_ROM_INVESTIGATION_COMPLETE