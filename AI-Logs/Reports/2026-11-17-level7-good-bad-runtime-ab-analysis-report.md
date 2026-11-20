# Level 7 good/bad visible-runtime A/B analysis

Date: 2026-11-17
Status: **READ-ONLY ANALYSIS COMPLETE; VISUAL REGRESSION REMAINS UNEXPLAINED AT THE MEASURED RENDERER STAGE**

## Executive result

The captures are distinguishable, complete `schemaVersion: 1` JSON-lines
diagnostics: the control is recorded playback and the manual capture is
normal manual gameplay. Neal's classifications remain authoritative:
**control visibly good; manual visibly bad**.

The bad/manual capture does **not** contain the reported progressive Tanker
slowdown or the inverse player-shot trend in any measured motion stage, through
the camera-transformed, submitted VectorRenderer geometry bounds. Tanker
height/tick, mapped-anchor/tick, final-anchor/tick, and submitted-extrema/tick
are effectively flat across depth in all qualifying trajectories. Player shots
are likewise flat near-to-deep. The measured difference that remains is
temporal jitter in the manual run and the fact that its live Blaster moves
during intervals when Tankers advance slowly in absolute screen pixels per
second; neither varies with Tanker depth in a way that explains the reported
progression.

This numeric result does not negate Neal's direct visual observation. The
earliest missing observable is rasterized framebuffer output; after that,
actual buffer presentation, operating-system composition, refresh, and
monitor scanout are also unobserved. No correction is justified by these logs.

## FACT FROM THE LOGS

### Run identity, metadata, and completeness

The task specified `.jsonl` names, while the actual worktree files have `.json`
extensions. They contain one JSON record per line. Their embedded
`outputPath` metadata still names the `.jsonl` variants; analysis used the
actual files below, not the metadata path or filenames to infer run identity.

| Property | Good control | Bad manual |
|---|---|---|
| Actual file | [level7-control-20261117.json](../../TEMP/Diagnostics/Level7VisibleRuntime/level7-control-20261117.json) | [level7-manual-20261117.json](../../TEMP/Diagnostics/Level7VisibleRuntime/level7-manual-20261117.json) |
| Size | 51,302,544 bytes | 37,017,758 bytes |
| SHA-256 | `8282d502a6bf02899a4460548a3df55fe4d22d847bc61eb7765ea243a9b645a9` | `e867c593a463da480b5edc151b8615f3b0e7002f3086f0387618be67b82347c5` |
| Start time | `2026-11-17T10:36:05.2761539+00:00` | `2026-11-17T10:18:46.7813881+00:00` |
| Runtime mode | `recordedPlayback` | `manualGameplay` |
| Presentation mode / target | `ExperimentalPortrait` / 60 FPS | `ExperimentalPortrait` / 60 FPS |
| Playback source in metadata | `TEMP\GameplayRecordings\Level-7-slowmo.json` | none |
| Level 7 Playing frames | 3,019 | 1,775 |
| Valid JSON-lines records | 56,774 | 40,552 |
| Final marker | `runEnded`, frame count 3,019 | `runEnded`, frame count 1,775 |

Both files parsed without errors, ended in a newline, and contain no
`incompleteFrame` or `diagnosticLimitReached` marker. Their `runEnded` counts
match their recorded Playing-frame counts. Each has a `levelScopeEnded` marker
after transition to Level 8 Approach.

The control's metadata identifies `Level-7-slowmo.json`, not
`level7-candidate-6.json` named in the immediately prior instrumentation
report. The recording was not inspected. This is a relevant input/source
identity difference, not evidence that the visually good classification is
wrong.

### Tankers: qualifying trajectories and depth bands

Depth bands use recorded `normalizedDepth`: deep `>= 0.8`, middle `> 0.2` and
`< 0.8`, near-rim `<= 0.2`. A qualifying trajectory is a continuous,
fixed-lane, unchanged-state segment with positive tick progression, at least
0.6 normalized-depth span, and observations in all three bands. All
qualifying trajectories in each capture were examined:

