# Prompt: Fix Direct Flipper Rim-Arrival Collision Bug

Focused bug-fix task (no commit, no push). Do not modify Tanker cargo behavior,
Tanker accounting, Spiker behavior, enemy bullets, level progression, exit
behavior, rendering, controls, or unrelated gameplay.

## Starting state

- `dotnet build` passes; `dotnet test`: 142 total, 142 passed.
- Tanker implementation and its two manual-validation corrections work.

## Observed bug

When a Flipper climbs to the rim in the exact lane occupied by the blaster, it
reaches the blaster but does not destroy it. It enters its rim pause, changes
lanes, and chases the blaster.

## Expected behavior

If an undestroyed climbing Flipper reaches `Level.Depth` in the lane currently
occupied by the blaster:

- the blaster enters the established player-death lifecycle;
- the existing 60-tick death sequence runs;
- exactly one life is deducted;
- the level restarts with only unresolved enemies entering as fresh Eggs;
- destroyed enemies and earned score are preserved.

## Player-shot ordering

Preserve the `TempestGame` order: (1) player input and firing, (2) player-shot
movement and collision, (3) enemy updates, (4) enemy-to-player collision. A shot
that destroys the Flipper in that step wins before the Flipper can kill the
blaster. Do not move player-collision handling ahead of shot collision. One event
must not cause both a Flipper kill score and a player death.

## Reference check (before editing)

Inspect `.github/copilot-instructions.md`, `TEMPEST-DEVELOPMENT-NOTES.md`,
`Gameplay/Flipper.cs`, `TempestGame.cs`, `SimulationTests.cs`,
`Reference/incitti/tempest.bmx`, `Reference/incitti/Readme.txt`, and
`AI-Logs/2026-10-30-tanker-implementation.md`. Everything under
`Reference/incitti` is reference-only and must never be modified.

Recheck Incitti's direct Flipper-to-player collision and distinguish:

- direct climbing arrival into the occupied player lane;
- completion of a visible pivot into the occupied player lane;
- a player moving underneath a Flipper during a pivot and leaving before completion;
- a player shot destroying the Flipper on the final possible tick.

Do not weaken or replace the established pivot-completion collision behavior.

## Implementation constraints

- Smallest practical correction.
- Prefer one explicit collision signal from Flipper consumed by `TempestGame`
  through the existing `CheckFlippersAgainstPlayer` path.
- Do not call `TempestGame` player-death logic from the Flipper entity.
- Do not add Raylib, input, score, lives, or global dependencies to Flipper.
- Avoid two overlapping flags if one clearly named signal can represent both
  lethal outcomes.
- No broad refactor of the Flipper state machine.
- Do not alter climb speed, rim pause duration, pivot speed, pivot
  source/destination behavior, Flipper shot collision, Flipper bullet firing, or
  Tanker cargo initialization.

## Follow-up (Task 2)

Create `AI-Logs/Prompts/` and `AI-Logs/Reports/` if missing, then archive this
prompt and the bug-fix report in them.
