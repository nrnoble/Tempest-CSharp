# Portrait Viewport Border

**Date:** 2026-11-14
**Status:** IMPLEMENTED — AUTOMATED VALIDATION PASS; MANUAL ACCEPTANCE PENDING
**Completion token:** `TEMPEST_PORTRAIT_VIEWPORT_BORDER_20261114`

## Starting Git state

- Branch: `master`; upstream: `origin/master`.
- `HEAD` and `origin/master`: `385f603a94391756b2ca6cccd524dc17b281e782`.
- Index was clean.
- The expected ten tracked files from the current portrait/perspective candidate
  were modified and unstaged; no non-ignored untracked files existed.
- The tracked candidate diff before this milestone was 1,763 insertions and
  88 deletions. The state matched the two named presentation reports; no
  unrelated work was found.

The current candidate changes were preserved. No files were staged, committed,
or pushed.

## Source inspected

- `src/Tempest.Game/Rendering/DisplayConfiguration.cs` — existing host,
  viewport and HUD configuration.
- `src/Tempest.Game/Rendering/VectorRenderer.cs` — `Draw`, the portrait
  overlays and playback overlay paths, and their camera/scissor ordering.
- `src/Tempest.Game/Program.cs` — presentation-mode selection and renderer
  invocation.
- `tests/Tempest.Game.Tests/DisplayConfigurationTests.cs` — existing portrait
  configuration tests.
- The two user-specified 2026-11-14 presentation reports to confirm the
  intended starting candidate state.

## Implementation

The border is one `Raylib.DrawRectangleLinesEx` outline using `Color.Blue`
(opaque Raylib blue) and a thickness of `1` host pixel.

Its rectangle is `x=280, y=0, width=720, height=960`, ending at host-space
`x=1000, y=960`. `DisplayConfiguration.PortraitViewport` derives it from
`PortraitViewportLeft`, `PortraitViewportWidth`, and
`PortraitViewportHeight`; the existing centered-left expression remains
`(WindowWidth - PortraitViewportWidth) / 2`. The viewport settings themselves
were not changed. A focused assertion covers the exact configured rectangle.

In normal portrait rendering, the outline is drawn after `DrawPortraitOverlays`
has ended the HUD camera and portrait scissor. The `GameOverAnimation` early
return also draws it after ending the world camera and scissor and, if present,
after the playback overlay has ended its camera and scissor. The border is
therefore in host coordinates, unclipped by the portrait scissor, outside both
cameras, and last over the portrait content.

Both call sites are inside the existing experimental-portrait condition.
Legacy presentation never calls the border helper. The same renderer path
covers portrait gameplay and its transitions/presentation states; no
state-specific border or simulation change was added.

## Files changed by this milestone

- `src/Tempest.Game/Rendering/DisplayConfiguration.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `tests/Tempest.Game.Tests/DisplayConfigurationTests.cs`
- `AI-Logs/Prompts/2026-11-14-portrait-viewport-border-prompt.md`
- `AI-Logs/Reports/2026-11-14-portrait-viewport-border-report.md`

The AI log files are ignored working material. No durable behavior or
development documentation was changed.

## Validation

- **Focused tests:** `DisplayConfigurationTests` passed, 69/69. An initial
  invocation used `00:03:00`, which VSTest rejected as a timeout format; it
  was rerun with the accepted `180s` timeout and passed.
- **Full suite:** passed, 1,411/1,411, with VSTest hang timeout `180s`.
- **Release build:** `dotnet build .\TempestCSharp.slnx --configuration Release`
  passed with 0 warnings and 0 errors.
- **`git diff --check`:** passed with no output.
- No simulation-state or exact native draw-call test was added; the outline
  helper performs only the Raylib draw call, and its two call sites and
  camera/scissor placement were verified by focused source inspection.

## Manual acceptance

The interactive game was not launched. Neal's requested manual checks:

```powershell
dotnet run --project .\src\Tempest.Game -- --level 9 --portrait-perspective 125
dotnet run --project .\src\Tempest.Game -- --level 9 --portrait-perspective 0
dotnet run --project .\src\Tempest.Game -- --level 9 --legacy-presentation
```

Expected: both portrait commands show one thin blue outline at the configured
viewport boundary without changing gameplay/presentation geometry; the legacy
command shows no portrait border.

## Final Git state and inspection boundaries

- Branch remains `master`; `HEAD` and `origin/master` remain
  `385f603a94391756b2ca6cccd524dc17b281e782`.
- Index remains clean. The ten tracked candidate files remain modified and
  unstaged; no non-ignored untracked files exist.
- Nothing was staged, committed, or pushed.
- No screenshots, images, videos, MAME frames, gameplay recordings, Gemini
  material, Incitti source, web sources, or raw ROM files were inspected.
  `TEMP/myNotes.md` was not read.
