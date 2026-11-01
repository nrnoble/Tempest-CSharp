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

### Controller keyboard compatibility (2026-10-31)

Neal's physical USB controller run in `--input-diagnostic` confirmed ordinary
Raylib keyboard press and release events for P, Enter, Space, left Ctrl,
left Alt, left Shift, Z, X, C and V. Text-producing buttons also exposed
character events, but production input uses only the existing key-state APIs.
This establishes usable Raylib events, not a particular USB/HID or Windows
injection mechanism. Subsequent physical-controller gameplay testing passed
for Space when available, resolving the earlier uncertainty for practical
validation without attributing a cause to the earlier isolated failure.

P is an additive R alias for the existing Game Over/Level Complete restart/
replay request, using `IsKeyPressed` and the same state gates. V is an
additive quit alias alongside Raylib's unchanged default Escape/window-close
condition, exiting before the next simulation step through the existing
cursor/window cleanup. Space/right mouse zap, Ctrl/left mouse Fire, spinner
and keyboard arrows are unchanged. Z/X/C/Enter/Alt/Shift/1/5 have no new
actions. These are control convenience decisions, not ARCADE VERIFIED rules.

The small red Atari-style Player 1/Player 2 volcano buttons under Turbo Mode
produced no observable Raylib diagnostic event. Their investigation, including
a possible future Player 1 Start binding, is explicitly deferred; no lower-
level Windows hook, Raw Input or remapping software was introduced.

The standalone DEVELOPMENT ONLY diagnostic remains available and does not
run during gameplay. Its readability and Neal's reported Tee-Object capture
issue are non-blocking diagnostic usability concerns, not part of this change.
Manual controller/input validation completed PASS on 2026-10-31 on both the
ordinary keyboard and physical USB controller. Spinner, existing Fire,
controller/keyboard Space, controller P in applicable restart/replay states,
keyboard R, controller V and keyboard Escape worked normally. Existing
controls were preserved and no obvious gameplay regression was observed.
The additive P/V aliases are manually accepted.

### Deferred fidelity comparisons

- **Spinner:** current C# movement is good, very playable and not a blocker.
  Using the same physical USB spinner, Neal found Incitti's response
  effectively perfect without changing Windows mouse settings. A dedicated
  future task should compare actual spinner/input/movement processing before
  changing sensitivity; no spinner refinement is part of this checkpoint.
- **Timing:** Neal's qualitative assessment, informed by extensive Atari
  arcade experience, is that Incitti's timing feels extremely close to the
  arcade. C#/Incitti timing differences are useful future investigation
  targets, not grounds for casual dismissal. Direct arcade observations
  remain highest authority; Incitti remains an engineering/reference source,
  not automatically ARCADE VERIFIED.
- **Visuals:** Neal rates C# Blaster/enemy rendering impressive and reasonably
  faithful at roughly 3.5/5, versus approximately 5/5 for Incitti's arcade
  resemblance. These subjective ratings are neither measurements nor
  automated acceptance criteria. Defer a visual-fidelity milestone comparing
  actual vector/object definitions, transforms, projection, scale, animation
  and rendering behavior before changing graphics.

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

- A Flipper that arrives directly at the rim in the Blaster's lane kills on
  that arrival. A Flipper arriving in another lane pauses and pivots toward the
  Blaster; visual overlap during that pivot is safe until the pivot completes
  into the occupied lane.
- A Flipper paused on the rim remains shootable.
- A Flipper remains shootable during the entire visible pivot toward the Blaster.
- The player can rapidly move the Blaster underneath a Flipper while that Flipper is visibly pivoting. Visual overlap during that pivot does not kill the Blaster.
- The Blaster is destroyed when the Flipper directly arrives in its lane or
  fully completes a pivot into the lane/location currently occupied by it.
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

## Level 1 Completion Goal

