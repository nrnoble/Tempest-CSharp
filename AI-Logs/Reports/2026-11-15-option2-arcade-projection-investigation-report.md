# Tempest Option 2: Read-Only Arcade Projection Investigation

## Executive conclusion

The local Atari disassembly supports a mixed, data-driven mechanism: the game selects one of 16 tube records containing 16 X/Y points, then the normal playfield draw path sends those points through a shared mathbox-backed coordinate routine using tube-indexed scale and vertical-offset parameters. The resulting coordinates are assembled into vector-list instructions. An open/closed flag also changes the edge traversal.

This establishes a common, depth-related transform applied to tube-specific data, but not a conventional pinhole-camera equation or its exact formula. The disassembly stores the tube and parameter tables as `.chunk` placeholders, so their numeric values and actual tube-to-tube differences cannot be checked here. The ROM evidence does not identify the C# `K/z` formula or `--portrait-perspective` correction as an implementation of the Atari transform.

## Atari mechanism

The source used is `Reference/arcade/Tempest Rom Source code.md`. It is an annotated disassembly whose opening note cautions that parts may be wrong or missing. Names and explanatory comments in it are disassembler annotations; the ROM instructions and their traced data flow are stronger evidence than an annotation's geometric interpretation.

### Data selection and preparation

1. The disassembly lays out `lev_x` and `lev_y` as `x[16][16]` and `y[16][16]`, alongside `lev_angle[16][16]`. The entries are annotated as point coordinates and sector angles. The tables begin at `$b97c`, `$ba7c`, and `$bb7c`. The actual bytes are represented as `.chunk`, not printed values. (`Tempest Rom Source code.md:7461-7472`)
2. `get_tube_no` maps the current level through `lev_remap` and sets `curtube`; its comment describes the remap as the order in which levels are encountered. (`$c2e8-$c30c`; data at `$bc7c`)
3. Level setup at `$c235-$c2e7` uses `curtube` to load per-tube parameters, copies the selected 16 X/Y/angle entries into `tube_x`, `tube_y`, and `tube_angle`, and calculates adjacent-point midpoints. (`Tempest Rom Source code.md:8202-8284`)

### Gameplay coordinate and vector path

The normal drawing path is distinct from the level-selection thumbnail path:

1. `$b367-$b374` calls `$c30d` when the game is producing the relevant vector display list.
2. `$c30d-$c3b9` prepares and emits the playfield geometry. It invokes `$c473` for two input values, `$f0` and `$10` (`$c312-$c333`).
3. `$c473-$c4e0` loops over all 16 `tube_x`/`tube_y` points, calls `$c098` for each, and stores the returned coordinate components in RAM arrays used by the drawing path. The output is clipped to the coordinate range handled by this routine.
4. `$c098-$c16b` writes operands to the mathbox memory-mapped registers, waits for the mathbox, reads its results, and applies the current offset state. The setup path has loaded tube-indexed scale and Y-related values before this routine runs. This is a traced coordinate calculation, not merely a vector processor scale instruction.
5. The playfield drawing routines difference successive coordinates and append long-draw/vector instructions. For example, `$c3ba-$c3ed` forms coordinate differences and calls `$df92`, the long-draw append path.

The use of two `$57` values and per-point mathbox results in the normal draw path strongly supports a longitudinal/depth-related transformation of the tube geometry. It does not expose a complete projection equation: the disassembly does not explain the mathbox's precise arithmetic semantics, and the data tables needed to evaluate concrete outputs are omitted.

The routine at `$c4e1-$c54a` is a separate level-selection thumbnail renderer. It reads `lev_x`/`lev_y` and emits coordinate deltas directly, with vector scale commands; it is not evidence that the gameplay tube bypasses the `$c30d`/`$c473`/`$c098` transform.

## Tube-specific versus common behavior

The table layout separates the point geometry from several per-tube values:

| Table | Disassembly address | Traced role |
|---|---:|---|
| `lev_x`, `lev_y` | `$b97c`, `$ba7c` | 16 point coordinates for each of 16 tube records |
| `lev_angle` | `$bb7c` | Per-point sector angles; copied to `tube_angle` |
| `lev_remap` | `$bc7c` | Maps level order to tube record |
| `lev_scale` | `$bc8c` | Loaded during setup, negated into `$5f`, then used by `$c098` |
| `lev_y3d` | `$bc9c` | Loaded into `y3d`, which `$c098` uses in its coordinate calculation |
| `lev_y2d`, `lev_y2db` | `$bcac`, `$bcbc` | Used to establish the offset state consumed by `$c098` |
| `lev_open` | `$bccc` | Loaded into `open_level`; controls edge traversal |
| `lev_fscale`, `lev_fscale2` | `$bcdc`, `$bcec` | Loaded by enemy drawing code for flipper scaling; not evidence of tube-wireframe projection |

