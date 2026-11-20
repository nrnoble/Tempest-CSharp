# Interactive speed multiplier implementation report

Date: 2026-11-08

## Starting state and investigation reused

The repository started at
`e3a3d4cf31449bc710c43d9d912ca5954605ade8` on `master`, tracking
`origin/master`. The only initial status entries were the pre-existing
untracked `screenshot000.png` through `screenshot013.png`; they were left
untouched.

Reused
`AI-Logs/Reports/2026-11-08-simulation-timing-speed-rom-investigation-report.md`.
Its recommendation was implemented: adjust only the interactive Raylib
host-loop target rate, retaining one simulation step and one render per
ordinary loop pass. No timing/ROM investigation was repeated.

## Implementation

- Added immutable `InteractiveSpeedOptions` to hold the validated multiplier,
  supported 0.25–4.0 inclusive range, normal 60-FPS constant and derived
  target.
- Added invariant-culture `--speed` parsing. Missing, malformed, non-finite,
  out-of-range and duplicate options fail with explicit diagnostics.
- Converted `60 * multiplier` to integer FPS using
  `Math.Round(..., MidpointRounding.AwayFromZero)`, i.e. nearest integer with
  exact halves away from zero. Omission constructs the normal option with the
  literal 60-FPS constant; explicit 1 yields an equivalent options value.
- `Program` applies only `TargetFramesPerSecond` through
  `Raylib.SetTargetFPS`. No speed value is passed into simulation state,
  gameplay constants, input processing, audio or recording identity.
- Recording and visible playback accept the runtime option. It is not
  serialized. Headless playback rejects it as an unsupported/incompatible
  argument and remains unthrottled. The unrelated input diagnostic explicitly
  rejects `--speed`.
- Updated usage and durable development/behavior documentation. No
  interactive game was launched.

## Files changed

- `src/Tempest.Game/Debug/InteractiveSpeedOptions.cs` (new)
- `src/Tempest.Game/Debug/DebugController.cs`
- `src/Tempest.Game/Program.cs`
- `tests/Tempest.Game.Tests/InteractiveSpeedOptionsTests.cs` (new)
- `tests/Tempest.Game.Tests/GameplayPlaybackSessionTests.cs`
- `tests/Tempest.Game.Tests/GameplayRecordingTests.cs`
- `tests/Tempest.Game.Tests/HeadlessGameplayPlaybackTests.cs`
- `README.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `TEMPEST-BEHAVIOR-SPEC.md`
- `AI-Logs/Prompts/2026-11-08-speed-multiplier-diagnostic-implementation-prompt.md`
- `AI-Logs/Reports/2026-11-08-speed-multiplier-diagnostic-implementation-report.md`

## Tests and validation

New tests cover omitted/explicit normal state, expected fractional and faster
targets, exact midpoint rounding, inclusive bounds, invalid and duplicate
values, invariant-culture parsing, recording/playback option acceptance,
recording-schema exclusion, equal visible-playback boundary state and the
headless rejection policy.

- Focused parser/recording/playback/headless tests: **101 passed, 0 failed**.
- Full solution tests: **1,022 passed, 0 failed**.
- Full solution build: **succeeded, 0 warnings, 0 errors**.
- `git diff --check`: **passed**.

Existing pause/step/reverse, audio, simulation, gameplay and campaign
regressions were exercised by the full suite. No wall-clock sleep-based tests
were added.

## Git state and manual acceptance

No files were staged, committed or pushed. The original screenshots remain
unmodified and untracked. `HEAD` remains
`e3a3d4cf31449bc710c43d9d912ca5954605ade8`. The final worktree contains the
tracked documentation, source and test edits listed above, the two new
untracked C# source/test files, and the same pre-existing untracked
screenshots. The prompt and report exist under `AI-Logs/`, which is ignored by
`.gitignore`. The implementation is left unstaged for Neal's manual
acceptance.

Run an interactive sample with:

```powershell
dotnet run --project src\Tempest.Game -- --speed 1 --lives 10
dotnet run --project src\Tempest.Game -- --speed 0.95 --lives 10
dotnet run --project src\Tempest.Game -- --speed 0.9 --lives 10
dotnet run --project src\Tempest.Game -- --speed 0.5 --lives 10
dotnet run --project src\Tempest.Game -- --speed 2 --lives 10
dotnet run --project src\Tempest.Game -- --level 7 --speed 0.95 --lives 10
dotnet run --project src\Tempest.Game -- --level 7 --speed 2 --lives 10
dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\level5.json --speed 0.5
```

Manual checklist:

1. Confirm `--speed 1` is equivalent to accepted baseline.
2. Compare global slowdown at 0.5 and speedup at 2 for player, enemies,
   shots, transitions and effects; also compare subtle 0.9 and 0.95 values.
3. Check live spinner/mouse control, Fire and Super Zapper behavior.
4. Confirm sound pitch/rate remains normal while event spacing changes.
5. Replay a fresh visible recording at multiple speeds and compare equal tick
   boundaries.
6. Verify pause, forward/grouped step, reverse scrubbing and screenshots.
7. Confirm headless playback is still fast and unthrottled.
8. Check for normal-speed gameplay regressions.

## Limitations and unresolved acceptance

`--speed` is a requested host-loop rate, not a real-time guarantee. Host,
render, display or driver limits may reduce actual pacing; live input is
sampled at the selected loop cadence. There is intentionally no accumulator,
catch-up, interpolation, separate input thread, independent renderer or audio
time stretching. Headless playback rejects the option. Neal's interactive
manual acceptance remains pending; the feature does not imply the accepted
normal 1.0 rate should be changed.
