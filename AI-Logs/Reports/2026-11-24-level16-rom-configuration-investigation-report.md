# Tempest Level 16 ROM / Configuration Investigation

**Status: INVESTIGATION COMPLETE — IMPLEMENTATION NOT AUTHORIZED OR STARTED**

## 1. Executive summary

Level 16 is ROM table row 11, a **closed, 16-lane tube**. The bounded Rev. 2
K1 ROM read gives a highly unusual shape: points 0 and 8 are the same central
coordinate, splitting the ordered outline into two lobes that meet at that
point. Their signed areas cancel exactly. This is a material C# architecture
gap: `Level` derives a single polygon winding from shoelace area and rejects a
zero-area, non-collinear outline. A Level 16 `LevelDefinition` alone cannot
currently initialize successfully without addressing that geometry/winding
assumption.

The ROM-backed ordinary wave supply is `$1B` (27); hard difficulty adjusts it
to 30, while easy leaves it at 27. The species table calls for at least one
(up to two) Tankers, 2–3 Spikers, and one Fuseball, with maximums of 5 Flippers,
2 Tankers, 3 Spikers, and 1 Fuseball. Pulsars are unavailable and ordinary
Flippers have a zero minimum, so the ROM picker does not select them. Flippers
still appear as Tanker cargo: all four Tanker load choices are `$01`, meaning
a pair of Flippers.

The below-rim Flipper program is the explicit ROM movement program `$24`, not
a generic `canflip` setting: it alternates two flips one way and three the
other, reversing between groups. The cargo Flippers are initialized through
the same Flipper program selection. Spikers grow/update lane Spikes, turn at
the `$20` along threshold, hop lanes near `$F2`, and can convert their original
identity into a Flipper-carrying Tanker when no original enemies remain pending.

Most identity, cargo, reconstruction, and completion concepts already have
shared C# representations. However, Level 16 needs its own ROM-backed wave
policy/configuration and progression/startup wiring, a geometry-winding
solution, and explicit translation decisions for ROM timing/coordinates. The
current per-level firing-capacity translation also needs careful review:
ROM `$enm_shotmax = $02` is used as the last slot index, which scans slots
0–2, not as a literal count of two.

No Level 16 behavior was directly observed in an arcade. No tests/builds were
run; this was a read-only investigation. No production or test files were
changed. The only created files are this report and its paired prompt.

## 2. Evidence sources and classifications

### ROM-BACKED

- `Reference/arcade/Tempest Rom Source code.md`: Rev. 2 wave-parameter tables,
  parameter loader, enemy selection, Tanker/Spiker lifecycle, Flipper p-code,
  firing and completion routines.
- A bounded read of the Level 16 row in
  `Reference/arcade/roms.rev2/136002-118.k1`: the 16 X, 16 Y, and 16 angle
  bytes, Level 16 remap byte, and seven associated presentation metadata bytes.
  The read was limited to the exact row/metadata offsets documented below.

### Existing PROJECT DECISIONS / TRANSLATIONS

- C# fixed-step simulation, deterministic recording/playback, Egg/reserve
  pacing, `Level3WaveLedger` identity representation, and generic Tanker cargo
  accounting are project mechanisms, not literal ROM structures.
- Existing generic Spiker-to-Tanker conversion uses the same original identity
  and allocates two cargo identities. The behavior specification explicitly
  classifies its C# height-zero conversion point as a project translation of
  the ROM `$F2`/`$F0` event, rather than literal coordinate/timing fidelity.
- Existing C# raw Spike-to-world-height mapping is a project translation; the
  ROM `$A0` value is not itself a C# height.

### ARCADE-OBSERVED

- No Level 16 arcade behavior was observed or manually compared as part of this
  investigation.

### REFERENCE-DERIVED

- Existing Fuseball and Tanker implementations contain Incitti-derived
  engineering behavior. That is not treated as proof of the Level 16 arcade
  result.

### Unresolved

- No evidence here settles Level 16's exact perceived speed, first Flipper
  handedness, C# conversions for all raw movement values, or manually accepted
  vector presentation.
- The ROM geometry's zero signed area conflicts with the current C# winding
  requirement. A project-approved translation is needed; do not silently
  invent a winding sign.

## 3. ROM-backed Level 16 tube geometry

