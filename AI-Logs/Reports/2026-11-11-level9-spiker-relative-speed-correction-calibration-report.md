# Level 9 Spiker/projectile relative-speed correction and calibration report

## Status

**Candidate implementation complete. Automated validation PASS. Manual calibration and acceptance PENDING NEAL.**

The existing Level 9 candidate was preserved. No interactive game was launched. No files were staged, committed, or pushed. Levels 4, 5, 6, and 8 were not retuned; Level 10 was not started.

## Starting candidate and Git state

- Started on `master`, tracking `origin/master`.
- `HEAD` and `origin/master` both matched `037b3f9877f38d0182a3eba4fb8a8f0279e4f1a9`.
- The index was empty. Before this correction, the candidate had 17 modified tracked files and two untracked files; its tracked diff was 512 insertions and 51 deletions.
- The candidate inventory matched the bounded Level 9 implementation and read-only relative-speed reports. No candidate file was reset, restored, discarded, or staged.
- The complete substantive prompt is retained in `AI-Logs/Prompts/2026-11-11-level9-spiker-relative-speed-correction-calibration-prompt.md`. AI-Logs are ignored by Git.

## Defect and evidence boundary

- **ARCADE/MAME MANUAL OBSERVATION (Neal, supplied in the task):** while both a firing Spiker and its projectile travel toward the rim, the projectile must pull visibly ahead. The Spiker must not catch, overtake, or outpace its shot. This applies across observed levels; absolute Spiker speed may vary.
- The old C# Level 9 path fell through to `ProgressionIndex / 48f + 2.5f + (float)(random.NextDouble() * 2)`. With Level 9 progression index 8, this selected approximately 2.6667–4.6667 per Spiker while projectiles move at 3.0 per active simulation step. Equal or faster Spiker outcomes violated the observed relative-speed requirement.
- **ROM CONTRADICTION: UNRESOLVED.** The bounded static trace found Level 9 Spiker and enemy projectile raw inputs of `$C0`; both `crack_speed` paths produce `$FE00`, with equal effective raw displacement in the normal active path. That does not reconcile the repeated arcade/MAME observation. This correction is not a literal ROM speed translation and does not claim either source is wrong.

## Implementation and classification

- Added an explicit Level 9 production Spiker-speed policy of `2.0`. It is classified in code as **PROJECT CALIBRATION CANDIDATE — MANUAL ACCEPTANCE PENDING NEAL**, not ROM-backed or accepted arcade speed.
- Level 9 no longer samples the old randomized speed formula. The maximum supported diagnostic speed is 2.75, strictly below the unchanged 3.0 projectile rate.
- Added `--level9-spiker-speed <value>`, accepting finite invariant-culture values from 1.0 through 2.75 inclusive. Missing, malformed, non-finite, repeated, conflicting, out-of-range, and non-Level-9 use is rejected. Level 7 and Level 9 override options cannot be combined.
- The existing Level 7 Tanker-derived speed policy remains unchanged. Other levels retain the old formula and its existing `NextDouble()` consumption.
- Recording format remains version 2. Optional `Level9SpikerSpeedOverride` startup metadata is emitted only for diagnostic recordings, validated on load, and restored by playback. Old version-2 files without the field still load; ordinary Level 9 recordings omit it and use 2.0. `--playback-file` rejects a direct Level 9 speed override. `--speed` remains presentation-only.
- The shared `EnemyBullet` movement remains exactly 3.0 per active step. Tanker, Flipper, firing cadence, repeated-cycle firing, reversal/retreat, geometry, cargo, wave selection, progression, and other candidate behavior were not retuned.
- **FOLLOW-UP FINDING, unchanged:** the inherited randomized speed path also applies to the Level 4 candidate and production Levels 5, 6, and 8, where Spikers may exceed the shared projectile rate. Do not fold these into this milestone; handle them as a separately authorized cross-level remediation. Level 7 remains frozen.

## Automated measurements and coverage

- The normal Level 9 production speed is selected as exactly 2.0 without consuming `Random.NextDouble()` for speed selection.
- Using the actual Spiker update and projectile advance methods for 10 matched active ascent steps: Spiker travel is 20, projectile travel is 30, separation grows by 10, and projectile/Spiker travel is 1.5.
- Diagnostic movement tests cover 1.0, 1.75, 2.0, 2.25, 2.5, and 2.75. Parsing/startup tests reject 3.0, values above 2.75, values below 1.0, zero, negative, NaN, infinities, malformed/invariant-culture-invalid input, and repeated or conflicting options.
- Firing tests cover shot-origin/update ordering (a new shot first advances on the following active step) and repeated-cycle Spiker firing at both normal 2.0 and diagnostic 2.5.
- Recording/playback tests cover the normal default, diagnostic round-trip, old version-2 metadata omission, visible startup restoration, headless startup determinism, and presentation-speed independence.
- Level 7 and existing non-Level-7 mixed-level selection tests verify the prior speed formula and random-selection call remain unchanged where applicable (Level 3, the Level 4 candidate, and Levels 5, 6, and 8). Level 1 and 2 have no normal Spiker production.
- Level 9 Tanker, Flipper, and projectile movement authorities remain 1.75, 2.0, and 3.0 respectively.

## Validation

The repository `bin` output was locked by an already-running `Tempest.Game` process (PID 60424). The default focused test command failed while copying `Tempest.Game.exe`; retrying with `UseAppHost=false` still encountered the same lock on `Tempest.Game.dll`. The process was not terminated. Validation then succeeded using an isolated session-workspace `OutDir` with `UseAppHost=false`; no security controls were bypassed.

