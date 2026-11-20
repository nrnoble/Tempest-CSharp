# Whole-Game Presentation Perspective / Transition Interaction

**Date:** 2026-11-14
**Status:** READ-ONLY INVESTIGATION COMPLETE
**Completion token:** `TEMPEST_PRESENTATION_PERSPECTIVE_TRANSITION_INTERACTION_READONLY_20261114`

## Scope and starting Git state

This investigation was limited to the relationship between the experimental
portrait-perspective correction and the existing exit/reverse camera. No source,
test, durable documentation, parser, recording, or candidate file was edited.
No game, build, or test suite was run.

Starting state matched the task's expected baseline:

- Branch: `master`
- `HEAD` and `origin/master`: `385f603a94391756b2ca6cccd524dc17b281e782`
- Index: clean
- Tracked worktree: exactly the nine expected modified candidate files
- Non-ignored untracked files: none
- Starting diff: 9 files changed, 1,070 insertions, 39 deletions

The substantive prompt was copied verbatim to
`AI-Logs/Prompts/2026-11-14-whole-game-presentation-perspective-transition-interaction-readonly-prompt.md`.

## Materials inspected

Authoritative reports:

- `AI-Logs/Reports/2026-11-14-whole-game-presentation-shared-perspective-experiment-report.md`
- `AI-Logs/Reports/2026-11-14-whole-game-presentation-perspective-range-expansion-report.md`

Source and directly relevant durable documentation:

- `src/Tempest.Game/Rendering/Projection.cs`
- `src/Tempest.Game/Rendering/DisplayConfiguration.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `src/Tempest.Game/TempestGame.cs`
- `src/Tempest.Game/Levels/Level.cs`
- `src/Tempest.Game/Levels/LevelApproach.cs`
- `src/Tempest.Game/Levels/LevelState.cs`
- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/Debug/GameplayPlaybackSession.cs`
- `src/Tempest.Game/Debug/VisiblePlaybackController.cs`
- `src/Tempest.Game/Debug/DebugController.cs`
- `TEMPEST-BEHAVIOR-SPEC.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`

No screenshots, images, videos, gameplay recordings, MAME frames, contact
sheets, Gemini material, Incitti source, web sources, or raw ROMs were opened,
searched, or analyzed.

## Current correction, range, and semantic role

For world point `(x,y,z)`, camera center `CenterY`, model offset `YOffset`,
near anchor `n`, live `K` and `ZOffset`:

```text
q(z) = 1 / (z + ZOffset)
q(n) = 1 / (n + ZOffset)

xProjected = 400 + x * K * q(z)
yProjected = CenterY + (y + YOffset) * K * q(z)
             + a * K * (q(z) - q(n))
```

`a` is the process-level presentation value, default `0`, presently bounded
to `[-5,+5]`; it is not simulation or recording metadata. The shared
projection implementation applies the correction before the uniform
experimental playfield camera. That camera has output scale `1.2` times
playfield scale `1.05`, or `1.26`, so host-pixel values below include that
factor.

The accepted stable calibration context is `K=70`, `ZOffset=5`, near depth
`50`, far depth `450`. At stable play, the formula is algebraically equivalent
to a depth-dependent vertical model displacement `a`, anchored to preserve
the near plane. Thus the current formula gives a coherent single-camera
projective image and is useful as a stable portrait calibration. But its
actual implementation treats `a` like a world-space Y displacement under
whatever live camera is active: both animated `K` and animated `ZOffset`
scale the term. The durable candidate comment and option behavior classify
`a` as presentation state, not as a second gameplay object or simulation
camera. The stable calibration intent is therefore not a reason to multiply
it by every transient camera value.

## Projection and camera lifecycle

Production levels currently share `Position=50`, `Depth=400`, so ordinary
near/far world depths are `50/450`. Approach applies its existing
`DepthOffset` to both ends and to the near reference. `Projection.Project`
adds the live `ZOffset` to depth; the portrait correction is added there.
`VectorRenderer.Draw` places the playfield under one uniform affine
`Camera2D`; HUD/overlays use a distinct camera and are not part of this
projection.

