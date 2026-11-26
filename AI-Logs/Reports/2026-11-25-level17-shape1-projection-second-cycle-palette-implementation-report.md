# Level 17 Shape 1 projection and second-cycle palette implementation

Date: 2026-11-25

Status: **PASS - bounded implementation candidate complete; Neal manual visual acceptance remains pending**

## 1. Executive summary

Level 17 now routes repeated canonical Shape 1 geometry through the same
accepted Shape 1 projection family used by Level 1. The routing key is the ROM
shape identity in presentation metadata, not the displayed level.

The bounded cycle palette resolver adds evidence-supported Level 17 tube,
HUD, Blaster-lane, Flipper, and Pulsar host translations. Gameplay state,
progression, enemy behavior, and Level 18+ content remain unchanged.

## 2. Starting Git/worktree state

- Branch: `master`
- Tracking: `origin/master`
- Ahead/behind: `0/0`
- Existing tracked and untracked candidate/worktree state was preserved.
- No reset, clean, restore, stash, stage, commit, or push was performed.
- Protected recordings, reference assets, screenshots, and videos were not modified.

## 3. Gameplay-freeze status

PASS. No gameplay, enemy supply, Flipper programs, Tanker cargo, Pulsars,
Spikers, firing, shots, Spike gameplay, random behavior, ledger, reconstruction,
completion, movement, or level progression code was changed for this milestone.

## 4. Shape 1 routing correction

`AtariPresentationAdapter.Project` now selects the established accepted depth
projection when `RomPresentationMetadata.TubeTableIndex == 0`. This makes
canonical repeated Shape 1 geometry share the Level 1/Shape 1 projection family
instead of using a `displayedLevel == 17` formula.

## 5. Level 1 regression protection

The existing Level 1 branch is retained and the Level 1 numerical formula,
placement, and Jacobian output are unchanged. A focused theory test compares
representative Level 1 and Level 17 Shape 1 projections.

## 6. Level 17 projection result

Level 17 resolves canonical row 0 and now uses the accepted Shape 1 projection
family. It retains displayed level 17 and all cycle-specific metadata.

## 7. Model-frame/calibration reuse

Level 17 retains the same `(25/14, +1)` model frame and accepted Shape 1
calibration as Level 1: Y gain `1.4`, tube scale `0.8`, vertical offset `35`,
and depth-Y gain `1`. Runtime CLI overrides remain absolute overrides.

## 8. Palette resolver architecture

Added `CyclePaletteResolver` in the rendering layer. It resolves palette 0 for
Levels 1-16 and palette 1 for Levels 17-32, with only bounded Level 17 support
exposed by this milestone.

## 9. Raw cycle blocks

- ROM-BACKED first cycle: `00 04 08 0C C3 07 0B 0B`
- ROM-BACKED second cycle: `00 07 0B 08 44 03 0C 0C`

No raw nibble was promoted into an inferred RGB role beyond the evidence-backed
host-role mappings recorded below.

## 10. Role-evidence matrix

| Role | Evidence | Implemented C# host color | Classification |
|---|---|---|---|
| Tube/web | MAME observation + existing C# red/blue path | Red at palette 1 | ROM/runtime role plus MAME observation plus PROJECT TRANSLATION |
| HUD score/high-score/initials | MAME observation | Cyan | MAME-backed PROJECT TRANSLATION |
| HUD lives | MAME observation | Green | MAME-backed PROJECT TRANSLATION |
| HUD level number | MAME observation | Red | MAME-backed PROJECT TRANSLATION |
| Blaster lane/player | ROM index 1 plus existing C# lane/yellow translation | Yellow unchanged | ROM-backed role, PROJECT TRANSLATION RGB |
| Flipper | ROM index 3 plus MAME magenta observation | Magenta | ROM-backed role plus MAME-backed translation |
| Pulsar normal state | ROM index 4 plus existing cyan translation | Cyan unchanged | ROM-backed role, PROJECT TRANSLATION RGB |
| Pulsar pulse | Established index 0/pulse-white behavior | White transient unchanged | Established project behavior |

## 11. MAME evidence used

Only the four explicitly authorized Level 17 MAME screenshots were specified
for this implementation. The implementation does not derive geometry or
gameplay from them.

## 12. Incitti evidence used

Incitti was not used to add an unsupported role mapping. Existing Incitti-derived
glyph geometry was retained and only supported host colors were changed.

## 13. Tube/web color mapping

Cycle 0 tube/web remains Blue. Cycle 1/Level 17 tube/web uses Red as observed
in the authorized MAME evidence and as an established C# vector color.

## 14. HUD color mappings

Production Atari portrait HUD uses:

- score, high score, and initials: Cyan;
- reserve lives: Green;
- Level 17 number: Red.

Cycle 0 HUD colors remain unchanged.

## 15. Player/Blaster color

ROM index 1 identifies the player. The accepted C# Blaster lane/player host
translation remains Yellow in both cycles; the current C# player glyph remains
unchanged. No evidence justified a cycle-1 RGB change.

## 16. Flipper color

Cycle 0 remains Red. Cycle 1 uses Magenta for the evidence-backed rim/Flipper
host translation.

## 17. Pulsar color/pulse

