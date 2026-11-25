# Level 16 bounded implementation report — 2026-11-24

## Status

**IMPLEMENTED; AUTOMATED VALIDATION PASS; MANUAL GAMEPLAY AND PRESENTATION
ACCEPTANCE PENDING.** The interactive game was not launched. No commit or push
was made. Level 17 and tube recycling were not implemented.

## 1. Starting Git/worktree state

- Branch: `master`
- HEAD: `63d37adb6c0b6b4f28a422f0c6c638cf5391e88c`
- Upstream: `origin/master`
- Index: no staged changes
- Initial worktree already included a modified Level 15 promotion closeout
  report and unrelated untracked AI-Logs, local reference, and `temo/` material.
  These pre-existing items were preserved and not staged.
- The required implementation prompt was saved at
  `AI-Logs/Prompts/2026-11-24-level16-bounded-implementation-prompt.md`.

## 2. Evidence used

- Primary task specification: the completed Level 16 ROM/configuration report
  at `AI-Logs/Reports/2026-11-24-level16-rom-configuration-investigation-report.md`.
- Project-local Rev. 2 disassembly at
  `Reference/arcade/Tempest Rom Source code.md` for bounded verification of
  wave, lifecycle, firing, and geometry rules.
- Existing shared geometry, production ledger, Tanker/cargo, reconstruction,
  wave completion, startup, and progression code/tests.
- Level 15 was inspected only to verify the existing progression surface and
  preserve its behavior. It was not used as a Level 16 implementation or
  configuration model. Level 14 was a process reference only; its gameplay
  configuration was not copied.
- No ROM files, screenshots, or other images were inspected for this work.

## 3. Files changed

Implementation and integration:

- `src/Tempest.Game/Levels/Level.cs`
- `src/Tempest.Game/Levels/LevelLoader.cs`
- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/Levels/Level16WaveSelection.cs` (new)
- `src/Tempest.Game/Levels/LevelProgression.cs`
- `src/Tempest.Game/Levels/RomPresentationMetadata.cs`
- `src/Tempest.Game/Levels/AtariTubeRecordCatalog.cs`
- `src/Tempest.Game/Gameplay/Flipper.cs`
- `src/Tempest.Game/Gameplay/Tanker.cs`
- `src/Tempest.Game/TempestGame.cs`
- `src/Tempest.Game/Program.cs`

Tests:

- `tests/Tempest.Game.Tests/Level16GeometryTests.cs` (new)
- `tests/Tempest.Game.Tests/Level16ProductionTests.cs` (new)
- `tests/Tempest.Game.Tests/StartLevelTests.cs`
- `tests/Tempest.Game.Tests/DebugControllerTests.cs`
- `tests/Tempest.Game.Tests/DirectLevelStartupTests.cs`
- `tests/Tempest.Game.Tests/GameplayRecordingTests.cs`
- `tests/Tempest.Game.Tests/InputDiagnosticTests.cs`
- `tests/Tempest.Game.Tests/RomPresentationMetadataTests.cs`
- `tests/Tempest.Game.Tests/LevelConfigurationTests.cs`
- `tests/Tempest.Game.Tests/Level12ProductionTests.cs`
- `tests/Tempest.Game.Tests/Level13ProductionTests.cs`
- `tests/Tempest.Game.Tests/Level15TubeCandidateTests.cs`
- `tests/Tempest.Game.Tests/Level8ProductionTests.cs`

Documentation:

- `README.md`
- `TEMPEST-BEHAVIOR-SPEC.md`
- This report.

## 4–5. Geometry/winding solution and topology preservation

ROM row 11 is loaded in its supplied order and translated using the established
ROM-coordinate transform. Its 16 lane points preserve the repeated central
coordinate at points 0 and 8, self-touching two-lobe shape, closed cyclic
adjacency, and final-to-first connection.

The global shoelace sum is zero because the ordered lobes have opposite signed
areas. `Level` retains global signed-area winding for ordinary nonzero-area
closed tubes. When the total is zero, winding-dependent transition geometry
uses the signed area of the repeated-vertex lobe containing the source lane.
The repeated ROM vertex splits the ordered closed walk into those loops; this
is a local interpretation of the supplied topology, not a substitute polygon
or a lane-order exception. Constructor spinner direction follows lane 0's lobe
sign. The shared spatial pivot path uses the source lane's lobe sign.

The change does not remove or merge vertices, reverse global winding, or
change open-tube behavior. Focused coverage checks exact ROM point data,
repeated center, zero-area construction, closed 16-lane transitions/pivots,
and representative simple-tube winding and transition behavior.

## 6. Regression protection for existing geometry

Ordinary nonzero-area outlines continue to use their existing global winding
calculation. Existing geometry/transition regression tests passed in the
bounded regression run (165 tests), including representative simple closed
tubes and prior production geometry. No unexplained accepted-level geometry
regression remains.

## 7. Level 16 configuration implemented

- Displayed Level 16 maps to ROM shape row 11; it is closed with 16 playable
  lanes and uses the ordered ROM X, Y, and angle bytes from the investigation.
- ROM presentation metadata is registered for normal Level 16 lookup:
  scale `$0F`, camera height `$80`, Y2D `$0000`, closed `$00`, and Flipper
  scale bytes `$00/$04`. These remain raw ROM metadata, separate from host-space
  calibration.
- No Level 16 Atari-presentation calibration or `--tube-*` defaults were set.
- The new production configuration uses ROM supply 27 at default/easy and 30
  at hard. Egg release cadence, lane schedule, and shared Flipper tuning are
  C# production pacing/configuration values, not newly asserted ROM timing.

## 8. Enemy composition/admission policy

`Level16WaveSelection` implements Flipper 0–5, Pulsar 0–0, Tanker 1–2,
Spiker 2–3, and Fuseball 1–1. Its selector excludes ordinary Flippers when
their minimum is zero while preserving Tanker cargo Flippers. It admits
required species minima before choosing among the remaining eligible types.
Admission reserves room for both future Tanker children and uses the existing
active-unit/ledger flow. It does not treat raw `max_enm=$06` as a six-actor C#
limit. Its seven-slot active/cargo envelope is an explicit **PROJECT
TRANSLATION** from the indexed physical-record structure, not an assertion
that ROM `max_enm` directly specifies C# actor concurrency.

## 9. Spike translation

All 16 Level 16 lanes initialize from ROM raw Spike value `$A0`.
**PROJECT TRANSLATION:** C# height is `(0xF0 - raw) * Level.Depth / 224`,
approximately `142.857` at depth 400. Existing shared Spike persistence,
shortening, Spiker interaction, rendering, and Blaster collision behavior are
reused.

## 10. Flipper `$24` mapping

Level 16 selects the shared repeating `$24` route: two flips in one direction,
reverse, three flips in the other direction, reverse, and repeat. The route's
existing move/flip boundary order is reused for ordinary and cargo Flippers;
it is not reduced to `canflip`. The implementation does not claim ROM-backed
initial clockwise/counterclockwise handedness. Final visual handedness remains
for Neal's manual comparison.

## 11. Tanker/cargo integration

Level 16's ROM load choices `$01/$01` produce two independently identified
Flipper cargo units. Existing Tanker lifecycle and production ledger retain
original Tanker identity, latent cargo, pending activation, active cargo, and
terminal obligations. Cargo releases on adjacent sides with opposite
direction bits and uses the Level 16 `$24` route. No parallel Level 16 cargo
counter was added.

## 12. Spiker lifecycle/conversion integration

Level 16 Spikers use the shared production mechanics and terminal
Spiker-to-Tanker conversion when original pending supply is exhausted. The
converted Tanker preserves the original Spiker identity and receives two
ledger-allocated cargo identities. The accepted project translation that
omits the ROM Spiker lane hop remains unchanged. Conversion does not depend on
a Level 15-specific workaround.

## 13. Fuseball/Pulsar policy

Level 16 permits one Fuseball and excludes Pulsars. The ROM raw Fuseball values
`fuzz_move_prb=$C0` and `fuzz_move_flg=$00` are preserved as metadata in the
policy; the existing shared C# Fuseball mechanics are reused without asserting
that raw values directly define C# motion. Fuseballs are not made enemy-shot
species.

## 14. Enemy firing translation

The policy records raw `shot_holdoff=$23` and shot-speed low byte `$C0`.
The existing C# holdoff approximation uses raw `$23` as ticks; this is a
**PROJECT APPROXIMATION**, not a ROM-to-tick conversion. The shared C# enemy
projectile movement is not calibrated to the raw shot-speed byte.

ROM `enm_shotmax=$02` is the final projectile-slot index and scans indices
0, 1, 2: Level 16 default C# capacity is three slots. The policy helpers
represent easy index 1 as two slots and hard index 3 as four slots. Level 14's
existing two-slot behavior is unchanged. Eligible Level 16 firing species are
Flippers, Tankers, and Spikers; no finite Level 16 ammunition count is added.

## 15. Ledger/completion behavior

The shared production ledger and completion check remain authoritative.
Pending original supply, Eggs/reserves, active Tankers and Spikers, Fuseball,
latent and pending cargo, active cargo Flippers, converted Spiker/Tanker
identity, and reconstructed unresolved identities remain lifecycle
obligations. Focused coverage confirms pending cargo prevents completion until
resolved. The meaning of ROM byte `$0116` remains unresolved and no meaning
was invented.

## 16. Death/reconstruction integration

Level 16 opts into existing generic ROM-policy reconstruction. Focused tests
cover reserve/original identity and Tanker cargo conservation, latent and
pending cargo reconstruction, converted lifecycle, Fuseball identity, and
fresh shared runtime state. No Level 16 reconstruction system or parallel
identity allocation was introduced.

## 17. Startup/progression integration

`--start-level 16` resolves the production Level 16 configuration. Natural
Level 15 completion advances into the same Level 16 configuration; campaign
score/lives and fresh-wave Super Zapper behavior continue through shared
production initialization. HUD identity and direct-start mapping are covered
by automated tests. Level 16 has no next production level and is terminal for
this milestone.

## 18. Presentation metadata

Level 16 raw metadata lookup is present in the existing ROM presentation
pipeline. The raw values are retained as ROM evidence only; no visual
calibration values were introduced or accepted.

## 19. Tests added/changed

- Added exact Level 16 geometry and winding/pivot tests.
- Added Level 16 wave-policy, difficulty supply, species selection, cargo,
  conversion, identity, reconstruction, completion, shot-slot, Spike, and
  metadata tests.
- Extended startup, HUD, progression, recording, and presentation-metadata
  coverage.
- Updated prior assertions that expected Level 15 to remain terminal or
  Level 16 to be unavailable. Updated the Level 15 exit test to assert
  transition into the Level 16 Approach with its fresh Spike state.
- No existing Level 14 firing capacity or Level 15 configuration was changed
  to normalize Level 16 behavior.

## 20–23. Automated validation

- Focused Level 16/startup/metadata tests:
  **PASS — 57 passed, 0 failed.**
- Bounded geometry, production lifecycle, campaign, and prior-level regression
  tests:
  **PASS — 165 passed, 0 failed.**
- Full solution test suite, `dotnet test TempestCSharp.slnx
  --blame-hang-timeout 2min --verbosity minimal`:
  **PASS — 1,994 passed, 0 failed.**
- Release build, `dotnet build TempestCSharp.slnx --configuration Release
  --no-restore --verbosity minimal`:
  **PASS — 0 warnings, 0 errors.**
- `git diff --check`:
  **PASS — exit code 0.** Git emitted informational LF-to-CRLF working-copy
  warnings for `README.md` and `src/Tempest.Game/Levels/LevelLoader.cs`.

The first full-suite run found stale assertions for the previous terminal-Level
15 boundary, including the old CLI error text, invalid Level 16 startup/
recording expectations, and a Level 15 completion assertion. These expectations
were updated to the requested Level 16 campaign boundary; the final full suite
then passed. No production behavior was changed to conceal a failing test.

## 24. Unresolved questions and evidence boundaries

- The ROM completion byte `$0116` remains unresolved.
- ROM shot speed `$C0` and raw holdoff `$23` do not establish C# projectile
  speed or simulation tick cadence; the existing shared projectile motion and
  documented holdoff approximation remain project translations.
- ROM does not establish initial `$24` visual handedness. Level 16 spinner and
  Flipper visual handedness require manual play comparison.
- Fuseball raw values are recorded, but the existing C# Fuseball behavior is
  not claimed as a byte-for-byte translation.
- No arcade fidelity or visual acceptance is claimed by this implementation
  or its automated tests.

## 25–26. Manual validation remaining

Neal must manually review gameplay, controller/spinner response, tube
orientation, Flipper motion/handedness, Tanker cargo, Spiker behavior,
Fuseball behavior, perceived timing/difficulty, presentation, and later
calibration. The interactive game was not launched by this implementation.

Verified direct-start command:

```powershell
dotnet run --project src\Tempest.Game -- --start-level 16
```

## 27–30. Final repository state and scope

- Final branch remains `master`, HEAD remains
  `63d37adb6c0b6b4f28a422f0c6c638cf5391e88c`, tracking `origin/master`.
- No files were staged, committed, or pushed. AI-Logs remain unstaged.
- Pre-existing unrelated modified/untracked files remain untouched.
- Level 17 configuration, tube recycling, and post-Level-16 repeating layout
  were not implemented.
- Level 15 was used only to verify the production progression surface and
  protect its behavior; it was not used as Level 16's implementation or
  configuration reference.
