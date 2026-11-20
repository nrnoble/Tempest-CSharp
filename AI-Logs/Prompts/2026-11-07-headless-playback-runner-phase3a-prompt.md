FAIL-CLOSED COMPLETENESS CHECK

Read this entire user-provided task before using tools, reading files, running
commands, modifying files, staging, committing, pushing, or taking repository
action.

Before anything else, locate the unique completion token near the end.

Require exactly one complete standalone token beginning with "TEMPEST_" on a
line by itself.

VS Code/agent-generated context after the user's token does not invalidate it.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before
any repository action.


TEMPEST HEADLESS RECORDED-PLAYBACK RUNNER — PHASE 3A

Implement Phase 3A of the accepted recorded-playback analysis/debugger roadmap.

PURPOSE

Give automated tests and VS Code Copilot a reusable deterministic facility that
can load a recorded Tempest gameplay file, execute the same accepted production
playback simulation WITHOUT opening the Raylib game window, stop at an exact
simulation boundary, and return structured readable gameplay state.

This must use the same GameplayPlaybackSession introduced in Phase 1 and used by
accepted visible playback in Phase 2.

Do not create a second replay engine.

This milestone should provide:

1. reusable in-process headless playback API;
2. thin DEVELOPMENT-ONLY command-line wrapper;
3. run-to-exact-boundary support;
4. human-readable and structured GameplayStateSummary output;
5. clear success/failure exit behavior;
6. optional checkpoint/assertion seams where narrowly appropriate.

Do NOT implement backward stepping.

Do NOT implement rewind.

Do NOT implement snapshots.

Do NOT implement state restoration.

Do NOT change visible playback controls.

Do NOT modify recording format.

Do NOT begin Level 8/9 work.


ACTIVE REPOSITORY

Work only in:

C:\Users\nrnob\source\repos\Tempest


EXPECTED CLEAN BASELINE

Expected branch:

master

Expected HEAD and origin/master:

358fca9229153e4f0fefe51fbedd711696095955
Add visible recorded playback controls

Require:

- clean tracked worktree;
- clean index;
- master tracking origin/master;
- local HEAD matching origin/master;
- no unexplained nonignored untracked files.

Before modifying anything run:

git status --short --branch
git rev-parse HEAD
git log -1 --oneline
git rev-parse origin/master
git diff --stat
git diff --cached --stat
git ls-files --others --exclude-standard
git remote -v

If anything differs unexpectedly, STOP rather than resetting, cleaning,
restoring, deleting, discarding, or overwriting anything.


READ FIRST

Read:

.github/copilot-instructions.md
README.md
TEMPEST-DEVELOPMENT-NOTES.md
TEMPEST-BEHAVIOR-SPEC.md

Read relevant reports:

AI-Logs/Reports/2026-11-06-recorded-playback-debugger-headless-analysis-report.md

AI-Logs/Reports/2026-11-07-shared-playback-session-phase1-closeout-report.md

AI-Logs/Reports/2026-11-07-visible-playback-controls-phase2-acceptance-closeout-report.md

If the exact Phase 2 closeout report filename differs, locate the accepted
Phase 2 closeout report rather than assuming it is absent.

Inspect current source/tests including:

Program.cs

DebugController.cs

GameplayPlaybackSession.cs

GameplayStateSummary.cs

GameplayRecordingFile.cs

GameplayStartupConfiguration.cs

PlayerInputReplay.cs

VisiblePlaybackController.cs

InputSystem.cs

TempestGame.cs

GameplayPlaybackSessionTests.cs

GameplayRecordingTests.cs

GameplayPlaybackTests.cs

VisiblePlaybackControllerTests.cs

and relevant direct-start/progression/death/ledger/enemy tests.

Use current source as authority.


PROTECTED ACCEPTED FOUNDATION

Preserve exactly:

- recording v1 compatibility;
- recording v2 compatibility;
- strict recording-file validation;
- recorded initial seed;
- recorded starting production level;
- recorded lives;
- recorded rim-speed multiplier;
- one recorded gameplay frame per simulation tick;
- restart bits;
- PlayerInput meanings;
- production TempestGame.RunOneStep behavior;
- GameplayPlaybackSession boundary semantics;
- StepForward semantics;
- RunToBoundary semantics;
- GameplayStateSummary;
- production progression;
- RNG initialization/order;
- visible playback fidelity;
- Space pause/resume;
- Right +1 scrubbing;
- Right native-repeat scrubbing;
- Shift+Right +5;
- accepted live-input handoff.

