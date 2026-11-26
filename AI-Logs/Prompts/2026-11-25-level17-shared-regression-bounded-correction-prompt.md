# Tempest Level 17 Candidate — Levels 1–16 Shared Regression Isolation and Bounded Correction

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

Identify and correct the shared gameplay regression introduced by the current uncommitted Level 17 candidate into the accepted Levels 1–16 production foundation.

This is a **bounded regression-isolation and correction milestone**.

The regression has already been proven.

Do not repeat the broad investigation.

Use temporary A/B hunk exclusion or similarly controlled variants to identify the causal shared change.

Only after causality is established may you make the smallest production correction necessary to restore the accepted Levels 1–16 behavior while preserving intended Level 17 work.

Do not continue unrelated Level 17 development.

Do not correct Level 17 palette/presentation issues in this task.

Do not implement Level 18+.

Do not stage, commit, or push.

Do not launch the interactive game.

Stop after the correction and automated validation requested below.

---

# Required reports

Read completely before taking project action:

`AI-Logs/Reports/2026-11-25-level17-bounded-production-implementation-report.md`

`AI-Logs/Reports/2026-11-25-level17-levels1-16-shared-regression-investigation-report.md`

Also consult:

`AI-Logs/Reports/2026-11-25-level17-rom-to-csharp-integration-readiness-investigation-report.md`

only as needed to distinguish intended Level 17 behavior from accidental prior-level behavior.

The shared-regression investigation is the immediate diagnostic authority.

Do not redo its already established baseline/candidate comparison merely for ceremony.

---

# Established regression facts

Treat these as established unless direct evidence demonstrates a contradiction.

## Accepted baseline

Commit:

`fde09aa31af760958f656aa553555462d51792a0`

The accepted baseline completes:

`TEMP/GameplayRecordings/lvl1-16.json`

at:

- completion reason `RecordingCompleted`;
- remaining frames `0`;
- displayed Level `16`;
- Level state `Complete`;
- lives `9`;
- score `134039`;
- random seed `1990198549`;
- recording length `35350` ticks.

## Current candidate

The current uncommitted Level 17 worktree deterministically ends the same recording at:

- completion reason `RecordingCompleted`;
- remaining frames `0`;
- displayed Level `13`;
- Level state `GameOver`;
- lives `0`;
- score `96636`;
- same random seed `1990198549`;
- same `35350` recording ticks.

Therefore:

**`RecordingCompleted` by itself is not a PASS.**

The accepted campaign boundary must be restored.

## First confirmed campaign divergence

The baseline/candidate objective state matches through boundary:

`19792`

inside Level 11.

The first sampled objective difference is boundary:

`19793`

Baseline:

- Level 11 / Playing;
- score `75675`;
- 2 Flippers;
- 0 Tankers;
- 3 Spikers.

Candidate:

- Level 11 / Playing;
- score `75525`;
- 3 Flippers;
- 0 Tankers;
- 3 Spikers.

By boundary 19799:

- candidate: 2 active Flippers / 0 Tankers;
- baseline: 1 active Flipper / 1 Tanker.

This is a simulation-state divergence, not merely presentation.

## Level 15 secondary oracle

Exact test:

`Level15ProductionSoakTests.SeededProductionSoakConservesIdentitiesAcross1200Ticks(seed: 1982)`

Accepted baseline:

**PASS**

Current candidate:

**FAIL**

Candidate selected set:

`Spiker, Tanker, Fuseball`

Expected/baseline includes:

`Flipper`

The failure is deterministic.

It is not currently classified as a stale test.

## Relationship

The Level 11 campaign regression and Level 15 soak failure both involve the shared Level 11/15 runtime family:

- Flipper/Tanker lifecycle;
- Tanker cargo;
- generic ledger;
- admission/hatching;
- random selection;
- reconstruction;
- representation;
- rim behavior.

A common causal hunk is likely but not yet proven.

---

# Level 17 presentation blockers are OUT OF SCOPE

Neal also observed:

- Level 17 looks like the old/legacy-looking tube presentation;
- the expected Levels 17–32 visible color scheme is absent;
- Spikes and Pulsars are visible.

