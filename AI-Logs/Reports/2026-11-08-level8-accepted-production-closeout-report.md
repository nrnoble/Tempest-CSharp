# Production Level 8 accepted closeout

Date: 2026-11-08
Result: **CLOSED — Neal manually accepted production Level 8.**

## Starting state and evidence reviewed

- Branch: `master`.
- Starting HEAD and `origin/master`: `bc109b4597f83477f5c7998bd3a9e258c72c6edf`
  (`Close out accepted interactive speed diagnostic`).
- Initial index was clean. The unstaged candidate contained 30 modified
  tracked files, the new `Level8WaveSelection.cs` and
  `Level8ProductionTests.cs`, and the local untracked Rev 2 ROM directory.
  The candidate was preserved; Level 8 was neither restarted nor reimplemented.
- Read the ROM preimplementation and Rev 2 geometry investigations, the
  implementation recovery report, and the orientation/exit and spinner/retry
  correction reports. These are historical evidence, not replacements for the
  final manual acceptance below.
- The complete supplied closeout prompt is preserved at
  `AI-Logs/Prompts/2026-11-08-level8-accepted-production-closeout-prompt.md`.

The accumulated candidate closes the ROM-derived shape-row-7 implementation:
16 ordered vertices, 15 open playable lanes, no closing edge, accepted
screen-Y reflection, winding-aware physical spinner mapping, and endpoint
clamping/reversal without wrapping. It includes Medium/default supply and
species limits, Tanker cargo/type-cap accounting, the `$07`/`$19` cargo
boundary and `$19` move/flip route, starting Spikes, generic death/re-entry,
natural Level 7-to-8 progression, direct `--level 8`, terminal Level 8, and
active-exit firing, shot movement, Spike interactions, collision, and retry.
Recording/playback, headless/visible playback, screenshot, stepping, and
`--speed` compatibility are covered. No Level 9 work was started.

Evidence classifications remain separate: raw Rev 2 geometry and parameter
bytes stay ROM data; decoded paths remain ROM-listing-backed; normalized C#
coordinates, Y reflection, palette/placement, Spike-height conversion, and
clockwise lane-index mapping remain project translations. Manual acceptance
does not convert those translations into literal Atari hardware constants.

## Manual acceptance

Neal reports that:

- Level 8 passes; the ROM-derived shape, corrected orientation, open topology,
  and endpoint presentation are accepted.
- Clockwise spinner input moves the Blaster clockwise and visually toward the
  left endpoint; counter-clockwise input moves it counter-clockwise and
  visually toward the right. Endpoint clamping/reversal work, with no wrap
  across the opening.
- Level 8 gameplay, active-exit firing and shot movement, shot-versus-Spike
  interaction, exit Spike hazards, and corrected retry behavior pass. No
  blocking Level 8 gameplay issue remains.
- An existing Levels 1-7 recording played correctly. Pause/resume,
  forward/reverse stepping, and `--speed` had no observed regression.

No separate manual result for the natural Level 7-to-8 transition is claimed;
automated progression tests cover that handoff. No additional manual test is
required by this closeout.

## Durable documentation

Updated `README.md`, `TEMPEST-BEHAVIOR-SPEC.md`, and
`TEMPEST-DEVELOPMENT-NOTES.md` to replace current pending-acceptance wording
with the final acceptance, keep historical checkpoint language historical,
record accepted gameplay/control/exit behavior and playback regressions, and
preserve the ROM-versus-project-translation distinctions.

## Final validation

Focused Level 8 and shared regression tests:

```text
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --filter "FullyQualifiedName~Level8ProductionTests|FullyQualifiedName~LevelConfigurationTests|FullyQualifiedName~LevelProgressionTests|FullyQualifiedName~ExitLaneControlTests|FullyQualifiedName~Level4CandidateTests|FullyQualifiedName~Level7ProductionTests|FullyQualifiedName~SimulationTests" --verbosity minimal
Passed: 451; failed: 0.
```

Expanded Level 8 and Levels 1-7 regression coverage included configuration,
geometry, progression, campaign soak, movement, Flippers, Tankers, Spikers,
wave ledger, death/re-entry, completion, Super Zapper, direct startup,
recording/playback, visible/headless playback, pause/stepping, screenshot,
input diagnostics, and speed diagnostics:

```text
dotnet test TempestCSharp.slnx --filter "FullyQualifiedName~Level8ProductionTests|FullyQualifiedName~LevelConfigurationTests|FullyQualifiedName~LevelProgressionTests|FullyQualifiedName~CampaignSoakTests|FullyQualifiedName~Level3GeometryTests|FullyQualifiedName~Level3WaveLedgerTests|FullyQualifiedName~Level3MixedWaveTests|FullyQualifiedName~Level3TestModeTests|FullyQualifiedName~Level4CandidateTests|FullyQualifiedName~Level5ProductionTests|FullyQualifiedName~Level6ProductionTests|FullyQualifiedName~Level7FlipperMovementTests|FullyQualifiedName~Level7ProductionTests|FullyQualifiedName~SimulationTests|FullyQualifiedName~SuperZapperTests|FullyQualifiedName~DirectLevelStartupTests|FullyQualifiedName~GameplayRecordingTests|FullyQualifiedName~GameplayPlaybackTests|FullyQualifiedName~GameplayPlaybackSessionTests|FullyQualifiedName~HeadlessGameplayPlaybackTests|FullyQualifiedName~VisiblePlaybackControllerTests|FullyQualifiedName~PlaybackScreenshotCaptureTests|FullyQualifiedName~InteractiveSpeedOptionsTests|FullyQualifiedName~InputDiagnosticTests|FullyQualifiedName~ExitLaneControlTests|FullyQualifiedName~ArcadeFlipperFiringTests|FullyQualifiedName~PlayerShotRenderingTests|FullyQualifiedName~Level2CanFlipTests|FullyQualifiedName~Level2SquareTests|FullyQualifiedName~GameplayInputTests|FullyQualifiedName~InputSystemTests" --verbosity minimal
Passed: 865; failed: 0.
```

- Full suite, `dotnet test TempestCSharp.slnx`: **1,062 passed, 0 failed**.
- Build, `dotnet build TempestCSharp.slnx`: **passed, 0 warnings, 0 errors**.
- Unstaged `git diff --check`: **passed**.
- Staged `git diff --cached --check`: **passed**.
- Committed `git diff HEAD^ HEAD --check`: **passed**.
- No interactive game was launched.

## Staging allowlist and exclusions

The staged allowlist contained exactly these 32 reviewed files:

```text
README.md
TEMPEST-BEHAVIOR-SPEC.md
TEMPEST-DEVELOPMENT-NOTES.md
src/Tempest.Game/Gameplay/Egg.cs
src/Tempest.Game/Gameplay/Flipper.cs
src/Tempest.Game/Gameplay/Fuseball.cs
src/Tempest.Game/Gameplay/Player.cs
src/Tempest.Game/Gameplay/Pulsar.cs
src/Tempest.Game/Gameplay/Tanker.cs
src/Tempest.Game/Levels/Level.cs
src/Tempest.Game/Levels/Level8WaveSelection.cs
src/Tempest.Game/Levels/LevelConfiguration.cs
src/Tempest.Game/Levels/LevelLoader.cs
src/Tempest.Game/Levels/LevelProgression.cs
src/Tempest.Game/Rendering/VectorRenderer.cs
src/Tempest.Game/TempestGame.cs
tests/Tempest.Game.Tests/CampaignSoakTests.cs
tests/Tempest.Game.Tests/DebugControllerTests.cs
tests/Tempest.Game.Tests/DirectLevelStartupTests.cs
tests/Tempest.Game.Tests/ExitLaneControlTests.cs
tests/Tempest.Game.Tests/GameplayPlaybackSessionTests.cs
tests/Tempest.Game.Tests/GameplayRecordingTests.cs
tests/Tempest.Game.Tests/HeadlessGameplayPlaybackTests.cs
tests/Tempest.Game.Tests/InputDiagnosticTests.cs
tests/Tempest.Game.Tests/Level4CandidateTests.cs
tests/Tempest.Game.Tests/Level7ProductionTests.cs
tests/Tempest.Game.Tests/Level8ProductionTests.cs
tests/Tempest.Game.Tests/LevelConfigurationTests.cs
tests/Tempest.Game.Tests/LevelProgressionTests.cs
tests/Tempest.Game.Tests/PlaybackScreenshotCaptureTests.cs
tests/Tempest.Game.Tests/SimulationTests.cs
tests/Tempest.Game.Tests/VisiblePlaybackControllerTests.cs
```

- `Reference/arcade/roms.rev2/` remains local and untracked. All 21 expected
  filenames, sizes, and SHA-256 values match the existing geometry report;
  zero ROM files were staged or committed.
- `UserData/`, including
  `UserData/Screenshots/Level8-orientation.png`, gameplay recordings, Videos,
  root screenshots, temporary output, and generated build output were excluded.
  The orientation screenshot remains 820,236 bytes with SHA-256
  `425944E92E9CD1A7DFBE5B1B937DF9416E04320E753242D55D50610AB47C8D7E`.
- `AI-Logs/` is ignored and excluded from the commit. The prompt and this new
  report remain local; historical reports were not rewritten.

## Commit and final repository state

- Commit: `d5ccbe1d3ef8af21a02bd53c8c465ed0c4908fef`
- Subject: `Close out accepted production Level 8`
- Body: `Record Neal's manual acceptance and preserve the ROM evidence classifications while closing the production campaign milestone.`
- Co-author trailer: `Co-authored-by: Copilot <223556219+Copilot@users.noreply.github.com>`
- Push: `git push origin master` succeeded.
- Local `HEAD`, local `origin/master`, and live remote `master` all equal
  `d5ccbe1d3ef8af21a02bd53c8c465ed0c4908fef`.
- Final tracked worktree and index are clean. The only visible untracked
  material is the excluded Rev 2 ROM directory.
- No unresolved issue remains in the accepted Level 8 scope. Production Level
  8 is manually accepted and closed; accepted Levels 1-8 form the campaign
  foundation. Level 9 remains out of scope.
