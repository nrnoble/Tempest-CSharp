# Start-level continuing campaign: implementation candidate

Status: automated validation PASS; manual acceptance PENDING.

## Scope and decision

Added `--start-level N` as a fresh, production-only continuing start. Eligibility
uses the existing production progression chain, not a separate list.
Diagnostic Level 10, direct-only Level 15, and unimplemented levels are
rejected. The current chain goes from Level 11 to terminal Level 12.

The original request assumed `--level` was terminal. Investigation established
that accepted production `--level` starts already continue normally. Neal chose
to preserve existing `--level` progression and add `--start-level` as a
production-only continuing option. Therefore the two options do not differ
in production progression semantics; `--levelN-test` scenarios remain the
separate terminal diagnostics. The earlier investigation report records the
conflict and evidence.

Existing startup, score, lives, random stream, wave, Super Zapper, calibration,
HUD, and progression logic are reused. Existing production restart behavior
remains unchanged: R after Game Over or a terminal production level resets
score and starting lives and launches a fresh Level 1 campaign. Keeping this
rule also preserves recording/playback identity without altering the format.

File recording accepts the new selector and stores the existing startup
metadata. File playback and headless playback reject it; conflicting selectors,
invalid values, built-in playback/scenario combinations, and input diagnostic
are rejected before window creation. Explicit presentation calibration
overrides retain their existing cross-level behavior.

## Changed files

- `src/Tempest.Game/Debug/DebugController.cs`: shared startup parsing,
  production eligibility, file-mode and presentation option compatibility.
- `src/Tempest.Game/Program.cs`: scenario conflict/error handling and
  input-diagnostic rejection.
- `tests/Tempest.Game.Tests/StartLevelTests.cs`: parsing, startup, recording,
  deterministic headless production transitions, HUD/calibration, and restart.
- `tests/Tempest.Game.Tests/TubeYGainTests.cs`: updated the existing expected
  error to include the new compatible selector.
- `README.md`: documented current chain, new option and actual restart rule.
- Paired prompt and this report in `AI-Logs`; no existing record was rewritten.

## Validation

- PASS: focused start-level tests (initial set: 8 tests).
- PASS: combined startup, progression, recording/playback, Atari presentation,
  HUD and Levels 1-12 regression selection: 854 tests.
- PASS: complete solution suite: 1,838 tests, zero failures or skips, using
  `dotnet test TempestCSharp.slnx --no-restore --blame-hang-timeout 2min`.
  The first full run had one failure because an existing error-message
  assertion still expected the old list of compatible options. Its expected
  message was updated and the full suite rerun successfully.
- PASS: `dotnet build TempestCSharp.slnx -c Release --no-restore`, zero
  warnings and errors.
- PASS: real noninteractive command-line checks for missing/diagnostic level,
  conflicting selectors, file playback, headless playback, and input
  diagnostic; all returned exit code 2 before creating a window.
- PASS: `git diff --check`.

The production transition test uses a bounded synthetic ledger-clear seam,
then executes the actual Level 11 exit, Level 12 approach, and Level 12 terminal
exit. It verifies score/lives carry, active HUD and calibration, and restart.
It does not establish manual gameplay or arcade-fidelity acceptance.

## Manual handoff

Do not launch the game on Neal's behalf. Neal may run:

```powershell
dotnet run --project src\Tempest.Game -- --level 11
dotnet run --project src\Tempest.Game -- --start-level 11
dotnet run --project src\Tempest.Game -- --start-level 11 --atari-presentation
dotnet run --project src\Tempest.Game -- --start-level 12
```

Verify normal fresh starts, Level 11 to 12 progression and score/life carry,
Level 12 terminal behavior, Atari presentation, and unchanged `--level`
continuation. Manual acceptance is pending. No Level 13 work, staging, commit,
or push was performed. The pre-existing Level 12 closeout-report change and
other unrelated tracked/untracked material were left untouched.
