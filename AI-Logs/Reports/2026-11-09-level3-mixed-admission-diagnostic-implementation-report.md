# Level 3 mixed-admission diagnostic implementation

Date: 2026-11-09

Disposition: **IMPLEMENTATION CANDIDATE COMPLETE; MANUAL GAMEPLAY ACCEPTANCE PENDING.**

## Starting state

- Branch: `master`.
- Expected and actual starting HEAD: `7acac60035383bfb6e0cb170c0adfa0dd88b6c4b`.
- The index and tracked worktree were clean before implementation.
- Existing untracked `Reference/arcade/roms.rev2/` was left untouched.
- The investigation handoff was [the Level 3 concurrency ROM report](./2026-11-09-level3-concurrency-rom-investigation-report.md); the substantive task is preserved in [the matching prompt record](../Prompts/2026-11-09-level3-mixed-admission-diagnostic-implementation-prompt.md).

## Implementation

- Added a Level 3 admission snapshot and policy in `src/Tempest.Game/Levels/Level3MixedAdmissionPolicy.cs`. It applies the seven-place cap, a one-Tanker maximum, minimum-driven Tanker preference, Flipper availability, and unchecked 8-bit latent-cargo subtraction followed by the physical-place clamp.
- Wired the policy into the production enemy factory only when the configuration is the canonical `LevelConfiguration.Level3Mixed` instance. Direct Level 3 test startup uses that configuration too. Other ledger waves and copied configurations retain their prior selection behavior.
- The snapshot counts ordinary and Tanker-released Flippers, shot-released cargo awaiting activation, active Tankers, latent Tanker cargo, and any other active enemy types. Runtime Flipper/Tanker collections naturally retain rim, transition, and pending-zap enemies in occupancy. Eggs, reserves, terminal ledger entries, bullets, and destruction effects are not counted.
- Admission is checked before cargo activation, original commitment, or enemy construction. A denied Egg remains ready and keeps its retained identity/type; the pure policy does not consume random state. The factory takes a new snapshot for every hatch in the existing same-tick loop.
- Cargo reentry bypasses the species picker and waits only for a physical place. Tanker release still materializes the complete accepted identity pair. **Project decision / diagnostic approximation:** pair release can temporarily exceed seven live objects; the ordinary hatch gate blocks further entry until capacity reopens. This does not claim strict global seven-object fidelity.
- Levels 1 and 2, the scheduler, Tanker lifecycle, scoring, death/reconstruction, Super Zapper handling, and completion logic were not redesigned.

## Files changed

- `src/Tempest.Game/Levels/Level3MixedAdmissionPolicy.cs` (new)
- `src/Tempest.Game/Levels/Level3WaveSelection.cs`
- `src/Tempest.Game/TempestGame.cs`
- `tests/Tempest.Game.Tests/Level3MixedAdmissionPolicyTests.cs` (new)
- `tests/Tempest.Game.Tests/Level3MixedWaveTests.cs`
- `tests/Tempest.Game.Tests/Level3TestModeTests.cs`
- `tests/Tempest.Game.Tests/PlayerDeathPresentationTests.cs`
- `AI-Logs/Prompts/2026-11-09-level3-mixed-admission-diagnostic-implementation-prompt.md` (ignored)
- This report (ignored)

The existing multi-Tanker lifecycle stress tests now use explicitly copied, noncanonical fixture configurations; they are not production admission expectations.

## Validation

- Focused Level 3 admission/wave tests: **PASS**, 53 tests before the final focused additions; the final admission-arithmetic and same-tick waiting subset also **PASS**, 13 tests.
- Targeted Level 3 ledger/startup/lifecycle, Super Zapper, campaign, progression, recording, and Level 1/2 regression tests: **PASS**, 239 tests.
- Full solution tests (`dotnet test TempestCSharp.slnx --blame-hang-timeout 5min`): **PASS**, 1,139 passed, 0 failed.
- Full solution build (`dotnet build TempestCSharp.slnx --no-restore`): **PASS**, 0 warnings, 0 errors.
- `git diff --check`: **PASS**.
- The interactive game was not launched. No commit or push was made.

## Remaining questions and manual validation

This is a candidate diagnostic, not accepted behavior. The local ROM investigation establishes the inspected Rev. 2 mechanism, but actual maximum Level 3 occupancy, the frequency of wrapping cases, and the visible effect of this translation remain unmeasured. Complete-pair C# cargo release intentionally differs from strict ROM slot allocation at saturation.

Launch Level 3 directly with ten lives:

```powershell
dotnet run --project src\Tempest.Game -- --level3-test --lives 10
```

Neal should compare whether:

- Level 3 feels less overcrowded and simultaneous Tankers are absent;
- Tanker cargo release still looks normal and both cargo Flippers remain;
- Egg waiting and entry cadence look natural;
- Flipper population still varies instead of appearing hard-locked to four;
- death/reentry, Super Zapper, rim behavior, and completion remain normal.

**Manual gameplay acceptance from Neal is still pending.** Do not promote this candidate into the behavior specification or development notes until that comparison is complete.
