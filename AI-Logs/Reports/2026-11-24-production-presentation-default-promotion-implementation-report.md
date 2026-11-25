# Tempest Accepted Atari Presentation — Production Default Promotion

**Status: AUTOMATED VALIDATION PASS — STOPPED FOR NEAL'S MANUAL ACCEPTANCE.**
The interactive game was not launched. Default-promotion acceptance is not
claimed.

## 1. Starting Git/repository state

- Branch: `master`.
- HEAD: `03346581b0fa3253bbb010022e8c617ce414d466`
  (`Complete Level 16 production and presentation`).
- Upstream: `origin/master`; ahead/behind `0/0`.
- Index: clean.
- Pre-existing tracked modifications: the Level 15 promotion closeout report
  and the Level 15 campaign-checkpoint wording hunk in
  `TEMPEST-BEHAVIOR-SPEC.md`. Both matched the unrelated work documented by
  the investigation report and were preserved.
- Pre-existing untracked state included the required investigation and Level
  16 closeout reports, earlier Level 15/16 AI-Logs, and
  `Reference/arcade/MBUCOD.V05.txt`. These were preserved; protected reference
  contents were not inspected.
- No implementation prompt/report or presentation-selection test existed at
  task start.

The complete required investigation report and Level 16 closeout report were
read. The checkpoint matched the requested accepted Level 16 commit.

## 2. Files changed

Implementation changes:

- `src/Tempest.Game/Debug/DebugController.cs`
- `src/Tempest.Game/Program.cs`
- `tests/Tempest.Game.Tests/PresentationSelectionTests.cs` (new)
- `tests/Tempest.Game.Tests/DebugControllerTests.cs`
- `tests/Tempest.Game.Tests/PortraitCalibrationGuideTests.cs`
- `tests/Tempest.Game.Tests/TubeDepthYGainTests.cs`
- `tests/Tempest.Game.Tests/TubeVerticalOffsetTests.cs`
- `tests/Tempest.Game.Tests/TubeYGainTests.cs`
- `README.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`

Required task records created:

- `AI-Logs/Prompts/2026-11-24-production-presentation-default-promotion-implementation-prompt.md`
- `AI-Logs/Reports/2026-11-24-production-presentation-default-promotion-implementation-report.md`

No presentation geometry, calibration-table values, gameplay, renderer
constructor/API, recording implementation, or durable behavior-spec content
was changed. The pre-existing `TEMPEST-BEHAVIOR-SPEC.md` modification was left
untouched.

## 3. Effective presentation-selection architecture

Added `PresentationSelectionKind` and `PresentationSelection` as the
centralized selection seam. It retains distinct kinds for the default Atari
portrait, the explicitly supplied Atari compatibility alias, the baseline
portrait diagnostic, legacy 4:3, headless playback, and input diagnostic.
Explicit alias presence remains represented separately from effective
selection; an omitted flag does not masquerade as an explicitly supplied
alias.

The application parses this selection before calibration controls and
non-rendering routing. Calibration parsers use the same seam to validate
against effective mode instead of testing for the literal Atari option.

## 4. Default Atari selection behavior

No-selector interactive gameplay now chooses the existing Atari projection
path and existing portrait viewport/camera. The normal Atari vector HUD,
per-level calibration resolver, model-frame and Level 5/6 special dispatch,
entity/tube projection, Approach, ExitZoom focal compensation, and Level 6
Game Over/vector-record old-projector exceptions remain in their existing
paths. No calibration values were recalculated or changed.

## 5. `--atari-presentation` compatibility behavior

The valueless option remains an explicit compatibility alias. Its effective
renderer mode and Atari projection boolean match omission, with the same
automatic per-level defaults. Duplicate, separate-value, joined-value, and
conflicting-selector forms reject. It is silently redundant when supplied;
normal launches emit no Atari-candidate warning.

## 6. `--baseline-portrait-presentation` behavior

