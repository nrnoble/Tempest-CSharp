# Between-level Approach Starfield Presentation Implementation

## FINAL MANUAL ACCEPTANCE / FINALIZATION (2026-11-03)

**ACCEPTED.** Neal manually evaluated the current combined presentation and
reports it looks very good and should be retained. No further visual tuning
of this transition is requested. This section supersedes all trial/pending
manual-validation language in the historical sections below.

Accepted implementation: 288 deterministic point stars, centralized 1.6x
clamped RGB gain, 960 initial approach depth offset, 96-unit approach-only
perspective bias, and a 181-update smoothstep approach. Stars begin during
late current-tube exit, remain continuous into Approach, diminish and vanish
before arrival. Blue exits 1-4 and later white/red/green/blue assignment,
exact `SUPERZAPPER RECHARGE`, normal HUD, distant tube, tiny visible
spinner-controlled rim Blaster and smooth arrival are retained.

The first visible star remains at ExitZoom update 65, commit at update 124,
and 59 exit intervals plus 181 approach updates produce 240 frame intervals:
approximately four seconds at normal 60 FPS. Stars are absent at approach
tick 154. Normal gameplay projection and zero bias/depth offset at arrival
are unchanged. Slow frames can extend wall-clock duration.

The actual Atari observations remain ARCADE VERIFIED/direct visual evidence
as recorded below. Exact count, deterministic coordinates/distribution,
brightness gain, RGB values/proportions, depth, bias, thresholds, ticks,
interpolation and text styling remain PROJECT DECISIONS. Manual acceptance
does not convert these into measured Atari internals. Neal's fidelity
standard permits reasonable compromises rather than frame-perfect cloning;
do not continue tuning accepted details.

### Finalization changes and records

No production code or tests were changed during finalization.
Updated `README.md`, `TEMPEST-BEHAVIOR-SPEC.md`, and
`TEMPEST-DEVELOPMENT-NOTES.md` to record accepted behavior, distinguish
evidence from implementation choices, replace superseded 120/240 tuning,
and remove the deferred approach-perspective item. Other milestones remain
out of scope.

Records checked:

- Original analysis and bounded perspective implementation prompt/report
  remain preserved under AI-Logs.
- This report preserves the frame identities, supplied observations, source
  findings, iterative tuning, historical policy blocker, real validation
  results, and final manual acceptance.
- [Consolidated follow-on/finalization requirements](../Prompts/2026-11-03-between-level-approach-presentation-follow-on-and-finalization-prompt.md)
  fill the missing follow-on prompt record. It is explicitly a consolidated
  requirements record, not a claim of verbatim transcription.
- The original perspective experiment report links to this follow-on record
  for current validation/acceptance rather than treating initial blocked tests
  as a pass.

### Final validation gate

All tests actually executed, zero failures or skips:

| Gate | Result |
|---|---|
| Focused transition/approach | **45 passed** |
| Affected progression/rendering | **398 passed** |
| Full solution | **640 passed** |
| Full solution build | **0 warnings, 0 errors** |
| `git diff --check` | **PASS**, line-ending conversion advisories only |

Commands/filters remain the established ones documented below. No policy
was bypassed. Reviewed the complete tracked diff and both new source/test
files: no unrelated implementation changes are present.

### Final staging recommendation (not executed)

HEAD remains `15933a8`. The intentional `.gitignore` diff only adds `Temp/`,
plus the separator newline needed after the previously unterminated `Videos/`
line. No other ignore rule was changed; this maintenance is approved for the
final staging boundary.

Exactly 11 intended files, including the two currently untracked source/test
files:

```powershell
git add -- .gitignore README.md TEMPEST-BEHAVIOR-SPEC.md TEMPEST-DEVELOPMENT-NOTES.md src\Tempest.Game\Levels\LevelApproach.cs src\Tempest.Game\Rendering\TransitionStarfield.cs src\Tempest.Game\Rendering\VectorRenderer.cs src\Tempest.Game\TempestGame.cs tests\Tempest.Game.Tests\BlasterLaneHighlightTests.cs tests\Tempest.Game.Tests\LevelApproachTests.cs tests\Tempest.Game.Tests\TransitionPresentationTests.cs
```

