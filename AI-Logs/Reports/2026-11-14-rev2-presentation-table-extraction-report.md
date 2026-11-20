# Rev 2 Presentation Table Extraction

**Date:** 2026-11-14
**Status:** READ-ONLY EXTRACTION COMPLETE
**Primary classification:** ROM TABLE EXTRACTION COMPLETE
**Completion token:** `TEMPEST_REV2_PRESENTATION_TABLE_EXTRACTION_20261114`

## Scope and starting Git state

This milestone recovered the six authorized per-tube tables and their displayed-level mapping from the local Rev 2 ROM. No implementation or C# translation was performed.

Starting state:

- Branch: `master`; tracking `origin/master`.
- `HEAD` and `origin/master`: `385f603a94391756b2ca6cccd524dc17b281e782`.
- Index: clean.
- Exactly the ten tracked, unstaged candidate files documented in the two required prior reports were modified. The diff summary was 1,763 insertions and 88 deletions across those files.
- No non-ignored untracked files existed.
- The candidate paths and status matched the whole-game presentation calibration report and the prior Levels 1–9 ROM investigation report.

The substantive prompt was preserved at `AI-Logs/Prompts/2026-11-14-rev2-presentation-table-extraction-prompt.md`.

## ROM provenance and address mapping

Only the contents of `Reference/arcade/roms.rev2/136002-118.k1` were read. The local ROM directory's filenames and sizes were listed to identify the relevant image; no other ROM contents were opened. K1 is 2,048 bytes and its read-only SHA-256 is:

`1F993280495B71AED969C55182E1A08D0F326DBB2ABDEC5269F12E01CA972D6F`

The project-local Rev 2 ROM continuation report records the same K1 size and SHA-256, identifies this file as the `$B800-$BFFF` CPU program image, and gives the byte-linear mapping `file offset = CPU address - $B800`. That existing report also describes the local Rev 2 program-image set and its Rev 2-specific replacement chips. The current hash agrees with that local record. No official local checksum manifest was found, and no external checksum or source was consulted; provenance is established from the local Rev 2 inventory and corroborating project-local records, not external authentication.

The mapping was checked against the local annotated disassembly and this read:

| CPU address | Source bytes | K1 file offset | K1 bytes |
|---|---|---:|---|
| `$B944` | `A6 74 A4 75 A5 76 85 74` | `$0144` | `A6 74 A4 75 A5 76 85 74` |
| `$B967` | `AD 15 04 F0 09 AD 6F CE` | `$0167` | `AD 15 04 F0 09 AD 6F CE` |
| `$BCFC` onward | `3E 85 55 B9 35 04 85 56 B9 45 04 85 58 20 98 C0` | `$04FC` | `3E 85 55 B9 35 04 85 56 B9 45 04 85 58 20 98 C0` |

These exact opcode matches corroborate the K1 bank and byte offset conversion. The six target tables are contiguous from CPU `$BC7C` through `$BCDB`, hence file offsets `$047C` through `$04DB`.

## Source-backed table semantics

The annotated disassembly declares 16 entries per target table and identifies:

- `lev_remap`: order in which levels are encountered.
- `lev_scale`: reciprocal level scale.
- `lev_y3d`: 3-D Y offset, described as camera height.
- `lev_y2d`: low byte and `lev_y2db`: high byte of a signed 2-D Y offset.
- `lev_open`: `$00` for closed and `$FF` for open.

`get_tube_no` reduces the level number to its within-16 index, reads `lev_remap[index]`, and stores that result as `curtube`. The setup at `$C235` calls this routine and loads the presentation arrays by `curtube`. The game level value used here is zero-based: displayed Level 1 has input index 0, and displayed Level 16 has input index 15.

## Extracted tables

Each target table is a 16-byte array. All raw bytes below were read directly from K1 at the stated mapped offsets.

| Source label | CPU address | File offset | Width / count | Raw bytes, indices 0–15 |
|---|---:|---:|---|---|
| `lev_remap` | `$BC7C` | `$047C` | 1 byte × 16 | `00 01 02 03 04 05 06 07 0D 09 08 0C 0E 0F 0A 0B` |
| `lev_scale` | `$BC8C` | `$048C` | 1 byte × 16 | `18 1C 18 0F 18 18 18 18 0A 18 10 0F 18 0C 14 0A` |
| `lev_y3d` | `$BC9C` | `$049C` | 1 byte × 16 | `50 50 50 68 50 50 68 B0 A0 50 90 80 20 B0 60 A0` |
| `lev_y2d` (low) | `$BCAC` | `$04AC` | 1 byte × 16 | `40 20 40 80 40 40 70 60 00 20 40 00 A0 40 40 00` |
| `lev_y2db` (high) | `$BCBC` | `$04BC` | 1 byte × 16 | `FF FF FF FF FF FF FF 00 01 FF 00 00 FE 01 FF 01` |
| `lev_open` | `$BCCC` | `$04CC` | 1 byte × 16 | `00 00 00 00 00 00 00 FF FF FF FF 00 00 FF 00 FF` |

The low/high Y bytes are combined as little-endian `high:low`. Since the source identifies the combined field as signed, the following are signed 16-bit two's-complement interpretations; no physical units are assigned.

