# Level 18 Bounded Production Implementation Report

## 1. Executive summary

Implemented medium/default production Level 18 using the shared gameplay,
identity, palette, tube, presentation, and progression systems. The implementation
adds ROM-backed wave eligibility and limits, reuses the canonical Shape 2 and
cycle-1 palette, shares the `$0B` Flipper/cargo route, and parameterizes the
shared Pulsar actor for Level 18's threshold and movement regions. Level 17's
existing Pulsar behavior remains on its explicit one-speed compatibility
configuration.

Focused regressions, both unmodified headless recordings, the complete solution
test suite, a fresh Release build, and whitespace validation completed
successfully. Manual gameplay/presentation acceptance remains pending; no
interactive game was launched.

## 2. Starting Git/worktree state

Starting point recorded before implementation: branch `master` at
`0ce7e538374c4e155b75cf536b4f3a67c8da3de0`, tracking `origin/master`, with no
staged or tracked worktree changes. Existing untracked Level 18 investigation
records and `Reference/arcade/MBUCOD.V05.txt` were protected and left untouched.

## 3. Authorities read

- Local ROM-derived disassembly: `Reference/arcade/Tempest Rom Source code.md`.
- Level 18 configuration investigation:
  `AI-Logs/Reports/2026-11-25-level18-rom-configuration-readonly-investigation-report.md`.
- Pulsar movement translation investigation:
  `AI-Logs/Reports/2026-11-26-level18-pulsar-movement-translation-investigation-report.md`.
- Durable project behavior/evidence records:
  `TEMPEST-BEHAVIOR-SPEC.md` and `TEMPEST-DEVELOPMENT-NOTES.md`.
- Existing C# gameplay, rendering, recording/playback, and regression tests.

No raw ROM copy was opened, hashed, enumerated, or modified. No arcade
observation or new manual acceptance is claimed.

## 4. Level 18 identity and mapping

Level 18 is displayed as 18, uses progression index 17, board index 1, and
effective gameplay parameter level 18. Level 19 remains unsupported.

## 5. Canonical Shape 2 reuse

Level 18 references `LevelConfiguration.Level2Square.Definition`; it does not
copy or create a Level 18 polygon. ROM row 1 is mapped to the established
canonical Shape 2 square with 16 lanes.

## 6. Shape 2 presentation reuse

The ROM presentation resolver now supports the later displayed level while
retaining row-based table selection. Level 18 resolves through the established
Shape 2 presentation row; calibration resolves by the first displayed level
for that shape, preserving the existing Level 2 defaults. No Level 18-specific
geometry or calibration was introduced.

## 7. Cycle-1 palette reuse

Displayed Level 18 resolves to palette index 1. Existing semantic palette
resolvers supply persistent HUD and gameplay colors; transient effects use the
active cycle's existing restoration path. No new palette colors were added.

## 8. Level 18 production configuration

Added `LevelConfiguration.Level18Production` and
`Level18WaveSelection`. The production configuration selects Shape 2, palette
cycle 1, four initial Eggs, the shared production ledger, ordinary Level 18
parameters, and the shared actor implementations.

## 9. Eligibility-before-parameters result

Species selection is governed by Level 18 minimum/maximum eligibility before
their parameters are applied. Nonzero or dormant ROM parameters do not enable
an actor. In medium/default data, Spikers and Fuseballs remain ineligible;
their speed/firing data does not instantiate them.

## 10. Supply and physical envelope

Medium/default supply is 21 original enemies. The ROM physical envelope is
seven slots. Tanker admission reserves its two latent cargo Flipper slots in
the shared active-unit accounting; cargo does not increase original supply.

## 11. Species ranges

ROM-backed medium/default ranges are: 0–3 ordinary Flippers, exactly one
Tanker, 2–5 Pulsars, zero Spikers, and zero Fuseballs. The shared selector
honors required minimums, maximums, and physical-slot admission.

## 12. Spike and Spiker absence

Level 18 has no starting Spikes (`wave_spikeht` is zero) and no eligible
Spikers, so the shared Spiker-driven Spike growth path is unreachable. The
implementation does not construct, fire, reconstruct, or otherwise activate a
Level 18 Spiker or Spike.

## 13. Tanker cargo

The Tanker carries two distinct Flipper cargo identities, using the existing
shared cargo ledger. Cargo remains separate from original-enemy supply and
uses the common lifecycle for natural release, shot release, Super Zapper
release, reconstruction, and fresh cargo construction.

## 14. `$0B` route extraction and reuse

Added the semantic `EightAdvanceTurn` movement program and
`TankerCargoMovementProgram.EightAdvanceTurn`. The existing Level 6 `$0B` enum
alias remains available for compatibility. Both ordinary Flippers and cargo
select the shared route rather than maintaining separate algorithms.

