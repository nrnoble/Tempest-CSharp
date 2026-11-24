# VS Code Copilot Runtime Gameplay Capability Probe

**Result: PARTIAL capability availability.** The existing headless playback path can run known-good production recordings to an exact boundary and return a useful production-state snapshot. A bounded multi-level segment ran much faster than real time. The current agent workflow cannot observe or control a live game window, and no .NET debugger control is available.

This was a bounded capability investigation, not gameplay acceptance or a fidelity evaluation. No source, tests, configuration, project documentation, recordings, branch, HEAD, or Git index state were changed. The only task-created files are this report and its paired prompt; both remain untracked and unstaged.

## Repository state and scope

- Branch: `master`; HEAD: `d1544654e780c677c852f38dc6bf1e2045900f6f`; upstream: `origin/master`.
- Pre-existing work at start, preserved: modified [2026-11-23-level12-final-closeout-report.md](./2026-11-23-level12-final-closeout-report.md) and untracked [MBUCOD.V05.txt](../../Reference/arcade/MBUCOD.V05.txt).
- Only the three explicitly authorized recordings were inspected or run:
  - [level-1.json](../../TEMP/GameplayRecordings/Tests/level-1.json) - 797 ticks.
  - [level-12.json](../../TEMP/GameplayRecordings/Tests/level-12.json) - 1,778 ticks.
  - [level1-9.json](../../TEMP/GameplayRecordings/Tests/level1-9.json) - 17,145 ticks.
- No other recording directories or recordings were searched.
- Commands used the existing Debug `net9.0` application output; no build, test, install, or project code changes were needed.

## Existing capability inventory

- [HeadlessGameplayPlaybackCli.cs](../../src/Tempest.Game/Debug/HeadlessGameplayPlaybackCli.cs) accepts `--headless-playback <file> [--boundary <tick>] [--json]`, loads a recording, and runs the same deterministic [GameplayPlaybackSession](../../src/Tempest.Game/Debug/GameplayPlaybackSession.cs) simulation used by visible playback, without initializing Raylib. The boundary is inclusive as the resulting state boundary; boundary 0 is startup and consuming frame N produces boundary N+1.
- JSON and human-readable [GameplayStateSummary.cs](../../src/Tempest.Game/Debug/GameplayStateSummary.cs) output reports the requested/achieved boundary, recording duration, remaining frames, completion reason, current level/state, score, lives, player lane/position/depth, Super Zapper charges, and counts of shots, enemy bullets, Eggs, Flippers, Tankers, Spikers, Spikes, Fuseballs, and Pulsars.
- The summary includes per-Flipper lane, height, movement state, `Direction`, `RimPursuitDirection`, pivot source/destination, angle, and progress. Tanker cargo Flippers carry cargo identity flags/IDs.
- The summary does **not** expose individual Tanker, Spiker, Fuseball, Egg, Spike, projectile, or cargo internals, nor pending/reserve/wave-ledger state. Those details cannot be inferred from entity counts.
- `--boundary` supplies bounded headless forward advancement. The CLI has no pause, interactive single-step, or reverse option. `GameplayPlaybackSession` also has public `StepForward` and deterministic `ReconstructToBoundary` APIs.
- Visible playback source supports Space to pause, Right to advance one frame, Shift+Right to advance five, and Left/Shift+Left to reconstruct backward while paused. On reaching the recorded end, visible playback reports completion and resumes live controls; recording completion does not terminate normal visible gameplay.
- Interactive `--speed` accepts 0.25 through 4.0 and adjusts the host-loop frame target. It is not accepted by the headless playback CLI, which permits only `--boundary` and `--json` after the recording path.
- [PlayerInput](../../src/Tempest.Game/Systems/InputSystem.cs) is a public input record; deterministic [TempestGame.RunOneStep](../../src/Tempest.Game/TempestGame.cs), the `TempestGame` state, and gameplay entities are exposed through existing C# APIs. No current agent tool provides an in-memory .NET execution surface to construct a game, submit successive chosen inputs, and inspect it without adding/running a harness.

## Bounded playback experiments

Commands used the equivalent process arguments below. Each headless run had a requested boundary and a 15-second secondary timeout; the command process exited after producing the boundary result.

1. `dotnet src\Tempest.Game\bin\Debug\net9.0\Tempest.Game.dll --headless-playback TEMP\GameplayRecordings\Tests\level-1.json --boundary 500 --json`
   - Reached boundary 500 of 797; 297 frames remained; completion was `RequestedBoundaryReached`.
   - Reported Level 1 / Playing, score 950, 20 lives, player lane 4, 5 shots, 2 enemy bullets, 1 Egg, and 4 Flippers.
   - Per-Flipper state was present. At this boundary all four were climbing, so no active rim-pursuit latch was present in this sample.