No accepted visible playback behavior should depend on the new headless runner.


SHARED EXECUTION REQUIREMENT

The headless runner MUST use GameplayPlaybackSession.

Conceptually:

recording file
-> GameplayRecordingFile validation/load
-> recorded GameplayStartupConfiguration
-> GameplayPlaybackSession
-> production TempestGame
-> ordinary recorded frames
-> RunOneStep
-> exact requested boundary
-> GameplayStateSummary

Do not rebuild the replay loop independently in the CLI.

Do not duplicate input gating.

Do not create a "test simulation."


HEADLESS MEANS HEADLESS

Headless execution must NOT initialize:

- Raylib window;
- renderer;
- OpenGL;
- visible playback controls;
- Raylib keyboard/mouse input;
- normal frame pacing;
- gameplay display.

Use the existing silent audio path.

Do not initialize the normal Raylib audio device merely for headless playback.

The runner should execute simulation as quickly as appropriate rather than
waiting for 60-Hz real time.


REUSABLE IN-PROCESS API

Introduce the smallest reusable orchestration API needed above
GameplayPlaybackSession.

Use project-consistent names, conceptually something like:

GameplayPlaybackRunner

HeadlessGameplayPlaybackRunner

or equivalent.

The reusable API should be usable directly by automated tests without spawning
another process.

It should accept a validated recording or recording path through an appropriate,
safe boundary.

It should support:

- run to recording completion;
- run to requested boundary N;
- retrieve resulting GameplayStateSummary;
- expose completion reason/status;
- propagate simulation/invariant failures.


BOUNDARY SEMANTICS

Preserve the Phase 1 accepted contract:

Boundary 0:
recorded startup before frame 0.

After executing frame 0:
Boundary 1.

After executing D frames:
Boundary D.

For requested target N:

0 <= N <= DurationTicks.

Headless run-to-boundary N must stop EXACTLY at boundary N.

Reject:

- N < 0;
- N > duration.

Do not clamp.

Do not silently round.

Do not consume frame N when stopping at boundary N.


RUN TO COMPLETION

When no target boundary is specified:

run through all recorded frames and stop at recording duration.

Do not stop prematurely merely because game state reaches:

- PlayerDying;
- Game Over;
- Level Complete;
- approach;
- exit;
- another non-Playing state.

The recording's remaining frames remain authoritative.

Do not perform visible live-input handoff in headless playback.


NO LIVE INPUT

Headless playback is deterministic recording analysis.

It must not:

- poll keyboard;
- poll spinner/mouse;
- poll controller;
- accept live PlayerInput;
- resume interactive gameplay after completion.

At recording completion, return a result.


STATE SUMMARY

Reuse GameplayStateSummary rather than creating another unrelated debugger
model.

At minimum, headless result should expose:

- requested boundary;
- achieved boundary;
- recording duration;
- remaining frames;
- completion status/reason;
- GameplayStateSummary.

Do not return live mutable TempestGame entities as public diagnostic output.


STRUCTURED OUTPUT

The thin CLI should support structured machine-readable state output.

Preferred:

JSON

using a stable explicit diagnostic schema based on GameplayStateSummary.

Also provide concise human-readable output when JSON is not requested, if that
can remain small and maintainable.

Do not serialize internal object graphs indiscriminately.

Do not expose private implementation details simply because JSON serialization
can reach them.

Use explicit diagnostic DTO/record structure.


CLI

Add a DEVELOPMENT-ONLY command-line surface.

Choose clear syntax consistent with existing options.

Preferred conceptual grammar:

dotnet run --project src\Tempest.Game -- \
  --headless-playback <recording-file>

Optional exact-boundary inspection:

dotnet run --project src\Tempest.Game -- \
  --headless-playback <recording-file> \
  --boundary 4820

Optional structured output:

dotnet run --project src\Tempest.Game -- \
  --headless-playback <recording-file> \
  --boundary 4820 \
  --json

Use actual option names only after inspecting current parser conventions.

If a materially better project-consistent naming convention exists, use it and
document it.

Do not overload --playback-file in a way that ambiguously switches visible and
headless behavior.


CLI CONFLICTS

Headless playback owns simulation startup from the recording.

Reject incompatible simulation overrides such as:

--level
--lives
--rim-speed
special scenario/test modes

just as deterministic file playback already protects its simulation identity.

Do not silently ignore conflicts.

Observation/debug output options may compose only when clearly safe.


EXIT STATUS

Define explicit process results.

At minimum distinguish:

SUCCESS:
recording loaded and requested boundary/completion reached.

INVALID COMMAND:
malformed CLI or incompatible options.

INVALID RECORDING:
format/validation/load failure.

SIMULATION FAILURE:
unhandled gameplay/invariant exception.

OUT-OF-RANGE BOUNDARY:
invalid requested target.

ASSERTION/CHECKPOINT FAILURE:
only if an assertion facility is actually included now.

Use nonzero status for failures.

Do not catch simulation exceptions merely to return success.

If exceptions are caught solely to print structured failure information, retain
the full exception and return nonzero.


ERROR VISIBILITY

Preserve the project's recently validated fail-visible philosophy.

A gameplay exception should remain diagnosable with:

- type;
- message;
- stack trace;
- inner exception where relevant;
- nonzero result.

Do not hide failures behind a generic "playback failed" line.


VS CODE COPILOT WORKFLOW

The practical goal is that VS Code Copilot can execute commands like:

run recording to boundary N
-> inspect structured state
-> compare nearby boundaries
-> reason about gameplay state
-> add/run assertions as appropriate

The AI should not need Raylib or a visible window.

Document example diagnostic workflows such as:

BOUNDARY N:

headless execution -> JSON summary.

N-1 / N / N+1:

run separate deterministic executions -> compare state.

FIRST SUSPECTED CHANGE:

inspect surrounding summaries and then write a focused automated regression if
appropriate.

Do not implement automated first-divergence comparison yet unless it falls out
trivially from tests.

That remains future tooling.


CHECKPOINT CALLBACK/API

Evaluate a narrowly reusable in-process checkpoint seam.

A caller may need to observe selected boundaries during one headless execution.

If simple, allow an optional read-only callback/observer receiving:

- boundary number;
- GameplayStateSummary.

The observer must not mutate TempestGame.

Do not create a complex event framework.

The runner's basic run-to-boundary operation remains primary.


NO ASSERTION LANGUAGE

Do not invent a DSL for assertions.

Automated tests can assert against typed GameplayStateSummary values.

VS Code Copilot can generate focused tests using the reusable API.

The CLI may expose state; it does not need an expression evaluator.


SUMMARY SCHEMA REVIEW

Review whether Phase 1 GameplayStateSummary is sufficient for meaningful AI
inspection.

Do NOT massively expand it during Phase 3A.

Only add a field when:

- it is clearly diagnostically valuable;
- current code exposes it through a narrow read-only seam;
- it materially supports realistic playback investigation.

Strong candidates, if missing and cleanly available:

- Flipper identities/direction/pivot data;
- current level/LevelState;
- lives/score;
- shots/bullets;
- Eggs;
- Tankers/cargo;
- Spikes;
- Super Zapper;
- reserve/ledger summary.

Do not expose mutable internals.


DETERMINISM TEST

For one recording:

Runner.RunToBoundary(N)

must produce the same selected GameplayStateSummary fields as:

a fresh GameplayPlaybackSession
-> RunToBoundary(N)
-> GetSummary()

Two fresh headless runs of the same file/build to the same boundary must yield
equivalent selected summary state.

Do not claim cross-build or cross-runtime deterministic compatibility.


VISIBLE-HEADLESS EQUIVALENCE

Add automated evidence that visible-session execution and headless runner
execution use the same GameplayPlaybackSession semantics.

Do not attempt native pixel comparison.

Compare meaningful production simulation state at matching boundaries.


VERSION 1 AND VERSION 2

Test both.

V1 must preserve:

- Level 1;
- 3 lives;
- rim speed 1.0;
- recorded seed/input.

V2 must preserve its explicit startup metadata.

Do not alter either recording schema.


PERFORMANCE

Measure enough to establish that headless playback does not intentionally use
real-time pacing.

Do not turn this into a benchmark milestone.

At minimum verify:

- runner has no TargetFPS or sleep/pacing;
- reaching a target uses sequential simulation execution.

Optionally report rough elapsed time for a test recording if readily available,
but do not create performance promises from one machine/run.


FILE SAFETY

Preserve strict recording parsing.

Headless playback must be read-only with respect to its recording file.

Do not:

- overwrite recording;
- rewrite recording;
- append metadata;
- create sidecars automatically;
- modify recording format.

JSON summary should go to stdout unless the user explicitly requests some
future file-output feature.

No file-output option is needed now.


RECORDING FIXTURE POLICY

Do not commit generated gameplay recordings in this milestone.

Tests should preferably use:

- existing in-memory/synthetic recording fixtures;
- temporary test files;
- deliberately controlled test data.

A future recording-fixture milestone may curate real user recordings after
review.


AUDIO

Use SilentAudioEvents for headless execution.

Do not initialize normal audio hardware.

No playback sound is required from the headless runner.

This must not change simulation results.


NO RAYLIB

Add tests or architectural checks demonstrating the reusable runner itself does
not depend on Raylib initialization.

Do not require OpenGL, window creation, keyboard polling, or audio device setup
for runner tests.


VISIBLE PLAYBACK

Do not change accepted Phase 2 controls:

Space
Right tap
Right repeat
Shift+Right
PAUSED overlay
live handoff

Headless implementation should not affect them.


PHASE 3B ROADMAP

Preserve future backward scrubbing:

Left tap:
-1 tick.

Left hold:
repeated -1.

Shift+Left:
-5 ticks.

Shift+Left hold:
repeated -5 groups.

Initial mechanism:

reconstruct from recorded startup
-> replay deterministically to earlier boundary.

Do not implement it in Phase 3A.


DEFERRED

Do not implement:

- backward step;
- rewind;
- reverse mutation;
- snapshots;
- Random-state serialization;
- fast-forward/speed controls beyond accepted Right-arrow scrubbing;
- seek UI;
- timeline;
- bookmark UI;
- loop UI;
- deterministic hashes;
- first-divergence command;
- playlist/browser;
- attract mode;
- enemy-destruction explosion;
- Level 8/9.


TESTS — RUNNER API

Cover:

1. Load valid v1 recording.

2. Load valid v2 recording.

3. Boundary 0 returns restored startup with no frame consumed.

4. RunToBoundary(1) consumes exactly frame 0.

5. RunToBoundary(N) reaches exactly N.

6. Run-to-completion reaches DurationTicks.

7. Negative boundary rejected.

8. Boundary > duration rejected.

9. No live input occurs.

10. No live handoff occurs.

11. Restart frames preserve accepted behavior.

12. Natural level progression is preserved.

13. State summary matches session state.

14. Repeated fresh runs to same boundary are equivalent.

15. Simulation exceptions remain failures.


TESTS — CLI

Cover:

- valid completion run;
- valid boundary run;
- JSON output;
- malformed boundary;
- missing filename;
- missing boundary value;
- unsupported/conflicting flags;
- invalid recording;
- unsupported version;
- expected nonzero error results;
- no window/audio initialization path.

Use parser/unit seams where possible rather than spawning native windows.


TESTS — JSON SUMMARY

Validate:

- syntactically valid JSON;
- explicit/stable diagnostic structure;
- requested/actual boundary;
- recording duration/remaining/completion;
- selected game-state values;
- Flipper diagnostic values where represented;
- deterministic/stable ordering.

Avoid brittle whitespace assertions.


NEIGHBORING REGRESSION

Run regression covering:

- GameplayPlaybackSession;
- GameplayStateSummary;
- visible Phase 2 controls;
- GameplayRecordingTests;
- GameplayPlaybackTests;
- v1/v2 compatibility;
- input;
- direct startup;
- death/re-entry;
- progression;
- campaign soak;
- Level 5;
- Level 7;
- Tanker/cargo;
- Super Zapper;
- recording completion/live handoff.


DOCUMENTATION

Update durable documentation with Phase 3A implementation candidate:

- reusable headless runner;
- thin CLI;
- same GameplayPlaybackSession as visible playback;
- exact-boundary semantics;
- structured state output;
- SilentAudioEvents;
- no Raylib window;
- read-only recording;
- nonzero failures;
- intended VS Code Copilot workflow;
- recording schemas unchanged;
- Phase 2 visual controls unchanged;
- Phase 3B backward reconstruction still deferred;
- manual acceptance pending only for CLI usability as appropriate.

Do not claim manual acceptance before Neal tests the CLI if manual testing is
needed.


AI LOGS

Preserve task as:

AI-Logs/Prompts/2026-11-07-headless-playback-runner-phase3a-prompt.md