The Rev. 2 mapping vector ends with displayed Level 16 -> table row `$0B`
(zero-based row 11). The current C# mapping catalog already records that
mapping, and the bounded K1 read independently returned `$0B` for the
displayed-Level-16 remap entry. The geometry is 16 ordered vertices:

```text
X: 80 70 48 20 10 20 48 70 80 90 B8 E0 F0 E0 B8 90
Y: 80 57 48 57 80 A9 BA A9 80 57 48 57 80 A9 BA A9
Angle: 0B 09 07 05 03 01 0F 0D 0D 0F 01 03 05 07 09 0B
```

Coordinates are hexadecimal ROM bytes in source order. Point 0 and point 8
are both `($80,$80)`. The segments in the first half close back to that point;
the second half forms the other lobe and returns to it. This is a self-touching
two-lobed outline, not an ordinary simple polygon. The independent bounded
shoelace calculation returned twice-signed-area `0`; the two lobes have
equal/opposite signed area.

`lev_open` row 11 is `$00` (closed), so the ROM topology is 16 playable
sectors with cyclic adjacency, including the final-to-first connection. The
shared C# lane-index topology can represent that cyclic connectivity. It must
not infer that the repeated coordinate removes a lane or breaks the ROM's
index ordering.

The source declares 16-point X/Y/angle arrays and 16-entry remap/metadata
arrays (`Tempest Rom Source code.md`, around lines 7460–7480). The binary
reads used the `$B800`-based 2 KiB K1 image layout: row 11 X at offset `0x22C`,
Y at `0x32C`, angles at `0x42C`; displayed level 16 remap at `0x48B`;
metadata at `0x497`, `0x4A7`, `0x4B7`, `0x4C7`, `0x4D7`, `0x4E7`, and `0x4F7`.
Only those 56 bytes were read.

**C# gap:** `LevelDefinition` supports ordered points and closed topology, but
the `Level` constructor calculates one polygon winding. Its helper throws for
non-collinear zero-area points; Level 16 therefore fails construction after
the ROM geometry is translated, unless that winding dependency is explicitly
resolved. A bounded geometry change must preserve the ROM lane order and test
both spinner handedness and lane-transition/pivot calculations. Do not replace
the ROM shape with a convenient simple polygon.

## 4. Enemy composition and supply

The ROM `wave_enemies` table's Level 16 entry is `$1B` (27 original wave
identities). `set_enm_and_spikes` copies that value to `enemies_pending`.
Difficulty handling is explicit:

- Medium/default: 27.
- Easy: 27; the easy branch adjusts the enemy-shot index and Flipper speed,
  not `wave_enemies`.
- Hard: 30. The hard routine adds the value shifted right three bits, including
  the carry produced by the shifts; for `$1B`, that increment is 3 (not a
  conventional mathematical round-up).

Level 16 type limits from the `$9607` ranges:

| ROM type | Minimum | Maximum | Level 16 result |
|---|---:|---:|---|
| Flipper | 0 | 5 | No ordinary/original Flipper selection; Tanker cargo Flippers are permitted. |
| Pulsar | 0 | 0 | Must not appear. |
| Tanker | 1 | 2 | Tanker supply is required; at most two ordinary Tankers. |
| Spiker | 2 | 3 | At least two; maximum three. |
| Fuzzball / C# Fuseball | 1 | 1 | Exactly one when admitted; maximum one. |

The ROM picker tries types with nonzero minima. Consequently, the Flipper
maximum does not make ordinary Flippers eligible when the minimum is zero.
This distinction does not prohibit Flippers released by Tankers.

The raw `max_enm` parameter is `$06` at all levels. The ROM has seven enemy
records indexed 0 through 6. Do not silently interpret the raw index as a
six- or seven-actor C# cap; the C# implementation must distinguish physical
records from active-unit admission and cargo occupancy.

## 5. Spike configuration

`wave_spikeht` for Level 16 is raw `$A0`. At wave setup the ROM writes that
same value into all 16 `spike_ht` entries (indices 15 down through 0). Since
this tube is closed with 16 playable sectors, the initial configuration is
16 lane Spikes, each at raw along value `$A0`.

The value is ROM along-coordinate data, not a C# world-space height. Existing
project code has a conversion convention, but Level 16 must not inherit a
Level 15 configuration merely because its raw byte matches. The conversion
and any timing/height relationship need to be recorded as a project
translation and validated independently.

