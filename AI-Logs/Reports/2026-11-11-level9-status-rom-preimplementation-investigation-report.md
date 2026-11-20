# Level 9 status and ROM preimplementation investigation

Date: 2026-11-11

Scope: Read-only Level 9 status and preimplementation investigation. Level 9
was not implemented. Levels 1-8 and all protected gameplay/reference material
remain unchanged.

## 1. Starting Git checkpoint and cleanliness

- Branch/status: `## master...origin/master`.
- `HEAD` and `origin/master` both equal
  `037b3f9877f38d0182a3eba4fb8a8f0279e4f1a9`
  (`Complete accepted Level 7 behavior`).
- `git diff --stat` and `git diff --cached --stat` were empty.
- No tracked or untracked worktree changes were present at the start.
- The expected checkpoint and cleanliness matched; investigation proceeded.

## 2. Current Level 9 implementation-status determination

**A. LEVEL 9 NOT IMPLEMENTED**

Current production configuration ends at `LevelConfiguration.Level8Production`.
`LevelProgression` links Level 7 to Level 8 and returns no next production
level after Level 8. `--level N` validation uses that same production chain,
so Level 9 is not a valid direct start. Recording startup resolves through the
same production-level validation. There is no Level 9 wave selector,
configuration, progression entry, production test, or implementation.

The test that constructs a synthetic configuration with `DisplayedLevel = 9`
does not register or implement Level 9. Durable behavior documentation states
that Level 8 is terminal and that Level 9 is not loaded. This agrees with the
current code; there is no stale implementation premise to reconcile.

## 3. Historical Level 9 reports/files found

- [2026-11-06 Level-Specific Flipper / Spike ROM Analysis](2026-11-06-level-specific-flipper-spike-rom-analysis-report.md)
  is the prior Level 9 ROM report. It is analysis only, not current
  implementation-status authority.
- No Level 9 implementation or acceptance closeout report was found.
- Level 8 implementation and manual acceptance are present in the starting
  checkpoint. [The Level 8 reconciliation report](2026-11-11-level8-preimplementation-reconciliation-investigation-report.md)
  explicitly records Level 9 as outside that work.

## 4. Evidence sources and authority

1. The supplied task's Level 9-specific `$53` conclusions were treated as
   prior evidence and checked against the local listing.
2. Primary source: the project-local
   [Atari Tempest Rev. 2 ROM listing](../../Reference/arcade/Tempest%20Rom%20Source%20code.md),
   including level setup/parameter evaluation, Flipper p-code, selection,
   cargo, firing, death reconstruction, and level progression.
3. Existing binary-backed evidence: [the Level 8 Rev. 2 geometry continuation](2026-11-08-level8-rev2-rom-geometry-continuation-report.md)
   records the K1 ROM address mapping and complete `lev_remap`/presentation
   tables, but contains raw geometry only for shape row 7.
4. Existing project decision and translation evidence:
   [TEMPEST-BEHAVIOR-SPEC.md](../../TEMPEST-BEHAVIOR-SPEC.md) and
   [TEMPEST-DEVELOPMENT-NOTES.md](../../TEMPEST-DEVELOPMENT-NOTES.md).
5. Current C# was inspected as architecture and behavior only, not as evidence
   for Level 9 arcade behavior. Incitti's “Steps” name is secondary evidence
   only; its geometry was not needed once the local Level 9 ROM row was read.

The Rev. 2 continuation report's established `$B800` CPU-to-file mapping
showed that the documented binary-backed extraction did **not** contain the
Level 9 row. Because exact geometry is essential to the readiness decision
and the repository instruction permits bounded ROM verification when
genuinely required, one narrow read was made from
`Reference/arcade/roms.rev2/136002-118.k1`: the 16 bytes each for shape-row-13
X, Y, and angle, plus seven shape-row-13 presentation bytes at the documented
table offsets. No other ROM binary was opened. The ROM directory was not
enumerated, and no ROM binary was hashed or modified. The existing remap and
color-table evidence was reused.

Evidence labels below distinguish **REV-2-BINARY-CONFIRMED**,
**ROM-LISTING-BACKED**, **SECONDARY REFERENCE**, **PROJECT TRANSLATION**,
**PROJECT APPROXIMATION**, and **MANUAL ACCEPTANCE NEEDED**.

## 5. Level identity and parameter indexing

