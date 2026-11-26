# Level 17 bounded production implementation report

**Status: PARTIAL — implementation candidate created, full validation blocked by one
Level 15 soak regression invariant.**

## 1. Starting Git state

- Branch: `master`
- Tracking: `origin/master`
- HEAD: `fde09aa31af760958f656aa553555462d51792a0`
- Ahead/behind: `0/0`
- Staged state: no staged changes were present at the start.
- Pre-existing tracked modification:
  [TEMPEST-BEHAVIOR-SPEC.md](../../TEMPEST-BEHAVIOR-SPEC.md)
- Pre-existing untracked state included the existing Level 17 prompts/reports,
  `Reference/arcade/MBUCOD.V05.txt`, and local reference material.
- The pre-existing tracked behavior-spec hunk and untracked local files were
  preserved. The implementation did not clean or reset the worktree.

## 2. Files changed

The implementation added or changed these source/test surfaces:

- Added [Level17WaveSelection.cs](../../src/Tempest.Game/Levels/Level17WaveSelection.cs)
- Changed [LevelConfiguration.cs](../../src/Tempest.Game/Levels/LevelConfiguration.cs)
  for the shared Level 17 configuration, medium firing/shot values, Spike
  height, straight-program identity, and terminal campaign boundary.
- Changed [LevelProgression.cs](../../src/Tempest.Game/Levels/LevelProgression.cs)
  to extend production progression from Level 16 through Level 17.
- Changed [RomPresentationMetadata.cs](../../src/Tempest.Game/Levels/RomPresentationMetadata.cs)
  for the bounded modulo-16 shape resolver, palette catalog, and Level 17
  presentation metadata reuse.
- Changed [LevelLoader.cs](../../src/Tempest.Game/Levels/LevelLoader.cs)
  to make its canonical Level 1 definition a stable shared object.
- Changed [AtariPresentationCalibration.cs](../../src/Tempest.Game/Rendering/AtariPresentationCalibration.cs)
  to add the Level 17 fallback to the accepted Level 1 calibration.
- Changed [AtariPresentationAdapter.cs](../../src/Tempest.Game/Rendering/AtariPresentationAdapter.cs)
  to recognize and project Level 17 through the Shape 1 model frame.
- Changed [VectorRenderer.cs](../../src/Tempest.Game/Rendering/VectorRenderer.cs)
  to permit palette index 1 while retaining existing host role colors.
- Changed [Flipper.cs](../../src/Tempest.Game/Gameplay/Flipper.cs)
  to add straight-$07 below-rim behavior and Tanker-cargo initialization.
- Changed [Tanker.cs](../../src/Tempest.Game/Gameplay/Tanker.cs)
  to return straight `$07` cargo for Level 17.
- Changed [Pulsar.cs](../../src/Tempest.Game/Gameplay/Pulsar.cs)
  to retain original-slot identity on production Pulsar actors.
- Changed [Level3WaveLedger.cs](../../src/Tempest.Game/Levels/Level3WaveLedger.cs)
  to add Pulsar original type/state, commit, runtime validation, reconstruction,
  terminal transitions, and representation validation.
- Changed [TempestGame.cs](../../src/Tempest.Game/TempestGame.cs)
  for Level 17 admission/hatching, speed/firing policy, Pulsar runtime
  representation, destruction/rim-clear handling, and all-rim completion.
- Added [Level17ProductionTests.cs](../../tests/Tempest.Game.Tests/Level17ProductionTests.cs).
- Updated existing startup, progression, calibration, remap, recording,
  presentation, and campaign expectation tests from the former terminal
  Level 16 behavior to the bounded terminal Level 17 behavior.

AI continuity records for this implementation were [the implementation prompt](./2026-11-25-level17-bounded-production-implementation-prompt.md)
and this report.

## 3. Shape-reuse implementation

- Level 17 uses canonical Shape 1 geometry through the same
  `LevelLoader.LoadLevel1()` definition object.
- No independent Level 17 point array or copied Level 1 geometry constants were
  added.
- `RomTubeMappingCatalog` now has a bounded modulo-16 resolver:
  `ResolveShapeRow(displayedLevel)` retains only the first 16 rows and rejects
  supported implementation beyond Level 17 at this milestone.
- `ShapeRowForDisplayedLevel(17)` resolves to row 0, matching
  `ShapeRowForDisplayedLevel(1)`.
- Displayed level, shape row, board identity, palette cycle, and gameplay
  configuration remain separate fields. Level 17 uses `BoardIndex: 0`,
  `PaletteIndex: 1`, and `DisplayedLevel: 17`.
