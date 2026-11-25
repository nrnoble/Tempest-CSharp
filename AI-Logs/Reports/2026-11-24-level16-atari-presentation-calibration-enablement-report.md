# Tempest Level 16 Atari-Presentation Calibration Enablement

## Status

**IMPLEMENTED; AUTOMATED VALIDATION PASS; MANUAL CALIBRATION PENDING.** The
Level 16 Atari-presentation candidate now uses the shared projection path and
the four existing calibration controls. Initial values are neutral and are
not accepted settings. No interactive game or MAME session was launched.
Nothing was staged, committed, or pushed.

## 1. Starting Git/worktree state

- Branch: `master`
- HEAD: `63d37adb6c0b6b4f28a422f0c6c638cf5391e88c`
- Upstream: `origin/master`
- Index: clean; no staged changes.
- The worktree already had 25 modified tracked paths and 26 untracked paths.
  Existing edits included the Level 16 row-11 production candidate, wave and
  progression code, raw ROM presentation metadata, geometry/winding and
  shared-center Flipper translation, associated tests and documentation, plus
  unrelated Level 15 edits.
- The relevant uncommitted candidate diff already contained Level 16's closed
  16-point row-11 geometry and metadata (`AtariTubeRecordCatalog`,
  `RomPresentationMetadata`, `LevelLoader`), production configuration and
  prior-level regression updates. It did not contain a Level 16 entry in
  `AtariPresentationAdapter.HasModelFrameMapping` /
  `GetModelFrameMapping`, a Level 16 calibration-table entry, or an updated
  presentation startup range. Those existing candidate changes were treated
  as the working baseline and preserved.
- Existing untracked Level 16 source/tests, prior AI-Logs, Level 15 material,
  and `Reference/arcade/MBUCOD.V05.txt` were preserved. No unrelated files
  were cleaned, reset, reverted, staged, or otherwise changed.

## 2. Manual failure evidence

Neal reported successful normal Level 16 gameplay and a good-looking static
tube, but immediate failure when launching with
`--start-level 16 --lives 12 --atari-presentation`. The reported console
message said support ended at production Level 15; the reported exception was
`InvalidOperationException: The Atari presentation candidate has no
model-frame mapping for Level 16`, through `ProjectWorldPointWithoutExitCompensation`
and `DrawTube`.

Before correction, a new headless test verified that Level 16 resolves as a
valid production configuration, the Atari presentation path is requested,
`HasModelFrameMapping(16)` is false, direct frame lookup throws, and
`VectorRenderer.DrawTube` throws on the same production projection path. This
reproduction test passed by observing/asserting the missing-frame failure; it
was then converted into a success regression.

## 3. Exact missing integration point

`VectorRenderer.ProjectWorldPointWithoutExitCompensation` dispatches mapped
production levels via `AtariPresentationAdapter.HasModelFrameMapping`. A
missing entry fell through to the explicit unsupported-level exception before
the shared adapter could project the Level 16 points. The Level 16 candidate
already had the displayed-level ROM metadata (shape row 11), raw row-11
record, and exact model geometry; it lacked only Atari model-frame admission
and a host calibration-table entry.

## 4. Presentation architecture and keying

- Candidate admission and model-frame selection are keyed by **displayed
  level** in `AtariPresentationAdapter`, not by polygon area, geometry
  identity, or ROM row. Levels 5 and 6 use their established special
  presentation paths; the other mapped levels use the shared adapter.
- The shared adapter resolves `GetModelFrameMapping(displayedLevel)` and
  independently validates raw metadata from
  `RomPresentationMetadataCatalog.ForDisplayedLevel(displayedLevel)`.
- `RomPresentationMetadata` is keyed by displayed level and carries
  `TubeTableIndex`; `RomTubeMappingCatalog` maps displayed levels to ROM
  shape rows, and `AtariTubeRecordCatalog` exposes records by row index.
  Thus Level 16 metadata selects row 11 without treating row number as the
  presentation calibration key.
- Per-level presentation defaults are keyed by displayed level in
  `AtariPresentationCalibration`. The command-line parsers remain
  level-agnostic; `Program` passes explicit values to `VectorRenderer`, which
  resolves each override against the active displayed level.
