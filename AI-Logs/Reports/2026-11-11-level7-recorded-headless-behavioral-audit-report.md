# Level 7 recorded headless behavioral audit

Date: 2026-11-11
Status: **PARTIAL** — bounded full playback completed; per-tick behavioral telemetry was blocked.

The complete task is saved in the paired
[prompt](../Prompts/2026-11-11-level7-recorded-headless-behavioral-audit-prompt.md).
No gameplay correction or speed change was made.

## Starting state and preservation

- Branch: `master`, tracking `origin/master`
- HEAD: `8f1c29a7d3e6ef4150c8ab64264108c3c2a9547b`
- Index: clean
- Pre-existing worktree: 18 modified tracked files and 3 untracked Level 7 test files
- The entire pre-existing candidate was preserved. No existing source, test,
  recording, or reference file was changed, staged, committed, pushed, reset,
  restored, or cleaned.
- A one-off test-only diagnostic source file was created to collect per-step
  telemetry, then removed after Windows Application Control blocked its test
  assembly. It is not present in the final worktree.

## A. Recording identity

- Path: `Temp\GameplayRecordings\lvl7-SpikerTankerMatch.json`
- Format: `tempest-gameplay-input`, version 2
- Starting scenario: `production`
- Starting displayed level: 7
- Starting lives: 10
- Random seed: 592090805
- Rim speed multiplier: 1
- Tanker-match Spiker startup flag: `true`
- Numeric Spiker speed override: absent
- Duration and exact input-frame count: 2,808, contiguous frames numbered
  0–2,807
- Recorded Super Zapper presses: 0
- Recorded restart presses: 0

The recording loaded successfully through the existing headless playback CLI.
Version-2 loading passes `Level7SpikerMatchesTankerSpeed` into
`GameplayStartupConfiguration` in
[GameplayRecordingFile.cs](../../src/Tempest.Game/Debug/GameplayRecordingFile.cs).
The CLI summary confirmed the restored level, lives, seed, and rim-speed
startup values. Its output does not include the Spiker-match flag or runtime
Spiker speed, so those were confirmed at the recording/configuration path,
not independently measured from entity telemetry.

## B. Headless playback result

**PASS — full bounded CLI playback.** The existing headless CLI completed
within the explicit 30-second wall-clock bound:

```powershell
dotnet run --no-build --project src\Tempest.Game -- --headless-playback Temp\GameplayRecordings\lvl7-SpikerTankerMatch.json --json
```

- Exit code: 0
- Completion reason: `RecordingCompleted`
- Achieved boundary: 2,808 / 2,808
- Remaining frames: 0
- Final displayed level/state: Level 8, `Approach`
- Final score: 5,290
- Final lives: 9 (one net life lost from the recorded start)
- Final player position: lane 7 + 0.2844187, depth 400
- Final Super Zapper charges: 2

The boundary-0 CLI run also passed. It reported Level 7 `Playing`, score 0,
10 lives, 2 Super Zapper charges, 4 Eggs, 16 Spikes, and no active Flippers,
Tankers, Spikers, or enemy bullets.

By the final boundary, playback had advanced from Level 7 into Level 8
`Approach`; the recording therefore did not stop early or end at a player exit
or game-over state. It ended when its input stream was exhausted during the
next-level approach. The exact Level 7 completion/transition tick is not
available from the CLI summary.

## C. Population summary

Only boundary-0 and final-boundary populations were exposed by the successful
CLI runs:

| Population | Boundary 0 | Boundary 2,808 |
|---|---:|---:|
| Tankers | 0 | 0 |
| Spikers | 0 | 0 |
| Flippers | 0 | 0 |
| Enemy projectiles | 0 | 0 |
| Eggs | 4 | 4 |
| Spikes | 16 | 15 |

These are endpoint counts, **not maxima**. The highest simultaneous Tanker,
Spiker, Flipper, projectile, and total-active-enemy populations—and their
ticks—remain **unknown** because per-tick telemetry could not be executed.

## D. Event timeline

The reliable boundary-level timeline is limited to:

- Boundary 0: startup restored at Level 7 `Playing`.
- Boundary 2,808: all recorded inputs consumed; Level 8 `Approach`, score
  5,290, 9 lives.

The final life count establishes one net life loss, but its death and
reconstruction ticks, preceding enemy state, and consequences are unavailable.
Enemy admissions/destructions, cargo releases, Spiker retreats/re-ascent, and
the Level 7 transition tick could not be reconstructed from final-state-only
output.

## E. Spiker/projectile sample and accepted manual context

No recorded-session Spiker shot could be associated with an owner and sampled
over follow-up steps; projectile-to-Spiker separation measurements and firing
cycles are therefore **not available** from this partial audit.

The supplied manual acceptance remains in force and was not re-tested or
recalibrated here:

1. Tanker speed equals Spiker speed.
2. Tanker-shot speed equals Spiker-shot speed.
3. Shots visibly travel faster than firing Tankers and Spikers.
4. Spikers continue firing during repeated ascent cycles.
5. Overall Level 7 speeds appear effectively identical to Neal's arcade
   experience.
6. No further speed changes are wanted.

No speed relationship was reopened or changed. Headless C# state alone would
not establish arcade fidelity.

## F. Tanker-surge analysis

The surge is **not determined**. Endpoint summaries show zero Tankers at both
boundaries, but do not reveal intermediate populations or admission paths.
Consequently, this audit cannot say whether the recording contains three or
more concurrent Tankers, whether a recent death/reconstruction contributed,
or how many obligations remained at such a point.

Neal's reported MAME reproduction remains relevant context. A rare Tanker
surge is not treated as a defect, and no correction is proposed.

## G. Objective anomalies

- **Confirmed:** the recording loaded; the full finite input stream completed
  without a playback exception or premature end; no restart or Super Zapper
  press was present in the recording.
- **Not established:** frame-by-frame determinism, identity conservation,
  cargo ownership/release consistency, population limits, stale entities,
  wave-completion obligations, and any surge lifecycle details.
- **Assessment:** no objective anomaly was confirmed by the evidence that ran;
  internal transition invariants remain unassessed, rather than cleared.

## H. Method assessment and validation blocker

The existing headless CLI reliably establishes recording compatibility,
startup metadata visible in its summary, requested-boundary completion,
endpoint state, and endpoint populations. Its JSON summary has no event
observer or per-step entity/ledger history, so it cannot answer the requested
maximum-population, lifecycle, surge, or relative-projectile-motion questions.

A narrowly scoped test-only diagnostic was written and compiled to use the
public playback session and inspect each bounded step. The test runner then
reported:

```text
An Application Control policy has blocked this file. (0x800711C7)
Artifact: C:\Users\nrnob\source\repos\Tempest\tests\Tempest.Game.Tests\bin\Debug\net9.0\Tempest.Game.Tests.dll
Command: dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~RecordedLevel7AuditDiagnosticTests" --logger "console;verbosity=detailed" --verbosity minimal
```

This is an infrastructure/security **BLOCKED** result, not a passing test.
The blocked test assembly was not retried, and no security control was
weakened. The temporary source harness was removed. Full per-step audit work
must stop here; no alternate harness was used to work around the block.

Validation summary:

- Recording metadata/load and boundary 0: **PASS**
- Full 2,808-frame existing headless CLI playback, 30-second bound: **PASS**
- Two-run per-boundary deterministic comparison and telemetry harness:
  **BLOCKED** by Application Control (`0x800711C7`)
- Tracked-file `git diff --check`: not applicable; no tracked files were
  changed.
- Interactive Tempest: not launched.

## Final Git status

The audit added no tracked or untracked project source/test files. The paired
prompt and this report are ignored `AI-Logs` working artifacts; they were not
staged. No commit or push occurred.

```text
## master...origin/master
 M .github/copilot-instructions.md
 M src/Tempest.Game/Debug/DebugController.cs
 M src/Tempest.Game/Debug/GameplayRecordingFile.cs
 M src/Tempest.Game/Debug/GameplayStartupConfiguration.cs
 M src/Tempest.Game/Gameplay/EnemyBullet.cs
 M src/Tempest.Game/Gameplay/Spiker.cs
 M src/Tempest.Game/Levels/Level3WaveLedger.cs
 M src/Tempest.Game/Levels/Level7WaveSelection.cs
 M src/Tempest.Game/Levels/LevelConfiguration.cs
 M src/Tempest.Game/Program.cs
 M src/Tempest.Game/TempestGame.cs
 M tests/Tempest.Game.Tests/DebugControllerTests.cs
 M tests/Tempest.Game.Tests/GameplayRecordingTests.cs
 M tests/Tempest.Game.Tests/InputDiagnosticTests.cs
 M tests/Tempest.Game.Tests/Level3WaveLedgerTests.cs
 M tests/Tempest.Game.Tests/Level7FlipperMovementTests.cs
 M tests/Tempest.Game.Tests/Level7ProductionTests.cs
 M tests/Tempest.Game.Tests/SimulationTests.cs
?? tests/Tempest.Game.Tests/Level7AdmissionPolicyTests.cs
?? tests/Tempest.Game.Tests/Level7SpikerFiringTests.cs
?? tests/Tempest.Game.Tests/Level7SpikerProjectileRelativeSpeedDiagnosticTests.cs
```

## Stop point

This is a partial audit, not a gameplay-fix or Level 7 closeout. No gameplay
correction, recording edit, speed change, commit, push, or interactive launch
was made. Further detailed analysis awaits a safe, authorized way to execute
per-step telemetry without bypassing Windows Application Control.
