# Level 8 Rev 2 ROM geometry continuation

Date: 2026-11-08
Result: **GEOMETRY BLOCKER RESOLVED — READY FOR A BOUNDED IMPLEMENTATION MILESTONE.**
Scope: read-only ROM investigation. No game, MAME, screenshots, recordings, or external/Incitti geometry was consulted.

## Executive findings

- The missing Level 8 tables are in the CPU program ROM `136002-118.k1`. Direct opcode matches establish its CPU base at `$B800`; table addresses map linearly to file offsets.
- Displayed Level 8 is zero-based `p1_level=$07`, parameter level `$08`. `lev_remap[7]=$07`, so Level 8 selects tube/shape row 7.
- Row 7 is an open 16-vertex polyline with 15 playable lanes, indexed 0-14. It runs from upper-left to lower-left, across a short bottom segment, and up to upper-right. These are mathematical coordinates, not a shape name inferred from visual references.
- The raw per-tube presentation values are `lev_scale=$18`, `lev_y3d=$B0`, `lev_y2d=$0060`, `lev_open=$FF`, and packed Flipper scale `$0240`. The tube-color lookup selects raw color index `$05`; the Level 1-16 color block is also recovered below. Neither is translated to RGB.
- A bounded C# coordinate translation is proposed: subtract `$80` from both coordinate bytes, preserve the ROM point order, then uniformly scale the largest absolute coordinate to 200 world units. This centers the geometry at `(0,0)` and yields `Continuous=false`. It is a project candidate, not literal ROM coordinates or validated screen presentation.
- The ROM along coordinate maps naturally to the C# `Level.Depth=400` interval `$F0` (far end, height 0) to `$10` (player end, height 400). Under that affine translation, starting Spike `$C8` becomes `71.4286` C# height—not 200 and not the existing Levels 4-7 value of 60. Treat the translation as a bounded candidate pending manual acceptance.
- The cargo boundary is unsigned: `$9B0B CMP #$20` followed by `BCS`. Equality belongs to the level-selected route, `$19`; values below `$20` use the straight `$07` entry. Ordinary Tankers release naturally on the first qualifying post-movement value in `$11..$1F`; a value of `$10` is already handled by the top-of-tube path.
- Rev 2 parameter bytes confirm the prior Level 8 supply/type findings. Flipper program `$19` is present verbatim and has no programmed reverse or Spike-test opcode.
- The open topology is **not fully supported by the current C# general architecture**: playable-lane count is correct, but several movement and rendering helpers still wrap across the open endpoints. This is a known implementation requirement, not an uncertainty about Level 8.

## 1. Starting Git state and records

- HEAD: `bc109b4597f83477f5c7998bd3a9e258c72c6edf` (`Close out accepted interactive speed diagnostic`).
- Branch: `master`, tracking `origin/master`.
- Initial `git status --short --branch` showed only `?? Reference/arcade/roms.rev2/`. This is the supplied, untracked, read-only ROM reference directory; it was not staged or otherwise changed.
- The complete 683-line substantive prompt was copied byte-for-byte to [the paired prompt record](../Prompts/2026-11-08-level8-rev2-rom-geometry-continuation-prompt.md); its SHA-256 matched the attachment and it contains exactly one standalone completion token.
- This report and that prompt are under ignored `AI-Logs/`. No other files were created or changed.
- No tests, builds, or gameplay were run; this was a read-only investigation.

## 2. Evidence and ROM mapping

Primary evidence was the exact local Rev 2 ROM files in `Reference/arcade/roms.rev2/` and the project-local [annotated ROM listing](../../Reference/arcade/Tempest%20Rom%20Source%20code.md). Existing Level 8 report findings were carried forward unless tested against the raw bytes. No internet disassembly, screenshots, video, MAME, Incitti source, or other Tempest implementation was used.

### Mapping method and confidence

The CPU mapping was not assumed from a guessed base. For example, the bytes listed at `$B944` match `136002-118.k1` at offset `$0144`, and the bytes at `$B967` match the same file at `$0167`; both yield base `$B800`. Likewise, listing bytes at `$C2E8` and `$C300` match `136002-119.lm1` at offsets `$02E8` and `$0300`, yielding base `$C000`. The other CPU images match listing instruction streams at the contiguous 2 KiB bases below. The Level 8 geometry tables are within the independently confirmed `$B800` image.

