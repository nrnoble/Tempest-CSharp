# Level 5 Preimplementation Analysis

## CURRENT CHECKPOINT / STATUS

- Requested checkpoint: `8fcfc24 Implement accepted arcade sound effects`.
- Verified `HEAD`: `8fcfc2463fdde2939d3aa70c6113500fbc5b0c94`.
- Initial worktree: clean (`git status --short` returned no entries).
- Scope: read-only analysis of production Level 1–4, Incitti's original-arcade level table, and the next Level 5 milestone. No production code, tests, tracked documentation, assets, index, or commits were changed.
- This report is under ignored `AI-Logs/`; its creation does not dirty the worktree.

The checked-in reference is `Reference/incitti/`, not `AI-Logs/Reference/`. The matching Level 1–4 configurations already correspond to the first four entries in `Reference/incitti/levelsets/levelset1.txt` (“Original Arcade”), which identifies the correct Level 5 row for this campaign.

**Important naming distinction:** `levelset1.txt` is the “Original Arcade” set and its fifth board is **Level 5 - cross**. `levelset5.txt` is a different, custom *set* called “Something For Everyone”; its fifth board is the open “curves” shape. It is not the reference for production Level 5 in the accepted campaign. See `Reference/incitti/levelsets/levelset1.txt:1-12` and `levelset5.txt:1-10`.

## CURRENT LEVEL 1–4 PROGRESSION ARCHITECTURE

### Configuration and level selection

`LevelConfiguration` is an immutable record containing progression/display identity, board and palette indices, a `LevelDefinition`, depth/position/player start, enemy supply, Egg pacing/sequences, exit policy, and normal enemy composition. Flipper tuning and optional initial Spike height are additional init properties. Actual geometry is supplied explicitly by `LevelLoader`; `BoardIndex` is identity/data, not an automatic geometry lookup (`src/Tempest.Game/Levels/LevelConfiguration.cs:24-59`).

| Production level | Progression / board / palette | Geometry | Original supply | Initial Eggs / reserve | Composition and notable flags |
|---|---|---|---:|---:|---|
| 1 | 0 / 0 / 0 | Closed 16-lane circle | 10 | 4 / 6 | Flippers only; cannot flip |
| 2 | 1 / 1 / 0 | Closed 16-lane square | 12 | 4 / 8 | Flippers only; can flip |
| 3 | 2 / 2 / 0 | Closed 16-lane plus | 15 | 4 / 11 | Natural `Level3Mixed`: Flippers and Tankers; two Flipper cargo units per Tanker |
| 4 | 3 / 3 / 0 | Closed 16-lane binoculars | 17 | 4 / 13 | Flippers, Tankers, Spikers; 16 height-60 Spikes; `AvoidSpikes` |

All four use depth 400, position 50, player start lane 7 at 0.5, a 50-tick reserve-release interval, and Flipper base tuning `(canFlip, climb, turn, lane-pause, rim-pause)` of `(false,2,8,21,24)` for Level 1 and `(true,2,8,21,24)` thereafter. The Level 4 values are explicit in `LevelConfiguration.cs:146-170`; Level 1–3 values are in the preceding static configurations. Level 3's plus-only `Level3Plus` diagnostic is not the natural mixed wave.

`LevelProgression.GetNextProductionLevel` is a reference-identity chain for 1→2→3→4 and returns null after Level 4. Normal progression is committed only after successful exit travel, not at wave-clear time. Standard exits use `UpdateExitZoom`; Level 4's `AvoidSpikes` path instead ends in terminal completion. Direct scenario modes are kept out of natural progression (`LevelProgression.cs:3-10`, `TempestGame.cs:1664-1730`).

### Geometry, palette, enemy selection, and accounting

