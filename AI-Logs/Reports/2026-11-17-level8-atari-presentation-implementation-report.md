# Level 8 Atari-presentation implementation report

Date: 2026-11-17

## Outcome

**PASS — implementation and automated validation complete; manual acceptance
PENDING.**

Level 8 now receives one shared final presentation Y translation in
`AtariPresentationAdapter.Project`. The placement uses its existing recovered
shape-7 geometry, existing projection, and existing Level 8 entity-rendering
path. It does not change gameplay, geometry, projection, scale, perspective,
or other levels.

No interactive game was launched. No screenshots, videos, frames, or raw ROMs
were inspected or modified. No files were staged, committed, or pushed.

## Starting repository state

- Branch: `master`
- HEAD: `7fb0819cdccddbf0f3e4fbc40409023050546d9d`
- Upstream: `origin/master`
- Index: clean
- Pre-existing tracked worktree modification: `.github/copilot-instructions.md`
- Pre-existing untracked files:
  - `Reference/arcade/MBUCOD.V05.txt`
  - `Reference/arcade/MBUDOC.DOC`
  - `screenshot000.png`

The HEAD matches the Level 10 closeout commit. The pre-existing instruction
edit and untracked files were preserved and excluded from this task.

## Geometry, provenance, and presentation path

- `RomPresentationMetadataCatalog` already maps displayed Level 8 to tube
  table index 7, with scale `0x18`, camera height `0xB0`, signed screen Y
  offset `0x0060`, and open encoding `0xFF`.
- `AtariTubeRecordCatalog` contains only the Level 7 record-06 prototype; it
  does not contain a Level 8 `AtariTubeRecord`.
- The existing Level 8 geometry is recovered raw X/Y data in
  `LevelLoader.LoadLevel8OpenTube`. It normalizes the 16 points with the
  existing `25/14` scale and screen-Y reflection. The Level 8 production
  definition uses those points; its open topology has 15 playable lanes.
  `Level8ProductionTests.ProductionDefinitionUsesRev2ShapeSevenAndNormalizedOpenGeometry`
  already checks the raw-coordinate-to-model translation.
- The Atari presentation path uses `LevelConfiguration.Definition.Points`,
  the existing Level 8 model-frame mapping `(25/14, -1)`, and Level 8 ROM
  metadata through `AtariPresentationAdapter.Project`. It does not use a
  Level 8 record from `AtariTubeRecordCatalog`. The recovered Level 8 arrays
  were reused; no geometry was extracted or invented.
- The local disassembly identifies the tube geometry and presentation tables
  and documents `lev_remap` selection. Existing source/data and tests provide
  the recovered Level 8 shape bytes. The presentation transform remains a
  project translation; no direct mathbox/DVG equivalence is claimed.
- The Level 8 projection and entity registration were already coherent:
  tube vertices and gameplay anchors use the same Level 8 presentation
  projector. Level 8 does not use the Level-7 entity-presentation adapter.

## Placement calculation

Bounds are the full projected envelope of all 16 near and far tube points,
transformed through the current experimental portrait playfield camera. The
calculation uses the actual Level 8 interval (`Position = 50`, `Depth = 400`),
projection `K = 70`, `ZOffset = 5`, and the existing Level 8 metadata.

Before the new translation:

- Logical bounds: X `145.4545..654.5455`, Y `11.3636..520.4545`
- Portrait host bounds: X `319.2727..960.7273`, Y `-25.4018..616.0527`
- Horizontal center: `640`
- Vertical center: `295.3255`

The portrait viewport is `720 x 960`, centered in the `1280 x 960` host
window. The existing persistent HUD reservation ends at host Y `165.6`.
Therefore the HUD-cleared gameplay center is host `(640, 562.8)`, equivalent
to logical Y `478.1905`.

The exact shared logical Y translation calculated from the complete tube
envelope is **+212.28139**. After placement:

- Host bounds: X `319.2727..960.7273`, Y `242.0727..883.5273`
- Center: host `(640, 562.8)`
- Clearance below HUD and above viewport bottom: about `76.47` pixels each

