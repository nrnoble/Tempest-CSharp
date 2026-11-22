# Level 11 runtime representation crash correction

Date: 2026-11-21

Disposition: **AUTOMATED VALIDATION PASS; MANUAL CRASH GATE AND GAMEPLAY /
PRESENTATION ACCEPTANCE PENDING.** Candidate remains uncommitted.

## Starting state and scope

- Branch `master`, upstream `origin/master`, HEAD
  `5915096903d78cb43a6edad6c2d2cb071be73fff`; index was empty.
- The uncommitted Level 11 candidate and unrelated existing work were
  preserved. No staging, commit, push, interactive game, ROM, screenshot,
  video, Temp or UserData inspection occurred.
- Prompt saved in the paired AI-Logs prompt record. AI-Logs remained unstaged.

## Deterministic failure and evidence

The original unseeded launch's random stream is unavailable. A headless
`Random(4)` Level 11 run independently reproduces the reported exact identity
and exception shape: **before correction, FAIL** at simulation tick 127.

Immediately before the tick, original 4 was an approaching Egg at lane 4,
height -2, with update counter 7. Two Spikers and one Tanker were active; the
Tanker owned its latent pair. A Level 11 Spike existed on lane 4. The open tube
has playable lanes 0-14; lane 4 remained lane 4, with no remapping. The Egg
advanced to ready, and the Level 11 minimum-driven dynamic selector chose
**Fuseball**. There is no Spike-avoidance or lane rejection in this Fuseball
admission path; the existing Egg lane was used directly. No placement was
deferred in this specific transition.

The factory committed original 4 from Egg to `ActiveFuseball`, constructed a
Fuseball without transferring the original ID, and returned it. The hatch loop
removed the Egg and added the Fuseball to the active list and collision order.
The Fuseball object was not removed; rather, the runtime representation
collector omitted the entire Fuseball list. Thus the active ledger obligation
had no corresponding identifiable representation at the end-of-step
validation boundary. The validator correctly threw; other original identities
remained present. This is a direct-Fuseball lifecycle integration gap, not an
open-lane mapping or Level 6 pending-lane rejection defect.

The historical 2026-11-10 Level 6 report describes a different problem:
depleted Spikes are removed as objects, and a valid pending lane must map to
the ROM's zero-height sentinel rather than throw. Its invariant and
fail-closed handling were reviewed; its lookup was not copied or altered.

## Correction

- `Gameplay/Fuseball.cs`: carry an optional stable original ID; development
  Fuseballs retain their existing default.
- `TempestGame.cs`: transfer that ID at direct hatch; include Fuseballs in
  runtime type/representation validation, collision-order consistency,
  active-enemy counting and all-rim completion. Shot and zap destruction now
  resolve the production ledger identity rather than only the development
  counter. Death uses the existing generic reconstruction path.
- `Level3WaveLedger.cs`: prevent an already selected Fuseball from being
  recommitted as another original species.
- Level 11 admission now counts Fuseballs and reserves one additional
  physical slot per active Tanker for its second cargo child. **PROJECT
  TRANSLATION**: a ready Egg waits without losing its identity when that
  reserved capacity is unavailable. This is an implementation safeguard, not a
  claim about literal arcade admission timing. Manual gameplay acceptance is
  required.
- The longer reproduction then exposed a second deterministic exception:
  the existing Level 11 `$24` and `$07` Tanker cargo program values reached
  a Flipper factory that rejected them. `Gameplay/Flipper.cs` now routes the
  existing `$24` selection through the repeating programmed pivot and `$07`
  through the existing straight cargo path. These are project translations;
  arcade movement fidelity has not been manually accepted.
- `Level11ProductionTests.cs`: regression, bounded soak, capacity deferral and
  later activation, direct shot/zap resolution, death reconstruction, all-rim
  accounting, and both cargo-program factory paths.

No evidence geometry, supply, selection limits, Spike values, presentation,
HUD, audio, scoring rules, lives or progression were intentionally changed.
The seven-slot reservation is an admission behavior change needed to preserve
the requested physical-capacity invariant; it can affect timing and requires
manual acceptance.

## Identity count qualification

Level 11 starts with **27 original identities**, verified at the triggering
transition and after deferred activation and the unreleased-carrier death
reconstruction. The existing ledger also allocates two *separate cargo
identities* per selected Tanker, so its `TotalIdentityCount` can exceed 27
without duplicating an original. Existing generic death reconstruction can
convert materialized cargo into new generic original obligations. Requiring
the ledger's total count to remain literally 27 in those cases would contradict
the existing cargo/reconstruction model; it was not achieved by deleting
obligations or changing that model.

## Validation

- Before correction, seeded exact regression: **FAIL as expected**, original 4
  missing at ActiveEnemy on tick 127.
- Final exact regression:
  `dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --configuration Release --no-build --no-restore --filter "FullyQualifiedName~SeededProductionWaveConservesEveryIdentityBeyondStartup" --blame-hang-timeout 2min`:
  **PASS**, 1/1. It runs 1,000 additional steps after the triggering tick and
  asserts over 300 active-play steps (more than five seconds at 60 Hz), with
  production validation at its normal boundaries, capacity and lane checks.
- Final combined Level 11, Level 3 ledger/mixed-wave/admission, Level 6
  pending-lane/Spike, enemy/cargo, zapper, Level 9, presentation and HUD batch
  with `--blame-hang-timeout 3min`: **PASS**, 744/744.
- Complete solution:
  `dotnet test TempestCSharp.slnx --configuration Release --no-restore --blame-hang-timeout 5min`:
  **PASS**, 1,785/1,785.
- Release solution build with 120-second process bound: **PASS**, zero warnings
  and errors.
- `git diff --check`: **PASS**, with Git LF-to-CRLF working-copy warnings.
- During test development an initial shot test hit the pre-existing Spike
  first, and one temporary assertion referenced inaccessible internals; these
  were corrected in the test fixture. Neither was a production failure.

## Remaining acceptance

Neal should first run Level 11 beyond the previous crash gate:

```powershell
dotnet run --project src\Tempest.Game -- --level 11
```

For Atari presentation:

```powershell
dotnet run --project src\Tempest.Game -- --atari-presentation --level 11
```

Gameplay, admission/cargo timing, movement, and presentation remain pending
manual acceptance. Automated tests do not establish arcade fidelity.