The `$24` Flipper p-code contains no Spike-sensitive branch. Ordinary shared
Spike collision, shot-shortening, persistence, and vector drawing may be
reusable once Level 16 lane geometry is represented. Spikers still interact
with these initial Spikes through their separate ROM routine.

## 6. Flipper movement program

`flipper_move` is `$24` for Level 16. The ROM loads this explicit program for
Flippers; it is not a generic `canflip` flag. The p-code at `$A0F7 + $24`
executes:

1. Move by the Flipper speed once.
2. Set a two-flip counter; flip twice, moving between flips except on a tick
   that starts a flip.
3. Reverse direction.
4. Set a three-flip counter; flip three times with the same move-between-flips
   cadence.
5. Reverse direction and repeat the two/three groups.

The ROM's own comment describes “twice one way, three times the other” and
repetition. This establishes below-rim lane changes and alternating reversal.
It does not establish a fixed initial clockwise/counterclockwise handedness;
that is selected elsewhere. The current C# default `+1` is an explicit project
choice, not a ROM fact.

Flipper speed raw is `$A0` at Level 16. The `$24` code is already represented
by the C# repeating-24 route (`[2,3]` flip counts) used by another ROM-backed
level. This is evidence for reusing the shared p-code translation, not for
copying another level's configuration or initial direction.

## 7. Tanker and cargo behavior

The normal Tanker movement entry is p-code `$07` (“move per speed”). The
per-level `tanker_load+2` and `tanker_load+3` parameters are both `$01` at
Level 16; the two base `tanker_load` bytes are also set to `$01`. All four
possible Tanker load choices therefore mean “split into two Flippers.”

When a Tanker reaches the `$20` threshold, the ROM attempts to create two
Flipper cargo units on the two adjacent sides of the source segment (the
second segment is computed by adding two modulo 16). The second child gets
the opposite direction bit. The cargo Flippers use the ordinary Flipper
initialization path, which selects `flipper_move`; Level 16 cargo therefore
uses the same `$24` movement program as ordinary Flippers would, if any were
selected. Tankers themselves enter on `$07`.

The ROM has a seven-record physical enemy array and performs two child
allocations on split. Admission and release must preserve room for this
derived pair and distinguish it from the original Tanker identity. There is
no separate Level 16 cargo-routing parameter beyond the ROM load type.

## 8. Spiker behavior and lifecycle

Level 16 has 2–3 Spikers. Their ROM p-code is the common Spiker entry:
move by Spiker speed, update/grow the Spike in the current segment, reverse
at along `$20`, and continue away. The Level 16 Spiker speed raw is `$00`;
that byte is not a C# speed. The ROM routine also invokes a lane hop near
`$F2`, choosing among the 16 sector Spike heights before continuing.

When the returning Spiker reaches the far-end threshold and
`enemies_pending == 0`, the ROM changes that same enemy slot into a Tanker
whose cargo directive is `$01` (two Flippers). It is not simply a terminal
Spiker that disappears. The original identity remains unresolved through the
conversion, and the resulting Tanker cargo creates two further obligations.
If pending originals remain, the Spiker does not take this terminal
conversion branch.

The current C# ledger already supports converting an active Spiker original
to a Tanker and allocating its two cargo identities. `TempestGame` currently
gates the production conversion predicate to existing level policies; Level 16
would need an explicit policy path. There is also a fidelity boundary:
`Spiker.RestartForPendingSupply` documents a project translation that does not
invent the ROM lane hop. For Level 16, that accepted shared translation must be
preserved unless Neal authorizes revisiting it; report the ROM hop vs C#
translation rather than silently changing it.

The ROM marks Spiker/Tanker/Flipper entities as shot-eligible. This is distinct
from the Spiker's Spike-building routine. Fuseballs are not marked as shooters.

## 9. Fuseballs and Pulsars

ROM terminology is “fuzzball”; the C# corresponding enemy class is `Fuseball`.
Level 16 minimum and maximum are both one. The `fuzz_move_prb` table gives raw
`$C0` for levels 11–27, including Level 16. The `fuzz_move_flg` table gives
`$00` for levels 11–20, including Level 16. The exact C#
movement/probability conversion remains to be verified; do not replace the ROM
parameter with an Incitti setting without labeling it reference-derived.

