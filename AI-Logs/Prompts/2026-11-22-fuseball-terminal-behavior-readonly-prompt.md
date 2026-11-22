Read this entire user-provided task before taking any action.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, files, commands, or taking repository action, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with TEMPEST_ appears on a line by itself in the user-provided task.

If exactly one complete token cannot be found, stop before any project or repository action and explain why.

# Task: Read-only investigation of terminal Fuseball behavior and all-rim interaction

Perform a tightly bounded READ-ONLY investigation.

Do not implement corrections.

Do not modify production source, tests, configuration, durable documentation, reference files, gameplay behavior, or Git state.

Continue from the current genuine Level 11 candidate after the accepted/fixed:

- direct-Fuseball runtime-representation crash;
- spinner-direction regression;
- Level 11 Atari model-frame crash;
- initial Spike-length correction;
- terminal Spiker-to-Tanker correction.

Those issues are not under investigation here.

## Manual observation to investigate

Neal reports a remaining non-blocking gameplay inconsistency concerning Fuseballs late in a wave.

Current C# observation:

- when a Fuseball is effectively the final remaining enemy obligation, or when the other remaining Flippers are already on the rim, the Fuseball may continue moving up and down the tube several additional times before finally coming completely to the rim;
- once it reaches the rim, normal exit behavior follows.

Arcade observation:

- Atari Tempest also allows the Fuseball to continue moving up/down before coming fully to the rim;
- Neal recalls approximately one or two additional up/down traversals;
- the exact number is uncertain;
- therefore do **not** encode “one” or “two” as an assumed arcade counter.

The issue does not prevent play or completion. The C# result appears longer/inconsistent relative to the arcade observation.

The objective is to determine the real Atari mechanism before deciding whether any correction is justified.

## Primary questions

Determine:

1. What exact Atari state causes a Fuseball to stop ordinary up/down cycling and begin its terminal movement to the rim?
2. Does Atari use:
- generic pending-enemy supply;
- number of active enemies;
- Fuseball being the final below-rim enemy;
- all other remaining Flippers being on the rim;
- an all-rim flag;
- unresolved enemy obligations;
- a dedicated hurry-up state;
- Fuseball endpoint state;
- a timer/counter;
- or some combination?
3. Does Atari explicitly count additional Fuseball cycles/traversals?
4. If not, can the observed one-or-two additional arcade traversals arise naturally from the Fuseball's position/state when the terminal condition becomes true?
5. What does current C# use for the equivalent decision?
6. Why can current C# permit several more cycles before the Fuseball finally reaches the rim?
7. Is the mismatch Level-11-specific or part of global C# Fuseball behavior?
8. Do existing earlier-level Fuseball implementations already contain the correct mechanism?

Do not assume the answer from visual observation alone.

## ROM/disassembly investigation

Trace the tracked Atari disassembly for the complete relevant Fuseball/Fuzzball lifecycle.

Investigate at minimum:

- Fuseball/Fuzzball admission;
- normal movement toward and away from the rim;
- depth or along-tube updates;
- endpoint handling;
- direction reversal;
- repeated traversal;
- any movement-program state;
- enemy-pending checks;
- active-enemy checks;
- rim-state checks;
- hurry-up behavior;
- any branch that forces progression toward the rim;
- conditions that suppress another reversal;
- conversion or state/type changes, if any;
- interaction with wave completion.

Determine whether the terminal condition is tested continuously, at an endpoint, at a state-transition boundary, or at another specific point in the movement program.

That timing may explain why the arcade Fuseball can continue one or more traversals after other enemy obligations appear effectively resolved.

## All-rim interaction

Investigate the Atari rule for the state Neal describes where all remaining Flippers are already on the rim while a Fuseball remains below the rim.

Determine:

- whether rim Flippers contribute to the relevant enemy/pending count;
- whether they are considered resolved for purposes of Fuseball terminal behavior;
- whether an “all enemies at rim” or equivalent state exists before the Fuseball itself reaches the rim;
- whether the Fuseball receives a special movement change when other enemies are at the rim;
- whether wave exit can begin while the Fuseball remains below the rim;
- whether the Fuseball must itself reach the rim or another terminal state before exit begins.

Keep simulation lifecycle and visual presentation distinct.

## Current C# investigation

Trace the current C# Fuseball path and corresponding wave-completion logic.

Determine:

- normal Fuseball depth-motion state;
- reversal trigger;
- cycle/repetition state;
- existing HurryUp behavior;
- exactly where and when HurryUp is set;
- which production levels can currently set it;
- whether genuine Level 11 production can set it;
- what HurryUp changes in Fuseball movement;
- whether the current Level 11 Fuseball repeatedly misses the terminal trigger because it is evaluated at the wrong lifecycle boundary;
- whether the trigger uses pending generic originals, active count, rim state, or another representation;
- whether all-rim completion and Fuseball terminal movement are incorrectly coupled or correctly separate.

