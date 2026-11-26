# Tempest Level 17 Tube-Rail, Tanker, and Egg Palette Refinement

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

The unique completion token is the final standalone line below.

---

# Task

Perform a narrowly bounded **Level 17 tube-rail, Tanker, and Egg palette refinement**.

The Shape 1 production projection correction has passed Neal's manual visual test.

The dominant second-cycle palette is much closer to MAME, but three discrepancies remain:

1. the ordinary Level 17 tube/web is not consistently red because many radial/depth rails are blue and some are yellow;
2. Level 17 Tankers are currently magenta, while Neal's direct MAME evidence establishes that Level 17 Tankers are blue;
3. Level 17 Eggs are currently red, while Neal's direct MAME evidence establishes that Level 17 Eggs are yellow.

Investigate the rail-color discrepancy, distinguish legitimate transient rail overrides from incorrect base-color inheritance, and implement the smallest evidence-backed corrections.

Correct:

- Level 17/cycle-1 Tankers to blue;
- Level 17/cycle-1 Eggs to yellow.

**GAMEPLAY IS FROZEN.
SHAPE 1 PROJECTION IS FROZEN.
HUD GEOMETRY AND COLORS ARE FROZEN.**

Do not modify:

- tube geometry;
- Shape 1 projection;
- calibration;
- model-frame mapping;
- HUD positions, scale, glyphs, or accepted colors;
- gameplay state;
- enemy supply;
- movement;
- Flipper behavior;
- Tanker behavior or cargo;
- Egg lifecycle/admission/hatching;
- Pulsar behavior;
- shots' gameplay state;
- Spike gameplay;
- random behavior;
- ledger;
- reconstruction;
- completion;
- progression;
- Level 18+.

Do not stage, commit, or push.

Do not launch the interactive game.

Stop after correction, automated validation, and manual-validation handoff.

---

# Required prior reports

Read completely:

`AI-Logs/Reports/2026-11-25-level17-shape1-projection-second-cycle-palette-implementation-report.md`

`AI-Logs/Reports/2026-11-25-level17-production-presentation-second-cycle-palette-investigation-report.md`

`AI-Logs/Reports/2026-11-25-level17-frozen-rim-flipper-correction-report.md`

`AI-Logs/Reports/2026-11-25-level17-shared-regression-bounded-correction-report.md`

Use the latest Shape 1/palette implementation report as the current implementation authority.

Do not repeat the broad presentation or gameplay investigations.

---

# Accepted current checkpoint

## Projection accepted

Neal manually reports:

- Level 17 Shape 1 projection is now fixed.

Treat this as manual acceptance of the Shape 1 projection correction.

Do not retune or refactor it in this task.

The accepted Shape 1 values remain:

- tube Y gain `1.4`;
- tube scale `0.8`;
- vertical offset `35`;
- depth-Y gain `1`.

## Gameplay accepted

Neal manually reports:

- Levels 1–16 recording plays correctly;
- Level 17 Flippers climb and move around the rim correctly;
- Pulsars behave correctly;
- Tankers behave correctly;
- Level 17 gameplay plays fine.

Do not alter gameplay.

## Current automated baseline

The current candidate reports:

- Level 17 recording PASS;
- Levels 1–16 accepted boundary PASS;
- full suite 2,062 passed;
- Release build 0 warnings and 0 errors;
- `git diff --check` PASS.

Preserve that baseline.

---

# Verified palette context

## First-cycle ROM palette

`00 04 08 0C C3 07 0B 0B`

## Second-cycle ROM palette

`00 07 0B 08 44 03 0C 0C`

Level 17 selects palette/cycle index 1.

The current candidate already implements:

- tube/web base role: red;
- score/high-score/initials: cyan;
- lives: green;
- Level 17 number: red;
- Blaster/player: yellow;
- Flipper: magenta;
- Pulsar normal: cyan;
- Pulsar pulse: white.

Do not alter those accepted/current mappings except where the rail investigation proves an existing rail path is bypassing the red tube/web base role.

---

# Authorized visual evidence

Use only the explicitly supplied Level 17 visual references.

## MAME references

`UserData/Screenshots/Level-17/MAME/MAME-0009.png`

`UserData/Screenshots/Level-17/MAME/MAME-0010.png`

`UserData/Screenshots/Level-17/MAME/MAME-0011.png`

`UserData/Screenshots/Level-17/MAME/MAME-0012.png`

## Current C# reference

Use the specifically supplied current Level 17 screenshot:

`live-level-17-tick-000194-001.png`

If the current C# screenshot is attached through agent-session context rather than stored at that name in the repository, use only the explicit attached image.

## Tanker crop