- Control: 24 continuous Tanker segments; **16 qualifying** full-depth climbs.
- Manual: 16 continuous Tanker segments; **6 qualifying** full-depth climbs.

The per-run values below are medians of each qualifying trajectory's
depth-band median, so differing lane populations do not masquerade as
depth-dependent behavior. Host pixels are the diagnostic camera-transformed
window-pixel coordinates.

| Run | Depth | Height change / tick | Host-anchor px / tick | Host-anchor px / second | Submitted bounds-extrema px / tick |
|---|---|---:|---:|---:|---:|
| Control (16 tracks) | Deep | +1.75 | 0.928 | 55.66 | 0.910 |
|  | Middle | +1.75 | 0.928 | 55.66 | 0.910 |
|  | Near rim | +1.75 | 0.928 | 55.67 | 0.910 |
| Manual (6 tracks) | Deep | +1.75 | 0.707 | 42.30 | 0.704 |
|  | Middle | +1.75 | 0.707 | 42.40 | 0.704 |
|  | Near rim | +1.75 | 0.707 | 42.39 | 0.704 |

Within-trajectory near/deep ratios reinforce the flat trend:

| Run | Measure | Median ratio | Range across qualifying tracks |
|---|---|---:|---:|
| Control Tankers (n=16) | Host px/tick | 1.000003 | 0.999980–1.000015 |
|  | Host px/second | 0.999988 | 0.999742–1.000200 |
| Manual Tankers (n=6) | Host px/tick | 1.000013 | 1.000000–1.000025 |
|  | Host px/second | 1.002001 | 0.997745–1.010168 |

Representative long climbs:

- Control Tanker `299`, lane 13: 215 observations, depth `0.975` to `0.03875`,
  height 10 to 384.5, ticks 2341–2555. Its host-anchor rate is 1.38724
  px/tick in deep, middle, and near bands (about 83.23 px/s); submitted
  bounds-extrema movement is 1.41976 px/tick in all three.
- Manual Tanker `370`, lane 2: 215 observations, depth `0.975` to `0.03875`,
  height 10 to 384.5, ticks 1443–1657. Its host-anchor rate is 0.70313
  px/tick across all bands (about 42.1 px/s); submitted bounds-extrema
  movement is 0.7001 px/tick throughout.
- A second manual climb, Tanker `48`, lane 3, spans depth `0.970625` to
  `0.03875` over 214 observations. It also remains flat at 0.58906 host
  px/tick across depth.

Raw rates differ by lane, as expected from the lane-specific record-06 rail
lengths; they must not be interpreted as a playback/manual speed difference.
For example, the qualifying lane-2 Tankers are 0.70313 host px/tick in both
runs, and lane-14 Tankers are 1.05840 px/tick in both. Within each shared
lane there is no measurable A/B rate divergence.

Scale and size do change strongly with depth, but not the measured movement
rate: representative Tanker local presentation scale grows from about 0.25
deep to 0.90 near-rim, and its submitted host-bounds diagonal grows from about
27 to 87 pixels. The bounds-extrema displacement per tick nevertheless stays
constant within each trajectory. Thus geometry size is not concealing the
reported progressive anchor/extrema slowdown at this stage.

### Player shots: inverse probe

The same qualification rule was applied in the opposite direction, from
near-rim toward deep:

- Control: 282 continuous shot segments; **138 qualifying** full-depth shots.
- Manual: 316 continuous shot segments; **157 qualifying** full-depth shots.

| Run | Depth | Height change / tick | Host-anchor px / tick | Host-anchor px / second | Submitted bounds-extrema px / tick |
|---|---|---:|---:|---:|---:|
| Control (n=138) | Near rim | -6 | 3.828 | 229.64 | 3.138 |
|  | Middle | -6 | 3.828 | 229.67 | 3.138 |
|  | Deep | -6 | 3.828 | 229.68 | 3.138 |
| Manual (n=157) | Near rim | -6 | 3.165 | 189.81 | 2.753 |
|  | Middle | -6 | 3.165 | 190.14 | 2.753 |
|  | Deep | -6 | 3.165 | 189.07 | 2.753 |

