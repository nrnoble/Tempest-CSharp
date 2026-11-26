# Tempest Level 17 Shape 1 Production Projection and Second-Cycle Palette — Bounded Implementation

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

The unique completion token for this task is the final standalone line below.

---

# Task

Implement the bounded Level 17 production-presentation correction and second-cycle visible palette.

This task has exactly two implementation objectives:

1. make Level 17 reuse the accepted Shape 1 production projection used by Level 1 rather than falling into the generic reciprocal projection branch;
2. implement an evidence-backed Levels 17–32 visible host palette using:
- verified Atari ROM palette data;
- ROM runtime-role evidence;
- existing accepted C# vector colors;
- the four explicitly authorized Level 17 MAME screenshots;
- Mark Incitti's local reference implementation only when needed to fill a remaining role-to-color translation gap.

**GAMEPLAY IS FROZEN.**

Do not modify Level 17 gameplay.

Do not modify Levels 1–16 gameplay.

Do not change:

- enemy supply;
- Flipper programs;
- Flipper rim behavior;
- Tanker cargo;
- Pulsars;
- Spikers;
- firing;
- shots' gameplay state;
- Spike gameplay;
- random behavior;
- ledger;
- reconstruction;
- completion;
- movement speed;
- level progression mechanics.

Do not implement Level 18+.

Do not stage, commit, or push.

Do not launch the interactive game.

Stop after automated validation and provide Neal a manual visual-acceptance handoff.

---

# Required prior reports

Read completely:

`AI-Logs/Reports/2026-11-25-production-presentation-default-promotion-closeout-report.md`

`AI-Logs/Reports/2026-11-25-level17-bounded-production-implementation-report.md`

`AI-Logs/Reports/2026-11-25-level17-shared-regression-bounded-correction-report.md`

`AI-Logs/Reports/2026-11-25-level17-frozen-rim-flipper-correction-report.md`

`AI-Logs/Reports/2026-11-25-level17-production-presentation-second-cycle-palette-investigation-report.md`

Also consult the verified ROM/palette evidence reports only as needed.

The final presentation/palette investigation is the specification for this milestone.

Do not repeat completed gameplay investigations.

---

# Gameplay freeze and accepted gameplay checkpoint

Treat the following as accepted current behavior:

## Levels 1–16

Neal reports the full recorded Levels 1–16 campaign now plays correctly again.

The automated accepted boundary is:

- score `134039`;
- lives `9`;
- seed `1990198549`;
- boundary `35350`;
- accepted Level 16 completion behavior before Level 17 continuation.

## Level 17

Neal reports:

- overall gameplay plays fine;
- Flippers climb correctly below the rim;
- Flippers now move/flip correctly after reaching the rim;
- Pulsars behave correctly;
- Tankers behave correctly;
- Blaster movement/shooting remains functional.

Latest automated gate:

- Level 17 diagnostic recording PASS;
- Levels 1–16 accepted campaign boundary PASS;
- Level 15 seed-1982 PASS;
- 68 focused tests PASS;
- full suite 2,060 passed / 0 failed;
- Release build 0 warnings / 0 errors;
- `git diff --check` PASS.

Any gameplay/state change in this presentation milestone is a blocker.

---

# Established presentation-routing defect

The investigation establishes:

- ordinary no-option Level 17 correctly selects production Atari presentation;
- Level 17 does not select explicit legacy 4:3;
- `HasModelFrameMapping(17)` is true;
- Level 17 reaches `ProjectAtariPresentation`;
- Level 17 uses canonical Shape 1 / ROM row 0;
- Level 17 receives the same model-frame mapping as Level 1;
- Level 17 receives the same accepted calibration values as Level 1;
- Level 17 nevertheless does **not** execute Level 1's accepted projection branch;
- Level 1 uses a dedicated accepted Shape 1 depth/projection branch;
- Level 17 falls into the generic reciprocal branch because projection routing remains displayed-level-specific.

This is the production presentation defect.

Do not add a special `displayedLevel == 17` copy of the Level 1 formula.

Correct the architectural keying so repeated Shape 1 geometry uses the Shape 1 accepted projection.

---

# Established Shape 1 calibration

