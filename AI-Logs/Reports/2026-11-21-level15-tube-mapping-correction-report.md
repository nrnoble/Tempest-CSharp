# Level 15 tube identity and Level 11 mapping correction

Date: 2026-11-21

## Outcome

**MAPPING CORRECTED; LEVEL 15 CANDIDATE PRESERVED; AUTOMATED VALIDATION PASS.**
The recovered tube is now a direct-start Level 15 candidate, not a Level 11
production level. Level 11 remains unimplemented despite unambiguous recovery
of its genuine geometry evidence. Neal's remaining manual acceptance is
required before any Level 15 gameplay-fidelity or presentation closure.

## Starting state and preservation

- Branch `master`, upstream `origin/master`, HEAD
  `5915096903d78cb43a6edad6c2d2cb071be73fff`; index was empty.
- Continued the existing uncommitted production-level candidate and preserved
  its Fuseball identity/lifecycle correction and runtime representation
  validation.
- Preserved `$24` repeating and `$07` straight Tanker-cargo Flipper factory
  routes, but tested them independently of the direct Level 15 configuration.
- No stage, commit, push, game launch, or ROM modification/copy occurred.
- `Reference/arcade/MBUCOD.V05.txt` was not inspected or changed. The other
  pre-existing untracked prompts/reports and local material were left intact.

## Mapping evidence and correction

The tracked disassembly defines `lev_remap` as encounter order and shows
`get_tube_no` indexing it with zero-based `p1_level`. The routine returns the
remapped shape row, which the geometry setup then uses to index the ordered
point/angle arrays and per-tube metadata. The bounded read of the authorized
single local image read only these regions:

- remap table at file offset `$047C`, 16 bytes:
  `00 01 02 03 04 05 06 07 0D 09 08 0C 0E 0F 0A 0B`;
- genuine shape-row-8 X/Y/angle arrays at offsets `$01FC`, `$02FC`, `$03FC`;
- row-8 scale, camera-height, Y-offset, open, and Flipper-scale bytes at
  offsets `$0494`, `$04A4`, `$04B4`, `$04C4`, `$04D4`, `$04E4`, `$04F4`.

Displayed Levels 11-16 therefore select shape rows `8, 12, 14, 15, 10, 11`.
Displayed Level 11 has lookup index 10 and `lev_remap[10]=$08`. Displayed
Level 15 has lookup index 14 and `lev_remap[14]=$0A`.

The prior C# candidate copied `lev_x[10]`, `lev_y[10]`, and `lev_angle[10]`
(shape row 10), plus row-10 metadata, but labeled them using the remap value
`$08`. The error was a project recovery/translation mistake that conflated
the lookup index with the selected shape row. It was not a ROM level-cycle
exception or a zero/one-based error in the ROM lookup. The copied row-10
geometry is exactly the tube Neal identifies as Level 15.

The gameplay parameter loader separately converts zero-based `curlevel` to
effective parameter level `curlevel+1`. The direct Level 15 candidate
therefore has displayed identity 15, geometry lookup index 14, selected shape
row 10, and an explicit effective-Level-11 gameplay parameter override used
only as an unaccepted development baseline. No Level 11 parameter row is
claimed as Level 15 gameplay evidence.

### Level 11 handoff evidence

Genuine shape row 8 is open (`lev_open=$FF`), with ordered X:
`10 1E 2D 3C 4B 5A 69 78 87 96 A5 B4 C3 D2 E1 F0`; ordered Y is all `$40`;
all 16 angle bytes are `$00`. Metadata: scale `$0A`, camera height `$A0`,
Y offset `$0100`, and Flipper scale `$02D0` (`lev_fscale=$50`,
`lev_fscale2=$05`). The resulting open row has 15 playable lanes. These facts
are recorded as ROM-backed evidence only; no Level 11 production geometry or
gameplay was implemented. Levels 12-14 and 16 have shape-row mapping only.

## Implementation and documentation

- Reclassified the exact existing row-10 geometry/metadata as displayed Level
  15 and enabled direct `--level 15` startup, HUD identity, recording/playback
  identity, and Atari presentation.
- Added the ROM-backed displayed-Level-to-shape-row mapping for Levels 1-16.
- Kept Level 11's genuine row-8 data as evidence only and made direct
  `--level 11` unsupported rather than allowing a Level 15 fallback.
- Kept production progression terminal at Level 9; diagnostic Level 10 is
  unchanged. Level 15 remains direct-start only.
- Marked the reused Level 11 gameplay row as an explicit development baseline,
  not Level 15-specific mechanics. The Level 15 Atari calibration `(1, 1, 0)`
  is an uncalibrated neutral placeholder; existing dimension overrides and
  calibration guides remain available.
- Updated the behavior specification and development notes with the evidence
  distinctions, mapping cause, Level 11 handoff, progression boundary, and
  acceptance status.

## Validation

- Focused mapping/lifecycle/progression/HUD/presentation/recording batch:
  **PASS**, 237 tests.
- Follow-up focused parameter/parser regressions after the full suite exposed
  two issues: **PASS**, 124 tests.
- Complete Release solution suite:
  **PASS**, 1,791/1,791; 0 skipped. The first full run exposed five stale CLI
  error-text assertions and a test fixture whose cloned displayed identity
  needed to resolve its parameter row dynamically. Both causes were corrected
  and the complete rerun passed.
- Release solution build: **PASS**, 0 warnings, 0 errors.
- `git diff --check`: **PASS**. Git emitted existing LF-to-CRLF working-copy
  warnings for `Fuseball.cs` and `LevelLoader.cs`; no whitespace errors.

## Neal manual handoff

The following commands are **not run by Copilot**:

```powershell
dotnet run --project src\Tempest.Game -- --level 15
dotnet run --project src\Tempest.Game -- --atari-presentation --level 15
dotnet run --project src\Tempest.Game -- --atari-presentation --level 15 --calibration-guides
dotnet run --project src\Tempest.Game -- --atari-presentation --level 15 --tube-y-gain 1.1 --tube-scale 0.9 --tube-vertical-offset 12
```

Manual status to retain:

- Tube identity: Neal manually established this geometry as arcade Level 15.
- Gameplay: plays normally and is generally good.
- Detailed Level 15 gameplay fidelity: not formally accepted.
- Presentation fine-tuning: pending as needed; no tuning was done here.
- Level 11 geometry: recovered unambiguously above for the next milestone;
  Level 11 remains unimplemented.

Completion token:
`TEMPEST_RECLASSIFY_LEVEL15_AND_RECONCILE_TUBE_MAPPING`
