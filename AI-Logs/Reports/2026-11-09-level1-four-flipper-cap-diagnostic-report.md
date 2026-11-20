# Level 1 Four-Flipper Cap Diagnostic

Date: 2026-11-09
Disposition: **AUTOMATED VALIDATION PASSED — MANUAL A/B ACCEPTANCE PENDING**

## Starting Git state

- Branch: `master`, tracking `origin/master`.
- Starting `HEAD`: `c1a974a625eea2dfaecf30d1b8d729e022ef17e8`
  (`Close out accepted spinner sensitivity diagnostic`), zero ahead/behind.
- Index and tracked worktree were clean before implementation.
- The only initial untracked repository path was the protected
  `Reference/arcade/roms.rev2/`; it remains untouched.

## Investigation reused and hypothesis isolated

Reused the findings in
[`2026-11-09-low-level-difficulty-comparative-investigation-report.md`](./2026-11-09-low-level-difficulty-comparative-investigation-report.md).
No broad difficulty investigation was repeated. That report ranked excess
simultaneous enemies, enabled by the accepted C# Egg schedule and absence of
the Level 1 ROM Flipper cap, as the strongest lower-level pressure hypothesis.
This implementation isolates only the Level 1 maximum of four concurrent
Flippers. The cap value is **ROM-LISTING-BACKED /
REV-2-BINARY-CONFIRMED**. Waiting C# Eggs are a **PROJECT TRANSLATION**, not a
literal ROM pending-entry mechanism.

Neal's report that C# feels harder than his original machine is **ARCADE
OBSERVED**. The cabinet setting is **CABINET SETTING BELIEVED MEDIUM /
UNVERIFIED**. No arcade comparison was performed in this implementation.

## Diagnostic and production policy

- Added `--level1-four-flipper-cap-test`, an explicit, unique,
  development-only startup using the accepted `LevelConfiguration.Level1`.
- Startup retains displayed Level 1, ten obligations, four starting Eggs,
  six reserve units, player start, geometry, normal scoring, shots, firing,
  Super Zapper, death/re-entry, and accepted 2.5 rim calibration.
- The scenario is terminal and replayable at its own boundary; R restarts it
  rather than progressing to Level 2.
- Production Level 1 remains uncapped. No-option startup, `--level 1`,
  campaign progression, Level 2-8, and production configuration data are
  unchanged.
- The only experimental gameplay difference is hatch eligibility at four
  active Level 1 Flippers. The separate ROM six-total-active gate is not
  implemented.
- `--lives`, `--speed`, and `--spinner-sensitivity` compose. `--rim-speed`,
  `--enemy-bullets-test`, input-diagnostic, other scenario selectors, and
  duplicate cap options are rejected.

## Cap, Egg waiting, and accounting

The cap uses `TempestGame`'s active `flippers` collection; no parallel counter
is maintained. A Flipper starts counting when the normal hatch path adds it to
that collection. It stops counting when the existing lifecycle removes it
after destruction or zap resolution, life reconstruction, or rim clear.
Flippers with pending zap timers remain active until normal removal.

When four Flippers are present, `HatchEggs` leaves ready Eggs in the existing
`ReadyToHatch` state and does not invoke a hatch factory. The Egg remains in
the Level collection and unresolved; it is not killed, rim-cleared, returned
to reserve, retargeted, or rerolled. The normal hatch path resumes as soon as
a slot is available. The blocked branch introduces no random call.

Level 1 release timing, four initial Eggs, 50-tick reserve and approach
cadences, maximum approaching-Egg policy, movement cadence, lane/direction
sequences, and total supply are unchanged. The cap does not add a release or
queue-room gate. Every tick in the automated cap scenarios checks conservation:

`reserve + Eggs + active Flippers + destroyed + rim-cleared = 10`

The Level 1 model has no separate original-slot identity objects; conservation
is verified through the existing supply/outcome accounting without introducing
a parallel identity system.

## Gameplay lifecycle and completion

- Death reconstruction retains the diagnostic flag and uses existing
  unresolved-Egg reconstruction. Repeated deaths preserve the ten-unit
  accounting and do not exceed the cap.
- Super Zapper still targets active enemies only. A resolved zap frees a slot;
  a waiting Egg then hatches through the normal conversion path. Zap charge and
  delayed-resolution behavior are unchanged.
- A deterministic fire/movement/zap test completes the diagnostic by resolving
  all ten obligations. No ready-Egg deadlock or false early completion was
  observed.
- Flipper firing remains enabled as in normal production Level 1 despite the
  diagnostic's terminal progression policy. Enemy-shot capacity remains two;
  player shots, movement, collision ordering, scoring, and rim values are
  unchanged.

## Recording and playback policy

