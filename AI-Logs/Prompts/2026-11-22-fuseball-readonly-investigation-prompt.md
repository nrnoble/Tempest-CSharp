# 2026-11-22 Fuseball Visual-Animation Read-Only Investigation Prompt

Read-only investigation of the Atari Fuseball visible animation.

## Scope

- Determine frame-to-frame changes in the authorized Level 11 Fuseball reference sequence.
- Compare the Atari/ROM-disassembly animation model with the current C# Fuseball renderer.
- Identify the first meaningful visual/rendering divergence.
- Recommend the smallest evidence-supported implementation without changing code.
- Use only `UserData/Video/Level-11-Fuseball/tempest-lvl9-12_5691.png` through `_5744.png`.
- Do not inspect unrelated images, protected scratch material, raw ROM files, or `Reference/arcade/MBUCOD.V05.txt`.
- Do not implement or modify production source, tests, configuration, durable documentation, reference data, gameplay behavior, or Git state.

## Constraints

Preserve Level 11 gameplay, presentation, projection, calibration, playback behavior, and completion state. The task is visual-only; no collision, movement, timer, approach, ExitZoom, viewport, HUD, or enemy-lifecycle changes are authorized.

Create the paired AI-Logs prompt and report records required by the repository instructions, but do not stage or commit them.

TEMPEST_FUSEBALL_VISUAL_ANIMATION_READONLY
