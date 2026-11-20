# Enemy-destruction ROM investigation

## 1. Starting Git state

- Branch: `master`, tracking `origin/master`.
- `HEAD`: `4604297920f90c96731abd727ca3c73660300e88`, exactly the accepted paused-playback screenshot baseline.
- Initial worktree had no tracked, staged, or deleted changes. The only untracked paths were `screenshot000.png` through `screenshot013.png`; they were left untouched.
- The requested prompt and this report are under ignored `AI-Logs/` paths.

## 2. Evidence inspected

- ROM material: [`Reference/arcade/Tempest Rom Source code.md`](../../Reference/arcade/Tempest%20Rom%20Source%20code.md), an annotated 6502/vector-ROM disassembly. Its opening disclaimer says material may be wrong or missing, and some labels are explicitly guesses. Findings below are therefore classified as ROM-listing-backed, not independently verified by running a ROM image.
- Production paths: [`TempestGame.cs`](../../src/Tempest.Game/TempestGame.cs), [`Level.cs`](../../src/Tempest.Game/Levels/Level.cs), [`Level3WaveLedger.cs`](../../src/Tempest.Game/Levels/Level3WaveLedger.cs), [`EnemyBullet.cs`](../../src/Tempest.Game/Gameplay/EnemyBullet.cs), [`Tanker.cs`](../../src/Tempest.Game/Gameplay/Tanker.cs), [`Fuseball.cs`](../../src/Tempest.Game/Gameplay/Fuseball.cs), [`Spiker.cs`](../../src/Tempest.Game/Gameplay/Spiker.cs), [`Pulsar.cs`](../../src/Tempest.Game/Gameplay/Pulsar.cs), [`Shot.cs`](../../src/Tempest.Game/Gameplay/Shot.cs), [`ScoreSystem.cs`](../../src/Tempest.Game/Systems/ScoreSystem.cs), [`AudioSystem.cs`](../../src/Tempest.Game/Systems/AudioSystem.cs), and [`VectorRenderer.cs`](../../src/Tempest.Game/Rendering/VectorRenderer.cs).
- Replay/render context: [`GameplayPlaybackSession.cs`](../../src/Tempest.Game/Debug/GameplayPlaybackSession.cs), [`GameplayRecordingFile.cs`](../../src/Tempest.Game/Debug/GameplayRecordingFile.cs), and [`VisiblePlaybackController.cs`](../../src/Tempest.Game/Debug/VisiblePlaybackController.cs).
- Durable project references: [`TEMPEST-BEHAVIOR-SPEC.md`](../../TEMPEST-BEHAVIOR-SPEC.md) and [`TEMPEST-DEVELOPMENT-NOTES.md`](../../TEMPEST-DEVELOPMENT-NOTES.md).
- Read-only user note: [`TEMP/myNotes.md`](../../TEMP/myNotes.md).
- Visually inspected the untracked root `screenshot000.png`–`screenshot013.png`. These are project gameplay/debug captures (including project HUD text), not a MAME hit-effect frame sequence. No MAME destruction images were attached to this task; `Reference/` contains no PNGs. The available captures do not establish target type, exact colors, or effect timing.

No secondary implementation was needed to resolve the main control-flow question. No tests were run: this was a read-only investigation, and tests would not add ROM or MAME evidence.

## 3. Current C# collision and destruction flow

`TempestGame.RunOneStep` updates the player, calls `UpdatePlayerShots`, then advances enemy shots and checks level completion before later enemy updates (`TempestGame.cs:289-417`). `UpdatePlayerShots` advances each player shot and routes it to either the Spike-only, basic Flipper/bullet, or insertion-ordered development-target scan (`:710-723`).

