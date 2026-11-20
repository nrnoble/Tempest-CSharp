# Level 9 Atari-presentation implementation report

Date: 2026-11-17

## Outcome

**PASS — bounded implementation and automated validation complete; manual
acceptance PENDING.**

Level 9 now receives one shared final logical Y translation after its existing
Atari presentation projection. The implementation reuses the production
Level 9 geometry, metadata, model-frame mapping, tube renderer, and entity
anchor path. No gameplay, geometry, projection formula, scale, perspective,
camera, recording format, or other-level behavior was changed.

No interactive game was launched. No screenshots, videos, frames, or raw ROMs
were inspected. No files were staged, committed, or pushed.

## Starting repository state

- Branch: `master`
- HEAD: `60b1e95f080ff9a3f0c78d7b000fd12e22cf0d4b`
- Upstream: `origin/master`
- Ahead/behind: `0/0`
- Index: clean
- Pre-existing tracked worktree modification, preserved untouched:
  `.github/copilot-instructions.md`
- Pre-existing untracked files, preserved untouched:
  - `Reference/arcade/MBUCOD.V05.txt`
  - `Reference/arcade/MBUDOC.DOC`
  - `screenshot000.png`

The starting HEAD matches the specified Level 8 closeout commit. The prompt
and report below are ignored AI-Logs records; no AI-Logs content was staged.

## Geometry, provenance, and presentation path

- Production displayed Level 9 uses `LevelConfiguration.Level9Production`,
  whose `LevelLoader.LoadLevel9OpenTube` definition is built from the existing
  `LevelLoader.Level9RomEvidence` ordered X/Y data. It does not use or rebuild
  `AtariTubeRecordCatalog` geometry.
- **ROM-BACKED / REV-2 evidence:** the local disassembly describes the
  `lev_remap` lookup; the accepted project records and current tests establish
  displayed Level 9 as shape row 13 (`lev_remap = $0D`). Current data has 16
  ordered vertices, open topology, and 15 playable lanes (0–14).
- The C# definition keeps the recovered point order and applies the existing
  project transform `(raw - 128) * 25/14`, with Y reflection. The existing
  model-frame mapping remains `(25/14, -1)`.
- Current Level 9 ROM presentation metadata is table index `13`, scale
  `0x0C`, camera height `0xB0`, little-endian Y2D `0x0140` (signed `320`),
  open value `0xFF`, Flipper scale `0x0250`, and tube color index `0x05`.
  The existing shape-angle sequence and raw geometry remain unchanged.
- Production values are position `50`, depth `400`, definition center Y
  `300`, and Y offset `0`.
- With Atari presentation enabled, `VectorRenderer.DrawTube` projects
  `LevelDefinition.Points` through `ProjectWorldPoint` and
  `AtariPresentationAdapter.Project`. The mapped world-anchor path used by
  Blaster, enemies, shots, spikes, and effects uses that same projector;
  lane-centered entities use the same path on the existing lane midpoint.
- `UsesAtariLevel7PresentationCandidateForCurrentLevel` is restricted to
  displayed Level 7 while Playing. Level 9 does not use the Level-7 entity or
  tube mapper. No Level-7 architecture was added or generalized.
- Existing production tests continue to verify the exact ordered Level 9
  source coordinates, angle data, topology, lane count, and metadata. New
  presentation tests verify those inputs are what the candidate renders.

## Entity registration and motion protection

`Level9AtariPresentationTests.LevelNineEntitiesRemainRegisteredToTheSharedTubeProjection`
checks:

- Blaster/player edge at the near rim;
- Flipper/enemy anchors at near, middle, and far depths;
- player and enemy shot lane-midpoint anchors;
- a starting and developing Spike;
- the deep-end Egg anchor.

The checks compare each anchor against the same projected level geometry and
verify that Level 9 is not dispatched through the Level-7 mapper.

`AtariPresentationRenderingDoesNotChangeLevelNineSimulation` advances matched
deterministic Level 9 games for 96 input ticks while exercising the candidate
render paths and compares player, entity, score, supply, and level state.
`LevelNineRecordingFormatAndPlaybackRemainIndependentOfPresentation` writes
and reloads the current Version 2 recording, verifies that presentation
selection is not added to its format, and compares standard and candidate
playback state over the replay.

## Placement calculation

The complete tube envelope includes all 16 points at both active interval
ends. The calculation uses the app's current portrait values: projection
`K = 70`, `ZOffset = 5`, perspective `0`, Level 9 position `50`, depth `400`,
the existing Level 9 metadata and model mapping, and the experimental portrait
playfield camera.

Before placement:

- Logical bounds: X `145.45454..654.54547`, Y `119.44444..475`
- Host-space bounds: X `319.27274..960.7273`, Y `110.78..558.78`
- Horizontal center: logical `400`, host `640`
- Vertical center: logical `297.22222`, host `334.78`

