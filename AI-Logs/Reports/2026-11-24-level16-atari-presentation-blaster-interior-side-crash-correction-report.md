# Tempest Level 16 Atari-Presentation Blaster Interior-Side Crash Correction

## Status

**CORRECTED; ORDERED AUTOMATED VALIDATION PASS; MANUAL PRESENTATION RETEST
PENDING.** The interactive game was not launched. No staging, commit, or push
was performed. Level 16 geometry, gameplay, Flipper transition, and
calibration values were not changed.

## 1. Starting Git/worktree state

- Branch: `master`
- HEAD: `63d37adb6c0b6b4f28a422f0c6c638cf5391e88c`
- Upstream: `origin/master`
- Index: no staged changes.
- Worktree was already dirty. Tracked edits included the Level 16 production,
geometry, progression, Flipper, presentation-mapping/calibration candidate,
related tests and behavior-spec changes, as well as unrelated Level 15,
README, and AI-Logs work. These were treated as the working baseline and
preserved.
- Relevant pre-existing untracked candidate files included
`Level16WaveSelection.cs`, `RomFlipperAnglePath.cs`,
`Level16AtariPresentationTests.cs`, and the Level 16 geometry, production, and
ROM-Flipper-transition tests, together with the six required prior report and
prompt pairs. Unrelated Level 15 AI-Logs and local reference material were
left untouched.
- The candidate already had row-11 closed geometry with repeated points 0/8,
the Level 16 production policy, source-lobe winding for geometry pivots,
`$24` Flipper routing, ROM-aligned shared-center Flipper projection,
Level 16 Atari model-frame mapping, neutral calibration and the four existing
controls. `VectorRenderer.cs` had no starting diff for this Blaster issue.
- The candidate's unstaged state and relevant paths were recorded before
editing. No unrelated changes were reset, cleaned, staged, or reverted.

## 2. Manual failure evidence

**USER-PROVIDED MANUAL EVIDENCE:** Neal launched:

```powershell
dotnet run --project src\Tempest.Game -- --start-level 16 --atari-presentation
```

Startup reported `DEVELOPMENT ONLY — Atari presentation candidate enabled for
production Levels 1-16.` The application initialized, then threw:

```text
System.InvalidOperationException: The Blaster rim edge has no determinable interior side from its presentation rails.
```

The supplied stack was `VectorRenderer.GetOutwardBlasterClawOffset` →
`DrawPlayer` → `DrawPlayer` → `Draw` → `Program.<Main>`. This establishes that
the Level 16 model-frame mapping resolved and the crash was in Blaster
presentation geometry, not the previous mapping exception or the prior
shared-center Flipper pivot crash.

## 3. Deterministic production-path reproduction

Before correction, a temporary headless sweep directly called production
`VectorRenderer.DrawPlayer` for each of the 16 production Level 16 lanes with
Atari presentation enabled and `LevelState.Playing`. It used the real
row-11 points, projected `DrawTube` rails, and the initial rim position
(lane 7, edge position 0.5). The Blaster orientation method was reached.
The diagnostic collected both rail-side candidates and caught exceptions
only in test code so the sweep could continue; production did not catch or
suppress them.

The initial lane 7 call reproduced the exact supplied
`InvalidOperationException`. The permanent
`DirectLevelSixteenAtariBlasterRendersOnItsInitialLane` regression was also
run against the pre-correction rail-only method: it failed with that same
exception through `DrawPlayer` and `GetOutwardBlasterClawOffset` (**expected
red reproduction**). After the correction, the same test passed. The
temporary diagnostic sweep was removed; permanent tests assert finite,
nonzero, outward claw geometry rather than checking exception text.

## 4. Failing lanes and measured geometry

The pre-correction neutral sweep found exactly four failures: lanes **0, 7,
8, and 15**. They are the four edges incident to repeated points 0 and 8.
The other twelve lanes (1–6 and 9–14) succeeded; their two rail-side dot
candidates agreed in sign, with the smallest absolute averaged side about
62.8384, well above the `1e-4` threshold.

All coordinates below are headless projected coordinates under neutral
Level 16 presentation. `R0` and `R1` are the corresponding near-to-far rail
vectors; `d0/d1 = dot(leftNormal, R0/R1)`, and `c0/c1 = cross(tangent, R0/R1)`.
The original rule used `(d0+d1)/2` and rejected absolute values at or below
`1e-4`.

