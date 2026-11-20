# ROM-backed presentation metadata foundation

## Status

**COMPLETE — inert metadata foundation implemented and automated validation passed.**

No presentation, simulation, gameplay, control, timing, recording, playback, or
rendering behavior was changed. No visual or gameplay improvement is claimed.

## Starting Git state

- Branch: `master`
- HEAD and `origin/master`: `385f603a94391756b2ca6cccd524dc17b281e782`
- Upstream: `origin/master`
- Index: clean
- Tracked worktree: the 14 authorized portrait/perspective and pause/screenshot
  candidate files; no unrelated tracked changes
- Non-ignored untracked files: none
- Starting tracked diff: 14 files, 2,006 insertions, 120 deletions

The candidate was preserved. No existing files were restored, staged, committed,
or pushed.

## Sources inspected

- `AI-Logs/Reports/2026-11-14-rev2-presentation-table-extraction-report.md`
- `AI-Logs/Reports/2026-11-14-rev2-presentation-mathematical-translation-report.md`
- `AI-Logs/Reports/2026-11-14-live-pause-screenshot-implementation-report.md`
- `src/Tempest.Game/Levels/LevelDefinition.cs`
- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/Levels/LevelProgression.cs`
- `src/Tempest.Game/Levels/LevelLoader.cs`
- Related Level 9 and level-configuration tests

The task prompt was preserved verbatim at
`AI-Logs/Prompts/2026-11-14-rom-presentation-metadata-foundation-prompt.md`
and verified against the read-only attachment by SHA-256.

No raw ROM files or directories, `TEMP/myNotes.md`, screenshots, images, videos,
MAME frames, Gemini material, Incitti source, or web sources were inspected.

## Metadata design and ownership

Added `RomPresentationMetadata`, a sealed immutable class with get-only
properties. The raw low/high bytes are authoritative; the combined unsigned
value uses explicit little-endian assembly, and the signed value uses unchecked
16-bit two's-complement interpretation.

`RomPresentationMetadataCatalog` owns one read-only metadata instance for each
displayed production Level 1–9. Lookup is by displayed level, reflecting the
ROM's `lev_remap` selection, and unsupported levels throw rather than fall back.
No Level 10–16 gameplay or metadata entries were added.

`LevelLoader.Level9RomEvidence` now forwards its existing shape-remap,
scale, camera-height, Y2D byte/value, and open evidence accessors to the same
Level 9 catalog instance. Its other evidence, including flipper scale and tube
color, remains unchanged. This avoids separate authoritative Level 9 copies
while preserving the existing Level 9 evidence assertions.

No `LevelDefinition`, `LevelConfiguration`, projection, renderer, or gameplay
ownership changes were needed.

## Preserved Levels 1–9 values

Displayed level maps to the following zero-based tube-table index:
`0, 1, 2, 3, 4, 5, 6, 7, 13`.

| Level | Tube index | Scale raw | Camera-height raw | Y2D low | Y2D high | Combined raw | Signed Y2D | Open raw | Interpretation |
|---:|---:|---:|---:|---:|---:|---:|---:|---:|---|
| 1 | 0 | `0x18` | `0x50` | `0x40` | `0xFF` | `0xFF40` | -192 | `0x00` | closed |
| 2 | 1 | `0x1C` | `0x50` | `0x20` | `0xFF` | `0xFF20` | -224 | `0x00` | closed |
| 3 | 2 | `0x18` | `0x50` | `0x40` | `0xFF` | `0xFF40` | -192 | `0x00` | closed |
| 4 | 3 | `0x0F` | `0x68` | `0x80` | `0xFF` | `0xFF80` | -128 | `0x00` | closed |
| 5 | 4 | `0x18` | `0x50` | `0x40` | `0xFF` | `0xFF40` | -192 | `0x00` | closed |
| 6 | 5 | `0x18` | `0x50` | `0x40` | `0xFF` | `0xFF40` | -192 | `0x00` | closed |
| 7 | 6 | `0x18` | `0x68` | `0x70` | `0xFF` | `0xFF70` | -144 | `0x00` | closed |
| 8 | 7 | `0x18` | `0xB0` | `0x60` | `0x00` | `0x0060` | +96 | `0xFF` | open |
| 9 | 13 | `0x0C` | `0xB0` | `0x40` | `0x01` | `0x0140` | +320 | `0xFF` | open |

For each row, combined raw Y2D is `low | (high << 8)`; casting that 16-bit
pattern to `short` gives the signed value. Tests explicitly cover both negative
(`0xFF40` → -192) and positive (`0x0060` → +96) results.

Construction accepts only `lev_open` encodings `0x00` and `0xFF`; `IsOpen` is
true only for `0xFF`. The metadata remains ROM evidence and does not replace
current gameplay topology.

## Level 9 and topology cross-checks

Level 9 remains cross-verified at tube index 13, scale `0x0C`, camera height
`0xB0`, low/high Y2D `0x40`/`0x01`, combined Y2D `0x0140` / +320, and open
`0xFF`. Its existing `Level9RomEvidence` API resolves to the same metadata
instance; existing Level 9 shape, transform, and evidence assertions remain.

Tests compare `IsOpen` with the current production definition topology for each
displayed level. Levels 1–7 agree as closed and Levels 8–9 agree as open. No
movement, lane-count, drawing, collision, or progression logic reads metadata.

## Inertness and active-consumer check

Source search found no metadata consumer in active projection, rendering,
display configuration, gameplay, transition, recording, or playback code.
The only production-source reference is the Level 9 evidence-forwarding
surface; the catalog is not wired to `LevelDefinition.Points`, `CenterY`,
`YOffset`, `Position`, `Depth`, `K`, `ZOffset`, perspective, playfield
placement, viewport, or any simulation state.

The metadata stores no converted C# units and has no active mathematical
translation. Scale, camera-height, and Y2D placement remain source data pending
mathbox/DVG translation work. Existing gameplay topology remains authoritative.

## Files changed

Implementation and tests:

- `src/Tempest.Game/Levels/RomPresentationMetadata.cs` — immutable value type
  and Levels 1–9 catalog
- `src/Tempest.Game/Levels/LevelLoader.cs` — Level 9 evidence forwards to the
  shared metadata instance
- `tests/Tempest.Game.Tests/RomPresentationMetadataTests.cs` — table values,
  signed derivation, open encoding, unsupported-level lookup, and topology
  agreement
- `tests/Tempest.Game.Tests/Level9ProductionTests.cs` — Level 9 shared-instance
  and signed-value assertions

The existing portrait/perspective and pause/screenshot candidate files were not
modified further. Durable behavior documentation was not changed because this
milestone does not change behavior.

## Validation

The standard Debug output was locked by an already-running `Tempest.Game`
process (PID 95860). It was not stopped. Required validation completed using an
isolated output directory under the session workspace, which built fresh
assemblies.

- Focused metadata, Level 9, and level-configuration tests with VSTest `3m`
  hang timeout: **PASS**, 42 passed, 0 failed
- Full test suite with VSTest `3m` hang timeout: **PASS**, 1,427 passed,
  0 failed
- Release solution build: **PASS**, 0 warnings, 0 errors
- `git diff --check`: **PASS**; Git emitted only LF-to-CRLF working-copy
  advisories for `LevelLoader.cs` and the candidate's `InputSystem.cs`

No interactive game launch was performed or required.

## Final Git state

- Branch: `master`
- HEAD and `origin/master`: `385f603a94391756b2ca6cccd524dc17b281e782`
- Index: clean; nothing staged
- Tracked worktree: 16 modified files, consisting of the 14 pre-existing
  candidate files plus `LevelLoader.cs` and `Level9ProductionTests.cs`
- Non-ignored untracked files: the new metadata source and focused test file
- Nothing was committed or pushed

The runtime result is deliberately **NO VISIBLE OR GAMEPLAY CHANGE**.
