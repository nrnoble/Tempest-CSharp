# Stage B2: Isolated Level 2 Flipper Canflip

Implementation date: 2026-10-31
Starting checkpoint: `3895fa4 Add isolated Level 2 square geometry`
Git state: started clean; implementation is uncommitted.

## Scope and authority

Implemented ordinary below-rim canflip only in the existing **DEVELOPMENT ONLY**
`--level2-test` configuration. The approved
[Stage B analysis](./2026-10-30-level2-square-canflip-analysis-report.md),
[B1 report](./2026-10-30-level2-square-b1-implementation-report.md),
[progression analysis](./2026-10-30-progression-architecture-analysis-report.md),
behavior specification, development notes, current C# implementation/tests and
literal Incitti Flipper/setup were the reference inputs.

The supplied request ended at "Do not introduce a second square-pivot".
A clarification requested the remainder; no additional instructions were
supplied. This implementation reuses B1's pivot without adding another one.

Evidence remains explicitly separated:

- **REFERENCE VERIFIED**: Incitti canflip decisions and tuning.
- **PROJECT DECISION**: preserved C# collision, geometry and lifecycle policies.
- **DEVELOPMENT ONLY**: isolated launch, deterministic seams/fixtures and tests.
- **ARCADE VERIFIED**: previously recorded observations and Neal's qualitative
  Level 2 observations in the completed manual validation below. No exact
  constants or implementation details are promoted by test success.

## Configuration

[LevelConfiguration](../../src/Tempest.Game/Levels/LevelConfiguration.cs)
now supplies a narrow immutable `FlipperConfiguration`:

| Setting | Level 1 | Level 2 square test |
|---|---:|---:|
| Canflip | false | true |
| Climb increment | 2 | 2 |
| Turn increment | 8 degrees | 8 degrees |
| Below-rim pause after an ordinary canflip turn | unused | 21 updates |
| Rim pause | 24 updates | 24 updates |

`FlipperCanFlip` remains a read-only projection of that configuration, not an
independent switch. Flipper does not examine displayed level text.

Level 2 retains internal identity 1, display 2, board 1, palette 0, the closed
16-lane square, supply 12, four initial Eggs/eight reserve, and Flipper-only
composition. Egg pacing/sequences are unchanged project policy. Complete
replay and Game Over restart still reinstall the selected isolated square;
life reconstruction retains its runtime/configuration.

Normal no-flag Level 1 and the other development scenarios retain canflip
disabled. No production Level 2, progression, Level 3, other enemy
composition, open tube, palette progression, bonus, extra-life, audio, Tanker
firing promotion/retuning or Blaster/input tuning was added.

## Ordinary canflip path

[Flipper](../../src/Tempest.Game/Gameplay/Flipper.cs) keeps the existing three
states and canonical non-pivot state contract:

1. First below-rim climbing update adds 2, enters Pausing with pause 0,
   attempts firing, then chooses direction from the current Blaster lane.
2. Next pause update adds 1 and immediately starts the adjacent pivot.
3. Turning adds 8 degrees and 2 height. Source/destination/direction stay
   stable for the entire active turn.
4. Completion commits the destination and enters a 21-update below-rim or
   24-update rim pause. Both turn angles reset and source/destination equal
   the committed lane, including lethal completion.
5. Below-rim pauses add 1 and retain the initial direction across subsequent
   turns. Rim pause expiry samples the then-current Blaster lane.

There is no random "whether to flip" roll and no per-update player pursuit
during below-rim turns or pauses. Source same-lane random ties and signed
antipodes are retained. The literal direction algorithm was extracted from
Pulsar into `Level.FindReferenceShortestDirection`; Pulsar calls it at exactly
its former decision points. This avoids duplicated reference logic without
changing Pulsar behavior or the legacy `Level.FindShortestDirection`.

Tanker cargo remains separate from ordinary canflip: prescribed below-rim
direction and the existing 24-update cargo pause are unchanged.

## Random and optional firing

[TempestGame](../../src/Tempest.Game/TempestGame.cs) supplies the established
inclusive random seam when canflip or optional bullets need it. No new Random
was introduced.

- Silent Level 2 creates no ammunition draw or firing rolls. Same-lane
  direction ties still use the existing game random source.
- With `--enemy-bullets-test`, hatch ammunition remains `Rand(1,3)` and
  timer 16. Ammunition is drawn before the initial direction decision.
