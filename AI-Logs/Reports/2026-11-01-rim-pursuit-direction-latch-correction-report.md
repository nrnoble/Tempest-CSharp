# Rim pursuit direction latch correction

## Starting checkpoint and baseline

- Starting checkpoint: `09a558e Implement natural Level 3 to Level 4 progression`.
- Verified `HEAD` was exactly `09a558e`; no tracked changes existed before work.
- `TEMP/` was already untracked and was left untouched.

## Arcade observation and Incitti findings

**ARCADE VERIFIED:** Once a rim pursuer establishes a direction, it does not
reverse simply because the Blaster crosses behind and the opposite route becomes
shorter. Rightward stays rightward and leftward stays leftward, even when that
becomes the long route. This verifies persistence, not the hidden selection
point.

**REFERENCE VERIFIED** from `Reference/incitti/tempest.bmx`:

- **Flipper** (`2646-2942`): has one `dir` field. `FindShortDir` is assigned
  during each climbing update (`2768`) and again at each rim-pause expiry
  (`2806`), so the source can change direction before every next rim step.
  At open-board endpoints it reverses before stepping beyond the endpoint
  (`2808+`). There is no separate pursuit latch.
- **Tanker cargo Flippers** (`2489+`): Tanker release creates fresh Flippers
  and assigns each an alternating `dir`; children use the same Flipper update.
  That route direction is retained below the rim, but the Flipper source
  resamples at rim-pause expiry.
- **Pulsar** (`2943-3284`): has one `dir` field and calls `FindShortDir` on
  climbing and descending updates (`3080`, `3112`). Lane-pause transfer uses
  that direction; open-board endpoints reverse it (`3153+`). No separate
  persistent rim-chase field exists.
- **Fuseball** (`3285+`): moves laterally with vertical excursions and can
  probabilistically change lateral direction; this is not rim pursuit.
- **Tanker** (`2407+`): climbs and releases cargo; it does not chase on the rim.
- **Spiker**: builds and retreats on its lane; it does not chase on the rim.
- New ordinary enemies are created through fresh entity creation/hatching.
  Tanker release also creates new child Flippers. Incitti has no distinct
  persistent rim-direction field to carry through death/reconstruction.

Neal's arcade observation takes precedence over Incitti where their direction
behavior differs. Open-board endpoint behavior is recorded as reference only;
no open-tube gameplay was added. Neal's final manual acceptance covers the
currently implemented closed-rim Levels 1-4 only; it is not a claim that the
same pursuit rule applies at every later difficulty level. Neal recalls Level
8 as the first open normal-progression board and V-shaped. This is recollection,
not implementation evidence; open-rim and Level 8 support remain deferred.

## C# root cause and correction

Flipper `BeginPivot` selected `FindReferenceShortestDirection` or
`FindShortestDirection` whenever a rim pause expired. Since every completed rim
pivot starts another pause, the Blaster's current lane could replace the prior
direction on each cycle. Pulsar recalculated its shortest direction on each
climbing/descending update before the next lane transfer. Neither type had
separate rim-pursuit state.

C# now keeps a nullable `RimPursuitDirection` separate from the existing
movement `Direction`. Flippers choose it when selecting their first rim
destination and reuse it for later rim destinations. Tanker cargo retains its
outward below-rim route until rim pursuit begins; the two directions are not
conflated. Development-only Pulsars latch when a rim lane destination is first
selected and clear the latch when they choose to descend. Existing pulse timing,
hazard behavior, directed transitions, and endpoint reversal are preserved.

The latch is per entity. Destruction/all-rim removal discards the entity;
ordinary player-death reconstruction uses fresh Eggs and fresh hatchlings;
fresh cargo creation, new levels, and full restarts also construct new
entities. No stale latch is carried into a new lifecycle. Direct rim-arrival
lethality, shot-before-player-collision ordering, and existing identity/ledger
ownership are unchanged.

Affected: normal Flippers, Tanker cargo Flippers, and development-only Pulsars.
Unaffected: Fuseballs, Tankers themselves, and Spikers, which do not use
rim-pursuit behavior. Pulsars remain disabled in natural Levels 1-4.

## Automated coverage

- Right- and left-committed Flippers move through another rim pivot after the
  Blaster moves behind and the opposite direction is strictly shorter.
- A crossover stress test switches the Blaster's side eight times per cycle
  across five rim turns, for both committed directions, and verifies actual
  destinations rather than only direction state.
- Pulsar test crosses the Blaster behind the Pulsar and verifies its next lane.
- Pulsar reset test verifies the latch is cleared when it chooses to descend.
- Tanker-cargo test verifies the below-rim route remains independent and rim
  pursuit establishes its own direction.
- Level 3 plus-board test retains the 270-degree directed transition and then
  verifies the committed route after crossover.
- Death/reconstruction test confirms the old latched Flipper is removed and a
  fresh hatched Flipper has no stale latch.
- Existing Level 2 geometry/canflip, directed-geometry, collision, shooting,
  all-rim, ledger, progression, Level 4, and death tests remain in the focused
  and full regression runs.

## Validation

Focused run passed: 505 tests, 0 failed, 0 skipped. The filter covered
`RimPursuitTests`, `SimulationTests`, `Level2CanFlipTests`,
`Level2SquareTests`, `Level3GeometryTests`, `Level3MixedWaveTests`,
`Level3WaveLedgerTests`, `Level4CandidateTests`,
`PlayerDeathPresentationTests`, `LevelProgressionTests`,
`Level3TestModeTests`, and `DirectedRotationInvestigationTests`.

- `dotnet test TempestCSharp.slnx --no-restore`: **PASS**, 587 passed,
  0 failed, 0 skipped.
- `dotnet build TempestCSharp.slnx --no-restore`: **PASS**, 0 warnings,
  0 errors.
- `git diff --check`: **PASS**.
- Natural Level 1 -> 2 -> 3 -> 4 campaign regressions: **PASS** in the full
  automated suite.
- `Reference/incitti` unchanged; `TEMP/` untouched.

## Scope and protected behavior

No Level 5, new geometry, open-tube implementation, production Pulsar
activation, wave-composition change, accounting change, window/fullscreen work,
vector-font work, attract mode, score rollover, or unrelated gameplay redesign
was added. Existing directed inward pivots, including the 270-degree route,
remain in use. No commit or push was made.

## Final manual acceptance — PASS

**ARCADE VERIFIED, closed-rim Levels 1-4:** Neal manually accepted the current
behavior. Once a rim Flipper establishes its pursuit direction, it does not
reverse merely because the Blaster moves behind it and the opposite route
becomes shorter. The verification is limited to currently implemented Levels
1-4 and does not generalize the latch to later levels.

Future open/unconnected tubes have physical endpoints where pursuit can
legitimately reverse. Neal recalls Level 8 as V-shaped and the first open
normal-progression board; this is arcade recollection only. No Level 8 or
open-tube behavior was implemented.
