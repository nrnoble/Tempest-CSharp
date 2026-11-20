# Development diagnostic `--speed` implementation

Implement a tightly bounded development/diagnostic option:

```text
--speed <multiplier>
```

The purpose is to let Neal manually play Tempest at slower or faster overall
wall-clock pacing while preserving the exact existing per-simulation-tick
gameplay rules. Work in `C:\Users\nrnob\source\repos\Tempest`.

The accepted baseline is commit
`e3a3d4cf31449bc710c43d9d912ca5954605ade8` (`Close out accepted ROM-derived
Blaster-shot rendering`). Confirm the actual Git state before modifying
anything. Reuse
`AI-Logs/Reports/2026-11-08-simulation-timing-speed-rom-investigation-report.md`;
do not repeat its timing/ROM investigation unless implementation requires
rechecking a detail.

## Intent and required semantics

The accepted normal game speed is not being changed. Neal considers normal
timing solid and possibly effectively identical to the arcade for practical
purposes. `--speed` is only a development/manual-testing tool for experimenting
with subtle global pacing while actively controlling the Blaster, including
on future higher levels. Do not treat it as a gameplay-balance correction.
Default normal play must remain equivalent to today's accepted behavior.

Support positive finite invariant-culture numeric multipliers from 0.25 through
4.0 inclusive. Reject out-of-range values; do not clamp. Expected examples:

- 1 or 1.0: accepted normal speed;
- 0.5: half normal wall-clock speed;
- 2: twice normal wall-clock speed;
- 0.9 and 0.95: approximately those proportions of normal.

Reject zero, negatives, NaN, both infinities, malformed values, missing values
and duplicate `--speed` options. Use clear command-line errors. Follow existing
duplicate-option policy.

## Pacing model and simulation invariants

The current interactive host requests `Raylib.SetTargetFPS(60)` and ordinarily
does one `RunOneStep` plus one render per host-loop iteration. Implement the
smallest recommended architecture: target FPS is the existing 60-Hz target
multiplied by `--speed`. Thus 0.5 requests 30 FPS, 2 requests 120 FPS, 0.9
requests 54 FPS, and 0.95 requests 57 FPS. Define and document a deterministic
integer rounding rule for Raylib. The no-option case must retain the existing
normal target of exactly 60.

Do not modify, multiply, or otherwise scale any per-tick simulation value:
Blaster movement, spinner sensitivity, player shots/cooldowns/capacity, enemy
or Flipper movement, rim pursuit, Tankers, Spikers, firing, bullets, Eggs,
timers, collisions, death/exit/approach timing, destruction effects, Game
Over, Super Zapper, RNG, level configuration, supply, scoring, or any other
gameplay constant. At half speed all tick-driven behavior takes about twice
the wall time; at twice speed it takes about half, assuming the host can meet
the request. Do not separately scale Blaster movement.

Keep the implementation small: expose a reusable command-line/runtime option
value for the parsed multiplier, validated range and derived target FPS. Keep
one normal target-rate constant and one conversion; do not scatter `60 *
speed` calculations or add a timing framework.

## Host loop, input, and rendering

Rendering remains coupled to the ordinary host loop and requested speed. Do
not add independent rendering, interpolation, a monotonic-clock accumulator,
skipped draws, multiple simulation updates per draw, catch-up logic, or a
separate input thread. Live mouse/spinner/controller input is sampled less
often at slower rates and more often at faster rates; this limitation is
understood. Document that the requested multiplier is not a hard real-time
guarantee and actual pace may be lower if the PC/render/display path cannot
sustain the target. Do not compensate with catch-up simulation.

Visible playback must use the selected interactive host rate, while preserving
tick-boundary state. Pause, single-step, grouped stepping, reverse
reconstruction and screenshot boundary identity remain simulation-boundary
operations and must not change with speed.

Headless playback remains as fast as practical and unthrottled. For
`--speed` combined with headless playback, either explicitly document and test
ignoring it or reject it clearly; prefer no headless throttling.

## Recording, audio, and classification

Do not add speed or wall-clock timing to gameplay recordings. Preserve file
schema/version, startup simulation identity, RNG seed semantics and effective
per-tick inputs; existing recordings remain readable. Recordings made with
`--speed` still contain ordinary per-tick inputs only.

