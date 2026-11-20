Implement the first direct-hatched Fuseball milestone.

Follow .github/copilot-instructions.md and the established Tempest architecture,
commenting, validation, and scope rules.

CURRENT CHECKPOINT

Commit: 8f26e9b
Expected baseline: clean tree, 166 passing tests.

Use the existing analysis:

AI-Logs/Reports/2026-10-30-next-milestone-analysis-report.md

Do not repeat broad progression analysis. Re-read only the Fuseball-related portions
of Reference/incitti/tempest.bmx necessary to implement this feature accurately.

GOAL

Add an opt-in development scenario:

--fuseball-test

Normal Level 1 and existing --spiker-test / --tanker-test scenarios must remain
unchanged.

The scenario should make direct-hatched Fuseballs easy to observe and compare with
arcade Tempest.

Use the existing Gameplay/Fuseball.cs scaffold.

IMPLEMENT SOURCE-BACKED DIRECT FUSEBALL BEHAVIOR

From the existing analysis, verify and implement:

- direct hatch begins at height 0;
- initial lateral state, w = 0, direction = 1;
- lateral position w ranges from 0 through 8 across an edge;
- lateral movement uses the source 8-update cadence;
- continuous tubes wrap appropriately;
- open tubes bounce at endpoints where applicable;
- source random direction/phase decisions;
- climbing state;
- descending state;
- source-backed climb/descend duration;
- source-backed climb speed for the controlled development scenario;
- do not implement rimit hurry-up yet.

Player-shot collision:

- same edge;
- strict source-backed height band;
- Fuseball is shootable only in the interior lateral band 1 < w < 7;
- preserve strict boundary behavior.

Player collision:

- use the source-backed lateral/rim collision rule;
- enter the existing player-death lifecycle;
- do not invent additional collision behavior.

Scoring:

- implement the source height-based Fuseball score:
  250 through 1250 in 250-point increments;
- implement only the minimum score-popup behavior required to present that score
  correctly;
- do not build a general popup/notification framework.

ANIMATION / RENDERING

Implement the Incitti-derived Fuseball vector animation needed for manual comparison:

- dedicated 4-frame x 5-arm animation;
- source-backed color behavior;
- correct tube projection and lateral interpolation;
- reuse existing projection/rendering architecture;
- do not redesign VectorRenderer.

Add concise maintainability comments explaining non-obvious w-position geometry,
state transitions, collision bands, scoring, and animation intent.

ACCOUNTING / RESET

A direct-hatched Fuseball is one ordinary enemy unit:

- one Egg becomes one Fuseball;
- killing it resolves one enemy;
- surviving Fuseballs return through the existing fresh-Egg life-reset behavior;
- no special cargo ledger is needed;
- Fuseballs must prevent completion while unresolved.

Keep death/reset, score persistence, and level exit behavior consistent with the
existing validated game.

DEVELOPMENT SCENARIO

Keep --fuseball-test isolated from --spiker-test and --tanker-test.

Use a simple deterministic Fuseball-focused wave rather than implementing general
arcade progression.

Do not add full level selection or progression.

Choose a wave composition that makes Fuseball movement/collision/scoring easy to
observe. Document the choice as development-only, not arcade level composition.

TESTS

Add focused deterministic coverage for the genuinely new Fuseball behavior:

- hatch/start state;
- lateral w movement and cadence;
- continuous-tube wrap;
- climb and descend state transitions;
- controlled randomness;
- strict player-shot height and w-band boundaries;
- player collision boundaries;
- height-based score boundaries;
- one-enemy accounting;
- fresh-Egg reconstruction after life loss;
- Fuseball prevents premature completion;
- finite rendering geometry / animation frame behavior.

Do not duplicate broad life/reset, projection, exit, or accounting tests already
covered elsewhere.

Run focused Fuseball tests during development.

Do not implement:

- Tanker-carried Fuseballs;
- Super Zapper / ZapIt;
- rimit hurry-up;
- Pulsars;
- full level progression;
- audio or bonuses;
- Spiker/Tanker/Flipper changes;
- suspected overlapping-Flipper behavior.

AI ARCHIVE

Save the task specification to:

AI-Logs/Prompts/2026-10-30-fuseball-implementation-prompt.md

Save a concise report to:

AI-Logs/Reports/2026-10-30-fuseball-implementation-report.md

Report only:

1. Source behavior implemented
2. Development scenario
3. Files changed
4. Important design decisions
5. Tests added and test result
6. Manual validation instructions
7. Deferred/uncertain arcade behavior

Do not commit or push.
