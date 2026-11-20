# Self-describing gameplay recording implementation report

Date: 2026-11-06

## 1. Starting Git state

- Repository: `C:\Users\nrnob\source\repos\Tempest`.
- Branch: `master`, tracking `origin/master`.
- Starting `HEAD` and `origin/master`:
  `8222acee9baf6e893499a7d6ddb859bd1b7e0903`
  (`Add direct level startup and controller quit`).
- Initial worktree and index were clean; there were no unexplained
  nonignored untracked files.
- The expected baseline matched exactly. No reset, cleanup, restore, or other
  baseline-recovery action was needed.

## 2. Accepted baseline protected

The manually accepted Level 1 recording round trip remains intact. Recording
still captures the effective `PlayerInput` and separate restart bit once per
`RunOneStep`, including neutral frames. Playback still uses the normal
simulation and seed, consumes the same tick-indexed frames, and hands control
back to live input at the original duration. The 60 Hz target, one-update-per-
render-loop behavior, RNG implementation, frame numbering, save-on-normal-
close point, temporary sibling file, and non-overwriting move were preserved.
No interactive game was launched.

The Reports directory had no separate records for the original deterministic
playback/profile and recorded-input foundation checkpoints; the accepted
history and manual round-trip decision were reviewed in
`TEMPEST-DEVELOPMENT-NOTES.md`, the existing playback/recording tests, and the
requested startup-options analysis report.

## 3. Canonical startup configuration

Added immutable `GameplayStartupConfiguration` with only:

- `StartingDisplayedLevel`
- `StartingLives`
- `RimSpeedMultiplier`
- `RandomSeed`

Its constructor validates the level through `LevelProgression`, positive lives,
positive finite rim speed, and nonnegative seed. It resolves the selected
production configuration through the existing catalog; it does not serialize
or duplicate `LevelConfiguration`.

## 4. Exact version-2 schema

Identifier remains `tempest-gameplay-input`. Version 2 serializes these root
members:

```text
Format
Version
RandomSeed
StartingScenario = "production"
StartingDisplayedLevel
StartingLives
RimSpeedMultiplier
DurationTicks
Frames[]
```

Each frame retains the exact fields:

```text
SimulationTick
RestartPressed
Input:
  Fire
  MouseDeltaX
  Left
  Right
  ZapPressed
  ControllerAxis
```

Required members and unknown-member rejection remain strict. `StartingScenario`
is still present; version 1 keeps its original `"level1"` value.

## 5. Version-1 compatibility

Version 1 loads with its original strict schema and semantics: production Level
1, 3 lives, rim speed 1.0, recorded seed, duration, and original frames.
Version-1 files are not rewritten. Version 2 is not promised to be readable by
older builds. Unsupported future versions are rejected.

## 6. CLI grammar and defaults

The transport flag and filename come first. Compatible recording options may
follow in any order:

```text
--record-gameplay <file> [--level N] [--lives N] [--rim-speed N] [--game-over-density N]
--playback-file <file> [--game-over-density N]
```

Recording without startup options resolves to Level 1, 3 lives, rim speed 1.0,
and a newly generated nonnegative seed. Duplicate, missing, malformed,
unsupported-level, unknown, and extra arguments fail with command-line errors.
Special development scenarios, including both built-in playback scenarios,
remain incompatible with file recording/playback.

## 7. Recording option composition

Recording accepts production `--level N`, positive `--lives N`, and positive
finite `--rim-speed N`; duplicate simulation options are rejected. The
recording command stores effective values, not option-presence flags.
`--game-over-density N` is accepted as an observation-only renderer option and
is not included in the startup identity.

## 8. Playback metadata restoration

Playback loads the file's complete startup configuration and builds the game
through the same `TempestGame` production initialization path as direct
production startup. Levels 1–7 resolve through `LevelProgression`; Level 5
therefore receives real production geometry, starting Spikes, `$53`
Flipper/Spike policy, enemy eligibility, and onward Level 5-to-6 progression.

## 9. Playback override rejection

Playback rejects `--level`, `--lives`, and `--rim-speed` regardless of whether
the supplied value matches the file. Special scenarios are also rejected
before Raylib window startup. Only compatible `--game-over-density` may be
combined with playback.

## 10. Density policy

Density remains renderer-only and is not serialized. Parser tests permit it
with recording and playback; a deterministic state test compares games under
two parsed density values and confirms that startup identity and simulation
state are equal.

## 11. Production initialization path

File recording and playback use the existing fresh production wave initializer
and configuration catalog. Direct Level 5 is not a special terminal scenario.
The same applies to each supported starting Level 1–7.

## 12. RNG ordering

The loaded/generated seed is put into `GameplayStartupConfiguration` and
converted to the seeded `Random` before `TempestGame` constructs its
randomized Game Over sequence or initializes the selected production wave.
Metadata parsing does not consume gameplay random values, and no internal
mutable RNG state was added to the file.

## 13. Restart semantics

The existing production campaign policy is unchanged: a full-game restart
starts a fresh Level 1 campaign, restores the effective configured lives, and
retains the rim-speed multiplier. Recording and playback use the same policy.
The new restart test pins this behavior for a recording begun directly at
Level 5. A seeded production Level 1 recording also runs through an actual
life loss and re-entry and compares the resulting simulation state with its
playback.

## 14. Input/frame semantics

No changes were made to capture count, neutral frames, separate restart input,
tick numbering, field meaning, replay cursor advancement, live-input handoff,
fixed simulation step, or RNG implementation.