| CPU address range | File | Size | Rev 2 disposition |
|---|---|---:|---|
| `$9000-$97FF` | `136002-113.d1` | `$800` | Program |
| `$9800-$9FFF` | `136002-114.e1` | `$800` | Program |
| `$A000-$A7FF` | `136002-115.f1` | `$800` | Program |
| `$A800-$AFFF` | `136002-316.h1` | `$800` | Rev 2 program image; supersedes supplied `136002-116.h1` at this same slot |
| `$B000-$B7FF` | `136002-217.j1` | `$800` | Rev 2 program image in the otherwise absent 117-numbered slot |
| `$B800-$BFFF` | `136002-118.k1` | `$800` | Program; contains the level geometry and presentation tables |
| `$C000-$C7FF` | `136002-119.lm1` | `$800` | Program; contains color and level-parameter setup code/data |
| `$C800-$CFFF` | `136002-120.mn1` | `$800` | Program |
| `$D000-$D7FF` | `136002-121.p1` | `$800` | Program |
| `$D800-$DFFF` | `136002-222.r1` | `$800` | Rev 2 program image in the otherwise absent 122-numbered slot |

These are byte-linear mappings: `file offset = CPU address - range base`. No byte interleaving or byte swapping was observed in the CPU images. `136002-116.h1` is an alternate image for the `$A800` slot, not an additional adjacent chip: the annotated `$A800` stream matches Rev 2 `316.h1` more closely (1970 matched bytes versus 1967 for `116.h1` in the parsed instruction rows). `217.j1` and `222.r1` occupy the `$B000` and `$D800` program slots; the corresponding 117/122-numbered images are not in the supplied directory, so no byte-for-byte comparison with those absent versions was possible.

The two 2 KiB vector instruction/data ROMs are `136002-123.np3` at vector-generator address `$1000-$17FF` and `136002-124.r3` at `$1800-$1FFF`, in file order. The listing's vector calls (including targets `$10CA`, `$127E`, and `$14C2`) fall in this vector address space. The vector words remain in their on-file byte order; these files are not interleaved with the 6502 images. The listing also identifies CPU-visible vector RAM at `$2000-$2FFF` and its vector-generator-visible aliases at `$0000-$0FFF`; e.g. its `$2006` CPU buffer has VG address `$0006`. This is a separate address space, not a CPU-address-minus-base conversion.

The remaining small images are PROM/lookup devices, not linear 6502 program or vector instruction images: `136002-125.d7` (256 bytes), `136002-126.a1` (32 bytes), and `136002-127.e1` through `136002-132.l1` (256 bytes each). Their exact hardware lookup pin/address decodes are not described by the annotated listing and were not guessed. The relevant level color lookup is instead a byte table in the CPU ROM at `$C22D`.

### Local ROM inventory and SHA-256 fingerprints

Hashes were calculated read-only at the start and compared again after analysis; all names, sizes, and hashes matched. No expected CRC/SHA manifest for these ROM files was found in the repository, and no external checksum was consulted.

| File | Bytes | SHA-256 |
|---|---:|---|
| `136002-113.d1` | 2048 | `95FEDC18B27B1C90286F46E6FC4FEC422254E106E29231463C6FC25CD9B3247B` |
| `136002-114.e1` | 2048 | `617F8D44C37D48FE63F23EE3A5A0F1E774B741F5FCC9D3E5F8D8F09BEF8D7114` |
| `136002-115.f1` | 2048 | `AFFD6866C08A55021A44B63E18069D880A8D3947F8EBAC4095A1BBE0461752A8` |
| `136002-116.h1` | 2048 | `E03DDAFE55597F570BEBE083BF10CE70E6FE52BBDC35573C3465470B3C52D171` |
| `136002-118.k1` | 2048 | `1F993280495B71AED969C55182E1A08D0F326DBB2ABDEC5269F12E01CA972D6F` |
| `136002-119.lm1` | 2048 | `EC4BC8B5880B331169E48E02EC0D5E8B8931F861885C0F22DA65BC5C1677D2F0` |
| `136002-120.mn1` | 2048 | `708ACA85F6CAE6D0A41168B01816F7D13148447A44CEF0DBD6C3ABD6B87E7D80` |
| `136002-121.p1` | 2048 | `335938685430794746D086427C382C5A377BB0E4CA16EB12209D65EC85E15C6F` |
| `136002-123.np3` | 2048 | `F660F7B54D5B6D8B37C9965EBA18203752D2ECB021796AEE26CE5000C008AFC5` |
| `136002-124.r3` | 2048 | `063E6584151B5CAED160089F90A628D2BED28E43A5CF4F8147C520AF45996158` |
| `136002-125.d7` | 256 | `45FDCE76E631695940DA6A1D94517177403591046E83BBABDBB72C674906D361` |
| `136002-126.a1` | 32 | `14EFB14AC87A8DB219FB998E4CE14FA62488F1C9CFD882B41B9EB09EFFFEE373` |
| `136002-127.e1` | 256 | `2150566BF27FB038C9A19081BC6FBDAE49B3F0E932EB5328261E51A1B2CF2767` |
| `136002-128.f1` | 256 | `26CDD1D690C0033366FF3ABA5284898DD4BC3192F004EF6004EFCC17C8E0C5DF` |
| `136002-129.h1` | 256 | `67A8CF9A25938FA75FFB05E70FD4252CEA808565BF3EB778D6EA13859C82DBC8` |
| `136002-130.j1` | 256 | `243E3AA7CD48920599DD01BF810A9CE38148E036E85A836A101E27D126563622` |
| `136002-131.k1` | 256 | `68B5EC6328BC5585930F5B7A617A4B2C3FFC961634D1DAE2841B8E8E681DD80D` |
| `136002-132.l1` | 256 | `42C7C60353DDC1A12344D6DB4617D2D4FD59E805FA2967736BB78E084A8F1109` |
| `136002-217.j1` | 2048 | `E529431A7896ECD55B345AF8F6F01A9A2A7AA4567C9C844F4D80074BF6A082F0` |
| `136002-222.r1` | 2048 | `D0D9C783415285F458F2FA60FC249789AFC9A62AEE62B2AEDADECBD18380607A` |
| `136002-316.h1` | 2048 | `5DF7E7431BA7D044CDFDF37999D3739A08B6082D6E3FA4A5EC41FEFFBFBFB315` |

