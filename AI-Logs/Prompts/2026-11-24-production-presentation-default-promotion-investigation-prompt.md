# Tempest Production-Presentation Default Promotion — Read-Only Architecture Investigation

## FAIL-CLOSED COMPLETION-TOKEN CHECK

Read the entire user-provided task before taking any action.

Before using tools, opening files, running commands, modifying files, or performing any repository action:

1. Locate the completion token near the end of this user-provided task.
2. Verify that exactly one complete token beginning with `TEMPEST_` appears on a line by itself in the user-provided task.
3. VS Code / agent-generated reminders, tagged-file information, attachment metadata, current-editor context, or other system-generated context appearing after the token do not invalidate it.
4. This check exists to detect an incomplete or truncated user task, not appended agent context.
5. If exactly one complete token cannot be found, STOP before any project or repository action and explain why.

---

# Task

Perform a **read-only architecture investigation** to determine the smallest safe way to promote the currently accepted `--atari-presentation` path to the normal/default production presentation.

Do not implement the change in this task.

Do not modify production code.

Do not modify tests.

Do not modify durable documentation.

Do not stage, commit, or push.

Do not launch the interactive game.

Do not investigate or implement Level 17 or later levels.

The purpose is to establish exactly what currently depends on `--atari-presentation` being opt-in and determine the bounded implementation scope for making the accepted presentation the normal default.

---

# User intent

The accepted Atari-presentation path is no longer considered an experimental presentation that Neal expects to enable selectively.

Neal's intended final game should use this accepted presentation normally.

Currently, continued development would require specifying `--atari-presentation` for nearly every ordinary runtime launch.

Neal sees the old/default presentation path primarily as a possible diagnostic A/B tool, for example when isolating whether a runtime failure is presentation-specific.

The goal is therefore to investigate whether the relationship should now be inverted:

**Normal production launch**

→ accepted Atari presentation automatically.

**Explicit diagnostic fallback, only if still useful**

→ old/legacy presentation path.

Do not assume the old path must remain.

Do not assume it should be deleted.

Determine that from the current architecture and dependencies.

---

# Starting authority

Begin from the clean accepted production checkpoint established by the completed Level 16 closeout commit.

Read:

`AI-Logs/Reports/2026-11-24-level16-final-closeout-report.md`

Also inspect current durable documentation and current production source/tests as needed.

The closeout establishes that:

- Level 16 gameplay is manually accepted.
- Level 16 Atari presentation is manually accepted.
- The accepted Level 16 calibration defaults are encoded.
- Level 16 has been validated, committed, pushed, and independently verified.
- Level 16 is the current completed production boundary.
- Level 17+ work was not started by the closeout.

Do not reopen Level 16 implementation or presentation acceptance during this investigation.

---

# Repository safety

Before investigating, record:

- current branch;
- HEAD;
- upstream;
- ahead/behind state where available;
- index state;
- tracked modifications;
- untracked files relevant to the task.

The previous closeout deliberately left unrelated historical/working material outside its commit.

Preserve all such material.

Do not clean, reset, restore, stage, commit, push, or modify anything.

If current repository state differs materially from the completed Level 16 checkpoint in a way that prevents reliable analysis, report the difference rather than silently assuming a clean checkout.

---

# Investigation 1: Define exactly what `--atari-presentation` controls today

Trace the option from command-line parsing through runtime selection.

Determine exactly:

- where the option is parsed;
- what state it sets;
- where the renderer branches on it;
- which projection path is selected when it is absent;
- which projection path is selected when it is present;
- whether HUD behavior depends on it;
- whether entity rendering depends on it;
- whether tube projection depends on it;
- whether approach/exit presentation depends on it;
- whether Blaster/Flipper projection depends on it;
- whether any gameplay/simulation behavior depends on it;
- whether window initialization depends on it;
- whether direct startup/progression depends on it.

Produce a concise architecture map:

command-line option → runtime state → renderer/presentation branches → affected systems.

Distinguish render-only consequences from gameplay consequences.

---

# Investigation 2: Characterize the current default/legacy presentation path

Determine what the application actually does today when `--atari-presentation` is absent.

Identify:

- the relevant renderer/projection methods;
- whether it is an intentionally maintained presentation mode or primarily a historical baseline;
- which current tests depend on it;
- whether current documentation presents it as the normal production mode;
- whether any accepted gameplay behavior depends on that projection rather than model geometry;
- whether any diagnostics specifically require this path.

