# Level 7 arcade-fidelity corrections implementation report

Date: 2026-11-04

## Starting repository state

The repository was on `master` with an existing uncommitted Level 7 production
implementation, tests, and documentation. The worktree was not clean before
this task. Those changes were preserved and extended; no reset, discard,
commit, or push was performed. No unrelated unexpected worktree change was
identified during the initial inspection.

The authoritative inputs were the behavior specification, development notes,
the Level 7 ROM investigation and Flipper ROM analysis, existing Level 7
implementation/tests, and the cited annotated ROM source.

## Files changed by this correction

- `README.md`
- `TEMPEST-BEHAVIOR-SPEC.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `src/Tempest.Game/Gameplay/Flipper.cs`
- `src/Tempest.Game/Gameplay/Tanker.cs`
- `src/Tempest.Game/Levels/Level.cs`
- `src/Tempest.Game/Levels/Level3WaveLedger.cs`
- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/Levels/Level7WaveSelection.cs` (new)
- `src/Tempest.Game/TempestGame.cs`
- `tests/Tempest.Game.Tests/ArcadeFlipperFiringTests.cs`
- `tests/Tempest.Game.Tests/Level7FlipperMovementTests.cs`
- `tests/Tempest.Game.Tests/Level7ProductionTests.cs`

Other modified or new files shown by final Git status were already changed in
the initial uncommitted Level 7 worktree and were retained without alteration
for this correction. The requested substantive prompt was copied to
`AI-Logs/Prompts/2026-11-04-level7-arcade-corrections-implementation-prompt.md`.
The AI log directory is ignored by Git.

## Implemented behavior and evidence distinctions

### Movement

**ARCADE VERIFIED:** Neal's visible Level 7 below-rim lane changes are
`2-3-2-2-2`. This supersedes the earlier C# interpretation of a repeating
two/three pattern. Level 7 now uses a per-Flipper five-block counter and
reverses at each block boundary. Wrap-around, repeated cycle boundaries,
direction reversals, player independence, and independent ordinary Flipper
route counters are tested. Accepted rim pursuit, inward directed pivots,
geometry, and full reflex sweeps remain on their prior paths.

**ROM-BACKED:** The ROM movement program separately traces as 2/reverse/3/reverse.
The direct arcade observation, not an extrapolation from that ROM trace,
controls the corrected five-block visible pattern.

**PROJECT APPROXIMATION:** The route's cycle repetition/boundary, initial C#
physical direction, and existing 60-Hz angle/height cadence are not literal
ROM timing or verified lane handedness.

### Ordinary bottom selection and Level 7 picker

Level 7 opts into a dedicated picker; Levels 1-6 keep using their prior shared
selection path. The Level 7 picker excludes ordinary Flippers, enforces the
ROM-backed Tanker 1/2 and Spiker 2/4 minimum/maximum counts, and caps generic
active enemy selection at six. Tankers continue to carry two distinct Flipper
children. A Tanker occupies one original/generic wave identity; latent cargo
does not add pending supply or consume extra units. Cargo children are
materialized separately at release. Deterministic seeded and picker-boundary
tests cover the no-ordinary-Flipper rule, eligible species, limits, and cargo
accounting.

### Death and re-entry

**ROM-BACKED:** Level 7 death reconstruction preserves generic unresolved
quantity but not prior unresolved species, cargo linkage, or movement state.
The Level 7 ledger clears unresolved original species and converts released
cargo bookkeeping units to generic re-entry records. It discards latent cargo
attached to an unresolved Tanker, whose identity is itself reconstructed as a
generic unit. Terminal outcomes remain terminal.

The shared identity ledger remains intact for Levels 1-6. Score, life
deduction, remaining Super Zapper charges and flash, damaged Spikes, and
resolved ledger state are covered by Level 7 regression tests. Tests cover a
literal one-unit reconstruction selecting a Spiker and released cargo
returning as generic pending supply.

### Final-pending Spiker conversion

**ROM-BACKED:** The traced Spiker checks generic pending supply and, when none
remains, rewrites the same enemy slot into a Flipper-carrying Tanker at the
bottom. This is not a two-Flipper merge, new supply unit, ordinary Flipper
hatch, or second pending decrement.

**PROJECT APPROXIMATION:** The C# mapping triggers when the final pending
Spiker completes its existing return-to-bottom movement and creates the
Tanker at the existing C# bottom-entry height. The same ledger identity is
changed from active Spiker to active Tanker, with only two latent cargo
children added. This preserves logical wave quantity; the ROM's along
coordinates and exact movement timing are not equated to C# heights.

### Documentation and scope

The README, behavior specification, and development notes now separate
arcade-observed movement/no-bottom-Flipper behavior, ROM-backed picker/death/
Tanker rules, and C# project approximations. Level 8 remains out of scope.
Levels 1-6 were not intentionally changed. No interactive game was launched.

## Tests

Focused Level 7 movement, production, picker, and firing selection:

- **20 passed, 0 failed**

Touched-seam regression selection, including Level 3 ledger/mixed-wave,
Levels 4-6 production, configuration, progression, campaign soak, exit lane,
Level 7, and firing tests:

- **191 passed, 0 failed**

Complete solution suite (`dotnet test TempestCSharp.slnx`):

- **744 passed, 0 failed, 0 skipped**

The passing runs discovered tests; no zero-test result was counted as success.

## Build and diff validation

- `dotnet build TempestCSharp.slnx`: succeeded, 0 warnings, 0 errors.
- `git diff --check`: passed. Git emitted only existing LF-to-CRLF working-copy
  advisory messages; no whitespace error was reported.

## Final repository state

Final status was `## master`, with the existing uncommitted Level 7 work and
this correction still available in the worktree. The changed/new files listed
above were not committed. Git reported no commit or push.

## Unresolved issues and manual acceptance

- Neal's final Level 7 gameplay acceptance is **PENDING**.
- The exact C# movement cadence and physical handedness remain approximations.
- The Spiker-to-Tanker ROM coordinate transition is mapped to the C# bottom
  return boundary as an approximation.
- No Level 8 behavior was implemented.

Manual launch command for Neal:

```powershell
dotnet run --project src\Tempest.Game -- --level7-test --lives 10
```

Suggested visible acceptance checklist:

1. No individual Flippers appear directly from the bottom.
2. Tankers appear from the bottom and release Flippers normally.
3. Follow Flippers below the rim and verify visible `2-3-2-2-2` lane changes.
4. Lose a life with enemy types in different states and verify prior species
   are not obviously reconstructed individually from the bottom.
5. If practical, reproduce the one-visible-Flipper death scenario and check
   that the remaining Flipper does not return individually.
6. Observe late-wave Spiker/Tanker behavior for a sensible final-pending
   conversion.
7. Verify Tanker-released Flippers behave normally.
8. Check Spikers, Spikes, firing, Super Zapper, audio, and player controls.
9. Verify Level 7 completion and terminal campaign behavior.
10. Note any obvious regression or behavior inconsistent with the arcade.

Do not treat automated results as final gameplay acceptance.
