# Reverse Playback Stepping Responsiveness — Read-Only Investigation

Date: 2026-11-12

## 1. Starting Git state

- Branch: `master`, tracking `origin/master`.
- Starting `HEAD` and `origin/master`: `cd678572c009d27fa8e1e1f0059ec45f6db5db11`.
- The tracked worktree and index were clean; there were no nonignored
  untracked project paths.
- The required exact recording existed before investigation.
- The complete substantive task was copied to the paired prompt record.

## 2. Test-recording identity and validity

Exact requested path:

`.\temp\GameplayRecordings\level9-spiker-speed-2.0-2.json`

- Exists; file size: 1,042,692 bytes.
- Format: `tempest-gameplay-input`, version 2, production scenario.
- Startup: displayed Level 9, 10 lives, rim-speed multiplier 1.0,
  random seed 805838616.
- `Level9SpikerSpeedOverride` is absent/null. The current game therefore uses
  `LevelConfiguration.Level9ProductionSpikerSpeed`, 2.0; this is the current
  Level 9 default, not an explicit override in this recording.
- Duration and exact input-frame count: 3,938.
- Strict loading succeeded.
- Full headless playback succeeded with exit code 0. Result: boundary 3,938 of
  3,938, zero frames remaining, `RecordingCompleted`; final state Level 9
  `Complete`, score 11,331, 7 lives, 2 Super Zapper charges, and 11 active
  spikes.
- All recorded input was consumed. The recording was not written or otherwise
  modified and is suitable for measuring replay/reverse responsiveness for
  this Level 9 / default Spiker-speed workload. The final state is not evidence
  that every frame contains active Spiker work.

The full CLI run took 427.801 ms including process startup, recording load,
simulation and diagnostic output. It used the existing headless executable;
no window, renderer, Raylib input, audio device or frame pacing was started.

## 3. Current reverse-scrubbing architecture

The historical reports' description matches the current implementation:

- `GameplayPlaybackSession.ReconstructToBoundary` creates a fresh session,
  replays ordinary recorded steps from boundary zero with silent audio, and
  reattaches the existing audio sink only after replay succeeds.
- `VisiblePlaybackController` publishes the replacement session only after
  reconstruction returns successfully. It clamps backward targets at zero.
- `Program` notices the replaced `Game` reference and rebinds the existing
  renderer. The Raylib window itself is not replaced.
- The paused overlay is built from the controller's current session boundary
  after input processing. Screenshot capture is requested only for paused file
  playback and occurs after drawing that selected game and overlay, before
  `EndDrawing`.
- Reverse reconstruction is synchronous on the main loop. Intermediate replay
  states are not rendered and do not emit historical audio. Normal audio is
  restored for subsequent visible steps.
- While playback is running, arrow controls do not request reverse or
  discrete forward navigation; the session advances one ordinary frame per
  loop. Reverse navigation is available while paused, including at the final
  boundary once paused.

Relevant current source: [GameplayPlaybackSession](../../src/Tempest.Game/Debug/GameplayPlaybackSession.cs),
[VisiblePlaybackController](../../src/Tempest.Game/Debug/VisiblePlaybackController.cs),
[Program](../../src/Tempest.Game/Program.cs), [InputSystem](../../src/Tempest.Game/Systems/InputSystem.cs),
[VectorRenderer](../../src/Tempest.Game/Rendering/VectorRenderer.cs), and
[PlaybackScreenshotCapture](../../src/Tempest.Game/Debug/PlaybackScreenshotCapture.cs).

## 4. Forward versus reverse control-path comparison

Every row below describes a paused visible-playback event. The initial press
and native repeat are reduced to the same Boolean navigation event by
`InputSystem`; the controller does not receive a separate “tap” or “repeat”
kind.

| Path | Input sampled | Requested work per accepted event | Session, audio, renderer and overlay |
|---|---|---|---|
| Right tap | `IsKeyPressed(Right)` | +1 ordinary sequential `StepForward` | Existing session and renderer; normal visible audio; overlay then reflects new boundary. |
| Right held/repeat | `IsKeyPressedRepeat(Right)` | +1 ordinary sequential `StepForward` | Same as Right tap. |
| Shift+Right tap | Right press plus Shift `IsKeyDown` | Up to +5 ordinary sequential steps | Same session; each step uses normal visible audio; overlay reflects actual achieved boundary. |
| Shift+Right held/repeat | Right native repeat plus Shift `IsKeyDown` | Up to +5 ordinary sequential steps | Same as Shift+Right tap. |
| Left tap | `IsKeyPressed(Left)` | Reconstruct to `max(0, N-1)` | Fresh session; silent historical replay, then audio-sink reassignment; renderer rebound after success; overlay reflects target. |
| Left held/repeat | `IsKeyPressedRepeat(Left)` | Reconstruct to `max(0, N-1)` | Same path as Left tap. |
| Shift+Left tap | Left press plus Shift `IsKeyDown` | Reconstruct to `max(0, N-5)` | Same reverse replacement/rebind path. |
| Shift+Left held/repeat | Left native repeat plus Shift `IsKeyDown` | Reconstruct to `max(0, N-5)` | Same path as Shift+Left tap. |

