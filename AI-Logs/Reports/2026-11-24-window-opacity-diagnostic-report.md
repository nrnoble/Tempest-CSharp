# Development-only window-opacity diagnostic

## Status and scope

**Implementation and automated validation PASS; manual visual acceptance
pending.** The bounded desktop smoke check established window initialization
and process responsiveness, but did not independently verify rendered frames
or visual translucency.

Starting state: `master` at `fa94f54ccba8de265ef474805c9516eb3b650bb6`,
tracking `origin/master` with ahead/behind `0/0`, and an empty index. The
pre-existing unstaged Level 14 post-push closeout-report update and unrelated
untracked `Reference/arcade/MBUCOD.V05.txt` and `temo/` were preserved.
No stage, commit, push, reset, restore, stash, clean or rebase was performed.

## Binding capability and design

The project references **Raylib-cs 8.1.0**. The restored net9.0 assets select
its `lib/net8.0/Raylib-cs.dll`; the accompanying package XML documents
`Raylib_cs.Raylib.SetWindowOpacity(System.Single)`, with range
`[0.0f..1.0f]`. The Release build confirms the managed call is callable.
No P/Invoke, layered-window manipulation, or new dependency was added.

`--window-opacity <value>` is a development-only whole-window startup setting.
Its omitted default is 1 and makes no additional native call. Explicit values
are invariant-culture finite numbers from 0 through 1 inclusive; missing,
malformed, non-finite, out-of-range, duplicate and joined forms are rejected.
The option does not require Atari presentation. It is not stored in gameplay
recordings and composes with file recording and interactive file playback.
Headless playback rejects it because no window exists.

The existing gameplay and standalone input-diagnostic window paths apply the
value once after initialization, after checking Raylib's window-ready state.
The default path is unchanged. No tube projection, gameplay, screenshot,
framebuffer, color, calibration or recording code was changed. The binding
documentation does not state guarantees for fullscreen, borderless, or
individual desktop compositors; those modes were not changed or tested.

## Changed files

- `src/Tempest.Game/Debug/WindowOpacityOptions.cs` — separate startup option
  and testable ready-window application seam.
- `src/Tempest.Game/Debug/DebugController.cs` — option parsing and file-mode
  compatibility.
- `src/Tempest.Game/Debug/HeadlessGameplayPlaybackCli.cs` — explicit rejection.
- `src/Tempest.Game/Debug/InputDiagnostic.cs` and `src/Tempest.Game/Program.cs`
  — application after window initialization and CLI help.
- `tests/Tempest.Game.Tests/WindowOpacityOptionsTests.cs` — bounds, culture,
  lifecycle, compatibility, headless, recording and render-content tests.
- `README.md` — diagnostic and limitations.
- `AI-Logs/Prompts/2026-11-24-window-opacity-diagnostic-prompt.md` and this
  paired report.

## Validation

- **PASS** — Focused Release VSTest gate with `--blame-hang-timeout 3m`:
  800 passed, 0 failed, 0 skipped. The filter covered opacity, startup/CLI,
  display, screenshot, calibration, playback/headless and production tests.
- The first focused attempt was **FAIL** at compile time: the binding's
  `IsWindowReady()` returns `CBool`, not a `bool` delegate. A lambda uses its
  supported conversion; the complete focused gate was then rerun and passed.
- **PASS** — `dotnet test TempestCSharp.slnx --configuration Release
  --no-restore --blame-hang-timeout 3m --verbosity minimal`: 1,958 passed,
  0 failed, 0 skipped, including Levels 1-14 production coverage.
- **PASS** — `dotnet build TempestCSharp.slnx --configuration Release
  --no-restore --verbosity minimal`: 0 warnings, 0 errors.
- **PASS** — `git diff --check`; Git emitted an LF-to-CRLF working-copy warning
  for README, not a whitespace error.
- **PASS** — Actual Release executable rejected opacity `1.1`, `NaN`, and
  the headless-playback combination with clear errors and exit code 2 before
  window initialization.
- **PARTIAL** — One bounded Level 14 graphical smoke run of the freshly built
  Release DLL with `--atari-presentation --window-opacity 0.5` initialized
  raylib's desktop GLFW/Win32 window and audio, had a nonzero window handle,
  remained responsive for the 8-second observation, and showed increasing
  process CPU time with no reported exception. The process was explicitly
  stopped by its PID. Window initialization and call completion passed; CPU
  activity does not prove normal rendered frames or visible translucency.
  No gameplay or visual acceptance was performed.

## Manual handoff

Neal should compare against an authorized reference behind the window and
check whole-window translucency, interaction, normal tube/HUD/entity drawing,
unchanged tube geometry, and F10 screenshots containing normal rendered
output rather than a desktop composite. Actual desktop translucency and
normal visible rendering remain unverified until this manual check.

```powershell
dotnet run --project src\Tempest.Game -- --level 14 --atari-presentation --window-opacity 1
dotnet run --project src\Tempest.Game -- --level 14 --atari-presentation --window-opacity 0.5
dotnet run --project src\Tempest.Game -- --level 14 --atari-presentation --window-opacity 0.25
dotnet run --project src\Tempest.Game -- --level 14 --atari-presentation --window-opacity 0.5 --calibration-guides --tube-y-gain 1 --tube-scale 1.1 --tube-vertical-offset 200 --tube-depth-y-gain 1
```