| State/context | Live `K`, `ZOffset` | Depth and anchor context | Projection path / registration |
|---|---|---|---|
| Normal `Playing` | `70, 5` | Tube `50..450`; near anchor `50` | Tube endpoints and tube-relative centers use the shared `Projection.Project` mapping. Portrait playfield camera follows projection. |
| `ExitDelay` | `70, 5` | Same normal depths/anchor | Delay counter advances; no camera animation. Ordinary ExitDelay freezes movement; the presentation correction remains the stable one. |
| `AvoidSpikes` | `70, 5` | Same normal depths/anchor | `UpdateExitSimulation` restores projection during this state. Spike/tube/player paths share the same projection context. |
| `ExitZoom` entry/phase 1 | At entry `70,5`; then `K=70`, `ZOffset` decreases by 2 from `5` through `-41` | Tube stays `50..450`; player projected depth is `50 + 400 - Player.Depth`, progressing from near depth toward about `440`; anchor remains `50` | Existing base projection and portrait correction both use live `K/ZOffset`. Spike and player shots remain projected; the player uses `ProjectApproachGeometry`; normal enemy drawing is suppressed. |
| `ExitZoom` phase 2 | `ZOffset=-41`; `K` rises by 2 per tick, with another 2 after it exceeds 100; last documented rendered exit reaches `K=440` | Same tube range/near anchor; player remains on the tube depth interval through successful exit | Same shared projective path and portrait camera. The base zoom itself remains unchanged by this investigation. |
| Fatal exit-Spike collision | Enters `ReverseZoom` using the just-advanced exit `K/ZOffset` | Tube/anchor remain `50..450` | `CheckSpikesAgainstPlayer` changes state; subsequent rendering still uses current live projection values. |
| `ReverseZoom` | Each update `K=max(70,K-4)`, `ZOffset+=1`; `ZOffset` clamps at `5`, then player depth resets | Tube/anchor remain `50/450`; projection follows the reverse sequence | Same paths. When reversal completes, projection is restored before retry/return to play; if game over starts, `BeginGameOverPresentation` also restores it. |
| Successful exit / `Complete` | Restored to `70,5` | Standard context | `CompleteSuccessfulExit` either restores directly or installs a fresh wave; no added transition state was introduced. |
| Between-level `Approach` | `70,5` throughout | Tube near/far are `50+offset` and `450+offset`; `offset` begins at 960 and smoothsteps to zero over 181 ticks; correction anchor is `50+offset` | `ProjectApproachGeometry` passes both shifted point depth and shifted near anchor. Tube, player, and spike geometry go through that helper/`ProjectLane`; the portrait camera remains affine. Gameplay entities are otherwise hidden/frozen as implemented. |
| Game Over vectors | `70,5` (restored on entry) | Vector record depths are `Position + record.Z`; recorded values described in the prior report project within `66..289`; default near reference is `50` | Vector records call `Projection.Project` directly and use the same portrait playfield camera. Screen-space HUD remains separate. |

`BeginExit` selects `AvoidSpikes` or `ExitDelay`; standard ExitDelay enters
ExitZoom after its counter, and active ExitZoom begins animating on the next
update. In the first exit phase, player depth decreases by 3 per tick while
ZOffset moves; in phase 2, K grows and player depth decreases by 3.2. The
projection is restored on successful completion, AvoidSpikes handling,
reverse completion, new-wave initialization, and Game Over entry.

Source trace: `TempestGame.cs:392-459` (state dispatch),
`:538-555` (fresh wave / normal / approach setup), `:699-704` (Game Over
restoration), `:782-794` (approach update), `:2343-2378` (exit start and
two camera phases), `:2381-2420` (completion and active-exit ordering),
`:2425-2435` (AvoidSpikes / ExitDelay), `:2446-2466` (fatal Spike
transition), and `:2473-2509` (ReverseZoom / restoration). Projection
parameters and formula are in `Projection.cs:6-56`; the portrait camera is
in `DisplayConfiguration.cs:29-55`; world depth constants are in
`Levels/Level.cs:18-19`; Approach depth/offset progression is in
`Levels/LevelApproach.cs:8-41`.