- **Ordinary Flipper:** `CheckShotAgainstFlippers` removes it from `flippers` and `developmentCollisionOrder`, records an enemy destroyed, awards 150, and plays `EnemyDestroyed` (`:754-768`). Level 1 uses this simple route.
- **Enemy projectile:** the same method removes it from `enemyBullets` and collision order and awards 25; it does not record an enemy kill or play a destruction sound (`:770-779`). `EnemyBullet` is deliberately not a level-accounted enemy (`Gameplay/EnemyBullet.cs`).
- **Development/production target dispatch:** `CheckShotAgainstDevelopmentTargets` removes targets and their collision-order entries before handling score/accounting (`:989-1068`). Fuseballs award their height-dependent `KillScore`, create a score popup, and play the enemy-destroyed sound; Pulsars award 200; Spiker bodies award 50; Flippers award 150; enemy bullets award 25 without an enemy-destroyed sound.
- **Tanker:** the shot removes the Tanker, resolves its non-ledger identity or later ledger identity, then calls `ReleaseTankerCargo` and plays `EnemyDestroyed` (`:1033-1042`, `:1284-1359`). In production-ledger mode the parent is marked destroyed and cargo is queued for shot activation; in the isolated path two cargo Flippers are created. Shot-released cargo is deferred so the killing shot cannot hit it in the same update. Ledger operations include `QueueCargoForShotActivation`, `MarkOriginalDestroyed`, and `MarkCargoDestroyed` (`Levels/Level3WaveLedger.cs:220-250`).
- **Spiker body versus Spike:** the body is a destroyable enemy. A Spike hit instead shortens/removes the Spike, awards one point, and plays `SpikeHit` (`:728-752`); it is not an enemy destruction.

The narrow seam is immediately after a hit is confirmed and before the target is removed or Tanker cargo is released: snapshot the target category, lane, and depth into visual-only state there, then execute the existing removal/score/sound/ledger/cargo code unchanged. Effects must not enter `developmentCollisionOrder`, inherit `Enemy`, mutate score/identity, or participate in collision or `CheckForLevelComplete` (`:1542+`). Current C# checks completion immediately after shot processing; it has no effect-count gate.

There is no reusable general visual-effect lifecycle. `FuseballScorePopup`, player-death presentation, and Game Over vector bursts are specialized state systems. `VectorRenderer.Draw` explicitly draws each entity family; there is no shared generic VFX collection (`Rendering/VectorRenderer.cs:148-200`).

## 4. ROM shot-hit and effect control flow

### Ordinary targets and enemy shots

- `pshot_hit` `$a454` scans player shots. `$a463` scans target slots and distinguishes enemy-shot slots (`Y < 4`) from enemies (`Y >= 4`) at `$a47b` (`ROM listing:4754-4798`).
- A matching enemy projectile calls `$a36f` (`$a483`). That handler calls `$ccc1`, captures `enm_shotpos` and `enm_shotseg`, supplies effect type `0`, calls `$a3d4`, then clears the projectile position and decrements `enm_shotcnt` (`:4639-4652`).
- An ordinary enemy hit calls `$a38e` (`$a4e2`). It marks the player shot for removal, derives the enemy segment (including a special adjustment while the enemy is between segments), calls `$a3ca` with type `0`, then calls `$a06f` to remove/resolve the enemy and finally scores by enemy type (`:4653-4679`). The score lookup is distinct from the effect selection.
- Both paths reach the same `$a3d4`/`$a3d6` allocator, so **enemy projectiles use a different collision/removal handler but the same visual-effect type and allocator as ordinary non-Fuzzball enemies**. `$a3ca` also calls `$ccc1`; no sound identity is inferred here.
- `$a3d6` has eight records (indices 0–7). It stores active along-position in `$030a`, effect type in `$0302`, frame in `$0312`, and segment in `$02fa` (`:4697-4718`). It captures position/lane and does not advance position afterward. The enemy/projectile object is separately cleared/resolved.
- If all eight records are occupied, the allocator selects the occupied record with the greatest `$0312` age and reuses it; equal ages resolve to the lowest index because the scan descends and ties replace the prior candidate. It does not silently drop the new effect (`:4699-4716`).
- `$a416` advances active records and deactivates them at the per-type threshold (`:4720-4743`). The normal gameplay sequence calls `pshot_hit` at `$9720` and `$a416` immediately after it at `$9723` (`:3017-3026`). For type 0, the frame increment is 3 and the stop threshold is 10: ages progress 0, 3, 6, 9, then deactivate on reaching 12. This is a ROM update-count observation, not an exact wall-clock or MAME-capture duration.
- `draw_explosions` `$b79a` reads the stored lane/depth, selects a vector graphic, and draws it at that location (`:7215-7250`). **Correction (2026-11-08):** type 0 uses table offset `$00` from `$b7e5`, not the hit-by-shot offset `$28`. The expression `(frame >> 1) & $fe` therefore selects graphic-table byte offsets `0, 0, 2, 4` at ages `0, 3, 6, 9`, resolving to entries 0, 0, 1, 2: “Explosion, size 1,” size 1, size 2, and size 3 (`:7215-7251`, `:9927-9930`, `:1016-1073`). Each picture sets `vstat ... c=0`. The size-1/2/3 vectors, rather than the nine-entry hit-by-shot sequence, are the common type-0 presentation.
- The nine hit-by-shot entries `$1b18`, `$1b14`, `$1b10`, `$1b0c`, `$1b08`, `$1b04`, `$1b0c`, `$1b14`, `$1b18` and shared body `$1b1a` change ROM color indices 9, 11, and 10 (`:9947-9955`, `:1557-1577`). They are not selected by the common type-0 path. The earlier conclusion that type 0 uses that multicolor picture was incorrect; the supplied MAME images were not available to establish how their reported multicolor appearance relates to the listing.