`ShiftDown` changes group size, not event source. At zero, a backward event
returns without reconstruction or wrap. Successful replacement is synchronous:
the main loop cannot poll another input, render, or present a frame while the
replay call is executing. After it returns, `SetGame` is a reference
assignment, the overlay is refreshed, and drawing/pacing resumes. `SetAudioEvents`
is likewise a reference assignment after reconstruction. Neither rebinding
operation replays simulation.

While unpaused, `ProcessControls` advances one normal frame regardless of
arrow input; this investigation found no new running-playback reverse path.
Space remains an initial-press pause toggle.

## 5. Current Left and Shift+Left behavior

- Left press and each accepted native repeat request exactly one earlier
  boundary.
- Shift+Left press and each accepted native repeat request five earlier
  boundaries, clamped at zero.
- Existing deterministic replay and round-trip tests cover backward/forward
  state equivalence, boundary clamping, repeated reconstruction and audio
  suppression/recovery. The relevant current tests are in
  [VisiblePlaybackControllerTests](../../tests/Tempest.Game.Tests/VisiblePlaybackControllerTests.cs),
  [GameplayPlaybackSessionTests](../../tests/Tempest.Game.Tests/GameplayPlaybackSessionTests.cs),
  and [HeadlessGameplayPlaybackTests](../../tests/Tempest.Game.Tests/HeadlessGameplayPlaybackTests.cs).
- This investigation did not run the test suite or interactive game; it made
  no behavior changes and does not claim new manual acceptance.

## 6. Native repeat and held-state findings

- Ordinary held Left depends on the initial `IsKeyPressed(Left)` or a later
  `IsKeyPressedRepeat(Left)` result. Shift+Left uses the exact same Left
  repeat source. Right and Shift+Right use the corresponding same pair for
  Right.
- `InputSystem.ReadPlaybackControls` polls those values once per main-loop
  iteration. It does not poll `IsKeyDown(Left/Right)` for playback and adds no
  application repeat timer. Shift state is polled with `IsKeyDown`.
- `IsKeyDown(Left/Right)` exists for live gameplay input, but the visible
  playback branch samples playback controls instead. It is not a held-arrow
  repeat source while playback is active.
- The synchronous reconstruction blocks all subsequent application polling
  until the request finishes. Raylib/OS repeat-event queuing or dropping during
  that interval was not measured, and is not inferred here.
- No application setting for native keyboard-repeat rate was found in the
  inspected playback path. No Windows keyboard setting was changed. Native
  repeat frequency was not measured reliably because the task prohibits
  launching the interactive game.
- Since Left and Right use the same event sources, native repeat alone does
  not explain the observed directional asymmetry. The differing work per
  event does: Right advances one existing-session step; Left reconstructs
  from startup.

## 7. Reconstruction timing methodology

A temporary external .NET 9 harness referenced the existing game project and
used the public recording/session/controller APIs. It loaded the specified
recording once, warmed the simulation through its duration, and used five
timed samples per route/target. Reported values are medians with observed
sample ranges where applicable.

- Fresh reconstruction: construct a new `GameplayPlaybackSession` and replay
  from boundary zero to the target. Timing includes session/game startup and
  simulation, but excludes file loading, process startup, renderer, Raylib,
  audio-device setup and presentation pacing. It uses silent audio.
- Sequential advance: prepare a session at the named anchor outside the
  timed interval, then time 100 ordinary `StepForward` calls and divide by
  100. At the near-final anchor only the remaining 100 boundaries were
  measured. No session creation or rendering is included.
- Reverse-control timing: prepare the controller at `N-1`, pause it, advance
  one sequential step to the final paused boundary `N`, then time the actual
  Left or Shift+Left controller event.
  This includes replay and audio-sink reassignment; it excludes Program's
  subsequent renderer reference assignment and drawing.
- Direct `ReconstructToBoundary` comparisons use an already prepared session
  at N and time the reconstruction call. Allocation counts are
  `GC.GetAllocatedBytesForCurrentThread` deltas.

The measurements are bounded diagnostics, not a stable benchmark across
machines. Target timings are not assumed to be linear in boundary number.

## 8. Early/middle/late timing measurements

