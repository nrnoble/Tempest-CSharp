# Level 7 prototype false recording/playback conflict

## Result

**PASS — candidate corrected; manual visual acceptance remains pending.**

## Root cause and correction

`Program.cs` first validates the prototype's context (including requiring a
direct Level 7 start), then parses a gameplay file command before constructing
the game. With no file-mode arguments, the parsed file command is null and the
normal startup path constructs the selected production level and passes the
prototype option to the renderer.

The false conflict was in `DebugController.TryParseGameplayFileCommand`: it
rejected every invocation containing `--atari-tube-prototype` before scanning
for `--record-gameplay` or `--playback-file`. Thus the ordinary requested
command never reached startup; no recording or playback state was active.

The guard now runs only after a real recording/playback option is found. The
prototype-only Level 7 command and an ordinary option-off launch are accepted;
actual prototype + recording and prototype + playback remain rejected. The
separate prototype context and scenario conflict checks remain unchanged.

## Files changed for this correction

- `src/Tempest.Game/Debug/DebugController.cs` — condition the file-mode conflict
  on finding a file-mode argument.
- `tests/Tempest.Game.Tests/AtariTubeRecordPrototypeTests.cs` — verify ordinary
  direct Level 7 and option-off parsing are accepted, and real recording and
  playback conflicts plus existing prototype restrictions remain rejected.
- `AI-Logs/Prompts/2026-11-16-level7-prototype-false-recording-playback-conflict-prompt.md`
  and this report — ignored task records.

The source and test files already contained pre-existing candidate work when
this task began; the correction was applied surgically without discarding it.

## Validation

- Regression-first check before the fix: the new direct-Level-7 parser assertion
  failed at the unconditional rejection.
- Focused `AtariTubeRecordPrototypeTests`: **PASS**, 33 passed, 0 failed.
- Full `Tempest.Game.Tests` suite: **PASS**, 1,538 passed, 0 failed.
- Release build of `Tempest.Game.csproj`: **PASS**, 0 warnings, 0 errors.
- `git diff --check`: **PASS**. Git emitted existing LF-to-CRLF working-copy
  warnings for `LevelLoader.cs` and `InputSystem.cs`; there were no whitespace
  errors.

The interactive game was not launched. No Atari coordinates, vector geometry,
projection, viewport mapping, presentation, or gameplay behavior were changed.

## Worktree and manual acceptance

At completion, the branch remains `master` at `385f603a94391756b2ca6cccd524dc17b281e782`,
tracking `origin/master`. The worktree remains dirty with pre-existing modified
and untracked candidate/reference files; nothing was staged, committed, reset,
cleaned, stashed, switched, or pushed. These unrelated pre-existing changes
were preserved.

Neal must still launch and visually assess the Level 7 direct-Atari wireframe.
Run exactly:

```powershell
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 7 --atari-tube-prototype
```

No visual acceptance is claimed by this report.