The tube remains horizontally centered without an X adjustment. The
translation is applied after projection to every Level 8 projected point and
entity anchor. There is no per-point movement, shape change, scale adjustment,
or perspective change.

## Changes

- `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`
  - Added Level 8's `212.28139f` final Y placement after the existing projection.
- `tests/Tempest.Game.Tests/Level8AtariPresentationTests.cs`
  - Added tests for selected shape-7 metadata/path, exact portrait bounds and
    centering, Blaster/rim registration, near/middle/far Flipper anchors,
    player-shot registration, starting/developing Spike anchors, deep Egg
    projection, option-off behavior, and equivalent deterministic Level 8
    simulation over 96 input ticks with and without Atari rendering.
- `tests/Tempest.Game.Tests/AtariPresentationAdapterTests.cs`
  - Updated the independent projection reference expectation for the new
    Level-8-only post-projection translation.

No gameplay geometry, lane topology, entity behavior, timing, spawning, RNG,
score, level progression, recording/playback simulation, shared projection
formula, camera architecture, or perspective values were changed.

No Level-7 architecture or Level-7-specific values were generalized into
Level 8. Levels 1-7, Level 9, and accepted Level 10 were left unchanged.
Existing Level 10 tests continue to verify its frozen **+50** final Y
placement.

## Validation

- **PASS — focused Level 8 tests:** 37 passed, 0 failed.
  `dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~Level8AtariPresentationTests|FullyQualifiedName~Level8ProductionTests" --blame-hang-timeout 4m --verbosity minimal`
- **PASS — targeted presentation and regression tests:** 367 passed, 0 failed.
  Covered Level 8, Levels 1-9 Atari presentation selection, metadata, Level 7
  presentation isolation, Level 10 placement, display/camera, transitions,
  approach, startup, and recording/playback.
- **PASS — full solution tests with hang protection:** 1,599 passed, 0 failed,
  0 skipped.
  `dotnet test TempestCSharp.slnx --no-restore --blame-hang-timeout 5m --verbosity minimal`
- **PASS — Release solution build:** 0 warnings, 0 errors.
  `dotnet build TempestCSharp.slnx --no-restore --configuration Release --verbosity minimal`
- **PASS — `git diff --check`:** no whitespace errors.
- **PASS — added test whitespace scan:** no trailing whitespace found.

The first focused bounds assertion caught an incorrect initial calculation
that assumed a `300/300` Level 8 depth interval. The implementation and tests
were recalculated using the actual `50/400` interval before final validation;
all final validation above completed successfully.

## Ending Git state

- Branch, HEAD, and upstream remain `master`, `7fb0819cdccddbf0f3e4fbc40409023050546d9d`,
  and `origin/master`.
- Index remains clean; no staging, commit, or push was performed.
- Task worktree changes are limited to the presentation adapter and the two
  related test files.
- The pre-existing `.github/copilot-instructions.md` modification and three
  pre-existing untracked files listed above remain untouched.
- The prompt and this report are ignored AI-Logs records and are not staged.

## Manual handoff

Manual acceptance remains **PENDING**. Neal should run:

```powershell
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation --level 8
```

Judge the Level 8 tube shape, horizontal centering, vertical centering beneath
the HUD, Blaster/rim registration, enemy and shot registration, normal
along-tube and rim motion, unchanged gameplay, and absence of regressions to
other accepted presentation behavior. No visual-fidelity acceptance is
claimed from automated tests.

## Known limitations and records

- Visual fidelity and gameplay feel require Neal's manual acceptance.
- The Level 8 record is not cataloged as an `AtariTubeRecord`; the current
  presentation uses the already-recovered Level 8 `LevelDefinition.Points`
  and existing ROM metadata.
- Manual acceptance is pending; no durable behavior-spec or development-notes
  acceptance entry was added.
- Prompt: `AI-Logs/Prompts/2026-11-17-level8-atari-presentation-implementation-prompt.md`
- Report: `AI-Logs/Reports/2026-11-17-level8-atari-presentation-implementation-report.md`