- Level 17 was intentionally not generalized into a 99-level or later campaign
  mapping.

## 4. Calibration inheritance

- Level 17 presentation initially inherits Shape 1/Level 1 presentation defaults:
  Y gain `1.4`, tube scale `0.8`, vertical offset `35`, and neutral depth-Y gain.
- [AtariPresentationCalibration.cs](../../src/Tempest.Game/Rendering/AtariPresentationCalibration.cs)
  contains the explicit Level 17 fallback rather than replaying/copying Level 1
  gameplay configuration.
- [AtariPresentationAdapter.cs](../../src/Tempest.Game/Rendering/AtariPresentationAdapter.cs)
  recognizes Level 17 and returns the accepted Shape 1 model-frame mapping.
- Existing CLI `--tube-*` controls remain absolute dimension overrides.
- Levels 1–16 keep their existing display-level calibration entries unchanged.
- No new visual calibration was derived from screenshots or performed for
  perceived Level 17 placement.

## 5. Raw palette catalog and host translation

- Added a bounded cycle palette representation.
- Preserved raw first-cycle block:
  `00 04 08 0C C3 07 0B 0B`.
- Added raw second-cycle block:
  `00 07 0B 08 44 03 0C 0C`.
- Level 1 resolves palette index 0; Level 17 resolves palette index 1.
- Existing C# entity-role and Blaster-lane colors remain the initial host
  translation. [VectorRenderer.cs](../../src/Tempest.Game/Rendering/VectorRenderer.cs)
  accepts palette 1 but retains existing role colors rather than inventing
  ROM-derived RGB role mappings.
- Pulsar cyan/pulse-white presentation and Super Zapper rail cycling were
  preserved. Raw nibble data exists without a claimed exact host-RGB semantic
  mapping.
- This is a ROM-BACKED raw palette/cycle catalog plus PROJECT TRANSLATION host
  role behavior; exact entity-role RGB mapping remains deferred.

## 6. Level 17 configuration

The shared `LevelConfiguration.Level17Production` medium/default configuration
contains:

- Displayed level: 17
- Progression index: 16
- Board/shape: canonical row 0
- Palette index: 1
- Ordinary supply: 20
- Physical enemy envelope: existing seven-record project translation
- Flippers: minimum 0, maximum 3
- Tankers: minimum 1, maximum 1
- Spikers: minimum 0, maximum 2
- Pulsars: minimum 2, maximum 5
- Fuseballs: minimum 0, maximum 0
- Raw Spike height: `$A0`
- Translated Spike height: approximately `142.857`
- Flipper/Tanker climb speed: `2.53125f`
- Fixed C# Spiker speed: `3f`
- Enemy-shot speed: existing `3f`
- Shot holdoff: 32 ticks
- Maximum active enemy-shot slots: 3
- Medium Pulsar firing: disabled
- Tanker cargo: exactly two Flippers
- Flipper movement: named straight `$07`
- Terminal campaign boundary: Level 17

The effective gameplay parameter level is 17. Easy/hard values remain outside
the production selector and configuration used by this milestone.

## 7. Wave selector

[Level17WaveSelection.cs](../../src/Tempest.Game/Levels/Level17WaveSelection.cs)
provides the dedicated bounded Level 17 policy:

- 20-unit medium supply
- seven physical slots
- guaranteed Pulsar selection when fewer than 2 Pulsar originals have been
  admitted
- Pulsar maximum 5
- exactly one selected Tanker under the 1/1 policy
- ordinary Flippers up to 3 including latent cargo units
- Spikers up to 2
- no Fuseball identities
- Tanker cargo counted as latent Flipper slots
- deterministic random choice through the existing `Func<int,int,int>`
  selector seam
- no predetermined full wave sequence

The selector uses existing physical-envelope checks and cargo-aware accounting.
It does not expose Level 18+ policy.

## 8. `$07` route implementation

- Added `BelowRimFlipperProgram.Straight07`.
- Ordinary Level 17 Flippers set straight-$07 behavior in
  [Flipper.cs](../../src/Tempest.Game/Gameplay/Flipper.cs).
- The route does not flip, reverse, or change lane below the rim.
- It climbs directly toward the rim using the configured 2.53125 climb speed.
- At rim depth it uses existing rim pursuit and clamps normally.
- The route does not inherit Level 5/9 Spike-sensitive predicates.
- Level 1 and Levels 8/16 movement-program behavior were not replaced.
- Level 17 Tanker cargo also initializes straight-$07.