| Lane → next | Near edge start → end | Far edge start → end | R0, R1 | d0, d1; c0, c1; average |
|---|---|---|---|---|
| 0 → 1 | (400,300) → (363.63635,393.18182) | (400,300) → (395.5793,311.32797) | (0,0); (31.942963,-81.85385) | 0, 0.0000114441; 0, 0.00122070; 0.00000572205 |
| 7 → 8 | (363.63635,206.81818) → (400,300) | (395.5793,288.67203) → (400,300) | (31.942963,81.85385); (0,0) | 0.0000114441, 0; 0.00122070, 0; 0.00000572205 |
| 8 → 9 | (400,300) → (436.36365,393.18182) | (400,300) → (404.4207,311.32797) | (0,0); (-31.942963,-81.85385) | 0, -0.0000114441; 0, -0.00122070; -0.00000572205 |
| 15 → 0 | (436.36365,206.81818) → (400,300) | (404.4207,288.67203) → (400,300) | (-31.942963,81.85385); (0,0) | -0.0000114441, 0; -0.00122070, 0; -0.00000572205 |

The pre-orientation claw offsets at lanes 0, 7, 8, and 15 were respectively
`(-18.631552,-7.270851)`, `(-18.631552,7.270851)`,
`(-18.631552,7.270851)`, and `(-18.631552,-7.270851)`. At the startup lane
7, the current offset was `(-18.631552,7.270851)`. After the local outward
side is selected, the lane-7 offset is reversed to approximately
`(18.631552,-7.270851)`, placing the outer claw point near
`(400.4497,246.1382)` from center `(381.8182,253.4091)`.

In every failing lane, one rail vector is zero at the shared center and the
other is collinear with the near-rim edge. Both side candidates are therefore
numerically indeterminate (near-zero dot and cross), not two valid candidates
that both classify as inside or outside. Each failing lane is adjacent to a
repeated center coordinate.

## 5. Geometric cause and selected local rule

The rail-average method works for the prior simple/open tubes, but assumes a
single interior side is available from the projected rails for every edge.
At Level 16's two self-touching lobes, the center-to-neighbor rail lies along
the same line as its near-rim edge, so neither rail supplies a signed
interior-side component. A global shoelace sign is also unsuitable: the
ordered lobes have opposite winding and cancel to zero total area.

**PROJECT TRANSLATION:** For a closed walk whose point count matches its
playable lanes and which has exactly one nonadjacent repeated vertex pair,
derive the source lane's local lobe winding from the actual projected
near-rim points. Positive projected lobe area puts that lobe's interior to
the left of its ordered edge, so the outward normal is the opposite normal;
negative area selects the left normal as outward. The two measured Level 16
projected lobe twice-areas are approximately `+94545.44` and `-94545.43`.
This structurally selected rule applies to the whole two-lobed outline,
including rail-classifiable lanes, so calibration changes cannot flip a
determinate rail average against the local lobe. It uses no displayed-level
branch, screen quadrant, or hard-coded lane sign/list.

For ordinary simple and open tubes, the existing projected-rail rule remains
unchanged. If neither a supported local loop nor the rails determine a side,
the existing fail-fast behavior remains. No exception was suppressed and no
player or Blaster drawing was skipped.

## 6. Files changed

- `src/Tempest.Game/Rendering/VectorRenderer.cs`
  - Adds a headless callback overload for drawing Flippers and applies
    topology-based local winding for a unique repeated-vertex closed walk;
    ordinary rail orientation and invalid-geometry failure remain.
- `src/Tempest.Game/Levels/Level.cs`
  - Makes the existing source-lane lobe-winding helper `internal` so the
    renderer reuses the established calculation. No geometry or gameplay
    behavior changed.
- `tests/Tempest.Game.Tests/Level16AtariPresentationTests.cs`
  - Adds exact crash, 16-lane/nearby-offset, calibration-control, and complete
    callback draw-path coverage.
- `tests/Tempest.Game.Tests/BlasterPresentationTests.cs`
  - Extends prior-level all-lane and direction-reversal coverage through
    Levels 11–15.
- `TEMPEST-BEHAVIOR-SPEC.md`
  - Records the new render-only local-lobe orientation as a project
    translation, not arcade acceptance.
