# Level 1 Stationary Rim-Combat Deterministic Comparison

## 1. Starting repository state

The required Git checks were first attempted from the workspace parent, which is not a Git repository, then rerun from `Tempest`, the repository root:

- Branch: `master`.
- `HEAD`: `3bebf42cbe9514846ab32bf4c8a12bcfdb048da7` (`Implement accepted production Level 7`).
- `git status --short --branch`: `## master`.
- Tracked worktree and index were clean; both diff stats were empty.
- No `comment for AI.md` appeared in the starting status. No such file was created or changed.

## 2. Source boundary and investigation method

The only Atari source consulted was the project-local [master ROM listing](../../Reference/arcade/Tempest%20Rom%20Source%20code.md). No Internet search, other disassembly, MAME, port, or other Tempest source was used. The prior [Blaster/fire/rim/difficulty investigation](./2026-11-04-blaster-fire-rim-difficulty-rom-investigation-report.md) was the foundation; the C# rim-turn arithmetic was independently exercised here and corrects one estimate in that report.

Test scenarios use real C# `Level`, `Flipper`, and `Shot` objects, geometry, `Flipper.Update`, `Shot.Update`, and `Flipper.IsHitBy`. A test-only scheduler mirrors the inspected C# order, cooldown, capacity, and hit retirement. Small ROM-model cases encode only source-established shot ordering, capacity, and lifetime; they are not an emulator, and their invocation counts are not converted to C# ticks or seconds. No production seam or gameplay code was added.

## 3. Neal's arcade observation

**ARCADE OBSERVED:** Neal reports that camping in one rim lane is not sustainable on his original cabinet, even with held Fire or careful single taps. Multiple pursuing Flippers and a moment with no available shot can force movement or result in death. The arcade timing and actual cabinet difficulty setting have not been independently measured.

## 4. Deterministic cases and scope

The focused test class covers:

1. A pre-existing projectile on the last pivot update, with C# and ROM event ordering.
2. The ROM new-shot birth-position exception.
3. Source- and destination-lane hit checks at the start, middle, and final update of a C# pivot.
4. Held-fire no-hit and controlled ordinary-hit shot streams, with isolated C# capacity/lifetime ablations.
5. Stationary held-fire pressure with one, two, and four Flippers already just below the rim.
6. Four pursuers under a periodic single-tap pattern.
7. C# Level 1 rim pause and measured directed pivot duration.

Pressure cases fix the player in lane 0 and use deterministic Flipper positions in lanes 2, 4, 6, and 8. They do not model arcade random generation or a full wave. The ROM source does not supply enough decoded angle data for a matching absolute-timing simulation.

## 5. Final-pivot existing-shot race

The test places a pre-existing shot in the Blaster's lane, vertically within the C# hit window, on the last update of a pivot into that lane.

- **Current C#:** shot movement/collision runs before Flipper movement/contact; the shot hits and destroys the Flipper before lethal contact is consumed.
- **ROM source order:** an already-moving shot's hit scan is after enemy movement/contact. The Flipper completes its lethal pivot first; a later matching shot cannot undo the contact.
- The separate ROM birth-position hit is not applicable to this already-existing projectile.

This is a demonstrated, concrete C# safety advantage on the exact final-pivot race. It establishes a material contributor to the discrepancy, but does not show that this race alone explains all stationary survival.

## 6. New-shot birth-position exception

The ROM model separately checks a newly allocated shot at its birth coordinates before enemy movement. A matching same-segment target within the strict hit tolerance is hit before contact. The test confirms that this can prevent the subsequent contact; it does **not** generalize the post-movement ordering to new shots.

The distinction is important for any later correction: preserve the early birth-position opportunity while changing only the timing of an already-moving shot's later hit pass.

## 7. Pivot lane shootability

At three sampled points during the C# pivot—start, midpoint, and immediately before completion—`IsHitBy` accepted both `SourceEdge` and `DestinationEdge`: **6/6 lane/state checks**.

The master ROM listing establishes a hit-tolerance test and different segment/state predicates for an in-progress Flipper; the stored shot-time state (`$02c0`) is part of that path. It definitely rejects shots outside tolerance and cases failing the applicable segment/state comparison. It does **not** establish a blanket visual “source or destination lane is always hittable” rule. The mapping from the ROM's enemy segment and in-progress shot-time state to the two visible C# pivot lanes remains **UNKNOWN**. Therefore C#'s broad rule is proven, but which of these six C# hits exceed ROM shootability cannot be asserted.

## 8. Shot capacity, lifetime, and ordinary-hit recycling

### Held, no-hit stream

Each model was run for 100 of its own invocations; these clocks are deliberately not treated as equivalent.