The new valueless option selects the existing old portrait projection by
passing the existing non-Atari renderer path, while retaining the same
portrait viewport/camera. It is not the legacy 4:3 mode. Duplicate and valued
forms, Atari/legacy conflicts, non-rendering use, and incompatible Atari
calibration/guides reject. File-mode parsing permits the diagnostic with
interactive recording/playback.

## 7. Preserved `--legacy-presentation` 4:3 behavior

The existing option still selects `PresentationMode.Legacy` and the existing
4:3 camera/HUD path; baseline portrait is a distinct selection. Legacy
gameplay-file support was not added. Existing 4:3 projection/camera tests
passed.

## 8. `--portrait-perspective` handling

The numeric parser and accepted finite-value semantics are unchanged.
Presentation selection now rejects it without an explicit
`--baseline-portrait-presentation`; it remains incompatible with Atari and
legacy modes. Updated runtime/help wording identifies it as an old-portrait
diagnostic control.

## 9. Calibration-control prerequisite changes

Tube Y gain, depth-Y gain, scale, vertical offset, HUD controls, and
calibration guides now require effective Atari presentation, not literal
`--atari-presentation`. They work with the normal interactive default and
remain incompatible with baseline/legacy and non-rendering modes. Existing
finite/range, duplicate, level/start/playback, file-mode and other option
validation remains in place.

## 10. Background-opacity confirmation

`--background-opacity` implementation and rendering semantics were not
changed. Its default remains `1`, and the existing global development-only
behavior and headless restriction remain covered by tests.

## 11. Recording-format compatibility

No recording format, metadata, or input-frame fields changed. Presentation
selection remains outside recording identity. Existing recording round-trip
and loading tests passed.

## 12. Visible playback behavior

Visible playback resolves the no-selector Atari default; the alias remains
equivalent. The baseline diagnostic is allowed by the existing interactive
file-mode architecture. The legacy 4:3 option remains excluded by its current
file-mode allowlist.

## 13. Headless/input-diagnostic routing behavior

The centralized selection returns a non-rendering kind for headless playback
and input diagnostics rather than applying the interactive Atari default.
Explicit Atari/baseline rendering selectors are rejected in these modes.
Calibration controls remain rejected when their selected mode is
non-rendering. The existing headless route still occurs before Raylib setup.
Direct selection/parser and existing headless/input tests passed.

## 14. Renderer constructor/API decision

`VectorRenderer` constructor default remains unchanged (`false`). The
application now passes the selected effective mode explicitly. Existing
default-constructed renderer tests and old-projector internal call sites were
not redefined.

## 15. Startup/help/README wording changes

The startup message that called Atari presentation a development candidate
was removed. `--help` now identifies the default, compatibility alias,
baseline A/B diagnostic, separate legacy 4:3 mode, calibration controls, and
perspective prerequisite. Current README launch examples no longer require
the alias; calibration instructions describe the effective default.

## 16. Durable documentation candidate updates

README and the current established-display summary in
`TEMPEST-DEVELOPMENT-NOTES.md` distinguish the manually accepted Levels 1–16
presentation/calibrations from this PROJECT DECISION candidate to make that
same path the default. Both document alias, baseline and 4:3 meanings.
Manual acceptance is explicitly pending. Historical AI reports were not
rewritten.

## 17. Tests added/changed

- Added centralized selection, conflict, perspective, baseline projection and
  viewport, 4:3 distinction, effective calibration-control, file-mode,
  recording-simulation equivalence, and non-rendering routing coverage.
- Updated parser/calibration tests for default Atari selection and preserved
  alias validation.
- Existing numeric Atari calibration/projection tests were preserved.

## 18. Focused validation results

The project uses VSTest (`net9.0`, .NET SDK `9.0.304`). Every test command
included `--blame-hang-timeout 2min`. Ordered focused phases:

1. Selection/parser: **PASS — 93 passed**.
2. No-flag/alias equivalence: **PASS — 9 passed**.
3. Baseline/perspective: **PASS — 3 passed**.
4. Legacy 4:3 and display configuration: **PASS — 72 passed**.
5. Calibration/guides/HUD/tube controls: **PASS — 207 passed**.
6. Recording/visible playback: **PASS — 89 passed**.
7. Headless/input/background opacity: **PASS — 66 passed**.
8. Levels 1–16 Atari presentation regression: **PASS — 268 passed**.
9. Gameplay/progression regression: **PASS — 234 passed**.

Focused filters overlap; counts are not additive.

## 19. Recording/playback validation

Existing gameplay-recording and playback-session tests passed. Added
selection-level evidence that default and alias visible playback parsing
produce the same command, presentation is absent from that command/recording
identity, and replayed simulation summaries are deterministic and identical
between the two selections.

## 20. Headless/input validation

Selection/parser tests confirm no-selector headless and input-diagnostic
requests resolve to their non-rendering modes, and explicit Atari/baseline
rendering options reject. Existing headless playback and input-diagnostic
test groups passed.

## 21. Levels 1–16 presentation regression

**PASS — 268 tests.** This includes existing accepted Atari adapter,
calibration, Level 7–9 and Level 16 presentation, Blaster, ExitZoom, renderer,
and display-configuration coverage. New no-flag/alias projection equivalence
covers Levels 1, 5, 6, 9, 13 and 16.

## 22. Gameplay/progression regression

**PASS — 234 tests.** Deterministic gameplay and startup/progression
regressions passed. The selection change does not enter simulation or
recording state.

## 23. Full-suite result

`dotnet test TempestCSharp.slnx --blame-hang-timeout 2min --verbosity minimal`
— **PASS: 2,047 passed, 0 failed, 0 skipped**.

## 24. Release-build result

`dotnet build TempestCSharp.slnx --configuration Release --no-restore --verbosity minimal`
— **PASS: 0 warnings, 0 errors**.

## 25. `git diff --check` result

**PASS.** `git diff --check` reported no whitespace errors. Git emitted only
an informational LF-to-CRLF working-copy warning for `README.md`.

## 26. Unresolved questions

No implementation or automated-validation blocker remains. Neal's manual
acceptance of the default-selection promotion remains required; no visual
acceptance is inferred from automated tests.

## 27. Manual-validation matrix and exact commands

Do not repeat historical calibration acceptance; check that the existing
accepted appearance is selected by the intended command.

**A. Normal default launch**

```powershell
dotnet run --project src\Tempest.Game
```

Confirm it looks like the previously accepted Atari presentation without the
flag.

**B. Direct production starts**

```powershell
dotnet run --project src\Tempest.Game -- --start-level 5
dotnet run --project src\Tempest.Game -- --start-level 6
dotnet run --project src\Tempest.Game -- --start-level 13
dotnet run --project src\Tempest.Game -- --start-level 16
```

Confirm special/mapped paths remain as accepted.

**C. Compatibility alias**

```powershell
dotnet run --project src\Tempest.Game -- --start-level 13 --atari-presentation
```

Confirm it looks identical to the no-flag default.

**D. Baseline portrait diagnostic**

```powershell
dotnet run --project src\Tempest.Game -- --baseline-portrait-presentation
```

Confirm it deliberately selects the visibly different old portrait projection
in the same portrait viewport.

**E. Visible playback**

```powershell
dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\level5.json
```

Confirm playback uses the accepted Atari presentation by default and ordinary
visible-playback controls remain correct. If that documented recording is not
available locally, substitute an existing recording path.

## 28. Final Git/worktree state

At closeout, branch/HEAD/upstream remained `master` /
`03346581b0fa3253bbb010022e8c617ce414d466` / `origin/master`, ahead/behind
`0/0`. The index remained empty. Pre-existing unrelated Level 15 report/spec
changes and historical/untracked AI-Logs and local reference material remain
preserved. The implementation and required AI records are unstaged worktree
changes.

## 29. Staging/commit/push confirmation

Nothing was staged, committed, or pushed.

## 30. Level 17+ scope confirmation

Level 17 and later were neither implemented nor investigated.

**STOP: awaiting Neal's manual acceptance.**