Pulsars do not appear at Level 16: both minimum and maximum are zero through
Level 16. Their minimum begins at level 17. The table's `pulsar_fire` baseline
is zero at Level 16; a separate hard-difficulty branch can set its enable bit,
but no Pulsar can be admitted while the Level 16 maximum remains zero.

## 10. Firing configuration

The Level 16 ROM `shot_holdoff` is `$23` (35 decimal) from the `$50`, minus-3
table range covering levels 1–20. It is the per-enemy minimum delay between
shots. The ROM shot-speed low-byte parameter is `$C0`; the surrounding speed
cracking code derives its runtime representation.

At medium/default difficulty `enm_shotmax` is raw `$02`. The firing routine
uses it as the final projectile-slot index and scans down through index zero,
so it reaches three slots (0, 1, 2), not two. Easy decrements the index to 1;
hard increments it to 3 (capped there), yielding indices 0–3. Existing C#
`MaximumActiveEnemyShots` treats the Level 14 raw `$02` as a capacity of two.
This is a demonstrated ROM-vs-translation ambiguity that Level 16 must resolve
explicitly; it is not authorization to change Level 14.

The ROM's raw new-shot thresholds, indexed by shots already active, are
`00 E0 F0 FA FF`. The routine fires when the random byte is at least the
selected threshold; for an 8-bit uniform byte this corresponds to 100%,
12.5%, 6.25%, about 2.34%, and about 0.39% respectively. Each eligible enemy
also has its own holdoff countdown. Flippers (including Tanker cargo),
Tankers, and Spikers carry the ROM shot-eligible bit; current flipping state,
along position, delay, random threshold, and free shot slots gate a shot.
There is no Level-16 per-enemy finite ammunition value in these tables.

The C# holdoff and shot-capacity switches currently stop at effective
Level 14. A Level 16 policy needs explicit values and a justified translation
of the ROM fixed-point speed, slot index, and difficulty behavior; do not
blindly reuse Level 14's C# count.

## 11. Admission and concurrency

`wave_enemies` is the pending original supply. ROM tracks pending supply,
enemies currently in the tube, and enemies at the top separately; it does not
define four initial C# Eggs. Initial Egg count, reserve-release interval,
approach limit, and deterministic lane schedule are C# project pacing data,
not ROM Level 16 constants.

The global `max_enm = 6` walks seven physical enemy records. Per-type
minimums/maximums are the values in section 4. The common ROM selector first
tries to satisfy nonzero type minimums, gives the Spiker/Tanker pair a
Spike-height decision when both remain available, then cycles available
minimum-bearing types from a random start. There is no separate Level 16
admission exception in the ROM table.

Tanker cargo is derived and can occupy two records after the Tanker is removed;
the Spiker-to-Tanker terminal path can create the same cargo late in the wave.
The C# policy must reserve child capacity and count latent cargo as well as
active enemies. The ROM's raw type caps and global record index are the
authority; do not infer caps from screenshots or copy Level 14's selector.

## 12. Completion obligations

The ROM completion path waits for pending supply and a separate byte at
`$0116` to clear, then checks every enemy record's along position before
clearing the wave; the disassembly examined does not identify `$0116`'s meaning.
Residual shots are cleared at the transition. A wave is not resolved merely
because the 27 original entries have been emitted. Tanker cargo, pending child
activation, and a terminal Spiker that can still convert must remain
represented until their lifecycle is terminal.

The C# production ledger already separates original IDs from cargo IDs and
tracks reserve/Egg, latent cargo, pending shot activation, active, destroyed,
rim-cleared, reconstructed, and capacity-not-allocated states. For ledger-backed
compositions, `TempestGame.CheckForLevelComplete` requires no unresolved
originals or cargo, reserve/Eggs, Tankers, Flippers, pending cargo activation,
Spikers, Fuseballs, or Pulsars. The ledger branch of `Level.IsComplete` checks
unresolved identities/reserves/Eggs and its active-count argument; it is not
the production gate used by `TempestGame` for ledger-backed waves. This shared
representation is suitable for Level 16 if the new wave policy wires every
conversion/release branch and its validators; no Level-16-only completion
counter is indicated by the ROM.

## 13. Transition and direct-start requirements

