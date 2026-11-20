# Level 8 orientation and exit investigation

Date: 2026-11-08
Result: **BLOCKED — no production changes made.** Local ROM/code evidence confirms the exit defects, but does not establish a defensible screen-orientation correction.

## Starting state and scope

- HEAD: `bc109b4597f83477f5c7998bd3a9e258c72c6edf` (`Close out accepted interactive speed diagnostic`).
- The existing Level 8 candidate was unstaged and preserved. It included the previously reported production, test, and documentation changes.
- `Reference/arcade/roms.rev2/` was already untracked reference material. It was read only.
- Neal's first blind launch found the Level 8 shape approximately 90% correct, with an orientation mismatch, and found that the Blaster could pass a Spike during exit and could not fire during exit. Full Level 8 acceptance remains pending.
- The complete task was copied to [the paired prompt record](../Prompts/2026-11-08-level8-orientation-exit-spike-correction-prompt.md); its SHA-256 matched the supplied task and it contains exactly one standalone completion token.
- The preimplementation, Rev 2 geometry continuation, and implementation recovery reports were read. The continuation report's shape-row-7 values and open topology remain the geometry authority.

## Orientation investigation

### Evidence

- The Rev 2 tables select shape row 7 for displayed Level 8. Its 16 raw coordinate pairs are recorded in the continuation report; `lev_open[7]=$FF` gives 15 consecutive lanes and no closing lane.
- The ROM drawing setup copies `lev_x` and `lev_y` to the tube arrays. Its vector drawing routine centers both bytes by XOR with `$80`, computes consecutive-point deltas, and passes X and Y as separate signed coordinates to `vapp_ldraw_A,X`. The listing documents that helper's A register as X and X register as Y. No axis swap, reflection, or fixed rotation appears in that ROM drawing path.
- The current [Level 8 loader](../../src/Tempest.Game/Levels/LevelLoader.cs) likewise subtracts `$80` from each axis, applies the project scale `25/14`, and preserves point order.
- The current [Projection.Project](../../src/Tempest.Game/Rendering/Projection.cs) maps positive X right and positive Y down. [DisplayConfiguration](../../src/Tempest.Game/Rendering/DisplayConfiguration.cs) applies no presentation rotation.
- The ROM listing defines `lev_angle` only as 16 sectors over 360 degrees. Its Level 8 sequence is `$0D` for the left slant, `$00` for the middle segment, and `$03` for the right slant. The listing does not specify whether those sectors increase clockwise or counterclockwise, so this sequence cannot independently establish the screen polarity of ROM Y.
- The ROM code and current C# projection establish the transforms actually applied, but the permitted local sources do not establish how positive vector-generator Y is oriented on the arcade display/cabinet. The raw point data and current projection therefore do not distinguish a direct Y mapping from a Y reflection. No quarter-turn or other presentation transform is supported by the inspected ROM path.

### Conclusion

The observed orientation cannot be corrected defensibly from the available local ROM data and C# projection alone. Selecting a reflection or rotation would be a guess, contrary to the task's blind-evidence requirement. No geometry, renderer, or orientation test was changed. A screenshot or an explicit orientation description from Neal is required before choosing a correction.

## Exit, firing, and Spike trace

### ROM behavior

- The Rev 2 `state_20` byte sequence was verified in `136002-113.d1` at offset `$072A`. It moves the player, calls the tube-exit movement/collision path, calls `player_fire`, and calls `move_shots`.
- `player_fire` uses the ordinary player-shot slots and firing checks. `move_shots` invokes the player-shot/Spike check. The Spike check compares the shot's segment and along-position to `spike_ht`, shortens or clears the Spike, and updates the shot's hit count.
- The exit movement path compares the Blaster's along-position against the Spike in its current segment. A remaining Spike crossed on that lane triggers the existing death path before successful exit.
- The `AVOID SPIKES` decision at `$A5FB-$A60F` is an interstitial/presentation branch: it is skipped when `curlevel >= $07`. Level 8 has `curlevel=$07`. This condition does not disable the `state_20` firing, shot movement, or exit Spike collision.
- Relevant bytes were read from the local Rev 2 CPU ROM images and agree with the annotated listing. No ROM file was written.

### C# behavior and cause

- The current [Level 8 configuration](../../src/Tempest.Game/Levels/LevelConfiguration.cs) correctly uses `LevelExitPolicy.Standard`; making it `AvoidSpikes` would conflate active exit hazards with the pre-Level-8 interstitial.
- In [TempestGame](../../src/Tempest.Game/TempestGame.cs), `SpikeExitEnabled` is limited to the development Spike wave or `AvoidSpikes` policy. Standard exit begins in `ExitDelay`, and `BeginExit` clears existing shots.
- During standard `ExitZoom`, the main update path calls only `UpdateExitZoom` and `UpdatePlayerMovement`. It does not call `UpdatePlayerFiring`, `UpdatePlayerShots`, or the Spike/player collision check. The existing specialized Spike-exit update does those operations, but the Level 8 standard policy does not enter that path.
- This is a shared exit-state seam: the C# currently couples the optional `AVOID SPIKES` policy to active Spike-exit simulation. The ROM makes those behaviors independent. The trace confirms the reported failures and supports separating the interstitial policy from active exit firing, shot/Spike interaction, and Spike collision.
- No gameplay code or tests were changed while the orientation correction remained blocked.

## Files and validation

- Created this blocked investigation report and the paired prompt record under ignored `AI-Logs/`.
- No production files, tests, durable behavior/development documentation, ROMs, screenshots, recordings, or unrelated files were modified.
- No tests or build were run because no source change was made. No interactive game was launched.
- No staging, commit, push, or `git clean` was performed.
- Level 8 remains partially tested and **MANUAL ACCEPTANCE PENDING**.

## Required next step

Provide a screenshot of the correct arcade Level 8, or otherwise clarify its displayed orientation, so the C# transform can be selected from evidence rather than guessed. After that, implement and test the orientation correction and the ROM-confirmed active-exit behavior, then run the requested full validation.

The eventual manual retest command remains:

```text
dotnet run --project src\Tempest.Game -- --level 8 --lives 10
```
