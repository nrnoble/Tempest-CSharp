Read this entire user-provided task before taking any action.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, files, commands, debugger facilities, launching processes, or taking repository action, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with TEMPEST_ appears on a line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment metadata, currently open editor information, or other system-generated context appearing after the user's token do not invalidate it.

If exactly one complete token cannot be found, stop before any project or repository action and explain why.

# Task: Bounded capability probe for VS Code Copilot runtime gameplay debugging and control

This is a READ-ONLY capability investigation.

Do not implement features.

Do not modify Tempest production source, tests, configuration, durable documentation, gameplay recordings, project instructions, or Git state.

The objective is to determine what the current VS Code Copilot/agent environment can already do with Tempest when investigating real gameplay through:

1. deterministic recorded gameplay playback;
2. accelerated recorded playback;
3. internal runtime-state observation;
4. simulation stepping/state inspection;
5. debugger-controlled execution if supported;
6. optionally, direct native Raylib-window observation and input if the current agent tools genuinely support it;
7. deterministic programmatic player input through already-existing Tempest APIs, if currently accessible without adding new infrastructure.

Do not spend substantial time trying to make an unsupported capability work.

This is a capability probe, not a development milestone.

## Known-good gameplay recordings

Use only these explicitly authorized, known-good recordings:

TEMP\GameplayRecordings\Tests\level-1.json

TEMP\GameplayRecordings\Tests\level-12.json

TEMP\GameplayRecordings\Tests\level1-9.json

Do not search for additional recordings.

Do not enumerate other recording directories.

Do not modify, rewrite, truncate, normalize, or replace these recordings.

Use them as deterministic input evidence only.

## Important playback behavior

Recorded playback does **not** necessarily cause the Tempest process to terminate when recorded input ends.

After the recorded input reaches its end, normal gameplay can continue with live input until:

- all lives are exhausted;
- the game exits normally;
- or the game/process is explicitly stopped.

Therefore:

**Do not treat “recording finished” as synonymous with “process finished.”**

Every playback experiment in this task must have an explicit bounded termination rule.

Permitted boundaries include:

- stop immediately after the known playback boundary is reached;
- stop at a predetermined simulation tick/boundary;
- stop after the required state has been observed;
- stop when a debugger breakpoint/condition has supplied the required evidence;
- stop after an explicit short command timeout.

Do not leave a gameplay process running indefinitely after playback input ends.

Do not wait for all lives to be lost merely to terminate a capability experiment.

## Hard scope/time rule

Keep this investigation short.

Use approximately one meaningful bounded attempt for each capability tier.

Do not:

- create a new automation framework;
- install desktop automation packages;
- build a new MCP server;
- add computer-vision infrastructure;
- add an AI player;
- modify Tempest input architecture;
- create new recordings;
- rewrite playback;
- create new permanent diagnostics;
- spend repeated attempts trying to force native GUI control to work.

If a capability is unavailable, report it and continue.

A negative result is a valid result.

## First inspect existing Tempest capabilities

Inspect the current local project for already-existing support for:

- recorded gameplay;
- visible playback;
- headless gameplay playback;
- playback-file startup;
- deterministic simulation;
- gameplay-state summaries;
- current-level reporting;
- player-state reporting;
- enemy-state reporting;
- Flipper state including Direction and RimPursuitDirection;
- Tanker, Spiker, Fuseball, and cargo state where exposed;
- wave-ledger/pending-state reporting where exposed;
- pause;
- forward stepping;
- reverse stepping;
- speed multiplier;
- faster-than-real-time playback;
- deterministic tick/boundary identification;
- startup configuration;
- headless playback CLI;
- existing input-sequence APIs.

Do not change any of this code.

Create a concise inventory of what already exists before performing the capability probes.

## Recording roles

Use the three recordings intentionally.

### level-1.json

Use as the simplest baseline.

Preferred purposes:

- prove that an existing recording can be started;
- identify playback boundary/tick information;
- inspect basic player/enemy state;
- prove a bounded run can be stopped safely when recorded input ends.

### level-12.json

Use as the current-production-level runtime example.

Preferred purposes:

- inspect modern shared lifecycle/ledger behavior;
- inspect Level 12 state;
- demonstrate internal-state observation on current gameplay code;
- test whether debugger/state inspection is useful during real production playback.

Do not use this probe to perform Level 12 acceptance.

Level 12 is already manually accepted and closed.

### level1-9.json

Use only if useful for demonstrating long deterministic playback and acceleration.

Preferred purposes:

- traverse multiple levels;
- test faster-than-real-time playback;
- verify that internal state can still be inspected while rapidly advancing through a long recording.

Do not run the complete 4.5 MB recording at real time merely for ceremony.

Stop once the capability has been demonstrated.

## Tier 1: deterministic recorded playback

First determine whether the agent can successfully invoke existing playback using the supplied recordings.

Use existing verified Tempest command-line/API behavior.

Do not invent command-line syntax.

Demonstrate at minimum:

1. playback starts successfully;
2. the game/simulation recognizes recorded input;
3. deterministic playback boundaries advance;
4. current level can be identified;
5. player state can be observed;
6. active enemy state can be observed through existing infrastructure where supported;
7. the run can be stopped cleanly at an explicit boundary.

Prefer headless playback for internal-state work when it already exposes sufficient information.

A visible playback launch may be tried separately only if useful.

Do not require native-window observation for Tier 1 success.

## Tier 1A: headless runtime-state observation

Using one known-good recording, determine whether the agent can inspect meaningful production state during playback without adding instrumentation.

Where supported, observe:

- current level;
- current playback boundary/tick;
- player lane;
- lives;
- score;
- active enemy counts/types;
- Flipper lane;
- Flipper movement state;
- Direction;
- RimPursuitDirection;
- SourceEdge;
- DestinationEdge;
- Tanker cargo state;
- Spiker state;
- Fuseball state;
- pending/reserve/ledger state.

Not every field must already be exposed.

Report exactly which fields can be observed with existing infrastructure.

Do not modify production code solely to expose missing fields.

## Tier 1B: accelerated playback

Neal commonly runs recorded gameplay at approximately 4× speed to reach later states quickly.

Determine whether existing Tempest controls already support accelerated playback in a way suitable for agent-driven runtime investigation.

Do not assume option syntax.

Read the current implementation/help/tests and use only verified existing controls.

If approximately 4× playback is supported:

1. run a bounded portion of level1-9.json faster than real time;
2. confirm deterministic production state still advances normally;
3. determine whether useful state can still be monitored;
4. stop at an explicit boundary before live gameplay continues.

If exact 4× is not supported but another established faster mode exists, report that instead.

Do not add a speed feature.

## Tier 1C: target-state approach

Determine whether the agent can use accelerated playback to approach a target gameplay condition and then switch to more precise observation.

A successful capability demonstration could be:

- run quickly toward a later playback boundary;
- stop/pause near a known state;
- inspect enemy/player state;
- advance through a small number of subsequent simulation steps;
- identify changes.

Do not require a particular gameplay bug in this probe.

The objective is capability, not defect reproduction.

## Tier 1 result

Classify recorded-playback runtime inspection as one of:

- FULLY AVAILABLE
- PARTIALLY AVAILABLE
- NOT AVAILABLE

Report:

- exact recording used;
- exact bounded command/path used;
- what internal state was observable;
- whether playback boundary/tick targeting worked;
- whether accelerated playback worked;
- how the run was terminated safely.

## Tier 2: debugger-controlled gameplay playback

Determine whether the current VS Code Copilot agent has an already-available debugger capability for this .NET project.

Do not install a debugger integration for this probe.

Do not modify .vscode project files solely to enable it.

Do not assume debugging capability from ordinary terminal execution.

If debugger control is available, perform one bounded demonstration using an existing playback recording or deterministic direct startup.

Determine whether the agent can:

1. launch Tempest under the debugger or attach to the relevant process;
2. set a breakpoint;
3. continue execution;
4. pause execution;
5. inspect local/object fields;
6. inspect a call stack;
7. evaluate an expression;
8. step over code;
9. step into code if supported;
10. resume;
11. stop the session cleanly.

If supported, also test one conditional breakpoint or equivalent state-based stop.

A useful candidate could involve existing Flipper rim-pursuit code because fields such as:

- EdgeIndex
- Direction
- RimPursuitDirection
- SourceEdge
- DestinationEdge
- State

are relevant and well understood.

Do not change Flipper code.

The recent rim-pursuit bug is already fixed; use it only as a convenient known state model if helpful.

## Tier 2: debugger plus playback

If both playback and debugger control are available, determine whether they can be combined.

The ideal diagnostic workflow would be:

1. start a known deterministic recording;
2. run quickly toward a target condition;
3. stop through a breakpoint/conditional breakpoint or controlled pause;
4. inspect live runtime state;
5. step through the relevant transition;
6. resume or stop.

Demonstrate only enough to prove or disprove that workflow.

Do not spend substantial time obtaining an ideal demo.

## Tier 2 result

Classify debugger-controlled gameplay analysis as:

- FULLY AVAILABLE
- PARTIALLY AVAILABLE
- NOT AVAILABLE

State exactly:

- what debugger capabilities were actually demonstrated;
- what was unavailable;
- whether playback and debugging can coexist usefully;
- whether the result would help investigate future runtime defects.

Do not claim capabilities you did not actually demonstrate.

## Tier 3: visible native gameplay launch

Determine whether the agent can launch Tempest in its normal Raylib/OpenGL graphical mode.

A successful launch alone does not prove native visual interaction.

If launching the native game is possible, distinguish:

- process launch;
- process lifetime monitoring;
- console/output monitoring;
- native game-window observation;
- native game-window input.

Use a bounded launch and stop it cleanly.

Do not leave Tempest running waiting for lives to expire.

## Tier 3A: native Raylib window observation

Determine whether the current agent toolset can actually observe the contents of the native Tempest window.

Do not confuse:

- integrated browser screenshots;
- terminal output;
- file screenshots;
- known static assets;

with actual observation of the live Raylib window.

If no native desktop/window observation tool is exposed, report:

**Native Raylib window observation is not available in the current agent toolset.**

Stop that portion immediately.

Do not attempt workarounds.

## Tier 3B: native Tempest input

Only attempt this if the current agent demonstrably has an existing supported way to send input to the native Tempest window.

If supported, perform only a tiny proof:

- move the Blaster in one direction;
- move it in the opposite direction;
- press Fire once;
- confirm from observable game/runtime state that those inputs were received;
- stop.

Do not attempt to complete a level.

Do not attempt to play competently.

Do not use the user's physical spinner/controller.

Do not install input automation software.

If native input is unavailable, state so plainly.

## Tier 3C: autonomous gameplay reasoning

Attempt this only if BOTH are already proven:

1. the agent can observe the live Tempest native window or equivalent live visual state;
2. the agent can send working player controls to Tempest.

If either prerequisite is absent:

**Autonomous visual gameplay is not available with the current toolset.**

Do not simulate success through prerecorded input.

If both prerequisites genuinely exist, perform only a tiny bounded experiment:

- direct safe level;
- extra lives if an existing option supports them;
- maximum roughly 10–15 seconds;
- simple goal: move, fire, avoid an obvious immediate hazard if possible;
- stop.

Do not evaluate gaming skill.

The only question is whether a perception -> decision -> control loop is technically possible.

## Tier 4: deterministic programmatic input without native visuals

Independently of GUI control, inspect whether existing Tempest architecture already permits programmatically supplied player inputs through deterministic APIs.

Determine whether an agent could, without adding a new gameplay architecture:

- construct PlayerInput or equivalent;
- supply movement/fire/Super-Zapper state;
- run one production simulation step;
- inspect resulting state;
- choose the next input;
- repeat.

Do not implement an AI player.

Do not create a new harness.

Do not modify tests.

Report whether this is:

- AVAILABLE THROUGH EXISTING APIS
- POSSIBLE BUT NOT EXPOSED TO CURRENT AGENT WORKFLOW
- REQUIRES ADDITIONAL TOOLING/CODE
- NOT PRACTICAL

This result is important even if Tier 3 native graphical control is unavailable.

It may show that an agent-driven gameplay policy could someday operate directly against simulation state rather than pixels.

## Playback completion safety

For every experiment using the supplied recordings:

- identify the playback-end condition;
- do not wait for the game itself to exit after recorded input ends;
- explicitly terminate or detach once sufficient evidence is collected;
- use a timeout as a secondary safety bound;
- report whether control would normally transfer to live input afterward.

A command that remains running after playback finishes must be stopped intentionally.

Do not classify the command as hung merely because normal gameplay continues after the recording ends.

## No automatic code changes

This task is an evaluation of existing capabilities.

Do not fix anything discovered.

Do not create permanent helpers.

Do not improve telemetry.

Do not add debugger hooks.

Do not add automation entry points.

Do not modify playback behavior.

If a missing capability would require code changes, classify it accordingly and stop.

## Recommended future Tempest workflow

Based only on demonstrated capability, recommend where agent runtime inspection should fit.

Consider:

### Routine level development

Potentially:

- focused unit/integration tests;
- bounded headless production soak;
- full suite;
- Release build;
- Neal manual gameplay acceptance.

### Difficult runtime gameplay defect

If demonstrated capabilities support it, recommend:

- use a known-good recording;
- accelerate playback to approach the event;
- inspect deterministic state;
- use breakpoint/conditional breakpoint where available;
- step through the first incorrect transition;
- add a permanent semantic regression test after diagnosis.

### Broad campaign check

If acceleration works, determine whether agent-driven fast playback of a multi-level recording is a practical supplemental smoke/regression method.

Do not present it as equivalent to unit tests or manual gameplay.

### Visual-fidelity work

Neal remains the final authority.

Use explicitly authorized screenshots/frames only when needed.

Do not claim internal-state correctness proves visual fidelity.

## Capability matrix

The final report must include:

Capability
Status
Demonstrated evidence
Limitation

Include rows for at least:

- locate/use supplied recording;
- headless playback;
- visible playback launch;
- playback-end detection;
- safe process termination after playback;
- accelerated playback;
- approximately 4× playback;
- state-summary inspection;
- current-level inspection;
- player-state inspection;
- enemy-state inspection;
- Flipper direction/latch inspection;
- deterministic boundary/tick targeting;
- bounded forward advancement;
- reverse-state/reverse-playback analysis;
- debugger launch/attach;
- breakpoint;
- conditional breakpoint;
- variable/object inspection;
- expression evaluation;
- call-stack inspection;
- source stepping;
- debugger + playback combination;
- native Raylib process launch;
- native Raylib window observation;
- native Tempest input;
- autonomous visual gameplay loop;
- deterministic programmatic input via existing APIs.

Use only:

- FULLY AVAILABLE
- PARTIALLY AVAILABLE
- NOT AVAILABLE
- NOT TESTED because prerequisite unavailable

for capability statuses where practical.

## Best runtime-debug workflow

State the strongest reliable runtime-debug workflow that was actually demonstrated.

For example, if supported:

**known-good recording -> accelerated deterministic playback -> target boundary -> debugger/state inspection -> bounded stepping -> stop**

Do not describe that workflow unless the probe actually demonstrates its essential components.

## What VS Code Copilot cannot currently do

State limitations plainly.

A negative result is useful.

Do not recommend additional infrastructure in this report unless Neal later asks to expand capabilities.

The objective is to discover the existing limit, not move it.

## Repository protection

Do not modify:

- source;
- tests;
- project configuration;
- .vscode;
- gameplay recordings;
- durable documentation;
- project instructions;
- reference files;
- AI-Logs history other than this paired capability prompt/report.

Do not stage, commit, or push.

Do not reset, restore, stash, clean, or rebase.

Do not begin Level 13.

## Final report

Save this prompt and the paired capability report under the tracked AI-Logs convention.

Report:

1. recordings used;
2. commands/workflows attempted;
3. explicit termination bounds;
4. Tier 1 result;
5. Tier 2 result;
6. Tier 3 result;
7. Tier 4 result;
8. capability matrix;
9. strongest demonstrated runtime-debug workflow;
10. recommended Tempest use cases;
11. unavailable capabilities;
12. whether any process required Neal intervention;
13. confirmation that no project files or recordings were modified.

Stop after the capability report.

TEMPEST_AGENT_RUNTIME_CAPABILITY_PROBE