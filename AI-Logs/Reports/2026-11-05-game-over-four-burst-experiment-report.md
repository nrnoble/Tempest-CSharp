# Four-Burst Game Over Experiment Report

**Status:** Implemented; awaiting Neal's manual visual comparison. The stale
Spike reverse-zoom test wait is corrected; rebuilt-test validation is blocked
again by Windows Application Control.

## Changes

- Replaced the single Game Over effect owner with a sequence of four independent
  ROM-informed animations. Each animation retains its own 16 parent slots,
  movement, bounds, spawn/drain lifecycle and seeded random behavior.
- Started the instances at simulation ticks 0, 32, 64 and 96. Started
  instances continue advancing while later instances are introduced.
- Kept the shared `$80` starting coordinates; no unsupported screen-origin
  offsets or fixed per-burst colors were added.
- Added stable burst-index variation to the procedural speck arrangement.
  Existing mark length, palette cycle and per-burst density multiplier remain
  unchanged.
- Added tests for start intervals, overlap, per-instance capacity, repeatable
  but distinct parent patterns, and the increased mark count without a change
  in mark length. Updated Game Over lifecycle timeouts to accommodate the
  longer sequence.

## Evidence classification

- **ROM-backed:** State `$24` uses one 16-slot record loop, `$80` initial
  coordinates, a 64-tick spawn window, bounded movement and slot reuse;
  graphic `$1F` selects two nonzero vector subrecords, `$47` and `$4A`.
- **Not established by the ROM:** four event groups, their staggered timing,
  records per group, or four origins.
- **Arcade observation:** Neal perceives about four overlapping, structurally
  distinct events. A targeted clean-MAME sample of frames 0001, 0003-0008,
  0012, 0020, 0025, 0032, 0050, 0075, 0090 and 0103 shows a central,
  asymmetric spread but does not confirm four origins or a four-group mapping.
- **Project translation:** Four independent 16-parent animations start 32
  ticks apart, half the ROM-inspired spawn window. This timing and grouping
  are experimental choices, not ROM findings. The experiment is not accepted
  until manual comparison.

## Validation

- `dotnet build TempestCSharp.slnx` — passed; 0 warnings and 0 errors.
- `git diff --check` — passed.
- Focused Game Over tests — blocked before test execution by Windows
  Application Control. The test host reported `0x800711C7` while loading the
  generated `Tempest.Game.dll`; a subsequent invocation also skipped the test
  assembly under the same policy.
- Neighboring gameplay/restart regressions — skipped because Application
  Control blocked the test assembly.
- Full `TempestCSharp.slnx` test run — skipped for the same reason. No test
  bodies ran, so these runs provide no pass/fail evidence about behavior. The
  policy was not bypassed.
- The game was not launched. No files were staged, committed or pushed.

## Validation recovery and narrow Spike follow-up (2026-11-06)

The earlier missing test evidence was caused by Windows Application Control,
not observed test failures. Neal subsequently cleared the block sufficiently
for the suite to execute and reported:

- `dotnet test TempestCSharp.slnx`: 809 total, 808 passed, 1 failed,
  0 skipped.
- `dotnet build TempestCSharp.slnx`: succeeded, 0 warnings, 0 errors.
- `git diff --check`: passed, with LF-to-CRLF advisories only.

The sole failure was
`SimulationTests.SpikeReverseZoomKeepsDeathDepthUntilOffsetRestoresAndHandlesGameOver`.
An isolated run against the existing assembly reproduced it at line 4437:
1 total, 0 passed, 1 failed, 0 skipped.

### Cause and correction

This is a stale test deadline, not a production lifecycle defect. The test
waited 300 ticks from entry to `GameOverPresentation`, including the 120-tick
text phase, leaving only 180 animation ticks. `BeginSpikerExit` supplies
`SpikerRandom`, which always returns the minimum: records have zero initial
velocity on both bounded axes, so the existing non-stalling guard supplies
Y velocity -1. They need 113 movement updates after creation to leave the
bounded region. The final burst starts at animation tick 96 and therefore
still has live records at the old assertion. From the implementation, this
fixture needs 210 animation updates, or 330 ticks including the text phase.

