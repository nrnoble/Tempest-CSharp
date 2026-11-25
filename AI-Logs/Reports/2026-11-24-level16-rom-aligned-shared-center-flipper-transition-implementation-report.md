# Tempest Level 16 ROM-Aligned Shared-Center Flipper Transition

**Status: IMPLEMENTED; AUTOMATED VALIDATION PASS; NEAL'S MANUAL RETEST
PENDING.** The interactive game was not launched. No commit or push was made.
Level 16 gameplay timing/state, tube geometry, `$24` route and presentation
calibration were not changed.

## 1. Starting repository/Git state

- Branch: `master`
- HEAD: `63d37adb6c0b6b4f28a422f0c6c638cf5391e88c`
- Upstream: `origin/master`
- Index: clean; no staged changes.
- Worktree: 24 tracked files were already modified before this task,
  including Level 16 configuration, zero-area winding, `$24` route, production
  setup/tests, and unrelated Level 15/docs work. Those modifications were
  preserved.
- Pre-existing untracked Level 16 files included the four required prior
  report/prompt pairs, `Level16WaveSelection.cs`,
  `Level16GeometryTests.cs`, and `Level16ProductionTests.cs`. These were
  preserved and not rewritten. Other untracked Level 15 and local reference
  material was also left untouched.
- The candidate diff already represented Level 16 row-11 geometry with
  repeated-vertex winding support, production policy/progression, and `$24`
  ordinary/cargo Flipper routing. It still used the winding-derived spatial
  sweep and required polygon-boundary intersection at every intermediate
  transition progress. The prior runtime report had reproduced the
  lane-0-to-15 production projection exception and made no correction.

## 2. Authoritative evidence used

Read completely before editing:

- `2026-11-24-level16-rom-configuration-investigation-report.md`
- `2026-11-24-level16-bounded-implementation-report.md`
- `2026-11-24-level16-runtime-flipper-pivot-crash-correction-report.md`
- `2026-11-24-level16-shared-center-flipper-transition-investigation-report.md`

The shared-center investigation is authoritative. Its ROM findings were
boundedly checked against the project-local disassembly's `$9d81-$9ef0`
Flipper continuation/start and `get_angle` implementation. No ROM image was
opened or reread, and the completed ROM investigation was not repeated.

Evidence classifications remain distinct:

- **ROM-BACKED:** generic ROM lane/direction/angle-phase rules, Level 16's
  `$0B,$09,$07,$05,$03,$01,$0F,$0D,$0D,$0F,$01,$03,$05,$07,$09,$0B` angle
  row, and its non-boundary-clamped transform reference.
- **Existing PROJECT DECISION/TRANSLATION:** C# endpoint-hinge geometry,
  gameplay timing, Flipper glyph, deterministic simulation and `$24` route.
- **New PROJECT TRANSLATION:** mapping the ROM's directed phase span to
  continuous C# endpoint rotation, retaining the current C# endpoint radius,
  and bypassing polygon-boundary clamping only at qualifying repeated
  vertices.
- **ARCADE-OBSERVED:** no Level 16 transition visual acceptance was performed
  or claimed.

## 3. Files changed