Historical prototype rule (superseded): completion required destroying all ten
enemies. Current Level 1 completion also permits the established all-rim clear:
after Eggs are exhausted, enemies reaching the rim resolve without kill score.
The current behavior and evidence boundaries are recorded in
`TEMPEST-BEHAVIOR-SPEC.md`.

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

## Incitti rimit / Hurry-Up Development Milestone

Starting checkpoint: `37535eb`. Only the direct `--fuseball-test` and
`--pulsar-test` scenarios enable this source-derived mechanic. It does not add
normal level composition or progression, and is not arcade-verified hurry-up.

`TempestGame.HurryUp` is a read-only game-owned latch. Active play advances the
shared pulse clock, player/firing, shots and enemy bullets, then checks for
completion. If still playing, it releases and updates Eggs, removes ready Eggs
through hatching, updates enemies (including those hatchlings), checks player
collisions, latches hurry-up, then checks completion again. The trigger requires
zero reserve Eggs, zero active Eggs, and at least one active enemy below
`Level.Depth`. The below-rim predicate also guarantees an active enemy exists.
Post-update height matters: an enemy reaching the rim on this tick does not
qualify just because it was below the rim before its update.

The trigger tick's enemy updates use the previous latch value. The next active
tick is the first to use newly activated hurry-up. Pulsars hatch at zero and
reach height one on their hatch tick; Fuseballs hatch at zero and increment
their lateral counter on that tick. Tests pin the last approaching Egg's
39th/40th updates, ordinary trigger-tick behavior and next-tick effects.
If a collision begins PlayerDying on the trigger tick, the end-of-tick latch
can still activate; it then freezes with the established death policy.

The latch starts false, never unlatches during the same-level attempt, and
survives all 60 PlayerDying updates and ordinary fresh-Egg reconstruction.
Replenished reserve/active Eggs after life loss do not clear or recalculate it.
Re-entered enemies receive the existing latch from their first active update:
Fuseballs first double the climb term when they enter climbing; Pulsars first
redirect when they next evaluate a top-end decision. Full Game Over restart
and Level Complete replay clear the latch through level initialization.
Existing restart/replay score and lives policies are unchanged.

Fuseballs still consume the ordinary boundary random roll, then hurry-up
overrides descent with climbing. A climbing update adds the existing source
term twice before clamping (3.3 + 3.3 = 6.6 in this development wave).
Already-descending Fuseballs retain their original speed and duration rules.
Lateral cadence, retargeting, collision windows, score bands and four-frame
five-color animation are unchanged.

Pulsars still consume their top-end random roll, but hurry-up forces the
18-update lane pause / flip branch rather than the descending rim-pause
branch. It does not interrupt an existing pause/descent or alter mid-tube and
bottom decisions. Climb, descent and climbing flips remain one unit per
movement update. The 14-update rim pause, lane transfer, shared 181-update
oscillator, pulsing-lane collisions and existing rendering are unchanged.

Hurry-up itself neither removes nor resolves a unit, awards points, increments
destroyed count nor changes Egg totals. The separate all-rim completion
milestone below now takes precedence after Eggs are exhausted and every active
enemy reaches the rim; below-rim Fuseballs/Pulsars continue to trigger
Hurry-Up.

Automated validation: focused hurry-up/Fuseball/Pulsar selection 121 passed,
0 failed, 0 skipped; full suite 289 passed, 0 failed, 0 skipped. Solution build:
0 warnings, 0 errors. These are headless C# checks, not manual/arcade validation.
Run these separately for Neal's pending manual comparison:

```powershell
dotnet run --project src\Tempest.Game -- --fuseball-test --lives 10
dotnet run --project src\Tempest.Game -- --pulsar-test --lives 10
```

Observe exhaustion of both Egg sources, later boundary/top decisions, death
freeze and fresh-Egg re-entry, final-kill exit, and restart/replay. Exact arcade
trigger/timing, speed and random tuning remain unverified. The behavior
specification is intentionally unchanged pending implementation review,
automated validation and Neal's manual validation. No Super Zapper,
onrim-based exit, new levels, canflip, Tanker-carried Fuseballs/Pulsars, promoted
enemy bullets, new rendering or new scoring are included.

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

