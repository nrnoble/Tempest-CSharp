# Final Level 13 closeout

**Status: VALIDATION PASS; COMMIT AND PUSH PENDING**

## Starting state and scope

- Starting branch: `master`
- Starting HEAD: `534cbf21150dc501300c548f98972a4c64198602`
- Upstream: `origin/master`
- Starting ahead/behind: `0/0`
- Starting index: empty
- Starting tracked worktree: accumulated Level 13 implementation and
  depth-Y diagnostic candidate, plus documentation/test updates belonging to
  those efforts.
- GitHub connector: **PASS**; independently returned remote `master` at
  `534cbf21150dc501300c548f98972a4c64198602`, matching the local baseline.
- Commit: pending
- Push: pending

This closeout froze the supplied accepted candidate. No further gameplay,
presentation, ROM, or ExitZoom investigation was performed; no interactive
game was launched; Level 14 was not started.

## Manual acceptance and accepted behavior

Neal's supplied Level 13 manual acceptance is recorded without extending its
arcade-fidelity scope. Accepted items: genuine Level 13 tube identity;
closed-tube gameplay and spinner behavior; normal gameplay and enemy mix;
Tanker behavior/cargo, Spiker behavior, and Fuseball behavior; absence of
Pulsars; starting Spikes; firing and Super Zapper; death/re-entry; wave
completion; HUD Level 13; and normal Atari Playing presentation after
calibration.

Displayed Level 13 is mapped to ROM-backed shape row 14. Its ordered geometry,
angle data, closed 16-lane topology/final-to-first wraparound, model-frame
mapping, presentation metadata, and production/entity registration were
preserved. The existing geometry is not claimed to be a literal C# rendering
equation; centering, scaling, and orientation remain PROJECT TRANSLATIONS.

Accepted Atari-presentation defaults:

| Tube Y gain | Tube scale | Vertical offset | Depth-Y gain |
|---:|---:|---:|---:|
| 1.2 | 1.0 | 0 | 0.3 |

Neal manually compared the Level 13 presentation against MAME. The non-neutral
depth-Y value `0.3` is a manually accepted **PROJECT TRANSLATION**, not an
Atari ROM constant. The geometry was not shown to be wrong; the near-rim
affine silhouette remains structurally preserved. The shared reciprocal-depth
camera-origin term produced the large depth-dependent vertical drift, which
the prior Y-gain, scale, and vertical-offset controls could not independently
adjust. The general development-only `--tube-depth-y-gain` exposes that degree
of freedom. It remains available for supported Atari-presentation levels,
accepts finite signed values subject to representable projection behavior,
and is independent of the other tube controls. `1` is neutral; `0` removes the
depth-dependent camera-origin contribution; fractions reduce it; values above
1 exaggerate it; negative values reverse it. Defaults for Levels 1-12 and
direct Level 15 remain unchanged.

Production reachability for Level 13 is Tankers, Spikers, and Fuseballs.
Ordinary Flipper's raw maximum does not independently enable its picker when
its minimum/eligibility does not permit selection. Flipper cargo remains in
the Tanker lifecycle. Pulsars remain absent before Level 17.

Production progression is `9 -> 10 -> 11 -> 12 -> 13`; the natural Level 12
handoff to Level 13, score/lives carry, between-level Super Zapper behavior,
HUD update, and active presentation configuration transition are preserved.
Level 13 remains terminal while Level 14 is unimplemented.
`--start-level 13` is a genuine production start. Direct-only Level 15 remains
outside normal campaign progression.

**Deferred, non-blocking presentation discrepancy:** Level 13 ExitZoom is not
arcade-perfect. Neal reports the exiting perspective is not fully correct,
similar in character to the separately deferred Level 12 issue, but not
distracting enough to block acceptance. The Blaster should ideally remain
visible throughout the exit. Apparent camera/Blaster speed divergence is an
observation/hypothesis, not a proven simulation defect. Global accepted
ExitZoom behavior is preserved; no Level-13-specific correction was made.
A future deterministic playback/state comparison could distinguish actual
simulation divergence from projection-created apparent motion. This is not
unresolved gameplay, and the Level 12 issue remains separately deferred.

## Validation

- **PASS — focused Level 13/calibration selection:** 81 passed, 0 failed,
  0 skipped. Covered `Level13ProductionTests`,
  `AtariPresentationCalibrationTests`, and `TubeDepthYGainTests`, including
  row-14 geometry/topology, species reachability, Level 12-to-13 progression,
  the 1,200-tick deterministic headless production soak, calibration defaults,
  parser/projection/composition, neutral Levels 1-12, and direct Level 15.
