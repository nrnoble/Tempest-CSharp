# Level 7 Atari Presentation Reconstruction — Implementation Report

## Scope and outcome

Reconstructed production Level 7 presentation around its existing production
geometry and the shared Atari presentation adapter. Removed the record-06
presentation prototype and its separate geometry/entity projection path while
retaining record-06 as source data and ROM metadata. Gameplay simulation,
deterministic recording/playback, and the accepted fixed-step model were not
redesigned.

The implementation candidate is complete. Neal's manual visual/gameplay
acceptance remains **PENDING**; no interactive game was launched.

## Implementation and evidence

- The 16 ordered points and 16 playable lanes from the production Level 7
  definition remain authoritative. The Level 7 configuration, center,
  position/depth, initial lane, and topology were not changed.
- Level 7 now follows the same production geometry and shared adapter path as
  tube lines, gameplay entities, rim edges, spikes, lane highlighting, and
  Flipper transition pivots. The Level 7-specific lane-width scale and
  record-lane entity projection are removed.
- Added one adapter placement, `Level7VerticalPlacement = 228.99702f`, to
  vertically place the common projection. The projected near/far production
  envelope is approximately X `[172.1818, 627.8182]` and Y
  `[21.3753, 477.0116]`; its center is approximately `(400, 249.1935)`.
  Horizontal correction is zero. The Y placement moves the envelope center
  to the HUD-cleared logical center, approximately `478.1905`. The normal
  portrait camera remains in the renderer path.
- Local artwork size continues to use the adapter Jacobian. Flipper transitions
  continue to use gameplay-selected directed transitions and shared pivot
  projection; tests cover source lanes, directions, progress, cargo Flippers,
  and the reflex sweep.
- Added Level 7 coverage for mapped far-geometry selection, stable closed-tube
  ExitZoom compensation, shared tube/Blaster/spike compensation, unchanged
  Blaster scale, and the existing compensation lifecycle. The same focal
  coverage also retains Level 8 tests.
- Added a deterministic playback comparison with presentation disabled and
  enabled. It compares all 3,510 recording boundaries and checks that rendering
  does not alter gameplay summaries.
- Removed the prototype renderer, coordinate projector, viewport map,
  reference-shape diagnostic, and their obsolete prototype/entity tests.
  Prototype-only CLI/debug-controller wiring was removed; obsolete switches
  are rejected instead of silently accepted.
- Retained the Level 7 record-06 catalog data and ROM metadata. Raw ROM copies
  were not inspected or modified. No screenshots, videos, or `TEMP/myNotes.md`
  were inspected.

These are implementation and mechanical test results, not claims of arcade
acceptance. The presentation remains an implementation candidate pending
Neal's manual review.

## Files changed

Implementation changes:

- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`
- `src/Tempest.Game/Debug/DebugController.cs`
- `src/Tempest.Game/Program.cs`
- `tests/Tempest.Game.Tests/Level7AtariPresentationTests.cs`
- `tests/Tempest.Game.Tests/ExitZoomFocalCompensationTests.cs`
- Updated shared presentation, diagnostic, CLI, Level 8, and Level 9 tests.
- Removed obsolete record-06 presentation helper and prototype test files.

The pre-existing edit to `.github/copilot-instructions.md` and the untracked
`Reference/arcade/MBUCOD.V05.txt` and `Reference/arcade/MBUDOC.DOC` were
preserved and are unrelated to this milestone.

The substantive prompt was saved at
`AI-Logs/Prompts/2026-11-18-level7-presentation-reconstruction-implementation-prompt.md`.

## Validation

- **PASS** — Final Level 7/playback/ExitZoom targeted tests: 182 passed, 0
  failed.
- **PASS** — Final shared and cross-level presentation targeted tests: 179
  passed, 0 failed.
- **PASS** — `dotnet test TempestCSharp.slnx --no-restore --configuration Release
  --blame-hang-timeout 5m --verbosity minimal`: Release build completed and all
  1,595 tests passed, 0 failed.
- **PASS** — `git diff --check`.
- **PASS** — Git remains on `master` at
  `aee59b39c1d68be830d0014a7b9497d1bb40f3c7`, tracking `origin/master`.
  No files were staged; no commit or push was made.
- **PENDING** — Neal's manual gameplay/presentation acceptance.

## Manual acceptance requested

Please inspect and judge:

- Overall Level 7 shape, centering, and HUD clearance.
- Registration of enemies, player, shots, lane highlights, and rim motion.
- Depth motion of enemies and player shots.
- Directed Flipper routes and cargo Flipper presentation.
- Exit/Approach coherence and any presentation regression.

After automated validation, run the following task-appropriate commands and
provide acceptance feedback:

```powershell
dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation --level 7
dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --playback-file TEMP\GameplayRecordings\Level-7-slowmo.json
dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --playback-file TEMP\GameplayRecordings\Level-7-slowmo.json --atari-presentation
```
