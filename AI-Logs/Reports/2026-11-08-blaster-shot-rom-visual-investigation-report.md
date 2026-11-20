# Tempest Blaster-shot ROM and visual investigation

## Scope and starting state

Read-only investigation against expected HEAD `4ccb3cfb6cf3a1e72ac47bcfc79454ce257cb9de`.
Starting `git status --short --branch` was `## master...origin/master` plus only
the 14 pre-existing untracked root files `screenshot000.png` through
`screenshot013.png`; there were no staged or tracked changes. `AI-Logs/` is
ignored by `.gitignore`, so this report and its paired prompt are ignored
project records.

No production code, tests, durable project documentation, screenshots,
recordings, configuration, or Git state were changed. No build, test suite, or
interactive game was run.

## Sources inspected

- ROM authority: `Reference/arcade/Tempest Rom Source code.md`, including RAM
  maps, shot create/update/collision code, display-list composition,
  `draw_shots`, `graphic_at_mid`, Mathbox projection/scale code, graphic table,
  and vector ROM data.
- C# production: `src/Tempest.Game/Systems/InputSystem.cs`,
  `src/Tempest.Game/Program.cs`, `src/Tempest.Game/Gameplay/Shot.cs`,
  `src/Tempest.Game/Gameplay/Player.cs`, `src/Tempest.Game/TempestGame.cs`,
  `src/Tempest.Game/Rendering/VectorRenderer.cs`,
  `src/Tempest.Game/Rendering/Projection.cs`, and
  `src/Tempest.Game/Debug/GameplayRecordingFile.cs`.
- Tests: `tests/Tempest.Game.Tests/PlayerShotRenderingTests.cs`,
  `GameplayInputTests.cs`, `SimulationTests.cs`, `GameplayPlaybackTests.cs`,
  `GameplayRecordingTests.cs`, `HeadlessGameplayPlaybackTests.cs`, and
  `EnemyDestructionEffectTests.cs`.
- Durable references: `TEMPEST-DEVELOPMENT-NOTES.md` (confirmed projectile
  behavior and accepted 4.5 size decision) and `TEMPEST-BEHAVIOR-SPEC.md`
  (type-0 destruction effect and the limited ROM-color-0 translation).
- All 41 readable MAME captures, 480x640, in
  `Videos/FramesBlasterShots/`, ordered from
  `Tempest-MAME-frame_2253.png` through `Tempest-MAME-frame_2293.png`.
  Ordered temporary contact sheets were used; source images were only read.

## Current C# player-shot lifecycle and rendering

- `InputSystem.ReadPlayer` accepts held left/right Ctrl or left mouse button.
  `Program` targets 60 FPS, calls `RunOneStep` once per main-loop iteration,
  records that input tick when recording, and renders afterward.
- `TempestGame.UpdatePlayerFiring` creates a shot before player movement, using
  the player's current lane and depth. The project limit is 10 active shots
  and the ordinary cooldown is 3 simulation ticks. `Shot.Update` reduces
  `Height` by 6 and advances cosmetic `Rotation` by 15 degrees each tick.
- `UpdatePlayerShots` advances shots, checks the active collision path, then
  removes them on a hit or when `Height < 0`. Death and ordinary exit clear
  shots. During the Spike-exit path, shots can continue updating and are drawn
  only in the states where `SpikeExitShotsVisible` allows them.
- Rendering projects each shot to its lane-edge midpoint. Its projected depth
  is `Level.Depth - Height + Position`; its size is
  `4.5 / ((projectedDepth + Projection.ZOffset) / Projection.K)`.
  `DrawRotatingShot` submits eight full diameter lines, 45 degrees apart,
  centered on the projected shot. This is one white, screen-oriented radial
  star per `Shot`; it shrinks as the shot travels inward and rotates with age.
  It does not rotate to match the lane.
- `PlayerShotRenderingTests` assert eight lines and the depth-scaled base size
  of 4.5. `SimulationTests` cover the three-tick/ten-shot policy, rotation,
  expiry and collision boundaries. Recording files store input frames and
  startup seed/configuration, not projectile objects; playback reconstructs
  shot state deterministically. Existing determinism assertions compare shot
  lane and height, but do not separately assert `Rotation` or rendered lines.
- The development notes explicitly accept 4.5 as a visual project decision
  over 4.0, not as a measured arcade constant.

## ROM player-shot lifecycle

- RAM labels identify `ply_shotcnt` at `$0135`, eight player lane slots
  `ply_shotseg` at `$02ad`, eight along-position slots `ply_shotpos` at
  `$02d3`, and per-shot special state at `$02f2`. A nonzero along-position
  marks a slot active.
- `player_fire` at `$a23f` scans player slots 7 down to 0, takes the first
  empty slot, increments the count, and copies `player_along` (`$0202`) and
  `player_seg` (`$0200`). The level starts `player_along` at `$10`.