- The four definitions are explicitly loaded and all closed. `LevelDefinition` already represents closed/open topology, but Level 5's Original Arcade board is also closed and keeps 16 vertices/lanes.
- Palette 0 represents the Level 1–16 source group. Current rendering resolves only palette-0 Blaster-lane color (yellow); tube, Blaster, and enemy colors are otherwise fixed renderer colors. Level 5 remains in palette 0.
- Production compositions are `FlippersOnly`, `FlippersAndTankers`, and `FlippersTankersAndSpikers`. Fuseballs and Pulsars exist as gameplay/test systems but are not enabled in the Level 1–4 production composition.
- The Level 3/4 production ledger conserves original obligations and separate Tanker cargo identities. Level 4 reuses it for Flipper/Tanker/Spiker accounting. Spikes are separate hazards, not enemy obligations. `Level.ResetAfterDeath` reconstructs unresolved units as Eggs and preserves resolved outcomes and Spikes.
- Fresh-level initialization constructs the configured level, clears old wave/transition objects, restores the player start, resets the Super Zapper to its initial two charges, clears its flash, and does not reset score or lives. Ordinary death reconstructs the current level instead (`TempestGame.cs:345-380`).

Some deliberately fixed or over-specific Level 1–4 assumptions remain:

1. The progression resolver explicitly enumerates the four configurations; it is not driven by board index.
2. `Level.EnemyTotal` still returns the Level 1 supply, although no production consumer was found; new logic should use `CurrentLevel.EnemySupply`.
3. There is no Level 5 CLI diagnostic flag. A configuration can be passed directly to the test-facing `TempestGame(LevelConfiguration)` constructor, but that constructor does not enable natural progression.
4. `Level3WaveLedger`/`Level3Ledger` names persist even though the same accounting already serves production Level 4. This is naming debt, not a functional blocker for the same Level 5 composition.
5. The accepted star color decision is explicitly keyed to the **exited** displayed level, not the incoming level.

### Eggs, firing, Zapper, and presentation

Level configuration holds initial Egg count, reserve cadence, active/approaching limits, hatch cadence, and deterministic lane/direction sequences. The mixed production-ledger path uses its established source-shaped selection/release policy rather than every simple-wave sequence field: releases can batch up to six reserve identities on a cadence, and up to two Eggs approach while fewer than nine enemy units are active. Tanker cargo can create additional children beyond the original supply.

Firing has a known, documented policy distinction:

- Tanker firing is active for production Levels 3–4. Level 4's source-shaped Tankers receive 1–2 rounds, fire below height 380, and use the shared enemy-bullet path.
- Production Level 4 Spikers can fire their source-shaped growth/retreat bullet. These bullets do not add enemy obligations.
- Ordinary and cargo Flippers are intentionally silent in accepted production Levels 1–4. `--enemy-bullets-test` enables their firing in diagnostics. The behavior specification explicitly records this as a project discrepancy from Incitti, who makes Flippers bullet-eligible from Level 1 (`TEMPEST-BEHAVIOR-SPEC.md:553-568`). Do not silently change this accepted policy as a side effect of Level 5.

The fresh-level Super Zapper recharge and the 181-update `LevelApproach` are configuration-independent and already reused. The new wave is installed at the successful exit commit, the outgoing-level approach presentation continues seamlessly, and the incoming wave remains frozen until arrival. The accepted presentation's recharge text, 960 depth offset, and four-second first-star-to-arrival duration do not need Level 5-specific mechanics.

## INCITTI LEVEL 5 FACTS

The source's internal `current_level` is zero-based; it displays `current_level + 1`. Natural completion increments it, selects `current_board = current_level Mod 16`, and selects `current_color = current_level/16` (`tempest.bmx:1420-1430`, HUD display at `3805`). Thus displayed Level 5 has internal index 4, board 4, and palette group 0.

`Reference/incitti/levelsets/levelset1.txt:10-11` supplies the exact source row:

```text
Level 5 - cross
1,415,-90,-30,210,-60,120,-120,60,-210,30,-210,-30,-120,-60,-60,-120,-30,-210,30,-210,60,-120,120,-60,210,-30,210,30,120,60,60,120,30,210
```

In C# field terms: closed (`Continuous = true`), `CenterY = 415`, `YOffset = -90`, and the ordered 16 points listed below. The source loop reads 16 point pairs and closes the last edge when the first field is true (`tempest.bmx:4200-4210`).