2. `dotnet src\Tempest.Game\bin\Debug\net9.0\Tempest.Game.dll --headless-playback TEMP\GameplayRecordings\Tests\level-12.json --boundary 1000 --json`
   - Reached boundary 1,000 of 1,778; 778 frames remained; completion was `RequestedBoundaryReached`.
   - Reported Level 12 / Playing, score 5,540, 3 lives, player lane 10, 2 Flippers, 1 Tanker, 2 Spikers, 15 Spikes, and 1 Fuseball.
   - One observed Flipper was Flipping on lane 7 with `Direction=-1`, `RimPursuitDirection=-1`, pivot source 7 and destination 8. The other was Flipping with `Direction=1`, no rim-pursuit latch, source 7 and destination 6.
   - This is a runtime-state example only, not Level 12 acceptance.

3. End-boundary check:
   `dotnet src\Tempest.Game\bin\Debug\net9.0\Tempest.Game.dll --headless-playback TEMP\GameplayRecordings\Tests\level-1.json --boundary 797 --json`
   - Reached boundary 797 of 797 with zero frames remaining, `RecordingCompleted`, and `isRecordingComplete=true`.
   - The resulting simulated state had advanced to Level 2 / Approach.
   - Headless mode returned and the process exited. By contrast, the visible application's code transfers to live controls after playback completion.

4. Bounded fast-forward on the long campaign recording:
   `dotnet src\Tempest.Game\bin\Debug\net9.0\Tempest.Game.dll --headless-playback TEMP\GameplayRecordings\Tests\level1-9.json --boundary 10000 --json`
   - Completed boundary 10,000 of 17,145 in 0.421 seconds, within a 20-second secondary timeout. The child process returned exit code 0.
   - The deterministic snapshot reported Level 7 / Playing, score 32,489, 20 lives, player lane 12, 2 Flippers, 1 Tanker, and 4 Spikers.
   - Elapsed time corresponds to about 395 times a 60-tick-per-second wall-clock pace for the measured segment. This proves useful headless fast-forward, not an interactive 4x run or continuous state monitoring. It stopped before recording completion.

No experiment waited for lives to be lost. Headless playback ends at the requested boundary or recording end; it does not transfer to live input.

## Tier results

### Tier 1 - recorded playback and state inspection: PARTIALLY AVAILABLE

All three supplied recordings were usable through the existing headless playback CLI. The probes demonstrated deterministic state advancement, exact boundary targeting, current-level/player/enemy-count reporting, per-Flipper latch/direction reporting, end detection, and fast long-recording advancement.

The practical limit is snapshot-at-target operation: the CLI does not expose a live session to pause, inspect repeatedly, or step forward/backward from the reached boundary. Headless JSON also omits individual non-Flipper enemy details and ledger/pending state. The visible playback controls exist in the application, but the agent cannot operate them because it cannot send native-window input.

### Tier 2 - debugger-controlled playback: NOT AVAILABLE

No current agent tool supports launching or attaching a .NET/C# debugger, breakpoints (conditional or otherwise), stepping, C# locals/object inspection, expression evaluation, or call-stack inspection. The available debugger tool discovered is Python-specific and is not applicable to this .NET project; it was not used.

Playback plus a .NET debugger was not demonstrated and cannot be usefully combined in this workflow. The deterministic headless summary still helps narrow a runtime state, but it does not provide breakpoint-level diagnosis.

### Tier 3 - native Raylib launch and interaction

- **Process launch:** demonstrated with `dotnet src\Tempest.Game\bin\Debug\net9.0\Tempest.Game.dll --level 1`. Raylib initialized the DESKTOP/GLFW backend, display, OpenGL, and audio. The process remained alive after four seconds.
- **Process termination:** the process was explicitly stopped by PID 138220 and verified absent. This was forced process termination, not a graceful in-game quit; the current agent has no demonstrated native-window input path.
- **Output monitoring:** Raylib initialization output was captured. This establishes process/backend startup, not visual inspection of gameplay.
- **Visible recorded-playback launch:** not separately exercised; the existing `--playback-file` path is present in the application, but no visual or input control was available to validate it interactively.
- **Native Raylib window observation is not available in the current agent toolset.**
- **Native Tempest input is not available in the current agent toolset.** No keyboard/mouse automation, physical controller, or workaround was attempted.
- **Autonomous visual gameplay is not available with the current toolset.** Both live visual observation and working native controls are prerequisites, and neither is available.

### Tier 4 - deterministic programmatic player input

**POSSIBLE BUT NOT EXPOSED TO CURRENT AGENT WORKFLOW.** Existing public C# APIs provide `PlayerInput`, deterministic `TempestGame` construction, `RunOneStep`, and inspectable game state, so the architecture supports the input -> production step -> state observation loop without a new gameplay architecture. The current tools do not expose an in-memory .NET runner for an agent to instantiate that loop, and this probe did not create a harness or alter tests.

## Capability matrix

