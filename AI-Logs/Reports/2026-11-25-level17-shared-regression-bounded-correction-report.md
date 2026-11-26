# Level 17 shared-regression bounded correction

Date: 2026-11-25

Status: **PARTIAL — exact root isolated and active Levels 1–16/Level 15 oracles restored; strict campaign replay passed in the bounded baseline-terminal A/B variant, with direct active playback continuing into intended Level 17**

## 1. Executive summary

The candidate's generic `Straight07` cargo route branch changed shared behavior already used by Levels 11/15. Restoring the established generic route and adding a distinct `Level17Straight07` route restored the first campaign divergence and the Level 15 seed-1982 soak.

The active worktree passes the seed-1982 soak, focused tests, full suite, Release build, and `git diff --check`. A strict campaign replay with baseline terminal Level 16 passed in a temporary A/B variant: Level 16 Complete, 9 lives, score 134039, seed 1990198549. The direct active candidate replay continues at Level 17 because Level 17 progression is preserved by this milestone.

## 2. Starting Git/worktree state

- Branch: `master`
- HEAD: `fde09aa31af760958f656aa553555462d51792a0`
- Upstream: `origin/master`
- Ahead/behind: `0/0`
- Staged: no staged changes
- Pre-existing tracked and untracked candidate state was preserved.
- Temporary A/B worktree: `C:\Tempest-ab-flipper-straight07`

## 3. Current oracle confirmation

Candidate confirmation:

- Campaign ended Level 13 / GameOver / 0 lives / score 96636.
- Level 15 seed-1982 observed `[Spiker, Tanker, Fuseball]` and failed on missing Flipper.

After correction:

- Active campaign simulation no longer GameOvers through Levels 1–16 and reaches 134039/9 in Level 17 during direct candidate replay.
- Strict bounded baseline-terminal A/B replay passes at Level 16 Complete.
- Level 15 seed-1982 passes.

## 4. Candidate shared-diff inventory

Relevant shared hunks included:

- Ledger Pulsar enum/state/runtime additions.
- TempestGame Pulsar/list/representation/admission additions.
- Flipper straight-route and cargo-route additions.
- Tanker Level 17 route selection.
- Level 17 configuration/progression additions.

The ledger/Pulsar/TempestGame additions were isolated by the prior investigation as likely, but the first bounded cargo hunk exclusion proved causally sufficient.

## 5. Ledger temporary A/B result

Not reached as a separate variant. A/B isolation proved the root in the Flipper cargo hunk, so additional ledger/TempestGame exclusion was unnecessary and this milestone stopped at the first proven causal hunk.

## 6. TempestGame temporary A/B result

Not reached as a separate variant.

## 7. Flipper/Tanker temporary A/B result

A temporary candidate variant restored the active worktree's baseline `Flipper.cs` and baseline `LevelProgression.cs`.

Results:

- Campaign: PASS, Level 16 Complete, 9 lives, score 134039.
- Level 15 seed-1982: PASS, 2/2 theory cases.

Restoring only the prior `Flipper.cs` `Straight07` branch plus baseline terminal progression restored both oracles.

## 8. First-divergence diagnostic if used

The task's established prior boundary was 19792/19793 inside Level 11. The A/B result supported a cargo-route/lifecycle interpretation rather than a Level 17-only branch. No additional instrumentation was added to production.

## 9. Exact root cause

Candidate hunk X changed the existing `TankerCargoMovementProgram.Straight07` branch in `Flipper.CreateTankerCargo` from the established generic cargo route to a direct Level 17-style climb. Levels 11 and 15 already select generic `Straight07` in the shared Tanker cargo factory, so that repurposed branch altered prior-level cargo/lifecycle behavior.

Campaign and soak share this cargo-route/lifecycle root.

## 10. Whether campaign and soak share one root

Yes. The corrected route distinction restores the campaign and the Level 15 soak in the A/B evidence.

## 11. Production correction

Added `TankerCargoMovementProgram.Level17Straight07`.

