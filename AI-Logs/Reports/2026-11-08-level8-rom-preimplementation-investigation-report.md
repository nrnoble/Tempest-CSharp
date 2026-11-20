# Production Level 8 ROM preimplementation investigation

Date: 2026-11-08
Result: **PARTIAL / BLOCKED — do not implement Level 8 from this investigation.**

## Executive findings

- The local ROM listing establishes displayed Level 8 as zero-based level index
  7, with effective parameter-table level `$08`. It does **not** contain the
  raw geometry, remap, or open/closed table bytes needed to identify or
  numerically reconstruct the tube. No visual shape was inferred from another
  source.
- ROM Medium/default supply is 24 generic pending units; the Hard branch makes
  it 27. All 16 ROM Spike-height slots receive raw `$c8`. A C# height
  conversion is not established.
- Ordinary generic selection can choose Tankers and Spikers, but not Flippers,
  Pulsars, or Fuzzballs. The type minima/maxima are Flipper 0/5, Pulsar 0/0,
  Tanker 1/2, Spiker 2/4, Fuzzball 0/0. Generic creation is gated at six
  active units; Tanker cargo release is a separate path and can exceed that
  generic gate.
- Tankers carry two Flippers. Their cargo is not extra generic pending supply.
  When no generic units remain, a Spiker can convert in-place into a
  Flipper-carrying Tanker without adding supply.
- Level 8's Flipper program byte is `$19`, whose p-code entry is the
  constant-flipping program with one along-movement tick between flips. This
  is not Level 7's `$24` 2-then-3 program. Ordinary Flippers are ineligible;
  the program matters for Tanker cargo released by a shot at or beyond ROM
  along-position `$20`. Near-rim/natural cargo uses the straight `$07` entry.
- Death reconstruction is generic: active-below-rim, at-rim, and pending
  counts are combined, species and movement identity are discarded, and
  latent Tanker cargo is not counted. Released cargo contributes as active
  units.
- Existing production startup, progression, terminal-level, Super Zapper,
  and recording/playback mechanisms are reusable. A dedicated Level 8
  geometry, raw Spike-height translation, and Level 8 Flipper/cargo policy
  still need source-backed implementation decisions.
- **Not ready for a bounded implementation milestone:** exact tube geometry,
  lane topology, tube palette, and ROM-to-C# Spike conversion are unavailable
  in the designated local ROM listing. The prior accepted Level 7 Flipper
  analysis also describes the natural-release boundary differently from the
  literal `$9b0b` comparison; resolve that boundary in a targeted trace before
  implementation.

## Starting Git state and scope

- HEAD: `bc109b4597f83477f5c7998bd3a9e258c72c6edf`
  (`Close out accepted interactive speed diagnostic`).
- Branch: `master...origin/master`.
- Initial tracked worktree and index were clean; `git status --short --branch`
  reported no staged, tracked, or untracked changes.
- This was read-only with respect to production code, tests, durable project
  documentation, references, recordings, screenshots, and Git state. No
  build, test suite, or game was run.
- The complete substantive user task is preserved verbatim in
  [the prompt record](../Prompts/2026-11-08-level8-rom-preimplementation-investigation-prompt.md).
- The only intended new project records are that prompt and this report under
  ignored `AI-Logs/`; neither is to be staged or committed.

## Evidence and limits

The sole primary arcade source used to establish Level 8 behavior was the
project-local [annotated ROM listing](../../Reference/arcade/Tempest%20Rom%20Source%20code.md).
No internet disassembly, Incitti code, screenshots, video, MAME, or another
Tempest implementation was used to determine Level 8 behavior or geometry.
Existing reports and C# source were consulted only for post-trace architecture
comparison.

The ROM file is an annotated transcription, not a local ROM binary. It
contains useful byte comments for the level-parameter tables, but several
geometry/color arrays are only `.chunk` declarations. Specifically,
`lev_x`, `lev_y`, `lev_angle`, `lev_remap`, and `lev_open` have no expanded
values. `Reference/arcade/` contains only this listing. Therefore numeric
geometry cannot be recovered from the approved local evidence.

## Level identity, tube selector, and geometry audit

### Selector trace