## Super Zapper implementation milestone

Implemented after checkpoint `587b9e3`. Super Zapper is enabled in normal
Level 1 and all existing development waves. Space and right mouse use
edge-triggered input separate from held fire. `SuperZapperSystem` owns two
charges and the 60-display-step flash. Initial setup, Game Over restart and
Level Complete replay restore two charges; ordinary life loss preserves the
remaining charge. No charge counter or icons are drawn.

The shared `Enemy.TryZap` contract rejects by default. Flipper, Tanker,
Spiker, Fuseball and Pulsar accept/reassign the base-owned countdown;
EnemyBullet remains unaffected and Spike is not an Enemy. The game enumerates
development entities in their existing insertion order (including rejected
bullets) and uses Flipper creation order in ordinary Level 1, without changing
the reverse Level 1 shot scan. Strength 2 gives accepted targets 4, 6, 8, ...
updates and starts the flash even if all targets reject. Strength 1 follows the
literal Incitti loop: shortest circular distance on the 16-lane tube (direct
distance if non-continuous), strict `distance < bestDistance + 2` gate, then
strictly greater height, strictly smaller distance, or strictly greater source
type number. This preserves the source's executable type comparison even
though its nearby comment says “same d.” The selected target gets delay 4;
with no eligible target the charge is consumed without starting a new flash.

Activation precedes shots and enemy updates. Shots therefore resolve pending
targets first; otherwise every active enemy continues its ordinary behavior
on earlier countdown updates, while expiry resolves and skips behavior on that
visit. Flipper/Pulsar award 150/200, Spiker awards 50 without changing its
persistent spike, and Fuseball uses expiry-height `KillScore` plus its existing
score popup. Enemy bullets continue their existing movement/collision rules.
The source-derived alternating blue/Tanker-magenta tube flash uses
`(displayCounter / 2) % 4 > 1`; the display counter advances during death even
though enemy timers and movement remain frozen.

Tanker zap expiry resolves the original slot as destroyed for 200, then
converts its two latent units into distinct unresolved cargo IDs. Each child
uses the source-adjacent lane/direction and normal Flipping geometry at carrier
height, receives an eight-update timer, and starts that timer on the next
simulation tick. This identity transfer does not change standard
`EnemiesDestroyed`; completion remains blocked until both cargo IDs resolve.
Ordinary shot and natural release still preempt a pending carrier zap using
their established outcomes. Death before carrier expiry restores the
unresolved slot as a Tanker Egg with latent cargo. Death after expiry restores
only unresolved materialized IDs as fresh cargo Eggs; resolved children and
earned score remain resolved. Repeated death/re-entry tests preserve IDs.

One source/project difference is retained deliberately: Incitti randomizes
the initial Tanker cargo side, while the C# development release path chooses a
deterministic first side. This deterministic orientation remains a C#
development decision. Incitti's literal release geometry pivots cargo inward
toward the Tanker lane, but Neal's arcade observation is stronger for the
visible result: both children survive the Tanker shot and appear on distinct
lanes other than the Tanker's. C# therefore uses an outward initial pivot to
keep the former Tanker lane out of both children's immediate collision paths;
the exact intermediate arcade route remains unknown. Below-rim pivots retain
their release direction, and cargo resumes player-directed pursuit at the rim.
Deferred child activation is likewise a project decision because static source
inspection cannot establish whether appended children update in the same pass.
No other contradiction required a design change. Super Zapper details are
**REFERENCE VERIFIED** against Incitti only and are not **ARCADE VERIFIED**.
`TEMPEST-BEHAVIOR-SPEC.md` remains unchanged pending implementation review and
Neal's manual validation.

Automated validation: focused Super Zapper tests 29 passed, 0 failed,
0 skipped; the full suite 318 passed, 0 failed, 0 skipped; solution build
0 warnings, 0 errors. These checks are headless. For manual comparison, run:

```powershell
dotnet run --project src\Tempest.Game -- --lives 10
dotnet run --project src\Tempest.Game -- --spiker-test --lives 10
dotnet run --project src\Tempest.Game -- --tanker-test --lives 10
dotnet run --project src\Tempest.Game -- --fuseball-test --lives 10
dotnet run --project src\Tempest.Game -- --pulsar-test --lives 10
```

Check Space/right-mouse press edges, both charge behaviors, delayed scoring
and ordinary movement, bullet/spike immunity, Tanker stillborn cargo and
re-entry, death charge persistence, restart/replay recharge, and the 60-step
tube flash while preserving Pulsar overlays, spinner feel, cursor capture,
Hurry-Up and existing exit behavior. Manual and Atari comparison remain
pending; audio, charge HUD, new enemy types, Level 3 and broader campaign
progression remain out of scope.

## Tanker-cargo direction correction

The post-Super-Zapper manual validation passed all five existing scenarios.
Separate Tanker-mode testing exposed that released cargo Flippers retargeted the
Blaster during below-rim pivots. The C# correction changes only Tanker-cargo
movement: children continue in a fixed direction through below-rim
pivot/pause cycles and resume player pursuit at the rim. Below-rim cargo pause
updates retain the source's one-unit climb; the deterministic side choice and
deferred activation remain project decisions. Normal Flipper movement, cargo
identity/accounting, eight-update stillborn timers, Super Zapper behavior,
Hurry-Up and completion rules are unchanged.

Neal verified that Atari Tanker cargo does not dynamically track the Blaster
below the rim. Follow-up manual testing found that children could disappear
after a Tanker shot. A deterministic test reproduced a lingering second shot
hitting a child because the first correction's inward destination made the
former Tanker lane part of both cargo collision paths. Neal established that
both children should survive and emerge on different lanes other than the
Tanker's. C# now chooses outward initial pivots; this is a project decision,
not an Incitti-derived route or a claim about the full arcade sequence. Exact
intermediate lanes and timing remain uncertain, and the behavior specification
remains unchanged pending validation of this final geometry.

Automated validation of the follow-up geometry is recorded in
`AI-Logs/Reports/2026-10-30-tanker-cargo-direction-correction-report.md`.
Manual validation of the final cargo geometry remains pending.

## All-rim completion implementation milestone

Implemented after checkpoint `7f6c776`. Neal's observation that exhausted
reserve/incoming Eggs plus all remaining enemies at the rim permit exit is
**ARCADE VERIFIED**. The C# cutoff (`Height >= Level.Depth`, currently 400),
the state-agnostic enemy set, exact update/tick ordering, pending-zap
disposition, and permanent rim-clear handling across Spike reverse/retry are
**REFERENCE VERIFIED / PROJECT DECISION**, not arcade-verified details.
The behavior specification was unchanged at this implementation checkpoint;
its later history records Neal's broad manual validation.

The single qualification check runs only in `Playing`, after Egg release,
Egg update/hatching, enemy/zap updates, pending cargo activation, and all
player/enemy/bullet/spike collisions. It requires no reserve Eggs, active Eggs,
pending cargo Eggs, unresolved latent cargo, or unrepresented Tanker identity;
at least one active Flipper, Tanker, Spiker, Fuseball or Pulsar; and every such
enemy at or above the rim. Enemy bullets and Spike hazards are excluded. The
existing early kill-all check remains after shots/bullets, preserving
same-tick shot-kill behavior.

Normal `EnemiesDestroyed` remains kill-only. `EnemiesRimCleared` separately
tracks non-kill outcomes, `RemainingEnemies` subtracts both dispositions, and
death/retry reconstruction does not recreate rim-cleared units. Tanker
original slots and cargo IDs have distinct rim-cleared sets; latent and queued
cargo fail closed, and aggregate resolved counts remain distinct from
score-bearing killed counts. Rim clear awards no points, creates no Fuseball
kill popup, preserves already-earned score, removes pending zap targets before
future score can resolve, and enters the existing ExitDelay or Spiker
AvoidSpikes path. Spikes remain persistent hazards; the Super Zapper flash
continues through exit. Below-rim Fuseball/Pulsar Hurry-Up remains unchanged,
and an existing latch does not block all-rim exit.

