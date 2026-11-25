Read this entire user-provided task before taking any action.

Before using tools, opening files, running commands, or changing the repository, locate the unique completion token near the end of this task. Verify that exactly one complete token beginning with `TEMPEST_` appears on a line by itself in the user-provided task. VS Code or agent-generated reminders, tagged-file information, attachment metadata, or other automatically appended context may appear after the token and do not invalidate it. This check is intended to detect an incomplete or truncated user task, not appended agent context. If exactly one complete token cannot be found, stop before any project or repository action and explain why.

# Investigate the Level 15 baseline discrepancy, establish the correct Spike evidence, and implement only the bounded Spike-height correction

## Current accepted manual result

Neal has now manually tested Level 15.

Record the following result accurately:

- Level 15 gameplay passes.
- No general Level 15 gameplay correction is requested.
- No broader Level 15 presentation redesign is requested.
- The accepted Level 15 tube-presentation values are:
  - `--tube-y-gain 1.15`
  - `--tube-scale 1`
  - `--tube-vertical-offset 75`
  - `--tube-depth-y-gain 1`
- Freeze those presentation values. Do not recalibrate, reinterpret, normalize, or replace them.
- The one identified remaining defect is that the Level 15 starting Spikes are significantly too long.
- Manual acceptance of the Spike correction remains pending until Neal tests the corrected candidate.

Treat “Level 15 passes” as acceptance of the tested gameplay and tube presentation except for the explicitly identified starting-Spike-length defect. Do not turn this into an unrestricted Level 15 audit or arcade-perfect claim.

## Required first phase: reconcile the baseline discrepancy

Before changing source or tests, investigate why the previous Spike-height task found a repository baseline that did not correspond to the Level 15 candidate Neal had just tested.

Use `AI-Logs/Reports/2026-11-24-level15-spike-height-correction-report.md` as the factual handoff from the blocked attempt.

That report states that the inspected repository contained only a development-only, direct-start Level 15 candidate using a Level 11 gameplay-parameter baseline, while the production campaign ended at Level 14. It also states that no source or test worktree changes were present and therefore stopped rather than modifying the wrong candidate.

Determine, with evidence:

1. The current branch, HEAD, upstream, ahead/behind state, index state, tracked worktree changes, and relevant untracked files.
2. Whether a newer, uncommitted Level 15 implementation exists now.
3. Which exact source, configuration, progression, geometry, presentation, test, or other files constitute the Level 15 candidate Neal manually tested.
4. Whether Level 15 is currently:
   - still a direct-start development candidate;
   - an uncommitted production candidate;
   - part of normal production progression;
   - or represented by a mixture of older direct-only and newer production work.
5. Why the previous task saw only the older direct-start/Level-11-derived baseline.
6. Whether the manually tested Level 15 candidate is available in the current worktree and can be identified unambiguously.
7. Whether any expected Level 15 files are missing, ignored, unstaged, located outside the active repository, or otherwise unavailable.
8. Whether the current candidate contains the accepted Level 15 presentation values listed above.
9. Whether the current starting-Spike implementation is genuinely Level 15-specific or still inherited from the prior Level 11 development baseline.

Distinguish clearly among:

- observed repository facts;
- ROM-backed evidence;
- existing project translations;
- inherited temporary baseline values;
- Neal’s manual acceptance;
- hypotheses;
- conclusions.

Do not silently reinterpret the manually tested candidate. Do not assume the earlier report was wrong merely because Neal has now tested Level 15. Explain the discrepancy from the actual current repository state.

## Fail-closed baseline gate

Do not modify gameplay or presentation unless the candidate Neal tested can be identified unambiguously.

If the current repository still lacks the tested Level 15 candidate, or if multiple competing Level 15 baselines cannot be reconciled safely:

- stop without changing production source or tests;
- preserve the worktree;
- report exactly what is present and missing;
- identify the files or state needed to continue;
- do not substitute the old Level 11-derived direct candidate;
- do not invent a new Level 15 implementation;
- do not adjust Spike height by visual guesswork;
- do not begin unrelated Level 15 work.

