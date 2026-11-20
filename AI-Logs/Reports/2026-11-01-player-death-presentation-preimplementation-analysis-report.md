# Player-Death Presentation: Pre-implementation Analysis

## Disposition

**GO, split into two checkpoints.**

1. Implement enemy retraction as a presentation-only rendering feature, preserving the existing 60-tick C# death lifecycle and reconstruction boundary.
2. Refine the Blaster-location death animation after Neal supplies arcade footage sufficient to establish its geometry and timing.

The enemy-retraction requirement has enough evidence to define a safe architecture: Neal directly observed retraction on Atari, and the current C# death/reconstruction boundary is well defined. The exact retreat pacing and the Blaster animation's shape remain unverified. This analysis changed no production code, tests, tracked documentation, or Git history. The requested ignored report is the only file created.

## Evidence labels

- **ARCADE VERIFIED**: Neal's supplied direct Atari observations: ordinary death includes visible enemies retracting backward/down the tube, a death animation at the Blaster's location, and the unresolved enemy supply returning according to new-life behavior. Retreat does not destroy or score enemies.
- **REFERENCE VERIFIED**: behavior literally present in `Reference/incitti`; this is not proof of Atari fidelity.
- **CURRENT C#**: behavior in the source/test evidence at HEAD `d07e6d6d59e6004db0c52aceab18ad27339e2bb3`.
- **PROJECT DECISION**: recommended behavior/architecture for the future recreation, not an arcade-fidelity claim.

## Atari / Incitti / C# comparison

| Topic | Atari observation | Incitti source | Current C# |
|---|---|---|---|
| Enemies on ordinary death | **ARCADE VERIFIED:** remain visible and retract away from the player; retreat itself neither destroys nor resolves them. | The ordinary death state continues enemy updates. It then enters `LEVEL_INTO_VORTEX`, where shared `ZOFFSET` advances by 3 per tick. No enemy-specific retreat or reverse-height update exists in the inspected paths. At the end, enemies are converted to Eggs. | Active runtime entities remain represented throughout `PlayerDying`; gameplay updates are frozen. At tick 60, unresolved supply is reconstructed from level accounting. There is no retreat offset. |
| Blaster presentation | **ARCADE VERIFIED:** a visible animation occurs at the Blaster's location. Exact shape/timing not established. | `Player.Draw()` has cause-dependent effects; this is not Neal's reported arcade treatment. In particular, do not copy the reported backward/off-screen Blaster motion as the arcade solution. | The renderer draws a player death effect over the 60-tick window. Its current geometry is a C# implementation, not evidence of the arcade animation. |
| Re-entry | Remaining unresolved supply returns under accepted new-life behavior. | After the ordinary death/vortex phases, active enemies are removed and eligible remaining supply is represented by Eggs. | `Level.ResetAfterDeath` keeps terminal outcomes and reconstructs unresolved identity/reserve state as fresh Eggs. The current level and score remain. |
| Bullets / hazards | Exact behavior beyond the supplied observation is not established. | Bullets are removed during conversion; spikes are retained. The source does not implement a dedicated enemy retreat animation. | Player shots are cleared when death starts; enemy bullets remain frozen until reset and are then cleared. Spikes are level geometry and persist. |

The report does not treat the reported Incitti visual result as proof of arcade behavior, nor does it treat source behavior as proof of Neal's observation.

## Current C# lifecycle at `d07e6d6`

### Trigger and 60-tick window

- Lethal collision flows call `KillPlayer`; it accepts only `Playing`, sets `DeathTicks = 60`, switches to `PlayerDying`, and clears player shots (`src/Tempest.Game/TempestGame.cs:461-467`). Trigger paths include Flipper contact/pivot, enemy bullets, Fuseballs, Pulsars, and ordinary spike collision (`TempestGame.cs:242-279`, `:1168-1185`).
- During `PlayerDying`, each simulation step decrements the death timer. The gameplay simulation is frozen: enemy, Egg, bullet, pulse-clock, and pending enemy-target timers do not advance. Score popups and the Super Zapper display flash continue advancing (`TempestGame.cs:184-243`; `SuperZapperTests.cs:578-617`).
- The renderer presents the player death effect while `DeathTicks > 0` (`src/Tempest.Game/Rendering/VectorRenderer.cs:84-126`, `:617-638`). Active entities remain present in simulation state until the reset boundary. The evidence reviewed here does not establish a per-subclass rendering/submission result for every entity during those ticks; that should be made an explicit rendering-test assertion in the future milestone.

### Reset, score, lives, and identity

