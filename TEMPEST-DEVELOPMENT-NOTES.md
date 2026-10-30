# Tempest C# Development Reference

## Purpose

This document is the durable development reference for the C# Tempest recreation. It exists so future AI sessions and coding agents do not need to reconstruct important gameplay decisions from chat history.

The goal is to recreate the behavior and feel of the original Atari Tempest arcade game. Mark Incitti's BlitzMax `tempest.bmx` is an important implementation reference, but observed arcade behavior takes priority when the two differ or when an interpretation of the source produces behavior that does not match the arcade game.

## Authority Order

When deciding behavior, use this order:

1. Observed original Atari Tempest arcade behavior, including manual comparison with real arcade controls and reliable gameplay footage.
2. Mark Incitti's `tempest.bmx` implementation as an engineering reference.
3. The current tested C# implementation.
4. New assumptions only when the first three do not answer the question.

Do not silently change established arcade behavior because another implementation seems cleaner.

## Current Development Workflow

- Develop and reason about gameplay behavior in small, testable milestones.
- Use the VS Code coding agent for coordinated multi-file work.
- Require `dotnet build` and `dotnet test` after significant changes.
- Manually validate graphical and control behavior after automated tests pass.
- Commit only known-good milestones to Git.
- Do not rely on AI conversation history as the permanent project record.

## Current Architecture

The repository is organized under `src/Tempest.Game` and `tests/Tempest.Game.Tests`.

Important areas include:

- `Program.cs` - startup/composition and main frame loop only.
- `TempestGame.cs` - high-level simulation/game orchestration.
- `Gameplay/Player.cs` - player state and movement.
- `Gameplay/Shot.cs` - player projectile state.
- `Gameplay/Flipper.cs` - Flipper behavior/state.
- `Gameplay/Egg.cs` - incoming enemy precursor state and hatching behavior.
- `Levels/Level.cs` - Level 1 state, enemy reserve/accounting, and level progression data.
- `Rendering/Projection.cs` - established perspective projection.
- `Rendering/VectorRenderer.cs` - tube, player, projectile, enemy, Egg, and HUD drawing.
- `Systems/InputSystem.cs` - input sampling.
- `Systems/ScoreSystem.cs` - score state.
- `tests/Tempest.Game.Tests/SimulationTests.cs` - headless simulation tests protecting established gameplay behavior.

Existing scaffold files for later enemy and system types should be reused rather than creating duplicate concepts.

## Known-Good Git Milestones

Important confirmed commits in the history include:

- `b024828` - Working Level 1 prototype with spinner, Flipper, scoring and lives.
- `2c610ac` - Level 1 multi-Flipper wave and arcade-accurate rim collision.
- `1e5ccbb` - Refactor prototype into game systems and add tests.
- `597ca6b` - Remove an accidentally created file after the refactor.

The Egg/hatching work should receive its own commit only after full manual validation.

## Established Display and Projection

Preserve unless intentionally revisited:

- Window: 800 x 600.
- `Projection.K = 70`.
- `Projection.ZOffset = 5`.
- Level 1 uses the established 16-segment circular tube geometry.
- Level 1 tube rendering is blue.
- Player Blaster is yellow.
- Player shots are white.
- Level 1 Flippers and incoming Egg/precursor points are red.

Do not tune projection or Level 1 geometry casually. These were established early and have remained stable through subsequent gameplay work.

## Player Controls

### Arcade Spinner

A real arcade spinner is connected to the development PC. Windows presents spinner rotation as horizontal mouse movement.

The game uses relative mouse movement and captures/hides the cursor during gameplay so the spinner can rotate indefinitely without the OS cursor leaving the game window.

Preserve:

- Spinner horizontal direction.
- Current spinner sensitivity unless deliberately recalibrating against the arcade experience.
- Cursor capture during gameplay.
- Left/right keyboard movement as a development fallback.

Tempest is fundamentally designed around a spinner. Keyboard/mouse fallback may technically operate the game, but arcade-spinner behavior is a critical requirement.

### Fire

Firing currently accepts keyboard Ctrl and left mouse/arcade-button input. The existing maximum active player-shot limit and shot timing should remain unless intentionally changed from a verified source.

## Confirmed Player Projectile Behavior

Established prototype behavior includes:

- Maximum of 10 simultaneous player shots.
- Player shots travel inward/down the tube.
- Shot movement uses the established 6-unit update rate.
- Shots are rendered as rotating white vector shapes.
- Shot collision must be processed before completed Flipper-to-player collision in the frame update order.

## Flipper Behavior

### Level 1 Climb

For Level 1:

- Flippers enter at the far end of a lane.
- They climb toward the player at the established Level 1 rate of 2 units per update.
- Level 1 Flippers do not perform mid-tube lane flipping.
- On reaching the player rim, they pause and then pivot between adjacent rim segments while pursuing the player.
- Rim pause is currently 24 update ticks.
- Pivot speed is currently 8 degrees per update.
- Destroying a Flipper awards 150 points.

### Critical Arcade Rim-Collision Rule

This behavior has been manually verified as matching the original arcade game and must not be "simplified" into ordinary overlap collision:

- A Flipper reaching the outer/player rim does **not** immediately kill the Blaster.
- A Flipper paused on the rim remains shootable.
- A Flipper remains shootable during the entire visible pivot toward the Blaster.
- The player can rapidly move the Blaster underneath a Flipper while that Flipper is visibly pivoting. Visual overlap during that pivot does not kill the Blaster.
- The Blaster is destroyed only after the Flipper fully completes its pivot into the lane/location currently occupied by the Blaster.
- If the player destroys the Flipper before pivot completion, the Blaster survives.

Automated tests should continue protecting this behavior.

## Lives and Player Death

Established behavior includes:

- Three lives at the beginning of the current game prototype.
- A 60-tick player destruction state.
- Gameplay resets after a lost life without resetting score.
- Previously destroyed enemies must remain destroyed after losing a life.
- Game Over occurs after the last life is lost.
- Restart behavior must remain functional.

Enemy reset/accounting after death must never duplicate or lose enemies.

## Egg Terminology and Behavior

### What "Egg" Means

`Egg` is Mark Incitti's internal name for the tiny incoming enemy precursor seen traveling in distant space toward the far/lower rim of the web.

Do **not** interpret `Egg` as a literal egg-shaped enemy.

The expected visual/gameplay idea is:

1. A very small incoming object is visible in distant space beyond the far end of the tube.
2. Incoming objects move around in the distance while waiting to enter play.
3. An incoming object approaches the far rim/web.
4. At the web, the Egg/precursor hatches into its real enemy type.
5. On Level 1, that enemy type is a Flipper.
6. The resulting Flipper begins the normal established Flipper behavior.

This presentation matches the developer's recollection of original Tempest gameplay; current Egg rendering is producing the expected visual behavior.

### Incitti Egg Mechanics Used as Reference

Important Level 1 Egg concepts from `tempest.bmx`:

- Level 1 begins with a total enemy supply represented by 10 Eggs/enemies.
- Four Eggs are initially active; the remainder are held for later release.
- General enemy release timing uses 50 ticks on the first level setup.
- A newly created Egg begins at height `-10`.
- Incitti draws an Egg as a single Flipper-colored point.
- While roaming, Egg movement operates on an eight-update cadence and moves between edges while changing negative height.
- Roaming height is kept behind the web, roughly between `-20` and `-1` in the reference implementation.
- No more than two Eggs are intended to be in the hatching process simultaneously.
- An approaching Egg increases height toward zero.
- At height zero, the Egg hatches into an enemy.
- Level 1 enables Flippers while the later enemy categories are disabled, so Level 1 Eggs produce Flippers.

Our C# implementation may use deterministic policies where needed to keep automated tests repeatable, but the visible pacing should still resemble the arcade game.

### Egg Rendering

The current intended projection relationship is:

`projectedDepth = Level.Position + Level.Depth - (egg.Height * 20)`

Because roaming Egg heights are negative, the incoming point is projected behind/beyond the far rim.

Eggs are currently rendered as small red points. Do not invent literal egg artwork unless strong arcade evidence calls for it.

## Enemy Accounting Invariant

For the current Level 1 Egg/Flipper implementation, enemy accounting must remain coherent across release, hatching, kills, and lost lives.

Before enemies are destroyed, the conceptual total is always 10 across:

- Unreleased/reserve Eggs.
- Active roaming/approaching Eggs.
- Active Flippers produced by hatching.
- Destroyed enemies.

Hatching is a conversion, not a kill and not creation of an additional enemy.

Previously destroyed enemies must never reappear following player death.

