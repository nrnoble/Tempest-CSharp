# Background-opacity closeout

## Status

**CLOSEOUT PASS — manual acceptance, validation, staged review, commit, and
push completed.** The GitHub connector could not independently verify remote
master because both attempted calls failed with a missing parameter-header
error. Local tracking confirms `master` and `origin/master` match.

## Accepted behavior

Neal's manual result: **`--background-opacity` works perfectly and produces
exactly the desired calibration behavior.** The MAME/desktop reference is
visible through Tempest's background; tube and HUD vectors remain solid;
Blaster, enemies, shots, and effects remain normally visible; geometry is
unchanged; input remains normal; and the feature is useful for manual overlay
calibration. Background-only transparency is accepted and frozen.

The accepted implementation is documented in
`AI-Logs/Reports/2026-11-24-background-opacity-correction-report.md`:
Raylib-cs 8.1.0 `ConfigFlags.TransparentWindow` is requested only for values
below 1 and before window initialization. The black background clear alpha
alone is controlled. The finite invariant-culture range is 0 through 1,
default is opaque black 1, headless playback rejects the option, and the
setting is not recording data. Whole-window `--window-opacity` remains a
non-accepted historical experiment and is not restored.

On the tested Windows/Raylib 6.0 path, `Raylib.TakeScreenshot` remains a
Tempest framebuffer capture, not a desktop composite. The probe found PNG
clear pixels saved with alpha 255, yielding opaque Tempest-only screenshots.
Screenshot architecture is unchanged.

## Repository state and closeout

- Starting branch: `master`; upstream: `origin/master`.
- Starting HEAD: `fa94f54ccba8de265ef474805c9516eb3b650bb6`.
- Starting ahead/behind: `0/0`; starting index: empty.
- Initial changes included the accepted background-opacity candidate and
  prior window-opacity historical logs, plus a pending Level 14 closeout
  report update. Unrelated `Reference/arcade/MBUCOD.V05.txt` and
  `temo/gameplayrecordings/level-13` are excluded and must remain untouched.
- **PASS** — Fresh focused Release closeout regression gate: 955 passed,
  0 failed, 0 skipped. The filter covered background-opacity parser/config,
  CLI/startup, window lifecycle, display, screenshots, Atari presentation,
  calibration and depth-Y controls, playback/headless, input, production
  Levels 1-14, and direct Level 15 preservation.
- **PASS** — After restoring original cursor/FPS setup placement, a final
  focused startup/opacity regression gate passed: 125 passed, 0 failed,
  0 skipped.
- **PASS** — Full solution Release suite:
  `dotnet test TempestCSharp.slnx --configuration Release --no-restore
  --blame-hang-timeout 3m --verbosity minimal`; 1,959 passed, 0 failed,
  0 skipped.
- **PASS** — Release solution build:
  `dotnet build TempestCSharp.slnx --configuration Release --no-restore
  --verbosity minimal`; 0 warnings, 0 errors.
- **PASS** — `git diff --check`; only the existing README LF-to-CRLF
  working-copy warning was emitted.
- **PASS** — `git diff --cached --check`; staged source, test, README, AI-Logs
  and Level 14 report update were reviewed. No protected or unrelated asset
  is staged.
- **PASS** — Staged inventory contains exactly the paths listed below.
- **PASS** — Commit `045aeb240fce91a3eaacf9424da30cd7359927bf`,
  subject `Add background overlay calibration`, with the required Copilot
  co-author trailer.
- **PASS** — Normal push to `origin/master` completed:
  `fa94f54..045aeb2 master -> master`.
- **BLOCKED** — Independent GitHub connector verification. Both
  `get_commit(master)` and `list_branches` failed with
  `MCP error -32020: header mismatch: missing Mcp-Param-owner header for
  parameter "owner"`. No connector result is claimed.
- **PASS** — Local `master` and `origin/master` both resolve to
  `045aeb240fce91a3eaacf9424da30cd7359927bf`, ahead/behind `0/0`; index
  is empty.

The fresh pre-stage audit confirmed `master` at the starting HEAD above,
tracking `origin/master` at ahead/behind `0/0`, with an empty index. The
Level 14 closeout-report post-push update was included in this commit. The
untracked MBUCOD reference and `temo/gameplayrecordings/level-13` remain
unrelated and excluded.

## Committed inventory

- `AI-Logs/Prompts/2026-11-24-background-opacity-closeout-prompt.md`
- `AI-Logs/Prompts/2026-11-24-background-opacity-correction-prompt.md`
- `AI-Logs/Prompts/2026-11-24-window-opacity-diagnostic-prompt.md`
- `AI-Logs/Reports/2026-11-24-background-opacity-closeout-report.md`
- `AI-Logs/Reports/2026-11-24-background-opacity-correction-report.md`
- `AI-Logs/Reports/2026-11-24-level14-final-closeout-report.md`
- `AI-Logs/Reports/2026-11-24-window-opacity-diagnostic-report.md`
- `README.md`
- `src/Tempest.Game/Debug/BackgroundOpacityOptions.cs`
- `src/Tempest.Game/Debug/DebugController.cs`
- `src/Tempest.Game/Debug/HeadlessGameplayPlaybackCli.cs`
- `src/Tempest.Game/Debug/InputDiagnostic.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `tests/Tempest.Game.Tests/BackgroundOpacityOptionsTests.cs`

## Final report

The commit contains exactly the 15 paths listed in the committed inventory.
No protected or unrelated local asset was included. At closeout, the working
tree has only this report's intentional post-push update unstaged, which
records the resulting SHA and connector outcome without amending or creating
a report/commit loop. The index is empty, `master` tracks `origin/master` at
`045aeb240fce91a3eaacf9424da30cd7359927bf` with ahead/behind `0/0`, and
untracked `Reference/arcade/MBUCOD.V05.txt` and `temo/` remain untouched and
unstaged. No Level 15 work was started.