1. `p1_level` is zero-based: new-game initialization starts at `$00`, and the
   next-level handler increments it. Thus displayed Level 8 is internal index
   7 (`curlevel=$07`).
2. The parameter loader computes the effective table level as `curlevel + 1`,
   so Level 8 uses effective level `$08`.
3. `get_tube_no` (`$c2e8`) receives the stored level index. It extracts the
   low-nibble position in the 16-level block and reads `lev_remap[7]` for
   Level 8. The remap table is a `.chunk 16`; the value at index 7 is absent.
4. `lev_x` starts at `$b97c`; `lev_y` at `$ba7c`; `lev_angle` at `$bb7c`;
   `lev_remap` at `$bc7c`; and `lev_open` at `$bccc`. Each geometry table is
   declared as a `.chunk` with no raw values.
5. Rendering reads 16 point entries and uses the open flag to select the
   initial point and whether the endpoint closes. The ROM has 16 point slots,
   but Level 8's shape index and open flag are unknown.

### Geometry audit result

| Required item | Result from the approved ROM listing |
|---|---|
| Zero-based level index | 7 |
| Effective parameter level | `$08` |
| Tube/shape selector | `lev_remap[7]`; selected value unavailable |
| Raw coordinate table | `lev_x[$shape][0..15]` / `lev_y[$shape][0..15]`; bytes unavailable |
| Angle table | `lev_angle[$shape][0..15]`; bytes unavailable |
| Point/lane order and endpoint values | Unavailable |
| Lane count | 15 if the omitted `lev_open` value is open; otherwise 16. Exact Level 8 value unknown. |
| Adjacency/wrap | ROM indexes adjacent segments; open endpoints have special reversal. Which topology applies to Level 8 is unknown. |
| Shared/convex/reflex vertices and directed pivots | Cannot be calculated without the raw point and angle bytes. |
| Center, scale, camera offset | Selected via additional omitted per-tube tables; Level 8 values unavailable. |
| Collision/projection implications | Cannot be resolved without Level 8 coordinates and topology. |
| Tube/palette | Tube color uses `curtube & 7` through `$c22d..$c234` (`06 03 01 04 00 05 05 05`), but `curtube` is unknown. Level color blocks at `$c1fd` are also `.chunk` placeholders. |

This is not a descriptive-shape guess: the approved source does not expose
enough bytes to derive any shape name, numerical vertices, connectivity map,
or color selection. No external visual or secondary reference was consulted.

### C# geometry translation candidate (not implementable yet)

`LevelDefinition` can represent a 16-point definition with `Continuous=true`
for a closed tube; `Continuous=false` retains the last point but omits the
closing lane. A closed source would yield lane edges `i -> (i+1) mod 16`;
an open source would yield only edges `0..14`. This is a translation rule,
not a finding about Level 8.

`LevelLoader` stores explicit `Vector2` coordinates; `Projection` applies the
existing perspective to those coordinates. A future translation must derive
the signed relative points from the ROM's encoded coordinates and select
screen scale/centering using the omitted ROM scale/offset values. It must not
copy Level 7's center, offset, or point set. No normalized coordinates or
lane definitions are proposed because that would fabricate source data.

There is an additional C# topology seam to verify after the ROM `lev_open`
byte is available: `LevelDefinition.PlayableLaneCount` excludes a closing
lane on open boards, while `Level.NextEdge` and `PreviousEdge` wrap modulo
the full point count. Player, enemy, and collision movement must follow the
actual open/closed ROM topology rather than assuming these helpers are
sufficient.

## Level 8 ROM parameter table

`$92c5-$9326` resolves the level-specific parameter tables. Values below are
for effective level `$08`, not literal screen coordinates unless identified
as raw ROM bytes.