Preserve the distinction between:

1. deciding the Fuseball should stop ordinary cycling;
2. moving the Fuseball toward the rim;
3. marking the Fuseball as rim/terminal;
4. allowing wave exit/completion.

## Compare Atari and C\

Produce a state-transition comparison rather than only a prose description.

For both Atari and C#, identify:

- ordinary Fuseball cycling state;
- trigger checked;
- point in the cycle where it is checked;
- state change caused by the trigger;
- subsequent motion;
- terminal/rim state;
- completion interaction.

Identify the **first semantic divergence**, rather than only the eventual visible difference.

## Global-versus-Level-11 classification

Determine whether the C# mismatch is:

- LEVEL-11-SPECIFIC
- GLOBAL FUSEBALL DEFECT
- EXISTING PROJECT APPROXIMATION
- EXPECTED BEHAVIOR
- INCONCLUSIVE

If the mechanism exists globally but Level 11 is not wired into it, say that explicitly.

If earlier levels cannot expose the behavior because of their enemy composition, distinguish “global code path” from “previously observable level.”

## Cycle-count question

Specifically answer:

**Does the Atari ROM encode an explicit number of extra Fuseball up/down traversals after the terminal condition becomes true?**

If yes:

- identify the counter/state and exact semantics.

If no:

- explain how variable apparent extra traversals arise from the point in the Fuseball cycle where the terminal condition is detected.

Do not transform Neal's approximate “1–2” observation into an implementation constant without ROM support.

## Historical evidence

Review relevant tracked project records concerning:

- Fuseball behavior;
- hurry-up;
- all-rim completion;
- Level 4-9 Fuseball-related behavior where applicable;
- Level 11 direct Fuseball implementation and identity correction;
- completion obligations.

Clearly distinguish earlier PROJECT DECISION behavior from ROM-backed facts.

## ROM-access boundary

Prefer the tracked textual disassembly and existing tracked reports.

If a specific missing byte/routine that is essential to answer the question is omitted from the tracked textual evidence, a tightly bounded read-only access to the already-authorized local Rev. 2 program image is permitted.

Do not enumerate the ROM directory.

Do not perform broad ROM mining.

Do not copy, modify, stage, commit, or publish ROM material.

Do not inspect Reference/arcade/MBUCOD.V05.txt.

## Deterministic C# observation

If static C# inspection cannot establish the current behavior, a bounded deterministic/headless Level 11 reproduction is permitted.

Do not launch the interactive game.

Do not ask Neal to reproduce the behavior again.

Do not permanently change production code or tests.

Temporary instrumentation must be removed before completion.

A deterministic reproduction should focus only on obtaining the missing state-transition evidence, not on tuning gameplay.

## Scope exclusions

Do not change:

- Fuseball code;
- wave-completion code;
- all-rim logic;
- Level 11 geometry;
- spinner behavior;
- Atari presentation;
- Spike behavior;
- Spiker-to-Tanker behavior;
- Flipper/Tanker cargo behavior;
- enemy selection limits;
- HUD;
- progression;
- Level 15.

Do not encode the current Level 11 presentation values in this investigation.

Do not inspect screenshots, images, videos, UserData, or Temp.

## Report

Produce a focused report containing:

### 1. Atari Fuseball terminal mechanism

Identify the exact trigger, state transition, timing point, and subsequent movement.

### 2. Atari all-rim relationship

Explain how rim Flippers and a below-rim Fuseball interact with terminal movement and wave completion.

### 3. Extra-cycle behavior

State whether Atari explicitly counts extra cycles and explain the observed apparent extra traversals.

### 4. Current C# mechanism

Identify the equivalent logic and current Level 11 behavior.

### 5. First semantic divergence

Pinpoint the earliest state/condition where C# differs from Atari.

### 6. Scope classification

State whether the issue is Level-11-specific or global.

### 7. Implementation readiness

Choose exactly one:

- READY FOR BOUNDED CORRECTION
- NEEDS MANUAL ARCADE OBSERVATION
- NO CORRECTION JUSTIFIED
- STILL UNRESOLVED

If ready, specify the smallest evidence-supported correction and focused regression tests.

Do not implement it.

## Records and Git

Save this prompt and investigation report under the tracked AI-Logs convention.

Do not stage, commit, or push.

Preserve unrelated tracked/untracked work and historical records.

Do not reset, restore, stash, clean, or rebase.

Stop after the read-only investigation report.

TEMPEST_FUSEBALL_TERMINAL_BEHAVIOR_READONLY