# Rim Flipper right-end oscillation — read-only investigation

Date: 2026-11-23

## Outcome

**Classification: OPEN-TUBE ENDPOINT DEFECT.**

**Implementation readiness: READY FOR BOUNDED CORRECTION.**

The right-end oscillation is reproduced in the current Level 10 diagnostic
geometry. At an open endpoint, `GetDirectedNeighborLane` reverses the live
`Direction` to take the only available inward lane but does not update
`RimPursuitDirection`. At the next rim pause expiry, `BeginPivot` copies the
stale pursuit latch back into `Direction`, selecting the endpoint lane again.
This creates the observed one-lane oscillation.

The same trace at the left endpoint reproduces the same behavior. Level 12's
closed topology does not take this fallback: it wraps from lane 15 to lane 0
while retaining both direction fields. Its rightmost geometry bends toward
lower X after its maximum-X lanes, which can look like leftward travel, but the
state machine does not reverse there or return right after one lane.

## Starting state and preservation

- Branch: `master`; HEAD: `aa6b7110fe845cf36173a80b7dc5d11c1f067806`,
  tracking `origin/master`.
- The worktree already contained extensive unrelated local edits and the
  uncommitted Level 12 candidate, including changes in `Flipper.cs`. They were
  preserved. The current `Flipper.cs` diff adds Level 12 below-rim route
  initialization; it does not change `BeginPivot` or
  `GetDirectedNeighborLane`.
- A temporary deterministic test was added only to capture the requested
  state trace, then removed. No source, test, configuration, or durable
  documentation changes remain. Only this report and its paired prompt record
  were added under `AI-Logs/`; no files were staged or committed.
- No interactive game, ROM binary, screenshot, video, `TEMP/`, or `UserData/`
  material was inspected.

## 1. Level 10 right-end trace

The only current Level 10 setup is `Level10Diagnostic`: genuine open Level 10
geometry with Level 7 diagnostic gameplay policy, not an arcade-authentic
Level 10 implementation. It has 15 playable lanes, indexed 0–14; lane 14 is
the right endpoint and lane 0 the left endpoint. This trace seeds a rim
Flipper at lane 12 and fixes the player at lane 14, deterministically selecting
`RimPursuitDirection = -1`.

The pause is 23 ticks. Each Level 10 pivot in this trace takes 8 simulation
steps (180 degrees at the configured rim rate).

| Start tick | Completed tick | Lane transition | Direction / pursuit latch | Endpoint at source | Next-lane selection |
|---:|---:|---|---|---|---|
| 24 | 32 | 12 → 13 | -1 / -1 | neither | Direction -1 requests index +1; lane 13 exists |
| 55 | 63 | 13 → 14 | -1 / -1 | neither | Direction -1 requests index +1; lane 14 exists; arrival is right endpoint |
| 86 | 94 | 14 → 13 | +1 / -1 | right | Latched -1 requests lane 15, which does not exist. Fallback reverses `Direction` to +1 and selects lane 13 |
| 117 | 125 | 13 → 14 | -1 / -1 | neither | `BeginPivot` restores -1 from the latch; lane 14 exists, so it returns to the endpoint |

At tick 86 the expected endpoint reversal itself occurs: the actual move is
inward, lane 14 to 13, with `Direction = +1`. The first inconsistent state is
that `RimPursuitDirection` remains -1. At tick 117 the next pivot restores
that stale value, causing the second reversal and the oscillation.

State at each pivot start was `Flipping`, with the current lane as source and
the selected adjacent lane as destination. The recorded directed transition
completed after the stated 8 steps, then `CompletePivot` moved the Flipper to
the destination and entered the 23-tick pause. The endpoint predicate at
arrival was `IsOpenEndpoint(14) == true`; at lane 13 it was false. There is no
separate left/right reversal predicate in `Flipper`: the fallback is triggered
when `TryGetAdjacentLane` fails.

The relevant current-source branches are `Flipper.BeginPivot` (rim-latch copy
and neighbor selection), `Flipper.GetDirectedNeighborLane` (failed-neighbor
direction reversal), `Level.IsOpenEndpoint`, and `Level.TryGetAdjacentLane`.
The latter wraps only when `Continuous` is true.