- At tick 60, exactly one life is deducted. If lives remain, runtime shots, bullets, Flippers, Tankers, Spikers, Fuseballs, Pulsars, score popups, and collision-order entries are cleared; the same `Level` is retained; unresolved supply is rebuilt as fresh Eggs; spikes persist (`TempestGame.cs:427-460`; `src/Tempest.Game/Levels/Level.cs:110-150`).
- Ordinary death does not reset score. Existing destroyed and rim-cleared outcomes remain terminal and are not recreated. Normal accounting remains reserve + Eggs/active + destroyed + rim-cleared = supply (`Level.cs:6-11`, `:34-48`, `:373-375`).
- Level 3 uses explicit original-enemy and cargo identities. Cargo remains associated with its original Tanker and ordinal; the rebuilt runtime objects come from unresolved ledger entries rather than preserving object references (`src/Tempest.Game/Levels/Level3WaveLedger.cs:1-115`; `Level.cs:37-48`, `:115-145`). Tanker development mode similarly tracks originals, latent/released cargo, kills, escapes, and rim clears (`TempestGame.cs:35-49`, `:350-419`; `src/Tempest.Game/Gameplay/Tanker.cs:40-162`).
- No death-presentation retreat may increment destroyed/rim-cleared counts, score a kill, mutate the ledger, release/re-own cargo, or change pending terminal outcomes. Runtime object replacement at reconstruction is intentional; identity/outcome continuity is the invariant.

### Ordinary life loss versus Game Over

- Both ordinary and final-life death use the 60-tick `PlayerDying` interval. If lives remain, the state resets/reconstructs unresolved supply. If the last life is consumed, the state becomes `GameOver`, runtime entities are cleared, and no new-life reconstruction occurs (`TempestGame.cs:427-460`).
- `RunOneStep` is idle in `GameOver` except for an explicit restart. Restart builds a fresh level and resets score/lives using the original launch configuration (`TempestGame.cs:199-215`, `:289-294`).
- **PROJECT DECISION:** attach presentation to `PlayerDying`, so ordinary and final-life deaths show the same bounded presentation unless future Atari footage establishes a difference. Preserve the existing Game Over boundary and restart behavior; do not extend or redesign it in the presentation change.

### Levels and development scenarios

- The normal campaign is Level 1 circular -> Level 2 square -> Level 3 mixed plus board. Direct Level 2/3/geometry and enemy diagnostic configurations are isolated launch scenarios, but share the same central `TempestGame` death/reset path (`TempestGame.cs:135-171`, `:204-215`).
- One depth-space presentation model can serve all three boards because the visual displacement should follow the existing tube-depth projection, not board-specific screen coordinates. Level 3's Flippers, Tankers, and cargo must still render from their existing identities and relationships.
- Apply the shared implementation to `--level2-test`, `--level3-test`, `--level3-geometry-test`, `--tanker-test`, `--spiker-test`, `--fuseball-test`, `--pulsar-test`, and enemy-bullet diagnostics where the mode can enter ordinary death. Do not add a new CLI mode.

## Incitti death path — REFERENCE VERIFIED

Source: `Reference/incitti/tempest.bmx`, with projection helpers in `Reference/incitti/transformfunctions.bmx`.

### States, timing, and projection

- `Player.deathcount`, `Player.deathtype`, and `Player.men` are the player-side state (`tempest.bmx:601-603`). `KillPlayer()` sets `deathcount = 60`, records the cause, and selects `LEVEL_PLAYER_DYING` (`:3956-3981`).
- The ordinary path spends 60 ticks in `LEVEL_PLAYER_DYING`, then 60 more in `LEVEL_INTO_VORTEX`; `ZOFFSET` advances by 3 per tick in that second state (`:1328-1342`). The shared transform adds `ZOFFSET` to projected depth (`transformfunctions.bmx:177-190`).
- This is a shared projection change, not enemy-specific retreat. `DrawBaddies()` continues to draw enemies in both death states; their own height remains governed by their ordinary movement. No inspected enemy update/draw path reverses height specifically for death (`tempest.bmx:2240-2278`, `:2310-2325`, `:2588-2635`). Therefore the source verifies a whole-scene depth/projection shift during vortex, but does **not** contain a discrete per-enemy retraction model or specify its direction/rate.
- The main loop continues drawing Eggs and Baddies during `LEVEL_INTO_VORTEX`, while the gameplay update block for Eggs, Baddies, Pulsars, Fuseballs, Shots, and Player is skipped (`tempest.bmx:3759-3787`). Existing Eggs are therefore visible/frozen in that phase; reconstructed Eggs are not created until conversion at its end.

### Entity and system effects

