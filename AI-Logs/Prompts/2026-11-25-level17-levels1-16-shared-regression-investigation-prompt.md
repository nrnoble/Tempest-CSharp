# Tempest Level 17 Candidate — Levels 1–16 Playback, Level 11, and Level 15 Shared-Regression Investigation

## FAIL-CLOSED COMPLETION-TOKEN CHECK

Read the entire user-provided task before taking any action.

Before tools, files, commands, modifications, tests, or repository actions:

1. Locate the completion token near the end of this user-provided task.
2. Verify that exactly one complete token beginning with `TEMPEST-END-` appears on a line by itself in the user-provided task.
3. The token may contain only uppercase ASCII letters, digits, and hyphens after the `TEMPEST-END-` prefix.
4. VS Code/agent-generated reminders, tagged-file information, attachment metadata, current-editor context, or other system-generated context appearing after the user's completion token do not invalidate it.
5. This check exists only to detect an incomplete or truncated user task, not appended agent context.
6. If exactly one complete standalone `TEMPEST-END-` token cannot be found, STOP before any project or repository action and explain why.

Do not require an older underscore-based `TEMPEST_...` token.

The unique completion token for this task is the final standalone line below.

---

# Task

Perform a **read-only deterministic regression investigation** of the current uncommitted Level 17 candidate.

The candidate has produced two serious regression signals in the accepted Levels 1–16 foundation:

1. Neal's previously accepted full campaign recording now diverges visibly beginning around Level 11 and ultimately fails to complete the Levels 1–16 campaign.
2. The full automated suite reports a Level 15 seeded soak failure in which seed 1982 does not observe an ordinary Flipper during the 1,200-tick soak.

Investigate whether these are manifestations of one shared regression introduced by the Level 17 candidate.

This is an investigation only.

**Do not modify production code.
Do not modify tests.
Do not modify durable documentation.
Do not fix the regression.
Do not modify the gameplay recording.
Do not stage, commit, or push.
Do not launch the interactive game.
Do not continue Level 17 feature development.
Do not investigate or implement Level 18+.**

Stop after producing a detailed evidence report and recommended bounded correction scope.

---

# Required prior reports

Read completely:

`AI-Logs/Reports/2026-11-25-level17-bounded-production-implementation-report.md`

`AI-Logs/Reports/2026-11-25-level17-rom-to-csharp-integration-readiness-investigation-report.md`

`AI-Logs/Reports/2026-11-25-production-presentation-default-promotion-closeout-report.md`

Also inspect the current uncommitted Level 17 diff and relevant historical Level 11/Level 15 reports.

Use the expanded implementation report as the change inventory.

Do not repeat Level 17 ROM investigations.

---

# Accepted baseline

The accepted pre-Level-17 baseline is:

`fde09aa31af760958f656aa553555462d51792a0`

This is the production-presentation closeout/report-finalization checkpoint.

The Level 17 candidate is the current uncommitted worktree based on that HEAD.

The accepted foundation includes Levels 1–16 and the deterministic campaign recording:

`TEMP/GameplayRecordings/lvl1-16.json`

Before Level 17 work, this recording completed successfully and deterministically through Level 16.

Do not modify or commit the recording.

---

# New manual regression evidence from Neal

Record this as direct MANUAL REGRESSION EVIDENCE.

Neal replayed:

`TEMP/GameplayRecordings/lvl1-16.json`

against the current Level 17 worktree.

Observed behavior:

- Playback begins visibly failing around Level 11.
- Rim Flippers appear either:
- to stop flipping/moving once on the rim, or
- to stop being rendered.
- Playback continues to execute after the visible divergence.
- Recorded input and game state become out of synchronization.
- The player eventually loses all lives.
- The replay does not successfully complete the recorded Levels 1–16 campaign.

This is a real regression in the accepted foundation.

Do not reduce it to a cosmetic concern.

Do not assume yet whether the defect is simulation-side or rendering-side.

---

# Authoritative campaign-recording pass/fail definition

For this recording, process continuation and exit code alone are not sufficient.

## PASS requires all of the following

