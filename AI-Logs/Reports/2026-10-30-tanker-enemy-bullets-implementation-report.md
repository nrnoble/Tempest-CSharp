# Tanker Enemy-Bullet Implementation Report — 2026-10-30

## 1. Summary

Tankers in the controlled development scenario now fire the existing EnemyBullet
when, and only when, `--tanker-test` is combined with `--enemy-bullets-test`.
No new command-line flag was added and no new projectile class was created. The
firing rules are taken directly from Incitti's `Tanker` type in
`Reference/incitti/tempest.bmx`. `--tanker-test` alone remains the silent,
cargo-focused scenario; `--enemy-bullets-test` alone preserves the existing
Flipper bullet behavior unchanged.

## 2. Incitti source behavior implemented

From `Type Tanker` in `tempest.bmx`:

- `Tanker.Create`: `hasbullets = Rand(1,2)` and `bullettime = 16 + Rand(64)`.
  BlitzMax `Rand(64)` is inclusive 1..64, so the initial timer is 16 plus an
  inclusive 1..64 offset drawn through the project's existing
  `NextRandomInclusive` seam.
- `Tanker.Update`: climbs first (`height :+ tankerclimbspeed`), then evaluates
  firing only while `height < 380` (strict), decrementing `bullettime`; when the
  timer goes below zero it resets to 16 and `Rand(0,100) > 75` decides the shot.
  `hasbullets` is decremented only when a bullet is actually created, so a
  failed roll keeps the shot for a later window.
- `bullets.Create(e_index, height)`: the bullet starts on the Tanker's lane at
  its post-climb height.
- The 380 firing ceiling sits below the `depth - 15` (385) release threshold,
  so a Tanker never fires on the tick that triggers natural cargo release, and
  the pending/released states never evaluate firing at all.

## 3. Tanker ammunition and timer design

`Tanker` gained two optional constructor parameters (`bulletAmmunition`,
defaulting to 0, and `bulletTimer`, defaulting to the 16-tick reset) plus
read-only `BulletAmmunition` / `BulletTimer` properties, mirroring the Flipper
implementation. The firing logic lives in a private `TryFireBullet` that
matches `Flipper.TryFireBullet` exactly (countdown gated on ammunition,
reset to 16 on expiry, strict `> 75` decision, ammunition consumed only on a
created bullet). An unarmed Tanker (zero ammunition or no random source) never
decrements its timer and never draws a decision.

The update seam is a new overload:

```
TankerUpdateResult UpdateForDevelopment(Func<int,int,int>? randomInclusive, out EnemyBullet? bullet)
```

The parameterless `UpdateForDevelopment()` forwards to it, keeping every
existing caller and test source-compatible. The Tanker never touches
TempestGame collections, score, Raylib, flags, accounting, or collision lists;
TempestGame decides arming and registers any returned bullet.

## 4. Flag/mode behavior

- `--tanker-test` alone: the Tanker hatches with 0 ammunition and a default
  timer; `UpdateTankers` passes a null random source, so Tankers and Flippers
  both stay silent. Existing cargo scenario unchanged.
- `--enemy-bullets-test` alone: no Tankers exist; Flipper behavior unchanged.
- Both flags: `CreateTankerWaveEnemy` arms the hatched Tanker with
  `NextRandomInclusive(1, 2)` ammunition and `16 + NextRandomInclusive(1, 64)`
  initial timer; ordinary and released-cargo Flippers keep their existing
  firing behavior. No new flag exists.

## 5. Update and collision ordering

Global simulation ordering is unchanged. Tanker firing happens inside the
existing `UpdateTankers` phase. Because `UpdateEnemyBullets` runs earlier in
`RunOneStep`, a bullet created during the Tanker phase does not receive an
extra movement update on its creation tick (same policy as Flipper bullets);
this is documented at the registration site. The bullet is appended to
`developmentCollisionOrder` at its creation point, preserving Incitti's
insertion-order shot collision. A bullet is an independent projectile after
creation: destroying or naturally releasing the Tanker never removes it.

## 6. Life-reset behavior

`ResetTankerScenarioAfterDeath` already discards all runtime enemies and
bullets; no changes were needed there. Because arming happens at hatch time in
`CreateTankerWaveEnemy`, a Tanker rebuilt from the fresh slot-0 Egg after a
life loss draws fresh source-backed ammunition and timer values; unused
ammunition and timer state from the discarded instance are never preserved.
A resolved slot 0 does not return merely because a bullet remained or was
cleared (bullets are not part of the ledger).

## 7. Files changed

- `src/Tempest.Game/Gameplay/Tanker.cs` — ammunition/timer state, firing
  ceiling constant, update/firing seam, maintainability comments.
- `src/Tempest.Game/TempestGame.cs` — source-backed arming at hatch, bullet
  registration in `UpdateTankers`, ordering comments.
- `tests/Tempest.Game.Tests/SimulationTests.cs` — new focused tests, two test
  doubles/helpers, and one updated test (below).

No other tracked files were modified. `Reference/incitti` is untouched.

## 8. Tests added