## 2. Level 10 left-end control

The equivalent control starts at lane 2 with player lane 0, selecting
`RimPursuitDirection = +1`. It reproduces the same state sequence mirrored:

| Start tick | Completed tick | Lane transition | Direction / pursuit latch | Endpoint at source | Next-lane selection |
|---:|---:|---|---|---|---|
| 24 | 32 | 2 → 1 | +1 / +1 | neither | Direction +1 requests index -1; lane 1 exists |
| 55 | 63 | 1 → 0 | +1 / +1 | neither | Direction +1 requests index -1; lane 0 exists; arrival is left endpoint |
| 86 | 94 | 0 → 1 | -1 / +1 | left | Latched +1 requests lane -1, which does not exist. Fallback reverses `Direction` to -1 and selects lane 1 |
| 117 | 125 | 1 → 0 | +1 / +1 | neither | `BeginPivot` restores +1 from the latch; lane 0 exists, so it returns to the endpoint |

Therefore the left endpoint does not behave differently under equivalent
deterministic conditions. The reported lack of a left-end occurrence was not
reproduced as a code asymmetry; both endpoints use the same failed-neighbor
fallback.

## 3. Level 12 comparison

Level 12 is modeled as a continuous, closed tube with 16 playable lanes
(indices 0–15). Starting at lane 13 with player lane 15 selects direction -1:

| Start tick | Completed tick | Lane transition | Direction / pursuit latch | Endpoint at source | Next-lane selection |
|---:|---:|---|---|---|---|
| 24 | 32 | 13 → 14 | -1 / -1 | none | Adjacent lane 14 exists |
| 55 | 62 | 14 → 15 | -1 / -1 | none | Adjacent lane 15 exists |
| 85 | 91 | 15 → 0 | -1 / -1 | none | Closed-tube adjacency wraps to lane 0 |
| 114 | 120 | 0 → 1 | -1 / -1 | none | Adjacent lane 1 exists |

At lane 15, `IsOpenEndpoint` is false and `TryGetAdjacentLane(15, +1, ...)`
returns lane 0. No fallback or direction reversal occurs. Level 12's
`Level12ConstantFlip` program is a below-rim route that hands off to the shared
rim state machine; it does not change the endpoint condition after handoff.
Each trace starts in `Flipping` at zero turn angle and completes into
`Pausing`; the four directed sweeps are approximately 162.582, 156.251,
117.986, and 116.382 degrees, taking 8, 7, 6, and 6 steps respectively.

“Far right” on Level 12 is a geometry position, not a state-machine endpoint.
The model X coordinates for lanes 13, 14, 15, and 0 are approximately
171.4, 200, 200, and 160.7. Thus the closed outline can move screen-left after
the maximum-X lanes while the lane index continues forward through 15 → 0
and both direction fields remain -1. This is curvature around the closed
outline, not the reproduced open-end bounce. The deterministic rim trace does
not reproduce a second Level 12 state reversal; if Neal's reported motion is a
literal right-then-left-then-right sequence there, it remains a separate
visual/state discrepancy not explained by this endpoint branch.

## 4. First incorrect transition and authoritative state

In `BeginPivot`, the first rim pivot initializes `RimPursuitDirection` and
copies it into `Direction`. `GetDirectedNeighborLane` then maps direction to
index delta (`+1` means index -1; `-1` means index +1). When that requested
lane is absent on an open tube, it reverses the by-reference `Direction` and
uses the opposite adjacent lane. It does not synchronize the latched pursuit
direction.

Consequently:

1. The endpoint fallback's `Direction` is authoritative for the immediate
   inward pivot.
2. The unchanged `RimPursuitDirection` becomes authoritative again at the
   next `BeginPivot` and overwrites that endpoint-adjusted direction.
3. This is not a stale source/destination or transition-completion problem.
   `CompletePivot` advances `EdgeIndex`, and `BeginPause` resets the transition
   fields to the current lane before the next pivot.