## 15. Ordinary Flipper `$0B` behavior

The shared program makes eight along-depth advances, then turns lanes while
holding depth, and repeats. It uses the Level 18 configured actor speed and
existing pivot/transition mechanics.

## 16. Cargo `$0B` behavior

Cargo Flippers use the same eight-advance/held-depth-turn program as ordinary
Flippers. The route is selected through the existing Tanker cargo program
path and applies to the shared cargo lifecycle rather than only one release
case.

## 17. Shared rim handoff

At rim arrival the `$0B` program ends and hands control to shared rim pursuit.
It does not continue a level-specific below-rim route after the rim. Existing
Flipper rim-handoff behavior remains the shared lifecycle.

## 18. Flipper/Tanker speed translation

The Level 18 medium raw speed `$AC` is translated to host climb speed
`2.625f`, using the ratio-preserving project translation documented alongside
the Level 17 speed mapping. This is not a visual tune or a claim of literal
ROM-tick equivalence.

## 19. Firing and shot configuration

Medium/default shot holdoff is 29 ticks (`$1D`); the last slot index `$02`
maps to a maximum of three active shots. Flipper and Tanker firing use the
shared eligibility, depth, timing, random, and capacity gates. Medium Pulsar
firing remains disabled. Shared enemy-shot movement remains three host units
per update; this is the existing project translation, not a new speed claim.

## 20. Shared Pulsar configuration design

`PulsarMovementConfiguration` carries an optional threshold, fixed speed, and
deep-side toward-rim speed. The shared Pulsar actor consumes this configuration
without checking displayed level. A missing threshold preserves the existing
one-speed compatibility behavior; Level 18 supplies its threshold and two
host speeds.

## 21. Level 17 Pulsar compatibility

Level 17 explicitly uses the no-threshold `1f` fixed / `1f` deep-side
configuration. Existing deterministic coverage for one-unit climbing,
update-51 decision cadence, pauses, flips, reversal, shared pulse timing,
collision, identity, and rendering passed.

The full suite initially exposed an unintended change: the first implementation
used the visual `height > 16` pulse-lane predicate as a new collision gate for
threshold-free Pulsars. The default path has been corrected to retain its
previous collision rule; only a configured Level 18 threshold adds a
height-region gate. The existing Pulsar death/re-entry regression now passes.

## 22. Level 18 threshold conversion

ROM threshold `$14` is mapped from the ROM along-coordinate endpoints
`$10`–`$F2` to C# depth 400:

`400 * (0xF2 - 0x14) / (0xF2 - 0x10) = 392.92035f`.

This is a PROJECT TRANSLATION from coordinate endpoints, not a literal ROM
host-height value.

## 23. Level 18 speed-region behavior

For toward-rim movement at or below the host threshold, speed is `21/11`
host units per update. Above the threshold, speed is `1f`. The alternate speed
is derived by preserving the ROM speed ratio against the accepted Level 17
fixed host rate. Lane direction does not select the speed.

## 24. Descending and reversal behavior

Away/deep movement uses the fixed `1f` speed. After descending, Level 18
reverses toward the rim when updated height reaches or crosses the threshold,
or when no generic originals remain pending. Exact threshold equality and
crossing are covered by deterministic tests. Level 17's no-threshold bottom
behavior is preserved.

## 25. Pulse/collision boundary

For Level 18, the ROM threshold also gates the Pulsar pulse-lane collision
region; the existing C# near-rim collision check still has priority. The
translated pulse region overlaps the existing eight-unit rim-collision band.
This overlap is an INFERENCE about the interaction of two implemented
boundaries, not proof of arcade-equivalent collision behavior; manual gameplay
acceptance remains required.

## 26. Pulsar identity and reconstruction

Level 18 Pulsars use the existing original-enemy identity, ledger,
reconstruction, zap, completion, and shared pulse-clock paths. They are not a
new actor subtype. The active Pulsar count participates in Level 18 admission
and pending-supply accounting so the seven-slot envelope remains enforced.

## 27. Progression and direct startup

Natural Level 17→18 progression and direct `--start-level 18` / `--level 18`
startup use the same production chain. Direct startup retains score/lives
carry, Super Zapper initialization, and shared restart behavior.

## 28. Terminal Level 18 boundary

Level 18 is the last supported production level. Its completion remains
terminal; Level 19 is not returned by the production progression resolver and
is rejected by startup/recording validation.

## 29. Files changed

Production:

- `src/Tempest.Game/Gameplay/Flipper.cs`
- `src/Tempest.Game/Gameplay/Pulsar.cs`
- `src/Tempest.Game/Gameplay/Tanker.cs`
- `src/Tempest.Game/Levels/Level18WaveSelection.cs` (new)
- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/Levels/LevelProgression.cs`
- `src/Tempest.Game/Levels/RomPresentationMetadata.cs`
- `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`
- `src/Tempest.Game/Rendering/AtariPresentationCalibration.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `src/Tempest.Game/TempestGame.cs`

Tests:

- `tests/Tempest.Game.Tests/Level18ProductionTests.cs` (new)
- `tests/Tempest.Game.Tests/DebugControllerTests.cs`
- `tests/Tempest.Game.Tests/DirectLevelStartupTests.cs`
- `tests/Tempest.Game.Tests/GameplayRecordingTests.cs`
- `tests/Tempest.Game.Tests/InputDiagnosticTests.cs`
- `tests/Tempest.Game.Tests/Level12ProductionTests.cs`
- `tests/Tempest.Game.Tests/Level13ProductionTests.cs`
- `tests/Tempest.Game.Tests/Level15TubeCandidateTests.cs`
- `tests/Tempest.Game.Tests/Level17ProductionTests.cs`
- `tests/Tempest.Game.Tests/LevelConfigurationTests.cs`
- `tests/Tempest.Game.Tests/RomPresentationMetadataTests.cs`
- `tests/Tempest.Game.Tests/StartLevelTests.cs`

Task records:

- `AI-Logs/Prompts/2026-11-26-level18-bounded-production-implementation-prompt.md`
- This report.

## 30. Tests added or changed

Added focused Level 18 coverage for identity/mapping, eligibility and selection,
slot limits, `$0B` route, cargo lifecycle and release paths, speed/cadence,
Pulsar thresholds, movement regions, reversal, pulse/collision region,
reconstruction, direct startup, progression, palette, presentation, and
terminal behavior.

Updated progression/terminal expectations for Levels 17 and 18. Updated
unsupported-level validation to reject Level 19 rather than newly supported
Level 18. Existing tests were retained as regression checks; no recording
fixture was modified.

## 31. Level 2 presentation regression

The focused compatibility run included Level 2 square, adapter, renderer, HUD,
calibration, and metadata tests. **PASS**; Level 18 reuses the canonical Shape
2 selection and existing presentation.

## 32. Level 6 `$0B` regression

`Level6TankerCargoMovementTests` and the broader Level 6 production/admission
coverage passed. The existing Level 6 `$0B` behavior remains available through
the retained compatibility alias and shared route.

## 33. Level 15 semantic-policy regression

Level 15 Fuseball, terminal Spiker, production soak, and candidate tests passed.
Shared semantic policy wiring did not change Level 15 lifecycle behavior.

## 34. Level 15 seed-1982 result

The Level 15 production-soak/candidate focused regression, including its
seed-1982 scenario, **PASS**. No recording or seed fixture was changed.

## 35. Level 17 focused regression

`Level17ProductionTests`, Level 17 presentation/metadata coverage, and shared
rim-pursuit tests passed. Level 17 remains on its original Shape 1, palette,
wave policy, actor lifecycle, and straight-`$07` Flipper route.

## 36. Level 17 Pulsar fingerprint

The suite's deterministic Pulsar cases passed, including exact one-unit
climbing/clamping, decision on update 51 (not 50), pause expiration, flip
increments, bottom reversal, strict rim/pulse/collision boundaries, and the
181-update shared pulse cycle. The default no-threshold collision behavior was
preserved after the initial full-suite run identified the accidental gate.

## 37. Levels 1–16 recording result

Unmodified `TEMP/GameplayRecordings/lvl1-16.json`: **PASS**, complete playback
at 35,350/35,350 frames, zero remaining frames. Final telemetry: displayed
Level 17, Playing, score 134,039, 9 lives.

## 38. Level 17-origin recording result

Unmodified `TEMP/GameplayRecordings/lvl-17-test.json`: **PASS** for complete
playback at 2,609/2,609 frames, zero remaining frames. Its updated final
telemetry is displayed Level 18, PlayerDying, score 8,175, 8 lives, with five
Pulsars active.

The original task expected the old Level 17 terminal result. Bounded replay
isolated the campaign transition: boundary 1871 is still Level 17 ExitZoom;
boundary 1872 starts Level 18 Approach. The user explicitly authorized
accepting the Level 18 replay result under the new natural Level 17→18 link.
The recording was neither changed nor replaced, and no playback-only
compatibility exception was added.

## 39. Focused test result

**PASS**: the final focused compatibility selection completed with 494 passed,
0 failed, 0 skipped. The follow-up selection covering the initial full-suite
failures, Simulation/Pulsar behavior, and Level 18 completed with 476 passed,
0 failed, 0 skipped. Both used VSTest's explicit five-minute hang timeout.

