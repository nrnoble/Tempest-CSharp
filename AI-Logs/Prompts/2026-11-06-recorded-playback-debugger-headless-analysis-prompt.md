FAIL-CLOSED COMPLETENESS CHECK

Read this complete user-provided task before using tools, reading/modifying
repository files, running commands, staging, committing, or pushing.

Before anything else, locate the unique standalone completion token near the end.

Require exactly one complete token beginning with "TEMPEST_" on a line by
itself. VS Code/agent-generated context after that token does not invalidate it.

If exactly one complete standalone token cannot be found, STOP before any
repository action.


TEMPEST RECORDED-PLAYBACK DEBUGGER / HEADLESS RUNNER ANALYSIS

Perform a READ-ONLY preimplementation analysis for extending the accepted
recorded-gameplay system into a deterministic debugger/playback facility.

Long-term desired capabilities:

VISIBLE PLAYBACK
- pause/resume;
- step forward one simulation tick;
- step backward;
- fast forward;
- rewind;
- selectable playback speed;
- pause/run to a selected simulation tick;
- optional read-only diagnostic/state display.

HEADLESS PLAYBACK
- load an existing recording;
- restore its recorded production startup;
- replay without opening the Raylib game window;
- run to a selected simulation tick;
- inspect simulation state;
- execute assertions/checkpoints;
- support VS Code Copilot investigation;
- potentially use curated recordings as deterministic regression fixtures.

ANALYSIS ONLY.

Do not implement anything.
Do not modify tracked production code.
Do not modify tests.
Do not modify durable tracked documentation.
Do not add CLI options or controls.
Do not launch the game.
Do not stage.
Do not commit.
Do not push.
Do not begin Level 8/9 work.


ACTIVE REPOSITORY

C:\Users\nrnob\source\repos\Tempest


EXPECTED BASELINE

Branch:

master

Expected HEAD and origin/master:

7032a7219cde69a0ae25b00e5f3bc9ac1ee3ed04
Use vector text for Avoid Spikes

Require clean tracked worktree/index and local master matching origin/master.

Run:

git status --short --branch
git rev-parse HEAD
git log -1 --oneline
git rev-parse origin/master
git diff --stat
git diff --cached --stat
git ls-files --others --exclude-standard
git remote -v

If unexpected repository state exists, STOP without cleaning, restoring,
resetting, deleting, or discarding anything.


READ FIRST

Read:

.github/copilot-instructions.md
README.md
TEMPEST-DEVELOPMENT-NOTES.md
TEMPEST-BEHAVIOR-SPEC.md

Read relevant AI reports covering:

- deterministic gameplay playback foundation;
- visible playback profiles;
- recorded-input round trip;
- recording/playback v2 startup metadata;
- recording/playback v2 acceptance;
- generic --level N startup;
- Level 5 crash investigation/fix where replay/headless debugging is relevant;
- any prior pause, stepping, headless, deterministic-verification, or playback
  control analysis.

Inspect current source/tests, including as applicable:

Program.cs
DebugController.cs
GameplayPlayback.cs
GameplayRecordingFile.cs
GameplayStartupConfiguration.cs
PlayerInputReplay.cs
PlayerInputRecorder.cs
InputSystem.cs
TempestGame.cs
GameplayPlaybackTests.cs
GameplayRecordingTests.cs
simulation/campaign/death/progression/ledger tests

Use current source as authority.


PROTECTED ACCEPTED FOUNDATION

Do not recommend changing the accepted replay contract merely to simplify
debugging.

Preserve:

- one effective recorded input frame per simulation tick;
- PlayerInput semantics;
- restart-bit semantics;
- recorded initial RNG seed;
- v2 starting level/lives/rim speed;
- v1 compatibility;
- production simulation and update ordering;
- production progression;
- RNG model;
- playback cursor semantics;
- current visible live-input handoff;
- playback simulation-override rejection;
- renderer-only treatment of game-over density.


PROJECT TESTING STRATEGY

Preserve the four complementary layers:

1. unit/integration;
2. headless gameplay/campaign regression;
3. visible deterministic gameplay playback;
4. Neal manual gameplay acceptance.

The proposed debugger should strengthen layers 2 and 3, not replace manual
gameplay as the final arcade-fidelity authority.


PRIMARY ARCHITECTURE QUESTION

