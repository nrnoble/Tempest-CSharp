# Level 12 ExitZoom candidate restoration and deferral

## Status

**RESTORED — Level 12 now uses the pre-candidate generic ExitZoom path.**
Neal's manual rejection is authoritative. No interactive game was launched,
and no screenshots or other visual assets were inspected.

## Restoration

- Removed the untracked candidate files
  `src/Tempest.Game/Rendering/Level12ExitZoomCamera.cs` and
  `tests/Tempest.Game.Tests/Level12ExitZoomCameraTests.cs`.
- Removed the Level-12-only dispatch, transform cache, and coherent glyph
  transform from `VectorRenderer`.
- Removed the candidate-only Level 11 and direct Level 15 preservation cases
  from `ExitZoomFocalCompensationTests`; all pre-candidate generic focal tests
  remain unchanged.
- Verified that `VectorRenderer.cs` and `ExitZoomFocalCompensationTests.cs`
  exactly match `HEAD`, which is their pre-candidate state. Generic focal
  compensation was not weakened.
- No Level 12 production source or tests were edited by this restoration.
  The genuine Level 12 definition, closed 16-lane topology, recovered
  geometry, ROM metadata, model-frame mapping, wave policy, progression,
  HUD/Atari presentation, neutral calibration, topology/model-frame tests,
  and bounded production soak remain present. The 1.3/0.8/0 calibration
  remains manual evidence and was not encoded or changed.

## Deferred issue

**Level 12 ExitZoom presentation discrepancy remains open/deferred.**

- Existing generic ExitZoom is visually imperfect on Level 12.
- The geometry-derived Level-12-only camera was manually rejected as worse.
- No further correction was attempted in this milestone.
- The discrepancy is presentation-only and does not block Level 12 gameplay.
- Future work may revisit ExitZoom only if a materially better architectural
  approach emerges.

The rejected camera is not accepted behavior. The accepted Levels 1-11 and
other protected work were not reopened or changed. No durable behavior-spec
changes were necessary.

## Validation

- **PASS** — bounded focused restoration regressions: 571 passed, 0 failed.
  Covered generic ExitZoom, Level 12 production/topology/model-frame/HUD/
  progression/spinner/soak, presentation regressions, Level 15, Fuseball,
  Blaster death, input, and playback/reverse-playback tests.
- **FAIL** — complete solution suite completed: 1,821 passed, 1 failed,
  1,822 total. The failure was
  `Level11ProductionCorrectionTests.LevelElevenTerminalSpikerConvertsInPlaceWithTwoCargoAndNoPendingLoss`
  at `Level11ProductionCorrectionTests.cs:85` (`Assert.DoesNotContain`
  matched an existing Spiker). This gameplay test is outside the restoration
  changes and was not modified or repaired here.
- **PASS** — Release solution build: 0 warnings, 0 errors.
- **PASS** — `git diff --check`. Git reported its existing LF-to-CRLF notice
  for `src/Tempest.Game/Levels/LevelLoader.cs`.
- **PASS** — candidate camera files are absent and no candidate symbols
  remain in production or test source.
- **PASS** — generic renderer and focal-compensation tests exactly match
  their pre-candidate `HEAD` versions.
- No staging, commit, push, reset, restore, stash, clean, or rebase was done.
  All unrelated pre-existing tracked and untracked work was preserved.

The complete-suite failure remains unresolved; no further ExitZoom
investigation or correction was started. Manual gameplay acceptance is not
requested for this restoration because Neal's manual rejection already
determined the outcome.

**LEVEL 12 EXITZOOM CANDIDATE REJECTED — PRE-CANDIDATE EXITZOOM RESTORED; DISCREPANCY DEFERRED**