## 3. Level selector and recovered table bytes

`p1_level` is zero-based. For displayed Level 8 it is `$07`; the parameter loader adds one, yielding effective level `$08`. `get_tube_no` (`$C2E8`) uses the low-nibble position 7 and reads `lev_remap[7]`.

All geometry/presentation table addresses below are CPU addresses in `136002-118.k1`; file offsets are the CPU address minus `$B800`.

| Table | CPU address | File offset | Length | Raw bytes |
|---|---:|---:|---:|---|
| `lev_remap` | `$BC7C` | `$047C` | 16 | `00 01 02 03 04 05 06 07 0D 09 08 0C 0E 0F 0A 0B` |
| `lev_scale` | `$BC8C` | `$048C` | 16 | `18 1C 18 0F 18 18 18 18 0A 18 10 0F 18 0C 14 0A` |
| `lev_y3d` | `$BC9C` | `$049C` | 16 | `50 50 50 68 50 50 68 B0 A0 50 90 80 20 B0 60 A0` |
| `lev_y2d` | `$BCAC` | `$04AC` | 16 | `40 20 40 80 40 40 70 60 00 20 40 00 A0 40 40 00` |
| `lev_y2db` | `$BCBC` | `$04BC` | 16 | `FF FF FF FF FF FF FF 00 01 FF 00 00 FE 01 FF 01` |
| `lev_open` | `$BCCC` | `$04CC` | 16 | `00 00 00 00 00 00 00 FF FF FF FF 00 00 FF 00 FF` |
| `lev_fscale` | `$BCDC` | `$04DC` | 16 | `00 00 60 40 00 00 48 40 50 28 50 00 00 50 00 40` |
| `lev_fscale2` | `$BCEC` | `$04EC` | 16 | `04 04 03 04 04 04 03 04 05 04 04 04 04 04 04 05` |

For every 256-byte geometry table, the Level 8 shape row begins at table offset `7*16=$70`:

| Table | CPU table address | File offset | Table length | Shape-7 row offset |
|---|---:|---:|---:|---:|
| `lev_x` | `$B97C` | `$017C` | 256 | `$01EC` |
| `lev_y` | `$BA7C` | `$027C` | 256 | `$02EC` |
| `lev_angle` | `$BB7C` | `$037C` | 256 | `$03EC` |

These three row offsets are file offsets in `136002-118.k1`. The exact shape-7 bytes are:

| Point/lane index | Raw X | Raw Y | Raw angle |
|---:|---:|---:|---:|
| 0 | `10` | `F0` | `0D` |
| 1 | `1E` | `D0` | `0D` |
| 2 | `2C` | `B0` | `0D` |
| 3 | `3A` | `90` | `0D` |
| 4 | `48` | `70` | `0D` |
| 5 | `56` | `50` | `0D` |
| 6 | `64` | `30` | `0D` |
| 7 | `70` | `10` | `00` |
| 8 | `90` | `10` | `03` |
| 9 | `9E` | `30` | `03` |
| 10 | `AC` | `50` | `03` |
| 11 | `BA` | `70` | `03` |
| 12 | `C8` | `90` | `03` |
| 13 | `D6` | `B0` | `03` |
| 14 | `E4` | `D0` | `03` |
| 15 | `F0` | `F0` | `00` |

`lev_remap[7]=$07` selects shape row 7; it is not inferred from a familiar tube. `lev_open[7]=$FF` marks that row open. The ROM copies 16 vertices/angles but, when drawing an open tube, removes the closing connection. Thus it has 16 points and 15 lanes, not 16 playable lanes.

## 4. Geometry, directed angles, and topology

### Raw coordinate interpretation

The ROM point bytes are unsigned 8-bit coordinates centered on `$80`. Subtracting `$80` gives:

```text
point 0..15 (x,y):
(-112,112) (-98,80) (-84,48) (-70,16)
(-56,-16) (-42,-48) (-28,-80) (-16,-112)
(16,-112) (30,-80) (44,-48) (58,-16)
(72,16) (86,48) (100,80) (112,112)
```

Coordinates here use the C# screen-oriented convention (positive X right, positive Y down). All 16 raw points are distinct; the 15 segments connect consecutive points and have no nonadjacent crossing. The first and last points are endpoints, not a shared wrap vertex.

Playable lane `i` is edge `Pi -> P(i+1)`, for `i=0..14`. Its only ordinary neighbors are `i-1` and `i+1` when those indices are in `0..14`. Lane 0 ends at point 0; lane 14 ends at point 15. There is no lane 15 and no lane14-to-lane0 wrap.

### ROM angles and point-derived turns

`lev_angle` is a 16-sector direction value (`16` steps per full turn). For the 15 playable segments, the bytes are:

```text
0D 0D 0D 0D 0D 0D 0D 00 03 03 03 03 03 03 03
```

These represent approximate segment headings of 292.5 degrees for lanes 0-6, 0 degrees for lane 7, and 67.5 degrees for lanes 8-14. The last `$00` at point slot 15 has no outgoing playable lane. The quantized changes are +3 sectors (67.5 degrees) at lane6->7 and +3 sectors at lane7->8; other neighboring lane headings remain in the same sector.

Computing turns directly from the raw coordinate deltas gives the following signed screen-coordinate turns at interior vertices (positive is the chosen x-right/y-down convention):

| Lane transition | Shared vertex | Derived signed turn |
|---|---:|---:|
| 0->1 through 4->5 | 1..5 | 0 degrees |
| 5->6 | 6 | -3.0733 degrees |
| 6->7 | 7 | +69.4440 degrees |
| 7->8 | 8 | +66.3706 degrees |
| 8->9 through 12->13 | 9..13 | 0 degrees |
| 13->14 | 14 | +3.0733 degrees |

Reverse traversal has the same magnitudes with opposite signs. The small turns at vertices 6 and 14 come from integer coordinate rounding and are not represented by a change in the 16-sector ROM angle. The open polyline has no enclosed interior, so convex/reflex labels are not intrinsic; classifying it as a closed polygon would add a fictitious endpoint edge. In implementation, retain the directed geometric turns and open endpoints rather than inventing a closure.

The ROM's open-end behavior is explicit:

- Player rim movement (`$9779`) clamps its along position at `$00`/`$EF` rather than wrapping.
- Enemy direction reverses at the open endpoints (`rev_if_edge`, `$9EAB` onward): segment 0 or the last playable segment 14 changes the movement direction.
- Tanker cargo lane selection at `$A0B2-$A0E9` corrects an attempted segment-15 wrap to an open endpoint.
- Tube rendering decrements the 16-segment count once when `open_level` is set (`$C395-$C39B`).

### ROM presentation values for shape 7

From the per-tube tables above:

- `lev_scale[7]=$18`. The setup code at `$C23F-$C251` converts it to byte negation `$E8`, then computes `$10-$E8=$28` for its vector scaling setup. This is not a direct C# pixel scale.
- `lev_y3d[7]=$B0` (176), copied to the ROM `y3d` camera-height field.
- `lev_y2db[7]:lev_y2d[7]=$00:$60`, i.e. signed 16-bit `$0060` (+96); the loader uses this as the 2-D Y-offset value.
- `lev_fscale2[7]<<7 | lev_fscale[7] = $04<<7 | $40 = $0240`.
- There is no separate per-tube X-center table. The point arrays are centered around `$80`; their row-7 coordinate bounding box is symmetric at `$80` on both axes.

The ROM 2-D/3-D offsets and reciprocal scale are known, but their exact pixel-equivalent mapping to the project's different perspective renderer is not established by these bytes alone.

## 5. Proposed normalized C# geometry

**PROJECT TRANSLATION CANDIDATE** (not raw ROM coordinates):

1. Keep all 16 points in their ROM order.
2. Center each axis by subtracting `$80`.
3. Use uniform factor `s=200/112=25/14` so the row's maximum absolute coordinate becomes 200 C# world units.
4. Set `Continuous=false`; retain 16 vertices and expose exactly 15 playable lanes, edges `0->1` through `14->15`.
5. Center the normalized point cloud at `(0,0)`. A provisional renderer placement can use `CenterY=300` (the 600-pixel logical canvas midpoint) and `YOffset=0`; these two values are project presentation choices, not ROM data. Keep the recovered `$0060` 2-D offset, `$B0` camera-height parameter, and `$18` reciprocal scale as explicit translation evidence rather than claiming they equal those C# fields.

With the proposed factor, the normalized points are:

```text
0  (-200.0,  200.0)   1  (-175.0,  142.9)
2  (-150.0,   85.7)   3  (-125.0,   28.6)
4  (-100.0,  -28.6)   5  ( -75.0,  -85.7)
6  ( -50.0, -142.9)   7  ( -28.6, -200.0)
8  (  28.6, -200.0)   9  (  53.6, -142.9)
10 (  78.6,  -85.7)  11  ( 103.6,  -28.6)
12 ( 128.6,   28.6)  13  ( 153.6,   85.7)
14 ( 178.6,  142.9)  15  ( 200.0,  200.0)
```

Formula, before display rounding: `xC#=(rawX-128)*25/14`, `yC#=(rawY-128)*25/14`. This preserves the raw path geometry and open ends. The factor 25/14 is a project sizing convention, not `lev_scale`. Final screen scale/centering remains subject to Neal's first-launch acceptance. Do not copy the Level 7 point set or its LevelDefinition offsets.

## 6. Open-tube C# audit

Reviewed `LevelDefinition`, `Level`, `Player`, `Egg`, `Flipper`, `Tanker`, `Spiker`, `Pulsar`, `Fuseball`, collision checks, and `VectorRenderer`.

| Surface | Current C# behavior | Level 8 consequence |
|---|---|---|
| `LevelDefinition.PlayableLaneCount` | Correctly returns `VertexCount-1` when `Continuous=false` | Produces 15 lanes |
| `Level.NextEdge` / `PreviousEdge` | Always wrap modulo all 16 points | Incorrectly admits lane/edge 15 and wraps endpoints |
| Player movement | `Move` and `MoveAlongTube` use modulo wrapping | Blaster can traverse the nonexistent closing connection |
| Egg/Flipper traversal | Uses the wrapping helpers; Flipper directed transitions also derive wrapped neighbors | Must reverse/stop at endpoints and must never enter lane 15 |
| Tanker cargo | Chooses opposite adjacent segments through `PreviousEdge`/`NextEdge` | At an endpoint it can choose the invalid wrap lane; port the ROM endpoint correction |
| Spiker | Its lane/Spike selection must stay within the playable set; generic edge helpers alone do not enforce that | Add explicit open-boundary rules |
| Collision | Existing tests are primarily same-lane checks and have no closing-edge concept | Valid if every entity is constrained to lanes 0-14; invalid wrapped lanes can miss/alter collisions |
| Flipper pivot geometry | Endpoint-hinge math exists, but neighbor selection still wraps and no open-end reversal is imposed by the geometry helper | Interior turns can use the derived values; endpoint handling must be added |
| Rendering | `DrawTube` joins every point to `NextEdge`, including point15 to point0, for both rims | Draws a false closing edge; omit that connection and use 15 lanes |
| Pulsar/Fuseball | Some classes already have open-boundary branches | Level 8 has zero Pulsars/Fuzzballs, so this does not repair the Flipper/player/render seams |

Conclusion: the configuration type can represent an open definition, but the general current architecture does not yet correctly support this exact topology. This must be an explicit implementation item. The collision predicates themselves need no artificial closing edge once actor lanes and transitions are restricted to `0..14`.

## 7. Tube color and Level color block

Both tables are in `136002-119.lm1`, whose CPU base is `$C000`:

| Table | CPU address | File offset | Length | Raw bytes |
|---|---:|---:|---:|---|
| `c22d` shape-to-tube-color map | `$C22D` | `$022D` | 8 | `06 03 01 04 00 05 05 05` |
| `c1fd` first 16-level color block | `$C1FD` | `$01FD` | 8 | `00 04 08 0C C3 07 0B 0B` |

`curtube=$07`; renderer setup masks it to 3 bits and reads `c22d[7]=$05`, the Level 8 tube/palette selection. This is an index, not an RGB value.

For color RAM setup, `p1_level=$07` selects the first block and starts at byte index 7, then walks down to index 0. The eight bytes are written in that reverse slot order; for example, slot 7 receives `$0B`, slot 4 `$C3`, and slot 0 `$00`. This establishes the ROM color-block data, not the colors those nibbles produce.

Current C# `PaletteIndex`/renderer mappings do not establish an accepted equivalent for ROM index `$05` or these bytes; the tube is currently drawn with project colors. No RGB was invented. The color-index-to-project-color choice remains a manual presentation decision.

## 8. Starting Spike `$C8`

### ROM coordinate meaning

