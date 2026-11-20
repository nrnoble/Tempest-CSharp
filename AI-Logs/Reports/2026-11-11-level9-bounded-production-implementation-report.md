# Level 9 Bounded Production Implementation Report

## Status

**Implementation candidate complete. Automated validation PASS. Manual acceptance PENDING NEAL.**

No interactive game was launched. No files were staged, committed, or pushed. Level 10 was not implemented.

## Starting checkpoint and inspected evidence

- Started on `master`, tracking `origin/master`, at `037b3f9877f38d0182a3eba4fb8a8f0279e4f1a9`; local HEAD and `origin/master` matched and the tracked worktree/index were clean.
- Read the Level 9 status/ROM preimplementation investigation, the Level-specific Flipper/Spike analysis, `TEMPEST-BEHAVIOR-SPEC.md`, and `TEMPEST-DEVELOPMENT-NOTES.md`.
- Inspected accepted Level 7/8 production code, level configuration/progression, the generic wave ledger, relevant gameplay update paths, and their tests.
- The substantive prompt is retained at `AI-Logs/Prompts/2026-11-11-level9-bounded-production-implementation-prompt.md`.
- Protected ROM copies, recordings, `Temp/`, and other user material were not modified.

## Implementation and evidence boundaries

### Geometry, topology, and presentation

- Added Level 9 from shape row 13: preserved 16 raw ordered vertices, angle data, remap `$0D`, and presentation fields; configured an open tube with 15 playable lanes (0–14), no closing edge, and no lane 15.
- Reused the established coordinate normalization and Y-reflection convention. Open-end traversal clamps the player, reverses enemy routes, keeps cargo lanes valid, and omits the closing renderer chord.
- **REV-2-BINARY-CONFIRMED:** retained raw geometry, angle, topology, remap, and presentation evidence.
- **PROJECT TRANSLATION:** C# geometry normalization, Y reflection, camera/placement choices, palette selection, and starting player lane. Raw color/scale fields are not claimed to be literal C# pixels or RGB values. Visual presentation remains pending manual acceptance.

### Configuration and wave policy

- Added canonical Level 9 configuration: displayed level 9, supply 27, four initial Eggs, a six-unit ordinary active-entry gate, Tanker 1–2, Spiker 2–4, Flipper 0–5, and no Pulsars/Fuseballs.
- Each ordinary Tanker consumes one generic unit and reserves two latent Flippers. Cargo reservations count toward the five-Flipper maximum; cargo release bypasses the ordinary six-unit gate.
- Ordinary Flippers are not selected. The deterministic post-minimum selection policy is a **PROJECT APPROXIMATION**, not a claim about exact ROM random-selection behavior.
- Preserved final-pending same-identity Spiker-to-Tanker conversion without adding generic supply or exceeding cargo/type limits.
- Added one distinct starting Spike on each playable lane, at candidate height approximately `85.7143`; this is the specified **PROJECT TRANSLATION**, not literal arcade pixel height.

### Flippers, cargo, firing, and reconstruction

- Added a Level 9-specific `$53` route: move along the current lane, then check its live Spike; equality permits a directed lane change; absent/depleted Spikes permit it; a false predicate continues climbing without a lock or pause. Active pivots continue along movement without destination-lane retesting; the destination is checked after pivot completion and the next along movement. Open-end reversal remains separate.
- **ROM-LISTING-BACKED:** Level 9 route selection and `$53` Spike-sensitive semantics. Level 5 remains isolated.
- **ROM-LISTING-BACKED:** Level 9 Tanker cargo uses `$07` for natural releases and shallow shot releases, and switches to `$53` at/above raw along `$20` (including equality). Tests cover `$1F`, `$20`, and `$21`; children have distinct identities and opposing adjacent routes.
- Added Level 9 holdoff metadata 56, shared enemy projectile accounting, four-shot capacity, eligible Tanker firing, and repeated-cycle Spiker firing. New projectiles advance on a later simulation step. C# timing/speeds remain **PROJECT APPROXIMATIONS**; no global projectile speed or accepted Levels 1–8 firing behavior was intentionally changed.
- Level 9 uses generic ledger reconstruction: unresolved released cargo returns as generic obligations, latent cargo is discarded when its carrier is lost, resolved outcomes remain terminal, and new entries start with fresh selection/movement state. Repeated reset and final conversion after reconstructed supply consumption are covered.
- **ROM-LISTING-BACKED:** reconstruction/progression concepts. Their C# ledger mapping is a **PROJECT TRANSLATION**.

### Progression and recording

- Connected Level 8 to Level 9; retained score/lives, new-level approach and Super Zapper reset behavior. Level 9 is terminal and restart returns to a fresh Level 1 campaign. Direct Level 9 startup works; Level 10 remains rejected.
- Kept recording format version 2 and generic starting-level metadata. Visible/headless Level 9 playback and presentation-only `--speed` independence are covered without adding Level 9 diagnostic metadata.
- No durable behavior or development specification was updated because manual acceptance is pending.

