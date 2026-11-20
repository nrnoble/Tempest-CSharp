# Recorded-Playback Debugger and Headless Runner Analysis

Date: 2026-11-06

Analysis only; recommendations below are not accepted gameplay behavior or
implementation decisions.

## 1. Starting Git state

`master` was clean and tracked `origin/master` at
`7032a7219cde69a0ae25b00e5f3bc9ac1ee3ed04` (`Use vector text for Avoid
Spikes`). The index, tracked worktree, and non-ignored untracked-file list were
empty. The configured remote is `origin`.

## 2. Current recording format and compatibility

`GameplayRecordingFile` reads strict `tempest-gameplay-input` v1 and v2 JSON.
Both preserve seed, duration, and one ordered input/restart frame per tick.
V2 adds production starting displayed level, lives, and rim-speed multiplier;
v1 retains Level 1, three lives, and speed 1.0 defaults. Unknown members,
unsupported versions/scenarios, missing fields, noncontiguous frames, invalid
metadata, and nonfinite input values are rejected. The accepted format and
visible live-input handoff are frozen; debugger state belongs outside this
file format.

## 3. Current visible playback architecture

`Program.cs` loads a file or creates a seeded profile, constructs
`TempestGame`, and owns a local simulation-tick counter. Each loop selects
replay or live input, applies the existing state-specific input/restart gates,
calls `RunOneStep` once, increments the counter, reports replay completion
once, and draws once. `PlayerInputReplay` is an indexed frame source; sparse
in-memory profiles yield neutral frames for omitted ticks. Playback currently
has no pause, stepping, speed, seek, or debugger-control object.

## 4. Current headless simulation seams

`TempestGame(GameplayStartupConfiguration, IAudioEvents?)` restores the
recorded seed and production startup before construction consumes randomness.
`RunOneStep(PlayerInput, restartPressed)` is the production simulation update
and does not call Raylib. If no audio adapter is supplied, the game uses
`SilentAudioEvents`; existing tests already construct games and replay frames
without a window or audio device. `GameplayRecordingFile.LoadRecording` and
`PlayerInputReplay` also have no Raylib dependency.

The missing seam is orchestration: the executable currently initializes its
window and audio unconditionally, and playback cursor/input gating live in its
visible loop. There is no reusable file-to-`RunOneStep` runner or state
checkpoint API.

## 5. Recommended shared execution core

Recommend one deterministic playback session shared by visible and headless
consumers. It should restore a validated recording's startup, own the replay
cursor and simulation tick, apply the existing input gates, and call
`TempestGame.RunOneStep` exactly once per consumed recording frame. Rendering,
Raylib debugger controls, audio policy, and observers should be optional
adapters around that core, never alternative simulations.

The core must preserve input frame meanings/order, seed and initialization
order, restart bits, normal progression, existing tick ordering, and the
handoff to live input at file completion.

## 6. Recorded input versus debugger controls

Keep debugger commands in a separate type and sampling path from
`PlayerInput`/`PlayerInputFrame`. Debugger controls are polled at the outer
loop boundary; they can pause, request a step, or change pacing but must never
be serialized, supplied as simulation input, or alter the playback cursor
unless the command explicitly advances it. While paused, do not sample or
record gameplay input. Avoid final key assignments until conflicts with the
existing spinner, Fire, Super Zapper, restart, and Enter/quit controls have
been checked.

## 7. Pause and resume semantics

While paused, freeze `TempestGame`, simulation tick, replay cursor, and all
tick-based presentation state. Continue the outer Raylib loop so window-close
and debugger events are handled and redraw the same state; rendering itself
must not advance gameplay. Keep the existing hidden-cursor policy unless a
separate debugger UI explicitly needs a cursor. Do not feed ignored paused
input to the next simulation tick.

Existing sound voices may finish naturally during a pause, but no new
simulation audio events should be emitted. The current renderer's transition
starfield is deterministic and computed from game state; game-over and
Super-Zapper display animation advance from `RunOneStep`, so they freeze with
the simulation.

## 8. Forward-step semantics

