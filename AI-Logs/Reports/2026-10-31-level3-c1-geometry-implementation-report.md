# Level 3 C1 Geometry Implementation

Implementation date: 2026-10-31
Starting checkpoint: `f6ac233 Implement Level 1 to Level 2 progression`
Scope: isolated Level 3 plus-board configuration and geometry groundwork.
Git: no commit or push created.

## 1. Baseline Git/build/test result

Before editing:

```text
git status --short
(empty)

git log -1 --oneline
f6ac233 Implement Level 1 to Level 2 progression

git diff --check
(no output; exit 0)

git status --short -- Reference/incitti
(empty)
```

Baseline validation:

| Command | Result |
|---|---|
| `dotnet test TempestCSharp.slnx --no-restore` | 409 passed, 0 failed, 0 skipped |
| `dotnet build TempestCSharp.slnx --no-restore` | Succeeded, 0 warnings, 0 errors |

The approved Level 3 analysis design and the inspected `f6ac233` source agree
on the geometry defect, endpoint-hinge approach, and C1 boundary. No source
contradiction required a design change.

## 2. Purpose and start checkpoint

Implemented C1 from the approved
[`2026-10-31-level3-tanker-progression-analysis-report.md`](./2026-10-31-level3-tanker-progression-analysis-report.md):
install one source-derived Level 3 plus-board configuration, generalize the
validated Level 2 endpoint hinge to that board, fix the Tanker-cargo geometry
bypass, and add an isolated geometry-only launch.

The exact supplied implementation prompt is saved at
[`2026-10-31-level3-c1-geometry-implementation-prompt.md`](../Prompts/2026-10-31-level3-c1-geometry-implementation-prompt.md).
Its copied-file SHA-256 matches the read-only attachment.

## 3. Evidence boundary

- **REFERENCE VERIFIED:** ordered plus-board points and board identity from
  Incitti `Reference/incitti/levelsets/levelset1.txt`, plus the Level 3
  source-derived setup values in the approved analysis.
- **PROJECT DECISION:** the diagnostic uses the existing deterministic C#
  Egg scheduling and configured-supply accounting; these are not claimed to
  reproduce Incitti's release schedule.
- **DEVELOPMENT ONLY:** the `--level3-geometry-test` Flipper-only setup, direct
  renderer tests, and controlled headless Tanker-cargo geometry fixtures.
- **ARCADE VERIFIED:** no new arcade behavior is claimed. Manual plus-board
  validation remains pending.

`TEMPEST-BEHAVIOR-SPEC.md` was not changed. Source-derived values and passing
automated tests do not establish arcade geometry or composition.

## 4. Level 3 configuration

Added exactly one shared `LevelConfiguration.Level3Plus` object. It records:

| Property | Value |
|---|---|
| Internal/displayed identity | 2 / 3 |
| Board / palette | 2 / 0 |
| Continuity and topology | Closed; 16 vertices / 16 playable lanes |
| Center Y / Y offset | 400 / -80 |
| Depth / normal position | 400 / 50 |
| Player start | Lane 7, fractional position 0.5 |
| Configured supply | 15 |
| Initial Eggs / reserve | 4 / 11 |
| Flipper tuning | Canflip enabled; climb 2, turn 8, lane pause 21, rim pause 24 |
| Pivot geometry | `EndpointHinge` |

The exact ordered points are asserted in `Level3GeometryTests`; the point list
is exposed read-only. The deterministic spawn sequence exists only to drive
the current C# diagnostic and is not the reference Level 3 Egg schedule.

## 5. Diagnostic launch policy

Added `--level3-geometry-test` as **DEVELOPMENT ONLY**. It selects the same
`Level3Plus` object in the game constructor and uses the existing configured
supply with ordinary Flippers. There is no `Level3Geometry`/`Level3Production`
configuration pair and no new production configuration copy.

The flag is mutually exclusive with `--level2-test`, `--spiker-test`,
`--tanker-test`, `--fuseball-test`, and `--pulsar-test`. `--lives` and the
existing orthogonal `--enemy-bullets-test` compose. The title, HUD,
completion text, and replay prompt identify a geometry diagnostic. R replay
and Game Over restart keep the diagnostic configuration; life reconstruction
retains its runtime/configuration.

Normal no-flag play remains Level 1 -> Level 2 -> terminal. The
Level 1-to-Level 2 production resolver was not extended.

## 6. Endpoint-hinge generalization

Replaced the square-only `SquareEndpointHinge` mode and helper names with the
narrow `EndpointHinge` mode and shared endpoint-pivot helpers. The existing
square still selects this same path; Level 1 still selects
`LegacyLaneTurn`, and `CalculateEdgeTurnAngle` remains unchanged.

For each adjacent transition, the geometry helper identifies the shared
vertex, keeps that hinge fixed, and rotates the free endpoint toward the
destination endpoint. Straight runs have a deterministic signed 180-degree
sweep; corner transitions have a 90-degree sweep. The ordered wrap pair
15 <-> 0 is covered. Radius correction accounts for rounded source points
without midpoint interpolation. Non-finite or zero endpoint rays fail
explicitly rather than producing a zero-turn pivot.

