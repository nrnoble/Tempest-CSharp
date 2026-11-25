# Level 16 Runtime Flipper-Pivot Crash Investigation

**Status: PARTIAL — deterministic production-path reproduction complete; no
correction made because the reproduced transition exposes a pivot-model
limitation covered by the task's architectural stop condition.** The manually
observed crash remains uncorrected. No interactive game was launched.

## 1. Starting Git/worktree state

- Branch: `master`
- HEAD: `63d37adb6c0b6b4f28a422f0c6c638cf5391e88c`
- Upstream: `origin/master`
- Index: no staged changes.
- Worktree: 24 tracked files were already modified before this task. This
  included the Level 16 candidate changes in `Level.cs`, `LevelLoader.cs`,
  `LevelConfiguration.cs`, `LevelProgression.cs`,
  `RomPresentationMetadata.cs`, `AtariTubeRecordCatalog.cs`,
  `Flipper.cs`, `Tanker.cs`, `TempestGame.cs`, and related startup/tests/docs;
  it also included unrelated Level 15, README, and behavior-spec changes.
- Relevant pre-existing untracked Level 16 candidate files:
  `src/Tempest.Game/Levels/Level16WaveSelection.cs`,
  `tests/Tempest.Game.Tests/Level16GeometryTests.cs`,
  `tests/Tempest.Game.Tests/Level16ProductionTests.cs`, and the two required
  Level 16 history report/prompt pairs.
- Other pre-existing untracked Level 15 AI-Logs, `Reference/arcade/MBUCOD.V05.txt`,
  and `temo/gameplayrecordings/level-13` were left untouched.
- The candidate remains uncommitted. No original Level 16 production diff was
  changed by this investigation; temporary reproduction tests were removed
  after capturing their failing values.

## 2. Manual failure evidence

**MANUALLY OBSERVED RUNTIME EVIDENCE (user-provided):** Neal successfully
launched Level 16; the tube looked good and gameplay began normally. After a
couple of seconds, rendering a Flipper transition threw
`System.InvalidOperationException: An inward pivot ray did not intersect its
closed tube polygon.` The command was
`dotnet run --project src\Tempest.Game -- --start-level 16 --lives 12`.
`--lives 12` is not implicated by the observed sequence.

The user-provided stack located the failure in
`Level.CalculateFirstPolygonBoundaryDistance`,
`Level.CalculateFlipperPivotEndpoints`, and
`VectorRenderer.ProjectFlipperEndpoints`/`DrawFlipper`.

## 3. Deterministic reproduction method

Two temporary fail-first test probes were added, run, and removed:

1. `Level16GeometryTests.EveryDirectedLevelSixteenPivotHasAForwardTubeBoundary`
   was configured to sweep all 16 source lanes, both directed adjacent
   transitions, and 1,023 interior progress values per transition. It stopped
   on the first failure: source lane `0`, direction `+1`, at progress
   `0.7636719`. The preceding source-0 direction-`-1` case completed. Thus
   this was a bounded fail-fast probe, not a completed matrix.
2. `Level16ProductionTests.Repeating24TankerCargoCanBeProjectedThroughItsCompleteRuntimePivot`
   used `Tanker.ReleaseFlipperCargo`, the production
   `Flipper.CreateTankerCargo` factory with `Repeating24`, repeated production
   `Flipper.Update` calls, and
   `VectorRenderer.ProjectFlipperEndpoints`. This is a headless production
   entity/render-projection path, not a geometry-only substitute.

The second probe failed from the same `InvalidOperationException`. Its
diagnostic test catch only added transition context and rethrew a test failure;
no production exception handling was added. Both temporary probes were removed
so no intentionally failing tests remain in the worktree.

## 4. Exact failing production transition values

- Flipper: Tanker cargo, stable diagnostic test ID `41`.
- Tanker release lane: `1`; first-side direction: `+1`.
- Source lane `0`; destination lane `15`; direction `+1`.
- Active `$24` repeating route transition; total turn `180` degrees.
- Failure progress: `0.8`; turn `144/180` degrees; Flipper height `46`.
- Hinge vertex: `0`; pivot origin: `<0, 0>`.
- Source lobe: ordered edges `0..7`, positive winding; computed twice-area
  `58367.34756255636` in the translated model coordinates.
- Spatial sweep: `+180` degrees.
- Proposed normalized ray: `<-0.25345543, -0.967347>`.
- All polygon edges `0..15` were considered; there were **no valid forward
  intersections** at this ray.