The renderer's tube and gameplay-space registrations are centralized enough
for one correction: tube rails and rims use `ProjectApproachGeometry`;
`ProjectLane` calls that helper; the Blaster uses it directly. Spikes,
enemy centers, player/enemy shots, and gameplay-space effect centers use
`ProjectLane` or the same `Projection.Project` equation at their computed
world depth. Fuseball/Pulsar and player-shot paths call `Projection.Project`
directly but are hidden during Approach, where the shifted anchor matters.
Game Over direct projection occurs after restoration. Glyph scales may also
use live `K/ZOffset`, but their anchor centers use the common projected
position; changing the perspective term does not alter their X coordinate.
No relevant path was found that assigns a different projection to equivalent
tube-space anchors in the active gameplay/exit contexts. Relevant renderer
locations: `VectorRenderer.cs:149-179,198-240,386-392,421-443,503-517,
574-598,680-739,744-765,835-851,908-928,1048-1052`.

## Bounded numerical analysis

For the reported normal far depth (`z=450`, `n=50`) the current/live correction
is `1.26 * a * K * (1/(z+ZOffset)-1/(n+ZOffset))`.

| Context | `K`, `ZOffset` | Current/live host shift per `a` at depth 450 | Current/live shift at `a=+40` | Stable-baseline direct shift at `a=+40` |
|---|---:|---:|---:|---:|
| Stable play | `70, 5` | `-1.409790 px` | `-56.392 px` | `-56.392 px` |
| Early ExitZoom | `70, 3` | `-1.469449 px` | `-58.778 px` | `-56.392 px` |
| Phase boundary | `70, -41` | `-9.584352 px` | `-383.374 px` | `-56.392 px` |
| High-K ExitZoom | `440, -41` | `-60.244499 px` | `-2409.780 px` | `-56.392 px` |
| Early ReverseZoom | `436, -40` | `-53.596098 px` | `-2143.844 px` | `-56.392 px` |
| Middle ReverseZoom | `360, -21` | `-14.584037 px` | `-583.361 px` | `-56.392 px` |
| Later ReverseZoom | `276, 0` | `-6.182400 px` | `-247.296 px` | `-56.392 px` |

These are mathematical probes, not observations. They reproduce the prior
report's `-383.4 px` boundary result and explain the much greater high-K
amplification. `K` multiplies the added term directly; reducing the near
denominator from `55` to `9` further magnifies its depth difference. Reverse
zoom uses the same coupled formula until its live parameters return, so
reversing the camera does not avoid amplification.

For nonstandard contexts:

- Full-offset Approach uses `n=1010`, `f=1410`, `K=70`, `ZOffset=5`. Its
  stable-baseline far correction is only `-0.024564 px per a`, or `-0.983 px`
  at `a=+40`. Approach itself does not animate K/ZOffset.
- Ordinary stable play at maximum enemy-retraction depth `850` remains under
  `K=70,ZOffset=5`; its direct stable correction at `a=+40` is `-60.019 px`.
- Game Over's reported `66..289` projected depths also use restored
  `70,5`; direct stable correction at `a=+40` ranges from `-14.455 px`
  at depth 66 to `-52.145 px` at depth 289.
- All present production levels use the same 50/400 near/depth values, but a
  future implementation should use each active configuration rather than
  hard-code those values. Approach must pass its shifted near/far context.

## Candidate analyses

### 1. Keep live K/ZOffset coupling

This is the current implementation. It is coherent as one projective
projection: for any frame, both projected coordinates are affine in
`qLive=1/(z+ZOffset)`. It preserves near anchoring, X coordinates,
line incidence, tube/entity registration, and stateless playback. Its
semantic interpretation, however, is a model-space vertical displacement
under the animated camera, not a fixed portrait calibration. At the
ExitZoom boundary and high-K frames it amplifies the stable probe by about
6.8x and 42.7x respectively. Even the current maximum `a=+5` produces
about `-301 px` at the reported high-K far-depth probe. This is the known
amplification defect, not a safe range-expansion strategy.

### 2. Direct stable-baseline correction (the literal pointwise candidate)