- Ordinary death has no explicit enemy freeze for its first 60 ticks: `UpdateBaddies()` continues, and collision checks are suppressed (`tempest.bmx:2240-2265`). The player cannot fire, use Super Zapper, or rotate while `deathcount != 0` (`:647-705`).
- `ConvertBaddieToEggs()` removes non-spike/non-bullet baddies, removes bullets, retains spikes, resets wave counters, and rebuilds the remaining eligible supply as Eggs (`:2248-2288`). This mutates gameplay state; it is not presentation-only. Egg hatching can select among level-available enemy types (`:2050-2100`).
- During ordinary death, Eggs can continue their normal updates; during `LEVEL_INTO_VORTEX`, existing Eggs are drawn but frozen. The source does not establish that all precursor behavior matches Atari.
- Enemy bullets are removed by conversion, not reconstructed as Eggs. Spikes are retained. Tanker cargo behavior can be affected by ordinary Tanker death/release paths (`:2490-2585`); the inspected death conversion should not be copied as a C# cargo/accounting model.
- Super Zapper charges are initialized on wave setup; its display timer is 60 ticks and advances in level updates (`:1410-1415`, `:1450-1455`). Zap targets use per-enemy delayed death timers (`:2289-2350`, `:2588-2610`). These are Incitti mechanics, not requirements to change C# semantics.
- `Player.Draw()` varies effects by cause, including shrink, depth oscillation, or particles (`:760-909`). It does not provide evidence for Neal's exact Blaster-location animation. Do not use the reported backward/off-screen Blaster retreat as an Atari design.

### Spike-death exception and Game Over

- Spike death bypasses the ordinary two-phase path: it immediately calls `ConvertBaddieToEggs()` and selects `LEVEL_REVERSE_ZOOM` (`tempest.bmx:3970-3974`). In reverse zoom, `ZOFFSET` advances by 1/tick; spikes remain and are drawn while non-spike baddies are suppressed (`:1365-1390`, `:2259-2278`).
- Incitti decrements lives after the 60-tick death count. Its Game Over check occurs when `men < 0`, then `GameOver()` waits for input (`:3868-3872`, `:3920-3950`). This boundary differs in implementation details from C# and is not an arcade behavior to import.

## Safest future retraction architecture

**Recommendation: model B, presentation-only positions/proxies, driven by game-owned presentation progress.**

- Keep gameplay ownership in `TempestGame`; expose a read-only death-presentation progress value (or derive it from its existing `PlayerDying` timer). Renderer consumes that value and applies a bounded offset in tube-depth space to entity render positions.
- Do **not** mutate `Enemy.Height`, lane index/transition state, timers, collision state, ownership, or ledger state. A depth offset at render submission preserves the frozen gameplay snapshot and prevents presentation from contaminating reset or accounting. Existing accepted tests assume death simulation freezes; changing actual heights would undermine that invariant even though unresolved objects are subsequently replaced.
- Apply one shared depth policy to each active enemy's rendered position, retaining its current lane/fractional transition position. A transition already in progress should be frozen where it was; don't complete, reverse, or resolve the transition as part of retreat. Include active Tankers and released cargo without changing their logical ownership. Clamp/guard positions at the ends of the projection domain so no invalid or non-finite vertex is submitted.
- Treat Eggs, enemy bullets, Spikes, Pulsar hazards, and Super Zapper visuals separately from active enemy retreat. Neal's supplied observation does not resolve them, and Incitti has distinct update/conversion behavior. Preserve current C# policy unless footage shows otherwise: no player shots after death trigger; enemy bullets freeze then clear at reset; spike geometry persists; pulse state freezes; Super Zapper display may continue while target timers remain frozen. Do not hide hazards or change collisions to simplify rendering.
- Incitti's shared `ZOFFSET` is a useful projection-space precedent, but do not transplant its entity deletion/conversion or its cause-specific Blaster visuals.
- Keep the Blaster death animation as a separate presentation component/state seam. Its current C# effect can be replaced/refined later without coupling it to enemy accounting. Do not specify explosion geometry until footage is available.
- Avoid a general animation framework. A small death-presentation progress value plus a render-only transform is sufficient.

## Eggs, shots, bullets, hazards, and Super Zapper policy

