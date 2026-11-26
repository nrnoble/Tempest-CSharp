# Displayed Level 19 ROM/Configuration Read-Only Investigation

Date: 2026-11-26

## 1. Executive summary

**READY FOR BOUNDED LEVEL 19 IMPLEMENTATION**

Medium/default Level 19 is bounded by the Rev. 2 ROM tables and the accepted
Level 17/18 architecture. The displayed level remaps to canonical first-cycle
Shape 3, uses cycle-1 palette data, has 22 original supplies, a seven-place
physical envelope, and initial 4-Egg admission. Ordinary Flippers 0-3, Tankers
1-1, and Pulsars 2-3 are eligible. Spikers, Fuseballs, starting/dynamic Spikes,
and Pulsar firing are absent in medium/default. Tankers carry two Flipper cargo
identities. Ordinary and cargo Flippers use ROM movement byte `$19`. Medium
Flipper/Tanker raw speed is `$A9`; the existing host actor and selector seams
can represent the wave.

## 2. Starting Git/worktree state

- Branch: `master`
- HEAD: `9a8f788a41e42ea578fb1dcad90dd74a9a3cc60b`
- Upstream: `origin/master`
- Ahead/behind: `0/0`
- Staged: none
- Tracked modifications: none
- Untracked: `Reference/arcade/MBUCOD.V05.txt`
- Protected assets/reference/recordings untouched
- Expected accepted Level 18 checkpoint `3034c227ae855061c6f2ae8bb193cc9e43405566`
  is an ancestor of current HEAD.

## 3. Evidence authorities

Primary evidence:

- `Reference/arcade/Tempest Rom Source code.md`
- `TEMPEST-BEHAVIOR-SPEC.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `.github/copilot-instructions.md`
- Level 18 final closeout and both Level 18 readiness/movement reports
- Current Level 17/18 wave selectors, LevelConfiguration, LevelProgression,
  ROM presentation catalog, shared actors, and pulse infrastructure

No Internet ROM search, Incitti gameplay authority, raw ROM inspection,
screenshots, videos, recordings, or private scratchpad material was used.

## 4. Level mapping

- Displayed Level: 19
- ROM `curlevel`: `$12` (18 zero-based)
- Effective `lev`: `$13` (19 one-based)
- Cycle number: 2
- Cycle-local position: 3
- Zero-based cycle index/remap index: 2
- ROM shape row: 2
- Board index: 2
- Palette index: 1

Arithmetic: `curlevel = lev - 1 = 19 - 1 = 18 = $12`;
`cycleLocal = ((lev - 1) & $0F) = 2`, with this task's Level 19 shape
position mapping to row index `lev_remap[2] = 2`.

## 5. Canonical shape reuse

ROM-BACKED shape row 2 corresponds to canonical first-cycle Level 3 geometry.
Existing C# equivalent:

- `LevelConfiguration.Level3Mixed`
- `LevelLoader.LoadLevel3Plus()`
- BoardIndex 2
- closed topology
- 16 lanes
- canonical 16 ordered vertices

No Level 19 coordinates should be added.

## 6. Production presentation reuse

Required future reuse chain:

```
Displayed Level 19
 -> canonical Shape 3 / ROM row 2
 -> existing Level 3 Plus geometry and model frame
 -> existing Level 3 production projection/calibration
