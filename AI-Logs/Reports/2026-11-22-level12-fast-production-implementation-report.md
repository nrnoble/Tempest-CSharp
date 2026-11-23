# Level 12 Fast Production Implementation Report

## Scope

Implemented production displayed Level 12 using Rev. 2 shape row 12, preserving the existing enemy ledger, lifecycle, presentation, HUD, playback, and progression architecture.

Completion token:

TEMPEST_LEVEL12_FAST_PRODUCTION_IMPLEMENTATION

## Evidence

- ROM-BACKED / DISASSEMBLY-BACKED: Level 12 shape row 12 geometry, presentation metadata, and parameter-row values.
- PROJECT TRANSLATION: C# 25/14 raw-coordinate centering, Y reflection, 47-tick holdoff approximation, starting-Spike height conversion, and neutral Atari calibration.
- The mapping lookup index is 11 while the selected shape row is 12.
- Row 12 ordered X: DA A4 87 80 79 5C 26 10 10 20 48 80 B8 E0 F0 F0.
- Row 12 ordered Y: E4 E8 B7 80 B7 E8 E4 B2 7A 47 20 10 20 47 7A B2.
- Row 12 angles: 08 0B 0C 04 05 08 0B 0C 0D 0E 0F 01 02 03 04 05.
- Presentation: scale 18, camera height 20, signed Y offset -352, closed topology, flipper scale 0400.
- Gameplay: enemy supply 24 ordinary / 27 hard, max 6 active units and 7 physical slots, 1-1 Tanker, 1-2 Spiker, 0-5 Flipper, 2-2 Pulsar, 1-1 Fuseball, raw A8 starting Spike, Flipper program 19, holdoff 47, max 2 enemy shots.

## Changes

- Added Level 12 ROM evidence and closed shape-row production loader.
- Added Level 12 presentation metadata and Atari tube record 12.
- Added neutral first-pass calibration and model-frame mapping.
- Added Level 12 wave policy and integrated generic production admission.
- Extended natural progression from Level 11 to Level 12 and terminal boundary at Level 12.
- Added direct startup, recording, spinner, presentation, and lifecycle coverage.
- Preserved direct-only Level 15 and Levels 1-11.

## Validation

- Targeted Level 12/direct/presentation/progression subset passed.
- Complete solution tests: PASS, 1819/1819.
- Release solution build: PASS, 0 warnings and 0 errors.
- `git diff --check`: PASS; one CRLF/LF working-copy normalization notice was reported for LevelLoader.
- No staged changes were created.

## Manual Handoff

- Legacy Level 12:
  `dotnet run --project src\Tempest.Game -- --level 12`
- Atari Level 12:
  `dotnet run --project src\Tempest.Game -- --level 12 --atari-presentation`
- Atari with guides:
  `dotnet run --project src\Tempest.Game -- --level 12 --atari-presentation --calibration-guides`
- Tube controls:
  `dotnet run --project src\Tempest.Game -- --level 12 --atari-presentation --tube-y-gain 1 --tube-scale 1 --tube-vertical-offset 0`
- High-lives run:
  `dotnet run --project src\Tempest.Game -- --level 12 --lives 10`

Manual acceptance remains required for tube identity, spinner direction, enemy mix, Flipper program 19, Tanker/Spiker/Fuseball/Pulsar behavior, Spike start, HUD, and first-pass presentation.
