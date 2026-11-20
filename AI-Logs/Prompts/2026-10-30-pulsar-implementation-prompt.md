Implement the direct-hatched Pulsar milestone for Tempest.

Repository:
C:\Users\nrnob\OneDrive\Repro\Tempest

Starting clean checkpoint:
b3bd0d4 Implement direct-hatched Fuseball behavior

FIRST:
Read and use this analysis as the implementation specification:

AI-Logs/Reports/2026-10-30-pulsar-analysis-report.md

Also inspect the CURRENT C# source and the Incitti reference yourself before changing code. If current code or literal reference source contradicts the analysis report, follow the stronger evidence and document the discrepancy.

SCOPE

Implement an isolated --pulsar-test development scenario using direct-hatched Pulsars only.

Do NOT implement:
- normal level progression or Level 17 composition
- Tanker-carried Pulsars
- rimit/hurry-up
- Super Zapper or ZapIt
- audio
- open-tube rendering
- new death-animation systems
- broad enemy/rendering/animation abstractions
- unrelated fixes or refactors

BEFORE IMPLEMENTING

Resolve the two uncertainties identified by the analysis:

1. Determine the actual BlitzMax semantics relevant to:
   20 - lv/6
   16 - lv/6
   when lv = 16 and the result is assigned to the integer pause fields.

Do not guess 18/14.
Use authoritative evidence available in the project/reference or otherwise establish the language semantics reliably.
Record the evidence and resulting exact tick values in the implementation report.

2. Make an explicit decision about the shared Pulsar pulse oscillator during the existing C# PlayerDying freeze.

The choice must be deliberate and documented.
Prefer consistency with the current C# gameplay freeze unless stronger project/reference evidence justifies changing that behavior.
Do not change the established death lifecycle merely to imitate unrelated Incitti presentation behavior.

IMPLEMENTATION REQUIREMENTS

Add --pulsar-test following the established development-scenario pattern.

It must:
- be mutually exclusive with --spiker-test, --tanker-test, and --fuseball-test
- compose with --lives
- use the existing ten-unit Egg/release/accounting model
- hatch direct Pulsars only
- remain on the existing tube
- not enable enemy bullet behavior for Pulsars

Implement Pulsar behavior according to the literal source findings in the analysis, including:
- explicit movement/state machine
- height-zero direct hatch
- ascending and descending movement
- exact 51-update decision boundary
- pauses
- adjacent-lane transition after the appropriate pause
- direction selection toward the player
- closed-tube wrapping
- source-supported open-endpoint model behavior where practical for unit testing, without implementing open-board rendering
- angular flipping behavior
- do not invent a normal transition into source state 2
- source collision boundary behavior
- 200-point shot destruction
- death/re-entry reconstruction
- ordinary one-unit accounting
- completion/exit compatibility

GLOBAL PULSE

Implement one shared pulse oscillator owned by the simulation/game, not one per Pulsar.

Preserve the source boundaries identified by the analysis:
- reset only after phase > 180
- 181 update-call cycle at the default step
- pulsing active only for phase > 70 && phase < 110
- exact pulse_zh calculation and dead zone
- shared state across every Pulsar

Keep simulation state independent of rendering.

RENDERING

Implement the smallest maintainable renderer extension:
- Pulsar six-segment zigzag glyph
- source-derived interpolation/offset behavior
- appropriate pulsing versus non-pulsing color state
- pulsing lane radial-boundary overlay
- only the affected lane receives the overlay
- preserve normal tube rendering underneath

Do not introduce a generic animation framework or redesign VectorRenderer.

COLLISION

Preserve the strict source boundaries described in the analysis.

In particular:
- same-lane requirement
- distinct near-rim and pulsing-lane collision branches
- exact Depth-height < 8 behavior
- exact flip-angle collision boundary
- far-from-rim lane hazard only in applicable movement states and during the global pulse window
- shot collision requires strict:
  0 < Pulsar.Height - Shot.Height < 16
- different lane misses
- preserve the established shot-before-enemy-update/player-collision ordering

MAINTAINABILITY

Use comments where they will help a developer understand the code years later.

Especially comment non-obvious rules such as:
- why >50 means the decision occurs on update 51
- why the global pulse cycle is 181 updates rather than 180
- why lane transfer occurs when it does
- why the electrical collision is lane-wide
- why source state 2 is represented/handled only if needed but is not entered speculatively
- any deliberate difference between Incitti death timing and the current C# death freeze

Avoid comments that merely restate obvious code.

TESTS

Add deterministic tests that cover the analysis report's proposed boundaries, including:

- pulsar-test parsing/exclusivity
- direct hatch and accounting
- ascending/descending exact movement
- top/bottom transitions
- tick 50 versus tick 51 decision behavior
- exact pause expiration
- adjacent-edge movement
- closed wrapping
- open endpoint model reflection where supported
- same-edge direction ties
- angular flip progression/completion
- no accidental state-2 path
- pulse phases 70/71, 109/110, 180/0
- exact 181-update oscillator period
- pulse_zh formula/dead zone
- shared oscillator across multiple Pulsars
- chosen death-freeze oscillator behavior
- Pulsar rendering geometry
- lane overlay boundaries, including height == 16 versus >16
- near-rim player collision
- pulsing-lane collision
- flip-angle collision boundaries
- shot collision exact 0 and 16 boundaries
- 200-point scoring
- death/reconstruction
- unresolved enemy return to Eggs
- destroyed Pulsars remain destroyed
- ten-unit accounting conservation
- completion/exit behavior
- regressions for established scenarios

VALIDATION

Run focused Pulsar tests first.

Then run the full automated test suite.

Run/build sufficiently to establish:
- zero build errors
- test totals
- failures/skips
- any warnings

Do not claim manual/visual validation. Neal will perform that after the automated implementation is reviewed.

Do not commit anything.

REPORTING

Save this exact implementation prompt to:

AI-Logs/Prompts/2026-10-30-pulsar-implementation-prompt.md

Save the implementation report to:

AI-Logs/Reports/2026-10-30-pulsar-implementation-report.md

AI-Logs is intentionally gitignored.

The implementation report must include:
1. exact behavior implemented
2. files changed
3. BlitzMax integer-division conclusion and supporting basis
4. chosen pulse-clock/death-freeze behavior and rationale
5. state-machine decisions and source deviations, if any
6. renderer changes
7. collision/scoring/accounting behavior
8. focused test result
9. full regression test result
10. build result/warnings
11. manual validation command and checklist
12. explicitly deferred behavior
13. any remaining uncertainty requiring Neal's arcade comparison

STOP after implementation, tests, build, and report generation.

Do NOT commit.
Do NOT begin another milestone.