| Element | Current C# death behavior | Incitti ordinary death | Future policy / evidence gap |
|---|---|---|---|
| Eggs | Simulation frozen; unresolved units are rebuilt as fresh Eggs at reset. Exact on-screen submission during `PlayerDying` was not established by the evidence reviewed. | Continue updating during first death phase; drawn and frozen during Into Vortex; rebuilt/adjusted at conversion. | Do not infer Atari behavior. Add explicit render tests and use footage before changing visibility or retreat. |
| Player shots | Cleared immediately by `KillPlayer`. | Player controls are gated during death; exact shot cleanup/rendering timing was not established here. | Keep current immediate clear; don't change shot behavior for the retreat effect. |
| Enemy bullets | Remain frozen until the reset, then are cleared. | Removed by conversion. | Do not count as killed enemies; preserve current reset semantics. |
| Spiker / Spikes | Runtime Spiker is cleared/reconstructed; spike geometry persists. | Spikes are retained, including the special reverse-zoom handling. | Keep spike geometry on its existing path; do not automatically depth-retreat it with active enemies. |
| Pulsar | Enemy/timer and pulse clock freeze during `PlayerDying`. | Baddies/pulse updates continue in first death state; gameplay updates freeze during Into Vortex. | Maintain C# timer semantics; preserve existing overlay visibility unless arcade footage demonstrates a presentation-only change. |
| Super Zapper | Charge/terminal state is preserved; display flash can advance; pending target timers freeze. Existing tests cover charge/flash and pending-timer semantics (`SuperZapperTests.cs:578-617`). | Display timer advances; target effects use enemy death timers, with behavior dependent on the death phase. | Do not reset charges, retarget, or advance pending C# timers during presentation. |

The Atari observation does not resolve whether distant Eggs remain visible, retract, disappear, or are rebuilt during the animation. Do not label any of those alternatives arcade behavior without footage.

## Blaster-lane highlighting

The accepted renderer keeps the lane highlight visible in `PlayerDying`; tests assert the visibility policy (`src/Tempest.Game/Rendering/VectorRenderer.cs:397-406`; `tests/Tempest.Game.Tests/BlasterLaneHighlightTests.cs:117-136`). Incitti also keeps the lane marker through `LEVEL_PLAYER_DYING`. Neal's supplied death observation does not say whether the lane remains visible through the retreat.

**PROJECT DECISION:** leave the accepted C# highlight policy unchanged for the first retraction checkpoint. Do not fade, remove, or move it with the player/enemy offset unless arcade footage requires that change. Add a regression assertion that any render-only enemy offset does not alter lane selection or color precedence.

## Timing and animation shape

- Current C# death window: exactly 60 simulation ticks; no timing change is recommended.
- Incitti: 60 ticks of death count plus a 60-tick Into Vortex phase for the ordinary path. Its timing is **REFERENCE VERIFIED**, not arcade-verified.
- Fit the first C# retraction and Blaster presentation within the existing 60 ticks. Do not extend death duration or postpone life consumption/Game Over in this milestone. Exact retreat pacing, sequencing, and Blaster animation timing need Atari footage.
- Neal's description establishes the Blaster-location event, but not shape, particle count, scale, color, or exact frame timing. Build only a replaceable seam, not speculative geometry.

## Future deterministic test strategy

Add focused tests without weakening or replacing existing death/reconstruction tests:

1. `KillPlayer` enters presentation once; the life decrements exactly once at the existing boundary.
2. Score, level, destroyed/rim-cleared outcomes, unresolved identity set, Tanker original/cargo ownership, and Super Zapper state remain unchanged during presentation.
3. Death presentation advances monotonically and completes in exactly 60 ticks; no presentation-only retreat awards score or resolves an enemy.
4. Gameplay enemy height, lane position/transition, timers, Egg state, bullet state, and ledger stay frozen during the presentation.
5. Render-only retreat moves each active enemy away from the player in tube-depth space and emits finite positions; test representative enemy subclasses, all levels 1–3, rim-position entities, and directed transitions.
6. Tanker plus active cargo render consistently without changing ownership or cargo outcome. Include original Flipper/Tanker and cargo identity ledger assertions.
7. Preserve spikes, player-shot clearing, enemy-bullet clearing, Pulsar state, Super Zapper charge/flash/target behavior, and lane highlight visibility/color precedence.
8. At completion, run existing post-death identity/reconstruction assertions: unresolved identities appear exactly once, terminal identities remain absent, counts agree, and no score/life/level reset occurs except the established life loss.
9. Test ordinary death and final-life Game Over separately: same presentation boundary, no final-life reconstruction, unchanged restart behavior.
10. Exercise the common path in production Levels 1–3 and applicable development scenarios; keep all existing regression tests intact.

Do not use reflection just to manufacture test state; use existing scenario builders/helpers.

## Manual acceptance checklist for Neal