- Displayed Level 9: **9**.
- ROM `p1_level`: **`$08`** (zero based).
- Effective parameter-table level: **`$09`** (`curlevel + 1`).
- Existing Rev. 2 `lev_remap` table gives `lev_remap[8] = $0D`.
- Therefore the tube/geometry row is **shape row 13**, not row 8.
- Incitti calls the secondary-reference board “Steps.” That name is not
  required to establish the binary-backed shape and is not Atari authority.

The relevant selector and parameter-table execution is at `$92C5-$9326` and
`get_tube_no` at `$C2E8` in the local listing.

## 6. Geometry and topology

**REV-2-BINARY-CONFIRMED** (bounded row read; K1 CPU base `$B800`):

| Table | File offset | CPU address | Shape-13 data |
|---|---:|---:|---|
| `lev_x` | `$024C` | `$BA4C` | `10 10 30 30 50 50 70 70 90 90 B0 B0 D0 D0 F0 F0` |
| `lev_y` | `$034C` | `$BB4C` | `90 70 70 50 50 30 30 10 10 30 30 50 50 70 70 90` |
| `lev_angle` | `$044C` | `$BC4C` | `0C 00 0C 00 0C 00 0C 00 04 00 04 00 04 00 04 00` |

With the previously extracted remap table, shape 13 is open:
`lev_open[13] = $FF`. It has 16 ordered vertices and **15 playable lanes**.
Lane `i` is edge `Pi -> P(i+1)`, `i=0..14`; there is no lane 15 and no
lane-14-to-lane-0 closure.

The path is a stepped open polyline. In lane order, its segment directions are
up, right, up, right, up, right, up, right, down, right, down, right, down,
right, down in the raw coordinate convention. The 16 angle entries encode
vertical/horizontal directions as shown in the table; slot 15 is the terminal
vertex and has no outgoing playable lane. Every interior turn is an
orthogonal turn. In the raw X-right/Y-increasing coordinate convention, the
turn signs alternate through the first staircase, turn right again at the
center transition from lane 7 to lane 8, then alternate through the second
staircase. A Y reflection in the C# screen-down translation reverses those
signed turns; it does not change vertex or lane order.

The ROM's open-end behavior is separate from the p-code:

- Player rim movement clamps at the open endpoints.
- `rev_if_edge` reverses enemy direction at an open endpoint when a flip starts.
- Tanker cargo allocation corrects an invalid open-end neighbor instead of
  creating a child on the nonexistent closing lane.
- Tube rendering omits the closing edge.

These ROM behaviors are handled by the open-topology seams exercised for
accepted Level 8. Level 9 must use them with its own 16 points and 15 lanes.

## 7. Palette and presentation evidence

**REV-2-BINARY-CONFIRMED / ROM-LISTING-BACKED:**

| Shape-13 field | Raw value | Meaning |
|---|---:|---|
| `lev_scale` | `$0C` | ROM reciprocal scale input |
| `lev_y3d` | `$B0` | ROM 3-D camera-height field |
| `lev_y2db:lev_y2d` | `$01:$40` = `$0140` | Signed ROM 2-D Y-offset value, +320 |
| `lev_open` | `$FF` | Open tube |
| `lev_fscale2:lev_fscale` | `$04:$50` = `$0250` | Packed Flipper reciprocal scale |

`curtube=$0D`; the ROM's tube-color lookup indexes `curtube & 7`, so entry 5
selects raw tube-color index `$05`. The Level 1-16 color block reused for
Level 9 is `00 04 08 0C C3 07 0B 0B`. These raw values do not specify C# pixels
or RGB values.

**PROJECT TRANSLATION:** The accepted Level 8 coordinate transform is a
candidate seam: subtract `$80` from raw X/Y, scale by `25/14` (the row's
maximum centered coordinate is 112), and apply the accepted Y reflection for
the screen-down projection. Preserve source point order and set the
definition open. `lev_scale`, Y3D, Y2D, and Flipper scale must remain recorded
as ROM evidence; none is a literal C# `CenterY`, `YOffset`, or pixel-scale
value. An initial project palette mapping and screen placement require Neal's
acceptance.

## 8. Level 9 Medium/default parameter table

The parameter loader indexes the tables by effective level `$09`.
**ROM-LISTING-BACKED**, with table records also confirmed by the prior Rev. 2
binary-backed Level 8 extraction where the same records apply:

| Parameter | Level 9 value | Meaning |
|---|---:|---|
| `wave_enemies` | `$1B` = 27 | Generic pending supply |
| `wave_spikeht` | `$C0` | Raw initial Spike along-position |
| `max_enm` | `$06` | Six-unit generic active-entry gate |
| Flipper minimum / maximum | 0 / 5 | No ordinary bottom Flipper; maximum reserves cargo capacity |
| Tanker minimum / maximum | 1 / 2 | Ordinary selection bounds |
| Spiker minimum / maximum | 2 / 4 | Ordinary selection bounds |
| Pulsar minimum / maximum | 0 / 0 | Not ordinarily eligible |
| Fuzzball minimum / maximum | 0 / 0 | Not ordinarily eligible |
| `tanker_load[0..3]` | 1, 1, 1, 1 | Type-1 / Flipper cargo; two children per Tanker |
| `flipper_move` | `$53` | Level-selected Flipper movement entry |
| `shot_holdoff` | 56 | ROM parameter ticks (`80 - 3*(9-1)`), not C# simulation ticks |
| `enm_shotmax` | 3 | Highest slot index; four simultaneous ROM enemy-shot slots |
| Flipper speed input | `$AF` | Raw ROM input to `crack_speed` |
| Tanker speed | Flipper speed | Setup copies the converted Flipper speed fields |
| Spiker speed input | `$C0` | Raw ROM input to `crack_speed` |
| Enemy projectile speed input | `$C0` | Raw ROM input to `crack_speed` |
| `flip_top_accel` | 2 | ROM level-table value |
| `pulsar_fire` | 0 | No Level 9 Pulsar eligibility |

The ordinary minimum/maximum selector uses nonzero minima as eligibility.
Therefore ordinary bottom Flippers are excluded despite the Flipper maximum
of five; that maximum is relevant to Tanker cargo accounting. The target is
Medium/default only. No general difficulty support is proposed.

## 9. Exact `$53` Flipper/Spike trace

The prior Level 9 report's trace is supported by the local p-code executor at
`$9B98-$9C3A`, the `$53` entry at `$A14A-$A157`, and Flipper table value
`$53` for effective level 9.

1. `$53` first yields, then `$54` executes the ordinary along-lane move.
2. `$55` (`$1C`) indexes `spike_ht` using the Flipper's **current**
   `enemy_seg`.
3. A zero/missing Spike entry is treated as `$FF`; otherwise the stored live
   Spike along-position is compared with `enemy_along`.
4. The predicate is true when `spike_ht >= enemy_along`, including equality.
   The listing describes this as the enemy being above the Spike tip.
5. If false, `$56` branches to the program halt. On the next update the
   Flipper moves farther along the **same lane** and tests again. It does not
   pause or start a lane change.
6. If true, `$58` starts a normal flip. `$5A/$5B` continue the flip while
   also moving along the tube. The lane is not retested during the pivot.
7. Once the flip completes, the route returns to its halt; on the next update
   it moves along the new current lane and then retests that lane's live Spike.
8. A destroyed/missing Spike allows the predicate through the `$FF` substitute.
   A shortened Spike changes the threshold because the stored live height is
   read on each test.
9. The predicate is not latched. `$53` contains no random-selection opcode
   and no `$16` direction-reversal opcode. Common open-end reversal is
   performed by the separate `rev_if_edge` handler when a flip starts.
10. The common enemy-firing routine can fire an eligible Flipper outside an
    active pivot; it rejects the active-flip flag. `$53` does not itself
    enable or disable firing.

This is a repeated **conditional lane-stay** effect, not a separate captured
or locked state. It advances while below the current live Spike tip and
permits a flip at equality or after passing the tip.

## 10. Human-observable `$53` behavior

At normal speed, a Level 9 `$53` Flipper climbs its current lane while the
live Spike tip is still ahead. It does not turn below that tip. At the tip,
including an exact tie, it can begin its ordinary lane-change animation while
continuing to climb. After that turn completes, it advances on the new lane
and checks that lane's Spike. If that Spike was destroyed or shortened, the
lane-stay interval is absent or shorter. The route itself does not randomly
choose whether to stay.

For frame-step acceptance, observe one Flipper on a known lane and record
frames at below-tip, equality, and above-tip positions; repeat after shortening
and destroying that lane's Spike. Confirm that the next lane is not consulted
mid-pivot and is checked only after the completed flip and subsequent along
move.

## 11. Ordinary Flippers versus Tanker cargo

- **Ordinary bottom Flippers:** not selectable on Level 9 because
  `min_flippers=0` and each ordinary selection path requires a nonzero species
  minimum.
- **Natural/near-rim Tanker cargo:** Tankers carry two Flippers. Natural
  release occurs in the ROM near-rim range below `$20`; cargo selects default
  entry `$07`, which moves straight up the lane and does not run `$53`.
