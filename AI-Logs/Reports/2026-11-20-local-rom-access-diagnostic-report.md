# Local Rev. 2 ROM access diagnostic

## Access result

**PROMPT-IMPOSED BLOCK CONFIRMED**

## Evidence

On the current task's explicit bounded authorization, PowerShell opened only `Reference/arcade/roms.rev2/136002-118.k1`, read its first byte, and closed the stream. The operation succeeded and returned decimal `59` (`$3B`). The subsequent bounded verification found a 2,048-byte image and an exact match for the historical disassembly signature at file offset `$0167`. No refusal or technical access error was observed.

The user-provided task background states that the earlier Level 11 prompts prohibited access to the ROM directory. The successful authorized read, alongside the recorded Level 10 method, confirms that this diagnostic is not independently blocked by the tool or platform. No licensing explanation is inferred.

The tracked recovery method places the 2-KiB image at CPU base `$B800`, uses the signature at `$B967` to identify the image, and translates table addresses to file offsets by subtracting `$B800`. The textual disassembly defines the 16-by-16 geometry arrays and the adjacent per-shape tables. The tracked Level 10 recovery report establishes that geometry points are decoded in `P=0..15` order.

## Level 11 geometry

Displayed Level 11 maps to geometry lookup index `10`. ROM-backed extraction gives `lev_remap[10] = $08`, selecting shape row `8`.

| P | X | Y | Angle |
|---:|---:|---:|---:|
| 0 | 10 | C0 | 0E |
| 1 | 24 | A6 | 0D |
| 2 | 30 | 8A | 0C |
| 3 | 36 | 6A | 0D |
| 4 | 3E | 4A | 0D |
| 5 | 49 | 2F | 0D |
| 6 | 5A | 14 | 01 |
| 7 | 75 | 24 | 0F |
| 8 | 94 | 20 | 02 |
| 9 | A4 | 39 | 03 |
| 10 | AC | 59 | 03 |
| 11 | BA | 75 | 00 |
| 12 | DA | 72 | 03 |
| 13 | E2 | 90 | 03 |
| 14 | EA | B0 | 03 |
| 15 | F0 | D0 | 00 |

Ordered arrays:

- `lev_x[10]`: `10 24 30 36 3E 49 5A 75 94 A4 AC BA DA E2 EA F0`
- `lev_y[10]`: `C0 A6 8A 6A 4A 2F 14 24 20 39 59 75 72 90 B0 D0`
- `lev_angle[10]`: `0E 0D 0C 0D 0D 0D 01 0F 02 03 03 00 03 03 03 00`

The arrays are at chip offsets `$021C-$022B` (X), `$031C-$032B` (Y), and `$041C-$042B` (angle). `lev_open[10] = $FF`, which the disassembly defines as open topology.

Directly adjacent row-specific metadata, located by the same table-base/stride method:

- `lev_scale[10] = $10`
- `lev_y3d[10] = $90`
- `lev_y2d[10] = $40`, `lev_y2db[10] = $00` (signed value `$0040`)
- `lev_fscale[10] = $50`, `lev_fscale2[10] = $04` (combined value `$0250`)

Cross-checking the recorded known row 7 metadata yielded `lev_remap=$07`, scale `$18`, camera height `$B0`, Y placement bytes `$60/$00`, and open flag `$FF`, matching the Level 8 values stated in the Level 10 recovery report.

Evidence chain: the ROM signature and image size identify the same local program image used in the historical recovery; table bases and companion-table order come from the disassembly and recovery report; the row-10 bytes were read only at the calculated offsets. This is ROM-backed raw geometry data, not a C# implementation or gameplay acceptance.

## Implications for Levels 12-16

The specific local Rev. 2 image remains readable through the same bounded method. Where later levels require geometry bytes omitted from the textual disassembly, the same single-image, table-offset workflow is available. This check does not recover or validate any Level 12-16 geometry.

No project source, tests, configuration, reference files, or durable behavior documentation were changed. No game launch or manual gameplay acceptance was performed or claimed.