Do not change audio sample playback rate or pitch. Slower/faster gameplay
changes the real-time spacing of gameplay-triggered sound events only.
Exercise existing audio regressions.

`--speed` is a **DEVELOPMENT / DIAGNOSTIC PROJECT FEATURE**, not ROM-backed
behavior, not a difficulty setting, and not evidence that accepted normal
speed was wrong. Medium/Easy/Hard and DIP switches are separate; do not change
fixed difficulty-derived gameplay values.

## Required automated tests

Add focused tests for:

1. Omitted option: multiplier 1.0 and target FPS exactly 60.
2. Explicit `--speed 1`: target 60 and runtime option state equivalent to the
   default.
3. Fractional values: 0.5 -> 30, 0.9 -> 54 and 0.95 -> 57.
4. Faster values: 2 -> 120 and 4 -> 240.
5. Inclusive bounds 0.25 and 4.0; reject below/above.
6. Invalid values, missing values and duplicate behavior.
7. Deterministic simulation: same initial state, seed and per-tick inputs
   produce equal state after N `RunOneStep` calls, independent of speed; no
   simulation constant is scaled.
8. Recording schema remains unchanged and existing files remain readable.
9. Visible playback changes presentation pacing only; equal tick boundaries
   have equal state.
10. Pause/forward/grouped-step/reverse behavior remains unchanged.
11. Headless playback remains unthrottled and its speed-option policy is
    explicitly tested.
12. Existing gameplay regressions remain unchanged across movement, firing,
    collision, enemy updates, transitions, effects and campaign behavior.

Do not add flaky `Thread.Sleep` or exact-millisecond tests. Test parsing, FPS
conversion, runtime policy and deterministic boundaries instead.

## Validation and documentation

Run the narrow new option/parser tests first, then focused regressions covering
command-line/startup, input, simulation, movement, firing, collision, audio,
recording/playback, visible controls/reverse scrubbing, headless playback,
screenshots and campaign soak/progression. Then run:

```text
dotnet test TempestCSharp.slnx
dotnet build TempestCSharp.slnx
git diff --check
```

Update `TEMPEST-DEVELOPMENT-NOTES.md`, `TEMPEST-BEHAVIOR-SPEC.md`, and README
usage documentation as appropriate. Document the development-only status,
default 1.0 behavior, range, host pacing only, unchanged per-tick values,
headless policy, audio limitation, host-performance limitation and pending
manual acceptance. Do not imply Atari functionality.

Do not launch the interactive game. Inspect the final diff. Do not stage,
commit or push. Preserve unrelated files, recordings, screenshots, references
and user data. Leave the candidate unstaged for Neal's acceptance.

## Manual acceptance handoff

Provide these practical examples:

```powershell
dotnet run --project src\Tempest.Game -- --speed 1 --lives 10
dotnet run --project src\Tempest.Game -- --speed 0.95 --lives 10
dotnet run --project src\Tempest.Game -- --speed 0.9 --lives 10
dotnet run --project src\Tempest.Game -- --speed 0.5 --lives 10
dotnet run --project src\Tempest.Game -- --speed 2 --lives 10
dotnet run --project src\Tempest.Game -- --level 7 --speed 0.95 --lives 10
dotnet run --project src\Tempest.Game -- --level 7 --speed 2 --lives 10
```

Ask Neal to verify normal `--speed 1` equivalence; global slowdown/speedup of
Blaster, enemies, shots, transitions and effects; useful 0.9/0.95 comparisons;
spinner/mouse usability; normal Fire/Super Zapper timing relative to
simulation; unchanged audio pitch; equivalent visible recording states at
equal boundaries; unchanged pause/step/reverse/screenshots; unthrottled
headless playback; and no normal-speed gameplay regression. Manual acceptance
must evaluate the diagnostic as a development tool, not use it to conclude
that normal speed should change.

The implementation report must include the starting/final Git state, reused
investigation report, parser/range, FPS rounding/default, headless policy,
recording/playback behavior, files/tests changed, focused and expanded test
results, build/diff-check results, manual commands/checklist and unresolved
limitations. Stop after automated validation and handoff; do not retune 1.0 or
gameplay mechanics.

TEMPEST_SPEED_MULTIPLIER_DIAGNOSTIC_IMPLEMENTATION_20261108
