# Level 7 saturated Tanker cargo allocation correction

Date: 2026-11-10

Disposition: **AUTOMATED VALIDATION PASSED. CANDIDATE REMAINS UNCOMMITTED.
MANUAL LEVEL 7 ACCEPTANCE IS PENDING.**

## Starting state and confirmed difference

- Accepted baseline and unchanged `HEAD`:
  `8f1c29a7d3e6ef4150c8ab64264108c3c2a9547b`.
- The existing Level 7 concurrency/movement candidate was already uncommitted.
  Its production changes remain intact except for this authorized cargo
  capacity correction. No commit or push was made.
- The ROM has seven physical enemy places. Removing a Tanker from a full
  seven-enemy population frees one place; the ROM attempts its two cargo
  children in order, so only the first fits. The former C# complete-pair
  release could instead produce eight live enemies and preserve a phantom
  extra obligation after death.
- Neal's manual comparison identifies the current candidate as materially
  harder than arcade/MAME Level 7. Automated validation does not constitute
  manual acceptance.

## Implementation

- Reused the canonical Level 7 admission snapshot and
  `Level7WaveSelection.GetOpenPhysicalSlots` as the physical occupancy source.
  The snapshot counts active ordinary/cargo Flippers, shot-release cargo
  awaiting activation, ordinary/converted Tankers, Spikers, and other live
  enemies. Eggs and latent cargo do not occupy places. Existing enemy
  collections retain rim, transition, and pending-zap enemies until removal.
- `ReleaseTankerCargo` is called after its carrier is removed. Canonical
  Level 7 now derives a child limit from that post-removal snapshot and
  processes the ordered cargo identities independently. Other configurations
  keep their existing complete-pair path.
- Added the terminal cargo disposition
  `NotAllocatedForPhysicalCapacity`. The skipped identity remains in the
  ledger as a terminal outcome, but is not unresolved, materialized, latent,
  pending activation, or a completion obligation. It creates no runtime
  enemy, score, destruction effect, or child audio and cannot return after
  death.
- Natural, player-shot, and Super Zapper-expiry releases use the same
  Level 7 capacity decision. Player-shot children that fit remain pending
  one-tick activation and count as physical occupants. Allocated
  Super-Zapper children retain their existing stillborn zap timer behavior.
  Converted Spiker-derived Tankers use the same release path.
- Generic death reconstruction is unchanged. Each allocated unresolved
  child becomes one generic obligation; the unallocated sibling remains
  terminal across repeated reconstruction. Tests also cover a reconstructed
  obligation later selected as a Tanker and another saturated one-child
  release.
- Completion continues to require all unresolved originals and cargo to be
  resolved. A materialized child blocks completion; the unallocated terminal
  sibling does not.
- The limit is gated by canonical Level 7 configuration identity. Levels 1–6,
  Level 8, and copied/synthetic configurations preserve complete-pair
  behavior.
- No Flipper or Tanker movement code/configuration changed. Existing Level 7
  movement tests remain in the focused validation.
- A zero-slot allocation after carrier removal is not reachable in canonical
  production: normal admission and every prior canonical release maintain at
  most seven physical occupants, and the current release removes one of
  them. The capacity helper is defensive for a full post-removal snapshot;
  no artificial over-capacity production scenario was added.

## Files changed for this correction

- `src/Tempest.Game/Levels/Level3WaveLedger.cs`
- `src/Tempest.Game/Levels/Level7WaveSelection.cs`
- `src/Tempest.Game/TempestGame.cs`
- `tests/Tempest.Game.Tests/Level3WaveLedgerTests.cs`
- `tests/Tempest.Game.Tests/Level7AdmissionPolicyTests.cs`
- `tests/Tempest.Game.Tests/Level7ProductionTests.cs`

The pre-existing candidate changes in `src/Tempest.Game/Gameplay/Spiker.cs`
and `src/Tempest.Game/Levels/LevelConfiguration.cs` remain unchanged by this
correction. `TEMPEST-DEVELOPMENT-NOTES.md`, `TEMPEST-BEHAVIOR-SPEC.md`, and
historical Level 7 reports were not modified.

The complete task is preserved in
[`../Prompts/2026-11-10-level7-saturated-cargo-allocation-correction-prompt.md`](../Prompts/2026-11-10-level7-saturated-cargo-allocation-correction-prompt.md).

## Validation

Test platform: **VSTest**, xUnit 2, .NET SDK `9.0.304`.

| Validation | Result |
|---|---|
| Saturated one-child regression | **PASS** — 1 passed, 0 failed, 0 skipped |
| Focused Level 7 production/admission, ledger, movement, and Level 3 cargo regressions | **PASS** — 101 passed, 0 failed, 0 skipped |
| Affected Release regressions across Levels 1–8, Tanker/cargo, movement, Spiker/Spike, death/re-entry, Super Zapper, rim/completion, progression, recording/playback, and campaign | **PASS** — 825 passed, 0 failed, 0 skipped |
| `dotnet test TempestCSharp.slnx --configuration Release --blame-hang-timeout 5min` | **PASS** — VSTest ran; 1,212 passed, 0 failed, 0 skipped. The Blame collector reported all tests finished and created no sequence file. |
| `dotnet build TempestCSharp.slnx --configuration Release` | **PASS** — 0 warnings, 0 errors |
| `git diff --check` | **PASS** |

One intermediate focused run exposed an incorrect expected exit state in the
new completion test: canonical Level 7 enters `AvoidSpikes`, not `ExitDelay`.
The assertion was corrected, the isolated test passed, and the focused,
affected, and full suites then passed.

## Repository state and manual handoff

- `HEAD` remains `8f1c29a7d3e6ef4150c8ab64264108c3c2a9547b`; the candidate is
  uncommitted. No staging, commit, or push was performed.
- The prompt and this report are present under `AI-Logs/`. The repository
  `.gitignore` ignores that directory, so those records do not appear in
  normal Git status output.
- The interactive game was not launched. Manual gameplay acceptance remains
  Neal's responsibility and is pending.

After automated validation, use:

```powershell
dotnet run --project src\Tempest.Game -- --level 7 --lives 10
```

Compare complete Level 7 wave difficulty against arcade/MAME, especially
whether Blaster death remains unlikely with little effort and whether repeated
deaths still produce an escalating workload. Where practical, verify that a
saturated Tanker release never exceeds seven live enemies and that less
crowded Tankers still release both children. Also check unchanged movement,
Spiker behavior, death/re-entry, and Levels 1–6. Do not claim manual
acceptance until Neal reports the result.