- **Deep player-shot Tanker cargo:** shot destruction passes the Tanker's
  current ROM along coordinate to the common cargo setup. At or beyond `$20`,
  including equality, the level-selected entry `$53` is used. Each child then
  reads the live Spike on its own current lane after moving.

Thus Level 9's ordinary Flippers are absent, while deep shot-released cargo
can exhibit the Spike-sensitive route. Do not apply `$53` to all Tanker cargo.

## 12. `$07` / `$53` cargo boundary

The unsigned branch at `$9B07-$9B1D` compares current Tanker `enemy_along`
against `$20`; `BCS` selects normal type setup at `$9B18` for values
`>= $20`, including equality. Values `< $20` use the type's default movement
entry `$07`.

The natural Tanker path releases on a qualifying post-movement value in
`$11..$1F`; `$10` is already handled by top-of-tube logic. Natural release
therefore uses `$07`. Player-shot destruction retains the current along
coordinate, so a deep shot at `$20` selects `$53`, while a shallower shot
below `$20` selects `$07`.

After `$53` cargo materializes, its Spike predicate is live and lane-specific,
identical to ordinary Level 9 `$53` movement. The two children occupy adjacent
lanes with opposite initial direction bits, subject to the open-end correction.

## 13. Starting Spikes and proposed translation

`set_enm_and_spikes` (`$923A-$9245`) copies `wave_spikeht=$C0` into all 16
ROM Spike slots. Shape 13 is open, so the C# lane model has 15 playable lanes
(0-14); slot 15 is the non-playable open-end slot. All 15 playable lanes
therefore start with a Spike.

The accepted Level 8 affine translation maps raw `$F0` to C# height 0 and
`$10` to `Level.Depth=400`:

```text
height = (0xF0 - rawAlong) * 400 / (0xF0 - 0x10)
Level 9: (240 - 192) * 400 / 224 = 85.7142857
```

**PROJECT TRANSLATION candidate:** initialize height about **85.7143** on
each playable C# lane. This is not a literal ROM coordinate or an arcade-
accepted Level 9 height. Do not reuse Level 8's numerical height, or Levels
4-7's current C# height 60. The existing `Tanker.HeightForRomAlongPosition`
and `Level.StartingSpikeHeight` seams can represent the affine candidate.
Spike presentation remains subject to manual Level 9 comparison.

## 14. Enemy selection and concurrency

**ROM-LISTING-BACKED:**

- Ordinary eligible species are Tankers and Spikers. Their minima/maxima are
  Tanker 1/2 and Spiker 2/4. Flipper minimum 0 excludes ordinary Flippers;
  Pulsars and Fuzzballs have zero minima/maxima.
- Generic enemy entry uses the six-unit `max_enm` gate. Tanker cargo uses a
  separate creation path and can increase active units beyond that generic
  entry gate.
- At wave start, the nonzero-minimum admission pass checks Spikers before
  Tankers: two Spikers satisfy the Spiker minimum, then one Tanker satisfies
  the Tanker minimum, assuming those types remain available.
- Once minima are met, when both Tanker and Spiker are available the ROM
  consults a selected lane's current Spike height against `$CC`; the
  subsequent fallback scans eligible types from a randomized starting point.
  Thus Spike state can influence selection; the exact post-minimum path is
  not equivalent to a uniform random Tanker/Spiker choice.
- Each active Tanker reserves two Flipper-type units against the maximum of
  five. Releasing cargo does not consume generic pending supply.
- At zero generic pending supply, the Spiker p-code can convert a final
  Spiker in place to a Tanker with two latent Flippers. It adds no new generic
  unit. This special conversion may recur after death reconstruction.

**PROJECT TRANSLATION boundary:** `Level8WaveSelection` already models the
same species minima/maxima, six-unit gate, and latent cargo reservation, but
its post-minimum seeded selection deliberately does not reproduce the ROM's
Spike-height/random-start policy. Level 9 can reuse verified limits without
claiming the Level 8 picker is literal Level 9 ROM behavior. Keep any
Level 9 integration separately opt-in and leave Level 8 unchanged.

## 15. Tanker cargo accounting and lifecycle

The four `tanker_load` entries resolve to type 1 (Flipper), which is a pair
per Tanker. The carrier consumes one generic pending unit; its two latent
cargo identities are not additional generic supply. On release the carrier
slot is retired and two child attempts are made on opposite adjacent lanes
with opposite direction bits. Open endpoints receive the ROM's endpoint
correction; cargo remains outside the ordinary six-unit entry gate.