The current production chain reaches Level 15 and has no next level. Direct
startup resolves through that same chain, so `--start-level 16` is unavailable.
Level 16 later requires a real production configuration in both the natural
Level 15 -> 16 transition and direct-start resolver. The generic HUD reads
`DisplayedLevel`; score and lives are carried by fresh-wave initialization,
which resets the level state and player position, not the campaign score/lives.
The shared Super Zapper resets to two charges and clears its flash on fresh
wave initialization; no Level-16-specific recharge is indicated by evidence
examined.

Level 16 completion should be wired as a genuine wave clear. Do not implement
Level 17 or recycling in this milestone. The future Level 17 boundary must
select the ROM's repeated-layout sequence beginning again at shape row 0 and
must preserve score/lives and accepted transition behavior.

## 14. Presentation implications

Level 16 ROM presentation metadata read from row 11:

| Field | Raw value |
|---|---:|
| `lev_remap` for displayed Level 16 | `$0B` |
| `lev_scale` | `$0F` |
| `lev_y3d` / camera height | `$80` |
| signed `lev_y2d` | `$0000` |
| `lev_open` | `$00` (closed) |
| `lev_fscale`, `lev_fscale2` | `$00`, `$04` |

These are ROM values, not host-space placement or calibrated `--tube-*`
settings. The current C# mapping array includes Level 16 -> row 11, but the
presentation metadata catalog contains only displayed Levels 1–15 and
`AtariTubeRecordCatalog` has no row 11 record. The vector renderer can draw
ordered line geometry, but Atari-presentation resolution for Level 16 currently
fails at the missing metadata lookup. Do not choose calibration values in this
investigation.

The ROM source comment describes `fscale2 << 7 | fscale`, while the current
C# `FlipperScaleCombinedRaw` property combines its high byte with `<< 8`.
Keep the Level 16 raw pair visible and resolve that interpretation before
relying on a combined scale. Manual visual calibration remains a later
Neal-acceptance activity.

## 15. Current C# architecture mapping

1. **Configuration-first feasibility:** most species counts, supply,
   Spike-height raw value, shot parameters, cargo type, and movement program
   are data-driven candidates. However, the figure-eight zero-area winding
   failure means Level 16 is not currently a configuration-only addition.
2. **Reusable shared systems:** `LevelDefinition` ordered points and closed
   topology; deterministic fixed-step simulation; Egg/reserve admission;
   production identity/cargo ledger; Flipper repeating-24 route; Tanker pair
   release; Spiker identity conversion; enemy-shot collections; shared Spike
   objects; Super Zapper; vector renderer and HUD; recording/playback.
3. **New level-specific policy wiring:** no Level16 configuration/selection
   class exists. The C# configuration switches for holdoff and shot capacity
   end at Level 14. `TempestGame` has explicit per-policy branches for species
   admission, Tanker limits, Fuseball limits, death reconstruction, and
   terminal-Spiker conversion. Level 16 must be wired deliberately.
4. **Genuine gap:** support the Level 16 non-simple, zero-area outline without
   falsifying lane order or winding. ROM-specific Spiker lane hopping is also
   absent from the current shared Spiker restart translation; this is an
   explicit ROM-vs-project decision boundary, not a reason to copy a prior
   level workaround.
5. **No redesign indicated:** extend the smallest policy/configuration seam
   after geometry and translation decisions are authorized. Do not replace
   working ledger, deterministic playback, or rendering architecture.

## 16. Regression-sensitive shared mechanisms

