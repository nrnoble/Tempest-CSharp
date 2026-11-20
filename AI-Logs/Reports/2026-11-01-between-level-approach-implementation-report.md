# Between-level approach implementation report

## Checkpoint and scope

- Starting checkpoint: `df0dc19 Complete natural Level 3 progression`.
- Implemented one shared natural transition approach for Level 1 -> Level 2 and
  Level 2 -> Level 3.
- Level 3 remains terminal. No Level 4, campaign cycle, or commit/push was
  added.
- The complete received prompt is saved at
  `AI-Logs/Prompts/2026-11-01-between-level-approach-implementation-prompt.md`.
- The existing untracked `TEMP/` directory was left untouched.

## Implementation

- A new `LevelState.Approach` and small `LevelApproach` model own transition
  progress and depth offset. The existing successful natural exit commit and
  `LevelProgression` resolver remain authoritative.
- The incoming production configuration and its fresh wave/Level 3 ledger are
  installed at approach start. Score and lives carry through as before.
- Approach advances only player rim movement. Egg release/update/hatching,
  enemy and bullet updates, shots, Super Zapper, collisions, death, Hurry-Up,
  all-rim completion, and normal completion checks remain frozen. Fire and
  Super Zapper input are ignored.
- Spinner and keyboard movement retain the existing movement path. The source
  did not have controller movement, so the existing input path now also maps
  the left gamepad stick using a 0.15 normalized dead zone and the shared
  keyboard movement speed. Spinner sensitivity/direction are unchanged.
- Arrival occurs once after 120 simulation updates. That update changes to
  `Playing` and returns; ordinary simulation starts on the next step. The
  player's selected rim position persists.
- Rendering adds a transition-only 240-unit depth offset to the incoming tube
  and Blaster, eased to zero with a deterministic smoothstep curve. Normal
  `Projection.K`, `Projection.ZOffset`, geometry and gameplay projection are
  unchanged. Wave HUD details and incoming gameplay objects are hidden during
  approach.
- Duration, initial offset, curve and controller dead zone are provisional
  **PROJECT DECISIONS**, not arcade measurements. Arcade evidence remains
  limited to Neal's qualitative observation of a distant growing tube, a
  movable Blaster, and engagement beginning after arrival.
- **Automated verified:** state transition, gameplay freeze, progression
  ownership, approach-model endpoints, projection finiteness, movement rules,
  fire/Zapper suppression, arrival behavior, development-mode regression, and
  terminal Level 3.
- **Manually accepted:** Neal observed no new jerk or discontinuity at the
  ExitZoom handoff, projection flash, momentary wrong board, strange Blaster
  jump, or other new problem. He found the approach impressive and close to
  remembered Atari behavior. This is qualitative acceptance of the current
  implementation, not a measured fidelity percentage.
- The Atari approach's elevated viewing perspective (approximately 30-45
  degrees by Neal's observation) remains a deferred fidelity refinement;
  Incitti's approach is sharper and is an engineering reference only.
- Do not characterize the provisional duration, initial depth offset,
  smoothstep curve, controller dead zone or exact projection trajectory as
  **ARCADE VERIFIED**.
- No C3/C4 defect was exposed. No Level 3 composition/ledger, Tanker/cargo,
  Zapper gameplay, death/reconstruction, completion, geometry or spinner
  tuning was redesigned. `Reference/incitti` is unchanged.
- No audio was implemented; audio/sound fidelity remains a long-term roadmap
  item.

## Changed files

- `README.md`
- `TEMPEST-BEHAVIOR-SPEC.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `src/Tempest.Game/Levels/LevelApproach.cs`
- `src/Tempest.Game/Levels/LevelState.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `src/Tempest.Game/Systems/InputSystem.cs`
- `src/Tempest.Game/TempestGame.cs`
- `tests/Tempest.Game.Tests/LevelApproachTests.cs`
- `tests/Tempest.Game.Tests/LevelConfigurationTests.cs`
- `tests/Tempest.Game.Tests/LevelProgressionTests.cs`
- `tests/Tempest.Game.Tests/SimulationTests.cs`

## Validation

- Approach/progression focused group: **28 passed, 0 failed, 0 skipped**.
- Broader progression/configuration/Level 2/Level 3/Zapper/input/simulation
  group (including input diagnostics): **521 passed, 0 failed, 0 skipped**.
- Full solution `dotnet test TempestCSharp.slnx --no-restore`: **536 passed,
  0 failed, 0 skipped**.
- Full solution `dotnet build TempestCSharp.slnx --no-restore`: **0 warnings,
  0 errors**.
- `git diff --check`: passed.
- `Reference/incitti`: unchanged. No Level 4/cycle or audio implementation.
- No commit or push.

## Manual acceptance

Neal manually tested both natural transitions, Level 1 -> Level 2 and
Level 2 -> Level 3, in normal campaign play and accepted the current
between-level approach presentation. He observed:

- no new jerk or discontinuity at the end of ExitZoom;
- no projection flash or momentary wrong board;
- no strange Blaster jump;
- no newly observed problems.

His assessment was that the approach remains impressive and that the current
game plays very close to the arcade game. His approximate 85% overall
recreation estimate is a qualitative assessment, not a measured metric and not
an approach-specific score.