The direct candidate leaves the base projection live but adds:

```text
q0(z) = 1 / (z + 5)
q0(n) = 1 / (n + 5)

deltaY = a * 70 * (q0(z) - q0(n))
```

It exactly preserves the stable image and near anchor, keeps X unchanged,
and removes live-K/live-Z amplification from the additive correction. It is
stateless and reversible. However, at a transient `ZOffset != 5`, the live
base uses `qLive`, while `q0` is a nonlinear rational function of `qLive`:

```text
q0 = qLive / (1 + (5 - ZOffset) * qLive)
```

For a fixed tube rail vertex, the live base x/y are affine in `qLive`, but
this added term is not. Thus the projected model-space rail is generally a
curve while `DrawTube` submits a single endpoint-to-endpoint line. At the
early `ZOffset=3` frame, a `+40` probe has up to `0.460 px` host-space
departure from that endpoint chord. At the `ZOffset=-41` boundary it reaches
`22.660 px`; high-K ExitZoom has the same curvature because the mismatch is
caused by ZOffset, not K. ReverseZoom has up to `21.431 px` early,
`8.136 px` at a representative middle state, and `1.188 px` near return.
At `a=+5`, these values scale to one eighth, but are still nonzero.

Representative point residuals (host pixels, `a=+40`) relative to the
near/far endpoint segment:

| State | Near | 25% depth (150) | Mid-depth (250) | 75% depth (350) | Far |
|---|---:|---:|---:|---:|---:|
| Stable `70,5` | 0 | 0 | 0 | 0 | 0 |
| Early ExitZoom `70,3` | 0 | +0.357 | +0.175 | +0.067 | 0 |
| Boundary `70,-41` | 0 | +11.515 | +4.867 | +1.774 | 0 |
| High-K exit `440,-41` | 0 | +11.515 | +4.867 | +1.774 | 0 |
| Reverse early `436,-40` | 0 | +11.163 | +4.739 | +1.729 | 0 |
| Reverse middle `360,-21` | 0 | +5.500 | +2.511 | +0.942 | 0 |
| Reverse later `276,0` | 0 | +0.910 | +0.442 | +0.170 | 0 |

The direct candidate therefore fails line coherence and does not qualify as
the implementation-ready solution. Anchors still call the same point
function, but their calculated positions no longer lie on the renderer's
straight projected tube rails, so practical tube/entity registration is
visibly inconsistent.

### 3. Instant neutralization in transitions

Setting `a=0` is projectively coherent in every frame and leaves the existing
base exit/reverse camera untouched. A gate keyed only to `ExitZoom` is
insufficient: an exit-Spike collision changes to `ReverseZoom` while the
camera remains extreme, so the coefficient would turn back on during reverse
and re-amplify. Gating both `ExitZoom` and `ReverseZoom` avoids that, but
still creates state-boundary jumps:

- At entry to ExitZoom, a stable `a=+40` far point drops the `-56.392 px`
  correction immediately.
- At return to normal projection, that correction reappears immediately.
- Successful exit also resets the projection and enters Approach, so
  re-enabling there changes the outgoing/incoming presentation at the
  handoff.
- At the current `a=+5` limit, the corresponding stable far-plane jump is
  `7.049 px`; the task provides no manual acceptance that this discontinuity
  is acceptable. At human-visible probes it would be much larger.

Gating all pretravel states instead only moves the discontinuity to
AvoidSpikes/ExitDelay entry. Line incidence and registration remain coherent
per frame; a pure state gate is deterministic and reversible, but it does not
make the boundary continuous. It alters an existing transition presentation
and is not recommended.

### 4. Fade from an existing transition value

The source has no continuous transition-progress value whose established
meaning is “how much portrait calibration should be active.” K/ZOffset are
discrete camera parameters, not a calibration fade. Player depth advances
differently in the two exit phases, and ReverseZoom also includes death
timing and a depth snap. A fade could be invented as a stateless function of
these values, but choosing its curve and endpoint semantics would create a
second camera/presentation animation without accepted meaning. No fade is
recommended. Any per-frame scalar fade would remain line-coherent, but that
alone does not justify its semantics or boundary timing.

