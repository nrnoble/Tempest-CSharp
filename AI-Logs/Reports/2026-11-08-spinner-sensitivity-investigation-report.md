# Spinner Sensitivity Investigation

Date: 2026-11-08
Scope: Read-only investigation; no production implementation.

## Executive findings

- The live mouse path is `Raylib.GetMouseDelta().X` -> `PlayerInput.MouseDeltaX` (a `float`) -> `TempestGame.UpdatePlayerMovement`. The only current spinner scalar is `SpinnerSensitivity = 0.0125f`.
- One Raylib delta unit therefore becomes `0.0125` movement units, then `2.25` degrees of the player's rim movement (`0.0125 * 180`). The angle required to cross a lane is derived from that lane's geometry. For example, the existing square-board tests exercise 90-degree and 180-degree transitions, equivalent to about 40 and 80 delta units when starting at a boundary. This is not a conversion from USB encoder counts; the device's actual Raylib deltas were not measured.
- A large delta can cross multiple lanes in one simulation call. `Player.MoveAlongTube` carries the remaining angular movement through repeated lane transitions, wraps on closed tubes, and stops at open endpoints. There is no mouse dead zone, acceleration, smoothing, per-tick clamp, or project-side queue. The player's float `EdgePosition` retains sub-lane travel.
- The code explains how multi-lane jumps can occur, but not whether the current scalar, Windows pointer processing, or Neal's particular device is the dominant cause. The observed device delta magnitude and raw-vs-processed Windows path are **UNKNOWN / REQUIRES MEASUREMENT**.
- Recommend a future `--spinner-sensitivity <multiplier>` option applied only to live mouse input, with default `1.0`. It would scale movement magnitude, not direction. A bounded `0.1` through `4.0` range is a practical calibration proposal, not an arcade-derived value. Do not add a per-tick clamp without separate evidence.
- Recordings currently capture the per-tick `PlayerInput`, including the unscaled `MouseDeltaX`; playback feeds those frames directly to the simulation. For the future option, scale live input before both simulation and recording. Then new recordings contain effective per-tick mouse input, the recording schema need not gain a sensitivity field, and playback does not use the machine's current setting.

## 1. Starting repository state

- Branch: `master`, tracking `origin/master`.
- HEAD: `d5ccbe1d3ef8af21a02bd53c8c465ed0c4908fef` (`Close out accepted production Level 8`), matching the requested accepted baseline.
- Initial index and tracked worktree were clean; no ahead/behind divergence was shown.
- Initial untracked state: 21 files under `Reference/arcade/roms.rev2/`. They were present before this investigation and were read only. No other untracked paths were reported.
- `AI-Logs/` is ignored by `.gitignore:6`; the prompt and report created for this task are the only authorized additions.

## 2. Inspected evidence

### C# implementation