On death, latent cargo disappears with its carrier. Released cargo is an
active obligation and is included in generic reconstruction. Final-pending
Spiker-to-Tanker conversion adds only latent cargo, not pending supply.

The closest current C# accounting seam is `Level3WaveLedger` plus the
Level 8 generic reconstruction policy. `Tanker.ReleaseFlipperCargo` already
handles open-tube child lanes, while Level 8's route dispatch distinguishes
natural `$07` from deep shot `$19`. Level 9 requires its own deep `$53`
dispatch, without changing either accepted Level 7 or Level 8 cargo policy.

## 16. Tanker, Spiker, Flipper, and projectile speed evidence

**ROM-LISTING-BACKED raw inputs:**

- Flipper: `$AF` from the effective-level table.
- Tanker: receives the same converted speed fields as the Flipper.
- Spiker: `$C0` input to `crack_speed`.
- Enemy projectile: `$C0` input to `crack_speed`.
- `flip_top_accel`: 2.

The Flipper/Tanker input differs from Spiker/projectile input. The Spiker and
projectile share the same raw input, but no C# units or visible
frames-per-distance ratio follows from these raw bytes. Do not transfer
Level 7's manually accepted Tanker/Spiker relationship to Level 9, and do not
invent a ROM-to-C# speed conversion.

If perceptual speed calibration is authorized later, Neal should use a
recording/frame-step comparison in MAME or the arcade at matched game speed:
measure Flipper and Tanker traversal over the same lane distance, Spiker
traversal over a fixed height interval, and enemy-projectile travel over a
fixed depth interval. Record frame counts separately; do not infer equality
from the shared `$C0` input.

## 17. Enemy firing

The common ROM firing pass applies to eligible Flippers, Tankers, and Spikers.
Pulsars and Fuzzballs are absent under Level 9's zero bounds. The common
routine requires an active shooter flag, raw along-position at least `$30`,
an expired per-enemy delay, and no active Flipper-pivot flag; it then checks
the chance threshold indexed by current shot count and a free shot slot.
Successful fire reloads `shot_holdoff=56`. Chance bytes are
`00 E0 F0 FA FF`; `enm_shotmax=3` addresses four slots. Spikers can fire
repeatedly when eligible and their reloaded delay expires; the route is not
limited to one shot for the Spiker's entire lifecycle.

The `$53` route does not itself modify firing eligibility. A Flipper in an
active pivot is skipped by the common firing pass; otherwise it can fire if
the shared position, delay, chance, and capacity checks pass. The ROM creates
shots after the current pass's `move_shots`, so a new shot first moves on a
later pass.

Current C# has shared Flipper firing, Tanker-created `EnemyBullet`s, a
Level-7-specific repeated Spiker firing path, and a simpler Spiker
growth/retreat shot path used elsewhere. `EnemyBullet` is a deterministic
shared entity but its 3-depth-unit step is not a ROM conversion. Reuse shared
shot capacity/chance and projectile accounting only as project policy; Level
9's 56-tick holdoff and repeated Spiker firing need explicit Level 9
configuration/policy. Do not alter accepted Level 7 firing.

## 18. Death and reconstruction

The ROM's death reconstruction adds active-below-rim (`enemies_in`),
at-rim (`enemies_top`), and generic pending counts, capped at 63, then
rebuilds generic pending entrants. Species and movement-program identity are
discarded. Latent Tanker cargo is not included and is lost with its Tanker;
released Flippers are active units and return as generic obligations.
Destroyed and rim-resolved outcomes remain resolved. Spikes are level state,
not enemy identities, and are not recreated from the original wave table on
ordinary death.

No `$53` program counter is resumed. A reselected Flipper starts from the
Level 9 level-selected entry `$53`, then tests its current lane's live Spike.
The same applies to a newly selected deep cargo Flipper after reconstruction.
Final-pending Spiker-to-Tanker conversion remains possible when the generic
pending pool is exhausted.

The closest C# model is `Level3WaveLedger.RebuildAfterDeath` with generic ROM
reconstruction enabled: clear unresolved species selection, drop latent
cargo, turn released unresolved cargo into generic obligations, preserve
terminal outcomes, and create fresh Eggs/enemy objects. A Level 9 config must
opt into this explicitly; Levels 1-8 behavior must remain unchanged.

## 19. Progression, direct startup, and completion boundary