Tests enumerate all 32 directed adjacent transitions and check expected angle
and sign, finite intermediate endpoints, nonzero geometry, exact initial and
destination endpoint ordering, and the shared hinge.

## 7. Tanker-cargo geometry correction

`Flipper.CreateTankerCargo` now initializes its mandatory pivot through
`Level.CalculateFlipperTurnAngle`, matching ordinary Flippers and the
installed configuration's geometry policy. This removes the zero-angle
straight-run bypass on the plus board.

Headless tests cover a 180-degree straight cargo pivot, 90-degree corner,
15/0 wrap, Level 2's selected endpoint-hinge policy, and unchanged Level 1
legacy angle selection. They also check distinct non-carrier lanes, the
deterministic outward route, assigned below-rim direction, and that direction
remains fixed during below-rim travel.

No Tanker ledger, identity, score, delayed activation, release route, firing,
or Super Zapper behavior was changed. Cargo is automated-only in this
checkpoint; it is not graphically exposed in the geometry diagnostic.

## 8. Renderer and lifecycle behavior

The existing renderer selects the generic endpoint-hinge calculation for
either endpoint-hinge configuration; no second renderer or global geometry
mutation was added. Headless checks cover 16 rails, 16 near-rim segments,
16 far-rim segments, player projection on every lane, Egg/shot/Flipper/Tanker/
cargo projections, straight and corner pivot captures, destination completion,
and unchanged geometry during Super Zapper flash recoloring.

The diagnostic uses the existing Super Zapper lifecycle and configured-supply
accounting. Tests verify fresh replay recharge, Game Over restart, same-Level
life reconstruction, and all-rim accounting's active-entity completion gate.
No production cargo obligation was added.

## 9. Level 1/2 preservation

- Level 1 keeps the legacy angle helper and legacy renderer transform.
- Level 2 keeps its exact square points, endpoint-hinge behavior, canflip,
  collision rules, and canonical completed-pivot state.
- Normal progression remains only Level 1 -> Level 2; Level 2 is still
  terminal.
- Existing `--level2-test`, Tanker, Spiker, Fuseball, Pulsar, and bullet
  scenarios are unchanged.

The existing Level 1 and Level 2 tests remain in the focused and full runs.

## 10. Files changed

Tracked source and test changes:

- `src/Tempest.Game/Debug/DebugController.cs`
- `src/Tempest.Game/Gameplay/Flipper.cs`
- `src/Tempest.Game/Levels/Level.cs`
- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/Levels/LevelLoader.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `src/Tempest.Game/TempestGame.cs`
- `tests/Tempest.Game.Tests/Level2CanFlipTests.cs`
- `tests/Tempest.Game.Tests/Level2SquareTests.cs`
- `tests/Tempest.Game.Tests/Level3GeometryTests.cs` (new)
- `README.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`

Requested AI-log deliverables:

- `AI-Logs/Prompts/2026-10-31-level3-c1-geometry-implementation-prompt.md`
- `AI-Logs/Reports/2026-10-31-level3-c1-geometry-implementation-report.md`

## 11. Tests added/updated

`Level3GeometryTests` adds nine focused cases for configuration, flag policy,
replay/restart/life reconstruction, all directed hinges, ordinary canflip,
rim wrap, Tanker cargo, rendering/flash, and configured-supply/all-rim
accounting. Existing square and canflip tests now use the generalized
endpoint-hinge API and mode name; their behavior assertions remain intact.

## 12. Focused/full/build/diff results

| Validation | Result |
|---|---|
| Focused geometry regression group (`Level3GeometryTests`, `Level2SquareTests`, `Level2CanFlipTests`) | 61 passed, 0 failed, 0 skipped |
| Final `dotnet test TempestCSharp.slnx --no-restore` | 418 passed, 0 failed, 0 skipped |
| Final `dotnet build TempestCSharp.slnx --no-restore` | Succeeded, 0 warnings, 0 errors |
| Final `git diff --check` | Passed, exit 0 |
| Final `git status --short` | 12 tracked files modified; one new test file untracked |
| Final `git diff --stat` | 12 tracked files, 193 insertions, 60 deletions; untracked test is not included in Git's diff stat |
| `Reference/incitti` | Status empty; `git diff --exit-code` passed |
| `TEMPEST-BEHAVIOR-SPEC.md` | `git diff --exit-code` passed; unchanged |

The final total is nine more than the 409-test baseline, matching the nine new
Level 3 test cases.

## 13. Intermediate failures

- The first focused build used a nonexistent named constructor argument
  (`source` instead of `edge`); corrected.
- The next focused run exposed two test setup mistakes: the all-rim fixture
  had not returned its initial Eggs to reserve, and the corner-pivot loop ran
  one update short. Both tests were corrected.
- The final focused geometry regression group and final full suite passed.
- No baseline build or test failure occurred. The initial partial patch
  application was inspected and only its successfully applied HUD hunk was
  retained.

## 14. Manual checklist

Run:

```powershell
dotnet run --project src/Tempest.Game -- --level3-geometry-test --lives 10
```

Neal's manual validation is still required. Observe:

1. Plus-board outline and proportions; near/far alignment and 16-lane
   presentation.
