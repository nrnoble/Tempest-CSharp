# Player-Shot / Flipper Contact Ordering Implementation

## 1. Baseline and starting worktree

- Branch: `master`.
- `HEAD`: `3bebf42cbe9514846ab32bf4c8a12bcfdb048da7` (`Implement accepted production Level 7`).
- Tracked worktree and index were clean; `git diff --stat` and `git diff --cached --stat` were empty.
- The only untracked file was the expected prior-milestone `tests/Tempest.Game.Tests/Level1StationaryRimCombatComparisonTests.cs`. It was preserved and extended with production regressions. No `comment for AI.md` was present.
- No stage, commit, push, reset, restore, clean, or discard operation occurred.

## 2. Source boundary and arcade observation

The prior investigation reports contain the required trace to the sole permitted authority, `Reference/arcade/Tempest Rom Source code.md`; no additional source lookup was needed. No Internet search, external Tempest disassembly, MAME, port, or other source was used.

**ARCADE OBSERVED:** Neal reports that indefinite stationary defense on enclosed tubes is not sustainable; held fire and careful tapping can temporarily defend, but multiple pursuing Flippers and unavailable shots can force movement or cause death. Endpoint camping on open tubes may be more defensible because approach is one-sided. This Level 1 experiment does not establish or change open-tube behavior.

## 3. Demonstrated defect and production change

Before the change, `RunOneStep` moved player shots and resolved their Flipper hits before updating Flippers and consuming completed-pivot/direct-arrival contact. Consequently, an already-active shot could remove a Flipper on the update it completed a lethal pivot.

After the change:

1. A successfully created shot is explicitly checked against Flippers at its birth coordinates during firing.
2. All surviving shots keep their normal movement. Flipper hits are skipped in the early moving-shot collision pass; other target and enemy-bullet paths retain their existing handling and precedence.
3. Flippers update, then `CheckFlippersAgainstPlayer` consumes contact.
4. If contact starts player death, `KillPlayer` clears shots and the update returns; no late hit can undo death or resolve another Flipper.
5. Otherwise, surviving moving shots are checked against Flippers after their update/contact phase.

The distinction is made by explicit call placement, not list ordering or incidental collection enumeration. New shots can receive the birth check and, if they survive, the ordinary later moving-shot check. A birth hit removes the shot and resolves its target once before Flipper movement.

The existing strict `Flipper.IsHitBy` depth tolerance was not changed. At ordinary Level 1 player depth, a shot is born at the same depth as a rim Flipper, so the strict positive-separation predicate does not ordinarily match at birth. The production hook is preserved; its focused positive test uses a synthetic overlapping Flipper height to exercise the birth event without changing gameplay tolerances.

## 4. Same-tick death, scoring, and accounting

The final-pivot race now ends in `PlayerDying`, leaves the contacting Flipper unresolved until ordinary death reconstruction, awards no shot points, and clears the in-flight shot. Death/re-entry tests confirm unresolved enemies are rebuilt as Eggs and accounting is conserved. A separate multiple-Flipper case confirms that a lethal contact prevents a late shot from scoring or removing another enemy.

The former player-shot-over-Super-Zapper-expiry priority is retained: a matching moving shot may resolve first, while that Flipper still skips movement on its zap-expiry update. Zap charges, selection, timing, and scoring are unchanged. Existing enemy-bullet collision priority is also retained.

## 5. Changed files

Production:

- `src/Tempest.Game/TempestGame.cs` — explicit birth hit; deferred moving-shot/Flipper resolution; shared Flipper accounting; preservation of matching-shot/zap-expiry priority.
- `src/Tempest.Game/Gameplay/Flipper.cs` — lifecycle comment updated to describe contact consumption before deferred moving-shot hits.

Tests:

- `tests/Tempest.Game.Tests/Level1StationaryRimCombatComparisonTests.cs` — retained analysis/model cases, updated pressure ordering, and added production tests for birth hit/miss, final-pivot death, post-update nonlethal and below-rim hits, multiple Flippers, held-fire cadence, and ten-shot capacity.
- `tests/Tempest.Game.Tests/SimulationTests.cs` — updated Level 1 and direct-rim/final-pivot expectations for contact-first outcomes.
- `tests/Tempest.Game.Tests/Level2CanFlipTests.cs` — updated the same-tick contact regression for Level 2.