### Fuseball special case

The ROM listing calls this enemy a **Fuzzball**. `$a463` branches specially for type 4 at `$a4a0`; unlike types 0–3, a successful type-4 shot uses `$a309`, which marks the player shot and selects an effect type from `POKEY2` (`$ad da 60`, low three bits). Values 0–2 become effect types 2–4; values 3–7 are reduced to 0 and therefore become type 2. It then calls `$a3ca` and `$a06f` and scores separately (`:4591-4616`, `:4795-4828`). The special branch also imposes additional position/segment/state checks before destruction (`:4795-4820`).

Types 2–4 have frame increment 3 and threshold 20 in `$a44e/$a448`; `draw_explosions` uses offsets `$5a`, `$58`, and `$56` from `$b7e5` and suppresses the type-0 animated-frame term for types >=2 (`:4720-4743`, `:7241-7251`). The graphic table separately labels four “cloud of dots” entries at vector addresses `$1628`, `$16de`, `$1780`, and `$182a` (`:9932-9935`), but the listing's type-offset arithmetic does not unambiguously map types 2–4 to those four entries. Exact Fuseball picture identity and shape are therefore **UNKNOWN / UNRESOLVED**, not assigned by guess.

### Gameplay/counting interaction

The effect is not checked by the shot-collision scan and is not an enemy or projectile slot. However, the active effect count `$0116` is explicitly included in arcade transition gates: `$a504` checks player shots, enemy shots, and `$0116` before its completion/death handling (`:4837-4845`), and `$a59b` checks `enemies_pending | $0116` before level-over processing (`:4908-4914`). Thus the ROM effect can delay arcade transition handling even though the enemy itself is resolved separately. This differs from accepted C#: C# completion is checked after shot processing and is based on enemy/ledger state only. The visual-only implementation must preserve the C# completion tick as explicitly requested.

The Spikes path is separate: player-shot Spike erosion is handled at `$a1fa` (`:4469+`) and Spike drawing state uses `$039a`; it should not be given the enemy-hit effect. Super Zapper destruction is outside the proposed implementation, but the listing's `$a8ac` jumps into `$a398`, which is shared with the enemy-kill path. This shared code supports the common allocator finding; it does not expand the requested effect scope.

## 5. Per-object behavior matrix