```text
(-30,210), (-60,120), (-120,60), (-210,30),
(-210,-30), (-120,-60), (-60,-120), (-30,-210),
(30,-210), (60,-120), (120,-60), (210,-30),
(210,30), (120,60), (60,120), (30,210)
```

### LEVEL 5 COLORS

At internal index 4, `current_color` is 0. Incitti's Level 1–16 palette assigns: tube/web blue; Blaster and highlighted lane yellow; bullets white; Flippers red; Tankers magenta; Spikers/Spikes green; Pulsars cyan; informational text blue (`tempest.bmx:4008-4047`). Level 5 is not a palette change. Existing C# palette-0 lane highlighting and fixed colors already match these source categories.

The accepted transition starfield is a separate presentation rule:

- Exiting Level 4 and entering Level 5 still uses the blue-only stars because the outgoing level is 4.
- Exiting Level 5 uses the accepted white/red/green/blue mixture because the outgoing level is 5.

This follows the actual call chain: ExitZoom draws with the current/outgoing `DisplayedLevel`; approach draws with `CurrentApproach.ExitedLevel`; `TransitionStarfield.Draw` chooses blue for `exitedLevel <= 4` and the mixture otherwise (`VectorRenderer.cs:184-195`, `TransitionStarfield.cs:17,26-48`). The behavior spec's “blue exits 1-4” and “mixture from exit 5 onward” is consistent, but “exit 5” should always mean **the transition after completing Level 5**, not entry into it (`TEMPEST-BEHAVIOR-SPEC.md:918-930`).

### LEVEL 5 WAVE / ENEMY COMPOSITION

Incitti's setup calculates `eggsleft = 10 + lv/2 + current_board*2`, caps at 64, sets `maxeggs`, and creates four initial Eggs while decrementing the reserve. For `lv=4`, `board=4`, this is **20 original obligations: four initial Eggs, 16 reserve, zero active enemies at setup** (`tempest.bmx:4210-4224`). This is original wave supply, not the total number of enemy objects eventually encountered: each selected Tanker carries two Flippers.

Level 5 enables:

| Source type / C# name | Level 5 state | Source evidence |
|---|---|---|
| Flipper | Enabled; canflip true | Flippers are always enabled; only `lv=0` disables canflip (`tempest.bmx:4225-4242`, `4266`) |
| Spiker (Incitti calls it Spinner) | Enabled; board has Spikes | Source enemy type 0 is eligible when `hasspikes`; it builds a persistent Spike |
| Tanker | Enabled; carries two Flippers | Board 4 exceeds the `current_board > 1` threshold; only Flipper cargo is eligible at this level |
| Pulsar | Disabled | Requires `lv > 15` |
| Fuseball | Disabled | Requires `current_board > 9` or `lv > 20` |
| Tanker-carried Pulsars / Fuseballs | Disabled | Require later `lv > 26` / `lv > 31` thresholds |

The board exceeds `current_board > 2`, so it starts with **16 height-60 Spikes**, one per closed lane. This is independent of the 20 enemy obligations (`tempest.bmx:4232-4249`). Level 5 therefore has the same enabled normal enemy family and Spike/exit policy as accepted Level 4; the original supply rises from 17 to 20.

Incitti does not define a fixed per-type quota. `Egg.PickEnemy` samples a random candidate, retries disabled types, suppresses Spikers near the end (`enemiesleft + egg_list.Count() < 4`) or above three active Spikers, and caps Tanker selection when six are active (`tempest.bmx:1982-2056`). This results in a randomized Flipper/Spiker/Tanker wave. A Tanker selected at Level 5 carries two Flippers; alternate cargo types are disabled. The C# production picker/ledger already models this family and its caps.

## TIMING / FIRING / CANFLIP / HURRY-UP

With internal `lv=4`, source setup gives:

| Setting | Level 5 reference value |
|---|---|
| Enemy reserve release | Every 50 global ticks; up to six Eggs released at a release visit |
| Egg start | Four Eggs; 20 maximum total original units |
| Egg approach selection | Active enemies below 9, fewer than 2 hatching; random `Rand(0,100) > 95-hatchrate` or fewer than 4 active enemies plus Eggs |
| `hatchrate` | `lv/8` = 0 at this level |
| Flipper canflip | True |
| Flipper turn speed | `8 + lv/5` = 8 degrees |
| Flipper climb | 2 |
| Flipper lane/rim pause | 21 / 24 ticks |
| Tanker climb | `1.75 + lv/20`; source/C# treat this expression as integer division at these levels, so 1.75 |
| Spiker/Spinner speed | `lv/48.0 + Rand(2.5,4.5)` |
| Initial Spiker/Spinner ammunition | Not a fixed wave quota; its source bullet roll is conditional on Spike growth/retreat |

The source also sets Pulsar turn speed to `10 + lv/8`, Fuseball climb to `3.3 + lv/20`, and pulse pauses, but these types do not hatch at Level 5. They are not Level 5 configuration requirements. Relevant formulas and assignments are at `tempest.bmx:4251-4266`; Egg timing/selection is at `2080-2101`.

Source firing details:

- Flippers receive `Rand(1,3)` bullets and a 16-tick countdown; an eligible firing visit uses `Rand(0,100)>75`. Incitti calls `Shoot()` while climbing and after the pause transition (`tempest.bmx:2767, 2838, 2860-2867, 2930-2931`). C# deliberately keeps ordinary/cargo Flippers silent in accepted production Levels 1–4. This is an existing project deviation, not a new Level 5 firing system; deciding to change it requires Neal's arcade judgment.
- Tankers receive `Rand(1,2)` bullets and initialize `bullettime = 16 + Rand(64)`. Below height 380, they decrement the timer, reset it to 16, and fire when the `>75` roll succeeds (`tempest.bmx:2575-2583, 2632-2633`). C# already enables production Tanker bullets. Its initial timer expression uses `16 + NextRandomInclusive(1,64)` (`TempestGame.cs:729`), which excludes the source's possible `16` if BlitzMax's single-argument `Rand(64)` includes zero; preserve or correct only after confirming the source-runtime endpoint semantics/arcade importance.
- Incitti's Spinner/Spiker can emit a bullet during its growth/retreat behavior with the separate strict `Rand(0,100)>70` roll (`tempest.bmx:3638-3655`). Production Level 4 already enables the corresponding C# Spiker bullet behavior.
- Enemy bullets are separate hazards, not enemy obligations; the existing C# collision, scoring, death, and audio paths are reusable. No additional Level 5-specific enemy-bullet type is present.

Incitti's `rimit`/Hurry-Up latch occurs after reserve and Eggs are gone while enemies remain (`tempest.bmx:3811-3824`). Its only source behavior consumers are Pulsar and Fuseball (`tempest.bmx:3067`, `3393-3399`). Neither is eligible on Level 5, so no Level 5 Hurry-Up gameplay change is required. Current C# `HurryUp` is limited to the isolated Fuseball/Pulsar modes (`TempestGame.cs:1006-1015`).

Incitti resets the Super Zapper to two charges after creating each next level (`tempest.bmx:1420-1434`). The C# fresh-level reset already gives two charges and clears old flash state. Level 5 introduces no special Zapper rule.

## EXISTING REUSABLE SYSTEMS

**A. Already implemented and reusable unchanged**

- Closed 16-lane geometry rendering and directed Flipper pivots; Level 5 is closed, so it does not expose an open-tube topology gap.
- Flippers/Tankers/Spikers, two-child Tanker cargo, source-shaped type selection/caps, original/cargo identity ledger, death reconstruction, completion blockers, all-rim handling, Spike persistence, Super Zapper targeting and charges.
- Height-60 Spikes and the accepted Avoid Spikes warning, fire gate, travel, damage, collision, death, and retry behavior.
- Palette-0 colors, the accepted exit/approach presentation, score/lives carry, and fresh-wave initialization.
- Existing audio events for PlayerMove/Shot, EnemyBullet, SpikeHit, ZapResolution, kill/death, ZoomIn/Out. Level 5 adds no sound event. BonusLife/`bonus.wav` remains intentionally deferred until actual score-threshold extra-life awarding exists.
- Level 5 uses no Pulsar or Fuseball behavior and therefore needs no new pulse, Fuseball scoring, or Hurry-Up system.

