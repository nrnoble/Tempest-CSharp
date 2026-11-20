# Framebuffer playback parser-wiring fix

Date: 2026-11-17
Status: **PASS — parser correction and safe Debug runtime recognition validated**

## Scope and change

The established cause was confirmed: `TryParseGameplayFileCommand` already
validated the framebuffer option through its dedicated parser, but its
playback argument scan did not consume the option and filename. The filename
then reached the generic unknown-gameplay-file-argument branch.

The scan in `src/Tempest.Game/Debug/DebugController.cs` now consumes
`--level7-framebuffer-diagnostic` and its following value using the existing
`TryReadOptionValue` helper, alongside the visible-runtime diagnostic option.
Framebuffer filename validation remains exclusively in the dedicated parser.
No aliases, unrelated options, or command semantics were added.

The focused regression in
`tests/Tempest.Game.Tests/Level7VisibleRuntimeDiagnosticTests.cs` exercises
`--playback-file` with both diagnostics through
`TryParseGameplayFileCommand`. It verifies successful playback parsing,
continued visible-runtime option handling, rejection of an unrelated option,
and that a missing framebuffer value returns the same error as the dedicated
framebuffer parser. It also checks playback parsing with diagnostics disabled.

## Validation

- **PASS — Debug build:** `dotnet build src\Tempest.Game\Tempest.Game.csproj --configuration Debug --no-restore`; 0 warnings, 0 errors.
- **PASS — focused tests:** `dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --configuration Debug --no-restore --filter "FullyQualifiedName~DebugControllerTests|FullyQualifiedName~Level7VisibleRuntimeDiagnosticTests|FullyQualifiedName~Level7FramebufferDiagnosticTests" --blame-hang-timeout 2m`; 66 passed, 0 failed, 0 skipped.
- **PASS — whitespace:** `git diff --check`.
- **PASS — safe default Debug runtime probe:** invoked `dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --playback-file C:\__tempest_framebuffer_parser_wiring_probe_20261117_no_such_recording.json --level7-visible-runtime-diagnostic level7-visible-runtime-parser-wiring-probe-20261117.jsonl --level7-framebuffer-diagnostic level7-framebuffer-parser-wiring-probe-20261117.jsonl`, with a 90-second timeout and no `--no-build`. It exited with code 2 at the deliberately missing recording:

  ```text
  Could not prepare gameplay file mode: Could not find file 'C:\__tempest_framebuffer_parser_wiring_probe_20261117_no_such_recording.json'.
  ```

  It did not reject `--level7-framebuffer-diagnostic`. Execution stopped before window/gameplay startup; no interactive game was launched. The exact probe visible-runtime JSONL destination and framebuffer output directory were absent before and after the probe; no diagnostic output or capture was created.

The requested A/B diagnostic destinations were checked under
`TEMP\Diagnostics\Level7VisibleRuntime` and
`TEMP\Diagnostics\Level7Framebuffer`; both OFF/ON destination pairs are absent.
The source playback recording exists. Therefore, the OFF/ON commands below
are validated for parser/runtime recognition and ready for Neal's controlled
manual experiment; that experiment and its manual acceptance have not been
performed.

## Files and behavior boundaries

- `src/Tempest.Game/Debug/DebugController.cs` — added the playback-scan consume branch.
- `tests/Tempest.Game.Tests/Level7VisibleRuntimeDiagnosticTests.cs` — extended the focused regression in a pre-existing untracked test file.
- `AI-Logs/Prompts/2026-11-17-framebuffer-playback-parser-wiring-fix-prompt.md` — saved the provided task.
- `AI-Logs/Reports/2026-11-17-framebuffer-playback-parser-wiring-fix-report.md` — this report.

No gameplay, presentation, playback, framebuffer capture, projection,
geometry, timing, or Level 7 investigation behavior was changed. No durable
gameplay/presentation documentation was updated.

## Git preservation

Initial and final repository state: branch `master`, HEAD
`385f603a94391756b2ca6cccd524dc17b281e782`, upstream `origin/master`, and an
empty index. All 17 pre-existing modified tracked paths and 21 pre-existing
untracked paths remain present; no unrelated work was staged, committed,
discarded, or rewritten. The required AI-Logs records remain ignored and
unstaged.

## Neal's controlled experiment commands

**COMMAND A — Atari OFF**

```powershell
dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --playback-file TEMP\GameplayRecordings\Level-7-slowmo.json --level7-visible-runtime-diagnostic level7-same-playback-atari-off-20261117.jsonl --level7-framebuffer-diagnostic level7-same-playback-atari-off-20261117.jsonl
```

**COMMAND B — Atari ON**

```powershell
dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --playback-file TEMP\GameplayRecordings\Level-7-slowmo.json --atari-presentation --level7-visible-runtime-diagnostic level7-same-playback-atari-on-20261117.jsonl --level7-framebuffer-diagnostic level7-same-playback-atari-on-20261117.jsonl
```
