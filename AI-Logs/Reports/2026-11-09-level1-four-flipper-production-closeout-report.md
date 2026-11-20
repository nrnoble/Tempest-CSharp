# Production Level 1 Four-Flipper Closeout

Date: 2026-11-09
Disposition: **MANUALLY ARCADE-FIDELITY ACCEPTED — VALIDATED — COMMITTED — PUSHED**

## Starting state and historical evidence

- Starting branch: `master`, tracking `origin/master`.
- Starting `HEAD`: `c1a974a625eea2dfaecf30d1b8d729e022ef17e8`
  (`Close out accepted spinner sensitivity diagnostic`), zero ahead/behind.
- The index was empty. The accepted production candidate was already present
  unstaged; this closeout preserved it rather than redoing its implementation.
- The protected untracked `Reference/arcade/roms.rev2/` directory contained
  21 local ROM chip files. Two new Level 1 production test files were also
  untracked. No other candidate changes were found.
- Read without rewriting:
  - `2026-11-09-low-level-difficulty-comparative-investigation-report.md`
  - `2026-11-09-level1-four-flipper-cap-diagnostic-report.md`
  - `2026-11-09-level1-cap-enemy-bullet-visibility-fix-report.md`
  - `2026-11-09-level1-production-four-flipper-correction-report.md`
- The complete closeout prompt was archived at
  `AI-Logs/Prompts/2026-11-09-level1-four-flipper-production-closeout-prompt.md`.
  Its SHA-256 matched the supplied task attachment.

## Manual acceptance and accepted behavior

Neal supplied final acceptance of ordinary production Level 1. Normal
production retains the improved arcade-like threat density, with no more than
four active Flippers and visibly fewer Flippers occupying the tube. Ready Eggs
wait and later hatch; all ten obligations appear and resolve; the wave remains
substantial without feeling artificially sparse. Enemy bullets are visible,
move, sound, collide, and threaten the Blaster normally. Rim pursuit remains
threatening, stationary defense is not trivially safe, spinner control and
player firing remain correct, Super Zapper remains normal, and death/re-entry
is coherent. Level 1 completes and naturally advances to Level 2; Level 2
remains unchanged.

Neal also played and recorded the complete production campaign through
Levels 1-8 with no blocking regression observed. This is manual acceptance of
the production Level 1 feel and behavior, not a claim that every hidden timing
value is literally arcade-equivalent.

- **ROM-LISTING-BACKED / REV-2-BINARY-CONFIRMED:** ten original Level 1
  Flipper obligations; maximum four concurrent Flippers.
- **PROJECT TRANSLATION:** the authoritative active Flipper collection enforces
  the cap. At four active Flippers, an eligible Egg stays unresolved in
  `ReadyToHatch`; it is not discarded, returned to reserve, scored, cleared,
  rerolled, or allowed to consume hatch randomness while blocked. It hatches
  normally when a slot opens. This is not claimed to be the ROM's literal
  pending-entry representation.
- The separate ROM six-total-active gate is not implemented. Level 2
  concurrency is separate deferred work. Levels 3-8 remain unchanged by the
  Level 1 cap.
- The accepted four starting Eggs, six reserve originals, 50-tick release and
  approach schedule, rim speed 2.5, firing, shots, enemy bullets, movement,
  scoring, collisions, Super Zapper, death/re-entry, and recording schema
  remain unchanged.
- Enemy-bullet rendering uses active bullet state and preserves the existing
  white geometry, size, rotation, projection, simulation, sound, and collision.
  Production Level 1 does not require `--enemy-bullets-test`.
- `--level1-four-flipper-cap-test` is not accepted. The obsolete scenario
  identity, parser and terminal behavior are gone. Running the old option
  returned exit code 2 with an explanatory error before game initialization.
- The hours-long test hang was a stale, unbounded helper loop that assumed
  every Level 1 Egg could hatch without resolving an active Flipper. The
  four-Flipper cap correctly left a ready Egg waiting; production was not
  deadlocked. The test now stops at the expected cap/wait boundary and checks
  conservation, and progression fixture loops are bounded. No hang dump or
  sequence artifact remains.

