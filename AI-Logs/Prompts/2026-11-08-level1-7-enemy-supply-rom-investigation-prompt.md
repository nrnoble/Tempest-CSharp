FAIL-CLOSED PROMPT CHECK

Read this entire user-provided task before doing anything.

Before using tools, reading files, running commands, modifying files, or taking
any repository action, locate the unique completion token near the end of this
user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a
line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment
metadata, or other system-generated context may appear after the user's token
and do not invalidate it.

This check is intended to detect an incomplete or truncated USER task, not
appended agent context.

If exactly one complete TEMPEST_ token cannot be found, stop before any project
or repository action and explain that the supplied user task appears incomplete.


TASK

Perform a tightly bounded, read-only investigation of enemy supply/counts for
the currently implemented production Levels 1-7.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected accepted baseline:

d2d8b1ef69b4a13be06ab59b41a1c9d38a8dd6e2
Close out accepted common type-0 destruction effect

Confirm the actual Git state before beginning.

Do not modify production code, tests, durable documentation, configuration,
project files, Git state, screenshots, recordings, or UserData.

Do not stage, commit, push, clean, rename, move, or delete anything.


USER-OBSERVED PROBLEM

Neal believes the number of enemies appearing on at least some current C#
levels does not match the original arcade game.

It is not yet known whether the discrepancy comes from:

- total original enemy supply;
- enemy-type mix;
- initial Eggs versus reserve supply;
- scheduling/release rate;
- Tankers carrying Flippers;
- direct Flipper entry versus Tanker delivery;
- Tanker cargo accounting;
- reconstruction after Blaster death;
- enemies remaining/visible counts;
- or some other part of the wave model.

Do not assume that Neal's observation proves the configured total is wrong.

The purpose of this task is to determine exactly how both the accepted C#
project and the Atari ROM determine enemy supply before proposing corrections.


IMPORTANT: REUSE PREVIOUS PROJECT ANALYSIS

Substantial Tanker/Flipper, Level 3 mixed-wave/ledger, death/reconstruction,
completion-accounting, and Level 7 supply work has already been performed in
this project.

Do not start from scratch.

Before new analysis, locate and review the relevant existing evidence,
including as applicable:

- TEMPEST-DEVELOPMENT-NOTES.md
- TEMPEST-BEHAVIOR-SPEC.md
- Level configuration/initialization code
- Level3MixedWaveTests.cs
- Level3WaveLedgerTests.cs
- Tanker-related tests
- progression tests
- Level 7 tests, including Level7FlipperMovementTests.cs where relevant
- previous ignored AI-Logs reports concerning Level 3 mixed waves, Tankers,
  cargo identities, enemy supply, and Level 7 arcade corrections
- other directly relevant tests/reports found in the repository.

Use prior accepted findings rather than independently reinventing the identity
model.

If an earlier report conflicts with current durable documentation, accepted
tests, current production code, or better ROM evidence, identify the conflict
explicitly rather than silently choosing one.


LOCAL ROM SOURCE

Use the project-local ROM-derived annotated disassembly:

Reference/arcade/Tempest Rom Source code.md

This local source has been used repeatedly in previous Tempest work.

Do not search the internet for another Tempest disassembly.

Do not use Mark Incitti's implementation or another port to establish enemy
counts when the local ROM material answers the question.

Atari ROM behavior/data takes precedence over secondary implementations unless
Neal's directly observed arcade behavior establishes otherwise.


EVIDENCE CLASSIFICATION

Clearly classify material as appropriate:

- ROM-LISTING-BACKED
- PREVIOUSLY ACCEPTED PROJECT BEHAVIOR
- EXISTING C# IMPLEMENTATION
- AUTOMATED-TEST EVIDENCE
- ARCADE-OBSERVED
- PROJECT DECISION
- INTERPRETATION / HYPOTHESIS
- UNKNOWN / UNRESOLVED

Do not describe inferred totals, identities, or relationships as ROM-backed
unless the local ROM listing actually establishes them.


CORE QUESTION 1: HOW DOES THE C# PROJECT DETERMINE SUPPLY?

Trace the complete current production path for Levels 1 through 7.