Recommended commit subject: `Refine accepted between-level approach presentation`.
AI-Logs remains ignored and is not included in staging. No stage, commit or
next milestone was performed.

---

## HISTORICAL IMPLEMENTATION / TRIAL RECORD

## BRIGHTNESS-ONLY TRIAL (2026-11-03, LATEST STATUS)

Neal completed the density comparison: 288 stars look much better than 192.
Keep 288; density is now good and is not being retuned. The remaining request
is brighter tiny points, with all accepted presentation behavior preserved.

Previously each star used the standard Raylib palette RGB without gain.
Alpha was `round(255 * clamp(visibility * 288 - index, 0, 1))`, subject to
the accepted 192-star-reference zero-alpha visibility gate. Most visible
stars were already at alpha 255, so raising peak alpha would not brighten
them and could disturb appearance/removal.

The new centralized `StarIntensityGain = 1.6f` in `TransitionStarfield`
maps each RGB channel to `min(255, round(channel * 1.6))`. It does not touch
alpha, point size, positions, color assignment, or visibility decisions.
For example, the existing Raylib blue (0,121,241) becomes (0,194,255):
still blue, but brighter. Red and green receive the same gain; white remains
unchanged because it is already full intensity. Channel saturation is an
intentional compromise of this bounded brightness trial, not an Atari
measurement. No glow, halo, sprite, streak, or particle effect was introduced.

Preserved exactly: 288 stars, generator/count/coordinates, tiny point size,
early blue-only and later white/red/green/blue assignment, onset at ExitZoom
update 65, exit-to-approach continuity, first star-free approach tick 154,
240-interval approximately-four-second journey, 960 depth offset, 96-unit
bias, recharge text, HUD, tiny Blaster/spinner presentation, normal projection
and gameplay/progression semantics.

Focused tests update the expected palette intensity, explicitly check first
star-free tick 154, and compare the prior alpha/count envelope at 1001
visibility values for both early and later color policies. They verify
deterministic position reuse and increased RGB luminance for colored stars
without changing white intensity. The existing actual-onset/four-second
tests remain unchanged.

Files changed for this trial:

- `src/Tempest.Game/Rendering/TransitionStarfield.cs`
- `tests/Tempest.Game.Tests/TransitionPresentationTests.cs`
- This ignored report.

Actual validation: **45 focused**, **398 affected**, **640 full-suite** cases
passed, zero failures/skips. Solution build: zero warnings/errors.
`git diff --check`: passed with existing line-ending advisories only.
Nothing staged or committed. The unrelated `.gitignore` modification remains
untouched.

Manual comparison pending:

```powershell
dotnet run --project src\Tempest.Game -- --lives 10
```

Judge sufficient brightness while retaining tiny distant points, accepted
density, clearly blue early stars, distinct distant tube, readable recharge
message and tiny Blaster, spinner clarity, identical onset/disappearance and
four-second timing, and seamless arrival without snap, flash, wrong board or
Blaster jump. If it looks right, retain it and stop starfield tuning.

## 288-STAR DENSITY TRIAL (2026-11-03, LATEST STATUS)

Neal manually tested the distance/timing refinement and reported that it now
looks close to perfect. Preserve that presentation; this trial changes only
density from **192 to 288 stars (+50%)**, pending visual comparison.

Changed `TransitionStarfield.MaximumStars` to 288. The unchanged generator
automatically uses 18 rather than 12 rows, retaining 16 columns, the same
integer hash, jitter, permutation, bounds, sizes, and colors. Coordinates
therefore redistribute mechanically within the existing generator; no
independent position tuning was performed.

