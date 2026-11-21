Read this entire user-provided task before taking any action.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, reading files, running commands, or taking repository action, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with TEMPEST_ appears on a line by itself in the user-provided task. VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context after the token do not invalidate it.

This check exists to detect an incomplete or truncated user task, not appended agent context.

If exactly one complete token cannot be found, stop before any project or repository action and explain why.

# Task: Refine Atari vector HUD scale, layout, spacing, and diagnostic placement

Continue from the current uncommitted Atari vector gameplay HUD candidate.

The first implementation follows the requested functional and architectural direction:

- ROM-informed vector digits;
- ROM-informed uppercase A;
- ROM-informed reserve-life Blaster geometry;
- live current score;
- fixed high score 38930;
- fixed initials AAA;
- live displayed level;
- reserve lives excluding the active Blaster;
- 25-symbol display cap without changing the underlying lives count;
- separate Atari portrait HUD camera;
- legacy HUD preserved.

Automated validation passed, but Neal's first manual visual test found that a focused presentation refinement is required.

Do not redesign or replace the vector HUD architecture.

Do not revisit the ROM investigation unless required to understand an already implemented geometry detail.

## Exact authorized visual references

For this task only, inspect these exact two files:

UserData\Screenshots\Level-2\reference\Protoype-2 HUD only.png

UserData\Screenshots\Level-2\reference\Protoype-2.png

These paths and filenames are intentional, including the spelling Protoype.

You are explicitly authorized to open and analyze only those two image files.

Do not search, enumerate, or inspect any other image, screenshot, video, recording, frame, UserData file, Temp file, or visual asset.

If either exact file is unavailable, stop the image-dependent portion and report which exact path could not be opened. Do not search for a substitute.

Use the two authorized prototypes as composition and relative-scale references, not as instructions to trace or replace ROM-derived glyph geometry.

## Manual findings to preserve

Neal's first manual test established:

- the reserve-life Blasters are well drawn;
- the reserve-life Blasters should be larger;
- the HUD lettering and digits are too large;
- the HUD field positions are substantially wrong;
- the overall layout needs to follow the supplied prototype more closely;
- the current diagnostic text inside the portrait viewport must be moved outside the portrait viewport.

These findings are authoritative manual observations for this refinement.

## Preserve the existing vector geometry and behavior

Retain the existing ROM/disassembly-informed geometry for:

- digits 0 through 9;
- uppercase A;
- reserve-life Blaster symbols.

Do not redraw those elements from the prototype images.

Do not substitute a conventional font.

Do not use screenshot tracing.

Do not change score, lives, level, progression, or rendering-state semantics.

The goal is to adjust:

- field scale;
- field anchor position;
- inter-field spacing;
- reserve-life symbol scale;
- reserve-life symbol spacing;
- development-diagnostic overlay placement.

## HUD composition target

Use the authorized prototype images to establish the intended relationships.

### Current score

The live current score must:

- use green ROM-informed vector digits;
- remain right-justified within its own fixed score field;
- use leading blanks rather than visible leading zeroes;
- appear substantially larger than the high-score digits, but smaller than in the current first implementation if required by the prototype;
- occupy the large upper-left/center HUD region;
- end at a stable right edge so score growth proceeds leftward;
- update live without shifting the other HUD fields.

Do not interpret “right-justified” as right-aligning the current score against the entire portrait viewport.

Right-justify it within an explicit current-score field whose visual placement is derived from the authorized prototypes.

### Reserve-life Blasters

The reserve-life Blasters must:

- retain the existing well-drawn ROM-informed shape;
- be larger than in the current first implementation;
- remain yellow;
- remain left-justified;
- appear below the current-score region as shown by the prototype;
- advance horizontally from left to right;
- use centralized scale and spacing constants;
- retain the existing reserve count and 25-symbol display behavior;
- never alter the underlying lives count;
- remain bounded within the portrait HUD region.

Do not add wrapping, scrolling, multiple rows, numeric lives text, or automatic shrinking.

### High score

The fixed high score must remain:

38930

It must:

- use green vector digits;
- appear to the right of the current-score field;
- be smaller than the current score;
- use an explicit independent scale and anchor;
- retain stable spacing from the initials.

Do not connect it to mutable high-score state.

### Initials

The fixed initials must remain:

AAA

They must:

- use the existing ROM-informed uppercase-A geometry;
- use green vector strokes;
- appear immediately to the right of the high score;
- use a scale visually compatible with the high-score digits;
- use centralized spacing and anchor constants.

Do not implement additional letters, high-score entry, or persistence in this task.

### Current level

The live displayed level must:

- use blue vector digits;
- appear beneath the high-score/initials area as shown in the prototype;
- use an independent scale and anchor;
- remain visually associated with the high-score/initials group rather than the current-score field;
- update live as the displayed level changes;
- retain deterministic overflow behavior.

Do not alter level progression.

## Independent field scales and anchors

Do not force one common scale or one common baseline across all HUD fields.

Centralize explicit layout values for at least:

- current-score scale;
- current-score field right edge;
- current-score vertical anchor;
- reserve-Blaster scale;
- reserve-Blaster horizontal advance;
- reserve-Blaster starting position;
- high-score scale and anchor;
- initials scale and anchor or spacing relative to the high score;
- level scale and anchor.

Keep these values in the 600x800 logical portrait HUD coordinate system.

Do not mix portrait logical coordinates with host-window or playfield coordinates.

Make the constants easy to adjust following Neal's next manual test.

Do not create a generalized layout engine or automatic responsive layout system.

## Development diagnostic information

The current development diagnostic information includes text such as:

- FLIPPERS 11
- ACTIVE 5

This information is not part of the Atari HUD.

