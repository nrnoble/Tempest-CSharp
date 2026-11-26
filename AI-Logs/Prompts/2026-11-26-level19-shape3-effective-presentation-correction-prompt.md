# Tempest Level 19 Shape 3 Effective-Presentation Investigation and Bounded Correction

## FAIL-CLOSED COMPLETION-TOKEN CHECK

Read the entire user-provided task before taking any action.

Before tools, files, commands, modifications, tests, builds, or repository actions:

1. Locate the completion token near the end of this user-provided task.
2. Verify exactly one complete token beginning with `TEMPEST-END-` appears on a line by itself.
3. The token may contain only uppercase ASCII letters, digits, and hyphens after `TEMPEST-END-`.
4. VS Code/agent-generated context after the token does not invalidate it.
5. This check exists only to detect an incomplete/truncated task.
6. If exactly one valid standalone token cannot be found, STOP before project/repository action.

The unique completion token is the final standalone line below.

---

# Task

Investigate and correct one narrow Level 19 presentation defect:

> Displayed Level 19 uses canonical Shape 3, but its effective vertical screen placement is visibly and substantially different from the already accepted Level 3 Shape 3 production placement.

Neal reports:

- Level 19 gameplay otherwise plays correctly.
- No gameplay regression is currently identified.
- Shape/form is otherwise recognizable as Shape 3.
- The Level 19 vertical placement is clearly wrong.
- Level 3's existing production vertical placement is correct and accepted.
- Level 19 appears to be using a legacy or otherwise incorrect vertical-placement path rather than the accepted Shape 3 placement.
- The discrepancy is large enough to be obvious without pixel-level comparison.

The current implementation report states that Level 19 reuses Level 3's model frame, projection, and calibration.

Therefore the immediate objective is:

> Determine why the effective rendered result differs despite the claimed reuse.

Do not manually recalibrate Level 19.

Do not create a Level 19-specific vertical offset.

Do not use screenshots to fit a value.

The correct solution should make repeated Shape 3 use the accepted Shape 3 production presentation automatically.

---

# Scope

This is a narrow presentation investigation and bounded correction.

Freeze Level 19 gameplay completely.

Do not modify:

- wave supply;
- enemy eligibility;
- Flipper movement;
- Tanker behavior/cargo;
- Pulsar behavior;
- firing;
- Spike/Spiker policy;
- speed/timing;
- identity;
- ledger;
- reconstruction;
- completion;
- palette;
- HUD geometry;
- Level 18→19 progression gameplay semantics;
- Level 20+.

Do not stage, commit, or push.

Do not launch the interactive game.

Stop after:

- root cause;
- smallest presentation correction;
- effective-output regression tests;
- automated validation;
- manual launch handoff.

---

# Required authorities

Read completely:

`AI-Logs/Reports/2026-11-26-level19-rom-configuration-readonly-investigation-report.md`

`AI-Logs/Reports/2026-11-26-level19-bounded-production-implementation-report.md`

Read as needed:

`AI-Logs/Reports/2026-11-26-level18-final-closeout-report.md`

`AI-Logs/Reports/2026-11-25-production-presentation-default-promotion-closeout-report.md`

Level 3 accepted presentation documentation/reports.

`TEMPEST-BEHAVIOR-SPEC.md`

`TEMPEST-DEVELOPMENT-NOTES.md`

Inspect current:

- `RomPresentationMetadata`;
- `AtariPresentationAdapter`;
- `AtariPresentationCalibration`;
- `VectorRenderer`;
- Level configuration/mapping;
- model-frame resolver;
- projection helpers;
- relevant presentation tests.

Do not reopen gameplay investigations.

---

# Critical prior claim to verify

The Level 19 implementation report states:

> Displayed Level 19 resolves ROM row 2 to canonical first-cycle Shape 3 and reuses the existing Level 3 model frame, projection, and calibration path.

Do not accept that statement merely because metadata lookup says so.

Prove or disprove it at the **effective output** level.

The test must distinguish:

```
resolved same metadata
```

from:

```
produced same screen-space presentation
```

---

# Accepted Shape 3 authority

Locate the manually accepted Level 3 Shape 3 production presentation.

Record:

- canonical shape/row ID;
- model-frame values;
- calibration;
- projection selection;
- vertical placement;
- effective screen center/rim coordinates;
- any shape-specific vertical accommodation;
- any adapter branch;
- any renderer translation.

Level 3 is the accepted reference owner of Shape 3.

Do not alter Level 3.

---

# Phase 1: trace Level 3 presentation end-to-end

Trace a representative Shape 3 tube point set through the entire production presentation pipeline.

Capture at each stage:

1. displayed level;
2. canonical shape/row identity;
3. model frame;
4. calibration object;
5. calibrated model coordinates;
6. projection-family selection;
7. projected x/y;
8. post-projection vertical transformation;
9. viewport/game-area translation;
10. renderer submission;
11. final screen-space coordinates.

Choose several representative deterministic points, including:

- near-rim top;
- near-rim bottom;
- far/deep vertex;
- shape center/reference point;
- one representative entity/lane point if useful.

Do not use a screenshot.