## 15–16. Production and test files changed

**Production**

- `src/Tempest.Game/Debug/GameplayStartupConfiguration.cs` — new immutable
  startup value.
- `src/Tempest.Game/Debug/GameplayRecordingFile.cs` — v2 schema, strict v1/v2
  loading, metadata restoration, and retained safe write behavior.
- `src/Tempest.Game/Debug/DebugController.cs` — transport-first grammar,
  strict recording options, playback override policy, and special-mode
  rejection.
- `src/Tempest.Game/Systems/PlayerInputRecorder.cs` — records canonical
  startup identity.
- `src/Tempest.Game/Program.cs` — resolves record/playback startup before
  normal production game construction.
- `src/Tempest.Game/TempestGame.cs` — canonical startup overload seeds before
  startup random consumption.

**Tests**

- `tests/Tempest.Game.Tests/GameplayRecordingTests.cs`
- `tests/Tempest.Game.Tests/InputDiagnosticTests.cs`

## 17–21. Validation results

- Format and CLI parser tests (`GameplayRecordingTests` and
  `InputDiagnosticTests`): **41 passed, 0 failed, 0 skipped**.
- Startup, playback-profile, direct-level, and rim-speed tests:
  **37 passed, 0 failed, 0 skipped**.
- Selected deterministic replay/startup equivalence cases:
  **8 passed, 0 failed, 0 skipped**, including a recorded life loss/re-entry,
  direct Level 5, Level 7 startup, restart, and density independence.
- Neighboring gameplay/replay regressions (recording/playback, Levels 5 and 7,
  campaign soak/progression, death/re-entry, restart, Super Zapper, rim pursuit,
  input, and exit): **629 passed, 0 failed, 0 skipped**.
- Full `dotnet test TempestCSharp.slnx`: **868 passed, 0 failed, 0 skipped**.
- Final `dotnet build TempestCSharp.slnx`: **0 warnings, 0 errors**.
- `git diff --check`: passed. Git printed only a line-ending advisory for
  `README.md`; no whitespace error was reported.

The focused groups overlap; their totals are not additive. An earlier
non-finite-input test assertion expected only `JsonException`, while the
loader correctly rejected the value as `InvalidDataException`. The assertion
was broadened to accept either validation exception, then the focused tests,
neighboring regressions, and full suite passed.

## 22. Documentation changed

- `README.md` — current record/playback commands, grammar, compatibility,
  override rules, disposable file policy, and pending manual acceptance.
- `TEMPEST-DEVELOPMENT-NOTES.md` — historical version-1 baseline distinguished
  from the version-2 implementation candidate and its deferred controls.
- `TEMPEST-BEHAVIOR-SPEC.md` — development-option entries, current schema and
  evidence classification, with manual acceptance explicitly pending.

No arcade-behavior claim was changed.

## 23–24. Manual commands and acceptance checklist

Run from the repository root in PowerShell; recordings are disposable under
ignored `Temp/` and must not be committed:

```powershell
# A. Record ordinary Level 1 with effective defaults
dotnet run --project src\Tempest.Game -- --record-gameplay Temp\GameplayRecordings\level1-defaults.json

# B. Play it back with no simulation overrides
dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\level1-defaults.json

# C. Record production Level 5 with 10 lives
dotnet run --project src\Tempest.Game -- --record-gameplay Temp\GameplayRecordings\level5-10-lives.json --level 5 --lives 10

# D. Playback restores Level 5 and lives from the file
dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\level5-10-lives.json

# E. Record Level 5 with an explicit rim-speed multiplier
dotnet run --project src\Tempest.Game -- --record-gameplay Temp\GameplayRecordings\level5-rim-speed-2.5.json --level 5 --lives 10 --rim-speed 2.5

# F. This must be rejected before the game window opens
dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\level5-10-lives.json --level 5
```

Neal's manual checklist:

1. Confirm Level 1 separate playback remains visually as previously accepted.
2. Confirm Level 5 starts with production geometry/behavior and 10 lives.
3. Confirm playback restores Level 5 and lives without CLI overrides.
4. Compare movement, Fire and Super Zapper timing.
5. Compare deaths and restart if exercised.
6. Compare Level 5-to-6 progression if reached.
7. Confirm simulation overrides are rejected before gameplay opens.
8. Confirm live controls resume after recorded input ends if that boundary is
   reached.

No manual acceptance is claimed for the version-2 extension.

## 25. Deferred features

Playback speed, pause, single-step, loop, start/end frame, seeking, snapshots,
overlays, deterministic signatures, a headless replay runner, attract mode,
playlist/browser UI, and other advanced playback controls remain deferred.

## 26. Final Git state

- `HEAD` remains `8222acee9baf6e893499a7d6ddb859bd1b7e0903`.
- Branch remains `master`, tracking `origin/master`.
- The index is clean. The intended source, test, and documentation changes are
  unstaged; the new startup-configuration source file is the only
  nonignored untracked project file.
- The prompt and this report are saved under ignored `AI-Logs/`.
- The prompt was copied from the supplied attachment and SHA-256 verified:
  `03638B30CE14149D63AF0A6748B6D6D672E188E3F77A0BD9154C7B079E98F82B`.
- No recording, reference, `Videos/`, `TEMP/`, tracked `bin/obj`, or Git
  configuration changes were made.
- No Level 8/9 work, gameplay retuning, advanced playback controls,
  interactive game launch, staging, commit, or push occurred.
