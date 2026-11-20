# Spiker and Spike Implementation Report

## 1. Summary of Implementation

Implemented an explicit Spiker development wave on the existing Level 1 tube.
Normal startup still selects the validated Flipper-only Level 1, without initial
spikes or changes to its spinner controls, Flipper behavior or exit sequence.
No additional enemy types, levels, packages or frameworks were introduced.

The development wave integrates Spiker hatching, building and retreat, persistent
lane spikes, shot collisions/scoring, mixed-enemy accounting, rendering, spike
avoidance and reverse-zoom retry. All gameplay simulation remains outside Program.
The graphical game was not automatically launched; visual success is not claimed
on the strength of headless tests alone.

## 2. Source-Backed Behaviors Implemented

- Spikers hatch on the Egg's current lane at height zero; the first update only
  changes the lifecycle state. They remain in that lane.
- Building adds the assigned speed while the pre-update height is below depth
  minus four. Existing lane spikes are reused: when the builder exceeds their
  height, their height is assigned to builder height minus four. Missing spikes
  are recreated at the builder's height.
- Growth greater than 100 permits a controllable random retreat decision using
  the reference threshold. Reaching the upper boundary also causes retreat.
- Retreat subtracts speed times 0.90. An update beginning at height zero or below
  removes the Spiker and returns exactly one enemy to pending Egg supply, with
  no score or destroyed-enemy increment.
- A same-lane shot strictly below Spiker height consumes the shot, destroys the
  Spiker and awards 50 points without removing its spike.
- A same-lane shot strictly below spike height consumes the shot, removes exactly
  ten height units and awards one point. Height zero or below deletes the spike.
- Development collision dispatch uses insertion order, including replacement
  spikes created after their builders, not proximity. Default Level 1 retains
  its established reverse-order Flipper scan.
- The development scenario starts height-60 spikes on all 16 lanes. Spikes are
  not enemies; they persist after builder destruction, retreat and life loss,
  and do not prevent level clear. Full restart/replay restores initial spikes.
- Hatch selection permits only Spikers and Flippers. It retains the source's
  Spiker bias, maximum four active Spikers, and exclusion when active enemies
  plus active Eggs is below four; reserve Eggs are excluded from that expression.
- Green spirals use the reference segment geometry and perspective scale.
  Rotation advances 45 degrees per simulation update, matching this game's one
  update per displayed frame. Spikes are green lines with white tips.
- Spike-enabled clear uses 151 avoidance updates, then 21 delay updates and the
  existing forward-zoom changes. Movement/firing remain available while alive;
  shots originate at current travel depth. Travel processes depth, existing shots,
  spike collision, then input locally, without replacing normal gameplay order.
- Same-lane spike height greater than player depth starts reverse zoom. Projection
  restores with K minus four (minimum 70) and offset plus one (maximum five),
  followed by the source-style depth snap and retry. A life is deducted once after
  the full established 60 death updates; an early reverse restoration cannot
  cancel that life loss. No reserve or active enemies are duplicated by retry.

## 3. Files Changed

Modified:
- README.md
- TEMPEST-DEVELOPMENT-NOTES.md
- src/Tempest.Game/Debug/DebugController.cs
- src/Tempest.Game/Gameplay/Player.cs
- src/Tempest.Game/Gameplay/Spike.cs
- src/Tempest.Game/Gameplay/Spiker.cs
- src/Tempest.Game/Levels/Level.cs
- src/Tempest.Game/Program.cs
- src/Tempest.Game/Rendering/VectorRenderer.cs
- src/Tempest.Game/TempestGame.cs
- tests/Tempest.Game.Tests/SimulationTests.cs

Created:
- AI-Logs/2026-10-30-spiker-implementation.md

The previously untracked AI-Logs/2026-10-30-spiker-source-analysis.md was preserved
unchanged. Reference/incitti remains unchanged. Flipper.cs, InputSystem.cs,
Projection.cs, level geometry, packages and target frameworks were not changed.
No commits, pushes, resets or discarded changes were performed.

## 4. Development-Wave Activation Instructions

Run the explicitly opted-in scenario:

```powershell
dotnet run --project src/Tempest.Game -- --spiker-test
```

