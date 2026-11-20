# Production Level 2 Four-Flipper Maximum

Date: 2026-11-09
Disposition: **CURRENT PRODUCTION CANDIDATE — AUTOMATED VALIDATION PASSED — FINAL PRODUCTION MANUAL ACCEPTANCE PENDING**

## Starting state, evidence, and scope

- Starting branch: `master`; `HEAD` remained
  `f362af7f87c23a9b983937f8733f5a4ac71d0162`
  (`Implement accepted Level 1 four-Flipper maximum`).
- The inherited Level 2 diagnostic candidate was unstaged. It modified the
  README, behavior/development notes, simulation, CLI, renderer, and added
  `Level2FourFlipperCapTests`. It was promoted in place rather than restarted
  from the baseline.
- The index was empty. The pre-existing untracked
  `Reference/arcade/roms.rev2/` directory was protected and left untouched.
- Read in full without rewriting:
  - `2026-11-09-level2-concurrency-investigation-report.md`
  - `2026-11-09-level2-four-flipper-cap-diagnostic-report.md`
  - `2026-11-09-level2-bottom-artifact-investigation-report.md`
  - `2026-11-09-level1-production-four-flipper-correction-report.md`
  - `2026-11-09-level1-four-flipper-production-closeout-report.md`
- Neal's manual Level 2 A/B comparison strongly accepted the four-Flipper
  diagnostic: fewer simultaneous Flippers, pivots, converging threats and
  grouped rim arrivals; useful breathing room; continued dangerous rim
  pursuit and normal visible enemy bullets; and a substantial 12-obligation
  wave. This supports the diagnostic's arcade-like threat density, not final
  production acceptance.
- The bottom-artifact investigation established that the tiny far/deep mark is
  the real cap-blocked `ReadyToHatch` Egg at height zero:
  **EXPECTED/HARMLESS PROJECT PRESENTATION — NO FIX**. It is not a Flipper,
  projectile, collision object, duplicate, or accounting defect. Do not hide,
  move, delete, or prematurely hatch it. Revisit only if cap-blocked Eggs
  become a recurring presentation across future production levels
  (**DEFERRED**).
- The complete supplied prompt was archived at
  `AI-Logs/Prompts/2026-11-09-level2-production-four-flipper-promotion-prompt.md`.
  Its SHA-256 matches the attachment:
  `D91548E748F917A2DCD7A720A9B8302E3BE55C1817456F45DC07847EB8033376`.
- No existing report was edited. No Level 3 concurrency, ROM global gate, or
  Level 9 work was started.

## Production policy and implementation

**ROM-LISTING-BACKED / REV-2-BINARY-CONFIRMED:** Production Level 2 supplies
12 original Flippers and permits at most four concurrently active across
Easy, Medium, and Hard. The ROM `max_enm` table value six uses an inclusive
comparison, is not a strict six-object ceiling, and does not independently
constrain Flipper-only Level 2. That global mechanism remains unimplemented.

**PROJECT TRANSLATION:** The existing hatch phase now shares one narrowly
scoped production predicate and a four-Flipper limit for canonical production
Levels 1 and 2. It counts the authoritative active Flipper collection. A
Flipper consumes a slot from normal hatch until existing lifecycle removal,
including climbing, pausing, pivoting, rim pursuit, and pending delayed Zapper
resolution. At capacity, a ready Egg stays unresolved in `ReadyToHatch`;
it is not discarded, destroyed, returned to reserve, rim-cleared, scored,
replaced, or rerolled. The gate runs before the hatch factory, so blocked
waiting consumes no hatch randomness and cannot transiently create a fifth
Flipper. Once an existing lifecycle opens capacity, normal hatch ordering
resumes. This is not a claim that the ROM uses the same pending-entry data
structure or timing.

The policy applies to natural Level 1-to-2 progression, generic `--level 2`,
normal death/re-entry, production recording startup, visible and headless
playback, and deterministic production simulation. The production-only gate
matches canonical configuration identity and natural progression; it does
not add a global or generalized species-cap framework.

Unchanged Level 2 supply and schedule: 12 originals; four starting Eggs;
eight reserve originals; 50-active-tick reserve release; maximum Egg queue
four; maximum approaching/ready Eggs two; 50-active-tick approach-selection
interval; existing Egg cadence, readiness and deterministic lane sequence.
Reserve release is not suppressed while Eggs wait.

Unchanged canflip direction/destination latching, endpoint-hinge geometry,
pivots, lane pause, climb, collisions, shot ownership, pivot firing
suppression, shot-delay/randomness semantics, firing thresholds, two-shot
capacity, 77-tick C# holdoff, bullet geometry/visibility/simulation/collision,
2.5 rim-speed calibration, rim pursuit, Super Zapper charges/scoring/targeting,
death reconstruction, accounting, completion, and Level 2-to-3 progression.
No recording metadata or schema/version change was made. Level 1's accepted
supply, cap and waiting behavior are unchanged. Levels 3-8 do not inherit the
Level 2 policy.

