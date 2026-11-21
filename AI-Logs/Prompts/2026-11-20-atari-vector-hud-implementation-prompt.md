Read this entire user-provided task before taking any action.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, files, commands, or taking any repository action, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with TEMPEST_ appears on a line by itself in the user-provided task. VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context appearing after the token do not invalidate the token.

This check exists to detect an incomplete or truncated user task, not appended agent context.

If exactly one complete token cannot be found, stop before any project or repository action and explain why.

# Task: Implement the first Atari-presentation vector gameplay HUD

Implement a tightly bounded first version of the Atari Tempest gameplay HUD for Atari presentation.

Use the completed read-only investigation as the primary implementation handoff:

AI-Logs/Reports/2026-11-20-atari-hud-read-only-investigation-report.md

Read that report before designing or changing code.

Use the project-local tracked disassembly and textual reference material identified by that investigation as needed.

Do not inspect or access:

Reference/arcade/roms.rev2/

Do not access excluded copyrighted local reference files.

Do not search for public ROM binaries or ROM dumps.

## Important visual-input rule

Do not search for, enumerate, open, or analyze screenshots, images, videos, frames, UserData, Temp, or other visual assets.

No screenshot path is supplied or required for this implementation.

The visual requirements derived from Neal's reference images are stated directly in this task. Implement those requirements without attempting to locate the images.

## Objective

Replace the existing font-rendered gameplay HUD in Atari presentation with arcade-informed vector drawing.

The first implementation must display:

- yellow reserve-life Blaster symbols;
- live current score in green;
- fixed high score 38930 in green;
- fixed high-score initials AAA in green;
- live current level in blue.

The HUD must use vector strokes.

Do not use Raylib font rendering for these Atari-presentation gameplay HUD elements.

Alphabetic characters must be uppercase.

The characters and digits must use block-like vector construction based on the recoverable Atari DVG glyph evidence identified in the investigation.

This is a first bounded implementation. It must be structurally faithful and visually testable, but it does not need to achieve a pixel-perfect arcade match before Neal sees it.

## Preserve accepted presentation

The Level 1-10 Atari-presentation calibration is accepted and frozen.

Do not change:

- tube Y gain;
- tube scale;
- tube vertical offset;
- per-level calibration values;
- playfield camera;
- portrait viewport dimensions;
- perspective;
- tube projection;
- tube geometry;
- Approach;
- ExitZoom;
- entity registration;
- HUD-reserved upper-region dimensions.

The new HUD must use the existing separate Atari portrait HUD camera and reserved upper information region.

Do not move, resize, recalibrate, or reproject the accepted tubes to accommodate the HUD.

## ROM-informed vector glyphs

Implement the smallest maintainable HUD-specific vector-glyph path needed for this milestone.

Use the ROM/disassembly evidence identified in the investigation for:

- digits 0 through 9;
- uppercase A;
- glyph dimensions;
- stroke construction;
- advance behavior;
- spacing where established;
- score-field behavior;
- reserve-life Blaster geometry.

Do not substitute a conventional font.

Do not simply reuse current project or Incitti-derived glyph geometry as if it were ROM-backed unless comparison proves it accurately represents the identified Atari HUD geometry.

Do not construct a screenshot-fitted alphabet.

The first implementation needs digits 0-9 and uppercase A. Do not implement an entire unused alphabet merely for completeness unless the cleanest ROM-table translation naturally provides it without materially expanding scope.

Represent ROM-derived or ROM-translated glyph data once in a clear and maintainable structure. Do not scatter unexplained line segments throughout the renderer.

Clearly distinguish in comments:

- ROM/DISASSEMBLY BACKED geometry and behavior;
- C# coordinate conversion;
- PROJECT TRANSLATION scale, placement, and RGB color choices.

## Vector-monitor junction artifact

Neal believes the slightly brighter dots at some line junctions are vector-monitor artifacts.

Treat that as a plausible visual interpretation, not as a proven explicit ROM feature.

If the existing vector-rendering path naturally produces suitable overlap or endpoint emphasis without special-case complexity, preserve that result.

