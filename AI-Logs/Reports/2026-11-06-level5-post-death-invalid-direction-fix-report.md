# Level 5 post-death invalid-direction fix

Date: 2026-11-06

## Summary

The Level 5 post-death failure was reproduced in a deterministic headless
production-simulation regression. The failure was a C# state-initialization
defect: a fresh Tanker-cargo Flipper reconstructed after death retained the
default `Direction == 0`, while the Level 5 `$53` Spike-sensitive path could
begin a below-rim pivot before assigning a direction. The lane-transition
geometry correctly rejected zero with
`ArgumentOutOfRangeException("direction")`.

Fresh reconstructed Level 5 cargo now receives a valid reference-shortest
direction toward the player. `BeginPivot` also guards its required direction
domain before creating a lane transition. The `$53` gate, its timing, and
other routes remain unchanged. This establishes a C# implementation defect
and correction, not new arcade evidence. Neal's manual acceptance remains
pending.

## Starting Git state

- Branch: `master`; expected base `11ddd630a3c6e01c18795658c9625280ca24ca8e`.
- The prior root-cause investigation's exit diagnostic and test changes were
  already uncommitted. The follow-up also began with unvalidated edits in
  `Flipper.cs`, `TempestGame.cs`, and `Level5ProductionTests.cs`.
- The previously reported index was clean. No unrelated changes were found
  during the follow-up; final state is recorded below.
- No stage, commit, push, reset, or cleanup was performed.

## Manual evidence and exact failure

Neal's control was fresh Level 5, no gameplay input, stationary Blaster death,
death/re-entry, and continued no-input play, repeated for multiple deaths; it
continued normally. In the failure case, Neal played Level 5 before death; the
death/re-entry presentation completed, then the game exited within seconds
without requiring post-re-entry input. Neal's trace identified an unhandled
`ArgumentOutOfRangeException("direction")` originating in
`Level.CalculateDirectedLaneTransition()` through `Flipper.BeginPivot()`.
The reported abnormal process result was `-532462766`.

The project has no gameplay-loop catch that suppresses such an exception.
`Program`'s `finally` can still close the Raylib window while the exception
unwinds, so `INFO: Window closed successfully` is cleanup output, not proof of
a normal exit. The development normal-exit diagnostic remains intact and
continues to distinguish `WindowShouldClose` from Enter; exception unwinding
does not become a normal exit or a success-shaped fallback.

Copilot did not launch the game. The process result and original stack trace
above are Neal's supplied evidence, not a result independently captured in
this session.

## Production lifecycle and root cause

The deterministic test uses the production Level 5 configuration and
deterministic Tanker selection:

1. Hatch a Tanker and let it climb to deep height (at least 250).
2. Fire a player shot into it, releasing its two production cargo Flippers.
3. Cause a released cargo Flipper to collide with the player and start death.
4. Complete death/re-entry; unresolved cargo identity returns as an Egg.
5. Hatch the cargo Egg and advance the reconstructed Flipper to its
   Spike-gated below-rim pivot.

The original Tanker release path creates cargo Flippers with a valid direction
from their release-pivot spawn. Death clears those active objects while the
ledger preserves unresolved cargo identity. The fresh-Egg reconstruction path
uses `CreateFreshTankerCargo`, which previously left `Direction` at its default
zero. The Level 5 policy can start a below-rim pivot after its live-Spike gate
opens; for cargo it intentionally did not assign a new direction, because
released cargo normally already had one. The later pivot attempted to build a
directed lane transition with zero and raised the reported exception.

This proves the missing direction initialization for reconstructed Level 5
cargo. The `$53` behavior exposed the invariant gap; the policy itself is not
disabled or changed. The automated test proves a sufficient
production-realistic active-play sequence, not that firing is necessary.
Natural Tanker cargo release was not reduced or tested as a smaller trigger.