| Target | Normal Blaster shot? | ROM-listing effect path | C# current behavior / evidence class |
|---|---|---|---|
| Ordinary Flipper | Yes | Common enemy path `$a38e` → `$a3ca` → type 0 allocator; hit-by-shot vector table | Removes, +150, enemy-destroyed sound. **ROM-LISTING-BACKED / EXISTING PROJECT BEHAVIOR** |
| Tanker | Yes | Same type-0 common enemy path; no Tanker-specific hit visual branch | Removes, resolves parent, releases cargo, +200, sound. **ROM-LISTING-BACKED / EXISTING PROJECT BEHAVIOR** |
| Tanker-released cargo Flipper | Yes, after materialization | Flipper follows common type-0 enemy path; no cargo-specific effect program found | Ledger marks cargo destroyed (or isolated cargo identity resolves), +150, sound. **ROM-LISTING-BACKED / EXISTING PROJECT BEHAVIOR** |
| Spiker body (not Spike) | Yes | Common type-0 enemy path for non-type-4 targets | Removes body and resolves identity, +50, sound; Spike itself is separately eroded. **ROM-LISTING-BACKED / EXISTING PROJECT BEHAVIOR** |
| Fuseball / ROM “Fuzzball” | Yes, subject to special state/segment checks | Type-4 `$a309` branch; POKEY-derived selection among effect types 2–4. Exact vector pointer unresolved. | Development-only test scenario; removal, height-based score, popup, sound. **ROM-LISTING-BACKED / EXISTING PROJECT BEHAVIOR** |
| Pulsar | Yes | No Pulsar-specific destruction branch; non-type-4 enemy dispatch uses type 0 | Development-only test scenario; removal, +200, sound. **ROM-LISTING-BACKED / EXISTING PROJECT BEHAVIOR** |
| Enemy projectile | Yes | Separate `$a36f` handler, same type-0 allocator and drawing path; no enemy score/count resolution | Removes projectile, +25, no enemy-destroyed sound or enemy accounting. **ROM-LISTING-BACKED / EXISTING PROJECT BEHAVIOR** |
| Spike itself | Eroded by a shot, not an enemy kill | Separate Spike-height/erosion path, outside this effect | Shortens/removes, +1, SpikeHit sound. Excluded from this milestone. **ROM-LISTING-BACKED / EXISTING PROJECT BEHAVIOR** |

The ROM's only explicit shot-destruction fork in this dispatch is type 4 (Fuzzball). The source code labels type 4 as Fuzzball; the normal enemy path does not choose different visual programs based on Flipper, Tanker, Spiker, or Pulsar identity. The project implements Tanker/Spiker behavior in production-ledger/development paths, while Fuseball and Pulsar remain development-only scenarios (`TempestGame.cs:14-18`).

## 6. MAME visual observations

**ARCADE/MAME-OBSERVED: none.** No MAME image sequence was present among the task's attachments or repository references. The visible root PNGs are project captures, not a time-ordered MAME sequence; any white marks in those single frames cannot safely be attributed to the described destruction effect. Accordingly, the following remain unverified visually: target identity, exact colors, whether the bright and speckled components coexist, motion/persistence, repeated MAME states, and whether the enemy geometry remains during any phase. No duration was inferred by counting PNGs.

## 7. Known facts versus interpretation

- **ROM-LISTING-BACKED:** normal enemy and enemy-projectile hits each create type-0 records in the same eight-slot effect pool; projectiles use a separate hit handler. Type 4 Fuzzball has a separate shot-hit branch with POKEY-derived effect-type selection.
- **ROM-LISTING-BACKED:** effects preserve hit lane/depth, animate via per-type frame/threshold tables, do not occupy enemy/shot slots, and can delay ROM transition logic through `$0116`.
- **EXISTING PROJECT BEHAVIOR:** C# has no enemy/projectile hit visual; it immediately removes the target and preserves its current score, audio, ledger, cargo, and completion behavior.
- **PROJECT TRANSLATION CANDIDATE:** a visual-only transient effect record created at the confirmed-hit seam, driven solely by simulation ticks and rendered as vector geometry.
- **ROM-LISTING-BACKED:** type-0 vector sizes progress through size 1, size 1, size 2, and size 3 at the reachable ages; all three size programs declare ROM color index 0. C# has no established equivalent ROM RGB value.
- **UNKNOWN / UNRESOLVED:** actual MAME color mapping and capture cadence; how the reported MAME multicolor appearance relates to the common type-0 listing path; exact Fuseball type-to-vector pointer mapping.
- **SECONDARY REFERENCE:** no secondary implementation was used to override or fill gaps in the ROM listing.

## 8. Bounded implementation recommendation