## 9. Flipper/Tanker translated speed

- ROM raw Flipper/Tanker input `$AF` is represented by the project host
  translation `2.53125f`.
- The same value is assigned to ordinary Level 17 Flippers and medium Tankers.
- The C# update remains host-frame simulation rather than a claim of exact
  arcade cycle timing.
- Level 16 remains at its accepted climb speed and repeating `$24` route.
- Easy/hard adjusted raw speed values remain deferred.

## 10. Tanker cargo

- Level 17 Tankers use the existing Tanker latent-cargo pair.
- Cargo count is exactly two Flippers.
- Cargo uses the existing owned-cargo IDs and ordinal identity.
- Natural Tanker release returns
  `TankerCargoMovementProgram.Straight07`.
- Shot/destruction cargo release and cargo re-entry reuse existing lifecycle
  surfaces rather than a Level 17-only cargo implementation.
- No Pulsar or Fuseball cargo was added.

## 11. Pulsar configuration

The implementation reuses the existing Pulsar actor and pulse presentation,
with Level 17 fields exposed through its configuration/ROM constants:

- raw flip rate `$28`
- raw pulse beat `$04`
- medium firing disabled
- existing host climb/descending behavior retained
- existing 51-update decision threshold retained
- existing 18-tick lane pause retained
- existing 14-tick rim pause retained
- existing 181-update pulse cycle retained
- existing 12-degree pulse flip presentation retained
- existing pulse/rim/hazard state machine retained

These retained timings remain [REFERENCE-DERIVED or PROJECT TRANSLATION], not
Level 17 ROM timing facts unless a specific report establishes otherwise.
`PulsarConfiguration` was not introduced as a separate redesigned subsystem;
the existing actor was parameterized through bounded Level 17 constants and
production configuration.

## 12. Pulsar production admission

- Pulsars become selectable original identities on Level 17.
- Minimum 2 means two guaranteed original ledger identities, not two
  simultaneous active actors.
- Maximum 5 bounds the selected Pulsar originals.
- Pulsars hatch through the existing Egg/original lifecycle.
- Pulsars consume normal active capacity.
- Tanker cargo does not create Pulsars.
- Medium production disables Pulsar firing.
- The existing development `--pulsar-test` path remains intact.

## 13. Ledger extension

The generic existing ledger was minimally extended:

- added `Level3OriginalType.Pulsar`
- added `Level3OriginalState.ActivePulsar`
- added `CommitOriginalAsPulsar`
- added Pulsar runtime-type validation
- added active Pulsar representation validation
- included active/reserve Pulsars in unresolved original sets
- included Pulsar rim-cleared/destroyed terminal transitions
- reused existing original IDs and Egg identity handling
- preserved existing Flipper, Tanker, Spiker, Fuseball, cargo, and terminal
  behavior
- no separate Pulsar counter or Level 17-only ledger was introduced

## 14. Pulsar reconstruction

- Reserve/unhatched Pulsars remain unresolved original identities.
- Active Pulsar originals can return to reserve through existing generic
  reconstruction.
- Pulsar Eggs retain their original slot ID.
- Active Pulsar destruction/rim-clear marks the existing original terminal.
- Pulse/pause state is fresh host runtime state after re-entry.
- Medium firing state remains disabled after reconstruction.
- Rim Pulsars participate through shared reconstruction and terminal principles.
- No duplicate original allocation is created.

## 15. Completion obligations

Pulsars now participate in generic unresolved ledger sets and representation
validation. Reserve, Egg, ready-to-hatch, active, transition, rim, pending
reconstruction, destruction/rim-clear, and missing-representation states are
covered by the existing ledger/runtime obligations.

Completion still requires:

- no unresolved Pulsar originals or cargo identities
- no reserve Egg pending
- no released-cargo pending activation
- no active or missing runtime representation
- all cargo and original identities terminal

No Pulsar-specific completion counter was added.

## 16. Pulsar rendering

- The existing Pulsar glyph and renderer were reused.
- The existing six-segment zigzag presentation, pulse-white state,
  lane-boundary flashing/electrification, directed lane transitions,
  rim projection, and collision presentation remain intact.
- No screenshot-based graphics tuning or glyph redesign occurred.
- Level 17 presentation uses the shared Shape 1 projection path.
- Headless projection/draw-path tests: PENDING BLOCKER / NOT COMPLETE.
- Manual visual acceptance remains pending.

## 17. Spiker translation

- Level 17 uses fixed C# Spiker speed `3f`.
- This is a PROJECT TRANSLATION, not a claim that raw Spiker input `$00`
  is the same as zero or a literal 3-unit ROM movement.
