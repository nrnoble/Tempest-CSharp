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

Implement and validate the already-established narrow parser-wiring correction
for:

--level7-framebuffer-diagnostic

when used together with:

--playback-file

This is a CLI integration correction only.

Do NOT change gameplay, presentation, playback behavior, framebuffer capture
behavior, projection, geometry, timing, or the Level 7 investigation itself.

ESTABLISHED ROOT CAUSE

Read:

AI-Logs/Reports/2026-11-17-framebuffer-cli-runtime-contradiction-report.md

Also read current project instructions.

The previous read-only investigation established the exact failure:

1. --level7-framebuffer-diagnostic exists in current source.

2. Its dedicated parser TryParseLevel7FramebufferDiagnostic recognizes and
   validates it.

3. Program invokes that dedicated parser.

4. When --playback-file is present, Program subsequently calls
   DebugController.TryParseGameplayFileCommand.

5. TryParseGameplayFileCommand scans the gameplay-file arguments.

6. That loop already consumes:
   --level7-visible-runtime-diagnostic
   and its value.

7. It does NOT consume:
   --level7-framebuffer-diagnostic
   and its value.

8. The framebuffer option therefore falls through to
   TryParseGameplayFileCommand's generic unknown-argument branch.

9. That branch produced Neal's exact runtime error:

   Unknown gameplay file option or argument
   '--level7-framebuffer-diagnostic'.

10. The failure was reproduced against the current Debug artifact.

11. Stale binaries and the historical locked Tempest.Game process were ruled
    out as explanations.

12. The previous validation gap was also identified: focused tests exercised
    the dedicated framebuffer parser/context but did not exercise the complete
    --playback-file argument scan containing both diagnostic options.

AUTHORIZED CORRECTION

Apply the smallest source correction justified by that evidence:

Update TryParseGameplayFileCommand so its gameplay-file argument loop consumes:

--level7-framebuffer-diagnostic

and the option's following filename/value in the same integration role as the
existing:

--level7-visible-runtime-diagnostic

handling.

Do not duplicate framebuffer filename validation unnecessarily.

The dedicated framebuffer parser already owns framebuffer-specific validation.

Do not redesign the parser.

Do not add aliases.

Do not rename the option.

Do not broaden accepted unrelated gameplay-file options.

Do not change command semantics.

REGRESSION TEST

Add or update narrowly targeted automated coverage that exercises the ACTUAL
failing combination.

The regression test must include:

--playback-file <recording>
--level7-visible-runtime-diagnostic <trace>
--level7-framebuffer-diagnostic <capture-name>

through the same TryParseGameplayFileCommand path that previously rejected
the framebuffer argument.

The test must establish that:

- the framebuffer option and its value are consumed by the gameplay-file
  argument scan;
- normal playback-file parsing remains valid;
- the visible-runtime diagnostic option continues to work;
- unknown unrelated arguments remain rejected;
- missing framebuffer values remain governed by the existing dedicated
  framebuffer validation rather than an invented second validation model.

Where existing tests already cover some of these invariants, extend the
smallest appropriate test set rather than duplicating broad coverage.

SAFE RUNTIME RECOGNITION VALIDATION

After implementation and normal automated validation, prove that the actual
default Debug `dotnet run` execution path now recognizes the full framebuffer
playback option combination.

Do NOT launch the interactive game.

Use the same safe strategy established by the previous investigation:

invoke the full playback argument combination using a deliberately
nonexistent playback-file path so that, if argument parsing succeeds,
execution fails at the missing recording before window/gameplay startup.

The validation must distinguish:

EXPECTED:
the command gets past gameplay-file option validation and fails because the
deliberately nonexistent playback recording cannot be opened/found.

NOT ACCEPTABLE:
Unknown gameplay file option or argument
'--level7-framebuffer-diagnostic'.

No Raylib window or gameplay should be launched by this probe.

Use an explicit command timeout.

Confirm that the probe does not create framebuffer captures or misleading
runtime diagnostic output.

BUILD / TEST VALIDATION

Perform bounded validation appropriate to this small parser correction.

At minimum:

1. build the current game using the normal configuration needed to validate
   Neal's default dotnet run path;

2. run the focused parser/CLI tests including the new full playback
   combination regression;

3. run the immediately relevant framebuffer/visible-runtime diagnostic tests;

4. if practical and proportionate, run the existing focused validation set
   associated with the framebuffer A/B milestone;

5. run git diff --check;

6. verify diagnostics-disabled behavior remains unaffected by this parser
   wiring change.

Use explicit test/build hang timeouts.

