# Level 8 preimplementation reconciliation investigation

Date: 2026-11-11

Result: **READY FOR BOUNDED LEVEL 8 IMPLEMENTATION** — the original evidence
blockers were subsequently resolved. **Scope correction:** Level 8 is already
implemented in the requested starting checkpoint and its production behavior
is documented as manually accepted. This report does not authorize or perform
a second implementation.

## 1. Starting Git checkpoint and cleanliness

- Branch/status: `## master...origin/master`.
- `HEAD` and `origin/master`: `037b3f9877f38d0182a3eba4fb8a8f0279e4f1a9`
  (`Complete accepted Level 7 behavior`).
- `git diff --stat` and `git diff --cached --stat` were empty.
- The Level 8 code, tests, and accepted documentation are already present in
  the clean checkpoint. The expected Git checkpoint is correct; it is the
  task's “preimplementation” premise that is stale.
- The historical Level 8 closeout commit recorded in its report is an
  ancestor of the current `HEAD`.

## 2. Prior Level 8 report reconciliation

The required [2026-11-08 preimplementation report](2026-11-08-level8-rom-preimplementation-investigation-report.md)
was read completely. Its **PARTIAL / BLOCKED** result was accurate for its
source set and checkpoint: the annotated ROM listing leaves geometry tables
as `.chunk` declarations, so that report could not establish the geometry,
palette, Spike translation, or cargo equality branch.

The later [Rev. 2 geometry continuation report](2026-11-08-level8-rev2-rom-geometry-continuation-report.md)
records a bounded local-binary table extraction and resolves the geometry
blocker. The later [accepted production closeout report](2026-11-08-level8-accepted-production-closeout-report.md),
[TEMPEST-BEHAVIOR-SPEC.md](../../TEMPEST-BEHAVIOR-SPEC.md), and
[TEMPEST-DEVELOPMENT-NOTES.md](../../TEMPEST-DEVELOPMENT-NOTES.md) document
implementation, automated validation, and Neal's manual acceptance. These
later findings supersede the earlier readiness status without rewriting that
historical report.

The earlier cargo question was resolved by the later instruction-level trace:
the unsigned `$20` comparison is inclusive for the level-selected route.
Natural release below `$20` uses `$07`; shot destruction at or above `$20`
uses `$19`.

## 3. Sources inspected and evidence authority

- Project-local [annotated Rev. 2 ROM listing](../../Reference/arcade/Tempest%20Rom%20Source%20code.md),
  including level setup, `$9B07`, p-code at `$A110`, speed setup, firing,
  geometry-table declarations, and rendering/topology routines.
- The two durable behavior/development records cited above.
- The prior preimplementation, ROM-geometry continuation, and accepted
  production closeout reports.
- Current Level 8 implementation and tests: `LevelConfiguration`,
  `LevelLoader`, `LevelProgression`, `Level8WaveSelection`, `Level`,
  `Level3WaveLedger`, `Flipper`, `Tanker`, `Spiker`, `EnemyBullet`,
  `TempestGame`, and `Level8ProductionTests`.
- The project-local Incitti reference directory contains level-set files for
  Levels 1-7. It was not used to establish Level 8 geometry or override ROM
  evidence; stronger Rev. 2 evidence is already recorded.

The Rev. 2 raw binary extraction is documented in the later project-local
report and durable records. The ROM copies are protected: they were not opened,
enumerated, hashed, or re-verified in this task. No Internet source, MAME,
gameplay recording, or protected screenshot was opened.

## 4. Geometry and topology findings

**REV-2-BINARY-CONFIRMED / RAW-ROM-DATA:** Displayed Level 8 is internal index
`$07`; the parameter level is `$08`; `lev_remap[7]=$07`. Shape row 7 has
`lev_open[7]=$FF`, 16 ordered vertices, and 15 lanes (0-14), with no closing
edge from vertex 15 to vertex 0.

Raw shape-7 coordinates and angles, as recorded in the later binary-backed
report:

```text
X:     10 1E 2C 3A 48 56 64 70 90 9E AC BA C8 D6 E4 F0
Y:     F0 D0 B0 90 70 50 30 10 10 30 50 70 90 B0 D0 F0
Angle: 0D 0D 0D 0D 0D 0D 0D 00 03 03 03 03 03 03 03 00
```

