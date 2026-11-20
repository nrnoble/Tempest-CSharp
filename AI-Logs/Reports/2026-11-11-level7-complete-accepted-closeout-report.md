# Complete accepted Level 7 closeout

Date: 2026-11-11
Status: **PASS — Level 7 closed, committed, and pushed.**

## Starting checkpoint and preservation

- Branch: `master`, tracking `origin/master`.
- Starting `HEAD`: `8f1c29a7d3e6ef4150c8ab64264108c3c2a9547b`.
- The index was clean. The worktree contained the accumulated Level 7
  implementation/tests, accepted project-instructions maintenance, and three
  untracked Level 7 test files.
- The required user task was saved verbatim as the
  [prompt record](../Prompts/2026-11-11-level7-complete-accepted-closeout-prompt.md)
  and its SHA-256 was verified against the read-only attachment.
- The complete candidate was inspected before editing. All tracked and
  untracked project changes were Level 7 implementation, Level 7 tests,
  recording/playback diagnostics, the accepted project-instructions
  maintenance, or the durable-documentation closeout. No unrelated or
  uncertain tracked changes were found.
- No local recording, `TEMP` material, video, ROM/reference asset, or build
  output was modified. AI-Logs and the real recording/build artifact paths
  were confirmed ignored.

## Final scope and files

Normal canonical Level 7 Spikers now use `Tanker.DevelopmentClimbSpeed` as
their movement authority; the numeric Level 7 speed override remains
DEVELOPMENT ONLY. The obsolete `--level7-spiker-match-tanker-speed` CLI
option now gives a clear removal message. Its optional version-2 recording
metadata remains loadable and maps to the accepted normal behavior. Recording
format versions 1 and 2 were not changed. Tanker movement, shared
`EnemyBullet` movement, firing cadence/holdoff, repeated-cycle firing,
projectile update order, retreat, Spike growth, and Levels 1-6 were not
retuned for this closeout.

The accumulated Level 7 implementation closes admission/concurrency,
reconstruction, cargo allocation, movement, repeated-cycle Spiker firing,
and recording/playback compatibility. The saturated Tanker cargo tests keep
the physical capacity and terminal skipped-child behavior covered. The
existing deterministic calibration tests remain, while the obsolete assertion
that normal Level 7 Spiker speed is randomized was replaced with the accepted
Tanker-rate assertion.

Files in the commit:

- `.github/copilot-instructions.md` — accepted local-material instruction
  maintenance.
- `TEMPEST-BEHAVIOR-SPEC.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `src/Tempest.Game/Debug/DebugController.cs`
- `src/Tempest.Game/Debug/GameplayRecordingFile.cs`
- `src/Tempest.Game/Debug/GameplayStartupConfiguration.cs`
- `src/Tempest.Game/Gameplay/EnemyBullet.cs`
- `src/Tempest.Game/Gameplay/Spiker.cs`
- `src/Tempest.Game/Levels/Level3WaveLedger.cs`
- `src/Tempest.Game/Levels/Level7WaveSelection.cs`
- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/TempestGame.cs`
- `tests/Tempest.Game.Tests/DebugControllerTests.cs`
- `tests/Tempest.Game.Tests/GameplayRecordingTests.cs`
- `tests/Tempest.Game.Tests/InputDiagnosticTests.cs`
- `tests/Tempest.Game.Tests/Level3WaveLedgerTests.cs`
- `tests/Tempest.Game.Tests/Level7AdmissionPolicyTests.cs`
- `tests/Tempest.Game.Tests/Level7FlipperMovementTests.cs`
- `tests/Tempest.Game.Tests/Level7ProductionTests.cs`
- `tests/Tempest.Game.Tests/Level7SpikerFiringTests.cs`
- `tests/Tempest.Game.Tests/Level7SpikerProjectileRelativeSpeedDiagnosticTests.cs`
- `tests/Tempest.Game.Tests/SimulationTests.cs`

The staged inventory contained exactly these 23 project files. It contained
no AI-Logs, `TEMP`, recording, video, ROM/reference-user asset, `bin`, or
`obj` path. `git diff --check` and `git diff --cached --check` both passed.

## Accepted behavior and evidence

- **ARCADE OBSERVED / MANUALLY ACCEPTED:** Neal compared Level 7 against the
  original arcade game and MAME. Tanker and Spiker movement is accepted at
  the same visible rate. Tanker and Spiker shots share the same visible rate
  and travel visibly faster than their firing enemies. Spikers continue
  firing during later ascent cycles. Neal judges overall Level 7 speeds
  extremely close to arcade play, appearing effectively identical. This is
  perceptual acceptance, not a numeric arcade measurement; no further Level 7
  speed tuning is wanted.
- **PROJECT DECISION:** The production Spiker/Tanker movement relationship
  uses the existing `Tanker.DevelopmentClimbSpeed` authority. Its current C#
  value is 1.75 depth units per simulation step and is Incitti-derived, not
  ROM-measured. Tanker and Spiker shots continue to use the same
  `EnemyBullet` implementation; no ROM-derived numeric projectile claim is
  made.