These are separate per-tube arrays, and the setup and mathbox code demonstrate that `lev_scale` and Y-related values enter the common coordinate path. That they affect the resulting projection is a strong inference from their traced use; the exact size and geometric meaning of each effect cannot be recovered from this listing alone. The table comments describe `lev_scale` as reciprocal scale and `lev_y3d` as a 3-D Y/camera-height offset, but those descriptions are disassembler interpretations, not original source comments.

The common behavior is the shared level setup and `$c30d`/`$c473`/`$c098` transformation path. The tube records provide its geometry and per-tube inputs. The disassembly therefore supports neither “only precomputed coordinates” nor “one universal projection coefficient”: it shows table-driven points plus a common transformation with tube-indexed parameters.

## Enclosed versus open tubes

The disassembly annotates `open_level` and `lev_open` as `$00` for closed and `$ff` for open (`Tempest Rom Source code.md:146`). Setup copies `lev_open[curtube]` to `open_level` at `$c25c-$c25f`.

The flag changes traversal counts in the gameplay rendering path: `$c393-$c39b` initializes an index/count to `$0f` and decrements it for an open tube; `$c5e6-$c5ec` performs the same 15-versus-16 adjustment in another rendering routine. This directly establishes that open and closed tubes are processed differently in these paths.

The thumbnail path also changes its indexing: at `$c505-$c51d`, the closed case subtracts `$0f` from the starting point index, while the open case leaves the start at point 15. Both cases then traverse the same number of loop iterations, but begin at different indices. The gameplay routine's explicit 16-versus-15 count is the direct evidence that an open tube omits one traversed segment there. The intended closure behavior is strongly supported by these branches, but because the point-table bytes are absent, this listing alone does not establish the exact endpoint pair or name the omitted gameplay segment.

## C# comparison

The comparison below describes the current worktree code only; it is not used as evidence for Atari behavior.

1. **Is the C# projection conceptually reproducing an identifiable arcade mechanism?**
   Only at a broad level. The C# `Projection.Project` uses a shared depth denominator (`liveDepth / K`) to scale X and Y, while the Atari path demonstrably transforms table coordinates using a common mathbox routine and tube-indexed parameters. The available Atari listing does not identify its arithmetic as the C# `K/z` equation, so equivalence to an identifiable Atari formula is not established.

2. **Does `--portrait-perspective` adjust something with an arcade counterpart?**
   It is a C#-specific presentation correction, not a traced counterpart to one Atari variable. It adds a vertical correction proportional to a normalized reciprocal-depth fraction and `PortraitPerspective`; it does not change the base denominator or projected X. Atari data does contain per-tube scale and Y-related inputs to the shared transform, but none is shown to be this single global correction. `Program.cs` enables the option only for experimental portrait presentation.

3. **Can the arcade evidence plausibly explain why one C# coefficient helps one tube but not others?**
   Yes, as a plausible explanation, not a demonstrated diagnosis. The Atari layout provides separate point sets and separate scale/Y/open values per tube, processed by one common routine. A global C# correction applied across differing point sets and tube-specific geometry can compensate one shape's residual while leaving another's different residual unchanged or worse. The omitted ROM table values prevent quantifying that variation, and this investigation did not analyze images or compare frames.

## Evidence table