Historical checkpoint validation: all-rim/Hurry-Up 41 passed,
Tanker/accounting 57 passed, Super Zapper 31 passed, Spiker/exit 50 passed;
the full suite had 330 passes and the build had no warnings or errors. Neal
later manually confirmed broad all-rim behavior in all five listed scenarios.
Exact thresholds and internal policies remain scoped as
REFERENCE VERIFIED / PROJECT DECISION in the behavior specification.

Manual validation commands:

```powershell
dotnet run --project src\Tempest.Game -- --lives 10
dotnet run --project src\Tempest.Game -- --fuseball-test --lives 10
dotnet run --project src\Tempest.Game -- --pulsar-test --lives 10
dotnet run --project src\Tempest.Game -- --tanker-test --lives 10
dotnet run --project src\Tempest.Game -- --spiker-test --lives 10
```

Neal later manually confirmed the broad all-rim behavior in each listed
scenario. Exact thresholds, update ordering, pending-zap handling, accounting
and Spike-retry rules remain reference/project details as scoped in the
behavior specification. The commands remain useful as a regression checklist.

## Level 1 configuration groundwork

`LevelConfiguration.Level1` describes internal/displayed identity 0/1,
board/palette 0/0, closed Level 1 geometry, player start, ten-unit supply,
Egg pacing, standard exit policy and Flipper-only composition.
`LevelConfiguration.Level2Square` is the single immutable production Level 2
configuration; `--level2-test` selects that same object for direct development
access. It retains board 1/palette 0, the closed 16-lane square, 12-unit
supply, Flipper-only composition, and the validated B1/B2 hinge/canflip path.

Normal play advances from Level 1 only after standard ExitZoom travel updates
the player to depth below 10. Wave clear, ExitDelay, death, Game Over and
Spiker reverse/retry do not advance. The existing fresh-wave initializer
installs Level 2 in that same simulation update, resetting projection and
Super Zapper to two charges/zero flash while carrying score and current lives.
No level bonus, bonus life, source `startbonus`, Begin/Ready presentation,
entry-position shift or audio is implemented. These are presentation/economy
omissions, not arcade-equivalent behavior.

Natural Level 2 is terminal because Level 3 is unavailable. It displays
"LEVEL 2 COMPLETE / NEXT LEVEL NOT IMPLEMENTED"; R starts a new Level 1
campaign with score reset and configured starting lives. Game Over in a normal
campaign also restarts at Level 1. Direct `--level2-test` completion replays
Level 2 with its existing score/lives semantics. Spiker, Tanker, Fuseball and
Pulsar development scenarios remain terminal and replay their own launch
scenario. Ordinary death reconstructs the existing `Level`, preserving its
configuration and resolved outcomes.

Definitions distinguish 16 ordered vertices from playable lanes: the current
closed Level 1 has 16 of each, while a geometry-only open-definition fixture
has 16 vertices and 15 lanes. This does not claim full open-tube gameplay.
The current four-Egg cap, release cadence and deterministic sequences are
project decisions, not literal Incitti behavior.

Fresh initialization detaches the old `SpikeCreated` callback and binds one
callback to the replacement runtime. The random source and shared pulse phase
continue across fresh replay as before; Fuseball animation resets on fresh
initialization and carries through life reconstruction. No behavior-spec
change or progression assertion was made for this groundwork.

## Isolated Level 2 square/canflip test

`--level2-test` is **DEVELOPMENT ONLY** and selects a closed 16-lane square
definition, board index 1, palette 0 and configured supply 12. It uses the
existing `Level` runtime and fresh-wave installation path. B1 established the
square endpoint-hinge geometry with canflip disabled. B2 enables ordinary
Flipper canflip only through this installed configuration; Level 1 retains
straight climbing and its legacy rim geometry and direction policy.