## Documentation and committed files

Updated the Level 1 acceptance/evidence section in README and the behavior and
development references. They now classify the four-Flipper value, C# waiting
translation, manual production acceptance, unchanged Levels 2-8 campaign
foundation, deferred Level 2 concurrency and six-total-active gate, and
remaining timing uncertainty.

The 12-file reviewed and committed allowlist was:

- `README.md`
- `TEMPEST-BEHAVIOR-SPEC.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `src/Tempest.Game/TempestGame.cs`
- `tests/Tempest.Game.Tests/Level1ProductionEnemyBulletRenderingTests.cs`
- `tests/Tempest.Game.Tests/Level1ProductionFlipperCapTests.cs`
- `tests/Tempest.Game.Tests/Level3TestModeTests.cs`
- `tests/Tempest.Game.Tests/Level4CandidateTests.cs`
- `tests/Tempest.Game.Tests/LevelProgressionTests.cs`
- `tests/Tempest.Game.Tests/SimulationTests.cs`

## Validation

The project uses .NET SDK 9.0.304, xUnit v2 and VSTest. All test invocations
used `--blame-hang-timeout 60s`.

| Validation | Result |
|---|---|
| Production cap, Egg waiting/accounting, death/re-entry, Zapper, completion, Levels 2-8 isolation, and playback class | 8 passed |
| Production enemy-bullet firing, rendering, movement, collision, and Levels 1-8 presentation class | 12 passed |
| Combined cap and enemy-bullet focused selection | 20 passed |
| Expanded affected regression selection: Egg/configuration, cap, bullet, progression, Zapper, death, recording/playback, firing, campaign, direct startup, Levels 2-8 | 504 passed |
| `SimulationTests` | 315 passed |
| `dotnet test TempestCSharp.slnx --blame-hang-timeout 60s` | 1,118 passed; 0 failed; 0 skipped |
| `dotnet build TempestCSharp.slnx` | Passed; 0 warnings; 0 errors |
| Unstaged `git diff --check` | Passed |
| Staged `git diff --cached --check` | Passed |
| Removed diagnostic-option check | Rejected with exit code 2 before game initialization |

Git printed only line-ending normalization warnings for README and the vector
renderer. No Windows Application Control block occurred. The interactive game
was not launched.

## Commit, push, and final repository state

- Commit: `f362af7f87c23a9b983937f8733f5a4ac71d0162`
- Subject: `Implement accepted Level 1 four-Flipper maximum`
- Push: successful to `origin` (`https://github.com/nrnoble/Tempest-CSharp.git`),
  branch `master`.
- Local `HEAD`: `f362af7f87c23a9b983937f8733f5a4ac71d0162`
- `origin/master`: `f362af7f87c23a9b983937f8733f5a4ac71d0162`
- Live remote `master`: `f362af7f87c23a9b983937f8733f5a4ac71d0162`
- Final status: `## master...origin/master`, plus the pre-existing untracked
  `Reference/arcade/roms.rev2/` directory. Tracked worktree and index are clean.
- Protected ROM files, `UserData/`, screenshots, gameplay recordings,
  `Videos/`, `TEMP/`, `Temp/`, build output, and test dumps/sequences were not
  staged or committed. AI logs remain Git-ignored and were not staged.

## Remaining unknowns and closeout

The exact arcade timing of Level 1 Egg entry/release, the literal ROM
pending-entry implementation, and equivalence of every hidden Level 1 timing
value remain unproven. No blocking issue remains within this milestone.
Level 2 concurrency and the ROM six-total-active gate remain explicitly
deferred. No Level 9, rim-speed or Egg-schedule retuning, broader difficulty
tuning, or unrelated cleanup was started.

The production Level 1 four-Flipper correction is manually accepted, validated,
committed, pushed, and closed. Levels 1-8 remain the accepted production
campaign foundation.