If Windows Application Control or another security mechanism blocks an
artifact, do not weaken security or retry the same blocked artifact
indefinitely.

If an existing running process locks the current required artifact, do not
kill it. Report the process/artifact and stop as BLOCKED if no materially
different safe validation route can establish the required runtime behavior.

SOURCE SCOPE

Expected production source scope should be extremely small and centered on:

src/Tempest.Game/Debug/DebugController.cs

Test changes should be limited to the most appropriate existing parser or
diagnostic test file(s).

If substantially broader production changes appear necessary, STOP before
making them and explain why the established root-cause model was incomplete.

DO NOT MODIFY

Do not change:

- Level 7 simulation;
- Tanker behavior;
- player shots;
- Blaster movement;
- recorded-input behavior;
- recording format;
- replay determinism;
- Atari presentation mapping;
- legacy presentation;
- record-06 geometry;
- perspective;
- camera;
- framebuffer readback implementation;
- framebuffer boundary/window selection;
- capture limits;
- capture PNG format;
- renderer geometry diagnostics;
- timing behavior;
- mapped-Flipper crash correction.

Do not perform additional visual analysis.

Do not open screenshots, framebuffer PNGs, videos, frames, contact sheets, or
other visual assets.

Do not inspect TEMP/myNotes.md.

Do not inspect protected ROM copies.

GIT PRESERVATION

The worktree contains substantial uncommitted presentation and diagnostic
work.

Record initial Git state.

Preserve:

- branch;
- HEAD;
- upstream;
- index;
- all pre-existing tracked modifications;
- all pre-existing untracked files.

Do not:

- stage;
- commit;
- push;
- reset;
- restore;
- stash;
- rebase;
- clean;
- checkout;

or otherwise rewrite existing Git/worktree state.

Do not overwrite or delete previous diagnostic evidence.

MANUAL EXPERIMENT HANDOFF

Only if the parser correction, tests, build, and safe default-Debug runtime
recognition probe all PASS:

provide Neal with the two exact ready-to-run commands for the controlled
same-recording framebuffer experiment.

They should remain equivalent to the intended experiment:

COMMAND A — Atari OFF

dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --playback-file TEMP\GameplayRecordings\Level-7-slowmo.json --level7-visible-runtime-diagnostic level7-same-playback-atari-off-20261117.jsonl --level7-framebuffer-diagnostic level7-same-playback-atari-off-20261117.jsonl

COMMAND B — Atari ON

dotnet run --project src\Tempest.Game\Tempest.Game.csproj -- --playback-file TEMP\GameplayRecordings\Level-7-slowmo.json --atari-presentation --level7-visible-runtime-diagnostic level7-same-playback-atari-on-20261117.jsonl --level7-framebuffer-diagnostic level7-same-playback-atari-on-20261117.jsonl

Do not hand these commands back as validated if the safe runtime recognition
probe has not actually passed.

If existing output destinations from the failed attempt would prevent
CreateNew behavior, determine whether they actually exist before changing the
commands.

Do NOT delete an existing diagnostic destination automatically.

If unique names are required, provide Neal updated OFF/ON names.

AI LOGGING

This is substantial Tempest corrective work.

Save this task as:

AI-Logs/Prompts/2026-11-17-framebuffer-playback-parser-wiring-fix-prompt.md

Write the paired report as:

AI-Logs/Reports/2026-11-17-framebuffer-playback-parser-wiring-fix-report.md

The report must record:

- established root cause;
- exact parser change;
- exact regression-test coverage;
- files changed;
- build/test results;
- safe Debug runtime-recognition probe and its exact outcome;
- whether interactive gameplay was launched;
- whether diagnostic output was created;
- initial/final Git state;
- explicit confirmation that no gameplay/presentation/framebuffer behavior
  was changed;
- whether the OFF/ON commands are now validated and ready for Neal.

AI-Logs remain ignored and must not be staged.

Do not rewrite previous historical reports.

Do not update durable gameplay/presentation documentation for this parser-only
correction.

SUCCESS CONDITION

Success requires ALL of the following:

- the proven missing parser-wiring branch is corrected;
- a regression test exercises the actual --playback-file combination;
- focused automated validation passes;
- the current default Debug dotnet run path demonstrably recognizes
  --level7-framebuffer-diagnostic in the complete playback command;
- the safe probe reaches the deliberately nonexistent recording failure
  instead of rejecting the framebuffer option;
- no game window is launched;
- no unrelated behavior changes are made;
- Git state remains preserved.

Only then hand Neal the exact OFF/ON experiment commands.

Stop after validation and handoff.

TEMPEST_FRAMEBUFFER_PLAYBACK_PARSER_WIRING_FIX