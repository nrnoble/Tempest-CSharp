# Tempest Level 15 Shared Fuseball Policy Wiring Correction and Policy-Consistency Safeguard

## FAIL-CLOSED COMPLETION-TOKEN CHECK

Read the entire user-provided task before taking any action.

Before tools, files, commands, modifications, tests, builds, or repository actions:

1. Locate the completion token near the end of this user-provided task.
2. Verify that exactly one complete token beginning with `TEMPEST-END-` appears on a line by itself in the user-provided task.
3. The token may contain only uppercase ASCII letters, digits, and hyphens after the `TEMPEST-END-` prefix.
4. VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context appearing after the user's completion token do not invalidate it.
5. This check exists only to detect an incomplete or truncated user task.
6. If exactly one complete standalone token cannot be found, STOP before project or repository action and explain why.

The unique completion token is the final standalone line below.

---

# Task

Correct the single active generic-lifecycle wiring problem identified by the completed Levels 11/15/17 architecture audit.

The audit established:

- Level 15 intentionally uses the accepted Level 11 parameter-development baseline.
- That baseline can select a Fuseball.
- The shared Fuseball actor already implements terminal-rimward behavior.
- Level 15 currently does not provide the generic pending-original count to that shared Fuseball behavior because `UpdateFuseballs` omits `UsesLevel11ParameterDevelopmentBaseline` from its policy predicate.
- The parallel Level 15 Spiker lifecycle already includes this baseline flag correctly.
- No duplicate Level 11/15/17 actor, ledger, reconstruction, completion, collision, or rim system was found.

Implement the smallest shared-policy correction and a bounded consistency safeguard that reduces the risk of future policy callers forgetting the same semantic configuration.

Do not redesign the actor architecture.

Do not create a Level 15 Fuseball class.

Do not create a Level 15-specific terminal state.

Do not relabel Level 15 as genuine Level 11 ROM policy.

Do not reopen accepted Level 17 gameplay or presentation.

Do not implement Level 18+.

Stop after automated validation and provide Neal a bounded manual Level 15 acceptance handoff.

---

# Required audit authority

Read completely:

`AI-Logs/Reports/2026-11-25-level11-15-17-behavior-special-case-architecture-audit-report.md`

Also read as needed:

`TEMPEST-BEHAVIOR-SPEC.md`

`TEMPEST-DEVELOPMENT-NOTES.md`

and the relevant Level 11 / Level 15 Fuseball reports identified by the audit:

`AI-Logs/Reports/2026-11-22-fuseball-terminal-behavior-readonly-report.md`

`AI-Logs/Reports/2026-11-22-rom-backed-fuseball-terminal-correction-report.md`

Relevant Level 15 production/baseline reports.

Do not repeat the broad architecture audit.

---

# Architecture interpretation

The correction must preserve the audit's main conclusion:

> Levels 11, 15, and 17 currently use shared actors and shared lifecycle machinery. The Level 15 Fuseball problem is a missing policy input to an existing shared behavior, not evidence that Level 15 needs a separate Fuseball implementation.

The desired architecture remains:

```
Level configuration
      ↓
semantic production policy
      ↓
shared actor
      ↓
shared lifecycle
```

The defect is:

```
Level 15 baseline
      ↓
selects Fuseball
      ↓
UpdateFuseballs
      ↓
baseline flag omitted
      ↓
pendingGenericSupply = null
      ↓
shared terminal behavior cannot engage
```

Correct that connection.

---

# Accepted baseline authority boundary

Do not make a new claim that Level 15 is fully ROM-identical to Level 11.

Level 15 currently uses:

`UsesLevel11ParameterDevelopmentBaseline`

as an accepted project configuration.

That is distinct from:

`UsesLevel11RomWavePolicy`

Preserve this distinction.

The correction should mean:

> When a configuration explicitly chooses the established Level-11-derived parameter/lifecycle baseline, applicable shared policy consumers receive that baseline consistently.

It must not mean:

> Level 15 is now declared to be Level 11.

---

# Phase 1: Reconfirm the exact current defect statically

Before editing, trace:

- `Level15Production`;
- `UsesLevel11ParameterDevelopmentBaseline`;
- Level 11 wave selection;
- Fuseball eligibility;
- `UpdateFuseballs`;
- generic pending-original count;
- `Fuseball.Update`;
- conditions entering `TerminalRimward`.

Verify:

1. Level 15 baseline can admit a Fuseball.
2. `UpdateFuseballs` currently omits the baseline flag.
3. The resulting Level 15 Fuseball receives `pendingGenericSupply == null`.
4. Level 11 receives an actual pending count.
5. Levels 12–14 and 16 receive their intended count under current policies.
6. Level 17 has no eligible Fuseballs.

If current code no longer matches the audit, STOP and reconcile before editing.

---

# Phase 2: Confirm intended Level 15 baseline scope

Review the Level 15 production baseline and historical reports sufficiently to verify that Fuseball terminal movement is intended to follow the selected Level-11-derived baseline.