| Area | Reuse assessment / Level 16 guard |
|---|---|
| Enemy identity and lifecycle | Reuse original IDs; verify Spiker-to-Tanker conversion retains the same original identity. |
| Egg/reserve admission | Reuse the shared scheduler; treat its cadence/count as project decisions, not ROM facts. |
| Tanker cargo | Reuse two independently owned cargo IDs; preserve latent, active, pending-shot, reconstruction, and terminal states. |
| Flipper movement | Reuse the explicit 2/3 `$24` route only after verifying its move/flip ordering; preserve handedness as an unresolved project/arcade choice. |
| Spiker lifecycle | Reuse the ledger conversion primitive; add Level 16 policy wiring. Preserve the documented no-lane-hop project translation unless Neal authorizes reconsideration. |
| Firing | Reuse the enemy-shot system, but verify eligible species, raw `$23` holdoff, thresholds, difficulty, and raw index-vs-count semantics. |
| Spike behavior | Reuse per-lane Spike mechanics; initialize all 16 closed-tube lanes from raw `$A0` through a documented height translation. |
| Death/reconstruction | Reuse identity reconstruction; test reserve, converted Spikers, both Tanker children, and residual projectiles. |
| Super Zapper | Reuse two-charge fresh-wave reset; assert no stale flash/charge state. |
| Wave completion | Reuse the ledger and validate no latent/pending cargo or active actors can complete early. |
| Direct startup/progression | Reuse shared resolver and transition; add Level 16 to chain/startup. Do not add Level 17 recycling now. |
| Rendering/runtime geometry | Vector line rendering is reusable; fix/resolve zero-area winding and add row 11 metadata before Atari presentation. |
| Deterministic recording/playback | Preserve fixed-step and seeded policy behavior; add Level 16 deterministic coverage when implementation is authorized. |

## 17. Level 14 development-process comparison

The Level 14 implementation record is useful only as a **process** reference:
it records a ROM-row-first configuration, a bounded production test, and
subsequent manual acceptance. That is a suitable discipline for Level 16:
derive exact ROM rows first, isolate translations, wire a bounded configuration,
and test lifecycle boundaries before Neal's visual/gameplay acceptance.

Do not copy Level 14's open topology, its `$07` Flipper route, its species
selector, calibration, or parameter translations. Level 16 has a closed,
self-touching row and program `$24`; those requirements are independently
established above.

## 18. Level 15 boundary confirmation

**Level 15 was not used as Level 16's implementation model, behavioral model,
or development template.** Its production code was only encountered as part of
the current configuration/progression surface. No Level 15 values or
workarounds were used to derive Level 16 geometry, supply, movement, spikes,
cargo, firing, or lifecycle requirements. The observed pre-existing Level 15
report edits and untracked Level 15 records were left untouched.

## 19. Uncertainties and evidence conflicts

- **Geometry conflict:** ROM row 11 has zero total signed area; current C#
  winding code rejects such non-collinear input. A lane-order-based/project
  winding decision is required.
- **Spiker lane hop:** ROM source selects a Spike-height lane on the `$F2`
  return path; current C# documents a project translation that does not add
  the ROM hop. Preserve that accepted translation unless separately
  authorized.
- **Shot-slot translation:** raw ROM `$02` indexes three medium shot slots;
  current C# Level 14 maps raw `$02` to a capacity of two. Do not copy that
  conversion to Level 16 without resolving the mismatch.
- **Initial Flipper handedness:** `$24` reverses after 2/3 groups, but the
  starting direction is not established by the movement table.
- **Raw-to-C# values:** Spike height, Flipper/Spiker speed, shot speed,
  shot-holdoff cadence, and Fuseball movement probability require explicit
  translations and appropriate validation.
- **Tanker capacity edge:** the ROM attempts two child allocations from a
  seven-record system. Saturated-slot behavior must be checked against the
  full allocation path; do not infer a free-child guarantee from screenshots.
- **ROM completion byte:** the clear routine also checks `$0116`; its meaning
  is unresolved in the examined disassembly and should not be misrepresented
  as a known cargo or enemy identity.
- **Presentation:** raw metadata does not establish accepted host-space
  calibration. `fscale2` combination shift differs between ROM comments and a
  current C# property.
- **No arcade Level 16 acceptance:** automated/source evidence cannot replace
  Neal's future manual gameplay, timing, controller, and visual acceptance.

## 20. Recommended bounded implementation scope

After Neal reviews and separately authorizes implementation:

1. Add the Level 16 ROM row 11 geometry/metadata and a closed 16-lane
   `LevelDefinition`; first solve the zero-area winding/transition requirement
   with a narrowly scoped test.
2. Add a Level16-specific wave/configuration policy from the independently
   tabulated values above, including the difficulty-specific supply and
   shot-slot semantics. Do not transplant Level 14 or Level 15 selectors.
3. Wire `$24` for ordinary and Tanker-released Flippers, Level 16 Spiker
   conversion, and the `$01` cargo pair through existing identity/ledger
   primitives. Preserve the accepted Spiker no-hop translation unless
   explicitly reconsidered.