- Effective level 8's `wave_spikeht` record is in `136002-113.d1`: table at CPU `$959C`, file offset `$059C`, length 19, bytes `06 01 63 00 00 00 E0 D8 D4 D0 C8 C0 B8 B0 A8 A0 A0 A0 A8`.
- The record format is opcode `$06`, level range `$01-$63`, then 16 per-level bytes. Effective level `$08` uses zero-based payload index 7, which is `$C8`.
- `set_enm_and_spikes` (`$923A-$9245`) copies this value to all 16 ROM `spike_ht` slots.
- The same along coordinate is used by `player_along`, `enemy_along`, and player-shot positions. The player starts at `$10`; pending enemies enter at `$F0`. Enemy movement advances from `$F0` toward the player end. `spike_ht` is therefore a raw along-position, not a C# height or a pixel value.
- Spiker growth (`$9FC9-$9FE3`) replaces the lane's Spike position with the enemy's smaller along byte as it advances. A smaller ROM along value maps to a taller C# Spike.
- Rendering reads `spike_ht` when placing/drawing the Spike (`$C6C9-$C6F6`) and projects it along the tube. Zero is treated as no Spike.
- Player shots move toward larger along values. At `$A1FA-$A23E`, a shot at or beyond the Spike's current along value writes its current position back as the new Spike height; after the first erosion it moves at +5 rather than +9 raw units and is removed after the second erosion check. Thus erosion is position-driven, not a literal C# `-10` ROM unit.
- The level-end path counts remaining nonzero Spikes (`$A5E9-$A5F9`) and shows the `AVOID SPIKES` interstitial only when `curlevel<$07`. Level 8 has `curlevel=$07`, so it bypasses that interstitial. The inspected `spike_ht` references do not establish a separate standard-play player collision rule for Level 8.

### Bounded C# translation candidate

Current C# `Level.Depth=400`, with height 0 at the far end and 400 at the rim/player. Mapping ROM `$F0` to 0 and `$10` to 400 gives:

```text
height = (0xF0 - rawSpikeAlong) * 400 / (0xF0 - 0x10)
Level 8: (240 - 200) * 400 / 224 = 71.4286
```

This affine conversion is derived from the ROM's along endpoints and the C# depth convention. It is a **PROJECT TRANSLATION CANDIDATE**, pending manual confirmation that the project's world-depth projection is an adequate stand-in for the ROM along coordinate. Do not use 200 or silently reuse 60.

For context, the raw starting values at effective Levels 4-7 are `$E0,$D8,$D4,$D0`; the same formula gives approximately `28.57, 42.86, 50.00, 57.14`, while accepted C# Levels 4-7 currently use 60 throughout. Those existing constants are approximations, not evidence against the affine Level 8 mapping. Initialize C# Spikes only on the 15 playable Level 8 lanes; the ROM writes all 16 slots, including the open-end slot.

## 9. Tanker cargo `$20` boundary

`$9B07` is in `136002-114.e1` at file offset `$0307`. Its bytes through `$9B1D` are:

```text
84 36 A5 29 C9 20 A5 2B B0 07 A8 20 EE 9A B8 50 03 20 88 9A A4 36 60
```

The decisive sequence is `LDA $29; CMP #$20; LDA $2B; BCS $9B18`. `$29` holds the Tanker's current unsigned along-position high byte. `CMP` sets carry for `$29 >= $20`; `BCS` therefore includes equality.

- If `$29<$20`, the routine puts cargo type `$2B` in Y and calls `$9AEE`, which uses the type's default movement entry `$07` for a Flipper: straight movement per speed.
- If `$29>=$20`, it calls the normal type setup at `$9A88`; Flipper setup reads `flipper_move`, so Level 8 selects `$19`.
- The natural movement path at `$9C63-$9C96` calls `$A06F` only after a moved enemy position is below `$20` and still beyond `player_along=$10`. Equality with `$10` already goes to top-of-tube handling. Thus natural Tanker cargo release occurs at the first qualifying post-move byte `$11..$1F`, and uses `$07`; it is not a release at exactly `$20`.
- Player-shot destruction (`$A32C` via `$A3CA`) copies the Tanker's current along byte into `$29` before the common cargo routine. A shot-destroyed Tanker at `$20` or deeper selects `$19`; one below `$20` selects `$07`. `$20` equality belongs to `$19`.
- The common cargo routine `$A06F` retires the Tanker slot, selects its two cargo entries, and sends them through `$9B07`. The children start on opposite adjacent segments with opposite directions. The open-tube correction at `$A0B2-$A0E9` avoids lane 15 at endpoints.

This refines rather than contradicts the prior accepted Level 7 wording: “natural release just below `$20`” means a qualifying value in `$11..$1F`; the comparison itself is strict `<$20` for natural release and inclusive `>=$20` for the alternative movement entry. The historical report remains unchanged.

## 10. Level 8 Flipper program `$19`

The Level 8 `flipper_move` record is in `136002-113.d1` at CPU `$95F4`, file offset `$05F4`, length 19:

```text
06 01 63 07 0B 19 24 53 0B 24 19 53 87 24 19 53 07 87 24
```

Opcode `$06` spans levels `$01-$63`; the value for effective level `$08` is payload index 7, `$19`. The actual p-code entry at `$A110` is in `136002-115.f1`, file offset `$0110`, length 11:

```text
0C 00 12 00 14 0C 04 06 1B 06 18
```