Across tracks, median deep/near host-pixel-per-tick ratios are 1.000002
(control) and 1.000002 (manual); corresponding host-pixels-per-second ratios
are 0.999973 and 0.999748. A representative control shot (`70`, lane 10) and
manual shot (`41`, lane 9) each span normalized depth `0.015` to `0.99` in 66
observations and show essentially constant mapped, submitted-anchor, and
bounds-extrema movement per tick in all three bands.

Shot geometry shrinks toward the deep end (representative local scale about
0.90 near-rim to 0.24 deep; host-bounds diagonal about 12 to 3 pixels), but
submitted bounds-extrema movement per tick is flat. The inverse tendency
Neal reports is not present in the logged motion through this renderer stage.
As with Tankers, absolute cross-run rates differ with lane composition; the
within-shot depth ratios are the relevant result.

### Blaster and simultaneous runtime intervals

The manual Blaster has 1,773 adjacent, valid Playing observations; its
camera-transformed anchor changes on 1,177 intervals (66.4%). On the exact
adjacent frame intervals represented by qualifying Tanker edges, median
manual Tanker anchor rates remain about 42.7 px/s across depth, while the
paired Blaster anchor rates are nonzero (deep about 863, middle about 630,
near about 229 px/s). This establishes measurable contemporaneous Blaster
movement while Tanker anchors advance much less in absolute host pixels per
second in this capture.

The Blaster rate is input-driven and discrete, not a calibrated continuous
arc-length speed. A frame interval may be counted once per visible Tanker,
and different depth bands occur at different moments in the run; therefore
the paired figures do not establish a causal or depth-correlated ratio. In
the control, the Blaster moves on 716 of 3,016 adjacent observations (23.7%),
and its median motion on Tanker-paired intervals is zero. Different recorded
and live inputs make that cross-run comparison non-equivalent. The Blaster
data supports a within-manual real-time reference, not a controlled
good-versus-bad input comparison.

### Tick, frame, elapsed-time, and mode findings

| Measure | Control | Manual |
|---|---:|---:|
| Tick delta for each logged Playing frame interval | Always 1; 0 repeats, 0 skips | Always 1; 0 repeats, 0 skips |
| Median time between presented-call returns | 16.667 ms | 16.668 ms |
| 10th–90th percentile return interval | 16.64–16.70 ms | 15.83–17.50 ms |
| Median render-call duration | 16.435 ms | 16.409 ms |
| Render calls >16.7 ms | 135 / 3,019 (4.5%) | 477 / 1,775 (26.9%) |
| Return intervals >50 ms | 1 | 1 |
| Playback-boundary delta | Always 1; no repeat/jump | Not applicable |
| Paused logged frames | 0 | 0 |

The largest control return interval is 321.9 ms at frame 1779/tick 1780; the
largest manual interval is 307.2 ms at frame 1/tick 2. Each run also has one
render call over 290 ms. Manual timing is noisier and can account for
occasional whole-frame stutter, but it does not produce the steadily
depth-dependent rates that are absent from the per-trajectory measurements.
The manual outlier is at the start; the control has one mid-run outlier.

Logged render-sequence gaps of 61 occur twice in control and once in manual.
Each follows a `Playing` to `PlayerDying` transition. The diagnostic records
entity geometry continuously only in Level 7 Playing and emits sparse
transition markers elsewhere; these are excluded state intervals, not
evidence of skipped Playing simulation ticks. The selected Tanker and shot
trajectories contain no gap, pause, state, or lane contamination.

The logs show different input sources and playback identity, but the
read-only code path does not switch Level 7 entity presentation based on
runtime mode. `Program.cs` sets the same 60-FPS target. The playback
controller advances one `GameplayPlaybackSession.StepForward`, which calls
the same `Game.RunOneStep` used by the manual path; both then call the same
renderer. The Level 7 presentation-candidate gate depends on Atari
presentation being enabled, Level 7, and `Playing`, not on playback/manual
mode. Relevant code: `src/Tempest.Game/Program.cs` (target FPS, main loop,
diagnostic mode, and renderer call), `src/Tempest.Game/Debug/VisiblePlaybackController.cs`,
`src/Tempest.Game/Debug/GameplayPlaybackSession.cs`, and
`src/Tempest.Game/Rendering/VectorRenderer.cs` (Level 7 gate and draw calls).

