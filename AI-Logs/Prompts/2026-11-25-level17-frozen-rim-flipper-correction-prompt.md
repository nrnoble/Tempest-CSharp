# Tempest Level 17 Frozen Rim-Flipper Investigation and Bounded Correction

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

Investigate and correct the **Level 17 frozen rim-Flipper defect** using Neal's new deterministic Level 17 recording.

This is a bounded Level 17 gameplay investigation and correction.

The accepted Levels 1–16 foundation has just been restored and manually validated. Preserve it exactly.

Do not revisit the already-corrected Levels 11/15 shared cargo-route regression unless direct evidence shows the Level 17 rim defect still depends on that correction.

Do not work on Level 17 palette/color or tube-presentation defects in this task.

Do not implement Level 18+.

Do not stage, commit, or push.

Do not launch the interactive game.

Stop after evidence-backed correction, automated validation, and a manual-validation handoff.

---

# Required prior reports

Read completely:

`AI-Logs/Reports/2026-11-25-level17-bounded-production-implementation-report.md`

`AI-Logs/Reports/2026-11-25-level17-levels1-16-shared-regression-investigation-report.md`

`AI-Logs/Reports/2026-11-25-level17-shared-regression-bounded-correction-report.md`

Also consult the final Level 17 ROM/C# readiness reports only as needed to distinguish:

- ROM-backed `$07` below-rim behavior;
- project-translated rim behavior;
- ordinary versus Tanker-cargo Flippers.

The shared-regression correction report is the authority for the restored prior-level route distinction.

---

# Starting accepted state

The preceding bounded correction established:

- the Level 17 candidate had repurposed the existing generic Tanker-cargo `Straight07` route;
- Levels 11 and 15 already depended on that generic route;
- a distinct `TankerCargoMovementProgram.Level17Straight07` restored the accepted prior-level behavior;
- Levels 1–16 campaign playback was restored;
- Level 15 seed-1982 was restored;
- focused regression passed;
- full suite passed 2,058/2,058;
- Release build passed with zero warnings and zero errors;
- `git diff --check` passed.

Neal has now manually confirmed:

- `TEMP/GameplayRecordings/lvl1-16.json` plays correctly again through Levels 1–16.

Treat the Levels 1–16 correction as manually accepted.

Do not modify the restored generic `Straight07` semantics unless direct evidence demonstrates the correction itself is incomplete.

---

# New Level 17 manual evidence

Record this as **MANUAL LEVEL 17 DEFECT EVIDENCE**.

Neal reports:

- Level 17 launches and is playable.
- Flippers climb to the rim.
- Once Level 17 Flippers reach the rim, some or all remain frozen on the rim.
- Frozen rim Flippers do not flip/move around the rim as expected.
- The Blaster can move underneath the frozen rim Flippers.
- The Blaster can shoot and destroy them.
- Pulsars reach the rim and continue flipping/moving as expected.
- Tankers behave as expected.
- The defect therefore appears specific to Flippers at or immediately after rim arrival.
- The earlier Levels 1–16 playback regression is fixed.

Do not reinterpret this as a below-rim `$07` failure. The Flippers successfully reach the rim.

Do not assume the problem is rendering-only. Prove whether simulation state advances.

---

# Level 17 diagnostic recording

Use this user-created recording as the primary reproduction:

`TEMP/GameplayRecordings/lvl-17-test.json`

User-supplied path spelling:

`.\TEMP\GameplayRecordings\lvl-17-test.json`

This recording is local diagnostic material.

Requirements:

- do not modify it;
- do not rewrite its input;
- do not change metadata;
- do not commit it;
- do not move it;
- do not inspect unrelated recordings;
- do not perform screenshot or frame analysis.

If the file is absent, malformed, or incompatible with the current candidate, report that precisely and stop rather than substituting another recording.

---

# Starting Git/worktree protection

Before changing anything, record:

- branch;
- HEAD;
- upstream;
- ahead/behind;
- staged state;
- tracked modifications;
- untracked files relevant to the task;
- current Level 17 candidate diff;
- preceding shared-regression correction diff.

Preserve:

- the complete current Level 17 candidate;
- the accepted shared-regression correction;
- pre-existing `TEMPEST-BEHAVIOR-SPEC.md` modification;
- AI-Logs;
- ROM/reference files;
- both local recordings;
- unrelated assets.

Do not clean, reset, restore, stash, or overwrite the active worktree.

Temporary diagnostics may use ignored `TEMP/` output or an external temporary location.

---

# Primary reproduction objective

Use headless playback and exact recording boundaries to locate one or more frozen Level 17 rim Flippers.

Do not treat:

- parsing success;
- process exit 0;
- `RecordingCompleted`;
- zero remaining frames;

as proof that rim behavior is correct.

The primary oracle is **state-based Flipper rim movement**.

