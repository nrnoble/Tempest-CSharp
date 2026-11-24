# Final Level 15 closeout

## Status

**CLOSEOUT PASS.** Neal's manual acceptance, fresh automated validation,
Release build, staged review, commit, push, and independent GitHub verification
all completed. Level 15 remains direct-start/development-only; no Level 16 work
was performed.

## Starting audit

- Branch: `master`
- HEAD: `045aeb240fce91a3eaacf9424da30cd7359927bf`
- Upstream: `origin/master`
- Ahead/behind: `0/0`
- Index: empty
- Independent GitHub `master` check: PASS; returned
  `045aeb240fce91a3eaacf9424da30cd7359927bf`, subject
  `Add background overlay calibration`

Starting tracked worktree changes:

- `AI-Logs/Reports/2026-11-24-background-opacity-closeout-report.md`
- `TEMPEST-BEHAVIOR-SPEC.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/Levels/RomPresentationMetadata.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`
- `src/Tempest.Game/Rendering/AtariPresentationCalibration.cs`
- `src/Tempest.Game/TempestGame.cs`
- `tests/Tempest.Game.Tests/AtariPresentationCalibrationTests.cs`
- `tests/Tempest.Game.Tests/Level15TubeCandidateTests.cs`

The accumulated Level 15 terminal test,
`tests/Tempest.Game.Tests/Level15TerminalSpikerTests.cs`, was initially
untracked and belongs to this candidate. Initial untracked historical Level 15
records, all left unstaged, were:

- Prompts: `AI-Logs/Prompts/2026-11-24-level15-baseline-spike-reconciliation-prompt.md`,
  `AI-Logs/Prompts/2026-11-24-level15-spike-arcade-evidence-correction-prompt.md`,
  `AI-Logs/Prompts/2026-11-24-level15-spike-height-correction-prompt.md`,
  `AI-Logs/Prompts/2026-11-24-level15-terminal-spiker-readonly-prompt.md`,
  and `AI-Logs/Prompts/2026-11-24-level15-terminal-spiker-correction-prompt.md`.
- Reports: `AI-Logs/Reports/2026-11-24-level15-baseline-spike-reconciliation-report.md`,
  `AI-Logs/Reports/2026-11-24-level15-spike-arcade-evidence-correction-report.md`,
  `AI-Logs/Reports/2026-11-24-level15-spike-height-correction-report.md`,
  `AI-Logs/Reports/2026-11-24-level15-terminal-spiker-readonly-report.md`,
  and `AI-Logs/Reports/2026-11-24-level15-terminal-spiker-correction-report.md`.

These are historical records and were not rewritten. `Reference/arcade/MBUCOD.V05.txt` and
`temo/gameplayrecordings/level-13` were identified as unrelated local
reference/recording material; neither was opened, changed, or staged.

No staged changes or unexpected ownership ambiguity were found. The existing
background-opacity report was the prior accepted milestone's post-push update
and was intentionally included as permitted for this commit.

## Manual acceptance (Neal)

Neal reports that Level 15 gameplay passes. The accepted direct-start
presentation is frozen at Y gain `1.15`, tube scale `1`, vertical offset `75`,
and depth-Y gain `1`.

The corrected starting Spikes are manually accepted: Neal reports, **“The
Spikes are fixed.”** The ROM-backed starting-Spike value is raw `$A0` for
Level 15. The C# height uses the accepted project coordinate translation
`(0xF0 - raw) * Level.Depth / 224`, yielding approximately `142.857` for
`Level.Depth = 400`; the raw ROM coordinate is not a literal rendered pixel
height. The prior Level-11-derived `$B0` expression was not restored.

The terminal-Spiker correction is manually accepted: Neal reports, **“The
terminal-Spiker bug is fixed.”** The read-only investigation classified the
cycling as Level 15 not being wired into the existing terminal lifecycle,
not as a demonstrated regression. With generic pending originals, the same
Spiker identity restarts. At zero pending originals, that identity converts
in place to a Tanker through the existing ledger conversion path, with exactly
two distinct, owned, latent cargo identities. Ordinary Tanker-selection
capacity and unrelated active enemies do not block conversion. The unresolved
Tanker and cargo obligations continue to block normal and all-rim completion.

These are Neal's manual results; automated tests below do not establish
arcade-fidelity acceptance or broader arcade perfection.

## Durable documentation and scope

Updated `TEMPEST-BEHAVIOR-SPEC.md` and `TEMPEST-DEVELOPMENT-NOTES.md` to record
the final acceptance, ROM evidence versus project translation, frozen
calibration, terminal-lifecycle classification and semantics, and direct-only
campaign boundary. Historical AI reports were left unchanged.