Classification of observed differences:

- **Potentially causal to input/event composition, but not established as
  causal to the visual regression:** different playback recording versus
  live input; the control source identity differs from the prior report's
  proposed candidate.
- **Potentially causal to intermittent global stutter, probably unrelated to
  progressive depth motion:** broader manual frame-time jitter and the one
  large start-of-run interval.
- **Probably unrelated to depth-specific motion:** per-frame update count,
  Level 7 entity mapping/render path, presentation mode, target frame rate,
  playback-boundary progression, and same-lane host rates are consistent.
- **Insufficient evidence:** what happens to rasterized pixels and actual
  physical display presentation after the latest instrumented geometry.

## MANUAL OBSERVATION

As supplied for this task, Neal watched the recorded-playback control and did
not see the regression. He manually played the bad Level 7 run and clearly
saw the depth-motion regression. He intentionally let most Tankers approach
all or nearly all the way from deep to rim. These observations are retained
without qualification; the data/observation conflict is unresolved.

## INTERPRETATION AND MEASURED BOUNDARY

Simulation height progression does not show depth-dependent slowdown:
Tankers advance +1.75 height units/tick and shots move -6/tick. Mapped-anchor
displacement, camera-transformed anchor displacement, and submitted
axis-aligned bounds-extrema movement are also effectively constant within
fixed-lane trajectories. Shared-lane A/B Tanker rates match. The manual run's
greater render-time jitter is real but is not correlated with normalized
depth in the qualifying tracks.

The latest measured stage is the geometry coordinates supplied to Raylib
drawing operations in `VectorRenderer`, plus diagnostic camera transforms
and bounds, with a monotonic timestamp immediately after `EndDrawing`
returns. This is not framebuffer readback. The logs do **not** observe GPU/
CPU rasterization results, the actual back-buffer pixels, buffer-swap and
window-system composition, physical monitor refresh, scanout, or emitted
display pixels. Therefore the BAD run is numerically normal through the
latest instrumented renderer stage; no earlier measured motion-stage
divergence explains Neal's observation.

## UNRESOLVED / NARROWEST NEXT STEP

Keep the visual observation and numeric result in conflict. The smallest next
boundary is a limited, development-only readback of the **actual rasterized
framebuffer** for a few consecutive frames while a Tanker traverses deep,
middle, and near-rim depths, synchronized to the existing entity/depth
diagnostics. If the framebuffer movement is normal too, only then investigate
the post-`EndDrawing` swap/compositor/monitor-presentation path. Do not change
the motion model, mapping, speed, tube, or gameplay based on this analysis.
No new capture or game launch was performed here.

## Validation and preservation

- **PASS:** both complete JSON-lines files were streamed with bounded
  processing (100 MB/file, 1,000,000 records, 60,100 frames, 120-second
  analysis bound); malformed-line count was zero.
- **PASS:** the SHA-256 values above matched before, during, and after the
  analysis passes; file sizes and modification times remained unchanged.
- **PASS:** no game was launched; no new diagnostic run was created; no
  screenshot, video, frame, contact sheet, recording, or `TEMP/myNotes.md`
  was inspected.
- **PASS:** no production, test, configuration, instrumentation, recording,
  diagnostic, or durable project documentation file was modified. Only this
  report and its paired ignored prompt were created.
- **PASS:** Git remained on `master` at
  `385f603a94391756b2ca6cccd524dc17b281e782`, tracking `origin/master`, with
  an empty index. The 17 pre-existing modified tracked paths and 19
  pre-existing nonignored untracked paths were preserved; AI-Logs was not
  staged.
- No code validation or new manual acceptance was run because this was a
  read-only analysis and the task explicitly prohibited launching the game.

Completion token: `TEMPEST_LEVEL7_GOOD_BAD_RUNTIME_AB_ANALYSIS`
