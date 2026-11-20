We are beginning analysis of the next Tempest milestone: direct-hatched Pulsars.

Current clean checkpoint:
b3bd0d4 Implement direct-hatched Fuseball behavior

IMPORTANT:
This is ANALYSIS ONLY.
Do not modify source code, tests, README, project files, or configuration.
Do not implement Pulsars yet.

Repository:
C:\Users\nrnob\OneDrive\Repro\Tempest

Analyze the CURRENT C# project at b3bd0d4 and compare it carefully with the Incitti Tempest reference source already stored in the project.

Primary goal:
Determine the smallest maintainable Pulsar milestone that can be implemented and manually validated in isolation before introducing normal level progression.

Investigate:

1. Incitti Pulsar creation/direct-hatch behavior.
2. Every Pulsar movement state and transition.
3. Up/down movement rates and random decision cadence.
4. Flipping behavior.
5. Mid-tube versus rim pause behavior.
6. Global pulse timing/oscillator behavior.
7. pulse_zh or equivalent geometry/animation behavior.
8. The relationship between a Pulsar and its tube lane/edge while pulsing.
9. Exactly how Pulsars alter tube/web rendering.
10. Player collision:
   - ordinary near-rim contact
   - electrified/pulsing-lane collision
11. Player-shot collision and scoring.
12. Death/life-loss behavior and reconstruction.
13. Enemy accounting and level-completion behavior.
14. Open versus closed tube behavior.
15. Randomness that must be injectable/deterministic for tests.
16. Whether existing C# rendering architecture can support Pulsars cleanly or whether a small extension is required.
17. Existing C# systems/classes that should be reused rather than duplicated.
18. Any interactions with Fuseballs, Tankers, Spikers, Flippers, EnemyBullets, Eggs, or existing death/exit logic.
19. Whether rimit/hurry-up behavior is actually required for this first Pulsar milestone or can remain deferred.
20. Any dependence on Super Zapper, progression, audio, bonuses, or other unfinished mechanics.

Development scenario:
Evaluate adding a dedicated --pulsar-test scenario comparable to --fuseball-test.
The initial milestone should preferably use direct-hatched Pulsars only and should not introduce Tanker-carried Pulsars or full level progression unless the source architecture makes that unavoidable.

Maintainability:
Follow the project's established maintainability standard. Recommend clear responsibilities and useful code comments for non-obvious state transitions, timing rules, collision rules, and rendering behavior. Avoid speculative abstractions or premature general frameworks.

Arcade accuracy:
Distinguish clearly between:
A. behavior explicitly established by the Incitti source,
B. engineering conclusions inferred from source/current C# architecture,
C. behavior requiring later manual comparison with Atari arcade Tempest.

Do not assume that the Incitti implementation is automatically arcade-accurate.

Testing:
Propose deterministic automated tests covering state transitions, timing boundaries, pulse windows, rendering state, collision boundaries, scoring, life reconstruction, accounting, and regression behavior.

Manual validation:
Provide a concise gameplay checklist for what Neal should visually compare after implementation.

Scope control:
Explicitly identify what should remain OUT OF SCOPE for this milestone.

Deliverable:
Save the analysis report as:

AI-Logs/Reports/2026-10-30-pulsar-analysis-report.md

Also save this prompt under:

AI-Logs/Prompts/2026-10-30-pulsar-analysis-prompt.md

AI-Logs is intentionally gitignored.

At the end, summarize:
- recommended implementation architecture
- files expected to change
- tests expected to add
- manual validation procedure
- risks/uncertainties
- explicit out-of-scope items

STOP after writing the report and prompt archive.
Do not make implementation changes.
