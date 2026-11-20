# Level 9 Complete Accepted Closeout Report

Date: 2026-11-12
Result: **LEVEL 9 CLOSED — committed and pushed.**

## Starting checkpoint and candidate inventory

- Started on `master`, tracking `origin/master`.
- Starting `HEAD` and `origin/master` both matched
  `037b3f9877f38d0182a3eba4fb8a8f0279e4f1a9`
  (`Complete accepted Level 7 behavior`).
- The starting index was empty. The accumulated candidate contained 22
  modified tracked files and three untracked Level 9 source/test files, plus
  ignored AI-Logs records. Every candidate path matched the implementation
  and Spiker-correction reports; no unrelated project path was present.
- No candidate work was reset, restored, discarded, or stashed.
- The complete substantive task was copied verbatim to
  `AI-Logs/Prompts/2026-11-12-level9-complete-accepted-closeout-prompt.md`
  and verified byte-for-byte against the read-only user attachment.

## Accepted Level 9 scope and evidence

Neal reports that overall Level 9 mechanics pass manual testing. This is
overall gameplay acceptance, not a claim that he inspected invisible ledger
identities or each synthetic test edge case. The exact natural Level 8→9
handoff is covered by automated progression tests; no separate manual
acceptance of that transition is claimed.

- **Geometry/topology:** Level 9 uses Rev. 2 shape row 13, with 16 ordered
  vertices, an open stepped tube, and 15 playable lanes (0–14), without a
  closing edge. Raw shape/angle/presentation data are retained as
  REV-2-BINARY-CONFIRMED. C# centering, scaling, Y reflection, palette and
  presentation remain project translations; no separate visual/pixel
  equivalence claim is made.
- **Wave/selection:** ROM-backed Level 9 supply is 27, with a six-unit
  ordinary-entry gate and Tanker 1–2 / Spiker 2–4 bounds. Ordinary bottom
  Flippers, Pulsars and Fuseballs are excluded. The deterministic
  post-minimum selector is a project approximation. Tankers reserve two
  latent Flipper cargo units; released cargo bypasses the ordinary gate.
  Final-pending Spiker-to-Tanker conversion adds no generic supply.
- **Spikes, Flippers and cargo:** All 15 playable lanes start with Spikes at
  the current project-translated height of about 85.7143 C# depth units; this
  is not a literal ROM height. Level 9 `$53` performs a post-move live-Spike
  check on the current lane. Natural and shallow Tanker cargo use straight
  `$07`; deep shot-released cargo at raw along `$20` and above uses `$53`.
- **Lifecycle/firing:** Generic post-death reconstruction is enabled, latent
  cargo is discarded with a lost carrier, released unresolved cargo returns
  as generic obligations, and resolved outcomes remain terminal. Level 9
  repeated-cycle Spiker firing and Tanker firing use the shared projectile
  path.
- **Progression/startup:** Natural Level 8→9 progression, direct `--level 9`,
  terminal Level 9 behavior, and Level 10 rejection are covered. Level 10
  remains unimplemented.
- **Recording/playback:** Recording format version 2 remains in use, including
  visible/headless Level 9 startup/playback. Normal recordings use production
  defaults; optional `--level9-spiker-speed` and its playback metadata are
  DEVELOPMENT ONLY calibration tooling. Legacy Level 9 recordings without
  that metadata remain compatible.
- Accepted Levels 1–8 remain frozen; no earlier-level production policy or
  random-number consumption was intentionally changed.

## Spiker/projectile defect and speed qualification

- Neal manually rejected the original projectile-lag behavior and now reports
  **THE PROBLEM IS CORRECTED**: Level 9 Spiker shots visibly travel faster
  than their firing Spiker. The blocking relative-speed defect is closed.
- The durable ARCADE/MAME-OBSERVED fidelity requirement is that a Spiker
  projectile travel visibly faster than its firing Spiker.
- Normal Level 9 production Spiker speed is **2.0 C# depth units per active
  step**, an accepted **PROJECT CALIBRATION**, not a ROM measurement.
  Shared enemy-projectile movement remains **3.0**.
- Neal did not independently verify that absolute 2.0 speed matches the
  arcade. The C# build feels significantly more difficult/faster overall
  across implemented levels, making absolute Level 9 speed difficult to
  isolate. No Level-9-only speed retuning was wanted during closeout.
- The bounded ROM/static trace yields equal Level 9 Spiker/projectile movement
  fields, which conflicts with the observed projectile-faster relationship.
  This ROM/static contradiction remains **UNRESOLVED**; no reconciliation or
  ROM-to-C# numeric conversion is claimed.
- The inherited relative-speed risk in the Level 4 candidate and production
  Levels 5, 6 and 8 remains a distinct future investigation. Those levels
  were not retuned.

## Durable documentation and deferred work