Without the flag, normal Flipper-only Level 1 remains the default. The scenario
is development-only, not arcade Level 1 or a full level-selection system.

Expect green lane spikes with white tips immediately, followed by incoming red
Eggs hatching into green rotating Spikers or red Flippers. Spikers build/extend
spikes and retreat, returning to the pending supply. Hold Ctrl or use the existing
arcade/mouse fire button to shoot bodies or shorten spikes. The HUD labels the
remaining supply ENEMIES and includes both active enemy types in ACTIVE.

After defeating all ten enemies, move and fire during avoidance and travel to
clear or avoid spikes. A spike hit costs one life and reverses the zoom; surviving
lives permit an exit retry with spikes preserved. Completion stops at the current
Level Complete display. R replays there or restarts at Game Over. Omit the flag
on the next launch to return to normal Level 1.

## 5. Automated Build/Test Results

- Baseline: dotnet build passed; dotnet test reported 54 passed, zero failed,
  zero skipped. The source-analysis log was already untracked before this work.
- Final: dotnet build passed without warnings; dotnet test reported 81 passed,
  zero failed, zero skipped. No editor diagnostics were reported.
- git diff --check passed. Source changes and git diff --stat were reviewed for
  unintended scope. git diff --exit-code -- Reference/incitti returned success
  with no changes.
- Coverage includes source lifecycle boundaries, deterministic retreat decisions,
  spike creation/reuse/persistence, strict hit boundaries, insertion-order conflicts,
  shot consumption/scoring, mixed-enemy accounting, full development-wave completion,
  reverse-zoom collision/retry/Game Over/replay, and finite render submissions.
- The original 54 regression cases continue to pass. No tests open a Raylib window.

## 6. Manual Validation Checklist

- Run without the flag: verify normal tube, Egg wave, spinner sensitivity/direction,
  cursor capture, fire controls, Flipper pivots/shootability, deaths and exit timing.
- Run with the flag: verify visible green rotating spirals, green lane lines and
  white tips, spike growth/reuse, retreat and persistence after body destruction.
- Verify body kills score 50, spike hits score one and visibly shorten spikes.
- Defeat the wave: verify avoidance controls, firing during travel, visible residual
  spikes, lane/depth hazard timing, reverse-zoom presentation and one-life loss.
- Retry in a safe/cleared lane; verify completion, replay and Game Over restart.
- Compare appearance, motion pacing, initial spike placement, damage/scoring and
  avoidance/reverse-zoom behavior with original Atari Tempest gameplay.

## 7. Known Limitations and Deliberate Omissions

- Development Spikers have an assigned speed of three. BlitzMax's fractional
  Rand(2.5,4.5) coercion remains unresolved; no matching random speed distribution
  is claimed. Random.Shared varies production hatch/retreat decisions; tests inject
  a controllable Random. Geometry, enemy supply and avoidance duration are an
  explicit development scenario, not full arcade progression.
- The source's optional retreat bullets are deliberately omitted, along with
  Tankers, Fuseballs, Pulsars, other enemy types, audio, bonuses and later levels.
- Reverse zoom intentionally guarantees the established 60-tick life deduction
  rather than copying the reference's potential early cancellation.
- Manual rendering validation and original-arcade comparison remain pending.
  Headless render callbacks verify geometry/submissions, not native framebuffer
  appearance, smoothness or physical arcade controls.

## 8. Todo Reconciliation

- Complete: reference review and baseline recording.
- Complete: Spiker lifecycle, Spike behavior and controllable retreat decisions.
- Complete: opt-in development wave, hatching, collision ordering and accounting.
- Complete: spike-specific avoidance, reverse zoom, life loss and exit retry.
- Complete: rendering, documentation, focused tests and regression validation.
- Complete: source-diff review, reference-safety check and implementation report.
- Pending manual validation: visual gameplay, spinner/cursor checks and arcade
  comparison listed above; not performed by automated tools.
- Deferred: exact BlitzMax fractional Rand speed coercion verification.
- Intentionally out of scope: retreat bullets, other enemies, bonuses, audio,
  full progression and additional levels. No implementation work remains in progress.