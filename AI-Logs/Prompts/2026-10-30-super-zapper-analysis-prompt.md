Analyze the Super Zapper milestone for Tempest.

Repository:
C:\Users\nrnob\OneDrive\Repro\Tempest

Starting clean checkpoint:
587b9e3 Implement Fuseball and Pulsar hurry-up behavior

ANALYSIS ONLY.

Do not modify gameplay code, tests, README, TEMPEST-DEVELOPMENT-NOTES.md,
TEMPEST-BEHAVIOR-SPEC.md, project/configuration files, or Git state.
Do not implement Super Zapper yet.
Do not commit.

FIRST READ

- TEMPEST-BEHAVIOR-SPEC.md
- README.md
- TEMPEST-DEVELOPMENT-NOTES.md
- current C# gameplay/rendering/input code
- tests/Tempest.Game.Tests/SimulationTests.cs
- Reference/incitti/tempest.bmx
- relevant AI-Logs analysis/implementation reports

Especially read:

AI-Logs/Reports/2026-10-30-next-milestone-after-pulsar-report.md

Treat the current behavior specification and current code as newer project
state than historical analysis reports.

EVIDENCE MODEL

Preserve:

ARCADE VERIFIED
REFERENCE VERIFIED
PROJECT DECISION
DEVELOPMENT ONLY

Incitti establishes reference behavior, not Atari arcade truth.

GOAL

Produce an implementation-ready Super Zapper design based on the CURRENT
post-Hurry-Up codebase.

Do not simply repeat the older milestone analysis.

Resolve the exact behavior and safest integration design before any code is
changed.

ANALYZE SOURCE BEHAVIOR

Establish from literal Incitti source:

1. Activation inputs and eligibility.
2. Number of charges.
3. Charge initialization/recharge lifecycle.
4. Behavior of first use versus second use.
5. Whether a charge is consumed when no valid target is found.
6. First-use target iteration/order.
7. First-use staggered delay assignment.
8. Second-use target-selection algorithm exactly as written.
9. Continuous-tube lane-distance handling.
10. Height/type tie and comparison behavior.
11. Which enemy/entity types accept ZapIt.
12. Which reject or are excluded:
    - enemy bullets
    - spikes
    - anything else
13. Per-enemy delayed-zap behavior:
    - Flipper
    - Tanker
    - Spiker
    - Fuseball
    - Pulsar
14. Exact scoring at delayed expiry.
15. Whether enemies continue ordinary movement/behavior while zap delay runs.
16. What happens if an enemy otherwise resolves before its zap timer expires.
17. Tanker zap behavior.
18. Stillborn Tanker cargo:
    - creation
    - child identity/accounting
    - initial state
    - delayed destruction
    - child scoring
    - interaction with original Tanker slot
19. Player death while zaps are pending.
20. Life reconstruction of unresolved zapped enemies/cargo.
21. Charge persistence across ordinary life loss.
22. Charge reset on new game/level/replay under the CURRENT project's lifecycle.
23. Completion behavior while delayed zap targets remain.
24. Tube/web flash timing and conditions.
25. Any source HUD representation of remaining charges.
26. Audio hooks, while keeping audio out of scope unless behavior depends on it.

CURRENT C# ARCHITECTURE

Inspect the current post-587b9e3 project and identify:

- existing SuperZapperSystem scaffold, if any;
- current PlayerInput/InputSystem design;
- cleanest way to represent zap input;
- existing enemy classes and whether a small shared zap contract is justified;
- whether adding zap state to the existing Enemy abstraction is cleaner than
  per-enemy duplicated state;
- interaction with developmentCollisionOrder;
- ScoreSystem integration;
- active enemy accounting;
- current Tanker original-slot/cargo ledger;
- death reconstruction;
- HurryUp;
- completion/exit;
- VectorRenderer;
- replay/new-game reset.

Do not introduce a broad enemy hierarchy rewrite.

If a shared abstraction is recommended, keep it minimal and justify why
Super Zapper requires it.

TANKER RISK ANALYSIS

Treat Tanker stillborn cargo as the highest-risk portion.

Explicitly design how to preserve:

- original Tanker slot identity;
- latent cargo before zap expiry;
- materialized cargo identities after zap expiry;
- cargo score resolution;
- destroyed/escaped accounting;
- player death before Tanker zap expiry;
- player death after Tanker expires but before stillborn cargo resolves;
- no duplication or disappearance of unresolved enemy units;
- conserved accounting invariants.

If Tanker stillborn cargo makes the milestone too large to implement safely as
one step, determine whether Super Zapper can be divided into independently
correct milestones WITHOUT introducing intentionally wrong temporary behavior.

Do not defer required behavior merely to make implementation easier unless the
intermediate state is explicitly safe and cannot be mistaken as completed
Super Zapper behavior.

DEVELOPMENT VALIDATION

Determine how Super Zapper should be exercised using existing scenarios:

- ordinary Level 1 / Flippers
- --spiker-test
- --tanker-test
- --fuseball-test
- --pulsar-test
- enemy-bullet combinations where relevant
- --lives 10 for manual testing

No full level progression should be required merely to validate Super Zapper.

AUTOMATED TEST PLAN

Design deterministic tests for:

- initial two-charge state;
- ordinary life loss not replenishing charges;
- replay/new-game reset behavior;
- first-use all-target selection;
- exclusions/rejections;
- exact 4,6,8,... accepted-target delay sequence;
- rejected targets not advancing that sequence;
- second-use selection algorithm and tie boundaries;
- charge consumption with/without a valid single target;
- enemy movement while zap delay counts down;
- exact expiry timing;
- per-enemy score;
- enemy/accounting resolution only at expiry;
- Fuseball score-popup behavior after zap;
- Pulsar zap behavior;
- Spiker/spike separation;
- bullets remaining unaffected;
- Tanker stillborn cargo ledger and child scoring;
- death/re-entry during every important pending-zap phase;
- completion blocked until appropriate delayed resolutions finish;
- web/tube flash duration;
- no regression to HurryUp behavior;
- existing enemy-development scenarios remain valid.

MANUAL VALIDATION PLAN

Specify what Neal should observe for each useful scenario.

Separate:
- behavior that can be reasonably seen by eye;
- source-derived exact values that automated tests can establish but manual
  gameplay should not be expected to measure precisely;
- behavior that requires comparison with Atari Tempest before being promoted
  to ARCADE VERIFIED.

BEHAVIOR SPECIFICATION

Identify what sections of TEMPEST-BEHAVIOR-SPEC.md would need updating AFTER
implementation and Neal's manual validation.

Do not edit the specification during analysis.

OUTPUT

Conclude whether Super Zapper should be:

A. one implementation milestone,
B. a deliberately staged set of milestones,
or
C. deferred in favor of another prerequisite.

Make one concrete recommendation and explain why.

Include:

- recommended architecture;
- exact scope;
- expected files;
- automated-test plan;
- manual-validation plan;
- Tanker accounting design;
- input/rendering design;
- lifecycle/reset design;
- behavior-spec impact;
- risks/uncertainties;
- explicit out-of-scope items.

Save this exact prompt to:

AI-Logs/Prompts/2026-10-30-super-zapper-analysis-prompt.md

Save the analysis to:

AI-Logs/Reports/2026-10-30-super-zapper-analysis-report.md

AI-Logs remains gitignored.

STOP after analysis/report generation.

Do not implement.
Do not modify tracked files.
Do not commit.