Updated `TEMPEST-DEVELOPMENT-NOTES.md` and `TEMPEST-BEHAVIOR-SPEC.md` with the
accepted Level 9 scope, provenance, manual-acceptance limits, speed
qualification, unresolved ROM/static contradiction, development-only
calibration tooling, progression boundary, and follow-up milestones. Historical
Level 8 terminal statements are identified as checkpoint-specific. Historical
AI reports were not rewritten.

Remaining deferred milestones are separate from and do not block Level 9:

1. Improve held reverse-playback stepping responsiveness while preserving
   exact single-frame reverse taps.
2. Improve enemy-shot vector-rendering fidelity without changing gameplay.
3. Refine whole-game screen/tube/HUD presentation, keep gameplay dimensions
   constant, define the 16 distinct layouts once and recycle from Level 17,
   and consider fullscreen/borderless presentation.
4. Investigate whole-game difficulty/timing across interacting movement,
   firing, collision and simulation timing; do not presume one global speed
   multiplier.
5. Investigate the cross-level Spiker/projectile invariant separately for
   Levels 4, 5, 6 and 8.

## Final validation

All commands completed on the default Debug `bin` output route. A process
check before validation found no running `Tempest.Game` process; no output
lock or Windows security block occurred. The interactive game was not
launched.

| Validation | Result |
|---|---:|
| Focused Level 9 production/integration selection | **PASS** — 166 passed |
| Complete Level 9 test subset | **PASS** — 39 passed |
| Levels 1–8 targeted regression selection | **PASS** — 726 passed |
| CLI, startup, recording and playback regression selection | **PASS** — 230 passed |
| Spiker/projectile speed-selection regression | **PASS** — 42 passed |
| `dotnet test TempestCSharp.slnx --no-restore --blame-hang-timeout 5min --verbosity minimal` | **PASS** — 1,336 passed, 0 failed, 0 skipped |
| `dotnet build TempestCSharp.slnx --no-restore --verbosity minimal` | **PASS** — 0 warnings, 0 errors |
| `git diff --check` | **PASS** |
| `git diff --cached --check` | **PASS** |

## Staging, commit and push

The staged allowlist contained exactly these 27 files:

```text
TEMPEST-BEHAVIOR-SPEC.md
TEMPEST-DEVELOPMENT-NOTES.md
src/Tempest.Game/Debug/DebugController.cs
src/Tempest.Game/Debug/GameplayRecordingFile.cs
src/Tempest.Game/Debug/GameplayStartupConfiguration.cs
src/Tempest.Game/Gameplay/EnemyBullet.cs
src/Tempest.Game/Gameplay/Flipper.cs
src/Tempest.Game/Gameplay/Tanker.cs
src/Tempest.Game/Levels/Level9WaveSelection.cs
src/Tempest.Game/Levels/LevelConfiguration.cs
src/Tempest.Game/Levels/LevelLoader.cs
src/Tempest.Game/Levels/LevelProgression.cs
src/Tempest.Game/Program.cs
src/Tempest.Game/TempestGame.cs
tests/Tempest.Game.Tests/ArcadeFlipperFiringTests.cs
tests/Tempest.Game.Tests/CampaignSoakTests.cs
tests/Tempest.Game.Tests/DebugControllerTests.cs
tests/Tempest.Game.Tests/DirectLevelStartupTests.cs
tests/Tempest.Game.Tests/GameplayRecordingTests.cs
tests/Tempest.Game.Tests/InputDiagnosticTests.cs
tests/Tempest.Game.Tests/Level7ProductionTests.cs
tests/Tempest.Game.Tests/Level7SpikerProjectileRelativeSpeedDiagnosticTests.cs
tests/Tempest.Game.Tests/Level8ProductionTests.cs
tests/Tempest.Game.Tests/Level9ProductionTests.cs
tests/Tempest.Game.Tests/Level9SpikerProjectileRelativeSpeedTests.cs
tests/Tempest.Game.Tests/LevelConfigurationTests.cs
tests/Tempest.Game.Tests/LevelProgressionTests.cs
```

No AI-Logs, TEMP, recordings, screenshots, UserData, videos, ROM/reference
assets, build outputs, isolated validation output, or local scratchpad material
was staged.

- Commit subject: `Complete accepted Level 9 behavior`
- Full commit: `cd678572c009d27fa8e1e1f0059ec45f6db5db11`
- Push: `git push origin master` **PASS**; `master` updated from
  `037b3f9` to `cd67857`.
- Post-push local `HEAD` and `origin/master` both equal
  `cd678572c009d27fa8e1e1f0059ec45f6db5db11`.
- Final Git status: `## master...origin/master`; tracked worktree and index
  clean; no nonignored untracked paths.

**LEVEL 9 IS CLOSED.** Level 10 has not begun. No deferred investigation or
implementation has begun.