The investigation must determine whether a Flipper that reaches the rim subsequently:

- changes rim lane;
- enters a Flipping/transition state;
- establishes a rim-pursuit direction/latch;
- updates its lane or transition progress;
- remains represented in simulation;
- remains represented in rendering submission where inspectable.

A Flipper that remains indefinitely at one rim lane without a valid transition is a FAIL even if the player later destroys it and the recording continues.

---

# Phase 1: Run and inventory the recording

Run:

`TEMP/GameplayRecordings/lvl-17-test.json`

through the existing Release headless playback CLI with an explicit process timeout.

Record:

- recording format/version;
- random seed;
- starting displayed level;
- recording duration;
- final completion reason;
- remaining frames;
- final level/state;
- lives;
- score.

Determine whether the recording begins directly at Level 17.

Do not infer success from the final state alone.

---

# Phase 2: Locate the first Level 17 Flipper rim arrival

Use the existing `--boundary` capability and deterministic reconstruction to locate:

1. the earliest Flipper that approaches the rim;
2. the exact boundary before rim arrival;
3. the rim-arrival boundary;
4. subsequent boundaries where movement should occur;
5. the interval over which the Flipper remains frozen.

Use coarse-to-fine bounded search.

Do not advance one tick at a time from recording start if boundary search can narrow it efficiently.

For each relevant Flipper capture:

- stable/runtime identity if exposed;
- ordinary original versus Tanker cargo;
- cargo owner/ordinal if applicable;
- lane;
- source lane;
- destination lane;
- height/depth;
- movement state;
- direction;
- `RimPursuitDirection`;
- active transition;
- transition progress;
- movement-program identity;
- destruction/terminal state;
- runtime representation.

If current JSON omits a required field, use the smallest temporary diagnostic harness or ignored instrumentation necessary to expose it.

Do not make permanent production changes merely to observe state.

---

# Phase 3: Determine affected Flipper categories

Establish whether the defect affects:

- ordinary original Level 17 Flippers;
- Level 17 Tanker-cargo Flippers;
- both;
- only Flippers following `BelowRimFlipperProgram.Straight07`;
- only Flippers following `TankerCargoMovementProgram.Level17Straight07`;
- all Level 17 Flippers regardless of origin.

Do not assume ordinary and cargo Flippers share the same initialization/handoff path.

Record separate evidence for each category visible in the recording.

If the recording does not exercise both categories, say so and supplement only with a bounded automated fixture using existing production factories.

Do not search for additional recordings.

---

# Phase 4: Trace the below-rim-to-rim handoff

For the first affected Flipper, trace the state transition from climbing to rim behavior.

Inspect:

- height/depth clamp;
- `AtRim` or equivalent condition;
- below-rim movement-program termination;
- lane-transition cancellation/completion;
- direction;
- handedness;
- rim-pursuit direction/latch;
- player-relative pursuit direction;
- timer/cooldown;
- factory flags;
- source species/identity;
- cargo route;
- current simulation state;
- rim update eligibility.

Determine whether Level 17 reaches the shared rim-pursuit update.

If not, identify the exact gate that excludes it.

If it reaches the update but does not move, identify the exact field or condition preventing lane transition.

---

# Phase 5: Compare with a working prior-level Flipper

Use an accepted working prior-level Flipper as a control.

Prefer a level using the same intended shared rim-pursuit subsystem without reopening unrelated configuration.

Compare only the relevant rim handoff/state fields:

- pre-rim state;
- rim-arrival state;
- `Direction`;
- `RimPursuitDirection`;
- state enum;
- cooldown/timer;
- movement-program cleanup;
- route identity;
- lane-change eligibility;
- player-relative direction calculation.

Do not copy an entire prior-level implementation into Level 17.

The goal is to identify the missing shared rim entry invariant.

---

# Phase 6: Compare with a working Level 17 Pulsar

Neal reports Level 17 Pulsars continue moving on the rim.

Use a working Level 17 Pulsar only as a **control for shared tube/rim geometry and general runtime progression**.

Compare:

- whether the game continues updating rim actors;
- whether Level 17 geometry accepts rim lane transitions;
- whether shared rim projection remains finite;
- whether the issue is Flipper-specific.

Do not copy Pulsar rim behavior into Flippers.

Pulsar success can disprove a whole-level frozen-rim or tube-connectivity problem, but it does not define Flipper pursuit behavior.

---

# Phase 7: Simulation versus rendering

Classify the frozen Flipper as:

## Simulation frozen

Lane/state/transition remains unchanged over expected updates.

## Rendering-only frozen

Simulation lane/state advances but visible/render submission does not.

## Both

Simulation and presentation each have a defect.

Use headless state as the primary evidence.

If simulation changes lanes normally, inspect:

- renderer inclusion;
- submitted lane geometry;
- transition geometry;
- projection finiteness;
- culling;
- state-to-glyph selection.

