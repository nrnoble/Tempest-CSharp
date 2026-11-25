# Level 15 production promotion closeout

## Status

**CLOSEOUT PASS — manual campaign-integration acceptance, fresh automated
validation, staged review, commit, push and independent GitHub verification.**
This report's post-push update is intentionally left for the next normal
commit rather than amending this one.

Neal reports: **Level 15 production promotion passes manual testing.**
Level 14 completes naturally into Level 15; Level 15 presentation and
configuration activate correctly; it otherwise behaves like the already
accepted direct-start version; no further promotion work is required. This is
campaign-integration acceptance, not new gameplay-fidelity evidence.

## Audit and scope

- Starting branch `master`; HEAD `bff70ae3faa50c8bb85f9ccde0c6d5ef4f048e6c`;
  upstream `origin/master`; ahead/behind `0/0`; index empty.
- Promotion source, tests, README and durable documentation matched the
  implementation handoff. Its prompt/report pair was untracked. The earlier
  Level 15 final-closeout prompt/report were untracked post-push records and
  are included unchanged in this normal commit. Five older Level 15
  investigation/correction prompt/report pairs were untracked historical
  records; they remain untouched and excluded.
- The background-opacity closeout report update was already committed by
  `bff70ae`, so no further update was pending.
- `Reference/arcade/MBUCOD.V05.txt`, `temo/`, ROMs, TEMP, UserData, videos,
  recordings, screenshots, build output and personal scratch material were
  not staged or modified.

## Accepted behavior

Production chain: `9 -> 10 -> 11 -> 12 -> 13 -> 14 -> 15`; Level 15 is
terminal while Level 16 is unimplemented. Both startup selectors use the
accepted production identity; existing score/lives carry, Super Zapper
transition, Approach, HUD, calibration and recording/playback are preserved.
No game was launched and no Level 16 work was done.

The accepted calibration remains Y gain `1.15`, scale `1.0`, vertical offset
`75`, depth-Y gain `1.0`, with development overrides. Raw starting Spike
`$A0` is ROM-backed; approximately `142.857` C# height is the accepted project
translation. Terminal Spiker conversion to the same-identity Tanker with two
owned cargo identities is the accepted project translation. Neither was
changed. Background opacity and generic ExitZoom remain unchanged; its known
presentation imperfection remains non-blocking.

The promotion handoff described two seeded 1,200-tick games; inspection showed
both used seed 1981. To satisfy the explicit two-seed closeout requirement,
the existing soak was parameterized with seeds 1981 and 1982, preserving two
identical runs per seed for deterministic comparison, all invariant checks,
and the correct ordinary-Flipper ledger assertion. No gameplay changed.

## Fresh validation

VSTest/xUnit v2, .NET SDK 9.0.304, Release, `--no-restore`; tests use a
two-minute per-test hang bound and each soak run is limited to 1,200 ticks.

- **PASS:** focused matrix, 1,438 passed, 0 failed, 0 skipped:
  `dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --configuration Release --no-restore --filter 'FullyQualifiedName~Level15|FullyQualifiedName~Level14|FullyQualifiedName~Level13|FullyQualifiedName~Level12|FullyQualifiedName~Level11|FullyQualifiedName~Level10|FullyQualifiedName~Level9|FullyQualifiedName~Level8|FullyQualifiedName~Level7|FullyQualifiedName~Level6|FullyQualifiedName~Level5|FullyQualifiedName~Level4|FullyQualifiedName~Level3|FullyQualifiedName~Level2|FullyQualifiedName~Level1|FullyQualifiedName~LevelProgression|FullyQualifiedName~StartLevel|FullyQualifiedName~DirectLevelStartup|FullyQualifiedName~DebugController|FullyQualifiedName~Flipper|FullyQualifiedName~Tanker|FullyQualifiedName~Spiker|FullyQualifiedName~Fuseball|FullyQualifiedName~Atari|FullyQualifiedName~Hud|FullyQualifiedName~BackgroundOpacity|FullyQualifiedName~Recording|FullyQualifiedName~Playback|FullyQualifiedName~Tube' --blame-hang-timeout 2min --verbosity minimal`.
  This covers startup parsing/initialization without graphical launch,
  progression, transition, carry, HUD, calibration, Spike, picker, enemy
  regressions, recording/playback, opacity, presentation and topology.