Lane `i` is edge `Pi -> P(i+1)`, `i=0..14`. Open endpoints clamp player
traversal; enemy reversal is a separate endpoint rule. The current C# level
model and tests enforce 15 playable lanes, no lane 15, and no endpoint wrap.
The source angle sectors and geometric turns are recorded in the continuation
report; an open polyline has no intrinsic polygon-wide convex/reflex
classification.

## 5. Tube palette and presentation

**ROM-LISTING-BACKED / REV-2-BINARY-CONFIRMED:** Shape-row values are
`lev_scale=$18`, `lev_y3d=$B0`, `lev_y2d=$0060`, open flag `$FF`, and packed
Flipper scale `$0240`. For `curtube=$07`, the tube-color lookup selects raw
index `$05`; the Level 1-16 color block is `00 04 08 0C C3 07 0B 0B`.
Neither the raw index nor color nibbles establish C# RGB values.

**PROJECT TRANSLATION / SCREENSHOT-SUPPORTED ORIENTATION:** C# subtracts
`$80`, scales coordinates by `25/14`, and reflects centered Y for screen-down
projection. The Y reflection is documented against Neal's authorized
orientation observation. `CenterY=300`, `YOffset=0`, the blue project tube
color, and Blaster start lane 7 remain project presentation/start choices,
not ROM constants. The final project behavior notes record manual acceptance
of shape, orientation, and endpoint presentation.

## 6. Level 8 ROM parameter reconciliation

For Medium/default Level 8:

| ROM parameter | Value | Meaning / current C# boundary |
|---|---:|---|
| `wave_enemies` | `$18` = 24 | Generic pending supply; implemented as 24 |
| `wave_spikeht` | `$C8` | Raw along-position copied to 16 ROM slots |
| `max_enm` | `$06` | Six-unit ordinary generic active-entry gate |
| Flipper min/max | 0 / 5 | No ordinary Flipper entry; cap includes cargo |
| Tanker min/max | 1 / 2 | Ordinary selection bounds |
| Spiker min/max | 2 / 4 | Ordinary selection bounds |
| Pulsar/Fuzzball min/max | 0 / 0 | Neither ordinarily eligible |
| `tanker_load[0..3]` | 1, 1, 1, 1 | Flipper cargo; two per Tanker |
| `flipper_move` | `$19` | Level-selected Flipper movement entry |
| `shot_holdoff` | 59 | ROM ticks; C# retains its documented 62-tick cadence |
| `enm_shotmax` | 3 | Four ROM enemy-shot slots; C# capacity is four |
| Flipper/Tanker speed input | `$B1` | Tanker receives converted Flipper speed |
| Spiker/projectile speed input | `$C0` | Raw inputs passed through ROM `crack_speed` |
| `flip_top_accel` | 2 | ROM table value |

The existing production configuration targets the project Medium/default
policy; Easy/Hard ROM adjustments are not part of this bounded production
configuration.

## 7. `$19` Flipper movement trace and observable pattern

The table selects `$19` for effective Level `$08`. At `$A110`, the p-code is:

```text
0C 00 12 00 14 0C 04 06 1B 06 18
```

Using the listing's opcode definitions: `$19` moves per speed and yields;
`$1B` starts a flip and yields; `$1D` continues/ends the flip; `$1E` moves per
speed; `$1F` tests flip continuation; `$20/$21` loop while the transition is
active; `$22/$23` branch back to `$19`. Thus the route starts each cycle with
an along move, starts a directed flip, and continues along movement on active
flip-continuation ticks before repeating. The program itself has no `$16`
direction-reversal opcode and no `$1C` Spike-test opcode. The independent
open-end rule can reverse direction at an endpoint.

The human-observable candidate is repeated one-step/flip behavior: deep
Tanker-released Flippers continue climbing during the pivot and begin the next
flip cycle after the preceding one completes. This is not Level 7's `$24`
2/3/reverse route. They remain Flippers subject to the common firing logic;
`$19` does not itself grant or suppress firing. Ordinary bottom Flippers are
not selected on Level 8, so the route is relevant to eligible Tanker cargo.
Neal's accepted project notes record manual acceptance of the resulting
one-along-interval/one-flip gameplay behavior, not literal C# tick timing.

## 8. `$07` / `$19` Tanker-cargo boundary trace