Interpreted with the listing's p-code definitions:

1. `$19`: move per speed, then yield.
2. `$1B`: start a flip, then yield.
3. `$1D`: continue/end the flip; `$1E` moves per speed once between flips.
4. `$1F-$23`: conditional/branch sequence loops to `$19`.

The p-code interpreter executes as part of the enemy update loop (`$9B3A-$9B4F`). The route repeats a one-along-movement tick between successive flips. There is no reverse-direction opcode `$16` and no Spike-test opcode `$1C` in this program. The common open-end reversal remains applicable independently of the p-code.

Expected below-rim route: deep Tanker cargo selected at or beyond `$20` runs the repeated `$19` movement/flip cycle; natural near-rim cargo selected below `$20` uses straight `$07` below the rim and enters the common top-of-tube handling on arrival. Do not translate either path into Level 7's `$24` 2/3 route.

## 11. Supply, type, and prior findings

The following selected bytes are in `136002-113.d1`; CPU/file offsets are identical after subtracting `$9000`. The table records contain level ranges and values, so the raw bytes below are the relevant complete records, not a repeated full supply analysis.

| Parameter/table | CPU address / file offset | Raw bytes | Level 8 result |
|---|---:|---|---|
| `min_spikers` | `$9489 / $0489` | `04 01 04 00 00 00 01 02 05 10 02 02 11 13 00 02 14 20 01 02 23 27 01 02 2C 63 01 00` | 2 |
| `max_spikers` | `$94A5 / $04A5` | `04 01 06 00 00 00 02 03 04 02 07 0A 04 02 0B 10 03 02 14 19 02 04 1A 20 01 02 02 02 01 01 02 02 35 27 01 02 2B 63 01 00` | 4 |
| `min_flippers` | `$94CD / $04CD` | `02 01 04 01 02 05 63 00 00` | 0 |
| `max_flippers` | `$94D6 / $04D6` | `02 01 04 04 02 05 10 05 02 11 13 03 02 14 19 04 02 1A 63 05 00` | 5 |
| `min_tankers` | `$94EB / $04EB` | `04 01 04 00 00 01 00 02 05 10 01 02 11 20 01 02 21 27 01 02 28 63 01 00` | 1 |
| `max_tankers` | `$9503 / $0503` | `04 01 05 00 00 01 00 01 02 06 10 02 02 11 1A 01 02 1B 20 01 02 21 2C 02 02 2D 63 03` | 2 |
| `min_pulsars` | `$951F / $051F` | `00 02 11 20 02 02 21 63 01 00` | 0; active ranges start at `$11` |
| `max_pulsars` | `$9529 / $0529` | `04 11 20 05 03 02 02 02 02 02 02 02 02 02 02 02 03 04 02 02 21 63 03 00` | 0; active ranges start at `$11` |
| `min_fuzzballs` | `$9541 / $0541` | `02 0B 10 01 02 16 19 01 02 1B 63 01 00` | 0; active ranges start at `$0B` |
| `max_fuzzballs` | `$954E / $054E` | `02 0B 10 01 02 16 19 01 02 1B 20 01 02 21 27 04 02 28 63 03 00` | 0; active ranges start at `$0B` |
| `max_enm` | `$9598 / $0598` | `02 01 63 06` | 6 |
| `wave_spikeht` | `$959C / $059C` | `06 01 63 00 00 00 E0 D8 D4 D0 C8 C0 B8 B0 A8 A0 A0 A0 A8` | `$C8` |
| `wave_enemies` | `$95B3 / $05B3` | `04 01 10 0A 0C 0F 11 14 16 14 18 1B 1D 1B 18 1A 1C 1E 1B` | `$18` = 24 |
| `flipper_move` | `$95F4 / $05F4` | `06 01 63 07 0B 19 24 53 0B 24 19 53 87 24 19 53 07 87 24` | `$19` |
| `tanker_load+2` | `$9475 / $0475` | `02 01 20 01 02 21 28 03 02 29 63 02` | 1 |
| `tanker_load+3` | `$9481 / $0481` | `02 01 30 01 02 31 63 03` | 1 |

The reset/setup code at `$93D7-$93DC` writes 1 to `tanker_load[0]` and `[1]`; the two level tables above write 1 to entries `[2]` and `[3]` at effective level 8. Thus all four cargo slots select type 1 (Flipper), i.e. two Flippers per Tanker. The Medium/default `wave_enemies` value is 24. Prior Easy/Hard modifiers and six-active generic gate remain as previously documented; no Level 8-specific contradiction was found.

**Major parameter status: CONFIRMED BY REV 2 BINARY** for wave supply 24, Spike byte `$C8`, generic active maximum 6, Flipper 0/5, Tanker 1/2, Spiker 2/4, Pulsar 0/0, Fuzzball 0/0, four Flipper cargo slots, and `$19`.