- The pivot origin is the repeated central coordinate (`points[0] == points[8]`).
  The transition moves between the two lobes through that shared junction.
- This is model-space geometry. The exception occurs before projected endpoints
  are passed to `ProjectWorldPoint`; projected depth does not affect the
  intersection outcome.

The exhaustive geometry probe independently failed at source `0` to
destination `15`, direction `+1`, progress `0.7636719`, with the same exception,
positive source-lobe twice-area, pivot `<0, 0>`, ray
`<-0.3619686, -0.93219036>`, and no forward intersections.

## 5. Geometric cause; static vs dynamic behavior

The current candidate correctly preserves the ordered ROM row 11 walk and
selects source-lobe winding when total shoelace area is zero. That does not
make the repeated central vertex an ordinary polygon vertex: its adjacent
edges `15->0` and `0->1` belong to different lobes. For the reproduced
transition, the resulting half-turn rotates the moving endpoint from the
source lobe toward the opposite lobe. Before reaching the destination, the ray
enters an angular gap between the lobe boundaries. At progress `0.8`, no
positive ray intersection exists on either lobe.

This is distinct from the static tube: the ROM points, lane count, repeated
center, cyclic adjacency, self-touching topology, and normal tube presentation
remain intact. The crash occurs only when the dynamic pivot asks a single
nonzero-radius ray to traverse the shared-center gap.

The ray direction can only be kept within the lobe union in that gap by
collapsing the moving endpoint to the shared point; reaching the other lobe
continuously then requires a retract-to-center / rotate-at-center /
extend-from-center path. That changes the current single-ray pivot model and
includes a degenerate instant. Choosing that visual transition without stronger
behavioral evidence would fabricate a Flipper motion rather than make a
bounded intersection correction. This meets the task's stop condition for a
shared-center transition requiring a broader pivot redesign.

## 6. Correction and files changed

**No production correction was implemented.** The exact ROM geometry and
presentation are unchanged. No durable behavior-spec update was needed because
no new maintainable rule was introduced.

The only files created for this task are this report and its paired prompt:

- `AI-Logs/Prompts/2026-11-24-level16-runtime-flipper-pivot-crash-correction-prompt.md`
- `AI-Logs/Reports/2026-11-24-level16-runtime-flipper-pivot-crash-correction-report.md`

The pre-existing Level 16 geometry/production tests were restored after the
temporary probes were removed. The production exception was not caught,
suppressed, or otherwise changed.

## 7. Test and validation outcomes

- Exact crash reproduction: **FAIL (reproduced as required; not corrected)**.
- Bounded directed-transition sweep: **FAIL (identified the no-intersection
  state; no full corrected-coverage test was retained)**.
- Production Flipper/render-projection path: **FAIL (reproduced the exception
  through the production path)**.
- Prior-level regression suite: **NOT RUN**; no production behavior was changed.
- Tanker/cargo and `$24` regression suite: the filtered Level 16 production
  run executed 13 tests; 12 passed and the temporary runtime reproduction test
  failed with the described crash. No other failures were reported.
- Full solution test suite: **NOT RUN** because the architectural stop
  condition was reached before correction.
- Release build: **NOT RUN** for the same reason.
- `git diff --check`: **PASS** (exit code 0). Git emitted only its existing
  LF-to-CRLF working-copy warnings for `README.md` and `LevelLoader.cs`.

The .NET SDK detected was `9.0.304`; the test project targets `net9.0`, uses
xUnit 2/VSTest, and test commands used the VSTest-compatible
`--blame-hang-timeout 2min` bound.

## 8. Unresolved question and manual acceptance boundary

The correct visually intended `$24` Flipper motion when crossing the point
where the two lobes touch is not established by the available evidence. A
follow-up would need an explicitly authorized topology-aware pivot/path design
and Neal's review of the proposed dynamic behavior. Do not treat this report or
automated reproduction as Level 16 gameplay acceptance.

The user's exact manual command is:

```powershell
dotnet run --project src\Tempest.Game -- --start-level 16 --lives 12
```

**Do not use it as a retest yet:** the crash remains and no correction was
made. Neal's manual acceptance remains pending any separately authorized
correction.

## 9. Scope and final repository state

- Level 17 and tube recycling were not implemented.
- Presentation calibration was not performed.
- Level 15 was not used as a correction model; Level 14 was process-only.
- No ROM geometry, protected reference material, or user recordings were
  changed.
- No files were staged, committed, or pushed. AI-Logs remain unstaged.
- Pre-existing unrelated edits and untracked files remain preserved.