| Parameter | Level 8 Medium/default | Source-derived meaning |
|---|---:|---|
| `wave_enemies` | `$18` = 24 | Generic pending wave units |
| `wave_spikeht` | `$c8` = 200 | Raw along-position byte copied to all 16 `spike_ht` slots |
| `max_enm` | `$06` | Generic creation gate at six active below-rim plus at-rim units |
| Flipper minimum / maximum | 0 / 5 | No ordinary bottom Flipper; maximum remains for Tanker cargo accounting |
| Pulsar minimum / maximum | 0 / 0 | Pulsars not eligible |
| Tanker minimum / maximum | 1 / 2 | Tankers eligible; concurrent type limits |
| Spiker minimum / maximum | 2 / 4 | Spikers eligible; concurrent type limits |
| Fuzzball minimum / maximum | 0 / 0 | Fuseballs/Fuzzballs not eligible |
| `tanker_load[0..3]` | 1, 1, 1, 1 | Type 1 / Flipper cargo; first two bytes are initialized to 1 and table entries +2/+3 also resolve to 1 |
| `flipper_move` | `$19` | P-code entry used by the level-selected Flipper program |
| `shot_holdoff` | 59 ticks | `$9607_0b`: 80 at level 1, decrement 3 per level through level 20 |
| `enm_shotmax` | 3 | Highest shot-slot index; creation loop provides four simultaneous enemy-shot slots |
| `enm_shotspd_lsb` | `$c0` raw input | Passed through `crack_speed`; do not equate directly to C# units |
| `spd_flipper_lsb` | `$b1` raw input | `$9607_67`: `$d4` at level 1 with `$fb` (-5) per level through level 8 |
| `spd_tanker` | Same converted speed as Flipper | ROM setup copies Flipper speed to Tanker speed |
| `spd_spiker_lsb` | `$c0` raw input | Passed through `crack_speed`; do not replace with the C# speed constant without a validated conversion |
| `flip_top_accel` | 2 | Level-table value; Easy reduces it by one for this level |
| `pulsar_fire` | 0 | Level table does not enable fire at level 8; Hard sets the flag, but Pulsar max remains zero |

### Difficulty

- Medium/default (difficulty bits 0 or 3): 24 pending units; shot-slot index
  3 (four shots).
- Easy: wave supply remains 24; shot-slot index decreases to 2 (three shots).
  The ROM also adjusts Flipper speed and decreases `flip_top_accel` for this
  level.
- Hard: the ROM adds one eighth of 24, yielding 27 pending units. The shot
  index increment is capped at 3, so the capacity remains four. The branch
  adjusts Flipper speed and enables the Pulsar-fire flag; no Pulsars are
  selectable on Level 8 because both Pulsar min/max are zero.
- Project policy should initially target Medium/default. Do not add a general
  difficulty selector as part of Level 8.

The supply table does not count bullets, Spikes, or latent Tanker cargo as
additional pending units. `set_enm_and_spikes` copies `wave_enemies` to
`enemies_pending` and writes `wave_spikeht` into indices `$0f` through `$00`.
The 16 writes establish 16 ROM Spike slots; the omitted topology prevents
claiming these equal 16 playable C# lanes.

## Enemy selection, Tankers, and cargo

The ROM selector computes each species' availability from `max_* - n_*`,
reserves two cargo-type availability units for each active Tanker, then caps
availability against remaining generic active capacity. Every ordinary
selection path requires a nonzero species minimum. On a fresh Level 8, the
first two satisfiable minimum units are Spikers, then a Tanker; this is a
minimum policy, not a fixed full-wave distribution. Once minima are met, the
selector can choose between available Tankers and Spikers using the shortest
Spike height (`$cc`) before its randomized fallback.

| Species/path | Ordinary bottom selection | Other creation path |
|---|---|---|
| Flipper | No: minimum is zero | Tanker cargo only; maximum 5 is retained for cargo |
| Tanker | Yes; minimum 1, maximum 2 | A final-pending Spiker can convert into one |
| Spiker | Yes; minimum 2, maximum 4 | Spiker program also selects/grows Spikes and can hop lanes |
| Pulsar | No: min/max zero | No Level 8 cargo type |
| Fuseball/Fuzzball | No: min/max zero | No Level 8 cargo type |

An ordinary Tanker consumes one generic pending unit when materialized. It
holds two latent Flippers, which do not decrement the pending counter.
At the ROM cargo point, the Tanker slot is retired and two child slots are
attempted on opposite adjacent segments with opposite direction bits. The
children do not consume additional pending units. The cargo type selection is
type 1, Flipper, in all four Level 8 load entries.