The regression investigation classified the first two as independent presentation issues.

Do not fix them here.

Do not modify:

- Level 17 host palette mapping;
- Shape 1 presentation calibration;
- Atari presentation mapping;
- renderer color translation;
- Level 17 tube placement.

Restore the accepted Levels 1–16 simulation foundation first.

---

# Starting-state safety

Before editing:

- record branch;
- HEAD;
- upstream;
- ahead/behind;
- staged state;
- tracked modifications;
- untracked files;
- pre-existing unrelated changes.

Preserve:

- the current Level 17 candidate;
- pre-existing `TEMPEST-BEHAVIOR-SPEC.md` modification;
- local recording;
- local ROM/reference files;
- AI-Logs;
- other unrelated assets.

Do not clean/reset/stash/restore the active worktree.

Temporary A/B environments may be created outside the active worktree.

---

# Primary regression oracles

Use these two fast oracles before any full-suite work.

## Oracle A: full Levels 1–16 recording

File:

`TEMP/GameplayRecordings/lvl1-16.json`

PASS requires all:

- process succeeds;
- completion reason `RecordingCompleted`;
- remaining frames `0`;
- ending displayed Level `16`;
- Level state `Complete`;
- lives greater than `0`;
- no earlier GameOver replacing the expected campaign state.

For comparison against the historical accepted outcome, also report:

- final lives;
- final score;
- random seed.

Do not modify or commit the recording.

## Oracle B: Level 15 seed-1982 soak

Run exactly:

`Level15ProductionSoakTests.SeededProductionSoakConservesIdentitiesAcross1200Ticks(seed: 1982)`

PASS requires the existing test to pass without weakening or changing it.

Use explicit VSTest hang timeout.

---

# Phase 1: Reconfirm candidate oracle failures only as needed

Do not spend time reproducing evidence already established multiple times.

Run each current candidate oracle once at the beginning to confirm the active worktree still represents the investigated candidate.

If both outcomes match the prior report, proceed immediately to hunk isolation.

If state differs, stop and reconcile the worktree before attempting correction.

---

# Phase 2: Candidate diff classification

Inspect actual candidate diffs in shared production files.

Priority surfaces:

1. `Level3WaveLedger.cs`
2. `TempestGame.cs`
3. `Flipper.cs`
4. `Tanker.cs`
5. `Pulsar.cs`

Also inspect the introduced Level 17 configuration only to understand branch conditions.

For every relevant hunk classify:

- Level 17-only by explicit guard;
- generic shared code;
- enum/switch expansion;
- ledger validation;
- runtime representation;
- reconstruction;
- hatching/admission;
- random-consumption surface;
- factory/cargo route;
- rim/Flipper movement;
- no possible Levels 1–16 effect.

Do not speculate from filename alone.

---

# Phase 3: Isolate generic ledger/Pulsar hunks first

The prior report recommends testing generic ledger/Pulsar additions first.

Create a temporary A/B candidate variant outside the active worktree that removes or neutralizes only the minimal generic Pulsar/ledger additions necessary to test causality while leaving unrelated Level 17 work intact as far as buildability permits.

Examples of relevant areas:

- new `Pulsar` original type;
- new `ActivePulsar` original state;
- commit/runtime representation paths;
- unresolved-set participation;
- reconstruction/terminal handling;
- switch/default behavior altered by the enum expansion.

Do not make the active production correction yet.

Build the temporary variant with an explicit timeout.

Run both regression oracles.

Record:

- campaign result;
- Level 15 result;
- whether both, one, or neither recover.

If removing the generic ledger change restores both, stop hunk search and identify the exact ledger mechanism.

If not, restore the temporary variant and continue.

---

# Phase 4: Isolate `TempestGame` admission/hatch/Pulsar changes

Use a second bounded temporary variant if necessary.

Test only relevant shared changes such as:

- Pulsar hatching;
- species selection;
- runtime representation;
- generic actor enumeration;
- admission capacity;
- random species selection;
- destruction/rim-clear routing;
- all-rim handling.