The comparison tests are marked `AnalysisModel`; production-path tests are marked `ProductionRegression`.

## 6. Automated validation

- Focused ordering, birth, pivot, direct-arrival, bullet-priority, and zap tests: **26 passed, 0 failed, 0 skipped**.
- Shared Levels 1-7 seam regression selection (firing, simulation, Level 2, Levels 3-7 production/progression, Tanker/cargo, death, Super Zapper, completion, and comparison tests): **612 passed, 0 failed, 0 skipped**.
- Full `dotnet test TempestCSharp.slnx`: **762 passed, 0 failed, 0 skipped**.
- `dotnet build TempestCSharp.slnx`: succeeded, **0 warnings, 0 errors**.
- `git diff --check`: passed with no whitespace errors.

## 7. Deterministic stationary-pressure comparison

The test-only pressure scheduler was updated to model birth checks, moving shots, Flipper movement/contact, then deferred moving-shot hits. These are deterministic fixture results, not a full-wave or arcade-RNG simulation.

| Scenario | Before: death tick / kills / remaining | After: death tick / kills / remaining | Other relevant result |
|---|---|---|---|
| 1 pursuer, held fire | no death / 1 / 0 | no death / 0 / 0 | 600 ticks; 91 accepted; 182 cooldown- and 327 capacity-unavailable attempts |
| 2 pursuers, held fire | no death / 2 / 0 | no death / 0 / 0 | 600 ticks; 92 accepted; 184 cooldown- and 324 capacity-unavailable attempts |
| 4 pursuers, held fire | 265 / 2 / 2 | 265 / 0 / 2 | 42 accepted; 84 cooldown- and 139 capacity-unavailable attempts |
| 4 pursuers, controlled single-tap | 265 / 2 / 2 | 265 / 2 / 2 | 7 accepted; 0 unavailable attempts |
| 8-slot ablation, 4 pursuers held | 265 / 2 / 2 | 265 / 0 / 2 | 34 accepted; 163 capacity-unavailable attempts |

The pressure fixtures' contact-event counts were 1, 4, 16, and 16 before, and 1, 4, 17, and 17 after, respectively, for the four primary scenarios. The 8-slot ablation had 17 contact events after; its earlier report did not retain a separate contact-event count, so no baseline value is inferred. The final-pivot rescue counter remained zero in the primary pressure fixtures. The isolated production race test changed as intended: the previous ordering allowed an existing shot to remove the final-pivot Flipper; now contact commits death before that shot can be resolved. One- and two-pursuer fixtures still clear without death, so the results do not prove this correction alone makes stationary defense unsustainable.

## 8. Preserved mechanics and remaining uncertainty

Held-level Fire, three-tick cooldown, ten-shot capacity, shot speed/lifetime, lane/depth origin, hit tolerance, rim timing, pivot-lane rules, below-rim movement, Level 7's accepted route, difficulty, enemy supply, and Super Zapper settings were not retuned. No anti-camping behavior was added.

The correction is automated-test validated but awaits Neal's manual gameplay comparison. Exact arcade timing and whole-wave survival remain unknown; no conclusion about open-tube balance is made.

## 9. Manual acceptance and final state

Launch command for Neal:

```powershell
dotnet run --project src\Tempest.Game -- --lives 10
```

Focus first on enclosed Level 1: normal launch/play; held fire and careful single taps against one and multiple pursuers; temporary defense versus indefinite safety; moments without an available shot; final-pivot contact; ordinary moving/spinner fairness; expected shot lanes/visual states; and, if desired, Levels 2-7. Temporary lane defense and well-timed shots should remain possible; the target is sustained pressure, not forced death or an anti-camping rule.

The final Git state remains on `master` at the accepted baseline, with the four listed tracked files modified and the expected comparison test still untracked; no other untracked file was found. AI prompt/report records are under ignored `AI-Logs/`. No interactive game launch, durable authority-document edit, reference edit, commit, push, or Internet disassembly search occurred. Stop for Neal's manual comparison.