Determine whether ONE shared deterministic playback execution core should serve:

A. visible replay with renderer and debugger controls;

B. headless replay used by automated tests and VS Code Copilot.

Preferred conceptual flow:

recording loader
-> restore recorded startup
-> deterministic playback executor/cursor
-> TempestGame.RunOneStep(...)
-> optional renderer OR headless observer
-> optional state inspection/checkpoints

Avoid separate visible/headless game simulations.


RECOMMENDED IMPLEMENTATION PHASES TO EVALUATE


PHASE 1 — FORWARD-ONLY DEBUGGING

Evaluate:

- pause/resume;
- one-tick step forward;
- visible playback-speed control;
- visible fast forward by multiple simulation ticks per rendered frame;
- pause/run to target tick;
- headless run to target tick;
- read-only state summary/checkpoints;
- deterministic completion reporting.

Prefer this as the first implementation milestone if architecture supports it.


PHASE 2 — BACKWARD NAVIGATION

Evaluate:

- step backward one tick;
- rewind;
- jump to an earlier tick.

Preferred initial implementation:

recreate recorded startup
-> deterministically replay from tick zero
-> stop at requested earlier tick.

DO NOT reverse collisions, RNG, ledger mutations, score changes, enemy creation,
etc.

Evaluate replay-from-zero before proposing snapshots.


PHASE 3 — SNAPSHOTS ONLY IF NECESSARY

Only if replay-from-zero is measurably inadequate, evaluate periodic snapshots.

Determine everything a complete deterministic snapshot would have to capture,
including:

- level/configuration;
- game state/timers;
- simulation tick;
- player;
- score/lives;
- player shots;
- enemy bullets;
- Eggs/reserve state;
- Flippers including directions/pivots/program state;
- Tankers/cargo;
- Spikers;
- Fuseballs;
- Pulsars;
- Spikes/damage;
- Super Zapper;
- progression/exit/approach state;
- wave/identity ledger;
- pending activations;
- Hurry-Up/pulse state;
- Game Over state;
- playback cursor;
- ALL simulation-affecting RNG state;
- anything else current code proves is required.

Do not recommend snapshots until completeness can be demonstrated.


PHASE 4 — OPTIONAL ADVANCED TOOLS

Analyze only:

- bookmarks;
- named checkpoints;
- loop between ticks;
- playback start/end tick;
- observation overlays;
- deterministic state hashes/signatures;
- recording-backed regression fixtures;
- state comparison between runs;
- first-divergence detection;
- state-summary export;
- playlist/browser UI;
- attract-mode reuse.

Rank by diagnostic value and risk.


RECORDED INPUT VS DEBUGGER INPUT

Keep these separate.

RECORDED SIMULATION INPUT includes:

- Fire;
- spinner/movement;
- mouse delta;
- controller axis;
- Super Zapper;
- restart.

DEBUGGER CONTROL includes:

- pause/resume;
- forward step;
- backward step;
- speed;
- fast forward;
- rewind;
- seek/run-to-tick;
- loop;
- quit debugger.

Determine where debugger controls should be sampled so they NEVER enter the
recorded PlayerInput stream or change simulation identity.

Recommend controls, but do not implement final key mappings.


PAUSE SEMANTICS

Determine what should happen while visible replay is paused.

Investigate separately:

- simulation;
- playback cursor;
- rendering;
- audio;
- display-only animations;
- Raylib window event handling;
- debugger input;
- cursor behavior;
- Game Over effects;
- approach/starfield presentation;
- overlays.

The window must remain responsive while simulation is paused.


STEP-FORWARD SEMANTICS

Define whether one step:

- consumes exactly one recorded frame;
- executes RunOneStep exactly once;
- advances the playback cursor once;
- renders afterward;
- emits normal simulation audio events;
- correctly handles restart frames;
- behaves predictably at final recorded tick.

Debugging unit should be one simulation tick.


FAST FORWARD

VISIBLE:

Process several simulation ticks per displayed frame without skipping simulation
ticks.

HEADLESS:

- no Raylib window;
- no rendering;
- no real-time sleep;
- repeatedly execute production RunOneStep;
- stop exactly at target tick.

Audit whether rendering/audio currently owns any simulation-affecting state
that would prevent true headless playback.


BACKWARD / REWIND

