# Blaster Death Stepped Animation Implementation Report

Date: 2026-11-22

## Scope

Implemented the tightly bounded Blaster-death presentation refinement authorized
by `TEMPEST_BLASTER_DEATH_STEPPED_ANIMATION`.

The implementation uses the verified enemy-shot stage order and relative holds
as a project translation over the existing 60-tick C# death lifecycle. It does
not change gameplay or add cause-specific behavior.

## Evidence

- **ROM/DISASSEMBLY BACKED:** stage order 1,2,3,4,5,6,4,2,1; holds
  2,2,2,2,2,4,3,2,1; shared multicolor fragment picture.
- **PROJECT TRANSLATION:** 20 relative holds mapped across 60 C# ticks;
  normalized stage scales anchored to the accepted 150% peak size; existing
  three-layer starburst geometry used without screenshot tracing.

## Implementation

Changed:

- `src/Tempest.Game/PlayerDeathPresentation.cs`
  - Added centralized `BlasterDeathStage` and `BlasterDeathStageSchedule`.
  - Added nine half-open elapsed-age ranges totaling exactly 60 ticks.
  - Added deterministic countdown-to-elapsed mapping.
  - Added centralized monotonic normalized scales for sizes 1 through 6.
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
  - Removed the continuously interpolated death radius envelope.
  - Selected a constant scale from the death countdown at each active tick.
  - Preserved the existing center, three layers, base scales, rotations,
    asymmetric 16-point radii, topology, and accepted 150% peak radius.
  - Preserved clean disappearance for `DeathTicks <= 0`.
- `tests/Tempest.Game.Tests/PlayerDeathPresentationTests.cs`
  - Added stage order, hold, duration, gap, overlap, boundary, scale,
    contraction, and final-remnant schedule assertions.

## Validation

1. `dotnet test ... --filter "FullyQualifiedName~PlayerDeathPresentationTests"`:
   **PASS**, 10 tests.
2. Focused death, Blaster, Blaster-scale, and Fuseball filtering:
   **PASS**, 107 tests.
3. Complete solution test suite:
   **PASS**, 1816 tests.
4. Release solution build:
   **PASS**, 0 warnings, 0 errors.
5. `git diff --check`:
   **PASS**.

## Preserved Boundaries

No gameplay, death detection, lives, re-entry, collision, player lane,
enemy retraction, score, Super Zapper, reverse-zoom, recording, or
deterministic simulation code was changed.

The legacy and Atari renderers continue to submit the same local stage
geometry; only their established projection path differs.

## Git State

No files were staged, committed, or pushed. Pre-existing tracked and
untracked work was preserved. The implementation files and ignored AI-Logs
prompt/report are the only task records or source changes created here.

## Manual Acceptance Handoff

Observe a normal Blaster death with the legacy presentation:

```powershell
dotnet run --project src\Tempest.Game -- --legacy-presentation --lives 10
```

Observe it with the Atari presentation:

```powershell
dotnet run --project src\Tempest.Game -- --atari-presentation --lives 10
```

Existing file playback may use the established recording path:

```powershell
dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\level5.json --speed 0.5
```

Pause with Space and use Right/Left for forward/reverse stepping where a
suitable death event exists.

Neal should judge compact initial breakup, deliberate held frames, stepped
expansion and contraction, held broad peak, compact final remnant, clean
disappearance, existing center/colors/overall size, and unchanged gameplay
re-entry behavior.
