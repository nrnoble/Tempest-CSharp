# Enemy-shot ROM four-frame rendering candidate

Date: 2026-11-12
Disposition: **PARTIAL — IMPLEMENTATION CANDIDATE; FULL TEST RUN BLOCKED; MANUAL ACCEPTANCE PENDING**

## Starting Git state

- Branch: `master`
- HEAD: `cd678572c009d27fa8e1e1f0059ec45f6db5db11` (`Complete accepted Level 9 behavior`)
- `origin/master`: same commit
- Staged, tracked-worktree, and non-ignored untracked changes: none

No files were staged, committed, or pushed.

## Files read

- `AI-Logs/Reports/2026-11-12-enemy-shot-rom-vector-rendering-recovery.md`
- `src/Tempest.Game/TempestGame.cs`
- `src/Tempest.Game/Gameplay/EnemyBullet.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `src/Tempest.Game/Rendering/PlayerShotVectorProgram.cs`
- `src/Tempest.Game/Rendering/Projection.cs`
- `src/Tempest.Game/Levels/Level.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Debug/GameplayPlaybackSession.cs`
- `src/Tempest.Game/Debug/VisiblePlaybackController.cs`
- `tests/Tempest.Game.Tests/Tempest.Game.Tests.csproj`
- Focused enemy-bullet, projectile, simulation, and playback test files listed below
- `TEMPEST-DEVELOPMENT-NOTES.md`

The recovery report was used as the geometry/evidence authority. No raw ROM
file or project-local disassembly was opened.

## Files changed

- Added `src/Tempest.Game/Rendering/EnemyShotVectorProgram.cs`.
- Updated `src/Tempest.Game/Rendering/VectorRenderer.cs` and
  `src/Tempest.Game/TempestGame.cs`.
- Updated focused tests in:
  - `tests/Tempest.Game.Tests/Level1ProductionEnemyBulletRenderingTests.cs`
  - `tests/Tempest.Game.Tests/GameplayPlaybackSessionTests.cs`
  - `tests/Tempest.Game.Tests/VisiblePlaybackControllerTests.cs`
  - `tests/Tempest.Game.Tests/Level2FourFlipperCapTests.cs`
  - `tests/Tempest.Game.Tests/Level2SquareTests.cs`
  - `tests/Tempest.Game.Tests/Level3TestModeTests.cs`
  - `tests/Tempest.Game.Tests/SimulationTests.cs`
- Updated `TEMPEST-DEVELOPMENT-NOTES.md` to record candidate status and
  pending manual acceptance.
- Preserved the supplied prompt at
  `AI-Logs/Prompts/2026-11-12-enemy-shot-rom-four-frame-implementation-prompt.md`.

## Implementation and evidence

**ROM-BACKED:** Each frame contains four ordered lit segments and four ordered
point marks in cumulative relative logical coordinates (X right, Y up).
Blank beam moves are omitted from visible output. The encoded segment and point
ROM color indices are 0 and 3. No full radial star or connecting line is
generated.

| Frame | Four lit segments, start -> end | Four point marks |
|---|---|---|
| 1 | `(-11,11)->(-17,17)`; `(-17,-11)->(-11,-17)`; `(17,-17)->(11,-11)`; `(11,17)->(17,11)` | `(6,6)`, `(-6,6)`, `(-6,-6)`, `(6,-6)` |
| 2 | `(-18,18)->(-18,4)`; `(-8,-14)->(-8,-22)`; `(18,-12)->(18,-4)`; `(8,14)->(8,22)` | `(-3,7)`, `(-7,3)`, `(3,-7)`, `(7,3)` |
| 3 | `(-17,3)->(-23,-3)`; `(-3,-23)->(3,-17)`; `(17,-3)->(23,3)`; `(3,23)->(-3,23)` | `(0,8)`, `(-8,0)`, `(0,-8)`, `(8,0)` |
| 4 | `(-22,-8)->(-14,-8)`; `(4,-18)->(12,-18)`; `(14,8)->(22,8)`; `(-4,18)->(-12,18)` | `(-7,3)`, `(-3,-7)`, `(7,3)`, `(3,7)` |

**PROJECT TRANSLATION:** `TempestGame` owns a dedicated modulo-16 simulation
step counter. Initial frame is 0; one frame advances per four `RunOneStep`
calls. This cadence is not a ROM-proven frequency. Rendering does not advance
the phase; pause has no simulation steps; forward stepping advances it; replay
reconstruction derives the same phase from the same boundary. A newly
constructed game starts at frame 0; the phase is not tied to bullet age,
rotation, lane, order, renderer calls, wall time, or another mechanic's clock.
The recording schema is unchanged.

**PROJECT TRANSLATION:** Rendering flips logical Y-up to screen Y-down and
uses the existing lane-midpoint/depth projection. The logical-unit scale is
`6 / ((projectedDepth + Projection.ZOffset) / Projection.K) / 24`; 24 is a
tunable normalization, not literal pixels. Each zero-displacement ROM mark is
submitted as one deterministic screen pixel.

The candidate retains white rendering, active-bullet collection authority,
existing game-state visibility gates, `EnemyBullet.Rotation` advancement,
projectile creation/movement/collision/expiration, and playback schema.
No gameplay behavior was intentionally changed.

## Tests and validation

Added or updated coverage checks the exact geometry and ROM color indices for
all four frames, four disconnected segments and four point marks, screen-Y
translation, white output, shared selection for bullets with different ages
and rotation, lane midpoint and depth scaling, render idempotence, four-step
phase progression/wrap, pause/forward/reverse reconstruction, active-bullet
visibility, and unchanged projectile lifecycle/rotation assertions.

- **PASS (earlier focused run):**
  `dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~Level1ProductionEnemyBulletRenderingTests|FullyQualifiedName~GameplayPlaybackSessionTests|FullyQualifiedName~VisiblePlaybackControllerTests" --blame-hang-timeout 2m`
  — 49 passed. This run preceded the final test-helper refactor.