`$9b07` selects the child movement entry using the ROM along coordinate:
below `$20` it uses the default `$07` straight-movement entry; at or beyond
`$20` it dispatches through the level-selected movement path, which selects
Level 8's `$19` program. The natural Tanker split is reached just below
`$20` and uses `$07`; a sufficiently deep player-shot Tanker destruction
uses the `$19` entry. The precise `$20` equality branch is `bcs` and belongs
to the level-selected side. The prior
[Level-specific Flipper/Spike analysis](2026-11-06-level-specific-flipper-spike-rom-analysis-report.md)
describes the natural-release boundary as `$07`; this is consistent with the
ROM's `<$20` branch. Keep these depth distinctions explicit; do not model all
Tanker cargo as one route.

## Flippers and movement

The Level 8 table byte is `$19`; the p-code bytes at `$a110` describe:

1. Move along once and yield.
2. Start a flip and yield.
3. Continue the flip while moving along on continuation ticks.
4. Repeat the one-move/flip cycle.

This program has no Spike test and no explicit direction-reversal opcode.
Open-endpoint reversal is separately conditional on `open_level`; whether
Level 8 uses that guard cannot be known until `lev_open[shape]` is available.
Level 7's `$24` route is different: its source program counts two flips,
reverses, counts three, and reverses again. Do not reuse the accepted C#
Level 7 `2-3-2-2-2` observed route for Level 8.

Because ordinary bottom Flippers are suppressed, ordinary Flipper initial
handedness is not a Level 8 entry case. Tanker children begin on opposite
adjacent segments and with opposite direction bits; mapping those bits to
C# `PreviousEdge`/`NextEdge` requires the omitted lane ordering. Shot eligibility
for Flipper cargo follows the ROM Flipper firing state; Level 8 does not
introduce a Spike-sensitive program.

The ordinary Spiker p-code is the common grow/reverse/hop script at `$a0f7`.
It creates or updates the Spike in its lane, hops to a lane chosen using ROM
Spike heights, and when its final path finds `enemies_pending == 0`, rewrites
that same enemy slot as a Flipper-carrying Tanker at `$f0`. It adds no pending
unit. The exact C# Spiker speed and Level 8 starting Spike height require
separate raw-unit translation; the raw Spiker speed input is `$c0`, and the
starting Spike byte is `$c8`.

## Pulsars, Fuseballs, and enemy firing

- **Pulsars:** min/max are 0/0 at level 8, so no ordinary Pulsars. The
  `pulsar_fire` table starts at effective level `$3c`; Medium Level 8 is off.
  The Hard branch sets the flag, but does not make the type selectable.
- **Fuseballs/Fuzzballs:** min/max are 0/0; neither ordinary selection nor
  Tanker cargo can create one. Level 8 needs no production Fuseball behavior.
- **Firing capacity:** `enm_shotmax=3` addresses four slots. The common
  random thresholds indexed by current enemy-shot count are `$00,$e0,$f0,$fa,$ff`.
- **Timing:** `shot_holdoff=59` updates after a shot. An enemy starts with
  shot delay zero; the ROM then decrements its per-enemy delay and checks
  whether a shot slot and random roll are available.
- **Species:** Flipper, Tanker, and Spiker setup carries the firing bit;
  Pulsar firing is separately controlled by `pulsar_fire`; Fuseballs are
  ineligible. There is no Level 8 per-species ammunition table.
- **Projectile speed:** the table supplies `$c0` to `crack_speed`; the ROM
  converts that to its fixed-point movement fields. Preserve the raw byte
  separately from any C# speed translation.

## Starting Spikes and death/re-entry

`wave_spikeht=$c8` is the Level 8 raw ROM value. Initialization copies it to
all 16 `spike_ht` entries. It is not a C# height. Existing C# production uses
height 60 for Levels 4-7 even though their raw ROM bytes differ, so neither
copying 60 nor using 200 directly is justified for Level 8. A conversion
requires evidence from ROM along-coordinate scaling and/or a later manual
acceptance comparison.