- `move_shots` at `$a18f` moves friendly shots toward increasing along values
  by `$09` per update. A nonzero `$02f2` reduces that step by `$04`; this
  state is used by the special Spike interaction. At `$f0` or beyond, a shot
  is removed. Spike contact can slow a shot and permit another interaction;
  ordinary target/enemy-shot collision marks it for removal. `pshot_hit`
  (`$a456`/`$a463`) performs the target scan; `$a38e` marks a target-hit shot
  and `$a4f1` removes it in the hit pass.
- `draw_shots` (`$b75a`) scans active entries. For player slots (`X < 8`) it
  selects graphic offset `$08`; enemy slots use a different, time-varying
  graphic selection. Player-shot appearance therefore does not depend on
  slot number or shot age. Its color changes are inside the selected static
  vector picture, not inherited from the preceding object's draw state.
- The display builder calls `draw_shots` at `$b242`, between player and enemy
  drawing; `draw_explosions` is a separate later pass at `$b25c`.

## ROM drawing, vector shape and color

- `graphic_table` entry `$ced0` is the player-shot VJSR to vector address
  `$1594`. `draw_shots` supplies the lane and along position to
  `graphic_at_mid` (`$bcfd`), which uses the lane midpoint data and projection
  routine `$c098`, computes an external vector scale through `$bd3e`/the
  Mathbox, appends that VScale, and invokes the graphic-table entry.
- The player-shot program starts at `$3594` with `vstat z=12 c=8 sparkle=1`.
  It alternates blank (`z=off`) movement vectors with zero-displacement
  `z=stat` vectors. Those zero-length vectors, with sparkle enabled, describe
  point-like marks rather than visible connecting line segments. There are
  nine such point positions in the first color group (the origin followed by
  eight short relative moves), then `vstat z=12 c=1 sparkle=1` at `$35de`
  and eight more point positions. The program ends with `vrts` at `$3620`.
