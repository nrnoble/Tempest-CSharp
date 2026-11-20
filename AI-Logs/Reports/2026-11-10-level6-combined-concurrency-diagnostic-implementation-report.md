# Level 6 combined concurrency diagnostic — implementation candidate

Status: **IMPLEMENTATION CANDIDATE FOR MANUAL DIAGNOSTIC TESTING. NOT ACCEPTED
BEHAVIOR.** The paired prompt is preserved at
[`../Prompts/2026-11-10-level6-combined-concurrency-diagnostic-implementation-prompt.md`](../Prompts/2026-11-10-level6-combined-concurrency-diagnostic-implementation-prompt.md).

## Starting state

- Branch: `master`, tracking `origin/master`.
- HEAD matched the requested baseline:
  `94ad02f52f8fe7f1eec79d60ffd7d5114b8ee933`.
- Index and tracked worktree were clean; no unrelated untracked files were
  present.

## Implementation

- Added an explicit canonical Level 6 admission policy with ROM-backed
  ordinary species minima/maxima, seven-place occupancy, Tanker cargo
  availability, and the Spiker-before-Tanker minimum priority. Ordinary
  Flippers, Pulsars, and Fuseballs are not candidates.
- Admission uses live enemy collections, so rim, transition, pending-zap,
  released cargo, and shot-pending cargo continue occupying places. Latent
  cargo, Eggs, Spikes, and bullets do not. Each hatch re-reads those live
  counts; denied ready Eggs remain unresolved and do not reach construction
  randomness.
- After minima are satisfied, selection uses the pending Egg lane's persistent
  Spike. The `$CC` comparison reuses the established ROM-coordinate-to-C#
  height conversion as a **PROJECT TRANSLATION / APPROXIMATION**; it is not a
  literal C# height or a global shortest-Spike scan.
- Tanker selection requires positive cargo-adjusted Flipper availability but
  only one free place. Existing complete-pair cargo release and its possible
  temporary occupancy above seven remain unchanged; ordinary admission waits
  for headroom.
- A retreating Spiker converts in place only when the canonical Level 6
  ledger's generic pending-original count is zero. It retains its original
  identity and receives one normal latent cargo pair; the conversion bypasses
  ordinary Tanker availability.
- Canonical Level 6 now uses the shared generic-obligation death reconstruction
  translation. Existing unresolved original obligations lose species
  commitments; latent cargo is removed without adding obligations; each
  unresolved materialized child becomes one generic original obligation.
  Terminal outcomes remain terminal, and repeated death preserves the current
  unresolved count.
- Policy and reconstruction activation are scoped to the canonical Level 6
  configuration. Direct production startup, campaign progression, and
  recording/playback routes that use that configuration retain the policy;
  copied configurations do not acquire it. Level 1-5 and Level 7-8 policies
  were not changed.
- Tanker cargo behavior, Super Zapper behavior, and the known ROM admission
  pause during `zap_running` were left unchanged; the admission-pause mismatch
  remains deferred.

## Files changed

- `src/Tempest.Game/Levels/Level6AdmissionPolicy.cs` — Level 6 policy,
  lane-Spike lookup, availability, and terminal-conversion condition.
- `src/Tempest.Game/Levels/LevelConfiguration.cs` — canonical Level 6 generic
  reconstruction opt-in and configuration comment.
- `src/Tempest.Game/Levels/Level.cs` — direct Level 6 death reset uses generic
  reconstruction.
- `src/Tempest.Game/TempestGame.cs` — canonical Level 6 admission and terminal
  conversion wiring.
- `tests/Tempest.Game.Tests/Level6AdmissionPolicyTests.cs` — deterministic
  policy, lane selection, waiting, headroom, conversion, and reconstruction
  coverage.
- `tests/Tempest.Game.Tests/Level6ProductionTests.cs` — updated Level 6 death
  reconstruction expectations.
- `tests/Tempest.Game.Tests/ArcadeFlipperFiringTests.cs` — updated Level 6
  production expectations and retained firing checks in an isolated copied
  configuration.
- `tests/Tempest.Game.Tests/Level5ProductionTests.cs` — updated the generic
  reconstruction isolation assertion for canonical Level 6.

No durable behavior specification or development notes were changed. The
prompt and this report are ignored `AI-Logs/` records and are not staged.

## Tests and validation

- Focused Level 6 policy tests: **PASS**, 14 passed.
- Level 6 production tests: **PASS**, 4 passed.
- Shared Level 3 wave-ledger tests: **PASS**, 18 passed.
- Tanker-related tests: **PASS**, 114 passed.
- Arcade Flipper firing tests: **PASS**, 18 passed.
- Level 5 production tests: **PASS**, 18 passed.
- Full Release solution test:
  `dotnet test TempestCSharp.slnx --configuration Release --blame-hang-timeout 5min`
  — **PASS**, 1,182 passed, 0 failed.
- Release solution build:
  `dotnet build TempestCSharp.slnx --configuration Release`
  — **PASS**, 0 warnings, 0 errors.
- `git diff --check` — **PASS**.
- Final HEAD remained at the requested baseline. No staging, commit, push, or
  interactive game launch occurred.

The first broad regression run found assertions expecting ordinary Level 6
Flippers and no Level 6 generic reconstruction. Those superseded expectations
were updated; the firing holdoff/shot-capacity behavior remains covered.

## Open fidelity questions and manual handoff

No Level 6 arcade comparison was performed. Neal's manual review remains
necessary for dynamic Tanker/Spiker composition, the pending-lane `$CC`
translation, terminal Spiker conversion timing/presentation, cargo behavior
under crowding, and generic post-death composition. The complete-pair C#
cargo translation can temporarily exceed the ROM's seven physical places.
The ROM's Super Zapper admission pause is deliberately deferred.

After automated validation, Neal can launch the candidate with:

```powershell
dotnet run --project src\Tempest.Game -- --level 6 --lives 10
```

The command syntax is supported by the production-level and starting-lives
parsers. Manual acceptance is pending; the candidate remains uncommitted.
