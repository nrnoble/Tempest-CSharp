# Level 7 framebuffer CLI runtime contradiction

Date: 2026-11-17
Status: **ROOT CAUSE IDENTIFIED; SOURCE FIX NOT APPLIED**

## Result

The option exists in the current worktree and its standalone parser accepts it.
The failure occurs later: when `--playback-file` is present,
`DebugController.TryParseGameplayFileCommand` scans the gameplay-file arguments
and does not consume `--level7-framebuffer-diagnostic`. Its generic unknown
argument branch emits the exact message Neal saw.

This is a source-level parser integration gap, not evidence of a stale or
Release-only artifact. The current default Debug `dotnet run` path reproduced
the exact error. A source change is required, so no source or behavior change
was made and the report stops before the framebuffer A/B.

## Required answers

1. **Is the option present in current source?** Yes. It is parsed and validated
   by `TryParseLevel7FramebufferDiagnostic`; `Program` invokes that parser.
   The project uses SDK default compile items, and MSBuild's evaluated Compile
   items include `DebugController.cs`, `Level7FramebufferDiagnostic.cs`, and
   `Program.cs`.

2. **Which parser should recognize it?** The dedicated framebuffer parser
   recognizes and validates its filename. For a playback command, the
   subsequent `TryParseGameplayFileCommand` argument scan must also consume
   the option and its value so the gameplay-file allow-list does not reject
   it.

3. **Which parser emitted Neal's error?** The generic unknown-argument branch
   inside `TryParseGameplayFileCommand` (`DebugController.cs`, line 1067).
   `Program.cs` calls this parser after the dedicated framebuffer parse. When
   playback is selected, the loop recognizes the visible-runtime option but
   has no corresponding framebuffer-option branch; it therefore falls
   through to the generic error.