The temporary `--level2-four-flipper-cap-test` scenario, parser/conflict
wiring, terminal replay path, renderer label, headless/recording rejection,
and README command were removed. The old option now exits with code 2 and
explains that production Level 2 applies the limit automatically. No obsolete
scenario identity or recording restriction remains.

## Tests and validation

`Level2FourFlipperCapTests` was converted from development-diagnostic
authority to production Level 2 fidelity coverage. It now covers canonical
direct, natural-campaign, and recording startup; supply, queue and approach
configuration; deterministic cap waiting; Egg persistence at height zero;
Super Zapper slot retention/removal/refill; production bullet firing,
rendering and collision; repeated death/re-entry; all 12 obligations
resolving and natural Level 3 progression; and recording, visible/headless playback, pause/resume, forward stepping and
reverse reconstruction. The obsolete CLI
composition and terminal diagnostic replay/rejection tests were removed.
The existing Level 2-through-8 regression in
`Level1ProductionFlipperCapTests` was updated because its prior uncapped-Level-2
assertion became stale; its Level 1 checks remain unchanged. Every new
simulation wait has a tick bound and state-bearing failure message.

| Validation | Result |
|---|---|
| Focused cap/startup regressions (`Level2FourFlipperCapTests`, `Level1ProductionFlipperCapTests`, `DirectLevelStartupTests`) | 28 passed |
| Expanded affected gameplay/playback selection | 773 passed |
| `SimulationTests` with `--blame-hang-timeout 60s` | 315 passed |
| `dotnet test TempestCSharp.slnx --blame-hang-timeout 60s` | 1,126 passed; 0 failed; 0 skipped |
| `dotnet build TempestCSharp.slnx` | Passed; 0 warnings; 0 errors |
| `git diff --check` | Passed |
| Removed `--level2-four-flipper-cap-test` CLI option | Rejected before window initialization; exit code 2 |

The 773-test expanded selection preceded the final natural-entry and visible
playback-controller assertions. The 28-test focused selection ran after the
natural-entry assertion; the updated Level 2 class (8 tests) and final full
solution suite ran after both refinements and passed. No Windows Application
Control or Smart App Control block occurred. No security setting was changed.
No test timed out, stalled, hung, or was canceled; VSTest produced no dump or
sequence file. No interactive game was launched.

## Files and repository state

Changed tracked files:

- `README.md`
- `TEMPEST-BEHAVIOR-SPEC.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/TempestGame.cs`
- `tests/Tempest.Game.Tests/Level1ProductionFlipperCapTests.cs`
- `tests/Tempest.Game.Tests/Level2FourFlipperCapTests.cs`

The behavior and development references now distinguish the ROM-backed value
from the C# waiting-Egg translation, record the strong diagnostic A/B result,
classify the deep-end Egg presentation, note the deferred future-level
presentation concern, and state that final production manual acceptance is
pending.

Protected ROM data, `UserData/`, screenshots, gameplay recordings, `Videos/`,
`TEMP/`, `Temp/`, historical reports, test dumps/sequences, and build output
were not manually modified, staged, moved, or deleted. `AI-Logs/` is
Git-ignored. Nothing was staged, committed, or pushed. The working candidate
remains unstaged on the original baseline.

## Manual handoff

Neal's **FINAL PRODUCTION MANUAL ACCEPTANCE PENDING**. Use these commands:

```powershell
dotnet run --project src\Tempest.Game -- --lives 10 --spinner-sensitivity 1
dotnet run --project src\Tempest.Game -- --level 2 --lives 10 --spinner-sensitivity 1
dotnet run --project src\Tempest.Game -- --record-gameplay Temp\GameplayRecordings\level2-four-flipper-production.json --level 2 --lives 10 --spinner-sensitivity 1
dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\level2-four-flipper-production.json
```

Also available as the normal production launch command:

```powershell
dotnet run --project src\Tempest.Game -- --lives 10
```

Please verify normal production Level 2 now has the accepted diagnostic's
improved threat density; no more than four simultaneous Flippers; fewer
simultaneous pivots and grouped rim arrivals; useful breathing room without
trivial stationary defense or an artificially sparse wave; dangerous rim
pursuit and visible normal enemy bullets; unchanged canflip and Egg timing;
waiting Eggs later hatch and all 12 obligations resolve; normal Super Zapper
and death/re-entry; successful Level 2 completion and Level 3 progression;
unchanged Level 1 and Levels 3-8 behavior; and fresh recording/playback with
pause/resume, forward/reverse, screenshots and live-input handoff. The tiny
deep-end waiting-Egg mark may remain and is currently accepted as harmless.
Judge threat timing, spacing, canflip density, rim convergence, control and
arcade resemblance rather than exact death count.

## Remaining uncertainty and deferred work

The production C# waiting-Egg representation and timing are project
translations, not literal ROM implementation. The exact relationship between
ROM timer units and the fixed 60 Hz simulation, and the cabinet's difficulty
setting, remain unproven. Final normal-production Level 2 manual acceptance
is pending. Revisit deep-end waiting-Egg presentation only if it recurs broadly
on future production levels. Level 3-8 concurrency corrections, a generalized
species-cap system, the ROM global `max_enm` gate, and Level 9 remain deferred.