Use the explicitly supplied cropped MAME Tanker reference if it is available through the agent-session attachments.

Neal has confirmed that the cropped blue vector object is the Tanker.

If the crop is unavailable, the user-confirmed Tanker color remains authoritative manual/MAME evidence.

Do not search screenshot directories.

Do not inspect unrelated screenshots or videos.

Use the images only for categorical color comparison.

Do not derive geometry, movement, timing, coordinates, or projection constants from them.

---

# Manual visual evidence

Record the following as direct manual/visual evidence.

## Projection

- Level 17 projection is fixed and manually accepted.

## Tube/web

- MAME Level 17 shows the ordinary tube/web predominantly red.
- The current C# Level 17 screenshot has a largely red perimeter but many radial/depth rails are blue.
- Some rails in the current C# screenshot are yellow.
- The mixed blue/yellow ordinary web does not match the dominant red MAME tube/web presentation.
- Some non-red rails could be legitimate transient overrides, so do not simply force every rail red without tracing the selection logic.

## Tanker

Neal supplied a close crop of the actual MAME Tanker and confirmed:

> Level 17 Tankers are blue.

The current candidate retains Tankers as magenta.

Therefore:

```
Level 17 / Levels 17–32 Tanker
Current C# color: magenta
Correct MAME-observed categorical color: blue
Required correction: blue
```

Use the existing accepted C# vector blue.

Do not invent a new RGB shade.

## Egg

Neal reports:

> Level 17 Eggs should be yellow.

The current C# candidate renders Level 17 Eggs red.

Therefore:

```
Level 17 / Levels 17–32 Egg
Current C# color: red
Correct MAME-observed categorical color: yellow
Required correction: yellow
```

Use the existing accepted C# vector yellow.

Do not invent a new RGB shade.

## Other colors

Keep frozen unless direct code tracing requires them for rail-selection analysis:

- Flippers: magenta;
- Pulsars: cyan with white pulse;
- score: cyan;
- lives: green;
- level number: red;
- Blaster/player: yellow;
- background: black.

---

# Phase 1: Trace every tube-rail color path

Inspect the renderer paths responsible for:

- outer perimeter/rim edges;
- radial/depth rails;
- player lane;
- occupied lanes;
- Pulsar lanes;
- Super Zapper rail cycling;
- active hazards;
- transient flashes;
- approach/ExitZoom rail colors;
- normal base tube/web color.

Trace the exact priority/order used to select a rail color.

Determine why, in the supplied C# Level 17 screenshot:

- the outer perimeter is mostly red;
- many radial rails are blue;
- some radial rails are yellow.

For each non-red rail category, classify it as:

- legitimate player-lane override;
- legitimate enemy/Pulsar occupancy override;
- legitimate Super Zapper/transient override;
- legitimate hazard/effect override;
- incorrect palette-0/base-color fallback;
- incorrect hard-coded host color;
- unresolved.

Do not classify by appearance alone.

Trace each code path.

---

# Phase 2: Compare current rail logic against the MAME references

Use the four MAME Level 17 screenshots to determine which non-red rail colors visibly occur in normal gameplay.

Answer:

1. Are ordinary unoccupied Level 17 rails consistently red?
2. Does the player lane receive a different rail color?
3. Do Pulsar lanes receive a temporary or persistent color override?
4. Do Flippers, Tankers, Spikers, Eggs, or shots recolor their lane rails?
5. Are yellow or blue rails visible only as transient effects?
6. Does Super Zapper explain any supplied screenshot state?
7. Does the current C# screenshot show non-red rails in circumstances where MAME retains red?

Do not infer a code rule from one screenshot if source/rendering evidence contradicts it.

Use Incitti only if needed to identify a specific rail-color priority or role.

Classify Incitti evidence as REFERENCE-DERIVED.

---

# Phase 3: Preserve legitimate transient behavior

Do not eliminate valid transient effects merely to make a static screenshot red.

Preserve where supported:

- Super Zapper rail cycling;
- player-lane indication;
- Pulsar pulse/electrification;
- temporary flashes;
- destruction effects.

The correction should ensure that:

- the Level 17 base/unoccupied tube web is red;
- a temporary override applies only while its condition is active;
- after the condition ends, the rail restores to the Level 17 cycle-1 red base color;
- no transient restores to cycle-0 blue by mistake.

If a current transient path restores a hard-coded blue/base color, change it to restore through the selected cycle palette.

Do not alter the transient's gameplay timing or state.

---

# Phase 4: Correct Level 17 Tanker color

Trace the current Tanker drawing path and color resolver.

Implement:

- palette 0 / Levels 1–16 Tanker color unchanged;
- palette 1 / Levels 17–32 Tanker host color: blue.

Evidence classification:

- Tanker role identification: current renderer/source;
- categorical blue: Neal-confirmed direct MAME visual evidence;
- host RGB: existing accepted C# vector blue, PROJECT TRANSLATION.

Do not change:

- Tanker glyph geometry;
- movement;
- cargo;
- firing;
- identity;
- destruction;
- reconstruction.

Add a focused palette test proving:

- Level 16 Tanker color remains unchanged;
- Level 17 Tanker resolves blue;
- palette selection, not displayed-level special casing, controls the color.

Do not add `if level == 17` when palette index 1 is the correct abstraction.

---

# Phase 5: Correct Level 17 Egg color

Trace the current Egg drawing path and color resolver.

Determine whether Egg color currently comes from:

- a fixed global color;
- current/original species;
- Flipper color;
- a palette role;
- a hard-coded first-cycle color;
- another rendering resolver.

Determine whether the same Egg rendering path applies to:

- ordinary original Eggs;
- reconstructed Eggs;
- cargo-reentry Eggs;
- Pulsar Eggs;
- other species before hatching.

Implement through the cycle-palette/render-role layer where appropriate:

- palette 0 / Levels 1–16 Egg color unchanged;
- palette 1 / Levels 17–32 Egg host color: yellow.

Evidence classification:

- Egg categorical yellow: Neal-confirmed direct MAME/manual evidence;
- host RGB: existing accepted C# vector yellow, PROJECT TRANSLATION.

Do not modify:

- Egg geometry;
- species identity;
- hatch timing;
- admission;
- reserve state;
- reconstruction;
- ledger;
- gameplay.

Add focused tests proving:

- palette-0 Egg remains unchanged;
- palette-1 Egg resolves yellow;
- ordinary Level 17 Eggs resolve yellow;
- reconstructed/cargo-reentry Level 17 Eggs use the same selected cycle palette;
- changing Egg color does not alter simulation state.

Do not add a displayed-Level-17 special case when palette index 1 is the correct abstraction.

---

# Phase 6: Keep palette independent from geometry and gameplay

The final design must remain:

```
Shape 1 projection
  geometry
  model frame
  calibration

Cycle palette 1
  base tube/web red
  Tanker blue
  Egg yellow
  HUD/entity role colors
```

Do not put color decisions into:

- tube geometry;
- Shape 1 projection helper;
- gameplay configuration;
- Tanker gameplay state;
- Egg lifecycle/species state.

Use the rendering palette resolver or another bounded rendering-layer role resolver.

---

# Phase 7: Add rail-selection tests

Add tests that establish:

## Base web

- Level 16/cycle 0 retains accepted base rail color.
- Level 17/cycle 1 ordinary unoccupied rail resolves red.
- Outer and radial ordinary rails share the correct cycle-1 base role unless a legitimate override is active.

## Override priority

Where current design supports it, test:

- player-lane override;
- Pulsar-lane override;
- Super Zapper/transient override;
- restoration to selected cycle base color.

Do not test arbitrary screenshot coordinates.

Test the actual resolver/state inputs.

## No cross-cycle leakage

- Level 17 transient completion restores red, not blue.
- Level 1 remains unchanged.

---

# Phase 8: Confirm compact blue object/Tanker distinction

The compact blue diamond-like object previously discussed as an unresolved likely player shot must remain unresolved unless source evidence identifies it.

Do not use the Tanker's confirmed blue color to reclassify every blue diamond-like object as a Tanker.

The Tanker identification for this task comes from Neal's specifically cropped and confirmed Tanker reference.

Document the distinction:

- confirmed Tanker: blue;
- compact blue diamond-like vector elsewhere: identity remains unresolved unless source trace confirms it.

Do not change player-shot color without evidence.

---

# Phase 9: Gameplay-freeze validation

Before broad tests, demonstrate no gameplay state changed.

Run existing focused tests covering:

- Level 17 Flipper `$07`;
- rim handoff;
- Tanker cargo;
- Tanker behavior;
- Egg lifecycle/hatching;
- Pulsars;
- firing;
- ledger;
- progression;
- completion.

No simulation fingerprint or recording result should change due to palette refinement.

---

# Phase 10: Recording regression

Run headlessly:

`TEMP/GameplayRecordings/lvl-17-test.json`

Verify the established Level 17 result remains valid.

Run:

`TEMP/GameplayRecordings/lvl1-16.json`

Verify the accepted campaign boundary remains valid.

Do not modify or commit either recording.

---

# Phase 11: Focused presentation validation

Run/add focused tests for:

- Shape 1 projection unchanged;
- Level 17 cycle palette selection;
- base rail red;
- legitimate override priority;
- transient restoration to cycle-1 base;
- Tanker blue on cycle 1;
- Tanker unchanged on cycle 0;
- Egg yellow on cycle 1;
- Egg unchanged on cycle 0;
- ordinary and reconstructed Level 17 Eggs use palette 1;
- HUD colors unchanged;
- Flipper magenta unchanged;
- Pulsar cyan/white unchanged;
- finite projection;
- no gameplay-state changes.

---

# Phase 12: Full validation

After fast gates pass:

1. Level 17 presentation/palette tests;
2. Level 17 gameplay tests;
3. Levels 1–16 regression tests;
4. full solution suite;
5. Release build;
6. `git diff --check`.

Use explicit time/hang bounds.

Require:

- no failed tests;
- zero build errors;
- warnings reported accurately;
- no whitespace errors.

Do not launch the game.

---

# Manual-validation handoff

After automated validation, STOP for Neal.

Provide:

```
dotnet run --project src\Tempest.Game -- --start-level 17 --lives 10
```

and:

```
dotnet run --project src\Tempest.Game -- --start-level 17 --lives 10 --background-opacity .5
```

Neal should compare against the four MAME references and verify:

## Frozen accepted items

- Shape 1 projection remains fixed;
- HUD remains cyan/green/red as accepted candidate;
- Flippers remain magenta;
- Pulsars remain cyan/white;
- gameplay remains correct.

## Refined items

- ordinary tube/web rails are predominantly red;
- any blue/yellow rails correspond only to legitimate active/transient states;
- rail colors restore to red after transient conditions;
- Tankers are blue;
- Eggs are yellow.

Do not ask Neal to accept unresolved shot colors in this milestone.

---

# Required AI artifacts

Save this prompt as:

`AI-Logs/Prompts/2026-11-25-level17-rail-tanker-egg-palette-refinement-prompt.md`

Create:

`AI-Logs/Reports/2026-11-25-level17-rail-tanker-egg-palette-refinement-report.md`

These are GitHub continuity records, but this task does not authorize staging, commit, or push.

---

# Required report contents

1. Executive summary
2. Starting Git/worktree state
3. Gameplay/projection/HUD freeze confirmation
4. Manual rail-color evidence
5. Manual Tanker-blue evidence
6. Manual Egg-yellow evidence
7. Tube-rail color-path inventory
8. Rail-color priority order
9. Cause of ordinary blue rails
10. Cause of ordinary yellow rails
11. Legitimate transient rail overrides
12. Incorrect base/fallback paths
13. Rail correction
14. Cycle-base restoration behavior
15. Tanker renderer trace
16. Tanker color correction
17. Egg renderer trace
18. Egg color correction
19. Ordinary/reconstructed Egg behavior
20. Compact blue object/Tanker distinction
21. Files changed
22. Tests added/changed
23. Shape 1 projection regression result
24. HUD/Flipper/Pulsar regression result
25. Level 17 gameplay-freeze validation
26. Level 17 recording result
27. Levels 1–16 recording result
28. Focused test result
29. Full-suite result
30. Release-build result
31. `git diff --check`
32. Remaining unresolved color roles
33. Manual-validation matrix
34. Final Git/worktree state
35. Stage/commit/push: none
36. Level 18+ untouched
37. Recommended next step

If the investigation shows a non-red rail is legitimate, document the exact state that justifies it rather than forcing it red.

---

# Stop conditions

STOP rather than guessing if:

- rail color cannot be separated from gameplay state safely;
- changing rail restoration would alter Super Zapper or Pulsar behavior;
- Tanker identification conflicts with source rendering evidence;
- Egg color cannot be changed through rendering without affecting species/lifecycle;
- projection changes would be required;
- HUD changes would be required;
- gameplay recordings regress;
- accepted prior-level palette behavior changes unexpectedly;
- commands stall or security blocks validation.

Report PARTIAL with precise evidence.

---

# Completion criteria

Complete only when:

- the reason for blue/yellow Level 17 rails is established;
- ordinary Level 17 base rails resolve red;
- legitimate overrides remain bounded and restore correctly;
- Level 17 Tankers resolve blue;
- Level 17 Eggs resolve yellow;
- Levels 1–16 Tanker and Egg colors remain unchanged;
- Shape 1 projection remains unchanged;
- HUD colors remain unchanged;
- gameplay remains unchanged;
- both recordings pass;
- focused tests pass;
- full suite passes;
- Release build succeeds with zero errors;
- `git diff --check` passes;
- nothing is staged, committed, or pushed;
- Level 18+ remains untouched.

Then STOP for Neal's manual visual acceptance.

TEMPEST-END-LEVEL17-RAIL-TANKER-EGG-PALETTE-REFINEMENT-COMPLETE