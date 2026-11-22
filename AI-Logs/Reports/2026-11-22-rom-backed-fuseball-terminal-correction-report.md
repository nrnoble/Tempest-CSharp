# ROM-Backed Fuseball Terminal Movement Correction

Date: 2026-11-22

## Outcome

**IMPLEMENTED — AUTOMATED VALIDATION PASS — MANUAL ACCEPTANCE PENDING.**

The shared Fuseball movement path now receives Level 11's generic pending
original count and enters a separate terminal-rimward state at its qualified
toward-rim boundary. It retains ordinary randomized movement while pending
supply remains, does not poll pending supply during unrelated states, and has
no fixed extra-traversal counter.

The original investigation handoff
([read-only report](2026-11-22-fuseball-terminal-behavior-readonly-report.md))
was used without repeating ROM or arcade investigation. No ROMs, screenshots,
videos, or local gameplay material were inspected.

## Implementation and evidence classification

- **ROM-BACKED behavior from the supplied handoff:** the Atari terminal check
  occurs on the toward-player leg at the player-side boundary. Zero generic
  pending supply bypasses ordinary reversal, retaining rimward motion; active
  enemies, rim occupancy, and a fixed traversal count are not the trigger.
- **PROJECT TRANSLATION:** C# `Climbing` represents the toward-player/rim leg.
  C# has no separate `player_along` coordinate, so the final climb step where
  `Height + ClimbSpeed` reaches or passes `Level.Depth` is the closest
  defensible decision boundary. Level 11's predicate is sampled there, before
  the ordinary endpoint handling. If supply is zero, the state changes to
  `TerminalRimward`, suppressing the ordinary return to lateral cycling.
- `TerminalRimward` advances height by the existing normal `ClimbSpeed`,
  clamped at the rim. It performs no random direction selection and does not
  initiate wave completion. The existing all-rim/completion path remains
  responsible for exit after the Fuseball reaches the rim and obligations are
  resolved.
- `TempestGame` supplies `Level3Ledger.PendingGenericOriginalCount` only for
  the Level 11 production policy. Other callers pass no terminal semantics.
  The terminal state remains independent of the development-only Hurry-Up
  latch. Terminal Fuseballs remain shootable despite holding a lateral
  endpoint, preserving the existing shot-to-ledger identity resolution path.
- The new state is movement state. Death reconstruction continues to discard
  runtime enemy objects and rebuild unresolved identities through the existing
  ledger path.

The C# boundary mapping is an intentional PROJECT TRANSLATION, not a claim
that `Level.Depth` or the C# climb threshold is a literal Atari coordinate.
Neal's manual observation remains necessary to accept its visible timing.

## Changed files

- `src/Tempest.Game/Gameplay/Fuseball.cs`
- `src/Tempest.Game/TempestGame.cs`
- `tests/Tempest.Game.Tests/FuseballTerminalMovementTests.cs`
- `AI-Logs/Prompts/2026-11-22-rom-backed-fuseball-terminal-correction-prompt.md`
- `AI-Logs/Reports/2026-11-22-rom-backed-fuseball-terminal-correction-report.md`

The two `AI-Logs` records remain untracked working material and were not
staged. Existing unrelated Level 11/15 candidate changes and other worktree
changes were preserved.
No durable behavior documentation was changed before manual acceptance.

## Automated coverage and validation

New tests cover ordinary cycling with pending supply, delayed boundary
evaluation, variable delay from different movement states, deterministic
timing, terminal rimward travel, production ledger sourcing with a rim Flipper,
completion waiting for other active obligations, terminal shot destruction,
identity validation, and death/re-entry reconstruction.

- **PASS** — focused terminal tests: 11 passed.
- **PASS** — Fuseball/Hurry-Up, Level 11 production, and Level 15 identity
  regressions: 108 passed.
- **PASS** — complete solution test suite: 1,810 passed, 0 failed.
- **PASS** — Release solution build: 0 warnings, 0 errors.
- **PASS** — `git diff --check`; the new untracked test file also has no
  trailing whitespace.

Tests used VSTest hang bounds (`60s`, `120s`, and `180s` by run). No interactive
game was launched.

## Remaining manual acceptance

Run the production Level 11 gameplay and intentionally preserve a Fuseball
late in the wave. Confirm:

- ordinary Fuseball cycling while generic pending supply remains;
- supply reaching zero does not cause an unnatural immediate reversal;
- the next qualifying toward-rim boundary suppresses another ordinary
  traversal;
- the Fuseball continues to the rim, with apparent extra traversals varying
  according to its state when supply reaches zero;
- all-rim exit waits until the Fuseball reaches the rim; and
- the wave completes normally.

```powershell
dotnet run --project src\Tempest.Game -- --level 11
```

For continued presentation observation, preserve the current calibration
candidate:

```powershell
dotnet run --project src\Tempest.Game -- --atari-presentation --level 11 --tube-y-gain 1.5 --tube-scale 1.1
```

Do not encode those presentation values as defaults. After Neal accepts the
behavior, update the appropriate durable behavior documentation with the
accepted state mapping, evidence classification, and fidelity boundary.

No files were staged, committed, or pushed.