- **PASS — complete Release solution tests:** 1,917 passed, 0 failed,
  0 skipped, using
  `dotnet test TempestCSharp.slnx --configuration Release --no-restore --blame-hang-timeout 5min`.
- **PASS — Release solution build:** 0 warnings, 0 errors, using
  `dotnet build TempestCSharp.slnx --configuration Release --no-restore`.
- **PASS — CLI help:** `dotnet run --project src\Tempest.Game
  --configuration Release --no-build -- --help`; help reports the Level 13
  depth-Y default and did not launch a window.
- **PASS — staged whitespace check:** `git diff --cached --check`.
- The 1,200-tick Level 13 headless production soak passed in the full suite.
- No gameplay regression was found in the final complete suite. No interactive
  game was launched; Neal's manual acceptance was supplied by the task.

## Staged inventory

Exactly 40 files are staged for this closeout; the final report's own SHA and
post-push fields will be completed after push and left for the next normal
project commit rather than creating an amend/report loop.

- `AI-Logs/Prompts/2026-11-23-agent-runtime-capability-probe-prompt.md`
- `AI-Logs/Prompts/2026-11-23-level13-fast-production-implementation-prompt.md`
- `AI-Logs/Prompts/2026-11-23-level13-final-closeout-prompt.md`
- `AI-Logs/Prompts/2026-11-23-level13-presentation-divergence-prompt.md`
- `AI-Logs/Prompts/2026-11-23-tube-depth-y-gain-diagnostic-prompt.md`
- `AI-Logs/Reports/2026-11-23-agent-runtime-capability-probe-report.md`
- `AI-Logs/Reports/2026-11-23-level13-fast-production-implementation-report.md`
- `AI-Logs/Reports/2026-11-23-level13-presentation-divergence-report.md`
- `AI-Logs/Reports/2026-11-23-start-level-level10-chain-closeout-final-report.md`
- `AI-Logs/Reports/2026-11-23-tube-depth-y-gain-diagnostic-report.md`
- `AI-Logs/Reports/2026-11-23-level13-final-closeout-report.md`
- `README.md`
- `TEMPEST-BEHAVIOR-SPEC.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `src/Tempest.Game/Debug/DebugController.cs`
- `src/Tempest.Game/Gameplay/Tanker.cs`
- `src/Tempest.Game/Levels/AtariTubeRecordCatalog.cs`
- `src/Tempest.Game/Levels/Level13WaveSelection.cs`
- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/Levels/LevelLoader.cs`
- `src/Tempest.Game/Levels/LevelProgression.cs`
- `src/Tempest.Game/Levels/RomPresentationMetadata.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`
- `src/Tempest.Game/Rendering/AtariPresentationCalibration.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `src/Tempest.Game/TempestGame.cs`
- `tests/Tempest.Game.Tests/AtariPresentationAdapterTests.cs`
- `tests/Tempest.Game.Tests/AtariPresentationCalibrationTests.cs`
- `tests/Tempest.Game.Tests/DebugControllerTests.cs`
- `tests/Tempest.Game.Tests/DirectLevelStartupTests.cs`
- `tests/Tempest.Game.Tests/GameplayRecordingTests.cs`
- `tests/Tempest.Game.Tests/InputDiagnosticTests.cs`
- `tests/Tempest.Game.Tests/Level12ProductionTests.cs`
- `tests/Tempest.Game.Tests/Level13ProductionTests.cs`
- `tests/Tempest.Game.Tests/Level15TubeCandidateTests.cs`
- `tests/Tempest.Game.Tests/LevelConfigurationTests.cs`
- `tests/Tempest.Game.Tests/RomPresentationMetadataTests.cs`
- `tests/Tempest.Game.Tests/StartLevelTests.cs`
- `tests/Tempest.Game.Tests/TubeDepthYGainTests.cs`

The staged inventory contains no ROMs, screenshots, MAME references, `Temp`,
`TEMP`, `Videos`, recordings, `UserData`, build outputs, or private scratch
material. The untracked `Reference/arcade/MBUCOD.V05.txt` and
`temo/gameplayrecordings/level-13` were inspected only as status entries and
remain untouched and unstaged.

## Pending closeout actions and final state

Staged review and `git diff --cached --check` passed; staged file ownership is
within this milestone. Commit `Complete accepted Level 13` on current `master`,
push normally to `origin/master`, and independently verify the new GitHub
master commit. After push, update this report with the commit SHA, push
outcome, connector result, and final local branch/upstream/ahead-behind/index/
worktree state. Do not amend or force-push. Preserve excluded untracked local
and reference material. Do not begin Level 14.