Level completion must not occur merely because no Flipper is currently visible if reserve or active Eggs still exist.

## Current Level 1 Completion Goal

The level is complete only after the entire 10-enemy supply has been destroyed and no reserve Egg, active Egg, or active Flipper remains.

The Level Complete, Game Over, score, and lives displays already exist and should be preserved while new gameplay systems are added.

## Automated Testing Policy

Automated tests are valuable but are not sufficient proof that graphical gameplay works.

The Egg feature demonstrated this clearly: an early implementation passed its test suite but initially produced no visible Eggs in the running game because runtime integration still required correction.

Therefore every significant gameplay feature needs both:

1. Headless logic tests.
2. Manual runtime validation in the actual Raylib game window.

Important test areas include:

- Spinner-independent pure simulation logic where practical.
- Shot-before-collision update ordering.
- Flipper pivot shootability.
- Safe player movement beneath a pivoting Flipper.
- Collision only at completed pivot.
- Death timing and lives.
- Enemy accounting.
- Egg roaming/hatching behavior.
- Level completion.

Do not write automated tests that require opening or controlling a Raylib window unless there is a deliberate future decision to add an appropriate integration-test harness.

## Manual Validation Checklist

After significant code changes, manually check:

1. Tube geometry and projection look unchanged unless the feature intentionally changes them.
2. Physical arcade spinner direction and sensitivity remain correct.
3. Cursor stays captured during gameplay.
4. Arcade fire button works.
5. Player shots move and render correctly.
6. Flippers climb correctly.
7. Flippers remain shootable on the rim and throughout pivots.
8. Blaster can pass beneath a pivoting Flipper safely.
9. Completed pivot into the occupied lane destroys the player.
10. Incoming Egg/precursor points appear in distant space and hatch into Flippers.
11. Lives, reset, Game Over, restart, and Level Complete remain correct.
12. Rendering remains smooth during rapid spinner movement and multiple enemies.

## External Gameplay Reference

A useful visual gameplay reference supplied during development is the YouTube video titled:

`TEMPEST (ARCADE - FULL GAME)`

URL:

`https://www.youtube.com/watch?v=AiTgA3ZOoSo&t=198s`

The user identified approximately 23 seconds as the beginning of Level 0 gameplay in that video. Use it as a presentation/gameplay reference, not as a substitute for source analysis or hands-on arcade knowledge.

## Git and AI-Agent Safety

Before large AI-agent changes:

1. Ensure the working tree is clean.
2. Make a known-good commit.
3. Instruct the coding agent not to commit or push.
4. Require `dotnet build`, `dotnet test`, `git diff --check`, and `git status --short` at completion.
5. Inspect every changed path before committing.
6. Manually run the game and validate behavior.
7. Commit only after manual validation succeeds.

A previous VS Code agent task accidentally created an unrelated root-level file with a malformed filename. It was removed in commit `597ca6b`. Future coding-agent prompts should explicitly prohibit scratch, memory, prompt, report, or task-wording files outside legitimate project paths.

## Important Development Principle

Do not blindly translate Incitti's implementation.

Use Incitti's code to understand mechanics and constants, then compare the result with observed arcade behavior. If a literal source interpretation produces behavior that conflicts with confirmed arcade behavior, investigate and preserve the arcade behavior.

The Flipper rim-collision timing is the current canonical example of this principle.

## Next Development Direction

After the Level 1 Egg/hatching milestone is fully validated and committed:

1. Preserve this document and update it with the resulting Git checkpoint and final automated test count.
2. Review Level 1 pacing against arcade gameplay.
3. Decide whether the next feature should be a missing Level 1 mechanic or the transition/zoom behavior after clearing the level before implementing later enemy categories.
4. Continue using small, manually verified gameplay milestones.

Do not begin later enemy types merely because scaffold files already exist. Introduce them according to verified Tempest level progression.

## Level 1 Clear and Exit Zoom

The final enemy kill now enters `LevelState.ExitDelay` on the same update,
provided all 10 enemies are destroyed and reserve Eggs, active Eggs and active
Flippers are exhausted. Shots are cleared. Movement, firing, restart and enemy
simulation are ignored throughout the exit, with cursor capture unchanged.

ExitDelay lasts exactly 21 subsequent updates. The transition update enters
`ExitZoom`; active zoom movement starts on the following update. The occupied
lane and lane position remain fixed, and the cleared tube and Blaster stay visible.