**REFERENCE VERIFIED**: the first climb adds 2, enters a zero-duration lane
pause, attempts firing, then samples the Blaster lane. The next update adds
1 and starts an adjacent pivot. Turning adds 8 degrees and 2 height; completed
turns pause for 21 updates below the rim or 24 at the rim. Below-rim pauses add
1 height and retain the initial direction across successive turns; only rim
pause expiry resamples the Blaster. Same-lane ties use the existing injected
random source; signed antipodal choices follow Incitti. Bullet attempts occur
on climbing and pause expiry, not during turns. Bullets remain separately
opt-in, with the existing 1-3 ammunition and 16 timer.

**PROJECT DECISION**: preserve B1's clamped height, source-anchored hinge
representation, two-lane pivot shot acceptance and completed-pivot safety.
Incitti can briefly overshoot depth, assigns its lane at pivot start, accepts
shots only on that destination, and can kill before angular completion.
A lane pause newly reaching the occupied rim signals arrival; merely moving
under an already-paused rim Flipper does not introduce a new collision policy.
The shared literal direction helper was extracted from Pulsar without changing
its behavior. At this B2 checkpoint, Level 1 still used its legacy turn
geometry; C1.5 later replaced it with the shared directed endpoint geometry.
That geometry and its inward-pivot correction were manually accepted by Neal
on 2026-10-31 as recorded in the C1/C1.5 section below. Tanker cargo keeps
its separate direction and 24-update below-rim pause.

Neal completed B2 manual validation on 2026-10-31. **ARCADE VERIFIED**:
below-rim lane changes broadly resembled Atari Level 2; straight-side and
corner flips looked mechanically sensible with no snapping or wrong-lane
landing. Active transitions did not visibly retarget halfway through after
Blaster movement; later transitions may respond to its then-current position.
Below-rim transition geometry was not lethal, and rim pursuit and completed-
pivot behavior remained normal. No crash occurred and the B1 completed-pivot
renderer crash did not recur.

These observations do not measure the exact 8-degree increment, 21/24 pauses,
random/tie rules or call ordering, tick boundaries, height increments, endpoint/
radial math, numeric shot band or internal state representation. Those remain
**REFERENCE VERIFIED** or **PROJECT DECISION** as applicable. Passing tests
still proves C# contracts, not arcade fidelity. The prior qualitative
observations covered isolated Level 2 gameplay, not the end-to-end natural
transition or its setup.

### Natural Level 1-to-Level 2 manual validation

On 2026-10-31, Neal manually ran:

```powershell
dotnet run --project src\Tempest.Game -- --lives 10
```

He confirmed the normal Level 1 tube exit leads directly into the square Level
2 without an intervening Level Complete/R pause. The displayed identity
changed, square geometry appeared immediately, score and current remaining
lives carried unchanged, four initial Eggs were visible, the wave had 12
units, Super Zapper had two usable charges, and validated square/canflip
behavior remained normal with no observed regression. In the observed run,
10 starting lives became 8 after two Level 1 deaths and Level 2 started with
8; this example is not a fixed life-count rule. These broad observable results
are **ARCADE VERIFIED** within Neal's manual comparison. Exact transition
ticks were not measured.

Do not promote implementation details to arcade observations: the exact
`Player.Depth < 10` commit boundary, update ordering, fresh-runtime and
accounting reset, exact Super Zapper recharge/flash timing, Egg setup
sequencing, player start offset, callback/list cleanup and projection reset
remain **REFERENCE VERIFIED / PROJECT DECISION** as applicable.

### Deferred Atari between-level approach