The accepted Shape 1/Level 1 values are:

- Y gain: `1.4`
- tube scale: `0.8`
- vertical offset: `35`
- depth-Y gain: `1`

Level 17 already receives these values.

Do not retune them in this task.

Do not create Level 17-specific replacements.

Runtime `--tube-*` overrides must remain available.

---

# Established ROM palette evidence

## First-cycle raw block

Levels 1–16:

`00 04 08 0C C3 07 0B 0B`

## Second-cycle raw block

Levels 17–32:

`00 07 0B 08 44 03 0C 0C`

Level 17 selects palette/cycle index 1.

ROM `setcolours` supplies eight runtime indices and low/high nibble arrays.

Established role evidence includes:

- index 1: player;
- index 3: Flipper;
- index 4: normal Pulsar;
- index 0: white/pulse/explosion fallback behavior in established paths.

Do not invent roles for unproven indices.

---

# Authorized MAME screenshots

The following four MAME Level 17 screenshots are explicitly authorized visual evidence for this task:

`UserData/Screenshots/Level-17/MAME/MAME-0009.png`

`UserData/Screenshots/Level-17/MAME/MAME-0010.png`

`UserData/Screenshots/Level-17/MAME/MAME-0011.png`

`UserData/Screenshots/Level-17/MAME/MAME-0012.png`

If the exact filenames differ only by the user's externally supplied/uploaded names, use only the four explicitly identified Level 17 screenshots and do not search for additional images.

Do not enumerate other screenshot directories.

Do not open unrelated screenshots.

Do not inspect videos.

Use these screenshots only for:

- categorical vector color roles;
- distinguishing persistent base colors from transient states;
- HUD color observation;
- visual confirmation of cycle-2 palette translation.

Do not derive:

- tube geometry;
- projection constants;
- movement;
- timing;
- speed;
- collision;
- gameplay behavior;
- world coordinates.

---

# Manual color observations already supplied by Neal

Treat these as user-supplied visual evidence to reconcile, not as ROM facts.

Across the four Level 17 MAME screenshots, the visible palette is small.

Observed persistent/categorical colors include:

- black background;
- red;
- cyan;
- green;
- magenta;
- yellow;
- blue;
- white/transient details.

Observed roles include:

## Tube/web

Predominantly red.

## HUD

- score: cyan;
- lives symbols: green;
- level/wave number at upper right: red;
- other upper HUD text/digits visibly include cyan.

HUD **color** is part of the Level 17/cycle presentation.

HUD geometry/layout remains frozen.

## Flipper-like rim vectors

Magenta is prominently visible.

## Other gameplay vector colors

Green, yellow, cyan, blue, and white appear in gameplay roles/transients.

Do not assign exact semantic roles solely from appearance when ROM/current renderer evidence can identify them.

---

# Important correction regarding the compact blue diamond-like vector

Do **not** classify the compact blue diamond-like object visible in the screenshots as an enemy.

Neal reports it is likely a shot fired by the Blaster.

Treat its identity as:

**UNRESOLVED VISUAL ROLE, likely Blaster/player shot per Neal's observation.**

Before changing shot color, identify the corresponding current/ROM rendering path.

If source/rendering evidence confirms that this visual is the player's shot, record that.

If not, leave the role unresolved rather than labeling it an enemy.

Do not modify shot gameplay.

---

# Incitti fallback authority

The local Mark Incitti implementation may be used as **REFERENCE-DERIVED** evidence specifically for color-role translation.

Use Incitti only if:

- ROM establishes a runtime role/index;
- MAME demonstrates a categorical visible color;
- current C# mapping remains ambiguous.

Incitti may help answer questions such as:

- which entity uses which named/vector color;
- what visible color is assigned in the second cycle;
- how the reference implementation maps palette ranges.

Do not let Incitti override:

- verified ROM palette bytes;
- ROM role assignments;
- Neal's direct MAME observations;
- accepted C# projection geometry.

Do not copy unrelated Incitti gameplay behavior.

---

# Phase 1: Implement Shape-keyed production projection

Refactor the smallest necessary presentation-routing seam.

Required outcome:

- Level 1 Shape 1 uses the existing accepted Shape 1 projection;
- Level 17 Shape 1 uses the exact same Shape 1 production projection;
- shape identity, not displayed-level identity, selects the projection family where the behavior is genuinely shape-specific;
- displayed Level 17 remains 17 for HUD/gameplay/palette;
- geometry remains canonical row 0;
- calibration remains Shape 1 values;
- entity registration remains shared.

Do not:

- copy the existing Level 1 formula into a Level 17 branch;
- duplicate geometry;
- duplicate calibration;
- add a per-entity Level 17 offset;
- tune the generic reciprocal branch to imitate Shape 1;
- alter gameplay.

If extraction of a small `ProjectShape1` helper or equivalent is the cleanest approach, use it.

Keep accepted Level 1 numerical output unchanged.

---

# Phase 2: Projection regression tests

Add focused tests proving:

## Level 1 unchanged

Representative Shape 1 tube/entity inputs produce the same accepted output before/after refactor.

## Level 17 structural equivalence

Under identical Shape 1 world/model inputs, Level 17 uses the same Shape 1 projection family as Level 1.

## Display identity remains independent

HUD/gameplay displayed level remains 17.

## Entity registration

Representative:

- Blaster;
- Flipper;
- Tanker;
- Pulsar;
- shot;
- Spike

remain registered to the correct projected tube lanes.

## No legacy fallback

Level 17 normal/default presentation does not use:

- explicit 4:3 legacy;
- baseline portrait;
- unmapped fallback.

---

# Phase 3: Build a bounded cycle-palette resolver

Create the smallest production palette seam that keeps:

```
Shape identity
```

separate from:

```
palette/cycle identity
```

Requirements:

- palette 0 = Levels 1–16;
- palette 1 = Levels 17–32;
- Level 16 remains palette 0;
- Level 17 selects palette 1;
- geometry/projection does not change when palette changes;
- first-cycle accepted colors remain unchanged.

Do not implement future Levels 18–99 gameplay.

A palette resolver may understand cycle identity beyond Level 17 only where the already verified palette table structurally requires it, but do not expose unsupported gameplay levels.

---

# Phase 4: Map known persistent color roles

Use the combined evidence:

1. ROM role/index;
2. accepted existing C# host colors;
3. authorized MAME screenshots;
4. Incitti if required.

Prefer reusing existing accepted C# vector colors:

- red;
- cyan;
- green;
- magenta;
- yellow;
- blue;
- white;

rather than inventing slightly different RGB shades.

For each changed role document:

- ROM index/value;
- screenshot-observed categorical color;
- current C# role/color;
- Incitti reference if used;
- implemented C# host color;
- classification.

Implement only roles supported by sufficient evidence.

---

# Phase 5: Tube/web color

The authorized MAME references clearly show the Level 17 tube/web in red.

Identify:

- the ROM/runtime color role used for tube/web;
- current C# tube color path;
- cycle-palette seam.

Implement Level 17/cycle-1 tube/web as the established existing C# red where evidence supports that mapping.

Do not alter tube geometry or projection to achieve color.

Level 1–16 tube/web colors must remain accepted.

---

# Phase 6: HUD color scheme

HUD color changes are explicitly within scope.

HUD **geometry is frozen**.

Do not modify:

- HUD positions;
- scale;
- glyph shapes;
- score formatting;
- life placement;
- level-number placement.

Implement cycle-appropriate HUD role colors where supported.

The MAME references establish:

- score: cyan;
- lives symbols: green;
- Level 17 number: red;
- upper HUD text/digits include cyan.

Trace current HUD draw paths and introduce the smallest palette roles required.

Do not force the entire HUD to one color if the reference clearly separates roles.

Add tests for color only, not new geometry.

---

# Phase 7: Blaster/player color

ROM evidence directly identifies curcolor index 1 as player.

Trace:

- cycle-0 raw value;
- cycle-1 raw value;
- current accepted C# Blaster host color;
- MAME Level 17 visible Blaster color where observable;
- Incitti if needed.

If ROM nibble reuse and accepted color mapping support unchanged player color, preserve it.

Do not change Blaster geometry.

---

# Phase 8: Flipper color