The initial count-only test run caught a subtle coupling: multiplying
visibility by the larger count caused alpha rounding to reveal a star on
ExitZoom update 64 rather than the accepted update 65. A tiny centralized
visibility gate now preserves the original 192-star zero-alpha cutoff,
independently of the density trial. This is the only mechanical adjustment
beyond the count. No visibility curves, thresholds, or state durations were
retuned. The four-second interval remains exactly 240 frame intervals at
the normal 60 FPS cadence.

The focused color/distribution test now explicitly expects 288 stars. All
other transition/progression tests and the accepted perspective/distance
settings remain unchanged. Actual validation: **43 focused**, **398
affected**, **638 full-suite** cases passed, with zero failures/skips;
solution build has zero warnings/errors; `git diff --check` passes.

Files changed for this trial:

- `src/Tempest.Game/Rendering/TransitionStarfield.cs`
- `tests/Tempest.Game.Tests/TransitionPresentationTests.cs`
- This ignored report.

Nothing staged or committed; existing worktree changes remain in place.
The unrelated `.gitignore` modification was left untouched.

Manual comparison remains pending:

```powershell
dotnet run --project src\Tempest.Game -- --lives 10
```

Decide whether the 50% denser field resembles the supplied arcade reference
better without clutter or a particle-like appearance. Confirm tiny blue
points, natural exit onset, distribution around the distant tube, diminishing
stars, recharge-message readability, tiny Blaster/spinner visibility,
unchanged four-second journey, and seamless arrival without snap, flash,
wrong board or Blaster jump. If overly dense, choose between this trial and
the already-good 192-star version rather than arbitrarily retuning it.

## DISTANCE / TIMING REFINEMENT (LATEST STATUS)

Neal manually tested the combined starfield and perspective experiment and
reported that the presentation is very good; no redesign is requested. His
remaining targets are a farther/smaller incoming tube and approximately four
seconds from first visible stars during exit to arrival. That duration is
Neal's observed arcade target, not a frame-perfect synchronization claim.
This section supersedes the earlier numerical tuning and pending-manual-test
status below. The revised distance/timing still needs another manual comparison.

### Actual runtime cadence and measured update boundaries

`src/Tempest.Game/Program.cs` calls `Raylib.SetTargetFPS(60)` and executes one
`RunOneStep` followed by one `renderer.Draw()` per loop. There is no elapsed-time
accumulator, delta-time update, or catch-up loop. The established
`TempestGame.SimulationStepSeconds = 1.0 / 60.0` agrees with that normal cadence,
but by itself does not control the runtime. No runtime/gameplay timing code
was changed. Slow frames can lengthen wall-clock duration; the calculation is
for actual normal operation at the established 60 FPS target, not a guarantee
under stalls.

Tests now trace the real exit updates and capture actual star submissions,
including the alpha rounding in `TransitionStarfield.Draw`, rather than merely
assuming stars become visible at a depth threshold:

| Boundary / interval | Updates or frame intervals | At normal 60 FPS |
|---|---:|---:|
| ExitZoom begins | 0 | 0 s |
| First submitted visible star | 65 | 1.0833 s after ExitZoom begins |
| Incoming level committed / approach tick 0 | 124 | 2.0667 s after ExitZoom begins |
| Exit remainder after first visible star | 59 | 0.9833 s |
| Previous approach duration | 120 | 2 s |
| Previous first-star-to-arrival duration | 179 | 2.9833 s |
| Revised approach duration | 181 | 3.0167 s |
| Revised first-star-to-arrival duration | 240 | 4 s |
| Arrival relative to ExitZoom start | 305 | 5.0833 s |

These intervals are measured between rendered-state boundaries: from the
frame containing the first star to the arrival frame, not an inclusive count
of both endpoint frames. The existing ExitDelay occurs before these
boundaries and is not part of the requested first-star interval.