A small, restrained simulation of brighter junction or endpoint intensity is permitted only if:

- it is isolated and maintainable;
- it does not distort glyph geometry;
- it does not materially expand the task;
- it can be removed or adjusted independently;
- it does not require repeated tuning.

Do not make the junction dots a required acceptance criterion.

Do not delay or complicate the first implementation to reproduce them.

If no simple justified implementation is available, omit them and report that decision.

## Current score

Render the live C# score from the existing score state.

Requirements:

- green vector digits;
- right-justified;
- leading positions blank rather than filled with zeroes;
- layout based on the Atari six-position score field;
- live updates as score changes during play;
- finite and deterministic vector submissions.

Do not change scoring mechanics.

Do not change score storage.

Do not add score persistence.

Do not implement high-score qualification.

For values that exceed the six-position Atari field, choose the smallest safe deterministic display behavior consistent with the current C# score type and document it. Do not mutate or clamp the underlying score state.

## Fixed high score and initials

Render:

38930 AAA

Requirements:

- green vector characters;
- high score placed in the Atari-informed high-score field;
- initials positioned to the right with Atari-informed spacing;
- digits created through the HUD vector-digit path;
- each A created through the ROM-informed uppercase-A path.

These values remain fixed for this milestone.

Do not connect them to persistent high-score state.

Do not implement leaderboard behavior.

Do not implement initials entry.

Do not implement high-score qualification or replacement.

## Current level

Render the live displayed level from:

CurrentLevel.Configuration.DisplayedLevel

Requirements:

- blue vector digits;
- Atari-informed placement beneath or relative to the high-score/initials region;
- support at least two displayed digits;
- live updates as the displayed level changes;
- no changes to progression mechanics.

If the C# diagnostic or future state exceeds the Atari two-character field, use a deterministic display policy and document it without mutating the current level.

## Reserve-life Blaster symbols

Use the ROM-informed nominal Player/Blaster vector geometry identified by the investigation.

Render reserve lives only.

Calculate the desired reserve count as:

- total live C# lives minus the active Blaster;
- never less than zero.

Equivalent semantic requirement:

- 3 total lives means 2 rendered HUD Blaster symbols;
- 1 total life means 0 rendered HUD Blaster symbols;
- 0 total lives means 0 rendered HUD Blaster symbols.

Requirements:

- yellow vector Blaster symbols;
- left-justified;
- advance horizontally across the HUD;
- use ROM-informed geometry and spacing where established;
- display at most 25 reserve-life symbols;
- never modify, clamp, or otherwise limit the underlying C# lives count;
- symbols beyond the temporary display cap are not drawn.

The 25-symbol display cap is a PROJECT DECISION, not an Atari-derived rule.

The original Atari six-total-life behavior and six HUD slots are reference behavior only and must not restrict C# lives state.

If the 25th symbol would extend beyond the portrait HUD boundary, preserve memory state and keep rendering bounded safely. Do not draw outside the intended viewport. Clearly report whether the practical geometry boundary or the explicit 25-symbol cap was reached first.

Do not introduce scrolling, wrapping, multiple rows, numeric life text, or automatic symbol shrinking.

## Colors

Use the target appearance established by Neal:

- current score: green;
- high score: green;
- initials: green;
- current level: blue;
- reserve-life Blaster symbols: yellow.

Use the strongest existing project color/palette evidence available.

The Atari DVG color-stat values are ROM-backed, but conversion to Raylib RGB is a PROJECT TRANSLATION.

Keep the RGB choices centralized and easy to adjust after manual testing.

Do not add a broad color-calibration system.

## Layout and spacing

Reproduce the Atari-informed composition reasonably closely:

- reserve-life symbols begin at the left;
- current score occupies the large left/center score field and is right-justified;
- high score appears to the right of the current score;
- initials appear to the right of the high score;
- current level appears below the high-score/initials area.

Preserve clear separation from the playfield.

Use explicit HUD-layout constants in the 600x800 logical portrait HUD coordinate system rather than mixing host pixels, playfield coordinates, and HUD coordinates.

Keep size, spacing, and placement values centralized for efficient manual adjustment after the first visual test.