- recording parses;
- deterministic playback consumes the recording as expected;
- remaining frames are 0;
- completion reason is `RecordingCompleted`;
- ending displayed level is 16;
- ending Level 16 state is `Complete`;
- no earlier game-over or exhausted-lives terminal state replaces the expected recording boundary.

## FAIL

Anything that does not reach that complete expected boundary is a regression, even if:

- the process exits 0;
- the executable does not crash;
- JSON telemetry is emitted;
- playback continues for many ticks;
- the game eventually reaches another valid terminal state.

Use this end-state gate explicitly.

Do not report a headless run as passing merely because it ran to process completion.

---

# Level 11 / Level 15 historical hypothesis

Neal provides this historical context:

- During Level 15 development, Level 15 followed much of the Level 11 development path.
- Level 15 initially reproduced several bugs that had already been corrected for Level 11.
- Neal considered Level 15's development excessively regression-prone and did not want it used as a model for Level 16.
- The current campaign regression becomes visible around Level 11.
- A separate current automated failure occurs in Level 15.

Treat this as a **high-value diagnostic hypothesis**, not established proof:

> The Level 11 playback divergence and Level 15 soak failure may be manifestations of one regression in a shared Level 11/15 Flipper, Tanker-cargo, ledger, transition, rim-state, admission, or rendering mechanism.

Investigate this directly.

Do not force a shared-cause conclusion if evidence shows two independent failures.

---

# Current automated regression evidence

The expanded Level 17 implementation report establishes:

- focused Level 17/configuration/progression run: 114 passed;
- final startup/remap/Level 17 group: 53 passed;
- full solution: 2,057 passed and 1 failed;
- failing test:
`Level15ProductionSoakTests.SeededProductionSoakConservesIdentitiesAcross1200Ticks(seed: 1982)`;
- selected species set was:
`Spiker, Tanker, Fuseball`;
- ordinary `Flipper` was not observed;
- Release build was not run after the stop condition;
- the full campaign recording was not run during that implementation validation.

Do not weaken the Level 15 test.

Do not assume it is stale.

Determine what it is telling us.

---

# Investigation 1: Preserve and inventory the current worktree

Record:

- branch;
- HEAD;
- upstream;
- ahead/behind;
- staged state;
- tracked modifications;
- untracked files;
- current Level 17 diff;
- pre-existing unrelated changes.

Do not clean, reset, restore, stash, switch the working tree, or overwrite anything.

The current candidate must remain intact.

---

# Investigation 2: Reproduce the campaign failure headlessly

Run the current worktree's Release headless playback on:

`TEMP/GameplayRecordings/lvl1-16.json`

Use an explicit process timeout.

Capture complete available JSON telemetry.

Evaluate the strict pass/fail boundary defined above.

Record:

- process exit;
- completion reason;
- remaining frames;
- current/ending displayed level;
- level state;
- lives;
- score;
- total ticks;
- any available failure/terminal reason.

If the current tooling reports only a final boundary and cannot identify the first divergence, use that result as the initial proof of failure and proceed to bounded instrumentation planning.

Do not modify the recording.

---

# Investigation 3: Confirm deterministic reproducibility

Run the same current-worktree headless playback at least twice with identical inputs.

Determine:

- whether final telemetry is identical;
- whether the same failure boundary occurs;
- whether the same level is reached;
- whether lives/score/state match;
- whether failure is stable.

If runs differ, investigate uncontrolled randomness/configuration before proceeding.

Do not claim deterministic regression without repeatable evidence.

---

# Investigation 4: Compare against the accepted baseline without disturbing the worktree

Run or reconstruct an A/B comparison against baseline commit:

`fde09aa31af760958f656aa553555462d51792a0`

Do not reset or checkout over the current worktree.

Use a safe read-only method such as:

- a temporary Git worktree outside the active project directory;
- building/running an already validated baseline artifact only if provenance is exact;
- another bounded method that preserves the active worktree.

Do not use stale binaries of uncertain provenance.

Use the same recording and equivalent Release/headless command.

Record baseline telemetry and verify baseline PASS against the strict campaign boundary.