- The depth-Y override uses the existing camera-origin drift on Levels 7–15.
  Level 16's raw camera height `$80` equals the model-frame center (128), so
  that drift is mathematically zero. To make the requested control useful on
  Level 16, its initial **PROJECT TRANSLATION** scales depth-dependent
  projected model-Y displacement relative to the near rim. Level 1–15
  calculations are unchanged; Level 16 gain 1 is identity, gain 0 removes
  this depth-dependent model-Y displacement.

## 5. Files changed for this milestone

- `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`
- `src/Tempest.Game/Rendering/AtariPresentationCalibration.cs`
- `src/Tempest.Game/Program.cs`
- `tests/Tempest.Game.Tests/Level16AtariPresentationTests.cs` (new)
- `tests/Tempest.Game.Tests/Level12ProductionTests.cs` (updated stale
  unsupported-Level-16 expectation)
- `tests/Tempest.Game.Tests/TubeDepthYGainTests.cs`
- `README.md`
- `TEMPEST-BEHAVIOR-SPEC.md`
- `AI-Logs/Prompts/2026-11-24-level16-atari-presentation-calibration-enablement-prompt.md`
- This report.

The already-uncommitted Level 16 gameplay/geometry/metadata candidate and its
historical AI-Logs were not rewritten.

## 6. Level 16 model-frame mapping

Level 16's existing model points are derived from ROM row 11 with
`(rawX - 128) * 25/14` for X and `-(rawY - 128) * 25/14` for Y. The shared
adapter therefore maps Level 16 with **25/14 model units per ROM coordinate
unit and Y orientation -1**, matching the transformation of this geometry
back to its actual row-11 coordinates. The mapping is selected by displayed
Level 16 and does not copy a Level 15 presentation frame.

Raw metadata still supplies shape row 11, scale `$0F`, camera height `$80`,
Y2D `$0000`, and closed topology. Those ROM values are not interpreted as
accepted host-space calibration.

## 7. Self-touching / zero-area geometry

The projection mapping does not use a polygon area or require a nonzero
global shoelace sum. It projects the existing ordered 16-point production
definition through the shared transform. The two lobes, repeated center
vertices 0 and 8, closed cyclic topology, and all 16 lanes remain unchanged.
The previously implemented source-lobe winding and ROM-aligned shared-center
Flipper transition are untouched.

Headless tests verify finite near/deep rails, distinct points on both lobes,
equal projections for the repeated center, and finite Flipper and entity
projections. These are coordinate-level mechanical tests, not visual-fidelity
acceptance.

## 8. Supported-level range and startup message

The development-only console message now states that the Atari presentation
candidate is enabled for production Levels **1–16**. Mapping support is
explicitly added for Level 16 only; Level 17 has no production configuration
or model-frame mapping and is not claimed.

## 9. `--tube-*` controls on Level 16

All existing parsers and renderer override paths are reused. Focused tests
confirm parser acceptance with direct Level 16 startup and observable
projection behavior:

| Control | Level 16 projection effect verified |
|---|---|
| `--tube-y-gain` | Changes projected source cross-section Y while leaving X unchanged. |
| `--tube-scale` | Uniformly scales projected points around the existing per-depth anchors. |
| `--tube-vertical-offset` | Applies the requested vertical translation without changing X. |
| `--tube-depth-y-gain` | Leaves the near rim unchanged and changes deep model-Y projection. |

Existing parser validation and prior-level projection behavior remain
unchanged. No aesthetic bounds were added.

## 10. Initial calibration values

Level 16 starts with `(tube-y-gain, tube-scale, vertical-offset,
tube-depth-y-gain) = (1, 1, 0, 1)`. These are neutral control values and an
initial **PROJECT TRANSLATION** only. They are not final defaults, an arcade
match, or Neal's manual acceptance.

## 11–12. Level 16 presentation/entity/Flipper test coverage

The new headless tests verify:

- direct Level 16 production validity, row-11 metadata and successful shared
  model-frame resolution;
- all closed tube rail/rim coordinates are finite at near and far depth;
- both lobes are represented and points 0/8 project identically;
- all four options parse and independently affect their intended projection
  quantity;