Neal reports that after the Blaster exits a tube, Atari shows a brief
first-person approach through space toward the next tube. The next tube starts
small and distant and grows as the player approaches; the Blaster is visible
on the distant rim and spinner movement moves it around that rim during the
approach. Combat/enemy engagement begins after arrival. This is
gameplay-relevant because rim movement remains interactive, but Neal does not
consider its absence a blocker for current progression development. The
approach presentation is deferred and not implemented. Do not infer duration,
zoom rate, projection formulas, enemy-start timing or firing enablement; none
was measured.

Neal also verified the current Level 2 terminal policy: no Level 3 appears,
the explicit "LEVEL 2 COMPLETE / NEXT LEVEL NOT IMPLEMENTED / PRESS R TO
START A NEW CAMPAIGN" presentation is shown, and R starts a fresh Level 1
campaign with score reset. This terminal policy is a **PROJECT DECISION**,
not a claim that Atari Level 2 is terminal.

```powershell
dotnet run --project src\Tempest.Game -- --level2-test --lives 10
```

Level 1 remains the normal no-flag startup, and normal Level 1 now progresses
to the natural Level 2 configuration after successful exit travel. The direct
development flag remains terminal and replayable; it is mutually exclusive
with the Spiker, Tanker, Fuseball and Pulsar development scenarios. `--lives`
and the separately opt-in `--enemy-bullets-test` remain orthogonal and do not
disable normal progression.

## Isolated Level 3 plus-board geometry diagnostic

`LevelConfiguration.Level3Plus` is one source-derived configuration object for
the closed 16-vertex/16-lane `Level 3 - plus` board (board 2, palette 0,
center Y 400, Y offset -80). It records internal/displayed identity 2/3,
depth/position 400/50, player lane 7 at offset 0.5, configured supply 15,
four initial Eggs and eleven reserve units. Its deterministic spawn sequence
is a C# diagnostic policy, not the Incitti Level 3 Egg schedule or an
arcade-verified schedule.

`--level3-geometry-test` is **DEVELOPMENT ONLY**. It selects this same
configuration for a controlled Flipper-only geometry run; this is not a
production Level 3 mixed wave and does not claim production composition.
Flippers use the existing early-level values (climb 2, turn 8, below-rim pause
21, rim pause 24) with canflip enabled. The endpoint-hinge geometry mode is
shared with Level 1, Level 2 and the plus-board diagnostic. A directed
transition retains its source and destination lanes, selected direction,
shared hinge, and full sweep magnitude for duration. Its C1.5
`SignedSweepDegrees` remains separate from `SpatialSweepDegrees`, which selects
the inward side from polygon winding and the local convex/reflex turn. The
rendered free endpoint is also limited to the first polygon boundary along
that inward ray, preventing narrow concave geometry from crossing the far rim.
This preserves full 90/180/270 sweeps while moving the glyph into the tube/web.
The plus board's straight runs remain 180 degrees, outer corners 90 degrees,
and reflex corners 270 degrees, including the 15/0 wrap. The Flipper renderer
and update timer consume the same transition progress; its configured
8-degree step takes 12, 23, or 34 updates for 90, 180, or 270 degrees.

Pulsars retain the selected source/destination transition and use the same
inward spatial geometry; their 12-degree step therefore takes 8, 15, or 23
updates at those angles. Their logical `EdgeIndex` changes when the lane pause
expires, before the rendered pivot completes; the active transition preserves
both endpoints. The Blaster keeps its existing input signs and exact-boundary
lane convention, but normalized movement is interpreted as angular distance
(`movement * 180 degrees`) and traversal is weighted by the lane's hinge angle.
It follows the rim and does not use the enemy inward-pivot transform. Tanker
cargo keeps its project-selected outward lane route, fixed below-rim direction,
identity, release accounting, and next-tick activation; its initial and later
pivots use the inward spatial arc, retain the full sweep, and can reach the rim
before a long pivot finishes. The visible geometry and timing behaviors
described in the C1/C1.5 acceptance section below are **ARCADE VERIFIED** only
to the extent Neal observed them; exact source constants and internal state
remain reference/project details. Cargo remains unavailable in this diagnostic.