Do not label it obsolete merely because Neal intends Atari presentation to become default.

Report its actual remaining technical role.

---

# Investigation 3: Characterize the accepted Atari-presentation path

Determine exactly which accepted production systems now rely on the Atari-presentation path across Levels 1–16.

Inspect the current level/presentation mappings and calibration tables.

Determine:

- whether all production Levels 1–16 have valid presentation support;
- whether all accepted per-level defaults are resolved automatically once Atari presentation is active;
- whether any level still deliberately falls back to a legacy projection internally;
- whether Levels 5/6 or other levels retain special presentation architecture;
- whether an external caller has to know or care about those internal fallbacks;
- whether turning Atari presentation on by default would produce the manually accepted presentation for every current production level.

Do not infer manual acceptance solely from automated mappings. Use durable acceptance documentation to distinguish mapped from manually accepted.

---

# Investigation 4: Audit options that currently depend on `--atari-presentation`

Search all command-line/debug/calibration options for explicit or implicit dependencies on `--atari-presentation`.

At minimum inspect:

- `--calibration-guides`
- `--tube-y-gain`
- `--tube-scale`
- `--tube-vertical-offset`
- `--tube-depth-y-gain`
- HUD calibration controls
- presentation perspective controls
- recording/playback options
- direct-level/start-level options
- screenshot/pause diagnostics
- `--background-opacity`

For each option report:

1. Does it require `--atari-presentation` today?
2. Why?
3. Would that requirement become redundant if accepted production presentation were the default?
4. Would changing/removing the requirement alter semantics or merely remove redundant syntax?
5. Should the option remain development-only?

Do not change any parser behavior in this task.

---

# Investigation 5: Recording and playback compatibility

Inspect deterministic recording/playback and visible playback handling.

Determine whether presentation mode:

- is stored in a recording;
- is inferred from command-line startup;
- affects deterministic simulation state;
- affects recording compatibility validation;
- affects playback CLI parsing;
- changes fingerprints or simulation results;
- is used only for rendering during playback.

Determine what promoting Atari presentation to default would mean for existing recordings.

Do not change the recording format unless later evidence demonstrates that is necessary.

Prefer compatibility with existing recordings.

---

# Investigation 6: Headless and automated test assumptions

Audit tests for assumptions such as:

- absence of `--atari-presentation` means legacy projection;
- explicit `--atari-presentation` is required to reach accepted presentation;
- calibration options must reject use without `--atari-presentation`;
- headless simulation assumes presentation disabled;
- screenshots or projection tests explicitly select presentation mode;
- startup/help text describes Atari presentation as a candidate.

Classify tests into:

1. tests of currently accepted production behavior;
2. deliberate legacy/A-B diagnostic tests;
3. parser/CLI tests;
4. stale assumptions likely to change when the default changes;
5. tests that should remain unchanged because they concern simulation, not presentation.

Do not modify tests in this investigation.

---

# Investigation 7: Determine whether the legacy presentation still has useful diagnostic value

Identify concrete current uses of the old/default presentation path.

Specifically determine whether it remains useful for:

- isolating presentation-specific runtime crashes;
- projection A/B comparisons;
- verifying gameplay simulation independently of presentation;
- renderer regression isolation;
- other existing diagnostics.

Then determine whether those needs actually require the legacy presentation to remain the default.

Evaluate these architecture choices:

## Model A: Promote Atari presentation to default, retain explicit legacy diagnostic switch

Normal launch uses accepted presentation.

A development-only explicit option selects the old renderer when needed.

## Model B: Promote Atari presentation to default and remove runtime access to the legacy presentation

Legacy implementation may then be removed now or in a later cleanup if it has no remaining technical value.

## Model C: Keep `--atari-presentation` opt-in

Retain only if a concrete technical dependency makes promotion unsafe or meaningfully premature.

Do not prefer Model A merely for caution.

Do not prefer Model B merely for cleanliness.

Base the recommendation on actual dependencies.

---

# Investigation 8: Determine semantics of the existing `--atari-presentation` option after promotion

If Atari presentation becomes the default, determine what should happen to the current option.

Evaluate explicitly:

- remove `--atari-presentation`;
- accept it temporarily as a harmless deprecated/no-op compatibility option;
- reject it as obsolete;
- retain it with changed semantics;
- another narrowly justified transition strategy.

Consider:

- scripts/commands documented in README;
- saved AI/manual command lines;
- recording/playback invocation;
- parser tests;
- user workflow.

Do not implement any option transition in this task.

Recommend the least confusing final behavior.

---

# Investigation 9: Help/startup/UI wording

Find all user-visible text that describes the accepted presentation as:

- a candidate;
- development-only;
- opt-in;
- supported on a limited production range;
- enabled only by `--atari-presentation`.

Determine what wording would become stale if the accepted presentation becomes production default.

Also inspect:

- `--help` output;
- startup console messages;
- README commands;
- calibration instructions.

Do not edit them yet.

Report the required wording scope for implementation.

---

# Investigation 10: Default-promotion implementation surface

Based on the architecture, identify the smallest production/test/documentation change required to make accepted presentation the normal default.

The implementation recommendation should specify:

- parser/default selection change;
- renderer wiring change, if any;
- calibration resolver impact;
- diagnostic-option impact;
- recording/playback impact;
- legacy-mode strategy;
- test updates;
- help/startup wording updates;
- README/durable-documentation updates.

Prefer changing the **selection/default seam** over duplicating or rewriting the accepted presentation implementation.

Do not redesign the renderer unless the investigation proves that changing the default cannot be isolated.

---

# Important non-goals

Do not:

- change any Level 1–16 accepted calibration;
- revisit tube geometry;
- revisit HUD placement;
- revisit ExitZoom;
- alter gameplay;
- alter enemy behavior;
- alter difficulty/timing;
- alter recording data without necessity;
- implement Level 17+;
- investigate tube recycling;
- redesign the overall renderer;
- remove useful development diagnostics without evidence;
- use screenshots/images/videos;
- launch the game.

This task answers one architecture question only:

**Is the accepted Atari presentation ready to become the normal production default, and exactly what is required to do that safely?**

---

# Validation during this read-only investigation

Do not run the full automated suite merely for ceremony.

Use targeted source/test inspection.

Run bounded read-only commands or narrowly relevant existing tests only if necessary to establish an architectural fact that source inspection cannot answer.

Do not modify tests.

If no runtime validation is needed, say so.

---

# Required report

Save this prompt as:

`AI-Logs/Prompts/2026-11-24-production-presentation-default-promotion-investigation-prompt.md`

Create:

`AI-Logs/Reports/2026-11-24-production-presentation-default-promotion-investigation-report.md`

The report must include:

1. Executive summary
2. Starting Git/repository state
3. Exact current `--atari-presentation` parsing/selection flow
4. Current default/legacy presentation architecture
5. Accepted Atari-presentation architecture
6. Levels 1–16 production-support/acceptance status relevant to default promotion
7. Complete option-dependency audit
8. Recording/playback dependency audit
9. Headless/test assumption audit
10. Concrete remaining uses of legacy presentation
11. Whether legacy presentation needs to remain accessible
12. Model A/B/C evaluation
13. Recommended default-promotion model
14. Recommended fate of `--atari-presentation`
15. Any recommended legacy diagnostic option and why, if warranted
16. Required help/startup/README wording changes
17. Smallest implementation surface
18. Tests that would need to change
19. Tests that should remain unchanged
20. Recording/backward-compatibility implications
21. Risks and stop conditions for implementation
22. Files/resources inspected
23. Commands/tests run, if any
24. Confirmation no production code changed
25. Confirmation no tests changed
26. Confirmation no durable documentation changed
27. Confirmation nothing was staged, committed, or pushed
28. Confirmation Level 17+ was neither implemented nor investigated
29. Clear recommendation whether to proceed with a bounded implementation

Keep current facts separate from recommendations.

Classify recommendations as project design decisions, not historical facts.

---

# Stop condition

STOP after completing the report.

Do not implement default promotion.

Do not create a legacy switch.

Do not remove `--atari-presentation`.

Do not alter parser behavior.

Do not alter tests.

Do not proceed to another development milestone.

Neal will review the investigation and separately authorize implementation.

TEMPEST_PRODUCTION_PRESENTATION_DEFAULT_PROMOTION_INVESTIGATION_COMPLETE
