**1. Source-Backed Findings**
Labels: **A** = explicit source rule; **B** = consequence of interacting code; **C** = not established as original arcade behavior.

- **Creation/progression [A]:** Incitti calls the Spiker `Spinner`. Eggs hatch into one on their current lane at height `0`. Spikers become eligible when `current_board > 2 || current_level > 47`: first at **displayed level 4**, not Level 1. See [enemy setup](../Reference/incitti/tempest.bmx#L4212).
- **Composition [A]:** Eligibility flags and randomized hatch selection are code-defined, not stored in levelsets. All seven levelsets contain 16 geometry records, each with 35 values: continuity, vertical center/offset, and 16 coordinate pairs. See [level loading](../Reference/incitti/tempest.bmx#L4334).
- **Selection [A]:** Hatch selection favors Spikers by mapping negative random results to type `0`. It excludes them when `enemiesleft + active Eggs < 4` or the current Spiker count is `> 3`; that permits a fourth Spiker. Reserve Eggs are not included in that expression. See [Egg selection](../Reference/incitti/tempest.bmx#L1982).
- **Speed [A/C]:** Spawn speed is written as `current_level / 48.0 + Rand(2.5,4.5)`. Verify BlitzMax `Rand` argument conversion before claiming a continuous fractional distribution.
- **Movement [A]:** An initial update changes state without movement. During building, height increases by `grow_speed` while its pre-update height is below `Depth - 4`. It remains in the same lane. See [Spinner implementation](../Reference/incitti/tempest.bmx#L3580).
- **Reversal [A]:** While extending an existing spike, accumulated growth increases by speed. Once growth is `> 100`, `Rnd(0,120) > 88` can initiate retreat. Reaching the upper threshold also initiates retreat. Retreat subtracts `speed × 0.90`; an update beginning at height `<= 0` removes the Spiker and returns **one pending Egg**, without awarding points.
- **Spike creation/growth [A]:** Spike-enabled boards initially receive a height-`60` spike on every lane. A Spiker reuses its lane's spike. When Spiker height exceeds spike height, it assigns spike height to `Spiker height - 4`. If none exists, it creates one and assigns its own height. Preserve these branches explicitly; this is not simply monotonic growth.
- **Persistence [A/B]:** Shooting or retreating a Spiker does not remove its spike. Death conversion explicitly keeps spikes and their current heights; level clearing removes them.
- **Shot rules [A]:** Same lane and strict `shot depth < object height` hits either object. There is **no Flipper-style 16-unit band**. A Spiker hit removes it and awards **50 points**. A spike hit consumes the shot, removes **10 height**, awards **1 point**, and removes the spike at height `<= 0`. See [Spikes](../Reference/incitti/tempest.bmx#L3495).
- **Collision priority [B]:** Shots stop at the first successful collision in shared-list insertion order, not nearest depth. Initial spikes precede hatched Spikers; replacement spikes can have different ordering. See [shots](../Reference/incitti/tempest.bmx#L1799).
- **Accounting [A/B]:** Spikers increment active enemy count; kills decrement it; retreats decrement it and replenish reserve. Spikes never increment enemy count. Residual spikes therefore do **not** prevent clearing.
- **Exit hazards [A]:** A spike kills when its lane equals the player lane and `spike height > player depth`; sub-lane position is not checked. Spikers themselves never collide lethally with the player. Spikes remain drawn and collision-active during zoom.
- **Avoidance [A/B]:** Movement and firing remain available while alive during avoidance/zoom. Shots originate at current player depth. Displayed levels **4–7** receive an avoidance period of **151, 111, 71, 31 updates**, respectively, before the existing 21-update zoom delay. Spike death starts reverse zoom, preserves spikes, and eventually retries the exit. See [level states](../Reference/incitti/tempest.bmx#L1344).
- **Ordering [A]:** Reference order is level-state update → shots → Eggs → enemy updates/collisions → player input. Spike collision therefore precedes that frame's player movement. Do not copy this globally over the validated C# ordering.
- **Rendering [A]:** Spiker: green spiral at lane midpoint, projected at `Position + Depth - height`, scale `TFormSZ(22,z) + 2`, rotation advances `45°` per draw. Spike: green line from far rim to its height, with a white tip. See [rendering](../Reference/incitti/tempest.bmx#L3495).
- **Out-of-scope source behavior [A]:** Random retreat can also fire an enemy bullet. Omit bullets deliberately for this milestone.

**2. Remaining Uncertainties**
**C:** Incitti establishes implementation behavior, not arcade authenticity. Compare initial spikes on every lane, retreat probabilities, regeneration, collision ordering, scoring, and exit controls against Atari gameplay. The readme explicitly records prior spike-growth/damage adjustments.

A milestone decision is also required: **how to expose Spikers without silently changing validated Level 1**. Recommend an explicitly selected development wave on existing geometry, not pretending Spikers belong in original Level 1.

**3. Revised Implementation Plan**
- Populate [Spiker.cs](../src/Tempest.Game/Gameplay/Spiker.cs) and [Spike.cs](../src/Tempest.Game/Gameplay/Spike.cs): explicit initialization/building/retreat states, persistent lane spike, verified damage and score rules.
- Extend [LevelDefinition.cs](../src/Tempest.Game/Levels/LevelDefinition.cs), [LevelLoader.cs](../src/Tempest.Game/Levels/LevelLoader.cs), and [Level.cs](../src/Tempest.Game/Levels/Level.cs): explicit wave eligibility, generalized hatching, retreat-to-reserve accounting, and spike initialization. Keep default Level 1 unchanged.
- Extend [TempestGame.cs](../src/Tempest.Game/TempestGame.cs): typed collections, ordered shot handling, mixed-enemy accounting, persistence/reset rules, and spike-specific exit handling. Keep existing Flipper code and order intact.
- Use existing `AvoidSpikes` and `ReverseZoom` states in [LevelState.cs](../src/Tempest.Game/Levels/LevelState.cs); extend [Player.cs](../src/Tempest.Game/Gameplay/Player.cs) only for reverse-travel depth. Adjust [Program.cs](../src/Tempest.Game/Program.cs) input gating only for spike-enabled exit states.
- Extend [VectorRenderer.cs](../src/Tempest.Game/Rendering/VectorRenderer.cs): spiral, persistent spikes/white tips, exit rendering. Preserve all existing colors and geometry.
- Extend [SimulationTests.cs](../tests/Tempest.Game.Tests/SimulationTests.cs); update development notes after validation. **No new files are inherently necessary.** No Flipper, input-system, projection, or LevelSet rewrite is needed.
- Use a small controllable decision source for Spiker randomness. Comment only non-obvious intent, source quirks, and ordering constraints.

**4. Proposed Automated Tests**
- Hatch eligibility, lane preservation, speed decisions, initial no-movement update, threshold boundaries, retreat and reserve return.
- Spike initialization, reuse/replacement, exact height assignments, persistence after creator removal and death.
- Strict shot boundaries, insertion-order conflicts, shot consumption, `50`/`1` scoring, ten-unit shortening and deletion.
- Accounting across mixed enemies, retreat, kills, death and replay; residual spikes do not block clear.
- Exit avoidance duration, permitted controls, shots from current depth, lane/depth collision boundaries, single life loss and reverse-zoom retry.
- Production spawn/update/render integration and finite projected geometry; retain all existing regression cases.

**5. Manual Arcade Comparison**
Verify spiral appearance/rotation, spike colors and white tips, travel/retreat pacing, spike growth and shot shortening, initial spike placement, exit movement/firing, collision timing, reverse-zoom presentation, and persistence after death. Recheck physical spinner feel and all established Flipper pivot behavior.

No files were edited or committed; the working tree remains clean.