```

The exact projection is shape-keyed, not a Level 19-specific transform.

## 7. Palette reuse

Palette index 1. Existing cycle-1 raw block:
`00 07 0B 08 44 03 0C 0C`.

Eligible role coverage is Flipper magenta, Tanker blue, Pulsar cyan,
Egg yellow, tube red, Blaster yellow. No eligible Level 19 species introduces
a new unresolved role. Pulsar pulse transient restores to the cyan base role.

## 8. Eligibility table

| Feature/species | Medium eligible | Evidence | Classification |
|---|---:|---|---|
| Ordinary Flippers | Yes | min 0, max 3 | ROM-BACKED |
| Tankers | Yes | min 1, max 1 | ROM-BACKED |
| Spikers | No | min/max 0/0 through source 17-19 | ROM-BACKED |
| Pulsars | Yes | min 2, max 3 | ROM-BACKED |
| Fuseballs | No | no eligible range | ROM-BACKED |
| Starting Spikes | No | wave_spikeht entry `$00` | ROM-BACKED |
| Dynamic Spikes | No | no Spiker P-code and no starting Spikes | ROM-BACKED |
| Tanker cargo | Yes | four `$01` entries resolve to Flipper pairs | ROM-BACKED |
| Flipper firing | Yes | shared species fire gate | ROM-BACKED |
| Tanker firing | Yes | shared species fire gate | ROM-BACKED |
| Spiker firing | Dormant | no active Spiker | ROM-BACKED |
| Pulsar firing | No | medium Pulsar fire flag not enabled | ROM-BACKED |
| Fuseball firing | Dormant | no active Fuseball | ROM-BACKED |

## 9. Spike/Spiker policy

Level 19's cycle entry in `wave_spikeht` is `$00`. It has no independent
starting Spike height. Spiker ranges are 0/0 for source levels 17-19, so no
Spiker can be selected and dynamic Spike growth cannot be reached.

Dormant Spiker speed/firing values do not activate either Spikers or Spikes.

## 10. Level 20 Spike-boundary confirmation

Level 19 is the last level before the user-observed Level 20 Spike-resumption
boundary. The Level 19 entry remains `$00`. Level 20 remains unsupported and
is documented only as evidence; no Level 20 implementation is specified.

## 11. Original supply

Medium/default Level 19 ROM wave supply is 22 originals.

- ROM table opcode/range: incremental 17-26, base `$14`, increment `$01`
- Effective lev: 19
- Result: `20 + (19 - 17) = 22`

Easy retains 22. Hard has the existing ROM difficulty adjustment and should
be validated rather than production-expanded in this milestone.

## 12. Species min/max

- Flipper: 0 minimum, 3 maximum
- Tanker: 1 minimum, 1 maximum
- Spiker: 0/0
- Pulsar: 2 minimum, 3 maximum
- Fuseball: 0/0

Minimums are selected before random choices. Pulsars and a Tanker occupy the
minimum envelope at wave start; Flippers fill remaining random capacity.
Latent Tanker cargo uses two cargo identities outside original supply and
reduces ordinary Flipper availability.

## 13. Physical envelope/admission

- Maximum physical enemy records: 7
- Initial Eggs: 4
- Maximum active Eggs: 4
- Maximum approaching Eggs: 2
- Hatch interval: 50 ticks
- Egg reserve: retained generic 4-Egg/re-entry pipeline
- Capacity admission: reject species/cargo if no physical unit is available

The accepted Level 18 generic ledger/selector architecture is directly
reusable; no target-level ledger is indicated.

## 14. Tanker cargo

- Tanker eligible: exactly 1 minimum/maximum
- Cargo type: `$01`
- Cargo count: 2 Flipper identities
- Cargo route: ordinary/cargo Flipper movement policy
- Ordinary identity: Tanker cargo remains distinct cargo identities
- Reconstruction: existing generic Egg reserve/re-entry

No source-level cargo third/fourth variants differ on effective Level 19.

## 15. Ordinary Flipper movement

Cycle-local `flipper_move` entry:

- ROM movement byte: `$19`
- C# semantic: existing `EightAdvanceTurn`/sixteen-pivot route family
- Shared rim handoff: below-rim program ends at rim and enters generic
  rim pursuit

## 16. Flipper P-code semantics

`$19` is an existing ROM P-code route: advance eight positions/depth units,
turn once at held depth, then repeat. Existing shared C# `EightAdvanceTurn`
is the intended implementation seam. No independent Level 19 P-code engine
is needed.

## 17. Existing C# movement-strategy match

Configuration-only existing route:

- ordinary Flipper: `BelowRimFlipperProgram.EightAdvanceTurn`
- cargo Flipper: generic cargo route using the same shared `EightAdvanceTurn`
- rim: shared rim lifecycle

No target-level behavior bypass is required.

## 18. Cargo route

Tanker cargo `$01` is a Flipper cargo route. ROM evidence gives no
cargo-specific movement byte different from ordinary `$19`; it should use the
same semantic route, preserving cargo identity and shared rim handoff.

## 19. Flipper/Tanker movement speed

- Medium/raw byte: `$A9`
- Cracked signed 8.8 magnitude: 2.71875
- Tanker copy rule: Tanker copies the cracked Flipper speed
- Existing C# seam: single shared climb rate on `FlipperConfiguration`

The exact easy/hard variants remain deferred. Easy/hard medium-proxy values
must not be installed unless a later implementation milestone explicitly
adds them.

## 20. Pulsar policy

- Eligible: minimum 2, maximum 3
- ROM threshold: `$14`
- Medium fixed Pulsar speed: `$FE.A0`, magnitude 1.375 ROM units
- Alternate speed: Level 19 Flipper speed `$A9`
- Pulse beat: `$04`
- Medium/easy firing: off
- Hard firing: hard-only `$40` gate

## 21. Shared Pulsar configuration match

The existing nullable-threshold shared Pulsar configuration is sufficient.
Map raw threshold `$14` using the accepted Level 18 mapping formula:

`depth * (F2 - 14) / (F2 - 10) = 392.92035f` host height.

Fixed host `1f`. The ROM alternate/fixed ratio is
`2.71875 / 1.375 = 87 / 55`, so the deep-side toward-rim host route is
`87/55` under the same fixed-1 anchor translation used across Levels 17-18.
This remains a PROJECT TRANSLATION, not literal ROM timing equivalence.

The existing implementation already supports threshold-aware climb,
descending reversal, and pulse-region gates.

## 22. Fuseball policy

Ineligible. No `Fuzzball` minimum/maximum, movement probability, firing,
terminal, or pending-supply participation applies on medium/default. Existing
generic Fuseball infrastructure remains dormant.

## 23. Spiker policy

Ineligible. No Spiker speed, movement, projectile, Spike growth, retreat,
terminal conversion, pending-supply, or identity lifecycle applies. The
accepted faster-shot-than-Spiker invariant remains irrelevant until eligibility
exists.

## 24. Firing policy

- Medium shot holdoff: `$1D` (29 ticks), consistent with existing Level 18
  approximation and the 17-19 medium transition
- Medium inclusive last slot index: `$02`
- Actual medium capacity: 3 active enemy shots
- Enemy shot raw speed: `$C0`
- Cracked host speed: 3
- Firing species: Flipper and Tanker only
- Pulsar medium: disabled
- Spiker/Fuseball: disabled

Easy/hard capacity and holdoff are difficulty variants and defer to a future
implementation milestone.

## 25. Pulsar firing

Medium/default Pulsar firing is off. Hard-only `$40` is dormant and must not
be enabled by the medium Level 19 configuration.

## 26. Egg/admission reuse

Existing generic behavior is sufficient:

- 4 active Eggs
- reserve
- two approaching limit
- 50-tick hatch pacing
- deterministic lane/direction sequence
- generic species selector
- shared Egg factory

No ROM-backed new Egg class or admission override is indicated.

## 27. Identity/ledger compatibility

Eligible identity classes fit the existing generic ledger:

- original identity
- cargo identity
- Egg identity/reserve
- active/rim/terminal states
- destruction/escape
- Tanker re-entry
- reconstruction

No target-level identity rule is indicated.

## 28. Death/reconstruction

The existing generic ROM-policy reconstruction should apply: player death
returns unresolved originals through reserve/Egg re-entry, released cargo
remains cargo-owned, and active destroyed identities stay destroyed. Movement
policy reapplies through the shared Flipper/Pulsar configuration. No Level 19
reconstruction implementation is needed.

## 29. Completion

Existing generic completion obligations can represent Level 19: original
supply and terminal obligations clear, Tanker cargo contributes the existing
Flipper obligations, and no special Level 19 counter is justified.

## 30. Semantic policy map

| Property/seam | Level 19 value | Future code |
|---|---|---|
| Species selector | 0/3 Flipper, 1/1 Tanker, 2/3 Pulsar | reuse new semantic wave config |
| Pending supply | 22 originals | reuse generic ledger |
| Physical envelope | 7 | reuse existing |
| Cargo | `$01`, 2 Flipper cargo | reuse cargo identity |
| Flipper movement | `$19` | reuse EightAdvanceTurn |
| Rim lifecycle | shared | none |
| Spike eligibility | none | reuse inactive state |
| Pulsar | threshold/speed config | existing shared strategy |
| Firing | 29/3, Flipper/Tanker | existing shared gate |
| Fuseball/Spiker | absent | none |

## 31. Natural Level 18 to 19 progression

Future implementation should preserve ExitZoom, Approach, score/lives,
Super Zapper recharge, cycle-1 palette, canonical Shape 3 transition, and
score carry. One Level 19 production configuration should drive both campaign
and direct startup.

## 32. Direct startup

`--start-level 19` and `--level 19` should both resolve through
`LevelProgression.GetProductionLevel(19)` and the same production
configuration. No separate direct behavior is justified.

## 33. Terminal boundary

Level 19 is the proposed terminal supported production level. Level 20 remains
unsupported. The unsupported condition must not silently advance to undefined
Level 20 behavior.

## 34. Recording implications

- `lvl1-16.json` remains the accepted foundation oracle.
- `lvl-17-test.json` naturally reaches Level 18 and does not establish Level 19
  playback.
- Adding Level 19 must not rewrite old Level 17/18 expected outcomes.
- A separate deterministic Level 19 recording should only be adopted after
  manual acceptance.

## 35. Presentation implications

Shape 3 reuse requires canonical first-cycle projection/calibration, not
copied Level 19 coordinates. Palette remains cycle 1. Level 19 manual
acceptance is required for shape, placement, movement feel, and perceived
timing.

## 36. Implementation-readiness matrix

| Field | Value | Evidence | C# seam | New code |
|---|---|---|---|---|
| Displayed/internal | 19 / lev19 / curlevel12 | ROM-BACKED | mapping catalog | bounded extension |
| Shape | canonical Shape 3/row2 | ROM-BACKED | Level3Plus | reuse |
| Palette | cycle1/index1 | ROM-BACKED | resolver/catalog | none |
| Supply | 22 medium | ROM-BACKED | wave selector | configuration |
| Eggs | 4/4/2/50 | EXISTING ACCEPTED | shared | none |
| Envelope | 7 | ROM-BACKED | ledger | reuse |
| Flippers | 0/3 | ROM-BACKED | selector | config |
| Tankers | 1/1 | ROM-BACKED | selector | config |
| Spikers | 0/0 | ROM-BACKED | dormant | none |
| Pulsars | 2/3 | ROM-BACKED | selector/Pulsar | config |
| Fuseballs | 0/0 | ROM-BACKED | dormant | none |
| Spikes | none | ROM-BACKED | dormant | none |
| Cargo | 2 Flipper `$01` | ROM-BACKED | Tanker cargo | config |
| Flipper program | `$19` | ROM-BACKED | EightAdvanceTurn | config |
| Flipper/Tanker speed | `$A9` | ROM-BACKED | shared speed | translation |
| Pulsar threshold | `$14` | ROM-BACKED | nullable threshold | config |
| Pulse beat | `$04` | ROM-BACKED | shared pulse | config |
| Firing | Flipper/Tanker, medium | ROM-BACKED | shared gate | config |
| Capacity/speed | 3 / `$C0` | ROM-BACKED | enemy bullets | reuse |

## 37-42. Complexity classification

**Configuration only**

- mapping, palette, supply, ranges, cargo, movement byte, threshold, pulse
  beat, Spike inactivity, and medium firing

**Existing shared strategy**

- Flipper `EightAdvanceTurn`, rim pursuit, Tanker cargo, Egg/admission, ledger,
  reconstruction, Pulsar threshold/speed, enemy firing, generic completion

**New reusable ROM strategy**

- none established

**Generic architecture extension**

- bounded expansion of Level 17/18 mapping/catalog switch boundaries to
  Level 19; no target-level actor or bypass

**Project translation**

- raw speed and threshold host mappings, Flipper P-code timing, pulse/collision
  boundaries, and raw shot holdoff approximation

**Manual acceptance only**

- shape, projection, palette, movement feel, timing, firing pressure, and
  natural/direct progression

## 43. Manual acceptance requirements

Neal must manually verify medium/default Level 19 Shape 3 presentation,
cycle-1 palette, Flipper/Pulsar/Tanker/cargo behavior, firing pressure,
progression, direct startup, and termination. Easy/hard remain unaccepted.

## 44. Regression plan

Future permanent focused coverage should include mapping; canonical Shape 3;
cycle-1 palette; eligibility; 22-supply and 0/3-1/1-2/3 ranges; no Spikes;
Tanker cargo; `$19` route; `$A9` speed; `$14` Pulsar threshold; pulse and firing;
identity/reconstruction/completion; natural 18→19; direct 19; terminal boundary;
Level 20 unsupported; Levels 1-18; and unmodified recordings.

## 45. READY/NOT READY result

## READY FOR BOUNDED LEVEL 19 IMPLEMENTATION

## 46. Exact blockers

No medium/default implementation blocker was established. Easy/hard exact
production variants and the host ratio/visual acceptance remain future
milestone boundaries, not blockers to a medium/default bounded implementation.

## 47. Files/resources inspected

- [ROM disassembly](/C:/Users/nrnob/source/repos/Tempest/Reference/arcade/Tempest%20Rom%20Source%20code.md)
- [Behavior spec](/C:/Users/nrnob/source/repos/Tempest/TEMPEST-BEHAVIOR-SPEC.md)
- [Development notes](/C:/Users/nrnob/source/repos/Tempest/TEMPEST-DEVELOPMENT-NOTES.md)
- [Copilot instructions](/C:/Users/nrnob/source/repos/Tempest/.github/copilot-instructions.md)
- Level 17/18 wave and LevelConfiguration/progression source
- Level 18 closeout and ROM/Pulsar readiness reports

## 48. Commands/calculations run

- Git status, HEAD, branch, tracking, and ancestry verification
- ROM table arithmetic for effective lev and remap index
- Wave supply formula `20 + (lev - 17)`
- Threshold and shape/palette remap calculations
- Static C# seam/reuse searches

## 49. Production changed

No.

## 50. Tests changed

No.

## 51. Durable docs changed

No.

## 52. Stage/commit/push

None.

## 53. Level 20+ implementation

None.

## 54. Recommended next step

Implement a bounded medium/default Level 19 configuration through existing
selectors, canonical Shape 3 reuse, cycle-1 palette, and shared Pulsar/Flipper
actors. Stop at Level 19; defer all easy/hard variants and Level 20.