2. Blaster rim travel around concave and convex turns.
3. Egg and shot alignment.
4. Ordinary Flipper climb, mid-tube straight-run flips, and corner flips.
5. Rim pivots, including the 15/0 wrap.
6. No snapping, wrong-lane landing, center crossing, or unexpectedly strange
   pivot arc.
7. No below-rim collision regression; Super Zapper flash and all-rim exit.
8. Death/re-entry and R replay remain on the geometry diagnostic.
9. No crash or non-finite rendering.

Tanker cargo geometry is automated-only in C1. Do not treat this run as
validation of production composition, multiple Tankers, Tanker firing, or
Level 2 -> Level 3 progression.

## 15. Explicit C1 limitations

No natural Level 2 -> Level 3 progression, production Level 3 mixed wave,
production Tanker accounting, per-Egg Flipper/Tanker selection, multiple
production Tankers, Tanker firing changes, automatic Tanker bullets, Flipper
bullet promotion, Level 4, open tubes, Spikers, Fuseballs, Pulsars, palette
cycles, board rotation, bonuses, bonus lives, audio, approach presentation,
or Blaster input tuning were implemented.

## 16. C2/C3/C4 relationship

- **C2** remains the separate Tanker firing validation/policy checkpoint.
- **C3** remains the production Level 3 mixed-wave identity, composition,
  multi-carrier Super Zapper, death reconstruction, and all-rim checkpoint.
- **C4** remains natural Level 2 -> Level 3 progression, gated on C3 and
  manual validation.

C1 creates no production-wave claims or prerequisites beyond the approved
geometry groundwork.

## 17. Historical geometry/arcade uncertainties before inward-pivot acceptance

The plus-board data is source-derived, not independently arcade-verified.
At this point in the investigation, the exact arcade hinge arcs, projection
appearance, and plus-board Flipper behavior remained unvalidated. Neal's
subsequent manual PASS is documented below and supersedes that status for the
specific visible observations recorded there. The deterministic C# Egg
schedule and controlled Flipper-only diagnostic do not claim Incitti Level 3
composition or release behavior. Tanker-cargo plus-board rendering and
movement remain unvalidated arcade behavior.

## 18. Historical C1.5 Git/maintenance snapshot (before inward correction)

The final worktree contains the 12 tracked files and one untracked test file
listed in §10. The requested prompt and report exist under ignored
`AI-Logs/`; they do not appear in `git status --short`. `git diff --check`
passed. Git printed informational LF-to-CRLF conversion warnings for several
edited text files; these did not affect the exit status or build/test results.
`Reference/incitti` remains unchanged and `TEMPEST-BEHAVIOR-SPEC.md` remains
untouched. No commit or push was created.

## 19. Manual directed-rotation finding and C1 acceptance status

**Historical status at the time of this investigation:** C1 was not manually
accepted. Neal's directed-rotation observation was a gameplay-significant
rejection of the geometry/timing behavior, not a cosmetic rendering note.
Subsequent C1.5 directed-sweep and inward-pivot corrections and Neal's
2026-10-31 manual PASS are recorded in the later sections below; that PASS
supersedes this historical status.

### Source and C# root cause

Incitti `UpdateAngles` stores the angle at each shared tube vertex via
`GetAngle` in `Reference/incitti/transformfunctions.bmx`. `GetAngle` preserves
reflex angles (including 270 degrees); it does not always fold to the
shortest 0-180 degree angle. Flipper and Pulsar select the relevant vertex
angle after their directed lane step. Their drawing/update branches also use
`dir` to select the rotation sign. A transition therefore has both a
direction-selected signed sweep and an angle-derived duration.

C1's endpoint-hinge helper instead uses `atan2(cross,dot)`, which always
returns a shortest signed ray difference in [-180, 180]. `CalculateFlipperTurnAngle`
takes its absolute value. Consequently a reflex 270-degree source sweep becomes
90 degrees, and the renderer follows the short signed ray instead of the
source-selected sweep. The final endpoint is forced to its exact destination,
so existing endpoint-only assertions can pass despite the different
intermediate route. The configured 8-degree Flipper update step then completes
a 90-degree pivot in 12 updates rather than the 34 updates required for 270
degrees.

This is not only a missing reflex magnitude. For 90- and 180-degree routes,
the source selects rotation sign from `dir` (`dir = -1` uses the positive
source angle; `dir = +1` uses the negative source angle), whereas C1 derives
the sign from the endpoint rays and uses a fixed tie-break for 180 degrees.
The Level 2 square table shows those signed-route differences even where the
magnitudes agree.

### Directed transition tables

