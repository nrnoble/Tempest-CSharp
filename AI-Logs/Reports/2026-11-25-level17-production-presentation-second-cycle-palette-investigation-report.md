# Level 17 production-presentation and second-cycle palette investigation

Date: 2026-11-25

Status: **NOT READY - presentation routing defect established; an evidence-backed
second-cycle host-palette translation is not established sufficiently to
implement without guessing.**

## 1. Executive summary

Level 17 is already routed through the production Atari adapter, not the
explicit legacy 4:3 projector. It is also not routed by the old unmapped
fallback branch: `AtariPresentationAdapter.HasModelFrameMapping(17)` is true,
and the renderer dispatches through `ProjectAtariPresentation`.

The presentation defect is more specific: Level 17 reuses the Shape 1
model-frame mapping and Level 1 metadata/calibration values, but it exits the
special Level 1 projection branch and enters the generic reciprocal branch in
`AtariPresentationAdapter.Project`. Thus it has Shape 1's geometry, metadata,
model-frame family and accepted calibration values, but not the same projection
function used by Level 1. This is the first proven Level 1/17 presentation
routing difference.

The Levels 17-32 raw palette block is verified:
`00 07 0B 08 44 03 0C 0C`. `setcolours` is also understood far enough to
establish index ordering and low/high nibble installation. The remaining
host-RGB blocker is that the local source/listing does not establish an
authoritative RGB meaning for the changed role nibbles. Existing C# slot hues
are only project translations.

## 2. Starting Git/worktree state

- Branch: `master`
- Tracking: `origin/master`
- Ahead/behind: `0/0`
- Pre-existing tracked and untracked Level 17 source/test state was preserved.
- The worktree was not reset, cleaned, restored, stashed, or staged.
- No production code, tests, durable documentation, ROM files, or protected
  gameplay assets were modified during this investigation.
- The interactive game was not launched.
- No broad test suite was run.

## 3. Gameplay-freeze confirmation

The gameplay freeze was preserved. No gameplay, enemy, Flipper, Pulsar, Tanker,
Spike, speed, random, ledger, progression, or level-selection state was
changed.

## 4. Manual presentation defect evidence

Neal reports that Level 17 is playable but appears visually like the
legacy/old tube presentation and does not visibly exhibit the expected
Levels 17-32 color scheme. Spikes and Pulsars remain visible.

This report treats the report as manual presentation defect evidence, not as
a substitute diagnosis. The code trace establishes the likely shape/path
difference and identifies a separate palette gap.

## 5. Level 17 effective presentation-selection trace

Interactive/direct startup defaults to Atari presentation unless an explicit
diagnostic selector requests otherwise. `Program.cs` constructs `VectorRenderer`
with `atariPresentationEnabled` from the presentation selection.

For Level 17:

1. `atariPresentationEnabled` is true in the normal/default production path.
2. `AtariPresentationAdapter.HasModelFrameMapping(17)` returns true.
3. `VectorRenderer.ProjectWorldPointWithoutExitCompensation` dispatches
   `ProjectAtariPresentation`.
4. It does not reach `ProjectWorldPointLegacy`.
5. It does not reach the explicit unmapped-level fallback.

Level 17 therefore uses the production Atari adapter, not the legacy 4:3
projector or an unmapped legacy fallback.

## 6. Level 1 effective presentation trace

For Level 1:

1. Atari presentation is enabled.
2. `HasModelFrameMapping(1)` is true.
3. `ProjectWorldPointWithoutExitCompensation` dispatches to
   `ProjectAtariPresentation`.
4. `AtariPresentationAdapter.Project` obtains model-frame mapping
   `(25/14, Y orientation +1)`.
5. It validates Level 1's ROM presentation metadata.
6. It reaches the explicit `displayedLevel == 1` branch.
7. That branch uses the accepted C# depth progression:
   - `depthScale = denominator / k`
   - `level1LocalScale = 1 / depthScale`
   - screen X is `LogicalCenterX + x / depthScale`
   - screen Y is
     `centerY + (tubeYGain * y + verticalOffset) / depthScale + 122`

## 7. First Level 1/17 routing difference

The first routing difference is inside `AtariPresentationAdapter.Project`.

Level 1 enters a dedicated branch. Level 17 does not. Level 17 instead reaches
the generic reciprocal projection branch, because no `displayedLevel == 17`
branch is present.

All shared inputs are otherwise aligned:

| Input | Level 1 | Level 17 |
|---|---|---|
| Board/shape | canonical row 0 | canonical row 0 |
| Model frame | `(25/14, +1)` | `(25/14, +1)` |
| Metadata row | row 0 Level 1 values | copies row 0 Level 1 values |
| Y gain | 1.4 | 1.4 |
| tube scale | 0.8 | 0.8 |
| vertical offset | 35 | 35 |
| depth-Y gain | 1 | 1 |