## REQUIRED GENERALIZATIONS

**B. Implemented but hard-coded where a small generalization is required**

1. **Add the explicit Level 5 configuration.** Add a LevelLoader definition and record with ProgressionIndex 4, DisplayedLevel 5, BoardIndex 4, PaletteIndex 0, closed 16-lane cross geometry above, depth 400/position 50, the established player start, supply 20, four initial Eggs, source-compatible release/hatch values, `FlippersTankersAndSpikers`, `AvoidSpikes`, canflip tuning, and starting Spike height 60. No new enemy-composition enum is needed.
2. **Extend natural selection by one edge.** Add Level 4 → Level 5 to `LevelProgression`. Keep Level 5 terminal; Level 6 must remain unavailable in this milestone.
3. **Allow successful production Spike exits to commit progression.** The current `UpdateSpikeExit` explicitly comments that Spike avoidance/retry is “never a production progression commit” and sets `Complete` at the end of travel (`TempestGame.cs:1718-1725`). That assumption blocks natural Level 4 → 5 because Level 4 is itself `AvoidSpikes`. On successful safe exit only, use the same “install configured successor or become terminal” policy as the standard exit path. Preserve collision/reverse/retry, and leave the Level 5 no-successor result terminal. This is the essential non-configuration generalization.
4. **Update the Level 4 terminal assertions and docs in a later implementation task.** Existing tests/spec explicitly assert Level 4 is terminal (`LevelConfigurationTests.cs:192-220`, `LevelProgressionTests.cs:362-411`; behavior spec currently states it). Level 4 will no longer be terminal once Level 5 is installed; Level 5 will be the intentionally terminal configuration.

No broader progression-table rewrite, palette system, enemy abstraction, or level recycling is justified by Level 5.

## NEW BEHAVIOR, IF ANY

No genuinely new enemy or gameplay system is required. Level 5 is new configuration and content using accepted systems. The only required gameplay-path change is to let the already-supported AvoidSpikes exit path install a natural successor after a **successful** exit; failure/retry remains unchanged.

The Incitti Flipper-bullet discrepancy is a documented accepted policy across Levels 1–4. Do not silently enable Flipper bullets only at Level 5 or reopen earlier levels; record Neal's arcade observation as an open decision. Tanker initial timer endpoint is also a small source/runtime detail to preserve as an explicit question rather than silently tune.

## NATURAL LEVEL 4 -> 5 PROGRESSION

1. Complete all Level 4 obligations through existing normal accounting.
2. Enter the already accepted Level 4 Avoid Spikes presentation and preserve its controls, shot rules, hazards, and retry behavior.
3. Commit progression only after safe exit travel completes; install a fresh Level 5 configuration and begin the existing `LevelApproach` with `ExitedLevel=4`.
4. Carry score/lives, clear prior wave objects and stale bullets, set the player start, reset the Super Zapper to two charges/zero flash, and freeze gameplay during approach.
5. The approach starfield remains blue because it is the exit from Level 4. Level 5 gameplay uses the cross, palette 0, 20 original obligations, and 16 starting Spikes.
6. On Level 5's own successful Avoid Spikes exit, no successor exists; show terminal Level 5 completion and retain the established R-to-new-campaign behavior. Do not create or expose Level 6.

The existing incoming-level install/approach path already carries score/lives and resets wave/Zapper state. The missing link is specifically the AvoidSpikes successful-exit branch. A direct Level 5 diagnostic/test constructor can exercise the wave without changing natural progression; adding a `--level5-test` command-line option is useful for manual acceptance but is an explicit development-only access choice, not a production requirement.

## AUDIO IMPACT