## 40. Full-suite result

**PASS**: `dotnet test TempestCSharp.slnx --blame-hang-timeout 5min -v minimal`
completed with 2,102 passed, 0 failed, 0 skipped. An earlier full-suite run
found seven failures: six stale Level 17/unsupported-Level-18 test
expectations, plus the default Pulsar collision compatibility regression.
Those cases were corrected narrowly; the complete suite passed afterward.

## 41. Release-build result

**PASS**: fresh non-incremental Release solution build succeeded with 0
warnings and 0 errors.

## 42. `git diff --check`

**PASS** before report creation: no whitespace errors in the tracked diff.
The final report was also written without trailing whitespace.

## 43. Unresolved issues

- Manual gameplay and presentation acceptance remains pending.
- The Level 18 pulse region's overlap with the existing near-rim collision band
  remains an implementation boundary interaction requiring manual review.
- Only the requested medium/default Level 18 configuration is implemented;
  non-default difficulty variants are not claimed.
- The Level 17-origin recording now completes in Level 18, as explicitly
  authorized; its previous Level 17 terminal telemetry is no longer the
  expected result under the new campaign chain.

## 44. Evidence classifications

- **ROM-BACKED:** Level 18 identity/mapping data, canonical row/shape mapping,
  palette index, eligibility, supply, species ranges, slot envelope, cargo
  type, `$0B` route, raw Flipper/Tanker speed, firing/holdoff/capacity data,
  Pulsar raw speed/threshold/pulse values, and Spike/Spiker absence.
- **PROJECT TRANSLATION:** mapping ROM coordinate endpoints to host height;
  scaling Level 18's deep Pulsar speed by the accepted Level 17 fixed-rate
  anchor; host actor speed `2.625f`; shared fixed-step movement and collision
  integration; and use of shared C# identities and lifecycle.
- **PROJECT DECISION:** medium/default scope, shared campaign progression,
  canonical geometry/palette reuse, and using the existing shared actor/state
  systems rather than level-specific actor implementations.
- **INFERENCE:** the translated Level 18 pulse region falls within the current
  near-rim collision band; the interaction still requires manual acceptance.
- **UNKNOWN / PENDING:** actual arcade equivalence of the perceived Level 18
  movement, pulse/collision interaction, presentation, and gameplay feel.

## 45. Manual launch commands

Direct Level 18:

```powershell
dotnet run --project src\Tempest.Game -- --start-level 18 --lives 10
```

Optional overlay comparison:

```powershell
dotnet run --project src\Tempest.Game -- --start-level 18 --lives 10 --background-opacity .5
```

Natural transition from Level 17:

```powershell
dotnet run --project src\Tempest.Game -- --start-level 17 --lives 10
```

These are handoff commands only; the interactive game was not launched.

## 46. Neutral manual observation checklist

- Observe canonical Shape 2 / Level 2 square presentation.
- Observe cycle-1 palette and Level 18 HUD roles.
- Observe ordinary Flipper `$0B` cadence: eight advances, then a held-depth
  lane turn.
- Observe Tanker cargo Flippers using the same `$0B` route.
- Observe Flipper/Tanker climb speed.
- Observe Tanker cargo release behavior.
- Observe Pulsars climbing faster on the deep side and switching near the
  threshold.
- Observe Pulsar descending and reversing behavior.
- Observe Pulsar lane, pulse, and near-rim interaction.
- Observe absence of Spikers, Fuseballs, and starting/dynamic Spikes.
- Observe enemy shooting pressure and shot capacity.
- Observe death, reconstruction, and re-entry.
- Observe Level 18 completion and its terminal boundary.
- Observe natural Level 17→18 transition.
- Observe direct Level 18 startup and restart behavior.
- Observe no obvious regression in Levels 1–17.

## 47. Manual acceptance status

**PENDING.** Automated implementation is complete. Manual gameplay and
presentation acceptance remain pending.

## 48. Final Git/worktree state

Branch remains `master`, tracking `origin/master`; `HEAD` was not changed.
Implementation source/tests and the required task prompt/report are unstaged.
Pre-existing untracked investigation records and protected
`Reference/arcade/MBUCOD.V05.txt` remain unmodified. No protected recordings,
screenshots, videos, ROMs, or user assets were edited.

## 49. Stage, commit, and push

None. No files were staged; no commit or push was made.

## 50. Level 19 and later

No Level 19+ configuration, wave data, or campaign progression was added.
Level 19 remains unsupported and is covered by startup/recording validation.

## 51. Recommended next step after independent results

After Neal independently reports manual observations, address any specifically
authorized findings. Only after explicit manual acceptance should the durable
behavior/evidence documentation be updated and the Level 18 milestone be
closed.
