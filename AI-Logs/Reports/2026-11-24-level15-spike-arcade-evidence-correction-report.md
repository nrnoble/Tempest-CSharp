# Level 15 starting-Spike evidence and correction

**Status: IMPLEMENTED — AUTOMATED VALIDATION PASS — MANUAL SPIKE ACCEPTANCE PENDING**

## Baseline and repository audit

The existing direct-start Level 15 candidate is the candidate Neal manually
tested; no other build was sought. Neal reports that Level 15 gameplay passes,
with no general gameplay correction requested. The single defect is excessive
starting-Spike length. The accepted presentation is Y gain `1.15`, scale `1`,
vertical offset `75`, and depth-Y gain `1`. Spike-correction acceptance is
pending.

Starting Git state: branch `master`; HEAD
`045aeb240fce91a3eaacf9424da30cd7359927bf`; upstream `origin/master`;
ahead/behind `0/0`; index empty. The only pre-existing tracked worktree change
was `AI-Logs/Reports/2026-11-24-background-opacity-closeout-report.md`. Existing
untracked Level 15 prompt/report records, including the earlier blocked report,
were preserved. Other untracked material was not inspected or changed.

## ROM-to-C# evidence chain

1. The ROM parameter loader at `$92C5-$92D4` converts zero-based `curlevel`
   to the effective 1-based level with `curlevel+1`. The loop at `$92D6-$9326`
   applies the per-level table rows. Pointer `$9657` selects the `wave_spikeht`
   row (`$9607_53`), whose range starts at level 1.
2. Counting the table's per-level values gives Level 11 `$B0`, Level 12 `$A8`,
   Level 13 `$A0`, Level 14 `$A0`, Level 15 `$A0`, and Level 16 `$A8`.
   Thus genuine arcade Level 15's raw starting-Spike value is `$A0`, not the
   candidate's inherited Level 11 `$B0`.
3. `set_enm_and_spikes` at `$9234` reads `wave_spikeht` and copies that raw
   value to the ROM `spike_ht` slots. The value is an along-tube coordinate,
   not a C# height. The level-start code initializes `player_along` to `$10`;
   the established project translation maps the ROM `$10-$F0` span in the
   opposite direction into C# height: `(0xF0 - raw) * Level.Depth / 224`.
4. `Level.Depth` is `400`, with C# height zero at the far end and `Depth` at
   the near rim. Level 15's `$A0` therefore maps to
   `(0xF0 - 0xA0) * 400 / 224 = 142.857...`.
5. Before this correction, `Level15DirectStartCandidate` assigned
   `0xB0 * Level.Depth / 224f`, approximately `314.286`. `$B0` is the Level 11
   table byte, and multiplying it directly treated a ROM coordinate as a
   positive C# height rather than inverting the `$10-$F0` coordinate span.
   The excessive length was therefore a Level 15 configuration/value and
   conversion error. `Level.Reset`, `CreateSpike`, and shared Spike lifecycle
   code were not defective and were not changed.

## Changes and preservation

- Added the Level 15 ROM byte `$A0` and its explicit ROM-coordinate-to-C#
  height translation; the direct-start candidate now uses that value.
- Added a focused Level 15 test for the ROM byte, converted height, rejection
  of the old Level 11 expression, 15 starting Spikes on lanes 0-14, and
  direct-only startup. A second focused test confirms a Spiker continues to
  extend a Level 15 starting Spike through the existing shared lifecycle.
- Set direct Level 15's accepted Atari-presentation defaults to
  `(1.15, 1, 75, 1)` and updated calibration coverage. Geometry, tube
  topology, projection behavior, and unrelated level calibrations are
  unchanged.
- Updated the behavior specification and development notes with the corrected
  baseline, evidence chain, accepted presentation, and pending Spike
  acceptance. Updated comments only where the previous uncalibrated wording
  contradicted the accepted defaults.
- No Level 15 campaign promotion, Level 16 work, enemy-policy change, or
  unrelated gameplay change was made. Existing Level 11 parameter-development
  baseline fields remain unchanged.

Changed project files:

- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/Levels/RomPresentationMetadata.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`
- `src/Tempest.Game/Rendering/AtariPresentationCalibration.cs`
- `tests/Tempest.Game.Tests/Level15TubeCandidateTests.cs`
- `tests/Tempest.Game.Tests/AtariPresentationCalibrationTests.cs`
- `TEMPEST-BEHAVIOR-SPEC.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`

## Automated validation

- **PASS — focused Release regressions:** 150 passed, 0 failed, 0 skipped.
  Covered Level 15 candidate lifecycle/headless simulation, direct startup,
  configuration, Levels 11-14, campaign soak, Spike exit/collision and
  destruction/audio interactions, and presentation calibration. The Level 15
  candidate test advances a seeded simulation for 1,000 ticks.
- **PASS — complete Release solution tests:** 1,961 passed, 0 failed,
  0 skipped; VSTest hang limit `2min`.
- **PASS — Release solution build:** 0 warnings, 0 errors; process exit code 0,
  bounded wait 480 seconds.
- **PASS — `git diff --check`.**

No repository material was staged, committed, pushed, amended, reset,
restored, stashed, cleaned, rebased, or switched. No interactive game was
launched.

## Manual handoff

Neal's manual acceptance of the corrected Spike length remains pending. Run:

```powershell
dotnet run --project src\Tempest.Game -- --atari-presentation --level 15 --tube-y-gain 1.15 --tube-scale 1 --tube-vertical-offset 75 --tube-depth-y-gain 1
```

Judge only whether the starting Spikes have an appropriate arcade-like length;
their count and lane placement; extension and normal interaction; whether Level
15 gameplay still passes; whether the accepted tube presentation is unchanged;
and whether any regression is visible. Do not treat automated validation as
manual gameplay acceptance.
