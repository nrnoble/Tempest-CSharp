# Blaster-Death ROM Geometry Implementation Report

Date: 2026-11-22

## Scope and completion

Implemented the bounded final Blaster-death geometry replacement.
The completion token for the task is
`TEMPEST_BLASTER_DEATH_ROM_GEOMETRY_IMPLEMENTATION`.

No protected ROM, screenshot, video, `TEMP/`, `UserData/`, or other
visual/reference assets were inspected or changed.

## Changed files

- `src/Tempest.Game/PlayerDeathPresentation.cs`
  - Added immutable `BlasterDeathSegment` records.
  - Added the exact 24-segment ROM-derived table in original draw order.
  - Added centralized `13/17` coordinate scale and bounds-center transform.
  - Added centralized project-local Atari-code RGB translation.
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
  - Removed the three legacy 16-edge Blaster-death outlines and obsolete
    death-geometry constants.
  - `DrawPlayerExplosion` now submits only the 24 transformed ROM segments.
  - Preserved the existing accepted explosion center and presentation path.
- `tests/Tempest.Game.Tests/PlayerDeathPresentationTests.cs`
  - Added exact endpoint, endpoint-order, color-code, bounds, and topology tests.
  - Added coordinate-scale, Y-axis, color translation, held-stage, and 24-line
    submission coverage.

## Evidence and translation classification

- ROM segment geometry: ROM-BACKED where supported by the tracked disassembly,
  using only the implementation values supplied in the final geometry handoff.
- RGB code 9/10/11 mapping: PROJECT TRANSLATION.
- The 24-segment local geometry is shared across legacy and Atari presentation;
  those modes retain only their established projection paths.
- The implementation does not change stage timing, 60-tick lifecycle, death
  state, re-entry, collision, enemy reconstruction, or gameplay behavior.

## Validation

- PASS: targeted `PlayerDeathPresentationTests` — 12 passed.
- PASS: complete `TempestCSharp.slnx` test suite — 1819 passed, 0 failed,
  0 skipped.
- PASS: `dotnet build TempestCSharp.slnx -c Release --no-restore`
  — 0 warnings, 0 errors.
- PASS: `git diff --check`.
- The interactive game was not launched.

## Manual handoff

Legacy presentation:

```powershell
dotnet run --project src\Tempest.Game -- --level 1 --lives 10 --legacy-presentation
```

Atari presentation:

```powershell
dotnet run --project src\Tempest.Game -- --level 1 --lives 10 --atari-presentation
```

Existing suitable playback recording:

```powershell
dotnet run --project src\Tempest.Game -- --playback-file <recording-file>
```

Use the existing paused playback controls for reverse stepping if the
selected recording contains a verified Blaster-death event.

Neal should judge whether the effect is materially more like the arcade
picture, especially topology, multicolor sectors, stepped expansion/hold/
contraction, maximum size, center stability, disappearance, and equality
between legacy and Atari rendering.

## Remaining boundary

No durable behavior document was changed. No files were staged, committed,
or pushed. Pre-existing unrelated work and historical records were preserved.
