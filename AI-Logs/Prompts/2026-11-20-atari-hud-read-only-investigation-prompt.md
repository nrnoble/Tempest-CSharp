# Atari HUD read-only investigation prompt

Date: 2026-11-20

## Scope

Perform a tightly bounded read-only investigation of the original Atari Tempest gameplay HUD and the current C# HUD/presentation architecture. Do not implement anything.

Investigate the target gameplay HUD:
- reserve-life Blaster symbols on the left;
- current score in green;
- fixed high score `38930` in green;
- fixed high-score initials `AAA` in green;
- current level in blue;
- vector-drawn uppercase characters and ROM-derived/recoverable Blaster and glyph geometry.

## Required investigation

1. Atari HUD routines, tables, state, and display-list construction.
2. Vector digit, letter, glyph, spacing, scale, intensity, and junction behavior.
3. Current-score conversion, leading-zero/blank handling, right justification, placement, and color.
4. High-score `38930` and initials `AAA` geometry and placement.
5. Current-level formatting, placement, color, and multi-digit support.
6. Reserve-life semantics, exact Blaster geometry/spacing/color, Atari lives limit, and any explicit Atari display cap.
7. Current C# architecture, font-based and vector-based text, state-to-renderer integration, separate portrait HUD camera, and legacy presentation preservation.
8. Evidence classification and bounded implementation recommendation.

## Constraints

- Read-only: no source, test, configuration, documentation, reference, or Git-state changes.
- Do not inspect `Reference/arcade/roms.rev2/`.
- Do not inspect unrelated screenshots, videos, recordings, `TEMP/`, `UserData/`, or public web ROM material.
- Do not launch the interactive game.
- Do not stage, commit, push, reset, restore, stash, clean, or otherwise modify repository state.
- Preserve accepted tube calibration, viewport, projection, Approach, ExitZoom, playback, and legacy presentation.
- Save this prompt and a paired investigation report under `AI-Logs/`; AI-Logs are permitted for this task and must not be staged or committed.
- Produce no production code and stop after the report.

## C# implementation boundaries supplied by the task

- The upcoming HUD display cap is temporary:
  - show `max(totalLives - 1, 0)` reserve-life symbols;
  - visually draw at most 25 symbols;
  - never alter underlying lives state;
  - left-justify symbols horizontally in the available HUD region;
  - omit symbols beyond 25.
- This 25-symbol rule is a C# project decision, not an Atari-derived limit.
- The original arcade maximum of six total lives is context only, not a C# restriction.
- The first implementation is fixed for high score `38930` and initials `AAA`.
- The live C# level and score remain dynamic.

Completion token: `TEMPEST_ATARI_HUD_READONLY_INVESTIGATION`
