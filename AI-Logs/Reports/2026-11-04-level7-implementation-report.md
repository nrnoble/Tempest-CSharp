# Level 7 implementation report

Date: 2026-11-04.

## Baseline

- Verified starting HEAD: `79e33886772424a31973332f33f3781b55e4c2f8`.
- The baseline worktree and index were clean before implementation. The final
  worktree contains only the Level 7 source, test, and documentation changes
  described below; nothing is staged.
- The complete substantive task, including its completion token, is preserved
  in `AI-Logs/Prompts/2026-11-04-level7-implementation-prompt.md`.

## Implementation

- Added immutable production Level 7 identity 6/display 7/board 6/palette 0:
  the exact source-ordered closed 16-lane clover; center Y 310, offset -10,
  depth 400, ready position 50 and player lane 7 at 0.5.
- Configured 20 ROM-backed original obligations, represented as four initial
  Eggs and 16 reserve, with Flippers, Tankers and Spikers eligible and
  Pulsars/Fuseballs excluded. This intentionally follows the ROM over
  Incitti's conflicting 25-original calculation. The accepted mixed picker
  and ledger remain in use. The level starts with 16 height-60 Spikes.
- Set explicit Level 7 Flipper firing values to successful-shot holdoff 62
  and active-shot capacity 3. Avoid Spikes threshold 30 yields 31 updates,
  then the existing 21-update pretravel and unchanged exit travel. Levels
  4/5 remain 151 updates and Level 6 remains 71.
- Added Level 7-only ordinary-Flipper `$24` route state per instance:
  two completed flips, reverse, three completed flips, reverse, repeat. It
  uses a fixed C# initial direction of `+1` and existing directed pivot
  geometry. No generic 20-tick lane pause was added. Flip-start does not
  advance height; active pivot updates advance turning and height together;
  full reflex sweeps complete before rim handoff.
- The physical `+1` direction is a reversible **PROJECT APPROXIMATION**.
  The 9-degree turn and 2-height-per-update cadence are also **PROJECT
  APPROXIMATIONS**, not ROM cycle timing. Existing rim pursuit remains
  separate and unchanged.
- Tankers remain straight-lane climbers. Only deep player-shot Level 7
  Tanker destruction gives cargo independent two/three routes with opposite
  initial directions. Natural/near-rim cargo keeps the existing path.
  `Height <= 370` is a named **PROJECT APPROXIMATION** at the release seam,
  not an exact ROM coordinate conversion. Reconstructed cargo retains
  identity but gets fresh runtime route state.
- Added natural Level 6 -> 7 progression at the existing safe Spike-exit
  commit point. It carries score/lives, creates a fresh Level 7 ledger,
  enemies and Spikes, restores two Zapper charges, clears stale state, and
  uses the accepted mixed-color approach and input policy. Failed exits do
  not install Level 7.
- Added `--level7-test` as **DEVELOPMENT ONLY**, with the production Level 7
  configuration, firing, ordinary/cargo movement and normal gameplay
  systems. It remains isolated and terminal, composes with `--lives`, and
  restarts/replays Level 7.
- Natural Level 7 is terminal by **PROJECT DECISION**; natural restart starts
  Level 1. Level 8 is not implemented. Death/reconstruction preserves
  accepted ledger, identity, score/lives, charge and Spike behavior while
  creating fresh route state.
- No Levels 1-6 behavior was intentionally retuned. No reference, recording,
  video/frame or audio implementation was changed.

## Files changed

- Production: `src/Tempest.Game/Levels/LevelConfiguration.cs`,
  `LevelLoader.cs`, `LevelProgression.cs`,
  `src/Tempest.Game/Gameplay/Flipper.cs`, `Tanker.cs`,
  `src/Tempest.Game/TempestGame.cs`,
  `src/Tempest.Game/Debug/DebugController.cs`, and
  `src/Tempest.Game/Program.cs`.
- Tests: new `tests/Tempest.Game.Tests/Level7ProductionTests.cs` and
  `Level7FlipperMovementTests.cs`; updated
  `ArcadeFlipperFiringTests.cs`, `CampaignSoakTests.cs`,
  `ExitLaneControlTests.cs`, `LevelConfigurationTests.cs`, and
  `LevelProgressionTests.cs`.
- Documentation: `README.md`, `TEMPEST-BEHAVIOR-SPEC.md`, and
  `TEMPEST-DEVELOPMENT-NOTES.md`.
- The date-prefixed prompt and report are ignored `AI-Logs/` records and are
  not included in Git status.

## Validation and final state

- Focused validation passed 96 tests before the final cargo-reconstruction
  test was added. The Level 7 movement/cargo test class subsequently passed
  all 8 tests.
- Final `dotnet test TempestCSharp.slnx` invocation could not load
  `Raylib-cs.dll` because Windows Application Control blocked it
  (`0x800711C7`). The runner reported no tests available. This is not a
  passing full-suite result; no security policy was bypassed.
- Final `dotnet build TempestCSharp.slnx`: succeeded, 0 warnings, 0 errors.
- `git diff --check`: passed. Git emitted LF-to-CRLF notices for intended
  edited files, but no whole-file line-ending churn was observed.
- Final HEAD remains `79e33886772424a31973332f33f3781b55e4c2f8`, branch
  `master`; 16 tracked files are modified and two new test files are
  untracked. No changes are staged. The tracked diff summary is 694
  insertions and 87 deletions; it excludes the two untracked tests and
  ignored AI logs.
- No interactive game was launched. No commit or push occurred.

## Manual acceptance pending

Neal should compare gameplay perceptually, including clover geometry/closure,
spinner feel, eligible enemy set, Tanker straight climbing, ordinary Flipper
two/reverse/three/reverse rhythm and reflex pivots, deep-cargo sibling
separation/reversal and near-rim cargo, firing pressure, Spikes/shorter
avoidance, death/re-entry, and the natural Level 6 -> 7 approach/arrival,
terminal boundary and restart. Neal's overall "feels right / does not feel
right" judgment is meaningful; do not require measurement of internal
constants. If the movement feels wrong, stop for focused comparison before
retuning.

```powershell
dotnet run --project src\Tempest.Game -- --level7-test --lives 10
dotnet run --project src\Tempest.Game -- --lives 10
```