- **PASS (earlier regression run):**
  `dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~Level1ProductionEnemyBulletRenderingTests|FullyQualifiedName~Level2FourFlipperCapTests|FullyQualifiedName~Level2SquareTests|FullyQualifiedName~Level3TestModeTests|FullyQualifiedName~SimulationTests|FullyQualifiedName~ArcadeFlipperFiringTests|FullyQualifiedName~Level7SpikerFiringTests|FullyQualifiedName~Level9ProductionTests|FullyQualifiedName~GameplayPlaybackTests|FullyQualifiedName~GameplayRecordingTests|FullyQualifiedName~GameplayPlaybackSessionTests|FullyQualifiedName~VisiblePlaybackControllerTests" --blame-hang-timeout 2m`
  — 536 passed. This run preceded the final test-helper refactor.
- **PASS:**
  `dotnet build TempestCSharp.slnx --no-restore`
  — build succeeded, 0 warnings, 0 errors. The later full test invocation also
  built the current project/test assemblies before test discovery was blocked.
- **PASS:** `git diff --check` — no whitespace errors. Git emitted a
  line-ending normalization warning for `VectorRenderer.cs`.
- **BLOCKED (not a pass):**
  `dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --blame-hang-timeout 2m`
  built the assemblies, but test discovery could not load:
  `C:\Users\nrnob\source\repos\Tempest\tests\Tempest.Game.Tests\bin\Debug\net9.0\Tempest.Game.Tests.dll`
  Error: `An Application Control policy has blocked this file. (0x800711C7)`.
  VSTest consequently reported that no tests were available. The process exit
  code does not make this a pass. The blocked artifact was not retried and no
  security control was bypassed.

## Unresolved questions and manual acceptance

- Exact arcade animation frequency, startup alignment, and timer-to-C#-step
  mapping remain unknown.
- One-pixel rasterization and the 24-unit normalization are candidate
  translations; screen appearance, point visibility, and apparent size remain
  unverified.
- White remains a project constraint, not a claim that the ROM indices have
  equal RGB color.
- No interactive game launch or manual visual acceptance was performed.

After the Application Control policy issue is resolved through the
organization's approved process, Neal should run the automated test project
again. Do not bypass the policy or reuse the blocked artifact. Manual
acceptance remains with Neal; the candidate is not accepted or arcade-identical.

Launch command for Neal:

```powershell
dotnet run --project src\Tempest.Game -- --enemy-bullets-test --lives 10
```

Manual checklist:

- Confirm a four-frame animation replaces the rotating radial star.
- Confirm simultaneous enemy bullets stay synchronized despite different
  ages and `Rotation` values.
- Compare near/far scaling and check that all point marks remain visible.
- Check that the four line segments remain disconnected.
- Confirm the color remains white.
- Confirm movement, speed, firing, collisions, sound, and gameplay are
  unchanged.
- Check pause, forward stepping, and reverse reconstruction using a suitable
  existing recording.

## Final status

The candidate remains unstaged in the worktree on `master`, based on
`cd678572c009d27fa8e1e1f0059ec45f6db5db11`. The new rendering source file is
untracked; the other listed source/test/documentation files are modified.
Ignored `AI-Logs/` records are not shown by ordinary status. No commit or push
occurred.

No images, screenshots, video frames, contact sheets, recordings, or other
visual assets were searched for, opened, extracted, or analyzed.