- **Level 1:** die to a Flipper/bullet; enemies visibly retreat rather than simply vanish; retreat produces no kill score; Blaster-location effect is present; unresolved wave returns correctly; exactly one life is lost.
- **Level 2:** square-board retreat looks coherent and normal re-entry follows.
- **Level 3:** mixed Flippers/Tankers retreat coherently; cargo ownership/outcomes survive; no missing or duplicate units.
- **Game Over:** final death presentation remains coherent; Game Over occurs at the established boundary; restart remains unchanged.
- **Visual fidelity:** compare retreat direction/depth and pacing with Atari; later compare Blaster animation with real arcade footage; check for projection explosions, flicker, lane-color artifacts, non-finite geometry, and crashes.
- Repeat representative checks in relevant development scenarios; confirm they use the same presentation rather than a separate diagnostic path.

## Footage needed and checkpoint shape

Sufficient to proceed with bounded infrastructure: Neal's direct Atari observation that active enemies retract; current C# identity/reconstruction tests and accounting; and the source evidence that a render-only depth transform can be kept separate from gameplay entity mutation.

Further Atari footage is needed for exact retreat pacing and sequence, Eggs/precursors, bullets/hazards, whether the lane cue persists, ordinary versus final-life differences, and especially the Blaster-location animation's shape/timing. Incitti is not a substitute for these observations.

**Recommended split:** first checkpoint implements enemy retraction/presentation progress and tests identity/accounting invariants; second checkpoint refines the Blaster-location animation after footage. The second checkpoint can be small and isolated; do not block safe enemy retraction on footage, and do not invent a large speculative animation.

## Conclusion

**GO** for a bounded enemy-retraction presentation milestone using a render-only depth offset and the existing 60-tick death window. **NO-GO** for claiming Incitti implements an enemy-specific retreat, for changing identity/accounting or hazard semantics, or for finalizing the Blaster animation geometry without arcade footage.

## End-of-run record

- **Disposition:** GO for presentation-only enemy retraction; split Blaster animation refinement.
- **Current C# death behavior:** `PlayerDying` lasts 60 ticks; simulation freezes, one life is consumed at completion; score and same-level unresolved identity accounting are preserved; unresolved supply rebuilds as Eggs if a life remains.
- **Incitti enemy-retraction finding:** common projection `ZOFFSET` shift during `LEVEL_INTO_VORTEX`, not enemy-specific reverse-height movement; source does not substantiate a dedicated retraction model.
- **Atari / Incitti Blaster distinction:** Atari observation is a Blaster-location death animation; Incitti uses cause-specific effects and is not the arcade geometry.
- **Recommended architecture:** game-owned/read-only progress with a render-only depth offset; never mutate gameplay enemy positions or identities.
- **Identity/reconstruction protection:** keep score, terminal outcomes, unresolved identities, cargo ownership, counts, and Zapper semantics unchanged; reconstruct only at the existing boundary.
- **Egg/projectile/hazard findings:** C# shots clear at trigger, bullets freeze then clear, spikes persist, pulse timers freeze, Zapper flash advances while target timers freeze. Atari Egg/hazard presentation remains uncertain.
- **Lane highlight:** preserve accepted `PlayerDying` visibility for now; test against the new rendering path.
- **Ordinary death / Game Over:** both use the same 60-tick death window; reconstruction only if a life remains; restart unchanged.
- **Timing:** do not change 60 ticks; Incitti ordinary path includes an additional 60-tick vortex phase, not arcade-verified.
- **Likely implementation areas:** `TempestGame.cs`, a small death-presentation state/value, `VectorRenderer.cs`, entity render submission paths, and death/reconstruction/render tests.
- **Automated plan:** deterministic progress, frozen gameplay state, finite monotonic retreat rendering across entity types and Levels 1–3, unchanged accounting/score/lives/cargo/Zapper state, and ordinary/final-life boundaries.
- **Manual plan:** Level 1/2/3, Game Over, no score on retreat, correct re-entry, visual pacing, Blaster effect, projection/lane artifacts.
- **Footage would clarify:** exact timing, retreat pacing, Eggs/bullets/hazards, lane policy, ordinary/final-life differences, and Blaster animation geometry.
- **Checkpoint recommendation:** A. enemy retreat/presentation infrastructure; B. Blaster death-animation refinement.
- **Blockers/uncertainties:** source-specific retreat is not found in Incitti; multiple Atari presentation details remain unobserved, especially Blaster shape/timing.
- **Report:** `AI-Logs/Reports/2026-11-01-player-death-presentation-preimplementation-analysis-report.md`.
- **Tracked files:** none changed; no commit or push.
- **Git HEAD/status at inspection:** `d07e6d6d59e6004db0c52aceab18ad27339e2bb3`; branch `master`; pre-existing untracked `TEMP/` remains untouched.