One existing test was intentionally rewritten:
`TankerScenarioEnemyBulletOptionAppliesToFlippersOnly` pinned the previous
"Tanker stays silent with both flags" behavior, which this task deliberately
changes; it is now `CombinedTankerAndBulletFlagsArmBothTankerAndFlippers`.

New unit tests (direct Tanker):
- `TankerFiringTimerDecrementsResetsAndUsesStrictDecisionBoundary` — timer
  countdown, reset to exactly 16, 75-fails/76-fires boundary, failed decision
  costs nothing, success consumes exactly one unit, exhausted ammunition never
  fires again.
- `UnarmedTankerNeverRunsTimerOrDrawsDecisions` — zero ammunition touches no
  firing state and never draws.
- `TankerFiringRequiresPostClimbHeightStrictlyBelow380` (379/380/381 cases) —
  climb precedes evaluation; bullet origin lane and post-climb height; exactly
  380 and above are ineligible and skip the countdown.
- `TankerCannotFireOncePendingOrAfterCargoRelease` — pending and released
  states never fire or draw.

New integration tests (TempestGame):
- `TankerTestAloneKeepsTankerUnarmedAndSilent`
- `ArmedTankerUsesControlledInclusiveAmmunitionAndTimerOffset` (1 and 2)
- `TankerDecisionBoundaryMatchesFlipperConventionInGame`
- `TankerFiresFromLaneAtPostClimbHeightThroughGameUpdate` — also verifies the
  ledger total is untouched by bullet creation.
- `TankerBulletSurvivesTankerShotDestructionAndIsWorthTwentyFive` — bullet
  survives the Tanker's destruction, is shot for 25 points, resolves no slot
  or cargo unit.
- `TankerBulletSurvivesNaturalCargoRelease`
- `LifeLossClearsTankerBulletsAndRearmsFreshTankerWithNewValues`
- `CombinedModeCompletesWithBulletsAndClearsResidualsAtExit` — full-ledger run
  in combined mode with every firing decision succeeding; bullets never block
  completion and residual bullets clear through the established exit path.

Helpers added: `PrepareTankerBulletGame` and the `TankerBulletModeRandom`
deterministic double (dispatches on each draw site's distinct exclusive upper
bound).

## 9. Baseline and final build/test totals

- Baseline (commit `7e25aea`, clean tree): build 0 errors; 146 total,
  146 passed, 0 failed.
- Final: build 0 errors; **161 total, 161 passed, 0 failed**
  (146 baseline-equivalent + 15 new test cases).
- Focused `--filter FullyQualifiedName~Tanker` run: 41 passed, 0 failed.

## 10. git diff --check result

Clean (exit 0). Git prints benign `LF will be replaced by CRLF` normalization
warnings for the two edited source files; there are no whitespace errors.

## 11. Manual validation checklist

Run:

```
dotnet run --project src/Tempest.Game -- --tanker-test --enemy-bullets-test --lives 10
```

Check:

- Tankers emit occasional small white rotating bullets;
- bullets originate in the Tanker's lane and at its apparent depth;
- cadence appears delayed and occasional rather than immediate/constant;
- no more than the source-backed ammunition (1–2 per Tanker) is emitted;
- bullets travel toward the player;
- player collision deducts exactly one life through the normal death lifecycle;
- bullets can be shot for 25 points;
- shooting the Tanker does not erase an already-fired bullet;
- natural cargo release does not erase an already-fired bullet;
- Tanker score (200) and cargo behavior remain correct;
- standard `--tanker-test` remains silent without `--enemy-bullets-test`;
- behavior requires comparison with the original arcade version.

## 12. Explicitly deferred features

Unchanged and out of scope: Spiker retreat bullets; normal-Level-1 automatic
Flipper bullet activation; Fuseballs; Pulsars; Super Zapper; Tanker stillborn
cargo (`ZapIt`); score popups; `rimit` hurry-up; mid-tube Flipper progression;
level progression; new level composition; audio/bonuses; renderer changes; the
suspected converging-rim-Flipper issue.

## 13. Remaining arcade-comparison uncertainties

- All constants (16-tick reset, 1..64 initial offset, 380 ceiling, >75
  decision, 1–2 ammunition) are Incitti-derived and unvalidated against the
  arcade.
- Whether the arcade Tanker fires at all (and at what cadence) needs direct
  arcade comparison; Incitti's Tanker firing may itself be an approximation.
- A literal Tanker-fired bullet surviving until wave completion is unreachable
  under source timing (a Tanker resolves by ~tick 203 of its climb while a
  bullet expires ~120 ticks after firing, long before the ledger can drain),
  so the residual-bullet completion test exercises the shared enemy-bullet
  list/exit path in the combined mode; the clearing code is identical for
  Tanker- and Flipper-fired bullets.

## 14. Todo reconciliation

All task todos were completed: baseline verification; required reading
(project docs, analysis report, current sources, Incitti reference); Tanker
firing implementation; TempestGame integration; deterministic tests; final
validation (focused tests, build, full suite, `git diff --check`/`--stat`,
status, Reference/incitti unchanged); prompt and report archived. Nothing was
deferred except the explicitly out-of-scope items in section 12. Nothing was
committed or pushed.
