Analyze the next Tempest development milestone after checkpoint:

37535eb Implement Pulsar behavior and add behavior specification

Repository:
C:\Users\nrnob\OneDrive\Repro\Tempest

ANALYSIS ONLY.
Do not modify gameplay code, tests, README, TEMPEST-BEHAVIOR-SPEC.md,
project/configuration files, or Git state.
Do not implement anything yet.

FIRST inspect the CURRENT project, including:

- TEMPEST-BEHAVIOR-SPEC.md
- current C# gameplay/rendering code
- SimulationTests.cs
- README.md
- TEMPEST-DEVELOPMENT-NOTES.md
- Reference/incitti/tempest.bmx
- relevant AI-Logs analysis/implementation reports

Treat TEMPEST-BEHAVIOR-SPEC.md as the current behavioral authority for this
project. Preserve its evidence model:

- ARCADE VERIFIED
- REFERENCE VERIFIED
- PROJECT DECISION
- DEVELOPMENT ONLY

Do not treat Incitti behavior as proof of Atari arcade behavior.

GOAL

Determine the best next SMALL, TESTABLE milestone now that direct Flipper,
Spiker, Tanker, Fuseball and Pulsar development behavior is implemented.

Re-evaluate rather than automatically accepting an older recommendation.

Evaluate at least:

1. Super Zapper
2. rimit / hurry-up behavior
3. beginning real level progression/composition
4. Level 2 / canflip behavior
5. Tanker-carried Pulsars
6. Tanker-carried Fuseballs
7. remaining enemy-bullet activation/promotion decisions
8. remaining Spiker/Tanker behavior gaps
9. unresolved Flipper direct-rim-arrival arcade validation implications
10. any smaller missing core mechanic revealed by the current code/specification

For each candidate determine:

- prerequisite systems
- implementation complexity
- behavior dependencies
- rendering impact
- accounting impact
- death/re-entry impact
- progression dependencies
- deterministic testability
- ability to manually validate in isolation
- whether implementation would prematurely introduce architectural machinery
- what behavior is Incitti-derived versus arcade-verified
- whether the milestone would materially improve the playable recreation

Pay special attention to Super Zapper now that Fuseballs and Pulsars exist.
If recommending it, analyze exact source behavior including:
- activation/input
- charge count/recharge
- first versus subsequent use
- enemy targeting/destruction behavior
- delayed effects
- scoring
- Tanker cargo consequences
- which enemies/projectiles/spikes are affected or unaffected
- death/re-entry and completion consequences
- required HUD/rendering behavior
- how it could be validated without prematurely building full progression

Also reconsider whether implementing progression before Super Zapper would
produce a more useful or faithful next playable milestone.

BEHAVIOR SPECIFICATION

For whichever milestone is ultimately recommended, identify what portions of
TEMPEST-BEHAVIOR-SPEC.md would eventually need updating AFTER implementation
and manual validation.

Do not update the specification during this analysis.

OUTPUT

Recommend exactly ONE next milestone.

Explain:
- why it should be next;
- why the competing candidates should wait;
- expected files involved;
- automated-test strategy;
- manual-validation strategy;
- behavior-specification impact;
- explicit out-of-scope items;
- risks/uncertainties.

Save this prompt to:

AI-Logs/Prompts/2026-10-30-next-milestone-after-pulsar-prompt.md

Save the analysis to:

AI-Logs/Reports/2026-10-30-next-milestone-after-pulsar-report.md

AI-Logs remains gitignored.

STOP after analysis and report generation.
Do not implement.
Do not commit.
