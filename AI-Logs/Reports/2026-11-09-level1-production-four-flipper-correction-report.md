# Production Level 1 Four-Flipper Maximum

Date: 2026-11-09
Disposition: **CURRENT PRODUCTION CANDIDATE — AUTOMATED VALIDATION PASSED — FINAL PRODUCTION MANUAL ACCEPTANCE PENDING**

## Starting state and evidence

- `HEAD` remained `c1a974a625eea2dfaecf30d1b8d729e022ef17e8`
  (`Close out accepted spinner sensitivity diagnostic`).
- The existing unstaged four-Flipper diagnostic and enemy-bullet visibility
  correction were preserved and converted; they were not restarted from the
  committed baseline. The index was empty. Existing untracked
  `Reference/arcade/roms.rev2/` files were left untouched.
- Read the low-level comparative investigation, diagnostic implementation,
  and enemy-bullet visibility correction reports. Those historical reports
  were not edited.
- The production request was archived at
  `AI-Logs/Prompts/2026-11-09-level1-production-four-flipper-correction-prompt.md`.
- Neal's corrected diagnostic A/B evidence: enemy bullets were visible; fewer
  Flippers occupied the tube; capped Level 1 felt much better and the same as
  or close to the original arcade Level 1. This is **MANUALLY ARCADE-FIDELITY
  SUPPORTED** for the cap policy. Final manual acceptance of the ordinary
  production launch is still pending.

## Production policy

**ROM-LISTING-BACKED / REV-2-BINARY-CONFIRMED:** Level 1's original supply is
10 Flippers, with a maximum of four active concurrently. The separate
six-total-active ROM creation gate is not implemented.

**PROJECT TRANSLATION:** The canonical production Level 1 configuration
enforces the cap in the existing hatch path. It counts the authoritative
active Flipper collection: a Flipper begins counting when the normal hatch
path adds it and stops counting only when the existing lifecycle removes it.
Pending delayed Super Zapper resolution does not remove a Flipper early.

At four active Flippers, a ready Egg remains in its existing `ReadyToHatch`
state and unresolved. It is not discarded, returned to reserve, counted as
destroyed/rim-cleared, retargeted, or repeatedly rerolled. Capacity is checked
before calling the hatch factory, so a blocked Egg consumes no hatch
randomness. Once a slot opens, the normal hatch path resumes. This C# waiting
mechanism is not the ROM's literal pending-entry structure or timing.

The rule applies to normal no-option campaign startup, generic `--level 1`,
fresh campaign/Game Over restart, campaign restart, Level 1 death/re-entry,
recording/playback startup, and headless production simulation. It is scoped
to production progression paths using the canonical Level 1 configuration;
development-only scenarios remain separate. No parallel cap counter or queue
was added.

Supply remains 10: four starting Eggs and six reserve originals. Existing
50-tick reserve release, approach selection cadence/limits, Egg movement,
ready timing, and deterministic lane/direction choices are unchanged. The cap
does not gate release and does not add the six-total-active rule.

Accepted rim speed 2.5, movement, rim pause/targeting/pivot, player/enemy
firing, projectile capacities/timing, bullet simulation, collisions, scoring,
lives, spinner sensitivity, `--speed`, geometry, audio, and Super Zapper rules
are unchanged. Death reconstruction retains unresolved obligations and the
production rule. Zapper targets remain active until their normal delayed
resolution/removal.

Level 1 bullet rendering uses actual supported active bullet state, preserving
the existing white 8-spoke geometry, size, rotation, projection, and shared
simulation/collision identity. The separate `--enemy-bullets-test` is not
required. No unsupported bullet simulation was introduced.

Normal production completion still waits for every obligation, including
ready Eggs, to resolve. Production recording schema is unchanged; visible and
headless playback restore ordinary production startup and use the cap. Pause,
forward step, reverse reconstruction, and input handoff retain their existing
implementation. Historical recordings replay under the new simulation;
byte-for-byte trajectory compatibility is not claimed.

Level 2 remains unchanged and is deferred for its own analysis and manual
acceptance. Levels 3-8 remain uncapped by this Level 1 rule. Level 9 and the
six-active gate are out of scope.

## Diagnostic option and test conversion

Removed `--level1-four-flipper-cap-test`, its simulation identity, parser and
conflict wiring, terminal restart behavior, and diagnostic-only recording
restrictions. The old token is rejected with a clear message explaining that
production Level 1 now applies the rule. Removed its README command and
diagnostic documentation.

Converted cap/accounting, waiting, determinism, re-entry, Zapper,
completion/progression, and other-level coverage to
`Level1ProductionFlipperCapTests`. Converted renderer checks to
`Level1ProductionEnemyBulletRenderingTests`. Updated Level 3 renderer
expectations to the active-bullet visibility predicate. Preserved Level 4
bullet regression at the moment a bullet is actually active.

Production tests cover default/direct starts, ten-unit supply, four starting
Eggs, maximum concurrency, waiting persistence, normal hatch after slot
release, accounting, repeatability, death/re-entry, Super Zapper,
completion/progression, and Levels 2-8 isolation. Existing firing, playback,
recording, pause/step/reverse, speed, spinner, screenshot, and campaign
coverage remains active.

## Stall investigation and correction

The earlier unbounded full-suite run had been left stalled and was stopped.
The first bounded class isolation was:

```powershell
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --filter "FullyQualifiedName~SimulationTests" --blame-hang-timeout 45s --logger "console;verbosity=detailed"
```

