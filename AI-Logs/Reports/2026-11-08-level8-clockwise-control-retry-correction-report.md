# Level 8 Spinner Direction and Spike Retry Correction

## Status and starting state

Completed the focused correction. HEAD remains
`bc109b4597f83477f5c7998bd3a9e258c72c6edf`. The worktree was already dirty
with the unstaged Level 8 candidate and local untracked Rev 2 ROM files. No
changes were staged, committed, or pushed; the ROM directory and `UserData`
remain untouched. The full user prompt was copied to
`AI-Logs/Prompts/2026-11-08-level8-clockwise-control-retry-correction-prompt.md`
and verified byte-for-byte against the read-only attachment.

## Spinner direction investigation

- `Player.MoveAlongTube` uses positive movement to advance lane index and
  negative movement to decrease it. Mouse delta is multiplied by positive
  spinner sensitivity, so positive mouse delta is the physical clockwise
  convention already used by the accepted closed Levels 1-7.
- In the screen-down coordinate system, positive signed polygon winding means
  increasing lane order is clockwise. The accepted closed Levels 1-7 have
  positive winding. Level 8's authorized Y reflection changes the winding sign:
  increasing lane order is counter-clockwise.
- Before this correction, positive spinner delta still increased Level 8 lane
  indices, so clockwise physical rotation followed the counter-clockwise rim
  path and appeared visually right.
- `Level.ClockwiseLaneIndexDelta` now derives the winding from the complete
  tube outline. `UpdatePlayerMovement` applies it to mouse spinner delta only.
  Keyboard and controller directions remain screen-relative. Canonical lane
  identities are unchanged; enemies, cargo, Spikes, shots, collisions, exit
  lanes, and playback keep their prior lane semantics.
- The open tube's virtual closing chord is used only to calculate orientation;
  playable adjacency still has 15 lanes, no lane 15, and no wrap across the gap.
- No Level 8 vertices or loader transform changed in this task. Existing
  geometry tests still verify all 16 reflected vertices and preserved
  pairwise distances.

## Spike retry investigation

The two reported tests exercise the Level 1 Spiker development scenario, not
production Level 8. That scenario enables runtime `SpikeExitEnabled` while its
configuration uses Standard exit. `UpdateReverseZoom` had incorrectly chosen
the post-death state from `Configuration.ExitPolicy` alone, reconstructing into
Playing instead of the expected AvoidSpikes retry. This was a production
regression, not a stale test expectation. The selection now uses the runtime
Spike-exit mode, preserving the existing diagnostic and earlier configured
AvoidSpikes behavior.

Level 8 remains Standard exit with `SpikeExitEnabled == false`. A death after
the Level 8 wave is resolved returns to Playing; the next completion check
starts Standard ExitDelay, without showing the Avoid Spikes interstitial. The
active exit firing, shot movement/erosion, and Blaster collision behavior
remain intact. The blocked investigation report's ROM trace establishes that
the pre-Level-8 AVOID SPIKES branch is skipped for Level 8 independently of
active exit hazards. The local ROM listing does not establish spinner screen
polarity; the spinner mapping is a project control translation based on the
accepted Levels 1-7 convention and Neal's manual observation.

## Changes

- Production: `src/Tempest.Game/Levels/Level.cs` derives clockwise lane-index
  direction from tube winding; `src/Tempest.Game/TempestGame.cs` maps physical
  spinner delta through that value and chooses retry state using the active
  Spike-exit mode.
- Tests: `tests/Tempest.Game.Tests/Level8ProductionTests.cs` adds geometry-based
  clockwise/counter-clockwise spinner checks for Levels 1-8, Level 8 visible
  direction assertions, open-endpoint clamp/reversal/no-wrap checks, and
  Standard-exit death/retry coverage. The two `SimulationTests` expectations
  remain unchanged.
- Documentation: `TEMPEST-BEHAVIOR-SPEC.md` and
  `TEMPEST-DEVELOPMENT-NOTES.md` now record the rotational invariant, visual
  consequences, lane-identity rationale, and retry-state distinction.
  `README.md` needed no update.

## Validation

- Focused spinner, geometry, movement, input, exit, Spike, and both reported
  SimulationTests: **149 passed, 0 failed**.
- Full solution `dotnet test TempestCSharp.slnx`: **1,062 passed, 0 failed**.
  This includes Levels 1-7, Level 8, enemies/cargo, recording/playback,
  pause/navigation, screenshots, and `--speed` coverage.
- `dotnet build TempestCSharp.slnx`: passed, 0 warnings and 0 errors.
- `git diff --check`: passed.
- Screenshot remains SHA-256
  `425944E92E9CD1A7DFBE5B1B937DF9416E04320E753242D55D50610AB47C8D7E`,
  820,236 bytes. No files under `Reference/arcade/roms.rev2/` or `UserData/`
  were modified.
- Final index is clean; all changes remain unstaged. No interactive game was
  launched.

## Manual retest

Level 8 manual acceptance remains pending. Neal should first test spinner
direction, both endpoints, reversal, and absence of wrapping; only after those
pass should he resume exit firing/Spike testing. Launch command:

```powershell
dotnet run --project src\Tempest.Game -- --level 8 --lives 10
```

This report documents the correction and automated validation; it is not
manual acceptance or authoritative evidence of hidden arcade implementation.