Use runtime-independent calculations or focused test instrumentation.

---

# Phase 2: trace Level 19 presentation identically

Run the exact same Shape 3 model points through displayed Level 19's production pipeline.

Capture the same values in the same order.

Do not compare only:

- calibration object equality;
- model-frame identity;
- ROM row;
- metadata index.

Compare actual numeric values all the way to final screen-space output.

---

# Phase 3: find the first numeric divergence

Produce a comparison matrix:

Stage | Level 3 | Level 19 | Equal? | Expected?

Identify the **first stage at which an effective numeric presentation value diverges**.

Do not jump directly to the visible final Y difference.

Possible areas to inspect include:

- model-frame selection;
- projection branch;
- shape-owner mapping;
- first-displayed-level mapping;
- calibration;
- vertical offset application;
- endpoint normalization;
- portrait base position;
- common gameplay center;
- Level 3-specific accepted branch;
- displayed-level range check;
- generic reciprocal fallback;
- legacy fallback;
- final viewport translation;
- renderer-level translation.

The root cause must be expressed as a code path/value difference.

---

# Phase 4: explicitly test for legacy/generic fallback

Determine whether Level 19 enters any path used by:

- explicit legacy 4:3;
- baseline portrait;
- generic reciprocal projection;
- pre-production fallback;
- neutral calibration;
- unavailable-shape fallback;
- displayed-level range fallback.

If yes:

- identify exact condition;
- explain why Level 19 satisfies it;
- determine why Level 3 does not.

Do not merely call the output "legacy-looking."

---

# Phase 5: identify displayed-level versus canonical-shape keying

Search presentation code for places where Shape 3 behavior is still selected with semantic equivalents of:

```
displayedLevel == 3
```

rather than:

```
canonicalShape == Shape3
```

or row/owner equivalent.

Classify every relevant use.

Not all displayed-level checks are wrong:

- HUD identity;
- palette cycle;
- gameplay level;
- transition identity;

may correctly use displayed level.

Shape-specific:

- projection;
- model frame;
- geometric calibration;
- vertical composition;
- shape-local transform;

should normally resolve via canonical shape identity.

Determine whether the defect is another displayed-level-keyed shape behavior.

---

# Phase 6: compare with Level 18 success

Level 18 reused canonical Shape 2 successfully enough to pass manual acceptance on its first candidate.

Compare Shape 2 reuse:

```
Level 2 ↔ Level 18
```

against Shape 3 reuse:

```
Level 3 ↔ Level 19
```

Identify what architectural difference allowed Shape 2 effective reuse to work while Shape 3 effective reuse did not.

This is critical.

Do not merely fix Level 19.

Explain why the existing test/routing architecture caught or avoided the issue for Shape 2 but did not for Shape 3.

---

# Phase 7: determine why existing Level 19 test passed

Inspect:

`Level19ProductionTests.PresentationAndPaletteResolveThroughCanonicalShapeThree`

and every supporting presentation test added/used for Level 19.

Determine exactly what the test proves.

Explicitly state what it **does not** prove.

If it only verifies metadata/calibration selection, classify the gap:

> structural-resolution test passed while effective-output equivalence was untested.

Do not delete the structural test if it remains useful.

Add a stronger complementary test.

---

# Phase 8: bounded correction

After proving the cause, apply the smallest architectural correction.

Requirements:

- Level 3 numerical output remains unchanged.
- Level 19 uses the exact accepted Shape 3 production presentation under equivalent shape/model inputs.
- no Level 19-specific calibration;
- no Level 19-specific vertical offset;
- no screenshot-derived value;
- no gameplay changes;
- no palette changes;
- no HUD geometry changes.

Prefer:

- canonical-shape keying;
- shared shape-owner resolution;
- reusable projection/calibration/composition function.

Do not copy Level 3 code into a Level 19 branch.

---

# Phase 9: add generic effective-output equivalence protection

Add a reusable regression seam protecting repeated shapes.

At minimum prove effective production projection equivalence for:

```
Level 1 ↔ Level 17   Shape 1
Level 2 ↔ Level 18   Shape 2
Level 3 ↔ Level 19   Shape 3
```

For equivalent canonical model-space inputs:

- shape geometry identity is shared;
- model-frame result is equivalent;
- shape calibration is equivalent;
- projection family is equivalent;
- final screen-space tube coordinates are equivalent within a justified numeric tolerance;
- representative entity/lane coordinates remain registered.

Exclude deliberately level/cycle-specific differences:

- HUD displayed level;
- palette cycle;
- gameplay actor population.

The test should fail if a repeated level resolves correct metadata but then enters a different projection/vertical-placement path.

---

# Phase 10: future-shape regression design

Where architecture permits, create or design the test seam so future repeats can naturally extend to:

```
Shape 4: Level 4 ↔ Level 20
Shape 5: Level 5 ↔ Level 21
...
Shape 16: Level 16 ↔ Level 32
```

Do not implement Levels 20–32.

Do not add fake level configurations.

The regression helper should make repeated-shape effective equivalence easy to test as supported levels are added.

---

# Phase 11: entity registration

Confirm the correction preserves:

- Blaster tube registration;
- Eggs;
- Flippers;
- Tankers;
- Pulsars;
- player shots;
- enemy shots;
- rim entities.

Use representative deterministic projected points.

Do not visually inspect images.

---

# Phase 12: gameplay freeze validation

Prove no simulation change.

Run focused Level 19 gameplay tests covering:

- configuration;
- wave;
- `$19` route;
- Tanker cargo;
- Pulsars;
- firing;
- identity;
- reconstruction;
- completion;
- progression.

Results should be unchanged.

---

# Phase 13: prior-level presentation regressions

Run focused presentation coverage for:

- Level 1 / Shape 1;
- Level 17 / Shape 1;
- Level 2 / Shape 2;
- Level 18 / Shape 2;
- Level 3 / Shape 3;
- Level 19 / Shape 3;
- shared adapter/calibration/model-frame;
- HUD/palette separation;
- entity projection.

Preserve accepted Levels 1–18.

---

# Phase 14: recording regression

Run unmodified:

`TEMP/GameplayRecordings/lvl1-16.json`

and:

`TEMP/GameplayRecordings/lvl-17-test.json`

Require accepted current outcomes.

Presentation correction must not change simulation telemetry.

Do not modify recordings.

---

# Phase 15: automated validation

After focused gates:

1. full solution test suite with explicit hang timeout;
2. fresh non-incremental Release build;
3. `git diff --check`.

Require:

- zero failed tests;
- zero build errors;
- warnings reported exactly;
- no whitespace errors.

Do not launch the game.

---

# Manual acceptance handoff

After automated validation, stop for Neal.

Provide:

```
dotnet run --project src\Tempest.Game -- --start-level 19 --lives 10
```

Optional:

```
dotnet run --project src\Tempest.Game -- --start-level 19 --lives 10 --background-opacity .5
```

Also provide direct Level 3 command if useful for immediate A/B comparison.

Neutral observation statements:

- Observe Level 19 Shape 3 vertical placement matching the accepted Level 3 production composition.
- Observe Shape 3 size/perspective unchanged from the accepted canonical presentation.
- Observe tube/entities remaining co-registered.
- Observe cycle-1 palette and HUD still correct.
- Observe Level 19 gameplay unchanged.
- Observe no obvious regression in prior shapes.

Do not ask questions.

Do not wait for Neal's answer.

State:

> Automated correction is complete. Manual Level 19 presentation acceptance remains pending.

Then STOP.

---

# Required AI artifacts

Save this prompt as:

`AI-Logs/Prompts/2026-11-26-level19-shape3-effective-presentation-correction-prompt.md`

Create:

`AI-Logs/Reports/2026-11-26-level19-shape3-effective-presentation-correction-report.md`

Do not stage, commit, or push.

---

# Required report contents

1. Executive summary
2. Starting Git/worktree state
3. Manual defect evidence
4. Prior implementation claim
5. Accepted Level 3 Shape 3 authority
6. Level 3 end-to-end presentation trace
7. Level 19 end-to-end presentation trace
8. first numeric divergence
9. legacy/generic-fallback audit
10. displayed-level-keying audit
11. Level 2/18 successful-reuse comparison
12. reason Level 18 worked
13. reason Level 19 failed
14. existing Level 19 test analysis
15. exact test gap
16. root cause
17. production correction
18. why correction is shape-generic
19. Level 3 numerical preservation
20. Level 19 output equivalence
21. Shape 1 repeat equivalence result
22. Shape 2 repeat equivalence result
23. Shape 3 repeat equivalence result
24. entity-registration result
25. gameplay-freeze result
26. Level 19 focused gameplay result
27. prior presentation regressions
28. Levels 1–16 recording result
29. Level 17-origin recording result
30. full-suite result
31. Release-build result
32. `git diff --check`
33. files changed
34. tests added/changed
35. manual commands/checklist
36. manual acceptance status: pending
37. remaining uncertainties
38. final Git/worktree state
39. stage/commit/push: none
40. Level 20+ untouched
41. recommended next step

---

# Stop conditions

STOP rather than guessing if:

- Level 3 accepted presentation cannot be established;
- first effective numeric divergence cannot be found;
- correction would require a Level 19-only vertical value;
- correction requires gameplay/model geometry changes;
- Level 3 output changes unexpectedly;
- Shape 1/2 accepted repeats regress;
- gameplay recordings change;
- commands stall or security blocks validation.

Do not fix by visual tuning.

---

# Completion criteria

Complete only when:

- first effective Level 3/19 divergence is identified;
- exact root cause is proven;
- Level 19 obtains Shape 3 effective production placement through shape reuse;
- Level 3 remains numerically unchanged;
- repeated Shape 1/2/3 effective-equivalence protection exists;
- gameplay remains unchanged;
- recordings pass;
- focused tests pass;
- full suite passes;
- Release build succeeds with zero errors;
- `git diff --check` passes;
- no Level 19-specific calibration/offset is introduced;
- nothing staged/committed/pushed;
- manual presentation acceptance remains pending.

Then STOP.

TEMPEST-END-LEVEL19-SHAPE3-EFFECTIVE-PRESENTATION-CORRECTION-COMPLETE