# Production presentation default promotion — final closeout

**Status: PASS — Neal's manual acceptance recorded, policy/docs reconciled,
focused and full validation passed. Commit, push, and independent GitHub
verification remain pending.** The interactive game was not launched.

## 1. Starting repository state and ownership

- Branch `master`; initial HEAD
  `03346581b0fa3253bbb010022e8c617ce414d466`
  (`Complete Level 16 production and presentation`); upstream
  `origin/master`; initial ahead/behind `0/0`; index clean.
- Initial tracked modifications were the prior Level 15 promotion closeout
  report, README, behavior specification, development notes, the presentation
  implementation in `DebugController.cs` and `Program.cs`, and its affected
  tests. These matched the accepted implementation report and this closeout's
  explicitly authorized scope.
- Initial untracked files included 30 AI-Logs prompts/reports, the related
  `PresentationSelectionTests.cs`, and
  `Reference/arcade/MBUCOD.V05.txt`. The reference file remains untouched and
  excluded. Existing work was preserved; no cleanup or reset was used.
- The modified Level 15 promotion report is a pre-existing, documented
  post-push update. It records facts already completed in that milestone and
  was left unchanged. Neal's current policy explicitly authorizes all
  legitimate current AI-Logs for this closeout.

## 2. Manual acceptance and accepted presentation behavior

**NEAL MANUAL ACCEPTANCE — PASS:** Neal reports that all requested
presentation-promotion manual tests passed. He manually played the complete
current production campaign, Levels 1–16, with the promoted default; all 16
levels passed and presentation/default selection passed throughout. Direct
starts and the requested representative presentation paths and diagnostics
also passed. The compatibility alias passed.

This is acceptance of the presentation-selection/default promotion only.
Existing per-level calibration values and accepted Levels 1–16 presentation
remain unchanged; no level was retuned.

- No-selector interactive gameplay and visible playback use the accepted
  Atari portrait presentation by default.
- `--atari-presentation` remains a valueless, redundant compatibility alias.
- `--baseline-portrait-presentation` remains the explicit old-portrait A/B
  diagnostic; `--portrait-perspective` remains gated to that mode.
- `--legacy-presentation` remains the separate legacy 4:3 presentation.
- Existing tube/HUD calibration controls and guides use effective Atari mode
  while retaining their value and selector validation.
- `--background-opacity` remains global and development-only, with default 1.
- Presentation selection remains outside deterministic simulation identity
  and the GameplayRecordingFile format. Headless playback and input
  diagnostics remain non-rendering.

## 3. Full campaign recording

`TEMP/GameplayRecordings/lvl1-16.json` exists at the requested local path. It
was not modified, inspected as frames, moved, or staged. The existing bounded
headless playback accepted it; two Release playback runs both exited 0 and
produced identical JSON telemetry:

- achieved boundary and recording duration: 35,350 ticks;
- remaining frames: 0; completion reason: `RecordingCompleted`;
- starting displayed level 1; ending displayed level 16 in `Complete` state;
- reported score: 134,039; random seed: 1,990,198,549.

This establishes successful deterministic simulation playback for this
recording, not arcade input fidelity or manual gameplay acceptance.

## 4. AI-Logs policy and inventory

Neal's new policy supersedes the former AI-Logs local-only/unstaged policy.
`AI-Logs/Prompts/`, `AI-Logs/Reports/`, and `AI-Logs/Copilot.txt` are GitHub
development-continuity records. This closeout authorizes including all
legitimate current records, not only this milestone's pair. Future removal,
archival, and ZIP consolidation remain deferred. The policy does not grant
general commit/push authorization for unrelated tasks.

Final pre-stage inventory: 376 prompt files, 400 report files, and
`Copilot.txt` (777 files total). The initial Git inventory had 745 tracked
AI-Logs paths, 30 untracked prompt/report paths, and one modified tracked
report; adding this closeout pair brought the untracked prompt/report set to
32 before staging. Counts were obtained from the filesystem and Git.

`Copilot.txt` is legitimate historical Tempest development context, is text,
and is intended for preservation unchanged. The bounded safety scan covered
all prompt/report files and `Copilot.txt`; it found no suspected credentials,
secrets, or private-key material and no non-text files.

