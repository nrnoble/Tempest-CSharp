# Simulation Timing, Arcade Cadence, and Future Speed Option

Date: 2026-11-08

## Executive summary

- **C# tick:** `TempestGame.SimulationStepSeconds` is exactly `1.0 / 60.0`, or
  16.6666667 ms nominally. `Program` requests 60 host frames per second and
  executes one `RunOneStep` followed by one render in each ordinary loop pass.
  The model itself receives no elapsed-time value.
- **Overload behavior:** this is a one-update-per-host-loop design, not an
  elapsed-time accumulator. If the host cannot meet its target, simulation
  ticks arrive more slowly in wall-clock time; the program neither performs
  catch-up updates nor skips an update while continuing to render. A fast host
  is nominally limited by Raylib's 60-FPS target.
- **Arcade cadence:** the local ROM establishes that the gameplay loop waits
  for nine increments of an interrupt-updated counter, then performs one main
  loop pass. It does not establish the interrupt frequency, CPU clock, or
  normal gameplay display/AVG cadence. Therefore the arcade loop's exact Hz
  and milliseconds per update are **UNKNOWN** from the checked-in evidence.
- **Comparison:** C# is a deliberately fixed 60-Hz project model, not a
  demonstrated literal translation of an established arcade update rate.
  It is partly calibrated by source translation and manual acceptance, but
  the current evidence cannot certify a direct real-time cadence match.
- **Cause of feel difference:** there is no evidence that global C# speed
  causes the slightly harder/longer feel. Neal reports the overall timing is
  very solid; the ROM's real-time rate is unresolved, and separate enemy,
  release, ordering, shot, and difficulty differences remain plausible.
- **Future `--speed`:** changing only wall-clock pacing can preserve the
  deterministic simulation at tick N, provided `RunOneStep` and its
  per-tick constants remain unchanged and the same seed/input sequence is
  supplied. A target-FPS multiplier is the smallest implementation under the
  current one-update/one-render loop; an accumulator is needed only if
  simulation pacing must be independent of render frequency or maintain
  catch-up behavior under load.

## 1. Starting repository state and scope

At start, `HEAD` was
`e3a3d4cf31449bc710c43d9d912ca5954605ade8`
(`Close out accepted ROM-derived Blaster-shot rendering`) on `master`, tracking
`origin/master`. The only initial status entries were pre-existing untracked
`screenshot000.png` through `screenshot013.png`; no tracked or staged changes
were present. They were left untouched.

This was a read-only investigation of production code and evidence. No game
was launched, no automated test or long-running experiment was run, and no
production code, tests, durable behavior/development documents, configuration,
project files, recordings, screenshots, or Git state were changed. Per the
requested project-record policy, only this report and its paired prompt were
written under ignored `AI-Logs/`.

### Evidence inspected

- C# host/model: `src/Tempest.Game/Program.cs`,
  `TempestGame.cs`, `Systems/InputSystem.cs`, `Systems/AudioSystem.cs`,
  `Systems/SuperZapperSystem.cs`, `Gameplay/Shot.cs`, `EnemyBullet.cs`,
  `Egg.cs`, `Flipper.cs`, `Tanker.cs`, `Levels/Level.cs`,
  `Levels/LevelConfiguration.cs`, `Levels/LevelApproach.cs`,
  `Gameplay/EnemyDestructionEffectPool.cs`, and rendering code in
  `Rendering/VectorRenderer.cs`.
- Recording and playback: `Debug/GameplayRecordingFile.cs`,
  `GameplayPlaybackSession.cs`, `VisiblePlaybackController.cs`,
  `HeadlessGameplayPlaybackRunner.cs`, and
  `HeadlessGameplayPlaybackCli.cs`.
- Automated evidence: `SimulationTests.cs`, gameplay recording/playback and
  headless playback tests, `VisiblePlaybackControllerTests.cs`,
  `CampaignSoakTests.cs`, `AudioEventTests.cs`, and timing-related assertions
  in gameplay/input/death/transition tests.
- Durable project references: `TEMPEST-DEVELOPMENT-NOTES.md` and
  `TEMPEST-BEHAVIOR-SPEC.md`.
- Prior project investigations: the 2026-11-04 local-ROM difficulty/fire/rim
  report, 2026-11-05 accepted rim-speed closeout, and 2026-11-07 headless and
  reverse-playback reports.
