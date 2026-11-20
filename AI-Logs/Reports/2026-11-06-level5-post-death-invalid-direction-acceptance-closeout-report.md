# Level 5 post-death invalid-direction acceptance closeout

Date: 2026-11-06

## Starting Git state

- Branch: `master`.
- `HEAD` and `origin/master` started at
  `11ddd630a3c6e01c18795658c9625280ca24ca8e`
  (`Extend recorded gameplay startup metadata`).
- Index was clean. The worktree contained the expected nine modified files
  for the fix, preceding normal-exit diagnostic, tests, and durable docs; no
  unrelated nonignored untracked files were present.
- The configured `origin` remote was unchanged.

## Manual acceptance and failure evidence

**LEVEL 5 POST-DEATH INVALID-DIRECTION FIX: MANUALLY ACCEPTED.**

Neal extensively tested production Level 5, repeatedly exercised the former
active-play/death/re-entry sequence without recurrence, verified stable
death/re-entry and ordinary Level 5 gameplay, and confirmed normal Level 5 to
Level 6 progression. No obvious Flipper, Spike, Tanker or cargo regression
was observed. All other currently implemented production levels were also
manually tested without an unexpected exit or obvious neighboring-level
regression. Several gameplay recordings and separate playbacks reproduced
gameplay correctly.

The original unhandled exception was
`ArgumentOutOfRangeException("direction")`, originating in
`Level.CalculateDirectedLaneTransition(..., direction)` through
`Flipper.BeginPivot()`. The supplied stack-trace summary identifies the
parameter and call path; the abnormal CLR process result was `-532462766`.
Raylib's `INFO: Window closed successfully` was cleanup output while the
exception unwound, not the initiating exit reason.

The proven production-realistic lifecycle was:

1. A production Level 5 Tanker released two cargo Flippers.
2. One cargo Flipper remained unresolved when its collision killed the
   Blaster.
3. Death/re-entry discarded runtime entities while preserving the unresolved
   cargo identity in the production ledger.
4. The identity returned as a fresh Egg.
5. `CreateFreshTankerCargo` reconstructed the Flipper with the default
   `Direction == 0`.
6. Level 5's `$53` Spike-sensitive policy could allow a below-rim pivot before
   cargo received a direction.
7. `BeginPivot` passed zero to the strict directed-transition geometry guard,
   which correctly threw `ArgumentOutOfRangeException("direction")`.

This is a proven C# state-initialization defect, not new arcade evidence. The
`$53` policy exposed the missing direction invariant; it was not proven wrong,
disabled, or weakened.

## Accepted correction and preserved behavior

- Fresh reconstructed Level 5 Tanker cargo receives a valid reference-shortest
  direction toward the player.
- `BeginPivot` checks that direction is `-1` or `+1` before constructing the
  directed lane transition.
- `CalculateDirectedLaneTransition` retains strict validation; invalid values
  are not clamped, ignored, or silently defaulted.
- Existing Level 7 programmed-direction restoration is unchanged.
- The `$53` gate, threshold, climbing behavior and timing are unchanged.
- The normal-exit diagnostic remains low-noise and reports either
  `WindowShouldClose` or `Enter` without changing exit selection or polling
  order. It does not catch or suppress gameplay exceptions.
- Unhandled gameplay exceptions continue to expose their type, message,
  parameter name, stack trace and nonzero process result through the ordinary
  .NET path.
- Recording-save behavior was not changed. The earlier failed run did not
  reach its normal recording-save path because the exception escaped first.

Neal supplied a Level 5 playback result that completed after 7,260 simulation
ticks, reported that live controls resumed, and later exited with
`Exit reason: WindowShouldClose`. No exception occurred and Raylib cleanup
completed normally. Manual acceptance does not claim that Neal isolated every
automated cargo identity or direction edge case. No recurrence of this crash
was observed in manual testing.

## Files and validation

Production files:

- `src/Tempest.Game/Gameplay/Flipper.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Systems/InputSystem.cs`
- `src/Tempest.Game/TempestGame.cs`

Test files:

- `tests/Tempest.Game.Tests/InputSystemTests.cs`
- `tests/Tempest.Game.Tests/Level5ProductionTests.cs`

Durable documentation files:

- `README.md`
- `TEMPEST-BEHAVIOR-SPEC.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`

Validation results:

- Focused Level 5, input/exit diagnostic, directed transition, and Level 4/6/7
  selections: **90 passed, 0 failed, 0 skipped**.
- Neighboring regression selection spanning Levels 1–7, cargo/ledger,
  Flipper/Tanker, death/re-entry, directed transitions, firing, Super Zapper,
  rim pursuit, progression, campaign soak, direct startup, recording/playback
  and input: **733 passed, 0 failed, 0 skipped**.
- `dotnet test TempestCSharp.slnx`: **874 passed, 0 failed, 0 skipped**.
- `dotnet build TempestCSharp.slnx`: **succeeded, 0 warnings, 0 errors**.
- `git diff --check`: **passed**.
- `git diff --cached --check`: **passed**.
- The nine-file staged patch had no unintended deletion, unrelated file, or
  unresolved unstaged tracked change.

## Commit and push

- Complete staged file list: the nine production, test, and documentation files
  listed above. `AI-Logs/` is intentionally ignored and was not staged.
- Commit message: `Fix reconstructed Level 5 cargo direction`
- Commit: `b290c0a17ff635a6ddeb40dfb5b2dc4bc0aa5d2b`
- Push: normal `git push` to the existing `origin` succeeded; no force-push,
  branch creation, or remote reconfiguration occurred.
- Final Git status: clean `master...origin/master`.
- Final `HEAD` and `origin/master` both identify
  `b290c0a17ff635a6ddeb40dfb5b2dc4bc0aa5d2b`.

No Level 8/9 work, unrelated gameplay changes, recording redesign, advanced
playback work, force-push, or remote reconfiguration occurred. Copilot did not
launch the interactive game; Neal's manual acceptance is recorded above.
