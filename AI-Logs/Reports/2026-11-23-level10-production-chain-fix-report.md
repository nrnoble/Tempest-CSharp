# Level 10 production-chain integration candidate

Date: 2026-11-23

## Status and scope

Automated validation PASS. Manual acceptance of startup and progression
integration remains pending. The game was not launched interactively; no
commit, push, or staging was performed.

## Root cause and historical structure

The authoritative `LevelProgression.GetNextProductionLevel` skipped from
Level 9 to Level 11. Production lookup and `--start-level` traverse that
chain, so they rejected Level 10 even though `--level 10` had a separate
direct-start mapping.

The direct-only configuration was named `Level10Diagnostic` and derived from
Level 7, with Level 10's open geometry and a 15-lane spawn sequence. Its
Level 7 wave-policy identity was explicitly recognized, while Level 10
restart had special diagnostic handling. Tests and historical documentation
also encoded its prior exclusion. This was an older development boundary,
not a technical barrier to using the same configuration in the campaign.

Historical acceptance records report Neal's acceptance of the candidate's
placement and gameplay, but do not establish authentic Level 10 arcade
mechanics. The preserved Level-7-like gameplay is a PROJECT TRANSLATION;
the geometry and raw presentation metadata have ROM-backed provenance.
This correction does not change those evidence classifications or claim new
manual acceptance.

## Implementation

- Renamed the existing configuration to `Level10Production`, preserving its
  geometry, wave policy, enemies, simulation parameters, and presentation.
  Corrected its inherited progression and board identity fields from 6 to 9;
  neither field is used by production simulation or projection.
- Changed the authoritative edges to 9 -> 10 -> 11, retaining 11 -> 12.
  Direct production startup continues to derive eligibility from that chain;
  the independent Level 10 mapping was removed. Level 15 stays direct-only.
- Removed obsolete Level 10 restart exceptions. Game Over after a production
  Level 10 start now follows the standard fresh-Level-1 campaign rule.
- Updated current comments, startup errors, README, and historical-scope
  notices in the durable documents. Historical closeouts remain historical;
  manual integration acceptance is not asserted.
- Updated progression and startup tests, including all 1-12 startup eligibility,
  Level 15 and unimplemented rejection, both direct selectors, Level 10
  recording/playback identity, and bounded Level 9 -> 10 and 10 -> 11
  transitions. Existing diagnostic and presentation checks now use the same
  canonical production configuration.

The change necessarily affects existing Level 10 recordings that play past a
Level 10 completion or Game Over: they now follow the corrected campaign
progression/restart rule instead of the former diagnostic boundary. Recording
format, seed handling, and in-level mechanics were not changed.

## Changed files

- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/Levels/LevelProgression.cs`
- `src/Tempest.Game/TempestGame.cs`
- `src/Tempest.Game/Debug/DebugController.cs`
- `src/Tempest.Game/Debug/GameplayStartupConfiguration.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`
- `tests/Tempest.Game.Tests/StartLevelTests.cs` (pre-existing untracked file)
- `tests/Tempest.Game.Tests/DebugControllerTests.cs`
- `tests/Tempest.Game.Tests/DiagnosticLevel10Tests.cs`
- `tests/Tempest.Game.Tests/DirectLevelStartupTests.cs`
- `tests/Tempest.Game.Tests/Level8ProductionTests.cs`
- `tests/Tempest.Game.Tests/Level9ProductionTests.cs`
- `tests/Tempest.Game.Tests/LevelConfigurationTests.cs`
- `tests/Tempest.Game.Tests/LevelProgressionTests.cs`
- `tests/Tempest.Game.Tests/RimPursuitTests.cs`
- `tests/Tempest.Game.Tests/RomPresentationMetadataTests.cs`
- `README.md`, `TEMPEST-BEHAVIOR-SPEC.md`, `TEMPEST-DEVELOPMENT-NOTES.md`
- Paired prompt and this report in `AI-Logs`.

The starting worktree included the unstaged `--start-level` candidate and
other unrelated tracked and untracked work. It was preserved; the new work was
integrated into relevant already-modified files without staging anything.

## Automated validation

- PASS: focused startup/progression run, 71 tests.
- Initial broader targeted run: FAIL, four assertions expected the old
  `--level` error wording. Updated them to assert the current message.
- PASS: targeted startup, progression, Level 10/11/12, recording/playback,
  Atari-presentation, and campaign coverage, 412 tests.
- Initial Release solution suite: FAIL, four existing tests expected the old
  9 -> 11 edge. Updated those assertions to the required 9 -> 10 -> 11 edges.
- PASS: `dotnet test TempestCSharp.slnx --configuration Release --no-restore --blame-hang-timeout 5min`
  — 1,852 passed, zero failed or skipped.
- PASS: `dotnet build TempestCSharp.slnx --configuration Release --no-restore`
  — zero warnings and errors.
- PASS: `git diff --check` (Git emitted its existing README LF/CRLF warning).

## Manual handoff and unresolved issues

Run both:

```powershell
dotnet run --project src\Tempest.Game -- --start-level 10
dotnet run --project src\Tempest.Game -- --start-level 10 --atari-presentation
```

For the practical handoff check, start Level 10 with extra lives and play
through its ordinary exit, confirming the Level 11 approach, HUD, and carried
score/lives:

```powershell
dotnet run --project src\Tempest.Game -- --start-level 10 --lives 10 --atari-presentation
```

Optionally start Level 9 similarly to check the natural 9 -> 10 boundary.
These checks do not require changing production behavior. Manual acceptance
of these integration paths remains pending; automated tests do not establish
arcade fidelity. No technical blocker was found.