At `$9B07`, the relevant instructions are `LDA $29; CMP #$20; ...; BCS
$9B18`. `$29` is the Tanker's unsigned ROM along-position. `BCS` is taken for
`$29 >= $20`, including equality. Values below `$20` use the Flipper default
entry `$07`; values at/above `$20` use normal level setup, which selects `$19`.

Natural Tanker movement releases after a qualifying post-movement coordinate
in `$11..$1F`; `$10` is already handled by top-of-tube logic. Natural release
therefore uses `$07`. Player-shot destruction passes the Tanker's current
along coordinate to the common release path: `<$20` uses `$07`, `>=$20` uses
`$19`. This is resolved in ROM coordinates; the C# height comparison is a
separate affine project translation.

## 9. Tanker / Spiker speed relationship

The ROM loads Flipper speed input `$B1` and passes it through `crack_speed`;
setup copies the resulting converted Flipper speed fields to Tanker. Spiker
input is `$C0`, so its raw input differs; enemy projectile input is also `$C0`.
The fixed-point ROM conversion does not by itself establish C# units or prove
the visible Level 8 Spiker/Tanker rates equal.

The current C# candidate uses the existing Flipper/Tanker/Spiker systems and
their project speeds; none is claimed as a literal ROM conversion. The
Level 7 Tanker=Spiker relationship must not be copied as Level 8 ROM evidence.
No new speed observation is a blocker for the already accepted Level 8
candidate. If speed fidelity is reopened, Neal should frame-step comparable
Level 8 Tanker and Spiker traversals and separately compare projectile travel;
record observed rates without treating them as ROM constants.

## 10. Starting Spikes and translation status

ROM setup copies raw `$C8` to all 16 Spike slots. Shape row 7 is open, so the
current C# board has 15 playable Spike lanes (0-14); slot 15 has no playable
lane. The project initializes one Spike per playable lane.

The accepted translation maps `$F0` to height 0 and `$10` to depth 400:
`(0xF0 - rawAlong) * 400 / (0xF0 - 0x10)`, making `$C8` approximately
71.4286. This is a **PROJECT TRANSLATION / MANUALLY ACCEPTED PRESENTATION**,
not a claim that C# height equals ROM along-position. C# growth/erosion remains
an approximation; the ROM's erosion is position-driven. No Spike conversion
blocker remains for the accepted candidate.

## 11. Enemy selection, concurrency, and final-pending conversion

ROM minima require two Spikers and one Tanker before discretionary selection;
maxima are Spiker 4, Tanker 2, and Flipper 5. Ordinary bottom selection does
not admit Flippers, Pulsars, or Fuzzballs. Generic creation stops at six
active below-rim plus at-rim units. ROM selection after minima also considers
Spike heights and randomized selection.

`Level8WaveSelection` implements the Level 8 minima/maxima and six-unit gate;
after minima it uses a deterministic seeded project selection rather than
claiming the exact ROM Spike-height/random-start policy. It counts released
Flippers and latent cargo against the Flipper type cap before selecting more
Tankers. The generic gate is not an extra cargo-pair gate, so Tanker cargo can
raise total active units above six.

At zero generic pending supply, the final Spiker can convert in place to a
Tanker, retaining the original identity and adding two latent Flipper cargo
identities without adding supply. The current tests cover same-ID conversion,
two cargo slots, zero remaining pending, type limits, and the active gate.

## 12. Tanker cargo lifecycle

Each Level 8 Tanker has two latent Flippers, which do not add generic supply.
On release the carrier is removed and its pair is materialized on opposing
adjacent routes/directions; open-end corrections keep both children on valid
lanes. The pair does not consume new generic pending units. Capacity checks
prevent excessive Tanker commitments, while a released pair is not limited by
the ordinary six-unit entry gate.

Natural release uses `$07`; shot destruction follows the ROM-coordinate
`$20` split; only deep shot-released cargo selects `$19`. The existing C#
tests exercise both endpoints, distinct cargo identity, opposite directions,
the `$1F/$20/$21` equality boundary, natural straight movement, and deep
constant-flip behavior. Current behavior and its evidence boundary are
documented as accepted.

## 13. Death and reconstruction semantics

ROM reconstruction combines active-below-rim, at-rim, and pending counts,
then discards species/movement identity. Latent cargo is not included and is
lost with its Tanker; released Flippers are active obligations and return as
generic unresolved units. Terminal/resolved outcomes stay resolved. A final
Spiker-to-Tanker conversion may occur again after the reconstructed supply is
consumed.