Normal production progression still ends at Level 14. Level 15 remains a
direct-start/development-only candidate and has not been promoted into the
campaign. No Level 16 work or acceptance claim is included.

## Fresh automated validation

**PASS — focused Release matrix:** 636 passed, 0 failed, 0 skipped. The
bounded VSTest command covered the Level 15 tube candidate and identity-based
terminal-Spiker tests; presentation calibration; Level 11 and production
Levels 12-14; Level 3 ledger/mixed-wave behavior; Level 4-9 regressions;
all-rim/reconstruction; playback, headless, and direct-start tests.

```powershell
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --configuration Release --no-restore --filter "FullyQualifiedName~Level15TerminalSpikerTests|FullyQualifiedName~Level15TubeCandidateTests|FullyQualifiedName~AtariPresentationCalibrationTests|FullyQualifiedName~Level11ProductionCorrectionTests|FullyQualifiedName~Level3WaveLedgerTests|FullyQualifiedName~Level3MixedWaveTests|FullyQualifiedName~Level3MixedAdmissionPolicyTests|FullyQualifiedName~Level4|FullyQualifiedName~Level5|FullyQualifiedName~Level6|FullyQualifiedName~Level7|FullyQualifiedName~Level8|FullyQualifiedName~Level9|FullyQualifiedName~Level12ProductionTests|FullyQualifiedName~Level13ProductionTests|FullyQualifiedName~Level14ProductionTests|FullyQualifiedName~AllRim|FullyQualifiedName~Reconstruct|FullyQualifiedName~Playback|FullyQualifiedName~Headless|FullyQualifiedName~DirectStart" --blame-hang-timeout 5min --verbosity minimal
```

The identity-specific tests directly verify pending-positive same-Spiker
restart; zero-pending same-ID Tanker conversion; exactly two distinct cargo
IDs owned by that identity; conversion at ordinary Tanker capacity and with
unrelated active enemies; and unresolved completion/all-rim obligations.
The broad headless test was not treated as proof of terminal conversion.

**PASS — complete Release solution suite:** 1,966 passed, 0 failed, 0 skipped.

```powershell
dotnet test TempestCSharp.slnx --configuration Release --no-restore --blame-hang-timeout 5min --verbosity minimal
```

**PASS — fresh Release solution build:** 0 warnings, 0 errors.

```powershell
dotnet build TempestCSharp.slnx --configuration Release --no-restore --verbosity minimal
```

**PASS — `git diff --check`** before staging.
**PASS — `git diff --cached --check`** after staged review.
No validation was canceled, timed out, blocked, skipped, or left unrun.
The graphical game was not launched.

## Committed inventory and publication

Staged review confirmed the accepted Y gain `1.15`, scale `1`, vertical offset
`75`, depth-Y gain `1`, raw `$A0` correction, terminal-Spiker correction,
direct-start status, and absence of protected/unrelated files or Level 16 work.
Exactly these 12 paths were committed:

- `AI-Logs/Reports/2026-11-24-background-opacity-closeout-report.md`
- `TEMPEST-BEHAVIOR-SPEC.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/Levels/RomPresentationMetadata.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`
- `src/Tempest.Game/Rendering/AtariPresentationCalibration.cs`
- `src/Tempest.Game/TempestGame.cs`
- `tests/Tempest.Game.Tests/AtariPresentationCalibrationTests.cs`
- `tests/Tempest.Game.Tests/Level15TerminalSpikerTests.cs`
- `tests/Tempest.Game.Tests/Level15TubeCandidateTests.cs`

Commit subject: `Complete accepted Level 15`

Commit SHA: `bff70ae3faa50c8bb85f9ccde0c6d5ef4f048e6c`

Push: **PASS**, normal push to `origin/master`:
`045aeb2..bff70ae master -> master`.

Post-push local verification: branch `master`; local HEAD and `origin/master`
both resolve to `bff70ae3faa50c8bb85f9ccde0c6d5ef4f048e6c`; ahead/behind `0/0`;
index empty; commit subject matches.

Independent GitHub connector verification: **PASS**. `master` returned SHA
`bff70ae3faa50c8bb85f9ccde0c6d5ef4f048e6c` and subject
`Complete accepted Level 15`, matching the local published commit.

This final report and its paired prompt,
`AI-Logs/Prompts/2026-11-24-level15-final-closeout-prompt.md`, are post-push
closeout records and remain untracked/unstaged. They record the final SHA
without amending the commit or creating a report/commit loop. Previously
untracked Level 15 history, the MBUCOD reference, and the `temo` recording
material remain excluded and untouched.
