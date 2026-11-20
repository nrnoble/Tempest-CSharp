Determine the next Tempest gameplay development milestone after the completed
Tanker/cargo milestone.

This is an ANALYSIS-ONLY task.

Do not modify executable source code.
Do not modify tests.
Do not implement anything.
Do not commit or push.

CURRENT VERIFIED CHECKPOINT

Git checkpoint:

7e25aea Add Tanker gameplay and improve code maintainability

The working tree should be clean.

Current established project behavior includes:

- Level 1 Flippers
- Egg roaming/approach/hatching
- direct Flipper rim-arrival player collision
- visible Flipper rim pivot behavior
- player shots and Flipper scoring
- life loss and fresh-Egg reconstruction of unresolved enemies
- configurable --lives
- development-only Spikers and spikes
- development-only Flipper-fired EnemyBullets
- development-only Tanker with two-Flipper cargo
- Tanker natural and shot release
- Tanker/cargo dynamic accounting
- tube exit behavior
- 146 automated regression tests at checkpoint
- maintainability-focused source comments

A suspected but unconfirmed issue is already documented:
two rim Flippers may occasionally converge and remain visually overlapped.
Do not investigate or fix that in this task.

AUTHORITY ORDER

Use:

1. observed original Atari Tempest behavior
2. Reference/incitti/tempest.bmx as engineering reference
3. current validated C# implementation
4. assumptions only when the first three do not answer the question

Do not treat Incitti behavior as automatic proof of arcade accuracy.

READ

Read:

1. .github/copilot-instructions.md
2. TEMPEST-DEVELOPMENT-NOTES.md
3. README.md
4. current relevant Gameplay/Levels/Systems/Rendering code
5. current SimulationTests.cs as needed
6. Reference/incitti/tempest.bmx
7. Reference/incitti/Readme.txt
8. relevant existing AI logs for Tanker and next-milestone analysis

Reference/incitti is read-only.

Do not modify anything under Reference/incitti.

CANDIDATE MILESTONES

Evaluate at least:

1. Tanker-fired EnemyBullets
2. Spiker-fired EnemyBullets, if source-backed
3. general activation of Flipper EnemyBullets
4. Fuseballs
5. Pulsars
6. Super Zapper
7. progression/level composition needed before another enemy
8. any smaller missing arcade mechanic that should logically precede these

Do not assume the next milestone must be a new enemy type.

PROGRESSION

Recheck Incitti progression and identify where each candidate becomes relevant.

For each candidate determine:

- first source eligibility;
- dependency on currently implemented mechanics;
- dependency on mechanics still missing;
- whether it can be isolated behind a development scenario;
- whether implementing it now would require premature general level progression;
- whether the existing C# scaffold/system is usable;
- likely accounting impact;
- likely death/reset impact;
- rendering complexity;
- random behavior requiring deterministic tests;
- arcade-comparison requirements.

ENEMY BULLET QUESTION

EnemyBullet already exists and has been validated for Flipper firing.

Determine whether the best next milestone is to reuse that system for:

- Tankers;
- Spikers;
- normal Flippers;

before introducing another enemy.

Identify exact source-backed differences in ammunition, timers, firing locations and
conditions for each firing enemy.

Do not implement them.

FUSEBALL ANALYSIS

If Fuseball is a strong candidate, determine enough to judge implementation scope:

- direct hatch eligibility;
- movement states;
- climbing/descending behavior;
- lateral edge movement;
- open versus continuous tube behavior;
- player collision;
- player-shot collision;
- score rules;
- animation requirements;
- random decisions;
- death/reset behavior;
- level accounting;
- whether direct-hatched Fuseballs can be implemented before Tanker-carried
  Fuseballs;
- dependencies on Super Zapper or score-popup behavior.

PULSAR ANALYSIS

Likewise determine enough to judge:

- first eligibility;
- movement and lane-change states;
- pulse oscillator;
- edge/web visual effects;
- player collision;
- scoring;
- randomness;
- rendering implications;
- whether Pulsar implementation would force a broader renderer/state-system change.

SUPER ZAPPER

Determine whether Super Zapper should be implemented before later enemy types or
whether it remains safe to defer.

Explicitly consider Tanker stillborn cargo and enemy ZapIt behavior.

Do not implement placeholders merely for architectural completeness.

LEVEL PROGRESSION QUESTION

We currently use development scenarios instead of full arcade progression.

Determine whether this approach remains appropriate for the next milestone.

Recommend either:

A. continue implementing one mechanic/enemy in a controlled development scenario;

or

B. implement a small progression/composition layer first.

Do not recommend full game progression merely because it will eventually be needed.

Choose the smallest step that reduces future rework while preserving our ability to
manually compare one behavior at a time.

RECOMMEND EXACTLY ONE NEXT MILESTONE

The final recommendation must name exactly one next implementation milestone.

Explain:

- why it should be next;
- why the other candidates should wait;
- approximate implementation complexity: low / medium / high;
- which existing files are likely involved;
- whether any new file is actually necessary;
- development command-line scenario if useful;
- automated-test strategy;
- manual arcade-comparison strategy;
- explicit out-of-scope items.

Do not provide implementation code.

DO NOT OVERDESIGN

This project has benefited from small validated steps.

Avoid recommending:

- a generalized entity-component system;
- a full enemy hierarchy redesign;
- a general configuration framework;
- complete level progression;
- broad renderer redesign;

unless current source evidence demonstrates one is genuinely required for the next
milestone.

AI ARCHIVE

AI-Logs/ already exists.

Create if necessary:

AI-Logs/Prompts/
AI-Logs/Reports/

Save this task specification to:

AI-Logs/Prompts/2026-10-30-next-milestone-analysis-prompt.md

Preserve verbatim if available.

If exact preservation is unavailable, begin with:

"Faithful task specification reconstructed by the VS Code Agent. The original
prompt was not archived verbatim during execution."

Save the analysis report

[Archival note by the VS Code Agent: the task specification received during
execution ended at the line above. The remainder of the original prompt,
presumably naming the report destination, was truncated in transmission. The
report was saved to AI-Logs/Reports/2026-10-30-next-milestone-analysis-report.md,
matching the existing Prompts/Reports naming convention.]
