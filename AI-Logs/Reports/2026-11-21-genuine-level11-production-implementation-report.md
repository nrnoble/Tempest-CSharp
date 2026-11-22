# Genuine Level 11 Production Implementation

Date: 2026-11-21

## Outcome

**IMPLEMENTED; AUTOMATED VALIDATION PASS.**
Displayed Level 11 now starts as a production level using genuine ROM-backed
shape row 8, while displayed Level 15 remains a direct-only development
candidate using preserved shape row 10.

## Implementation

- Added `LevelConfiguration.Level11Production` with row-8 geometry, 15 playable
  lanes, HUD identity 11, enemy supply 27, effective gameplay parameter level
  11, `$24` repeat Flipper routing, `$07`/`$24` Tanker cargo routing, and
  the recovered spike parameter.
- Added `LevelLoader.LoadLevel11OpenTube()` using the 16 ordered ROM row-8 X,
  Y, and angle evidence.
- Added row-8 to `AtariTubeRecordCatalog`.
- Added neutral Atari presentation calibration `(1, 1, 0)` for Level 11.
- Advanced production progression from Level 9 directly to Level 11 and made
  Level 11 the terminal production level until Level 12 is implemented.
- Preserved direct `--level 15`; Level 15 no longer receives genuine Level 11
  production identity. Its separate development-only parameter baseline remains
  explicit and unaccepted.
- Preserved direct Fuseball, Tanker-cargo, identity reconstruction, and runtime
  representation correction through the existing generic and Level-11-specific
  paths.
- Added a neutral fallback winding sign for the genuine row-8 open outline,
  which is collinear in the ROM-backed point data. The geometry itself is not
  altered or screenshot-fitted.

## Validation

- Focused Level 11/15/progression/HUD/presentation batch: PASS, 94 tests.
- Complete `dotnet test TempestCSharp.slnx`: PASS, 1,792/1,792.
- Release solution build: PASS, 0 warnings, 0 errors.
- `git diff --check`: PASS. Existing LF-to-CRLF working-copy warnings were
  observed for `Fuseball.cs` and `LevelLoader.cs`; no whitespace errors.
- No ROM read or interactive game launch occurred.
- No staging, commit, push, reset, restore, stash, clean, or rebase occurred.

## Manual Handoff

```powershell
dotnet run --project src\Tempest.Game -- --level 11
dotnet run --project src\Tempest.Game -- --atari-presentation --level 11
dotnet run --project src\Tempest.Game -- --atari-presentation --level 11 --calibration-guides
dotnet run --project src\Tempest.Game -- --atari-presentation --level 11 --tube-y-gain 1 --tube-scale 1 --tube-vertical-offset 0
dotnet run --project src\Tempest.Game -- --atari-presentation --level 11 --lives 9
dotnet run --project src\Tempest.Game -- --atari-presentation --level 11 --fuseball-test
```

Neal should manually judge genuine Level 11 tube identity, open-tube
playability, enemy behavior, Fuseballs, ordinary Flippers, Tanker cargo,
Spikers and Spikes, firing, Super Zapper, death/re-entry, HUD, and first-pass
Atari presentation. Level 11 presentation is not automatically tuned.

Completion token:
`TEMPEST_GENUINE_LEVEL11_PRODUCTION_IMPLEMENTATION`
