# Record/Playback Startup-Options Analysis

Date: 2026-11-06

## 1. Starting Git state

- Branch: `master`, tracking `origin/master`.
- `HEAD` and `origin/master`: `8222acee9baf6e893499a7d6ddb859bd1b7e0903`
  (`Add direct level startup and controller quit`).
- Initial tracked worktree and index were clean; there were no unexplained
  nonignored untracked files.
- Remote: `origin` at `https://github.com/nrnoble/Tempest-CSharp.git`.

## 2. Accepted record/playback baseline

The durable accepted/frozen record is
[TEMPEST-DEVELOPMENT-NOTES.md](../../TEMPEST-DEVELOPMENT-NOTES.md#L2478).
It records Neal's manual Level 1 recording/playback acceptance as “100% perfect”
and exactly what he wanted. Preserve the existing per-tick effective-input
capture, seed-based replay, normal production simulation, and live-input
handoff. This analysis recommends only how to carry startup identity through
that mechanism; it does not recommend changing its validated simulation or
input semantics.

The current automated evidence includes exact file-field/input round-trip and
same-seed headless state comparison in
[GameplayRecordingTests.cs](../../tests/Tempest.Game.Tests/GameplayRecordingTests.cs#L36).
The project's four complementary validation layers are unit/integration,
headless gameplay/campaign, selective visible playback, and Neal's normal manual
gameplay acceptance ([TEMPEST-DEVELOPMENT-NOTES.md](../../TEMPEST-DEVELOPMENT-NOTES.md#L42)).
Visible replay remains supplemental; manual play remains the arcade-fidelity
authority.

## 3. Files and evidence examined

- Workspace instructions: `.github/copilot-instructions.md`.
- Durable design/evidence: `TEMPEST-DEVELOPMENT-NOTES.md` (including built-in
  playback history and the accepted/frozen recorded-input section) and
  `TEMPEST-BEHAVIOR-SPEC.md`.
- Recent direct-start evidence:
  [2026-11-06-generic-direct-level-and-enter-quit-report.md](2026-11-06-generic-direct-level-and-enter-quit-report.md)
  and its acceptance closeout.
- Startup/composition: [Program.cs](../../src/Tempest.Game/Program.cs),
  [DebugController.cs](../../src/Tempest.Game/Debug/DebugController.cs),
  [TempestGame.cs](../../src/Tempest.Game/TempestGame.cs),
  [LevelProgression.cs](../../src/Tempest.Game/Levels/LevelProgression.cs),
  [LevelConfiguration.cs](../../src/Tempest.Game/Levels/LevelConfiguration.cs),
  [Level.cs](../../src/Tempest.Game/Levels/Level.cs), and
  [Player.cs](../../src/Tempest.Game/Gameplay/Player.cs).
- Record/replay: [GameplayRecordingFile.cs](../../src/Tempest.Game/Debug/GameplayRecordingFile.cs),
  [PlayerInputRecorder.cs](../../src/Tempest.Game/Systems/PlayerInputRecorder.cs),
  [PlayerInputReplay.cs](../../src/Tempest.Game/Systems/PlayerInputReplay.cs),
  [GameplayPlayback.cs](../../src/Tempest.Game/Debug/GameplayPlayback.cs),
  [InputSystem.cs](../../src/Tempest.Game/Systems/InputSystem.cs), and
  [VectorRenderer.cs](../../src/Tempest.Game/Rendering/VectorRenderer.cs).
- Tests: `GameplayRecordingTests.cs`, `GameplayPlaybackTests.cs`,
  `DebugControllerTests.cs`, `InputDiagnosticTests.cs`,
  `DirectLevelStartupTests.cs`, and `RimSpeedDiagnosticTests.cs`.

## 4–5. Current CLI and normal startup

`Program.cs` parses rim speed, game-over density, file mode, and direct level,
then checks scenario conflicts before initializing Raylib. Ordinary startup
constructs the production Level 1 game with default lives (3), rim-speed
multiplier 1.0, and `Random.Shared`; the renderer's default game-over density
is 2.0. Simulation still advances once per main-loop iteration at the 60 FPS
target, without a catch-up accumulator.

`--level N` is validated against the production progression chain (currently
1–7), resolves the existing `LevelConfiguration`, and passes
`startingProductionLevel` to `TempestGame`. The same fresh-wave initializer is
used as normal play; direct start keeps natural progression enabled. Level-
specific geometry, enemies, Spikes, policy, and initial RNG consumption come
from the selected production configuration, not a second test implementation.
Successful onward progression is therefore deterministic under the same seed
and inputs. See [DebugController.cs](../../src/Tempest.Game/Debug/DebugController.cs#L25),
[Program.cs](../../src/Tempest.Game/Program.cs#L44), and
[TempestGame.cs](../../src/Tempest.Game/TempestGame.cs#L181).

## 6–9. Lives, recording, playback, and randomness

`--lives N` is read by `GetStartingLives` and passed to `TempestGame`; a
positive integer is accepted, otherwise the method silently falls back to the
default of 3. It does not currently reject duplicates explicitly. The game
retains the effective starting value and restores it on a full-game restart;
deaths and progression make the initial count observable and simulation-
relevant.

The current record command is exactly `--record-gameplay <file>`. It generates
a nonnegative seed with `Random.Shared.Next()`, constructs a recorder for
`LevelConfiguration.Level1`, and uses `new Random(seed)` for the game. It
captures the effective input and restart value for every `RunOneStep`, even
when the effective gameplay input is neutral. It writes only on normal window
close. `PlayerInputRecorder` and `GameplayRecordingFile.SaveNew` both reject
starting configurations other than the Level 1 singleton.

The current file playback command is exactly `--playback-file <file>`. Loading
returns a Level 1 replay and its seed; `Program` creates the game with that
seed and today's default startup values. The loaded starting configuration is
not passed into `TempestGame`—this is safe only because the file reader accepts
Level 1 alone. Playback consumes one indexed frame per simulation tick and
resumes live input after the recorded duration.

For recording, the seed is the initial seed, not serialized internal PRNG
state. The game constructs its shared `Random` before `GameOverEffect` and
fresh-level initialization; the same callback then supplies later random
draws. A direct level must be restored before this initialization because its
configuration can change initial entities and random consumption. Same-seed
repeatability is established for the same compatible game implementation,
.NET runtime/random behavior, startup, and per-tick input—not as cross-runtime
or cross-version compatibility. No random model change is recommended.

## 10. Recording format and validation

`GameplayRecordingFile` uses format identifier `tempest-gameplay-input`, version
1. The root fields are `Format`, `Version`, `RandomSeed`, `StartingScenario`
(currently only `level1`), `DurationTicks`, and `Frames`. Each frame stores
`SimulationTick`, `RestartPressed`, and the complete `PlayerInput`: `Fire`,
`MouseDeltaX`, `Left`, `Right`, `ZapPressed`, and `ControllerAxis`.

Loading rejects unsupported identifiers/versions/scenarios, negative seed or
duration, mismatched duration/frame count, null/incomplete entries, non-
contiguous ticks, and non-finite floating input. JSON unknown members are
explicitly rejected (`UnmappedMemberHandling.Disallow`); required root/frame
properties are enforced by deserialization. Save uses a temporary sibling file
and a non-overwriting move. There are no startup-life, rim-speed, state-
snapshot, or RNG-state fields.

## 11–14. CLI composition and option classification

`TryParseGameplayFileCommand` requires the file-mode flag first and exactly one
path argument; recording and file playback are mutually exclusive. Any
additional argument is rejected before startup. The existing tests explicitly
cover rejection of `--playback-file file --lives 10`. `HasConflictingScenarios`
separately makes `--level` exclusive with the built-in profile and legacy
scenario selectors. `--lives`, `--rim-speed`, and `--game-over-density`
otherwise compose with ordinary/debug scenarios.

| Option(s) | Class | Current meaning / composition |
|---|---|---|
| `--level N` | A | Fresh production level 1–7; onward progression stays on. Conflicts with scenario/profile selectors. |
| `--lives N` | A | Starting lives; invalid/missing/nonpositive values silently use 3. Composes with normal/direct starts. |
| `--rim-speed N` | A | Changes production Flipper rim-turn speed through `Level.RimTurnSpeedMultiplier`; it changes simulation, not presentation. |
| `--game-over-density N` | B | Renderer-only procedural Game Over speck count; does not change marks or game simulation. |
| `--record-gameplay <file>`, `--playback-file <file>` | C | Serialized record/playback transport; each is currently standalone. |
| `--gameplay-playback`, `--gameplay-playback-level4` | C | Select built-in seeded input profiles; mutually exclusive with each other, direct level, and legacy scenarios. Level 4 uses a terminal test startup, not natural campaign progression. |
| `--level2-test`, `--level3-test`, `--level4-test`, `--level6-test`, `--level7-test`, `--level3-geometry-test` | D | Development-only terminal/direct configurations or geometry diagnostic. |
| `--spiker-test`, `--tanker-test`, `--fuseball-test`, `--pulsar-test` | D | Isolated development enemy/scenario modes. |
| `--enemy-bullets-test` | D | Development simulation modifier that enables special Flipper firing; it is not ordinary production startup identity and should remain excluded unless a separate recorded-scenario need is approved. |
| `--input-diagnostic` | D | Standalone Raylib input window, not gameplay. It returns before game construction; it has no comprehensive conflict check with ordinary options. |

The classification rule fits: persist effective simulation-affecting values
for supported production record sessions; keep presentation-only settings out
of gameplay identity; and explicitly reject special diagnostic/scenario modes
rather than silently treating them as ordinary recordings. `--enemy-bullets-test`
and the legacy test scenarios are exceptions by deliberate scope, not omitted
production settings.

## 15–16. Why current record mode cannot compose with direct level or lives

The immediate blocker is parser policy, not one isolated schema field:

1. `TryParseGameplayFileCommand` requires the mode at argv index 0 and rejects
   more than two arguments, so `--level` and `--lives` are rejected before
   game startup.
2. Even if that check were relaxed, `Program` hardcodes the recording metadata
   and recorder to Level 1; `PlayerInputRecorder` rejects any other starting
   configuration; and `SaveNew` again rejects non-Level-1 configurations.
3. The version-1 file writes only `StartingScenario = "level1"`. Playback
   validates only that value and reconstructs `LevelConfiguration.Level1`.
4. `--lives` is not present in the file; the current standalone record path
   starts with the default 3. `--rim-speed` is likewise absent and currently
   cannot be passed to file mode.

The concrete anchors are
[DebugController.cs](../../src/Tempest.Game/Debug/DebugController.cs#L156),
[Program.cs](../../src/Tempest.Game/Program.cs#L74),
[PlayerInputRecorder.cs](../../src/Tempest.Game/Systems/PlayerInputRecorder.cs#L15),
and [GameplayRecordingFile.cs](../../src/Tempest.Game/Debug/GameplayRecordingFile.cs#L38).
The direct-start tests confirm that the selected production level produces
fresh configuration state and that campaign progression remains enabled.

## 17–22. Minimum metadata, compatibility, and recommended CLI policy

The minimum supported production startup identity is:

- starting production displayed level/configuration;
- effective starting lives;
- current `--rim-speed` multiplier, because it changes Flipper simulation;
- random seed (already present);
- existing ordered input frames and duration.

The density multiplier is observation-only and need not be recorded. Do not
serialize a live `LevelConfiguration` object; persist a stable displayed-level
identifier and resolve it through the production progression/configuration
catalog.

A small `GameplayStartupConfiguration` is warranted for this finite set of
effective production values (level, lives, rim-speed multiplier, seed), so
normal launch, record initialization, and playback restore use one resolved
value set. Keep the existing `LevelConfiguration` and
`startingProductionLevel` production path authoritative. Do not add a generic
configuration framework, arbitrary debug scenario fields, or speculative
options.

Recommend format version 2 for these new fields. Version 1 rejects unknown
members and supports only `level1`; it has no safe extension mechanism.
Version-2 loading should continue to read v1 as its original semantics:
Level 1, default 3 lives, and rim-speed 1.0. New files should be read by the
new version only; compatibility with old binaries is not required. Do not
rewrite or migrate old files. This is the smallest explicit schema boundary
that preserves accepted v1 recordings and avoids weakening strict validation.

Recommended recording syntax retains the accepted spelling:

```text
--record-gameplay <file> --level 5 --lives 10 [--rim-speed N] [--game-over-density N]
```

Allow recognized options to compose regardless of their ordering after the
file/path pair (or document one order consistently), validate them strictly,
resolve the complete startup configuration once, then initialize the normal
production game and recorder from it. Keep special modes mutually exclusive.

For playback, all simulation-affecting values come from the file. Reject an
explicit `--level`, `--lives`, `--rim-speed`, or special scenario override
before constructing the game—even if the supplied value happens to match.
Do not silently ignore or warn-and-continue. Permit observation-only options
such as `--game-over-density` if desired. Build the production game from the
recorded startup configuration before creating/consuming its seeded RNG.

## 23–24. Smallest implementation boundary and risks

The smallest later implementation is limited to startup resolution and its
record/replay boundary: introduce the small canonical startup value, let the
file parser distinguish transport arguments from allowed startup/presentation
options, serialize the production values under version 2, add a strict v1
compatibility loader, and use the exact same production `TempestGame`
initialization path for recording and playback. Do not change fixed-step
simulation, captured frame semantics, RNG implementation, or enemy behavior.

Main risks are: applying the recorded seed after construction (wrong initial
random consumption); restoring the displayed level without the normal
progression-enabled direct-start path; losing the effective lives reset value;
omitting rim speed; silently honoring playback-time overrides; weakening
unknown-field validation; or changing an accepted input/transition boundary.

## 25–26. Later implementation validation

Required automated coverage:

- v2 round-trip of level, lives, rim speed, seed, duration, and every existing
  input/restart field;
- v1 fixture reads with Level 1/default lives/default speed and unchanged
  replay behavior;
- strict rejection of unsupported versions, unknown members, invalid levels,
  invalid lives/speed, malformed metadata, and conflicting CLI modes;
- command parsing for recording options in supported argument orders, including
  `--level`/`--lives`, and rejection of simulation overrides during playback;
- same-seed headless state equivalence for Level 1 and direct production starts,
  including deaths/restarts and natural onward progression;
- prove density changes presentation only and does not change deterministic
  game state; retain existing round-trip tests unchanged.

Required Neal manual acceptance after that implementation:

- record and separately replay a normal Level 1 session, confirming the
  existing accepted “100% perfect” result remains intact;
- record a fresh Level 5 (`--lives 10`) session, replay it visibly, and compare
  starting state, movement/fire/zap timing, deaths, and any Level 5-to-6
  progression that occurs;
- repeat at a later implemented level when appropriate;
- verify playback rejects `--level`, `--lives`, `--rim-speed`, and special
  scenario overrides before opening gameplay, while an observation-only
  density option does not change the simulation;
- verify normal live controls and input handoff at recording boundaries.

Automated checks supplement, not replace, manual gameplay acceptance.

## 27–28. Deferred playback/observation capabilities

| Capability | Architectural fit and identity |
|---|---|
| Speed | Straightforward as render/pacing control: keep each `RunOneStep` a fixed simulation tick and process more recorded ticks per rendered frame when speeding up. Never change tick duration. No startup-format field. |
| Pause | Natural loop-level gate; no simulation identity change or format field. |
| Single-step | Natural because each recorded input is indexed by simulation tick and one `RunOneStep` advances exactly one update. No format field. |
| Automatic loop | Recreate the game, RNG from seed, startup configuration, tick counter, and recording cursor at each loop. Do not rely on ordinary `ResetGame`: a direct production run's restart policy can reset to campaign Level 1, and mutable RNG state must also be reset. No format field for simple loop. |
| End frame | A playback stop/trimming boundary; no required format change if selected at playback time. |
| Start frame | Observation trimming can replay from tick 0 and suppress display until the target. True arbitrary seeking needs snapshots or deterministic fast-forward from tick 0; indexed input lookup alone does not restore game/RNG state. Snapshots require a separately designed versioned format. |
| Overlays | Read-only renderer diagnostics can sample current level/tick/entity state without changing the simulation. RNG state is private and not currently exposed. No format change for live overlays. |
| Deterministic verification | Tests already compare selected state fields. Periodic/final signatures could capture game state, score/lives/level, entities and RNG-related evidence, but no canonical signature or RNG state accessor exists. Persisted expected signatures require versioned format additions; avoid hashing incidental presentation state. |
| Headless verification | Strong fit: `TempestGame.RunOneStep` is independent of Raylib and existing tests feed recorded frames directly. A future runner can use the same startup resolver and replay input without rendering/audio. This strengthens the headless layer; visible replay and normal manual acceptance remain separate layers. |

Suggested order: first startup metadata/version compatibility and replay
regression; then headless verification/state-signature design if it answers a
concrete regression need; then pause/single-step and pacing controls; then
loop/end-frame conveniences; only then consider start-frame fast-forward or
snapshots and diagnostic overlays. Do not bundle those deferred features into
the startup-metadata change.

## 29. Higher-level testing workflow implications

The proposed workflow is supported by current production direct starts and
fixed-tick recorded input. The recording must capture the effective starting
configuration and seed at the point it is chosen, not infer them from current
defaults during playback. Repeated visible replay lets Neal observe the same
difficult gameplay without playing simultaneously; future headless replay can
reuse it for regression. Neither replaces manual control/feel/fidelity
acceptance. This addresses higher-level testing cost without broad replay
redesign.

## 30–32. Files, validation, and final scope

Files written:

- `AI-Logs/Prompts/2026-11-06-record-playback-startup-options-analysis-prompt.md`
  — exact supplied task copied from the read-only attachment; SHA-256 verified
  against the source (`9E08E09B872FF5B093BE98BCA1E6FA9D8A1F6DEBDC9D04E57D48ED3CFACBDECE`).
- `AI-Logs/Reports/2026-11-06-record-playback-startup-options-analysis-report.md`
  — this analysis.

No build or tests were run: this was read-only analysis, no production or test
files changed, and the task explicitly says automated validation is not
required absent a concrete architecture question. No interactive game was
launched.

Final Git state was checked against the requested commands: `master` remains at
`8222acee9baf6e893499a7d6ddb859bd1b7e0903`, equal to `origin/master`; tracked
worktree and index are clean and there are no nonignored untracked files. Only
the two authorized, ignored AI records were written. No production code, tests,
durable documentation, recording/replay data, Git history, or GitHub state was
modified. No staging, commit, or push occurred.

Stop for Neal's review before any recording/playback modification.
