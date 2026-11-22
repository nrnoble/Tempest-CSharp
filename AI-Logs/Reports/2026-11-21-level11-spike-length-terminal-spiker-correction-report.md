# Level 11 Spike-length and terminal-Spiker correction report

Date: 2026-11-21

## Scope completed

Implemented the two bounded Level 11 corrections specified by the read-only handoff:

1. Added a derived `Level11StartingSpikeHeight` configuration value using the established
   Level 8/9 affine convention:
   `(0xF0 - 0xB0) * Level.Depth / (0xF0 - 0x10)`.
   Genuine Level 11 now receives approximately `114.285714` C# world-height units.
2. Added Level 11-specific terminal Spiker handling in `UpdateSpikers`:
   - pending generic supply above zero restarts the existing Spiker lifecycle;
   - pending generic supply equal to zero converts the same Spiker original in place
     through `ConvertActiveSpikerToTanker`;
   - the existing Tanker construction and ledger cargo ownership retain the original
     identity and two latent cargo identities.

The Level 15 direct-start candidate was not changed. Existing presentation calibration
remains CLI-only; no defaults, geometry, HUD, viewport, Approach, ExitZoom, or winding
behavior changed.

## Evidence and translation

- `$B0` remains preserved as ROM-backed raw tip coordinate evidence in
  `Level11WaveSelection.StartingSpikeHeightRaw`.
- The affine conversion and 224-unit `$10..$F0` C# gameplay translation are
  PROJECT TRANSLATION behavior.
- Terminal conversion uses the existing ledger conversion architecture and the
  `PendingGenericOriginalCount == 0` evidence-equivalent Level 11 condition.

## Changed files

- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/TempestGame.cs`
- `tests/Tempest.Game.Tests/Level11ProductionCorrectionTests.cs`
- `AI-Logs/Prompts/2026-11-21-level11-spike-length-terminal-spiker-correction-prompt.md`

## Automated validation

- Focused Level 11 correction tests: PASS, 4/4.
- Relevant Level 11, Level 8/9, direct-start, Spiker/Tanker/cargo, and ledger suite:
  PASS, 116/116.
- Complete solution test suite: PASS, 1799/1799.
- Release solution build: PASS, 0 warnings and 0 errors.
- `git diff --check`: PASS.

## Manual acceptance remaining

Neal should verify gameplay with:

```text
dotnet run --project src\Tempest.Game -- --level 11
```

For continued visual/gameplay observation:

```text
dotnet run --project src\Tempest.Game -- --atari-presentation --level 11 --tube-y-gain 1.5 --tube-scale 1.1
```

Required observations:

- Initial Level 11 Spikes now have the corrected shorter starting length.
- Spike collision and routing remain normal.
- Level 11 Spikers continue cycling while generic pending originals remain.
- Once pending generic originals reach zero, a terminal Spiker converts in place to
  a Tanker and releases cargo normally.
- All-rim completion remains blocked until the converted Tanker/cargo obligations are
  resolved.

No interactive game was launched. No source behavior outside the two bounded
corrections was intentionally changed. No staging, commit, push, reset, restore,
stash, clean, or rebase was performed.