Each cell is `destination / shared hinge / Incitti signed sweep / C1 signed
endpoint sweep`. C# `Direction +1` selects the previous lane and `Direction
-1` selects the next lane. Incitti's signed sweep column applies its
direction-controlled rendering branch to the source `GetAngle` magnitude.
These tables enumerate every adjacent directed transition, including wrap.

**Level 2 square**

| Source | Previous (`+1`) | Next (`-1`) |
|---:|---|---|
| 0 | 15 / P0 / -90 / +90 | 1 / P1 / +180 / -180 |
| 1 | 0 / P1 / -180 / +180 | 2 / P2 / +180 / -180 |
| 2 | 1 / P2 / -180 / +180 | 3 / P3 / +180 / -180 |
| 3 | 2 / P3 / -180 / +180 | 4 / P4 / +90 / -90 |
| 4 | 3 / P4 / -90 / +90 | 5 / P5 / +180 / -180 |
| 5 | 4 / P5 / -180 / +180 | 6 / P6 / +180 / -180 |
| 6 | 5 / P6 / -180 / +180 | 7 / P7 / +180 / -180 |
| 7 | 6 / P7 / -180 / +180 | 8 / P8 / +90 / -90 |
| 8 | 7 / P8 / -90 / +90 | 9 / P9 / +180 / -180 |
| 9 | 8 / P9 / -180 / +180 | 10 / P10 / +180 / -180 |
| 10 | 9 / P10 / -180 / +180 | 11 / P11 / +180 / -180 |
| 11 | 10 / P11 / -180 / +180 | 12 / P12 / +90 / -90 |
| 12 | 11 / P12 / -90 / +90 | 13 / P13 / +180 / -180 |
| 13 | 12 / P13 / -180 / +180 | 14 / P14 / +180 / -180 |
| 14 | 13 / P14 / -180 / +180 | 15 / P15 / +180 / -180 |
| 15 | 14 / P15 / -180 / +180 | 0 / P0 / +90 / -90 |

Level 2 source magnitudes are 90 degrees at P0/P4/P8/P12 and 180 degrees
elsewhere. C1 matches those magnitudes, but its signed endpoint route differs
from Incitti on corner transitions and on the negative 180-degree direction.

**Level 3 plus**

| Source | Previous (`+1`) | Next (`-1`) |
|---:|---|---|
| 0 | 15 / P0 / -270 / -90 | 1 / P1 / +90 / -90 |
| 1 | 0 / P1 / -90 / +90 | 2 / P2 / +180 / -180 |
| 2 | 1 / P2 / -180 / +180 | 3 / P3 / +90 / -90 |
| 3 | 2 / P3 / -90 / +90 | 4 / P4 / +270 / +90 |
| 4 | 3 / P4 / -270 / -90 | 5 / P5 / +90 / -90 |
| 5 | 4 / P5 / -90 / +90 | 6 / P6 / +180 / -180 |
| 6 | 5 / P6 / -180 / +180 | 7 / P7 / +90 / -90 |
| 7 | 6 / P7 / -90 / +90 | 8 / P8 / +270 / +90 |
| 8 | 7 / P8 / -270 / -90 | 9 / P9 / +90 / -90 |
| 9 | 8 / P9 / -90 / +90 | 10 / P10 / +180 / -180 |
| 10 | 9 / P10 / -180 / +180 | 11 / P11 / +90 / -90 |
| 11 | 10 / P11 / -90 / +90 | 12 / P12 / +270 / +90 |
| 12 | 11 / P12 / -270 / -90 | 13 / P13 / +90 / -90 |
| 13 | 12 / P13 / -90 / +90 | 14 / P14 / +180 / -180 |
| 14 | 13 / P14 / -180 / +180 | 15 / P15 / +90 / -90 |
| 15 | 14 / P15 / -90 / +90 | 0 / P0 / +270 / +90 |

The four reflex hinges are P0/P4/P8/P12 (270 degrees); the outer corners
P1/P3/P5/P7/P9/P11/P13/P15 are 90 degrees; P2/P6/P10/P14 are straight
180-degree hinges. Both directed transitions at each reflex hinge currently
report a 90-degree magnitude instead of 270 degrees (eight affected
transitions).

### Entity-by-entity analysis

- **Blaster:** C# updates `Player.EdgePosition` by a fixed input fraction per
  update and renders linearly along the current lane. Incitti likewise updates
  `scl` by a fixed per-update spinner amount. Neither implementation computes
  the turn angle or uses a 90/180/270-degree transition timer. No Blaster
  geometry/timing fix is justified by this finding; changing its lane-based
  input speed would be a separate behavior question.
- **Ordinary Flipper:** `BeginPivot` selects `DestinationEdge` from
  `Direction`, but C1 computes only the shortest endpoint-ray sweep.
  `TurnAngle` advances by the configured 8 degrees per update and completion
  is angle-based, so the wrong sweep also shortens the gameplay window for
  shooting or moving past the Flipper.
- **Pulsar:** Incitti uses the same direction-selected `edges[].angle`.
  C# `Pulsar.GetTurnAngle` uses `CalculateEdgeTurnAngle` (a folded lane-heading
  difference), not the directed shared-hinge sweep. For the rounded Level 1
  circle, the current lane-heading difference is about 23.777 degrees while
  the source hinge angle is about 156.223 degrees. Pulsar advances by 12
  degrees per update, so this also changes the transition duration. Its
  post-lane-step angle selection must be checked along with the Flipper fix.
- **Tanker cargo:** C1 correctly removed the zero-angle bypass by using
  `CalculateFlipperTurnAngle`, but cargo inherits the same shortest-angle
  error. Its stored destination and direction remain intact; geometry/timing
  must be corrected without changing outward routing, identity, release
  accounting, or activation.

### Deterministic investigation tests and boundary

Added `DirectedRotationInvestigationTests.cs` as characterization tests, not
as acceptance tests. The five tests enumerate all square/plus transitions,
reproduce the eight plus-board 90-vs-270 mismatches, exercise a custom
225-degree concave hinge (C1 returns 135 degrees), reproduce Flipper's
12-versus-34-update timing, check the Tanker-cargo route, and distinguish the
rounded Level 1 circle's 23.777-degree heading change from its 156.223-degree
source hinge angle. The test run passed 5/5; passing means the discrepancy is
reproducible, not corrected.

**Scope decision: track this as a separate C1.5 directed-rotation checkpoint.**
It is a prerequisite to manual C1 acceptance because the correction crosses
angle calculation, Flipper/Pulsar update timing, renderer interpolation, and
Tanker cargo. Do not fold a guessed correction into the already manually
rejected C1 geometry work. C1.5 should add tests for both directions at 90,
180, 270, and non-right-angle hinges; then implement and validate a shared
directed sweep consistently across all affected entities and rendering,
preserving Player lane-based movement and Tanker accounting/routing. After
automated regression, Neal must manually recheck Level 2 and plus-board
corner/straight pivots, 90/180/270 timing, opportunities to shoot/move during
each transition, and Tanker cargo before accepting C1.

No production source, `Reference/incitti`, or `TEMPEST-BEHAVIOR-SPEC.md`
changes were made during this investigation. No commit or push was created.

## 10. C1.5 directed-rotation implementation

C1.5 implements the prerequisite identified above without enabling production
Level 3, changing the behavior specification, or changing the Tanker release
route/accounting policy. The investigation tables above remain a historical
record of the C1 defect; the final column describes the old C1 result.

### Shared directed transition

Added `DirectedLaneTransition`, carrying source lane, destination lane,
selected direction, hinge vertex, source/destination free vertices, the
destination endpoint ordering, and the signed sweep. `Level` derives the
angle with the Incitti `GetAngle` algorithm rather than `atan2`'s shortest-ray
result. Direction `+1` keeps the negative sweep and direction `-1` the
positive sweep; the absolute full sweep magnitude determines completion time.
The old `CalculateEdgeTurnAngle` remains available as a legacy helper, while
the Level 1, Level 2, and Level 3 configurations use the directed endpoint
geometry.

Ordinary Flippers and Tanker cargo store the active transition and use its
magnitude for their 8-degree update step. Pulsars retain the lane they leave
before the pause expires, then store the same transition for their 12-degree
step and rendering. Flipper and Pulsar rendering now interpolate those same
directed endpoint rays, including reflex sweeps and exact endpoints. Existing
shot eligibility, below-rim safety, completion collision ordering, cargo
identity, outward release route, delayed activation, and accounting remain
intact. The longer cargo pivot can reach the rim before completion; collision
is still evaluated only at completed pivot.

Blaster input signs and the existing normalized mouse/keyboard sensitivities
remain unchanged. The normalized movement is interpreted as angular distance
(`movement * 180 degrees`) and advances across each lane using its directed
hinge angle. Exact lane-boundary ownership is unchanged. For example, a 9
degree keyboard movement takes 10, 20, and 30 updates to traverse 90, 180,
and 270 degrees, respectively; Level 1 uses its source-derived non-right
vertex angles.

### Acceptance coverage and validation

The former C1 characterization tests were converted to acceptance coverage.
Tests now verify all 32 directed transitions on the Level 2 square and Level 3
plus boards, signed 90/180/270-degree routes, complementary arbitrary
101.31/258.69-degree routes, intermediate endpoint interpolation, final
endpoint ordering, Flipper/Pulsar/cargo durations, Blaster traversal in both
directions, Level 1's 156.223-degree source hinge, and Pulsar rendering during
an active transition. Related Level 2, Level 3, collision, shooting, cargo
ledger, and Player-input regressions were updated to assert angle-dependent
timing while preserving their existing policy checks.

Focused validation before the final full-suite run:

| Scope | Result |
|---|---|
| Directed-rotation acceptance tests | 12 passed |
| Level 2 geometry/canflip, Level 3 geometry, Simulation, Super Zapper, and directed-rotation suites | 404 passed |

Final solution test/build results and `git diff --check` are recorded below.
`TEMPEST-BEHAVIOR-SPEC.md` and `Reference/incitti` remain unchanged. No commit
or push was created.

| Final check | Result |
|---|---|
| `dotnet test TempestCSharp.slnx --no-restore` | 431 passed, 0 failed, 0 skipped |
| `dotnet build TempestCSharp.slnx --no-restore` | Succeeded, 0 warnings, 0 errors |
| `git diff --check` | Passed |
| `git status --short -- Reference/incitti TEMPEST-BEHAVIOR-SPEC.md` | Empty; neither protected path changed |
| Commit / push | None |

### Manual retest required

Automated geometry and source agreement are not arcade acceptance. Manually
run the Level 2, Level 3 geometry diagnostic, Pulsar, and Tanker development
scenarios and check: both directions at 90/180/270 degrees; Flipper
8-degree and Pulsar 12-degree cadence; the full 270-degree route rather than
the opposite 90-degree shortcut; Blaster movement signs and traversal time;
shot opportunities during long pivots; and Tanker cargo's outward route,
below-rim behavior, rim arrival, and completion collision. Example launches:

```powershell
dotnet run --project src\Tempest.Game -- --level2-test --lives 10
dotnet run --project src\Tempest.Game -- --level3-geometry-test --lives 10
dotnet run --project src\Tempest.Game -- --pulsar-test --lives 10
dotnet run --project src\Tempest.Game -- --tanker-test --lives 10
```

This was the pre-acceptance instruction. Neal's later PASS is recorded below;
it permits accepting C1/C1.5 but does not authorize C2 or production Level 3.

## 11. Manual inward-pivot rejection and correction

### New arcade observation and evidence boundary

Neal's direct arcade observation is **ARCADE VERIFIED** for the behavior
requirement: Flippers and enemies swing inward toward the tube/web, not around
the exterior side of the rim. It is independent of lane selection and the
full directed sweep: a transition can reach the correct lane with the correct
270-degree duration and still be wrong if its intermediate arc is mirrored
outward. The long directed sweep continues to determine duration and the
shoot/move-under opportunity. The Blaster was observed to have correct
direction; its earlier issue was geometry-dependent duration, not pivot side.

The supporting local recording at
`.\Tempest\Videos\Tempest-Bad Rotation.mp4` was accessible in the repository
as `Videos\Tempest-Bad Rotation.mp4`. It was inspected only as supporting
manual evidence, was not edited or added to Git, and is not required by the
implementation. Neal's written arcade observation remains the authority.
The new observation does not authorize applying the enemy transform to the
Blaster.

### Three independent transition properties

The correction explicitly distinguishes:

1. **Destination/lane direction:** source lane, selected adjacent lane,
   pursuit/routing semantics, and destination remain unchanged.
2. **Directed sweep/duration:** C1.5 `SignedSweepDegrees` and its magnitude
   continue to represent the selected full route, including 90, 180, 270, and
   arbitrary angles. They remain the source of entity timing.
3. **Spatial pivot side:** `SpatialSweepDegrees` selects the local polygon
   interior side for the visible enemy arc. On closed boards this is derived
   from polygon winding and the local convex/reflex turn, not from an assumed
   clockwise sign or screen-Y intuition.

The C1.5 operation that caused the mirror was using the same signed value for
both properties: the directed sweep's sign was fed directly to the renderer's
world-coordinate rotation. In the renderer's screen-oriented coordinates,
that does not establish which side of a board edge is the web interior.
The selected lane, shared hinge, source/destination free endpoints, endpoint
ordering, and full duration were not the cause; no source/destination order
reversal was found. `Projection.Project` adds board Y to screen Y, so positive
mathematical rotation appears clockwise on-screen; clockwise/counterclockwise
alone is therefore not a reliable inward selector. The radius interpolation
also had no far-boundary limit: on a narrow concave section, an otherwise
inward ray could pass through the opposite rim.
The correction keeps the directed sweep for duration, derives a separate
inward spatial sweep, and limits the intermediate free-endpoint radius to the
first intersection with the closed board polygon. Exact source/destination
endpoints are still returned at progress 0/1.

### Geometric and entity findings

For a closed polygon, its signed area supplies the winding sign. At each
shared hinge, the signed turn from incoming to outgoing boundary rays yields
the local interior angle (`180 - winding * signedTurn`), including convex,
straight, and reflex vertices. The spatial sweep uses the selected lane
direction and winding to traverse that interior side. Intermediate radius is
limited along the selected ray to the first board boundary, preventing a
narrow concavity from letting the endpoint cross the far rim. This uses the
configured point order as-is; it does not reverse or mutate board geometry.
For open tubes there is no enclosed web side, so their pre-existing directed
spatial convention is retained.

| Entity/path | Finding and result |
|---|---|
| Level 1 rim Flippers | The defect was present: Level 1 uses the same generalized endpoint-hinge renderer, so this was not limited to the C1 plus-board path. The endpoint hinge and selected source/destination endpoints were correct; only the intermediate spatial rotation side was wrong. The inward-side and boundary limit now apply without changing Level 1's board coordinates or angle-dependent timing. |
| Level 2 below-rim canflip and rim pursuit | Both use the shared Flipper transition/render path. The same geometry correction applies to both; source/destination, pursuit choice, canflip policy, and progress timing are preserved. |
| Level 3 below-rim canflip and rim pursuit | The same shared path now handles convex corners, reflex corners, straight runs and wrap. All 32 directed transitions are checked at intermediate progress, with the full 270-degree reflex route retained. |
| Pulsar | Pulsars use the shared endpoint renderer and therefore shared the outward spatial transform. They now receive the inward spatial sweep while retaining selected direction, full-sweep duration, pause cadence, pulse behavior and gameplay progress. `EdgeIndex` still changes when the lane pause expires; `ActiveTransition` retains both endpoints during the visual sweep. Existing lane-based collision policy was not changed. |
| Tanker-cargo Flippers | Cargo uses the Flipper transition and shared renderer, so it inherited the same pivot-side defect. Its deterministic outward **lane route** remains unchanged; only the spatial pivot is inward. Identity, fixed below-rim direction, activation, scoring, accounting and reconstruction are unchanged. |
| Blaster | It does not use the enemy endpoint-hinge transform. Its direction and C1.5 angular-distance traversal remain unchanged; no concrete regression was found. |

The renderer does not independently reconstruct a shortest or mirrored arc:
Flipper and Pulsar endpoint rendering consumes the active transition's
progress through `Level.CalculateFlipperPivotEndpoints`. Flipper collision
continues to use the established active-transition lane policy (source and
destination remain shootable); unfinished visual overlap is not made lethal.
Completed-pivot collision remains authoritative. Pulsar keeps its existing
logical lane collision timing. There is no separate outward endpoint path
behind the inward Flipper animation.

### Tests, failures, and validation status

The deterministic directed-rotation suite now checks intermediate endpoint
containment for Level 1, all directed Level 2 square transitions, all 32
directed Level 3 transitions, a non-right-angle convex fixture, and a
reversed-winding copy of the plus board. It also retains arbitrary
101.31/258.69-degree sweep assertions, exact endpoint/direction checks,
angle-dependent Flipper/Pulsar timing, cargo direction, and Blaster-duration
checks. Existing shootability, move-under, completed-pivot collision, Level 2,
Pulsar, cargo accounting and Blaster regressions remain active.

Iteration failures and their disposition:

- The first intermediate-geometry acceptance test failed on Level 1 lane
  `0 -> 15`; the free endpoint was outside the board polygon. This reproduced
  the written outward-mirror defect before the first production correction.
- A non-right-angle convex fixture exposed the old source-derived angle
  calculation returning the reflex complement (about 252 degrees rather than
  the polygon's approximately 108-degree interior angle). Closed-board angle
  derivation now uses winding and the local signed turn.
- The first closed-board-only guard regressed an existing open-tube Pulsar
  test. The guard was removed; open tubes retain their prior spatial behavior.
- Extending polygon-containment coverage to the narrow arbitrary concave test
  fixture found a second issue: an inward ray could cross a distant board
  boundary. The first-boundary radius limit was added; the complete directed
  geometry suite now passes for this fixture as well.
- One initial test command used a project path relative to the parent
  directory and failed to locate the project; rerunning from the repository
  root succeeded. A concurrently running `Tempest.Game.exe` locked normal
  build outputs, so focused/final validation uses a session-local
  `BaseOutputPath`; the process was not stopped.

Focused validation after the endpoint boundary correction:

| Scope | Result |
|---|---|
| Directed-rotation acceptance suite | 15 passed, 0 failed |
| Directed rotation, Level 3 geometry, Level 2 square/canflip, and Simulation suites | 376 passed, 0 failed |

Final solution test/build and Git maintenance results are recorded below.

### Manual retest checklist (completed; acceptance recorded below)

This checklist was prepared before arcade acceptance. Neal has since completed
the Level 1, Level 2, Level 3, Pulsar and Tanker observations; the outcomes
and limits of that evidence are recorded in the final acceptance section.

1. **Normal Level 1**
   `dotnet run --project src/Tempest.Game -- --lives 10`
   - Flippers pivot inward, with sensible direction/duration and no outward
     mirrored arc; remain shootable; permit moving under an unfinished pivot;
     completed-pivot collision remains correct; no crash.
2. **Level 2 square**
   `dotnet run --project src/Tempest.Game -- --level2-test --lives 10`
   - Below-rim and rim Flippers pivot inward; corners and straight 180-degree
     transitions take the appropriate relative time; no wrong lane, outward
     arc or short-path reversal; Blaster behavior remains correct.
3. **Level 3 plus board**
   `dotnet run --project src/Tempest.Game -- --level3-geometry-test --lives 10`
   - Convex 90-degree and reflex 270-degree pivots swing inward; straight
     180-degree and 15/0 wrap remain correct; destination and 90/180/270
     timing are right; shoot/move-under opportunity, Blaster direction and
     duration, all-rim, death/re-entry, replay and no-crash behavior hold.
4. **Pulsar**
   `dotnet run --project src/Tempest.Game -- --pulsar-test --lives 10`
   - Check inward/outward side, direction and angle-dependent duration, while
     pulse visuals, pulse hazard and unrelated behavior remain unchanged.
5. **Tanker**
   `dotnet run --project src/Tempest.Game -- --tanker-test --lives 10`
   - Ordinary Tanker destruction leaves two cargo Flippers on distinct
     non-Tanker lanes; the established outward lane route remains while
     visible pivots swing inward; below-rim direction stays fixed and rim
     behavior remains correct.

The checklist's requests to perform a retest are historical and are superseded
by the PASS recorded below. Neal did not measure exact ticks or milliseconds.
No C2 work is authorized by manual acceptance.

### Final validation and maintenance

| Check | Result |
|---|---|
| Focused directed-rotation suite | 15 passed, 0 failed |
| Focused directed rotation, Level 3 geometry, Level 2 square/canflip, and Simulation suites | 376 passed, 0 failed |
| `dotnet test TempestCSharp.slnx --no-restore` | 434 passed, 0 failed, 0 skipped; isolated `BaseOutputPath` used because the running game locked normal outputs |
| `dotnet build TempestCSharp.slnx --no-restore` | Succeeded, 0 warnings, 0 errors; same isolated output path |
| `git diff --check` | Passed; Git emitted only existing LF-to-CRLF working-copy warnings |
| `git status --short -- Reference/incitti` | Empty; reference files unchanged |
| `git status --short -- TEMPEST-BEHAVIOR-SPEC.md` | Modified intentionally to record Neal's observed behaviors with evidence labels |
| Video status | `Videos\Tempest-Bad Rotation.mp4` remains unchanged and untracked |
| Commit / push / HEAD | None; HEAD remains `f6ac233 Implement Level 1 to Level 2 progression` |

The first full run after the spatial correction passed 433 tests. A further
endpoint-continuity assertion was then added for the narrow concave case, and
the final full run passed 434 tests. The count increases from the preserved
C1.5 result of 431 because the directed-rotation suite now includes three
additional inward-geometry/continuity cases. Existing historical baseline
entries (409 tests, the 418-test validation, five characterization tests,
C1 rejection, C1.5's 431-test result, and earlier failures) are retained above.
The report and exact acceptance prompt are under ignored `AI-Logs/`; neither
appears in `git status --short`.

Final worktree status:

- Modified tracked: `README.md`, `TEMPEST-BEHAVIOR-SPEC.md`,
  `TEMPEST-DEVELOPMENT-NOTES.md`,
  `src/Tempest.Game/Debug/DebugController.cs`,
  `src/Tempest.Game/Gameplay/Flipper.cs`,
  `src/Tempest.Game/Gameplay/Player.cs`,
  `src/Tempest.Game/Gameplay/Pulsar.cs`,
  `src/Tempest.Game/Gameplay/Tanker.cs`,
  `src/Tempest.Game/Levels/Level.cs`,
  `src/Tempest.Game/Levels/LevelConfiguration.cs`,
  `src/Tempest.Game/Levels/LevelLoader.cs`, `src/Tempest.Game/Program.cs`,
  `src/Tempest.Game/Rendering/VectorRenderer.cs`,
  `src/Tempest.Game/TempestGame.cs`,
  `tests/Tempest.Game.Tests/Level2CanFlipTests.cs`,
  `tests/Tempest.Game.Tests/Level2SquareTests.cs`,
  `tests/Tempest.Game.Tests/SimulationTests.cs`, and
  `tests/Tempest.Game.Tests/SuperZapperTests.cs`.
- Untracked: `Videos/Tempest-Bad Rotation.mp4`,
  `src/Tempest.Game/Levels/DirectedLaneTransition.cs`,
  `tests/Tempest.Game.Tests/DirectedRotationInvestigationTests.cs`, and
  `tests/Tempest.Game.Tests/Level3GeometryTests.cs`.

The final tracked diff reports 18 modified files, 735 insertions and 228
deletions. `git status --short -- Reference/incitti` is empty;
`git status --short -- TEMPEST-BEHAVIOR-SPEC.md` shows its intentional
modification. The video is the only file under `Videos/` and remains
untracked; the report and saved prompt are ignored by `.gitignore`.

### C1/C1.5 manual acceptance — PASS (2026-10-31)

Neal manually passed C1/C1.5:

- Level 1 and Level 2 Flippers swing inward through the tube/web.
- Level 3 plus-board 90-degree convex and full 270-degree reflex pivots swing
  inward; the reflex route remains the long route, not a 90-degree shortcut.
  180-degree transitions look sensible and Flippers land on the correct lane.
- Blaster direction and movement feel correct. The longer 270-degree sweep
  preserves the opportunity to shoot or move underneath. No outward arc,
  snapping, strange tube crossing or crash was observed.
- Pulsars move inward with sensible direction and geometry-dependent duration;
  pulse visuals and hazard remain normal, with no unrelated regression/crash.
- Tankers release two cargo Flippers on distinct non-Tanker lanes. Their
  established outward lane route remains correct while visible pivots swing
  inward; below-rim direction, rim arrival and subsequent behavior remain
  correct, with no new collision problem or crash.

The verified behavior is limited to these visible observations. Keep
destination/lane direction, full directed sweep/duration, and spatial pivot
side separate: the 270-degree long sweep and its opportunity window remain
while the enemy arc swings inward. Neal did not establish exact timing,
complete Atari cargo routing, internal C# geometry, or production Level 3
composition. The behavior specification now records observations with the
existing evidence labels.

Final validation is complete: the focused suites passed (15 directed-rotation
tests; 376 across directed rotation, Level 3 geometry, Level 2 square/canflip
and Simulation), all 434 solution tests passed, the solution build succeeded
with zero warnings/errors, and `git diff --check` passed. `Reference/incitti`
is unchanged. `TEMPEST-BEHAVIOR-SPEC.md` changed only to record the observed
behaviors. The exact prompt and this report are saved under ignored
`AI-Logs/`; the prompt is
`AI-Logs/Prompts/2026-10-31-c1-c1.5-manual-validation-acceptance-prompt.md`.
`Videos/Tempest-Bad Rotation.mp4` remains unchanged and untracked.

C1/C1.5 is ready to commit together; exclude the video from any commit. No
other commit blocker was identified. No commit or push was created.