- **MAME OBSERVED:** Neal reproduced the rare end-of-wave Tanker surge in
  MAME after the Blaster died with approximately seven or fewer enemies
  remaining at a narrow point in their lifecycle. He no longer considers it
  a C# discrepancy or gameplay issue; it is rare in ordinary play. No Tanker
  cap or change to admission, reconstruction, concurrency, enemy supply, or
  Tanker creation was made. The earlier Spiker-to-Tanker explanation remains
  a discarded, unproven hypothesis.
- **C# MEASURED:** The real protected recording
  `Temp\GameplayRecordings\lvl7-SpikerTankerMatch.json` completed all 2,808
  frames through headless playback. It advanced from Level 7 Playing into
  Level 8 Approach with completion reason `RecordingCompleted`, 2,808/2,808
  frames consumed, and no remaining frames.
- **PARTIAL / DEVELOPMENT-ONLY diagnostic direction:** Per-tick telemetry
  was not completed because Windows Application Control blocked
  `tests\Tempest.Game.Tests\bin\Debug\net9.0\Tempest.Game.Tests.dll` with
  `0x800711C7`. The blocked diagnostic was not retried, security was not
  weakened, its temporary source was removed, and no telemetry success was
  claimed. The successful headless run establishes bounded playback and
  startup restoration, not per-tick population/lifecycle maxima. Telemetry
  remains optional future diagnostic work and is not a Level 7 blocker.

The durable behavior and development documents record these classifications,
the closed Level 7 candidate, the corrected seven-physical-place ROM
description, the surge conclusion, and the headless audit limits.

## Validation

All commands ran on .NET SDK 9 with the test assembly unblocked.

1. Focused Level 7, admission/reconstruction, Tanker/Flipper/projectile,
   command-line, recording/playback, and simulation coverage:

   ```powershell
   dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~Level7|FullyQualifiedName~Level3WaveLedgerTests|FullyQualifiedName~Tanker|FullyQualifiedName~EnemyBullet|FullyQualifiedName~Flipper|FullyQualifiedName~DebugControllerTests|FullyQualifiedName~InputDiagnosticTests|FullyQualifiedName~GameplayRecording|FullyQualifiedName~GameplayPlayback|FullyQualifiedName~HeadlessGameplayPlayback|FullyQualifiedName~SimulationTests|FullyQualifiedName~InteractiveSpeedOptionsTests" --verbosity minimal
   ```

   **PASS** — 688 passed, 0 failed, 0 skipped.

2. Full Level 7 regression subset:

   ```powershell
   dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-build --no-restore --filter "FullyQualifiedName~Level7" --verbosity minimal
   ```

   **PASS** — 94 passed, 0 failed, 0 skipped.

3. Recording/playback regression subset:

   ```powershell
   dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-build --no-restore --filter "FullyQualifiedName~GameplayRecording|FullyQualifiedName~GameplayPlayback|FullyQualifiedName~HeadlessGameplayPlayback" --verbosity minimal
   ```

   **PASS** — 96 passed, 0 failed, 0 skipped.

4. Full solution tests:

   ```powershell
   dotnet test TempestCSharp.slnx --no-restore --blame-hang-timeout 5min --verbosity minimal
   ```

   **PASS** — 1,264 passed, 0 failed, 0 skipped. The Blame collector reported
   that all tests finished and generated no sequence file.

5. Solution build:

   ```powershell
   dotnet build TempestCSharp.slnx --no-restore --verbosity minimal
   ```

   **PASS** — 0 warnings, 0 errors.

6. `git diff --check` and `git diff --cached --check`: **PASS**.

7. Protected-recording headless playback:

   ```powershell
   dotnet run --no-build --project src\Tempest.Game -- --headless-playback Temp\GameplayRecordings\lvl7-SpikerTankerMatch.json --json
   ```

   **PASS** — `RecordingCompleted`, 2,808/2,808 frames, Level 8 `Approach`.
   The recording was read but not modified. Interactive Tempest was not
   launched.

The first focused run exposed one ambiguous test constructor call after
removing the obsolete runtime option; that test call was made explicit. The
next focused run caught an assertion still expecting the old parser error
text; it was updated to the new removal message. The final focused and all
subsequent validation runs passed.

## Staging, commit, push, and final state

- Staged exactly the 23 files listed above. Confirmed no ignored AI-Logs,
  `TEMP`, recording, video, ROM/reference-user asset, or build output was
  staged.
- Commit: `037b3f9877f38d0182a3eba4fb8a8f0279e4f1a9`
- Commit subject: `Complete accepted Level 7 behavior`
- Commit summary: 23 files changed, 2,401 insertions, 195 deletions.
- Push: `git push origin master` **PASS**, fast-forwarded `master` from
  `8f1c29a` to `037b3f9`; no force-push.
- Post-push fetch verified local `HEAD` and `origin/master` both equal
  `037b3f9877f38d0182a3eba4fb8a8f0279e4f1a9`.
- Final Git status: `## master...origin/master`; index and tracked worktree
  are clean.
- Unresolved required issues: none. Fine-grained playback telemetry remains
  optional and incomplete as described above.

**LEVEL 7 IS CLOSED. No Level 8 work began in this closeout.**