- **PASS:** complete solution Release suite, 1,974 passed, 0 failed, 0 skipped:
  `dotnet test TempestCSharp.slnx --configuration Release --no-restore --blame-hang-timeout 2min --verbosity minimal`.
- **PASS:** `dotnet build TempestCSharp.slnx --configuration Release --no-restore --verbosity minimal`:
  0 warnings, 0 errors.
- **PASS:** `git diff --check`; Git emitted working-copy LF-to-CRLF warnings
  for README and LevelLoader, not whitespace errors.
- **PASS:** `git diff --cached --check`; staged source, test, documentation
  and record diffs were reviewed. The 32 paths below were the exact staged
  inventory; no protected or unrelated paths were staged.

## Committed inventory

- `AI-Logs/Prompts/2026-11-24-level15-final-closeout-prompt.md`
- `AI-Logs/Prompts/2026-11-24-level15-production-promotion-closeout-prompt.md`
- `AI-Logs/Prompts/2026-11-24-level15-production-promotion-prompt.md`
- `AI-Logs/Reports/2026-11-24-level15-final-closeout-report.md`
- `AI-Logs/Reports/2026-11-24-level15-production-promotion-closeout-report.md`
- `AI-Logs/Reports/2026-11-24-level15-production-promotion-report.md`
- `README.md`
- `TEMPEST-BEHAVIOR-SPEC.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `src/Tempest.Game/Debug/DebugController.cs`
- `src/Tempest.Game/Debug/GameplayStartupConfiguration.cs`
- `src/Tempest.Game/Levels/AtariTubeRecordCatalog.cs`
- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/Levels/LevelLoader.cs`
- `src/Tempest.Game/Levels/LevelProgression.cs`
- `src/Tempest.Game/Levels/RomPresentationMetadata.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Rendering/AtariPresentationCalibration.cs`
- `src/Tempest.Game/TempestGame.cs`
- `tests/Tempest.Game.Tests/BackgroundOpacityOptionsTests.cs`
- `tests/Tempest.Game.Tests/DebugControllerTests.cs`
- `tests/Tempest.Game.Tests/DirectLevelStartupTests.cs`
- `tests/Tempest.Game.Tests/GameplayRecordingTests.cs`
- `tests/Tempest.Game.Tests/Level12ProductionTests.cs`
- `tests/Tempest.Game.Tests/Level13ProductionTests.cs`
- `tests/Tempest.Game.Tests/Level15ProductionSoakTests.cs`
- `tests/Tempest.Game.Tests/Level15TerminalSpikerTests.cs`
- `tests/Tempest.Game.Tests/Level15TubeCandidateTests.cs`
- `tests/Tempest.Game.Tests/Level8ProductionTests.cs`
- `tests/Tempest.Game.Tests/LevelConfigurationTests.cs`
- `tests/Tempest.Game.Tests/StartLevelTests.cs`
- `tests/Tempest.Game.Tests/TubeDepthYGainTests.cs`

## Commit and publication

- **PASS:** commit `63d37adb6c0b6b4f28a422f0c6c638cf5391e88c`, subject
  `Promote accepted Level 15 to production`, including the Copilot co-author
  trailer.
- **PASS:** normal push to `origin/master`: `bff70ae..63d37ad`.
- **PASS:** GitHub connector independently returned that SHA and subject for
  remote `master`.
- **PASS:** local `master` and `origin/master` resolve to the same SHA;
  ahead/behind `0/0`; index empty.
- Final tracked worktree: only this intentional post-push report update
  remains unstaged. The five earlier historical prompt/report pairs and the
  unrelated `Reference/arcade/MBUCOD.V05.txt` and `temo/` remain untracked and
  untouched.