## 12. Death/re-entry, progression, and terminal boundary

No newly recovered Level 8 table changes the prior report's conclusions:

- Preserve the Level 7-style generic death reconstruction: rebuild pending supply from active-below-rim, at-rim, and pending counts; discard movement/species identity; latent cargo is lost with its Tanker; already released cargo is active and included.
- Natural progression from Level 7 index 6 to Level 8 index 7 remains the existing campaign path. No new Level 8 transition mechanic was found.
- Level 8's `curlevel=$07` skips the `curlevel<$07` Avoid Spikes interstitial.
- Preserve the prior terminal Level 8 recommendation; do not implement Level 9.
- The exact initial Blaster lane on the open Level 8 board remains a project-transition choice; the raw geometry does not itself specify the existing C# startup lane.

No prior first-report finding was contradicted. The `$20` explanation above tightens the raw boundary semantics without changing the historical report.

## 13. Readiness, remaining unknowns, and next milestone

**The original geometry blocker is resolved. Level 8 is ready for a bounded implementation milestone**, with these items explicitly classified as project translations or acceptance tasks rather than ROM facts:

1. Add the 16-point shape-7 geometry with open topology and 15 lanes using the normalization formula above.
2. Implement open endpoint behavior consistently in player, Egg/enemy, Flipper/Tanker cargo, collision-valid lane indexing, and rendering; do not wrap lane 14 to lane 0.
3. Carry the recovered ROM scale/offset and color index separately from C# world/palette settings until those mappings are validated.
4. Use `$19` for the deep cargo Flipper path and `$07` for the below-`$20` path; implement the unsigned inclusive `$20` split.
5. Start with the derived Spike candidate `71.4286` on playable lanes 0-14, and test the affine along-coordinate conversion and ROM-shaped erosion behavior explicitly.
6. Preserve the previously established enemy supply, death/re-entry, Level 7-to-8 progression, and terminal Level 8 policies.

Remaining uncertainties are limited to: exact screen-space translation of the ROM reciprocal scale/camera/2-D offsets into this project's projection; RGB interpretation and C# color equivalent for ROM index `$05`/color-block nibbles; the best project starting Blaster lane for the open tube; and manual confirmation of the provisional Spike and presentation translations. None blocks implementing and testing the ROM-derived geometry/topology and behavior policy.

### Revised automated test plan

- Assert the remap/open/scale/offset/color bytes and every shape-7 point/angle against the recorded ROM offsets.
- Assert normalized point values/order, `Continuous=false`, 16 vertices, 15 playable lanes, edge connectivity, and no lane-14-to-lane-0 edge.
- Exercise Blaster traversal and enemy/Flipper/Tanker-cargo reversal at both endpoints; assert no player/entity/shot/Spike can enter lane 15.
- Assert rendering draws 15 near-rim and 15 far-rim connections without a closing edge; test collision at both endpoints and on each valid lane.
- Assert directed geometric pivots for the interior turns, plus no invalid endpoint pivot.
- Assert supply/type limits and two cargo Flippers; test raw cargo boundary at `$1F`, `$20`, and `$21`, including natural release and player-shot destruction.
- Assert the `$19` move/flip loop, absence of reversal/Spike-test behavior, and `$07` near-rim straight entry.
- Test starting Spike translation (`$C8` -> `71.4286` at depth 400), Spiker growth direction, shot erosion mapping, 15 playable starts, and Level 8 exit behavior.
- Test generic death reconstruction, natural Level 7 -> 8 progression, terminal Level 8 completion/restart, deterministic playback, and regressions proving Levels 1-7 remain unchanged.

### Revised manual acceptance checklist

After implementation and automated validation, Neal should verify the independently ROM-derived open tube, endpoint behavior, Blaster traversal, cargo movement, Flipper `$19`, Spike height/erosion, projection/scale, and tube/palette presentation. Do not open MAME or use a visual reference before Neal's first launch.

VS Code Copilot must not launch the game. Neal's launch command for that later acceptance is:

```text
dotnet run --project src\Tempest.Game -- --lives 10
```

## 14. Final state

- Report created at `AI-Logs/Reports/2026-11-08-level8-rev2-rom-geometry-continuation-report.md`; paired prompt created at `AI-Logs/Prompts/2026-11-08-level8-rev2-rom-geometry-continuation-prompt.md`. Both are ignored by Git.
- No production code, tests, durable project documentation, ROM files, references, recordings, screenshots, or timestamps were intentionally modified.
- Final Git state is the same as the starting state: branch `master` at `bc109b4597f83477f5c7998bd3a9e258c72c6edf`, with only the pre-existing untracked `Reference/arcade/roms.rev2/` directory shown by `git status`.
- All 21 ROM filenames, sizes, and SHA-256 fingerprints matched the initial inventory; ROM binaries are unchanged.
- No tests/build/gameplay were run. No visual validation was performed.