### 5. Refined endpoint-normalized baseline correction

A different, stateless baseline-referenced correction passes the geometric
checks. It uses the stable camera to define the desired near-to-far
calibration displacement, then distributes that correction affinely over
the live camera's reciprocal-depth coordinate. Let `n/f` be the active
near/far world depths (shifted by Approach offset when applicable):

```text
qLive(z) = 1 / (z + ZOffset)
qLiveN   = 1 / (n + ZOffset)
qLiveF   = 1 / (f + ZOffset)
u(z)     = (qLiveN - qLive(z)) / (qLiveN - qLiveF)

q0N      = 1 / (n + 5)
q0F      = 1 / (f + 5)
deltaY   = a * 70 * (q0F - q0N) * u(z)
```

At stable `K=70,ZOffset=5`, this reduces exactly to the current formula at
every depth. At `z=n`, `u=0`; at `z=f`, `u=1`, so the stable far-plane
displacement is preserved exactly. For `z` between the near/far planes,
`u` stays in `[0,1]`; therefore the correction is bounded by the stable
endpoint displacement and does not grow with live K. At stable normal
dimensions, `a=+40` remains `-56.392 host px` at the far plane; at full
Approach it is `-0.983 host px`.

For each fixed model rail vertex, the base coordinates are affine in
`qLive`, and `u(z)` is also affine in `qLive`. Their sum therefore remains
affine in the same projective parameter. The projected rail remains a
straight line; X is unchanged; the portrait camera preserves incidence.
This is a fixed stable-baseline endpoint calibration, not a world-space
height multiplied by the animated camera. The correction distribution at
intermediate depths follows the current projective depth fraction: at
`ZOffset=-41`, for example, the midpoint correction for `a=+40` is about
`-55.177 px`, compared with the stable midpoint's `-50.310 px`, while both
near/far endpoints remain fixed at `0/-56.392 px`. That redistribution is
the coherent way to keep the correction on the live projected segment.

This formulation uses only the current projection values, active level
depths, approach offset, and runtime `a`; it needs no hidden interpolation
state or transition history. K/Z transitions remain untouched. Reconstructing
the same state yields the same projection and backward/forward scrubbing
revisits the same camera values. It is bounded on the in-tube transition
range; outside-tube entity depths are extrapolations and should be covered
in focused tests, but existing retraction/game-over contexts are restored
stable contexts and use the same shared mapping.

The formula must receive consistent near/far references on every production
projection path. The current central projection helper carries a near anchor
but not a far endpoint; the next implementation should pass the active
configuration's near/far values (and Approach offset) through the shared
projection context, including direct `Projection.Project` callers. This is
bounded plumbing within the renderer and does not require redesigning
ExitZoom or ReverseZoom.

This formulation bounds the *added correction*, not the existing base-camera
image. ExitZoom can itself move geometry offscreen as part of its established
zoom. The calculation does not certify that the combined nonzero-coefficient
transition has no clipping, and does not authorize accepting clipping. The
next implementation must distinguish pre-existing base-camera clipping from
any additional coefficient-induced clipping and obtain manual review.

## Line incidence and tube/entity registration summary

| Interpretation | Straight rails? | X / near anchor | Tube-relative entity registration | Deterministic/reversible |
|---|---|---|---|---|
| Current live coupling | Yes: all terms are affine in `qLive` | X unchanged; near correction zero | Same shared mapping | Yes; current K/Z and `a` suffice |
| Direct stable-baseline `q0(z)` added to live base | No when `ZOffset != 5`; rails curve relative to the endpoint chord | X unchanged; near anchored | Point projection remains shared, but anchors can sit off the drawn rail | Yes, but geometrically unsuitable |
| Instant state neutralization | Yes, separately in each frame | X unchanged; correction zero when gated | Same mapping | Yes; boundary discontinuity remains |
| Fade using an arbitrary state-derived scalar | Yes, if one scalar per frame | X unchanged; near anchored | Same mapping | Could be stateless, but no semantic fade variable exists |
| Endpoint-normalized baseline correction | Yes: correction is affine in `qLive` | X unchanged; near zero, far stable-calibrated | Yes when all paths receive the same near/far/approach context | Yes; pure current-state calculation |
| Abandon/nonzero coefficient disabled | Yes, neutral baseline | Unchanged | Unchanged | Yes, but forfeits calibration |

