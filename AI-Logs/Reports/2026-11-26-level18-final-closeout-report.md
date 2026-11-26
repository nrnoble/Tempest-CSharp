# Level 18 Final Closeout Report

Date: 2026-11-26

## 1. Executive summary

Final closeout reconciles the accepted Level 18 candidate against the ROM,
Pulsar, and production implementation authorities. Neal has manually accepted
the medium/default Level 18 milestone. Automated final validation, Git staging,
commit/push, and independent GitHub verification are recorded below.

## 2. Starting Git/worktree state

- Branch: `master`
- HEAD before closeout: `0ce7e538374c4e155b75cf536b4f3a67c8da3de0`
- Upstream: `origin/master`
- Index at start: clean
- Tracked modifications: Level 18 production/test implementation files
- Untracked: Level 18 AI-Logs, `Level18WaveSelection.cs`,
  `Level18ProductionTests.cs`, and unrelated
  `Reference/arcade/MBUCOD.V05.txt`
- Ignored protected directories: `TEMP/`, `UserData/`, `Videos/`,
  `Reference/arcade/roms.rev2/`, and build output

No unrelated work was cleaned, reset, stashed, deleted, or modified.

## 3. Manual acceptance

**MANUAL ACCEPTANCE - LEVEL 18**

Neal reports that Level 18 passes manual gameplay and presentation testing,
with no new problems and no regressions. Accepted scope includes Shape 2
presentation, cycle-1 palette, HUD, Flippers, Tankers, cargo, Pulsars, enemy
shooting pressure, death/re-entry, natural Level 17 to 18 progression, direct
startup, and terminal behavior. Easy/hard variants were not accepted.

## 4-28. Final implementation reconciliation

Level 18 is displayed 18 with progression index 17, ROM `curlevel=$11`, effective
`lev=$12`, cycle-local position 2, remap index 1, ROM shape row 1, board index
1, and palette index 1. It reuses canonical Level 2 Shape 2 geometry and
Level 2 production projection/calibration. It uses cycle-1 palette roles,
21 original supply, seven physical slots, 4 initial Eggs, Flippers 0-3,
Tanker exactly 1, Pulsars 2-5, no Spikers/Fuseballs/Spikes, and exactly two
Flipper cargo identities.

Ordinary and cargo Flippers share `EightAdvanceTurn` and generic rim pursuit.
Host Flipper/Tanker climb is 2.625; holdoff/capacity is 29/3. Medium Pulsars
do not fire. The shared Pulsar configuration has threshold approximately
392.92035, fixed 1 speed, and 21/11 deep-side toward-rim speed. Level 17
retains no-threshold fixed-1 compatibility. The threshold/pulse/collision
overlap remains a documented project translation boundary.

Natural progression enters Level 18 by Approach after Level 17 ExitZoom.
Direct 18 startup resolves to the same configuration. Level 18 is terminal;
Level 19 remains unsupported.

The `lvl-17-test.json` recording now naturally continues into Level 18 and
ends with Level 18 `PlayerDying` telemetry as a consequence of supported
progression, not as a playback-only exception.

## 29-32. Files and documentation

Production and test files, requested durable documentation, and Level 18
AI-Logs are reconciled as this closeout. The implementation report remains the
historical record of pre-closeout automated validation.

## 33-41. Fresh validation

**PASS - Level 18 focused and shared regressions:** 575 passed, 0 failed,
0 skipped, with explicit VSTest 5-minute hang timeout.

**PASS - Levels 1-16 recording:** unmodified
`TEMP/GameplayRecordings/lvl1-16.json` completed all 35,350 frames, remaining
frames 0, and returned `RecordingCompleted`. Final telemetry: displayed Level
17, Playing, score 134,039, 9 lives, no active enemies.

**PASS - Level 17-origin recording:** unmodified
`TEMP/GameplayRecordings/lvl-17-test.json` completed all 2,609 frames,
remaining frames 0, and returned `RecordingCompleted`. Final telemetry:
displayed Level 18, PlayerDying, score 8,175, 8 lives, 5 Pulsars, 1
Tanker, 1 active cargo Flipper, 1 enemy bullet. This is the newly valid
natural Level 17 to 18 result.

**PASS - full solution:** 2,102 passed, 0 failed, 0 skipped, with explicit
VSTest 5-minute hang timeout.

**PASS - fresh Release build:** non-incremental `dotnet build
TempestCSharp.slnx --configuration Release --no-incremental -v minimal`
succeeded with 0 warnings and 0 errors.

The Level 15 seed-1982 and shared semantic regressions were included in the
focused selection and passed. No recording or protected asset was modified.

## 42. Whitespace

**PASS:** unstaged `git diff --check` passed, and staged
`git diff --cached --check` passed after the selective Level 18 staging.

## 43-51. Git and closeout

Selective staging included 34 intended files: production source, tests,
durable documentation, four prompt/report pairs, and the closeout prompt/report.
Protected recordings, screenshots, videos, ROM/reference assets, TEMP material,
generated output, `Reference/arcade/MBUCOD.V05.txt`, and unrelated state remain
untracked or unstaged.

Primary commit, push, independent GitHub verification, optional
report-finalization commit, and post-push repository state remain pending.

## Evidence classifications

- **ROM-BACKED:** Level 18 mapping, eligibility, wave/speed/firing data,
  Shape 2 row, palette block, and absence of Spikers/Fuseballs/Spikes.
- **MANUAL ACCEPTANCE:** gameplay and Shape 2/cycle-1 presentation accepted by
  Neal for medium/default Level 18.
- **PROJECT TRANSLATION:** coordinate threshold and speed mapping, shared
  fixed-step actor behavior, pulse/collision boundary interaction.
- **PROJECT DECISION:** canonical Shape 2/palette reuse and terminal Level 18
  campaign policy.
