# Spinner Sensitivity Implementation Report

Date: 2026-11-08

## Starting Git state and baseline

- Branch: `master`, tracking `origin/master`, with no ahead/behind divergence.
- HEAD: `d5ccbe1d3ef8af21a02bd53c8c465ed0c4908fef` (`Close out accepted production Level 8`), matching the requested accepted baseline.
- The index and tracked worktree were clean before implementation.
- The only pre-existing untracked paths were 21 ROM files under `Reference/arcade/roms.rev2/`. They remain untouched and untracked.
- No files were staged, committed, or pushed.
- Level 9 was not started.

## Investigation reused

Read and reused `2026-11-08-spinner-sensitivity-investigation-report.md`. Its established input, movement, winding, recording and playback findings were not re-investigated. Neal's physical USB spinner is routed through horizontal mouse movement; the game's boundary is `Raylib.GetMouseDelta().X`. The exact device-to-Raylib/Windows pointer processing relationship remains unknown and requires physical measurement.

## Implementation

- Added the exact option `--spinner-sensitivity <multiplier>`, with default `1.0`, inclusive range `0.1`–`4.0`, finite invariant-culture float parsing, duplicate/missing/malformed/range validation, and explicit-option tracking.
- Scaled only the live horizontal mouse delta at `InputSystem.ReadPlayer`, before constructing effective `PlayerInput`. No Y/wheel/button, keyboard, controller, firing or Zapper input is scaled. The existing `TempestGame.SpinnerSensitivity = 0.0125f`, direction mapping, movement/clamping, geometry, and simulation timing were not changed.
- Kept fractional values as floats with no clamp, accumulator, smoothing, dead zone, filtering or nonlinear response.
- Program passes the same effective `PlayerInput` value to `RunOneStep` and `PlayerInputRecorder`. Live recordings therefore store scaled per-tick input; the recording schema and metadata are unchanged.
- Visible file playback rejects this live-only option with a clear diagnostic. Headless playback and input-diagnostic mode also reject it as inapplicable. Playback input is never rescaled.
- `--speed` remains independent; combinations are accepted. Explicit sensitivity prints one concise startup line; omission prints none.
- No Windows, USB, driver, cursor, or device settings are changed.
- Updated README, development notes and behavior specification. The option is classified as a development/diagnostic/manual-calibration project feature, not ROM behavior or an arcade setting.

## Files changed

- `src/Tempest.Game/Debug/SpinnerSensitivityOptions.cs` (new)
- `src/Tempest.Game/Debug/DebugController.cs`
- `src/Tempest.Game/Debug/HeadlessGameplayPlaybackCli.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Systems/InputSystem.cs`
- `tests/Tempest.Game.Tests/SpinnerSensitivityOptionsTests.cs` (new)
- `tests/Tempest.Game.Tests/GameplayRecordingTests.cs`
- `tests/Tempest.Game.Tests/HeadlessGameplayPlaybackTests.cs`
- `README.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `TEMPEST-BEHAVIOR-SPEC.md`
- Prompt record: `AI-Logs/Prompts/2026-11-08-spinner-sensitivity-implementation-prompt.md`

## Tests and validation

- Focused parser/scaling/recording/headless selection: **44 passed, 0 failed**.
- Full solution tests (`dotnet test TempestCSharp.slnx`): **1098 passed, 0 failed, 0 skipped**.
- Full solution build (`dotnet build TempestCSharp.slnx`): **succeeded, 0 warnings, 0 errors**.
- `git diff --check`: passed; Git only reports existing line-ending conversion warnings for `README.md` and `InputSystem.cs`.
- CLI smoke checks verified exit code 2 and clear diagnostics for input-diagnostic, visible playback, headless playback and out-of-range values. These invalid-command checks returned before opening a window or loading a recording.
- Existing full-suite movement, Level 1–8 topology/direction, recording, visible/headless playback, reverse reconstruction, input, and speed regression tests all passed. New focused tests cover default/explicit 1.0, bounds and invalid values, invariant culture, fractional scaling and sub-lane accumulation, sign preservation, independent speed parsing, live-recording acceptance, playback rejection, and absence of sensitivity recording metadata.
- No physical-device or wall-clock tests were added. The interactive game was not launched.

## Protected files and final Git state

- `Reference/arcade/roms.rev2/` remains the same 21 pre-existing untracked files. No arcade/Incitti reference was changed.
- `UserData/`, screenshots, gameplay recordings, videos, and historical AI reports were not modified.
- AI log files are ignored by Git; the complete prompt was copied byte-for-byte from the supplied attachment and contains exactly one standalone completion token.
- Final branch and HEAD remain `master` at `d5ccbe1d3ef8af21a02bd53c8c465ed0c4908fef`, tracking `origin/master`.
- The implementation and tests are unstaged. No commit or push was made.

## Manual calibration handoff

Manual acceptance is **PENDING**. Do not change the default from `1.0` or claim a preferred multiplier before Neal's physical-spinner testing.

Commands:

```powershell
# Current/default response
dotnet run --project src\Tempest.Game -- --level 1 --lives 10 --spinner-sensitivity 1

# 75%, half, 40%, and 30% response
dotnet run --project src\Tempest.Game -- --level 1 --lives 10 --spinner-sensitivity 0.75
dotnet run --project src\Tempest.Game -- --level 1 --lives 10 --spinner-sensitivity 0.5
dotnet run --project src\Tempest.Game -- --level 1 --lives 10 --spinner-sensitivity 0.4
dotnet run --project src\Tempest.Game -- --level 1 --lives 10 --spinner-sensitivity 0.3

# Open Level 8 at half response
dotnet run --project src\Tempest.Game -- --level 8 --lives 10 --spinner-sensitivity 0.5

# Half response at slower and faster game speeds
dotnet run --project src\Tempest.Game -- --level 8 --lives 10 --speed 0.5 --spinner-sensitivity 0.5
dotnet run --project src\Tempest.Game -- --level 8 --lives 10 --speed 2 --spinner-sensitivity 0.5

# Record half-response live input, then replay without the sensitivity option
dotnet run --project src\Tempest.Game -- --record-gameplay Temp\GameplayRecordings\spinner-half.json --level 1 --lives 10 --spinner-sensitivity 0.5
dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\spinner-half.json
```

Calibration checklist: use the actual spinner without changing Windows settings; test small corrections, target-lane stopping, multi-lane jumps, slow travel and rapid spins; verify both rotational directions; check closed Level 1 wrapping and Level 8 endpoint clamping/reversal; compare normal, `--speed 0.5` and `--speed 2`; record and replay without the sensitivity option; verify pause, forward and reverse reconstruction; then record the preferred multiplier and rationale.

## Known limitations

The multiplier scales the delta Raylib supplies; it does not identify hardware or establish Windows raw/processed pointer behavior. It is linear and stateless, so sufficiently large samples can still cross multiple lanes. Manual calibration is required to determine whether sensitivity alone addresses Neal's control issue.