- Arcade authority: the sole checked-in arcade source is
  `Reference/arcade/Tempest Rom Source code.md`. No separate checked-in
  hardware/driver timing reference was found. No external disassembly or
  internet source was consulted.

Evidence labels in this report distinguish **ROM-LISTING-BACKED**,
**EXISTING C# IMPLEMENTATION**, **AUTOMATED-TEST EVIDENCE**,
**PREVIOUSLY ACCEPTED PROJECT BEHAVIOR**, **MANUALLY ARCADE-ACCEPTED**,
**PROJECT DECISION**, **INTERPRETATION / HYPOTHESIS**, and
**UNKNOWN / UNRESOLVED**.

## 2. Current C# host-loop timing

### Loop flow

`Program` handles headless playback before Raylib/window setup. Interactive
startup calls `Raylib.SetTargetFPS(60)` at `Program.cs:143`. The ordinary loop
at `Program.cs:201-315` then:

1. polls quit and applicable playback controls;
2. samples live player input at `Program.cs:257` when normal play is active;
3. calls `game.RunOneStep(effectiveInput, effectiveRestartPressed)` at
   `Program.cs:259` exactly once, except when visible playback controls have
   already consumed the iteration or playback is paused;
4. captures the input frame for recording, if enabled; and
5. calls `renderer.Draw(...)` at `Program.cs:315` once.

`VectorRenderer.Draw` (`Rendering/VectorRenderer.cs:183-199`) brackets
presentation with `Raylib.BeginDrawing()` and `EndDrawing()`. There is no
elapsed-time argument to `TempestGame.RunOneStep` (`TempestGame.cs:292`), no
`GameTime`-style update, no stopwatch/accumulator in the normal loop, and no
independent render-skip path. `SimulationStepSeconds` at
`TempestGame.cs:23` is only declared there; a source search found no use of
that constant in runtime code. Actual pacing is supplied by the host loop.

| Property | Finding | Evidence |
|---|---|---|
| Requested render/loop rate | 60 FPS | `Program.cs`: `Raylib.SetTargetFPS(60)` |
| Nominal simulation rate | 60 calls/second when the loop sustains its target | `Program.cs` loop; one `RunOneStep` per ordinary iteration |
| Nominal tick duration | `1/60 s` = `16.6666667 ms` | `TempestGame.SimulationStepSeconds` (`TempestGame.cs:23`, declarative only) |
| Fixed or variable update | Fixed model step, but scheduled once per loop iteration rather than by a fixed-step accumulator | `RunOneStep` has no elapsed-time parameter |
| Pacing | Raylib target-FPS frame limiter | `SetTargetFPS(60)` plus `EndDrawing()` |
| VSync | No explicit VSync flag or swap-interval configuration appears in the C# startup path | Source inspection; platform/driver behavior is not measured |
| Catch-up / cap | No accumulated elapsed time and no multi-step catch-up cap | One simulation call in the loop |
| Slow host | Fewer ticks per real second; game time runs slow relative to wall time | Consequence of one step per loop |
| Fast host | Normally waits at the 60-FPS target | Raylib target-FPS request |
| Render decoupling | None; one draw follows each ordinary loop pass | `Program` loop |
| Pause | Visible-playback pause suppresses simulation but continues the loop and drawing; ordinary live play has no general pause | `VisiblePlaybackController` and `Program` |

**VSync/monitor conclusion:** the source does not explicitly enable VSync, so
no source evidence says monitor refresh selects the simulation rate. The
simulation is nevertheless coupled to loop iteration count: if a platform,
driver, or future VSync setting blocks presentation, that blocking can reduce
simulation calls per second. No hardware/display measurement was performed.

There is no deliberate “drop a simulation tick but keep wall time” policy.
When work takes longer than the target interval, the loop simply starts its
next single update later. Thus it loses real-time cadence rather than skipping
logical simulation indices. When the computer has spare capacity, the target
FPS limiter is intended to prevent an unbounded update rate.

### Rendering and interpolation

Rendering reads the current game state; no render interpolation between
simulation states was found. Gameplay visual effects with lifetimes or phases
are advanced by simulation calls and then drawn from their current state.
`PlayerDeathPresentation`, `GameOverVectorBurstSequence`, the Super Zapper
display phase, the approach presentation, and destruction effects use tick
progress rather than elapsed wall time. The window presentation itself is
handled by Raylib.

Audio events are issued by simulation/event code, while Raylib plays sound
through its audio device. Audio sample playback has its own device clock; the
game does not scale sample rate or pitch with simulation ticks. Event
occurrence timing still follows the simulation loop.