The current recording startup identity cannot distinguish this development
scenario from production Level 1. Recording, visible playback, and headless
playback therefore reject the diagnostic. No schema version was changed;
historic recordings retain their existing behavior. No persistent telemetry
or HUD was added; comparison tests inspect existing game state.

## Files changed

- `src/Tempest.Game/TempestGame.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Debug/DebugController.cs`
- `src/Tempest.Game/Debug/HeadlessGameplayPlaybackCli.cs`
- `tests/Tempest.Game.Tests/Level1FourFlipperCapDiagnosticTests.cs`
- `tests/Tempest.Game.Tests/DirectLevelStartupTests.cs`
- `tests/Tempest.Game.Tests/InputDiagnosticTests.cs`
- `README.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `TEMPEST-BEHAVIOR-SPEC.md`
- Ignored prompt archive:
  `AI-Logs/Prompts/2026-11-09-level1-four-flipper-cap-diagnostic-prompt.md`
- This ignored report under `AI-Logs/Reports/`.

The prompt archive is byte-identical to the supplied attachment (SHA-256
`D2B710C62E66BC694173DC4FCF01EC995990AE1E1FE21371EA6C97F8F516C8F6`) and
contains exactly one complete `TEMPEST_` token line.

## Tests and deterministic comparison

Added coverage for production-default startup, diagnostic configuration and
terminal restart, baseline-versus-cap behavior, active-count and supply
invariants, ready-Egg persistence, release schedule isolation, deterministic
replay of fixed-seed inputs, death/re-entry, zap resolution, wave completion,
Levels 2-8 isolation, and CLI composition/conflicts.

With seed `1981` and no player input, the production `--level 1` model first
reached five active Flippers at simulation boundary **289**. A repeated
baseline run reached that boundary identically. The diagnostic remained at
four while a fifth ready Egg waited. At boundaries 0, 100, and 200, baseline
and diagnostic active/rim counts, reserve, and Egg states matched. Repeated
diagnostic runs also matched Egg and Flipper states; the cap path preserved
normal Level 1 enemy firing and the two-shot capacity. These are **AUTOMATED
TEST EVIDENCE**, not an arcade test or a difficulty/survival conclusion.

## Validation

- Focused CLI, startup, cap, death/re-entry, Super Zapper, and completion tests:
  **30 passed**.
- Full suite: `dotnet test TempestCSharp.slnx` — **1,107 passed, 0 failed**.
- Full build: `dotnet build TempestCSharp.slnx` — **succeeded, 0 warnings,
  0 errors**.
- `git diff --check` — passed. Git emitted only its existing line-ending
  normalization warning for `README.md`.
- Level 2-8 regression coverage passed in the full suite; direct starts at
  Levels 2-8 remain uncapped.
- The interactive game was not launched. No Windows Application Control
  block occurred.

## Protected local data and final Git state

No writes were made to `Reference/arcade/roms.rev2/`, `UserData/`, screenshots,
recordings, `Videos/`, or historical AI reports. The initial untracked ROM
directory remains. No files under `TEMP/` were changed.

Process note: the tagged `TEMP/myNotes.md` was opened during the initial
workspace-context read before the task's TEMP exclusion was visible. It was
not used as evidence; no further TEMP files were read.

At closeout, `HEAD` remains
`c1a974a625eea2dfaecf30d1b8d729e022ef17e8` on `master` tracking
`origin/master`. Changes remain unstaged; nothing was committed or pushed.
The new test file and the original untracked Rev 2 directory are the only
untracked repository paths. AI logs are ignored by Git.

## Neal's manual A/B comparison

Run both with the same physical spinner, sensitivity `1.0`, lives, movement
and firing strategy, Super Zapper strategy, and display/window setup:

```powershell
dotnet run --project src\Tempest.Game -- --level 1 --lives 10 --spinner-sensitivity 1
dotnet run --project src\Tempest.Game -- --level1-four-flipper-cap-test --lives 10 --spinner-sensitivity 1
```

Compare maximum simultaneous Flippers; whether a fifth appears while four are
active; Egg/hatch spacing; simultaneous climbers and rim pursuers; overlapping
enemy bullets and recovery time; directional threat convergence and target
manageability; stationary-defense difficulty; scoring and wave length; whether
waiting Eggs return as slots open; normal completion, death/re-entry, and
post-Zapper pressure. Judge the timing and density of threats, not only death
count. Manual acceptance remains **MANUAL ACCEPTANCE PENDING**. Do not promote
the cap into production based on automated results.

## Limitations

The cabinet difficulty setting remains unverified. The accepted Egg schedule
is intentionally retained, so this is not ROM-accurate pending timing. No
six-total-active gate or Levels 2+ cap is included. Recording/playback is not
supported for this scenario. Neal's manual A/B comparison is required before
any conclusion about arcade fidelity or production difficulty.