| Capability | Status | Demonstrated evidence | Limitation |
|---|---|---|---|
| Locate/use supplied recording | FULLY AVAILABLE | Used all three authorized paths | No other recordings inspected |
| Headless playback | FULLY AVAILABLE | Three `--headless-playback` runs | Snapshot output only |
| Visible playback launch | PARTIALLY AVAILABLE | Native Raylib app launched; application has `--playback-file` path | A visible playback run was not separately exercised |
| Playback-end detection | FULLY AVAILABLE | Level 1 boundary 797 returned `RecordingCompleted` and `isRecordingComplete=true` | Visible mode continues into live input |
| Safe process termination after playback | FULLY AVAILABLE | Headless runs ended at explicit boundaries; native process was stopped and verified absent | Native app required forced PID termination, not graceful UI exit |
| Accelerated playback | PARTIALLY AVAILABLE | 10,000-tick headless segment completed in 0.421 s | No continuous monitoring while it runs |
| Approximately 4x playback | PARTIALLY AVAILABLE | Interactive `--speed` supports 4.0 in existing code/docs | Exact 4x visible playback was not run/observed; headless mode rejects `--speed` |
| State-summary inspection | FULLY AVAILABLE | JSON snapshots at selected boundaries | Bounded set of summary fields |
| Current-level inspection | FULLY AVAILABLE | Level 1, 2, 7, and 12 reported in snapshots | None for the reported boundary |
| Player-state inspection | FULLY AVAILABLE | Lane, position, depth, lives, score and charges | No direct live object inspection |
| Enemy-state inspection | PARTIALLY AVAILABLE | Counts for enemy/projectile types are included | Detailed individual state generally omitted |
| Flipper direction/latch inspection | FULLY AVAILABLE | Level 12 sample exposed direction, latch and pivot edges | Only Flippers have detailed summary entries |
| Deterministic boundary/tick targeting | FULLY AVAILABLE | Exact requested and achieved boundaries, including end | Target is selected at launch |
| Bounded forward advancement | FULLY AVAILABLE | Headless runner advanced from startup to requested boundaries | No CLI command to continue from a paused snapshot |
| Reverse-state/reverse-playback analysis | PARTIALLY AVAILABLE | Existing session reconstructs a target boundary; visible controls support Left while paused | No agent control of that session |
| Debugger launch/attach | NOT AVAILABLE | No .NET debugger tool in current agent toolset | No C# debugger session created |
| Breakpoint | NOT AVAILABLE | No .NET breakpoint control available | n/a |
| Conditional breakpoint | NOT AVAILABLE | No .NET conditional-breakpoint control available | n/a |
| Variable/object inspection (debugger) | NOT AVAILABLE | No debugger object inspection available | JSON summaries are a separate, bounded facility |
| Expression evaluation | NOT AVAILABLE | No C# debugger expression evaluator available | n/a |
| Call-stack inspection | NOT AVAILABLE | No .NET debugger stack inspection available | n/a |
| Source stepping | NOT AVAILABLE | No .NET debugger step control available | n/a |
| Debugger + playback combination | NOT AVAILABLE | Neither C# debugger control nor debugger session was available | Cannot break/inspect/step within playback |
| Native Raylib process launch | FULLY AVAILABLE | Raylib DESKTOP/GLFW and OpenGL initialization logged | Launch does not imply visual access |
| Native Raylib window observation | NOT AVAILABLE | No native desktop/window observation tool exposed | **Native Raylib window observation is not available in the current agent toolset.** |
| Native Tempest input | NOT AVAILABLE | No supported native-window input tool exposed | No input attempt made |
| Autonomous visual gameplay loop | NOT TESTED because prerequisite unavailable | Prerequisite observation and control both absent | **Autonomous visual gameplay is not available with the current toolset.** |
| Deterministic programmatic input via existing APIs | PARTIALLY AVAILABLE | Public input, simulation-step and state APIs exist | No current agent in-memory .NET execution surface |

## Strongest demonstrated workflow and recommendations

**Strongest demonstrated runtime workflow:** known-good recording -> headless deterministic fast-forward to an explicit boundary -> inspect the JSON production-state snapshot -> stop at that boundary. It is not a debugger/step-through workflow.

- **Routine level development:** use focused tests and existing bounded headless playback as a supplemental production-state check; use normal build/test validation and Neal's manual gameplay acceptance for fidelity and feel.
- **Difficult runtime defect:** use a known-good recording and a bounded target boundary to establish relevant production state. The current agent can compare snapshots obtained by separate deterministic runs, but cannot pause a running session, inspect arbitrary object fields, or step a defect transition in a C# debugger.
- **Broad campaign check:** the measured 10,000-tick, multi-level segment shows fast headless campaign smoke playback is practical. It is supplemental only, not a substitute for tests or manual acceptance, and the complete recording was not run.
- **Visual fidelity:** no live window image was observed. Neal remains the authority; internal-state results do not establish presentation fidelity.

No additional infrastructure is recommended by this probe.

## Termination, validation, and completion

- Every recording run had a finite target boundary; the Level 1 end check targeted its exact 797-tick duration.
- Secondary time bounds were 15 seconds for the short CLI runs, 20 seconds for the long segment, and four seconds for the native launch. None timed out.
- Headless CLI processes exited after their bounded work. The native process was stopped explicitly and verified absent. No process required Neal intervention.
- **PASS:** requested probes completed with the reported limits. This is a tool-capability result, not a gameplay-fidelity acceptance.
- No source, tests, project configuration, recordings, durable project documentation, branch, HEAD, index, or pre-existing worktree state was modified. The two requested AI-Logs files remain untracked and unstaged; the pre-existing modified report and untracked reference file were preserved.