The exact structural defect is that Shape 1 reuse is implemented as shared
shape/model-frame/calibration data but Level 17 is not mapped to Level 1's
accepted projection branch.

## 8. Presentation table/catalog audit

Presentation data is spread across:

- `LevelLoader`/`LevelConfiguration` for canonical geometry and level identity;
- `RomTubeMappingCatalog` for ROM shape-row reuse;
- `RomPresentationMetadataCatalog` for model-frame metadata;
- `AtariPresentationCalibration` for manually accepted host calibration;
- `AtariPresentationAdapter` for model-frame/projection routing;
- `VectorRenderer` for dispatch and shared entity/tube projection.

Level 17 currently copies/carries the canonical Level 1 model-frame metadata
and calibration instead of using a shape-keyed shared object. It also has a
separate displayed-level branch in `AtariPresentationAdapter.Project`, where
there is no Level 17 branch.

## 9. Shape-specific data classification

The following should be shared Shape 1 data:

- canonical 16-point Level 1 geometry;
- tube topology/open-closed identity;
- Shape 1 model-frame mapping `(25/14, +1)`;
- accepted Shape 1 calibration Y gain, scale, vertical offset and depth-Y gain;
- tube and entity projection routing;
- entity registration to the tube's projected lanes;
- Shape 1 ExitZoom/approach geometry.

## 10. Level/cycle-specific data classification

The following should remain distinct:

- displayed HUD level 17;
- Level 17 gameplay configuration;
- palette/cycle index 1;
- Level 17 enemy composition and ledger state;
- cycle-2 host role translations;
- level/cycle transition activation.

No implementation should make Level 17 into copied Shape 1 geometry or copy
Level 17 calibration values into gameplay configuration.

## 11. Exact cause of legacy-looking Level 17

The exact code-level cause is not a legacy presentation selector. The normal
renderer reaches the Atari/production adapter.

The proven presentation difference is:

- Level 1 uses its dedicated accepted C# depth/projection branch;
- Level 17 reuses Shape 1 metadata/model-frame/calibration but falls into the
  generic reciprocal branch;
- cycle 1 color is cataloged but intentionally not translated into any
  palette-specific host colors.

Both effects can make the result look legacy or visually wrong:

1. projection mismatch explains why the tube placement/geometry may differ
   from accepted Level 1;
2. missing palette-role translation explains why the second-cycle scheme is
   not visibly changed.

The exact screenshot contribution cannot be isolated further without Neal's
visual comparison or a bounded implementation candidate.

## 12. Shape 1 accepted calibration values

Level 1 accepted host defaults are:

- tube Y gain `1.4`;
- tube scale `0.8`;
- vertical offset `35`;
- depth-Y gain `1`.

These are manually accepted project presentation values, not ROM constants.

## 13. Level 17 effective calibration values

Level 17 resolves to:

- tube Y gain `1.4`;
- tube scale `0.8`;
- vertical offset `35`;
- depth-Y gain `1`.

The absolute CLI controls remain overrides and are not shape-derived.

## 14. Calibration inheritance assessment

Level 17 calibration inheritance is correct numerically. It returns the exact
Shape 1/Level 1 accepted values.

It is not proof that Level 17 should use the Level 1 projection branch. Values
and routing are separate: inherited calibration does not currently repair the
Level 1-versus-Level 17 adapter difference.

## 15. Model-frame reuse assessment

Level 17 and Level 1 share:

- tube remap shape row 0;
- model-frame mapping `(25/14, +1)`;
- Level 1's raw scale/camera/Y2D/Open metadata.

The model-frame reuse is structurally present. The projection branch is not
shared; Level 17 needs a shape-keyed route to the accepted Level 1 branch,
not a copied point array or a level-specific affine transform.

## 16. Tube/entity registration assessment

All production tube and entity anchors use `ProjectWorldPoint`/the shared
renderer projector. Level 17 does not have a separate entity-coordinate
system.

Therefore the defect is registered through the same projector for the tube,
Blaster, Flippers, Tankers, Spikers, Pulsars, shots and Spikes. The first
structural fix should be shared projection routing, not separate per-entity
correction.

## 17. Verified second-cycle raw palette

The exact ROM-backed block is:

`00 07 0B 08 44 03 0C 0C`

It is selected for source Level 17 through the 16-level block mechanism.
The low/high nibble split produces:

| curcolor index | Low nibble | High nibble |
|---:|---:|---:|
| 0 | 0 | 0 |
| 1 | 7 | 0 |
| 2 | B | 0 |
| 3 | 8 | 0 |
| 4 | 4 | 4 |
| 5 | 3 | 0 |
| 6 | C | 0 |
| 7 | C | 0 |