| Target boundary | Recording % | Route | Median elapsed | Five-sample range | Session startup included? | Renderer/Raylib? |
|---:|---:|---|---:|---:|---|---|
| 39 | 0.99% | Fresh session + replay from 0 | 0.928 ms | 0.924–0.981 ms | Yes | No |
| 984 | 24.99% | Fresh session + replay from 0 | 38.563 ms | 38.039–39.794 ms | Yes | No |
| 1,969 | 50.00% | Fresh session + replay from 0 | 96.703 ms | 90.080–99.056 ms | Yes | No |
| 2,953 | 74.99% | Fresh session + replay from 0 | 63.062 ms | 61.981–75.137 ms | Yes | No |
| 3,937 | 99.97% | Fresh session + replay from 0 | 89.002 ms | 88.726–89.964 ms | Yes | No |

The early target is quick; middle and late targets cost tens to about 100 ms.
The profile is not monotonic, consistent with work depending on simulated
state rather than target count alone. The 75% sample range is wider than other
targets, so its median is less precise.

Five-sample sequential per-boundary medians (100 steps per sample, session
already at the anchor) were:

| Starting boundary | Recording % | Median per `StepForward` | Range |
|---:|---:|---:|---:|
| 39 | 0.99% | 0.0138 ms | 0.014–0.014 ms |
| 984 | 24.99% | 0.0145 ms | 0.014–0.015 ms |
| 1,969 | 50.00% | 0.0239 ms | 0.024–0.030 ms |
| 2,953 | 74.99% | 0.0366 ms | 0.035–0.054 ms |
| 3,838 | 97.46% | 0.0010 ms | 0.001–0.001 ms |

## 9. -1 versus -5 target-cost comparison

At `N = 3,938`, five timed actual controller requests produced:

| Route | Target | Median | Sample range |
|---|---:|---:|---:|
| Paused Left | 3,937 (`N-1`) | 115.338 ms | 101.002–122.290 ms |
| Paused Shift+Left | 3,933 (`N-5`) | 106.279 ms | 100.767–115.050 ms |

The comparison uses small samples with overlapping ranges; it does not
establish that one target is faster. More importantly, both routes replay from
zero and have comparable elapsed cost, while Shift+Left moves five boundaries
per accepted event. This supports the existing faster-navigation fallback;
it does not make an individual reverse reconstruction cheap.

## 10. Hypothetical -10 target cost

Direct `ReconstructToBoundary` timings at the same `N = 3,938`:

| Route | Target | Median | Sample range | Allocated bytes per sample |
|---|---:|---:|---:|---:|
| `N-1` | 3,937 | 111.675 ms | 104.801–120.735 ms | 101,032,936 |
| `N-5` | 3,933 | 118.235 ms | 106.582–120.797 ms | 101,032,936 |
| Hypothetical `N-10` | 3,928 | 116.180 ms | 101.247–126.078 ms | 101,032,936 |

The ranges overlap and the allocation count is identical. Ten fewer target
frames did not materially reduce cost in these measurements; the replay from
zero dominates. This does not justify adding -10 or progressive grouping.

## 11. Presentation-speed interaction

`InteractiveSpeedOptions` maps the selected multiplier to Raylib's target
frame rate; `Program` calls `Raylib.SetTargetFPS` before the main loop:

- `--speed 0.5`: 30 FPS
- `--speed 1.0` or default: 60 FPS
- `--speed 2.0`: 120 FPS

`ReadPlaybackControls` is sampled in the main loop, and `EndDrawing` applies
presentation pacing. Thus the option can change the number of input-poll
opportunities per wall-clock second and can affect perceived response. During
synchronous reconstruction the loop is blocked, so the selected frame rate
does not make the replay itself faster; it only affects scheduling/polling
before and after the block. The source and existing state-independence test
show no change to simulation state for a given replay boundary. No interactive
measurement was made, and no claim is made about how Raylib/OS repeat events
are queued at those rates.

## 12. Primary bottleneck classification

**A. RECONSTRUCTION-LIMITED**

Measured mid/late replay-from-start costs are roughly 39–97 ms, versus
approximately 0.014–0.038 ms for a sequential step at comparable anchors.
At the final target, reverse reconstruction took about 106–118 ms in these
samples. It also allocated 101,032,936 bytes per near-final reconstruction.
The call blocks the main loop and therefore delays the next input poll and
render. This is sufficient to explain why one-boundary reverse advances much
more slowly than forward stepping.

Native repeat is the common external event source and its rate is unmeasured;
it remains a possible shared ceiling, not an evidenced cause of the
Left/Right difference. The classification is based on the directly measured
reconstruction cost and source-confirmed per-event work asymmetry.

## 13. Simple held-Left improvement evaluated

No evidence supports a small, clean held-Left change that resolves the
measured bottleneck while preserving the accepted model:

- A custom repeat timer or key-down polling could produce more requests only
  when the main loop is available. Each reverse request would still block on
  replay and allocate about 101 MB near the final boundary. Native repeat
  timing was not measured, so replacing it would add behavior without
  evidence that it is the limiting cause.
- The work surrounding replay is small in source: after the initial
  constructor/JIT sample, session construction had a 0.044 ms median (the
  first sample was 13.996 ms); `SetAudioEvents` and `VectorRenderer.SetGame`
  are reference assignments. They do not explain tens of milliseconds.
- The measured cost is the replay/session work itself. Removing that cost
  safely is not an established one-line input-path correction.

No change is recommended to native-repeat input, tap semantics, or the
controller.

## 14. Existing-control-model fallback evaluation

**Acceptable and recommended for this milestone.** Keep Left for exact
one-boundary inspection and Shift+Left for five-boundary navigation. The
five-frame group takes approximately the same reconstruction time as a
one-frame group near the final boundary, so it increases navigation distance
per event without changing the reconstruction algorithm. Preserve the
manually accepted native-repeat workflow.

## 15. Reconstruction optimization evaluation

Replay-from-start is the measured dominant work. The near-final reconstruction
allocates about 96.3 MiB on the timed thread. Constructor-only timing and
source inspection rule out renderer and audio-provider rebinding as meaningful
cost centers. The current task does not isolate which simulation subsystems
or allocation sites account for that memory/time, so no particular internal
micro-optimization is established as safe. A separate bounded profiler task
would be needed before proposing an implementation; it is not needed to
choose the existing controls now.

## 16. Snapshot/checkpoint justification

**Not justified now.** Although the measured replay cost is substantial,
Shift+Left -5 is already an accepted practical faster-navigation solution and
costs about the same per event as -1. The task provides no evidence that this
fallback fails Neal's practical navigation needs. Snapshots/checkpoints would
add state capture/restoration and deterministic fidelity risk without being
necessary to meet the current accepted control requirement.

## 17. Larger/progressive grouping justification

**Not justified.** No -10, progressive acceleration or new group size is
recommended. The existing -5 request already moves five boundaries for
roughly one reconstruction cost and is the approved fallback.

## 18. Recommended next outcome

**B. KEEP THE EXISTING CONTROL MODEL.**

Do not implement a responsiveness correction in this milestone. Leave Left
and Shift+Left unchanged. If Neal later finds the existing -5 fallback
insufficient, investigate allocation/performance profiles as a separate
authorized task before considering a larger control redesign or snapshots.

## 19. Exact behaviors that must remain frozen

1. Left tap/repeat remains one boundary backward; reliable exact frame
   inspection is preserved.
2. Shift+Left remains five boundaries backward per accepted event.
3. Right remains +1 and Shift+Right remains +5; their behavior is unchanged.
4. Space pause/resume is unchanged.
5. Replay reconstruction remains deterministic and starts from recorded
   startup.
6. Historical audio remains suppressed; visible audio resumes after rewind.
7. Boundary zero clamps without wrapping; a paused final boundary remains
   rewindable.
8. `-1/+1` and `-5/+5` visual/state round trips remain deterministic.
9. The paused boundary overlay continues to show the selected boundary.
10. Live-input handoff behavior remains unchanged.
11. Screenshots remain bound to the selected paused playback boundary.
12. Recording bytes/format and gameplay simulation/recording semantics remain
    unchanged.
13. Reverse behavior does not apply while normal playback is running.

## 20. Files or temporary artifacts changed by this investigation

- Created the required ignored prompt record and this required ignored report.
- Created a temporary .NET measurement harness under the session-state
  `files` directory. It referenced the existing project and recording without
  changing production source, tests, durable documentation, or the recording.
- The harness directory was removed before completion. The project build may
  have refreshed ignored build outputs; no tracked build/source changes are
  present.
- No screenshot, `UserData`, `TEMP`/`temp`, video, ROM/reference, recording,
  Git configuration, production source, test, README, or durable behavior
  document was changed.

## 21. Temporary-artifact cleanup

Confirmed the external measurement harness directory no longer exists.
No temporary analysis artifact remains in the repository or appears as a
nonignored untracked path.

## 22. Final Git status

Final verification after report creation:

- `git status --short --branch`: `## master...origin/master`
- `HEAD` and `origin/master`: both
  `cd678572c009d27fa8e1e1f0059ec45f6db5db11`.
- Tracked and staged diff stats are empty; no nonignored untracked project
  paths exist. AI-Logs records remain ignored.

## 23. Behavior-change confirmation

No gameplay, playback, input, screenshot, or recording behavior changed.
The interactive game was not launched. No manual gameplay acceptance is
claimed or required for this read-only diagnostic.

## 24. Commit/push confirmation

No commit, push, staging, reset, checkout, stash, cleanup of user content, or
Git configuration change occurred.
