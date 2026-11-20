# Production Level 8 implementation recovery

Date: 2026-11-08
Result: **AUTOMATED VALIDATION PASSED — MANUAL ACCEPTANCE PENDING**

## Recovery context and starting state

The prior Level 8 implementation request failed with HTTP 408 after substantial
work had already been written. This recovery preserved and inspected that
unstaged candidate rather than restarting or restoring it.

- Expected baseline and actual HEAD:
  `bc109b4597f83477f5c7998bd3a9e258c72c6edf`
  (`Close out accepted interactive speed diagnostic`).
- Branch: `master...origin/master`.
- Initial candidate included 21 modified tracked files, the untracked
  `Level8WaveSelection.cs` and `Level8ProductionTests.cs`, and the pre-existing
  untracked `Reference/arcade/roms.rev2/` reference directory.
- Initial `git diff --check` had already passed. Line-ending warnings were
  informational and were not normalized.
- No Level 8 implementation report from the failed session existed. The two
  investigation reports and the saved original implementation prompt were
  read and retained unchanged.
- The complete recovery task was copied verbatim to
  `AI-Logs/Prompts/2026-11-08-level8-implementation-recovery-prompt.md`.
  Its SHA-256 matches the attached user-provided task.

## Work inherited and recovery changes

The inherited candidate already contained the Level 8 geometry/configuration,
open-topology changes, endpoint movement, renderer update, Level 8 wave and
cargo paths, natural progression/direct startup, terminal handling, and
focused production tests.

Recovery work:

- Completed the Level 8 Flipper type-cap accounting: active/released Flippers
  and two latent Flipper slots per active Tanker now reserve against the
  ROM-confirmed maximum of five before another Tanker is selectable. The
  Level 7 selector remains unchanged.
- Strengthened renderer assertions to verify the 15 open rim segments, all 16
  depth rails, absence of the false closing edge, and the closed-tube closing
  edge.
- Extended Level 8 playback coverage through headless startup, visible reverse
  reconstruction, pause/forward stepping, speed-diagnostic state comparisons,
  and screenshot output naming.
- Updated README and both durable behavior/development documents to record
  Level 8 as a ROM-derived candidate, distinguish historical checkpoints from
  current state, and retain the manual-acceptance boundary.
- Created this recovery report. Neither it nor the prompt is staged; `AI-Logs/`
  is Git-ignored.

## Requirement status

| Status | Requirements |
|---|---|
| **COMPLETE** | Rev 2 shape row 7; 16 ordered vertices; bounded `25/14` C# geometry transform; open topology with 15 playable lanes; no lane 15 or endpoint wrap; Blaster clamping; topology-aware Egg/Flipper/Pulsar/Fuseball movement; Tanker cargo endpoint correction; open/closed renderer behavior; supply 24; six-active gate; Tanker 1/2 and Spiker 2/4; no ordinary Flippers, Pulsars, or Fuseballs; two Tanker cargo identities; generic death/re-entry; final-pending Spiker conversion; Level 7 -> 8 progression; `--level 8`; terminal Level 8 without Level 9; Super Zapper setup; recording/playback startup and controls. |
| **COMPLETE, PROJECT TRANSLATION** | The deterministic post-minimum Level 8 selector. It preserves seeded behavior and enforces the confirmed minima, maxima, active gate, and Flipper cargo maximum. The exact ROM Spike-height/random-start choice sequence is deliberately not represented as ROM-equivalent behavior. |
| **COMPLETE, PROJECT TRANSLATION** | Unsigned Tanker cargo split: ROM-equivalent `$1F` selects `$07`; `$20` and `$21` select `$19`. Natural below-`$20` cargo uses straight `$07`; deep player-shot cargo uses the repeating Level 8 move/flip `$19` route. The `$19` route does not use Level 7's 2/3 reversal program or a Spike test. |
| **COMPLETE, PROJECT TRANSLATION** | ROM starting Spike `$C8` maps to approximately 71.428571 C# height on lanes 0-14. Existing C# growth/erosion behavior remains; ROM position-driven erosion is not claimed as equivalent. |
| **CORRECTED DURING RECOVERY** | The inherited selector did not reserve the two latent cargo Flippers when deciding whether another Tanker fit under the Flipper maximum of five. Level 8 selection now counts active/released and latent Flipper units before allowing another Tanker. |
| **PARTIAL / MANUAL ACCEPTANCE PENDING** | Geometry screen scale/placement, blue C# tube color, initial Blaster lane 7, Spike-height translation, `$19` visual quality, and gameplay presentation. These remain project choices until Neal's blind first launch. |
| **UNVALIDATED MANUALLY** | All visual and gameplay acceptance, including the first comparison of the blind geometry, enemy/cargo behavior, Spike presentation, progression, terminal completion, and interactive recording/playback. The game was not launched. |
| **INTENTIONALLY OUT OF SCOPE** | Level 9, external/MAME/screenshot/video comparison, exact ROM-to-C# speed conversion, and the exact ROM post-minimum species-selection sequence. |