If the tested candidate is present and can be identified with high confidence, continue directly into the bounded evidence and correction phases below.

## Determine the proper Level 15 starting-Spike height

After resolving the baseline, determine the correct Level 15 starting-Spike height from the strongest available evidence.

Use this authority order:

1. Neal’s direct arcade observations where applicable.
2. Atari ROM data and the project-local Tempest disassembly.
3. Existing accepted project behavior and mathematically justified translation.
4. Relevant reference implementations only as secondary engineering evidence.

Do not search the web for ROM material. Use the project-local disassembly and existing repository evidence.

Trace the complete provenance of the current Level 15 Spike length:

- the Level 15 ROM level/parameter row;
- the raw Spike tip or height value, if present;
- the ROM routine or table that interprets it;
- any subtraction, inversion, normalization, depth scaling, coordinate conversion, or presentation mapping;
- the final C# configuration value;
- the initialization path that creates the starting Spikes;
- the rendering path that presents their visible length.

Compare this with the already accepted implementations for nearby levels, especially Levels 11 through 14, but do not assume that Level 15 must reuse any neighboring level’s value or formula.

The blocked report observed that the old development candidate used `0xB0 * Level.Depth / 224f`, associated with Level 11, while accepted Levels 11–14 used source expressions that subtract a raw tip coordinate from `0xF0` before depth scaling. Verify these facts against current source and ROM evidence rather than merely repeating the report.

Determine whether the excessive length comes from:

- the wrong raw Level 15 ROM value;
- reuse of the Level 11 baseline;
- an incorrect raw-tip-to-height conversion;
- a missing `0xF0` subtraction or equivalent;
- applying the correct value in the wrong coordinate domain;
- duplicate scaling;
- configuration selection;
- initialization;
- or a different specific cause supported by evidence.

Do not change tube geometry, depth projection, presentation calibration, or global Spike rendering to compensate for a Level 15 configuration error.

## Bounded implementation

If and only if the baseline is resolved and the proper Level 15 Spike height is supported by ROM/project evidence, implement the smallest justified correction.

The implementation must:

- correct only the Level 15 starting-Spike height or its narrowly responsible Level 15-specific derivation;
- preserve the number and lane placement of starting Spikes;
- preserve Spike lifecycle, extension, collision, scoring, destruction, exit behavior, and other established mechanics;
- preserve the Level 15 tube geometry and topology;
- preserve all accepted Level 15 enemy supply, admission, Tanker cargo, Spiker, Fuseball, Flipper, firing, Super Zapper, death/re-entry, completion, scoring, lives, HUD, and progression behavior;
- preserve the accepted Level 15 presentation values exactly:
  - Y gain `1.15`;
  - scale `1`;
  - vertical offset `75`;
  - depth-Y gain `1`;
- preserve Levels 1–14;
- preserve direct or production Level 15 behavior unrelated to Spike height;
- preserve any existing later-level candidate coverage;
- avoid changing shared/global Spike behavior unless evidence proves the defect is shared and such a change is necessary. If the necessary correction would have broader effects, stop and report rather than expanding scope.

Do not tune the final Spike value only until it “looks right.” The value and conversion must have documented evidence. Neal’s visual judgment remains the final manual acceptance authority after implementation.

Add maintainability-focused comments only where necessary to preserve the evidence and explain a non-obvious ROM-to-project translation. Do not add speculative comments.

## Tests and automated validation

Add or update narrowly relevant tests that prove:

1. The active Level 15 candidate uses the correct evidence-backed starting-Spike value.
2. The raw ROM value is translated through the intended formula.
3. The Level 15 starting-Spike count and lane placement remain correct.
4. Spike lifecycle and gameplay behavior are unchanged apart from initial length.
5. The accepted Level 15 presentation values remain:
   - Y gain `1.15`;
   - scale `1`;
   - vertical offset `75`;
   - depth-Y gain `1`.