- `src/Tempest.Game/Levels/Level.cs`
- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/Levels/DirectedLaneTransition.cs`
- `src/Tempest.Game/Levels/RomFlipperAnglePath.cs` (new)
- `tests/Tempest.Game.Tests/Level16RomFlipperTransitionTests.cs` (new)
- `TEMPEST-BEHAVIOR-SPEC.md`
- `AI-Logs/Prompts/2026-11-24-level16-rom-aligned-shared-center-flipper-transition-implementation-prompt.md` (new)
- This report (new)

The existing candidate geometry and gameplay code was integrated, not
reverted. No earlier AI report was modified.

## 4. Applicability rule

The ROM angle path is attached only when all of the following hold:

1. The tube is closed.
2. The configuration supplies lane-angle metadata matching its playable lane
   count.
3. The transition hinge is a coordinate repeated at another tube vertex.
4. Both adjacent lane edges meet at that repeated coordinate.

Only Level 16 currently supplies this angle row. The rule is structural and
metadata-based rather than a `DisplayedLevel == 16` rendering branch. Other
Level 16 transitions and configurations without the ROM angle metadata retain
the existing endpoint-hinge and boundary-clamp mechanism, preserving accepted
behavior on Levels 1-15.

## 5. ROM-phase representation

`RomFlipperAnglePath` carries source/destination lanes, ROM direction-bit
semantics, start/target 4-bit phases, directed modulo-16 step direction/count,
and the converted host-space sweep. C# `Direction == +1` maps to the ROM
direction bit clear and increasing phases; `Direction == -1` maps to the bit
set and decreasing phases. The target phase is derived from the destination
lane angle and ROM's opposite-direction `+8 mod 16` rule.

The same stored phase path is validated as part of the directed transition,
so recomputation cannot silently switch the transition's lane, direction or
phase metadata.

## 6. Lane 0 -> lane 15 mapping

- Source lane 0, destination 15; C# direction `+1`; ROM direction bit clear.
- Start phase `$0B`; target phase `($0B + 8) mod 16 = $03`.
- Progression is increasing modulo 16, eight phase steps:
  `$0B,$0C,$0D,$0E,$0F,$00,$01,$02,$03`.
- Y-axis conversion maps increasing ROM phase to negative host rotation:
  **-180 degrees**, opposite the existing C# `+180` spatial sweep.

The phase test also checks the actual halfway endpoint orientation, not only
the final sweep value.

## 7. Lane 15 -> lane 0 mapping

- Source lane 15, destination 0; C# direction `-1`; ROM direction bit set.
- Start phase `($0B + 8) mod 16 = $03`; target phase `$0B`.
- Progression is decreasing modulo 16, eight phase steps.
- Converted host sweep is **+180 degrees**, the reverse of lane 0 -> 15.

## 8. Point-8 junction mappings

Both lanes 7 and 8 have ROM angle `$0D`.

- Lane 7 -> 8: direction `-1`, start `($0D + 8) mod 16 = $05`, target
  `$0D`, decreasing eight steps; host sweep **+180 degrees**.
- Lane 8 -> 7: direction `+1`, start `$0D`, target
  `($0D + 8) mod 16 = $05`, increasing eight steps; host sweep
  **-180 degrees**.

Both paths use the same formula and Level 16 row as the point-0 junction;
neither uses a junction-specific constant.

## 9. C# glyph/render translation

For an intermediate transition progress, C# rotates its existing moving
endpoint about the shared tube vertex using the converted host sweep. Its
existing line-endpoint radius interpolation is retained and stays nonzero for
the tested geometry. The renderer continues using the existing C# six-line
Flipper glyph and scale; no Atari vector-glyph engine or renderer-wide
redesign was added.

The existing C# progress (`TurnAngle / TotalTurnAngle`) maps linearly to the
ROM phase span. This deterministic continuous mapping preserves current C#
gameplay/transition timing. It is not ROM-literal phase-per-frame cadence and
does not claim eight visible frames.

## 10. Polygon-boundary behavior

Qualifying repeated-center transitions do not call
`CalculateFirstPolygonBoundaryDistance`; their intermediate transform is not
constrained to a tube edge. Ordinary transitions still use the existing
polygon-boundary clamp. The ROM investigation establishes no polygon boundary
intersection requirement for the Flipper transform reference.

No exception suppression, catch-and-continue path, fabricated boundary
intersection, alternate polygon intersection, lobe pivot, or
collapse/re-expand behavior was added.

## 11. Gameplay-state preservation

This correction changes transition projection geometry only. It does not
change Level 16 ROM row-11 points/order, 16-lane closed topology, repeated
points 0 and 8, `$24` 2/3 route scheduling, Tanker cargo, lane destination,
Flipper movement/state timing, Spikers, admission/firing, Spike behavior,
identity/ledger behavior, completion, or progression. Level 15 was not used as
an implementation or correction model.

## 12. Exact production crash regression

`TankerCargoRepeating24LaneZeroToFifteenProjectsWithoutBoundaryIntersection`
creates Level 16 Tanker cargo at source lane 0 using the production Tanker
release method, production Flipper factory and Level 16 repeating-`$24`
configuration. Eighteen real Flipper updates reach height 46 and turn angle
144/180 (progress 0.8), reproducing the previously failing state. It checks
the ROM phase path, finite nonzero model endpoints, and finite nonzero
`VectorRenderer.ProjectFlipperEndpoints` output without opening Raylib or
rendering a framebuffer.

## 13. Shared-center transition tests

Four phase/direction cases verify the exact lane 0 -> 15 and reverse mappings,
both directions through lanes 7 <-> 8, modulo step direction/count, converted
host sweep and measured halfway vector orientation.

## 14. Bounded directed-transition coverage

The bounded test exercises all 32 directed adjacent-lane transitions at seven
representative model progress values (`0`, `.125`, `.25`, `.5`, `.75`, `.875`,
`1`), and projects real Repeating24 cargo Flippers through `VectorRenderer` at
the six active-transition samples. All model/projected endpoints remain finite
and nonzero through intermediate progress. Exactly four transitions use ROM
angle paths (both directions at points 0 and 8); the other 28 retain the
existing Level 16 representation.

## 15. Headless production-path coverage

The exact regression combines actual Tanker cargo construction, the Level 16
Repeating24 cargo factory/configuration, production Flipper updates and the
production renderer projection API. It is narrowly scoped to the new
topology/projection risk; no projection requirement was added to ordinary
headless gameplay tests.

## 16. Prior-level regression coverage

Existing directed-rotation, Level 2 canflip/square, Level 3 geometry, Level 4
candidate, Level 7 Atari-presentation and Level 7 production transition suites
passed. No prior-level expectation was changed for this correction.

## 17. Focused test results

All commands used the VSTest/xUnit v2 path with a two-minute hang bound:

1. Exact production crash regression: **PASS — 1 test**.
2. ROM phase/direction cases: **PASS — 4 cases**.
3. Both shared junctions/both directions: **PASS — included in the four
   phase/direction cases above**.
4. Bounded all-directed-transition model and renderer coverage: **PASS — 1
   test**.
5. Level 16 `$24`/Tanker cargo production tests: **PASS — 12 tests**.
6. Level 16 geometry tests: **PASS — 3 tests**.
7. Geometry/Flipper rendering regressions
   (`Level2SquareTests`, `Level3GeometryTests`, `Level4CandidateTests`,
   `VectorRendererAtariPresentationTests`): **PASS — 82 tests**.
8. Accepted prior-level transition regressions: **PASS — 73 tests**.

The first rerun after extending all-transition coverage encountered a test-only
named-argument casing error during compilation. The argument was corrected,
and the all-transition test then completed successfully.

## 18. Full automated suite

**PASS — 2,000 passed, 0 failed, 0 skipped.**
Command: `dotnet test TempestCSharp.slnx --blame-hang-timeout 2min --verbosity minimal`.

## 19. Release build

**PASS — 0 warnings, 0 errors.**
Command: `dotnet build TempestCSharp.slnx --configuration Release --no-restore --verbosity minimal`.

## 20. `git diff --check`

**PASS — exit code 0.** Git emitted existing informational LF-to-CRLF
working-copy warnings for `README.md` and `src/Tempest.Game/Levels/LevelLoader.cs`.

## 21. Visible timing and handoff uncertainty

The ROM P-Code trace does not yield between every angle update, so the
investigation does not establish eight separately rendered Atari frames. The
continuous C# progress-to-phase-span mapping is a **PROJECT TRANSLATION**.
Whether the shared-center path, handedness, perceived cadence and destination
handoff look natural remains for Neal's manual review. No visual match or
manual acceptance is claimed.

## 22. Exact manual retest command

After automated validation, Neal should run:

```powershell
dotnet run --project src\Tempest.Game -- --start-level 16 --lives 12
```

The interactive game was not launched as part of this milestone.

## 23. Final Git/worktree state

- Branch remains `master`; HEAD remains
  `63d37adb6c0b6b4f28a422f0c6c638cf5391e88c`; upstream remains `origin/master`.
- No staged changes. The implementation and AI artifacts remain unstaged.
- Pre-existing unrelated Level 15/docs changes and untracked files remain
  present and untouched.

## 24. Git action confirmation

Nothing was staged, committed or pushed.

## 25. Level 17/recycling confirmation

Level 17 and tube recycling were not implemented or modified.

## 26. Presentation-calibration confirmation

No Level 16 presentation calibration or `--tube-*` default was performed.

## 27. Level 15 model confirmation

Level 15 was not used as the implementation, gameplay or correction model.

## 28. Completion boundary

The deterministic correction and required automated validation are complete.
Level 16 remains unaccepted visually until Neal manually retests using the
command above; stop here for that review.