If a temporary worktree is created:

- use a clearly temporary location;
- do not copy local user/reference assets beyond the recording needed for the test;
- remove it after evidence is captured if safe;
- do not modify the main worktree.

If baseline cannot be run safely, state that explicitly rather than assuming it passes solely from history.

---

# Investigation 5: Establish the earliest objective divergence

The visible symptom appears around Level 11, but the first simulation divergence may occur earlier.

Determine the earliest objective divergence between baseline and candidate.

Prefer existing deterministic boundaries/fingerprints if available.

If current tooling can compare per-tick or per-boundary fingerprints, use it.

If not, identify the smallest **temporary, non-production diagnostic** needed to compare:

- level boundaries;
- random-state fingerprints;
- gameplay state;
- ledger identity sets;
- active enemies;
- player state.

This task is read-only with respect to production/tests/durable docs.

Temporary diagnostic scripts or output under ignored `TEMP/` may be created only if needed and must not modify production behavior.

Do not commit temporary diagnostics.

Use bounded binary search over ticks/boundaries if supported.

Report:

- last matching boundary/tick;
- first differing boundary/tick;
- displayed level;
- level state;
- relevant state differences.

If exact first divergence cannot be found, identify the narrowest confirmed divergence interval.

---

# Investigation 6: Compare random consumption

Because Level 15's selected species changed, compare random-number consumption between baseline and candidate.

Determine whether the Level 17 candidate causes Levels 1–16 to consume additional, fewer, or reordered random draws.

Inspect specifically:

- construction of level configurations;
- shape/palette resolution;
- presentation initialization;
- wave selectors;
- Flipper route initialization;
- Tanker cargo initialization;
- Pulsar-related branches;
- ledger validation/reconstruction;
- any generic logic that now considers Pulsars before Level 17;
- direct/default configuration creation.

Identify any shared change that executes for Levels 1–16 despite being intended only for Level 17.

Capture random-state/fingerprint evidence at:

- campaign start;
- each level transition;
- Level 11 entry;
- first confirmed divergence;
- Level 15 entry/soak setup.

Do not add permanent instrumentation.

---

# Investigation 7: Simulation versus rendering at Level 11

Determine whether the Level 11 visible Flipper symptom is:

## A. Simulation divergence

The Flipper's game state, lane, transition, rim movement, identity or collision behavior differs.

## B. Rendering divergence

Simulation remains equivalent, but the renderer no longer submits/draws/project the Flipper correctly.

## C. Both

A simulation change later produces a rendering symptom.

For the first affected Flipper, compare baseline and candidate:

- stable identity;
- original versus Tanker cargo;
- runtime type;
- source lane;
- destination lane;
- current lane;
- direction/handedness;
- below-rim program;
- transition progress;
- active transition;
- depth/height;
- rim state;
- pursuit state;
- destroyed/terminal state;
- ledger state;
- runtime representation;
- renderer inclusion;
- projected geometry finiteness.

No screenshot/frame analysis is authorized in this task.

Use gameplay state and render-submission data only.

---

# Investigation 8: Shared Level 11/15 mechanics

Map the exact shared Level 11 and Level 15 implementation paths relevant to the symptoms.

At minimum inspect:

- Level 11/15 wave selection;
- ordinary Flipper factory;
- Tanker factory;
- Tanker cargo factory;
- cargo movement-program selection;
- Flipper below-rim program state;
- rim pursuit;
- Flipper rendering;
- identity ledger;
- runtime representation validation;
- death/reconstruction;
- completion;
- random selection/admission.

Determine:

- which shared paths were modified by Level 17;
- whether either level now enters a Level 17 branch accidentally;
- whether new enum values changed defaults, fall-through, exhaustive-switch behavior, indexing, serialization or ordering;
- whether selected-species enumeration/order changed;
- whether Pulsar addition changed random selection even when Pulsars are ineligible;
- whether Straight07 addition changed a default Flipper route;
- whether shape/palette/calibration changes affect only presentation.

Do not assume Level 11 and 15 are identical.

