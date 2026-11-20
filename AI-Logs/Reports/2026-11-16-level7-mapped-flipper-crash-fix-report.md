# Level 7 mapped Flipper transition crash correction

## Status

**Automated validation: PASS. Manual acceptance: PENDING.** The candidate is
ready for Neal's requested interactive retest. No interactive game was launched.

## Root cause and reproduction

The exception text was used to locate
`AtariLevel7EntityPresentation.MapFlipperTransition` and its former
`CalculateFirstPolygonBoundaryDistance` call. The exception did not contain the
live Flipper fields, so the exact runtime `TurnAngle`/progress could not be
recovered. A bounded enumeration of Level 7 production transitions reproduced
the same exception with a valid `Level.CalculateDirectedLaneTransition`:

- source lane 1, destination lane 2, direction -1;
- hinge vertex 2, source-free vertex 1, destination-free vertex 3,
  `HingeIsStart = true`;
- gameplay spatial sweep -307.893 degrees;
- progress 0.75 and normalized depth 0 (near cross-section).

This is production-reachable transition geometry. The regression test now
covers all 16 source lanes, both production-legal directions, representative
progress values, every per-tick progress for both configured and rim turn
speeds, and normalized depths from 0 through 2.

At the reproduced near cross-section, the mapped pivot was
`(571.4662, 416.43542)`, source-free point `(555.28064, 266.3604)`, and
destination-free point `(477.3874, 478.19043)`. The polygon winding was
positive (twice-area `249452.625`). The mapped local hinge is convex, with a
short endpoint-ray sweep of -117.126 degrees, while the gameplay hinge selects
a long reflex sweep of 307.893 degrees. Applying the gameplay angle directly
to the mapped source ray therefore aimed outside the mapped polygon.

At progress 0.75, the old ray was `(0.8393856, 0.5435363)`. Edges 1 and 2 met
only at the origin (`t = 0`). Positive-distance candidates on edges 4, 5, and
8 had segment parameters `u = -0.7693`, `-3.1838`, and `-9.8057` respectively,
so each was correctly rejected as outside its segment. The other candidates
were behind the ray or parallel. This was not an endpoint tolerance or
near-parallel failure: the whole-polygon clipping assumption was incompatible
with the mapped hinge's local geometry.

## Correction

`AtariLevel7EntityPresentation` now derives the angular sweep from the mapped
source-free and destination-free rays about the mapped hinge. The route's
long/short choice still comes from the existing gameplay transition; for the
reproduced case, the mapped complementary long sweep is +242.874 degrees.
Radius is interpolated between the mapped endpoint distances, so both progress
endpoints agree exactly with their mapped lanes and interior samples remain
continuous. This avoids clipping a valid directed path against an outline
whose local convexity differs from gameplay. The endpoint remains anchored at
the mapped hinge; no gameplay or Flipper state is written.

Representative geometry after the change: at progress 0.75 and depth 0 the
moving endpoint is approximately `(579.9863, 538.2767)` on the mapped long
pivot arc; at progress 1 it reaches the mapped destination-free point
`(477.3874, 478.19043)`. Interior long-route samples can lie outside the mapped
cross-section because the corresponding gameplay reflex hinge maps to a convex
record-06 hinge. The implementation preserves that directed long route rather
than silently taking the short route or inventing a polygon boundary endpoint.

Malformed transition metadata still fails explicitly. Record-06 data and
selection, point order, lane/depth registration, centering, the projector,
non-Flipper entities, simulation logic, and `VectorRenderer` were not changed.
Ordinary and Tanker-released cargo Flippers exercise the shared renderer
projection path. Tests also verify endpoint finiteness, mapped anchor/radius
geometry, lane endpoint registration, route distinction, tangent/normal
coherence, continuity, state preservation, and malformed-state rejection.

## Files changed

- `src/Tempest.Game/Rendering/AtariLevel7EntityPresentation.cs` — mapped
  transition geometry and explicit transition validation.
- `tests/Tempest.Game.Tests/AtariLevel7EntityPresentationTests.cs` — exact
  regression, all-lane/direction/depth/tick-progress coverage, ordinary/cargo
  render-only checks, and malformed-state test.
- `AI-Logs/Prompts/2026-11-16-level7-mapped-flipper-crash-fix-prompt.md` — saved
  substantive user prompt.
- This paired report.

## Validation

- Focused Level 7 presentation and Flipper tests: **PASS**, 24 passed.
- Targeted Level 7, Flipper, directed-transition, presentation, recording/
  playback, transition, exit, and Game Over regressions: **PASS**, 525 passed.
- Full solution VSTest suite with `--blame-hang-timeout 5min`: **PASS**, 1,578
  passed, 0 failed, 0 skipped.
- Release solution build: **PASS**, 0 warnings, 0 errors.
- `git diff --check`: **PASS**. Git emitted only existing line-ending
  conversion warnings for unrelated `LevelLoader.cs` and `InputSystem.cs`.

The same Flipper snapshot (lane, direction, height, pivot progress, active
transition, cargo identity, pause/collision/fire state) and gameplay point set
were unchanged across rendering in the regression test. Recording/playback
tests passed, and no gameplay, simulation, timing, recording, or playback code
was modified. Legacy, option-off, other-level, transition, approach/exit, and
Game Over paths were included in the targeted/full regression runs. The
separate Blaster-exit defect remains out of scope.

## Git state

At start and finish: branch `master`, HEAD
`385f603a94391756b2ca6cccd524dc17b281e782`, upstream `origin/master`.
There were no staged changes. The pre-existing 17 modified tracked paths and
17 untracked paths were preserved; the two source/test candidate files listed
above were already untracked at task start and were edited in place. No other
tracked or untracked project files were staged, committed, or pushed. AI-Logs
remain ignored.

## Manual handoff

Neal's manual acceptance remains **PENDING**. Retest with:

```powershell
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation --level 7
```

Confirm only that ordinary and Tanker-released Flipper transitions no longer
crash, Flippers remain visually registered to the visible tube, gameplay
remains unchanged, and tube centering/alignment improvements remain intact.
This manual acceptance is still required.

Prompt: `AI-Logs/Prompts/2026-11-16-level7-mapped-flipper-crash-fix-prompt.md`

Completion token: `TEMPEST_LEVEL7_MAPPED_FLIPPER_TRANSITION_CRASH_FIX`
