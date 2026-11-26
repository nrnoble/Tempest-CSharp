# Level 19 Bounded Production Implementation Report

Date: 2026-11-26

## 1. Executive summary

Implementation of medium/default displayed Level 19 is in progress as a bounded
candidate. The implementation reuses the canonical Shape 3 route, cycle-1
palette, shared EightAdvanceTurn Flipper route, shared Pulsar threshold policy,
generic Tanker cargo/ledger/reconstruction systems, natural Level 18-to-19
progression, direct startup, and terminal Level 19 stopping. Level 19 remains
without manual acceptance.

## 2. Starting Git/worktree state

- Branch: `master`
- HEAD at implementation start: `9a8f788a41e42ea578fb1dcad90dd74a9a3cc60b`
- Upstream: `origin/master`
- Ahead/behind: `0/0`
- Index: clean
- Tracked modifications: none
- Existing untracked paths:
  - `AI-Logs/Prompts/2026-11-26-level19-rom-configuration-readonly-investigation-prompt.md`
  - `AI-Logs/Reports/2026-11-26-level19-rom-configuration-readonly-investigation-report.md`
  - `Reference/arcade/MBUCOD.V05.txt`
- Protected TEMP, UserData, Videos, ROM/reference, and recording material was
  preserved and not modified.

## 3. Authorities read

- `AI-Logs/Reports/2026-11-26-level19-rom-configuration-readonly-investigation-report.md`
- `AI-Logs/Reports/2026-11-26-level18-final-closeout-report.md`
- `AI-Logs/Reports/2026-11-26-level18-bounded-production-implementation-report.md`
- `AI-Logs/Reports/2026-11-26-level18-pulsar-movement-translation-investigation-report.md`
- `TEMPEST-BEHAVIOR-SPEC.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `.github/copilot-instructions.md`
- Existing Level 17/18 production, presentation, actor, and test implementations.

No raw ROM copy, private scratchpad, recording, screenshot, or Internet source
was used.

## 4. Level 19 identity/mapping

- Displayed level: 19
- Progression index: 18
- ROM `curlevel`: `$12`
- Effective one-based `lev`: `$13`
- Cycle: 2
- Cycle-local position: 3
- Zero-based remap index: 2
- ROM shape row: 2
- Board index: 2
- Palette index: 1
- Medium/default production policy
- Terminal production boundary

## 5. Canonical Shape 3 reuse

Level 19 references `LevelConfiguration.Level3Mixed.Definition` and retains the
existing 16-lane closed Shape 3 model. It does not copy geometry or create a
bespoke Level 19 polygon.

## 6. Production presentation reuse

Displayed Level 19 resolves ROM row 2 to canonical first-cycle Shape 3 and reuses
the existing Level 3 model frame, projection, and calibration path.

## 7. Cycle-1 palette reuse

Level 19 uses the existing palette index 1 semantic resolver, including the
cycle-1 red tube, cyan score, green reserves, red HUD level, yellow player,
magenta Flippers, blue Tankers, yellow Eggs, cyan Pulsars, and transient pulse
restoration.

## 8. Production configuration

`LevelConfiguration.Level19Production` supplies the canonical configuration,
with no target-level ledger or actor identities.

## 9. Eligibility-before-parameters

Species eligibility is expressed in `Level19WaveSelection` before parameters or
actors are admitted. Dormant Spiker/Fuseball/Spike values do not instantiate
actors.

## 10. Supply and physical envelope

- Original supply: 22
- Maximum physical enemy units: 7
- Initial Eggs: 4
- Maximum active Eggs: 4
- Maximum approaching Eggs: 2
- Reserve hatch interval: 50 ticks

## 11. Species ranges

- Flippers: 0 minimum, 3 maximum
- Tankers: exactly 1
- Spikers: 0/0
- Pulsars: 2 minimum, 3 maximum
- Fuseballs: 0/0

## 12. Spike/Spiker absence

Level 19 has no starting or dynamic Spike path and no eligible Spiker or
Spiker projectile. The implementation leaves the Spike-resumption boundary at
Level 20 untouched.

## 13. Tanker cargo

Exactly one Tanker carries two distinct Flipper cargo identities outside
original supply. Release, reconstruction, Super Zapper, and completion remain
on generic shared ledger/lifecycle systems.

## 14. `$19`/`EightAdvanceTurn` reuse

The ROM `$19` movement byte resolves semantically to the existing
`EightAdvanceTurn` below-rim program.

## 15. Ordinary Flipper behavior

Ordinary Flippers use eight depth advances, a held-depth lane turn, and repeat
until generic rim pursuit takes over.

## 16. Cargo behavior

Tanker cargo Flippers use the same semantic `EightAdvanceTurn` route.

## 17. Shared rim handoff

Below-rim ownership ends at rim arrival and enters the existing shared rim
pursuit lifecycle.

## 18. Flipper/Tanker speed

Raw `$A9` translates to host 2.71875 for both Flipper climb and Tanker climb
as a **PROJECT TRANSLATION**.

## 19. Firing policy

Medium/default firing uses holdoff 29 and three active enemy-shot slots.
Flippers and Tankers remain eligible; medium Pulsar firing remains disabled.

## 20. Shared Pulsar configuration

Level 19 supplies threshold `$14`, fixed host speed 1, and deep-side host ratio
87/55 to the shared parameterized Pulsar.

## 21. Level 17 compatibility

Level 17 retains its existing no-threshold compatibility configuration.

## 22. Level 18 compatibility

Level 18 retains its existing threshold/speed configuration and gains natural
progression to Level 19.

## 23. Level 19 threshold/speed behavior

Host threshold is approximately 392.92035; equality and deeper positions use
87/55 toward-rim movement, near-rim movement uses 1, and descending movement
uses fixed 1.

## 24. Pulsar firing

Medium/default Pulsar firing remains disabled.

## 25. Pulsar identity/reconstruction

Pulsar identity, reconstruction, pulse clock, and completion remain shared.

## 26. Progression/direct startup

Natural Level 18 completion progresses to Level 19. Direct `--start-level 19`
and `--level 19` resolve through `LevelProgression` to the same configuration.

## 27. Terminal boundary

Level 19 is the terminal production level. Level 20 remains unsupported.

## 28. Recording implications

Recording implications and unmodified recording outcomes are recorded after
validation.

## 29. Files changed

Production:

- `src/Tempest.Game/Levels/Level19WaveSelection.cs`
- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/Levels/LevelProgression.cs`
- `src/Tempest.Game/Levels/RomPresentationMetadata.cs`
- `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`
- `src/Tempest.Game/TempestGame.cs`

