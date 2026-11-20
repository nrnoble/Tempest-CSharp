# Level 6 Tanker Flipper `$0b` / `$07` implementation candidate

Date: 2026-11-10

Status: **AUTOMATED VALIDATION PASSED. IMPLEMENTATION CANDIDATE ONLY.
NEAL'S MANUAL ACCEPTANCE IS PENDING.**

Paired prompt: [`2026-11-10-level6-flipper-0b-07-implementation-prompt.md`](../Prompts/2026-11-10-level6-flipper-0b-07-implementation-prompt.md).

## Starting state and scope

- Branch: `master`, tracking `origin/master`.
- HEAD matched the specified baseline:
  `94ad02f52f8fe7f1eec79d60ffd7d5114b8ee933`.
- The existing Level 6 concurrency candidate was present in the worktree and
  was preserved. No unrelated candidate changes were reverted or restaged.
- Work was limited to canonical Level 6 Tanker-cargo below-rim movement.
  Admission, accounting, death reconstruction, other levels, and Super Zapper
  behavior were not redesigned.

## Evidence and implementation

- **ARCADE-OBSERVED (Neal):** Deep Level 6 cargo moves straight for eight
  advances, flips without climbing, then repeats the interval; each child
  retains its direction and the pair travel in opposite directions.
- **ROM-BACKED:** Deep cargo at ROM `enemy_along >= $20` uses Level 6 program
  `$0b` (eight along advances, a fixed-depth flip, then an immediate new
  interval). Near-rim/natural cargo below `$20` uses `$07` (straight movement
  until top handling). The two children start with opposite direction bits.
- **PROJECT TRANSLATION / APPROXIMATION:** Release selection uses the existing
  `Tanker.RomAlongPositionForHeight` mapping and the ROM `$20` boundary. The
  C# height scale is not asserted to be literally equivalent to the ROM byte.
  Selection occurs at Tanker release, from its preserved height and release
  cause, and is restricted to canonical Level 6 natural/player-shot releases.
  Super Zapper and copied/noncanonical configurations retain existing cargo
  movement.
- Level 6 `$0b` cargo starts climbing from its existing release lane without
  the common C# release pivot; after eight C# climb updates, a pivot runs
  without height advancement or a below-rim pause. Its next interval starts
  immediately and preserves direction. `$07` cargo climbs straight below the
  rim, then enters existing rim handling.
- The complete cargo pair, identities, accounting, spawn lanes, and opposite
  directions are preserved. Movement state belongs to the materialized
  Flipper and is not retained through death reconstruction.

## Files

Movement implementation:

- `src/Tempest.Game/Gameplay/Flipper.cs`
- `src/Tempest.Game/Gameplay/Tanker.cs`
- `src/Tempest.Game/TempestGame.cs`
- `tests/Tempest.Game.Tests/Level6TankerCargoMovementTests.cs`

The already-present Level 6 concurrency candidate remains in the same
uncommitted worktree:

- `src/Tempest.Game/Levels/Level.cs`
- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/Levels/Level6AdmissionPolicy.cs`
- `tests/Tempest.Game.Tests/ArcadeFlipperFiringTests.cs`
- `tests/Tempest.Game.Tests/Level5ProductionTests.cs`
- `tests/Tempest.Game.Tests/Level6AdmissionPolicyTests.cs`
- `tests/Tempest.Game.Tests/Level6ProductionTests.cs`

No durable behavior specification or development notes were changed.

## Validation

All automated validation used Release configuration.

- New Level 6 movement tests: **PASS**, 5 passed.
- Focused Level 6/admission/cargo/Flipper/Spiker/Spike/death batch: **PASS**,
  383 passed.
- Affected Levels 1-8, Tanker, recording/playback, progression, Super Zapper,
  rim/completion, and campaign batch: **PASS**, 689 passed.
- `dotnet test TempestCSharp.slnx --configuration Release --blame-hang-timeout 5min`:
  **PASS**, 1,189 passed, 0 failed, 0 skipped. The blame collector reported
  all tests finished and generated no sequence file.
- `dotnet build TempestCSharp.slnx --configuration Release`: **PASS**, 0
  warnings, 0 errors.
- Complete tracked candidate diff reviewed; `git diff --check`: **PASS**.
- No game launch, staging, commit, or push occurred.

## Repository state and manual handoff

HEAD remains at the requested baseline on `master`; the index is unchanged.
The entire Level 6 candidate remains uncommitted. No Level 7 concurrency work
was started.

Neal's manual acceptance is still required. Launch with:

```powershell
dotnet run --project src\Tempest.Game -- --level 6 --lives 10
```

Compare deep player-shot Tanker releases for the eight-advance/held-depth flip
cadence and persistent opposite directions. Compare natural near-rim splits
for straight below-rim climbing and normal rim handoff. Also verify the
existing concurrency, pending-lane Spike, death/reentry, and Levels 1-5
behavior. Do not treat this implementation report or automated results as
manual acceptance.