## 5. Ignore rules, current instructions, and token protocol

- `.gitignore` required no change: AI-Logs paths are not ignored. The existing
  `/Temp/` rule continues to ignore `TEMP/GameplayRecordings/lvl1-16.json`
  on this Windows worktree. No broad unignore was added.
- `.github/copilot-instructions.md` now states that all legitimate AI-Logs
  remain in GitHub during explicitly authorized closeouts; commit/push still
  requires Neal's explicit authorization. It also preserves local TEMP,
  gameplay recording, screenshot, video, ROM/reference and user-asset
  protections.
- The durable completion token is now a unique standalone
  `TEMPEST-END-...` token using uppercase ASCII letters, digits, and hyphens.
  The former underscore format is obsolete for new prompts and remains
  untouched in historical records.
- This prompt was copied byte-for-byte from the supplied task attachment.
  Its token check found exactly one valid standalone completion token.

## 6. Documentation and implementation preservation

Updated README, development notes, behavior specification, and
`.github/copilot-instructions.md` to record the accepted default, alias and
diagnostic boundaries, manual Levels 1–16 acceptance, AI-Logs GitHub policy,
and new token protocol. No calibration table, projection, gameplay, recording
format, or runtime selection behavior was changed during closeout. The
previously completed production-presentation implementation and its tests
were preserved.

## 7. Automated validation

All commands completed; test runs used a two-minute VSTest hang timeout.

- Focused selection/parser, calibration, recording/playback, headless/input,
  progression, opacity and presentation coverage: **PASS — 460 passed,
  0 failed, 0 skipped**.
- Levels 1–16 Atari-presentation, ExitZoom, Blaster, direct-start,
  recording/playback, and progression regression: **PASS — 486 passed,
  0 failed, 0 skipped**.
- Full solution:
  `dotnet test TempestCSharp.slnx --blame-hang-timeout 2min --verbosity minimal`
  — **PASS — 2,047 passed, 0 failed, 0 skipped**.
- Fresh Release build:
  `dotnet build TempestCSharp.slnx --configuration Release --no-restore --verbosity minimal`
  — **PASS — 0 warnings, 0 errors**.
- Focused `TEMP`/AI-Logs/token-policy checks: **PASS** for the relevant
  ignore and token properties. The initial text assertion used case-sensitive
  matching for “archival” and failed to match the lower-case policy wording;
  this was a check-script mismatch, not a repository-policy failure. The
  follow-up check uses case-insensitive matching.
- `git diff --check` and staged whitespace check: pending final staging audit.

## 8. Staging and publication

- Staged-file audit, `git status`, staged stat/diff review, and
  whitespace checks: **PASS**. The staged set contains 45 paths: 16 prompts,
  17 reports (including the modified Level 15 post-push update), and 12
  implementation/test/documentation/policy paths. All 32 newly untracked
  prompt/report files and the one modified historical report are staged.
- `AI-Logs/Copilot.txt` is already tracked and unchanged in the starting
  commit, so it remains in the repository tree and is not a changed/staged
  path in this commit. Its content was preserved.
- The behavior-spec staged diff contains only the 26-line default-promotion
  addition. The unrelated pre-existing Level 15 checkpoint wording hunk
  remains unstaged. `git diff --cached --check` and `git diff --check` passed.
- Staged path audit found no unexpected paths and no `TEMP/`, `Reference/`,
  screenshot, video, generated, or user assets.
- `TEMP/GameplayRecordings/lvl1-16.json`, other TEMP content,
  `Reference/arcade/MBUCOD.V05.txt`, and unrelated local assets are excluded.
- Primary commit subject: `Make accepted Atari presentation the default`.
  Commit SHA and push result: pending.
- Independent GitHub primary-commit verification and retrieval of this
  prompt, implementation report, historical prompt/report, and
  `Copilot.txt`: pending.
- The bounded report-finalization process permits at most one report-only
  follow-up commit. Its final push and remote verification will be reported
  after that commit without a third report-update commit.

## 9. Remaining scope and local material

No Level 17+ implementation or investigation was performed. No archive, ZIP
consolidation, or historical-log cleanup occurred. The local campaign
recording, the unrelated untracked reference file, and any unrelated local
assets remain protected and uncommitted.