Tests and boundary regressions:

- `tests/Tempest.Game.Tests/Level19ProductionTests.cs`
- Direct startup, debug parsing, configuration, recording, presentation metadata,
  and Level 12/13/15/17/18 regression tests updated for the 18→19 terminal boundary.

Required AI artifacts:

- `AI-Logs/Prompts/2026-11-26-level19-bounded-production-implementation-prompt.md`
- `AI-Logs/Reports/2026-11-26-level19-bounded-production-implementation-report.md`

## 30. Tests added/changed

Added permanent Level 19 mapping, eligibility, selector, cargo, route, speed,
Pulsar, presentation, palette, and terminal-boundary coverage. Updated
Level 18 and shared startup/recording/presentation assertions so Level 19 is
supported and Level 20 remains unsupported.

## 31. Level 3 presentation regression

**PASS:** `Level19ProductionTests.PresentationAndPaletteResolveThroughCanonicalShapeThree`
verifies Shape 3 reuse and matching Level 3 calibration.

## 32. Level 6 route regression

**PASS:** the focused Level 17–19 gate included existing shared route coverage
and completed 115/115.

## 33. Level 15 semantic-policy regression

**PASS:** focused startup and existing Level 15 semantic-regression coverage
completed successfully within the Level 17–19 gate and full suite.

## 34. Level 15 seed-1982