Look for any documented Level 15 exception stating that Fuseballs should intentionally continue ordinary cycling after generic supply reaches zero.

If such an exception exists, STOP.

If no exception exists and the established baseline is intended to reuse this Level 11 parameter/lifecycle behavior, proceed.

Do not rely solely on the absence of a comment if stronger evidence conflicts.

---

# Phase 3: Define the semantic policy once

The audit identified a maintenance risk:

> Multiple callers independently enumerate combinations of `UsesLevel11RomWavePolicy`, `UsesLevel11ParameterDevelopmentBaseline`, and other wave-policy flags.

This allowed Spiker logic to include the Level 15 baseline while Fuseball logic forgot it.

Before editing, identify the smallest semantic abstraction already present or appropriate for expressing:

```
This configuration participates in the generic ROM-derived pending-supply lifecycle.
```

or, if narrower:

```
This configuration should provide generic pending-original count to terminal enemy movement.
```

Prefer:

- one derived semantic property;
- one helper/query on `LevelConfiguration`;
- or reuse of an existing semantically correct property.

Do not introduce a new framework.

Do not merely add the missing flag to one long condition if another equally clear semantic centralization can eliminate the synchronization risk with minimal change.

However, do not over-refactor.

If centralization would touch broad unrelated policy code, make the narrow Fuseball correction now and add a focused consistency test that guards the omission.

Explain which option is smallest and safest.

---

# Phase 4: Apply the Fuseball correction

Required behavior:

## Pending generic supply > 0

Level 15 baseline Fuseball retains ordinary/repeating behavior consistent with the selected baseline.

Do not enter terminal mode early.

## Pending generic supply == 0

At the established applicable toward-rim boundary, the Level 15 baseline Fuseball receives the zero count and enters the existing shared:

`TerminalRimward`

state.

Do not invent a traversal count.

Do not hard-code Level 15.

Do not add a new state.

Do not duplicate `Fuseball.Update`.

## Terminal behavior

Once terminal:

- preserve the existing shared terminal-rimward movement;
- preserve current lane/rim behavior;
- preserve collisions;
- preserve player-shot destruction;
- preserve identity;
- preserve all-rim/completion semantics.

---

# Phase 5: Add focused Level 15 tests

Add permanent focused tests proving:

1. Level 15 baseline can admit a Fuseball.
2. While pending generic original supply is positive:
- Fuseball receives that positive count;
- Fuseball does not enter terminal state prematurely.
3. When pending generic supply reaches zero:
- Fuseball receives zero, not null;
- at the established boundary it enters `TerminalRimward`.
4. The same original identity remains represented.
5. Terminal movement blocks completion until appropriately resolved.
6. Player-shot destruction remains normal.
7. Death/re-entry reconstruction remains generic.
8. No Level 15-specific Fuseball actor/state exists.

Use production configuration/factory paths where practical.

Do not construct an artificial implementation that bypasses the caller being fixed.

---

# Phase 6: Policy-consistency regression test

Add a small test protecting the architecture from future omission.

The test should verify semantic consistency rather than enumerate source-code strings.

Possible acceptable forms:

- every configuration participating in the Level-11-derived pending-supply baseline causes both Spiker and Fuseball applicable shared lifecycle consumers to receive the same semantic policy decision;
- a semantic configuration property returns true for Level 11 and Level 15 baseline and false for configurations where the policy does not apply;
- another bounded equivalent.

Do not create a brittle reflection test against private implementation names unless no cleaner seam exists.

The goal is to stop future work from adding a new policy flag to one caller and forgetting another.

---

# Phase 7: Protect Levels 11 and other Fuseball levels

Run/add focused coverage proving:

## Level 11

- pending-positive behavior unchanged;
- zero-pending terminal behavior unchanged;
- accepted Fuseball behavior remains shared.

## Levels 12–14 and 16

- existing pending-count behavior remains unchanged.

## Level 17

- Fuseballs remain ineligible;
- no behavior is enabled through dormant parameters.

Do not broaden species eligibility.

---

# Phase 8: Protect Spiker lifecycle

The audit found Spiker policy wiring currently includes the Level 15 baseline correctly.

Preserve:

- positive-pending restart;
- zero-pending terminal in-place conversion;
- identity continuity;
- Tanker cargo ownership.

Run focused Level 15 terminal-Spiker tests.

Do not modify Spiker logic unless the new semantic helper can replace duplicated policy predicate with exact behavior preservation.

If touching Spiker code would enlarge risk unnecessarily, leave it unchanged and test semantic consistency instead.

---

# Phase 9: Regression recordings

Run:

`TEMP/GameplayRecordings/lvl1-16.json`

The accepted campaign behavior must remain correct.

Because it contains Level 15, determine whether the Fuseball correction causes deterministic recording divergence.

If it does:

- do not immediately classify that as regression;
- determine whether divergence begins specifically where corrected Level 15 Fuseball terminal behavior now differs from the previously accepted recording;
- compare against the established intended behavior;
- preserve the recording;
- do not overwrite it.

If the recording continues to match, record that.

