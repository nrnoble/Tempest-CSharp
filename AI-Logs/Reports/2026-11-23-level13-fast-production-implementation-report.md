# Level 13 fast production implementation report

**Status: IMPLEMENTED; AUTOMATED VALIDATION PASS; MANUAL ACCEPTANCE PENDING**

## Scope and baseline

Implemented displayed Level 13 as a production campaign level using ROM-backed
shape row 14 and the established Level 11/12 production ledger, presentation,
and lifecycle systems. The level follows Level 12, is terminal until Level 14
is implemented, and remains available through both `--level 13` and
`--start-level 13`. Direct-only Level 15 remains outside the campaign.

The task's authoritative completion token was
`TEMPEST_LEVEL13_FAST_PRODUCTION_IMPLEMENTATION`. The starting repository
baseline was `master` at `534cbf21150dc501300c548f98972a4c64198602`, tracking
`origin/master`. Existing untracked material under `AI-Logs/` and
`Reference/arcade/` was preserved. No files were staged, committed, pushed,
stashed, reset, or rebased, and the interactive game was not launched.

## Implementation

- Added Level 13's ordered shape-row-14 geometry and raw presentation metadata;
  integrated its closed 16-lane tube, production configuration, Atari model
  frame, neutral first-pass calibration, vector HUD, and documentation.
- Extended natural progression from Level 12 to Level 13, with Level 13
  terminal behavior and fresh Level 1 restart preserved. Direct startup,
  recording-start validation, and presentation support now include Level 13.
- Added Level 13 supply and picker policy: ordinary/hard supply 26/29, active
  enemy cap 6, physical slot cap 7, and production selection of Tankers,
  Spikers, and Fuseballs. The source picker requires nonzero species minima;
  therefore Flipper raw maximum 5 does not enable ordinary Flippers, and
  Pulsars remain unavailable before the accepted Level 17 boundary.
- Reused the existing Spiker, Fuseball, production-ledger, death/re-entry, and
  Tanker cargo lifecycle. Level 13 cargo uses the established deep
  spike-sensitive `$53` and straight `$07` paths with the existing `$20`
  along threshold. The raw shot holdoff 44 remains a simulation-tick
  approximation; no independently verified Level 13 timing is claimed.
- Added headless tests for ROM-row data and ordering, selector reachability,
  cargo programs, closed topology and spinner direction, presentation and HUD,
  direct/natural progression, and a deterministic 1,200-tick ledger soak.
  Updated existing tests whose former Level 13 “unsupported” expectation is
  no longer valid.
- No global spinner mapping or movement behavior was changed.

## Evidence and fidelity boundary

The shape data and presentation parameters are documented as ROM-backed,
anchored to the local disassembly in
`Reference/arcade/Tempest Rom Source code.md`. Displayed Level 13 maps to shape
row 14 / lookup index 12. Ordered X, Y, and angle data are captured in
`src/Tempest.Game/Levels/LevelLoader.cs` and asserted in
`tests/Tempest.Game.Tests/Level13ProductionTests.cs`. Presentation metadata is
scale `0x18`, camera height `0x20`, signed Y offset `-352`, closed flag `0`,
and Flipper scale `0x0400`.

The centered coordinates, `25/14` scaling, Y reflection, neutral presentation
calibration, simulation-tick holdoff, and existing movement/cargo mappings are
project translations or approximations, not independent arcade observations.
The source picker only tries species with nonzero minima. Level 13's raw
Flipper `0/5`, Tanker `1/2`, Spiker `2/3`, Fuseball `1/1`, and Pulsar `0/0`
values therefore produce reachable original species Tanker, Spiker, and
Fuseball; Flipper cargo remains a separate Tanker lifecycle.

A separate bounded K1 parameter-byte probe attempted offsets `0x0607` and
`0x0633`; those offsets were not the parameter-table locations, so its bytes
were discarded and no gameplay conclusion was taken from them. The recorded
parameter values come from the tracked disassembly. No ROM reference files
were modified.

## Changed project files

- `README.md`, `TEMPEST-BEHAVIOR-SPEC.md`, `TEMPEST-DEVELOPMENT-NOTES.md`
- `src/Tempest.Game/Gameplay/Tanker.cs`
- `src/Tempest.Game/Levels/AtariTubeRecordCatalog.cs`
- `src/Tempest.Game/Levels/Level13WaveSelection.cs` (new)
- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/Levels/LevelLoader.cs`
- `src/Tempest.Game/Levels/LevelProgression.cs`
- `src/Tempest.Game/Levels/RomPresentationMetadata.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`
- `src/Tempest.Game/Rendering/AtariPresentationCalibration.cs`
- `src/Tempest.Game/TempestGame.cs`
- `tests/Tempest.Game.Tests/AtariPresentationAdapterTests.cs`
- `tests/Tempest.Game.Tests/DebugControllerTests.cs`
- `tests/Tempest.Game.Tests/DirectLevelStartupTests.cs`
- `tests/Tempest.Game.Tests/GameplayRecordingTests.cs`
- `tests/Tempest.Game.Tests/InputDiagnosticTests.cs`
- `tests/Tempest.Game.Tests/Level12ProductionTests.cs`
- `tests/Tempest.Game.Tests/Level13ProductionTests.cs` (new)
- `tests/Tempest.Game.Tests/Level15TubeCandidateTests.cs`
- `tests/Tempest.Game.Tests/LevelConfigurationTests.cs`
- `tests/Tempest.Game.Tests/RomPresentationMetadataTests.cs`
- `tests/Tempest.Game.Tests/StartLevelTests.cs`

The paired user task is recorded in
`AI-Logs/Prompts/2026-11-23-level13-fast-production-implementation-prompt.md`.
`AI-Logs/` records are ignored working history and were not staged.

## Automated validation

- Focused Level 13 production tests: **PASS**, 8 tests.
- Level 13 plus Level 12 closed-rim spinner tests: **PASS**, 9 tests.
- Full solution test suite, Release, with a five-minute hang bound:
  **PASS**, 1,862 passed, 0 failed, 0 skipped.
- Release solution build: **PASS**, 0 warnings, 0 errors.
- `git diff --check`: **PASS**. Git emitted only line-ending conversion
  warnings for existing working-copy files; no whitespace errors were found.

An intermediate full-suite run exposed tests that still treated Level 13 as
unsupported and an over-specific first draft of the new spinner test. The
unsupported-level test cases were moved to Level 14, the spinner test was
bounded around actual lane-boundary crossings, and the final full-suite run
passed. No existing global spinner behavior was retained from the discarded
experiment.

## Manual acceptance required

No interactive gameplay or visual acceptance was performed. Neal should review
Level 13's tube identity and silhouette, closed-lane traversal in both
directions, spinner feel, Tanker cargo and enemy mix/movement, firing and
enemy-shot behavior, wave completion and Level 12-to-13 transition, score/life
carry, HUD, Atari presentation, and terminal restart. Automated geometry,
ledger, and deterministic simulation tests do not establish arcade-fidelity
acceptance.

Suggested launch commands (run manually from the repository root):

```powershell
dotnet run --project src\Tempest.Game -- --level 13 --lives 10
dotnet run --project src\Tempest.Game -- --start-level 13 --lives 10
dotnet run --project src\Tempest.Game -- --start-level 13 --atari-presentation --lives 10
dotnet run --project src\Tempest.Game -- --start-level 13 --atari-presentation --calibration-guides --tube-y-gain 1 --tube-scale 1 --tube-vertical-offset 0 --lives 10
```

Do not treat this implementation candidate as manually accepted until Neal
completes the gameplay and visual review.