6. Levels 11–14 retain their accepted Spike values and behavior.
7. Level 15 geometry, topology, progression/direct-start status, and production configuration remain consistent with the baseline established during the investigation.
8. Relevant playback, headless, presentation, ExitZoom, Approach, death/re-entry, and completion behavior do not regress.
9. Existing later-level development candidates remain unchanged where applicable.

Run bounded validation with explicit time or hang limits:

- the new or modified Level 15 Spike tests;
- relevant Level 11–15 configuration and production tests;
- Spike initialization, lifecycle, collision, and rendering-semantics tests;
- Level 15 geometry, topology, enemy selection, ledger, reconstruction, progression, start-level, HUD, calibration, and presentation tests;
- relevant Levels 1–14 regression coverage;
- deterministic Level 15 headless gameplay or playback coverage if the current architecture supports it without inventing new infrastructure;
- the complete Release test suite;
- a Release solution build;
- `git diff --check`.

Do not report a canceled, timed-out, stalled, blocked, skipped, or unrun command as passing. If a broad run fails, isolate it with progressively smaller bounded runs. If validation reveals a genuine defect outside the authorized Spike correction, stop and report it rather than expanding scope.

Do not launch the interactive game for manual acceptance. Neal may be away while this task runs. End with the exact launch command Neal should use to test Level 15 manually with the accepted presentation values.

## Manual acceptance handoff

The final handoff must ask Neal to verify only the relevant manual points:

1. Level 15 starts with appropriately shorter Spikes consistent with the arcade presentation.
2. Spike lane placement and count remain correct.
3. Normal Spike extension and interaction remain correct.
4. Gameplay still passes.
5. Tube shape and placement still match the accepted calibration.
6. No visual or gameplay regression is apparent.

Provide a launch command using:

`--tube-y-gain 1.15 --tube-scale 1 --tube-vertical-offset 75 --tube-depth-y-gain 1`

Use the correct Level 15 startup option based on the reconciled baseline. Do not assume `--level 15` or `--start-level 15`; establish which one corresponds to the candidate Neal tested and provide the appropriate command. If both are supported but have materially different semantics, explain that and provide the command for the accepted candidate first.

## Repository and evidence protection

Before any change, audit the repository state. Preserve unrelated tracked and untracked material.

Do not stage, commit, push, amend, reset, restore, stash, clean, rebase, switch branches, or rewrite history.

Do not modify or inspect unrelated screenshots, videos, recordings, personal scratch files, unknown image assets, MAME captures, UserData, Temp/TEMP, Videos, ROM binaries, or unrelated reference material.

The project-local disassembly and explicitly relevant ROM-derived text/data may be inspected for this task. Do not modify protected ROM or reference assets.

Do not analyze images or frames unless Neal explicitly provides or identifies them for this task. This task currently authorizes no image or frame analysis.

## Prompt and report records

Save this substantive prompt under `AI-Logs/Prompts` with a descriptive date-prefixed filename.

Save a paired report under `AI-Logs/Reports`. The report must include:

- starting Git/repository state;
- the exact baseline discrepancy;
- why the prior task saw the older candidate;
- the identity and status of the candidate Neal tested;
- the evidence chain for Level 15 Spike height;
- observed facts, hypotheses, and conclusions kept separate;
- files changed;
- tests added or updated;
- all validation commands and exact results;
- unresolved issues;
- manual acceptance status;
- the exact manual launch command;
- confirmation that the accepted presentation values were preserved;
- confirmation that no unrelated Level 15 work was performed;
- confirmation that nothing was staged, committed, or pushed.

If the baseline cannot be reconciled, save a blocked investigation report rather than an implementation report and make no production changes.

Do not begin Level 16 or any unrelated Level 15 milestone.

TEMPEST_LEVEL15_BASELINE_SPIKE_CORRECTION