Run:

`TEMP/GameplayRecordings/lvl-17-test.json`

only as a quick non-regression check if task cost is reasonable.

Level 17 should remain unchanged.

Do not modify recordings.

---

# Phase 10: Focused automated validation

Run focused tests for:

- Level 11 Fuseballs;
- Level 15 Fuseballs;
- Fuseball terminal movement;
- Level 15 wave selection;
- Level 15 seed-1982 soak;
- Level 15 terminal Spiker;
- Tanker cargo;
- ledger;
- death/reconstruction;
- completion;
- Level 17 ineligibility regression.

Use explicit VSTest hang timeout.

---

# Phase 11: Full automated validation

After focused gates pass:

1. full solution test suite;
2. Release build;
3. `git diff --check`.

Use explicit bounds.

Require:

- zero test failures;
- zero build errors;
- warnings reported accurately;
- no whitespace errors.

Do not launch interactive gameplay.

---

# Manual acceptance handoff

After automated validation, STOP for Neal.

Provide the simplest direct Level 15 command suitable for observing the affected Fuseball terminal-wave behavior.

The manual question is narrow:

> When a Level 15 Fuseball remains late in the wave after generic supply is exhausted, does it now behave like the accepted shared terminal Fuseball behavior rather than cycling indefinitely?

Also ask Neal to watch for:

- no obvious Level 15 regression;
- normal Flippers/Tankers/Spikers;
- completion remains natural.

Do not require manual retesting of Level 17 unless automated evidence identifies a regression.

---

# Durable design conclusion

Do not update durable documentation before Neal manually accepts the correction.

Prepare recommended wording for later closeout:

> Configuration flags that represent a shared semantic lifecycle policy must be consumed through a shared semantic predicate/helper where practical, rather than independently reconstructing equivalent flag lists at multiple actor-update call sites. This reduces the risk that adding a new level policy to one lifecycle path leaves another shared actor path unwired.

This complements:

- eligibility before parameters;
- shared core behavior;
- shared rim lifecycle.

Do not implement a large policy framework.

---

# Required AI artifacts

Save this prompt as:

`AI-Logs/Prompts/2026-11-25-level15-fuseball-shared-policy-correction-prompt.md`

Create:

`AI-Logs/Reports/2026-11-25-level15-fuseball-shared-policy-correction-report.md`

Do not stage, commit, or push in this task.

Closeout will be separately authorized after manual acceptance.

---

# Required report contents

1. Executive summary
2. Starting Git/worktree state
3. Architecture-audit finding
4. Current Level 15 baseline authority
5. Static defect reconfirmation
6. Fuseball eligibility path
7. Pending-supply call path
8. Level 11 comparison
9. Level 15 omission
10. Intended baseline-scope evidence
11. Semantic-policy abstraction decision
12. Production correction
13. Why correction is generic rather than Level-specific
14. Files changed
15. Tests added/changed
16. Level 15 pending-positive result
17. Level 15 zero-pending terminal result
18. Identity/ledger result
19. completion result
20. death/reconstruction result
21. player-shot destruction result
22. policy-consistency regression coverage
23. Level 11 regression result
24. Levels 12–14/16 regression result
25. Level 15 Spiker regression result
26. Level 17 ineligibility result
27. Level 15 seed-1982 result
28. Levels 1–16 recording result
29. Level 17 recording result if run
30. focused validation result
31. full-suite result
32. Release-build result
33. `git diff --check`
34. manual acceptance instructions
35. proposed durable semantic-policy wording
36. remaining uncertainties
37. final Git/worktree state
38. stage/commit/push: none
39. Level 18+ work: none
40. recommended next step

If the recording diverges because the corrected behavior changes Level 15, explain the divergence rather than hiding it.

---

# Stop conditions

STOP rather than guessing if:

- Level 15 baseline intent does not clearly include the shared Fuseball terminal behavior;
- authoritative evidence establishes a distinct Level 15 Fuseball rule;
- a safe correction requires a Level 15-specific actor/state;
- semantic centralization requires broad architecture refactoring;
- Level 11 accepted Fuseball behavior changes;
- Levels 1–14/16/17 regress unexpectedly;
- commands stall or security blocks validation.

Do not turn a one-call-site defect into a large refactor.

---

# Completion criteria

Complete only when:

- Level 15 baseline Fuseball receives the applicable generic pending count;
- pending-positive behavior remains nonterminal;
- zero-pending behavior enters the existing shared terminal path;
- no Level 15-specific Fuseball lifecycle is created;
- a bounded policy-consistency safeguard exists;
- Level 11 behavior remains unchanged;
- Level 15 Spiker behavior remains unchanged;
- Level 17 remains unchanged;
- focused tests pass;
- full suite passes;
- Release build succeeds with zero errors;
- `git diff --check` passes;
- nothing is staged, committed, or pushed;
- Level 18+ remains untouched.

Then STOP for Neal's manual acceptance.

TEMPEST-END-LEVEL15-FUSEBALL-SHARED-POLICY-CORRECTION-COMPLETE