ROM evidence directly identifies index 3 for Flippers.

Cycle-1 raw value differs from cycle 0.

Authorized MAME screenshots show prominent magenta rim-Flipper-like vectors.

Trace and implement an evidence-backed cycle-1 Flipper host color.

Use existing accepted C# magenta if that role reconciliation is supported.

Do not alter Flipper movement or glyph geometry.

---

# Phase 9: Pulsar color

ROM evidence identifies index 4 as normal Pulsar color.

Preserve pulse-white behavior through index 0 / established pulse state.

Determine:

- cycle-1 normal Pulsar categorical color;
- current cyan project translation;
- MAME evidence;
- Incitti if needed.

If evidence does not justify changing normal Pulsar from the currently accepted cyan, retain cyan and document the boundary.

Do not change pulse timing or hazard behavior.

---

# Phase 10: Player-shot / compact blue vector role

Investigate the compact blue diamond-like vector only enough to identify its rendering role.

Check:

- current Blaster/player-shot glyph;
- shot color;
- MAME geometry correspondence;
- ROM shot draw path;
- Incitti shot rendering if necessary.

If it is confirmed as the player shot, implement cycle-appropriate shot color only if supported.

If identity remains ambiguous, do not change it.

Record Neal's likely-shot observation.

Do not classify it as an enemy without evidence.

---

# Phase 11: Tanker, Spiker, enemy-shot and Spike colors

For each:

- trace current C# role;
- locate ROM role/index if available;
- compare MAME categorical color;
- use Incitti only for unresolved role/color mapping.

If evidence is sufficient, apply cycle-1 host color.

If evidence is insufficient, preserve existing C# color and mark it for Neal's manual review.

Do not delay the whole palette implementation for a minor role whose safe existing color can remain temporarily.

Do not alter gameplay.

---

# Phase 12: Transient colors

Preserve:

- Pulsar pulse-white;
- Super Zapper cycling;
- explosions/destruction;
- temporary flashes.

Ensure transient completion restores the selected base cycle palette rather than hard-coded cycle-0 colors.

Add regression tests where practical.

Do not redesign effects.

---

# Phase 13: Natural Level 16→17 palette activation

Implement cycle-1 activation at the established Level 17 entry boundary.

Preserve:

- Level 16 outgoing presentation under palette 0;
- existing ExitZoom;
- starfield;
- approach;
- gameplay transition timing.

At the point Level 17 becomes the entered/current production level, palette 1 should become active for applicable roles.

Do not change transition timing.

Add a focused natural-progression palette test.

---

# Phase 14: Gameplay-state freeze validation

Before broad presentation validation, prove projected/color changes do not mutate simulation.

Run focused fingerprints/state tests covering:

- Level 17 Flipper route/rim;
- Tanker cargo;
- Pulsars;
- firing;
- ledger;
- progression;
- completion.

No gameplay values should change because of color/projection routing.

---

# Phase 15: Recording regression

Run:

`TEMP/GameplayRecordings/lvl-17-test.json`

headlessly.

Verify the established Level 17 gameplay oracle remains valid.

Run:

`TEMP/GameplayRecordings/lvl1-16.json`

using the established accepted boundary oracle.

Do not modify either recording.

Do not commit them.

---

# Phase 16: Focused presentation tests

Run/add tests for:

- Shape 1 projection shared by Level 1 and Level 17;
- Level 1 numerical presentation unchanged;
- Level 17 no legacy fallback;
- cycle palette selection;
- tube color;
- HUD score/lives/level colors;
- Flipper color;
- known entity colors;
- Pulsar pulse-white;
- Super Zapper/transient restoration;
- entity/tube registration;
- finite projection.

Do not assert visually guessed roles as ROM-backed.

---

# Phase 17: Full validation

After fast gameplay/presentation gates pass:

1. relevant Level 1/17 presentation tests;
2. Levels 1–16 regression tests;
3. Level 17 gameplay tests;
4. full automated solution suite;
5. Release build;
6. `git diff --check`.

Use explicit bounds.

Require zero failed tests and zero build errors.

Do not launch the game.

---

# Manual visual-acceptance handoff

After automated validation, STOP for Neal.