| Classification | Atari source location | Finding | Reasoning or trace | Confidence / remaining uncertainty |
|---|---|---|---|---|
| ROM/DISASSEMBLY-BACKED | `Tempest Rom Source code.md:7461-7480`; `$b97c-$bcfb` | Tube records include 16 X/Y points and 16-entry per-tube parameter tables. | The table declarations specify `[16][16]` point arrays and separate 16-entry remap, scale, Y, open, and flipper-scale arrays. `$c2a4-$c2be` copies 16 points and angles. | High for table structure and consumption. The `.chunk` placeholders omit actual values. |
| ROM/DISASSEMBLY-BACKED | `$c2e8-$c30c`, `$c235-$c2e7` | Level selection maps through `lev_remap`; setup loads the mapped record and its parameters. | `get_tube_no` reads `lev_remap`, sets `curtube`; `$c235` indexes parameter tables and copies point data. | High. Level 99 includes a random mapping path; it does not alter the common structure. |
| ROM/DISASSEMBLY-BACKED | `$b367-$b374`, `$c30d-$c3b9`, `$c473-$c4e0`, `$c098-$c16b` | Normal playfield rendering transforms all 16 points using one shared mathbox-backed routine and then builds vector commands. | Main draw calls `$c30d`; `$c473` loops over `tube_x/y` and calls `$c098`; `$c098` writes mathbox operands and reads results; later routines append coordinate deltas. | High for the data/control flow. Exact mathbox equation and units are not shown. |
| STRONG INFERENCE | `$c312-$c333`, `$c473-$c4e0`, `$c098-$c16b` | The shared transform uses longitudinal/depth-related samples, not only precomputed final screen points. | `$c30d` supplies `$f0` and `$10`; `$c473` transforms each table point at each input and outputs coordinates used by vector drawing. | Strong, but the precise depth mapping and visual interpretation of the values remain unknown. |
| ROM/DISASSEMBLY-BACKED; STRONG INFERENCE for geometric meaning | `$c235-$c25f`, `$c098-$c16b`; table comments at `:7465-7469` | Per-tube scale and Y-related values independently feed the common coordinate calculation. | Setup places `lev_scale` in `$5f` and `lev_y3d` in `y3d`; `$c098` consumes those values. Y2D entries establish offset state used by the same routine. | High that inputs are selected by tube index and consumed; moderate for their exact geometric effect, since bytes and mathbox semantics are absent. |
| ROM/DISASSEMBLY-BACKED | `$c25c-$c25f`, `$c393-$c39b`, `$c505-$c51d`, `$c524-$c546` | Open/closed state changes rendering traversal and the thumbnail's starting point index. | `lev_open` is loaded into `open_level`; gameplay render loops reduce 16 iterations to 15 for open tubes. The thumbnail path subtracts `$0f` from the starting index only for closed tubes. | High for traversal and index changes. The exact geometric identity of the omitted gameplay segment is unknown from the listing. |
| ROM/DISASSEMBLY-BACKED | `$b67c-$b684`; `$b634-$b686`; table at `:7479-7480` | The per-tube `fscale` pair is used for flipper rendering, not evidence of a tube-geometry projection coefficient. | Enemy drawing loads the pair after establishing flipper coordinates; the gameplay tube point transform does not load it in `$c235` or `$c473`. | High within the traced use. Other untraced uses are not ruled out, but none establish an effect on tube wireframe geometry. |
| STRONG INFERENCE | Atari locations above; `Projection.cs:64-112`; `VectorRenderer.cs:181-190, 868-893` | C# and Atari share only the broad concept of depth-sensitive coordinate mapping; formula equivalence is not evidenced. | C# divides X/Y by `liveDepth/K` and draws near/far rims from shared world points. Atari uses table values, tube parameters, and mathbox results with no recovered matching equation. | High that no exact correspondence is demonstrated by the available listing; Atari formula remains unknown. |
| ROM/DISASSEMBLY-BACKED for table/use; STRONG INFERENCE for consequence | `$c235-$c25f`, `$c30d-$c4e0`; `Projection.cs:87-112`; `Program.cs:211-213` | One C# portrait adjustment can plausibly fit one tube better than another because Atari evidence shows tube-specific geometry and inputs under a common transform. | The C# correction is global and affects projected Y, while ROM data separates point sets and per-tube scale/Y inputs. | Plausible, not proven for any particular tube or screenshot. No image/frame analysis was performed. |

## Unknowns

- The actual contents of `$b97c-$bcfb` are not present in the disassembly; therefore the numeric differences between tube shapes and per-tube parameters cannot be measured.
- The exact mathbox arithmetic for `$c098` is not documented in this source. In particular, this evidence does not establish a conventional pinhole perspective formula or an equivalent C# denominator.
- The listing shows runtime consumption of table point coordinates but does not establish how those values were originally derived (manual/precomputed shape data, a prior generation process, or another source).
- Without the values and exact mathbox operation, the precise influence and relative contribution of `lev_scale`, `lev_y3d`, and the Y2D values cannot be calculated.
- The listing's traversal logic establishes the open/closed distinction, but it does not fully annotate the topology of each generated gameplay vector segment.
- No Atari evidence identifies `--portrait-perspective` or a directly equivalent single global parameter.

## Recommendation

The justified next evidence step is a separately authorized, bounded, read-only decoding of only the geometry/parameter table range `$b97c-$bcfb`, plus a verified Atari mathbox-operation reference for the register sequence at `$c098`. That would allow numeric cross-tube comparison and a better-supported account of the transform. Do not tune or replace the C# formula based on this investigation alone.

## Scope and completion

- Investigation only. No existing tracked production code, tests, project files, documentation, configuration, or other tracked project content was changed. The sole new file is the permitted ignored report.
- No images, screenshots, frames, unrelated implementations, or Internet sources were used.
- Raw ROM files were not opened or enumerated; no tests/builds were run and the interactive game was not launched.
- No files were staged; nothing was committed or pushed.
- Sole file created: `AI-Logs/Reports/2026-11-15-option2-arcade-projection-investigation-report.md`.
- Most important ROM/disassembly-backed finding: normal playfield rendering feeds per-tube 16-point coordinates through a shared mathbox-backed transform whose inputs include selected per-tube scale and Y-related values.
- Most important remaining unknown: the table bytes and mathbox semantics needed to establish the numeric projection and compare tube-specific effects are absent from the local disassembly listing.