4. Add targeted tests for raw table values, type exclusion/minimums,
   concurrency and cargo capacity, death reconstruction, conversion,
   no-premature completion, direct startup, natural Level 15 -> 16 progression,
   HUD identity, Super Zapper reset, and geometry adjacency/render data.
5. Keep Level 16 completion terminal for that implementation scope. Defer
   Level 17/recycling and all `--tube-*` calibration; stop after automated
   validation for Neal's manual gameplay/presentation acceptance.

## 21. Files and resources inspected

- `Reference/arcade/Tempest Rom Source code.md` — configuration tables,
  enemy initialization/selection, p-code, Spike/Tanker routines, firing,
  level completion, and geometry-table layout.
- `Reference/arcade/roms.rev2/136002-118.k1` — only the bounded 56-byte
  Level 16 geometry/remap/presentation slices described in section 3.
- `src/Tempest.Game/Levels/LevelConfiguration.cs`,
  `LevelDefinition.cs`, `LevelLoader.cs`, `AtariTubeRecordCatalog.cs`,
  `RomPresentationMetadata.cs`, `LevelProgression.cs`, `Level.cs`, and
  `Level3WaveLedger.cs`.
- `src/Tempest.Game/Gameplay/Flipper.cs`, `Tanker.cs`, `Spiker.cs`,
  `Spike.cs`, `Fuseball.cs`, and `Pulsar.cs`.
- `src/Tempest.Game/TempestGame.cs`,
  `src/Tempest.Game/Systems/SuperZapperSystem.cs`, and
  `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`,
  `VectorRenderer.cs`.
- `tests/Tempest.Game.Tests/Level14ProductionTests.cs`,
  `RomPresentationMetadataTests.cs`, `DirectLevelStartupTests.cs`,
  `LevelProgressionTests.cs`, and `CampaignSoakTests.cs` (read-only).
- `AI-Logs/Reports/2026-11-23-level14-fast-production-implementation-report.md`
  and `AI-Logs/Reports/2026-11-24-level14-final-closeout-report.md` for the
  permitted Level 14 process comparison.
- `TEMPEST-BEHAVIOR-SPEC.md` for existing Spiker/cargo project-decision
  boundaries.
- `Reference/arcade/MBUCOD.V05.txt` and `Reference/arcade/MBUDOC.DOC` received
  only targeted string searches; no additional geometry evidence was used.

## 22. Commands and validation

- Initial Git baseline:
  `git status --short --branch; git branch -vv; git rev-parse HEAD; git rev-parse --abbrev-ref --symbolic-full-name '@{u}'; git diff --stat; git diff --cached --stat; git ls-files --others --exclude-standard`
  — branch `master`, upstream `origin/master`, HEAD
  `63d37adb6c0b6b4f28a422f0c6c638cf5391e88c`. A pre-existing modified
  Level 15 closeout report and unrelated untracked Level 15 records, MBUCOD
  source text, and local recording material were preserved.
- Targeted `rg` searches and bounded `view` reads across the local disassembly,
  configuration, gameplay, rendering, progression, durable behavior
  documentation, and tests; no production/test content was changed.
- A first bounded K1 read used an incorrect assumed file base and returned a
  zero-byte short read at offset `0xA2C`; it was stopped. `Get-Item` reported a
  2,048-byte file. The corrected `$B800` mapping was used for only the
  56-byte row/metadata read; it returned the values printed in section 3.
- `python -c` shoelace check over the extracted row returned
  `twice_signed_area=0` and `point0_equals_point8=True`.
- **Tests/builds: NOT RUN.** No source or test change required baseline
  validation; running broad tests/builds would not materially improve this
  read-only investigation.
- No interactive game launched; no manual Level 16 acceptance claimed.

## 23. Change and Git confirmation

- **PASS:** no production files changed.
- **PASS:** no test files changed.
- **PASS:** no durable behavior specification or historical report changed.
- **PASS:** no files staged; nothing committed or pushed.
- Created only this report and
  `AI-Logs/Prompts/2026-11-24-level16-rom-configuration-investigation-prompt.md`.
- Pre-existing Level 15 edits and unrelated untracked material remain
  untouched. AI-Logs files are intentionally left unstaged.

**Stop condition satisfied.** Await Neal's review and explicit authorization
before any Level 16 implementation.