| Tube index | Low | High | Combined hex | Signed decimal |
|---:|---:|---:|---:|---:|
| 0 | `40` | `FF` | `FF40` | -192 |
| 1 | `20` | `FF` | `FF20` | -224 |
| 2 | `40` | `FF` | `FF40` | -192 |
| 3 | `80` | `FF` | `FF80` | -128 |
| 4 | `40` | `FF` | `FF40` | -192 |
| 5 | `40` | `FF` | `FF40` | -192 |
| 6 | `70` | `FF` | `FF70` | -144 |
| 7 | `60` | `00` | `0060` | 96 |
| 8 | `00` | `01` | `0100` | 256 |
| 9 | `20` | `FF` | `FF20` | -224 |
| 10 | `40` | `00` | `0040` | 64 |
| 11 | `00` | `00` | `0000` | 0 |
| 12 | `A0` | `FE` | `FEA0` | -352 |
| 13 | `40` | `01` | `0140` | 320 |
| 14 | `40` | `FF` | `FF40` | -192 |
| 15 | `00` | `01` | `0100` | 256 |

## Displayed-level mapping

The map uses the zero-based within-cycle level index as the `lev_remap` index. The resulting byte is the tube-table index, also zero-based.

| Displayed level | `lev_remap` index | Tube-table index |
|---:|---:|---:|
| 1 | 0 | 0 |
| 2 | 1 | 1 |
| 3 | 2 | 2 |
| 4 | 3 | 3 |
| 5 | 4 | 4 |
| 6 | 5 | 5 |
| 7 | 6 | 6 |
| 8 | 7 | 7 |
| 9 | 8 | 13 |
| 10 | 9 | 9 |
| 11 | 10 | 8 |
| 12 | 11 | 12 |
| 13 | 12 | 14 |
| 14 | 13 | 15 |
| 15 | 14 | 10 |
| 16 | 15 | 11 |

## Production Levels 1–9 presentation matrix

`Open` is interpreted solely using the source's encoding: `$00` closed, `$FF` open.

| Displayed level | Tube index | `lev_scale` | `lev_y3d` | `lev_y2d` low | `lev_y2db` high | Combined signed `lev_y2d` | `lev_open` |
|---:|---:|---:|---:|---:|---:|---:|---|
| 1 | 0 | `18` | `50` | `40` | `FF` | `FF40` (-192) | `00` closed |
| 2 | 1 | `1C` | `50` | `20` | `FF` | `FF20` (-224) | `00` closed |
| 3 | 2 | `18` | `50` | `40` | `FF` | `FF40` (-192) | `00` closed |
| 4 | 3 | `0F` | `68` | `80` | `FF` | `FF80` (-128) | `00` closed |
| 5 | 4 | `18` | `50` | `40` | `FF` | `FF40` (-192) | `00` closed |
| 6 | 5 | `18` | `50` | `40` | `FF` | `FF40` (-192) | `00` closed |
| 7 | 6 | `18` | `68` | `70` | `FF` | `FF70` (-144) | `00` closed |
| 8 | 7 | `18` | `B0` | `60` | `00` | `0060` (96) | `FF` open |
| 9 | 13 | `0C` | `B0` | `40` | `01` | `0140` (320) | `FF` open |

## C# cross-checks and evidence classification

**Level 9 — CROSS-VERIFIED:** The extracted Level 9 row is tube index 13 and exactly matches the existing `Level9RomEvidence` values in `src/Tempest.Game/Levels/LevelLoader.cs`: scale `$0C`, Y3D `$B0`, Y2D `$0140` (`$40` low, `$01` high), and open `$FF`. The existing shape remap `$0D` also agrees with `lev_remap[8]`. This verifies the table selection and raw fields, not any C# mathematical or visual translation.

**Level 8 — no independent C# camera-table cross-check available.** The current C# source search found no explicitly labeled Level 8 `lev_scale`, `lev_y3d`, `lev_y2d`/`lev_y2db`, or `lev_open` evidence. The existing Level 8 holdoff reference is unrelated. No camera value was inferred from `CenterY` or `YOffset`.

Classifications:

- **ROM-BACKED:** The six table byte arrays, including their raw field values and the mapped Level 1–16 table selections.
- **SOURCE-BACKED:** Table labels, entry counts, field semantics, signed little-endian interpretation, and the `get_tube_no`/`curtube` selection flow.
- **CROSS-VERIFIED:** Level 9 raw values and remap agree with independently recorded C# ROM evidence.
- **PROJECT TRANSLATION:** Any existing C# presentation geometry or placement values remain project translations; they are not established as literal ROM fields by this extraction.
- **HYPOTHESIS:** No hypothesis was needed to report these raw fields. Their mathematical conversion to C# remains unresolved and is outside this milestone.

The exact evidence now available for a separate translation investigation is the complete 16-entry raw dataset for all six fields, the one-based displayed-level to zero-based tube mapping, the signed 16-bit Y2D interpretations, and source-backed roles for scale, camera-height, post-projection Y placement, and topology. This does not select a projection equation or a C# constant.

## Validation and final state

This was a bounded read-only extraction. No build, test, or interactive game was run; none was required for byte extraction.

Final Git state:

- Branch remains `master`; `HEAD` and `origin/master` remain `385f603a94391756b2ca6cccd524dc17b281e782`.
- Index remains clean.
- The same ten pre-existing candidate source/test files remain modified and unstaged; this task did not modify them.
- No other non-ignored untracked files were introduced.
- Nothing was staged, committed, or pushed.
- Only `136002-118.k1` ROM contents were read; it was not modified.
- No screenshots, images, MAME frames, videos, gameplay recordings, Gemini material, Incitti source, or web sources were inspected.