| Model | Accepted | Capacity unavailable | Cooldown unavailable | Maximum active | Lifetime | First opportunity after exhaustion |
|---|---:|---:|---:|---:|---:|---:|
| C# (10 slots) | 20 | 40 | 40 | 10 | 67 C# updates | tick 68 |
| ROM model (8 slots) | 32 | 68 | none | 8 | 25 ROM movement passes | pass 26 |

### Controlled ordinary-hit stream

| Model | Accepted | Capacity unavailable | Cooldown unavailable | Maximum active | Hit retirement |
|---|---:|---:|---:|---:|---|
| C# (10 slots) | 34 | 0 | 66 | 1 transient | Yes; hit shot is removed |
| ROM model (8 slots) | 32 | 68 | none | 8 | No; ordinary hits do not clear slots |

The hit stream deliberately marks each ROM slot as hit while retaining it through normal retirement. This makes the opposite recycling behavior explicit. Counts remain in their model's local invocation units and are not a rate comparison.

### Isolated C#-clock ablations

| C# test model, 100 ticks | Accepted | Capacity unavailable | Maximum active |
|---|---:|---:|---:|
| 10 slots, 67-update lifetime | 20 | 40 | 10 |
| 8 slots, 67-update lifetime | 16 | 52 | 8 |
| 10 slots, forced 25-update lifetime | 34 | 0 | 8 |

Shortening lifetime in the same C# clock frees slots sooner; it is not a conversion of the ROM's 25 passes. A 10-versus-8-slot pressure ablation in the same four-Flipper scene also produced the same player death tick and the same two Flipper kills: at 10 slots, death was tick 265 with 42 accepted shots and 139 capacity-unavailable attempts; at 8 slots, death was tick 265 with 34 accepted and 163 capacity-unavailable attempts. The extra slots increased firing availability but did not change that scenario's survival outcome.

## 9. Stationary held-fire results

The C# scheduler and actual `Shot`/`Flipper` update methods were run with no player movement:

| Pursuers | Ticks | Attempts / accepted | Cooldown / capacity unavailable | Max shots | Rim arrivals | Pivots started / completed | Pivot kills | Killed on would-be lethal step | Remaining | Deaths | Lane/contact events |
|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|
| 1 | 600 | 600 / 91 | 182 / 327 | 10 | 1 | 2 / 1 | 1 | 0 | 0 | 0 | 1 |
| 2 | 600 | 600 / 92 | 184 / 324 | 10 | 2 | 6 / 4 | 2 | 0 | 0 | 0 | 4 |
| 4 | 265 (death) | 265 / 42 | 84 / 139 | 10 | 4 | 18 / 15 | 2 | 0 | 2 | 1 | 16 |

With one or two fixed pursuers, held fire destroyed all of them during the 600-tick observation without a death. With four simultaneously approaching pursuers, two were destroyed during pivots, the player died at tick 265, and two remained. The measured result is conditional: temporary stationary defense is strong, while this crowded setup creates genuine pressure. It is not a complete wave-level or ROM-versus-C# survival probability.

## 10. Controlled single-tap results

The four-pursuer case fired one tick every 40 simulation ticks, with the player stationary:

- Death at tick **265**; 7 attempts, 7 accepted.
- Cooldown/capacity-unavailable attempts: **0 / 0**; maximum concurrent shots: **2**.
- Four rim arrivals; 18 pivots started, 15 completed; two Flippers destroyed during pivots; none on the would-be lethal step.
- Two Flippers remained; one player death; 16 lane/contact events.

This chosen tap schedule did not sustain the crowded stationary defense. It is not claimed to represent an optimally skilled or arcade-calibrated tap schedule.

## 11. Multiple pursuers

One and two pursuers can be cleared by held fire in this controlled C# scene; four simultaneous pursuers cause death under both held fire and the selected tapping pattern. The extra pursuers create pressure rather than a single isolated duel. This supports Neal's qualitative description of accumulated rim pressure, but the manually fixed starting positions are a deterministic comparison fixture, not a reconstruction of ROM randomness.

## 12. C# and ROM rim-pursuit timing

The actual Level 1 C# geometry yielded a directed pivot sweep of **156.22269 degrees**, requiring **20 updates** at 8 degrees per update, plus the configured **24-tick rim pause**. That is 44 C# simulation updates from the start of a pause through an adjacent-lane pivot completion.

This corrects the prior investigation report's estimate of a roughly 22.5-degree / 3-update Level 1 pivot. The earlier estimate confused lane-heading change with the directed pivot sweep used by `CalculateDirectedLaneTransition`; no prior report or authority document was edited here.

The ROM source establishes the shared top-rim sequence: check/contact, four-count processing, pivot start/continue, and Level 1's top factor of 2. It also establishes direction choice toward the player with a branch involving another rim enemy and persistence of the selected direction. The decoded master listing lacks the angle data needed to calculate absolute Level 1 lane-to-lane duration. No seconds conversion or replacement pause value is inferred.

