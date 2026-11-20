READ THE ENTIRE USER-PROVIDED TASK BEFORE TAKING ANY ACTION.

Before using tools, opening files, running commands, inspecting repository
content, or modifying anything, locate the unique completion token near the
end of this user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on
a line by itself in the user-provided task. VS Code/agent-generated reminders,
tagged-file information, attachment metadata, or other system-generated
context may appear after the user's token and do not invalidate it.

This check is intended to detect an incomplete or truncated USER task, not
appended agent context.

If exactly one complete TEMPEST_ token cannot be found, STOP before any
project or repository action and explain why.

TASK

Perform a tightly bounded investigation of a runtime/implementation
contradiction in the newly added Level 7 framebuffer diagnostic.

Do NOT continue the framebuffer experiment yet.

Do NOT change behavior merely to make the command work.

First determine exactly why the runtime invoked by Neal rejected an option
that the immediately preceding implementation report says was implemented,
built, tested, and supplied in the exact manual handoff command.

OBSERVED FAILURE

Neal ran this exact command from:

C:\Users\nrnob\source\repos\Tempest

dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --playback-file TEMP\GameplayRecordings\Level-7-slowmo.json --level7-visible-runtime-diagnostic level7-same-playback-atari-off-20261117.jsonl --level7-framebuffer-diagnostic level7-same-playback-atari-off-20261117.jsonl

The process immediately reported:

Unknown gameplay file option or argument '--level7-framebuffer-diagnostic'.

Neal stopped at that point.

He did NOT run the Atari-ON command.

Do not ask him to retry until this contradiction is understood.

RELEVANT REPORT

Read:

AI-Logs/Reports/2026-11-17-level7-same-playback-atari-off-on-framebuffer-ab-report.md

Also read the immediately preceding framebuffer instrumentation report only
as needed.

The latest report states that:

- the existing framebuffer diagnostic was generalized for Atari OFF and ON
  recorded playback;
- --level7-framebuffer-diagnostic is supported;
- Release build passed;
- 284 focused tests passed;
- both Atari OFF and Atari ON CLI configurations were tested;
- the exact failed command above was supplied as COMMAND A for Neal.

The observed runtime therefore conflicts with the report.

PURPOSE

Determine which artifact/source/parser actually handled Neal's dotnet run
command and why it did not recognize:

--level7-framebuffer-diagnostic

Do not guess.

Establish the evidence chain from:

current source
-> project file/build configuration
-> dotnet run behavior
-> actual executable/assembly invoked
-> argument parser that produced the observed error.

FIRST PRESERVE STATE

Record:

- current branch;
- HEAD;
- upstream;
- index status;
- tracked modifications;
- relevant untracked files;
- currently running Tempest.Game/dotnet processes relevant to this diagnosis.

Do not stop or kill an existing process merely because it exists.

Do not reset, restore, stash, clean, checkout, stage, commit, or push.

SOURCE VERIFICATION

Inspect the CURRENT worktree source and establish:

1. Does the literal option string
   --level7-framebuffer-diagnostic
   exist in the current source?

2. Which source file parses it?

3. Is that source file included in
   src\Tempest.Game\Tempest.Game.csproj?

4. What exact validation/dispatch path should recognize the option?

5. What code emits:
   "Unknown gameplay file option or argument ..."

6. Can that error path execute despite the framebuffer option being present
   elsewhere in the current source?

Do not modify source while answering these questions.

BUILD/RUNTIME VERIFICATION

Determine what `dotnet run --project src\Tempest.Game\Tempest.Game.csproj`
would actually execute from this worktree.

Inspect relevant build output metadata/artifacts read-only.

Establish whether the runtime artifact corresponds to the current modified
source containing the framebuffer option.

If practical, compare timestamps/hashes/build metadata sufficient to identify
the artifact without relying on assumptions.

Be alert to:

- Debug versus Release differences;
- stale build output;
- a locked executable or assembly;
- incremental-build behavior;
- multiple Tempest.Game artifacts;
- project/output-path mismatches;
- parser implementations that differ by execution path;
- command-line preprocessing before the intended diagnostic parser.

These are investigation possibilities only.

Do not assume any one is the cause.

IMPORTANT PRIOR EVIDENCE

The previous framebuffer instrumentation report recorded that an initial
Debug-config test build could not replace:

src\Tempest.Game\bin\Debug\net9.0\Tempest.Game.exe

because an already-running Tempest.Game process had the executable locked.

That report then validated through an alternate Release route.

Treat that as relevant evidence to investigate, NOT as proof of the cause.

Determine whether that condition actually explains Neal's current
`dotnet run` behavior.

Do not kill a process or delete build output merely to test the theory.

SAFE REPRODUCTION