The persistent HUD ends at host Y `165.6`; the portrait viewport ends at
`960`. The HUD-cleared gameplay center is host `(640, 562.8)`, equivalent to
logical Y `478.190476`.

The calculated shared final translation is **`+180.968254` logical Y**,
implemented as `Level9VerticalPlacement = 180.96825f`. No X correction is
required. The final host bounds are X `319.27274..960.7273`,
Y `338.8..786.8`, centered at `(640, 562.8)`. This leaves `173.2` host pixels
of clearance above and below the tube within the HUD-cleared composition.

The existing projection, local scale, perspective, geometry, and near/far
relationships remain intact; translation occurs only after the existing
projection. Frozen Level 8 `+212.28139` and Level 10 `+50` placements remain
unchanged and are regression-asserted.

## Files changed

- `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`
  - Added the Level-9-only post-projection Y translation.
- `tests/Tempest.Game.Tests/Level9AtariPresentationTests.cs`
  - Added geometry/envelope, registration, option-off, deterministic
    simulation, and recording/playback regressions.
- `tests/Tempest.Game.Tests/AtariPresentationAdapterTests.cs`
  - Updated the independent projection reference expectation for Level 9's
    new final translation.
- `AI-Logs/Prompts/2026-11-17-level9-atari-presentation-implementation-prompt.md`
  - Saved the supplied substantive prompt.
- `AI-Logs/Reports/2026-11-17-level9-atari-presentation-implementation-report.md`
  - This report.

Gameplay and simulation code were not changed. Level 7 was not changed and no
Level-7-specific presentation architecture was imported. Level 8 and Level 10
implementations and their accepted documentation were not changed. No durable
behavior document was updated because this presentation candidate is not yet
manually accepted.

## Validation

- **PASS — focused Level 9 presentation/production tests:** 30 passed,
  0 failed.
  `dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~Level9AtariPresentationTests|FullyQualifiedName~Level9ProductionTests" --blame-hang-timeout 4m --verbosity minimal`
- The first broad targeted run found two independent-reference assertions
  that did not yet include the Level 9 translation. The reference test was
  updated; the completed targeted rerun passed.
- **PASS — targeted presentation and gameplay regressions:** 550 passed,
  0 failed. Coverage included Atari presentation and Level 9 production,
  Levels 1–9 selection, Level 8/10 placement, Approach/transition/exit,
  recording/playback, camera, and display tests.
  `dotnet test TempestCSharp.slnx --no-restore --filter "FullyQualifiedName~AtariPresentation|FullyQualifiedName~Level9Production|FullyQualifiedName~DiagnosticLevel10|FullyQualifiedName~Level8Production|FullyQualifiedName~Approach|FullyQualifiedName~Transition|FullyQualifiedName~Exit|FullyQualifiedName~Recording|FullyQualifiedName~Playback|FullyQualifiedName~Camera|FullyQualifiedName~Display" --blame-hang-timeout 4m --verbosity minimal`
- **PASS — full solution tests:** 1,604 passed, 0 failed, 0 skipped.
  `dotnet test TempestCSharp.slnx --no-restore --blame-hang-timeout 5m --verbosity minimal`
- **PASS — Release solution build:** 0 warnings, 0 errors.
  `dotnet build TempestCSharp.slnx --no-restore --configuration Release --verbosity minimal`
- **PASS — `git diff --check`:** no whitespace errors.
- **PASS — new test-file whitespace check:** no trailing whitespace.

## Ending repository state

- Branch: `master`
- HEAD: `60b1e95f080ff9a3f0c78d7b000fd12e22cf0d4b` (unchanged)
- Upstream: `origin/master`; ahead/behind `0/0`
- Index: no staged changes
- Worktree changes: the two implementation/test edits above plus the
  pre-existing `.github/copilot-instructions.md` edit
- Untracked files: the new Level 9 test plus the three pre-existing untracked
  local files listed above
- AI-Logs prompt/report remain ignored and unstaged

No reset, restore, clean, stash, branch change, stage, commit, or push was
performed.

## Manual handoff

**Manual acceptance: PENDING.** After automated validation, Neal should run:

`dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation --level 9`

Please judge Level 9 tube shape and open topology, horizontal and HUD-cleared
vertical centering, Blaster/rim and enemy registration, player/enemy shot
registration, along-tube and rim motion, unchanged gameplay, and no regression
to Levels 8 or 10. This implementation stops here for manual acceptance.

Known fidelity boundary: the geometry and presentation fields are recovered
ROM-backed inputs, while the `25/14` C# model-frame transform and
`+180.96825` final placement are project translations, not Atari constants or
a claim of hardware-equivalent rendering.
