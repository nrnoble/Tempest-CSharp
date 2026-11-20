# Arcade Flipper Firing Implementation Report

Date: 2026-11-03

## Implementation

- Replaced ordinary/cargo Flipper finite-ammunition, movement-embedded firing
  with one separate `TempestGame` firing phase for natural production Levels
  1-5. Tanker firing code remains unchanged.
- New and reconstructed Flippers start ready (`shot_delay = 0`). Eligible
  updates decrement the per-Flipper delay before the between-segments gate.
  Active pivots suppress emission but not timer progression; reaching ready
  during a pivot keeps the Flipper ready for its first post-pivot attempt.
- Applied the medium/default per-level holdoffs 80/77/74/71/68 and active-shot
  capacities 2/2/2/3/4. The source probability thresholds are 00/E0/F0/FA/FF
  for 0-4 active shots, with inclusive acceptance.
- Used `Height <= 343` as the explicit integer approximation for ROM
  `enemy_along >= $30`, mapping source endpoints `$F0/$10` to project heights
  0/400. Exact coordinate equivalence is unavailable.
- Reused `EnemyBullet` and its shared collection, travel, collision, rendering,
  player-shot destruction, 25-point score, cleanup, and audio event. Tanker-
  created bullets in that collection contribute to Flipper shot pressure and
  capacity; exact arcade Tanker firing integration is deferred.
- Added deterministic coverage for initial readiness, level parameters,
  exact probability boundaries, capacity and failed-shot behavior, movement
  state/Level 2 lane-change timing, transition emission, cutoff semantics,
  audio, and existing bullet behavior. Updated prior expectations that assumed
  normal Flippers were silent or used a finite ammunition pool.

## Evidence classification

- **ARCADE VERIFIED:** Neal observes enemy fire in every normal level;
  Level 1 Flippers fire while climbing and Levels 2+ continue firing through
  below-rim lane changes. One or two shots per Flipper is observational, not a
  hard cap.
- **REFERENCE VERIFIED:** `Tempest Rom Source code.md` documents the global
  `enm_shoot` phase (`$a2a5-$a303`), gameplay call sequence (`$9717-$9723`),
  initial zero delay (`$9977-$9979`), shot pressure table, holdoffs, and slot
  capacities.
- **PROJECT DECISION:** The separate firing phase and integer height cutoff
  approximation are C# design choices. The former Incitti-derived movement-
  state countdown starved Level 2 lane changes and is retired from production.
- **MANUAL ACCEPTANCE PENDING:** No gameplay or arcade acceptance is claimed.

## Validation

- Focused Flipper firing, EnemyBullet, audio, Level 2 canflip, and Tanker
  regressions: **162 passed**.
- Level 5 production tests: **3 passed**.
- Full solution tests: **663 passed, 0 failed**.
- Full solution build: **0 warnings, 0 errors**.
- `git diff --check`: **passed**.
- HEAD remains `3a1791c`; no files staged or committed. Existing Level 5 and
  other uncommitted work remains preserved. `TEMP/`, `sfx/`, and
  `Tempest Rom Source code.md` were not edited.

## Manual acceptance pending

Neal should launch normal play with:

```powershell
dotnet run --project src/Tempest.Game -- --lives 10
```

Validate visible Flipper firing and movement on Levels 1-5, Level 2 lane
changing and sparse shot feel, normal Tanker firing, bullet audio, and
death/re-entry. This report does not claim that manual validation has occurred.