On player death, the ROM sets generic pending supply to
`min(63, enemies_in + enemies_top + enemies_pending)`. It clears active type
counters and rebuilds generic pending entry records. No active Flipper,
Tanker, or Spiker identity/movement program is retained. A Tanker's latent
cargo is not included in the sum and disappears with that carrier; released
cargo is active and contributes through the active counters. The final-pending
Spiker-to-Tanker substitution can occur again after the last generic unit is
consumed.

The existing Level 7 generic re-entry model is the closest C# starting point:
clear unresolved species selection, turn released cargo into generic
unresolved units after death, discard latent cargo when its carrier is lost,
and keep already resolved outcomes terminal. Do not use the Level 3-6
species-preserving reconstruction policy for Level 8. Existing ledger
identities may remain an internal C# accounting tool if they do not affect
ROM-shaped species selection or re-entry counts.

## Progression, completion, and terminal boundary

- ROM natural progression increments `p1_level`/`curlevel`; Level 7 index 6
  advances to Level 8 index 7. The existing between-level approach and fresh
  wave initialization can be reused.
- The ROM completion path waits for generic pending and death work to clear
  and for active enemies to finish their near-rim handling. Level 8 does not
  use the `AVOID SPIKES` interstitial reserved by the ROM's `curlevel < 7`
  condition.
- ROM setup resets Super Zapper usage when a new level is installed. C#
  `InitializeFreshWave` already resets the Super Zapper for each level.
  Score/lives carry and fresh-level Blaster placement can reuse existing
  campaign policy; exact Level 8 ROM lane placement is not established here.
- Do not implement Level 9. Once Level 8 is appended as the last production
  level, the existing terminal campaign behavior can show Level 8 completion
  and restart at Level 1. No new terminal-level mechanism is required.

## Existing C# architecture and implementation recommendation

Reviewed: `LevelConfiguration`, `LevelLoader`, `LevelDefinition`, `Level`,
`LevelProgression`, `Level3WaveLedger`, `Level3WaveSelection`,
`Level7WaveSelection`, `Flipper`, `Tanker`, `Spiker`, `Spike`,
`Pulsar`, `Fuseball`, enemy bullets, `TempestGame`, projection/renderer,
direct startup, and progression/recording/playback tests. Prior Level 7
reports were used only as engineering-history cross-checks, not as the source
for Level 8 geometry or ROM values.

| Area | Reuse / required decision |
|---|---|
| Configuration and loading | Append a Level 8 configuration and loader entry only after the raw geometry/remap/open bytes and palette can be transcribed. Existing 16-point `LevelDefinition` is suitable. |
| Progression/direct start | Extend the existing production chain; direct `--level 8`, natural Level 7 -> 8, recording startup, and replay resolve through the same configuration. |
| Enemy accounting | Reuse the production ledger with Level 7's generic re-entry semantics; Tanker remains one generic pending unit and cargo two child units outside pending. Confirm latent/released cargo death cases with tests. |
| Enemy selection | The ROM min/max and max-enemy values match Level 7's broad gates, but its current C# post-minimum random choice is not the ROM's spike-height/random-start control flow. A Level 8 ROM-compatible picker or a safely shared policy that leaves accepted Level 7 behavior unchanged is needed for strict fidelity. |
| Flippers | Add/represent the `$19` p-code route and distinguish `$07` near-rim Tanker cargo from `$19` deep shot-released cargo. Do not route Level 8 through the Level 7 `$24` implementation. |
| Tanker/Spiker | Reuse the two-child Tanker and Spiker-to-Tanker accounting mechanisms; make the zero-pending conversion a same-unit conversion, not new supply. |
| Spikes | Preserve 16 ROM slots, but do not assign C# height until the `$c8` conversion and open-topology lane mapping are established. |
| Completion/terminal | Reuse existing completion, exit, terminal-level, Super Zapper recharge, and campaign restart paths. |
| Geometry/topology | No implementation proposal is possible until the omitted ROM tables are supplied. Audit `NextEdge`/`PreviousEdge` for open topology before using those helpers. |
| Debug/recording/playback | Existing direct startup, deterministic recordings, visible/headless playback, stepping/scrubbing, screenshots, and speed diagnostic should remain configuration-agnostic; add Level 8 coverage without changing tooling behavior. |