Do not reverse game mutations.

For backward step from tick N:

reconstruct startup
-> replay appropriate frames from tick zero
-> present state at N-1.

Define tick indexing exactly to avoid off-by-one behavior.

Evaluate expected performance rather than assuming it will be slow.


HEADLESS RUNNER

Analyze a reusable facility capable of:

- loading v1/v2 recording files;
- validating recording format;
- restoring recorded production startup;
- running without Raylib graphics;
- replaying until completion or target tick;
- producing read-only state summaries;
- performing assertions/checkpoints;
- returning nonzero on validation/simulation/assertion failure.

Determine whether architecture should provide:

A. reusable in-process API;
B. development CLI;
C. both, with CLI wrapping the reusable API.

Prefer reusable production-simulation code rather than test-only duplication.


VS CODE COPILOT USE CASES

Assess workflows such as:

1. Run recording headlessly to tick N.
2. Return structured state at N.
3. Compare N-1, N, N+1.
4. Add assertion around suspected invariant.
5. Locate first divergent tick between executions.
6. Turn difficult manually recorded scenario into permanent regression fixture.
7. Run same file visibly for Neal and headlessly for automated investigation.

Copilot cannot magically inspect arbitrary running memory.

State must be exposed through explicit project-owned summaries/APIs/tests.


READ-ONLY STATE SUMMARY

Inventory diagnostically valuable fields.

At minimum consider:

- simulation tick;
- displayed/internal level;
- LevelState;
- player lane/fractional location/lives;
- score;
- player shots;
- enemy bullets;
- Eggs;
- Flipper identity/type/lane/height/state/direction/pivot target/program state;
- Tankers/cargo ownership;
- Spikers;
- Spike lane/height/state;
- Fuseballs;
- Pulsars;
- Super Zapper state;
- unresolved/terminal wave identities;
- reserve obligations;
- pending activations;
- death/re-entry/exit/approach timers;
- playback cursor/remaining frames.

Recommend a bounded read-only summary model.

Do not expose every private field indiscriminately.


DETERMINISTIC SIGNATURES

Analyze whether an optional diagnostic state hash could locate the first
diverging tick.

Distinguish:

- same-build/runtime diagnostic comparison;
- cross-version/cross-runtime compatibility.

Do not promise stable cross-version hashes unless architecture actually
supports them.

Human-readable state summaries must remain available.


AUDIO POLICY

Analyze expected audio behavior during:

- pause;
- step forward;
- fast forward;
- headless replay;
- backward reconstruction.

Determine whether headless/debug audio can be suppressed without changing
simulation.

Do not redesign audio.


HEADLESS SAFETY AUDIT

Identify Raylib/audio dependencies in:

- TempestGame construction;
- RunOneStep;
- gameplay systems;
- recording/playback;
- rendering;
- Program composition.

Existing tests already exercise headless TempestGame simulation.

Determine the minimum additional seam needed to replay a real recording without
initializing Raylib.


RECORDINGS AS REGRESSION FIXTURES

Distinguish:

A. temporary generated recordings under ignored Temp/;

B. deliberately reviewed/committed regression recordings;

C. synthetic/in-memory test recordings.

Recommend policy for:

- naming;
- provenance;
- file size;
- expected scenario/level;
- checkpoint ticks;
- recording format compatibility;
- intentional gameplay changes;
- privacy/machine-specific data;
- whether JSON remains appropriate.

Do not commit recordings now.


ERROR HANDLING

Future runner should clearly distinguish:

- invalid recording;
- unsupported version;
- simulation exception;
- invariant/assertion failure;
- user-requested stop;
- successful completion.

Preserve visible exceptions and nonzero failure exit codes.

Do not suppress exceptions.


RECORDING COMPATIBILITY

Retain:

- v1 playback compatibility;
- accepted v2 playback.

Prefer debugger/session data to remain separate from the gameplay recording
format unless something is truly part of simulation identity.

Do not propose rewriting existing recordings.


PERFORMANCE MEASUREMENT PLAN

Recommend future benchmarks for:

- headless ticks/second;
- visible fast-forward throughput;
- replay-from-zero time to representative ticks;
- repeated single-tick backward cost;
- possible snapshot memory size;
- possible snapshot restore cost.

Do not invent benchmark values.