The no-input automated control uses production Level 5 configuration, forces
the simulation into the death state without creating active Tanker cargo,
and advances repeated re-entry/no-input intervals. It is a useful
state-specific control, but it does not replace Neal's exact stationary-Blaster
manual control.

## Correction and regression risks

- At fresh Level 5 cargo reconstruction, compute a reference-shortest
  direction from the Egg lane toward the current player lane and pass it to
  `CreateFreshTankerCargo`.
- Keep Level 7 programmed-direction restoration unchanged.
- Validate the direction domain before `BeginPivot` calculates a transition.
- Preserve the Level 5 Spike gate, movement timing, enemy supply, and other
  Flipper routes.

The reference-shortest helper uses the existing reference tie rule; if the
Egg and player share a lane, that tie may consume one random value. No
non-tie direction assignment consumes randomness. This is the intended route
selection for a reconstructed Flipper and should be included in Neal's manual
acceptance. The `BeginPivot` guard makes any future invalid internal state fail
at the invariant boundary instead of later in geometry construction.

## Diagnostics and exit behavior

The pre-existing normal-exit diagnostic was retained in `Program.cs` and
`InputSystem.cs`; its tests remain. No catch, process-exit handling, recording
behavior, or application shutdown path was changed. Normal Enter/window-close
paths remain the only normal loop exits described in the prior root-cause
report. The Level 5 exception follows the uncaught managed-exception path;
cleanup may print Raylib's close message during unwinding.

## Validation

- Pre-fix exact regression, temporarily run with the reconstruction fix and
  early invariant guard removed: **1 passed**. It asserted
  `ArgumentOutOfRangeException.ParamName == "direction"`. The production fix
  and final test were restored afterward.
- Post-fix `Level5ProductionTests`: **13 passed, 0 failed**.
- Selected neighboring and related production suites (Levels 3–7, cargo and
  ledger, progression, soak, firing, direct startup, recording/playback,
  rim-pursuit, Super Zapper, and input): **290 passed, 0 failed**.
- Final `dotnet test TempestCSharp.slnx`: **874 passed, 0 failed, 0 skipped**.
- Final `dotnet build TempestCSharp.slnx`: **succeeded, 0 warnings, 0 errors**.
- `git diff --check`: **passed**.
- No interactive game launch or manual acceptance was performed.

## Files changed

- `src/Tempest.Game/Gameplay/Flipper.cs`
- `src/Tempest.Game/TempestGame.cs`
- `tests/Tempest.Game.Tests/Level5ProductionTests.cs`
- `README.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `TEMPEST-BEHAVIOR-SPEC.md`
- `src/Tempest.Game/Program.cs` (prior exit diagnostic)
- `src/Tempest.Game/Systems/InputSystem.cs` (prior exit diagnostic)
- `tests/Tempest.Game.Tests/InputSystemTests.cs` (prior diagnostic tests)
- This report and its reconstructed prompt record.

The original follow-up was an attachment whose payload was not present in the
resumed transcript. The paired prompt file is explicitly labeled as a
reconstruction from the retained task summary; it cannot preserve the
attachment's exact wording or completion token.

## Manual acceptance still required

Neal should verify the no-input Level 5 death/re-entry control, the former
active-play cargo/death failure sequence without post-re-entry input, and
ordinary Level 5-to-6 progression. Copilot must not launch the game.

Required general launch command:

```powershell
dotnet run --project src\Tempest.Game -- --lives 10
```

For direct Level 5 reproduction:

```powershell
dotnet run --project src\Tempest.Game -- --level 5 --lives 10
```

## Final Git state

- Branch: `master`, tracking `origin/master`.
- `HEAD` and `origin/master`: `11ddd630a3c6e01c18795658c9625280ca24ca8e`.
- The index is clean; no changes were staged.
- The tracked worktree contains the nine files listed above. The new
  `AI-Logs/` prompt and report are ignored by Git and are not listed as
  untracked files.
- No commit or push was made.