- Existing growth, retreat, and terminal conversion behavior were preserved.
- Enemy shots remain at existing 3-unit/update host speed to preserve the
  intended visible relationship.
- Randomized Level 17 Spiker speed was not introduced.

## 18. Enemy firing

- Medium holdoff: 32 ticks.
- Medium capacity: 3 simultaneous enemy-shot slots.
- Eligible species: Flipper, Tanker, and Spiker.
- Pulsar firing: disabled.
- Fuseballs: excluded.
- Existing random/state/depth gates remain.
- Existing enemy-shot speed remains `3f`.
- Level 17 check paths pass a count/capacity of 3 rather than normalizing
  accepted prior-level capacities.
- Existing prior-level shot capacities remain unchanged.

## 19. Spike translation

- Raw Level 17 Spike height `$A0`.
- C# conversion formula:
  `(0xF0 - raw) * Level.Depth / 224`.
- Calculated result: approximately `142.857`.
- Initial Spike topology policy uses existing production lane initialization
  for the reused Shape 1 tube.
- No new lane topology was copied or fabricated for Level 17.

## 20. Natural progression

- `Level16Production -> Level17Production` was added to the production chain.
- Completing Level 16 enters the existing exit/approach transition.
- Score/lives, Super Zapper recharge/reset, and approach behavior reuse existing
  level initialization.
- Level 17 installs shape row 0, palette 1, 20-unit supply, and the terminal
  Level 17 configuration.
- The current Level 17 implementation is terminal and does not create Level 18.

## 21. Direct startup

- `--start-level 17` and `--level 17` resolve through
  `LevelProgression.GetProductionLevel(17)`.
- Both use the shared `LevelConfiguration.Level17Production` instance.
- HUD, score/lives/startup, presentation, palette, and terminal boundary use
  the normal direct-start path.
- Level 18 remains rejected.
- Unsupported malformed and conflicting startup cases remain outside the
  Level 17 implementation.

## 22. Terminal boundary

- Level 17 is the terminal accepted production campaign level for this milestone.
- `GetLastProductionLevel()` resolves Level 17.
- `GetNextProductionLevel(Level17Production)` returns null.
- Level 18 and later levels do not have production configuration or campaign
  startup support.

## 23. Tests added/changed and what they prove

Added [Level17ProductionTests.cs](../../tests/Tempest.Game.Tests/Level17ProductionTests.cs):

- canonical Shape 1 object reuse and row 0 resolution
- palette 0/1 and exact raw block/nibble identities
- medium supply, straight `$07`, 2.53125 speed, 32-tick holdoff, 3-slot capacity,
  and `$A0` Spike translation
- bounded deterministic seed wave-selection properties
- Pulsar ledger commit/destroyed state and Tanker cargo pair identity
- direct startup and inherited Level 1 calibration

Existing tests were updated for Level 17 progression/startup and to reflect
the terminal boundary. Existing shape, palette, presentation, calibration,
ledger, direct-start, and campaign tests continued to cover 1–16 behavior.

## 24. Focused validation results

Actual validation completed:

- **PASS:** Level 17/configuration/progression startup-focused run,
  114 passed, 0 failed.
- **PASS:** final startup/remap/Level 17 focused group,
  53 passed, 0 failed.
- The Level 17 prompt did not reach its separate `$07`, Pulsar, Tanker, or
  presentation test stages as named independent stages because validation stopped
  at the full-suite Level 15 soak failure.

## 25. Levels 1–16 regression results actually obtained

- Focused progression/calibration/startup/presentation tests passed.
- `2044` of `2058` tests passed in the first full-suite run; later reruns
  showed 2057 passed and 1 failed.
- The remaining known failure is in the Level 15 soak test, not a Level 1–16
  suite-wide pass.
- Levels 1–16 behavior is therefore **PENDING BLOCKER**, not declared fully
  green. No regression was retuned or weakened.
- The test failure concerns seed 1982 expecting Flipper species visibility
  during a Level 15 1200-tick soak.

## 26. Recording regression status

- **NOT RUN / NOT REACHED:** local `TEMP/GameplayRecordings/lvl1-16.json`
  deterministic headless playback.
- The recording file was identified but was not replayed or modified.
- Recording pass status remains PENDING BLOCKER.

## 27. Full-suite result

- Final full solution suite:
  **FAIL**, 2057 passed, 1 failed, 0 skipped.
