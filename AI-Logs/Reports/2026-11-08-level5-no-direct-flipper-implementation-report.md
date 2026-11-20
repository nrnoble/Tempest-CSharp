# Level 5 no-direct-Flipper implementation candidate

Date: 2026-11-08

## Starting Git state

- Branch: `master`, tracking `origin/master`.
- Starting commit: `d2d8b1ef69b4a13be06ab59b41a1c9d38a8dd6e2`
  (`Close out accepted common type-0 destruction effect`).
- No tracked or staged changes were present before implementation.
- `screenshot000.png` through `screenshot013.png` were already untracked.
  Their SHA-256 hashes were unchanged after the work.

## ROM evidence and prior investigation

Reused `AI-Logs/Reports/2026-11-08-level1-7-enemy-supply-rom-investigation-report.md`
as required; this was not a repeat of its Levels 1-7 supply analysis. It
establishes that Level 5's base original supply is 20, matching the C#
configuration, and records the Level 5 per-type limits as Flipper 0/5,
Tanker 1/1, and Spiker 2/3.

Rechecked the project-local `Reference/arcade/Tempest Rom Source code.md`:

- `min_flippers` at `$94d1` sets the minimum to zero for Levels 5-99;
  `max_flippers` at `$94da` sets the maximum to five for Levels 5-16.
- Tanker minimum `$94f2` and maximum table `$9607_27` give Level 5 1/1.
- Spiker minimum `$9490` and maximum table `$9607_2f` give Level 5 2/3.
- The ordinary selector at `$9a0e-$9a24`, `$9a26-$9a3e`, and
  `$9a61-$9a86` checks nonzero type minima before trying selection, including
  the Flipper minimum in the random route.
- The base `wave_enemies` table at `$9607_4f` remains consistent with the
  Level 5 supply of 20.

This is **ROM-LISTING-BACKED** selection evidence. The Flipper maximum is not
permission for an ordinary Flipper bottom entry; it continues to account for
Flippers released as Tanker cargo.

## C# cause and implementation

`TempestGame.CreateLevel3MixedEnemy` routes mixed Flipper/Tanker/Spiker
production waves through `Level3WaveSelection.SelectLevel4OriginalType`.
That picker returned `Flipper` for candidate 1 without considering the
Level 5 minimum, so production Level 5 could hatch an ordinary original
Flipper.

Added `LevelConfiguration.AllowsOrdinaryFlipperSelection`, defaulting to
`true` and set to `false` only for `Level5Production`. The existing mixed
picker now skips a Flipper candidate when that policy is false; Tanker and
Spiker selection remain available. No parallel wave picker was introduced.

Cargo remains separate: `CreateLevel3MixedEnemy` handles an Egg carrying a
`CargoUnitId` before it enters original-species selection. The existing cargo
activation, release, ownership, movement, destruction, and completion routes
were not changed.

For death reconstruction, Level 5 clears only an unresolved original's
previous Flipper species selection so it must pass the no-direct-Flipper
policy again. Tanker selections and latent/materialized cargo identities are
preserved. The existing Level 3-6 identity ledger and completion accounting
remain intact.

## Changed files

- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/Levels/Level3WaveSelection.cs`
- `src/Tempest.Game/Levels/Level3WaveLedger.cs`
- `src/Tempest.Game/Levels/Level.cs`
- `src/Tempest.Game/TempestGame.cs`
- `tests/Tempest.Game.Tests/Level5ProductionTests.cs`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `TEMPEST-BEHAVIOR-SPEC.md`

The complete user task was copied unchanged to the ignored
`AI-Logs/Prompts/2026-11-08-level5-no-direct-flipper-implementation-prompt.md`.

## Automated evidence and validation

- **Ordinary selection:** 32 deterministic random seeds select no ordinary
  Flipper and exercise both Tanker and Spiker results. A production Level 5
  case also rejects scripted Flipper candidates while hatching two Tankers
  and two Spikers.
- **Supply and identities:** Configuration remains at 20 originals, four
  initial Eggs, and 16 reserve entries. The production selection test observes
  20 unresolved original identities and four distinct cargo identities;
  each Tanker owns its two cargo identities.
- **Cargo:** Existing Level 5 natural and player-shot Tanker-release tests
  still materialize the two cargo Flippers. Existing cargo death/re-entry tests
  still cover reconstruction and movement. A focused ledger test confirms the
  new Flipper-selection reset leaves Tanker commitments and latent cargo
  identities untouched.
- **Death/re-entry:** The production test performs two death/re-entry cycles,
  then hatches unresolved originals under the Level 5 policy. It confirms no
  ordinary Flippers, no lost original obligations, and unchanged Tanker/
  Spiker selection. A separate ledger reconstruction case confirms an
  unresolved prior Flipper selection is cleared without clearing Tanker cargo.
- **Completion:** Completion stays blocked after all original obligations are
  terminal while either cargo obligation remains; it becomes complete only
  after both cargo identities are terminal.
- **Isolation and gameplay regressions:** Level 4 and Level 6 keep ordinary
  Flipper selection enabled. Level 7 keeps its dedicated picker. The expanded
  regression included Level 3 mixed-wave/ledger behavior, Levels 4-7,
  configuration, progression, Flippers, Tankers/cargo, Spikers, death/
  re-entry, destruction effects, and Super Zapper.
- Focused Level 5 tests: **17 passed, 0 failed**.
- Expanded focused regression: **300 passed, 0 failed**.
- Full solution tests (`dotnet test TempestCSharp.slnx --no-restore`):
  **990 passed, 0 failed, 0 skipped**.
- Full solution build (`dotnet build TempestCSharp.slnx --no-restore`):
  **succeeded, 0 warnings, 0 errors**.
- `git diff --check`: **clean**.

## Final Git state

The eight listed source, test, and documentation files are modified and
unstaged. Nothing was staged, committed, or pushed. The fourteen screenshot
files remain untracked and their hashes are unchanged. No UserData,
recording, screenshot, or other generated/local files were modified.
The prompt and this report are under ignored `AI-Logs/`.

## Manual acceptance handoff

**MANUAL ACCEPTANCE PENDING.** Do not treat earlier Level 5 gameplay or
Flipper-movement acceptance as acceptance of this newly identified hidden
selection rule. The game was not launched.

Direct Level 5 launch command:

```powershell
dotnet run --project src\Tempest.Game -- --level 5 --lives 10
```

Suggested observations for Neal:

1. Start directly on Level 5 and watch several ordinary bottom entries.
   Ordinary Flippers should not enter directly; Tankers and Spikers should.
2. Destroy Tankers or let them reach their normal release point. Their cargo
   Flippers should still appear and behave normally.
3. Distinguish solitary/original bottom-entry Flippers (not expected) from
   Flippers appearing as Tanker cargo (expected).
4. If practical, take a death/re-entry and confirm ordinary bottom Flippers
   still do not appear afterward.
5. If practical, complete Level 5 and observe normal completion/progression
   without an apparent missing obligation or stuck wave.
6. Note any change in Level 5 pacing or clear-time impression, but do not use
   that as a pass/fail condition for this correction.

## Unresolved issues

- Neal's manual play-test remains required; no arcade acceptance is claimed.
- Level 6 is unchanged and remains a separate evaluation.
- ROM active caps, release/pending timing, and other listed species-limit
  differences remain separate potential milestones.
- The previously observed longer C# clear-time impression was not investigated
  or assumed to be explained or fixed by this picker correction.