The existing exit math reaches just below half depth on update 64, but its
very faint first star rounds to alpha zero. The first nonzero-alpha submission
is update 65, with the player at approximately depth 196.6 of 400. Exit logic,
onset threshold, alpha policy, and commit threshold remain unchanged.

### Distance refinement and preserved presentation

- `LevelApproach.InitialDepthOffset`: **240 -> 960**.
- Near-rim presentation depth at first appearance: **290 -> 1010**, or
  **295 -> 1015** including the unchanged `ZOffset = 5`.
- First near-rim dimensions are approximately **295 / 1015 = 29.06%** of their
  previous initial dimensions, without modifying the tube's geometry.
- Initial Blaster claw height remains proportionate to the rim:
  about **1.08 logical pixels / 1.73 physical pixels** at the accepted scale,
  rather than the previous 3.73 logical pixels.
- `LevelApproach.DurationTicks`: **120 -> 181**. Only presentation-state
  lifetime is extended; incoming gameplay remains frozen until the same
  arrival boundary semantics, then starts on the next step.
- Smoothstep depth interpolation is unchanged. Normal projection, geometry,
  display scaling, gameplay/input rules, and natural progression are unchanged.
- The **96-unit perspective bias** is retained, normalized by the new initial
  depth so it still starts at 96 and fades exactly to zero. No extra vertical
  magnitude or camera adjustment was necessary.
- Star count, positions, color policy, point appearance, and exit continuity
  are unchanged. The existing 85%-of-approach disappearance envelope adapts
  automatically: stars are absent by **tick 154**, leaving the final **27
  ticks (0.45 seconds)** star-free.
- `SUPERZAPPER RECHARGE`, top HUD, rim attachment and spinner movement are
  preserved.

The exact distance and 181-update duration are PROJECT DECISIONS that implement
Neal's qualitative distance and approximate timing targets; they are not
claims about Atari camera parameters.

### Files changed in this refinement

- `src/Tempest.Game/Levels/LevelApproach.cs`: two presentation constants and
  their tuning rationale.
- `tests/Tempest.Game.Tests/LevelApproachTests.cs`: revised duration/distance
  expectations, smaller initial rim geometry comparison, and monotonic near/
  far rim growth at every approach update.
- `tests/Tempest.Game.Tests/TransitionPresentationTests.cs`: three natural-exit
  timing cases tracing first actual visible point through arrival; adaptive
  star-removal boundary and normalized perspective expectations.
- This ignored report: current manual result, calculation, tuning and validation.

Other uncommitted changes from the preceding presentation work remain intact.
The unrelated `.gitignore` modification was not edited. Nothing was staged or
committed; HEAD remains `15933a8`.

### Latest automated validation

All tests actually executed with no failures or skips:

| Validation | Result |
|---|---|
| Focused transition/approach tests | **43 passed** |
| Affected progression/rendering tests | **398 passed** |
| Complete solution tests | **638 passed** |
| Complete solution build | **0 warnings, 0 errors** |
| `git diff --check` | **PASS**, line-ending advisories only |

Used the same commands/filters documented below, rebuilding through the
focused test command before the affected and full `--no-build` runs.
The focused tests check the exact 240-interval target for all three existing
natural exit/approach edges. Geometry, Blaster/rim attachment, spinner movement,
zero arrival bias, exact normal projection, pre-arrival star disappearance,
wave freeze and progression checks all pass. No tests assert subjective arcade
distance fidelity.

### Next manual comparison

```powershell
dotnet run --project src\Tempest.Game -- --lives 10
```

Confirm the incoming tube now looks sufficiently distant; the stars, distant
tube and approach read as one journey; and the total duration feels close to
the arcade's approximately four seconds. Check recharge text, tiny Blaster
visibility/spinner response, natural star disappearance and fully smooth
arrival, with no snap, flash, wrong board or Blaster jump. Do not redesign the
already good presentation or begin another milestone.

---