If simulation state is frozen, do not broaden into rendering work.

---

# Phase 8: Inspect the Level 17 route distinction

The prior correction added:

`TankerCargoMovementProgram.Level17Straight07`

while restoring the existing generic:

`TankerCargoMovementProgram.Straight07`

Inspect all uses of:

- `BelowRimFlipperProgram.Straight07`;
- `TankerCargoMovementProgram.Level17Straight07`;
- existing generic `Straight07`;
- Level 17 ordinary Flipper factory;
- Level 17 Tanker-cargo factory;
- Egg/reconstruction factory;
- rim entry/handoff.

Determine whether the new route:

- correctly controls only below-rim movement;
- accidentally remains active after rim arrival;
- skips normal initialization;
- leaves a direction/latch at zero or invalid;
- selects a movement state not eligible for rim updates;
- behaves differently after reconstruction;
- works for one Flipper category but not the other.

Do not revert to repurposing the generic prior-level route.

---

# Phase 9: Review the ROM/project behavior boundary

The verified ROM evidence establishes `$07` below-rim straight climbing.

Determine what the ROM/project evidence establishes about Flipper behavior after rim arrival.

Separate:

- below-rim `$07` movement;
- rim pursuit/flip behavior;
- project translation;
- existing accepted shared behavior.

Do not conclude that `$07` means "remain stationary at the rim."

Unless authoritative evidence says otherwise, the new route should terminate at rim entry and hand off to normal Flipper rim behavior.

Record this classification explicitly.

---

# Phase 10: Establish exact root cause

Before modifying active production code, provide a causal statement:

> Level 17 Flippers freeze at the rim because route/state X leaves field/state Y in condition Z, preventing shared rim transition gate Q.

If more than one category has separate causes, document both.

Support the statement with recording-boundary evidence and source trace.

Do not make a speculative correction first and reason backward from a passing run.

---

# Phase 11: Apply the smallest bounded correction

After proving causality, correct only the Level 17 rim-handoff defect.

Requirements:

- preserve Level 17 `$07` straight below-rim behavior;
- preserve distinct `Level17Straight07` cargo routing;
- preserve restored Levels 1–16 behavior;
- preserve ordinary/cargo identity;
- preserve Pulsars;
- preserve Tankers;
- preserve existing rim-pursuit speed and direction semantics;
- avoid Level 11/15 special cases;
- avoid global route changes unless evidence proves a generic invariant is missing.

Prefer restoring a shared rim-entry invariant over adding a recurring per-level workaround.

Do not change Level 17 palette or presentation calibration.

---

# Phase 12: Permanent focused regression test

Add a deterministic test that fails on the current defect and passes after correction.

The test must establish:

1. A Level 17 Flipper reaches the rim.
2. The Flipper remains active and represented.
3. Within a bounded expected interval, the Flipper:
- begins a valid rim transition, or
- changes rim lane according to shared pursuit behavior.
4. The Flipper does not remain indefinitely frozen at one rim lane.
5. Ordinary and Tanker-cargo Level 17 Flippers are both covered where applicable.

Do not write a weak test that merely checks `AtRim == true`.

Do not make the test depend only on eventual destruction by the player's shot.

Also preserve tests for:

- straight below-rim route;
- no below-rim lane changes;
- correct cargo route distinction.

---

# Phase 13: Level 17 recording oracle

Use:

`TEMP/GameplayRecordings/lvl-17-test.json`

after correction.

Define a bounded state oracle from the affected Flipper interval.

At minimum prove:

- the identified affected Flipper reaches the rim;
- rim state no longer remains frozen;
- lane/transition state advances;
- playback does not crash;
- runtime representation remains valid.

If the recording later destroys the Flipper, assert successful movement before destruction.

Do not require the recording to end at a particular score or life count unless the file's purpose and current telemetry establish that as stable.

Do not commit the recording.

---

# Phase 14: Foundation regression gates

After the focused Level 17 defect test and recording oracle pass, run:

## Levels 1–16 recording

`TEMP/GameplayRecordings/lvl1-16.json`

The active candidate now intentionally allows Level 17 progression, so confirm accepted behavior through the Level 16 completion boundary using the established bounded/baseline-terminal oracle.

Required accepted boundary:

- Level 16;
- state `Complete`;
- lives `9`;
- score `134039`;
- seed `1990198549`;
- recording ticks `35350`;
- no earlier GameOver/divergence.

If active playback continues into Level 17 at the final input boundary, use the already established bounded terminal comparison method rather than changing the recording.

## Level 15 seed-1982

Run unchanged:

`Level15ProductionSoakTests.SeededProductionSoakConservesIdentitiesAcross1200Ticks(seed: 1982)`

Both must pass.

---

# Phase 15: Focused validation

Run bounded focused tests covering:

- Level 17 ordinary Flipper below-rim and rim handoff;
- Level 17 cargo Flipper below-rim and rim handoff;
- Level 17 Tanker cargo;
- Level 17 Pulsar rim behavior;
- Level 17 configuration/admission;
- shared Flipper rim pursuit;
- Levels 11 and 15 cargo-route regression;
- ledger/runtime representation;
- reconstruction;
- completion.

Use explicit VSTest hang timeout.

---

# Phase 16: Full validation

Only after all fast oracles pass:

1. full solution test suite;
2. Release build;
3. `git diff --check`.

Require:

- zero failed tests;
- zero build errors;
- report warnings exactly;
- no whitespace errors.

If an accepted prior-level regression appears, stop.

---

# Manual-validation boundary

Do not launch the game.

After automated validation, stop for Neal.

Provide exact commands for:

## Level 17 recording

```
dotnet run --project src\Tempest.Game -- --playback-file TEMP\GameplayRecordings\lvl-17-test.json
```

Neal should verify:

- Flippers climb straight below the rim;
- after rim arrival, Flippers resume normal rim flipping/movement;
- ordinary and cargo Flippers work where observed;
- Pulsars remain correct;
- Tankers remain correct;
- Blaster collision/shooting remains correct.

## Levels 1–16 recording

```
dotnet run --project src\Tempest.Game -- --playback-file TEMP\GameplayRecordings\lvl1-16.json
```

Neal should confirm the accepted campaign remains synchronized.

Do not ask for Level 17 presentation/color acceptance in this task.

---

# Known out-of-scope Level 17 blockers

Keep these explicitly pending:

- Level 17 visually resembles the old/legacy-looking tube presentation.
- The expected Levels 17–32 visible color scheme is absent.
- Exact host palette role mapping remains unresolved.
- Level 17 visual calibration/placement remains unaccepted.

Do not change those surfaces.

---

# Required AI artifacts

Save this prompt as:

`AI-Logs/Prompts/2026-11-25-level17-frozen-rim-flipper-correction-prompt.md`

Create:

`AI-Logs/Reports/2026-11-25-level17-frozen-rim-flipper-correction-report.md`

These are GitHub continuity records, but this task does not authorize staging, commit, or push.

---

# Required report contents

The report must include:

1. Executive summary
2. Starting Git/worktree state
3. Manual defect evidence
4. Recording metadata/final telemetry
5. First Flipper rim-arrival boundary
6. Frozen interval
7. Ordinary-versus-cargo classification
8. Flipper state before rim arrival
9. Flipper state at rim arrival
10. Frozen Flipper state after arrival
11. Working prior-level comparison
12. Working Pulsar comparison
13. Simulation-versus-rendering classification
14. Route/factory trace
15. Rim-handoff trace
16. Exact root cause
17. Production correction
18. Why correction preserves `$07` below-rim behavior
19. Why correction preserves Levels 1–16
20. Files changed
21. Permanent focused regression test
22. Level 17 recording final result
23. Levels 1–16 recording result
24. Level 15 seed-1982 result
25. Focused validation
26. Full-suite result
27. Release-build result
28. `git diff --check`
29. Remaining presentation/color blockers
30. Manual-validation commands
31. Temporary diagnostic cleanup
32. Final Git/worktree state
33. Stage/commit/push: none
34. Level 18+ untouched
35. Recommended next step

If a stop condition occurs, document everything completed rather than producing a short blocker summary.

---

# Stop conditions

Stop rather than guessing if:

- the Level 17 recording does not reproduce the defect;
- affected Flipper identity/category cannot be established;
- required state cannot be observed without broad production instrumentation;
- the defect requires redesigning generic rim pursuit;
- correction threatens the restored Levels 1–16 behavior;
- ordinary and cargo Flippers show unrelated defects requiring separate milestones;
- security blocks required bounded diagnostics;
- commands stall.

Preserve the worktree and report PARTIAL.

---

# Completion criteria

Complete only when:

- the recording reproduces the frozen rim-Flipper defect;
- simulation-versus-rendering is determined;
- exact rim-handoff root cause is established;
- smallest correction is implemented;
- Level 17 Flippers move/flip after reaching the rim;
- `$07` remains straight below the rim;
- ordinary and cargo paths are correct where applicable;
- Pulsars remain correct;
- Tankers remain correct;
- Level 17 recording oracle passes;
- Levels 1–16 recording oracle passes;
- Level 15 seed-1982 passes;
- focused tests pass;
- full suite passes;
- Release build succeeds with zero errors;
- `git diff --check` passes;
- presentation/color remains unchanged and pending;
- nothing is staged, committed, or pushed;
- Level 18+ remains untouched.

Then STOP for Neal's manual acceptance.

TEMPEST-END-LEVEL17-FROZEN-RIM-FLIPPER-CORRECTION-COMPLETE