4. **What does normal `dotnet run` target?** MSBuild evaluation from the
   repository root reported `Configuration=Debug`,
   `TargetFramework=net9.0`, `OutputPath=bin\Debug\net9.0\`,
   `TargetPath=...\src\Tempest.Game\bin\Debug\net9.0\Tempest.Game.dll`,
   `UseAppHost=true`, and `OutputType=Exe`. Thus normal `dotnet run` uses the
   Debug output, building it as needed; the corresponding apphost is
   `Tempest.Game.exe`.

5. **Does that artifact correspond to the current option source?** The
   current Debug DLL and apphost were written at 04:44 on Nov. 17, after the
   relevant worktree sources (Program at 04:32, DebugController at 04:29,
   framebuffer diagnostic at 04:37). The DLL contains the framebuffer
   parser's validation string. A bounded `dotnet run --no-build` against that
   default Debug output reproduced the exact unknown-option message. This
   establishes that the current Debug artifact includes the diagnostic parser
   yet exhibits the same missing playback-argument handling. No historical
   launch log or hash was available to fingerprint Neal's individual process,
   but stale output is not needed to explain the observed behavior.

6. **Is the old locked process relevant?** No current relevant Tempest.Game or
   dotnet process was found at investigation start or end. The prior report's
   PID 104920 lock is historical; the current Debug apphost and DLL have newer
   write times. A lock could prevent a build from replacing an executable,
   but it does not explain the exact parser error reproduced from the current
   Debug output. Do not terminate any process to address this issue.

7. **Why did earlier validation pass?**
   - **SOURCE VALIDATION:** the option's dedicated parser, Program dispatch,
     and framebuffer capture integration are present.
   - **TEST-ASSEMBLY VALIDATION:** the reported 284 focused tests passed, but
     the CLI test does not cover this playback combination. In
     `Level7VisibleRuntimeDiagnosticTests`, the composed `TryParseGameplayFileCommand`
     case has no `--playback-file`; with no file mode selected, that parser
     returns before scanning its gameplay-file allow-list. The OFF/ON context
     tests validate context booleans, not the complete playback argument
     dispatch.
   - **RELEASE BUILD VALIDATION:** the prior Release build proves compilation,
     not that normal Debug `dotnet run` accepts these arguments.
   - **DEBUG/DOTNET-RUN RUNTIME VALIDATION:** it was explicitly not run in the
     prior report. The current Debug route now reproduces the failure.

   Therefore the prior Release build and focused tests can be correct as
   reported, while the conclusion that the supplied playback command was
   usable was incomplete. The cause is the missing parser integration branch,
   not a demonstrated Debug/Release artifact mismatch.

8. **Proven root cause:** `TryParseGameplayFileCommand` lacks a case to
   consume `--level7-framebuffer-diagnostic` and its filename in the
   playback-options loop. This is the exact branch that emits Neal's message.

9. **Does resolution require source changes?** Yes. The smallest proposed
   correction is to have that loop consume the framebuffer option's value,
   alongside its existing handling for `--level7-visible-runtime-diagnostic`.
   The dedicated parser already validates the framebuffer filename. This
   production-source change was not applied; Neal's separate implementation
   decision is required.

10. **What must Neal do now?** Do not rerun the A/B or close a game process.
    Decide whether to authorize the small parser-wiring source fix as a
    separate implementation task. The issue is not resolved for experiment
    use until that change and a safe runtime recognition check are authorized
    and completed.

11. **OFF/ON commands:** none are supplied as ready-to-run commands here.
    Their recognition has not been validated after a fix, and the instruction
    was not to ask Neal to repeat the experiment until the contradiction is
    resolved.

## Safe reproduction and validation

The only runtime probe was:

```powershell
dotnet run --no-build --project src\Tempest.Game\Tempest.Game.csproj -- --playback-file C:\__tempest_framebuffer_cli_probe_20261117_0448_no_such_recording.json --level7-visible-runtime-diagnostic runtime-probe.jsonl --level7-framebuffer-diagnostic framebuffer-probe.jsonl
```

The deliberately nonexistent playback path ensured that, if argument
validation accepted the command, playback would fail before window/gameplay
startup. The process exited with code 2 and printed exactly:

```text
Unknown gameplay file option or argument '--level7-framebuffer-diagnostic'.
```

No interactive game was launched. The expected probe JSONL and framebuffer
output paths were confirmed absent afterward. No framebuffer captures were
generated. No build was requested by the probe (`--no-build`).

## Preserved Git/process state

- Branch: `master`; HEAD:
  `385f603a94391756b2ca6cccd524dc17b281e782`; upstream: `origin/master`.
- Index: empty at start and end.
- Worktree: the same 17 modified tracked paths and 21 untracked paths were
  present at start and end. Relevant untracked diagnostic sources/tests
  included `src/Tempest.Game/Debug/Level7FramebufferDiagnostic.cs`,
  `src/Tempest.Game/Debug/Level7VisibleRuntimeDiagnostic.cs`,
  `tests/Tempest.Game.Tests/Level7FramebufferDiagnosticTests.cs`, and
  `tests/Tempest.Game.Tests/Level7VisibleRuntimeDiagnosticTests.cs`. All
  unrelated modifications and untracked material were left untouched.
- No relevant Tempest.Game/dotnet process was running at either process
  snapshot. The prior report's PID 104920 was not running in this inspection.
- No source, tests, build outputs, playback files, or diagnostic outputs were
  changed. No files were staged or committed. Only the required ignored
  prompt and report records were written under `AI-Logs/`.

## Evidence paths

- `src/Tempest.Game/Program.cs`: standalone option parse at lines 103–105;
  later gameplay-file dispatch at lines 250–253.
- `src/Tempest.Game/Debug/DebugController.cs`: dedicated framebuffer parser
  beginning at line 140; gameplay-file parser beginning at line 819; existing
  visible-runtime handling at lines 1013–1018; generic error at line 1067.
- `tests/Tempest.Game.Tests/Level7VisibleRuntimeDiagnosticTests.cs`: the
  composed option test at lines 222–229 has no playback mode; the separate
  framebuffer validation assertions do not execute the playback argument
  scan.
- `AI-Logs/Reports/2026-11-17-level7-same-playback-atari-off-on-framebuffer-ab-report.md`:
  previous Release build, test count, and supplied A/B commands.
- `AI-Logs/Reports/2026-11-17-level7-framebuffer-readback-instrumentation-report.md`:
  prior Debug executable lock and alternate Release validation.

Manual A/B capture remains **NOT RUN** pending a separately authorized source
fix and safe runtime validation.