- This prompt and report under `AI-Logs/`.

No Level 16 ROM points, lane ordering, topology, model mapping, Flipper
transition, calibration default, control, gameplay state, or prior-level
presentation was changed.

## 7. Permanent regression and full bounded draw-path coverage

- The exact direct-start regression reaches the complete `DrawPlayer` path,
  checks the initial lane 7 Blaster, six finite lines, nonzero claw output,
  and the geometrically justified outward side.
- The all-lane test checks all 16 lanes at offsets 0.1, 0.5, and 0.9 (48
  samples), confirms opposite local lobe winding, and explicitly verifies
  lanes 0, 7, 8, and 15 as repeated-center-adjacent lanes.
- Each non-neutral control test checks all lanes at the same three offsets:
  Y gain 1.25, scale 0.8, vertical offset 23, and depth-Y gain 0.5. These are
  test inputs only; no calibration was selected.
- The callback draw-path test exercises the 48 tube lines, Blaster,
  production Flipper body in a shared-center `$24` transition, Tanker,
  Fuseball, Egg, Spike, player shot, and enemy bullet. It checks nonempty
  geometry, expected tube/player/Flipper counts, and finite coordinates
  without opening Raylib or a framebuffer.
- Existing prior-level Blaster orientation tests now include every production
  Level 1–15 lane, both projected winding orientations, and direction
  reversal. Representative simple closed and open tubes retain their
  expected outward orientation.
- A prior all-level control attempt with only an ambiguity fallback exposed
  that a non-neutral depth-Y projection can make a rail-derived side disagree
  with the source lobe. The implemented topology rule therefore consistently
  uses local lobe winding for the qualifying self-touching walk; the final
  four-control suite passes.

## 8. Validation results

The test project uses .NET SDK `9.0.304`, `net9.0`, xUnit 2/VSTest.
Every test command used `--blame-hang-timeout 2min`. Validation was run in the
requested order:

1. Exact Blaster crash regression: **PASS — 1 test**. Pre-correction run:
   **FAIL as expected**, reproducing the `InvalidOperationException`.
2. All 16 lanes and nearby offsets: **PASS — 1 test, 48 lane/offset cases**.
3. Non-neutral controls: **PASS — 4 tests, 192 lane/offset cases**.
4. Bounded complete Level 16 draw path: **PASS — 1 test**.
5. Level 16 Flipper/entity presentation regressions: **PASS — 19 tests**.
6. Levels 1–15 Blaster/presentation regressions: **PASS — 142 tests**.
7. Level 16 gameplay/startup regressions: **PASS — 121 tests**.
8. Full solution suite: **PASS — 2,029 passed, 0 failed, 0 skipped**.
   Command: `dotnet test TempestCSharp.slnx --blame-hang-timeout 2min --verbosity minimal`.
9. Release build: **PASS — 0 warnings, 0 errors**.
   Command: `dotnet build TempestCSharp.slnx --configuration Release --no-restore --verbosity minimal`.
10. `git diff --check`: **PASS — exit code 0**. Git emitted informational
    existing LF-to-CRLF working-copy warnings for `README.md` and
    `src/Tempest.Game/Levels/LevelLoader.cs`.

## 9. Remaining visual acceptance

Automated geometry confirms a unique finite outward side; it does not
establish arcade visual fidelity. The appearance of the player/claws,
Level 16 tube placement, scale, Y gain, vertical offset, and depth-Y gain
remain for Neal's manual review. No visual match, final calibration, or
manual acceptance is claimed.

After automated validation, Neal should run exactly:

```powershell
dotnet run --project src\Tempest.Game -- --start-level 16 --atari-presentation
```

The interactive game was not launched by this work.

## 10. Final Git state and scope confirmations

- Branch remains `master`; HEAD remains
  `63d37adb6c0b6b4f28a422f0c6c638cf5391e88c`; upstream remains `origin/master`.
- The existing dirty worktree was preserved. This task's source/test/doc
  changes and AI-Logs remain unstaged; the index is empty.
- Nothing was staged, committed, or pushed.
- Level 17 and later were not implemented or investigated.
- No final Level 16 calibration was encoded; neutral defaults and all four
  controls remain intact.
- Level 15 was not used as the correction model.
- Level 16 geometry, Flipper correction, gameplay, player lane/movement, and
  collision state remain unchanged.