- ROM natural progression increments zero-based level 7 to 8, i.e. displayed
  Level 8 to Level 9. The existing C# natural `LevelProgression` chain needs
  a Level 8-to-9 link; the established approach, score/lives carry, and fresh
  Super Zapper setup can be reused.
- The C# project start lane 7 is a project choice, not established here as a
  Level 9 arcade start lane. Reuse it provisionally only as a **PROJECT
  TRANSLATION**, with manual acceptance.
- Generic `--level N`, `GameplayStartupConfiguration`, recording v2, visible
  playback, and headless playback resolve through `LevelProgression`. A
  registered Level 9 can use these seams without a recording-format change.
- The arcade naturally continues from Level 9 to Level 10. Level 10 is out
  of scope and unsupported in this bounded milestone. Level 9 should
  initially be the final supported production level; completion/restart
  should follow the current terminal policy and begin a fresh Level 1
  campaign. This is a **PROJECT DECISION**, not arcade progression.

## 20. Accepted Levels 1-8 regression constraints

Levels 1-8 are accepted and must remain unchanged. Specifically:

- Do not alter Level 5 merely because it also selects ROM `$53`.
- Do not replace Level 7's accepted route, speed, or firing behavior.
- Do not alter Level 8's accepted `$19` deep-cargo behavior, open geometry,
  selection policy, Spike presentation, or terminal boundary.
- Do not generalize Spike-sensitive Flipper movement to levels that do not
  select `$53`.
- Keep Level 9 behavior behind its own configuration/production opt-in and
  add regressions covering the existing levels.

## 21. Current C# architecture reuse map

| Level 9 mechanic | Reusable C# component | Level 9-specific extension | Evidence / regression risk |
|---|---|---|---|
| Shape and lane topology | `LevelDefinition`, `LevelLoader`, `Level`, `DirectedLaneTransition` | Add row-13 data; retain 16 vertices/15 lanes and open endpoints | Raw row is binary-confirmed; project coordinate/camera translation needs acceptance. Do not disturb Level 8 topology. |
| Tube/palette presentation | `Projection`, `VectorRenderer`, `PaletteIndex` | Keep ROM `$05`, `$0C`, `$B0`, `$0140`, `$0250` distinct from C# values | ROM values do not define RGB or C# pixels. |
| Wave setup/configuration | `LevelConfiguration`, `Level` | Supply 27, starting Spike translation, shot holdoff/capacity, Level 9 movement policy | Parameters are ROM-backed; C# Eggs/speed/tick choices are translations. |
| Selection/concurrency | `Level8WaveSelection`, `Level3WaveLedger`, `TempestGame` | Level 9-specific integration and optional Spike-sensitive post-minimum translation | Limits match Level 8, but exact selector consults Spike state and ROM random start. Avoid changing Level 8. |
| `$53` Flipper | `Flipper`, `BelowRimFlipperProgram`, `DirectedLaneTransition` | A Level 9-specific `$53` opt-in with post-move live-Spike checks and movement during turns | Exact route ROM-backed; do not silently reuse `CanFlip` or alter Level 5. |
| Tanker cargo | `Tanker`, `Flipper.CreateTankerCargo`, `TempestGame.ReleaseTankerCargo` | Natural/shallow `$07`, deep-shot `$53`, current-lane Spike checks | `$20` branch and pair ROM-backed; preserve Level 7/8 policies. |
| Starting Spikes | `Tanker.HeightForRomAlongPosition`, `Level.StartingSpikeHeight` | `$C0` -> candidate 85.7143 on lanes 0-14 | Affine conversion is a project translation; slot 15 is non-playable. |
| Spiker and conversion | `Spiker`, `Level3WaveLedger`, `TempestGame.UpdateSpikers` | Level 9 firing/terminal conversion gating; preserve current Spiker implementation policy | ROM selection/conversion is traced; C# movement and conversion timing are approximations. |
| Enemy firing/projectiles | `TempestGame` firing phases, `EnemyBullet` | Level 9 56-tick setting, four-shot cap, repeated Spiker policy | Chance table is source-backed; C# tick/speed conversion remains project policy. |
| Death reconstruction | `Level3WaveLedger.RebuildAfterDeath`, `Level.ResetAfterDeath` | Enable generic reconstruction for the canonical Level 9 config | Generic ROM counts are source-backed; protect Levels 1-8 accounting. |
| Progression/direct startup | `LevelProgression`, `TempestGame.InitializeFreshWave`, `DebugController` | Add 8->9, `--level 9`, terminal Level 9 handling | Campaign boundary is a project decision; Level 10 remains unsupported. |
| Recording/playback | `GameplayStartupConfiguration`, `GameplayRecordingFile`, visible/headless playback | Add tests resolving Level 9 metadata | Existing version-2 startup-level field is generic; no schema change expected. |

