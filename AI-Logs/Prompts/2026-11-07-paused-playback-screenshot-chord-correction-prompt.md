# Paused playback screenshot chord correction

This prompt record reconstructs the substantive requirements from the summarized
continuation and attached user task; it is not a verbatim transcript.

Continue the existing uncommitted screenshot/UserData candidate. Do not restart
the milestone. Neal's diagnostic evidence is decisive: his tested physical
attempt produced one logical F12, no F11, no Tempest capture request or project
destination, and a new native `screenshot000.png`. Do not repeat the F11/F9
mapping assumption or select another speculative function key.

Implement **Ctrl+Shift+S** for Tempest-owned screenshots. Accept either left or
right Control and either left or right Shift, and require an S press edge. The
command is valid only during paused visible file playback. Audit conflicts
before implementation; if a real conflict exists, stop and report it rather
than choosing another binding. Keep the command outside `PlayerInput` and
recording serialization. A held key or autorepeat must not repeat captures;
a fresh S press edge may capture again.

Preserve the accepted screenshot behavior: render/capture timing, boundary-aware
filenames, collision-safe no-overwrite suffixes, `AppContext.BaseDirectory` as
the root, `<application root>\UserData\Screenshots\` as the destination,
lazy screenshot-directory creation, and no creation of
`UserData\GameplayRecordings` merely because a screenshot is taken. Preserve
the recording path policy and do not migrate or rewrite recordings. Do not
change gameplay, playback, rendering, recording formats, or paths.

Do not rebuild or replace Raylib, patch its DLL, install hooks, or use raw
platform input. Refine the bounded opt-in diagnostic to report Control/Shift,
the S press edge, the project request and exact destination/file result, F12,
and new or overwritten generic native screenshots. Do not continuously log
unrelated key events.

Add focused coverage for both sides of each modifier, missing modifiers,
press-edge/held behavior, paused-only operation, no running/live/headless
capture, separation from gameplay input, no Tempest F11/F12 command, unchanged
playback boundary/cursor, destination, unique suffixes, and diagnostic
outcomes. Unit tests do not establish that Raylib created no native screenshot;
Neal must verify that manually.

Update the directly related README and behavior/development notes. Run focused
tests, the full `dotnet test TempestCSharp.slnx` suite, `dotnet build
TempestCSharp.slnx`, and `git diff --check`.

Do not launch the interactive game; Neal performs manual acceptance. Leave all
changes unstaged and uncommitted, do not push, and do not touch the existing
untracked `screenshot000.png` through `screenshot006.png` artifacts. Report the
manual command:

```powershell
dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\levels1-7.json --screenshot-key-diagnostic
```