## Changed files

**Production files**

- `src/Tempest.Game/Gameplay/Flipper.cs`
- `src/Tempest.Game/Gameplay/Tanker.cs`
- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/Levels/LevelLoader.cs`
- `src/Tempest.Game/Levels/LevelProgression.cs`
- `src/Tempest.Game/Levels/Level9WaveSelection.cs` (new)
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/TempestGame.cs`

**Test files added or changed**

- `tests/Tempest.Game.Tests/Level9ProductionTests.cs` (new)
- `tests/Tempest.Game.Tests/ArcadeFlipperFiringTests.cs`
- `tests/Tempest.Game.Tests/CampaignSoakTests.cs`
- `tests/Tempest.Game.Tests/DebugControllerTests.cs`
- `tests/Tempest.Game.Tests/DirectLevelStartupTests.cs`
- `tests/Tempest.Game.Tests/GameplayRecordingTests.cs`
- `tests/Tempest.Game.Tests/InputDiagnosticTests.cs`
- `tests/Tempest.Game.Tests/Level7ProductionTests.cs`
- `tests/Tempest.Game.Tests/Level8ProductionTests.cs`
- `tests/Tempest.Game.Tests/LevelConfigurationTests.cs`
- `tests/Tempest.Game.Tests/LevelProgressionTests.cs`

No durable documentation files were changed. The prompt and this report are ignored AI-log records; neither is staged.

## Validation

All commands completed; no validation was blocked or timed out.

| Validation | Result |
|---|---:|
| Focused Level 9, Flipper firing, and direct-start tests | **PASS** — 60 passed |
| Levels 1–8 and affected shared regressions | **PASS** — 437 passed |
| Recording, playback, CLI, speed, and startup regressions | **PASS** — 193 passed |
| Complete solution tests: `dotnet test TempestCSharp.slnx --no-restore --blame-hang-timeout 5min --verbosity minimal` | **PASS** — 1,295 passed, 0 failed |
| Solution build: `dotnet build TempestCSharp.slnx --no-restore --verbosity minimal` | **PASS** — 0 warnings, 0 errors |
| `git diff --check` | **PASS** |

During validation, outdated tests that still treated Level 9 as unsupported or Level 8 as terminal were updated to the authorized Level 8→9 progression boundary. Final full-suite validation passed.

## Neal's manual acceptance (PENDING NEAL)

Do not treat automated validation as arcade-fidelity acceptance. In particular, tube presentation, starting Spike appearance, `$53` visible behavior, speeds, transitions, firing pressure, and overall playability remain to be compared manually.

**Direct Level 9 play**

```powershell
dotnet run --project src\Tempest.Game -- --level 9 --lives 10 --spinner-sensitivity 1
```

**Half-speed observation**

```powershell
dotnet run --project src\Tempest.Game -- --level 9 --lives 10 --spinner-sensitivity 1 --speed 0.5
```

**Record Level 9**

```powershell
dotnet run --project src\Tempest.Game -- --level 9 --lives 10 --spinner-sensitivity 1 --record-gameplay Temp\GameplayRecordings\level9-manual-acceptance.json
```

**Play that recording at half presentation speed**

```powershell
dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\level9-manual-acceptance.json --speed 0.5
```

Acceptance checklist:

1. **Tube/startup:** stepped shape/orientation, open endpoints, no false closing line or wrap, 15 lanes, Blaster direction, starting Spikes and their presentation, Level 9 identity, and no ordinary Flippers/Pulsars/Fuseballs.
2. **Tanker/`$53`:** natural and shallow cargo travel straight; deep cargo follows live Spike-sensitive behavior below/equal/above the tip; shortening/destruction changes the threshold; movement continues during pivots; destination is checked only after pivot and next along step; no invalid endpoint route.
3. **Combat/presentation:** Spiker behavior and repeated firing, Tanker/eligible Flipper firing, projectile behavior, visible Flipper/Tanker/Spiker/projectile speeds, and overall arcade/MAME pressure.
4. **Death/progression/terminal:** latent and released cargo on death, generic re-entry and fresh `$53`, Level 8→9 transition, score/lives carry, Super Zapper recharge, Level 9 completion, restart to Level 1, and recording/playback fidelity.

Automated tests—not manual play—cover identity conservation and other invisible ledger invariants.

## Final repository state

- Branch `master` still tracks `origin/master`; HEAD and `origin/master` remain `037b3f9877f38d0182a3eba4fb8a8f0279e4f1a9`.
- The index is empty. The tracked diff reports 17 modified files, 512 insertions, and 51 deletions; two new source/test files are untracked and therefore excluded from that stat.
- `git diff --check` passed. Git emitted only its existing LF-to-CRLF working-copy warning for `LevelLoader.cs`.
- Levels 1–8 gameplay was not intentionally changed; the only production progression change is the authorized Level 8→9 link. Related tests were updated for the new boundary.
- Level 10 was not implemented. No commit or push occurred.
- **Manual acceptance: PENDING NEAL.**