Add a small non-`Enemy` destruction-effect state/list owned by `TempestGame`. On each supported hit, snapshot lane, depth, target/effect category, and the selected ROM program before mutating the target. Keep existing collision, score, sound, identity, Tanker cargo, and completion code in its current order. Effects must not enter the collision-order list or any level/ledger count.

Represent the observed ROM branches explicitly: a common type-0 program for Flipper/Tanker/Spiker/Pulsar/cargo-Flipper/projectile and a separate Fuseball program selection only after its mapping is verified. For the common program, preserve the ROM's eight-record capacity, age progression, deterministic oldest-record replacement, and reachable size-1/2/3 vector entries rather than using an unlimited generic particle emitter. Do not add a Spike program.

Advance effects only with gameplay simulation ticks (not renderer wall time). Add them to the render path using captured lane/depth and existing vector projection conventions. Ensure they remain drawable through an exit transition long enough to show the shot that completed a wave; keep the list entirely outside `CheckForLevelComplete`, so current C# completion timing does not change. Pause freezes state; reverse scrub reconstructs it by replay; forward scrub advances it sequentially. The recording stores seeded input frames, not world state (`GameplayRecordingFile.cs`); do not change the format. If the Fuseball's visual type needs randomness, do not consume the shared gameplay RNG stream, since that could change later gameplay. Use a deterministic visual-only stream/counter after its selection is validated.

## 9. Proposed automated tests

1. Each target hit creates exactly one effect snapshot at the pre-removal lane/depth while preserving current score, sound, ledger, cargo release, and enemy/projectile counts.
2. Spike erosion, Super Zapper, and player-death paths do not accidentally receive this normal Blaster-hit effect.
3. Fixed-tick effect frame progression, exact removal threshold, eight-slot capacity, oldest-slot replacement, and deterministic tie handling; multiple simultaneous hits remain deterministic.
4. A Tanker kill still queues/releases the same cargo, and the effect is associated with the Tanker rather than either child.
5. Completion occurs on the same C# tick as before, including the last-enemy hit; effects never block or satisfy level completion.
6. Headless playback produces equal effect states at equal boundaries; pause holds the state, reverse reconstruction restores it, and forward scrub replays it. Recording format and gameplay RNG outcomes remain unchanged.
7. Vector-renderer delegate tests assert captured geometry/program and continued drawing during the relevant exit state without launching the interactive game. Existing test patterns include `PlayerShotRenderingTests`, `SimulationTests`, `GameplayPlaybackSessionTests`, and `VisiblePlaybackControllerTests`.

## 10. Proposed manual acceptance scenarios

- Use the same normal Flipper shot sequence in MAME and C#; compare the initial hit location, vector program phases, color, persistence, and disappearance.
- Destroy an enemy projectile separately and confirm it follows the same visual program while retaining its current C# score/audio behavior.
- Destroy a Tanker and confirm its effect stays at the carrier position while cargo still materializes and follows existing timing.
- Exercise closely spaced hits (including more than eight effects if reproducible) to validate pool replacement.
- Inspect Fuseball separately against MAME before accepting a special program; do not infer it from ordinary Flipper frames.
- Verify the final-enemy effect remains visible during exit without delaying C# level completion; test pause, one-step forward/back, and screenshot capture at fixed playback boundaries.

## 11. Unresolved evidence

1. Supply the actual MAME frame sequence (or a recording with capture cadence) to classify visual phases and colors.
2. Resolve how ROM effect types 2–4 map through the annotated graphic table to concrete Fuseball vector programs; the present listing's offset arithmetic is not enough to assign exact pictures confidently.
3. Confirm whether the observed white burst and yellow/orange fragments are simultaneous parts of one ROM vector program or separate rendered components.
4. Compare the disassembly against an authoritative ROM/vector-ROM dump or emulator trace before claiming pixel-level shape, palette name, or absolute duration.

## 12. Final Git state

No production code, tests, durable specifications, configuration, project files, screenshots, recordings, or Git state were changed. The copied prompt and this report are ignored by `AI-Logs/`. Final verification confirmed `HEAD` remained `4604297920f90c96731abd727ca3c73660300e88`, with no tracked/staged changes; the original fourteen untracked screenshot files remain.