Document exact shared and distinct surfaces.

---

# Investigation 9: Reproduce and classify the Level 15 soak failure

Run the exact failing test alone:

`Level15ProductionSoakTests.SeededProductionSoakConservesIdentitiesAcross1200Ticks(seed: 1982)`

Use explicit VSTest hang timeout.

Repeat it to confirm deterministic stability.

Run the same exact test against the accepted baseline in the safe A/B environment.

Compare:

- species selected;
- random state;
- original identities;
- reserve/Egg;
- active enemies;
- Tanker cargo;
- completion/identity conservation;
- whether ordinary Flipper is required or merely permitted.

Determine whether the failure is:

- production gameplay regression;
- deterministic random-stream drift;
- stale/over-specific invariant;
- test-order/isolation problem;
- shared Level 11/15 regression;
- independent from playback failure;
- unresolved.

Do not revise the test.

---

# Investigation 10: Candidate-diff attribution

Use the expanded implementation report and actual diff to attribute the regression.

Candidate shared files include:

- `LevelConfiguration.cs`
- `LevelProgression.cs`
- `RomPresentationMetadata.cs`
- `LevelLoader.cs`
- `AtariPresentationCalibration.cs`
- `AtariPresentationAdapter.cs`
- `VectorRenderer.cs`
- `Flipper.cs`
- `Tanker.cs`
- `Pulsar.cs`
- `Level3WaveLedger.cs`
- `TempestGame.cs`
- affected tests

New Level 17-only files include:

- `Level17WaveSelection.cs`
- `Level17ProductionTests.cs`

Classify each changed production file:

- cannot affect Levels 1–16 runtime;
- can affect only presentation;
- can affect gameplay state;
- can affect random consumption;
- can affect Level 11/15 shared behavior;
- uncertain until tested.

Then identify the smallest likely regression-inducing hunk(s).

Use safe temporary A/B patch exclusion or targeted diagnostic reasoning where appropriate, but do not modify the active candidate.

If temporary build variants are required:

- create them outside the active worktree;
- apply one bounded candidate-hunk exclusion at a time;
- run the strict campaign oracle;
- preserve exact provenance;
- use explicit process bounds;
- remove temporary environments when complete.

Do not brute-force every diff combination.

---

# Investigation 11: Full campaign pass/fail oracle for future tests

Define the exact automated regression assertion that should permanently protect:

`TEMP/GameplayRecordings/lvl1-16.json`

The local recording must remain uncommitted unless Neal later authorizes fixture inclusion.

Recommend how VSC can run this local file during authorized development validation.

The assertion must fail unless:

- completion reason is `RecordingCompleted`;
- remaining frames are 0;
- ending displayed level is 16;
- Level 16 state is `Complete`.

If the existing headless command does not distinguish early Game Over from successful replay completion, recommend the smallest tooling/test extension needed.

Do not implement it in this investigation.

Also identify useful intermediate assertions, such as:

- expected level-boundary sequence;
- no early Game Over;
- lives remain sufficient;
- deterministic final fingerprint.

Do not overfit to score/lives unless those values are already established stable recording outcomes.

---

# Investigation 12: Relationship to Level 17 visual defects

Neal also observed in manual Level 17 testing:

- old/legacy-looking tube presentation;
- no visibly new color scheme;
- Spikes and Pulsars are present.

The expanded implementation report explains that raw palette 1 was cataloged but existing host-role colors remained unchanged.

For this regression investigation:

- record these as separate Level 17 presentation blockers;
- determine only whether any presentation change involved in those symptoms is also implicated in the Levels 1–16 regression;
- do not investigate or correct host palette mapping;
- do not calibrate Level 17;
- do not implement presentation fixes.

Keep accepted-foundation regression diagnosis as the priority.

---

# Required classification

End with a clear classification for each signal.

## Campaign playback

- confirmed simulation regression;
- confirmed rendering-only regression;
- both;
- unresolved.

## Level 15 soak

- same root cause as campaign;
- related shared drift but different symptom;
- independent real regression;
- stale/over-specific test;
- test-order/isolation issue;
- unresolved.