The flag is mutually exclusive with `--level2-test`, `--spiker-test`,
`--tanker-test`, `--fuseball-test` and `--pulsar-test`. `--lives` and the
optional `--enemy-bullets-test` remain composable. The diagnostic is terminal:
R replays the plus-board setup, Game Over restart returns to it, and life
reconstruction retains its Level configuration. It uses the existing
configured-supply accounting only; no production cargo ledger is added.

Natural campaign progression remains Level 1 -> Level 2 and terminal after
Level 2. Level 3 mixed-wave accounting, production Tanker composition and
Tanker firing remain deferred to later C2/C3/C4 work. The exact board is
**REFERENCE VERIFIED** from Incitti; manual validation establishes only the
specific visible behaviors Neal observed. The earlier C1 manual rejection led
to the C1.5 directed-rotation correction and inward spatial correction. Neal
completed manual C1/C1.5 validation on 2026-10-31 and passed Level 1, Level 2,
Level 3 plus-board, Pulsar and Tanker observations. The behavior specification
now records those observations as **ARCADE VERIFIED**; exact timing constants,
complete Atari cargo routes, internal C# geometry and development diagnostic
composition are not thereby arcade-verified.

### C1/C1.5 manual acceptance — PASS (2026-10-31)

Neal manually passed the combined Level 3 geometry, directed-rotation and
inward-pivot checkpoint:

- Normal Level 1 and Level 2 square Flippers swing inward through the web.
- On the Level 3 plus-board diagnostic, convex 90-degree and reflex
  270-degree pivots swing inward; the reflex route takes the full long route,
  not the opposite 90-degree shortcut. 180-degree transitions look sensible,
  and Flippers land on the correct lane.
- Blaster direction and movement feel correct during Level 3 validation. The
  longer 270-degree enemy transition preserves an opportunity to shoot or
  move underneath. No outward arc, snapping, strange tube crossing or crash
  was observed.
- Pulsars change lanes inward through the web with sensible direction and
  geometry-dependent duration. Pulse visuals and hazard remain normal; no
  unrelated regression or crash was observed.
- Tanker destruction releases two cargo Flippers on distinct non-Tanker lanes.
  Their established outward **lane route** remains correct while their visible
  pivot swings inward. Below-rim direction remains stable; rim arrival and
  subsequent behavior look correct, with no new collision problem or crash.

Keep the properties separate: destination/lane direction selects the route;
the full directed sweep determines its duration; spatial pivot side determines
whether the enemy arc swings inward through the web. The observed 270-degree
route must retain its long sweep and duration rather than normalize to 90
degrees. Blaster movement follows the rim and is not part of the enemy inward
transform.

This is **ARCADE VERIFIED** for the listed visible observations only.
Implementation data structures, polygon winding/interior math, source-derived
rates, exact tick counts, the complete Atari Tanker-cargo route, and the
diagnostic's artificial Level 3 composition remain **PROJECT DECISION**,
**REFERENCE VERIFIED**, or **DEVELOPMENT ONLY** as applicable. C1/C1.5 is
manually accepted; no C2 work is authorized by this acceptance.

## Deferred manual observations outside Stage A

- **Tanker firing fidelity:** `--tanker-test` alone remains silent;
  `--tanker-test --enemy-bullets-test` enables Tanker firing. Neal observed one
  Tanker shot in a run but could not assess whether a second would occur
  before release/rim arrival. His arcade experience is that Tankers fire
  individual shots at intervals and at least two can occur before reaching
  that area. This predates Stage A and is not a Stage A regression. Do not
  infer exact ammunition, interval, random probability, initial delay or
  height cutoff; current exact values remain Incitti-derived unless measured.
- **Blaster rim movement:** movement works, but Neal perceives a very small
  delay between lane movements compared with the arcade spinner. Cause is
  unknown and unestablished. This is not a Stage A blocker; revisit before
  higher-level gameplay where tighter timing may affect playability. No
  movement or input tuning was part of Stage A.