There is no `<`/`<=` or last-lane/last-vertex comparison error in the
reproduction. `IsOpenEndpoint` correctly identifies lane 0 and
`PlayableLaneCount - 1`; the problematic branch is the direction/latch
desynchronization around the failed adjacency.

## 5. Geometry and direction mapping

Both geometries report `ClockwiseLaneIndexDelta = -1`. Under the project's
screen-space winding convention, increasing lane order is counter-clockwise
and decreasing lane order is clockwise. For this Flipper state machine,
`Direction = -1` increases lane index; `Direction = +1` decreases it.
Clockwise/counter-clockwise describes the signed point ordering on the closed
Level 12 contour. For open Level 10, the implicit closing chord supplies only
an orientation sign; it is not a traversable rim lane.

Level 10's right endpoint (lane 14) has positive/right-side X and its left
endpoint (lane 0) negative/left-side X. Moving toward the right endpoint is
increasing index (`Direction = -1`); after the expected endpoint reversal,
moving left/inward is decreasing index (`Direction = +1`). On Level 12,
increasing indices proceed around the closed outline and wrap 15 → 0; a
screen-X change at the rightmost bend does not imply a change in `Direction`.
No player-spinner behavior was examined.

## 6. Historical evidence and baseline

- **ARCADE-OBSERVED / accepted project behavior:** the historical rim-latch
  record documents Neal's accepted direction persistence on closed-rim Levels
  1–4. That acceptance does not establish a universal open-end rule.
- **REFERENCE-DERIVED:** the same historical record describes Incitti's
  one-direction Flipper state and reversal at open-board endpoints. This is
  secondary reference behavior, not ROM proof.
- **PROJECT TRANSLATION:** C# stores a separate `RimPursuitDirection` to
  preserve the accepted closed-rim behavior. The historical notes explicitly
  leave open-tube pursuit qualified rather than generalizing that closed-rim
  latch.
- **Current implementation:** the open topology correctly has no neighbor
  beyond its endpoints and the first forced reversal selects the inward lane;
  the missing synchronization causes the subsequent return.
- **Baseline:** the endpoint fallback and rim-direction latch are present in
  committed HEAD `aa6b7110`. The current uncommitted Level 12 route additions
  do not alter them. Thus Level 10 diagnostic behavior exists in the committed
  baseline and was not introduced by the current Level 12 candidate. This
  establishes presence in the baseline, not the exact commit/date that first
  introduced the defect.

## 7. Scope classification and correction readiness

**OPEN-TUBE ENDPOINT DEFECT**

**READY FOR BOUNDED CORRECTION**

Candidate correction, not implemented: when an open-tube endpoint fallback
reverses the actual rim movement, synchronize the pursuit latch to that
post-fallback direction. Preserve this invariant: pursuit direction stays
latched through player crossovers and changes only when an open endpoint has
no neighbor in the requested direction; after the reversal, movement continues
away from that endpoint.

Focused regressions should cover:

- Level 10 diagnostic right endpoint: continue from 14 → 13 to 12, rather than
  returning 13 → 14; mirror the same behavior at the left endpoint.
- A closed Level 12 control traversing 15 → 0 with `Direction` and
  `RimPursuitDirection` unchanged.
- Existing closed-rim player-crossover tests, to preserve the accepted latch.

The correction should affect the shared rim-pursuit path only when an open
endpoint has no requested neighbor (including other open-tube levels using
that path). It should not change closed Levels 1–7 or Level 12, Level 12's
below-rim constant-flip program, geometry, enemy selection, or spinner input.

## Validation and remaining acceptance

- Temporary deterministic Level 10 right/left and Level 12 trace: **PASS**;
  1 diagnostic test completed, and the temporary test file was removed.
- Existing `RimPursuitTests`: **PASS**, 19 passed, 0 failed, 0 skipped.
- No production correction was implemented, and no interactive game was
  launched. Neal's supplied observation remains the visual evidence; the
  Level 12 literal second reversal was not reproduced in the closed-rim
  state trace.
