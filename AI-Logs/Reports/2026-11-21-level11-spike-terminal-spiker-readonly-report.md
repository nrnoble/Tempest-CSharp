# Level 11 Spike-height and terminal-Spiker read-only investigation

Date: 2026-11-21

**Disposition: READ-ONLY INVESTIGATION COMPLETE. No gameplay corrections were implemented.**

## 1. Level 11 Spike-height chain

### ROM/DISASSEMBLY BACKED

- The Level 11 `wave_spikeht` table value is `$B0`. Wave initialization loads `wave_spikeht` and stores that byte directly into all 16 `spike_ht` entries; it does not scale or complement it. See the table and initialization at [Tempest Rom Source code.md](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L2833) and [the `$923A-$9245` initialization](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L2471).
- The stored value is an unsigned 8-bit along-tube coordinate. The Atari player starts at `$10`; a newly admitted enemy starts at `$F0`. Those endpoints define a nominal `$E0`/224-unit tube coordinate span. Spiker growth travels from `$F0` toward `$20` (208 units); when its along coordinate is below the current `spike_ht`, it replaces the stored tip with the smaller coordinate. This is coordinate movement, not signed height arithmetic.
- The Spike-tip coordinate and Spiker coordinate are separate state: `spike_ht[segment]` is the current tip, while `enemy_along[slot]` is the moving Spiker. As the Spiker passes the existing tip, the tip follows its along coordinate. The Spiker clamps/reverses at `$20`, later hops and returns to `$F0`; this cycle is in the shared `$A0F7` program and `$9FC6-$A027` action.
- Collision and routing use those same raw coordinates. The player Spike-hit test compares the same segment's `spike_ht` with `player_along` (`$9881-$989E`); the Flipper route predicate compares `spike_ht` with `enemy_along` (`eact_1c`, `$9C24-$9C3A`). Player-shot Spike collision also compares shot along-position to `spike_ht` (`$A1FA-$A23E`).
- Therefore the initial raw Spike extends from the far endpoint `$F0` to tip `$B0`: `$F0 - $B0 = $40` = 64 raw units. It is **64/224 = 28.57%** of the nominal `$10..$F0` tube-depth domain (or 64/208 = 30.77% of the Spiker's `$20..$F0` growth interval). The byte `$B0` is not a 176-unit Spike length.
- No extra per-shape Spike-height scale or level-specific transformation occurs in the initialization/gameplay path. Tube geometry and projection determine how the same along-depth displacement appears in screen space; the Spike coordinate itself remains a scalar raw along-position. The disassembly does not establish an exact pixel length for Level 11's selected shape.

### CURRENT C# BEHAVIOR

- `Level11WaveSelection.StartingSpikeHeightRaw` is `0xB0`, but `LevelConfiguration.Level11Production` currently sets `StartingSpikeHeight = 0xB0 * Level.Depth / 224f`. With `Level.Depth = 400`, this is **314.2857 C# world-height units**, or **78.57%** of the 400-unit C# depth.
- `Level.Reset` creates a Spike at that value on each playable lane. `VectorRenderer.DrawSpikes` anchors it at the far web and projects the tip at `Position + Depth - spike.Height`; the C# height is therefore the length from the far web, not the raw ROM along-coordinate. C# player/Flipper collision and routing use this same world-height convention.
- Thus current C# is using the Level 8/9 scale denominator but incorrectly using the Level 11 raw coordinate as a length. It produces a value 2.75 times the length implied by the established affine conversion. That quantitatively supports Neal's report that it looks too long. The reported “roughly halfway” is a screen-space observation; perspective and Level 11 geometry mean it is not equivalent to the linear 78.57% world-depth fraction.

### PROJECT TRANSLATION and recommendation

- Existing Level 8/9 C# values use the affine translation `height = (0xF0 - rawAlong) * Level.Depth / (0xF0 - 0x10)`. Level 8's ROM `$C8` becomes 40 raw length units; Level 9's `$C0` becomes 48. The mapping is documented for Level 8 and as a Level 9 project candidate; it is not a claim that raw ROM units are literal rendered pixels. Levels 4-7 use C# height 60 despite differing ROM bytes, and the earlier supply investigation explicitly left that conversion unresolved; they are not a better calibration precedent.
- Applying the same existing Level 8/9 translation to Level 11 gives:

  ```text
  (0xF0 - 0xB0) * 400 / (0xF0 - 0x10)
  = 64 * 400 / 224
  = 114.2857
  ```

- **Justified recommendation:** use the Level 8/9 affine mapping for Level 11, yielding an initial C# Spike height of **114.2857** on each playable lane. This is a **PROJECT TRANSLATION**, not a ROM literal or a new manual arcade acceptance. A later correction should preserve the `$B0` raw evidence and derive the C# length from `$F0 - raw`.
- **MANUAL OBSERVATION:** Neal reports current Level 11 Spikes look too long.
- **UNRESOLVED:** exact arcade-pixel extent of Level 11 initial Spikes and Neal's manual acceptance of the corrected gameplay length.

## 2. Terminal Spiker behavior

### ROM/DISASSEMBLY BACKED

- `wave_enemies` initializes `enemies_pending`; the count decrements when a pending enemy descriptor successfully initializes an active enemy (`$9923-$9944`). It tracks remaining generic entrants, not the number of active enemies or the number of enemies still alive.
- The shared Spiker program grows the Spike, reverses at `$20`, returns/hops, and checks `enemies_pending` at the endpoint (`$A0F7-$A101`, `$9FC6-$A027`). After its hop, it places the same slot at `$F0`. If `enemies_pending != 0`, the slot remains a Spiker and repeats. If the count is zero, the code rewrites that same enemy slot's type/attributes to a Flipper-carrying Tanker. This is neither destruction/replacement nor a new original/pending enemy. The check does not wait for other active enemies to be killed, and it does not use the ordinary Tanker selection cap.
- This is shared Spiker machinery, not a Level-11-specific routine. It applies at the Spiker's endpoint whenever its generic pending supply is exhausted; the level tables determine whether Spikers can be selected in the first place.
- The disassembly has a separate “if no pending, rush to top” check inside the Fuzzball action (`$9F19`). That is not Spiker conversion. C# `HurryUp` is currently latched only for the Fuseball/Pulsar development scenarios, not normal Level 11 production. Neither hurry-up path is the terminal Spiker trigger.

### CURRENT C# BEHAVIOR

- `Spiker.Update` builds to the C# depth limit, retreats to height zero, and marks the object `ReturnedToReserve`; it contains no Tanker conversion. `TempestGame.UpdateSpikers` then selects among gated terminal-conversion branches for Level 4 candidate, Levels 5-6 production, and Levels 7-9. There is no Level 11 condition.
- Level 11 does use a production ledger, so `PendingGenericOriginalCount` accurately identifies original entries in Reserve/Egg state. However, the local `pendingGenericOriginals` variable in `UpdateSpikers` is only populated for Level 7/9; all Level 11 terminal-conversion predicates are false. The fallback removes the Spiker and calls `ReturnOriginalToReserve`; `Level3WaveLedger.ReturnSpikerToReserve` resets its selected type to `Unselected` and puts the same identity in Reserve.
- Later hatch is a new selection, not an in-place conversion. With exactly two Spikers active, when one returns the active count falls to one; Level 11's `MinimumSpikers = 2` immediately selects Spiker again. This deterministically explains repeated Spiker selection in the two-Spiker case. With three active, the remaining two meet the minimum, but Spiker remains eligible below its maximum of three; the seeded/random selector can choose Spiker again, and other active Tanker/latent-cargo limits can further narrow the alternatives. This permits repeated Spiker cycles without guaranteeing them for every random sequence.
- If all other original supply has been committed, the ledger pending count can be zero at the Spiker endpoint. Because Level 11 has no conversion branch, returning the Spiker to Reserve creates a new pending identity; it can then be admitted again. This is not a case where active Spikers are incorrectly counted as “pending” for conversion: the conversion policy is simply absent for this level.
- Level 11's active-enemy gate (six active, seven physical slots) and ordinary Tanker/Flipper/Fuseball limits apply to hatching/selection. They do not execute a terminal Spiker conversion. A future ROM-aligned terminal conversion should be an in-place special path, not ordinary Tanker selection: one Spiker becomes one Tanker, retains its original identity, adds the existing two latent cargo identities, and does not add generic supply. Active enemies—including a live Fuseball—do not keep Atari's `enemies_pending` nonzero once admitted; reserve/Egg originals do.
- The all-rim completion check requires no reserve/Eggs, no unresolved ledger originals/cargo, and no active enemies. A returned Spiker remains unresolved, so this path cannot make the wave disappear or complete around it. The all-rim rule is separate from terminal conversion.

### Historical evidence

- The Level 4/5 implementation reports initially called terminal conversion a candidate/approximation, not accepted Atari fact. Later durable Level 4/6/8/9 records classify the conversion trigger from ROM evidence and distinguish C# identity/accounting and retreat-boundary translations from literal ROM behavior.
- The Level 7 repeated-cycle investigation confirms the shared ROM endpoint and zero-pending conversion, but its manual observation concerned repeated firing, not manual validation of terminal conversion.
- The current Level 11 implementation report documents selection/admission and the recovered Spike parameter, but not a Level 11 terminal conversion. Neal's supplied observation is about the current C# game; no Level 11 arcade observation of this endpoint was available in this read-only task.

## 3. Scope classification

**LEVEL-11-SPECIFIC.** The Atari mechanism is global shared Spiker behavior, but current C# terminal conversion is wired for the Level 4 candidate, Levels 5-6, and Levels 7-9—not Level 11. The Level 11 selection policy is present, but its terminal Spiker lifecycle is not connected to the conversion path. No Level 4-9 behavior needs to be changed to address this Level 11 gap.

Relevant current gates: [TempestGame.UpdateSpikers](../../src/Tempest.Game/TempestGame.cs#L1511), [Level11WaveSelection](../../src/Tempest.Game/Levels/Level11WaveSelection.cs#L7), and [Level3WaveLedger.ReturnSpikerToReserve](../../src/Tempest.Game/Levels/Level3WaveLedger.cs#L240).

## 4. Implementation readiness

| Issue | Readiness | Smallest evidence-supported correction | Focused regression tests |
|---|---|---|---|
| Level 11 initial Spike length | **READY FOR BOUNDED CORRECTION** | Derive the C# height from `(0xF0 - StartingSpikeHeightRaw) * Level.Depth / 224f`, yielding 114.2857; do not alter presentation calibration or raw ROM data. | Assert Level 11's configured and initialized Spike heights on each playable lane; retain Level 8/9 values and verify the `$F0`/`$10` conversion endpoints. |
| Terminal Spiker-to-Tanker | **READY FOR BOUNDED CORRECTION** | Add a Level-11-only zero-`PendingGenericOriginalCount` terminal conversion using the existing ledger conversion and Tanker construction at the existing C# return boundary. Preserve the existing C# endpoint as a project approximation; do not route through ordinary Level 11 selection caps. | Deterministically test no conversion while generic Reserve/Egg originals remain; after pending originals reach zero, test same-ID Spiker-to-Tanker conversion, two latent cargo IDs, unchanged original supply, conversion despite other active enemies, and no false all-rim completion. Confirm Level 4-9 regressions remain unchanged. |

Both corrections remain subject to Neal's manual Level 11 gameplay acceptance after an authorized implementation. No interactive game was launched here; no tests/builds were run because static source and the shared ROM listing established the requested mechanics without requiring dynamic observation.

## Read-only scope and repository state

- Read the tracked Tempest disassembly, current source, relevant tests, durable behavior/development notes, and previous Level 4-9/Level 11 reports.
- Did not inspect local ROM contents, `MBUCOD.V05.txt`, screenshots, images, video, `UserData`, or `Temp`.
- Created only the requested AI-Logs prompt/report; production source, tests, configuration, durable project documentation, reference material, the existing uncommitted Level 11 candidate, and other starting work were preserved. No index, branch, or commit state was changed.
- No test/build/game launch, staging, commit, or push was performed.