**PASS:** included by the full solution suite; no deterministic change was
required for Level 19.

## 35. Level 17 Pulsar regression

**PASS:** the Level 17 production and compatibility tests passed in the focused
gate and full suite.

## 36. Level 18 Pulsar/progression regression

**PASS:** Level 18 production, threshold-aware Pulsar, presentation, and
progression coverage passed. Level 18 now naturally proceeds to Level 19.

## 37. Levels 1-16 recording result

**PASS:** unmodified `TEMP/GameplayRecordings/lvl1-16.json` completed all
35,350 frames, with 0 remaining frames and `RecordingCompleted`. Final
telemetry remained displayed Level 17, Playing, score 134,039, 9 lives.

## 38. Level 17-origin recording result

**PASS:** unmodified `TEMP/GameplayRecordings/lvl-17-test.json` completed all
2,609 frames, with 0 remaining frames and `RecordingCompleted`. The accepted
natural result is displayed Level 18, `PlayerDying`, score 8,175, 8 lives,
with 5 Pulsars, 1 Tanker, and 1 cargo Flipper. No recording modification or
Level 19 playback compatibility behavior was added.

## 39. Focused test result

**PASS:** 115 tests passed, 0 failed, 0 skipped, with VSTest and an explicit
5-minute hang timeout.

## 40. Full-suite result

**PASS:** 2,110 tests passed, 0 failed, 0 skipped, with an explicit 5-minute
VSTest hang timeout.

## 41. Release-build result

**PASS:** fresh non-incremental Release build succeeded with 0 warnings and
0 errors.

## 42. `git diff --check`

**PASS:** `git diff --check` reported no whitespace errors.

## 43. Unresolved issues

No implementation issue remains. Manual gameplay/presentation acceptance is
pending and no arcade acceptance is claimed.

## 44. Evidence classifications

- ROM-BACKED: Level 19 mapping, shape row, palette, supply/ranges, movement
  byte, speeds, firing, Tanker cargo, and Spiker/Fuseball/Spike absence.
- PROJECT TRANSLATION: C# threshold/height and speed mapping.
- PROJECT DECISION: canonical Shape 3 and cycle-1 reuse, terminal campaign policy.
- UNKNOWN: no arcade/manual acceptance is claimed yet.

## 45. Manual launch commands

```text
dotnet run --project src\Tempest.Game -- --start-level 19 --lives 10
dotnet run --project src\Tempest.Game -- --start-level 19 --lives 10 --background-opacity .5
dotnet run --project src\Tempest.Game -- --start-level 18 --lives 10
```

## 46. Neutral observation checklist

- Observe canonical Shape 3 / Level 3 presentation.
- Observe cycle-1 palette and Level 19 HUD.
- Observe ordinary Flipper and cargo Flipper EightAdvanceTurn cadence.
- Observe Level 19 Flipper/Tanker climb speed.
- Observe one Tanker with two Flipper cargo identities.
- Observe Pulsar threshold, descending, reversal, lane, pulse, and rim behavior.
- Observe absence of Spikers, Fuseballs, and Spikes.
- Observe Flipper/Tanker firing pressure and shot capacity.
- Observe death/re-entry and reconstruction.
- Observe natural Level 18-to-19 transition.
- Observe direct Level 19 startup and restart.
- Observe terminal Level 19 with no Level 20.
- Observe no obvious regression in Levels 1-18.

## 47. Manual acceptance status

Pending. No interactive game was launched and no manual acceptance is claimed.

## 48. Final Git/worktree state

Final branch is `master`, tracking `origin/master`, with no staged changes.
The implementation is present as tracked modifications plus the new Level 19
source/test files and required AI-Logs. Existing protected
`Reference/arcade/MBUCOD.V05.txt` remains untracked and untouched.

## 49. Stage/commit/push

None.

## 50. Level 20+ untouched

Level 20 and later levels remain unimplemented and unsupported.

## 51. Recommended next step

Run the exact launch commands, independently compare the observations, and
report manual acceptance or a specific discrepancy.