Provide exact commands for:

## Level 1 comparison

```
dotnet run --project src\Tempest.Game -- --start-level 1
```

## Level 17

```
dotnet run --project src\Tempest.Game -- --start-level 17 --lives 10
```

## Optional background overlay

Provide the equivalent Level 17 command with:

```
--background-opacity .5
```

for comparison against MAME screenshots.

Neal should judge:

### Projection

- Level 17 Shape 1 placement/perspective matches the accepted Level 1 production presentation family;
- the tube no longer looks like the old/legacy projection.

### Palette

- red tube/web;
- cyan score;
- green lives;
- red Level 17 number;
- entity colors;
- Flipper magenta;
- Pulsar color/pulse;
- shots;
- transient effects.

### Gameplay

Only confirm presentation changes did not visibly regress gameplay.

Do not ask Neal to recalibrate movement/timing.

---

# Documentation boundary

This remains a candidate until Neal accepts it.

Do not encode new manual acceptance in durable docs before Neal responds.

Do not close Level 17.

Do not stage/commit/push.

---

# Required AI artifacts

Save this prompt as:

`AI-Logs/Prompts/2026-11-25-level17-shape1-projection-second-cycle-palette-implementation-prompt.md`

Create:

`AI-Logs/Reports/2026-11-25-level17-shape1-projection-second-cycle-palette-implementation-report.md`

These are GitHub continuity records but this task does not authorize commit/push.

---

# Required report contents

1. Executive summary
2. Starting Git/worktree state
3. Gameplay-freeze status
4. Shape 1 routing correction
5. Level 1 regression protection
6. Level 17 projection result
7. Model-frame/calibration reuse
8. Palette resolver architecture
9. Raw cycle blocks
10. Role-evidence matrix
11. MAME evidence used
12. Incitti evidence used
13. Tube/web color mapping
14. HUD color mappings
15. Player/Blaster color
16. Flipper color
17. Pulsar color/pulse
18. Player-shot/blue-vector identification result
19. Tanker color
20. Spiker color
21. enemy-shot color
22. Spike color
23. transient-color behavior
24. Level 16→17 palette activation
25. gameplay-state freeze validation
26. Level 17 recording result
27. Levels 1–16 recording result
28. focused presentation tests
29. full-suite result
30. Release-build result
31. `git diff --check`
32. unresolved color roles
33. project translations versus ROM facts
34. manual visual-acceptance matrix
35. files changed
36. final Git/worktree state
37. stage/commit/push: none
38. Level 18+ untouched
39. recommended next step

If a role remains unresolved, state that rather than inventing a mapping.

---

# Stop conditions

STOP rather than guessing if:

- Shape 1 routing correction changes accepted Level 1 output;
- projection correction requires gameplay changes;
- palette resolver alters geometry;
- required major color roles cannot be supported by ROM + MAME + accepted C# + Incitti evidence;
- a proposed host palette would be arbitrary;
- gameplay recordings regress;
- Flipper/Pulsar/Tanker behavior changes;
- prior levels regress;
- validation stalls or is security-blocked.

Minor unresolved colors may remain existing project translations if clearly documented and they do not prevent Neal from evaluating the dominant Level 17 scheme.

---

# Completion criteria

Complete only when:

- Level 17 uses the accepted Shape 1 production projection;
- Level 1 accepted projection remains unchanged;
- Level 17 no longer falls into the generic Shape 1-inappropriate branch;
- Level 17 selects cycle palette 1;
- cycle palette is independent of shape;
- dominant visible Level 17 palette is implemented from evidence;
- HUD color scheme is cycle-aware without layout changes;
- compact blue vector is not falsely classified as an enemy;
- gameplay remains frozen and regression-safe;
- Level 17 recording passes;
- Levels 1–16 recording passes;
- focused presentation tests pass;
- full suite passes;
- Release build succeeds with zero errors;
- `git diff --check` passes;
- nothing is staged, committed or pushed;
- Level 18+ remains untouched.

Then STOP for Neal's visual acceptance.

TEMPEST-END-LEVEL17-SHAPE1-PROJECTION-SECOND-CYCLE-PALETTE-COMPLETE