# Player-Death Enemy Retraction Implementation Report

## Starting checkpoint

- `d07e6d6 Finalize accepted Blaster-lane highlighting`
- The pre-existing untracked `TEMP/` directory was left untouched.

## Implementation

- Added a game-owned `PlayerDeathPresentation` timeline tied to the existing
  60-update `PlayerDying` interval. Its deterministic progress is read-only to
  the renderer and resets on ordinary re-entry or restart.
- The renderer applies one shared projection-depth offset, increasing by one
  tube depth over the presentation. Flippers, Tankers, Spikers, Fuseballs and
  Pulsars use it; logical enemy height, lane, state, timers, identities and
  accounting are not changed. The supported retreat depth is clamped to keep
  projection finite and ordered.
- Existing death-state simulation freezing remains in place. If a collision
  starts `PlayerDying`, the rest of that active tick now stops before additional
  collision checks, all-rim completion or Hurry-Up processing. An already
  latched Hurry-Up state is preserved.
- Spikes continue through their existing fixed web rendering path and persist
  across ordinary life reconstruction. Eggs, shots, enemy bullets, Pulsar
  hazards and Super Zapper behavior retain their existing C# presentation and
  lifecycle policies.
- The accepted Blaster-lane highlight remains unchanged and visible during
  `PlayerDying`. The final Atari Blaster-location animation was not implemented.

## Evidence and timing

- **ARCADE VERIFIED:** Neal's observation establishes inactive enemies
  retracting backward/down the tube, fixed/persistent Spikes, and a complete
  retraction taking no more than approximately two seconds. The timing is a
  qualitative upper bound, not an exact measurement, frame count or curve.
- **REFERENCE VERIFIED:** Incitti's shared projection/depth shift is an
  engineering clue; the inspected source does not contain a dedicated
  enemy-specific retreat routine.
- **PROJECT DECISION:** linear render-only displacement over the existing
  60-tick lifecycle (one second at the fixed 60 Hz simulation rate). Manual
  comparison accepted the visible result and pacing within Neal's remembered
  approximate two-second maximum. No Atari-exact duration or curve is claimed;
  future footage/manual comparison may still inform tuning.
- **PROJECT DECISION / UNCERTAINTY:** Egg visibility and other unobserved
  death-presentation details retain their established C# policy pending
  footage. The Blaster-location animation remains pending footage for geometry
  and timing.

## Identity, gameplay and lifecycle protection

- No identity ownership, resolved/unresolved outcome, score, life-loss,
  reconstruction, Tanker cargo, Super Zapper, level progression or completion
  implementation was redesigned.
- Existing Flipper inactivity and Tanker/cargo identity behavior remained
  intact. Level 3 mixed-wave rendering was exercised with a Tanker; the
  existing Level 3 ledger/cargo regression tests remain unchanged and pass.
- Ordinary death still consumes exactly one life and reconstructs only
  unresolved units at the existing boundary. Final-life death reaches Game
  Over without reconstruction; restart resets presentation progress.
- Spikes are excluded from the enemy offset. The Spiker and its persistent
  Spike were checked independently during ordinary player death.

## Tests and validation

### Automated verified

- Deterministic presentation progression, render-only retreat, and frozen
  logical enemy height/lane/state.
- Identity/accounting conservation, Tanker/cargo conservation, fixed-Spike
  behavior, death/reconstruction, Game Over/restart, and protected rendering
  and gameplay systems.
- Focused `PlayerDeathPresentationTests` rerun after manual acceptance:
  **5 passed**. The pre-manual focused death-presentation gate recorded
  **8/8 passed**.
- Broader regression group (death/restart, Simulation, Level 3 mixed wave and
  ledger, Tanker/cargo, Super Zapper, lane highlight, approach, progression,
  and board/test-mode coverage): **476 passed**.
- `dotnet test TempestCSharp.slnx --no-restore`: **554 passed, 0 failed**.
- `dotnet build TempestCSharp.slnx --no-restore`: **succeeded, 0 warnings,
  0 errors**.
- `git diff --check`: **passed**.

### Manually accepted by Neal

The normal campaign was run with:

```powershell
dotnet run --project src/Tempest.Game -- --lives 10
```

The Spiker scenario was run with:

```powershell
dotnet run --project src/Tempest.Game -- --spiker-test --lives 10
```

- **Level 1:** enemies visibly retreated backward/down the tube instead of
  vanishing immediately; direction matched Neal's Atari observation; no
  sideways/lane movement; retracting enemies appeared inactive; no firing,
  score, or resolution anomaly; pacing stayed within the remembered
  approximate two-second maximum; reconstruction was normal and exactly one
  life was lost.
- **Level 2:** square-board retreat direction, geometry and smoothness were
  accepted; reconstruction remained normal.
- **Level 3:** active mixed-wave enemies retracted; Tanker remained inactive;
  next-life mixed-wave reconstruction looked normal, with no obvious missing
  or duplicated enemies/cargo and sensible score/lives.
- **Spiker/Spike:** active enemies retracted; Spike remained fixed on the web,
  did not retract or disappear on Blaster death, and behaved normally on the
  next life.
- **Game Over:** final-life retraction occurred; Game Over and restart worked.

These are manual visible-behavior results, not evidence that the C# one-second
duration or linear curve is arcade-exact. The approximate two-second figure
remains Neal's qualitative upper-bound observation.

## Changed files

- `src/Tempest.Game/PlayerDeathPresentation.cs`
- `src/Tempest.Game/TempestGame.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `tests/Tempest.Game.Tests/PlayerDeathPresentationTests.cs`
- `tests/Tempest.Game.Tests/SimulationTests.cs`
- `README.md`
- `TEMPEST-BEHAVIOR-SPEC.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `AI-Logs/Prompts/2026-11-01-player-death-enemy-retraction-implementation-prompt.md`
- `AI-Logs/Reports/2026-11-01-player-death-enemy-retraction-implementation-report.md`

`AI-Logs` remains ignored. `Reference/incitti` was not modified. No unrelated
existing gameplay defect was exposed.

## Final status

The player-death enemy-retraction milestone is complete at its
arcade-verified/manual acceptance boundary. The separate Atari Blaster-location
death animation remains deferred until footage establishes its geometry and
timing; it is not a blocker for this accepted retraction milestone.

No commit or push was made.