## 18. ROM palette runtime-array trace

`setcolours` reads `curlevel`, derives the block index, walks eight block
entries in reverse, and writes:

- low nibbles to `$0019,Y`;
- low nibbles to `col_ram,Y`;
- high nibbles to `$0021,Y`;
- high nibbles to `$0808,Y`.

The runtime role array therefore uses `curcolor` indices 0-7. Known direct
assignments include:

- index 1: player;
- index 3: Flipper;
- index 4: Pulsar in normal Pulsar drawing;
- index 0: pulse-white/explosion or pulse fallback;
- other uses remain less directly established in the reviewed local listing.

## 19. Color-code/intensity semantics

The listing establishes nibble splitting and low/high register installation,
but it does not establish a complete Atari-to-RGB mapping or semantics for all
second-cycle nibbles.

The existing C# code therefore may not equate raw nibble values to RGB values.
The safest classification is:

- low/high nibble role selection: ROM-BACKED;
- slot-index use by C#: existing project translation;
- exact RGB/inensity meaning of nibbles above the accepted C# slot map:
  UNKNOWN.

## 20. First-cycle ROM-to-C# reconciliation

The first-cycle block is:

`00 04 08 0C C3 07 0B 0B`

The host accepted C# translations include:

- C# slot 0: white;
- C# slot 1: palette-0 Blaster yellow;
- C# slot 2: green;
- C# slot 3: red;
- C# slot 4: cyan;
- C# slot 5: magenta;
- C# slot 6: blue.

This reconciliation is project translation only. The listing identifies runtime
role slots but the exact Atari RGB output remains unresolved.

## 21. Second-cycle host-palette proposal

No sufficiently evidence-backed exact proposal is available.

The smallest future implementation seam should be:

- preserve the raw first/second cycle blocks;
- resolve palette index 0 or 1 at level/cycle boundary;
- add explicit host role resolvers for the known ROM role indices;
- preserve fixed transient and pulse-white behavior until a role-specific
  ROM-to-host mapping is established;
- use manual Neal comparison for the first cycle-2 RGB candidate.

The implementation should not invent a nibble-to-RGB table for nibbles such
as $07, $08, $0B or $0C without an authority.

## 22. Evidence classification for every proposed host color

No new second-cycle host RGB is proposed as implementation-ready:

- existing C# fixed entity colors: PROJECT TRANSLATION;
- existing slot 0-6 Super Zapper mapping: PROJECT TRANSLATION;
- same-index player $07 reuse in first/second cycle: direct ROM nibble reuse
  plus accepted C# yellow translation;
- second-cycle Flipper/Pulsar/tube nibble changes: ROM-BACKED values,
  exact host RGB UNKNOWN;
- pulse-white: existing C# behavior plus explicit ROM 0/4 Pulsar phase:
  existing project translation, not full first-color RGB proof.

## 23. Shape-versus-palette architecture assessment

The desired architecture is already partly separated:

- Shape uses `BoardIndex`, canonical geometry and ROM shape-row metadata;
- cycle uses `PaletteIndex`;
- gameplay remains separate.

The remaining needed split is:

- shape-level projection branch keyed to Shape 1, not displayed Level 17;
- palette-level resolver keyed to palette/cycle index;
- neither should alter the other.

## 24. Super Zapper/transient color interactions

The existing implementation separates normal colors, Super Zapper rail
indices and pulse behavior. The future implementation should preserve:

- Super Zapper transient rail override;
- occupied/player-lane handling;
- Pulsar lane handling winning the active transient selection;
- return to base cycle colors after transient completion;
- fixed/transient white and pulse behavior unless separately changed.

No second-cycle implementation is justified to alter these mechanics.

## 25. Pulsar pulse-white interaction

The ROM draw path selects color 4 normally and color 0 when the pulse state is
white. The second-cycle block gives index 4 low/high nibble `$44` and index 0
`$00`.

This establishes a second-cycle Pulsar base slot and pulse fallback slot but
not exact RGB output. Existing C# cyan Pulsar and white pulse should remain
fixed until a source-backed RGB mapping exists.

## 26. Level 16→17 transition palette timing

The ROM installs palette colors when level entry calls `setcolours`.
Candidate implementation timing is therefore:

- Level 16 ExitZoom may remain cycle 0;
- level entry completes the 16→17 transition;
- the newly entered Level 17 should select palette 1;
- stars/background/transients should remain independently controlled.

The exact source frame boundary is not further established by this pass.

## 27. Production-presentation test recommendations

Future tests should verify:

- no-option/direct Level 17 chooses production Atari path;
- no legacy fallback is logged or used;
- Level 17 Shape 1 projection equals a Shape 1 keyed route under identical
  camera inputs;