Define tick numbers as state boundaries: state at boundary 0 is the restored
startup before any frame; consuming recorded frame 0 executes one
`RunOneStep` and produces boundary 1. A forward step consumes exactly the
next frame, calls the normal production `RunOneStep` once (including that
frame's restart bit), advances the cursor/tick once, then renders the new
state. It must not bypass death, exit, game-over, or approach handling.
Normal simulation audio events may fire for that step. At the final recorded
boundary, report completion without consuming a nonexistent frame or silently
stepping live input.

## 9. Visible fast-forward

Keep the fixed simulation tick unchanged; execute a selected number of
ordinary session steps between draws. Never skip input frames or call a
larger-delta simulation update. Check window-close/debug controls at safe
boundaries, at least between bounded batches, so a large speed setting cannot
make the window unresponsive. Pause or stop-at-target takes effect before
consuming a frame beyond the requested boundary. Preserve audio events in
normal-speed/step mode; provide a simple suppression policy for fast-forward
if playback becomes sonically unusable, without changing simulation.

## 10. Headless run-to-tick design

Use the same session and production `RunOneStep`, with no renderer, no
Raylib window, no target-FPS pacing, and `SilentAudioEvents`. `RunToTick(N)`
should stop at state boundary N, with N in `[0, DurationTicks]`; reject
out-of-range targets rather than clamping. The default headless run ends at
the recording duration. It must not stop early merely because the game enters
Game Over or Level Complete: the remaining recorded restart/neutral frames
are part of the accepted input sequence.

Return the final tick, replay cursor/remaining count, completion reason, and
read-only summary. Optional checkpoint callbacks should observe the exact
boundary without affecting the game.

## 11. Completion and live-input semantics

Keep the visible behavior that the recording ends at its original duration
and live controls resume on the next normal simulation iteration. Distinguish
the simulation tick from the recording cursor after this handoff: the
simulation continues, while the recording cursor stays at duration. A paused
or single-step session at the final recorded frame should display
“recording complete”; resuming then returns to the existing live-input path.
Headless playback has no live handoff and reports success at duration or the
requested earlier boundary.

Backward navigation, when later added, can only reconstruct states covered by
the recording. Once live input has resumed, deterministic rewind beyond the
recorded range is unavailable unless a separate recording of those live
inputs is deliberately introduced.

## 12. State-inspection inventory

A useful diagnostic view should include:

- boundary tick, next recording frame, remaining frames, startup seed/config,
  active displayed/internal level, and `LevelState`;
- player lane, fractional lane position, depth, lives, score, death ticks,
  approach/exit/death/game-over elapsed state, and projection values when
  explaining a rendered transition;
- shots and enemy bullets, including lane, height, rotation, and pending zap;
- Eggs and reserve counts, including state, lane, height, update counter, and
  original/cargo identity where present;
- Flippers' identity/type, lane/height, climb/pause/flip state, direction,
  pivot source/destination/progress, rim-pursuit direction, firing delay,
  pending zap, and programmed-route state;
- Tanker identity, state, ammunition/timer, latent/released cargo and
  associated identities; Spiker state/growth/rotation and persistent Spike
  lane/height; Fuseball/Pulsar state, direction/lane/height and timers;
- wave-ledger original/cargo dispositions, unresolved/resolved obligations,
  pending cargo activation, and reserve counts;
- Super-Zapper charges/flash/display phase, Hurry-Up and pulse phase/value,
  Fuseball animation phase, shot/exit timers, and game-over effect progress.

These are diagnostic candidates, not a request to expose every private field.
Use explicit bounded projections of current objects and identity ledgers.

## 13. State-summary contract

Add a small immutable `GameplayStateSummary` (and typed entity subrecords)
created at a tick boundary. Include the boundary tick and recording cursor so
off-by-one comparisons are visible. Copy collections into deterministic,
stable-order read-only values; do not return mutable game entities or
collections. Include only selected counters and entity state needed to explain
gameplay. Keep it inspectable as human-readable output and serializable
structured JSON. If a necessary timer/identity is private, add a narrow
read-only diagnostic projection rather than public setters or broad reflection.

## 14. Deterministic signatures

An optional per-boundary signature can quickly compare same-build replays,
but current code has no canonical snapshot/hash and its `System.Random`
instance is private with no supported state export. Existing tests compare
selected fields, not complete simulation identity. A summary-derived hash
could miss hidden entity or RNG divergence and is therefore a diagnostic
accelerator, not proof of equivalence or a regression oracle by itself.

If added, version the signature schema, define canonical field/collection
ordering and float representation, and keep the readable summary available.
Limit guarantees to the same build/runtime/startup; do not promise
cross-version or cross-runtime stable hashes or rewrite the recording format
to store them.

## 15. Backward-step design

Do not reverse collisions, RNG draws, scores, ledgers, entity creation, or
other mutations. From boundary N, construct a fresh game from the recorded
startup, silently consume frames `[0, N-1)`, and present boundary N-1. At
boundary 0, return a clear “already at start” result without changing state.
The same session step path must be used during reconstruction, with audio
suppressed and intermediate rendering disabled.

## 16. Rewind and seek semantics

Rewind means reconstruct boundary 0 from the recorded startup. Seek to an
earlier boundary means reconstruct from 0 and stop exactly at the requested
boundary. Run-to-tick and seek use boundary numbering consistently with
forward stepping; e.g. comparing N-1, N, and N+1 means states after those
respective numbers of input frames. Do not seek by indexing an input frame and
pretending the game state is restored. Preserve normal live-input handoff at
the recording end; constrain deterministic seek/rewind to recorded ticks.

## 17. Replay-from-zero performance concerns

One backward step costs O(N) simulation ticks at boundary N; repeated
single-step backward navigation can cost O(N^2) over a long recording.
Forward seek/run-to-target is also O(N) from startup. No performance numbers
were measured, so do not assume these costs are unacceptable or introduce
snapshots preemptively. Initial target ticks and common recording lengths
should be benchmarked before making snapshot decisions.

## 18. Snapshot completeness inventory

If replay-from-zero proves inadequate, a valid snapshot would need to capture
at least: level/config identity and mutable `Level` state/timers; global tick,
player, score/lives, projection state, shots/bullets; all Eggs/reserve and
enemy objects; every Flipper transition/program/direction/firing field;
Tankers and cargo ownership/queues; Spikers and persistent Spikes;
Fuseballs/Pulsars; Super-Zapper and pulse/display state; wave identity
ledger, resolved/unresolved identities, pending activations; Hurry-Up,
death/re-entry/exit/approach/game-over timers and effects; shot cooldown,
animation counters, and playback cursor. It must also restore every
simulation-affecting RNG state and preserve update/collection ordering.

The current `Random` model does not expose a supported serializable state.
Snapshot completeness is therefore not currently demonstrable from startup
metadata alone.

## 19. Snapshot risks

`TempestGame` owns many private counters and collections, while level ledgers
contain identity transitions and entities own private state-machine fields.
A partial copy could render plausibly but diverge later, especially after
death, cargo activation, exit/re-entry, or random events. A snapshot format
would also need versioning, validation, compatibility policy, restore tests,
and memory/restore benchmarks. Do not add snapshots until replay-from-zero
misses a measured workflow requirement and every field (including RNG) has a
demonstrated capture/restore path.

## 20. Audio policy

Headless and backward reconstruction use `SilentAudioEvents`; this already
exists and does not alter game state. Pausing emits no new events, while
already-playing Raylib sounds can finish. A forward step emits the ordinary
events caused by that `RunOneStep`. Fast-forward may use a small switchable
audio gate because simulation runs faster than real audio, but it must be
outside the simulation. Keep audio changes limited to event routing; do not
redesign the accepted sound system.

## 21. Rendering/headless dependency audit

`TempestGame` construction and `RunOneStep` make no Raylib calls. They accept
an `IAudioEvents` adapter and default to `SilentAudioEvents`. `InputSystem`,
`AudioSystem`, `VectorRenderer`, and the current `Program` window/frame loop
are Raylib-facing. Recording JSON and replay frame types are not.

One residual coupling is that `TempestGame` imports `Rendering.Projection`
and changes its static zoom/offset during exit/reverse-zoom updates and fresh
wave restoration. This is presentation state advanced by simulation ticks,
not window/audio access or collision logic; a headless runner can use it
without initializing Raylib. Include it only when a diagnostic must explain
rendered projection or a future complete visible snapshot. The game-over
vector sequence is held and advanced by `TempestGame` and consumes the seeded
random stream, so it is simulation-tick state even though its name/purpose is
visual. `TransitionStarfield` is deterministic and derives its points from
state; renderer-only game-over density is already documented not to affect
simulation.

Minimum seam: dispatch the headless mode before `Raylib.InitWindow` and
`Raylib.InitAudioDevice`, restore projection defaults consistently, construct
the same production `TempestGame` with silent events, and omit
`VectorRenderer.Draw`.

## 22. Proposed APIs and ownership

- `GameplayPlaybackSession`: owns `TempestGame`, replay, absolute simulation
  boundary, recording cursor, completion status, `StepForward`,
  `RunToBoundary`, and fresh-start reconstruction. It centralizes current
  state-specific input/restart gating.
- `GameplayStateSummary` / `GameplayStateInspector`: immutable read-only
  projection and checkpoint generation, not simulation mutation.
- `HeadlessGameplayPlaybackRunner`: loads or accepts a validated
  `GameplayRecording`, invokes the shared session with silent audio, emits
  checkpoints/result, and never constructs Raylib systems.
- A visible debugger controller/input adapter: owns pause, step, speed, and
  target commands, while the existing Program owns Raylib and rendering.

These are candidate names. Keep the session usable in-process by tests and
Copilot automation; let the CLI be a thin composition layer, not a duplicate
simulation.

## 23. Proposed CLI surface

Add a separate development-only mode, for example:

```text
--headless-playback <file> [--until-tick N] [--checkpoint-tick N ...]
```

Default to `DurationTicks`; `--until-tick` denotes a state boundary and must
be within `[0, DurationTicks]`. A checkpoint option emits a read-only summary
at the requested boundary. Keep assertions in typed in-process test/observer
code for the first iteration rather than adding an expression language or
executing recording-supplied scripts. Output a structured summary/result to
stdout and clear diagnostics to stderr. Do not add this CLI now.

## 24. Proposed visible controls

Conceptually provide pause/resume, one-frame advance, slower/faster playback,
fast-forward batch sizing, and run/pause at an entered tick. Add a visible
read-only tick/cursor/status indicator; entity overlays can follow only if
the summary proves insufficient. Use dedicated debugger commands distinct
from gameplay `PlayerInput` and restart/quit input. Do not settle final key
mappings in this analysis; first reconcile keyboard/controller conflicts and
decide how a target tick is entered.

## 25. VS Code Copilot workflows

The in-process API should support a test or diagnostic task that loads v1/v2,
runs to a boundary, and returns a structured immutable summary. Copilot can
then compare N-1/N/N+1, add a typed assertion, run two seeded sessions to
find the first differing summary, or use the same recording for headless and
visible runs. It cannot inspect arbitrary live process memory; only explicit
project-owned summaries/checkpoints and test output are available to it.
Manual gameplay remains Neal's final arcade-fidelity check.

## 26. Recording-fixture policy

- Keep ad-hoc captures in ignored `Temp/`; do not commit them by default.
- Prefer short synthetic/in-memory frames for unit cases.
- Commit a recording only after review as a named scenario fixture with
  provenance, expected level/behavior, checkpoint ticks, format version, and
  an explanation of what accepted behavior it protects.
- Keep fixtures small and focused; avoid machine paths, usernames, unrelated
  personal input, and machine-specific assumptions. Current v1/v2 schema
  stores game input/startup only, not host paths.
- Keep JSON while strict v1/v2 parsing and current input scale remain
  adequate. Fixture updates should accompany an intentional behavior change
  and reviewed expectation updates, not automatic rewrites.

No fixture recording should be added in Phase 1.

## 27. Error and exit-code policy

Keep distinct outcomes for invalid/unsupported recording or CLI input,
simulation exception, failed assertion/checkpoint, requested stop, and
successful completion. Suggested CLI mapping: invalid args/recording = 2;
simulation or assertion failure = 1; success = 0; explicit Ctrl+C/user
cancellation = a documented nonzero cancellation code. Print the actual
failure (including exception details for simulation errors); do not suppress
exceptions or report success-shaped partial results. The reusable API should
preserve typed failure information and not silently catch simulation errors.

## 28. File and security review

Continue using the strict existing loader: no execution of recording content,
no unknown members, unsupported versions, or nonfinite values accepted.
Resolve the explicitly supplied input path once; headless mode must be
read-only and must not call `PrepareOutputPath`, save files, or modify
arbitrary paths. Never infer output paths from recording contents. Consider
a documented input-size/tick ceiling if recordings may be untrusted, since
the current loader reads the whole JSON file before frame validation; select
limits from supported workloads rather than inventing a threshold. Preserve
v1/v2 without rewriting.

## 29. Performance measurement plan

Benchmark release builds with fixed recordings and identical startup/runtime:

- headless simulation ticks/second (including startup separately);
- visible fast-forward ticks/second at several batch sizes;
- replay-from-zero time to representative short, medium, and long boundaries;
- repeated backward-step cost at increasing boundaries;
- only if later needed, snapshot memory per capture and restore latency.

Record machine/runtime/build and recording length, warm/cold conditions, and
timing method. Establish acceptable interaction budgets with Neal; no
throughput or memory values are claimed here.

## 30. Testing strategy

Preserve all four layers: unit/integration, headless gameplay/campaign
regression, visible deterministic playback, and Neal's manual gameplay
acceptance. Add focused tests for frame/cursor/tick boundaries (0, 1, duration),
exactly-one-step behavior, restart frames, pause not consuming a frame,
run-to-target, v1/v2 startup restoration, deterministic repeatability,
completion/live handoff, checkpoint immutability, and error categories.
Exercise existing production progression and death/re-entry without changing
the four-layer authority. A committed fixture is not required initially.

No build or tests were run: this request is read-only analysis and prohibits
implementation. Existing acceptance evidence includes the v1/v2 tests,
same-seed headless round trips, production-startup coverage, and 868 passing
tests in the prior v2 acceptance closeout; that historical count is not a
validation run for this analysis.

## 31. Regression risks

Most likely regressions are off-by-one cursor/state boundaries; a missed
neutral or restart frame; moving current input-gating or `RunOneStep` order;
seeding after Game Over/wave construction has consumed randomness; changing
natural progression or restart behavior; reading gameplay input while
paused; debugger keys leaking into recordings; silently continuing past a
failed checkpoint; different state between visible/headless paths; and
restarting live input at the wrong point. Preserve renderer-only density and
the currently accepted v1/v2/file validation contract.

## 32. Recommended implementation phases

The proposed all-at-once Phase 1 is too broad for one verifiable change.
Order it as:

1. Build the shared playback session and immutable summary; test exact
   boundaries, restart/input gates, determinism, and completion in-process.
2. Add the headless runner API and thin CLI; test v1/v2 loading, target ticks,
   checkpoints, no-Raylib startup, and failures.
3. Route visible playback through the same session; add pause/resume and
   one-tick stepping, then obtain Neal's visible acceptance.
4. Add visible speed/fast-forward and run/pause-to-target, with bounded
   window-event servicing and a regression pass.
5. Measure replay-from-zero and backward-navigation costs before deciding
   whether Phase 2 needs any further machinery.

Stop after each milestone for review; do not combine this work with gameplay
or Level 8/9 changes.

## 33. Exact bounded Phase-1 scope

The first implementation milestone should be Phase 1 above: one session
that restores a recording and executes the current production simulation,
plus read-only summaries and deterministic boundary tests. It should not
include visible controls, new recording metadata, snapshots, a committed
fixture, altered RNG, or gameplay tuning. This proves the critical shared
headless/visible execution contract before exposing controls. If the project
requires an immediately useful headless command, make the CLI the next
separate milestone, not part of this core change.

## 34. Deferred features

Defer backward stepping/rewind until replay-from-zero is measured; snapshots
until replay-from-zero is demonstrably inadequate; loops, bookmarks, arbitrary
start/end editing, state-hash compatibility guarantees, playlist/browser,
attract-mode reuse, rich observation overlays, recording-format changes, and
permanent fixtures. No snapshot or state hash is needed to deliver the
forward-only core.

## 35. Final Git state

After writing only the required ignored prompt/report records, final checks
confirmed `master` and `origin/master` still at
`7032a7219cde69a0ae25b00e5f3bc9ac1ee3ed04`; tracked worktree/index diffs and
the non-ignored untracked list remained empty. No staging, commit, or push
occurred. The copied prompt's SHA-256 matched the supplied attachment.

## 36. Scope confirmation

No production implementation, test, or durable tracked documentation was
changed. No game was launched; no Level 8/9 work, gameplay change, recording
modification, stage, commit, or push occurred. Only the ignored prompt and
this ignored report were written. Stop for Neal's review before
implementation.