## 22. Explicit implementation-readiness decision

**READY FOR BOUNDED LEVEL 9 IMPLEMENTATION**

Geometry/topology, Medium/default composition, exact `$53` behavior, Spike
initialization and a defensible translation candidate, cargo paths,
concurrency, death reconstruction, and progression are sufficiently
established. Unknown C# screen/RGB/speed mappings are explicit project
translation and manual-acceptance work, not missing ROM facts. This report
does not authorize implementation.

## 23. Bounded implementation specification (candidate only)

Do not implement in this task. A separately authorized Level 9 milestone
should:

- Add a Level 9 loader/configuration using shape row 13, all 16 vertices in
  ROM order, 15 playable lanes, no closing chord, and the open endpoint rules.
  Keep the raw coordinate, angle, scale, Y3D/Y2D, Flipper-scale, remap, and
  palette evidence available for tests/docs.
- Use the accepted coordinate normalization/Y-reflection as a **PROJECT
  TRANSLATION** candidate, retaining any camera/vertical placement and RGB
  mapping as explicit project presentation choices pending Neal's acceptance.
- Configure Medium/default supply 27, four initial C# Eggs as an existing
  project wave-setup choice, 15 starting Spikes at candidate height 85.7143,
  and only Spikers/Tankers for ordinary selection. Use Spiker 2/4,
  Tanker 1/2, Flipper 0/5, and zero Pulsar/Fuzzball limits.
- Retain the six-unit generic entry gate and two latent Flipper identities per
  Tanker; do not charge cargo against generic pending supply. Preserve the
  ROM's cargo accounting against the Flipper maximum.
- Keep ordinary bottom Flippers ineligible. Route natural/near-rim and
  shallow shot cargo through `$07`; route shot cargo at or beyond ROM `$20`
  through the Level 9 `$53` policy. Children use opposing adjacent lanes and
  directions, respecting open endpoints.
- Implement `$53` as a Level 9-specific opt-in: move, test the current lane's
  live Spike (`spike >= along`, equality true; missing/destroyed treated as
  `$FF`), continue same-lane movement while false, move during the active
  flip, and retest the new lane only after flip completion and another
  along-move. Do not use a latched capture state or random movement branch.
- Reuse the common Spiker architecture and final-pending same-unit
  Spiker-to-Tanker accounting, while marking differences between ROM
  behavior and the current C# Spiker movement/timing as project translation.
- Configure holdoff 56 and four simultaneous enemy shots. Reuse shared
  projectile accounting/chance thresholds only as explicit C# project
  behavior; add a Level 9 repeated Spiker firing policy without altering
  Level 7.
- Opt Level 9 into generic death reconstruction: rebuild active-below-rim,
  at-rim, and pending obligations as generic fresh entries; discard latent
  cargo with a lost Tanker; re-enter released cargo as generic obligations;
  preserve resolved outcomes; restart new Flippers at `$53`.
- Link Level 8 naturally to Level 9; carry score/lives, use the existing
  approach and Super Zapper reset, and provisionally start the Blaster on
  lane 7 as a project choice.
- Support direct `--level 9`, deterministic recordings, visible/headless
  playback, and natural progression using the existing generic startup
  metadata.
- Make Level 9 the initial terminal supported level. On completion/restart,
  use the existing fresh-campaign Level 1 policy; do not implement Level 10.
- Add the automated tests below, then stop for Neal's manual acceptance.

## 24. If NOT READY: blockers and next evidence

Not applicable. The readiness decision is READY; the evidence boundaries and
required manual observations are recorded in Sections 7, 16, and 26.

## 25. Proposed automated implementation tests

1. Assert `lev_remap[8]=$0D`, the exact X/Y/angle arrays, raw presentation
   bytes, open flag, 16 vertices, 15 lanes, endpoint order, and no closing
   edge. Verify the chosen C# coordinate transform without conflating it
   with literal ROM units.
2. Assert Level 9 Medium/default configuration values, 27 pending units,
   15 starting Spike lanes, candidate `$C0` height mapping, and that slot 15
   is not a playable lane.
3. Assert ordinary Flipper ineligibility; Tanker/Spiker minima and maxima;
   initial minimum admission; six-unit generic gate; cargo reservation
   against Flipper maximum; and cargo release above the generic gate.