- Climb attempts precede the initial direction call. At rim pause expiry,
  direction is drawn before a firing roll and the shot uses the chosen
  destination lane.
- Firing attempts occur only during climbing and pause expiry. Turning and
  waiting ticks do not decrement the timer.
- Expiry is strictly below zero: the first firing roll is the seventeenth
  eligible attempt, not the seventeenth simulation update. The firing gate
  remains strictly greater than 75; ammunition is consumed only on success.

The bullet-enabled B1 render fixture was updated to prime eligible attempts
on the actual square Flipper and retain incoming supply, rather than expect
a bullet after 17 straight-climbing simulation ticks. This is deterministic
test setup, not a gameplay retune.

## Literal-source discrepancies and deliberate preservation

The approved report was checked against
[Incitti Flipper](../../Reference/incitti/tempest.bmx#L2646),
[FindShortDir](../../Reference/incitti/tempest.bmx#L3987),
[SetUpEnemies](../../Reference/incitti/tempest.bmx#L4208) and the
[Level 2 data](../../Reference/incitti/levelsets/levelset1.txt).

| Topic | Literal source | B2 disposition |
|---|---|---|
| Height clamp | In type 0/type 1, `height < depth` is checked before adding 2; a height such as 399 can briefly become 401. Pause additions can also overshoot fractional heights. | The analysis describes clamping and B1 already clamps. Preserve immediate clamp at 400 as a **PROJECT DECISION**, not literal exact source arithmetic. |
| Lane during turning | Source assigns `e_Index`/`OnEdge` at pause expiry and draws using residual destination-edge rotation. | Preserve B1's source-anchored hinge and destination commit on completion. Stable source/destination fields describe the active transition. |
| Shot ownership | Source accepts only `OnEdge`, the destination after pivot start, within strict separation 0 to 16. | Preserve both participating C# lanes during the pivot. Correct the old spec's inaccurate reference label; two-lane acceptance is a **PROJECT DECISION**, pending Atari comparison. |
| Player pivot collision | Source can kill with remaining turn angle under 30 degrees at the rim. | Preserve the existing completed-pivot safety rule and shot-before-player collision order. Below-rim pivots are not lethal. |
| Paused rim collision | Source type 3 can kill whenever its lane matches the Blaster at depth. | A below-rim pause newly arriving at the occupied rim signals arrival; merely moving under an already-paused rim Flipper introduces no new B1 collision policy. |
| Artificial rim-start fixture | A source type-0 Flipper constructed directly at depth would enter its initially zero pause. | Preserve B1's 24-update initial rim pause for direct rim-start fixtures; ordinary Level 2 hatches below the rim and uses the new zero-pause canflip path. |
| Ammunition/Egg randomness | Source creates Flipper ammunition and uses its own random Egg scheduling even without a development bullet flag. | Keep bullets separately opt-in and B1's deterministic Egg scheduling. Do not claim full source RNG-stream equivalence. |

The source-derived numeric constants remain **REFERENCE VERIFIED**, not
Atari-measured values. No silent promotion of evidence occurred.

## B1 preservation and documentation

The square hinge calculation, radial endpoint correction, renderer and
legacy geometry helper were not changed. Tests exercise every square pair
in both directions, including wrap, at changing mid-tube depth. Straight
sweeps take 23 turning updates; corners take 12.

Existing accounting, all-rim clearing, Super Zapper selection/delays/flash,
optional bullet lifecycle, death freeze, replay and restart paths are reused.
All-rim remains post-update, non-scoring and subordinate to lethal collision.
Lethal completed pivots remain Pausing with equal source/destination lanes,
so the B1 renderer crash state is not reintroduced.

Updated [README](../../README.md),
[development notes](../../TEMPEST-DEVELOPMENT-NOTES.md), and
[behavior specification](../../TEMPEST-BEHAVIOR-SPEC.md) to describe B2's
development scope, decisions, source differences and manual evidence boundaries.
Program/DebugController comments now describe square/canflip rather than a
geometry-only fixture. Launch behavior is unchanged.

## Automated validation

SDK 9.0.304; xUnit v2 on VSTest. Existing manifests/dependencies were unchanged;
no dependency install/restore was required.

| Validation | Result |
|---|---|
| Pre-edit focused B1/Flipper/cargo/reference-direction baseline | 59 passed |
| Final focused B2/B1/Flipper/cargo/reference-direction tests | 94 passed, 0 failed, 0 skipped |
| Full solution, repeated after projection-race correction | 391 passed, 0 failed, 0 skipped on each of three runs |
| `dotnet build TempestCSharp.slnx --no-restore` | Succeeded; 0 warnings, 0 errors |
| `git diff --check` | Passed |
| HEAD | Still `3895fa4`; no commit made |

Added 35 deterministic B2 cases covering initial zero-pause timing, all
closed-ring direction pairs, both tie outcomes, signed antipodes, movement
without retargeting, all square hinge pairs/timing/projected endpoints,
21/24 pauses, depth clamping including fractional heights, rim resampling,
two-lane strict-band shots, no below-rim death, direct/pause rim arrival,
RNG call ordering, firing boundaries, optional bullet hatch order,
shot-preempted death, canonical lethal projection, freeze/re-entry and
mid-tube use of both Zapper charges.

B1 lifecycle/accounting/rendering tests remain; their fixtures now account
for canflip rather than assume straight ascent. A lethal life-reconstruction
fixture prepares a final rim pivot because a lone canflip survivor can instead
resolve legitimately through all-rim clearing.

### Intermediate failures and corrections

1. A focused rim-tie test detected that the first arrival check could kill on
   any already-paused matching rim lane. Restricting it to a pause that began
   below the rim preserves B1's existing stationary-rim behavior while
   handling the new actual arrival path.
2. After focused success, a full run exposed a geometry assertion race:
   another test advanced global `Projection.K`/`ZOffset` between expected and
   actual projections. Level 2's two test classes now share one narrowly
   nonparallel xUnit collection, preventing their geometry tests from racing
   exit-zoom tests. Projection values, production code and exact assertions
   were not weakened. Three subsequent full runs passed.

## Completed manual acceptance

On 2026-10-31, Neal completed final manual validation of the **DEVELOPMENT ONLY**
isolated Level 2 B2 scenario. No graphical run was performed by the coding
agent during implementation; the observations below are Neal's subsequent
manual results, not inferences from automated tests.

### Observed gameplay

**ARCADE VERIFIED**, scoped to the qualitative Level 2 behavior Neal compared:

- Flippers change lanes while still below the rim; the implemented broad
  behavior resembles Atari Level 2.
- Straight-side and corner mid-tube flips look mechanically sensible.
- No snapping or wrong-lane landing was observed.
- Moving the Blaster during an already-active transition does not visibly
  retarget that transition halfway through.
- Subsequent transitions may respond to the Blaster's then-current position.
  This does not establish an exact sampling point or contradict the recorded
  source finding that below-rim direction is latched and rim expiry resamples.
- Below-rim Flippers do not kill the Blaster merely because the transition
  geometry visually crosses it.
- Rim pursuit remains normal, as does the previously validated completed-
  pivot collision behavior.

### Stability and B1 history

No crash occurred during the B2 manual run. The B1 completed-pivot renderer
crash did not recur. That earlier B1 failure and its canonical-state
correction remain documented in the [B1 report](./2026-10-30-level2-square-b1-implementation-report.md);
successful B2 validation does not erase that history or prove the exact hinge
math is an arcade implementation.

### Evidence limits

The exact 8-degree increment, 21-update below-rim pause, 24-update rim pause,
random/tie behavior, random-call ordering, transition tick boundaries, height
increments, numeric shot band and source decision/state behavior remain
**REFERENCE VERIFIED**, not manually measured arcade details. B1/B2 endpoint
hinge/radial-correction math, C# internal Flipper representation, two-lane shot
acceptance and undistinguished collision/update ordering remain **PROJECT
DECISION**. The prior shot-ownership discrepancy remains unresolved by these
observations.

`--level2-test`, isolated replay, deterministic Egg scheduling and the optional
`--enemy-bullets-test` combination remain **DEVELOPMENT ONLY**. The supplied
manual results do not separately identify a bullet-enabled run or validate
bullet cadence, shot-lane ownership, Zapper lifecycle, accounting or replay
paths beyond their existing evidence. Level 2 is still reachable only through
the development scenario; normal no-flag startup remains Level 1. Natural
Level 1-to-2 progression and authentic Level 2 wave setup have not been
implemented or validated, and Level 3 has not begun.

The 391/391 full-suite result on each of three runs, build with zero warnings/
errors, architectural/source findings and intermediate failures above are
preserved. This manual-acceptance update changes documentation only; gameplay
code and tests were not changed or rerun, and no commit was made.