The relevant anchors include Blaster, enemies, Spikers, player shots, enemy
shots, Spikes, and gameplay-space effects. All use either the central
`Projection.Project` routine directly or the renderer's lane/approach helper
that calls it. Existing non-projection glyph scale calculations are separate
from their center anchoring. HUD and screen-space transition stars are not
tube-relative anchors.

## Continuity and playback/reverse-step determinism

The direct baseline formula is continuous in world depth and camera state,
but its mixed denominator causes the rail curvature above. The refined
endpoint-normalized correction equals the existing stable correction at
`K=70,ZOffset=5`; as ZOffset moves away from 5 it changes as a pure function
of live `q` and remains fixed at its defined near/far endpoint corrections.
The first exit step and every reverse step are state-derived with no gate,
hidden history, or mutable interpolation. Returning to the same `(K,ZOffset,
level depths,a)` returns the same correction. The approach offset is likewise
a deterministic property of `LevelApproach` progress.

`GameplayPlaybackSession` replays each input frame through normal
`TempestGame.RunOneStep`; backward visible navigation reconstructs a fresh
session and replays to the target boundary. `Projection.K/ZOffset` are
simulation-driven transition values; the portrait coefficient is not
recorded. Keeping the formula stateless preserves playback/reverse-step
repeatability and requires no new recording field. The option remains a
process presentation value, so the same invocation/configuration must be
used to compare frames. Source locations: `GameplayPlaybackSession.cs:43-88`
and `VisiblePlaybackController.cs:31-53`.

## Transition evidence classification

- **PROJECT DECISION / REFERENCE-DERIVED implementation:** the C# ExitZoom
  phase sequence and ReverseZoom restoration are explicit source behavior.
  The source describes its frame-based transition constants as reference
  values; they are not established here as ROM-backed Atari camera constants.
- **Manually accepted visible behavior, with a boundary:** durable notes
  record manual acceptance of the natural progression presentation and
  observed no ExitZoom handoff discontinuity, projection flash, or Blaster
  jump. They also explicitly say the chosen timing, offsets, and exact
  projection trajectory are not thereby verified as Atari behavior.
- **UNRESOLVED / not an arcade claim:** the precise animated camera trajectory
  and its behavior under nonzero portrait calibration. Existing notes retain
  manual validation needs for cleared-level travel, zoom smoothness/framing,
  Blaster alignment, and projection restoration. The nonzero transition
  interactions analyzed here have no manual acceptance.

Relevant durable records: `TEMPEST-DEVELOPMENT-NOTES.md:738-762,1274-1297`
and `TEMPEST-BEHAVIOR-SPEC.md:340-358`. The later approach acceptance is
qualitative and does not establish exact trajectory values.

Consequently, preserving the base transition implementation constrains this
experiment, but does not make the current coupled correction an Atari
authority. No transition behavior is changed or reopened by this report.

## Command-line implications

The existing `--portrait-perspective <signed-number>` is a singleton,
finite, invariant-culture option, defaults neutral, and currently uses the
`[-5,+5]` range constants. It conflicts with `--legacy-presentation`; reports
record its compatibility with level/lives/speed/rim-speed, recording,
playback, seeded visible playback, and existing scenario selection. It is not
recorded in startup metadata/schema.
Parser locations are `DebugController.cs:90-140,575-590`; range/default
constants are `Projection.cs:17-20,26-38`.

The endpoint-normalized formulation would make human-visible stable-play
probes such as `±40` technically feasible without changing that option's
semantics, recording format, deterministic replay, or legacy conflict. The
prior stable envelope report found `±40` within its production stable-play
far-rim/HUD bounds. This does **not** authorize range expansion: parser
constants and bounds remain unchanged in this task, and nonzero transition
composition still requires focused clipping analysis and Neal's manual
acceptance. A future range change can remain a separate, explicit milestone.