- Exact failing test:
  `Level15ProductionSoakTests.SeededProductionSoakConservesIdentitiesAcross1200Ticks(seed: 1982)`
- Exact failure: the seed 1982 Level 15 soak's selected-type set was
  `[Spiker, Tanker, Fuseball]`; the test did not find `Flipper`.
- This is the exact stop-condition blocker. No fix or investigation was performed.

## 28. Release-build status

- **NOT RUN**.
- Release build was not reached because the stop condition occurred during the
  full suite, after the final suite result showed the Level 15 soak failure.
- No build pass or build failure is claimed.

## 29. `git diff --check` result

- **PASS**: no whitespace errors were reported.
- Git emitted only its working-copy LF-to-CRLF warning for
  `src/Tempest.Game/Levels/LevelLoader.cs`; no diff-check failure occurred.

## 30. Evidence classifications

- **ROM-BACKED:** Shape 1 remap reuse; first/second palette blocks; `$07`
  Flipper P-code; raw `$AF` Flipper/Tanker input; medium supply 20;
  Pulsar minimum 2/maximum 5; medium Pulsar firing off; Tanker cargo `$01`;
  raw `$A0` Spike; shot holdoff `$20`; medium last slot `$02`;
  Pulsar `$28` flip rate and `$04` pulse beat.
- **PROJECT TRANSLATION:** 2.53125 host Flipper/Tanker speed; 3-unit
  fixed Spiker; 3-unit enemy shots; existing Pulsar 51-update/18-tick/14-tick/
  181-update/12-degree behavior; Level 1 calibration inheritance; host role
  RGB reuse.
- **PROJECT DECISION:** terminal Level 17; medium-only support; bounded
  16-level remap reuse and no Level 18+.
- **UNKNOWN:** exact host RGB role mapping and exact easy/hard host timing.
- **APPROXIMATION/INFERENCE:** none promoted to arcade acceptance in this report.
- Manual acceptance remains pending.

## 31. Deferred hard/easy boundaries

- Easy/hard difficulty selection is not implemented.
- Hard supply 23, hard four-shot capacity, and hard Pulsar firing `$40` remain
  named future ROM facts, not implemented player-selectable behavior.
- Easy 2-slot capacity is not implemented.
- Easy/hard adjusted Flipper/Tanker speed and pulse differences remain
  deferred.
- No Levels 18+ or general campaign generator were added.

## 32. Manual-validation status

**PENDING BLOCKER**: the Level 15 soak failure prevents declaring the candidate
complete and begins before final manual acceptance.

Once separately resolved and the remaining required validation is reached,
Neal's Level 17 matrix should cover:

- direct Level 17 startup with `--start-level 17 --lives 12`
- natural 16→17 progression with normal play
- Level 1 versus Level 17 direct-start palette/presentation comparison
- optional `--background-opacity` overlay comparison
- number/presence and visual behavior of 2–5 Pulsars
- straight `$07` climb, no below-rim lane flip/reversal
- Tanker cargo release of exactly two Flippers
- Flipper/Tanker perceived 2.53125 host speed
- Spiker/projectile relative speed
- medium 32-tick holdoff and three-shot pressure
- medium Pulsar firing absence
- Pulsar climb/descend, pulse timing, lane changes, rim pursuit, hazard state,
  collision, and rendering
- exit/completion only after all Pulsars/cargo/enemies resolve
- Shape 1 inherited placement and whether it remains visually acceptable

## 33. Final Git/worktree state

- Branch: `master`
- Tracking: `origin/master`
- HEAD: `fde09aa31af760958f656aa553555462d51792a0`
- Ahead/behind: `0/0`
- The worktree retains the pre-existing [TEMPEST-BEHAVIOR-SPEC.md](../../TEMPEST-BEHAVIOR-SPEC.md)
  modification.
- Level 17 source/test files and the implementation report/prompt are present as
  uncommitted or untracked work according to normal repository status.
- [Reference/arcade/MBUCOD.V05.txt](../../Reference/arcade/MBUCOD.V05.txt)
  and protected local reference material remain preserved.

## 34. Confirmation of staging/commit/push

- Nothing was staged.
- No commit was created.
- No push was performed.
- The implementation report/prompt are continuity records only.

## 35. Level 18+ and scope confirmation

- Level 18+ remains unsupported.
- No 99-level campaign or multi-level generator was implemented.
- No difficulty-selection architecture was implemented.
- No unrelated Levels 1–16 tuning or Level 15 soak fix was performed.
- The candidate remains PARTIAL until the separate Level 15 blocker and the
  remaining required validation are resolved.
