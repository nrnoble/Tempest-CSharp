# Level 17 rail, Tanker, and Egg palette refinement

Date: 2026-11-25

Status: **PASS - bounded rendering correction complete; Neal manual visual acceptance remains pending**

## 1. Executive summary

The Level 17 cycle palette now restores ordinary radial rails to red after Super Zapper inactive or completed states instead of retaining hard-coded blue. Cycle 1 Tankers resolve blue and cycle 1 Eggs resolve yellow through the rendering palette resolver. Cycle 0 remains unchanged.

No tube geometry, Shape 1 projection, HUD geometry, gameplay state, enemy lifecycle, timing, or Level 18+ behavior was changed.

## 2. Starting Git/worktree state

- Branch: `master`
- Tracking: `origin/master`
- Ahead/behind: `0/0`
- HEAD: `fde09aa31af760958f656aa553555462d51792a0`
- Existing tracked and untracked candidate state was preserved.
- The index was empty for this task, and no reset, clean, restore, stash, stage, commit, or push was performed.

## 3. Gameplay/projection/HUD freeze confirmation

PASS. The accepted Shape 1 values and prior HUD, Flipper, and Pulsar mappings were not changed. The existing candidate worktree remained under the same gameplay freeze constraints.

## 4. Manual rail-color evidence

ARCADE-OBSERVED: Neal established that the ordinary Level 17 tube/web is predominantly red and that mixed blue/yellow rails are not a correct ordinary base presentation.

## 5. Manual Tanker-blue evidence

ARCADE-OBSERVED: Neal confirmed the specifically supplied MAME Tanker crop is blue. The existing C# vector blue is used as the PROJECT TRANSLATION host RGB.

## 6. Manual Egg-yellow evidence

ARCADE-OBSERVED: Neal established that Level 17 Eggs are yellow. The existing C# `Color.Yellow` vector is used as the PROJECT TRANSLATION host RGB.

## 7. Tube-rail color-path inventory

- Outer perimeter and rim connections: `DrawTube`, cycle tube base color.
- Radial/depth rails: `DrawTube`, player-lane boundaries or Super Zapper-selected rail role.
- Player lane: `ResolvePlayerLaneColor`.
- Pulsar lanes: existing Pulsar pulse overlay remains separate from base rail selection.
- Super Zapper: `SuperZapperSystem.TubeColorIndex` with ROM-backed sequence.
- Active/transient rail roles: existing white, player, Spiker, red, Pulsar, Tanker, and slot 6 role colors.
- Normal unoccupied rail fallback: previously slot 6, hard-coded blue.
- Tankers: main entity draw path in `VectorRenderer.DrawTankers`.
- Eggs: `VectorRenderer.DrawEggs`.

## 8. Rail-color priority order

1. Player-lane boundary highlight, while gameplay presentation is active.
2. Active Super Zapper slot role.
3. Selected cycle tube base role for inactive slot 6/restoration.
4. Rim connections use the selected cycle tube base role.

Pulsar lane overlay remains governed by existing pulse drawing behavior and does not alter the base fallback.

## 9. Cause of ordinary blue rails

Confirmed: `SuperZapperSystem.TubeColorIndex` returns inactive slot `6`, and `ResolveSuperZapperRailColor` translated slot `6` to `Color.Blue`. Every radial rail used that fallback whenever Super Zapper was inactive, bypassing the cycle-1 red base.

## 10. Cause of ordinary yellow rails

The supplied screenshot's yellow rails are explained by the active player-lane highlight, which intentionally uses the existing accepted Blaster-lane yellow role. This is a legitimate current-state lane override, not evidence for making every ordinary rail yellow.

## 11. Legitimate transient rail overrides

- Super Zapper slots retain their existing sequence and host-role translations.
- Player-lane boundaries retain the cycle Blaster-lane color.
- Existing Pulsar/pulse behavior remains unchanged.
- Temporary red/white/green/cyan/magenta/blue slot roles remain bounded to active flash state.

## 12. Incorrect base/fallback paths

- The inactive Super Zapper slot `6` was incorrectly treated as hard-coded blue instead of the selected cycle tube base.
- No additional incorrect ordinary rail inheritance was found in the traced base tube path.

## 13. Rail correction

`ResolveSuperZapperRailColor` slot `6` now resolves `CyclePaletteResolver.ResolveTubeColor(displayedLevel)`.

- Palette 0: inactive/restored rail Blue.
- Palette 1: inactive/restored rail Red.

## 14. Cycle-base restoration behavior

After inactive or completed Super Zapper flash state, radial rails restore through the selected cycle tube resolver. Level 17 restores Red; Level 1 retains Blue.

## 15. Tanker renderer trace

The main Tanker glyph is emitted by `VectorRenderer.DrawTankers`, with normal rendering supplied the local `TankerColor` constant at the call site. The cycle resolver is now used by that call and Super Zapper's Tanker-role slot.

## 16. Tanker color correction

Added `ResolveTankerColor(int displayedLevel)`:

- Palette 0 / Levels 1-16: Magenta, unchanged.
- Palette 1 / Levels 17-32: Blue.