## EARLIER STARFIELD IMPLEMENTATION RECORD

## CHECKPOINT / STATUS

- HEAD remains `15933a85f8ac7e9b1390a5436d092d96774c3213` (`15933a8 Refine accepted Blaster death animation`).
- This extends the uncommitted 96-unit approach-perspective experiment within the same between-level presentation milestone.
- Automated validation now actually executes and passes: 40 focused cases, 398 affected cases, and 635 full-solution cases, with zero failures or skips. Build: zero warnings/errors. `git diff --check`: passed.
- The previous Windows Application Control failure is a historical result, not a passing run. The ordinary commands succeeded during this follow-on run; no policy bypass or policy changes were made.
- No stage or commit. The intentional pre-existing `.gitignore` modification remains untouched. `Temp/` was not investigated.
- Manual visual acceptance of the combined experiment is still pending. No durable arcade-behavior document has been updated to claim acceptance.

## NEW ARCADE EVIDENCE

Neal supplied these actual Atari transition frame identities:

- `Transition-frame_0038.png`
- `Transition-frame_0066.png`
- `Transition-frame_0080.png`
- `Transition-frame_0110.png`
- `Transition-frame_0130.png`
- `Transition-frame_0140.png`

The user reports that the browser AI inspected them. This implementation uses the supplied observations as authority; it does not claim independent local inspection of those images. They were not found in the narrowly checked existing `Videos/Video Frames` directory. No broader media/scratch investigation was needed.

### ARCADE VERIFIED / DIRECT VISUAL EVIDENCE

- Many tiny point-like stars against black, not elongated warp-speed streaks.
- Blue stars in the supplied early-level frames.
- Exact uppercase wording: `SUPERZAPPER RECHARGE`.
- A very distant/small incoming tube surrounded by stars, with its small Blaster visible on the rim.
- Stars diminish as the tube grows.

### NEAL DIRECT ARCADE OBSERVATION

- Stars start appearing around halfway down the current tube's exit. "About halfway" is qualitative, not a measured threshold.
- Exits from Levels 1 through 4 use blue stars.
- Starting with exit from Level 5, use a white/red/green/blue mixture.
- Stars are completely gone shortly before the approaching Blaster reaches the rim.
- Spinner input moves the tiny Blaster around the approaching rim.

### PROJECT DECISIONS

The count, distribution, exact activation/removal thresholds, smoothstep envelopes, static star positions, alpha handling, later-level color proportions, and message styling/timing below are project tuning choices, not measurements of Atari internals.

## IMPLEMENTATION ARCHITECTURE

`Rendering/TransitionStarfield.cs` is a compact, stateless deterministic presentation helper. It uses no `Random`, no gameplay entities, no particles, and no per-frame mutable star state.

It generates up to 192 point positions on the existing logical canvas. A fixed integer hash jitters points within 16 by 12 cells. The cell order is permuted so stars appear/disappear around the screen rather than sweeping in rows. Positions are stable between frames, across independent draws, and across color phases. Drawing submits single pixel points through the renderer's callback seam; no streak lines are submitted.

The distribution reserves the top HUD region: points are below logical Y=112, with side/bottom margins. Stars draw before tube geometry, while the existing HUD draws last and remains unchanged. Normal display scaling remains 800x600 logical at 1.6 scale to 1280x960.

`LevelApproach` now retains only one extra piece of information: `ExitedLevel`. The existing fresh-wave initializer captures the outgoing displayed level before replacing the runtime. That preserves the source-level color policy during approach rather than accidentally using the incoming level. No timer, simulation ordering, progression resolver, or wave setup policy was changed.

## EXIT-TO-APPROACH LIFECYCLE