- Level 17 inherits Level 1 accepted defaults;
- runtime tube overrides remain absolute;
- model-frame/metadata is shared rather than copied.

## 28. Palette test recommendations

Future tests should verify:

- first-cycle raw block unchanged;
- second-cycle raw block exact;
- Level 17/33/49 bounded cycle selection;
- Level 16 remains cycle 0;
- Level 17 selects cycle 1;
- palette changes do not alter geometry;
- transient white/Super Zapper colors restore base palette;
- no palette 1 fallthrough silently reuses palette 0.

## 29. Gameplay-freeze regression recommendations

Keep the existing Level 17 gameplay, rim-Flipper, campaign, seed-1982 and
full-suite gates unchanged. A palette/presentation implementation must not
modify wave selection, Flipper P-code, Tanker cargo, Pulsar behavior or level
progression.

## 30. Legacy-development-path audit

Interactive/default use is production Atari. Legacy remains opt-in.

Level 17 direct startup and model-frame routing already avoid the explicit
legacy 4:3 selector. However, tests or future diagnostics must not compare
only through low-level legacy geometry or baseline portrait projection.
Production-presentation tests should instantiate the normal adapter and
explicitly assert no legacy fallback.

## 31. Smallest bounded implementation scope

The smallest evidence-backed implementation has two independent bounded
parts:

1. **Presentation correction**
   - route Shape 1's Level 17 reuse through the accepted Level 1 shape
     projection branch or extract a shared Shape 1 projection helper;
   - retain existing Level 1 and Level 17 metadata/calibration;
   - add no per-entity projection correction.

2. **Palette correction**
   - add a bounded palette resolver for raw blocks/cycle 0 and 1;
   - use explicit role resolvers for ROM color-index 0-7;
   - require a visually accepted project host-color table before filling the
     table's exact RGB values.

The implementation must remain gameplay-neutral.

## 32. Manual visual-acceptance questions

Neal's future implementation acceptance must answer:

- whether the Shape 1 projection route restores the accepted tube placement;
- whether cycle 1 is visibly distinct;
- which tube, Blaster, Flipper, Tanker, Spiker and Pulsar roles should change;
- whether Spikes remain an established fixed color;
- whether pulse-white and Super Zapper transient behavior remain correct;
- whether the Level 16→17 transition activates the new palette cleanly.

## 33. Remaining uncertainties

- exact RGB mapping for second-cycle nibbles $07, $08, $0B and $0C;
- exact semantic roles for every runtime `curcolor` index;
- authoritative source frame for the 16→17 palette activation;
- whether screenshot appearance is fully explained by projection or primarily
  by missing palette.

## 34. READY/NOT READY result

## NOT READY

Exact blockers:

- no established RGB mapping for all changed second-cycle role nibbles;
- no authoritative evidence that a particular host RGB assignment corresponds
  to raw $07/$08/$0B/$0C at the remaining roles;
- no bounded manual-visible palette candidate can be marked implementation-ready
  without adding a guessed RGB table.

The Shape 1 presentation routing defect is ready to narrow, but this milestone
must not combine it with an unsupported palette implementation.

## 35. Exact blockers

1. Second-cycle host RGB remains unresolved.
2. Runtime `curcolor` role coverage is only partial in the reviewed local source.
3. Existing C# role constants are project translations, not proof of arcade RGB.

## 36. Files/resources inspected

- [LevelConfiguration.cs](../../src/Tempest.Game/Levels/LevelConfiguration.cs)
- [LevelLoader.cs](../../src/Tempest.Game/Levels/LevelLoader.cs)
- [RomPresentationMetadata.cs](../../src/Tempest.Game/Levels/RomPresentationMetadata.cs)
- [AtariPresentationCalibration.cs](../../src/Tempest.Game/Rendering/AtariPresentationCalibration.cs)
- [AtariPresentationAdapter.cs](../../src/Tempest.Game/Rendering/AtariPresentationAdapter.cs)
- [VectorRenderer.cs](../../src/Tempest.Game/Rendering/VectorRenderer.cs)
- [Program.cs](../../src/Tempest.Game/Program.cs)
- [Tempest Rom Source code.md](../../Reference/arcade/Tempest%20Rom%20Source%20code.md)
- the six required prior reports and closeout report

## 37. Commands/calculations run

Static source/disassembly searches, focused file reads and nibble calculations
only. No broad test, interactive launch or gameplay simulation was run.

## 38. Production code changed

No.

## 39. Tests changed

No.

## 40. Durable documentation changed

No.

## 41. Stage/commit/push

None.

## 42. Gameplay changed

No.

## 43. Level 18+ work

None.