No requested production Level 8 feature remains identified as missing. Automated
tests establish C# behavior only and do not establish arcade equivalence.

## Implementation details and evidence boundaries

### Geometry, topology, and rendering

The Level 8 definition selects Rev 2 shape row 7, keeps the ROM point ordering,
and subtracts `$80` from raw X/Y before applying `25/14`. It has 16 vertices,
`Continuous=false`, and 15 lanes. `CenterY=300`, `YOffset=0`, and the initial
player lane are project presentation/startup choices. ROM angles and
presentation values remain documented evidence; they are not asserted as
literal C# pixels or RGB values.

`Level` exposes playable-lane validation and topology-aware adjacency.
Open endpoints have no outward neighbor; movement that reverses at an
endpoint selects the valid inward lane. Blaster movement stops at both ends.
Tanker cargo preserves two identities and valid children at endpoints.
Rendering uses 16 longitudinal rails, 15 near-rim edges, and 15 far-rim edges.
Closed Levels 1-7 keep their previous modulo/wrap behavior.

### Wave policy, Tankers, and Flippers

Level 8 uses Medium/default supply 24 and a six-active-unit ordinary creation
gate. Ordinary entries are Tankers and Spikers only. The bounded selector
enforces Tanker 1/2, Spiker 2/4, no ordinary Flippers, and Flipper maximum
five including released Flippers and two latent cargo slots for each active
Tanker. Pulsars and Fuseballs remain absent. The selector's stable seeded
choice after minima is explicitly a C# project translation.

The cargo boundary converts C# height back to the ROM-equivalent along value
before comparing with `$20`; equality belongs to `$19`. Natural Tanker release
occurs below `$20` and follows `$07`. Deep player-shot destruction follows
`$19`, whose repeating sequence is one along interval then one directed flip.
The existing C# enemy-firing cadence is retained; the Rev 2 59-update value is
not treated as a validated 59-C#-tick conversion.

The starting Spike conversion uses:

```text
height = (0xF0 - rawAlong) * 400 / (0xF0 - 0x10)
```

Final-pending Spiker conversion retains the same original identity and adds
Tanker cargo without increasing generic supply. Level 8 death reconstruction
uses the generic policy: unresolved species/program selection is cleared,
latent cargo disappears with a lost Tanker, released unresolved cargo
re-enters generically, and resolved results remain resolved.

### Progression and playback

Natural Level 7 completion advances into production Level 8 through the
existing approach; score/lives carry and fresh Super Zapper charges are
installed. Generic direct startup resolves `--level 8` to the same production
configuration. Level 8 is terminal; the existing restart returns to fresh
Level 1 and no Level 9 is loaded.

Recording schema v2 remains unchanged. Level 8 is exercised through recording
startup, headless playback, visible reverse reconstruction, pause/forward
stepping, screenshot output, and playback-speed diagnostic comparisons.
Interactive `--speed` remains separate from and absent from the recording
schema.

## Files changed

Production and project documentation:

- `README.md`
- `TEMPEST-BEHAVIOR-SPEC.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `src/Tempest.Game/Gameplay/Egg.cs`
- `src/Tempest.Game/Gameplay/Flipper.cs`
- `src/Tempest.Game/Gameplay/Fuseball.cs`
- `src/Tempest.Game/Gameplay/Player.cs`
- `src/Tempest.Game/Gameplay/Pulsar.cs`
- `src/Tempest.Game/Gameplay/Tanker.cs`
- `src/Tempest.Game/Levels/Level.cs`
- `src/Tempest.Game/Levels/Level8WaveSelection.cs` (new)
- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/Levels/LevelLoader.cs`
- `src/Tempest.Game/Levels/LevelProgression.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `src/Tempest.Game/TempestGame.cs`

Tests:

- `tests/Tempest.Game.Tests/CampaignSoakTests.cs`
- `tests/Tempest.Game.Tests/DebugControllerTests.cs`
- `tests/Tempest.Game.Tests/DirectLevelStartupTests.cs`
- `tests/Tempest.Game.Tests/GameplayPlaybackSessionTests.cs`
- `tests/Tempest.Game.Tests/GameplayRecordingTests.cs`
- `tests/Tempest.Game.Tests/HeadlessGameplayPlaybackTests.cs`
- `tests/Tempest.Game.Tests/InputDiagnosticTests.cs`
- `tests/Tempest.Game.Tests/Level7ProductionTests.cs`
- `tests/Tempest.Game.Tests/Level8ProductionTests.cs` (new)
- `tests/Tempest.Game.Tests/LevelConfigurationTests.cs`
- `tests/Tempest.Game.Tests/LevelProgressionTests.cs`
- `tests/Tempest.Game.Tests/PlaybackScreenshotCaptureTests.cs`
- `tests/Tempest.Game.Tests/VisiblePlaybackControllerTests.cs`

The untracked `Reference/arcade/roms.rev2/` directory is pre-existing
read-only reference material, not part of the Level 8 implementation.

## Validation

- Focused Level 8 production tests: **16 passed**.
- Focused Level 8 and playback seams: **100 passed**.
- Expanded Level 8/open-topology, progression, rendering, cargo, recording,
  playback, Super Zapper, and closed-level regression set: **674 passed**.
- `dotnet test TempestCSharp.slnx`: **1,045 passed, 0 failed**.
- `dotnet build TempestCSharp.slnx`: **succeeded, 0 warnings, 0 errors**.
- `git diff --check`: **passed**. Git printed only the pre-existing LF/CRLF
  conversion warnings; no line-ending normalization was performed.
- Level 1-7 closed-topology and regression tests passed as part of the 674
  expanded tests and the full suite.
- All 21 Rev 2 ROM files match the continuation report's names, sizes, and
  SHA-256 fingerprints. No ROM file was written or staged.
- The saved recovery prompt's SHA-256 matches its source attachment.

## Final Git state

- Branch remains `master`; HEAD remains
  `bc109b4597f83477f5c7998bd3a9e258c72c6edf`.
- Implementation changes remain unstaged; the index is empty.
- `Reference/arcade/roms.rev2/` remains untracked as before.
- `AI-Logs/` is ignored. No commit, push, or `git clean` was performed.
- The interactive game was not launched.

## Manual handoff

Neal's first Level 8 launch remains the blind visual acceptance test. Suggested
commands:

```powershell
dotnet run --project src\Tempest.Game -- --level 8 --lives 10
dotnet run --project src\Tempest.Game -- --level 8 --lives 10 --speed 0.75
dotnet run --project src\Tempest.Game -- --lives 10
dotnet run --project src\Tempest.Game -- --record-gameplay Temp\GameplayRecordings\level8-candidate.json --level 8 --lives 10
dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\level8-candidate.json
dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\level8-candidate.json --speed 2
```

Manual checklist:

- Judge the ROM-derived geometry, scale, placement, color, open endpoints, and
  absence of a false closing edge.
- Traverse all lanes; verify spinner direction and Blaster clamping.
- Confirm ordinary Tanker/Spiker entry and absence of bottom Flippers,
  Pulsars, and Fuseballs.
- Observe two Tanker cargo Flippers, valid endpoint lanes, natural straight
  `$07` movement, and deep shot-destroyed `$19` move/flip behavior.
- Judge starting Spike height, Spiker growth, Spike erosion, enemy firing,
  player shots, destruction effects, score, sound, and Super Zapper.
- If practical, check death/re-entry, natural Level 7 -> 8 progression,
  Level 8 completion, and restart to Level 1 (no Level 9).
- Record a fresh Level 8 run; check visible/headless playback, pause/resume,
  forward stepping, reverse scrubbing, screenshots, and slower/faster
  `--speed`.

No arcade/manual acceptance is claimed. Remaining presentation questions are
the visual geometry scale/placement/color, initial lane, Spike-height
translation, visible `$19` behavior, and whether C# movement/firing timing
needs any future evidence-backed translation.