1. Playing, AvoidSpikes, ExitDelay, ReverseZoom, death, Complete and GameOver render no transition stars.
2. During ExitZoom, derive visibility from the Blaster's remaining tube depth. Stars begin after remaining depth drops below 50% and reach full visibility by 20% remaining depth, using smoothstep. These exact thresholds are project approximations to "around halfway".
3. Full visibility is reached before the existing successful exit commit. The source level and fixed star distribution survive the handoff: the last full exit starfield equals the first approach starfield in positions, count, and colors.
4. During Approach, visibility is `1 - smoothstep(progress / 0.85)`. Visible stars progressively disappear in the stable permuted order. The last marginal star fades in/out through alpha rather than abruptly changing the whole field's brightness.
5. Stars are exactly absent by approach tick 102, leaving the final 18 of 120 ticks star-free.
6. Playing starts at the existing boundary, with no stars or recharge message. Restart, terminal completion, and spike-reversal states do not retain stale presentation state.

The current terminal Level 4 still has no implemented successor. Its exit can display blue stars, but this milestone adds no Level 5 gameplay or campaign progression.

## COLOR POLICY

- Source exit level 1, 2, 3 or 4: blue only.
- Source exit level 5 and later: repeating white/red/green/blue assignment across the deterministic distribution.
- This reuses the project's Raylib colors; exact RGB values and equal assignment are presentation choices.
- The policy does not cycle at level 16 and does not implement any broader palette milestone.
- Future incoming Level 5 must retain Level 4's blue exit field; future incoming Level 6 must retain Level 5's four-color field. Capturing the exited level rather than deriving it from the incoming configuration protects that distinction.
- Tests cover later source-level policy directly and with a synthetic Level 5 exit configuration; no production Level 5 was added.

## SUPERZAPPER RECHARGE / HUD

`VectorRenderer.DrawRechargeMessage` submits exactly `SUPERZAPPER RECHARGE`, centered using the existing `DrawCenteredText` path at logical Y=540, size 20, white.

The message is visible throughout Approach and absent during ordinary play, exit, death and terminal states. That exact lifetime, Y position, font size and white color are project decisions. Existing Raylib text rendering was reused because `VectorFont` is only an empty scaffold; no new font renderer or unrelated HUD typography was introduced.

The message is visual only: it does not recharge Super Zapper again or change its established reset/input semantics. The existing top score/lives/level HUD is unchanged.

## INTERACTION WITH THE PERSPECTIVE EXPERIMENT

- The 96-unit pre-projection vertical bias is preserved exactly, following `96 * (ApproachDepthOffset / 240)` and reaching exactly zero at arrival.
- The starfield is a logical-screen background, independent of the tube's approach projection and ExitZoom's animated projection globals.
- The tube and Blaster retain the existing 240-unit distant start, 120 ticks, and smoothstep depth progression. No additional unmeasured scale/depth tuning was introduced in this extension.
- Blaster claw sizing, rim attachment and spinner movement are unchanged. Existing perspective tests now execute successfully.
- Initial tube smallness relative to the reference remains part of Neal's visual comparison, not something established by deterministic tests.
- No change to `Projection.K`, `Projection.ZOffset`, normal gameplay coordinates, or 1280x960 scaling was needed.

## FILES CHANGED

New in this follow-on:

- `src/Tempest.Game/Rendering/TransitionStarfield.cs`
- `tests/Tempest.Game.Tests/TransitionPresentationTests.cs`
- This ignored implementation report.

Adjusted for the follow-on:

- `src/Tempest.Game/Levels/LevelApproach.cs`: retain exited level.
- `src/Tempest.Game/TempestGame.cs`: capture exited level at the existing fresh-wave boundary.
- `src/Tempest.Game/Rendering/VectorRenderer.cs`: draw background stars and approach-only recharge text.
- `tests/Tempest.Game.Tests/LevelApproachTests.cs`: supply the source level in the model test.
- `AI-Logs/Reports/2026-11-02-between-level-approach-perspective-fidelity-experiment-report.md`: mark the later successful validation as superseding the earlier blocker.