Preserve intended Level 17 configuration and unrelated presentation code where possible.

Run both oracles.

Determine whether this surface restores:

- campaign only;
- Level 15 only;
- both;
- neither.

Do not keep piling exclusions into one opaque variant.

---

# Phase 5: Isolate Flipper/Tanker cargo-route changes only if required

Only if ledger and `TempestGame` isolation do not prove the root, isolate:

- `Straight07` additions;
- cargo-route selector changes;
- ordinary Flipper program selection;
- route default/fallback behavior;
- Tanker cargo construction/release paths;
- direction/program enum ordering.

Look specifically for:

- changed enum numeric/default value;
- changed switch fall-through;
- default enum zero semantics;
- accidental use of Straight07 outside Level 17;
- changed Tanker-cargo route for Level 11/15;
- changed rim pursuit behavior.

Run both oracles after each bounded variant.

Stop once a causal hunk is proven.

---

# Phase 6: Narrow first divergent state when needed

If a surface restores an oracle but still contains several possible hunks, use boundary `19792/19793` to identify the exact initiating transition.

Capture only what is needed around that interval:

- active enemy identity/type;
- Tanker cargo ownership;
- source/destination lane;
- Flipper route/program;
- ledger original/cargo state;
- runtime representation;
- random-call index/state;
- scoring event;
- destruction event;
- cargo release;
- rim-state transition.

Temporary instrumentation is allowed in the temporary A/B environment.

Do not leave debugging instrumentation in production.

Do not use screenshots/image analysis.

---

# Phase 7: Determine exact root cause

Before editing active production code, write a concise causal statement of the form:

> Candidate hunk X changed shared behavior Y because condition/state Z now does …, affecting Levels 11 and/or 15 even though Level 17 was the intended consumer.

Or, if two independent roots exist:

> Campaign root X and Level 15 soak root Y are separate.

Include direct A/B evidence.

Do not make a production change without having this statement supported.

---

# Phase 8: Apply the smallest active-worktree correction

Once causality is proven, correct only the responsible shared behavior.

Requirements:

- restore baseline Levels 1–16 behavior;
- preserve intended Level 17 functionality;
- preserve Pulsar production integration where possible;
- preserve shape/palette/configuration work;
- preserve Level 17 `$07` behavior;
- avoid special-casing Level 11 or Level 15 merely to satisfy tests unless evidence proves a level-specific correction is correct;
- prefer correcting a generic regression at its source.

Do not fix Level 17 presentation/color.

Do not add Level 18.

---

# Phase 9: Add permanent regression tests

Add the smallest permanent tests necessary to prevent recurrence.

At minimum:

## Shared regression test

Capture the exact generic condition that caused the Level 11/15 drift.

Prefer a focused deterministic test over reproducing the whole campaign inside every test run.

## Level 11 regression

Protect the specific shared state/lifecycle behavior that first diverged.

## Level 15 regression

Keep the existing seed-1982 soak invariant unchanged unless the proved root shows separate test correction is objectively necessary.

Do not weaken it merely to pass.

## Level 17 preservation

Add a focused test proving the correction does not disable the intended Level 17 Pulsar/ledger/route functionality involved.

---

# Phase 10: Fast foundation gate

Before the full suite, run:

## Strict campaign oracle

Must PASS all required fields.

Prefer exact accepted baseline final telemetry if deterministic behavior is expected unchanged:

- Level 16;
- Complete;
- 9 lives;
- score 134039;
- remaining frames 0;
- RecordingCompleted;
- seed 1990198549.

If final score/lives differ despite all state mechanics appearing correct, do not immediately relax the oracle. Investigate why deterministic state differs.

## Level 15 seed-1982

Must PASS unchanged.

If either fails, STOP and continue root isolation rather than running the full suite.

---

# Phase 11: Focused regression validation

Once both primary oracles pass, run bounded focused tests for:

- Level 11 production;
- Level 11 Flipper/Tanker lifecycle;
- Level 15 production;
- Level 15 soak;
- ledger;
- Tanker cargo;
- Flipper movement/rim behavior;
- death/reconstruction;
- completion;
- Level 17 Pulsar admission/ledger;
- Level 17 `$07`;
- Level 17 Tanker cargo.

Use explicit VSTest hang timeouts.

All must pass before full suite.

---

# Phase 12: Full validation

Only after the two primary foundation oracles and focused tests pass:

1. full automated solution suite;
2. Release build;
3. `git diff --check`.

Use explicit bounds.

If another accepted-level regression appears, STOP.

Do not weaken accepted tests.

---

# Manual-validation boundary

Do not launch the game.

If automated correction succeeds, STOP for Neal.

Provide:

## Campaign replay command

The normal visible playback command for:

`TEMP/GameplayRecordings/lvl1-16.json`

Neal should confirm that playback once again visibly stays synchronized and completes all 16 levels.

## Level 17 command

Provide direct Level 17 startup only for checking that the correction did not destroy Level 17 runtime functionality.

Do **not** ask for Level 17 presentation acceptance yet because the known legacy-looking presentation and missing color scheme remain unresolved.

---

# Documentation boundary

Do not close Level 17.

Do not mark Level 17 accepted.

Update durable behavior/development documentation only if the correction establishes an important generic invariant that current durable documentation would otherwise misrepresent.

Do not rewrite historical reports.

---

# Required AI artifacts

Save this prompt as:

`AI-Logs/Prompts/2026-11-25-level17-shared-regression-bounded-correction-prompt.md`

Create:

`AI-Logs/Reports/2026-11-25-level17-shared-regression-bounded-correction-report.md`

These are GitHub continuity records, but this task does not authorize staging, commit or push.

---

# Required report contents

The report must include:

1. Executive summary
2. Starting Git/worktree state
3. Current oracle confirmation
4. Candidate shared-diff inventory
5. Ledger temporary A/B result
6. TempestGame temporary A/B result
7. Flipper/Tanker temporary A/B result if reached
8. First-divergence diagnostic if used
9. Exact root cause
10. Whether campaign and soak share one root
11. Production correction
12. Why correction preserves Level 17
13. Files changed
14. Tests added/changed
15. Strict campaign final result
16. Campaign final telemetry
17. Level 15 seed-1982 final result
18. Level 11 focused regression
19. Level 15 focused regression
20. Ledger/cargo/Flipper regressions
21. Level 17 preservation tests
22. Full-suite result
23. Release-build result
24. `git diff --check`
25. Remaining Level 17 presentation/color blockers
26. Manual-validation commands
27. Temporary environment cleanup
28. Final Git/worktree state
29. Stage/commit/push: none
30. Level 18+ untouched
31. Recommended next step

If a stop condition occurs, still document every completed isolation step in detail.

---

# Stop conditions

STOP rather than broadening if:

- both foundation oracles cannot be reproduced from current candidate;
- temporary A/B variants cannot build safely;
- more than a small number of shared surfaces must be excluded simultaneously;
- causal root remains ambiguous after bounded ledger/TempestGame/Flipper-Tanker isolation;
- correction requires redesigning the ledger;
- correction requires removing Level 17 Pulsar support wholesale;
- accepted prior-level behavior conflicts materially;
- security blocks required bounded tests;
- commands stall.

Report PARTIAL rather than guessing.

---

# Completion criteria

This milestone is complete only when:

- exact causal regression is established;
- smallest correction is implemented;
- strict Levels 1–16 recording oracle passes;
- deterministic final campaign state is restored or any difference is explicitly resolved;
- Level 15 seed-1982 passes unchanged;
- focused Level 11/15 shared regressions pass;
- intended Level 17 functionality remains;
- full suite passes;
- Release build succeeds with zero errors;
- `git diff --check` passes;
- nothing is staged, committed or pushed;
- Level 17 presentation/color remains explicitly pending;
- Level 18+ remains untouched.

Then STOP for Neal's manual regression confirmation.

TEMPEST-END-LEVEL17-SHARED-REGRESSION-BOUNDED-CORRECTION-COMPLETE