Create:

AI-Logs/Reports/2026-11-07-headless-playback-runner-phase3a-report.md

Report must include:

1. Starting Git state.
2. Runner architecture.
3. Shared-session reuse.
4. Recording loading path.
5. Boundary semantics.
6. Completion semantics.
7. No-live-input policy.
8. No-live-handoff policy.
9. State-summary output.
10. JSON schema.
11. CLI syntax.
12. CLI parser/conflict policy.
13. Exit-code policy.
14. Exception policy.
15. Silent-audio policy.
16. Raylib-independence evidence.
17. v1 behavior.
18. v2 behavior.
19. Visible/headless equivalence.
20. VS Code Copilot workflow.
21. Checkpoint observer, if implemented.
22. File-safety behavior.
23. Production files changed.
24. Test files changed.
25. Documentation changed.
26. Focused test counts.
27. Neighboring regression counts.
28. Full-suite counts.
29. Build warnings/errors.
30. git diff --check.
31. Manual CLI commands/checklist.
32. Phase 3B roadmap.
33. Final Git state.
34. Confirmation no backward step, rewind, snapshots, recording change,
    visible-control change, enemy explosion, Level 8/9, stage, commit or push
    occurred.


AUTOMATED VALIDATION

Run focused Phase 3A tests.

Run neighboring playback/gameplay regressions.

Then run:

dotnet test TempestCSharp.slnx

dotnet build TempestCSharp.slnx

git diff --check

Require:

- 0 failed;
- 0 unexpected skipped;
- build 0 errors;
- report warning count;
- diff check passes.


NO INTERACTIVE GAME LAUNCH

Do not launch the game window.

Headless CLI commands MAY be executed because they must not open the game
window.

Neal will manually inspect CLI usability if needed.

Do not stage.
Do not commit.
Do not push.


MANUAL CLI EXAMPLES

After implementation, provide exact commands based on actual chosen syntax.

Expected conceptual examples:

Run recording to completion:

dotnet run --project src\Tempest.Game -- \
  --headless-playback Temp\GameplayRecordings\testlevel5-3.json

Inspect boundary 4820 as JSON:

dotnet run --project src\Tempest.Game -- \
  --headless-playback Temp\GameplayRecordings\testlevel5-3.json \
  --boundary 4820 \
  --json

Do not assume that specific recording exists; provide generic forms too.


MANUAL CHECKLIST

If CLI testing is useful, ask Neal to verify:

1. No game window opens.

2. No audio device/game audio starts.

3. Recording runs to completion rapidly.

4. Boundary command stops exactly at requested boundary.

5. JSON is readable/useful.

6. Reported level/lives/state make sense for known recording.

7. Invalid boundary reports a clear error and nonzero result.

8. Existing visible playback still behaves normally in a separate manual check
   only if Neal chooses; do not require redundant Phase 2 retesting unless
   automated evidence reveals concern.


FINAL GIT CHECK

Run:

git status --short --branch
git rev-parse HEAD
git rev-parse origin/master
git diff --stat
git diff --name-status
git diff
git diff --check
git diff --cached --stat
git ls-files --others --exclude-standard

Expected HEAD remains:

358fca9229153e4f0fefe51fbedd711696095955

Require candidate unstaged and uncommitted.


FINAL RESPONSE

Return:

1. Runner API.
2. CLI syntax.
3. Shared-session reuse.
4. Boundary behavior.
5. Completion behavior.
6. No-live-input behavior.
7. State-summary output.
8. JSON output.
9. Exit-code policy.
10. Exception behavior.
11. Silent-audio behavior.
12. Raylib independence.
13. v1/v2 compatibility.
14. Visible/headless equivalence.
15. VS Code Copilot workflow.
16. Production files changed.
17. Tests changed.
18. Documentation changed.
19. Focused test counts.
20. Neighboring regression counts.
21. Full-suite counts.
22. Build warnings/errors.
23. git diff --check.
24. Manual headless commands.
25. Manual checklist.
26. Report filename.
27. Phase 3B next step.
28. Final Git status.
29. Confirmation no backward navigation, rewind, snapshots, recording-format
    change, Phase 2 visual-control change, enemy-explosion implementation,
    Level 8/9, stage, commit or push occurred.

Stop for Neal's review.

TEMPEST_HEADLESS_PLAYBACK_RUNNER_PHASE3A_COMPLETE