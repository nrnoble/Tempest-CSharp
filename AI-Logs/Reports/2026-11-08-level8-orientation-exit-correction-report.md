# Level 8 Orientation and Exit Correction

## Status

Implemented the screenshot-supported Level 8 Y reflection and ROM-backed active
exit behavior. All changes remain unstaged. The interactive game was not
launched. Manual acceptance by Neal is pending.

## Evidence and decisions

- Used `UserData/Screenshots/Level8-orientation.png` for orientation only. Its
  SHA-256 remained
  `425944E92E9CD1A7DFBE5B1B937DF9416E04320E753242D55D50610AB47C8D7E`
  (820,236 bytes).
- Reflected centered Y only. This puts both open endpoints above the short
  connector while preserving all 16 points, point order, relative distances,
  angles, and open topology. X reflection does not correct the vertical
  inversion; a 180-degree rotation also swaps source endpoint identities.
- Reused the established ROM evidence for active exit firing, moving shots,
  Spike interaction, and Blaster/Spike collision. Level 8 remains on Standard
  exit; active hazards do not enable the Avoid Spikes interstitial.

## Changes

- `src/Tempest.Game/Levels/LevelLoader.cs`: applied the Y reflection to Level 8.
- `src/Tempest.Game/TempestGame.cs` and
  `src/Tempest.Game/Rendering/VectorRenderer.cs`: routed active ExitZoom
  through the shared Spike-aware exit simulation; enabled normal shot cooldown,
  capacity and audio, shot movement/rendering, Spike erosion, and current-lane
  collision. A death after the wave is resolved retries Standard exit rather
  than entering the Avoid Spikes presentation state.
- `tests/Tempest.Game.Tests/Level8ProductionTests.cs`,
  `ExitLaneControlTests.cs`, `Level4CandidateTests.cs`,
  `LevelProgressionTests.cs`, and `SimulationTests.cs`: added geometry,
  playback, firing, lane, endpoint, collision, death/re-entry, and terminal
  coverage; updated assertions for authorized active-exit firing and the normal
  three-tick cooldown.
- `TEMPEST-BEHAVIOR-SPEC.md` and `TEMPEST-DEVELOPMENT-NOTES.md`: recorded the
  evidence classification, transform, exit behavior, and pending acceptance.

## Validation

- `dotnet build TempestCSharp.slnx`: passed, 0 warnings and 0 errors.
- Earlier focused runs passed: Level 8 tests 22/22; exit/playback regressions
  87/87. These preceded the final expectation updates.
- A full test run after enabling the new behavior executed 1,051 tests: 1,048
  passed and three failed against old expectations (Level 8 completion after
  a remaining Spike, shots absent during active exit, and long Spike survival
  under continuous firing). Those tests were updated to cover the corrected
  behavior.
- Subsequent focused reruns were blocked by Windows Application Control when
  loading `Tempest.Game.Tests.dll` (`0x800711C7`), including from a fresh
  temporary artifacts directory. The final test updates therefore remain
  unverified by a completed post-update test run.
- `git diff --check`: passed. The screenshot hash and size were rechecked.

## Remaining work

Neal should manually verify the reflected tube, active-exit firing and shot
movement, Spike erosion in the firing lane, death/re-entry on a remaining
Spike, and safe completion. Launch command:

```powershell
dotnet run --project src\Tempest.Game -- --level 8 --lives 10
```

Do not treat this report or the automated approximation as manual gameplay
acceptance or authoritative arcade behavior.