The production transitions were read end to end:

- Ordinary death uses `PlayerDying` and `UpdatePlayerDeath`.
- Spike exit contact intentionally uses `ReverseZoom` and
  `UpdateReverseZoom`, not ordinary `PlayerDying`. It preserves collision depth
  through offset restoration, restores rim depth, and deducts the last life
  at the end of the established 60-tick death interval.
- Both final-death paths call `BeginGameOverPresentation`. After 120 ticks,
  it starts `GameOverAnimation`; the four independent bursts begin at ticks
  0, 32, 64 and 96. Terminal `GameOver` is entered only when all four bursts
  have started and all their records have drained.

Only the failing test was changed in this follow-up. It now checks the
reverse-zoom state, restored offset, retained life before deduction, restored
projection, text-to-animation boundary, and active four-burst intermediate
state. It then reuses the existing `FinishGameOverPresentation` state-based
helper (500-tick limit), checking completion of every burst and zero live
records before retaining the existing life-loss/restart assertions. The
limit is conservative: 96 ticks to the last burst, at most 64 spawning ticks,
and at most 113 movement updates for any record with a nonzero bounded-axis
velocity, plus the 120-tick text phase, fit within 500 ticks even from text
entry. No production code, visual design, density diagnostic, GAME OVER text,
or unrelated gameplay was changed.

### Post-edit validation limitation

Rebuilding the corrected test assembly renewed the Application Control block:
`Tempest.Game.Tests.dll` could not load (`0x800711C7`). A no-build retry and
another retry after requesting normal policy approval remained blocked.
The successful original-assembly execution above does not validate the edit.

- Corrected test alone: attempted; 0 tests executed, assembly loading skipped.
- Focused Game Over, death, Spike-exit and four-burst tests: attempted in one
  filtered run; 0 tests executed, assembly loading skipped. Filter:
  `FullyQualifiedName~GameOver|FullyQualifiedName~Death|FullyQualifiedName~Dying|FullyQualifiedName~SpikeExit|FullyQualifiedName~SpikeReverseZoom|FullyQualifiedName~SpikeTravel|FullyQualifiedName~SpikeWait|FullyQualifiedName~SpikeAvoid|FullyQualifiedName~ExitLaneControlTests|FullyQualifiedName~GameOverVectorBurstSequenceTests`.
- `dotnet test TempestCSharp.slnx`: attempted; 0 tests executed, assembly
  loading skipped. The runner returned exit code 0 but reported no tests
  available; this is not a pass and supplied no per-test skipped count.
- `dotnet build TempestCSharp.slnx`: succeeded, 0 warnings, 0 errors.
- `git diff --check`: passed; four LF-to-CRLF advisories, no whitespace errors.

All-tests-passing and zero-skipped acceptance remains unverified until the
rebuilt assembly is approved through the normal Windows policy process and
the requested isolated, focused and full runs execute. The policy was not
changed or bypassed. The game was not launched; nothing was staged, committed
or pushed. The pre-existing dirty tree was preserved: 14 tracked modified
files and four untracked files, with no staged changes. This follow-up adds
only the test edit and this report update; `AI-Logs/` is Git-ignored, so the
saved report does not appear in ordinary Git status.

## Manual comparison

Run from the repository root:

```powershell
dotnet run --project src\Tempest.Game -- --lives 1 --game-over-density 0.5
dotnet run --project src\Tempest.Game -- --lives 1 --game-over-density 1
dotnet run --project src\Tempest.Game -- --lives 1 --game-over-density 2
```

Compare the final-life sequence against the clean MAME capture:

1. Check whether four distinct procedural events begin rapidly, approximately
   halfway through the prior event's evolution, and visibly overlap.
2. Check that the events look different without suggesting unsupported
   separate origins or fixed color groups.
3. Decide whether density `0.5` preserves the preferred tiny-speck appearance
   once all four events overlap; densities `1` and `2` are comparison
   diagnostics, not accepted defaults.
4. Confirm marks remain short and gameplay stays isolated during the
   presentation; confirm R restart still works afterward.

Manual acceptance and any further tuning remain pending.