## Decision matrix

| Candidate | Stable usefulness | Transition amplification / clipping | Continuity | Line coherence / registration | Determinism | Scope / effect on existing transition | Evidence |
|---|---|---|---|---|---|---|---|
| Current live K/Z coupling | Useful stable probes | Severe amplification; known high-K clipping risk | Continuous with camera values | Coherent and registered | Strong | No new code; preserves base camera but makes calibration camera-relative | Direct source/math; prior blocked range report |
| Direct stable-baseline q at each depth | Exact stable result | Correction bounded by stable term | Continuous | **Fails** for animated Z; rails curve and point anchors leave chord | Stateless | Small code change but incorrect shared geometry | Algebra and bounded residual table |
| Instant neutralize ExitZoom only | Calibration retained in stable states | Avoids only ExitZoom; reactivates during ReverseZoom | **Jumps** at state entry/exit | Coherent while on/off | Stateless | Changes transition presentation; not adequate for spike reversal | Source state flow and endpoint delta |
| Instant neutralize ExitZoom+ReverseZoom | Calibration retained in stable states | Avoids live amplification in both states | **Jumps** at transitions/handoff | Coherent | Stateless | Changes accepted/unresolved presentation and causes coefficient jump | Source state flow and bounded 7.05/56.39 px jumps |
| Existing-state fade | Potentially useful | Could bound it if chosen | No existing semantically suitable fade | Coherent if scalar per frame | Can be stateless, but curve is invented | Adds second presentation animation; not recommended | Source has no calibration-progress variable |
| Endpoint-normalized baseline correction | Exact same stable image; stable ±40 remains plausible | Added term bounded across in-tube live depth; does not scale with K. Does not certify base-camera clipping | Continuous per state; exact return at baseline | **Passes** algebraically when all paths share context | Pure current-state function | Bounded shared-projection context plumbing; leaves exit/reverse base animation intact | Direct derivation; numerical probes; requires focused tests/manual acceptance |
| Abandon/disable coefficient | No nonzero calibration | None | No transition boundary effect | Coherent | Strong | Simplest, but loses the requested calibration | Neutral baseline known safe |

## Primary diagnostic and recommended next milestone

**READY FOR BASELINE-REFERENCED PERSPECTIVE CORRECTION** — specifically the
endpoint-normalized formulation above. This classification does **not** apply
to directly adding `a*70*(q0(z)-q0(n))` to the live projection; that variant
fails line incidence. The endpoint-normalized alternative is mathematically
coherent, stable-exact, continuous in the live projection state, reversible,
and bounded across the tube near/far interval without changing the existing
exit/reverse camera animation.

Smallest next implementation milestone, if separately authorized: implement
only the endpoint-normalized correction in the existing shared projection
path, with active near/far depths and Approach offset supplied consistently.
Keep the current option range and default unchanged. Add focused tests for
stable exactness, near/far anchors, unchanged X, straight rail incidence at
intermediate depths in stable/early-exit/boundary/high-K/reverse states,
equivalent anchors, approach and out-of-tube depths, and replay reconstruction.
Then perform task-specific manual comparison for stable calibration and
transition compositing. Only after that should a separate range-expansion
milestone decide whether to expose `±40` or another range.

## Validation, final Git state, and closeout

- **Source/math investigation:** COMPLETE. Numerical calculations were bounded
  to the specified normal, exit, reverse, approach, retraction, and Game Over
  contexts.
- **Build/tests/game/manual play:** NOT RUN, as required by the read-only task.
  The next implementation requires focused automated validation and Neal's
  manual acceptance; this report is not an arcade-fidelity acceptance.
- Candidate files were not edited by this investigation; the original nine
  tracked modifications remain intact.
- Final branch: `master`
- Final `HEAD` and `origin/master`: `385f603a94391756b2ca6cccd524dc17b281e782`
- Final index: clean
- Final tracked worktree: the same nine expected modified candidate files
- Final non-ignored untracked files: none
- Final diff summary: unchanged at 9 files, 1,070 insertions, 39 deletions
- Nothing was staged, committed, or pushed.
- No visual assets or prohibited references were inspected.