## 3. What one C# simulation tick means

`RunOneStep` is the authoritative boundary for gameplay, timer, and most
animated presentation work. The exact executed work depends on `LevelState`;
not every subsystem advances in every state. All values below are per-call,
not per elapsed millisecond.

| System / behavior | Per-tick expression in C# |
|---|---|
| Blaster movement | `PlayerSpeed` and `SpinnerSensitivity` are applied once to the sampled input in `UpdatePlayerMovement`. Mouse delta is a per-loop sample; keyboard/controller input is applied per call. |
| Player shots | `Shot.Update` reduces height by 6 each active update. Fire cooldown is 3 simulation calls in ordinary play (4 during the specified Spike-exit travel); a held fire can create at most one shot per eligible call, subject to the 10-shot cap. |
| Enemy movement | Flippers, Tankers, Spikers, Pulsars, Fuseballs, and other active objects update in the active gameplay path. Their configured movement/turn values are units per update; timers and pauses decrement by update. |
| Enemy firing | Per-enemy bullet delays/holdoffs are tick countdowns. Firing decisions, including random rolls, occur during enemy updates; seeded random draws are tied to the sequence of simulation calls, not elapsed time. |
| Eggs / release | Egg movement advances every eighth Egg update. Reserve release and hatch-selection timers count calls; current release intervals are configuration values (often 50 ticks) and are project configuration, not wall-clock timers. |
| Tanker behavior | Its current climb amount is applied per update (the implementation's default is 1.75 height units); release and firing timers are simulation-tick based. |
| Rim behavior | Flipper turn angle and pauses advance by per-update configuration. `Level.RimTurnSpeedCalibrationMultiplier` is 2.5; this is an accepted/manual-calibration mechanic multiplier, not a global clock conversion. |
| Collision/order | Collision checks are ordered within `RunOneStep`: player update/fire, player shots, enemy bullets, egg/enemy updates and enemy firing, then player-contact checks and wave/exit checks. A change to call order can change outcomes without any cadence change. |
| Death / re-entry | Ordinary player death lasts 60 calls (1 nominal second at 60 Hz), with game entities frozen under the existing lifecycle rules; re-entry reconstructs entities according to the current level policy. |
| Exit / transition | Exit delays and travel are update-driven. `LevelApproach.DurationTicks` is 181 calls (about 3.017 nominal seconds). The existing development notes describe a 240-interval combined travel/approach at normal 60-FPS pacing as approximately four seconds. |
| Super Zapper | Charge/target behavior is event-driven; its display phase and 60-tick flash lifetime advance at the `RunOneStep` boundary. |
| Destruction / Game Over effects | Destruction effect age, death presentation, Game Over text counter (120 ticks), burst sequence, and score-popup lifetime advance by calls. |

Some counters use modulo or threshold conditions, and random enemy decisions
are made on updates. Those decisions therefore occur more or less often per
wall-clock second if host pacing changes. In active play, player shots and
enemy bullets move before later collision processing; newly created enemy
bullets and Tanker cargo can begin moving on the next tick. The update-order
rules are independent of the requested host rate.

**Interpretation:** because tick-based motion and all countdowns share this
boundary, changing global simulation rate would scale their wall-clock rates
together. That does not make individually different movement values,
randomization, supply/release policy, or collision order equivalent to the
arcade.

## 4. Recording, playback, tests, and determinism

- **Recording:** `PlayerInputRecorder.CaptureTick` stores a contiguous
  simulation-tick index, input values, and restart press. Version-2 files also
  carry starting level, lives, rim-speed multiplier, and random seed.
  `DurationTicks` equals the exact number of input frames. There is no
  wall-clock timestamp, inter-frame delay, host FPS, or speed multiplier.
- **Visible playback:** the controller advances one recorded tick per normal
  host iteration when running. Space pauses/resumes; paused Right advances one
  boundary (or five with Shift); paused Left reconstructs the prior boundary
  by restoring startup and replaying from tick zero. The simulation does not
  advance while paused, but `Program` continues input/UI handling and drawing.
  Reverse reconstruction is synchronous replay, not negative-time simulation.
- **Headless playback:** `HeadlessGameplayPlaybackRunner` loops through
  `GameplayPlaybackSession.StepForward` with no Raylib initialization or
  real-time throttle. It runs as fast as the process allows. It uses the same
  production `TempestGame.RunOneStep` and gates as visible playback, with
  silent audio.
- **Automated tests:** playback tests compare state summaries for equal seeds,
  recorded input sequences, and boundaries; headless tests verify fresh runs
  match shared-session results. Campaign soak tests execute deterministic
  tick loops and assert state boundaries. Pause/step/reverse tests assert
  simulation boundaries. These establish deterministic per-tick behavior and
  boundary semantics, not 60 wall-clock updates per second.
- **Audio/screenshots:** playback audio events occur at simulation step
  boundaries; reverse reconstruction suppresses historical audio. Paused
  screenshots capture the rendered state at a boundary. Neither feature
  inserts timing into the recording.

Therefore changing real-time playback pacing should not invalidate existing
recording files or change the state reached at tick N, provided simulation
code, seed, and input sequence remain unchanged. Determinism is not the same
as presentation speed: the same boundary can be displayed at a different
wall-clock time.

## 5. Arcade ROM cadence and effective update rate

### ROM-backed control flow

The local annotated master listing establishes this software structure:

1. `$c7a0-$c7bb` is the gameplay main loop. It waits until the RAM counter
   `$53` reaches `#$09`, clears `$53`, then calls `$c7bd`, `$c891`, and `$b1b6`
   before branching back.
2. The common NMI/IRQ/BRK handler increments `$53` at `$d7a5`.
3. `$c7bd` calls `$a7d2` and dispatches the current state via the jump table.
   The normal Playing handler (`state_04`, `$970b`) calls player movement
   (`$9749`), player firing (`$a23f`), zap processing (`$a83a`), enemy
   creation (`$98a2`), enemy movement (`$9b1e`), shot movement (`$a18f`),
   enemy firing (`$a2a6`), shot-hit processing (`$a454`), and further
   collision/state routines (`$a416`, `$a504`).
4. `timectr` increments once per completed gameplay-state pass at `$c8e3`;
   bit 0 controls a conditional call at `$de1b`. The listing does not establish
   that this is a skipped gameplay/display frame, so it is recorded as a
   divider/conditional path, not proof of an alternating simulation rate.
5. Input handling is not a single-rate fact: `$a7d2` is called from the
   gameplay pass, while the interrupt/input path samples and debounces the
   `zap_fire_starts` bits. Sound service is also advanced from the interrupt
   path (`$cf24` / `$cd0a` before the `$d7a5` counter increment); gameplay
   events call sound routines such as `$ccea`.

**ROM-LISTING-BACKED derivation:** the gameplay main-loop gate is one pass per
nine increments of `$53`; `timectr` counts those software passes. A normal
Playing-state pass updates its player/enemy/shot logic once. `$53` is
interrupt-driven, but the listing does not establish the interrupt frequency
or prove which hardware source invokes the shared handler at a known rate.
Thus no exact updates/second or milliseconds/update can be derived.

### Display/AVG and higher-frequency work

The self-test loop at `$da8d-$daa9` counts 21 annotated 3-kHz signal
transitions and polls the vector halt/done bit; it starts the vector generator
with `vg_go` around `$dae2`. This is direct evidence of that self-test's
polling sequence only. It does not establish the normal game's display refresh
rate, nor a one-display-per-gameplay-pass relationship. No checked-in
hardware/driver reference resolves CPU clock, IRQ frequency, AVG completion
rate, or the relation between the main loop and visible monitor refresh.
Display-list generation/presentation cadence and any game-loop/display
alternation remain **UNKNOWN / UNRESOLVED** from the available local evidence.

### Frequency conclusion

| Candidate conclusion | Evidence |
|---|---|
| One gameplay update per 60-Hz display frame | Not established |
| One update every two display frames | Not established |
| Nine IRQ/handler counter increments per main gameplay loop pass | ROM-backed at `$c7a0-$c7bb`, `$d7a5` |
| Exact gameplay updates per second / duration per update | Unknown: interrupt rate is not established |
| Separate faster interrupt services | ROM-backed interrupt service exists for input/sound, but its absolute rate is unknown |
| Certain subsystems have software sub-cadences | ROM-backed `timectr` bit tests and other counters; their wall-clock rates cannot be converted without IRQ/main-loop frequency |

No decimal arcade rate is inferred from a nominal 60-Hz display assumption.

## 6. Medium difficulty and C# comparison

### ROM difficulty path

The prior local-ROM investigation traces `get_diff_bits` to the low bits of
`zap_fire_starts` at `$60d8`, with `read_optsws` saving the setting in
`diff_bits` (`$016a`). The source comment and `diff_str_tbl` map `00=Medium`,
`01=Easy`, `10=Hard`, and `11=Medium`. The ROM therefore establishes the
decode; it does not establish a factory/default physical switch position.
Neal's task context states that his cabinet is set to Medium.

The per-level parameter path `$92c5` / `$9328-$93df` applies Easy/Hard
adjustments after level-table values are loaded. The local ROM analysis found:

- Easy reduces enemy-shot capacity, Flipper speed, and (on early levels)
  top-rim acceleration.
- Hard increases/caps enemy-shot capacity, increases Flipper speed, increases
  wave enemy supply, and enables Pulsar fire where applicable.
- Derived hit tolerances are recomputed from converted movement speeds.
- Player Fire sampling/cadence, player-shot slot count/speed/lifetime,
  enemy-shot speed/holdoff, Spiker speed, concurrent `max_enm`, per-type
  min/max values, Spike behavior, and pending-enemy timing are not changed by
  the described difficulty branch.
- No Easy/Hard branch changes the `$c7a0` threshold, `$53` interrupt
  increment, or global loop structure.

These are **ROM-LISTING-BACKED** and reuse
`AI-Logs/Reports/2026-11-04-blaster-fire-rim-difficulty-rom-investigation-report.md`.
The 2026-11-04 report noted that the cabinet setting had not then been
independently verified; the current task's statement that it is Medium is
Neal's supplied context, not a new hardware readout.

### C# comparison and likely compensation

The C# project exposes no DIP/difficulty selection; it runs its fixed
per-level configuration. Its values can be compared with the ROM's Medium
baseline, but no claim is made here that every fixed C# parameter exactly
matches Medium. `--rim-speed` is a distinct per-mechanic diagnostic; the
accepted 2.5 rim-turn calibration is **PROJECT DECISION / MANUAL CALIBRATION**,
not a conversion between arcade and C# global tick rates. Other C# movement,
fire, release, and pause values are per-tick translations/configuration and
should not be silently rescaled.

Overall classification: **PROJECT DECISION plus manual gameplay calibration,
with arcade cadence equivalence unsupported by the available timing evidence**.
The source establishes C#'s nominal 60-Hz wall-clock target and the ROM's
nine-interrupt-counter main-loop gate, but not the ratio between them.

## 7. Does global speed explain the harder/longer feel?

No cause is established. Relevant evidence:

- Neal reports the accepted game's overall gameplay timing is very solid,
  possibly perfect; this task did not measure comparative elapsed time.
- Previously accepted transition timing is described in the development notes
  as approximately four seconds at the C# normal 60-FPS cadence. That is a
  project/manual-acceptance anchor, not a measured ROM cadence.
- The accepted Level 1-7 rim pursuit speed is manually calibrated (2.5x the
  former configured rate) and has its own report; it is not evidence that the
  whole simulation is globally too fast or slow.
- The arcade's absolute gameplay rate remains unresolved. Therefore the
  available evidence cannot rule out a small global-rate difference, but
  cannot support one as the cause either.
- Medium/Hard changes to enemy supply, enemy firing capacity, enemy movement,
  top-rim acceleration, and Pulsar firing can change threat and wave duration
  without changing the global loop cadence. C# has no selectable difficulty.
- Separate likely candidates remain pending/release pacing, active/per-type
  limits, collision/update order, shot persistence and cooldown, enemy
  selection/movement programs, death/re-entry, and fixed difficulty
  parameters. These are mechanic or ordering differences, not evidence of a
  clock mismatch.

**Conclusion:** global speed is a possible but unisolated hypothesis.
Mechanic-level and difficulty differences are more directly supported as
independent ways to alter feel. No constants should be changed on this
evidence.

## 8. Future `--speed` feasibility and recommended semantics

Recommended semantics:

- Default `1.0` preserves current normal behavior and 60-FPS target.
- Positive finite `N` changes only real-time pacing: nominal simulation rate
  is `60 * N` ticks/second. `2.0` is twice normal wall-clock speed;
  `0.5` is half normal. Do not multiply movement, timers, enemy speed, RNG,
  or gameplay constants.
- Given the same random seed and input frame for every simulation tick,
  state after tick N remains identical at every multiplier.
- Keep headless playback unthrottled unless a separate explicit throttle is
  requested. Do not put pacing metadata in recordings.
- Pause, step, and reverse remain defined by simulation boundaries, not
  wall-clock duration.
- Audio sample pitch/rate remains unchanged; only event spacing in real time
  changes. Test overlap/voice limits at faster settings.

### Smallest current-loop implementation

The smallest future change is to parse and validate `--speed`, then set the
Raylib loop target to an integer target near `60 * speed`, leaving the
one-`RunOneStep`/one-draw structure and every gameplay constant unchanged.
This is sufficient for a convenience/diagnostic speed control at ordinary
values such as 0.5, 1, and 2, provided the computer and presentation path can
meet the requested rate. Preserve the existing default 60 target exactly.

This is not a hard real-time guarantee. The current loop has no elapsed-time
accumulator, so under load it still runs fewer ticks than requested; a
platform/driver presentation block can also cap the loop. A multiplier that
must remain exact despite render-rate limits or temporary stalls would need a
monotonic-clock fixed-step accumulator, independent render scheduling, and an
explicit maximum catch-up/drop policy. That is a larger architectural change
and should not be added merely to support the initial diagnostic option.

Input sampling is presently once per host iteration. At slower targets,
edge-triggered input may be sampled less frequently; at faster targets,
mouse/controller samples arrive more frequently. Playback inputs are already
per-tick and are unaffected. Any future accumulator must define how live input
is sampled and assigned across zero, one, or multiple simulation steps; mouse
delta must not be applied repeatedly on catch-up steps. Rendering would run at
the chosen loop cadence in the minimal approach; at 0.5 it is nominally 30
FPS, and at 2 it may request 120 FPS, which can increase CPU/GPU use and may
exceed the monitor's useful presentation rate.

Validation should reject zero, negative, NaN, infinity, parse failures, and
out-of-policy extreme values with an explicit command-line error. A practical
allowed range is a product decision; a bounded range such as 0.25-4.0 would
avoid absurd frame targets while covering the requested values. Any bound
must be documented and tested rather than silently clamped.

## 9. Proposed future validation (not performed)

### Automated tests

1. No option and `--speed 1` retain the existing target/behavior.
2. Parsing accepts the documented finite range and rejects zero, negatives,
   NaN, infinities, malformed values, duplicates, and values outside bounds.
3. With the same seed and per-tick input, state after N steps is identical
   for 0.5, 1, and 2; no gameplay constant is multiplied.
4. Test nominal interval calculation (`1 / (60 * speed)`) and target-FPS
   integer conversion, or use an injected monotonic clock if an accumulator
   is chosen.
5. Pause performs no simulation step; single-step advances one (or the
   existing requested group); reverse reconstruction restores the same
   boundary state.
6. Existing recording files remain readable and contain no speed/timing data;
   visible playback is paced by the selected host setting; headless playback
   stays unthrottled.
7. Verify input sampling/assignment policy, audio event ordering and voice
   behavior, rendering responsiveness, and no catch-up spiral or render
   starvation under the selected design.
8. Run existing gameplay, determinism, playback, pause/step, and campaign
   regression tests unchanged.

Timing assertions should use a fake clock or tolerant integration bounds,
not flaky exact wall-clock sleeps.

### Bounded manual acceptance

At 1.0, 0.5, and 2.0, Neal can launch the same fresh short recording, compare
visible playback, pause, single-step, and reverse to matching boundaries, and
confirm wall-clock durations are approximately normal, doubled, and halved.
Compare state at the same tick boundary and verify audio remains intelligible
and rendering usable. Separately test live input at the slow and fast rates.

This would accept a diagnostic/convenience feature only; it would not show
that the accepted normal speed is wrong or replace manual arcade-fidelity
acceptance.

## 10. Unknowns and final state

Unresolved evidence includes the physical arcade interrupt frequency and CPU
clock assumption, normal AVG/display cadence, any actual display/game-loop
divider relationship, the precise rate of each interrupt-serviced subsystem,
and whether a particular host/driver imposes an unconfigured presentation
limit. No elapsed-time measurement on Neal's PC or arcade cabinet was made.
The current C# fixed difficulty values were not audited parameter-by-parameter
against the ROM's Medium table in this timing task.

No tests or builds were run because this was a bounded read-only trace, not a
code change. The interactive game was not launched.

At completion, tracked files and index remain unchanged. The only intentional
filesystem additions are this report and the paired prompt under the ignored
`AI-Logs/` directory. The pre-existing untracked screenshots remain untouched.