- Level 16 Tanker cargo Flippers project, including the ROM-aligned shared
  center 0-to-15 transition at the same `$24` route progress that exposed the
  prior runtime crash;
- representative Tanker, Fuseball, Egg, Spike, shot and enemy-bullet
  projection APIs return finite coordinates; existing game-registered Eggs
  and Spikes are used for their projection calls;
- presentation leaves Level 16 model points, player lane/depth, score, and
  state unchanged.

These tests do not assert visual match, final placement, or Flipper visual
acceptance.

## 13. Levels 1–15 regression coverage

The focused prior-level presentation filter covered the adapter mappings,
accepted calibration defaults, Levels 7–9 presentation, Level 10 diagnostic
projection, Level 14 and Level 15 presentation, tube Y/scale/offset/depth
controls, renderer projection, and ExitZoom compensation:

**PASS — 339 passed, 0 failed.** No prior-level expected projection was
changed.

## 14. Focused validation results

The project uses .NET SDK `9.0.304`, `net9.0`, xUnit 2 and VSTest. Commands
used `--blame-hang-timeout 2min`.

1. Pre-correction missing-mapping reproduction:
   **PASS — 1 test asserted the reported missing mapping/production renderer
   failure.**
2. Corrected exact Level 16 model-frame regression:
   **PASS — 1 test.**
3. Level 16 geometry and model-frame tests:
   **PASS — 4 tests.**
4. All four Level 16 override/parser tests:
   **PASS — 5 tests.**
5. Level 16 Flipper/entity projections and unchanged-state tests:
   **PASS — 2 tests.**
6. Levels 1–15 Atari-presentation regressions:
   **PASS — 339 tests.**
7. Level 16 geometry, production gameplay, shared-center Flipper, startup,
   configuration and metadata regressions:
   **PASS — 105 tests.**

## 15. Full solution suite

The first full run found one stale assertion in
`Level12ProductionTests.ProductionAtariRendererValidatesProjectsAndPreservesClosedTopology`,
which expected `GetModelFrameMapping(16)` to throw. It was updated to assert
the new Level 16 mapping; no production behavior was changed to satisfy that
test.

Final full suite:

```text
PASS — 2012 passed, 0 failed, 0 skipped.
dotnet test TempestCSharp.slnx --blame-hang-timeout 2min --verbosity minimal
```

## 16. Release build

**PASS — 0 warnings, 0 errors.**

```text
dotnet build TempestCSharp.slnx --configuration Release --no-restore --verbosity minimal
```

## 17. `git diff --check`

**PASS — exit code 0.** Git printed informational LF-to-CRLF working-copy
warnings for `README.md` and `src/Tempest.Game/Levels/LevelLoader.cs`.

## 18. Unresolved visual questions

Neal still needs to compare Level 16 against MAME/arcade and select accepted
Y gain, scale, vertical offset and depth-Y gain. Automated projection cannot
establish tube placement/shape fidelity, apparent depth, visual balance,
Flipper glyph appearance, or a match to the arcade. No final setting has been
encoded and no presentation acceptance is claimed.

## 19. Manual calibration command template

```powershell
dotnet run --project src\Tempest.Game -- --start-level 16 --lives 12 --atari-presentation --tube-y-gain VALUE --tube-scale VALUE --tube-vertical-offset VALUE --tube-depth-y-gain VALUE
```

Neal determines and reports accepted values through manual comparison. The
interactive game was not launched by this task.

## 20–21. Final Git/worktree state and actions

- Branch remains `master`; HEAD remains
  `63d37adb6c0b6b4f28a422f0c6c638cf5391e88c`; upstream remains `origin/master`.
- Index remains empty; no staged changes.
- The pre-existing modified and untracked Level 15/Level 16 candidate work
  remains present. This task's prompt, report and new tests remain unstaged.
- **Nothing was staged, committed or pushed.**

## 22–24. Scope confirmations

- Level 17+, recycling and later levels were neither implemented nor
  investigated.
- Level 15 was not used as the Level 16 presentation model; the frame was
  derived from row 11's existing model-coordinate transform and the shared
  normalization convention.
- No final Level 16 calibration values were encoded. Only the neutral initial
  values `(1, 1, 0, 1)` are present pending Neal's manual comparison.