- [InputSystem.cs](../../src/Tempest.Game/Systems/InputSystem.cs#L1): `PlayerInput`, mouse acquisition, keyboard and controller input, controller dead zone.
- [Program.cs](../../src/Tempest.Game/Program.cs#L150): cursor mode, `--speed` target FPS, live-input/playback selection, one-step loop, recording capture.
- [TempestGame.cs](../../src/Tempest.Game/TempestGame.cs#L25): current movement constants and movement composition.
- [Player.cs](../../src/Tempest.Game/Gameplay/Player.cs#L62): continuous lane position, angular conversion, repeated transitions, topology endpoints.
- [Level.cs](../../src/Tempest.Game/Levels/Level.cs#L30): clockwise winding, adjacent-lane rules, geometry-derived lane transition angle.
- [DebugController.cs](../../src/Tempest.Game/Debug/DebugController.cs#L121) and [InteractiveSpeedOptions.cs](../../src/Tempest.Game/Debug/InteractiveSpeedOptions.cs#L5): CLI parsing conventions and speed bounds.
- [PlayerInputRecorder.cs](../../src/Tempest.Game/Systems/PlayerInputRecorder.cs#L23), [GameplayRecordingFile.cs](../../src/Tempest.Game/Debug/GameplayRecordingFile.cs#L32), [PlayerInputReplay.cs](../../src/Tempest.Game/Systems/PlayerInputReplay.cs#L1), [GameplayPlaybackSession.cs](../../src/Tempest.Game/Debug/GameplayPlaybackSession.cs#L43), and [HeadlessGameplayPlaybackCli.cs](../../src/Tempest.Game/Debug/HeadlessGameplayPlaybackCli.cs#L142): input serialization and replay.
- The installed Raylib-cs 8.1.0 XML API documentation describes `GetMouseDelta` as “Get mouse delta between frames” (`%USERPROFILE%\.nuget\packages\raylib-cs\8.1.0\lib\net8.0\Raylib-cs.xml:621-622`).

### Tests and project references

- Inspected [GameplayInputTests.cs](../../tests/Tempest.Game.Tests/GameplayInputTests.cs), [InputSystemTests.cs](../../tests/Tempest.Game.Tests/InputSystemTests.cs), [Level8ProductionTests.cs](../../tests/Tempest.Game.Tests/Level8ProductionTests.cs), [DirectedRotationInvestigationTests.cs](../../tests/Tempest.Game.Tests/DirectedRotationInvestigationTests.cs), and [GameplayRecordingTests.cs](../../tests/Tempest.Game.Tests/GameplayRecordingTests.cs).
- The tests cover deterministic tick input, Level 8 clockwise/counter-clockwise mapping, endpoint clamping and reversal, and recording of input fields. `DirectedRotationInvestigationTests` explicitly demonstrates that lane-transition geometry affects how much movement is needed to cross a lane.
- Read the CLI and playback descriptions in [README.md](../../README.md#L145) and the project-local Incitti and arcade references below.
- No tests, builds, or interactive game were run. This was a read-only investigation with no code changes to validate.

## 3. Complete current C# input path

1. `Program` disables the cursor with `Raylib.DisableCursor()` and chooses the host-loop target FPS from `--speed`. It does not configure a mouse scale, filter, acceleration curve, raw-input flag, or device-specific mapping.
2. On each live-input loop iteration, `InputSystem.ReadPlayer()` calls `Raylib.GetMouseDelta().X` once. The result is a `float` frame-to-frame X delta. Y and wheel are not used for Blaster movement. Raylib's wrapper documentation identifies a between-frame delta; it does not document USB encoder counts or prove the underlying Windows acquisition mode.
3. `ReadPlayer` places that float unchanged into `PlayerInput.MouseDeltaX`; there is no intermediate `RawPlayerInput` type or mouse-specific transform. It also reads held Left/Right keys and the controller's left-X axis. The controller axis alone has a `0.15` dead zone.
4. `Program` passes the resulting `PlayerInput` to one `game.RunOneStep(...)` call. For live recording, it then captures that same input for the simulation tick.
5. `TempestGame.UpdatePlayerMovement` computes:

   `spinnerMovement = MouseDeltaX * 0.0125f * CurrentLevel.ClockwiseLaneIndexDelta`

   Mouse movement therefore scales with the delta magnitude. The winding sign adjusts lane-index direction so the established physical clockwise/counter-clockwise convention survives different board winding.
6. Keyboard and controller movement are added independently: held Left subtracts `0.05`, held Right adds `0.05`, and the controller contributes `ControllerAxis * 0.05`. Those are movement units per simulation call, not mouse counts.
7. `Player.MoveAlongTube` converts the combined scalar into `abs(movement) * 180` degrees. At the current mouse multiplier, one Raylib delta unit contributes `0.0125` movement units or `2.25` degrees. Each lane boundary consumes a geometry-derived angle from `Level.CalculateLaneTransitionAngle`; consequently, the same delta does not correspond to a fixed fraction of a lane on every transition.
8. `EdgePosition` is a float fraction along the current lane. Movement within a lane is retained there; movement past a boundary carries the remaining angle into the next lane. No integer conversion discards small or fractional mouse values.

No separate per-tick mouse accumulator or remainder exists. The frame delta is consumed once when read by live gameplay. Within that call, however, lane-position remainder is retained as `EdgePosition`. If input would continue beyond an open endpoint, the Blaster is clamped at the endpoint and the excess outward movement is discarded.

## 4. Sensitivity, geometry, and multi-lane movement

- The sole current mouse response constant is `TempestGame.SpinnerSensitivity = 0.0125f`. It is applied in `UpdatePlayerMovement`, after the raw Raylib delta has entered `PlayerInput`.
- No mouse dead zone, acceleration, smoothing, filtering, quantization, maximum delta, or clamp is applied in the C# path. `InputSystem.ControllerDeadZone = 0.15f` applies only to the controller axis.
- The mapping is exactly `2.25` movement degrees per Raylib delta unit before lane-boundary geometry is considered. A transition of angle `A` degrees takes `A / 2.25` delta units from a boundary at sensitivity 1.0. As concrete test geometry, transitions of 90 and 180 degrees correspond to about 40 and 80 units respectively. A regular 16-sided Level 1 edge turn is approximately 157.5 degrees, or about 70 units from a boundary. These are project-coordinate calculations, not device counts.
- `Player.MoveAlongTube` uses `while` loops to process all the movement. A sufficiently large single frame delta can cross several lane boundaries in that one simulation call; the crossing order and leftover offset are preserved. There is no per-call maximum. On closed tubes the lane lookup wraps. On open Level 8 it cannot cross either endpoint.
- Therefore, the exact code-level reason a delta can make a multi-lane jump is the unbounded magnitude reaching a loop that deliberately consumes all movement. The current `0.0125f` scalar contributes to the magnitude, while geometry sets lane cost. Whether the physical spinner produces a “small” or “large” Raylib delta on Neal's setup is not statically knowable. This investigation cannot choose between a scalar that feels too high and a large already-processed device delta as the dominant real-world cause.
- A fractional multiplier is compatible with the current float pipeline: there is no integer cast between acquisition and movement, and `EdgePosition` retains sub-lane travel. For example, multiplying a one-unit input by `0.5` yields `0.5` input units and `1.125` degrees, rather than zero. Deterministic scaling adds no smoothing or time-dependent state.

## 5. Sampling cadence and `--speed`

- The game targets 60 FPS by default. `Program` calls `ReadPlayer`, then `RunOneStep`, then increments the simulation tick once per loop iteration; there is no separate fixed-step catch-up accumulator that consumes one mouse sample across several simulation ticks.
- `--speed` changes the requested host-loop rate to rounded `60 * multiplier`: `0.5` requests 30 FPS, `1` 60 FPS, and `2` 120 FPS. It changes the rate at which the whole simulation advances; it does not multiply spinner sensitivity.
- For the same monotonic total of Raylib delta units, the linear mouse mapping has no explicit per-second scaling, so merely partitioning that total into more samples does not change the ideal total rim travel. In practice `--speed` changes sample grouping and the gameplay cadence. Opposite movements combined between samples can net differently, endpoint clamping discards outward overflow, and Windows/device processing may be cadence-dependent. Those effects prevent a static guarantee that a physical motion produces identical travel at all speeds.
- Keyboard and analog-controller distance per real-world second does change with the simulation tick rate because their movement is per tick. The sensitivity feature should not compensate for or modify `--speed`.
- Future manual calibration should compare normal speed with reduced sensitivity, and reduced sensitivity at `--speed 0.5` and `--speed 2`. The option should state that it scales each live frame delta and does not normalize for FPS.

## 6. Direction, keyboard, controller, and Level 8

- `Level` computes `ClockwiseLaneIndexDelta` from polygon winding. `TempestGame` applies that sign only to mouse movement; keys and controller remain screen/lane-index controls. A future sensitivity multiplier must preserve this ordering and sign behavior.
- Existing Level 8 tests establish that positive mouse rotation is clockwise and visually toward the left endpoint; negative rotation is counter-clockwise and visually toward the right. They also verify repeated outward input remains clamped at both open endpoints and reversal moves inward. Closed Level 7 tests verify wraparound.
- Sensitivity should affect only the live horizontal mouse delta that acts as spinner input. Keyboard `0.05` movement, controller `axis * 0.05` movement and its `0.15` dead zone, playback, and replay reconstruction should be unchanged. An analog controller is not part of the mouse delta path and should not be affected.
- `InputDiagnostic` disables the cursor and reports keys and mouse-button states, but does not log `GetMouseDelta`; it cannot currently measure Neal's spinner deltas.

## 7. Windows and device uncertainty

- The project receives a Raylib frame-to-frame X delta; it does not identify a USB device, query its vendor/product identity, or access an encoder-count API.
- The game and input diagnostic disable the cursor, but the inspected C# code does not explicitly enable raw mouse motion or call a Windows Raw Input API. The managed `GetMouseDelta` documentation alone does not establish whether the installed native Raylib/backend path on this Windows machine bypasses Windows pointer speed or Enhance Pointer Precision.
- If the native delta is derived from OS-processed cursor movement, pointer speed/acceleration may materially affect it; if it is raw relative input, those settings may not. Which path Raylib 8.1.0 uses in this exact configuration, and how the spinner driver presents motion, are **UNKNOWN / REQUIRES MEASUREMENT**. Do not change Windows settings or infer raw behavior from `DisableCursor`.
- The user's observation that the USB spinner is routed through the game's mouse path is recorded as **MANUALLY OBSERVED**; the exact device-to-Raylib conversion remains unknown.

## 8. Recording and playback behavior

- Today, recordings store per-tick input fields, including `MouseDeltaX`, keys, controller axis, Fire, and Zapper. They do not store the final lane transition or resulting player position as input. The file's startup metadata stores level, lives, rim-speed multiplier, and random seed; it has no mouse-sensitivity field.
- At present `MouseDeltaX` is captured before the `0.0125f` scalar, so it is the Raylib delta sample, not the scaled movement. Playback supplies each saved `PlayerInput` directly to the same `TempestGame` movement method and does not read a physical mouse.
- Visible playback uses `GameplayPlaybackSession`; its pause and navigation controls are handled outside `PlayerInput`, forward steps consume recorded frames, and backward navigation reconstructs from startup then replays to the selected boundary. No live `ReadPlayer` input is used for those steps. Headless playback uses that same deterministic session. The headless CLI accepts only `--boundary` and `--json` after the recording path; unknown or incompatible options are rejected. `--speed` is also documented as rejected for headless playback and is not serialized.
- Applying a new sensitivity in `TempestGame.UpdatePlayerMovement` would rescale saved inputs during playback and make existing/new playback depend on the current option. Instead, apply the multiplier only to live input acquisition before `PlayerInput` is sent both to simulation and `PlayerInputRecorder`. Playback must bypass that acquisition scaling.
- Under that design, historical recordings remain compatible at the default multiplier 1.0. New recordings retain the existing schema but store the sensitivity-adjusted per-tick `MouseDeltaX` as effective input. Playback then uses the recorded value and the fixed existing gameplay scalar; it does not need the machine's current sensitivity setting. No recording-schema field is required.
- Recommend rejecting the option as inapplicable for headless playback (the current parser is strict and headless playback has no live mouse). Apply it to live `--record-gameplay` sessions. For visible `--playback-file`, reject it as an inapplicable live-input override rather than silently accepting a no-op. `--input-diagnostic` also has no gameplay movement to calibrate and should not silently claim to apply it.

## 9. Mark Incitti secondary engineering reference

In [tempest.bmx](../../Reference/incitti/tempest.bmx#L124), `PLAYERSPEED` is `0.25` normalized edge position per `Player.Update`. At [lines 639-720](../../Reference/incitti/tempest.bmx#L639), the game reads `MouseX() - 400`, recenters with `MoveMouse(400,300)`, and uses the sign for direction. Only when `abs(mousemove) > 16` does it add `abs(mousemove) / 64`, capped at `1`, to the per-update base speed.

This is not a linear raw-delta multiplier: any nonzero movement selects a direction and still advances by the base `0.25` per update; larger deltas add a capped boost. `scl#` holds a fractional edge position, but on a boundary crossing the code shifts once and sets the offset to exactly 0 or 1, dropping overshoot rather than traversing multiple lanes in the same update. The boost threshold and cap are the only visible filtering; there is no separate smoothing/remainder. Since displacement is applied per update, the response depends on update rate. The reference does not establish the Windows/raw-input behavior or a transferable sensitivity value. This is **SECONDARY ENGINEERING REFERENCE**, not ROM authority.

## 10. Arcade ROM and Rev. 2 binary evidence

The project listing [Tempest Rom Source code.md](../../Reference/arcade/Tempest%20Rom%20Source%20code.md) and pre-existing Rev. 2 binaries were inspected read-only.

- In the interrupt path at listing address `$D71D-$D739`, the ROM reads the spinner/cabinet byte at `$60C8`, takes the low nibble, differences it against the previous sample in `$52`, sign-extends the four-bit difference, and adds it to the signed pending delta at `$50`. The individual sample difference is therefore encoded in the spinner hardware's small signed nibble range (normally -8 through +7); it is not a PC mouse pixel count.
- `move_player` at `$9749-$97C4` consumes `$50`, clamps it to `[-31, 31]`, clears the pending delta, and subtracts it from the 8-bit rim position `$51`. Its high nibble selects one of 16 segments; its low nibble preserves 16 subpositions per segment. Closed-board byte arithmetic wraps; open-board handling clamps the position to the playable range rather than wrapping. No scale factor converts Windows mouse units to these spinner steps.
- The absolute position is integer/fixed-point, not an arbitrary fractional device count. Input samples accumulate between player-movement calls, but excess beyond the per-call `+/-31` clamp is discarded rather than carried to a later call. Exact interrupt/update frequency was not established from this trace.
- The separate `track_spinner` routine at `$ADCE-$ADE9` adds the pending delta shifted by three into `$51`, clears `$50`, and changes its returned value by at most one according to direction. Its callers include initials/test-style paths; it is not the normal `move_player` path and should not be mistaken for gameplay's full movement mapping.
- Binary byte signatures from the listing were found in the pre-existing local images: the spinner-read sequence at `136002-121.p1 + 0x71d`, the gameplay movement clamp at `136002-113.d1 + 0x75b`, and the `track_spinner` routine at `136002-116.h1 + 0x5ce` (also present in `136002-316.h1 + 0x5ce`). This is **REV-2-BINARY-CONFIRMED** for those instruction bytes; it does not prove a PC input conversion.

## 11. Recommended future option and implementation boundary

### Name and semantics

Use `--spinner-sensitivity <multiplier>`. It describes the purpose more clearly than `--spinner`, and is more specific than `--mouse-sensitivity` because it applies to horizontal mouse movement as the game's spinner control. Help text should state that ordinary mouse movement used as spinner input is included.

Define `effective live mouse delta = Raylib mouse delta X * multiplier`, ahead of the existing `0.0125f` gameplay conversion. Semantics:

- no option and explicit `1.0`: exactly current response;
- `0.5`: half the current live mouse response;
- `0.75`: three-quarters of the current live response;
- `1.25`: 25 percent more response.

Keep the initial default at `1.0`; Neal's manual calibration, not this investigation, should determine whether a later default change is desirable.

### Range and validation

Recommend `0.1` through `4.0`, inclusive. The lower bound permits substantial reduction without making the option a disable switch; the upper bound is a practical calibration ceiling consistent with other bounded multipliers, not an arcade measurement. Reject missing, malformed, zero, negative, non-finite, out-of-range, and duplicate values with a clear error. Do not clamp invalid values. Parse with `NumberStyles.Float` and `CultureInfo.InvariantCulture`, following the existing CLI pattern.

### Fractional behavior, clamp, and feedback

Keep the scaled value fractional as `float`; do not cast to an integer. The existing movement and edge-position state preserve fractions, so no additional fractional accumulator is required for basic scaling. Keep the sequence-to-result deterministic; do not add smoothing or frame-time dependence.

Do not add a per-tick clamp in this feature. A multiplier reduces all live mouse movement proportionally, but cannot mathematically guarantee that a sufficiently large input sample will not cross multiple lanes. Neal should first test whether the multiplier solves the control issue. If a clamp is later justified, specify whether it limits degrees or lane travel and preserve excess in a deterministic remainder rather than silently dropping legitimate fast movement.

If explicitly supplied, print a startup console line with the selected multiplier and its meaning relative to the existing response. No in-game HUD or settings UI is needed.

### Smallest likely code surface

1. Add a validated sensitivity parser/property to [DebugController.cs](../../src/Tempest.Game/Debug/DebugController.cs) using existing invariant-culture and duplicate-option conventions.
2. Pass it from [Program.cs](../../src/Tempest.Game/Program.cs) to the live acquisition boundary in [InputSystem.cs](../../src/Tempest.Game/Systems/InputSystem.cs), scaling only `GetMouseDelta().X` before creating `PlayerInput`.
3. Keep `TempestGame`'s direction, `SpinnerSensitivity`, keyboard/controller composition, `Player.MoveAlongTube`, and all lane topology unchanged.
4. Ensure the live `PlayerInput` captured by [PlayerInputRecorder.cs](../../src/Tempest.Game/Systems/PlayerInputRecorder.cs) is the same input passed to `RunOneStep`. Do not modify the recording schema or apply scaling in playback, headless playback, or direct simulation tests.
5. Update [README.md](../../README.md) with option semantics and examples when implementation is authorized. This investigation did not change it.

## 12. Proposed automated tests for the future feature

1. No option preserves current default behavior.
2. Explicit `1.0` equals the default.
3. `0.5` halves effective live spinner movement.
4. `0.75` scales movement correctly.
5. The supported maximum scales movement correctly.
6. Small fractional input is preserved rather than truncated.
7. Identical raw samples and multiplier produce identical results.
8. Clockwise/counter-clockwise direction is unchanged.
9. Closed Levels 1-7 retain wrapping and direction behavior.
10. Open Level 8 retains winding-aware direction and endpoint clamping.
11. Keyboard movement is unchanged.
12. Controller movement and its dead zone are unchanged.
13. Recording schema does not gain a sensitivity field.
14. Visible playback state is independent of the current live sensitivity setting.
15. Reverse reconstruction remains exact.
16. Headless playback remains unaffected and rejects an inapplicable sensitivity option clearly.
17. Sensitivity and `--speed` are independent.
18. Invariant-culture decimal values parse correctly.
19. Missing, malformed, zero, negative, non-finite, duplicate, and out-of-range values are rejected.
20. Existing input, playback, and recording regression tests remain passing.

## 13. Proposed manual calibration plan

Use the same physical spinner, without changing Windows pointer settings. Test sensitivity values in this order: `1.0`, `0.75`, `0.5`, `0.4`, `0.3`. Keep 1.0 as the reference; do not select a new default during initial testing.

At each value, evaluate:

- precise one-lane correction, slow movement, target-lane stopping, and whether a small movement still jumps multiple lanes;
- rapid emergency travel and whether reduced sensitivity becomes too slow;
- clockwise/counter-clockwise consistency;
- closed-tube traversal on Level 1 and open-endpoint control/reversal on Level 8;
- higher-level combat when practical;
- normal speed, then `--speed 0.5` and `--speed 2` with the reduced setting;
- record a short live sample at a selected multiplier, then replay it without live input and confirm the recorded path is stable. Sensitivity should not be supplied to playback; it is a live-input option.

Do not use the interactive game as an automated acceptance step. Manual gameplay validation remains Neal's responsibility after a future implementation.

## 14. Unresolved questions and evidence classification

- **EXISTING C# IMPLEMENTATION:** current delta path, scalar, tick composition, lane geometry, topology, playback, and recording behavior described above.
- **AUTOMATED-TEST EVIDENCE:** source tests verify Level 8 winding-aware direction/endpoints and geometric transition behavior; no tests were run for this read-only task.
- **PREVIOUSLY ACCEPTED PROJECT BEHAVIOR:** accepted Levels 1-8 preserve physical rotational direction, with Level 8 following an open rim and clamping rather than wrapping.
- **MANUALLY OBSERVED:** Neal reports that the physical USB spinner is too sensitive and is routed through the game's mouse path.
- **SECONDARY ENGINEERING REFERENCE:** Incitti's mouse-position recenter/boost method is frame-based and materially different.
- **ROM-LISTING-BACKED / REV-2-BINARY-CONFIRMED:** spinner nibble sampling, pending signed delta, player-position arithmetic/clamp, and separate `track_spinner` routine.
- **DESIGN RECOMMENDATION:** multiplier-based live-only adjustment, option name/range/parser, recording boundary, no clamp in this milestone, and manual calibration sequence.
- **UNKNOWN / REQUIRES MEASUREMENT:** actual USB spinner counts; whether the Windows device driver emits ordinary cursor motion or raw relative motion; whether Raylib's native disabled-cursor path bypasses pointer speed/acceleration in this exact environment; actual per-frame deltas and their dependence on Windows settings and `--speed`.

## 15. Final repository verification

The investigation made no production, test, durable-documentation, configuration, recording, screenshot, reference, or ROM changes. No tests/builds/game were run. The only task-created files are the authorized, ignored prompt and report:

- [Prompt](../Prompts/2026-11-08-spinner-sensitivity-investigation-prompt.md)
- [Report](./2026-11-08-spinner-sensitivity-investigation-report.md)

Final verification confirmed HEAD remains `d5ccbe1d3ef8af21a02bd53c8c465ed0c4908fef`, the index and tracked worktree remain clean, and the same 21 pre-existing untracked ROM files remain under `Reference/arcade/roms.rev2/`. `AI-Logs/` remains ignored; no files were staged or committed.