Do not modify Levels 1-7 to make Level 8 fit. Keep the 24-unit Medium wave,
four initial C# Eggs, ROM-compatible ordinary type eligibility, cargo
accounting, and natural Level 7 -> 8 progression in the future candidate.
Use Level 8's own Flipper program and the existing terminal campaign boundary.
Do not expand scope to Level 9 or deferred Fuseball destruction graphics.

## Proposed automated tests for the future implementation

- Exact ROM-derived shape selector, all 16 raw points/angles, open/closed
  topology, lane count/order/connectivity, directed pivots, and player traversal.
- Projection/collision boundaries and Spike slot placement for the selected
  topology.
- Medium supply 24, Easy/Hard rules, six-unit generic active gate, species
  minima/maxima, no ordinary Flipper/Pulsar/Fuseball entry, and no latent
  cargo counted as pending.
- Tanker type and two-child release, near-rim `$07` versus deep `$19` cargo
  route, opposite initial directions, and generic death reconstruction with
  both latent and released cargo.
- Level 8 Flipper `$19` sequence and no accidental Level 7 `$24` sequence.
- Spiker growth, starting Spike raw-to-C# conversion, final-pending
  Spiker-to-Tanker conversion, and completion.
- Enemy firing holdoff, shot capacity, chance-table indexing, and ROM-speed
  translation.
- Level 7 -> 8 natural transition, direct `--level 8` startup, score/lives
  carry, Super Zapper recharge, terminal Level 8 completion/restart, and
  Level 9 rejection.
- Fresh deterministic Level 8 recording, visible/headless playback,
  pause/step/reverse inspection, screenshots, and speed-diagnostic playback.
- Regression coverage proving accepted Levels 1-7 remain unchanged.

## Proposed manual arcade-acceptance checklist

After the geometry bytes and implementation exist:

- Compare the independently ROM-derived tube visually to the arcade; verify
  lane count/topology and Blaster traversal.
- Verify tube color/presentation, ordinary bottom-entry species, Tankers and
  their cargo, Flipper `$19` movement, Spikers, and starting Spikes.
- Confirm Pulsars and Fuseballs are absent on Level 8; verify enemy firing
  and Super Zapper behavior.
- Test one death/re-entry if practical; verify natural Level 7 -> 8
  progression and Level 8 completion/terminal behavior.
- Create a fresh Level 8 recording; inspect difficult behaviors with pause,
  forward stepping, and reverse scrubbing. Optionally use `--speed` below
  1.0 for detail and above 1.0 for faster replay.
- Use automated tests, not manual play, to verify invisible identity and
  pending-supply bookkeeping.

## Exact unresolved questions

1. What are the 16 `lev_remap` values and which shape is `lev_remap[7]`?
2. What are the Level 8 raw `lev_x`, `lev_y`, `lev_angle`, and `lev_open`
   bytes, and what are the corresponding lane count and directed pivots?
3. What ROM `lev_scale`, `lev_y3d`, `lev_y2d`, and related values apply to
   the selected tube?
4. Which tube-color map entry applies after resolving `curtube`, and what
   are the Level 8 color-block bytes at `$c1fd`?
5. What validated conversion maps raw starting Spike `$c8` to the C# height
   scale, and does an open tube alter which of the 16 slots are playable?
6. Confirm the exact natural/deep Tanker cargo program at the `$20` boundary
   with a fresh instruction-by-instruction trace. The `<$20`/`bcs` control
   flow indicates natural release uses `$07` and equality selects `$19`;
   preserve that distinction.
7. What initial Level 8 lane/Blaster position is appropriate for the C#
   transition, beyond the existing project default?
8. What ROM-to-C# fixed-point speed values should be used for Flippers,
   Tankers, Spikers, and enemy projectiles?

## Final state

No production code, tests, durable documentation, references, recordings, or
Git state were changed. The prompt copy was hash-verified against the
828-line attachment and contains exactly one standalone completion token.
The required next step is to provide the missing expanded local ROM table
data (or an approved local binary-derived transcription) and resolve the
noted cargo-boundary trace before Level 8 implementation.