The current Level 8 configuration uses the generic Level 7-style ledger
reconstruction: clear unresolved species selection, drop latent cargo,
reconstitute released unresolved cargo as generic obligations, preserve
terminal outcomes, and prevent duplicate identities. Existing tests cover
released and latent cargo and repeated reset idempotence.

## 14. Enemy firing

ROM Level 8 holdoff is 59 ticks; `enm_shotmax=3` means four simultaneous shot
slots. The common chance thresholds are `$00,$E0,$F0,$FA,$FF`, indexed by
existing shot count. Flippers, Tankers, and Spikers carry firing eligibility;
Pulsars and Fuzzballs are absent under the Level 8 type limits. Projectile
speed input `$C0` is converted by `crack_speed`.

The current project reuses Flipper/Tanker/Spiker and shared `EnemyBullet`
updates. C# uses four active slots but retains 62-tick Flipper holdoff and
projectile movement/cadence approximations; Tanker firing follows the existing
shared C#/secondary-reference policy and Spiker firing retains its C# growth
retreat route. In the ROM, a successful shot reloads the firing enemy's delay
with 59; the common per-tick firing check can later fire again if the enemy
remains eligible, a slot is available, and the count-indexed random threshold
passes. This is explicitly not a ROM-to-C# timing conversion. The accepted
project documentation records no further blocking firing issue.

## 15. Progression, direct startup, and campaign boundary

`LevelProgression` links natural Level 7 to 8, resolves direct `--level 8`,
and defines Level 8 as terminal. Fresh-wave setup preserves score/lives,
restores Super Zapper charges, and uses the existing approach. Initial lane 7
is a project choice. Standard completion/restart returns to Level 1; no Level
9 is loaded. Recording v2 and visible/headless playback use the common
starting-level configuration.

These are already implemented and covered by Level 8 progression, direct
startup, terminal restart, Super Zapper, and recording/playback tests. Durable
notes claim automated Level 7-to-8 coverage but do not claim a separate manual
acceptance of that exact transition.

## 16. Accepted Levels 1-7 regression constraints

Levels 1-7 remain accepted/frozen. Level 8 has its own production
configuration, wave selection, `$19` cargo route, and open-topology seams.
Do not generalize Level 8 policy into earlier levels or tune them as part of
this reconciliation. The Level 8 closeout report records Levels 1-7 playback
regression checks; the Level 7 closeout records the accepted Level 7 behavior
and compatibility obligations.

## 17. Current C# architecture reuse map

| ROM / arcade mechanic | Existing reusable C# component | Level 8-specific behavior already present | Evidence / risk |
|---|---|---|---|
| Shape, points, projection | `LevelDefinition`, `LevelLoader`, `Projection` | Shape row 7 translation and Y reflection | Raw bytes are ROM-backed; scale/offset are project translation |
| Open lanes and rim | `Level`, `Player`, `VectorRenderer` | 15 lanes, endpoint clamp/reversal, no false closing edge | Topology ROM-backed; endpoint/presentation behavior manually accepted |
| Wave data/progression | `LevelConfiguration`, `LevelProgression` | Supply 24, Level 7→8, terminal Level 8 | ROM-backed supply; campaign wiring is project behavior |
| Ordinary selection | `Level8WaveSelection`, `TempestGame` | Level 8 minima/maxima, gate, cargo-aware cap | Limits ROM-backed; post-minimum deterministic picker is project translation |
| Identity/re-entry | `Level3WaveLedger`, `Level.ResetAfterDeath` | Generic reconstruction, cargo accounting, conversion | ROM semantics plus project ledger mapping |
| `$19` / `$07` cargo | `Tanker`, `Flipper`, `TempestGame` | Depth-specific route and open-end child mapping | ROM branch/program backed; C# tick/speed mapping approximate |
| Spiker and Spike | `Spiker`, `Spike`, `TempestGame` | Growth, generic conversion, 15 initial Spike locations | Raw `$C8` ROM-backed; height/growth/erosion are project translations |
| Enemy firing | `Flipper`, `Tanker`, `Spiker`, `EnemyBullet` | Shared C# bullets and shot pressure | Species/capacity inputs ROM-backed; C# cadence/speed approximate |
| Startup/playback | `GameplayStartupConfiguration`, `Program`, playback sessions | Direct Level 8, recording v2, visible/headless replay | Existing generic infrastructure; configuration tests cover it |
| Completion/restart | `TempestGame`, `LevelProgression` | Standard exit, terminal restart to Level 1 | Existing campaign policy; no Level 9 |