VSTest's blame timeout identified the exact running test:
`SimulationTests.HurryUpIsNotEnabledForNormalSpikerOrTankerScenarios`.
Running that method alone with
`--blame-hang-timeout 10s` reproduced the stall. The first theory row was the
normal production case (`spiker: False, tanker: False`).

**Root cause:** the test helper looped until reserve and Egg collections were
empty, assuming all production Level 1 Eggs would hatch in succession. With
four active Flippers, a ready fifth Egg correctly waits. The test did not
advance a slot by resolving enemies and had no loop bound; once gameplay stopped
with unresolved units, the loop condition could never become false. This was
a stale test assumption plus an unbounded test loop, not a production
progression deadlock.

The test now stops at the expected production cap/wait boundary, asserts the
waiting Egg and ten-unit conservation, and uses an explicit tick bound with a
useful failure message for other scenarios. The Level 1-to-2 fixture helper
also uses explicit bounded release/hatch batches. Separate production
completion testing resolves all ten obligations and observes natural Level
1-to-2 progression, demonstrating that waiting Eggs can hatch and the wave
does not deadlock.

Timeout-generated dump/sequence files were removed after diagnosis. No test
parallelization or repository runsettings changes were made.

## Files changed

- `src/Tempest.Game/TempestGame.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `tests/Tempest.Game.Tests/Level1ProductionFlipperCapTests.cs`
- `tests/Tempest.Game.Tests/Level1ProductionEnemyBulletRenderingTests.cs`
- `tests/Tempest.Game.Tests/SimulationTests.cs`
- `tests/Tempest.Game.Tests/LevelProgressionTests.cs`
- `tests/Tempest.Game.Tests/Level3TestModeTests.cs`
- `tests/Tempest.Game.Tests/Level4CandidateTests.cs`
- `README.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `TEMPEST-BEHAVIOR-SPEC.md`

## Validation

All diagnostic test commands used VSTest hang timeouts.

| Selection / command | Result |
|---|---|
| Isolated production theory row (`spiker: False, tanker: False`), 30-second timeout | 1 passed |
| `SimulationTests`, 60-second timeout | 315 passed |
| Focused cap/startup/rendering/progression/firing/campaign/recording/playback regressions, 60-second timeout | 221 passed |
| `LevelProgressionTests`, 30-second timeout after bounded fixture change | 30 passed |
| `dotnet test TempestCSharp.slnx --blame-hang-timeout 60s` | 1,118 passed, 0 failed, 0 skipped |
| `dotnet build TempestCSharp.slnx` | Passed, 0 warnings/errors |
| `git diff --check` | Passed; only existing CRLF normalization warnings were printed |
| Removed diagnostic CLI option | Exits 2 before window initialization, with explanatory error |

No Windows Application Control block occurred. The interactive game was not
launched.

## Git, protected files, and manual acceptance

No files were staged, committed, or pushed. `HEAD` is unchanged. The
pre-existing untracked ROM directory remains untouched; no writes were made to
`UserData/`, screenshots, recordings, `Videos/`, or `TEMP/`. Historical reports
remain unchanged. The implementation report and prompt are ignored under
`AI-Logs/`.

Neal's final production acceptance is **FINAL PRODUCTION MANUAL ACCEPTANCE
PENDING**. Use these commands:

```powershell
dotnet run --project src\Tempest.Game -- --lives 10 --spinner-sensitivity 1
dotnet run --project src\Tempest.Game -- --level 1 --lives 10 --spinner-sensitivity 1
dotnet run --project src\Tempest.Game -- --record-gameplay Temp\GameplayRecordings\level1-four-flipper-production.json --level 1 --lives 10 --spinner-sensitivity 1
dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\level1-four-flipper-production.json
```

Please verify normal production Level 1 threat density and spacing; no more
than four active Flippers; waiting Eggs later hatching; all ten obligations
resolving; visible, moving, audible, colliding enemy bullets; unchanged rim
pursuit, spinner, firing, Super Zapper and death/re-entry; successful Level
completion and natural Level 1-to-2 progression; recording/playback pause,
forward/reverse, and input handoff; and no obvious Levels 2-8 regression.
Judge arcade feel and threat timing rather than an exact death count.

Detailed acceptance checklist:

1. Normal no-option Level 1 has arcade-like threat density.
2. No more than four Flippers are active simultaneously.
3. Additional Eggs wait and later hatch as slots open.
4. All ten enemies eventually appear and resolve.
5. The wave does not feel unnaturally short or sparse.
6. Enemy bullets remain visible.
7. Enemy bullets move, sound, and collide normally.
8. The Blaster is not destroyed by invisible threats.
9. Rim pursuit remains threatening.
10. Stationary defense has not become trivially safe.
11. The game retains useful breathing room and target manageability.
12. Spinner control remains correct.
13. Player firing and Super Zapper remain normal.
14. Death/re-entry remains coherent.
15. Level 1 completes normally.
16. Natural Level 1-to-2 progression works.
17. Level 2 remains behaviorally unchanged.
18. Fresh recording/playback reproduces the production run.
19. Pause/resume/forward/reverse remain correct.
20. No obvious Levels 2-8 regression appears.

This production candidate does not claim that all Level 1 timing matches the
ROM. The C# Egg waiting policy remains a project translation. Level 2
concurrency and the ROM six-total-active gate remain deferred.