Move the applicable development diagnostic information completely outside the portrait viewport into the unused application-window area on the left.

Requirements:

- align the diagnostic block to the far-left edge of the application window, with only the minimum practical inset needed to avoid clipping;
- keep the diagnostic block outside the portrait viewport;
- do not place it between Atari HUD elements;
- do not use the portrait HUD camera for its final placement;
- keep it in fixed application/host coordinates;
- preserve its existing diagnostic content, colors, and live values unless a minimal readability adjustment is required;
- do not move Atari gameplay HUD information outside the viewport;
- do not move gameplay or tube presentation;
- do not create a new diagnostic system.

The current application window is wider than the centered portrait viewport. Use the existing left-side application area rather than changing window or viewport dimensions.

If multiple development diagnostic overlays currently share the same portrait-region placement path, move only the overlays necessary to remove development information from the portrait viewport while preserving their functionality.

## Tube and viewport protection

The accepted Level 1-10 tube presentation is frozen.

Do not change:

- per-level Atari-presentation calibration;
- tube Y gain;
- tube scale;
- tube vertical offset;
- portrait perspective;
- tube geometry;
- playfield camera;
- viewport dimensions;
- viewport centering;
- Approach;
- ExitZoom;
- entity registration;
- gameplay geometry.

The HUD refinement must not move, scale, clip, or otherwise alter the tube.

## Colors

Preserve the current target color categories:

- current score: green;
- high score: green;
- initials: green;
- current level: blue;
- reserve-life Blasters: yellow.

Do not launch a broad color-calibration effort.

If the prototype analysis shows that the existing RGB translations are materially inconsistent with the target, make only a small centralized translation adjustment and clearly report it.

Do not adjust colors simply because screenshots may vary in brightness.

## Vector-junction artifact

The optional brighter junction dots remain nonessential.

Do not add or tune the junction-artifact simulation in this refinement unless it already emerges naturally from the existing renderer.

Do not make it a manual-acceptance requirement.

## Scope exclusions

Do not change:

- score mechanics;
- lives mechanics;
- extra-life rules;
- scoring timing;
- level progression;
- high-score persistence;
- initials entry;
- leaderboard behavior;
- attract mode;
- recording schema;
- deterministic playback state;
- screenshot destination or naming;
- F9 pause;
- F10 screenshot capture;
- Game Over behavior;
- legacy presentation behavior.

Do not implement unrelated HUD labels.

Do not add a font-rendered fallback to Atari presentation.

Do not stage, commit, or push.

## Automated validation

Update or add focused tests proving at minimum:

1. Existing ROM-informed glyph geometry remains unchanged apart from presentation transforms.
2. Existing reserve-Blaster base geometry remains unchanged.
3. Reserve Blasters use the new larger presentation scale.
4. Reserve-life count and 25-symbol cap semantics remain unchanged.
5. Player.Lives remains unchanged by HUD rendering.
6. Current score remains live and right-justified within its explicit field.
7. Score growth proceeds leftward from a stable right edge.
8. Current-score field changes do not shift high score, initials, or level.
9. Fixed high score remains 38930.
10. Fixed initials remain AAA.
11. High score and initials use independent, smaller presentation scale than the current score where specified by the layout.
12. Current level remains live and blue.
13. Level uses its independent anchor beneath the high-score/initials group.
14. HUD vector submissions are deterministic and finite.
15. HUD content remains within the portrait HUD region.
16. Development diagnostic submission is outside the portrait viewport and aligned to the left application area.
17. Diagnostic data and colors remain functionally intact.
18. Legacy HUD remains unchanged.
19. Accepted tube calibration and playfield projection remain unchanged.
20. Rendering does not mutate gameplay/model state.
21. Visible playback remains deterministic as score and level change.
22. F9/F10 screenshot behavior remains valid.
23. Existing Approach, ExitZoom, viewport, calibration, guide, playback, and presentation regressions remain valid.

Do not attempt pixel-perfect image tests against the prototype screenshots.

The prototypes are manual composition references, not golden-image regression files.

Run bounded:

- focused Atari HUD/layout/diagnostic tests;
- relevant vector, portrait, viewport, calibration, playback, screenshot, Approach, and ExitZoom regressions;
- complete solution test suite;
- Release solution build;
- git diff --check.

Use established hang timeouts and security/stall procedures.

Do not weaken Windows security.

Do not enter same-artifact retry loops.

Do not launch the interactive game.

## Manual handoff

After automated validation, provide concise commands for Neal to inspect:

1. Level 2 Atari presentation with the normal HUD.
2. Level 2 with enough lives to assess the enlarged reserve-Blaster scale and spacing.
3. Level 10 to assess multi-digit level placement.
4. Visible playback to assess live score and level changes.
5. Any established diagnostic option needed to make the relocated Flipper/Active information visible.

Use only existing verified CLI options and repository playback-file patterns.

Do not invent command-line options or recording filenames.

Manual acceptance should focus on:

- reserve Blaster size;
- current-score scale and right-edge alignment;
- high-score scale and position;
- AAA scale and spacing;
- level position;
- overall HUD composition;
- colors;
- diagnostic information being fully outside the portrait viewport;
- no movement or regression of the accepted tube presentation.

The optional vector-junction artifact remains non-blocking.

## Records and repository protection

Save this substantial prompt and its implementation report under the tracked AI-Logs prompt/report convention.

Do not stage, commit, or push during this refinement.

Preserve unrelated tracked and untracked material, including any previously updated AI-Logs report.

Do not reset, restore, stash, clean, rebase, or alter unrelated files.

Stop after implementation, automated validation, and manual-test handoff.

TEMPEST_ATARI_HUD_PRESENTATION_REFINEMENT