Do not introduce automatic tube repositioning or HUD-driven playfield layout.

## Presentation-mode boundary

Apply the new vector gameplay HUD only to Atari/experimental portrait presentation.

Preserve the existing legacy HUD behavior unless a narrow shared helper can be introduced without changing legacy output.

Do not remove or rewrite unrelated diagnostics or overlays.

Existing wave/development diagnostics remain outside this milestone. Ensure they do not accidentally become part of the arcade HUD design.

## Gameplay and state boundaries

Do not change:

- scoring rules;
- score timing;
- lives mechanics;
- player-death behavior;
- extra-life behavior;
- progression;
- enemy behavior;
- gameplay timing;
- collision behavior;
- recording schema;
- deterministic playback state;
- screenshot paths or naming;
- pause behavior;
- Game Over behavior;
- attract mode.

The HUD reads existing state and renders it. The HUD must not own or mutate gameplay state.

## Testing

Add focused automated coverage for the new Atari HUD.

Verify at minimum:

1. Atari gameplay HUD elements use vector submissions rather than Raylib text calls.
2. Score uses live C# state.
3. Score is formatted into a fixed right-justified field with leading blanks.
4. Score vector output is deterministic and finite.
5. High score displays fixed 38930.
6. Initials display fixed uppercase AAA.
7. Level uses the live displayed-level state.
8. Single-digit and multi-digit levels produce deterministic finite geometry.
9. Reserve-life count excludes the active Blaster.
10. Total lives 3 produces 2 reserve symbols.
11. Total lives 1 produces 0 reserve symbols.
12. Total lives 0 safely produces 0 reserve symbols.
13. More than 26 total lives does not produce more than 25 reserve symbols.
14. The display cap does not modify Player.Lives.
15. Reserve-life symbols are left-justified and retain deterministic spacing.
16. HUD output remains within the portrait viewport.
17. Legacy HUD presentation remains unchanged.
18. Accepted tube calibration values and playfield projection remain unchanged.
19. Gameplay/model state remains unchanged by HUD rendering.
20. Visible playback and normal gameplay can update score and level without changing playback determinism.
21. Existing F9/F10 screenshot behavior remains valid.
22. Existing Approach, ExitZoom, calibration, viewport, and presentation regressions remain valid.

If an optional junction-artifact simulation is included, add isolated deterministic geometry/intensity coverage for it without making it a broad visual-fidelity test.

Run bounded:

- focused HUD and vector-glyph tests;
- relevant presentation, viewport, playback, screenshot, Approach, ExitZoom, and calibration regressions;
- complete solution test suite;
- Release solution build;
- git diff --check.

Use established hang timeouts and project stall/security rules.

Do not weaken Windows security.

Do not enter same-artifact retry loops.

Do not launch the interactive game.

## Manual handoff

After automated validation, provide concise commands for Neal to inspect:

1. direct Level 1 Atari presentation with the default starting lives;
2. Level 1 with a larger development lives count so reserve-symbol spacing and the 25-symbol cap can be judged;
3. a multi-digit direct level such as Level 10;
4. visible playback where score and displayed level change.

Use only established CLI options and real repository playback-file patterns.

Do not invent an option or recording filename.

Manual acceptance must explicitly cover:

- glyph appearance;
- character scale;
- current-score right justification;
- high-score/initial spacing;
- level placement;
- reserve-life symbol geometry and spacing;
- colors;
- overall composition;
- no movement or regression of the accepted tube presentation.

The optional junction artifact is not an acceptance blocker.

## Records and repository protection

Save this substantial prompt and its implementation report under the tracked AI-Logs prompt/report convention.

Do not stage, commit, or push during this implementation task.

Preserve unrelated tracked and untracked worktree material.

The completed post-push AI-Logs tracking report may already be a tracked worktree change. Preserve it and do not mix it into this implementation candidate.

Do not stage, reset, restore, stash, clean, rebase, or alter unrelated material.

Stop after implementation, automated validation, and manual-test handoff.

TEMPEST_ATARI_VECTOR_HUD_IMPLEMENTATION