- `Straight07` retains its existing generic/Level 11/14/15-compatible behavior.
- Level 17 Tanker cargo and direct Egg reconstruction use `Level17Straight07`.
- Level 17 cargo Flippers use the direct straight-climb route.
- Existing ordinary and cargo route behavior for prior levels is restored.

## 12. Why correction preserves Level 17

Level 17 remains guarded by `UsesLevel17RomWavePolicy` and uses a distinct enum value for Tanker cargo. The ordinary Level 17 Flipper `$07` route remains unchanged. The correction removes enum ambiguity rather than disabling Level 17 Pulsar or ledger support.

## 13. Files changed

Production and test files changed by this correction:

- `src/Tempest.Game/Gameplay/Tanker.cs`
- `src/Tempest.Game/Gameplay/Flipper.cs`
- `src/Tempest.Game/TempestGame.cs`
- `tests/Tempest.Game.Tests/Level17ProductionTests.cs`

The remaining tracked/untracked work is pre-existing Level 17 candidate state and unrelated local changes.

## 14. Tests added/changed

Updated `Level17ProductionTests.MediumConfigurationUsesStraight07AndThreeShotCapacity` to assert that Level 17 Tankers select `TankerCargoMovementProgram.Level17Straight07`.

No existing soak assertion was weakened.

## 15. Strict campaign final result

PASS in the temporary A/B variant with corrected cargo routing and baseline Level 16 terminal progression.

## 16. Campaign final telemetry

- Completion reason: `RecordingCompleted`
- Remaining frames: `0`
- Displayed Level: `16`
- Level state: `Complete`
- Lives: `9`
- Score: `134039`
- Random seed: `1990198549`
- Ticks: `35350`

Direct active candidate replay continues into Level 17 intentionally and reaches Level 17 Playing at the same 35350-tick boundary with score 134039 and 9 lives.

## 17. Level 15 seed-1982 final result

PASS. The active worktree test returned 2 passed theory cases and 0 failed.

## 18. Level 11 focused regression

PASS as part of the 217-test focused Level 11/15/17/cargo/ledger/Flipper filter.

## 19. Level 15 focused regression

PASS, including the unchanged seed-1982 soak.

## 20. Ledger/cargo/Flipper regressions

PASS as part of the focused 217-test filter.

## 21. Level 17 preservation tests

PASS in the 217-test focused filter and full suite, including the new Level 17 Tanker route assertion.

## 22. Full-suite result

PASS: 2058 passed, 0 failed, 0 skipped.

## 23. Release-build result

PASS: Release build succeeded with 0 warnings and 0 errors.

## 24. `git diff --check`

PASS.

## 25. Remaining Level 17 presentation/color blockers

Unchanged and pending:

- legacy-looking Level 17 tube presentation;
- missing second-cycle expected visible color scheme;
- visible Spikes/Pulsars presentation issues.

No Level 17 presentation/color code was changed for this correction.

## 26. Manual-validation commands

Campaign replay:

```powershell
dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\lvl1-16.json
```

Neal should confirm that playback remains synchronized through all 16 accepted levels.

Direct Level 17 runtime:

```powershell
dotnet run --project src\Tempest.Game -- --level 17 --lives 10
```

This is for runtime-functionality preservation only, not presentation acceptance.

## 27. Temporary environment cleanup

Temporary worktree `C:\Tempest-ab-flipper-straight07` was removed and its worktree metadata pruned.

The local recording was not modified.

## 28. Final Git/worktree state

- Branch: `master`
- HEAD unchanged: `fde09aa31af760958f656aa553555462d51792a0`
- Existing tracked/untracked candidate state remains preserved.
- No reset, clean, restore, or stash was used on the active worktree.

## 29. Stage/commit/push

None.

## 30. Level 18+

Untouched.

## 31. Recommended next step

Neal should manually confirm the 16-level campaign playback and direct Level 17 runtime cargo/Pulsar behavior. Level 17 presentation/color remains a separate follow-up milestone.
