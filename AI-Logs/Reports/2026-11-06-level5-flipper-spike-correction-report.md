# Level 5 Flipper / Spike Correction

Date: 2026-11-06
Status: Implementation candidate; Neal's manual acceptance pending.

## 1. Starting Git state

- Branch: `master`, tracking `origin/master`; tracked worktree and index clean.
- HEAD: `aa4eb762b9e207b2014daec4533fbd5a513cffcd` (`Implement accepted Game Over presentation`).
- No unrelated work was present.

## 2. ROM evidence used

The checked-in [arcade ROM transcription](../../Reference/arcade/Tempest%20Rom%20Source%20code.md) was the sole ROM authority:

- Level-table selection assigns Level 5 Flipper program `$53`; the level-indexed movement table and p-code execution are detailed in the preceding [ROM analysis report](./2026-11-06-level-specific-flipper-spike-rom-analysis-report.md).
- Opcode `$1c` (`$9c22-$9c3a`) reads the live `spike_ht` entry for `enemy_seg`; zero is substituted with `$ff`, otherwise the predicate is true when `spike_ht >= enemy_along`.
- Program `$53` (`$a14a-$a157`) moves, tests, and loops without starting a flip when the predicate is false. When true it starts a flip, moves during it, and returns to move-and-test after the flip.
- The depth selector (`$9b07-$9b1d`) uses straight-climb `$07` for near-rim Tanker cargo and the selected level program for deep player-shot cargo. Level 5 therefore uses `$53` only for sufficiently deep player-shot cargo.
- The ROM has no permanent Spike-lock state or random lock/unlock decision. Re-entry selects fresh movement behavior; it does not resume a destroyed Flipper's old program counter.

## 3. Current C# mismatch

Before this change, Level 5 used generic `CanFlip` behavior and `Flipper.Update` did not read Spike state. Level 4 and Level 6 also have Spikes but use different ROM programs; Level 7 uses its separately accepted C# route. Tanker cargo had no Level 5 movement-program selection.

## 4. Implementation seam

Added `BelowRimFlipperProgram.Level5SpikeSensitive` and assigned it only to `Level5Production`. The existing C# Flipper state machine remains in place; no general ROM interpreter or permanent lock state was added. The live gate is checked after climbing, and a Flipper in a Spike lane resumes climbing rather than remaining in a movement pause when the predicate is false.

Deep player-shot Level 5 cargo receives the same policy through the existing Tanker release seam. Natural/near-rim cargo does not. Re-entry creates fresh Flippers with the current Level 5 policy rather than restoring an old phase.

## 5. Coordinate translation

In C#, `Flipper.Height` increases from 0 at the far end toward the 400-unit rim; `Spike.Height` identifies the live tip in that coordinate model. The translation compares the post-climb Flipper height with the current lane's live Spike height: `Flipper.Height >= Spike.Height` permits a lane change. A missing or depleted Spike permits it; shortening the Spike lowers the release threshold. This is a project coordinate mapping, not a literal ROM-byte conversion.

The ROM's cargo split at `$20` is approximated with the existing C# `Level7DeepCargoMaximumHeight` cutoff of 370. This reuses the established project threshold and is not asserted as an exact conversion.

## 6–12. Behavior and preservation

- **Ordinary Flipper:** Continues climbing in the same lane below the current live Spike tip. The existing C# pivot/pause behavior applies once the predicate permits a lane change. The gate itself uses no randomness.
- **Intact Spike:** The Flipper stays in-lane below the tip; equality and above-tip heights permit lane changing.
- **Shortened/destroyed Spike:** The shortened live height releases the gate sooner. Removed, absent, or depleted Spikes do not suppress lane changes.
- **Natural/near-rim Tanker cargo:** Retains its existing straight-climb/release behavior; it does not receive `$53`.
- **Deep player-shot Tanker cargo:** The production release path applies Level 5's Spike-sensitive policy at or below the 370-unit C# cutoff. Cargo sibling identities and opposite directions remain unchanged.
- **Death/re-entry:** Fresh ordinary Level 5 Flippers and reconstructed cargo use the current Level 5 policy; no prior movement-program instruction pointer is restored.
- **Levels 4/6/7:** The new policy is not enabled for these levels. Level 7's accepted `2-3-2-2-2` route remains unchanged and is covered by its movement/cargo tests.

## 13–15. Files changed

Production:

- `src/Tempest.Game/Gameplay/Flipper.cs`
- `src/Tempest.Game/Gameplay/Tanker.cs`
- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/TempestGame.cs`

Tests:

- `tests/Tempest.Game.Tests/Level5ProductionTests.cs`
- `tests/Tempest.Game.Tests/Level7ProductionTests.cs` (records the Level 5 policy exception while retaining the Level 4/6 defaults)

Durable documentation:

- `TEMPEST-BEHAVIOR-SPEC.md`
- `TEMPEST-DEVELOPMENT-NOTES.md` (records the recommended future-level preimplementation workflow)

Paired prompt: `AI-Logs/Prompts/2026-11-06-level5-flipper-spike-correction-prompt.md`.

## 16–18. Automated validation

- Focused Level 5 Flipper/Spike, Tanker, and re-entry tests: **11 passed, 0 failed, 0 skipped**.
- Neighboring Level 4–7, Level 7 route/cargo, lifecycle, Spike, progression, and campaign soak selection: **458 passed, 0 failed, 0 skipped**.
- `dotnet test TempestCSharp.slnx`: **817 passed, 0 failed, 0 skipped**.
- `dotnet build TempestCSharp.slnx`: **0 warnings, 0 errors**.
- `git diff --check`: passed.

## 19. Manual acceptance checklist

**PENDING:** Neal should run `dotnet run --project src\Tempest.Game -- --lives 10` and compare against arcade/MAME:

1. Observe an ordinary Flipper entering an intact Spike lane.
2. Confirm it continues climbing rather than freezing.
3. Confirm it stays in-lane below the live Spike tip.
4. Observe lane changing resume around the tip threshold.
5. Confirm a shortened Spike releases it sooner.
6. Confirm destroying the Spike removes the lane-stay constraint.
7. Check ordinary non-Spike lane changing.
8. Check Tanker cargo behavior.
9. Check Levels 4, 6, and 7 for regressions.
10. Judge overall Level 5 gameplay feel against the arcade.

## 20. Final Git state and scope

HEAD remains `aa4eb762b9e207b2014daec4533fbd5a513cffcd`; branch remains `master...origin/master`. The intended tracked changes are the eight production/test/documentation files listed above. AI logs are ignored by the repository. No files were staged; no commit or push was made. No Level 8/9 behavior, unrelated gameplay, or interactive game launch was performed.