4. Test `$53` with live Spike states below, equal, and above the Flipper's
   along position; absent/destroyed and shortened Spikes; movement while
   staying; movement during an active turn; and new-lane retest only after
   turn completion and the next along move.
5. Test open endpoint direction reversal and ensure no `$53`, player, Spike,
   or Tanker child can wrap to lane 15 or lane 0.
6. Test natural/shallow cargo `$07` and shot cargo at raw `$1F`, `$20`, `$21`
   selecting `$07`, `$53`, `$53`; test two child identities, opposing
   directions, and current-lane Spike behavior for deep children.
7. Test repeated Spiker-to-Tanker conversion at exhaustion without adding
   generic supply; verify two latent cargo units and the generic gate.
8. Test generic death reconstruction for active-below-rim, at-rim, pending,
   latent cargo, released cargo, and resolved outcomes; verify re-entry
   creates fresh Level 9 `$53` Flippers rather than restoring movement state.
9. Test ROM-derived firing metadata (56 holdoff parameter, four-shot
   capacity, chance-table thresholds), C# active-pivot firing suppression,
   repeated Spiker firing, and deterministic next-tick projectile movement.
   Treat C# time/speed behavior as project policy, not ROM conversion.
10. Test natural Level 8->9 progression, score/lives carry, approach, fresh
    Super Zapper, direct `--level 9`, terminal Level 9 restart to Level 1,
    and rejection/no implementation of Level 10.
11. Test recording-v2, visible playback, and headless playback starting at
    Level 9, plus deterministic replay.
12. Run targeted regression coverage proving accepted Levels 1-8 remain
    unchanged.

## 26. Proposed manual arcade/MAME acceptance checklist

After an authorized implementation and automated validation:

- Start Level 9 and compare the stepped tube's orientation, 16 vertices,
  15 playable lanes, and both open endpoints. Verify no false closing line
  and no traversal across the gap.
- Compare starting Spike presentation on each playable lane. Confirm that
  the C# height is a project translation, not a claimed raw-ROM pixel height.
- Frame-step an ordinary Tanker cargo case and verify near-rim/natural
  children climb straight (`$07`). Shoot a Tanker at known shallow and deep
  positions; confirm the `$20` equality boundary and deep `$53` route.
- For `$53`, record the current lane and Spike state. Compare below-tip,
  equality, above-tip, shortened-tip, and destroyed-tip behavior. Verify
  same-lane movement below the tip, continuing movement during a flip, and
  post-flip retest on the new lane only after the next along move.
- Verify ordinary Flippers do not enter from the bottom. Observe final
  Spiker-to-Tanker conversion and its two cargo units near wave exhaustion.
- Compare Tanker, Spiker, Flipper, and enemy-shot travel at matched game
  speed using fixed distances and frame counts; do not infer speed equality
  from raw input bytes.
- Lose a life with a Tanker carrying latent cargo and with released cargo
  active. Confirm generic reconstruction and fresh `$53` movement.
- Verify natural Level 8->9 approach, score/lives carry, Super Zapper
  recharge, and terminal Level 9 completion/restart behavior. The exact
  Level 8->9 handoff has not yet been manually accepted.
- Verify Level 9 recording and visible/headless replay remain deterministic.

Task-appropriate launch command for Neal after that future implementation:

```text
dotnet run --project src\Tempest.Game -- --level 9 --lives 10 --spinner-sensitivity 1
```

No game was launched during this investigation.

## 27. Files changed by this investigation

- Added the verbatim user task at
  [the paired prompt record](../Prompts/2026-11-11-level9-status-rom-preimplementation-investigation-prompt.md).
- Added this report.
- No other file was created or changed.

## 28. Final Git status

Final Git status was rechecked after saving this report. Branch remains
`master` at `037b3f9877f38d0182a3eba4fb8a8f0279e4f1a9`, tracking the matching
`origin/master`. Tracked worktree, index, and visible untracked state remain
clean; both diff-stat checks are empty. The prompt and report are ignored
`AI-Logs/` records.

## 29. Production/reference protection confirmation

No production code, tests, durable project documentation, references,
recordings, screenshots, user material, or ROM files were modified. The only
ROM access was the bounded read of the specified Level 9 K1 geometry and
presentation bytes described in Section 4. No build or test run was needed
for this static/read-only investigation. No game was launched.

## 30. Commit/push confirmation

No files were staged. No commit or push occurred.