If the failure can be reproduced without launching interactive gameplay,
perform the smallest bounded CLI reproduction necessary.

Prefer argument-validation/help/error paths that exit before opening the
interactive game.

Do NOT launch normal gameplay.

Do NOT generate framebuffer captures.

Do NOT create another large diagnostic.

Use explicit command timeouts.

If a command might launch the interactive game rather than failing
immediately, do not run it.

VALIDATION REPORT CONTRADICTION

Determine how the previous milestone was able to report:

- Release build success;
- focused CLI test success;
- Atari OFF/ON diagnostic configuration success;

while Neal's normal `dotnet run` rejected the option.

Explicitly distinguish:

SOURCE VALIDATION
TEST-ASSEMBLY VALIDATION
RELEASE BUILD VALIDATION
DEBUG/DOTNET-RUN RUNTIME VALIDATION

if those are materially different in this situation.

The goal is to determine whether the previous report was:

- correct but validated a different artifact/configuration;
- incomplete because normal `dotnet run` was not actually exercised;
- contradicted by stale/locked runtime output;
- or affected by some other proven mechanism.

Do not accuse the previous report of being wrong unless evidence establishes
that.

NO BEHAVIOR CHANGES DURING DIAGNOSIS

Before identifying the cause, do NOT:

- edit parser logic;
- add another CLI alias;
- rename the option;
- rebuild/delete arbitrary artifacts;
- kill running processes;
- change Debug/Release defaults;
- change gameplay;
- change presentation;
- change playback;
- change framebuffer behavior.

This first phase is diagnostic.

IF ROOT CAUSE IS IDENTIFIED

If the exact cause is identified and the fix is purely development/build/CLI
plumbing required to make the already implemented diagnostic accessible,
report the proposed smallest correction BEFORE applying it.

Then:

- if the correction requires production/source modification, STOP and hand
  the proposed change to Neal for a separate implementation decision;
- if no source correction is required and a safe existing build/runtime route
  already exposes the implemented option, validate that route without
  launching gameplay and provide Neal the corrected command.

Do not silently modify source in this task.

IF A LOCKED PROCESS IS THE CAUSE

If an existing Tempest.Game process is proven to be preventing the current
Debug runtime artifact from being updated:

- identify the PID/process and exact locked artifact;
- do NOT terminate it;
- state clearly that Neal must decide whether to close that existing game
  process;
- do not claim the command is fixed until the intended current artifact can
  actually be validated.

IF STALE BUILD OUTPUT IS THE CAUSE

If stale output is proven:

- explain exactly why `dotnet run` selected it;
- do not use `git clean`, reset, or destructive cleanup;
- recommend the smallest ordinary safe rebuild route;
- validate command-line recognition only if this can be done without
  launching gameplay.

DO NOT REOPEN THE LEVEL 7 BUG

This task is not another investigation of:

- Tanker motion;
- shot motion;
- simulation state;
- Atari geometry;
- framebuffer pixels;
- playback determinism;
- perspective;
- Level 7 tube presentation.

Those investigations remain preserved.

This task concerns only why the newly documented framebuffer CLI was not
recognized at runtime.

VALIDATION / BOUNDS

All commands must have explicit practical timeouts.

Do not wait on stalled build/test processes.

If Windows security blocks an artifact, follow the existing project security
policy and stop/report rather than bypassing it.

Do not report a failed, blocked, canceled, stale, or unexecuted runtime path
as successfully validated.

AI LOGGING

This is substantial Tempest diagnostic work.

Save this prompt as:

AI-Logs/Prompts/2026-11-17-framebuffer-cli-runtime-contradiction-prompt.md

Save the report as:

AI-Logs/Reports/2026-11-17-framebuffer-cli-runtime-contradiction-report.md

AI-Logs remain ignored and must not be staged.

Do not rewrite historical reports.

Do not update durable gameplay/presentation documentation.

REPORT REQUIRED ANSWERS

The final report must answer:

1. Is --level7-framebuffer-diagnostic present in current source?

2. Which parser should recognize it?

3. Which parser actually emitted Neal's observed error?

4. What artifact/configuration did Neal's normal dotnet run execute or
   attempt to execute?

5. Was that artifact built from the current source containing the option?

6. Is an existing running process or locked artifact relevant?

7. Why did previous Release/test validation pass while Neal's command failed?

8. What is the proven root cause?

9. Does resolving the issue require source changes?

10. What exact action, if any, must Neal take?

11. What exact OFF/ON commands should Neal use afterward, but only if their
    runtime option recognition has now been validated safely?

Do not ask Neal to repeat the framebuffer experiment until the runtime
contradiction is resolved.

Stop after the report.

TEMPEST_FRAMEBUFFER_CLI_RUNTIME_CONTRADICTION