Normal Pulsar color remains Cyan for both cycles. Pulse-white behavior remains
white and restores the selected base color after transient completion.

## 18. Player-shot/blue-vector identification

The compact blue diamond-like vector is not classified as an enemy. Neal's
observation is recorded as unresolved, likely a Blaster/player shot. Source
evidence was insufficient to justify changing the existing player-shot host
color in this milestone.

## 19. Tanker color

Tanker remains the existing Magenta host translation. It was not changed without
a stronger cycle-specific role/color result.

## 20. Spiker color

Spiker remains the existing Green host translation.

## 21. enemy-shot color

Enemy shots remain existing project translations. No cycle-specific change was
implemented from unsupported exact RGB evidence.

## 22. Spike color

Spikes remain existing Green/white project translations. No spike gameplay or
presentation geometry changed.

## 23. transient-color behavior

Pulsar pulse-white and Super Zapper cycle behavior remain intact. The new
palette resolver changes only persistent base role resolution; existing
transient white/cycle restoration paths remain unmodified.

## 24. Level 16→17 palette activation

Level 16 resolves palette 0 and Level 17 resolves palette 1. Palette activation
is configuration-keyed and independent of geometry/projection routing.

## 25. gameplay-state freeze validation

Production/gameplay code was not changed after the gameplay-freeze checkpoint.
Focused Level 17 projection/palette tests and 2062 full-suite tests passed.

## 26. Level 17 recording result

PASS:

- `dotnet run --project src\Tempest.Game -- --headless-playback TEMP\GameplayRecordings\lvl-17-test.json`
- achieved 2609/2609 ticks;
- `RecordingCompleted`;
- remaining frames 0;
- final Level 17 ExitZoom;
- score 5956;
- lives 6.

The first visible-playback invocation completed the same recording and resumed
live controls; it was stopped immediately and the remaining oracle runs used
the repository headless mode.

## 27. Levels 1-16 recording result

PASS at the established 35350-tick accepted boundary:

- score `134039`;
- lives `9`;
- seed `1990198549`;
- remaining frames `0`;
- `RecordingCompleted`;
- active production candidate then continues at Level 17 Playing.

The active candidate result is consistent with the previously accepted boundary
and does not indicate a gameplay regression.

## 28. focused presentation tests

PASS: 15 Level 17 production tests passed, including:

- Shape 1 projection shared by Level 1 and Level 17;
- Level 17 Shape 1 structural equivalence;
- cycle palette exact raw data;
- dominant tube/HUD/Flipper/Pulsar host colors;
- no Level 17 gameplay-state changes.

## 29. full-suite result

PASS: `2062` passed, `0` failed, `0` skipped.

## 30. Release-build result

PASS:

- 0 warnings;
- 0 errors.

## 31. git diff --check

PASS. Git emitted only a line-ending warning for an existing modified file.

## 32. unresolved color roles

- compact blue diamond-like vector/player shot: unresolved visual role retained
  as the existing project translation;
- exact Tanker, Spiker, enemy-shot, and Spike cycle-1 RGB roles: retained existing
  project translations pending stronger evidence;
- arbitrary minor RGB shades: not invented.

## 33. project translations versus ROM facts

- Shape row 0 reuse: ROM-BACKED and shape-keyed implementation.
- repeated Shape 1 projection: PROJECT TRANSLATION accepted host branch keyed
  by shape identity.
- raw cycle blocks: ROM-BACKED.
- C# host RGB roles: PROJECT TRANSLATION based on existing accepted colors,
  MAME categorical observations, and ROM/runtime role evidence.

## 34. manual visual-acceptance matrix

Neal should check:

- Level 17 tube placement/perspective matches the accepted Level 1 Shape 1 family;
- red tube/web;
- cyan score/high-score text;
- green lives;
- red Level 17 number;
- magenta Flippers;
- unchanged cyan Pulsar and white pulse state;
- unchanged Tanker/Spiker/Spike/project-translated minor roles;
- no perceived gameplay movement/timing regression.

## 35. files changed

- `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`
- `src/Tempest.Game/Rendering/CyclePaletteResolver.cs`
- `src/Tempest.Game/Rendering/AtariHudVector.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `tests/Tempest.Game.Tests/Level17ProductionTests.cs`
- `AI-Logs/Prompts/2026-11-25-level17-shape1-projection-second-cycle-palette-implementation-prompt.md`
- `AI-Logs/Reports/2026-11-25-level17-shape1-projection-second-cycle-palette-implementation-report.md`

## 36. final Git/worktree state

The implementation remains in an unstaged, dirty worktree together with prior
Level 17 and unrelated pre-existing candidate modifications. No files are
staged by this milestone.

## 37. stage/commit/push

None.

## 38. Level 18+

Untouched.

## 39. recommended next step

Stop for Neal's manual visual acceptance using:

```powershell
dotnet run --project src\Tempest.Game -- --start-level 1
dotnet run --project src\Tempest.Game -- --start-level 17 --lives 10
dotnet run --project src\Tempest.Game -- --start-level 17 --lives 10 --background-opacity .5
```

If accepted, document the visual acceptance and promote durable Level 17
presentation/palette facts. If rejected, keep this candidate and use the
specific visual discrepancy as bounded follow-up evidence.
