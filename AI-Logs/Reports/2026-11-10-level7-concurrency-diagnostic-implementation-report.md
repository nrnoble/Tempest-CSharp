# Level 7 concurrency diagnostic implementation

Date: 2026-11-10

Disposition: **AUTOMATED VALIDATION PASSED. CANDIDATE REMAINS UNCOMMITTED.
MANUAL LEVEL 7 ACCEPTANCE IS PENDING.**

## Starting state

- Branch: `master`, tracking `origin/master`.
- Starting `HEAD`: `8f1c29a7d3e6ef4150c8ab64264108c3c2a9547b`
  (`Implement accepted Level 6 concurrency and cargo movement`).
- The tracked worktree and index were clean before implementation.
- The complete substantive prompt and its standalone completion token are
  preserved in
  [the matching prompt record](../Prompts/2026-11-10-level7-concurrency-diagnostic-implementation-prompt.md).

## Implementation

- Updated `Level7WaveSelection` with seven physical places; Flipper 0/5,
  Tanker 1/2, and Spiker 2/4 availability; Spiker-before-Tanker minima;
  zero-minimum Flipper exclusion; and Tanker cargo availability. Converted
  Tankers remain physical occupants but do not consume the ordinary Tanker
  maximum.
- Availability counts active Flippers, ordinary and converted Tankers,
  Spikers, other live enemies, and shot cargo pending activation. Latent
  cargo affects Flipper availability but consumes no physical places.
  Occupancy is sampled anew for each Egg admitted during the hatch loop.
- Added the open-place dead-end behavior: with two Tankers and four Spikers,
  positive zero-minimum Flipper availability does not admit an ordinary
  Flipper; the ready Egg and its obligation remain pending.
- Post-minimum selection reads the pending Egg's own lane Spike. The
  established ROM-along conversion remains a **PROJECT TRANSLATION /
  APPROXIMATION**, not coordinate equivalence. Missing or depleted Spikes
  map to `$ff`; values at/above `$cc` select Spiker, lower values select
  Tanker. Invalid lane indices fail closed with
  `ArgumentOutOfRangeException`. The bounded random-start fallback is used
  only when no lane comparison is available; blocked selections consume no
  random values.
- Tanker cargo still bypasses ordinary species selection and needs physical
  headroom only when a cargo Egg is admitted. The established complete-pair
  release, cargo identities, movement, scoring, and deferred activation were
  not changed. Complete-pair release can exceed seven live objects; ordinary
  admission then waits for physical headroom.
- While authoritative ledger generic pending supply remains, a retreating
  canonical Level 7 Spiker keeps the same live identity and place and
  restarts its local building cycle. This is an explicit **PROJECT
  TRANSLATION**: it does not invent the ROM's lane hop. At actual ledger
  pending zero, existing same-place conversion to a Flipper-carrying Tanker
  remains, with one cargo pair allocated exactly once and no ordinary
  Tanker-maximum check.
- Verified and preserved generic death reconstruction; no reconstruction
  infrastructure was rewritten. Focused coverage checks latent cargo,
  materialized children, cleared species commitments, terminal outcomes,
  and repeated quantity conservation.
- The policy now applies only to the canonical production Level 7
  configuration. Direct, campaign, and recorded production startup retain
  that canonical configuration; copied configurations do not acquire the new
  Level 7 policy.
- The Super Zapper ordinary-entry pause remains deferred.
- **Movement freeze:** no Flipper/Tanker movement source or movement
  configuration was changed. The accepted `2-3-2-2-2` ordinary route and
  accepted cargo movement remain under their existing tests. The ROM `$24`
  conflict remains unresolved and separate.

## Files changed

- `src/Tempest.Game/Levels/Level7WaveSelection.cs`
- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/TempestGame.cs`
- `src/Tempest.Game/Gameplay/Spiker.cs`
- `tests/Tempest.Game.Tests/Level7AdmissionPolicyTests.cs`
- `tests/Tempest.Game.Tests/Level7ProductionTests.cs`

No behavior specification or development notes were changed.

## Validation

- Final focused Level 7 admission, production, and movement tests:
  **PASS**, 32 passed, 0 failed.
- Affected Release regressions covering Levels 1–8, Tanker/cargo,
  Spiker/Spike lifecycle, death/re-entry, Super Zapper, rim/completion,
  progression, recording/playback, campaign, and direct startup:
  **PASS**, 868 passed, 0 failed.
- Full requested command,
  `dotnet test TempestCSharp.slnx --configuration Release --blame-hang-timeout 5min`:
  **PASS**, 1,202 passed, 0 failed, 0 skipped. The Blame collector reported
  that all tests finished and generated no sequence file.
- `dotnet build TempestCSharp.slnx --configuration Release`: **PASS**, 0
  warnings, 0 errors.
- `git diff --check`: **PASS**.
- Reviewed the candidate diff. No movement implementation files changed.

## Final state and manual handoff

- `HEAD` remains at the requested baseline; the candidate is uncommitted.
- No commit or push was made. The interactive game was not launched.
- Manual Level 7 acceptance is still required. Supported command:

  ```powershell
  dotnet run --project src\Tempest.Game -- --level 7 --lives 10
  ```

- Neal should verify the concurrency checklist in the preserved prompt,
  including no ordinary bottom Flippers, species availability/dead-end
  behavior, complete-pair cargo, Spiker persistence and final conversion,
  death/re-entry accounting, and unchanged accepted Level 7 movement.
- Do not use this candidate to resolve the ROM `$24` movement conflict or
  begin Level 8 concurrency.