This is palette-index-based, not displayed-level `==17`.

## 17. Egg renderer trace

Egg rendering was fixed in `VectorRenderer.DrawEggs` to red regardless of cycle. The Egg object and lifecycle state do not contain rendering identity or species color decisions.

## 18. Egg color correction

Added `ResolveEggColor(int displayedLevel)`:

- Palette 0 / Levels 1-16: Red, unchanged.
- Palette 1 / Levels 17-32: Yellow.

The public Egg draw path now passes the resolved render color while existing geometry and two-argument test overload behavior remain available.

## 19. Ordinary/reconstructed Egg behavior

Ordinary and reconstructed/cargo Egg objects share `VectorRenderer.DrawEggs`; all read the cycle palette at draw time. No Egg state, identity, reserve, reconstruction, or hatch field was changed.

## 20. Compact blue object/Tanker distinction

The compact blue diamond-like vector remains unresolved. It was not reclassified as a Tanker. Tanker identity remains based on the supplied MAME crop and the existing renderer/source Tanker glyph path.

## 21. Files changed

- `src/Tempest.Game/Rendering/CyclePaletteResolver.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `tests/Tempest.Game.Tests/Level17ProductionTests.cs`
- `AI-Logs/Prompts/2026-11-25-level17-rail-tanker-egg-palette-refinement-prompt.md`
- `AI-Logs/Reports/2026-11-25-level17-rail-tanker-egg-palette-refinement-report.md`

## 22. Tests added/changed

Added Level 17 tests for:

- Base radial rail red in cycle 1 with legitimate player-lane yellow override.
- Ordinary and reconstructed/cargo Egg objects resolving cycle-1 yellow.
- Egg lifecycle state remaining unchanged.
- Tanker cycle-0/cycle-1 resolver behavior.
- Egg cycle-0/cycle-1 resolver behavior.
- Super Zapper completion restoring cycle-1 red and cycle-0 blue.

The existing projection, HUD, Flipper, Pulsar, Super Zapper, and cargo tests remained in the focused/full gates.

## 23. Shape 1 projection regression result

PASS. No Shape 1 projection, model-frame, calibration, or adapter code changed.

## 24. HUD/Flipper/Pulsar regression result

PASS. The existing focused suite retained:

- HUD cyan/green/red mappings.
- Flipper magenta.
- Pulsar cyan/white pulse behavior.

No HUD or enemy gameplay code was changed.

## 25. Level 17 gameplay-freeze validation

PASS. No gameplay state, enemy supply, movement, firing, cargo, Egg lifecycle, Tanker, Pulsar, Spiker, Spike, random, ledger, reconstruction, completion, or progression code was changed.

## 26. Level 17 recording result

PASS:

- `dotnet run --project src\Tempest.Game -- --headless-playback TEMP/GameplayRecordings/lvl-17-test.json`
- 2609/2609 ticks
- `RecordingCompleted`
- remaining frames `0`
- final Level 17 ExitZoom
- score `5956`
- lives `6`

## 27. Levels 1-16 recording result

PASS at the established campaign boundary:

- `dotnet run --project src\Tempest.Game -- --headless-playback TEMP/GameplayRecordings/lvl1-16.json`
- 35350/35350 ticks
- `RecordingCompleted`
- remaining frames `0`
- accepted score `134039`
- lives `9`
- direct candidate then continues into Level 17 Playing, as prior candidate behavior requires

## 28. Focused test result

PASS: 68 focused Level 17, Blaster-lane, and Super Zapper tests passed.

## 29. Full-suite result

PASS: 2064 tests passed, 0 failed, 0 skipped.

## 30. Release-build result

PASS:

- 0 warnings
- 0 errors

## 31. `git diff --check`

PASS. Git emitted an existing line-ending warning for `src/Tempest.Game/Levels/LevelLoader.cs`; no whitespace error was reported.

## 32. Remaining unresolved color roles

- The compact blue diamond-like vector remains unresolved.
- Exact non-role RGB values not established by the supplied evidence remain outside this task.
- No speculative shot, Spike, or Spiker cycle-1 change was made.

## 33. Manual-validation matrix

Neal should use:

```powershell
dotnet run --project src\Tempest.Game -- --start-level 17 --lives 10
```

and:

```powershell
dotnet run --project src\Tempest.Game -- --start-level 17 --lives 10 --background-opacity .5
```

Verify ordinary unoccupied rails are predominantly red, player-lane yellow remains bounded, active transient rails restore to red, Tankers are blue, and Eggs are yellow.

## 34. Final Git/worktree state

- Branch remains `master`
- HEAD remains `fde09aa31af760958f656aa553555462d51792a0`
- Existing tracked/untracked candidate state remains preserved.
- No task files were staged.

## 35. Stage/commit/push

None.

## 36. Level 18+

Untouched.

## 37. Recommended next step

Neal should perform the two supplied Level 17 manual visual comparisons and confirm the red base rail, blue Tanker, yellow Egg, and bounded transient/player-lane overrides.
