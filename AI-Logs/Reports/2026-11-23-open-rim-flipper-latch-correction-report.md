# Open Rim Flipper Pursuit-Latch Correction Report

Date: 2026-11-23

## Outcome

**Status: IMPLEMENTATION AND AUTOMATED VALIDATION PASS**

The Flipper now reports an open-end fallback from neighbor selection and
synchronizes `RimPursuitDirection` to the post-fallback `Direction` only in
rim-pursuit `BeginPivot`. The synchronized latch makes a Flipper continue away
from the open endpoint while preserving the existing pursuit semantics.
Below-rim programmed neighbor selection does not synchronize the latch.

Manual gameplay acceptance remains pending for Level 10 and Level 12.

## Changed files

- `src/Tempest.Game/Gameplay/Flipper.cs`
  - `GetDirectedNeighborLane` now reports `openEndpointFallback`.
  - `BeginPivot` updates `RimPursuitDirection` after an actual open-end
    reversal.
  - The existing programmed-route call remains separate and ignores the
    fallback report.
- `tests/Tempest.Game.Tests/RimPursuitTests.cs`
  - Added open Level 10 right-end and left-end continuation regressions.
  - Added a closed Level 12 wrap/pursuit-latch control.

No existing Fuseball, HUD, Blaster-death, spinner, playback, ExitZoom,
Level 12 geometry, or presentation work was modified.

## Git-history finding

### Relevant commits

- **`7201e87eb7ca7ca1b139e71a162ce499e55e9f62`**
  - Title: `Correct arcade rim pursuit behavior`
  - Date: 2026-11-02
  - Introduced `RimPursuitDirection` and began copying it into `Direction`
    for rim pursuit.
- **`d5ccbe1d3ef8af21a02bd53c8c465ed0c4908fef`**
  - Title: `Close out accepted production Level 8`
  - Date: 2026-11-08
  - Introduced `GetDirectedNeighborLane` and replaced direct
    `PreviousEdge`/`NextEdge` selection in `BeginPivot`.
  - This helper reverses `Direction` when the requested open-tube neighbor
    does not exist but does not synchronize `RimPursuitDirection`.

Git ancestry confirms `7201e87e` predates `d5ccbe1d`, and `7201e87e` is
an ancestor of the baseline `aa6b7110fe845cf36173a80b7dc5d11c1f067806`.
The parent of `d5ccbe1d` uses the latched rim direction with direct edge
selection and does not contain `GetDirectedNeighborLane`; after
`d5ccbe1d`, both the latch and failed-neighbor reversal coexist.

### Historical classification

**INTRODUCED BY IDENTIFIED COMMIT**

The exact stale-latch combination is first identifiable in
`d5ccbe1d`, where open-end fallback reversal was added after
`RimPursuitDirection` had already been introduced in `7201e87e`. The earlier
commit is latch introduction, not the exact oscillation mechanism.

## Automated validation

- Focused `RimPursuitTests`: **PASS**, 22/22.
- Level 12 production/topology filter: **PASS**, 9/9.
- Flipper below-rim, Tanker-cargo, and Level 11 filter: **PASS**, 67/67.
- Levels 1-11 gameplay filter: **PASS**, 577/577.
- Complete solution suite: **PASS**, 1828/1828.
- Release solution build: **PASS**, 0 warnings and 0 errors.
- `git diff --check`: **PASS**.

The Level 12 deterministic control and Level 12 production soak in the existing
test suite remain passing.

## Manual handoff

Observe the open Level 10 diagnostic:

```powershell
dotnet run --project src\Tempest.Game -- --level 10
```

Verify a rim Flipper at either open endpoint reverses once and continues away
from the endpoint for more than one lane.

Observe the closed Level 12 production tube:

```powershell
dotnet run --project src\Tempest.Game -- --level 12
```

Verify normal closed-rim travel remains coherent and no new endpoint behavior
appears. If the earlier visible right-side observation persists, it remains
separate from this state-machine correction.

No interactive game was launched.

TEMPEST_OPEN_RIM_FLIPPER_LATCH_CORRECTION