## Level 17 presentation

- related to campaign regression;
- independent known palette-translation gap;
- independent presentation-mapping defect;
- unresolved.

Do not combine them without evidence.

---

# Required correction recommendation

Recommend the smallest bounded next implementation task.

It may be:

- one shared regression correction;
- separate simulation and rendering corrections;
- production correction plus test correction;
- temporary rollback of one candidate hunk;
- another narrowly scoped evidence task.

The recommendation must:

- restore the strict Levels 1–16 campaign oracle;
- restore or correctly reconcile the Level 15 soak;
- preserve intended Level 17 work where possible;
- avoid continuing Level 17 feature development until the foundation is green.

Do not implement the correction in this task.

---

# Validation and bounded execution

This is an investigation, but it should run the specific diagnostics/tests necessary to establish the facts.

Use explicit bounds for:

- baseline/candidate playback;
- repeated tests;
- temporary builds;
- tick comparisons.

Do not run the entire suite repeatedly.

Do not launch the interactive game.

Do not inspect screenshots/videos/images.

Do not weaken Windows security or bypass Application Control.

If an operation stalls, stop and narrow it.

Canceled/timed-out/blocked runs are not passes.

---

# Required AI artifacts

Save this prompt as:

`AI-Logs/Prompts/2026-11-25-level17-levels1-16-shared-regression-investigation-prompt.md`

Create:

`AI-Logs/Reports/2026-11-25-level17-levels1-16-shared-regression-investigation-report.md`

These are GitHub continuity records, but this investigation does not authorize staging, commit or push.

---

# Required report contents

The report must include:

1. Executive summary
2. Starting Git/worktree state
3. Accepted baseline and candidate identities
4. Manual campaign-regression evidence
5. Strict recording pass/fail oracle
6. Current candidate headless playback result
7. Deterministic repeat result
8. Baseline headless playback result
9. Baseline/candidate final telemetry comparison
10. Last matching and first differing tick/boundary
11. Level at first divergence
12. Random-state comparison
13. Level-transition comparison
14. First affected Flipper identity/state
15. Simulation-versus-rendering determination
16. Level 11 shared-path analysis
17. Level 15 shared-path analysis
18. Exact Level 11/15 common mechanisms
19. Exact Level 11/15 differences
20. Level 15 seed-1982 isolated result
21. Level 15 seed-1982 baseline result
22. Level 15 failure classification
23. Candidate changed-file classification
24. Smallest implicated hunk(s)
25. Temporary A/B variants used, if any
26. Relationship between campaign and soak failures
27. Relationship to Level 17 visual defects
28. Recommended campaign regression assertion/tooling
29. Smallest bounded correction recommendation
30. Remaining uncertainties
31. Commands/tests run and bounds
32. Temporary worktree/tool cleanup
33. Production code changed: no
34. Tests changed: no
35. Durable docs changed: no
36. Recording changed: no
37. Stage/commit/push: none
38. Level 17 feature work continued: no
39. Level 18+ work: none
40. Recommended next step

Do not omit details because a stop condition is reached. If a diagnostic cannot be completed, document exactly what was completed and what blocked the remainder.

---

# Stop conditions

Stop and report if:

- baseline cannot be reproduced safely;
- current playback failure is nondeterministic;
- temporary A/B environment cannot preserve provenance;
- recording is missing/corrupt;
- first-divergence instrumentation requires production changes;
- Windows security blocks required bounded diagnostics;
- worktree state prevents safe comparison.

Do not guess.

---

# Completion criteria

This investigation is complete when it establishes, or narrowly bounds:

- whether the campaign failure is simulation or rendering;
- earliest divergence;
- whether random consumption changed;
- whether Level 11 and Level 15 share the root cause;
- whether the Level 15 test reflects a real regression;
- which Level 17 candidate hunk(s) caused the issue;
- the strict recording regression oracle;
- the smallest correction task.

Then STOP.

Do not implement the correction.

TEMPEST-END-LEVEL17-LEVELS1-16-SHARED-REGRESSION-INVESTIGATION-COMPLETE