## 13. Difficulty

`00` decodes to Medium; the cabinet's physical setting remains unverified. The ROM difficulty branch changes Flipper threat/supply parameters, but does not change player-shot cadence, capacity, speed, or lifetime. The deterministic shot-order and slot tests do not depend on a difficulty switch. The C# pressure fixtures use the existing Level 1 configuration; any ROM Medium path is a target assumption pending cabinet confirmation, not a claim about Neal's hardware.

## 14. Candidate classification

| Candidate | Classification | Evidence and limitation |
|---|---|---|
| Shot/contact ordering | **Demonstrated material contributor** | Existing-shot final-pivot race saves the Flipper in C# before contact; ROM order consumes contact before its moving-shot scan. |
| Pivot-lane shootability | **Possible contributor** | C# accepts both lanes for the whole pivot; ROM's visual mapping is unresolved. |
| Shot capacity (10 vs 8) | **Not a demonstrated contributor to the measured survival outcome** | In the four-pursuer ablation, 8 slots and 10 slots both die at tick 265 with two pivot kills, though 10 slots accepts eight more shots. |
| Shot lifetime / slot recycling | **Possible contributor (lifetime only)** | Longer C# occupancy can increase lane coverage and also block new shots; actual hit retirement frees C# capacity, unlike ordinary ROM hit handling, which counteracts excessive safety. No matched-clock arcade survival comparison exists. |
| Rim pursuit timing | **Unresolved** | C# timing is measured; ROM absolute lane timing cannot be derived from the available master data. |
| Difficulty | **Not a contributor to the tested player-shot/order difference** | No player-shot adjustments are identified; the physical cabinet setting and effect of enemy difficulty on a full wave remain unverified. |

## 15. Root-cause conclusion and recommended implementation contract

Stationary C# safety is reproduced **conditionally**, not as a universal result: 1-2 pursuers are cleared without death in 600 ticks, while four create death at tick 265. The deterministic evidence demonstrates one concrete ROM/C# safety advantage—the already-moving shot/contact race—but does not prove it is the sole cause of the reported whole-game difference.

The narrowest future production contract to review is to preserve a new shot's immediate birth-position collision opportunity, allow enemy movement/contact to resolve, and defer only the later moving-shot-versus-Flipper hit resolution until after Flipper movement/contact. Do not change fire cadence, shot speed, simulation speed, difficulty, or accepted below-rim enemy behavior as part of that first correction. The pivot-lane rule should not be narrowed without decoding the ROM state-to-visual-lane mapping or obtaining targeted arcade observation.

This contract is supported for the concrete race, but a full correction should not be authorized on these tests alone. Neal should review the evidence and approve any gameplay change.

## 16. Mechanics that should remain unchanged absent new evidence

- Held-level Fire and the current player-shot cadence.
- Player-shot travel speed.
- Global simulation speed.
- The immediate new-shot birth-position check when introducing any ordering correction.
- Ordinary-hit slot recycling unless an independently validated ROM mapping calls for a change.
- Existing below-rim Flipper behavior.
- No arbitrary stationary-player death or anti-camping rule.
- No guessed ROM rim-pause constant or broad pivot-lane change.
- No difficulty switch or unsupported claim that the cabinet is Medium.

## 17. Validation

- Focused comparison tests: **11 passed, 0 failed, 0 skipped**.
- Full solution tests (`dotnet test TempestCSharp.slnx`): **755 passed, 0 failed, 0 skipped**.
- Solution build (`dotnet build TempestCSharp.slnx`): succeeded, **0 warnings, 0 errors**.
- `git diff --check`: passed with no output.

## 18. Changed files and final repository state

No previously tracked file was modified. One new, unstaged test/harness file is present:

- `tests/Tempest.Game.Tests/Level1StationaryRimCombatComparisonTests.cs`

The requested prompt and report are saved under ignored `AI-Logs/` paths:

- `AI-Logs/Prompts/2026-11-04-level1-stationary-rim-combat-comparison-prompt.md`
- `AI-Logs/Reports/2026-11-04-level1-stationary-rim-combat-comparison-report.md`

Final Git checks: branch `master`; `HEAD` remains `3bebf42cbe9514846ab32bf4c8a12bcfdb048da7`; `git status --short --branch` shows only `?? tests/Tempest.Game.Tests/Level1StationaryRimCombatComparisonTests.cs`; both `git diff --stat` and `git diff --cached --stat` are empty. Both AI records are ignored by `.gitignore`.

No production gameplay code, authority document, or reference file was changed. No game was launched; no commit or push was made. No Internet disassembly search occurred. Stop here for Neal's review.
