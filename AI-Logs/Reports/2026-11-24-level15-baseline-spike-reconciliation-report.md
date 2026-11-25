# Level 15 baseline and Spike correction: blocked

**Status: BLOCKED — tested candidate not identified unambiguously.**

## Manual result and scope

Neal reports Level 15 gameplay passes and accepts the tested tube presentation
at Y gain 1.15, scale 1, vertical offset 75, and depth-Y gain 1, except that
the starting Spikes are significantly too long. This is not a general
arcade-fidelity acceptance. Manual acceptance of a Spike correction remains
pending; no correction has been made.

## Starting repository state

- Repository: `C:\Users\nrnob\source\repos\Tempest`, branch `master`,
  HEAD `045aeb240fce91a3eaacf9424da30cd7359927bf` ("Add background overlay
  calibration"), upstream `origin/master`, ahead 0, behind 0.
- Index empty. Tracked worktree had only the pre-existing modification to
  `AI-Logs/Reports/2026-11-24-background-opacity-closeout-report.md`;
  there were no tracked or ordinary untracked changes under `src` or `tests`.
- Pre-existing untracked entries were the previous Spike prompt and report,
  `Reference/arcade/MBUCOD.V05.txt`, and `temo/`. They were not altered or
  inspected beyond the explicitly authorized previous report and prompt.
- The only registered Git worktree is this repository. A bounded ignored-file
  name check under `src` and `tests` for Level 15, Spike, presentation and
  progression names found only build-output sound files, not a source
  candidate. This is not proof that no candidate exists elsewhere, in an
  ignored file under another name, or in an external or previously built
  executable. Such locations were not inspected.
- Contrary to the general AI-Logs policy's description, the newly created
  prompt path is not actually Git-ignored in this checkout; earlier AI-Logs
  files include tracked records. Neither this prompt nor this report is staged.

## Observed baseline and discrepancy

The previous blocked report recorded the same HEAD and no source/test changes.
The present audit finds the same HEAD and likewise no source/test changes.
That is why both inspections see the same old direct-start candidate; the
repository evidence does not establish why a requested newer uncommitted
production candidate is absent, whether it ever existed, or which binary or
checkout Neal actually ran. The earlier prompt explicitly requested a current
uncommitted production candidate; the prior report accurately stopped on the
resulting mismatch. Neal's subsequent gameplay result does not itself identify
the source revision or prove that a new production implementation existed.

The identifiable current candidate comprises, at minimum:

- `src/Tempest.Game/Levels/LevelConfiguration.cs`: explicitly development-only
  `Level15DirectStartCandidate`, geometry from `LoadLevel15OpenTube`, effective
  gameplay parameter override 11, and
  `UsesLevel11ParameterDevelopmentBaseline = true`.
- `src/Tempest.Game/Levels/LevelProgression.cs`: direct level 15 routes to
  this candidate; normal production progression stops at Level 14.
- `src/Tempest.Game/Levels/LevelLoader.cs`,
  `src/Tempest.Game/Levels/AtariTubeRecordCatalog.cs`, and
  `src/Tempest.Game/Levels/RomPresentationMetadata.cs`: candidate geometry,
  tube record and presentation metadata.
- `src/Tempest.Game/Rendering/AtariPresentationCalibration.cs`: Level 15
  defaults to `(1, 1, 0, 1)`; CLI values are absolute overrides. The accepted
  `(1.15, 1, 75, 1)` values are not encoded as Level 15 defaults and can be
  supplied as overrides. Their use in Neal's actual run is not independently
  verifiable from the checkout.
- `src/Tempest.Game/Debug/DebugController.cs` and `src/Tempest.Game/Program.cs`:
  `--level 15` permits the direct candidate, whereas `--start-level 15` is
  rejected because it requires a production level.
- `tests/Tempest.Game.Tests/Level15TubeCandidateTests.cs` exercises the
  direct candidate, including its Level 11 development evidence. No
  `Level15WaveSelection.cs` exists at the expected source path.

This is not evidence that these exact files or current binaries constituted
Neal's tested build. No newer production candidate is identifiable in the
current source or test worktree. A different candidate's file list, location,
or existence cannot be established from the available evidence.

## Spike evidence and limits

**Observed source:** the direct candidate sets `StartingSpikeHeight` to
`0xB0 * Level.Depth / 224f`. `Level11WaveSelection.StartingSpikeHeightRaw`
is `0xB0`. Levels 11–14 instead use the project expressions
`(0xF0 - StartingSpikeHeightRaw) * Level.Depth / 224f`, with raw values
`0xB0`, `0xA8`, `0xA0`, and `0xA0`, respectively. The former value is
associated with the explicit temporary Level 11 development baseline, not
established as the genuine Level 15 ROM value.

**Existing evidence:** the 2026-11-21 Level 15 mapping report classifies the
row-10 geometry mapping as ROM-backed and the reuse of Level 11 gameplay
parameters as a development baseline. This investigation did not reverify
ROM data. The correct Level 15 raw Spike tip, interpretation, initialization
and rendering provenance, and root cause of Neal's observed excessive length
remain unestablished. The apparent conversion difference is a hypothesis,
not proof that modifying the current candidate is authorized or correct.
Per the fail-closed gate, the requested further ROM investigation and
implementation did not begin.

## Changes and validation

- Created the exact supplied prompt at
  `AI-Logs/Prompts/2026-11-24-level15-baseline-spike-reconciliation-prompt.md`;
  byte-for-byte comparison via `Get-FileHash` **PASS**.
- Created this paired blocked report. No production source, tests, durable
  documentation, presentation calibration, protected material, or previous
  reports were modified. No unrelated Level 15 work was performed.
- `git diff --check`: **PASS** (tracked worktree diff).
- Level 15 Spike tests, relevant regression batches, deterministic headless
  run/playback, complete Release tests, and Release solution build:
  **BLOCKED / NOT RUN** because the baseline gate failed and no correction
  exists. No interactive game was launched.
- No stage, commit, push, branch switch, stash, reset, restore, or cleanup.

## Handoff

Identify the exact build Neal ran: source repository/worktree path and
commit, any uncommitted patch or candidate files, and the actual startup
command (or confirm that the current direct Level 15 candidate at HEAD
`045aeb2` was the tested version). If a different build was used, restore
or provide its corresponding source and test changes without overwriting
unrelated work. Clarify whether the accepted values were command-line
overrides. The executable alone cannot establish the candidate's source.

For reference only, the current checkout's uncorrected direct candidate can
be started with the following command; it is **not** a corrected-candidate
acceptance command and has not been run by Copilot:

```powershell
dotnet run --project src\Tempest.Game -- --atari-presentation --level 15 --tube-y-gain 1.15 --tube-scale 1 --tube-vertical-offset 75 --tube-depth-y-gain 1
```

Once the candidate is identified and corrected and automated validation
completes, Neal's manual check should cover shorter arcade-consistent starting
Spikes; unchanged count and lanes; extension and interactions; passing
gameplay; accepted tube shape and placement; and absence of visual or gameplay
regressions. The accepted presentation values have not been changed.