No new audio event is indicated by the Level 5 source configuration. Eligible existing behaviors map to current audio events: Flipper/Tanker/Spiker destruction and zap, Tanker/Spiker enemy bullets, Spike hits, Player shots/movement, player death, and level ZoomOut/ZoomIn. PulsarPulse does not occur. BonusLife remains dormant; Level 5 must not invent score thresholds or activate `bonus.wav`.

## TEST PLAN

Reuse Level 4's extensive generalized evidence rather than duplicating every test. Minimum deterministic additions/updates:

1. **Direct configuration/access:** assert displayed/progression/board/palette identity, exact ordered 16 points, center/offset, closed topology/16 playable lanes, depth/start, 20 supply, four initial Eggs/16 reserve, 16 height-60 Spikes, AvoidSpikes and Flippers/Tankers/Spikers composition. Exercise direct `TempestGame(Level5Configuration)` access.
2. **Wave composition/eligibility:** deterministic picker cases for all three allowed types, Tanker cap and Spiker late-wave cap, with Pulsar/Fuseball impossible. Assert 20 original IDs and exactly two separate latent cargo IDs for each selected Tanker; do not assert a fixed random type quota.
3. **Accounting/death/re-entry/completion:** parameterize or reuse Level 4 ledger tests at supply 20 to prove unresolved originals/cargo are conserved through death/re-entry, Spiker retreat, pending Zaps, and completion blockers. Include active Egg, Tanker latent/pending cargo, and Spike separation. Keep score and resolved outcomes.
4. **Geometry and presentation:** exact cross rendering/projection, every lane's directed pivot, lane highlights, player shots, Spikes, and all 16 lane indices. Use Level 4 geometry tests as the base; Level 5's coordinates still need exact fixture coverage.
5. **Natural Level 4 → 5:** resolve Level 4, traverse the AvoidSpikes successful exit, and assert one Level 5 installation/approach; score/lives carry, old objects are cleared, new Level has four Eggs/16 reserve and 16 Spikes, player start is restored, and Zapper is two charges with zero flash.
6. **Presentation chronology:** exit from 4/approach into 5 is blue; exit from 5 is mixed. Existing starfield tests already exercise exitedLevel 5; add a natural handoff assertion rather than changing the accepted rule.
7. **Level 5 completion / Level 6 boundary:** successful safe Spike exit completes Level 5 terminally, `GetNextProductionLevel(Level5)` remains null, and R starts a fresh Level 1 campaign. Add a regression that does not confuse a Spike collision/retry with successful exit.
8. **Super Zapper:** existing generalized target/order, flash, and two-charge tests cover mechanics; explicitly assert fresh Level 5 recharges/clears flash and approach input cannot consume a charge.
9. **Hurry-Up/audio:** no Level 5 Hurry-Up test is necessary because neither eligible consumer is present; assert no Pulsar/Fuseball entities. Reuse AudioEvent tests to verify existing events only; no new event or bonus playback.

The current LevelProgression tests cover fresh two-charge recharge only on Level 2, and the current Level 4 tests cover the ledger, Spikes, selection, and exits. New coverage should target Level 5 config and the changed AvoidSpikes progression branch, not duplicate those entire suites.

## MANUAL ACCEPTANCE PLAN

Use a bounded two-part run, with a Level 5 direct diagnostic if added:

**Neal visual/feel judgment**

- From direct Level 5 access, inspect the closed 16-lane cross silhouette, proportions, near/far rim, all lanes, Blaster lane cue, colors, Egg positions, and starting Spikes. Move around all lanes and confirm no gaps, flicker, bad lane transition, or stale Level 4 rendering.
- Play a short wave segment with Flippers, Tankers, and Spikers, then take one death/reconstruction. Confirm the accepted Level 4 Spike persistence, retreat, Zapper, firing, and enemy presentation still feel right on the cross.
- Complete one natural Level 4 exit into Level 5; visually judge the Avoid Spikes exit, blue Level-4-exit stars, recharge message, distance/arrival, score/lives carry, and no stale outgoing board. Do not spend time revalidating the already accepted transition tuning absent a regression.
- Complete Level 5 through its Spike exit and confirm terminal Level 5 completion/no Level 6.