SECURITY / FILE SAFETY

Preserve strict recording validation.

Future runner must not:

- execute recording content;
- overwrite source recordings;
- modify arbitrary files;
- accept nonfinite values;
- silently accept unknown format members;
- silently accept unsupported versions.

Audit path handling if a headless CLI is proposed.


BOUNDED PHASE-1 RECOMMENDATION REQUIRED

Evaluate this proposed first implementation milestone:

- shared playback executor/controller;
- visible pause/resume;
- visible one-tick step forward;
- visible speed/fast-forward;
- run/pause at target tick;
- reusable headless recording runner;
- read-only state summary at target tick;
- automated tests;
- NO backward navigation yet;
- NO snapshots;
- NO permanent recording fixture.

If still too broad, split Phase 1 into smaller ordered milestones.

Prefer staged, manually/automatically verifiable checkpoints rather than one
large implementation.


NO IMPLEMENTATION

Do not:

- modify tracked source;
- modify tests;
- modify tracked durable docs;
- add controls;
- add CLI options;
- add snapshots;
- add fixture recordings;
- launch the game;
- stage;
- commit;
- push.

Only write ignored AI prompt/report records required by project policy.


AI LOGS

Preserve this complete task as:

AI-Logs/Prompts/2026-11-06-recorded-playback-debugger-headless-analysis-prompt.md

Create:

AI-Logs/Reports/2026-11-06-recorded-playback-debugger-headless-analysis-report.md

Report must contain:

1. Starting Git state.
2. Current recording format/compatibility.
3. Current visible playback architecture.
4. Current headless simulation seams.
5. Recommended shared execution core.
6. Recorded-input/debugger-control separation.
7. Pause/resume semantics.
8. Step-forward semantics.
9. Visible fast-forward design.
10. Headless run-to-tick design.
11. Completion/live-input semantics.
12. State-inspection inventory.
13. State-summary contract.
14. Deterministic-signature assessment.
15. Backward-step replay-from-zero design.
16. Rewind/seek semantics.
17. Replay-from-zero performance concerns.
18. Snapshot completeness inventory.
19. Snapshot risks.
20. Audio policy.
21. Rendering/headless dependency audit.
22. Proposed APIs/classes and ownership.
23. Proposed CLI surface.
24. Proposed visible controls.
25. VS Code Copilot workflows.
26. Recording-fixture policy.
27. Error/exit-code policy.
28. File/security review.
29. Performance measurement plan.
30. Testing strategy.
31. Regression risks.
32. Recommended implementation phases.
33. Exact Phase-1 scope.
34. Deferred features.
35. Final Git state.
36. Confirmation no implementation/stage/commit/push/game launch/Level 8 or 9/
    gameplay change/recording modification occurred.


FINAL GIT CHECK

Run:

git status --short --branch
git rev-parse HEAD
git rev-parse origin/master
git diff --stat
git diff --cached --stat
git ls-files --others --exclude-standard

Expected:

HEAD remains:

7032a7219cde69a0ae25b00e5f3bc9ac1ee3ed04

Require:

- clean tracked worktree/index;
- local master matches origin/master;
- only ignored AI records written;
- no commit;
- no push.


FINAL RESPONSE

Return:

1. Whether one shared visible/headless playback engine is recommended.
2. Current architectural constraints.
3. Recommended Phase 1.
4. Pause/resume semantics.
5. Forward-step semantics.
6. Visible fast-forward approach.
7. Headless run-to-tick approach.
8. State-summary recommendation.
9. VS Code Copilot usage model.
10. Backward-step design.
11. Rewind design.
12. Whether replay-from-zero should precede snapshots.
13. Snapshot requirements/risks.
14. Audio policy.
15. Proposed APIs/classes.
16. Proposed CLI.
17. Proposed visible controls.
18. Testing strategy.
19. Performance measurement strategy.
20. Recording-fixture policy.
21. Error/exit-code policy.
22. Recommended implementation sequence.
23. Deferred features.
24. Report filename.
25. Final Git state.
26. Confirmation no implementation, stage, commit, push, game launch,
    Level 8/9 work, gameplay change, or recording modification occurred.

Stop for Neal's review before implementation.

TEMPEST_RECORDED_PLAYBACK_DEBUGGER_HEADLESS_ANALYSIS_COMPLETE