Player depth starts at 400, with projected depth computed as
`Level.Position + Level.Depth - Player.Depth`. Phase 1 reduces `Projection.ZOffset`
by 2 and player depth by 3 each update while the offset is at least -40. Starting
at 5, this takes 23 updates and leaves the offset at -41. Phase 2 increases
`Projection.K` by 2, adds another 2 when the updated K exceeds 100, and reduces
player depth by 3.2 each update. The existing claw proportions scale with depth;
normal gameplay retains its established shape and near-rim placement.

When player depth falls below 10, the game enters the existing `Complete` state,
restores K to 70 and ZOffset to 5, and displays the existing Level Complete text.
R replays Level 1 with normal depth, projection, Egg supply and exit counters;
the established replay score/lives behavior is preserved. No Level 2 or bonus
is implemented.

Automated validation: 54 headless tests pass, with no failures or skipped tests.
This includes exit eligibility, timing, phase changes, projection finiteness,
final-shot precedence, replay and the established gameplay regressions.
Manual visual validation is still required for recognizable cleared-level travel,
Blaster scale and lane alignment, zoom smoothness/framing, the final projection
restoration, message timing and physical spinner/cursor behavior after replay.

## Spiker and Spike Development Milestone

Normal Level 1 remains Flipper-only with its validated input, collision order
and exit sequence. The explicit `--spiker-test` application argument selects a
development wave on the same geometry; it does not implement arcade progression.
Use `dotnet run --project src/Tempest.Game -- --spiker-test` to launch it.

The scenario starts height-60 spikes on all 16 lanes and uses the existing
ten-enemy Egg supply. Only Spikers and Flippers are eligible. Spiker selection
uses the source's negative-result bias, four-Spiker limit and active-enemies-plus-
Eggs restriction (reserve is excluded). Assigned Spiker speed is fixed at 3 for
this development wave: the fractional-argument coercion of BlitzMax's
`Rand(2.5,4.5)` is still unverified, so no claim of matching that distribution is
made. Tests inject Random to control selection and retreat decisions.

Spikers initialize without moving, build outward, and reuse their lane's spike.
The source-specific assignment `spike.Height = spiker.Height - 4` is preserved,
including possible brief shortening. Missing spikes are recreated at the
builder's current height. Retreat uses speed times 0.90 and returns one pending
Egg on removal, without a defeat or points. Spiker shots score 50; spike shots
score 1 and subtract 10 height, deleting at zero or below. Spikes persist through
builder death, retreat and life loss. Replay/full restart restores initial spikes.

Development-wave shot targets retain insertion order, including replacement
spikes created after their builders; they are not sorted by proximity. Normal
Level 1 retains its existing reverse Flipper collision scan. Spikers update in
creation order. The spiral follows the reference geometry and rotates 45 degrees
per simulation update (one update per displayed game frame); spikes are green
lines with white tips using the established projection.

Residual spikes do not count as enemies or block clear. This scenario uses the
first spike-enabled reference avoidance interval: 151 updates, followed by the
21-update delay and the established zoom. Movement and firing are allowed while
alive in those states; shots originate at current player depth. Locally, travel
processes depth, existing shots, spike hazards, then player input. A same-lane
spike with height greater than player depth triggers reverse zoom. The source's
K-minus-4/offset-plus-1 restoration and depth snap are preserved; the game always
waits for the full 60-tick life loss before retrying, rather than allowing a short
reverse zoom to cancel the loss. Spike-free Level 1 controls/timing are unchanged.

Automated validation: 81 tests passed, zero failed/skipped. Includes production
mixed-wave completion, insertion-order collisions, retreat accounting, spike
persistence, reverse-zoom retry/Game Over/replay, and finite render submissions.
The optional retreat bullet, other enemies, bonuses, audio and multi-level
progression are intentionally omitted. Manual comparison is still required for
spiral appearance/rotation, spike growth/damage feel, initial spikes, scoring,
avoidance timing and reverse-zoom presentation against original Atari Tempest.
Physical spinner, cursor capture, Flipper regressions and normal exit visuals
also require manual rechecking. Automated tests did not open a game window.

## Flipper Enemy Bullet Development Milestone

Normal Level 1 still does not fire EnemyBullets. The opt-in
`--enemy-bullets-test` argument enables Flipper ammunition and firing; it
composes independently with `--spiker-test` and `--lives`. Examples:

```powershell
dotnet run --project src/Tempest.Game -- --enemy-bullets-test
dotnet run --project src/Tempest.Game -- --enemy-bullets-test --lives 10
dotnet run --project src/Tempest.Game -- --spiker-test --enemy-bullets-test --lives 10
```

Enabled Flippers receive 1-3 bullet attempts using the injected Random source and
begin with a 16-call timer. During climbing and after pause-to-edge transitions,
eligible calls decrement it. Below zero, it resets to 16; only a random result
above 75 fires and consumes ammunition. A bullet starts on the firing lane at
the Flipper's post-movement height. It moves outward by 3 per update and expires
only after exceeding depth 400. Same-lane player collision requires
`Depth - bullet.Height < 8`; same-lane player shots hit when shot depth is
strictly below bullet height. Bullet hits award 25 points and do not alter enemy
accounting. Bullets remain frozen through the death animation and are cleared on
life reset; they are also cleared when exit starts.

Development shot collision dispatch uses explicit insertion order: hatching
adds a Flipper to the test target list; bullets append when created. Thus a
Flipper precedes its bullets in a collision tie. Normal Level 1 retains its
existing reverse Flipper-only collision path. Player/Flipper movement, firing,
projection and exit behavior are unchanged. Bullets render as distinct white
radial lines using projected size based on 6 units and rotate 15 degrees per
simulation update.

Automated validation: 111 headless tests passed, zero failed/skipped. Manual
comparison remains required for Flipper bullet cadence/ammunition, visible scale
and speed, rim hit reach, score display and arcade hit feel. The original source
is an engineering reference, not confirmation of arcade accuracy.

## Tanker Cargo Development Milestone

Normal Level 1 and `--spiker-test` remain unchanged. The mutually exclusive
`--tanker-test` scenario creates exactly one deterministic Tanker from original
Egg slot 0; the other nine original slots hatch as ordinary Flippers. Run it with:

```powershell
dotnet run --project src/Tempest.Game -- --tanker-test --lives 10
```

The Tanker is fixed to its hatch lane, starts at height 10, carries two latent
Flipper units, and uses a documented development climb speed of 1.85. It releases
only after updated height exceeds 385. Natural release awards no Tanker score;
shooting the Tanker requires same lane and shot depth strictly below its height,
consumes that shot, awards 200, and releases live cargo. Neither path changes
ordinary Flipper behavior. Cargo appears on opposite adjacent lanes at release
height, starts in the existing Flipping state with calculated geometry and
opposing directions, and advances beginning on the next simulation update.

Tanker mode keeps a separate original-slot ledger and materialized cargo-unit
ledger; it does not change `Level.EnemyTotal`, normal `EnemiesDestroyed`, or the
normal/Spiker ten-unit invariant. Original slot 0 is stable across resets. Before
release it returns as one Tanker Egg after a life loss, still carrying two latent
units. After release the Tanker slot stays resolved and surviving cargo snapshots
return as pending cargo Flippers, not Eggs. Level clear waits for every original
slot and materialized cargo unit to resolve, and for all active/pending entities
to be gone. The Tanker HUD separately displays unresolved original slots, cargo
units (including latent cargo) and active objects.

The shared development shot list retains insertion order: a Tanker is tested
before cargo appended after its destruction; the consuming shot cannot hit those
new children. Normal Level 1 keeps its existing collision path. Existing
Flipper-fired bullets can be explicitly enabled with
`--tanker-test --enemy-bullets-test`; this does not enable Tanker-fired bullets.
Tanker bullets, Super Zapper, zap timing, stillborn cargo, other carrier types and
full progression are deferred.

Automated validation: at the completion of the Tanker cargo milestone, 142 headless
tests passed, zero failed/skipped (per the Tanker implementation report; later
milestones changed the suite total). Manual
arcade comparison remains required for Tanker geometry/color/scale, 1.85 speed,
release point, child placement and pivot appearance, score/pacing, player danger,
and reset/exit presentation. This scenario uses an intentionally deterministic
slot and cargo-side choice; it is a development test, not normal arcade progression.

## Flipper Enemy Bullet Development Milestone

Normal Level 1 still does not fire EnemyBullets. The opt-in `--enemy-bullets-test`
argument enables Flipper ammunition and firing; it composes independently with
`--spiker-test` and `--lives`. Examples:

```powershell
dotnet run --project src/Tempest.Game -- --enemy-bullets-test
dotnet run --project src/Tempest.Game -- --enemy-bullets-test --lives 10
dotnet run --project src/Tempest.Game -- --spiker-test --enemy-bullets-test --lives 10
```

An enabled Flipper is assigned 1-3 bullet attempts using the injected Random
source and begins with a 16-call timer. During climbing and after pause-to-edge
transitions, eligible calls decrement the timer. Below zero, it resets to 16;
only a random value above 75 fires and consumes ammunition. A bullet starts on
the firing lane at the Flipper's post-movement height. It moves outward by 3 per
update and expires only after exceeding depth 400. Same-lane player collision
requires `Depth - bullet.Height < 8`; same-lane player shots hit when shot depth
is strictly below bullet height. Bullet hits award 25 points and do not alter
enemy accounting. Bullets remain frozen through the death animation and are
cleared on life reset; they are also cleared when exit starts.

Development shot collision dispatch uses explicit insertion order: hatching
adds a Flipper to the test target list; bullets append when created. Thus a
Flipper precedes its own bullets in a collision tie. Normal Level 1 retains its
existing reverse Flipper-only collision path. Player/Flipper movement, firing,
projection, and exit behavior are unchanged. Bullets render as distinct white
radial lines using projected size based on 6 units and rotate 15 degrees per
simulation update.

Automated validation: 111 headless tests passed, zero failed/skipped. Manual
comparison remains required for Flipper bullet cadence/ammunition, visible scale
and speed, rim hit reach, score display, and arcade hit feel. The original source
is an engineering reference, not confirmation of arcade accuracy.

## Tanker Cargo Development Milestone

Normal Level 1 and `--spiker-test` remain unchanged. The mutually exclusive
`--tanker-test` scenario deterministically assigns original Egg slot 0 to one
Tanker; all other original slots hatch into ordinary Flippers. Run it with:

```powershell
dotnet run --project src/Tempest.Game -- --tanker-test --lives 10
```

The Tanker is fixed to its lane, starts at height 10, carries two latent Flippers,
and uses a documented development climb speed of 1.85. It releases only after
updated height is strictly greater than 385. Natural release awards no Tanker
score. A same-lane shot strictly below its height destroys it for 200 points and
releases the same two live Flippers. Children start at release height on opposite
adjacent lanes, in Flipping state with calculated geometry and opposing
directions. Their first update is deferred to the next tick.

Tanker mode adds a separate original-slot/cargo ledger and does not change
`Level.EnemyTotal`, `EnemiesDestroyed`, or the existing normal/Spiker accounting
invariants. Before release, a Tanker survivor returns as one Egg with two latent
cargo units. After release, the Tanker slot stays resolved and each surviving
cargo Flipper is restored as pending cargo, not an Egg or another Tanker. Clear
requires all original slots and materialized cargo units resolved and no active
or reserve entities. Tanker HUD rows distinguish unresolved ORIGINAL slots,
latent/materialized CARGO units, and ACTIVE entities.

Shot targets in tanker mode use the existing development insertion order. Tanker
is inserted when its Egg hatches; released cargo appends after carrier removal.
The consuming shot cannot hit newly created children. Normal Level 1 retains its
existing collision path. Existing Flipper bullets can be explicitly enabled by
combining `--enemy-bullets-test`; they never originate from Tankers. Tanker
bullets, Super Zapper, zap/stillborn cargo, other enemy types and full progression
are deferred.

Automated validation: at the completion of the Tanker cargo milestone, 142 headless
tests passed, zero failed/skipped (per the Tanker implementation report; later
milestones changed the suite total). Manual
comparison remains necessary for Tanker line-art appearance, color and scale,
1.85 speed, release timing, cargo placement/pivots, player danger, score and
death/reset/exit presentation. This is a deterministic development scenario, not
normal Level 3 arcade progression.


## SUSPECTED / NOT YET REPRODUCED: converging rim Flippers

Reported by the project owner: two Flippers on the rim approaching each other from
opposite directions may converge on the same lane and stay synchronized, so they
look like a single Flipper. The original Atari game is reported not to do this.
This has NOT been reproduced in the C# game and NOT source-analyzed against Incitti
or the arcade. No gameplay change has been made. Investigate (reproduce first,
then compare with Incitti's pivot lane selection) before changing behavior.