## 18. Explicit implementation-readiness decision

**READY FOR BOUNDED LEVEL 8 IMPLEMENTATION.** The later local Rev. 2 extraction
provides the essential geometry/topology, selected table values, `$19`
program, and `$20` cargo branch. Other mappings are explicitly project choices
or approximations, not fabricated ROM facts.

However, the current checkpoint already contains the Level 8 implementation
and a later manual-acceptance closeout. This is therefore a **completed
milestone encountered during a stale preimplementation request**, not an
authorization to implement or tune Level 8 again.

## 19. Bounded implementation specification (already present)

The existing candidate uses the 16 raw shape-7 points and 15 open lanes;
Medium supply 24 and four initial C# Eggs; Tanker/Spiker-only ordinary
eligibility with the stated minima/maxima and six-unit gate; two Flipper cargo
units per Tanker; `$07` for shallow/natural cargo and `$19` at/above `$20` on
deep shot destruction; generic death reconstruction; raw `$C8` mapped to
accepted project height 71.4286 on lanes 0-14; shared firing/campaign
infrastructure; natural Level 7-to-8 progression; and terminal Level 8
behavior. Preserve all known project-translation boundaries and Levels 1-7
behavior.

## 20. Blockers and exact next evidence

No evidence blocker remains for the bounded candidate; **no new Neal
observation is requested before implementation**, because that implementation
and its manual acceptance already exist. Remaining non-literal mappings are
known boundaries, not missing essentials: ROM-to-C# pixel scale/camera
translation, RGB interpretation, exact post-minimum Spike-height selection,
speed/tick conversion, and ROM-shaped Spike erosion.

If a separate speed-fidelity milestone is authorized, Neal can frame-step
comparable Level 8 Tanker and Spiker climbs and projectile travel. Do not treat
that as a prerequisite to the already accepted candidate or alter it here.

## 21. Proposed automated implementation tests

The relevant coverage is already in `Level8ProductionTests` and shared
configuration/progression/playback tests. It includes raw-coordinate
translation and shape order, 15-lane topology and rendering, endpoint
movement/cargo, `$1F/$20/$21` cargo routing, `$19` movement, species minima/
maxima and six-unit gate, initial Spike supply/height, death reconstruction,
final-pending conversion, Level 7-to-8 progression, direct startup, terminal
restart, Super Zapper recharge, and recording/playback. The historical
accepted closeout report records its test/build results; no suite was run in
this read-only task.

## 22. Manual acceptance checklist

The durable accepted behavior spec records Neal's manual acceptance of tube
shape/orientation, open topology, spinner direction, endpoint behavior,
absence of wrap, supply/species/cargo behavior, shallow `$07` versus deep
`$19`, `$19` movement, starting Spike presentation, and active-exit firing,
shot/Spike interaction, hazards, and retry behavior.

If any of those accepted behaviors is reopened, re-check the relevant item with
a fresh Level 8 play/recording and frame-step subtle cargo motion. Do not
reopen or retune accepted behavior merely to pursue a cleaner design. The
natural Level 7-to-8 handoff has automated coverage; a separate manual result
is not claimed in the durable notes.

## 23. Files changed by this task

- Added the verbatim task prompt at
  `AI-Logs/Prompts/2026-11-11-level8-preimplementation-reconciliation-investigation-prompt.md`.
- Added this report at
  `AI-Logs/Reports/2026-11-11-level8-preimplementation-reconciliation-investigation-report.md`.
- No existing report was rewritten.

## 24. Final Git status

Final status and both diff-stat checks were re-run after saving these ignored
records. The tracked worktree and index remain clean on `master` at
`037b3f9877f38d0182a3eba4fb8a8f0279e4f1a9`, tracking the matching
`origin/master`.

## 25. Production/documentation/reference confirmation

No production code, tests, durable project documentation, references,
recordings, screenshots, or ROM files were modified. No game was launched and
no tests/build were run. The two AI log records are ignored historical/
working artifacts.

## 26. Commit/push confirmation

No files were staged. No commit or push occurred.