- The first group's clear relative moves are `(0,0), (+7,0), (-2,+5),
  (-5,+2), (-5,-2), (-2,-5), (+2,-5), (+5,-2), (+5,+2)`. They describe a
  center and a small octagonal ring of point locations. The second group
  begins `(+10,+5), (-4,+11), (-11,+4), (-11,-4)`, producing an expanded
  point track. The local listing's own comments mark the last four movement
  coordinates with `?`; its decoded values repeat `(0,-11)` and `(+11,0)`,
  while the inline comments suggest different components. Thus an exact,
  perfectly symmetric outer-ring reconstruction is not established by this
  listing. The robust ROM-backed result is a two-color, multi-point sparkle
  picture with 9 first-group and 8 second-group point instructions, not a
  one-dot or eight-spoke line star.
- ROM color indices are explicitly `8` for the first group and `1` for the
  second, both set inside the vector program. The listing gives no literal RGB
  mapping. The project's current player-shot renderer uses `Color.White` and
  has no established mapping for either shot index. The project-specific
  game-over/Super-Zapper color choices are not a verified mapping for these
  shot indices; the accepted index-0-to-white translation applies to the
  separate type-0 destruction effect only.
- The vector picture does not change coordinates or its `c=8`/`c=1` sequence
  with age, depth, or slot. `sparkle=1` is ROM-backed, but the annotated
  listing does not specify the vector-generator's exact sparkle/flicker
  appearance. The Mathbox supplies depth-dependent VScale outside the static
  picture. It projects the shot at the tube lane midpoint and does not apply
  a lane-specific rotation to the picture; it remains display-oriented. The
  scale is a hardware/level-dependent Mathbox result, not a literal C# pixel
  radius given by the listing.

## MAME frame evidence

All 41 files were readable and inspected in filename order. Frame numbers are
capture sequence labels, not verified simulation-tick numbers.

| Frames | Directly visible evidence |
|---|---|
| 2253-2259 | A highlighted yellow lane, Blaster/rim geometry, and a few tiny warm-colored marks along the lane. A separate reddish glyph is visible near the lower lane end. No clear white impact burst yet. |
| 2260-2273 | More small red/orange/yellow candidate marks coexist at separated positions along the same lane, forming an apparent dotted trail. A target-like colored glyph remains near the lane's lower end. Exact per-frame shot count and depth cannot be recovered from these small marks. |
| 2274-2276 | A bright white point at the target area grows into a radial burst while the earlier trail is still visible. This is temporally distinct from the small lane marks. |
| 2277 | The burst is gone and the displayed score has changed from 1200 to 1350. This supports a target-destruction handoff; it does not make the burst part of the Blaster shot. |
| 2278-2286 | The highlighted lane moves around the upper-left side of the rim. Small warm marks remain within the tube; no comparably bright impact burst is present. Their precise number is uncertain. |
| 2287-2293 | The highlighted lane continues toward the lower-left rim. Small colored marks are still visible but overlap and become difficult to separate. No distinct white impact burst is visible. |

Classification: the evolving, lane-aligned warm-colored marks are medium/high
confidence player-shot candidates, consistent with several independent active
shot slots; the MAME capture cannot resolve the internal point loci of one
shot. The yellow lane and rim Blaster are not shots. The reddish glyph near
the lower end is a target/object candidate, not confidently identified as a
shot. The white radial burst at 2274-2276 is a separate destruction effect
(high confidence from its growth, target disappearance, and following score
change). A small red/magenta mark near the center hub is not temporally clear
enough to classify and was excluded from the shot count.

The images support a very small, warm-colored on-screen shot appearance and
multiple simultaneous lane marks. They do not establish the ROM RGB mapping,
exact per-tick cadence, exact shot count, vector-point size, or a measurable
near/far size curve. The ROM VScale code establishes depth-dependent scale;
the screenshots are too small and contain multiple different-age shots to
measure that scale. No reliable rotation of an individual tiny shot is visible.

## ROM versus C# and discrepancies

| Aspect | ROM evidence | Current C# |
|---|---|---|
| Shape | Static, sparkling, two-color multi-point vector picture; 9 plus 8 point instructions and no nonzero-length lit segments. Last four outer coordinates remain ambiguous in the local listing. | Eight full white diameters, one radial star per shot. |
| Color | `c=8`, then `c=1`, both in the vector program; exact RGB/flicker appearance unresolved. | Hard-coded white. |
| Size/depth | One vector picture is VScaled using lane/along projection and Mathbox. Deeper positions use a different scale; exact C# unit conversion is not in the ROM listing. | 4.5 base radius under `Projection` reciprocal depth; shrinks inward. Direction is qualitatively consistent, conversion is project-defined. |
| Orientation/animation | Drawn at lane midpoint; no lane rotation or age/slot branch. `sparkle=1` is enabled; exact hardware sparkle cadence is unresolved. | Screen-oriented but rotates 15 degrees per simulation tick. |
| Multiple shots | Eight player slots, all use the same picture; separated marks arise from multiple active positions, with each picture itself containing many points. | Up to ten active shots, each independently drawn as one star. |
| Collision/impact | Hit shot is marked for removal; destruction uses a separate effect/display pass. Spike contact is a special slowed-shot path. | A shot is removed on a hit; enemy destruction is drawn separately. The broad handoff is consistent. |
| Cadence | Rebuilt in the display list; listing does not establish a per-shot animation sequence. | One game step per main-loop frame, target 60 FPS; input is recorded and replayed deterministically. |

Proven mismatches are the C# white color, rotating eight-line geometry, and
4.5 C# radius model versus the ROM's static two-color point program and
external VScale. MAME frames support the discrete trail and separate white
impact, but do not settle RGB, exact point sizes, or precise animation/flicker.
The C# 10-shot capacity, 3-tick cooldown, and 6-unit movement are known project
translations, not verified ROM constants in this investigation.

## Bounded next step, tests and manual acceptance

The smallest safe future change is rendering-only: replace the radial-line
primitive with a data-driven translation of the `$1594` point program, use the
existing shot lane/depth projection, and keep firing, collision, scoring,
recording, and slot behavior unchanged. Preserve 4.5 for now only as a
project-space conversion/compatibility knob, not as an arcade radius; revisit
its mapping after the ROM VScale and MAME/manual appearance are compared. Do
not add a shared projection refactor absent evidence it is necessary. Make
project RGB choices for ROM indices 8 and 1 explicit and label them as
translations, not Atari color facts. Resolve the last four vector coordinates
against the local listing before claiming exact outer-ring geometry.

Recommended automated coverage:

1. Assert the static point-program coordinates, two color groups, and
   point/segment submission count; ensure blank moves do not become visible
   connecting lines.
2. Assert geometry and color grouping are unchanged across shot age/slot, and
   screen orientation is independent of lane.
3. Assert scale is monotonic across near/mid/far positions and all projected
   coordinates remain finite.
4. Assert two active shots produce two independently projected programs;
   preserve existing collision, expiry, firing-limit, and exit-state tests.
5. Replay a recorded input stream and compare shot state plus rendered
   submissions, including `Rotation` only if it remains part of the chosen
   project model.

After implementation, Neal can use the existing file-recording flow, for
example:

```powershell
dotnet run --project src\Tempest.Game -- --record-gameplay Temp\GameplayRecordings\level5.json --level 5 --lives 12
dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\level5.json
```

Pause/step playback around an isolated shot and a multi-shot trail; compare
near, middle, and far tube positions, lane orientations, and the 8/1 project
color translations. Check that the impact burst appears only on a hit and
that collision/scoring are unchanged. Compare with the MAME sequence without
assuming capture-frame numbers are simulation ticks. Neal's manual visual
acceptance remains required; no game was launched during this investigation.

## Final safety check

HEAD remained `4ccb3cfb6cf3a1e72ac47bcfc79454ce257cb9de`. SHA-256 fingerprints
for all 41 frame images, the 14 root screenshots, the 49 files under
`UserData/`, and the 14 files under `TEMP/GameplayRecordings/` were captured
before log creation and verified unchanged afterward. The final Git status
remained `## master...origin/master` with only the same 14 untracked root
screenshots; no staged or tracked changes were created.