**Compare against arcade/MAME or Incitti**

- Compare the Original Arcade Level 5 cross outline, relative scale/orientation, and colors. Use Incitti as source evidence, not as higher authority than Neal's arcade observations.
- Compare enemy mix/pacing, canflip, Spiker/Spike behavior, Tanker speed/firing, enemy bullets, Super Zapper, and the outgoing-Level-5 mixed starfield. Resolve the known Flipper bullet policy with Neal's observation before changing it.
- Automated tests establish exact values/state transitions/accounting, not subjective silhouette, color impression, random-composition feel, audio similarity, or arcade fidelity.

## LEVEL 6 FORWARD-COMPATIBILITY CONSIDERATIONS

The Original Arcade table has 16 distinct board rows; Level 6 is the closed “triangle” at internal board 5 (`levelset1.txt:12-13`). Incitti chooses `current_board = current_level Mod 16`, advances palette by 16, and cycles the source level index after six 16-board/color groups (`tempest.bmx:1420-1427`; level setup/indexing at `1729-1732`). Level 6 would still use Spikes, Tankers, Flippers, and no Pulsar/Fuseball under the current thresholds; its base supply would be 22. These are context only, not a Level 6 implementation.

C# progression/display/board/palette identifiers are `int` and can represent additional levels. The current static configuration plus explicit successor chain is enough for Level 5 and does not require redesign. Future production levels will need their own data and explicit progression edges; do not add modulo recycling or Level 6 content now. The starfield's accepted exited-level rule is intentionally independent of palette-group cycling.

## RECOMMENDED IMPLEMENTATION SCOPE

**Recommendation: B. A configuration extension plus a bounded reusable-system generalization.**

Level 5 itself is a modest configuration addition: exact closed cross geometry, palette 0, supply 20, the same accepted Flipper/Tanker/Spiker family, and the same Spike/exit policy as Level 4. No Level 5-specific enemy, timing model, palette, audio event, or Hurry-Up mechanic is needed.

It is **not** safe as configuration-only because the natural Level 4→5 commit must pass through the production `AvoidSpikes` success path, whose current implementation is explicitly terminal. The smallest clean change is to share the successful-exit successor-or-terminal decision with that path, add the Level 5 configuration and one progression edge, and preserve Level 5 as terminal. Existing level setup, approach, accounting, Zapper, color, and audio systems should remain unchanged.

This avoids both extremes: no future-level framework just for Level 5, and no Level 6/recycling work. The accepted Flipper bullet compromise is not silently changed; it remains a documented evidence decision.

## RISKS / OPEN QUESTIONS

- **Reference set identification:** use `levelset1.txt` (“Original Arcade”) because it exactly matches accepted production Levels 1–4. Do not mistake the fifth file/set (`levelset5.txt`) for displayed Level 5; it would incorrectly replace the closed cross with an unrelated open curves board.
- **Arcade evidence:** source values are reference-verified, not automatically arcade-verified. Neal's observations take precedence.
- **Flipper bullets:** Incitti fires them, but the accepted C# policy intentionally suppresses production Flipper bullets through Level 4. Do not enable them implicitly for Level 5; obtain Neal's decision if Level 5 comparison makes it material.
- **Tanker timer endpoint:** source `16+Rand(64)` and C# `16+NextRandomInclusive(1,64)` may differ by the initial value 16. Confirm BlitzMax `Rand` endpoint semantics and whether observed behavior makes this worth correcting.
- **Level 4 terminal records:** implementation will invalidate Level 4-terminal assertions and current behavior-spec statements. Update related documentation only in the later implementation milestone, not in this analysis-only task.
- **Random composition:** source selection is stochastic and does not provide a fixed Flipper/Tanker/Spiker count. Test the eligibility, caps, and accounting deterministically; do not encode invented quotas.
- **Manual entrypoint:** decide whether a Level 5-only diagnostic flag is worth adding for bounded visual acceptance. It must remain development-only and not affect natural campaign selection.

TEMPEST_LEVEL5_PREIMPLEMENTATION_ANALYSIS_COMPLETE