| Validation | Result |
|---|---:|
| Focused speed, CLI, recording, and Level 7 regression tests | **PASS** — 132 passed |
| Complete Level 9 test subset | **PASS** — 58 passed |
| CLI, recording, playback, startup, and input-diagnostic tests | **PASS** — 158 passed |
| Levels 1–8 targeted regression tests | **PASS** — 191 passed |
| Non-Level-7 speed-selection regression, including Level 3 and Level 8 | **PASS** — 28 passed |
| Full solution tests, required blame timeout retained and outputs isolated | **PASS** — 1,336 passed, 0 failed |
| Full solution build | **PASS** — 0 warnings, 0 errors |
| `git diff --check` | **PASS** |

No interactive game was launched. The specified default-output test command could not complete because of the existing process lock; the same full suite completed successfully through the isolated-output route above.

## Manual calibration handoff — Neal

Run each setting and compare with arcade/MAME. These are calibration candidates, not an acceptance claim.

```powershell
# Normal Level 9 production candidate — Spiker 2.0
dotnet run --project src\Tempest.Game -- --level 9 --lives 10 --spinner-sensitivity 1

# Spiker 1.75
dotnet run --project src\Tempest.Game -- --level 9 --lives 10 --spinner-sensitivity 1 --level9-spiker-speed 1.75

# Spiker 2.25
dotnet run --project src\Tempest.Game -- --level 9 --lives 10 --spinner-sensitivity 1 --level9-spiker-speed 2.25

# Spiker 2.5
dotnet run --project src\Tempest.Game -- --level 9 --lives 10 --spinner-sensitivity 1 --level9-spiker-speed 2.5

# Spiker 2.75
dotnet run --project src\Tempest.Game -- --level 9 --lives 10 --spinner-sensitivity 1 --level9-spiker-speed 2.75

# Half-speed recording of normal 2.0
dotnet run --project src\Tempest.Game -- --level 9 --lives 10 --spinner-sensitivity 1 --speed 0.5 --record-gameplay Temp\GameplayRecordings\level9-spiker-speed-2.0.json

# Half-speed recording with the 2.5 override
dotnet run --project src\Tempest.Game -- --level 9 --lives 10 --spinner-sensitivity 1 --speed 0.5 --record-gameplay Temp\GameplayRecordings\level9-spiker-speed-2.5.json --level9-spiker-speed 2.5

# Playback; the selected startup speed is restored from each recording
dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\level9-spiker-speed-2.0.json --speed 0.5
dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\level9-spiker-speed-2.5.json --speed 0.5
```

For each setting, please judge:

1. **Absolute Spiker speed:** clearly too slow, somewhat too slow, approximately correct, somewhat too fast, or clearly too fast.
2. **Projectile relationship:** pulls away strongly/moderately/slightly, appears nearly equal, or lags. Any lag is unacceptable.
3. **Reversal:** whether the projectile has a natural visible lead before the Spiker reverses; the prior lagging-shot symptom must be gone.
4. **Level 9 pressure:** whether ordinary gameplay remains close to arcade/MAME.
5. **Regression:** `$53`, repeated firing, Tankers/cargo, geometry, starting Spikes, and other Level 9 behavior remain correct.

Manual arcade/MAME comparison and final speed acceptance remain **PENDING NEAL**. Do not promote 2.0 or any override to the accepted production value without Neal's explicit authorization.

## Files changed specifically by this correction

The following paths received correction-specific edits:

- `src/Tempest.Game/Debug/DebugController.cs`
- `src/Tempest.Game/Debug/GameplayRecordingFile.cs`
- `src/Tempest.Game/Debug/GameplayStartupConfiguration.cs`
- `src/Tempest.Game/Gameplay/EnemyBullet.cs` (comment only; shared movement unchanged)
- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/TempestGame.cs`
- `tests/Tempest.Game.Tests/DebugControllerTests.cs`
- `tests/Tempest.Game.Tests/GameplayRecordingTests.cs`
- `tests/Tempest.Game.Tests/Level7SpikerProjectileRelativeSpeedDiagnosticTests.cs`
- `tests/Tempest.Game.Tests/Level9SpikerProjectileRelativeSpeedTests.cs` (new)

Five of these files—`LevelConfiguration.cs`, `Program.cs`, `TempestGame.cs`, `DebugControllerTests.cs`, and `GameplayRecordingTests.cs`—already contained uncommitted Level 9 candidate changes at task start. The correction was integrated without replacing that work. The original candidate's other files remain present; no Levels 1–8 production behavior was intentionally changed.

## Final Git state and confirmations

- Branch remains `master...origin/master`; `HEAD` and `origin/master` remain `037b3f9877f38d0182a3eba4fb8a8f0279e4f1a9`.
- Index remains empty. Current status is 22 modified tracked files and three untracked files; this includes the pre-existing Level 9 candidate and the new speed test. `git diff --stat` reports 22 tracked files changed, 863 insertions, and 73 deletions; untracked files are not included in that stat.
- `git diff --check` passed. Git emitted the existing LF-to-CRLF working-copy warning for `src/Tempest.Game/Levels/LevelLoader.cs`.
- **Shared projectile speed unchanged:** 3.0 per active step.
- **Levels 1–8 not intentionally changed:** Level 7 policy remains accepted and unchanged; other prior speed-selection behavior remains on the existing formula.
- **No stage, commit, or push occurred.** No interactive Tempest launch occurred. Level 10 was not started.
- **Manual acceptance: PENDING NEAL.**