For each level determine:

- where its configuration originates;
- how many original enemy identities exist;
- how many enemies begin as initial Eggs or equivalent initial objects;
- how many remain in reserve;
- how reserve enemies are selected/released;
- which enemy types can enter directly from the bottom;
- how Tankers enter;
- how Tanker cargo is represented;
- whether cargo Flippers are counted as independent original enemies or as
  identities owned/represented through the Tanker model;
- how Spikers are represented;
- how Pulsars are represented;
- how Fuseballs are represented;
- how starting Spikes relate to enemy supply;
- any active-enemy cap;
- any release/scheduling mechanism that can make a correct total appear
  visually too large or too small;
- how death/re-entry reconstructs unresolved supply;
- how level completion determines that all required enemy identities have been
  resolved.

Do not equate "number visibly entering the tube" with "number of original
enemy identities" unless the code actually establishes that equivalence.


CORE QUESTION 2: WHAT DOES THE ROM SPECIFY?

Trace the local ROM code and tables responsible for enemy population/supply
for the corresponding arcade Levels 1-7.

Identify concrete addresses, tables, constants, indexing expressions and
control flow.

Determine, where the listing permits:

- whether the ROM gives a literal total enemy count per level;
- whether supply is derived from a formula or progression;
- whether level/tube number indexes one or multiple tables;
- enemy-type distribution;
- Egg creation;
- reserve supply;
- order or rules governing which enemy type enters next;
- Tanker supply;
- Tanker cargo accounting;
- direct Flipper supply versus Tanker-carried Flippers;
- Spiker supply;
- Pulsar supply;
- Fuseball supply;
- starting Spikes versus Spiker-created Spikes;
- active-enemy limits;
- replacement/release behavior;
- death/re-entry reconstruction;
- level-completion accounting.

Pay particular attention to any ROM tables that previous Level 3 or Level 7
analysis already identified.

Do not assume the same supply interpretation applies identically to every
level.


CORE QUESTION 3: RECONCILE PRIOR ACCEPTED WORK

Specifically investigate previous accepted findings involving:

LEVEL 3:
- mixed-wave identity accounting;
- Tanker parent/cargo ownership;
- original versus cargo identities;
- initial Eggs;
- reserve supply;
- death/re-entry reconstruction;
- completion blockers;
- conservation of unresolved identities.

LEVEL 7:
- previously established ROM-backed original supply;
- initial Eggs and reserve;
- Tanker-only versus direct-Flipper entry conclusions;
- Tanker cargo;
- ordinary Flipper behavior versus cargo Flipper behavior;
- any previous manually accepted Level 7 observations relevant to supply.

Determine whether those earlier findings remain consistent with:

1. current production code;
2. current durable documentation;
3. current tests;
4. the local ROM listing.

Do not modify or overturn previously accepted behavior merely because a
different interpretation appears simpler.


LEVEL-BY-LEVEL MATRIX

Create a detailed comparison matrix for production Levels 1-7.

At minimum include separate fields for:

- Level
- C# total original supply
- C# initial supply/Eggs
- C# reserve supply
- C# direct Flipper supply
- C# Tanker supply
- C# Tanker cargo representation
- C# Spiker supply
- C# Pulsar supply
- C# Fuseball supply
- C# starting Spikes
- ROM-derived corresponding values/rules
- exact discrepancy, if any
- confidence/evidence classification.

If a single numeric value cannot legitimately be assigned to a category,
describe the rule instead of manufacturing a number.

Distinguish carefully among:

A. original enemy identities;
B. carrier Tankers;
C. latent Tanker cargo;
D. materialized cargo Flippers;
E. active/visible enemies at a particular moment;
F. reserve enemies not yet spawned;
G. enemy projectiles;
H. Spikes.

Do not count enemy projectiles or Spikes as enemies merely because they are
shot targets.


TOTALS AND USER EXPERIENCE

The user-observed symptom is that the arcade and C# games appear to present
different numbers of enemies.

After establishing the underlying data, analyze whether each discrepancy is
actually caused by:

- incorrect total supply;
- incorrect type distribution;
- incorrect carrier/cargo representation;
- direct spawning where cargo delivery should occur;
- scheduling/release pacing;
- different active-enemy caps;
- HUD/count presentation;
- death/re-entry reconstruction;
- completion accounting;
- or another documented mechanism.

Keep factual findings separate from hypotheses.


OPTIONAL READ-ONLY PLAYBACK ANALYSIS

The project now has accepted deterministic gameplay recording/playback,
headless playback, pause/resume, forward/reverse scrubbing, and screenshots.

Use existing recordings only if doing so adds meaningful evidence.

Do not create new recordings or launch the interactive game.

If existing headless playback can cheaply expose useful counts/state without
modifying anything, it may be used as supplementary EXISTING C# evidence.

Do not treat a deterministic C# playback as evidence of arcade behavior.


NO IMPLEMENTATION

This prompt is investigation-only.

Do not:

- change enemy counts;
- change level configuration;
- change spawning;
- change Egg behavior;
- change Tanker behavior;
- change cargo ownership;
- change Flipper behavior;
- change Spiker/Pulsar/Fuseball behavior;
- change starting Spikes;
- change completion logic;
- change death/re-entry;
- change HUD counts;
- add diagnostics;
- modify tests;
- update durable documentation;
- launch the interactive game;
- commit or push.

The purpose is to understand the discrepancy before touching accepted gameplay.


VALIDATION

Read-only code searches, ROM inspection, existing test inspection, and narrowly
useful read-only commands are allowed.

Run existing tests only if they answer or confirm a specific investigation
question.

Do not run the full test suite or build merely as ritual for a no-change
investigation.

At the end verify:

- HEAD remains unchanged;
- no tracked or staged changes were created;
- existing untracked/local files remain untouched.


REPORT

Preserve this complete substantive prompt under the ignored AI-Logs/Prompts
directory according to project policy.

Create a paired date-prefixed report under ignored AI-Logs/Reports.

The report must contain:

1. Starting Git state and baseline.
2. Prior analyses/reports/tests reviewed.
3. Current C# supply architecture.
4. Current C# Level 1-7 supply matrix.
5. Local ROM routines/tables/addresses governing supply.
6. ROM Level 1-7 supply matrix/rules.
7. Reconciliation with prior Level 3 work.
8. Reconciliation with prior Level 7 work.
9. Exact C# versus ROM discrepancies.
10. Separation of total-supply differences from spawn/release-pacing
    differences.
11. Death/re-entry and completion-accounting implications.
12. Findings that are already correct and must not be disturbed.
13. Remaining unknowns.
14. Recommended correction order, if discrepancies are proven.
15. Proposed smallest first implementation milestone, but no implementation.
16. Suggested automated acceptance tests for that future correction.
17. Suggested manual arcade-fidelity checks.
18. Final Git state confirming no tracked changes.


RECOMMENDATION REQUIREMENTS

Do not simply recommend "make all Levels 1-7 match the ROM."

If discrepancies exist, rank future corrections by risk and dependency.

Prefer:

- correcting one clearly proven supply discrepancy at a time;
- preserving previously accepted mechanics;
- implementing and validating one level at a time when practical;
- manual arcade acceptance before closeout;
- avoiding simultaneous changes to supply, spawn pacing, movement, and
  completion unless ROM control flow proves they are inseparable.

If the investigation instead shows that totals are already correct but the
user-observed difference is primarily spawn pacing or carrier/cargo
presentation, say so explicitly and recommend investigation of that mechanism
rather than changing totals.


STOP POINT

Stop after the read-only investigation and recommendation.

Do not implement anything.

At completion, provide Neal a concise summary answering:

- How does the current C# project determine enemy supply?
- Does the ROM establish different supply/counts for Levels 1-7?
- Which C# levels, if any, have proven count/supply discrepancies?
- Are any apparent discrepancies actually Tanker/cargo or spawn-pacing issues?
- Do previous Level 3 and Level 7 accepted findings remain valid?
- What is the smallest safe correction milestone to do next?

TEMPEST_LEVEL1_7_ENEMY_SUPPLY_ROM_INVESTIGATION_20261108