The worktree also retains the prior perspective implementation and its changes to `BlasterLaneHighlightTests.cs` and `LevelApproachTests.cs`. The unrelated `.gitignore` modification is not part of this milestone.

## AUTOMATED VALIDATION

Tests protect:

- No stars before the intended late-exit window; increasing exit density and full visibility before commit.
- Stable finite point coordinates within bounds, deterministic cell distribution, and no color-dependent positional changes.
- Blue-only exits 1-4 and the established four-color set for source level 5 and later.
- Natural handoff continuity for incoming Levels 2, 3 and 4.
- Monotonically diminishing visibility/density during approach, zero stars by tick 102, and no premature gameplay.
- Exact recharge-message wording and intended approach visibility.
- Player registration to rim geometry during spinner movement.
- Frozen incoming Eggs/reserves/accounting, suppressed Fire/Zapper, carried score/lives, unchanged projection globals, and exact 120-tick arrival.
- Zero perspective bias and no transition overlays at normal gameplay arrival.
- No starfield/message in other states even if an approach model remains present.
- Repeated presentation submissions do not consume gameplay RNG calls or change simulation outcomes versus an identical control run. The comparison accounts for the existing globally shared projection values.

No tests claim pixel-perfect rendering or subjective arcade fidelity.

Commands from the repository root:

```powershell
dotnet build TempestCSharp.slnx --no-restore
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-build --no-restore --filter "FullyQualifiedName~TransitionPresentationTests|FullyQualifiedName~LevelApproachTests"
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-build --no-restore --filter "FullyQualifiedName~LevelProgressionTests|FullyQualifiedName~LevelConfigurationTests|FullyQualifiedName~BlasterLaneHighlightTests|FullyQualifiedName~DisplayConfigurationTests|FullyQualifiedName~Level4CandidateTests|FullyQualifiedName~SimulationTests"
dotnet test TempestCSharp.slnx --no-build --no-restore
git diff --check
```

| Validation | Actual result |
|---|---|
| Solution build | PASS: 0 warnings, 0 errors |
| Focused transition/perspective | 40 passed, 0 failed, 0 skipped |
| Affected progression/rendering | 398 passed, 0 failed, 0 skipped |
| Full solution | 635 passed, 0 failed, 0 skipped |
| Diff check | PASS; line-ending conversion advisories only |
| Diff review | Reviewed lifecycle and renderer integration; no unrelated production changes |

These are real test executions, not a no-tests-discovered exit code. The policy blocker did not recur; no system security configuration was altered.

## REMAINING MANUAL VALIDATION

Use the normal campaign from the repository root:

```powershell
dotnet run --project src\Tempest.Game -- --lives 10
```

Compare with the supplied Atari frames/actual arcade or MAME and the accepted pre-experiment presentation:

1. Stars appear around mid-exit, not at wave clear or only after approach begins.
2. Stars read as many tiny blue distant points, not streaks, oversized particles, or a visibly regular grid.
3. The same field continues through the exit/approach handoff with no flash or reset.
4. The small incoming tube is convincingly distant within the field; its tiny Blaster remains visible and spinner-movable.
5. The 96-unit bias provides a better elevated/viewpoint-travel impression while the tube still grows naturally.
6. Stars progressively disappear and are gone shortly before arrival.
7. `SUPERZAPPER RECHARGE` is legible lower-center without harming tube/Blaster visibility; the normal top HUD is preserved.
8. No premature gameplay, arrival snap, vertical jump, projection flash, wrong board, or Blaster jump.
9. Normal campaign score/lives/progression and 1280x960 scaling remain correct.
10. Is the combined result clearly better? If the perspective experiment is not better, prefer retaining the previously accepted perspective rather than escalating complexity. Do not equate rejecting the bias with rejecting the newly supplied starfield/message evidence.

Level 5+ mixed colors are covered objectively but cannot be naturally exercised until that campaign content exists. Do not add a new level solely for this visual check.

No manual acceptance, commit, or staging was performed.
